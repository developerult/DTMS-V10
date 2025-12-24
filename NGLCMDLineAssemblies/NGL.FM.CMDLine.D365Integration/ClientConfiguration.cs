using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGL.FM.CMDLine.D365Integration
{
    class ClientConfiguration
    {
        //public static ClientConfiguration Default
        //{
        //    get
        //    {
        //        return ClientConfiguration.OneBox;
        //    }
        //}
        //public static ClientConfiguration OneBox = new ClientConfiguration()
        //{


        //    UriString = System.Configuration.ConfigurationSettings.AppSettings["D365_URL"], // "https://ngl-dev015e64cfd23d55673cdevaos.axcloud.dynamics.com/",
        //    ActiveDirectoryResource = System.Configuration.ConfigurationSettings.AppSettings["ActiveDirectoryResource"], //"https://ngl-dev015e64cfd23d55673cdevaos.axcloud.dynamics.com",
        //    ActiveDirectoryTenant = System.Configuration.ConfigurationSettings.AppSettings["ActiveDirectoryTenant"], //"https://sts.windows.net/2518be7e-c933-4905-af64-24ad0157202f",
        //    ActiveDirectoryClientAppId = System.Configuration.ConfigurationSettings.AppSettings["ActiveDirectoryClientAppId"], //"f065bd6b-2455-4963-a00b-d205c095c461",
        //    ActiveDirectoryClientAppSecret = System.Configuration.ConfigurationSettings.AppSettings["ActiveDirectoryClientAppSecret"], //"0bF7Q~mPdDfMJfC9FkJf4N.fM5NTUrzVJaz45",

        //};
        public static string UriString { get; set; }
        public static string ActiveDirectoryResource { get; set; }
        public static String ActiveDirectoryTenant { get; set; }
        public static String ActiveDirectoryClientAppId { get; set; }
        public static String ActiveDirectoryClientAppSecret { get; set; }
    }
}
