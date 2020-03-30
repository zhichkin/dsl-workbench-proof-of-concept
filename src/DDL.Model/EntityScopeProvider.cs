using OneCSharp.AST.Model;
using OneCSharp.DDL.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OneCSharp.DDL.Model
{
    public sealed class EntityScopeProvider : IScopeProvider
    {
        public IEnumerable<ISyntaxNode> Scope(ISyntaxNode concept, string propertyName)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<ISyntaxNode> Scope(ISyntaxNode concept, string propertyName, ISyntaxNode consumer)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<Type> Scope(ISyntaxNode context, Type scopeType)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            List<Type> scope = new List<Type>();

            ScriptConcept script = context.Ancestor<ScriptConcept>() as ScriptConcept;
            if (script == null) return scope;

            foreach (var statement in script.Statements)
            {
                if (!(statement is UseDatabaseConcept database)) continue;
                if (database.Assembly == null) continue;
                foreach (Type type in database.Assembly.GetTypes()
                    .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(ReferenceObject))))
                {
                    scope.Add(type);
                }
            }
            return scope;
        }
    }
}