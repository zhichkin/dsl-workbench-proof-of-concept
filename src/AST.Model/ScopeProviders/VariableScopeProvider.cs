using System;
using System.Collections.Generic;
using System.Text;

namespace OneCSharp.AST.Model
{
    public sealed class VariableScopeProvider : IScopeProvider
    {
        public IEnumerable<ISyntaxNode> Scope(ISyntaxNode concept, string propertyName)
        {
            FunctionConcept function = (FunctionConcept)concept.Ancestor<FunctionConcept>();
            if (function.Variables.HasValue
                && function.Variables.Value != null
                && function.Variables.Value.Count > 0)
            {
                return function.Variables.Value;
            }
            return null;
        }
        public IEnumerable<ISyntaxNode> Scope(ISyntaxNode concept, string propertyName, ISyntaxNode consumer)
        {
            throw new NotImplementedException();
        }
    }
}