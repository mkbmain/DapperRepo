
create database PlaceHolderDbName

{WaitBlock}
USE [PlaceHolderDbName]


CREATE TABLE [dbo].[TestTable](
    [Id] [uniqueidentifier] NOT NULL,
    [Name] [varchar](50) NULL,
    [SomeNumber] [int] NULL,
    CONSTRAINT [PK_TestTable] PRIMARY KEY CLUSTERED
(
[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]

