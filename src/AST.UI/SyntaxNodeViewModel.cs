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
        ISyntaxNode Model { get; set; }
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
        public SyntaxNodeViewModel(ISyntaxNodeViewModel owner, ISyntaxNode model) : this(owner) { Model = model; }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ISyntaxNode Model { get; set; }
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
                concept.IsMouseOver = true;
                concept.ShowOptions();
            }
        }
        protected virtual void OnMouseLeave(object parameter)
        {
            IsMouseOver = false;
            ConceptNodeViewModel concept = this.Ancestor<ConceptNodeViewModel>() as ConceptNodeViewModel;
            if (concept != null)
            {
                concept.IsMouseOver = false;
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

            Type metadata = Owner.Model.GetType();
            PropertyInfo property = metadata.GetProperty(PropertyBinding);
            if (property == null) return;

            if (property.IsOptional())
            {
                IOptional optional = (IOptional)property.GetValue(Owner.Model);
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
            set { _isVisible = value; OnPropertyChanged(nameof(IsVisible)); }
        }
        public bool IsTemporallyVisible
        {
            get { return _isTemporallyVisible; }
            set { _isTemporallyVisible = value; OnPropertyChanged(nameof(IsTemporallyVisible)); }
        }
        public bool ResetHideOptionFlag
        {
            get { return _resetHideOptionFlag; }
            set { _resetHideOptionFlag = value; OnPropertyChanged(nameof(ResetHideOptionFlag)); }
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
            }
        }



        private void ShowBorder()
        {
            //if (string.IsNullOrEmpty(PropertyBinding)) return;
            //ConceptNodeViewModel concept = this.Ancestor<ConceptNodeViewModel>() as ConceptNodeViewModel;
            //if (concept == null) return;
            //var nodes = concept.GetNodesByPropertyName(PropertyBinding);
            //if (nodes.Count == 0) return;

            //NodePosition position = concept.GetPosition(nodes[0]);
            //BorderViewModel border = new BorderViewModel();
            //foreach (var node in nodes)
            //{
            //    border.Nodes.Add(node);
            //}
            //concept.Lines[position.Line].Nodes.Insert(position.Position, border);
        }
    }
}