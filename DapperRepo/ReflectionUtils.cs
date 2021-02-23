using System.Linq;

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
        
    }
}