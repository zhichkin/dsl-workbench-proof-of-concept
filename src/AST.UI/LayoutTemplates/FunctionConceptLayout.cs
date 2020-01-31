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
                .Repeatable().Decorate("{", "}").Bind(nameof(concept.Parameters));
            // .Reference(nameof(concept.ReturnType)).Optional() ... using IScopeProvider
        }
    }
}