using Barembo.Interfaces;
using Barembo.Models;
using Microsoft.Toolkit.Collections;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Barembo.App.Core.ViewModels;
using Prism.Events;

namespace Barembo.UnoApp.Shared.Helpers
{
    public class EntryViewModelSource : IIncrementalSource<EntryViewModel>
    {
        private IEnumerable<EntryReference> _entryReferences;
        private readonly IEntryService _entryService;
        private readonly BookReference _bookReference;
        private bool _loaded;
        private List<EntryViewModel> _viewModels = new List<EntryViewModel>();
        private Task _showPreviewsTask;

        public EntryViewModelSource(IEntryService entryService, BookReference bookReference)
        {
            _entryService = entryService;
            _bookReference = bookReference;
            _showPreviewsTask = Task.Factory.StartNew(ShowPreviews);
        }
        public Windows.UI.Core.CoreDispatcher Dispatcher { get; set; }
        private async Task ShowPreviews()
        {
            while (true)
            {
                foreach (var vm in _viewModels.ToList())
                {
                    foreach (var preview in vm.AttachmentPreviews)
                    {
                        if (preview.IsVideo)
                        {
                            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, () =>
                            {
                                preview.ShowNextVideoImage();
                            });
                        }
                    }
                }

                await Task.Delay(200);
            }
        }

        public void Unload()
        {
            _showPreviewsTask.Dispose();
            _viewModels.Clear();
        }

        public async Task InitAsync()
        {
            if (!_loaded)
            {
                //System.Diagnostics.Debug.WriteLine("Loading entryrefs")
;                _entryReferences = await _entryService.ListEntriesAsync(_bookReference);
                _loaded = true;
            }
        }

        public async Task<IEnumerable<EntryViewModel>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            await InitAsync().ConfigureAwait(false);

            //System.Diagnostics.Debug.WriteLine("Paging " + pageIndex + " - " + pageSize);

            var paged = (from e in _entryReferences
                         select e).Skip(pageIndex * pageSize).Take(pageSize);

            return paged.Select(s =>
            {
                //System.Diagnostics.Debug.WriteLine("Selected " + s.EntryId + " - " + s.CreationDate.ToLongDateString());
                var vm = new EntryViewModel(s, _entryService, SynchronizationContext.Current);
                vm.EntryLoaded += Vm_EntryLoaded;
                return vm;
            });
        }

        private async void Vm_EntryLoaded(EntryViewModel vm, Entry entry)
        {
            //System.Diagnostics.Debug.WriteLine("Loaded " + entry.Id + " - " + entry.CreationDate.ToLongDateString() + " - " + entry.Header);
            vm.EntryLoaded -= Vm_EntryLoaded;
            await vm.LoadAttachmentPreviewsAsync();
            _viewModels.Add(vm);
        }
    }
}
