using OneCSharp.Persistence.Shared;
using System;

namespace OneCSharp.Metadata.Shared
{
    [TypeCode(7)]
    public sealed class Relation : MetadataObject
    {
        public Relation() : base(7) { }
        public Relation(Guid key) : base(7, key) { }

        private Property property = null; // fk to owing property
        private Entity entity = null; // fk to entity wich is allowed data type for this property
        private int onDelete = 0; // 0 - no action, 1 - set default, 2 - delete
        public Property Property { set { Set(value, ref property); } get { return property; } }
        public Entity Entity { set { Set(value, ref entity); } get { return entity; } }
        public int OnDelete { set { Set(value, ref onDelete); } get { return onDelete; } }
    }
}
