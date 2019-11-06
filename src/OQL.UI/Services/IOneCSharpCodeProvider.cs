using OneCSharp.OQL.Model;
using System.Windows.Controls;

namespace OneCSharp.OQL.UI.Services
{
    public interface IOneCSharpCodeProvider
    {
        IOneCSharpCodeEditor GetCodeEditor(ISyntaxNode node);
        UserControl GetCodeEditorView(IOneCSharpCodeEditor editor);
    }
}
