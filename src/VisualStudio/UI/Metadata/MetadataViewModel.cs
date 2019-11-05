using Microsoft.VisualStudio;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using OneCSharp.Metadata;
using OneCSharp.OQL.Model;
using OneCSharp.OQL.UI;
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
                MessageBoxDialog msgBox = new MessageBoxDialog()
                {
                    Title = "Error",
                    Content = new TextBlock()
                    {
                        Text = "Unable to connect server!",
                        Margin = new Thickness(20),
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center
                    }
                };
                _ = msgBox.ShowModal();
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
            OpenCodeEditorWindow();
        }
        private void OpenCodeEditorWindow()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            IVsUIShell vsUIShell = (IVsUIShell)Package.GetGlobalService(typeof(SVsUIShell));
            Guid guid = typeof(OneCSharpToolWindow).GUID;
            IVsWindowFrame windowFrame = null;
            for (uint i = 0; i < 10; i++)
            {
                int result = vsUIShell.FindToolWindowEx((uint)__VSFINDTOOLWIN.FTW_fFrameOnly, ref guid, i, out windowFrame);
                if (result == VSConstants.S_OK)
                {
                    continue;
                }
                else
                {
                    _ = vsUIShell.FindToolWindowEx((uint)__VSFINDTOOLWIN.FTW_fForceCreate, ref guid, i, out windowFrame);
                    break;
                }
            }
            OneCSharpToolWindow codeEditorWindow = windowFrame as OneCSharpToolWindow;
            if (codeEditorWindow != null)
            {
                var dataContext = new ProcedureViewModel(new Procedure());
                codeEditorWindow.SetDataContext(dataContext);
            }
            ErrorHandler.ThrowOnFailure(windowFrame.Show());
        }
    }
}
