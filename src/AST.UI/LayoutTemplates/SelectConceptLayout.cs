using OneCSharp.AST.Model;

namespace OneCSharp.AST.UI
{
    public sealed class SelectConceptLayout : IConceptLayout
    {
        public ISyntaxNodeViewModel Layout(ISyntaxNode model)
        {
            SelectConcept concept;
            return (new ConceptNodeViewModel(null, model))
                .Keyword("SELECT")
                .Repeatable().Bind(nameof(concept.SelectExpressions));
        }
    }
    public sealed class SelectExpressionLayout : IConceptLayout
    {
        public ISyntaxNodeViewModel Layout(ISyntaxNode model)
        {
            SelectExpression concept;
            return (new ConceptNodeViewModel(null, model))
                .Identifier()
                .Literal(" = ")
                .Reference().Bind(nameof(concept.ColumnReference));
        }
    }
}
