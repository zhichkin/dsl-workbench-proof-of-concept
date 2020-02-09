using System;
using System.Collections.Generic;

namespace OneCSharp.AST.Model
{
    public sealed class TypeConceptScopeProvider : IScopeProvider
    {
        public IEnumerable<ISyntaxNode> Scope(ISyntaxNode concept, string propertyName)
        {
            return TypeConstraints.GetPropertyTypeConstraints(concept, propertyName);
        }
        public IEnumerable<ISyntaxNode> Scope(ISyntaxNode concept, string propertyName, ISyntaxNode consumer)
        {
            throw new NotImplementedException();
        }
    }
}