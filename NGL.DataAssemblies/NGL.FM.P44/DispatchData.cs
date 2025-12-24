using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGL.FM.P44
{
    public class Dispatch
    {
            public string[] Accessorials { get; set; }
            public int BidControl { get; set; }
            public string BillOfLading { get; set; }
            public string BOLLegalText { get; set; }
            public int BookControl { get; set; }
            public int CarrierControl { get; set; }
            public string CarrierProNumber { get; set; }
            public int CarrTarEquipControl { get; set; }
            public int CarrTarEquipMatControl { get; set; }
            public string ConfidentialNote { get; set; }
            //
            // Summary:
            //     Maps to RequiredDate
            public DateTime DeliveryDate { get; set; }
            //
            // Summary:
            //     End of Delivery time window as string
            //
            // Remarks:
            //     When saving to the database add time to DeliveryDate to create a valid date field
            public string DeliveryEndTime { get; set; }
            public string DeliveryNote { get; set; }
            //
            // Summary:
            //     Beginning of Delivery time window
            //
            // Remarks:
            //     When saving to the database add time to DeliveryDate to create a valid date field
            public string DeliveryStartTime { get; set; }
            public AddressBook Destination { get; set; }
            public bool DirectionOverride { get; set; }
            //
            // Summary:
            //     Maps to API Code for "SHIPPER" "CONSIGNEE" "THIRD_PARTY" when DirectionOverride
            //     is true
            public string DirectionOverrideControl { get; set; }
            public int DispatchBidType { get; set; }
            public string DispatchLegalText { get; set; }
            public Contact EmergencyContact { get; set; }
            //
            // Summary:
            //     Values like "400" = invalid request; "401" = invalid or missing credentials;
            //     "403" = User not authorized to perform this operation
            public string ErrorCode { get; set; }
            public string ErrorMessage { get; set; }
            //
            // Summary:
            //     Used to generate Load Status Messages associated with dispatching
            public APIMessage[] Errors { get; set; }
            public string EXTERNALNbr { get; set; }
            public decimal Fuel { get; set; }
            public string FuelUOM { get; set; }
            public decimal FuelVariable { get; set; }
            public int HazControl { get; set; }
            //
            // Summary:
            //     Used to generate Load Status Messages associated with dispatching
            public APIMessage[] InfoMessages { get; set; }
            public Item[] Items { get; set; }
            public string LengthUnit { get; set; }
            public int LinearFeet { get; set; }
            public decimal LineHaul { get; set; }
            public int LoadTenderControl { get; set; }
            //
            // Summary:
            //     Outbound = 1, Transfer = 2, Inbound = 3
            public int LoadTenderTransTypeControl { get; set; }
            public int ModeTypeControl { get; set; }
            public string OrderNumber { get; set; }
            public AddressBook Origin { get; set; }
            public bool PaymentTermsOverride { get; set; }
            //
            // Summary:
            //     Maps to API Code for "PREPAID" "COLLECT" "THIRD_PARTY" when PaymentTermsOverride
            //     is true
            public int PaymentTermsOverrideControl { get; set; }
            //
            // Summary:
            //     Maps to LoadDate
            public DateTime PickupDate { get; set; }
            //
            // Summary:
            //     End of pickup time window as string
            //
            // Remarks:
            //     When saving to the database add time to PickupDate to create a valid date field
            public string PickupEndTime { get; set; }
            public string PickupNote { get; set; }
            //
            // Summary:
            //     Maps to BookCarrOrderNumber
            public string PickupNumber { get; set; }
            //
            // Summary:
            //     Beginning of pickup time window
            //
            // Remarks:
            //     When saving to the database add time to PickupDate to create a valid date field
            public string PickupStartTime { get; set; }
            public string PONumber { get; set; }
            public string ProviderSCAC { get; set; }
            public string QuoteNumber { get; set; }
            public AddressBook Requestor { get; set; }
            public string RespCapacityProviderBolUrl { get; set; }
            public string RespPackingVisualizationUrl { get; set; }
            public DateTime? RespPickupDateTime { get; set; }
            public string RespPickupNote { get; set; }
            public string SHID { get; set; }
            public string SystemGeneratedNbr { get; set; }
            public string TotalCost { get; set; }
            public int TotalCube { get; set; }
            public int TotalPlts { get; set; }
            public int TotalQty { get; set; }
            public double TotalWgt { get; set; }
            public string VendorSCAC { get; set; }
            public string WeightUnit { get; set; }

            public string sShipIDs { get; set; }
        
    }

    public class AddressBook
    {
    
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public Contact Contact { get; set; }
        public string Country { get; set; }
        public string LocationCode { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
    }

    public class Contact
    {
     
        public string Contact800 { get; set; }
        public int ContactCarrierControl { get; set; }
        public int ContactCompControl { get; set; }
        public int ContactControl { get; set; }
        public bool ContactDefault { get; set; }
        public string ContactEmail { get; set; }
        public string ContactFax { get; set; }
        public int ContactLECarControl { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public string ContactPhoneExt { get; set; }
        public bool ContactScheduler { get; set; }
        public bool ContactTender { get; set; }
        public string ContactTitle { get; set; }
    }

    public class Item
    {
        public string ItemDesc { get; set; }
        public string ItemDimensions { get; set; }
        public string ItemFreightClass { get; set; }
        public string ItemHazmatClass { get; set; }
        public string ItemHazmatID { get; set; }
        public string ItemHazmatPackingGroup { get; set; }
        public string ItemHazmatProperShipName { get; set; }
        public decimal ItemHeight { get; set; }
        public bool ItemIsHazmat { get; set; }
        public decimal ItemLength { get; set; }
        public string ItemNMFCItemCode { get; set; }
        public string ItemNMFCSubCode { get; set; }
        public string ItemNumber { get; set; }
        public string ItemPackageType { get; set; }
        public int ItemPieces { get; set; }
        public bool ItemStackable { get; set; }
        public int ItemTotalPackages { get; set; }
        public decimal ItemWgt { get; set; }
        public decimal ItemWidth { get; set; }
    }

    //
    // Remarks:
    //     Added By RHR for v-8.1 on 03/30/2018 Typically used to generate BookTrack status
    //     messages If linked to a tblLoadTenderRecord the data is stored in tblNGLMessage
    //     tblNGLMessage mapping rules NMNMTControl maps to tblNGLMessageType.NMTControl
    //     for one of the following new types NMTName = NGLAPIInfoMessages NMTName = NGLAPIWarnings
    //     NMTName = NGLAPIErrors NMMTRefControl maps to the tblLoadTender.LoadTenderControl
    //     NMMTRefAlphaControl maps to tblLoadTender.LTBookSHID NMMTRefName maps to Dispatch.ErrorCode
    //     if available or Info by default NMErrorMessage maps to Dispatch.ErrorMessage
    //     Severity maps to NMErrorReason Message maps to NMMessage Diagnostic maps to NMErrorDetails
    //     Source maps to NMKeyString use new spAddLoadTenderMessage stored procedure
    public class APIMessage
    {
       
        //
        // Summary:
        //     Typically maps to BookTrackComment
        public string Diagnostic { get; set; }
        //
        // Summary:
        //     Typically maps to BookTrackComment
        public string Message { get; set; }
        //
        // Summary:
        //     Values like "ERROR" "WARNING" "INFO" Typically maps to BookTrackStatus codes
        //     based on Severity and Source
        public string Severity { get; set; }
        //
        // Summary:
        //     Values like "SYSTEM" "CAPACITY_PROVIDER" Typically maps to BookTrackStatus codes
        //     based on Severity and Source
        public string Source { get; set; }
    }

    public class ApiError
    {
        public string HttpStatusCode { get; set; }
        public string HttpMessage { get; set; }
        public string ErrorMessage { get; set; }

        public List<NGL.FM.P44.Message> Errors { get; set; }
        public string SupportReferenceId { get; set; }
    }


}
