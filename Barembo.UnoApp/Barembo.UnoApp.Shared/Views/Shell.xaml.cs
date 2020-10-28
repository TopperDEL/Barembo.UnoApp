using Barembo.App.Core.Interfaces;
using Barembo.Models;
using Prism;
using Prism.DryIoc;
using Prism.Events;
using Prism.Ioc;
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

namespace Barembo.UnoApp.Shared.Views
{
    public sealed partial class Shell
    {
        readonly IRegionManager _regionManager;
        readonly IEventAggregator _eventAggregator;

        public Shell(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            this.InitializeComponent();

            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<Barembo.App.Core.Messages.SuccessfullyLoggedInMessage>().Subscribe(NavigateToBookShelfView);
        }

        private void ContentControl_Loaded(object sender, RoutedEventArgs e)
        {
            _regionManager.RegisterViewWithRegion("ContentRegion", typeof(LoginView));
        }

        private void NavigateToBookShelfView(StoreAccess storeAccess)
        {

        }
    }
}
