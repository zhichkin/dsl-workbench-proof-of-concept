using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace OneCSharp.AST.Model
{
    public sealed class AssemblyScopeProvider : IScopeProvider
    {
        private const string ONE_C_SHARP = "OneCSharp";
        private const string LANGUAGE_MODULE_TOKEN = "Model";
        public IEnumerable<ISyntaxNode> Scope(ISyntaxNode concept, string propertyName)
        {
            List<ISyntaxNode> scope = new List<ISyntaxNode>();
            if (!(concept is LanguageConcept)) return scope;

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                string moduleName = Path.GetFileName(assembly.Location);
                if (moduleName.StartsWith(ONE_C_SHARP)
                    && moduleName.Contains(LANGUAGE_MODULE_TOKEN))
                {
                    scope.Add(new LanguageConcept() { Assembly = assembly });
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