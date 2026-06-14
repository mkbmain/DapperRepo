using System;
using System.Linq.Expressions;

namespace Mkb.DapperRepo.Search
{
    public class SearchTerm<T>
    {
        public string PropertyName { get; }
        public object Value { get; }
        public SearchType SearchType { get; }

        private SearchTerm(string propertyName, object value, SearchType searchType)
        {
            PropertyName = propertyName;
            Value = value;
            SearchType = searchType;
        }

        public static SearchTerm<T> Create<TProp>(Expression<Func<T, TProp>> property, TProp value, SearchType searchType)
        {
            return new SearchTerm<T>(MemberName(property), value, searchType);
        }

        public static SearchTerm<T> IsNull<TProp>(Expression<Func<T, TProp>> property)
        {
            return new SearchTerm<T>(MemberName(property), null, SearchType.IsNull);
        }

        private static string MemberName<TProp>(Expression<Func<T, TProp>> property)
        {
            if (!(property.Body is MemberExpression member))
                throw new ArgumentException("Expression must be a simple property access, e.g. x => x.Name");
            return member.Member.Name;
        }
    }
}
