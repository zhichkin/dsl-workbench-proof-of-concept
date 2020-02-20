using System;
using System.Collections.Generic;

namespace OneCSharp.AST.Model
{
    public sealed class SelectConcept : SyntaxNode
    {
        public SelectConcept()
        {
            From = new FromConcept() { Parent = this };
        }
        public Optional<bool> IsDistinct { get; } = new Optional<bool>();
        [TypeConstraint(typeof(int), typeof(ParameterConcept), typeof(VariableConcept))]
        public Optional<object> TopExpression { get; } = new Optional<object>();
        public Optional<List<SelectExpression>> Expressions { get; } = new Optional<List<SelectExpression>>();
        public FromConcept From { get; private set; }
        public Optional<WhereConcept> Where { get; } = new Optional<WhereConcept>();
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
    }
}