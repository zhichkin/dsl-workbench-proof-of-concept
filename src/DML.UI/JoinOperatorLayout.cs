using OneCSharp.AST.UI;
using OneCSharp.DML.Model;

namespace OneCSharp.DML.UI
{
    public sealed class JoinOperatorLayout : ConceptLayout<JoinOperatorConcept>
    {
        public override ISyntaxNodeViewModel Layout(JoinOperatorConcept concept)
        {
            return (new ConceptNodeViewModel(null, concept))
                .Selector().Bind(nameof(concept.Operator))
                .Keyword("JOIN")
                .Concept().Bind(nameof(concept.TableReference))
                .NewLine()
                //.Keyword("ON")
                .Concept().Bind(nameof(concept.Predicate));
        }
    }
}