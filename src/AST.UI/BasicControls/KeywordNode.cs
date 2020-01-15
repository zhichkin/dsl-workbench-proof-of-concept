using OneCSharp.Core.Model;
using OneCSharp.MVVM;
using System.Collections.ObjectModel;

namespace OneCSharp.AST.UI
{
    public sealed class KeywordNode : SyntaxNode
    {   
        private string _keyword = string.Empty;
        private bool _isContextMenuEnabled = true;
        public KeywordNode(ISyntaxNode owner) : base(owner) { }
        public KeywordNode(ISyntaxNode owner, Entity model) : base(owner, model) { }
        public string Keyword
        {
            get { return _keyword; }
            set { _keyword = value; OnPropertyChanged(nameof(Keyword)); }
        }
        public bool IsContextMenuEnabled
        {
            get { return _isContextMenuEnabled; }
            set { _isContextMenuEnabled = value; OnPropertyChanged(nameof(IsContextMenuEnabled)); }
        }
        public ObservableCollection<MenuItemViewModel> ContextMenu { get; } = new ObservableCollection<MenuItemViewModel>();
        protected override void OnMouseDown(object parameter)
        {
            //if (!(parameter is MouseButtonEventArgs args)) return;
            //if (args.ChangedButton == MouseButton.Right)
            //{
            //    args.Handled = true;
            //}
        }
    }
}