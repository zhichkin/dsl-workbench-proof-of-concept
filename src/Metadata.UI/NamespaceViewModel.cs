using OneCSharp.Metadata.Model;
using OneCSharp.MVVM;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace OneCSharp.Metadata.UI
{
    public sealed class NamespaceViewModel : ViewModelBase
    {
        private readonly IShell _shell;
        private Namespace _model;
        public NamespaceViewModel(IShell shell, Namespace model)
        {
            _shell = shell ?? throw new ArgumentNullException(nameof(shell));
            _model = model ?? throw new ArgumentNullException(nameof(model));
            InitializeViewModel();
        }
        public void InitializeViewModel()
        {
            if (_model.Namespaces.Count > 0)
            {
                Namespaces = new ObservableCollection<NamespaceViewModel>();
                foreach (var ns in _model.Namespaces)
                {
                    var vm = new NamespaceViewModel(_shell, (Namespace)ns);
                    Namespaces.Add(vm);
                    vm.Parent = this;
                    vm.InitializeViewModel();
                }
            }
            if (_model.Entities.Count > 0)
            {
                DbObjects = new ObservableCollection<EntityViewModel>();
                foreach (var dbo in _model.Entities)
                {
                    var vm = new EntityViewModel((Entity)dbo);
                    vm.Parent = this;
                    DbObjects.Add(vm);
                    vm.InitializeViewModel();
                }
            }
            if (_model.Requests.Count > 0)
            {
                DbProcedures = new ObservableCollection<RequestViewModel>();
                foreach (var prc in _model.Requests)
                {
                    var vm = new RequestViewModel(_shell, (Request)prc);
                    vm.Parent = this;
                    DbProcedures.Add(vm);
                    vm.InitializeViewModel();
                }
            }
        }
        public ViewModelBase Parent { get; set; }
        public Namespace Model { get { return _model; } }
        public string Name
        {
            get { return _model.Name; }
            set
            {
                _model.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public ObservableCollection<EntityViewModel> DbObjects { get; private set; } = new ObservableCollection<EntityViewModel>();
        public ObservableCollection<NamespaceViewModel> Namespaces { get; private set; } = new ObservableCollection<NamespaceViewModel>();
        public ObservableCollection<RequestViewModel> DbProcedures { get; private set; } = new ObservableCollection<RequestViewModel>();


        public void AddChild(ViewModelBase child)
        {
            if (child is NamespaceViewModel)
            {
                AddChildNamespace((NamespaceViewModel)child);
            }
            else if (child is RequestViewModel)
            {
                AddChildDbProcedure((RequestViewModel)child);
            }
        }
        private void AddChildNamespace(NamespaceViewModel child)
        {
            if (_model.Namespaces
                .Where(i => i.Name == child.Name)
                .FirstOrDefault() != null) return;

            child.Model.Domain = _model.Domain;
            child.Parent = this;
            _model.Namespaces.Add(child.Model);
            child.InitializeViewModel();
            Namespaces.Add(child);
        }
        private void AddChildDbProcedure(RequestViewModel child)
        {
            if (_model.Requests
                .Where(i => i.Name == child.Name)
                .FirstOrDefault() != null) return;

            child.Model.Parent = _model;
            child.Parent = this;
            _model.Requests.Add(child.Model);
            child.InitializeViewModel();
            DbProcedures.Add(child);
        }
        public void CreateNamespaceViewModel(string name)
        {
            Namespace ns = new Namespace() { Name = name };
            NamespaceViewModel child = new NamespaceViewModel(_shell, ns);
            AddChild(child);
        }
        public void CreateProcedureViewModel(string name)
        {
            Request procedure = new Request() { Name = name };
            RequestViewModel child = new RequestViewModel(_shell, procedure);
            AddChild(child);
        }
    }
}
