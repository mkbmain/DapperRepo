namespace Mkb.DapperRepo.Tests
{
    public static class Connection
    {
#if MYSQL
        public const string MasterConnectionString = "Server=localhost;Database=master;Uid=root;Pwd=A1234567a;"; // MYSQL
        public const Environment SelectedEnvironment = Environment.MySql;
#elif POSTGRES
          public const string MasterConnectionString = "User ID=postgres;Password=A1234567a;Host=localhost;Port=5432;Database=master;"; // Postgres
          public const Environment SelectedEnvironment = Environment.PostgreSQL;
#elif SQLSERVER
        public const string MasterConnectionString = "Server=localhost;Database=master;User Id=sa;Password=A1234567a;TrustServerCertificate=true"; // SQL
        public const Environment SelectedEnvironment = Environment.Sql;
#else
        public const string MasterConnectionString = "Data Source=master.sqlite"; // Sqlite
        public const Environment SelectedEnvironment = Environment.Sqlite;
#endif
    }

    public enum Environment
    {
        Sql,
        Sqlite,
        MySql,
        PostgreSQL
    }
}