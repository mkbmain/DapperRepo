using System;
using System.Collections.Generic;
using System.Data.Common;
using Dapper;
using Mkb.DapperRepo.Mappers;
using Mkb.DapperRepo.Search;
using System.Linq;

namespace Mkb.DapperRepo.Repo
{
    public class SqlRepo : SqlRepoBase
    {
        public SqlRepo(Func<DbConnection> connection) : base(connection)
        {
        }

        public virtual T QuerySingle<T>(string sql) => QuerySingle<T>(sql, null);

        public virtual T QuerySingle<T>(string sql, object param)
        {
            TableMapper.Setup<T>();
            using (var connection = CreateConnection())
                return connection.QueryFirstOrDefault<T>(sql, param);
        }

        public virtual IEnumerable<T> QueryMany<T>(string sql) => QueryMany<T>(sql, null);

        public virtual IEnumerable<T> QueryMany<T>(string sql, object param)
        {
            TableMapper.Setup<T>();
            using (var connection = CreateConnection())
                return connection.Query<T>(sql, param);
        }

        public virtual IEnumerable<T> GetAllByX<T, PropT>(string property, PropT term) where T : class, new()
        {
            return Search(SetFieldOf<T, PropT>(new T(), property, term),
                SearchCriteria.Create(property, SearchType.Equals));
        }

        public virtual T GetById<T>(T element)
        {
            TableMapper.Setup<T>();
            using (var connection = CreateConnection())
                return connection.QueryFirstOrDefault<T>(GetByIdSql<T>(), element);
        }

        public virtual IEnumerable<T> GetAll<T>()
        {
            TableMapper.Setup<T>();
            using (var connection = CreateConnection())
                return connection.Query<T>(SelectAllSql<T>());
        }

        public virtual int Count<T>()
        {
            TableMapper.Setup<T>();
            using (var connection = CreateConnection())
                return connection.ExecuteScalar<int>(new CommandDefinition(CountSql<T>()));
        }

        public virtual IEnumerable<T> GetExactMatches<T>(T item, bool ignoreNulls)
        {
            TableMapper.Setup<T>();
            using (var connection = CreateConnection())
                return connection.Query<T>(ExactMatchesSql<T>(item, ignoreNulls), item);
        }

        public virtual IEnumerable<T> Search<T, TIn>(string property, TIn term, SearchType searchType)
            where T : class, new()
        {
            return Search<T>(SetFieldOf<T, TIn>(new T(), property, term), SearchCriteria.Create(property, searchType));
        }

        public virtual IEnumerable<T> Search<T>(string property, string term) where T : class, new()
        {
            return Search<T, string>(property, term, SearchType.Like);
        }

        public virtual IEnumerable<T> Search<T>(T item, SearchCriteria searchCriteria)
        {
            return Search(item, new[] { searchCriteria });
        }

        public virtual IEnumerable<T> Search<T>(T item, IEnumerable<SearchCriteria> searchCriteria)
        {
            TableMapper.Setup<T>();
            using (var connection = CreateConnection())
                return connection.Query<T>(SearchSql<T>(searchCriteria), item);
        }

        public virtual int SearchCount<T, TIn>(string property, TIn term, SearchType searchType)
            where T : class, new()
        {
            return SearchCount(SetFieldOf<T, TIn>(new T(), property, term),
                SearchCriteria.Create(property, searchType));
        }

        public virtual int SearchCount<T>(T item, SearchCriteria searchCriteria)
        {
            return SearchCount(item, new[] { searchCriteria });
        }

        public virtual int SearchCount<T>(T item, IEnumerable<SearchCriteria> searchCriteria)
        {
            TableMapper.Setup<T>();
            using (var connection = CreateConnection())
                return connection.ExecuteScalar<int>(new CommandDefinition(SearchCountSql<T>(searchCriteria), item));
        }

        public virtual IEnumerable<T> Search<T>(IEnumerable<SearchTerm<T>> terms)
        {
            TableMapper.Setup<T>();
            var (sql, dp) = SearchTermsSql(terms.ToArray());
            using (var connection = CreateConnection())
                return connection.Query<T>(sql, dp);
        }

        public virtual int SearchCount<T>(IEnumerable<SearchTerm<T>> terms)
        {
            TableMapper.Setup<T>();
            var (sql, dp) = SearchCountTermsSql(terms.ToArray());
            using (var connection = CreateConnection())
                return connection.ExecuteScalar<int>(new CommandDefinition(sql, dp));
        }

        public virtual void Add<T>(T element)
        {
            TableMapper.Setup<T>();
            using (var connection = CreateConnection())
                connection.Execute(BuildInsertSql(new[] { element }), element);
        }

        public virtual void Update<T>(T element, bool ignoreNullProperties = false)
        {
            TableMapper.Setup<T>();
            using (var connection = CreateConnection())
                connection.Execute(BuildUpdateSql(element, ignoreNullProperties), new[] { element });
        }

        public virtual void Execute(string sql)
        {
            using (var connection = CreateConnection())
                connection.Execute(sql);
        }

        public virtual void Execute<T>(T element, string sql)
        {
            TableMapper.Setup<T>();
            using (var connection = CreateConnection())
                connection.Execute(sql, new[] { element });
        }

        public virtual void Delete<T>(T element)
        {
            using (var connection = CreateConnection())
                connection.Execute(DeleteSql<T>(), new[] { element });
        }
    }
}
