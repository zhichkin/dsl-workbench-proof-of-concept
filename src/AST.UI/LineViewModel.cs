using System.Collections.ObjectModel;

namespace OneCSharp.AST.UI
{
    public sealed class LineViewModel
    {
        private readonly IConceptViewModel _owner;
        public LineViewModel(IConceptViewModel owner)
        {
            _owner = owner;
        }
        public ObservableCollection<IConceptViewModel> Items { get; } = new ObservableCollection<IConceptViewModel>();
    }
}