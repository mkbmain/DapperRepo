using System;
using System.Linq;
using System.Threading.Tasks;
using Mkb.DapperRepo.Tests.Entities;
using Mkb.DapperRepo.Tests.Tests.BaseTestClasses;
using Mkb.DapperRepo.Tests.Utils;
using NUnit.Framework;

namespace Mkb.DapperRepo.Tests.Tests.Repo.GetById
{
    public class GetByIdAsyncTest : BaseAsyncTestClass
    {
        public GetByIdAsyncTest() : base($"{nameof(GetByIdAsyncTest)}")
        {
        }

        [Test]
        public async Task Ensure_we_Can_Get_correct_record_Back()
        {
            var testTableItems = new[]
            {
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid().ToString("N"), Name = "Michale", SomeNumber = 33},
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid().ToString("N"), Name = "othername", SomeNumber = 1}
            };

            DataBaseScriptRunnerAndBuilder.InsertTableWithNoAutoGeneratedPrimaryKey(Connection, testTableItems);

            var item = await Sut.GetById(new TableWithNoAutoGeneratedPrimaryKey {Id = testTableItems.First().Id});
            Assert.IsNotNull(item);
            Assert.AreEqual(testTableItems.First().Id, item.Id);
            Assert.AreEqual(testTableItems.First().SomeNumber, item.SomeNumber);
            Assert.AreEqual(testTableItems.First().Name, item.Name);
        }
        
        [Test]
        public async Task Ensure_we_Can_Get_correct_record_Back_diff_name()
        {
            var testTableItems = new[]
            {
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid().ToString("N"), Name = "Michale", SomeNumber = 33},
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid().ToString("N"), Name = "othername", SomeNumber = 1}
            };

            DataBaseScriptRunnerAndBuilder.InsertTableWithNoAutoGeneratedPrimaryKey(Connection, testTableItems);

            var item = await Sut.GetById(new TableWithNoAutoGeneratedPrimaryKeyDiffSqlName {Id = testTableItems.First().Id});
            Assert.IsNotNull(item);
            Assert.AreEqual(testTableItems.First().Id, item.Id);
            Assert.AreEqual(testTableItems.First().SomeNumber, item.SomeNumber);
            Assert.AreEqual(testTableItems.First().Name, item.Name);
        }


        [Test]
        public async Task Ensure_we_Can_Get_correct_record_Back_when_id_is_a_number()
        {
            var testTableItems = new[]
            {
                new TableWithAutoIncrementPrimaryKey {Name = "Michale", SomeNumber = 33},
            };
            DataBaseScriptRunnerAndBuilder.InsertTableWithAutoGeneratedPrimaryKey(Connection, testTableItems);
            var record = DataBaseScriptRunnerAndBuilder.GetAll<TableWithAutoIncrementPrimaryKey>(Connection).First();


            var item = await Sut.GetById(new TableWithAutoIncrementPrimaryKey {Id = record.Id});
            Assert.IsNotNull(item);
            Assert.AreEqual(record.Id, item.Id);
            Assert.AreEqual(testTableItems.First().SomeNumber, item.SomeNumber);
            Assert.AreEqual(testTableItems.First().Name, item.Name);
        }
        
        [Test]
        public async Task Ensure_we_Can_Get_correct_record_Back_when_id_is_a_number_diff_name()
        {
            var testTableItems = new[]
            {
                new TableWithAutoIncrementPrimaryKey {Name = "Michale", SomeNumber = 33},
            };
            DataBaseScriptRunnerAndBuilder.InsertTableWithAutoGeneratedPrimaryKey(Connection, testTableItems);
            var record = DataBaseScriptRunnerAndBuilder.GetAll<TableWithAutoIncrementPrimaryKey>(Connection).First();


            var item = await Sut.GetById(new TableWithAutoIncrementPrimaryKeyDiffSqlName {Id = record.Id});
            Assert.IsNotNull(item);
            Assert.AreEqual(record.Id, item.Id);
            Assert.AreEqual(testTableItems.First().SomeNumber, item.SomeNumber);
            Assert.AreEqual(testTableItems.First().Name, item.Name);
        }

        [Test]
        public async Task Ensure_if_no_record_found_we_return_null()
        {
            var testTableItems = new[]
            {
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid().ToString("N"), Name = "Michale", SomeNumber = 33},
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid().ToString("N"), Name = "othername", SomeNumber = 1}
            };

            DataBaseScriptRunnerAndBuilder.InsertTableWithNoAutoGeneratedPrimaryKey(Connection, testTableItems);

            var item = await Sut.GetById(new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid().ToString("N")});
            Assert.IsNull(item);
        }
        
        [Test]
        public async Task Ensure_if_no_record_found_we_return_null_diff_name()
        {
            var testTableItems = new[]
            {
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid().ToString("N"), Name = "Michale", SomeNumber = 33},
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid().ToString("N"), Name = "othername", SomeNumber = 1}
            };

            DataBaseScriptRunnerAndBuilder.InsertTableWithNoAutoGeneratedPrimaryKey(Connection, testTableItems);

            var item = await Sut.GetById(new TableWithNoAutoGeneratedPrimaryKeyDiffSqlName {Id = Guid.NewGuid().ToString("N")});
            Assert.IsNull(item);
        }
    }
}