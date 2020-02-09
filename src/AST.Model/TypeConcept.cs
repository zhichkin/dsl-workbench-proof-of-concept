using System;
using System.ComponentModel;

namespace OneCSharp.AST.Model
{
    public static class SimpleTypes
    {
        static SimpleTypes()
        {
            Binary = new BinaryDataType();
            String = new StringDataType();
            Boolean = new BooleanDataType();
            Numeric = new NumericDataType();
            DateTime = new DateTimeDataType();
            UniqueIdentifier = new UniqueIdentifierDataType();
            Types = new Type[]
            {
                typeof(BinaryDataType),
                typeof(StringDataType),
                typeof(BooleanDataType),
                typeof(NumericDataType),
                typeof(DateTimeDataType),
                typeof(UniqueIdentifierDataType)
            };
            References = new ISyntaxNode[]
            {
                Binary,
                String,
                Boolean,
                Numeric,
                DateTime,
                UniqueIdentifier
            };
            DotNetTypes = new Type[]
            {
                typeof(int),
                typeof(bool),
                typeof(string),
                typeof(decimal),
                typeof(Guid),
                typeof(DateTime),
                typeof(byte[])
            };
        }
        public static readonly Type[] Types;
        public static readonly Type[] DotNetTypes;
        public static readonly ISyntaxNode[] References;
        
        public static readonly SimpleDataType Binary;
        public static readonly SimpleDataType String;
        public static readonly SimpleDataType Boolean;
        public static readonly SimpleDataType Numeric;
        public static readonly SimpleDataType DateTime;
        public static readonly SimpleDataType UniqueIdentifier;
    }
    [Description("Data types")]
    public abstract class DataType : SyntaxNode, IIdentifiable
    {
        public string Identifier { get; set; }
        public override string ToString() { return Identifier; }
    }
    [Description("Simple data types")] public abstract class SimpleDataType : DataType { }
    [Description("Complex data types")] public abstract class ComplexDataType : DataType { }
    public sealed class BinaryDataType : SimpleDataType { public BinaryDataType() { Identifier = nameof(SimpleTypes.Binary); } }
    public sealed class StringDataType : SimpleDataType { public StringDataType() { Identifier = nameof(SimpleTypes.String); } }
    public sealed class BooleanDataType : SimpleDataType { public BooleanDataType() { Identifier = nameof(SimpleTypes.Boolean); } }
    public sealed class NumericDataType : SimpleDataType { public NumericDataType() { Identifier = nameof(SimpleTypes.Numeric); } }
    public sealed class DateTimeDataType : SimpleDataType { public DateTimeDataType() { Identifier = nameof(SimpleTypes.DateTime); } }
    public sealed class UniqueIdentifierDataType : SimpleDataType { public UniqueIdentifierDataType() { Identifier = nameof(SimpleTypes.UniqueIdentifier); } }
}