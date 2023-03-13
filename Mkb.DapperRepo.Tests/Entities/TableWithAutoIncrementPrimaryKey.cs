using System;
using Mkb.DapperRepo.Attributes;

namespace Mkb.DapperRepo.Tests.Entities
{
    public class TableWithAutoIncrementPrimaryKey
    {
        [PrimaryKey]
        [SqlColumnName("Id")]
        public int? BigTest { get; set; }

        [SqlColumnName("Name")]
        public string NameTests { get; set; }

        [SqlColumnName("SomeNumber")]
        public int? SomeNum { get; set; }

        [SqlIgnoreColumn]
        public string Blah { get; set; }
    }
}