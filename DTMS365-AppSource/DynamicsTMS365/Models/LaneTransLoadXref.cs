using Ngl.FreightMaster.Data.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class LaneTransLoadXref
    {


        public double LaneTranXBenchMiles { get; set; }
        public int LaneTranXControl { get; set; }
        public int LaneTranXLaneControl { get; set; }
        public string LaneTranXLaneName { get; set; }
        public string LaneTranXLaneNumber { get; set; }
        public int LaneTranXMaxCases { get; set; }
        public int LaneTranXMaxCube { get; set; }
        public double LaneTranXMaxPL { get; set; }
        public double LaneTranXMaxWgt { get; set; }
        public int LaneTranXMinCases { get; set; }
        public int LaneTranXMinCube { get; set; }
        public double LaneTranXMinPL { get; set; }
        public double LaneTranXMinWgt { get; set; }
        public DateTime? LaneTranXModDate { get; set; }
        public int LaneTranXModeTypeControl { get; set; }
        public string LaneTranXModeTypeName { get; set; }
        public string LaneTranXModUser { get; set; }
        public string LaneTranXName { get; set; }
        public int LaneTranXSequence { get; set; }
        public string LaneTranXTempType { get; set; }
        public string LaneTranXTempTypeName { get; set; }
        public string LaneTranXUser1 { get; set; }
        public string LaneTranXUser2 { get; set; }
        public string LaneTranXUser3 { get; set; }
        public string LaneTranXUser4 { get; set; }
        public string TempType { get; set; }



        private byte[] _LaneTranXUpdated;


        public string LaneTranXUpdated
        {
            get
            {
                if (this._LaneTranXUpdated != null)
                {

                    return Convert.ToBase64String(this._LaneTranXUpdated);

                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._LaneTranXUpdated = null;

                }

                else
                {

                    this._LaneTranXUpdated = Convert.FromBase64String(value);

                }

            }
        }

        public void setUpdated(byte[] val) { _LaneTranXUpdated = val; }
        public byte[] getUpdated() { return _LaneTranXUpdated; }
    }
}