using OneCSharp.DSL.Model;
using OneCSharp.DSL.Services;
using OneCSharp.DSL.UI.Services;
using OneCSharp.Metadata.Model;
using OneCSharp.Metadata.Services;
using OneCSharp.MVVM;
using System;
using System.Windows;
using System.Windows.Controls;

namespace OneCSharp.Metadata.UI
{
    public sealed class RequestViewModel : ViewModelBase, IOneCSharpCodeEditorConsumer
    {
        private readonly IShell _shell;
        private Request _model;
        public IMetadataProvider Metadata { get; set; }
        public RequestViewModel(IShell shell, Request model)
        {
            _shell = shell ?? throw new ArgumentNullException(nameof(shell));
            _model = model ?? throw new ArgumentNullException(nameof(model));
            InitializeViewModel();
        }
        public void InitializeViewModel()
        {
            //TODO: collection of parameters
        }
        public NamespaceViewModel Parent { get; set; }
        public Request Model { get { return _model; } }
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

            try
            {
                SaveProcedure(procedure);
                this.Name = procedure.Name;

                _ = MessageBox.Show(
                    $"Procedure \"{procedure.Name}\" has been successfully saved.",
                    "ONE-C-SHARP", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception error)
            {
                _ = MessageBox.Show(
                    $"Error saving procedure \"{procedure.Name}\":" + Environment.NewLine + error.Message,
                    "ONE-C-SHARP", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            //TODO: unsubscribe when tool window is closed or new procedure editing begins in the same tool window !!!
            //editor.Save -= this.SaveSyntaxNode;
        }
        private void SaveProcedure(Procedure procedure)
        {
            if (_shell == null) return;
            IOneCSharpJsonSerializer serializer = new OneCSharpJsonSerializer();
            string json = serializer.ToJson(procedure);
            _shell.AddTabItem($"{procedure.Name}.json", UIServices.CreateTextBox(json));
        }
    }
}
