namespace Mkb.DapperRepo.Tests
{
    public static class Connection
    { 
        // public const string MasterConnectionString = "Server=localhost;Database=master;User Id=sa;Password=A1234567a;";  // SQL
        //  public const string MasterConnectionString = "Server=localhost;Database=master;Uid=root;Pwd=A1234567a;"; // MYSQL
        //  public const string MasterConnectionString = "Server=192.168.0.216;Database=master;Uid=sa;Pwd=A1234567a;"; // MYSQL
        //  public const string MasterConnectionString = "User ID=postgres;Password=A1234567a;Host=localhost;Port=5432;Database=master;"; // Postgres
        public const string MasterConnectionString = "Data Source=master.sqlite"; // Sqlite
        public static Enviroment SelectedEnvironment = Enviroment.Sqlite;
    }

    public enum Enviroment
    {
        Sql,
        Sqlite,
        MySql,
        PostgreSQL
    }
}