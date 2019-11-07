using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using OneCSharp.OQL.UI.Services;
using Task = System.Threading.Tasks.Task;

namespace OneCSharp.VisualStudio
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [Guid(OneCSharpPackage.PackageGuidString)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    //[ProvideService(typeof(SOneCSharpCodeProvider), IsAsyncQueryable = true)]
    [ProvideToolWindow(typeof(OneCSharpToolWindow), MultiInstances = true, Style = VsDockStyle.MDI)]
    [ProvideToolWindow(typeof(MetadataToolWindow), MultiInstances = false, Style = VsDockStyle.Tabbed, Orientation = ToolWindowOrientation.Left)]
    public sealed class OneCSharpPackage : AsyncPackage
    {
        /// <summary>
        /// VSIXProject1Package GUID string.
        /// </summary>
        public const string PackageGuidString = "81bea8cc-e02d-49f5-bd0f-68d3ebc2120d";

        //public const string guidOQLPackageCmdSet = "75e92cfb-6655-4e40-b237-39cb43bef33a";  // get the GUID from the .vsct file
        //public const int OneCSharpMetadataToolbar = 0x1000;

        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to monitor for initialization cancellation, which can occur when VS is shutting down.</param>
        /// <param name="progress">A provider for progress updates.</param>
        /// <returns>A task representing the async work of package initialization, or an already completed task if there is none. Do not return null from this method.</returns>
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            // When initialized asynchronously, the current thread may be a background thread at this point.
            // Do any initialization that requires the UI thread after switching to the UI thread.
            await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            await MetadataToolWindowCommand.InitializeAsync(this);
            await OneCSharpToolWindowCommand.InitializeAsync(this);
            IComponentModel MEF = (IComponentModel)GetService(typeof(SComponentModel));
            if (MEF != null)
            {
                // TODO: make twm private field so as to manage it's life time !?
                IToolWindowManager twm = MEF.GetService<IToolWindowManager>();
                twm.Initialize(this);
            }
        }
        //private object CreateService(IServiceContainer container, Type serviceType)
        //{
        //    if (serviceType == typeof(SOneCSharpCodeProvider))
        //    {
        //        return new OneCSharpCodeProvider(this);
        //    }
        //    return null;
        //}
        //public async Task<object> CreateServiceAsync(IAsyncServiceContainer container, CancellationToken cancellationToken, Type serviceType)
        //{
        //    OneCSharpCodeProvider service = new OneCSharpCodeProvider(this);
        //    await service.InitializeAsync(cancellationToken);
        //    return service;
        //}
        #endregion
    }
}
