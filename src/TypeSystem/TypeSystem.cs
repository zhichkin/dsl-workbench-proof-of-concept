namespace OneCSharp.TypeSystem
{
    public interface IType
    {
        string Name { get; }
        IType BaseType { get; }
        bool IsValueType { get; }
    }
    public abstract class ValueType : IType
    {
        public abstract string Name { get; }
        public IType BaseType { get; protected set; }
        public bool IsValueType { get { return true; } }
    }
    public static class TypeSystem
    {
        static TypeSystem()
        {
            Binary = new BinaryType();
            Boolean = new BooleanType();
            Numeric = new NumericType();
            String = new StringType();
            DateTime = new DateTimeType();
            UUID = new UUIDType();
        }
        public static BinaryType Binary { get; private set; }
        public static BooleanType Boolean { get; private set; }
        public static NumericType Numeric { get; private set; }
        public static StringType String { get; private set; }
        public static DateTimeType DateTime { get; private set; }
        public static UUIDType UUID { get; private set; }
    }
}
