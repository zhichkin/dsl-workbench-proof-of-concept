using System;
using System.Collections.Generic;

namespace OneCSharp.AST.Model
{
    public interface IScopeProvider
    {
        IEnumerable<Type> Scope(ISyntaxNode context, Type scopeType);
        IEnumerable<ISyntaxNode> Scope(ISyntaxNode concept, string propertyName);
        IEnumerable<ISyntaxNode> Scope(ISyntaxNode concept, string propertyName, ISyntaxNode consumer);
    }
}