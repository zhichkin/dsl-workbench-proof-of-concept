using OneCSharp.Metadata;
using System;
using System.Collections.ObjectModel;
using System.Linq;

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
        public ObservableCollection<NamespaceViewModel> Namespaces { get; private set; } = new ObservableCollection<NamespaceViewModel>();

        public void AddChild(NamespaceViewModel child)
        {
            if (_model.Namespaces
                .Where(i => i.Name == child.Name)
                .FirstOrDefault() != null) return;

            child.Model.InfoBase = _model;
            child.Parent = this;
            _model.Namespaces.Add(child.Model);
            child.InitializeViewModel();
            Namespaces.Add(child);
        }
        public void CreateNamespaceViewModel(string name)
        {
            Namespace ns = new Namespace() { Name = name };
            NamespaceViewModel child = new NamespaceViewModel(ns);
            AddChild(child);
        }
    }
}
