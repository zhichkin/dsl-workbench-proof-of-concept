using OneCSharp.AST.Model;
using OneCSharp.MVVM;
using System;
using System.Windows.Input;
using System.Windows.Media;

namespace OneCSharp.AST.UI
{
    public sealed class RepeatableOptionViewModel : SyntaxNodeViewModel
    {
        private string _presentation;
        public RepeatableOptionViewModel(RepeatableViewModel owner) : base(owner)
        {
            if (owner == null) throw new ArgumentNullException(nameof(owner));
            _presentation = owner.PropertyBinding;
        }
        public string Presentation
        {
            get { return _presentation; }
            set { _presentation = value; OnPropertyChanged(nameof(Presentation)); }
        }
        protected override void OnMouseDown(object parameter)
        {
            if (!(parameter is MouseButtonEventArgs args)) return;
            if (args.ChangedButton == MouseButton.Right) return;
            args.Handled = true;

            var concept = this.Ancestor<ConceptNodeViewModel>();
            if (concept == null) return;
            if (concept.SyntaxNode == null) return;

            ScriptConcept script;
            if (concept.SyntaxNode is ScriptConcept)
            {
                script = (ScriptConcept)concept.SyntaxNode;
            }
            else
            {
                script = concept.SyntaxNode.Ancestor<ScriptConcept>() as ScriptConcept;
            }
            string propertyName = Owner.PropertyBinding;
            TypeConstraint constraints = SyntaxTreeManager.GetTypeConstraints(script?.Languages, concept.SyntaxNode, propertyName);

            // if selection is obvious then just create the node ...
            if (constraints.Concepts.Count == 1 && constraints.DataTypes.Count == 0)
            {
                CreateRepetableOption(constraints.Concepts[0], concept.SyntaxNode, propertyName);
                return;
            }
            if (constraints.DataTypes.Count == 1 && constraints.Concepts.Count == 0)
            {
                CreateRepetableOption(constraints.DataTypes[0], concept.SyntaxNode, propertyName);
                return;
            }

            // build tree view
            TreeNodeViewModel viewModel = SyntaxNodeSelector.BuildSelectorTree(constraints);

            // open dialog window
            Visual control = args.Source as Visual;
            PopupWindow dialog = new PopupWindow(control, viewModel);
            _ = dialog.ShowDialog();
            if (dialog.Result == null) { return; }

            // get result
            Type selectedType = dialog.Result.NodePayload as Type;
            if (selectedType == null) { return; }

            // create concept option, its view model and add it to syntax tree
            CreateRepetableOption(selectedType, concept.SyntaxNode, propertyName);
        }
        private void CreateRepetableOption(Type optionType, ISyntaxNode parent, string propertyName)
        {
            // TODO: move this code to SyntaxTreeManager
            ISyntaxNode model = SyntaxTreeManager.CreateRepeatableConcept(optionType, parent, propertyName);
            ConceptNodeViewModel node = SyntaxTreeController.Current.CreateSyntaxNode(Owner, model);
            Owner.Add(node);
        }
    }
}