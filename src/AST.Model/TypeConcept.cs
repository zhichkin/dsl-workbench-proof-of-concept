using System;

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
            Types = new ISyntaxNode[]
            {
                Binary,
                String,
                Boolean,
                Numeric,
                DateTime,
                UniqueIdentifier
            };
        }
        public static readonly ISyntaxNode[] Types;
        public static readonly TypeConcept Binary;
        public static readonly TypeConcept String;
        public static readonly TypeConcept Boolean;
        public static readonly TypeConcept Numeric;
        public static readonly TypeConcept DateTime;
        public static readonly TypeConcept UniqueIdentifier;

        public static readonly Type[] List = new Type[]
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
    public abstract class TypeConcept : SyntaxNode, IIdentifiable
    {
        public string Identifier { get; set; }
        public override string ToString() { return Identifier; }
    }
    public sealed class BinaryTypeConcept : TypeConcept { public BinaryTypeConcept() { Identifier = nameof(SimpleTypes.Binary); } }
    public sealed class StringTypeConcept : TypeConcept { public StringTypeConcept() { Identifier = nameof(SimpleTypes.String); } }
    public sealed class BooleanTypeConcept : TypeConcept { public BooleanTypeConcept() { Identifier = nameof(SimpleTypes.Boolean); } }
    public sealed class NumericTypeConcept : TypeConcept { public NumericTypeConcept() { Identifier = nameof(SimpleTypes.Numeric); } }
    public sealed class DateTimeTypeConcept : TypeConcept { public DateTimeTypeConcept() { Identifier = nameof(SimpleTypes.DateTime); } }
    public sealed class UniqueIdentifierTypeConcept : TypeConcept { public UniqueIdentifierTypeConcept() { Identifier = nameof(SimpleTypes.UniqueIdentifier); } }
}