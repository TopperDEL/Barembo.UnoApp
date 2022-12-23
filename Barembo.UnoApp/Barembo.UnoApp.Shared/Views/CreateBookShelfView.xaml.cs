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

// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace Barembo.UnoApp.Shared.Views
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class CreateBookShelfView : Page, INavigationAware
    {
        public CreateBookShelfView()
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
            ((CreateBookShelfViewModel)this.DataContext).Init((StoreAccess)navigationContext.Parameters["StoreAccess"]);
            var error = navigationContext.Parameters["Error"] as string;
            if(!string.IsNullOrEmpty(error))
            {
                ErrorText.Text = error;
            }
        }
    }
}
