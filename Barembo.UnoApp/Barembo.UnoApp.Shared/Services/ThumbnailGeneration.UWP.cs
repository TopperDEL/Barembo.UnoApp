using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Barembo.UnoApp.Shared.Services
{
    //This file holds the UWP-specific implementations.
    //It is powered by FFMpeg and it's FrameGrabber and extracts the image from the stream.
    public partial class ThumbnailGeneration
    {
#if WINDOWS_UWP
        public static async Task<string> GenerateThumbnailBase64FromVideoAsync_UWP(Stream videoStream, float positionPercent, string filePath)
        {
            var grabber = await FFmpegInterop.FrameGrabber.CreateFromStreamAsync(videoStream.AsRandomAccessStream());
            var extractLocation = grabber.Duration * positionPercent;
            var frame = await grabber.ExtractVideoFrameAsync(extractLocation);
            using (var stream = new Windows.Storage.Streams.InMemoryRandomAccessStream())
            {
                await frame.EncodeAsJpegAsync(stream);
                stream.Seek(0);

                return Convert.ToBase64String(await ConvertToByteArray(stream));
            }
        }

        private static async Task<byte[]> ConvertToByteArray(Windows.Storage.Streams.IRandomAccessStream s)
        {
            var dr = new Windows.Storage.Streams.DataReader(s.GetInputStreamAt(0));
            var bytes = new byte[s.Size];
            await dr.LoadAsync((uint)s.Size);
            dr.ReadBytes(bytes);
            return bytes;
        }
#endif
    }
}
