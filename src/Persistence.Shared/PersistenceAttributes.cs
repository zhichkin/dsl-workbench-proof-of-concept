using System;

namespace OneCSharp.Persistence.Shared
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class TypeCodeAttribute : Attribute
    {
        public TypeCodeAttribute(int typeCode)
        {
            this.TypeCode = typeCode;
        }
        public int TypeCode { get; private set; }
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class SchemaAttribute : Attribute
    {
        public SchemaAttribute(string schema)
        {
            this.Schema = schema;
        }
        public string Schema { get; private set; }
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class TableAttribute : Attribute
    {
        public TableAttribute(string name)
        {
            this.Name = name;
        }
        public string Name { get; private set; }
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public sealed class FieldAttribute : Attribute
    {
        public FieldAttribute(string name, string typeName)
        {
            this.Name = name;
            this.TypeName = typeName;
        }
        public string Name { get; private set; }
        public string TypeName { get; private set; }
        // TODO: purpose !!!
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class PrimaryKeyAttribute : Attribute
    {
        public PrimaryKeyAttribute(string name)
        {
            this.Name = name;
        }
        public string Name { get; private set; }
    }
}
