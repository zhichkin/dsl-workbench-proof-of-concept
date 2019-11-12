using OneCSharp.Metadata;
using OneCSharp.OQL.Model;
using OneCSharp.OQL.UI.Services;
using System;
using System.Windows;

namespace OneCSharp.VisualStudio.UI
{
    public sealed class DbProcedureViewModel : ViewModelBase, IOneCSharpCodeEditorConsumer
    {
        private DbProcedure _model;
        public MetadataProvider Metadata { get; set; }
        public DbProcedureViewModel(DbProcedure model)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));
            InitializeViewModel();
        }
        public void InitializeViewModel()
        {
            //TODO: collection of parameters
        }
        public NamespaceViewModel Parent { get; set; }
        public DbProcedure Model { get { return _model; } }
        public string Name
        {
            get { return _model.Name; }
            set
            {
                _model.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public void SaveSyntaxNode(IOneCSharpCodeEditor editor, CodeEditorEventArgs args)
        {
            Procedure procedure = args.SyntaxNode as Procedure;
            if (procedure == null)
            {
                _ = MessageBox.Show(
                $"Procedure can not be saved!",
                "ONE-C-SHARP",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
                args.Cancel = true;
                return;
            }
            MessageBoxResult result = MessageBox.Show(
                $"Save procedure \"{procedure.Name}\" ?",
                "ONE-C-SHARP",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Question);
            if (result != MessageBoxResult.OK)
            {
                args.Cancel = true;
                return;
            }

            //TODO: save Procedure to storage (ex. file)
            this.Name = procedure.Name;

            //TODO: unsubscribe when tool window is closed or new procedure editing begins in the same tool window !!!
            //editor.Save -= this.SaveSyntaxNode;

            //TODO: update parameters collection

            _ = MessageBox.Show(
                $"Saving procedure \"{procedure.Name}\"...",
                "Under construction",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
        }
    }
}
