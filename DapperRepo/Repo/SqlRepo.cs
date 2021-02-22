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
            return BaseGet((connection, s) => Task.FromResult(connection.QueryFirstOrDefault<T>(s, element))).Result;
        }

        public IEnumerable<T> GetAll<T>()
        {
            return BaseGetAll((connection, s) => Task.FromResult(connection.Query<T>(s))).Result;
        }

        public void AddMany<T>(IEnumerable<T> elements)
        {
            BaseAddMany<T>((connection, s) =>
            {
                connection.Execute(s, elements);
                return Task.CompletedTask;
            });
        }

        public void Add<T>(T element)
        {
            AddMany(new[] {element});
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
            BaseDelete(element, (connection, s) =>
            {
                connection.Execute(s, new[] {element});
                return Task.CompletedTask;
            });
        }
    }
}