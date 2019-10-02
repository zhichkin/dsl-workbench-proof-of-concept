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

        public Property Property { set { Set<Property>(value, ref property); } get { return Get<Property>(ref property); } }
        public Entity Entity { set { Set<Entity>(value, ref entity); } get { return Get<Entity>(ref entity); } }
    }
}
