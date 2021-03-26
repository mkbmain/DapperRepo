using DapperRepo.Repo;

namespace DapperRepoTests.Tests.BaseTestClasses
{
    public abstract class BaseSyncTestClass : BaseDbSetupTestClass
    {
        protected SqlRepo Sut => new SqlRepo(Connection);

        protected BaseSyncTestClass(string dbName) : base(dbName)
        {
        }
    }
}