using OneCSharp.AST.Model;
using System;
using System.Collections.Generic;

namespace OneCSharp.DML.Model
{
    public sealed class ParameterScopeProvider : IScopeProvider
    {
        public IEnumerable<ISyntaxNode> Scope(ISyntaxNode concept, string propertyName)
        {
            FunctionConcept function = (FunctionConcept)concept.Ancestor<FunctionConcept>();
            if (function.Parameters.HasValue
                && function.Parameters.Value != null
                && function.Parameters.Value.Count > 0)
            {
                return function.Parameters.Value;
            }
            return null;
        }
        public IEnumerable<ISyntaxNode> Scope(ISyntaxNode concept, string propertyName, ISyntaxNode consumer)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<Type> Scope(ISyntaxNode context, Type scopeType)
        {
            throw new NotImplementedException();
        }
    }
}