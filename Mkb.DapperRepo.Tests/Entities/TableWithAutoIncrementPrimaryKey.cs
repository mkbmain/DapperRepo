using Mkb.DapperRepo.Attributes;

namespace Mkb.DapperRepo.Tests.Entities
{
    public class TableWithAutoIncrementPrimaryKey
    {
        [PrimaryKey]
        public int? Id { get; set; }
        public string Name { get; set; }
        public int? SomeNumber { get; set; }
    }
}