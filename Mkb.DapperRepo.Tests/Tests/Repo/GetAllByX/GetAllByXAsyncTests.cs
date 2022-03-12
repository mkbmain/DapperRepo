using System;
using System.Linq;
using System.Threading.Tasks;
using Mkb.DapperRepo.Tests.Entities;
using Mkb.DapperRepo.Tests.Tests.BaseTestClasses;
using Mkb.DapperRepo.Tests.Utils;
using NUnit.Framework;

namespace Mkb.DapperRepo.Tests.Tests.Repo.GetAllByX
{
    public class GetAllByXAsyncTests : BaseAsyncTestClass
    {
        public GetAllByXAsyncTests() : base($"{nameof(GetAllByXAsyncTests)}{RandomChars}")
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

            var result = await Sut.GetAllByX<TableWithNoAutoGeneratedPrimaryKey,int?>("SomeNumber", testTableItems.FirstOrDefault().SomeNumber);

            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public async Task Ensure_we_can_match_exact_diff_name()
        {
            var testTableItems = new[]
            {
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid(), Name = "Michale", SomeNumber = 33},
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid(), Name = "Michael", SomeNumber = 13},
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid(), Name = "chaal", SomeNumber = 73},
                new TableWithNoAutoGeneratedPrimaryKey {Id = Guid.NewGuid(), Name = "othername", SomeNumber = 1}
            };

            DataBaseScriptRunnerAndBuilder.InsertTableWithNoAutoGeneratedPrimaryKey(Connection, testTableItems);

            var result = await Sut.GetAllByX<TableWithNoAutoGeneratedPrimaryKeyDiffSqlName,String>("Name", "Michael");

            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public void Ensure_we_throw_error_if_property_not_defined_correct()
        {

            var exception = Assert.ThrowsAsync<Exception>(async () =>
                await Sut.GetAllByX<TableWithNoAutoGeneratedPrimaryKey,int>("Name", 532));

            Assert.True(exception.Message.Contains("Type Must Be Int32"));
        }

        [Test]
        public void Ensure_we_throw_error_if_property_is_not_found()
        {

            var exception = Assert.ThrowsAsync<Exception>(async () =>
                await Sut.GetAllByX<TableWithNoAutoGeneratedPrimaryKey,String>("ge", "%cha%"));

            Assert.True(exception.Message.Contains("not found in Type:"));
        }
    }
}