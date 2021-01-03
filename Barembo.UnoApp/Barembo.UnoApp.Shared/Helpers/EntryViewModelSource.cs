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
        private readonly IEventAggregator _eventAggregator;
        private readonly BookReference _bookReference;
        private bool _loaded;

        public EntryViewModelSource(IEntryService entryService, IEventAggregator eventAggregator, BookReference bookReference)
        {
            _entryService = entryService;
            _eventAggregator = eventAggregator;
            _bookReference = bookReference;
        }

        public async Task InitAsync()
        {
            if (!_loaded)
            {
                _entryReferences = await _entryService.ListEntriesAsync(_bookReference);
                _loaded = true;
            }
        }

        public async Task<IEnumerable<EntryViewModel>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            await InitAsync();

            var paged = (from e in _entryReferences
                          select e).Skip(pageIndex * pageSize).Take(pageSize);

            return paged.Select(s =>
            {
                var vm = new EntryViewModel(s, _entryService, SynchronizationContext.Current);
                vm.EntryLoaded += Vm_EntryLoaded;
                return vm;
            });
        }

        private void Vm_EntryLoaded(EntryViewModel vm, Entry entry)
        {
        }
    }
}
