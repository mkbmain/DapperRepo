using System;
using System.Collections.Generic;
using System.Data.Common;
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
        private Func<DbConnection> Connection;

        public SqlRepoBase(Func<DbConnection> connection)
        {
            Connection = connection;
        }

        protected TOut BaseGet<T, TOut>(Func<DbConnection, string, TOut> func)
        {
            return BaseGetAll<T, TOut>((connection, s) =>
                func(connection, $"{s} {PrimaryKeyWhereClause(ReflectionUtils.GetEntityPropertyInfo<T>())}"));
        }


        protected TOut BaseGetAll<T, TOut>(Func<DbConnection, string, TOut> func)
        {
            return BaseGetAll(func, $"select * from {GetTableNameFromType(typeof(T))}");
        }

        protected TOut BaseGetAll<TOut>(Func<DbConnection, string, TOut> func, string sql)
        {
            return func.Invoke(Connection(), sql);
        }
        
        protected Tout BaseGetExactMatches<T, Tout>(T element, Func<DbConnection, string, Tout> func, bool ignoreNulls )
        {
            var entityPropertyInfo = ReflectionUtils.GetEntityPropertyInfo<T>();
            var wheres = entityPropertyInfo.AllNonId
                .Where(r=> !ignoreNulls || (typeof(T).GetProperty(r.Name)?.GetValue(element, null) != null))
                .Select(f => $"{entityPropertyInfo.ClassPropertyColNamesDetails[f.Name].SqlPropertyName} {(typeof(T).GetProperty(f.Name)?.GetValue(element, null) == null ? "IS NULL" : $"= @{f.Name}")}")
                .ToArray();

            var test = $" where {String.Join(" and ", wheres)}";

            return BaseGetAll<T, Tout>((connection, s) =>
                func(connection,$"{s}{test}" ));
        }

        protected Tout BaseSearch<T, Tout>(Func<DbConnection, string, Tout> func,
            IEnumerable<SearchCriteria> searchCriteria)
        {
            var searches = string.Join(" And ",
                searchCriteria.Select(e =>
                    $"{e.PropertyName} {SearchCriteriaHelper.SearchTypeToQuery(e.SearchType)}  @{e.PropertyName}"));
            return BaseGetAll<T, Tout>((connection, sql2) => func(connection, ($"{sql2} where  {searches} ")));
        }

        protected Task BaseAdd<T>(IEnumerable<T> elements, Func<DbConnection, string, Task> action)
        {
            var propertyInfo = ReflectionUtils.GetEntityPropertyInfo<T>();
            var properties = propertyInfo.All
                .Where(f => elements.Any(e => typeof(T).GetProperty(f.Name)?.GetValue(e, null) != null))
                .Select(f => f.Name)
                .ToArray();

            var sqlNames = propertyInfo.ClassPropertyColNamesDetails.Select(e=> e.Value)
                .Where(e=> properties.Contains(e.ClassPropertyName) )
                .Select(e => e.SqlPropertyName);
            var sql =
                $"insert into {GetTableNameFromType(typeof(T))} ({string.Join(",", sqlNames)}) values ({string.Join(",", properties.Select(t => $"@{t}"))})";

            return action.Invoke(Connection(), sql);
        }

        // we need to return here to ensure if its async it completes the task hence we use func not action
        protected Task BaseUpdate<T>(T element, bool ignoreNullProperties, Func<DbConnection, string, Task> func)
        {
            var entityPropertyInfo = ReflectionUtils.GetEntityPropertyInfo<T>();
            var updates = entityPropertyInfo.AllNonId
                .Where(f => !ignoreNullProperties || typeof(T).GetProperty(f.Name)?.GetValue(element, null) != null)
                .Select(f => $"{entityPropertyInfo.ClassPropertyColNamesDetails[f.Name].SqlPropertyName} = @{f.Name}");

            var sql =
                $"update  {GetTableNameFromType(typeof(T))} set  {string.Join(",", updates)} {PrimaryKeyWhereClause(entityPropertyInfo)}";
            return func.Invoke(Connection(), sql);
        }

        protected Task BaseDelete<T>(Func<DbConnection, string, Task> func)
        {
            var delete =
                $"delete from {GetTableNameFromType(typeof(T))} {PrimaryKeyWhereClause(ReflectionUtils.GetEntityPropertyInfo<T>())}";
            return func.Invoke(Connection(), delete);
        }

        protected static T SetFieldOf<T, PropT>(T item, string property, object valueToSearchBy)
        {
            var theField = ReflectionUtils.GetPropertyInfoOfType<T>(typeof(PropT), property);
            theField.SetValue(item, valueToSearchBy);
            return item;
        }
        
        private static string PrimaryKeyWhereClause(EntityPropertyInfo entityPropertyInfo) =>
            $"where {entityPropertyInfo.IdColNameDetails.SqlPropertyName} = @{entityPropertyInfo.Id.Name}";

        private static string GetTableNameFromType(MemberInfo type)
        {
            var attribute = type.GetCustomAttribute(typeof(SqlTableNameAttribute), false);
            return attribute != null ? ((SqlTableNameAttribute) attribute).Name : type.Name;
        }
    }
}