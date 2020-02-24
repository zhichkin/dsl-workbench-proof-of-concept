using OneCSharp.AST.Model;

namespace OneCSharp.DDL.Model
{
    public sealed class FieldConcept : SyntaxNode, IIdentifiable
    {
        private const string PLACEHOLDER = "<field>";
        public FieldConcept() { Identifier = PLACEHOLDER; }
        public string Identifier { get; set; }
        [SimpleTypeConstraint]
        public SimpleDataType ValueType { get; set; }
    }
}