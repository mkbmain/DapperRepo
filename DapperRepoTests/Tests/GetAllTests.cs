using System;
using System.Linq;
using DapperRepoTests.Entities;
using DapperRepoTests.Utils;
using NUnit.Framework;

namespace DapperRepoTests.Tests
{
    public class GetAllTests : BaseTestClass
    {
        private static string dbName = $"GetAllDapperRepoTests{Guid.NewGuid().ToString("N").Substring(0, 5)}";
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
        public void Ensure_we_get_all_records_back()
        {
            var testTableItems = new[]
            {
                new TestTable {Id = Guid.NewGuid(), Name = "Michale", SomeNumber = 33},
                new TestTable {Id = Guid.NewGuid(), Name = "othername", SomeNumber = 1}
            };
            DataBaseScriptRunnerAndBuilder.InsertTestTables(_connection, testTableItems);

            var items = SUT.GetAll<TestTable>().ToArray();

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
            var items = ( SUT.GetAll<TestTable>()).ToArray();
            Assert.AreEqual(0, items.Length);
        }
    }
}