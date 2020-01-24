using OneCSharp.Core.Model;

namespace OneCSharp.AST.UI
{
    public sealed class LiteralNodeViewModel : SyntaxNodeViewModel
    {
        private string _literal = string.Empty;
        public LiteralNodeViewModel(ISyntaxNodeViewModel owner) : base(owner) { }
        public LiteralNodeViewModel(ISyntaxNodeViewModel owner, Entity model) : base(owner, model) { }
        public string Literal
        {
            get { return _literal; }
            set { _literal = value; OnPropertyChanged(nameof(Literal)); }
        }
    }
}