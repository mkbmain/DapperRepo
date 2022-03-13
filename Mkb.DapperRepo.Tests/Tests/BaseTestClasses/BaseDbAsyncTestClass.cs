using System.Data.SqlClient;
using Mkb.DapperRepo.Repo;
using Mkb.DapperRepo.Tests.Utils;

namespace Mkb.DapperRepo.Tests.Tests.BaseTestClasses
{
    public abstract class BaseAsyncTestClass : BaseDbSetupTestClass
    {
        protected SqlRepoAsync Sut
        {
            get
            {
                return new SqlRepoAsync(()=> DataBaseScriptRunnerAndBuilder.GetConnection(Connection));
            }
        }

        protected BaseAsyncTestClass(string className) : base(className)
        {
        }
    }
}