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

        protected string DbName;

        protected string _connection => Connection.MasterConnectionString.Replace("master", DbName);

        [SetUp]
        public void Setup()
        {
            DataBaseScriptRunnerAndBuilder.RunDb(Connection.MasterConnectionString, DbName, PathBuilder.BuildSqlScriptLocation("CreateDbWithTestTable.Sql"));
        }

        [TearDown]
        public void TearDown()
        {
            DataBaseScriptRunnerAndBuilder.KillDb(Connection.MasterConnectionString, DbName);
        }
    }
}