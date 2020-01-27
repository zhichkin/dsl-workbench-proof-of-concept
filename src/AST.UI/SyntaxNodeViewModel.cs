using OneCSharp.MVVM;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace OneCSharp.AST.UI
{
    public interface ISyntaxNodeViewModel
    {
        object Model { get; set; }
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
        public SyntaxNodeViewModel(ISyntaxNodeViewModel owner, object model) : this(owner) { Model = model; }
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public object Model { get; set; }
        public ISyntaxNodeViewModel Owner { get; set; }
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
            if (IsFocused) return;
            //IsBorderVisible = true;
            //BorderBrush = Brushes.Black;
        }
        protected virtual void OnMouseLeave(object parameter)
        {
            IsMouseOver = false;
            if (!IsFocused)
            {
                //IsBorderVisible = false;
                //BorderBrush = Brushes.White;
            }
        }
        protected virtual void OnMouseDown(object parameter)
        {
            IsFocused = true;
            //IsBorderVisible = true;
            //BorderBrush = Brushes.Black;
            //FocusManager.SetFocus(this);
        }
        protected virtual void OnKeyDown(object parameter)
        {
            if (!(parameter is KeyEventArgs args)) return;
            
            if (args.Key == Key.Enter)
            {
                BreakLine(this);
                args.Handled = true;
            }
            else if (args.Key == Key.Back)
            {
                RestoreLine(this);
                args.Handled = true;
            }
            else if (args.Key == Key.Left)
            {
                FocusLeft(this);
                args.Handled = true;
            }
            else if (args.Key == Key.Right)
            {
                FocusRight(this);
                args.Handled = true;
            }
            else if (args.Key == Key.Tab)
            {
                IndentLine(this);
                args.Handled = true;
            }
            //MessageBox.Show($"{Keyword}: {args.Key}");
        }
        protected virtual void OnCtrlC(object parameter)
        {

        }
        protected virtual void OnCtrlV(object parameter)
        {

        }


        public void BreakLine(ISyntaxNodeViewModel node)
        {
            for (int current = 0; current < Lines.Count; current++)
            {
                ICodeLineViewModel line = Lines[current];
                int position = line.Nodes.IndexOf(node);
                if (position == -1 || position == 0) continue; // position == 0 means no empty line allowed
                if (line.Nodes.Count == 1) return; // no empty line allowed

                ICodeLineViewModel newLine = new CodeLineViewModel(this);
                while (position != line.Nodes.Count)
                {
                    newLine.Nodes.Add(line.Nodes[position]);
                    line.Nodes.RemoveAt(position);
                }
                Lines.Insert(++current, newLine);
            }
            //if (node is KeywordViewModel)
            //{
            //    FocusManager.SetFocus((KeywordViewModel)node);
            //}
        }
        public void RestoreLine(ISyntaxNodeViewModel node)
        {
            if (Lines.Count == 0 || Lines.Count == 1) return;

            for (int current = 1; current < Lines.Count; current++)
            {
                ICodeLineViewModel line = Lines[current];
                int position = line.Nodes.IndexOf(node);
                if (position != 0) continue; // only first item can restore line

                ICodeLineViewModel restoringLine = Lines[--current];
                while (position != line.Nodes.Count)
                {
                    restoringLine.Nodes.Add(line.Nodes[position]);
                    line.Nodes.RemoveAt(position);
                }
                Lines.RemoveAt(++current);
            }
            //if (item is KeywordViewModel)
            //{
            //    FocusManager.SetFocus((KeywordViewModel)item);
            //}
        }
        public void FocusLeft(ISyntaxNodeViewModel node)
        {
            for (int current = 0; current < Lines.Count; current++)
            {
                ICodeLineViewModel line = Lines[current];
                int position = line.Nodes.IndexOf(node);
                if (position == -1) continue;
                if (position == 0) return;

                //if (line.Nodes[position - 1] is KeywordViewModel)
                //{
                //    FocusManager.SetFocus((KeywordViewModel)line.Nodes[position - 1]);
                //}
            }
        }
        public void FocusRight(ISyntaxNodeViewModel node)
        {
            for (int current = 0; current < Lines.Count; current++)
            {
                ICodeLineViewModel line = Lines[current];
                int position = line.Nodes.IndexOf(node);
                if (position == -1) continue;
                if (position == line.Nodes.Count - 1) return;

                //if (line.Nodes[position + 1] is KeywordViewModel)
                //{
                //    FocusManager.SetFocus((KeywordViewModel)line.Nodes[position + 1]);
                //}
            }
        }
        public void IndentLine(ISyntaxNodeViewModel node)
        {
            // TODO: don't forget about remove indent command
        }
    }
}