using OneCSharp.Core.Model;
using OneCSharp.MVVM;
using OneCSharp.WEB.Model;
using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace OneCSharp.WEB.Module
{
    public sealed class Module : IModule
    {
        private const string ONE_C_SHARP = "ONE-C-SHARP";
        public const string WEB_SERVER = "pack://application:,,,/OneCSharp.WEB.Module;component/images/WebServer.png";
        public const string ADD_WEB_SERVER = "pack://application:,,,/OneCSharp.WEB.Module;component/images/AddWebService.png";

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
        public IShell Shell { get; private set; }
        public void Initialize(IShell shell)
        {
            Shell = shell ?? throw new ArgumentNullException(nameof(shell));

            //_controllers.Add(typeof(Database), new DatabaseController(this));

            Shell.AddMenuItem(new MenuItemViewModel()
            {
                MenuItemIcon = new BitmapImage(new Uri(ADD_WEB_SERVER)),
                MenuItemHeader = "Add web server",
                MenuItemCommand = new RelayCommand(CreateWebServer),
                MenuItemPayload = this
            });
        }
        public void Persist(Entity entity)
        {
            throw new NotImplementedException();
        }

        private void CreateWebServer(object parameter)
        {
            InputStringDialog dialog = new InputStringDialog();
            _ = dialog.ShowDialog();
            if (dialog.Result == null) return;
            string serverAddress = (string)dialog.Result;
            WebServer server = new WebServer() { Address = serverAddress };

            var treeNode = new TreeNodeViewModel()
            {
                NodeText = server.Address,
                NodeIcon = new BitmapImage(new Uri(WEB_SERVER)),
                NodePayload = server
            };
            //treeNode.ContextMenuItems.Add(new MenuItemViewModel()
            //{
            //    MenuItemHeader = "Add web service",
            //    MenuItemIcon = new BitmapImage(new Uri(CONNECT_TO_DATABASE)),
            //    MenuItemCommand = new RelayCommand(ConnectToInfoBase),
            //    MenuItemPayload = treeNode
            //});
            Shell.AddTreeNode(treeNode);
        }
    }
}