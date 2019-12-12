using System;
using System.Collections.Generic;

namespace OneCSharp
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


        private static readonly Dictionary<Type, string> _TypeNames = new Dictionary<Type, string>()
        {
            { typeof(Guid), "UUID" },
            { typeof(byte[]), "Binary" },
            { typeof(string), "String" },
            { typeof(int), "Numeric" },
            { typeof(bool), "Boolean" },
            { typeof(DateTime), "DateTime" },
            { typeof(long), "Numeric" },
            { typeof(decimal), "Numeric" },
            { typeof(float), "Numeric" },
            { typeof(byte), "Numeric" },
            { typeof(uint), "Numeric" },
            { typeof(sbyte), "Numeric" },
            { typeof(short), "Numeric" },
            { typeof(ushort), "Numeric" }
        };
        public static string GetTypeName(Type type)
        {
            if (type == null) return "Unknown";
            if (_TypeNames.TryGetValue(type, out string name))
            {
                return name;
            }
            return "Unknown";
        }
    }
}
