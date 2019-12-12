namespace OneCSharp
{
    public sealed class BinaryType : ValueType
    {
        public override string Name { get { return nameof(TypeSystem.Binary); } }
    }
    public sealed class BooleanType : ValueType
    {
        public override string Name { get { return nameof(TypeSystem.Boolean); } }
    }
    public sealed class NumericType : ValueType
    {
        public override string Name { get { return nameof(TypeSystem.Numeric); } }
    }
    public sealed class StringType : ValueType
    {
        public override string Name { get { return nameof(TypeSystem.String); } }
    }
    public sealed class DateTimeType : ValueType
    {
        public override string Name { get { return nameof(TypeSystem.DateTime); } }
    }
    public sealed class UUIDType : ValueType
    {
        public override string Name { get { return nameof(TypeSystem.UUID); } }
    }
}
