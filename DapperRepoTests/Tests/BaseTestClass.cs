using DapperRepo.Repo;

namespace DapperRepoTests.Tests
{
    public class ConnectionRawBase
    {
        protected virtual string _connection => "";
    }

    public class BaseTestClass : ConnectionRawBase
    {
        protected SqlRepo SUT;

        public BaseTestClass()
        {
            SUT = new SqlRepo(_connection);
        }
    }

    public class BaseTestClassAsync : ConnectionRawBase
    {
        protected SqlRepoAsync SUT;

        public BaseTestClassAsync()
        {
            SUT = new SqlRepoAsync(_connection);
        }
    }
}