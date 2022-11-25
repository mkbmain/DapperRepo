using System;

namespace Mkb.DapperRepo.Search
{
    public class SearchCriteria
    {
        public static SearchCriteria Create(string propertyName, SearchType searchType) => new SearchCriteria
            { PropertyName = propertyName, SearchType = searchType };

        public string PropertyName { get; set; }
        public SearchType SearchType { get; set; }
    }
}