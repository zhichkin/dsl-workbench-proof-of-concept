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

        private int _code = 0; // type code
        private ObjectReference _table = null; // Table
        private ObjectReference _namespace = null; // Namespace
        private ObjectReference _owner = null; // Entity (nesting)
        private ObjectReference _parent = null; // Entity (inheritance)
        private bool _isSealed = false;
        private bool _isAbstract = false;

        ///<summary>Type code of the entity</summary>
        public int Code { set { Set(value, ref _code); } get { return Get(ref _code); } }
        public ObjectReference Table { set { Set(value, ref _table); } get { return Get(ref _table); } }
        public ObjectReference Namespace { set { Set(value, ref _namespace); } get { return Get(ref _namespace); } }
        ///<summary>Nesting entity reference</summary>
        public ObjectReference Owner { set { Set(value, ref _owner); } get { return Get(ref _owner); } }
        ///<summary>Inheritance: base entity reference</summary>
        public ObjectReference Parent { set { Set(value, ref _parent); } get { return Get(ref _parent); } }
        public bool IsSealed { set { Set(value, ref _isSealed); } get { return Get(ref _isSealed); } }
        public bool IsAbstract { set { Set(value, ref _isAbstract); } get { return Get(ref _isAbstract); } }
    }
}
