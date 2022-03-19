using System.Data.SqlClient;
using Mkb.DapperRepo.Repo;
using Mkb.DapperRepo.Tests.Utils;
using MySql.Data.MySqlClient;

namespace Mkb.DapperRepo.Tests.Tests.BaseTestClasses
{
    public abstract class BaseSyncTestClass : BaseDbSetupTestClass
    {
        protected SqlRepo Sut
        {
            get
            {
                return new SqlRepo(()=> DataBaseScriptRunnerAndBuilder.GetConnection(Connection));
            }
        }
        
    }
}