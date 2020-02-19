using OneCSharp.AST.Model;
using System;
using System.Collections.ObjectModel;
using System.Reflection;

namespace OneCSharp.AST.UI
{
    public sealed class PropertyViewModel : SyntaxNodeViewModel
    {
        public PropertyViewModel(ConceptNodeViewModel owner) : base(owner, owner.Model)
        {
            if (owner == null) throw new ArgumentNullException(nameof(owner));
        }
        public ObservableCollection<ISyntaxNodeViewModel> Nodes { get; } = new ObservableCollection<ISyntaxNodeViewModel>();
        protected override void OnMouseEnter(object parameter)
        {
            base.OnMouseEnter(parameter);

            PropertyInfo property = Model.GetPropertyInfo(PropertyBinding);
            if (property.IsOptional())
            {
                IOptional optional = (IOptional)property.GetValue(Model);
                if (!optional.HasValue)
                {
                    return;
                }
            }
            ShowCommands();
        }
        protected override void OnMouseLeave(object parameter)
        {
            base.OnMouseLeave(parameter);

            PropertyInfo property = Model.GetPropertyInfo(PropertyBinding);
            if (property.IsOptional())
            {
                IOptional optional = (IOptional)property.GetValue(Model);
                if (!optional.HasValue)
                {
                    return;
                }
            }
            HideCommands();
        }
        public void ShowCommands()
        {
            if (Nodes.Count == 0) return;
            if (Nodes[Nodes.Count - 1] is RemoveOptionViewModel) return;
            Nodes.Add(new RemoveOptionViewModel(this)
            {
                PropertyBinding = this.PropertyBinding
            });
        }
        public void HideCommands()
        {
            if (Nodes.Count == 0) return;
            int lastNodeIndex = Nodes.Count - 1;
            if (Nodes[lastNodeIndex] is RemoveOptionViewModel)
            {
                Nodes.RemoveAt(lastNodeIndex);
            }
        }
    }
}