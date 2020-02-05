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
                .Repeatable().Bind(nameof(concept.Expressions))
                .Concept().Bind(nameof(concept.From))
                .Concept().Bind(nameof(concept.Where));
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
    public sealed class FromConceptLayout : IConceptLayout
    {
        public ISyntaxNodeViewModel Layout(ISyntaxNode model)
        {
            FromConcept concept;
            return (new ConceptNodeViewModel(null, model))
                .Keyword("FROM")
                .Repeatable().Bind(nameof(concept.Expressions));
        }
    }
    public sealed class WhereConceptLayout : IConceptLayout
    {
        public ISyntaxNodeViewModel Layout(ISyntaxNode model)
        {
            WhereConcept concept;
            return (new ConceptNodeViewModel(null, model))
                .Keyword("WHERE")
                .Repeatable().Bind(nameof(concept.Expressions));
        }
    }
    public sealed class TableConceptLayout : IConceptLayout
    {
        public ISyntaxNodeViewModel Layout(ISyntaxNode model)
        {
            //TableConcept concept;
            return (new ConceptNodeViewModel(null, model))
                .Identifier();
        }
    }
}