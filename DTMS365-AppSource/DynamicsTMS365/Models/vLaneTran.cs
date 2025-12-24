using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class vLaneTran
    {
        public int ID { get; set; }
        public int LaneTransNumber { get; set; }
        public string LaneTransTypeDesc { get; set; }
        public int rowguid { get; set; }
        public string LaneTransServiceFeeMin { get; set; }
        public string LaneTransServiceFeeMax { get; set; }
        public string LaneTransServiceFeeFlat { get; set; }
        public double LaneTransServiceFeePerc { get; set; }
        private byte[] _LaneTransUpdated;
        public string LaneTransUpdated
        {
            get
            {
                if (this._LaneTransUpdated != null)
                {

                    return Convert.ToBase64String(this._LaneTransUpdated);

                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._LaneTransUpdated = null;

                }

                else
                {

                    this._LaneTransUpdated = Convert.FromBase64String(value);

                }

            }
        }

        public void setUpdated(byte[] val) { _LaneTransUpdated = val; }
        public byte[] getUpdated() { return _LaneTransUpdated; }
    }
}