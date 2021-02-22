using DapperRepo.Repo;

namespace DapperRepoTests.Tests.BaseTestClasses
{
    public abstract class BaseDbAsyncTestClass : BaseDbSetupTestClass
    {
        protected SqlRepoAsync Sut => new SqlRepoAsync(_connection);

        protected BaseDbAsyncTestClass(string dbName) : base(dbName)
        {
        }
    }
}