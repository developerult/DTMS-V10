using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class vLECompAccessorial
    {
        public int LECompAccessorialControl { get; set; }
        public int LECompAccessorialLEAControl { get; set; }
        public string LEAdminLegalEntity { get; set; }
        public int LECompAccessorialCompControl { get; set; }
        public string CompName { get; set; }
        public string CompNumber { get; set; }
        public int LECompAccessorialCode { get; set; }
        public string AccessorialName { get; set; }
        public string AccessorialDescription { get; set; }
        public int LECompAccessorialVariableCode { get; set; }
        public string AccessorialVariableCodesName { get; set; }
        public string AccessorialVariableCodesDescription { get; set; }
        public bool LECompAccessorialVisible { get; set; }
        public bool LECompAccessorialAutoApprove { get; set; }
        public bool LECompAccessorialAllowCarrierUpdates { get; set; }
        public string LECompAccessorialCaption { get; set; }
        public string LECompAccessorialEDICode { get; set; }
        public bool LECompAccessorialTaxable { get; set; }
        public bool LECompAccessorialIsTax { get; set; }
        public int LECompAccessorialTaxSortOrder { get; set; }
        public string LECompAccessorialBOLText { get; set; }
        public string LECompAccessorialBOLPlacement { get; set; }
        public int LECompAccessorialAccessorialFeeAllocationTypeControl { get; set; }
        public string AccessorialFeeAllocationTypeName { get; set; }
        public string AccessorialFeeAllocationTypeDesc { get; set; }
        public int LECompAccessorialTarBracketTypeControl { get; set; }
        public string TarBracketTypeName { get; set; }
        public string TarBracketTypeDesc { get; set; }
        public int LECompAccessorialAccessorialFeeCalcTypeControl { get; set; }
        public string AccessorialFeeCalcTypeName { get; set; }
        public string AccessorialFeeCalcTypeDesc { get; set; }
        public bool LECompAccessorialProfileSpecific { get; set; }
        public int LECompAccessorialHDMControl { get; set; }
        public string HDMName { get; set; }
        public string HDMDesc { get; set; }
        public decimal LECompAccessorialMinimum { get; set; }
        public double LECompAccessorialVariable { get; set; }
        public DateTime? LECompAccessorialModDate { get; set; }
        public string LECompAccessorialModUser { get; set; }

        private byte[] _LECompAccessorialUpdated;
        public string LECompAccessorialUpdated
        {
            get
            {
                if (this._LECompAccessorialUpdated != null) { return Convert.ToBase64String(this._LECompAccessorialUpdated); }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value)) { this._LECompAccessorialUpdated = null; } else { this._LECompAccessorialUpdated = Convert.FromBase64String(value); }
            }
        }

        public void setUpdated(byte[] val) { _LECompAccessorialUpdated = val; }
        public byte[] getUpdated() { return _LECompAccessorialUpdated; }

    }
}