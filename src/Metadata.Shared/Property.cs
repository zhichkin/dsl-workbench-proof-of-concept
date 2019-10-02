using OneCSharp.Persistence.Shared;
using System;

namespace OneCSharp.Metadata.Shared
{
    [TypeCode(4)]
    public sealed class Property : MetadataObject
    {
        public Property() : base() { }
        public Property(Guid key) : base(key) { }
        public override int TypeCode { get { return 4; } }

        private Entity entity = null; // Entity - owner of the property
        private PropertyPurpose purpose = PropertyPurpose.Property; // purpose of the property
        private int ordinal = 0;
        private bool isAbstract = false;
        private bool isReadOnly = false;
        private bool isPrimaryKey = false;

        public Entity Entity { set { Set<Entity>(value, ref entity); } get { return Get<Entity>(ref entity); } }
        public PropertyPurpose Purpose { set { Set<PropertyPurpose>(value, ref purpose); } get { return Get<PropertyPurpose>(ref purpose); } }
        public int Ordinal { set { Set<int>(value, ref ordinal); } get { return Get<int>(ref ordinal); } }
        public bool IsAbstract { set { Set<bool>(value, ref isAbstract); } get { return Get<bool>(ref isAbstract); } }
        public bool IsReadOnly { set { Set<bool>(value, ref isReadOnly); } get { return Get<bool>(ref isReadOnly); } }
        public bool IsPrimaryKey { set { Set<bool>(value, ref isPrimaryKey); } get { return Get<bool>(ref isPrimaryKey); } }
    }
}
