using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using P44SDK.V4.Api; // P44SDK.V4.Api;
using P44SDK.V4.Client;
using P44M = P44SDK.V4.Model;


namespace NGL.FM.P44
{
    public class P44Dispatch
    {

        /// <summary>
        /// Dispatch the load to P44 API and return a formatted dispatch results object
        /// </summary>
        /// <param name="P44WebServiceUrl"></param>
        /// <param name="P44WebServiceLogin"></param>
        /// <param name="P44WebServicePassword"></param>
        /// <param name="P44AccountGroup"></param>
        /// <param name="d"></param>
        /// <param name="strSI"></param>
        /// <returns></returns>
        /// <remarks>
        /// Modified by RHR for v-8.2.0.116 on 7/16/19
        ///   added logic to test for null or missing 
        ///   Shipment Identifiers or Info Messages and 
        ///   to provide a default message about the missing data
        /// </remarks>
        public NGL.FM.P44.Message Dispatch(string P44WebServiceUrl, string P44WebServiceLogin, string P44WebServicePassword, string P44AccountGroup, ref NGL.FM.P44.Dispatch d,out string strSI)
        {
            NGL.FM.P44.Message response = new NGL.FM.P44.Message() { Diagnostic = "Success!" };
            strSI = "";
            try
            {               
                
                if (string.IsNullOrWhiteSpace(P44WebServiceUrl) || string.IsNullOrWhiteSpace(P44WebServiceLogin) || string.IsNullOrWhiteSpace(P44WebServicePassword) || string.IsNullOrWhiteSpace(P44AccountGroup))
                {
                    response.message = "Not Authorized - NGL API credentials not found.";
                    response.Diagnostic = "Failed";
                    response.Severity = SeverityEnum.ERROR;
                    response.Source = SourceEnum.SYSTEM;
                    return response;
                }

                LTLDispatchApi apiClient = new LTLDispatchApi(P44WebServiceUrl);
                apiClient.Configuration.Username = P44WebServiceLogin;
                apiClient.Configuration.Password = P44WebServicePassword;


                //   P44 PhoneNumber:
                //     Only North American phone numbers are accepted. 
                //     In requests, only digits and an optional 'X' (or lowercase 'x')
                //     marking the start of an extension will be used. Any other characters included
                //     for formatting will be stripped - - project44 will provide the phone number to
                //     the capacity provider in the format they accept. There must be at least ten digits
                //     before the optional 'X' (for the area code, central office code, and station
                //     code), and no more than thirteen, with the first digits exceeding ten being interpreted
                //     as the country code. The number of digits after the optional 'X' must be less
                //     than seven. Examples of acceptable phone numbers: '+1 123-456-7890 x 32', '(123)456-7890
                //     ext 30', '1234567890', '11234567890', '55-123-456-7890 ext. 312412', '+1 123-456-7890,
                //     ext. 1234'. In responses, phone numbers will be returned in the following format:
                //     '[+123 ]123-456-7890[, ext. 123456]' excluding the brackets and with the characters
                //     in brackets being optionally returned..

                #region "CAPACITY PROVIDER ACCOUNT GROUP"

                //** CAPACITY PROVIDER ACCOUNT GROUP **
                //Capacity provider account group, containing the account to be used for authentication
                //with the capacity provider's shipment API. (required).

                #region "Account Group"

                //(Code)
                //The code for the capacity provider account group that contains all accounts against
                //which an operation is to be performed. 
                //Capacity provider account groups are set up through the project44 Self-Service Portal. 
                //If no key is specified, the 'Default' account group will be used.

                var accountGroup = P44AccountGroup;

                #endregion

                #region "Account Codes"

                //(Accounts)
                //Capacity provider accounts used for authentication with the capacity providers' APIs. 
                //For quoting, defaults to all accounts within the account group. 
                //For shipment, shipment status, and image, one and only one account is required.

                List<P44M.CapacityProviderAccount> accounts = new List<P44M.CapacityProviderAccount>();

                var accountCode = new P44M.CapacityProviderAccount(d.ProviderSCAC);
                accounts.Add(accountCode);

                #endregion

                var capacityProviderAccountGroup = new P44M.CapacityProviderAccountGroup()
                {
                    Code = accountGroup,
                    Accounts = accounts
                };

                #endregion

                #region "ORIGIN"

                //** ORIGIN **
                //The origin address and contact for the shipment to be picked up. 
                //The origin contact will default to the requester, if not provided. (required).
                var origAddress = new P44M.Address(d.Origin.Zip, new List<string> { d.Origin.Address1, d.Origin.Address2, d.Origin.Address3 }, d.Origin.City, d.Origin.State, Utilities.getCountryEnum(d.Origin.Country));

                var origContact = new P44M.Contact()
                {
                    CompanyName = d.Origin.Name,
                    ContactName = d.Origin.Contact.ContactName,
                    PhoneNumber = d.Origin.Contact.ContactPhone,
                    PhoneNumber2 = "",
                    Email = d.Origin.Contact.ContactEmail,
                    FaxNumber = d.Origin.Contact.ContactFax
                };
                var origin = new P44M.Location(origAddress, origContact); // NOTE: Location.Id not used for now but can be included for possible future use

                #endregion

                #region "DESTINATION"

                //** DESTINATION **
                //The destination address and contact for the requested shipment. (required).
                var destAddress = new P44M.Address(d.Destination.Zip, new List<string> { d.Destination.Address1, d.Destination.Address2, d.Destination.Address3 }, d.Destination.City, d.Destination.State, Utilities.getCountryEnum(d.Destination.Country));

                var destContact = new P44M.Contact()
                {
                    CompanyName = d.Destination.Name,
                    ContactName = d.Destination.Contact.ContactName,
                    PhoneNumber = d.Destination.Contact.ContactPhone,
                    PhoneNumber2 = "",
                    Email = d.Destination.Contact.ContactEmail,
                    FaxNumber = d.Destination.Contact.ContactFax
                };
                var destination = new P44M.Location(destAddress, destContact); // NOTE: Location.Id not used for now but can be included for possible future use

                #endregion

                #region "REQUESTOR LOCATION"

                //** REQUESTOR LOCATION **
                //The address and contact of the agent or freight coordinator who is responsible for the order. 
                //Contact name, phone number, and email are required. (required).
                //** NOTE ** For now I am defaulting this to be the same as origin
                var requestorAddress = new P44M.Address(d.Requestor.Zip, new List<string> { d.Requestor.Address1, d.Requestor.Address2, d.Requestor.Address3 }, d.Requestor.City, d.Requestor.State, Utilities.getCountryEnum(d.Requestor.Country));

                var requestorContact = new P44M.Contact()
                {
                    CompanyName = d.Requestor.Name,
                    ContactName = d.Requestor.Contact.ContactName,
                    PhoneNumber = d.Requestor.Contact.ContactPhone,
                    PhoneNumber2 = "",
                    Email = d.Requestor.Contact.ContactEmail,
                    FaxNumber = d.Requestor.Contact.ContactFax
                };
                var requestor = new P44M.Location(requestorAddress, requestorContact); // NOTE: Location.Id not used for now but can be included for possible future use

                #endregion

                #region "LINE ITEMS"

                //** LINE ITEMS **
                //The line items to be shipped.
                //A line item consists of one or more packages, all of the same package type 
                //and with the same dimensions, freight class, and NMFC code. 
                //Each package, however, may have a different number of pieces and a different weight. 
                //Note that each capacity provider has a different maximum number of 
                //line items that they can accept. (required).
                List<P44M.LineItem> lineItems = new List<P44M.LineItem>();

                if (d.Items != null && d.Items.Count() > 0)
                {
                    foreach (NGL.FM.P44.Item item in d.Items)
                    {
                        //(TotalWeight) Total weight of all packages composing this line item. (required).
                        //(PackageDimensions) Dimensions of each package in this line item. (required).
                        //(FreightClass) Freight class of all packages composing this item. Required for LTL quotes and shipments only
                        var dimensions = new P44M.CubicDimension(item.ItemLength, item.ItemWidth, item.ItemHeight);
                        var freightClass = Utilities.getFreightClassEnum(item.ItemFreightClass, P44M.LineItem.FreightClassEnum._50);

                        // Initialize a line item
                        var li = new P44M.LineItem(item.ItemWgt, dimensions, freightClass);

                        //(PackageType) Type of packages composing this line item. (default: 'PLT').
                        //li.PackageType = Utilities.getPackageTypeEnum(item.ItemPackageType);
                        li.PackageType = Utilities.getPackageTypeEnum((string.IsNullOrWhiteSpace(item.ItemPackageType) ? "PLT" : item.ItemPackageType));
                        //(TotalPackages) The number of packages composing this line item. (default: '1').
                        li.TotalPackages = item.ItemTotalPackages;

                        //(TotalPieces) The total number of pieces across all packages composing this line item. (default:'1').
                        li.TotalPieces = item.ItemPieces;

                        //(Description) Readable description of this line item
                        string sDesc = "";
                        //if this is a user defined item number insert it at the start of the description
                        if (!string.IsNullOrWhiteSpace(item.ItemNumber) && item.ItemNumber.Trim().Length > 0)
                        {
                            int iNbr = 0;
                            if (int.TryParse(item.ItemNumber, out iNbr))
                            {
                                if (iNbr > 99) { sDesc = "Nbr: " + item.ItemNumber.Trim() + " "; }
                            }
                            else
                            {
                                sDesc = "Nbr: " + item.ItemNumber.Trim() + " ";
                            }
                        }
                        li.Description = sDesc + item.ItemDesc;

                        //(Stackable) Whether the packages composing this line item are stackable. (default: 'false').
                        li.Stackable = item.ItemStackable;

                        //(NmfcItemCode) NMFC prefix code for all packages composing this line item.
                        li.NmfcItemCode = item.ItemNMFCItemCode;

                        //(NmfcSubCode) NMFC suffix code for all packages composing this line item.
                        li.NmfcSubCode = item.ItemNMFCSubCode;

                        #region "Hazmat"
                        //Todo: add code to lookup  hazmat details using control number
                        if (item.ItemIsHazmat)
                        {
                            //(HazmatDetail)
                            //Not available in rating (send the hazmat accessorial instead). 
                            //Required for shipment if this line item contains hazardous materials. 
                            //Provides important information about the hazardous materials to be transported, 
                            //as required by the US Department of Transportation (DOT).
                            var hazmatDetail = new P44M.LineItemHazmatDetail()
                            {
                                //(IdentificationNumber) The United Nations (UN) or North America (NA) number identifying the hazmat item. (required).                      
                                IdentificationNumber = item.ItemHazmatID,
                                //(ProperShippingName) The proper shipping name of the hazmat item. (required).
                                ProperShippingName = item.ItemHazmatProperShipName,
                                //(HazardClass)
                                //The hazard class number, according to the classification system outlined by the
                                //Federal Motor Carrier Safety Administration (FMCSA). 
                                //This is a one digit number or a two digit number separated by a decimal. (required).
                                HazardClass = item.ItemHazmatClass,
                                //(PackingGroup) The hazmat packing group for a line item, indicating the degree of danger. (required).
                                PackingGroup = Utilities.getPackingGroupEnum(item.ItemHazmatPackingGroup)
                            };
                            li.HazmatDetail = hazmatDetail;
                        }

                        #endregion

                        //Add the line item to the list
                        lineItems.Add(li);
                    }
                }


                #endregion

                #region "PICKUP WINDOW"

                //** PICKUP WINDOW **
                //The pickup date and time range in the timezone of the shipment's origin location. (required).
                var pickupWindow = new P44M.LocalDateTimeWindow()
                {
                    //(Date) Date for this time window in the timezone of the applicable location.(default: current date, format: yyyy-MM-dd).
                    Date = d.PickupDate.ToString("yyyy-MM-dd"),
                    //(StartTime) Start time of this window in the timezone of the applicable location. (format: HH:mm).
                    StartTime = Utilities.formatDispatchTimeStringForAPI(d.PickupStartTime, "08:00"),
                    //(EndTime) End time of this window in the timezone of the applicable location. (format: HH:mm).
                    EndTime = Utilities.formatDispatchTimeStringForAPI(d.PickupEndTime, "17:00")
                };

                #endregion

                #region "DELIVERY WINDOW"

                //** DELIVERY WINDOW **
                //The delivery date and time range in the timezone of the shipment's destination location. 
                //Required by some capacity providers when requesting guaranteed or expedited services.
                var deliveryWindow = new P44M.LocalDateTimeWindow()
                {
                    //(Date) Date for this time window in the timezone of the applicable location.(default: current date, format: yyyy-MM-dd).
                    Date = d.DeliveryDate.ToString("yyyy-MM-dd"),
                    //(StartTime) Start time of this window in the timezone of the applicable location. (format: HH:mm).
                    StartTime = Utilities.formatDispatchTimeStringForAPI(d.DeliveryStartTime, "08:00"),
                    //(EndTime) End time of this window in the timezone of the applicable location. (format: HH:mm).
                    EndTime = Utilities.formatDispatchTimeStringForAPI(d.DeliveryEndTime, "17:00")
                };

                #endregion

                #region "CARRIER CODE"

                //** CARRIER CODE **
                //SCAC of the carrier that is to pick up this shipment. 
                //Required only for capacity providers that support multiple SCACs.

                var carrierCode = d.VendorSCAC;

                #endregion

                #region "SHIPMENT IDENTIFIERS"

                //Notes from Rob
                //---------------------------------------------------------------
                //External = readonly (default this value to CNS) label it SHID 
                //PRO = readonly BookProNumber
                //BOL = editable default to SHID/CNS
                //PO = editable default to order number
                //cust ref = readonly order number (label order no on screen)
                //---------------------------------------------------------------

                //** SHIPMENT IDENTIFIERS **
                //A list of identifiers or reference numbers for this shipment. 
                //Most capacity providers accept only identifiers of types 
                //'BILL_OF_LADING', 'PURCHASE_ORDER', and 'CUSTOMER_REFERENCE'.
                //Only one identifier of each type may be provided. 
                //An identifier of type 'SYSTEM_GENERATED' may not be provided. 
                //An identifier of type 'EXTERNAL' may be provided and subsequently
                //tracked with through project44 - - this identifier will not 
                //be communicated to the capacity provider.

                List<P44M.LtlShipmentIdentifier> shipmentIdentifiers = new List<P44M.LtlShipmentIdentifier>();
                if (!string.IsNullOrWhiteSpace(d.SHID))
                {
                    //Bill of lading number, originated by the user (required)
                    //var BOL = new P44M.LtlShipmentIdentifier(P44M.LtlShipmentIdentifier.TypeEnum.BILLOFLADING, d.SHID ?? "");
                    shipmentIdentifiers.Add(new P44M.LtlShipmentIdentifier(P44M.LtlShipmentIdentifier.TypeEnum.BILLOFLADING, d.SHID ?? ""));
                }


                if (!string.IsNullOrWhiteSpace(d.PONumber))
                {
                    //Purchase order number, originated by the user
                    //var PO = new P44M.LtlShipmentIdentifier(P44M.LtlShipmentIdentifier.TypeEnum.PURCHASEORDER, d.PONumber ?? "");
                    shipmentIdentifiers.Add(new P44M.LtlShipmentIdentifier(P44M.LtlShipmentIdentifier.TypeEnum.PURCHASEORDER, d.PONumber ?? ""));
                }

                if (!string.IsNullOrWhiteSpace(d.OrderNumber))
                {
                    //Other customer reference number, originated by the user
                    // var CUSTREF = new P44M.LtlShipmentIdentifier(P44M.LtlShipmentIdentifier.TypeEnum.CUSTOMERREFERENCE, d.OrderNumber ?? "");
                    shipmentIdentifiers.Add(new P44M.LtlShipmentIdentifier(P44M.LtlShipmentIdentifier.TypeEnum.CUSTOMERREFERENCE, d.OrderNumber ?? ""));
                }

                if (!string.IsNullOrWhiteSpace(d.CarrierProNumber))
                {
                    //PRO Number, originated by the capacity provider
                    // Can we send this?? I think we can but I can't find my notes on it
                    //var PRO = new P44M.LtlShipmentIdentifier(P44M.LtlShipmentIdentifier.TypeEnum.PRO, d.CarrierProNumber ?? "");
                    shipmentIdentifiers.Add(new P44M.LtlShipmentIdentifier(P44M.LtlShipmentIdentifier.TypeEnum.PRO, d.CarrierProNumber ?? ""));
                }

                if (!string.IsNullOrWhiteSpace(d.EXTERNALNbr))
                {
                    //External shipment identifier, originated by the user
                    //(our internal id - can get passed back for tracking)
                    //var EXT = new P44M.LtlShipmentIdentifier(P44M.LtlShipmentIdentifier.TypeEnum.EXTERNAL, d.EXTERNALNbr ?? "");
                    shipmentIdentifiers.Add(new P44M.LtlShipmentIdentifier(P44M.LtlShipmentIdentifier.TypeEnum.EXTERNAL, d.EXTERNALNbr ?? ""));
                }


                //shipmentIdentifiers = new List<P44M.LtlShipmentIdentifier>() { BOL, PO, CUSTREF, PRO, EXT };

                #endregion

                #region "ACCESSORIAL SERVICES"

                //** ACCESSORIAL SERVICES **
                //List of accessorial services to be requested for this shipment. 
                //Some capacity providers support accessorial services without 
                //providing a way of requesting them through their API. 
                //To handle this, project44 sends these accessorial services
                //through the capacity provider's pickup note API field, 
                //according to the shipment note configuration.

                List<P44M.AccessorialService> accessorialServices = new List<P44M.AccessorialService>();
                if (d.Accessorials != null && d.Accessorials.Length > 0)
                {
                    foreach (string a in d.Accessorials)
                    {
                        //Bug Fix: need to find out why fuel charge is not working at P44 on dispatch.
                        if (a != "FSC") {                           
                            accessorialServices.Add(new P44M.AccessorialService(a));
                        }
                        //(Code) The code for the requested accessorial service. 
                        //A list of accessorial service codes supported by project44 is provided in the API reference data section. (required).
                        
                    }
                }


                #endregion

                #region "PICKUP NOTE"

                //** PICKUP NOTE **
                //Note that applies to the pickup of this shipment. 
                //The shipment note configuration determines the final pickup note 
                //that is sent through the capacity provider's API and whether or 
                //not part of it may be cut off.

                var pickupNote = d.PickupNote ?? "";

                #endregion

                #region "DELIVERY NOTE"

                //** DELIVERY NOTE **
                //Note that applies to the delivery of this shipment. 
                //Currently, since nearly all capacity provider APIs have only 
                //a pickup note field and not a delivery note field, 
                //this delivery note will be inserted into the capacity provider's 
                //pickup note API field, according to the shipment note configuration.

                var deliveryNote = d.DeliveryNote ?? "";

                #endregion

                #region "EMERGENCY CONTACT"

                //** EMERGENCY CONTACT **
                //Emergency contact name and phone number are required when the shipment contains
                //items marked as hazardous materials.

                //If this is not provided just duplicate the Requestor Contact Mapping 
                var emergencyContact = requestorContact;

                #endregion

                #region "CAPACITY PROVIDER QUOTE NUMBER"

                //** CAPACITY PROVIDER QUOTE NUMBER **
                //The quote number for this shipment assigned by the capacity provider. 
                //Only a few LTL capacity providers accept a quote number when placing a shipment for pickup. 
                //Most volume LTL capacity providers, however, require a quote number.

                var capacityProviderQuoteNumber = d.QuoteNumber ?? "";

                #endregion

                #region "TOTAL LINEAR FEET (Not Used - Only for volume LTL shipments)"

                //** TOTAL LINEAR FEET **
                //The total linear feet that the shipment being quoted will take up in a trailer.
                //!***** ONLY FOR VOLUME LTL SHIPMENTS. *****!

                var totalLinearFeet = d.LinearFeet; // 0;

                #endregion

                #region "WEIGHT UNIT"

                var weightUnit = getWeightUnitEnum(d.WeightUnit ?? "");

                #endregion

                #region "LENGTH UNIT"

                var lengthUnit = getLengthUnitEnum(d.LengthUnit ?? "");

                #endregion

                #region "PAYMENT TERMS OVERRIDE"

                //** PAYMENT TERMS OVERRIDE **
                //An override of payment terms for the capacity provider account used by this request,
                //if it has 'Enable API override of payment terms' set as 'true' in the project44 Self-Service Portal. 
                //This functionality is typically used in situations where both inbound and outbound shipments 
                //are common for a given capacity provider and account number.

                //TODO: add logic to convert PaymentTermsOverrideControl to enum; or set to null?
                var paymentTermsOverride = getPaymentTermsOverrideEnum("");

                #endregion

                #region "DIRECTION OVERRIDE"

                //** DIRECTION OVERRIDE **
                //An override of direction for the capacity provider account used by this request,
                //if it has 'Enable API override of direction' set as 'true' in the project44 Self-Service Portal.
                //This functionality is typically used in situations where both inbound and outbound shipments 
                //are common for a given capacity provider and account number.

                //TODO: add logic to convert DirectionOverrideControl to enum; or set to null?
                var directionOverride = getDirectionOverrideEnum("");

                #endregion

                #region "API CONFIGURATION"

                //** API CONFIGURATION **
                //Fields for configuring the behavior of this API.

                #region "Note Configuration"
                //** Note Configuration **
                //Configuration of the pickup note that will be constructed by project44. 
                //Pickup note construction is used to send some requested accessorial services to 
                //capacity providers through their API when no other API field is available for these services.
                //It is also used to send the delivery note through the capacity provider's pickup note API field. 
                //If no note configuration is provided, the default note configuration will be used, which will 
                //send the following note sections in order and will not enable truncation: 
                //pickup note, priority accessorials, pickup accessorials, dimensions, delivery note, 
                //delivery accessorials, and other accessorials.


                //(EnableTruncation)
                //If set to 'true', project44 will truncate the final pickup note it constructs
                //if it exceeds the maximum allowable length for the capacity provider. 
                //When 'false', an error will be returned and the shipment will be not placed 
                //for pickup with the capacity provider if the constructed pickup note exceeds
                //the maximum allowable length for the capacity provider. (default: 'false').

                var enableTruncation = true;

                //(NoteSections)
                //A list of sections, in order, to send through the capacity provider's pickup note API field. 
                //Brief descriptions of accessorial services will be used. 
                //Item dimensions are added in the format 00x00x00. 
                //If not provided, the default note sections, in order, are as follows: 
                //pickup note, priority accessorials, pickup accessorials, dimensions, 
                //delivery note, delivery accessorials, and other accessorials.

                List<P44M.ShipmentNoteSection> noteSections = new List<P44M.ShipmentNoteSection>()
                {
                    new P44M.ShipmentNoteSection(P44M.ShipmentNoteSection.NameEnum.PICKUPNOTE),
                    new P44M.ShipmentNoteSection(P44M.ShipmentNoteSection.NameEnum.DELIVERYNOTE),
                    new P44M.ShipmentNoteSection(P44M.ShipmentNoteSection.NameEnum.PRIORITYACCESSORIALS),
                    new P44M.ShipmentNoteSection(P44M.ShipmentNoteSection.NameEnum.PICKUPACCESSORIALS),
                    new P44M.ShipmentNoteSection(P44M.ShipmentNoteSection.NameEnum.DELIVERYACCESSORIALS),
                    new P44M.ShipmentNoteSection(P44M.ShipmentNoteSection.NameEnum.OTHERACCESSORIALS),
                    new P44M.ShipmentNoteSection(P44M.ShipmentNoteSection.NameEnum.DIMENSIONS)
                };

                var noteConfiguration = new P44M.ShipmentNoteConfiguration()
                {
                    EnableTruncation = enableTruncation,
                    NoteSections = noteSections
                };

                #endregion

                #region "Allow Unsupported Accessorials"
                //** Allow Unsupported Accessorials **
                //If set to 'true', accessorial services that are not known to be supported by
                //the capacity provider will be allowed and will be sent through the capacity provider's
                //pickup note API field, according to the shipment note configuration. 
                //This is useful when the customer knows that a capacity provider supports an accessorial
                //service that they have not documented, or when the customer has a special agreement
                //with the capacity provider. (default: 'false').

                var allowUnsupportedAccessorials = true;

                #endregion

                #region "Enable Unit Conversion"
                //** Enable Unit Conversion **
                //If set to 'true', weight and length values in this shipment request will 
                //be converted when necessary to the capacity provider's supported units. 
                //When 'false', an error will be returned and the shipment will not be placed 
                //with the capacity provider if the capacity provider does not support the 
                //provided weight and length units. (default: 'false').

                var enableUnitConversion = true;

                #endregion

                #region "Fall Back To Default Account Group"
                //** Fall Back To Default Account Group **
                //If set to 'true' and the provided capacity provider account group 
                //code is invalid, the default capacity provider account group will be used. 
                //When 'false', an error will be returned if the provided capacity provider 
                //account group code is invalid. (default: 'false').

                var fallBackToDefaultAccountGroup = false;

                #endregion

                #region "Pre Scheduled Pickup"
                //** Pre Scheduled Pickup **
                //If set to 'true', will identify the pickup for this shipment as being already
                //scheduled, and will only transmit BOL information to the carrier.

                var preScheduledPickup = false;

                #endregion


                var apiConfiguration = new P44M.ShipmentApiConfiguration()
                {
                    NoteConfiguration = noteConfiguration,
                    AllowUnsupportedAccessorials = allowUnsupportedAccessorials,
                    EnableUnitConversion = enableUnitConversion,
                    FallBackToDefaultAccountGroup = fallBackToDefaultAccountGroup,
                    PreScheduledPickup = preScheduledPickup
                };

                #endregion

                var shipment = new P44M.LtlDispatchedShipment(capacityProviderAccountGroup, origin, destination, requestor, lineItems, pickupWindow, deliveryWindow, carrierCode, shipmentIdentifiers, accessorialServices, pickupNote, deliveryNote, emergencyContact, capacityProviderQuoteNumber, totalLinearFeet, weightUnit, lengthUnit, null, null, apiConfiguration);


                // POST: Create a shipment.
                var result = apiClient.CreateShipment(shipment);

                if (result != null)
                {
                    var strResult = "";

                    strSI = "Shipment Identifiers: " + Environment.NewLine + "<br />";
                    //Modified by RHR for v-8.2.0.116 on 7/16/19
                    if (result.ShipmentIdentifiers != null && result.ShipmentIdentifiers.Count() > 0)
                    {                    
                        foreach (P44M.LtlShipmentIdentifier si in result.ShipmentIdentifiers)
                        {
                            switch (si.Type)
                            {
                                case P44M.LtlShipmentIdentifier.TypeEnum.BILLOFLADING:
                                    strSI += string.Format("{0} - {1}.{2}", "TMS BOL Nbr", d.BillOfLading, Environment.NewLine + "<br />");
                                    if (!string.IsNullOrWhiteSpace(si.Value))
                                    {
                                        strSI += string.Format("{0} - {1}.{2}", "Carrier BOL Nbr", si.Value, Environment.NewLine + "<br />");
                                    }
                                    break;
                                case P44M.LtlShipmentIdentifier.TypeEnum.PURCHASEORDER:
                                    strSI += string.Format("{0} - {1}.{2}", "TMS PO Nbr", d.PONumber, Environment.NewLine + "<br />");
                                    if (!string.IsNullOrWhiteSpace(si.Value))
                                    {
                                        strSI += string.Format("{0} - {1}.{2}", "Carrier PO Nbr", si.Value, Environment.NewLine + "<br />");
                                    }
                                    break;
                                case P44M.LtlShipmentIdentifier.TypeEnum.CUSTOMERREFERENCE:
                                    strSI += string.Format("{0} - {1}.{2}", "TMS Order Nbr", d.OrderNumber, Environment.NewLine + "<br />");
                                    if (!string.IsNullOrWhiteSpace(si.Value))
                                    {
                                        strSI += string.Format("{0} - {1}.{2}", "CUSTOMER REFERENCE", si.Value, Environment.NewLine + "<br />");
                                    }
                                    break;
                                case P44M.LtlShipmentIdentifier.TypeEnum.PICKUP:
                                    d.PickupNumber = si.Value;
                                    strSI += string.Format("{0} - {1}.{2}", "Confirmation Nbr", si.Value, Environment.NewLine + "<br />");
                                    break;
                                case P44M.LtlShipmentIdentifier.TypeEnum.PRO:
                                    strSI += string.Format("{0} - {1}.{2}", "TMS Carrier PRO Nbr", d.CarrierProNumber, Environment.NewLine + "<br />");
                                    if (!string.IsNullOrWhiteSpace(si.Value))
                                    {
                                        strSI += string.Format("{0} - {1}.{2}", "Returned Carrier PRO Nbr", si.Value, Environment.NewLine + "<br />");
                                    }
                                    if (string.IsNullOrWhiteSpace(d.CarrierProNumber) && !string.IsNullOrWhiteSpace(si.Value))
                                    {
                                        d.CarrierProNumber = si.Value;
                                    }
                                    break;
                                //case P44M.LtlShipmentIdentifier.TypeEnum.SYSTEMGENERATED:
                                //    d.SystemGeneratedNbr = si.Value;
                                //    strSI += string.Format("{0} - {1}.{2}", "System Nbr", si.Value, Environment.NewLine + "<br />");
                                //    break;
                                case P44M.LtlShipmentIdentifier.TypeEnum.EXTERNAL:
                                    d.EXTERNALNbr = si.Value;
                                    strSI += string.Format("{0} - {1}.{2}", "External Nbr", si.Value, Environment.NewLine + "<br />");
                                    break;
                                default:
                                    strSI += string.Format("{0} - {1}.{2}", si.Type.ToString(), si.Value, Environment.NewLine + "<br />");
                                    break;
                            }
                        }
                    }
                    else
                    {
                        strSI += string.Format("{0}.{1}", "No shipment identifiers were returned from provider", Environment.NewLine + "<br />");
                        
                    }
                    if (string.IsNullOrWhiteSpace(d.PickupNumber))
                    {
                        if( P44WebServiceUrl.Contains("test"))
                        {
                            d.PickupNumber = "test conf# not provided by carrier";
                        } else
                        {
                            d.PickupNumber = "conf# not provided by carrier";
                        }
                    }
                    d.sShipIDs = strSI;

                    var sMsg = "Info Messages:" + Environment.NewLine + "<br />";
                    List<NGL.FM.P44.APIMessage> lAPIMsgs = new List<NGL.FM.P44.APIMessage>();
                    //Modified by RHR for v-8.2.0.116 on 7/16/19
                    if (result.InfoMessages != null && result.InfoMessages.Count() > 0)
                    {
                        foreach (P44M.Message e in result.InfoMessages)
                        {
                            NGL.FM.P44.APIMessage oAPIMsg = new NGL.FM.P44.APIMessage
                            {
                                Severity = e.Severity.ToString(),
                                Message = e._Message,
                                Diagnostic = e.Diagnostic,
                                Source = e.Source.ToString(),
                            };
                            lAPIMsgs.Add(oAPIMsg);
                            sMsg += string.Format("{4}{0} - {1}. {2} - {3}.{4}{4}", e.Severity, e._Message, e.Diagnostic, e.Source, Environment.NewLine + "<br />");
                        }
                    }
                    else
                    {
                        sMsg += string.Format("{0}.{1}", "No information messages were returned from provider", Environment.NewLine + "<br />");

                    }
                    d.InfoMessages = lAPIMsgs.ToArray();

                    d.RespCapacityProviderBolUrl = result.CapacityProviderBolUrl;
                    // 3.0 only d.RespPackingVisualizationUrl = result.PackingVisualizationUrl
                    d.RespPickupNote = result.PickupNote;
                    d.RespPickupDateTime = result.PickupDateTime;
                    
                }
                else
                {
                    response.message = "Dispatch Failed. No response from carrier.";
                    response.Diagnostic = "Failed";
                    response.Severity = SeverityEnum.ERROR;
                    response.Source = SourceEnum.SYSTEM;
                    d.ErrorMessage = "Dispatch Failed. No response from carrier.";
                    return response;
                 
                }

            }
            catch (ApiException ex)
            {
                NGL.FM.P44.ApiError ae = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<NGL.FM.P44.ApiError>(ex.ErrorContent);
                var sMsg = "";
                if (ae.Errors?.Count > 0)
                {
                    foreach (NGL.FM.P44.Message e in ae.Errors)
                    {

                        sMsg += string.Format("{4}Severity: {0}{4}Message: {1}{4}Diagnostic: {2}{4}Source: {3}{4}", e.Severity, e.message, e.Diagnostic, e.Source, Environment.NewLine + "<br />");
                    }
                }
                response.message = string.Format("ErrorMessage: {2}.{0} {3}{0} Support Ref ID: {4}{0}", Environment.NewLine + "<br />", ae.HttpMessage, ae.ErrorMessage, sMsg, ae.SupportReferenceId);
                response.Diagnostic = "Failed";
                response.Severity = SeverityEnum.ERROR;
                response.Source = SourceEnum.SYSTEM;
                return response;              
            }
            catch (Exception ex)
            {
               
                var sMsg = 
                
                response.message = string.Format("{4}Severity: {0}{4}Warning: {2}{4}Message: {1}{4}Source: {3}{4}", "System Error", ex.Message, "Your load may have been recieved by the carrier but the dispatch confirmation could not be processed.  You should validate the information with the carrier and manually accept this load as a spot rate.", "NGL API Dispatching BLL", Environment.NewLine + "<br />" );
                response.Diagnostic = "Failed";
                response.Severity = SeverityEnum.ERROR;
                response.Source = SourceEnum.SYSTEM;
                return response;
                throw;
            }

            // return the HTTP Response.
            return response;
        }


