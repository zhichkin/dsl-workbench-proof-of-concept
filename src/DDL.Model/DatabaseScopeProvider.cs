using OneCSharp.AST.Model;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace OneCSharp.DDL.Model
{
    public sealed class DatabaseScopeProvider : IScopeProvider
    {
        private readonly Dictionary<string, Assembly> _databases = new Dictionary<string, Assembly>();
        public void RegisterDatabase(Assembly assembly)
        {
            _databases.Add(assembly.FullName, assembly);
        }
        public IEnumerable<ISyntaxNode> Scope(ISyntaxNode concept, string propertyName)
        {
            List<ISyntaxNode> scope = new List<ISyntaxNode>();
            if (!(concept is UseDatabaseConcept)) return scope;

            foreach (var database in _databases)
            {
                scope.Add(new UseDatabaseConcept() { Assembly = database.Value });
            }
            return scope;
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