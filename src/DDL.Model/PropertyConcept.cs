using OneCSharp.AST.Model;
using System.Collections.Generic;

namespace OneCSharp.DDL.Model
{
    public sealed class PropertyConcept : SyntaxNode, IIdentifiable
    {
        private const string PLACEHOLDER = "<property>";
        public PropertyConcept() { Identifier = PLACEHOLDER; }
        public string Identifier { get; set; }
        [SimpleTypeConstraint]
        [TypeConstraint(typeof(EntityConcept))]
        public DataType ValueType { get; set; }
        public Optional<List<FieldConcept>> Fields { get; } = new Optional<List<FieldConcept>>();
    }
}