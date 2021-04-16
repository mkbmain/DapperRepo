using Mkb.DapperRepo.Repo;

namespace Mkb.DapperRepo.Tests.Tests.BaseTestClasses
{
    public abstract class BaseAsyncTestClass : BaseDbSetupTestClass
    {
        protected SqlRepoAsync Sut => new SqlRepoAsync(Connection);

        protected BaseAsyncTestClass(string dbName) : base(dbName)
        {
        }
    }
}