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
                .Repeatable().Bind(nameof(concept.Parameters)); // .Decorate("{", "}")

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
                .Keyword("OUTPUT").Bind(nameof(concept.IsOutput));
            // .Reference(nameof(concept.ParameterType)) ... using IScopeProvider
        }
    }
}