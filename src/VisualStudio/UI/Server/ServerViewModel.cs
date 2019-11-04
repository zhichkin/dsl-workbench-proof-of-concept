using OneCSharp.Metadata;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OneCSharp.VisualStudio.UI
{
    public sealed class ServerViewModel : ViewModelBase
    {
        private DbServer _server;
        private readonly MetadataProvider _metadataProvider;
        public ServerViewModel(DbServer server, MetadataProvider metadataProvider)
        {
            _server = server ?? throw new ArgumentNullException(nameof(server));
            _metadataProvider = metadataProvider ?? throw new ArgumentNullException(nameof(metadataProvider));
            InitializeViewModel();
        }
        private void InitializeViewModel()
        {
            
        }
        public MetadataViewModel Parent { get; set; }
        public DbServer Model { get { return _server; } }
        public string Address
        {
            get { return _server.Address; }
            set
            {
                _server.Address = value;
                OnPropertyChanged(nameof(Address));
            }
        }
        public ObservableCollection<InfoBaseViewModel> InfoBases { get; private set; } = new ObservableCollection<InfoBaseViewModel>();
    }
}
