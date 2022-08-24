using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Mkb.DapperRepo.Attributes;

[assembly: InternalsVisibleTo("Mkb.DapperRepo.Tests")]

namespace Mkb.DapperRepo.Reflection
{
    internal class EntityPropertyInfo
    {
        private Dictionary<string, PropertyInfo[]> QuickNameLookUp;
        public PropertyInfo Id { get; }
        public PropertyColName IdColNameDetails => ClassPropertyColNamesDetails[Id.Name];
        internal IEnumerable<PropertyInfo> All { get; }
        public IEnumerable<PropertyInfo> AllNonId => All.Where(f => f != Id);

        public Dictionary<string, PropertyColName> ClassPropertyColNamesDetails { get; }
        public Dictionary<string, PropertyColName> SqlPropertyColNamesDetails { get; }

        public EntityPropertyInfo(PropertyInfo id, IEnumerable<PropertyInfo> all)
        {
            Id = id;
            All = all;
            QuickNameLookUp = all.GroupBy(e => e.Name.ToLower()).ToDictionary(e => e.Key, e => e.ToArray());
            var hold = all.Select(e => new
                {
                    details = e,
                    Attr = (RepoColumnAttribute) Attribute.GetCustomAttribute(e, typeof(RepoColumnAttribute)),
                })
                .Select(r =>
                    new PropertyColName(r.details.Name, r.Attr == null ? r.details.Name : r.Attr.Name, r.details))
                .ToArray();
            
            ClassPropertyColNamesDetails = hold.GroupBy(e => e.ClassPropertyName)
                .ToDictionary(e => e.Key, e => e.First());
            
            SqlPropertyColNamesDetails = hold.GroupBy(e => e.SqlPropertyName.ToLower())
                .ToDictionary(e => e.Key, e => e.First());
        }

        public PropertyInfo NameLookUp(string name, Type type) =>
            NameLookUp(name)?.FirstOrDefault(e => e.PropertyType == type);

        public IEnumerable<PropertyInfo> NameLookUp(string name)
        {
            if (QuickNameLookUp.TryGetValue(name.ToLower(), out var items))
            {
                return items;
            }

            return null;
        }
    }

    internal class PropertyColName
    {
        public PropertyColName(string classPropertyName, string sqlPropertyName, PropertyInfo propertyInfo)
        {
            this.PropertyInfo = propertyInfo;
            this.SqlPropertyName = sqlPropertyName;
            this.ClassPropertyName = classPropertyName;
        }

        public string SqlPropertyName { get; }
        public string ClassPropertyName { get; }

        public PropertyInfo PropertyInfo { get; }
    }
}