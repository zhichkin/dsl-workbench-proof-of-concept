using OneCSharp.AST.Model;

namespace OneCSharp.AST.UI
{
    public interface IConceptLayout
    {
        ISyntaxNodeViewModel Layout(ISyntaxNode model);
    }
    public sealed class FunctionConceptLayout : IConceptLayout
    {
        public ISyntaxNodeViewModel Layout(ISyntaxNode model)
        {
            FunctionConcept concept;
            return (new ConceptNodeViewModel(null, model))
                .Keyword("FUNCTION")
                .Identifier()
                //.NewLine()
                //.Indent()
                .Keyword("RETURNS").Bind(nameof(concept.ReturnType))
                .Node().Bind(nameof(concept.ReturnType))
                .Repeatable().Bind(nameof(concept.Parameters))
                .Repeatable().Bind(nameof(concept.Variables))
                .Repeatable().Bind(nameof(concept.Statements));
        }
    }
    public sealed class ParameterConceptLayout : IConceptLayout
    {
        public ISyntaxNodeViewModel Layout(ISyntaxNode model)
        {
            ParameterConcept concept;
            return (new ConceptNodeViewModel(null, model))
                .Keyword("@")
                .Identifier()
                .Node().Bind(nameof(concept.ParameterType))
                .Keyword("OUTPUT").Bind(nameof(concept.IsOutput));
        }
    }
    public sealed class VariableConceptLayout : IConceptLayout
    {
        public ISyntaxNodeViewModel Layout(ISyntaxNode model)
        {
            VariableConcept concept;
            return (new ConceptNodeViewModel(null, model))
                .Keyword("DECLARE")
                .Identifier()
                .Node().Bind(nameof(concept.VariableType));
        }
    }
}