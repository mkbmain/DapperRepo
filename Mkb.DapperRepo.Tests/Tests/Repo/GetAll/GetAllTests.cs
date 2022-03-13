using System;
using System.Linq;
using Mkb.DapperRepo.Tests.Entities;
using Mkb.DapperRepo.Tests.Tests.BaseTestClasses;
using Mkb.DapperRepo.Tests.Utils;
using NUnit.Framework;

namespace Mkb.DapperRepo.Tests.Tests.Repo.GetAll
{
    public class GetAllTests : BaseSyncTestClass
    {
        public GetAllTests() : base($"{nameof(GetAllTests)}{RandomChars}")
        {
        }

        [Test]
        public void Ensure_we_get_all_records_back()
        {
            var testTableItems = new[]
            {
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid().ToString("N"), Name = "Michale", SomeNumber = 33},
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid().ToString("N"), Name = "othername", SomeNumber = 1}
            };
            DataBaseScriptRunnerAndBuilder.InsertTableWithNoAutoGeneratedPrimaryKey(Connection, testTableItems);

            var items = Sut.GetAll<TableWithNoAutoGeneratedPrimaryKey>().ToArray();

            Assert.AreEqual(testTableItems.Length, items.Length);
            foreach (var item in testTableItems)
            {
                var test = items.FirstOrDefault(x => x.Id == item.Id);

                Assert.IsNotNull(test);
                Assert.AreEqual(item.Name, test.Name);
                Assert.AreEqual(item.SomeNumber, test.SomeNumber);
            }
        }

        [Test]
        public void Ensure_we_get_all_records_back_with_diff_name()
        {
            var testTableItems = new[]
            {
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid().ToString("N"), Name = "Michale", SomeNumber = 33},
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid().ToString("N"), Name = "othername", SomeNumber = 1}
            };

            DataBaseScriptRunnerAndBuilder.InsertTableWithNoAutoGeneratedPrimaryKey(Connection, testTableItems);

            var items = Sut.GetAll<TableWithNoAutoGeneratedPrimaryKeyDiffSqlName>().ToArray();

            Assert.AreEqual(testTableItems.Length, items.Length);
            foreach (var item in testTableItems)
            {
                var test = items.FirstOrDefault(x => x.Id == item.Id);

                Assert.IsNotNull(test);
                Assert.AreEqual(item.Name, test.Name);
                Assert.AreEqual(item.SomeNumber, test.SomeNumber);
            }
        }

        [Test]
        public void Ensure_if_we_have_no_records_we_do_not_blow_up()
        {
            var items = (Sut.GetAll<TableWithNoAutoGeneratedPrimaryKey>()).ToArray();
            Assert.AreEqual(0, items.Length);
        }
    }
}