using OneCSharp.Metadata.Model;
using OneCSharp.MVVM;
using System;

namespace OneCSharp.Metadata.UI
{
    public sealed class PropertyViewModel : ViewModelBase
    {
        private Property _model;
        public PropertyViewModel(Property model)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));
            InitializeViewModel();
        }
        public void InitializeViewModel()
        {

        }
        public EntityViewModel Parent { get; set; }
        public Property Model { get { return _model; } }
        public string Name
        {
            get { return _model.Name; }
            set
            {
                _model.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        //public ObservableCollection<InfoBaseViewModel> Fields { get; private set; } = new ObservableCollection<InfoBaseViewModel>();
    }
}
