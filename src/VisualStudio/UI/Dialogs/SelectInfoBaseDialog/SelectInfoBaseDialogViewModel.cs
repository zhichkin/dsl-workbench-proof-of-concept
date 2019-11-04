using Microsoft.VisualStudio.PlatformUI;
using OneCSharp.Metadata;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace OneCSharp.VisualStudio.UI
{
    public sealed class SelectInfoBaseDialogViewModel : ViewModelBase
    {
        public SelectInfoBaseDialogViewModel(List<InfoBase> infoBases)
        {
            InfoBases = new ObservableCollection<InfoBaseViewModel>();
            foreach (var ib in infoBases)
            {
                InfoBases.Add(new InfoBaseViewModel(ib));
            }
            ConfirmCommand = new DelegateCommand(Confirm);
            CancelCommand = new DelegateCommand(Cancel);
        }
        public ObservableCollection<InfoBaseViewModel> InfoBases { get; private set; }
        public Action OnCancel { get; set; }
        public Action<object> OnConfirm { get; set; }
        public InfoBaseViewModel SelectedItem { get; set; }
        public ICommand ConfirmCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        private void Confirm()
        {
            OnConfirm?.Invoke(SelectedItem);
        }
        private void Cancel()
        {
            OnCancel?.Invoke();
        }
    }
}
