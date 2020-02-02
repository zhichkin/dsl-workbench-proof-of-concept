using System;
using System.Collections.Generic;

namespace OneCSharp.AST.Model
{
    public sealed class FunctionConcept : SyntaxNode, IIdentifiable, IScopeProvider
    {
        private const string PLACEHOLDER = "<function name>";
        public string Identifier { get; set; } = PLACEHOLDER;

        [SimpleTypeConstraint]
        public Optional<object> ReturnType { get; } = new Optional<object>();
        public Optional<List<ParameterConcept>> Parameters { get; } = new Optional<List<ParameterConcept>>();
        public IEnumerable<ISyntaxNode> Scope(Type scopeType)
        {
            if (!Parameters.HasValue) return null;
            if (scopeType != typeof(ParameterConcept)) return null;
            return Parameters.Value;
        }
    }
    public sealed class ParameterConcept : SyntaxNode, IIdentifiable
    {
        private const string PLACEHOLDER = "<parameter name>";
        public string Identifier { get; set; } = PLACEHOLDER;

        [SimpleTypeConstraint]
        public object ParameterType { get; set; }
        public Optional<bool> IsOutput { get; } = new Optional<bool>();
    }
}