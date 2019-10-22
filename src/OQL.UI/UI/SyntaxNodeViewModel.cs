using OneCSharp.OQL.Model;
using System.ComponentModel;

namespace OneCSharp.OQL.UI
{
    public interface ISyntaxNodeViewModel<TModel> : INotifyPropertyChanged where TModel : ISyntaxNode
    {
        TModel Model { get; set; }
    }
    public abstract class SyntaxNodeViewModel<TModel> : ISyntaxNodeViewModel<TModel> where TModel : ISyntaxNode
    {
        protected TModel _model = default;
        public SyntaxNodeViewModel() { }
        public SyntaxNodeViewModel(TModel model) { _model = model; }
        public TModel Model { get { return _model; } set { _model = value; } }
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
