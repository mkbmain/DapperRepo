using System;
using System.Linq;
using DapperRepoTests.Entities;
using DapperRepoTests.Tests.BaseTestClasses;
using DapperRepoTests.Utils;
using NUnit.Framework;

namespace DapperRepoTests.Tests.AddMany
{
    public class AddManyDbSyncTests : BaseDbSyncTestClass
    {

        public AddManyDbSyncTests() : base( $"{nameof(AddManyDbSyncTests)}{RandomChars}")
        {
        }
        

        [Test]
        public void Ensure_we_can_add_multiple_records()
        {
            var testTableItems = new[]
            {
                new TestTable {Id = Guid.NewGuid(), Name = "Michale", SomeNumber = 33},
                new TestTable {Id = Guid.NewGuid(), Name = "othername", SomeNumber = 1}
            };

            Sut.AddMany(testTableItems);

            var result = DataBaseScriptRunnerAndBuilder.GetAll<TestTable>(_connection);  
            var items = result as TestTable[] ?? result.ToArray();

            Assert.AreEqual(testTableItems.Length, items.Length);
            foreach (var item in testTableItems)
            {
                var test = items.FirstOrDefault(x => x.Id == item.Id);

                Assert.IsNotNull(test);
                Assert.AreEqual(item.Name, test.Name);
                Assert.AreEqual(item.SomeNumber, test.SomeNumber);
            }
        }
    }
}