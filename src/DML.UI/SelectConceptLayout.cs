using OneCSharp.AST.UI;
using OneCSharp.DML.Model;

namespace OneCSharp.DML.UI
{
    public sealed class SelectConceptLayout : ConceptLayout<SelectConcept>
    {
        public override ISyntaxNodeViewModel Layout(SelectConcept concept)
        {
            return (new ConceptNodeViewModel(null, concept))
                .Keyword("SELECT")
                //.Keyword("DISTINCT").Bind(nameof(concept.IsDistinct))
                .Property(nameof(concept.IsDistinct))
                    .Keyword("DISTINCT")
                .Property(nameof(concept.TopExpression))
                    .Keyword("TOP")
                    .Literal("(")
                    .Selector()
                    .Literal(")")
                .Repeatable().Bind(nameof(concept.Expressions))
                .Concept().Bind(nameof(concept.FROM))
                .Concept().Bind(nameof(concept.WHERE));
        }
    }
    public sealed class SelectExpressionLayout : ConceptLayout<SelectExpression>
    {
        public override ISyntaxNodeViewModel Layout(SelectExpression concept)
        {
            return (new ConceptNodeViewModel(null, concept))
                .Identifier()
                .Literal(" = ")
                .Selector().Bind(nameof(concept.ColumnReference));
        }
    }
    public sealed class FromConceptLayout : ConceptLayout<FromConcept>
    {
        public override ISyntaxNodeViewModel Layout(FromConcept concept)
        {
            return (new ConceptNodeViewModel(null, concept))
                .Keyword("FROM")
                .Repeatable().Bind(nameof(concept.Expressions));
        }
    }
    public sealed class WhereConceptLayout : ConceptLayout<WhereConcept>
    {
        public override ISyntaxNodeViewModel Layout(WhereConcept concept)
        {
            return (new ConceptNodeViewModel(null, concept))
                .Keyword("WHERE")
                .Repeatable().Bind(nameof(concept.Expressions));
        }
    }
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