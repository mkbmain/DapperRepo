using System;
using System.Collections.Immutable;
using System.Linq;
using Mkb.DapperRepo.Search;
using Mkb.DapperRepo.Tests.Entities;
using Mkb.DapperRepo.Tests.Tests.BaseTestClasses;
using Mkb.DapperRepo.Tests.Utils;
using Xunit;

namespace Mkb.DapperRepo.Tests.Tests.Repo.Search
{
    [Collection("Integration")]
    public class SearchTests : BaseSyncTestClass
    {
        [Fact]
        public void Ensure_we_can_match_null()
        {
            var testTableItems = new[]
            {
                new TableWithNoAutoGeneratedPrimaryKey
                    { Id = Guid.NewGuid().ToString("N"), NameTest = "Michael", SomeNumber = 33 },
                new TableWithNoAutoGeneratedPrimaryKey
                    { Id = Guid.NewGuid().ToString("N"), NameTest = "Michael", SomeNumber = null }
            };

            DataBaseScriptRunnerAndBuilder.InsertTableWithNoAutoGeneratedPrimaryKey(Connection, testTableItems);

            var result = Sut.Search(new TableWithNoAutoGeneratedPrimaryKey { NameTest = "Michael" },
            [
                SearchCriteria.Create(nameof(TableWithNoAutoGeneratedPrimaryKey.NameTest), SearchType.Equals), 
                    SearchCriteria.Create(nameof(TableWithNoAutoGeneratedPrimaryKey.SomeNumber), SearchType.IsNull)
            ]);

            Assert.Single(result);
        }

        [Fact]
        public void Ensure_we_can_match_exact()
        {
            var testTableItems = new[]
            {
                new TableWithNoAutoGeneratedPrimaryKey
                    { Id = Guid.NewGuid().ToString("N"), NameTest = "Michale", SomeNumber = 33 },
                new TableWithNoAutoGeneratedPrimaryKey
                    { Id = Guid.NewGuid().ToString("N"), NameTest = "Michael", SomeNumber = 13 },
                new TableWithNoAutoGeneratedPrimaryKey
                    { Id = Guid.NewGuid().ToString("N"), NameTest = "chaal", SomeNumber = 73 },
                new TableWithNoAutoGeneratedPrimaryKey
                    { Id = Guid.NewGuid().ToString("N"), NameTest = "othername", SomeNumber = 1 }
            };

            DataBaseScriptRunnerAndBuilder.InsertTableWithNoAutoGeneratedPrimaryKey(Connection, testTableItems);

            var result = Sut.Search(new TableWithNoAutoGeneratedPrimaryKey { NameTest = "Michael" },
                SearchCriteria.Create(nameof(TableWithNoAutoGeneratedPrimaryKey.NameTest), SearchType.Equals));

            Assert.Single(result);
        }

        [Fact]
        public void Ensure_we_can_match_on_numbers()
        {
            var testTableItems = new[]
            {
                new TableWithNoAutoGeneratedPrimaryKey
                    { Id = Guid.NewGuid().ToString("N"), NameTest = "Michale", SomeNumber = 33 },
                new TableWithNoAutoGeneratedPrimaryKey
                    { Id = Guid.NewGuid().ToString("N"), NameTest = "Michael", SomeNumber = 13 },
                new TableWithNoAutoGeneratedPrimaryKey
                    { Id = Guid.NewGuid().ToString("N"), NameTest = "chael", SomeNumber = 73 },
                new TableWithNoAutoGeneratedPrimaryKey
                    { Id = Guid.NewGuid().ToString("N"), NameTest = "othername", SomeNumber = 1 }
            };

            DataBaseScriptRunnerAndBuilder.InsertTableWithNoAutoGeneratedPrimaryKey(Connection, testTableItems);

            var result =
                Sut.Search<TableWithNoAutoGeneratedPrimaryKey, int?>(
                    nameof(TableWithNoAutoGeneratedPrimaryKey.SomeNumber), 33, SearchType.Equals);

            Assert.Single(result);
        }

