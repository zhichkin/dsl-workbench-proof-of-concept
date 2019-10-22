namespace OQL
{
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for MetadataToolWindowControl.
    /// </summary>
    public partial class MetadataToolWindowControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataToolWindowControl"/> class.
        /// </summary>
        public MetadataToolWindowControl()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Handles click on the button by displaying a message box.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        [SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Justification = "Sample code")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Default event handler naming pattern")]
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            OpenToolWindow();
        }
        private void OpenToolWindow()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            
            IVsUIShell vsUIShell = (IVsUIShell)Package.GetGlobalService(typeof(SVsUIShell));
            Guid guid = typeof(OneCSharpToolWindow).GUID;

            int result = -1;
            IVsWindowFrame windowFrame = null;
            for (uint i = 0; i < 10; i++)
            {
                result = vsUIShell.FindToolWindowEx((uint)__VSFINDTOOLWIN.FTW_fFrameOnly, ref guid, i, out windowFrame);
                if (result == VSConstants.S_OK)
                {
                    continue;
                }
                else
                {
                    result = vsUIShell.FindToolWindowEx((uint)__VSFINDTOOLWIN.FTW_fForceCreate, ref guid, i, out windowFrame);
                    break;
                }
            }
            ErrorHandler.ThrowOnFailure(windowFrame.Show());
        }
    }
}