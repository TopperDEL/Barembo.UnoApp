using Barembo.App.Core.ViewModels;
using Barembo.Interfaces;
using Barembo.Models;
using Microsoft.AppCenter.Analytics;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using uplink.NET.Interfaces;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Barembo.UnoApp.Shared.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BookShelfView : Page, INavigationAware
    {
        readonly IUploadQueueService _uploadQueueService;
        IStoreService _storeService;

        public BookShelfView(IUploadQueueService uploadQueueService, IStoreService storeService)
        {
            this.InitializeComponent();

            _uploadQueueService = uploadQueueService;
            _storeService = storeService;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            Analytics.TrackEvent("BookShelfView - OnNavigatedTo");
            var storeAccess = (StoreAccess)navigationContext.Parameters["StoreAccess"];
            BookShelf bookShelf;
            try
            {
                Analytics.TrackEvent("BookShelfView - OnNavigatedTo - try fetch bookshelf");
                bookShelf = await _storeService.GetObjectFromJsonAsync<BookShelf>(storeAccess, StoreKey.BookShelf());
                Analytics.TrackEvent("BookShelfView - OnNavigatedTo - bookshelf fetched");
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("BookShelfView - OnNavigatedTo - fetch bookshelf exception");
                Microsoft.AppCenter.Crashes.Crashes.TrackError(ex, new Dictionary<string, string>() {
                {"ExceptionType","Try to load BookShelf on Android"}
                });
            }
            Analytics.TrackEvent("BookShelfView - OnNavigatedTo - before initasync");
            await ((BookShelfViewModel)this.DataContext).InitAsync((StoreAccess)navigationContext.Parameters["StoreAccess"]);
            Analytics.TrackEvent("BookShelfView - OnNavigatedTo - after initasync");
            _uploadQueueService.ProcessQueueInBackground();
            Analytics.TrackEvent("BookShelfView - OnNavigatedTo - after procqueueinbackground");
        }
    }
}
