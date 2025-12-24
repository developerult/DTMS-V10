using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class SystemInfo
    {
        //Client Software Version
        public string CurrentClientSoftwareRelease { get; set; }
        public string LastClientModified { get; set; }
        public string AuthNumber { get; set; }

        //Server Software Version
        public string CurrentServerSoftwareRelease { get; set; } //dbo.getMaxVersion() AS version,
        public string LastServerSoftwareModified { get; set; } //(Select top 1[VersionDate] From dbo.Version Order By VersionDate Desc) as ServerLastMod //about.ServerLastMod.Value & " " & about.ServerLastMod.Value.ToLongTimeString
        public string Database { get; set; }
        public string Server { get; set; }

        //Extra Fields (for future maybe?)
        public string AuthKey { get; set; }
        public string AuthName { get; set; }
        public string AuthAddress { get; set; }
        public string AuthCity { get; set; }
        public string AuthState { get; set; }
        public string AuthZip { get; set; }

        public string XactVersion { get; set; } // dbo.getMaxxactVersion() AS xactversion
        public string MasVersion { get; set; } // dbo.getMaxmasVersion() AS masversion
        public string ClaimVersion { get; set; } // dbo.getMaxclaimVersion() AS claimversion

    }
}