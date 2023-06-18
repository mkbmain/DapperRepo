using System.Reflection;

namespace Mkb.DapperRepo.Reflection
{
    internal record PropertyColName(string ClassPropertyName, string SqlPropertyName, PropertyInfo PropertyInfo);
}