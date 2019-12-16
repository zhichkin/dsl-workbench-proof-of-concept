namespace OneCSharp
{
    public sealed class BinaryType : IValueType
    {
        public string Name { get { return nameof(TypeSystem.Binary); } }
    }
    public sealed class BooleanType : IValueType
    {
        public string Name { get { return nameof(TypeSystem.Boolean); } }
    }
    public sealed class NumericType : IValueType
    {
        public string Name { get { return nameof(TypeSystem.Numeric); } }
    }
    public sealed class StringType : IValueType
    {
        public string Name { get { return nameof(TypeSystem.String); } }
    }
    public sealed class DateTimeType : IValueType
    {
        public string Name { get { return nameof(TypeSystem.DateTime); } }
    }
    public sealed class UUIDType : IValueType
    {
        public string Name { get { return nameof(TypeSystem.UUID); } }
    }
}