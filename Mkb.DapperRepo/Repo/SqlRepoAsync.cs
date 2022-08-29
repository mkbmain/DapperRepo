using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
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
        
        public virtual Task<T> QuerySingle<T>(string sql)
        {
            return BaseGetAll<T,Task<T>>((connection, sql2) => connection.QueryFirstOrDefaultAsync<T>(sql2), sql);
        }

        public virtual Task<IEnumerable<T>> QueryMany<T>(string sql)
        {
            return BaseGetAll<T,Task<IEnumerable<T>>>((connection, sql2) => connection.QueryAsync<T>(sql2), sql);
        }

        public virtual Task<IEnumerable<T>> GetAllByX<T, PropT>(string property, object term) where T : class, new()
        {
            return Search<T>(SetFieldOf<T, PropT>(new T(), property, term),
                new SearchCriteria {PropertyName = property, SearchType = SearchType.Equals});
        }
        
        public virtual Task<T> GetById<T>(T element)
        {
            return BaseGet<T, Task<T>>((connection, s) => connection.QueryFirstOrDefaultAsync<T>(s, element));
        }
        
        public virtual Task<IEnumerable<T>> GetAll<T>()
        {
            return BaseGetAll<T, Task<IEnumerable<T>>>((connection, s) => (connection.QueryAsync<T>(s)));
        }

        public virtual Task<IEnumerable<T>> GetExactMatches<T>(T item, bool ignoreNulls)
        {
            return BaseGetExactMatches(item, (connection2, s2) =>
                connection2.QueryAsync<T>(s2, item), ignoreNulls);
        }

        public virtual Task<IEnumerable<T>> Search<T>(string property, string term) where T : class, new()
        {
            return Search(SetFieldOf<T, string>(new T(), property, term),
                new SearchCriteria {PropertyName = property, SearchType = SearchType.Like});
        }

        public virtual Task<IEnumerable<T>> Search<T>(T item, SearchCriteria searchCriteria)
        {
            return Search(item, new[] {searchCriteria});
        }

        public virtual Task<IEnumerable<T>> Search<T>(T item, IEnumerable<SearchCriteria> searchCriteria)
        {
            return BaseSearch<T, Task<IEnumerable<T>>>((connection, s) => connection.QueryAsync<T>(s, item),
                searchCriteria);
        }

        public virtual Task Add<T>(T element)
        {
            return BaseAdd(new[] {element}, async (connection, s) => { await connection.QueryAsync<T>(s, element); });
        }

        public virtual Task Update<T>(T element, bool ignoreNullProperties = false)
        {
            return BaseUpdate(element, ignoreNullProperties,
                (connection, s) => connection.ExecuteAsync(s, new[] {element}));
        }

        public virtual Task Delete<T>(T element)
        {
            return BaseDelete<T>((connection, s) => connection.ExecuteAsync(s, new[] {element}));
        }
    }
}