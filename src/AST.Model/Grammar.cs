using System;
using System.Collections.Generic;

namespace OneCSharp.AST.Model
{
    public sealed class Grammar
    {
        private readonly Dictionary<Type, ISyntaxConcept> _concepts = new Dictionary<Type, ISyntaxConcept>();
        public Grammar()
        {
            _concepts.Add(typeof(FunctionSyntaxConcept), new FunctionSyntaxConcept());
            _concepts.Add(typeof(ParameterSyntaxConcept), new ParameterSyntaxConcept());
        }
        public ISyntaxConcept GetSyntaxConcept(Type conceptType)
        {
            return _concepts[conceptType];
        }
    }
    public sealed class FunctionSyntaxConcept : SyntaxConcept
    {
        private const string FUNCTION = "FUNCTION";
        private const string RETURNS = "RETURNS";
        public FunctionSyntaxConcept()
        {
            this.Keyword(FUNCTION)
                .Name()
                .Keyword(RETURNS).Optional()
                .Repeatable(new List<ISyntaxElement>()
                {
                    new ParameterSyntaxConcept()
                }).Optional();
        }
    }
    public sealed class ParameterSyntaxConcept : SyntaxConcept
    {
        private const string KEYWORD = "@";
        private const string EQUAL_SIGN = "=";
        private const string INPUT = "INPUT";
        private const string OUTPUT = "OUTPUT";
        public ParameterSyntaxConcept()
        {
            this.Keyword(KEYWORD)
                .Name()
                .Literal(EQUAL_SIGN)
                .Keyword(INPUT).Optional()
                .Keyword(OUTPUT).Optional();
        }
    }
}