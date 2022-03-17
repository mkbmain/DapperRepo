using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Microsoft.Data.Sqlite;
using Mkb.DapperRepo.Tests.Entities;
using MySql.Data.MySqlClient;
using Npgsql;

namespace Mkb.DapperRepo.Tests.Utils
{
    public static class DataBaseScriptRunnerAndBuilder
    {
        public static void InsertTableWithNoAutoGeneratedPrimaryKey(string connection,
            IEnumerable<TableWithNoAutoGeneratedPrimaryKey> testTables)
        {
            var sql =
                $"Insert into {nameof(TableWithNoAutoGeneratedPrimaryKey)} (id,name,SomeNumber) values{string.Join(",", testTables.Select(f => $"('{f.Id}','{f.Name}',{f.SomeNumber})"))}";
            ExecuteCommandNonQuery(connection, sql);
        }

        public static void InsertTableWithAutoGeneratedPrimaryKey(string connection,
            IEnumerable<TableWithAutoIncrementPrimaryKey> testTables)
        {
            var sql =
                $"Insert into {nameof(TableWithAutoIncrementPrimaryKey)} (name,SomeNumber) values{string.Join(",", testTables.Select(f => $"('{f.Name}',{f.SomeNumber})"))}";
            ExecuteCommandNonQuery(connection, sql);
        }



        public static void KillDb(string connectionToMaster, string dbName)
        {
            if (Connection.SelectedEnvironment == Enviroment.SqlLite)
            {
                // this does not appear to work but rebuild solves it so meh
                var path = System.IO.Path.Combine(Environment.CurrentDirectory, dbName);
                System.IO.File.Delete( path);
                return;
            }
            string start = Connection.SelectedEnvironment == Enviroment.Sql
                ? $"ALTER DATABASE [{dbName}] SET  SINGLE_USER WITH ROLLBACK IMMEDIATE"
                : "";
            if (Connection.SelectedEnvironment != Enviroment.PostgreSQL)
            {
                ExecuteCommandNonQuery(connectionToMaster, $"{start}{Environment.NewLine}DROP DATABASE {dbName}");
                return;
            }
          
            ExecuteCommandNonQuery(connectionToMaster,$"REVOKE CONNECT ON DATABASE {dbName} FROM public;");
            ExecuteCommandNonQuery(connectionToMaster,@$"SELECT pg_terminate_backend(pg_stat_activity.pid)
            FROM pg_stat_activity
            WHERE pg_stat_activity.datname = '{dbName}';");
            ExecuteCommandNonQuery(connectionToMaster, $"DROP DATABASE {dbName};");

        }

        public static void ExecuteCommandNonQuery(string connection, string sql)
        {
            if(string.IsNullOrWhiteSpace(sql)){return;}
            using var conn = GetConnection(connection);
            conn.Open();
            conn.Execute(sql);
        }

        public static DbConnection GetConnection(string connection)
        {
            switch (DapperRepo.Tests.Connection.SelectedEnvironment)
            {
                case Enviroment.MySql:
                    return new MySqlConnection(connection);
                case Enviroment.PostgreSQL:
                    return new NpgsqlConnection(connection);
                case Enviroment.SqlLite:
                    return new  SqliteConnection(connection);
                case Enviroment.Sql:
                default:
                    return new SqlConnection(connection);
            }
        }

        public static IEnumerable<T> GetAll<T>(string connection) where T : class, new()
        {
            using var conn = GetConnection(connection);
            conn.Open();
            var items =   conn.Query<T>($"select * from {typeof(T).Name}");
            return items;
        }
    }
}