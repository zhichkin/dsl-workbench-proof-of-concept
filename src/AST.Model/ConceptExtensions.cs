using OneCSharp.Core.Model;
using System;
using System.Linq;

namespace OneCSharp.AST.Model
{
    public static class ConceptExtensions
    {
        private const string NAME = "NAME"; // user defined name of the concept, example: FUNCTION GetWhateverYouWant
        public static LanguageConcept Name(this LanguageConcept concept)
        {
            Property property = concept.Properties
                .Where(p => p.Name == NAME)
                .FirstOrDefault();

            if (property != null)
            {
                return concept;
            }

            int ordinal = concept.Properties.Count;
            property = new Property()
            {
                Name = NAME,
                Owner = concept,
                Ordinal = ordinal,
                IsOptional = false,
                ValueType = SimpleType.String
            };
            concept.Properties.Add(property);

            return concept;
        }
        public static LanguageConcept List(this LanguageConcept concept, string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));

            Property property = concept.Properties
                .Where(p => p.Name == name)
                .FirstOrDefault();

            if (property != null)
            {
                return concept;
            }

            int ordinal = concept.Properties.Count;
            concept.Properties.Add(new Property()
            {
                Name = name,
                Owner = concept,
                Ordinal = ordinal,
                IsOptional = false,
                ValueType = new ListType()
            });

            return concept;
        }
        public static LanguageConcept Property(this LanguageConcept concept, string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));

            Property property = concept.Properties
                .Where(p => p.Name == name)
                .FirstOrDefault();

            if (property != null)
            {
                return concept;
            }

            int ordinal = concept.Properties.Count;
            concept.Properties.Add(new Property()
            {
                Name = name,
                Owner = concept,
                Ordinal = ordinal,
                IsOptional = false,
                ValueType = null
            });

            return concept;
        }
        public static LanguageConcept Optional(this LanguageConcept concept)
        {
            int count = concept.Properties.Count;
            if (count > 0)
            {
                concept.Properties[count - 1].IsOptional = true;
            }
            return concept;
        }
        public static LanguageConcept ValueType(this LanguageConcept concept, DataType valueType)
        {
            if (valueType == null) throw new ArgumentNullException(nameof(valueType));

            int count = concept.Properties.Count;
            if (count == 0)
            {
                return concept;
            }

            Property property = concept.Properties[count - 1];

            if (property.ValueType == null || property.ValueType == SimpleType.NULL)
            {
                property.ValueType = valueType;
            }
            else if (property.ValueType is SimpleType || property.ValueType is ComplexType)
            {
                MultipleType multipleType = new MultipleType();
                multipleType.Types.Add(property.ValueType);
                multipleType.Types.Add(valueType);
                property.ValueType = multipleType;
            }
            else if (property.ValueType is MultipleType)
            {
                ((MultipleType)property.ValueType).Types.Add(valueType);
            }
            else if (property.ValueType is ListType)
            {
                DataType type = ((ListType)property.ValueType).Type;
                if (type == null || type == SimpleType.NULL)
                {
                    ((ListType)property.ValueType).Type = valueType;
                }
                if (type is SimpleType || type is ComplexType)
                {
                    MultipleType multipleType = new MultipleType();
                    multipleType.Types.Add(type);
                    multipleType.Types.Add(valueType);
                    ((ListType)property.ValueType).Type = multipleType;
                }
                else if (type is MultipleType)
                {
                    ((MultipleType)((ListType)property.ValueType).Type).Types.Add(valueType);
                }
            }
            
            return concept;
        }
    }
}