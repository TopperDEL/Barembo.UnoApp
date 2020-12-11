using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Prism.Ioc;
using Barembo.UnoApp.Shared.Views;
using Prism;
using Prism.Mvvm;

namespace Barembo.UnoApp
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App
    {
        /// <summary>
        /// Returns the Ioc-Container if something needs to be resolved
        /// </summary>
        /// <returns>The IContainerProvider</returns>
        public static IContainerProvider GetIocContainer()
        {
            return ((PrismApplicationBase)App.Current).Container;
        }

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
#if __IOS__
            AppCenter.Start("a36c506c-f8f6-438d-a1f3-bb1fa9bb783e", typeof(Analytics), typeof(Crashes));
            var fmw = @"storj_uplink.framework/storj_uplink";
            var documentsPath = Foundation.NSBundle.MainBundle.BundlePath;
            var filepath = System.IO.Path.Combine(documentsPath, "Frameworks", fmw);

            mono_dllmap_insert(IntPtr.Zero, "storj_uplink", null, filepath, null);

            ////IntPtr result = ObjCRuntime.Dlfcn.dlopen(filepath, 0);
            var uni2 = universe2();
            //var uni = universe();
            //var version = get_storj_version();
#endif
#if __DROID__
            AppCenter.Start("c4909240-0dcb-4d5c-a2f3-95c0501ca07d", typeof(Analytics), typeof(Crashes));
#endif
#if __WINDOWS__
            AppCenter.Start("cfc03045-3cc0-4026-b98d-e4bc207dc783", typeof(Analytics), typeof(Crashes));
#endif

            ConfigureFilters(global::Uno.Extensions.LogExtensionPoint.AmbientLoggerFactory);

            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        [System.Runtime.InteropServices.DllImport("__Internal",CharSet = System.Runtime.InteropServices.CharSet.Ansi)]
        private static extern void mono_dllmap_insert(IntPtr assembly, string dll, string func, string tdll, string tfunc);

        //[global::System.Runtime.InteropServices.DllImport("@rpath/storj_uplink.framework/storj_uplink", EntryPoint = "CSharp_uplinkfSWIG_get_storj_version___", CharSet = System.Runtime.InteropServices.CharSet.Ansi)]
        //public static extern string get_storj_version();

        //[global::System.Runtime.InteropServices.DllImport("@rpath/storj_uplink.framework/storj_uplink", EntryPoint = "CSharp_uplinkfSWIG_internal_UniverseIsEmpty___", CharSet = System.Runtime.InteropServices.CharSet.Ansi)]
        //public static extern bool universe();

        [global::System.Runtime.InteropServices.DllImport("storj_uplink", EntryPoint = "CSharp_uplinkfSWIG_internal_UniverseIsEmpty___")]
        public static extern bool universe2();

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            base.OnLaunched(e);
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception($"Failed to load {e.SourcePageType.FullName}: {e.Exception}");
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        protected override UIElement CreateShell()
        {
            return Container.Resolve<Shell>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //Store-Services
            containerRegistry.RegisterSingleton<Barembo.Interfaces.IAttachmentStoreService, Barembo.StoreServices.AttachmentStoreService>();
            containerRegistry.RegisterSingleton<Barembo.Interfaces.IBookShareStoreService, Barembo.StoreServices.BookShareStoreService>();
            containerRegistry.RegisterSingleton<Barembo.Interfaces.IBookShelfStoreService, Barembo.StoreServices.BookShelfStoreService>();
            containerRegistry.RegisterSingleton<Barembo.Interfaces.IBookStoreService, Barembo.StoreServices.BookStoreService>();
            containerRegistry.RegisterSingleton<Barembo.Interfaces.IContributorStoreService, Barembo.StoreServices.ContributorStoreService>();
            containerRegistry.RegisterSingleton<Barembo.Interfaces.IEntryStoreService, Barembo.StoreServices.EntryStoreService>();

            //Services
            containerRegistry.RegisterSingleton<Barembo.Interfaces.IBookService, Barembo.Services.BookService>();
            containerRegistry.RegisterSingleton<Barembo.Interfaces.IBookShelfService, Barembo.Services.BookShelfService>();
            containerRegistry.RegisterSingleton<Barembo.Interfaces.IStoreService, Barembo.Services.BufferedStoreService>();
            containerRegistry.RegisterSingleton<Barembo.Interfaces.IEntryService, Barembo.Services.EntryService>();
            containerRegistry.RegisterSingleton<Barembo.Interfaces.IStoreAccessService, Barembo.Services.StoreAccessService>();
            containerRegistry.RegisterSingleton<Barembo.Interfaces.IStoreService, Barembo.Services.StoreService>();

            //App-Core-Services
            containerRegistry.RegisterSingleton<Barembo.App.Core.Interfaces.IThumbnailGeneratorService, Barembo.App.Core.Services.ThumbnailGeneratorService>();

            //App-Services
            containerRegistry.RegisterSingleton<Barembo.App.Core.Interfaces.ILoginService, Barembo.UnoApp.Shared.Services.LoginService>();
            containerRegistry.RegisterSingleton<Barembo.App.Core.Interfaces.IBuildVersionInfoService, Barembo.UnoApp.Shared.Services.BuildVersionInfoService>();

            //Views to navigate to
            containerRegistry.RegisterForNavigation<BookShelfView>();
            containerRegistry.RegisterForNavigation<CreateBookShelfView>();
            containerRegistry.RegisterForNavigation<CreateBookView>();
            containerRegistry.RegisterForNavigation<CreateBookEntryView>();
            //containerRegistry.RegisterForNavigation<ImportBookView>();
            containerRegistry.RegisterForNavigation<BookEntriesView>();
        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();

            ViewModelLocationProvider.Register<Shell, Barembo.App.Core.ViewModels.VersionInfoViewModel>();
            ViewModelLocationProvider.Register<LoginView, Barembo.App.Core.ViewModels.LoginViewModel>();
            ViewModelLocationProvider.Register<BookShelfView, Barembo.App.Core.ViewModels.BookShelfViewModel>();
            ViewModelLocationProvider.Register<CreateBookShelfView, Barembo.App.Core.ViewModels.CreateBookShelfViewModel>();
            ViewModelLocationProvider.Register<CreateBookView, Barembo.App.Core.ViewModels.CreateBookViewModel>();
            ViewModelLocationProvider.Register<CreateBookEntryView, Barembo.App.Core.ViewModels.CreateBookEntryViewModel>();
            //ViewModelLocationProvider.Register<ImportBookView, Barembo.App.Core.ViewModels.ImportBookViewModel>();
            ViewModelLocationProvider.Register<BookEntriesView, Barembo.App.Core.ViewModels.BookEntriesViewModel>();
        }


        /// <summary>
        /// Configures global logging
        /// </summary>
        /// <param name="factory"></param>
        static void ConfigureFilters(ILoggerFactory factory)
        {
            factory
                .WithFilter(new FilterLoggerSettings
                    {
                        { "Uno", Microsoft.Extensions.Logging.LogLevel.Warning },
                        { "Windows", Microsoft.Extensions.Logging.LogLevel.Warning },

       //                  //Debug JS interop

       //                  { "Uno.Foundation.WebAssemblyRuntime", Microsoft.Extensions.Logging.LogLevel.Debug },

       //                  //Generic Xaml events

       //                  { "Windows.UI.Xaml", Microsoft.Extensions.Logging.LogLevel.Debug },
       //                  { "Windows.UI.Xaml.VisualStateGroup", Microsoft.Extensions.Logging.LogLevel.Debug },
       //                  { "Windows.UI.Xaml.StateTriggerBase", Microsoft.Extensions.Logging.LogLevel.Debug },
       //                  { "Windows.UI.Xaml.UIElement", Microsoft.Extensions.Logging.LogLevel.Debug },

       //                  //Layouter specific messages

       //                  { "Windows.UI.Xaml.Controls", Microsoft.Extensions.Logging.LogLevel.Debug },
       //                  { "Windows.UI.Xaml.Controls.Layouter", Microsoft.Extensions.Logging.LogLevel.Debug },
       //                  { "Windows.UI.Xaml.Controls.Panel", Microsoft.Extensions.Logging.LogLevel.Debug },
       //                  { "Windows.Storage", Microsoft.Extensions.Logging.LogLevel.Debug },

       //                  //Binding related messages

       //                  { "Windows.UI.Xaml.Data", Microsoft.Extensions.Logging.LogLevel.Debug },

       //                  //DependencyObject memory references tracking

       //                  { "ReferenceHolder", Microsoft.Extensions.Logging.LogLevel.Debug },

       //                  //ListView-related messages

       //                  { "Windows.UI.Xaml.Controls.ListViewBase", Microsoft.Extensions.Logging.LogLevel.Debug },
       //                  { "Windows.UI.Xaml.Controls.ListView", Microsoft.Extensions.Logging.LogLevel.Debug },
       //                  { "Windows.UI.Xaml.Controls.GridView", Microsoft.Extensions.Logging.LogLevel.Debug },
       //                  { "Windows.UI.Xaml.Controls.VirtualizingPanelLayout", Microsoft.Extensions.Logging.LogLevel.Debug },
       //                  { "Windows.UI.Xaml.Controls.NativeListViewBase", Microsoft.Extensions.Logging.LogLevel.Debug },
       //                  { "Windows.UI.Xaml.Controls.ListViewBaseSource", Microsoft.Extensions.Logging.LogLevel.Debug }, //iOS
						 //{ "Windows.UI.Xaml.Controls.ListViewBaseInternalContainer", Microsoft.Extensions.Logging.LogLevel.Debug }, //iOS
						 //{ "Windows.UI.Xaml.Controls.NativeListViewBaseAdapter", Microsoft.Extensions.Logging.LogLevel.Debug }, //Android
						 //{ "Windows.UI.Xaml.Controls.BufferViewCache", Microsoft.Extensions.Logging.LogLevel.Debug }, //Android
						 //{ "Windows.UI.Xaml.Controls.VirtualizingPanelGenerator", Microsoft.Extensions.Logging.LogLevel.Debug }, //WASM
					}
                )
#if DEBUG
				.AddConsole(Microsoft.Extensions.Logging.LogLevel.Debug);
#else
                .AddConsole(Microsoft.Extensions.Logging.LogLevel.Information);
#endif
        }
    }
}
