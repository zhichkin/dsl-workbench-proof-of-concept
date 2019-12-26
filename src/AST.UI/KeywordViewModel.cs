using OneCSharp.MVVM;
using System.Windows.Input;

namespace OneCSharp.AST.UI
{
    public sealed class KeywordViewModel : SyntaxElementViewModel
    {
        private bool _isFocused = true;
        private bool _isBorderVisible = false;
        private string _keyword = string.Empty;
        public KeywordViewModel()
        {
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
            IsBorderVisible = true;
        }
        protected override void OnMouseLeave(object parameter)
        {
            IsBorderVisible = false;
        }
        protected override void OnClick(object parameter)
        {
            base.OnClick(parameter);
        }
        private void OnKeyUp(object parameter)
        {

        }
        private void OnCtrlC(object parameter)
        {

        }
        private void OnCtrlV(object parameter)
        {

        }
    }
}