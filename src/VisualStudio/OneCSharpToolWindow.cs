namespace OQL
{
    using System;
    using System.Runtime.InteropServices;
    using global::OneCSharp.OQL.Model;
    using global::OneCSharp.OQL.UI;
    using Microsoft.VisualStudio.Shell;

    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    /// </summary>
    /// <remarks>
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
    /// usually implemented by the package implementer.
    /// <para>
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its
    /// implementation of the IVsUIElementPane interface.
    /// </para>
    /// </remarks>
    [Guid("0a01d6af-6211-40c0-ab52-9d433d3512b1")]
    public class OneCSharpToolWindow : ToolWindowPane
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OneCSharpToolWindow"/> class.
        /// </summary>
        public OneCSharpToolWindow() : base(null)
        {
            this.Caption = "OneCSharp Tool Window";

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            
            //new OneCSharpToolWindowControl();
            this.Content = new ProcedureView(new ProcedureViewModel(new Procedure()));
        }
    }
}
