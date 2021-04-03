using DapperRepo;

namespace DapperRepoTests.Entities
{
    public class TableWithAutIncrementPrimaryKey
    {
        [PrimaryKey]
        public int? Id { get; set; }
        public string Name { get; set; }
        public int? SomeNumber { get; set; }
    }
}