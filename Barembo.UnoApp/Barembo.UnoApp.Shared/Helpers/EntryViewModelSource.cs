﻿using Barembo.Interfaces;
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
        readonly List<EntryViewModel> _viewModels = new List<EntryViewModel>();

        public EntryViewModelSource(IEntryService entryService, BookReference bookReference)
        {
            _entryService = entryService;
            _bookReference = bookReference;
        }
        public Windows.UI.Core.CoreDispatcher Dispatcher { get; set; }
        
        public void Unload()
        {
            _viewModels.Clear();
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
            await InitAsync().ConfigureAwait(false);

            var paged = (from e in _entryReferences
                         select e).Skip(pageIndex * pageSize).Take(pageSize);

            return paged.Select(s =>
            {
                var vm = new EntryViewModel(s, _entryService, SynchronizationContext.Current);
                vm.EntryLoaded += Vm_EntryLoaded;
                return vm;
            });
        }

        private async void Vm_EntryLoaded(EntryViewModel vm, Entry entry)
        {
            vm.EntryLoaded -= Vm_EntryLoaded;
            await vm.LoadAttachmentPreviewsAsync();
            _viewModels.Add(vm);
        }
    }
}
