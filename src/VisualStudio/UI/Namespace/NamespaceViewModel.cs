using OneCSharp.Metadata;
using OneCSharp.OQL.UI.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace OneCSharp.VisualStudio.UI
{
    public sealed class NamespaceViewModel : ViewModelBase, IOneCSharpCodeEditorConsumer
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

        public void SaveSyntaxNode(IOneCSharpCodeEditor editor, CodeEditorEventArgs args)
        {
            _ = MessageBox.Show($"Save syntax node: {args.SyntaxNode}");
        }


        public void AddChild(NamespaceViewModel child)
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
        public void CreateNamespaceViewModel(string name)
        {
            Namespace ns = new Namespace() { Name = name };
            NamespaceViewModel child = new NamespaceViewModel(ns);
            AddChild(child);
        }
    }
}
