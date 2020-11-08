using Barembo.App.Core.ViewModels;
using Barembo.Interfaces;
using Barembo.Models;
using Barembo.UnoApp.Shared.Helpers;
using Microsoft.Toolkit.Uwp;
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
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BookEntriesView : Page, INavigationAware, INotifyPropertyChanged
    {
        private readonly IEntryService _entryService;

        public event PropertyChangedEventHandler PropertyChanged;

        public IncrementalLoadingCollection<EntryViewModelSource, EntryViewModel> LocalEntries { get; set; }

        public BookEntriesView(IEntryService entryService)
        {
            this.InitializeComponent();

            _entryService = entryService;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var vm = (BookEntriesViewModel)this.DataContext;
            var source = new EntryViewModelSource(_entryService, (BookReference)navigationContext.Parameters["BookReference"]);
            var collection = new IncrementalLoadingCollection<EntryViewModelSource, EntryViewModel>(source, 5);
            vm.Entries = collection;
            LocalEntries = collection;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LocalEntries)));
        }
    }
}
