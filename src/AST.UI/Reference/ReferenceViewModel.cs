using OneCSharp.AST.Model;
using OneCSharp.MVVM;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;

namespace OneCSharp.AST.UI
{
    public sealed class ReferenceViewModel : SyntaxNodeViewModel
    {
        private Brush _textColor = Brushes.Black;
        private Brush _defaultColor = Brushes.Black;
        private Brush _temporallyVisibleColor = Brushes.LightGray;
        private Brush _selectedValueBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2B91AF"));
        public ReferenceViewModel(ISyntaxNodeViewModel owner) : base(owner) { }
        public string Presentation
        {
            get { return (Model == null ? $"{{{PropertyBinding}}}" : Model.ToString()); }
        }
        public Brush TextColor
        {
            get { return _textColor; }
            set { _textColor = value; OnPropertyChanged(nameof(TextColor)); }
        }
        public override bool IsTemporallyVisible
        {
            get { return base.IsTemporallyVisible; }
            set
            {
                base.IsTemporallyVisible = value;
                TextColor = (IsTemporallyVisible ? _temporallyVisibleColor : _defaultColor);
            }
        }
        protected override void OnMouseDown(object parameter)
        {
            if (!(parameter is MouseButtonEventArgs args)) return;

            // TODO: delegate this code to the SyntaxTreeController !?
            if (args.ChangedButton == MouseButton.Left || args.ChangedButton == MouseButton.Right)
            {
                args.Handled = true;

                // get parent concept node
                var ancestor = this.Ancestor<ConceptNodeViewModel>();
                if (ancestor == null) return;

                // get scope provider from parent ConceptNode
                // TODO: if (propertyType is SimpleTypeConcept) { } select appropriate scope provider
                IScopeProvider scopeProvider = new TypeConceptScopeProvider();
                IEnumerable<ISyntaxNode> scope = scopeProvider.Scope(ancestor.Model, PropertyBinding);
                if (scope == null || scope.Count() == 0) return;

                // build tree view
                TreeNodeViewModel viewModel = SyntaxNodeExtensions.BuildReferenceSelectorTree(scope);

                // open dialog window
                Visual control = args.Source as Visual;
                PopupWindow dialog = new PopupWindow(control, viewModel);
                _ = dialog.ShowDialog();
                if (dialog.Result == null) { return; }

                // set binded property of the model by selected reference
                ISyntaxNode model = dialog.Result.NodePayload as ISyntaxNode;
                if (model == null) { return; }
                SyntaxTreeManager.SetConceptProperty(ancestor.Model, PropertyBinding, model);

                // reset view model's state
                Model = model;
                TextColor = _selectedValueBrush;
                OnPropertyChanged(nameof(Presentation));
            }
        }
    }
}