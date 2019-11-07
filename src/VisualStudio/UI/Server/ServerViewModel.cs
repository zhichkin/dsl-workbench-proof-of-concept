using OneCSharp.Metadata;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace OneCSharp.VisualStudio.UI
{
    public sealed class ServerViewModel : ViewModelBase
    {
        private DbServer _model;
        private readonly MetadataProvider _metadataProvider;
        public ServerViewModel(DbServer model, MetadataProvider metadataProvider)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));
            _metadataProvider = metadataProvider ?? throw new ArgumentNullException(nameof(metadataProvider));
            InitializeViewModel();
        }
        public void InitializeViewModel()
        {
            if (_model.InfoBases.Count > 0)
            {
                InfoBases = new ObservableCollection<InfoBaseViewModel>();
                foreach (var ns in _model.InfoBases)
                {
                    var vm = new InfoBaseViewModel(ns);
                    vm.Parent = this;
                    InfoBases.Add(vm);
                    vm.InitializeViewModel();
                }
            }
        }
        public MetadataViewModel Parent { get; set; }
        public DbServer Model { get { return _model; } }
        public string Address
        {
            get { return _model.Address; }
            set
            {
                _model.Address = value;
                OnPropertyChanged(nameof(Address));
            }
        }
        public ObservableCollection<InfoBaseViewModel> InfoBases { get; private set; } = new ObservableCollection<InfoBaseViewModel>();
        public void AddInfoBase(InfoBaseViewModel child)
        {
            if (_model.InfoBases
                .Where(ib => ib.Database == child.Model.Database)
                .FirstOrDefault() != null) return;

            child.Model.Server = _model;
            child.Parent = this;
            _model.InfoBases.Add(child.Model);
            child.InitializeViewModel();
            InfoBases.Add(child);
        }
        public void CreateInfoBase(string address)
        {
            if (_model.InfoBases
                .Where(ib => ib.Database == address)
                .FirstOrDefault() != null) return;

            InfoBase infoBase = new InfoBase()
            {
                Server = _model,
                Name = address,
                Database = address
            };
            InfoBaseViewModel child = new InfoBaseViewModel(infoBase);
            AddInfoBase(child);
        }
    }
}
