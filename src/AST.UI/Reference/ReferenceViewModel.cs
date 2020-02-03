using OneCSharp.AST.Model;
using OneCSharp.MVVM;
using System;
using System.Collections;
using System.Reflection;
using System.Windows.Input;
using System.Windows.Media;

namespace OneCSharp.AST.UI
{
    public sealed class ReferenceViewModel : SyntaxNodeViewModel
    {
        private Brush _textColor = Brushes.Black;
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
        protected override void OnMouseDown(object parameter)
        {
            if (!(parameter is MouseButtonEventArgs args)) return;

            // TODO: delegate this code to the SyntaxTreeController !?
            if (args.ChangedButton == MouseButton.Left || args.ChangedButton == MouseButton.Right)
            {
                args.Handled = true;

                var ancestor = this.Ancestor<ConceptNodeViewModel>();
                if (ancestor == null) return;

                // get scope provider from parent ConceptNode
                IScopeProvider scopeProvider = (ancestor.Owner == null)
                    ? ancestor.Model as IScopeProvider
                    : ancestor.Owner.Model as IScopeProvider;
                if (scopeProvider == null) return;

                // get type to reference: it can be simple type or ConceptNode type
                Type scopeType;
                PropertyInfo property = ancestor.Model.GetPropertyInfo(PropertyBinding);
                if (property.IsOptional())
                {
                    property = property.PropertyType.GetProperty("Value");
                }
                // TODO: check for TypeConstraint and SimpleTypeConstraint attributes of the property !
                if (property.IsRepeatable())
                {
                    scopeType = property.GetRepeatableTypes()[0];
                }
                else
                {
                    scopeType = property.PropertyType;
                }
                // get possible references in the current scope
                var scope = scopeProvider.Scope(scopeType);
                if (scope == null) return;

                // build tree view
                TreeNodeViewModel viewModel = new TreeNodeViewModel();
                foreach (ISyntaxNode node in scope)
                {
                    viewModel.TreeNodes.Add(new TreeNodeViewModel()
                    {
                        NodeText = node.ToString(),
                        NodePayload = node
                    });
                }
                // open dialog window
                Visual control = args.Source as Visual;
                PopupWindow dialog = new PopupWindow(control, viewModel);
                _ = dialog.ShowDialog();
                if (dialog.Result == null) { return; }
                if (!(dialog.Result.NodePayload is ISyntaxNode model)) return;

                // set binded property of the model by selected reference
                // TODO: check if simple type selected so as to set int to int property, not Numeric ...
                property = ancestor.Model.GetPropertyInfo(PropertyBinding);
                if (property.IsOptional())
                {
                    IOptional optional = (IOptional)property.GetValue(ancestor.Model);
                    optional.Value = model;
                }
                else
                {
                    property.SetValue(ancestor.Model, model);
                }

                // reset view model's state
                Model = model;
                TextColor = _selectedValueBrush;
                OnPropertyChanged(nameof(Presentation));
            }
        }
    }
}