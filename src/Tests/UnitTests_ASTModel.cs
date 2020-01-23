using Microsoft.VisualStudio.TestTools.UnitTesting;
using OneCSharp.AST.Model;

namespace OneCSharp.Tests
{
    [TestClass]
    public class UnitTests_ASTModel
    {
        [TestMethod]
        public void WorkWithCollection()
        {
            ISyntaxElement firstElement = null;
            ISyntaxElement middleElement = null;
            ISyntaxElement lastElement = null;
            SyntaxConcept concept = new SyntaxConcept();
            for (int i = 0; i < 10; i++)
            {
                ISyntaxElement element = new NameSyntaxElement()
                {
                    Name = i.ToString()
                };
                if (i == 0)
                {
                    firstElement = element;
                }
                else if (i == 5)
                {
                    middleElement = element;
                }
                else if (i == 9)
                {
                    lastElement = element;
                }
                concept.AddElement(element);
            }
            concept.RemoveElement(firstElement);
            concept.RemoveElement(middleElement);
            concept.RemoveElement(lastElement);
        }
    }
}
