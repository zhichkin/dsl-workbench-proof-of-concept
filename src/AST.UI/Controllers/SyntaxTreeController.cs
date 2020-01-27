using OneCSharp.AST.Model;
using OneCSharp.Core.Model;

namespace OneCSharp.AST.UI
{
    public sealed class SyntaxTreeController
    {
        public ConceptNodeViewModel CreateSyntaxNode(ISyntaxNodeViewModel parentNode, ISyntaxConcept model, ISyntaxConcept grammar)
        {
            ConceptNodeViewModel node = new ConceptNodeViewModel(parentNode, (Entity)model);
            CodeLineViewModel codeLine = new CodeLineViewModel(node);
            node.Lines.Add(codeLine);

            foreach (ISyntaxNode element in model.Nodes)
            {
                if (element.Placement == SyntaxNodePlacement.NewLine)
                {
                    codeLine = new CodeLineViewModel(node);
                    node.Lines.Add(codeLine);
                }
                if (element.UseIndent)
                {
                    codeLine.Nodes.Add(new IndentNodeViewModel(node));
                }
                //CreateSyntaxNode(node, codeLine, (ConceptNode)element);
            }

            return node;
        }
    }
}