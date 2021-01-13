using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Barembo.UnoApp.Shared.Services
{
    //This files holds the Android-speicific implementation.
    //It is powered by the Android-internals and fetches it's thumbnails from the file-path instead of the stream.
    public partial class ThumbnailGeneration
    {
#if __DROID__
        static ThumbnailGeneration()
        {
            LibVLCSharp.Shared.Core.Initialize();
        }

        public static async Task<string> GenerateThumbnailBase64FromVideoAsync_Droid(Stream videoStream, float positionPercent, string filePath, Barembo.Interfaces.IThumbnailGeneratorService thumbnailGenerator)
        {
            Android.Media.MediaMetadataRetriever retriever = new Android.Media.MediaMetadataRetriever();
            await retriever.SetDataSourceAsync(filePath);

            var length = retriever.ExtractMetadata(Android.Media.MetadataKey.Duration);
            var playLength = Convert.ToInt32(length);
            var extractLocation = playLength * positionPercent * 1000; //GetFrameAtTime is in Microseconds instead of Milliseconds

            var frame = retriever.GetFrameAtTime((long)extractLocation, Android.Media.Option.ClosestSync);

            using (MemoryStream finalStream = new MemoryStream())
            {
                await frame.CompressAsync(Android.Graphics.Bitmap.CompressFormat.Jpeg, 100, finalStream);

                finalStream.Position = 0;

                return await thumbnailGenerator.GenerateThumbnailBase64FromImageAsync(finalStream);
            }
        }
#endif
    }
}
