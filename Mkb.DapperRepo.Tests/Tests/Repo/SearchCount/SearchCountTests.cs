using System;
using Mkb.DapperRepo.Search;
using Mkb.DapperRepo.Tests.Entities;
using Mkb.DapperRepo.Tests.Tests.BaseTestClasses;
using Mkb.DapperRepo.Tests.Utils;
using NUnit.Framework;

namespace Mkb.DapperRepo.Tests.Tests.Repo.SearchCount
{
    public class SearchCountTests : BaseSyncTestClass
    {
        [Test]
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

            var count = Sut.SearchCount(new TableWithNoAutoGeneratedPrimaryKey { NameTest = "Michael" },
                new[]
                {
                    new SearchCriteria(nameof(TableWithNoAutoGeneratedPrimaryKey.NameTest), SearchType.Equals), 
                    new SearchCriteria(nameof(TableWithNoAutoGeneratedPrimaryKey.SomeNumber), SearchType.IsNull)
                });

            Assert.AreEqual(1, count);
        }

        [Test]
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

            var result = Sut.SearchCount(new TableWithNoAutoGeneratedPrimaryKey { NameTest = "Michael" },
                new SearchCriteria(nameof(TableWithNoAutoGeneratedPrimaryKey.NameTest), SearchType.Equals));

            Assert.AreEqual(1, result);
        }

        [Test]
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
                Sut.SearchCount<TableWithNoAutoGeneratedPrimaryKey, int?>(
                    nameof(TableWithNoAutoGeneratedPrimaryKey.SomeNumber), 33, SearchType.Equals);

            Assert.AreEqual(1, result);
        }

        [Test]
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

            var result = Sut.SearchCount(new TableWithNoAutoGeneratedPrimaryKey
                {
                    SomeNumber = 63
                },
                new SearchCriteria(nameof(TableWithNoAutoGeneratedPrimaryKey.SomeNumber), SearchType.GreaterThan));

            Assert.AreEqual(1, result);
        }

        [Test]
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

            var result = Sut.SearchCount(new TableWithNoAutoGeneratedPrimaryKey
                {
                    SomeNumber = 51
                },
                new SearchCriteria(nameof(TableWithNoAutoGeneratedPrimaryKey.SomeNumber),
                    SearchType.GreaterThanEqualTo));

            Assert.AreEqual(2, result);
        }

        [Test]
        public void Ensure_we_can_match_on_LessThan()
        {
            var goodId = Guid.NewGuid().ToString("N");
            ;
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

            var result = Sut.SearchCount(new TableWithNoAutoGeneratedPrimaryKey
            {
                SomeNumber = 9
            }, new SearchCriteria(nameof(TableWithNoAutoGeneratedPrimaryKey.SomeNumber), SearchType.LessThan));

            Assert.AreEqual(1, result);
        }

        [Test]
        public void Ensure_we_can_match_on_LessThanEqual()
        {
            var goodId = Guid.NewGuid().ToString("N");
            ;
            var goodId2 = Guid.NewGuid().ToString("N");
            ;
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

            var result = Sut.SearchCount(new TableWithNoAutoGeneratedPrimaryKey
            {
                SomeNumber = 9
            }, new SearchCriteria(nameof(TableWithNoAutoGeneratedPrimaryKey.SomeNumber), SearchType.LessThanEqualTo));

            Assert.AreEqual(2, result);
        }

        [Test]
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

            var result = Sut.SearchCount(new TableWithNoAutoGeneratedPrimaryKey
            {
                NameTest = "%hae%",
                SomeNumber = 9
            }, new[]
            {
                new SearchCriteria(nameof(TableWithNoAutoGeneratedPrimaryKey.SomeNumber),SearchType.LessThanEqualTo), 
                new SearchCriteria(nameof(TableWithNoAutoGeneratedPrimaryKey.NameTest),SearchType.Like)
            });

            Assert.AreEqual(1, result);
        }
    }
}