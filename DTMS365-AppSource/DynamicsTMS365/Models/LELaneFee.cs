using Ngl.FreightMaster.Data.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class LELaneFee
    {

        
        public string AccessorialFeeAllocationTypeName { get; set; }
        public string AccessorialFeeCalcTypeName { get; set; }
        public string AccessorialName { get; set; }
        public int? LaneFeesAccessorialCode { get; set; }
        public int LaneFeesAccessorialFeeAllocationTypeControl { get; set; }
        public int LaneFeesAccessorialFeeCalcTypeControl { get; set; }
        public bool LaneFeesAllowCarrierUpdates { get; set; }
        public bool LaneFeesAutoApprove { get; set; }
        public string LaneFeesBOLPlacement { get; set; }
        public string LaneFeesBOLText { get; set; }
        public string LaneFeesCaption { get; set; }
        public int LaneFeesControl { get; set; }
        public string LaneFeesEDICode { get; set; }
        public bool LaneFeesIsTax { get; set; }
        public int? LaneFeesLaneControl { get; set; }
        public string LaneFeesMinimum { get; set; }
        public DateTime? LaneFeesModDate { get; set; }
        public string LaneFeesModUser { get; set; }
        public int LaneFeesTarBracketTypeControl { get; set; }
        public bool LaneFeesTaxable { get; set; }
        public int LaneFeesTaxSortOrder { get; set; }
        public double? LaneFeesVariable { get; set; }
        public int? LaneFeesVariableCode { get; set; }
        public bool LaneFeesVisible { get; set; }
        public string AccessorialVariableCodesName { get; set; }

        public string TarBracketTypeName { get; set; }
        private byte[] _LaneFeesUpdated;


        public string LaneFeesUpdated
        {
            get
            {
                if (this._LaneFeesUpdated != null)
                {

                    return Convert.ToBase64String(this._LaneFeesUpdated);

                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._LaneFeesUpdated = null;

                }

                else
                {

                    this._LaneFeesUpdated = Convert.FromBase64String(value);

                }

            }
        }

        public void setUpdated(byte[] val) { _LaneFeesUpdated = val; }
        public byte[] getUpdated() { return _LaneFeesUpdated; }

    }
}