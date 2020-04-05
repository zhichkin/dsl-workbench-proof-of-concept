using OneCSharp.Integrator.Services;
using OneCSharp.MVVM;
using System;
using System.Reflection;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace OneCSharp.Integrator.Module
{
    public sealed class ContractsController : IController
    {
        #region "Private fileds"
        private const string CONTRACTS_NAME = "Contracts";
        private const string CONTRACTS_TOOLTIP = "Data contracts";
        private const string CONTRACTS_ICON_PATH = "pack://application:,,,/OneCSharp.Integrator.Module;component/images/Catalog.png";
        private const string CONTRACTS_ADD_ICON_PATH = "pack://application:,,,/OneCSharp.Integrator.Module;component/images/AddCatalog.png";
        private const string JSON_SCRIPT_ICON_PATH = "pack://application:,,,/OneCSharp.Integrator.Module;component/images/JsonScript.png";
        private readonly BitmapImage CONTRACTS_ICON = new BitmapImage(new Uri(CONTRACTS_ICON_PATH));
        private readonly BitmapImage CONTRACTS_ADD_ICON = new BitmapImage(new Uri(CONTRACTS_ADD_ICON_PATH));
        private readonly BitmapImage JSON_SCRIPT_ICON = new BitmapImage(new Uri(JSON_SCRIPT_ICON_PATH));
        #endregion
        private IModule Module { get; set; }
        public ContractsController(IModule module)
        {
            Module = module;
        }
        public void BuildTreeNode(object model, out TreeNodeViewModel treeNode)
        {
            throw new NotImplementedException();
        }
        public void AttachTreeNodes(TreeNodeViewModel parentNode)
        {
            TreeNodeViewModel mainNode = new TreeNodeViewModel()
            {
                IsExpanded = false,
                NodeIcon = CONTRACTS_ICON,
                NodeText = CONTRACTS_NAME,
                NodeToolTip = CONTRACTS_TOOLTIP,
                NodePayload = null
            };
            mainNode.ContextMenuItems.Add(new MenuItemViewModel()
            {
                MenuItemHeader = "Add JSON contract",
                MenuItemIcon = CONTRACTS_ADD_ICON,
                MenuItemCommand = new RelayCommand(AddJsonContractCommand),
                MenuItemPayload = mainNode
            });
            parentNode.TreeNodes.Add(mainNode);

            AttachContracts(mainNode);
        }
        private void AttachContracts(TreeNodeViewModel parentNode)
        {
            var provider = Module.GetProvider<ContractsProvider>();
            foreach (Assembly contract in provider.GetContracts())
            {
                AttachContractsTreeNodes(contract, parentNode);
            }
        }
        private void AttachContractsTreeNodes(Assembly contract, TreeNodeViewModel parentNode)
        {
            TreeNodeViewModel contractNode = new TreeNodeViewModel()
            {
                IsExpanded = false,
                NodeIcon = CONTRACTS_ICON,
                NodeText = $"{contract.GetName().Name} ({contract.GetName().Version.ToString()})",
                NodeToolTip = null,
                NodePayload = contract
            };
            foreach (Type type in contract.GetTypes())
            {
                TreeNodeViewModel typeNode = new TreeNodeViewModel()
                {
                    IsExpanded = false,
                    NodeIcon = JSON_SCRIPT_ICON,
                    NodeText = type.Name,
                    NodeToolTip = type.FullName,
                    NodePayload = type
                };
                typeNode.ContextMenuItems.Add(new MenuItemViewModel()
                {
                    MenuItemHeader = "View JSON object",
                    MenuItemIcon = JSON_SCRIPT_ICON,
                    MenuItemCommand = new RelayCommand(ViewDummyObjectCommand),
                    MenuItemPayload = typeNode
                });
                contractNode.TreeNodes.Add(typeNode);
            }
            parentNode.TreeNodes.Add(contractNode);
        }
        private void AddJsonContractCommand(object parameter)
        {
            MessageBox.Show("Under construction.", "1C# Integrator", MessageBoxButton.OK, MessageBoxImage.Information);

            //if (!(parameter is TreeNodeViewModel parentNode)) return;

            //OpenFileDialog dialog = new OpenFileDialog()
            //{
            //    Multiselect = false,
            //    InitialDirectory = Module.ContractsCatalogPath,
            //    Filter = "Contract file (.dll)|*.dll"
            //};
            //var result = dialog.ShowDialog();
            //if (result != true) return;

            //Assembly contract = Assembly.LoadFrom(dialog.FileName);
            //AttachContractsTreeNodes(contract, parentNode);
        }
        private void ViewDummyObjectCommand(object parameter)
        {
            if (!(parameter is TreeNodeViewModel typeNode)) return;
            if (typeNode.NodePayload == null) return;
            Type valueType = (Type)typeNode.NodePayload;
            object value = Activator.CreateInstance(valueType);
            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                WriteIndented = true
            };
            string json = JsonSerializer.Serialize(value, valueType, options);
            TextBox textBox = new TextBox
            {
                Text = json
            };
            Module.Shell.AddTabItem(valueType.FullName, textBox);
        }
    }
}