using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class CarrierFuelAdRate
    {

       
        public int CarrFuelAdRatesCarrFuelAdControl { get; set; }
        
        public int CarrFuelAdRatesControl { get; set; }
        
        public DateTime CarrFuelAdRatesEffDate { get; set; }
        
        public DateTime? CarrFuelAdRatesModDate { get; set; }
        
        public string CarrFuelAdRatesModUser { get; set; }
        
        public decimal? CarrFuelAdRatesPercent { get; set; }
        
        public decimal? CarrFuelAdRatesPerMile { get; set; }
       
        public decimal CarrFuelAdRatesPriceFrom { get; set; }
       
        public decimal CarrFuelAdRatesPriceTo { get; set; }

        public double CarrFuelAdRatesEffDateNum { get; set; }


        private byte[] _CarrFuelAdRatesUpdated;


        public string CarrFuelAdRatesUpdated
        {
            get
            {
                if (this._CarrFuelAdRatesUpdated != null)
                {

                    return Convert.ToBase64String(this._CarrFuelAdRatesUpdated);

                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._CarrFuelAdRatesUpdated = null;

                }

                else
                {

                    this._CarrFuelAdRatesUpdated = Convert.FromBase64String(value);

                }

            }
        }

        public void setUpdated(byte[] val) { _CarrFuelAdRatesUpdated = val; }
        public byte[] getUpdated() { return _CarrFuelAdRatesUpdated; }


    }
}