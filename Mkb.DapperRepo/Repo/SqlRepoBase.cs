using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Mkb.DapperRepo.Attributes;
using Mkb.DapperRepo.Exceptions;
using Mkb.DapperRepo.Reflection;
using Mkb.DapperRepo.Search;

namespace Mkb.DapperRepo.Repo
{
    public abstract class SqlRepoBase
    {
        private readonly Func<DbConnection> _connection;

        public SqlRepoBase(Func<DbConnection> connection)
        {
            _connection = connection;
        }

        protected TOut BaseGet<T, TOut>(Func<DbConnection, string, TOut> func)
        {
            return BaseGetAll<T, TOut>((connection, s) =>
                func(connection, $"{s} {PrimaryKeyWhereClause<T>(ReflectionUtils.GetEntityPropertyInfo<T>())}"));
        }

        protected TOut BaseCount<T, TOut>(Func<DbConnection, string, TOut> func)
        {
            return BaseGetAll<T, TOut>(func, $"select COUNT(*) from {GetTableNameFromType(typeof(T))}");
        }

        protected TOut BaseGetAll<T, TOut>(Func<DbConnection, string, TOut> func)
        {
            return BaseGetAll<T, TOut>(func, $"select * from {GetTableNameFromType(typeof(T))}");
        }

        protected TOut BaseGetAll<T, TOut>(Func<DbConnection, string, TOut> func, string sql)
        {
            Mappers.TableMapper.Setup<T>();
            return func.Invoke(_connection(), sql);
        }

        protected TOut BaseGetExactMatches<T, TOut>(T element, Func<DbConnection, string, TOut> func, bool ignoreNulls)
        {
            var entityPropertyInfo = ReflectionUtils.GetEntityPropertyInfo<T>();
            var wheres = entityPropertyInfo.AllNonId
                .Where(r => !ignoreNulls || (typeof(T).GetProperty(r.Name)?.GetValue(element, null) != null))
                .Select(f =>
                    $"{entityPropertyInfo.ClassPropertyColNamesDetails[f.Name].SqlPropertyName} {(typeof(T).GetProperty(f.Name)?.GetValue(element, null) == null ? "IS NULL" : $"= @{f.Name}")}")
                .ToArray();

            var whereClause = $" where {String.Join(" and ", wheres)}";

            return BaseGetAll<T, TOut>((connection, sql) =>
                func(connection, $"{sql}{whereClause}"));
        }

        protected TOut BaseSearchEntity<T, TOut>(Func<DbConnection, string, TOut> func,
            IEnumerable<SearchCriteria> searchCriteria)
        {
            return BaseGetAll<T, TOut>((connection, sql2) => func(connection, BuildWhereString<T>(sql2,searchCriteria)));
        }

        protected TOut BaseSearchCount<T, TOut>(Func<DbConnection, string, TOut> func,
            IEnumerable<SearchCriteria> searchCriteria)
        {
            return BaseCount<T, TOut>((connection, sql2) => func(connection, BuildWhereString<T>(sql2,searchCriteria)));
        }

        protected Task BaseAdd<T>(IEnumerable<T> elements, Func<DbConnection, string, Task> action)
        {
            var propertyInfo = ReflectionUtils.GetEntityPropertyInfo<T>();
            var properties = propertyInfo.All
                .Where(f => elements.Any(e => typeof(T).GetProperty(f.Name)?.GetValue(e, null) != null))
                .Select(f => f.Name)
                .ToArray();

            var sqlNames = propertyInfo.ClassPropertyColNamesDetails.Select(e => e.Value)
                .Where(e => properties.Contains(e.ClassPropertyName))
                .Select(e => e.SqlPropertyName);
            var sql =
                $"insert into {GetTableNameFromType(typeof(T))} ({string.Join(",", sqlNames)}) values ({string.Join(",", properties.Select(t => $"@{t}"))})";

            return action.Invoke(_connection(), sql);
        }


        protected Task BaseUpdate<T>(T element, bool ignoreNullProperties, Func<DbConnection, string, Task> func)
        {
            var entityPropertyInfo = ReflectionUtils.GetEntityPropertyInfo<T>();
            var updates = entityPropertyInfo.AllNonId
                .Where(f => !ignoreNullProperties || typeof(T).GetProperty(f.Name)?.GetValue(element, null) != null)
                .Select(f => $"{entityPropertyInfo.ClassPropertyColNamesDetails[f.Name].SqlPropertyName} = @{f.Name}");

            var sql =
                $"update {GetTableNameFromType(typeof(T))} set  {string.Join(",", updates)} {PrimaryKeyWhereClause<T>(entityPropertyInfo)}";

            return BaseExecute<T>(sql, func);
        }

        protected Task BaseExecute<T>(string sql, Func<DbConnection, string, Task> func)
        {
            ReflectionUtils.GetEntityPropertyInfo<T>();
            return BaseExecute(sql, func);
        }

        // we need to return here to ensure if its async it completes the task hence we use func not action
        protected Task BaseExecute(string sql, Func<DbConnection, string, Task> func)
        {
            return func.Invoke(_connection(), sql);
        }

        protected Task BaseDelete<T>(Func<DbConnection, string, Task> func)
        {
            var delete =
                $"delete from {GetTableNameFromType(typeof(T))} {PrimaryKeyWhereClause<T>(ReflectionUtils.GetEntityPropertyInfo<T>())}";
            return func.Invoke(_connection(), delete);
        }

        protected static T SetFieldOf<T, TProp>(T item, string property, object valueToSearchBy)
        {
            var theField = ReflectionUtils.GetPropertyInfoOfType<T>(typeof(TProp), property);
            theField.SetValue(item, valueToSearchBy);
            return item;
        }

        private static string PrimaryKeyWhereClause<T>(EntityPropertyInfo entityPropertyInfo)
        {
            if (entityPropertyInfo.Id is null) throw new PrimaryKeyNotFoundException($"Primary key not found on table:{GetTableNameFromType(typeof(T))}");
            return $"where {entityPropertyInfo.IdColNameDetails.SqlPropertyName} = @{entityPropertyInfo.Id.Name}";
        }

        private static string GetTableNameFromType(MemberInfo type)
        {
            var attribute = type.GetCustomAttribute(typeof(SqlTableNameAttribute), false);
            return attribute != null ? ((SqlTableNameAttribute) attribute).Name : type.Name;
        }
        
        private static string BuildWhereString<T>(string sql, IEnumerable<SearchCriteria> searchCriteria) =>
            $"{sql} where {SearchBuilder<T>(searchCriteria)}";

        private static string SearchBuilder<T>(IEnumerable<SearchCriteria> searchCriteria)
        {
            var reflectionType = ReflectionUtils.GetEntityPropertyInfo<T>();
            return string.Join(" And ",
                searchCriteria.Select(e =>
                    $"{reflectionType.ClassPropertyColNamesDetails[e.PropertyName].SqlPropertyName} {SearchCriteriaHelper.SearchTypeToQuery(e.SearchType)}  {(e.SearchType == SearchType.IsNull ? "" : $"@{e.PropertyName}")}"));
        }
    }
}