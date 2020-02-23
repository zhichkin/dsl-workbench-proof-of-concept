using OneCSharp.AST.Model;
using OneCSharp.MVVM;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Input;

namespace OneCSharp.AST.UI
{
    public interface ISyntaxNodeViewModel
    {
        bool IsVisible { get; set; }
        bool IsTemporallyVisible { get; set; }
        bool ResetHideOptionFlag { get; set; }
        void StartHideOptionAnimation();
        void StopHideOptionAnimation();
        void ResetHideOptionAnimation();
        Type SyntaxNodeType { get; set; }
        ISyntaxNode SyntaxNode { get; set; }
        string PropertyBinding { get; set; }
        void Add(ISyntaxNodeViewModel child);
        void Remove(ISyntaxNodeViewModel child);
        ISyntaxNodeViewModel Owner { get; set; }
        ObservableCollection<ICodeLineViewModel> Lines { get; }
        bool IsFocused { get; set; }
        bool IsMouseOver { get; set; }
        ICommand KeyDownCommand { get; set; }
        ICommand MouseDownCommand { get; set; }
        ICommand MouseEnterCommand { get; set; }
        ICommand MouseLeaveCommand { get; set; }
        ICommand CtrlCCommand { get; set; }
        ICommand CtrlVCommand { get; set; }
    }
    public abstract class SyntaxNodeViewModel : ISyntaxNodeViewModel, INotifyPropertyChanged
    {
        private bool _isFocused = false;
        private bool _isMouseOver = false;
        private Type _syntaxNodeType = null;
        private string _propertyBinding = null;
        public event PropertyChangedEventHandler PropertyChanged;
        public SyntaxNodeViewModel()
        {
            KeyDownCommand = new RelayCommand(OnKeyDown);
            MouseDownCommand = new RelayCommand(OnMouseDown);
            MouseEnterCommand = new RelayCommand(OnMouseEnter);
            MouseLeaveCommand = new RelayCommand(OnMouseLeave);
            CtrlCCommand = new RelayCommand(OnCtrlC);
            CtrlVCommand = new RelayCommand(OnCtrlV);
        }
        public SyntaxNodeViewModel(ISyntaxNodeViewModel owner) : this() { Owner = owner; }
        public SyntaxNodeViewModel(ISyntaxNodeViewModel owner, ISyntaxNode model) : this(owner) { SyntaxNode = model; }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ISyntaxNode SyntaxNode { get; set; }
        public Type SyntaxNodeType
        {
            set { _syntaxNodeType = value; }
            get
            {
                if (_syntaxNodeType == null)
                {
                    return SyntaxNode?.GetType();
                }
                return _syntaxNodeType;
            }
        }
        public string PropertyBinding
        {
            get { return _propertyBinding; }
            set { _propertyBinding = value; SetVisibility(); }
        }
        public ISyntaxNodeViewModel Owner { get; set; }
        public virtual void Add(ISyntaxNodeViewModel child) { }
        public virtual void Remove(ISyntaxNodeViewModel child) { }
        public ObservableCollection<ICodeLineViewModel> Lines { get; } = new ObservableCollection<ICodeLineViewModel>();


