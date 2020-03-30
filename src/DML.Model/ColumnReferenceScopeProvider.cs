using OneCSharp.AST.Model;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace OneCSharp.DML.Model
{
    public sealed class ColumnReferenceScopeProvider : IScopeProvider
    {
        public IEnumerable<ISyntaxNode> Scope(ISyntaxNode concept, string propertyName)
        {
            List<ISyntaxNode> scope = new List<ISyntaxNode>();
            if (!(concept.Ancestor<SelectConcept>() is SelectConcept select)) return scope;
            if (select.FROM == null) return scope;
            if (select.FROM.Expressions == null) return scope;
            if (select.FROM.Expressions.Count == 0) return scope;

            foreach (TableConcept table in select.FROM.Expressions)
            {
                if (table.TableDefinition == null) continue;
                foreach (PropertyInfo property in table.TableDefinition.GetProperties())
                {
                    scope.Add(new ColumnConcept()
                    {
                        Parent = concept,
                        Name = property.Name,
                        TableReference = table
                    });
                }
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