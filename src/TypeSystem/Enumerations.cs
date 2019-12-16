using System.Collections.Generic;

namespace OneCSharp
{
    public sealed class Enumeration : IEnumeration
    {
        public Enumeration(string name, IValueType parent)
        {
            Name = name;
            BaseType = parent;
        }
        public string Name { get; }
        public IValueType BaseType { get; }
        public Dictionary<string, object> Values { get; } = new Dictionary<string, object>();
    }
}