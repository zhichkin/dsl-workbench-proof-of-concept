using OneCSharp.DSL.Model;
using OneCSharp.DSL.Services;
using OneCSharp.DSL.UI;
using OneCSharp.Metadata.Model;
using OneCSharp.Metadata.Services;
using OneCSharp.MVVM;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace OneCSharp.Metadata.UI
{
    public sealed class MetadataViewModel : ViewModelBase
    {
        private const string ONE_C_SHARP = "ONE-C-SHARP";
        private readonly IShell _shell;
        private readonly IMetadataProvider _metadataProvider;
        public MetadataViewModel(IShell shell, IMetadataProvider metadataProvider)
        {
            _shell = shell;
            _metadataProvider = metadataProvider;
            InitializeViewModel();
        }
        public ICommand AddServerCommand { get; private set; }
        private void InitializeViewModel()
        {
            this.AddServerCommand = new RelayCommand(AddServer);
        }
        public ObservableCollection<ServerViewModel> Servers { get; } = new ObservableCollection<ServerViewModel>();
        public void AddServer(object parameter)
        {
            AddServerDialog dialog = new AddServerDialog();
            _ = dialog.ShowDialog();
            if (dialog.Result != null)
            {
                OnAddServer((string)dialog.Result);
            }
        }
        private void OnAddServer(string serverName)
        {
            IServer server = _metadataProvider.Servers.Where(s => s.Address == serverName).FirstOrDefault();
            if (server != null) return;
            server = new Server() { Address = serverName };
            if (!_metadataProvider.CheckServerConnection(server))
            {
                var result = MessageBox.Show(
                    "Unable to connect server!",
                    ONE_C_SHARP,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            _metadataProvider.AddServer(server);
            ServerViewModel serverViewModel = new ServerViewModel(_shell, (Server)server, _metadataProvider);
            serverViewModel.Parent = this;
            Servers.Add(serverViewModel);
        }
        public object SelectedItem { get; set; }
        public void ImportInfoBase()
        {
            if (SelectedItem is ServerViewModel)
            {
                ServerViewModel server = (ServerViewModel)SelectedItem;
                List<IDomain> nodes = _metadataProvider.GetDomains(server.Model);
                SelectInfoBaseDialog dialog = new SelectInfoBaseDialog(_shell, nodes);
                _ = dialog.ShowDialog();
                if (dialog.Result != null)
                {
                    DomainViewModel selectedItem = (DomainViewModel)dialog.Result;
                    OnInfoBaseSelected(server, selectedItem);
                }
            }
        }
        private void OnInfoBaseSelected(ServerViewModel parent, DomainViewModel child)
        {
            if (parent.Model.Domains
                .Where(ib => ib.Database == child.Model.Database)
                .FirstOrDefault() != null) return;

            child.Model.Server = parent.Model;
            _metadataProvider.ImportMetadata(child.Model);
            parent.AddInfoBase(child);
        }


        public void AddWebService()
        {
            if (SelectedItem == null)
            {
                _ = MessageBox.Show(
                    "Web service owner is not selected!",
                    ONE_C_SHARP,
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            string dbCatalogAddress;
            AddServerDialog dialog = new AddServerDialog();
            _ = dialog.ShowDialog();
            if (dialog.Result == null)
            {
                return;
            }
            dbCatalogAddress = (string)dialog.Result;

            if (SelectedItem is ServerViewModel)
            {
                ServerViewModel server = (ServerViewModel)SelectedItem;
                server.CreateInfoBase(dbCatalogAddress);
            }
        }
        public void AddNamespace()
        {
            if (SelectedItem == null)
            {
                _ = MessageBox.Show(
                    "Namespace owner is not selected!",
                    ONE_C_SHARP,
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            string namespaceName;
            AddServerDialog dialog = new AddServerDialog();
            _ = dialog.ShowDialog();
            if (dialog.Result == null)
            {
                return;
            }
            namespaceName = (string)dialog.Result;

            if (SelectedItem is DomainViewModel)
            {
                DomainViewModel ib = (DomainViewModel)SelectedItem;
                ib.CreateNamespaceViewModel(namespaceName);

            }
            else if (SelectedItem is NamespaceViewModel)
            {
                NamespaceViewModel ns = (NamespaceViewModel)SelectedItem;
                ns.CreateNamespaceViewModel(namespaceName);
            }
        }
        public void AddProcedure()
        {
            if (SelectedItem == null)
            {
                _ = MessageBox.Show(
                    "Procedure owner is not selected!",
                    ONE_C_SHARP,
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            string name;
            AddServerDialog dialog = new AddServerDialog();
            _ = dialog.ShowDialog();
            if (dialog.Result == null)
            {
                return;
            }
            name = (string)dialog.Result;

            if (SelectedItem is NamespaceViewModel)
            {
                NamespaceViewModel ns = (NamespaceViewModel)SelectedItem;
                ns.CreateProcedureViewModel(name);
            }
        }
        public void RenameProcedure()
        {
            if (SelectedItem == null)
            {
                _ = MessageBox.Show(
                    "Procedure is not selected!",
                    ONE_C_SHARP,
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            string name;
            AddServerDialog dialog = new AddServerDialog();
            _ = dialog.ShowDialog();
            if (dialog.Result == null)
            {
                return;
            }
            name = (string)dialog.Result;

            if (SelectedItem is RequestViewModel)
            {
                RequestViewModel vm = (RequestViewModel)SelectedItem;
                vm.Name = name;
                //TODO: think how to save changes !
            }
        }
        public void EditProcedure()
        {
            if (SelectedItem == null)
            {
                _ = MessageBox.Show(
                    "Procedure is not selected!",
                    ONE_C_SHARP,
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }
            if (!(SelectedItem is RequestViewModel procedure))
            {
                _ = MessageBox.Show(
                    $"{SelectedItem.GetType()} is not procedure !",
                    ONE_C_SHARP,
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }
            if (!(SelectedItem is IOneCSharpCodeEditorConsumer consumer))
            {
                _ = MessageBox.Show(
                    $"{SelectedItem.GetType()} does not implement IOneCSharpCodeEditorConsumer interface !",
                    ONE_C_SHARP,
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }
            // TODO: get Procedure from storage (ex. file)
            consumer.Metadata = _metadataProvider;
            Procedure syntaxNode = new Procedure() { Name = procedure.Name };
            OpenOneCSharpCodeEditor(procedure.Name, consumer, syntaxNode);
        }
        private void OpenOneCSharpCodeEditor(string caption, IOneCSharpCodeEditorConsumer consumer, ISyntaxNode node)
        {
            // TODO: find existing window used by consumer
            var _codeEditor = new ProcedureViewModel(_shell, (Procedure)node);
            _codeEditor.Metadata = consumer.Metadata;
            _codeEditor.Save += consumer.SaveSyntaxNode;
            _shell.AddTabItem(caption, new ProcedureView(_codeEditor));
        }
    }
}
