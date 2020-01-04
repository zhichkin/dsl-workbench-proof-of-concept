using System;

namespace OneCSharp.Core.Model
{
    public abstract class DataType : Entity
    {
        public Guid UUID { get; set; } // Guid.Empty - not used
        public int TypeCode { get; set; } // 0 - not used; < 0 - simple types; > 0 complex types
        public Namespace Owner { get; set; }
    }
}