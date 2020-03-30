using OneCSharp.AST.Model;
using System.Collections.Generic;

namespace OneCSharp.DML.Model
{
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
        public ColumnConcept ColumnReference { get; set; }
    }
    public sealed class FromConcept : SyntaxNode
    {
        public List<TableConcept> Expressions { get; } = new List<TableConcept>();
    }
    public sealed class WhereConcept : SyntaxNode
    {
        public List<TableConcept> Expressions { get; } = new List<TableConcept>();
    }
}