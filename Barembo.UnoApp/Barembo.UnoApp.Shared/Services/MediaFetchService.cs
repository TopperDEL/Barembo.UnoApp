using Barembo.App.Core.Interfaces;
using Barembo.App.Core.Messages;
using Barembo.Models;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Barembo.UnoApp.Shared.Services
{
    public class MediaFetchService : IMediaFetchService
    {
        readonly IEventAggregator _eventAggregator;

        public MediaFetchService(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<MediaRequestedMessage>().Subscribe(HandleMediaRequested);
        }

        private async void HandleMediaRequested(MediaResult result)
        {
            var mediaData = await FetchMediaAsync();
        }

        public async Task<Tuple<Attachment, Stream>> FetchMediaAsync()
        {
            throw new NotImplementedException();
        }
    }
}
