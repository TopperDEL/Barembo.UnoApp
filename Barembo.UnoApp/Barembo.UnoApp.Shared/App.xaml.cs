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
            AppCenter.Start("cfc03045-3cc0-4026-b98d-e4bc207dc783", typeof(Analytics), typeof(Crashes));

            ConfigureFilters(global::Uno.Extensions.LogExtensionPoint.AmbientLoggerFactory);

            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

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

            //App-Services
            containerRegistry.RegisterSingleton<Barembo.App.Core.Interfaces.ILoginService, Barembo.UnoApp.Shared.Services.LoginService>();

            //Views to navigate to
            containerRegistry.RegisterForNavigation<BookShelfView>();
            containerRegistry.RegisterForNavigation<CreateBookShelfView>();
        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();

            ViewModelLocationProvider.Register<LoginView, Barembo.App.Core.ViewModels.LoginViewModel>();
            ViewModelLocationProvider.Register<BookShelfView, Barembo.App.Core.ViewModels.BookShelfViewModel>();
            ViewModelLocationProvider.Register<CreateBookShelfView, Barembo.App.Core.ViewModels.CreateBookShelfViewModel>();
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

						// Debug JS interop
						// { "Uno.Foundation.WebAssemblyRuntime", LogLevel.Debug },

						// Generic Xaml events
						// { "Windows.UI.Xaml", LogLevel.Debug },
						// { "Windows.UI.Xaml.VisualStateGroup", LogLevel.Debug },
						// { "Windows.UI.Xaml.StateTriggerBase", LogLevel.Debug },
						// { "Windows.UI.Xaml.UIElement", LogLevel.Debug },

						// Layouter specific messages
						// { "Windows.UI.Xaml.Controls", LogLevel.Debug },
						// { "Windows.UI.Xaml.Controls.Layouter", LogLevel.Debug },
						// { "Windows.UI.Xaml.Controls.Panel", LogLevel.Debug },
						// { "Windows.Storage", LogLevel.Debug },

						// Binding related messages
						// { "Windows.UI.Xaml.Data", LogLevel.Debug },

						// DependencyObject memory references tracking
						// { "ReferenceHolder", LogLevel.Debug },

						// ListView-related messages
						// { "Windows.UI.Xaml.Controls.ListViewBase", LogLevel.Debug },
						// { "Windows.UI.Xaml.Controls.ListView", LogLevel.Debug },
						// { "Windows.UI.Xaml.Controls.GridView", LogLevel.Debug },
						// { "Windows.UI.Xaml.Controls.VirtualizingPanelLayout", LogLevel.Debug },
						// { "Windows.UI.Xaml.Controls.NativeListViewBase", LogLevel.Debug },
						// { "Windows.UI.Xaml.Controls.ListViewBaseSource", LogLevel.Debug }, //iOS
						// { "Windows.UI.Xaml.Controls.ListViewBaseInternalContainer", LogLevel.Debug }, //iOS
						// { "Windows.UI.Xaml.Controls.NativeListViewBaseAdapter", LogLevel.Debug }, //Android
						// { "Windows.UI.Xaml.Controls.BufferViewCache", LogLevel.Debug }, //Android
						// { "Windows.UI.Xaml.Controls.VirtualizingPanelGenerator", LogLevel.Debug }, //WASM
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
