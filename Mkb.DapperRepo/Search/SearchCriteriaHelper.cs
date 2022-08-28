using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Mkb.DapperRepo.Tests")]
namespace Mkb.DapperRepo.Search
{
    internal class SearchCriteriaHelper
    {
        public static string SearchTypeToQuery(SearchType type)
        {
            switch (type)
            {
                case SearchType.IsNull:
                    return "is null";
                case SearchType.Equals:
                    return "=";
                case SearchType.Like:
                    return "like";
                case SearchType.GreaterThan:
                    return ">";
                case SearchType.LessThan:
                    return "<";
                case SearchType.GreaterThanEqualTo:
                    return ">=";
                case SearchType.LessThanEqualTo:
                    return "<=";
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}