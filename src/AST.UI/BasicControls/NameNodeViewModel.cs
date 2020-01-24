using OneCSharp.Core.Model;

namespace OneCSharp.AST.UI
{
    public sealed class NameNodeViewModel : SyntaxNodeViewModel
    {
        private bool _isReadOnly = false;
        private string _name = string.Empty;
        public NameNodeViewModel(ISyntaxNodeViewModel owner, Entity model) : base(owner, model) { }
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(nameof(Name)); }
        }
        public bool IsReadOnly
        {
            get { return _isReadOnly; }
            set { _isReadOnly = value; OnPropertyChanged(nameof(IsReadOnly)); }
        }
    }
}