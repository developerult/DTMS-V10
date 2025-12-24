using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class CarrierFuelAdEx
    {

       
        public int CarrFuelAdExCarrFuelAdContol { get; set; }
       
        public int CarrFuelAdExControl { get; set; }
        
        public DateTime CarrFuelAdExEffDate { get; set; }
        
        public DateTime? CarrFuelAdExModDate { get; set; }
        
        public string CarrFuelAdExModUser { get; set; }
        
        public decimal? CarrFuelAdExPercent { get; set; }
        
        public decimal? CarrFuelAdExRatePerMile { get; set; }
       
        public string CarrFuelAdExState { get; set; }
      

        private byte[] _CarrFuelAdExUpdated;


        public string CarrFuelAdExUpdated
        {
            get
            {
                if (this._CarrFuelAdExUpdated != null)
                {

                    return Convert.ToBase64String(this._CarrFuelAdExUpdated);

                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._CarrFuelAdExUpdated = null;

                }

                else
                {

                    this._CarrFuelAdExUpdated = Convert.FromBase64String(value);

                }

            }
        }

        public void setUpdated(byte[] val) { _CarrFuelAdExUpdated = val; }
        public byte[] getUpdated() { return _CarrFuelAdExUpdated; }

    }
}