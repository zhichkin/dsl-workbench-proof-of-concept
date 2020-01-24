using System.Collections.ObjectModel;

namespace OneCSharp.AST.UI
{
    public interface ICodeLineViewModel
    {
        ISyntaxNodeViewModel Owner { get; }
        ObservableCollection<ISyntaxNodeViewModel> Nodes { get; }
    }
    public sealed class CodeLineViewModel : ICodeLineViewModel
    {
        public CodeLineViewModel(ISyntaxNodeViewModel owner)
        {
            Owner = owner;
        }
        public ISyntaxNodeViewModel Owner { get; }
        public ObservableCollection<ISyntaxNodeViewModel> Nodes { get; } = new ObservableCollection<ISyntaxNodeViewModel>();
    }
}