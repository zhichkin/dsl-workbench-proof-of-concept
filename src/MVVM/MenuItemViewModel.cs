using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace OneCSharp.MVVM
{
    public sealed class MenuItemViewModel : ViewModelBase
    {
        private string _headerText;
        private ICommand _menuCommand;
        private BitmapImage _iconImage;
        public MenuItemViewModel()
        {

        }
        public string HeaderText
        {
            get { return _headerText; }
            set { _headerText = value; OnPropertyChanged(nameof(HeaderText)); }
        }
        public ICommand MenuCommand
        {
            get { return _menuCommand; }
            set { _menuCommand = value; OnPropertyChanged(nameof(MenuCommand)); }
        }
        public BitmapImage IconImage
        {
            get { return _iconImage; }
            set { _iconImage = value; OnPropertyChanged(nameof(IconImage)); }
        }
        public ObservableCollection<MenuItemViewModel> MenuItems { get; } = new ObservableCollection<MenuItemViewModel>();
    }
}