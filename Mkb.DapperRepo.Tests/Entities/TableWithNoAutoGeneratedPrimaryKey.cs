using System;
using System.ComponentModel.DataAnnotations.Schema;
using Mkb.DapperRepo.Attributes;

namespace Mkb.DapperRepo.Tests.Entities
{
    public class TableWithNoAutoGeneratedPrimaryKey
    {
        [PrimaryKey]
        public string Id { get; set; } // hate this butttttt mysql does not support guids

        [SqlColumnName("Name")]
        public string NameTest { get; set; }

        public int? SomeNumber { get; set; }

        [SqlColumnIgnore]
        public string Blah { get; set; }
    }

    // This is a copy of TableWithAutIncrementPrimaryKey but with different names and sql name makes them the same for testing
    // this is to test SqlName
}