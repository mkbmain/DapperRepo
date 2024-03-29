using Mkb.DapperRepo.Attributes;

namespace Mkb.DapperRepo.Tests.Entities
{
    [SqlTableName("TableWithNoAutoGeneratedPrimaryKey")]
    public class TableWithNoAutoGeneratedPrimaryKeyDiffSqlName
    {
        public TableWithNoAutoGeneratedPrimaryKeyDiffSqlName()
        {
        }

        public TableWithNoAutoGeneratedPrimaryKeyDiffSqlName(TableWithNoAutoGeneratedPrimaryKey item)
        {
            Id = item.Id;
            Name = item.NameTest;
            SomeNumber = item.SomeNumber;
        }

        [PrimaryKey]
        public string Id { get; set; }

        public string Name { get; set; }

        public int? SomeNumber { get; set; }

        [SqlIgnoreColumn]
        public string Blah { get; set; }
    }
}