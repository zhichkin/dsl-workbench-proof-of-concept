using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OneCSharp.MVVM;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;

namespace OneCSharp.Shell
{
    public sealed class ShellViewModel : IShell, INotifyPropertyChanged
    {
        private TreeNodeViewModel _LeftRegion = new TreeNodeViewModel();
        private object _RightRegion;
        private string _StatusBarRegion = string.Empty;
        private AppSettings _settings;
        private readonly IServiceProvider _serviceProvider;

        private string _appCatalogPath;
        private string _modulesCatalogPath;
        public string AppCatalogPath { get { return _appCatalogPath; } }
        public string ModulesCatalogPath { get { return _modulesCatalogPath; } }
        public IService GetService<IService>()
        {
            return _serviceProvider.GetService<IService>();
        }
        public ShellViewModel(IServiceProvider serviceProvider, IOptions<AppSettings> options)
        {
            _settings = options.Value;
            _serviceProvider = serviceProvider;

            SetupCatalogPath();
            InitializeViewModel();
        }
        private void SetupCatalogPath()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            _appCatalogPath = Path.GetDirectoryName(asm.Location);
            _modulesCatalogPath = Path.Combine(_appCatalogPath, "Modules");
            if (!Directory.Exists(_modulesCatalogPath))
            {
                _ = Directory.CreateDirectory(_modulesCatalogPath);
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ObservableCollection<MenuItemViewModel> MainMenuRegion { get; } = new ObservableCollection<MenuItemViewModel>();
        public ObservableCollection<TabViewModel> Tabs { get; } = new ObservableCollection<TabViewModel>();
        public TreeNodeViewModel LeftRegion
        {
            get { return _LeftRegion; }
            set { _LeftRegion = value; OnPropertyChanged(nameof(LeftRegion)); }
        }
        public object RightRegion
        {
            get { return _RightRegion; }
            set { _RightRegion = value; OnPropertyChanged(nameof(RightRegion)); }
        }
        public string StatusBarRegion
        {
            get { return _StatusBarRegion; }
            set { _StatusBarRegion = value; OnPropertyChanged(nameof(StatusBarRegion)); }
        }
        private void InitializeViewModel()
        {
            StatusBarRegion = "Welcome to 1C# ! =)";

            // Plug in main AST module
            IModule module1 = new OneCSharp.AST.Module.Module();
            module1.Initialize(this);

            // Plug in SQL module (TODO: move it to extension modules in future)
            IModule module2 = new OneCSharp.SQL.UI.Module();
            module2.Initialize(this);

            InitializeExtensionModules();
        }
        private TabViewModel _selectedTab;
        public TabViewModel SelectedTab
        {
            get { return _selectedTab; }
            set { _selectedTab = value; OnPropertyChanged(nameof(SelectedTab)); }
        }
        public object SelectedTabViewModel
        {
            get
            {
                if (SelectedTab == null) return null;
                if (!(SelectedTab.Content is UserControl content)) return null;
                return content.DataContext;
            }
        }
        public void AddTabItem(string header, object content)
        {
            TabViewModel tab = new TabViewModel(this)
            {
                Header = header,
                Content = content
            };
            Tabs.Add(tab);
            SelectedTab = tab;
        }
        public void RemoveTabItem(TabViewModel tab)
        {
            if (tab != null)
            {
                Tabs.Remove(tab);
                if (Tabs.Count > 0)
                {
                    SelectedTab = Tabs[0];
                }
            }
        }
        public void AddMenuItem(MenuItemViewModel menuItem)
        {
            MainMenuRegion.Add(menuItem);
        }
        public void AddTreeNode(TreeNodeViewModel treeNode)
        {
            LeftRegion.TreeNodes.Add(treeNode);
        }
        public void ShowStatusBarMessage(string message)
        {
            StatusBarRegion = message;
            //App.Current.MainWindow.Dispatcher.Invoke(() => { StatusBarRegion = message; });
        }
        private void InitializeExtensionModules()
        {
            foreach (string directory in Directory.GetDirectories(ModulesCatalogPath))
            {
                foreach (string file in Directory.GetFiles(directory, "*.dll"))
                {
                    Assembly assembly = Assembly.LoadFrom(file);
                    foreach (Type type in assembly.GetTypes())
                    {
                        if (type.GetInterfaces().Where(i => i == typeof(IModule)).Count() > 0)
                        {
                            IModule module = (IModule)Activator.CreateInstance(type);
                            module.Initialize(this);
                        }
                    }
                }
            }
        }
    }
}