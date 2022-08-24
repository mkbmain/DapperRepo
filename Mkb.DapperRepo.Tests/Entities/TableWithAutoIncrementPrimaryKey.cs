using System;
using Mkb.DapperRepo.Attributes;

namespace Mkb.DapperRepo.Tests.Entities
{
    public class TableWithAutoIncrementPrimaryKey
    {
        [PrimaryKey]
        [RepoColumn("Id")]
        public int? BigTest { get; set; }
        [RepoColumn("Name")]
        public string NameTests { get; set; }
        [RepoColumn("SomeNumber")]
        public int? SomeNum { get; set; }
    }
}