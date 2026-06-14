using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Mkb.DapperRepo.Mappers;
using Mkb.DapperRepo.Search;

namespace Mkb.DapperRepo.Repo
{
    public class SqlRepoAsync : SqlRepoBase
    {
        public SqlRepoAsync(Func<DbConnection> connection) : base(connection)
        {
        }

        public virtual Task<T> QuerySingle<T>(string sql, CancellationToken cancellationToken = default)
        {
            return QuerySingle<T>(sql, null, cancellationToken);
        }

        public virtual async Task<T> QuerySingle<T>(string sql, object param, CancellationToken cancellationToken = default)
        {
            TableMapper.Setup<T>();
            using (var connection = CreateConnection())
                return await connection.QueryFirstOrDefaultAsync<T>(
                    new CommandDefinition(sql, param, cancellationToken: cancellationToken));
        }

        public virtual Task<IEnumerable<T>> QueryMany<T>(string sql, CancellationToken cancellationToken = default)
        {
            return QueryMany<T>(sql, null, cancellationToken);
        }

        public virtual async Task<IEnumerable<T>> QueryMany<T>(string sql, object param,
            CancellationToken cancellationToken = default)
        {
            TableMapper.Setup<T>();
            using (var connection = CreateConnection())
                return await connection.QueryAsync<T>(
                    new CommandDefinition(sql, param, cancellationToken: cancellationToken));
        }

        public virtual Task<IEnumerable<T>> GetAllByX<T, PropT>(string property, PropT term,
            CancellationToken cancellationToken = default) where T : class, new()
        {
            return Search<T>(SetFieldOf<T, PropT>(new T(), property, term),
                SearchCriteria.Create(property, SearchType.Equals), cancellationToken);
        }

        public virtual async Task<T> GetById<T>(T element, CancellationToken cancellationToken = default)
        {
            TableMapper.Setup<T>();
            using (var connection = CreateConnection())
                return await connection.QueryFirstOrDefaultAsync<T>(
                    new CommandDefinition(GetByIdSql<T>(), element, cancellationToken: cancellationToken));
        }

        public virtual async Task<IEnumerable<T>> GetAll<T>(CancellationToken cancellationToken = default)
        {
            TableMapper.Setup<T>();
            using (var connection = CreateConnection())
                return await connection.QueryAsync<T>(
                    new CommandDefinition(SelectAllSql<T>(), cancellationToken: cancellationToken));
        }

        public virtual async Task<int> Count<T>(CancellationToken cancellationToken = default)
        {
            TableMapper.Setup<T>();
            using (var connection = CreateConnection())
                return await connection.ExecuteScalarAsync<int>(
                    new CommandDefinition(CountSql<T>(), cancellationToken: cancellationToken));
        }

        public virtual async Task<IEnumerable<T>> GetExactMatches<T>(T item, bool ignoreNulls,
            CancellationToken cancellationToken = default)
        {
            TableMapper.Setup<T>();
            using (var connection = CreateConnection())
                return await connection.QueryAsync<T>(
                    new CommandDefinition(ExactMatchesSql<T>(item, ignoreNulls), item,
                        cancellationToken: cancellationToken));
        }

        public virtual Task<IEnumerable<T>> Search<T, TIn>(string property, TIn term, SearchType searchType,
            CancellationToken cancellationToken = default) where T : class, new()
        {
            return Search(SetFieldOf<T, TIn>(new T(), property, term), SearchCriteria.Create(property, searchType),
                cancellationToken);
        }

        public virtual Task<IEnumerable<T>> Search<T>(string property, string term,
            CancellationToken cancellationToken = default) where T : class, new()
        {
            return Search<T, string>(property, term, SearchType.Like, cancellationToken);
        }

        public virtual Task<IEnumerable<T>> Search<T>(T item, SearchCriteria searchCriteria,
            CancellationToken cancellationToken = default)
        {
            return Search(item, new[] { searchCriteria }, cancellationToken);
        }

