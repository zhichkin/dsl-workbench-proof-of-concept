using OneCSharp.AST.UI;
using OneCSharp.DDL.Model;

namespace OneCSharp.DDL.UI
{
    public sealed class PropertyLayout : ConceptLayout<PropertyConcept>
    {
        public override ISyntaxNodeViewModel Layout(PropertyConcept concept)
        {
            return (new ConceptNodeViewModel(null, concept))
                .Keyword("PROPERTY")
                .Identifier()
                .Selector().Bind(nameof(concept.ValueType))
                .Repeatable().Bind(nameof(concept.Fields));
        }
    }
}