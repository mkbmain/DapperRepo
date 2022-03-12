using System;

namespace Mkb.DapperRepo.Search
{

    public class SearchCriteria
    {
        public string PropertyName { get; set; }
        public SearchType SearchType { get; set; }
    }

    public enum SearchType
    {
        Equals,
        Like,
        GreaterThan,
        LessThan,
        GreaterThanEqualTo,
        LessThanEqualTo
    }
}