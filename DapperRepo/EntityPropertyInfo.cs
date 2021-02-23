using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DapperRepo
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