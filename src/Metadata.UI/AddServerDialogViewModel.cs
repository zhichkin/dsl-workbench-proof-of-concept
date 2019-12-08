using OneCSharp.MVVM;
using System;
using System.Windows.Input;

namespace OneCSharp.Metadata.UI
{
    public sealed class AddServerDialogViewModel : ViewModelBase
    {
        public AddServerDialogViewModel()
        {
            ConfirmCommand = new RelayCommand(Confirm);
            CancelCommand = new RelayCommand(Cancel);
        }
        private string _serverName;
        public string ServerName
        {
            get { return _serverName; }
            set
            {
                _serverName = value;
                OnPropertyChanged(nameof(ServerName));
            }
        }
        public Action OnCancel { get; set; }
        public Action<object> OnConfirm { get; set; }
        public ICommand ConfirmCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        private void Confirm(object parameter)
        {
            OnConfirm?.Invoke(ServerName);
        }
        private void Cancel(object parameter)
        {
            OnCancel?.Invoke();
        }
    }
}
