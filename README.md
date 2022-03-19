# DapperRepo

Simple dapper repo with async implementation.
<br>designed to generate simple crud operations while still being lite weight

## Currently supported and tested for these providers

| Provider   | Db Connection    | Nuget Package         |
|------------|------------------|-----------------------|
| Sql        | SqlConnection    | System.Data.SqlClient |
| MySql      | MySqlConnection  | MySql.Data            |
| PostgreSQL | NpgsqlConnection | Npgsql                |
| Sqlite     | SqliteConnection | Microsoft.Data.Sqlite |

It may work for others. <br>
(sql lite I recently added, I made no changes to the repo just added test setup to confirm it worked).

Please note due to wanting to support multiple Db providers certain choices have been made that are for compatibility over optimisation.
The repo its self has no scope over any provider (nor should it). 


# Setup

All examples are done with the standard repo but will work with the async version to (will need to be awaited).
Full examples can be found on the github repo with in the test project.

## Initializing a new repo
```
  new SqlRepo(()=> new DbConnection());
  
  e.g
  new SqlRepo(()=> new SqlConnection("connection string"));     // MsSql
  new SqlRepo(()=> new MySqlConnection("connection string"));   // MySql
  new SqlRepo(()=> new NpgsqlConnection("connection string"));  // PostgreSQL
  new SqlRepo(()=> new SqliteConnection("connection string"));  // Sqlite
  ```
or of course via DI
```
  
  // or where ever you want to get the connection string from it from
  // you can ofc use Scoped or Single instead if you wish but depends on sql implementation and how it handles connections
  // would check dapper and sql providers docs for best practice per implementation 
  
  services.Configure<ConnectionStrings>(Configuration.GetSection(nameof(ConnectionStrings)));
  services.AddTransient(r => new SqlConnection(r.GetService<IOptions<ConnectionStrings>>().Value.SqlDb));
  services.AddScoped(r => new SqlRepoAsync(r.GetService<SqlConnection>));
  
```
by taking a DbConnection directly it does support multiple providers. Allowing a abstract way to interact with data. 
Regardless of the db tech.



# Usages

## Creating entities
please note the primary key attribute found in DapperRepo.PrimaryKeyAttribute.cs

```
    [SqlTableName("Test_Table")]         // can be used to get around pluralization and also unconventional table names  
    public class TableModel
    {
        [PrimaryKey] // Required for repo to work effectively
        public Guid Id { get; set; }
// if a field is auto generated by the db you can set it to be nullable i.e int? 
// if value is null unless told repo will not try to insert null fields but update will this can be overriden using ignoreNullProperties on the update method 

        public string Name { get; set; }
        public string Email { get; set; }
        public int SomeNumber { get; set; }
    }
```

## Search
```
Repo.Search<TableModel>("Name", "%cha%"); 

Repo.Search<TableModel>(nameof(TableModel.Name), "%cha%");    // recommended 

and search multiple criteria

 Repo.Search(new TableModel
            {
                Name = "%hae%",
                SomeNumber = 9
            }, new[]
            {
                new SearchCriteria
                {
                    PropertyName = nameof(TableWithNoAutoGeneratedPrimaryKey.SomeNumber),
                    SearchType = SearchType.LessThanEqualTo
                },
                new SearchCriteria
                {
                    PropertyName = nameof(TableWithNoAutoGeneratedPrimaryKey.Name),
                    SearchType = SearchType.Like
                },
            });
            
           // this will allow you to search where name like "%hae%" 
           // and SomeNumber less than equal to 9
            
```

## GetAllByX
```
Repo.GetAllByX<TableModel, int>("SomeNumber", 35);

Repo.GetAllByX<TableModel, int>(nameof(TableModel.SomeNumber), 35);
```

## Query single
```
Repo.QuerySingle<TableModel>(
                "select * from TableModel where SomeNumber = 33");
```


## Query Multiple
```
Repo.QueryMany<TableModel>(
                "select * from TableModel where SomeNumber = 33");
```

## Get By Id
```
Repo.GetById(new TableWithGuid {Id =  Guid.Parse("....")});
Repo.GetById(new TableWithInt {Id =  325});
```

## Get All
```
Repo.GetAll<TableModel>().ToArray();
```

## Delete
```
var item =Repo.GetById(new TableModel {Id =  325});
Repo.Delete(item);
```


## Add
```
  var testTableItem = new TableModel() {Name = "Michael", SomeNumber = 44};
  Repo.Add(testTableItem);
```

## Update
```
 update command is built from the primary get so strictly speaking if you wish to update all the values on a row and know its primary key a get is not required

 var item = Repo.GetById(new TableModel {Id =  325});
 item.Name = "mike"
 item.Email = null
 Repo.Update(testTableItem); // will update properites and also null properties so db will set email to null

 // Example 2
 var item = Repo.GetById(new TableModel {Id =  325});
 item.Name = "mike"
 item.Email = null
 
 Repo.Update(testTableItem, true);   // will ignore null properties and only update name in db
```


### Getting Tests working
Connection.cs contains connection config these are not unit tests they do require a db and will spin one up on fly but for a repo seems better than mocking
```
   public static string MasterConnectionString = "Server=localhost;Database=master;Trusted_Connection=True";
```

also please note there are separate getall and insert methods that are raw in  DataBaseScriptRunnerAndBuilder.cs as the tests don't use the implemenation they are are testing for setups or verifies.



Located in CreateDbWithTestTable.Sql you will find the create database scripts.




Special Thanks to armanx
