using DapperRepo.Repo;

namespace DapperRepoTests.Tests.BaseTestClasses
{
    public abstract class BaseDbSyncTestClass : BaseDbSetupTestClass
    {
        protected SqlRepo Sut => new SqlRepo(Connection);

        protected BaseDbSyncTestClass(string dbName) : base(dbName)
        {
        }
    }
}