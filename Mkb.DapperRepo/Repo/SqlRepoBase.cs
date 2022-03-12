using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Mkb.DapperRepo.Attributes;
using Mkb.DapperRepo.Reflection;
using Mkb.DapperRepo.Search;

namespace Mkb.DapperRepo.Repo
{
    public abstract class SqlRepoBase
    {
        private readonly string _connectionString;

        private static string PrimaryKeyWhereClause(EntityPropertyInfo entityPropertyInfo) =>
            $"where {entityPropertyInfo.Id.Name} = @{entityPropertyInfo.Id.Name}";

        private static string GetTableNameFromType(MemberInfo type)
        {
            var attribute = type.GetCustomAttribute(typeof(SqlTableNameAttribute), false);
            return attribute != null ? ((SqlTableNameAttribute) attribute).Name : type.Name;
        }

        public SqlRepoBase(string connectionString)
        {
            _connectionString = connectionString;
        }

        internal TOut BaseGet<T, TOut>(Func<SqlConnection, string, TOut> func)
        {
            return BaseGetAll<T, TOut>((connection, s) =>
                func(connection, $"{s} {PrimaryKeyWhereClause(ReflectionUtils.GetEntityPropertyInfo<T>())}"));
        }
        
        internal TOut BaseGetAll<T, TOut>(Func<SqlConnection, string, TOut> func)
        {
            return BaseGetAll(func, $"select * from {GetTableNameFromType(typeof(T))}");
        }

        internal TOut BaseGetAll<TOut>(Func<SqlConnection, string, TOut> func, string sql)
        {
            return func.Invoke(new SqlConnection(_connectionString), sql);
        }

        internal Tout BaseSearch<T, Tout>(Func<SqlConnection, string, Tout> func,
            IEnumerable<SearchCriteria> searchCriteria)
        {
            var searches = string.Join(" And ",
                searchCriteria.Select(e =>
                    $"{e.PropertyName} {SearchCriteriaHelper.SearchTypeToQuery(e.SearchType)}  @{e.PropertyName}"));
            return BaseGetAll<T, Tout>((connection, sql2) => func(connection, ($"{sql2} where  {searches} ")));
        }

        internal TOut BaseAdd<T, TOut>(IEnumerable<T> elements, Func<SqlConnection, string, TOut> action,
            bool withOutput)
        {
            var propertyInfo = ReflectionUtils.GetEntityPropertyInfo<T>();
            var properties = propertyInfo.All
                .Where(f => elements.Any(e => typeof(T).GetProperty(f.Name)?.GetValue(e, null) != null))
                .Select(f => f.Name)
                .ToArray();
            var output = withOutput ? $"output inserted.*" : "";
            var sql =
                $"insert into {GetTableNameFromType(typeof(T))} ({string.Join(",", properties)})  {output} values ({string.Join(",", properties.Select(t => $"@{t}"))})";

            return action.Invoke(new SqlConnection(_connectionString), sql);
        }

        // we need to return here to ensure if its async it completes the task hence we use func not action
        internal Task BaseUpdate<T>(T element, bool ignoreNullProperties, Func<SqlConnection, string, Task> func)
        {
            var entityPropertyInfo = ReflectionUtils.GetEntityPropertyInfo<T>();
            var updates = entityPropertyInfo.AllNonId
                .Where(f => !ignoreNullProperties || typeof(T).GetProperty(f.Name)?.GetValue(element, null) != null)
                .Select(f => $"{f.Name} = @{f.Name}");

            var sql =
                $"update  {GetTableNameFromType(typeof(T))} set  {string.Join(",", updates)} {PrimaryKeyWhereClause(entityPropertyInfo)}";
            return func.Invoke(new SqlConnection(_connectionString), sql);
        }

        internal Task BaseDelete<T>(Func<SqlConnection, string, Task> func)
        {
            var delete =
                $"delete from {GetTableNameFromType(typeof(T))} {PrimaryKeyWhereClause(ReflectionUtils.GetEntityPropertyInfo<T>())}";
            return func.Invoke(new SqlConnection(_connectionString), delete);
        }

        protected static T SetFieldOf<T, PropT>(T item, string property, object valueToSearchBy)
        {
            var theField = ReflectionUtils.GetPropertyInfoOfType<T>(typeof(PropT), property);
            theField.SetValue(item, valueToSearchBy);
            return item;
        }
    }
}