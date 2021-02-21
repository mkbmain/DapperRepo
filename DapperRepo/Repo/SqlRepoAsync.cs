using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;

namespace DapperRepo.Repo
{
    public class SqlRepoAsync : SqlRepoBase
    {
        public SqlRepoAsync(string connectionString) : base(connectionString)
        {
        }

        public Task<T> GetById<T>(Guid Id)
        {
            return BaseGet(Id, (connection, s) => connection.QueryFirstOrDefaultAsync<T>(s));
        }

        public Task<IEnumerable<T>> GetAll<T>()
        {
            return BaseGetAll(async (connection, s) => (await connection.QueryAsync<T>(s)));
        }

        public Task AddMany<T>(IEnumerable<T> elements)
        {
            return BaseAddMany<T>((connection, s) => connection.ExecuteAsync(s, elements));
        }

        public Task Add<T>(T element)
        {
            return AddMany(new[] {element});
        }

        public Task Update<T>(T element, bool ignoreNullProperties = false)
        {
            return BaseUpdate(element, ignoreNullProperties, (connection, s) => connection.ExecuteAsync(s, new[] {element}));
        }

        public Task Delete<T>(T element)
        {
            return BaseDelete(element, (connection, s) => connection.ExecuteAsync(s, new[] {element}));
        }
    }
}