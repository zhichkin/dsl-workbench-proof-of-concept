using OneCSharp.Core.Model;
using OneCSharp.MVVM;
using OneCSharp.SQL.Model;
using OneCSharp.SQL.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace OneCSharp.SQL.UI
{
    public sealed class Module : IModule
    {
        private const string ONE_C_SHARP = "ONE-C-SHARP";
        public const string LOCAL_SERVER = "pack://application:,,,/OneCSharp.SQL.UI;component/images/DataServer.png";
        public const string ADD_LOCAL_SERVER = "pack://application:,,,/OneCSharp.SQL.UI;component/images/AddLocalServer.png";
        public const string CONNECT_TO_DATABASE = "pack://application:,,,/OneCSharp.SQL.UI;component/images/ConnectToDatabase.png";
        public const string DATABASE = "pack://application:,,,/OneCSharp.SQL.UI;component/images/Database.png";
        public const string ADD_NAMESPACE = "pack://application:,,,/OneCSharp.SQL.UI;component/images/AddNamespace.png";
        public const string NAMESPACE_PUBLIC = "pack://application:,,,/OneCSharp.SQL.UI;component/images/NamespacePublic.png";
        public const string TABLE = "pack://application:,,,/OneCSharp.SQL.UI;component/images/Table.png";
        public const string NESTED_TABLE = "pack://application:,,,/OneCSharp.SQL.UI;component/images/NestedTable.png";
        public const string FIELD_PUBLIC = "pack://application:,,,/OneCSharp.SQL.UI;component/images/FieldPublic.png";
        private IShell _shell;
        private readonly Dictionary<Type, IController> _controllers = new Dictionary<Type, IController>();
        public Module() { }
        public IShell Shell { get { return _shell; } }
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
            _shell = shell ?? throw new ArgumentNullException(nameof(shell));

            _controllers.Add(typeof(Database), new DatabaseController(this));

            _shell.AddMenuItem(new MenuItemViewModel()
            {
                MenuItemIcon = new BitmapImage(new Uri(ADD_LOCAL_SERVER)),
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

            IMetadataReader metadataProvider = _shell.GetService<IMetadataReader>();

            string serverName = (string)dialog.Result;
            Server server = new Server() { Address = serverName };
            if (!metadataProvider.CheckServerConnection(server))
            {
                _ = MessageBox.Show(
                    "Unable to connect server!",
                    ONE_C_SHARP,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            var treeNode = new TreeNodeViewModel()
            {
                NodeText = server.Address,
                NodeIcon = new BitmapImage(new Uri(LOCAL_SERVER)),
                NodePayload = server
            };
            treeNode.ContextMenuItems.Add(new MenuItemViewModel()
            {
                MenuItemHeader = "Connect to database...",
                MenuItemIcon = new BitmapImage(new Uri(CONNECT_TO_DATABASE)),
                MenuItemCommand = new AsyncCommand(ConnectToInfoBase, CanExecuteConnectToInfoBase),
                MenuItemPayload = treeNode
            });

            _shell.AddTreeNode(treeNode);
        }
        public bool IsBusy { get; set; } = false;
        private bool CanExecuteConnectToInfoBase() { return !IsBusy; }
        private async Task ConnectToInfoBase(object parameter)
        {
            TreeNodeViewModel treeNode = parameter as TreeNodeViewModel;
            if (treeNode == null) return;
            Server server = treeNode.NodePayload as Server;
            if (server == null) return;

            IMetadataReader metadataReader = _shell.GetService<IMetadataReader>();
            List<Database> databases = metadataReader.GetDatabases(server);
            if (databases.Count == 0) return;

            SelectInfoBaseDialog dialog = new SelectInfoBaseDialog(databases);
            _ = dialog.ShowDialog();
            if (dialog.Result == null) return;

            string databaseName = ((Database)dialog.Result).Name;
            Database database = (Database)server.Domains.Where(i => i.Name == databaseName).FirstOrDefault();
            if (database != null) return;

            database = new Database() { Name = databaseName, Owner = server };
            server.Domains.Add(database);

            await DoWorkAsync(database);

            IController controller = GetController<Database>();
            controller.BuildTreeNode(database, out TreeNodeViewModel childNode);
            treeNode.TreeNodes.Add(childNode);
            //Persist(database);
        }
        private async Task DoWorkAsync(Database database)
        {
            IMetadataReader metadataReader = _shell.GetService<IMetadataReader>();

            _shell.ShowStatusBarMessage(string.Empty);
            try
            {
                Progress<string> progress = new Progress<string>(ReportProgress);
                Stopwatch stopwatch = Stopwatch.StartNew();
                await metadataReader.ReadMetadataAsync(database, progress);
                stopwatch.Stop();
                _shell.ShowStatusBarMessage(stopwatch.ElapsedMilliseconds.ToString());
            }
            catch (Exception error)
            {
                _ = MessageBox.Show($"{error.Message}", "ONE-C-SHARP", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        private void ReportProgress(string message)
        {
            _shell.ShowStatusBarMessage(message);
        }
    }
}