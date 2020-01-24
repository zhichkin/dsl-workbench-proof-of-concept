using System;
using System.Collections.Generic;

namespace OneCSharp.AST.Model
{
    public static class SyntaxConceptExtensions
    {
        public static ISyntaxConcept Name(this ISyntaxConcept concept)
        {
            concept.Add(new NameSyntaxNode());
            return concept;
        }
        public static ISyntaxConcept Literal(this ISyntaxConcept concept, string literal)
        {
            if (string.IsNullOrWhiteSpace(literal)) throw new ArgumentNullException(nameof(literal));
            concept.Add(new LiteralSyntaxNode() { Literal = literal });
            return concept;
        }
        public static ISyntaxConcept Keyword(this ISyntaxConcept concept, string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) throw new ArgumentNullException(nameof(keyword));
            concept.Add(new KeywordSyntaxNode() { Keyword = keyword });
            return concept;
        }
        public static ISyntaxConcept Repeatable(this ISyntaxConcept concept, List<ISyntaxNode> elements)
        {
            RepeatableSyntaxNode repeatable = new RepeatableSyntaxNode();
            foreach (ISyntaxNode element in elements)
            {
                repeatable.Selector.AddConstraint(element);
            }
            concept.Add(repeatable);
            return concept;
        }
        public static ISyntaxConcept Optional(this ISyntaxConcept concept)
        {
            int count = concept.Nodes.Count;
            if (count > 0)
            {
                concept.Nodes[count - 1].IsOptional = true;
            }
            return concept;
        }
    }
}