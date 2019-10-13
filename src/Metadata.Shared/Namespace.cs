using OneCSharp.Persistence.Shared;
using System;

namespace OneCSharp.Metadata.Shared
{
    [TypeCode(2)]
    public sealed class Namespace : MetadataObject
    {
        public Namespace() : base(2) { }
        public Namespace(Guid key) : base(2, key) { }

        private ReferenceObject _owner = null; // InfoBase | Namespace
        public ReferenceObject Owner { set { Set(value, ref _owner); } get { return _owner; } }
    }
}
