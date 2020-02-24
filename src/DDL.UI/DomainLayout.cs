using OneCSharp.AST.UI;
using OneCSharp.DDL.Model;

namespace OneCSharp.DDL.UI
{
    public sealed class DomainLayout : ConceptLayout<DomainConcept>
    {
        public override ISyntaxNodeViewModel Layout(DomainConcept concept)
        {
            return (new ConceptNodeViewModel(null, concept))
                .Keyword("DOMAIN")
                .Identifier()
                .Repeatable().Bind(nameof(concept.Entities));
        }
    }
}