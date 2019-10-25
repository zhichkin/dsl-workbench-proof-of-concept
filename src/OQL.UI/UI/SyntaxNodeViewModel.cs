using System.ComponentModel;

namespace OneCSharp.OQL.UI
{
    public abstract class SyntaxNodeViewModel : INotifyPropertyChanged
    {
        protected SyntaxNodeViewModel _parent = null;
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
}
