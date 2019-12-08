using OneCSharp.Metadata.Model;
using OneCSharp.MVVM;
using System;
using System.Collections.ObjectModel;

namespace OneCSharp.Metadata.UI
{
    public sealed class EntityViewModel : ViewModelBase
    {
        private Entity _model;
        public EntityViewModel(Entity model)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));
            InitializeViewModel();
        }
        public void InitializeViewModel()
        {
            if (_model.Properties.Count > 0)
            {
                Properties = new ObservableCollection<PropertyViewModel>();
                foreach (var property in _model.Properties)
                {
                    var vm = new PropertyViewModel((Property)property);
                    vm.Parent = this;
                    Properties.Add(vm);
                    vm.InitializeViewModel();
                }
            }
            if (_model.NestedEntities.Count > 0)
            {
                NestedObjects = new ObservableCollection<EntityViewModel>();
                foreach (var dbo in _model.NestedEntities)
                {
                    var vm = new EntityViewModel((Entity)dbo);
                    vm.Parent = this;
                    NestedObjects.Add(vm);
                    vm.InitializeViewModel();
                }
            }
        }
        public ViewModelBase Parent { get; set; }
        public Entity Model { get { return _model; } }
        public string Name
        {
            get { return _model.Name; }
            set
            {
                _model.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public ObservableCollection<PropertyViewModel> Properties { get; private set; } = new ObservableCollection<PropertyViewModel>();
        public ObservableCollection<EntityViewModel> NestedObjects { get; private set; } = new ObservableCollection<EntityViewModel>();
    }
}
