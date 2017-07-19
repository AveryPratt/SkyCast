using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.Azure.KeyVault;
using System.Web.Configuration;
using SkyCast.App_Start;

namespace SkyCast
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
		{
			// I put my GetToken method in a Utils class. Change for wherever you placed your method.
			var kv = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(Utils.GetToken));

			var sec = kv.GetSecretAsync(WebConfigurationManager.AppSettings["SecretUri"]).Result;

			//I put a variable in a Utils class to hold the secret for general  application use.
			Utils.EncryptSecret = sec.Value;
			AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
