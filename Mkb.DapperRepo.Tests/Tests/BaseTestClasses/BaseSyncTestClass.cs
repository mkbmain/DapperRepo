using Mkb.DapperRepo.Repo;
using Mkb.DapperRepo.Tests.Utils;

namespace Mkb.DapperRepo.Tests.Tests.BaseTestClasses
{
    public abstract class BaseSyncTestClass : BaseDbSetupTestClass
    {
        protected SqlRepo Sut => new SqlRepo(()=> DataBaseScriptRunnerAndBuilder.GetConnection(Connection));
    }
}