using OneCSharp.AST.Model;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace OneCSharp.AST.UI
{
    public interface ISyntaxTreeCommand
    {
        // TODO: ???
    }
    public sealed class ShowSyntaxTreeCommand
    {
        public void Execute(ConceptNodeViewModel syntaxNode, string propertyName)
        {
            if (syntaxNode == null) throw new ArgumentNullException(nameof(syntaxNode));
            if (string.IsNullOrWhiteSpace(propertyName)) throw new ArgumentNullException(nameof(propertyName));

            //syntaxNode.ShowSyntaxNodes(propertyName);

            IEnumerable<ISyntaxNode> constraints = TypeConstraints.GetPropertyTypeConstraints(syntaxNode.Model, propertyName);

        }
    }
    public sealed class HideSyntaxTreeCommand
    {
        public void Execute(ConceptNodeViewModel concept, string propertyName)
        {
            if (concept == null) throw new ArgumentNullException(nameof(concept));
            if (string.IsNullOrWhiteSpace(propertyName)) throw new ArgumentNullException(nameof(propertyName));
            concept.HideSyntaxNodes(propertyName);
        }
    }

    
}