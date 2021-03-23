using Barembo.App.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Barembo.UnoApp.Shared.Services
{
    public class QRCodeScannerService : IQRCodeScannerService
    {
#if WINDOWS_UWP
        public static Windows.UI.Core.CoreDispatcher _dispatcher;
        public static object _rootFrame;
#endif
        public async Task<string> ScanQRCodeAsync()
        {
#if WINDOWS_UWP
            var scanner = new ZXing.Mobile.MobileBarcodeScanner(_dispatcher);
            scanner.RootFrame = _rootFrame as Windows.UI.Xaml.Controls.Frame;
#else
            var scanner = new ZXing.Mobile.MobileBarcodeScanner();
#endif

            var result = await scanner.Scan();

            return result.Text;
        }
    }
}
