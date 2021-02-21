# DapperRepo
Special Thanks to armanx

Simple dapper repo with asyc implementation aswell no interface base to build on all work to create the table for Runner 


# Sql
connection strin is located in Program.cs in main as a const 
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


## creating new entitys
please note if you make your own the primary key attritube found in DapperRepo.PrimaryKeyAttribute.cs
