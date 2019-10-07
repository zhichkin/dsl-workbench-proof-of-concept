using OneCSharp.Persistence.Shared;
using System;

namespace OneCSharp.Metadata.Shared
{
    [TypeCode(2)]
    public sealed class Namespace : MetadataObject
    {
        public Namespace() : base() { }
        public Namespace(Guid key) : base(key) { }
        public override int TypeCode { get { return 2; } }

        private ObjectReference _owner = null; // InfoBase | Namespace
        public ObjectReference Owner { set { Set<ObjectReference>(value, ref _owner); } get { return Get<ObjectReference>(ref _owner); } }
    }
}
