namespace OneCSharp.Core.Model
{
    public class Parameter : Entity
    {
        public Method Owner { get; set; }
        public DataType ValueType { get; set; }
        public object DefaultValue { get; set; }
    }
}