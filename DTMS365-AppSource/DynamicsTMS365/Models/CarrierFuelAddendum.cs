using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class CarrierFuelAddendum
    {

       
        public int CarrFuelAdCarrierControl { get; set; }

        public int CarrFuelAdCarrTarControl { get; set; }

        public int CarrFuelAdCarrTarEquipControl { get; set; }

        public int CarrFuelAdControl { get; set; }

        public decimal CarrFuelAdDefFuelRate { get; set; }

        public DateTime? CarrFuelAdModDate { get; set; }

        public string CarrFuelAdModUser { get; set; }

       
        public bool CarrFuelAdUseNatAvg { get; set; }

        public bool CarrFuelAdUseRatePerMile { get; set; }

        public bool CarrFuelAdUseZoneAvg { get; set; }

        private byte[] _CarrFuelAdUpdated;

    
        public string CarrFuelAdUpdated
        {
            get
            {
                if (this._CarrFuelAdUpdated != null)
                {

                    return Convert.ToBase64String(this._CarrFuelAdUpdated);

                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._CarrFuelAdUpdated = null;

                }

                else
                {

                    this._CarrFuelAdUpdated = Convert.FromBase64String(value);

                }

            }
        }

        public void setUpdated(byte[] val) { _CarrFuelAdUpdated = val; }
        public byte[] getUpdated() { return _CarrFuelAdUpdated; }
    }
}