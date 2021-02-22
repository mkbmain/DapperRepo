using System;
using System.Linq;
using DapperRepoTests.Entities;
using DapperRepoTests.Tests.BaseTestClasses;
using DapperRepoTests.Utils;
using NUnit.Framework;

namespace DapperRepoTests.Tests.GetAll
{
    public class GetAllDbSyncTests : BaseDbSyncTestClass
    {
        public GetAllDbSyncTests() : base($"GetAllDapperRepoTests{Guid.NewGuid().ToString("N").Substring(0, 5)}")
        {
        }

        [Test]
        public void Ensure_we_get_all_records_back()
        {
            var testTableItems = new[]
            {
                new TestTable {Id = Guid.NewGuid(), Name = "Michale", SomeNumber = 33},
                new TestTable {Id = Guid.NewGuid(), Name = "othername", SomeNumber = 1}
            };
            DataBaseScriptRunnerAndBuilder.InsertTestTables(_connection, testTableItems);

            var items = Sut.GetAll<TestTable>().ToArray();

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
        public void Ensure_if_we_have_no_records_we_do_not_blow_up()
        {
            var items = (Sut.GetAll<TestTable>()).ToArray();
            Assert.AreEqual(0, items.Length);
        }
    }
}