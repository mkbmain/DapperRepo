using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DapperRepoTests")]
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

        internal static PropertyInfo GetPropertyInfoOfType<T>(Type type,string property, bool throwIfNotFound =true)
        {
            var fields = typeof(T).GetProperties().ToArray();
            var theField = fields.FirstOrDefault(f =>
                f.Name.ToLower() == property.ToLower() && f.PropertyType ==type);
            if (theField == null && throwIfNotFound)
            {
                if (fields.Any(f => f.Name.ToLower() == property.ToLower()))
                {
                    throw new Exception("Type Must Be string"); 
                }
                throw new Exception($"Property:{property} not found in Type:{type.Name}");
            }

            return theField;
        }
    }
}