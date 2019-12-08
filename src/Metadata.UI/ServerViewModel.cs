using OneCSharp.Metadata.Model;
using OneCSharp.Metadata.Services;
using OneCSharp.MVVM;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace OneCSharp.Metadata.UI
{
    public sealed class ServerViewModel : ViewModelBase
    {
        private readonly IShell _shell;
        private Server _model;
        private readonly IMetadataProvider _metadataProvider;
        public ServerViewModel(IShell shell, Server model, IMetadataProvider metadataProvider)
        {
            _shell = shell ?? throw new ArgumentNullException(nameof(shell));
            _model = model ?? throw new ArgumentNullException(nameof(model));
            _metadataProvider = metadataProvider ?? throw new ArgumentNullException(nameof(metadataProvider));
            InitializeViewModel();
        }
        public void InitializeViewModel()
        {
            if (_model.Domains.Count > 0)
            {
                Nodes = new ObservableCollection<DomainViewModel>();
                foreach (var node in _model.Domains)
                {
                    var vm = new DomainViewModel(_shell, (Domain)node);
                    vm.Parent = this;
                    Nodes.Add(vm);
                    vm.InitializeViewModel();
                }
            }
        }
        public MetadataViewModel Parent { get; set; }
        public Server Model { get { return _model; } }
        public string Address
        {
            get { return _model.Address; }
            set
            {
                _model.Address = value;
                OnPropertyChanged(nameof(Address));
            }
        }
        public ObservableCollection<DomainViewModel> Nodes { get; private set; } = new ObservableCollection<DomainViewModel>();
        public void AddInfoBase(DomainViewModel child)
        {
            if (_model.Domains
                .Where(ib => ib.Database == child.Model.Database)
                .FirstOrDefault() != null) return;

            child.Model.Server = _model;
            child.Parent = this;
            _model.Domains.Add(child.Model);
            child.InitializeViewModel();
            Nodes.Add(child);
        }
        public void CreateInfoBase(string address)
        {
            if (_model.Domains
                .Where(ib => ib.Database == address)
                .FirstOrDefault() != null) return;

            Domain domain = new Domain()
            {
                Server = _model,
                Name = address,
                Database = address
            };
            DomainViewModel child = new DomainViewModel(_shell, domain);
            AddInfoBase(child);
        }
    }
}
