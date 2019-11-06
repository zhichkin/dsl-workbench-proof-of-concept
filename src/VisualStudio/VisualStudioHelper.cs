using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using OneCSharp.OQL.Model;
using OneCSharp.OQL.UI.Services;
using System;

namespace OneCSharp.VisualStudio
{
    public static class VisualStudioHelper
    {
        public static void OpenOneCSharpCodeEditorWindow(IOneCSharpCodeEditorConsumer consumer, ISyntaxNode node)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            IVsUIShell vsUIShell = (IVsUIShell)Package.GetGlobalService(typeof(SVsUIShell));
            Guid guid = typeof(OneCSharpToolWindow).GUID;
            IVsWindowFrame windowFrame = null;
            for (uint i = 0; i < 10; i++)
            {
                int result = vsUIShell.FindToolWindowEx((uint)__VSFINDTOOLWIN.FTW_fFrameOnly, ref guid, i, out windowFrame);
                if (result == VSConstants.S_OK)
                {
                    continue;
                }
                else
                {
                    _ = vsUIShell.FindToolWindowEx((uint)__VSFINDTOOLWIN.FTW_fForceCreate, ref guid, i, out windowFrame);
                    break;
                }
            }
            OneCSharpToolWindow codeEditorWindow = windowFrame as OneCSharpToolWindow;
            if (codeEditorWindow != null)
            {
                codeEditorWindow.EditSyntaxNode(consumer, node);
            }
            ErrorHandler.ThrowOnFailure(windowFrame.Show());
        }
    }
}
