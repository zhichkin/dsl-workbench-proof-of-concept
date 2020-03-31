using OneCSharp.AST.UI;
using OneCSharp.DML.Model;

namespace OneCSharp.DML.UI
{
    public sealed class BooleanOperatorLayout : ConceptLayout<BooleanOperatorConcept>
    {
        public override ISyntaxNodeViewModel Layout(BooleanOperatorConcept concept)
        {
            return (new ConceptNodeViewModel(null, concept))
                .Selector().Bind(nameof(concept.Operator))
                .Concept().Bind(nameof(concept.Expression));
        }
    }
}