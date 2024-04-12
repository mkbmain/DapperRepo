using System;
using System.Linq;
using Mkb.DapperRepo.Tests.Entities;
using Mkb.DapperRepo.Tests.Tests.BaseTestClasses;
using Mkb.DapperRepo.Tests.Utils;
using Xunit;

namespace Mkb.DapperRepo.Tests.Tests.Repo.ExecuteTests;

[Collection("Integration")]
public class ExecuteTest : BaseSyncTestClass
{
    [Fact]
    public void Ensure_we_Execute()
    {
        const string Name = "blog";
        var testTableItem = new TableWithNoAutoGeneratedPrimaryKey
            { Id = Guid.NewGuid().ToString("N"), NameTest = "Michale", SomeNumber = 33 };
        DataBaseScriptRunnerAndBuilder.InsertTableWithNoAutoGeneratedPrimaryKey(Connection, new[] { testTableItem });
        Sut.Execute($"Update TableWithNoAutoGeneratedPrimaryKey set Name = '{Name}' where  SomeNumber = 33");

        Reflection.ReflectionUtils
            .GetEntityPropertyInfo<TableWithNoAutoGeneratedPrimaryKey>(); // slightly bad but mapping is handy

        var records = DataBaseScriptRunnerAndBuilder.GetAll<TableWithNoAutoGeneratedPrimaryKey>(Connection)
            .ToArray();
        Assert.Single(records);
        Assert.Equal(testTableItem.Id, records.First().Id);
        Assert.Equal(testTableItem.SomeNumber, records.First().SomeNumber);
        Assert.Equal(Name, records.First().NameTest);
    }

    [Fact]
    public void Ensure_we_Execute_T_Type()
    {
        const string Name = "blog";
        var testTableItem = new TableWithNoAutoGeneratedPrimaryKey
            { Id = Guid.NewGuid().ToString("N"), NameTest = "Michale", SomeNumber = 33 };
        DataBaseScriptRunnerAndBuilder.InsertTableWithNoAutoGeneratedPrimaryKey(Connection, new[] { testTableItem });
        testTableItem.NameTest = Name;
        Sut.Execute(testTableItem,
            $"Update TableWithNoAutoGeneratedPrimaryKey set Name = @NameTest where  SomeNumber = 33");

        Reflection.ReflectionUtils
            .GetEntityPropertyInfo<TableWithNoAutoGeneratedPrimaryKey>(); // slightly bad but mapping is handy

        var records = DataBaseScriptRunnerAndBuilder.GetAll<TableWithNoAutoGeneratedPrimaryKey>(Connection)
            .ToArray();
        Assert.Single(records);
        Assert.Equal(testTableItem.Id, records.First().Id);
        Assert.Equal(testTableItem.SomeNumber, records.First().SomeNumber);
        Assert.Equal(Name, records.First().NameTest);
    }
}