using System;
using DapperRepoTests.Utils;
using NUnit.Framework;

namespace DapperRepoTests.Tests.BaseTestClasses
{
    public abstract class BaseDbSetupTestClass
    {
        public BaseDbSetupTestClass(string dbName)
        {
            DbName = dbName;
        }

        protected static string RandomChars => Guid.NewGuid().ToString("N").Substring(0, 6);
        protected string DbName;

        protected string Connection => DapperRepoTests.Connection.MasterConnectionString.Replace("master", DbName);

        [SetUp]
        public void Setup()
        {
            DataBaseScriptRunnerAndBuilder.RunDb(DapperRepoTests.Connection.MasterConnectionString, DbName, PathBuilder.BuildSqlScriptLocation("CreateDbWithTestTable.Sql"));
        }

        [TearDown]
        public void TearDown()
        {
            DataBaseScriptRunnerAndBuilder.KillDb(DapperRepoTests.Connection.MasterConnectionString, DbName);
        }
    }
}