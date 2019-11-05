using OneCSharp.Metadata;
using System;
using System.Collections.ObjectModel;

namespace OneCSharp.VisualStudio.UI
{
    public sealed class InfoBaseViewModel : ViewModelBase
    {
        private InfoBase _model;
        public InfoBaseViewModel(InfoBase model)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));
            InitializeViewModel();
        }
        public InfoBase Model { get { return _model; } }
        public ServerViewModel Parent { get; set; }
        public void InitializeViewModel()
        {
            if (_model.Namespaces.Count > 0)
            {
                Namespaces = new ObservableCollection<NamespaceViewModel>();
                foreach (var ns in _model.Namespaces)
                {
                    var vm = new NamespaceViewModel(ns);
                    vm.Parent = this;
                    Namespaces.Add(vm);
                    vm.InitializeViewModel();
                }
            }
        }
        public string Database
        {
            get { return _model.Database; }
            set
            {
                _model.Database = value;
                OnPropertyChanged(nameof(Database));
            }
        }
        public ObservableCollection<NamespaceViewModel> Namespaces { get; private set; }
    }
}
