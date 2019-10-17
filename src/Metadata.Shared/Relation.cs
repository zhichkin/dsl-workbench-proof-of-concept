using OneCSharp.Persistence.Shared;
using System;

namespace OneCSharp.Metadata.Shared
{
    [TypeCode(7)]
    public sealed class Relation : MetadataObject
    {
        public Relation() : base(7) { }
        public Relation(Guid key) : base(7, key) { }
        public Property Property { get; set; }
        public Entity Entity { get; set; }
        public int OnDelete { get; set; } // 0 - no action, 1 - set default, 2 - delete
    }
}
