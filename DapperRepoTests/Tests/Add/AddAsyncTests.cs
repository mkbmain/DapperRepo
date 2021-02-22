using System;
using System.Linq;
using System.Threading.Tasks;
using DapperRepoTests.Entities;
using DapperRepoTests.Tests.BaseTestClasses;
using DapperRepoTests.Utils;
using NUnit.Framework;

namespace DapperRepoTests.Tests.Add
{
    public class AddTestClassAsyncDbAsyncTests : BaseDbAsyncTestClass
    {
        public AddTestClassAsyncDbAsyncTests() : base($"AddAsyncDapperRepoTests{Guid.NewGuid().ToString("N").Substring(0, 5)}")
        {
        }
        
        [Test]
        public async Task Ensure_we_can_add()
        {
            var testTableItem = new TestTable {Id = Guid.NewGuid(), Name = "Michale", SomeNumber = 33};
            await Sut.Add(testTableItem);

            var result = DataBaseScriptRunnerAndBuilder.GetAll<TestTable>(_connection);
            var items = result as TestTable[] ?? result.ToArray();

            Assert.AreEqual(1, items.Length);
            var test = items.FirstOrDefault();
            Assert.AreEqual(testTableItem.Id, test.Id);
            Assert.AreEqual(testTableItem.Name, test.Name);
            Assert.AreEqual(testTableItem.SomeNumber, test.SomeNumber);
        }
    }
}