#if __DROID__
using Android.Graphics;
#endif
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
            var retriever = new Android.Media.MediaMetadataRetriever();
            await retriever.SetDataSourceAsync(filePath);

            var length = retriever.ExtractMetadata(Android.Media.MetadataKey.Duration);
            var playLength = Convert.ToInt32(length);
            var extractLocation = playLength * positionPercent * 1000; //GetFrameAtTime is in Microseconds instead of Milliseconds
            
            var frame = retriever.GetFrameAtTime((long)extractLocation, Android.Media.Option.ClosestSync);
            using (MemoryStream finalStream = new MemoryStream())
            {
                frame = Bitmap.CreateScaledBitmap(frame, 600, 450, false);
                await frame.CompressAsync(Android.Graphics.Bitmap.CompressFormat.Jpeg, 100, finalStream);

                finalStream.Position = 0;

                return Convert.ToBase64String(finalStream.ToArray());
            }
        }

        public static async Task<string> GenerateThumbnailBase64FromImageAsync_Droid(Stream image, int width, int height)
        {
            image.Position = 0;
            Bitmap originalImage = await BitmapFactory.DecodeStreamAsync(image);

            float newHeight;
            float newWidth;

            var originalHeight = originalImage.Height;
            var originalWidth = originalImage.Width;

            if (originalHeight > originalWidth)
            {
                newHeight = height;
                float ratio = originalHeight / height;
                newWidth = originalWidth / ratio;
            }
            else
            {
                newWidth = width;
                float ratio = originalWidth / width;
                newHeight = originalHeight / ratio;
            }

            Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, (int)newWidth, (int)newHeight, false);

            originalImage.Recycle();

            using (MemoryStream ms = new MemoryStream())
            {
                await resizedImage.CompressAsync(Bitmap.CompressFormat.Png, 100, ms);

                resizedImage.Recycle();

                return Convert.ToBase64String(ms.ToArray());
            }
        }
#endif
    }
}
