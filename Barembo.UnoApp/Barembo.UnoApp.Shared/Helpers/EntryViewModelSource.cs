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

namespace Barembo.UnoApp.Shared.Helpers
{
    public class EntryViewModelSource : IIncrementalSource<EntryViewModel>
    {
        private IEnumerable<EntryReference> _entryReferences;
        private readonly IEntryService _entryService;
        private readonly BookReference _bookReference;
        private bool _loaded;

        public EntryViewModelSource(IEntryService entryService, BookReference bookReference)
        {
            _entryService = entryService;
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

            return paged.Select(s => new EntryViewModel(s, _entryService, SynchronizationContext.Current));
        }
    }
}
