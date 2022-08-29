namespace Mkb.DapperRepo.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Property)]  
    public class SqlColumnNameAttribute :System.Attribute  
    {
        public string Name { get; }
        public SqlColumnNameAttribute(string name)
        {
            Name = name;
        }
    }
}