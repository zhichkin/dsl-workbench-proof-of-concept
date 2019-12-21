using OneCSharp.Core;
using OneCSharp.MVVM;
using System;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace OneCSharp.AST.UI
{
    public sealed class NamespaceViewModel : ViewModelBase
    {
        private readonly IShell _shell;
        private readonly Namespace _model;
        public NamespaceViewModel(Namespace model, IShell shell)
        {
            _shell = shell ?? throw new ArgumentNullException(nameof(shell));
            _model = model ?? throw new ArgumentNullException(nameof(model));
            SetupContextMenu();
            InitializeViewModel();
        }
        private void SetupContextMenu()
        {
            //ContextMenuItems.Add(new MenuItemViewModel()
            //{
            //    MenuItemHeader = "Add namespace...",
            //    MenuItemCommand = new RelayCommand(AddNamespace),
            //    MenuItemIcon = new BitmapImage(new Uri("pack://application:,,,/OneCSharp.AST.UI;component/images/AddNamespace.png"))
            //});
        }
        private void InitializeViewModel()
        {
            AddNamespaceCommand = new RelayCommand(AddNamespace);

            //if (_model.Children != null)
            //{
            //    foreach (var ns in _model.Children)
            //    {
            //        NamespaceViewModel vm = new NamespaceViewModel(ns, _shell);
            //        Namespaces.Add(vm);
            //    }
            //}
        }
        public ICommand AddNamespaceCommand { get; private set; }
        public string Name
        {
            get { return _model.Name; }
            set { _model.Name = value; OnPropertyChanged(nameof(Name)); }
        }
        public void AddNamespace(object parameter)
        {
            InputStringDialog dialog = new InputStringDialog();
            _ = dialog.ShowDialog();
            if (dialog.Result == null) { return; }

            Namespace model = new Namespace()
            {
                Name = (string)dialog.Result
            };
            NamespaceViewModel viewModel = new NamespaceViewModel(model, _shell);

            var treeNode = new TreeNodeViewModel()
            {
                NodeText = model.Name,
                NodeIcon = new BitmapImage(new Uri("pack://application:,,,/OneCSharp.AST.UI;component/images/NamespacePublic.png")),
                NodePayload = viewModel
            };
            treeNode.ContextMenuItems.Add(new MenuItemViewModel()
            {
                MenuItemHeader = "Add namespace...",
                MenuItemIcon = new BitmapImage(new Uri("pack://application:,,,/OneCSharp.AST.UI;component/images/AddNamespace.png")),
                MenuItemCommand = viewModel.AddNamespaceCommand,
                MenuItemPayload = treeNode
            });

            TreeNodeViewModel node = (TreeNodeViewModel)parameter;
            node.TreeNodes.Add(treeNode);
        }
    }
}