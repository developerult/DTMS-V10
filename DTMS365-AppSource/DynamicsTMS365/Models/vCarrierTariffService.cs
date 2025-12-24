using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class vCarrierTariffService
    {
        public string CarrTarEquipCarrProName { get; set; }
        public int CarrTarEquipCarrTarControl { get; set; }
        public int CarrTarEquipCarrierEquipControl { get; set; }
        public double CarrTarEquipCasesConsiderFull { get; set; }
        public double CarrTarEquipCasesMaximum { get; set; }
        public double CarrTarEquipCasesMinimum { get; set; }
        public int CarrTarEquipControl { get; set; }
        public double CarrTarEquipCubesConsiderFull { get; set; }
        public double CarrTarEquipCubesMaximum { get; set; }
        public double CarrTarEquipCubesMinimum { get; set; }
        public string CarrTarEquipDescription { get; set; }
        public bool CarrTarEquipDLFri { get; set; }
        public bool CarrTarEquipDLMon { get; set; }
        public bool CarrTarEquipDLSat { get; set; }
        public bool CarrTarEquipDLSun { get; set; }
        public bool CarrTarEquipDLThu { get; set; }
        public bool CarrTarEquipDLTue { get; set; }
        public bool CarrTarEquipDLWed { get; set; }
        public int? CarrTarEquipMatCarrTarEquipControl { get; set; }
        public int? CarrTarEquipMatCarrTarMatBPControl { get; set; }
        public string CarrTarEquipMatCity { get; set; }
        public int? CarrTarEquipMatClassTypeControl { get; set; }
        public int? CarrTarEquipMatControl { get; set; }
        public string CarrTarEquipMatCountry { get; set; }
        public string CarrTarEquipMatFromZip { get; set; }
        public int? CarrTarEquipMatMaxDays { get; set; }
        public decimal? CarrTarEquipMatMin { get; set; }
        public string CarrTarEquipMatName { get; set; }
        public string CarrTarEquipMatState { get; set; }
        public int? CarrTarEquipMatTarBracketTypeControl { get; set; }
        public int? CarrTarEquipMatTarRateTypeControl { get; set; }
        public string CarrTarEquipMatToZip { get; set; }
        public DateTime? CarrTarEquipModDate { get; set; }
        public string CarrTarEquipModUser { get; set; }
        public string CarrTarEquipName { get; set; }
        public int CarrTarEquipPltsConsiderFull { get; set; }
        public int CarrTarEquipPltsMaximum { get; set; }
        public int CarrTarEquipPltsMinimum { get; set; }
        public bool CarrTarEquipPUFri { get; set; }
        public bool CarrTarEquipPUMon { get; set; }
        public bool CarrTarEquipPUSat { get; set; }
        public bool CarrTarEquipPUSun { get; set; }
        public bool CarrTarEquipPUThu { get; set; }
        public bool CarrTarEquipPUTue { get; set; }
        public bool CarrTarEquipPUWed { get; set; }
        public int CarrTarEquipTempType { get; set; }
        public double CarrTarEquipWgtConsiderFull { get; set; }
        public double CarrTarEquipWgtMaximum { get; set; }
        public double CarrTarEquipWgtMinimum { get; set; }
        public string Temp { get; set; }
        public string ClassType { get; set; }
        public string RateType { get; set; }
        public string BracketType { get; set; }
        public bool? CarrTarEquipMultiOrigRating { get; set; }

        private byte[] _CarrTarEquipUpdated;

        /// <summary>
        /// BidUpdated should be bound to UI, _BidUpdated is only bound on the controller
        /// </summary>
        public string CarrTarEquipUpdated
        {
            get
            {
                if (this._CarrTarEquipUpdated != null)
                {

                    return Convert.ToBase64String(this._CarrTarEquipUpdated);

                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._CarrTarEquipUpdated = null;

                }

                else
                {

                    this._CarrTarEquipUpdated = Convert.FromBase64String(value);

                }

            }
        }

        public void setUpdated(byte[] val) { _CarrTarEquipUpdated = val; }
        public byte[] getUpdated() { return _CarrTarEquipUpdated; }
    }
}