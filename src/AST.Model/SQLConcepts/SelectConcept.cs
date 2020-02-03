using System;
using System.Collections.Generic;

namespace OneCSharp.AST.Model
{
    public sealed class SelectConcept : SyntaxNode, IScopeProvider
    {
        public Optional<List<SelectExpression>> SelectExpressions { get; } = new Optional<List<SelectExpression>>();
        public FromConcept From { get; } = new FromConcept();
        public IEnumerable<ISyntaxNode> Scope(Type scopeType)
        {
            if (scopeType == typeof(object))
            {
                return SimpleTypes.Types;
            }
            return null;
        }
    }
    public sealed class SelectExpression : SyntaxNode, IIdentifiable
    {
        private const string PLACEHOLDER = "<alias>";
        public string Identifier { get; set; } = PLACEHOLDER;
        public object ColumnReference { get; set; }
    }
    public sealed class FromConcept : SyntaxNode
    {
        public List<TableConcept> FromExpressions { get; } = new List<TableConcept>();
    }
    public sealed class TableConcept : SyntaxNode, IIdentifiable
    {
        private const string PLACEHOLDER = "<table>";
        public string Identifier { get; set; } = PLACEHOLDER;
    }
}