using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Die Elementvorlage "Benutzersteuerelement" wird unter https://go.microsoft.com/fwlink/?LinkId=234236 dokumentiert.

namespace Barembo.UnoApp.Shared.Controls
{
    public sealed partial class VideoPreviewControl : UserControl
    {
        readonly Task _updatePreviewTask;
        private bool _shouldStopPreview;

        public VideoPreviewControl()
        {
            this.InitializeComponent();

            _updatePreviewTask = Task.Factory.StartNew(UpdatePreview, this);

            this.Unloaded += VideoPreviewControl_Unloaded;
        }

        private void VideoPreviewControl_Unloaded(object sender, RoutedEventArgs e)
        {
            _shouldStopPreview = true;
            _updatePreviewTask.Dispose();
            
            this.Unloaded -= VideoPreviewControl_Unloaded;
        }

        private async Task UpdatePreview(object target)
        {
            int number = 1;
            VideoPreviewControl targetControl = target as VideoPreviewControl;
            while (!_shouldStopPreview)
            {
                await Dispatcher.RunIdleAsync((_) => targetControl.SetImage(number++));
                await Task.Delay(300);
                if (number > 6)
                {
                    number = 1;
                }
            }
        }

        public void SetImage(int number)
        {
            ShowImage1 = Visibility.Collapsed;
            ShowImage2 = Visibility.Collapsed;
            ShowImage3 = Visibility.Collapsed;
            ShowImage4 = Visibility.Collapsed;
            ShowImage5 = Visibility.Collapsed;
            ShowImage6 = Visibility.Collapsed;
            if (number == 1)
            {
                ShowImage1 = Visibility.Visible;
            }
            else if (number == 2)
            {
                ShowImage2 = Visibility.Visible;
            }
            else if (number == 3)
            {
                ShowImage3 = Visibility.Visible;
            }
            else if (number == 4)
            {
                ShowImage4 = Visibility.Visible;
            }
            else if (number == 5)
            {
                ShowImage5 = Visibility.Visible;
            }
            else if (number == 6)
            {
                ShowImage6 = Visibility.Visible;
            }
        }



        public Visibility ShowImage1
        {
            get { return (Visibility)GetValue(ShowImage1Property); }
            set { SetValue(ShowImage1Property, value); }
        }
        public static readonly DependencyProperty ShowImage1Property =
            DependencyProperty.Register("ShowImage1", typeof(Visibility), typeof(VideoPreviewControl), new PropertyMetadata(null));

        public Visibility ShowImage2
        {
            get { return (Visibility)GetValue(ShowImage2Property); }
            set { SetValue(ShowImage2Property, value); }
        }
        public static readonly DependencyProperty ShowImage2Property =
            DependencyProperty.Register("ShowImage2", typeof(Visibility), typeof(VideoPreviewControl), new PropertyMetadata(null));

        public Visibility ShowImage3
        {
            get { return (Visibility)GetValue(ShowImage3Property); }
            set { SetValue(ShowImage3Property, value); }
        }
        public static readonly DependencyProperty ShowImage3Property =
            DependencyProperty.Register("ShowImage3", typeof(Visibility), typeof(VideoPreviewControl), new PropertyMetadata(null));

        public Visibility ShowImage4
        {
            get { return (Visibility)GetValue(ShowImage4Property); }
            set { SetValue(ShowImage4Property, value); }
        }
        public static readonly DependencyProperty ShowImage4Property =
            DependencyProperty.Register("ShowImage4", typeof(Visibility), typeof(VideoPreviewControl), new PropertyMetadata(null));

        public Visibility ShowImage5
        {
            get { return (Visibility)GetValue(ShowImage5Property); }
            set { SetValue(ShowImage5Property, value); }
        }
        public static readonly DependencyProperty ShowImage5Property =
            DependencyProperty.Register("ShowImage5", typeof(Visibility), typeof(VideoPreviewControl), new PropertyMetadata(null));

        public Visibility ShowImage6
        {
            get { return (Visibility)GetValue(ShowImage6Property); }
            set { SetValue(ShowImage6Property, value); }
        }
        public static readonly DependencyProperty ShowImage6Property =
            DependencyProperty.Register("ShowImage6", typeof(Visibility), typeof(VideoPreviewControl), new PropertyMetadata(null));

        public ImageSource Image1
        {
            get { return (ImageSource)GetValue(Image1Property); }
            set { SetValue(Image1Property, value); }
        }
        public static readonly DependencyProperty Image1Property =
            DependencyProperty.Register("Image1", typeof(ImageSource), typeof(VideoPreviewControl), new PropertyMetadata(null));

        public ImageSource Image2
        {
            get { return (ImageSource)GetValue(Image2Property); }
            set { SetValue(Image2Property, value); }
        }
        public static readonly DependencyProperty Image2Property =
            DependencyProperty.Register("Image2", typeof(ImageSource), typeof(VideoPreviewControl), new PropertyMetadata(null));

        public ImageSource Image3
        {
            get { return (ImageSource)GetValue(Image3Property); }
            set { SetValue(Image3Property, value); }
        }
        public static readonly DependencyProperty Image3Property =
            DependencyProperty.Register("Image3", typeof(ImageSource), typeof(VideoPreviewControl), new PropertyMetadata(null));

        public ImageSource Image4
        {
            get { return (ImageSource)GetValue(Image4Property); }
            set { SetValue(Image4Property, value); }
        }
        public static readonly DependencyProperty Image4Property =
            DependencyProperty.Register("Image4", typeof(ImageSource), typeof(VideoPreviewControl), new PropertyMetadata(null));

        public ImageSource Image5
        {
            get { return (ImageSource)GetValue(Image5Property); }
            set { SetValue(Image5Property, value); }
        }
        public static readonly DependencyProperty Image5Property =
            DependencyProperty.Register("Image5", typeof(ImageSource), typeof(VideoPreviewControl), new PropertyMetadata(null));

        public ImageSource Image6
        {
            get { return (ImageSource)GetValue(Image6Property); }
            set { SetValue(Image6Property, value); }
        }
        public static readonly DependencyProperty Image6Property =
            DependencyProperty.Register("Image6", typeof(ImageSource), typeof(VideoPreviewControl), new PropertyMetadata(null));
    }
}
