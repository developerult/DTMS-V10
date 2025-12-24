using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class CompEDI
    {
            
        public Comp Comp { get; set; }
        public bool CompEDIAcceptOn997 { get; set; }
        public bool CompEDIAcknowledgementRequested { get; set; }
        public string CompEDIComment { get; set; }
        public int CompEDICompControl { get; set; }
        public int CompEDIControl { get; set; }
        public string CompEDIEmailAddress { get; set; }
        public bool CompEDIEmailNotificationOn { get; set; }
        public int CompEDIISASequence { get; set; }
        public string CompEDIMethodOfPayment { get; set; }
        public DateTime? CompEDIModDate { get; set; }
        public string CompEDIModUser { get; set; }
        public string CompEDIPartnerCode { get; set; }
        public string CompEDIPartnerQual { get; set; }
        public string CompEDISecurityCode { get; set; }
        public string CompEDISecurityQual { get; set; }
        public int CompEDISequence { get; set; }
        public byte[] _CompEDIUpdated;
        public string CompEDIXaction { get; set; }

        public string CompEDIUpdated
        {
            get
            {
                if (this._CompEDIUpdated != null)
                {

                    return Convert.ToBase64String(this._CompEDIUpdated);

                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._CompEDIUpdated = null;

                }

                else
                {

                    this._CompEDIUpdated = Convert.FromBase64String(value);

                }

            }
        }

        public void setUpdated(byte[] val) { _CompEDIUpdated = val; }
        public byte[] getUpdated() { return _CompEDIUpdated; }
    }
}