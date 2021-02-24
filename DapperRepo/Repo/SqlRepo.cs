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

        public T GetById<T>(T element)
        {
            return BaseGet<T, T>((connection, s) => connection.QueryFirstOrDefault<T>(s, element));
        }

        public IEnumerable<T> GetAll<T>()
        {
            return BaseGetAll<T, IEnumerable<T>>((connection, s) => connection.Query<T>(s));
        }

        public void AddMany<T>(IEnumerable<T> elements)
        {
            BaseAdd(elements, (connection, s) =>
            {
                connection.Execute(s, elements);
                return Task.CompletedTask;
            }, false);
        }

        public T Add<T>(T element)
        {
            return BaseAdd(new[] {element}, (connection, s) => connection.QuerySingle<T>(s, element), true);
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