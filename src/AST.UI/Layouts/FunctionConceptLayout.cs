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
                .Property(nameof(concept.ReturnType))
                    .Keyword("RETURNS")
                    .Selector()
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
                .Selector().Bind(nameof(concept.ParameterType))
                .Property(nameof(concept.IsOutput))
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
                .Selector().Bind(nameof(concept.VariableType));
        }
    }
}