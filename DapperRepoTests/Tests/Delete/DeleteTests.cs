using System;
using System.Linq;
using System.Threading.Tasks;
using DapperRepoTests.Entities;
using DapperRepoTests.Utils;
using NUnit.Framework;

namespace DapperRepoTests.Tests.Delete
{
    public class DeleteTest : BaseTestClass
    {
        private static string dbName = $"DeleteDapperRepoTests{Guid.NewGuid().ToString("N").Substring(0, 5)}";
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
        public void Ensure_we_can_Delete_a_record()
        {
            var testTableItem = new TestTable {Id = Guid.NewGuid(), Name = "Michale", SomeNumber = 33};
            var dontTouch = new TestTable {Id = Guid.NewGuid(), Name = "gwgw", SomeNumber = 12};
            DataBaseScriptRunnerAndBuilder.InsertTestTables(_connection, new[] {testTableItem, dontTouch});
            SUT.Delete(testTableItem);

            var records = DataBaseScriptRunnerAndBuilder.GetAll<TestTable>(_connection);

            Assert.AreEqual(1, records.Count());
            Assert.AreEqual(dontTouch.Id, records.First().Id);
            Assert.AreEqual(dontTouch.SomeNumber, records.First().SomeNumber);
            Assert.AreEqual(dontTouch.Name, records.First().Name);
        }
    }
}