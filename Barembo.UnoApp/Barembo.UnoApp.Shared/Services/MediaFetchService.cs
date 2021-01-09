using Barembo.App.Core.Interfaces;
using Barembo.App.Core.Messages;
using Barembo.Models;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

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

        private async void HandleMediaRequested()
        {
            try
            {
                var tempFile = Path.GetTempFileName();

                var mediaData = await FetchVideoAsync();

                if (mediaData != null)
                {
                    _eventAggregator.GetEvent<MediaReceivedMessage>().Publish(mediaData);
                }
            }
            catch(Exception)
            {
            }
        }

        public async Task<MediaData> FetchMediaAsync()
        {
            try
            {
                var photo = await MediaPicker.PickPhotoAsync();
                if(photo != null)
                {
                    MediaData mediaData = new MediaData();
                    mediaData.Stream = await photo.OpenReadAsync();
                    mediaData.Attachment = new Attachment();
                    mediaData.Attachment.Type = AttachmentType.Image;
                    mediaData.Attachment.Size = mediaData.Stream.Length;
                    mediaData.Attachment.FileName = photo.FileName;
                    mediaData.FilePath = photo.FullPath;
                    return mediaData;
                }
            }
            catch (Exception)
            {
            }

            return null;
        }

        public async Task<MediaData> FetchVideoAsync()
        {
            try
            {
                var video = await MediaPicker.PickVideoAsync();
                if (video != null)
                {
                    MediaData mediaData = new MediaData();
                    mediaData.Stream = await video.OpenReadAsync();
                    mediaData.Attachment = new Attachment();
                    mediaData.Attachment.Type = AttachmentType.Video;
                    mediaData.Attachment.Size = mediaData.Stream.Length;
                    mediaData.Attachment.FileName = video.FileName;
                    mediaData.FilePath = video.FullPath;
                    return mediaData;
                }
            }
            catch (Exception)
            {
            }

            return null;
        }
    }
}