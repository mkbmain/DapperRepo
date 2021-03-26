using System;
using System.Linq;
using System.Threading.Tasks;
using DapperRepoTests.Entities;
using DapperRepoTests.Tests.BaseTestClasses;
using DapperRepoTests.Utils;
using NUnit.Framework;

namespace DapperRepoTests.Tests.Delete
{
    public class DeleteTests : BaseSyncTestClass
    {
        public DeleteTests() : base( $"{nameof(DeleteTests)}{RandomChars}")
        {
        }
        

        [Test]
        public void Ensure_we_can_Delete_a_record()
        {
            var testTableItem = new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid(), Name = "Michale", SomeNumber = 33};
            var dontTouch = new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid(), Name = "gwgw", SomeNumber = 12};
            DataBaseScriptRunnerAndBuilder.InsertTableWithNoAutoGeneratedPrimaryKey(Connection, new[] {testTableItem, dontTouch});
            Sut.Delete(testTableItem);

            var records = DataBaseScriptRunnerAndBuilder.GetAll<TableWithNoAutoGeneratedPrimaryKey>(Connection).ToArray();

            Assert.AreEqual(1, records.Count());
            Assert.AreEqual(dontTouch.Id, records.First().Id);
            Assert.AreEqual(dontTouch.SomeNumber, records.First().SomeNumber);
            Assert.AreEqual(dontTouch.Name, records.First().Name);
        }
    }
}