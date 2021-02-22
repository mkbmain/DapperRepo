using System;
using System.Linq;
using DapperRepoTests.Entities;
using DapperRepoTests.Tests.BaseTestClasses;
using DapperRepoTests.Utils;
using NUnit.Framework;

namespace DapperRepoTests.Tests.Add
{
    public class AddDbSyncTests : BaseDbSyncTestClass
    {
        public AddDbSyncTests() : base( $"AddDapperRepoTests{Guid.NewGuid().ToString("N").Substring(0, 5)}")
        {
        }

        [Test]
        public void Ensure_we_can_add()
        {
            var testTableItem = new TestTable {Id = Guid.NewGuid(), Name = "Michale", SomeNumber = 33};
            Sut.Add(testTableItem);

            var result = DataBaseScriptRunnerAndBuilder.GetAll<TestTable>(_connection);
            var items = result as TestTable[] ?? result.ToArray();

            Assert.AreEqual(1, items.Length);
            var test = items.FirstOrDefault();
            Assert.AreEqual(testTableItem.Id, test.Id);
            Assert.AreEqual(testTableItem.Name, test.Name);
            Assert.AreEqual(testTableItem.SomeNumber, test.SomeNumber);
        }
    }
}