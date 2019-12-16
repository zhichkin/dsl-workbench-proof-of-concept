using System;
using System.Collections.Generic;

namespace OneCSharp
{
    public interface IType
    {
        string Name { get; }
    }
    public interface IValueType : IType
    {
        
    }
    public interface IEnumeration : IValueType
    {
        IValueType BaseType { get; }
        Dictionary<string, object> Values { get; }
    }
    public interface IReferenceType : IType
    {
        IReferenceType BaseType { get; set; }
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
        public static Type GetSystemType(IValueType type)
        {
            if (type == null) return null;
            
            Type test = type.GetType();
            if (test == typeof(BinaryType)) return typeof(byte[]);
            else if (test == typeof(BooleanType)) return typeof(bool);
            else if (test == typeof(NumericType)) return typeof(int);
            else if (test == typeof(StringType)) return typeof(string);
            else if (test == typeof(DateTimeType)) return typeof(DateTime);
            else if (test == typeof(UUIDType)) return typeof(Guid);
            else return null;
        }


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
