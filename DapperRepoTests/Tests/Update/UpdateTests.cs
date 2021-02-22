using System;
using System.Linq;
using System.Threading.Tasks;
using DapperRepoTests.Entities;
using DapperRepoTests.Tests.BaseTestClasses;
using DapperRepoTests.Utils;
using NUnit.Framework;

namespace DapperRepoTests.Tests.Update
{
    public class UpdateDbSyncTest : BaseDbSyncTestClass
    {
        public UpdateDbSyncTest() : base( $"UpdateDapperRepoTests{Guid.NewGuid().ToString("N").Substring(0, 5)}")
        {
        }

        [Test]
        public void Ensure_we_can_update_a_record()
        {
            var testTableItem = new TestTable {Id = Guid.NewGuid(), Name = "Michale", SomeNumber = 33};
            DataBaseScriptRunnerAndBuilder.InsertTestTables(_connection, new[] {testTableItem});

            testTableItem.Name = "SomeOtherNAme";
            testTableItem.SomeNumber = 532;
            Sut.Update(testTableItem);

            var records = DataBaseScriptRunnerAndBuilder.GetAll<TestTable>(_connection);

            Assert.AreEqual(1, records.Count());
            Assert.AreEqual(testTableItem.Id, records.First().Id);
            Assert.AreEqual(testTableItem.SomeNumber, records.First().SomeNumber);
            Assert.AreEqual(testTableItem.Name, records.First().Name);
        }

        [Test]
        public void Ensure_we_only_update_one_record()
        {
            var testTableItem = new TestTable {Id = Guid.NewGuid(), Name = "Michale", SomeNumber = 33};
            var dontTouch = new TestTable {Id = Guid.NewGuid(), Name = "tgwre", SomeNumber = 33};
            DataBaseScriptRunnerAndBuilder.InsertTestTables(_connection, new[] {testTableItem, dontTouch});

            testTableItem.Name = "SomeOtherNAme";
            testTableItem.SomeNumber = 532;
            Sut.Update(testTableItem);

            var records = DataBaseScriptRunnerAndBuilder.GetAll<TestTable>(_connection);

            Assert.AreEqual(2, records.Count());
            Assert.AreEqual(testTableItem.Id, records.First(f => f.Id == testTableItem.Id).Id);
            Assert.AreEqual(testTableItem.SomeNumber, records.First(f => f.Id == testTableItem.Id).SomeNumber);
            Assert.AreEqual(testTableItem.Name, records.First(f => f.Id == testTableItem.Id).Name);

            Assert.AreEqual(dontTouch.Id, records.First(f => f.Id == dontTouch.Id).Id);
            Assert.AreEqual(dontTouch.SomeNumber, records.First(f => f.Id == dontTouch.Id).SomeNumber);
            Assert.AreEqual(dontTouch.Name, records.First(f => f.Id == dontTouch.Id).Name);
        }


        [Test]
        public void Ensure_we_can_ignore_null_update_a_record()
        {
            var testTableItem = new TestTable {Id = Guid.NewGuid(), Name = "Michale", SomeNumber = 33};
            DataBaseScriptRunnerAndBuilder.InsertTestTables(_connection, new[] {testTableItem});

            testTableItem.Name = null;
            testTableItem.SomeNumber = 532;
            Sut.Update(testTableItem, true);

            var records = DataBaseScriptRunnerAndBuilder.GetAll<TestTable>(_connection);

            Assert.AreEqual(1, records.Count());
            Assert.AreEqual(testTableItem.Id, records.First().Id);
            Assert.AreEqual(testTableItem.SomeNumber, records.First().SomeNumber);
            Assert.AreEqual("Michale", records.First().Name);
        }

        [Test]
        public void Ensure_we_dont_ignore_nulls_by_default()
        {
            var testTableItem = new TestTable {Id = Guid.NewGuid(), Name = "Michale", SomeNumber = 33};
            DataBaseScriptRunnerAndBuilder.InsertTestTables(_connection, new[] {testTableItem});

            testTableItem.Name = null;
            testTableItem.SomeNumber = 532;
            Sut.Update(testTableItem);

            var records = DataBaseScriptRunnerAndBuilder.GetAll<TestTable>(_connection);

            Assert.AreEqual(1, records.Count());
            Assert.AreEqual(testTableItem.Id, records.First().Id);
            Assert.AreEqual(testTableItem.SomeNumber, records.First().SomeNumber);
            Assert.IsNull(records.First().Name);
        }
    }
}