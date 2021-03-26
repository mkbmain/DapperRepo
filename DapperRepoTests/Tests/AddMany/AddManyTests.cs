using System;
using System.Diagnostics;
using System.Linq;
using DapperRepoTests.Entities;
using DapperRepoTests.Tests.BaseTestClasses;
using DapperRepoTests.Utils;
using NUnit.Framework;

namespace DapperRepoTests.Tests.AddMany
{
    public class AddManyTests : BaseSyncTestClass
    {
        public AddManyTests() : base($"{nameof(AddManyTests)}{RandomChars}")
        {
        }


        [Test]
        public void Ensure_we_can_add_multiple_records()
        {
            var testTableItems = new[]
            {
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid(), Name = "Michale", SomeNumber = 33},
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid(), Name = "othername", SomeNumber = 1}
            };

            Sut.AddMany(testTableItems);

            var result = DataBaseScriptRunnerAndBuilder.GetAll<TableWithNoAutoGeneratedPrimaryKey>(Connection);
            var items = result as TableWithNoAutoGeneratedPrimaryKey[] ?? result.ToArray();

            Assert.AreEqual(testTableItems.Length, items.Length);
            foreach (var item in testTableItems)
            {
                var test = items.FirstOrDefault(x => x.Id == item.Id);

                Assert.IsNotNull(test);
                Assert.AreEqual(item.Name, test.Name);
                Assert.AreEqual(item.SomeNumber, test.SomeNumber);
            }
        }

        private static Random _random = new Random(Guid.NewGuid().GetHashCode());
        
        [Test]
        public void Ensure_we_can_add_multiple_records_auto_increment()
        {
            var testTableItems = new[]
            {
                new TableWithAutIncrementPrimaryKey {Name = "Michale", SomeNumber = 33},
                new TableWithAutIncrementPrimaryKey {Name = "othername"}
            };

            Sut.AddMany(testTableItems);

            var result = DataBaseScriptRunnerAndBuilder.GetAll<TableWithAutIncrementPrimaryKey>(Connection);
            var items = result as TableWithAutIncrementPrimaryKey[] ?? result.ToArray();

            Assert.AreEqual(testTableItems.Length, items.Length);

            Assert.IsTrue(items.Any(x =>
                x.Id != null && x.Name == testTableItems.LastOrDefault().Name && x.SomeNumber == null));
            Assert.IsTrue(items.Any(x =>
                x.Id != null && x.SomeNumber == testTableItems.First().SomeNumber &&
                x.Name == testTableItems.First().Name));
        }
    }
}