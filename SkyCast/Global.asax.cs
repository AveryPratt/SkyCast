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
			//Utils.GetCert();
			//var kv = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(Utils.GetAccessToken));
			var kv = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(Utils.GetToken));
			
			AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
