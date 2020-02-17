using OneCSharp.AST.Model;
using OneCSharp.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;

namespace OneCSharp.AST.UI
{
    public sealed class SelectorViewModel : SyntaxNodeViewModel
    {
        //private Brush _textColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A31515"));
        //private Brush _defaultColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A31515"));
        //private Brush _temporallyVisibleColor = Brushes.LightGray;
        //private Brush _selectedValueBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2B91AF"));
        public SelectorViewModel(ISyntaxNodeViewModel owner) : base(owner) { }
        public string Presentation
        {
            get { return (Model == null ? $"{{{PropertyBinding}}}" : Model.ToString()); }
        }
        protected override void OnMouseDown(object parameter)
        {
            if (!(parameter is MouseButtonEventArgs args)) return;
            if (args.ChangedButton == MouseButton.Right) return;
            args.Handled = true;

            // get parent concept node
            var ancestor = this.Ancestor<ConceptNodeViewModel>();
            if (ancestor == null) return;

            // get type constraints of the property
            TypeConstraint constraints = SyntaxTreeManager.GetTypeConstraints(ancestor.Model, PropertyBinding);

            // build tree view
            TreeNodeViewModel viewModel = SyntaxNodeSelector.BuildSelectorTree(constraints);

            // open dialog window
            Visual control = args.Source as Visual;
            PopupWindow dialog = new PopupWindow(control, viewModel);
            _ = dialog.ShowDialog();
            if (dialog.Result == null) { return; }

            Type selectedType = dialog.Result.NodePayload as Type;
            if (selectedType == null) { return; }

            ISyntaxNode reference = SelectSyntaxNodeReference(selectedType, ancestor.Model, PropertyBinding, control);
            if (reference == null) { return; }

            // set binded property of the model by selected reference
            SyntaxTreeManager.SetConceptProperty(ancestor.Model, PropertyBinding, reference);

            // reset view model's state
            Model = reference;
            OnPropertyChanged(nameof(Presentation));
        }
        private ISyntaxNode SelectSyntaxNodeReference(Type childConcept, ISyntaxNode parentConcept, string propertyName, Visual control)
        {
            // get scope provider
            IScopeProvider scopeProvider = SyntaxTreeManager.GetScopeProvider(childConcept);
            if (scopeProvider == null) { return null; }

            // get references in the scope
            IEnumerable<ISyntaxNode> scope = scopeProvider.Scope(parentConcept, PropertyBinding);
            if (scope == null || scope.Count() == 0) { return null; }

            // build tree view
            TreeNodeViewModel viewModel = SyntaxNodeExtensions.BuildReferenceSelectorTree(scope);

            // open dialog window
            PopupWindow dialog = new PopupWindow(control, viewModel);
            _ = dialog.ShowDialog();
            if (dialog.Result == null) { return null; }

            // return selected reference
            return (dialog.Result.NodePayload as ISyntaxNode);
        }
    }
}