using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace DapperRepo.Repo
{
    public class SqlRepoAsync : SqlRepoBase
    {
        public SqlRepoAsync(string connectionString) : base(connectionString)
        {
        }

        public Task<T> GetById<T>(T element)
        {
            return BaseGet((connection, s) => connection.QueryFirstOrDefaultAsync<T>(s, element));
        }

        public Task<IEnumerable<T>> GetAll<T>()
        {
            return BaseGetAll((connection, s) => (connection.QueryAsync<T>(s)));
        }

        public Task AddMany<T>(IEnumerable<T> elements)
        {
            return BaseAddMany<T,Task>(elements,(connection, s) => connection.ExecuteAsync(s, elements),false);
        }

        public Task<T> Add<T>(T element)
        {
            return BaseAddMany(new []{element}, async (connection, s) => (await connection.QueryAsync<T>(s, element)).First(),true);
        }

        public Task Update<T>(T element, bool ignoreNullProperties = false)
        {
            return BaseUpdate(element, ignoreNullProperties,
                (connection, s) => connection.ExecuteAsync(s, new[] {element}));
        }

        public Task Delete<T>(T element)
        {
            return BaseDelete<T>((connection, s) => connection.ExecuteAsync(s, new[] {element}));
        }
    }
}