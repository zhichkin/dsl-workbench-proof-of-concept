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
        private Table _table = null;
        private Namespace _namespace = null; // Namespace
        private Entity owner = null; // Nesting
        private Entity parent = null; // Inheritance
        private bool isSealed = false;
        private bool isAbstract = false;

        ///<summary>Type code of the entity</summary>
        public int Code { set { Set<int>(value, ref _code); } get { return Get<int>(ref _code); } }
        public Table Table { set { Set<Table>(value, ref _table); } get { return Get<Table>(ref _table); } }
        public Namespace Namespace { set { Set<Namespace>(value, ref _namespace); } get { return Get<Namespace>(ref _namespace); } }
        ///<summary>Nesting entity reference</summary>
        public Entity Owner { set { Set<Entity>(value, ref owner); } get { return Get<Entity>(ref owner); } }
        ///<summary>Inheritance: base entity reference</summary>
        public Entity Parent { set { Set<Entity>(value, ref parent); } get { return Get<Entity>(ref parent); } }
        public bool IsSealed { set { Set<bool>(value, ref isSealed); } get { return Get<bool>(ref isSealed); } }
        public bool IsAbstract { set { Set<bool>(value, ref isAbstract); } get { return Get<bool>(ref isAbstract); } }
    }
}
