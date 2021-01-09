using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Barembo.UnoApp.Shared.Services
{
    public partial class ThumbnailGeneration
    {
#if __DROID__
        public static async Task<string> GenerateThumbnailBase64FromVideoAsync_Droid(Stream videoStream, float positionPercent, string filePath)
        {
            Android.Media.MediaMetadataRetriever retriever = new Android.Media.MediaMetadataRetriever();
            await retriever.SetDataSourceAsync(filePath);

            var frame = retriever.GetFrameAtTime(1); //ToDo
            using (MemoryStream finalStream = new MemoryStream())
            {
                await frame.CompressAsync(Android.Graphics.Bitmap.CompressFormat.Jpeg, 100, finalStream);

                return Convert.ToBase64String(finalStream.ToArray());
            }
        }
#endif
    }
}