        public bool IsFocused
        {
            get { return _isFocused; }
            set { _isFocused = value; OnPropertyChanged(nameof(IsFocused)); }
        }
        public bool IsMouseOver
        {
            get { return _isMouseOver; }
            set { _isMouseOver = value; OnPropertyChanged(nameof(IsMouseOver)); }
        }
        public ICommand KeyDownCommand { get; set; }
        public ICommand MouseDownCommand { get; set; }
        public ICommand MouseEnterCommand { get; set; }
        public ICommand MouseLeaveCommand { get; set; }
        public ICommand CtrlCCommand { get; set; }
        public ICommand CtrlVCommand { get; set; }
        protected virtual void OnMouseEnter(object parameter)
        {
            IsMouseOver = true;
            ConceptNodeViewModel concept = this.Ancestor<ConceptNodeViewModel>() as ConceptNodeViewModel;
            if (concept != null)
            {
                // show options only if first line is entered ...
                if (concept.Lines.Count > 0)
                {
                    if (concept.Lines[0].Nodes.Count > 0)
                    {
                        for (int i = 0; i < concept.Lines[0].Nodes.Count; i++)
                        {
                            var node = concept.Lines[0].Nodes[i];
                            if (node is IndentNodeViewModel) continue;
                            if (node == this)
                            {
                                concept.ShowOptions();
                                if (concept.Owner is RepeatableViewModel)
                                {
                                    concept.ShowCommands();
                                }
                                else if (concept.Owner is ConceptNodeViewModel)
                                {
                                    PropertyInfo property = concept.Owner.SyntaxNode.GetPropertyInfo(concept.PropertyBinding);
                                    if (property.IsOptional())
                                    {
                                        concept.ShowCommands();
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }
        protected virtual void OnMouseLeave(object parameter)
        {
            IsMouseOver = false;
            ConceptNodeViewModel concept = this.Ancestor<ConceptNodeViewModel>() as ConceptNodeViewModel;
            if (concept != null)
            {
                if (concept.Owner is RepeatableViewModel)
                {
                    concept.HideCommands();
                }
            }
        }
        protected virtual void OnMouseDown(object parameter)
        {
            if (!(parameter is MouseButtonEventArgs args)) return;
            if (args.ChangedButton == MouseButton.Right) return;

            ConceptNodeViewModel concept = this.Ancestor<ConceptNodeViewModel>() as ConceptNodeViewModel;
            if (concept != null)
            {
                concept.ProcessOptionSelection(PropertyBinding);
            }
        }
        protected virtual void OnKeyDown(object parameter)
        {
            //if (!(parameter is KeyEventArgs args)) return;
            
            //if (args.Key == Key.Enter)
            //{
            //    BreakLine(this);
            //    args.Handled = true;
            //}
            //else if (args.Key == Key.Back)
            //{
            //    RestoreLine(this);
            //    args.Handled = true;
            //}
            //else if (args.Key == Key.Left)
            //{
            //    FocusLeft(this);
            //    args.Handled = true;
            //}
            //else if (args.Key == Key.Right)
            //{
            //    FocusRight(this);
            //    args.Handled = true;
            //}
            //else if (args.Key == Key.Tab)
            //{
            //    args.Handled = true;
            //}
            //MessageBox.Show($"{Keyword}: {args.Key}");
        }
        protected virtual void OnCtrlC(object parameter)
        {

        }
        protected virtual void OnCtrlV(object parameter)
        {

        }


        private void SetVisibility()
        {
            if (string.IsNullOrWhiteSpace(PropertyBinding)) return;

            Type metadata = Owner.SyntaxNode.GetType();
            PropertyInfo property = metadata.GetProperty(PropertyBinding);
            if (property == null) return;

            if (property.IsOptional())
            {
                IOptional optional = (IOptional)property.GetValue(Owner.SyntaxNode);
                IsVisible = optional.HasValue;
                IsTemporallyVisible = false;
                ResetHideOptionFlag = false;
            }
            else
            {
                IsVisible = true;
                IsTemporallyVisible = false;
                ResetHideOptionFlag = false;
            }
        }
        private bool _isVisible = true;
        private bool _isTemporallyVisible = false;
        private bool _resetHideOptionFlag = false;
        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                _isVisible = value;
                if (this is PropertyViewModel property)
                {
                    foreach(var node in property.Nodes)
                    {
                        node.IsVisible = _isVisible;
                    }
                }
                OnPropertyChanged(nameof(IsVisible));
            }
        }
        public bool IsTemporallyVisible
        {
            get { return _isTemporallyVisible; }
            set
            {
                _isTemporallyVisible = value;
                if (this is PropertyViewModel property)
                {
                    foreach (var node in property.Nodes)
                    {
                        node.IsTemporallyVisible = _isTemporallyVisible;
                    }
                }
                OnPropertyChanged(nameof(IsTemporallyVisible));
            }
        }
        public bool ResetHideOptionFlag
        {
            get { return _resetHideOptionFlag; }
            set
            {
                _resetHideOptionFlag = value;
                if (this is PropertyViewModel property)
                {
                    foreach (var node in property.Nodes)
                    {
                        node.ResetHideOptionFlag = _resetHideOptionFlag;
                    }
                }
                OnPropertyChanged(nameof(ResetHideOptionFlag));
            }
        }
        public void StartHideOptionAnimation()
        {
            if (!IsTemporallyVisible)
            {
                IsVisible = true;
                IsTemporallyVisible = true;
            }
        }
        public void ResetHideOptionAnimation()
        {
            if (IsTemporallyVisible)
            {
                ResetHideOptionFlag = true;
                ResetHideOptionFlag = false;
            }
        }
        public void StopHideOptionAnimation()
        {
            if (IsTemporallyVisible)
            {
                IsVisible = false;
                IsTemporallyVisible = false;
                ResetHideOptionFlag = false;
                if (this is RepeatableOptionViewModel)
                {
                    if (Owner == null) return;
                    Owner.Remove(this);
                    if (Owner.Lines.Count == 0)
                    {
                        Owner.StopHideOptionAnimation();
                    }
                }
                else if (Owner is PropertyViewModel property)
                {
                    property.StopHideOptionAnimation();
                }
            }
        }
    }
}