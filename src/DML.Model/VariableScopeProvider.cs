using OneCSharp.AST.Model;
using System;
using System.Collections.Generic;

namespace OneCSharp.DML.Model
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