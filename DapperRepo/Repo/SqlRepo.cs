using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;

namespace DapperRepo.Repo
{
    public class SqlRepo : SqlRepoBase
    {
        public SqlRepo(string connectionString) : base(connectionString)
        {
        }
        
        public T QuerySingle<T>(string sql)
        {
            return BaseGetAll((connection, sql2) => connection.QueryFirstOrDefault<T>(sql2), sql);
        }
        
        public IEnumerable<T> QueryMany<T>(string sql)
        {
            return BaseGetAll((connection, sql2) => connection.Query<T>(sql2), sql);
        }

        public T GetById<T>(T element)
        {
            return BaseGet<T, T>((connection, s) => connection.QueryFirstOrDefault<T>(s, element));
        }

        public IEnumerable<T> GetAll<T>()
        {
            return BaseGetAll<T, IEnumerable<T>>((connection, s) => connection.Query<T>(s));
        }
        
        public T Add<T>(T element)
        {
            return BaseAdd(new[] {element}, (connection, s) => connection.QuerySingle<T>(s, element), true);
        }
        
        public void AddMany<T>(IEnumerable<T> elements)
        {
            BaseAdd(elements, (connection, s) =>
            {
                connection.Execute(s, elements);
                return Task.CompletedTask;
            }, false);
        }
        
        public void Update<T>(T element, bool ignoreNullProperties = false)
        {
            BaseUpdate(element, ignoreNullProperties, (connection, s) =>
            {
                connection.Execute(s, new[] {element});
                return Task.CompletedTask;
            });
        }

        public void Delete<T>(T element)
        {
            BaseDelete<T>((connection, s) =>
            {
                connection.Execute(s, new[] {element});
                return Task.CompletedTask;
            });
        }
    }
}