using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class Claim
    {
        public string CarrierAlphaCode { get; set; }
        public string CarrierName { get; set; }
        public int? CarrierNumber { get; set; }
        public string ClaimBOLIssueby { get; set; }
        public DateTime? ClaimBOLIssueDate { get; set; }
        public string ClaimCarrierContact { get; set; }
        public string ClaimCarrierContactPhone { get; set; }
        public int? ClaimCarrierControl { get; set; }
        public decimal? ClaimCheckAmt { get; set; }
        public string ClaimCheckNo { get; set; }
        public decimal? ClaimClaimAmt { get; set; }
        public string ClaimConnLine { get; set; }
        public string ClaimConsAddress1 { get; set; }
        public string ClaimConsAddress2 { get; set; }
        public string ClaimConsAddress3 { get; set; }
        public string ClaimConsCity { get; set; }
        public int? ClaimConsCompControl { get; set; }
        public string ClaimConsCountry { get; set; }
        public string ClaimConsFax { get; set; }
        public string ClaimConsName { get; set; }
        public string ClaimConsPhone { get; set; }
        public string ClaimConsState { get; set; }
        public string ClaimConsZip { get; set; }
        public int ClaimControl { get; set; }
        public int ClaimCustCompControl { get; set; }
        public DateTime? ClaimDateAck { get; set; }
        public DateTime? ClaimDatePaid { get; set; }
        public DateTime? ClaimDateSubm { get; set; }
        public bool ClaimDeclined { get; set; }
        public decimal? ClaimDeclinedAmt { get; set; }
        public string ClaimDeclinedByCarrRep { get; set; }
        public DateTime? ClaimDeclinedDate { get; set; }
        public string ClaimDeclinedReason { get; set; }
        public decimal? ClaimDiff { get; set; }
        public string ClaimFB { get; set; }
        public string ClaimFinalDest { get; set; }
        public string ClaimInvName { get; set; }
        public string ClaimInvPhone { get; set; }
        public DateTime? ClaimModDate { get; set; }
        public string ClaimModUser { get; set; }
        public string ClaimOrderNumber { get; set; }
        public string ClaimProNumber { get; set; }
        public string ClaimRemark { get; set; }
        public string ClaimShipDesc { get; set; }
        public string ClaimShipFrom { get; set; }
        public string ClaimShipTo { get; set; }
        public string ClaimTruckNo { get; set; }      
        public string ClaimVendAddress1 { get; set; }
        public string ClaimVendAddress2 { get; set; }
        public string ClaimVendAddress3 { get; set; }
        public string ClaimVendCity { get; set; }
        public int? ClaimVendCompControl { get; set; }
        public string ClaimVendCountry { get; set; }
        public string ClaimVendFax { get; set; }
        public string ClaimVendName { get; set; }
        public string ClaimVendPhone { get; set; }
        public string ClaimVendState { get; set; }
        public string ClaimVendZip { get; set; }
        public string ClaimVia { get; set; }
        public string CompAlphaCode { get; set; }
        public string CompLegalEntity { get; set; }
        public string CompName { get; set; }
        public int? CompNumber { get; set; }
        public int LEAdminControl { get; set; }
        public byte[] _ClaimUpdated { get; set; }
        public string ClaimUpdated
        {
            get
            {
                if (this._ClaimUpdated != null)
                {

                    return Convert.ToBase64String(this._ClaimUpdated);

                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._ClaimUpdated = null;

                }

                else
                {

                    this._ClaimUpdated = Convert.FromBase64String(value);

                }

            }
        }

        public void setUpdated(byte[] val) { _ClaimUpdated = val; }
        public byte[] getUpdated() { return _ClaimUpdated; }
    }
}