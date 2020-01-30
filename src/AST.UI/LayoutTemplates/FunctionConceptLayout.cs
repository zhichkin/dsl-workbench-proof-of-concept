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
            //FunctionConcept concept = (FunctionConcept)model;
            return (new ConceptNodeViewModel(null, model))
                .Keyword("FUNCTION")
                .Name(); // looks for IIdentifiable interface
            // .Keyword("RETURNS").Optional(nameof(concept.ReturnType)) ... Map(nameof(concept.ReturnType))
            // .Reference(nameof(concept.ReturnType)).Optional() ... using IScopeProvider
            // .NewLine()
            // .Indent()
            // .Repeatable().Vertical().Optional(nameof(concept.Parameters))
            // TODO: Visibility = Hidden + bind to PropertyName of the model + Visibility depends on IOptional.HasValue
        }
    }
}