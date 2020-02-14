namespace OneCSharp.AST.UI
{
    public sealed class SyntaxTreeOptionViewModel : SyntaxNodeViewModel
    {
        public SyntaxTreeOptionViewModel(ISyntaxNodeViewModel owner) : base(owner) { }
        private string _title = string.Empty;
        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged(nameof(Title)); }
        }
    }
}