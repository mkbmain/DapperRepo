# DapperRepo
Special Thanks to armanx

Simple dapper repo with async implementation aswell no interface base to build on all work to create the table for Runner

## creating new entities
please note the primary key attribute found in DapperRepo.PrimaryKeyAttribute.cs

```[SqlTableName("Test_Table")]         // can be used to get around pluralization and also unconventional table names  
    public class TestTable
    {
        [PrimaryKey] // Required for repo to work effectively
        public Guid Id { get; set; }

        public string Name { get; set; }
        public int SomeNumber { get; set; }
    }
```

## Tests
Connection.cs contains connection config these are not unit tests they do require a db and will spin one up on fly but for a repo seems better than mocking
```
   public static string MasterConnectionString = "Server=localhost;Database=master;Trusted_Connection=True";
```

also please note there are separate getall and insert methods that are raw in  DataBaseScriptRunnerAndBuilder.cs



Located in CreateDbWithTestTable.Sql you will find the create database scripts


