namespace CafeOrderManager.Infrastructure.Attributes
{
    public class StringValueAttribute : System.Attribute
    {
        public StringValueAttribute(string value)
        {
            Value = value;
        }
        public string Value { get; }
    }
}