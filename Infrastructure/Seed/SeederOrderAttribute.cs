namespace ServiceLog.Infrastructure.Seed
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class SeederOrderAttribute : Attribute
    {
        public int Order { get; }

        public SeederOrderAttribute(int order)
        {
            Order = order;
        }
    }
}
