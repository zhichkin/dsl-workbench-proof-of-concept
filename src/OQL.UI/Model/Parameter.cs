using System;

namespace OneCSharp.OQL.Model
{
    public sealed class Parameter : SyntaxNode
    {
        public Parameter() { }
        public Parameter(ISyntaxNode parent) : base(parent) { }
        public string Name { get; set; } = string.Empty;
        public Type Type { get; set; } = typeof(int);
        public bool IsInput { get; set; } = true;
        public bool IsOutput { get; set; } = false;
        public object Value { get; set; }
    }
}