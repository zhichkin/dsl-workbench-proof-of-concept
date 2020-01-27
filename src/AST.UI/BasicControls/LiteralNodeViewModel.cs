namespace OneCSharp.AST.UI
{
    public sealed class LiteralNodeViewModel : SyntaxNodeViewModel
    {
        private string _literal = string.Empty;
        public LiteralNodeViewModel(ISyntaxNodeViewModel owner) : base(owner) { }
        public string Literal
        {
            get { return _literal; }
            set { _literal = value; OnPropertyChanged(nameof(Literal)); }
        }
    }
}