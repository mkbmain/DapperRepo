namespace Mkb.DapperRepo.Search
{
    public enum SearchType
    {
        /// <summary>
        /// Is null,This will ignore any value of a property and just search for null
        /// </summary>
        IsNull,
        /// <summary>
        /// =
        /// </summary>
        Equals,
        /// <summary>
        /// Like
        /// </summary>
        Like,
        /// <summary>
        /// >
        /// </summary>
        GreaterThan,
        /// <summary>
        /// <
        /// </summary>
        LessThan,
        /// <summary>
        /// >=
        /// </summary>
        GreaterThanEqualTo,
        /// <summary>
        /// <=
        /// </summary>
        LessThanEqualTo
    }
}