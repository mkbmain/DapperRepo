using System;
using System.Linq;
using System.Threading.Tasks;
using DapperRepoTests.Entities;
using DapperRepoTests.Tests.BaseTestClasses;
using DapperRepoTests.Utils;
using NUnit.Framework;

namespace DapperRepoTests.Tests.Delete
{
    public class DeleteTestClassAsyncDbAsyncTest : BaseDbAsyncTestClass
    {
        public DeleteTestClassAsyncDbAsyncTest() : base($"{nameof(DeleteTestClassAsyncDbAsyncTest)}{RandomChars}")
        {
        }

        [Test]
        public async Task Ensure_we_can_Delete_a_record()
        {
            var testTableItem = new TestTable {Id = Guid.NewGuid(), Name = "Michale", SomeNumber = 33};
            var dontTouch = new TestTable {Id = Guid.NewGuid(), Name = "gwgw", SomeNumber = 12};
            DataBaseScriptRunnerAndBuilder.InsertTestTables(Connection, new[] {testTableItem, dontTouch});
            await Sut.Delete(testTableItem);

            var records = DataBaseScriptRunnerAndBuilder.GetAll<TestTable>(Connection).ToArray();
            Assert.AreEqual(1, records.Count());
            Assert.AreEqual(dontTouch.Id, records.First().Id);
            Assert.AreEqual(dontTouch.SomeNumber, records.First().SomeNumber);
            Assert.AreEqual(dontTouch.Name, records.First().Name);
        }
    }
}