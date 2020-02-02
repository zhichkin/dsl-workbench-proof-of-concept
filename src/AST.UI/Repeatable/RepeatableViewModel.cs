using System;

namespace OneCSharp.AST.UI
{
    public sealed class RepeatableViewModel : SyntaxNodeViewModel
    {
        private string _openingLiteral;
        private string _closingLiteral;
        public RepeatableViewModel(ISyntaxNodeViewModel owner) : base(owner) { }
        public string Delimiter { get; set; } = string.Empty;
        public string OpeningLiteral
        {
            get { return _openingLiteral; }
            set { _openingLiteral = value; OpeningLiteralChanged(); }
        }
        public string ClosingLiteral
        {
            get { return _closingLiteral; }
            set { _closingLiteral = value; ClosingLiteralChanged(); }
        }
        private void OpeningLiteralChanged()
        {
            if (string.IsNullOrEmpty(OpeningLiteral))
            {
                // TODO
            }
            else
            {
                ICodeLineViewModel line = new CodeLineViewModel(this);
                line.Nodes.Add(new IndentNodeViewModel(this));
                line.Nodes.Add(new LiteralNodeViewModel(this) { Literal = OpeningLiteral });
                if (Lines.Count == 0)
                {
                    Lines.Add(line);
                }
                else
                {
                    Lines.Insert(0, line);
                }
            }
        }
        private void ClosingLiteralChanged()
        {
            if (string.IsNullOrEmpty(ClosingLiteral))
            {
                // TODO
            }
            else
            {
                ICodeLineViewModel line = new CodeLineViewModel(this);
                line.Nodes.Add(new IndentNodeViewModel(this));
                line.Nodes.Add(new LiteralNodeViewModel(this) { Literal = ClosingLiteral });
                Lines.Add(line);
            }
        }
        public override void Add(ISyntaxNodeViewModel node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));

            ICodeLineViewModel line = new CodeLineViewModel(this);
            if (string.IsNullOrEmpty(OpeningLiteral))
            {
                line.Nodes.Add(new IndentNodeViewModel(this));
            }
            else
            {
                line.Nodes.Add(new IndentNodeViewModel(this));
                line.Nodes.Add(new IndentNodeViewModel(this));
            }
            if (!string.IsNullOrEmpty(Delimiter))
            {
                int count = 0;
                count += (string.IsNullOrEmpty(OpeningLiteral) ? 0 : 1);
                count += (string.IsNullOrEmpty(ClosingLiteral) ? 0 : 1);
                count = Lines.Count - count;
                if (count > 0)
                {
                    line.Nodes.Add(new LiteralNodeViewModel(this) { Literal = Delimiter });
                }
            }

            line.Nodes.Add(node);
            if (string.IsNullOrEmpty(ClosingLiteral))
            {
                Lines.Add(line);
            }
            else
            {
                Lines.Insert(Lines.Count - 1, line);
            }
        }
    }
}