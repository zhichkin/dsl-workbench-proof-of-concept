using OneCSharp.AST.Model;

namespace OneCSharp.AST.UI
{
    public sealed class SyntaxTreeController
    {
        public ConceptNodeViewModel CreateSyntaxNode(ISyntaxNodeViewModel parentNode, ISyntaxNode model, LanguageConcept grammar)
        {
            ConceptNodeViewModel node = new ConceptNodeViewModel(parentNode, model);
            CodeLineViewModel codeLine = new CodeLineViewModel(node);
            node.Lines.Add(codeLine);

            foreach (ISyntaxNode element in model.Children)
            {
                //CreateSyntaxNode(node, codeLine, (ConceptNode)element);
            }

            return node;
        }
    }
}