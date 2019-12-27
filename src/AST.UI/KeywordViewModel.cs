using OneCSharp.MVVM;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace OneCSharp.AST.UI
{
    public sealed class KeywordViewModel : SyntaxElementViewModel, IConceptViewModel
    {
        private bool _isFocused = false;
        private bool _isBorderVisible = false;
        private Brush _borderBrush = Brushes.White;
        private string _keyword = string.Empty;
        private readonly ConceptViewModel _owner;
        public KeywordViewModel(ConceptViewModel owner)
        {
            _owner = owner;
            KeyUpCommand = new RelayCommand(OnKeyUp);
            CtrlCCommand = new RelayCommand(OnCtrlC);
            CtrlVCommand = new RelayCommand(OnCtrlV);
        }
        public ICommand KeyUpCommand { get; private set; }
        public ICommand CtrlCCommand { get; private set; }
        public ICommand CtrlVCommand { get; private set; }
        public string Keyword
        {
            get { return _keyword; }
            set { _keyword = value; OnPropertyChanged(nameof(Keyword)); }
        }


        public Brush BorderBrush
        {
            get { return _borderBrush; }
            set { _borderBrush = value; OnPropertyChanged(nameof(BorderBrush)); }
        }



        public bool IsBorderVisible
        {
            get { return _isBorderVisible; }
            set { _isBorderVisible = value; OnPropertyChanged(nameof(IsBorderVisible)); }
        }
        public bool IsFocused
        {
            get { return _isFocused; }
            set { _isFocused = value; OnPropertyChanged(nameof(IsFocused)); }
        }

        protected override void OnMouseEnter(object parameter)
        {
            if (IsFocused) return;
            IsBorderVisible = true;
            BorderBrush = Brushes.Black;
        }
        protected override void OnMouseLeave(object parameter)
        {
            if (!IsFocused)
            {
                IsBorderVisible = false;
                BorderBrush = Brushes.White;
            }
        }
        protected override void OnClick(object parameter)
        {
            base.OnClick(parameter);
            IsFocused = true;
            IsBorderVisible = true;
            BorderBrush = Brushes.Black;
            FocusManager.SetFocus(this);
        }
        private void OnKeyUp(object parameter)
        {
            if (!(parameter is KeyEventArgs args)) return;
                args.Handled = true;

            if (args.Key == Key.Enter)
            {
                _owner.BreakLine(this);
            }
            else if (args.Key == Key.Back)
            {
                _owner.RestoreLine(this);
            }
            else if (args.Key == Key.Left)
            {
                _owner.FocusLeft(this);
            }
            else if (args.Key == Key.Right)
            {
                _owner.FocusRight(this);
            }
            else if (args.Key == Key.Tab)
            {
                _owner.IndentLine(this);
            }

            //MessageBox.Show($"{Keyword}: {args.Key}");
        }
        private void OnCtrlC(object parameter)
        {

        }
        private void OnCtrlV(object parameter)
        {

        }
    }
}