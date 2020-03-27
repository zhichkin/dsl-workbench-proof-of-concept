using System;
using System.Collections.Generic;
using System.Linq;

namespace OneCSharp.AST.Services
{
    public interface ISerializationBinder
    {
        Type GetType(int typeCode);
        int GetTypeCode(Type type);
        Dictionary<int, Type> KnownTypes { get; }
    }
    public sealed class JsonSerializationBinder : ISerializationBinder
    {
        public Dictionary<int, Type> KnownTypes { get; } = new Dictionary<int, Type>();
        public JsonSerializationBinder() { }
        public Type GetType(int typeCode)
        {
            return KnownTypes.SingleOrDefault(i => i.Key == typeCode).Value;
        }
        public int GetTypeCode(Type type)
        {
            return KnownTypes.SingleOrDefault(i => i.Value == type).Key;
        }
    }
}