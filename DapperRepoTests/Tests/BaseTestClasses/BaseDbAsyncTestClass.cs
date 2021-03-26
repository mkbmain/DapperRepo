using DapperRepo.Repo;

namespace DapperRepoTests.Tests.BaseTestClasses
{
    public abstract class BaseAsyncTestClass : BaseDbSetupTestClass
    {
        protected SqlRepoAsync Sut => new SqlRepoAsync(Connection);

        protected BaseAsyncTestClass(string dbName) : base(dbName)
        {
        }
    }
}