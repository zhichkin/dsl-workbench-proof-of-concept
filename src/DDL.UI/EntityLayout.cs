using OneCSharp.AST.UI;
using OneCSharp.DDL.Model;

namespace OneCSharp.DDL.UI
{
    public sealed class EntityLayout : ConceptLayout<EntityConcept>
    {
        public override ISyntaxNodeViewModel Layout(EntityConcept concept)
        {
            return (new ConceptNodeViewModel(null, concept))
                .Keyword("ENTITY")
                .Identifier()
                .Repeatable().Bind(nameof(concept.Properties));
        }
    }
}