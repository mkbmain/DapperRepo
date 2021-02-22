using System;
using System.Linq;
using System.Threading.Tasks;
using DapperRepoTests.Entities;
using DapperRepoTests.Tests.BaseTestClasses;
using DapperRepoTests.Utils;
using NUnit.Framework;

namespace DapperRepoTests.Tests.Delete
{
    public class DeleteDbSyncTest : BaseDbSyncTestClass
    {
        public DeleteDbSyncTest() : base( $"DeleteDapperRepoTests{Guid.NewGuid().ToString("N").Substring(0, 5)}")
        {
        }
        

        [Test]
        public void Ensure_we_can_Delete_a_record()
        {
            var testTableItem = new TestTable {Id = Guid.NewGuid(), Name = "Michale", SomeNumber = 33};
            var dontTouch = new TestTable {Id = Guid.NewGuid(), Name = "gwgw", SomeNumber = 12};
            DataBaseScriptRunnerAndBuilder.InsertTestTables(_connection, new[] {testTableItem, dontTouch});
            Sut.Delete(testTableItem);

            var records = DataBaseScriptRunnerAndBuilder.GetAll<TestTable>(_connection);

            Assert.AreEqual(1, records.Count());
            Assert.AreEqual(dontTouch.Id, records.First().Id);
            Assert.AreEqual(dontTouch.SomeNumber, records.First().SomeNumber);
            Assert.AreEqual(dontTouch.Name, records.First().Name);
        }
    }
}