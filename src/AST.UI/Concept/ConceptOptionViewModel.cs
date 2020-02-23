using OneCSharp.AST.Model;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace OneCSharp.AST.UI
{
    public sealed class ConceptOptionViewModel : SyntaxNodeViewModel
    {
        public ConceptOptionViewModel(ConceptNodeViewModel owner) : base(owner)
        {
            if (owner == null) throw new ArgumentNullException(nameof(owner));
        }
        public string Presentation
        {
            get { return PropertyBinding; }
        }
        protected override void OnMouseDown(object parameter)
        {
            if (!(parameter is MouseButtonEventArgs args)) return;
            if (args.ChangedButton == MouseButton.Right) return;
            args.Handled = true;

            // TODO: move this code to SyntaxTreeManager
            PropertyInfo property = Owner.SyntaxNode.GetPropertyInfo(PropertyBinding);
            Type optionType = SyntaxTreeManager.GetPropertyType(property);
            ISyntaxNode concept = SyntaxTreeManager.CreateConcept(optionType, Owner.SyntaxNode, PropertyBinding);
            SyntaxTreeController controller = new SyntaxTreeController();
            ConceptNodeViewModel node = controller.CreateSyntaxNode(Owner, concept);
            node.PropertyBinding = PropertyBinding;

            NodePosition position = Owner.GetPosition(this);
            Owner.Lines[position.Line].Nodes.RemoveAt(position.Position + 1); // remove concept view model from layout
            Owner.Lines[position.Line].Nodes.RemoveAt(position.Position); // remove this concept option command
            Owner.Lines[position.Line].Nodes.Add(node); // add newly created concept node
        }
    }
}