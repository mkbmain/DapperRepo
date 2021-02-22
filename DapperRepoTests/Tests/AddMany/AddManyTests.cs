using System;
using System.Linq;
using DapperRepoTests.Entities;
using DapperRepoTests.Utils;
using NUnit.Framework;

namespace DapperRepoTests.Tests.AddMany
{
    public class AddManyTests : BaseTestClass
    {
        private static string dbName = $"AddManyDapperRepoTests{Guid.NewGuid().ToString("N").Substring(0, 5)}";
        protected override string _connection => Connection.MasterConnectionString.Replace("master", dbName);

        [SetUp]
        public void Setup()
        {
            DataBaseScriptRunnerAndBuilder.RunDb(Connection.MasterConnectionString, dbName, PathBuilder.BuildSqlScriptLocation("CreateDbWithTestTable.Sql"));
        }

        [TearDown]
        public void TearDown()
        {
            DataBaseScriptRunnerAndBuilder.KillDb(Connection.MasterConnectionString, dbName);
        }

        [Test]
        public void Ensure_we_can_add_multiple_records()
        {
            var testTableItems = new[]
            {
                new TestTable {Id = Guid.NewGuid(), Name = "Michale", SomeNumber = 33},
                new TestTable {Id = Guid.NewGuid(), Name = "othername", SomeNumber = 1}
            };

            SUT.AddMany(testTableItems);

            var result = DataBaseScriptRunnerAndBuilder.GetAll<TestTable>(_connection);  
            var items = result as TestTable[] ?? result.ToArray();

            Assert.AreEqual(testTableItems.Length, items.Length);
            foreach (var item in testTableItems)
            {
                var test = items.FirstOrDefault(x => x.Id == item.Id);

                Assert.IsNotNull(test);
                Assert.AreEqual(item.Name, test.Name);
                Assert.AreEqual(item.SomeNumber, test.SomeNumber);
            }
        }
    }
}