﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Barembo.UnoApp.Shared.Services
{
    public partial class ThumbnailGeneration
    {
#if __IOS__
        public static async Task<string> GenerateThumbnailBase64FromVideoAsync_iOs(Stream videoStream, float positionPercent, string filePath)
        {
            try
            {
                CoreMedia.CMTime actualTime;
                Foundation.NSError outError;
                using (var asset = AVFoundation.AVAsset.FromUrl(Foundation.NSUrl.FromFilename(filePath)))
                using (var imageGen = new AVFoundation.AVAssetImageGenerator(asset))
                {
                    imageGen.AppliesPreferredTrackTransform = true; //To rotate the image like the video was recorded
                    var frameAt = asset.Duration * positionPercent;
                    using (var imageRef = imageGen.CopyCGImageAtTime(frameAt, out actualTime, out outError))
                    {
                        return UIKit.UIImage.FromImage(imageRef).AsJPEG().GetBase64EncodedString(Foundation.NSDataBase64EncodingOptions.None);
                    }
                }
            }
            catch
            {
            }
            return null;
        }
#endif
    }
}
