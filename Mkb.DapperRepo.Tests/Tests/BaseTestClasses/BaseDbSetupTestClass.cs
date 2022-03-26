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
            ClassName = this.GetType().Name;
        }

        private static string RandomChars => Guid.NewGuid().ToString("N").Substring(0, 6);
        protected string DbName;
        protected string ClassName;

        protected string Connection => DapperRepo.Tests.Connection.MasterConnectionString.Replace("master", DbName);

        [SetUp]
        public void Setup()
        {
            DbName = (ClassName + RandomChars).ToLower();
            var scriptLocation = PathBuilder.BuildSqlScriptLocation($"CreateDbWithTestTable.{DapperRepo.Tests.Connection.SelectedEnvironment.ToString()}");
            
            var sql = File.ReadAllText(scriptLocation).Replace("PlaceHolderDbName", DbName);
            bool first = true;

            foreach (var item in sql.Split("{WaitBlock}"))
            {
                // this might be come a bottlekneck as we create a new connection for every command but hay its tests should not be doing anything massive
               DataBaseScriptRunnerAndBuilder. ExecuteCommandNonQuery(first? DapperRepo.Tests.Connection.MasterConnectionString : Connection, item);
               first = false;
            }
        }

        [TearDown]
        public void TearDown()
        {
            
            DataBaseScriptRunnerAndBuilder.KillDb( DapperRepo.Tests.Connection.MasterConnectionString, DbName);
        }
    }
}