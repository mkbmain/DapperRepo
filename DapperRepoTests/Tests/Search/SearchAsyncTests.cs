using System;
using System.Linq;
using System.Threading.Tasks;
using DapperRepoTests.Entities;
using DapperRepoTests.Tests.BaseTestClasses;
using DapperRepoTests.Utils;
using NUnit.Framework;

namespace DapperRepoTests.Tests.Search
{
    public class SearchAsyncTests : BaseAsyncTestClass
    {
        public SearchAsyncTests() : base($"{nameof(SearchAsyncTests)}{RandomChars}")
        {
        }


        [Test]
        public async Task Ensure_we_can_match_exact()
        {
            var testTableItems = new[]
            {
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid(), Name = "Michale", SomeNumber = 33},
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid(), Name = "Michael", SomeNumber = 13},
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid(), Name = "chaal", SomeNumber = 73},
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid(), Name = "othername", SomeNumber = 1}
            };

            DataBaseScriptRunnerAndBuilder.InsertTableWithNoAutoGeneratedPrimaryKey(Connection, testTableItems);

            var result = await Sut.Search<TableWithNoAutoGeneratedPrimaryKey>("Name", "Michael");

            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public async Task Ensure_we_ignore_case_on_property()
        {
            var testTableItems = new[]
            {
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid(), Name = "Michale", SomeNumber = 33},
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid(), Name = "Michael", SomeNumber = 13},
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid(), Name = "chaal", SomeNumber = 73},
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid(), Name = "othername", SomeNumber = 1}
            };

            DataBaseScriptRunnerAndBuilder.InsertTableWithNoAutoGeneratedPrimaryKey(Connection, testTableItems);

            var result = await Sut.Search<TableWithNoAutoGeneratedPrimaryKey>("NAME", "Michael");

            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public async Task Ensure_we_can_match_ignoring_first_chars()
        {
            var testTableItems = new[]
            {
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid(), Name = "Michale", SomeNumber = 33},
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid(), Name = "Michael", SomeNumber = 13},
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid(), Name = "chael", SomeNumber = 73},
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid(), Name = "othername", SomeNumber = 1}
            };

            DataBaseScriptRunnerAndBuilder.InsertTableWithNoAutoGeneratedPrimaryKey(Connection, testTableItems);

            var result = await Sut.Search<TableWithNoAutoGeneratedPrimaryKey>("Name", "%chael");

            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task Ensure_we_can_match_ignoring_end()
        {
            var testTableItems = new[]
            {
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid(), Name = "Michale", SomeNumber = 33},
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid(), Name = "Michael", SomeNumber = 13},
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid(), Name = "chael", SomeNumber = 73},
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid(), Name = "othername", SomeNumber = 1}
            };

            DataBaseScriptRunnerAndBuilder.InsertTableWithNoAutoGeneratedPrimaryKey(Connection, testTableItems);

            var result = await Sut.Search<TableWithNoAutoGeneratedPrimaryKey>("Name", "%cha%");

            Assert.AreEqual(3, result.Count());
        }

        [Test]
        public async Task Ensure_we_throw_error_if_property_is_not_a_string()
        {
            var testTableItems = new[]
            {
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid(), Name = "Michale", SomeNumber = 33},
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid(), Name = "Michael", SomeNumber = 13},
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid(), Name = "chael", SomeNumber = 73},
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid(), Name = "othername", SomeNumber = 1}
            };

            DataBaseScriptRunnerAndBuilder.InsertTableWithNoAutoGeneratedPrimaryKey(Connection, testTableItems);


            var exception = Assert.ThrowsAsync<Exception>(async () =>
                await Sut.Search<TableWithNoAutoGeneratedPrimaryKey>("SomeNumber", "%cha%"));

            Assert.True(exception.Message.Contains("Type Must Be String"));
        }

        [Test]
        public async Task Ensure_we_throw_error_if_property_is_not_found()
        {
            var testTableItems = new[]
            {
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid(), Name = "Michale", SomeNumber = 33},
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid(), Name = "Michael", SomeNumber = 13},
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid(), Name = "chael", SomeNumber = 73},
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid(), Name = "othername", SomeNumber = 1}
            };

            DataBaseScriptRunnerAndBuilder.InsertTableWithNoAutoGeneratedPrimaryKey(Connection, testTableItems);


            var exception = Assert.ThrowsAsync<Exception>(async () =>
                await Sut.Search<TableWithNoAutoGeneratedPrimaryKey>("ge", "%cha%"));

            Assert.True(exception.Message.Contains("not found in Type:"));
        }
    }
}