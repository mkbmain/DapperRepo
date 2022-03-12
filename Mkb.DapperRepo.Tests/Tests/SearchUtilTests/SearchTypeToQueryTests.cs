using Mkb.DapperRepo.Search;
using NUnit.Framework;

namespace Mkb.DapperRepo.Tests.Tests.SearchUtilTests
{
    public class SearchTypeToQueryTests
    {
        [Test]
        public void Ensure_GreaterThan()
        {
            Assert.AreEqual(">", SearchCriteriaHelper.SearchTypeToQuery(SearchType.GreaterThan));
        }
        
        [Test]
        public void Ensure_GreaterThanEqualTo()
        {
            Assert.AreEqual(">=", SearchCriteriaHelper.SearchTypeToQuery(SearchType.GreaterThanEqualTo));
        }
        
        [Test]
        public void Ensure_Equals()
        {
            Assert.AreEqual("=", SearchCriteriaHelper.SearchTypeToQuery(SearchType.Equals));
        }
        
        [Test]
        public void Ensure_LessThan()
        {
            Assert.AreEqual("<", SearchCriteriaHelper.SearchTypeToQuery(SearchType.LessThan));
        }
        
        [Test]
        public void Ensure_LessThanEqualTo()
        {
            Assert.AreEqual("<=", SearchCriteriaHelper.SearchTypeToQuery(SearchType.LessThanEqualTo));
        }

        [Test]
        public void Ensure_Like()
        {
            Assert.AreEqual("like", SearchCriteriaHelper.SearchTypeToQuery(SearchType.Like));
        }
    }
}