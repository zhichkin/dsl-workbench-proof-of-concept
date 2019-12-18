using OneCSharp.AST.Model;
using OneCSharp.Core;
using OneCSharp.Metadata.Services;
using OneCSharp.MVVM;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace OneCSharp.AST.UI
{
    public sealed class DomainLanguageViewModel : ViewModelBase
    {
        private readonly IShell _shell;
        private readonly IMetadataProvider _metadataProvider;
        private readonly Language _model;
        public DomainLanguageViewModel(Language model, IShell shell, IMetadataProvider metadataProvider)
        {
            _shell = shell ?? throw new ArgumentNullException(nameof(shell));
            _model = model ?? throw new ArgumentNullException(nameof(model));
            _metadataProvider = metadataProvider ?? throw new ArgumentNullException(nameof(metadataProvider));
            InitializeViewModel();
        }
        private void InitializeViewModel()
        {
            AddNamespaceCommand = new RelayCommand(AddNamespace);

            if (_model.Namespaces != null)
            {
                foreach (var ns in _model.Namespaces)
                {
                    LanguageNamespaceViewModel vm = new LanguageNamespaceViewModel(ns, _shell, _metadataProvider);
                    Namespaces.Add(vm);
                }
            }
        }
        public ICommand AddNamespaceCommand { get; private set; }
        public string Name
        {
            get { return _model.Name; }
            set { _model.Name = value; OnPropertyChanged(nameof(Name)); }
        }
        public ObservableCollection<ViewModelBase> Namespaces { get; } = new ObservableCollection<ViewModelBase>();
        public void AddNamespace(object parameter)
        {
            InputStringDialog dialog = new InputStringDialog();
            _ = dialog.ShowDialog();
            if (dialog.Result == null) { return; }

            Namespace model = new Namespace()
            {
                Name = (string)dialog.Result
            };
            LanguageNamespaceViewModel vm = new LanguageNamespaceViewModel(model, _shell, _metadataProvider);
            Namespaces.Add(vm);
        }
    }
}
