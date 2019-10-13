using OneCSharp.Persistence.Shared;
using System;

namespace OneCSharp.Metadata.Shared
{
    [TypeCode(4)]
    public sealed class Property : MetadataObject
    {
        public Property() : base(4) { }
        public Property(Guid key) : base(4, key) { }

        private Entity entity = null; // Entity - owner of the property
        private PropertyPurpose purpose = PropertyPurpose.Property; // purpose of the property
        private int ordinal = 0;
        private bool isAbstract = false;
        private bool isReadOnly = false;
        private bool isPrimaryKey = false;

        public Entity Entity { set { Set(value, ref entity); } get { return entity; } }
        public PropertyPurpose Purpose { set { Set(value, ref purpose); } get { return purpose; } }
        public int Ordinal { set { Set(value, ref ordinal); } get { return ordinal; } }
        public bool IsAbstract { set { Set(value, ref isAbstract); } get { return isAbstract; } }
        public bool IsReadOnly { set { Set(value, ref isReadOnly); } get { return isReadOnly; } }
        public bool IsPrimaryKey { set { Set(value, ref isPrimaryKey); } get { return isPrimaryKey; } }
    }
}
