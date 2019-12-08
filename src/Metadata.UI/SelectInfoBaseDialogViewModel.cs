using OneCSharp.Metadata.Model;
using OneCSharp.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace OneCSharp.Metadata.UI
{
    public sealed class SelectInfoBaseDialogViewModel : ViewModelBase
    {
        public SelectInfoBaseDialogViewModel(IShell shell, List<IDomain> nodes)
        {
            Nodes = new ObservableCollection<DomainViewModel>();
            foreach (var node in nodes)
            {
                Nodes.Add(new DomainViewModel(shell, (Domain)node));
            }
            ConfirmCommand = new RelayCommand(Confirm);
            CancelCommand = new RelayCommand(Cancel);
        }
        public ObservableCollection<DomainViewModel> Nodes { get; private set; }
        public Action OnCancel { get; set; }
        public Action<object> OnConfirm { get; set; }
        public DomainViewModel SelectedItem { get; set; }
        public ICommand ConfirmCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        private void Confirm(object parameter)
        {
            OnConfirm?.Invoke(SelectedItem);
        }
        private void Cancel(object parameter)
        {
            OnCancel?.Invoke();
        }
    }
}
