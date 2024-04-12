using Mkb.DapperRepo.Search;
using Xunit;

namespace Mkb.DapperRepo.Tests.Tests.SearchUtilTests
{
    public class SearchTypeToQueryTests
    {
        [Fact]
        public void Ensure_GreaterThan()
        {
            Assert.Equal(">", SearchCriteriaHelper.SearchTypeToQuery(SearchType.GreaterThan));
        }
        
        [Fact]
        public void Ensure_GreaterThanEqualTo()
        {
            Assert.Equal(">=", SearchCriteriaHelper.SearchTypeToQuery(SearchType.GreaterThanEqualTo));
        }
        
        [Fact]
        public void Ensure_Equals()
        {
            Assert.Equal("=", SearchCriteriaHelper.SearchTypeToQuery(SearchType.Equals));
        }
        
        [Fact]
        public void Ensure_LessThan()
        {
            Assert.Equal("<", SearchCriteriaHelper.SearchTypeToQuery(SearchType.LessThan));
        }
        
        [Fact]
        public void Ensure_LessThanEqualTo()
        {
            Assert.Equal("<=", SearchCriteriaHelper.SearchTypeToQuery(SearchType.LessThanEqualTo));
        }

        [Fact]
        public void Ensure_Like()
        {
            Assert.Equal("like", SearchCriteriaHelper.SearchTypeToQuery(SearchType.Like));
        }
    }
}