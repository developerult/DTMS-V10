using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class vTariffRate
    {
        public int CarrTarEquipMatControl { get; set; }
        public  string CarrTarEquipMatName { get; set; }
        public int CarrTarEquipMatCarrTarControl { get; set; }
        public int CarrTarEquipMatCarrTarEquipControl { get; set; }
        public string CarrTarEquipName { get; set; }
        public string CarrTarEquipDescription { get; set; }
        public int CarrTarEquipMatCarrTarMatBPControl { get; set; }
        public string CarrTarEquipMatCountry { get; set; }
        public string CarrTarEquipMatState { get; set; }
        public string CarrTarEquipMatCity { get; set; }
        public string CarrTarEquipMatFromZip { get; set; }
        public string CarrTarEquipMatToZip { get; set; }
        public int CarrTarEquipMatLaneControl { get; set; }
        public decimal? CarrTarEquipMatMin { get; set; }
        public int CarrTarEquipMatMaxDays { get; set; }
        public string CarrTarEquipMatClass { get; set; }
        public int CarrTarEquipMatClassTypeControl { get; set; }
        public int CarrTarEquipMatTarRateTypeControl { get; set; }
        public int CarrTarEquipMatTarBracketTypeControl { get; set; }
        public string CarrTarEquipMatModUser { get; set; }
        public  DateTime CarrTarEquipMatModDate { get; set; }
        public  decimal? Val1 { get; set; }
        public decimal? Val2 { get; set; }
        public decimal? Val3 { get; set; }
        public decimal? Val4 { get; set; }
        public decimal? Val5 { get; set; }
        public decimal? Val6 { get; set; }
        public decimal? Val7 { get; set; }
        public decimal? Val8 { get; set; }
        public decimal? Val9 { get; set; }
        public decimal Val10 { get; set; }
        public string LaneNumber { get; set; }
        public string BPHeader1 { get; set; }
        public string BPHeader2 { get; set; }
        public string BPHeader3 { get; set; }
        public string BPHeader4 { get; set; }
        public string BPHeader5 { get; set; }
        public string BPHeader6 { get; set; }
        public string BPHeader7 { get; set; }
        public string BPHeader8 { get; set; }
        public string BPHeader9 { get; set; }
        public string BPHeader10 { get; set; }

        private byte[] _CarrTarEquipMatUpdated;

        /// <summary>
        /// BidUpdated should be bound to UI, _BidUpdated is only bound on the controller
        /// </summary>
        public string CarrTarEquipMatUpdated
        {
            get
            {
                if (this._CarrTarEquipMatUpdated != null)
                {

                    return Convert.ToBase64String(this._CarrTarEquipMatUpdated);

                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._CarrTarEquipMatUpdated = null;

                }

                else
                {

                    this._CarrTarEquipMatUpdated = Convert.FromBase64String(value);

                }

            }
        }

        public void setUpdated(byte[] val) { _CarrTarEquipMatUpdated = val; }
        public byte[] getUpdated() { return _CarrTarEquipMatUpdated; }

    }
}