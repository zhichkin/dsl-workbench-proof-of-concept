using OneCSharp.AST.Model;
using OneCSharp.Core.Model;

namespace OneCSharp.AST.UI
{
    public sealed class SyntaxTreeController
    {
        public ConceptNode CreateSyntaxNode(ISyntaxNode parentNode, ISyntaxConcept model, ISyntaxConcept grammar)
        {
            ConceptNode node = new ConceptNode(parentNode, (Entity)model);
            SyntaxNodeLine codeLine = new SyntaxNodeLine(node);
            node.Lines.Add(codeLine);

            foreach (ISyntaxElement element in model.Elements)
            {
                if (element.Placement == SyntaxElementPlacement.NewLine)
                {
                    codeLine = new SyntaxNodeLine(node);
                    node.Lines.Add(codeLine);
                }
                if (element.UseIndent)
                {
                    codeLine.Nodes.Add(new IndentNode(node));
                }
                //CreateSyntaxElement(node, codeLine, (ConceptElement)element);
            }

            return node;
        }
    }
}