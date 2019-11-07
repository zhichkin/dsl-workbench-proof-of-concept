using Microsoft.VisualStudio;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using OneCSharp.Metadata;
using OneCSharp.OQL.Model;
using OneCSharp.OQL.UI;
using OneCSharp.OQL.UI.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OneCSharp.VisualStudio.UI
{
    public sealed class MetadataViewModel : ViewModelBase
    {
        private readonly MetadataProvider _metadataProvider = new MetadataProvider();
        public MetadataViewModel()
        {
            InitializeViewModel();
        }
        public ICommand AddServerCommand { get; private set; }
        private void InitializeViewModel()
        {
            this.AddServerCommand = new DelegateCommand(AddServer);
        }
        public ObservableCollection<ServerViewModel> Servers { get; } = new ObservableCollection<ServerViewModel>();
        public void AddServer()
        {
            AddServerDialog dialog = new AddServerDialog();
            _ = dialog.ShowModal();
            if (dialog.Result != null)
            {
                OnAddServer((string)dialog.Result);
            }
        }
        private void OnAddServer(string serverName)
        {
            DbServer server = _metadataProvider.Servers.Where(s => s.Address == serverName).FirstOrDefault();
            if (server != null) return;
            server = new DbServer() { Address = serverName };
            if (!_metadataProvider.CheckServerConnection(server))
            {
                var result = MessageBox.Show(
                    "Unable to connect server!",
                    "ONE-C-SHARP",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            _metadataProvider.AddServer(server);
            ServerViewModel serverViewModel = new ServerViewModel(server, _metadataProvider);
            serverViewModel.Parent = this;
            Servers.Add(serverViewModel);
        }
        public object SelectedItem { get; set; }
        public void ImportInfoBase()
        {
            if (SelectedItem is ServerViewModel)
            {
                ServerViewModel server = (ServerViewModel)SelectedItem;
                List<InfoBase> infoBases = _metadataProvider.GetInfoBases(server.Model);
                SelectInfoBaseDialog dialog = new SelectInfoBaseDialog(infoBases);
                _ = dialog.ShowModal();
                if (dialog.Result != null)
                {
                    InfoBaseViewModel selectedItem = (InfoBaseViewModel)dialog.Result;
                    OnInfoBaseSelected(server, selectedItem);
                }
            }
        }
        private void OnInfoBaseSelected(ServerViewModel parent, InfoBaseViewModel child)
        {
            if (parent.Model.InfoBases
                .Where(ib => ib.Database == child.Model.Database)
                .FirstOrDefault() != null) return;

            child.Model.Server = parent.Model;
            _metadataProvider.ImportMetadata(child.Model);
            parent.AddInfoBase(child);
        }


        public void AddProcedure()
        {
            if (SelectedItem == null)
            {
                _ = MessageBox.Show(
                    "Procedure's owner is not selected!",
                    "ONE-C-SHARP",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }
            
            IOneCSharpCodeEditorConsumer consumer = SelectedItem as IOneCSharpCodeEditorConsumer;
            
            if (consumer == null)
            {
                _ = MessageBox.Show(
                    $"{SelectedItem.GetType()} does not implement IOneCSharpCodeEditorConsumer interface !",
                    "ONE-C-SHARP",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            ToolWindowManager.OpenOneCSharpCodeEditor("My caption...", consumer, new Procedure() { Name = "set the name of procedure..." });
        }
    }
}
