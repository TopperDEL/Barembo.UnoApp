using Barembo.App.Core.ViewModels;
using Barembo.Models;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Barembo.UnoApp.Shared.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ShowBookShareView : Page, INavigationAware
    {
        public ShowBookShareView()
        {
            this.InitializeComponent();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            //Nothing to do here
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var vm = (ShowBookShareViewModel)this.DataContext;
            vm.Init((BookShareReference)navigationContext.Parameters["BookShareReference"]);
        }
    }
}
