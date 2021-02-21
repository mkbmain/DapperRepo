using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DapperRepo.Repo
{
    public abstract class SqlRepoBase 
    {
        protected readonly string ConnectionString;

        public SqlRepoBase(string connectionString)
        {
            ConnectionString = connectionString;
        }
        

        protected Task<T> BaseGet<T>(Guid id, Func<SqlConnection, string, Task<T>> func)
        {
            string sqlQuery = $"select * from {typeof(T).Name} where Id = '{id}'";
            return func.Invoke(new SqlConnection(ConnectionString), sqlQuery);
        }

        protected Task<IEnumerable<T>> BaseGetAll<T>(Func<SqlConnection, string, Task<IEnumerable<T>>> func)
        {
            return func.Invoke(new SqlConnection(ConnectionString), $"select * from {typeof(T).Name}");
        }

        // we need to return here to ensure if its async it completes the task hence we use func not action
        protected Task BaseAddMany<T>(Func<SqlConnection, string, Task> action)
        {
            var properties = typeof(T).GetProperties().Select(f => f.Name).ToArray();
            var sql = $"insert into {typeof(T).Name} ( {string.Join(",", properties)}) values ({string.Join(",", properties.Select(t => $"@{t}"))})";
            return action.Invoke(new SqlConnection(ConnectionString), sql);
        }

 

        protected Task BaseUpdate<T>(T element, bool ignoreNullProperties, Func<SqlConnection, string, Task> func)
        {
            var entityPropertyInfo = ReflectionUtils.GetBaseEntityProperyInfo(element);
            var updates = entityPropertyInfo.AllNonId
                .Where(f => ignoreNullProperties ? typeof(T).GetProperty(f.Name)?.GetValue(element, null) != null : true)
                .Select(f => $"{f.Name} = @{f.Name}");

            var sql = $"update  {typeof(T).Name} set  {string.Join(",", updates)} where {entityPropertyInfo.Id.Name} = @{entityPropertyInfo.Id.Name}";
            return func.Invoke(new SqlConnection(ConnectionString), sql);
        }

        protected Task BaseDelete<T>(T element, Func<SqlConnection,string,Task> func)
        {
            var entityPropertyInfo = ReflectionUtils.GetBaseEntityProperyInfo(element);
            var delete = $"delete from {typeof(T).Name} where {entityPropertyInfo.Id.Name} = @{entityPropertyInfo.Id.Name}";
            return func.Invoke(new SqlConnection(ConnectionString), delete);
        }
        
    }
}