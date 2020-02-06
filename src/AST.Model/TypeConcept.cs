using System;
using System.ComponentModel;

namespace OneCSharp.AST.Model
{
    public static class SimpleTypes
    {
        static SimpleTypes()
        {
            Binary = new BinaryTypeConcept();
            String = new StringTypeConcept();
            Boolean = new BooleanTypeConcept();
            Numeric = new NumericTypeConcept();
            DateTime = new DateTimeTypeConcept();
            UniqueIdentifier = new UniqueIdentifierTypeConcept();
            Types = new Type[]
            {
                typeof(BinaryTypeConcept),
                typeof(StringTypeConcept),
                typeof(BooleanTypeConcept),
                typeof(NumericTypeConcept),
                typeof(DateTimeTypeConcept),
                typeof(UniqueIdentifierTypeConcept)
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
        
        public static readonly SimpleTypeConcept Binary;
        public static readonly SimpleTypeConcept String;
        public static readonly SimpleTypeConcept Boolean;
        public static readonly SimpleTypeConcept Numeric;
        public static readonly SimpleTypeConcept DateTime;
        public static readonly SimpleTypeConcept UniqueIdentifier;
    }
    [Description("Data types")]
    public abstract class DataType : SyntaxNode, IIdentifiable
    {
        public string Identifier { get; set; }
        public override string ToString() { return Identifier; }
    }
    [Description("Simple data types")] public abstract class SimpleTypeConcept : DataType { }
    [Description("Complex data types")] public abstract class ComplexTypeConcept : DataType { }
    public sealed class BinaryTypeConcept : SimpleTypeConcept { public BinaryTypeConcept() { Identifier = nameof(SimpleTypes.Binary); } }
    public sealed class StringTypeConcept : SimpleTypeConcept { public StringTypeConcept() { Identifier = nameof(SimpleTypes.String); } }
    public sealed class BooleanTypeConcept : SimpleTypeConcept { public BooleanTypeConcept() { Identifier = nameof(SimpleTypes.Boolean); } }
    public sealed class NumericTypeConcept : SimpleTypeConcept { public NumericTypeConcept() { Identifier = nameof(SimpleTypes.Numeric); } }
    public sealed class DateTimeTypeConcept : SimpleTypeConcept { public DateTimeTypeConcept() { Identifier = nameof(SimpleTypes.DateTime); } }
    public sealed class UniqueIdentifierTypeConcept : SimpleTypeConcept { public UniqueIdentifierTypeConcept() { Identifier = nameof(SimpleTypes.UniqueIdentifier); } }
}