using OneCSharp.AST.Model;
using System;
using System.Collections;
using System.Reflection;
using System.Windows.Input;

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
            if (concept.Model == null) return;

            string propertyName = Owner.PropertyBinding;
            TypeConstraint constraints = SyntaxTreeManager.GetTypeConstraints(concept.Model, propertyName);
            // TODO: use selector to choose type of node
            if (constraints.Concepts.Count == 1)
            {
                ISyntaxNode model = SyntaxTreeManager.CreateRepeatableConcept(constraints.Concepts[0], concept.Model, propertyName);
                SyntaxTreeController controller = new SyntaxTreeController();
                ConceptNodeViewModel node = controller.CreateSyntaxNode(Owner, model);
                Owner.Add(node);
            }
        }
    }
}