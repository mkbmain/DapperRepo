using System.Linq;
using Mkb.DapperRepo.Attributes;
using Mkb.DapperRepo.Reflection;
using Mkb.DapperRepo.Tests.Entities;
using NUnit.Framework;

namespace Mkb.DapperRepo.Tests.Tests.ReflectionUtilsTests
{
    public class GetEntityPropertyInfoTests
    {
        private const string ColName = "col_name";

        [Test]
        public void Ensure_we_Get_back_correct_primary_key()
        {
            var entityPropertyInfo = ReflectionUtils.GetEntityPropertyInfo<TableWithNoAutoGeneratedPrimaryKey>();
            Assert.AreEqual(nameof(TableWithNoAutoGeneratedPrimaryKey.Id), entityPropertyInfo.Id.Name);
        }

        [Test]
        public void Ensure_we_get_back_correct_nonIdFields()
        {
            var entityPropertyInfo = ReflectionUtils.GetEntityPropertyInfo<TableWithNoAutoGeneratedPrimaryKey>();

            Assert.AreEqual(2, entityPropertyInfo.AllNonId.Count());
            Assert.Contains(nameof(TableWithNoAutoGeneratedPrimaryKey.NameTest),
                entityPropertyInfo.AllNonId.Select(x => x.Name).ToArray());
            Assert.Contains(nameof(TableWithNoAutoGeneratedPrimaryKey.SomeNumber),
                entityPropertyInfo.AllNonId.Select(x => x.Name).ToArray());
        }

        [Test]
        public void Ensure_we_respect_RepoColumn()
        {
            var entityPropertyInfo = ReflectionUtils.GetEntityPropertyInfo<Example>();
            Assert.Contains(ColName, entityPropertyInfo.SqlPropertyColNamesDetails.Keys);
            Assert.AreEqual(entityPropertyInfo.SqlPropertyColNamesDetails[ColName].ClassPropertyName,
                nameof(Example.ColNameChange));
            Assert.Contains(nameof(Example.ColNameChange), entityPropertyInfo.ClassPropertyColNamesDetails.Keys);
            Assert.AreEqual(
                entityPropertyInfo.ClassPropertyColNamesDetails[nameof(Example.ColNameChange)].SqlPropertyName,
                ColName);
        }

        [Test]
        public void Ensure_we_get_back_correct_All()
        {
            var entityPropertyInfo = ReflectionUtils.GetEntityPropertyInfo<TableWithNoAutoGeneratedPrimaryKey>();
            Assert.AreEqual(3, entityPropertyInfo.All.Count());
            Assert.Contains(nameof(TableWithNoAutoGeneratedPrimaryKey.NameTest),
                entityPropertyInfo.All.Select(x => x.Name).ToArray());
            Assert.Contains(nameof(TableWithNoAutoGeneratedPrimaryKey.SomeNumber),
                entityPropertyInfo.All.Select(x => x.Name).ToArray());
            Assert.Contains(nameof(TableWithNoAutoGeneratedPrimaryKey.Id),
                entityPropertyInfo.All.Select(x => x.Name).ToArray());
        }

        private class Example
        {
          [SqlColumnName(ColName)] 
          public string ColNameChange { get; set; }
        }
    }
}