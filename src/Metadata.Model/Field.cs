namespace OneCSharp.Metadata.Model
{
    public enum FieldPurpose
    {
        /// <summary>Value of the property (default).</summary>
        Value,
        /// <summary>Helps to locate fields having [boolean, string, number, binary, datetime, object] types</summary>
        Discriminator,
        /// <summary>Boolean value.</summary>
        Boolean,
        /// <summary>String value.</summary>
        String,
        /// <summary>Numeric value.</summary>
        Numeric,
        /// <summary>Binary value (bytes array).</summary>
        Binary,
        /// <summary>Date and time value.</summary>
        DateTime,
        /// <summary>Reference type primary key value.</summary>
        Object,
        /// <summary>Type code of the reference type (class discriminator).</summary>
        TypeCode,
        /// <summary>Record's version (timestamp|rowversion).</summary>
        Version
    }
    public interface IField
    {
        public IProperty Parent { get; set; }
        public string Name { get; set; }
        public string TypeName { get; set; }
        public int Length { get; set; }
        public byte Precision { get; set; }
        public int Scale { get; set; }
        public bool IsNullable { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsPrimaryKey { get; set; }
        public int KeyOrdinal { get; set; }
        public FieldPurpose Purpose { get; set; }
    }
    public sealed class Field : IField
    {
        public IProperty Parent { get; set; }
        public string Name { get; set; }
        public string TypeName { get; set; }
        public int Length { get; set; }
        public byte Precision { get; set; }
        public int Scale { get; set; }
        public bool IsNullable { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsPrimaryKey { get; set; }
        public int KeyOrdinal { get; set; }
        public FieldPurpose Purpose { get; set; }
    }
}
