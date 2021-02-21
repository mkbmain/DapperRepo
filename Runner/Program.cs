using System;
using System.Linq;
using System.Threading.Tasks;
using DapperRepo;
using DapperRepo.Repo;

namespace Runner
{
    class Program
    {
        private static void SyncVersion(string connection)
        {
            var repo = new SqlRepo(connection);
            var items = new[]
            {
                new TestTable {Id = Guid.NewGuid(), Name = "item1", SomeNumber = 33},
                new TestTable {Id = Guid.NewGuid(), Name = "item1", SomeNumber = 35},
            };
            repo.AddMany(items);

            var item = repo.GetById<TestTable>(items.First().Id);
            item.Name = "we editedit";
            repo.Update(item);
            items = repo.GetAll<TestTable>().ToArray();
            repo.Delete(items.LastOrDefault());
        }

        static async Task AsyncVersion(string connection)
        {
            var items = new[]
            {
                new TestTable {Id = Guid.NewGuid(), Name = "item2", SomeNumber = 33},
                new TestTable {Id = Guid.NewGuid(), Name = "item2", SomeNumber = 35},
            };
            var asyncRepo = new SqlRepoAsync(connection);
            await asyncRepo.AddMany(items);

            var item = await asyncRepo.GetById<TestTable>(items.First().Id);
            item.Name = "we editedit";
            await asyncRepo.Update(item);
            items = (await asyncRepo.GetAll<TestTable>()).ToArray();
            await asyncRepo.Delete(items.LastOrDefault());
        }

        static async Task Main(string[] args)
        {
            const string connection = "Server=localhost;Database=test;Trusted_Connection=True";
            SyncVersion(connection);
            await AsyncVersion(connection);
        }
    }
    public class TestTable
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        public string Name { get; set; }
        public int SomeNumber { get; set; }
    }
}