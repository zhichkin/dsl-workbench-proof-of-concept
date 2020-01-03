using OneCSharp.MVVM;
using OneCSharp.SQL.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace OneCSharp.SQL.UI
{
    public sealed class SelectInfoBaseDialogViewModel : ViewModelBase
    {
        public SelectInfoBaseDialogViewModel(List<Database> nodes)
        {
            Nodes = new ObservableCollection<Database>();
            foreach (var node in nodes)
            {
                Nodes.Add(node);
            }
            ConfirmCommand = new RelayCommand(Confirm);
            CancelCommand = new RelayCommand(Cancel);
        }
        public ObservableCollection<Database> Nodes { get; private set; }
        public Action OnCancel { get; set; }
        public Action<object> OnConfirm { get; set; }
        public Database SelectedItem { get; set; }
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