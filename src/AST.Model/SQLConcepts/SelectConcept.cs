using System.Collections.Generic;

namespace OneCSharp.AST.Model
{
    public sealed class SelectConcept : SyntaxNode
    {
        public Optional<List<SelectExpression>> Expressions { get; } = new Optional<List<SelectExpression>>();
        public FromConcept From { get; } = new FromConcept();
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
    public sealed class TableConcept : SyntaxNode, IIdentifiable
    {
        private const string PLACEHOLDER = "<table>";
        public string Identifier { get; set; } = PLACEHOLDER;
    }
}