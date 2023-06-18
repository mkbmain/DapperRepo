using System;
using System.Collections.Generic;
using Dapper;
using Mkb.DapperRepo.Reflection;

namespace Mkb.DapperRepo.Mappers
{
    internal class TableMapper
    {
        private static readonly HashSet<Type> MapDone = new();

        internal static void Setup<T>()
        {
            if (MapDone.Contains(typeof(T)))
            {
                return;
            }

            SetMap<T>(ReflectionUtils.GetEntityPropertyInfo<T>());
        }

        private static void SetMap<T>(EntityPropertyInfo info)
        {
            if (MapDone.Contains(typeof(T)))
            {
                return;
            }

            SqlMapper.SetTypeMap(typeof(T),
                new CustomPropertyTypeMap(typeof(T),
                    (_, colName) =>
                    {
                        if (info.SqlPropertyColNamesDetails.TryGetValue(colName.ToLower(), out var prop) || 
                            info.ClassPropertyColNamesLowerDetails.TryGetValue(colName.ToLower(), out prop))
                        {
                            return prop.PropertyInfo;
                        }

                        return null;
                    }));


            lock (MapDone)
            {
                MapDone.Add(typeof(T));
            }
        }
    }
}