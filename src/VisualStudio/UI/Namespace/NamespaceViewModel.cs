using OneCSharp.Metadata;
using OneCSharp.OQL.UI.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace OneCSharp.VisualStudio.UI
{
    public sealed class NamespaceViewModel : ViewModelBase
    {
        private Namespace _model;
        public NamespaceViewModel(Namespace model)
        {
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
                    var vm = new NamespaceViewModel(ns);
                    Namespaces.Add(vm);
                    vm.Parent = this;
                    vm.InitializeViewModel();
                }
            }
            if (_model.DbObjects.Count > 0)
            {
                DbObjects = new ObservableCollection<DbObjectViewModel>();
                foreach (var dbo in _model.DbObjects)
                {
                    var vm = new DbObjectViewModel(dbo);
                    vm.Parent = this;
                    DbObjects.Add(vm);
                    vm.InitializeViewModel();
                }
            }
            if (_model.DbProcedures.Count > 0)
            {
                DbProcedures = new ObservableCollection<DbProcedureViewModel>();
                foreach (var prc in _model.DbProcedures)
                {
                    var vm = new DbProcedureViewModel(prc);
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
        public ObservableCollection<DbObjectViewModel> DbObjects { get; private set; } = new ObservableCollection<DbObjectViewModel>();
        public ObservableCollection<NamespaceViewModel> Namespaces { get; private set; } = new ObservableCollection<NamespaceViewModel>();
        public ObservableCollection<DbProcedureViewModel> DbProcedures { get; private set; } = new ObservableCollection<DbProcedureViewModel>();


        public void AddChild(ViewModelBase child)
        {
            if (child is NamespaceViewModel)
            {
                AddChildNamespace((NamespaceViewModel)child);
            }
            else if (child is DbProcedureViewModel)
            {
                AddChildDbProcedure((DbProcedureViewModel)child);
            }
        }
        private void AddChildNamespace(NamespaceViewModel child)
        {
            if (_model.Namespaces
                .Where(i => i.Name == child.Name)
                .FirstOrDefault() != null) return;

            child.Model.InfoBase = _model.InfoBase;
            child.Parent = this;
            _model.Namespaces.Add(child.Model);
            child.InitializeViewModel();
            Namespaces.Add(child);
        }
        private void AddChildDbProcedure(DbProcedureViewModel child)
        {
            if (_model.DbProcedures
                .Where(i => i.Name == child.Name)
                .FirstOrDefault() != null) return;

            child.Model.Parent = _model;
            child.Parent = this;
            _model.DbProcedures.Add(child.Model);
            child.InitializeViewModel();
            DbProcedures.Add(child);
        }
        public void CreateNamespaceViewModel(string name)
        {
            Namespace ns = new Namespace() { Name = name };
            NamespaceViewModel child = new NamespaceViewModel(ns);
            AddChild(child);
        }
        public void CreateProcedureViewModel(string name)
        {
            DbProcedure procedure = new DbProcedure() { Name = name };
            DbProcedureViewModel child = new DbProcedureViewModel(procedure);
            AddChild(child);
        }
    }
}
