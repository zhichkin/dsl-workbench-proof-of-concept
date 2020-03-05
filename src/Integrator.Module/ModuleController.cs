using OneCSharp.Integrator.Model;
using OneCSharp.MVVM;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace OneCSharp.Integrator.Module
{
    public sealed class ModuleController : IController
    {
        private const string MODULE_NAME = "Integrator";
        private const string MODULE_TOOLTIP = "Integrator module";
        private const string NODES_NAME = "Nodes";
        private const string NODES_TOOLTIP = "Integration nodes";

        private const string MODULE_ICON_PATH = "pack://application:,,,/OneCSharp.Integrator.Module;component/images/Cloud.png";
        private const string WEB_SERVER_PATH = "pack://application:,,,/OneCSharp.Integrator.Module;component/images/WebServer.png";
        private const string ADD_NODE_PATH = "pack://application:,,,/OneCSharp.Integrator.Module;component/images/AddLocalServer.png";
        private const string ADD_CATALOG_PATH = "pack://application:,,,/OneCSharp.Integrator.Module;component/images/AddWebCatalog.png";

        private readonly BitmapImage MODULE_ICON = new BitmapImage(new Uri(MODULE_ICON_PATH));
        private readonly BitmapImage WEB_SERVER_ICON = new BitmapImage(new Uri(WEB_SERVER_PATH));
        private readonly BitmapImage ADD_NODE_ICON = new BitmapImage(new Uri(ADD_NODE_PATH));
        private readonly BitmapImage ADD_CATALOG_ICON = new BitmapImage(new Uri(ADD_CATALOG_PATH));

        private IShell Shell { get; set; }
        public ModuleController(IShell shell)
        {
            Shell = shell;
        }
        public string ModuleCatalogPath
        {
            get { return Path.Combine(Shell.ModulesCatalogPath, MODULE_NAME); }
        }
        public void BuildTreeNode(object model, out TreeNodeViewModel treeNode)
        {
            treeNode = new TreeNodeViewModel()
            {
                IsExpanded = true,
                NodeIcon = MODULE_ICON,
                NodeText = MODULE_NAME,
                NodeToolTip = MODULE_TOOLTIP,
                NodePayload = null
            };
            treeNode.ContextMenuItems.Add(new MenuItemViewModel()
            {
                MenuItemHeader = "About...",
                MenuItemIcon = MODULE_ICON,
                MenuItemCommand = new RelayCommand(ShowAboutWindow),
                MenuItemPayload = null
            });

            ConfigureModel();
            ConfigureNodes(treeNode);
        }
        private void ShowAboutWindow(object parameter)
        {
            MessageBox.Show("1C# Integrator module © 2020"
                + Environment.NewLine
                + Environment.NewLine + "Created by Zhichkin"
                + Environment.NewLine + "dima_zhichkin@mail.ru"
                + Environment.NewLine + "https://github.com/zhichkin/",
                "1C#",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
        private void ConfigureNodes(TreeNodeViewModel mainNode)
        {
            TreeNodeViewModel nodes = new TreeNodeViewModel()
            {
                IsExpanded = true,
                NodeIcon = WEB_SERVER_ICON,
                NodeText = NODES_NAME,
                NodeToolTip = NODES_TOOLTIP,
                NodePayload = null
            };
            nodes.ContextMenuItems.Add(new MenuItemViewModel()
            {
                MenuItemHeader = "Add new catalog...",
                MenuItemIcon = ADD_CATALOG_ICON,
                MenuItemCommand = new RelayCommand(AddIntegrationCatalog),
                MenuItemPayload = nodes
            });
            nodes.ContextMenuItems.Add(new MenuItemViewModel()
            {
                MenuItemHeader = "Add integration node...",
                MenuItemIcon = ADD_NODE_ICON,
                MenuItemCommand = new RelayCommand(AddIntegrationNode),
                MenuItemPayload = nodes
            });
            mainNode.TreeNodes.Add(nodes);
        }

        private void ConfigureModel()
        {
            if (!Directory.Exists(ModuleCatalogPath))
            {
                _ = Directory.CreateDirectory(ModuleCatalogPath);
            }
            //TODO: load model from integrator.json file, see Core.Services for serializer
        }

        private void AddIntegrationCatalog(object parameter)
        {
            TreeNodeViewModel treeNode = parameter as TreeNodeViewModel;
            if (treeNode == null) return;

            IntegrationNodeCatalog catalog = new IntegrationNodeCatalog();
        }
        private void AddIntegrationNode(object parameter)
        {
            TreeNodeViewModel treeNode = parameter as TreeNodeViewModel;
            if (treeNode == null) return;
            //IntegrationNodeCatalog server = treeNode.NodePayload as IntegrationNodeCatalog;
            //if (server == null) return;
        }
    }
}