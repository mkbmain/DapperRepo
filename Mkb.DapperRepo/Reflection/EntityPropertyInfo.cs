using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Mkb.DapperRepo.Tests")]

namespace Mkb.DapperRepo.Reflection
{
    internal class EntityPropertyInfo
    {
        private Dictionary<string, PropertyInfo[]> QuickNameLookUp;

        public EntityPropertyInfo(PropertyInfo id, IEnumerable<PropertyInfo> all)
        {
            Id = id;
            All = all;
            QuickNameLookUp = all.GroupBy(e => e.Name.ToLower()).ToDictionary(e => e.Key, e => e.ToArray());
        }

        public PropertyInfo NameLookUp(string name, Type type) => NameLookUp(name)?.FirstOrDefault(e => e.PropertyType == type);
        

        public IEnumerable<PropertyInfo> NameLookUp(string name)
        {
            if (QuickNameLookUp.TryGetValue(name.ToLower(), out var items))
            {
                return items;
            }

            return null;
        }

        public PropertyInfo Id { get; set; }
        internal IEnumerable<PropertyInfo> All { get; set; }
        public IEnumerable<PropertyInfo> AllNonId => All.Where(f => f != Id);
    }
}