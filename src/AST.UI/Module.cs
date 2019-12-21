using OneCSharp.AST.Model;
using OneCSharp.MVVM;
using System;
using System.Windows.Media.Imaging;

namespace OneCSharp.AST.UI
{
    public sealed class Module : IModule
    {
        private IShell _shell;
        public Module() { }
        public void Initialize(IShell shell)
        {
            _shell = shell;

            _shell.AddMenuItem(new MenuItemViewModel()
            {
                MenuItemIcon = new BitmapImage(new Uri("pack://application:,,,/OneCSharp.AST.UI;component/images/AddLanguage.png")),
                MenuItemHeader = "Add new language",
                MenuItemCommand = new RelayCommand(AddLanguage),
                MenuItemPayload = this
            });
        }
        private void AddLanguage(object parameter)
        {
            InputStringDialog dialog = new InputStringDialog();
            _ = dialog.ShowDialog();
            if (dialog.Result == null) return;

            Language language = new Language()
            {
                Name = (string)dialog.Result
            };
            LanguageViewModel viewModel = new LanguageViewModel(language, _shell);

            var treeNode = new TreeNodeViewModel()
            {
                NodeText = language.Name,
                NodeIcon = new BitmapImage(new Uri("pack://application:,,,/OneCSharp.AST.UI;component/images/Solution.png")),
                NodePayload = viewModel
            };
            treeNode.ContextMenuItems.Add(new MenuItemViewModel()
            {
                MenuItemHeader = "Add namespace...",
                MenuItemIcon = new BitmapImage(new Uri("pack://application:,,,/OneCSharp.AST.UI;component/images/AddNamespace.png")),
                MenuItemCommand = viewModel.AddNamespaceCommand,
                MenuItemPayload = treeNode
            });

            _shell.AddTreeNode(treeNode);
        }
    }
}