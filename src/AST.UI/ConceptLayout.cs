using OneCSharp.AST.Model;

namespace OneCSharp.AST.UI
{
    public interface IConceptLayout
    {
        ISyntaxNodeViewModel Layout(ISyntaxNode concept);
    }
    public interface IConceptLayout<TConcept> : IConceptLayout where TConcept : ISyntaxNode
    {
        ISyntaxNodeViewModel Layout(TConcept concept);
    }
    public abstract class ConceptLayout<TConcept> : IConceptLayout<TConcept> where TConcept : ISyntaxNode
    {
        public ISyntaxNodeViewModel Layout(ISyntaxNode concept)
        {
            return Layout((TConcept)concept);
        }
        public abstract ISyntaxNodeViewModel Layout(TConcept concept);
    }
}