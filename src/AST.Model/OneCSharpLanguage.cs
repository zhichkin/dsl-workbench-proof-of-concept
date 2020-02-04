using System.Collections.Generic;

namespace OneCSharp.AST.Model
{
    public sealed class FunctionConcept : SyntaxNode, IIdentifiable
    {
        private const string PLACEHOLDER = "<function name>";
        public string Identifier { get; set; } = PLACEHOLDER;
        public Optional<SimpleTypeConcept> ReturnType { get; } = new Optional<SimpleTypeConcept>();
        public Optional<List<ParameterConcept>> Parameters { get; } = new Optional<List<ParameterConcept>>();
        public Optional<List<SelectConcept>> Statements { get; } = new Optional<List<SelectConcept>>();
    }
    public sealed class ParameterConcept : SyntaxNode, IIdentifiable
    {
        private const string PLACEHOLDER = "<parameter name>";
        public string Identifier { get; set; } = PLACEHOLDER;
        public SimpleTypeConcept ParameterType { get; set; }
        public Optional<bool> IsOutput { get; } = new Optional<bool>();
    }
}