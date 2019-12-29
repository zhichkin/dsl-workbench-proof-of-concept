using OneCSharp.AST.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace OneCSharp.AST.UI
{
    public interface ISyntaxNode
    {
        Concept Model { get; set; }
        ISyntaxNode Owner { get; set; }
        ObservableCollection<ISyntaxNodeLine> Lines { get; }
        bool IsFocused { get; set; }
        bool IsMouseOver { get; set; }
        ICommand KeyDownCommand { get; set; }
        ICommand MouseDownCommand { get; set; }
        ICommand MouseEnterCommand { get; set; }
        ICommand MouseLeaveCommand { get; set; }
        ICommand CtrlCCommand { get; set; }
        ICommand CtrlVCommand { get; set; }
    }
    public interface ISyntaxNodeLine
    {
        ISyntaxNode Owner { get; }
        ObservableCollection<ISyntaxNode> Nodes { get; }
    }
}