        #region " Private Methods"

        ////private string formatDispatchTimeStringForAPI(string sTime, string sDefault)
        ////{
        ////    string sRet = sDefault;
        ////    DateTime dtParsed = DateTime.Now;
        ////    string sToParse = string.Format("{0} {1}", "2018-01-01", sTime);
        ////    if (DateTime.TryParse(sToParse, out dtParsed))
        ////    {
        ////        sRet = dtParsed.ToString("HH:mm");
        ////    }

        ////    return sRet;
        ////}

        private P44M.LtlDispatchedShipment.LengthUnitEnum getLengthUnitEnum(string s)
        {
            var strLengthUnitEnum = s.Trim().ToUpper();

            //Set default
            P44M.LtlDispatchedShipment.LengthUnitEnum retVal = P44M.LtlDispatchedShipment.LengthUnitEnum.IN;


            if (string.IsNullOrWhiteSpace(strLengthUnitEnum)) { return retVal; }

            if (strLengthUnitEnum == "CM") { retVal = P44M.LtlDispatchedShipment.LengthUnitEnum.CM; }

            return retVal;
        }

        private P44M.LtlDispatchedShipment.WeightUnitEnum getWeightUnitEnum(string s)
        {
            var strWeightUnitEnum = s.Trim().ToUpper();

            //Set return value to US by default
            P44M.LtlDispatchedShipment.WeightUnitEnum retVal = P44M.LtlDispatchedShipment.WeightUnitEnum.LB;

            //If country is null return US by default
            if (string.IsNullOrWhiteSpace(strWeightUnitEnum)) { return retVal; }

            if (strWeightUnitEnum == "KG") { retVal = P44M.LtlDispatchedShipment.WeightUnitEnum.KG; }

            return retVal;
        }

