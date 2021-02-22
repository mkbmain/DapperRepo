# DapperRepo
Special Thanks to armanx

Simple dapper repo with async implementation aswell no interface base to build on all work to create the table for Runner 


# Sql
connection string is located in Program.cs in main as a const 
Also for tests is located in test project on Connection.cs
``` c#  
  const string connection = "Server=localhost;Database=test;Trusted_Connection=True";
 ```

Create a empty table you can name it test to match connection string or your own name if you wish to use the mode i used 

``` sql

CREATE TABLE [dbo].[TestTable](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [varchar](50) NULL,
	[SomeNumber] [int] NULL,
 CONSTRAINT [PK_TestTable] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```


## creating new entities
please note if you make your own the primary key attribute found in DapperRepo.PrimaryKeyAttribute.cs


## Tests
Connection.cs contains connection config these are not unit tests they do require a db and will spin one up on fly but for a repo seems better than mocking

also please note there are separate getall and insert methods that are raw in  DataBaseScriptRunnerAndBuilder.cs