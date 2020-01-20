using OneCSharp.Core.Model;
using System;
using System.Collections.Generic;

namespace OneCSharp.AST.Model
{
    public abstract class ConceptElement : Property, ICloneable
    {
        public bool IsOptional { get; set; }
        public object Value { get; set; } // used to keep values of concrete syntax tree : parameterized keywords and repeatable concept lists
        public int LineNumber { get; set; }
        public int LineOrdinal { get; set; }
        public abstract object Clone();

        //public static KeywordElement CreateKeyword(LanguageConcept owner, string name)
        //{
        //    return new KeywordElement()
        //    {
        //        Owner = owner,
        //        Name = name,
        //        ValueType = SimpleType.NULL,
        //        IsOptional = false
        //    };
        //}
    }
    public abstract class LanguageConcept : ComplexType
    {
        public void PrepareForEditing() // prepare object for editing as parse syntax tree
        {
            int index = 0;
            while (index < Properties.Count)
            {
                ConceptElement element = (ConceptElement)Properties[index];
                if (element.IsOptional)
                {
                    Properties.RemoveAt(index);
                }
                else
                {
                    index++;
                }
            }
        }
        public LanguageConcept Literal(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            int ordinal = Properties.Count;
            Properties.Add(new LiteralElement()
            {
                Owner = this,
                Name = name,
                Ordinal = ordinal,
                IsOptional = false,
                ValueType = SimpleType.NULL
            });
            return this;
        }
        public LanguageConcept Keyword(string name)
        {
            return Keyword(name, false);
        }
        public LanguageConcept Keyword(string name, bool optional)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            int ordinal = Properties.Count;
            Properties.Add(new KeywordElement()
            {
                Owner = this,
                Name = name,
                Ordinal = ordinal,
                IsOptional = optional,
                ValueType = SimpleType.NULL
            });
            return this;
        }
        public LanguageConcept WithParameter(DataType parameterType)
        {
            int count = Properties.Count;
            if (count == 0) throw new InvalidOperationException("Concept properties list is empty.");
            if (!(Properties[count - 1] is KeywordElement keyword))
            {
                throw new InvalidOperationException("No preceding keyword is found.");
            }
            if (parameterType == null)
            {
                keyword.ValueType = SimpleType.NULL;
            }
            else
            {
                keyword.ValueType = parameterType;
            }
            return this;
        }
        public LanguageConcept UserName()
        {
            return UserName(null);
        }
        public LanguageConcept UserName(string placeholder)
        {
            int ordinal = Properties.Count;
            Properties.Add(new NameElement()
            {
                Owner = this,
                Ordinal = ordinal,
                Name = string.IsNullOrWhiteSpace(placeholder) ? "<name>" : placeholder,
                ValueType = SimpleType.NULL
            });
            return this;
        }
        public LanguageConcept Repeat(LanguageConcept concept)
        {
            int ordinal = Properties.Count;
            RepeatableElement repeatable = new RepeatableElement()
            {
                Owner = this,
                Name = string.Empty,
                Ordinal = ordinal,
                IsOptional = true // TODO !
            };
            ((MultipleType)((ListType)repeatable.ValueType).Type).Types.Add(concept);
            Properties.Add(repeatable);
            return this;
        }
        public LanguageConcept Selector(DataType dataType)
        {
            //int ordinal = Properties.Count;
            //Properties.Add(new SelectorElement()
            //{
            //    Owner = this,
            //    Name = string.Empty,
            //    Ordinal = ordinal,
            //    ValueType = dataType
            //});
            return this;
        }
    }
    public sealed class FunctionConcept : LanguageConcept
    {
        private const string FUNCTION = "FUNCTION";
        private const string RETURNS = "RETURNS";
        public FunctionConcept()
        {
            Name = FUNCTION;
            Keyword(FUNCTION)
                .UserName()
                .Keyword(RETURNS, true).WithParameter(new MultipleType())
                .Repeat(new ParameterConcept());
        }
    }
    public sealed class ParameterConcept : LanguageConcept
    {
        private const string PARAMETER = "PARAMETER";
        private const string KEYWORD = "@";
        private const string EQUALS_SIGN = "=";
        private const string INPUT = "INPUT";
        private const string OUTPUT = "OUTPUT";
        private const string PLACEHOLDER = "<parameter name>";
        public ParameterConcept()
        {
            Name = PARAMETER;
            Keyword(KEYWORD)
                .UserName(PLACEHOLDER)
                .Literal(EQUALS_SIGN)
                .Keyword(INPUT, true)
                .Keyword(OUTPUT, true);
        }
    }
    public sealed class NameElement : ConceptElement
    {
        private const string PLACEHOLDER = "<name>";
        public override object Clone()
        {
            return new NameElement()
            {
                Owner = null,
                Name = PLACEHOLDER,
                Ordinal = this.Ordinal,
                IsOptional = false,
                ValueType = SimpleType.NULL
            };
        }
    }
    public sealed class LiteralElement : ConceptElement
    {
        public override object Clone()
        {
            return new LiteralElement()
            {
                Owner = null,
                Name = this.Name,
                Ordinal = this.Ordinal,
                ValueType = this.ValueType,
                IsOptional = this.IsOptional
            };
        }
    }
    public sealed class KeywordElement : ConceptElement
    {
        public override object Clone()
        {
            return new KeywordElement()
            {
                Owner = null,
                Name = this.Name,
                Value = this.Value,
                Ordinal = this.Ordinal,
                ValueType = this.ValueType,
                IsOptional = this.IsOptional
            };
        }
    }
    public sealed class RepeatableElement : ConceptElement
    {
        public RepeatableElement()
        {
            ValueType = new ListType()
            {
                Type = new MultipleType()
            };
        }
        public override object Clone()
        {
            return new RepeatableElement()
            {
                Owner = null,
                Name = this.Name,
                Ordinal = this.Ordinal,
                ValueType = new ListType()
                {
                    Type = ((ListType)this.ValueType).Type
                },
                IsOptional = this.IsOptional
            };
        }
    }
    
    //public sealed class SelectorElement : ConceptElement { } // !?
}