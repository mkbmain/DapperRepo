using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using DapperRepoTests.Entities;

namespace DapperRepoTests.Utils
{
    public class DataBaseScriptRunnerAndBuilder
    {
        public static void InsertTestTables(string connection, IEnumerable<TestTable> testTables)
        {
            var sql =
                $"Insert into TestTable (id,name,SomeNumber) values{string.Join(",", testTables.Select(f => $"('{f.Id}','{f.Name}',{f.SomeNumber})"))}";
            ExecuteCommandNonQuery(connection, sql);
        }

        public static void RunDb(string connectionToMaster, string dbName, string scriptLocation)
        {
            var sql = File.ReadAllText(scriptLocation).Replace("PlaceHolderDbName", dbName);
            foreach (var item in sql.Split("{WaitBlock}"))
            {
                // this might be come a bottlekneck as we create a new connection for every command but hay its tests should not be doing anything massive
                ExecuteCommandNonQuery(connectionToMaster, item);
            }
        }

        public static void KillDb(string connectionToMaster, string dbName)
        {
            ExecuteCommandNonQuery(connectionToMaster, $"ALTER DATABASE [{dbName}] SET  SINGLE_USER WITH ROLLBACK IMMEDIATE{Environment.NewLine}DROP DATABASE {dbName}");
        }
        
        private static void ExecuteCommandNonQuery(string connection, string sql)
        {
            using var conn = new SqlConnection(connection);
            conn.Open();
            var sqlCommand = new SqlCommand(sql, conn);
            sqlCommand.ExecuteNonQuery();
        }

        public static IEnumerable<T> GetAll<T>(string connection) where T : class, new()
        {
            using var conn = new SqlConnection(connection);
            conn.Open();
            var sqlCommand = new SqlCommand($"select * from {typeof(T).Name}", conn);
            var sqlDataReader = sqlCommand.ExecuteReader();
            var items = new List<T>();
            foreach (var record in sqlDataReader)
            {
                var dataRecord = record as DbDataRecord;
                if (dataRecord == null)
                {
                    continue;
                }

                var item = Activator.CreateInstance<T>();
                for (int i = 0; i < dataRecord.FieldCount; i++)
                {
                    if (dataRecord[i] is DBNull)
                    {
                        continue;
                    }

                    System.Reflection.PropertyInfo propertyInfo = item.GetType().GetProperty(dataRecord.GetName(i));
                    if(propertyInfo == null){continue;}
                    propertyInfo.SetValue(item, Convert.ChangeType(dataRecord[i], propertyInfo.PropertyType), null);
                }

                items.Add(item);
            }

            return items;
        }
        
    }
}