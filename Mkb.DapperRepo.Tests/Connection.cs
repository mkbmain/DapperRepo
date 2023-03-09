namespace Mkb.DapperRepo.Tests
{
    public static class Connection
    {
#if MYSQL
        public const string MasterConnectionString = "Server=localhost;Database=master;Uid=root;Pwd=A1234567a;"; // MYSQL
        public static Enviroment SelectedEnvironment = Enviroment.MySql;
#elif POSTGRES
          public const string MasterConnectionString = "User ID=postgres;Password=A1234567a;Host=localhost;Port=5432;Database=master;"; // Postgres
                public static Enviroment SelectedEnvironment = Enviroment.PostgreSQL;
#elif SQLSERVER
        public const string MasterConnectionString = "Server=localhost;Database=master;User Id=sa;Password=A1234567a;"; // SQL
        public static Enviroment SelectedEnvironment = Enviroment.Sql;
#else
        public const string MasterConnectionString = "Data Source=master.sqlite"; // Sqlite
        public static Enviroment SelectedEnvironment = Enviroment.Sqlite;
#endif
    }

    public enum Enviroment
    {
        Sql,
        Sqlite,
        MySql,
        PostgreSQL
    }
}