using OneCSharp.AST.Model;
using OneCSharp.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            get
            {
                if (SyntaxNode == null)
                {
                    if (SyntaxNodeType == null)
                    {
                        // TODO: refactor all this hell below !!!
                        var ancestor = this.Ancestor<ConceptNodeViewModel>();
                        if (ancestor == null) return $"{{{PropertyBinding}}}";
                        PropertyInfo property = ancestor.SyntaxNode.GetPropertyInfo(PropertyBinding);
                        Type propertyType = SyntaxTreeManager.GetPropertyType(property);
                        if(propertyType == null) return $"{{{PropertyBinding}}}";
                        if (propertyType.IsEnum)
                        {
                            if (property.IsOptional())
                            {
                                IOptional optional = (IOptional)property.GetValue(ancestor.SyntaxNode);
                                if (optional.HasValue)
                                {
                                    return ReflectionExtensions.GetEnumValuePresentation(
                                        propertyType,
                                        optional.Value);
                                }
                                else
                                {
                                    return $"{{{PropertyBinding}}}";
                                }
                            }
                            else
                            {
                                return ReflectionExtensions.GetEnumValuePresentation(
                                    propertyType,
                                    property.GetValue(ancestor.SyntaxNode));
                            }
                        }
                        return $"{{{PropertyBinding}}}";
                    }
                    else
                    {
                        return SyntaxNodeType.ToString();
                    }
                }
                return SyntaxNode.ToString();
            }
        }
        protected override void OnMouseLeave(object parameter)
        {
            if (parameter == null) return;
            MouseEventArgs args = (MouseEventArgs)parameter;
            args.Handled = true;
        }
        protected override void OnMouseDown(object parameter)
        {
            if (!(parameter is MouseButtonEventArgs args)) return;
            if (args.ChangedButton == MouseButton.Right) return;
            args.Handled = true;

            Visual control = args.Source as Visual;

            if (IsTemporallyVisible)
            {
                base.OnMouseDown(parameter);
                return;
            }

            // get parent concept node
            var ancestor = this.Ancestor<ConceptNodeViewModel>();
            if (ancestor == null) return;

            // check if property is referencing assembly - special case !
            if (ancestor.SyntaxNode.IsAssemblyReference(PropertyBinding))
            {
                IAssemblyConcept assemblyConcept = SelectAssemblyReference(ancestor.SyntaxNode, PropertyBinding, control);
                if (assemblyConcept == null) { return; }
                SyntaxTreeManager.SetConceptProperty(ancestor.SyntaxNode, PropertyBinding, assemblyConcept.Assembly);
                SyntaxNode = ancestor.SyntaxNode;
                OnPropertyChanged(nameof(Presentation));
                return;
            }

            // get hosting script concept
            ScriptConcept script;
            if (ancestor.SyntaxNode is ScriptConcept)
            {
                script = (ScriptConcept)ancestor.SyntaxNode;
            }
            else
            {
                script = ancestor.SyntaxNode.Ancestor<ScriptConcept>() as ScriptConcept;
            }

            // get type constraints of the property
            TypeConstraint constraints = SyntaxTreeManager.GetTypeConstraints(script?.Languages, ancestor.SyntaxNode, PropertyBinding);

            // build tree view
            TreeNodeViewModel viewModel = SyntaxNodeSelector.BuildSelectorTree(constraints);

            // open dialog window
            PopupWindow dialog = new PopupWindow(control, viewModel);
            _ = dialog.ShowDialog();
            if (dialog.Result == null) { return; }

            Type selectedType = dialog.Result.NodePayload as Type;
            if (selectedType == null) { return; }

            // set binded property of the model by selected reference
            PropertyInfo property = ancestor.SyntaxNode.GetPropertyInfo(PropertyBinding);
            if (SyntaxTreeManager.GetPropertyType(property) == typeof(Type))
            {
                selectedType = SelectTypeReference(ancestor.SyntaxNode, selectedType, control);
                SyntaxTreeManager.SetConceptProperty(ancestor.SyntaxNode, PropertyBinding, selectedType);
                SyntaxNodeType = selectedType;
            }
            else if (selectedType.IsEnum)
            {
                object enumValue = SelectEnumerationValue(selectedType, control);
                if (enumValue == null) { return; }
                SyntaxTreeManager.SetConceptProperty(ancestor.SyntaxNode, PropertyBinding, enumValue);
            }
            else
            {
                ISyntaxNode reference;
                if (selectedType.IsSubclassOf(typeof(DataType)))
                {
                    reference = (ISyntaxNode)Activator.CreateInstance(selectedType);
                }
                else
                {
                    // use dialog and scope provider to find reference to the node in the syntax tree
                    reference = SelectSyntaxNodeReference(selectedType, ancestor.SyntaxNode, PropertyBinding, control);
                    if (reference == null) { return; }
                }
                SyntaxTreeManager.SetConceptProperty(ancestor.SyntaxNode, PropertyBinding, reference);
                SyntaxNode = reference;
            }

            // reset view model's state
            OnPropertyChanged(nameof(Presentation));
        }
        private object SelectEnumerationValue(Type enumType, Visual control)
        {
            // build tree view
            TreeNodeViewModel viewModel = SyntaxNodeExtensions.BuildEnumerationSelectorTree(enumType);

            // open dialog window
            PopupWindow dialog = new PopupWindow(control, viewModel);
            _ = dialog.ShowDialog();
            if (dialog.Result == null) { return null; }

            // return selected enumeration value
            return dialog.Result.NodePayload;
        }
        private IAssemblyConcept SelectAssemblyReference(ISyntaxNode concept, string propertyName, Visual control)
        {
            // get scope provider
            IScopeProvider scopeProvider = SyntaxTreeManager.GetScopeProvider(concept.GetType());
            if (scopeProvider == null) { return null; }

            // get references in the scope
            IEnumerable<ISyntaxNode> scope = scopeProvider.Scope(concept, propertyName);
            if (scope == null || scope.Count() == 0) { return null; }

            // build tree view
            TreeNodeViewModel viewModel = SyntaxNodeExtensions.BuildAssemblySelectorTree(scope);

            // open dialog window
            PopupWindow dialog = new PopupWindow(control, viewModel);
            _ = dialog.ShowDialog();
            if (dialog.Result == null) { return null; }

            // return selected reference
            return (dialog.Result.NodePayload as IAssemblyConcept);
        }
        private Type SelectTypeReference(ISyntaxNode context, Type scopeType, Visual control)
        {
            // get scope provider
            IScopeProvider scopeProvider = SyntaxTreeManager.GetScopeProvider(scopeType);
            if (scopeProvider == null) { return scopeType; } // scope provider is not registered

            // get references in the scope
            IEnumerable<Type> scope = scopeProvider.Scope(context, scopeType);
            if (scope == null || scope.Count() == 0) { return scopeType; } // scope provider found nothing

            // build Type selection tree
            TreeNodeViewModel viewModel = SyntaxNodeExtensions.BuildTypeSelectionTree(scope);
            
            // open dialog window
            PopupWindow dialog = new PopupWindow(control, viewModel);
            _ = dialog.ShowDialog();
            if (dialog.Result == null) { return null; } // user made no choice

            // return selected reference
            return (dialog.Result.NodePayload as Type);
        }
        private ISyntaxNode SelectSyntaxNodeReference(Type childConcept, ISyntaxNode parentConcept, string propertyName, Visual control)
        {
            // get scope provider
            IScopeProvider scopeProvider = SyntaxTreeManager.GetScopeProvider(childConcept);
            if (scopeProvider == null)
            {
                return null;
                // TODO: if selector is for language concepts which are not yet nodes of the syntax tree !!!
                // Create ConceptViewModel and ConceptModel
                // SyntaxTreeController.Current.CreateSyntaxNode(this, new childConcept());
            }

            // get references in the scope
            IEnumerable<ISyntaxNode> scope = scopeProvider.Scope(parentConcept, propertyName);
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