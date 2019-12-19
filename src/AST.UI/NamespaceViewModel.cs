using OneCSharp.Core;
using OneCSharp.Metadata.Services;
using OneCSharp.MVVM;
using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace OneCSharp.AST.UI
{
    public sealed class NamespaceViewModel : ViewModelBase
    {
        private readonly IShell _shell;
        private readonly IMetadataProvider _metadataProvider;
        private readonly Namespace _model;
        private BitmapImage _iconImage = new BitmapImage(new Uri("pack://application:,,,/OneCSharp.AST.UI;component/images/NamespacePublic.png"));
        public NamespaceViewModel(Namespace model, IShell shell, IMetadataProvider metadataProvider)
        {
            _shell = shell ?? throw new ArgumentNullException(nameof(shell));
            _model = model ?? throw new ArgumentNullException(nameof(model));
            _metadataProvider = metadataProvider ?? throw new ArgumentNullException(nameof(metadataProvider));
            SetupContextMenu();
            InitializeViewModel();
        }
        private void SetupContextMenu()
        {
            ContextMenuItems.Add(new MenuItemViewModel()
            {
                HeaderText = "Add namespace...",
                MenuCommand = new RelayCommand(AddNamespace),
                IconImage = new BitmapImage(new Uri("pack://application:,,,/OneCSharp.AST.UI;component/images/AddNamespace.png"))
            });
        }
        private void InitializeViewModel()
        {
            if (_model.Children != null)
            {
                foreach (var ns in _model.Children)
                {
                    NamespaceViewModel vm = new NamespaceViewModel(ns, _shell, _metadataProvider);
                    Namespaces.Add(vm);
                }
            }
        }
        public ObservableCollection<MenuItemViewModel> ContextMenuItems { get; } = new ObservableCollection<MenuItemViewModel>();
        public ICommand AddNamespaceCommand { get; private set; }
        public string Name
        {
            get { return _model.Name; }
            set { _model.Name = value; OnPropertyChanged(nameof(Name)); }
        }
        public BitmapImage IconImage
        {
            get { return _iconImage; }
            set { _iconImage = value; OnPropertyChanged(nameof(IconImage)); }
        }
        public ObservableCollection<ViewModelBase> Namespaces { get; } = new ObservableCollection<ViewModelBase>();
        public void AddNamespace(object parameter)
        {
            InputStringDialog dialog = new InputStringDialog();
            _ = dialog.ShowDialog();
            if (dialog.Result == null) { return; }

            Namespace model = new Namespace()
            {
                Name = (string)dialog.Result
            };
            NamespaceViewModel vm = new NamespaceViewModel(model, _shell, _metadataProvider);
            Namespaces.Add(vm);
        }
    }
}