using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;

namespace OneCSharp.DSL.UI.Dialogs
{
    public class TreeNodeViewModel : SyntaxNodeViewModel
    {
        public string _caption = string.Empty;
        public TreeNodeViewModel() { } // used to create root node
        public TreeNodeViewModel(TreeNodeViewModel parent, string caption) : base()
        {
            _caption = caption;
            this.Parent = parent;
        }
        public override void InitializeViewModel() { }
        public TreeNodeViewModel(TreeNodeViewModel parent, string caption, object payload) : this(parent, caption)
        {
            this.Payload = payload;
        }
        public object Payload { get; set; }
        //public TreeNodeViewModel Parent { get; set; }
        public string Caption
        {
            get { return _caption; }
            set { _caption = value; OnPropertyChanged(nameof(Caption)); }
        }
        public TreeNodeViewModel SelectedNode { get; set; }
        public ObservableCollection<TreeNodeViewModel> Children { get; set; } = new ObservableCollection<TreeNodeViewModel>();
    }
}
