using OneCSharp.Metadata;
using System;
using System.Collections.ObjectModel;

namespace OneCSharp.VisualStudio.UI
{
    public sealed class DbObjectViewModel : ViewModelBase
    {
        private DbObject _model;
        public DbObjectViewModel(DbObject model)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));
            InitializeViewModel();
        }
        public void InitializeViewModel()
        {
            if (_model.Properties.Count > 0)
            {
                Properties = new ObservableCollection<DbPropertyViewModel>();
                foreach (var property in _model.Properties)
                {
                    var vm = new DbPropertyViewModel(property);
                    vm.Parent = this;
                    Properties.Add(vm);
                    vm.InitializeViewModel();
                }
            }
            if (_model.NestedObjects.Count > 0)
            {
                NestedObjects = new ObservableCollection<DbObjectViewModel>();
                foreach (var dbo in _model.NestedObjects)
                {
                    var vm = new DbObjectViewModel(dbo);
                    vm.Parent = this;
                    NestedObjects.Add(vm);
                    vm.InitializeViewModel();
                }
            }
        }
        public ViewModelBase Parent { get; set; }
        public DbObject Model { get { return _model; } }
        public string Name
        {
            get { return _model.Name; }
            set
            {
                _model.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public ObservableCollection<DbPropertyViewModel> Properties { get; private set; } = new ObservableCollection<DbPropertyViewModel>();
        public ObservableCollection<DbObjectViewModel> NestedObjects { get; private set; } = new ObservableCollection<DbObjectViewModel>();
    }
}
