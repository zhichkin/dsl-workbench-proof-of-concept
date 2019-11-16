namespace OneCSharp.OQL.Model
{
    public sealed class BooleanOperator : SyntaxNode, IKeyword
    {
        public BooleanOperator(ISyntaxNode parent) : base(parent)
        {
            Keyword = BooleanOperators.AND;
            Operands = new SyntaxNodeList(this);
        }
        public string Keyword { get; set; }
        public SyntaxNodeList Operands { get; set; }
        public bool IsRoot
        {
            get
            {
                return !(Parent is BooleanOperator);
            }
        }
        public bool IsLeaf
        {
            get
            {
                return (Operands.Count == 0)
                    || (Operands[0] is ComparisonOperator);
            }
        }
        public void AddChild(ISyntaxNode child)
        {
            child.Parent = this;
            Operands.Add(child);
        }
        public void RemoveChild(ISyntaxNode child)
        {
            Operands.Remove(child);
        }
    }
    public sealed class ComparisonOperator : SyntaxNode, IOperator
    {
        public ComparisonOperator(ISyntaxNode parent) : base(parent)
        {
            Literal = ComparisonOperators.Equal;
        }
        public string Literal { get; set; }
        public bool IsRoot
        {
            get
            {
                return !(Parent is BooleanOperator);
            }
        }
        public ISyntaxNode LeftExpression { get; set; }
        public ISyntaxNode RightExpression { get; set; }
    }

    public sealed class OnSyntaxNode : SyntaxNode, IKeyword
    {
        public OnSyntaxNode(ISyntaxNode parent) : base(parent) { }
        public string Keyword { get { return Keywords.ON; } }
        public ISyntaxNode Expression { get; set; }
    }
    public sealed class WhereSyntaxNode : SyntaxNode, IKeyword
    {
        public WhereSyntaxNode(ISyntaxNode parent) : base(parent) { }
        public string Keyword { get { return Keywords.WHERE; } }
        public ISyntaxNode Expression { get; set; }
    }
    public sealed class HavingSyntaxNode : SyntaxNode, IKeyword
    {
        public HavingSyntaxNode(ISyntaxNode parent) : base(parent) { }
        public string Keyword { get { return Keywords.HAVING; } }
        public ISyntaxNode Expression { get; set; }
    }
}
