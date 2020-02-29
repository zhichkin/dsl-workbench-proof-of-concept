using OneCSharp.AST.Model;
using System.Collections.Generic;

namespace OneCSharp.DDL.Model
{
    public sealed class DomainConcept : SyntaxRoot, IIdentifiable
    {
        private const string PLACEHOLDER = "<domain>";
        public DomainConcept() { Identifier = PLACEHOLDER; }
        public string Identifier { get; set; }
        public List<EntityConcept> Entities { get; } = new List<EntityConcept>();
    }
}