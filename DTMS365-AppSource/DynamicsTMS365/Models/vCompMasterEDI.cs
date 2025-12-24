using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class vCompMasterEDI
    {
        public int CompMasterEDIControl { get; set; }
        public string CompMasterEDIXaction { get; set; }
        public string CompMasterEDIComment { get; set; }
        public string CompMasterEDISecurityQual { get; set; }
        public string CompMasterEDISecurityCode { get; set; }
        public string CompMasterEDIPartnerQual { get; set; }
        public string CompMasterEDIPartnerCode { get; set; }
        public int CompMasterEDIISASequence { get; set; }
        public int CompMasterEDISequence { get; set; }
        public bool CompMasterEDIEmailNotificationOn { get; set; }
        public string CompMasterEDIEmailAddress { get; set; }
        public bool CompMasterEDIAcknowledgementRequested { get; set; }
        public bool CompMasterEDIAcceptOn997 { get; set; }
        public string CompMasterEDIMethodOfPayment { get; set; }
        public string CompMasterEDIModUser { get; set; }
        public DateTime? CompMasterEDIModDate { get; set; }
        public byte[] _CompMasterEDIUpdated { get; set; }
        public string CompMasterEDIUpdated
        {
            get
            {
                if (this._CompMasterEDIUpdated != null)
                {

                    return Convert.ToBase64String(this._CompMasterEDIUpdated);

                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._CompMasterEDIUpdated = null;

                }

                else
                {

                    this._CompMasterEDIUpdated = Convert.FromBase64String(value);

                }

            }
        }
        public void setUpdated(byte[] val) { _CompMasterEDIUpdated = val; }
        public byte[] getUpdated() { return _CompMasterEDIUpdated; }

    }
}