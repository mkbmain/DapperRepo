using System;
using System.Linq;
using System.Threading.Tasks;
using DapperRepoTests.Entities;
using DapperRepoTests.Tests.BaseTestClasses;
using DapperRepoTests.Utils;
using NUnit.Framework;

namespace DapperRepoTests.Tests.GetAll
{
    public class GetAllTestClassAsyncDbAsyncTests : BaseDbAsyncTestClass
    {
        public GetAllTestClassAsyncDbAsyncTests() : base( $"GetAllAsyncDapperRepoTests{Guid.NewGuid().ToString("N").Substring(0, 5)}")
        {
        }
        
        [Test]
        public async Task Ensure_we_get_all_records_back()
        {
            var testTableItems = new[]
            {
                new TestTable {Id = Guid.NewGuid(), Name = "Michale", SomeNumber = 33},
                new TestTable {Id = Guid.NewGuid(), Name = "othername", SomeNumber = 1}
            };
            
            DataBaseScriptRunnerAndBuilder.InsertTestTables(_connection, testTableItems);
            
            var items = (await Sut.GetAll<TestTable>()).ToArray();

            Assert.AreEqual(testTableItems.Length, items.Length);
            foreach (var item in testTableItems)
            {
                var test = items.FirstOrDefault(x => x.Id == item.Id);

                Assert.IsNotNull(test);
                Assert.AreEqual(item.Name, test.Name);
                Assert.AreEqual(item.SomeNumber, test.SomeNumber);
            }
        }
        
        [Test]
        public async Task Ensure_if_we_have_no_records_we_do_not_blow_up()
        {
            var items = (await Sut.GetAll<TestTable>()).ToArray();
            Assert.AreEqual(0, items.Length);
        }
    }
}