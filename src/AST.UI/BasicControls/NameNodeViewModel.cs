namespace OneCSharp.AST.UI
{
    public sealed class NameNodeViewModel : SyntaxNodeViewModel
    {
        private bool _isReadOnly = false;
        private string _name = string.Empty;
        public NameNodeViewModel(ISyntaxNodeViewModel owner, object model) : base(owner, model) { }
        public string Name
        {
            get { return _name; } // TODO: bind to model's name property
            set { _name = value; OnPropertyChanged(nameof(Name)); }
        }
        public bool IsReadOnly
        {
            get { return _isReadOnly; }
            set { _isReadOnly = value; OnPropertyChanged(nameof(IsReadOnly)); }
        }
    }
}