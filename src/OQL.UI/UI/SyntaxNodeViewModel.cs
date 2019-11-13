using System.Collections.ObjectModel;
using System.ComponentModel;

namespace OneCSharp.OQL.UI
{
    public interface ISyntaxNodeViewModel
    {
        ISyntaxNodeViewModel Parent { get; set; }
        void InitializeViewModel();
        // TODO: add functions to add and remove children, so as children could ask parent to remove them from parent's collections
    }
    public abstract class SyntaxNodeViewModel : ISyntaxNodeViewModel, INotifyPropertyChanged
    {
        protected ISyntaxNodeViewModel _parent;
        public SyntaxNodeViewModel() { }
        public SyntaxNodeViewModel(ISyntaxNodeViewModel parent) { _parent = parent; }
        public ISyntaxNodeViewModel Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }
        public abstract void InitializeViewModel();
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
    public class SyntaxNodesViewModel : ObservableCollection<SyntaxNodeViewModel>, ISyntaxNodeViewModel, INotifyPropertyChanged
    {
        protected ISyntaxNodeViewModel _parent;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public SyntaxNodesViewModel() { }
        public SyntaxNodesViewModel(ISyntaxNodeViewModel parent) { _parent = parent; }
        public ISyntaxNodeViewModel Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }
        public virtual void InitializeViewModel() { }
    }
}
