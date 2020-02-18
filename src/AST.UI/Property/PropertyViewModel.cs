using System;
using System.Collections.ObjectModel;

namespace OneCSharp.AST.UI
{
    public sealed class PropertyViewModel : SyntaxNodeViewModel
    {
        public PropertyViewModel(ConceptNodeViewModel owner) : base(owner, owner.Model)
        {
            if (owner == null) throw new ArgumentNullException(nameof(owner));
        }
        public ObservableCollection<ISyntaxNodeViewModel> Nodes { get; } = new ObservableCollection<ISyntaxNodeViewModel>();
    }
}