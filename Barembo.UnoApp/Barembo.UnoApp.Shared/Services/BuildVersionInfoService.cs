using Barembo.App.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace Barembo.UnoApp.Shared.Services
{
    public class BuildVersionInfoService : IBuildVersionInfoService
    {
        public string GetBaremboVersion()
        {
            return VersionTracking.CurrentVersion + " - " + VersionTracking.CurrentBuild;
        }
    }
}
