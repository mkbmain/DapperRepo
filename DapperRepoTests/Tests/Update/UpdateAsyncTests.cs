using System;
using System.Linq;
using System.Threading.Tasks;
using DapperRepoTests.Entities;
using DapperRepoTests.Tests.BaseTestClasses;
using DapperRepoTests.Utils;
using NUnit.Framework;

namespace DapperRepoTests.Tests.Update
{
    public class UpdateTestClassAsyncDbAsyncTest : BaseDbAsyncTestClass
    {
        public UpdateTestClassAsyncDbAsyncTest() : base( $"{nameof(UpdateTestClassAsyncDbAsyncTest)}{RandomChars}")
        {
        }

        [Test]
        public async Task Ensure_we_can_update_a_record()
        {
            var testTableItem = new TestTable {Id = Guid.NewGuid(), Name = "Michale", SomeNumber = 33};
            DataBaseScriptRunnerAndBuilder.InsertTestTables(Connection, new []{testTableItem});
            
            testTableItem.Name = "SomeOtherNAme";
            testTableItem.SomeNumber = 532;
            await Sut.Update(testTableItem);

            var records = DataBaseScriptRunnerAndBuilder.GetAll<TestTable>(Connection).ToArray();
                
            Assert.AreEqual(1,records.Count());
            Assert.AreEqual(testTableItem.Id, records.First().Id);
            Assert.AreEqual(testTableItem.SomeNumber, records.First().SomeNumber);
            Assert.AreEqual(testTableItem.Name, records.First().Name);
        }
        
        [Test]
        public async Task Ensure_we_only_update_one_record()
        {
            var testTableItem = new TestTable {Id = Guid.NewGuid(), Name = "Michale", SomeNumber = 33};
            var dontTouch = new TestTable {Id = Guid.NewGuid(), Name = "tgwre", SomeNumber = 33};
            DataBaseScriptRunnerAndBuilder.InsertTestTables(Connection, new []{testTableItem ,dontTouch  });
            
            testTableItem.Name = "SomeOtherNAme";
            testTableItem.SomeNumber = 532;
            await Sut.Update(testTableItem);

            var records = DataBaseScriptRunnerAndBuilder.GetAll<TestTable>(Connection).ToArray();
                
            Assert.AreEqual(2,records.Count());
            Assert.AreEqual(testTableItem.Id, records.First(f=> f.Id == testTableItem.Id).Id);
            Assert.AreEqual(testTableItem.SomeNumber, records.First(f=> f.Id == testTableItem.Id).SomeNumber);
            Assert.AreEqual(testTableItem.Name, records.First(f=> f.Id == testTableItem.Id).Name);
            
            Assert.AreEqual(dontTouch.Id, records.First(f=> f.Id == dontTouch.Id).Id);
            Assert.AreEqual(dontTouch.SomeNumber, records.First(f=> f.Id == dontTouch.Id).SomeNumber);
            Assert.AreEqual(dontTouch.Name, records.First(f=> f.Id == dontTouch.Id).Name);
        }
        
        [Test]
        public async Task Ensure_we_can_ignore_null_update_a_record()
        {
            var testTableItem = new TestTable {Id = Guid.NewGuid(), Name = "Michale", SomeNumber = 33};
            DataBaseScriptRunnerAndBuilder.InsertTestTables(Connection, new []{testTableItem});
            
            testTableItem.Name =null;
            testTableItem.SomeNumber = 532;
            await Sut.Update(testTableItem, true);

            var records = DataBaseScriptRunnerAndBuilder.GetAll<TestTable>(Connection).ToArray();
                
            Assert.AreEqual(1,records.Count());
            Assert.AreEqual(testTableItem.Id, records.First().Id);
            Assert.AreEqual(testTableItem.SomeNumber, records.First().SomeNumber);
            Assert.AreEqual("Michale", records.First().Name);
        }
        
        [Test]
        public async Task Ensure_we_dont_ignore_nulls_by_default()
        {
            var testTableItem = new TestTable {Id = Guid.NewGuid(), Name = "Michale", SomeNumber = 33};
            DataBaseScriptRunnerAndBuilder.InsertTestTables(Connection, new []{testTableItem});
            
            testTableItem.Name =null;
            testTableItem.SomeNumber = 532;
            await Sut.Update(testTableItem);

            var records = DataBaseScriptRunnerAndBuilder.GetAll<TestTable>(Connection).ToArray();
                
            Assert.AreEqual(1,records.Count());
            Assert.AreEqual(testTableItem.Id, records.First().Id);
            Assert.AreEqual(testTableItem.SomeNumber, records.First().SomeNumber);
            Assert.IsNull( records.First().Name);
        }
    }
}