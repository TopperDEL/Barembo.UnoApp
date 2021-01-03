using Barembo.App.Core.Interfaces;
using Barembo.App.Core.Messages;
using Barembo.App.Core.ViewModels;
using Barembo.Models;
using Prism;
using Prism.DryIoc;
using Prism.Events;
using Prism.Ioc;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Barembo.UnoApp.Shared.Views
{
    public sealed partial class Shell
    {
        public string ContentRegion { get; } = "ContentRegion";
        readonly IRegionManager _regionManager;
        readonly IEventAggregator _eventAggregator;
        private StoreAccess _currentStoreAccess;
        public VersionInfoViewModel VersionInfoVM { get; set; }

        public Shell(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            this.InitializeComponent();

            _regionManager = regionManager;
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<Barembo.App.Core.Messages.SuccessfullyLoggedInMessage>().Subscribe(NavigateToBookShelfView);
            _eventAggregator.GetEvent<Barembo.App.Core.Messages.NoBookShelfExistsMessage>().Subscribe(NavigateToCreateBookShelfView);
            _eventAggregator.GetEvent<Barembo.App.Core.Messages.BookShelfCreatedMessage>().Subscribe(NavigateToBookShelfView);
            _eventAggregator.GetEvent<Barembo.App.Core.Messages.AddOwnBookMessage>().Subscribe(NavigateToCreateBookView);
            _eventAggregator.GetEvent<Barembo.App.Core.Messages.AddForeignBookMessage>().Subscribe(NavigateToImportBookView);
            _eventAggregator.GetEvent<Barembo.App.Core.Messages.BookCreatedMessage>().Subscribe(NavigateToBookShelfView);
            _eventAggregator.GetEvent<Barembo.App.Core.Messages.CreateBookEntryMessage>().Subscribe(NavigateToCreateEntryView);
            _eventAggregator.GetEvent<Barembo.App.Core.Messages.BookEntrySavedMessage>().Subscribe(NavigateFromSavedBookEntry);
            _eventAggregator.GetEvent<Barembo.App.Core.Messages.ShowBookEntriesMessage>().Subscribe(NavigateToBookEntriesView);
            _eventAggregator.GetEvent<Barembo.App.Core.Messages.GoBackMessage>().Subscribe(GoBack);
            _eventAggregator.GetEvent<Barembo.App.Core.Messages.ErrorMessage>().Subscribe(RaiseError);
        }

        private void ShellLoaded(object sender, RoutedEventArgs e)
        {
            _regionManager.RegisterViewWithRegion(ContentRegion, typeof(LoginView));
        }
        private void NavigateToBookShelfView(StoreAccess storeAccess)
        {
            _currentStoreAccess = storeAccess;
            var parameters = new NavigationParameters();
            parameters.Add("StoreAccess", storeAccess);
            _regionManager.RequestNavigate(ContentRegion, "BookShelfView", parameters);
        }

        private void NavigateToCreateBookShelfView(StoreAccess storeAccess)
        {
            var parameters = new NavigationParameters();
            parameters.Add("StoreAccess", storeAccess);
            _regionManager.RequestNavigate(ContentRegion, "CreateBookShelfView", parameters);
        }

        private void NavigateToCreateBookView(Tuple<StoreAccess, BookShelf> data)
        {
            var parameters = new NavigationParameters();
            parameters.Add("StoreAccess", data.Item1);
            parameters.Add("BookShelf", data.Item2);
            _regionManager.RequestNavigate(ContentRegion, "CreateBookView", parameters);
        }

        private void NavigateToImportBookView(Tuple<StoreAccess, BookShelf> data)
        {
            var parameters = new NavigationParameters();
            parameters.Add("StoreAccess", data.Item1);
            parameters.Add("BookShelf", data.Item2);
            _regionManager.RequestNavigate(ContentRegion, "ImportBookView", parameters);
        }

        private void NavigateToCreateEntryView(BookReference bookReference)
        {
            var parameters = new NavigationParameters();
            parameters.Add("BookReference", bookReference);
            _regionManager.RequestNavigate(ContentRegion, "CreateBookEntryView", parameters);
        }

        private void NavigateFromSavedBookEntry(Tuple<EntryReference,Entry> entryData)
        {
            NavigateToBookShelfView(_currentStoreAccess);
        }

        private void NavigateToBookEntriesView(BookReference bookReference)
        {
            var parameters = new NavigationParameters();
            parameters.Add("BookReference", bookReference);
            _regionManager.RequestNavigate(ContentRegion, "BookEntriesView", parameters);
        }

        private void GoBack()
        {
            _regionManager.Regions[ContentRegion].NavigationService.Journal.GoBack();
        }

        private void RaiseError(Tuple<ErrorType, string> errorData)
        {

        }
    }
}
