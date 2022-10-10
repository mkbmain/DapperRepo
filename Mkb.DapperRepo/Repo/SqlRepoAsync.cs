using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
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
            return BaseGetAll<T, Task<T>>((connection, sql2) =>
                connection.QueryFirstOrDefaultAsync<T>(
                    new CommandDefinition(sql2, cancellationToken: cancellationToken)), sql);
        }

        public virtual Task<IEnumerable<T>> QueryMany<T>(string sql, CancellationToken cancellationToken = default)
        {
            return BaseGetAll<T, Task<IEnumerable<T>>>(
                (connection, sql2) =>
                    connection.QueryAsync<T>(new CommandDefinition(sql2, cancellationToken: cancellationToken)), sql);
        }

        public virtual Task<IEnumerable<T>> GetAllByX<T, PropT>(string property, object term,
            CancellationToken cancellationToken = default) where T : class, new()
        {
            return Search<T>(SetFieldOf<T, PropT>(new T(), property, term),
                new SearchCriteria {PropertyName = property, SearchType = SearchType.Equals}, cancellationToken);
        }

        public virtual Task<T> GetById<T>(T element, CancellationToken cancellationToken = default)
        {
            return BaseGet<T, Task<T>>((connection, s) =>
                connection.QueryFirstOrDefaultAsync<T>(new CommandDefinition(s, element,
                    cancellationToken: cancellationToken)));
        }

        public virtual Task<IEnumerable<T>> GetAll<T>(CancellationToken cancellationToken = default)
        {
            return BaseGetAll<T, Task<IEnumerable<T>>>((connection, s) =>
                (connection.QueryAsync<T>(new CommandDefinition(s, cancellationToken: cancellationToken))));
        }

        public virtual Task<IEnumerable<T>> GetExactMatches<T>(T item, bool ignoreNulls,
            CancellationToken cancellationToken = default)
        {
            return BaseGetExactMatches(item, (connection2, s2) =>
                    connection2.QueryAsync<T>(new CommandDefinition(s2, item, cancellationToken: cancellationToken)),
                ignoreNulls);
        }

        public virtual Task<IEnumerable<T>> Search<T>(string property, string term,
            CancellationToken cancellationToken = default) where T : class, new()
        {
            return Search(SetFieldOf<T, string>(new T(), property, term),
                new SearchCriteria {PropertyName = property, SearchType = SearchType.Like}, cancellationToken);
        }

        public virtual Task<IEnumerable<T>> Search<T>(T item, SearchCriteria searchCriteria,
            CancellationToken cancellationToken = default)
        {
            return Search(item, new[] {searchCriteria}, cancellationToken);
        }

        public virtual Task<IEnumerable<T>> Search<T>(T item, IEnumerable<SearchCriteria> searchCriteria,
            CancellationToken cancellationToken = default)
        {
            return BaseSearch<T, Task<IEnumerable<T>>>(
                (connection, s) =>
                    connection.QueryAsync<T>(new CommandDefinition(s, item, cancellationToken: cancellationToken)),
                searchCriteria);
        }

        public virtual Task Add<T>(T element, CancellationToken cancellationToken = default)
        {
            return BaseAdd(new[] {element},
                async (connection, s) =>
                {
                    await connection.QueryAsync<T>(new CommandDefinition(s, element,
                        cancellationToken: cancellationToken));
                });
        }

        public virtual Task Update<T>(T element, bool ignoreNullProperties = false,
            CancellationToken cancellationToken = default)
        {
            return BaseUpdate(element, ignoreNullProperties,
                (connection, s) =>
                    connection.ExecuteAsync(new CommandDefinition(s, new[] {element},
                        cancellationToken: cancellationToken)));
        }

        public virtual Task Execute(string sql) => BaseExecute(sql, (connection, s) => connection.ExecuteAsync(s));
        
        public virtual Task Execute<T>(T element, string sql) => BaseExecute<T>(sql, ExecuteFunc(element));

        private static Func<DbConnection, string, Task> ExecuteFunc<T>(T element) => (connection, s) =>
        {
            return connection.ExecuteAsync(s, new[] {element});
        };

        public virtual Task Delete<T>(T element, CancellationToken cancellationToken = default)
        {
            return BaseDelete<T>((connection, s) =>
                connection.ExecuteAsync(new CommandDefinition(s, new[] {element},
                    cancellationToken: cancellationToken)));
        }
    }
}