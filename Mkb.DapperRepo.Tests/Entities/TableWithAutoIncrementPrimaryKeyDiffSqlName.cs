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
            TestId = item.BigTest;
            NameTest = item.NameTests;
            SomeNum = item.SomeNum;
        }
        
        [PrimaryKey]
        [SqlColumnName("Id")]
        public int? TestId { get; set; }
        [SqlColumnName("Name")]
        public string NameTest { get; set; }
        [SqlColumnName("SomeNumber")]
        public int? SomeNum { get; set; }
    }
    
    // This is a copy of TableWithAutIncrementPrimaryKey but with different names and sql name makes them the same for testing
    // this is to test SqlName
}