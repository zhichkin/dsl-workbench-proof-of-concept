using OneCSharp.Persistence.Shared;
using System;

namespace OneCSharp.Metadata.Shared
{
    [TypeCode(2)] [Table("namespaces")]
    public sealed class Namespace : MetadataObject
    {
        public Namespace() : base(2) { }
        public Namespace(Guid key) : base(2, key) { }
        [Field("owner_", "int", FieldPurpose.TypeCode)]
        [Field("owner", "uniqueidentifier", FieldPurpose.Object)]
        public ReferenceObject Owner { get; set; }
    }
}
