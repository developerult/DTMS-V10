using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class CarrierTariff
    {
        public int CarrTarControl { get; set; }
        public int CarrTarCarrierControl { get; set; }
        public int CarrTarCompControl { get; set; }
        public string CarrTarID { get; set; }
        public int CarrTarBPBracketType { get; set; }
        public int CarrTarTLCapacityType { get; set; }
        public int CarrTarTempType { get; set; }
        public string CarrTarTariffType { get; set; }
        public Boolean CarrTarDefWgt { get; set; }
        public string CarrTarModUser { get; set; }
        public DateTime CarrTarModDate { get; set; }
        public DateTime? CarrTarEffDateFrom { get; set; }
        public DateTime? CarrTarEffDateTo { get; set; }
        public Boolean CarrTarAutoAssignPro { get; set; }
        public int CarrTarTariffTypeControl { get; set; }
        public int CarrTarTariffModeTypeControl { get; set; }
        public string CarrTarName { get; set; }
        public int CarrTarRevisionNumber { get; set; }
        public Boolean CarrTarApproved { get; set; }
        public DateTime? CarrTarApprovedDate { get; set; }
        public string CarrTarApprovedBy { get; set; }
        public Boolean CarrTarRejected { get; set; }
        public string CarrTarRejectedBy { get; set; }
        public DateTime? CarrTarRejectedDate { get; set; }
        public Boolean CarrTarOutbound { get; set; }
        public int CarrTarAgentControl { get; set; }
        public DateTime? CarrTarIssuedDate { get; set; }
        public string CarrTarUser1 { get; set; }
        public string CarrTarUser2 { get; set; }
        public string CarrTarUser3 { get; set; }
        public string CarrTarUser4 { get; set; }
        public Boolean CarrTarPreCloneControl { get; set; }
        public Boolean CarrTarWillDriveSunday { get; set; }
        public Boolean CarrTarWillDriveSaturday { get; set; }

        private byte[] _CarrTarUpdated;

        /// <summary>
        /// BidUpdated should be bound to UI, _BidUpdated is only bound on the controller
        /// </summary>
        public string CarrTarUpdated
        {
            get
            {
                if (this._CarrTarUpdated != null)
                {

                    return Convert.ToBase64String(this._CarrTarUpdated);

                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._CarrTarUpdated = null;

                }

                else
                {

                    this._CarrTarUpdated = Convert.FromBase64String(value);

                }

            }
        }

        public void setUpdated(byte[] val) { _CarrTarUpdated = val; }
        public byte[] getUpdated() { return _CarrTarUpdated; }
        
    }
}