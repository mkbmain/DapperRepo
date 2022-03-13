using System;
using System.Linq;
using Mkb.DapperRepo.Search;
using Mkb.DapperRepo.Tests.Entities;
using Mkb.DapperRepo.Tests.Tests.BaseTestClasses;
using Mkb.DapperRepo.Tests.Utils;
using NUnit.Framework;

namespace Mkb.DapperRepo.Tests.Tests.Repo.Search
{
    public class SearchTests : BaseSyncTestClass
    {
        public SearchTests() : base($"{nameof(SearchTests)}")
        {
        }

        [Test]
        public void Ensure_we_can_match_exact()
        {
            var testTableItems = new[]
            {
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid().ToString("N"), Name = "Michale", SomeNumber = 33},
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid().ToString("N"), Name = "Michael", SomeNumber = 13},
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid().ToString("N"), Name = "chaal", SomeNumber = 73},
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid().ToString("N"), Name = "othername", SomeNumber = 1}
            };

            DataBaseScriptRunnerAndBuilder.InsertTableWithNoAutoGeneratedPrimaryKey(Connection, testTableItems);

            var result = Sut.Search(new TableWithNoAutoGeneratedPrimaryKey{Name = "Michael"}, new SearchCriteria
            {
                PropertyName = nameof(TableWithNoAutoGeneratedPrimaryKey.Name),
                SearchType = SearchType.Equals
            });

            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public void Ensure_we_ignore_case_on_property()
        {
            if (DapperRepo.Tests.Connection.SelectedEnvironment == Enviroment.PostgreSQL)
            {
                // PostgreSQL is case sensitive this will fail
                return;
            }
            var testTableItems = new[]
            {
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid().ToString("N"), Name = "Michale", SomeNumber = 33},
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid().ToString("N"), Name = "Michael", SomeNumber = 13},
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid().ToString("N"), Name = "chaal", SomeNumber = 73},
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid().ToString("N"), Name = "othername", SomeNumber = 1}
            };

            DataBaseScriptRunnerAndBuilder.InsertTableWithNoAutoGeneratedPrimaryKey(Connection, testTableItems);

            var result = Sut.Search<TableWithNoAutoGeneratedPrimaryKey>("NAME", "Michael");

            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public void Ensure_we_can_match_like_ignoring_first_chars()
        {
            var testTableItems = new[]
            {
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid().ToString("N"), Name = "Michale", SomeNumber = 33},
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid().ToString("N"), Name = "Michael", SomeNumber = 13},
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid().ToString("N"), Name = "chael", SomeNumber = 73},
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid().ToString("N"), Name = "othername", SomeNumber = 1}
            };

            DataBaseScriptRunnerAndBuilder.InsertTableWithNoAutoGeneratedPrimaryKey(Connection, testTableItems);

            var result = Sut.Search<TableWithNoAutoGeneratedPrimaryKey>("Name", "%chael");

            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public void Ensure_we_can_match_like_ignoring_end()
        {
            var testTableItems = new[]
            {
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid().ToString("N"), Name = "Michale", SomeNumber = 33},
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid().ToString("N"), Name = "Michael", SomeNumber = 13},
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid().ToString("N"), Name = "chael", SomeNumber = 73},
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid().ToString("N") ,Name = "othername", SomeNumber = 1}
            };

            DataBaseScriptRunnerAndBuilder.InsertTableWithNoAutoGeneratedPrimaryKey(Connection, testTableItems);

            var result = Sut.Search<TableWithNoAutoGeneratedPrimaryKey>("Name", "%cha%");

            Assert.AreEqual(3, result.Count());
        }
        
        [Test]
        public void Ensure_we_can_match_on_LessThan()
        {
            var goodId = Guid.NewGuid().ToString("N");;
            var testTableItems = new[]
            {
                new TableWithNoAutoGeneratedPrimaryKey
                    {Id = Guid.NewGuid().ToString("N"), Name = "Michale", SomeNumber = 33,},
                new TableWithNoAutoGeneratedPrimaryKey
                    {Id = Guid.NewGuid().ToString("N") ,Name = "Michael", SomeNumber = 13,},
                new TableWithNoAutoGeneratedPrimaryKey
                    {Id = goodId, Name = "chael", SomeNumber = 5},
                new TableWithNoAutoGeneratedPrimaryKey
                    {Id = Guid.NewGuid().ToString("N"), Name = "othername", SomeNumber = 15,}
            };

            DataBaseScriptRunnerAndBuilder.InsertTableWithNoAutoGeneratedPrimaryKey(Connection, testTableItems);

            var result =  Sut.Search(new TableWithNoAutoGeneratedPrimaryKey
            {
                SomeNumber = 9
            }, new SearchCriteria
            {
                PropertyName = nameof(TableWithNoAutoGeneratedPrimaryKey.SomeNumber),
                SearchType = SearchType.LessThan
            });

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(goodId, result.FirstOrDefault().Id);
        }

        [Test]
        public void Ensure_we_can_match_on_LessThanEqual()
        {
            var goodId =Guid.NewGuid().ToString("N");;
            var goodId2 = Guid.NewGuid().ToString("N");;
            var testTableItems = new[]
            {
                new TableWithNoAutoGeneratedPrimaryKey
                    {Id = Guid.NewGuid().ToString("N"), Name = "Michale", SomeNumber = 33},
                new TableWithNoAutoGeneratedPrimaryKey
                    {Id = Guid.NewGuid().ToString("N") ,Name = "Michael", SomeNumber = 13},
                new TableWithNoAutoGeneratedPrimaryKey
                    {Id = goodId, Name = "chael", SomeNumber = 5},
                new TableWithNoAutoGeneratedPrimaryKey
                    {Id = goodId2, Name = "othername", SomeNumber = 9}
            };

            DataBaseScriptRunnerAndBuilder.InsertTableWithNoAutoGeneratedPrimaryKey(Connection, testTableItems);

            var result =  Sut.Search(new TableWithNoAutoGeneratedPrimaryKey
            {
                SomeNumber = 9
            }, new SearchCriteria
            {
                PropertyName = nameof(TableWithNoAutoGeneratedPrimaryKey.SomeNumber),
                SearchType = SearchType.LessThanEqualTo
            });

            Assert.AreEqual(2, result.Count());
            Assert.Contains(goodId, result.Select(t => t.Id).ToArray());
            Assert.Contains(goodId2, result.Select(t => t.Id).ToArray());
        }

        [Test]
        public void Ensure_we_can_match_on_multiple()
        {
            var goodId =Guid.NewGuid().ToString("N");
            var testTableItems = new[]
            {
                new TableWithNoAutoGeneratedPrimaryKey
                    {Id = Guid.NewGuid().ToString("N"), Name = "Michale", SomeNumber = 33},
                new TableWithNoAutoGeneratedPrimaryKey
                    {Id = Guid.NewGuid().ToString("N"), Name = "Michael", SomeNumber = 13},
                new TableWithNoAutoGeneratedPrimaryKey
                    {Id = goodId, Name = "chael", SomeNumber = 8},
                new TableWithNoAutoGeneratedPrimaryKey
                    {Id = Guid.NewGuid().ToString("N") ,Name = "othername", SomeNumber = 8}
            };

            DataBaseScriptRunnerAndBuilder.InsertTableWithNoAutoGeneratedPrimaryKey(Connection, testTableItems);

            var result =  Sut.Search(new TableWithNoAutoGeneratedPrimaryKey
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

            Assert.AreEqual(1, result.Count());
            Assert.Contains(goodId, result.Select(t => t.Id).ToArray());
        }

        

        [Test]
        public void Ensure_we_throw_error_if_property_is_not_a_string()
        {
            var exception = Assert.Throws<Exception>(() =>
                Sut.Search<TableWithNoAutoGeneratedPrimaryKey>("SomeNumber", "%cha%"));

            Assert.True(exception.Message.Contains("Type Must Be String"));
        }

        [Test]
        public void Ensure_we_throw_error_if_property_is_not_found()
        {
            var exception =
                Assert.Throws<Exception>(() => Sut.Search<TableWithNoAutoGeneratedPrimaryKey>("ge", "%cha%"));

            Assert.True(exception.Message.Contains("not found in Type:"));
        }
    }
}