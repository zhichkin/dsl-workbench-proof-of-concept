using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace OneCSharp.MVVM
{
    public sealed class TreeNodeViewModel : ViewModelBase
    {
        private string _nodeText;
        private string _nodeToolTip;
        private BitmapImage _nodeIcon;
        private object _nodePayload;
        private bool _isExpanded;
        public TreeNodeViewModel()
        {
            SelectedItemChanged = new RelayCommand(SelectedItemChangedHandler);
        }
        public string NodeText
        {
            get { return _nodeText; }
            set { _nodeText = value; OnPropertyChanged(nameof(NodeText)); }
        }
        public string NodeToolTip
        {
            get { return _nodeToolTip; }
            set { _nodeToolTip = value; OnPropertyChanged(nameof(NodeToolTip)); }
        }
        public BitmapImage NodeIcon
        {
            get { return _nodeIcon; }
            set { _nodeIcon = value; OnPropertyChanged(nameof(NodeIcon)); }
        }
        public object NodePayload
        {
            get { return _nodePayload; }
            set { _nodePayload = value; OnPropertyChanged(nameof(NodePayload)); }
        }
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (_isExpanded == value) return;
                _isExpanded = value; OnPropertyChanged("IsExpanded");
            }
        }
        public ICommand SelectedItemChanged { get; set; }
        private void SelectedItemChangedHandler(object parameter)
        {
            var args = parameter as RoutedPropertyChangedEventArgs<object>;
            if (args == null) return;
            args.Handled = true;
            SelectedItem = args.NewValue as TreeNodeViewModel;
        }
        public TreeNodeViewModel SelectedItem { get; private set; }
        public ObservableCollection<TreeNodeViewModel> TreeNodes { get; } = new ObservableCollection<TreeNodeViewModel>();
        public ObservableCollection<MenuItemViewModel> ContextMenuItems { get; } = new ObservableCollection<MenuItemViewModel>();
    }
}