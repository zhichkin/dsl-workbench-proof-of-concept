using OneCSharp.AST.UI;
using OneCSharp.DDL.Model;

namespace OneCSharp.DDL.UI
{
    public sealed class UseDatabaseLayout : ConceptLayout<UseDatabaseConcept>
    {
        public override ISyntaxNodeViewModel Layout(UseDatabaseConcept concept)
        {
            return (new ConceptNodeViewModel(null, concept))
                .Keyword("USE DATABASE")
                .Selector().Bind(nameof(concept.Assembly));
        }
    }
}