        private P44M.LtlDispatchedShipment.PaymentTermsOverrideEnum getPaymentTermsOverrideEnum(string s)
        {
            //Set return value to PREPAID by default
            P44M.LtlDispatchedShipment.PaymentTermsOverrideEnum retVal = P44M.LtlDispatchedShipment.PaymentTermsOverrideEnum.PREPAID;

            var strPayment = s.Trim().ToUpper();

            switch (strPayment)
            {
                case "PREPAID":
                    retVal = P44M.LtlDispatchedShipment.PaymentTermsOverrideEnum.PREPAID;
                    break;
                case "COLLECT":
                    retVal = P44M.LtlDispatchedShipment.PaymentTermsOverrideEnum.COLLECT;
                    break;
                case "THIRDPARTY":
                    retVal = P44M.LtlDispatchedShipment.PaymentTermsOverrideEnum.THIRDPARTY;
                    break;
            }

            return retVal;
        }

        private P44M.LtlDispatchedShipment.DirectionOverrideEnum getDirectionOverrideEnum(string s)
        {
            //Set return value to THIRDPARTY by default
            P44M.LtlDispatchedShipment.DirectionOverrideEnum retVal = P44M.LtlDispatchedShipment.DirectionOverrideEnum.THIRDPARTY;

            var strDirection = s.Trim().ToUpper();

            switch (strDirection)
            {
                case "THIRDPARTY":
                    retVal = P44M.LtlDispatchedShipment.DirectionOverrideEnum.THIRDPARTY;
                    break;
                case "SHIPPER":
                    retVal = P44M.LtlDispatchedShipment.DirectionOverrideEnum.SHIPPER;
                    break;
                case "CONSIGNEE":
                    retVal = P44M.LtlDispatchedShipment.DirectionOverrideEnum.CONSIGNEE;
                    break;
            }

            return retVal;
        }

        #endregion


    }
}
