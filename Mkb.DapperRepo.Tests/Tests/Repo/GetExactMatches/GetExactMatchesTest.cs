using System.Linq;
using System.Threading.Tasks;
using Mkb.DapperRepo.Tests.Entities;
using Mkb.DapperRepo.Tests.Tests.BaseTestClasses;
using Mkb.DapperRepo.Tests.Utils;
using NUnit.Framework;

namespace Mkb.DapperRepo.Tests.Tests.Repo.GetExactMatches
{
    public class GetExactMatchesTest : BaseSyncTestClass
    {
        [Test]
        public void Ensure_we_can_match_on_properties_exactly()
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

            var results = Sut.GetExactMatches(item, false);
            Assert.AreEqual(1, results.Count());
            var test = results.First();
            Assert.IsNotNull(test);
            Assert.AreEqual(item.NameTests, test.NameTests);
            Assert.AreEqual(item.SomeNum, test.SomeNum);
        }

        [Test]
        public void Ensure_we_can_match_on_properties_withNull()
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

            var results = Sut.GetExactMatches(item, false);
            Assert.AreEqual(1, results.Count());
            var test = results.First();
            Assert.IsNotNull(test);
            Assert.AreEqual(item.NameTests, test.NameTests);
            Assert.AreEqual(item.SomeNum, test.SomeNum);
        }

        [Test]
        public void Ensure_we_can_match_on_properties_ignoring_nulls()
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
            var results = Sut.GetExactMatches(item, true);
            Assert.AreEqual(3, results.Count());
            Assert.AreEqual(3, results.Count(t => t.NameTests == item.NameTests));
            Assert.AreEqual(1, results.Count(t => t.SomeNum == oldNumber));
            Assert.AreEqual(1, results.Count(t => t.SomeNum is null));
        }
    }
}