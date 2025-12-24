using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class User
    {
        public int UserSecurityControl { get; set; }

        public string UserEmail { get; set; }

        public string UserName { get; set; }

        public string UserFriendlyName { get; set; }

        public string UserFirstName { get; set; }

        public string UserLastName { get; set; }

        public string UserTitle { get; set; }

        public string UserDepartment { get; set; }

        public string UserPhoneCell { get; set; }

        public string UserPhoneHome { get; set; }

        public string UserPhoneWork { get; set; }

        public string UserPhoneWorkExt { get; set; }

        public string UserTheme365 { get; set; }

        public DateTime? UserStartFreeTrial { get; set; }

        public DateTime? UserEndFreeTrial { get; set; }

        public bool UserFreeTrialActive { get; set; }


        //Added By LVV on 4/3/18 for v-8.0 TMS 365
        public int UserUserGroupsControl { get; set; }
        public int LEAControl { get; set; }
        public bool UseMicrosoftAccount { get; set; }
        public bool AllowNGLAPI { get; set; }
        public string AccountGroup { get; set; }

        public bool AutoGeneratePwd { get; set; }
        public bool SendUserPwd { get; set; }
        public string Pwd { get; set; }

        public bool blnIsCarrierUser { get; set; }
        public vUserSecurityCarrier[] AssociatedCarriers { get; set; }
        public string UserCultureInfo { get; set; }
        public string UserTimeZone { get; set; }
    }

    public class userNewPassword
    {
        public string currentPassword { get; set; }

        public string newPassword { get; set; }
    }
}