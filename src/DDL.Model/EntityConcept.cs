using OneCSharp.AST.Model;
using System;
using System.Collections.Generic;

namespace OneCSharp.DDL.Model
{
    public sealed class EntityConcept : ComplexDataType
    {
        private const string PLACEHOLDER = "<entity>";
        public EntityConcept() { Identifier = PLACEHOLDER; }
        public Type TableDefinition { get; set; }
        public List<PropertyConcept> Properties { get; } = new List<PropertyConcept>();
    }
}