        [Fact]
        public void Ensure_we_ignore_case_on_property()
        {
            if (DapperRepo.Tests.Connection.SelectedEnvironment == Environment.PostgreSQL)
            {
                // PostgreSQL is case sensitive this will fail
                return;
            }

            var testTableItems = new[]
            {
                new TableWithNoAutoGeneratedPrimaryKey
                    { Id = Guid.NewGuid().ToString("N"), NameTest = "Michale", SomeNumber = 33 },
                new TableWithNoAutoGeneratedPrimaryKey
                    { Id = Guid.NewGuid().ToString("N"), NameTest = "Michael", SomeNumber = 13 },
                new TableWithNoAutoGeneratedPrimaryKey
                    { Id = Guid.NewGuid().ToString("N"), NameTest = "chaal", SomeNumber = 73 },
                new TableWithNoAutoGeneratedPrimaryKey
                    { Id = Guid.NewGuid().ToString("N"), NameTest = "othername", SomeNumber = 1 }
            };

            DataBaseScriptRunnerAndBuilder.InsertTableWithNoAutoGeneratedPrimaryKey(Connection, testTableItems);

            var result = Sut.Search<TableWithNoAutoGeneratedPrimaryKey>("NameTest", "Michael");

            Assert.Single(result);
        }

        [Fact]
        public void Ensure_we_can_match_on_GreaterThan()
        {
            var goodId = Guid.NewGuid().ToString("N");
            var testTableItems = new[]
            {
                new TableWithNoAutoGeneratedPrimaryKey
                    { Id = Guid.NewGuid().ToString("N"), NameTest = "Michale", SomeNumber = 33, },
                new TableWithNoAutoGeneratedPrimaryKey
                    { Id = Guid.NewGuid().ToString("N"), NameTest = "Michael", SomeNumber = 13, },
                new TableWithNoAutoGeneratedPrimaryKey
                    { Id = goodId, NameTest = "chael", SomeNumber = 73, },
                new TableWithNoAutoGeneratedPrimaryKey
                    { Id = Guid.NewGuid().ToString("N"), NameTest = "othername", SomeNumber = 1, }
            };

            DataBaseScriptRunnerAndBuilder.InsertTableWithNoAutoGeneratedPrimaryKey(Connection, testTableItems);

            var result = Sut.Search(new TableWithNoAutoGeneratedPrimaryKey
                {
                    SomeNumber = 63
                },
                SearchCriteria.Create(nameof(TableWithNoAutoGeneratedPrimaryKey.SomeNumber), SearchType.GreaterThan)).ToImmutableArray();

            Assert.Single(result);
            Assert.Equal(goodId, result.FirstOrDefault().Id);
        }

        [Fact]
        public void Ensure_we_can_match_on_GreaterThanEqual()
        {
            var goodId = Guid.NewGuid().ToString("N");
            var goodId2 = Guid.NewGuid().ToString("N");
            var testTableItems = new[]
            {
                new TableWithNoAutoGeneratedPrimaryKey
                    { Id = Guid.NewGuid().ToString("N"), NameTest = "Michale", SomeNumber = 33, },
                new TableWithNoAutoGeneratedPrimaryKey
                    { Id = Guid.NewGuid().ToString("N"), NameTest = "Michael", SomeNumber = 13, },
                new TableWithNoAutoGeneratedPrimaryKey
                    { Id = goodId, NameTest = "chael", SomeNumber = 51 },
                new TableWithNoAutoGeneratedPrimaryKey
                    { Id = goodId2, NameTest = "othername", SomeNumber = 91, }
            };

            DataBaseScriptRunnerAndBuilder.InsertTableWithNoAutoGeneratedPrimaryKey(Connection, testTableItems);

            var result = Sut.Search(new TableWithNoAutoGeneratedPrimaryKey
                {
                    SomeNumber = 51
                },
                SearchCriteria.Create(nameof(TableWithNoAutoGeneratedPrimaryKey.SomeNumber),
                    SearchType.GreaterThanEqualTo));

            var tableWithNoAutoGeneratedPrimaryKeys = result as TableWithNoAutoGeneratedPrimaryKey[] ?? result.ToArray();
            Assert.Equal(2, tableWithNoAutoGeneratedPrimaryKeys.Count());
            Assert.Contains(goodId, tableWithNoAutoGeneratedPrimaryKeys.Select(t => t.Id).ToArray());
            Assert.Contains(goodId2, tableWithNoAutoGeneratedPrimaryKeys.Select(t => t.Id).ToArray());
        }

