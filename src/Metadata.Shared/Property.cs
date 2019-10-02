using OneCSharp.Persistence.Shared;
using System;

namespace OneCSharp.Metadata.Shared
{
    [TypeCode(3)]
    public sealed class Property : MetadataObject
    {
        public Property() : base() { }
        public Property(Guid key) : base(key) { }
        public override int TypeCode { get { return 4; } }
    }
}
