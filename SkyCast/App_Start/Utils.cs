using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Security.Cryptography.X509Certificates;

namespace SkyCast.App_Start
{
	public class Utils
	{
		public static string EncryptSecret { get; set; }
		
		public static async Task<string> GetToken(string authority, string resource, string scope)
		{
			var authContext = new AuthenticationContext(authority);
			ClientCredential clientCred = new ClientCredential(WebConfigurationManager.AppSettings["ClientId"],
						WebConfigurationManager.AppSettings["ClientSecret"]);
			AuthenticationResult result = await authContext.AcquireTokenAsync(resource, clientCred);

			if (result == null)
				throw new InvalidOperationException("Failed to obtain the JWT token");

			return result.AccessToken;
		}

		public static ClientAssertionCertificate AssertionCert { get; set; }

		public static void GetCert()
		{
			var clientAssertionCertPfx = CertificateHelper.FindCertificateByThumbprint(WebConfigurationManager.AppSettings["thumbprint"]);
			AssertionCert = new ClientAssertionCertificate(WebConfigurationManager.AppSettings["clientid"], clientAssertionCertPfx);
		}

		public static async Task<string> GetAccessToken(string authority, string resource, string scope)
		{
			var context = new AuthenticationContext(authority, TokenCache.DefaultShared);
			var result = await context.AcquireTokenAsync(resource, AssertionCert);
			return result.AccessToken;
		}
	}
	public static class CertificateHelper
	{
		public static X509Certificate2 FindCertificateByThumbprint(string findValue)
		{
			X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
			try
			{
				store.Open(OpenFlags.ReadOnly);
				X509Certificate2Collection col = store.Certificates.Find(X509FindType.FindByThumbprint,
					findValue, false);
				if (col == null || col.Count == 0)
					return null;
				return col[0];
			}
			finally
			{
				store.Close();
			}
		}
	}
}