using System;
using System.Linq;
using DapperRepoTests.Entities;
using DapperRepoTests.Utils;
using NUnit.Framework;

namespace DapperRepoTests.Tests.GetById
{
    public class GetByIdTest : BaseTestClass
    {
        private static string dbName = $"GetByIdDapperRepoTests{Guid.NewGuid().ToString("N").Substring(0, 5)}";
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
        public void Ensure_we_Can_Get_correct_record_Back()
        {
            var testTableItems = new[]
            {
                new TestTable {Id = Guid.NewGuid(), Name = "Michale", SomeNumber = 33},
                new TestTable {Id = Guid.NewGuid(), Name = "othername", SomeNumber = 1}
            };
            DataBaseScriptRunnerAndBuilder.InsertTestTables(_connection, testTableItems);


            var item = SUT.GetById<TestTable>(testTableItems.First().Id);
            Assert.IsNotNull(item);
            Assert.AreEqual(testTableItems.First().Id, item.Id);
            Assert.AreEqual(testTableItems.First().SomeNumber, item.SomeNumber);
            Assert.AreEqual(testTableItems.First().Name, item.Name);
        }
        
        [Test]
        public void Ensure_if_no_record_found_we_return_null()
        {
            var testTableItems = new[]
            {
                new TestTable {Id = Guid.NewGuid(), Name = "Michale", SomeNumber = 33},
                new TestTable {Id = Guid.NewGuid(), Name = "othername", SomeNumber = 1}
            };
            
            DataBaseScriptRunnerAndBuilder.InsertTestTables(_connection, testTableItems);

            var item = SUT.GetById<TestTable>(Guid.NewGuid());
            Assert.IsNull(item);
        }
    }
}