using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Mkb.DapperRepo.Attributes;

[assembly: InternalsVisibleTo("Mkb.DapperRepo.Tests")]
namespace Mkb.DapperRepo.Reflection
{
    internal static class ReflectionUtils
    {
        internal static EntityPropertyInfo GetEntityPropertyInfo<T>()
        {
            var properties = typeof(T).GetProperties().ToArray();
            var id = properties.FirstOrDefault(f => f.GetCustomAttributes(typeof(PrimaryKeyAttribute), false).Any()); // primary key determined by attribute now
            return new EntityPropertyInfo(id, properties);
        }

        internal static PropertyInfo GetPropertyInfoOfType<T>(Type type,string property, bool throwIfNotFound =true)
        {
            var fields = GetEntityPropertyInfo<T>();
            var theField = fields.All.FirstOrDefault(f =>
                f.Name.ToLower() == property.ToLower() && f.PropertyType ==type);
            if (theField == null && throwIfNotFound)
            {
                if (fields.All.Any(f => f.Name.ToLower() == property.ToLower()))
                {
                    throw new Exception($"Type Must Be {type.Name}"); 
                }
                throw new Exception($"Property:{property} not found in Type:{typeof(T).Name}");
            }

            return theField;
        }
    }
}