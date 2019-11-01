using System.Collections.Generic;

namespace OneCSharp.Metadata
{
    public sealed class InfoBase
    {
        public string Name;
        public string Database;
        public List<Namespace> Namespaces = new List<Namespace>();
    }
    public sealed class Namespace
    {
        public InfoBase InfoBase;
        public Namespace Parent;
        public string Name;
        public List<Namespace> Namespaces = new List<Namespace>();
        public List<DbObject> DbObjects = new List<DbObject>();
    }
    public sealed class DbObject
    {
        public Namespace Parent;
        public string Token;
        public string Name;
        public int TypeCode;
        public string TableName;
        public List<DbProperty> Properties = new List<DbProperty>();
        public DbObject Owner;
        public List<DbObject> NestedObjects = new List<DbObject>();
    }
    public sealed class DbProperty
    {
        public DbObject Parent;
        public string Name;
        public List<DbType> Types = new List<DbType>();
        public List<DbField> Fields = new List<DbField>();
    }
    public sealed class DbType
    {
        public int TypeCode;
        public string Name;
        public string UUID;
        public DbObject DbObject;
    }
    public sealed class DbField
    {
        public DbProperty Parent;
        public string Name;
        public string TypeName;
        public int Length;
        public byte Precision;
        public int Scale;
        public bool IsNullable;
        public bool IsReadOnly;
        public bool IsPrimaryKey;
        public int KeyOrdinal;
        public DbFieldPurpose Purpose;
    }
    public enum DbFieldPurpose
    {
        /// <summary>Value of the property (default).</summary>
        Value,
        /// <summary>Helps to locate fields having [boolean, string, number, binary, datetime, object] types</summary>
        Locator,
        /// <summary>Boolean value.</summary>
        Boolean,
        /// <summary>String value.</summary>
        String,
        /// <summary>Numeric value.</summary>
        Number,
        /// <summary>Binary value (bytes array).</summary>
        Binary,
        /// <summary>Date and time value.</summary>
        DateTime,
        /// <summary>Reference type primary key value.</summary>
        Object,
        /// <summary>Type code of the reference type (discriminator).</summary>
        TypeCode
    }
    public sealed class DbProcedure // !?
    {
        public string Name;
    }
}
