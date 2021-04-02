using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DapperRepo.Repo
{
    public abstract class SqlRepoBase
    {
        private readonly string _connectionString;

        private static string WhereClause(EntityPropertyInfo entityPropertyInfo) =>
            $"where {entityPropertyInfo.Id.Name} = @{entityPropertyInfo.Id.Name}";

        public SqlRepoBase(string connectionString)
        {
            _connectionString = connectionString;
        }
        internal TOut BaseGet<T, TOut>(Func<SqlConnection, string, TOut> func)
        {
            return BaseGetAll<T, TOut>((connection, s) =>
                func(connection, $"{s} {WhereClause(ReflectionUtils.GetBaseEntityProperyInfo<T>())}"));
        }

        internal TOut BaseGetAll<T, TOut>(Func<SqlConnection, string, TOut> func)
        {
            return BaseGetAll(func, $"select * from {typeof(T).Name}");
        }

        internal TOut BaseGetAll<TOut>(Func<SqlConnection, string, TOut> func, string sql)
        {
            return func.Invoke(new SqlConnection(_connectionString), sql);
        }
        
        internal Tout Search<T, Tout>(Func<SqlConnection, string, Tout> func,string property) 
        {
            return BaseGetAll<T, Tout>((connection, sql2) => func(connection,($"{sql2} where {property} like  @{property}")));
        }

        internal TOut BaseAdd<T, TOut>(IEnumerable<T> elements, Func<SqlConnection, string, TOut> action,
            bool withOutput)
        {
            var propertyInfo = ReflectionUtils.GetBaseEntityProperyInfo<T>();
            var properties = propertyInfo.All
                .Where(f => elements.Any(e => typeof(T).GetProperty(f.Name)?.GetValue(e, null) != null))
                .Select(f => f.Name)
                .ToArray();
            var output = withOutput ? $"output inserted.*" : "";
            var sql =
                $"insert into {typeof(T).Name} ({string.Join(",", properties)})  {output} values ({string.Join(",", properties.Select(t => $"@{t}"))})";

            return action.Invoke(new SqlConnection(_connectionString), sql);
        }

        // we need to return here to ensure if its async it completes the task hence we use func not action
        internal Task BaseUpdate<T>(T element, bool ignoreNullProperties, Func<SqlConnection, string, Task> func)
        {
            var entityPropertyInfo = ReflectionUtils.GetBaseEntityProperyInfo<T>();
            var updates = entityPropertyInfo.AllNonId
                .Where(f => !ignoreNullProperties || typeof(T).GetProperty(f.Name)?.GetValue(element, null) != null)
                .Select(f => $"{f.Name} = @{f.Name}");

            var sql = $"update  {typeof(T).Name} set  {string.Join(",", updates)} {WhereClause(entityPropertyInfo)}";
            return func.Invoke(new SqlConnection(_connectionString), sql);
        }

        internal Task BaseDelete<T>(Func<SqlConnection, string, Task> func)
        {
            var delete = $"delete from {typeof(T).Name} {WhereClause(ReflectionUtils.GetBaseEntityProperyInfo<T>())}";
            return func.Invoke(new SqlConnection(_connectionString), delete);
        }
    }
}