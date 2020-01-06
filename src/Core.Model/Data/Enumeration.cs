using System.Collections.Generic;

namespace OneCSharp.Core.Model
{
    public sealed class Enumeration : DataType
    {
        public SimpleType Type { get; set; }
        public Dictionary<string, object> Values { get; } = new Dictionary<string, object>();
    }
}