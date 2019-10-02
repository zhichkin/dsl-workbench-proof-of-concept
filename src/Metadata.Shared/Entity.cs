using OneCSharp.Persistence.Shared;
using System;

namespace OneCSharp.Metadata.Shared
{
    [TypeCode(3)]
    public sealed class Entity : MetadataObject
    {
        public Entity() : base() { }
        public Entity(Guid key) : base(key) { }
        public override int TypeCode { get { return 3; } }
    }
}
