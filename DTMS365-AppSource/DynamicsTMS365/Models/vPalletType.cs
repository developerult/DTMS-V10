using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class vPalletType
    {
        public int ID { get; set; }
        public int PalletTypeBitPos { get; set; }
        public string PalletType { get; set; }
        public string PalletTypeDescription { get; set; }
        public int rowguid { get; set; }
        public double PalletTypeWeight { get; set; }
        public double PalletTypeHeight { get; set; }
        public double PalletTypeWidth { get; set; }
        public double PalletTypeDepth { get; set; }
        public double PalletTypeVolume { get; set; }
        public double PalletTypeContainer { get; set; }

        private byte[] _PalletTypeUpdated;


        public string PalletTypeUpdated
        {
            get
            {
                if (this._PalletTypeUpdated != null) { return Convert.ToBase64String(this._PalletTypeUpdated); }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value)) { this._PalletTypeUpdated = null; }
                else { this._PalletTypeUpdated = Convert.FromBase64String(value); }
            }
        }

        public void setUpdated(byte[] val) { _PalletTypeUpdated = val; }
        public byte[] getUpdated() { return _PalletTypeUpdated; }
    }
}