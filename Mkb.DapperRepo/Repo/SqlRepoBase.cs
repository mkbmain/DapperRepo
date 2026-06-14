using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using Dapper;
using Mkb.DapperRepo.Attributes;
using Mkb.DapperRepo.Exceptions;
using Mkb.DapperRepo.Reflection;
using Mkb.DapperRepo.Search;

namespace Mkb.DapperRepo.Repo
{
    public abstract class SqlRepoBase
    {
        private readonly Func<DbConnection> _connectionFactory;

        protected SqlRepoBase(Func<DbConnection> connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        protected DbConnection CreateConnection() => _connectionFactory();

        protected static string SelectAllSql<T>() =>
            $"select * from {TableName(typeof(T))}";

        protected static string CountSql<T>() =>
            $"select COUNT(*) from {TableName(typeof(T))}";

        protected static string GetByIdSql<T>()
        {
            var info = ReflectionUtils.GetEntityPropertyInfo<T>();
            return $"{SelectAllSql<T>()} {PrimaryKeyWhereClause<T>(info)}";
        }

        protected static string DeleteSql<T>()
        {
            var info = ReflectionUtils.GetEntityPropertyInfo<T>();
            return $"delete from {TableName(typeof(T))} {PrimaryKeyWhereClause<T>(info)}";
        }

        protected static string BuildInsertSql<T>(T[] elements)
        {
            var propertyInfo = ReflectionUtils.GetEntityPropertyInfo<T>();
            var properties = propertyInfo.All
                .Where(f => elements.Any(e => f.GetValue(e) != null))
                .Select(f => f.Name)
                .ToArray();

            var sqlNames = propertyInfo.ClassPropertyColNamesDetails
                .Select(e => e.Value)
                .Where(e => properties.Contains(e.ClassPropertyName))
                .Select(e => e.SqlPropertyName);

            return $"insert into {TableName(typeof(T))} ({string.Join(",", sqlNames)}) values ({string.Join(",", properties.Select(t => $"@{t}"))})";
        }

        protected static string BuildUpdateSql<T>(T element, bool ignoreNullProperties)
        {
            var info = ReflectionUtils.GetEntityPropertyInfo<T>();
            var updates = info.AllNonId
                .Where(f => !ignoreNullProperties || f.GetValue(element) != null)
                .Select(f => $"{info.ClassPropertyColNamesDetails[f.Name].SqlPropertyName} = @{f.Name}");

            return $"update {TableName(typeof(T))} set {string.Join(",", updates)} {PrimaryKeyWhereClause<T>(info)}";
        }

        protected static string ExactMatchesSql<T>(T element, bool ignoreNulls)
        {
            var info = ReflectionUtils.GetEntityPropertyInfo<T>();
            var wheres = info.AllNonId
                .Where(r => !ignoreNulls || r.GetValue(element) != null)
                .Select(f =>
                    $"{info.ClassPropertyColNamesDetails[f.Name].SqlPropertyName} {(f.GetValue(element) == null ? "IS NULL" : $"= @{f.Name}")}")
                .ToArray();

            return $"{SelectAllSql<T>()} where {string.Join(" and ", wheres)}";
        }

        protected static string SearchSql<T>(IEnumerable<SearchCriteria> searchCriteria) =>
            $"{SelectAllSql<T>()} where {SearchWhereClause<T>(searchCriteria)}";

        protected static string SearchCountSql<T>(IEnumerable<SearchCriteria> searchCriteria) =>
            $"{CountSql<T>()} where {SearchWhereClause<T>(searchCriteria)}";

        protected static (string sql, DynamicParameters parameters) SearchTermsSql<T>(SearchTerm<T>[] terms) =>
            BuildSearchTermsQuery(SelectAllSql<T>(), terms);

        protected static (string sql, DynamicParameters parameters) SearchCountTermsSql<T>(SearchTerm<T>[] terms) =>
            BuildSearchTermsQuery(CountSql<T>(), terms);

        private static (string sql, DynamicParameters parameters) BuildSearchTermsQuery<T>(string baseSql, SearchTerm<T>[] terms)
        {
            var info = ReflectionUtils.GetEntityPropertyInfo<T>();
            var dp = new DynamicParameters();
            var whereParts = new string[terms.Length];

            for (var i = 0; i < terms.Length; i++)
            {
                var term = terms[i];
                var colName = info.ClassPropertyColNamesDetails[term.PropertyName].SqlPropertyName;
                if (term.SearchType == SearchType.IsNull)
                {
                    whereParts[i] = $"{colName} is null";
                }
                else
                {
                    var paramName = $"p{i}";
                    dp.Add(paramName, term.Value);
                    whereParts[i] = $"{colName} {SearchCriteriaHelper.SearchTypeToQuery(term.SearchType)} @{paramName}";
                }
            }

            return ($"{baseSql} where {string.Join(" And ", whereParts)}", dp);
        }

        protected static T SetFieldOf<T, TProp>(T item, string property, object valueToSearchBy)
        {
            var theField = ReflectionUtils.GetPropertyInfoOfType<T>(typeof(TProp), property);
            theField.SetValue(item, valueToSearchBy);
            return item;
        }

        private static string PrimaryKeyWhereClause<T>(EntityPropertyInfo info)
        {
            if (info.Id is null)
                throw new PrimaryKeyNotFoundException($"Primary key not found on table:{TableName(typeof(T))}");
            return $"where {info.IdColNameDetails.SqlPropertyName} = @{info.Id.Name}";
        }

        private static string TableName(MemberInfo type)
        {
            var attribute = type.GetCustomAttribute(typeof(SqlTableNameAttribute), false);
            return attribute != null ? ((SqlTableNameAttribute)attribute).Name : type.Name;
        }

        private static string SearchWhereClause<T>(IEnumerable<SearchCriteria> searchCriteria)
        {
            var info = ReflectionUtils.GetEntityPropertyInfo<T>();
            return string.Join(" And ",
                searchCriteria.Select(e =>
                    $"{info.ClassPropertyColNamesDetails[e.PropertyName].SqlPropertyName} {SearchCriteriaHelper.SearchTypeToQuery(e.SearchType)}  {(e.SearchType == SearchType.IsNull ? "" : $"@{e.PropertyName}")}"));
        }
    }
}
