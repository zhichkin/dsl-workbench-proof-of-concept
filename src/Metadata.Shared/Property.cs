using OneCSharp.Persistence.Shared;
using System;

namespace OneCSharp.Metadata.Shared
{
    [TypeCode(4)]
    public sealed class Property : MetadataObject
    {
        public Property() : base(4) { }
        public Property(Guid key) : base(4, key) { }
        public Entity Entity { get; set; }
        public PropertyPurpose Purpose { get; set; }
        public int Ordinal { get; set; }
        public bool IsAbstract { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsPrimaryKey { get; set; }
    }
}
