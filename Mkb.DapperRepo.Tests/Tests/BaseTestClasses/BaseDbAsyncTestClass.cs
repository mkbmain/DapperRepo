using Mkb.DapperRepo.Repo;
using Mkb.DapperRepo.Tests.Utils;

namespace Mkb.DapperRepo.Tests.Tests.BaseTestClasses
{
    public abstract class BaseAsyncTestClass : BaseDbSetupTestClass
    {
        protected SqlRepoAsync Sut => new(()=> DataBaseScriptRunnerAndBuilder.GetConnection(Connection));
    }
}