using OneCSharp.Metadata;
using System;

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
        private void InitializeViewModel()
        {

        }
        public InfoBaseViewModel Parent { get; set; }
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
    }
}
