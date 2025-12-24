using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class CarrierEquipmentCode
    {

        public int CarrierEquipControl { get; set; }
        public string CarrierEquipCode { get; set; }
        public string CarrierEquipDescription { get; set; }
        public double CarrierEquipCasesMinimum { get; set; }
        public double CarrierEquipCasesConsiderFull { get; set; }
        public double CarrierEquipCasesMaximum { get; set; }
        public double CarrierEquipWgtMinimum { get; set; }
        public double CarrierEquipWgtConsiderFull { get; set; }
        public double CarrierEquipWgtMaximum { get; set; }
        public double CarrierEquipCubesMinimum { get; set; }
        public double CarrierEquipCubesConsiderFull { get; set; }
        public double CarrierEquipCubesMaximum { get; set; }
        public int CarrierEquipPltsMinimum { get; set; }
        public int CarrierEquipPltsConsiderFull { get; set; }
        public int CarrierEquipPltsMaximum { get; set; }
        public int CarrierEquipTempType { get; set; }
        public string CarrierEquipModUser { get; set; }
        public DateTime? CarrierEquipModDate { get; set; }
        //modified by RHR for v-8.5.3.006 on 10/17/2022 added map code data
        public string CarrierEquipMapCode { get; set; }

        private byte[] _CarrierEquipCodesUpdated;
        /// <summary>
        /// CarrierUpdated Property as STRING
        /// </summary>
        public string CarrierEquipCodesUpdated
        {
            get
            {
                if (this._CarrierEquipCodesUpdated != null)
                {

                    return Convert.ToBase64String(this._CarrierEquipCodesUpdated);

                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._CarrierEquipCodesUpdated = null;

                }

                else
                {

                    this._CarrierEquipCodesUpdated = Convert.FromBase64String(value);

                }

            }
        }
        

        public void setUpdated(byte[] val) { _CarrierEquipCodesUpdated = val; }
        public byte[] getUpdated() { return _CarrierEquipCodesUpdated; }
    }
}