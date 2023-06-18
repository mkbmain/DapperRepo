using System;
using System.IO;
using Mkb.DapperRepo.Tests.Utils;
using NUnit.Framework;

namespace Mkb.DapperRepo.Tests.Tests.BaseTestClasses
{
    public abstract class BaseDbSetupTestClass
    {
        public BaseDbSetupTestClass()
        {
            _className = this.GetType().Name;
        }

        private static string RandomChars => Guid.NewGuid().ToString("N")[..8];
        private string _dbName;
        private readonly string _className;

        protected string Connection => DapperRepo.Tests.Connection.MasterConnectionString.Replace("master", _dbName);

        [SetUp]
        public void Setup()
        {
            _dbName = (_className + RandomChars).ToLower();
            var scriptLocation = PathBuilder.BuildSqlScriptLocation($"CreateDbWithTestTable.{DapperRepo.Tests.Connection.SelectedEnvironment.ToString()}");
            
            var sql = File.ReadAllText(scriptLocation).Replace("PlaceHolderDbName", _dbName);
            bool first = true;

            foreach (var item in sql.Split("{WaitBlock}"))
            {
                // this might be come a bottlekneck as we create a new connection for every command but hay its tests should not be doing anything massive
               DataBaseScriptRunnerAndBuilder.ExecuteCommandNonQuery(first? DapperRepo.Tests.Connection.MasterConnectionString : Connection, item);
               first = false;
            }
        }

        [TearDown]
        public void TearDown()
        {
            DataBaseScriptRunnerAndBuilder.KillDb(DapperRepo.Tests.Connection.MasterConnectionString, _dbName);
        }
    }
}