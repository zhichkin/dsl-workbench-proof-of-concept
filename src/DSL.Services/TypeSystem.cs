using System;
using System.Collections.Generic;
using System.Text;

namespace OneCSharp.DSL.Services
{
    public static class TypeSystem
    {
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
