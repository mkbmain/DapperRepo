using System;
using System.Collections.Concurrent;
using Dapper;
using Mkb.DapperRepo.Reflection;

namespace Mkb.DapperRepo.Mappers
{
    internal class TableMapper
    {
        private static readonly ConcurrentDictionary<Type, bool> MapDone = new ConcurrentDictionary<Type, bool>();

        internal static void Setup<T>()
        {
            if (MapDone.ContainsKey(typeof(T))) return;
            SetMap<T>(ReflectionUtils.GetEntityPropertyInfo<T>());
        }

        internal static void SetMap<T>(EntityPropertyInfo info)
        {
            if (MapDone.ContainsKey(typeof(T))) return;
            if (!MapDone.TryAdd(typeof(T), true)) return;
            SqlMapper.SetTypeMap(typeof(T),
                new CustomPropertyTypeMap(typeof(T),
                    (type, colName) =>
                    {
                        if (info.SqlPropertyColNamesDetails.TryGetValue(colName.ToLower(), out var prop))
                        {
                            return prop.PropertyInfo;
                        }

                        if (info.ClassPropertyColNamesLowerDetails.TryGetValue(colName.ToLower(), out prop))
                        {
                            return prop.PropertyInfo;
                        }

                        return null;
                    }));
        }
    }
}