using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class LECompCar
    {

        public string CarrierName { get; set; }
        public int? CarrierNumber { get; set; }
        public string CarrierSCAC { get; set; }
        public string CompName { get; set; }
        public int? CompNumber { get; set; }
        public bool LECompCarAllowApptDelete { get; set; }
        public bool LECompCarAllowApptEdit { get; set; }
        public bool LECompCarApptAutomation { get; set; }
        public int LECompCarApptModCutOffMinutes { get; set; }
        public int LECompCarCarrierControl { get; set; }
        public int LECompCarCompControl { get; set; }
        public int LECompCarControl { get; set; }
        public string LECompCarDefaultLastLoadTime { get; set; }
        public int LECompCarLEAControl { get; set; }
        public DateTime? LECompCarModDate { get; set; }
        public string LECompCarModUser { get; set; }

        private byte[] _LECompCarUpdated;

        /// <summary>
        /// LECompCarUpdated should be bound to UI, _LECompCarUpdated is only bound on the controller
        /// </summary>
        public string LECompCarUpdated
        {
            get
            {
                if (this._LECompCarUpdated != null)
                {

                    return Convert.ToBase64String(this._LECompCarUpdated);

                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._LECompCarUpdated = null;

                }

                else
                {

                    this._LECompCarUpdated = Convert.FromBase64String(value);

                }

            }
        }

        public void setUpdated(byte[] val) { _LECompCarUpdated = val; }
        public byte[] getUpdated() { return _LECompCarUpdated; }


    }
}