using Microsoft.VisualStudio;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using OneCSharp.OQL.Model;
using OneCSharp.OQL.UI;
using OneCSharp.OQL.UI.Services;
using System;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using Task = System.Threading.Tasks.Task;
using Shell = Microsoft.VisualStudio.Shell;

namespace OneCSharp.VisualStudio
{
    public interface IToolWindowManager
    {
        void Initialize(AsyncPackage serviceProvider);
        void OpenOneCSharpCodeEditor(string caption, IOneCSharpCodeEditorConsumer consumer, ISyntaxNode node);
        IOneCSharpCodeEditor GetCodeEditor(ISyntaxNode node);
        UserControl GetCodeEditorView(IOneCSharpCodeEditor editor);
    }

    [Export(typeof(IToolWindowManager))]
    public sealed class ToolWindowManager : IToolWindowManager
    {
        private AsyncPackage _package;
        public ToolWindowManager() { }
        public void Initialize(AsyncPackage package)
        {
            _package = package;
        }
        public IOneCSharpCodeEditor GetCodeEditor(ISyntaxNode node)
        {
            if (node is Procedure)
            {
                return new ProcedureViewModel((Procedure)node);
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
        public static void OpenOneCSharpCodeEditor(string caption, IOneCSharpCodeEditorConsumer consumer, ISyntaxNode node)
        {
            IComponentModel MEF = (IComponentModel)Package.GetGlobalService(typeof(SComponentModel));
            IToolWindowManager manager = MEF.GetService<IToolWindowManager>();
            manager.OpenOneCSharpCodeEditor(caption, consumer, node);
        }
        void IToolWindowManager.OpenOneCSharpCodeEditor(string caption, IOneCSharpCodeEditorConsumer consumer, ISyntaxNode node)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            for (int i = 0; i < 10; i++)
            {
                OneCSharpToolWindow codeEditorWindow = (OneCSharpToolWindow)_package.FindToolWindow(typeof(OneCSharpToolWindow), i, false);
                if (codeEditorWindow == null)
                {
                    // Create tool window with the first free InstanceID.
                    codeEditorWindow = (OneCSharpToolWindow)_package.FindToolWindow(typeof(OneCSharpToolWindow), i, true);
                    if ((null == codeEditorWindow) || (null == codeEditorWindow.Frame))
                    {
                        throw new NotSupportedException("Cannot create tool window");
                    }

                    var _codeEditor = GetCodeEditor(node);
                    _codeEditor.Save += consumer.SaveSyntaxNode;
                    codeEditorWindow.Caption = caption;
                    ((ContentControl)codeEditorWindow.Content).Content = GetCodeEditorView(_codeEditor);

                    IVsWindowFrame windowFrame = (IVsWindowFrame)codeEditorWindow.Frame;
                    ErrorHandler.ThrowOnFailure(windowFrame.Show());
                    
                    break;
                }
            }
        }
    }
}
