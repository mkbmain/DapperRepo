using System;
using System.Linq;
using DapperRepoTests.Entities;
using DapperRepoTests.Tests.BaseTestClasses;
using DapperRepoTests.Utils;
using NUnit.Framework;

namespace DapperRepoTests.Tests.GetById
{
    public class GetByIdDbSyncTest : BaseDbSyncTestClass
    {
        public GetByIdDbSyncTest() : base( $"GetByIdDapperRepoTests{Guid.NewGuid().ToString("N").Substring(0, 5)}")
        {
        }

        [Test]
        public void Ensure_we_Can_Get_correct_record_Back()
        {
            var testTableItems = new[]
            {
                new TestTable {Id = Guid.NewGuid(), Name = "Michale", SomeNumber = 33},
                new TestTable {Id = Guid.NewGuid(), Name = "othername", SomeNumber = 1}
            };
            DataBaseScriptRunnerAndBuilder.InsertTestTables(_connection, testTableItems);


            var item = Sut.GetById(new TestTable{Id = testTableItems.First().Id});;
            Assert.IsNotNull(item);
            Assert.AreEqual(testTableItems.First().Id, item.Id);
            Assert.AreEqual(testTableItems.First().SomeNumber, item.SomeNumber);
            Assert.AreEqual(testTableItems.First().Name, item.Name);
        }
        
        [Test]
        public void Ensure_if_no_record_found_we_return_null()
        {
            var testTableItems = new[]
            {
                new TestTable {Id = Guid.NewGuid(), Name = "Michale", SomeNumber = 33},
                new TestTable {Id = Guid.NewGuid(), Name = "othername", SomeNumber = 1}
            };
            
            DataBaseScriptRunnerAndBuilder.InsertTestTables(_connection, testTableItems);

            var item = Sut.GetById(new TestTable{Id = Guid.NewGuid()});;
            Assert.IsNull(item);
        }
    }
}