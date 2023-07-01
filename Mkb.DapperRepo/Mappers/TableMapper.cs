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

            var info = ReflectionUtils.GetEntityPropertyInfo<T>();

            SqlMapper.SetTypeMap(typeof(T),
                new CustomPropertyTypeMap(typeof(T),
                    (_, colName) => info.SqlPropertyColNamesDetails.TryGetValue(colName.ToLower(), out var prop) ? prop.PropertyInfo : null));


            lock (MapDone)
            {
                MapDone.Add(typeof(T));
            }
        }
    }
}