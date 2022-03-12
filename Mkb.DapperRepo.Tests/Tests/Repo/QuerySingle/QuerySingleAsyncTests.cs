using System;
using System.Linq;
using System.Threading.Tasks;
using Mkb.DapperRepo.Tests.Entities;
using Mkb.DapperRepo.Tests.Tests.BaseTestClasses;
using Mkb.DapperRepo.Tests.Utils;
using NUnit.Framework;

namespace Mkb.DapperRepo.Tests.Tests.Repo.QuerySingle
{
    public class QuerySingleAsyncTests : BaseAsyncTestClass
    {
        public QuerySingleAsyncTests() : base($"{nameof(QuerySingleAsyncTests)}{RandomChars}")
        {
        }

        [Test]
        public async Task Ensure_we_Get_correct_record_back()
        {
            var testTableItems = new[]
            {
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid(), Name = "Wanted1", SomeNumber = 11},
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid(), Name = "othername", SomeNumber = 33},
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid(), Name = "Wanted2", SomeNumber = 21}
            };

            DataBaseScriptRunnerAndBuilder.InsertTableWithNoAutoGeneratedPrimaryKey(Connection, testTableItems);

            var item = (await Sut.QuerySingle<TableWithNoAutoGeneratedPrimaryKey>(
                "select * from TableWithNoAutoGeneratedPrimaryKey where SomeNumber = 33"));
            var expected = testTableItems.First(f => f.SomeNumber == 33);


            Assert.IsNotNull(item);
            Assert.AreEqual(item.Name, expected.Name);
            Assert.AreEqual(item.Id, expected.Id);
            Assert.AreEqual(item.SomeNumber, expected.SomeNumber);
        }
        
        [Test]
        public async Task Ensure_we_Get_correct_record_back_with_diff_name()
        {
            var testTableItems = new[]
            {
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid(), Name = "Wanted1", SomeNumber = 11},
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid(), Name = "othername", SomeNumber = 33},
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid(), Name = "Wanted2", SomeNumber = 21}
            };

            DataBaseScriptRunnerAndBuilder.InsertTableWithNoAutoGeneratedPrimaryKey(Connection, testTableItems);

            var item = (await Sut.QuerySingle<TableWithNoAutoGeneratedPrimaryKeyDiffSqlName>(
                "select * from TableWithNoAutoGeneratedPrimaryKey where SomeNumber = 33"));
            var expected = testTableItems.First(f => f.SomeNumber == 33);


            Assert.IsNotNull(item);
            Assert.AreEqual(item.Name, expected.Name);
            Assert.AreEqual(item.Id, expected.Id);
            Assert.AreEqual(item.SomeNumber, expected.SomeNumber);
        }

        [Test]
        public async Task Ensure_if_we_have_no_records_we_do_not_blow_up()
        {
            var item = (await Sut.QuerySingle<TableWithNoAutoGeneratedPrimaryKey>(
                "select * from TableWithNoAutoGeneratedPrimaryKey"));
            Assert.IsNull(item);
        }
    }
}