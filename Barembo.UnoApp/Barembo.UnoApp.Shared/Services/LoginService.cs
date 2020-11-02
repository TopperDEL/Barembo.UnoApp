using Barembo.App.Core.Interfaces;
using Barembo.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.Security.Credentials;

namespace Barembo.UnoApp.Shared.Services
{
    public class LoginService : ILoginService
    {
        private const string BAREMBO_RESOURCE = "BAREMBO";
        private const string ACCESS_GRANT = "ACCESS_GRANT";

        private PasswordVault _vault = new PasswordVault();

        public bool GetIsLoggedIn()
        {
            try
            {
                var access = GetLogin();
                if (!string.IsNullOrEmpty(access.AccessGrant))
                    return true;
            }
            catch { }
            return false;
        }

        public StoreAccess GetLogin()
        {
            return new StoreAccess(_vault.Retrieve(BAREMBO_RESOURCE, ACCESS_GRANT)?.Password);
        }

        public bool Login(StoreAccess storeAccess)
        {
            //ToDo: Verify if access is valid

            //Save access grant to vault
            _vault.Add(new PasswordCredential(BAREMBO_RESOURCE, ACCESS_GRANT, storeAccess.AccessGrant));

            return true;
        }

        public void Logout()
        {
            var list = _vault.FindAllByResource(BAREMBO_RESOURCE);
            foreach (var entry in list)
                _vault.Remove(entry);
        }
    }
}
