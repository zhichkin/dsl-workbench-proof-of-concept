using System;
using System.ComponentModel;
using System.Windows.Input;

namespace OneCSharp.MVVM
{
    public sealed class TabViewModel : INotifyPropertyChanged
    {
        private readonly IShell _shell;
        private string _header;
        private object _content;
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public TabViewModel(IShell shell)
        {
            _shell = shell ?? throw new ArgumentNullException(nameof(shell));
            CloseTabCommand = new RelayCommand(CloseTab);
        }
        public string Header
        {
            get { return _header; }
            set { _header = value; OnPropertyChanged(nameof(Header)); }
        }
        public object Content
        {
            get { return _content; }
            set { _content = value; OnPropertyChanged(nameof(Content)); }
        }
        public ICommand CloseTabCommand { get; private set; }
        private void CloseTab(object parameter)
        {
            _shell.RemoveTabItem(this);
        }
    }
}
