namespace OneCSharp.Metadata.Model
{
    public interface ITypeInfo
    {
        public int TypeCode { get; set; }
        public string Name { get; set; }
        public string UUID { get; set; }
        public IEntity Entity { get; set; }
    }
    public sealed class TypeInfo : ITypeInfo
    {
        public int TypeCode { get; set; }
        public string Name { get; set; }
        public string UUID { get; set; }
        public IEntity Entity { get; set; }
    }
}
