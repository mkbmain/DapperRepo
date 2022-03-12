using System;

namespace Mkb.DapperRepo.Search
{
    public class SearchCriteriaValue : SearchCriteria
    {
        public object Value { get; set; }
        public Type PropertyType { get; set; }
    }

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