        public virtual async Task<IEnumerable<T>> Search<T>(T item, IEnumerable<SearchCriteria> searchCriteria,
            CancellationToken cancellationToken = default)
        {
            TableMapper.Setup<T>();
            using (var connection = CreateConnection())
                return await connection.QueryAsync<T>(
                    new CommandDefinition(SearchSql<T>(searchCriteria), item, cancellationToken: cancellationToken));
        }

        public virtual Task<int> SearchCount<T, TIn>(string property, TIn term, SearchType searchType,
            CancellationToken cancellationToken = default) where T : class, new()
        {
            return SearchCount(SetFieldOf<T, TIn>(new T(), property, term),
                SearchCriteria.Create(property, searchType), cancellationToken);
        }

        public virtual Task<int> SearchCount<T>(T item, SearchCriteria searchCriteria,
            CancellationToken cancellationToken = default)
        {
            return SearchCount(item, new[] { searchCriteria }, cancellationToken);
        }

        public virtual async Task<int> SearchCount<T>(T item, IEnumerable<SearchCriteria> searchCriteria,
            CancellationToken cancellationToken = default)
        {
            TableMapper.Setup<T>();
            using (var connection = CreateConnection())
                return await connection.ExecuteScalarAsync<int>(
                    new CommandDefinition(SearchCountSql<T>(searchCriteria), item,
                        cancellationToken: cancellationToken));
        }

        public virtual async Task<IEnumerable<T>> Search<T>(IEnumerable<SearchTerm<T>> terms,
            CancellationToken cancellationToken = default)
        {
            TableMapper.Setup<T>();
            var (sql, dp) = SearchTermsSql(terms.ToArray());
            using (var connection = CreateConnection())
                return await connection.QueryAsync<T>(
                    new CommandDefinition(sql, dp, cancellationToken: cancellationToken));
        }

        public virtual async Task<int> SearchCount<T>(IEnumerable<SearchTerm<T>> terms,
            CancellationToken cancellationToken = default)
        {
            TableMapper.Setup<T>();
            var (sql, dp) = SearchCountTermsSql(terms.ToArray());
            using (var connection = CreateConnection())
                return await connection.ExecuteScalarAsync<int>(
                    new CommandDefinition(sql, dp, cancellationToken: cancellationToken));
        }

        public virtual async Task Add<T>(T element, CancellationToken cancellationToken = default)
        {
            TableMapper.Setup<T>();
            using (var connection = CreateConnection())
                await connection.ExecuteAsync(
                    new CommandDefinition(BuildInsertSql(new[] { element }), element,
                        cancellationToken: cancellationToken));
        }

        public virtual async Task Update<T>(T element, bool ignoreNullProperties = false,
            CancellationToken cancellationToken = default)
        {
            TableMapper.Setup<T>();
            using (var connection = CreateConnection())
                await connection.ExecuteAsync(
                    new CommandDefinition(BuildUpdateSql(element, ignoreNullProperties), new[] { element },
                        cancellationToken: cancellationToken));
        }

        public virtual async Task Execute(string sql, CancellationToken cancellationToken = default)
        {
            using (var connection = CreateConnection())
                await connection.ExecuteAsync(
                    new CommandDefinition(sql, cancellationToken: cancellationToken));
        }

        public virtual async Task Execute<T>(T element, string sql, CancellationToken cancellationToken = default)
        {
            TableMapper.Setup<T>();
            using (var connection = CreateConnection())
                await connection.ExecuteAsync(
                    new CommandDefinition(sql, new[] { element }, cancellationToken: cancellationToken));
        }

        public virtual async Task Delete<T>(T element, CancellationToken cancellationToken = default)
        {
            using (var connection = CreateConnection())
                await connection.ExecuteAsync(
                    new CommandDefinition(DeleteSql<T>(), new[] { element }, cancellationToken: cancellationToken));
        }
    }
}
