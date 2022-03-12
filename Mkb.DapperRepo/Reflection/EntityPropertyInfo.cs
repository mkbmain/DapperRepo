using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Mkb.DapperRepo.Tests")]
namespace Mkb.DapperRepo.Reflection
{
    internal class EntityPropertyInfo
    {
        public EntityPropertyInfo(PropertyInfo id, IEnumerable<PropertyInfo> all)
        {
            Id = id;
            All = all;
        }
        public PropertyInfo Id { get; set; }
        internal IEnumerable<PropertyInfo> All { get; set; }
        public IEnumerable<PropertyInfo> AllNonId => All.Where(f=> f!= Id);
    }
}