using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Mkb.DapperRepo.Attributes;
using Mkb.DapperRepo.Exceptions;
using Mkb.DapperRepo.Mappers;

[assembly: InternalsVisibleTo("Mkb.DapperRepo.Tests")]

namespace Mkb.DapperRepo.Reflection
{
    internal static class ReflectionUtils
    {
        private static readonly ConcurrentDictionary<Type, EntityPropertyInfo> TypeLookup = new ConcurrentDictionary<Type, EntityPropertyInfo>();

        internal static EntityPropertyInfo GetEntityPropertyInfo<T>()
        {
            if (TypeLookup.TryGetValue(typeof(T), out var item))
            {
                return item;
            }

            var properties = typeof(T).GetProperties()
                .Where(w => !w.GetCustomAttributes(typeof(SqlIgnoreColumnAttribute), true).Any())
                .ToArray();

            var id = properties.FirstOrDefault(f =>
                f.GetCustomAttributes(typeof(PrimaryKeyAttribute), true)
                    .Any()); // primary key determined by attribute now
            var epv = new EntityPropertyInfo(id, properties);

            if (TypeLookup.ContainsKey(typeof(T)))
            {
                return epv;
            }

            TypeLookup.TryAdd(typeof(T), epv);


            TableMapper.Setup<T>();
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
                    throw new TypeMissMatchException($"Type Must Be {type.Name}");


                throw new PropertyNotFoundException($"Property:{property} not found in Type:{typeof(T).Name}");
            }

            return theField;
        }
    }
}