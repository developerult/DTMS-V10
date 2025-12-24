using Ngl.FreightMaster.Data.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class LELaneProfileFees
    {
        public int LaneProfileSettingsAccessorialCode { get; set; }
        public string LaneProfileSettingsAccessorialName { get; set; }
        public int LaneProfileSettingsLaneControl { get; set; }
        public string LaneProfileSettingsLaneName { get; set; }
        public string LaneProfileSettingsLaneNumber { get; set; }                
        public bool? LaneProfileSettingsSelected { get; set; }

     //   public string LaneProfileSettingsSelectedText { get; set; }

        private byte[] _LaneProfileUpdated;

        public string LaneProfileSettingsSelectedText
        {
            get
            {
                if (this.LaneProfileSettingsSelected ==false)
                {

                    return "False";

                }
                else
                {
                    return "True";
                }
                
            }
           set { }
        }
        public string LaneProfileUpdated
        {
            get
            {
                if (this._LaneProfileUpdated != null)
                {

                    return Convert.ToBase64String(this._LaneProfileUpdated);

                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._LaneProfileUpdated = null;

                }

                else
                {

                    this._LaneProfileUpdated = Convert.FromBase64String(value);

                }

            }
        }
        public void setUpdated(byte[] val) { _LaneProfileUpdated = val; }
        public byte[] getUpdated() { return _LaneProfileUpdated; }

    }
}