using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OneCSharp.Metadata.Services;
using OneCSharp.Metadata.UI;
using OneCSharp.MVVM;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace OneCSharp.Shell
{
    public sealed class ShellViewModel : IShell, INotifyPropertyChanged
    {
        private object _LeftRegion;
        private object _RightRegion;
        private object _StatusBarRegion;
        private AppSettings _settings;
        private readonly IServiceProvider _serviceProvider;
        public ShellViewModel(IServiceProvider serviceProvider, IOptions<AppSettings> options)
        {
            _settings = options.Value;
            _serviceProvider = serviceProvider;
            InitializeViewModel();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ObservableCollection<MenuItemViewModel> MainMenuRegion { get; private set; }
        public ObservableCollection<TabViewModel> Tabs { get; private set; }
        public object LeftRegion
        {
            get { return _LeftRegion; }
            set { _LeftRegion = value; OnPropertyChanged(nameof(LeftRegion)); }
        }
        public object RightRegion
        {
            get { return _RightRegion; }
            set { _RightRegion = value; OnPropertyChanged(nameof(RightRegion)); }
        }
        public object StatusBarRegion
        {
            get { return _StatusBarRegion; }
            set { _StatusBarRegion = value; OnPropertyChanged(nameof(_StatusBarRegion)); }
        }
        private void InitializeViewModel()
        {
            IMetadataProvider metadataProvider = _serviceProvider.GetService<IMetadataProvider>();
            var metadata = new MetadataViewModel(this, metadataProvider);
            LeftRegion = metadata;
            StatusBarRegion = new StatusBarViewModel();

            MainMenuRegion = new ObservableCollection<MenuItemViewModel>();
            //MainMenuRegion.Add(new MenuItemViewModel()
            //{
            //    CommandName = "Add server ...",
            //    CommandAction = metadata.AddServerCommand
            //});

            Tabs = new ObservableCollection<TabViewModel>();
            //Tabs.Add(new TabViewModel()
            //{
            //    Content = new TabViewModel()
            //});
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
    }
}