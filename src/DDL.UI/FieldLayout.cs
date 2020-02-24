using OneCSharp.AST.UI;
using OneCSharp.DDL.Model;

namespace OneCSharp.DDL.UI
{
    public sealed class FieldLayout : ConceptLayout<FieldConcept>
    {
        public override ISyntaxNodeViewModel Layout(FieldConcept concept)
        {
            return (new ConceptNodeViewModel(null, concept))
                .Keyword("FIELD")
                .Identifier()
                .Selector().Bind(nameof(concept.ValueType));
        }
    }
}