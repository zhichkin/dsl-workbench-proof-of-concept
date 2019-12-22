using OneCSharp.Metadata.Model;
using OneCSharp.Metadata.Services;
using OneCSharp.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace OneCSharp.Metadata.UI
{
    public sealed class Module : IModule
    {
        private const string ONE_C_SHARP = "ONE-C-SHARP";
        private IShell _shell;
        private readonly Dictionary<Type, IController> _controllers = new Dictionary<Type, IController>();
        public Module() { }
        public IController GetController<T>()
        {
            return _controllers[typeof(T)];
        }
        public IController GetController(Type type)
        {
            return _controllers[type];
        }
        public void Initialize(IShell shell)
        {
            _shell = shell;

            _shell.AddMenuItem(new MenuItemViewModel()
            {
                MenuItemIcon = new BitmapImage(new Uri("pack://application:,,,/OneCSharp.Metadata.UI;component/images/AddLocalServer.png")),
                MenuItemHeader = "Connect to server",
                MenuItemCommand = new RelayCommand(ConnectToServer),
                MenuItemPayload = this
            });
        }
        private void ConnectToServer(object parameter)
        {
            InputStringDialog dialog = new InputStringDialog();
            _ = dialog.ShowDialog();
            if (dialog.Result == null) return;

            IMetadataProvider metadataProvider = _shell.GetService<IMetadataProvider>();

            string serverName = (string)dialog.Result;
            IServer server = metadataProvider.Servers.Where(s => s.Address == serverName).FirstOrDefault();
            if (server != null) return;
            server = new Model.Server() { Address = serverName };
            if (!metadataProvider.CheckServerConnection(server))
            {
                var result = MessageBox.Show(
                    "Unable to connect server!",
                    ONE_C_SHARP,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            metadataProvider.AddServer(server);
            ServerViewModel viewModel = new ServerViewModel(_shell, (Model.Server)server, metadataProvider);
            //serverViewModel.Parent = this;

            var treeNode = new TreeNodeViewModel()
            {
                NodeText = server.Address,
                NodeIcon = new BitmapImage(new Uri("pack://application:,,,/OneCSharp.Metadata.UI;component/images/LocalServer.png")),
                NodePayload = viewModel
            };
            treeNode.ContextMenuItems.Add(new MenuItemViewModel()
            {
                MenuItemHeader = "Connect to database...",
                MenuItemIcon = new BitmapImage(new Uri("pack://application:,,,/OneCSharp.Metadata.UI;component/images/ConnectToDatabase.png")),
                //MenuItemCommand = viewModel.AddInfoBase,
                MenuItemPayload = treeNode
            });

            _shell.AddTreeNode(treeNode);
        }

        public void Persist(Core.Entity model)
        {
            throw new NotImplementedException();
        }
    }
}