using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicsTMS365.Models
{
	/// <summary>
	/// AMSBookItem class for bbook item
	/// </summary>
	public class AMSBookItem
	{
		/// <summary>
		/// PropertyBookItemControl/ID as INT
		/// </summary>
		public int BookItemControl { get; set; }

		/// <summary>
		/// PropertyBookItemBookLoadControl as INT
		/// </summary>
		public int BookItemBookLoadControl { get; set; }

		/// <summary>
		/// PropertyBookItemFixOffInvAllow as Decimal
		/// </summary>
		public Decimal BookItemFixOffInvAllow { get; set; }

		/// <summary>
		/// PropertyBookItemFixFrtAllow as Double
		/// </summary>
		public Double BookItemFixFrtAllow { get; set; }

		/// <summary>
		/// PropertyBookItemItemNumber as STRING
		/// </summary>
		public string BookItemItemNumber { get; set; }

		/// <summary>
		/// PropertyBookItemQtyOrdered as INT
		/// </summary>
		public int BookItemQtyOrdered { get; set; }

		/// <summary>
		/// PropertyBookItemFreightCost as Double
		/// </summary>
		public Double BookItemFreightCost { get; set; }

		/// <summary>
		/// PropertyBookItemActFreightCost as Double
		/// </summary>
		public Double BookItemActFreightCost { get; set; }

		/// <summary>
		/// PropertyBookItemItemCost as Double
		/// </summary>
		public Double BookItemItemCost { get; set; }

		/// <summary>
		/// PropertyBookItemWeight as Decimal
		/// </summary>
		public Decimal BookItemWeight { get; set; }

		/// <summary>
		/// PropertyBookItemCube as INT
		/// </summary>
		public int BookItemCube { get; set; }

		/// <summary>
		/// PropertyBookItemPack as INT
		/// </summary>
		public int BookItemPack { get; set; }

		/// <summary>
		/// PropertyBookItemSize as STRING
		/// </summary>
		public string BookItemSize { get; set; }

		/// <summary>
		/// PropertyBookItemDescription as STRING
		/// </summary>
		public string BookItemDescription { get; set; }

		/// <summary>
		/// PropertyBookItemHazmat as STRING
		/// </summary>
		public string BookItemHazmat { get; set; }

		/// <summary>
		/// PropertyBookItemModDate as DateTime
		/// </summary>
		public DateTime BookItemModDate { get; set; }
		private string _BookItemModDate;

		/// <summary>
		/// PropertyBookItemModUser as STRING
		/// </summary>
		public string BookItemModUser { get; set; }

		/// <summary>
		/// PropertyBookItemBrand as STRING
		/// </summary>
		public string BookItemBrand { get; set; }

		/// <summary>
		/// PropertyBookItemCostCenter as STRING
		/// </summary>
		public string BookItemCostCenter { get; set; }

		/// <summary>
		/// PropertyBookItemLotNumber as STRING
		/// </summary>
		public string BookItemLotNumber { get; set; }

		/// <summary>
		/// PropertyBookItemLotExpirationDate as DateTime
		/// </summary>
		public DateTime BookItemLotExpirationDate { get; set; }
		private string _BookItemLotExpirationDate;


		/// <summary>
		/// PropertyBookItemGTIN as STRING
		/// </summary>
		public string BookItemGTIN { get; set; }

		/// <summary>
		/// PropertyBookCustItemNumber as STRING
		/// </summary>
		public string BookCustItemNumber { get; set; }

		/// <summary>
		/// PropertyBookItemBFC as Double
		/// </summary>
		public Double BookItemBFC { get; set; }

		/// <summary>
		/// PropertyBookItemCountryOfOrigin as STRING
		/// </summary>
		public string BookItemCountryOfOrigin { get; set; }

		/// <summary>
		/// PropertyBookItemHST as STRING
		/// </summary>
		public string BookItemHST { get; set; }

		/// <summary>
		/// PropertyBookItemPalletTypeID as INT
		/// </summary>
		public int BookItemPalletTypeID { get; set; }

		/// <summary>
		/// PropertyBookItemHazmatTypeCode as STRING
		/// </summary>
		public string BookItemHazmatTypeCode { get; set; }

		/// <summary>
		/// PropertyBookItem49CFRCode as STRING
		/// </summary>
		public string BookItem49CFRCode { get; set; }

		/// <summary>
		/// PropertyBookItemIATACode as STRING
		/// </summary>
		public string BookItemIATACode { get; set; }

		/// <summary>
		/// PropertyBookItemDOTCode as STRING
		/// </summary>
		public string BookItemDOTCode { get; set; }

		/// <summary>
		/// PropertyBookItemMarineCode as STRING
		/// </summary>
		public string BookItemMarineCode { get; set; }

		/// <summary>
		/// PropertyBookItemNMFCClass as STRING
		/// </summary>
		public string BookItemNMFCClass { get; set; }

		/// <summary>
		/// PropertyBookItemFAKClass as STRING
		/// </summary>
		public string BookItemFAKClass { get; set; }

		/// <summary>
		/// PropertyBookItemLimitedQtyFlag as bool
		/// </summary>
		public bool BookItemLimitedQtyFlag { get; set; }

		/// <summary>
		/// PropertyBookItemPallets as Decimal
		/// </summary>
		public Decimal BookItemPallets { get; set; }

		/// <summary>
		/// PropertyBookItemTies as Decimal
		/// </summary>
		public Decimal BookItemTies { get; set; }

		/// <summary>
		/// PropertyBookItemHighs as Decimal
		/// </summary>
		public Decimal BookItemHighs { get; set; }

		/// <summary>
		/// PropertyBookItemQtyPalletPercentage as Decimal
		/// </summary>
		public Decimal BookItemQtyPalletPercentage { get; set; }

		/// <summary>
		/// PropertyBookItemQtyLength as Decimal
		/// </summary>
		public Decimal BookItemQtyLength { get; set; }

		/// <summary>
		/// PropertyBookItemQtyWidth as Decimal
		/// </summary>
		public Decimal BookItemQtyWidth { get; set; }

		/// <summary>
		/// PropertyBookItemQtyHeight as Decimal
		/// </summary>
		public Decimal BookItemQtyHeight { get; set; }

		/// <summary>
		/// PropertyBookItemStackable as bool
		/// </summary>
		public bool BookItemStackable { get; set; }

		/// <summary>
		/// PropertyBookItemLevelOfDensity as INT
		/// </summary>
		public int BookItemLevelOfDensity { get; set; }

		/// <summary>
		/// PropertyBookItemDiscount as decimal
		/// </summary>
		public decimal BookItemDiscount { get; set; }

		/// <summary>
		/// PropertyBookItemLineHaul as decimal
		/// </summary>
		public decimal BookItemLineHaul { get; set; }

		/// <summary>
		/// PropertyBookItemTaxableFees as decimal
		/// </summary>
		public decimal BookItemTaxableFees { get; set; }

		/// <summary>
		/// PropertyBookItemTaxes as decimal
		/// </summary>
		public decimal BookItemTaxes { get; set; }

		/// <summary>
		/// PropertyBookItemNonTaxableFees as decimal
		/// </summary>
		public decimal BookItemNonTaxableFees { get; set; }

		/// <summary>
		/// PropertyBookItemDeficitCostAdjustment as decimal
		/// </summary>
		public decimal BookItemDeficitCostAdjustment { get; set; }

		/// <summary>
		/// PropertyBookItemDeficitWeightAdjustment as decimal
		/// </summary>
		public decimal BookItemDeficitWeightAdjustment { get; set; }

		/// <summary>
		/// PropertyBookItemWeightBreak as decimal
		/// </summary>
		public decimal BookItemWeightBreak { get; set; }

		/// <summary>
		/// PropertyBookItemDeficit49CFRCode as STRING
		/// </summary>
		public string BookItemDeficit49CFRCode { get; set; }

		/// <summary>
		/// PropertyBookItemDeficitIATACode as STRING
		/// </summary>
		public string BookItemDeficitIATACode { get; set; }

		/// <summary>
		/// PropertyBookItemDeficitDOTCode as STRING
		/// </summary>
		public string BookItemDeficitDOTCode { get; set; }

		/// <summary>
		/// PropertyBookItemDeficitMarineCode as STRING
		/// </summary>
		public string BookItemDeficitMarineCode { get; set; }

		/// <summary>
		/// PropertyBookItemDeficitNMFCClass as STRING
		/// </summary>
		public string BookItemDeficitNMFCClass { get; set; }

		/// <summary>
		/// PropertyBookItemDeficitFAKClass as STRING
		/// </summary>
		public string BookItemDeficitFAKClass { get; set; }

		/// <summary>
		/// PropertyBookItemRated49CFRCode as STRING
		/// </summary>
		public string BookItemRated49CFRCode { get; set; }

		/// <summary>
		/// PropertyBookItemRatedIATACode as STRING
		/// </summary>
		public string BookItemRatedIATACode { get; set; }

		/// <summary>
		/// PropertyBookItemRatedDOTCode as STRING
		/// </summary>
		public string BookItemRatedDOTCode { get; set; }

		/// <summary>
		/// PropertyBookItemRatedMarineCode as STRING
		/// </summary>
		public string BookItemRatedMarineCode { get; set; }

		/// <summary>
		/// PropertyBookItemRatedNMFCClass as STRING
		/// </summary>
		public string BookItemRatedNMFCClass { get; set; }

		/// <summary>
		/// PropertyBookItemRatedFAKClass as STRING
		/// </summary>
		public string BookItemRatedFAKClass { get; set; }

		/// <summary>
		/// PropertyBookItemCarrTarEquipMatControl as INT
		/// </summary>
		public int BookItemCarrTarEquipMatControl { get; set; }

		/// <summary>
		/// PropertyBookItemCarrTarEquipMatName as STRING
		/// </summary>
		public string BookItemCarrTarEquipMatName { get; set; }

		/// <summary>
		/// PropertyBookItemCarrTarEquipMatDetID as INT
		/// </summary>
		public int BookItemCarrTarEquipMatDetID { get; set; }

		/// <summary>
		/// PropertyBookItemCarrTarEquipMatDetValue as decimal
		/// </summary>
		public decimal BookItemCarrTarEquipMatDetValue { get; set; }

		/// <summary>
		/// PropertyBookItemUser1 as STRING
		/// </summary>
		public string BookItemUser1 { get; set; }

		/// <summary>
		/// PropertyBookItemUser2 as STRING
		/// </summary>
		public string BookItemUser2 { get; set; }

		/// <summary>
		/// PropertyBookItemUser3 as STRING
		/// </summary>
		public string BookItemUser3 { get; set; }

		/// <summary>
		/// PropertyBookItemUser4 as STRING
		/// </summary>
		public string BookItemUser4 { get; set; }

		/// <summary>
		/// PropertyBookItemUnitOfMeasureControl as INT
		/// </summary>
		public int BookItemUnitOfMeasureControl { get; set; }

		/// <summary>
		/// PropertyBookItemRatedNMFCSubClass as STRING
		/// </summary>
		public string BookItemRatedNMFCSubClass { get; set; }

		/// <summary>
		/// PropertyBookItemCommCode as STRING
		/// </summary>
		public string BookItemCommCode { get; set; }

		/// <summary>
		/// PropertyBookItemNMFCSubClass as STRING
		/// </summary>
		public string BookItemNMFCSubClass { get; set; }

		/// <summary>
		/// PropertyBookItemCustomerPO as STRING
		/// </summary>
		public string BookItemCustomerPO { get; set; }

		/// <summary>
		/// PropertyBookItemLocationCode as STRING
		/// </summary>
		public string BookItemLocationCode { get; set; }

		/// <summary>
		/// PropertyBookItemHazControl as INT
		/// </summary>
		public int BookItemHazControl { get; set; }


		private byte[] _BookItemUpdated;
		/// <summary>
		/// BookItemUpdated Property as STRING
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
		/// <summary>
		/// setUpdated Method for setting Update RowVersion Number
		/// </summary>
		/// <param name="val"></param>
		public void setUpdated(byte[] val) { _BookItemUpdated = val; }
		/// <summary>
		/// getUpdated Method for getting the book item Updated
		/// </summary>
		/// <returns></returns>
		public byte[] getUpdated() { return _BookItemUpdated; }
	}
}