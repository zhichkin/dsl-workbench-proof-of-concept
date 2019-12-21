using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OneCSharp.MVVM;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Reflection;

namespace OneCSharp.Shell
{
    public sealed class ShellViewModel : IShell, INotifyPropertyChanged
    {
        private TreeNodeViewModel _LeftRegion = new TreeNodeViewModel();
        private object _RightRegion;
        private StatusBarViewModel _StatusBarRegion = new StatusBarViewModel();
        private AppSettings _settings;
        private readonly IServiceProvider _serviceProvider;

        private string _catalogPath;
        public string AppCatalogPath { get { return _catalogPath; } }
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
            _catalogPath = Path.GetDirectoryName(asm.Location);
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
        public StatusBarViewModel StatusBarRegion
        {
            get { return _StatusBarRegion; }
            set { _StatusBarRegion = value; OnPropertyChanged(nameof(_StatusBarRegion)); }
        }
        private void InitializeViewModel()
        {
            //IMetadataProvider metadataProvider = _serviceProvider.GetService<IMetadataProvider>();
            //var metadata = new MetadataViewModel(this, metadataProvider);
            //LeftRegion = metadata;

            // Plug in Metadata module
            IModule module1 = new OneCSharp.Metadata.UI.Module();
            module1.Initialize(this);

            // Plug in AST module
            IModule module2 = new OneCSharp.AST.UI.Module();
            module2.Initialize(this);
        }
        private TabViewModel _selectedTab;
        public TabViewModel SelectedTab
        {
            get { return _selectedTab; }
            set { _selectedTab = value; OnPropertyChanged(nameof(SelectedTab)); }
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
    }
}