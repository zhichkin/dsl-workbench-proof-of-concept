using System.Collections.Generic;

namespace OneCSharp.AST.Model
{
    public interface IScopeProvider
    {
        IEnumerable<ISyntaxNode> Scope(ISyntaxNode concept, string propertyName);
    }
}