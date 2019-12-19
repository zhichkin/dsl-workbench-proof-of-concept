using System.Collections.Generic;

namespace OneCSharp.Core
{
    public sealed class Enumeration : SimpleEntity
    {
        public SimpleEntity Type { get; set; }
        public Dictionary<string, object> Values { get; } = new Dictionary<string, object>();
    }
}