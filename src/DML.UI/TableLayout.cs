using OneCSharp.AST.UI;
using OneCSharp.DML.Model;

namespace OneCSharp.DML.UI
{
    public sealed class TableConceptLayout : ConceptLayout<TableConcept>
    {
        public override ISyntaxNodeViewModel Layout(TableConcept concept)
        {
            return (new ConceptNodeViewModel(null, concept))
                .Identifier()
                .Selector().Bind(nameof(concept.TableDefinition))
                .Property(nameof(concept.Hint))
                    .Keyword("WITH")
                    .Literal("(")
                    .Selector()
                    .Literal(")");
        }
    }
}