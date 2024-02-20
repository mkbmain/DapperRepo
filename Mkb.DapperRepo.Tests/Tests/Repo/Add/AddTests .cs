using System;
using System.Linq;
using Mkb.DapperRepo.Tests.Entities;
using Mkb.DapperRepo.Tests.Tests.BaseTestClasses;
using Mkb.DapperRepo.Tests.Utils;
using NUnit.Framework;

namespace Mkb.DapperRepo.Tests.Tests.Repo.Add
{
    public class AddTests : BaseSyncTestClass
    {
        [Test]
        public void Ensure_we_can_add()
        {
            var testTableItem = new TableWithNoAutoGeneratedPrimaryKey
                { Id = Guid.NewGuid().ToString("N"), NameTest = "Michale", SomeNumber = 33 };
            Sut.Add(testTableItem);

            var resultInDb = DataBaseScriptRunnerAndBuilder.GetAll<TableWithNoAutoGeneratedPrimaryKey>(Connection);
            var items = resultInDb as TableWithNoAutoGeneratedPrimaryKey[] ?? resultInDb.ToArray();

            Assert.AreEqual(1, items.Length);
            var test = items.FirstOrDefault();
            Assert.IsNotNull(test);
            Assert.AreEqual(testTableItem.Id, test.Id);
            Assert.AreEqual(testTableItem.NameTest, test.NameTest);
            Assert.AreEqual(testTableItem.SomeNumber, test.SomeNumber);
        }

        [Test]
        public void Ensure_we_can_add_tables_with_diff_name()
        {
            var testTableItem = new TableWithNoAutoGeneratedPrimaryKey
                { Id = Guid.NewGuid().ToString("N"), NameTest = "Michale", SomeNumber = 33 };
            Sut.Add(new TableWithNoAutoGeneratedPrimaryKeyDiffSqlName(testTableItem));

            var resultInDb = DataBaseScriptRunnerAndBuilder.GetAll<TableWithNoAutoGeneratedPrimaryKey>(Connection);
            var items = resultInDb as TableWithNoAutoGeneratedPrimaryKey[] ?? resultInDb.ToArray();

            Assert.AreEqual(1, items.Length);
            var test = items.FirstOrDefault();
            Assert.IsNotNull(test);
            Assert.AreEqual(testTableItem.Id, test.Id);
            Assert.AreEqual(testTableItem.NameTest, test.NameTest);
            Assert.AreEqual(testTableItem.SomeNumber, test.SomeNumber);
        }

        [Test]
        public void Ensure_we_can_add_with_auto_increment()
        {
            var testTableItem = new TableWithAutoIncrementPrimaryKey() { NameTests = "Michale", SomeNum = 33 };
            Sut.Add(testTableItem);


            var resultInDb = DataBaseScriptRunnerAndBuilder.GetAll<TableWithAutoIncrementPrimaryKey>(Connection);
            var items = resultInDb as TableWithAutoIncrementPrimaryKey[] ?? resultInDb.ToArray();

            Assert.AreEqual(1, items.Length);
            var test = items.FirstOrDefault();
            Assert.IsNotNull(test);
            Assert.IsNotNull(test.BigTest);
            Assert.True(test.BigTest > 0);
            Assert.AreEqual(testTableItem.NameTests, test.NameTests);
            Assert.AreEqual(testTableItem.SomeNum, test.SomeNum);
        }

        [Test]
        public void Ensure_we_can_add_with_auto_increment_with_diff_names()
        {
            var testTableItem = new TableWithAutoIncrementPrimaryKey() { NameTests = "Michale", SomeNum = 33 };
            Sut.Add(new TableWithAutoIncrementPrimaryKeyDiffSqlName(testTableItem));

            var resultInDb = DataBaseScriptRunnerAndBuilder.GetAll<TableWithAutoIncrementPrimaryKey>(Connection);
            var items = resultInDb as TableWithAutoIncrementPrimaryKey[] ?? resultInDb.ToArray();

            Assert.AreEqual(1, items.Length);
            var test = items.FirstOrDefault();
            Assert.IsNotNull(test);
            Assert.IsNotNull(test.BigTest);
            Assert.True(test.BigTest > 0);
            Assert.AreEqual(testTableItem.NameTests, test.NameTests);
            Assert.AreEqual(testTableItem.SomeNum, test.SomeNum);
        }

        [Test]
        public void Ensure_we_can_add_with_null_field_for_other_values()
        {
            var testTableItem = new TableWithAutoIncrementPrimaryKey() { NameTests = "Michale", SomeNum = null };
            Sut.Add(testTableItem);

            var resultInDb = DataBaseScriptRunnerAndBuilder.GetAll<TableWithAutoIncrementPrimaryKey>(Connection);
            var items = resultInDb as TableWithAutoIncrementPrimaryKey[] ?? resultInDb.ToArray();

            Assert.AreEqual(1, items.Length);
            var test = items.FirstOrDefault();
            Assert.IsNotNull(test);
            Assert.IsNotNull(test.BigTest);
            Assert.True(test.BigTest > 0);
            Assert.AreEqual(testTableItem.NameTests, test.NameTests);
            Assert.IsNull(test.SomeNum);
        }

        [Test]
        public void Ensure_we_can_add_with_null_field_for_other_values_with_diff_name()
        {
            var testTableItem = new TableWithAutoIncrementPrimaryKey() { NameTests = "Michale", SomeNum = null };
            Sut.Add(new TableWithAutoIncrementPrimaryKeyDiffSqlName(testTableItem));

            var resultInDb = DataBaseScriptRunnerAndBuilder.GetAll<TableWithAutoIncrementPrimaryKey>(Connection);
            var items = resultInDb as TableWithAutoIncrementPrimaryKey[] ?? resultInDb.ToArray();

            Assert.AreEqual(1, items.Length);
            var test = items.FirstOrDefault();
            Assert.IsNotNull(test);
            Assert.IsNotNull(test.BigTest);
            Assert.True(test.BigTest > 0);
            Assert.AreEqual(testTableItem.NameTests, test.NameTests);
            Assert.IsNull(test.SomeNum);
        }
    }
}