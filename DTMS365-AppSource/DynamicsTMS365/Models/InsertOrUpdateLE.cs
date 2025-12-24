using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class InsertOrUpdateLE
    {
        //LE fields
        public int LEAdminControl { get; set; }
        public string LegalEntity { get; set; }
        public string LEAdminCNSPrefix { get; set; }
        public int LEAdminCNSNumberLow { get; set; }
        public int LEAdminCNSNumberHigh { get; set; }
        public int LEAdminCNSNumber { get; set; }
        public int LEAdminPRONumber { get; set; }
        public bool LEAdminAllowCreateOrderSeq { get; set; }
        public int LEAdminAutoAssignOrderSeqSeed { get; set; }
        //Modified by RHR for v-8.1.1.1 on 05/10/2018
        public string LEAdminBOLLegalText { get; set; }
        public string LEAdminDispatchLegalText { get; set; }
        public bool LEAdminCarApptAutomation { get; set; } //default off(0)
        public int LEAdminApptModCutOffMinutes { get; set; } //default = to 48 hours stored as 2880  minutes
        public string LEAdminDefaultLastLoadTime { get; set; } //default = 15:00	equal to 3 pm
        public int LEAdminApptNotSetAlertMinutes { get; set; } //default = to 48 hours stored as 2880  minutes
        public bool LEAdminAllowApptEdit { get; set; } //default off(0)
        public bool LEAdminAllowApptDelete { get; set; } //default off(0)
        public int LEAdminCarrierAcceptLoadMins { get; set; }
        public string LEAdminExpiredLoadsTo { get; set; }
        public string LEAdminExpiredLoadsCc { get; set; }
        public int? LEAdminSecurityLevel { get; set; }


        //Comp fields
        public int CompControl { get; set; }
        public bool CompActive { get; set; }
        public int CompNumber { get; set; }
        public string CompName { get; set; }
        public string CompAlphaCode { get; set; }
        public string CompAbrev { get; set; }
        public string CompWebsite { get; set; }
        public string CompEmail { get; set; }
        public string CompAddress1 { get; set; }
        public string CompAddress2 { get; set; }
        public string CompAddress3 { get; set; }
        public string CompCity { get; set; }
        public string CompState { get; set; }
        public string CompZip { get; set; }
        public string CompCountry { get; set; }

        //CompCont fields
        public string CompContName { get; set; }
        public string CompContTitle { get; set; }
        public string CompCont800 { get; set; }
        public string CompContPhone { get; set; }
        public string CompContPhoneExt { get; set; }
        public string CompContFax { get; set; }
        public string CompContEmail { get; set; }
        public bool CompContTender { get; set; }





    }
}