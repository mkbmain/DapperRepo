namespace Mkb.DapperRepo.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Property)]  
    public class RepoColumnAttribute :System.Attribute  
    {
        public string Name { get; }
        public RepoColumnAttribute(string name)
        {
            Name = name;
        }
    }
}