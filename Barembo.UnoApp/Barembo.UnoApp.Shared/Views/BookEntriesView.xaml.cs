using Barembo.App.Core.ViewModels;
using Barembo.Interfaces;
using Barembo.Models;
using Barembo.UnoApp.Shared.Helpers;
using Microsoft.Toolkit.Uwp;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Barembo.UnoApp.Shared.Views
{
    public sealed partial class BookEntriesView : Page, INavigationAware
    {
        private readonly IEntryService _entryService;
        private readonly IEventAggregator _eventAggregator;
        private EntryViewModelSource _viewModelSource;
        private IncrementalLoadingCollection<EntryViewModelSource, EntryViewModel> _collection;

        public BookEntriesView(IEntryService entryService, IEventAggregator eventAggregator)
        {
            this.InitializeComponent();

            _entryService = entryService;
            _eventAggregator = eventAggregator;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            _viewModelSource?.Unload();
            _collection = null;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var vm = (BookEntriesViewModel)this.DataContext;
            _viewModelSource = new EntryViewModelSource(_entryService, (BookReference)navigationContext.Parameters["BookReference"]);
            _viewModelSource.Dispatcher = Dispatcher;
            _collection = new IncrementalLoadingCollection<EntryViewModelSource, EntryViewModel>(_viewModelSource, 5);
            vm.Entries = _collection;
        }

        private void EntrySelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                ShowEntryDetailsStoryboard.Begin();
                EntryDetails.IsHitTestVisible = true;
            }
        }

        private void CloseEntryDetails(object sender, RoutedEventArgs e)
        {
            HideEntryDetailsStoryboard.Begin();
            HideEntryDetailsStoryboard.Completed += (a,b)=> { EntriesList.SelectedItem = null; } ;
            EntryDetails.IsHitTestVisible = false;
        }
    }
}
