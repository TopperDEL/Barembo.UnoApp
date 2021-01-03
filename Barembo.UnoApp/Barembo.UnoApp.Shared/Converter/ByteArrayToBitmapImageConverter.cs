using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace Barembo.UnoApp.Shared.Converter
{
    class ByteArrayToBitmapImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return null;

            BitmapImage img = new BitmapImage();
            using (MemoryStream memStream = new MemoryStream(value as byte[]))
            {
                img.SetSourceAsync(memStream.AsRandomAccessStream()); //ToDo: Do the conversion in the ViewModel
            }
            return img;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