        [Fact]
        public void Ensure_we_can_match_like_ignoring_first_chars()
        {
            var testTableItems = new[]
            {
                new TableWithNoAutoGeneratedPrimaryKey
                    { Id = Guid.NewGuid().ToString("N"), NameTest = "Michale", SomeNumber = 33 },
                new TableWithNoAutoGeneratedPrimaryKey
                    { Id = Guid.NewGuid().ToString("N"), NameTest = "Michael", SomeNumber = 13 },
                new TableWithNoAutoGeneratedPrimaryKey
                    { Id = Guid.NewGuid().ToString("N"), NameTest = "chael", SomeNumber = 73 },
                new TableWithNoAutoGeneratedPrimaryKey
                    { Id = Guid.NewGuid().ToString("N"), NameTest = "othername", SomeNumber = 1 }
            };

            DataBaseScriptRunnerAndBuilder.InsertTableWithNoAutoGeneratedPrimaryKey(Connection, testTableItems);

            var result =
                Sut.Search<TableWithNoAutoGeneratedPrimaryKey>(nameof(TableWithNoAutoGeneratedPrimaryKey.NameTest),
                    "%chael");

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void Ensure_we_can_match_like_ignoring_end()
        {
            var testTableItems = new[]
            {
                new TableWithNoAutoGeneratedPrimaryKey
                    { Id = Guid.NewGuid().ToString("N"), NameTest = "Michale", SomeNumber = 33 },
                new TableWithNoAutoGeneratedPrimaryKey
                    { Id = Guid.NewGuid().ToString("N"), NameTest = "Michael", SomeNumber = 13 },
                new TableWithNoAutoGeneratedPrimaryKey
                    { Id = Guid.NewGuid().ToString("N"), NameTest = "chael", SomeNumber = 73 },
                new TableWithNoAutoGeneratedPrimaryKey
                    { Id = Guid.NewGuid().ToString("N"), NameTest = "othername", SomeNumber = 1 }
            };

            DataBaseScriptRunnerAndBuilder.InsertTableWithNoAutoGeneratedPrimaryKey(Connection, testTableItems);

            var result =
                Sut.Search<TableWithNoAutoGeneratedPrimaryKey>(nameof(TableWithNoAutoGeneratedPrimaryKey.NameTest),
                    "%cha%");

            Assert.Equal(3, result.Count());
        }

        [Fact]
        public void Ensure_we_can_match_on_LessThan()
        {
            var goodId = Guid.NewGuid().ToString("N");
            
            var testTableItems = new[]
            {
                new TableWithNoAutoGeneratedPrimaryKey
                    { Id = Guid.NewGuid().ToString("N"), NameTest = "Michale", SomeNumber = 33, },
                new TableWithNoAutoGeneratedPrimaryKey
                    { Id = Guid.NewGuid().ToString("N"), NameTest = "Michael", SomeNumber = 13, },
                new TableWithNoAutoGeneratedPrimaryKey
                    { Id = goodId, NameTest = "chael", SomeNumber = 5 },
                new TableWithNoAutoGeneratedPrimaryKey
                    { Id = Guid.NewGuid().ToString("N"), NameTest = "othername", SomeNumber = 15, }
            };

            DataBaseScriptRunnerAndBuilder.InsertTableWithNoAutoGeneratedPrimaryKey(Connection, testTableItems);

            var result = Sut.Search(new TableWithNoAutoGeneratedPrimaryKey
            {
                SomeNumber = 9
            }, SearchCriteria.Create(nameof(TableWithNoAutoGeneratedPrimaryKey.SomeNumber), SearchType.LessThan)).ToImmutableArray();

            Assert.Single(result);
            Assert.Equal(goodId, result.FirstOrDefault().Id);
        }

        [Fact]
        public void Ensure_we_can_match_on_LessThanEqual()
        {
            var goodId = Guid.NewGuid().ToString("N");
            
            var goodId2 = Guid.NewGuid().ToString("N");
            
            var testTableItems = new[]
            {
                new TableWithNoAutoGeneratedPrimaryKey
                    { Id = Guid.NewGuid().ToString("N"), NameTest = "Michale", SomeNumber = 33 },
                new TableWithNoAutoGeneratedPrimaryKey
                    { Id = Guid.NewGuid().ToString("N"), NameTest = "Michael", SomeNumber = 13 },
                new TableWithNoAutoGeneratedPrimaryKey
                    { Id = goodId, NameTest = "chael", SomeNumber = 5 },
                new TableWithNoAutoGeneratedPrimaryKey
                    { Id = goodId2, NameTest = "othername", SomeNumber = 9 }
            };

            DataBaseScriptRunnerAndBuilder.InsertTableWithNoAutoGeneratedPrimaryKey(Connection, testTableItems);

            var result = Sut.Search(new TableWithNoAutoGeneratedPrimaryKey
            {
                SomeNumber = 9
            }, SearchCriteria.Create(nameof(TableWithNoAutoGeneratedPrimaryKey.SomeNumber), SearchType.LessThanEqualTo)).ToImmutableArray();

            Assert.Equal(2, result.Length);
            Assert.Contains(goodId, result.Select(t => t.Id).ToArray());
            Assert.Contains(goodId2, result.Select(t => t.Id).ToArray());
        }

        [Fact]
        public void Ensure_we_can_match_on_multiple()
        {
            var goodId = Guid.NewGuid().ToString("N");
            var testTableItems = new[]
            {
                new TableWithNoAutoGeneratedPrimaryKey
                    { Id = Guid.NewGuid().ToString("N"), NameTest = "Michale", SomeNumber = 33 },
                new TableWithNoAutoGeneratedPrimaryKey
                    { Id = Guid.NewGuid().ToString("N"), NameTest = "Michael", SomeNumber = 13 },
                new TableWithNoAutoGeneratedPrimaryKey
                    { Id = goodId, NameTest = "chael", SomeNumber = 8 },
                new TableWithNoAutoGeneratedPrimaryKey
                    { Id = Guid.NewGuid().ToString("N"), NameTest = "othername", SomeNumber = 8 }
            };

            DataBaseScriptRunnerAndBuilder.InsertTableWithNoAutoGeneratedPrimaryKey(Connection, testTableItems);

            var result = Sut.Search(new TableWithNoAutoGeneratedPrimaryKey
            {
                NameTest = "%hae%",
                SomeNumber = 9
            }, [
                SearchCriteria.Create(nameof(TableWithNoAutoGeneratedPrimaryKey.SomeNumber),SearchType.LessThanEqualTo), 
                SearchCriteria.Create(nameof(TableWithNoAutoGeneratedPrimaryKey.NameTest),SearchType.Like)
            ]).ToImmutableArray();

            Assert.Single(result);
            Assert.Contains(goodId, result.Select(t => t.Id).ToArray());
        }


        [Fact]
        public void Ensure_we_throw_error_if_property_is_not_a_string()
        {
            var exception = Assert.Throws<Exception>(() =>
                Sut.Search<TableWithNoAutoGeneratedPrimaryKey>("SomeNumber", "%cha%"));

            Assert.Contains("Type Must Be String", exception.Message);
        }

        [Fact]
        public void Ensure_we_throw_error_if_property_is_not_found()
        {
            var exception =
                Assert.Throws<Exception>(() => Sut.Search<TableWithNoAutoGeneratedPrimaryKey>("ge", "%cha%"));

            Assert.Contains("not found in Type:", exception.Message);
        }
    }
}