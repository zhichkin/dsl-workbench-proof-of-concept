using OneCSharp.AST.UI;
using OneCSharp.DML.Model;

namespace OneCSharp.DML.UI
{
    public sealed class FunctionConceptLayout : ConceptLayout<FunctionConcept>
    {
        public override ISyntaxNodeViewModel Layout(FunctionConcept concept)
        {
            return (new ConceptNodeViewModel(null, concept))
                .Keyword("FUNCTION")
                .Identifier()
                //.NewLine()
                //.Indent()
                .Property(nameof(concept.ReturnType))
                    .Keyword("RETURNS")
                    .Selector()
                .Repeatable().Bind(nameof(concept.Parameters))
                .Repeatable().Bind(nameof(concept.Variables))
                .Repeatable().Bind(nameof(concept.Statements));
        }
    }
    public sealed class ParameterConceptLayout : ConceptLayout<ParameterConcept>
    {
        public override ISyntaxNodeViewModel Layout(ParameterConcept concept)
        {
            return (new ConceptNodeViewModel(null, concept))
                .Keyword("@")
                .Identifier()
                .Selector().Bind(nameof(concept.ParameterType))
                .Property(nameof(concept.IsOutput))
                    .Keyword("OUTPUT").Bind(nameof(concept.IsOutput));
        }
    }
    public sealed class VariableConceptLayout : ConceptLayout<VariableConcept>
    {
        public override ISyntaxNodeViewModel Layout(VariableConcept concept)
        {
            return (new ConceptNodeViewModel(null, concept))
                .Keyword("DECLARE")
                .Identifier()
                .Selector().Bind(nameof(concept.VariableType));
        }
    }
}