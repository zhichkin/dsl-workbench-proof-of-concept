using System;
using System.Collections.Generic;

namespace OneCSharp
{
    public abstract class Enumeration<T> : ValueType
    {
        private readonly string _name;
        public Enumeration(string name, ValueType parent)
        {
            _name = name;
            BaseType = parent;
        }
        public override string Name { get { return _name; } }
        public Dictionary<string, T> Values { get; } = new Dictionary<string, T>();
    }
    public sealed class NumericEnumeration : Enumeration<int>
    {
        public NumericEnumeration(string name) : base(name, TypeSystem.Numeric) { }
    }
    public sealed class StringEnumeration : Enumeration<string>
    {
        public StringEnumeration(string name) : base(name, TypeSystem.String) { }
    }
    public sealed class UUIDEnumeration : Enumeration<Guid>
    {
        public UUIDEnumeration(string name) : base(name, TypeSystem.UUID) { }
    }
}
