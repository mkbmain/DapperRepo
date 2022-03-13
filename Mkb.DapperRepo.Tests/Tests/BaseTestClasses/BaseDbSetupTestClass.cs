using System;
using Mkb.DapperRepo.Tests.Utils;
using NUnit.Framework;

namespace Mkb.DapperRepo.Tests.Tests.BaseTestClasses
{
    public abstract class BaseDbSetupTestClass
    {
        public BaseDbSetupTestClass(string dbName)
        {
            DbName = dbName;
        }

        protected static string RandomChars => Guid.NewGuid().ToString("N").Substring(0, 6);
        protected string DbName;

        protected string Connection => DapperRepo.Tests.Connection.MasterConnectionString.Replace("master", DbName);

        [SetUp]
        public void Setup()
        {
            DataBaseScriptRunnerAndBuilder.RunDb(DapperRepo.Tests.Connection.MasterConnectionString, DbName, PathBuilder.BuildSqlScriptLocation($"CreateDbWithTestTable.{ DapperRepo.Tests.Connection.SelectedEnvironment.ToString()}"));
        }

        [TearDown]
        public void TearDown()
        {
            DataBaseScriptRunnerAndBuilder.KillDb(DapperRepo.Tests.Connection.MasterConnectionString, DbName);
        }
    }
}