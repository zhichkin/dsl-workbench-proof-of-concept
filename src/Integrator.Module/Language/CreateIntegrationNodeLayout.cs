using OneCSharp.AST.UI;
using OneCSharp.Integrator.Model;

namespace OneCSharp.Integrator.Module
{
    public sealed class CreateIntegrationNodeLayout : ConceptLayout<CreateIntegrationNodeConcept>
    {
        public override ISyntaxNodeViewModel Layout(CreateIntegrationNodeConcept concept)
        {
            return (new ConceptNodeViewModel(null, concept))
                .Keyword("CREATE NODE").Identifier()
                //.NewLine().Indent().Keyword("OWNER").Selector().Bind(nameof(concept.Owner))
                .NewLine().Indent().Keyword("ADDRESS").Selector().Bind(nameof(concept.Address))
                .NewLine().Indent().Keyword("SERVER").Selector().Bind(nameof(concept.Server))
                .NewLine().Indent().Keyword("DATABASE").Selector().Bind(nameof(concept.Database));
        }
    }
}