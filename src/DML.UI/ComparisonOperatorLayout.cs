using OneCSharp.AST.UI;
using OneCSharp.DML.Model;

namespace OneCSharp.DML.UI
{
    public sealed class ComparisonOperatorLayout : ConceptLayout<ComparisonOperatorConcept>
    {
        public override ISyntaxNodeViewModel Layout(ComparisonOperatorConcept concept)
        {
            return (new ConceptNodeViewModel(null, concept))
                .Selector().Bind(nameof(concept.LeftExpression))
                .Selector().Bind(nameof(concept.Operator))
                .Selector().Bind(nameof(concept.RightExpression));
        }
    }
}