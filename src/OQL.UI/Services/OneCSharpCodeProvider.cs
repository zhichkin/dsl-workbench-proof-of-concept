using Microsoft.VisualStudio.OLE.Interop;
using OneCSharp.OQL.Model;
using System.Windows.Controls;

namespace OneCSharp.OQL.UI.Services
{
    public interface SOneCSharpCodeProvider { } // Visual Studio want's it just like that
    public sealed class OneCSharpCodeProvider : SOneCSharpCodeProvider, IOneCSharpCodeProvider
    {
        private IServiceProvider _serviceProvider;
        public OneCSharpCodeProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public IOneCSharpCodeEditor GetCodeEditor(ISyntaxNode node)
        {
            if (node is Procedure)
            {
                return new ProcedureViewModel();
            }
            return null;
        }
        public UserControl GetCodeEditorView(IOneCSharpCodeEditor editor)
        {
            if (editor is ProcedureViewModel)
            {
                return new ProcedureView(editor);
            }
            return null;
        }
    }
}
