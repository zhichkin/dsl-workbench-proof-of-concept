using OneCSharp.Metadata;
using System;
using System.Collections.ObjectModel;

namespace OneCSharp.VisualStudio.UI
{
    public sealed class DbPropertyViewModel : ViewModelBase
    {
        private DbProperty _model;
        public DbPropertyViewModel(DbProperty model)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));
            InitializeViewModel();
        }
        public void InitializeViewModel()
        {

        }
        public DbObjectViewModel Parent { get; set; }
        public DbProperty Model { get { return _model; } }
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
