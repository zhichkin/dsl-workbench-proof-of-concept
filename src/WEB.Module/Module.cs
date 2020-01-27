using OneCSharp.AST.Model;
using OneCSharp.AST.UI;
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
        #region " String resources "
        public const string ONE_C_SHARP = "ONE-C-SHARP";
        public const string WEB_SERVER = "pack://application:,,,/OneCSharp.WEB.Module;component/images/WebServer.png";
        public const string WEB_NAMESPACE = "pack://application:,,,/OneCSharp.WEB.Module;component/images/Namespace.png";
        public const string WEB_APPLICATON = "pack://application:,,,/OneCSharp.WEB.Module;component/images/WebApplication.png";
        public const string WEB_INTERFACE = "pack://application:,,,/OneCSharp.WEB.Module;component/images/Interface.png";
        public const string WEB_SERVICE = "pack://application:,,,/OneCSharp.WEB.Module;component/images/WebService.png";
        public const string ADD_WEB_SERVER = "pack://application:,,,/OneCSharp.WEB.Module;component/images/AddWebService.png";
        public const string ADD_WEB_NAMESPACE = "pack://application:,,,/OneCSharp.WEB.Module;component/images/AddNamespace.png";
        public const string ADD_WEB_APPLICATON = "pack://application:,,,/OneCSharp.WEB.Module;component/images/AddWebApplication.png";
        public const string ADD_WEB_INTERFACE = "pack://application:,,,/OneCSharp.WEB.Module;component/images/AddInterface.png";
        public const string EDIT_WEB_METHOD = "pack://application:,,,/OneCSharp.WEB.Module;component/images/EditWindow.png";
        #endregion

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

            //_controllers.Add(typeof(WebMethod), new FunctionConceptController());

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
            treeNode.ContextMenuItems.Add(new MenuItemViewModel()
            {
                MenuItemHeader = "Add web service",
                MenuItemIcon = new BitmapImage(new Uri(ADD_WEB_APPLICATON)),
                MenuItemCommand = new RelayCommand(CreateWebService),
                MenuItemPayload = treeNode
            });
            Shell.AddTreeNode(treeNode);
        }
        private void CreateWebService(object parameter)
        {
            InputStringDialog dialog = new InputStringDialog();
            _ = dialog.ShowDialog();
            if (dialog.Result == null) return;

            string serviceName = (string)dialog.Result;
            WebService service = new WebService() { Name = serviceName };

            TreeNodeViewModel treeNode = (TreeNodeViewModel)parameter;
            WebServer server = (WebServer)treeNode.NodePayload;
            if (server == null) return;

            service.Owner = server;
            server.Domains.Add(service);

            var childNode = new TreeNodeViewModel()
            {
                NodeText = service.Name,
                NodeIcon = new BitmapImage(new Uri(WEB_APPLICATON)),
                NodePayload = service
            };
            childNode.ContextMenuItems.Add(new MenuItemViewModel()
            {
                MenuItemHeader = "Add namespace",
                MenuItemIcon = new BitmapImage(new Uri(ADD_WEB_NAMESPACE)),
                MenuItemCommand = new RelayCommand(CreateWebNamespace),
                MenuItemPayload = childNode
            });
            treeNode.TreeNodes.Add(childNode);

            //Persist(server);
        }
        private void CreateWebNamespace(object parameter)
        {
            InputStringDialog dialog = new InputStringDialog();
            _ = dialog.ShowDialog();
            if (dialog.Result == null) return;

            string namespaceName = (string)dialog.Result;
            Namespace child = new Namespace() { Name = namespaceName };

            TreeNodeViewModel treeNode = (TreeNodeViewModel)parameter;
            if (treeNode.NodePayload is Domain domain)
            {
                child.Owner = domain;
                domain.Namespaces.Add(child);
            }
            else if (treeNode.NodePayload is Namespace parent)
            {
                child.Owner = parent;
                parent.Namespaces.Add(child);
            }
            else
            {
                return;
            }

            var childNode = new TreeNodeViewModel()
            {
                NodeText = child.Name,
                NodePayload = child,
                NodeIcon = new BitmapImage(new Uri(WEB_NAMESPACE))
            };
            childNode.ContextMenuItems.Add(new MenuItemViewModel()
            {
                MenuItemHeader = "Add namespace",
                MenuItemPayload = childNode,
                MenuItemCommand = new RelayCommand(CreateWebNamespace),
                MenuItemIcon = new BitmapImage(new Uri(ADD_WEB_NAMESPACE)),
            });
            childNode.ContextMenuItems.Add(new MenuItemViewModel()
            {
                MenuItemHeader = "Add interface",
                MenuItemPayload = childNode,
                MenuItemCommand = new RelayCommand(CreateWebInterface),
                MenuItemIcon = new BitmapImage(new Uri(ADD_WEB_INTERFACE)),
            });
            treeNode.TreeNodes.Add(childNode);

            //Persist(server);
        }
        private void CreateWebInterface(object parameter)
        {
            InputStringDialog dialog = new InputStringDialog();
            _ = dialog.ShowDialog();
            if (dialog.Result == null) return;

            string childName = (string)dialog.Result;
            Interface child = new Interface() { Name = childName };

            TreeNodeViewModel treeNode = (TreeNodeViewModel)parameter;
            Namespace owner = (Namespace)treeNode.NodePayload;
            if (owner == null) return;

            child.Owner = owner;
            owner.Interfaces.Add(child);

            var childNode = new TreeNodeViewModel()
            {
                NodeText = child.Name,
                NodeIcon = new BitmapImage(new Uri(WEB_INTERFACE)),
                NodePayload = child
            };
            childNode.ContextMenuItems.Add(new MenuItemViewModel()
            {
                MenuItemHeader = "Add method",
                MenuItemIcon = new BitmapImage(new Uri(ADD_WEB_SERVER)),
                MenuItemCommand = new RelayCommand(CreateWebMethod),
                MenuItemPayload = childNode
            });
            treeNode.TreeNodes.Add(childNode);

            //Persist(server);
        }
        private void CreateWebMethod(object parameter)
        {
            InputStringDialog dialog = new InputStringDialog();
            _ = dialog.ShowDialog();
            if (dialog.Result == null) return;

            string childName = (string)dialog.Result;
            WebMethod child = new WebMethod() { Name = childName };

            TreeNodeViewModel treeNode = (TreeNodeViewModel)parameter;
            Interface owner = (Interface)treeNode.NodePayload;
            if (owner == null) return;

            child.Owner = owner;
            owner.Methods.Add(child);

            var childNode = new TreeNodeViewModel()
            {
                NodeText = child.Name,
                NodeIcon = new BitmapImage(new Uri(WEB_SERVICE)),
                NodePayload = child
            };
            childNode.ContextMenuItems.Add(new MenuItemViewModel()
            {
                MenuItemHeader = "Edit method",
                MenuItemIcon = new BitmapImage(new Uri(EDIT_WEB_METHOD)),
                MenuItemCommand = new RelayCommand(EditWebMethod),
                MenuItemPayload = childNode
            });
            treeNode.TreeNodes.Add(childNode);

            //Persist(server);
        }
        private void EditWebMethod(object parameter)
        {
            TreeNodeViewModel treeNode = (TreeNodeViewModel)parameter;
            WebMethod method = (WebMethod)treeNode.NodePayload;
            if (method == null) return;

            //TODO: bind/deserialize WebMethod with LanguageConcept !!!
            Language language = AST.Model.OneCSharp.ONECSHARP;
            LanguageConcept grammar = language.Concept(AST.Model.OneCSharp.FUNCTION);
            //syntaxTree.Owner = method;

            LanguageConceptController controller = new LanguageConceptController();
            CodeEditor editor = new CodeEditor()
            {
                DataContext = controller.CreateConceptNode(null, grammar) // TODO: add parameter for syntaxTree to be edited by user
            };

            Shell.AddTabItem(method.Name, editor);
        }
    }
}