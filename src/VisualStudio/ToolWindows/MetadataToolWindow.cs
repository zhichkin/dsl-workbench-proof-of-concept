namespace OneCSharp.VisualStudio
{
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Shell;
    using System.ComponentModel.Design;

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
    [Guid("f9146dad-61b3-42d8-a091-169101148bf8")]
    public class MetadataToolWindow : ToolWindowPane
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataToolWindow"/> class.
        /// </summary>
        public MetadataToolWindow() : base(null)
        {
            this.Caption = "ONE-C-SHARP © 2019";
            //this.ToolBar = new CommandID(new Guid(OQLPackage.guidOQLPackageCmdSet), OQLPackage.OneCSharpMetadataToolbar);
            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            this.Content = new MetadataToolWindowControl();
        }
    }
}
