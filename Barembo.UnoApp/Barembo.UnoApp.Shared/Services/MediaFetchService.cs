using Barembo.App.Core.Messages;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Barembo.UnoApp.Shared.Services
{
    public class MediaFetchService
    {
        readonly IEventAggregator _eventAggregator;

        public MediaFetchService(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<MediaRequestedMessage>().Subscribe(HandleMediaRequested);
        }

        private void HandleMediaRequested(MediaResult result)
        {

        }
    }
}
