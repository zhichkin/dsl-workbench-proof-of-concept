using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace OneCSharp.MVVM
{
    public sealed class MenuItemViewModel : ViewModelBase
    {
        private string _header;
        private ICommand _command;
        private BitmapImage _image;
        private object _payload;
        public MenuItemViewModel()
        {

        }
        public string MenuItemHeader
        {
            get { return _header; }
            set { _header = value; OnPropertyChanged(nameof(MenuItemHeader)); }
        }
        public ICommand MenuItemCommand
        {
            get { return _command; }
            set { _command = value; OnPropertyChanged(nameof(MenuItemCommand)); }
        }
        public BitmapImage MenuItemIcon
        {
            get { return _image; }
            set { _image = value; OnPropertyChanged(nameof(MenuItemIcon)); }
        }
        public object MenuItemPayload
        {
            get { return _payload; }
            set { _payload = value; OnPropertyChanged(nameof(MenuItemPayload)); }
        }
        public ObservableCollection<MenuItemViewModel> MenuItems { get; } = new ObservableCollection<MenuItemViewModel>();
    }
}