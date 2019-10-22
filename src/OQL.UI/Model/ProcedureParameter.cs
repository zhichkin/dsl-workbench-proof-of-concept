using System;

namespace OneCSharp.OQL.Model
{
    public sealed class ProcedureParameter : SyntaxNode
    {
        public ProcedureParameter() { }
        public ProcedureParameter(ISyntaxNode parent) : base(parent) { }
        public string Name { get; set; }
        public Type Type { get; set; }
        public object Value { get; set; }
        public bool IsInput { get; set; } = true;
        public bool IsOutput { get; set; } = false;
    }
}