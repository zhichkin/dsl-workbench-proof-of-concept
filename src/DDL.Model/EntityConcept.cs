using OneCSharp.AST.Model;
using System.Collections.Generic;

namespace OneCSharp.DDL.Model
{
    public sealed class EntityConcept : ComplexDataType
    {
        private const string PLACEHOLDER = "<entity>";
        public EntityConcept() { Identifier = PLACEHOLDER; }
        public List<PropertyConcept> Properties { get; } = new List<PropertyConcept>();
    }
}