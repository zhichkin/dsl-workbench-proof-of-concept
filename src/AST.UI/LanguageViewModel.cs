using OneCSharp.AST.Model;
using OneCSharp.Core;
using OneCSharp.MVVM;
using System;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace OneCSharp.AST.UI
{
    public sealed class LanguageViewModel : ViewModelBase
    {
        private readonly IShell _shell;
        private readonly Language _model;
        private const string ADD_NAMESPACE = "pack://application:,,,/OneCSharp.AST.UI;component/images/AddNamespace.png";
        private const string NAMESPACE_PUBLIC = "pack://application:,,,/OneCSharp.AST.UI;component/images/NamespacePublic.png";
        public LanguageViewModel(Language model, IShell shell)
        {
            _shell = shell ?? throw new ArgumentNullException(nameof(shell));
            _model = model ?? throw new ArgumentNullException(nameof(model));
            InitializeViewModel();
        }
        private void InitializeViewModel()
        {
            AddNamespaceCommand = new RelayCommand(AddNamespace);

            //if (_model.Namespaces != null)
            //{
            //    foreach (var ns in _model.Namespaces)
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
                NodeIcon = new BitmapImage(new Uri(NAMESPACE_PUBLIC)),
                NodePayload = viewModel
            };
            treeNode.ContextMenuItems.Add(new MenuItemViewModel()
            {
                MenuItemHeader = "Add namespace...",
                MenuItemIcon = new BitmapImage(new Uri(ADD_NAMESPACE)),
                MenuItemCommand = viewModel.AddNamespaceCommand,
                MenuItemPayload = treeNode
            });

            TreeNodeViewModel node = (TreeNodeViewModel)parameter;
            node.TreeNodes.Add(treeNode);
        }
    }
}