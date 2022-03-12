using System;
using Mkb.DapperRepo.Attributes;

namespace Mkb.DapperRepo.Tests.Entities
{
    [SqlTableName("TableWithAutoIncrementPrimaryKey")]
    public class TableWithAutoIncrementPrimaryKeyDiffSqlName
    {
        public TableWithAutoIncrementPrimaryKeyDiffSqlName()
        {
            
        }
        public TableWithAutoIncrementPrimaryKeyDiffSqlName(TableWithAutoIncrementPrimaryKey item)
        {
            Id = item.Id;
            Name = item.Name;
            SomeNumber = item.SomeNumber;
        }
        
        [PrimaryKey]
        public int? Id { get; set; }

        public string Name { get; set; }
        public int? SomeNumber { get; set; }
    }
    
    // This is a copy of TableWithAutIncrementPrimaryKey but with different names and sql name makes them the same for testing
    // this is to test SqlName
}