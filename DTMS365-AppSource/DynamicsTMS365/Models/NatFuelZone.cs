using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class NatFuelZone
    { 
        public DateTime? NatFuelZoneStatesModDate { get; set; }
        public string NatFuelZoneStatesModUser { get; set; }
        public int NatFuelZoneStatesNatFuelZoneID { get; set; }
        public string NatFuelZoneStatesState { get; set; }
        public byte[] _NatFuelZoneStatesUpdated { get; set; }
        public string NatFuelZoneStatesUpdated
        {
            get
            {
                if (this._NatFuelZoneStatesUpdated != null) { return Convert.ToBase64String(this._NatFuelZoneStatesUpdated); }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value)) { this._NatFuelZoneStatesUpdated = null; }
                else { this._NatFuelZoneStatesUpdated = Convert.FromBase64String(value); }
            }
        }

        public void setUpdated(byte[] val) { _NatFuelZoneStatesUpdated = val; }
        public byte[] getUpdated() { return _NatFuelZoneStatesUpdated; }
    }
    public class vNatFuelZone
    {
        public string NatFuelState { get; set; }
        public int NatFuelZoneID { get; set; }
        public int NatFuelzoneStateRowID { get; set; }
        public DateTime? NatFuelZoneModDate { get; set; }
        public string NatFuelZoneModUser { get; set; }
        public string NatFuelZoneName { get; set; }
        public byte[] _NatFuelZoneUpdated { get; set; }
    }
}