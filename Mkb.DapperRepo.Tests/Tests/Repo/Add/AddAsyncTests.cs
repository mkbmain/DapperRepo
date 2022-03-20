using System;
using System.Linq;
using System.Threading.Tasks;
using Mkb.DapperRepo.Tests.Entities;
using Mkb.DapperRepo.Tests.Tests.BaseTestClasses;
using Mkb.DapperRepo.Tests.Utils;
using NUnit.Framework;

namespace Mkb.DapperRepo.Tests.Tests.Repo.Add
{
    public class AddAsyncTests : BaseAsyncTestClass
    {
        [Test]
        public async Task Ensure_we_can_add()
        {
            var testTableItem = new TableWithNoAutoGeneratedPrimaryKey
                {Id = Guid.NewGuid().ToString("N"), Name = "Michale", SomeNumber = 33};
           await Sut.Add(testTableItem);


            var resultInDb = DataBaseScriptRunnerAndBuilder.GetAll<TableWithNoAutoGeneratedPrimaryKey>(Connection);
            var items = resultInDb as TableWithNoAutoGeneratedPrimaryKey[] ?? resultInDb.ToArray();

            Assert.AreEqual(1, items.Length);
            var test = items.FirstOrDefault();
            Assert.IsNotNull(test);
            Assert.AreEqual(testTableItem.Id, test.Id);
            Assert.AreEqual(testTableItem.Name, test.Name);
            Assert.AreEqual(testTableItem.SomeNumber, test.SomeNumber);
        }
        
        [Test]
        public async Task Ensure_we_can_add_tables_with_diff_name()
        {
            var testTableItem = new TableWithNoAutoGeneratedPrimaryKey
                {Id = Guid.NewGuid().ToString("N"), Name = "Michale", SomeNumber = 33};
           await Sut.Add(new TableWithNoAutoGeneratedPrimaryKeyDiffSqlName(testTableItem));

            var resultInDb = DataBaseScriptRunnerAndBuilder.GetAll<TableWithNoAutoGeneratedPrimaryKey>(Connection);
            var items = resultInDb as TableWithNoAutoGeneratedPrimaryKey[] ?? resultInDb.ToArray();

            Assert.AreEqual(1, items.Length);
            var test = items.FirstOrDefault();
            Assert.IsNotNull(test);
            Assert.AreEqual(testTableItem.Id, test.Id);
            Assert.AreEqual(testTableItem.Name, test.Name);
            Assert.AreEqual(testTableItem.SomeNumber, test.SomeNumber);
        }

        [Test]
        public async Task Ensure_we_can_add_with_auto_increment()
        {
            var testTableItem = new TableWithAutoIncrementPrimaryKey() {Name = "Michale", SomeNumber = 33};
           await Sut.Add(testTableItem);


            var resultInDb = DataBaseScriptRunnerAndBuilder.GetAll<TableWithAutoIncrementPrimaryKey>(Connection);
            var items = resultInDb as TableWithAutoIncrementPrimaryKey[] ?? resultInDb.ToArray();

            Assert.AreEqual(1, items.Length);
            var test = items.FirstOrDefault();
            Assert.IsNotNull(test);
            Assert.IsNotNull(test.Id);
            Assert.True(test.Id>0);
            Assert.AreEqual(testTableItem.Name, test.Name);
            Assert.AreEqual(testTableItem.SomeNumber, test.SomeNumber);
        }
        
        [Test]
        public async Task Ensure_we_can_add_with_auto_increment_diff_name()
        {
            var testTableItem = new TableWithAutoIncrementPrimaryKey() {Name = "Michale", SomeNumber = 33};
           await Sut.Add(new TableWithAutoIncrementPrimaryKeyDiffSqlName(testTableItem));

            var resultInDb = DataBaseScriptRunnerAndBuilder.GetAll<TableWithAutoIncrementPrimaryKey>(Connection);
            var items = resultInDb as TableWithAutoIncrementPrimaryKey[] ?? resultInDb.ToArray();

            Assert.AreEqual(1, items.Length);
            var test = items.FirstOrDefault();
            Assert.IsNotNull(test);
            Assert.IsNotNull(test.Id);
            Assert.True(test.Id>0);
            Assert.AreEqual(testTableItem.Name, test.Name);
            Assert.AreEqual(testTableItem.SomeNumber, test.SomeNumber);
        }

        [Test]
        public async Task Ensure_we_can_add_with_null_field_for_other_values()
        {
            var testTableItem = new TableWithAutoIncrementPrimaryKey() {Name = "Michale", SomeNumber = null};
            await Sut.Add(testTableItem);


            var resultInDb = DataBaseScriptRunnerAndBuilder.GetAll<TableWithAutoIncrementPrimaryKey>(Connection);
            var items = resultInDb as TableWithAutoIncrementPrimaryKey[] ?? resultInDb.ToArray();

            Assert.AreEqual(1, items.Length);
            var test = items.FirstOrDefault();
            Assert.IsNotNull(test);
            Assert.IsNotNull(test.Id);
            Assert.True(test.Id>0);
            Assert.AreEqual(testTableItem.Name, test.Name);
            Assert.IsNull(test.SomeNumber);
        }
        
        [Test]
        public async Task Ensure_we_can_add_with_null_field_for_other_values_with_diff_name()
        {
            var testTableItem = new TableWithAutoIncrementPrimaryKey() {Name = "Michale", SomeNumber = null};
            await Sut.Add(new TableWithAutoIncrementPrimaryKeyDiffSqlName(testTableItem));

            var resultInDb = DataBaseScriptRunnerAndBuilder.GetAll<TableWithAutoIncrementPrimaryKey>(Connection);
            var items = resultInDb as TableWithAutoIncrementPrimaryKey[] ?? resultInDb.ToArray();

            Assert.AreEqual(1, items.Length);
            var test = items.FirstOrDefault();
            Assert.IsNotNull(test);
            Assert.IsNotNull(test.Id);
            Assert.True(test.Id>0);
            Assert.AreEqual(testTableItem.Name, test.Name);
            Assert.IsNull(test.SomeNumber);
        }
    }
}