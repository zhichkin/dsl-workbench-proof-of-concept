using OneCSharp.Core.Model;
using System;
using System.Linq;

namespace OneCSharp.AST.Model
{
    public static class LanguageExtentions
    {
        private const string CONCEPT_NAMESPACE = "Concepts";
        public static LanguageConcept Concept(this Language language, string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));

            Namespace concepts = language.Namespaces
                .Where(n => n.Name == CONCEPT_NAMESPACE)
                .FirstOrDefault();
            
            if (concepts == null)
            {
                concepts = new Namespace()
                {
                    Owner = language,
                    Name = CONCEPT_NAMESPACE
                };
                language.Namespaces.Add(concepts);
            }

            LanguageConcept concept = (LanguageConcept)concepts.DataTypes
                .Where(c => c is LanguageConcept && c.Name == name)
                .FirstOrDefault();

            if (concept != null)
            {
                return concept;
            }

            concept = new LanguageConcept()
            {
                Name = name,
                Owner = concepts,
                UUID = new Guid()
            };
            concepts.DataTypes.Add(concept);

            return concept;
        }
    }
}