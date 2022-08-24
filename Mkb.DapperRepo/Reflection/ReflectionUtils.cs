using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Mkb.DapperRepo.Attributes;

[assembly: InternalsVisibleTo("Mkb.DapperRepo.Tests")]

namespace Mkb.DapperRepo.Reflection
{
    internal static class ReflectionUtils
    {
        private static Dictionary<Type, EntityPropertyInfo> TypeLookup = new Dictionary<Type, EntityPropertyInfo>();

        internal static EntityPropertyInfo GetEntityPropertyInfo<T>()
        {
            if (TypeLookup.TryGetValue(typeof(T), out var item))
            {
                return item;
            }

            var properties = typeof(T).GetProperties().ToArray();
            var id = properties.FirstOrDefault(f =>
                f.GetCustomAttributes(typeof(PrimaryKeyAttribute), false)
                    .Any()); // primary key determined by attribute now
            var epv = new EntityPropertyInfo(id, properties);
            TypeLookup.Add(typeof(T), epv);
            return epv;
        }

        internal static PropertyInfo GetPropertyInfoOfType<T>(Type type, string property, bool throwIfNotFound = true)
        {
            var fields = GetEntityPropertyInfo<T>();
            var theField = fields.NameLookUp(property, type);
            if (theField == null && throwIfNotFound)
            {
                var match = fields.NameLookUp(property);
                if (match != null && match.Any())
                {
                    throw new Exception($"Type Must Be {type.Name}");
                }

                throw new Exception($"Property:{property} not found in Type:{typeof(T).Name}");
            }

            return theField;
        }
    }
}