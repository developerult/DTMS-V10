using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace DynamicsTMS365.Models
{
    public class vPOItem
    {
        public string POItemControl { get; set; }
        public string ItemPONumber { get; set; }
        public string ItemNumber { get; set; }
        public short? Pack { get; set; }
        public string Size { get; set; }
        public string Description { get; set; }
        public decimal? FreightCost { get; set; }
        public decimal? ItemCost { get; set; }
        public int? QtyOrdered { get; set; }
        public double? Weight { get; set; }
        public int? Cube { get; set; }
        public double? FixOffInvAllow { get; set; }
        public decimal? FixFrtAllow { get; set; }
        public string Brand { get; set; }
        public string CostCenter { get; set; }
        public string CustItemNumber { get; set; }
        public int? CustomerNumber { get; set; }
        public string GTIN { get; set; }
        public string Hazmat { get; set; }
        public DateTime? LotExpirationDate { get; set; }
        public string LotNumber { get; set; }
        public int POOrderSequence { get; set; }
        public string PalletType { get; set; }
        public string POItemHazmatTypeCode { get; set; }
        public string POItem49CFRCode { get; set; }
        public string POItemIATACode { get; set; }
        public string POItemDOTCode { get; set; }
        public string POItemMarineCode { get; set; }
        public string POItemNMFCClass { get; set; }
        public string POItemNMFCSubClass { get; set; }
        public string POItemFAKClass { get; set; }
        public bool? POItemLimitedQtyFlag { get; set; }
        public double? POItemPallets { get; set; }
        public double? POItemTies { get; set; }
        public double? POItemHighs { get; set; }
        public double? POItemQtyPalletPercentage { get; set; }
        public double? POItemQtyLength { get; set; }
        public double? POItemQtyWidth { get; set; }
        public double? POItemQtyHeight { get; set; }
        public bool? POItemStackable { get; set; }
        public int? POItemLevelOfDensity { get; set; }
        public string POItemCompLegalEntity { get; set; }
        public string POItemCompAlphaCode { get; set; }
        public string POItemUser1 { get; set; }
        public string POItemUser2 { get; set; }
        public string POItemUser3 { get; set; }
        public string POItemUser4 { get; set; }
        public string POItemCommCode { get; set; }
        public string POItemCustomerPO { get; set; }
        public string POItemLocationCode { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedUser { get; set; }
        public string POItemOrderNumber { get; set; }

        private byte[] _POItemUpdated;

        /// <summary>
        /// POItemUpdated should be bound to UI, _POItemUpdated is only bound on the controller
        /// </summary>
        public string POItemUpdated
        {
            get
            {
                if (this._POItemUpdated != null) { return Convert.ToBase64String(this._POItemUpdated); }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value)) { this._POItemUpdated = null; } else { this._POItemUpdated = Convert.FromBase64String(value); }
            }
        }

        public void setUpdated(byte[] val) { _POItemUpdated = val; }
        public byte[] getUpdated() { return _POItemUpdated; }


    }
}