using OneCSharp.Persistence.Shared;
using System;

namespace OneCSharp.Metadata.Shared
{
    [TypeCode(3)]
    public sealed class Entity : MetadataObject
    {
        public Entity() : base(3) { }
        public Entity(Guid key) : base(3, key) { }

        private int _code = 0; // type code
        private ReferenceObject _table = null; // Table
        private ReferenceObject _namespace = null; // Namespace
        private ReferenceObject _owner = null; // Entity (nesting)
        private ReferenceObject _parent = null; // Entity (inheritance)
        private bool _isSealed = false;
        private bool _isAbstract = false;

        ///<summary>Type code of the entity</summary>
        public int Code { set { Set(value, ref _code); } get { return _code; } }
        public ReferenceObject Table { set { Set(value, ref _table); } get { return _table; } }
        public ReferenceObject Namespace { set { Set(value, ref _namespace); } get { return _namespace; } }
        ///<summary>Nesting entity reference</summary>
        public ReferenceObject Owner { set { Set(value, ref _owner); } get { return _owner; } }
        ///<summary>Inheritance: base entity reference</summary>
        public ReferenceObject Parent { set { Set(value, ref _parent); } get { return _parent; } }
        public bool IsSealed { set { Set(value, ref _isSealed); } get { return _isSealed; } }
        public bool IsAbstract { set { Set(value, ref _isAbstract); } get { return _isAbstract; } }
    }
}
