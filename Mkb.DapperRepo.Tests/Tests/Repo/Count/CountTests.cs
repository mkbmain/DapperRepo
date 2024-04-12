using System;
using Mkb.DapperRepo.Tests.Entities;
using Mkb.DapperRepo.Tests.Tests.BaseTestClasses;
using Mkb.DapperRepo.Tests.Utils;
using Xunit;

namespace Mkb.DapperRepo.Tests.Tests.Repo.Count;

[Collection("Integration")]
public class CountTests : BaseSyncTestClass
{
    [Fact]
    public void Ensure_we_get_all_records_back()
    {
        var testTableItems = new[]
        {
            new TableWithNoAutoGeneratedPrimaryKey { Id = Guid.NewGuid().ToString("N"), NameTest = "Michale", SomeNumber = 33 },
            new TableWithNoAutoGeneratedPrimaryKey { Id = Guid.NewGuid().ToString("N"), NameTest = "othername", SomeNumber = 1 }
        };

        DataBaseScriptRunnerAndBuilder.InsertTableWithNoAutoGeneratedPrimaryKey(Connection, testTableItems);

        var count = Sut.Count<TableWithNoAutoGeneratedPrimaryKey>();

        Assert.Equal(testTableItems.Length, count);
    }

    [Fact]
    public void Ensure_we_get_all_records_back_with_diff_name()
    {
        var testTableItems = new[]
        {
            new TableWithNoAutoGeneratedPrimaryKey { Id = Guid.NewGuid().ToString("N"), NameTest = "Michale", SomeNumber = 33 },
            new TableWithNoAutoGeneratedPrimaryKey { Id = Guid.NewGuid().ToString("N"), NameTest = "othername", SomeNumber = 1 }
        };

        DataBaseScriptRunnerAndBuilder.InsertTableWithNoAutoGeneratedPrimaryKey(Connection, testTableItems);

        var count = Sut.Count<TableWithNoAutoGeneratedPrimaryKey>();

        Assert.Equal(testTableItems.Length, count);
    }

    [Fact]
    public void Ensure_if_we_have_no_records_we_do_not_blow_up()
    {
        var count = Sut.Count<TableWithNoAutoGeneratedPrimaryKey>();
        Assert.Equal(0, count);
    }
}