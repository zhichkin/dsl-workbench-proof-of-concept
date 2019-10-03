using OneCSharp.Persistence.Shared;
using System;

namespace OneCSharp.Metadata.Shared
{
    [TypeCode(7)]
    public sealed class Relation : MetadataObject
    {
        public Relation() : base() { }
        public Relation(Guid key) : base(key) { }
        public override int TypeCode { get { return 7; } }

        private Property property = null; // fk to owing property
        private Entity entity = null; // fk to entity wich is allowed data type for this property
        private int onDelete = 0; // 0 - no action, 1 - set default, 2 - delete
        public Property Property { set { Set<Property>(value, ref property); } get { return Get<Property>(ref property); } }
        public Entity Entity { set { Set<Entity>(value, ref entity); } get { return Get<Entity>(ref entity); } }
        public int OnDelete { set { Set<int>(value, ref onDelete); } get { return Get<int>(ref onDelete); } }
    }
}
