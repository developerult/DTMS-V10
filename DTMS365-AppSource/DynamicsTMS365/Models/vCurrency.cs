using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class vCurrency
    {
        public int ID { get; set; }
        public string CurrencyType { get; set; }
        public string CurrencyName { get; set; }
        public  string CurrencyCountry { get; set; }
        public int rowguid { get; set; }
        private byte[] _CurrencyUpdated;
        public string CurrencyUpdated
        {
            get
            {
                if (this._CurrencyUpdated != null)
                {

                    return Convert.ToBase64String(this._CurrencyUpdated);

                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._CurrencyUpdated = null;

                }

                else
                {

                    this._CurrencyUpdated = Convert.FromBase64String(value);

                }

            }
        }

        public void setUpdated(byte[] val) { _CurrencyUpdated = val; }
        public byte[] getUpdated() { return _CurrencyUpdated; }
    }
}