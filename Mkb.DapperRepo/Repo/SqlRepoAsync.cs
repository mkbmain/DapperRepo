using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace Mkb.DapperRepo.Repo
{
    public class SqlRepoAsync : SqlRepoBase
    {
        public SqlRepoAsync(string connectionString) : base(connectionString)
        {
        }
        

        public Task<T> QuerySingle<T>(string sql)
        {
            return BaseGetAll((connection, sql2) => connection.QueryFirstOrDefaultAsync<T>(sql2), sql);
        }

        public Task<IEnumerable<T>> QueryMany<T>(string sql)
        {
            return BaseGetAll((connection, sql2) => connection.QueryAsync<T>(sql2), sql);
        }

        public Task<T> GetById<T>(T element)
        {
            return BaseGet<T, Task<T>>((connection, s) => connection.QueryFirstOrDefaultAsync<T>(s, element));
        }

        public Task<IEnumerable<T>> GetAll<T>()
        {
            return BaseGetAll<T, Task<IEnumerable<T>>>((connection, s) => (connection.QueryAsync<T>(s)));
        }
        
        public Task<IEnumerable<T>> Search<T>(string property, string term) where T : class, new()
        {
            T item = new T();
            return Search(item, property, term);
        }

        public Task<IEnumerable<T>> Search<T>(T item, string property, string term) 
        {
            var theField = ReflectionUtils.GetPropertyInfoOfType<T>(typeof(string), property);

            theField.SetValue(item, term);

            return BaseSearch<T, Task<IEnumerable<T>>>((connection, s) => connection.QueryAsync<T>(s, item), property);
        }

        public Task<T> Add<T>(T element)
        {
            return BaseAdd(new[] {element},
                async (connection, s) => (await connection.QueryAsync<T>(s, element)).First(), true);
        }

        public Task AddMany<T>(IEnumerable<T> elements)
        {
            return BaseAdd<T, Task>(elements, (connection, s) => connection.ExecuteAsync(s, elements), false);
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