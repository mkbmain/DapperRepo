using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DapperRepo
{
    internal static class ReflectionUtils
    {
        internal static EntityPropertyInfo GetBaseEntityProperyInfo<T>()
        {
            var properties = typeof(T).GetProperties().ToArray();
            var id = properties.FirstOrDefault(f => f.GetCustomAttributes(typeof(PrimaryKeyAttribute), false).Any()); // primary key determined by attribute now
            return new EntityPropertyInfo(id, properties);
        }
        
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
}