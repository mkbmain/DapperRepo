using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Mkb.DapperRepo.Tests.Entities;
using Mkb.DapperRepo.Tests.Tests.BaseTestClasses;
using Mkb.DapperRepo.Tests.Utils;
using Xunit;

namespace Mkb.DapperRepo.Tests.Tests.Repo.GetExactMatches
{
    [Collection("Integration")]
    public class GetExactMatchesAsyncTest : BaseAsyncTestClass
    {
        [Fact]
        public async Task Ensure_we_can_match_on_properties_exactly()
        {
            var item = new TableWithAutoIncrementPrimaryKey {NameTests = "Michale", SomeNum = 33};
            var testTableItems = new[]
            {
                new TableWithAutoIncrementPrimaryKey {NameTests = "Michale", SomeNum = 31},
                item,
                new TableWithAutoIncrementPrimaryKey {NameTests = "Michael", SomeNum = 33},
                new TableWithAutoIncrementPrimaryKey {NameTests = "Michale", SomeNum = null},
                new TableWithAutoIncrementPrimaryKey {NameTests = null, SomeNum = 33}
            };

            DataBaseScriptRunnerAndBuilder.InsertTableWithAutoGeneratedPrimaryKey(Connection, testTableItems);

            var results = (await Sut.GetExactMatches(item, false)).ToImmutableArray();
            Assert.Single(results);
            var test = results.First();
            Assert.NotNull(test);
            Assert.Equal(item.NameTests, test.NameTests);
            Assert.Equal(item.SomeNum, test.SomeNum);
        }

        [Fact]
        public async Task Ensure_we_can_match_on_properties_withNull()
        {
            var item = new TableWithAutoIncrementPrimaryKey {NameTests = "Michale", SomeNum = null};
            var testTableItems = new[]
            {
                new TableWithAutoIncrementPrimaryKey {NameTests = "Michale", SomeNum = 31},
                item,
                new TableWithAutoIncrementPrimaryKey {NameTests = "Michael", SomeNum = 33},
                new TableWithAutoIncrementPrimaryKey {NameTests = null, SomeNum = 33}
            };

            DataBaseScriptRunnerAndBuilder.InsertTableWithAutoGeneratedPrimaryKey(Connection, testTableItems);

            var results = (await Sut.GetExactMatches(item, false)).ToImmutableArray();
            Assert.Single(results);
            var test = results.First();
            Assert.NotNull(test);
            Assert.Equal(item.NameTests, test.NameTests);
            Assert.Equal(item.SomeNum, test.SomeNum);
        }

        [Fact]
        public async Task Ensure_we_can_match_on_properties_ignoring_nulls()
        {
            var item = new TableWithAutoIncrementPrimaryKey {NameTests = "Michale", SomeNum = 33};
            var testTableItems = new[]
            {
                new TableWithAutoIncrementPrimaryKey {NameTests = "Michale", SomeNum = 31},
                item,
                new TableWithAutoIncrementPrimaryKey {NameTests = "Michael", SomeNum = 33},
                new TableWithAutoIncrementPrimaryKey {NameTests = "Michale", SomeNum = null},
                new TableWithAutoIncrementPrimaryKey {NameTests = null, SomeNum = 33}
            };

            DataBaseScriptRunnerAndBuilder.InsertTableWithAutoGeneratedPrimaryKey(Connection, testTableItems);
            var oldNumber = item.SomeNum;
            item.SomeNum = null;
            var results = (await Sut.GetExactMatches(item, true)).ToImmutableArray();
            Assert.Equal(3, results.Count());
            Assert.Equal(3, results.Count(t => t.NameTests == item.NameTests));
            Assert.Equal(1, results.Count(t => t.SomeNum == oldNumber));
            Assert.Equal(1, results.Count(t => t.SomeNum is null));
        }
    }
}