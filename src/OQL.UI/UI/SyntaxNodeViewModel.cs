using System.Collections.ObjectModel;
using System.ComponentModel;

namespace OneCSharp.OQL.UI
{
    public interface ISyntaxNodeViewModel
    {
        SyntaxNodeViewModel Parent { get; set; }
        // TODO: add functions to add and remove children, so as children could ask parent to remove them from parent's collections
    }
    public abstract class SyntaxNodeViewModel : ISyntaxNodeViewModel, INotifyPropertyChanged
    {
        protected SyntaxNodeViewModel _parent;
        public SyntaxNodeViewModel() { }
        public SyntaxNodeViewModel(SyntaxNodeViewModel parent) { _parent = parent; }
        public SyntaxNodeViewModel Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
    public class SyntaxNodesViewModel : ObservableCollection<SyntaxNodeViewModel>, ISyntaxNodeViewModel
    {
        protected SyntaxNodeViewModel _parent;
        public SyntaxNodesViewModel() { }
        public SyntaxNodesViewModel(SyntaxNodeViewModel parent) { _parent = parent; }
        public SyntaxNodeViewModel Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }
    }
}
