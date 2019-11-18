using OneCSharp.OQL.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace OneCSharp.OQL.UI
{
    public interface ISyntaxNodeViewModel
    {
        ISyntaxNode Model { get; }
        ISyntaxNodeViewModel Parent { get; set; }
        // TODO: add functions to add and remove children, so as children could ask parent to remove them from parent's collections
    }
    public abstract class SyntaxNodeViewModel : ISyntaxNodeViewModel, INotifyPropertyChanged
    {
        protected ISyntaxNode _model;
        protected ISyntaxNodeViewModel _parent;
        public SyntaxNodeViewModel() { }
        public SyntaxNodeViewModel(ISyntaxNodeViewModel parent, ISyntaxNode model)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));
            _parent = parent ?? throw new ArgumentNullException(nameof(parent));
        }
        public ISyntaxNode Model { get { return _model; } }
        public ISyntaxNodeViewModel Parent { get { return _parent; } set { _parent = value; } }
        public abstract void InitializeViewModel();
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
    public class SyntaxNodeListViewModel : ObservableCollection<ISyntaxNodeViewModel>, ISyntaxNodeViewModel
    {
        protected ISyntaxNode _model;
        protected ISyntaxNodeViewModel _parent;
        public SyntaxNodeListViewModel() { }
        public SyntaxNodeListViewModel(ISyntaxNodeViewModel parent, ISyntaxNode model)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));
            _parent = parent ?? throw new ArgumentNullException(nameof(parent));
        }
        public ISyntaxNodeViewModel Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }
        public ISyntaxNode Model { get { return _model; } }
        public virtual void InitializeViewModel() { }
        protected void NotifyPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
    }
}
