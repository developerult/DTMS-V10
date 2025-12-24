using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGL.DTMS.CMDLine.BCIntegration.Sample
{
    internal class Sample
    {
    }


    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:microsoft-dynamics-nav/xmlports/DynamicsTMSAP")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:microsoft-dynamics-nav/xmlports/DynamicsTMSAP", IsNullable = false)]
    public partial class AP
    {

        private byte aPControlField;

        private byte carrierNumberField;

        private string bookFinAPBillNumberField;

        private string bookFinAPBillInvDateField;

        private string bookCarrOrderNumberField;

        private string laneNumberField;

        private string bookItemCostCenterNumberField;

        private byte bookFinAPActCostField;

        private string bookCarrBLNumberField;

        private byte bookFinAPActWgtField;

        private string bookFinAPBillNoDateField;

        private byte bookFinAPActTaxField;

        private string bookProNumberField;

        private byte bookFinAPExportRetryField;

        private string bookFinAPExportDateField;

        private string prevSentDateField;

        private string companyNumberField;

        private byte bookOrderSequenceField;

        private string carrierEquipmentCodesField;

        private string bookCarrierTypeCodeField;

        private byte aPFee1Field;

        private byte aPFee2Field;

        private byte aPFee3Field;

        private byte aPFee4Field;

        private byte aPFee5Field;

        private byte aPFee6Field;

        private byte otherCostsField;

        private string bookWarehouseNumberField;

        private byte bookMilesFromField;

        private byte compNatNumberField;

        private string bookReasonCodeField;

        private byte bookTransTypeField;

        private string bookShipCarrierProNumberField;

        private string bookShipCarrierNumberField;

        private byte aPTaxDetail1Field;

        private byte aPTaxDetail2Field;

        private byte aPTaxDetail3Field;

        private byte aPTaxDetail4Field;

        private byte aPTaxDetail5Field;

        private string compLegalEntityField;

        private string compAlphaCodeField;

        private string bookSHIDField;

        private string carrierAlphaCodeField;

        private string carrierLegalEntityField;

        private string laneLegalEntityField;

        private string bookConsPrefixField;

        private bool bookRouteConsFlagField;

        private string bookShipCarrierNameField;

        private byte bookFinAPStdCostField;

        private byte bookFinAPTotalTaxableFeesField;

        private byte bookFinAPTotalTaxesField;

        private byte bookFinAPNonTaxableFeesField;

        private string bookWhseAuthorizationNoField;

        private string bookFinAPGLNumberField;

        private APDetails[] detailsField;

        /// <remarks/>
        public byte APControl
        {
            get
            {
                return this.aPControlField;
            }
            set
            {
                this.aPControlField = value;
            }
        }

        /// <remarks/>
        public byte CarrierNumber
        {
            get
            {
                return this.carrierNumberField;
            }
            set
            {
                this.carrierNumberField = value;
            }
        }

        /// <remarks/>
        public string BookFinAPBillNumber
        {
            get
            {
                return this.bookFinAPBillNumberField;
            }
            set
            {
                this.bookFinAPBillNumberField = value;
            }
        }

        /// <remarks/>
        public string BookFinAPBillInvDate
        {
            get
            {
                return this.bookFinAPBillInvDateField;
            }
            set
            {
                this.bookFinAPBillInvDateField = value;
            }
        }

        /// <remarks/>
        public string BookCarrOrderNumber
        {
            get
            {
                return this.bookCarrOrderNumberField;
            }
            set
            {
                this.bookCarrOrderNumberField = value;
            }
        }

        /// <remarks/>
        public string LaneNumber
        {
            get
            {
                return this.laneNumberField;
            }
            set
            {
                this.laneNumberField = value;
            }
        }

        /// <remarks/>
        public string BookItemCostCenterNumber
        {
            get
            {
                return this.bookItemCostCenterNumberField;
            }
            set
            {
                this.bookItemCostCenterNumberField = value;
            }
        }

        /// <remarks/>
        public byte BookFinAPActCost
        {
            get
            {
                return this.bookFinAPActCostField;
            }
            set
            {
                this.bookFinAPActCostField = value;
            }
        }

        /// <remarks/>
        public string BookCarrBLNumber
        {
            get
            {
                return this.bookCarrBLNumberField;
            }
            set
            {
                this.bookCarrBLNumberField = value;
            }
        }

        /// <remarks/>
        public byte BookFinAPActWgt
        {
            get
            {
                return this.bookFinAPActWgtField;
            }
            set
            {
                this.bookFinAPActWgtField = value;
            }
        }

        /// <remarks/>
        public string BookFinAPBillNoDate
        {
            get
            {
                return this.bookFinAPBillNoDateField;
            }
            set
            {
                this.bookFinAPBillNoDateField = value;
            }
        }

        /// <remarks/>
        public byte BookFinAPActTax
        {
            get
            {
                return this.bookFinAPActTaxField;
            }
            set
            {
                this.bookFinAPActTaxField = value;
            }
        }

        /// <remarks/>
        public string BookProNumber
        {
            get
            {
                return this.bookProNumberField;
            }
            set
            {
                this.bookProNumberField = value;
            }
        }

        /// <remarks/>
        public byte BookFinAPExportRetry
        {
            get
            {
                return this.bookFinAPExportRetryField;
            }
            set
            {
                this.bookFinAPExportRetryField = value;
            }
        }

        /// <remarks/>
        public string BookFinAPExportDate
        {
            get
            {
                return this.bookFinAPExportDateField;
            }
            set
            {
                this.bookFinAPExportDateField = value;
            }
        }

        /// <remarks/>
        public string PrevSentDate
        {
            get
            {
                return this.prevSentDateField;
            }
            set
            {
                this.prevSentDateField = value;
            }
        }

        /// <remarks/>
        public string CompanyNumber
        {
            get
            {
                return this.companyNumberField;
            }
            set
            {
                this.companyNumberField = value;
            }
        }

        /// <remarks/>
        public byte BookOrderSequence
        {
            get
            {
                return this.bookOrderSequenceField;
            }
            set
            {
                this.bookOrderSequenceField = value;
            }
        }

        /// <remarks/>
        public string CarrierEquipmentCodes
        {
            get
            {
                return this.carrierEquipmentCodesField;
            }
            set
            {
                this.carrierEquipmentCodesField = value;
            }
        }

        /// <remarks/>
        public string BookCarrierTypeCode
        {
            get
            {
                return this.bookCarrierTypeCodeField;
            }
            set
            {
                this.bookCarrierTypeCodeField = value;
            }
        }

        /// <remarks/>
        public byte APFee1
        {
            get
            {
                return this.aPFee1Field;
            }
            set
            {
                this.aPFee1Field = value;
            }
        }

        /// <remarks/>
        public byte APFee2
        {
            get
            {
                return this.aPFee2Field;
            }
            set
            {
                this.aPFee2Field = value;
            }
        }

        /// <remarks/>
        public byte APFee3
        {
            get
            {
                return this.aPFee3Field;
            }
            set
            {
                this.aPFee3Field = value;
            }
        }

        /// <remarks/>
        public byte APFee4
        {
            get
            {
                return this.aPFee4Field;
            }
            set
            {
                this.aPFee4Field = value;
            }
        }

        /// <remarks/>
        public byte APFee5
        {
            get
            {
                return this.aPFee5Field;
            }
            set
            {
                this.aPFee5Field = value;
            }
        }

        /// <remarks/>
        public byte APFee6
        {
            get
            {
                return this.aPFee6Field;
            }
            set
            {
                this.aPFee6Field = value;
            }
        }

        /// <remarks/>
        public byte OtherCosts
        {
            get
            {
                return this.otherCostsField;
            }
            set
            {
                this.otherCostsField = value;
            }
        }

        /// <remarks/>
        public string BookWarehouseNumber
        {
            get
            {
                return this.bookWarehouseNumberField;
            }
            set
            {
                this.bookWarehouseNumberField = value;
            }
        }

        /// <remarks/>
        public byte BookMilesFrom
        {
            get
            {
                return this.bookMilesFromField;
            }
            set
            {
                this.bookMilesFromField = value;
            }
        }

        /// <remarks/>
        public byte CompNatNumber
        {
            get
            {
                return this.compNatNumberField;
            }
            set
            {
                this.compNatNumberField = value;
            }
        }

        /// <remarks/>
        public string BookReasonCode
        {
            get
            {
                return this.bookReasonCodeField;
            }
            set
            {
                this.bookReasonCodeField = value;
            }
        }

        /// <remarks/>
        public byte BookTransType
        {
            get
            {
                return this.bookTransTypeField;
            }
            set
            {
                this.bookTransTypeField = value;
            }
        }

        /// <remarks/>
        public string BookShipCarrierProNumber
        {
            get
            {
                return this.bookShipCarrierProNumberField;
            }
            set
            {
                this.bookShipCarrierProNumberField = value;
            }
        }

        /// <remarks/>
        public string BookShipCarrierNumber
        {
            get
            {
                return this.bookShipCarrierNumberField;
            }
            set
            {
                this.bookShipCarrierNumberField = value;
            }
        }

        /// <remarks/>
        public byte APTaxDetail1
        {
            get
            {
                return this.aPTaxDetail1Field;
            }
            set
            {
                this.aPTaxDetail1Field = value;
            }
        }

        /// <remarks/>
        public byte APTaxDetail2
        {
            get
            {
                return this.aPTaxDetail2Field;
            }
            set
            {
                this.aPTaxDetail2Field = value;
            }
        }

        /// <remarks/>
        public byte APTaxDetail3
        {
            get
            {
                return this.aPTaxDetail3Field;
            }
            set
            {
                this.aPTaxDetail3Field = value;
            }
        }

        /// <remarks/>
        public byte APTaxDetail4
        {
            get
            {
                return this.aPTaxDetail4Field;
            }
            set
            {
                this.aPTaxDetail4Field = value;
            }
        }

        /// <remarks/>
        public byte APTaxDetail5
        {
            get
            {
                return this.aPTaxDetail5Field;
            }
            set
            {
                this.aPTaxDetail5Field = value;
            }
        }

        /// <remarks/>
        public string CompLegalEntity
        {
            get
            {
                return this.compLegalEntityField;
            }
            set
            {
                this.compLegalEntityField = value;
            }
        }

        /// <remarks/>
        public string CompAlphaCode
        {
            get
            {
                return this.compAlphaCodeField;
            }
            set
            {
                this.compAlphaCodeField = value;
            }
        }

        /// <remarks/>
        public string BookSHID
        {
            get
            {
                return this.bookSHIDField;
            }
            set
            {
                this.bookSHIDField = value;
            }
        }

        /// <remarks/>
        public string CarrierAlphaCode
        {
            get
            {
                return this.carrierAlphaCodeField;
            }
            set
            {
                this.carrierAlphaCodeField = value;
            }
        }

        /// <remarks/>
        public string CarrierLegalEntity
        {
            get
            {
                return this.carrierLegalEntityField;
            }
            set
            {
                this.carrierLegalEntityField = value;
            }
        }

        /// <remarks/>
        public string LaneLegalEntity
        {
            get
            {
                return this.laneLegalEntityField;
            }
            set
            {
                this.laneLegalEntityField = value;
            }
        }

        /// <remarks/>
        public string BookConsPrefix
        {
            get
            {
                return this.bookConsPrefixField;
            }
            set
            {
                this.bookConsPrefixField = value;
            }
        }

        /// <remarks/>
        public bool BookRouteConsFlag
        {
            get
            {
                return this.bookRouteConsFlagField;
            }
            set
            {
                this.bookRouteConsFlagField = value;
            }
        }

        /// <remarks/>
        public string BookShipCarrierName
        {
            get
            {
                return this.bookShipCarrierNameField;
            }
            set
            {
                this.bookShipCarrierNameField = value;
            }
        }

        /// <remarks/>
        public byte BookFinAPStdCost
        {
            get
            {
                return this.bookFinAPStdCostField;
            }
            set
            {
                this.bookFinAPStdCostField = value;
            }
        }

        /// <remarks/>
        public byte BookFinAPTotalTaxableFees
        {
            get
            {
                return this.bookFinAPTotalTaxableFeesField;
            }
            set
            {
                this.bookFinAPTotalTaxableFeesField = value;
            }
        }

        /// <remarks/>
        public byte BookFinAPTotalTaxes
        {
            get
            {
                return this.bookFinAPTotalTaxesField;
            }
            set
            {
                this.bookFinAPTotalTaxesField = value;
            }
        }

        /// <remarks/>
        public byte BookFinAPNonTaxableFees
        {
            get
            {
                return this.bookFinAPNonTaxableFeesField;
            }
            set
            {
                this.bookFinAPNonTaxableFeesField = value;
            }
        }

        /// <remarks/>
        public string BookWhseAuthorizationNo
        {
            get
            {
                return this.bookWhseAuthorizationNoField;
            }
            set
            {
                this.bookWhseAuthorizationNoField = value;
            }
        }

        /// <remarks/>
        public string BookFinAPGLNumber
        {
            get
            {
                return this.bookFinAPGLNumberField;
            }
            set
            {
                this.bookFinAPGLNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Details")]
        public APDetails[] Details
        {
            get
            {
                return this.detailsField;
            }
            set
            {
                this.detailsField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:microsoft-dynamics-nav/xmlports/DynamicsTMSAP")]
    public partial class APDetails
    {

        private string orderNumberField;

        private string itemNumberField;

        private byte qtyOrderedField;

        private byte freightCostField;

        private byte itemCostField;

        private byte weightField;

        private byte cubeField;

        private byte packField;

        private string sizeField;

        private string descriptionField;

        private string customerItemNumberField;

        private string customerNumberField;

        private byte orderSequenceField;

        private string hazmatField;

        private string brandField;

        private string costCenterField;

        private string lotNumberField;

        private string lotExpirationDateField;

        private string gTINField;

        private string countryOfOriginField;

        private string customerPONumberField;

        private byte bFCField;

        private string hSTField;

        private string bookProNumberField;

        private string palletTypeField;

        private byte aPControlField;

        private byte compNatNumberField;

        private string compLegalEntityField;

        private string compAlphaCodeField;

        private byte bookItemDiscountField;

        private byte bookItemLineHaulField;

        private byte bookItemTaxableFeesField;

        private byte bookItemTaxesField;

        private byte bookItemNonTaxableFeesField;

        private byte bookItemWeightBreakField;

        private string bookItemRated49CFRCodeField;

        private string bookItemRatedIATACodeField;

        private string bookItemRatedDOTCodeField;

        private string bookItemRatedMArineCodeField;

        private string bookItemRatedNMFCClassField;

        private string bookItemRatedNMFCSubClassField;

        private string bookItemRatedFAKClassField;

        private string hazmatTypeCodeField;

        private string hazmat49CFRCodeField;

        private string iATACodeField;

        private string dOTCodeField;

        private string marineCodeField;

        private string nMFCClassField;

        private string fAKClassField;

        private bool limitedQtyFlagField;

        private byte palletsField;

        private byte tiesField;

        private byte highsField;

        private byte qtyPalletPercentageField;

        private byte qtyLengthField;

        private byte qtyWidthField;

        private byte qtyHeightField;

        private bool stackableField;

        private byte levelOfDensityField;

        private string bookItemOrderNumberField;

        /// <remarks/>
        public string OrderNumber
        {
            get
            {
                return this.orderNumberField;
            }
            set
            {
                this.orderNumberField = value;
            }
        }

        /// <remarks/>
        public string ItemNumber
        {
            get
            {
                return this.itemNumberField;
            }
            set
            {
                this.itemNumberField = value;
            }
        }

        /// <remarks/>
        public byte QtyOrdered
        {
            get
            {
                return this.qtyOrderedField;
            }
            set
            {
                this.qtyOrderedField = value;
            }
        }

        /// <remarks/>
        public byte FreightCost
        {
            get
            {
                return this.freightCostField;
            }
            set
            {
                this.freightCostField = value;
            }
        }

        /// <remarks/>
        public byte ItemCost
        {
            get
            {
                return this.itemCostField;
            }
            set
            {
                this.itemCostField = value;
            }
        }

        /// <remarks/>
        public byte Weight
        {
            get
            {
                return this.weightField;
            }
            set
            {
                this.weightField = value;
            }
        }

        /// <remarks/>
        public byte Cube
        {
            get
            {
                return this.cubeField;
            }
            set
            {
                this.cubeField = value;
            }
        }

        /// <remarks/>
        public byte Pack
        {
            get
            {
                return this.packField;
            }
            set
            {
                this.packField = value;
            }
        }

        /// <remarks/>
        public string Size
        {
            get
            {
                return this.sizeField;
            }
            set
            {
                this.sizeField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public string CustomerItemNumber
        {
            get
            {
                return this.customerItemNumberField;
            }
            set
            {
                this.customerItemNumberField = value;
            }
        }

        /// <remarks/>
        public string CustomerNumber
        {
            get
            {
                return this.customerNumberField;
            }
            set
            {
                this.customerNumberField = value;
            }
        }

        /// <remarks/>
        public byte OrderSequence
        {
            get
            {
                return this.orderSequenceField;
            }
            set
            {
                this.orderSequenceField = value;
            }
        }

        /// <remarks/>
        public string Hazmat
        {
            get
            {
                return this.hazmatField;
            }
            set
            {
                this.hazmatField = value;
            }
        }

        /// <remarks/>
        public string Brand
        {
            get
            {
                return this.brandField;
            }
            set
            {
                this.brandField = value;
            }
        }

        /// <remarks/>
        public string CostCenter
        {
            get
            {
                return this.costCenterField;
            }
            set
            {
                this.costCenterField = value;
            }
        }

        /// <remarks/>
        public string LotNumber
        {
            get
            {
                return this.lotNumberField;
            }
            set
            {
                this.lotNumberField = value;
            }
        }

        /// <remarks/>
        public string LotExpirationDate
        {
            get
            {
                return this.lotExpirationDateField;
            }
            set
            {
                this.lotExpirationDateField = value;
            }
        }

        /// <remarks/>
        public string GTIN
        {
            get
            {
                return this.gTINField;
            }
            set
            {
                this.gTINField = value;
            }
        }

        /// <remarks/>
        public string CountryOfOrigin
        {
            get
            {
                return this.countryOfOriginField;
            }
            set
            {
                this.countryOfOriginField = value;
            }
        }

        /// <remarks/>
        public string CustomerPONumber
        {
            get
            {
                return this.customerPONumberField;
            }
            set
            {
                this.customerPONumberField = value;
            }
        }

        /// <remarks/>
        public byte BFC
        {
            get
            {
                return this.bFCField;
            }
            set
            {
                this.bFCField = value;
            }
        }

        /// <remarks/>
        public string HST
        {
            get
            {
                return this.hSTField;
            }
            set
            {
                this.hSTField = value;
            }
        }

        /// <remarks/>
        public string BookProNumber
        {
            get
            {
                return this.bookProNumberField;
            }
            set
            {
                this.bookProNumberField = value;
            }
        }

        /// <remarks/>
        public string PalletType
        {
            get
            {
                return this.palletTypeField;
            }
            set
            {
                this.palletTypeField = value;
            }
        }

        /// <remarks/>
        public byte APControl
        {
            get
            {
                return this.aPControlField;
            }
            set
            {
                this.aPControlField = value;
            }
        }

        /// <remarks/>
        public byte CompNatNumber
        {
            get
            {
                return this.compNatNumberField;
            }
            set
            {
                this.compNatNumberField = value;
            }
        }

        /// <remarks/>
        public string CompLegalEntity
        {
            get
            {
                return this.compLegalEntityField;
            }
            set
            {
                this.compLegalEntityField = value;
            }
        }

        /// <remarks/>
        public string CompAlphaCode
        {
            get
            {
                return this.compAlphaCodeField;
            }
            set
            {
                this.compAlphaCodeField = value;
            }
        }

        /// <remarks/>
        public byte BookItemDiscount
        {
            get
            {
                return this.bookItemDiscountField;
            }
            set
            {
                this.bookItemDiscountField = value;
            }
        }

        /// <remarks/>
        public byte BookItemLineHaul
        {
            get
            {
                return this.bookItemLineHaulField;
            }
            set
            {
                this.bookItemLineHaulField = value;
            }
        }

        /// <remarks/>
        public byte BookItemTaxableFees
        {
            get
            {
                return this.bookItemTaxableFeesField;
            }
            set
            {
                this.bookItemTaxableFeesField = value;
            }
        }

        /// <remarks/>
        public byte BookItemTaxes
        {
            get
            {
                return this.bookItemTaxesField;
            }
            set
            {
                this.bookItemTaxesField = value;
            }
        }

        /// <remarks/>
        public byte BookItemNonTaxableFees
        {
            get
            {
                return this.bookItemNonTaxableFeesField;
            }
            set
            {
                this.bookItemNonTaxableFeesField = value;
            }
        }

        /// <remarks/>
        public byte BookItemWeightBreak
        {
            get
            {
                return this.bookItemWeightBreakField;
            }
            set
            {
                this.bookItemWeightBreakField = value;
            }
        }

        /// <remarks/>
        public string BookItemRated49CFRCode
        {
            get
            {
                return this.bookItemRated49CFRCodeField;
            }
            set
            {
                this.bookItemRated49CFRCodeField = value;
            }
        }

        /// <remarks/>
        public string BookItemRatedIATACode
        {
            get
            {
                return this.bookItemRatedIATACodeField;
            }
            set
            {
                this.bookItemRatedIATACodeField = value;
            }
        }

        /// <remarks/>
        public string BookItemRatedDOTCode
        {
            get
            {
                return this.bookItemRatedDOTCodeField;
            }
            set
            {
                this.bookItemRatedDOTCodeField = value;
            }
        }

        /// <remarks/>
        public string BookItemRatedMArineCode
        {
            get
            {
                return this.bookItemRatedMArineCodeField;
            }
            set
            {
                this.bookItemRatedMArineCodeField = value;
            }
        }

        /// <remarks/>
        public string BookItemRatedNMFCClass
        {
            get
            {
                return this.bookItemRatedNMFCClassField;
            }
            set
            {
                this.bookItemRatedNMFCClassField = value;
            }
        }

        /// <remarks/>
        public string BookItemRatedNMFCSubClass
        {
            get
            {
                return this.bookItemRatedNMFCSubClassField;
            }
            set
            {
                this.bookItemRatedNMFCSubClassField = value;
            }
        }

        /// <remarks/>
        public string BookItemRatedFAKClass
        {
            get
            {
                return this.bookItemRatedFAKClassField;
            }
            set
            {
                this.bookItemRatedFAKClassField = value;
            }
        }

        /// <remarks/>
        public string HazmatTypeCode
        {
            get
            {
                return this.hazmatTypeCodeField;
            }
            set
            {
                this.hazmatTypeCodeField = value;
            }
        }

        /// <remarks/>
        public string Hazmat49CFRCode
        {
            get
            {
                return this.hazmat49CFRCodeField;
            }
            set
            {
                this.hazmat49CFRCodeField = value;
            }
        }

        /// <remarks/>
        public string IATACode
        {
            get
            {
                return this.iATACodeField;
            }
            set
            {
                this.iATACodeField = value;
            }
        }

        /// <remarks/>
        public string DOTCode
        {
            get
            {
                return this.dOTCodeField;
            }
            set
            {
                this.dOTCodeField = value;
            }
        }

        /// <remarks/>
        public string MarineCode
        {
            get
            {
                return this.marineCodeField;
            }
            set
            {
                this.marineCodeField = value;
            }
        }

        /// <remarks/>
        public string NMFCClass
        {
            get
            {
                return this.nMFCClassField;
            }
            set
            {
                this.nMFCClassField = value;
            }
        }

        /// <remarks/>
        public string FAKClass
        {
            get
            {
                return this.fAKClassField;
            }
            set
            {
                this.fAKClassField = value;
            }
        }

        /// <remarks/>
        public bool LimitedQtyFlag
        {
            get
            {
                return this.limitedQtyFlagField;
            }
            set
            {
                this.limitedQtyFlagField = value;
            }
        }

        /// <remarks/>
        public byte Pallets
        {
            get
            {
                return this.palletsField;
            }
            set
            {
                this.palletsField = value;
            }
        }

        /// <remarks/>
        public byte Ties
        {
            get
            {
                return this.tiesField;
            }
            set
            {
                this.tiesField = value;
            }
        }

        /// <remarks/>
        public byte Highs
        {
            get
            {
                return this.highsField;
            }
            set
            {
                this.highsField = value;
            }
        }

        /// <remarks/>
        public byte QtyPalletPercentage
        {
            get
            {
                return this.qtyPalletPercentageField;
            }
            set
            {
                this.qtyPalletPercentageField = value;
            }
        }

        /// <remarks/>
        public byte QtyLength
        {
            get
            {
                return this.qtyLengthField;
            }
            set
            {
                this.qtyLengthField = value;
            }
        }

        /// <remarks/>
        public byte QtyWidth
        {
            get
            {
                return this.qtyWidthField;
            }
            set
            {
                this.qtyWidthField = value;
            }
        }

        /// <remarks/>
        public byte QtyHeight
        {
            get
            {
                return this.qtyHeightField;
            }
            set
            {
                this.qtyHeightField = value;
            }
        }

        /// <remarks/>
        public bool Stackable
        {
            get
            {
                return this.stackableField;
            }
            set
            {
                this.stackableField = value;
            }
        }

        /// <remarks/>
        public byte LevelOfDensity
        {
            get
            {
                return this.levelOfDensityField;
            }
            set
            {
                this.levelOfDensityField = value;
            }
        }

        /// <remarks/>
        public string BookItemOrderNumber
        {
            get
            {
                return this.bookItemOrderNumberField;
            }
            set
            {
                this.bookItemOrderNumberField = value;
            }
        }
    }


}
