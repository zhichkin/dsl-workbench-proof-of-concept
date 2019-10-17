using OneCSharp.Persistence.Shared;
using System;

namespace OneCSharp.Metadata.Shared
{
    [TypeCode(6)]
    public sealed class Field : MetadataObject
    {
        public Field() : base(6) { }
        public Field(Guid key) : base(6, key) { }
        public Table Table { get; set; }
        public Property Property { get; set; }
        public FieldPurpose Purpose { get; set; } = FieldPurpose.Value;
        public string TypeName { get; set; }
        public int Length { get; set; }
        public int Precision { get; set; }
        public int Scale { get; set; }
        public bool IsNullable { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsPrimaryKey { get; set; }
        public byte KeyOrdinal { get; set; }
    }
}
