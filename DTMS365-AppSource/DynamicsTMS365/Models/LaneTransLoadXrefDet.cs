using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class LaneTransLoadXrefDet
    {



        public double LaneTranXDetBenchMiles { get; set; }
        public double? LaneTranXDetBFC { get; set; }
        public string LaneTranXDetBFCType { get; set; }
        public bool? LaneTranXDetBilledSeperately { get; set; }
        public int LaneTranXDetCarrierContControl { get; set; }
        public int LaneTranXDetCarrierControl { get; set; }
        public string LaneTranXDetCarrierName { get; set; }
        public int? LaneTranXDetCarrierNumber { get; set; }
        public int LaneTranXDetCarrTarControl { get; set; }
        public int LaneTranXDetCarrTarEquipControl { get; set; }
        public string LaneTranXDetCarrTarEquipName { get; set; }
        public string LaneTranXDetCarrTarName { get; set; }
        public bool? LaneTranXDetConsolidateSplits { get; set; }
        public string LaneTranXDetCont800 { get; set; }
        public string LaneTranXDetContExt { get; set; }
        public string LaneTranXDetContName { get; set; }
        public string LaneTranXDetContPhone { get; set; }
        public int LaneTranXDetControl { get; set; }
        public int LaneTranXDetLaneControl { get; set; }
        public string LaneTranXDetLaneName { get; set; }
        public string LaneTranXDetLaneNumber { get; set; }
        public int LaneTranXDetLaneTranXControl { get; set; }
        public DateTime? LaneTranXDetModDate { get; set; }
        public int LaneTranXDetModeTypeControl { get; set; }
        public string LaneTranXDetModeTypeName { get; set; }
        public string LaneTranXDetModUser { get; set; }
        public string LaneTranXDetName { get; set; }
        public bool? LaneTranXDetRule11Required { get; set; }
        public int LaneTranXDetSequence { get; set; }
        public int LaneTranXDetTransferHours { get; set; }
        public int LaneTranXDetTransitHours { get; set; }
        public int? LaneTranXDetTransType { get; set; }
        public string LaneTranXDetTransTypeName { get; set; }
        public string LaneTranXDetUser1 { get; set; }
        public string LaneTranXDetUser2 { get; set; }
        public string LaneTranXDetUser3 { get; set; }
        public string LaneTranXDetUser4 { get; set; }
        public string TransLoadName { get; set; }

        private byte[] _LaneTranXDetUpdated;

        public string LaneTranXDetUpdated
        {
            get
            {
                if (this._LaneTranXDetUpdated != null)
                {

                    return Convert.ToBase64String(this._LaneTranXDetUpdated);

                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._LaneTranXDetUpdated = null;

                }

                else
                {

                    this._LaneTranXDetUpdated = Convert.FromBase64String(value);

                }

            }
        }

        public void setUpdated(byte[] val) { _LaneTranXDetUpdated = val; }
        public byte[] getUpdated() { return _LaneTranXDetUpdated; }
    }
}