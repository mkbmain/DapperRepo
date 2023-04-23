using System.Reflection;

namespace Mkb.DapperRepo.Reflection
{
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