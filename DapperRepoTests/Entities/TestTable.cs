using System;
using DapperRepo;

namespace DapperRepoTests.Entities
{
    public class TestTable
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        public string Name { get; set; }
        public int SomeNumber { get; set; }
    }
}