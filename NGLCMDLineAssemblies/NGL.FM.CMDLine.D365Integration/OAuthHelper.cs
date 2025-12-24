using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGL.FM.CMDLine.D365Integration
{
    class OAuthHelper
    {

        /// <summary>  
        /// The header to use for OAuth.  
        /// </summary>  
        public const string OAuthHeader = "Authorization";

        /// <summary>  
        /// retrieves an authentication header from the service.  
        /// </summary>  
        /// <returns>
        /// the authentication header for the Web API call.
        /// </returns>  
        public static string GetAuthenticationHeader(bool useWebAppAuthentication = false)
        {
            //string aadTenant = ClientConfiguration.Default.ActiveDirectoryTenant;
            //string aadClientAppId = ClientConfiguration.Default.ActiveDirectoryClientAppId;
            //string aadClientAppSecret = ClientConfiguration.Default.ActiveDirectoryClientAppSecret;
            //string aadResource = ClientConfiguration.Default.ActiveDirectoryResource;

            string aadTenant = ClientConfiguration.ActiveDirectoryTenant;
            string aadClientAppId = ClientConfiguration.ActiveDirectoryClientAppId;
            string aadClientAppSecret = ClientConfiguration.ActiveDirectoryClientAppSecret;
            string aadResource = ClientConfiguration.ActiveDirectoryResource;

            AuthenticationContext authenticationContext = new AuthenticationContext(aadTenant, false);
            AuthenticationResult authenticationResult = null;

            if (useWebAppAuthentication)
            {
                if (string.IsNullOrEmpty(aadClientAppSecret))
                {
                    Program.Log("Please fill AAD application secret in ClientConfiguration if you choose authentication by the application.");
                    
                    throw new Exception("Failed OAuth by empty application secret.");
                }

                try
                {
                    var creadential = new ClientCredential(aadClientAppId, aadClientAppSecret);
                    authenticationResult = authenticationContext.AcquireTokenAsync(aadResource, creadential).Result;
                }
                catch (Exception ex)
                {
                    Program.Log(string.Format("Failed to authenticate with AAD by application with exception {0} and the stack trace {1}", ex.ToString(), ex.StackTrace));
                    throw new Exception("Failed to authenticate with AAD by application.");
                }
            }
            return authenticationResult.CreateAuthorizationHeader();
        }

        /// <summary>  
        /// retrieves an authentication header from the service.  
        /// </summary>  
        /// <returns>
        /// AccessToken for the Web API call.
        /// </returns>  
        public static string GetAccessToken()
        {
            //string aadTenant = ClientConfiguration.Default.ActiveDirectoryTenant;
            //string aadClientAppId = ClientConfiguration.Default.ActiveDirectoryClientAppId;
            //string aadClientAppSecret = ClientConfiguration.Default.ActiveDirectoryClientAppSecret;
            //string aadResource = ClientConfiguration.Default.ActiveDirectoryResource;

            string aadTenant = ClientConfiguration.ActiveDirectoryTenant;
            string aadClientAppId = ClientConfiguration.ActiveDirectoryClientAppId;
            string aadClientAppSecret = ClientConfiguration.ActiveDirectoryClientAppSecret;
            string aadResource = ClientConfiguration.ActiveDirectoryResource;

            AuthenticationContext authenticationContext = new AuthenticationContext(aadTenant, false);
            AuthenticationResult authenticationResult = null;

            if (string.IsNullOrEmpty(aadClientAppSecret))
            {
                Program.Log("Please fill AAD application secret in ClientConfiguration if you choose authentication by the application.");
                throw new Exception("Failed OAuth by empty application secret.");
            }

            try
            {
                var creadential = new ClientCredential(aadClientAppId, aadClientAppSecret);
                authenticationResult = authenticationContext.AcquireTokenAsync(aadResource, creadential).Result;
            }
            catch (Exception ex)
            {
                Program.Log(string.Format("Failed to authenticate with AAD by application with exception {0} and the stack trace {1}", ex.ToString(), ex.StackTrace));
                throw new Exception("Failed to authenticate with AAD by application.");
            }

            return authenticationResult.AccessToken;
        }
    }
}
