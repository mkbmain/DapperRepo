using System;
using System.Linq;
using System.Threading.Tasks;
using DapperRepoTests.Entities;
using DapperRepoTests.Utils;
using NUnit.Framework;

namespace DapperRepoTests.Tests
{
    public class AddAsyncTests : BaseTestClassAsync
    {
        private static string dbName = $"AddAsyncDapperRepoTests{Guid.NewGuid().ToString("N").Substring(0, 5)}";
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
        public async Task Ensure_we_can_add()
        {
            var testTableItem = new TestTable {Id = Guid.NewGuid(), Name = "Michale", SomeNumber = 33};
            await SUT.Add(testTableItem);

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