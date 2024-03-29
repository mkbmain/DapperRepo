using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Dapper;
using Mkb.DapperRepo.Search;

namespace Mkb.DapperRepo.Repo
{
    public class SqlRepo : SqlRepoBase
    {
        public SqlRepo(Func<DbConnection> connection) : base(connection)
        {
        }

        public virtual T QuerySingle<T>(string sql)
        {
            return QuerySingle<T>(sql, null);
        }

        public virtual T QuerySingle<T>(string sql, object param)
        {
            return BaseGetAll<T, T>((connection, sql2) => connection.QueryFirstOrDefault<T>(sql2, param), sql);
        }

        public virtual IEnumerable<T> QueryMany<T>(string sql)
        {
            return QueryMany<T>(sql, null);
        }

        public virtual IEnumerable<T> QueryMany<T>(string sql, object param)
        {
            return BaseGetAll<T, IEnumerable<T>>((connection, sql2) => connection.Query<T>(sql2, param), sql);
        }

        public virtual IEnumerable<T> GetAllByX<T, PropT>(string property, object term) where T : class, new()
        {
            return Search(SetFieldOf<T, PropT>(new T(), property, term),
                SearchCriteria.Create(property, SearchType.Equals));
        }

        public virtual T GetById<T>(T element)
        {
            return BaseGet<T, T>((connection, s) => connection.QueryFirstOrDefault<T>(s, element));
        }

        public virtual IEnumerable<T> GetAll<T>()
        {
            return BaseGetAll<T, IEnumerable<T>>((connection, s) => connection.Query<T>(s));
        }

        public virtual int Count<T>()
        {
            return BaseCount<T, int>((connection, s) => (connection.ExecuteScalar<int>(new CommandDefinition(s))));
        }

        public virtual IEnumerable<T> GetExactMatches<T>(T item, bool ignoreNulls)
        {
            return BaseGetExactMatches(item, (connection2, s2) =>
                connection2.Query<T>(s2, item), ignoreNulls);
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
            return BaseSearchEntity<T, IEnumerable<T>>((connection, s) => connection.Query<T>(s, item), searchCriteria);
        }

        public virtual int SearchCount<T, TIn>(string property, TIn term, SearchType searchType) where T : class, new()
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
            return BaseSearchCount<T, int>(
                (connection, s) =>
                    connection.ExecuteScalar<int>(new CommandDefinition(s, item)),
                searchCriteria);
        }

        public virtual void Add<T>(T element)
        {
            BaseAdd(new[] { element }, (connection, s) =>
            {
                connection.Query<T>(s, element);
                return Task.CompletedTask;
            });
        }

        public virtual void Update<T>(T element, bool ignoreNullProperties = false)
        {
            BaseUpdate(element, ignoreNullProperties, ExecuteFunc(element));
        }

        public virtual void Execute(string sql) => BaseExecute(sql, (connection, s) =>
        {
            connection.Execute(s);
            return Task.CompletedTask;
        });

        public virtual void Execute<T>(T element, string sql) => BaseExecute<T>(sql, ExecuteFunc(element));

        private static Func<DbConnection, string, Task> ExecuteFunc<T>(T element) => (connection, s) =>
        {
            connection.Execute(s, new[] { element });
            return Task.CompletedTask;
        };

        public virtual void Delete<T>(T element)
        {
            BaseDelete<T>((connection, s) =>
            {
                connection.Execute(s, new[] { element });
                return Task.CompletedTask;
            });
        }
    }
}