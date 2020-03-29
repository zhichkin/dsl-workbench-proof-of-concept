using OneCSharp.AST.Model;
using System;
using System.Collections.Generic;

namespace OneCSharp.DML.Model
{
    public sealed class FunctionConcept : SyntaxRoot, IIdentifiable
    {
        private const string PLACEHOLDER = "<function name>";
        public string Identifier { get; set; } = PLACEHOLDER;
        [TypeConstraint(typeof(int))] public Optional<Type> ReturnType { get; } = new Optional<Type>();
        public Optional<List<ParameterConcept>> Parameters { get; } = new Optional<List<ParameterConcept>>();
        public Optional<List<VariableConcept>> Variables { get; } = new Optional<List<VariableConcept>>();
        public Optional<List<SelectConcept>> Statements { get; } = new Optional<List<SelectConcept>>();
    }
    public sealed class ParameterConcept : SyntaxNode, IIdentifiable
    {
        private const string PLACEHOLDER = "<parameter name>";
        public string Identifier { get; set; } = PLACEHOLDER;
        public SimpleDataType ParameterType { get; set; }
        public Optional<bool> IsOutput { get; } = new Optional<bool>();
    }
    public sealed class VariableConcept : SyntaxNode, IIdentifiable
    {
        private const string PLACEHOLDER = "<variable name>";
        public string Identifier { get; set; } = PLACEHOLDER;
        public SimpleDataType VariableType { get; set; }
        public override string ToString()
        {
            return $"{Identifier} ({VariableType})";
        }
    }
    public sealed class SelectConcept : SyntaxNode
    {
        public SelectConcept()
        {
            FROM = new FromConcept() { Parent = this };
        }
        public Optional<bool> IsDistinct { get; } = new Optional<bool>();
        [TypeConstraint(typeof(int), typeof(ParameterConcept), typeof(VariableConcept))]
        public Optional<object> TopExpression { get; } = new Optional<object>();
        public Optional<List<SelectExpression>> Expressions { get; } = new Optional<List<SelectExpression>>();
        public FromConcept FROM { get; set; }
        public Optional<WhereConcept> WHERE { get; } = new Optional<WhereConcept>();
    }
    public sealed class SelectExpression : SyntaxNode, IIdentifiable
    {
        private const string PLACEHOLDER = "<alias>";
        public string Identifier { get; set; } = PLACEHOLDER;
        public object ColumnReference { get; set; }
    }
    public sealed class FromConcept : SyntaxNode
    {
        public List<TableConcept> Expressions { get; } = new List<TableConcept>();
    }
    public sealed class WhereConcept : SyntaxNode
    {
        public List<TableConcept> Expressions { get; } = new List<TableConcept>();
    }
    public sealed class TableConcept : ComplexDataType, IIdentifiable
    {
        private const string PLACEHOLDER = "<table>";
        public TableConcept() { Identifier = PLACEHOLDER; }
        [TypeConstraint(typeof(ComplexDataType))] public Type TableDefinition { get; set; }
        public Optional<TableHints> Hint { get; set; } = new Optional<TableHints>();
    }
    public enum TableHints
    {
        NO_LOCK,
        ROW_LOCK,
        READ_PAST,
        READ_COMMITED,
        READ_UNCOMMITED,
        READ_REPEATABLE,
        SERIALIZABLE
    }
}