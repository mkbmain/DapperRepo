namespace Mkb.DapperRepo.Attributes
{
    // as we know you can't always name your table in c# the same as sql e.g legacy tables SOMTHING_LUT or person vs persons
    [System.AttributeUsage(System.AttributeTargets.Class)]  
    public class SqlTableNameAttribute : System.Attribute
    {
        public string Name { get; }

        public SqlTableNameAttribute(string name)
        {
            Name = name;
        }
    }
}