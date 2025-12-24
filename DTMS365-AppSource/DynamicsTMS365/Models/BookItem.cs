using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
    public class BookItem
    {
        
        public string BookCustItemNumber { get; set; }        
        public string BookItem49CFRCode { get; set; }
        
        public decimal? BookItemActFreightCost { get; set; }
        
        public decimal BookItemBFC { get; set; }
        
        public int BookItemBookLoadControl { get; set; }
              
        public int BookItemBookPkgControl { get; set; }
        
        public string BookItemBrand { get; set; }
        
        public int BookItemCarrTarEquipMatControl { get; set; }
        
        public int BookItemCarrTarEquipMatDetID { get; set; }
        
        public decimal? BookItemCarrTarEquipMatDetValue { get; set; }
        
        public string BookItemCarrTarEquipMatName { get; set; }
        
        public string BookItemCommCode { get; set; }
        
        public int BookItemControl { get; set; }
        
        public string BookItemCostCenter { get; set; }
        
        public string BookItemCountryOfOrigin { get; set; }
        
        public int BookItemCube { get; set; }

        public string BookItemCustomerPO { get; set; }

        public string BookItemDeficit49CFRCode { get; set; }
        
        public decimal BookItemDeficitCostAdjustment { get; set; }
        
        public string BookItemDeficitDOTCode { get; set; }
        
        public string BookItemDeficitFAKClass { get; set; }
        
        public string BookItemDeficitIATACode { get; set; }
        
        public string BookItemDeficitMarineCode { get; set; }
        
        public string BookItemDeficitNMFCClass { get; set; }
        
        public decimal BookItemDeficitWeightAdjustment { get; set; }
        
        public string BookItemDescription { get; set; }
        
        public decimal BookItemDiscount { get; set; }
        
        public string BookItemDOTCode { get; set; }
        
        public string BookItemFAKClass { get; set; }
        
        public decimal BookItemFixFrtAllow { get; set; }
        
        public double BookItemFixOffInvAllow { get; set; }
        
        public decimal BookItemFreightCost { get; set; }
        
        public string BookItemGTIN { get; set; }
             
        public int BookItemHazControl { get; set; }
        
        public string BookItemHazmat { get; set; }
        
        public string BookItemHazmatTypeCode { get; set; }
        
        public double BookItemHighs { get; set; }
        
        public string BookItemHST { get; set; }
        
        public string BookItemIATACode { get; set; }
        
        public decimal BookItemItemCost { get; set; }
        
        public string BookItemItemNumber { get; set; }
        
        public int BookItemLevelOfDensity { get; set; }
        
        public bool BookItemLimitedQtyFlag { get; set; }
        
        public decimal BookItemLineHaul { get; set; }

        public string BookItemLocationCode { get; set; }
        public DateTime? BookItemLotExpirationDate { get; set; }
        
        public string BookItemLotNumber { get; set; }
        
        public string BookItemMarineCode { get; set; }
        
        public DateTime? BookItemModDate { get; set; }
        
        public string BookItemModUser { get; set; }
        
        public string BookItemNMFCClass { get; set; }

        public string BookItemNMFCSubClass { get; set; }
        public decimal BookItemNonTaxableFees { get; set; }
     
        public string BookItemOrderNumber { get; set; }
        
        public short BookItemPack { get; set; }
        
        public double BookItemPallets { get; set; }
        
        public int BookItemPalletTypeID { get; set; }
        
        public double BookItemQtyHeight { get; set; }
        
        public double BookItemQtyLength { get; set; }
        
        public int BookItemQtyOrdered { get; set; }
        
        public double BookItemQtyPalletPercentage { get; set; }
        
        public double BookItemQtyWidth { get; set; }
        
        public string BookItemRated49CFRCode { get; set; }
        
        public string BookItemRatedDOTCode { get; set; }
        
        public string BookItemRatedFAKClass { get; set; }
        
        public string BookItemRatedIATACode { get; set; }
        
        public string BookItemRatedMarineCode { get; set; }
        
        public string BookItemRatedNMFCClass { get; set; }
        
        public string BookItemRatedNMFCSubClass { get; set; }
        
        public string BookItemSize { get; set; }
        
        public bool BookItemStackable { get; set; }
        
        public decimal BookItemTaxableFees { get; set; }
        
        public decimal BookItemTaxes { get; set; }
        
        public double BookItemTies { get; set; }
        
        public int BookItemUnitOfMeasureControl { get; set; }
             
        public string BookItemUser1 { get; set; }
        
        public string BookItemUser2 { get; set; }
        
        public string BookItemUser3 { get; set; }
        
        public string BookItemUser4 { get; set; }
        
        public double BookItemWeight { get; set; }
        
        public decimal BookItemWeightBreak { get; set; }
    
        public int TempControl { get; set; }
               

        private byte[] _BookItemUpdated;

        /// <summary>
        /// BookItemUpdated should be bound to UI, _BookItemUpdated is only bound on the controller
        /// </summary>
        public string BookItemUpdated
        {
            get
            {
                if (this._BookItemUpdated != null)
                {

                    return Convert.ToBase64String(this._BookItemUpdated);

                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    this._BookItemUpdated = null;

                }

                else
                {

                    this._BookItemUpdated = Convert.FromBase64String(value);

                }

            }
        }

        public void setUpdated(byte[] val) { _BookItemUpdated = val; }
        public byte[] getUpdated() { return _BookItemUpdated; }


    }
}