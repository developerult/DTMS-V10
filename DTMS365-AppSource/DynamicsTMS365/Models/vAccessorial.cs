using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class vAccessorial
    {
        public int AccessorialCode { get; set; }
        public string AccessorialName { get; set; }
        public string AccessorialDescription { get; set; }
        public int AccessorialVariableCode { get; set; }
        public string AccessorialVariableCodesName { get; set; }
        public string AccessorialVariableCodesDescription { get; set; }
        public bool AccessorialVisible { get; set; }
        public bool AccessorialAutoApprove { get; set; }
        public bool AccessorialAllowCarrierUpdates { get; set; }
        public string AccessorialCaption { get; set; }
        public string AccessorialEDICode { get; set; }
        public bool AccessorialTaxable { get; set; }
        public bool AccessorialIsTax { get; set; }
        public int AccessorialTaxSortOrder { get; set; }
        public string AccessorialBOLText { get; set; }
        public string AccessorialBOLPlacement { get; set; }
        public int AccessorialAccessorialFeeAllocationTypeControl { get; set; }
        public string AccessorialFeeAllocationTypeName { get; set; }
        public string AccessorialFeeAllocationTypeDesc { get; set; }
        public int AccessorialTarBracketTypeControl { get; set; }
        public string TarBracketTypeName { get; set; }
        public string TarBracketTypeDesc { get; set; }
        public int AccessorialAccessorialFeeCalcTypeControl { get; set; }
        public string AccessorialFeeCalcTypeName { get; set; }
        public string AccessorialFeeCalcTypeDesc { get; set; }
        public bool AccessorialProfileSpecific { get; set; }
        public int AccessorialHDMControl { get; set; }
        public string HDMName { get; set; }
        public string HDMDesc { get; set; }      
        public decimal AccessorialMinimum { get; set; }
        public double AccessorialVariable { get; set; }
        public DateTime? AccessorialModDate { get; set; }
        public string AccessorialModUser { get; set; }

        private byte[] _AccessorialUpdated;
        public string AccessorialUpdated
        {
            get
            {
                if (this._AccessorialUpdated != null) { return Convert.ToBase64String(this._AccessorialUpdated); }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value)) { this._AccessorialUpdated = null; } else { this._AccessorialUpdated = Convert.FromBase64String(value); }
            }
        }

        public void setUpdated(byte[] val) { _AccessorialUpdated = val; }
        public byte[] getUpdated() { return _AccessorialUpdated; }

    }
}