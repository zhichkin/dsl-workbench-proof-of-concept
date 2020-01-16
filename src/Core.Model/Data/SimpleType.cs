namespace OneCSharp.Core.Model
{
    public sealed class SimpleType : DataType
    {
        public SimpleType() { }
        public SimpleType(string name) { Name = name; }
        public static SimpleType NULL { get; } = new SimpleType(nameof(NULL));
        public static SimpleType Binary { get; } = new SimpleType(nameof(Binary));
        public static SimpleType Boolean { get; } = new SimpleType(nameof(Boolean));
        public static SimpleType Numeric { get; } = new SimpleType(nameof(Numeric));
        public static SimpleType String { get; } = new SimpleType(nameof(String));
        public static SimpleType DateTime { get; } = new SimpleType(nameof(DateTime));
        public static SimpleType UniqueIdentifier { get; } = new SimpleType(nameof(UniqueIdentifier));
    }
}