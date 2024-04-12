using Mkb.DapperRepo.Repo;
using Mkb.DapperRepo.Tests.Utils;
using Xunit;

namespace Mkb.DapperRepo.Tests.Tests.BaseTestClasses
{
    public abstract class BaseSyncTestClass : BaseDbSetupTestClass
    {
        protected SqlRepo Sut => new (()=> DataBaseScriptRunnerAndBuilder.GetConnection(Connection));
    }
}