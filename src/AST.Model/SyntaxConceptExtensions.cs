using System;
using System.Collections.Generic;

namespace OneCSharp.AST.Model
{
    public static class SyntaxConceptExtensions
    {
        public static ISyntaxConcept Name(this ISyntaxConcept concept)
        {
            concept.AddElement(new NameSyntaxElement());
            return concept;
        }
        public static ISyntaxConcept Literal(this ISyntaxConcept concept, string literal)
        {
            if (string.IsNullOrWhiteSpace(literal)) throw new ArgumentNullException(nameof(literal));
            concept.AddElement(new LiteralSyntaxElement() { Literal = literal });
            return concept;
        }
        public static ISyntaxConcept Keyword(this ISyntaxConcept concept, string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) throw new ArgumentNullException(nameof(keyword));
            concept.AddElement(new KeywordSyntaxElement() { Keyword = keyword });
            return concept;
        }
        public static ISyntaxConcept Repeatable(this ISyntaxConcept concept, List<ISyntaxElement> elements)
        {
            RepeatableSyntaxElement repeatable = new RepeatableSyntaxElement();
            foreach (ISyntaxElement element in elements)
            {
                repeatable.Selector.AddElement(element);
            }
            concept.AddElement(repeatable);
            return concept;
        }
        public static ISyntaxConcept Optional(this ISyntaxConcept concept)
        {
            int count = concept.Elements.Count;
            if (count > 0)
            {
                concept.Elements[count - 1].IsOptional = true;
            }
            return concept;
        }
    }
}