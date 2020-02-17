using System;
using System.Collections.ObjectModel;

namespace OneCSharp.AST.UI
{
    public sealed class PropertyViewModel : SyntaxNodeViewModel
    {
        public PropertyViewModel(ConceptNodeViewModel owner) : base(owner)
        {
            if (owner == null) throw new ArgumentNullException(nameof(owner));
        }
        public ObservableCollection<ISyntaxNodeViewModel> Nodes { get; } = new ObservableCollection<ISyntaxNodeViewModel>();
        protected override void OnMouseEnter(object parameter)
        {
            base.OnMouseEnter(parameter);
            IsMouseOver = true;
        }
        protected override void OnMouseLeave(object parameter)
        {
            base.OnMouseLeave(parameter);
            IsMouseOver = false;
        }
    }
}