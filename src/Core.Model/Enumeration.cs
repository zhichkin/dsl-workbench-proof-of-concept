using System.Collections.Generic;

namespace OneCSharp.Core
{
    public sealed class Enumeration : SimpleEntity
    {
        public Entity EnumType { get; set; }
        public Dictionary<string, object> Values { get; } = new Dictionary<string, object>();
    }
}