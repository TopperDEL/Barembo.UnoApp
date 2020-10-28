using Barembo.App.Core.Interfaces;
using Barembo.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Barembo.UnoApp.Shared.Services
{
    public class LoginService : ILoginService
    {
        public bool GetIsLoggedIn()
        {
            return false;
        }

        public StoreAccess GetLogin()
        {
            throw new NotImplementedException();
        }

        public bool Login(StoreAccess storeAccess)
        {
            return true;
        }

        public void Logout()
        {
            throw new NotImplementedException();
        }
    }
}
