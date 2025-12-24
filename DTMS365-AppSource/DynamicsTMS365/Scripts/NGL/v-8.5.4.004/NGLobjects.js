//***********************************************************************************
//** IQSS - Please add all new code to the top of this file only **
//***********************************************************************************

//const { debug } = require("console");


// Modified by RHR for v-8.5.3.007 on 03/14/2023 added Class for NGLToolTip data
function NGLRespToolTipCtrl() {
    NGLToolTips = null;
    function NGLToolTip() {
        id: "0";
        containerid: "0";
        desc: "";
    }

    this.addEvents = function () {
        var tObj = this;
        if (typeof (tObj.NGLToolTips) !== 'undefined' && ngl.isObject(tObj.NGLToolTips) === true && ngl.isArray(tObj.NGLToolTips) === true) {
            tObj.NGLToolTips.forEach(function (arrayItem) {
                var id = arrayItem.id;
                var desc = arrayItem.desc;
                var IWin = arrayItem.containerid; // like $("#blueborder" + tObj.IDKey + "Edit");
                $("#" + id.toString()).on("mouseover", function (event) { ngl.showNGLTooltipOnWindow(event, desc, IWin); });
                $("#" + id.toString()).on("mouseout", ngl.hideNGLTooltip);
            });
        }
    }

    this.addToolTip = function (id, containerid, desc) {
        var oToolTip = new NGLToolTip()
        var tObj = this;
        oToolTip.id = id;
        oToolTip.desc = desc;
        oToolTip.containerid = containerid;

        if (typeof (tObj.NGLToolTips) !== 'undefined' && ngl.isObject(tObj.NGLToolTips) === true && ngl.isArray(tObj.NGLToolTips) === true) {
            tObj.NGLToolTips.push(oToolTip);
        } else {
            tObj.NGLToolTips = new Array();
            tObj.NGLToolTips.push(oToolTip);
        }
    }

}

//function NGLToolTip() {
//    id: "0";
//    containerid: "0";
//    desc: "";

//    this.addEvents = function (oNGLToolTips) {
//        if (oNGLToolTips) {
//            oNGLToolTips.forEach(function (arrayItem) {
//                var id = arrayItem.id;
//                var desc = arrayItem.desc;
//                var IWin = arrayItem.containerid; // like $("#blueborder" + tObj.IDKey + "Edit");
//                $("#" + id.toString()).on("mouseover", function (event) { ngl.showNGLTooltipOnWindow(event, desc, IWin); });
//                $("#" + id.toString()).on("mouseout", ngl.hideNGLTooltip);
//            });
//        }

//    }
//}


//Object declaration for Document Type
function NGLDocType() {
    EDITControl: 0;
    EDITName: "";
    EDITDescription: "";

}

//Object declaration for Element
function NGLElement() {
    ElementControl: 0;
    ElementName: "";
    ElementDescription: "";
    ElementMinLength: 0;
    ElementMaxLength: 0;
    ElementEDIDataTypeControl: 0;
    ElementValidationTypeControl: 0;
    ElementFormattingFnControl: 0;

}

function AMSAppointments() {
    ApptControl: 0;
    ApptName: "";
}
function AMSOrders() {
    BookControl: 0;
    BookName: "";
}

//Added By LVV on 8/7/18 for v-8.3 TMS365 Scheduler
//Removed by RHR for v-8.2 on 09/15/2018
//we now use PageSetting Model Object
function UserPageSetting() {
    UserPSControl: 0;
    UserPSUserSecurityControl: 0; //On Save populate this in the Controller method with Parameters.UserControl
    UserPSPageControl: 0;         //On Save this gets populated with the Javascript variable PageControl in the .aspx file
    UserPSName: "";               //This must be unique to the page						
    UserPSMetaData: "";           //This is where you store the data for the setting. Just store the entire JSON string here
    UserPSModel: "";              //Do not use this field
    UserPSAPIReference: "";       //Do not use this field	
    UserPSAPIFilterID: "";        //Do not use this field		
    UserPSAPISortKey: "";         //Do not use this field			
}
//Created by RHR for v-8.2 on 09/15/2018 
//shared model available to all pages
//the page controller will assign user and page details
//the name is the page setting key 
function PageSettingModel() {
    name: "";
    value: "";
}

function AMSCarrierAvailableSlots() {
    ApptControl: 0;
    Date: "";
    StartTime: "";
    EndTime: "";
    Docks: "";
    Warehouse: "";
    Books: "";
    CarrierNumber: 0;
    CarrierName: "";
    CompControl: 0;
    CarrierControl: 0;
}

function NSAcceptBid() {
    LTBookControl: 0;
    BidControl: 0;
    BidLoadTenderControl: 0;
    BidBidTypeControl: 0;
    BidCarrierControl: 0;
    BidLineHaul: 0;
    BidFuelTotal: 0;
    BidFuelVariable: 0;
    BidFuelUOM: "";
    BidTotalCost: 0;
    BidBookCarrTarEquipMatControl: 0;
    BidBookCarrTarEquipControl: 0;
    BidBookModeTypeControl: 0;
}

function LECarrierAccessorial() {
    LECAControl: 0;
    LECALECarControl: 0;
    LECAAccessorialCode: 0;
    LECACaption: "";
    LECAEDICode: "";
    LECAAutoApprove: 0;
    LECAAllowCarrierUpdates: 0;
    LECAAccessorialVisible: 1;
    LECADynamicAverageValue: 0;
    LECAAverageValue: 0;
    LECAApproveToleranceLow: 0;
    LECAApproveToleranceHigh: 0;
    LECAApproveTolerancePerLow: 0;
    LECAApproveTolerancePerHigh: 0;
    LECAModDate: "";
    LECAModUser: "";
    LECAUpdated: "";
}

function MassUpdateSingleSignOn() {
    NEXTStop: false;
    P44: false;
    SSOAXUN: "";
    SSOAXPass: "";
    NewPass: "";
    SSOAXRefID: "";
    CopyFromSSOAXCtrl: 0;
    UserControls: [];
}

function OrderPreviewFilters() {
    NatAcctChecked: false;
    CompChecked: false;
    NatAcctDDLValue: "";
    CompDDLValue: "";
    FrtTypDDLValue: 0;
}

function vUserSecurityCarrier() {
    USCControl: 0;
    USCUserSecurityControl: 0;
    USCCarrierControl: 0;
    USCCarrierNumber: 0;
    CarrierName: "";
    CarrierSCAC: "";
    USCCarrierContControl: 0;
    CarrierContName: "";
    CarrierContactEMail: "";
    CarrierContactPhone: "";
    CarrierContPhoneExt: "";
    USCCarrierAccounting: "";
    USCModDate: "";
    USCModUser: "";
    USCUpdated: "";
}

function AMSCarrierBAWrapper() {
    AMSCarrierAvailableSlots: null;
    blnIsPickup: false;
}

function SystemInfo() {
    CurrentClientSoftwareRelease: "";
    LastClientModified: "";
    AuthNumber: "";
    //Server Software Version
    CurrentServerSoftwareRelease: "";
    LastServerSoftwareModified: "";
    Database: "";
    Server: "";
    //Extra Fields (for future maybe?)
    AuthKey: "";
    AuthName: "";
    AuthAddress: "";
    AuthCity: "";
    AuthState: "";
    AuthZip: "";
    XactVersion: "";
    MasVersion: "";
    ClaimVersion: "";
}

function AuditFilters() {
    CarrierDDLValue: "";
    APAuditFltrsDDLValue: 0;
    APReceivedDateFrom: "";
    APReceivedDateTo: "";
}

//Added By LVV on 9/19/19 for Bing Maps
function tblStop() {
    StopControl: 0;
    StopName: "";
    StopAddress1: "";
    StopCity: "";
    StopState: "";
    StopCountry: "";
    StopZip: "";
    StopLatitude: 0;
    StopLongitude: 0;
    StopModDate: "";
    StopModUser: "";
    StopUpdated: "";
}

//Added by LVV on 5/27/20 for v-8.2.1.008 - Task #202005191648 Fix Procedure Level Security Deployment Issues
function SelectContactFilters() {
    Control: 0;
    ContactType: 0;
}

//Added by LVV on 5/27/20 for v-8.2.1.008 - Task #202005191648 Fix Procedure Level Security Deployment Issues
function SelectableGridSave() {
    Control: 0;
    BitPositionsOn: [];
}

//Added by LVV on 5/29/20 for v-8.2.1.008 - Task #202005191648 Fix Procedure Level Security Deployment Issues
function MultiSelectBatchObjects() {
    Controls: [];
    SelectedBits: [];
    ConfigType: 0;
    Action: 0;
}

//Added By LVV On 8/19/20 For v-8.3.0.001 - Task#20200817144456 - Modify the License File Integration Command Line Utility
function SelectableGridItem() {
    SGItemBitPos: 0;
    SGItemCaption: "";
    SGItemOn: false;
}

//***********************************************************************************
//** Do not modify or add any code below this comment**
//***********************************************************************************


// Begin Simple Array Data Sources
//var nglAPIFrtClasses = [{ "FreightClass": "50" }, { "FreightClass": "55" }, { "FreightClass": "60" }, { "FreightClass": "65" }, { "FreightClass": "70" }, { "FreightClass": "77.5" }, { "FreightClass": "85" }, { "FreightClass": "92.5" }, { "FreightClass": "100" }, { "FreightClass": "110" }, { "FreightClass": "125" }, { "FreightClass": "150" }, { "FreightClass": "175" }, { "FreightClass": "200" }, { "FreightClass": "250" }, { "FreightClass": "300" }, { "FreightClass": "400" }, { "FreightClass": "500" }];
var nglAPIFrtClasses = [
    { FreightClass: "50" },
    { FreightClass: "55" },
    { FreightClass: "60" },
    { FreightClass: "65" },
    { FreightClass: "70" },
    { FreightClass: "77.5" },
    { FreightClass: "85" },
    { FreightClass: "92.5" },
    { FreightClass: "100" },
    { FreightClass: "110" },
    { FreightClass: "125" },
    { FreightClass: "150" },
    { FreightClass: "175" },
    { FreightClass: "200" },
    { FreightClass: "250" },
    { FreightClass: "300" },
    { FreightClass: "400" },
    { FreightClass: "500" }
];
var nglAPIWeightUnit = [{ "WeightUnit": "LB" }, { "WeightUnit": "KG" }];
//var nglAPILengthUnit = [{ "LengthUnit": "IN" }, { "LengthUnit": "CM" }, { "LengthUnit": "FT" }, { "LengthUnit": "M" }];
//in v-2 or P44 API only IN and CM are supported. 
var nglAPILengthUnit = [{ "LengthUnit": "IN" }, { "LengthUnit": "CM" }];

// End Simple Array Data Sources

// nglvlookupEditors are used to populate grid editable drop down lists.
var nglvLookupEditors = {
    // Freight Class list
    dsFrtClass: new kendo.data.DataSource({ data: nglAPIFrtClasses }),
    FrtClassDropDownEditor: function (container, options) {
        //$('<input name="' + options.field + ' data-bind="value:' + options.field + '" />')
        //$('<input required data-text-field="FreightClass" data-value-field="FreightClass" data-bind="value:' + options.field + ' name="' + options.field + '"/>')
        $('<input data-bind="value:' + options.field + '" />')
            .appendTo(container)
            .kendoDropDownList({
                autoBind: true,
                dataTextField: "FreightClass",
                dataValueField: "FreightClass",
                autoWidth: true,
                dataSource: nglvLookupEditors.dsFrtClass
            });
    },
    // Weight Unit list
    dsWgtUnit: new kendo.data.DataSource({ data: nglAPIWeightUnit }),
    WgtUnitDropDownEditor: function (container, options) {
        $('<input required data-text-field="WeightUnit" data-value-field="WeightUnit" data-bind="value:' + options.field + '"/>')
            .appendTo(container)
            .kendoComboBox({
                autoBind: false,
                dataSource: nglvLookupEditors.dsWgtUnit
            });
    },
    // Length Unit list
    dsLengthUnit: new kendo.data.DataSource({ data: nglAPILengthUnit }),
    LengthUnitDropDownEditor: function (container, options) {
        $('<input required data-text-field="LengthUnit" data-value-field="LengthUnit" data-bind="value:' + options.field + '"/>')
            .appendTo(container)
            .kendoComboBox({
                autoBind: false,
                dataSource: nglvLookupEditors.dsLengthUnit
            });
    },
    // Package Type Description Text only List
    dsPkgTypeByDesc: new kendo.data.DataSource({
        transport: {
            read: function (options) {
                $.ajax({
                    async: false,
                    type: "GET",
                    url: "api/vLookupList/GetStaticList/" + nglStaticLists.vNGLAPIPalletTypes,
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                    success: function (data) { options.success(data); },
                    error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Get Pkg Type Failure"); ngl.showErrMsg("Read Package Type Error", sMsg, null); }
                });
            }
        },
        schema: {
            data: "Data",
            total: "Count",
            model: {
                id: "Description",
                fields: {
                    Description: { editable: false }
                }
            },
            errors: "Errors"
        },
        error: function (e) { ngl.showErrMsg("Read Package Type Error", e.errors, null); this.cancelChanges(); },
        serverPaging: false,
        serverSorting: false,
        serverFiltering: false
    }),
    PkgTypeDescDropDownEditor: function (container, options) {
        //$('<input required data-text-field="Description" data-value-field="Description" data-bind="value:' + options.field + '"/>')
        $('<input data-bind="value:' + options.field + '" />')
            .appendTo(container)
            .kendoDropDownList({
                autoBind: false,
                dataTextField: "Description",
                dataValueField: "Description",
                autoWidth: true,
                dataSource: nglvLookupEditors.dsPkgTypeByDesc
            });
    },
    //Load Tender Trans Type List
    dsLoadTenderTransType: new kendo.data.DataSource({
        transport: {
            read: function (options) {
                $.ajax({
                    async: false,
                    type: "GET",
                    url: "api/vLookupList/GetStaticList/" + nglStaticLists.tblLoadTenderTransType,
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                    success: function (data) { options.success(data); },
                    error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Get Load Tender Trans Type Failure"); ngl.showErrMsg("Read Package Type Error", sMsg, null); }
                });
            }
        },
        schema: {
            data: "Data",
            total: "Count",
            model: {
                id: "Control",
                fields: {
                    Name: { editable: false },
                    Description: { editable: false }
                }
            },
            errors: "Errors"
        },
        error: function (e) { ngl.showErrMsg("Read Package Type Error", e.errors, null); this.cancelChanges(); },
        serverPaging: false,
        serverSorting: false,
        serverFiltering: false
    }),
    // Package Type Description Text only List
    dsPkgDesc: new kendo.data.DataSource({
        transport: {
            read: function (options) {
                $.ajax({
                    async: false,
                    type: "GET",
                    url: "api/vLookupList/GetUserDynamicList/" + nglUserDynamicLists.PackageDescription,
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                    success: function (data) { options.success(data); },
                    error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Get Package Description Failure"); ngl.showErrMsg("Read Package Description Error", sMsg, null); }
                });
            }
        },
        schema: {
            data: "Data",
            total: "Count",
            model: {
                id: "Control",
                fields: {
                    Control: { editable: false },
                    Name: { editable: false },
                    Description: { editable: false }
                }
            },
            errors: "Errors"
        },
        error: function (e) { ngl.showErrMsg("Read Package Description Error", e.errors, null); this.cancelChanges(); },
        serverPaging: false,
        serverSorting: false,
        serverFiltering: false
    }),
    PkgDescDropDownEditor: function (container, options) {
        //$('<input required data-text-field="Description" data-value-field="Description" data-bind="value:' + options.field + '"/>')
        $('<input data-bind="value:' + options.field + '" />')
            .appendTo(container)
            .kendoDropDownList({
                autoBind: false,
                dataTextField: "Name",
                dataValueField: "Control",
                autoWidth: true,
                dataSource: nglvLookupEditors.dsPkgDesc
            });
    },
}

function validationResults() {
    Success: false;
    Message: "";
}

function OrderLocation(ScheduledDate, Name, Street, City, State, Zip, Country) {
    this.scheduledDate = ScheduledDate;
    this.name = Name;
    this.street = Street;
    this.city = City;
    this.state = State;
    this.zip = Zip;
    this.country = Country;
}

function rateReqOrder() {
    ID: 0; //maps to RRControl
    ShipKey: ""; //maps to SHID default equals BookConsPrefix Rate Requests have their own date based unique number key using Date.now() or the NGL core.js function getUTCTimeStamp() 
    BookConsPrefix: ""; //maps to book table if available Rate Requests have their own date based unique number key
    ShipDate: "";
    DeliveryDate: "";
    BookCustCompControl: 0;
    CompName: "";
    CompNumber: 0;
    CompAlphaCode: "";
    BookCarrierControl: 0;
    CarrierName: "";
    CarrierNumber: 0;
    CarrierAlphaCode: "";
    TotalCases: 0;
    TotalWgt: 0;
    TotalPL: 0;
    TotalCube: 0;
    TotalStops: 0;
    Pickup: null;
    Stops: [];
    Accessorials: [];  //LVV ADD
    WeightUnit: "";
    LengthUnit: "";
    CommCodeType: ""; //Added By LVV on 4/20/20 LBDemo
    Inbound: false; //Added By LVV on 4/20/20 LBDemo
    BookTransType: 0; //Added By LVV on 4/24/20 LBDemo
}

function rateReqStop() {
    ID: 0; //maps to RRSControl
    ParentID: 0; //maps to RRSRRControl 
    BookControl: 0; //maps to BookControl if available
    BookProNumber: "";
    StopIndex: 0;
    BookCarrOrderNumber: '';
    CompControl: 0;
    CompName: "";
    CompAddress1: "";
    CompAddress2: "";
    CompAddress3: "";
    CompCity: "";
    CompState: "";
    CompCountry: "";
    CompPostalCode: "";
    IsPickup: false;
    StopNumber: 0;
    TotalCases: 0;
    TotalWgt: 0;
    TotalPL: 0;
    TotalCube: 0;
    LoadDate: "";
    RequiredDate: "";
    Items: [];
    WeightUnit: "";
    LengthUnit: "";
    SHID: "";
}

function rateReqItem() {
    ID: 0; //maps to RRIControl
    ParentID: 0; //maps to RRIRRSControl 
    ItemIndex: 0;
    ItemStopIndex: 0;
    LoadID: 0;
    ItemNumber: "";
    Weight: 0;
    WeightUnit: "lbs";
    FreightClass: "70";
    PalletCount: 1;
    NumPieces: 1;
    Description: "";
    Quantity: "1";
    HazmatId: "";
    Code: "";
    HazmatClass: "";
    IsHazmat: false;
    Pieces: "";
    PackageType: "";
    Length: 0;
    Width: 0;
    Height: 0;
    Density: "";
    NMFCItem: "";
    NMFCSub: "";
    Stackable = false;
}

function OrderFilter() {
    BookControl: 0;
    BookCustCompControl: 0;
    CompNameNumberCode: "";
    BookCarrierControl: 0;
    CarrierNameNumberCode: "";
    LoadDateFrom: "";
    LoadDateTo: "";
    ReqDateFrom: "";
    ReqDateTo: "";
    BookCarrOrderNumber: "";
    BookProNumber: "";
    BookSHID: "";
    BookConsPrefix: "";
    BookOrigName: "";
    BookOrigAddress1: "";
    BookOrigCity: "";
    BookOrigState: "";
    BookOrigCountry: "";
    BookDestName: "";
    BookDestAddress1: "";
    BookDestCity: "";
    BookDestState: "";
    BookDestCountry: "";
    BookTotalCases: 0;
    BookTotalWgt: 0;
    BookTotalPL: 0;
    BookTotalCube: 0;
    NewOrders: true;
    AssignedOrders: true;
    TenderedOrders: false;
    AcceptedOrders: false;
    ShippedOrders: false;
    DeliveredOrders: false;
    LTLOnly: false;
    CNSOnly: false;
}

function RateAdjustment(ID, Class, Wgt, Desc, Code, Amt, Rate) {
    this.index = ID;
    this.freightClass = Class;
    this.weight = Wgt;
    this.description = Desc;
    this.descriptionCode = Code;
    this.amount = Amt;
    this.rate = Rate;
}

function RateErrors(ID, Code, Msg, eMsg, fieldName) {
    this.index = ID;
    this.errorCode = Code;
    this.errorMessage = Msg;
    this.eMessage = eMsg;
    this.errorfieldName = fieldName;
}

function allGridItem() {
    Control: 0;
    ProNumber: "";
    CnsNumber: "";
    StopNumber: 0;
    PurchaseOrderNumber: "";
    OrderNumber: "";
    ScheduledToLoad: "";
    RequestedToArrive: "";
    AssignedCarrier: "";
    DestinationName: "";
    DestinationCity: "";
    DestinationState: "";
    Comments: "";
    Status: 0;
    BookNotes1: "";
    BookNotes2: "";
    BookNotes3: "";
    AssignedProNumber: "";
    BookShipCarrierProNumberRaw: "";
    BookShipCarrierProControl: 0;
    AssignedCarrierNumber: "";
    AssignedCarrierName: "";
    AssignedCarrierContact: "";
    AssignedCarrierContactPhone: "";
    BookPickupStopNumber: 0;
    ApplyToAllDestinations: false;
    ApplyToAllPickups: false;
    ApplyCommentsToCNS: false;
    BookAMSPickupApptControl: 0;
    BookAMSDeliveryApptControl: 0;
    BookLoadControl: 0;
    SHID: "";
    CarrierPro: "";
    BookModDate: "";
    BookModUser: "";
    //BookControl: 0;
    //BookCarrFBNumber: ""; 
    //BookCarrOrderNumber: ""; 
    //BookCarrBLNumber: ""; 
    //BookCarrBookDate: ""; 
    //BookCarrBookTime: ""; 
    //BookCarrBookContact: "";
    BookCarrScheduleDate: "";
    BookCarrScheduleTime: "";
    BookCarrActualDate: "";
    BookCarrActualTime: "";
    BookCarrActLoadComplete_Date: "";
    BookCarrActLoadCompleteTime: "";
    BookCarrDockPUAssigment: "";
    //BookCarrPODate: ""; 
    //BookCarrPOTime: ""; 
    BookCarrApptDate: "";
    BookCarrApptTime: "";
    BookCarrActDate: "";
    BookCarrActTime: "";
    BookCarrActUnloadCompDate: "";
    BookCarrActUnloadCompTime: "";
    BookCarrDockDelAssignment: "";
    //BookCarrVarDay: 0;
    //BookCarrVarHrs: 0; 
    BookCarrTrailerNo: "";
    BookCarrSealNo: "";
    BookCarrDriverNo: "";
    BookCarrDriverName: "";
    //BookCarrRouteNo: ""; 
    //BookCarrTripNo: ""; 
    BookWhseAuthorizationNo: "";
    BookCarrStartLoadingDate: "";
    BookCarrStartLoadingTime: "";
    BookCarrFinishLoadingDate: "";
    BookCarrFinishLoadingTime: "";
    BookCarrStartUnloadingDate: "";
    BookCarrStartUnloadingTime: "";
    BookCarrFinishUnloadingDate: "";
    BookCarrFinishUnloadingTime: "";
    BookFinAPActWgt: 0;
    BookCarrBLNumber: "";
    Pages: 0;
    RecordCount: 0;
    Page: 0;
    PageSize: 0;
    ROW_NUMBER: 0;
    //Added By LVV on 9/19/19 for Bing Maps - CLT aka "Comment Location Tag" (optional location to be tagged as detail with a comment)
    CLTStopControl: 0;
    CLTStopName: "";
    CLTStopAddress1: "";
    CLTStopCity: "";
    CLTStopState: "";
    CLTStopCountry: "";
    CLTStopZip: "";
    CLTStopLatitude: 0;
    CLTStopLongitude: 0;
}


function SSOAccount() {
    SSOAControl: 0; //zero for new users; system will look up control with SSOAName on new users
    SSOAName: ""; //use Web config SSOADefaultName for new users
    SSOADesc: ""; //not required
    AllowNonPrimaryComputers: false; //on new users this is True until the primary computer is configured.  On Trial Users only one computer is allowed per user
    AllowPublicComputer: false; //on new users we do not allow Public Computers
    SSOAClientID: ""; //use Web config idaClientId for new users; empty when using NGL Authentication
    SSOALoginURL: ""; //use Web config idaInstance for new users; empty when using NGL Authentication
    SSOADataURL: ""; //not required
    SSOARedirectURL: ""; //use Web config WebBaseURI for new users; empty when using NGL Authentication. Provide a default value but typically generated dynamically at run time
    SSOAClientSecret: ""; //Not Required empty when using NGL Authentication
    SSOAAuthCode: ""; //maps to tenant use Web config idaTenant for new users;  default = common
    SSOAAuthenticationRequired: false;  //always true for new users. Generally false when using NGL Authentication
    SSOAUserSecurityControl: 0; //zero for new users. Maps to tblUserSecurity.UserSecurityControl
    SSOAUserName: "";  //for new users this is the HttpContext.Current.User.Identity
    SSOAUserEmail: ""; //empty for new users
    UserFriendlyName: ""; // empty for new users maps to tblUserSecurity.UserFriendlyName
    UserFirstName: ""; // empty for new users maps to tblUserSecurity.UserFirstName
    UserLastName: ""; // empty for new users maps to tblUserSecurity.UserLastName
    AuthenticationErrorCode: 0; // if Authentication Fails return an Error Code (typically bound to an enumerator)
    AuthenticationErrorMessage: "";  //Default Error Message returned in English if authentication fails.  Error Code may be used to lookup language specific message    
    //Added By LVV 7/31/17 for v-8.0 TMS365
    IsUserCarrier: false; //Indicates if the current user is a carrier or not
    UserCarrierControl: 0; //0 if the current user is not a carrier
    UserCarrierContControl: 0; //0 if the current user is not a carrier
    UserLEControl: 0; //The Legal Entity for the user
    UserTheme365: "";
    UserWorkPhone: "";
    UserWorkPhoneExt: "";
    CatControl: 0;
}

function SSOResults() {
    PrimaryComputer: false; //on new users we ask if this is the primary computer and if this is a private or public computer
    PublicComputer: false; //on new users we ask if this is a public/private computer and if this is a private or public computer
    SSOAControl: 0; //zero for new users; system will look up control with SSOAName on new users
    SSOAName: ""; //use Web config SSOADefaultName for new users
    SSOAClientID: ""; //use Web config idaClientId for new users; empty when using NGL Authentication
    SSOALoginURL: ""; //use Web config idaInstance for new users; empty when using NGL Authentication
    SSOARedirectURL: ""; //use Web config WebBaseURI for new users; empty when using NGL Authentication
    SSOAClientSecret: ""; //empty when using NGL Authentication
    SSOAAuthCode: ""; //use tenant use Web config idaTenant for new users;  default = common
    UserSecurityControl: 0; //maps to tblUserSecurity.UserSecurityControl if zero this is a new user and a new record needs to be created 
    UserName: "";  //maps to tblUserSecurity.UserName for new users this maps to HttpContext.Current.User.Identity if this is the primary computer and it is private not public 
    UserLastName: ""; //maps to tblUserSecurity.UserLastName from user.profile.family_name on new users
    UserFirstName: ""; //maps to tblUserSecurity.UserFirstName from user.profile.given_name on new users and also maps to and tblUserSecurity.UserFriendlyName on new users
    USATUserID: ""; //maps to tblUserSecurityAccessToken.USATUserID from user.profile.unique_name on new users if PrimaryComputer = false or PublicComputer = true maps to tblUserSecurity.UserName
    USATToken: ""; //maps to tblUserSecurityAccessToken.USATToken from JWT token retrieved via validateUser ADAL.getCachedToken
    SSOAUserEmail: ""; //maps to tblUserSecurity.UserEmail from user.profile.email on new users
    SSOAExpires: 0; //maps to user.profile.exp: number of seconds after January 1, 1970 (1970-01-01T0:0:0Z) UTC  when the token expires
    SSOAIssuedAtTime: 0; //maps to user.profile.iat: number of seconds after January 1, 1970 (1970-01-01T0:0:0Z) UTC  when the token was issued
    //Note: tblUserSecurityAccessToken.USATExpires maps to Expiration time formula: current datetime + (SSOAExpires - SSOAIssuedAtTime) - 5 minutes for processing 
    //Added By LVV 7/31/17 for v-8.0 TMS365
    IsUserCarrier: false; //Indicates if the current user is a carrier or not
    UserCarrierControl: 0; //0 if the current user is not a carrier
    UserCarrierContControl: 0; //0 if the current user is not a carrier
    UserLEControl: 0; //The Legal Entity for the user
    UserFriendlyName: ""; // empty for new users maps to tblUserSecurity.UserFriendlyName
    UserTheme365: "";
    UserWorkPhone: "";
    UserWorkPhoneExt: "";
    CatControl: 0;
    SSOAExpiresMilli: 0; //maps to user.profile.exp: number of milliseconds after January 1, 1970 (1970-01-01T0:0:0Z) UTC  when the token expires
}

function freeTrialComp() {
    UserSecurityControl: 0;
    CompControl: 0;
    CompLegalEntity: "";
    CompName: "";
    CompNumber: 0;
    ShipFromAddress1: "";
    ShipFromAddress2: "";
    ShipFromAddress3: "";
    ShipFromCity: "";
    ShipFromState: "";
    ShipFromZip: "";
    ShipFromCountry: "";
    CompAbrev: "";
    CompAlphaCode: "";
    CompContName: "";
    CompContTitle: "";
    CompCont800: "";
    CompContPhone: "";
    CompContPhoneExt: "";
    CompContFax: "";
    CompContEmail: "";
    ValidationMsg: "";
    WarningMsg: "";
}

function NGLClass14() {
    NGLvar1455: "";
    NGLvar1452: 0;
    NGLvar1451: "";
    NGLvar1454: "";
    NGLvar1450: "";
    NGLvar1458: "";
    NGLvar1474: "";
    NGLvar1472: 0;
    NGLvar1457: "";
}

function helpItem() {
    PHControlL1: 0;
    PHControlL2: 0;
    PHControlL3: 0;
    PHControlL4: 0;
    HelpWindowTitle: "";
    CompTitle: "";
    UserTitle: "";
    DefaultTitle: "";
    NotesL1: "";
    NotesL2: "";
    NotesL3: "";
    NotesL4: "";
    NotesLocalL1: "";
    NotesLocalL2: "";
    NotesLocalL3: "";
    NotesLocalL4: "";
    ALevel: 3;
    USec: 0;
    Page: 0;
}

function zip() {
    ZipCodeControl: 0;
    ZipCode: "";
    City: "";
    State: "";
}

function editorContent() {
    PageControl: 0;
    USec: 0;
    EditorName: "";
    Content: "";
    PageDetControl: 0;
}

function rateShopSaveLast() {
    OrigName: "";
    OrigAddress1: "";
    OrigCity: "";
    OrigState: "";
    OrigCountry: "";
    OrigPostalCode: "";
    OrigContact: null; //maps to Contact object

    DestName: "";
    DestAddress1: "";
    DestCity: "";
    DestState: "";
    DestCountry: "";
    DestPostalCode: "";
    DestContact: null; //maps to Contact object

    OrderNumber: "";
    TotalCases: 0;
    TotalWgt: 0;
    TotalPL: 0;
    TotalCube: 0;
    LoadDate: "";
    RequiredDate: "";
    Items: [];
    Accessorials: [];
    WeightUnit: "";
    LengthUnit: "";
    SHID: "";
    EmergencyContact: null;  //maps to Contact object
}

function EmailObject() {
    emailTo: "";
    emailFrom: "";
    emailCc: "";
    emailBcc: "";
    emailSubject: "";
    emailBody: "";
}

function AllFilter() {
    filterName: "";
    filterValue: "";
    filterFrom: "";
    filterTo: "";
    sortName: "";
    sortDirection: "";
    page: 0;
    pageSize: 0;
    skip: 0;
    take: 0;
    CarrierControlFrom: 0;
    CarrierControlTo: 0;
    CarrierNumberFrom: 0;
    CarrierNumberTo: 0;
    CompControlFrom: 0;
    CompControlTo: 0;
    CompNumberFrom: 0;
    CompNumberTo: 0;
    BookControl: 0;
    LaneControl: 0;
    LEAdminControl: 0;
    Data: "";
    ParentControl: 0;
    NatActNumber: 0;
    ContactControl: 0;
    FilterValues: [];   //array of FilterDetails
    SortValues: [];     //array of SortDetails
    Groups: []; // Added by RHR for v-8.3.0.001 on 08/12/2020  A list of field name used to group grids    
    PrimaryKey: 0; // Modified by RHR for v-8.5.1.002 on 04/09/2022

    this.filterValuesContains = function (sName) {
        var blnRet = false;
        if (typeof (this.FilterValues) === 'undefined' || ngl.isArray(this.FilterValues) === false) { return blnRet; };
        if (typeof (sName) === 'undefined' || sName === null) { return blnRet; };
        var idx, l = this.FilterValues.length;
        for (var j = 0; j < l; j++) {
            if (this.FilterValues[j].filterName === sName) { blnRet = true; break; }
        }
        return blnRet;
    };

}

function FilterDetails() {
    filterID: 0;            //ID used for record counter
    filterCaption: "";      //Dispaly caption
    filterName: "";         //name of field
    filterValueFrom: "";    //value from
    filterValueTo: "";      //value to
    filterFrom: "";         //date from
    filterTo: "";           //date to
    filterIsDate: false;

    this.containsFilterName = function (sName) {
        if (typeof (this.FilterData) === 'undefined' || ngl.isArray(this.FilterData) === false) { return false; };
        if (typeof (sName) === 'undefined' || sName === null) { return false; };
        var idx,
            l = this.FilterData.length;
        for (var j = 0; j < l; j++) {
            if (this.FilterData[j].filterName == sName) {
                return this.FilterData[j].filterIsDate;
                break;
            }
        }
        return false;
    };
}

//Modified by RHR for v-8.2 on 10/18/2018  
//added fieldParentTagID and fieldSequenceNo properties
function DataFieldDetails() {
    fieldID: 0;                 //ID used for record counter cmPageDetail.PageDetControl
    fieldTagID: "";             //DOM ID PageDetTagIDReference
    fieldCaption: "";           //Dispaly caption cmPageDetail.(localize PageDetCaptionLocal)
    fieldName: "";              //name of field cmPageDetail.PageDetName
    fieldDefaultValue: "";      //default value for insert cmPageDetail.PageDetMetaData
    fieldGroupSubType: 0;       //value to identify the data type associated with the nglGroupSubTypeEnum
    fieldReadOnly: true;        //do we allow updates PageDetReadOnly
    fieldFormat: "";            //optional input mask for kendo control cmElementField.ElmtFieldFormat or new cmPageDetail.PageDetFieldFormatOverride
    fieldTemplate: "";          //optional template layout for kendo control cmElementField.ElmtFieldTemplate or new cmPageDetail.PageDetFieldTemplateOverride
    fieldAllowNull: true;       //allow blank or null values  cmElementField.ElmtFieldAllowNull
    fieldVisible: true;         //flag to show or hide field  PageDetVisible
    fieldRequired: true;        //flag to require update PageDetRequired
    fieldMaxlength: "50";       //maximum size of text fields as string  cmElementField.ElmtFieldMaxLength
    fieldInsertOnly: false;     // allow updates on insert only PageDetInsertOnly 
    fieldAPIReference: "";      // reference to vlookuplist api 
    fieldAPIFilterID: "";       // reference to key enum for vlookuplist
    fieldParentTagID: "";        // reference to pageDetailParentID
    fieldSequenceNo: 0;         // reference to pageDetailSequenceNo
    fieldCRUDTagID: "";         // Modified by RHR for v-8.2 tag id for the widget used on CRUD operations if different than fieldTagID, and example is when a fast tab contains a grid, the grid's tag id will be used to read the data; typically used on nested grids in popup windows
    fieldWidgetAction: "";      // Modified by RHR for v-8.2 action to be executed by the widget's executeAction method 
    fieldWidgetActionKey: "";	// Modified by RHR for v-8.2  optional key used by the widget's executeAction method to determine when to trigger an action
    fieldCssClass: "";	        // Modified by RHR for v-8.2
    fieldStyle: "";	            // Modified by RHR for v-8.2
    fieldLockVisibility: "";    //lock the ability to make an option visible
    fieldNGLToolTip: "";        //Modified by RHR for v-8.5.3.007 on 03/14/2023 map Description to tool tip
}

//Created by RHR for v-8.2 on 10/18/2018
function WorkFlowSetting() {
    fieldID: 0;                 //ID used for record counter cmPageDetail.PageDetControl
    fieldName: "";              //name of field cmPageDetail.PageDetName
    fieldDefaultValue: "";      //default value for insert cmPageDetail.PageDetMetaData
    fieldVisible: "";           //stores the users visible selection option.
    fieldReadOnly: "";          //stores the users readonly option enabled true or false
    fieldLockVisibility: "";    //lock the ability to make an option visible
}

function SortDetails() {
    sortName: "";           // name of field to sort on
    sortDirection: "";      //direction for sorting like ASC or DESC
}

function NGLAPIAccessorial() {
    FeeNumber: 0;
    Control: 0;
    Code: "";
    Name: "";
    Desc: "";
    Value: 0;
}

//Modified by RHR for v-8.2 on 12/22/2018 added new fields needed for dispatching
//Modified by RHR for v-8.2 on 8/8/2019 added new fields needed for dispatching
//  CarrierName,ItemOrderNumbers,OtherCost
function Dispatch() {
    LoadTenderControl: 0;  //maps to the Load Tender associated with the Selected Quote
    Origin: null;
    Destination: null;
    Items: [];
    HazControl: 0;
    PickupDate: "";
    PickupStartTime: "";
    PickupEndTime: "";
    DeliveryDate: "";
    DeliveryStartTime: "";
    DeliveryEndTime: "";
    ProviderSCAC: "";
    VendorSCAC: "";
    BillOfLading: "";
    PONumber: "";
    SHID: "";
    OrderNunmber: "";
    PickupNumber: "";
    CarrierProNumber: "";
    SystemGeneratedNbr: "";
    EXTERNALNbr: "";
    Accessorials: [];
    PickupNote: "";
    DeliveryNote: "";
    ConfidentialNote: "";
    QuoteNumber: "";
    TotalWgt: 0;
    TotalQty: "";
    TotalPlts: 0;
    TotalCube: 0;
    WeightUnit: "";
    LengthUnit: ""; //LengthUnit
    Requestor: null; //set to origin.  may be used for alternate billing in future?
    EmergencyContact: null;  //maps to Contact object
    PaymentTermsOverride: false; //Boolean 
    PaymentTermsOverrideControl: 0; //Maps to API Code for "PREPAID" "COLLECT" "THIRD_PARTY" when PaymentTermsOverride is true
    DirectionOverride: false; //Boolean
    DirectionOverrideControl: false; //Maps to API Code for "SHIPPER" "CONSIGNEE" "THIRD_PARTY"  when DirectionOverride is true
    LoadTenderTransTypeControl: 1; //Outbound = 1, Transfer = 2, Inbound = 3
    LinearFeet: 0;
    //Response Fields updated by Controller
    RespCapacityProviderBolUrl: "";
    RespPackingVisualizationUrl: "";
    RespPickupNote: "";
    RespPickupDateTime: "";
    InfoMessages: []; //maps to APIMessage object generally only populated by the controller on the server.
    ErrorCode: "";    // like "400 invalid request" "401 invalid or missing credentials" "403 User not authorized to perform this operation"
    ErrorMessage: "";
    Errors: [];      //maps to APIMessage object generally only populated by the controller on the server.
    BidControl: 0;
    LineHaul: 0;
    Fuel: 0;
    TotalCost: 0;
    BOLLegalText: "";
    DispatchLegalText: "";
    //Modified by RHR for v-8.2 on 12/22/2018 added new fields needed for dispatching
    DispatchBidType: 0;
    BookControl: 0;
    ModeTypeControl: 0;
    CarrierControl: 0;
    CarrTarEquipMatControl: 0;
    CarrTarEquipControl: 0;
    FuelVariable: 0;
    FuelUOM: "";
    DispatchLoadTenderType: 0;
    CarrierContact: null;  //maps to Contact object //Added By LVV on 2/19/2019 for bug fix add Carrier Contact selection to Dispatch
    AutoAcceptOnDispatch: false;
    EmailLoadTenderSheet: true;
    CarrierName: "";
    ItemOrderNumbers: "";
    OtherCost: 0;
}

function APIMessage() {
    Severity: ""; // like  "ERROR" "WARNING" "INFO" 
    Message: "";
    Diagnostic: "";
    Source: "";  // like "SYSTEM" "CAPACITY_PROVIDER" 
}

function Item() {
    ItemNumber: "";
    ItemDesc: "";
    ItemFreightClass: "70";
    ItemWgt: 0;
    ItemPackageType: "PLT";
    ItemTotalPackages: 0;
    ItemPieces: 0;
    ItemStackable: false;
    ItemLength: 0;
    ItemWidth: 0;
    ItemHeight: 0;
    ItemNMFCItemCode: "";
    ItemNMFCSubCode: "";
    ItemIsHazmat: false;
    ItemHazmatID: "";
    ItemHazmatClass: "";
    ItemHazmatPackingGroup: "";
    ItemHazmatProperShipName: "";
    ItemDimensions: "";
    ItemCube: 0; //Added By LVV for BOL Report Changes

}

function AddressBook() {
    Name: "";
    Address1: "";
    Address2: "";
    Address3: "";
    City: "";
    State: "";
    Country: "";
    Zip: "";
    LocationCode: "";
    Contact: null;  //maps to Contact object

}

function Contact() {
    ContactControl: 0;
    ContactName: "";
    ContactTitle: "";
    ContactPhone: "";
    ContactPhoneExt: "";
    ContactFax: "";
    Contact800: "";
    ContactEmail: "";
    ContactDefault: false;
    ContactTender: false;
    ContactScheduler: false;
    ContactCarrierControl: 0;
    ContactLECarControl: 0;
    ContactCompControl: 0;
}

function User() {
    UserSecurityControl: 0;
    UserEmail: "";
    UserName: "";
    UserFriendlyName: "";
    UserFirstName: "";
    UserLastName: "";
    UserTitle: "";
    UserDepartment: "";
    UserPhoneCell: "";
    UserPhoneHome: "";
    UserPhoneWork: "";
    UserPhoneWorkExt: "";
    UserTheme365: "";
    UserStartFreeTrial: "";
    UserEndFreeTrial: "";
    UserFreeTrialActive: 1;

    //Added By LVV on 4/3/18 for v-8.0 TMS 365
    UserUserGroupsControl: 0;
    LEAControl: 0;
    UseMicrosoftAccount: false;
    AllowNGLAPI: false;
    AccountGroup: "";

    AutoGeneratePwd: false;
    SendUserPwd: false;
    Pwd: "";

    blnIsCarrierUser: false;
    AssociatedCarriers: [];
}

function GenericResult() {
    Control: 0;
    ErrNumber: 0;
    RetMsg: "";
    strField: "";
    strField2: "";
    strField3: "";
    strField4: "";
    dtField: "";
    blnField: 0;
    blnField1: 0;
    blnField2: 0;
    intField1: 0;
    intField2: 0;
    intField3: 0;
    decField1: 0;
    longField1: 0;
    intArray: [];
    strArray: [];
    log: [];
}

function CarrierLegalAccessorialXref() {
    CLAXControl: 0;
    LEAdminControl: 0;
    CarrierControl: 0;
    AccessorialCode: 0;
    Caption: "";
    EDICode: "";
    AutoApprove: 0;
    AllowCarrierUpdates: 0;
    AccessorialVisible: 1;
    ApproveToleranceLow: 0;
    ApproveToleranceHigh: 0;
    ApproveTolerancePerLow: 0;
    ApproveTolerancePerHigh: 0;
    AverageValue: 0;
    DynamicAverageValue: 0;
}

function SettlementFee() {
    Control: 0;
    BookControl: 0;
    Minimum: 0;
    Cost: 0;
    AccessorialCode: 0;
    Caption: "";
    AutoApprove: false;
    AllowCarrierUpdates: false;
    FeeIndex: 0;
    Pending: true;
    Msg: "";
    StopSequence: -1; //Missing (New field Default -1, When -1 the system must lookup Using BookControl) Maps To APMassEntryHistoryFees.APMHFeesStopSequence
    BookCarrOrderNumber: null; //Missng (ne field Default null, When a bookcontrol exists And this Is null lookup the value In the book table) Maps To  APMassEntryHistoryFees.APMHFeesOrderNumber
    BFPControl: 0;
    EDICode: "";
    BookOrderSequence: 0;
    MissingFee: false;
    BilledFee: false;
    OrigName: ""; //Added By LVV on 3/15/20
    DestName: ""; //Added By LVV on 3/15/20
    CNS: ""; //Added By LVV on 3/15/20
    SHID: ""; //Added By LVV on 3/15/20
    FeeAllocationTypeControl: 0; //Added By LVV on 3/15/20
    FeeAllocationTypeName: ""; //Added By LVV on 3/15/20
    FeeAllocationTypeDesc: ""; //Added By LVV on 3/15/20
}

function SettlementSave() {
    ID: 0;
    BookSHID: "";
    BookControl: 0;
    CarrierControl: 0;
    CompControl: 0;
    InvoiceAmt: 0;
    LineHaul: 0;
    TotalFuel: 0;
    InvoiceNo: "";
    BookCarrBLNumber: "";
    BookFinAPActWgt: 0;
    Fees: [];
    APBillDate: "";
}

function SettlementFBDEData() {
    LastStopCompControl: 0;
    LastStopCompName: "";
    LastStopCarrierControl: 0;
    LastStopCarrierName: "";
    LastStopCompLE: "";
    ShowAuditFailReason: false;
    ShowPendingFeeFailReason: false;
    APMessage: "";
    LoadFees: [];
    OrderFees: [];
    OrigFees: [];
    DestFees: [];
}

function CarrierDispatchSettings() {
    LECarControl: 0; //REPLACE *
    LEAdminControl: 0;
    CarrierControl: 0;
    DispatchTypeControl: 0;
    RateShopOnly: 0;
    APIDispatching: 0;
    APIStatusUpdates: 0;
    ShowAuditFailReason: 0;
    ShowPendingFeeFailReason: 0;
    BillToCompControl: 0;
    CarrierAccountRef: "";
    LECarUseDefault: true;
    LECarExpiredLoadsTo: "";
    LECarExpiredLoadsCc: "";
    LECarCarrierAcceptLoadMins: 0;
    LECarBillingAddress1: "";
    LECarBillingAddress2: "";
    LECarBillingAddress3: "";
    LECarBillingCity: "";
    LECarBillingState: "";
    LECarBillingZip: "";
    LECarBillingCountry: "";
    LECarAllowLTLConsolidation: false;
}

function LECarrierCont() {
    LECarContControl: 0;
    LECarContNACXControl: 0;
    LECarContName: "";
    LECarContTitle: "";
    LECarContPhone: "";
    LECarContPhoneExt: "";
    LECarContFax: "";
    LECarCont800: "";
    LECarContEmail: "";
    LECarContDefault: false;
    LECarContModDate: "";
    LECarContModUser: "";
    LECarContUpdated: "";
}

function InsertOrUpdateLE() {
    //LE fields
    LEAdminControl: 0;
    LegalEntity: "";
    LEAdminCNSPrefix: "";
    LEAdminCNSNumberLow: 0;
    LEAdminCNSNumberHigh: 0;
    LEAdminCNSNumber: 0;
    LEAdminPRONumber: 0;
    LEAdminAllowCreateOrderSeq: true;
    LEAdminAutoAssignOrderSeqSeed: 0;
    //Modified by RHR for v-8.1.1.1 on 05/10/2018
    LEAdminBOLLegalText: "";
    LEAdminDispatchLegalText: "";
    LEAdminCarApptAutomation: false; //default off(0)
    LEAdminApptModCutOffMinutes: 2880; //default = to 48 hours stored as 2880  minutes
    LEAdminDefaultLastLoadTime: "15:00"; //default = 15:00	equal to 3 pm
    LEAdminApptNotSetAlertMinute: 2880; //default = to 48 hours stored as 2880  minutes
    LEAdminAllowApptEdit: false; //default off(0)
    LEAdminAllowApptDelete: false; //default off(0)
    LEAdminCarrierAcceptLoadMins: 0;
    LEAdminExpiredLoadsTo: "";
    LEAdminExpiredLoadsCc: "";
    //Comp fields
    CompControl: 0;
    CompActive: true;
    CompNumber: 0;
    CompName: "";
    CompAlphaCode: "";
    CompAbrev: "";
    CompWebsite: "";
    CompEmail: "";
    CompAddress1: "";
    CompAddress2: "";
    CompAddress3: "";
    CompCity: "";
    CompState: "";
    CompZip: "";
    CompCountry: "";
    //CompCont fields
    CompContName: "";
    CompContTitle: "";
    CompCont800: "";
    CompContPhone: "";
    CompContPhoneExt: "";
    CompContFax: "";
    CompContEmail: "";
    CompContTender: false;
}

function SingleSignOn() {
    SSOAXControl: 0;
    SSOAXUN: "";
    SSOAXPass: "";
    SSOAXRefID: "";
    USC: 0;
    UserName: "";
    NewPass: "";
    SSOAControl: 0;
    SSOAName: "";
    SSOADesc: "";
    UpdateP: false;
}

function LegalEntityAdmin() {
    LEAdminControl: 0;
    LegalEntity: "";
}

function CompMaint() {
    //Comp fields
    Company: null;
    CompanyContact: null;
}

function Comp() {
    //Comp fields
    CompControl: 0;
    CompActive: true;
    CompNumber: 0;
    CompName: "";
    CompLegalEntity: "";
    CompAlphaCode: "";
    CompAbrev: "";
    CompWeb: "";
    CompEmail: "";
    CompStreetAddress1: "";
    CompStreetAddress2: "";
    CompStreetAddress3: "";
    CompStreetCity: "";
    CompStreetState: "";
    CompStreetZip: "";
    CompStreetCountry: "";
    //LEAdmin
    LEAdminControl: 0;
}

function CompContact() {
    CompContControl: 0;
    CompContCompControl: 0;
    CompContName: "";
    CompContTitle: "";
    CompCont800: "";
    CompContPhone: "";
    CompContPhoneExt: "";
    CompContFax: "";
    CompContEmail: "";
    CompContTender: false;
    CompContUpdated: "";
}

function Lane() {
    LaneControl: 0;
    LEAdminControl: 0;
    LTTransType: 3;
    LaneCompControl: 0;
    CompName: "";
    CompNumber: "";
    LaneNumber: "";
    LaneName: "";
    LaneLegalEntity: "";
    LaneActive: true;
    LaneOriginAddressUse: true;
    LaneTransType: "";
    LaneTempType: "";
    TransType: "";
    TempType: "";
    LaneComments: "";
    LaneCommentsConfidential: "";
    LaneOrigCompControl: 0;
    LaneOrigName: "";
    LaneOrigAddress1: "";
    LaneOrigAddress2: "";
    LaneOrigAddress3: "";
    LaneOrigCity: "";
    LaneOrigState: "";
    LaneOrigZip: "";
    LaneOrigCountry: "";
    LaneOrigContactName: "";
    LaneOrigContactPhone: "";
    LaneOrigContactEmail: "";
    LaneOrigEmergencyContactPhone: "";
    LaneOrigEmergencyContactName: "";
    LaneAppt: false;
    LaneRecHourStart: "";
    LaneRecHourStop: "";
    LaneDestCompControl: 0;
    LaneDestName: "";
    LaneDestAddress1: "";
    LaneDestAddress2: "";
    LaneDestAddress3: "";
    LaneDestCity: "";
    LaneDestState: "";
    LaneDestZip: "";
    LaneDestCountry: "";
    LaneDestContactName: "";
    LaneDestContactPhone: "";
    LaneDestContactEmail: "";
    LaneDestEmergencyContactPhone: "";
    LaneDestEmergencyContactName: "";
    LaneAptDelivery: false;
    LaneDestHourStart: "";
    LaneDestHourStop: "";
    LaneModDate: "";
    LaneModUser: "";
    LaneUpdated: "";
    LaneAllowCarrierBookApptByEmail: false;
    LaneRequireCarrierAuthBookApptByEmail: false;
    LaneUseCarrieContEmailForBookApptByEmail: false;
    LaneCarrierBookApptviaTokenEmail: "";
    LaneCarrierBookApptviaTokenFailEmail: "";
    LaneCarrierBookApptviaTokenFailPhone: "";
    LaneTransLeadTimeLocationOption: 0;
    LaneTransLeadTimeUseMasterLane: false;
    LaneTransLeadTimeCalcType: 0;
}

function MenuTree() {
    MenuTreeControl: 0;
    Caption: "";
    LinkPageControl: 0;
    LinkTo: "";
    Expanded: false;
}


//GroupTypes Array
function GroupTypes() {

    this.arrTypes = [];

    this.addType = function (Control, Name, Desc) {
        var newType = new cmGroupType();
        newType.loadDefaults(Control, Name, Desc);
        this.arrTypes.push(newType)
    }

    this.addDefaultTypesIfNeeded = function () {
        if (this.arrTypes.length < 15) { this.loadDefaultTypes(); }
    }

    this.loadDefaultTypes = function () {
        arrTypes = [];
        addType(1, 'Data Management', 'Grids, spreadsheets etc...');
        addType(2, 'Editors', 'Text Boxes, Combo, DatePicker etc...');
        addType(3, 'Charts', 'Area, Bar, Bubble etc..');
        addType(4, 'BarCodes', 'barcode, QR Code');
        addType(5, 'Gauges', 'Linear, Radial');
        addType(6, 'Map', 'Map');
        addType(7, 'Diagram', 'workflows ');
        addType(8, 'Scheduling', 'Calendar, Scheduler, Gantt');
        addType(9, 'Layout', 'Dialogs, FastTabs, Notificaitons, Popup Windows etc...');
        addType(10, 'Navigation', 'Buttons, Menus, Panels, Tabs, etc...');
        addType(11, 'Media', 'MediaPlayer');
        addType(12, 'Interactive', 'ProgressBar,Filter Selection');
        addType(13, 'Grid Detail', 'Tab strips for grid');
        addType(14, 'HTML Content', 'Content for HTML Containers like Div tags');
        addType(15, 'Information', 'Infortmation Content like footers');
    }

    this.loadDefaults = function () {
        this.loadDefaultTypes();
        return this.arrTypes;
    }

}

//cmGroupType Model
function cmGroupType() {
    GroupTypeControl: "";
    GroupTypeName: "";
    GroupTypeDesc: "";
    GroupTypeModDate: "";
    GroupTypeModUser: "";
    GroupTypeUpdated: "";

    this.loadDefaults = function (TypeControl, Name, Desc) {
        this.GroupTypeControl = TypeControl;
        this.GroupTypeName = Name;
        this.GroupTypeDesc = Desc;
    }
}

//kendoMasks Defaults
function kendoMasks() {

    this.arrMasks = [];

    this.addMask = function (sName, sMask) {
        var newMask = {
            Name: sName,
            Mask: sMask
        }
        this.arrMasks.push(newMask)
    }

    this.addDefaultMasksIfNeeded = function () {
        if (this.arrMasks.length < 3) { this.loadDefaultMasks(); }
    }

    this.loadDefaultMasks = function () {
        this.arrMasks = [
            {
                Name: "phone_number",
                Mask: "(999) 000-0000"
            },
            {
                Name: "credit_card",
                Mask: "0000 0000 0000 0000"
            },
            {
                Name: "ssn",
                Mask: "000-00-0000"
            }
        ];
    }

    this.loadDefaults = function (sName, sMask) {
        this.addDefaultMasksIfNeeded();
        this.addMask(sName, sMask);
        return this.arrMasks;
    }

    this.getMask = function (sKey) {
        if (ngl.isArray(this.arrMasks)) {
            for (var i = 0; i < this.arrMasks.length; i++) {
                var sName = this.arrMasks[i].Name;
                if (sName === sKey) {
                    return this.arrMasks[i].Mask;
                    break;
                }
            }
        }
        return '';
    }

}

function nglEventParameters() {
    source: ""; //the source of the event like "ContainerSelected" as a string
    msg: ""; // returned message typically "Success" or "Failed"
    error: null; // a javascript Error Object properties are name and message
    widget: null; // object containing the source
    text: "";  // text value associated with the event as a string like the name of the selected item
    value: 0;  // numeric value associated with the event like the control number of the selected item
    description: "";  // description of the value associated with the event as a string like the description of the item selected
    data: null; // the data returned as a result of the event; the event handler must be aware of the possible types of data returned from source
    datatype: ""; //the name or description of the object returned via data
    keyindex: 0;  // optional index key used to find a reference to the object triggering the callback
    keyname: ""; // optional unique name used as a reference to the object triggering the callback
    CRUD: ""; // returns null, create, read, update, or delete match with msg for "Success" or "Failed"
    Dialog: null; //reference to an open alert dialog widget,  call back may need to call the toFront method like:  $("#dialog").kendoDialog(); var dialog = $("#dialog").data("kendoDialog"); dialog.toFront();

}

//GroupSubTypes Array
function GroupSubTypes() {

    this.arrSubTypes = [];

    this.addSubType = function (Control, GroupTypeControl, Name, Desc) {
        var newSubType = new cmGroupSubType();
        newSubType.loadDefaults(Control, GroupTypeControl, Name, Desc);
        this.arrSubTypes.push(newSubType)
    }

    this.addDefaultSubTypesIfNeeded = function () {
        if (this.arrSubTypes.length < 62) { this.loadDefaultSubTypes(); }
    }

    this.loadDefaultSubTypes = function () {
        this.arrSubTypes = [];
        this.addSubType(0, 9, 'Page', 'Page');
        this.addSubType(1, 1, 'Grid', 'Data Grids');
        this.addSubType(2, 1, 'SpreadSheet', 'Spread Sheets');
        this.addSubType(3, 1, 'ListView', 'List Views');
        this.addSubType(4, 1, 'TreeList', 'Tree Types Lists');
        this.addSubType(5, 1, 'ScrollView', 'Next and Previous Scrolling on record at a time');
        this.addSubType(6, 2, 'AutoComplete', 'Auto Complete Text Box');
        this.addSubType(7, 2, 'ColorPicker', 'Color Pallet Selection Data');
        this.addSubType(8, 2, 'ComboBox', 'Combination Editor and drop down list');
        this.addSubType(9, 2, 'DatePicker', 'Date Selection Data');
        this.addSubType(10, 2, 'DateTimePicker', 'Date and Time Selection Data');
        this.addSubType(11, 2, 'DropDownList', 'Read only drop down list');
        this.addSubType(12, 2, 'Editor', 'Normal Text Editor');
        this.addSubType(13, 2, 'MaskedTextBox', 'Text Editor with a Mask applied');
        this.addSubType(14, 2, 'MultiSelect', 'Mulit-Selection List');
        this.addSubType(15, 2, 'NumericTextBox', 'Limit data entry to numbers');
        this.addSubType(16, 2, 'kendoSwitch', 'On Off Boolean');
        this.addSubType(17, 10, 'Button', 'Button');
        this.addSubType(18, 13, 'Tab Strip', 'Grid Detail Tab Strips');
        this.addSubType(19, 9, 'Grid Fast Tab', 'Fast Tab Container for Kendo Grid');
        this.addSubType(20, 12, 'Filter Selection', 'Filter Selection Area');
        this.addSubType(22, 9, 'Form Fast Tab', 'Fast Tab Container for Page Data Entry Form');
        this.addSubType(23, 12, 'Fast Tab Action', 'Actions Executed Via Fast Tabs');
        this.addSubType(24, 2, 'Editor Custom Tool', 'Editor Toolbar Custom Tool');
        this.addSubType(25, 2, 'Editor Standard Tools', 'Included Editor Toolbar Tools ');
        this.addSubType(26, 9, 'Standard Border', 'Standard Layout Div with Rounded Border ');
        this.addSubType(27, 9, 'Full Page Border', 'Full Page Display Layout Div with Rounded Border ');
        this.addSubType(28, 9, 'Float Block Left', 'Left Floating Div with No Border ');
        this.addSubType(29, 14, 'Two Column Data', 'Multiple Row Table Container with No Borders ');
        this.addSubType(30, 14, 'Line', 'HR Line Tag');
        this.addSubType(31, 14, 'Header 1', 'H1 container ');
        this.addSubType(32, 14, 'Header 2', 'H2 container ');
        this.addSubType(33, 14, 'Header 3', 'H3 container ');
        this.addSubType(34, 14, 'Header 4', 'H4 container ');
        this.addSubType(35, 14, 'Div', 'Standard Div container ');
        this.addSubType(36, 14, 'Span', 'Standard Span container ');
        this.addSubType(37, 14, 'Paragraph', 'Standard Paragraph container ');
        this.addSubType(38, 14, 'Images', 'Standard img tag ref in metadata ');
        this.addSubType(39, 14, 'Link', 'Standard html link tag ref in metadata ');
        this.addSubType(40, 15, 'Page Footer', 'Standard Page Footer ');
        this.addSubType(41, 14, 'Kendo Icon Button', 'Requires sub type 42 (K-Icon) ');
        this.addSubType(42, 14, 'K-Icon', 'Kendo Icon Span image in MetaData');
        this.addSubType(43, 14, 'HTML Space', 'Standard HTML Space &nbsp; ');
        this.addSubType(44, 14, 'HTML Line Break', 'Standard HTML line break <br />');
        this.addSubType(45, 15, 'Full Page Footer', 'Footer typically nested inside of a Full Page Border');
        this.addSubType(46, 9, 'Tool Tip', 'Kendo Tool Tip');
        this.addSubType(47, 13, 'GridToolBarTemplate', 'Data grid tool bar using cm template control');
        this.addSubType(48, 13, 'KendoGridExportToolBar', 'Default Kendo Excel and PDF export tools for grid');
        this.addSubType(49, 2, 'TimePicker', 'Time Selection Data');
        this.addSubType(50, 1, 'NGLEditWindCtrl', 'Widget for Edit Add and Delete via Content Management Popup Window');
        this.addSubType(51, 1, 'NGLAddDependentCtrl', 'Widget for Adding Dependent Records via Content Management Popup Window');
        this.addSubType(52, 1, 'NGLSummaryDataCtrl', 'Widget to display read only summary data on a page using Content Management');
        this.addSubType(53, 1, 'NGLEditOnPageCtrl', 'Widget for Adding and Editing Records on a page using Content Management');
        this.addSubType(54, 2, 'Checkbox', 'HTML Input Checkbox');
        this.addSubType(55, 14, 'FloatLeftTable', 'Multi-Column Floating Table');
        this.addSubType(56, 14, 'TableHeader', 'Multi-Column Table Headers, determines number of columns');
        this.addSubType(57, 14, 'Label', 'Label Text');
        this.addSubType(58, 1, 'NGLWorkFlowOptionCtrl', 'Widget for Workflow option selections via Content Management Window');
        this.addSubType(59, 2, 'NGLWorkFlowGroup', 'Group ul container for  NGLWorkFlow...Switches');
        this.addSubType(60, 2, 'NGLWorkFlowOnOffSwitch', 'On Off Switch for NGLWorkFlowOptionCtrl via Content Management Window');
        this.addSubType(61, 2, 'NGLWorkFlowYesNoSwitch', 'Yes No Switch for NGLWorkFlowOptionCtrl via Content Management Window');
        this.addSubType(62, 9, 'NGLWorkFlowSectionCtrl', 'Widget for Workflow data section associated with a Workflow switch or a workdflow group');
        this.addSubType(63, 9, 'NGLPopupWindCtrl', 'Widget container for displaying nested NGL Ctrls via popup window');
        this.addSubType(64, 9, 'NGLErrWarnMsgLogCtrl', 'Widget container for displaying Errors, Warnings, Messages and Logs via popup window');
        this.addSubType(65, 10, 'DropDownButton', 'DropDownButton');
    }

    this.getSubTypeName = function (subTypeControl) {
        var sRet = "";
        for (var i = 0; i < (this.arrSubTypes.length); i++) {
            var stype = this.arrSubTypes[i];
            if (stype.GroupSubTypeControl == subTypeControl) {
                sRet = stype.GroupSubTypeName;
                break;
            }
        }
        return sRet;
    }

    this.isSubTypeAContainer = function (subTypeControl) {
        var bRet = false;
        for (var i = 0; i < (this.arrSubTypes.length); i++) {
            var stype = this.arrSubTypes[i];
            if (stype.GroupSubTypeControl == subTypeControl) {
                bRet = stype.isContainer();
                break;
            }
        }
        return bRet;
    }

    this.allowDataBindingForSubType = function (subTypeControl) {
        var bRet = false;
        for (var i = 0; i < (this.arrSubTypes.length); i++) {
            var stype = this.arrSubTypes[i];
            if (stype.GroupSubTypeControl == subTypeControl) {
                bRet = stype.allowDataBinding();
                break;
            }
        }
        return bRet;
    }

    this.getSubType = function (subTypeControl) {
        var sRet = null;
        for (var i = 0; i < (this.arrSubTypes.length); i++) {
            var stype = this.arrSubTypes[i];
            if (stype.GroupSubTypeControl == subTypeControl) {
                sRet = stype;
                break;
            }
        }
        return sRet;
    }

    this.loadDefaults = function () {
        this.addDefaultSubTypesIfNeeded();
        return this.arrSubTypes;
    }
}

//cmGroupSubType Model
function cmGroupSubType() {
    GroupSubTypeControl: "";
    GroupSubTypeGroupTypeControl: "";
    GroupSubTypeName: "";
    GroupSubTypeDesc: "";
    GroupSubTypeInfo: "";
    GroupSubTypeModDate: "";
    GroupSubTypeModUser: "";
    GroupSubTypeUpdated: "";

    this.isContainer = function () {
        var blnRet = false;
        switch (this.GroupSubTypeGroupTypeControl) {
            case 1: //Grids SpreadSheets Lists etc...
                switch (this.GroupSubTypeControl) {
                    case 3: //ListView                        
                        break;
                    case 4: //TreeView
                        break;
                    default: //Grid, SpreadSheet, ScrollView
                        blnRet = true
                }
                break;
            case 2: //Editors some may contain other elements 
                switch (this.GroupSubTypeControl) {
                    case 12: //Editor
                    case 59: //NGLWorkFlowGroup
                    case 60: //NGLWorkFlowOnOffSwitch
                    case 61: //NGLWorkFlowYesNoSwitch
                        blnRet = true
                        break;
                    default: //All others return false
                        blnRet = false
                }
                break;
            case 9: //Dialogs, FastTabs, Notificaitons, Popup Windows etc...
                blnRet = true;
                break;
            case 10://Buttons, Menus, Panels, Tabs, etc...
                switch (this.GroupSubTypeControl) {
                    case 17: //Button
                        break;
                    case 65: //Drop Down Button
                        break;
                    default:
                        blnRet = true
                }
                break;
            case 12: //Filter Selection
                blnRet = true;
                break;
            case 13://Grid Details, tab strips toolbars etc..
                switch (this.GroupSubTypeControl) {
                    case 18: //Tab strips for grid
                        blnRet = true;
                        break;
                    default:
                        blnRet = false
                }
                break;
            case 14: //Content for HTML Containers like Div tags
                switch (this.GroupSubTypeControl) {
                    case 30: //HR Line Tag
                        break;
                    case 38: //Standard img tag ref in metadata
                        break;
                    case 42: //Kendo Icon Span image in MetaData
                        break;
                    case 43: //Standard HTML Space &nbsp; 
                        break;
                    case 44: //Standard HTML line break <br /> 
                        break;
                    default:
                        blnRet = true
                }
                break;
            default:
                break;
        }
        return blnRet;
    }

    this.allowDataBinding = function () {
        var blnRet = false;
        switch (this.GroupSubTypeControl) {
            case 1: // Grid; Data Grids
            case 2: // SpreadSheet; Spread Sheets
            case 3: // ListView; List Views
            case 4: // TreeList; Tree Types Lists
            case 8: // ComboBox; Combination Editor and drop down list
            case 11: // DropDownList; Read only drop down list
            case 14: // MultiSelect; Mulit-Selection List
            case 29: // Two Column Data; Multiple Row Table Container with No Borders 
            case 50: // NGLEditWindCtrl; Widget for Edit Add and Delete via Content Management Popup Window
            case 51: // NGLAddDependentCtrl; Widget for Adding Dependent Records via Content Management Popup Window
            case 52: // NGLSummaryDataCtrl; Widget to display read only summary data on a page using Content Management
            case 53: // NGLEditOnPageCtrl; Widget for Adding and Editing Records on a page using Content Management
                blnRet = true;
                break;
            default:
                blnRet = false;
                break;
        }
        return blnRet;
    }

    this.allowCaption = function () {
        var blnRet = true;
        switch (this.GroupSubTypeControl) {
            case 26: //Standard Border
                blnRet = false;
                break;
            case 27: //Full Page Border
                blnRet = false;
                break;
            case 28: //Left Floating Div with No Border
                blnRet = false;
                break;
            case 29: //Multiple Row Table Container with No Borders
                blnRet = false;
                break;
            case 30: //HR Line Tag
                blnRet = false;
                break;
            case 37: //Standard Paragraph container
                blnRet = false;
                break;
            case 38: //Standard img tag ref in metadata
                blnRet = false;
                break;
            case 40: //Standard img tag ref in metadata
                blnRet = false;
                break;
            case 42: //Kendo Icon Span image in MetaData
                blnRet = false;
                break;
            case 43: //Standard HTML Space &nbsp;
                blnRet = false;
                break;
            case 44: //Standard HTML line break <br />
                blnRet = false;
                break;
            case 45: //Footer typically nested inside of a Full Page Border
                blnRet = false;
                break;
            case 47: //Data grid tool bar using cm template control
                blnRet = false;
                break;
            case 48: //Default Kendo Excel and PDF export tools for grid
                blnRet = false;
                break;
            case 55: //  FloatLeftTable
                blnRet = false;
                break;
            default:
                break;
        }
        return blnRet;
    }

    this.canContain = function (childSubTypeControl) {
        var blnRet = false;
        switch (this.GroupSubTypeGroupTypeControl) {
            case 1: //Grids SpreadSheets Lists etc...
                switch (this.GroupSubTypeControl) {
                    case 1: //Grid  Modified by RHR for v-8.2.1 on 10/7/19 added the ability to extend which items are allowed on the control -->  Span, button, Header(1 -- > 4), Paragraph, Link, HTMLSpace, Div, FloatLeftTable, FloatBlockLeft, TableHeader, Label         
                        switch (childSubTypeControl) {
                            case 0: //page
                            case 2: // SpreadSheet; Spread Sheets
                            case 4: //TreeList  
                            case 5: //'ScrollView'
                            case 19: //Grid Fast Tab
                            case 20: // Filter Selection; Filter Selection Area
                            case 22: //Form Fast Tab
                            case 23: //Fast Tab Action
                            case 24: //Editor Custom Tool
                            case 25: //Editor Standard Tools
                            case 26: //Standard Border
                            case 27: //Full Page Border
                            case 29: //Two Column Data
                            case 30: //HR Line Tag
                            case 40: //Page Footer
                            case 42: //K-Icon
                            case 44: //HTML Line Break
                            case 45: //Full 
                            //case 50: // NGLEditWindCtrl; Widget for Edit Add and Delete via Content Management Popup Window
                            //case 51: // NGLAddDependentCtrl; Widget for Adding Dependent Records via Content Management Popup Window
                            case 52: // NGLSummaryDataCtrl; Widget to display read only summary data on a page using Content Management
                            case 53: // NGLEditOnPageCtrl; Widget for Adding and Editing Records on a page using Content Management
                            case 58: // NGLWorkFlowOptionCtrl; Widget for Workflow option selections via Content Management Window
                            case 59: // NGLWorkFlowGroup; Group ul container for  NGLWorkFlow...Switches
                            case 60: // NGLWorkFlowOnOffSwitch; On Off Switch for NGLWorkFlowOptionCtrl via Content Management Window
                            case 61: // NGLWorkFlowYesNoSwitch; Yes No Switch for NGLWorkFlowOptionCtrl via Content Management Window
                            case 62: // NGLWorkFlowSectionCtrl; Widget for Workflow data section associated with a Workflow switch or a workdflow group
                            case 63: // NGLPopupWindCtrl; Widget container for displaying nested NGL Ctrls via popup window
                                break; //false
                            default: //All others can be contained inside  this container
                                blnRet = true;
                                break;
                        }
                        break;
                    case 2: //spreadsheet//Grid                        
                        switch (childSubTypeControl) {
                            case 0: //page
                            case 1: // Grid; Data Grids
                            case 2: // SpreadSheet; Spread Sheets
                            case 4: //TreeList  
                            case 5: //'ScrollView'
                            case 18: // Tab Strip; Grid Detail Tab Strips
                            case 19: //Grid Fast Tab
                            case 20: // Filter Selection; Filter Selection Area
                            case 22: //Form Fast Tab
                            case 23: //Fast Tab Action
                            case 24: //Editor Custom Tool
                            case 25: //Editor Standard Tools
                            case 26: //Standard Border
                            case 27: //Full Page Border
                            case 28: //Float Block Left
                            case 29: //Two Column Data
                            case 30: //HR Line Tag
                            case 31: //Header 1
                            case 32: //Header 2
                            case 33: //Header 3
                            case 34: //Header 4
                            case 35: //Div
                            case 36: //Span
                            case 37: //Paragraph
                            case 40: //Page Footer
                            case 42: //K-Icon
                            case 43: //HTML Space
                            case 44: //HTML Line Break
                            case 45: // Full Page Footer; Footer typically nested inside of a Full Page Border
                            case 50: // NGLEditWindCtrl; Widget for Edit Add and Delete via Content Management Popup Window
                            case 51: // NGLAddDependentCtrl; Widget for Adding Dependent Records via Content Management Popup Window
                            case 52: // NGLSummaryDataCtrl; Widget to display read only summary data on a page using Content Management
                            case 53: // NGLEditOnPageCtrl; Widget for Adding and Editing Records on a page using Content Management
                            case 55: // FloatLeftTable; Multi-Column Floating Table
                            case 56: // TableHeader; Multi-Column Table Headers, determines number of columns
                            case 58: // NGLWorkFlowOptionCtrl; Widget for Workflow option selections via Content Management Window
                            case 59: // NGLWorkFlowGroup; Group ul container for  NGLWorkFlow...Switches
                            case 60: // NGLWorkFlowOnOffSwitch; On Off Switch for NGLWorkFlowOptionCtrl via Content Management Window
                            case 61: // NGLWorkFlowYesNoSwitch; Yes No Switch for NGLWorkFlowOptionCtrl via Content Management Window
                            case 62: // NGLWorkFlowSectionCtrl; Widget for Workflow data section associated with a Workflow switch or a workdflow group
                            case 63: // NGLPopupWindCtrl; Widget container for displaying nested NGL Ctrls via popup window
                                break; //false
                            default: //All others can be contained inside  this container
                                blnRet = true;
                                break;
                        }
                        break;
                    case 5: //Next and Previous Scrolling one record at a time                       
                        switch (childSubTypeControl) {
                            case 0: //page                          
                            case 5: //'ScrollView'                  
                            case 23: //Fast Tab Action
                            case 24: //Editor Custom Tool
                            case 25: //Editor Standard Tools
                            case 27: //Full Page Border
                            case 40: //Page Footer
                            case 42: //K-Icon
                            case 45: //Full Page Footer                            
                            case 50: // NGLEditWindCtrl; Widget for Edit Add and Delete via Content Management Popup Window
                            case 51: // NGLAddDependentCtrl; Widget for Adding Dependent Records via Content Management Popup Window
                            case 52: // NGLSummaryDataCtrl; Widget to display read only summary data on a page using Content Management
                            case 53: // NGLEditOnPageCtrl; Widget for Adding and Editing Records on a page using Content Management
                            case 55: // FloatLeftTable; Multi-Column Floating Table
                            case 56: // TableHeader; Multi-Column Table Headers, determines number of columns
                            case 58: // NGLWorkFlowOptionCtrl; Widget for Workflow option selections via Content Management Window
                            case 59: // NGLWorkFlowGroup; Group ul container for  NGLWorkFlow...Switches
                            case 60: // NGLWorkFlowOnOffSwitch; On Off Switch for NGLWorkFlowOptionCtrl via Content Management Window
                            case 61: // NGLWorkFlowYesNoSwitch; Yes No Switch for NGLWorkFlowOptionCtrl via Content Management Window
                            case 62: // NGLWorkFlowSectionCtrl; Widget for Workflow data section associated with a Workflow switch or a workdflow group
                            case 63: // NGLPopupWindCtrl; Widget container for displaying nested NGL Ctrls via popup window
                            default: //All others can be contained inside this container
                                blnRet = true;
                                break;
                        }
                        break;
                    case 50:  // NGLEditWindCtrl  Modified by RHR for v-8.2.1 on 10/7/19 added the ability to extend which items are allowed on the control -->  Span, button, Header(1 -- > 4), Paragraph, Link, HTMLSpace, Div, FloatLeftTable, FloatBlockLeft, TableHeader, Label
                        switch (childSubTypeControl) {
                            case 0: //page
                            case 1: // Grid; Data Grids
                            case 2: // SpreadSheet; Spread Sheets
                            case 3: // ListView; List Views
                            case 4: // TreeList; Tree Types Lists
                            case 5: // ScrollView; Next and Previous Scrolling on record at a time
                            case 6: // AutoComplete; Auto Complete Text Box
                            case 8: // ComboBox; Combination Editor and drop down list
                            case 14: // MultiSelect; Mulit-Selection List
                            case 18: // Tab Strip; Grid Detail Tab Strips
                            case 19: // Grid Fast Tab; Fast Tab Container for Kendo Grid
                            case 20: // Filter Selection; Filter Selection Area
                            case 22: // Form Fast Tab; Fast Tab Container for Page Data Entry Form
                            case 23: // Fast Tab Action; Actions Executed Via Fast Tabs
                            case 24: // Editor Custom Tool; Editor Toolbar Custom Tool
                            case 25: // Editor Standard Tools; Included Editor Toolbar Tools 
                            case 26: // Standard Border; Standard Layout Div with Rounded Border 
                            case 27: // Full Page Border; Full Page Display Layout Div with Rounded Border 
                            case 29: // Two Column Data; Multiple Row Table Container with No Borders 
                            case 30: // Line; HR Line Tag
                            case 38: // Images; Standard img tag ref in metadata 
                            case 40: // Page Footer; Standard Page Footer 
                            case 41: // Kendo Icon Button; Requires sub type 42 (K-Icon) 
                            case 42: // K-Icon; Kendo Icon Span image in MetaData
                            case 44: // HTML Line Break; Standard HTML line break <br />
                            case 45: // Full Page Footer; Footer typically nested inside of a Full Page Border
                            case 46: // Tool Tip; Kendo Tool Tip
                            case 47: // GridToolBarTemplate; Data grid tool bar using cm template control
                            case 48: // KendoGridExportToolBar; Default Kendo Excel and PDF export tools for grid
                            case 50: // NGLEditWindCtrl; Widget for Edit Add and Delete via Content Management Popup Window
                            case 51: // NGLAddDependentCtrl; Widget for Adding Dependent Records via Content Management Popup Window
                            case 52: // NGLSummaryDataCtrl; Widget to display read only summary data on a page using Content Management
                            case 53: // NGLEditOnPageCtrl; Widget for Adding and Editing Records on a page using Content Management
                            case 57: // Label; Label Text
                            case 58: // NGLWorkFlowOptionCtrl; Widget for Workflow option selections via Content Management Window
                            case 59: // NGLWorkFlowGroup; Group ul container for  NGLWorkFlow...Switches
                            case 60: // NGLWorkFlowOnOffSwitch; On Off Switch for NGLWorkFlowOptionCtrl via Content Management Window
                            case 61: // NGLWorkFlowYesNoSwitch; Yes No Switch for NGLWorkFlowOptionCtrl via Content Management Window
                            case 62: // NGLWorkFlowSectionCtrl; Widget for Workflow data section associated with a Workflow switch or a workdflow group
                            case 63: // NGLPopupWindCtrl; Widget container for displaying nested NGL Ctrls via popup window
                                break; //false
                            default: //All others can be contained inside this container
                                blnRet = true;
                                break;
                        }
                        break;
                    case 51:  // NGLAddDependentCtrl 
                        switch (childSubTypeControl) {
                            case 0: //page
                            case 1: // Grid; Data Grids
                            case 2: // SpreadSheet; Spread Sheets
                            case 3: // ListView; List Views
                            case 4: // TreeList; Tree Types Lists
                            case 5: // ScrollView; Next and Previous Scrolling on record at a time
                            case 6: // AutoComplete; Auto Complete Text Box
                            case 8: // ComboBox; Combination Editor and drop down list
                            case 14: // MultiSelect; Mulit-Selection List
                            case 17: // Button; Button
                            case 18: // Tab Strip; Grid Detail Tab Strips
                            case 19: // Grid Fast Tab; Fast Tab Container for Kendo Grid
                            case 20: // Filter Selection; Filter Selection Area
                            case 22: // Form Fast Tab; Fast Tab Container for Page Data Entry Form
                            case 23: // Fast Tab Action; Actions Executed Via Fast Tabs
                            case 24: // Editor Custom Tool; Editor Toolbar Custom Tool
                            case 25: // Editor Standard Tools; Included Editor Toolbar Tools 
                            case 26: // Standard Border; Standard Layout Div with Rounded Border 
                            case 27: // Full Page Border; Full Page Display Layout Div with Rounded Border 
                            case 28: // Float Block Left; Left Floating Div with No Border 
                            case 29: // Two Column Data; Multiple Row Table Container with No Borders 
                            case 30: // Line; HR Line Tag
                            case 31: // Header 1; H1 container 
                            case 32: // Header 2; H2 container 
                            case 33: // Header 3; H3 container 
                            case 34: // Header 4; H4 container 
                            case 35: // Div; Standard Div container 
                            case 36: // Span; Standard Span container 
                            case 37: // Paragraph; Standard Paragraph container 
                            case 38: // Images; Standard img tag ref in metadata 
                            case 39: // Link; Standard html link tag ref in metadata 
                            case 40: // Page Footer; Standard Page Footer 
                            case 41: // Kendo Icon Button; Requires sub type 42 (K-Icon) 
                            case 42: // K-Icon; Kendo Icon Span image in MetaData
                            case 43: // HTML Space; Standard HTML Space &nbsp; 
                            case 44: // HTML Line Break; Standard HTML line break <br />
                            case 45: // Full Page Footer; Footer typically nested inside of a Full Page Border
                            case 46: // Tool Tip; Kendo Tool Tip
                            case 47: // GridToolBarTemplate; Data grid tool bar using cm template control
                            case 48: // KendoGridExportToolBar; Default Kendo Excel and PDF export tools for grid
                            case 50: // NGLEditWindCtrl; Widget for Edit Add and Delete via Content Management Popup Window
                            case 51: // NGLAddDependentCtrl; Widget for Adding Dependent Records via Content Management Popup Window
                            case 52: // NGLSummaryDataCtrl; Widget to display read only summary data on a page using Content Management
                            case 53: // NGLEditOnPageCtrl; Widget for Adding and Editing Records on a page using Content Management
                            case 55: // FloatLeftTable; Multi-Column Floating Table
                            case 56: // TableHeader; Multi-Column Table Headers, determines number of columns
                            case 57: // Label; Label Text
                            case 58: // NGLWorkFlowOptionCtrl; Widget for Workflow option selections via Content Management Window
                            case 59: // NGLWorkFlowGroup; Group ul container for  NGLWorkFlow...Switches
                            case 60: // NGLWorkFlowOnOffSwitch; On Off Switch for NGLWorkFlowOptionCtrl via Content Management Window
                            case 61: // NGLWorkFlowYesNoSwitch; Yes No Switch for NGLWorkFlowOptionCtrl via Content Management Window
                            case 62: // NGLWorkFlowSectionCtrl; Widget for Workflow data section associated with a Workflow switch or a workdflow group
                            case 63: // NGLPopupWindCtrl; Widget container for displaying nested NGL Ctrls via popup window
                            case 65: // DropDownButton; Drop Down Button
                                break; //false
                            default: //All others can be contained inside this container
                                blnRet = true;
                                break;
                        }
                        break;
                    case 52:  // NGLSummaryDataCtrl
                        switch (childSubTypeControl) {
                            case 0: //page
                            case 1: // Grid; Data Grids
                            case 2: // SpreadSheet; Spread Sheets
                            case 3: // ListView; List Views
                            case 4: // TreeList; Tree Types Lists
                            case 5: // ScrollView; Next and Previous Scrolling on record at a time
                            case 6: // AutoComplete; Auto Complete Text Box
                            case 7: // ColorPicker; Color Pallet Selection Data
                            case 8: // ComboBox; Combination Editor and drop down list
                            case 11: // DropDownList; Read only drop down list
                            case 14: // MultiSelect; Mulit-Selection List
                            case 17: // Button; Button
                            case 18: // Tab Strip; Grid Detail Tab Strips
                            case 19: // Grid Fast Tab; Fast Tab Container for Kendo Grid
                            case 20: // Filter Selection; Filter Selection Area
                            case 22: // Form Fast Tab; Fast Tab Container for Page Data Entry Form
                            case 23: // Fast Tab Action; Actions Executed Via Fast Tabs
                            case 24: // Editor Custom Tool; Editor Toolbar Custom Tool
                            case 25: // Editor Standard Tools; Included Editor Toolbar Tools 
                            case 26: // Standard Border; Standard Layout Div with Rounded Border 
                            case 27: // Full Page Border; Full Page Display Layout Div with Rounded Border 
                            case 28: // Float Block Left; Left Floating Div with No Border 
                            case 29: // Two Column Data; Multiple Row Table Container with No Borders 
                            case 30: // Line; HR Line Tag
                            case 31: // Header 1; H1 container 
                            case 32: // Header 2; H2 container 
                            case 33: // Header 3; H3 container 
                            case 34: // Header 4; H4 container 
                            case 35: // Div; Standard Div container 
                            case 36: // Span; Standard Span container 
                            case 37: // Paragraph; Standard Paragraph container 
                            case 40: // Page Footer; Standard Page Footer 
                            case 41: // Kendo Icon Button; Requires sub type 42 (K-Icon) 
                            case 42: // K-Icon; Kendo Icon Span image in MetaData
                            case 43: // HTML Space; Standard HTML Space &nbsp; 
                            case 44: // HTML Line Break; Standard HTML line break <br />
                            case 45: // Full Page Footer; Footer typically nested inside of a Full Page Border
                            case 46: // Tool Tip; Kendo Tool Tip
                            case 47: // GridToolBarTemplate; Data grid tool bar using cm template control
                            case 48: // KendoGridExportToolBar; Default Kendo Excel and PDF export tools for grid
                            case 50: // NGLEditWindCtrl; Widget for Edit Add and Delete via Content Management Popup Window
                            case 51: // NGLAddDependentCtrl; Widget for Adding Dependent Records via Content Management Popup Window
                            case 52: // NGLSummaryDataCtrl; Widget to display read only summary data on a page using Content Management
                            case 53: // NGLEditOnPageCtrl; Widget for Adding and Editing Records on a page using Content Management
                            case 55: // FloatLeftTable; Multi-Column Floating Table
                            case 56: // TableHeader; Multi-Column Table Headers, determines number of columns
                            case 57: // Label; Label Text
                            case 58: // NGLWorkFlowOptionCtrl; Widget for Workflow option selections via Content Management Window
                            case 59: // NGLWorkFlowGroup; Group ul container for  NGLWorkFlow...Switches
                            case 60: // NGLWorkFlowOnOffSwitch; On Off Switch for NGLWorkFlowOptionCtrl via Content Management Window
                            case 61: // NGLWorkFlowYesNoSwitch; Yes No Switch for NGLWorkFlowOptionCtrl via Content Management Window
                            case 62: // NGLWorkFlowSectionCtrl; Widget for Workflow data section associated with a Workflow switch or a workdflow group
                            case 63: // NGLPopupWindCtrl; Widget container for displaying nested NGL Ctrls via popup window
                            case 65: // DropDownButton; Drop Down Button
                                break; //false
                            default: //All others can be contained inside this container
                                blnRet = true;
                                break;
                        }
                        break;
                    case 53:  // NGLEditOnPageCtrl 
                        switch (childSubTypeControl) {
                            case 0: //page
                            case 1: // Grid; Data Grids
                            case 2: // SpreadSheet; Spread Sheets
                            case 3: // ListView; List Views
                            case 4: // TreeList; Tree Types Lists
                            case 5: // ScrollView; Next and Previous Scrolling on record at a time
                            case 6: // AutoComplete; Auto Complete Text Box
                            case 8: // ComboBox; Combination Editor and drop down list
                            case 14: // MultiSelect; Mulit-Selection List
                            case 17: // Button; Button
                            case 18: // Tab Strip; Grid Detail Tab Strips
                            case 19: // Grid Fast Tab; Fast Tab Container for Kendo Grid
                            case 20: // Filter Selection; Filter Selection Area
                            case 22: // Form Fast Tab; Fast Tab Container for Page Data Entry Form
                            case 23: // Fast Tab Action; Actions Executed Via Fast Tabs
                            case 24: // Editor Custom Tool; Editor Toolbar Custom Tool
                            case 25: // Editor Standard Tools; Included Editor Toolbar Tools 
                            case 26: // Standard Border; Standard Layout Div with Rounded Border 
                            case 27: // Full Page Border; Full Page Display Layout Div with Rounded Border 
                            case 29: // Two Column Data; Multiple Row Table Container with No Borders 
                            case 40: // Page Footer; Standard Page Footer 
                            case 41: // Kendo Icon Button; Requires sub type 42 (K-Icon) 
                            case 42: // K-Icon; Kendo Icon Span image in MetaData
                            case 45: // Full Page Footer; Footer typically nested inside of a Full Page Border
                            case 46: // Tool Tip; Kendo Tool Tip
                            case 47: // GridToolBarTemplate; Data grid tool bar using cm template control
                            case 48: // KendoGridExportToolBar; Default Kendo Excel and PDF export tools for grid
                            case 50: // NGLEditWindCtrl; Widget for Edit Add and Delete via Content Management Popup Window
                            case 51: // NGLAddDependentCtrl; Widget for Adding Dependent Records via Content Management Popup Window
                            case 52: // NGLSummaryDataCtrl; Widget to display read only summary data on a page using Content Management
                            case 53: // NGLEditOnPageCtrl; Widget for Adding and Editing Records on a page using Content Management
                            case 58: // NGLWorkFlowOptionCtrl; Widget for Workflow option selections via Content Management Window
                            case 59: // NGLWorkFlowGroup; Group ul container for  NGLWorkFlow...Switches
                            case 60: // NGLWorkFlowOnOffSwitch; On Off Switch for NGLWorkFlowOptionCtrl via Content Management Window
                            case 61: // NGLWorkFlowYesNoSwitch; Yes No Switch for NGLWorkFlowOptionCtrl via Content Management Window
                            case 62: // NGLWorkFlowSectionCtrl; Widget for Workflow data section associated with a Workflow switch or a workdflow group
                            case 63: // NGLPopupWindCtrl; Widget container for displaying nested NGL Ctrls via popup window
                            case 65: // DropDownButton; Drop Down Button
                                break; //false
                            default: //All others can be contained inside this container
                                blnRet = true;
                                break;
                        }
                        break;
                    case 58:  // NGLWorkFlowOptionCtrl 
                        switch (childSubTypeControl) {
                            case 59: //NGLWorkFlowGroup
                            case 60: //NGLWorkFlowOnOffSwitch
                            case 61: //NGLWorkFlowYesNoSwitch
                                blnRet = true;
                                break;
                            default: //All others are false
                                blnRet = false;
                                break;
                        }
                        break;
                    default: //not a container
                        break;
                }
                break;
            case 2: // Some Editor Control can have children
                switch (this.GroupSubTypeControl) {
                    case 12: //Editor
                        switch (childSubTypeControl) {
                            case 24: //Editor Custom Tool
                            case 25: //Editor Standard Tools
                                blnRet = true;
                                break;
                            default: //All others are false
                                blnRet = false;
                                break;
                        }
                        break;
                    case 59:  // NGLWorkFlowGroup 
                        switch (childSubTypeControl) {
                            case 60: //NGLWorkFlowOnOffSwitch
                            case 61: //NGLWorkFlowYesNoSwitch
                            case 62: //NGLWorkFlowSectionCtrl
                                blnRet = true;
                                break;
                            default: //All others are false
                                blnRet = false;
                                break;
                        }
                        break;
                    case 60:  // NGLWorkFlowOnOffSwitch 
                        switch (childSubTypeControl) {
                            case 62: //NGLWorkFlowSectionCtrl
                                blnRet = true;
                                break;
                            default: //All others are false
                                blnRet = false;
                                break;
                        }
                        break;
                    case 61:  // NGLWorkFlowYesNoSwitch 
                        switch (childSubTypeControl) {
                            case 62: //NGLWorkFlowSectionCtrl
                                blnRet = true;
                                break;
                            default: //All others are false
                                blnRet = false;
                                break;
                        }
                        break;
                    default: //not a container
                        break;
                }
                break;
            case 9: //Pages Dialogs, FastTabs, Notificaitons, Popup Windows etc...
                switch (this.GroupSubTypeControl) {
                    case 0: //Page 
                        switch (childSubTypeControl) {
                            case 0: //page
                            case 23: //Fast Tab Action
                            case 24: //Editor Custom Tool
                            case 25: //Editor Standard Tools
                            case 40: //Page Footer
                            case 42: //K-Icon
                            case 45: // Full Page Footer; Footer typically nested inside of a Full Page Border
                            case 50: // NGLEditWindCtrl; Widget for Edit Add and Delete via Content Management Popup Window
                            case 51: // NGLAddDependentCtrl; Widget for Adding Dependent Records via Content Management Popup Window
                            case 52: // NGLSummaryDataCtrl; Widget to display read only summary data on a page using Content Management
                            case 53: // NGLEditOnPageCtrl; Widget for Adding and Editing Records on a page using Content Management
                            case 55: // FloatLeftTable; Multi-Column Floating Table
                            case 56: // TableHeader; Multi-Column Table Headers, determines number of columns
                            case 59: // NGLWorkFlowGroup; Group ul container for  NGLWorkFlow...Switches
                            case 60: // NGLWorkFlowOnOffSwitch; On Off Switch for NGLWorkFlowOptionCtrl via Content Management Window
                            case 61: // NGLWorkFlowYesNoSwitch; Yes No Switch for NGLWorkFlowOptionCtrl via Content Management Window
                            case 62: // NGLWorkFlowSectionCtrl; Widget for Workflow data section associated with a Workflow switch or a workdflow group
                            default: //All others can be contained inside this container
                                blnRet = true;
                                break;
                        }
                        break;
                    case 19: //Grid Fast Tab
                        switch (childSubTypeControl) {
                            case 0: //page
                            case 23: //Fast Tab Action
                            case 24: //Editor Custom Tool
                            case 25: //Editor Standard Tools
                            case 40: //Page Footer
                            case 42: //K-Icon
                            case 45: // Full Page Footer; Footer typically nested inside of a Full Page Border
                            case 50: // NGLEditWindCtrl; Widget for Edit Add and Delete via Content Management Popup Window
                            case 51: // NGLAddDependentCtrl; Widget for Adding Dependent Records via Content Management Popup Window
                            case 52: // NGLSummaryDataCtrl; Widget to display read only summary data on a page using Content Management
                            case 53: // NGLEditOnPageCtrl; Widget for Adding and Editing Records on a page using Content Management
                            case 55: // FloatLeftTable; Multi-Column Floating Table
                            case 56: // TableHeader; Multi-Column Table Headers, determines number of columns
                            case 58: // NGLWorkFlowOptionCtrl; Widget for Workflow option selections via Content Management Window
                            case 59: // NGLWorkFlowGroup; Group ul container for  NGLWorkFlow...Switches
                            case 60: // NGLWorkFlowOnOffSwitch; On Off Switch for NGLWorkFlowOptionCtrl via Content Management Window
                            case 61: // NGLWorkFlowYesNoSwitch; Yes No Switch for NGLWorkFlowOptionCtrl via Content Management Window
                            case 62: // NGLWorkFlowSectionCtrl; Widget for Workflow data section associated with a Workflow switch or a workdflow group
                            case 63: // NGLPopupWindCtrl; Widget container for displaying nested NGL Ctrls via popup window
                                break; //false;
                            default: //All others can be contained inside this container
                                blnRet = true;
                                break;
                        }
                        break;
                    case 22: //Form Fast Tab
                        switch (childSubTypeControl) {
                            case 0: //page
                            case 23: //Fast Tab Action
                            case 24: //Editor Custom Tool
                            case 25: //Editor Standard Tools
                            case 40: //Page Footer
                            case 42: //K-Icon
                            case 45: // Full Page Footer; Footer typically nested inside of a Full Page Border
                            case 50: // NGLEditWindCtrl; Widget for Edit Add and Delete via Content Management Popup Window
                            case 51: // NGLAddDependentCtrl; Widget for Adding Dependent Records via Content Management Popup Window
                            case 52: // NGLSummaryDataCtrl; Widget to display read only summary data on a page using Content Management
                            case 53: // NGLEditOnPageCtrl; Widget for Adding and Editing Records on a page using Content Management
                            case 55: // FloatLeftTable; Multi-Column Floating Table
                            case 56: // TableHeader; Multi-Column Table Headers, determines number of columns
                            case 58: // NGLWorkFlowOptionCtrl; Widget for Workflow option selections via Content Management Window
                            case 59: // NGLWorkFlowGroup; Group ul container for  NGLWorkFlow...Switches
                            case 60: // NGLWorkFlowOnOffSwitch; On Off Switch for NGLWorkFlowOptionCtrl via Content Management Window
                            case 61: // NGLWorkFlowYesNoSwitch; Yes No Switch for NGLWorkFlowOptionCtrl via Content Management Window
                            case 62: // NGLWorkFlowSectionCtrl; Widget for Workflow data section associated with a Workflow switch or a workdflow group
                            case 63: // NGLPopupWindCtrl; Widget container for displaying nested NGL Ctrls via popup window
                                break; //false
                            default: //All others can be contained inside this container
                                blnRet = true;
                                break;
                        }
                        break;
                    case 26: //Standard Border
                        switch (childSubTypeControl) {
                            case 0: //page
                            case 23: //Fast Tab Action
                            case 24: //Editor Custom Tool
                            case 25: //Editor Standard Tools
                            case 40: //Page Footer
                            case 42: //K-Icon
                            case 45: //Full Page Footer
                            case 50: // NGLEditWindCtrl; Widget for Edit Add and Delete via Content Management Popup Window
                            case 51: // NGLAddDependentCtrl; Widget for Adding Dependent Records via Content Management Popup Window
                            case 55: // FloatLeftTable; Multi-Column Floating Table
                            case 56: // TableHeader; Multi-Column Table Headers, determines number of columns
                                break;
                            default: //All others can be contained inside this container
                                blnRet = true;
                                break;
                        }
                        break;
                    case 27: //Full Page Border'
                        switch (childSubTypeControl) {
                            case 0: //page
                            case 23: //Fast Tab Action
                            case 24: //Editor Custom Tool
                            case 25: //Editor Standard Tools
                            case 26: //Standard Border
                            case 40: //Page Footer
                            case 42: //K-Icon
                            case 50: // NGLEditWindCtrl; Widget for Edit Add and Delete via Content Management Popup Window
                            case 51: // NGLAddDependentCtrl; Widget for Adding Dependent Records via Content Management Popup Window
                            case 55: // FloatLeftTable; Multi-Column Floating Table
                            case 56: // TableHeader; Multi-Column Table Headers, determines number of columns
                                break;  //false
                            default: //All others can be contained inside this container
                                blnRet = true;
                                break;
                        }
                        break;
                    case 28: //Float Block Left
                        switch (childSubTypeControl) {
                            case 0: //page
                            case 23: //Fast Tab Action
                            case 24: //Editor Custom Tool
                            case 25: //Editor Standard Tools
                            case 26: //Standard Border
                            case 27: //Full Page Border
                            case 40: //Page Footer
                            case 42: //K-Icon
                            case 45: //Full Page Footer
                            case 50: // NGLEditWindCtrl; Widget for Edit Add and Delete via Content Management Popup Window
                            case 51: // NGLAddDependentCtrl; Widget for Adding Dependent Records via Content Management Popup Window
                            case 55: // FloatLeftTable; Multi-Column Floating Table
                            case 56: // TableHeader; Multi-Column Table Headers, determines number of columns
                                break; //false
                            default: //All others can be contained inside this container
                                blnRet = true;
                                break;
                        }
                        break;
                    case 62:  // NGLWorkFlowSectionCtrl 
                        switch (childSubTypeControl) {
                            case 0: //page
                            case 3: // ListView; List Views
                            case 4: // TreeList; Tree Types Lists
                            case 5: // ScrollView; Next and Previous Scrolling on record at a time
                            case 6: // AutoComplete; Auto Complete Text Box
                            case 8: // ComboBox; Combination Editor and drop down list
                            case 14: // MultiSelect; Mulit-Selection List
                            case 23: // Fast Tab Action; Actions Executed Via Fast Tabs
                            case 24: // Editor Custom Tool; Editor Toolbar Custom Tool
                            case 25: // Editor Standard Tools; Included Editor Toolbar Tools 
                            case 26: // Standard Border; Standard Layout Div with Rounded Border 
                            case 27: // Full Page Border; Full Page Display Layout Div with Rounded Border 
                            case 29: // Two Column Data; Multiple Row Table Container with No Borders 
                            case 40: // Page Footer; Standard Page Footer 
                            case 41: // Kendo Icon Button; Requires sub type 42 (K-Icon) 
                            case 42: // K-Icon; Kendo Icon Span image in MetaData
                            case 45: // Full Page Footer; Footer typically nested inside of a Full Page Border
                            case 46: // Tool Tip; Kendo Tool Tip
                            case 47: // GridToolBarTemplate; Data grid tool bar using cm template control
                            case 48: // KendoGridExportToolBar; Default Kendo Excel and PDF export tools for grid
                            case 50: // NGLEditWindCtrl; Widget for Edit Add and Delete via Content Management Popup Window
                            case 51: // NGLAddDependentCtrl; Widget for Adding Dependent Records via Content Management Popup Window
                            case 52: // NGLSummaryDataCtrl; Widget to display read only summary data on a page using Content Management
                            case 53: // NGLEditOnPageCtrl; Widget for Adding and Editing Records on a page using Content Management
                            case 58: // NGLWorkFlowOptionCtrl; Widget for Workflow option selections via Content Management Window
                            case 59: // NGLWorkFlowGroup; Group ul container for  NGLWorkFlow...Switches
                            case 60: // NGLWorkFlowOnOffSwitch; On Off Switch for NGLWorkFlowOptionCtrl via Content Management Window
                            case 61: // NGLWorkFlowYesNoSwitch; Yes No Switch for NGLWorkFlowOptionCtrl via Content Management Window
                            case 62: // NGLWorkFlowSectionCtrl; Widget for Workflow data section associated with a Workflow switch or a workdflow group
                            case 63: // NGLPopupWindCtrl; Widget container for displaying nested NGL Ctrls via popup window
                                break; //false
                            default: //All others can be contained inside this container
                                blnRet = true;
                                break;
                        }
                        break;
                    case 63:  // NGLPopupWindCtrl 
                        switch (childSubTypeControl) {
                            case 0: //page
                            case 3: // ListView; List Views
                            case 4: // TreeList; Tree Types Lists
                            case 5: // ScrollView; Next and Previous Scrolling on record at a time
                            case 6: // AutoComplete; Auto Complete Text Box
                            case 8: // ComboBox; Combination Editor and drop down list
                            case 14: // MultiSelect; Mulit-Selection List
                            case 23: // Fast Tab Action; Actions Executed Via Fast Tabs
                            case 24: // Editor Custom Tool; Editor Toolbar Custom Tool
                            case 25: // Editor Standard Tools; Included Editor Toolbar Tools 
                            case 26: // Standard Border; Standard Layout Div with Rounded Border 
                            case 27: // Full Page Border; Full Page Display Layout Div with Rounded Border 
                            case 29: // Two Column Data; Multiple Row Table Container with No Borders 
                            case 40: // Page Footer; Standard Page Footer 
                            case 41: // Kendo Icon Button; Requires sub type 42 (K-Icon) 
                            case 42: // K-Icon; Kendo Icon Span image in MetaData
                            case 45: // Full Page Footer; Footer typically nested inside of a Full Page Border
                            case 46: // Tool Tip; Kendo Tool Tip
                            case 47: // GridToolBarTemplate; Data grid tool bar using cm template control
                            case 48: // KendoGridExportToolBar; Default Kendo Excel and PDF export tools for grid
                            case 50: // NGLEditWindCtrl; Widget for Edit Add and Delete via Content Management Popup Window
                            case 51: // NGLAddDependentCtrl; Widget for Adding Dependent Records via Content Management Popup Window
                            case 52: // NGLSummaryDataCtrl; Widget to display read only summary data on a page using Content Management
                            case 53: // NGLEditOnPageCtrl; Widget for Adding and Editing Records on a page using Content Management
                            case 59: // NGLWorkFlowGroup; Group ul container for  NGLWorkFlow...Switches
                            case 60: // NGLWorkFlowOnOffSwitch; On Off Switch for NGLWorkFlowOptionCtrl via Content Management Window
                            case 61: // NGLWorkFlowYesNoSwitch; Yes No Switch for NGLWorkFlowOptionCtrl via Content Management Window
                            case 62: // NGLWorkFlowSectionCtrl; Widget for Workflow data section associated with a Workflow switch or a workdflow group
                            case 63: // NGLPopupWindCtrl; Widget container for displaying nested NGL Ctrls via popup window
                                break; //false
                            default: //All others can be contained inside this container
                                blnRet = true;
                                break;
                        }
                        break;
                    default: //not a container
                        break;
                }
                break;
            case 10://Buttons, Menus, Panels, Tabs, etc...
                switch (this.GroupSubTypeControl) {
                    case 17: //Button
                    case 65: // DropDownButton; Drop Down Button
                        break;
                    default:
                        switch (childSubTypeControl) {
                            case 0: //page
                            case 23: //Fast Tab Action
                            case 24: //Editor Custom Tool
                            case 25: //Editor Standard Tools
                            case 26: //Standard Border
                            case 40: //Page Footer
                            case 42: //K-Icon
                            case 45: // Full Page Footer; Footer typically nested inside of a Full Page Border
                            case 50: // NGLEditWindCtrl; Widget for Edit Add and Delete via Content Management Popup Window
                            case 51: // NGLAddDependentCtrl; Widget for Adding Dependent Records via Content Management Popup Window
                            case 52: // NGLSummaryDataCtrl; Widget to display read only summary data on a page using Content Management
                            case 53: // NGLEditOnPageCtrl; Widget for Adding and Editing Records on a page using Content Management
                            case 55: // FloatLeftTable; Multi-Column Floating Table
                            case 56: // TableHeader; Multi-Column Table Headers, determines number of columns
                            case 58: // NGLWorkFlowOptionCtrl; Widget for Workflow option selections via Content Management Window
                            case 59: // NGLWorkFlowGroup; Group ul container for  NGLWorkFlow...Switches
                            case 60: // NGLWorkFlowOnOffSwitch; On Off Switch for NGLWorkFlowOptionCtrl via Content Management Window
                            case 61: // NGLWorkFlowYesNoSwitch; Yes No Switch for NGLWorkFlowOptionCtrl via Content Management Window
                            case 62: // NGLWorkFlowSectionCtrl; Widget for Workflow data section associated with a Workflow switch or a workdflow group
                            case 63: // NGLPopupWindCtrl; Widget container for displaying nested NGL Ctrls via popup window
                                break; //false
                            default: //All others can be contained inside this container
                                blnRet = true;
                                break;
                        }
                        break;
                }
                break;
            case 12: //Filter Selection
                switch (childSubTypeControl) {
                    case 0: //Page  
                    case 1: //Grid  
                    case 2: //SpreadSheet 
                    case 4: //TreeList    
                    case 5: //'ScrollView'
                    case 23: //Fast Tab Action
                    case 24: //Editor Custom Tool
                    case 25: //Editor Standard Tools
                    case 26: //Standard Border
                    case 27: //Full Page Border
                    case 40: //Page Footer
                    case 42: //K-Icon
                    case 45: // Full Page Footer; Footer typically nested inside of a Full Page Border
                    case 50: // NGLEditWindCtrl; Widget for Edit Add and Delete via Content Management Popup Window
                    case 51: // NGLAddDependentCtrl; Widget for Adding Dependent Records via Content Management Popup Window
                    case 52: // NGLSummaryDataCtrl; Widget to display read only summary data on a page using Content Management
                    case 53: // NGLEditOnPageCtrl; Widget for Adding and Editing Records on a page using Content Management
                    case 55: // FloatLeftTable; Multi-Column Floating Table
                    case 56: // TableHeader; Multi-Column Table Headers, determines number of columns
                    case 58: // NGLWorkFlowOptionCtrl; Widget for Workflow option selections via Content Management Window
                    case 59: // NGLWorkFlowGroup; Group ul container for  NGLWorkFlow...Switches
                    case 60: // NGLWorkFlowOnOffSwitch; On Off Switch for NGLWorkFlowOptionCtrl via Content Management Window
                    case 61: // NGLWorkFlowYesNoSwitch; Yes No Switch for NGLWorkFlowOptionCtrl via Content Management Window
                    case 62: // NGLWorkFlowSectionCtrl; Widget for Workflow data section associated with a Workflow switch or a workdflow group
                    case 63: // NGLPopupWindCtrl; Widget container for displaying nested NGL Ctrls via popup window
                        break; //False
                    default: //All others can be contained inside this container
                        blnRet = true;
                        break;
                }
                break;
            case 13://Tab strips for grid

                switch (this.GroupSubTypeControl) {
                    case 47: //Data grid tool bar using cm template control
                    case 48: //Default Kendo Excel and PDF export tools for grid
                        blnRet = false;
                        break;
                    default:
                        switch (childSubTypeControl) {
                            case 0: //page
                            case 23: //Fast Tab Action
                            case 24: //Editor Custom Tool
                            case 25: //Editor Standard Tools
                            case 26: //Standard Border
                            case 27: //Full Page Border
                            case 40: //Page Footer
                            case 42: //K-Icon
                            case 45: //Full Page Footer
                                break; //false
                            default: //All others can be contained inside this container
                                blnRet = true;
                                break;
                        }
                        break;
                }
                break;
            case 14: //Content for HTML Containers like Div tags
                switch (this.GroupSubTypeControl) {
                    case 29: //Two Column Data
                        switch (childSubTypeControl) {
                            case 0: //Page 
                            case 1: //Grid 
                            case 2: //SpreadSheet  
                            case 3: //ListView  
                            case 4: //TreeList  
                            case 5: //'ScrollView'
                            case 19: //Grid Fast Tab
                            case 22: //Form Fast Tab
                            case 23: //Fast Tab Action
                            case 24: //Editor Custom Tool
                            case 25: //Editor Standard Tools
                            case 26: //Standard Border
                            case 27: //Full Page Border
                            case 28: //Float Block Left
                            case 29: //Two Column Data
                            case 30: //HR Line Tag
                            case 31: //Header 1
                            case 32: //Header 2
                            case 33: //Header 3
                            case 34: //Header 4
                            case 35: //Div
                            case 36: //Span
                            case 37: //Paragraph
                            case 40: //Page Footer
                            case 42: //K-Icon
                            case 43: //HTML Space
                            case 44: //HTML Line Break
                            case 45: // Full Page Footer; Footer typically nested inside of a Full Page Border
                            case 50: // NGLEditWindCtrl; Widget for Edit Add and Delete via Content Management Popup Window
                            case 51: // NGLAddDependentCtrl; Widget for Adding Dependent Records via Content Management Popup Window
                            case 52: // NGLSummaryDataCtrl; Widget to display read only summary data on a page using Content Management
                            case 53: // NGLEditOnPageCtrl; Widget for Adding and Editing Records on a page using Content Management
                            case 55: // FloatLeftTable; Multi-Column Floating Table
                            case 56: // TableHeader; Multi-Column Table Headers, determines number of columns
                            case 58: // NGLWorkFlowOptionCtrl; Widget for Workflow option selections via Content Management Window
                            case 59: // NGLWorkFlowGroup; Group ul container for  NGLWorkFlow...Switches
                            case 60: // NGLWorkFlowOnOffSwitch; On Off Switch for NGLWorkFlowOptionCtrl via Content Management Window
                            case 61: // NGLWorkFlowYesNoSwitch; Yes No Switch for NGLWorkFlowOptionCtrl via Content Management Window
                            case 62: // NGLWorkFlowSectionCtrl; Widget for Workflow data section associated with a Workflow switch or a workdflow group
                            case 63: // NGLPopupWindCtrl; Widget container for displaying nested NGL Ctrls via popup window
                                break; //false
                            default: //All others can be contained inside this container
                                blnRet = true;
                                break;
                        }
                        break;
                    case 30: //HR Line Tag
                        break;
                    case 31: //Header 1
                        switch (childSubTypeControl) {
                            case 0: //Page 
                            case 1: //Grid   
                            case 2: //SpreadSheet   
                            case 3: //ListView      
                            case 4: //TreeList    
                            case 5: //'ScrollView'
                            case 6: //'AutoComplete'
                            case 7: //'ColorPicker'
                            case 8: //'ComboBox'
                            case 9: //'DatePicker'
                            case 10: //'DateTimePicker'
                            case 11: //'DropDownList'
                            case 12: //'Editor'
                            case 13: //'MaskedTextBox'
                            case 14: //'MultiSelect'
                            case 15: //'NumericTextBox'
                            case 16: //'kendoSwitch'
                            case 18: //'Tab Strip'
                            case 19: //Grid Fast Tab
                            case 20: //Filter Selection
                            case 22: //Form Fast Tab
                            case 23: //Fast Tab Action
                            case 24: //Editor Custom Tool
                            case 25: //Editor Standard Tools
                            case 26: //Standard Border
                            case 27: //Full Page Border
                            case 28: //Float Block Left
                            case 29: //Two Column Data
                            case 30: //HR Line Tag
                            case 35: //Div
                            case 40: //Page Footer
                            case 42: //K-Icon
                            case 44: //HTML Line Break
                            case 45: //Full Page Footer
                            case 49: //TimePicker
                            case 50: // NGLEditWindCtrl; Widget for Edit Add and Delete via Content Management Popup Window
                            case 51: // NGLAddDependentCtrl; Widget for Adding Dependent Records via Content Management Popup Window
                            case 52: // NGLSummaryDataCtrl; Widget to display read only summary data on a page using Content Management
                            case 53: // NGLEditOnPageCtrl; Widget for Adding and Editing Records on a page using Content Management
                            case 54: // Checkbox; HTML Input Checkbox
                            case 55: // FloatLeftTable; Multi-Column Floating Table
                            case 56: // TableHeader; Multi-Column Table Headers, determines number of columns
                            case 58: // NGLWorkFlowOptionCtrl; Widget for Workflow option selections via Content Management Window
                            case 59: // NGLWorkFlowGroup; Group ul container for  NGLWorkFlow...Switches
                            case 60: // NGLWorkFlowOnOffSwitch; On Off Switch for NGLWorkFlowOptionCtrl via Content Management Window
                            case 61: // NGLWorkFlowYesNoSwitch; Yes No Switch for NGLWorkFlowOptionCtrl via Content Management Window
                            case 62: // NGLWorkFlowSectionCtrl; Widget for Workflow data section associated with a Workflow switch or a workdflow group
                            case 63: // NGLPopupWindCtrl; Widget container for displaying nested NGL Ctrls via popup window 
                                break; //false
                            default: //All others can be contained inside this container
                                blnRet = true;
                                break;
                        }
                        break;
                    case 32: //Header 2
                        switch (childSubTypeControl) {
                            case 0: //Page 
                            case 1: //Grid   
                            case 2: //SpreadSheet   
                            case 3: //ListView      
                            case 4: //TreeList    
                            case 5: //'ScrollView'
                            case 6: //'AutoComplete'
                            case 7: //'ColorPicker'
                            case 8: //'ComboBox'
                            case 9: //'DatePicker'
                            case 10: //'DateTimePicker'
                            case 11: //'DropDownList'
                            case 12: //'Editor'
                            case 13: //'MaskedTextBox'
                            case 14: //'MultiSelect'
                            case 15: //'NumericTextBox'
                            case 16: //'kendoSwitch'
                            case 18: //'Tab Strip'
                            case 19: //Grid Fast Tab
                            case 20: //Filter Selection
                            case 22: //Form Fast Tab
                            case 23: //Fast Tab Action
                            case 24: //Editor Custom Tool
                            case 25: //Editor Standard Tools
                            case 26: //Standard Border
                            case 27: //Full Page Border
                            case 28: //Float Block Left
                            case 29: //Two Column Data
                            case 30: //HR Line Tag
                            case 35: //Div
                            case 40: //Page Footer
                            case 42: //K-Icon
                            case 44: //HTML Line Break
                            case 45: //Full Page Footer
                            case 49: //TimePicker
                            case 50: // NGLEditWindCtrl; Widget for Edit Add and Delete via Content Management Popup Window
                            case 51: // NGLAddDependentCtrl; Widget for Adding Dependent Records via Content Management Popup Window
                            case 52: // NGLSummaryDataCtrl; Widget to display read only summary data on a page using Content Management
                            case 53: // NGLEditOnPageCtrl; Widget for Adding and Editing Records on a page using Content Management
                            case 54: // Checkbox; HTML Input Checkbox
                            case 55: // FloatLeftTable; Multi-Column Floating Table
                            case 56: // TableHeader; Multi-Column Table Headers, determines number of columns 
                            case 58: // NGLWorkFlowOptionCtrl; Widget for Workflow option selections via Content Management Window
                            case 59: // NGLWorkFlowGroup; Group ul container for  NGLWorkFlow...Switches
                            case 60: // NGLWorkFlowOnOffSwitch; On Off Switch for NGLWorkFlowOptionCtrl via Content Management Window
                            case 61: // NGLWorkFlowYesNoSwitch; Yes No Switch for NGLWorkFlowOptionCtrl via Content Management Window
                            case 62: // NGLWorkFlowSectionCtrl; Widget for Workflow data section associated with a Workflow switch or a workdflow group
                            case 63: // NGLPopupWindCtrl; Widget container for displaying nested NGL Ctrls via popup window
                                break; //false
                            default: //All others can be contained inside this container
                                blnRet = true;
                                break;
                        }
                        break;
                    case 33: //Header 3
                        switch (childSubTypeControl) {
                            case 0: //Page 
                            case 1: //Grid   
                            case 2: //SpreadSheet   
                            case 3: //ListView      
                            case 4: //TreeList    
                            case 5: //'ScrollView'
                            case 6: //'AutoComplete'
                            case 7: //'ColorPicker'
                            case 8: //'ComboBox'
                            case 9: //'DatePicker'
                            case 10: //'DateTimePicker'
                            case 11: //'DropDownList'
                            case 12: //'Editor'
                            case 13: //'MaskedTextBox'
                            case 14: //'MultiSelect'
                            case 15: //'NumericTextBox'
                            case 16: //'kendoSwitch'
                            case 18: //'Tab Strip'
                            case 19: //Grid Fast Tab
                            case 20: //Filter Selection
                            case 22: //Form Fast Tab
                            case 23: //Fast Tab Action
                            case 24: //Editor Custom Tool
                            case 25: //Editor Standard Tools
                            case 26: //Standard Border
                            case 27: //Full Page Border
                            case 28: //Float Block Left
                            case 29: //Two Column Data
                            case 30: //HR Line Tag
                            case 35: //Div
                            case 40: //Page Footer
                            case 42: //K-Icon
                            case 44: //HTML Line Break
                            case 45: //Full Page Footer
                            case 49: //TimePicker
                            case 50: // NGLEditWindCtrl; Widget for Edit Add and Delete via Content Management Popup Window
                            case 51: // NGLAddDependentCtrl; Widget for Adding Dependent Records via Content Management Popup Window
                            case 52: // NGLSummaryDataCtrl; Widget to display read only summary data on a page using Content Management
                            case 53: // NGLEditOnPageCtrl; Widget for Adding and Editing Records on a page using Content Management
                            case 54: // Checkbox; HTML Input Checkbox
                            case 55: // FloatLeftTable; Multi-Column Floating Table
                            case 56: // TableHeader; Multi-Column Table Headers, determines number of columns 
                            case 58: // NGLWorkFlowOptionCtrl; Widget for Workflow option selections via Content Management Window
                            case 59: // NGLWorkFlowGroup; Group ul container for  NGLWorkFlow...Switches
                            case 60: // NGLWorkFlowOnOffSwitch; On Off Switch for NGLWorkFlowOptionCtrl via Content Management Window
                            case 61: // NGLWorkFlowYesNoSwitch; Yes No Switch for NGLWorkFlowOptionCtrl via Content Management Window
                            case 62: // NGLWorkFlowSectionCtrl; Widget for Workflow data section associated with a Workflow switch or a workdflow group
                            case 63: // NGLPopupWindCtrl; Widget container for displaying nested NGL Ctrls via popup window
                                break; //false
                            default: //All others can be contained inside this container
                                blnRet = true;
                                break;
                        }
                        break;
                    case 34: //Header 4
                        switch (childSubTypeControl) {
                            case 0: //Page 
                            case 1: //Grid   
                            case 2: //SpreadSheet   
                            case 3: //ListView      
                            case 4: //TreeList    
                            case 5: //'ScrollView'
                            case 6: //'AutoComplete'
                            case 7: //'ColorPicker'
                            case 8: //'ComboBox'
                            case 9: //'DatePicker'
                            case 10: //'DateTimePicker'
                            case 11: //'DropDownList'
                            case 12: //'Editor'
                            case 13: //'MaskedTextBox'
                            case 14: //'MultiSelect'
                            case 15: //'NumericTextBox'
                            case 16: //'kendoSwitch'
                            case 18: //'Tab Strip'
                            case 19: //Grid Fast Tab
                            case 20: //Filter Selection
                            case 22: //Form Fast Tab
                            case 23: //Fast Tab Action
                            case 24: //Editor Custom Tool
                            case 25: //Editor Standard Tools
                            case 26: //Standard Border
                            case 27: //Full Page Border
                            case 28: //Float Block Left
                            case 29: //Two Column Data
                            case 30: //HR Line Tag
                            case 35: //Div
                            case 40: //Page Footer
                            case 42: //K-Icon
                            case 44: //HTML Line Break
                            case 45: //Full Page Footer
                            case 49: //TimePicker
                            case 50: // NGLEditWindCtrl; Widget for Edit Add and Delete via Content Management Popup Window
                            case 51: // NGLAddDependentCtrl; Widget for Adding Dependent Records via Content Management Popup Window
                            case 52: // NGLSummaryDataCtrl; Widget to display read only summary data on a page using Content Management
                            case 53: // NGLEditOnPageCtrl; Widget for Adding and Editing Records on a page using Content Management
                            case 54: // Checkbox; HTML Input Checkbox
                            case 55: // FloatLeftTable; Multi-Column Floating Table
                            case 56: // TableHeader; Multi-Column Table Headers, determines number of columns 
                            case 58: // NGLWorkFlowOptionCtrl; Widget for Workflow option selections via Content Management Window
                            case 59: // NGLWorkFlowGroup; Group ul container for  NGLWorkFlow...Switches
                            case 60: // NGLWorkFlowOnOffSwitch; On Off Switch for NGLWorkFlowOptionCtrl via Content Management Window
                            case 61: // NGLWorkFlowYesNoSwitch; Yes No Switch for NGLWorkFlowOptionCtrl via Content Management Window
                            case 62: // NGLWorkFlowSectionCtrl; Widget for Workflow data section associated with a Workflow switch or a workdflow group
                            case 63: // NGLPopupWindCtrl; Widget container for displaying nested NGL Ctrls via popup window
                                break; //false
                            default: //All others can be contained inside this container
                                blnRet = true;
                                break;
                        }
                        break;
                    case 35: //Div
                        switch (childSubTypeControl) {
                            case 0: //Page
                            case 23: //Fast Tab Action
                            case 24: //Editor Custom Tool
                            case 25: //Editor Standard Tools
                            case 26: //Standard Border
                            case 27: //Full Page Border
                            case 40: //Page Footer
                            case 42: //K-Icon
                            case 45: // Full Page Footer; Footer typically nested inside of a Full Page Border
                            case 50: // NGLEditWindCtrl; Widget for Edit Add and Delete via Content Management Popup Window
                            case 51: // NGLAddDependentCtrl; Widget for Adding Dependent Records via Content Management Popup Window
                            case 55: // FloatLeftTable; Multi-Column Floating Table
                            case 56: // TableHeader; Multi-Column Table Headers, determines number of columns 
                                break; //false
                            default: //All others can be contained inside this container
                                blnRet = true;
                                break;
                        }
                        break;
                    case 36: //Span
                        switch (childSubTypeControl) {
                            case 0: //Page  
                            case 1: //Grid   
                            case 2: //SpreadSheet  
                            case 3: //ListView   
                            case 4: //TreeList   
                            case 5: //'ScrollView'
                            case 18: //'Tab Strip'
                            case 19: //Grid Fast Tab
                            case 20: //Filter Selection
                            case 22: //Form Fast Tab
                            case 23: //Fast Tab Action
                            case 24: //Editor Custom Tool
                            case 25: //Editor Standard Tools
                            case 26: //Standard Border
                            case 27: //Full Page Border
                            case 28: //Float Block Left
                            case 29: //Two Column Data
                            case 30: //HR Line Tag
                            case 35: //Div
                            case 40: //Page Footer
                            case 42: //K-Icon
                            case 45: //Full Page Footer
                            case 50: // NGLEditWindCtrl; Widget for Edit Add and Delete via Content Management Popup Window
                            case 51: // NGLAddDependentCtrl; Widget for Adding Dependent Records via Content Management Popup Window
                            case 52: // NGLSummaryDataCtrl; Widget to display read only summary data on a page using Content Management
                            case 53: // NGLEditOnPageCtrl; Widget for Adding and Editing Records on a page using Content Management
                            case 55: // FloatLeftTable; Multi-Column Floating Table
                            case 56: // TableHeader; Multi-Column Table Headers, determines number of columns
                            case 58: // NGLWorkFlowOptionCtrl; Widget for Workflow option selections via Content Management Window
                            case 63: // NGLPopupWindCtrl; Widget container for displaying nested NGL Ctrls via popup window
                                break; //false
                            default: //All others can be contained inside this container
                                blnRet = true;
                                break;
                        }
                        break;
                    case 37: //Paragraph
                        switch (childSubTypeControl) {
                            case 0: //Page 
                            case 1: //Grid   
                            case 2: //SpreadSheet  
                            case 3: //ListView    
                            case 4: //TreeList  
                            case 5: //'ScrollView'
                            case 18: //'Tab Strip'
                            case 19: //Grid Fast Tab
                            case 20: //Filter Selection
                            case 22: //Form Fast Tab
                            case 23: //Fast Tab Action
                            case 24: //Editor Custom Tool
                            case 25: //Editor Standard Tools
                            case 26: //Standard Border
                            case 27: //Full Page Border
                            case 28: //Float Block Left
                            case 29: //Two Column Data
                            case 30: //HR Line Tag
                            case 35: //Div
                            case 40: //Page Footer
                            case 42: //K-Icon
                            case 45: //Full Page Footer
                            case 50: // NGLEditWindCtrl; Widget for Edit Add and Delete via Content Management Popup Window
                            case 51: // NGLAddDependentCtrl; Widget for Adding Dependent Records via Content Management Popup Window
                            case 52: // NGLSummaryDataCtrl; Widget to display read only summary data on a page using Content Management
                            case 53: // NGLEditOnPageCtrl; Widget for Adding and Editing Records on a page using Content Management
                            case 55: // FloatLeftTable; Multi-Column Floating Table
                            case 56: // TableHeader; Multi-Column Table Headers, determines number of columns
                            case 58: // NGLWorkFlowOptionCtrl; Widget for Workflow option selections via Content Management Window
                            case 59: // NGLWorkFlowGroup; Group ul container for  NGLWorkFlow...Switches
                            case 60: // NGLWorkFlowOnOffSwitch; On Off Switch for NGLWorkFlowOptionCtrl via Content Management Window
                            case 61: // NGLWorkFlowYesNoSwitch; Yes No Switch for NGLWorkFlowOptionCtrl via Content Management Window
                            case 62: // NGLWorkFlowSectionCtrl; Widget for Workflow data section associated with a Workflow switch or a workdflow group
                            case 63: // NGLPopupWindCtrl; Widget container for displaying nested NGL Ctrls via popup window
                                break; //false
                            default: //All others can be contained inside this container
                                blnRet = true;
                                break;
                        }
                        break;
                    case 38: //Standard img tag ref in metadata
                        break;
                    case 39: //Link
                        switch (childSubTypeControl) {
                            case 0: //Page 
                            case 1: //Grid 
                            case 2: //SpreadSheet 
                            case 3: //ListView    
                            case 4: //TreeList    
                            case 5: //'ScrollView'
                            case 18: //'Tab Strip'
                            case 19: //Grid Fast Tab
                            case 20: //Filter Selection
                            case 22: //Form Fast Tab
                            case 23: //Fast Tab Action
                            case 24: //Editor Custom Tool
                            case 25: //Editor Standard Tools
                            case 26: //Standard Border
                            case 27: //Full Page Border
                            case 28: //Float Block Left
                            case 29: //Two Column Data
                            case 30: //HR Line Tag
                            case 35: //Div
                            case 40: //Page Footer
                            case 42: //K-Icon
                            case 45: //Full Page Footer
                            case 50: // NGLEditWindCtrl; Widget for Edit Add and Delete via Content Management Popup Window
                            case 51: // NGLAddDependentCtrl; Widget for Adding Dependent Records via Content Management Popup Window
                            case 52: // NGLSummaryDataCtrl; Widget to display read only summary data on a page using Content Management
                            case 53: // NGLEditOnPageCtrl; Widget for Adding and Editing Records on a page using Content Management
                            case 55: // FloatLeftTable; Multi-Column Floating Table
                            case 56: // TableHeader; Multi-Column Table Headers, determines number of columns
                            case 58: // NGLWorkFlowOptionCtrl; Widget for Workflow option selections via Content Management Window
                            case 59: // NGLWorkFlowGroup; Group ul container for  NGLWorkFlow...Switches
                            case 60: // NGLWorkFlowOnOffSwitch; On Off Switch for NGLWorkFlowOptionCtrl via Content Management Window
                            case 61: // NGLWorkFlowYesNoSwitch; Yes No Switch for NGLWorkFlowOptionCtrl via Content Management Window
                            case 62: // NGLWorkFlowSectionCtrl; Widget for Workflow data section associated with a Workflow switch or a workdflow group
                            case 63: // NGLPopupWindCtrl; Widget container for displaying nested NGL Ctrls via popup window
                                break; //false
                            default: //All others can be contained inside this container
                                blnRet = true;
                                break;
                        }
                        break;
                    case 40: //Page Footer
                        break;
                    case 41: //Kendo Icon Button
                        if (childSubTypeControl == 42) blnRet = true;
                        break;
                    case 42: //Kendo Icon Span image in MetaData
                        break;
                    case 43: //Standard HTML Space &nbsp; 
                        break;
                    case 44: //Standard HTML line break <br /> 
                        break;
                    case 45: //Standard HTML line break <br /> 
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
        return blnRet;
    }

    this.getMetaDataLabel = function () {
        var sRet = "Meta Data";
        switch (this.GroupSubTypeControl) {
            case 6: //AutoComplete
            case 54: //Checkbox
            case 7: //ColorPicker
            case 8: //ComboBox
            case 9: //DatePicker
            case 10: //DateTimePicker
            case 11: //DropDownList
            case 12: //Editor
            case 24: //Editor Custom Tool
            case 25: //Editor Standard Tools
            case 16: //kendoSwitch
            case 13: //MaskedTextBox
            case 14: //MultiSelect
            case 59: //NGLWorkFlowGroup
            case 60: //NGLWorkFlowOnOffSwitch
            case 61: //NGLWorkFlowYesNoSwitch
            case 15: //NumericTextBox
            case 49: //TimePicker
                sRet = "Enter Default Value"
                break;
            case 17: //Button
                sRet = "Enter On Click Function"
                break;
            case 24: //Editor Toolbar Custom Tool
                sRet = "Enter On Click Function"
                break;
            case 25: //Included Editor Toolbar Tools
                sRet = "Included Editor Toolbar Tools"
                break;
            case 28: //Float Block Left
                sRet = "Enter Additional Style Tags"
                break;
            case 29: //Two Column Data
                sRet = "Enter Additional Style Tags"
                break;
            case 31: //Header 1
                sRet = "Enter Additional Style Tags"
                break;
            case 32: //Header 2
                sRet = "Enter Additional Style Tags"
                break;
            case 33: //Header 3
                sRet = "Enter Additional Style Tags"
                break;
            case 34: //Header 4
                sRet = "Enter Additional Style Tags"
                break;
            case 35: //Div
                sRet = "Enter Additional Style Tags"
                break;
            case 36: //Span
                sRet = "Enter Additional Style Tags"
                break;
            case 37: //Paragraph
                sRet = "Enter Page Text"
                break;
            case 38: //Images
                sRet = "Enter Image Source Path"
                break;
            case 39: //Link
                sRet = "Enter Link Path"
                break;
            case 41: //Kendo Icon Button
                sRet = "Enter On Click Function"
                break;
            case 42: //K-Icon
                sRet = "Enter Icon Image Class"
                break;
            case 50: //NGLEditWindCtrl
                sRet = "CRUD for Create Read Update or Delete"
                break;
            case 65: //Button
                sRet = "Enter On Click Function"
                break;
            default:
                break;
        }
        return sRet;
    }

    this.getDefaultMetaData = function () {
        var sRet = "";
        switch (this.GroupSubTypeControl) {
            case 25: //Included Editor Toolbar Tools
                sRet = "bold|italic|underline|strikethrough|subscript|superscript|fontName|fontSize|foreColor|backColor|justifyLeft|justifyCenter|justifyRight|justifyFull|insertUnorderedList|insertOrderedList|indent|outdent|createLink|unlink|insertImage|insertFile|createTable|addColumnLeft|addColumnRight|addRowAbove|addRowBelow|deleteRow|deleteColumn|formatting|print|pdf";
                break;
            default:
                break;
        }
        return sRet;
    }

    this.loadDefaults = function (TypeControl, GroupTypeControl, Name, Desc) {
        this.GroupSubTypeControl = TypeControl;
        this.GroupSubTypeGroupTypeControl = GroupTypeControl;
        this.GroupSubTypeName = Name;
        this.GroupSubTypeDesc = Desc;
    }
}

//cmPageDetail Model
function cmPageDetail() {
    PageDetControl: 0;
    PageDetPageControl: 0;
    PageDetGroupTypeControl: 0;
    PageDetGroupSubTypeControl: 0;
    PageDetName: "";
    PageDetDesc: "";
    PageDetCaption: "";
    PageDetCaptionLocal: "";
    PageDetSequenceNo: 0;
    PageDetParentID: 0;
    PageDetOrientation: 0;
    PageDetWidth: 0;
    PageDetAllowFilter: true;
    PageDetFilterTypeControl: 0;
    PageDetAllowSort: true;
    PageDetAllowPaging: true;
    PageDetUserSecurityControl: 0;
    PageDetVisible: true;
    PageDetReadOnly: false;
    PageDetDataElmtControl: 0;
    PageDetElmtFieldControl: 0;
    PageDetPageTemplateControl: 0;
    PageDetExpanded: true;
    PageDetMetaData: "";
    PageDetFKReference: "";
    PageDetTagIDReference: "";
    PageDetCSSClass: "";
    PageDetAttributes: "";
    PageDetAPIReference: "";
    PageDetAPIFilterID: "";
    PageDetAPISortKey: "";
    PageDetRequired: false;
    PageDetInsertOnly: false;
    PageDetFieldFormatOverride: "";
    PageDetWidgetAction: "";
    PageDetWidgetActionKey: "";
    PageDetEditWndVisibility: 0; //Added by LVV on 7/26/19
    PageDetEditWndSeqNo: 0;      //Added by LVV on 7/26/19
    PageDetAddWndVisibility: 0; //Added by LVV on 7/26/19
    PageDetAddWndSeqNo: 0;	    //Added by LVV on 7/26/19
    PageDetModDate: "";
    PageDetModUser: "";
    PageDetUpdated: "";
    Pages: 0;
    RecordCount: 0;
    Page: 0;
    PageSize: 0;
    ROW_NUMBER: 0;

    this.addKeys = function (Control, Control, GroupTypeControl, GroupSubTypeControl) {
        this.PageDetControl = Control;
        this.PageDetPageControl = PageControl;
        this.PageDetGroupTypeControl = GroupTypeControl;
        this.PageDetGroupSubTypeControl = GroupSubTypeControl;
    }

    this.addDetails = function (Name, Desc, Caption, CaptionLocal, SequenceNo, Width, Visible, MetaData) {
        this.PageDetName = Name;
        this.PageDetDesc = Desc;
        this.PageDetCaption = Caption;
        this.PageDetCaptionLocal = CaptionLocal;
        this.PageDetSequenceNo = SequenceNo;
        this.PageDetWidth = Width;
        this.PageDetVisible = Visible;
        this.PageDetMetaData = MetaData;
    }

    this.loadDefaults = function (Control, Control, GroupTypeControl, GroupSubTypeControl, Name, Desc, Caption, CaptionLocal, SequenceNo, Width, Visible, MetaData) {
        this.PageDetControl = Control;
        this.PageDetPageControl = PageControl;
        this.PageDetGroupTypeControl = GroupTypeControl;
        this.PageDetGroupSubTypeControl = GroupSubTypeControl;
        this.PageDetName = Name;
        this.PageDetDesc = Desc;
        this.PageDetCaption = Caption;
        this.PageDetCaptionLocal = CaptionLocal;
        this.PageDetSequenceNo = SequenceNo;
        this.PageDetWidth = Width;
        this.PageDetVisible = Visible;
        this.PageDetMetaData = MetaData;
    }

}

//createPageDetailFromFieldFilters Model
function createPageDetailFromFieldFilters() {
    PageDetPageControl: 0;
    PageDetParentID: 0;
    PageDetElmtFieldControl: 0;
}


//cmPage Model
function cmPage() {
    PageControl: 0;
    PageName: "";
    PageDesc: "";
    PageCaption: "";
    PageCaptionLocal: "";
    PageFormControl: 0;
    PageFormName: "";
    PageFormDesc: "";
    PageDataSource: false;
    PageSortable: false;
    PagePageable: false;
    PageGroupable: false;
    PageEditable: false;
    PageDataElmtControl: 0;
    PageElmtFieldControl: 0;
    PageAutoRefreshSec: 0;
    PageModDate: "";
    PageModUser: "";
    PageUpdated: "";
    Pages: 0;
    RecordCount: 0;
    Page: 0;
    PageSize: 0;
    ROW_NUMBER: 0;


}


//userNewPassword Model
function postNewPassword() {
    this.lastErr = "";
    this.notifyOnError = true;
    this.notifyOnSuccess = true;
    this.notifyOnValidationFailure = true;
    onSave: null;

    this.saveSuccessCallback = function (data) {
        var oResults = new nglEventParameters();
        var tObj = this;
        oResults.source = "saveSuccessCallback";
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed  
        try {
            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                if (data.Errors != null) {
                    oResults.error = new Error();
                    if (data.StatusCode === 203) {
                        oResults.error.name = "Authorization Timeout";
                        oResults.error.message = data.Errors;
                    }
                    else {
                        oResults.error.name = "Access Denied";
                        oResults.error.message = data.Errors;
                    }
                    ngl.Alert("Change Password Failure", oResults.error.message)
                    //    ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                }
                else {
                    oResults.msg = "Success." + "  Your password has been changed."
                    ngl.showSuccessMsg(oResults.msg, tObj);

                }
            } else {
                oResults.msg = "Success." + "  Your password has been changed."
                ngl.showSuccessMsg(oResults.msg, tObj);
            }
        } catch (err) {
            oResults.error = err
        }
        if (ngl.isFunction(this.onSave)) {
            this.onSave(oResults);
        }

    }

    this.saveAjaxErrorCallback = function (xhr, textStatus, error) {
        var oResults = new nglEventParameters();
        var tObj = this;
        oResults.source = "saveAjaxErrorCallback";
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed  
        oResults.error = new Error();
        oResults.error.name = "Save Password Failure"
        oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);

        if (ngl.isFunction(this.onSave)) {
            this.onSave(oResults);
        }
    }

    this.postPassword = function (sNew, sPass) {
        var blnRet = false;
        var sAPI = "SSOA/PostNGLPassword";
        var oChanges = { currentPassword: sPass, newPassword: sNew };
        var tObj = this;
        try {
            var oCRUDCtrl = new nglRESTCRUDCtrl();
            blnRet = oCRUDCtrl.update(sAPI, oChanges, tObj, "saveSuccessCallback", "saveAjaxErrorCallback");

        } catch (err) {
            ngl.showErrMsg("Post New Password Failure", err.message, null);
        }
        return blnRet;
    }

    this.loadDefaults = function (saveCallBack) {
        this.onSave = saveCallBack;
        this.lastErr = "";
        this.notifyOnError = true;
        this.notifyOnSuccess = true;
        this.notifyOnValidationFailure = true;
    }
}

//nglParameter Model
function nglParameter() {
    ParKey: "";
    ParDescription: "";
    ParText: "";
    ParValue: 0;
    ParCategoryControl: 0;
    ParIsGlobal: true;
    ParUpdated: "";

    this.loadDefaults = function (key, desc, text, value, category, isGlobal, updated) {
        this.ParKey = key;
        this.ParDescription = desc;
        this.ParText = text;
        this.ParValue = value;
        this.ParCategoryControl = category;
        this.ParIsGlobal = isGlobal;
        this.ParUpdated = updated;
    }

}

//cmPageDetail Model
function vTenderedOrder() {
    BookControl: 0;
    BookProNumber: "";
    BookConsPrefix: "";
    BookCarrOrderNumber: "";
    PickUpLocation: "";
    BookOrigName: "";
    BookOrigAddress1: "";
    BookOrigAddress2: "";
    BookOrigCity: "";
    BookOrigState: "";
    BookOrigZip: "";
    BookOrigCountry: "";
    DeliveryLocation: "";
    BookDestName: "";
    BookDestAddress1: "";
    BookDestAddress2: "";
    BookDestCity: "";
    BookDestState: "";
    BookDestZip: "";
    BookDestCountry: "";
    Quantity: "";
    BookTotalCases: 0;
    BookTotalWgt: 0;
    BookTotalPl: 0;
    BookTotalCube: 0;
    BookDateLoad: "";
    BookDateRequired: "";
    BookRevTotalCost: 0;
    BookTrackDate: "";
    BookNotesVisable1: "";
    BookNotesVisable2: "";
    BookNotesVisable3: "";
    BookStopNo: 0;
    BookCarrierControl: 0;
    BookCarrierContControl: 0;
    BookSHID: "";
    StatusDate: "";
    AllowedMinutes: 0;
    Status: "";
    ExpiresOn: "";
}


function vLookupListCriteria() {
    id: 0; // maps to enum GlobalDynamicLists current values are CarrierEquipCode = 0, CarrierEquipment = 1, vLookupERPSettings = 2, tblUserSecurity = 3, cmDataElement = 4, cmElementField = 5
    sortKey: 0; //maps to enum ListSortType current values are Control = 0,    Name = 1,     DescOrNbr = 2
    Criteria: null; //object used as a filter like key contraint typically a control number from the parent object
}

function vLookupList() {
    Control: 0; //typically maps to Databound key identifier
    Name: ""; //typically maps to the text displayed in the list
    Description: ""; //typically maps to a description or alternate key value
}


function AcceptorReject() {
    BookControl: 0;
    CarrierControl: 0; //part of the model but populated in the controller
    CarrierContControl: 0; //part of the model but populated in the controller
    AcceptRejectCode: 0; //Accepted = 0,1 = Rejected,2 = Expired,3 =  Unfinalize,4 = Tendered,5 = Dropped,6 = Unassigned,7 = ModifyUnaccepted
    SendEmail: false; //part of the model but populated in the controller
    BookTrackComment: '';
    BookTrackStatus: 0; //part of the model but populated in the controller
    NotificationEMailAddress: ''; //part of the model but populated in the controller
    NotificationEMailAddressCc: ''; //part of the model but populated in the controller
    AcceptRejectMode: 1; //part of the model but populated in the controller  possible values 0=Manual, 1 = EDI, 2 = web, 3 = system, 4 = token, 5 = DAT
    UserName: ''; //part of the model but populated in the controller
}


function vNGLAPIPalletTypes() {
    ID: 0;
    PalletType: "";
    PalletTypeDescription: "";
}

//Begin NGL Simple Widgets 
//*********************************************************************


//Build databound cascading combo boxes that update and refresh data based on the configured parent child relationship
//Inherits from the kendoComboBox 
//Must use standard vLookup model for the data source
function nglCascadingCombo() {

    this.lastErr = "";
    this.notifyOnError = true;
    this.notifyOnSuccess = true;
    this.notifyOnValidationFailure = true;
    this.DataCombo = null; // reference to this objects combo DOM object
    this.ChildCombos = new Array();  //an array of nglCascadingCombo boxes allows for nested children.
    this.KeyIndex = 0;   //idnex in the parent objects nglCascadingCombo(ChildCombos) array 
    this.KeyName = "";  //unique name of this object typcally the DOM id
    this.APIControler = "";  //we always use 'api/vLookupList/' this is the rest service method like 'GetGlobalDynamicList'
    this.APIFilterID = 0;  //we always use 'api/vLookupList/'  this is the id for the enumerator used by the APIControler method like 4 for cmDataElement 
    this.SortKey = 1; //reference to sorting enum.  values are Control = 0,  Name = 1,   DescOrNbr = 2
    this.ChildKeyIndex = 0; //reference to FK used to populate ChildCombos ParentControlNbr.  values are Control = 0,  Name = 1,   DescOrNbr = 2
    this.ControlNbr = "0";
    this.ParentControlNbr = "0"; //alpha numeric control value for FK lookup
    this.DataType = "vLookupList";
    this.DOMHiddenField = "";  //the actual DOM id (not a reference to the object) of the hidden field on the page that holds the current selected or database value
    this.data = new vLookupList();
    this.rData = null;
    this.onSelect = null;
    this.onChildSelect = null;

    this.updateHiddenKeyField = function (iSelected) {
        var oPageElement = $("#" + this.DOMHiddenField);
        if (typeof (oPageElement) !== 'undefined') {
            oPageElement.val(iSelected);
        }
        //check if we have Child Combos and clear all of the dependent value
        //users must select a new value 
        if (ngl.isArray(this.ChildCombos) && this.ChildCombos.length > 0) {
            var cLen = this.ChildCombos.length;
            for (i = 0; i < cLen; i++) {
                var cCombo = this.ChildCombos[i];
                var kcb = cCombo.DataCombo.data('kendoComboBox');
                if (typeof (kcb) !== 'undefined') {
                    //clear the data
                    kcb.setDataSource();
                    kcb.text("");
                    kcb.value("");
                }

                //Removed by RHR we do not want to clear the current value of the hidden key because this will prevent the default value from loading in the combo box.
                // ** clear the data bound key field 
                // ** var cPageElement = $("#" + cCombo.DOMHiddenField);
                // ** if (typeof (cPageElement) !== 'undefined') {
                // **     cPageElement.val(0);
                // ** }
            }
        }
    }

    //on Child Select call back function The ItemSelected Method will call the onSelected callback; so when we create each child combo we set the onSelected callback to this instance of the  ChildItemSelected function which in turn call the parent objects onChildSelected event callback.
    //par must be passed as an nglEventParameters object
    //par.data holds a referece to the Kendo e item
    //par.datatype must equal the ChildCombos datatype stored in the nglCascadingCombo.DataType property
    //par.keyindex reference the KeyIndex property of the child nglCascadingCombo and is the index in the child combo array.
    //par.keyname refrernces the KeyName property the child nglCascadingCombo and will be compared with the instance stored in the ChildCombos array for the provided index
    this.ChildItemSelected = function (par) {
        //TODO:  add any special event handlers here
        //for now  we just bubble up this event to the caller.  the container of the widget should handle any processing of the child item selected event
        if (ngl.isFunction(this.onChildSelect)) {
            this.onChildSelect(par);
        }
    }

    this.ItemSelected = function (e) {
        var oResults = new nglEventParameters();
        oResults.source = "ItemSelected";
        var tObj = this;
        oResults.widget = tObj;
        oResults.msg = "Item Selected";
        try {
            //clear the control number'
            this.ControlNbr = 0
            if (typeof (e.item) !== 'undefined' && ngl.isObject(e.item)) {
                var dataitem = this.DataCombo.data('kendoComboBox').dataItem(e.item.index());
                if (typeof (dataitem) !== 'undefined' && ngl.isObject(dataitem)) {
                    //console.log("Item Selected: " + dataitem.Name);
                    if ('Description' in dataitem) { oResults.description = dataitem.Description; }
                    if ('Name' in dataitem) { oResults.text = dataitem.Name; }
                    if ('Control' in dataitem) oResults.value = dataitem.Control;
                    this.ControlNbr = oResults.value;
                    this.updateHiddenKeyField(this.ControlNbr);
                    oResults.msg = oResults.text + " Selected";
                    oResults.data = dataitem;
                    oResults.datatype = this.APIControler + this.APIFilterID.toString();
                    switch (this.ChildKeyIndex) {
                        case 1:
                            this.loadChildCombos(oResults.text);
                            break;
                        case 2:
                            this.loadChildCombos(oResults.description);
                            break;
                        default:
                            this.loadChildCombos(this.ControlNbr);
                    }
                }
            } else {

                oResults.text = this.DataCombo.data('kendoComboBox').text();
                oResults.value = this.DataCombo.data('kendoComboBox').value();
                this.ControlNbr = oResults.value;
                this.updateHiddenKeyField(this.ControlNbr);
                switch (this.ChildKeyIndex) {
                    case 1:
                        this.loadChildCombos(oResults.text);
                        break;
                    case 2:
                        var data = this.DataCombo.data('kendoComboBox').dataSource.data();
                        for (var i = 0; i < data.length; i++) {
                            if (data[i].Control == this.ControlNbr) {
                                this.loadChildCombos(data[i].Description);
                                break;

                            }
                        }
                        break;
                    default:
                        this.loadChildCombos(this.ControlNbr);
                }

                // oResults.msg = "Not in List"
            }


        } catch (err) {
            oResults.error = err
        }
        if (ngl.isFunction(this.onSelect)) {
            this.onSelect(oResults);
        }
    }

    this.loadData = function () {
        var tObj = this;
        if (typeof (this.DataCombo) !== 'undefined' && ngl.isObject(this.DataCombo)) {
            if (this.ParentControlNbr == 0) {
                this.DataCombo.kendoComboBox({
                    dataTextField: "Name",
                    dataValueField: "Control",
                    autoWidth: true,
                    filter: "contains",
                    index: 2,
                    dataSource: {
                        serverFiltering: false,
                        transport: {
                            read: {
                                url: "api/vLookupList/" + this.APIControler + "/" + this.APIFilterID.toString(),
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                complete: function (jqXHR, textStatus) {
                                    if (typeof (jqXHR) !== 'undefined' && jqXHR != null) {
                                        if (typeof (jqXHR.responseJSON) !== 'undefined' && jqXHR.responseJSON != null) {

                                            var data = jqXHR.responseJSON
                                            if (typeof (data.Errors) !== 'undefined' && data.Errors != null) {
                                                ngl.showErrMsg("Read Lookup Data Failure", data.Errors, tObj);
                                            } else {
                                                var blnSet = false;
                                                var combobox = tObj.DataCombo.data("kendoComboBox");
                                                var oHidden = $("#" + tObj.DOMHiddenField);
                                                if (typeof (data.Data) !== 'undefined' && ngl.isObject(data.Data)) {
                                                    if (typeof (oHidden) !== 'undefined' && ngl.isObject(oHidden)) {
                                                        var iSelect = oHidden.val();
                                                        var blnTestForCurrent = (typeof (iSelect) !== 'undefined') && (iSelect.toString() !== "0");
                                                        if (blnTestForCurrent) {
                                                            combobox.value(iSelect);
                                                            blnSet = true;
                                                        }
                                                    }
                                                }
                                                if (blnSet === false) {
                                                    //combobox.select(0)
                                                    combobox.value(0);
                                                }
                                                //combobox.trigger("change");
                                                combobox.trigger("select");
                                            }
                                        }
                                    }
                                }
                            }
                        },
                        change: function (e) {
                            //var dta = e;
                            //console.log("parent change");
                        },
                        schema: {
                            data: "Data",
                            total: "Count",
                            model: {
                                id: "Control",
                                fields: {
                                    Control: { type: "number" },
                                    Name: { type: "string" },
                                    Description: { type: "string" }
                                }
                            },
                            errors: "Errors"
                        },
                    },
                    error: function (xhr, textStatus, error) { ngl.showErrMsg("Load Lookup List Error", formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"), tObj); },
                    select: function (e) {
                        //console.log('parent selected');
                        //console.log('selected index: ' +  e.item.index()); 
                        tObj.ItemSelected(e);
                    }
                });
            } else {
                var ofilter = new vLookupListCriteria();
                ofilter.id = this.APIFilterID;
                ofilter.sortKey = this.SortKey;
                ofilter.Criteria = this.ParentControlNbr.toString();
                this.DataCombo.kendoComboBox({
                    dataTextField: "Name",
                    dataValueField: "Control",
                    autoWidth: true,
                    filter: "contains",
                    index: 2,
                    dataSource: {
                        serverFiltering: false,
                        transport: {
                            read: {
                                url: "api/vLookupList/" + this.APIControler,
                                contentType: "application/json; charset=utf-8",
                                dataType: 'json',
                                data: { filter: JSON.stringify(ofilter) },
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                complete: function (jqXHR, textStatus) {
                                    if (typeof (jqXHR) !== 'undefined' && jqXHR != null) {
                                        if (typeof (jqXHR.responseJSON) !== 'undefined' && jqXHR.responseJSON != null) {
                                            var data = jqXHR.responseJSON
                                            if (typeof (data.Errors) !== 'undefined' && data.Errors != null) {
                                                ngl.showErrMsg("Read Data Failure", data.Errors, tObj);
                                            } else {
                                                var blnSet = false;
                                                var combobox = tObj.DataCombo.data("kendoComboBox");
                                                var oHidden = $("#" + tObj.DOMHiddenField);
                                                if (typeof (data.Data) !== 'undefined' && ngl.isObject(data.Data)) {
                                                    if (typeof (oHidden) !== 'undefined' && ngl.isObject(oHidden)) {
                                                        var iSelect = oHidden.val();
                                                        if (typeof (iSelect) !== 'undefined' && iSelect.toString() !== "0") {
                                                            combobox.value(iSelect)
                                                            blnSet = true;
                                                        }
                                                    }
                                                }
                                                if (blnSet === false) {
                                                    //combobox.select(0)
                                                    combobox.value(0);
                                                }
                                                //combobox.trigger("change");
                                                combobox.trigger("select");
                                            }
                                        }
                                    }
                                }
                            }
                        },
                        change: function (e) {
                            // alert("child change");
                            //if (data.Errors != null) {
                            //    ngl.showErrMsg("Read Child Data Failure", data.Errors);
                            //}
                        },
                        schema: {
                            data: "Data",
                            total: "Count",
                            model: {
                                id: "Control",
                                fields: {
                                    Control: { type: "number" },
                                    Name: { type: "string" },
                                    Description: { type: "string" }
                                }
                            },
                            errors: "Errors"
                        },
                    },
                    error: function (xhr, textStatus, error) {
                        ngl.showErrMsg("Load Child Lookup List Error", formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"), tObj);
                    },
                    select: function (e) { tObj.ItemSelected(e); }
                });
            }


        }
    }

    this.loadChildCombos = function (criteria) {
        var tObj = this;
        if (typeof (criteria) !== 'undefined' && criteria != "0") {
            if (ngl.isArray(this.ChildCombos) && this.ChildCombos.length > 0) {
                var cLen = this.ChildCombos.length;
                for (i = 0; i < cLen; i++) {
                    var cCombo = this.ChildCombos[i];
                    cCombo.ParentControlNbr = criteria;
                    cCombo.loadData();
                }
            }
        }
    }

    this.addChildCombo = function (domId, domHiddenField, apiController, apiFilterID, sortKey) {
        var child = new nglCascadingCombo();
        var tObj = this;
        child.loadDefaults(domId, domHiddenField, apiController, apiFilterID, sortKey, tObj.ChildItemSelected, tObj.ChildItemSelected);
        if (ngl.isArray(this.ChildCombos)) {
            var cLen = this.ChildCombos.length;
            var iIndex = 0;
            switch (cLen) {
                case 0:
                    child.KeyIndex = (iIndex + 1);
                    this.ChildCombos.push(child);
                    break;
                case 1:
                    var oInstance = this.ChildCombos[0];
                    if (typeof (oInstance) === 'undefined' || oInstance === null || (oInstance instanceof nglCascadingCombo) === false) {
                        //just update zero because this is not a valid object
                        child.KeyIndex = 0;
                        this.ChildCombos[0] = child;
                    } else {
                        child.KeyIndex = (iIndex + 1);
                        this.ChildCombos.push(child);
                    }
                    break;
                default:
                    child.KeyIndex = (iIndex + 1);
                    this.ChildCombos.push(child);

            }
        } else {
            child.KeyIndex = 0;
            this.ChildCombos = new Array(child);
        }
        return child;

    }

    //loadDefaults sets up the callbacks cbSelect and cbSave
    //all call backs return a reference to this object and a string message as parameters
    this.loadDefaults = function (domId, domHiddenField, apiController, apiFilterID, sortKey, childKeyIndex, onSelectCallback, onChildSelectCallback) {
        this.KeyIndex = 0;
        this.KeyName = domId;
        this.DataCombo = $("#" + domId);
        this.DataCombo.kendoComboBox();
        this.DOMHiddenField = domHiddenField;
        this.APIControler = apiController;
        this.APIFilterID = apiFilterID;
        this.SortKey = sortKey;
        this.ChildKeyIndex = childKeyIndex
        this.onSelect = onSelectCallback;
        this.onChildSelect = onChildSelectCallback;
        this.DataType = apiController + apiFilterID.toString();
        this.lastErr = "";
        this.notifyOnError = true;
        this.notifyOnSuccess = true;
        this.notifyOnValidationFailure = true;
    }
}


//Build databound cascading combo boxes that update and refresh data based on the configured parent child relationship
//Inherits from the kendoComboBox 
//Must use standard vLookup model for the data source  use ChildKeyIndex to deter which field (Control, Name, or Description) is the childs parent key
function nglCascadingContainerDropDowns() {

    this.lastErr = "";
    this.notifyOnError = true;
    this.notifyOnSuccess = true;
    this.notifyOnValidationFailure = true;
    this.DataDropDown = null; // reference to this objects DropDown DOM object
    this.ChildDropDowns = new Array();  //an array of nglCascadingContainerDropDown items allows for nested children.
    this.KeyIndex = 0;   //index in the parent objects nglCascadingContainerDropDown(ChildDropDowns) array 
    this.KeyName = "";  //unique name of this object typcally the DOM id 
    this.ChildKeyIndex = 2; //reference to FK used to populate ChildDropDownLists ParentControlNbr.  values are Control = 0,  Name = 1,   DescOrNbr = 2
    this.ControlNbr = "0"; //alpha numeric control value for PK reference
    this.ParentControlNbr = "-1"; //alpha numeric control value for FK lookup -1 indicates top level item
    this.DataType = "vLookupList";
    this.DOMHiddenField = "";  //the actual DOM id (not a reference to the object) of the hidden field on the page that holds the current selected or database value
    this.containers = []; //an array of cmPageDetail objects
    this.ctrlSubTypes = new GroupSubTypes();
    this.data = new vLookupList();
    this.rData = null;
    this.onSelect = null;
    this.onChildSelect = null;

    this.updateHiddenKeyField = function (iSelected) {
        var oPageElement = $("#" + this.DOMHiddenField);
        if (typeof (oPageElement) !== 'undefined') {
            oPageElement.val(iSelected);
        }
        //check if we have Child DropDowns and clear all of the dependent value
        //users must select a new value 
        if (ngl.isArray(this.ChildDropDowns) && this.ChildDropDowns.length > 0) {
            var cLen = this.ChildDropDowns.length;
            for (i = 0; i < cLen; i++) {
                var cDDL = this.ChildDropDowns[i];
                var kddl = cDDL.DataDropDown.data('kendoDropDownList');
                if (typeof (kddl) !== 'undefined') {
                    //clear the data
                    kddl.setDataSource();
                    kddl.text("");
                    kddl.value("");
                }
                //Removed by RHR we do not want to clear the current value of the hidden key because this will prevent the default value from loading in the list.
                // ** clear the data bound key field 
                // ** var cPageElement = $("#" + cDDL.DOMHiddenField);
                // ** if (typeof (cPageElement) !== 'undefined') {
                // **     cPageElement.val(0);
                // ** }
            }
        }
    }

    //on Child Select call back function The ItemSelected Method will call the onSelected callback; so when we create each child drop down list
    //we set the onSelected callback to this instance of the  ChildItemSelected function which in turn calls the parent objects onChildSelected event callback.
    //par must be passed as an nglEventParameters object
    //par.data holds a referece to the Kendo e item
    //par.datatype must equal the ChildDropDowns datatype stored in the nglCascadingContainerDropDowns.DataType property typically cmPageDetail
    //par.keyindex reference the KeyIndex property of the child nglCascadingContainerDropDowns and is the index in the child combo array.
    //par.keyname refrernces the KeyName property the child nglCascadingContainerDropDowns and will be compared with the instance stored 
    //in the ChildDropDowns array for the provided index
    this.ChildItemSelected = function (par) {
        //Child Item Selected Mehtod
        //TODO:  does not work,  called inside the wrong object
        //   should be called inside the parent function not inside the child function
        //   a reference to the wrong instance is being used
        //for now  we just bubble up this event to the caller.  the container of the widget should handle any processing of the child item selected event
        if (ngl.isFunction(this.onChildSelect)) {
            this.onChildSelect(par);
        }
    }

    //this call back does not really work except to bubble up errors 
    this.ChildItemSelectedCallBack = function (par) {
        //Child Item  Selected Call Back Method

        //TODO:  does not work,  called inside the wrong object
        //   should be called inside the parent function not inside the child function
        //   a reference to the wrong instance is being used
        //for now  we just bubble up this event to the caller.  the container of the widget should handle any processing of the child item selected event
        //if (ngl.isFunction(this.onChildSelect)) {
        //    this.onChildSelect(par);
        //}
    }

    //this call back does not really work except to bubble up errors 
    this.ChildItemChildSelectedCallBack = function (par) {
        //Child Item Child Selected Call Back Method
        //TODO:  does not work,  called inside the wrong object
        //   should be called inside the parent function not inside the child function
        //   a reference to the wrong instance is being used
        //for now  we just bubble up this event to the caller.  the container of the widget should handle any processing of the child item selected event
        //if (ngl.isFunction(this.onChildSelect)) {
        //    this.onChildSelect(par);
        //}
    }

    this.ItemSelected = function (e) {
        //Item Selected Call Back Method
        var oResults = new nglEventParameters();
        oResults.source = "ItemSelected";
        var tObj = this;
        oResults.widget = tObj;
        oResults.msg = "Item Selected";
        try {
            //clear the control number'
            this.ControlNbr = 0
            if (typeof (e.item) !== 'undefined' && ngl.isObject(e.item)) {
                var dataitem = this.DataDropDown.data('kendoDropDownList').dataItem(e.item.index());
                if (typeof (dataitem) !== 'undefined' && ngl.isObject(dataitem)) {
                    if ('Description' in dataitem) { oResults.description = dataitem.Description; }
                    if ('Name' in dataitem) { oResults.text = dataitem.Name; }
                    if ('Control' in dataitem) { oResults.value = dataitem.Control; }
                    this.ControlNbr = oResults.value;
                    this.updateHiddenKeyField(this.ControlNbr);
                    oResults.msg = oResults.text + " Selected";
                    oResults.data = dataitem;
                    oResults.datatype = "vLookupList";
                    switch (this.ChildKeyIndex) {
                        case 1:
                            this.loadChildDropDowns(oResults.text);
                            break;
                        case 2:
                            this.loadChildDropDowns(oResults.description);
                            break;
                        default:
                            this.loadChildDropDowns(this.ControlNbr);
                    }
                }
            } else {
                oResults.text = this.DataDropDown.data('kendoDropDownList').text();
                oResults.value = this.DataDropDown.data('kendoDropDownList').value();
                this.ControlNbr = oResults.value;
                this.updateHiddenKeyField(this.ControlNbr);
                switch (this.ChildKeyIndex) {
                    case 1:
                        this.loadChildDropDowns(oResults.text);
                        break;
                    case 2:
                        var data = this.DataDropDown.data('kendoDropDownList').dataSource.data();
                        for (var i = 0; i < data.length; i++) {
                            if (data[i].Control == this.ControlNbr) {
                                this.loadChildDropDowns(data[i].Description);
                                break;

                            }
                        }
                        break;
                    default:
                        this.loadChildDropDowns(this.ControlNbr);
                }
            }


        } catch (err) {
            oResults.error = err
        }
        if (ngl.isFunction(this.onSelect)) {
            this.onSelect(oResults);
        }
    }

    //pass in an array of vLookupList object  selectedValue is the 
    //current value to select by default
    this.loadData = function (oVLookupArray, selectedValue, intPageControl) {
        var tObj = this;
        //clear the current container array
        this.containers = [];
        var iDetail = 0;
        if (typeof (this.DataDropDown) !== 'undefined' && ngl.isObject(this.DataDropDown)) {
            if (this.ParentControlNbr == "-1") {
                //this is the top level so we load all of the container details on the page
                //first we create a new record that represents the Page as the container.
                var defaultPageContainer = new vLookupList()
                defaultPageContainer.Control = 0;
                defaultPageContainer.Description = 0;
                defaultPageContainer.Name = "Page";
                this.containers.push(defaultPageContainer);

                if (typeof (oVLookupArray) !== 'undefined' && ngl.isArray(oVLookupArray)) {
                    if (typeof (this.ctrlSubTypes) === "undefined" || !ngl.isObject(this.ctrlSubTypes)) { this.ctrlSubTypes = new GroupSubTypes(); }
                    this.ctrlSubTypes.addDefaultSubTypesIfNeeded();
                    var pLen = oVLookupArray.length;
                    for (iDetail = 0; iDetail < oVLookupArray.length; iDetail++) {
                        var dataitem = oVLookupArray[iDetail];
                        if (this.ctrlSubTypes.isSubTypeAContainer(dataitem.Description)) {
                            //var oContainerItem = new vLookupList()
                            //oContainerItem.Control = dataitem.PageDetControl;
                            //oContainerItem.Description = dataitem.PageDetGroupSubTypeControl;
                            //oContainerItem.Name = dataitem.PageDetCaption;
                            this.containers.push(dataitem);
                        }
                    }
                }
                var ContainerDataSource = new kendo.data.DataSource({
                    data: this.containers,
                });

                this.DataDropDown.kendoDropDownList({
                    dataTextField: "Name",
                    dataValueField: "Control",
                    autoWidth: true,
                    index: 0,
                    dataSource: ContainerDataSource,
                    select: function (e) { tObj.ItemSelected(e); }
                });
                var oddl = this.DataDropDown.data("kendoDropDownList");
                if (typeof (selectedValue) !== 'undefined' && selectedValue != "0") {
                    oddl.value(selectedValue);
                } else {
                    // oddl.select(0);
                    oddl.value(0);
                }
                //oddl.trigger("change");
                oddl.trigger("select");
            } else {
                //use this.ParentControlNbr to populate the list 
                this.ctrlSubTypes.loadDefaults()
                var conainer = this.ctrlSubTypes.getSubType(this.ParentControlNbr);
                for (iDetail = 0; iDetail < this.ctrlSubTypes.arrSubTypes.length; iDetail++) {
                    var dataitem = ctrlSubTypes.arrSubTypes[iDetail];
                    if (conainer.canContain(dataitem.GroupSubTypeControl)) {
                        var oSubContainerItem = new vLookupList()
                        oSubContainerItem.Control = dataitem.GroupSubTypeControl;
                        oSubContainerItem.Description = dataitem.GroupSubTypeGroupTypeControl;
                        oSubContainerItem.Name = dataitem.GroupSubTypeName;
                        this.containers.push(oSubContainerItem);
                    }
                }

                var ContainerDataSource = new kendo.data.DataSource({
                    data: this.containers,
                });

                this.DataDropDown.kendoDropDownList({
                    dataTextField: "Name",
                    dataValueField: "Control",
                    autoWidth: true,
                    index: 0,
                    dataSource: ContainerDataSource,
                    select: function (e) { tObj.ItemSelected(e); }
                });
                var oddl = this.DataDropDown.data("kendoDropDownList");
                if (typeof (selectedValue) !== 'undefined' && selectedValue != "0") {
                    oddl.value(selectedValue);
                } else {
                    var blnSet = false;
                    var oHidden = $("#" + tObj.DOMHiddenField);
                    if (typeof (oHidden) !== 'undefined' && ngl.isObject(oHidden)) {
                        var iSelect = oHidden.val();
                        var blnTestForCurrent = (typeof (iSelect) !== 'undefined') && (iSelect.toString() !== "0");
                        if (blnTestForCurrent) {
                            oddl.value(iSelect);
                            blnSet = true;
                        }
                    }
                    if (blnSet === false) {
                        //combobox.select(0)
                        oddl.value(0);
                    }
                }
                //oddl.trigger("change");
                oddl.trigger("select");
            }
        }
    }

    this.loadChildDropDowns = function (criteria) {
        var tObj = this;
        if (typeof (criteria) !== 'undefined' && criteria != "-1") {
            if (ngl.isArray(this.ChildDropDowns) && this.ChildDropDowns.length > 0) {
                var cLen = this.ChildDropDowns.length;
                for (i = 0; i < cLen; i++) {
                    var cDDL = this.ChildDropDowns[i];
                    cDDL.ParentControlNbr = criteria;
                    cDDL.loadData(null, 0, 0); //Note:  more work may be needed here to select the correct default value.
                }
            }
        }
    }

    this.addChildDropDown = function (domId, domHiddenField, tObj, onSelectCallback, ChildItemSelectedCallBack) {
       
        var child = new nglCascadingContainerDropDowns();
        //child.loadDefaults(domId, domHiddenField, tObj.ChildItemSelectedCallBack, tObj.ChildItemChildSelectedCallBack);
        child.loadDefaults(domId, domHiddenField, tObj, onSelectCallback, ChildItemSelectedCallBack);
        var refRet
        if (ngl.isArray(this.ChildDropDowns)) {
            var cLen = this.ChildDropDowns.length;
            var iIndex = 0;
            switch (cLen) {
                case 0:
                    child.KeyIndex = (iIndex + 1);
                    this.ChildDropDowns.push(child);
                    break;
                case 1:
                    var oInstance = this.ChildDropDowns[0];
                    if (typeof (oInstance) === 'undefined' || oInstance === null || (oInstance instanceof nglCascadingContainerDropDowns) === false) {
                        //just update zero because this is not a valid object
                        child.KeyIndex = 0;
                        this.ChildDropDowns[0] = child;
                    } else {
                        child.KeyIndex = (iIndex + 1);
                        this.ChildDropDowns.push(child);
                    }
                    break;
                default:
                    child.KeyIndex = (iIndex + 1);
                    this.ChildDropDowns.push(child);
            }
        } else {
            child.KeyIndex = 0;
            this.ChildDropDowns = new Array(child);
        }
        return child;

    }

    //loadDefaults sets up the callbacks cbSelect and cbSave
    //all call backs return a reference to this object and a string message as parameters
    this.loadDefaults = function (domId, domHiddenField, tObj, onSelectCallback, onChildSelectCallback) {
        this.KeyIndex = 0;
        this.KeyName = domId;
        this.DataDropDown = $("#" + domId);
        //this.DataDropDown.kendoComboBox();
        this.DOMHiddenField = domHiddenField;
        this.onSelect = tObj[onSelectCallback];
        this.onChildSelect = tObj[onChildSelectCallback];
        this.DataType = "vLookupList";
        this.lastErr = "";
        this.notifyOnError = true;
        this.notifyOnSuccess = true;
        this.notifyOnValidationFailure = true;
    }
}


//reuseable object for Create, Read, Update and Delete Rest Service Calls
function nglRESTCRUDCtrl() {
    //the create method will POST a json version of oChanges to the strRestService provided
    // callbacks must be passed in as a string representing the function name
    this.create = function (strRestService, oChanges, tObj, successCallBack, errorCallBack, doasync) {
        var blnRet = true;
        if (typeof (doasync) === 'undefined' || doasync === null || doasync !== true) { doasync === false; }
        if (typeof (oChanges) !== 'undefined' && ngl.isObject(oChanges)) {
            try {
                $.ajax({
                    async: doasync,
                    type: 'POST',
                    url: 'api/' + strRestService,
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    data: JSON.stringify(oChanges),
                    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                    success: function (data) { tObj[successCallBack](data); },
                    error: function (xhr, textStatus, error) { tObj[errorCallBack](xhr, textStatus, error); }
                });
            } catch (err) {
                ngl.showErrMsg("Add to Database Failure", err.message, tObj);
            }
        } else {
            blnRet = false;
            ngl.showErrMsg("Add to Database Failure", "The new record cannot be empty", tObj);
        }
        return blnRet;
    }

    // callbacks must be passed in as a string representing the function name
    this.read = function (strRestService, intControl, tObj, successCallBack, errorCallBack, doasync) {
        var blnRet = true;
        if (typeof (doasync) === 'undefined' || doasync === null || doasync !== true) { doasync === false; }
        if (typeof (intControl) != 'undefined' && intControl != null) {
            try {
                $.ajax({
                    async: doasync,
                    type: 'GET',
                    url: 'api/' + strRestService + '/' + intControl,
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                    success: function (data) { tObj[successCallBack](data); },
                    error: function (xhr, textStatus, error) { tObj[errorCallBack](xhr, textStatus, error); }
                });
            } catch (err) {
                blnRet = false;
                ngl.showErrMsg("Read from Database Failure", err.message, tObj);
            }
        } else {
            blnRet = false;
            ngl.showErrMsg("Read from Database Failure", "The selected key value for the record cannot be empty", tObj);
        }
        return blnRet;
    }

    // callbacks must be passed in as a string representing the function name
    this.filteredRead = function (strRestService, strFilter, tObj, successCallBack, errorCallBack, doasync) {
        var blnRet = true;
        if (typeof (doasync) === 'undefined' || doasync === null || doasync !== true) { doasync === false; }
        if (typeof (strFilter) != 'undefined' && strFilter != null) {
            try {
                $.ajax({
                    async: doasync,
                    type: 'Get',
                    url: 'api/' + strRestService,
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    data: { filter: JSON.stringify(strFilter) },
                    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                    success: function (data) { tObj[successCallBack](data); },
                    error: function (xhr, textStatus, error) { tObj[errorCallBack](xhr, textStatus, error); }
                });
            } catch (err) {
                blnRet = false;
                ngl.showErrMsg("Filtered Read Records Failure", err.message, tObj);
            }
        } else {
            blnRet = false;
            ngl.showErrMsg("Filtered Read Records Failure", "The provided filter cannot be empty", tObj);
        }
        return blnRet;
    }

    // callbacks must be passed in as a string representing the function name
    //note: controller must have a parameter named filter as a string like so:  [System.Web.Http.FromBody]string filter
    this.filteredPost = function (strRestService, strFilter, tObj, successCallBack, errorCallBack, doasync) {
        var blnRet = true;
        if (typeof (doasync) === 'undefined' || doasync === null || doasync !== true) { doasync = false; }
        if (typeof (strFilter) != 'undefined' && strFilter != null) {
            try {
                $.ajax({
                    traditional: true,
                    async: doasync,
                    type: 'POST',
                    url: 'api/' + strRestService,
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    data: strFilter,
                    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                    success: function (data) {
                        tObj[successCallBack](data);
                    },
                    error: function (xhr, textStatus, error) { tObj[errorCallBack](xhr, textStatus, error); }
                });
            } catch (err) {
                blnRet = false;
                ngl.showErrMsg("Filtered Posting Failure", err.message, tObj);
            }
        } else {
            blnRet = false;
            ngl.showErrMsg("Filtered Posting Failure", "The provided filter cannot be empty", tObj);
        }
        return blnRet;
    }

    this.filteredPostJSON = function (strRestService, strFilter, tObj, successCallBack, errorCallBack, doasync) {
        var blnRet = true;
        if (typeof (doasync) === 'undefined' || doasync === null || doasync !== true) { doasync = false; }
        if (typeof (strFilter) != 'undefined' && strFilter != null) {
            try {
                $.ajax({
                    async: doasync,
                    type: 'POST',
                    url: 'api/' + strRestService,
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    data: JSON.stringify(strFilter),
                    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                    success: function (data) { tObj[successCallBack](data); },
                    error: function (xhr, textStatus, error) { tObj[errorCallBack](xhr, textStatus, error); }
                });
            } catch (err) {
                blnRet = false;
                ngl.showErrMsg("Filtered Posting Failure", err.message, tObj);
            }
        } else {
            blnRet = false;
            ngl.showErrMsg("Filtered Posting Failure", "The provided filter cannot be empty", tObj);
        }
        return blnRet;
    }

    this.filteredPostJSONReturnJSON = function (strRestService, sJSON, tObj, successCallBack, errorCallBack, doasync) {
        var blnRet = true;
        if (typeof (doasync) === 'undefined' || doasync === null || doasync !== true) { doasync = false; }
        if (typeof (sJSON) != 'undefined' && sJSON != null) {
            try {
                $.ajax({
                    async: doasync,
                    type: 'POST',
                    url: 'api/' + strRestService,
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    data: JSON.stringify(sJSON),
                    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                    success: function (data) { tObj[successCallBack](data, sJSON); },
                    error: function (xhr, textStatus, error) { tObj[errorCallBack](xhr, textStatus, error, sJSON); }
                });
            } catch (err) {
                blnRet = false;
                tObj[errorCallBack]('nglRESTCRUDCtrl', "Filtered Posting Failure", err.message, sJSON);
            }
        } else {
            blnRet = false;
            tObj[errorCallBack]('nglRESTCRUDCtrl', "Filtered Posting Failure", "The selected record cannot be empty", sJSON);
        }
        return blnRet;
    }

    //the update method will POST a json version of oChanges to the strRestService provided
    // callbacks must be passed in as a string representing the function name
    this.update = function (strRestService, oChanges, tObj, successCallBack, errorCallBack, doasync) {
        var blnRet = true;
        if (typeof (doasync) === 'undefined' || doasync === null || doasync !== true) { doasync = false; }
        if (typeof (oChanges) !== 'undefined' && ngl.isObject(oChanges)) {
            try {
                //console.log('Changes');
                //console.log(oChanges.BookModDate);
                //console.log('Changes to JSON');
                //console.log(JSON.stringify(oChanges));
                $.ajax({
                    async: doasync,
                    type: 'POST',
                    url: 'api/' + strRestService,
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    data: JSON.stringify(oChanges),
                    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                    success: function (data) { tObj[successCallBack](data); },
                    error: function (xhr, textStatus, error) { tObj[errorCallBack](xhr, textStatus, error); }
                });
            } catch (err) {
                ngl.showErrMsg("Save to Database Failure", err.message, tObj);
            }
        } else {
            blnRet = false;
            ngl.showErrMsg("Save to Database Failure", "The changes to the record cannot be empty", tObj);
        }
        return blnRet;
    }

    // callbacks must be passed in as a string representing the function name
    this.delete = function (strRestService, intControl, tObj, successCallBack, errorCallBack, doasync) {
        var blnRet = true;
        if (typeof (doasync) === 'undefined' || doasync === null || doasync !== true) { doasync = false; }
        if (typeof (intControl) != 'undefined' && intControl != null) {
            try {
                $.ajax({
                    async: doasync,
                    type: 'DELETE',
                    url: 'api/' + strRestService + '/' + intControl,
                    contentType: 'application/json; charset=utf-8',
                    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                    success: function (data) { tObj[successCallBack](data); },
                    error: function (xhr, textStatus, error) { tObj[errorCallBack](xhr, textStatus, error); }
                });
            } catch (err) {
                blnRet = false;
                ngl.showErrMsg("Delete from Database Failure", err.message, tObj);
            }
        } else {
            blnRet = false;
            ngl.showErrMsg("Delete from Database Failure", "The selected key value for the record cannot be empty", tObj);
        }
        return blnRet;
    }

    // callbacks must be passed in as a string representing the function name
    // creaed by RHR for v-8.5.2.007 provides a way to delete with a string primary key
    this.filteredDelete = function (strRestService, strFilter, tObj, successCallBack, errorCallBack, doasync) {
        var blnRet = true;
        if (typeof (doasync) === 'undefined' || doasync === null || doasync !== true) { doasync = false; }
        if (typeof (strFilter) != 'undefined' && strFilter != null) {
            try {
                $.ajax({
                    async: doasync,
                    type: 'Get',
                    url: 'api/' + strRestService,
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    data: { filter: JSON.stringify(strFilter) },
                    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                    success: function (data) { tObj[successCallBack](data); },
                    error: function (xhr, textStatus, error) { tObj[errorCallBack](xhr, textStatus, error); }
                });
            } catch (err) {
                blnRet = false;
                ngl.showErrMsg("Filtered Delete from Database Failure", err.message, tObj);
            }
        } else {
            blnRet = false;
            ngl.showErrMsg("Filtered Delete from Database Failure", "The selected key value for the record cannot be empty", tObj);
        }
        return blnRet;
    }

    this.executeDataItemAPI = function (dataItem) {
        var oResults = new nglEventParameters();
        oResults.source = "executeDataItemAPI";
        oResults.widget = dataItem;
        oResults.msg = 'Failed'; //set default to Failed   
        oResults.CRUD = "execute"
        if (typeof (dataItem) !== 'undefined' && ngl.isObject(dataItem) && !isEmpty(dataItem.fieldWidgetAction)) {
            var strRestService = item.fieldAPIReference
            var blnRet = true;
            try {
                $.ajax({
                    async: doasync,
                    type: 'POST',
                    url: 'api/' + strRestService,
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    data: JSON.stringify(dataItem),
                    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                    success: function (data, oResults) {
                        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                            if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                oResults.error = new Error();
                                oResults.error.name = "Execute " + dataItem.fieldName + " API Failure";
                                oResults.error.message = data.Errors;
                            }
                        } else {
                            blnSuccess = true;
                            oResults.datatype = "DataFieldDetails";
                            oResults.data = data;
                            oResults.msg = "Success";
                        }
                    },
                    error: function (xhr, textStatus, error, oResults) {
                        oResults.error = new Error();
                        oResults.error.name = "Execute " + dataItem.fieldName + " API Failure"
                        oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
                    }
                });
            } catch (err) {
                oResults.error = err
                oResults.error.name = "Execute " + dataItem.fieldName + " API Failure"
            }
        }
        return oResults;
    }
}


//End NGL Simple Widgets
//**********************************************************************

//Begin NGL Data Entry Window Widgets 
//**********************************************************************


//Processes content management page detail information associated
//with the included html data store in Views/CMPageDetailCtrl.html
//and Views/CMNewPageDetailCtrl.html
function CMPageDetailCtrl() {
    this.lastErr = "";
    this.notifyOnError = true;
    this.notifyOnSuccess = true;
    this.notifyOnValidationFailure = true;
    this.ctrlSubTypes = new GroupSubTypes();
    data: cmPageDetail;
    rData: null;
    onSelect: null;
    onSave: null;
    onClose: null;
    onRead: null;
    dSource: null;
    sourceDiv: null;
    kendoWindowsObj: null;
    selectedcmGroupSubType: null;
    this.kendoWindowsObjSaveEventAdded = 0;
    EditWndVisibilityCombo: null; // reference to comboEditWndVisibility Added by LVV on 7/26/19
    AddWndVisibilityCombo: null; // reference to comboAddWndVisibility Added by LVV on 7/26/19

    this.EditWndVisibilitySelected = function (e) {
        //Added by LVV on 7/26/19
        var oResults = new nglEventParameters();
        oResults.source = "EditWndVisibilitySelected";
        var tObj = this;
        oResults.widget = tObj;
        oResults.msg = "Success"
        try {
            if (typeof (e.item) !== 'undefined' && ngl.isObject(e.item)) {
                var dataitem = this.EditWndVisibilityCombo.data('kendoComboBox').dataItem(e.item.index());
                if (typeof (dataitem) !== 'undefined' && ngl.isObject(dataitem)) {
                    if ('Description' in dataitem) { oResults.description = dataitem.Description; }
                    if ('Name' in dataitem) { oResults.text = dataitem.Name; }
                    if ('Control' in dataitem) oResults.value = dataitem.Control;
                    //$("#txtPageFormDesc").val(oResults.description);
                    //$("#txtPageFormName").val(oResults.text);
                    $("#txtPageDetEditWndVisibility").val(oResults.value);
                    oResults.msg = "Selected";
                    oResults.data = dataitem;
                    //oResults.datatype = "CMVisibility";
                }
            } else {
                oResults.text = this.EditWndVisibilityCombo.data('kendoComboBox').text();
                oResults.value = this.EditWndVisibilityCombo.data('kendoComboBox').value();
                //$("#txtPageFormDesc").val('');
                //$("#txtPageFormName").val(oResults.text);
                $("#txtPageDetEditWndVisibility").val(0);
                oResults.msg = "Not in List"
            }
        } catch (err) { oResults.error = err; }
        //if (ngl.isFunction(this.onSelect)) { this.onSelect(oResults); }
    }
    this.loadEditWndVisibilityList = function (e) {
        //Added by LVV on 7/26/19
        this.EditWndVisibilityCombo = e;
        var tObj = this;
        if (typeof (e) !== 'undefined' && ngl.isObject(e)) {
            e.kendoComboBox({
                dataTextField: "Description",
                dataValueField: "Control",
                autoWidth: true,
                filter: "contains",
                index: 0,
                dataSource: {
                    serverFiltering: false,
                    transport: {
                        read: {
                            url: "api/vLookupList/GetStaticList/" + nglStaticLists.CMVisibility, //82
                            headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }
                        }
                    },
                    schema: {
                        data: "Data",
                        total: "Count",
                        model: {
                            id: "Control",
                            fields: {
                                Control: { type: "number" },
                                Name: { type: "string" },
                                Description: { type: "string" }
                            }
                        },
                        errors: "Errors"
                    },
                },
                select: function (e) { tObj.EditWndVisibilitySelected(e); }
            });
        }
    }

    this.AddWndVisibilitySelected = function (e) {
        //Added by LVV on 7/26/19
        var oResults = new nglEventParameters();
        oResults.source = "AddWndVisibilitySelected";
        var tObj = this;
        oResults.widget = tObj;
        oResults.msg = "Success"
        try {
            if (typeof (e.item) !== 'undefined' && ngl.isObject(e.item)) {
                var dataitem = this.AddWndVisibilityCombo.data('kendoComboBox').dataItem(e.item.index());
                if (typeof (dataitem) !== 'undefined' && ngl.isObject(dataitem)) {
                    if ('Description' in dataitem) { oResults.description = dataitem.Description; }
                    if ('Name' in dataitem) { oResults.text = dataitem.Name; }
                    if ('Control' in dataitem) oResults.value = dataitem.Control;
                    //$("#txtPageFormDesc").val(oResults.description);
                    //$("#txtPageFormName").val(oResults.text);
                    $("#txtPageDetAddWndVisibility").val(oResults.value);
                    oResults.msg = "Selected";
                    oResults.data = dataitem;
                    //oResults.datatype = "CMVisibility";
                }
            } else {
                oResults.text = this.AddWndVisibilityCombo.data('kendoComboBox').text();
                oResults.value = this.AddWndVisibilityCombo.data('kendoComboBox').value();
                //$("#txtPageFormDesc").val('');
                //$("#txtPageFormName").val(oResults.text);
                $("#txtPageDetEditWndVisibility").val(0);
                oResults.msg = "Not in List"
            }
        } catch (err) { oResults.error = err; }
        //if (ngl.isFunction(this.onSelect)) { this.onSelect(oResults); }
    }
    this.loadAddWndVisibilityList = function (e) {
        //Added by LVV on 7/26/19
        this.AddWndVisibilityCombo = e;
        var tObj = this;
        if (typeof (e) !== 'undefined' && ngl.isObject(e)) {
            e.kendoComboBox({
                dataTextField: "Description",
                dataValueField: "Control",
                autoWidth: true,
                filter: "contains",
                index: 0,
                dataSource: {
                    serverFiltering: false,
                    transport: {
                        read: {
                            url: "api/vLookupList/GetStaticList/" + nglStaticLists.CMVisibility, //82
                            headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }
                        }
                    },
                    schema: {
                        data: "Data",
                        total: "Count",
                        model: {
                            id: "Control",
                            fields: {
                                Control: { type: "number" },
                                Name: { type: "string" },
                                Description: { type: "string" }
                            }
                        },
                        errors: "Errors"
                    },
                },
                select: function (e) { tObj.AddWndVisibilitySelected(e); }
            });
        }
    }

    this.DataElementSelectedCallBack = function (par) {
        //TODO:  add any special event handlers here
        //for now  we just show an alert for testing
        //alert(par.msg)
    }

    this.ElementFieldSelectedCallBack = function (par) {
        //TODO:  add any special event handlers here
        //for now  we just show an alert for testing
        //alert(par.msg);
    }

    this.ContainerSelectedCallBack = function (par) {
        //Container Selected Call Back
        //save the par.description value to the txtNewPageDetGroupTypeControl
        //if (typeof (par) !== 'undefined' && typeof (par.description) !== 'undefined') {
        //    $("#txtNewPageDetGroupTypeControl").val(par.description);
        //} else {
        //    $("#txtNewPageDetGroupTypeControl").val(0);
        //}
        //TODO:  add any special event handlers here
        //for now  we just show an alert for testing
        //alert(par.msg + ' ' + par.description);
    }
    this.DetailItemSelectedCallBack = function (par) {
        //Detail Item Selected Call Back
        //Capture the Element Type (SubTypeControl)
        if (typeof (par) === 'undefined') { return; }
        if (typeof (par.widget) === 'undefined') { return; }
        if (typeof (par.widget.KeyName) === 'undefined') { return; }
        if (typeof (par.description) === 'undefined') { return; }
        if (par.widget.KeyName === "ddlNewPageDetGroupSubTypeControl") {
            $("#txtNewPageDetGroupTypeControl").val(par.description);
        } else if ((par.widget.KeyName === "ddlPageDetGroupSubTypeControl")) {
            $("#txtPageDetGroupTypeControl").val(par.description);
        }
        //if (typeof (this.kendoWindowsObj) !== 'undefined' && ngl.isObject(this.kendoWindowsObj)) {
        //    if (typeof (par) !== 'undefined' && typeof (par.description) !== 'undefined') {
        //        $("#txtNewPageDetGroupTypeControl").val(par.description);
        //    }
        //    //else {
        //    //    $("#txtNewPageDetGroupTypeControl").val(0);
        //    //}
        //} else {
        //    if (typeof (par) !== 'undefined' && typeof (par.description) !== 'undefined') {
        //        $("#txtPageDetGroupTypeControl").val(par.description);
        //    } 
        //    //else {
        //    //    $("#txtPageDetGroupTypeControl").val(0);
        //    //}
        //}
    }

    //this.ContainerSelected = function (e) {
    //    var oResults = new nglEventParameters();
    //    oResults.source = "ContainerSelected";
    //    oResults.widget = this;
    //    var tSource = $("#txtNewPageDetParentID");
    //    try {
    //        if (typeof (e.item) !== 'undefined' && ngl.isObject(e.item)) {
    //            var dataitem = tSource.data('kendoDropDownList').dataItem(e.item.index());
    //            if (typeof (dataitem) !== 'undefined' && ngl.isObject(dataitem)) {
    //                if ('PageDetName' in dataitem) { oResults.text = dataitem.PageDetName; }
    //                if ('PageDetPageControl' in dataitem) oResults.value = dataitem.PageDetPageControl;
    //                oResults.msg = "Selected";
    //                oResults.data = dataitem;
    //                oResults.datatype = "cmPageDetail";
    //            }
    //        } else {
    //            oResults.msg = "Failed";
    //            oResults.error = new Error();
    //            oResults.error.name = "Invalid Operation";
    //            oResults.error.message = "Selection is not in the list.";
    //        }
    //    } catch (err) {
    //        oResults.error = err
    //    }
    //    if (ngl.isFunction(this.onSelect)) {
    //        this.onSelect(oResults);
    //    }
    //}

    //this.DetailItemSelected = function (e) {
    //    var oResults = new nglEventParameters();
    //    oResults.source = "DetailItemSelected";
    //    oResults.widget = this;
    //    var tSource = $("#txtNewPageDetGroupSubTypeControl");
    //    try {
    //        if (typeof (e.item) !== 'undefined' && ngl.isObject(e.item)) {
    //            var dataitem = tSource.data('kendoDropDownList').dataItem(e.item.index());
    //            if (typeof (dataitem) !== 'undefined' && ngl.isObject(dataitem)) {
    //                if ('GroupSubTypeName' in dataitem) { oResults.text = dataitem.GroupSubTypeName; this.selectedcmGroupSubType = dataitem; }
    //                if ('GroupSubTypeControl' in dataitem) { oResults.value = dataitem.GroupSubTypeControl; }
    //                if ('GroupSubTypeDesc' in dataitem) { oResults.description = dataitem.GroupSubTypeDesc; }
    //                oResults.msg = "Selected";
    //                oResults.data = dataitem;
    //                oResults.datatype = "cmGroupSubType";
    //            }
    //        } else {
    //            oResults.msg = "Failed";
    //            oResults.error = new Error();
    //            oResults.error.name = "Invalid Operation";
    //            oResults.error.message = "Selection is not in the list.";
    //        }
    //    } catch (err) {
    //        oResults.error = err
    //    }
    //    if (ngl.isFunction(this.onSelect)) {
    //        this.onSelect(oResults);
    //    }
    //}

    //loadContainerList is an overload to loadData that accepts an array of pgDetailsData
    //and converts it to the required vLookupList array needed by the widget 
    //typically generated from the page using a kendodatasource like pgDetailsData
    //where the cmGroupSubType.isContainer value = true.  The caller has control over
    //which page details to submit but only details where the Group Sub Type 
    //control's isSubTypeAContainer property = true will be loaded into the list
    this.loadContainerList = function (cmPageDetailArray, selectedValue, intPageControl) {
        //cmPageDetailArray must be an array of cmPageDetail
        //New Code using nglCascadingCombo widget
        //step 1: create an instance of the widget
        var ddlWidget = new nglCascadingContainerDropDowns();
        var tObj = this;
        if (typeof (this.kendoWindowsObj) !== 'undefined' && ngl.isObject(this.kendoWindowsObj)) {
            // this is the add new popup window so set the New field data objects
            //save the current page control 
            $("#txtNewPageDetPageControl").val(intPageControl);
            //step 2: load defaults
            ddlWidget.loadDefaults("ddlNewPageDetParentID", "txtNewPageDetParentID", tObj, "ContainerSelectedCallBack", "DetailItemSelectedCallBack");
            //step 3: add child element widget(s)
            var ddlChildWidget = ddlWidget.addChildDropDown("ddlNewPageDetGroupSubTypeControl", "txtNewPageDetGroupSubTypeControl", tObj, "DetailItemSelectedCallBack", "");
        } else {
            //alert("add ddlPageDetGroupSubTypeControl")
            //debugger;
            //this is the on page edit data object
            $("#txtPageDetPageControl").val(intPageControl);//step 2: load defaults
            ddlWidget.loadDefaults("ddlPageDetParentID", "txtPageDetParentID", tObj, "ContainerSelectedCallBack", "DetailItemSelectedCallBack");//step 3: add child element widget(s)
            var ddlChildWidget = ddlWidget.addChildDropDown("ddlPageDetGroupSubTypeControl", "txtPageDetGroupSubTypeControl", tObj, "DetailItemSelectedCallBack", "");
        }

        //setp 4: add more child widgets as desired (not used here 
        //childWidget.addChildCombo("example", "example", "example",0, 0)
        //step 5: Create the parent data table and load this into the parent drop down list.
        //Note: in this prototype the child data is fixed to use a GroupSupType data object
        var iDetail = 0;
        var containers = [];
        for (iDetail = 0; iDetail < cmPageDetailArray.length; iDetail++) {
            var dataitem = cmPageDetailArray[iDetail];
            if (ctrlSubTypes.isSubTypeAContainer(dataitem.PageDetGroupSubTypeControl)) {
                var newLookup = new vLookupList();
                newLookup.Control = dataitem.PageDetControl;
                newLookup.Name = dataitem.PageDetName;
                newLookup.Description = dataitem.PageDetGroupSubTypeControl
                containers.push(newLookup);
            }
        }
        ddlWidget.loadData(containers, selectedValue, intPageControl);
        // the widget will do the following:
        //  1. Read the parent data
        //  2. Select the current value of the first item
        //  3. trigger the select method which will cascade the selection process down through all children.

        //var ContainerDataSource = new kendo.data.DataSource({
        //    data: containers,
        //});
        //var tObj = this;
        //$("#txtNewPageDetParentID").kendoDropDownList({
        //    dataTextField: "PageDetName",
        //    dataValueField: "PageDetPageControl",
        //    autoWidth: true,
        //    index: 0,
        //    dataSource: ContainerDataSource,
        //    select: function (e) { tObj.ContainerSelected(e); }
        //});

    }

    //this.loadDetailItemList = function (pageDataItems) {
    //    //pageDataItems must be an array of cmGroupSubType with the following:    
    //    //GroupSubTypeControl: "";
    //    //GroupSubTypeGroupTypeControl: "";
    //    //GroupSubTypeName: "";
    //    //GroupSubTypeDesc: "";
    //    //GroupSubTypeInfo: "";
    //    //GroupSubTypeModDate: "";
    //    //GroupSubTypeModUser: "";
    //    //GroupSubTypeUpdated: "";
    //    //with the value field being GroupSubTypeControl
    //    //and the txt = GroupSubTypeName
    //    var pageDataItemsDataSource = new kendo.data.DataSource({
    //        data: pageDataItems,
    //    });
    //    var tObj = this;
    //    $("#txtNewPageDetGroupSubTypeControl").kendoDropDownList({
    //        dataTextField: "GroupSubTypeName",
    //        dataValueField: "GroupSubTypeControl",
    //        autoWidth: true,
    //        index: 0,
    //        dataSource: pageDataItemsDataSource,
    //        select: function (e) { tObj.DetailItemSelected(e); }
    //    });
    //    this.selectedcmGroupSubType = $("#txtNewPageDetGroupSubTypeControl").data('kendoDropDownList').dataItem(0);
    //}

    this.saveSuccessCallback = function (data) {
        var oResults = new nglEventParameters();
        oResults.source = "saveSuccessCallback";
        var tObj = this;
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed     
        this.rData = null;
        try {
            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                if (data.Errors != null) {
                    oResults.error = new Error();
                    if (data.StatusCode === 203) { oResults.error.name = "Authorization Timeout"; } else { oResults.error.name = "Access Denied"; }
                    oResults.error.message = data.Errors;
                    ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                }
                else {
                    this.rData = data.Data;
                    if (data.Data != null) {
                        oResults.data = data.Data;
                        this.edit(data.Data)
                        oResults.datatype = "cmPageDetail";
                        oResults.msg = "Success." + "  Your changes have been saved."
                        ngl.showSuccessMsg(oResults.msg, tObj);
                    }
                    else {
                        oResults.error = new Error();
                        oResults.error.name = "Invalid Request";
                        oResults.error.message = "No Data available.";
                        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                    }
                }
            } else { oResults.msg = "Success no data returned"; ngl.showSuccessMsg(oResults.msg, tObj); }
        } catch (err) { oResults.error = err }
        if (ngl.isFunction(this.onSave)) { this.onSave(oResults); }
    }

    this.saveAjaxErrorCallback = function (xhr, textStatus, error) {
        var oResults = new nglEventParameters();
        oResults.source = "saveAjaxErrorCallback";
        var tObj = this;
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed  
        oResults.error = new Error();
        oResults.error.name = "Save Page Detail Failure"
        oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
        if (ngl.isFunction(this.onSave)) { this.onSave(oResults); }
    }

    //Not sure if we use read on the cmPageDetailEdit page
    this.readSuccessCallback = function (data) {
        var oResults = new nglEventParameters();
        oResults.source = "readSuccessCallback";
        var tObj = this;
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed        
        this.rData = null; //clear any old return data in rData
        try {
            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                if (typeof (data.Errors) !== 'undefined' && data.Errors != null) {
                    oResults.error = new Error();
                    oResults.error.name = "Read Page Detail Failure";
                    oResults.error.message = data.Errors;
                    ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                }
                else {
                    this.rData = data.Data;
                    if (data.Data != null) {
                        oResults.data = data.Data;
                        oResults.datatype = "cmPageDetail";
                        oResults.msg = "Success: ready to edit"
                        var row = this.rData[0];
                        if (row instanceof cmPageDetail) {
                            $("#txtPageDetControl").val(row.PageDetControl);
                            $("#txtPageDetPageControl").val(row.PageDetPageControl);
                            $("#txtPageDetGroupTypeControl").val(row.PageDetGroupTypeControl);
                            $("#txtPageDetGroupSubTypeControl").val(row.PageDetGroupSubTypeControl);
                            $("#txtPageDetName").val(row.PageDetName);
                            $("#txtPageDetDesc").val(row.PageDetDesc);
                            $("#txtPageDetCaption").val(row.PageDetCaption);
                            $("#txtPageDetCaptionLocal").val(row.PageDetCaptionLocal);
                            $("#txtPageDetSequenceNo").data("kendoNumericTextBox").value(row.PageDetSequenceNo);
                            $("#txtPageDetEditWndSeqNo").data("kendoNumericTextBox").value(row.PageDetSequenceNo); //Added by LVV on 7/26/19
                            $("#txtPageDetAddWndSeqNo").data("kendoNumericTextBox").value(row.PageDetSequenceNo); //Added by LVV on 7/26/19
                            $("#txtPageDetParentID").val(row.PageDetParentID);
                            $("#txtPageDetOrientation").val(row.PageDetOrientation);
                            $("#txtPageDetWidth").data("kendoNumericTextBox").value(row.PageDetWidth);
                            $("#txtPageDetAllowFilter").val(row.PageDetAllowFilter);
                            $("#txtPageDetFilterTypeControl").val(row.PageDetFilterTypeControl);
                            var allowPaging = document.getElementById("txtPageDetAllowPaging");
                            if (row.PageDetAllowPaging === true) { allowPaging.checked = true; } else { allowPaging.checked = false; }
                            //$("#txtPageDetAllowPaging").val(row.PageDetAllowPaging);
                            $("#txtPageDetUserSecurityControl").val(row.PageDetUserSecurityControl);
                            var vis = document.getElementById("txtPageDetVisible");
                            if (row.PageDetVisible === true) { vis.checked = true; } else { vis.checked = false; }
                            $('#comboEditWndVisibility').data('kendoComboBox').value(row.PageDetEditWndVisibility); //Added by LVV on 7/26/19
                            $("#txtPageDetEditWndVisibility").val(row.PageDetEditWndVisibility); //Added by LVV on 7/26/19
                            $('#comboAddWndVisibility').data('kendoComboBox').value(row.PageDetAddWndVisibility); //Added by LVV on 7/26/19
                            $("#txtPageDetAddWndVisibility").val(row.PageDetAddWndVisibility); //Added by LVV on 7/26/19
                            var readonly = document.getElementById("txtPageDetReadOnly");
                            if (row.PageDetReadOnly === true) { readonly.checked = true; } else { readonly.checked = false; }
                            var sortable = document.getElementById("txtPageDetAllowSort");
                            if (row.PageDetAllowSort === true) { sortable.checked = true; } else { sortable.checked = false; }
                            var required = document.getElementById("txtPageDetRequired");
                            if (row.PageDetRequired === true) { required.checked = true; } else { required.checked = false; }
                            var insertonly = document.getElementById("txtPageDetInsertOnly");
                            if (row.PageDetInsertOnly === true) { insertonly.checked = true; } else { insertonly.checked = false; }
                            var expanded = document.getElementById("txtPageDetExpanded");
                            if (row.PageDetExpanded === true) { expanded.checked = true; } else { expanded.checked = false; }
                            $("#txtPageDetPageTemplateControl").data("kendoNumericTextBox").value(row.PageDetPageTemplateControl);
                            $("#txtPageDetFieldFormatOverride").val(row.PageDetFieldFormatOverride);
                            $("#txtPageDetFieldTemplateOverride").val(row.PageDetFieldTemplateOverride);
                            $("#txtPageDetWidgetAction").val(row.PageDetWidgetAction);
                            $("#txtPageDetWidgetActionKey").val(row.PageDetWidgetActionKey);
                            $("#txtPageDetDataElmtControl").val(row.PageDetDataElmtControl);
                            $("#txtPageDetElmtFieldControl").val(row.PageDetElmtFieldControl);
                            $("#txtPageDetMetaData").val(row.PageDetMetaData);
                            $("#txtPageDetFKReference").val(row.PageDetFKReference);
                            $("#txtPageDetTagIDReference").val(row.PageDetTagIDReference);
                            $("#lblPageDetTagIDReference").text(row.PageDetTagIDReference);
                            $("#txtPageDetCSSClass").val(row.PageDetCSSClass);
                            $("#txtPageDetAttributes").val(row.PageDetAttributes);
                            $("#txtPageDetAPIReference").val(row.PageDetAPIReference);
                            $("#txtPageDetAPIFilterID").val(row.PageDetAPIFilterID);
                            $("#txtPageDetAPISortKey").val(row.PageDetAPISortKey);
                            $("#txtPageDetModDate").val(row.PageDetModDate);
                            $("#txtPageDetModUser").val(row.PageDetModUser);
                            $("#txtPageDetUpdated").val(row.PageDetUpdated);
                            if (row.PageDetDataElmtControl != 0) {

                            } else {
                                $("#PageDetDataElements").hide;
                                $("#PageDetElementFields").hide;
                            }
                        }
                        //for now we do not do anything with the read method
                        //this.edit(row.PageControl, row.PageName, row.PageDesc, row.PageCaption, row.PageCaptionLocal, row.PageDataSource, row.PageSortable, row.PagePageable, row.PageGroupable, row.PageEditable, row.PageDataElmtControl, row.PageElmtFieldControl, row.PageAutoRefreshSec, row.PageFormControl)
                    }
                    else {
                        oResults.error = new Error();
                        oResults.error.name = "Invalid Request";
                        oResults.error.message = "No Data available.";
                        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                    }
                }
            } else { oResults.msg = "Success but no data was returned: Nothing to edit"; ngl.showSuccessMsg(oResults.msg, tObj); }
        } catch (err) { oResults.error = err; }
        if (ngl.isFunction(this.onRead)) { this.onRead(oResults); }
    }

    this.readAjaxErrorCallback = function (xhr, textStatus, error) {
        var oResults = new nglEventParameters();
        var tObj = this;
        oResults.source = "readAjaxErrorCallback";
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed  
        oResults.error = new Error();
        oResults.error.name = "Read Page Detail Failure"
        oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
        if (ngl.isFunction(this.onRead)) { this.onRead(oResults); }
    }

    this.hideAllControls = function () {
        var tObj = this;
        try {
            var sPageControl = $("#txtPageDetPageControl").val();
            this.rData = null;
            var oCRUDCtrl = new nglRESTCRUDCtrl();
            var blnRet = oCRUDCtrl.filteredPost("cmPageDetail/PostHideAllControls", sPageControl, tObj, "saveSuccessCallback", "saveAjaxErrorCallback");

        } catch (err) { ngl.showErrMsg(err.name, err.message, tObj); }
    }

    this.copyDescriptionToCaption = function () {
        var oChanges = new cmPageDetail();
        var tObj = this;
        try {
            var sControl = ngl.intTryParse($("#txtPageDetControl").val(), 0);
            if (typeof (sControl) !== 'undefined') { oChanges.PageDetControl = sControl; } else { oChanges.PageDetControl = 0; }
            //default values;
            oChanges.PageDetOrientation = $("#txtPageDetOrientation").val();
            oChanges.PageDetAllowFilter = $("#txtPageDetAllowFilter").prop("checked");
            oChanges.PageDetFilterTypeControl = $("#txtPageDetFilterTypeControl").val();
            oChanges.PageDetAllowPaging = $("#txtPageDetAllowPaging").prop("checked"); // $("#txtPageDetAllowPaging").val();
            oChanges.PageDetUserSecurityControl = ngl.intTryParse($("#txtPageDetUserSecurityControl").val(), 0);
            oChanges.PageDetVisible = $("#txtPageDetVisible").prop("checked");
            oChanges.PageDetEditWndVisibility = ngl.intTryParse($("#txtPageDetEditWndVisibility").val(), 0); //Added by LVV on 7/26/19
            oChanges.PageDetAddWndVisibility = ngl.intTryParse($("#txtPageDetAddWndVisibility").val(), 0); //Added by LVV on 7/26/19
            oChanges.PageDetReadOnly = $("#txtPageDetReadOnly").prop("checked");
            oChanges.PageDetAllowSort = $("#txtPageDetAllowSort").prop("checked");
            oChanges.PageDetRequired = $("#txtPageDetRequired").prop("checked");
            oChanges.PageDetInsertOnly = $("#txtPageDetInsertOnly").prop("checked");
            oChanges.PageDetExpanded = $("#txtPageDetExpanded").prop("checked");
            oChanges.PageDetFieldFormatOverride = $("#txtPageDetFieldFormatOverride").val();
            oChanges.PageDetFieldTemplateOverride = $("#txtPageDetFieldTemplateOverride").val();
            oChanges.PageDetPageTemplateControl = ngl.intTryParse($("#txtPageDetPageTemplateControl").val(), 0);
            oChanges.PageDetWidgetAction = $("#txtPageDetWidgetAction").val();
            oChanges.PageDetWidgetActionKey = $("#txtPageDetWidgetActionKey").val();
            oChanges.PageDetFKReference = $("#txtPageDetFKReference").val();
            oChanges.PageDetPageControl = ngl.intTryParse($("#txtPageDetPageControl").val(), intPageControl);
            oChanges.PageDetDataElmtControl = ngl.intTryParse($("#txtPageDetDataElmtControl").val(), 0);
            oChanges.PageDetElmtFieldControl = ngl.intTryParse($("#txtPageDetElmtFieldControl").val(), 0);
            oChanges.PageDetAPIReference = $("#txtPageDetAPIReference").val();
            oChanges.PageDetAPIFilterID = $("#txtPageDetAPIFilterID").val();
            oChanges.PageDetAPISortKey = $("#txtPageDetAPISortKey").val();
            oChanges.PageDetParentID = ngl.intTryParse($("#txtPageDetParentID").val(), 0);
            oChanges.PageDetTagIDReference = $("#txtPageDetTagIDReference").val();
            oChanges.PageDetCSSClass = $("#txtPageDetCSSClass").val();
            oChanges.PageDetAttributes = $("#txtPageDetAttributes").val();
            oChanges.PageDetMetaData = $("#txtPageDetMetaData").val();
            oChanges.PageDetName = $("#txtPageDetName").val();
            oChanges.PageDetDesc = $("#txtPageDetDesc").val();
            oChanges.PageDetCaption = oChanges.PageDetDesc;
            oChanges.PageDetCaptionLocal = "ENU=" + oChanges.PageDetDesc;
            oChanges.PageDetGroupSubTypeControl = ngl.intTryParse($("#txtPageDetGroupSubTypeControl").val(), 0);
            oChanges.PageDetGroupTypeControl = ngl.intTryParse($("#txtPageDetGroupTypeControl").val(), 0);
            oChanges.PageDetUserSecurityControl = ngl.intTryParse($("#txtPageDetUserSecurityControl").val(), 0);
            oChanges.PageDetSequenceNo = ngl.intTryParse($("#txtPageDetSequenceNo").val(), 0);
            oChanges.PageDetEditWndSeqNo = ngl.intTryParse($("#txtPageDetEditWndSeqNo").val(), 0); //Added by LVV on 7/26/19
            oChanges.PageDetAddWndSeqNo = ngl.intTryParse($("#txtPageDetAddWndSeqNo").val(), 0); //Added by LVV on 7/26/19
            oChanges.PageDetWidth = ngl.intTryParse($("#txtPageDetWidth").val(), 0);
            oChanges.PageDetModDate = $("#txtPageDetModDate").val();
            oChanges.PageDetModUser = $("#txtPageDetModUser").val();
            oChanges.PageDetUpdated = $("#txtPageDetUpdated").val();
            if (isEmpty(oChanges.PageDetName)) {
                var title = "Save Page Detail Validation Failue"
                var msg = "A name is required for the detail item!"
                ngl.showValidationMsg("Save Page Detail Validation Failue", msg, tObj);
            } else {
                this.rData = null;
                var oCRUDCtrl = new nglRESTCRUDCtrl();
                var blnRet = oCRUDCtrl.update("cmPageDetail/PostPageDetail", oChanges, tObj, "saveSuccessCallback", "saveAjaxErrorCallback");
            }
        } catch (err) { ngl.showErrMsg(err.name, err.message, tObj); }
    }

    this.makeEditFilter = function () {
        var oChanges = new cmPageDetail();
        var tObj = this;
        try {
            var sControl = ngl.intTryParse($("#txtPageDetControl").val(), 0);
            if (typeof (sControl) !== 'undefined') { oChanges.PageDetControl = sControl; } else { oChanges.PageDetControl = 0; }
            //default values;
            oChanges.PageDetOrientation = $("#txtPageDetOrientation").val();
            oChanges.PageDetAllowFilter = true;
            oChanges.PageDetFilterTypeControl = $("#txtPageDetFilterTypeControl").val();
            oChanges.PageDetAllowPaging = $("#txtPageDetAllowPaging").prop("checked");; // $("#txtPageDetAllowPaging").val();
            oChanges.PageDetUserSecurityControl = ngl.intTryParse($("#txtPageDetUserSecurityControl").val(), 0);
            oChanges.PageDetVisible = true;
            oChanges.PageDetEditWndVisibility = ngl.intTryParse($("#txtPageDetEditWndVisibility").val(), 0); //Added by LVV on 7/26/19
            oChanges.PageDetAddWndVisibility = ngl.intTryParse($("#txtPageDetAddWndVisibility").val(), 0); //Added by LVV on 7/26/19
            oChanges.PageDetReadOnly = false;
            oChanges.PageDetAllowSort = true;
            oChanges.PageDetRequired = $("#txtPageDetRequired").prop("checked");
            oChanges.PageDetInsertOnly = $("#txtPageDetInsertOnly").prop("checked");
            oChanges.PageDetExpanded = $("#txtPageDetExpanded").prop("checked");
            oChanges.PageDetFieldFormatOverride = $("#txtPageDetFieldFormatOverride").val();
            oChanges.PageDetFieldTemplateOverride = $("#txtPageDetFieldTemplateOverride").val();
            oChanges.PageDetPageTemplateControl = ngl.intTryParse($("#txtPageDetPageTemplateControl").val(), 0);
            oChanges.PageDetWidgetAction = $("#txtPageDetWidgetAction").val();
            oChanges.PageDetWidgetActionKey = $("#txtPageDetWidgetActionKey").val();
            oChanges.PageDetFKReference = $("#txtPageDetFKReference").val();
            oChanges.PageDetPageControl = ngl.intTryParse($("#txtPageDetPageControl").val(), intPageControl);
            oChanges.PageDetDataElmtControl = ngl.intTryParse($("#txtPageDetDataElmtControl").val(), 0);
            oChanges.PageDetElmtFieldControl = ngl.intTryParse($("#txtPageDetElmtFieldControl").val(), 0);
            oChanges.PageDetAPIReference = $("#txtPageDetAPIReference").val();
            oChanges.PageDetAPIFilterID = $("#txtPageDetAPIFilterID").val();
            oChanges.PageDetAPISortKey = $("#txtPageDetAPISortKey").val();
            oChanges.PageDetParentID = ngl.intTryParse($("#txtPageDetParentID").val(), 0);
            oChanges.PageDetTagIDReference = $("#txtPageDetTagIDReference").val();
            oChanges.PageDetCSSClass = $("#txtPageDetCSSClass").val();
            oChanges.PageDetAttributes = $("#txtPageDetAttributes").val();
            oChanges.PageDetMetaData = $("#txtPageDetMetaData").val();
            oChanges.PageDetName = $("#txtPageDetName").val();
            oChanges.PageDetDesc = $("#txtPageDetDesc").val();
            oChanges.PageDetCaption = $("#txtPageDetCaption").val();
            oChanges.PageDetCaptionLocal = $("#txtPageDetCaptionLocal").val();
            oChanges.PageDetGroupSubTypeControl = ngl.intTryParse($("#txtPageDetGroupSubTypeControl").val(), 0);
            oChanges.PageDetGroupTypeControl = ngl.intTryParse($("#txtPageDetGroupTypeControl").val(), 0);
            oChanges.PageDetUserSecurityControl = ngl.intTryParse($("#txtPageDetUserSecurityControl").val(), 0);
            oChanges.PageDetSequenceNo = ngl.intTryParse($("#txtPageDetSequenceNo").val(), 0);
            oChanges.PageDetEditWndSeqNo = ngl.intTryParse($("#txtPageDetEditWndSeqNo").val(), 0); //Added by LVV on 7/26/19
            oChanges.PageDetAddWndSeqNo = ngl.intTryParse($("#txtPageDetAddWndSeqNo").val(), 0); //Added by LVV on 7/26/19
            oChanges.PageDetWidth = ngl.intTryParse($("#txtPageDetWidth").val(), 0);
            oChanges.PageDetModDate = $("#txtPageDetModDate").val();
            oChanges.PageDetModUser = $("#txtPageDetModUser").val();
            oChanges.PageDetUpdated = $("#txtPageDetUpdated").val();
            if (isEmpty(oChanges.PageDetName)) {
                var title = "Save Page Detail Validation Failue"
                var msg = "A name is required for the detail item!"
                ngl.showValidationMsg("Save Page Detail Validation Failue", msg, tObj);
            } else {
                this.rData = null;
                var oCRUDCtrl = new nglRESTCRUDCtrl();
                var blnRet = oCRUDCtrl.update("cmPageDetail/PostPageDetail", oChanges, tObj, "saveSuccessCallback", "saveAjaxErrorCallback");
            }
        } catch (err) { ngl.showErrMsg(err.name, err.message, tObj); }
    }

    this.makeReadOnlyFilter = function () {
        var oChanges = new cmPageDetail();
        var tObj = this;
        try {
            var sControl = ngl.intTryParse($("#txtPageDetControl").val(), 0);
            if (typeof (sControl) !== 'undefined') { oChanges.PageDetControl = sControl; } else { oChanges.PageDetControl = 0; }
            //default values;
            oChanges.PageDetOrientation = $("#txtPageDetOrientation").val();
            oChanges.PageDetAllowFilter = true;
            oChanges.PageDetFilterTypeControl = $("#txtPageDetFilterTypeControl").val();
            oChanges.PageDetAllowPaging = $("#txtPageDetAllowPaging").prop("checked");; //$("#txtPageDetAllowPaging").val();
            oChanges.PageDetUserSecurityControl = ngl.intTryParse($("#txtPageDetUserSecurityControl").val(), 0);
            oChanges.PageDetVisible = true;
            oChanges.PageDetEditWndVisibility = ngl.intTryParse($("#txtPageDetEditWndVisibility").val(), 0); //Added by LVV on 7/26/19
            oChanges.PageDetAddWndVisibility = ngl.intTryParse($("#txtPageDetAddWndVisibility").val(), 0); //Added by LVV on 7/26/19
            oChanges.PageDetReadOnly = true;
            oChanges.PageDetAllowSort = true;
            oChanges.PageDetRequired = $("#txtPageDetRequired").prop("checked");
            oChanges.PageDetInsertOnly = $("#txtPageDetInsertOnly").prop("checked");
            oChanges.PageDetExpanded = $("#txtPageDetExpanded").prop("checked");
            oChanges.PageDetFieldFormatOverride = $("#txtPageDetFieldFormatOverride").val();
            oChanges.PageDetFieldTemplateOverride = $("#txtPageDetFieldTemplateOverride").val();
            oChanges.PageDetPageTemplateControl = ngl.intTryParse($("#txtPageDetPageTemplateControl").val(), 0);
            oChanges.PageDetWidgetAction = $("#txtPageDetWidgetAction").val();
            oChanges.PageDetWidgetActionKey = $("#txtPageDetWidgetActionKey").val();
            oChanges.PageDetFKReference = $("#txtPageDetFKReference").val();
            oChanges.PageDetPageControl = ngl.intTryParse($("#txtPageDetPageControl").val(), intPageControl);
            oChanges.PageDetDataElmtControl = ngl.intTryParse($("#txtPageDetDataElmtControl").val(), 0);
            oChanges.PageDetElmtFieldControl = ngl.intTryParse($("#txtPageDetElmtFieldControl").val(), 0);
            oChanges.PageDetAPIReference = $("#txtPageDetAPIReference").val();
            oChanges.PageDetAPIFilterID = $("#txtPageDetAPIFilterID").val();
            oChanges.PageDetAPISortKey = $("#txtPageDetAPISortKey").val();
            oChanges.PageDetParentID = ngl.intTryParse($("#txtPageDetParentID").val(), 0);
            oChanges.PageDetTagIDReference = $("#txtPageDetTagIDReference").val();
            oChanges.PageDetCSSClass = $("#txtPageDetCSSClass").val();
            oChanges.PageDetAttributes = $("#txtPageDetAttributes").val();
            oChanges.PageDetMetaData = $("#txtPageDetMetaData").val();
            oChanges.PageDetName = $("#txtPageDetName").val();
            oChanges.PageDetDesc = $("#txtPageDetDesc").val();
            oChanges.PageDetCaption = $("#txtPageDetCaption").val();
            oChanges.PageDetCaptionLocal = $("#txtPageDetCaptionLocal").val();
            oChanges.PageDetGroupSubTypeControl = ngl.intTryParse($("#txtPageDetGroupSubTypeControl").val(), 0);
            oChanges.PageDetGroupTypeControl = ngl.intTryParse($("#txtPageDetGroupTypeControl").val(), 0);
            oChanges.PageDetUserSecurityControl = ngl.intTryParse($("#txtPageDetUserSecurityControl").val(), 0);
            oChanges.PageDetSequenceNo = ngl.intTryParse($("#txtPageDetSequenceNo").val(), 0);
            oChanges.PageDetEditWndSeqNo = ngl.intTryParse($("#txtPageDetEditWndSeqNo").val(), 0); //Added by LVV on 7/26/19
            oChanges.PageDetAddWndSeqNo = ngl.intTryParse($("#txtPageDetAddWndSeqNo").val(), 0); //Added by LVV on 7/26/19
            oChanges.PageDetWidth = ngl.intTryParse($("#txtPageDetWidth").val(), 0);
            oChanges.PageDetModDate = $("#txtPageDetModDate").val();
            oChanges.PageDetModUser = $("#txtPageDetModUser").val();
            oChanges.PageDetUpdated = $("#txtPageDetUpdated").val();
            if (isEmpty(oChanges.PageDetName)) {
                var title = "Save Page Detail Validation Failue"
                var msg = "A name is required for the detail item!"
                ngl.showValidationMsg("Save Page Detail Validation Failue", msg, tObj);
            } else {
                this.rData = null;
                var oCRUDCtrl = new nglRESTCRUDCtrl();
                var blnRet = oCRUDCtrl.update("cmPageDetail/PostPageDetail", oChanges, tObj, "saveSuccessCallback", "saveAjaxErrorCallback");
            }
        } catch (err) { ngl.showErrMsg(err.name, err.message, tObj); }
    }


    this.makeHiddenReadonly = function () {
        var oChanges = new cmPageDetail();
        var tObj = this;
        try {
            var sControl = ngl.intTryParse($("#txtPageDetControl").val(), 0);
            if (typeof (sControl) !== 'undefined') { oChanges.PageDetControl = sControl; } else { oChanges.PageDetControl = 0; }
            //default values;
            oChanges.PageDetOrientation = $("#txtPageDetOrientation").val();
            oChanges.PageDetAllowFilter = false;
            oChanges.PageDetFilterTypeControl = $("#txtPageDetFilterTypeControl").val();
            oChanges.PageDetAllowPaging = $("#txtPageDetAllowPaging").prop("checked");
            oChanges.PageDetUserSecurityControl = ngl.intTryParse($("#txtPageDetUserSecurityControl").val(), 0);
            oChanges.PageDetVisible = false;
            oChanges.PageDetEditWndVisibility = 0; //Added by LVV on 7/26/19
            oChanges.PageDetAddWndVisibility = 0; //Added by LVV on 7/26/19
            oChanges.PageDetReadOnly = true;
            oChanges.PageDetAllowSort = false;
            oChanges.PageDetRequired = $("#txtPageDetRequired").prop("checked");
            oChanges.PageDetInsertOnly = false;
            oChanges.PageDetExpanded = $("#txtPageDetExpanded").prop("checked");
            oChanges.PageDetFieldFormatOverride = $("#txtPageDetFieldFormatOverride").val();
            oChanges.PageDetFieldTemplateOverride = $("#txtPageDetFieldTemplateOverride").val();
            oChanges.PageDetPageTemplateControl = ngl.intTryParse($("#txtPageDetPageTemplateControl").val(), 0);
            oChanges.PageDetWidgetAction = $("#txtPageDetWidgetAction").val();
            oChanges.PageDetWidgetActionKey = $("#txtPageDetWidgetActionKey").val();
            oChanges.PageDetFKReference = $("#txtPageDetFKReference").val();
            oChanges.PageDetPageControl = ngl.intTryParse($("#txtPageDetPageControl").val(), intPageControl);
            oChanges.PageDetDataElmtControl = ngl.intTryParse($("#txtPageDetDataElmtControl").val(), 0);
            oChanges.PageDetElmtFieldControl = ngl.intTryParse($("#txtPageDetElmtFieldControl").val(), 0);
            oChanges.PageDetAPIReference = $("#txtPageDetAPIReference").val();
            oChanges.PageDetAPIFilterID = $("#txtPageDetAPIFilterID").val();
            oChanges.PageDetAPISortKey = $("#txtPageDetAPISortKey").val();
            oChanges.PageDetParentID = ngl.intTryParse($("#txtPageDetParentID").val(), 0);
            oChanges.PageDetTagIDReference = $("#txtPageDetTagIDReference").val();
            oChanges.PageDetCSSClass = $("#txtPageDetCSSClass").val();
            oChanges.PageDetAttributes = $("#txtPageDetAttributes").val();
            oChanges.PageDetMetaData = $("#txtPageDetMetaData").val();
            oChanges.PageDetName = $("#txtPageDetName").val();
            oChanges.PageDetDesc = $("#txtPageDetDesc").val();
            oChanges.PageDetCaption = $("#txtPageDetCaption").val();
            oChanges.PageDetCaptionLocal = $("#txtPageDetCaptionLocal").val();
            oChanges.PageDetGroupSubTypeControl = ngl.intTryParse($("#txtPageDetGroupSubTypeControl").val(), 0);
            oChanges.PageDetGroupTypeControl = ngl.intTryParse($("#txtPageDetGroupTypeControl").val(), 0);
            oChanges.PageDetUserSecurityControl = ngl.intTryParse($("#txtPageDetUserSecurityControl").val(), 0);
            oChanges.PageDetSequenceNo = 101;
            oChanges.PageDetEditWndSeqNo = 101; //Added by LVV on 7/26/19
            oChanges.PageDetAddWndSeqNo = 101; //Added by LVV on 7/26/19
            oChanges.PageDetWidth = ngl.intTryParse($("#txtPageDetWidth").val(), 0);
            oChanges.PageDetModDate = $("#txtPageDetModDate").val();
            oChanges.PageDetModUser = $("#txtPageDetModUser").val();
            oChanges.PageDetUpdated = $("#txtPageDetUpdated").val();
            if (isEmpty(oChanges.PageDetName)) {
                var title = "Save Page Detail Validation Failue"
                var msg = "A name is required for the detail item!"
                ngl.showValidationMsg("Save Page Detail Validation Failue", msg, tObj);
            } else {
                this.rData = null;
                var oCRUDCtrl = new nglRESTCRUDCtrl();
                var blnRet = oCRUDCtrl.update("cmPageDetail/PostPageDetail", oChanges, tObj, "saveSuccessCallback", "saveAjaxErrorCallback");
            }
        } catch (err) { ngl.showErrMsg(err.name, err.message, tObj); }
    }


    this.save = function () {
        var oChanges = new cmPageDetail();
        var tObj = this;
        try {
            var sControl = ngl.intTryParse($("#txtPageDetControl").val(), 0);
            if (typeof (sControl) !== 'undefined') { oChanges.PageDetControl = sControl; } else { oChanges.PageDetControl = 0; }
            //default values;
            oChanges.PageDetOrientation = $("#txtPageDetOrientation").val();
            oChanges.PageDetAllowFilter = $("#txtPageDetAllowFilter").prop("checked");
            oChanges.PageDetFilterTypeControl = $("#txtPageDetFilterTypeControl").val();
            oChanges.PageDetAllowPaging = $("#txtPageDetAllowPaging").prop("checked");  //$("#txtPageDetAllowPaging").val();
            oChanges.PageDetUserSecurityControl = ngl.intTryParse($("#txtPageDetUserSecurityControl").val(), 0);
            oChanges.PageDetVisible = $("#txtPageDetVisible").prop("checked");
            oChanges.PageDetEditWndVisibility = ngl.intTryParse($("#txtPageDetEditWndVisibility").val(), 0); //Added by LVV on 7/26/19
            oChanges.PageDetAddWndVisibility = ngl.intTryParse($("#txtPageDetAddWndVisibility").val(), 0); //Added by LVV on 7/26/19
            oChanges.PageDetReadOnly = $("#txtPageDetReadOnly").prop("checked");
            oChanges.PageDetAllowSort = $("#txtPageDetAllowSort").prop("checked");
            oChanges.PageDetRequired = $("#txtPageDetRequired").prop("checked");
            oChanges.PageDetInsertOnly = $("#txtPageDetInsertOnly").prop("checked");
            oChanges.PageDetExpanded = $("#txtPageDetExpanded").prop("checked");
            oChanges.PageDetFieldFormatOverride = $("#txtPageDetFieldFormatOverride").val();
            oChanges.PageDetFieldTemplateOverride = $("#txtPageDetFieldTemplateOverride").val();
            oChanges.PageDetPageTemplateControl = ngl.intTryParse($("#txtPageDetPageTemplateControl").val(), 0);
            oChanges.PageDetWidgetAction = $("#txtPageDetWidgetAction").val();
            oChanges.PageDetWidgetActionKey = $("#txtPageDetWidgetActionKey").val();
            oChanges.PageDetFKReference = $("#txtPageDetFKReference").val();
            oChanges.PageDetPageControl = ngl.intTryParse($("#txtPageDetPageControl").val(), intPageControl);
            oChanges.PageDetDataElmtControl = ngl.intTryParse($("#txtPageDetDataElmtControl").val(), 0);
            oChanges.PageDetElmtFieldControl = ngl.intTryParse($("#txtPageDetElmtFieldControl").val(), 0);
            oChanges.PageDetAPIReference = $("#txtPageDetAPIReference").val();
            oChanges.PageDetAPIFilterID = $("#txtPageDetAPIFilterID").val();
            oChanges.PageDetAPISortKey = $("#txtPageDetAPISortKey").val();
            oChanges.PageDetParentID = ngl.intTryParse($("#txtPageDetParentID").val(), 0);
            oChanges.PageDetTagIDReference = $("#txtPageDetTagIDReference").val();
            oChanges.PageDetCSSClass = $("#txtPageDetCSSClass").val();
            oChanges.PageDetAttributes = $("#txtPageDetAttributes").val();
            oChanges.PageDetMetaData = $("#txtPageDetMetaData").val();
            oChanges.PageDetName = $("#txtPageDetName").val();
            oChanges.PageDetDesc = $("#txtPageDetDesc").val();
            oChanges.PageDetCaption = $("#txtPageDetCaption").val();
            oChanges.PageDetCaptionLocal = $("#txtPageDetCaptionLocal").val();
            oChanges.PageDetGroupSubTypeControl = ngl.intTryParse($("#txtPageDetGroupSubTypeControl").val(), 0);
            oChanges.PageDetGroupTypeControl = ngl.intTryParse($("#txtPageDetGroupTypeControl").val(), 0);
            oChanges.PageDetUserSecurityControl = ngl.intTryParse($("#txtPageDetUserSecurityControl").val(), 0);
            oChanges.PageDetSequenceNo = ngl.intTryParse($("#txtPageDetSequenceNo").val(), 0);
            oChanges.PageDetEditWndSeqNo = ngl.intTryParse($("#txtPageDetEditWndSeqNo").val(), 0); //Added by LVV on 7/26/19
            oChanges.PageDetAddWndSeqNo = ngl.intTryParse($("#txtPageDetAddWndSeqNo").val(), 0); //Added by LVV on 7/26/19
            oChanges.PageDetWidth = ngl.intTryParse($("#txtPageDetWidth").val(), 0);
            oChanges.PageDetModDate = $("#txtPageDetModDate").val();
            oChanges.PageDetModUser = $("#txtPageDetModUser").val();
            oChanges.PageDetUpdated = $("#txtPageDetUpdated").val();
            if (isEmpty(oChanges.PageDetName)) {
                var title = "Save Page Detail Validation Failue"
                var msg = "A name is required for the detail item!"
                ngl.showValidationMsg("Save Page Detail Validation Failue", msg, tObj);
            } else {
                this.rData = null;
                var oCRUDCtrl = new nglRESTCRUDCtrl();
                var blnRet = oCRUDCtrl.update("cmPageDetail/PostPageDetail", oChanges, tObj, "saveSuccessCallback", "saveAjaxErrorCallback");
            }
        } catch (err) { ngl.showErrMsg(err.name, err.message, tObj); }
    }

    this.changeSequence = function (iChange) {
        var tObj = this;
        var iSeqAdj = 0;
        if (typeof (iChange) !== 'undefined') { iSeqAdj = ngl.intTryParse(iChange, 0); }
        if (iSeqAdj === 0) {
            var title = "Changes sequence validation failue";
            var msg = "The sequence number cannot be changed!";
            ngl.showValidationMsg(title, msg, tObj);
            return false;
        }

        //var sqNoField = ngl.intTryParse($("#txtPageDetSequenceNo").val(), 0) + iSeqAdj;
        var nCurrent = ngl.intTryParse($("#txtPageDetSequenceNo").val(), 0)
        var nNew = nCurrent + iSeqAdj
        //var sqNoField = ngl.intTryParse($("#txtPageDetSequenceNo").val(), 0) + iSeqAdj;
        if (nNew < 0) {
            var title = "Changes sequence validation failue";
            var msg = "The item is already the first item in the sequence!";
            ngl.showValidationMsg(title, msg, tObj);
            return false;
        }
        $("#txtPageDetSequenceNo").data("kendoNumericTextBox").value(nNew);
        this.save();
        return true;
        //var oChanges = new cmPageDetail();
        ////validate the data:
        ////rule 1.  A page must be assigned to the selected element
        //oChanges.PageDetPageControl = ngl.intTryParse($("#txtPageDetPageControl").val(), intPageControl);
        //if (oChanges.PageDetPageControl == 0) {
        //    var title = "Changes sequence validation failue";
        //    var msg = "A page has not been assigned to the selected element!";
        //    ngl.showValidationMsg(title, msg, tObj);
        //    return false;
        //}
        //oChanges.PageDetName = $("#txtPageDetName").val();
        //if (isEmpty(oChanges.PageDetName)) {
        //    var title = "Save Page Detail Validation Failue"
        //    var msg = "A name is required for the detail item!"
        //    ngl.showValidationMsg("Save Page Detail Validation Failue", msg, tObj);
        //    return false;
        //}

        //oChanges.PageDetSequenceNo = ngl.intTryParse($("#txtPageDetSequenceNo").val(), 0) + iSeqAdj;
        //if (oChanges.PageDetSequenceNo < 0) {
        //    var title = "Changes sequence validation failue";
        //    var msg = "The item is already the first item in the sequence!";
        //    ngl.showValidationMsg(title, msg, tObj);
        //    return false;
        //}

        //try {
        //    //we save all the changes not just the sequence numbers
        //    //default values;
        //    oChanges.PageDetOrientation = $("#txtPageDetOrientation").val();
        //    oChanges.PageDetAllowFilter = $("#txtPageDetAllowFilter").val();
        //    oChanges.PageDetFilterTypeControl = $("#txtPageDetFilterTypeControl").val();
        //    oChanges.PageDetAllowSort = $("#txtPageDetAllowSort").val();
        //    oChanges.PageDetAllowPaging = $("#txtPageDetAllowPaging").val();
        //    oChanges.PageDetUserSecurityControl = ngl.intTryParse($("#txtPageDetUserSecurityControl").val(), 0);
        //    oChanges.PageDetVisible = $("#txtPageDetVisible").val();
        //    oChanges.PageDetReadOnly = $("#txtPageDetReadOnly").val();
        //    oChanges.PageDetPageTemplateControl = ngl.intTryParse($("#txtPageDetPageTemplateControl").val(), 0);
        //    oChanges.PageDetExpanded = $("#txtPageDetExpanded").val();
        //    oChanges.PageDetFKReference = $("#txtPageDetFKReference").val();
        //    oChanges.PageDetPageControl = ngl.intTryParse($("#txtPageDetPageControl").val(), intPageControl);
        //    oChanges.PageDetDataElmtControl = ngl.intTryParse($("#txtPageDetDataElmtControl").val(), 0);
        //    oChanges.PageDetElmtFieldControl = ngl.intTryParse($("#txtPageDetElmtFieldControl").val(), 0);
        //    oChanges.PageDetAPIReference = $("#txtPageDetAPIReference").val();
        //    oChanges.PageDetAPIFilterID = $("#txtPageDetAPIFilterID").val();
        //    oChanges.PageDetAPISortKey = $("#txtPageDetAPISortKey").val();
        //    oChanges.PageDetParentID = ngl.intTryParse($("#txtPageDetParentID").val(), 0);
        //    oChanges.PageDetTagIDReference = $("#txtPageDetTagIDReference").val();
        //    oChanges.PageDetCSSClass = $("#txtPageDetCSSClass").val();
        //    oChanges.PageDetAttributes = $("#txtPageDetAttributes").val();
        //    oChanges.PageDetMetaData = $("#txtPageDetMetaData").val();       
        //    oChanges.PageDetDesc = $("#txtPageDetDesc").val();
        //    oChanges.PageDetCaption = $("#txtPageDetCaption").val();
        //    oChanges.PageDetCaptionLocal = $("#txtPageDetCaptionLocal").val();
        //    oChanges.PageDetGroupSubTypeControl = ngl.intTryParse($("#txtPageDetGroupSubTypeControl").val(), 0);
        //    oChanges.PageDetGroupTypeControl = ngl.intTryParse($("#txtPageDetGroupTypeControl").val(), 0);
        //    oChanges.PageDetUserSecurityControl = ngl.intTryParse($("#txtPageDetUserSecurityControl").val(), 0);
        //    oChanges.PageDetWidth = ngl.intTryParse($("#txtPageDetWidth").val(), 0);
        //        this.rData = null;
        //        var oCRUDCtrl = new nglRESTCRUDCtrl();
        //        return oCRUDCtrl.update("cmPageDetail/PostPageDetail", oChanges, tObj, "saveSuccessCallback", "saveAjaxErrorCallback");
        //} catch (err) {
        //    ngl.showErrMsg(err.name, err.message, tObj);
        //    return false;
        //}
    }

    this.addDataElements = function () {
        var tObj = this;
        //validate the data:
        //rule 1.  A page must be assigned to the selected element
        var iPageControl = ngl.intTryParse($("#txtPageDetPageControl").val(), intPageControl);
        if (iPageControl == 0) {
            var title = "Add all data elements validation failue";
            var msg = "A page has not been assigned to the selected element!";
            ngl.showValidationMsg(title, msg, tObj);
            return false;
        }
        //rule 2.  A data element must be selected in the parent object
        var iDataElement = ngl.intTryParse($("#txtPageDetDataElmtControl").val(), 0);
        if (iDataElement == 0) {
            var title = "Add all data elements validation failue";
            var msg = "A data table or view has not been assigned to the selected element!";
            ngl.showValidationMsg(title, msg, tObj);
            return;
        }
        //rule 3.  A page group sub type must be selected and it must be a data container
        var iGroupSubTypeControl = ngl.intTryParse($("#txtPageDetGroupSubTypeControl").val(), 0);
        if (iGroupSubTypeControl == 0) {
            var title = "Add all data elements validation failue";
            var msg = "A valid container as not been assigned for the selected element!";
            ngl.showValidationMsg(title, msg, tObj);
            return false;
        } else {
            //verify that this container is a valid data container           
            if (typeof (this.ctrlSubTypes) === "undefined" || !ngl.isObject(this.ctrlSubTypes)) { this.ctrlSubTypes = new GroupSubTypes(); }
            this.ctrlSubTypes.addDefaultSubTypesIfNeeded();
            if (!this.ctrlSubTypes.allowDataBindingForSubType(iGroupSubTypeControl) == 'true') {
                var title = "Add all data elements validation failue";
                var msg = "The selected container element does not support child data binding!";
                ngl.showValidationMsg(title, msg, tObj);
                return false;
            }
        }
        //rule 4.  A page detail control is required this is used as the parent ID
        var iPageDetControl = ngl.intTryParse($("#txtPageDetControl").val(), 0);
        if (iPageDetControl == 0) {
            var title = "Add all data elements validation failue";
            var msg = "A your changes have not been properly saved.  Please save your changes and try again!";
            ngl.showValidationMsg(title, msg, tObj);
            return false;
        }
        try {
            //we pass in a cmPageDetail object but we really only need the pagecontrol,  DataElmtControl and the PageDetControl which is assigned to the parent id of all the child elements
            var oChanges = new cmPageDetail();
            oChanges.PageDetPageControl = iPageControl;
            oChanges.PageDetDataElmtControl = iDataElement;
            oChanges.PageDetGroupSubTypeControl = iGroupSubTypeControl;
            oChanges.PageDetControl = iPageDetControl;
            var oCRUDCtrl = new nglRESTCRUDCtrl();
            return oCRUDCtrl.update("cmPageDetail/PostAddDataElements", oChanges, tObj, "saveSuccessCallback", "saveAjaxErrorCallback");
        } catch (err) { ngl.showErrMsg(err.name, err.message, tObj); }
    }

    this.add = function () {
        if (ngl.intTryParse($("#txtNewPageDetGroupSubTypeControl").val(), 0) > 0) {
            var tObj = this;
            var oChanges = new cmPageDetail();
            //default values;
            oChanges.PageDetOrientation = 0;
            oChanges.PageDetAllowFilter = false;
            oChanges.PageDetFilterTypeControl = 0;
            oChanges.PageDetAllowSort = true;
            oChanges.PageDetAllowPaging = true;
            oChanges.PageDetUserSecurityControl = 0;
            oChanges.PageDetVisible = true;
            oChanges.PageDetEditWndVisibility = 0; //Added by LVV on 7/26/19
            oChanges.PageDetAddWndVisibility = 0; //Added by LVV on 7/26/19
            oChanges.PageDetReadOnly = false;
            oChanges.PageDetRequired = false;
            oChanges.PageDetInsertOnly = false;
            oChanges.PageDetPageTemplateControl = 0;
            oChanges.PageDetExpanded = true;
            oChanges.PageDetFKReference = '';
            oChanges.PageDetControl = 0;
            oChanges.PageDetPageControl = ngl.intTryParse($("#txtNewPageDetPageControl").val(), intPageControl); // $("#txtPageDetPageControl").val();

            //PageDetDataElements 
            oChanges.PageDetDataElmtControl = ngl.intTryParse($("#txtNewPageDetDataElmtControl").val(), 0);
            oChanges.PageDetElmtFieldControl = ngl.intTryParse($("#txtNewPageDetElmtFieldControl").val(), 0);
            oChanges.PageDetAPIReference = $("#txtNewPageDetAPIReference").val();
            oChanges.PageDetAPIFilterID = $("#txtNewPageDetAPIFilterID").val();
            oChanges.PageDetAPISortKey = $("#txtNewPageDetAPISortKey").val();
            oChanges.PageDetParentID = ngl.intTryParse($("#txtNewPageDetParentID").val(), 0);
            oChanges.PageDetWidgetAction = $("#txtNewPageDetWidgetAction").val();
            oChanges.PageDetWidgetActionKey = $("#txtNewPageDetWidgetActionKey").val();

            //removed by RHR on 10/26/2017  logic incomplete for using PostCreatePageDetailFromField
            //for now we always call create with the full cmPageDetail data
            //if (oChanges.PageDetElmtFieldControl != 0) {
            //    //TODO:  we need to validate this logic.  When do we actually want to call PostCreatePageDetailFromField
            //    this.rData = null;
            //    var oFilters = new createPageDetailFromFieldFilters();
            //    oFilters.PageDetPageControl = oChanges.PageDetPageControl;
            //    oFilters.PageDetParentID = oChanges.PageDetParentID;
            //    oFilters.PageDetElmtFieldControl = oChanges.PageDetElmtFieldControl;
            //    //TODO: we may need to add the following fields to this controller
            //    //PageDetTagIDReference
            //    //PageDetCSSClasspge
            //    //PageDetAttributes
            //    //PageDetMetaData
            //    //PageDetName
            //    //PageDetDesc
            //    //PageCaption
            //    //PageDetSequenceNo
            //    //PageDetWidth
            //    //basically we need to add the entire cmPageDetail model object and let the controller figure out what fields are important
            //    var oCRUDCtrl = new nglRESTCRUDCtrl();
            //    if (oCRUDCtrl.create("cmPageDetail/PostCreatePageDetailFromField", oFilters, tObj, "saveSuccessCallback", "saveAjaxErrorCallback")) { this.kendoWindowsObj.close(); }
            //} else {

            //Standard data entry fields
            oChanges.PageDetTagIDReference = $("#txtPageDetTagIDReference").val();
            oChanges.PageDetCSSClass = $("#txtNewPageDetCSSClass").val();
            oChanges.PageDetAttributes = $("#txtNewPageDetAttributes").val();
            oChanges.PageDetMetaData = $("#txtNewPageDetMetaData").val();

            //manual data entry fields
            oChanges.PageDetName = $("#txtNewPageDetName").val();
            if (isEmpty($("#txtNewPageDetDesc").val())) { oChanges.PageDetDesc = oChanges.PageDetName; } else { oChanges.PageDetDesc = $("#txtNewPageDetDesc").val(); }
            if (isEmpty($("#txtNewPageDetCaption").val())) { oChanges.PageDetCaption = oChanges.PageDetName; } else { oChanges.PageDetCaption = $("#txtNewPageDetCaption").val(); }
            if (isEmpty($("#txtNewPageDetCaptionLocal").val())) { oChanges.PageDetCaptionLocal = "ENU=" + oChanges.PageDetName; } else { oChanges.PageDetCaptionLocal = $("#txtNewPageDetCaptionLocal").val(); }

            oChanges.PageDetGroupTypeControl = ngl.intTryParse($("#txtNewPageDetGroupTypeControl").val(), 0); // this.selectedcmGroupSubType.GroupSubTypeGroupTypeControl;
            oChanges.PageDetGroupSubTypeControl = ngl.intTryParse($("#txtNewPageDetGroupSubTypeControl").val(), 0); // this.selectedcmGroupSubType.GroupSubTypeControl;
            oChanges.PageDetSequenceNo = ngl.intTryParse($("#txtNewPageDetSequenceNo").val(), 0);
            oChanges.PageDetEditWndSeqNo = ngl.intTryParse($("#txtNewPageDetEditWndSeqNo").val(), 0); //Added by LVV on 7/26/19
            oChanges.PageDetAddWndSeqNo = ngl.intTryParse($("#txtNewPageDetAddWndSeqNo").val(), 0); //Added by LVV on 7/26/19
            oChanges.PageDetWidth = ngl.intTryParse($("#txtNewPageDetWidth").val(), 50);

            if (isEmpty(oChanges.PageDetName)) {
                var title = "Save Page Detail Validation Failue"
                var msg = "A name is required for the detail item!"
                ngl.showValidationMsg("Save Page Detail Validation Failue", msg, tObj);
            } else {
                this.rData = null;
                var oCRUDCtrl = new nglRESTCRUDCtrl();
                if (oCRUDCtrl.create("cmPageDetail/PostPageDetail", oChanges, tObj, "saveSuccessCallback", "saveAjaxErrorCallback")) { this.kendoWindowsObj.close(); }
            }
            //}
        } else { ngl.showValidationMsg("Save Page Detail Validation Failue", "A Detail Type has not been selected", tObj); }
    }

    this.show = function () {
        //$("#txtPageControl").val(0);
        //$("#txtPageName").val('');
        //$("#txtPageDesc").val('');
        //$("#txtPageCaption").val('');
        //$("#txtPageCaptionLocal").val('');
        //$("#txtPageDataSource").val(false);
        //$("#txtPageSortable").val(false);
        //$("#txtPagePageable").val(false);
        //$("#txtPageGroupable").val(false);
        //$("#txtPageEditable").val(false);
        //$("#txtPageDataElmtControl").val(0);
        //$("#txtPageElmtFieldControl").val(0);
        //$("#txtPageAutoRefreshSec").val(0);
        //$("#comboFormName").data("kendoComboBox").dataSource.read();
        //$("#txtPageFormDesc").val('');
        //$("#txtPageFormName").val('');
        //$("#txtPageFormControl").val(0);

        $("#comboEditWndVisibility").data("kendoComboBox").dataSource.read(); //Added by LVV on 7/26/19
        $("#comboAddWndVisibility").data("kendoComboBox").dataSource.read(); //Added by LVV on 7/26/19

        //New Code using nglCascadingCombo widget
        //step 1: create an instance of the widget
        var cWidget = new nglCascadingCombo();
        //step 2: load defaults
        cWidget.loadDefaults("comboNewPageDetDataElements", "txtNewPageDetDataElmtControl", "GetGlobalDynamicList", 4, 1, this.DataElementSelectedCallBack, this.ElementFieldSelectedCallBack);
        //step 3: add child element widget(s)
        var childWidget = cWidget.addChildCombo("comboNewPageDetElementFields", "txtNewPageDetElmtFieldControl", "GetGlobalDynamicListFiltered", 5, 1);

        //setp 4: add more child widgets as desired (not used here 
        //childWidget.addChildCombo("example", "example", "example",0, 0)
        //step 5: Load the Data
        cWidget.loadData();
        // the widget will do the following:
        //  1. Read the parent data
        //  2. Select the current value of the first item
        //  3. trigger the select method which will cascade the selection process down through all children.

        var tObj = this;
        this.kendoWindowsObj = $("#winNewPageDetail").kendoWindow({
            title: "Create New Page Detail Item",
            modal: true,
            visible: false,
            height: '75%',
            width: '75%',
            scrollable: true,
            actions: ["save", "Minimize", "Maximize", "Close"],
            close: function (e) { tObj.close(e); }
        }).data("kendoWindow");

        if (this.kendoWindowsObjSaveEventAdded === 0) {
            //Modified by RHR for Kendo v-2018 xxx must reference .parent() on save button click.
            //Old  Code $("#winNewPageDetail").data("kendoWindow").wrapper.find(".k-svg-i-save").click(function (e) { tObj.add(); });
            $("#winNewPageDetail").data("kendoWindow").wrapper.find(".k-svg-i-save").parent().click(function (e) { e.preventDefault(); tObj.add(); });
        }
        this.kendoWindowsObjSaveEventAdded = 1;

        this.kendoWindowsObj.center().open();
    }

    this.read = function (intControl) {
        var blnRet = false;
        var tObj = this;
        if (typeof (intControl) != 'undefined' && intControl != null) {
            this.rData = null;
            var oCRUDCtrl = new nglRESTCRUDCtrl();
            blnRet = oCRUDCtrl.read("cmPageDetail", intControl, tObj, "readSuccessCallback", "readAjaxErrorCallback");
        }
        return blnRet;
    }

    this.edit = function (mPageDetail) {
        $("#txtPageDetControl").val(mPageDetail.PageDetControl);
        $("#txtPageDetPageControl").val(mPageDetail.PageDetPageControl);
        $("#txtPageDetGroupTypeControl").val(mPageDetail.PageDetGroupTypeControl);
        $("#txtPageDetGroupSubTypeControl").val(mPageDetail.PageDetGroupSubTypeControl);
        $("#txtPageDetParentID").val(mPageDetail.PageDetParentID);
        $("#txtPageDetOrientation").val(mPageDetail.PageDetOrientation);
        var allowFilter = document.getElementById("txtPageDetAllowFilter");
        if (mPageDetail.PageDetAllowFilter === true) { allowFilter.checked = true; } else { allowFilter.checked = false; }
        $("#txtPageDetFilterTypeControl").val(mPageDetail.PageDetFilterTypeControl);
        var allowPaging = document.getElementById("txtPageDetAllowPaging")
        if (mPageDetail.PageDetAllowPaging === true) { allowPaging.checked = true; } else { allowPaging.checked = false; }
        //$("#txtPageDetAllowPaging").val(mPageDetail.PageDetAllowPaging);
        $("#txtPageDetUserSecurityControl").val(mPageDetail.PageDetUserSecurityControl);
        //$("#txtPageDetVisible").prop("Checked", mPageDetail.PageDetVisible);
        var vis = document.getElementById("txtPageDetVisible");
        if (mPageDetail.PageDetVisible === true) { vis.checked = true; } else { vis.checked = false; }
        $("#comboEditWndVisibility").data("kendoComboBox").dataSource.read(); //Added by LVV on 7/26/19
        $('#comboEditWndVisibility').data('kendoComboBox').value(mPageDetail.PageDetEditWndVisibility); //Added by LVV on 7/26/19
        $("#txtPageDetEditWndVisibility").val(mPageDetail.PageDetEditWndVisibility); //Added by LVV on 7/26/19
        $("#comboAddWndVisibility").data("kendoComboBox").dataSource.read(); //Added by LVV on 7/26/19
        $('#comboAddWndVisibility').data('kendoComboBox').value(mPageDetail.PageDetAddWndVisibility); //Added by LVV on 7/26/19
        $("#txtPageDetAddWndVisibility").val(mPageDetail.PageDetAddWndVisibility); //Added by LVV on 7/26/19
        var readonly = document.getElementById("txtPageDetReadOnly")
        if (mPageDetail.PageDetReadOnly === true) { readonly.checked = true; } else { readonly.checked = false; }
        var sortable = document.getElementById("txtPageDetAllowSort");
        if (mPageDetail.PageDetAllowSort === true) { sortable.checked = true; } else { sortable.checked = false; }
        var Required = document.getElementById("txtPageDetRequired")
        if (mPageDetail.PageDetRequired === true) { Required.checked = true; } else { Required.checked = false; }
        var InsertOnly = document.getElementById("txtPageDetInsertOnly")
        if (mPageDetail.PageDetInsertOnly === true) { InsertOnly.checked = true; } else { InsertOnly.checked = false; }
        var Expanded = document.getElementById("txtPageDetExpanded")
        if (mPageDetail.PageDetExpanded === true) { Expanded.checked = true; } else { Expanded.checked = false; }
        $("#txtPageDetFieldFormatOverride").val(mPageDetail.PageDetFieldFormatOverride);
        $("#txtPageDetFieldTemplateOverride").val(mPageDetail.PageDetFieldTemplateOverride);
        $("#txtPageDetPageTemplateControl").data("kendoNumericTextBox").value(mPageDetail.PageDetPageTemplateControl);
        $("#txtPageDetWidgetAction").val(mPageDetail.PageDetWidgetAction);
        $("#txtPageDetWidgetActionKey").val(mPageDetail.PageDetWidgetActionKey);
        $("#txtPageDetDataElmtControl").val(mPageDetail.PageDetDataElmtControl);
        $("#txtPageDetElmtFieldControl").val(mPageDetail.PageDetElmtFieldControl);
        $("#txtPageDetFKReference").val(mPageDetail.PageDetFKReference);
        $("#txtPageDetTagIDReference").val(mPageDetail.PageDetTagIDReference);
        $("#lblPageDetTagIDReference").text(mPageDetail.PageDetTagIDReference);
        $("#txtPageDetName").val(mPageDetail.PageDetName);
        $("#txtPageDetDesc").val(mPageDetail.PageDetDesc);
        $("#txtPageDetCaption").val(mPageDetail.PageDetCaption);
        $("#txtPageDetCaptionLocal").val(mPageDetail.PageDetCaptionLocal);
        $("#txtPageDetSequenceNo").data("kendoNumericTextBox").value(mPageDetail.PageDetSequenceNo);
        $("#txtPageDetEditWndSeqNo").data("kendoNumericTextBox").value(mPageDetail.PageDetEditWndSeqNo); //Added by LVV on 7/26/19
        $("#txtPageDetAddWndSeqNo").data("kendoNumericTextBox").value(mPageDetail.PageDetAddWndSeqNo); //Added by LVV on 7/26/19
        //$("#txtFilterVal").data("kendoMaskedTextBox").value();
        //$("#txtValue").data("kendoNumericTextBox").value(item.BFPValue);
        $("#txtPageDetWidth").data("kendoNumericTextBox").value(mPageDetail.PageDetWidth);
        //var mdLabel = 'Meta';
        //if (typeof (sType) != 'undefined' && sType != null){
        //    mdLabel = sType.getMetaDataLabel();
        //    //alert(mdLabel);
        //} 
        //$("#pgMetaData").text(mdLabel);
        $("#pgMetaData").text(function () { if (typeof (sType) != 'undefined' && sType != null) { return sType.getMetaDataLabel(); } else { return 'Meta Data'; } });
        $("#txtPageDetMetaData").val(mPageDetail.PageDetMetaData);
        $("#txtPageDetCSSClass").val(mPageDetail.PageDetCSSClass);
        $("#txtPageDetAttributes").val(mPageDetail.PageDetAttributes);
        $("#txtPageDetAPIReference").val(mPageDetail.PageDetAPIReference);
        $("#txtPageDetAPIFilterID").val(mPageDetail.PageDetAPIFilterID);
        $("#txtPageDetAPISortKey").val(mPageDetail.PageDetAPISortKey);
        $("#txtPageDetModDate").val(mPageDetail.PageDetModDate);
        $("#txtPageDetModUser").val(mPageDetail.PageDetModUser);
        $("#txtPageDetUpdated").val(mPageDetail.PageDetUpdated);
        var cWidget = new nglCascadingCombo();
        //step 2: load defaults
        cWidget.loadDefaults("comboPageDetDataElements", "txtPageDetDataElmtControl", "GetGlobalDynamicList", 4, 1, this.DataElementSelectedCallBack, this.ElementFieldSelectedCallBack);
        //step 3: add child element widget(s)
        var childWidget = cWidget.addChildCombo("comboPageDetElementFields", "txtPageDetElmtFieldControl", "GetGlobalDynamicListFiltered", 5, 1);
        //setp 4: add more child widgets as desired (not used here 
        //childWidget.addChildCombo("example", "example", "example",0, 0)
        //step 5: Load the Data
        cWidget.loadData();
    }

    this.delete = function () {
        var oChanges = new cmPageDetail();
        var tObj = this;
        try {
            var sControl = ngl.intTryParse($("#txtPageDetControl").val(), 0);
            if (typeof (sControl) !== 'undefined') { oChanges.PageDetControl = sControl; } else { var msg = "This detail record does not have a valid primary key and cannot be deleted!"; ngl.showValidationMsg("Delete Page Detail Validation Failue", msg, tObj); return; }
            //default values;
            oChanges.PageDetOrientation = $("#txtPageDetOrientation").val();
            oChanges.PageDetAllowFilter = $("#txtPageDetAllowFilter").val();
            oChanges.PageDetFilterTypeControl = $("#txtPageDetFilterTypeControl").val
            oChanges.PageDetAllowPaging = $("#txtPageDetAllowPaging").prop("checked"); //$("#txtPageDetAllowPaging").val();
            oChanges.PageDetUserSecurityControl = ngl.intTryParse($("#txtPageDetUserSecurityControl").val(), 0);
            oChanges.PageDetVisible = $("#txtPageDetVisible").prop("checked");
            oChanges.PageDetEditWndVisibility = ngl.intTryParse($("#txtPageDetEditWndVisibility").val(), 0); //Added by LVV on 7/26/19
            oChanges.PageDetAddWndVisibility = ngl.intTryParse($("#txtPageDetAddWndVisibility").val(), 0); //Added by LVV on 7/26/19
            oChanges.PageDetReadOnly = $("#txtPageDetReadOnly").prop("checked");
            oChanges.PageDetAllowSort = $("#txtPageDetAllowSort").prop("checked");
            oChanges.PageDetRequired = $("#txtPageDetRequired").prop("checked");
            oChanges.PageDetInsertOnly = $("#txtPageDetInsertOnly").prop("checked");
            oChanges.PageDetFieldFormatOverride = $("#txtPageDetFieldFormatOverride").val();
            oChanges.PageDetFieldTemplateOverride = $("#txtPageDetFieldTemplateOverride").val();
            oChanges.PageDetPageTemplateControl = ngl.intTryParse($("#txtPageDetPageTemplateControl").val(), 0);
            oChanges.PageDetWidgetAction = $("#txtPageDetWidgetAction").val();
            oChanges.PageDetWidgetActionKey = $("#txtPageDetWidgetActionKey").val();
            oChanges.PageDetExpanded = $("#txtPageDetExpanded").val();
            oChanges.PageDetFKReference = $("#txtPageDetFKReference").val();
            oChanges.PageDetPageControl = ngl.intTryParse($("#txtPageDetPageControl").val(), intPageControl);
            oChanges.PageDetDataElmtControl = ngl.intTryParse($("#txtPageDetDataElmtControl").val(), 0);
            oChanges.PageDetElmtFieldControl = ngl.intTryParse($("#txtPageDetElmtFieldControl").val(), 0);
            oChanges.PageDetAPIReference = $("#txtPageDetAPIReference").val();
            oChanges.PageDetAPIFilterID = $("#txtPageDetAPIFilterID").val();
            oChanges.PageDetAPISortKey = $("#txtPageDetAPISortKey").val();
            oChanges.PageDetParentID = ngl.intTryParse($("#txtPageDetParentID").val(), 0);
            oChanges.PageDetTagIDReference = $("#txtPageDetTagIDReference").val();
            oChanges.PageDetCSSClass = $("#txtPageDetCSSClass").val();
            oChanges.PageDetAttributes = $("#txtPageDetAttributes").val();
            oChanges.PageDetMetaData = $("#txtPageDetMetaData").val();
            oChanges.PageDetName = $("#txtPageDetName").val();
            oChanges.PageDetDesc = $("#txtPageDetDesc").val();
            oChanges.PageDetCaption = $("#txtPageDetCaption").val();
            oChanges.PageDetCaptionLocal = $("#txtPageDetCaptionLocal").val();
            oChanges.PageDetGroupSubTypeControl = ngl.intTryParse($("#txtPageDetGroupSubTypeControl").val(), 0);
            oChanges.PageDetGroupTypeControl = ngl.intTryParse($("#txtPageDetGroupTypeControl").val(), 0);
            oChanges.PageDetUserSecurityControl = ngl.intTryParse($("#txtPageDetUserSecurityControl").val(), 0);
            oChanges.PageDetSequenceNo = ngl.intTryParse($("#txtPageDetSequenceNo").val(), 0);
            oChanges.PageDetEditWndSeqNo = ngl.intTryParse($("#txtPageDetEditWndSeqNo").val(), 0); //Added by LVV on 7/26/19
            oChanges.PageDetAddWndSeqNo = ngl.intTryParse($("#txtPageDetAddWndSeqNo").val(), 0); //Added by LVV on 7/26/19
            oChanges.PageDetWidth = ngl.intTryParse($("#txtPageDetWidth").val(), 0);
            oChanges.PageDetModDate = $("#txtPageDetModDate").val();
            oChanges.PageDetModUser = $("#txtPageDetModUser").val();
            oChanges.PageDetUpdated = $("#txtPageDetUpdated").val();
            this.rData = null;
            var oCRUDCtrl = new nglRESTCRUDCtrl();
            var blnRet = oCRUDCtrl.update("cmPageDetail/PostDelete", oChanges, tObj, "saveSuccessCallback", "saveAjaxErrorCallback");
        } catch (err) { ngl.showErrMsg(err.name, err.message, tObj); }
    }

    //Note:  this method never gets alled because we do not have a callback method for the kendoWindow
    this.close = function (e) {
        var oResults = new nglEventParameters();
        oResults.source = "close";
        var tObj = this;
        oResults.widget = tObj;
        oResults.msg = 'closing nothing is saved';
        if (typeof (this.onClose) === "function") { this.onClose(oResults); }
    }

    //loadDefaults sets up the callbacks cbSelect and cbSave
    //all call backs return a reference to this object and a string message as parameters
    this.loadDefaults = function (pageVariable, selectCallBack, saveCallBack, closeCallBack, readCallBack) {
        this.onSelect = selectCallBack;
        this.onSave = saveCallBack;
        this.onClose = closeCallBack;
        this.onRead = readCallBack;
        this.data = new cmPageDetail();
        this.lastErr = "";
        this.notifyOnError = true;
        this.notifyOnSuccess = true;
        this.notifyOnValidationFailure = true;
        var tObj = this;
        if (typeof (pageVariable) !== 'undefined' && ngl.isObject(pageVariable)) {
            //this is the add new popup window
            this.kendoWindowsObj = pageVariable;
            //$("#btnSaveNewPageDetailtop").click(function () { tObj.add(); })
            //$("#btnSaveNewDetailbot").click(function () { tObj.add(); })
            //$("#txtNewPageDetName").kendoMaskedTextBox();
            //$("#txtNewPageDetParentID").kendoDropDownList();
            //$("#txtNewPageDetGroupSubTypeControl").kendoDropDownList();
            $("#txtNewPageDetSequenceNo").kendoNumericTextBox({ format: "0" });
            $("#txtNewPageDetEditWndSeqNo").kendoNumericTextBox({ format: "0" }); //Added by LVV on 7/26/19
            $("#txtNewPageDetAddWndSeqNo").kendoNumericTextBox({ format: "0" }); //Added by LVV on 7/26/19
            $("#txtNewPageDetWidth").kendoNumericTextBox({ format: "0" });
            $("#txtNewPageDetMetaData").kendoMaskedTextBox();
            //$("#txtNewPageDetMetaData").kendoEditor({
            //    resizable: {
            //        content: true,
            //        toolbar: true                  
            //    },
            //    // Empty tools so do not display toolbar
            //    tools: []
            //});              
            $("#txtNewPageDetPageTemplateControl").kendoNumericTextBox({ format: "0" });
            $("#txtNewPageDetName").kendoMaskedTextBox();
            $("#txtNewPageDetDesc").kendoMaskedTextBox();
            $("#txtNewPageDetCaption").kendoMaskedTextBox();
            $("#txtNewPageDetCaptionLocal").kendoMaskedTextBox();
            $("#txtNewPageDetCSSClass").kendoMaskedTextBox();
            $("#txtNewPageDetAttributes").kendoMaskedTextBox();
            $("#txtNewPageDetAPIReference").kendoMaskedTextBox();
            $("#txtNewPageDetAPIFilterID").kendoMaskedTextBox();
            $("#txtNewPageDetAPISortKey").kendoMaskedTextBox();
            $("#txtNewPageDetWidgetAction").kendoMaskedTextBox();
            $("#txtNewPageDetWidgetActionKey").kendoMaskedTextBox();
            $("#comboEditWndVisibility").kendoComboBox(); //Added by LVV on 7/26/19
            this.loadEditWndVisibilityList($("#comboEditWndVisibility")); //Added by LVV on 7/26/19
            $("#comboAddWndVisibility").kendoComboBox(); //Added by LVV on 7/26/19
            this.loadAddWndVisibilityList($("#comboAddWndVisibility")); //Added by LVV on 7/26/19
        } else { this.kendoWindowsObj = null; }
    }
}


//Processes content management page information associated
//with the included html data store in Views/CreateCMPageCtrl.html
function createCMPageCtrl() {
    this.lastErr = "";
    this.notifyOnError = true;
    this.notifyOnSuccess = true;
    this.notifyOnValidationFailure = true;
    data: cmPage;
    rData: null;
    onSelect: null;
    onSave: null;
    onClose: null;
    onRead: null;
    FormNameCombo: null; // reference to comboFormName
    sourceDiv: null;
    kendoWindowsObj: null;
    DataElementCombo: null; // reference to comboPageDataElements control
    ElementFieldCombo: null; // reference to comboPageElementFields control
    this.kendoWindowsObjSaveEventAdded = 0;


    this.PageFormSelected = function (e) {
        var oResults = new nglEventParameters();
        oResults.source = "PageFormSelected";
        var tObj = this;
        oResults.widget = tObj;
        oResults.msg = "Success"
        try {
            if (typeof (e.item) !== 'undefined' && ngl.isObject(e.item)) {
                var dataitem = this.FormNameCombo.data('kendoComboBox').dataItem(e.item.index());
                if (typeof (dataitem) !== 'undefined' && ngl.isObject(dataitem)) {
                    if ('Description' in dataitem) { oResults.description = dataitem.Description; }
                    if ('Name' in dataitem) { oResults.text = dataitem.Name; }
                    if ('Control' in dataitem) oResults.value = dataitem.Control;
                    $("#txtPageFormDesc").val(oResults.description);
                    $("#txtPageFormName").val(oResults.text);
                    $("#txtPageFormControl").val(oResults.value);
                    oResults.msg = "Selected";
                    oResults.data = dataitem;
                    oResults.datatype = "cmPage";

                }
            } else {

                oResults.text = this.FormNameCombo.data('kendoComboBox').text();
                oResults.value = this.FormNameCombo.data('kendoComboBox').value();
                $("#txtPageFormDesc").val('');
                $("#txtPageFormName").val(oResults.text);
                $("#txtPageFormControl").val(0);
                oResults.msg = "Not in List"
            }
        } catch (err) {
            oResults.error = err
        }
        if (ngl.isFunction(this.onSelect)) {
            this.onSelect(oResults);
        }

    }

    this.loadFormList = function (e) {
        this.FormNameCombo = e;
        var tObj = this;
        if (typeof (e) !== 'undefined' && ngl.isObject(e)) {
            e.kendoComboBox({
                dataTextField: "Name",
                dataValueField: "Control",
                autoWidth: true,
                filter: "contains",
                index: 2,
                dataSource: {
                    serverFiltering: false,
                    transport: {
                        read: {
                            url: "api/vLookupList/GetUserDynamicList/28",
                            headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }
                        }
                    },
                    schema: {
                        data: "Data",
                        total: "Count",
                        model: {
                            id: "Control",
                            fields: {
                                Control: { type: "number" },
                                Name: { type: "string" },
                                Description: { type: "string" }
                            }
                        },
                        errors: "Errors"
                    },
                },
                select: function (e) { tObj.PageFormSelected(e); }
            });
        }

    }

    this.DataElementSelectedCallBack = function (par) {
        //TODO:  add any special event handlers here
        //for now  we just show an alert for testing
        //alert(par.msg)
    }

    this.ElementFieldSelectedCallBack = function (par) {
        //TODO:  add any special event handlers here
        //for now  we just show an alert for testing
        //alert(par.msg);
    }

    this.saveSuccessCallback = function (data) {
        var oResults = new nglEventParameters();
        var tObj = this;
        oResults.source = "saveSuccessCallback";
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed     
        this.rData = null;
        try {
            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                if (data.Errors !== null) {
                    oResults.error = new Error();
                    if (data.StatusCode === 203) {
                        oResults.error.name = "Authorization Timeout";
                        oResults.error.message = data.Errors;
                    }
                    else {
                        oResults.error.name = "Access Denied";
                        oResults.error.message = data.Errors;
                    }
                    ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                }
                else {
                    this.rData = data.Data;
                    if (data.Data !== null) {
                        oResults.data = data.Data;
                        oResults.datatype = "cmPage";
                        oResults.msg = "Success." + "  Your changes have been saved."
                        ngl.showSuccessMsg(oResults.msg, tObj);
                    }
                    else {
                        oResults.error = new Error();
                        oResults.error.name = "Invalid Request";
                        oResults.error.message = "No Data available.";
                        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                    }
                }
            } else {
                oResults.msg = "Success no data returned";
                ngl.showSuccessMsg(strMsg, tObj);
            }
        } catch (err) {
            oResults.error = err
        }
        if (ngl.isFunction(this.onSave)) {
            this.onSave(oResults);
        }

    }

    this.saveAjaxErrorCallback = function (xhr, textStatus, error) {
        var oResults = new nglEventParameters();
        var tObj = this;
        oResults.source = "saveAjaxErrorCallback";
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed  
        oResults.error = new Error();
        oResults.error.name = "Save Page Information Failure"
        oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);

        if (ngl.isFunction(this.onSave)) {
            this.onSave(oResults);
        }
    }

    this.readSuccessCallback = function (data) {
        var oResults = new nglEventParameters();
        var tObj = this;
        oResults.source = "readSuccessCallback";
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed   
        //clear any old return data in rData
        this.rData = null;
        try {
            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                if (data.Errors != null) {
                    oResults.error = new Error();
                    //Note: we need to clean up the meaning of the StatusCodes
                    //Not sure how this should work yet
                    if (data.StatusCode === 203) {
                        oResults.error.name = "Authorization Timeout";
                        oResults.error.message = data.Errors;
                    }
                    else {
                        oResults.error.name = "Access Denied";
                        oResults.error.message = data.Errors;
                    }
                    ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                }
                else {
                    this.rData = data.Data;
                    if (data.Data != null) {
                        oResults.data = data.Data;
                        oResults.datatype = "cmPage";
                        oResults.msg = "Success: ready to edit"
                        this.rData = data.Data;
                        var row = this.rData[0];
                        this.edit(row.PageControl, row.PageName, row.PageDesc, row.PageCaption, row.PageCaptionLocal, row.PageDataSource, row.PageSortable, row.PagePageable, row.PageGroupable, row.PageEditable, row.PageDataElmtControl, row.PageElmtFieldControl, row.PageAutoRefreshSec, row.PageFormControl)
                    }
                    else {
                        oResults.error = new Error();
                        oResults.error.name = "Invalid Request";
                        oResults.error.message = "No Data available.";
                        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                    }
                }
            } else {
                oResults.msg = "Success but no data was returned: Nothing to edit";
                ngl.showSuccessMsg(oResults.msg, tObj);
            }
        } catch (err) {
            oResults.error = err
        }
        if (ngl.isFunction(this.onRead)) {
            this.onRead(oResults);
        }

    }

    this.readAjaxErrorCallback = function (xhr, textStatus, error) {
        var oResults = new nglEventParameters();
        var tObj = this;
        oResults.source = "readAjaxErrorCallback";
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed  
        oResults.error = new Error();
        oResults.error.name = "Read Page Information Failure"
        oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);

        if (ngl.isFunction(this.onRead)) {
            this.onRead(oResults);
        }
    }

    this.add = function () {
        var oChanges = new cmPage();
        var tObj = this;

        try {
            oChanges.PageControl = 0;
            oChanges.PageName = $("#txtPageName").val();
            oChanges.PageDesc = $("#txtPageDesc").val();
            oChanges.PageCaption = $("#txtPageCaption").val();
            oChanges.PageCaptionLocal = $("#txtPageCaptionLocal").val();
            oChanges.PageDataSource = $("#txtPageDataSource").val();
            oChanges.PageSortable = $("#txtPageSortable").val();
            oChanges.PagePageable = $("#txtPagePageable").val();
            oChanges.PageGroupable = $("#txtPageGroupable").val();
            oChanges.PageEditable = $("#txtPageEditable").val();
            oChanges.PageDataElmtControl = $("#txtPageDataElmtControl").val();
            oChanges.PageElmtFieldControl = $("#txtPageElmtFieldControl").val();
            oChanges.PageAutoRefreshSec = $("#txtPageAutoRefreshSec").val();
            oChanges.PageFormDesc = $("#txtPageFormDesc").val();
            oChanges.PageFormName = $("#txtPageFormName").val();
            oChanges.PageFormControl = $("#txtPageFormControl").val();
            if (isEmpty(oChanges.PageName)) {
                ngl.showValidationMsg("Add New Page Validation Failue", "A page name is required!", tObj);
            } else {
                //save  changes and close window
                this.data = oChanges;
                this.rData = null;
                var oCRUDCtrl = new nglRESTCRUDCtrl();
                if (oCRUDCtrl.create("cmPage/PostNewPage", oChanges, tObj, "saveSuccessCallback", "saveAjaxErrorCallback")) { this.kendoWindowsObj.close(); }
            }
        } catch (err) {
            ngl.showErrMsg(err.name, err.message, tObj);
        }
    }

    this.save = function () {
        var oChanges = new cmPage();
        var tObj = this;
        var sControl = $("#txtPageControl").val();
        try {
            if (typeof (sControl) != 'undefined' && sControl != null) {
                oChanges.PageControl = sControl;
            } else {
                oChanges.PageControl = 0;
            }
            oChanges.PageName = $("#txtPageName").val();
            oChanges.PageDesc = $("#txtPageDesc").val();
            oChanges.PageCaption = $("#txtPageCaption").val();
            oChanges.PageCaptionLocal = $("#txtPageCaptionLocal").val();
            oChanges.PageDataSource = $("#txtPageDataSource").val();
            oChanges.PageSortable = $("#txtPageSortable").val();
            oChanges.PagePageable = $("#txtPagePageable").val();
            oChanges.PageGroupable = $("#txtPageGroupable").val();
            oChanges.PageEditable = $("#txtPageEditable").val();
            oChanges.PageDataElmtControl = $("#txtPageDataElmtControl").val();
            oChanges.PageElmtFieldControl = $("#txtPageElmtFieldControl").val();
            oChanges.PageAutoRefreshSec = $("#txtPageAutoRefreshSec").val();
            oChanges.PageFormDesc = $("#txtPageFormDesc").val();
            oChanges.PageFormName = $("#txtPageFormName").val();
            oChanges.PageFormControl = $("#txtPageFormControl").val();
            if (isEmpty(oChanges.PageName)) {
                var title = "Save New Page Validation Failue"
                var msg = "A page name is required!"
                ngl.showValidationMsg("Save New Page Validation Failue", "A page name is required!", tObj);
            } else {
                //save  changes and close window
                this.data = oChanges;
                this.rData = null;
                var oCRUDCtrl = new nglRESTCRUDCtrl();
                if (oCRUDCtrl.update("cmPage/PostNewPage", oChanges, tObj, "saveSuccessCallback", "saveAjaxErrorCallback")) { this.kendoWindowsObj.close(); }
            }
        } catch (err) {
            ngl.showErrMsg(err.name, err.message, tObj);
        }
    }

    this.show = function () {
        var tObj = this;
        $("#txtPageControl").val(0);
        $("#txtPageName").val('');
        $("#txtPageDesc").val('');
        $("#txtPageCaption").val('');
        $("#txtPageCaptionLocal").val('');
        $("#txtPageAutoRefreshSec").val(0);
        $("#comboFormName").data("kendoComboBox").dataSource.read();
        $("#txtPageFormDesc").val('');
        $("#txtPageFormName").val('');
        $("#txtPageFormControl").val(0);
        $("#txtPageDataElmtControl").val(0);
        //$("#comboPageDataElements").data("kendoComboBox").dataSource.read();
        $("#txtPageDataSource").val(false);
        $("#txtPageSortable").val(false);
        $("#txtPagePageable").val(false);
        $("#txtPageGroupable").val(false);
        $("#txtPageEditable").val(false);
        $("#txtPageElmtFieldControl").val(0);
        //old data combo code
        //$("#comboPageElementFields").data("kendoComboBox").dataSource.read();

        //New Code using nglCascadingCombo widget
        //step 1: create an instance of the widget
        var cWidget = new nglCascadingCombo();
        //step 2: load defaults
        cWidget.loadDefaults("comboPageDataElements", "txtPageElmtFieldControl", "GetGlobalDynamicList", 4, 1, this.DataElementSelectedCallBack, this.ElementFieldSelectedCallBack);
        //step 3: add child element widget(s)
        var childWidget = cWidget.addChildCombo("comboPageElementFields", "txtPageDataElmtControl", "GetGlobalDynamicListFiltered", 5, 1);
        //setp 4: add more child widgets as desired (not used here 
        //childWidget.addChildCombo("example", "example", "example",0, 0)
        //step 5: Load the Data
        cWidget.loadData();
        // the widget will do the following:
        //  1. Read the parent data
        //  2. Select the current value of the first item
        //  3. trigger the select method which will cascade the selection process down through all children.
        var tObj = this;
        this.kendoWindowsObj = $("#winNewPage").kendoWindow({
            title: "Create New Page",
            modal: true,
            visible: false,
            height: '75%',
            width: '300',
            scrollable: true,
            actions: ["save", "Minimize", "Maximize", "Close"],
            close: function (e) { tObj.close(e); }
        }).data("kendoWindow");
        //this.kendoWindowsObj.data("kendoWindow").wrapper.find(".k-svg-i-save").click(function (e) { tObj.save(); });
        if (this.kendoWindowsObjSaveEventAdded === 0) {
            //Modified by RHR for Kendo v-2018 xxx must reference .parent() on save button click.
            $("#winNewPage").data("kendoWindow").wrapper.find(".k-svg-i-save").parent().click(function (e) { tObj.save(); });
        }
        this.kendoWindowsObjSaveEventAdded = 1;

        this.kendoWindowsObj.center().open();
    }

    this.read = function (intControl) {
        var blnRet = false;

        var tObj = this;
        if (typeof (intControl) != 'undefined' && intControl != null) {
            this.rData = null;
            this.rData = null;
            var oCRUDCtrl = new nglRESTCRUDCtrl();
            blnRet = oCRUDCtrl.read("cmPage/GetPage", intControl, tObj, "readSuccessCallback", "readAjaxErrorCallback");
        }


        return blnRet;
    }

    this.edit = function (PageControl, PageName, PageDesc, PageCaption, PageCaptionLocal, PageDataSource, PageSortable, PagePageable, PageGroupable, PageEditable, PageDataElmtControl, PageElmtFieldControl, PageAutoRefreshSec, PageFormControl) {
        var tObj = this;
        $("#txtPageControl").val(PageControl);
        $("#txtPageName").val(PageName);
        $("#txtPageDesc").val(PageDesc);
        $("#txtPageCaption").val(PageCaption);
        $("#txtPageCaptionLocal").val(PageCaptionLocal);
        $("#txtPageDataSource").val(PageDataSource);
        $("#txtPageSortable").val(PageSortable);
        $("#txtPagePageable").val(PagePageable);
        $("#txtPageGroupable").val(PageGroupable);
        $("#txtPageEditable").val(PageEditable);



        $("#txtPageAutoRefreshSec").val(PageAutoRefreshSec);

        $("#comboFormName").data("kendoComboBox").dataSource.read();
        $('#comboFormName').data('kendoComboBox').value(PageFormControl);
        $("#txtPageFormControl").val(PageFormControl);

        //current values already saved:
        $("#txtPageElmtFieldControl").val(PageElmtFieldControl);
        $("#txtPageDataElmtControl").val(PageDataElmtControl);
        //Old code using combo boxes Edit Data Combo Box s
        //$("#comboPageDataElements").data("kendoComboBox").dataSource.read();
        //$("#comboPageDataElements").data("kendoComboBox").value(PageDataElmtControl);
        //$("#comboPageDataElements").data("kendoComboBox").trigger("change");
        // $("#comboPageElementFields").data("kendoComboBox").value(PageDataElmtControl);
        // $("#comboPageElementFields").data("kendoComboBox").trigger("change");
        //New Code using nglCascadingCombo widget
        //step 1: create an instance of the widget
        var cWidget = new nglCascadingCombo();
        //step 2: load defaults
        cWidget.loadDefaults("comboPageDataElements", "txtPageElmtFieldControl", "GetGlobalDynamicList", 4, 1, this.DataElementSelectedCallBack, this.ElementFieldSelectedCallBack);
        //step 3: add child element widget(s)
        var childWidget = cWidget.addChildCombo("comboPageElementFields", "txtPageDataElmtControl", "GetGlobalDynamicListFiltered", 5, 1);
        //setp 4: add more child widgets as desired (not used here 
        //childWidget.addChildCombo("example", "example", "example",0, 0)
        //step 5: Load the Data
        cWidget.loadData();
        // the widget will do the following:
        //  1. Read the parent data
        //  2. Select the current value of the first item
        //  3. trigger the select method which will cascade the selection process down through all children.
        this.kendoWindowsObj = $("#winNewPage").kendoWindow({
            title: "Edit Page Details",
            modal: true,
            visible: false,
            height: '75%',
            width: '300',
            scrollable: true,
            actions: ["save", "Minimize", "Maximize", "Close"],
            close: function (e) { tObj.close(e); }
        }).data("kendoWindow");
        //Modified by RHR for Kendo v-2018 xxx must reference .parent() on save button click.
        $("#winNewPage").data("kendoWindow").wrapper.find(".k-svg-i-save").parent().click(function (e) { tObj.save(); });
        this.kendoWindowsObj.center().open();
    }

    this.close = function (e) {
        var oResults = new nglEventParameters();
        oResults.source = "close";
        var tObj = this;
        oResults.widget = tObj;
        oResults.msg = 'Closing nothing is saved';

        if (typeof (this.onClose) === "function") {
            this.onClose(oResults);
        }
    }

    //loadDefaults sets up the callbacks cbSelect and cbSave
    //all call backs return a reference to this object and a string message as parameters
    this.loadDefaults = function (pageVariable, selectCallBack, saveCallBack, closeCallBack, readCallBack) {
        this.kendoWindowsObj = pageVariable;
        this.onSelect = selectCallBack;
        this.onSave = saveCallBack;
        this.onClose = closeCallBack;
        this.onRead = readCallBack;
        this.data = new cmPage();
        this.lastErr = "";
        this.notifyOnError = true;
        this.notifyOnSuccess = true;
        this.notifyOnValidationFailure = true;
        var tObj = this;
        //$("#btnSaveNewPagetop").click(function () { tObj.save(); })
        //$("#btnSaveNewPagebot").click(function () { tObj.save(); })
        $("#txtPageName").kendoMaskedTextBox();
        $("#txtPageDesc").kendoMaskedTextBox();
        $("#txtPageCaption").kendoMaskedTextBox();
        $("#txtPageCaptionLocal").kendoMaskedTextBox();
        $("#txtPageDataSource").kendoSwitch();
        $("#txtPageSortable").kendoSwitch();
        $("#txtPagePageable").kendoSwitch();
        $("#txtPageGroupable").kendoSwitch();
        $("#txtPageEditable").kendoSwitch();
        //$("#txtPageDataElmtControl").kendoNumericTextBox({ format: "0" });
        //$("#txtPageElmtFieldControl").kendoNumericTextBox({ format: "0" });
        $("#txtPageAutoRefreshSec").kendoNumericTextBox({ format: "0" });
        $("#comboFormName").kendoComboBox();

        $("#txtPageFormDesc").kendoMaskedTextBox();
        this.loadFormList($("#comboFormName"));
        //$("#comboPageDataElements").kendoComboBox();
        //$("#comboPageElementFields").kendoComboBox();
        //this.loadDataElements($("#comboPageDataElements"));
    }
}

//Processes load dispatching information associated
//with the rateshopping selecte quote html data store in Views/DispatchingDialog.html
////function DispatchingDialogCtrl() {
////    //Generic properties for all Widgets
////    this.lastErr = "";
////    this.notifyOnError = true;
////    this.notifyOnSuccess = true;
////    this.notifyOnValidationFailure = true;
////    data: Dispatch;
////    rData: null;
////    onSelect: null;
////    onSave: null;
////    onClose: null;
////    onRead: null;
////    dSource: null;
////    sourceDiv: null;
////    kendoWindowsObj: null;
////    oSelectContactCtrl: null; //Added By LVV on 2/19/2019 for bug fix add Carrier Contact selection to Dispatch
////    //Widget specific properties

////    this.kendoWindowsObjUploadEventAdded = 0;

////    //Widget specific functions
////    this.calcLinearFt = function () {
////        var iRet = 0;
////        if (typeof (this.data) !== 'undefined' && ngl.isObject(this.data)) {
////            if (typeof (this.data.Items) !== 'undefined' && ngl.isArray(this.data.Items) && this.data.Items.length > 0) {
////                var TotalLen = 0;
////                for (i = 0, len = this.data.Items.length; i < len; i++) {
////                    var oItem = this.data.Items[i];
////                    var iLen = this.data.Items[i].ItemLength
////                    if (typeof (iLen) !== 'undefined' && iLen !== null && !isNaN(iLen) && iLen > 0) {
////                        TotalLen += iLen;
////                    }

////                }
////                if (TotalLen !== null && !isNaN(TotalLen) && TotalLen > 0) {
////                    switch (this.data.LengthUnit.toUpperCase()) {
////                        case "IN":
////                            iRet = ngl.convertINtoLinearFeet(TotalLen);
////                            break;
////                        case "CM":
////                            iRet = ngl.convertCMtoLinearFeet(TotalLen);
////                            break;
////                        case "FT":
////                            iRet = TotalLen;
////                            break;
////                        case "M":
////                            iRet = ngl.convertMtoLinearFeet(TotalLen);
////                            break;
////                        default:
////                            iRet = 0;
////                    }
////                }
////            }

////        }
////        return iRet;
////    }

////    this.updateLoadSummaryData = function () {
////        if (typeof (this.data) !== 'undefined' && ngl.isObject(this.data)) {

////            var orig = $("#spDispOrigin");
////            var sVal = "";
////            sVal = sVal.concat(this.data.Origin.Name, '<br />', this.data.Origin.Address1, '<br />', this.data.Origin.City, " ", this.data.Origin.State, "  ", this.data.Origin.Zip);
////            orig.html(sVal);
////            var details = $("#spDispShipDetails");
////            sVal = "";
////            sVal = sVal.concat("To Load: ", kendo.toString(kendo.parseDate(this.data.PickupDate, 'yyyy-MM-dd'), 'MM/dd/yyyy'), '<br />', "To Deliver: ", kendo.toString(kendo.parseDate(this.data.DeliveryDate, 'yyyy-MM-dd'), 'MM/dd/yyyy'));
////            details.html(sVal);

////            var shipWgt = $("#spDispShipWgt").html(this.data.TotalWgt);
////            var shipQty = $("#spDispShipQty").html(this.data.TotalQty);
////            var shipPlts = $("#spDispShipPlts").html(this.data.TotalPlts);

////            var dest = $("#spDispDest");
////            sVal = "";
////            sVal = sVal.concat(this.data.Destination.Name, '<br />', this.data.Destination.Address1, '<br />', this.data.Destination.City, " ", this.data.Destination.State, "  ", this.data.Destination.Zip);
////            //sVal = string.concat($("#txtdestCompName").data("kendoMaskedTextBox").value(),'<br />',getDestPC());
////            dest.html(sVal);
////        }
////    }

////    //Added By LVV on 2/19/2019 for bug fix add Carrier Contact selection to Dispatch
////    this.updateCarrierCont = function (item) {
////        if (typeof (item) !== 'undefined' && ngl.isObject(item)) {
////            if (typeof (this.data) !== 'undefined' && ngl.isObject(this.data)) {
////                this.data.CarrierContact.ContactControl = item.ContactControl;
////                this.data.CarrierContact.ContactName = item.ContactName;
////                this.data.CarrierContact.ContactTitle = item.ContactTitle;
////                this.data.CarrierContact.ContactPhone = item.ContactPhone;
////                this.data.CarrierContact.ContactPhoneExt = item.ContactPhoneExt;
////                this.data.CarrierContact.ContactFax = item.ContactFax;
////                this.data.CarrierContact.Contact800 = item.Contact800;
////                this.data.CarrierContact.ContactEmail = item.ContactEmail;
////                this.data.CarrierContact.ContactDefault = item.ContactDefault;
////                this.data.CarrierContact.ContactTender = item.ContactTender;
////                this.data.CarrierContact.ContactScheduler = item.ContactScheduler;
////                this.data.CarrierContact.ContactCarrierControl = item.ContactCarrierControl;
////                this.data.CarrierContact.ContactLECarControl = item.ContactLECarControl;
////                this.data.CarrierContact.ContactCompControl = item.ContactCompControl;

////                $("#spDispCarContName").html(item.ContactName);
////                $("#spDispCarContTitle").html(item.ContactTitle);
////                $("#spDispCarContEmail").html(item.ContactEmail);
////                $("#spDispCarContPhone").html(item.ContactPhone);
////                $("#spDispCarContPhoneExt").html(item.ContactPhoneExt);
////                $("#spDispCarCont800").html(item.Contact800);
////                $("#spDispCarContFax").html(item.ContactFax);
////                $("#txtDispCarContControl").val(item.ContactControl);
////                $("#txtDispCarContCarrierControl").val(item.ContactCarrierControl);
////                $("#txtDispCarContLECarControl").val(item.ContactLECarControl);
////                if (item.ContactDefault) { $("#chkDispCarContDefault").prop('checked', true); } else { $("#chkDispCarContDefault").prop('checked', false); }
////                if (item.ContactScheduler) { $("#chkDispCarContScheduler").prop('checked', true); } else { $("#chkDispCarContScheduler").prop('checked', false); }
////            }
////        }
////    }

////    //Added By LVV on 2/19/2019 for bug fix add Carrier Contact selection to Dispatch
////    this.openContactWindow = function () {

////        this.oSelectContactCtrl.SelectedContactCarrierControl = 0; //reset the value

////        this.oSelectContactCtrl.SelectedContactCarrierControl = this.data.CarrierControl;

////        this.oSelectContactCtrl.read(0);
////    }


////    this.addSuccessCallback = function (data) {
////        var windowWidget = $("#winDispatchDialog").data("kendoWindow");      
////        kendo.ui.progress(windowWidget.element, false);
////        var oResults = new nglEventParameters();
////        oResults.source = "addSuccessCallback";
////        var tObj = this;
////        oResults.widget = tObj;
////        oResults.msg = 'Failed'; //set default to Failed   
////        //kendo.ui.progress($(document.body), false);
////        this.rData = null;
////        try {
////            var blnSuccess = false;
////            var blnErrorShown = false;
////            var strValidationMsg = "";
////            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {                
////                if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
////                    oResults.error = new Error();                   
////                    oResults.error.name = "Dispatch Failure";
////                    oResults.error.message = data.Errors;                   
////                    blnErrorShown = true;
////                    if (data.StatusCode == 400) {
////                        var wdth = ($(window).width() / 10) * 6;
////                        var hgt = ($(window).height() / 10) * 6;

////                        ngl.Alert("Dispatch Failure", data.Errors, wdth, hgt);
////                    } else {
////                        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
////                    }
////                }
////                else {
////                    this.rData = data.Data;
////                    if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
////                        if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
////                            blnSuccess = true;
////                            oResults.data = data.Data;                            
////                            oResults.datatype = "string";
////                            oResults.msg = "<h3>Success</h3>" + data.Data[0].ErrorMessage
////                            this.kendoWindowsObj.close();
////                            //Add Code here to Parse the new data object and display the results using the 
////                            //new dispatch report control                            
////                            //ngl.Alert('Dispatch Results',oResults.msg,400,400);
////                            //ngl.showSuccessMsg(oResults.msg, tObj);
////                        }
////                    }

////                }
////            }
////            if (blnSuccess === false && blnErrorShown === false) {
////                oResults.error = new Error();
////                oResults.error.name = "Dispatch Failure";
////                oResults.error.message = "No results are available.";
////                ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
////            }
////        } catch (err) {
////            oResults.error = err
////        }
////        if (ngl.isFunction(this.onSave)) {
////            this.onSave(oResults);
////        }

////    }

////    this.addAjaxErrorCallback = function (xhr, textStatus, error) {
////        var windowWidget = $("#winDispatchDialog").data("kendoWindow");
////        kendo.ui.progress(windowWidget.element, false);
////        var oResults = new nglEventParameters();
////        var tObj = this;
////        //kendo.ui.progress($(document.body), false);
////        oResults.source = "addAjaxErrorCallback";
////        oResults.widget = tObj;
////        oResults.msg = 'Failed'; //set default to Failed  
////        oResults.error = new Error();
////        oResults.error.name = "Dispatch Load Failure"
////        oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
////        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
////        //this.kendoWindowsObj.center().open();

////        if (ngl.isFunction(this.onSave)) {
////            this.onSave(oResults);
////        }
////    }

////    this.readSHIDSuccessCallback = function (response) {
////        var oResults = new nglEventParameters();
////        var tObj = this;
////        oResults.source = "readSHIDSuccessCallback";
////        oResults.widget = tObj;
////        oResults.msg = 'Failed'; //set default to Failed   
////        //clear any old return data in rData
////        this.rData = null;
////        try {
////            var blnSuccess = false;
////            var blnErrorShown = false;
////            var strValidationMsg = "";
////            if (typeof (response) !== 'undefined' && ngl.isObject(response)) {               
////                if (typeof (response.Errors) !== 'undefined' && response.Errors != null && response.Errors.length > 0) {
////                   var sErrMsg = "Please Close the Window and try again.  The actual error is: " + response.Errors;
////                    ngl.showErrMsg("Get Shipment ID Failure", sErrMsg, tObj);
////                }
////                else {                    
////                    if (typeof (response.Data) !== 'undefined' && ngl.isArray(response.Data)) {                       
////                        this.rData = response.Data;
////                        if (typeof (this.data) !== 'undefined' && ngl.isObject(this.data)) {
////                            this.data.SHID = this.rData[0];
////                            $("#spDispSHID").html(this.data.SHID);
////                        }                        
////                    }
////                }
////            } 
////        } catch (err) {
////            var sErrMsg = "Please Close the Window and try again.  The actual error is: " + err.description;
////            ngl.showErrMsg("Get Shipment ID Failure", sErrMsg, tObj);
////        }

////    }

////    this.readSHIDAjaxErrorCallback = function (xhr, textStatus, error) {
////        var tObj = this;        
////        var sErrMsg = "Please Close the Window and try again.  The actual error is: " + formatAjaxJSONResponsMsgs(xhr, textStatus, error, 'Failed to get the next SHID');
////        ngl.showErrMsg("Get Shipment ID Failure", sErrMsg, tObj);
////    }

////    //Generic Call back functions for all Widgets
////    this.saveSuccessCallback = function (data) {
////        var oResults = new nglEventParameters();
////        oResults.source = "saveSuccessCallback";
////        var tObj = this;
////        oResults.widget = tObj;
////        oResults.msg = 'Failed'; //set default to Failed     
////        this.rData = null;
////        try {
////            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
////                if (data.Errors != null) {
////                    oResults.error = new Error();
////                    if (data.StatusCode === 203) {
////                        oResults.error.name = "Authorization Timeout";
////                        oResults.error.message = data.Errors;
////                    }
////                    else {
////                        oResults.error.name = "Access Denied";
////                        oResults.error.message = data.Errors;
////                    }
////                    ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
////                }
////                else {
////                    this.rData = data.Data;
////                    if (data.Data != null) {
////                        oResults.data = data.Data;
////                        this.edit(data.Data)
////                        oResults.datatype = "cmPageDetail";
////                        oResults.msg = "Success." + "  Your changes have been saved."
////                        ngl.showSuccessMsg(oResults.msg, tObj);
////                    }
////                    else {
////                        oResults.error = new Error();
////                        oResults.error.name = "Invalid Request";
////                        oResults.error.message = "No Data available.";
////                        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
////                    }
////                }
////            } else {
////                oResults.msg = "Success no data returned";
////                ngl.showSuccessMsg(oResults.msg, tObj);
////            }
////        } catch (err) {
////            oResults.error = err
////        }
////        if (ngl.isFunction(this.onSave)) {
////            this.onSave(oResults);
////        }

////    }

////    this.saveAjaxErrorCallback = function (xhr, textStatus, error) {
////        var oResults = new nglEventParameters();
////        var tObj = this;
////        oResults.source = "saveAjaxErrorCallback";
////        oResults.widget = tObj;
////        oResults.msg = 'Failed'; //set default to Failed  
////        oResults.error = new Error();
////        oResults.error.name = "Save Page Detail Failure"
////        oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
////        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);

////        if (ngl.isFunction(this.onSave)) {
////            this.onSave(oResults);
////        }
////    }


////    this.readSuccessCallback = function (data) {
////        //var windowWidget = $("#winDispatchDialog").data("kendoWindow");      
////        //kendo.ui.progress(windowWidget.element, false);
////        var oResults = new nglEventParameters();
////        var tObj = this;
////        oResults.source = "readSuccessCallback";
////        oResults.widget = tObj;
////        oResults.msg = 'Failed'; //set default to Failed   
////        //clear any old return data in rData
////        this.rData = null;
////        try {
////            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
////                if (typeof (data.Errors) !== 'undefined' && data.Errors != null) {
////                    oResults.error = new Error();
////                    oResults.error.name = "Read Dispatch Data Failure";
////                    oResults.error.message = data.Errors;
////                    ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
////                }
////                else {
////                    this.rData = data.Data;
////                    if (data.Data != null) {
////                        oResults.data = data.Data;
////                        oResults.datatype = "Dispatch";
////                        oResults.msg = "Success"
////                        this.data = data.Data[0];
////                        //this.show();
////                    }
////                    else {
////                        oResults.error = new Error();
////                        oResults.error.name = "Invalid Request";
////                        oResults.error.message = "No Data available.";
////                        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
////                    }
////                }
////            } else {
////                oResults.msg = "Success but no data was returned. Please refresh your page and try again.";
////                ngl.showSuccessMsg(oResults.msg, tObj);
////            }
////        } catch (err) {
////            oResults.error = err
////        }
////        if (ngl.isFunction(this.onRead)) {
////            this.onRead(oResults);
////        }
////        if (oResults.msg == "Success") {
////            this.show();
////        }

////    }

////    this.readAjaxErrorCallback = function (xhr, textStatus, error) {
////        //var windowWidget = $("#winDispatchDialog").data("kendoWindow");
////        //kendo.ui.progress(windowWidget.element, false);
////        var oResults = new nglEventParameters();
////        var tObj = this;
////        oResults.source = "readAjaxErrorCallback";
////        oResults.widget = tObj;
////        oResults.msg = 'Failed'; //set default to Failed  
////        oResults.error = new Error();
////        oResults.error.name = "Read Dispatch Data Failure"
////        oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
////        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);

////        if (ngl.isFunction(this.onRead)) {
////            this.onRead(oResults);
////        }
////    }

////    // Generic functions for all Widgets

////    this.validateRequiredFields = function (data){
////        var oRet = new validationResults();
////        if (typeof (data) === 'undefined' || !ngl.isObject(data)) {
////            //something went wrong so throw an error. this should never happen
////            ngl.showValidationMsg("Dispatch Validation Failed; No Data", "Please Contact Technical Support", tObj);
////            oRet.Message = "Dispatch Validation Failed; No Data";
////            return oRet;
////        } 

////        var blnValidated = true;
////        var sValidationMsg = "";
////        var sSpacer = "";
////        if (isEmpty(data.Origin.Name)) {blnValidated = false; sValidationMsg += sSpacer + " Origin Location Name is Required. "; sSpacer = " And "; }

////        if (isEmpty(data.Origin.Contact.ContactName)) {blnValidated = false; sValidationMsg += sSpacer + " Origin Contact Name is Required. "; sSpacer = " And "; }
////        if (isEmpty(data.Origin.Contact.ContactPhone)) {blnValidated = false; sValidationMsg += sSpacer + " Origin Contact Phone is Required. "; sSpacer = " And "; }       
////        if (isEmpty(data.Origin.Address1)) {blnValidated = false; sValidationMsg += sSpacer + " Origin Street Addrtess is Required. "; sSpacer = " And "; }

////        if (isEmpty(data.Destination.Name)) {blnValidated = false; sValidationMsg += sSpacer + " Destination Location Name is Required. "; sSpacer = " And "; }
////        if (isEmpty(data.Destination.Contact.ContactName)) {blnValidated = false; sValidationMsg += sSpacer + " Destination Contact Name is Required. "; sSpacer = " And "; }
////        if (isEmpty(data.Destination.Contact.ContactPhone)) {blnValidated = false; sValidationMsg += sSpacer + " Destination Contact Phone is Required. "; sSpacer = " And "; }        
////        if (isEmpty(data.Destination.Address1)) {blnValidated = false; sValidationMsg += sSpacer + " Destination Street Addrtess  is Required. "; sSpacer = " And "; }

////        if (isEmpty(data.PickupDate)) {blnValidated = false; sValidationMsg += sSpacer + " Pickup Date is Required. "; sSpacer = " And "; }
////        if (isEmpty(data.PickupStartTime)) {blnValidated = false; sValidationMsg += sSpacer + " Pickup Start Time is Required. "; sSpacer = " And "; }
////        if (isEmpty(data.PickupEndTime)) {blnValidated = false; sValidationMsg += sSpacer + " Pickup End Time is Required. "; sSpacer = " And "; }
////        if (isEmpty(data.DeliveryDate)) {blnValidated = false; sValidationMsg += sSpacer + " Delivery Date is Required. "; sSpacer = " And "; }
////        if (isEmpty(data.DeliveryStartTime)) {blnValidated = false; sValidationMsg += sSpacer + " Delivery Start Time is Required. "; sSpacer = " And "; }
////        if (isEmpty(data.DeliveryEndTime)) {blnValidated = false; sValidationMsg += sSpacer + " Delivery End Time is Required. "; sSpacer = " And "; }
////        oRet.Success = blnValidated;
////        oRet.Message = sValidationMsg;


////        return oRet;
////     }

////    this.print = function () {
////        var kinWin = $("#winDispatchDialog").kendoWindow().data("kendoWindow");
////        var winWrapper = kinWin.wrapper;
////        winWrapper.removeClass("k-window");
////        winWrapper.addClass("printable");
////        //put pring code here
////        window.print();
////        winWrapper.removeClass("printable");
////        winWrapper.addClass("k-window");      

////    }
////    // Generic CRUD functions for all Widgets
////    this.save = function () {
////        return;
////        //var oChanges = new cmPageDetail();
////        //var tObj = this;
////        //try {
////        //    var sControl = ngl.intTryParse($("#txtPageDetControl").val(), 0);
////        //    if (typeof (sControl) !== 'undefined') {
////        //        oChanges.PageDetControl = sControl;
////        //    } else {
////        //        oChanges.PageDetControl = 0;
////        //    }

////        //    //default values;
////        //    oChanges.PageDetOrientation = $("#txtPageDetOrientation").val();
////        //    oChanges.PageDetAllowFilter = $("#txtPageDetAllowFilter").prop("checked");
////        //    oChanges.PageDetFilterTypeControl = $("#txtPageDetFilterTypeControl").val();
////        //    oChanges.PageDetAllowPaging = $("#txtPageDetAllowPaging").val();
////        //    oChanges.PageDetUserSecurityControl = ngl.intTryParse($("#txtPageDetUserSecurityControl").val(), 0);
////        //    oChanges.PageDetVisible = $("#txtPageDetVisible").prop("checked");
////        //    oChanges.PageDetReadOnly = $("#txtPageDetReadOnly").prop("checked");
////        //    oChanges.PageDetAllowSort = $("#txtPageDetAllowSort").prop("checked");
////        //    oChanges.PageDetPageTemplateControl = ngl.intTryParse($("#txtPageDetPageTemplateControl").val(), 0);
////        //    oChanges.PageDetExpanded = $("#txtPageDetExpanded").val();
////        //    oChanges.PageDetFKReference = $("#txtPageDetFKReference").val();

////        //    oChanges.PageDetPageControl = ngl.intTryParse($("#txtPageDetPageControl").val(), intPageControl);
////        //    oChanges.PageDetDataElmtControl = ngl.intTryParse($("#txtPageDetDataElmtControl").val(), 0);
////        //    oChanges.PageDetElmtFieldControl = ngl.intTryParse($("#txtPageDetElmtFieldControl").val(), 0);
////        //    oChanges.PageDetAPIReference = $("#txtPageDetAPIReference").val();
////        //    oChanges.PageDetAPIFilterID = $("#txtPageDetAPIFilterID").val();
////        //    oChanges.PageDetAPISortKey = $("#txtPageDetAPISortKey").val();
////        //    oChanges.PageDetParentID = ngl.intTryParse($("#txtPageDetParentID").val(), 0);

////        //    oChanges.PageDetTagIDReference = $("#txtPageDetTagIDReference").val();
////        //    oChanges.PageDetCSSClass = $("#txtPageDetCSSClass").val();
////        //    oChanges.PageDetAttributes = $("#txtPageDetAttributes").val();
////        //    oChanges.PageDetMetaData = $("#txtPageDetMetaData").data("kendoEditor").value();

////        //    oChanges.PageDetName = $("#txtPageDetName").val();
////        //    oChanges.PageDetDesc = $("#txtPageDetDesc").val();
////        //    oChanges.PageDetCaption = $("#txtPageDetCaption").val();
////        //    oChanges.PageDetCaptionLocal = $("#txtPageDetCaptionLocal").val();
////        //    oChanges.PageDetGroupSubTypeControl = ngl.intTryParse($("#txtPageDetGroupSubTypeControl").val(), 0);
////        //    oChanges.PageDetGroupTypeControl = ngl.intTryParse($("#txtPageDetGroupTypeControl").val(), 0);
////        //    oChanges.PageDetUserSecurityControl = ngl.intTryParse($("#txtPageDetUserSecurityControl").val(), 0);
////        //    oChanges.PageDetSequenceNo = ngl.intTryParse($("#txtPageDetSequenceNo").val(), 0);
////        //    oChanges.PageDetWidth = ngl.intTryParse($("#txtPageDetWidth").val(), 0);

////        //    oChanges.PageDetModDate = $("#txtPageDetModDate").val();
////        //    oChanges.PageDetModUser = $("#txtPageDetModUser").val();
////        //    oChanges.PageDetUpdated = $("#txtPageDetUpdated").val();

////        //    if (isEmpty(oChanges.PageDetName)) {
////        //        var title = "Save Page Detail Validation Failue"
////        //        var msg = "A name is required for the detail item!"
////        //        ngl.showValidationMsg("Save Page Detail Validation Failue", msg, tObj);
////        //    } else {
////        //        this.rData = null;
////        //        var oCRUDCtrl = new nglRESTCRUDCtrl();
////        //        var blnRet = oCRUDCtrl.update("cmPageDetail/PostPageDetail", oChanges, tObj, "saveSuccessCallback", "saveAjaxErrorCallback");
////        //    }
////        //} catch (err) {
////        //    ngl.showErrMsg(err.name, err.message, tObj);
////        //}
////    }

////    this.add = function () {
////        var tObj = this;

////        if (typeof (this.data) === 'undefined' || !ngl.isObject(this.data)) {
////            //something went wrong so throw an error. this should never happen
////            ngl.showValidationMsg("Dispatch Failed; No Data", "Please Contact Technical Support", tObj);
////            return;
////        }           

////        this.data.Origin.Name = $("#txtDispOrigName").data("kendoMaskedTextBox").value();
////        this.data.Origin.Contact.ContactName = $("#txtDispOrigContName").data("kendoMaskedTextBox").value();
////        this.data.Origin.Contact.ContactPhone = $("#txtDispOrigContPhone").data("kendoMaskedTextBox").value();
////        this.data.Origin.Contact.ContactEmail = $("#txtDispOrigContEmail").data("kendoMaskedTextBox").value();
////        this.data.Origin.Address1 = $("#txtDispOrigAddress1").data("kendoMaskedTextBox").value();
////        this.data.Requestor.Name = this.data.Origin.Name;
////        this.data.Requestor.Contact.ContactName = this.data.Origin.Contact.ContactName;
////        this.data.Requestor.Contact.ContactPhone = this.data.Origin.Contact.ContactPhone;
////        this.data.Requestor.Contact.ContactEmail = this.data.Origin.Contact.ContactEmail;
////        this.data.Requestor.Address1 = this.data.Origin.Address1;

////        this.data.Destination.Name = $("#txtDispDestName").data("kendoMaskedTextBox").value();
////        this.data.Destination.Contact.ContactName = $("#txtDispDestContName").data("kendoMaskedTextBox").value();
////        this.data.Destination.Contact.ContactPhone = $("#txtDispDestContPhone").data("kendoMaskedTextBox").value();
////        this.data.Destination.Contact.ContactEmail = $("#txtDispDestContEmail").data("kendoMaskedTextBox").value();
////        this.data.Destination.Address1 = $("#txtDispDestAddress1").data("kendoMaskedTextBox").value();
////        this.data.PickupDate = $("#txtDispPickupDate").data("kendoDatePicker").value();
////        //var sTime = ngl.getLocalTimeString($("#txtDispPickupStartTime").data("kendoTimePicker").value(),'08:00');
////        this.data.PickupStartTime = ngl.getLocalTimeString($("#txtDispPickupStartTime").data("kendoTimePicker").value(), '08:00');
////        //sTime = ngl.getLocalTimeString($("#txtDispPickupEndTime").data("kendoTimePicker").value(),'17:00');
////        this.data.PickupEndTime = ngl.getLocalTimeString($("#txtDispPickupEndTime").data("kendoTimePicker").value(), '17:00');
////        this.data.DeliveryDate = $("#txtDispDeliveryDate").data("kendoDatePicker").value();
////        this.data.DeliveryStartTime = ngl.getLocalTimeString($("#txtDispDeliveryStartTime").data("kendoTimePicker").value(),'08:00');
////        this.data.DeliveryEndTime = ngl.getLocalTimeString($("#txtDispDeliveryEndTime").data("kendoTimePicker").value(),'17:00');
////        this.data.BillOfLading = this.data.SHID; // $("#txtDispBillOfLading").data("kendoMaskedTextBox").value();
////        this.data.PONumber = $("#txtDispPONumber").data("kendoMaskedTextBox").value();
////        this.data.OrderNumber = $("#txtDispOrderNumber").data("kendoMaskedTextBox").value();
////        this.data.CarrierProNumber = $("#txtDispCarrierProNumber").data("kendoMaskedTextBox").value();
////        this.data.PickupNote = $("#txtDispPickupNote").data("kendoMaskedTextBox").value();
////        this.data.PickupNote = this.data.PickupNote.replace("&nbsp;", " ");
////        this.data.PickupNote = this.data.PickupNote.replace(/(<([^>]+)>)/ig, "");
////        this.data.DeliveryNote = $("#txtDispDeliveryNote").data("kendoMaskedTextBox").value();
////        this.data.DeliveryNote = this.data.DeliveryNote.replace("&nbsp;", " ");
////        this.data.DeliveryNote = this.data.DeliveryNote.replace(/(<([^>]+)>)/ig, "");
////        this.data.ConfidentialNote = $("#txtDispConfidentialNote").data("kendoMaskedTextBox").value();
////        this.data.ConfidentialNote = this.data.ConfidentialNote.replace(/(<([^>]+)>)/ig, "");
////        this.data.LoadTenderTransTypeControl = $("#txtLoadTenderTransTypeControl").data("kendoComboBox").value();
////        this.data.AutoAcceptOnDispatch = $("#swDispAutoAcceptOnDispatch").data("kendoSwitch").check(); 
////        this.data.EmailLoadTenderSheet = $("#swDispEmailLoadTenderSheet").data("kendoSwitch").check();

////        // Add Validation Logic for Required Fields
////        var oValidationResults = this.validateRequiredFields(this.data);
////        if (typeof (oValidationResults) === 'undefined' || !ngl.isObject(oValidationResults)) {            
////            ngl.showValidationMsg("Cannot validate Dispatch Information", "Invalid validation procedure, please contact technical support", tObj);
////            return;
////        } else {
////            if (oValidationResults.Success === false) {
////                ngl.Alert("Cannot Dispatch Data Validation Failure", oValidationResults.Message, 400, 400);
////                //ngl.showValidationMsg("Cannot Dispatch Data Validation Failure", oValidationResults.Message, tObj);
////                return;
////            }
////        }
////        //save the changes
////        //kendo.ui.progress($(document.body), true);
////        var windowWidget = $("#winDispatchDialog").data("kendoWindow");
////        kendo.ui.progress(windowWidget.element, true);
////        setTimeout(function (tObj) {
////                var oCRUDCtrl = new nglRESTCRUDCtrl();
////                if (oCRUDCtrl.create("Dispatching/Dispatch", tObj.data, tObj, "addSuccessCallback", "addAjaxErrorCallback") == false) {
////                    kendo.ui.progress(windowWidget.element, false);
////                }
////        }, 2000, tObj);
////        //var oCRUDCtrl = new nglRESTCRUDCtrl();
////        //setTimeout(oCRUDCtrl.create("Dispatching/Dispatch", this.data, tObj, "addSuccessCallback", "addAjaxErrorCallback"),2000)
////        //setTimeout(function (tObj) {
////        //        var oCRUDCtrl = new nglRESTCRUDCtrl();
////        //        if (oCRUDCtrl.create("Dispatching/Dispatch", tObj.data, tObj, "addSuccessCallback", "addAjaxErrorCallback") == false) {
////        //            kendo.ui.progress(windowWidget.element, false);
////        //        }
////        // }, 2000);

////        //    if (isEmpty(oChanges.PageDetName)) {
////        //        var title = "Save Page Detail Validation Failue"
////        //        var msg = "A name is required for the detail item!"
////        //        ngl.showValidationMsg("Save Page Detail Validation Failue", msg, tObj);
////        //    } else {
////        //        this.rData = null;
////        //        var oCRUDCtrl = new nglRESTCRUDCtrl();
////        //        if (oCRUDCtrl.create("Dispatching/Dispatch", this.data, tObj, "addSuccessCallback", "addAjaxErrorCallback")) { this.kendoWindowsObj.close(); }
////        //    }
////        //    //}
////        //} else {
////        //    ngl.showValidationMsg("Save Page Detail Validation Failue", "A Detail Type has not been selected", tObj);
////        //}
////    }

////    this.show = function () {
////        if (typeof (this.data) !== 'undefined' && ngl.isObject(this.data)) {
////            this.edit(this.data);
////        }

////        var tObj = this;

////        this.kendoWindowsObj = $("#winDispatchDialog").kendoWindow({
////            title: "Dispatch",
////            modal: true,
////            visible: false,
////            height: '75%',
////            width: '75%',
////            scrollable: true,
////            actions: ["print", "upload", "Minimize", "Maximize", "Close"],
////            close: function (e) { tObj.close(e); }
////        }).data("kendoWindow");

////        if (this.kendoWindowsObjUploadEventAdded === 0) {

////            //$("#winDispatchDialog").data("kendoWindow").wrapper.find(".k-svg-i-upload").parent().click(function (e) { e.preventDefault(); tObj.add(); });
////            this.kendoWindowsObj.wrapper.find(".k-svg-i-upload").parent().click(function (e) { e.preventDefault(); tObj.add(); });
////            this.kendoWindowsObj.wrapper.find(".k-svg-i-print").parent().click(function (e) { e.preventDefault(); tObj.print(); });

////            this.kendoWindowsObj.wrapper.find(".k-svg-i-book").parent().click(function (e) { e.preventDefault(); tObj.openContactWindow(); }); //Added By LVV on 2/19/2019 for bug fix add Carrier Contact selection to Dispatch
////        }
////        this.kendoWindowsObjUploadEventAdded = 1;

////        this.kendoWindowsObj.center().open();
////    }

////    this.read = function (intControl) {
////        var blnRet = false;
////        //if not supported just return false
////        //return blnRet;
////        var tObj = this;
////        if (typeof (intControl) != 'undefined' && intControl != null) {
////            this.rData = null;
////            this.data = null;
////            var oCRUDCtrl = new nglRESTCRUDCtrl();
////            blnRet = oCRUDCtrl.read("Dispatching/GetBidToDispatch", intControl, tObj, "readSuccessCallback", "readAjaxErrorCallback");
////        }
////        return blnRet;
////    }

////    this.edit = function (data) {
////       //load data to the screen
////         var tObj = this;
////        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
////            this.updateLoadSummaryData();
////            //load all the data from the Dispatch Object to the Screen object
////            $("#spProviderSCAC").html(data.ProviderSCAC);
////            $("#spVendorSCAC").html(data.VendorSCAC);
////            $("#spQuoteNumber").html(data.QuoteNumber);            
////            data.LinearFeet = this.calcLinearFt();
////            $("#spLinearFeet").html(data.LinearFeet);
////            $("#spLineHaul").html(ngl.currencyFormat(data.LineHaul));
////            $("#spFuel").html(ngl.currencyFormat(data.Fuel));
////            $("#spTotalCost").html(ngl.currencyFormat(data.TotalCost));
////            $("#txtDispOrigName").data("kendoMaskedTextBox").value(data.Origin.Name);
////            $("#txtDispOrigContName").data("kendoMaskedTextBox").value(data.Origin.Contact.ContactName);
////            $("#txtDispOrigContPhone").data("kendoMaskedTextBox").value(data.Origin.Contact.ContactPhone);
////            $("#txtDispOrigContEmail").data("kendoMaskedTextBox").value(data.Origin.Contact.ContactEmail);
////            $("#txtDispOrigAddress1").data("kendoMaskedTextBox").value(data.Origin.Address1);
////            $("#txtDispDestName").data("kendoMaskedTextBox").value(data.Destination.Name);
////            $("#txtDispDestContName").data("kendoMaskedTextBox").value(data.Destination.Contact.ContactName);
////            $("#txtDispDestContPhone").data("kendoMaskedTextBox").value(data.Destination.Contact.ContactPhone);
////            $("#txtDispDestContEmail").data("kendoMaskedTextBox").value(data.Destination.Contact.ContactEmail);
////            $("#txtDispDestAddress1").data("kendoMaskedTextBox").value(data.Destination.Address1);
////            $("#txtDispPickupDate").data("kendoDatePicker").value(data.PickupDate);
////            //if (data.PickupStartTime === null || ngl.isNullOrWhitespace(data.PickupStartTime)) { data.PickupStartTime = "08.00";}
////            $("#txtDispPickupStartTime").data("kendoTimePicker").value(ngl.getDateFromTimeString(data.PickupStartTime,data.PickupDate, '08:00'));
////            //if (data.PickupEndTime === null || ngl.isNullOrWhitespace(data.PickupEndTime)) { data.PickupEndTime = "16.00"; }
////            $("#txtDispPickupEndTime").data("kendoTimePicker").value(ngl.getDateFromTimeString(data.PickupEndTime, data.PickupDate, '17:00'));
////            $("#txtDispDeliveryDate").data("kendoDatePicker").value(data.DeliveryDate);
////            //if (data.DeliveryStartTime === null || ngl.isNullOrWhitespace(data.DeliveryStartTime)) { data.DeliveryStartTime = "08.00"; }
////            $("#txtDispDeliveryStartTime").data("kendoTimePicker").value(ngl.getDateFromTimeString(data.DeliveryStartTime, data.PickupDate, '08:00'));
////            //if (data.DeliveryEndTime === null || ngl.isNullOrWhitespace(data.DeliveryEndTime)) { data.DeliveryEndTime = "16.00"; }
////            $("#txtDispDeliveryEndTime").data("kendoTimePicker").value(ngl.getDateFromTimeString(data.DeliveryEndTime, data.PickupDate, '17:00'));
////            //$("#txtDispBillOfLading").data("kendoMaskedTextBox").value(data.BillOfLading);
////            $("#txtDispPONumber").data("kendoMaskedTextBox").value(data.PONumber);

////            $("#txtDispOrderNumber").data("kendoMaskedTextBox").value(data.OrderNumber);
////            $("#txtDispCarrierProNumber").data("kendoMaskedTextBox").value(data.CarrierProNumber);
////            $("#txtDispPickupNote").data("kendoMaskedTextBox").value(data.PickupNote);
////            $("#txtDispDeliveryNote").data("kendoMaskedTextBox").value(data.DeliveryNote);
////            $("#txtDispConfidentialNote").data("kendoMaskedTextBox").value(data.ConfidentialNote);
////            if (isEmpty(data.LoadTenderTransTypeControl) || data.LoadTenderTransTypeControl == "0") { data.LoadTenderTransTypeControl = 1;}
////            $("#txtLoadTenderTransTypeControl").data("kendoComboBox").value(data.LoadTenderTransTypeControl);
////            if (isEmpty(data.SHID) || data.SHID === "Quote" || data.SHID === "(Auto)") {
////                data.SHID = '';
////                $("#spDispSHID").html(data.SHID);
////                //Get a new SHID using the Get Next CNS REST Service
////                var oCRUDCtrl = new nglRESTCRUDCtrl();
////                var blnRet = oCRUDCtrl.read("Dispatching/GetNextCNSNbr", 0, tObj, "readSHIDSuccessCallback", "readSHIDAjaxErrorCallback")
////            } else {
////                $("#spDispSHID").html(data.SHID);
////            }

////            //Added By LVV on 2/19/2019 for bug fix add Carrier Contact selection to Dispatch
////            $("#spDispCarContName").html(data.CarrierContact.ContactName);
////            $("#spDispCarContTitle").html(data.CarrierContact.ContactTitle);
////            $("#spDispCarContEmail").html(data.CarrierContact.ContactEmail);
////            $("#spDispCarContPhone").html(data.CarrierContact.ContactPhone);
////            $("#spDispCarContPhoneExt").html(data.CarrierContact.ContactPhoneExt);
////            $("#spDispCarCont800").html(data.CarrierContact.Contact800);
////            $("#spDispCarContFax").html(data.CarrierContact.ContactFax);
////            $("#txtDispCarContControl").val(data.CarrierContact.ContactControl);
////            $("#txtDispCarContCarrierControl").val(data.CarrierContact.ContactCarrierControl);
////            $("#txtDispCarContLECarControl").val(data.CarrierContact.ContactLECarControl);
////            if (data.CarrierContact.ContactDefault) { $("#chkDispCarContDefault").prop('checked', true); } else { $("#chkDispCarContDefault").prop('checked', false); }
////            if (data.CarrierContact.ContactScheduler) { $("#chkDispCarContScheduler").prop('checked', true); } else { $("#chkDispCarContScheduler").prop('checked', false); }

////            $("#txtDispCarrierControl").val(data.CarrierControl);
////            var switchdataItem = data.AutoAcceptOnDispatch;
////            if (switchdataItem !== true) { switchdataItem = false; }
////            $("#swDispAutoAcceptOnDispatch").data("kendoSwitch").check(switchdataItem);
////            switchdataItem = data.EmailLoadTenderSheet;
////            if (switchdataItem !== true) { switchdataItem = false; }
////            $("#swDispEmailLoadTenderSheet").data("kendoSwitch").check(switchdataItem);

////        }

////    }

////    this.delete = function () {
////        //if not supported just return false
////        return ;
////        //var oChanges = new cmPageDetail();
////        //var tObj = this;
////        //try {
////        //    var sControl = ngl.intTryParse($("#txtPageDetControl").val(), 0);
////        //    if (typeof (sControl) !== 'undefined') {
////        //        oChanges.PageDetControl = sControl;
////        //    } else {
////        //        var msg = "This detail record does not have a valid primary key and cannot be deleted!"
////        //        ngl.showValidationMsg("Delete Page Detail Validation Failue", msg, tObj);
////        //        return;
////        //    }

////        //    //default values;
////        //    oChanges.PageDetOrientation = $("#txtPageDetOrientation").val();
////        //    oChanges.PageDetAllowFilter = $("#txtPageDetAllowFilter").val();
////        //    oChanges.PageDetFilterTypeControl = $("#txtPageDetFilterTypeControl").val
////        //    oChanges.PageDetAllowPaging = $("#txtPageDetAllowPaging").val();
////        //    oChanges.PageDetUserSecurityControl = ngl.intTryParse($("#txtPageDetUserSecurityControl").val(), 0);
////        //    oChanges.PageDetVisible = $("#txtPageDetVisible").prop("checked");
////        //    oChanges.PageDetReadOnly = $("#txtPageDetReadOnly").prop("checked");
////        //    oChanges.PageDetAllowSort = $("#txtPageDetAllowSort").prop("checked");
////        //    oChanges.PageDetPageTemplateControl = ngl.intTryParse($("#txtPageDetPageTemplateControl").val(), 0);
////        //    oChanges.PageDetExpanded = $("#txtPageDetExpanded").val();
////        //    oChanges.PageDetFKReference = $("#txtPageDetFKReference").val();

////        //    oChanges.PageDetPageControl = ngl.intTryParse($("#txtPageDetPageControl").val(), intPageControl);
////        //    oChanges.PageDetDataElmtControl = ngl.intTryParse($("#txtPageDetDataElmtControl").val(), 0);
////        //    oChanges.PageDetElmtFieldControl = ngl.intTryParse($("#txtPageDetElmtFieldControl").val(), 0);
////        //    oChanges.PageDetAPIReference = $("#txtPageDetAPIReference").val();
////        //    oChanges.PageDetAPIFilterID = $("#txtPageDetAPIFilterID").val();
////        //    oChanges.PageDetAPISortKey = $("#txtPageDetAPISortKey").val();
////        //    oChanges.PageDetParentID = ngl.intTryParse($("#txtPageDetParentID").val(), 0);

////        //    oChanges.PageDetTagIDReference = $("#txtPageDetTagIDReference").val();
////        //    oChanges.PageDetCSSClass = $("#txtPageDetCSSClass").val();
////        //    oChanges.PageDetAttributes = $("#txtPageDetAttributes").val();
////        //    oChanges.PageDetMetaData = $("#txtPageDetMetaData").data("kendoEditor").value();

////        //    oChanges.PageDetName = $("#txtPageDetName").val();
////        //    oChanges.PageDetDesc = $("#txtPageDetDesc").val();
////        //    oChanges.PageDetCaption = $("#txtPageDetCaption").val();
////        //    oChanges.PageDetCaptionLocal = $("#txtPageDetCaptionLocal").val();
////        //    oChanges.PageDetGroupSubTypeControl = ngl.intTryParse($("#txtPageDetGroupSubTypeControl").val(), 0);
////        //    oChanges.PageDetGroupTypeControl = ngl.intTryParse($("#txtPageDetGroupTypeControl").val(), 0);
////        //    oChanges.PageDetUserSecurityControl = ngl.intTryParse($("#txtPageDetUserSecurityControl").val(), 0);
////        //    oChanges.PageDetSequenceNo = ngl.intTryParse($("#txtPageDetSequenceNo").val(), 0);
////        //    oChanges.PageDetWidth = ngl.intTryParse($("#txtPageDetWidth").val(), 0);

////        //    oChanges.PageDetModDate = $("#txtPageDetModDate").val();
////        //    oChanges.PageDetModUser = $("#txtPageDetModUser").val();
////        //    oChanges.PageDetUpdated = $("#txtPageDetUpdated").val();

////        //    this.rData = null;
////        //    var oCRUDCtrl = new nglRESTCRUDCtrl();
////        //    var blnRet = oCRUDCtrl.update("cmPageDetail/PostDelete", oChanges, tObj, "saveSuccessCallback", "saveAjaxErrorCallback");

////        //} catch (err) {
////        //    ngl.showErrMsg(err.name, err.message, tObj);
////        //}

////    }

////    //Note:  this method never gets called because we do not have a callback method for the kendoWindow
////    this.close = function (e) {
////        //var oResults = new nglEventParameters();
////        //oResults.source = "close";
////        //oResults.widget = this;
////        //oResults.msg = 'closing nothing is saved';

////        //if (typeof (this.onClose) === "function") {
////        //    this.onClose(oResults);
////        //}
////    }

////    //loadDefaults sets up the callbacks cbSelect and cbSave
////    //all call backs return a reference to this object and a string message as parameters
////    this.loadDefaults = function (pageVariable, selectCallBack, saveCallBack, closeCallBack, readCallBack) {

////        this.onSelect = selectCallBack;
////        this.onSave = saveCallBack;
////        this.onClose = closeCallBack;
////        this.onRead = readCallBack;
////        this.data = new Dispatch();
////        this.lastErr = "";
////        this.notifyOnError = true;
////        this.notifyOnSuccess = true;
////        this.notifyOnValidationFailure = true;
////        var tObj = this;

////        if (typeof (pageVariable) !== 'undefined' && ngl.isObject(pageVariable)) {
////            //this is the add new popup window
////            this.kendoWindowsObj = pageVariable;
////            //$("#btnSaveNewPageDetailtop").click(function () { tObj.add(); })
////            //$("#btnSaveNewDetailbot").click(function () { tObj.add(); })
////            //$("#txtNewPageDetName").kendoMaskedTextBox();
////            //$("#txtNewPageDetParentID").kendoDropDownList();
////            //$("#txtNewPageDetGroupSubTypeControl").kendoDropDownList();
////            $("#txtLoadTenderTransTypeControl").kendoComboBox({
////                index: 0,
////                dataTextField: "Name",
////                dataValueField: "Control",
////                dataSource: nglvLookupEditors.dsLoadTenderTransType,
////                change: function (e) { this.data.LoadTenderTransTypeControl = e.value; }
////            });

////            $("#txtDispOrigName").kendoMaskedTextBox();
////            $("#txtDispOrigContName").kendoMaskedTextBox();
////            $("#txtDispOrigContPhone").kendoMaskedTextBox({ mask: ngl.getPhoneNumberMask() });
////            $("#txtDispOrigContEmail").kendoMaskedTextBox();
////            $("#txtDispOrigAddress1").kendoMaskedTextBox();

////            $("#txtDispDestName").kendoMaskedTextBox();
////            $("#txtDispDestContName").kendoMaskedTextBox();
////            $("#txtDispDestContPhone").kendoMaskedTextBox({ mask: ngl.getPhoneNumberMask() });
////            $("#txtDispDestContEmail").kendoMaskedTextBox();
////            $("#txtDispDestAddress1").kendoMaskedTextBox();

////            $("#txtDispPickupDate").kendoDatePicker();
////            $("#txtDispPickupStartTime").kendoTimePicker({
////                format: "HH:mm ",
////                interval: 15
////            });
////            $("#txtDispPickupEndTime").kendoTimePicker({
////                format: "HH:mm ",
////                interval: 15
////            });

////            $("#txtDispDeliveryDate").kendoDatePicker();
////            $("#txtDispDeliveryStartTime").kendoTimePicker({
////                format: "HH:mm ",
////                interval: 15
////            });
////            $("#txtDispDeliveryEndTime").kendoTimePicker({
////                format: "HH:mm ",
////                interval: 15
////            });

////            //$("#txtDispBillOfLading").kendoMaskedTextBox();
////            $("#txtDispPONumber").kendoMaskedTextBox();
////            $("#txtDispOrderNumber").kendoMaskedTextBox();
////            $("#txtDispCarrierProNumber").kendoMaskedTextBox();           
////            $("#txtDispPickupNote").kendoMaskedTextBox();
////            $("#txtDispDeliveryNote").kendoMaskedTextBox();
////            $("#txtDispConfidentialNote").kendoMaskedTextBox();

////            $("#swDispAutoAcceptOnDispatch").kendoSwitch(); 
////            $("#swDispEmailLoadTenderSheet").kendoSwitch();
////            //Added By LVV on 2/19/2019 for bug fix add Carrier Contact selection to Dispatch
////            this.oSelectContactCtrl = new SelectContactCtrl();
////            this.oSelectContactCtrl.loadDefaults(pageVariable, this);

////        } else {
////            this.kendoWindowsObj = null;
////        }
////    }
////}


// Dispatching Dialog Ctrl requires Views/DispatchingDialog.html
function DispatchingDialogCtrl() {
    //Generic properties for all Widgets
    this.lastErr = "";
    this.notifyOnError = true;
    this.notifyOnSuccess = true;
    this.notifyOnValidationFailure = true;
    data: Dispatch;
    rData: null;
    onSelect: null;
    onSave: null;
    onClose: null;
    onRead: null;
    dSource: null;
    sourceDiv: null;
    kendoWindowsObj: null;
    oSelectContactCtrl: null; //Added By LVV on 2/19/2019 for bug fix add Carrier Contact selection to Dispatch
    //Widget specific properties

    this.kendoWindowsObjUploadEventAdded = 0;

    //Widget specific functions
    this.calcLinearFt = function () {
        var iRet = 0;
        if (typeof (this.data) !== 'undefined' && ngl.isObject(this.data)) {
            if (typeof (this.data.Items) !== 'undefined' && ngl.isArray(this.data.Items) && this.data.Items.length > 0) {
                var TotalLen = 0;
                for (i = 0, len = this.data.Items.length; i < len; i++) {
                    var oItem = this.data.Items[i];
                    var iLen = this.data.Items[i].ItemLength
                    if (typeof (iLen) !== 'undefined' && iLen !== null && !isNaN(iLen) && iLen > 0) {
                        TotalLen += iLen;
                    }

                }
                if (TotalLen !== null && !isNaN(TotalLen) && TotalLen > 0) {
                    switch (this.data.LengthUnit.toUpperCase()) {
                        case "IN":
                            iRet = ngl.convertINtoLinearFeet(TotalLen);
                            break;
                        case "CM":
                            iRet = ngl.convertCMtoLinearFeet(TotalLen);
                            break;
                        case "FT":
                            iRet = TotalLen;
                            break;
                        case "M":
                            iRet = ngl.convertMtoLinearFeet(TotalLen);
                            break;
                        default:
                            iRet = 0;
                    }
                }
            }

        }
        return iRet;
    }

    this.updateLoadSummaryData = function () {
        if (typeof (this.data) !== 'undefined' && ngl.isObject(this.data)) {

            var orig = $("#spDispOrigin");
            var sVal = "";
            sVal = sVal.concat(this.data.Origin.Name, '<br />', this.data.Origin.Address1, '<br />', this.data.Origin.City, " ", this.data.Origin.State, "  ", this.data.Origin.Zip);
            orig.html(sVal);
            var details = $("#spDispShipDetails");
            sVal = "";
            sVal = sVal.concat("To Load: ", kendo.toString(kendo.parseDate(this.data.PickupDate, 'yyyy-MM-dd'), 'MM/dd/yyyy'), '<br />', "To Deliver: ", kendo.toString(kendo.parseDate(this.data.DeliveryDate, 'yyyy-MM-dd'), 'MM/dd/yyyy'));
            details.html(sVal);

            var shipWgt = $("#spDispShipWgt").html(this.data.TotalWgt);
            var shipQty = $("#spDispShipQty").html(this.data.TotalQty);
            var shipPlts = $("#spDispShipPlts").html(this.data.TotalPlts);

            var dest = $("#spDispDest");
            sVal = "";
            sVal = sVal.concat(this.data.Destination.Name, '<br />', this.data.Destination.Address1, '<br />', this.data.Destination.City, " ", this.data.Destination.State, "  ", this.data.Destination.Zip);
            //sVal = string.concat($("#txtdestCompName").data("kendoMaskedTextBox").value(),'<br />',getDestPC());
            dest.html(sVal);
        }
    }

    //Added By LVV on 2/19/2019 for bug fix add Carrier Contact selection to Dispatch
    this.updateCarrierCont = function (item) {
        if (typeof (item) !== 'undefined' && ngl.isObject(item)) {
            if (typeof (this.data) !== 'undefined' && ngl.isObject(this.data)) {
                this.data.CarrierContact.ContactControl = item.ContactControl;
                this.data.CarrierContact.ContactName = item.ContactName;
                this.data.CarrierContact.ContactTitle = item.ContactTitle;
                this.data.CarrierContact.ContactPhone = item.ContactPhone;
                this.data.CarrierContact.ContactPhoneExt = item.ContactPhoneExt;
                this.data.CarrierContact.ContactFax = item.ContactFax;
                this.data.CarrierContact.Contact800 = item.Contact800;
                this.data.CarrierContact.ContactEmail = item.ContactEmail;
                this.data.CarrierContact.ContactDefault = item.ContactDefault;
                this.data.CarrierContact.ContactTender = item.ContactTender;
                this.data.CarrierContact.ContactScheduler = item.ContactScheduler;
                this.data.CarrierContact.ContactCarrierControl = item.ContactCarrierControl;
                this.data.CarrierContact.ContactLECarControl = item.ContactLECarControl;
                this.data.CarrierContact.ContactCompControl = item.ContactCompControl;

                $("#spDispCarContName").html(item.ContactName);
                $("#spDispCarContTitle").html(item.ContactTitle);
                $("#spDispCarContEmail").html(item.ContactEmail);
                $("#spDispCarContPhone").html(item.ContactPhone);
                $("#spDispCarContPhoneExt").html(item.ContactPhoneExt);
                $("#spDispCarCont800").html(item.Contact800);
                $("#spDispCarContFax").html(item.ContactFax);
                $("#txtDispCarContControl").val(item.ContactControl);
                $("#txtDispCarContCarrierControl").val(item.ContactCarrierControl);
                $("#txtDispCarContLECarControl").val(item.ContactLECarControl);
                if (item.ContactDefault) { $("#chkDispCarContDefault").prop('checked', true); } else { $("#chkDispCarContDefault").prop('checked', false); }
                if (item.ContactScheduler) { $("#chkDispCarContScheduler").prop('checked', true); } else { $("#chkDispCarContScheduler").prop('checked', false); }
            }
        }
    }

    //Added By LVV on 2/19/2019 for bug fix add Carrier Contact selection to Dispatch
    this.openContactWindow = function () {

        this.oSelectContactCtrl.SelectedContactCarrierControl = 0; //reset the value

        this.oSelectContactCtrl.SelectedContactCarrierControl = this.data.CarrierControl;

        this.oSelectContactCtrl.read(0);
    }


    this.addSuccessCallback = function (data) {
        var windowWidget = $("#winDispatchDialog").data("kendoWindow");
        kendo.ui.progress(windowWidget.element, false);
        var oResults = new nglEventParameters();
        oResults.source = "addSuccessCallback";
        var tObj = this;
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed   
        //kendo.ui.progress($(document.body), false);
        this.rData = null;
        try {
            var blnSuccess = false;
            var blnErrorShown = false;
            var strValidationMsg = "";
            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                    oResults.error = new Error();
                    oResults.error.name = "Dispatch Failure";
                    oResults.error.message = data.Errors;
                    blnErrorShown = true;
                    if (data.StatusCode == 400) {
                        var wdth = ($(window).width() / 10) * 6;
                        var hgt = ($(window).height() / 10) * 6;

                        ngl.Alert("Dispatch Failure", data.Errors, wdth, hgt);
                    } else {
                        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                    }
                }
                else {
                    this.rData = data.Data;
                    if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                        if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                            blnSuccess = true;
                            oResults.data = data.Data;
                            oResults.datatype = "string";
                            oResults.msg = "<h3>Success</h3>" + data.Data[0].ErrorMessage
                            this.kendoWindowsObj.close();
                            //Add Code here to Parse the new data object and display the results using the 
                            //new dispatch report control                            
                            //ngl.Alert('Dispatch Results',oResults.msg,400,400);
                            //ngl.showSuccessMsg(oResults.msg, tObj);
                        }
                    }

                }
            }
            if (blnSuccess === false && blnErrorShown === false) {
                oResults.error = new Error();
                oResults.error.name = "Dispatch Failure";
                oResults.error.message = "No results are available.";
                ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
            }
        } catch (err) {
            oResults.error = err
        }
        if (ngl.isFunction(this.onSave)) {
            this.onSave(oResults);
        }

    }

    this.addAjaxErrorCallback = function (xhr, textStatus, error) {
        var windowWidget = $("#winDispatchDialog").data("kendoWindow");
        kendo.ui.progress(windowWidget.element, false);
        var oResults = new nglEventParameters();
        var tObj = this;
        //kendo.ui.progress($(document.body), false);
        oResults.source = "addAjaxErrorCallback";
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed  
        oResults.error = new Error();
        oResults.error.name = "Dispatch Load Failure"
        oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
        //this.kendoWindowsObj.center().open();

        if (ngl.isFunction(this.onSave)) {
            this.onSave(oResults);
        }
    }

    this.readSHIDSuccessCallback = function (response) {
        var oResults = new nglEventParameters();
        var tObj = this;
        oResults.source = "readSHIDSuccessCallback";
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed   
        //clear any old return data in rData
        this.rData = null;
        try {
            var blnSuccess = false;
            var blnErrorShown = false;
            var strValidationMsg = "";
            if (typeof (response) !== 'undefined' && ngl.isObject(response)) {
                if (typeof (response.Errors) !== 'undefined' && response.Errors != null && response.Errors.length > 0) {
                    var sErrMsg = "Please Close the Window and try again.  The actual error is: " + response.Errors;
                    ngl.showErrMsg("Get Shipment ID Failure", sErrMsg, tObj);
                }
                else {
                    if (typeof (response.Data) !== 'undefined' && ngl.isArray(response.Data)) {
                        this.rData = response.Data;
                        if (typeof (this.data) !== 'undefined' && ngl.isObject(this.data)) {
                            this.data.SHID = this.rData[0];
                            $("#spDispSHID").html(this.data.SHID);
                        }
                    }
                }
            }
        } catch (err) {
            var sErrMsg = "Please Close the Window and try again.  The actual error is: " + err.description;
            ngl.showErrMsg("Get Shipment ID Failure", sErrMsg, tObj);
        }

    }

    this.readSHIDAjaxErrorCallback = function (xhr, textStatus, error) {
        var tObj = this;
        var sErrMsg = "Please Close the Window and try again.  The actual error is: " + formatAjaxJSONResponsMsgs(xhr, textStatus, error, 'Failed to get the next SHID');
        ngl.showErrMsg("Get Shipment ID Failure", sErrMsg, tObj);
    }

    //Generic Call back functions for all Widgets
    this.saveSuccessCallback = function (data) {

        //debugger;
        var oResults = new nglEventParameters();
        oResults.source = "saveSuccessCallback";
        var tObj = this;
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed     
        this.rData = null;
        try {
            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                if (data.Errors != null) {
                    oResults.error = new Error();
                    if (data.StatusCode === 203) {
                        oResults.error.name = "Authorization Timeout";
                        oResults.error.message = data.Errors;
                    }
                    else {
                        oResults.error.name = "Access Denied";
                        oResults.error.message = data.Errors;
                    }
                    ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                }
                else {
                    this.rData = data.Data;
                    if (data.Data != null) {
                        oResults.data = data.Data;
                        this.edit(data.Data)
                        oResults.datatype = "cmPageDetail";
                        oResults.msg = "Success." + "  Your changes have been saved."
                        ngl.showSuccessMsg(oResults.msg, tObj);
                    }
                    else {
                        oResults.error = new Error();
                        oResults.error.name = "Invalid Request";
                        oResults.error.message = "No Data available.";
                        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                    }
                }
            } else {
                oResults.msg = "Success no data returned";
                ngl.showSuccessMsg(oResults.msg, tObj);
            }
        } catch (err) {
            oResults.error = err
        }
        if (ngl.isFunction(this.onSave)) {
            this.onSave(oResults);
        }

    }

    this.saveAjaxErrorCallback = function (xhr, textStatus, error) {
        var oResults = new nglEventParameters();
        var tObj = this;
        oResults.source = "saveAjaxErrorCallback";
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed  
        oResults.error = new Error();
        oResults.error.name = "Save Page Detail Failure"
        oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);

        if (ngl.isFunction(this.onSave)) {
            this.onSave(oResults);
        }
    }


    this.readSuccessCallback = function (data) {
        //var windowWidget = $("#winDispatchDialog").data("kendoWindow");      
        //kendo.ui.progress(windowWidget.element, false);
        var oResults = new nglEventParameters();
        var tObj = this;
        oResults.source = "readSuccessCallback";
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed   
        //clear any old return data in rData
        this.rData = null;
        try {
            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                if (typeof (data.Errors) !== 'undefined' && data.Errors != null) {
                    oResults.error = new Error();
                    oResults.error.name = "Read Dispatch Data Failure";
                    oResults.error.message = data.Errors;
                    ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                }
                else {
                    this.rData = data.Data;
                    if (data.Data != null) {
                        oResults.data = data.Data;
                        oResults.datatype = "Dispatch";
                        oResults.msg = "Success"
                        this.data = data.Data[0];
                        //this.show();
                    }
                    else {
                        oResults.error = new Error();
                        oResults.error.name = "Invalid Request";
                        oResults.error.message = "No Data available.";
                        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                    }
                }
            } else {
                oResults.msg = "Success but no data was returned. Please refresh your page and try again.";
                ngl.showSuccessMsg(oResults.msg, tObj);
            }
        } catch (err) {
            oResults.error = err
        }
        if (ngl.isFunction(this.onRead)) {
            this.onRead(oResults);
        }
        if (oResults.msg == "Success") {
            this.show();
        }

    }

    this.readAjaxErrorCallback = function (xhr, textStatus, error) {
        //var windowWidget = $("#winDispatchDialog").data("kendoWindow");
        //kendo.ui.progress(windowWidget.element, false);
        var oResults = new nglEventParameters();
        var tObj = this;
        oResults.source = "readAjaxErrorCallback";
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed  
        oResults.error = new Error();
        oResults.error.name = "Read Dispatch Data Failure"
        oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);

        if (ngl.isFunction(this.onRead)) {
            this.onRead(oResults);
        }
    }

    // Generic functions for all Widgets

    this.validateRequiredFields = function (data) {
        var oRet = new validationResults();
        if (typeof (data) === 'undefined' || !ngl.isObject(data)) {
            //something went wrong so throw an error. this should never happen
            ngl.showValidationMsg("Dispatch Validation Failed; No Data", "Please Contact Technical Support", tObj);
            oRet.Message = "Dispatch Validation Failed; No Data";
            return oRet;
        }

        var blnValidated = true;
        var sValidationMsg = "";
        var sSpacer = "";
        if (isEmpty(data.Origin.Name)) { blnValidated = false; sValidationMsg += sSpacer + " Origin Location Name is Required. "; sSpacer = " And "; }

        if (isEmpty(data.Origin.Contact.ContactName)) { blnValidated = false; sValidationMsg += sSpacer + " Origin Contact Name is Required. "; sSpacer = " And "; }
        if (isEmpty(data.Origin.Contact.ContactPhone)) { blnValidated = false; sValidationMsg += sSpacer + " Origin Contact Phone is Required. "; sSpacer = " And "; }
        if (isEmpty(data.Origin.Contact.ContactEmail)) { blnValidated = false; sValidationMsg += sSpacer + " Origin Contact Email is Required. "; sSpacer = " And "; }

        if (isEmpty(data.Origin.Address1)) { blnValidated = false; sValidationMsg += sSpacer + " Origin Street Addrtess is Required. "; sSpacer = " And "; }

        if (isEmpty(data.Destination.Name)) { blnValidated = false; sValidationMsg += sSpacer + " Destination Location Name is Required. "; sSpacer = " And "; }
        if (isEmpty(data.Destination.Contact.ContactName)) { blnValidated = false; sValidationMsg += sSpacer + " Destination Contact Name is Required. "; sSpacer = " And "; }
        if (isEmpty(data.Destination.Contact.ContactPhone)) { blnValidated = false; sValidationMsg += sSpacer + " Destination Contact Phone is Required. "; sSpacer = " And "; }
        if (isEmpty(data.Destination.Contact.ContactEmail)) { blnValidated = false; sValidationMsg += sSpacer + " Destination Contact Email is Required. "; sSpacer = " And "; }

        if (isEmpty(data.Destination.Address1)) { blnValidated = false; sValidationMsg += sSpacer + " Destination Street Addrtess  is Required. "; sSpacer = " And "; }
        //debugger;
        if (isEmpty(data.PickupDate)) { blnValidated = false; sValidationMsg += sSpacer + " Pickup Date is Required. "; sSpacer = " And "; }
        if (isEmpty(data.PickupStartTime)) { blnValidated = false; sValidationMsg += sSpacer + " Pickup Start Time is Required. "; sSpacer = " And "; }
        if (isEmpty(data.PickupEndTime)) { blnValidated = false; sValidationMsg += sSpacer + " Pickup End Time is Required. "; sSpacer = " And "; }
        if (isEmpty(data.DeliveryDate)) { blnValidated = false; sValidationMsg += sSpacer + " Delivery Date is Required. "; sSpacer = " And "; }
        if (isEmpty(data.DeliveryStartTime)) { blnValidated = false; sValidationMsg += sSpacer + " Delivery Start Time is Required. "; sSpacer = " And "; }
        if (isEmpty(data.DeliveryEndTime)) { blnValidated = false; sValidationMsg += sSpacer + " Delivery End Time is Required. "; sSpacer = " And "; }
        oRet.Success = blnValidated;
        oRet.Message = sValidationMsg;


        return oRet;
    }

    this.print = function () {
        var kinWin = $("#winDispatchDialog").kendoWindow().data("kendoWindow");
        var winWrapper = kinWin.wrapper;
        winWrapper.removeClass("k-window");
        winWrapper.addClass("printable");
        //put pring code here
        window.print();
        winWrapper.removeClass("printable");
        winWrapper.addClass("k-window");

    }
    // Generic CRUD functions for all Widgets
    this.save = function () {
        return;
        //var oChanges = new cmPageDetail();
        //var tObj = this;
        //try {
        //    var sControl = ngl.intTryParse($("#txtPageDetControl").val(), 0);
        //    if (typeof (sControl) !== 'undefined') {
        //        oChanges.PageDetControl = sControl;
        //    } else {
        //        oChanges.PageDetControl = 0;
        //    }

        //    //default values;
        //    oChanges.PageDetOrientation = $("#txtPageDetOrientation").val();
        //    oChanges.PageDetAllowFilter = $("#txtPageDetAllowFilter").prop("checked");
        //    oChanges.PageDetFilterTypeControl = $("#txtPageDetFilterTypeControl").val();
        //    oChanges.PageDetAllowPaging = $("#txtPageDetAllowPaging").val();
        //    oChanges.PageDetUserSecurityControl = ngl.intTryParse($("#txtPageDetUserSecurityControl").val(), 0);
        //    oChanges.PageDetVisible = $("#txtPageDetVisible").prop("checked");
        //    oChanges.PageDetReadOnly = $("#txtPageDetReadOnly").prop("checked");
        //    oChanges.PageDetAllowSort = $("#txtPageDetAllowSort").prop("checked");
        //    oChanges.PageDetPageTemplateControl = ngl.intTryParse($("#txtPageDetPageTemplateControl").val(), 0);
        //    oChanges.PageDetExpanded = $("#txtPageDetExpanded").val();
        //    oChanges.PageDetFKReference = $("#txtPageDetFKReference").val();

        //    oChanges.PageDetPageControl = ngl.intTryParse($("#txtPageDetPageControl").val(), intPageControl);
        //    oChanges.PageDetDataElmtControl = ngl.intTryParse($("#txtPageDetDataElmtControl").val(), 0);
        //    oChanges.PageDetElmtFieldControl = ngl.intTryParse($("#txtPageDetElmtFieldControl").val(), 0);
        //    oChanges.PageDetAPIReference = $("#txtPageDetAPIReference").val();
        //    oChanges.PageDetAPIFilterID = $("#txtPageDetAPIFilterID").val();
        //    oChanges.PageDetAPISortKey = $("#txtPageDetAPISortKey").val();
        //    oChanges.PageDetParentID = ngl.intTryParse($("#txtPageDetParentID").val(), 0);

        //    oChanges.PageDetTagIDReference = $("#txtPageDetTagIDReference").val();
        //    oChanges.PageDetCSSClass = $("#txtPageDetCSSClass").val();
        //    oChanges.PageDetAttributes = $("#txtPageDetAttributes").val();
        //    oChanges.PageDetMetaData = $("#txtPageDetMetaData").data("kendoEditor").value();

        //    oChanges.PageDetName = $("#txtPageDetName").val();
        //    oChanges.PageDetDesc = $("#txtPageDetDesc").val();
        //    oChanges.PageDetCaption = $("#txtPageDetCaption").val();
        //    oChanges.PageDetCaptionLocal = $("#txtPageDetCaptionLocal").val();
        //    oChanges.PageDetGroupSubTypeControl = ngl.intTryParse($("#txtPageDetGroupSubTypeControl").val(), 0);
        //    oChanges.PageDetGroupTypeControl = ngl.intTryParse($("#txtPageDetGroupTypeControl").val(), 0);
        //    oChanges.PageDetUserSecurityControl = ngl.intTryParse($("#txtPageDetUserSecurityControl").val(), 0);
        //    oChanges.PageDetSequenceNo = ngl.intTryParse($("#txtPageDetSequenceNo").val(), 0);
        //    oChanges.PageDetWidth = ngl.intTryParse($("#txtPageDetWidth").val(), 0);

        //    oChanges.PageDetModDate = $("#txtPageDetModDate").val();
        //    oChanges.PageDetModUser = $("#txtPageDetModUser").val();
        //    oChanges.PageDetUpdated = $("#txtPageDetUpdated").val();

        //    if (isEmpty(oChanges.PageDetName)) {
        //        var title = "Save Page Detail Validation Failue"
        //        var msg = "A name is required for the detail item!"
        //        ngl.showValidationMsg("Save Page Detail Validation Failue", msg, tObj);
        //    } else {
        //        this.rData = null;
        //        var oCRUDCtrl = new nglRESTCRUDCtrl();
        //        var blnRet = oCRUDCtrl.update("cmPageDetail/PostPageDetail", oChanges, tObj, "saveSuccessCallback", "saveAjaxErrorCallback");
        //    }
        //} catch (err) {
        //    ngl.showErrMsg(err.name, err.message, tObj);
        //}
    }

    this.add = function () {
        var tObj = this;

        if (typeof (this.data) === 'undefined' || !ngl.isObject(this.data)) {
            //something went wrong so throw an error. this should never happen
            ngl.showValidationMsg("Dispatch Failed; No Data", "Please Contact Technical Support", tObj);
            return;
        }

        this.data.Origin.Name = $("#txtDispOrigName").data("kendoMaskedTextBox").value();
        this.data.Origin.Contact.ContactName = $("#txtDispOrigContName").data("kendoMaskedTextBox").value();
        this.data.Origin.Contact.ContactPhone = $("#txtDispOrigContPhone").data("kendoMaskedTextBox").value();
        this.data.Origin.Contact.ContactEmail = $("#txtDispOrigContEmail").data("kendoMaskedTextBox").value();
        this.data.Origin.Address1 = $("#txtDispOrigAddress1").data("kendoMaskedTextBox").value();
        this.data.Requestor.Name = this.data.Origin.Name;
        this.data.Requestor.Contact.ContactName = this.data.Origin.Contact.ContactName;
        this.data.Requestor.Contact.ContactPhone = this.data.Origin.Contact.ContactPhone;
        this.data.Requestor.Contact.ContactEmail = this.data.Origin.Contact.ContactEmail;
        this.data.Requestor.Address1 = this.data.Origin.Address1;

        this.data.Destination.Name = $("#txtDispDestName").data("kendoMaskedTextBox").value();
        this.data.Destination.Contact.ContactName = $("#txtDispDestContName").data("kendoMaskedTextBox").value();
        this.data.Destination.Contact.ContactPhone = $("#txtDispDestContPhone").data("kendoMaskedTextBox").value();
        this.data.Destination.Contact.ContactEmail = $("#txtDispDestContEmail").data("kendoMaskedTextBox").value();
        this.data.Destination.Address1 = $("#txtDispDestAddress1").data("kendoMaskedTextBox").value();
        this.data.PickupDate = $("#txtDispPickupDate").data("kendoDatePicker").value();
        //var sTime = ngl.getLocalTimeString($("#txtDispPickupStartTime").data("kendoTimePicker").value(),'08:00');
        this.data.PickupStartTime = ngl.getLocalTimeString($("#txtDispPickupStartTime").data("kendoTimePicker").value(), '08:00');
        //sTime = ngl.getLocalTimeString($("#txtDispPickupEndTime").data("kendoTimePicker").value(),'17:00');
        this.data.PickupEndTime = ngl.getLocalTimeString($("#txtDispPickupEndTime").data("kendoTimePicker").value(), '17:00');
        this.data.DeliveryDate = $("#txtDispDeliveryDate").data("kendoDatePicker").value();
        this.data.DeliveryStartTime = ngl.getLocalTimeString($("#txtDispDeliveryStartTime").data("kendoTimePicker").value(), '08:00');
        this.data.DeliveryEndTime = ngl.getLocalTimeString($("#txtDispDeliveryEndTime").data("kendoTimePicker").value(), '17:00');
        this.data.BillOfLading = this.data.SHID; // $("#txtDispBillOfLading").data("kendoMaskedTextBox").value();
        this.data.PONumber = $("#txtDispPONumber").data("kendoMaskedTextBox").value();
        this.data.OrderNumber = $("#txtDispOrderNumber").data("kendoMaskedTextBox").value();
        this.data.CarrierProNumber = $("#txtDispCarrierProNumber").data("kendoMaskedTextBox").value();
        this.data.PickupNote = $("#txtDispPickupNote").data("kendoMaskedTextBox").value();
        this.data.PickupNote = this.data.PickupNote.replace("&nbsp;", " ");
        this.data.PickupNote = this.data.PickupNote.replace(/(<([^>]+)>)/ig, "");
        this.data.DeliveryNote = $("#txtDispDeliveryNote").data("kendoMaskedTextBox").value();
        this.data.DeliveryNote = this.data.DeliveryNote.replace("&nbsp;", " ");
        this.data.DeliveryNote = this.data.DeliveryNote.replace(/(<([^>]+)>)/ig, "");
        this.data.ConfidentialNote = $("#txtDispConfidentialNote").data("kendoMaskedTextBox").value();
        this.data.ConfidentialNote = this.data.ConfidentialNote.replace(/(<([^>]+)>)/ig, "");
        this.data.LoadTenderTransTypeControl = $("#txtLoadTenderTransTypeControl").data("kendoComboBox").value();
        this.data.AutoAcceptOnDispatch = $("#swDispAutoAcceptOnDispatch").data("kendoSwitch").check();
        this.data.EmailLoadTenderSheet = $("#swDispEmailLoadTenderSheet").data("kendoSwitch").check();

        // Add Validation Logic for Required Fields
        var oValidationResults = this.validateRequiredFields(this.data);
        if (typeof (oValidationResults) === 'undefined' || !ngl.isObject(oValidationResults)) {
            ngl.showValidationMsg("Cannot validate Dispatch Information", "Invalid validation procedure, please contact technical support", tObj);
            return;
        } else {
            if (oValidationResults.Success === false) {
                ngl.Alert("Cannot Dispatch Data Validation Failure", oValidationResults.Message, 400, 400);
                //ngl.showValidationMsg("Cannot Dispatch Data Validation Failure", oValidationResults.Message, tObj);
                return;
            }
        }
        //save the changes
        //kendo.ui.progress($(document.body), true);
        var windowWidget = $("#winDispatchDialog").data("kendoWindow");
        kendo.ui.progress(windowWidget.element, true);
        setTimeout(function (tObj) {
            var oCRUDCtrl = new nglRESTCRUDCtrl();
            if (oCRUDCtrl.create("Dispatching/Dispatch", tObj.data, tObj, "addSuccessCallback", "addAjaxErrorCallback") == false) {
                kendo.ui.progress(windowWidget.element, false);
            }
        }, 2000, tObj);
        //var oCRUDCtrl = new nglRESTCRUDCtrl();
        //setTimeout(oCRUDCtrl.create("Dispatching/Dispatch", this.data, tObj, "addSuccessCallback", "addAjaxErrorCallback"),2000)
        //setTimeout(function (tObj) {
        //        var oCRUDCtrl = new nglRESTCRUDCtrl();
        //        if (oCRUDCtrl.create("Dispatching/Dispatch", tObj.data, tObj, "addSuccessCallback", "addAjaxErrorCallback") == false) {
        //            kendo.ui.progress(windowWidget.element, false);
        //        }
        // }, 2000);

        //    if (isEmpty(oChanges.PageDetName)) {
        //        var title = "Save Page Detail Validation Failue"
        //        var msg = "A name is required for the detail item!"
        //        ngl.showValidationMsg("Save Page Detail Validation Failue", msg, tObj);
        //    } else {
        //        this.rData = null;
        //        var oCRUDCtrl = new nglRESTCRUDCtrl();
        //        if (oCRUDCtrl.create("Dispatching/Dispatch", this.data, tObj, "addSuccessCallback", "addAjaxErrorCallback")) { this.kendoWindowsObj.close(); }
        //    }
        //    //}
        //} else {
        //    ngl.showValidationMsg("Save Page Detail Validation Failue", "A Detail Type has not been selected", tObj);
        //}
    }

    this.show = function () {
        if (typeof (this.data) !== 'undefined' && ngl.isObject(this.data)) {
            this.edit(this.data);
        }

        var tObj = this;

        this.kendoWindowsObj = $("#winDispatchDialog").kendoWindow({
            title: "Dispatch",
            modal: true,
            visible: false,
            height: '75%',
            width: '75%',
            scrollable: true,
            actions: ["print", "upload", "Minimize", "Maximize", "Close"],
            close: function (e) { tObj.close(e); }
        }).data("kendoWindow");

        if (this.kendoWindowsObjUploadEventAdded === 0) {

            //$("#winDispatchDialog").data("kendoWindow").wrapper.find(".k-svg-i-upload").parent().click(function (e) { e.preventDefault(); tObj.add(); });
            this.kendoWindowsObj.wrapper.find(".k-svg-i-upload").parent().click(function (e) { e.preventDefault(); tObj.add(); });
            this.kendoWindowsObj.wrapper.find(".k-svg-i-print").parent().click(function (e) { e.preventDefault(); tObj.print(); });

            this.kendoWindowsObj.wrapper.find(".k-svg-i-book").parent().click(function (e) { e.preventDefault(); tObj.openContactWindow(); }); //Added By LVV on 2/19/2019 for bug fix add Carrier Contact selection to Dispatch
        }
        this.kendoWindowsObjUploadEventAdded = 1;

        this.kendoWindowsObj.center().open();
    }

    this.read = function (intControl) {
        var blnRet = false;
        //if not supported just return false
        //return blnRet;
        var tObj = this;
        if (typeof (intControl) != 'undefined' && intControl != null) {
            this.rData = null;
            this.data = null;
            var oCRUDCtrl = new nglRESTCRUDCtrl();
            blnRet = oCRUDCtrl.read("Dispatching/GetBidToDispatch", intControl, tObj, "readSuccessCallback", "readAjaxErrorCallback");
        }
        return blnRet;
    }

    this.edit = function (data) {
        //load data to the screen
        var tObj = this;
        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
            this.updateLoadSummaryData();
            //load all the data from the Dispatch Object to the Screen object
            $("#spProviderSCAC").html(data.ProviderSCAC);
            $("#spVendorSCAC").html(data.VendorSCAC);
            $("#spQuoteNumber").html(data.QuoteNumber);
            data.LinearFeet = this.calcLinearFt();
            $("#spLinearFeet").html(data.LinearFeet);
            $("#spLineHaul").html(ngl.currencyFormat(data.LineHaul));
            $("#spFuel").html(ngl.currencyFormat(data.Fuel));
            $("#spTotalCost").html(ngl.currencyFormat(data.TotalCost));
            $("#txtDispOrigName").data("kendoMaskedTextBox").value(data.Origin.Name);
            $("#txtDispOrigContName").data("kendoMaskedTextBox").value(data.Origin.Contact.ContactName);
            $("#txtDispOrigContPhone").data("kendoMaskedTextBox").value(data.Origin.Contact.ContactPhone);
            $("#txtDispOrigContEmail").data("kendoMaskedTextBox").value(data.Origin.Contact.ContactEmail);
            $("#txtDispOrigAddress1").data("kendoMaskedTextBox").value(data.Origin.Address1);
            $("#txtDispDestName").data("kendoMaskedTextBox").value(data.Destination.Name);
            $("#txtDispDestContName").data("kendoMaskedTextBox").value(data.Destination.Contact.ContactName);
            $("#txtDispDestContPhone").data("kendoMaskedTextBox").value(data.Destination.Contact.ContactPhone);
            $("#txtDispDestContEmail").data("kendoMaskedTextBox").value(data.Destination.Contact.ContactEmail);
            $("#txtDispDestAddress1").data("kendoMaskedTextBox").value(data.Destination.Address1);
            $("#txtDispPickupDate").data("kendoDatePicker").value(data.PickupDate);
            //if (data.PickupStartTime === null || ngl.isNullOrWhitespace(data.PickupStartTime)) { data.PickupStartTime = "08.00";}
            $("#txtDispPickupStartTime").data("kendoTimePicker").value(ngl.getDateFromTimeString(data.PickupStartTime, data.PickupDate, '08:00'));
            //if (data.PickupEndTime === null || ngl.isNullOrWhitespace(data.PickupEndTime)) { data.PickupEndTime = "16.00"; }
            $("#txtDispPickupEndTime").data("kendoTimePicker").value(ngl.getDateFromTimeString(data.PickupEndTime, data.PickupDate, '17:00'));
            $("#txtDispDeliveryDate").data("kendoDatePicker").value(data.DeliveryDate);
            //if (data.DeliveryStartTime === null || ngl.isNullOrWhitespace(data.DeliveryStartTime)) { data.DeliveryStartTime = "08.00"; }
            $("#txtDispDeliveryStartTime").data("kendoTimePicker").value(ngl.getDateFromTimeString(data.DeliveryStartTime, data.PickupDate, '08:00'));
            //if (data.DeliveryEndTime === null || ngl.isNullOrWhitespace(data.DeliveryEndTime)) { data.DeliveryEndTime = "16.00"; }
            $("#txtDispDeliveryEndTime").data("kendoTimePicker").value(ngl.getDateFromTimeString(data.DeliveryEndTime, data.PickupDate, '17:00'));
            //$("#txtDispBillOfLading").data("kendoMaskedTextBox").value(data.BillOfLading);
            $("#txtDispPONumber").data("kendoMaskedTextBox").value(data.PONumber);

            $("#txtDispOrderNumber").data("kendoMaskedTextBox").value(data.OrderNumber);
            $("#txtDispCarrierProNumber").data("kendoMaskedTextBox").value(data.CarrierProNumber);
            $("#txtDispPickupNote").data("kendoMaskedTextBox").value(data.PickupNote);
            $("#txtDispDeliveryNote").data("kendoMaskedTextBox").value(data.DeliveryNote);
            $("#txtDispConfidentialNote").data("kendoMaskedTextBox").value(data.ConfidentialNote);
            if (isEmpty(data.LoadTenderTransTypeControl) || data.LoadTenderTransTypeControl == "0") { data.LoadTenderTransTypeControl = 1; }
            $("#txtLoadTenderTransTypeControl").data("kendoComboBox").value(data.LoadTenderTransTypeControl);
            if (isEmpty(data.SHID) || data.SHID === "Quote" || data.SHID === "(Auto)") {
                data.SHID = '';
                $("#spDispSHID").html(data.SHID);
                //Get a new SHID using the Get Next CNS REST Service
                var oCRUDCtrl = new nglRESTCRUDCtrl();
                var blnRet = oCRUDCtrl.read("Dispatching/GetNextCNSNbr", 0, tObj, "readSHIDSuccessCallback", "readSHIDAjaxErrorCallback")
            } else {
                $("#spDispSHID").html(data.SHID);
            }

            //Added By LVV on 2/19/2019 for bug fix add Carrier Contact selection to Dispatch
            $("#spDispCarContName").html(data.CarrierContact.ContactName);
            $("#spDispCarContTitle").html(data.CarrierContact.ContactTitle);
            $("#spDispCarContEmail").html(data.CarrierContact.ContactEmail);
            $("#spDispCarContPhone").html(data.CarrierContact.ContactPhone);
            $("#spDispCarContPhoneExt").html(data.CarrierContact.ContactPhoneExt);
            $("#spDispCarCont800").html(data.CarrierContact.Contact800);
            $("#spDispCarContFax").html(data.CarrierContact.ContactFax);
            $("#txtDispCarContControl").val(data.CarrierContact.ContactControl);
            $("#txtDispCarContCarrierControl").val(data.CarrierContact.ContactCarrierControl);
            $("#txtDispCarContLECarControl").val(data.CarrierContact.ContactLECarControl);
            if (data.CarrierContact.ContactDefault) { $("#chkDispCarContDefault").prop('checked', true); } else { $("#chkDispCarContDefault").prop('checked', false); }
            if (data.CarrierContact.ContactScheduler) { $("#chkDispCarContScheduler").prop('checked', true); } else { $("#chkDispCarContScheduler").prop('checked', false); }

            $("#txtDispCarrierControl").val(data.CarrierControl);
            var switchdataItem = data.AutoAcceptOnDispatch;
            if (switchdataItem !== true) { switchdataItem = false; }
            $("#swDispAutoAcceptOnDispatch").data("kendoSwitch").check(switchdataItem);
            switchdataItem = data.EmailLoadTenderSheet;
            if (switchdataItem !== true) { switchdataItem = false; }
            $("#swDispEmailLoadTenderSheet").data("kendoSwitch").check(switchdataItem);

        }

    }

    this.delete = function () {
        //if not supported just return false
        return;
        //var oChanges = new cmPageDetail();
        //var tObj = this;
        //try {
        //    var sControl = ngl.intTryParse($("#txtPageDetControl").val(), 0);
        //    if (typeof (sControl) !== 'undefined') {
        //        oChanges.PageDetControl = sControl;
        //    } else {
        //        var msg = "This detail record does not have a valid primary key and cannot be deleted!"
        //        ngl.showValidationMsg("Delete Page Detail Validation Failue", msg, tObj);
        //        return;
        //    }

        //    //default values;
        //    oChanges.PageDetOrientation = $("#txtPageDetOrientation").val();
        //    oChanges.PageDetAllowFilter = $("#txtPageDetAllowFilter").val();
        //    oChanges.PageDetFilterTypeControl = $("#txtPageDetFilterTypeControl").val
        //    oChanges.PageDetAllowPaging = $("#txtPageDetAllowPaging").val();
        //    oChanges.PageDetUserSecurityControl = ngl.intTryParse($("#txtPageDetUserSecurityControl").val(), 0);
        //    oChanges.PageDetVisible = $("#txtPageDetVisible").prop("checked");
        //    oChanges.PageDetReadOnly = $("#txtPageDetReadOnly").prop("checked");
        //    oChanges.PageDetAllowSort = $("#txtPageDetAllowSort").prop("checked");
        //    oChanges.PageDetPageTemplateControl = ngl.intTryParse($("#txtPageDetPageTemplateControl").val(), 0);
        //    oChanges.PageDetExpanded = $("#txtPageDetExpanded").val();
        //    oChanges.PageDetFKReference = $("#txtPageDetFKReference").val();

        //    oChanges.PageDetPageControl = ngl.intTryParse($("#txtPageDetPageControl").val(), intPageControl);
        //    oChanges.PageDetDataElmtControl = ngl.intTryParse($("#txtPageDetDataElmtControl").val(), 0);
        //    oChanges.PageDetElmtFieldControl = ngl.intTryParse($("#txtPageDetElmtFieldControl").val(), 0);
        //    oChanges.PageDetAPIReference = $("#txtPageDetAPIReference").val();
        //    oChanges.PageDetAPIFilterID = $("#txtPageDetAPIFilterID").val();
        //    oChanges.PageDetAPISortKey = $("#txtPageDetAPISortKey").val();
        //    oChanges.PageDetParentID = ngl.intTryParse($("#txtPageDetParentID").val(), 0);

        //    oChanges.PageDetTagIDReference = $("#txtPageDetTagIDReference").val();
        //    oChanges.PageDetCSSClass = $("#txtPageDetCSSClass").val();
        //    oChanges.PageDetAttributes = $("#txtPageDetAttributes").val();
        //    oChanges.PageDetMetaData = $("#txtPageDetMetaData").data("kendoEditor").value();

        //    oChanges.PageDetName = $("#txtPageDetName").val();
        //    oChanges.PageDetDesc = $("#txtPageDetDesc").val();
        //    oChanges.PageDetCaption = $("#txtPageDetCaption").val();
        //    oChanges.PageDetCaptionLocal = $("#txtPageDetCaptionLocal").val();
        //    oChanges.PageDetGroupSubTypeControl = ngl.intTryParse($("#txtPageDetGroupSubTypeControl").val(), 0);
        //    oChanges.PageDetGroupTypeControl = ngl.intTryParse($("#txtPageDetGroupTypeControl").val(), 0);
        //    oChanges.PageDetUserSecurityControl = ngl.intTryParse($("#txtPageDetUserSecurityControl").val(), 0);
        //    oChanges.PageDetSequenceNo = ngl.intTryParse($("#txtPageDetSequenceNo").val(), 0);
        //    oChanges.PageDetWidth = ngl.intTryParse($("#txtPageDetWidth").val(), 0);

        //    oChanges.PageDetModDate = $("#txtPageDetModDate").val();
        //    oChanges.PageDetModUser = $("#txtPageDetModUser").val();
        //    oChanges.PageDetUpdated = $("#txtPageDetUpdated").val();

        //    this.rData = null;
        //    var oCRUDCtrl = new nglRESTCRUDCtrl();
        //    var blnRet = oCRUDCtrl.update("cmPageDetail/PostDelete", oChanges, tObj, "saveSuccessCallback", "saveAjaxErrorCallback");

        //} catch (err) {
        //    ngl.showErrMsg(err.name, err.message, tObj);
        //}

    }

    //Note:  this method never gets called because we do not have a callback method for the kendoWindow
    this.close = function (e) {
        //var oResults = new nglEventParameters();
        //oResults.source = "close";
        //oResults.widget = this;
        //oResults.msg = 'closing nothing is saved';

        //if (typeof (this.onClose) === "function") {
        //    this.onClose(oResults);
        //}
    }

    //loadDefaults sets up the callbacks cbSelect and cbSave
    //all call backs return a reference to this object and a string message as parameters
    this.loadDefaults = function (pageVariable, selectCallBack, saveCallBack, closeCallBack, readCallBack) {

        this.onSelect = selectCallBack;
        this.onSave = saveCallBack;
        this.onClose = closeCallBack;
        this.onRead = readCallBack;
        this.data = new Dispatch();
        this.lastErr = "";
        this.notifyOnError = true;
        this.notifyOnSuccess = true;
        this.notifyOnValidationFailure = true;
        var tObj = this;
        if (typeof (pageVariable) !== 'undefined' && ngl.isObject(pageVariable)) {
            //this is the add new popup window
            this.kendoWindowsObj = pageVariable;
            //$("#btnSaveNewPageDetailtop").click(function () { tObj.add(); })
            //$("#btnSaveNewDetailbot").click(function () { tObj.add(); })
            //$("#txtNewPageDetName").kendoMaskedTextBox();
            //$("#txtNewPageDetParentID").kendoDropDownList();
            //$("#txtNewPageDetGroupSubTypeControl").kendoDropDownList();
            $("#txtLoadTenderTransTypeControl").kendoComboBox({
                index: 0,
                dataTextField: "Name",
                dataValueField: "Control",
                dataSource: nglvLookupEditors.dsLoadTenderTransType,
                change: function (e) { this.data.LoadTenderTransTypeControl = e.value; }
            });

            $("#txtDispOrigName").kendoMaskedTextBox();
            $("#txtDispOrigContName").kendoMaskedTextBox();
            $("#txtDispOrigContPhone").kendoMaskedTextBox({ mask: ngl.getPhoneNumberMask() });
            $("#txtDispOrigContEmail").kendoMaskedTextBox();
            $("#txtDispOrigAddress1").kendoMaskedTextBox();

            $("#txtDispDestName").kendoMaskedTextBox();
            $("#txtDispDestContName").kendoMaskedTextBox();
            $("#txtDispDestContPhone").kendoMaskedTextBox({ mask: ngl.getPhoneNumberMask() });
            $("#txtDispDestContEmail").kendoMaskedTextBox();
            $("#txtDispDestAddress1").kendoMaskedTextBox();

            $("#txtDispPickupDate").kendoDatePicker();
            $("#txtDispPickupStartTime").kendoTimePicker({
                format: "HH:mm ",
                interval: 15
            });
            $("#txtDispPickupEndTime").kendoTimePicker({
                format: "HH:mm ",
                interval: 15
            });

            $("#txtDispDeliveryDate").kendoDatePicker();
            $("#txtDispDeliveryStartTime").kendoTimePicker({
                format: "HH:mm ",
                interval: 15
            });
            $("#txtDispDeliveryEndTime").kendoTimePicker({
                format: "HH:mm ",
                interval: 15
            });

            //$("#txtDispBillOfLading").kendoMaskedTextBox();
            $("#txtDispPONumber").kendoMaskedTextBox();
            $("#txtDispOrderNumber").kendoMaskedTextBox();
            $("#txtDispCarrierProNumber").kendoMaskedTextBox();
            $("#txtDispPickupNote").kendoMaskedTextBox();
            $("#txtDispDeliveryNote").kendoMaskedTextBox();
            $("#txtDispConfidentialNote").kendoMaskedTextBox();

            $("#swDispAutoAcceptOnDispatch").kendoSwitch();
            $("#swDispEmailLoadTenderSheet").kendoSwitch();
            //Added By LVV on 2/19/2019 for bug fix add Carrier Contact selection to Dispatch
            this.oSelectContactCtrl = new SelectContactCtrl();
            this.oSelectContactCtrl.loadDefaults(pageVariable, this);

        } else {
            this.kendoWindowsObj = null;
        }
    }
}

//Display the load dispatching information report
//in Views/DispatchingReport.html
function DispatchingReportCtrl() {
    //Generic properties for all Widgets
    this.lastErr = "";
    this.notifyOnError = true;
    this.notifyOnSuccess = true;
    this.notifyOnValidationFailure = true;
    data: Dispatch();
    rData: null;
    onSelect: null;
    onSave: null;
    onClose: null;
    onRead: null;
    dSource: null;
    sourceDiv: null;
    kendoWindowsObj: null;
    //Widget specific properties

    this.kendoWindowsObjUploadEventAdded = 0;

    //Widget specific functions
    this.calcLinearFt = function () {
        var iRet = 0;
        var hdrData = this.data[0];
        if (typeof (hdrData) !== 'undefined' && ngl.isObject(hdrData)) {
            if (typeof (hdrData.Items) !== 'undefined' && ngl.isArray(hdrData.Items) && hdrData.Items.length > 0) {
                var TotalLen = 0;
                for (i = 0, len = hdrData.Items.length; i < len; i++) {
                    var iLen = hdrData.Items[i].ItemLength
                    if (typeof (iLen) !== 'undefined' && iLen !== null && !isNaN(iLen) && iLen > 0) { TotalLen += iLen; }
                }
                if (TotalLen !== null && !isNaN(TotalLen) && TotalLen > 0) {
                    switch (hdrData.LengthUnit.toUpperCase()) {
                        case "IN":
                            iRet = ngl.convertINtoLinearFeet(TotalLen);
                            break;
                        case "CM":
                            iRet = ngl.convertCMtoLinearFeet(TotalLen);
                            break;
                        case "FT":
                            iRet = TotalLen;
                            break;
                        case "M":
                            iRet = ngl.convertMtoLinearFeet(TotalLen);
                            break;
                        default:
                            iRet = 0;
                    }
                }
            }
        }
        return iRet;
    }

    this.updateHeaderData = function (hdr) {
        $("#spDispRptShipWgt").html(hdr.TotalWgt);
        $("#spDispRptShipQty").html(hdr.TotalQty);
        $("#spDispRptShipPlts").html(hdr.TotalPlts);
        $("#spDispRptProviderName").html(hdr.CarrierName);
        $("#spDispRptProviderSCAC").html(hdr.ProviderSCAC);
        $("#spDispRptVendorSCAC").html(hdr.VendorSCAC);
        $("#spDispRptQuoteNumber").html(hdr.QuoteNumber);
        $("#spDispRptLineHaul").html(ngl.currencyFormat(hdr.LineHaul));
        $("#spDispRptFuel").html(ngl.currencyFormat(hdr.Fuel));
        $("#spDispRptTotalCost").html(ngl.currencyFormat(hdr.TotalCost));
        var sTransType = 'Outbound';
        if (isEmpty(hdr.LoadTenderTransTypeControl) || hdr.LoadTenderTransTypeControl == "0") { hdr.LoadTenderTransTypeControl = 1; }
        if (hdr.LoadTenderTransTypeControl == 3) { sTransType = 'Inbound'; }
        $("#spDispRptTransType").html(sTransType);
        $("#spDispRptSHID").html(hdr.SHID);
        $("#spDispRptCarrierNotes").html(hdr.ErrorMessage);
        $("#spDispRptLegal").html(hdr.DispatchLegalText);
    }

    this.addSuccessCallback = function (data) {
        var oResults = new nglEventParameters();
        oResults.source = "addSuccessCallback";
        var tObj = this;
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed   
        //kendo.ui.progress($(document.body), false);
        this.rData = null;
        try {
            var blnSuccess = false;
            var blnErrorShown = false;
            var strValidationMsg = "";
            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                    oResults.error = new Error();
                    oResults.error.name = "Dispatch Failure";
                    oResults.error.message = data.Errors;
                    blnErrorShown = true;
                    ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                }
                else {
                    this.rData = data.Data;
                    if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                        if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                            blnSuccess = true;
                            oResults.data = data.Data;
                            oResults.datatype = "string";
                            oResults.msg = "<h3>Success</h3>" + data.Data[0]
                            this.kendoWindowsObj.close();
                            ngl.Alert('Dispatch Results', oResults.msg, 400, 400);
                            //ngl.showSuccessMsg(oResults.msg, tObj);
                        }
                    }

                }
            }
            if (blnSuccess === false && blnErrorShown === false) {
                oResults.error = new Error();
                oResults.error.name = "Dispatch Failure";
                oResults.error.message = "No results are available.";
                ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
            }
        } catch (err) {
            oResults.error = err
        }
        if (ngl.isFunction(this.onSave)) {
            this.onSave(oResults);
        }

    }

    this.print = function () {
        var kinWin = $("#winDispRpt").kendoWindow().data("kendoWindow");
        var winWrapper = kinWin.wrapper;
        winWrapper.removeClass("k-window");
        winWrapper.addClass("printable");
        //put pring code here
        window.print();
        winWrapper.removeClass("printable");
        winWrapper.addClass("k-window");
    }

    this.show = function () {
        if (typeof (this.data) !== 'undefined' && ngl.isObject(this.data)) {
            this.edit(this.data);
        }

        var tObj = this;

        this.kendoWindowsObj = $("#winDispRpt").kendoWindow({
            title: "Dispatch Report",
            modal: true,
            visible: false,
            height: '75%',
            width: '75%',
            scrollable: true,
            actions: ["print", "Minimize", "Maximize", "Close"],
            close: function (e) { tObj.close(e); }
        }).data("kendoWindow");

        if (this.kendoWindowsObjUploadEventAdded === 0) {
            $("#winDispRpt").data("kendoWindow").wrapper.find(".k-svg-i-print").parent().click(function (e) { e.preventDefault(); tObj.print(); });
        }
        this.kendoWindowsObjUploadEventAdded = 1;

        this.kendoWindowsObj.center().open();
    }

    this.read = function (intControl) {
        var blnRet = false;
        //if not supported just return false
        return blnRet;
        //var tObj = this;
        //if (typeof (intControl) != 'undefined' && intControl != null) {
        //    this.rData = null;
        //    var oCRUDCtrl = new nglRESTCRUDCtrl();
        //    blnRet = oCRUDCtrl.read("cmPageDetail", intControl, tObj, "readSuccessCallback", "readAjaxErrorCallback");
        //}
        //return blnRet;
    }

    this.fixIds = function (elem, cntr) {
        $(elem).find("[id]").add(elem).each(function () {
            this.id = this.id + cntr;
        });
    }

    this.edit = function (data) {
        //load data to the screen
        var tObj = this;
        $("#divDispRpt").show();
        $("#divErrMsgDispRpt").hide();
        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
            if (typeof (data) !== 'undefined' && ngl.isArray(data) && data.length > 0) {

                if (typeof (data[0]) === 'undefined' || !ngl.isObject(data[0])) { $("#divDispRpt").hide(); $("#divErrMsgDispRpt").show(); return; }
                if (data[0].BookControl === 0 && data[0].LoadTenderControl === 0 && !ngl.stringHasValue(data[0].ProviderSCAC)) { $("#divDispRpt").hide(); $("#divErrMsgDispRpt").show(); return; }
                for (i = 1; i < 100; i++) {
                    var sID = "divDispatchStops" + i.toString();
                    var sPbrID = "pgbr" + i.toString();
                    if ($("#" + sID).length) {
                        $("#" + sID).remove();
                        if ($("#" + sPbrID).length) {
                            $("#" + sPbrID).remove();
                        }
                    } else {
                        break;
                    }
                }
                //load all the data from the Dispatch Object to the Screen object   
                this.updateHeaderData(data[0]);
                data[0].LinearFeet = this.calcLinearFt();
                $("#spDispRptLinearFeet").html(data[0].LinearFeet);
                //Process Pickup and Delivery
                var strIx = "";
                for (i = 0; i < data.length; i++) {
                    if (typeof (data[i]) !== 'undefined' && ngl.isObject(data[i])) {
                        if (i > 0) {
                            var clone = $("#divDispatchStops").clone(true, true);
                            this.fixIds(clone, i);
                            clone.insertAfter("#divDispatchStops" + strIx); //must be first
                            strIx = i.toString(); //after intsertAfter
                            var sn = (i + 1).toString(); //because the html with index 0 is actually stop 1
                            $("#lblPickupInfo" + strIx).html("<b>Pickup Information #" + sn + "</b>");
                            $("#lblDeliveryInfo" + strIx).html("<b>Delivery Information #" + sn + "</b>");
                        }
                        $("#spDispRptOrigName" + strIx).html(data[i].Origin.Name);
                        $("#spDispRptOrigContName" + strIx).html(data[i].Origin.Contact.ContactName);
                        $("#spDispRptOrigContPhone" + strIx).html(ngl.formatPhoneNumber(data[i].Origin.Contact.ContactPhone));
                        $("#spDispRptOrigContEmail" + strIx).html(data[i].Origin.Contact.ContactEmail);
                        $("#spDispRptOrigAddress1" + strIx).html(data[i].Origin.Address1);
                        $("#spDispRptDestName" + strIx).html(data[i].Destination.Name);
                        $("#spDispRptDestContName" + strIx).html(data[i].Destination.Contact.ContactName);
                        $("#spDispRptDestContPhone" + strIx).html(ngl.formatPhoneNumber(data[i].Destination.Contact.ContactPhone));
                        $("#spDispRptDestContEmail" + strIx).html(data[i].Destination.Contact.ContactEmail);
                        $("#spDispRptDestAddress1" + strIx).html(data[i].Destination.Address1);
                        $("#spDispRptPickupDate" + strIx).html(ngl.getShortDateString(data[i].PickupDate));
                        $("#spDispRptPickupStartTime" + strIx).html(data[i].PickupStartTime);
                        $("#spDispRptPickupEndTime" + strIx).html(data[i].PickupEndTime);
                        $("#spDispRptDeliveryDate" + strIx).html(ngl.getShortDateString(data[i].DeliveryDate));
                        $("#spDispRptDeliveryStartTime" + strIx).html(data[i].DeliveryStartTime);
                        $("#spDispRptDeliveryEndTime" + strIx).html(data[i].DeliveryEndTime);
                        $("#spDispRptConfNbr" + strIx).html(data[i].PickupNumber);
                        $("#spDispRptPONumberPU" + strIx).html(data[i].PONumber);
                        $("#spDispRptPONumberDEL" + strIx).html(data[i].PONumber);
                        $("#spDispRptOrderNumberPU" + strIx).html(data[i].OrderNumber);
                        $("#spDispRptOrderNumberDEL" + strIx).html(data[i].OrderNumber);
                        $("#spDispRptItemOrderNumbersPU" + strIx).html(data[i].ItemOrderNumbers);
                        $("#spDispRptItemOrderNumbersDEL" + strIx).html(data[i].ItemOrderNumbers);
                        $("#spDispRptCarrierProNumber" + strIx).html(data[i].CarrierProNumber);
                        $("#spDispRptConfidentialNote" + strIx).html(data[i].ConfidentialNote);
                        $("#spDispRptPickupNote" + strIx).html(data[i].PickupNote);
                        $("#spDispRptDeliveryNote" + strIx).html(data[i].DeliveryNote);
                    }
                }
                if (data.length > 1) { $("#lblPickupInfo").html("<b>Pickup Information #1</b>"); $("#lblDeliveryInfo").html("<b>Delivery Information #1</b>"); }
            } else { $("#divDispRpt").hide(); $("#divErrMsgDispRpt").show(); }
        } else { $("#divDispRpt").hide(); $("#divErrMsgDispRpt").show(); }
    }

    //Note:  this method never gets called because we do not have a callback method for the kendoWindow
    this.close = function (e) {
        var oResults = new nglEventParameters();
        oResults.source = "close";
        var tObj = this;
        oResults.widget = tObj;
        oResults.msg = 'closing nothing is saved';
        oResults.data = this.data;

        if (typeof (this.onClose) === "function") {
            this.onClose(oResults);
        }
    }

    //loadDefaults sets up the callbacks cbSelect and cbSave
    //all call backs return a reference to this object and a string message as parameters
    this.loadDefaults = function (pageVariable, selectCallBack, saveCallBack, closeCallBack, readCallBack) {

        this.onSelect = selectCallBack;
        this.onSave = saveCallBack;
        this.onClose = closeCallBack;
        this.onRead = readCallBack;
        this.data = new Dispatch();
        this.lastErr = "";
        this.notifyOnError = true;
        this.notifyOnSuccess = true;
        this.notifyOnValidationFailure = true;
        var tObj = this;

        if (typeof (pageVariable) !== 'undefined' && ngl.isObject(pageVariable)) {
            //this is the add new popup window
            this.kendoWindowsObj = pageVariable;
        } else {
            this.kendoWindowsObj = null;
        }

    }
}

//Added By LVV on 4/18/18 for v-8.1
//SuperUsers can switch Legal Entities - html data store in Views/wndLE.html
function ChangeLEDialogCtrl() {
    //Generic properties for all Widgets
    onSave: null;
    kendoWindowsObj: null;
    //Widget specific properties

    this.kendoWindowsObjUploadEventAdded = 0;

    // Generic CRUD functions for all Widgets
    this.save = function () {
        //Get the selected LE and send the results back to the screen
        var control = $("#ddlChangeLEWidgetLEA").data("kendoDropDownList").value();
        var name = $("#ddlChangeLEWidgetLEA").data("kendoDropDownList").text();

        var oResults = new LegalEntityAdmin();
        oResults.LEAdminControl = control;
        oResults.LegalEntity = name;

        if (ngl.isFunction(this.onSave)) {
            this.onSave(oResults);
            this.kendoWindowsObj.close();
        }
    }

    this.show = function () {
        var tObj = this
        this.kendoWindowsObj = $("#wndChangeLEDialog").kendoWindow({
            title: "Change Legal Entity",
            modal: true,
            visible: false,
        }).data("kendoWindow");

        if (this.kendoWindowsObjUploadEventAdded === 0) {
            $("#btnChangeLEWidgetOK").click(function (e) { e.preventDefault(); tObj.save(); });
        }
        this.kendoWindowsObjUploadEventAdded = 1;

        this.kendoWindowsObj.center().open();
    }

    //loadDefaults sets up the callback cbSave
    //all call backs return a reference to this object and a string message as parameters
    this.loadDefaults = function (pageVariable, saveCallBack) {
        this.onSave = saveCallBack;
        var tObj = this;
        if (typeof (pageVariable) !== 'undefined' && ngl.isObject(pageVariable)) {
            this.kendoWindowsObj = pageVariable;
            $("#ddlChangeLEWidgetLEA").kendoDropDownList({
                autoBind: false,
                dataTextField: "LegalEntity",
                dataValueField: "LEAdminControl",
                autoWidth: true,
                dataSource: {
                    transport: {
                        read: {
                            url: function (options) {
                                return "api/LegalEntity/GetLegalEntityAdmins/";
                            },
                            headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }
                        },
                        parameterMap: function (options, operation) { return options; }
                    },
                    schema: {
                        data: "Data",
                        total: "Count",
                        model: {
                            id: "LEAdminControl",
                            fields: {
                                LEAdminControl: { type: "number", editable: false },
                                CompName: { type: "string", editable: false },
                                LegalEntity: { type: "string", editable: false }
                            }
                        },
                        errors: "Errors"
                    },
                    error: function (xhr, textStatus, error) { ngl.showErrMsg("Get Legal Entity Admins JSON Response Error", formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failuire"), null); this.cancelChanges(); },
                    serverPaging: false,
                    serverSorting: false,
                    serverFiltering: false
                }
            });
        } else { this.kendoWindowsObj = null; }
    }
}

//Added By LVV on 4/19/18 for v-8.1
//Widget for Edit/Add on Comp Maint - html data store in Views/CompMaintEditWindow.html
function CompMaintEAWndCtrl() {
    //Generic properties for all Widgets
    this.lastErr = "";
    this.notifyOnError = true;
    this.notifyOnSuccess = true;
    this.notifyOnValidationFailure = true;
    data: CompMaint;
    rData: null;
    onSave: null;
    dSource: null;
    sourceDiv: null;
    kendoWindowsObj: null;
    //Widget specific properties
    screenLEName: null;

    this.kendoWindowsObjUploadEventAdded = 0;
    this.EditError = false;


    //Generic Call back functions for all Widgets
    this.saveSuccessCallback = function (data) {
        // debugger;
        var oResults = new nglEventParameters();
        oResults.source = "saveSuccessCallback";
        var tObj = this;
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed   
        //kendo.ui.progress($(document.body), false);
        this.rData = null;
        try {
            var blnSuccess = false;
            var blnErrorShown = false;
            var strValidationMsg = "";
            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                    oResults.error = new Error();
                    oResults.error.name = "Save Company Failure";
                    oResults.error.message = data.Errors;
                    blnErrorShown = true;
                    ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                }
                else {
                    this.rData = data.Data;
                    if (typeof (data.Data) !== 'undefined' && data.Data != null && ngl.isArray(data.Data)) {
                        if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                            blnSuccess = true;
                            oResults.data = data.Data;
                            oResults.datatype = "string";
                            oResults.msg = "<h3>Success</h3>" + data.Data[0]
                            //Only close the window if the save was successful
                            this.kendoWindowsObj.close();
                            ;
                            //ngl.showSuccessMsg(oResults.msg, tObj);
                        }
                    }

                }
            }
            if (blnSuccess === false && blnErrorShown === false) {
                oResults.error = new Error();
                oResults.error.name = "Save Company Failure";
                oResults.error.message = "No results are available.";
                ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
            }
        } catch (err) {
            oResults.error = err
        }
        if (ngl.isFunction(this.onSave)) {
            this.onSave(oResults);
        }

    }

    this.saveAjaxErrorCallback = function (xhr, textStatus, error) {
        var oResults = new nglEventParameters();
        var tObj = this;
        //kendo.ui.progress($(document.body), false);
        oResults.source = "saveAjaxErrorCallback";
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed  
        oResults.error = new Error();
        oResults.error.name = "Save Company Failure"
        oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);

        if (ngl.isFunction(this.onSave)) {
            this.onSave(oResults);
            //this.kendoWindowsObj.close();
        }
    }

    // Generic functions for all Widgets
    this.validateRequiredFields = function (data) {
        var oRet = new validationResults();
        if (typeof (data) === 'undefined' || !ngl.isObject(data)) {
            //something went wrong so throw an error. this should never happen
            ngl.showValidationMsg("Save Company Validation Failed; No Data", "Please Contact Technical Support", tObj);
            oRet.Message = "Save Company Validation Failed; No Data";
            return oRet;
        }

        var blnValidated = true;
        var sValidationMsg = "";
        var sSpacer = "";

        if (isEmpty(data.Company.CompName)) { blnValidated = false; sValidationMsg += sSpacer + "Company Name"; sSpacer = ", "; }
        if (isEmpty(data.Company.CompLegalEntity)) { blnValidated = false; sValidationMsg += sSpacer + "Legal Entity"; sSpacer = ", "; }
        if (isEmpty(data.Company.CompAlphaCode)) { blnValidated = false; sValidationMsg += sSpacer + "Location Code"; sSpacer = ", "; }
        if (isEmpty(data.Company.CompAbrev)) { blnValidated = false; sValidationMsg += sSpacer + "Pro Abrv"; sSpacer = ", "; }
        if (isEmpty(data.Company.CompEmail)) { blnValidated = false; sValidationMsg += sSpacer + "Email"; sSpacer = ", "; }

        if (isEmpty(data.Company.CompStreetAddress1)) { blnValidated = false; sValidationMsg += sSpacer + "Street Address 1"; sSpacer = ", "; }
        if (isEmpty(data.Company.CompStreetCity)) { blnValidated = false; sValidationMsg += sSpacer + "Street City"; sSpacer = ", "; }
        if (isEmpty(data.Company.CompStreetState)) { blnValidated = false; sValidationMsg += sSpacer + "Street State"; sSpacer = ", "; }
        if (isEmpty(data.Company.CompStreetZip)) { blnValidated = false; sValidationMsg += sSpacer + "Street Postal Code"; sSpacer = ", "; }
        if (isEmpty(data.Company.CompStreetCountry)) { blnValidated = false; sValidationMsg += sSpacer + "Street Country"; sSpacer = ", "; }
        if (isEmpty(data.Company.CompActive)) { blnValidated = false; sValidationMsg += sSpacer + "Active"; sSpacer = ", "; }

        if (isEmpty(data.CompanyContact.CompContName)) { blnValidated = false; sValidationMsg += sSpacer + "Contact Name"; sSpacer = ", "; }
        if (isEmpty(data.CompanyContact.CompContEmail)) { blnValidated = false; sValidationMsg += sSpacer + "Contact Email"; sSpacer = ", "; }


        oRet.Success = blnValidated;
        oRet.Message = sValidationMsg;

        return oRet;
    }

    // Generic CRUD functions for all Widgets
    this.save = function () {
        var tObj = this;

        //WHAT IS THIS?? SPECIFIC TO DISPATCHING??
        ////if (typeof (this.data) === 'undefined' || !ngl.isObject(this.data)) {
        ////    //something went wrong so throw an error. this should never happen
        ////    ngl.showValidationMsg("Dispatch Failed; No Data", "Please Contact Technical Support", tObj);
        ////    return;
        ////}

        this.data = new CompMaint(); //clear any old data

        var company = new Comp();
        var compContact = new CompContact();

        var tfCompActive = false;
        var tfCompContTender = false;
        if ($('#chkCompActive').is(":checked")) { tfCompActive = true; }
        if ($('#chkCompContTender').is(":checked")) { tfCompContTender = true; }

        var intCompControl = ngl.intTryParse($("#txtCompControl").val(), 0);

        //Comp
        company.CompControl = intCompControl;
        company.CompName = $("#txtCompName").data("kendoMaskedTextBox").value();
        company.CompNumber = $("#txtCompNumber").data("kendoMaskedTextBox").value();
        company.CompAlphaCode = $("#txtCompAlphaCode").data("kendoMaskedTextBox").value();
        company.CompLegalEntity = $("#txtCompLegalEntity").data("kendoMaskedTextBox").value();
        company.CompAbrev = $("#txtCompProAbrv").data("kendoMaskedTextBox").value();
        company.CompWeb = $("#txtCompWebsite").data("kendoMaskedTextBox").value();
        company.CompEmail = $("#txtCompEmail").data("kendoMaskedTextBox").value();
        company.CompStreetAddress1 = $("#txtCompStreetAddress1").data("kendoMaskedTextBox").value();
        company.CompStreetAddress2 = $("#txtCompStreetAddress2").data("kendoMaskedTextBox").value();
        company.CompStreetAddress3 = $("#txtCompStreetAddress3").data("kendoMaskedTextBox").value();
        company.CompStreetCity = $("#txtCompStreetCity").data("kendoMaskedTextBox").value();
        company.CompStreetState = $("#txtCompStreetState").data("kendoMaskedTextBox").value();
        company.CompStreetZip = $("#txtCompStreetZip").data("kendoMaskedTextBox").value();
        company.CompStreetCountry = $("#txtCompStreetCountry").data("kendoMaskedTextBox").value();
        company.CompActive = tfCompActive;
        //CompCont
        compContact.CompContCompControl = intCompControl;
        compContact.CompContTender = tfCompContTender;
        compContact.CompContName = $("#txtCompContName").data("kendoMaskedTextBox").value();
        compContact.CompContTitle = $("#txtCompContTitle").data("kendoMaskedTextBox").value();
        compContact.CompCont800 = $("#txtCompCont800").data("kendoMaskedTextBox").value();
        compContact.CompContPhone = $("#txtCompContPhone").data("kendoMaskedTextBox").value();
        compContact.CompContPhoneExt = $("#txtCompContPhoneExt").data("kendoMaskedTextBox").value();
        compContact.CompContFax = $("#txtCompContFax").data("kendoMaskedTextBox").value();
        compContact.CompContEmail = $("#txtCompContEmail").data("kendoMaskedTextBox").value();

        this.data.Company = company;
        this.data.CompanyContact = compContact;

        if (intCompControl === 0) {
            //Only Validate on Insert
            var oValidationResults = this.validateRequiredFields(this.data);
            if (typeof (oValidationResults) === 'undefined' || !ngl.isObject(oValidationResults)) {
                ngl.showValidationMsg("Cannot validate Company Information", "Invalid validation procedure, please contact technical support", tObj);
                return;
            }
            else { if (oValidationResults.Success === false) { ngl.showValidationMsg("Required Fields", oValidationResults.Message, tObj); return; } }
        }
        //save the changes   
        var changes = this.data;
        var windowWidget = $("#wndCompMaintEA").data("kendoWindow");
        kendo.ui.progress(windowWidget.element, true);
        setTimeout(function (tObj) {
            var oCRUDCtrl = new nglRESTCRUDCtrl();
            oCRUDCtrl.update("Comp/InsertOrUpdateCompMaint", changes, tObj, "saveSuccessCallback", "saveAjaxErrorCallback");
        }, 50, tObj);


    }

    this.show = function () {
        var tObj = this;

        this.kendoWindowsObj = $("#wndCompMaintEA").kendoWindow({
            title: "Add New Company",
            modal: true,
            visible: false,
            height: '75%',
            width: '75%',
            actions: ["save", "Minimize", "Maximize", "Close"],
            close: function (e) { tObj.close(e); }
        }).data("kendoWindow");

        if (this.kendoWindowsObjUploadEventAdded === 0) {
            $("#wndCompMaintEA").data("kendoWindow").wrapper.find(".k-svg-i-save").parent().click(function (e) { e.preventDefault(); tObj.save(); });
        }
        this.kendoWindowsObjUploadEventAdded = 1;

        //if this is an edit load the data to the window
        if (typeof (this.data) !== 'undefined' && ngl.isObject(this.data)) {
            this.edit(this.data);
        }
        else {
            this.kendoWindowsObj.title("Add New Company");
            //Show all the red "required field" * on Add
            $(".redRequiredAsterik").show();
            //Get the LegalEntity from the screen on Add New
            $("#txtCompLegalEntity").data("kendoMaskedTextBox").value(this.screenLEName);
        }

        if (this.EditError === false) { this.kendoWindowsObj.center().open(); }
    }

    this.edit = function (data) {
        //load data to the screen
        var tObj = this;

        this.kendoWindowsObj.title("Edit Company");
        //$("#wndCompMaintEA").data("kendoWindow").title("Edit Company");
        //Hide all the red "required field" * on Edit
        $(".redRequiredAsterik").hide();

        if (typeof (data) !== 'undefined' && data != null && ngl.isObject(data)) {
            //Verify CompControl is a column in the grid and is not null or 0
            if ('CompControl' in data) {
                if (typeof (data.CompControl) === 'undefined' || data.CompControl == null || data.CompControl == 0) { ngl.showErrMsg("CompControl Required", "CompControl cannot be 0", null); this.EditError = true; return; }

                $("#txtCompControl").val(data.CompControl); //Save the CompControl so it can be accessed by the Save function
                if ('CompName' in data) { $("#txtCompName").data("kendoMaskedTextBox").value(data.CompName); }
                if ('CompNumber' in data) { $("#txtCompNumber").data("kendoMaskedTextBox").value(data.CompNumber); }
                if ('CompAlphaCode' in data) { $("#txtCompAlphaCode").data("kendoMaskedTextBox").value(data.CompAlphaCode); }
                if ('CompLegalEntity' in data) { $("#txtCompLegalEntity").data("kendoMaskedTextBox").value(data.CompLegalEntity); }
                if ('CompAbrev' in data) { $("#txtCompProAbrv").data("kendoMaskedTextBox").value(data.CompAbrev); }
                if ('CompWeb' in data) { $("#txtCompWebsite").data("kendoMaskedTextBox").value(data.CompWeb); }
                if ('CompEmail' in data) { $("#txtCompEmail").data("kendoMaskedTextBox").value(data.CompEmail); }
                if ('CompStreetAddress1' in data) { $("#txtCompStreetAddress1").data("kendoMaskedTextBox").value(data.CompStreetAddress1); }
                if ('CompStreetAddress2' in data) { $("#txtCompStreetAddress2").data("kendoMaskedTextBox").value(data.CompStreetAddress2); }
                if ('CompStreetAddress3' in data) { $("#txtCompStreetAddress3").data("kendoMaskedTextBox").value(data.CompStreetAddress3); }
                if ('CompStreetCity' in data) { $("#txtCompStreetCity").data("kendoMaskedTextBox").value(data.CompStreetCity); }
                if ('CompStreetState' in data) { $("#txtCompStreetState").data("kendoMaskedTextBox").value(data.CompStreetState); }
                if ('CompStreetZip' in data) { $("#txtCompStreetZip").data("kendoMaskedTextBox").value(data.CompStreetZip); }
                if ('CompStreetCountry' in data) { $("#txtCompStreetCountry").data("kendoMaskedTextBox").value(data.CompStreetCountry); }
                if ('CompActive' in data) {
                    if (data.CompActive) { $("#chkCompActive").prop('checked', true); } else { $("#chkCompActive").prop('checked', false); }
                }
            }
            else { ngl.showErrMsg("CompControl Required", "Row object does not contain property CompControl.", null); this.EditError = true; return; }

            //Hide divEACompCont on Edit
            $("#divEACompCont").hide();

            //Show CompNumber on Edit but make it readonly
            $("#divCompNumber").show();
            $("#txtCompNumber").data("kendoMaskedTextBox").readonly(true);

            //Set LegalEntity to readonly
            $("#txtCompLegalEntity").data("kendoMaskedTextBox").readonly(true);
        }
        else { ngl.showErrMsg("Error", "There was a problem getting the row from the grid", null); this.EditError = true; return; }
    }

    this.close = function (e) {
        //** Always Set up the window for "Add New" on close **
        //** This way we only have to do special set up in the "Edit" method
        //reset values
        $("#txtCompControl").val(0);
        $("#txtCompName").data("kendoMaskedTextBox").value("");
        $("#txtCompNumber").data("kendoMaskedTextBox").value("");
        $("#txtCompLegalEntity").data("kendoMaskedTextBox").value("");
        $("#txtCompAlphaCode").data("kendoMaskedTextBox").value("");
        $("#txtCompProAbrv").data("kendoMaskedTextBox").value("");
        $("#txtCompWebsite").data("kendoMaskedTextBox").value("");
        $("#txtCompEmail").data("kendoMaskedTextBox").value("");
        $("#txtCompStreetAddress1").data("kendoMaskedTextBox").value("");
        $("#txtCompStreetAddress2").data("kendoMaskedTextBox").value("");
        $("#txtCompStreetAddress3").data("kendoMaskedTextBox").value("");
        $("#txtCompStreetCity").data("kendoMaskedTextBox").value("");
        $("#txtCompStreetState").data("kendoMaskedTextBox").value("");
        $("#txtCompStreetZip").data("kendoMaskedTextBox").value("");
        $("#txtCompStreetCountry").data("kendoMaskedTextBox").value("");
        $("#txtCompContName").data("kendoMaskedTextBox").value("");
        $("#txtCompContTitle").data("kendoMaskedTextBox").value("");
        $("#txtCompCont800").data("kendoMaskedTextBox").value("");
        $("#txtCompContPhone").data("kendoMaskedTextBox").value("");
        $("#txtCompContPhoneExt").data("kendoMaskedTextBox").value("");
        $("#txtCompContFax").data("kendoMaskedTextBox").value("");
        $("#txtCompContEmail").data("kendoMaskedTextBox").value("");

        //Check "CompActive" and show divEACompCont
        $("#chkCompActive").prop('checked', true);
        $("#divEACompCont").show();

        //Hide CompNumber on Add since this is auto-generated
        $("#divCompNumber").hide();

        //Set LegalEntity to readonly because this comes from the page
        $("#txtCompLegalEntity").data("kendoMaskedTextBox").readonly(true);
        //reset the LE so we get it fresh from the screen everytime
        this.screenLEName = "";

        //Expand the Fast Tabs
        expandEACompany();
        expandEACompCont();

    }

    //loadDefaults sets up the callbacks cbSelect and cbSave
    //all call backs return a reference to this object and a string message as parameters
    this.loadDefaults = function (pageVariable, saveCallBack) {

        this.onSave = saveCallBack;
        this.data = new Dispatch();
        this.lastErr = "";
        this.notifyOnError = true;
        this.notifyOnSuccess = true;
        this.notifyOnValidationFailure = true;
        var tObj = this;

        if (typeof (pageVariable) !== 'undefined' && ngl.isObject(pageVariable)) {

            this.kendoWindowsObj = pageVariable;

            //Comp Fields
            $("#txtCompControl").val(0);
            $("#txtCompName").kendoMaskedTextBox();
            $("#txtCompNumber").kendoMaskedTextBox();
            $("#txtCompLegalEntity").kendoMaskedTextBox();
            $("#txtCompAlphaCode").kendoMaskedTextBox();
            $("#txtCompProAbrv").kendoMaskedTextBox();
            $("#txtCompWebsite").kendoMaskedTextBox();
            $("#txtCompEmail").kendoMaskedTextBox();
            $("#txtCompStreetAddress1").kendoMaskedTextBox();
            $("#txtCompStreetAddress2").kendoMaskedTextBox();
            $("#txtCompStreetAddress3").kendoMaskedTextBox();
            $("#txtCompStreetCity").kendoMaskedTextBox();
            $("#txtCompStreetState").kendoMaskedTextBox();
            $("#txtCompStreetZip").kendoMaskedTextBox();
            $("#txtCompStreetCountry").kendoMaskedTextBox();
            //CompCont Fields
            $("#txtCompContName").kendoMaskedTextBox();
            $("#txtCompContTitle").kendoMaskedTextBox();
            $("#txtCompCont800").kendoMaskedTextBox();
            $("#txtCompContPhone").kendoMaskedTextBox({ mask: ngl.getPhoneNumberMask() });
            $("#txtCompContPhoneExt").kendoMaskedTextBox();
            $("#txtCompContFax").kendoMaskedTextBox({ mask: ngl.getPhoneNumberMask() });
            $("#txtCompContEmail").kendoMaskedTextBox();

            //** Set up the window for "Add New" on initialize **
            //** This way we only have to do special set up in the "Edit" method

            //Check "CompActive" and show divEACompCont
            $("#chkCompActive").prop('checked', true);
            $("#divEACompCont").show();

            //Hide CompNumber on Add since this is auto-generated
            $("#divCompNumber").hide();

            //Set LegalEntity to readonly because this comes from the page
            $("#txtCompLegalEntity").data("kendoMaskedTextBox").readonly(true);

            //Set the title of the window to "Add" -- NOTE: This gets set in show method every time it is called so no need to reset here

            expandEACompany = function () {
                $("#FastTabDivEACompany").show();
                $("#ExpandEACompanySpan").hide();
                $("#CollapseEACompanySpan").show();
            }
            collapseEACompany = function () {
                $("#FastTabDivEACompany").hide();
                $("#ExpandEACompanySpan").show();
                $("#CollapseEACompanySpan").hide();
            }

            expandEACompCont = function () {
                $("#FastTabDivEACompCont").show();
                $("#ExpandEACompContSpan").hide();
                $("#CollapseEACompContSpan").show();
            }
            collapseEACompCont = function () {
                $("#FastTabDivEACompCont").hide();
                $("#ExpandEACompContSpan").show();
                $("#CollapseEACompContSpan").hide();
            }

        } else {
            this.kendoWindowsObj = null;
        }

    }
}

//Added By LVV on 4/23/18 for v-8.1
//Widget for Edit/Add on Comp Cont - html data store in Views/CompContEAWindow.html
function CompContEAWndCtrl() {
    //Generic properties for all Widgets
    this.lastErr = "";
    this.notifyOnError = true;
    this.notifyOnSuccess = true;
    this.notifyOnValidationFailure = true;
    data: CompContact;
    rData: null;
    onSave: null;
    onDelete: null;
    dSource: null;
    sourceDiv: null;
    kendoWindowsObj: null;
    //Widget specific properties
    screenCompName: "";
    screenCompControl: 0;
    deleteCompContControl: 0;

    this.kendoWindowsObjUploadEventAdded = 0;

    //Widget specific functions
    this.confirmDeleteContact = function (iRet) {
        if (typeof (iRet) === 'undefined' || iRet === null || iRet === 0) { return; }

        if (!ngl.isNullOrUndefined(this.deleteCompContControl) && this.deleteCompContControl > 0) {
            var tObj = this;
            var oCRUDCtrl = new nglRESTCRUDCtrl();
            var blnRet = oCRUDCtrl.delete("CompCont/DeleteCompContact", this.deleteCompContControl, tObj, "deleteSuccessCallback", "deleteAjaxErrorCallback");
        }
    }

    //Generic Call back functions for all Widgets
    this.saveSuccessCallback = function (data) {
        //debugger;
        var oResults = new nglEventParameters();
        oResults.source = "saveSuccessCallback";
        var tObj = this;
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed   
        //kendo.ui.progress($(document.body), false);
        this.rData = null;
        try {
            var blnSuccess = false;
            var blnErrorShown = false;
            var strValidationMsg = "";
            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                    oResults.error = new Error();
                    oResults.error.name = "Save Company Contact Failure";
                    oResults.error.message = data.Errors;
                    blnErrorShown = true;
                    ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                }
                else {
                    this.rData = data.Data;
                    if (typeof (data.Data) !== 'undefined' && data.Data != null && ngl.isArray(data.Data)) {
                        if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                            blnSuccess = true;
                            oResults.data = data.Data;
                            oResults.datatype = "string";
                            oResults.msg = "<h3>Success</h3>" + data.Data[0]
                            //Only close the window if the save was successful
                            this.kendoWindowsObj.close();
                            ;
                            //ngl.showSuccessMsg(oResults.msg, tObj);
                        }
                    }

                }
            }
            if (blnSuccess === false && blnErrorShown === false) {
                oResults.error = new Error();
                oResults.error.name = "Save Company Contact Failure";
                oResults.error.message = "No results are available.";
                ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
            }
        } catch (err) {
            oResults.error = err
        }
        if (ngl.isFunction(this.onSave)) {
            this.onSave(oResults);
        }

    }

    this.saveAjaxErrorCallback = function (xhr, textStatus, error) {
        var oResults = new nglEventParameters();
        var tObj = this;
        //kendo.ui.progress($(document.body), false);
        oResults.source = "saveAjaxErrorCallback";
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed  
        oResults.error = new Error();
        oResults.error.name = "Save Company Contact Failure"
        oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);

        if (ngl.isFunction(this.onSave)) {
            this.onSave(oResults);
            //this.kendoWindowsObj.close();
        }
    }

    this.deleteSuccessCallback = function (data) {
        var oResults = new nglEventParameters();
        oResults.source = "deleteSuccessCallback";
        var tObj = this;
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed   
        //kendo.ui.progress($(document.body), false);
        this.rData = null;
        try {
            var blnSuccess = false;
            var blnErrorShown = false;
            var strValidationMsg = "";
            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                    oResults.error = new Error();
                    oResults.error.name = "Delete Company Contact Failure";
                    oResults.error.message = data.Errors;
                    blnErrorShown = true;
                    ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                }
            }
        } catch (err) {
            oResults.error = err
        }
        if (ngl.isFunction(this.onDelete)) {
            this.onDelete(oResults);
        }
    }

    this.deleteAjaxErrorCallback = function (xhr, textStatus, error) {
        var oResults = new nglEventParameters();
        var tObj = this;
        //kendo.ui.progress($(document.body), false);
        oResults.source = "deleteAjaxErrorCallback";
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed  
        oResults.error = new Error();
        oResults.error.name = "Delete Company Contact Failure"
        oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);

        if (ngl.isFunction(this.onDelete)) {
            this.onDelete(oResults);
        }
    }

    // Generic functions for all Widgets
    this.validateRequiredFields = function (data) {
        var oRet = new validationResults();
        if (typeof (data) === 'undefined' || !ngl.isObject(data)) {
            //something went wrong so throw an error. this should never happen
            ngl.showValidationMsg("Save Company Contact Validation Failed; No Data", "Please Contact Technical Support", tObj);
            oRet.Message = "Save Company Contact Validation Failed; No Data";
            return oRet;
        }

        var blnValidated = true;
        var sValidationMsg = "";
        var sSpacer = "";

        if (isEmpty(data.CompContName)) { blnValidated = false; sValidationMsg += sSpacer + "Contact Name"; sSpacer = ", "; }
        if (isEmpty(data.CompContEmail)) { blnValidated = false; sValidationMsg += sSpacer + "Contact Email"; sSpacer = ", "; }

        oRet.Success = blnValidated;
        oRet.Message = sValidationMsg;

        return oRet;
    }

    // Generic CRUD functions for all Widgets
    this.save = function () {
        var tObj = this;

        this.data = new CompContact(); //clear any old data

        var tfCompContTender = false;
        if ($('#chkEACompContTender').is(":checked")) { tfCompContTender = true; }

        var intCompControl = ngl.intTryParse($("#txtEACompContCompControl").val(), 0);
        var intCompContControl = ngl.intTryParse($("#txtEACompContControl").val(), 0);


        this.data.CompContControl = intCompContControl;
        this.data.CompContCompControl = intCompControl;
        this.data.CompContTender = tfCompContTender;
        this.data.CompContName = $("#txtEACompContName").data("kendoMaskedTextBox").value();
        this.data.CompContTitle = $("#txtEACompContTitle").data("kendoMaskedTextBox").value();
        this.data.CompCont800 = $("#txtEACompCont800").data("kendoMaskedTextBox").value();
        this.data.CompContPhone = $("#txtEACompContPhone").data("kendoMaskedTextBox").value();
        this.data.CompContPhoneExt = $("#txtEACompContPhoneExt").data("kendoMaskedTextBox").value();
        this.data.CompContFax = $("#txtEACompContFax").data("kendoMaskedTextBox").value();
        this.data.CompContEmail = $("#txtEACompContEmail").data("kendoMaskedTextBox").value();
        this.data.CompContUpdated = $("#txtEACompContUpdated").val();


        if (intCompContControl === 0) {
            //Only Validate on Insert
            var oValidationResults = this.validateRequiredFields(this.data);
            if (typeof (oValidationResults) === 'undefined' || !ngl.isObject(oValidationResults)) {
                ngl.showValidationMsg("Cannot validate Company Contact Information", "Invalid validation procedure, please contact technical support", tObj);
                return;
            }
            else { if (oValidationResults.Success === false) { ngl.showValidationMsg("Required Fields", oValidationResults.Message, tObj); return; } }
        }
        //save the changes

        var oCRUDCtrl = new nglRESTCRUDCtrl();
        oCRUDCtrl.update("CompCont/InsertOrUpdateCompCont", this.data, tObj, "saveSuccessCallback", "saveAjaxErrorCallback");

    }

    this.show = function () {
        var tObj = this;

        this.kendoWindowsObj = $("#wndCompContEA").kendoWindow({
            title: "Add New Company Contact",
            modal: true,
            visible: false,
            ////height: '75%',
            ////width: '75%',
            actions: ["save", "Minimize", "Maximize", "Close"],
            close: function (e) { tObj.close(e); }
        }).data("kendoWindow");

        if (this.kendoWindowsObjUploadEventAdded === 0) {
            $("#wndCompContEA").data("kendoWindow").wrapper.find(".k-svg-i-save").parent().click(function (e) { e.preventDefault(); tObj.save(); });
        }
        this.kendoWindowsObjUploadEventAdded = 1;

        //Get the CompName from the master record
        $("#lblCompContCompName").html("<h2>" + this.screenCompName + "</h2>");

        //if this is an edit load the data to the window
        if (typeof (this.data) !== 'undefined' && ngl.isObject(this.data)) {
            this.edit(this.data);
        }
        else {
            this.add();
        }

        this.kendoWindowsObj.center().open();
    }

    this.edit = function (data) {
        //load data to the screen
        var tObj = this;

        this.kendoWindowsObj.title("Edit Company Contact");
        //$("#wndCompMaintEA").data("kendoWindow").title("Edit Company");
        //Hide all the red "required field" * on Edit
        $(".redRequiredAsterik").hide();

        if (typeof (data) !== 'undefined' && data != null && ngl.isObject(data)) {
            //Verify CompContControl is a column in the grid and is not null or 0
            if ('CompContControl' in data) {
                if (typeof (data.CompContControl) === 'undefined' || data.CompContControl == null || data.CompContControl == 0) { ngl.showErrMsg("CompContControl Required", "CompContControl cannot be 0", null); return; }

                if ('CompContCompControl' in data) {
                    if (typeof (data.CompContCompControl) === 'undefined' || data.CompContCompControl == null || data.CompContCompControl == 0) { ngl.showErrMsg("CompContCompControl Required", "CompContCompControl cannot be 0", null); return; }

                    $("#txtEACompContControl").val(data.CompContControl); //Save the CompContControl so it can be accessed by the Save function
                    $("#txtEACompContCompControl").val(data.CompContCompControl); //Save the CompContCompControl so it can be accessed by the Save function

                    if ('CompContName' in data) { $("#txtEACompContName").data("kendoMaskedTextBox").value(data.CompContName); }
                    if ('CompContTitle' in data) { $("#txtEACompContTitle").data("kendoMaskedTextBox").value(data.CompContTitle); }
                    if ('CompCont800' in data) { $("#txtEACompCont800").data("kendoMaskedTextBox").value(data.CompCont800); }
                    if ('CompContPhone' in data) { $("#txtEACompContPhone").data("kendoMaskedTextBox").value(data.CompContPhone); }
                    if ('CompContPhoneExt' in data) { $("#txtEACompContPhoneExt").data("kendoMaskedTextBox").value(data.CompContPhoneExt); }
                    if ('CompContFax' in data) { $("#txtEACompContFax").data("kendoMaskedTextBox").value(data.CompContFax); }
                    if ('CompContEmail' in data) { $("#txtEACompContEmail").data("kendoMaskedTextBox").value(data.CompContEmail); }
                    if ('CompContTender' in data) {
                        if (data.CompContTender) { $("#chkEACompContTender").prop('checked', true); } else { $("#chkEACompContTender").prop('checked', false); }
                    }
                    if ('CompContUpdated' in data) { $("#txtEACompContUpdated").val(data.CompContUpdated); } //Save the Updated flag so it can be accessed by the Save function - optimistic concurrency
                }
                else { ngl.showErrMsg("CompContCompControl Required", "Row object does not contain property CompContCompControl.", null); return; }
            }
            else { ngl.showErrMsg("CompContControl Required", "Row object does not contain property CompContControl.", null); return; }
        }
        else { ngl.showErrMsg("Error", "There was a problem getting the row from the grid", null); return; }
    }

    this.add = function () {
        var tObj = this;

        this.kendoWindowsObj.title("Add New Company Contact");
        //Show all the red "required field" * on Add
        $(".redRequiredAsterik").show();

        $("#txtEACompContControl").val(0); //Save the CompContControl so it can be accessed by the Save function - set to 0 on Add
        $("#txtEACompContCompControl").val(this.screenCompControl); //Save the CompContCompControl so it can be accessed by the Save function
        $("#txtEACompContUpdated").val(""); //Save the Updated flag so it can be accessed by the Save function - optimistic concurrency -- reset to "" on Add
    }

    this.delete = function (data) {
        var tObj = this;
        var contName = "";

        if (!ngl.isNullOrUndefined(data) && ngl.isObject(data)) {
            //Verify CompContControl is a column in the grid and is not null or 0
            if ('CompContControl' in data) {
                if (ngl.isNullOrUndefined(data.CompContControl) || data.CompContControl == 0) { ngl.showErrMsg("CompContControl Required", "CompContControl cannot be 0", null); return; }
                this.deleteCompContControl = data.CompContControl;
                if ('CompContName' in data) { contName = data.CompContName; }
            }
            else { ngl.showErrMsg("CompContControl Required", "Row object does not contain property CompContControl.", null); return; }
        }
        else { ngl.showErrMsg("Error", "There was a problem getting the row from the grid", null); return; }

        var msgPrompt = "Are you sure that you want to delete this Contact?";
        if (!ngl.isNullOrUndefined(contName) && contName.length > 0) { msgPrompt = "Are you sure that you want to delete Contact " + contName + "?"; }

        ngl.OkCancelConfirmation(
            "Delete Confirmation",
            msgPrompt,
            400,
            400,
            tObj,
            "confirmDeleteContact");

    }

    this.close = function (e) {
        //** Always Set up the window for "Add New" on close **
        //** This way we only have to do special set up in the "Edit" method
        //reset values
        $("#txtEACompContControl").val(0);
        $("#txtEACompContCompControl").val(0);
        $("#txtEACompContUpdated").val("");
        $("#txtEACompContName").data("kendoMaskedTextBox").value("");
        $("#txtEACompContTitle").data("kendoMaskedTextBox").value("");
        $("#txtEACompCont800").data("kendoMaskedTextBox").value("");
        $("#txtEACompContPhone").data("kendoMaskedTextBox").value("");
        $("#txtEACompContPhoneExt").data("kendoMaskedTextBox").value("");
        $("#txtEACompContFax").data("kendoMaskedTextBox").value("");
        $("#txtEACompContEmail").data("kendoMaskedTextBox").value("");

        //Uncheck "CompContTender"
        $("#chkEACompContTender").prop('checked', false);

        //reset the CompName so we get it fresh from the screen everytime
        this.screenCompName = "";
        this.screenCompControl = 0;
        this.deleteCompContControl = 0;
    }

    //loadDefaults sets up the callbacks cbSelect and cbSave
    //all call backs return a reference to this object and a string message as parameters
    this.loadDefaults = function (pageVariable, saveCallBack, deleteCallBack) {

        this.onSave = saveCallBack;
        this.onDelete = deleteCallBack;
        this.data = new CompContact();
        this.lastErr = "";
        this.notifyOnError = true;
        this.notifyOnSuccess = true;
        this.notifyOnValidationFailure = true;
        var tObj = this;

        if (typeof (pageVariable) !== 'undefined' && ngl.isObject(pageVariable)) {

            this.kendoWindowsObj = pageVariable;

            //CompCont Fields
            $("#txtEACompContName").kendoMaskedTextBox();
            $("#txtEACompContTitle").kendoMaskedTextBox();
            $("#txtEACompCont800").kendoMaskedTextBox();
            $("#txtEACompContPhone").kendoMaskedTextBox({ mask: ngl.getPhoneNumberMask() });
            $("#txtEACompContPhoneExt").kendoMaskedTextBox();
            $("#txtEACompContFax").kendoMaskedTextBox({ mask: ngl.getPhoneNumberMask() });
            $("#txtEACompContEmail").kendoMaskedTextBox();

            //** Set up the window for "Add New" on initialize **
            //** This way we only have to do special set up in the "Edit" method

            //Uncheck "CompContTender"
            $("#chkEACompContTender").prop('checked', false);

            //Set the title of the window to "Add" -- NOTE: This gets set in show method every time it is called so no need to reset here

            //Set the values of these to 0 initially
            $("#txtEACompContControl").val(0);
            $("#txtEACompContCompControl").val(0);
            $("#txtEACompContUpdated").val("");

        } else {
            this.kendoWindowsObj = null;
        }

    }
}

//Added By LVV on 4/19/18 for v-8.1
//Widget for Edit/Add on Lane Maint - html data store in Views/LaneEAWindow.html
function LaneEAWndCtrl() {
    //Generic properties for all Widgets
    this.lastErr = "";
    this.notifyOnError = true;
    this.notifyOnSuccess = true;
    this.notifyOnValidationFailure = true;
    data: Lane;
    rData: null;
    onSave: null;
    onDelete: null;
    dsLEComps: kendo.data.DataSource;
    kendoWindowsObj: null;
    //Widget specific properties
    screenLEName: null;
    screenLEControl: null;
    deleteLaneControl: 0;
    this.EditError = false;
    this.blnIsEdit = false;

    this.kendoWindowsObjUploadEventAdded = 0;


    this.getLECompData = function () {

        this.dsLEComps = new kendo.data.DataSource({
            transport: {
                read: {
                    url: "api/Comp/GetLEComps/" + this.screenLEControl,
                    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }
                },
                parameterMap: function (options, operation) { return options; }
            },
            schema: {
                data: "Data",
                total: "Count",
                model: {
                    id: "CompControl",
                    fields: {
                        CompControl: { type: "number" },
                        CompName: { type: "string" },
                        CompStreetAddress1: { type: "string" },
                        CompStreetAddress2: { type: "string" },
                        CompStreetAddress3: { type: "string" },
                        CompStreetCity: { type: "string" },
                        CompStreetState: { type: "string" },
                        CompStreetZip: { type: "string" },
                        CompStreetCountry: { type: "string" }
                    }
                },
                errors: "Errors"
            },
            error: function (xhr, textStatus, error) { ngl.showErrMsg("GetLEComps JSON Response Error", formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failuire"), null); this.cancelChanges(); },
            serverPaging: false,
            serverSorting: false,
            serverFiltering: false
        });

        //var dsFrtClass = new kendo.data.DataSource({ data: this.sourceDiv });
        $("#ddlEALEComps").data("kendoDropDownList").setDataSource(this.dsLEComps);
        $("#ddlEALECompOrig").data("kendoDropDownList").setDataSource(this.dsLEComps);
        $("#ddlEALECompDest").data("kendoDropDownList").setDataSource(this.dsLEComps);
    }

    this.refreshLookupLists = function () {
        this.getLECompData();


        //$("#ddlEALEComps").data("kendoDropDownList").setDataSource(this.dsLEComps);
        //$('#ddlEALEComps').data('kendoDropDownList').dataSource.read();
    }

    //Origin
    this.clearOrigAddressFields = function () {
        $("#txtEALaneOrigName").data("kendoMaskedTextBox").value("");
        $("#txtEALaneOrigAddress1").data("kendoMaskedTextBox").value("");
        $("#txtEALaneOrigAddress2").data("kendoMaskedTextBox").value("");
        $("#txtEALaneOrigAddress3").data("kendoMaskedTextBox").value("");
        $("#txtEALaneOrigCity").data("kendoMaskedTextBox").value("");
        $("#txtEALaneOrigState").data("kendoMaskedTextBox").value("");
        $("#txtEALaneOrigZip").data("kendoMaskedTextBox").value("");
        $("#txtEALaneOrigCountry").data("kendoMaskedTextBox").value("");

        $("#txtEALaneOrigContactName").data("kendoMaskedTextBox").value("");
        $("#txtEALaneOrigContactPhone").data("kendoMaskedTextBox").value("");
        $("#txtEALaneOrigContactEmail").data("kendoMaskedTextBox").value("");
        $("#txtEALaneOrigEmergencyContactName").data("kendoMaskedTextBox").value("");
        $("#txtEALaneOrigEmergencyContactPhone").data("kendoMaskedTextBox").value("");
        //Modified by RHR for v-8.5.4.003 on 10/28/2023 changed from datetimepicker to kendoMaskedTextBox
        $("#tpEALaneRecHourStart").data("kendoMaskedTextBox").value("");
        $("#tpEALaneRecHourStop").data("kendoMaskedTextBox").value("");
    }
    this.origAddressFieldsEnabled = function (tfParam) {
        $("#txtEALaneOrigName").data("kendoMaskedTextBox").enable(tfParam);
        $("#txtEALaneOrigAddress1").data("kendoMaskedTextBox").enable(tfParam);
        $("#txtEALaneOrigAddress2").data("kendoMaskedTextBox").enable(tfParam);
        $("#txtEALaneOrigAddress3").data("kendoMaskedTextBox").enable(tfParam);
        $("#txtEALaneOrigCity").data("kendoMaskedTextBox").enable(tfParam);
        $("#txtEALaneOrigState").data("kendoMaskedTextBox").enable(tfParam);
        $("#txtEALaneOrigZip").data("kendoMaskedTextBox").enable(tfParam);
        $("#txtEALaneOrigCountry").data("kendoMaskedTextBox").enable(tfParam);
    }

    //Dest
    this.clearDestAddressFields = function () {
        $("#txtEALaneDestName").data("kendoMaskedTextBox").value("");
        $("#txtEALaneDestAddress1").data("kendoMaskedTextBox").value("");
        $("#txtEALaneDestAddress2").data("kendoMaskedTextBox").value("");
        $("#txtEALaneDestAddress3").data("kendoMaskedTextBox").value("");
        $("#txtEALaneDestCity").data("kendoMaskedTextBox").value("");
        $("#txtEALaneDestState").data("kendoMaskedTextBox").value("");
        $("#txtEALaneDestZip").data("kendoMaskedTextBox").value("");
        $("#txtEALaneDestCountry").data("kendoMaskedTextBox").value("");

        $("#txtEALaneDestContactName").data("kendoMaskedTextBox").value("");
        $("#txtEALaneDestContactPhone").data("kendoMaskedTextBox").value("");
        $("#txtEALaneDestContactEmail").data("kendoMaskedTextBox").value("");
        $("#txtEALaneDestEmergencyContactName").data("kendoMaskedTextBox").value("");
        $("#txtEALaneDestEmergencyContactPhone").data("kendoMaskedTextBox").value("");

        $("#tpEALaneDestHourStart").data("kendoMaskedTextBox").value("");
        $("#tpEALaneDestHourStop").data("kendoMaskedTextBox").value("");
    }
    this.destAddressFieldsEnabled = function (tfParam) {
        $("#txtEALaneDestName").data("kendoMaskedTextBox").enable(tfParam);
        $("#txtEALaneDestAddress1").data("kendoMaskedTextBox").enable(tfParam);
        $("#txtEALaneDestAddress2").data("kendoMaskedTextBox").enable(tfParam);
        $("#txtEALaneDestAddress3").data("kendoMaskedTextBox").enable(tfParam);
        $("#txtEALaneDestCity").data("kendoMaskedTextBox").enable(tfParam);
        $("#txtEALaneDestState").data("kendoMaskedTextBox").enable(tfParam);
        $("#txtEALaneDestZip").data("kendoMaskedTextBox").enable(tfParam);
        $("#txtEALaneDestCountry").data("kendoMaskedTextBox").enable(tfParam);
    }

    this.toggleInboundOutbound = function (control) {
        switch (control) {
            case 3:     //INBOUND                                                                       
                /* DestCompControl is required */
                $("#ddlEALECompDest").data("kendoDropDownList").readonly(false); //enable the dropdownlist
                this.clearDestAddressFields();
                this.destAddressFieldsEnabled(false); //disable the text fields
                //Don't allow them to select "None" in the Dest ddl
                //** TODO ** FIGURE OUT HOW TO GRAY THE ITEM OUT
                //$("#ddlEALECompDest_listbox .k-item")[0].disabled = true;

                /* OriginCompControl is optional */
                $("#ddlEALECompOrig").data("kendoDropDownList").readonly(false); //enable the dropdownlist
                $("#ddlEALECompOrig").data("kendoDropDownList").select(0); //Select "None"
                this.origAddressFieldsEnabled(true); //enable the text fields
                break;
            case 1:    //OUTBOUND
                /* OriginCompControl is required */
                $("#ddlEALECompOrig").data("kendoDropDownList").readonly(false); //enable the dropdownlist
                this.clearOrigAddressFields();
                this.origAddressFieldsEnabled(false); //disable the text fields
                //Don't allow them to select "None" in the Dest ddl
                //** TODO ** FIGURE OUT HOW TO GRAY THE ITEM OUT
                //$("#ddlEALECompOrig_listbox .k-item")[0].disabled = true;

                /* DestCompControl is optional */
                $("#ddlEALECompDest").data("kendoDropDownList").readonly(false); //enable the dropdownlist
                $("#ddlEALECompDest").data("kendoDropDownList").select(0); //Select "None"
                this.destAddressFieldsEnabled(true); //enable the text fields
                break;
            case 2:    //TRANSFER
                /* OriginCompControl is required */
                $("#ddlEALECompOrig").data("kendoDropDownList").readonly(false); //enable the dropdownlist
                this.clearOrigAddressFields();
                this.origAddressFieldsEnabled(false); //disable the text fields
                //Don't allow them to select "None" in the Dest ddl
                //** TODO ** FIGURE OUT HOW TO GRAY THE ITEM OUT
                //$("#ddlEALECompOrig_listbox .k-item")[0].disabled = true;

                /* DestCompControl is required */
                $("#ddlEALECompDest").data("kendoDropDownList").readonly(false); //enable the dropdownlist
                this.clearDestAddressFields();
                this.destAddressFieldsEnabled(false); //disable the text fields
                //Don't allow them to select "None" in the Dest ddl
                //** TODO ** FIGURE OUT HOW TO GRAY THE ITEM OUT
                //$("#ddlEALECompDest_listbox .k-item")[0].disabled = true;
                break;
            default:
                break;
        }
    }

    this.confirmDeleteLane = function (iRet) {
        if (typeof (iRet) === 'undefined' || iRet === null || iRet === 0) { return; }

        if (!ngl.isNullOrUndefined(this.deleteLaneControl) && this.deleteLaneControl > 0) {
            var tObj = this;
            var oCRUDCtrl = new nglRESTCRUDCtrl();
            var blnRet = oCRUDCtrl.delete("Lane/DeleteLane", this.deleteLaneControl, tObj, "deleteSuccessCallback", "deleteAjaxErrorCallback");
        }
    }

    //I need allow for the value returned to be null or ''
    this.getLocalTimeString = function (sVal, sDefault) {
        //if (typeof (sDefault) === 'undefined' || sDefault === null || ngl.isNullOrWhitespace(sDefault)) { sDefault = '12:00'; }
        if (typeof (sVal) === 'undefined' || sVal === null || ngl.isNullOrWhitespace(sVal)) { return sDefault; }
        //console.log("Get Time String");
        //console.log(sVal);
        var timestamp = Date.parse(sVal)

        if (isNaN(timestamp) == false) {
            var d = new Date(timestamp);
            var sHour = d.getHours().toString();
            var sMIn = d.getMinutes().toString();
            var sRet = sHour.concat(":", sMIn);
            return sRet; //d.toLocaleTimeString();
        } else { return sDefault; }
    }


    //Generic Call back functions for all Widgets
    this.saveSuccessCallback = function (data) {
        // debugger;
        var oResults = new nglEventParameters();
        oResults.source = "saveSuccessCallback";
        var tObj = this;
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed   
        //kendo.ui.progress($(document.body), false);
        this.rData = null;
        try {
            kendo.ui.progress($("#wndLaneEA"), false);
        } catch (err) { ngl.showInfoNotification("Cancel Progress Failure", "Your process is complete but the wait cursor could not be turned off, please refresh the page if the wait cursor does not go away.", null); }
        try {
            var blnSuccess = false;
            var blnErrorShown = false;
            var strValidationMsg = "";
            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                    oResults.error = new Error();
                    oResults.error.name = "Save Lane Failure";
                    oResults.error.message = data.Errors;
                    blnErrorShown = true;
                    ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                }
                else {
                    this.rData = data.Data;
                    if (typeof (data.Data) !== 'undefined' && data.Data != null && ngl.isArray(data.Data)) {
                        if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                            blnSuccess = true;
                            oResults.data = data.Data;
                            oResults.datatype = "string";
                            oResults.msg = "<h3>Success</h3>" + data.Data[0]
                            //Only close the window if the save was successful
                            if (data.Data[0].Success == true) { this.kendoWindowsObj.close(); }
                            //this.kendoWindowsObj.close();
                            ;
                            //ngl.showSuccessMsg(oResults.msg, tObj);
                        }
                    }
                }
            }
            if (blnSuccess === false && blnErrorShown === false) {
                oResults.error = new Error();
                oResults.error.name = "Save Lane Failure";
                oResults.error.message = "No results are available.";
                ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
            }
        } catch (err) { oResults.error = err; }
        if (ngl.isFunction(this.onSave)) { this.onSave(oResults); }
    }
    this.saveAjaxErrorCallback = function (xhr, textStatus, error) {
        var oResults = new nglEventParameters();
        var tObj = this;
        //kendo.ui.progress($(document.body), false);
        oResults.source = "saveAjaxErrorCallback";
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed  
        oResults.error = new Error();
        oResults.error.name = "Save Lane Failure"
        oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);

        if (ngl.isFunction(this.onSave)) {
            this.onSave(oResults);
            //this.kendoWindowsObj.close();
        }
    }

    this.deleteSuccessCallback = function (data) {
        var oResults = new nglEventParameters();
        oResults.source = "deleteSuccessCallback";
        var tObj = this;
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed   
        //kendo.ui.progress($(document.body), false);
        this.rData = null;
        try {
            var blnSuccess = false;
            var blnErrorShown = false;
            var strValidationMsg = "";
            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                    oResults.error = new Error();
                    oResults.error.name = "Delete Lane Failure";
                    oResults.error.message = data.Errors;
                    blnErrorShown = true;
                    ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                }
            }
        } catch (err) {
            oResults.error = err
        }
        if (ngl.isFunction(this.onDelete)) {
            this.onDelete(oResults);
        }
    }

    this.deleteAjaxErrorCallback = function (xhr, textStatus, error) {
        var oResults = new nglEventParameters();
        var tObj = this;
        //kendo.ui.progress($(document.body), false);
        oResults.source = "deleteAjaxErrorCallback";
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed  
        oResults.error = new Error();
        oResults.error.name = "Delete Lane Failure"
        oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);

        if (ngl.isFunction(this.onDelete)) {
            this.onDelete(oResults);
        }
    }

    // Generic functions for all Widgets
    this.validateRequiredFields = function (data, blnIsTransfer) {
        var oRet = new validationResults();
        if (typeof (data) === 'undefined' || !ngl.isObject(data)) {
            //something went wrong so throw an error. this should never happen
            ngl.showValidationMsg("Save Lane Validation Failed; No Data", "Please Contact Technical Support", tObj);
            oRet.Message = "Save Lane Validation Failed; No Data";
            return oRet;
        }
        var blnValidated = true;
        var sValidationMsg = "";
        var sSpacer = "";

        if (isEmpty(data.LaneName)) { blnValidated = false; sValidationMsg += sSpacer + "Lane Name"; sSpacer = ", "; }
        
        if (isEmpty(data.LaneNumber)) {
            if (data.LaneControl != 0) { 
                blnValidated = false; sValidationMsg += sSpacer + "Lane Number"; sSpacer = ", ";
            }
        }

        if (isEmpty(data.LaneLegalEntity)) { blnValidated = false; sValidationMsg += sSpacer + "Legal Entity"; sSpacer = ", "; }
        if (ngl.isNullOrUndefined(data.LaneCompControl) || data.LaneCompControl === 0) { blnValidated = false; sValidationMsg += sSpacer + "Company"; sSpacer = ", "; }
        if (ngl.isNullOrUndefined(data.LaneActive)) { blnValidated = false; sValidationMsg += sSpacer + "Active"; sSpacer = ", "; }

        if (ngl.isNullOrUndefined(data.LaneOriginAddressUse)) {
            blnValidated = false; sValidationMsg += sSpacer + "Direction"; sSpacer = ", ";
        }
        else {
            // Modified by RHR for v-8.5.4.004 on 01/22/2024 none is now allowed on both
            //if (data.LaneOriginAddressUse) {
            //    //Inbound so LaneDestCompControl is required 
            //    if (ngl.isNullOrUndefined(data.LaneDestCompControl) || data.LaneDestCompControl === 0) { blnValidated = false; sValidationMsg += sSpacer + "Dest Company"; sSpacer = ", "; }
            //}
            //else {
            //    if (blnIsTransfer) {
            //        //Transfer so both LaneOrigCompControl and LaneDestCompControl are required
            //        if (ngl.isNullOrUndefined(data.LaneDestCompControl) || data.LaneDestCompControl === 0) { blnValidated = false; sValidationMsg += sSpacer + "Dest Company"; sSpacer = ", "; }
            //        if (ngl.isNullOrUndefined(data.LaneOrigCompControl) || data.LaneOrigCompControl === 0) { blnValidated = false; sValidationMsg += sSpacer + "Orig Company"; sSpacer = ", "; }
            //    }
            //    else {
            //        //Outbound so LaneOrigCompControl is required
            //        if (ngl.isNullOrUndefined(data.LaneOrigCompControl) || data.LaneOrigCompControl === 0) { blnValidated = false; sValidationMsg += sSpacer + "Orig Company"; sSpacer = ", "; }
            //    }
            //}
        }

        //Orig
        if (isEmpty(data.LaneOrigName)) { blnValidated = false; sValidationMsg += sSpacer + "Orig Name"; sSpacer = ", "; }
        if (isEmpty(data.LaneOrigAddress1)) { blnValidated = false; sValidationMsg += sSpacer + "Orig Address 1"; sSpacer = ", "; }
        if (isEmpty(data.LaneOrigCity)) { blnValidated = false; sValidationMsg += sSpacer + "Orig City"; sSpacer = ", "; }
        if (isEmpty(data.LaneOrigState)) { blnValidated = false; sValidationMsg += sSpacer + "Orig State"; sSpacer = ", "; }
        if (isEmpty(data.LaneOrigZip)) { blnValidated = false; sValidationMsg += sSpacer + "Orig Postal Code"; sSpacer = ", "; }
        if (isEmpty(data.LaneOrigCountry)) { blnValidated = false; sValidationMsg += sSpacer + "Orig Country"; sSpacer = ", "; }
        if (isEmpty(data.LaneOrigContactName)) { blnValidated = false; sValidationMsg += sSpacer + "Orig Cont Name"; sSpacer = ", "; }
        if (isEmpty(data.LaneOrigContactEmail)) { blnValidated = false; sValidationMsg += sSpacer + "Orig Cont Email"; sSpacer = ", "; }
        if (isEmpty(data.LaneOrigEmergencyContactPhone)) { blnValidated = false; sValidationMsg += sSpacer + "Orig Emergency Name"; sSpacer = ", "; }
        if (isEmpty(data.LaneOrigEmergencyContactName)) { blnValidated = false; sValidationMsg += sSpacer + "Orig Emergency Phone"; sSpacer = ", "; }
        //Dest
        if (isEmpty(data.LaneDestName)) { blnValidated = false; sValidationMsg += sSpacer + "Dest Name"; sSpacer = ", "; }
        if (isEmpty(data.LaneDestAddress1)) { blnValidated = false; sValidationMsg += sSpacer + "Dest Address 1"; sSpacer = ", "; }
        if (isEmpty(data.LaneDestCity)) { blnValidated = false; sValidationMsg += sSpacer + "Dest City"; sSpacer = ", "; }
        if (isEmpty(data.LaneDestState)) { blnValidated = false; sValidationMsg += sSpacer + "Dest State"; sSpacer = ", "; }
        if (isEmpty(data.LaneDestZip)) { blnValidated = false; sValidationMsg += sSpacer + "Dest Postal Code"; sSpacer = ", "; }
        if (isEmpty(data.LaneDestCountry)) { blnValidated = false; sValidationMsg += sSpacer + "Dest Country"; sSpacer = ", "; }
        if (isEmpty(data.LaneDestContactName)) { blnValidated = false; sValidationMsg += sSpacer + "Dest Cont Name"; sSpacer = ", "; }
        if (isEmpty(data.LaneDestContactEmail)) { blnValidated = false; sValidationMsg += sSpacer + "Dest Cont Email"; sSpacer = ", "; }
        if (isEmpty(data.LaneDestEmergencyContactPhone)) { blnValidated = false; sValidationMsg += sSpacer + "Dest Emergency Name"; sSpacer = ", "; }
        if (isEmpty(data.LaneDestEmergencyContactName)) { blnValidated = false; sValidationMsg += sSpacer + "Dest Emergency Phone"; sSpacer = ", "; }


        oRet.Success = blnValidated;
        oRet.Message = sValidationMsg;

        return oRet;
    }

    // Generic CRUD functions for all Widgets
    this.save = function () {
        var tObj = this;

        this.data = new Lane(); //clear any old data

        var tfLaneActive = false;
        var tfLaneAppt = false;
        var tfLaneAptDelivery = false;
        var blnIsTransfer = false;
        var tfLaneIsTransLoad = false;
        var tfLaneIsCrossDockFacility = false;
        var tfLaneDefaultCarrierUse = false;
        var tfLaneAutoTenderFlag = false;
        var tfLaneDoNotInvoice = false;
        var tfLaneAllowInterline = false;
        var tfLaneCascadingDispatchingFlag = false;
        var tfLaneRestrictCarrierSelection = false;
        var tfLaneWarnOnRestrictedCarrierSelection = false;
        var tfLanePalletExchange = false;
        var tfLaneAllowCarrierBookApptByEmail = false;
        var tfLaneRequireCarrierAuthBookApptByEmail = false;
        var tfLaneUseCarrieContEmailForBookApptByEmail = false;
        var tfLaneTransLeadTimeUseMasterLane = false;

        if ($('#chkEALaneActive').is(":checked")) { tfLaneActive = true; }
        if ($('#chkEALaneAppt').is(":checked")) { tfLaneAppt = true; }
        if ($('#chkEALaneAptDelivery').is(":checked")) { tfLaneAptDelivery = true; }
        if ($('#chkEALaneIsTransLoad').is(":checked")) { tfLaneIsTransLoad = true; }
        if ($('#chkEALaneIsCrossDockFacility').is(":checked")) { tfLaneIsCrossDockFacility = true; }
        if ($('#chkEALaneDefaultCarrierUse').is(":checked")) { tfLaneDefaultCarrierUse = true; }
        if ($('#chkEALaneAutoTenderFlag').is(":checked")) { tfLaneAutoTenderFlag = true; }
        if ($('#chkEALaneDoNotInvoice').is(":checked")) { tfLaneDoNotInvoice = true; }
        if ($('#chkEALaneAllowInterline').is(":checked")) { tfLaneAllowInterline = true; }
        if ($('#chkEALaneCascadingDispatchingFlag').is(":checked")) { tfLaneCascadingDispatchingFlag = true; }
        if ($('#chkEALaneRestrictCarrierSelection').is(":checked")) { tfLaneRestrictCarrierSelection = true; }
        if ($('#chkEALaneWarnOnRestrictedCarrierSelection').is(":checked")) { tfLaneWarnOnRestrictedCarrierSelection = true; }
        if ($('#chkEALanePalletExchange').is(":checked")) { tfLanePalletExchange = true; }
        if ($('#chkEALaneAllowCarrierBookApptByEmail').is(":checked")) { tfLaneAllowCarrierBookApptByEmail = true; }
        if ($('#chkEALaneRequireCarrierAuthBookApptByEmail').is(":checked")) { tfLaneRequireCarrierAuthBookApptByEmail = true; }
        if ($('#chkEALaneUseCarrieContEmailForBookApptByEmail').is(":checked")) { tfLaneUseCarrieContEmailForBookApptByEmail = true; }
        if ($('#chkEALaneTransLeadTimeUseMasterLane').is(":checked")) { tfLaneTransLeadTimeUseMasterLane = true; }

        var intLaneControl = ngl.intTryParse($("#txtEALaneControl").val(), 0);

        var diComp = $("#ddlEALEComps").data("kendoDropDownList").dataItem();
        var diCompOrig = $("#ddlEALECompOrig").data("kendoDropDownList").dataItem();
        var diCompDest = $("#ddlEALECompDest").data("kendoDropDownList").dataItem();
        var diTrans = $("#ddlEALaneTrans").data("kendoDropDownList").dataItem();
        var diTemp = $("#ddlEATempType").data("kendoDropDownList").dataItem();
        var diRouteGuide = $("#ddlEALaneRouteGuide").data("kendoDropDownList").dataItem();
        var diRouteType = $("#ddlEALaneRouteTypeCode").data("kendoDropDownList").dataItem();
        var diCostType = $("#ddlEALaneBFCType").data("kendoDropDownList").dataItem();
        var diModeType = $("#ddlEALaneModeType").data("kendoDropDownList").dataItem();
        var diPalletType = $("#ddlEALanePalletType").data("kendoDropDownList").dataItem();
        var diLaneTransLeadTimeLocationOption = $("#ddlEALaneTransLeadTimeLocationOption").data("kendoDropDownList").dataItem();
        var diLaneTransLeadTimeCalcType = $("#ddlEALaneTransLeadTimeCalcType").data("kendoDropDownList").dataItem();


        if (this.blnIsEdit) {
            //EDIT
            var tfLaneOrigAddressUse = false;
            if ($('#chkEALaneOrigAddressUse').is(":checked")) { tfLaneOrigAddressUse = true; }
            //console.log('Edit chkEALaneOrigAddressUse flag')
            //console.log(tfLaneOrigAddressUse)
            this.data.LaneOriginAddressUse = tfLaneOrigAddressUse;
           
        }
        if ($("#ddlEALTTransType")) {


            //ADD
            var diDir = $("#ddlEALTTransType").data("kendoDropDownList").dataItem();
            //console.log('ddlEALTTransType flag')
            //console.log(diDir)
            if (diDir.Control === 3) {
                this.data.LaneOriginAddressUse = true;
            }
            else {
                this.data.LaneOriginAddressUse = false;
                if (diDir.Control === 2) { blnIsTransfer = true; }
            }
            //console.log('diDir');
            //console.log(diDir);
            //console.log('diDir.Control');
            //console.log(diDir.Control);
            this.data.LTTransType = diDir.Control;
        } //else { console.log('no LTTransType'); }
        //Lane
        this.data.LaneControl = intLaneControl;

        if (typeof (diComp) !== 'undefined' && typeof (diComp.CompControl) !== 'undefined') {
            this.data.LaneCompControl = diComp.CompControl;
        }
        if (typeof (diTrans) !== 'undefined' && typeof (diTrans.Description) !== 'undefined') {
            this.data.LaneTransType = diTrans.Description;
        }
        if (typeof (diTemp) !== 'undefined' && typeof (diTemp.Control) !== 'undefined') {
            this.data.LaneTempType = diTemp.Control;
        }

        this.data.LaneName = $("#txtEALaneName").data("kendoMaskedTextBox").value();
        this.data.LaneNumber = $("#txtEALaneNumber").data("kendoMaskedTextBox").value();
        this.data.LaneLegalEntity = $("#txtEALaneLegalEntity").data("kendoMaskedTextBox").value();
        this.data.LaneActive = tfLaneActive;
        this.data.LaneNumberMaster = $("#txtEALaneNumberMaster").data("kendoMaskedTextBox").value(); //Added By LVV on 10/12/20 for v-8.3.0.001 Task #20201012142747 - Add Fields to Lane Maint
        this.data.LaneNameMaster = $("#txtEALaneNameMaster").data("kendoMaskedTextBox").value(); //Added By LVV on 10/12/20 for v-8.3.0.001 Task #20201012142747 - Add Fields to Lane Maint
        this.data.LaneCarrierEquipmentCodes = $("#txtEALaneCarrierEquipmentCodes").data("kendoMaskedTextBox").value(); //Added By LVV on 10/12/20 for v-8.3.0.001 Task #20201012142747 - Add Fields to Lane Maint
        //Comments
        this.data.LaneComments = $("#txtEALaneComments").data("kendoMaskedTextBox").value();
        this.data.LaneCommentsConfidential = $("#txtEALaneCommentsConfidential").data("kendoMaskedTextBox").value();
        this.data.LaneUser1 = $("#txtEALaneUser1").data("kendoMaskedTextBox").value(); //Added By LVV on 10/12/20 for v-8.3.0.001 Task #20201012142747 - Add Fields to Lane Maint
        this.data.LaneUser2 = $("#txtEALaneUser2").data("kendoMaskedTextBox").value(); //Added By LVV on 10/12/20 for v-8.3.0.001 Task #20201012142747 - Add Fields to Lane Maint
        this.data.LaneUser3 = $("#txtEALaneUser3").data("kendoMaskedTextBox").value(); //Added By LVV on 10/12/20 for v-8.3.0.001 Task #20201012142747 - Add Fields to Lane Maint
        this.data.LaneUser4 = $("#txtEALaneUser4").data("kendoMaskedTextBox").value(); //Added By LVV on 10/12/20 for v-8.3.0.001 Task #20201012142747 - Add Fields to Lane Maint        
        //Capacity Settings
        this.data.LaneTLWgt = $("#txtEALaneTLWgt").data("kendoNumericTextBox").value(); //Added By LVV on 5/18/20 - default TL capcity settings to 0 on create new lane from 365
        this.data.LaneTLCases = $("#txtEALaneTLCases").data("kendoNumericTextBox").value(); //Added By LVV on 5/18/20 - default TL capcity settings to 0 on create new lane from 365
        this.data.LaneTLCube = $("#txtEALaneTLCube").data("kendoNumericTextBox").value(); //Added By LVV on 5/18/20 - default TL capcity settings to 0 on create new lane from 365
        this.data.LaneTLPL = $("#txtEALaneTLPL").data("kendoNumericTextBox").value(); //Added By LVV on 5/18/20 - default TL capcity settings to 0 on create new lane from 365
        //Orig
        if (typeof (diCompOrig) !== 'undefined' && typeof (diCompOrig.CompControl) !== 'undefined') {
            this.data.LaneOrigCompControl = diCompOrig.CompControl;
        }
        this.data.LaneOrigName = $("#txtEALaneOrigName").data("kendoMaskedTextBox").value();
        this.data.LaneOrigAddress1 = $("#txtEALaneOrigAddress1").data("kendoMaskedTextBox").value();
        this.data.LaneOrigAddress2 = $("#txtEALaneOrigAddress2").data("kendoMaskedTextBox").value();
        this.data.LaneOrigAddress3 = $("#txtEALaneOrigAddress3").data("kendoMaskedTextBox").value();
        this.data.LaneOrigCity = $("#txtEALaneOrigCity").data("kendoMaskedTextBox").value();
        this.data.LaneOrigState = $("#txtEALaneOrigState").data("kendoMaskedTextBox").value();
        this.data.LaneOrigZip = $("#txtEALaneOrigZip").data("kendoMaskedTextBox").value();
        this.data.LaneOrigCountry = $("#txtEALaneOrigCountry").data("kendoMaskedTextBox").value();
        this.data.LaneOrigContactName = $("#txtEALaneOrigContactName").data("kendoMaskedTextBox").value();
        this.data.LaneOrigContactPhone = $("#txtEALaneOrigContactPhone").data("kendoMaskedTextBox").value();
        this.data.LaneOrigContactEmail = $("#txtEALaneOrigContactEmail").data("kendoMaskedTextBox").value();
        this.data.LaneOrigEmergencyContactName = $("#txtEALaneOrigEmergencyContactName").data("kendoMaskedTextBox").value();
        this.data.LaneOrigEmergencyContactPhone = $("#txtEALaneOrigEmergencyContactPhone").data("kendoMaskedTextBox").value();
        this.data.LaneAppt = tfLaneAppt;
        //this.data.LaneRecHourStart = this.getLocalTimeString($("#tpEALaneRecHourStart").data("kendoTimePicker").value(), '');
        //this.data.LaneRecHourStop = this.getLocalTimeString($("#tpEALaneRecHourStop").data("kendoTimePicker").value(), '');

        //Modified by RHR for v-8.5.4.003 on 10/28/2023 changed from datetimepicker to kendoMaskedTextBox
        this.data.LaneRecHourStart = $("#tpEALaneRecHourStart").data("kendoMaskedTextBox").value();
        this.data.LaneRecHourStop = $("#tpEALaneRecHourStop").data("kendoMaskedTextBox").value();

        //Dest
        if (typeof (diCompDest) !== 'undefined' && typeof (diCompDest.CompControl) !== 'undefined') {
            this.data.LaneDestCompControl = diCompDest.CompControl;
        }
        this.data.LaneDestName = $("#txtEALaneDestName").data("kendoMaskedTextBox").value();
        this.data.LaneDestAddress1 = $("#txtEALaneDestAddress1").data("kendoMaskedTextBox").value();
        this.data.LaneDestAddress2 = $("#txtEALaneDestAddress2").data("kendoMaskedTextBox").value();
        this.data.LaneDestAddress3 = $("#txtEALaneDestAddress3").data("kendoMaskedTextBox").value();
        this.data.LaneDestCity = $("#txtEALaneDestCity").data("kendoMaskedTextBox").value();
        this.data.LaneDestState = $("#txtEALaneDestState").data("kendoMaskedTextBox").value();
        this.data.LaneDestZip = $("#txtEALaneDestZip").data("kendoMaskedTextBox").value();
        this.data.LaneDestCountry = $("#txtEALaneDestCountry").data("kendoMaskedTextBox").value();
        this.data.LaneDestContactName = $("#txtEALaneDestContactName").data("kendoMaskedTextBox").value();
        this.data.LaneDestContactPhone = $("#txtEALaneDestContactPhone").data("kendoMaskedTextBox").value();
        this.data.LaneDestContactEmail = $("#txtEALaneDestContactEmail").data("kendoMaskedTextBox").value();
        this.data.LaneDestEmergencyContactName = $("#txtEALaneDestEmergencyContactName").data("kendoMaskedTextBox").value();
        this.data.LaneDestEmergencyContactPhone = $("#txtEALaneDestEmergencyContactPhone").data("kendoMaskedTextBox").value();
        this.data.LaneAptDelivery = tfLaneAptDelivery;
        //this.data.LaneDestHourStart = this.getLocalTimeString($("#tpEALaneDestHourStart").data("kendoTimePicker").value(), '');
        //this.data.LaneDestHourStop = this.getLocalTimeString($("#tpEALaneDestHourStop").data("kendoTimePicker").value(), '');

        //Modified by RHR for v-8.5.4.003 on 10/28/2023 changed from datetimepicker to kendoMaskedTextBox
        this.data.LaneDestHourStart = $("#tpEALaneDestHourStart").data("kendoMaskedTextBox").value();
        this.data.LaneDestHourStop = $("#tpEALaneDestHourStop").data("kendoMaskedTextBox").value();

        //Added By LVV on 10/12/20 for v-8.3.0.001 Task #20201012142747 - Add Fields to Lane Maint       
        //Dynamic Routing
        this.data.LaneBenchMiles = $("#txtEALaneBenchMiles").data("kendoNumericTextBox").value();
        this.data.LaneLatitude = $("#txtEALaneLatitude").data("kendoNumericTextBox").value();
        this.data.LaneLongitude = $("#txtEALaneLongitude").data("kendoNumericTextBox").value();
        this.data.LaneIsTransLoad = tfLaneIsTransLoad;
        this.data.LaneIsCrossDockFacility = tfLaneIsCrossDockFacility;
        //Static Routing
        if (typeof (diPalletType) !== 'undefined' && typeof (diPalletType.Name) !== 'undefined') {
            this.data.LanePalletType = diPalletType.Name;
        }
        if (typeof (diRouteGuide) !== 'undefined' && typeof (diRouteGuide.Control) !== 'undefined') {
            this.data.LaneRouteGuideControl = diRouteGuide.Control;
        }

        this.data.LaneDefaultRouteSequence = $("#txtEALaneDefaultRouteSequence").data("kendoNumericTextBox").value();
        if (typeof (diRouteType) !== 'undefined' && typeof (diRouteType.Control) !== 'undefined') {
            this.data.LaneRouteTypeCode = diRouteType.Control;
        }

        //Options
        this.data.LaneDefaultCarrierUse = tfLaneDefaultCarrierUse;
        this.data.LaneAutoTenderFlag = tfLaneAutoTenderFlag;
        this.data.LaneDoNotInvoice = tfLaneDoNotInvoice;
        this.data.LaneAllowInterline = tfLaneAllowInterline;
        this.data.LaneCascadingDispatchingFlag = tfLaneCascadingDispatchingFlag;
        this.data.LaneRestrictCarrierSelection = tfLaneRestrictCarrierSelection;
        this.data.LaneWarnOnRestrictedCarrierSelection = tfLaneWarnOnRestrictedCarrierSelection;
        this.data.LanePalletExchange = tfLanePalletExchange;
        //Added by RHR for v-8.4.0.003 on 07/15/2021  Book Appointment by Email Options
        this.data.LaneAllowCarrierBookApptByEmail = tfLaneAllowCarrierBookApptByEmail;
        this.data.LaneRequireCarrierAuthBookApptByEmail = tfLaneRequireCarrierAuthBookApptByEmail;
        this.data.LaneUseCarrieContEmailForBookApptByEmail = tfLaneUseCarrieContEmailForBookApptByEmail;
        this.data.LaneCarrierBookApptviaTokenEmail = $("#txtEALaneCarrierBookApptviaTokenEmail").data("kendoMaskedTextBox").value();
        this.data.LaneCarrierBookApptviaTokenFailEmail = $("#txtEALaneCarrierBookApptviaTokenFailEmail").data("kendoMaskedTextBox").value();
        this.data.LaneCarrierBookApptviaTokenFailPhone = $("#txtEALaneCarrierBookApptviaTokenFailPhone").data("kendoMaskedTextBox").value();
        if (typeof (diLaneTransLeadTimeLocationOption) !== 'undefined' && typeof (diLaneTransLeadTimeLocationOption.Control) !== 'undefined') {
            this.data.LaneTransLeadTimeLocationOption = diLaneTransLeadTimeLocationOption.Control;
        }
        if (typeof (diLaneTransLeadTimeCalcType) !== 'undefined' && typeof (diLaneTransLeadTimeCalcType.Control) !== 'undefined') {
            this.data.LaneTransLeadTimeCalcType = diLaneTransLeadTimeCalcType.Control;
        }
        this.data.LaneTransLeadTimeUseMasterLane = tfLaneTransLeadTimeUseMasterLane;


        //Default Provider
        this.data.LaneDefaultCarrierContact = $("#txtEALaneDefaultCarrierContact").data("kendoMaskedTextBox").value();
        this.data.LaneDefaultCarrierPhone = $("#txtEALaneDefaultCarrierPhone").data("kendoMaskedTextBox").value();
        this.data.LaneStops = $("#txtEALaneStops").data("kendoNumericTextBox").value();
        //Config
        this.data.LaneOLTBenchmark = $("#txtEALaneOLTBenchmark").data("kendoNumericTextBox").value();
        this.data.LaneTLTBenchmark = $("#txtEALaneTLTBenchmark").data("kendoNumericTextBox").value();
        this.data.LaneRecMinIn = $("#txtEALaneRecMinIn").data("kendoNumericTextBox").value();
        this.data.LaneRecMinUnload = $("#txtEALaneRecMinUnload").data("kendoNumericTextBox").value();
        this.data.LaneRecMinOut = $("#txtEALaneRecMinOut").data("kendoNumericTextBox").value();
        this.data.LaneBFC = $("#txtEALaneBFC").data("kendoNumericTextBox").value();
        this.data.LanePrimaryBuyer = $("#txtEALanePrimaryBuyer").data("kendoMaskedTextBox").value();
        this.data.LaneConsigneeNumber = $("#txtEALaneConsigneeNumber").data("kendoMaskedTextBox").value();
        this.data.LanePortofEntry = $("#txtEALanePortofEntry").data("kendoMaskedTextBox").value();
        if (typeof (diCostType) !== 'undefined' && typeof (diCostType.Name) !== 'undefined') {
            this.data.LaneBFCType = diCostType.Name;
        }
        if (typeof (diModeType) !== 'undefined' && typeof (diModeType.Control) !== 'undefined') {
            this.data.LaneModeTypeControl = diModeType.Control;
        }

        if (typeof (diPalletType) !== 'undefined' && typeof (diPalletType.Name) !== 'undefined') {
            this.data.LanePalletType = diPalletType.Name;
        }



        this.data.LaneUpdated = $("#txtEALaneUpdated").val();

        if (intLaneControl === 0) {
            //Only Validate on Insert
            var oValidationResults = this.validateRequiredFields(this.data, blnIsTransfer);
            if (typeof (oValidationResults) === 'undefined' || !ngl.isObject(oValidationResults)) {
                ngl.showValidationMsg("Cannot validate Lane Information", "Invalid validation procedure, please contact technical support", tObj);
                return;
            }
            else { if (oValidationResults.Success === false) { ngl.showValidationMsg("Required Fields", oValidationResults.Message, tObj); return; } }
        }
        //save the changes
        var tObj = this;
        var windowWidget = $("#wndLaneEA").data("kendoWindow");
        kendo.ui.progress(windowWidget.element, true);
        setTimeout(function (tObj) {
            var oCRUDCtrl = new nglRESTCRUDCtrl();            
            oCRUDCtrl.update("Lane/InsertOrUpdateLane", tObj.data, tObj, "saveSuccessCallback", "saveAjaxErrorCallback");
        }, 100, tObj);
    }

    this.show = function () {
        var tObj = this;

        this.kendoWindowsObj = $("#wndLaneEA").kendoWindow({
            title: "Add New Lane",
            modal: true,
            visible: false,
            height: '90%',
            width: '95%',
            actions: ["save", "Minimize", "Maximize", "Close"],
            close: function (e) { tObj.close(e); }
        }).data("kendoWindow");

        if (this.kendoWindowsObjUploadEventAdded === 0) {
            $("#wndLaneEA").data("kendoWindow").wrapper.find(".k-svg-i-save").parent().click(function (e) { e.preventDefault(); tObj.save(); });
        }
        this.kendoWindowsObjUploadEventAdded = 1;

        //if this is an edit load the data to the window
        if (typeof (this.data) !== 'undefined' && ngl.isObject(this.data)) {
            this.edit(this.data);
        }
        else {
            this.kendoWindowsObj.title("Add New Lane");
            //Show all the red "required field" * on Add
            $(".redRequiredAsterik").show();
            //Get the LegalEntity from the screen on Add New
            $("#txtEALaneLegalEntity").data("kendoMaskedTextBox").value(this.screenLEName);
        }

        if (this.EditError === false) { this.kendoWindowsObj.center().open(); }
    }

    this.edit = function (data) {
        //load data to the screen
        var tObj = this;

        this.blnIsEdit = true;

        this.kendoWindowsObj.title("Edit Lane");
        //Hide all the red "required field" * on Edit
        //$(".redRequiredAsterik").hide();
        //Hide LTTransType ddl and show Inbound chk on Edit
        
        $("#divLTTransType").hide();
        $("#divInbound").show();

        if (typeof (data) !== 'undefined' && data != null && ngl.isObject(data)) {
            //Verify LaneControl is a column in the grid and is not null or 0
            if ('LaneControl' in data) {
                if (typeof (data.LaneControl) === 'undefined' || data.LaneControl == null || data.LaneControl == 0 ) {
                    if (data.LaneName != 'Duplicate') {
                        ngl.showErrMsg("Invalid  Lane Data Required", "Please select a valid lane record and try again", null); this.EditError = true; return;
                    } else {
                        $("#divLTTransType").show();
                        $("#divInbound").hide();
                        if ('LaneOriginAddressUse' in data) {
                            if (data.LaneOriginAddressUse) {
                                $("#ddlEALTTransType").data("kendoDropDownList").select(function (dataItem) { return dataItem.Control === 3; });
                            } else {
                                $("#ddlEALTTransType").data("kendoDropDownList").select(function (dataItem) { return dataItem.Control === 1; });
                            }
                        }
                    }
                }

                if ('LaneOriginAddressUse' in data) {
                    //console.log('LaneOriginAddressUse');
                    //console.log(data.LaneOriginAddressUse);
                    if (data.LaneOriginAddressUse) {
                        $("#chkEALaneOrigAddressUse").prop('checked', true);
                        //Modified by RHR for v-8.2 on 07/24/2018
                        //added logic to toggel the inbound outbound visibility flag
                        this.toggleInboundOutbound(3);
                        this.data.LTTransType = 3;
                        
                    } else {
                        $("#chkEALaneOrigAddressUse").prop('checked', false);
                        //added logic to toggel the inbound outbound visibility flag
                        this.toggleInboundOutbound(1)
                        this.data.LTTransType = 1;
                    }
                }
                //console.log('LTTransType: = ' + this.data.LTTransType)
                //console.log("LaneCompControl: " + data.LaneCompControl);
                $("#ddlEALEComps").data("kendoDropDownList").select(function (dataItem) { return dataItem.CompControl === data.LaneCompControl; });
                $("#ddlEALECompOrig").data("kendoDropDownList").select(function (dataItem) { return dataItem.CompControl === data.LaneOrigCompControl; });
                $("#ddlEALECompDest").data("kendoDropDownList").select(function (dataItem) { return dataItem.CompControl === data.LaneDestCompControl; });

                //$("#ddlEALaneTrans").data("kendoDropDownList").select(function (dataItem) { return dataItem.Description === data.LaneTransType; });
                $('#ddlEALaneTrans').data('kendoDropDownList').search(data.TransType);
                //$("#ddlEATempType").data("kendoDropDownList").select(function (dataItem) { return dataItem.Control === data.LaneTempType; });
                $('#ddlEATempType').data('kendoDropDownList').search(data.TempType);


                $("#txtEALaneControl").val(data.LaneControl); //Save the LaneControl so it can be accessed by the Save function
                if ('LaneUpdated' in data) { $("#txtEALaneUpdated").val(data.LaneUpdated); } //Save the LaneUpdated so it can be accessed by the Save function
                if ('LaneName' in data) { $("#txtEALaneName").data("kendoMaskedTextBox").value(data.LaneName); }
                if ('LaneNumber' in data) { $("#txtEALaneNumber").data("kendoMaskedTextBox").value(data.LaneNumber); }
                if ('LaneLegalEntity' in data) { $("#txtEALaneLegalEntity").data("kendoMaskedTextBox").value(data.LaneLegalEntity); }
                //Added By LVV on 10/12/20 for v-8.3.0.001 Task #20201012142747 - Add Fields to Lane Maint
                if ('LaneNumberMaster' in data) { $("#txtEALaneNumberMaster").data("kendoMaskedTextBox").value(data.LaneNumberMaster); }
                if ('LaneNameMaster' in data) { $("#txtEALaneNameMaster").data("kendoMaskedTextBox").value(data.LaneNameMaster); }
                if ('LaneCarrierEquipmentCodes' in data) { $("#txtEALaneCarrierEquipmentCodes").data("kendoMaskedTextBox").value(data.LaneCarrierEquipmentCodes); }

                //Comments
                if ('LaneComments' in data) { $("#txtEALaneComments").data("kendoMaskedTextBox").value(data.LaneComments); }
                if ('LaneCommentsConfidential' in data) { $("#txtEALaneCommentsConfidential").data("kendoMaskedTextBox").value(data.LaneCommentsConfidential); }
                //Added By LVV on 10/12/20 for v-8.3.0.001 Task #20201012142747 - Add Fields to Lane Maint
                if ('LaneUser1' in data) { $("#txtEALaneUser1").data("kendoMaskedTextBox").value(data.LaneUser1); }
                if ('LaneUser2' in data) { $("#txtEALaneUser2").data("kendoMaskedTextBox").value(data.LaneUser2); }
                if ('LaneUser3' in data) { $("#txtEALaneUser3").data("kendoMaskedTextBox").value(data.LaneUser3); }
                if ('LaneUser4' in data) { $("#txtEALaneUser4").data("kendoMaskedTextBox").value(data.LaneUser4); }

                if ('LaneActive' in data) {
                    if (data.LaneActive) { $("#chkEALaneActive").prop('checked', true); } else { $("#chkEALaneActive").prop('checked', false); }
                }
                //Capacity Settings
                if ('LaneTLWgt' in data) { $("#txtEALaneTLWgt").data("kendoNumericTextBox").value(data.LaneTLWgt); } //Added By LVV on 5/18/20 - default TL capcity settings to 0 on create new lane from 365
                if ('LaneTLCases' in data) { $("#txtEALaneTLCases").data("kendoNumericTextBox").value(data.LaneTLCases); } //Added By LVV on 5/18/20 - default TL capcity settings to 0 on create new lane from 365
                if ('LaneTLCube' in data) { $("#txtEALaneTLCube").data("kendoNumericTextBox").value(data.LaneTLCube); } //Added By LVV on 5/18/20 - default TL capcity settings to 0 on create new lane from 365
                if ('LaneTLPL' in data) { $("#txtEALaneTLPL").data("kendoNumericTextBox").value(data.LaneTLPL); } //Added By LVV on 5/18/20 - default TL capcity settings to 0 on create new lane from 365
                //Orig
                if ('LaneOrigName' in data) { $("#txtEALaneOrigName").data("kendoMaskedTextBox").value(data.LaneOrigName); }
                if ('LaneOrigAddress1' in data) { $("#txtEALaneOrigAddress1").data("kendoMaskedTextBox").value(data.LaneOrigAddress1); }
                if ('LaneOrigAddress2' in data) { $("#txtEALaneOrigAddress2").data("kendoMaskedTextBox").value(data.LaneOrigAddress2); }
                if ('LaneOrigAddress3' in data) { $("#txtEALaneOrigAddress3").data("kendoMaskedTextBox").value(data.LaneOrigAddress3); }
                if ('LaneOrigCity' in data) { $("#txtEALaneOrigCity").data("kendoMaskedTextBox").value(data.LaneOrigCity); }
                if ('LaneOrigState' in data) { $("#txtEALaneOrigState").data("kendoMaskedTextBox").value(data.LaneOrigState); }
                if ('LaneOrigZip' in data) { $("#txtEALaneOrigZip").data("kendoMaskedTextBox").value(data.LaneOrigZip); }
                if ('LaneOrigCountry' in data) { $("#txtEALaneOrigCountry").data("kendoMaskedTextBox").value(data.LaneOrigCountry); }
                if ('LaneAppt' in data) {
                    if (data.LaneAppt) { $("#chkEALaneAppt").prop('checked', true); } else { $("#chkEALaneAppt").prop('checked', false); }
                }
                //Modified by RHR for v-8.5.4.003 on 10/28/2023 changed from datetimepicker to kendoMaskedTextBox
                //console.log("Rec Hours Start");
                //console.log(data.LaneRecHourStart);
                if ('LaneRecHourStart' in data) { $("#tpEALaneRecHourStart").data("kendoMaskedTextBox").value(data.LaneRecHourStart); }
                if ('LaneRecHourStop' in data) { $("#tpEALaneRecHourStop").data("kendoMaskedTextBox").value(data.LaneRecHourStop); }
                //Orig Contacts
                if ('LaneOrigContactName' in data) { $("#txtEALaneOrigContactName").data("kendoMaskedTextBox").value(data.LaneOrigContactName); }
                if ('LaneOrigContactPhone' in data) { $("#txtEALaneOrigContactPhone").data("kendoMaskedTextBox").value(data.LaneOrigContactPhone); }
                if ('LaneOrigContactEmail' in data) { $("#txtEALaneOrigContactEmail").data("kendoMaskedTextBox").value(data.LaneOrigContactEmail); }
                if ('LaneOrigEmergencyContactName' in data) { $("#txtEALaneOrigEmergencyContactName").data("kendoMaskedTextBox").value(data.LaneOrigEmergencyContactName); }
                if ('LaneOrigEmergencyContactPhone' in data) { $("#txtEALaneOrigEmergencyContactPhone").data("kendoMaskedTextBox").value(data.LaneOrigEmergencyContactPhone); }
                //Dest
                if ('LaneDestName' in data) { $("#txtEALaneDestName").data("kendoMaskedTextBox").value(data.LaneDestName); }
                if ('LaneDestAddress1' in data) { $("#txtEALaneDestAddress1").data("kendoMaskedTextBox").value(data.LaneDestAddress1); }
                if ('LaneDestAddress2' in data) { $("#txtEALaneDestAddress2").data("kendoMaskedTextBox").value(data.LaneDestAddress2); }
                if ('LaneDestAddress3' in data) { $("#txtEALaneDestAddress3").data("kendoMaskedTextBox").value(data.LaneDestAddress3); }
                if ('LaneDestCity' in data) { $("#txtEALaneDestCity").data("kendoMaskedTextBox").value(data.LaneDestCity); }
                if ('LaneDestState' in data) { $("#txtEALaneDestState").data("kendoMaskedTextBox").value(data.LaneDestState); }
                if ('LaneDestZip' in data) { $("#txtEALaneDestZip").data("kendoMaskedTextBox").value(data.LaneDestZip); }
                if ('LaneDestCountry' in data) { $("#txtEALaneDestCountry").data("kendoMaskedTextBox").value(data.LaneDestCountry); }
                if ('LaneAptDelivery' in data) {
                    if (data.LaneAptDelivery) { $("#chkEALaneAptDelivery").prop('checked', true); } else { $("#chkEALaneAptDelivery").prop('checked', false); }
                }
                //Modified by RHR for v-8.5.4.003 on 10/28/2023 changed from datetimepicker to kendoMaskedTextBox
                if ('LaneDestHourStart' in data) { $("#tpEALaneDestHourStart").data("kendoMaskedTextBox").value(data.LaneDestHourStart); }
                if ('LaneDestHourStop' in data) { $("#tpEALaneDestHourStop").data("kendoMaskedTextBox").value(data.LaneDestHourStop); }
                //Dest Contacts
                if ('LaneDestContactName' in data) { $("#txtEALaneDestContactName").data("kendoMaskedTextBox").value(data.LaneDestContactName); }
                if ('LaneDestContactPhone' in data) { $("#txtEALaneDestContactPhone").data("kendoMaskedTextBox").value(data.LaneDestContactPhone); }
                if ('LaneDestContactEmail' in data) { $("#txtEALaneDestContactEmail").data("kendoMaskedTextBox").value(data.LaneDestContactEmail); }
                if ('LaneDestEmergencyContactName' in data) { $("#txtEALaneDestEmergencyContactName").data("kendoMaskedTextBox").value(data.LaneDestEmergencyContactName); }
                if ('LaneDestEmergencyContactPhone' in data) { $("#txtEALaneDestEmergencyContactPhone").data("kendoMaskedTextBox").value(data.LaneDestEmergencyContactPhone); }
                //Added By LVV on 10/12/20 for v-8.3.0.001 Task #20201012142747 - Add Fields to Lane Maint
                //Dynamic Routing                          
                if ('LaneBenchMiles' in data) { $("#txtEALaneBenchMiles").data("kendoNumericTextBox").value(data.LaneBenchMiles); }
                if ('LaneLatitude' in data) { $("#txtEALaneLatitude").data("kendoNumericTextBox").value(data.LaneLatitude); }
                if ('LaneLongitude' in data) { $("#txtEALaneLongitude").data("kendoNumericTextBox").value(data.LaneLongitude); }
                if ('LaneIsTransLoad' in data) {
                    if (data.LaneIsTransLoad) { $("#chkEALaneIsTransLoad").prop('checked', true); } else { $("#chkEALaneIsTransLoad").prop('checked', false); }
                }
                if ('LaneIsCrossDockFacility' in data) {
                    if (data.LaneIsCrossDockFacility) { $("#chkEALaneIsCrossDockFacility").prop('checked', true); } else { $("#chkEALaneIsCrossDockFacility").prop('checked', false); }
                }
                //Static Routing                
                $("#ddlEALaneRouteGuide").data("kendoDropDownList").select(function (dataItem) { return dataItem.Control === data.LaneRouteGuideControl; });
                if ('LaneDefaultRouteSequence' in data) { $("#txtEALaneDefaultRouteSequence").data("kendoNumericTextBox").value(data.LaneDefaultRouteSequence); }
                $("#ddlEALaneRouteTypeCode").data("kendoDropDownList").select(function (dataItem) { return dataItem.Control === data.LaneRouteTypeCode; });
                //Options
                if ('LaneDefaultCarrierUse' in data) {
                    if (data.LaneDefaultCarrierUse) { $("#chkEALaneDefaultCarrierUse").prop('checked', true); } else { $("#chkEALaneDefaultCarrierUse").prop('checked', false); }
                }
                if ('LaneAutoTenderFlag' in data) {
                    if (data.LaneAutoTenderFlag) { $("#chkEALaneAutoTenderFlag").prop('checked', true); } else { $("#chkEALaneAutoTenderFlag").prop('checked', false); }
                }
                if ('LaneDoNotInvoice' in data) {
                    if (data.LaneDoNotInvoice) { $("#chkEALaneDoNotInvoice").prop('checked', true); } else { $("#chkEALaneDoNotInvoice").prop('checked', false); }
                }
                if ('LaneAllowInterline' in data) {
                    if (data.LaneAllowInterline) { $("#chkEALaneAllowInterline").prop('checked', true); } else { $("#chkEALaneAllowInterline").prop('checked', false); }
                }
                if ('LaneCascadingDispatchingFlag' in data) {
                    if (data.LaneCascadingDispatchingFlag) { $("#chkEALaneCascadingDispatchingFlag").prop('checked', true); } else { $("#chkEALaneCascadingDispatchingFlag").prop('checked', false); }
                }
                if ('LaneRestrictCarrierSelection' in data) {
                    if (data.LaneRestrictCarrierSelection) { $("#chkEALaneRestrictCarrierSelection").prop('checked', true); } else { $("#chkEALaneRestrictCarrierSelection").prop('checked', false); }
                }
                if ('LaneWarnOnRestrictedCarrierSelection' in data) {
                    if (data.LaneWarnOnRestrictedCarrierSelection) { $("#chkEALaneWarnOnRestrictedCarrierSelection").prop('checked', true); } else { $("#chkEALaneWarnOnRestrictedCarrierSelection").prop('checked', false); }
                }
                if ('LanePalletExchange' in data) {
                    if (data.LanePalletExchange) { $("#chkEALanePalletExchange").prop('checked', true); } else { $("#chkEALanePalletExchange").prop('checked', false); }
                }
                // Modified by RHR for v-8.4.0.003 on 07/15/2021 -- added Book Appt via Email Options
                if ('LaneAllowCarrierBookApptByEmail' in data) {
                    if (data.LaneAllowCarrierBookApptByEmail) { $("#chkEALaneAllowCarrierBookApptByEmail").prop('checked', true); } else { $("#chkEALaneAllowCarrierBookApptByEmail").prop('checked', false); }
                }

                if ('LaneRequireCarrierAuthBookApptByEmail' in data) {
                    if (data.LaneRequireCarrierAuthBookApptByEmail) { $("#chkEALaneRequireCarrierAuthBookApptByEmail").prop('checked', true); } else { $("#chkEALaneRequireCarrierAuthBookApptByEmail").prop('checked', false); }
                }

                if ('LaneUseCarrieContEmailForBookApptByEmail' in data) {
                    if (data.LaneUseCarrieContEmailForBookApptByEmail) { $("#chkEALaneUseCarrieContEmailForBookApptByEmail").prop('checked', true); } else { $("#chkEALaneUseCarrieContEmailForBookApptByEmail").prop('checked', false); }
                }

                if ('LaneCarrierBookApptviaTokenEmail' in data) { $("#txtEALaneCarrierBookApptviaTokenEmail").data("kendoMaskedTextBox").value(data.LaneCarrierBookApptviaTokenEmail); }


                if ('LaneCarrierBookApptviaTokenFailEmail' in data) { $("#txtEALaneCarrierBookApptviaTokenFailEmail").data("kendoMaskedTextBox").value(data.LaneCarrierBookApptviaTokenFailEmail); }


                if ('LaneCarrierBookApptviaTokenFailPhone' in data) { $("#txtEALaneCarrierBookApptviaTokenFailPhone").data("kendoMaskedTextBox").value(data.LaneCarrierBookApptviaTokenFailPhone); }


                $("#ddlEALaneTransLeadTimeLocationOption").data("kendoDropDownList").select(function (dataItem) { return dataItem.Control === data.LaneTransLeadTimeLocationOption; });
                if ('LaneTransLeadTimeUseMasterLane' in data) {
                    if (data.LaneTransLeadTimeUseMasterLane) { $("#chkEALaneTransLeadTimeUseMasterLane").prop('checked', true); } else { $("#chkEALaneTransLeadTimeUseMasterLane").prop('checked', false); }
                }
                $("#ddlEALaneTransLeadTimeCalcType").data("kendoDropDownList").select(function (dataItem) { return dataItem.Control === data.LaneTransLeadTimeCalcType; });

                //Default Provider
                if ('txtLaneDefaultCarrier' in data) { $("#txtEALaneDefaultCarrierText").data("kendoMaskedTextBox").value(data.txtLaneDefaultCarrier); }
                if ('LaneDefaultCarrierContact' in data) { $("#txtEALaneDefaultCarrierContact").data("kendoMaskedTextBox").value(data.LaneDefaultCarrierContact); }
                if ('LaneDefaultCarrierPhone' in data) { $("#txtEALaneDefaultCarrierPhone").data("kendoMaskedTextBox").value(data.LaneDefaultCarrierPhone); }
                if ('LaneStops' in data) { $("#txtEALaneStops").data("kendoNumericTextBox").value(data.LaneStops); }
                //Config
                if ('LaneOLTBenchmark' in data) { $("#txtEALaneOLTBenchmark").data("kendoNumericTextBox").value(data.LaneOLTBenchmark); }
                if ('LaneTLTBenchmark' in data) { $("#txtEALaneTLTBenchmark").data("kendoNumericTextBox").value(data.LaneTLTBenchmark); }
                if ('LaneRecMinIn' in data) { $("#txtEALaneRecMinIn").data("kendoNumericTextBox").value(data.LaneRecMinIn); }
                if ('LaneRecMinUnload' in data) { $("#txtEALaneRecMinUnload").data("kendoNumericTextBox").value(data.LaneRecMinUnload); }
                if ('LaneRecMinOut' in data) { $("#txtEALaneRecMinOut").data("kendoNumericTextBox").value(data.LaneRecMinOut); }
                if ('LaneBFC' in data) { $("#txtEALaneBFC").data("kendoNumericTextBox").value(data.LaneBFC); }
                if ('LanePrimaryBuyer' in data) { $("#txtEALanePrimaryBuyer").data("kendoMaskedTextBox").value(data.LanePrimaryBuyer); }
                if ('LaneConsigneeNumber' in data) { $("#txtEALaneConsigneeNumber").data("kendoMaskedTextBox").value(data.LaneConsigneeNumber); }
                if ('LanePortofEntry' in data) { $("#txtEALanePortofEntry").data("kendoMaskedTextBox").value(data.LanePortofEntry); }
                $("#ddlEALanePalletType").data("kendoDropDownList").select(function (dataItem) { return dataItem.Name === data.LanePalletType; });
                $("#ddlEALaneModeType").data("kendoDropDownList").select(function (dataItem) { return dataItem.Control === data.LaneModeTypeControl; });
                $("#ddlEALaneBFCType").data("kendoDropDownList").select(function (dataItem) { return dataItem.Name === data.LaneBFCType; });
            }
            else { ngl.showErrMsg("LaneControl Required", "Row object does not contain property LaneControl.", null); this.EditError = true; return; }

            //removed by RHR we always let users edit the lane number.
            //Show CompNumber on Edit but make it readonly
            //$("#divLaneNumber").show();
            //$("#txtEALaneNumber").data("kendoMaskedTextBox").readonly(false);
            //$("#divLaneNumberAddMsg").hide();
            

        }
        else { ngl.showErrMsg("Error", "There was a problem getting the row from the grid", null); this.EditError = true; return; }
    }

    this.delete = function (data) {
        var tObj = this;
        var laneName = "";

        if (!ngl.isNullOrUndefined(data) && ngl.isObject(data)) {
            //Verify LaneControl is a column in the grid and is not null or 0
            if ('LaneControl' in data) {
                if (ngl.isNullOrUndefined(data.LaneControl) || data.LaneControl == 0) { ngl.showErrMsg("LaneControl Required", "LaneControl cannot be 0", null); return; }
                this.deleteLaneControl = data.LaneControl;
                if ('LaneName' in data) { laneName = data.LaneName; }
            }
            else { ngl.showErrMsg("LaneControl Required", "Row object does not contain property LaneControl.", null); return; }
        }
        else { ngl.showErrMsg("Error", "There was a problem getting the row from the grid", null); return; }

        var msgPrompt = "Are you sure that you want to delete this Lane?";
        //Commented this out until the bug can be fixed
        //if (!ngl.isNullOrUndefined(laneName) && laneName.length > 0) { msgPrompt = "Are you sure that you want to delete Lane " + laneName + "?"; }

        ngl.OkCancelConfirmation(
            "Delete Confirmation",
            msgPrompt,
            400,
            400,
            tObj,
            "confirmDeleteLane");

    }

    this.close = function (e) {
        //** Always Set up the window for "Add New" on close **
        //** This way we only have to do special set up in the "Edit" method
        //reset values
        $("#txtEALaneControl").val(0);
        $("#txtEALaneUpdated").val("");

        this.clearOrigAddressFields();
        this.clearDestAddressFields();

        $("#txtEALaneName").data("kendoMaskedTextBox").value("");
        $("#txtEALaneNumber").data("kendoMaskedTextBox").value("");
        $("#txtEALaneLegalEntity").data("kendoMaskedTextBox").value("");
        $("#txtEALaneNumberMaster").data("kendoMaskedTextBox").value(""); //Added By LVV on 10/12/20 for v-8.3.0.001 Task #20201012142747 - Add Fields to Lane Maint
        $("#txtEALaneNameMaster").data("kendoMaskedTextBox").value(""); //Added By LVV on 10/12/20 for v-8.3.0.001 Task #20201012142747 - Add Fields to Lane Maint
        $("#txtEALaneCarrierEquipmentCodes").data("kendoMaskedTextBox").value(""); //Added By LVV on 10/12/20 for v-8.3.0.001 Task #20201012142747 - Add Fields to Lane Maint
        //Comments
        $("#txtEALaneComments").data("kendoMaskedTextBox").value("");
        $("#txtEALaneCommentsConfidential").data("kendoMaskedTextBox").value("");
        $("#txtEALaneUser1").data("kendoMaskedTextBox").value(""); //Added By LVV on 10/12/20 for v-8.3.0.001 Task #20201012142747 - Add Fields to Lane Maint
        $("#txtEALaneUser2").data("kendoMaskedTextBox").value(""); //Added By LVV on 10/12/20 for v-8.3.0.001 Task #20201012142747 - Add Fields to Lane Maint
        $("#txtEALaneUser3").data("kendoMaskedTextBox").value(""); //Added By LVV on 10/12/20 for v-8.3.0.001 Task #20201012142747 - Add Fields to Lane Maint
        $("#txtEALaneUser4").data("kendoMaskedTextBox").value(""); //Added By LVV on 10/12/20 for v-8.3.0.001 Task #20201012142747 - Add Fields to Lane Maint
        //Hours
        //Modified by RHR for v-8.5.4.003 on 10/28/2023 changed from datetimepicker to kendoMaskedTextBox
        $("#tpEALaneRecHourStart").data("kendoMaskedTextBox").value("");
        $("#tpEALaneDestHourStop").data("kendoMaskedTextBox").value("");
        $("#tpEALaneDestHourStart").data("kendoMaskedTextBox").value("");
        $("#tpEALaneDestHourStop").data("kendoMaskedTextBox").value("");
        //Capacity Settings
        $("#txtEALaneTLWgt").data("kendoNumericTextBox").value(0); //Added By LVV on 5/18/20 - default TL capcity settings to 0 on create new lane from 365
        $("#txtEALaneTLCases").data("kendoNumericTextBox").value(0); //Added By LVV on 5/18/20 - default TL capcity settings to 0 on create new lane from 365
        $("#txtEALaneTLCube").data("kendoNumericTextBox").value(0); //Added By LVV on 5/18/20 - default TL capcity settings to 0 on create new lane from 365
        $("#txtEALaneTLPL").data("kendoNumericTextBox").value(0); //Added By LVV on 5/18/20 - default TL capcity settings to 0 on create new lane from 365

        //Select "Outbound" by default
        //Modified By RHR for v-8.2 on 07/24/2018 - new logic to set Outbound as the default
        $("#ddlEALTTransType").data("kendoDropDownList").select(function (dataItem) { return dataItem.Control === 1; });
        this.toggleInboundOutbound(1);

        //Rest both CompLists to Select "None"
        $("#ddlEALECompOrig").data("kendoDropDownList").select(0);
        $("#ddlEALECompDest").data("kendoDropDownList").select(0);
        $("#ddlEALEComps").data("kendoDropDownList").select(0);

        //Reset Transport Type to "N/A" and Temp Req to "None"
        $("#ddlEALaneTrans").data("kendoDropDownList").select(0);
        $("#ddlEATempType").data("kendoDropDownList").select(0);

        //Check "LaneActive"
        $("#chkEALaneActive").prop('checked', true);
        //Check "Outbound"       
        $("#chkEALaneOrigAddressUse").prop('checked', false); //Modified by RHR for v-8.2 on 07/24/2018 we now set the default to Outbound (checked = false)
        //Uncheck "Appt Req"
        $("#chkEALaneAppt").prop('checked', false);
        $("#chkEALaneAptDelivery").prop('checked', false);

        //removed by RHR we now let users edit the lane number.
        ////Hide LaneNumber on Add since this is auto-generated
        //$("#divLaneNumber").hide();

        //Shoq LTTransType ddl and hide Inbound chk on Edit
        $("#divLTTransType").show();
        $("#divInbound").hide();

        //Set LegalEntity to readonly because this comes from the page
        $("#txtEALaneLegalEntity").data("kendoMaskedTextBox").readonly(true);
        //reset the LE so we get it fresh from the screen everytime
        this.screenLEName = "";
        screenLEControl = 0;
        this.deleteLaneControl = 0;
        this.blnIsEdit = false;

        //NOTE: The title of the window gets set to "Add" every time the show method is called so no need to reset here

        //Added By LVV on 10/12/20 for v-8.3.0.001 Task #20201012142747 - Add Fields to Lane Maint           
        $("#chkEALaneIsTransLoad").prop('checked', false); //Uncheck "Transload Active"           
        $("#chkEALaneIsCrossDockFacility").prop('checked', false); //Uncheck "Cross Dock Facility"
        $("#txtEALaneBenchMiles").data("kendoNumericTextBox").value(0);
        $("#txtEALaneLatitude").data("kendoNumericTextBox").value(0);
        $("#txtEALaneLongitude").data("kendoNumericTextBox").value(0);
        $("#ddlEALaneRouteGuide").data("kendoDropDownList").select(0); //Reset Route Guide to "None"
        $("#txtEALaneDefaultRouteSequence").data("kendoNumericTextBox").value(0);
        $("#ddlEALaneRouteTypeCode").data("kendoDropDownList").select(0);
        $("#chkEALaneDefaultCarrierUse").prop('checked', false); //Uncheck "Auto Assign Carrier" 
        $("#chkEALaneAutoTenderFlag").prop('checked', false); //Uncheck "Auto Tender" 
        $("#chkEALaneDoNotInvoice").prop('checked', false); //Uncheck "Invoice" 
        $("#chkEALaneAllowInterline").prop('checked', false); //Uncheck "Allow Interline" 
        $("#chkEALaneCascadingDispatchingFlag").prop('checked', false); //Uncheck "Cascading Dispatching" 
        $("#chkEALaneRestrictCarrierSelection").prop('checked', false); //Uncheck "Using Company Carrier Restriction Settings"
        $("#chkEALaneWarnOnRestrictedCarrierSelection").prop('checked', false); //Uncheck "Using Company Warning Settings"
        $("#txtEALaneDefaultCarrierText").data("kendoMaskedTextBox").value("");
        $("#txtEALaneDefaultCarrierContact").data("kendoMaskedTextBox").value("");
        $("#txtEALaneDefaultCarrierPhone").data("kendoMaskedTextBox").value("");
        $("#txtEALaneStops").data("kendoNumericTextBox").value(0);


        $("#chkEALanePalletExchange").prop('checked', false); //Uncheck "Pallet Xchg"
        $("#txtEALaneOLTBenchmark").data("kendoNumericTextBox").value(0);
        $("#txtEALaneTLTBenchmark").data("kendoNumericTextBox").value(0);
        $("#txtEALaneRecMinIn").data("kendoNumericTextBox").value(0);
        $("#txtEALaneRecMinUnload").data("kendoNumericTextBox").value(0);
        $("#txtEALaneRecMinOut").data("kendoNumericTextBox").value(0);
        $("#txtEALaneBFC").data("kendoNumericTextBox").value(0);
        $("#txtEALanePrimaryBuyer").data("kendoMaskedTextBox").value("");
        $("#txtEALaneConsigneeNumber").data("kendoMaskedTextBox").value("");
        $("#txtEALanePortofEntry").data("kendoMaskedTextBox").value("");
        $("#ddlEALaneBFCType").data("kendoDropDownList").select(0);
        $("#ddlEALanePalletType").data("kendoDropDownList").select(0);
        $("#ddlEALaneModeType").data("kendoDropDownList").select(2);

        // Modified by RHR for v-8.4.0.003 on 07/15/2021 -- added Book Appt via Email Options
        $("#chkEALaneAllowCarrierBookApptByEmail").prop('checked', false);
        $("#chkEALaneRequireCarrierAuthBookApptByEmail").prop('checked', false);
        $("#chkEALaneUseCarrieContEmailForBookApptByEmail").prop('checked', false);

        $("#txtEALaneCarrierBookApptviaTokenEmail").data("kendoMaskedTextBox").value("");
        $("#txtEALaneCarrierBookApptviaTokenFailEmail").data("kendoMaskedTextBox").value("");
        $("#txtEALaneCarrierBookApptviaTokenFailPhone").data("kendoMaskedTextBox").value("");

        $("#ddlEALaneTransLeadTimeLocationOption").data("kendoDropDownList").select(0); //LaneTransLeadTimeLocationOption
        $("#chkEALaneTransLeadTimeUseMasterLane").prop('checked', false);
        $("#ddlEALaneTransLeadTimeCalcType").data("kendoDropDownList").select(0);

        //Expand the Fast Tabs
        expandEALane();
        expandEALaneOrigCont();
        expandEALaneDestCont();

        //Added By LVV on 10/12/20 for v-8.3.0.001 Task #20201012142747 - Add Fields to Lane Maint
        expandAddressBlock();
        expandOptions();
        collapseCommentsBlock();
        collapseDefaultProvider();
        collapseCapacitySettings();
        collapseDynamicRouting();
        collapseStaticRouting();
        collapseConfig();
    }

    //loadDefaults sets up the callbacks cbSelect and cbSave
    //all call backs return a reference to this object and a string message as parameters
    this.loadDefaults = function (pageVariable, saveCallBack, deleteCallBack) {

        this.onSave = saveCallBack;
        this.onDelete = deleteCallBack;
        this.data = new Dispatch();
        this.lastErr = "";
        this.notifyOnError = true;
        this.notifyOnSuccess = true;
        this.notifyOnValidationFailure = true;
        var tObj = this;

        if (typeof (pageVariable) !== 'undefined' && ngl.isObject(pageVariable)) {

            this.kendoWindowsObj = pageVariable;

            //Lane Fields
            $("#txtEALaneControl").val(0);
            $("#txtEALaneName").kendoMaskedTextBox();
            $("#txtEALaneNumber").kendoMaskedTextBox();
            $("#txtEALaneLegalEntity").kendoMaskedTextBox();
            $("#txtEALaneNumberMaster").kendoMaskedTextBox(); //Added By LVV on 10/12/20 for v-8.3.0.001 Task #20201012142747 - Add Fields to Lane Maint
            $("#txtEALaneNameMaster").kendoMaskedTextBox(); //Added By LVV on 10/12/20 for v-8.3.0.001 Task #20201012142747 - Add Fields to Lane Maint
            $("#txtEALaneCarrierEquipmentCodes").kendoMaskedTextBox(); //Added By LVV on 10/12/20 for v-8.3.0.001 Task #20201012142747 - Add Fields to Lane Maint
            //Comments
            $("#txtEALaneComments").kendoMaskedTextBox();
            $("#txtEALaneCommentsConfidential").kendoMaskedTextBox();
            $("#txtEALaneUser1").kendoMaskedTextBox(); //Added By LVV on 10/12/20 for v-8.3.0.001 Task #20201012142747 - Add Fields to Lane Maint
            $("#txtEALaneUser2").kendoMaskedTextBox(); //Added By LVV on 10/12/20 for v-8.3.0.001 Task #20201012142747 - Add Fields to Lane Maint
            $("#txtEALaneUser3").kendoMaskedTextBox(); //Added By LVV on 10/12/20 for v-8.3.0.001 Task #20201012142747 - Add Fields to Lane Maint
            $("#txtEALaneUser4").kendoMaskedTextBox(); //Added By LVV on 10/12/20 for v-8.3.0.001 Task #20201012142747 - Add Fields to Lane Maint
            //Capacity Settings 
            $("#txtEALaneTLWgt").kendoNumericTextBox(); //Added By LVV on 5/18/20 - default TL capcity settings to 0 on create new lane from 365
            $("#txtEALaneTLCases").kendoNumericTextBox({ format: "{0:0}" }); //Added By LVV on 5/18/20 - default TL capcity settings to 0 on create new lane from 365
            $("#txtEALaneTLCube").kendoNumericTextBox({ format: "{0:0}" }); //Added By LVV on 5/18/20 - default TL capcity settings to 0 on create new lane from 365
            $("#txtEALaneTLPL").kendoNumericTextBox(); //Added By LVV on 5/18/20 - default TL capcity settings to 0 on create new lane from 365
            //Origin
            $("#txtEALaneOrigName").kendoMaskedTextBox();
            $("#txtEALaneOrigAddress1").kendoMaskedTextBox();
            $("#txtEALaneOrigAddress2").kendoMaskedTextBox();
            $("#txtEALaneOrigAddress3").kendoMaskedTextBox();
            $("#txtEALaneOrigCity").kendoMaskedTextBox();
            $("#txtEALaneOrigState").kendoMaskedTextBox();
            $("#txtEALaneOrigZip").kendoMaskedTextBox();
            $("#txtEALaneOrigCountry").kendoMaskedTextBox();
            $("#txtEALaneOrigContactName").kendoMaskedTextBox();
            $("#txtEALaneOrigContactPhone").kendoMaskedTextBox({ mask: ngl.getPhoneNumberMask() });
            $("#txtEALaneOrigContactEmail").kendoMaskedTextBox();
            $("#txtEALaneOrigEmergencyContactPhone").kendoMaskedTextBox({ mask: ngl.getPhoneNumberMask() });
            $("#txtEALaneOrigEmergencyContactName").kendoMaskedTextBox();
            //$("#tpEALaneRecHourStart").kendoTimePicker({ dateInput: true, format: "HH:mm ", interval: 15 });
            //$("#tpEALaneRecHourStop").kendoTimePicker({ dateInput: true, format: "HH:mm ", interval: 15 });
            //Modified by RHR for v-8.5.4.003 on 10/28/2023 changed from datetimepicker to kendoMaskedTextBox
            $("#tpEALaneRecHourStart").kendoMaskedTextBox();
            $("#tpEALaneRecHourStop").kendoMaskedTextBox();

            //Destination
            $("#txtEALaneDestName").kendoMaskedTextBox();
            $("#txtEALaneDestAddress1").kendoMaskedTextBox();
            $("#txtEALaneDestAddress2").kendoMaskedTextBox();
            $("#txtEALaneDestAddress3").kendoMaskedTextBox();
            $("#txtEALaneDestCity").kendoMaskedTextBox();
            $("#txtEALaneDestState").kendoMaskedTextBox();
            $("#txtEALaneDestZip").kendoMaskedTextBox();
            $("#txtEALaneDestCountry").kendoMaskedTextBox();
            $("#txtEALaneDestContactName").kendoMaskedTextBox();
            $("#txtEALaneDestContactPhone").kendoMaskedTextBox({ mask: ngl.getPhoneNumberMask() });
            $("#txtEALaneDestContactEmail").kendoMaskedTextBox();
            $("#txtEALaneDestEmergencyContactPhone").kendoMaskedTextBox({ mask: ngl.getPhoneNumberMask() });
            $("#txtEALaneDestEmergencyContactName").kendoMaskedTextBox();
            //$("#tpEALaneDestHourStart").kendoTimePicker({ dateInput: true, format: "HH:mm ", interval: 15 });
            //$("#tpEALaneDestHourStop").kendoTimePicker({ dateInput: true, format: "HH:mm ", interval: 15 });
            //Modified by RHR for v-8.5.4.003 on 10/28/2023 changed from datetimepicker to kendoMaskedTextBox
            $("#tpEALaneDestHourStart").kendoMaskedTextBox();
            $("#tpEALaneDestHourStop").kendoMaskedTextBox();

            //Added By LVV on 10/12/20 for v-8.3.0.001 Task #20201012142747 - Add Fields to Lane Maint
            //Dynamic Routing
            $("#txtEALaneBenchMiles").kendoNumericTextBox({ format: "{0:n6}", decimals: 6 });
            $("#txtEALaneLatitude").kendoNumericTextBox({ format: "{0:n15}", decimals: 15 });
            $("#txtEALaneLongitude").kendoNumericTextBox({ format: "{0:n15}", decimals: 15 });
            $("#lblEALaneIsCrossDockFacility").kendoTooltip({ content: "If checked, this lane is set up as a cross dock shipping segment." }); //CrossDockFacilityDescrip
            $("#lblEALaneIsTransLoad").kendoTooltip({ content: "If checked, this lane will follow the transload configurations set in the Transload Tab." }); //TransloadActiveDescrip
            //$('#chkEALaneIsCrossDockFacility').kendoTooltip({ content: function (e) { return 'Edit'; } }).data('kendoTooltip'); //TODO - use ajax to get localized value          
            //Static Routing
            $("#ddlEALaneRouteGuide").kendoDropDownList({
                dataTextField: "Name",
                dataValueField: "Control",
                autoWidth: true,
                //optionLabel: { Description: "Any", Control: "" },
                dataSource: {
                    transport: {
                        read: function (options) {
                            $.ajax({
                                async: false,
                                type: "GET",
                                url: "api/vLookupList/GetStaticListVcTnd/" + nglStaticLists.tblStaticRoutes,
                                contentType: "application/json; charset=utf-8",
                                dataType: 'json',
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                success: function (data) { options.success(data); },
                                error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Get Static Routes Failure"); ngl.showErrMsg("Read Static Routes Error", sMsg, null); }
                            });
                        }
                    },
                    schema: {
                        data: "Data",
                        total: "Count",
                        model: {
                            id: "Control",
                            fields: {
                                Name: { editable: false },
                                Description: { editable: false }
                            }
                        },
                        errors: "Errors"
                    },
                    error: function (e) { ngl.showErrMsg("Read Static Routes Error", e.errors, null); this.cancelChanges(); },
                    serverPaging: false,
                    serverSorting: false,
                    serverFiltering: false
                }
            });
            $("#txtEALaneDefaultRouteSequence").kendoNumericTextBox();
            $("#ddlEALaneRouteTypeCode").kendoDropDownList({
                dataTextField: "Name",
                dataValueField: "Control",
                autoWidth: true,
                //optionLabel: { Description: "Any", Control: "" },
                dataSource: {
                    transport: {
                        read: function (options) {
                            $.ajax({
                                async: false,
                                type: "GET",
                                url: "api/vLookupList/GetStaticList/" + nglStaticLists.tblRouteTypes,
                                contentType: "application/json; charset=utf-8",
                                dataType: 'json',
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                success: function (data) { options.success(data); },
                                error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Get Static Route Types Failure"); ngl.showErrMsg("Read Static Route Types Error", sMsg, null); }
                            });
                        }
                    },
                    schema: {
                        data: "Data",
                        total: "Count",
                        model: {
                            id: "Control",
                            fields: {
                                Name: { editable: false },
                                Description: { editable: false }
                            }
                        },
                        errors: "Errors"
                    },
                    error: function (e) { ngl.showErrMsg("Read Static Route Types Error", e.errors, null); this.cancelChanges(); },
                    serverPaging: false,
                    serverSorting: false,
                    serverFiltering: false
                }
            });
            //Default Provider
            $("#txtEALaneDefaultCarrierText").kendoMaskedTextBox();
            $("#txtEALaneDefaultCarrierContact").kendoMaskedTextBox();
            $("#txtEALaneDefaultCarrierPhone").kendoMaskedTextBox({ mask: ngl.getPhoneNumberMask() });
            $("#txtEALaneStops").kendoNumericTextBox();
            // Modified by RHR for v-8.4.0.003 on 07/15/2021 -- added Book Appt via Email Options
            $("#txtEALaneCarrierBookApptviaTokenEmail").kendoMaskedTextBox();
            $("#txtEALaneCarrierBookApptviaTokenFailEmail").kendoMaskedTextBox();
            $("#txtEALaneCarrierBookApptviaTokenFailPhone").kendoMaskedTextBox({ mask: ngl.getPhoneNumberMask() });

            $("#ddlEALaneTransLeadTimeLocationOption").kendoDropDownList({
                dataTextField: "Name",
                dataValueField: "Control",
                autoWidth: true,
                //optionLabel: { Description: "Any", Control: "" },
                dataSource: {
                    transport: {
                        read: function (options) {
                            $.ajax({
                                async: false,
                                type: "GET",
                                url: "api/vLookupList/GetStaticList/" + nglStaticLists.TransLeadTimeLocationOptions,
                                contentType: "application/json; charset=utf-8",
                                dataType: 'json',
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                success: function (data) { options.success(data); },
                                error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Get Trans Lead Time Location Options Failure"); ngl.showErrMsg("Read Trans Lead Time Location Options Error", sMsg, null); }
                            });
                        }
                    },
                    schema: {
                        data: "Data",
                        total: "Count",
                        model: {
                            id: "Control",
                            fields: {
                                Name: { editable: false },
                                Description: { editable: false }
                            }
                        },
                        errors: "Errors"
                    },
                    error: function (e) { ngl.showErrMsg("Read Trans Lead Time Location Option Error", e.errors, null); this.cancelChanges(); },
                    serverPaging: false,
                    serverSorting: false,
                    serverFiltering: false
                }
            });

            $("#ddlEALaneTransLeadTimeCalcType").kendoDropDownList({
                dataTextField: "Name",
                dataValueField: "Control",
                autoWidth: true,
                //optionLabel: { Description: "Any", Control: "" },
                dataSource: {
                    transport: {
                        read: function (options) {
                            $.ajax({
                                async: false,
                                type: "GET",
                                url: "api/vLookupList/GetStaticList/" + nglStaticLists.TransLeadTimeCalcTypes,
                                contentType: "application/json; charset=utf-8",
                                dataType: 'json',
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                success: function (data) { options.success(data); },
                                error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Get Trans Lead Time Calc Type Failure"); ngl.showErrMsg("Read Trans Lead Time Calc Type Error", sMsg, null); }
                            });
                        }
                    },
                    schema: {
                        data: "Data",
                        total: "Count",
                        model: {
                            id: "Control",
                            fields: {
                                Name: { editable: false },
                                Description: { editable: false }
                            }
                        },
                        errors: "Errors"
                    },
                    error: function (e) { ngl.showErrMsg("Read Trans Lead Time Calc Type Error", e.errors, null); this.cancelChanges(); },
                    serverPaging: false,
                    serverSorting: false,
                    serverFiltering: false
                }
            });

            //Config
            $("#txtEALaneOLTBenchmark").kendoNumericTextBox();
            $("#txtEALaneTLTBenchmark").kendoNumericTextBox();
            $("#txtEALaneRecMinIn").kendoNumericTextBox();
            $("#txtEALaneRecMinUnload").kendoNumericTextBox();
            $("#txtEALaneRecMinOut").kendoNumericTextBox();
            $("#txtEALaneBFC").kendoNumericTextBox();

            $("#txtEALanePrimaryBuyer").kendoMaskedTextBox();
            $("#txtEALaneConsigneeNumber").kendoMaskedTextBox();
            $("#txtEALanePortofEntry").kendoMaskedTextBox();

            $("#ddlEALanePalletType").kendoDropDownList({
                dataTextField: "Description",
                dataValueField: "Name",
                autoWidth: true,
                optionLabel: { Name: "", Description: "" },
                dataSource: {
                    transport: {
                        read: function (options) {
                            $.ajax({
                                async: false,
                                type: "GET",
                                url: "api/vLookupList/GetStaticList/" + nglStaticLists.PalletType,
                                contentType: "application/json; charset=utf-8",
                                dataType: 'json',
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                success: function (data) { options.success(data); },
                                error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Get Pallet Types Failure"); ngl.showErrMsg("Read Pallet Types Error", sMsg, null); }
                            });
                        }
                    },
                    schema: {
                        data: "Data",
                        total: "Count",
                        model: {
                            id: "Control",
                            fields: {
                                Name: { editable: false },
                                Description: { editable: false }
                            }
                        },
                        errors: "Errors"
                    },
                    error: function (e) { ngl.showErrMsg("Read Pallet Types Error", e.errors, null); this.cancelChanges(); },
                    serverPaging: false,
                    serverSorting: false,
                    serverFiltering: false
                }
            });

            $("#ddlEALaneBFCType").kendoDropDownList({
                dataTextField: "Name",
                dataValueField: "Name",
                autoWidth: true,
                optionLabel: { Name: "" },
                dataSource: {
                    transport: {
                        read: function (options) {
                            $.ajax({
                                async: false,
                                type: "GET",
                                url: "api/vLookupList/GetStaticList/" + nglStaticLists.UOM,
                                contentType: "application/json; charset=utf-8",
                                dataType: 'json',
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                success: function (data) { options.success(data); },
                                error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Get UOM Failure"); ngl.showErrMsg("Read UOM Error", sMsg, null); }
                            });
                        }
                    },
                    schema: {
                        data: "Data",
                        total: "Count",
                        model: {
                            id: "Control",
                            fields: {
                                Name: { editable: false },
                                Description: { editable: false }
                            }
                        },
                        errors: "Errors"
                    },
                    error: function (e) { ngl.showErrMsg("Read UOM Error", e.errors, null); this.cancelChanges(); },
                    serverPaging: false,
                    serverSorting: false,
                    serverFiltering: false
                }
            });

            $("#ddlEALaneModeType").kendoDropDownList({
                dataTextField: "Name",
                dataValueField: "Control",
                autoWidth: true,
                //optionLabel: { Description: "Any", Control: "" },
                dataSource: {
                    transport: {
                        read: function (options) {
                            $.ajax({
                                async: false,
                                type: "GET",
                                url: "api/vLookupList/GetStaticList/" + nglStaticLists.tblModeType,
                                contentType: "application/json; charset=utf-8",
                                dataType: 'json',
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                success: function (data) { options.success(data); },
                                error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Get Mode Types Failure"); ngl.showErrMsg("Read Mode Types Error", sMsg, null); }
                            });
                        }
                    },
                    schema: {
                        data: "Data",
                        total: "Count",
                        model: {
                            id: "Control",
                            fields: {
                                Name: { editable: false },
                                Description: { editable: false }
                            }
                        },
                        errors: "Errors"
                    },
                    error: function (e) { ngl.showErrMsg("Read Mode Types Error", e.errors, null); this.cancelChanges(); },
                    serverPaging: false,
                    serverSorting: false,
                    serverFiltering: false
                }
            });


            this.dsLEComps = new kendo.data.DataSource({
                transport: {
                    read: {
                        url: "api/Comp/GetLEComps/" + this.screenLEControl,
                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }
                    },
                    parameterMap: function (options, operation) { return options; }
                },
                schema: {
                    data: "Data",
                    total: "Count",
                    model: {
                        id: "CompControl",
                        fields: {
                            CompControl: { type: "number" },
                            CompName: { type: "string" },
                            CompStreetAddress1: { type: "string" },
                            CompStreetAddress2: { type: "string" },
                            CompStreetAddress3: { type: "string" },
                            CompStreetCity: { type: "string" },
                            CompStreetState: { type: "string" },
                            CompStreetZip: { type: "string" },
                            CompStreetCountry: { type: "string" }
                        }
                    },
                    errors: "Errors"
                },
                error: function (xhr, textStatus, error) { ngl.showErrMsg("GetLEComps JSON Response Error", formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failuire"), null); this.cancelChanges(); },
                serverPaging: false,
                serverSorting: false,
                serverFiltering: false
            });

            $("#ddlEALEComps").kendoDropDownList({
                dataTextField: "CompName",
                dataValueField: "CompControl",
                autoWidth: true,
                dataSource: this.dsLEComps,
                optionLabel: {
                    CompName: "None",
                    CompControl: 0
                }
            });


            $("#ddlEALECompOrig").kendoDropDownList({
                autoBind: false,
                dataTextField: "CompName",
                dataValueField: "CompControl",
                dataSource: this.dsLEComps,
                autoWidth: true,
                optionLabel: {
                    CompName: "None",
                    CompControl: 0
                },
                select: function (e) {
                    var item = e.dataItem;
                    if (!ngl.isNullOrUndefined(item) && ngl.isObject(item)) {
                        if ('CompControl' in item) {
                            //Get the direction
                            var blnInbound = false;
                            var direction = $("#ddlEALTTransType").data("kendoDropDownList").dataItem();
                            if (direction.Control === 3) { blnInbound = true; } //3 = inbound
                            //Get the selection
                            if (item.CompControl !== 0) {
                                //Company was selected --> cannot edit the address info (unless they select 'None')
                                tObj.origAddressFieldsEnabled(false);
                                //Modified by RHR for v-8.5.4.004 on 01/16/2024 The Lane Company can be different
                                //on outbound set the lanecompcontrol = laneorigincompcontrol
                                //if (!blnInbound) {
                                //    var ddl = $("#ddlEALEComps").data("kendoDropDownList");
                                //    ddl.select(function (dataItem) { return dataItem.CompControl === item.CompControl; });
                                //}
                                //set the address info 
                                if ('CompName' in item) { $("#txtEALaneOrigName").data("kendoMaskedTextBox").value(item.CompName); }
                                if ('CompStreetAddress1' in item) { $("#txtEALaneOrigAddress1").data("kendoMaskedTextBox").value(item.CompStreetAddress1); }
                                if ('CompStreetAddress2' in item) { $("#txtEALaneOrigAddress2").data("kendoMaskedTextBox").value(item.CompStreetAddress2); }
                                if ('CompStreetAddress3' in item) { $("#txtEALaneOrigAddress3").data("kendoMaskedTextBox").value(item.CompStreetAddress3); }
                                if ('CompStreetCity' in item) { $("#txtEALaneOrigCity").data("kendoMaskedTextBox").value(item.CompStreetCity); }
                                if ('CompStreetState' in item) { $("#txtEALaneOrigState").data("kendoMaskedTextBox").value(item.CompStreetState); }
                                if ('CompStreetZip' in item) { $("#txtEALaneOrigZip").data("kendoMaskedTextBox").value(item.CompStreetZip); }
                                if ('CompStreetCountry' in item) { $("#txtEALaneOrigCountry").data("kendoMaskedTextBox").value(item.CompStreetCountry); }
                            }
                            else {
                                //"None" is
                                //Modified by RHR for v-8.5.4.004 on 01/22/2024.  anytime None is selected
                                // we let users edit the address data not just on Inbound
                                //if (blnInbound) {
                                    //if it is inbound origin is optional so we can enable the text fields
                                    //tObj.clearOrigAddressFields();
                                    tObj.origAddressFieldsEnabled(true);
                                //}
                            }
                        }
                    }
                }
            });

            $("#ddlEALECompDest").kendoDropDownList({
                autoBind: false,
                dataTextField: "CompName",
                dataValueField: "CompControl",
                dataSource: this.dsLEComps,
                autoWidth: true,
                optionLabel: {
                    CompName: "None",
                    CompControl: 0
                },
                select: function (e) {
                    var item = e.dataItem;
                    if (!ngl.isNullOrUndefined(item) && ngl.isObject(item)) {
                        if ('CompControl' in item) {
                            //Get the direction
                            var blnInbound = false;
                            var direction = $("#ddlEALTTransType").data("kendoDropDownList").dataItem();
                            if (direction.Control === 3) { blnInbound = true; } //3 = inbound
                            //Get the selection
                            if (item.CompControl !== 0) {
                                //Company was selected --> cannot edit the address info (unless they select 'None')
                                tObj.destAddressFieldsEnabled(false);
                                //Modified by RHR for v-8.5.4.004 on 01/16/2024 The Lane Company can be different
                                //on inbound set the lanecompcontrol = laneDestcompcontrol
                                //if (blnInbound) {
                                //    var ddl = $("#ddlEALEComps").data("kendoDropDownList");
                                //    ddl.select(function (dataItem) { return dataItem.CompControl === item.CompControl; });
                                //}
                                //set the address info 
                                if ('CompName' in item) { $("#txtEALaneDestName").data("kendoMaskedTextBox").value(item.CompName); }
                                if ('CompStreetAddress1' in item) { $("#txtEALaneDestAddress1").data("kendoMaskedTextBox").value(item.CompStreetAddress1); }
                                if ('CompStreetAddress2' in item) { $("#txtEALaneDestAddress2").data("kendoMaskedTextBox").value(item.CompStreetAddress2); }
                                if ('CompStreetAddress3' in item) { $("#txtEALaneDestAddress3").data("kendoMaskedTextBox").value(item.CompStreetAddress3); }
                                if ('CompStreetCity' in item) { $("#txtEALaneDestCity").data("kendoMaskedTextBox").value(item.CompStreetCity); }
                                if ('CompStreetState' in item) { $("#txtEALaneDestState").data("kendoMaskedTextBox").value(item.CompStreetState); }
                                if ('CompStreetZip' in item) { $("#txtEALaneDestZip").data("kendoMaskedTextBox").value(item.CompStreetZip); }
                                if ('CompStreetCountry' in item) { $("#txtEALaneDestCountry").data("kendoMaskedTextBox").value(item.CompStreetCountry); }
                            }
                            else {
                                //"None" is selected
                                //Modified by RHR for v-8.5.4.004 on 01/22/2024.  anytime None is selected
                                // we let users edit the address data not just on Inbound
                                //if (!blnInbound) {
                                    //if it is outbound dest is optional so we can enable the text fields
                                    //tObj.clearDestAddressFields();
                                    tObj.destAddressFieldsEnabled(true);
                                //}
                            }
                        }
                    }
                }
            });


            $("#ddlEALTTransType").kendoDropDownList({
                dataTextField: "Name",
                dataValueField: "Control",
                autoWidth: true,
                dataSource: nglvLookupEditors.dsLoadTenderTransType,
                select: function (e) {
                    var item = e.dataItem;
                    if (!ngl.isNullOrUndefined(item) && ngl.isObject(item)) {
                        if ('Control' in item) {
                            tObj.toggleInboundOutbound(item.Control);
                        }
                        else { ngl.showErrMsg("Control Required", "Contact IT - DropDownList Row object does not contain property Control (LTTransType).", null); this.EditError = true; return; }
                    }

                    ////var item = e.dataItem;
                    ////if (!ngl.isNullOrUndefined(item) && ngl.isObject(item)) {
                    ////    if ('Control' in item) {
                    ////        switch (item.Control) {
                    ////            case 3:     //INBOUND                                                                       
                    ////                /* DestCompControl is required */
                    ////                $("#ddlEALECompDest").data("kendoDropDownList").readonly(false); //enable the dropdownlist
                    ////                tObj.clearDestAddressFields();
                    ////                tObj.destAddressFieldsEnabled(false); //disable the text fields
                    ////                //Don't allow them to select "None" in the Dest ddl
                    ////                //** TODO ** FIGURE OUT HOW TO GRAY THE ITEM OUT
                    ////                $("#ddlEALECompDest_listbox .k-item")[0].disabled = true;

                    ////                /* OriginCompControl is optional */
                    ////                $("#ddlEALECompOrig").data("kendoDropDownList").readonly(false); //enable the dropdownlist
                    ////                $("#ddlEALECompOrig").data("kendoDropDownList").select(0); //Select "None"
                    ////                tObj.origAddressFieldsEnabled(true); //enable the text fields
                    ////                break;
                    ////            default:    //OUTBOUND
                    ////                /* OriginCompControl is required */
                    ////                $("#ddlEALECompOrig").data("kendoDropDownList").readonly(false); //enable the dropdownlist
                    ////                tObj.clearOrigAddressFields();
                    ////                tObj.origAddressFieldsEnabled(false); //disable the text fields
                    ////                //Don't allow them to select "None" in the Dest ddl
                    ////                //** TODO ** FIGURE OUT HOW TO GRAY THE ITEM OUT
                    ////                $("#ddlEALECompOrig_listbox .k-item")[0].disabled = true;

                    ////                /* DestCompControl is optional */
                    ////                $("#ddlEALECompDest").data("kendoDropDownList").readonly(false); //enable the dropdownlist
                    ////                $("#ddlEALECompDest").data("kendoDropDownList").select(0); //Select "None"
                    ////                tObj.destAddressFieldsEnabled(true); //enable the text fields
                    ////                break;
                    ////        }
                    ////    }
                    ////    else { ngl.showErrMsg("Control Required", "Contact IT - DropDownList Row object does not contain property Control (LTTransType).", null); this.EditError = true; return; }
                    ////}
                }
            });

            $("#ddlEALaneTrans").kendoDropDownList({
                dataTextField: "Name",
                dataValueField: "Description", //because LaneTransNumber is saved to LaneTransType
                autoWidth: true,
                dataSource: {
                    transport: {
                        read: function (options) {
                            $.ajax({
                                async: false,
                                type: "GET",
                                url: "api/vLookupList/GetStaticList/" + nglStaticLists.LaneTran,
                                contentType: "application/json; charset=utf-8",
                                dataType: 'json',
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                success: function (data) {
                                    options.success(data);
                                },
                                error: function (xhr, textStatus, error) {
                                    var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Get Lane Trans Type Failure");
                                    ngl.showErrMsg("Read Lane Trans Type Error", sMsg, null);
                                }
                            });
                        }
                    },
                    schema: {
                        data: "Data",
                        total: "Count",
                        model: {
                            id: "Control",
                            fields: {
                                Name: { editable: false },
                                Description: { editable: false }
                            }
                        },
                        errors: "Errors"
                    },
                    error: function (e) {
                        ngl.showErrMsg("Read Lane Trans Type Error", e.errors, null);
                        this.cancelChanges();
                    },
                    serverPaging: false,
                    serverSorting: false,
                    serverFiltering: false
                }
            });

            $("#ddlEATempType").kendoDropDownList({
                dataTextField: "Description",
                dataValueField: "Control",
                autoWidth: true,
                optionLabel: {
                    Description: "Any",
                    Control: ""
                },
                dataSource: {
                    transport: {
                        read: function (options) {
                            $.ajax({
                                async: false,
                                type: "GET",
                                url: "api/vLookupList/GetStaticList/" + nglStaticLists.TempType,
                                contentType: "application/json; charset=utf-8",
                                dataType: 'json',
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                success: function (data) {
                                    options.success(data);
                                },
                                error: function (xhr, textStatus, error) {
                                    var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Get Temp Type Failure");
                                    ngl.showErrMsg("Read Temp Type Error", sMsg, null);
                                }
                            });
                        }
                    },
                    schema: {
                        data: "Data",
                        total: "Count",
                        model: {
                            id: "Control",
                            fields: {
                                Name: { editable: false },
                                Description: { editable: false }
                            }
                        },
                        errors: "Errors"
                    },
                    error: function (e) {
                        ngl.showErrMsg("Read Temp Type Error", e.errors, null);
                        this.cancelChanges();
                    },
                    serverPaging: false,
                    serverSorting: false,
                    serverFiltering: false
                }
            });



            //** Set up the window for "Add New" on initialize **
            //** This way we only have to do special set up in the "Edit" method

            // Modified by RHR for v-8.5.4.004 on 01/16/2024 we no longer force a link between Orig and Dest address and Lane Company
            //  they can be different
            //This ddl is always readonly because it is determined by the orig/dest address (depending on if it is inbound or outbound)
            //$("#ddlEALEComps").data("kendoDropDownList").readonly();
            //Modified By LVV on 5/18/20 - New Rule: Hide the Lane Comp DDL since it is not editable
            //$("#divLaneCompDDL").hide();

            //Select "Inbound" by default
            //$("#ddlEALTTransType").data("kendoDropDownList").select(function (dataItem) { return dataItem.Control === 3; });
            //this.toggleInboundOutbound(3);

            //Modified By RHR for v-8.2 on 07/24/2018
            //  new logic to set Outbound as the default
            $("#ddlEALTTransType").data("kendoDropDownList").select(function (dataItem) { return dataItem.Control === 1; });
            this.toggleInboundOutbound(1);


            //Check "LaneActive"
            $("#chkEALaneActive").prop('checked', true);
            //Check "Inbound"
            //Modified by RHR for v-8.2 on 07/24/2018 the default is now Outbound (checked = false)
            $("#chkEALaneOrigAddressUse").prop('checked', false);

            //removed by RHR we now let users edit the lane number.
            //Hide LaneNumber on Add since this is auto-generated
            //$("#divLaneNumber").hide();

            //Show LTTransType ddl and hide Inbound chk on Edit
            $("#divLTTransType").show();
            $("#divInbound").hide();

            //Set LegalEntity to readonly because this comes from the page
            $("#txtEALaneLegalEntity").data("kendoMaskedTextBox").readonly(true);

            //Clear the date fields
            //Modified by RHR for v-8.5.4.003 on 10/28/2023 changed from datetimepicker to kendoMaskedTextBox
            $("#tpEALaneRecHourStart").data("kendoMaskedTextBox").value("");
            $("#tpEALaneRecHourStop").data("kendoMaskedTextBox").value("");
            $("#tpEALaneDestHourStart").data("kendoMaskedTextBox").value("");
            $("#tpEALaneDestHourStop").data("kendoMaskedTextBox").value("");
            //Clear the updated flag
            $("#txtEALaneUpdated").val("");
            //Capacity Settings
            $("#txtEALaneTLWgt").data("kendoNumericTextBox").value(0); //Added By LVV on 5/18/20 - default TL capcity settings to 0 on create new lane from 365
            $("#txtEALaneTLCases").data("kendoNumericTextBox").value(0); //Added By LVV on 5/18/20 - default TL capcity settings to 0 on create new lane from 365
            $("#txtEALaneTLCube").data("kendoNumericTextBox").value(0); //Added By LVV on 5/18/20 - default TL capcity settings to 0 on create new lane from 365
            $("#txtEALaneTLPL").data("kendoNumericTextBox").value(0); //Added By LVV on 5/18/20 - default TL capcity settings to 0 on create new lane from 365
            //Added By LVV on 10/12/20 for v-8.3.0.001 Task #20201012142747 - Add Fields to Lane Maint           
            $("#chkEALaneIsTransLoad").prop('checked', false); //Uncheck "Transload Active"           
            $("#chkEALaneIsCrossDockFacility").prop('checked', false); //Uncheck "Cross Dock Facility"
            $("#txtEALaneBenchMiles").data("kendoNumericTextBox").value(0);
            $("#txtEALaneLatitude").data("kendoNumericTextBox").value(0);
            $("#txtEALaneLongitude").data("kendoNumericTextBox").value(0);
            $("#txtEALaneDefaultCarrierText").data("kendoMaskedTextBox").readonly(true); //not editable
            $("#ddlEALaneModeType").data("kendoDropDownList").select(2);

            /************* Fast Tab Functions *************/
            expandEALane = function () {
                $("#divFastTabEALane").show();
                $("#ExpandEALaneSpan").hide();
                $("#CollapseEALaneSpan").show();
            }
            collapseEALane = function () {
                $("#divFastTabEALane").hide();
                $("#ExpandEALaneSpan").show();
                $("#CollapseEALaneSpan").hide();
            }
            expandEALaneOrigCont = function () {
                $("#divFastTabEALaneOrigCont").show();
                $("#ExpandEALaneOrigContSpan").hide();
                $("#CollapseEALaneOrigContSpan").show();
            }
            collapseEALaneOrigCont = function () {
                $("#divFastTabEALaneOrigCont").hide();
                $("#ExpandEALaneOrigContSpan").show();
                $("#CollapseEALaneOrigContSpan").hide();
            }
            expandEALaneDestCont = function () {
                $("#divFastTabEALaneDestCont").show();
                $("#ExpandEALaneDestContSpan").hide();
                $("#CollapseEALaneDestContSpan").show();
            }
            collapseEALaneDestCont = function () {
                $("#divFastTabEALaneDestCont").hide();
                $("#ExpandEALaneDestContSpan").show();
                $("#CollapseEALaneDestContSpan").hide();
            }
            //Added By LVV on 10/12/20 for v-8.3.0.001 Task #20201012142747 - Add Fields to Lane Maint
            expandAddressBlock = function () {
                $("#divFastTabAddressBlock").show();
                $("#ExpandAddressBlock").hide();
                $("#CollapseAddressBlock").show();
            }
            collapseAddressBlock = function () {
                $("#divFastTabAddressBlock").hide();
                $("#ExpandAddressBlock").show();
                $("#CollapseAddressBlock").hide();
            }
            expandCommentsBlock = function () {
                $("#divFastTabCommentsBlock").show();
                $("#ExpandCommentsBlock").hide();
                $("#CollapseCommentsBlock").show();
            }
            collapseCommentsBlock = function () {
                $("#divFastTabCommentsBlock").hide();
                $("#ExpandCommentsBlock").show();
                $("#CollapseCommentsBlock").hide();
            }
            expandCapacitySettings = function () {
                $("#divFastTabCapacitySettings").show();
                $("#ExpandCapacitySettings").hide();
                $("#CollapseCapacitySettings").show();
            }
            collapseCapacitySettings = function () {
                $("#divFastTabCapacitySettings").hide();
                $("#ExpandCapacitySettings").show();
                $("#CollapseCapacitySettings").hide();
            }
            expandDynamicRouting = function () {
                $("#divFastTabDynamicRouting").show();
                $("#ExpandDynamicRouting").hide();
                $("#CollapseDynamicRouting").show();
            }
            collapseDynamicRouting = function () {
                $("#divFastTabDynamicRouting").hide();
                $("#ExpandDynamicRouting").show();
                $("#CollapseDynamicRouting").hide();
            }
            expandStaticRouting = function () {
                $("#divFastTabStaticRouting").show();
                $("#ExpandStaticRouting").hide();
                $("#CollapseStaticRouting").show();
            }
            collapseStaticRouting = function () {
                $("#divFastTabStaticRouting").hide();
                $("#ExpandStaticRouting").show();
                $("#CollapseStaticRouting").hide();
            }
            expandOptions = function () {
                $("#divFastTabOptions").show();
                $("#ExpandOptions").hide();
                $("#CollapseOptions").show();
            }
            collapseOptions = function () {
                $("#divFastTabOptions").hide();
                $("#ExpandOptions").show();
                $("#CollapseOptions").hide();
            }
            expandDefaultProvider = function () {
                $("#divFastTabDefaultProvider").show();
                $("#ExpandDefaultProvider").hide();
                $("#CollapseDefaultProvider").show();
            }
            collapseDefaultProvider = function () {
                $("#divFastTabDefaultProvider").hide();
                $("#ExpandDefaultProvider").show();
                $("#CollapseDefaultProvider").hide();
            }
            expandConfig = function () {
                $("#divFastTabConfig").show();
                $("#ExpandConfig").hide();
                $("#CollapseConfig").show();
            }
            collapseConfig = function () {
                $("#divFastTabConfig").hide();
                $("#ExpandConfig").show();
                $("#CollapseConfig").hide();
            }

        } else { this.kendoWindowsObj = null; }
    }
}

//Added By LVV on 5/7/18 for v-8.1
//Widget for Edit/Add on LECarrier Maint - html data store in Views/CarSetEAWindow.html
//NOTE: NOT ACTUALLY BEING USED ANYWHERE -- NOT FINISHED
function CarSetEAWndCtrl() {
    //Generic properties for all Widgets
    this.lastErr = "";
    this.notifyOnError = true;
    this.notifyOnSuccess = true;
    this.notifyOnValidationFailure = true;
    data: CarrierDispatchSettings;
    rData: null;
    onSave: null;
    onDelete: null;
    dsLEComps: kendo.data.DataSource;
    kendoWindowsObj: null;
    //Widget specific properties
    screenLEName: null;
    screenLEControl: null;
    deleteCarSetControl: 0;
    this.EditError = false;
    this.blnIsEdit = false;

    this.kendoWindowsObjUploadEventAdded = 0;


    this.confirmDeleteCarSet = function (iRet) {
        if (typeof (iRet) === 'undefined' || iRet === null || iRet === 0) { return; }

        if (!ngl.isNullOrUndefined(this.deleteCarSetControl) && this.deleteCarSetControl > 0) {
            var tObj = this;
            var oCRUDCtrl = new nglRESTCRUDCtrl();
            var blnRet = oCRUDCtrl.delete("LegalEntityCarrier/DeleteLegalEntityCarrier", this.deleteCarSetControl, tObj, "deleteSuccessCallback", "deleteAjaxErrorCallback");
        }
    }


    //Generic Call back functions for all Widgets
    this.saveSuccessCallback = function (data) {
        //debugger;
        var oResults = new nglEventParameters();
        oResults.source = "saveSuccessCallback";
        var tObj = this;
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed   
        //kendo.ui.progress($(document.body), false);
        this.rData = null;
        try {
            var blnSuccess = false;
            var blnErrorShown = false;
            var strValidationMsg = "";
            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                    oResults.error = new Error();
                    oResults.error.name = "Save Carrier Settings Failure";
                    oResults.error.message = data.Errors;
                    blnErrorShown = true;
                    ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                }
                else {
                    this.rData = data.Data;
                    if (typeof (data.Data) !== 'undefined' && data.Data != null && ngl.isArray(data.Data)) {
                        if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                            blnSuccess = true;
                            oResults.data = data.Data;
                            oResults.datatype = "string";
                            oResults.msg = "<h3>Success</h3>" + data.Data[0]
                            //Only close the window if the save was successful
                            this.kendoWindowsObj.close();
                            ;
                            //ngl.showSuccessMsg(oResults.msg, tObj);
                        }
                    }

                }
            }
            if (blnSuccess === false && blnErrorShown === false) {
                oResults.error = new Error();
                oResults.error.name = "Save Carrier Settings Failure";
                oResults.error.message = "No results are available.";
                ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
            }
        } catch (err) {
            oResults.error = err
        }
        if (ngl.isFunction(this.onSave)) {
            this.onSave(oResults);
        }

    }
    this.saveAjaxErrorCallback = function (xhr, textStatus, error) {
        var oResults = new nglEventParameters();
        var tObj = this;
        //kendo.ui.progress($(document.body), false);
        oResults.source = "saveAjaxErrorCallback";
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed  
        oResults.error = new Error();
        oResults.error.name = "Save Carrier Settings Failure"
        oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);

        if (ngl.isFunction(this.onSave)) {
            this.onSave(oResults);
            //this.kendoWindowsObj.close();
        }
    }

    this.deleteSuccessCallback = function (data) {
        var oResults = new nglEventParameters();
        oResults.source = "deleteSuccessCallback";
        var tObj = this;
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed   
        //kendo.ui.progress($(document.body), false);
        this.rData = null;
        try {
            var blnSuccess = false;
            var blnErrorShown = false;
            var strValidationMsg = "";
            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                    oResults.error = new Error();
                    oResults.error.name = "Delete Carrier Settings Failure";
                    oResults.error.message = data.Errors;
                    blnErrorShown = true;
                    ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                }
            }
        } catch (err) {
            oResults.error = err
        }
        if (ngl.isFunction(this.onDelete)) {
            this.onDelete(oResults);
        }
    }
    this.deleteAjaxErrorCallback = function (xhr, textStatus, error) {
        var oResults = new nglEventParameters();
        var tObj = this;
        //kendo.ui.progress($(document.body), false);
        oResults.source = "deleteAjaxErrorCallback";
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed  
        oResults.error = new Error();
        oResults.error.name = "Delete Carrier Settings Failure"
        oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);

        if (ngl.isFunction(this.onDelete)) {
            this.onDelete(oResults);
        }
    }

    // Generic functions for all Widgets
    this.validateRequiredFields = function (data, blnIsTransfer) {
        var oRet = new validationResults();
        if (typeof (data) === 'undefined' || !ngl.isObject(data)) {
            //something went wrong so throw an error. this should never happen
            ngl.showValidationMsg("Save Lane Validation Failed; No Data", "Please Contact Technical Support", tObj);
            oRet.Message = "Save Lane Validation Failed; No Data";
            return oRet;
        }
        var blnValidated = true;
        var sValidationMsg = "";
        var sSpacer = "";

        if (isEmpty(data.LaneName)) { blnValidated = false; sValidationMsg += sSpacer + "Lane Name"; sSpacer = ", "; }
        if (isEmpty(data.LaneLegalEntity)) { blnValidated = false; sValidationMsg += sSpacer + "Legal Entity"; sSpacer = ", "; }
        if (ngl.isNullOrUndefined(data.LaneCompControl) || data.LaneCompControl === 0) { blnValidated = false; sValidationMsg += sSpacer + "Company"; sSpacer = ", "; }
        if (ngl.isNullOrUndefined(data.LaneActive)) { blnValidated = false; sValidationMsg += sSpacer + "Active"; sSpacer = ", "; }

        if (ngl.isNullOrUndefined(data.LaneOriginAddressUse)) {
            blnValidated = false; sValidationMsg += sSpacer + "Direction"; sSpacer = ", ";
        }
        else {

            if (data.LaneOriginAddressUse) {
                //Inbound so LaneDestCompControl is required
                
                if (ngl.isNullOrUndefined(data.LaneDestCompControl) || data.LaneDestCompControl === 0) { blnValidated = false; sValidationMsg += sSpacer + "Dest Company"; sSpacer = ", "; }
            }
            else {
                if (blnIsTransfer) {
                    //Transfer so both LaneOrigCompControl and LaneDestCompControl are required
                    if (ngl.isNullOrUndefined(data.LaneDestCompControl) || data.LaneDestCompControl === 0) { blnValidated = false; sValidationMsg += sSpacer + "Dest Company"; sSpacer = ", "; }
                    if (ngl.isNullOrUndefined(data.LaneOrigCompControl) || data.LaneOrigCompControl === 0) { blnValidated = false; sValidationMsg += sSpacer + "Orig Company"; sSpacer = ", "; }
                }
                else {
                    //Outbound so LaneOrigCompControl is required
                    if (ngl.isNullOrUndefined(data.LaneOrigCompControl) || data.LaneOrigCompControl === 0) { blnValidated = false; sValidationMsg += sSpacer + "Orig Company"; sSpacer = ", "; }
                }
            }
        }

        //Orig
        if (isEmpty(data.LaneOrigName)) { blnValidated = false; sValidationMsg += sSpacer + "Orig Name"; sSpacer = ", "; }
        if (isEmpty(data.LaneOrigAddress1)) { blnValidated = false; sValidationMsg += sSpacer + "Orig Address 1"; sSpacer = ", "; }
        if (isEmpty(data.LaneOrigCity)) { blnValidated = false; sValidationMsg += sSpacer + "Orig City"; sSpacer = ", "; }
        if (isEmpty(data.LaneOrigState)) { blnValidated = false; sValidationMsg += sSpacer + "Orig State"; sSpacer = ", "; }
        if (isEmpty(data.LaneOrigZip)) { blnValidated = false; sValidationMsg += sSpacer + "Orig Postal Code"; sSpacer = ", "; }
        if (isEmpty(data.LaneOrigCountry)) { blnValidated = false; sValidationMsg += sSpacer + "Orig Country"; sSpacer = ", "; }
        if (isEmpty(data.LaneOrigContactName)) { blnValidated = false; sValidationMsg += sSpacer + "Orig Cont Name"; sSpacer = ", "; }
        if (isEmpty(data.LaneOrigContactEmail)) { blnValidated = false; sValidationMsg += sSpacer + "Orig Cont Email"; sSpacer = ", "; }
        if (isEmpty(data.LaneOrigEmergencyContactPhone)) { blnValidated = false; sValidationMsg += sSpacer + "Orig Emergency Name"; sSpacer = ", "; }
        if (isEmpty(data.LaneOrigEmergencyContactName)) { blnValidated = false; sValidationMsg += sSpacer + "Orig Emergency Phone"; sSpacer = ", "; }
        //Dest
        if (isEmpty(data.LaneDestName)) { blnValidated = false; sValidationMsg += sSpacer + "Dest Name"; sSpacer = ", "; }
        if (isEmpty(data.LaneDestAddress1)) { blnValidated = false; sValidationMsg += sSpacer + "Dest Address 1"; sSpacer = ", "; }
        if (isEmpty(data.LaneDestCity)) { blnValidated = false; sValidationMsg += sSpacer + "Dest City"; sSpacer = ", "; }
        if (isEmpty(data.LaneDestState)) { blnValidated = false; sValidationMsg += sSpacer + "Dest State"; sSpacer = ", "; }
        if (isEmpty(data.LaneDestZip)) { blnValidated = false; sValidationMsg += sSpacer + "Dest Postal Code"; sSpacer = ", "; }
        if (isEmpty(data.LaneDestCountry)) { blnValidated = false; sValidationMsg += sSpacer + "Dest Country"; sSpacer = ", "; }
        if (isEmpty(data.LaneDestContactName)) { blnValidated = false; sValidationMsg += sSpacer + "Dest Cont Name"; sSpacer = ", "; }
        if (isEmpty(data.LaneDestContactEmail)) { blnValidated = false; sValidationMsg += sSpacer + "Dest Cont Email"; sSpacer = ", "; }
        if (isEmpty(data.LaneDestEmergencyContactPhone)) { blnValidated = false; sValidationMsg += sSpacer + "Dest Emergency Name"; sSpacer = ", "; }
        if (isEmpty(data.LaneDestEmergencyContactName)) { blnValidated = false; sValidationMsg += sSpacer + "Dest Emergency Phone"; sSpacer = ", "; }


        oRet.Success = blnValidated;
        oRet.Message = sValidationMsg;

        return oRet;
    }

    // Generic CRUD functions for all Widgets
    this.save = function () {
        var tObj = this;

        this.data = new CarrierDispatchSettings(); //clear any old data

        var otmp = $("#focusCancel").focus();

        var tfRateShopOnly = false;
        var tfAPIDispatching = false;
        var tfAPIStatusUpdates = false;
        var tfShowAuditFailReason = false;
        var tfShowPendingFeeFailReason = false;

        if ($('#chkRateShopOnly').is(":checked")) { tfRateShopOnly = true; }
        if ($('#chkAPIDispatching').is(":checked")) { tfAPIDispatching = true; }
        if ($('#chkAPIStatusUpdates').is(":checked")) { tfAPIStatusUpdates = true; }
        if ($('#chkShowAuditFailReason').is(":checked")) { tfShowAuditFailReason = true; }
        if ($('#chkShowPendingFeeFailReason').is(":checked")) { tfShowPendingFeeFailReason = true; }

        this.data.LEAdminControl = pgLEControl;

        var dataItemC = $("#ddlCarriers").data("kendoDropDownList").dataItem();
        this.data.CarrierControl = dataItemC.Control;

        var dataItemDT = $("#ddlDispatchType").data("kendoDropDownList").dataItem();
        this.data.DispatchTypeControl = dataItemDT.Control;

        this.data.RateShopOnly = tfRateShopOnly;
        this.data.APIDispatching = tfAPIDispatching;
        this.data.APIStatusUpdates = tfAPIStatusUpdates;
        this.data.ShowAuditFailReason = tfShowAuditFailReason;
        this.data.ShowPendingFeeFailReason = tfShowPendingFeeFailReason;

        ////if (intLaneControl === 0) {
        ////    //Only Validate on Insert
        ////    var oValidationResults = this.validateRequiredFields(this.data, blnIsTransfer);
        ////    if (typeof (oValidationResults) === 'undefined' || !ngl.isObject(oValidationResults)) {
        ////        ngl.showValidationMsg("Cannot validate Lane Information", "Invalid validation procedure, please contact technical support", tObj);
        ////        return;
        ////    }
        ////    else { if (oValidationResults.Success === false) { ngl.showValidationMsg("Required Fields", oValidationResults.Message, tObj); return; } }
        ////}
        //////save the changes

        var oCRUDCtrl = new nglRESTCRUDCtrl();
        oCRUDCtrl.update("LegalEntityCarrier/SaveLegalEntityCarrier", this.data, tObj, "saveSuccessCallback", "saveAjaxErrorCallback");
    }

    this.show = function () {
        var tObj = this;

        this.kendoWindowsObj = $("#wndCarSet").kendoWindow({
            title: "Add New Carrier",
            modal: true,
            visible: false,
            //height: '50%',
            //width: '50%',
            height: 485,
            width: 475,
            actions: ["save", "Minimize", "Maximize", "Close"],
            close: function (e) { tObj.close(e); }
        }).data("kendoWindow");

        if (this.kendoWindowsObjUploadEventAdded === 0) {
            $("#wndCarSet").data("kendoWindow").wrapper.find(".k-svg-i-save").parent().click(function (e) { e.preventDefault(); tObj.save(); });
        }
        this.kendoWindowsObjUploadEventAdded = 1;

        //if this is an edit load the data to the window
        if (typeof (this.data) !== 'undefined' && ngl.isObject(this.data)) {
            this.edit(this.data);
        }
        else {
            this.kendoWindowsObj.title("Add New Carrier");
            //Show all the red "required field" * on Add
            $(".redRequiredAsterik").show();
            ////Get the LegalEntity from the screen on Add New
            //$("#txtEALaneLegalEntity").data("kendoMaskedTextBox").value(this.screenLEName);
            //Set the LE label
            var l = "<h3>Legal Entity: " + this.screenLEName + "</h3>";
            $("#lblLE").html(l);
        }

        if (this.EditError === false) { this.kendoWindowsObj.center().open(); }
    }

    this.edit = function (data) {
        //load data to the screen
        var tObj = this;

        this.blnIsEdit = true;

        this.kendoWindowsObj.title("Edit Carrier Settings");

        //Set the LE label
        var l = "<h3>Legal Entity: " + this.screenLEName + "</h3>";
        $("#lblLE").html(l);
        //Hide all the red "required field" * on Edit
        $(".redRequiredAsterik").hide();


        if (typeof (data) !== 'undefined' && data != null && ngl.isObject(data)) {
            //Verify LaneControl is a column in the grid and is not null or 0
            if ('LECarControl' in data) { //REPLACE *
                if (typeof (data.LECarControl) === 'undefined' || data.LECarControl == null || data.LECarControl == 0) { ngl.showErrMsg("LECarControl Required", "LECarControl cannot be 0", null); this.EditError = true; return; } //REPLACE *

                $("#ddlCarriers").data("kendoDropDownList").select(function (dataItem) { return dataItem.Control === data.CarrierControl; });
                $("#ddlCarriers").data("kendoDropDownList").readonly();
                $("#ddlDispatchType").data("kendoDropDownList").select(function (dataItem) { return dataItem.Control === data.DispatchTypeControl; });
                $("#ddlDispatchType").data("kendoDropDownList").readonly();

                if ('RateShopOnly' in data) {
                    if (data.RateShopOnly) { $("#chkRateShopOnly").prop('checked', true); } else { $("#chkRateShopOnly").prop('checked', false); }
                }
                if ('APIDispatching' in data) {
                    if (data.APIDispatching) { $("#chkAPIDispatching").prop('checked', true); } else { $("#chkAPIDispatching").prop('checked', false); }
                }
                if ('APIStatusUpdates' in data) {
                    if (data.APIStatusUpdates) { $("#chkAPIStatusUpdates").prop('checked', true); } else { $("#chkAPIStatusUpdates").prop('checked', false); }
                }
                if ('ShowAuditFailReason' in data) {
                    if (data.ShowAuditFailReason) { $("#chkShowAuditFailReason").prop('checked', true); } else { $("#chkShowAuditFailReason").prop('checked', false); }
                }
                if ('ShowPendingFeeFailReason' in data) {
                    if (data.ShowPendingFeeFailReason) { $("#chkShowPendingFeeFailReason").prop('checked', true); } else { $("#chkShowPendingFeeFailReason").prop('checked', false); }
                }
            }
            else { ngl.showErrMsg("LECarControl Required", "Row object does not contain property LECarControl.", null); this.EditError = true; return; } //REPLACE *
        }
        else { ngl.showErrMsg("Error", "There was a problem getting the row from the grid", null); this.EditError = true; return; }
    }

    this.delete = function (data) {
        var tObj = this;
        var carrierName = "";//CarrierName

        if (!ngl.isNullOrUndefined(data) && ngl.isObject(data)) {
            //Verify LaneControl is a column in the grid and is not null or 0
            if ('LECarControl' in data) { //REPLACE *
                if (ngl.isNullOrUndefined(data.LECarControl) || data.LECarControl == 0) { ngl.showErrMsg("LECarControl Required", "LECarControl cannot be 0", null); return; } //REPLACE *
                this.deleteCarSetControl = data.LECarControl; //REPLACE *
                if ('CarrierName' in data) { carrierName = data.CarrierName; }
            }
            else { ngl.showErrMsg("LECarControl Required", "Row object does not contain property LECarControl.", null); return; } //REPLACE *
        }
        else { ngl.showErrMsg("Error", "There was a problem getting the row from the grid", null); return; }

        var msgPrompt = "Are you sure that you want to delete this Carrier?";
        //Commented this out until the bug can be fixed
        //if (!ngl.isNullOrUndefined(carrierName) && carrierName.length > 0) { msgPrompt = "Are you sure that you want to delete Carrier " + carrierName + "?"; }

        ngl.OkCancelConfirmation(
            "Delete Confirmation",
            msgPrompt,
            400,
            400,
            tObj,
            "confirmDeleteCarSet");
    }

    this.close = function (e) {
        //** Always Set up the window for "Add New" on close **
        //** This way we only have to do special set up in the "Edit" method
        //reset values

        $("#txtCaption").data("kendoMaskedTextBox").value("");
        $("#txtEDICode").data("kendoMaskedTextBox").value("");
        $("#txtApproveToleranceLow").data("kendoNumericTextBox").value(0);
        $("#txtApproveToleranceHigh").data("kendoNumericTextBox").value(0);
        $("#txtApproveTolerancePerLow").data("kendoNumericTextBox").value(0);
        $("#txtApproveTolerancePerHigh").data("kendoNumericTextBox").value(0);
        $("#txtAverageValue").data("kendoNumericTextBox").value(0);


        //Set readonly to false and select first item on Add
        var dropdownlist = $("#ddlCarriers").data("kendoDropDownList");
        dropdownlist.readonly(false);
        dropdownlist.select(0);

        //Select "NGLTariff" by default and set readonly to false on Add
        var ddl = $("#ddlDispatchType").data("kendoDropDownList");
        ddl.select(function (dataItem) { return dataItem.Control === 1; });
        ddl.readonly(false);
        //ddl.readonly();

        //Uncheck "RateShopOnly", "APIDispatching", "APIStatusUpdates", "ShowAuditFailReason", and "ShowPendingFeeFailReason"
        $("#chkRateShopOnly").prop('checked', false);
        $("#chkAPIDispatching").prop('checked', false);
        $("#chkAPIStatusUpdates").prop('checked', false);
        $("#chkShowAuditFailReason").prop('checked', false);
        $("#chkShowPendingFeeFailReason").prop('checked', false);

        //reset the LE so we get it fresh from the screen everytime
        this.screenLEName = "";
        screenLEControl = 0;
        this.deleteCarSetControl = 0;
        this.blnIsEdit = false;

        //NOTE: The title of the window gets set to "Add" every time the show method is called so no need to reset here
    }

    //loadDefaults sets up the callbacks cbSelect and cbSave
    //all call backs return a reference to this object and a string message as parameters
    this.loadDefaults = function (pageVariable, saveCallBack, deleteCallBack) {

        this.onSave = saveCallBack;
        this.onDelete = deleteCallBack;
        this.data = new CarrierDispatchSettings();
        this.lastErr = "";
        this.notifyOnError = true;
        this.notifyOnSuccess = true;
        this.notifyOnValidationFailure = true;
        var tObj = this;

        if (typeof (pageVariable) !== 'undefined' && ngl.isObject(pageVariable)) {

            this.kendoWindowsObj = pageVariable;

            //Kendo Definitions
            $("#txtCaption").kendoMaskedTextBox();
            $("#txtEDICode").kendoMaskedTextBox();
            $("#txtApproveToleranceLow").kendoNumericTextBox({ format: "{0:c2}" });
            $("#txtApproveToleranceHigh").kendoNumericTextBox({ format: "{0:c2}" });
            $("#txtApproveTolerancePerLow").kendoNumericTextBox({
                format: "p0",
                factor: 100,
                min: 0,
                max: 1,
                step: 0.01
            });
            $("#txtApproveTolerancePerHigh").kendoNumericTextBox({
                format: "p0",
                factor: 100,
                min: 0,
                max: 1,
                step: 0.01
            });

            $("#txtAverageValue").kendoNumericTextBox({
                change: function () {
                    var value = this.value();
                    if (value > 0) {
                        $("#txtApproveTolerancePerLow").data("kendoNumericTextBox").enable(true);
                        $("#txtApproveTolerancePerHigh").data("kendoNumericTextBox").enable(true);
                        $("#txtApproveToleranceLow").data("kendoNumericTextBox").enable(false);
                        $("#txtApproveToleranceHigh").data("kendoNumericTextBox").enable(false);

                        $("#txtApproveToleranceLow").data("kendoNumericTextBox").value(0);
                        $("#txtApproveToleranceHigh").data("kendoNumericTextBox").value(0);
                    }
                    if (value === 0) {
                        $("#txtApproveTolerancePerLow").data("kendoNumericTextBox").enable(false);
                        $("#txtApproveTolerancePerHigh").data("kendoNumericTextBox").enable(false);
                        $("#txtApproveToleranceLow").data("kendoNumericTextBox").enable(true);
                        $("#txtApproveToleranceHigh").data("kendoNumericTextBox").enable(true);

                        $("#txtApproveTolerancePerLow").data("kendoNumericTextBox").value(0);
                        $("#txtApproveTolerancePerHigh").data("kendoNumericTextBox").value(0);
                    }
                }
            });

            $("#ddlCarriers").kendoDropDownList({
                dataTextField: "Name",
                dataValueField: "Control",
                autoWidth: true,
                filter: "contains",
                dataSource: {
                    serverFiltering: false,
                    transport: {
                        read: {
                            url: "api/vLookupList/GetUserDynamicList/13",
                            headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }
                        }
                    },
                    schema: {
                        data: "Data",
                        total: "Count",
                        model: {
                            id: "Control",
                            fields: {
                                Control: { type: "number" },
                                Name: { type: "string" },
                                Description: { type: "string" }
                            }
                        },
                        errors: "Errors"
                    },
                    error: function (xhr, textStatus, error) { ngl.showErrMsg("Get Carriers JSON Response Error", formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"), null); this.cancelChanges(); }
                }
            });

            $("#ddlDispatchType").kendoDropDownList({
                dataTextField: "Name",
                dataValueField: "Control",
                autoWidth: true,
                filter: "contains",
                dataSource: {
                    serverFiltering: false,
                    transport: {
                        read: {
                            url: "api/vLookupList/GetStaticList/62",
                            headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }
                        }
                    },
                    schema: {
                        data: "Data",
                        total: "Count",
                        model: {
                            id: "Control",
                            fields: {
                                Control: { type: "number" },
                                Name: { type: "string" },
                                Description: { type: "string" }
                            }
                        },
                        errors: "Errors"
                    },
                    error: function (xhr, textStatus, error) { ngl.showErrMsg("Get DispatchType JSON Response Error", formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"), null); this.cancelChanges(); }
                }
            });

            var l = "<h3>Legal Entity: " + this.screenLEName + "</h3>";
            $("#lblLE").html(l);


            //** Set up the window for "Add New" on initialize **
            //** This way we only have to do special set up in the "Edit" method

            //Set readonly to false and select first item on Add
            var dropdownlist = $("#ddlCarriers").data("kendoDropDownList");
            dropdownlist.readonly(false);
            dropdownlist.select(0);

            //Select "NGLTariff" by default and set readonly to false on Add
            var ddl = $("#ddlDispatchType").data("kendoDropDownList");
            ddl.select(function (dataItem) { return dataItem.Control === 1; });
            ddl.readonly(false);
            //ddl.readonly();

            //Uncheck "RateShopOnly", "APIDispatching", "APIStatusUpdates", "ShowAuditFailReason", and "ShowPendingFeeFailReason"
            $("#chkRateShopOnly").prop('checked', false);
            $("#chkAPIDispatching").prop('checked', false);
            $("#chkAPIStatusUpdates").prop('checked', false);
            $("#chkShowAuditFailReason").prop('checked', false);
            $("#chkShowPendingFeeFailReason").prop('checked', false);

        } else {
            this.kendoWindowsObj = null;
        }

    }
}


//Display the load dispatching information report
//in Views/DispatchingReport.html
////function BOLReportCtrl() {
////    //Generic properties for all Widgets
////    this.lastErr = "";
////    this.notifyOnError = true;
////    this.notifyOnSuccess = true;
////    this.notifyOnValidationFailure = true;
////    data: Dispatch;
////    rData: null;
////    onSelect: null;
////    onSave: null;
////    onClose: null;
////    onRead: null;
////    dSource: null;
////    sourceDiv: null;
////    kendoWindowsObj: null;
////    strAdditionalServices: "";
////    //Widget specific properties

////    this.kendoWindowsObjUploadEventAdded = 0;

////    //Widget specific functions
////    this.calcLinearFt = function () {
////        var iRet = 0;
////        if (typeof (this.data) !== 'undefined' && ngl.isObject(this.data)) {
////            if (typeof (this.data.Items) !== 'undefined' && ngl.isArray(this.data.Items) && this.data.Items.length > 0) {
////                var TotalLen = 0;
////                for (i = 0, len = this.data.Items.length; i < len; i++) {
////                    var oItem = this.data.Items[i];
////                    var iLen = this.data.Items[i].ItemLength
////                    if (typeof (iLen) !== 'undefined' && iLen !== null && !isNaN(iLen) && iLen > 0) {
////                        TotalLen += iLen;
////                    }

////                }
////                if (TotalLen !== null && !isNaN(TotalLen) && TotalLen > 0) {
////                    switch (this.data.LengthUnit.toUpperCase()) {
////                        case "IN":
////                            iRet = ngl.convertINtoLinearFeet(TotalLen);
////                            break;
////                        case "CM":
////                            iRet = ngl.convertCMtoLinearFeet(TotalLen);
////                            break;
////                        case "FT":
////                            iRet = TotalLen;
////                            break;
////                        case "M":
////                            iRet = ngl.convertMtoLinearFeet(TotalLen);
////                            break;
////                        default:
////                            iRet = 0;
////                    }
////                }
////            }

////        }
////        return iRet;
////    }

////    this.readSuccessCallback = function (data) {
////        //var windowWidget = $("#winDispatchDialog").data("kendoWindow");      
////        //kendo.ui.progress(windowWidget.element, false);
////        var oResults = new nglEventParameters();
////        var tObj = this;
////        oResults.source = "readSuccessCallback";
////        oResults.widget = tObj;
////        oResults.msg = 'Failed'; //set default to Failed   
////        //clear any old return data in rData
////        this.rData = null;
////        try {
////            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
////                if (typeof (data.Errors) !== 'undefined' && data.Errors != null) {
////                    oResults.error = new Error();
////                    oResults.error.name = "Read BOL Data Failure";
////                    oResults.error.message = data.Errors;
////                    ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
////                }
////                else {
////                    this.rData = data.Data;
////                    if (data.Data != null) {
////                        oResults.data = data.Data;
////                        oResults.datatype = "Dispatch";
////                        oResults.msg = "Success"
////                        this.data = data.Data[0];
////                        if (typeof (this.data.LoadTenderControl) !== 'undefined' && this.data.LoadTenderControl != null) {
////                            this.readAdditionalServices(this.data.LoadTenderControl);
////                        }
////                        //this.show();
////                    }
////                    else {
////                        oResults.error = new Error();
////                        oResults.error.name = "Invalid Request";
////                        oResults.error.message = "No Data available.";
////                        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
////                    }
////                }
////            } else {
////                oResults.msg = "Success but no data was returned. Please refresh your page and try again.";
////                ngl.showSuccessMsg(oResults.msg, tObj);
////            }
////        } catch (err) {
////            oResults.error = err
////        }
////        if (ngl.isFunction(this.onRead)) { this.onRead(oResults); }
////        if (oResults.msg == "Success") { this.show(); }
////    }

////    this.readAjaxErrorCallback = function (xhr, textStatus, error) {
////        //var windowWidget = $("#winDispatchDialog").data("kendoWindow");
////        //kendo.ui.progress(windowWidget.element, false);
////        var oResults = new nglEventParameters();
////        var tObj = this;
////        oResults.source = "readAjaxErrorCallback";
////        oResults.widget = tObj;
////        oResults.msg = 'Failed'; //set default to Failed  
////        oResults.error = new Error();
////        oResults.error.name = "Read BOL Data Failure"
////        oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
////        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);

////        if (ngl.isFunction(this.onRead)) {
////            this.onRead(oResults);
////        }
////    }

////    this.readAdditionalServicesSuccessCallback = function (data) {
////        //var windowWidget = $("#winDispatchDialog").data("kendoWindow");      
////        //kendo.ui.progress(windowWidget.element, false);
////        var oResults = new nglEventParameters();
////        var tObj = this;
////        oResults.source = "readAdditionalServicesSuccessCallback";
////        oResults.widget = tObj;
////        oResults.msg = 'Failed'; //set default to Failed   
////        //clear any old return data in rData
////        this.strAdditionalServices = "";
////        try {
////            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
////                if (typeof (data.Errors) !== 'undefined' && data.Errors != null) {
////                    oResults.error = new Error();
////                    oResults.error.name = "Read Additional Services Data Failure";
////                    oResults.error.message = data.Errors;
////                    ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
////                }
////                else {                   
////                    if (data.Data != null) {
////                        oResults.data = data.Data;
////                        oResults.datatype = "String";
////                        oResults.msg = "Success"
////                        this.strAdditionalServices = data.Data[0];
////                        $("#spBOLRptAdditionalServices").html(this.strAdditionalServices);
////                    }
////                    else {
////                        oResults.error = new Error();
////                        oResults.error.name = "Invalid Request";
////                        oResults.error.message = "No Data available.";
////                        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
////                    }
////                }
////            } else {
////                oResults.msg = "Success but no data was returned. Please refresh your page and try again.";
////                ngl.showSuccessMsg(oResults.msg, tObj);
////            }
////        } catch (err) {
////            oResults.error = err
////        }
////        if (ngl.isFunction(this.onRead)) {
////            this.onRead(oResults);
////        }

////    }

////    this.readAdditionalServicesAjaxErrorCallback = function (xhr, textStatus, error) {
////        //var windowWidget = $("#winDispatchDialog").data("kendoWindow");
////        //kendo.ui.progress(windowWidget.element, false);
////        var oResults = new nglEventParameters();
////        var tObj = this;
////        oResults.source = "readAdditionalServicesAjaxErrorCallback";
////        oResults.widget = tObj;
////        oResults.msg = 'Failed'; //set default to Failed  
////        oResults.error = new Error();
////        oResults.error.name = "Read Additional Services Data Failure"
////        oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
////        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
////        this.strAdditionalServices = "";
////        $("#spBOLRptAdditionalServices").html(this.strAdditionalServices);
////        if (ngl.isFunction(this.onRead)) {
////            this.onRead(oResults);
////        }
////    }

////    this.readAdditionalServices = function (intControl) {
////        var blnRet = false;
////        var tObj = this;
////        if (typeof (intControl) != 'undefined' && intControl != null && intControl !== 0) {
////            this.strAdditionalServices = "";
////            var oCRUDCtrl = new nglRESTCRUDCtrl();
////            blnRet = oCRUDCtrl.read("BOL/GetBOLAdditionalServices", intControl, tObj, "readAdditionalServicesSuccessCallback", "readAdditionalServicesAjaxErrorCallback");
////        }
////        return blnRet;
////    }

////    this.print = function () {
////        var kinWin = $("#winBOLRpt").kendoWindow().data("kendoWindow");
////        var winWrapper = kinWin.wrapper;
////        winWrapper.removeClass("k-window");
////        winWrapper.addClass("printable");
////        //put pring code here
////        window.print();
////        winWrapper.removeClass("printable");
////        winWrapper.addClass("k-window");

////    }

////    this.show = function () {
////        if (typeof (this.data) !== 'undefined' && ngl.isObject(this.data)) {
////            this.edit(this.data);
////        }

////        var tObj = this;
////        this.kendoWindowsObj = $("#winBOLRpt").kendoWindow({
////            title: "Bill of Lading Report",
////            modal: true,
////            visible: false,
////            height: '75%',
////            width: '75%',
////            scrollable: true,
////            actions: ["print", "Minimize", "Maximize", "Close"],
////            close: function (e) { tObj.close(e); }
////        }).data("kendoWindow");

////        if (this.kendoWindowsObjUploadEventAdded === 0) {
////            $("#winBOLRpt").data("kendoWindow").wrapper.find(".k-svg-i-print").parent().click(function (e) { e.preventDefault(); tObj.print(); });
////        }
////        this.kendoWindowsObjUploadEventAdded = 1;

////        this.kendoWindowsObj.center().open();
////    }

////    this.read = function (intControl) {
////        var blnRet = false;
////        var tObj = this;
////        if (typeof (intControl) != 'undefined' && intControl != null) {
////            this.rData = null;
////            this.data = null;
////            var oCRUDCtrl = new nglRESTCRUDCtrl();
////            blnRet = oCRUDCtrl.read("BOL/GetBOL", intControl, tObj, "readSuccessCallback", "readAjaxErrorCallback");

////        }
////        return blnRet;
////    }

////    this.readByBookControl = function (intBookControl) {
////        var blnRet = false;
////        var tObj = this;
////        if (typeof (intBookControl) != 'undefined' && intBookControl != null) {
////            this.rData = null;
////            this.data = null;
////            var oCRUDCtrl = new nglRESTCRUDCtrl();
////            blnRet = oCRUDCtrl.read("BOL/Get", intBookControl, tObj, "readSuccessCallback", "readAjaxErrorCallback");
////            //if (blnRet) {
////            //    this.readAdditionalServices(intControl);
////            //}
////        }
////        return blnRet;
////    }

////    this.edit = function (data) {
////        //load data to the screen
////        var tObj = this;
////        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
////            ////if (typeof (data.BidControl) === 'undefined' || data.BidControl === null || data.BidControl === 0) { ngl.showValidationMsg("Cannot Open BOL", "The selected Booking record does not have a valid load tender bid record. Please re-tender the load and try again.", tPage); return; }
////            $("#spBOLRptSHID").html(data.SHID);
////            $("#spBOLRptBOL").html(data.BillOfLading);
////            $("#spBOLRptCarrier").html(data.ProviderSCAC);
////            $("#spBOLRptPickupDate").html(ngl.getShortDateString(data.PickupDate));

////            $("#spBOLRptOrigName").html(data.Origin.Name);
////            $("#spBOLRptOrigAddress1").html(data.Origin.Address1);
////            $("#spBOLRptOrigCity").html(data.Origin.City );
////            $("#spBOLRptOrigState").html(data.Origin.State);
////            $("#spBOLRptOrigZip").html(data.Origin.Zip);
////            $("#spBOLRptOrigContactName").html(data.Origin.Contact.ContactName);
////            $("#spBOLRptOrigPhone").html(data.Origin.Contact.ContactPhone);
////            $("#spBOLRptOrigEmail").html(data.Origin.Contact.ContactEmail);
////            $("#spBOLRptDeliveryDate").html(ngl.getShortDateString(data.DeliveryDate));

////            $("#spBOLRptDestName").html(data.Destination.Name);
////            $("#spBOLRptDestAddress1").html(data.Destination.Address1);
////            $("#spBOLRptDestCity").html(data.Destination.City);
////            $("#spBOLRptDestState").html(data.Destination.State);
////            $("#spBOLRptDestZip").html(data.Destination.Zip);
////            $("#spBOLRptDestContactName").html(data.Destination.Contact.ContactName);
////            $("#spBOLRptDestPhone").html(data.Destination.Contact.ContactPhone);
////            $("#spBOLRptDestEmail").html(data.Destination.Contact.ContactEmail);

////            var shipWgt = $("#spBOLRptShipWgt").html(this.data.TotalWgt);
////            var shipQty = $("#spBOLRptShipQty").html(this.data.TotalQty);
////            var shipPlts = $("#spBOLRptShipPlts").html(this.data.TotalPlts);
////            var shipCubes = $("#spBOLRptShipCubes").html(this.data.TotalCube);
////            var strInstructions = "";
////            if (ngl.isNullOrWhitespace(data.PickupNote) === false) { strInstructions =  "<b>Pickup:</b>&nbsp;" + data.PickupNote + "&nbsp;&nbsp;";}
////            if (ngl.isNullOrWhitespace(data.DeliveryNote) === false) { strInstructions = strInstructions + " <b>Delivery:</b>&nbsp; " + data.DeliveryNote; }
////            $("#spBOLRptSpecialInstructions").html(strInstructions);

////            $("#spBOLRptQuoteNumber").html(data.QuoteNumber);            
////            $("#spBOLRptConfNbr").html(data.PickupNumber);
////            $("#spBOLRptPONumber").html(data.PONumber);
////            $("#spBOLRptOrderNumber").html(data.OrderNumber);
////            $("#spBOLRptCarrierProNumber").html(data.CarrierProNumber);

////            $("#spBOLRptBillToName").html(data.Requestor.Name);
////            $("#spBOLRptBillToAddress1").html(data.Requestor.Address1);
////            $("#spBOLRptBillToCity").html(data.Requestor.City);
////            $("#spBOLRptBillToState").html(data.Requestor.State);
////            $("#spBOLRptBillToZip").html(data.Requestor.Zip);
////            $("#spBOLRptBillToPhone").html(data.Requestor.Contact.ContactPhone);

////            $("#spBOLRptBillingType").html("Prepaid");
////            $("#spBOLRptLegal").html("<small>" + data.BOLLegalText + "</small>");
////            var arrItems = data.Items;
////            var OrderItemsDataSource = new kendo.data.DataSource({
////                data: arrItems,
////                schema: {
////                    model: {
////                        id: "ItemNumber",
////                        fields: {
////                            "ItemNumber": { type: "string", visible: true, editable: false, nullable: true },
////                            "ItemWgt": { type: "number", defaultValue: 1 },
////                            "ItemFreightClass": { type: "string", defaultValue: "100", editable: false, nullable: true },
////                            "ItemPackageType": { type: "string", defaultValue: "PLT", editable: false, nullable: true },
////                            "ItemTotalPackages": { type: "number", defaultValue: 1 },
////                            "ItemPieces": { type: "number", defaultValue: 1 },
////                            "ItemDesc": { type: "string", editable: true, nullable: true },
////                            "ItemDimensions": { type: "string" },
////                            "ItemNMFCItemCode": { type: "string", editable: false, nullable: true },
////                            "ItemNMFCSubCode": { type: "string", editable: false, nullable: true },
////                            "ItemCube": { type: "number", defaultValue: 1 }
////                        }
////                    }
////                },
////                aggregate: [
////                    { field: "ItemPieces", aggregate: "sum" },
////                    { field: "ItemTotalPackages", aggregate: "sum" },
////                    { field: "ItemWgt", aggregate: "sum" },
////                    { field: "ItemCube", aggregate: "sum" }
////                ]
////            });

////            var ItemsGrid = $("#divBOLRptItemDetails").kendoGrid({
////                scrollable: false,
////                columns: [
////                    { field: "ItemPieces", title: "Qty", headerAttributes: { "class": "table-header-cell", style: "font-weight: bold;" }, footerTemplate: "#: sum #", width: 20 },
////                    { field: "ItemPackageType", title: "Pkg", headerAttributes: { "class": "table-header-cell", style: "font-weight: bold;" }, template: "#: ItemTotalPackages # #: ItemPackageType #", footerTemplate: "#= kendo.toString(data.ItemTotalPackages.sum)#", width: 35 },
////                    { field: "ItemDesc", title: "Description", headerAttributes: { "class": "table-header-cell", style: "font-weight: bold;" }, width: 80 },
////                    { field: "ItemNumber", title: "Ref", headerAttributes: { "class": "table-header-cell", style: "font-weight: bold;" }, width: 35 },
////                    { field: "ItemDimensions", title: "Dims (lwh)", headerAttributes: { "class": "table-header-cell", style: "font-weight: bold;" }, width: 50 },
////                    { field: "ItemWgt", title: "Wgt", headerAttributes: { "class": "table-header-cell", style: "font-weight: bold;" }, footerTemplate: "#: sum #", width: 35 },
////                    { field: "ItemCube", title: "Cube", headerAttributes: { "class": "table-header-cell", style: "font-weight: bold;" }, footerTemplate: "#: sum #", width: 35 },
////                    { field: "ItemFreightClass", title: "FAK", headerAttributes: { "class": "table-header-cell", style: "font-weight: bold;" }, width: 25 },
////                    { field: "ItemNMFCItemCode", title: "NMFC", headerAttributes: { "class": "table-header-cell", style: "font-weight: bold;" }, width: 35 },
////                    { field: "ItemNMFCSubCode", title: "Sub", headerAttributes: { "class": "table-header-cell", style: "font-weight: bold;" }, width: 25 }
////                ],
////                dataSource: OrderItemsDataSource,
////            });

////            $('#divBOLRptItemDetails').data('kendoGrid').dataSource.read();
////        }
////    }

////    //Note:  this method never gets called because we do not have a callback method for the kendoWindow
////    this.close = function (e) {
////        //var oResults = new nglEventParameters();
////        //oResults.source = "close";
////        //oResults.widget = this;
////        //oResults.msg = 'closing nothing is saved';

////        //if (typeof (this.onClose) === "function") {
////        //    this.onClose(oResults);
////        //}
////    }

////    //loadDefaults sets up the callbacks cbSelect and cbSave
////    //all call backs return a reference to this object and a string message as parameters
////    this.loadDefaults = function (pageVariable, selectCallBack, saveCallBack, closeCallBack, readCallBack) {

////        this.onSelect = selectCallBack;
////        this.onSave = saveCallBack;
////        this.onClose = closeCallBack;
////        this.onRead = readCallBack;
////        this.data = new Dispatch();
////        this.lastErr = "";
////        this.notifyOnError = true;
////        this.notifyOnSuccess = true;
////        this.notifyOnValidationFailure = true;
////        var tObj = this;

////        if (typeof (pageVariable) !== 'undefined' && ngl.isObject(pageVariable)) {
////            //this is the add new popup window
////            this.kendoWindowsObj = pageVariable;
////        } else {
////            this.kendoWindowsObj = null;
////        }

////    }
////}

//NEW
function BOLReportCtrl() {
    //Generic properties for all Widgets
    this.lastErr = "";
    this.notifyOnError = true;
    this.notifyOnSuccess = true;
    this.notifyOnValidationFailure = true;
    data: Dispatch();
    rData: null;
    onSelect: null;
    onSave: null;
    onClose: null;
    onRead: null;
    dSource: null;
    sourceDiv: null;
    kendoWindowsObj: null;
    strAdditionalServices: "";
    //Widget specific properties

    this.kendoWindowsObjUploadEventAdded = 0;

    //Widget specific functions
    this.calcLinearFt = function () {
        var iRet = 0;
        if (typeof (this.data) !== 'undefined' && ngl.isObject(this.data)) {
            if (typeof (this.data.Items) !== 'undefined' && ngl.isArray(this.data.Items) && this.data.Items.length > 0) {
                var TotalLen = 0;
                for (i = 0, len = this.data.Items.length; i < len; i++) {
                    var oItem = this.data.Items[i];
                    var iLen = this.data.Items[i].ItemLength
                    if (typeof (iLen) !== 'undefined' && iLen !== null && !isNaN(iLen) && iLen > 0) {
                        TotalLen += iLen;
                    }
                }
                if (TotalLen !== null && !isNaN(TotalLen) && TotalLen > 0) {
                    switch (this.data.LengthUnit.toUpperCase()) {
                        case "IN":
                            iRet = ngl.convertINtoLinearFeet(TotalLen);
                            break;
                        case "CM":
                            iRet = ngl.convertCMtoLinearFeet(TotalLen);
                            break;
                        case "FT":
                            iRet = TotalLen;
                            break;
                        case "M":
                            iRet = ngl.convertMtoLinearFeet(TotalLen);
                            break;
                        default:
                            iRet = 0;
                    }
                }
            }
        }
        return iRet;
    }

    this.readSuccessCallback = function (data) {
        //var windowWidget = $("#winDispatchDialog").data("kendoWindow");      
        //kendo.ui.progress(windowWidget.element, false);
        var oResults = new nglEventParameters();
        var tObj = this;
        oResults.source = "readSuccessCallback";
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed   
        //clear any old return data in rData
        this.rData = null;
        try {
            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                if (typeof (data.Errors) !== 'undefined' && data.Errors != null) {
                    oResults.error = new Error();
                    oResults.error.name = "Read BOL Data Failure";
                    oResults.error.message = data.Errors;
                    ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                }
                else {
                    this.rData = data.Data;
                    if (data.Data != null) {
                        oResults.data = data.Data;
                        oResults.datatype = "Dispatch";
                        oResults.msg = "Success"
                        this.data = data.Data;
                        if (typeof (this.data[0].BookControl) !== 'undefined' && this.data[0].BookControl != null) {
                            this.readAdditionalServices(this.data[0].BookControl);
                        }
                    }
                    else {
                        oResults.error = new Error();
                        oResults.error.name = "Invalid Request";
                        oResults.error.message = "No Data available.";
                        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                    }
                }
            } else { oResults.msg = "Success but no data was returned. Please refresh your page and try again."; ngl.showSuccessMsg(oResults.msg, tObj); }
        } catch (err) { oResults.error = err; }
        if (ngl.isFunction(this.onRead)) { this.onRead(oResults); }
        if (oResults.msg == "Success") { this.show(); }
    }

    this.readAjaxErrorCallback = function (xhr, textStatus, error) {
        //var windowWidget = $("#winDispatchDialog").data("kendoWindow");
        //kendo.ui.progress(windowWidget.element, false);
        var oResults = new nglEventParameters();
        var tObj = this;
        oResults.source = "readAjaxErrorCallback";
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed  
        oResults.error = new Error();
        oResults.error.name = "Read BOL Data Failure"
        oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
        if (ngl.isFunction(this.onRead)) { this.onRead(oResults); }
    }

    this.readAdditionalServicesSuccessCallback = function (data) {
        //var windowWidget = $("#winDispatchDialog").data("kendoWindow");      
        //kendo.ui.progress(windowWidget.element, false);
        var oResults = new nglEventParameters();
        var tObj = this;
        oResults.source = "readAdditionalServicesSuccessCallback";
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed   
        //clear any old return data in rData
        this.strAdditionalServices = "";
        try {
            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                if (typeof (data.Errors) !== 'undefined' && data.Errors != null) {
                    oResults.error = new Error();
                    oResults.error.name = "Read Additional Services Data Failure";
                    oResults.error.message = data.Errors;
                    ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                }
                else {
                    if (data.Data != null) {
                        oResults.data = data.Data;
                        oResults.datatype = "String";
                        oResults.msg = "Success"
                        this.strAdditionalServices = data.Data[0];
                        $("#spBOLRptAdditionalServices").html(this.strAdditionalServices);
                    }
                    else {
                        oResults.error = new Error();
                        oResults.error.name = "Invalid Request";
                        oResults.error.message = "No Data available.";
                        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                    }
                }
            } else { oResults.msg = "Success but no data was returned. Please refresh your page and try again."; ngl.showSuccessMsg(oResults.msg, tObj); }
        } catch (err) { oResults.error = err }
        if (ngl.isFunction(this.onRead)) { this.onRead(oResults); }
    }

    this.readAdditionalServicesAjaxErrorCallback = function (xhr, textStatus, error) {
        //var windowWidget = $("#winDispatchDialog").data("kendoWindow");
        //kendo.ui.progress(windowWidget.element, false);
        var oResults = new nglEventParameters();
        var tObj = this;
        oResults.source = "readAdditionalServicesAjaxErrorCallback";
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed  
        oResults.error = new Error();
        oResults.error.name = "Read Additional Services Data Failure"
        oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
        this.strAdditionalServices = "";
        $("#spBOLRptAdditionalServices").html(this.strAdditionalServices);
        if (ngl.isFunction(this.onRead)) { this.onRead(oResults); }
    }

    this.readAdditionalServices = function (intControl) {
        var blnRet = false;
        var tObj = this;
        if (typeof (intControl) != 'undefined' && intControl != null && intControl !== 0) {
            this.strAdditionalServices = "";
            var oCRUDCtrl = new nglRESTCRUDCtrl();
            blnRet = oCRUDCtrl.read("BOL/GetBOLAdditionalServices", intControl, tObj, "readAdditionalServicesSuccessCallback", "readAdditionalServicesAjaxErrorCallback");
        }
        return blnRet;
    }

    this.print = function () {
        var kinWin = $("#winBOLRpt").kendoWindow().data("kendoWindow");
        var winWrapper = kinWin.wrapper;
        winWrapper.removeClass("k-window");
        winWrapper.addClass("printable");
        //put pring code here
        window.print();
        winWrapper.removeClass("printable");
        winWrapper.addClass("k-window");
    }

    this.email = function () {
        var docTitle = "BOL-" + this.data[0].SHID + ".pdf"
        var tObj = this;

        ////kendo.drawing.drawDOM("#winBOLRpt", { paperSize: "A4", forcePageBreak: ".pgbrk" }).then(function (group) { kendo.drawing.pdf.saveAs(group, docTitle) });

        var draw = kendo.drawing;
        draw.drawDOM($("#winBOLRpt")).then(function (root) {
            return draw.exportPDF(root, { paperSize: "A4", forcePageBreak: ".pgbrk" }); // Chaining the promise via then
        }).done(function (dataURI) {
            //Extracting the base64-encoded string and the contentType
            var data = {};
            var parts = dataURI.split(";base64,");
            //data.contentType = parts[0].replace("data:", "");
            //data.base64 = parts[1];

            var gr = new GenericResult();
            gr.strField = parts[0].replace("data:", "");
            gr.strField2 = parts[1];
            var oCRUDCtrl = new nglRESTCRUDCtrl();
            oCRUDCtrl.update("BOL/EmailBOL", gr, tObj, "emailBOLSuccessCallback", "emailBOLAjaxErrorCallback");

            ////var blob;
            ////var BASE64_MARKER = ';base64,';
            ////if (dataURI.indexOf(BASE64_MARKER) == -1) {
            ////    var parts = dataURI.split(',');
            ////    var contentType = parts[0].split(':')[1];
            ////    var raw = decodeURIComponent(parts[1]);
            ////    blob = new Blob([raw], { type: contentType });
            ////}
            ////else {
            ////    var parts = dataURI.split(BASE64_MARKER);
            ////    var contentType = parts[0].split(':')[1];
            ////    var raw = window.atob(parts[1]);
            ////    var rawLength = raw.length;
            ////    var uInt8Array = new Uint8Array(rawLength);
            ////    for (var i = 0; i < rawLength; ++i) {
            ////        uInt8Array[i] = raw.charCodeAt(i);
            ////    }
            ////    blob = new Blob([uInt8Array], { type: contentType });
            ////}

        });
    }

    this.emailBOLSuccessCallback = function (data) {
        //var oResults = new nglEventParameters();
        var tObj = this;
        try {
            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                if (ngl.stringHasValue(data.Errors)) { ngl.showErrMsg("Email BOL Data Failure", data.Errors, tObj); }
                else {
                    if (typeof (data.Data) !== 'undefined' && data.Data != null && ngl.isArray(data.Data)) {
                        if (ngl.stringHasValue(data.Data[0])) { ngl.showSuccessMsg(data.Data[0], tObj); }
                    }
                    else { ngl.showErrMsg("Invalid Request", "No Data available.", tObj); }
                }
            } else { ngl.showSuccessMsg("Success but no data was returned. Please refresh your page and try again.", tObj); }
        } catch (err) { ngl.showErrMsg(err.name, err.message, null); }
    }

    this.emailBOLAjaxErrorCallback = function (xhr, textStatus, error) {
        var oResults = new nglEventParameters();
        var tObj = this;
        oResults.source = "emailBOLAjaxErrorCallback";
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed  
        oResults.error = new Error();
        oResults.error.name = "Email BOL Data Failure"
        oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
    }

    this.show = function () {
        if (typeof (this.data) !== 'undefined' && ngl.isObject(this.data)) {
            this.edit(this.data);
        }

        var tObj = this;
        this.kendoWindowsObj = $("#winBOLRpt").kendoWindow({
            title: "Bill of Lading Report",
            modal: true,
            visible: false,
            height: '75%',
            width: '75%',
            scrollable: true,
            //actions: ["email","print", "Minimize", "Maximize", "Close"],
            actions: ["print", "Minimize", "Maximize", "Close"],
            close: function (e) { tObj.close(e); }
        }).data("kendoWindow");

        if (this.kendoWindowsObjUploadEventAdded === 0) {
            $("#winBOLRpt").data("kendoWindow").wrapper.find(".k-svg-i-print").parent().click(function (e) { e.preventDefault(); tObj.print(); });
            //$("#winBOLRpt").data("kendoWindow").wrapper.find(".k-svg-i-envelope").parent().click(function (e) { e.preventDefault(); tObj.email(); });
        }
        this.kendoWindowsObjUploadEventAdded = 1;

        this.kendoWindowsObj.center().open();
    }

    this.read = function (intControl) {
        var blnRet = false;
        var tObj = this;
        if (typeof (intControl) != 'undefined' && intControl != null) {
            this.rData = null;
            this.data = null;
            var oCRUDCtrl = new nglRESTCRUDCtrl();
            blnRet = oCRUDCtrl.read("BOL/GetBOL", intControl, tObj, "readSuccessCallback", "readAjaxErrorCallback");
        }
        return blnRet;
    }

    this.readByBookControl = function (intBookControl) {
        var blnRet = false;
        var tObj = this;
        if (typeof (intBookControl) != 'undefined' && intBookControl != null) {
            this.rData = null;
            this.data = null;
            var oCRUDCtrl = new nglRESTCRUDCtrl();
            blnRet = oCRUDCtrl.read("BOL/Get", intBookControl, tObj, "readSuccessCallback", "readAjaxErrorCallback");
        }
        return blnRet;
    }

    this.fixIds = function (elem, cntr) {
        $(elem).find("[id]").add(elem).each(function () {
            this.id = this.id + cntr;
        });
    }

    this.edit = function (data) {
        //load data to the screen
        var tObj = this;

        $("#divBOLRpt").show();
        $("#divErrMsgBOLRpt").hide();
        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
            if (typeof (this.data) !== 'undefined' && ngl.isArray(this.data) && this.data.length > 0) {

                if (typeof (data[0]) === 'undefined' || !ngl.isObject(data[0])) { $("#divBOLRpt").hide(); $("#divErrMsgBOLRpt").show(); return; }
                if (data[0].BookControl === 0 && data[0].LoadTenderControl === 0 && !ngl.stringHasValue(data[0].ProviderSCAC)) { $("#divBOLRpt").hide(); $("#divErrMsgBOLRpt").show(); return; }
                for (i = 1; i < 100; i++) {
                    var sID = "divBOLRpt" + i.toString();
                    var sPbrID = "pgbr" + i.toString();
                    if ($("#" + sID).length) {
                        $("#" + sID).remove();
                        if ($("#" + sPbrID).length) {
                            $("#" + sPbrID).remove();
                        }
                    } else {
                        break;
                    }
                }


                var strIx = "";
                for (i = 0; i < data.length; i++) {
                    if (typeof (data[i]) !== 'undefined' && ngl.isObject(data[i])) {

                        if (i > 0) {
                            var clone = $("#divBOLRpt").clone(true, true);
                            this.fixIds(clone, i);
                            clone.insertAfter("#pgbr" + strIx); //must be first
                            strIx = i.toString(); //after intsertAfter
                            if (i != (data.length - 1)) {
                                $("<h1 id='pgbr" + strIx + "' class='pgbrk'></h1>").insertAfter("#divBOLRpt" + strIx); //must be last
                            }
                        }

                        $("#spBOLRptSHID" + strIx).html(data[i].SHID);
                        $("#spBOLRptBOL" + strIx).html(data[i].BillOfLading);
                        $("#spBOLRptCarrierName" + strIx).html(data[i].CarrierName);
                        $("#spBOLRptCarrierSCAC" + strIx).html(data[i].ProviderSCAC);
                        $("#spBOLRptPickupDate" + strIx).html(ngl.getShortDateString(data[i].PickupDate));
                        $("#spBOLRptOrigName" + strIx).html(data[i].Origin.Name);
                        $("#spBOLRptOrigAddress1" + strIx).html(data[i].Origin.Address1);
                        $("#spBOLRptOrigCity" + strIx).html(data[i].Origin.City);
                        $("#spBOLRptOrigState" + strIx).html(data[i].Origin.State);
                        $("#spBOLRptOrigZip" + strIx).html(data[i].Origin.Zip);
                        $("#spBOLRptOrigContactName" + strIx).html(data[i].Origin.Contact.ContactName);
                        $("#spBOLRptOrigPhone" + strIx).html(ngl.formatPhoneNumber(data[i].Origin.Contact.ContactPhone));
                        $("#spBOLRptOrigEmail" + strIx).html(data[i].Origin.Contact.ContactEmail);
                        $("#spBOLRptDeliveryDate" + strIx).html(ngl.getShortDateString(data[i].DeliveryDate));
                        $("#spBOLRptDestName" + strIx).html(data[i].Destination.Name);
                        $("#spBOLRptDestAddress1" + strIx).html(data[i].Destination.Address1);
                        $("#spBOLRptDestCity" + strIx).html(data[i].Destination.City);
                        $("#spBOLRptDestState" + strIx).html(data[i].Destination.State);
                        $("#spBOLRptDestZip" + strIx).html(data[i].Destination.Zip);
                        $("#spBOLRptDestContactName" + strIx).html(data[i].Destination.Contact.ContactName);
                        $("#spBOLRptDestPhone" + strIx).html(ngl.formatPhoneNumber(data[i].Destination.Contact.ContactPhone));
                        $("#spBOLRptDestEmail" + strIx).html(data[i].Destination.Contact.ContactEmail);
                        $("#spBOLRptShipWgt" + strIx).html(data[i].TotalWgt);
                        $("#spBOLRptShipQty" + strIx).html(data[i].TotalQty);
                        $("#spBOLRptShipPlts" + strIx).html(data[i].TotalPlts);
                        $("#spBOLRptShipCubes" + strIx).html(data[i].TotalCube);
                        var strInstructions = "";
                        if (ngl.isNullOrWhitespace(data[i].PickupNote) === false) { strInstructions = "<b>Pickup:</b>&nbsp;" + data[i].PickupNote + "&nbsp;&nbsp;"; }
                        if (ngl.isNullOrWhitespace(data[i].DeliveryNote) === false) { strInstructions = strInstructions + " <b>Delivery:</b>&nbsp; " + data[i].DeliveryNote; }
                        $("#spBOLRptSpecialInstructions" + strIx).html(strInstructions);
                        $("#spBOLRptQuoteNumber" + strIx).html(data[i].QuoteNumber);
                        $("#spBOLRptConfNbr" + strIx).html(data[i].PickupNumber);
                        $("#spBOLRptPONumber" + strIx).html(data[i].PONumber);
                        $("#spBOLRptOrderNumber" + strIx).html(data[i].OrderNumber);
                        $("#spBOLRptCarrierProNumber" + strIx).html(data[i].CarrierProNumber);
                        $("#spBOLRptItemOrderNumbers" + strIx).html(data[i].ItemOrderNumbers);
                        $("#spBOLRptBillToName" + strIx).html(data[i].Requestor.Name);
                        $("#spBOLRptBillToAddress1" + strIx).html(data[i].Requestor.Address1);
                        $("#spBOLRptBillToCity" + strIx).html(data[i].Requestor.City);
                        $("#spBOLRptBillToState" + strIx).html(data[i].Requestor.State);
                        $("#spBOLRptBillToZip" + strIx).html(data[i].Requestor.Zip);
                        $("#spBOLRptBillToPhone" + strIx).html(ngl.formatPhoneNumber(data[i].Requestor.Contact.ContactPhone));
                        $("#spBOLRptBillingType" + strIx).html("Prepaid");
                        $("#spBOLRptLegal" + strIx).html("<small>" + data[i].BOLLegalText + "</small>");
                        var arrItems = data[i].Items;

                        $("#divBOLRptItemDetails" + strIx).kendoGrid({
                            dataSource: {
                                data: arrItems,
                                schema: {
                                    model: {
                                        id: "ItemNumber",
                                        fields: {
                                            "ItemNumber": { type: "string", visible: true, editable: false, nullable: true },
                                            "ItemWgt": { type: "number", defaultValue: 1 },
                                            "ItemFreightClass": { type: "string", defaultValue: "100", editable: false, nullable: true },
                                            "ItemPackageType": { type: "string", defaultValue: "PLT", editable: false, nullable: true },
                                            "ItemTotalPackages": { type: "number", defaultValue: 1 },
                                            "ItemPieces": { type: "number", defaultValue: 1 },
                                            "ItemDesc": { type: "string", editable: true, nullable: true },
                                            "ItemDimensions": { type: "string" },
                                            "ItemNMFCItemCode": { type: "string", editable: false, nullable: true },
                                            "ItemNMFCSubCode": { type: "string", editable: false, nullable: true },
                                            "ItemCube": { type: "number", defaultValue: 1 }
                                        }
                                    }
                                },
                                aggregate: [
                                    { field: "ItemPieces", aggregate: "sum" },
                                    { field: "ItemTotalPackages", aggregate: "sum" },
                                    { field: "ItemWgt", aggregate: "sum" },
                                    { field: "ItemCube", aggregate: "sum" }
                                ]
                            },
                            scrollable: false,
                            columns: [
                                { field: "ItemDesc", title: "Description", headerAttributes: { "class": "table-header-cell", style: "font-weight: bold;" }, footerTemplate: "Totals:", width: 80 },
                                { field: "ItemPieces", title: "Qty", headerAttributes: { "class": "table-header-cell", style: "font-weight: bold;" }, footerTemplate: "#: sum #", width: 20 },
                                { field: "ItemPackageType", title: "Pkg", headerAttributes: { "class": "table-header-cell", style: "font-weight: bold;" }, template: "#: ItemTotalPackages # #: ItemPackageType #", footerTemplate: "#= kendo.toString(data.ItemTotalPackages.sum)#", width: 35 },
                                { field: "ItemWgt", title: "Wgt", headerAttributes: { "class": "table-header-cell", style: "font-weight: bold;" }, footerTemplate: "#: sum #", width: 35 },
                                { field: "ItemCube", title: "Cube", headerAttributes: { "class": "table-header-cell", style: "font-weight: bold;" }, footerTemplate: "#: sum #", width: 35 },
                                { field: "ItemNumber", title: "Ref", headerAttributes: { "class": "table-header-cell", style: "font-weight: bold;" }, width: 35 },
                                { field: "ItemDimensions", title: "Dims (lwh)", headerAttributes: { "class": "table-header-cell", style: "font-weight: bold;" }, width: 50 },
                                { field: "ItemFreightClass", title: "FAK", headerAttributes: { "class": "table-header-cell", style: "font-weight: bold;" }, width: 25 },
                                { field: "ItemNMFCItemCode", title: "NMFC", headerAttributes: { "class": "table-header-cell", style: "font-weight: bold;" }, width: 35 },
                                { field: "ItemNMFCSubCode", title: "Sub", headerAttributes: { "class": "table-header-cell", style: "font-weight: bold;" }, width: 25 }
                                ////{ field: "ItemPieces", title: "Qty", headerAttributes: { "class": "table-header-cell", style: "font-weight: bold;" }, footerTemplate: "#: sum #", width: 20 },
                                ////{ field: "ItemPackageType", title: "Pkg", headerAttributes: { "class": "table-header-cell", style: "font-weight: bold;" }, template: "#: ItemTotalPackages # #: ItemPackageType #", footerTemplate: "#= kendo.toString(data.ItemTotalPackages.sum)#", width: 35 },
                                ////{ field: "ItemDesc", title: "Description", headerAttributes: { "class": "table-header-cell", style: "font-weight: bold;" }, width: 80 },
                                ////{ field: "ItemNumber", title: "Ref", headerAttributes: { "class": "table-header-cell", style: "font-weight: bold;" }, width: 35 },
                                ////{ field: "ItemDimensions", title: "Dims (lwh)", headerAttributes: { "class": "table-header-cell", style: "font-weight: bold;" }, width: 50 },
                                ////{ field: "ItemWgt", title: "Wgt", headerAttributes: { "class": "table-header-cell", style: "font-weight: bold;" }, footerTemplate: "#: sum #", width: 35 },
                                ////{ field: "ItemCube", title: "Cube", headerAttributes: { "class": "table-header-cell", style: "font-weight: bold;" }, footerTemplate: "#: sum #", width: 35 },
                                ////{ field: "ItemFreightClass", title: "FAK", headerAttributes: { "class": "table-header-cell", style: "font-weight: bold;" }, width: 25 },
                                ////{ field: "ItemNMFCItemCode", title: "NMFC", headerAttributes: { "class": "table-header-cell", style: "font-weight: bold;" }, width: 35 },
                                ////{ field: "ItemNMFCSubCode", title: "Sub", headerAttributes: { "class": "table-header-cell", style: "font-weight: bold;" }, width: 25 }
                            ]
                        });

                        $('#divBOLRptItemDetails' + strIx).data('kendoGrid').dataSource.read();

                        var strAccessorials = "";
                        var arrAccessorials = data[i].Accessorials;
                        if (ngl.isArray(arrAccessorials)) {
                            var sSep = "";
                            for (j = 0; j < arrAccessorials.length; j++) {
                                strAccessorials += (sSep + arrAccessorials[j]);
                                sSep = ", ";
                            }
                        }
                        if (!ngl.stringHasValue(strAccessorials)) { strAccessorials = "None"; }
                        //this.strAdditionalServices = data.Data[0];
                        $("#spBOLRptAdditionalServices" + strIx).html(strAccessorials);
                    }
                }
            } else { $("#divBOLRpt").hide(); $("#divErrMsgBOLRpt").show(); }
        } else { $("#divBOLRpt").hide(); $("#divErrMsgBOLRpt").show(); }
    }

    //Note:  this method never gets called because we do not have a callback method for the kendoWindow
    this.close = function (e) {
        //var oResults = new nglEventParameters();
        //oResults.source = "close";
        //oResults.widget = this;
        //oResults.msg = 'closing nothing is saved';
        //if (typeof (this.onClose) === "function") { this.onClose(oResults); }
    }

    //loadDefaults sets up the callbacks cbSelect and cbSave
    //all call backs return a reference to this object and a string message as parameters
    this.loadDefaults = function (pageVariable, selectCallBack, saveCallBack, closeCallBack, readCallBack) {
        this.onSelect = selectCallBack;
        this.onSave = saveCallBack;
        this.onClose = closeCallBack;
        this.onRead = readCallBack;
        this.data = new Dispatch();
        this.lastErr = "";
        this.notifyOnError = true;
        this.notifyOnSuccess = true;
        this.notifyOnValidationFailure = true;
        var tObj = this;

        if (typeof (pageVariable) !== 'undefined' && ngl.isObject(pageVariable)) {
            //this is the add new popup window
            this.kendoWindowsObj = pageVariable;
        } else { this.kendoWindowsObj = null; }
    }
}

//NGL Widget for selecting DataSource Filters
function AllFiltersCtrl(saveFilters, gridApiRef) {
    //Generic properties for all Widgets
    this.lastErr = "";
    this.notifyOnError = true;
    this.notifyOnSuccess = true;
    this.notifyOnValidationFailure = true;
    data: AllFilter;
    rData: null;
    onSelect: null;
    onSave: null;
    onClose: null;
    onRead: null;
    dSource: null;
    sourceDiv: null;
    //Widget specific properties
    iFilterID: 0;
    IDKey: "id1234";
    Theme: "blueopal";
    FilterData: FilterDetails;  //list of available filters
    ContainerDivID: "";
    this.ActionsCaption = "Actions";
    this.FilterIDCaption = "ID";
    this.FilterCaption = "Filter";
    this.FilterNameCaption = "Name";
    this.FilterValueFromCaption = "Val From";
    this.FilterValueToCaption = "Val To";
    this.FilterFromCaption = "Date From";
    this.FilterToCaption = "Date To";
    this.FilterFastTabCaption = "Filters"

    //Widget specific functions

    this.getFilterIndexByID = function (ID) {
        if (typeof (this.data) === 'undefined' || ngl.isObject(this.data) === false) { return null; };
        if (typeof (this.data.FilterValues) === 'undefined' || ngl.isArray(this.data.FilterValues) === false) { return null; };
        if (typeof (ID) === 'undefined' || ID === null) { return null; };
        var idx,
            l = this.data.FilterValues.length;
        for (var j = 0; j < l; j++) {
            if (this.data.FilterValues[j].filterID == ID) {
                return j;
                break;
            }
        }
        return null;
    };

    //uses the filterName of the available filters to lookup the filterIsDate property
    //the default value is false;
    this.isFilterADate = function (sName) {
        if (typeof (this.FilterData) === 'undefined' || ngl.isArray(this.FilterData) === false) { return false; };
        if (typeof (sName) === 'undefined' || sName === null) { return false; };
        var idx,
            l = this.FilterData.length;
        for (var j = 0; j < l; j++) {
            if (this.FilterData[j].filterName == sName) {

                return this.FilterData[j].filterIsDate;
                break;
            }
        }
        return false;
    };

    this.pushToAllFilters = function (f) {
       
        var tObj = this;
        if (typeof (f) !== 'undefined' && ngl.isObject(f)) {
            if (typeof (this.data) === 'undefined' || ngl.isObject(this.data) === false) {
                this.data = new AllFilter();
            };
            var fValues = this.data.FilterValues;
            if (typeof (fValues) === 'undefined' || ngl.isArray(fValues) === false) {
                fValues = new Array();
            }
            fValues.push(f);            
            this.data.FilterValues = fValues;
            //tObj.data = new AllFilter();
            //tObj.data.FilterValues = fValues;
            //console.log("tObj.data.FilterValues:")
            //console.log(tObj.data.FilterValues);
            $("#sp" + tObj.IDKey + "filterButtons").show();
            // Modified by RHR for v-8.5.4.004 on 12/19/2023 fixed bug in filter grid
            //  where old rows were not removed by when updating the data source.
            var fooGrid = $("#grd" + this.IDKey + "filters");
            if (fooGrid) { $("#grd" + this.IDKey + "filters").empty(); }
            var oFilterGrid = $("#grd" + this.IDKey + "filters").kendoGrid({
                theme: tObj.Theme,
                dataSource: tObj.data.FilterValues,
                scrollable: true,
                sortable: true,
                selectable: "single, row",
                resizable: true,
                columns: [
                    {
                        command: [
                            { className: "cm-icononly-button", name: "destroy", text: "" }
                        ], title: this.ActionsCaption, width: 75
                    },
                    { field: "filterID", title: tObj.FilterIDCaption, hidden: true },
                    { field: "filterCaption", title: tObj.FilterCaption },
                    { field: "filterName", title: this.FilterNameCaption, hidden: true },
                    { field: "filterValueFrom", title: tObj.FilterValueFromCaption },
                    { field: "filterValueTo", title: tObj.FilterValueToCaption },
                    { field: "filterFrom", title: tObj.FilterFromCaption, template: "#= kendo.toString(kendo.parseDate(filterFrom, 'yyyy-MM-dd'), 'MM/dd/yyyy') #" },
                    { field: "filterTo", title: tObj.FilterToCaption, template: "#= kendo.toString(kendo.parseDate(filterTo, 'yyyy-MM-dd'), 'MM/dd/yyyy') #" }],
                editable: {
                    mode: 'incell',
                    confirmation: false
                },
                save: function (e) { // added by kanna 02-13-2023 for enabling inline updating data using edit action on filter grid 
                    var ID = e.model.filterID
                        var fIndex = tObj.getFilterIndexByID(ID);
                        var filtersArray = tObj.data.FilterValues;
                        var updateFilter = filtersArray[fIndex];
                        updateFilter.filterCaption =  e.values.filterCaption || e.model.filterCaption;
                        updateFilter.filterName =  e.values.filterName ||e.model.filterName;
                        updateFilter.filterValueFrom =  e.values.filterValueFrom ||e.model.filterValueFrom;
                        updateFilter.filterValueTo =  e.values.filterValueTo ||e.model.filterValueTo;
                        updateFilter.filterFrom =  e.values.filterFrom ||e.model.filterFrom;
                        updateFilter.filterTo =  e.values.filterTo || e.model.filterTo;
                        tObj.data.FilterValues[fIndex]=updateFilter;
                        tObj.reapplyFilter();

                        oFilterGrid.data("kendoGrid").dataSource.read();

                },
                remove: function (e) {
                    var sName = e.model.filterName;
                    var ID = e.model.filterID
                    var fIndex = tObj.getFilterIndexByID(ID);
                    //alert('name: ' + sName + ' id: ' + ID.toString() + ' index: ' + fIndex.toString() );
                    tObj.data.FilterValues.splice(fIndex, 1);
                    //$.each(this.data.FilterValues, function(index, item){
                    //    alert('name: ' + item.filterName + ' id: ' + item.filterID.toString() );
                    //});
                    tObj.reapplyFilter();
                }
            });
            // Modified by RHR for v-8.5.4.004 on 12/19/2023 we just need to call sync to update the data now
            oFilterGrid.data("kendoGrid").dataSource.sync();//refresh grid
           
        }
    };

    this.addFilterDetail = function () {       
        var f = new FilterDetails();
        if (isNaN(this.iFilterID)) { this.iFilterID = 0; }
        this.iFilterID = this.iFilterID + 1;
        f.filterID = this.iFilterID;
        f.filterCaption = $("#ddl" + this.IDKey + "Filters").data("kendoDropDownList").text();
        f.filterName = $("#ddl" + this.IDKey + "Filters").data("kendoDropDownList").value();
        f.filterValueFrom = $("#txt" + this.IDKey + "FilterVal").data("kendoMaskedTextBox").value();
        f.filterValueTo = $("#txt" + this.IDKey + "FilterValTo").data("kendoMaskedTextBox").value();
        f.filterFrom = $("#dp" + this.IDKey + "FilterFrom").data("kendoDatePicker").value();
        f.filterTo = $("#dp" + this.IDKey + "FilterTo").data("kendoDatePicker").value();
      
        this.pushToAllFilters(f);

    };

    this.addFilterDetailOwnFilter = function (filters) {

        for (const filter of filters) {
            var f = new FilterDetails();
            f.filterID = filter.uid;
            f.filterCaption = filter.filterCaption;
            f.filterName = filter.filterName;
            f.filterValueFrom = filter.filterValueFrom;
            f.filterValueTo = filter.filterValueTo;
            f.filterFrom = null;
            f.filterTo = null;
          
            this.pushToAllFilters(f);
        }
    };

    this.addSavedFilters = function (savedFilters) {
        var tObj = this;
        if (typeof (savedFilters) !== 'undefined' && ngl.isObject(savedFilters) && ngl.isArray(savedFilters)) {
            try {
                this.data = new AllFilter();

                this.data.FilterValues = savedFilters;
              
                //first filter
                var first = this.data.FilterValues[0];
                if (typeof (first) !== 'undefined' && ngl.isObject(first) && isEmpty(first.filterName) == false) {
                    $("#ddl" + tObj.IDKey + "Filters").data("kendoDropDownList").value(first.filterName);
                }

                var oFilterGrid = $("#grd" + this.IDKey + "filters").kendoGrid({
                    theme: tObj.Theme,
                    dataSource: tObj.data.FilterValues,
                    scrollable: true,
                    sortable: true,
                    selectable: "single, row",
                    resizable: true,
                    columns: [
                        {
                            command: [
                                { className: "cm-icononly-button", name: "destroy", text: "" }],
                            title: this.ActionsCaption, width: 200
                        },
                        { field: "filterID", title: tObj.FilterIDCaption, hidden: true },
                        { field: "filterCaption", title: tObj.FilterCaption },
                        { field: "filterName", title: this.FilterNameCaption, hidden: true },
                        { field: "filterValueFrom", title: tObj.FilterValueFromCaption },
                        { field: "filterValueTo", title: tObj.FilterValueToCaption },
                        { field: "filterFrom", title: tObj.FilterFromCaption, template: "#= kendo.toString(kendo.parseDate(filterFrom, 'yyyy-MM-dd'), 'MM/dd/yyyy') #" },
                        { field: "filterTo", title: tObj.FilterToCaption, template: "#= kendo.toString(kendo.parseDate(filterTo, 'yyyy-MM-dd'), 'MM/dd/yyyy') #" }],
                    editable: {
                        mode: 'incell',
                        confirmation: false
                    },
                    save: function (e) {  // added by kanna 02-13-2023 for enabling inline updating data using edit action on filter grid 

                        var sName = e.model.filterName;
                        var ID = e.model.filterID;
                        var fIndex = tObj.getFilterIndexByID(ID);
                        var filtersArray = tObj.data.FilterValues;
                        var updateFilter = filtersArray[fIndex];
                        updateFilter.filterCaption = e.values.filterCaption || e.model.filterCaption;
                        updateFilter.filterName = e.values.filterName || e.model.filterName;
                        updateFilter.filterValueFrom = e.values.filterValueFrom || e.model.filterValueFrom;
                        updateFilter.filterValueTo = e.values.filterValueTo || e.model.filterValueTo;
                        updateFilter.filterFrom = e.values.filterFrom || e.model.filterFrom;
                        updateFilter.filterTo = e.values.filterTo || e.model.filterTo;
                        tObj.data.FilterValues[fIndex] = updateFilter;
                        tObj.reapplyFilter();
                        oFilterGrid.data("kendoGrid").dataSource.read();
                    },
                    remove: function (e) {
                        var sName = e.model.filterName;
                        var ID = e.model.filterID
                        var fIndex = tObj.getFilterIndexByID(ID);
                        //alert('name: ' + sName + ' id: ' + ID.toString() + ' index: ' + fIndex.toString() );
                        tObj.data.FilterValues.splice(fIndex, 1);
                        //$.each(this.data.FilterValues, function(index, item){
                        //    alert('name: ' + item.filterName + ' id: ' + item.filterID.toString() );
                        //});
                        tObj.reapplyFilter();
                    }
                });
                oFilterGrid.data("kendoGrid").dataSource.read();
                //$("#grd" + this.IDKey + "filters").data("kendoGrid").dataSource.read();
            } catch (err) {
                ngl.showInfoNotification("Cannot Load Saved Filter", "Your data is available but your saved filter could not be loaded.  Please re-enter your filters,  if this problem persists contact technical support.", null)
            }
        }
    }

    this.ItemSelected = function (e) {
        var tObj = this;
        if (typeof (e) === 'undefined' || ngl.isObject(e) === false) {
            return;
        }
        var name = e.dataItem.text;
        var val = e.dataItem.value;
        var isADate = tObj.isFilterADate(val);
        $("#txt" + tObj.IDKey + "FilterVal").data("kendoMaskedTextBox").value("");
        $("#txt" + tObj.IDKey + "FilterValTo").data("kendoMaskedTextBox").value("");
        $("#dp" + tObj.IDKey + "FilterFrom").data("kendoDatePicker").value("");
        $("#dp" + tObj.IDKey + "FilterTo").data("kendoDatePicker").value("");
        if (val === "None") {
            $("#sp" + tObj.IDKey + "filterText").hide();
            $("#sp" + tObj.IDKey + "filterDates").hide();
            //$("#sp" + tObj.IDKey + "filterButtons").hide();
        } else {
            //show the  buttons 
            $("#sp" + tObj.IDKey + "filterButtons").show();
            $("#btn" + this.IDKey + "AddFilter").show();
            if (isADate === true) {
                $("#sp" + tObj.IDKey + "filterText").hide();
                $("#sp" + tObj.IDKey + "filterDates").show();
            } else {
                $("#sp" + tObj.IDKey + "filterText").show();
                $("#sp" + tObj.IDKey + "filterDates").hide();
            }
        }
    }

    this.loadHTML = function () {
        var sHtml = '<div id="' + this.IDKey + 'wrapper">' +
            '<div id="' + this.IDKey + 'FilterFastTab" style="margin-left: 50px;">' +
            '<span id="Expand' + this.IDKey + 'FilterFastTabSpan" style="display:none;">' +
            '<a onclick="expandFastTab(\'Expand' + this.IDKey + 'FilterFastTabSpan\',\'Collapse' + this.IDKey + 'FilterFastTabSpan\',\'' + this.IDKey + 'FilterFastTabHeader\',null);">' +
            '<span style="font - size: small; font - weight:bold;" class="k-icon k-i-chevron-down"></span></a></span>' +
            '<span id="Collapse' + this.IDKey + 'FilterFastTabSpan" style="display:normal;">' +
            '<a onclick="collapseFastTab(\'Expand' + this.IDKey + 'FilterFastTabSpan\',\'Collapse' + this.IDKey + 'FilterFastTabSpan\',\'' + this.IDKey + 'FilterFastTabHeader\',null);">' +
            '<span style="font - size: small; font - weight:bold;" class="k-icon k-i-chevron-up"></span></a></span>' +
            '<span style="font-size:small; font-weight:bold" > ' + this.FilterFastTabCaption + ' </span>' +
            '<div id="' + this.IDKey + 'FilterFastTabHeader" style="padding: 10px; margin-left: 5px;"><span>';

        if (saveFilters == 'True') {
            sHtml += '<div style="margin: 5px 0 15px 0">' +
                '<label for="filter' + this.IDKey + 'Groups" style="margin-right: 5px">Load Saved Filter:</label>' +
                '<input id="filter' + this.IDKey + 'Groups" style="width: 200px;" />' +
                '<input type="text" id="filter' + this.IDKey + 'Name" name="filterName" style="margin-right: 5px; width: 200px;"/>' +
                '<button type="button" id="saveOwn' + this.IDKey + 'Filter">Save Current Filter</button>' +
                '</div>'
        }

        sHtml += '<label for="ddl' + this.IDKey + 'Filters" > Select Filter:</label>' +
            '<input id="ddl' + this.IDKey + 'Filters" style="width: 200px;" />' +
            '<span id="sp' + this.IDKey + 'filterText">' +
            '<label for="txt' + this.IDKey + 'FilterVal" > From:</label>' +
            '<input id="txt' + this.IDKey + 'FilterVal" style="width: 200px;" />' +
            '<label for="txt' + this.IDKey + 'FilterValTo" > To:</label>' +
            '<input id="txt' + this.IDKey + 'FilterValTo" style="width: 200px;"/></span>' +
            '<span id="sp' + this.IDKey + 'filterDates" >' +
            '<label for="dp' + this.IDKey + 'FilterFrom" > From:</label>' +
            '<input id="dp' + this.IDKey + 'FilterFrom" style="width: 200px;"/>' +
            '<label for="dp' + this.IDKey + 'FilterTo" > To:</ label>' +
            '<input id="dp' + this.IDKey + 'FilterTo" style="width: 200px;"/></span>' +
            '<span id="sp' + this.IDKey + 'filterButtons" >' +
            '<a id="btn' + this.IDKey + 'AddFilter" ></a>' +
            '<a id="btn' + this.IDKey + 'Filter" ></a>' +
            '<a id="btn' + this.IDKey + 'ClearFilter" ></a></span></span>' +
            '<div id="grd' + this.IDKey + 'filters"></div>' +
            '<input id="txt' + this.IDKey + 'SortDirection" type="hidden" />' +
            '<input id="txt' + this.IDKey + 'SortField" type="hidden" /></div></div>' +
            '</div>';
        //'<div id="' + this.IDKey + '" style="height:calc(100vh - 10); "></div></div>';
        //console.log(sHtml);
        $("#" + this.ContainerDivID).html(sHtml);
    };

    this.loadCustomFiltrDDL = function () {
        var tObj = this;
        if (typeof (this.FilterData) === 'undefined' || ngl.isObject(this.FilterData) === false || ngl.isArray(this.FilterData) === false) { return; };

        let gridKey = gridApiRef;
        let ddlKey = this.IDKey;
        // The API url: `/api/LoadBoard/Filters?gridId=${gridKey}`, is inside the NGLControllerBase class LoadBoard is used  by default but any controller would work.
        $("#filter" + this.IDKey + "Groups").kendoDropDownList({
            dataTextField: "Name",
            template: `<div style="display: flex; justify-content: space-between; "><span>#:data.Name#</span>&nbsp;&nbsp;  <span class="delBtn${ddlKey}" data-filterId="#:data.FilterId#" onClick="return false"></span></div>`,
            optionLabel: "None",
            filter: "contains",
            dataSource: {
                transport: {
                    read: function (options) {
                        $.ajax({
                            async: true,
                            type: "GET",
                            url: `/api/LoadBoard/Filters?gridId=${gridKey}`,
                            contentType: "application/json; charset=utf-8",
                            dataType: 'json',
                            headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                            success: function (data) {
                                if (data.Data) {
                                    options.success(data.Data);
                                } else {
                                    options.success([]);
                                }
                            }
                        });
                    }
                }
            },
            open: function () {
                let that = this;
                if ($(`.delBtn${ddlKey}:not(.k-button)`).length) {
                    $(`.delBtn${ddlKey}`).kendoButton({
                        icon: "trash",
                        click: function (e) {
                            let filterId = $(e.sender.element[0]).data().filterid;
                            if (filterId) {
                                $.ajax({
                                    type: 'DELETE',
                                    url: `/api/LoadBoard/DeleteFilter?filterId=${filterId}&gridId=${gridKey}`,
                                    contentType: "application/json; charset=utf-8",
                                    dataType: 'json',
                                    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                    success: function () {
                                        let ddlFilter = that;

                                        //find the item which will be removed from kendoDropDownList
                                        var item = ddlFilter.dataSource.data().find(function (e) {
                                            return e.FilterId === filterId;
                                        });

                                        //select the current item's index
                                        let selectedIndex = ddlFilter.select() - 1 < 0 ? 0 : ddlFilter.select() - 1;
                                        let selectedItem = ddlFilter.dataSource._data[selectedIndex];

                                        //remove the item from the dataSource
                                        ddlFilter.dataSource.remove(item);

                                        if (selectedItem.FilterId === filterId) {
                                            ddlFilter.value("None");
                                            ddlFilter.trigger("change");
                                            tObj.clearFilter();
                                        }

                                        ddlFilter.close();
                                    }
                                });
                            }
                        }
                    });
                }
            },
            select: function (e) {
                let item = e.item;
                let text = item.text();

                if (text === "None") {
                    tObj.clearFilter();
                } else {
                    let globalMatch = false;

                    for (const ddlItem of this.dataSource._data) {
                        if (!tObj.data.FilterValues || ddlItem.Filters.length !== tObj.data.FilterValues.length) {
                            continue;
                        }

                        for (const ddlFilter of ddlItem.Filters) {
                            let match = false;

                            for (const current of tObj.data.FilterValues) {
                                if (ddlFilter.filterCaption === current.filterCaption &&
                                    ddlFilter.filterName === current.filterName &&
                                    ddlFilter.filterValueFrom === current.filterValueFrom &&
                                    ddlFilter.filterValueTo === current.filterValueTo
                                ) {
                                    match = true;
                                }
                                else {
                                    match = false;
                                }
                            }
                            if (match) {
                                globalMatch = true;
                            }
                        }
                    }
                    if (!globalMatch && tObj.data.FilterValues && tObj.data.FilterValues.length !== 0) {
                        let filters = Object.assign({}, tObj.data);
                        ngl.OkCancelConfirmation("Do you want to save this new filter?", "New Filter name: <input id='ownFilterName'></input>", 400, 400, tObj, "addOwnRecord", filters);
                    }
                    tObj.data.FilterValues = [];
                    tObj.addFilterDetailOwnFilter(e.dataItem.Filters);
                    tObj.reapplyFilter();
                }
            }
        });

        this.addOwnRecord = function (e, data) {
            if (e == 1) {
                let filterName = $('#ownFilterName').val()
                let dataObject = JSON.stringify(data);
                $.ajax({
                    async: true,
                    type: 'POST',
                    url: `/api/LoadBoard/SaveFilter?filterName=${filterName}&filter=${dataObject}&gridId=${gridKey}`,
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                    success: function (res) {
                        $("#filter" + tObj.IDKey + "Name").val("");

                        let ddlFilters = $("#filter" + tObj.IDKey + "Groups").data("kendoDropDownList")
                        ddlFilters.dataSource.add({ Name: filterName, Filters: data.FilterValues, FilterId: res.Data[0] });

                        ngl.showSuccessMsg("Filter has been saved");
                    }
                });
            }
            else {
                return;
            }
        }


        $("#saveOwn" + this.IDKey + "Filter").kendoButton({
            click: function () {
                let filterName = $("#filter" + tObj.IDKey + "Name").val();
                if (!filterName) {
                    ngl.Alert('Error message', 'You need to provide a filter name', 300, 300);
                    return;
                }
                let data = JSON.stringify(tObj.data);
                $.ajax({
                    async: true,
                    type: 'POST',
                    url: `/api/LoadBoard/SaveFilter?filterName=${filterName}&filter=${data}&gridId=${gridKey}`,
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                    success: function (res) {
                        $("#filter" + tObj.IDKey + "Name").val("");

                        let ddlFilters = $("#filter" + tObj.IDKey + "Groups").data("kendoDropDownList")
                        ddlFilters.dataSource.add({ Name: filterName, Filters: tObj.data.FilterValues, FilterId: res.Data[0] });
                        ddlFilters.select(ddlFilters.dataSource._data.length);

                        ngl.showSuccessMsg("Filter has been saved");
                    }
                });
            }
        });

        //$("#filter" + this.IDKey + "Name").kendoTextBox({
        $("#filter" + this.IDKey + "Name").kendoMaskedTextBox({
            placeholder: "Filter Name",
            label: {
                floating: false
            }
        });
    }

    this.loadFilterDDL = function () {
        var tObj = this;
        if (typeof (this.FilterData) === 'undefined' || ngl.isObject(this.FilterData) === false || ngl.isArray(this.FilterData) === false) { return; };
        var ddlfilterData = new Array();
        ddlfilterData.push({ text: "None", value: "None" })
        $.each(this.FilterData, function (index, item) {
            ddlfilterData.push({ text: item.filterCaption, value: item.filterName })
        });

        $("#ddl" + this.IDKey + "Filters").kendoDropDownList(
            {
                dataTextField: "text", dataValueField: "value",
                autoWidth: true,
                dataSource: ddlfilterData,
                filter: "contains",
                select: function (e) {
                    tObj.ItemSelected(e);
                }
            });
    }

    this.addSelectedFilter = function () {
      
        var tObj = this;
        var dataItem = $("#ddl" + tObj.IDKey + "Filters").data("kendoDropDownList").dataItem();
        var name = dataItem.text;
        var val = dataItem.value;
        if (val === 'None') { return; }
        var isADate = tObj.isFilterADate(val);
        if (isADate === true) {
            var dtFrom = $("#dp" + tObj.IDKey + "FilterFrom").data("kendoDatePicker").value();
            if (!dtFrom) {
                ngl.showErrMsg("Required Fields", "Filter From date cannot be null", null);
                return;
            }
        } else {
            var valFrom = $("#txt" + tObj.IDKey + "FilterVal").data("kendoMaskedTextBox").value();
            if (!valFrom) {
                ngl.showErrMsg("Required Fields", "Filter From value cannot be null", null);
                return;
            }
        }
        
        tObj.addFilterDetail();
        //reset the selected filter
        $("#txt" + tObj.IDKey + "FilterVal").data("kendoMaskedTextBox").value("");
        $("#txt" + tObj.IDKey + "FilterValTo").data("kendoMaskedTextBox").value("");
        $("#dp" + tObj.IDKey + "FilterFrom").data("kendoDatePicker").value("");
        $("#dp" + tObj.IDKey + "FilterTo").data("kendoDatePicker").value("");
        $("#sp" + tObj.IDKey + "filterText").hide();
        $("#sp" + tObj.IDKey + "filterDates").hide();
        //hide the add buttion until a new filter is selected
        $("#btn" + this.IDKey + "AddFilter").hide();
        var dropdownlist = $("#ddl" + tObj.IDKey + "Filters").data("kendoDropDownList");
        dropdownlist.select(0);
        dropdownlist.trigger("change");
        //$("#ddl" + tObj.IDKey + "Filters").data("kendoDropDownList").select(0);
        //$("#sp" + this.IDKey + "filterText").hide();
        //$("#sp" + this.IDKey + "filterDates").hide();
        //$("#sp" + this.IDKey + "filterButtons").hide();

    }

    this.clearFilter = function () {
        var tObj = this;
       
        var dropdownlist = $("#ddl" + tObj.IDKey + "Filters").data("kendoDropDownList");
        dropdownlist.select(0);
        dropdownlist.trigger("change");
        if (typeof (tObj.data) !== 'undefined' && ngl.isObject(tObj.data) === true) {
            if (typeof (tObj.data.FilterValues) !== 'undefined') {
                tObj.data.FilterValues = new Array();
            };
        };
        $("#txt" + tObj.IDKey + "FilterVal").data("kendoMaskedTextBox").value("");
        $("#txt" + tObj.IDKey + "FilterValTo").data("kendoMaskedTextBox").value("");
        $("#dp" + tObj.IDKey + "FilterFrom").data("kendoDatePicker").value("");
        $("#dp" + tObj.IDKey + "FilterTo").data("kendoDatePicker").value("");
        $("#sp" + tObj.IDKey + "filterText").hide();
        $("#sp" + tObj.IDKey + "filterDates").hide();
        $("#sp" + tObj.IDKey + "filterButtons").hide();
        var fGrid = $("#grd" + tObj.IDKey + "filters").data("kendoGrid");
        if (typeof (fGrid) !== 'undefined' && ngl.isObject(fGrid) === true) {
            //$("#grd" + tObj.IDKey + "filters").data("kendoGrid").dataSource.read();
            //tObj.data.FilterValues
            //fGrid.dataSource.read();
            //fGrid.dataSource.sync();

            //var grid = $("#grid").data("kendoGrid");
            var dataSource = fGrid.dataSource;
            dataSource.data([]);//clear out old data
            dataSource.data(tObj.data.FilterValues);//add new data
          
            //fGrid.setDataSource(tObj.data.FilterValues);//set the new data as the grids new datasource
            dataSource.sync();//refresh grid
        }
        if (ngl.isFunction(tObj.onSelect)) {
            var oResults = new nglEventParameters();
            oResults.source = "filter_clear";
            oResults.widget = tObj;
            oResults.msg = 'success'; //set def
            oResults.data = tObj.data;
            oResults.datatype = "AllFilter";
            tObj.onSelect(oResults);
        }
    };


    this.reapplyFilter = function () {
     
        var tObj = this;
        //add the current filter to the list
        // tObj.addSelectedFilter();
        if (ngl.isFunction(tObj.onSelect)) {
            var oResults = new nglEventParameters();
            oResults.source = "filter_click";
            oResults.widget = tObj;
            oResults.msg = 'success'; //set def
            oResults.data = tObj.data;
            oResults.datatype = "AllFilter";
            tObj.onSelect(oResults);
        }
    };

    this.applyFilter = function () {
        var tObj = this;
        //add the current filter to the list
       
        tObj.addSelectedFilter();
        if (ngl.isFunction(tObj.onSelect)) {
            var oResults = new nglEventParameters();
            oResults.source = "filter_click";
            oResults.widget = tObj;
            oResults.msg = 'success'; //set def
            oResults.data = tObj.data;
            oResults.datatype = "AllFilter";
            tObj.onSelect(oResults);
        }
    };


    this.show = function () {
      
        if (typeof (this.data) === 'undefined' || ngl.isObject(this.data) === false) {
            return;
        }

        var tObj = this;
        //All the HTML DOM objects must be loaded first
        this.loadHTML();
        //if the filter data is missing we cannot continue
        if (typeof (this.FilterData) === 'undefined' || ngl.isObject(this.FilterData) === false || ngl.isArray(this.FilterData) === false) { return; };
        //load the drop down list 
        this.loadFilterDDL();
        // CHA 26.07.2021
        // Added a way to add customfilter menu
        if (saveFilters == 'True') {
            this.loadCustomFiltrDDL();
        }
        //configure all the Kendo objects used for filtering
        $("#txt" + this.IDKey + "FilterVal").kendoMaskedTextBox();
        $("#txt" + this.IDKey + "FilterValTo").kendoMaskedTextBox();
        $("#dp" + this.IDKey + "FilterFrom").kendoDatePicker();
        $("#dp" + this.IDKey + "FilterTo").kendoDatePicker();
        $("#btn" + this.IDKey + "AddFilter").kendoButton({
            icon: "plus",
            click: function (e) {
                
                tObj.addSelectedFilter();
            }
        });
        $("#btn" + this.IDKey + "Filter").kendoButton({
            icon: "filter",
            click: function (e) {
               
                tObj.applyFilter();
            }
        });
        $("#btn" + this.IDKey + "ClearFilter").kendoButton({
            icon: "filter-clear",
            click: function (e) {
                tObj.clearFilter();
            }
        });
        $("#txt" + this.IDKey + "FilterVal").data("kendoMaskedTextBox").value("");
        $("#txt" + this.IDKey + "FilterValTo").data("kendoMaskedTextBox").value("");
        $("#dp" + this.IDKey + "FilterFrom").data("kendoDatePicker").value("");
        $("#dp" + this.IDKey + "FilterTo").data("kendoDatePicker").value("");
        $("#sp" + this.IDKey + "filterText").hide();
        $("#sp" + this.IDKey + "filterDates").hide();
        $("#sp" + this.IDKey + "filterButtons").hide();
    };

    //Note:  this method never gets called because we do not have a callback method for the kendoWindow
    this.close = function (e) {
        //var oResults = new nglEventParameters();
        //oResults.source = "close";
        //oResults.widget = this;
        //oResults.msg = 'closing nothing is saved';

        //if (typeof (this.onClose) === "function") {
        //    this.onClose(oResults);
        //}
    }

    //loadDefaults sets up the callbacks cbSelect and cbSave
    //all call backs return a reference to this object and a string message as parameters
    this.loadDefaults = function (ContainerDivID, IDKey, FilterData, Theme, selectCallBack, sFilterFastTabCaption) {
        this.onSelect = selectCallBack;
        this.data = new AllFilter();
        this.lastErr = "";
        this.notifyOnError = true;
        this.notifyOnSuccess = false;
        this.notifyOnValidationFailure = false;
        var tObj = this;
        this.ContainerDivID = ContainerDivID;
        this.IDKey = IDKey;
        this.FilterData = FilterData;
        this.Theme = Theme;
        if (!isEmpty(sFilterFastTabCaption)) { this.FilterFastTabCaption = sFilterFastTabCaption; } else { this.FilterFastTabCaption = "Filters"; }

    }
}

function NGLClassTest() {

    TariffGridCRUDCtrl: null;

    CarrTarName: null;

    CarrTarID: null;
}

function NGLClassTest1() {
    TariffGridCRUDCtrl = "";

    CarrTarName = "";

    CarrTarID = "";
}


/// Added By RHR on 07/06/2018 for v-8.2
/// Widget for Edit Add and Delete via Content Management Popup Window
///     Get{id} to read the record
///     POST{data} to insert or update a record
///     DELETE{id} to delete the record
function NGLEditWindCtrl() {
    //Generic properties for all Widgets
    this.lastErr = "";
    this.notifyOnError = true;
    this.notifyOnSuccess = true;
    this.notifyOnValidationFailure = true;
    this.NGLRespToolTip = null;
    data: null;
    DataType: "";
    rData: null;
    callback: null;
    dSource: null;
    sourceDiv: null;
    kendoWindowsObj: null;
    //Widget specific properties
    this.DataSourceName = "Record";
    this.EditErrorMsg = "Please select a row with valid data";
    this.EditErrorTitle = "Data Required";
    this.AddErrorMsg = "Invalid field information; cannot add new record";
    this.AddErrorTitle = "Field Data Required";
    screenLEName: null;
    IDKey: "id1234";
    ParentIDKey: "id22222";
    this.Theme = "blueopal";
    dataFields: DataFieldDetails;  //list of fields to edit or insert
    ContainerDivID: "";
    this.kendoWindowsObjUploadEventAdded = 0;
    this.EditError = false;
    API: "";
    this.blnEditing = false;
    this.CRUD = "create";
    this.sNGLCtrlName = ""; //variable used on page for the control like "wdgt" +  sCleanPageDetName + "Dialog"
    this.ctrlSubTypes = new GroupSubTypes();
    this.ctrlContainers = null; //variable to hold a list of all the containers that may expose CRUD  and Action operations.  the popup control will call these methods using this list
    this.ctrlKendoNGLGrid = null;
    //Widget specific functions

    this.comparefieldSequenceNo = function (a, b) {
        //if return value is  less than zero a comes before b
        if (parseInt(b.fieldSequenceNo) > parseInt(a.fieldSequenceNo)) return -1;
        //if return value is greater than zero b comes before a (move b up)
        if (parseInt(a.fieldSequenceNo) > parseInt(b.fieldSequenceNo)) return 1;
        //if return value is zero no change
        return 0;
    }

    this.GetFieldID = function (sName) {
        var sRet = "";
        if (!sName) { return sRet; }
        $.each(this.dataFields, function (index, item) {
            if (item.fieldName == sName) {
                sRet = item.fieldTagID;
                return;
            }
        });
        return sRet;
    }

    this.SetFieldDefault = function (sName, sVal) {
        var blnRet = false;
        if (!sVal) { return blnRet; }
        if (!sName) { return blnRet; }
        $.each(this.dataFields, function (index, item) {
            if (item.fieldName == sName) {
                sRet = item.fieldDefaultValue = sVal;
                blnRet = true;
                return;
            }
        });
        return blnRet;
    }

    this.GetFieldName = function (sID) {
        var sRet = "";
        if (!sID) { return sRet; }
        $.each(this.dataFields, function (index, item) {
            if (item.fieldTagID == sID) {
                sRet = item.fieldName;
                return;
            }
        });
        return sRet;
    }

    //Modified by RHR for v-8.2.1 on 10/7/19
    //  updated functionality to use logic from NGLPopupWindow HTML code
    //  this expands functionality to include additional html tags like headers etc...
    this.loadHTML = function () {
        var tObj = this;
        this.NGLRespToolTip = new NGLRespToolTipCtrl();
        if (typeof (this.ctrlSubTypes) === "undefined" || !ngl.isObject(this.ctrlSubTypes)) { this.ctrlSubTypes = new GroupSubTypes(); }
        this.ctrlSubTypes.addDefaultSubTypesIfNeeded();
        //in the new code we use Jquery to append html objects into parents
        // the root parent is the windows Container Div
        var divContainer = $("#" + this.ContainerDivID);
        divContainer.css({ position: "relative" }) // position must be relative       
        if (!divContainer) { return false; } // no parent so cannot continue
        //set the active container
        var divActive = divContainer;
        //append the primary wrapper container,  for NGLEDITWidgets we use this format
        var divThisWrapperID = "wnd" + this.IDKey + "Edit";
        divActive.append(kendo.format("<div id='{0}' style='position:relative;'></div>", divThisWrapperID));
        divActive = $("#" + divThisWrapperID);
        //add the blue border (all edit widgets must be inside a blue border for v-8.2.1
        var divBlueWrapperID = "blueborder" + this.IDKey + "Edit";
        divActive.append(kendo.format("<div id='{0}' style='position:relative;' class='ngl-blueBorderFullPage'></div>", divBlueWrapperID));
        divActive = $("#" + divBlueWrapperID);
        //append a tag to change focus on cancel or save
        divActive.append('<a id="' + this.IDKey + 'focusCancel" href="#"></a>');
        //append the edit container inside the blue border
        var divEditContainer = this.IDKey + "EditDiv";
        divActive.append(kendo.format("<div id='{0}' style='position:relative; padding: 0px 10px 10px 10px;'></div>", divEditContainer));
        //make the edit container active
        divActive = $("#" + divEditContainer);
        //add a float block, we should let the content management contorol this
        //but for testing and backward compatibility we inser the first data
        //container as a float block
        var divDataContainer = this.IDKey + 'Data';
        //append the data container to the edit container
        divActive.append(kendo.format("<div id='{0}' style='float:left;'></div>", divDataContainer));
        // make the data container active
        divActive = $("#" + divDataContainer);
        var divEdit = $("#" + divDataContainer);
        // add the html objects to the data container
        if (typeof (this.dataFields) !== 'undefined' && ngl.isObject(this.dataFields) === true && ngl.isArray(this.dataFields) === true) {
            var divWrapper = $("#" + divThisWrapperID);
            divActive.append(kendo.format("<div id='div{0}InsertOnly' style='padding: 0px 10px 10px 10px; display:none; float: left;'></div>", this.IDKey));

            tObj.blnEditing = false;
            //if we have data we have an existing record to edit so set blnEditing to true
            if (typeof (this.data) !== 'undefined' && ngl.isObject(this.data)) {
                this.blnEditing = true;
            };
            var blnMustResequence = false;
            if (this.blnEditing == true) {
                $.each(this.dataFields, function (index, item) {
                    if (item.fieldEditWndVisibility == 1) {
                        blnMustResequence = true;
                        item.fieldSequenceNo = item.fieldEditWndSeqNo;
                        item.fieldVisible = true;
                    } else if (item.fieldEditWndVisibility > 1) {
                        item.fieldVisible = false;
                    }
                });
            } else {
                $.each(this.dataFields, function (index, item) {
                    if (item.fieldAddWndVisibility == 1) {
                        blnMustResequence = true;
                        item.fieldSequenceNo = item.fieldAddWndSeqNo;
                        item.fieldVisible = true;
                    } else if (item.fieldAddWndVisibility > 1) {
                        item.fieldVisible = false;
                    }
                });
            }
            if (blnMustResequence == true) {
                this.dataFields.sort(this.comparefieldSequenceNo);
            }
            // create a define the repeating containers used for default data objects
            var $HiddenFields = $("<div style='display:none;'></div>");
            var $InsertOnlyHTML = $("#div" + this.IDKey + "InsertOnly");  //$(kendo.format("<div id='div{0}InsertOnly' style='padding: 0px 10px 10px 10px; display:none; float: left;'></div>", this.IDKey));
            var $floattable = $("<table class='tblResponsive'></table>");
            var $floattableheaderrow = $("<tr></tr>");
            var $floattableitemrow = $("<tr></tr>");
            var floattablecount = 0;
            var sRequiredAsterik = '';
            var sReadOnly = '';
            var bDivOpen = false;
            var bFloatLeftTableOpen = false;
            var bFloatHeadersOpen = false;
            var sFloatTableHeader = "";
            var sFloatTableRow = "";
            var iTableColumns = 0;
            var iTableHeaders = 0;
            var sFormated = "";
            var bInsertOnlyOpen = false;
            var domLastID = divDataContainer;
            $.each(this.dataFields, function (index, item) {
                // if (item.fieldAllowNull === false) { item.fieldRequired = true; }               
                sFormated = "";
                if (item.fieldReadOnly === true) {
                    sReadOnly = '<span class="k-icon k-i-lock"></span>&nbsp;';
                    sRequiredAsterik = '';
                } else {
                    sReadOnly = '';
                    //if (item.fieldAllowNull === false || item.fieldRequired === true) {
                    //Modified by RHR for v-8.2 on 01/02/2019 removed logic to test for allow null on required
                    if (item.fieldRequired === true) {
                        sRequiredAsterik = '<span class="redRequiredAsterik"> *</span>';
                    }
                    else {
                        sRequiredAsterik = '';
                    }
                }
                // note: this code is from the popup window control  the actual meaning is not clear
                //we may have issues with this on the edit window as we are not sure why we need to 
                //add a temporary container wrapper for each child?

                //any child containers created by Content Management must load any required html into a temp container
                var sTmpChildContainerDivID = "tmp" + item.fieldTagID + "wrapper";
                var divTmpContainer = $("#" + sTmpChildContainerDivID);
                if (typeof (divTmpContainer) !== 'undefined' && divTmpContainer != null) {
                    //add the divTmpContainer html to this container the child container does not exist
                    var sChildContainer = "div" + item.fieldTagID + "wrapper";
                    var divChildContainer = $("#" + sChildContainer);
                    if (!divChildContainer) {
                        var sChildHTML = divTmpContainer.html();
                        divChildContainer.html(sChildHTML);
                        divTmpContainer.html("");
                        divActive.append(divChildContainer);
                    }
                }
                //check if this is a container and add it to the list.
                if (tObj.ctrlSubTypes.isSubTypeAContainer(item.fieldGroupSubType)) {
                    if (tObj.ctrlContainers == null) {
                        tObj.ctrlContainers = new Array();
                    }
                    tObj.ctrlContainers.push(item);
                }
                // now check for hidden fields
                if (tObj.blnEditing === true && ((item.fieldVisible === false && item.fieldReadOnly === true) || (item.fieldVisible === false && (item.fieldRequired === false || item.fieldInsertOnly === true)))) {
                    //when editing hide fields where visible is false and readonly is true, or where visible is false and (required is false or insert only is true)
                    $HiddenFields.append('<input id="' + item.fieldTagID + '" type="hidden" />');
                } else if (tObj.blnEditing === false && (item.fieldAddWndVisibility != 1 && ((item.fieldVisible === false && item.fieldReadOnly === true) || (item.fieldVisible === false && item.fieldInsertOnly === false && item.fieldRequired === false) || (item.fieldVisible === true && item.fieldReadOnly === true && item.fieldInsertOnly === false)))) {
                    //when adding new records  hide hide fields where visible is false and readonly is true, or where visible is false and (required is false or insert only is true) or where visible = true and readonly = true and insertonly = false
                    $HiddenFields.append('<input id="' + item.fieldTagID + '" type="hidden" />');
                } else {
                    if (item.fieldGroupSubType === nglGroupSubTypeEnum.Span) {
                        sFormated = kendo.format("<span id='sp{3}' style='{0}' class='{1}'>{2}</span>", item.fieldStyle, item.fieldCssClass, item.fieldCaption, item.fieldTagID);
                        domLastID = "sp" + item.fieldTagID;
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Button) {
                        var sAction = "";
                        if (!isEmpty(item.fieldWidgetAction)) {
                            //sAction = 'tPage["' + tObj.sNGLCtrlName + '"].raiseAction("' + item.fieldWidgetAction + '")';
                            if (typeof (tPage["execActionClick"]) !== 'undefined' && ngl.isFunction(tPage["execActionClick"])) {
                                //tPage["execActionClick"](dataItem.fieldWidgetAction, tObj.sNGLCtrlName);
                                sAction = 'tPage["execActionClick"]("' + item.fieldWidgetAction + '","' + tObj.sNGLCtrlName + '")';
                            }
                            //console.log("sAction: ");
                            //console.log(sAction);
                        }
                        // Modifie by RHR for v-8.5.4.001 added sub string comparison to CSS class
                        if (item.fieldCssClass === 'cm-icononly-button' || item.fieldCssClass.substring(0, 8) === 'k-button') {
                            if (isEmpty(item.fieldStyle)) {
                                item.fieldStyle = 'k-icon k-i-connector';
                            } sFormated = kendo.format("&nbsp;&nbsp;<a id='btn{5}' class='{0}' href='#' onclick='{1}'><span class='{2}' style='{3}'></span>{4}</a>&nbsp;&nbsp;", item.fieldCssClass, sAction, item.fieldStyle, item.fieldFormat, item.fieldCaption, item.fieldTagID);

                        } else {
                            sFormated = kendo.format("&nbsp;&nbsp;<button id='btn{4}' class='{0}' style='{2}' onclick='{1}'>{3}</button>&nbsp;&nbsp;", item.fieldCssClass, sAction, item.fieldStyle, item.fieldCaption, item.fieldTagID);
                        }
                        domLastID = "btn" + item.fieldTagID;
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Header1) {
                        if (item.fieldCssClass == 'NGLToolTip' && ngl.stringHasValue(item.fieldNGLToolTip) == true) {
                            tObj.NGLRespToolTip.addToolTip('h1' + item.fieldTagID, $("#blueborder" + tObj.IDKey + "Edit"), item.fieldNGLToolTip);
                        }
                        sFormated = kendo.format(" <h1 id='h1{0}' style='clear:both; display: block; float: none;'>{1}</h1>", item.fieldTagID, item.fieldCaption);
                        domLastID = "h1" + item.fieldTagID;
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Header2) {
                        if (item.fieldCssClass == 'NGLToolTip' && ngl.stringHasValue(item.fieldNGLToolTip) == true) {
                            tObj.NGLRespToolTip.addToolTip('h2' + item.fieldTagID, $("#blueborder" + tObj.IDKey + "Edit"), item.fieldNGLToolTip);
                        }
                        sFormated = kendo.format(" <h2 id='h2{0}' style='clear:both; display: block; float: none;'>{1}</h2>", item.fieldTagID, item.fieldCaption);
                        domLastID = "h2" + item.fieldTagID;
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Header3) {
                        if (item.fieldCssClass == 'NGLToolTip' && ngl.stringHasValue(item.fieldNGLToolTip) == true) {
                            tObj.NGLRespToolTip.addToolTip('h3' + item.fieldTagID, $("#blueborder" + tObj.IDKey + "Edit"), item.fieldNGLToolTip);
                        }
                        sFormated = kendo.format(" <h3 id='h3{0}' style='clear:both; display: block; float: none;'>{1}</h3>", item.fieldTagID, item.fieldCaption);
                        domLastID = "h3" + item.fieldTagID;
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Header4) {
                        if (item.fieldCssClass == 'NGLToolTip' && ngl.stringHasValue(item.fieldNGLToolTip) == true) {
                            tObj.NGLRespToolTip.addToolTip('h4' + item.fieldTagID, $("#blueborder" + tObj.IDKey + "Edit"), item.fieldNGLToolTip);
                        }
                        sFormated = kendo.format(" <h4 id='h4{0}' style='clear:both; display: block; float: none;'>{1}</h4>", item.fieldTagID, item.fieldCaption);
                        domLastID = "h4" + item.fieldTagID;
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Paragraph) {
                        sFormated = kendo.format(" <p id='p{0}' style='clear:both; display: block; float: none;'>{1}</p>", item.fieldTagID, item.fieldCaption);
                        domLastID = "p" + item.fieldTagID;
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Link) {
                        sFormated = kendo.format(" <a id='a{0}' href='{2}'>{1}</a>", item.fieldTagID, item.fieldCaption, item.fieldAPIReference);
                        domLastID = "a" + item.fieldTagID;
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.HTMLSpace) {
                        sFormated = "&nbsp;";
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.HTMLLineBreak) {
                        sFormated = kendo.format(" <br id='br{0}' style='clear:both; display: block; float: none;' />", item.fieldTagID);
                        domLastID = "br" + item.fieldTagID;
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Div) {
                        //once a div is entered it becomes active and everything below it falls inside this div until   
                        //the next float block, float blocks are always appended to divEdit
                        if (bFloatLeftTableOpen == true) {
                            //append the float table first because divs are not allowed in tables
                            divActive.append(kendo.format("<div id='divFloatTable-{0}' style='float: left;'></div>", floattablecount.toString()));
                            $floattable.append($floattableitemrow);
                            $("#divFloatTable-" + floattablecount.toString()).append($floattable);
                            bFloatLeftTableOpen = false
                        }
                        divActive.append("<div id='div" + item.fieldTagID + "Break' style='padding: 0px 10px 10px 10px; clear:both; display: block; float: none;'></div>");
                        divActive = $("#div" + item.fieldTagID + "Break");
                        domLastID = "div" + item.fieldTagID + "Break";
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.FloatLeftTable) {
                        //Note: Once FloatLeftTable is active all fields are added to the table until a 
                        //div a FloatBlockLeft or another FloatLeftTable is activated
                        // fields are added to rows according to the number of tableheader fields included
                        // Labels are used to include text in a row instead of a data object                       
                        if (bFloatLeftTableOpen == true) {
                            //append the open float table to the active div and create a new float table
                            //float tables are always nested inside a <div style="float: left;">
                            divActive.append(kendo.format("<div id='divFloatTable-{0}' style='float: left;'></div>", floattablecount.toString()));
                            $floattable.append($floattableitemrow);
                            $("#divFloatTable-" + floattablecount.toString()).append($floattable);
                        }
                        $floattable = $("<table class='tblResponsive'></table>");
                        $floattableheaderrow = $("<tr></tr>");
                        $floattableitemrow = $("<tr></tr>");
                        floattablecount = floattablecount + 1;
                        iTableColumns = 0;
                        iTableHeaders = 0;
                        bFloatLeftTableOpen = true;
                        bFloatHeadersOpen = false;

                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.FloatBlockLeft) {
                        //float blocks are always appended to divEdit
                        if (bFloatLeftTableOpen == true) {
                            //append the float table first because FloatBlocks are not allowed in tables
                            divActive.append(kendo.format("<div id='divFloatTable-{0}' style='float: left;'></div>", floattablecount.toString()));
                            $floattable.append($floattableitemrow);
                            $("#divFloatTable-" + floattablecount.toString()).append($floattable);
                            bFloatLeftTableOpen = false
                        }
                        divEdit.append("<div id='div" + item.fieldTagID + "FloatBlock' style='float:left;'></div>");
                        divActive = $("#div" + item.fieldTagID + "FloatBlock");
                        domLastID = "div" + item.fieldTagID + "FloatBlock";
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.TableHeader) {
                        if (bFloatLeftTableOpen == true) {
                            bFloatHeadersOpen = true;
                            iTableHeaders = iTableHeaders + 1;
                            $floattableheaderrow.append("<th style='border:none;'>" + item.fieldCaption + "</th>");
                        }
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Label) {
                        if (item.fieldCssClass == 'NGLToolTip' && ngl.stringHasValue(item.fieldNGLToolTip) == true) {
                            tObj.NGLRespToolTip.addToolTip('td' + item.fieldTagID, $("#blueborder" + tObj.IDKey + "Edit"), item.fieldNGLToolTip);
                        }
                        if (bFloatLeftTableOpen == true) {
                            if (iTableColumns == iTableHeaders) {
                                iTableColumns = 0;
                                //close the old row and start a new one
                                if (bFloatHeadersOpen == true) {
                                    $floattable.append($floattableheaderrow);
                                    bFloatHeadersOpen = false;
                                }
                                $floattable.append($floattableitemrow);
                                $floattableitemrow = $("<tr></tr>");
                            }
                            iTableColumns = iTableColumns + 1;
                            //add the item to the  current Float Table row
                            $floattableitemrow.append("<td id='td" + item.fieldTagID + "' class='tblResponsive-top'>" + item.fieldCaption + "</td>");
                        } else {
                            //add a Responsive table with just a caption (no data) -- this should not be used
                            sFormated = kendo.format("<div id='div{0}' style='float: left;'><table class='tblResponsive'><tr><td id='td{0}' class='tblResponsive-top' style='max-width:200px; min-width:200px;'>{1}</td></tr><tr><td class='tblResponsive-top' style='max-width:200px; min-width:200px;'></td></tr></table></div>", item.fieldTagID, item.fieldCaption);
                            domLastID = "div" + item.fieldTagID;
                        }
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Checkbox) {
                        if (item.fieldCssClass == 'NGLToolTip' && ngl.stringHasValue(item.fieldNGLToolTip) == true) {
                            tObj.NGLRespToolTip.addToolTip('td' + item.fieldTagID, $("#blueborder" + tObj.IDKey + "Edit"), item.fieldNGLToolTip);
                        }
                        if (bFloatLeftTableOpen == true) {
                            if (iTableColumns == iTableHeaders) {
                                iTableColumns = 0;
                                //close the old row and start a new one
                                if (bFloatHeadersOpen == true) {
                                    $floattable.append($floattableheaderrow);
                                    bFloatHeadersOpen = false;
                                }
                                $floattable.append($floattableitemrow);
                                $floattableitemrow = $("<tr></tr>");
                            }
                            iTableColumns = iTableColumns + 1;
                            //add the item to the  current Float Table row
                            $floattableitemrow.append(kendo.format("<td id='td{3}' class='tblResponsive-top'>{0}{2}<input id='{3}' type='checkbox'  /></td>",
                                sReadOnly, item.fieldCaption, sRequiredAsterik, item.fieldTagID));
                        } else {
                            //add a Responsive table
                            sFormated = kendo.format(" <div id='div{3}' style='float: left;'><table class='tblResponsive'><tr><td class='tblResponsive-top' style='max-width:200px; min-width:200px;'>{0}{1}{2}</td></tr><tr><td id='td{3}' class='tblResponsive-top' style='max-width:200px; min-width:200px;'><input type='checkbox' id='{3}' class='k-checkbox' /></td></tr></table></div>",
                                sReadOnly, item.fieldCaption, sRequiredAsterik, item.fieldTagID);
                            domLastID = "div" + item.fieldTagID;
                        }
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.kendoSwitch) {
                        if (item.fieldCssClass == 'NGLToolTip' && ngl.stringHasValue(item.fieldNGLToolTip) == true) {
                            tObj.NGLRespToolTip.addToolTip('td' + item.fieldTagID, $("#blueborder" + tObj.IDKey + "Edit"), item.fieldNGLToolTip);
                        }
                        if (bFloatLeftTableOpen == true) {
                            if (iTableColumns == iTableHeaders) {
                                iTableColumns = 0;
                                //close the old row and start a new one
                                if (bFloatHeadersOpen == true) {
                                    $floattable.append($floattableheaderrow);
                                    bFloatHeadersOpen = false;
                                }
                                $floattable.append($floattableitemrow);
                                $floattableitemrow = $("<tr></tr>");
                            }
                            iTableColumns = iTableColumns + 1;
                            //add the item to the  current Float Table row
                            $floattableitemrow.append(kendo.format("<td  id='td{3}' class='tblResponsive-top'>{0}{2}<input id='{3}' type='checkbox' class='k-checkbox' /></td>",
                                sReadOnly, item.fieldCaption, sRequiredAsterik, item.fieldTagID));
                        } else {
                            sFormated = kendo.format("<div id='div{3}' style='float: left;'><table class='tblResponsive'><tr><td class='tblResponsive-top' style='max-width:200px; min-width:200px;'>{0}{1}{2}</td></tr><tr><td  id='td{3}' class='tblResponsive-top' style='max-width:200px; min-width:200px;'><input type='checkbox' id='{3}' class='k-checkbox' /></td></tr></table></div>",
                                sReadOnly, item.fieldCaption, sRequiredAsterik, item.fieldTagID);
                            domLastID = "div" + item.fieldTagID;
                        }
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Editor) {
                        if (item.fieldCssClass == 'NGLToolTip' && ngl.stringHasValue(item.fieldNGLToolTip) == true) {
                            tObj.NGLRespToolTip.addToolTip('td' + item.fieldTagID, $("#blueborder" + tObj.IDKey + "Edit"), item.fieldNGLToolTip);
                        }

                        var sMaxLength = '';
                        if (typeof (item.fieldMaxlength) !== 'undefined' && item.fieldMaxlength !== null && isEmpty(item.fieldMaxlength) === false && isNaN(item.fieldMaxlength) === false) {
                            var sMaxLength = " maxlength='" + item.fieldMaxlength + "' ";
                        }
                        if (bFloatLeftTableOpen == true) {
                            if (iTableColumns == iTableHeaders) {
                                iTableColumns = 0;
                                //close the old row and start a new one
                                if (bFloatHeadersOpen == true) {
                                    $floattable.append($floattableheaderrow);
                                    bFloatHeadersOpen = false;
                                }
                                $floattable.append($floattableitemrow);
                                $floattableitemrow = $("<tr></tr>");
                            }
                            iTableColumns = iTableColumns + 1;
                            //add the item to the  current Float Table row
                            $floattableitemrow.append(kendo.format("<td  id='td{3}' class='tblResponsive-top'>{0}{2}<textarea rows='2' cols='75' {4} id='{3}' style='height:50px;'></textarea></td>",
                                sReadOnly, item.fieldCaption, sRequiredAsterik, item.fieldTagID, sMaxLength));
                        } else {
                            sFormated = kendo.format("<div id='div{3}' style='float: left;'><table class='tblResponsive'><tr><td class='tblResponsive-top' style='max-width:200px; min-width:200px;'>{0}{1}{2}</td></tr><tr><td  id='td{3}' class='tblResponsive-top' style='max-width:200px; min-width:200px;'><textarea rows='2' cols='75' {4} id='{3}' style='height:50px;'></textarea></td></tr></table></div>",
                                sReadOnly, item.fieldCaption, sRequiredAsterik, item.fieldTagID, sMaxLength);
                            domLastID = "div" + item.fieldTagID;
                        }
                    } else if (typeof (item.fieldMaxlength) !== 'undefined' && item.fieldMaxlength !== null && isEmpty(item.fieldMaxlength) === false && isNaN(item.fieldMaxlength) === false && item.fieldMaxlength > 150) {
                        if (item.fieldCssClass == 'NGLToolTip' && ngl.stringHasValue(item.fieldNGLToolTip) == true) {
                            tObj.NGLRespToolTip.addToolTip('td' + item.fieldTagID, $("#blueborder" + tObj.IDKey + "Edit"), item.fieldNGLToolTip);
                        }
                        var sMaxLength = " maxlength='" + item.fieldMaxlength + "' ";

                        if (bFloatLeftTableOpen == true) {
                            if (iTableColumns == iTableHeaders) {
                                iTableColumns = 0;
                                //close the old row and start a new one
                                if (bFloatHeadersOpen == true) {
                                    $floattable.append($floattableheaderrow);
                                    bFloatHeadersOpen = false;
                                }
                                $floattable.append($floattableitemrow);
                                $floattableitemrow = $("<tr></tr>");
                            }
                            iTableColumns = iTableColumns + 1;
                            //add the item to the  current Float Table row
                            $floattableitemrow.append(kendo.format("<td id='td{3}' class='tblResponsive-top'>{0}{2}<textarea rows='2' cols='75' {4} id='{3}' style='height:50px;'></textarea></td>",
                                sReadOnly, item.fieldCaption, sRequiredAsterik, item.fieldTagID, sMaxLength));
                        } else {
                            sFormated = kendo.format("<div id='div{3}' style='float: left;'><table class='tblResponsive'><tr><td class='tblResponsive-top' style='max-width:200px; min-width:200px;'>{0}{1}{2}</td></tr><tr><td id='td{3}' class='tblResponsive-top' style='max-width:200px; min-width:200px;'><textarea rows='2' cols='75' {4} id='{3}' style='height:50px;'></textarea></td></tr></table></div>",
                                sReadOnly, item.fieldCaption, sRequiredAsterik, item.fieldTagID, sMaxLength);
                            domLastID = "div" + item.fieldTagID;
                        }

                    } else {
                        if (item.fieldCssClass == 'NGLToolTip' && ngl.stringHasValue(item.fieldNGLToolTip) == true) {
                            tObj.NGLRespToolTip.addToolTip('td' + item.fieldTagID, $("#blueborder" + tObj.IDKey + "Edit"), item.fieldNGLToolTip);
                        }
                        var sMaxLength = '';
                        if (typeof (item.fieldMaxlength) !== 'undefined' && item.fieldMaxlength !== null && isEmpty(item.fieldMaxlength) === false && isNaN(item.fieldMaxlength) === false) {
                            var sMaxLength = " maxlength='" + item.fieldMaxlength + "' ";
                        }
                        if (bFloatLeftTableOpen == true) {
                            if (iTableColumns == iTableHeaders) {
                                iTableColumns = 0;
                                //close the old row and start a new one
                                if (bFloatHeadersOpen == true) {
                                    $floattable.append($floattableheaderrow);
                                    bFloatHeadersOpen = false;
                                }
                                $floattable.append($floattableitemrow);
                                $floattableitemrow = $("<tr></tr>");
                            }
                            iTableColumns = iTableColumns + 1;
                            //add the item to the  current Float Table row
                            $floattableitemrow.append(kendo.format("<td id='td{3}'  class='tblResponsive-top'>{0}{2}<input id='{3}' {4} /></td>",
                                sReadOnly, item.fieldCaption, sRequiredAsterik, item.fieldTagID, sMaxLength));
                        } else {
                            sFormated = kendo.format("<div id='div{3}' style='float: left;'><table class='tblResponsive'><tr><td class='tblResponsive-top' style='max-width:200px; min-width:200px;'>{0}{1}{2}</td></tr><tr><td id='td{3}'  class='tblResponsive-top' style='max-width:200px; min-width:200px;'><input id='{3}' {4} /></td></tr></table></div>",
                                sReadOnly, item.fieldCaption, sRequiredAsterik, item.fieldTagID, sMaxLength);
                            domLastID = "div" + item.fieldTagID;
                        }
                    }
                    if (isEmpty(sFormated) === false) {
                        if (item.fieldInsertOnly === true) {
                            $InsertOnlyHTML.append(sFormated);
                        } else {
                            divActive.append(sFormated);
                        }
                    }
                }

            });
            if (bFloatLeftTableOpen == true) {
                //append the open float table to the active div and create a new float table
                //float tables are always nested inside a <div style="float: left;">
                divActive.append(kendo.format("<div id='divFloatTable-{0}' style='float: left;'></div>", floattablecount.toString()));
                $floattable.append($floattableitemrow);
                $("#divFloatTable-" + floattablecount.toString()).append($floattable);
            }
            //divEdit.append($InsertOnlyHTML) ;   
            divWrapper.append($HiddenFields);
            var sHT = $("#" + this.ContainerDivID).html();


        };

    };

    this.loadKendo = function () {
        if (typeof (this.dataFields) !== 'undefined' && ngl.isObject(this.dataFields) === true && ngl.isArray(this.dataFields) === true) {
            var tObj = this;
            this.blnEditing = false;
            //if we have data we have an existing record to edit so set blnEditing to true
            if (typeof (this.data) !== 'undefined' && ngl.isObject(this.data)) {
                this.blnEditing = true;
            };
            $.each(this.dataFields, function (index, item) {
                //if (item.fieldAllowNull === false) { item.fieldRequired = true; }
                if (tObj.blnEditing === true && ((item.fieldVisible === false && item.fieldReadOnly === true) || (item.fieldVisible === false && (item.fieldRequired === false || item.fieldInsertOnly === true)))) {
                    //when editing no kindo widgets for these hidden where visible is false and readonly is true, or where visible is false and (required is false or insert only is true)
                } else if (tObj.blnEditing === false && ((item.fieldVisible === false && item.fieldReadOnly === true) || (item.fieldVisible === false && item.fieldInsertOnly === false && item.fieldRequired === false) || (item.fieldVisible === true && item.fieldReadOnly === true && item.fieldInsertOnly === false))) {
                    //when adding new records  no kindo widgets for fields where visible is false and readonly is true, or where visible is false and (required is false or insert only is true) or where visible = true and readonly = true and insertonly = false
                } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.kendoSwitch) {
                    var nglkendoitem = $("#" + item.fieldTagID).data("kendoSwitch");
                    if (!nglkendoitem) {
                        $("#" + item.fieldTagID).kendoSwitch();
                    }
                } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.NumericTextBox) {
                    var nglkendoitem = $("#" + item.fieldTagID).data("kendoNumericTextBox");
                    if (!nglkendoitem) {
                        if (typeof (item.fieldFormat) !== 'undefined' && item.fieldFormat !== null && isEmpty(item.fieldFormat) === false) {
                            $("#" + item.fieldTagID).kendoNumericTextBox({ format: item.fieldFormat, decimals: 6 });
                        } else {
                            $("#" + item.fieldTagID).kendoNumericTextBox({ decimals: 6 });
                        };
                    }
                } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.DatePicker) {
                    //debugger; console.log(item);
                    var nglkendoitem = $("#" + item.fieldTagID).data("kendoDatePicker");
                    if (!nglkendoitem) {
                        if (typeof (item.fieldFormat) !== 'undefined' && item.fieldFormat !== null && isEmpty(item.fieldFormat) === false) {
                            $("#" + item.fieldTagID).kendoDatePicker({ format: item.fieldFormat });
                        } else {
                            $("#" + item.fieldTagID).kendoDatePicker();
                        };
                    }
                } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.DateTimePicker) {
                    var nglkendoitem = $("#" + item.fieldTagID).data("kendoDateTimePicker");
                    if (!nglkendoitem) {
                        if (typeof (item.fieldFormat) !== 'undefined' && item.fieldFormat !== null && isEmpty(item.fieldFormat) === false) {
                            $("#" + item.fieldTagID).kendoDateTimePicker({ format: item.fieldFormat, interval: 15 });
                        } else {
                            $("#" + item.fieldTagID).kendoDateTimePicker({ format: "HH:mm ", interval: 15 });
                        };
                    }
                } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.TimePicker) {
                    var nglkendoitem = $("#" + item.fieldTagID).data("kendoTimePicker");
                    if (!nglkendoitem) {
                        // Modified by RHR for v-8.5.2.006 on 12/28/2022 add default format and interval for kendoTimePicker
                        if (typeof (item.fieldFormat) !== 'undefined' && item.fieldFormat !== null && isEmpty(item.fieldFormat) === false) {
                            $("#" + item.fieldTagID).kendoTimePicker({ format: item.fieldFormat, interval: 5 });
                        } else {
                            $("#" + item.fieldTagID).kendoTimePicker({ format: 'HH:mm', interval: 5 });
                        };
                    }
                } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Editor) {
                    var nglkendoitem = $("#" + item.fieldTagID).data("kendoEditor");
                    if (!nglkendoitem) {
                        $("#" + item.fieldTagID).kendoEditor({
                            resizable: {
                                content: false,
                                toolbar: false

                            },
                            // Empty tools so do not display toolbar
                            tools: []
                        });
                    }
                } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.ColorPicker) {
                    var nglkendoitem = $("#" + item.fieldTagID).data("kendoColorPicker");
                    if (!nglkendoitem) {
                        $("#" + item.fieldTagID).kendoColorPicker();
                    }
                } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.DropDownList) {
                    var nglkendoitem = $("#" + item.fieldTagID).data("kendoDropDownList");
                    if (!nglkendoitem) {
                        $("#" + item.fieldTagID).kendoDropDownList({
                            dataTextField: "Name",
                            dataValueField: "Control",
                            autoWidth: true,
                            dataSource: {
                                transport: {
                                    read: function (options) {
                                        $.ajax({
                                            async: true,
                                            type: "GET",
                                            url: "api/" + item.fieldAPIReference + "/" + item.fieldAPIFilterID,
                                            contentType: "application/json; charset=utf-8",
                                            dataType: 'json',
                                            headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                            success: function (data) {
                                                options.success(data);

                                            },
                                            error: function (xhr, textStatus, error) {
                                                var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Get " + item.fieldName + "  Failure");
                                                ngl.showErrMsg("Read " + item.fieldName + " Error", sMsg, null);
                                            }
                                        });
                                    }
                                },
                                schema: {
                                    data: "Data",
                                    total: "Count",
                                    model: {
                                        id: "Control",
                                        fields: {
                                            Name: { editable: false },
                                            Description: { editable: false }
                                        }
                                    },
                                    errors: "Errors"
                                },
                                error: function (e) {
                                    ngl.showErrMsg("Read " + item.fieldName + " Error", e.errors, null);
                                    this.cancelChanges();
                                },
                                serverPaging: false,
                                serverSorting: false,
                                serverFiltering: false
                            },
                            dataBound: function (e) {
                                var listContainer = e.sender.list.closest(".k-list-container");
                                var iNewWidth = listContainer.width() + 25;
                                listContainer.width(iNewWidth);

                            },
                        });

                        var localDropDown = $("#" + item.fieldTagID).data("kendoDropDownList");
                        localDropDown.list.width("auto");
                    } else {
                        nglkendoitem.dataSource.read();
                    }
                } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.MaskedTextBox) {
                    var nglkendoitem = $("#" + item.fieldTagID).data("kendoMaskedTextBox");
                    if (!nglkendoitem) {
                        if (typeof (item.fieldFormat) !== 'undefined' && item.fieldFormat !== null && isEmpty(item.fieldFormat) === false) {
                            $("#" + item.fieldTagID).kendoMaskedTextBox({ mask: item.fieldFormat });
                        } else {
                            $("#" + item.fieldTagID).kendoMaskedTextBox();
                        };
                    }
                };
            });
        };
    };

    //Generic Call back functions for all Widgets
    this.saveSuccessCallback = function (data) {
        //debugger;
        var oResults = new nglEventParameters();
        oResults.source = "saveSuccessCallback";
        var tObj = this;
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed   
        //kendo.ui.progress($(document.body), false);       
        oResults.CRUD = this.CRUD;
        this.rData = null;
        try {
            kendo.ui.progress(tObj.kendoWindowsObj.element, false);
        } catch (err) {
            ngl.showInfoNotification("Cancel Progress Failure", "Your process is complete but the wait cursor could not be turned off, please refresh the page if the wait cursor does not go away.", null)
        }
        try {
            var blnSuccess = false;
            var blnErrorShown = false;
            var strValidationMsg = "";
            //debugger;
            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                    oResults.error = new Error();
                    oResults.error.name = "Save " + this.DataSourceName + " Failure";
                    oResults.error.message = data.Errors;
                    blnErrorShown = true;
                    ngl.showErrMsg(oResults.error.name, oResults.error.message, null);
                }
                else {
                    this.rData = data.Data;
                    if (typeof (data.Data) !== 'undefined' && data.Data != null && ngl.isArray(data.Data) && data.Data[0] == true) {
                        blnSuccess = true;
                        oResults.datatype = "bool";
                        oResults.data = data.Data[0];
                        oResults.msg = "Success"
                        ngl.showSuccessMsg("Success your " + this.DataSourceName + " changes have been saved", null);


                        //Only close the window if the save was successful
                        this.kendoWindowsObj.close();

                        //ngl.showSuccessMsg(oResults.msg, tObj);
                    } else {
                        blnErrorShown = true;
                        if (typeof (data.Errors) !== 'undefined' && ngl.stringHasValue(data.Errors)) {
                            oResults.error = data.Data[0].ErrMsg;
                            ngl.showErrMsg(this.DataSourceName, data.Errors, tObj);
                        } else {
                            oResults.error = new Error();
                            oResults.error.name = "Unable to save your " + this.DataSourceName + " changes";
                            oResults.error.message = "The save procedure returned false, please refresh your data and try again.";
                            ngl.showErrMsg(oResults.error.name, oResults.error.message, null);
                        }
                    }
                    if (typeof (data.Warnings) !== 'undefined' && ngl.stringHasValue(data.Warnings)) { ngl.showWarningMsg("", data.Warnings, tObj); }
                    if (typeof (data.Messages) !== 'undefined' && ngl.stringHasValue(data.Messages)) { ngl.showWarningMsg("", data.Messages, tObj); }

                    try {
                        //Modified by RHR for v-8.5.3.006 on 10/29/2022 added logic to determine if the parent grid ID key is valid
                        // on nested tab controls
                        if (this.ctrlKendoNGLGrid) {
                            if (this.ctrlKendoNGLGrid.dataSource) {
                                this.ctrlKendoNGLGrid.dataSource.read();
                            }
                        } else if (typeof (tObj.ParentIDKey) !== 'undefined' && tObj.ParentIDKey !== null) {
                            var grid = $("#" + tObj.ParentIDKey).data("kendoNGLGrid");

                            if (grid) {
                                if (tObj.blnEditing == false) {
                                    grid.dataSource.page(1);
                                }
                                grid.dataSource.read();
                            }
                        }
                    } catch (err) {
                        ngl.showErrMsg(err.name, err.message, null);

                    }
                }
            }

            if (blnSuccess === false) {
                if (this.blnEditing === false) {
                    //if we are adding a new record we need to clear the data and start over on the save 
                    //with the new record logic.
                    this.data = undefined;
                };
                if (blnErrorShown === false) {
                    oResults.error = new Error();
                    oResults.error.name = "Save " + this.DataSourceName + " Failure";
                    oResults.error.message = "No results are available.";
                    ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                };
            };
        } catch (err) {
            oResults.error = err;
            ngl.showErrMsg(err.name, err.message, null);

        }
        if (ngl.isFunction(tPage[this.callback])) {
            tPage[this.callback](oResults);
        }

    }

    this.saveAjaxErrorCallback = function (xhr, textStatus, error) {
        var oResults = new nglEventParameters();
        var tObj = this;
        try {
            kendo.ui.progress(tObj.kendoWindowsObj.element, false);
        } catch (err) {
            ngl.showInfoNotification("Cancel Progress Failure", "Your process is complete but the wait cursor could not be turned off, please refresh the page if the wait cursor does not go away.", null)
        }
        //kendo.ui.progress($(document.body), false);
        oResults.source = "saveAjaxErrorCallback";
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed         
        oResults.CRUD = this.CRUD;
        oResults.error = new Error();
        oResults.error.name = "Save " + this.DataSourceName + " Failure"
        oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);

        if (ngl.isFunction(tPage[this.callback])) {
            tPage[this.callback](oResults);
        }
    }

    this.readSuccessCallback = function (data) {
        //debugger;
        var oResults = new nglEventParameters();
        var tObj = this;
        oResults.source = "readSuccessCallback";
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed         
        oResults.CRUD = "read"
        //clear any old return data in rData
        this.rData = null;
        try {
            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                if (typeof (data.Errors) !== 'undefined' && data.Errors != null) {
                    oResults.error = new Error();
                    oResults.error.name = "Read " + this.DataSourceName + " Failure";
                    oResults.error.message = data.Errors;
                    ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                }
                else {
                    this.rData = data.Data;
                    if (data.Data != null) {
                        oResults.data = data.Data;
                        oResults.datatype = this.DataType;
                        oResults.msg = "Success"
                        this.data = data.Data[0];
                        //this.show();
                    }
                    else {
                        oResults.error = new Error();
                        oResults.error.name = "Invalid Request";
                        oResults.error.message = "No Data available.";
                        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                    }
                }
            } else {
                oResults.msg = "Success but no data was returned. Please refresh your page and try again.";
                ngl.showSuccessMsg(oResults.msg, tObj);
            }
        } catch (err) {
            oResults.error = err
        }
        if (ngl.isFunction(tPage[this.callback])) {
            tPage[this.callback](oResults);
        }
        if (oResults.msg == "Success") {
            this.show();
        }

    }

    this.readAjaxErrorCallback = function (xhr, textStatus, error) {
        //debugger;
        var oResults = new nglEventParameters();
        var tObj = this;
        oResults.source = "readAjaxErrorCallback";
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed        
        oResults.CRUD = "read"
        oResults.error = new Error();
        oResults.error.name = "Read " + this.DataSourceName + " Failure"
        oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);

        if (ngl.isFunction(tPage[this.callback])) {
            tPage[this.callback](oResults);
        }
    }

    this.deleteSuccessCallback = function (data) {

        var oResults = new nglEventParameters();
        var tObj = this;
        oResults.source = "deleteSuccessCallback";
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed  
        oResults.CRUD = "delete"
        var parentGrid = $('#' + this.ParentIDKey).data("kendoNGLGrid");
        if (typeof (parentGrid) !== 'undefined' && ngl.isObject(parentGrid)) {
            kendo.ui.progress(parentGrid.element, false);
        }
        //clear any old return data in rData
        this.rData = null;
        try {
            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                if (typeof (data.Errors) !== 'undefined' && data.Errors != null) {
                    oResults.error = new Error();
                    oResults.error.name = "Delete " + this.DataSourceName + " Failure";
                    oResults.error.message = data.Errors;
                    ngl.showErrMsg(oResults.error.name, oResults.error.message, null);
                } else {
                    if (typeof (data.Data) !== 'undefined' && data.Data != null && ngl.isArray(data.Data) && data.Data[0] == true) {
                        oResults.datatype = "bool";
                        oResults.data = data.Data[0];
                        oResults.msg = "Success"
                        ngl.showSuccessMsg("Success your " + this.DataSourceName + " record has been deleted", tObj);
                    }
                    else {
                        oResults.error = new Error();
                        oResults.error.name = "Unable to delete your " + this.DataSourceName + " record";
                        oResults.error.message = "The delete procedure returned false, please refresh your data and try again.";
                        ngl.showErrMsg(oResults.error.name, oResults.error.message, null);
                    }

                    try {
                        //Modified by RHR for v - 8.5.3.006 on 10 / 29 / 2022 added logic to determine if the parent grid ID key is valid
                        // on nested tab controls
                        if (this.ctrlKendoNGLGrid) {
                            if (this.ctrlKendoNGLGrid.dataSource) {
                                this.ctrlKendoNGLGrid.dataSource.read();
                            }
                        } else if (typeof (tObj.ParentIDKey) !== 'undefined' && tObj.ParentIDKey !== null) {
                            var grid = $("#" + tObj.ParentIDKey).data("kendoNGLGrid");
                            //Modified by RHR for v-8.5.3.006 on 10/29/2022 added logic to determine if the parent grid ID key is valid
                            // on nested tab controls                            
                            if (grid) {
                                grid.dataSource.page(1);
                                grid.dataSource.read();
                            }
                        }
                    } catch (err) {
                        ngl.showErrMsg(err.name, err.message, null);

                    }
                }
            } else {
                oResults.msg = "Success but no data was returned. Please refresh your page and check the results.";
                ngl.showSuccessMsg(oResults.msg, null);
            }
        } catch (err) {
            oResults.error = err
            ngl.showErrMsg(err.name, err.message, null);
        }
        if (ngl.isFunction(tPage[this.callback])) {
            tPage[this.callback](oResults);
        }

    }

    this.deleteAjaxErrorCallback = function (xhr, textStatus, error) {
        var oResults = new nglEventParameters();
        var tObj = this;
        var parentGrid = $('#' + this.ParentIDKey).data("kendoNGLGrid");
        if (typeof (parentGrid) !== 'undefined' && ngl.isObject(parentGrid)) {
            kendo.ui.progress(parentGrid.element, false);
        }
        oResults.source = "deleteAjaxErrorCallback";
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed
        oResults.CRUD = "delete"
        oResults.error = new Error();
        oResults.error.name = "Delete " + this.DataSourceName + " Failure"
        oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);

        if (ngl.isFunction(tPage[this.callback])) {
            tPage[this.callback](oResults);
        }
    }

    // Generic functions for all Widgets
    this.validateRequiredFields = function (data, editing) {
        var oRet = new validationResults();
        var tObj = this;
        if (typeof (data) === 'undefined' || !ngl.isObject(data)) {
            //something went wrong so throw an error. this should never happen
            ngl.showValidationMsg("Save " + this.DataSourceName + " Validation Failed; No Data", "Please Contact Technical Support", tObj);
            oRet.Message = "Save " + this.DataSourceName + " Validation Failed; No Data";
            return oRet;
        }

        var blnValidated = true;
        var sValidationMsg = "";
        var sSpacer = "";
        if (typeof (this.dataFields) !== 'undefined' && ngl.isObject(this.dataFields) === true && ngl.isArray(this.dataFields) === true) {
            this.blnEditing = false;
            //if (typeof (editing) === 'undefined') { this.blnEditing = false; } else { this.blnEditing = editing == "true"; }
            if (typeof (editing) === 'undefined') { this.blnEditing = false; } else { this.blnEditing = editing; }

            $.each(this.dataFields, function (index, item) {
                if (item.fieldName in data) {
                    if (
                        (tObj.blnEditing === true && ((item.fieldVisible === false && item.fieldReadOnly === true) || (item.fieldVisible === false && (item.fieldRequired === false || item.fieldInsertOnly === true))))
                        ||
                        (tObj.blnEditing === false && ((item.fieldVisible === false && item.fieldReadOnly === true) || (item.fieldVisible === false && item.fieldInsertOnly === false && item.fieldRequired === false) || (item.fieldVisible === true && item.fieldReadOnly === true && item.fieldInsertOnly === false)))
                    ) {
                        //valid 
                    } else {
                        if (item.fieldRequired === true) {
                            var field = data[item.fieldName];
                            // Modified by RHR for v-8.5.3.007 when a field is required it cannot be null or undefined
                            if (typeof (field) === 'undefined' || field === null) {
                                blnValidated = false; sValidationMsg += sSpacer + item.fieldCaption; sSpacer = ", ";
                            }
                            else {

                                if (isEmpty(field) === true) { blnValidated = false; sValidationMsg += sSpacer + item.fieldCaption; sSpacer = ", "; }
                            }
                        }
                    }
                }
                //if (tObj.blnEditing === true) { //update changes
                //    if (item.fieldVisible === false && (item.fieldRequired === false || item.fieldInsertOnly === true)) {
                //        //valid
                //    } else  if ((item.fieldAllowNull === false || item.fieldRequired === true)  && (item.fieldName in data)  ) {
                //        var field = data[item.fieldName];                       
                //        if (isEmpty(field)) { blnValidated = false; sValidationMsg += sSpacer + item.fieldCaption; sSpacer = ", "; }
                //    }
                //} else {
                //    if (item.fieldVisible === false  && item.fieldInsertOnly === false && item.fieldRequired === false ) {
                //        //valid
                //    } else if ((item.fieldAllowNull === false || item.fieldRequired === true )  && (item.fieldName in data)  )  { 
                //        var field = data[item.fieldName];                       
                //        if (isEmpty(field)) { blnValidated = false; sValidationMsg += sSpacer + item.fieldCaption; sSpacer = ", "; }
                //    }
                //}

            });
        }


        oRet.Success = blnValidated;
        oRet.Message = sValidationMsg;

        return oRet;
    }

    // Generic CRUD functions for all Widgets
    this.save = function () {
        var tObj = this;
        var otmp = $("#" + this.IDKey + "focusCancel").focus();
        if (typeof (this.dataFields) !== 'undefined' && ngl.isObject(this.dataFields) === true && ngl.isArray(this.dataFields) === true) {
            this.blnEditing = false;

            //if we have data we have an existing record to edit so set blnEditing to true
            if (typeof (this.data) !== 'undefined' && ngl.isObject(this.data)) {
                this.blnEditing = true;
            };
            if (this.blnEditing == false) {
                this.CRUD = "create"
            } else {
                this.CRUD = "update"
            }
            this.data = new window[this.DataType]; //clear any old data
            //read the fields
            $.each(tObj.dataFields, function (index, item) {
                var field = tObj.data[item.fieldName];
                //console.log("field: " + field);
                //if (item.fieldAllowNull === false) { item.fieldRequired = true; };

                if (
                    (tObj.blnEditing === true && ((item.fieldVisible === false && item.fieldReadOnly === true) || (item.fieldVisible === false && (item.fieldRequired === false || item.fieldInsertOnly === true))))
                    ||
                    (tObj.blnEditing === false && ((item.fieldVisible === false && item.fieldReadOnly === true) || (item.fieldVisible === false && item.fieldInsertOnly === false && item.fieldRequired === false) || (item.fieldVisible === true && item.fieldReadOnly === true && item.fieldInsertOnly === false)))
                ) {
                    //hidden fields
                    var domHidden = $("#" + item.fieldTagID)
                    if (typeof (domHidden) !== 'undefined' && ngl.isObject(domHidden)) {
                        var domVal = domHidden.val();
                        if (item.fieldGroupSubType === nglGroupSubTypeEnum.NumericTextBox) {
                            field = ngl.nbrTryParse(domVal, ngl.nbrTryParse(item.fieldDefaultValue, 0));
                        } else {
                            field = domVal;
                        };
                    } else {
                        field = item.fieldDefaultValue;
                    }
                } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Checkbox) {
                    //var kswitch = $("#" + item.fieldTagID).data("kendoSwitch");
                    //var bchecked = kswitch.check();                   
                    //field = bchecked;
                    field = $("#" + item.fieldTagID).prop("checked");
                } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.kendoSwitch) {
                    //var kswitch = $("#" + item.fieldTagID).data("kendoSwitch");
                    //var bchecked = kswitch.check();                   
                    //field = bchecked;
                    field = $("#" + item.fieldTagID).data("kendoSwitch").check();
                } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.NumericTextBox) {
                    //debugger;
                    //console.log("item: " );
                    //console.log(item);
                    // modified 08/28/2023 for bug testing
                    var numVal = "0";
                    var tItemelement = $("#" + item.fieldTagID).data("kendoNumericTextBox");
                    if (tItemelement) {
                        numVal = $("#" + item.fieldTagID).data("kendoNumericTextBox").value();
                    } else {

                        numVal = $("#" + item.fieldTagID).val();
                    }
                    field = ngl.nbrTryParse(numVal, ngl.nbrTryParse(item.fieldDefaultValue, 0));
                    //console.log("t field: " + field);
                } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.DatePicker) {
                    //debugger; console.log(item);
                    // Modified by RHR for v-8.5.2.006 on 12/28/2022 fix for UTC conversion of KendoDatePicker  data to JSON string
                    //  removes the Location Key so time does not change when sent to server.
                    field = ngl.convertDateTimeToDateString($("#" + item.fieldTagID).data("kendoDatePicker").value(), null, null); //$("#" + item.fieldTagID).data("kendoDatePicker").value();
                } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.DateTimePicker) {
                    // Modified by RHR for v-8.5.2.006 on 12/28/2022 fix for UTC conversion of KendoDatePicker  data to JSON string
                    //  removes the Location Key so time does not change when sent to server.
                    field = ngl.convertDateTimeToDateString($("#" + item.fieldTagID).data("kendoDateTimePicker").value(), null, null);
                    //field = $("#" + item.fieldTagID).data("kendoDateTimePicker").value();
                } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.TimePicker) {
                    field = ngl.convertTimePickerToDateString($("#" + item.fieldTagID).data("kendoTimePicker").value(), null, null);
                } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Editor) {
                    field = $("#" + item.fieldTagID).data("kendoEditor").value();
                } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.ColorPicker) {
                    field = $("#" + item.fieldTagID).data("kendoColorPicker").value();
                } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.DropDownList) {
                    field = $("#" + item.fieldTagID).data('kendoDropDownList').value();
                } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.MaskedTextBox) {
                    if ($("#" + item.fieldTagID).data('kendoMaskedTextBox')) {
                        field = $("#" + item.fieldTagID).data('kendoMaskedTextBox').value();
                    }
                } else {
                    var sVal = $("#" + item.fieldTagID).val();
                    if (sVal) { field = sVal; }
                };
                tObj.data[item.fieldName] = field;
            });
            var oValidationResults = this.validateRequiredFields(this.data, this.blnEditing);
            //console.log("oValidationResults: " + oValidationResults);
            if (typeof (oValidationResults) === 'undefined' || !ngl.isObject(oValidationResults)) {
                ngl.showErrMsg("Cannot validate " + this.DataSourceName + " Information", "Invalid validation procedure, please contact technical support", tObj);
                return;
            } else {
                if (oValidationResults.Success === false) {
                    var wdth = ($(window).width() / 10) * 6;
                    var hgt = ($(window).height() / 10) * 6;
                    ngl.Alert("Required Fields", oValidationResults.Message, wdth, hgt);
                    return;
                }
            }

            if (typeof (this.API) === 'undefined' || this.API === null) {
                //the caller saves the changes
                var oResults = new nglEventParameters();
                oResults.source = "saveDataCB";
                var tObj = this;
                oResults.widget = tObj;
                oResults.msg = 'Success'
                oResults.data = this.data;
                oResults.CRUD = this.CRUD
                if (ngl.isFunction(tPage[this.callback])) {
                    tPage[this.callback](oResults);
                }
                this.kendoWindowsObj.close();
            } else {

                //save the changes using the API
                //var windowWidget = this.kendoWindowsObj.data("kendoWindow");
                //kendo.ui.progress(windowWidget.element, true);
                var windowWidget = $("#wnd" + this.IDKey + "Edit").data("kendoWindow");

                kendo.ui.progress(tObj.kendoWindowsObj.element, true);

                // kendo.ui.progress(tObj.kendoWindowsObj, true);
                setTimeout(function (tObj) {
                    var oCRUDCtrl = new nglRESTCRUDCtrl();
                    if (oCRUDCtrl.update(tObj.API + "/POST", tObj.data, tObj, "saveSuccessCallback", "saveAjaxErrorCallback") == false) {
                        kendo.ui.progress(tObj.kendoWindowsObj.element, false);
                        //kendo.ui.progress(windowWidget.element, false);
                    }
                }, 2000, tObj);
            }
        };

    }

    this.show = function (fk) {
        var tObj = this;
        try {
            this.loadHTML();
            this.loadKendo();
            ngl.RunKendoIconFix();
            //var oNGLToolTips = this.NGLToolTips;            
            if (typeof (this.dataFields) === 'undefined' || ngl.isObject(this.dataFields) === false || ngl.isArray(this.dataFields) === false) { return; }
            //try{
            //    var dialog = $("#wnd" + this.IDKey + "Edit").data("kendoWindow");
            //    dialog.destroy();
            //    this.kendoWindowsObjUploadEventAdded = 0;
            //} catch (err) {
            //    //oResults.error = err
            //}
            //this.kendoWindowsObj = kendo.ui.Window;

            this.kendoWindowsObj = $("#wnd" + this.IDKey + "Edit").kendoWindow({
                title: "Add " + this.DataSourceName,
                modal: true,
                visible: false,
                height: '75%',
                width: '75%',
                actions: ["save", "Minimize", "Maximize", "Close"],
                close: function (e) { tObj.close(e); },
                deactivate: function () {
                    this.destroy();
                }
            }).data("kendoWindow");

            //this.kendoWindowsObj = $("#div" + this.IDKey).kendoWindow({
            //    title: "Add " + this.DataSourceName,
            //    modal: true,
            //    visible: false,
            //    height: '75%',
            //    width: '75%',
            //    actions: ["save", "Minimize", "Maximize", "Close"],
            //    close: function (e) { tObj.close(e); },
            //    deactivate: function () {
            //        this.destroy();
            //    }
            //}).data("kendoWindow");

            //if (this.kendoWindowsObjUploadEventAdded === 0) {
            try {
                $("#wnd" + this.IDKey + "Edit").data("kendoWindow").wrapper.find(".k-svg-i-save").parent().click(function (e) { e.preventDefault(); tObj.save(); });
            } catch (err) {
                ngl.showInfoNotification("Save not available", "Could not load save method please reload the page to save", null);
            }


            //}
            //this.kendoWindowsObjUploadEventAdded = 1;

            //if this is an edit load the data to the window
            if (typeof (this.data) !== 'undefined' && ngl.isObject(this.data)) {
                this.kendoWindowsObj.title("Edit " + this.DataSourceName);
            }
            this.edit(this.data, fk);
        } catch (err) {

            ngl.showErrMsg("Cannot show Window", err.message, null);

        }
        var parentGrid = $('#' + this.ParentIDKey).data("kendoNGLGrid");
        if (typeof (parentGrid) !== 'undefined' && ngl.isObject(parentGrid)) {
            kendo.ui.progress(parentGrid.element, false);
        }


        //this.kendoWindowsObj.refresh();
        if (this.EditError === false) {
            this.kendoWindowsObj.center().open();
            if (ngl.isFunction(tPage[this.callback])) {
                var oResults = new nglEventParameters();
                var tObj = this;
                oResults.source = "showWidgetCallback";
                oResults.widget = tObj;
                oResults.msg = 'Success';
                tPage[this.callback](oResults);
            }
        }
        // Modified by RHR for v-8.5.3.007 on 03/14/2023 added Class for NGLToolTip data        
        if (tObj.NGLRespToolTip) { tObj.NGLRespToolTip.addEvents(); }

    }

    this.read = function (intControl) {
        //debugger;
        var blnRet = true;
        var tObj = this;
        if (typeof (intControl) != 'undefined' && intControl != null) {
            this.rData = null;
            this.data = null;
            //save the changes
            //var windowWidget = this.kendoWindowsObj.data("kendoWindow");
            //kendo.ui.progress(windowWidget.element, true);
            //kendo.ui.progress(this.kendoWindowsObj.element, true);
            setTimeout(function (tObj) {
                var oCRUDCtrl = new nglRESTCRUDCtrl();
                // Modified by RHR for v-8.5.2.007 on 04/24/2023
                // added logic to test for string primary key and call
                // filtered Read Method
                if (isNaN(intControl)) {
                    if (oCRUDCtrl.filteredRead(tObj.API + "/GetRecord", intControl, tObj, "readSuccessCallback", "readAjaxErrorCallback") == false) {
                        blnRet = false;
                        //kendo.ui.progress(tObj.kendoWindowsObj.element, false);
                        //kendo.ui.progress(windowWidget.element, false);
                    }

                } else {
                    if (oCRUDCtrl.read(tObj.API + "/GET", intControl, tObj, "readSuccessCallback", "readAjaxErrorCallback") == false) {
                        blnRet = false;
                        //kendo.ui.progress(tObj.kendoWindowsObj.element, false);
                        //kendo.ui.progress(windowWidget.element, false);
                    }
                }
            }, 2000, tObj);
        }
        return blnRet;
    }

    this.delete = function (intControl) {
        var blnRet = true;
        var tObj = this;
        if (typeof (intControl) != 'undefined' && intControl != null) {
            this.rData = null;
            this.data = null;

            var parentGrid = $('#' + this.ParentIDKey).data("kendoNGLGrid");
            if (typeof (parentGrid) !== 'undefined' && ngl.isObject(parentGrid)) {
                kendo.ui.progress(parentGrid.element, true);
            }
            setTimeout(function (tObj) {
                var oCRUDCtrl = new nglRESTCRUDCtrl();
                // Modified by RHR for v-8.5.2.007 on 04/24/2023
                // added logic to test for string primary key and call
                // filtered Read Method
                if (isNaN(intControl)) {
                    if (oCRUDCtrl.filteredDelete(tObj.API + "/DeleteRecord", intControl, tObj, "deleteSuccessCallback", "deleteAjaxErrorCallback") == false) {
                        blnRet = false;
                        var parentGrid = $('#' + tObj.ParentIDKey).data("kendoNGLGrid");
                        if (typeof (parentGrid) !== 'undefined' && ngl.isObject(parentGrid)) {
                            kendo.ui.progress(parentGrid.element, false);
                        }
                    }

                } else {
                    if (oCRUDCtrl.delete(tObj.API + "/DELETE", intControl, tObj, "deleteSuccessCallback", "deleteAjaxErrorCallback") == false) {
                        blnRet = false;
                        var parentGrid = $('#' + tObj.ParentIDKey).data("kendoNGLGrid");
                        if (typeof (parentGrid) !== 'undefined' && ngl.isObject(parentGrid)) {
                            kendo.ui.progress(parentGrid.element, false);
                        }
                    }
                }
            }, 2000, tObj);
        }
        return blnRet;
    }

    this.edit = function (data, fk) {
        //load data to the screen
        var tObj = this;
        if (typeof (data) !== 'undefined' && data != null && ngl.isObject(data)) {
            $('#div' + this.IDKey + 'InsertOnly').hide();
            if (typeof (this.dataFields) !== 'undefined' && ngl.isObject(this.dataFields) === true && ngl.isArray(this.dataFields) === true) {
                $.each(this.dataFields, function (index, item) {
                    if (item.fieldName in data) {
                        var blnReadOnly = false;
                        if (item.fieldReadOnly === true) { blnReadOnly = true; }
                        var dataItem = data[item.fieldName];
                        if (typeof (dataItem) !== 'undefined' && dataItem !== null) {
                            if ((item.fieldVisible === false && item.fieldReadOnly === true) || (item.fieldVisible === false && (item.fieldRequired === false || item.fieldInsertOnly === true))) {
                                //no kindo widgets for hidden fields  
                                $("#" + item.fieldTagID).val(dataItem);
                            } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Checkbox) {
                                if (dataItem !== true) { dataItem = false; }
                                $("#" + item.fieldTagID).prop('checked', dataItem);
                                $("#" + item.fieldTagID).prop("disabled", blnReadOnly);
                            } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.kendoSwitch) {
                                if (dataItem !== true) { dataItem = false; }
                                $("#" + item.fieldTagID).data("kendoSwitch").check(dataItem);
                                //$("#" + item.fieldTagID).data("kendoSwitch").value({ checked: dataItem });
                                $("#" + item.fieldTagID).data("kendoSwitch").enable(!blnReadOnly);
                                //$("#" + item.fieldTagID).prop("data-enable", !blnReadOnly);
                                //$("#" + item.fieldTagID).data("kendoSwitch").enabled(!blnReadOnly);
                            } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.NumericTextBox) {
                                $("#" + item.fieldTagID).data("kendoNumericTextBox").value(dataItem);
                                $("#" + item.fieldTagID).data("kendoNumericTextBox").readonly(blnReadOnly);
                                //$("#" + item.fieldTagID).data("kendoNumericTextBox").spinners(!blnReadOnly);
                                //$("#" + item.fieldTagID).data("kendoNumericTextBox").enable(!blnReadOnly); 
                            } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.DatePicker) {
                                //debugger; console.log(item);
                                //Modified by RHR for v-8.5.4.004 on 12/12/2023 added logic to convert string to date
                                //  this works when format is MM/DD/YYYY  it may not work for other formats
                                //old code removed $("#" + item.fieldTagID).data("kendoDatePicker").value(dataItem);
                                $("#" + item.fieldTagID).data("kendoDatePicker").value(new Date(dataItem));
                                $("#" + item.fieldTagID).data("kendoDatePicker").readonly(blnReadOnly);
                                //$("#" +item.fieldTagID).data("kendoDatePicker").enable(!blnReadOnly);
                            } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.DateTimePicker) {
                                $("#" + item.fieldTagID).data("kendoDateTimePicker").value(dataItem);
                                $("#" + item.fieldTagID).data("kendoDateTimePicker").readonly(blnReadOnly);
                                //$("#" +item.fieldTagID).data("kendoDateTimePicker").enable(!blnReadOnly);
                            } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.TimePicker) {
                                // Modified by RHR for v-8.5.2.006 on 12/28/2022 call convertTime to fix issue with kendoTimePicker not loading time properly
                                $("#" + item.fieldTagID).data("kendoTimePicker").value(ngl.convertTime(dataItem));
                                //$("#" + item.fieldTagID).data("kendoTimePicker").value(ngl.getShortDateString(item.fieldDefaultValue, new Date())
                                $("#" + item.fieldTagID).data("kendoTimePicker").readonly(blnReadOnly);
                                //$("#" +item.fieldTagID).data("kendoTimePicker").enable(!blnReadOnly);
                            } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Editor) {
                                $("#" + item.fieldTagID).data("kendoEditor").value(dataItem);
                            } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.ColorPicker) {
                                $("#" + item.fieldTagID).data("kendoColorPicker").value({ value: dataItem });
                                $("#" + item.fieldTagID).data("kendoColorPicker").enabled(!blnReadOnly);
                            } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.DropDownList) {
                                $("#" + item.fieldTagID).data("kendoDropDownList").value(dataItem);
                                $("#" + item.fieldTagID).data("kendoDropDownList").readonly(blnReadOnly);
                            } else {
                                $("#" + item.fieldTagID).data("kendoMaskedTextBox").value(dataItem);
                                $("#" + item.fieldTagID).data("kendoMaskedTextBox").readonly(blnReadOnly);
                                //$("#" +item.fieldTagID).data("kendoMaskedTextBox").enable(!blnReadOnly);
                            };
                        };
                    };
                });
            } else {
                ngl.showValidationMsg(this.EditErrorTitle, this.EditErrorMsg, null); this.EditError = true; return false;
            };

        } else {

            //we are adding a new record
            if (typeof (this.dataFields) !== 'undefined' && ngl.isObject(this.dataFields) === true && ngl.isArray(this.dataFields) === true) {

                $('#div' + this.IDKey + 'InsertOnly').show();
                $.each(this.dataFields, function (index, item) {
                    var blnReadOnly = false;
                    if (item.fieldInsertOnly === false && item.fieldReadOnly === true && item.fieldRequired === false) { blnReadOnly = true; }
                    var blnHasDefaultVal = false;
                    if (typeof (item.fieldAPIReference) !== 'undefined' && item.fieldAPIReference !== null && item.fieldAPIReference == "fk" && typeof (fk) !== 'undefined' && fk !== null) {
                        blnHasDefaultVal = true;
                        item.fieldDefaultValue = fk;
                    } else {

                        if (typeof (item.fieldDefaultValue) !== 'undefined' && item.fieldDefaultValue !== null && isEmpty(item.fieldDefaultValue) === false) { blnHasDefaultVal = true; }
                    }
                    if ((item.fieldVisible === false && item.fieldReadOnly === true) || (item.fieldVisible === false && item.fieldInsertOnly === false && item.fieldRequired === false) || (item.fieldVisible === true && item.fieldReadOnly === true && item.fieldInsertOnly === false)) {
                        //no kindo widgets for hidden fields  
                        if (blnHasDefaultVal === true) {
                            $("#" + item.fieldTagID).val(item.fieldDefaultValue);
                        } else {
                            $("#" + item.fieldTagID).val();
                        };
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Checkbox) {
                        if (blnHasDefaultVal === true) {
                            if (item.fieldDefaultValue !== true) { item.fieldDefaultValue = false; }
                            $("#" + item.fieldTagID).prop('checked', item.fieldDefaultValue);
                        } else {
                            $("#" + item.fieldTagID).prop('checked', false);
                        }
                        $("#" + item.fieldTagID).prop("disabled", blnReadOnly);
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.kendoSwitch) {
                        if (blnHasDefaultVal === true) {
                            if (item.fieldDefaultValue !== true) { item.fieldDefaultValue = false; }
                            $("#" + item.fieldTagID).data("kendoSwitch").value({ checked: item.fieldDefaultValue });
                        } else {
                            $("#" + item.fieldTagID).data("kendoSwitch").value({ checked: false });
                        }
                        $("#" + item.fieldTagID).prop("data-enable", !blnReadOnly);
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.NumericTextBox) {
                        if (blnHasDefaultVal === true) {
                            $("#" + item.fieldTagID).data("kendoNumericTextBox").value(item.fieldDefaultValue);
                        } else {
                            $("#" + item.fieldTagID).data("kendoNumericTextBox").value();
                        }
                        $("#" + item.fieldTagID).data("kendoNumericTextBox").readonly(blnReadOnly);
                        //$("#" + item.fieldTagID).data("kendoNumericTextBox").spinners(!blnReadOnly); 
                        //$("#" + item.fieldTagID).data("kendoNumericTextBox").enable(!blnReadOnly); 
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.DatePicker) {
                        //debugger; console.log(item);
                        if (blnHasDefaultVal === true) {
                            $("#" + item.fieldTagID).data("kendoDatePicker").value(ngl.getShortDateString(item.fieldDefaultValue, new Date()));
                        } else {
                            $("#" + item.fieldTagID).data("kendoDatePicker").value();
                        }
                        $("#" + item.fieldTagID).data("kendoDatePicker").readonly(blnReadOnly);
                        //$("#" +item.fieldTagID).data("kendoDatePicker").enable(!blnReadOnly);
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.DateTimePicker) {
                        if (blnHasDefaultVal === true) {
                            $("#" + item.fieldTagID).data("kendoDateTimePicker").value(ngl.getShortDateString(item.fieldDefaultValue, new Date()));
                        } else {
                            $("#" + item.fieldTagID).data("kendoDateTimePicker").value();
                        }
                        $("#" + item.fieldTagID).data("kendoDateTimePicker").readonly(blnReadOnly);
                        //$("#" +item.fieldTagID).data("kendoDateTimePicker").enable(!blnReadOnly);
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.TimePicker) {
                        if (blnHasDefaultVal === true) {
                            $("#" + item.fieldTagID).data("kendoTimePicker").value(ngl.getShortDateString(item.fieldDefaultValue, new Date()));
                        } else {
                            $("#" + item.fieldTagID).data("kendoTimePicker").value();
                        }
                        $("#" + item.fieldTagID).data("kendoTimePicker").readonly(blnReadOnly);
                        //$("#" +item.fieldTagID).data("kendoTimePicker").enable(!blnReadOnly);
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Editor) {
                        if (blnHasDefaultVal === true) {
                            $("#" + item.fieldTagID).data("kendoEditor").value(item.fieldDefaultValue);
                        } else {
                            $("#" + item.fieldTagID).data("kendoEditor").value();
                        }
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.ColorPicker) {
                        if (blnHasDefaultVal === true) {
                            $("#" + item.fieldTagID).data("kendoColorPicker").value({ value: item.fieldDefaultValue });
                        } else {
                            $("#" + item.fieldTagID).data("kendoColorPicker").value();
                        }
                        $("#" + item.fieldTagID).data("kendoColorPicker").enabled(!blnReadOnly);
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.DropDownList) {
                        if (blnHasDefaultVal === true) {
                            $("#" + item.fieldTagID).data("kendoDropDownList").value(item.fieldDefaultValue);
                        } else {
                            $("#" + item.fieldTagID).data("kendoDropDownList").select(0);
                        }
                        $("#" + item.fieldTagID).data("kendoDropDownList").readonly(blnReadOnly);
                    } else {
                        if ($("#" + item.fieldTagID).data("kendoMaskedTextBox")) {
                            if (blnHasDefaultVal === true) {
                                $("#" + item.fieldTagID).data("kendoMaskedTextBox").value(item.fieldDefaultValue);
                            } else {
                                $("#" + item.fieldTagID).data("kendoMaskedTextBox").value();
                            }
                            $("#" + item.fieldTagID).data("kendoMaskedTextBox").readonly(blnReadOnly);
                            //$("#" +item.fieldTagID).data("kendoMaskedTextBox").enable(!blnReadOnly);
                        };
                    };
                });
            } else {
                ngl.showValidationMsg(this.AddErrorTitle, this.AddErrorMsg, null); this.EditError = true; return false;
            };
        };
        return true;
    }

    this.close = function (e) {


    }

    /// loadDefaults sets up the callbacks cbSelect and cbSave
    /// all call backs return a reference to this object and a string message as parameters
    /// CallBackFunction can be used by the caller to handle return information.  it returns a nglEventParameters objec
    this.loadDefaults = function (ContainerDivID,
        WindowPageVariable,
        IDKey,
        ParentIDKey,
        fieldData,
        Theme,
        CallBackFunction,
        API,
        DataType,
        DataSourceName,
        EditErrorMsg,
        EditErrorTitle,
        AddErrorMsg,
        AddErrorTitle,
        sNGLCtrlName) {

        var tObj = this;
        this.callback = CallBackFunction;
        this.dataFields = fieldData;
        this.lastErr = "";
        this.notifyOnError = true;
        this.notifyOnSuccess = true;
        this.notifyOnValidationFailure = true;
        this.ContainerDivID = ContainerDivID;
        this.IDKey = IDKey;
        this.ParentIDKey = ParentIDKey;
        this.Theme = Theme;
        this.kendoWindowsObj = WindowPageVariable;
        this.API = API;
        this.DataType = DataType;
        this.DataSourceName = DataSourceName;
        this.EditErrorMsg = EditErrorMsg;
        this.EditErrorTitle = EditErrorTitle;
        this.AddErrorMsg = AddErrorMsg;
        this.AddErrorTitle = AddErrorTitle;
        this.sNGLCtrlName = sNGLCtrlName;


    }
}

/// Added By RHR on 08/21/2018 for v-8.2
/// Widget Read and Show Summary data at the top of the screen using Content Management cmPageDetails
///     Get{id} to read the record
function NGLSummaryDataCtrl() {
    //Generic properties for all Widgets
    this.lastErr = "";
    this.notifyOnError = true;
    this.notifyOnSuccess = true;
    this.notifyOnValidationFailure = true;
    data: null;
    DataType: "";
    rData: null;
    callback: null;
    dSource: null;
    sourceDiv: null;
    //Widget specific properties
    this.DataSourceName = "Record";
    screenLEName: null;
    IDKey: "id1234";
    this.Theme = "blueopal";
    dataFields: DataFieldDetails;  //list of fields to edit or insert
    ContainerDivID: "";
    API: "";
    //Widget specific functions


    this.GetFieldID = function (sName) {
        var sRet = "";
        if (!sName) { return sRet; }
        $.each(this.dataFields, function (index, item) {
            if (item.fieldName == sName) {
                sRet = item.fieldTagID;
                return;
            }
        });
        return sRet;
    }

    this.GetFieldName = function (sID) {
        var sRet = "";
        if (!sID) { return sRet; }
        $.each(this.dataFields, function (index, item) {
            if (item.fieldTagID == sID) {
                sRet = item.fieldName;
                return;
            }
        });
        return sRet;
    }

    this.loadHTML = function () {
        var tObj = this;
        var sHtml = ' <span id="Expand' + this.IDKey + 'Span" style="display: none;"><a onclick="expandFastTab(\'Expand' + this.IDKey + 'Span\',\'Collapse' + this.IDKey + 'Span\',\'div' + this.IDKey + 'Summary\',\'\');"><span style="font-size: small;font-weight:bold;" class=\'k-icon k-i-chevron-down\'></span></a></span>' +
            ' <span id="Collapse' + this.IDKey + 'Span" style="display: normal;"><a onclick="collapseFastTab(\'Expand' + this.IDKey + 'Span\',\'Collapse' + this.IDKey + 'Span\',\'div' + this.IDKey + 'Summary\',\'\');"><span style="font-size: small;font-weight:bold;" class=\'k-icon k-i-chevron-up\'></span></a></span>' +
            ' <span id="sp' + this.IDKey + 'Lbl" style="font-size:small; font-weight:bold">' + this.DataSourceName + '</span>&nbsp;&nbsp;<br />' +
            ' <div id="div' + this.IDKey + 'Summary" class="k-block k-info-colored" style=" margin-left:2px; margin-right:2px; margin-top:2px; width:auto; padding-left:2px; padding-right:2px; float: left;">'
        if (typeof (this.dataFields) !== 'undefined' && ngl.isObject(this.dataFields) === true && ngl.isArray(this.dataFields) === true) {
            tObj.blnEditing = false;
            //if we have data we have an existing record to edit so set blnEditing to true
            if (typeof (this.data) !== 'undefined' && ngl.isObject(this.data)) {
                this.blnEditing = true;
            };
            var sTableOpen = '<div style="float: left;"><table class="tblResponsive"><tr>';
            var sTableClose = '</tr></table></div>';
            var sHiddenFields = '';
            var sTopRow = '';
            var blnFirstTable = true;
            $.each(this.dataFields, function (index, item) {
                if (blnFirstTable == true) {
                    blnFirstTable = false;
                } else {
                    sHtml = sHtml + sTableOpen + sTopRow + sTableClose;
                    sTopRow = '';
                }
                if (item.fieldVisible === false) {
                    //when editing hide fields where visible is false and readonly is true, or where visible is false and (required is false or insert only is true)
                    sHiddenFields = sHiddenFields + '<input id="' + item.fieldTagID + '" type="hidden" />';
                } else {
                    //only add the $ to the label if the field is currency
                    if (item.fieldFormat === "{0:c2}") {
                        sTopRow = '<td class="tblResponsive-top" style="max-width:200px; min-width:200px;">&nbsp;' + item.fieldCaption + '&nbsp;$<span id="' + item.fieldTagID + '"></span></td>';
                    } else {
                        sTopRow = '<td class="tblResponsive-top" style="max-width:200px; min-width:200px;">&nbsp;' + item.fieldCaption + '&nbsp;<span id="' + item.fieldTagID + '"></span></td>';
                    }
                }
            });
            //add the last table
            sHtml = sHtml + sTableOpen + sTopRow + sTableClose;
        };
        sHtml = sHtml + sHiddenFields + '</div><div style="clear:both;"><br /></div>'
        $("#" + this.ContainerDivID).html(sHtml);
    };


    this.readSuccessCallback = function (data) {

        var oResults = new nglEventParameters();
        var tObj = this;
        oResults.source = "readSuccessCallback";
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed         
        oResults.CRUD = "read"
        //clear any old return data in rData
        this.rData = null;
        try {
            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                if (typeof (data.Errors) !== 'undefined' && data.Errors != null) {
                    oResults.error = new Error();
                    oResults.error.name = "Read " + this.DataSourceName + " Failure";
                    oResults.error.message = data.Errors;
                    ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                }
                else {
                    this.rData = data.Data;
                    if (data.Data != null) {
                        oResults.data = data.Data;
                        oResults.datatype = this.DataType;
                        oResults.msg = "Success"
                        this.data = data.Data[0];
                        //this.show();
                    }
                    else {
                        oResults.error = new Error();
                        oResults.error.name = "Invalid Request";
                        oResults.error.message = "No Data available.";
                        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                    }
                }
            } else {
                oResults.msg = "Success but no data was returned. Please refresh your page and try again.";
                ngl.showSuccessMsg(oResults.msg, tObj);
            }
        } catch (err) {
            oResults.error = err
        }
        if (ngl.isFunction(tPage[this.callback])) {
            tPage[this.callback](oResults);
        }
        if (oResults.msg == "Success") {
            this.show();
        }

    }

    this.readAjaxErrorCallback = function (xhr, textStatus, error) {
        var oResults = new nglEventParameters();
        var tObj = this;
        oResults.source = "readAjaxErrorCallback";
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed        
        oResults.CRUD = "read"
        oResults.error = new Error();
        oResults.error.name = "Read " + this.DataSourceName + " Failure"
        oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);

        if (ngl.isFunction(tPage[this.callback])) {
            tPage[this.callback](oResults);
        }
    }

    this.show = function () {
        var tObj = this;
        try {
            this.loadHTML();

            this.edit(this.data);
        } catch (err) {

            ngl.showErrMsg("Cannot show summary data", err.message, null);

        }

    }

    this.read = function (intControl) {

        var blnRet = true;
        var tObj = this;
        if (typeof (intControl) != 'undefined' && intControl != null) {
            this.rData = null;
            this.data = null;
            setTimeout(function (tObj) {
                var oCRUDCtrl = new nglRESTCRUDCtrl();
                if (oCRUDCtrl.read(tObj.API, intControl, tObj, "readSuccessCallback", "readAjaxErrorCallback") == false) {
                    blnRet = false;
                }
            }, 20, tObj);
        }
        return blnRet;
    }

    this.edit = function (data) {
        //load data to the screen
        var tObj = this;
        if (typeof (data) !== 'undefined' && data != null && ngl.isObject(data)) {
            if (typeof (this.dataFields) !== 'undefined' && ngl.isObject(this.dataFields) === true && ngl.isArray(this.dataFields) === true) {
                $.each(this.dataFields, function (index, item) {
                    if (item.fieldName in data) {
                        var dataItem = data[item.fieldName];
                        if (typeof (dataItem) !== 'undefined' && dataItem !== null) {
                            if (item.fieldVisible === false) {
                                //update the hidden fields  
                                $("#" + item.fieldTagID).val(dataItem);
                            } else {
                                if (item.fieldGroupSubType === nglGroupSubTypeEnum.DatePicker) {
                                    //debugger; console.log(item);
                                    $("#" + item.fieldTagID).html(ngl.formatDate(dataItem, '', 'd'));
                                } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.DateTimePicker) {
                                    $("#" + item.fieldTagID).html(ngl.formatDate(dataItem, '', 'g'));
                                } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.TimePicker) {
                                    $("#" + item.fieldTagID).html(ngl.formatDate(dataItem, '', 't'));
                                } else {
                                    //$("#" + item.fieldTagID).html(dataItem
                                    //Begin Modified by RHR for v-8.4.0.003 on 07/07/2021
                                    //debugger;
                                    var sData = "";
                                    switch (item.fieldFormat) {
                                        case "{0:n6}":
                                            sData = Number(dataItem).toFixed(6);
                                            break;
                                        case "{0:n5}":
                                            sData = Number(dataItem).toFixed(5);
                                            break;
                                        case "{0:n4}":
                                            sData = Number(dataItem).toFixed(4);
                                            break;
                                        case "{0:n3}":
                                            sData = Number(dataItem).toFixed(3);
                                            break;
                                        case "{0:n2}":
                                            // debugger;
                                            sData = Number(dataItem).toFixed(2);
                                            break;
                                        case "{0:n1}":
                                            sData = Number(dataItem).toFixed(1);
                                            break;
                                        case "{0:n0}":
                                            sData = Number(dataItem).toFixed(0);
                                            break;
                                        case "{0:#0}":
                                            sData = Number(dataItem).toFixed(0);
                                            break;
                                        case "{0:c2}":
                                            sData = Number(dataItem).toFixed(2);
                                            break;
                                        default:
                                            // code block                                    
                                            sData = dataItem;
                                    }

                                    $("#" + item.fieldTagID).html(sData);
                                }
                            };
                        };
                    };
                });
            } else {
                ngl.showValidationMsg(this.EditErrorTitle, this.EditErrorMsg, null); this.EditError = true; return false;
            };
        };
        return true;
    }

    /// loadDefaults sets up the callbacks cbSelect and cbSave
    /// all call backs return a reference to this object and a string message as parameters
    /// CallBackFunction can be used by the caller to handle return information.  it returns a nglEventParameters objec
    this.loadDefaults = function (ContainerDivID,
        IDKey,
        fieldData,
        Theme,
        CallBackFunction,
        API,
        DataType,
        DataSourceName) {


        var tObj = this;
        this.callback = CallBackFunction;
        this.dataFields = fieldData;
        this.lastErr = "";
        this.notifyOnError = true;
        this.notifyOnSuccess = true;
        this.notifyOnValidationFailure = true;
        this.ContainerDivID = ContainerDivID;
        this.IDKey = IDKey;
        this.Theme = Theme;
        this.API = API;
        this.DataType = DataType;
        this.DataSourceName = DataSourceName;


    }
}


/// Added By RHR on 07/06/2018 for v-8.2
/// Widget for Edit Add and Delete via Content Management Popup Window
///     Get{id} to read the record
///     POST{data} to insert or update a record
///     DELETE{id} to delete the record
function NGLEditOnPageCtrl() {
    //Generic properties for all Widgets
    this.lastErr = "";
    this.notifyOnError = true;
    this.notifyOnSuccess = true;
    this.notifyOnValidationFailure = true;
    data: null;
    DataType: "";
    rData: null;
    callback: null;
    dSource: null;
    sourceDiv: null;
    //Widget specific properties
    this.DataSourceName = "Record";
    this.EditErrorMsg = "Please select a row with valid data";
    this.EditErrorTitle = "Data Required";
    screenLEName: null;
    IDKey: "id1234";
    this.Theme = "blueopal";
    dataFields: DataFieldDetails;  //list of fields to edit or insert
    ContainerDivID: "";
    this.EditError = false;
    API: "";
    this.blnEditing = false;
    this.PKName = "";
    this.sNGLCtrlName = "";
    //Widget specific functions

    this.ListChanged = function (e, tList) {
        var source = this;
        //NGLEDITOnPageListChanged
        if (typeof (NGLEDITOnPageListChanged) !== 'undefined' && ngl.isFunction(NGLEDITOnPageListChanged)) {
            NGLEDITOnPageListChanged(e, tList, source);
        }
    };

    this.GetFieldID = function (sName) {
        var sRet = "";
        if (!sName) { return sRet; }
        $.each(this.dataFields, function (index, item) {
            if (item.fieldName == sName) {
                sRet = item.fieldTagID;
                return;
            }
        });
        return sRet;
    }

    this.SetFieldDefault = function (sName, sVal) {
        var blnRet = false;
        if (!sVal) { return blnRet; }
        if (!sName) { return blnRet; }
        $.each(this.dataFields, function (index, item) {
            if (item.fieldName == sName) {
                sRet = item.fieldDefaultValue = sVal;
                blnRet = true;
                return;
            }
        });
        return blnRet;
    }

    this.GetFieldName = function (sID) {
        var sRet = "";
        if (!sID) { return sRet; }
        $.each(this.dataFields, function (index, item) {
            if (item.fieldTagID == sID) {
                sRet = item.fieldName;
                return;
            }
        });
        return sRet;
    }

    this.GetFieldItem = function (sID) {
        var sRet = null;
        if (!sID) { return sRet; }
        $.each(this.dataFields, function (index, item) {
            if (item.fieldTagID == sID) {
                sRet = item;
                return;
            }
        });
        return sRet;
    }

    this.GetFieldItemByFieldID = function (sFieldID) {
        var sRet = null;
        if (!sFieldID) { return sRet; }
        $.each(this.dataFields, function (index, item) {
            if (item.fieldID == sFieldID) {
                sRet = item;
                return;
            }
        });
        return sRet;
    }

    this.loadHTML = function () {
        var tObj = this;
        //var sHtml = kendo.format("<div id='div{0}Border' class='ngl-blueBorderFullPageWide' style='position: relative; min-width:250px;' ><div id='div{0}Edit' style='padding: 0px 10px 10px 10px;'><div style='margin:5px;'><span id='sp{0}Lbl' style='font-size:large; font-weight:bold'>{1}</span>&nbsp;&nbsp;<a onclick='{2}.save();' class='k-button k-button-solid-base k-button-solid k-button-md k-rounded-md' href='#'><span class='k-icon k-i-save'></span>Save</a></div>", this.IDKey,this.DataSourceName,this.sNGLCtrlName);


        if (typeof (this.dataFields) !== 'undefined' && ngl.isObject(this.dataFields) === true && ngl.isArray(this.dataFields) === true) {
            tObj.blnEditing = false;
            //if we have data we have an existing record to edit so set blnEditing to true
            if (typeof (this.data) !== 'undefined' && ngl.isObject(this.data)) {
                this.blnEditing = true;
            };
            //$("#" + this.ContainerDivID).html("");
            $("#" + this.ContainerDivID).html(' <span id="Expand' + this.IDKey + 'Span" style="display: none;"><a onclick="expandFastTab(\'Expand' + this.IDKey + 'Span\',\'Collapse' + this.IDKey + 'Span\',\'div' + this.IDKey + 'Border\',\'\');"><span style="font-size: small;font-weight:bold;" class=\'k-icon k-i-chevron-down\'></span></a></span>' +
                ' <span id="Collapse' + this.IDKey + 'Span" style="display: normal;"><a onclick="collapseFastTab(\'Expand' + this.IDKey + 'Span\',\'Collapse' + this.IDKey + 'Span\',\'div' + this.IDKey + 'Border\',\'\');"><span style="font-size: small;font-weight:bold;" class=\'k-icon k-i-chevron-up\'></span></a></span>' +
                ' <span id="sp' + this.IDKey + 'Lbl" style="font-size:small; font-weight:bold">' + this.DataSourceName + '</span>&nbsp;&nbsp;<br />');

            $("#" + this.ContainerDivID).append(kendo.format("<div id='div{0}Border' class='ngl-blueBorderFullPageWide' style='position: relative; min-width:250px;' ></div>", this.IDKey));
            var divBorder = $("#div" + this.IDKey + "Border");

            divBorder.append(kendo.format("<div style='margin:5px;'><span id='sp{0}Lbl' style='font-size:large; font-weight:bold'>{1}</span>&nbsp;&nbsp;<a onclick='{2}.save();' class='k-button k-button-solid-base k-button-solid k-button-md k-rounded-md' href='#'><span class='k-icon k-i-save'></span>Save</a></div><div id='div{0}InsertOnly' style='padding: 0px 10px 10px 10px; display:none; float: left;'></div><div id='div{0}Edit' style='padding: 0px 10px 10px 10px;'></div>", this.IDKey, this.DataSourceName, this.sNGLCtrlName));
            var divEdit = $("#div" + this.IDKey + "Edit");
            var divActive = divEdit
            var $HiddenFields = $("<div style='display:none;'></div>");
            var $InsertOnlyHTML = $("#div" + this.IDKey + "InsertOnly");  //$(kendo.format("<div id='div{0}InsertOnly' style='padding: 0px 10px 10px 10px; display:none; float: left;'></div>", this.IDKey));
            var $floattable = $("<table class='tblResponsive'></table>");
            var $floattableheaderrow = $("<tr></tr>");
            var $floattableitemrow = $("<tr></tr>");
            var floattablecount = 0;
            var sRequiredAsterik = '';
            var sReadOnly = '';
            var bDivOpen = false;
            var bFloatLeftTableOpen = false;
            var bFloatHeadersOpen = false;
            var sFloatTableHeader = "";
            var sFloatTableRow = "";
            var iTableColumns = 0;
            var iTableHeaders = 0;
            var sFormated = "";
            var bInsertOnlyOpen = false;

            $.each(this.dataFields, function (index, item) {
                //if (item.fieldAllowNull === false) { item.fieldRequired = true; }

                sFormated = "";
                if (item.fieldReadOnly === true) {
                    sReadOnlyClass = ' class="k-icon k-i-lock" ';
                    //sReadOnly = '<span class="k-icon k-i-lock"></span>&nbsp;';
                    sRequiredAsterik = '';
                } else {
                    sReadOnlyClass = '';
                    //if (item.fieldAllowNull === false || item.fieldRequired === true) {
                    //Modified by RHR for v-8.2 on 01/02/2019 removed logic to test for allow null on required
                    if (item.fieldRequired === true) {
                        sRequiredAsterik = '<span class="redRequiredAsterik"> *</span>';
                    }
                    else {
                        sRequiredAsterik = '';
                    }
                }

                if (tObj.blnEditing === true && ((item.fieldVisible === false && item.fieldReadOnly === true) || (item.fieldVisible === false && (item.fieldRequired === false || item.fieldInsertOnly === true)))) {
                    //when editing hide fields where visible is false and readonly is true, or where visible is false and (required is false or insert only is true)
                    $HiddenFields.append('<input id="' + item.fieldTagID + '" type="hidden" />');
                } else if (tObj.blnEditing === false && ((item.fieldVisible === false && item.fieldReadOnly === true) || (item.fieldVisible === false && item.fieldInsertOnly === false && item.fieldRequired === false) || (item.fieldVisible === true && item.fieldReadOnly === true && item.fieldInsertOnly === false))) {
                    //when adding new records  hide hide fields where visible is false and readonly is true, or where visible is false and (required is false or insert only is true) or where visible = true and readonly = true and insertonly = false
                    $HiddenFields.append('<input id="' + item.fieldTagID + '" type="hidden" />');
                } else {
                    if (item.fieldGroupSubType === nglGroupSubTypeEnum.Header1) {
                        sFormated = " <h1 style='clear:both; display: block; float: none;'>" + item.fieldCaption + "</h1>";
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Header2) {
                        sFormated = " <h2 style='clear:both; display: block; float: none;'>" + item.fieldCaption + "</h2>";
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Header3) {
                        sFormated = " <h3 style='clear:both; display: block; float: none;'>" + item.fieldCaption + "</h3>";
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Header4) {
                        sFormated = " <h4 style='clear:both; display: block; float: none;'>" + item.fieldCaption + "</h4>";
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Paragraph) {
                        sFormated = " <p style='clear:both; display: block; float: none;'>" + item.fieldCaption + "</p>";
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Link) {
                        sFormated = " <a href='" + item.fieldAPIReference + "'>" + tem.fieldCaption + "</a>";
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.HTMLSpace) {
                        sFormated = "&nbsp;";
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.HTMLLineBreak) {
                        sFormated = " <br style='clear:both; display: block; float: none;'/>";
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Div) {
                        //once a div is entered it becomes active and everything below it falls inside this div until   
                        //the next float block, float blocks are always appended to divEdit
                        if (bFloatLeftTableOpen == true) {
                            //append the float table first because divs are not allowed in tables
                            divActive.append(kendo.format("<div id='divFloatTable-{0}' style='float: left;'></div>", floattablecount.toString()));
                            $floattable.append($floattableitemrow);
                            $("#divFloatTable-" + floattablecount.toString()).append($floattable);
                            bFloatLeftTableOpen = false
                        }
                        divActive.append("<div id='div" + item.fieldTagID + "Break' style='padding: 0px 10px 10px 10px; clear:both; display: block; float: none;'></div>");
                        divActive = $("#div" + item.fieldTagID + "Break");
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.FloatLeftTable) {
                        //Note: Once FloatLeftTable is active all fields are added to the table until a 
                        //div a FloatBlockLeft or another FloatLeftTable is activated
                        // fields are added to rows according to the number of tableheader fields included
                        // Labels are used to include text in a row instead of a data object                       
                        if (bFloatLeftTableOpen == true) {
                            //append the open float table to the active div and create a new float table
                            //float tables are always nested inside a <div style="float: left;">
                            divActive.append(kendo.format("<div id='divFloatTable-{0}' style='float: left;'></div>", floattablecount.toString()));
                            $floattable.append($floattableitemrow);
                            $("#divFloatTable-" + floattablecount.toString()).append($floattable);
                        }
                        $floattable = $("<table class='tblResponsive'></table>");
                        $floattableheaderrow = $("<tr></tr>");
                        $floattableitemrow = $("<tr></tr>");
                        floattablecount = floattablecount + 1;
                        iTableColumns = 0;
                        iTableHeaders = 0;
                        bFloatLeftTableOpen = true;
                        bFloatHeadersOpen = false;

                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.FloatBlockLeft) {
                        //float blocks are always appended to divEdit
                        if (bFloatLeftTableOpen == true) {
                            //append the float table first because FloatBlocks are not allowed in tables
                            divActive.append(kendo.format("<div id='divFloatTable-{0}' style='float: left;'></div>", floattablecount.toString()));
                            $floattable.append($floattableitemrow);
                            $("#divFloatTable-" + floattablecount.toString()).append($floattable);
                            bFloatLeftTableOpen = false
                        }
                        divEdit.append("<div id='div" + item.fieldTagID + "FloatBlock' style='float:left;'></div>");
                        divActive = $("#div" + item.fieldTagID + "FloatBlock");
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.TableHeader) {
                        if (bFloatLeftTableOpen == true) {
                            bFloatHeadersOpen = true;
                            iTableHeaders = iTableHeaders + 1;
                            $floattableheaderrow.append("<th style='border:none;'>" + item.fieldCaption + "</th>");
                        }
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Label) {
                        if (bFloatLeftTableOpen == true) {
                            if (iTableColumns == iTableHeaders) {
                                iTableColumns = 0;
                                //close the old row and start a new one
                                if (bFloatHeadersOpen == true) {
                                    $floattable.append($floattableheaderrow);
                                    bFloatHeadersOpen = false;
                                }
                                $floattable.append($floattableitemrow);
                                $floattableitemrow = $("<tr></tr>");
                            }
                            iTableColumns = iTableColumns + 1;
                            //add the item to the  current Float Table row
                            $floattableitemrow.append("<td class='tblResponsive-top'>" + item.fieldCaption + "</td>");
                        } else {
                            //add a Responsive table with just a caption (no data) -- this should not be used
                            sFormated = kendo.format("<div style='float: left;'><table class='tblResponsive'><tr><td class='tblResponsive-top' style='max-width:200px; min-width:200px;'>{0}</td></tr><tr><td class='tblResponsive-top' style='max-width:200px; min-width:200px;'></td></tr></table></div>", item.fieldCaption);
                        }
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Checkbox) {
                        if (bFloatLeftTableOpen == true) {
                            if (iTableColumns == iTableHeaders) {
                                iTableColumns = 0;
                                //close the old row and start a new one
                                if (bFloatHeadersOpen == true) {
                                    $floattable.append($floattableheaderrow);
                                    bFloatHeadersOpen = false;
                                }
                                $floattable.append($floattableitemrow);
                                $floattableitemrow = $("<tr></tr>");
                            }
                            iTableColumns = iTableColumns + 1;
                            //add the item to the  current Float Table row
                            $floattableitemrow.append(kendo.format("<td class='tblResponsive-top'><span id='splock{3}' {0}></span>&nbsp;{2}<input id='{3}' type='checkbox'  /></td>",
                                sReadOnlyClass, item.fieldCaption, sRequiredAsterik, item.fieldTagID));
                        } else {
                            //add a Responsive table
                            sFormated = kendo.format(" <div style='float: left;'><table class='tblResponsive'><tr><td class='tblResponsive-top' style='max-width:200px; min-width:200px;'><span id='splock{3}' {0}></span>&nbsp;{1}{2}</td></tr><tr><td class='tblResponsive-top' style='max-width:200px; min-width:200px;'><input type='checkbox' id='{3}' class='k-checkbox'></td></tr></table></div>",
                                sReadOnlyClass, item.fieldCaption, sRequiredAsterik, item.fieldTagID);
                        }
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.kendoSwitch) {
                        if (bFloatLeftTableOpen == true) {
                            if (iTableColumns == iTableHeaders) {
                                iTableColumns = 0;
                                //close the old row and start a new one
                                if (bFloatHeadersOpen == true) {
                                    $floattable.append($floattableheaderrow);
                                    bFloatHeadersOpen = false;
                                }
                                $floattable.append($floattableitemrow);
                                $floattableitemrow = $("<tr></tr>");
                            }
                            iTableColumns = iTableColumns + 1;
                            //add the item to the  current Float Table row
                            $floattableitemrow.append(kendo.format("<td class='tblResponsive-top'><span id='splock{3}' {0}></span>&nbsp;{2}<input id='{3}' type='checkbox' class='k-checkbox' /></td>",
                                sReadOnlyClass, item.fieldCaption, sRequiredAsterik, item.fieldTagID));
                        } else {
                            sFormated = kendo.format("<div style='float: left;'><table class='tblResponsive'><tr><td class='tblResponsive-top' style='max-width:200px; min-width:200px;'><span id='splock{3}' {0}></span>&nbsp;{1}{2}</td></tr><tr><td class='tblResponsive-top' style='max-width:200px; min-width:200px;'><input type='checkbox' id='{3}' class='k-checkbox'></td></tr></table></div>",
                                sReadOnlyClass, item.fieldCaption, sRequiredAsterik, item.fieldTagID);
                        }
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Editor) {
                        var sMaxLength = '';
                        if (typeof (item.fieldMaxlength) !== 'undefined' && item.fieldMaxlength !== null && isEmpty(item.fieldMaxlength) === false && isNaN(item.fieldMaxlength) === false) {
                            var sMaxLength = " maxlength='" + item.fieldMaxlength + "' ";
                        }
                        if (bFloatLeftTableOpen == true) {
                            if (iTableColumns == iTableHeaders) {
                                iTableColumns = 0;
                                //close the old row and start a new one
                                if (bFloatHeadersOpen == true) {
                                    $floattable.append($floattableheaderrow);
                                    bFloatHeadersOpen = false;
                                }
                                $floattable.append($floattableitemrow);
                                $floattableitemrow = $("<tr></tr>");
                            }
                            iTableColumns = iTableColumns + 1;
                            //add the item to the  current Float Table row
                            $floattableitemrow.append(kendo.format("<td id='td{3}' class='tblResponsive-top'><span id='splock{3}' {0}></span>&nbsp;{2}<textarea rows='2' cols='75' {4} id='{3}' style='height:50px;'></textarea></td>",
                                sReadOnlyClass, item.fieldCaption, sRequiredAsterik, item.fieldTagID, sMaxLength));
                        } else {
                            sFormated = kendo.format("<div style='float: left;'><table class='tblResponsive'><tr><td class='tblResponsive-top' style='max-width:200px; min-width:200px;'><span id='splock{3}' {0}></span>&nbsp;{1}{2}</td></tr><tr><td id='td{3}' class='tblResponsive-top' style='max-width:200px; min-width:200px;'><textarea rows='2' cols='75' {4} id='{3}' style='height:50px;'></textarea></td></tr></table></div>",
                                sReadOnlyClass, item.fieldCaption, sRequiredAsterik, item.fieldTagID, sMaxLength);
                        }
                    } else if (typeof (item.fieldMaxlength) !== 'undefined' && item.fieldMaxlength !== null && isEmpty(item.fieldMaxlength) === false && isNaN(item.fieldMaxlength) === false && item.fieldMaxlength > 150) {

                        var sMaxLength = " maxlength='" + item.fieldMaxlength + "' ";

                        if (bFloatLeftTableOpen == true) {
                            if (iTableColumns == iTableHeaders) {
                                iTableColumns = 0;
                                //close the old row and start a new one
                                if (bFloatHeadersOpen == true) {
                                    $floattable.append($floattableheaderrow);
                                    bFloatHeadersOpen = false;
                                }
                                $floattable.append($floattableitemrow);
                                $floattableitemrow = $("<tr></tr>");
                            }
                            iTableColumns = iTableColumns + 1;
                            //add the item to the  current Float Table row
                            $floattableitemrow.append(kendo.format("<td id='td{3}' class='tblResponsive-top'><span id='splock{3}' {0}></span>&nbsp;{2}<textarea rows='2' cols='75' {4} id='{3}' style='height:50px;'></textarea></td>",
                                sReadOnlyClass, item.fieldCaption, sRequiredAsterik, item.fieldTagID, sMaxLength));
                        } else {
                            sFormated = kendo.format("<div style='float: left;'><table class='tblResponsive'><tr><td class='tblResponsive-top' style='max-width:200px; min-width:200px;'><span id='splock{3}' {0}></span>&nbsp;{1}{2}</td></tr><tr><td id='td{3}' class='tblResponsive-top' style='max-width:200px; min-width:200px;'><textarea rows='2' cols='75' {4} id='{3}' style='height:50px;'></textarea></td></tr></table></div>",
                                sReadOnlyClass, item.fieldCaption, sRequiredAsterik, item.fieldTagID, sMaxLength);
                        }
                    } else {

                        var sMaxLength = '';
                        if (typeof (item.fieldMaxlength) !== 'undefined' && item.fieldMaxlength !== null && isEmpty(item.fieldMaxlength) === false && isNaN(item.fieldMaxlength) === false) {
                            var sMaxLength = " maxlength='" + item.fieldMaxlength + "' ";
                        }
                        if (bFloatLeftTableOpen == true) {
                            if (iTableColumns == iTableHeaders) {
                                iTableColumns = 0;
                                //close the old row and start a new one
                                if (bFloatHeadersOpen == true) {
                                    $floattable.append($floattableheaderrow);
                                    bFloatHeadersOpen = false;
                                }
                                $floattable.append($floattableitemrow);
                                $floattableitemrow = $("<tr></tr>");
                            }
                            iTableColumns = iTableColumns + 1;
                            //add the item to the  current Float Table row
                            $floattableitemrow.append(kendo.format("<td class='tblResponsive-top'><span id='splock{3}' {0}></span>&nbsp;{2}<input id='{3}' {4} /></td>",
                                sReadOnlyClass, item.fieldCaption, sRequiredAsterik, item.fieldTagID, sMaxLength));
                        } else {
                            sFormated = kendo.format("<div style='float: left;'><table class='tblResponsive'><tr><td class='tblResponsive-top' style='max-width:200px; min-width:200px;'><span id='splock{3}' {0}></span>&nbsp;{1}{2}</td></tr><tr><td class='tblResponsive-top' style='max-width:200px; min-width:200px;'><input id='{3}' {4} /></td></tr></table></div>",
                                sReadOnlyClass, item.fieldCaption, sRequiredAsterik, item.fieldTagID, sMaxLength);
                        }
                    }
                    if (isEmpty(sFormated) === false) {
                        if (item.fieldInsertOnly === true) {
                            $InsertOnlyHTML.append(sFormated);
                        } else {
                            divActive.append(sFormated);
                        }
                    }
                }


            });

            if (bFloatLeftTableOpen == true) {
                //append the open float table to the active div and create a new float table
                //float tables are always nested inside a <div style="float: left;">
                divActive.append(kendo.format("<div id='divFloatTable-{0}' style='float: left;'></div>", floattablecount.toString()));
                $floattable.append($floattableitemrow);
                $("#divFloatTable-" + floattablecount.toString()).append($floattable);
            }
            //divEdit.append($InsertOnlyHTML) ;   
            divEdit.append($HiddenFields);
            var sHT = $("#" + this.ContainerDivID).html();
            //debugger;
            //console.log(sHT);
        };


    };

    this.loadKendo = function () {
        if (typeof (this.dataFields) !== 'undefined' && ngl.isObject(this.dataFields) === true && ngl.isArray(this.dataFields) === true) {
            var tObj = this;
            this.blnEditing = false;
            //if we have data we have an existing record to edit so set blnEditing to true
            if (typeof (this.data) !== 'undefined' && ngl.isObject(this.data)) {
                this.blnEditing = true;
            };
            $.each(this.dataFields, function (index, item) {
                //if (item.fieldAllowNull === false) { item.fieldRequired = true; }
                if (tObj.blnEditing === true && ((item.fieldVisible === false && item.fieldReadOnly === true) || (item.fieldVisible === false && (item.fieldRequired === false || item.fieldInsertOnly === true)))) {
                    //when editing no kindo widgets for these hidden where visible is false and readonly is true, or where visible is false and (required is false or insert only is true)
                } else if (tObj.blnEditing === false && ((item.fieldVisible === false && item.fieldReadOnly === true) || (item.fieldVisible === false && item.fieldInsertOnly === false && item.fieldRequired === false) || (item.fieldVisible === true && item.fieldReadOnly === true && item.fieldInsertOnly === false))) {
                    //when adding new records  no kindo widgets for fields where visible is false and readonly is true, or where visible is false and (required is false or insert only is true) or where visible = true and readonly = true and insertonly = false
                } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.kendoSwitch) {
                    var nglkendoitem = $("#" + item.fieldTagID).data("kendoSwitch");
                    if (!nglkendoitem) {
                        $("#" + item.fieldTagID).kendoSwitch();
                    }
                } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.NumericTextBox) {
                    var nglkendoitem = $("#" + item.fieldTagID).data("kendoNumericTextBox");
                    if (!nglkendoitem) {
                        // widget instance does not exist
                        if (typeof (item.fieldFormat) !== 'undefined' && item.fieldFormat !== null && isEmpty(item.fieldFormat) === false) {
                            $("#" + item.fieldTagID).kendoNumericTextBox({ format: item.fieldFormat });
                        } else {
                            $("#" + item.fieldTagID).kendoNumericTextBox({ decimals: 5 });
                        };
                    }

                } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.DatePicker) {
                    var nglkendoitem = $("#" + item.fieldTagID).data("kendoDatePicker");
                    //debugger; console.log(item);
                    if (!nglkendoitem) {
                        if (typeof (item.fieldFormat) !== 'undefined' && item.fieldFormat !== null && isEmpty(item.fieldFormat) === false) {
                            $("#" + item.fieldTagID).kendoDatePicker({ format: item.fieldFormat });
                        } else {
                            $("#" + item.fieldTagID).kendoDatePicker();
                        };
                    }
                } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.DateTimePicker) {
                    var nglkendoitem = $("#" + item.fieldTagID).data("kendoDateTimePicker");
                    if (!nglkendoitem) {
                        if (typeof (item.fieldFormat) !== 'undefined' && item.fieldFormat !== null && isEmpty(item.fieldFormat) === false) {
                            $("#" + item.fieldTagID).kendoDateTimePicker({ format: item.fieldFormat, interval: 15 });
                        } else {
                            $("#" + item.fieldTagID).kendoDateTimePicker({ format: "HH:mm ", interval: 15 });
                        };
                    }
                } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.TimePicker) {
                    var nglkendoitem = $("#" + item.fieldTagID).data("kendoTimePicker");
                    if (!nglkendoitem) {
                        // Modified by RHR for v-8.5.2.006 on 12/28/2022 add default format and interval for kendoTimePicker
                        if (typeof (item.fieldFormat) !== 'undefined' && item.fieldFormat !== null && isEmpty(item.fieldFormat) === false) {
                            $("#" + item.fieldTagID).kendoTimePicker({ format: item.fieldFormat, interval: 5 });
                        } else {
                            $("#" + item.fieldTagID).kendoTimePicker({ format: 'HH:mm', interval: 5 });
                        };
                    }

                } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Editor) {
                    var nglkendoitem = $("#" + item.fieldTagID).data("kendoEditor");
                    if (!nglkendoitem) {
                        $("#" + item.fieldTagID).kendoEditor({
                            resizable: {
                                content: false,
                                toolbar: false

                            },
                            // Empty tools so do not display toolbar
                            tools: []
                        });
                    }
                } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.ColorPicker) {
                    var nglkendoitem = $("#" + item.fieldTagID).data("kendoColorPicker");
                    if (!nglkendoitem) {
                        $("#" + item.fieldTagID).kendoColorPicker();
                    }
                } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.DropDownList) {
                    var nglkendoitem = $("#" + item.fieldTagID).data("kendoDropDownList");
                    if (!nglkendoitem) {
                        $("#" + item.fieldTagID).kendoDropDownList({
                            dataTextField: "Name",
                            dataValueField: "Control",
                            autoWidth: true,
                            dataSource: {
                                transport: {
                                    read: function (options) {
                                        $.ajax({
                                            async: true,
                                            type: "GET",
                                            url: "api/" + item.fieldAPIReference + "/" + item.fieldAPIFilterID,
                                            contentType: "application/json; charset=utf-8",
                                            dataType: 'json',
                                            headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                            success: function (data) {
                                                options.success(data);
                                            },
                                            error: function (xhr, textStatus, error) {
                                                var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Get " + item.fieldName + "  Failure");
                                                ngl.showErrMsg("Read " + item.fieldName + " Error", sMsg, null);
                                            }
                                        });
                                    }
                                },
                                schema: {
                                    data: "Data",
                                    total: "Count",
                                    model: {
                                        id: "Control",
                                        fields: {
                                            Name: { editable: false },
                                            Description: { editable: false }
                                        }
                                    },
                                    errors: "Errors"
                                },
                                error: function (e) {
                                    ngl.showErrMsg("Read " + item.fieldName + " Error", e.errors, null);
                                    this.cancelChanges();
                                },
                                serverPaging: false,
                                serverSorting: false,
                                serverFiltering: false
                            },
                            change: function (e) { var tList = this; if (typeof (tObj.ListChanged) !== 'undefined' && ngl.isFunction(tObj.ListChanged)) { tObj.ListChanged(e, tList); } },
                            dataBound: function (e) {
                                var listContainer = e.sender.list.closest(".k-list-container");
                                var iNewWidth = listContainer.width() + 25;
                                listContainer.width(iNewWidth);
                                var tList = this;
                                if (typeof (tObj.ListChanged) !== 'undefined' && ngl.isFunction(tObj.ListChanged)) {
                                    tObj.ListChanged(e, tList);
                                }
                            },
                        });

                        var localDropDown = $("#" + item.fieldTagID).data("kendoDropDownList");
                        localDropDown.list.width("auto");
                    } else {
                        nglkendoitem.dataSource.read();
                    }
                } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.MaskedTextBox) {
                    var nglkendoitem = $("#" + item.fieldTagID).data("kendoMaskedTextBox");
                    if (!nglkendoitem) {
                        if (typeof (item.fieldFormat) !== 'undefined' && item.fieldFormat !== null && isEmpty(item.fieldFormat) === false) {
                            $("#" + item.fieldTagID).kendoMaskedTextBox({ mask: item.fieldFormat });
                        } else {
                            $("#" + item.fieldTagID).kendoMaskedTextBox();
                        };
                    }

                };
            });
        };
    };

    //Generic Call back functions for all Widgets
    this.saveSuccessCallback = function (data) {
        //debugger;
        var oResults = new nglEventParameters();
        oResults.source = "saveSuccessCallback";
        var tObj = this;
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed   
        //kendo.ui.progress($(document.body), false);
        if (this.blnEditing == false) {
            oResults.CRUD = "create"
        } else {
            oResults.CRUD = "update"
        }

        this.rData = null;
        try {
            kendo.ui.progress($("#div" + this.IDKey + "Border"), false);
        } catch (err) {
            ngl.showInfoNotification("Cancel Progress Failure", "Your process is complete but the wait cursor could not be turned off, please refresh the page if the wait cursor does not go away.", null)
        }
        try {
            var blnSuccess = false;
            var blnErrorShown = false;
            var strValidationMsg = "";
            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                    oResults.error = new Error();
                    oResults.error.name = "Save " + this.DataSourceName + " Failure";
                    oResults.error.message = data.Errors;
                    blnErrorShown = true;
                    ngl.showErrMsg(oResults.error.name, oResults.error.message, null);
                }
                else {
                    this.rData = data.Data;
                    if (typeof (data.Data) !== 'undefined' && data.Data != null && ngl.isArray(data.Data) && data.Data[0] == true) {
                        blnSuccess = true;
                        oResults.datatype = "bool";
                        oResults.data = data.Data[0];
                        oResults.msg = "Success"
                        ngl.showSuccessMsg("Success your " + this.DataSourceName + " changes have been saved", null);
                    } else {
                        blnErrorShown = true;
                        oResults.error = new Error();
                        oResults.error.name = "Unable to save your " + this.DataSourceName + " changes";
                        oResults.error.message = "The save procedure returned false, please refresh your data and try again.";
                        ngl.showErrMsg(oResults.error.name, oResults.error.message, null);
                    }

                }
            }
            if (blnSuccess === false && blnErrorShown === false) {
                oResults.error = new Error();
                oResults.error.name = "Save " + this.DataSourceName + " Failure";
                oResults.error.message = "No results are available.";
                ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
            }
        } catch (err) {
            oResults.error = err;
            ngl.showErrMsg(err.name, err.message, null);

        }
        if (ngl.isFunction(tPage[this.callback])) {
            tPage[this.callback](oResults);
        }
        if (this.blnEditing == false) { this.read(0); }

    }

    this.saveAjaxErrorCallback = function (xhr, textStatus, error) {
        var oResults = new nglEventParameters();
        var tObj = this;
        try {
            kendo.ui.progress($("#div" + this.IDKey + "Border"), false);
        } catch (err) {
            ngl.showInfoNotification("Cancel Progress Failure", "Your process is complete but the wait cursor could not be turned off, please refresh the page if the wait cursor does not go away.", null)
        }
        //kendo.ui.progress($(document.body), false);
        oResults.source = "saveAjaxErrorCallback";
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed 
        if (this.blnEditing == false) {
            oResults.CRUD = "create"
        } else {
            oResults.CRUD = "update"
        }
        oResults.error = new Error();
        oResults.error.name = "Save " + this.DataSourceName + " Failure"
        oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);

        if (ngl.isFunction(tPage[this.callback])) {
            tPage[this.callback](oResults);
        }
    }

    this.readSuccessCallback = function (data) {

        var oResults = new nglEventParameters();
        var tObj = this;
        try {
            kendo.ui.progress($("#div" + this.IDKey + "Border"), false);
        } catch (err) {
            ngl.showInfoNotification("Cancel Progress Failure", "Your process is complete but the wait cursor could not be turned off, please refresh the page if the wait cursor does not go away.", null)
        }
        oResults.source = "readSuccessCallback";
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed         
        oResults.CRUD = "read"
        //clear any old return data in rData
        this.rData = null;
        try {
            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                if (typeof (data.Errors) !== 'undefined' && data.Errors != null) {
                    oResults.error = new Error();
                    oResults.error.name = "Read " + this.DataSourceName + " Failure";
                    oResults.error.message = data.Errors;
                    ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                }
                else {
                    this.rData = data.Data;
                    if (data.Data != null) {
                        oResults.data = data.Data;
                        oResults.datatype = this.DataType;
                        oResults.msg = "Success"
                        this.data = data.Data[0];
                        //this.show();
                    }
                    else {
                        oResults.error = new Error();
                        oResults.error.name = "Invalid Request";
                        oResults.error.message = "No Data available.";
                        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                    }
                }
            } else {
                oResults.msg = "Success but no data was returned. Please refresh your page and try again.";
                ngl.showSuccessMsg(oResults.msg, tObj);
            }
        } catch (err) {
            oResults.error = err
        }
        if (ngl.isFunction(tPage[this.callback])) {
            tPage[this.callback](oResults);
        }
        if (oResults.msg == "Success") {
            this.show();
        }

    }

    this.readAjaxErrorCallback = function (xhr, textStatus, error) {
        var oResults = new nglEventParameters();
        var tObj = this;
        try {
            kendo.ui.progress($("#div" + this.IDKey + "Border"), false);
        } catch (err) {
            ngl.showInfoNotification("Cancel Progress Failure", "Your process is complete but the wait cursor could not be turned off, please refresh the page if the wait cursor does not go away.", null)
        }
        oResults.source = "readAjaxErrorCallback";
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed        
        oResults.CRUD = "read"
        oResults.error = new Error();
        oResults.error.name = "Read " + this.DataSourceName + " Failure"
        oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);

        if (ngl.isFunction(tPage[this.callback])) {
            tPage[this.callback](oResults);
        }
    }

    this.deleteSuccessCallback = function (data) {

        var oResults = new nglEventParameters();
        var tObj = this;
        oResults.source = "deleteSuccessCallback";
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed  
        oResults.CRUD = "delete"
        try {
            kendo.ui.progress($("#div" + this.IDKey + "Border"), false);
        } catch (err) {
            ngl.showInfoNotification("Cancel Progress Failure", "Your process is complete but the wait cursor could not be turned off, please refresh the page if the wait cursor does not go away.", null)
        }
        //clear any old return data in rData
        this.rData = null;
        try {
            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                if (typeof (data.Errors) !== 'undefined' && data.Errors != null) {
                    oResults.error = new Error();
                    oResults.error.name = "Delete " + this.DataSourceName + " Failure";
                    oResults.error.message = data.Errors;
                    ngl.showErrMsg(oResults.error.name, oResults.error.message, null);
                } else {
                    if (typeof (data.Data) !== 'undefined' && data.Data != null && ngl.isArray(data.Data) && data.Data[0] == true) {
                        oResults.datatype = "bool";
                        oResults.data = data.Data;
                        oResults.msg = "Success"
                        ngl.showSuccessMsg("Success your " + this.DataSourceName + " record has been deleted", tObj);
                        this.addNew();
                    }
                    else {
                        oResults.error = new Error();
                        oResults.error.name = "Unable to delete your " + this.DataSourceName + " record";
                        oResults.error.message = "The delete procedure returned false, please refresh your data and try again.";
                        ngl.showErrMsg(oResults.error.name, oResults.error.message, null);
                    }

                    try {
                        if (typeof (tObj.ParentIDKey) !== 'undefined' && tObj.ParentIDKey !== null) {
                            var grid = $("#" + tObj.ParentIDKey).data("kendoNGLGrid");
                            //Modified by RHR for v-8.5.3.006 on 10/29/2022 added logic to determine if the parent grid ID key is valid
                            // on nested tab controls                            
                            if (grid) {
                                grid.dataSource.page(1);
                                grid.dataSource.read();
                            }
                        }
                    } catch (err) {
                        ngl.showErrMsg(err.name, err.message, null);

                    }
                }
            } else {
                oResults.msg = "Success but no data was returned. Please refresh your page and check the results.";
                ngl.showSuccessMsg(oResults.msg, null);
            }
        } catch (err) {
            oResults.error = err
            ngl.showErrMsg(err.name, err.message, null);
        }
        if (ngl.isFunction(tPage[this.callback])) {
            tPage[this.callback](oResults);
        }

    }

    this.deleteAjaxErrorCallback = function (xhr, textStatus, error) {
        var oResults = new nglEventParameters();
        var tObj = this;
        try {
            kendo.ui.progress($("#div" + this.IDKey + "Border"), false);
        } catch (err) {
            ngl.showInfoNotification("Cancel Progress Failure", "Your process is complete but the wait cursor could not be turned off, please refresh the page if the wait cursor does not go away.", null)
        }
        oResults.source = "deleteAjaxErrorCallback";
        oResults.widget = tObj;
        oResults.msg = 'Failed'; //set default to Failed
        oResults.CRUD = "delete"
        oResults.error = new Error();
        oResults.error.name = "Delete " + this.DataSourceName + " Failure"
        oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);

        if (ngl.isFunction(tPage[this.callback])) {
            tPage[this.callback](oResults);
        }
    }

    // Generic functions for all Widgets
    this.validateRequiredFields = function (data, editing) {
        var oRet = new validationResults();
        var tObj = this;
        if (typeof (data) === 'undefined' || !ngl.isObject(data)) {
            //something went wrong so throw an error. this should never happen
            ngl.showValidationMsg("Save " + this.DataSourceName + " Validation Failed; No Data", "Please Contact Technical Support", tObj);
            oRet.Message = "Save " + this.DataSourceName + " Validation Failed; No Data";
            return oRet;
        }

        var blnValidated = true;
        var sValidationMsg = "";
        var sSpacer = "";
        if (typeof (this.dataFields) !== 'undefined' && ngl.isObject(this.dataFields) === true && ngl.isArray(this.dataFields) === true) {
            this.blnEditing = false;
            if (typeof (editing) === 'undefined') { this.blnEditing = false; } else { this.blnEditing = editing == "true"; }

            $.each(this.dataFields, function (index, item) {
                if (item.fieldName in data) {
                    //if (item.fieldAllowNull === false) { item.fieldRequired = true; }
                    if (
                        (tObj.blnEditing === true && ((item.fieldVisible === false && item.fieldReadOnly === true) || (item.fieldVisible === false && (item.fieldRequired === false || item.fieldInsertOnly === true))))
                        ||
                        (tObj.blnEditing === false && ((item.fieldVisible === false && item.fieldReadOnly === true) || (item.fieldVisible === false && item.fieldInsertOnly === false && item.fieldRequired === false) || (item.fieldVisible === true && item.fieldReadOnly === true && item.fieldInsertOnly === false)))
                    ) {
                        //valid 
                    } else {
                        if (item.fieldRequired === true) {
                            var field = data[item.fieldName];
                            // Modified by RHR for v-8.5.3.007 when a field is required it cannot be null or undefined
                            if (typeof (field) === 'undefined' || field === null) {
                                blnValidated = false; sValidationMsg += sSpacer + item.fieldCaption; sSpacer = ", ";
                            }
                            else {

                                if (isEmpty(field) === true) { blnValidated = false; sValidationMsg += sSpacer + item.fieldCaption; sSpacer = ", "; }
                            }
                        }
                    }
                }
                //if (tObj.blnEditing === true) { //update changes
                //    if (item.fieldVisible === false && (item.fieldRequired === false || item.fieldInsertOnly === true)) {
                //        //valid
                //    } else  if ((item.fieldAllowNull === false || item.fieldRequired === true)  && (item.fieldName in data)  ) {
                //        var field = data[item.fieldName];                       
                //        if (isEmpty(field)) { blnValidated = false; sValidationMsg += sSpacer + item.fieldCaption; sSpacer = ", "; }
                //    }
                //} else {
                //    if (item.fieldVisible === false  && item.fieldInsertOnly === false && item.fieldRequired === false ) {
                //        //valid
                //    } else if ((item.fieldAllowNull === false || item.fieldRequired === true )  && (item.fieldName in data)  )  { 
                //        var field = data[item.fieldName];                       
                //        if (isEmpty(field)) { blnValidated = false; sValidationMsg += sSpacer + item.fieldCaption; sSpacer = ", "; }
                //    }
                //}

            });
        }


        oRet.Success = blnValidated;
        oRet.Message = sValidationMsg;

        return oRet;
    }

    // Generic CRUD functions for all Widgets
    this.save = function () {
        var tObj = this;
        //debugger;
        if (typeof (this.dataFields) !== 'undefined' && ngl.isObject(this.dataFields) === true && ngl.isArray(this.dataFields) === true) {
            this.blnEditing = false;
            //if we have data we have an existing record to edit so set blnEditing to true
            if (typeof (this.data) !== 'undefined' && ngl.isObject(this.data)) {
                this.blnEditing = true;
            };
            this.data = new window[this.DataType]; //clear any old data
            //read the fields
            $.each(tObj.dataFields, function (index, item) {
                var field = tObj.data[item.fieldName];
                var elmt = $("#" + item.fieldTagID);
                if (typeof (elmt) !== 'undefined' && elmt != null) {
                    try {

                        if (
                            (tObj.blnEditing === true && ((item.fieldVisible === false && item.fieldReadOnly === true) || (item.fieldVisible === false && (item.fieldRequired === false || item.fieldInsertOnly === true))))
                            ||
                            (tObj.blnEditing === false && ((item.fieldVisible === false && item.fieldReadOnly === true) || (item.fieldVisible === false && item.fieldInsertOnly === false && item.fieldRequired === false) || (item.fieldVisible === true && item.fieldReadOnly === true && item.fieldInsertOnly === false)))
                        ) {
                            //hidden fields                                
                            var hVal = elmt.val();
                            if (item.fieldGroupSubType === nglGroupSubTypeEnum.NumericTextBox) {
                                field = ngl.nbrTryParse(hVal, ngl.nbrTryParse(item.fieldDefaultValue, 0));
                            } else {
                                field = hVal;
                            };
                        } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Checkbox) {
                            field = elmt.prop("checked");
                        } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.kendoSwitch) {
                            field = elmt.data("kendoSwitch").check();
                        } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.NumericTextBox) {
                            var numVal = elmt.data("kendoNumericTextBox").value();
                            field = ngl.nbrTryParse(numVal, ngl.nbrTryParse(item.fieldDefaultValue, 0));
                        } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.DatePicker) {
                            //console.log('Save');
                            //console.log(item);
                            //if (elmt.data("kendoDatePicker")) {
                            //    console.log(elmt.data("kendoDatePicker").value());
                            //}
                            // Modified by RHR for v-8.5.2.006 on 12/28/2022 fix for UTC conversion of KendoDatePicker  data to JSON string
                            //  removes the Location Key so time does not change when sent to server.
                            field = ngl.convertDateTimeToDateString(elmt.data("kendoDatePicker").value(), null, null); //elmt.data("kendoDatePicker").value();
                            //if (field) {
                            //    console.log(field);
                            //}
                        } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.DateTimePicker) {
                            // Modified by RHR for v-8.5.2.006 on 12/28/2022 fix for UTC conversion of kendoDateTimePicker  data to JSON string
                            //  removes the Location Key so time does not change when sent to server.
                            field = ngl.convertDateTimeToDateString(elmt.data("kendoDateTimePicker").value(), null, null); //elmt.data("kendoDateTimePicker").value();
                        } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.TimePicker) {
                            field = ngl.convertTimePickerToDateString(elmt.data("kendoTimePicker").value(), null, null);
                        } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Editor) {
                            field = elmt.data("kendoEditor").value();
                        } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.ColorPicker) {
                            field = elmt.data("kendoColorPicker").value();
                        } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.DropDownList) {
                            field = elmt.data('kendoDropDownList').value();
                        } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.MaskedTextBox) {
                            field = elmt.data("kendoMaskedTextBox").value();
                        };
                    } catch (err) {
                        //just read the jquery value
                        var sVal = elmt.val();
                        if (item.fieldGroupSubType === nglGroupSubTypeEnum.NumericTextBox) {
                            field = ngl.nbrTryParse(sVal, ngl.nbrTryParse(item.fieldDefaultValue, 0));
                        } else {
                            field = sVal;
                        };
                    }
                } else {
                    field = item.fieldDefaultValue;
                }
                tObj.data[item.fieldName] = field;
            });
            var oValidationResults = this.validateRequiredFields(this.data, this.blnEditing);

            //console.log("oValidationResults - 1: " + oValidationResults);
            if (typeof (oValidationResults) === 'undefined' || !ngl.isObject(oValidationResults)) {
                ngl.showErrMsg("Cannot validate " + this.DataSourceName + " Information", "Invalid validation procedure, please contact technical support", tObj);
                return;
            } else {
                if (oValidationResults.Success === false) {
                    var wdth = ($(window).width() / 10) * 6;
                    var hgt = ($(window).height() / 10) * 6;
                    ngl.Alert("Required Fields", oValidationResults.Message, wdth, hgt);
                    return;
                }
            }

            //save the changes         

            kendo.ui.progress($("#div" + this.IDKey + "Border"), true);

            setTimeout(function (tObj) {
                var oCRUDCtrl = new nglRESTCRUDCtrl();
                //if (tObj.data) { console.log(tObj.data);}
                if (oCRUDCtrl.update(tObj.API + "/POST", tObj.data, tObj, "saveSuccessCallback", "saveAjaxErrorCallback") == false) {
                    kendo.ui.progress($("#div" + this.IDKey + "Border"), false);
                }
            }, 2000, tObj);
        };

    }

    this.show = function () {
        var tObj = this;
        try {
            this.loadHTML();
            this.loadKendo();
            if (typeof (this.dataFields) === 'undefined' || ngl.isObject(this.dataFields) === false || ngl.isArray(this.dataFields) === false) { return; }

            this.edit(this.data);
        } catch (err) {

            ngl.showErrMsg("Cannot Load Data", err.message, null);

        }
        kendo.ui.progress($("#div" + this.IDKey + "Border"), false);
    }

    this.read = function (intControl) {

        var blnRet = true;
        var tObj = this;
        if (typeof (intControl) != 'undefined' && intControl != null) {
            this.rData = null;
            this.data = null;
            kendo.ui.progress($("#div" + this.IDKey + "Border"), true);
            setTimeout(function (tObj) {
                var oCRUDCtrl = new nglRESTCRUDCtrl();
                if (oCRUDCtrl.read(tObj.API + "/GET", intControl, tObj, "readSuccessCallback", "readAjaxErrorCallback") == false) {
                    blnRet = false;
                    kendo.ui.progress($("#div" + this.IDKey + "Border"), false);
                }
            }, 1, tObj);
        }
        return blnRet;
    }

    this.delete = function () {
        var blnRet = true;
        var tObj = this;
        var intControl
        if (typeof (this.data) === 'undefined' || ngl.isObject(this.data) === false) {
            ngl.showWarningMsg("Cannot Delete", "Invalid data. If this is a new record no changes have been saved so there is nothing to delete.", null);
            return false;
        }
        if (typeof (this.PKName) === 'undefined' || this.PKName == null || ((this.PKName in this.data) == false)) {
            ngl.showWarningMsg("Cannot Delete", "Invalid value for key field " + this.PKName + ". Please check the grid content management settings or contact technical support..", null);
            return false;
        }
        intControl = this.data[this.PKName];
        if (typeof (intControl) != 'undefined' && intControl != null) {
            this.rData = null;
            this.data = null;

            kendo.ui.progress($("#div" + this.IDKey + "Border"), true);
            setTimeout(function (tObj) {
                var oCRUDCtrl = new nglRESTCRUDCtrl();
                if (oCRUDCtrl.delete(tObj.API + "/DELETE", intControl, tObj, "deleteSuccessCallback", "deleteAjaxErrorCallback") == false) {
                    blnRet = false;
                    kendo.ui.progress($("#div" + this.IDKey + "Border"), false);
                }
            }, 200, tObj);
        } else {
            ngl.showWarningMsg("Cannot Delete", "Invalid value in key field " + this.PKName + ". If this is a new record no changes have been saved so there is nothing to delete.", null);
            return false;
        }
        return blnRet;
    }

    this.addNew = function () {
        this.data = null;
        this.loadHTML();
        this.loadKendo();
        $('#div' + this.IDKey + 'InsertOnly').show();
        this.edit(this.data);
    }

    this.edit = function (data) {
        //load data to the screen
        var tObj = this;
        if (typeof (data) !== 'undefined' && data != null && ngl.isObject(data)) {
            $('#div' + this.IDKey + 'InsertOnly').hide();
            if (typeof (this.dataFields) !== 'undefined' && ngl.isObject(this.dataFields) === true && ngl.isArray(this.dataFields) === true) {
                $.each(this.dataFields, function (index, item) {
                    if (item.fieldName in data) {
                        var blnReadOnly = false;
                        if (item.fieldReadOnly === true) { blnReadOnly = true; }
                        var dataItem = data[item.fieldName];
                        if (typeof (dataItem) !== 'undefined' && dataItem !== null) {
                            if ((item.fieldVisible === false && item.fieldReadOnly === true) || (item.fieldVisible === false && (item.fieldRequired === false || item.fieldInsertOnly === true))) {
                                //no kindo widgets for hidden fields  
                                $("#" + item.fieldTagID).val(dataItem);
                            } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Checkbox) {
                                if (dataItem !== true) { dataItem = false; }
                                $("#" + item.fieldTagID).prop('checked', dataItem);
                                $("#" + item.fieldTagID).prop("disabled", blnReadOnly);
                            } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.kendoSwitch) {
                                if (dataItem !== true) { dataItem = false; }
                                $("#" + item.fieldTagID).data("kendoSwitch").check(dataItem);
                                //$("#" + item.fieldTagID).data("kendoSwitch").value({ checked: dataItem });
                                $("#" + item.fieldTagID).data("kendoSwitch").enable(!blnReadOnly);
                                //$("#" + item.fieldTagID).prop("data-enable", !blnReadOnly);
                                //$("#" + item.fieldTagID).data("kendoSwitch").enabled(!blnReadOnly);
                            } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.NumericTextBox) {
                                $("#" + item.fieldTagID).data("kendoNumericTextBox").value(dataItem);
                                $("#" + item.fieldTagID).data("kendoNumericTextBox").readonly(blnReadOnly);
                                //$("#" + item.fieldTagID).data("kendoNumericTextBox").spinners(!blnReadOnly);
                                //$("#" + item.fieldTagID).data("kendoNumericTextBox").enable(!blnReadOnly); 
                            } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.DatePicker) {
                                //debugger; console.log(item);
                                //console.log('Edit');
                                //console.log(item);
                                //if (dataItem) {
                                //    console.log(dataItem);
                                //}
                                // Modified by RHR for v-8.5.2.006 on 12/28/2022 call convertTime to fix issue with KendoDatePicker not loading time properly
                                //  Note: testing if convertTime is needed here this code may not be required
                                $("#" + item.fieldTagID).data("kendoDatePicker").value(ngl.convertTime(dataItem));
                                $("#" + item.fieldTagID).data("kendoDatePicker").readonly(blnReadOnly);
                                //if ($("#" + item.fieldTagID).data("kendoDatePicker")) {
                                //    console.log($("#" + item.fieldTagID).data("kendoDatePicker").value());
                                //}
                                //$("#" +item.fieldTagID).data("kendoDatePicker").enable(!blnReadOnly);
                            } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.DateTimePicker) {
                                $("#" + item.fieldTagID).data("kendoDateTimePicker").value(dataItem);
                                $("#" + item.fieldTagID).data("kendoDateTimePicker").readonly(blnReadOnly);
                                //$("#" +item.fieldTagID).data("kendoDateTimePicker").enable(!blnReadOnly);
                            } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.TimePicker) {
                                // Modified by RHR for v-8.5.2.006 on 12/28/2022 call convertTime to fix issue with kendoTimePicker not loading time properly
                                $("#" + item.fieldTagID).data("kendoTimePicker").value(ngl.convertTime(dataItem));
                                $("#" + item.fieldTagID).data("kendoTimePicker").readonly(blnReadOnly);
                                //$("#" +item.fieldTagID).data("kendoTimePicker").enable(!blnReadOnly);
                            } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Editor) {
                                $("#" + item.fieldTagID).data("kendoEditor").value(dataItem);
                            } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.ColorPicker) {
                                $("#" + item.fieldTagID).data("kendoColorPicker").value({ value: dataItem });
                                $("#" + item.fieldTagID).data("kendoColorPicker").enabled(!blnReadOnly);
                            } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.DropDownList) {
                                $("#" + item.fieldTagID).data("kendoDropDownList").value(dataItem);
                                $("#" + item.fieldTagID).data("kendoDropDownList").readonly(blnReadOnly);
                            } else {
                                $("#" + item.fieldTagID).data("kendoMaskedTextBox").value(dataItem);
                                $("#" + item.fieldTagID).data("kendoMaskedTextBox").readonly(blnReadOnly);
                                //$("#" +item.fieldTagID).data("kendoMaskedTextBox").enable(!blnReadOnly);
                            };
                        };
                    };
                });
            } else {
                ngl.showValidationMsg(this.EditErrorTitle, this.EditErrorMsg, null); this.EditError = true; return false;
            };
        } else {
            $('#div' + this.IDKey + 'InsertOnly').show();
            //we are adding a new record
            if (typeof (this.dataFields) !== 'undefined' && ngl.isObject(this.dataFields) === true && ngl.isArray(this.dataFields) === true) {
                $.each(this.dataFields, function (index, item) {
                    var blnReadOnly = false;
                    if (item.fieldName == 'CarrTarEquipCarrierEquipControl') {
                        var sDebugString = item.fieldDefaultValue;
                    }
                    if (item.fieldInsertOnly === false && item.fieldReadOnly === true && item.fieldRequired === false) { blnReadOnly = true; }
                    var blnHasDefaultVal = false;
                    if (typeof (item.fieldDefaultValue) !== 'undefined' && item.fieldDefaultValue !== null && isEmpty(item.fieldDefaultValue) === false) { blnHasDefaultVal = true; }
                    if ((item.fieldVisible === false && item.fieldReadOnly === true) || (item.fieldVisible === false && item.fieldInsertOnly === false && item.fieldRequired === false) || (item.fieldVisible === true && item.fieldReadOnly === true && item.fieldInsertOnly === false)) {
                        //no kindo widgets for hidden fields  
                        if (blnHasDefaultVal === true) {
                            $("#" + item.fieldTagID).val(item.fieldDefaultValue);
                        } else {
                            $("#" + item.fieldTagID).val();
                        };
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Checkbox) {
                        if (blnHasDefaultVal === true) {
                            if (item.fieldDefaultValue == "true") {
                                $("#" + item.fieldTagID).prop('checked', true);
                            } else {
                                $("#" + item.fieldTagID).prop('checked', false);
                            }

                        } else {
                            $("#" + item.fieldTagID).prop('checked', false);
                        }
                        $("#" + item.fieldTagID).prop("disabled", blnReadOnly);

                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.kendoSwitch) {
                        if (blnHasDefaultVal === true) {
                            if (item.fieldDefaultValue == "true") {
                                $("#" + item.fieldTagID).data("kendoSwitch").check(true);
                            } else {
                                $("#" + item.fieldTagID).data("kendoSwitch").check(false);
                            }

                        } else {
                            $("#" + item.fieldTagID).data("kendoSwitch").check(false);
                            //$("#" + item.fieldTagID).data("kendoSwitch").value({ checked: false });
                        }
                        $("#" + item.fieldTagID).data("kendoSwitch").enable(!blnReadOnly);
                        //$("#" + item.fieldTagID).prop("data-enable", !blnReadOnly);
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.NumericTextBox) {
                        if (blnHasDefaultVal === true) {
                            $("#" + item.fieldTagID).data("kendoNumericTextBox").value(item.fieldDefaultValue);
                        } else {
                            $("#" + item.fieldTagID).data("kendoNumericTextBox").value();
                        }
                        $("#" + item.fieldTagID).data("kendoNumericTextBox").readonly(blnReadOnly);
                        //$("#" + item.fieldTagID).data("kendoNumericTextBox").spinners(!blnReadOnly); 
                        //$("#" + item.fieldTagID).data("kendoNumericTextBox").enable(!blnReadOnly); 
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.DatePicker) {
                        //debugger; console.log(item);
                        if (blnHasDefaultVal === true) {
                            $("#" + item.fieldTagID).data("kendoDatePicker").value(ngl.getShortDateString(item.fieldDefaultValue, new Date()));
                        } else {
                            $("#" + item.fieldTagID).data("kendoDatePicker").value();
                        }
                        $("#" + item.fieldTagID).data("kendoDatePicker").readonly(blnReadOnly);
                        //$("#" +item.fieldTagID).data("kendoDatePicker").enable(!blnReadOnly);
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.DateTimePicker) {
                        if (blnHasDefaultVal === true) {
                            $("#" + item.fieldTagID).data("kendoDateTimePicker").value(ngl.getShortDateString(item.fieldDefaultValue, new Date()));
                        } else {
                            $("#" + item.fieldTagID).data("kendoDateTimePicker").value();
                        }
                        $("#" + item.fieldTagID).data("kendoDateTimePicker").readonly(blnReadOnly);
                        //$("#" +item.fieldTagID).data("kendoDateTimePicker").enable(!blnReadOnly);
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.TimePicker) {
                        if (blnHasDefaultVal === true) {
                            $("#" + item.fieldTagID).data("kendoTimePicker").value(ngl.getShortDateString(item.fieldDefaultValue, new Date()));
                        } else {
                            $("#" + item.fieldTagID).data("kendoTimePicker").value();
                        }
                        $("#" + item.fieldTagID).data("kendoTimePicker").readonly(blnReadOnly);
                        //$("#" +item.fieldTagID).data("kendoTimePicker").enable(!blnReadOnly);
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.Editor) {
                        if (blnHasDefaultVal === true) {
                            $("#" + item.fieldTagID).data("kendoEditor").value(item.fieldDefaultValue);
                        } else {
                            $("#" + item.fieldTagID).data("kendoEditor").value();
                        }
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.ColorPicker) {
                        if (blnHasDefaultVal === true) {
                            $("#" + item.fieldTagID).data("kendoColorPicker").value({ value: item.fieldDefaultValue });
                        } else {
                            $("#" + item.fieldTagID).data("kendoColorPicker").value();
                        }
                        $("#" + item.fieldTagID).data("kendoColorPicker").enabled(!blnReadOnly);
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.DropDownList) {
                        if (!$("#" + item.fieldTagID).data("kendoDropDownList")) {
                            if (!$("#" + item.fieldTagID)) {
                                //do nothing                                
                            } else {
                                if (blnHasDefaultVal === true) {
                                    $("#" + item.fieldTagID).val(item.fieldDefaultValue);
                                } else {
                                    $("#" + item.fieldTagID).val();
                                }
                            }
                        } else {

                            if (blnHasDefaultVal === true) {
                                $("#" + item.fieldTagID).data("kendoDropDownList").value(item.fieldDefaultValue);
                            } else {
                                $("#" + item.fieldTagID).data("kendoDropDownList").select(0);
                            }
                            $("#" + item.fieldTagID).data("kendoDropDownList").readonly(blnReadOnly);
                        }
                    } else if (item.fieldGroupSubType === nglGroupSubTypeEnum.MaskedTextBox) {
                        if (blnHasDefaultVal === true) {
                            $("#" + item.fieldTagID).data("kendoMaskedTextBox").value(item.fieldDefaultValue);
                        } else {
                            $("#" + item.fieldTagID).data("kendoMaskedTextBox").value();
                        }
                        $("#" + item.fieldTagID).data("kendoMaskedTextBox").readonly(blnReadOnly);
                        //$("#" +item.fieldTagID).data("kendoMaskedTextBox").enable(!blnReadOnly);
                    };
                });
            } else {
                ngl.showValidationMsg(this.AddErrorTitle, this.AddErrorMsg, null); this.EditError = true; return false;
            };
        };
        return true;
    }

    this.close = function (e) {


    }

    /// loadDefaults sets up the callbacks cbSelect and cbSave
    /// all call backs return a reference to this object and a string message as parameters
    /// CallBackFunction can be used by the caller to handle return information.  it returns a nglEventParameters objec
    this.loadDefaults = function (ContainerDivID,
        IDKey,
        fieldData,
        Theme,
        CallBackFunction,
        API,
        DataType,
        DataSourceName,
        EditErrorMsg,
        EditErrorTitle,
        PKName,
        sNGLCtrlName) {

        var tObj = this;
        this.callback = CallBackFunction;
        this.dataFields = fieldData;
        this.lastErr = "";
        this.notifyOnError = true;
        this.notifyOnSuccess = true;
        this.notifyOnValidationFailure = true;
        this.ContainerDivID = ContainerDivID;
        this.IDKey = IDKey;
        this.Theme = Theme;
        this.API = API;
        this.DataType = DataType;
        this.DataSourceName = DataSourceName;
        this.EditErrorMsg = EditErrorMsg;
        this.EditErrorTitle = EditErrorTitle;
        this.PKName = PKName;
        this.sNGLCtrlName = sNGLCtrlName;


    }
}


//End NGL Data Entry Window Widgets
//**********************************************************************


//Added By LVV on 2/19/2019 for bug fix add Carrier Contact selection to Dispatch
function SelectContactCtrl() {
    //Generic properties for all Widgets
    kendoWindowsObj: null;
    oContainer: null;
    SelectedContactCarrierControl: 0;
    //Widget specific properties
    this.kendoWindowsObjUploadEventAdded = 0;

    //Widget specific functions
    this.loadHTML = function () {
        var tObj = this;
        var sHtml = '<div id="wndSelectContact"><a id="focusCancel" href="#"></a><div><div style="padding: 0px 10px 0px 10px;"><strong>Select a Contact from the list and click the save button</strong><div id="selectContactGrid"></div><input id="txtSelectedContactCarrierControl" type="hidden"/></div></div></div>';
        $("#divContactsWindow").html(sHtml);
    };

    this.loadKendo = function () {

        var control = this.SelectedContactCarrierControl;

        $("#selectContactGrid").kendoGrid({
            noRecords: { template: "<p>No records available.</p>" },
            selectable: "row",
            //autoBind: false,
            //height: 200,
            pageable: true,
            resizable: true,
            dataSource: {
                pageSize: 10,
                transport: {
                    read: function (options) {
                        var s = new AllFilter();
                        //s.sortName = $("#txtUOPSortField").val();
                        //s.sortDirection = $("#txtUOPSortDirection").val();
                        //s.filterName = $("#ddlUOPFilters").data("kendoDropDownList").value();
                        s.page = options.data.page;
                        s.skip = options.data.skip;
                        s.take = options.data.take;
                        s.CarrierControlFrom = control;

                        $.ajax({
                            url: 'api/Contact/GetUserLECarrierContacts',
                            contentType: 'application/json; charset=utf-8',
                            dataType: 'json',
                            data: { filter: JSON.stringify(s) },
                            headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                            success: function (data) {
                                options.success(data);
                                //console.log(data.Data);
                            },
                            error: function (result) { options.error(result); }
                        });
                    },
                    parameterMap: function (options, operation) { return options; }
                },
                schema: {
                    data: "Data",
                    total: "Count",
                    model: {
                        id: "ContactControl",
                        fields: {
                            ContactControl: { type: "number" },
                            ContactName: { type: "string" },
                            ContactTitle: { type: "string" },
                            ContactEmail: { type: "string" },
                            ContactPhone: { type: "string" },
                            ContactPhoneExt: { type: "string" },
                            Contact800: { type: "string" },
                            ContactFax: { type: "string" },
                            ContactDefault: { type: "boolean" },
                            ContactScheduler: { type: "boolean" },
                            ContactTender: { type: "boolean" },
                            ContactCarrierControl: { type: "number" },
                            ContactLECarControl: { type: "number" },
                            ContactCompControl: { type: "number" }
                        }
                    },
                    errors: "Errors"
                },
                error: function (xhr, textStatus, error) { ngl.showErrMsg("Get Contacts Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges(); }
            },
            columns: [
                { field: "ContactControl", title: "ContactControl", hidden: true },
                { field: "ContactName", title: "Name", template: "<span title='${ContactName}'>${ContactName}</span>" },
                { field: "ContactTitle", title: "Title", template: "<span title='${ContactTitle}'>${ContactTitle}</span>" },
                { field: "ContactEmail", title: "Email", template: "<span title='${ContactEmail}'>${ContactEmail}</span>" },
                { field: "ContactPhone", title: "Phone" },
                { field: "ContactPhoneExt", title: "Phone Ext" },
                { field: "Contact800", title: "800" },
                { field: "ContactFax", title: "Fax" },
                { field: "ContactDefault", title: "Dispatch Default", template: "<input type='checkbox' #= ContactDefault ? 'checked=checked' : '' # disabled='disabled' ></input>" },
                { field: "ContactScheduler", title: "Scheduling Contact", template: "<input type='checkbox' #= ContactScheduler ? 'checked=checked' : '' # disabled='disabled' ></input>" },
                { field: "ContactTender", title: "Tender", template: "<input type='checkbox' #= ContactTender ? 'checked=checked' : '' # disabled='disabled' ></input>", hidden: true },
                { field: "ContactCarrierControl", title: "ContactCarrierControl", hidden: true },
                { field: "ContactLECarControl", title: "ContactLECarControl", hidden: true },
                { field: "ContactCompControl", title: "ContactCompControl", hidden: true }
            ]
        });

    };

    //Generic Call back functions for all Widgets
    ////this.readSuccessCallback = function (data) {
    ////    //var windowWidget = $("#winDispatchDialog").data("kendoWindow");      
    ////    //kendo.ui.progress(windowWidget.element, false);
    ////    var oResults = new nglEventParameters();
    ////    var tObj = this;
    ////    oResults.source = "readSuccessCallback";
    ////    oResults.widget = tObj;
    ////    oResults.msg = 'Failed'; //set default to Failed   
    ////    //clear any old return data in rData
    ////    this.rData = null;
    ////    try {
    ////        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
    ////            if (typeof (data.Errors) !== 'undefined' && data.Errors != null) {
    ////                oResults.error = new Error();
    ////                oResults.error.name = "Read Dispatch Data Failure";
    ////                oResults.error.message = data.Errors;
    ////                ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
    ////            }
    ////            else {
    ////                this.rData = data.Data;
    ////                if (data.Data != null) {
    ////                    oResults.data = data.Data;
    ////                    oResults.datatype = "Dispatch";
    ////                    oResults.msg = "Success"
    ////                    this.data = data.Data[0];
    ////                    //this.show();
    ////                }
    ////                else {
    ////                    oResults.error = new Error();
    ////                    oResults.error.name = "Invalid Request";
    ////                    oResults.error.message = "No Data available.";
    ////                    ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
    ////                }
    ////            }
    ////        } else {
    ////            oResults.msg = "Success but no data was returned. Please refresh your page and try again.";
    ////            ngl.showSuccessMsg(oResults.msg, tObj);
    ////        }
    ////    } catch (err) {
    ////        oResults.error = err
    ////    }
    ////    if (ngl.isFunction(this.onRead)) {
    ////        this.onRead(oResults);
    ////    }
    ////    if (oResults.msg == "Success") {
    ////        this.show();
    ////    }

    ////}

    ////this.readAjaxErrorCallback = function (xhr, textStatus, error) {
    ////    //var windowWidget = $("#winDispatchDialog").data("kendoWindow");
    ////    //kendo.ui.progress(windowWidget.element, false);
    ////    var oResults = new nglEventParameters();
    ////    var tObj = this;
    ////    oResults.source = "readAjaxErrorCallback";
    ////    oResults.widget = tObj;
    ////    oResults.msg = 'Failed'; //set default to Failed  
    ////    oResults.error = new Error();
    ////    oResults.error.name = "Read Dispatch Data Failure"
    ////    oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
    ////    ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);

    ////    if (ngl.isFunction(this.onRead)) {
    ////        this.onRead(oResults);
    ////    }
    ////}


    // Generic CRUD functions for all Widgets
    this.read = function (intControl) {
        ////var blnRet = false;
        //////if not supported just return false
        //////return blnRet;
        ////var tObj = this;
        ////if (typeof (intControl) != 'undefined' && intControl != null) {
        ////    this.rData = null;
        ////    this.data = null;
        ////    var oCRUDCtrl = new nglRESTCRUDCtrl();
        ////    blnRet = oCRUDCtrl.read("Dispatching/GetBidToDispatch", intControl, tObj, "readSuccessCallback", "readAjaxErrorCallback");
        ////}
        ////return blnRet;

        this.show();
    }

    this.save = function () {
        var grid = $("#selectContactGrid").data("kendoGrid");
        var item = grid.dataItem(grid.select());

        this.oContainer.updateCarrierCont(item);
        this.kendoWindowsObj.close();
    }

    this.show = function () {
        var tObj = this;
        try {
            this.loadHTML();
            this.loadKendo();

            this.kendoWindowsObj = $("#wndSelectContact").kendoWindow({
                title: "Select Contact",
                modal: true,
                visible: false,
                //height: '50%',
                width: '75%',
                scrollable: true,
                actions: ["save", "Minimize", "Maximize", "Close"],
                close: function (e) { tObj.close(e); },
                deactivate: function () { this.destroy(); }
            }).data("kendoWindow");

            ////if (this.kendoWindowsObjUploadEventAdded === 0) {
            ////    this.kendoWindowsObj.wrapper.find(".k-svg-i-save").parent().click(function (e) { e.preventDefault(); tObj.save(); });
            ////}
            ////this.kendoWindowsObjUploadEventAdded = 1;
            ////this.kendoWindowsObj.center().open();

            try {
                this.kendoWindowsObj.wrapper.find(".k-svg-i-save").parent().click(function (e) { e.preventDefault(); tObj.save(); });
            }
            catch (err) { ngl.showInfoNotification("Save not available", "Could not load save method please reload the page to save", null); }
        }
        catch (err) {
            ngl.showErrMsg("Cannot show Window", err.message, null);
        }
        this.kendoWindowsObj.center().open();
    }

    this.close = function (e) {
        //do nothing
    }

    //loadDefaults sets up the callback cbSave
    //all call backs return a reference to this object and a string message as parameters
    this.loadDefaults = function (pageVariable, container) {
        this.oContainer = container;
        var tObj = this;
        if (typeof (pageVariable) !== 'undefined' && ngl.isObject(pageVariable)) {
            this.kendoWindowsObj = pageVariable;
        } else { this.kendoWindowsObj = null; }
    }
}

var contactTypeEnum = {
    None: 0,
    Lane: 1,
    Carrier: 2,
    Comp: 3
}

//Added By LVV on 12/19/2019 for v-8.2.1.004
//Widget for general contact selection, can be used anywhere - html data store in Views/SelectContactWindow.html
function SelectContactWndCtrl() {
    //Generic properties for all Widgets
    onSave: null;
    kendoWindowsObj: null;
    this.kendoWindowsObjUploadEventAdded = 0;
    //Widget specific properties
    FilterControl: 0;
    eContactType: 0;

    //Widget specific functions
    this.getContactType = function () { return this.eContactType; }

    this.getFilterControl = function (data) { return this.FilterControl; }

    this.configureGridDefault = function () {
        //DEFAULT
        //Name, Title, Email, Phone, Phone Ext, 800, Fax
        var grid = $("#selectContactWndCtrlGrid").data("kendoGrid");
        grid.hideColumn("ContactControl");
        grid.showColumn("ContactName");
        grid.showColumn("ContactTitle");
        grid.showColumn("ContactEmail");
        grid.showColumn("ContactPhone");
        grid.showColumn("ContactPhoneExt");
        grid.showColumn("Contact800");
        grid.showColumn("ContactFax");
        grid.hideColumn("ContactDefault");
        grid.hideColumn("ContactScheduler");
        grid.hideColumn("ContactTender");
        grid.hideColumn("ContactCarrierControl");
        grid.hideColumn("ContactLECarControl");
        grid.hideColumn("ContactCompControl");
    }

    this.configureGridLane = function () {
        //LANE
        //Name, Title, Email, Phone, Phone Ext, 800, Fax
        var grid = $("#selectContactWndCtrlGrid").data("kendoGrid");
        grid.hideColumn("ContactControl");
        grid.showColumn("ContactName");
        grid.showColumn("ContactTitle");
        grid.showColumn("ContactEmail");
        grid.showColumn("ContactPhone");
        grid.showColumn("ContactPhoneExt");
        grid.showColumn("Contact800");
        grid.showColumn("ContactFax");
        grid.hideColumn("ContactDefault");
        grid.hideColumn("ContactScheduler");
        grid.hideColumn("ContactTender");
        grid.hideColumn("ContactCarrierControl");
        grid.hideColumn("ContactLECarControl");
        grid.hideColumn("ContactCompControl");
    }

    this.configureGridCarrier = function () {
        //CARRIER
        //Name, Title, Phone, Phone Ext, Fax, 800, Email, Dispatch Default, Scheduling Contact
        var grid = $("#selectContactWndCtrlGrid").data("kendoGrid");
        grid.hideColumn("ContactControl");
        grid.showColumn("ContactName");
        grid.showColumn("ContactTitle");
        grid.showColumn("ContactEmail");
        grid.showColumn("ContactPhone");
        grid.showColumn("ContactPhoneExt");
        grid.showColumn("Contact800");
        grid.showColumn("ContactFax");
        grid.showColumn("ContactDefault");
        grid.showColumn("ContactScheduler");
        grid.hideColumn("ContactTender");
        grid.hideColumn("ContactCarrierControl");
        grid.hideColumn("ContactLECarControl");
        grid.hideColumn("ContactCompControl");
    }

    this.configureGridComp = function () {
        //COMPANY
        //Name, Title, Email, Phone, Phone Ext, 800, Fax, Notify
        var grid = $("#selectContactWndCtrlGrid").data("kendoGrid");
        grid.hideColumn("ContactControl");
        grid.showColumn("ContactName");
        grid.showColumn("ContactTitle");
        grid.showColumn("ContactEmail");
        grid.showColumn("ContactPhone");
        grid.showColumn("ContactPhoneExt");
        grid.showColumn("Contact800");
        grid.showColumn("ContactFax");
        grid.hideColumn("ContactDefault");
        grid.hideColumn("ContactScheduler");
        grid.showColumn("ContactTender");
        grid.hideColumn("ContactCarrierControl");
        grid.hideColumn("ContactLECarControl");
        grid.hideColumn("ContactCompControl");
    }

    this.configureWnd = function () {
        var tObj = this;
        var type = tObj.getContactType();
        switch (type) {
            case contactTypeEnum.None:
                this.kendoWindowsObj.title("Select Contact");
                tObj.configureGridDefault();
                break;
            case contactTypeEnum.Lane:
                this.kendoWindowsObj.title("Select Lane Contact");
                tObj.configureGridLane();
                break;
            case contactTypeEnum.Carrier:
                this.kendoWindowsObj.title("Select Carrier Contact");
                tObj.configureGridCarrier();
                break;
            case contactTypeEnum.Comp:
                this.kendoWindowsObj.title("Select Company Contact");
                tObj.configureGridComp();
                break;
        }
        //We need to reset the page before refresh to avoid problematic behavior since we are completely changing the data returned (not just filtering or sorting from one retured record set)
        $('#selectContactWndCtrlGrid').data('kendoGrid').dataSource.page(1);
        $('#selectContactWndCtrlGrid').data('kendoGrid').dataSource.read();
    }

    //Generic Call back functions for all Widgets

    // Generic CRUD functions for all Widgets
    this.save = function () {
        var tObj = this;
        var grid = $("#selectContactWndCtrlGrid").data("kendoGrid");
        var item = grid.dataItem(grid.select());
        if (!item) { ngl.showValidationMsg("Contact Record Required", "Please select a Contact to continue", tObj); return; }
        if (ngl.isFunction(this.onSave)) { this.onSave(item); }
        this.kendoWindowsObj.close();
    }

    // Generic functions for all Widgets
    this.show = function () {
        var tObj = this;

        this.kendoWindowsObj = $("#wndSelectContactWndCtrl").kendoWindow({
            title: "Select Contact",
            modal: true,
            visible: false,
            height: '75%',
            width: '75%',
            actions: ["save", "Minimize", "Maximize", "Close"],
            close: function (e) { tObj.close(e); }
        }).data("kendoWindow");

        if (this.kendoWindowsObjUploadEventAdded === 0) {
            $("#wndSelectContactWndCtrl").data("kendoWindow").wrapper.find(".k-svg-i-save").parent().click(function (e) { e.preventDefault(); tObj.save(); });
        }
        this.kendoWindowsObjUploadEventAdded = 1;

        this.configureWnd();
        this.kendoWindowsObj.center().open();
    }

    this.close = function (e) {
        //reset to default values
        this.eContactType = contactTypeEnum.None;
        this.FilterControl = 0;
    }

    //loadDefaults sets up the callbacks cbSelect and cbSave
    //all call backs return a reference to this object and a string message as parameters
    this.loadDefaults = function (pageVariable, saveCallBack) {
        this.onSave = saveCallBack;
        var tObj = this;
        if (typeof (pageVariable) !== 'undefined' && ngl.isObject(pageVariable)) {
            this.kendoWindowsObj = pageVariable;
            $("#selectContactWndCtrlGrid").kendoGrid({
                noRecords: { template: "<p>No records available.</p>" },
                selectable: "row",
                autoBind: false,
                //height: 200,
                pageable: { pageSize: 10 },
                resizable: true,
                dataSource: {
                    pageSize: 10,
                    transport: {
                        read: function (options) {
                            var s = new SelectContactFilters();
                            s.ContactType = tObj.getContactType();
                            s.Control = tObj.getFilterControl();
                            $.ajax({
                                url: 'api/Contact/GetContacts',
                                contentType: 'application/json; charset=utf-8',
                                dataType: 'json',
                                data: { filter: JSON.stringify(s) },
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                success: function (data) {
                                    options.success(data);
                                    //console.log(data.Data);
                                },
                                error: function (result) { options.error(result); }
                            });
                        },
                        parameterMap: function (options, operation) { return options; }
                    },
                    schema: {
                        data: "Data",
                        total: "Count",
                        model: {
                            id: "ContactControl",
                            fields: {
                                ContactControl: { type: "number" },
                                ContactName: { type: "string" },
                                ContactTitle: { type: "string" },
                                ContactEmail: { type: "string" },
                                ContactPhone: { type: "string" },
                                ContactPhoneExt: { type: "string" },
                                Contact800: { type: "string" },
                                ContactFax: { type: "string" },
                                ContactDefault: { type: "boolean" },
                                ContactScheduler: { type: "boolean" },
                                ContactTender: { type: "boolean" },
                                ContactCarrierControl: { type: "number" },
                                ContactLECarControl: { type: "number" },
                                ContactCompControl: { type: "number" }
                            }
                        },
                        errors: "Errors"
                    },
                    error: function (xhr, textStatus, error) { ngl.showErrMsg("Get Contacts Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges(); }
                },
                columns: [
                    { field: "ContactControl", title: "ContactControl", hidden: true },
                    { field: "ContactName", title: "Name", template: "<span title='${ContactName}'>${ContactName}</span>" },
                    { field: "ContactTitle", title: "Title", template: "<span title='${ContactTitle}'>${ContactTitle}</span>" },
                    { field: "ContactEmail", title: "Email", template: "<span title='${ContactEmail}'>${ContactEmail}</span>" },
                    { field: "ContactPhone", title: "Phone" },
                    { field: "ContactPhoneExt", title: "Phone Ext" },
                    { field: "Contact800", title: "800" },
                    { field: "ContactFax", title: "Fax" },
                    { field: "ContactDefault", title: "Dispatch Default", template: "<input type='checkbox' #= ContactDefault ? 'checked=checked' : '' # disabled='disabled' ></input>" },
                    { field: "ContactScheduler", title: "Scheduling Contact", template: "<input type='checkbox' #= ContactScheduler ? 'checked=checked' : '' # disabled='disabled' ></input>" },
                    { field: "ContactTender", title: "Notify", template: "<input type='checkbox' #= ContactTender ? 'checked=checked' : '' # disabled='disabled' ></input>", hidden: true },
                    { field: "ContactCarrierControl", title: "ContactCarrierControl", hidden: true },
                    { field: "ContactLECarControl", title: "ContactLECarControl", hidden: true },
                    { field: "ContactCompControl", title: "ContactCompControl", hidden: true }
                ]
            });
        } else { this.kendoWindowsObj = null; }
    }

}


//this function does is not complete.  the original idea was to create a 
//wrapper method to the C# content management logic to be executed by JavaScript
//time was not permitted to finish this logic.  see RateShopping or Test.html for 
//how this is implemented manually without the widget
//function NGLJavaScriptCMGridWithEditCtrl() {
//    this.dataFields = [];
//    this.callback = "";
//    this.lastErr = "";
//    this.notifyOnError = true;
//    this.notifyOnSuccess = true;
//    this.notifyOnValidationFailure = true;
//    this.data =  null;
//    this.DataType = "";
//    this.ContainerDivID = "";
//    this.IDKey = "id1234";
//    this.ParentIDKey = null;
//    this.API = null;
//    this.DataSourceName = "";
//    this.EditErrorMsg = "";
//    this.EditErrorTitle = "";
//    this.AddErrorMsg = "";
//    this.AddErrorTitle = "";
//    this.wndEditObj = null;
//    this.wdgtEditWindCtrl = null;

//    // ************** Start BookPkgGrid Functions ******************
//    //Widget object is wdgtBookPkgGridEdit
//    this. iBookPkgPkgDescControlID = '0';
//    var ddlBookPkgPkgDescControl = undefined;
//    // implement widget callback logic
//    this.execEditWidgtCtrlCB = function (oResults) {
//        if (!oResults) { return; }
//        if (oResults.source == "showWidgetCallback") {
//            //if (oResults.source == "showWidgetCallback"  && iCarrTarFeesAccessorialCodeID == '0'  ){
//            iBookPkgPkgDescControlID = wdgtBookPkgGridEdit.GetFieldID("BookPkgPkgDescControl");
//            ddlBookPkgPkgDescControl = $("#" + iBookPkgPkgDescControlID).data("kendoDropDownList");
//            if (ddlBookPkgPkgDescControl) {
//                ddlBookPkgPkgDescControl.bind("change", BookPkgPkgDescControl_change);
//            }

//        } else if (oResults.source == "saveDataCB") {
//            //update the item information and reload the grid
//            var i = oResults.data
//            var intItemNumIndex =  1;
//            if (oResults.CRUD == "create") {

//                if (typeof (orderitems) === 'undefined' || orderitems === null) {
//                    orderitems = new Array();
//                } else {
//                    for (index = 0; index < orderitems.length; ++index) {
//                        var item = orderitems[index];
//                        if (isNaN(item.ItemNumber) == false && item.ItemNumber > intItemNumIndex) {
//                            intItemNumIndex = item.ItemNumber + 1
//                        }                                
//                    }
//                }
//                var item = new rateReqItem();
//                item.ItemNumber = intItemNumIndex;
//                item.Description = i.Description;
//                item.FreightClass = i.FreightClass;
//                item.Weight = i.Weight;
//                item.PackageType = i.PackageType;
//                item.PalletCount = i.PalletCount;
//                item.Quantity = i.Quantity;
//                item.Stackable = i.Stackable;
//                item.Length = i.Length;
//                item.Width = i.Width;
//                item.Height = i.Height;
//                item.NMFCItem = i.NMFCItem;
//                item.NMFCSub = i.NMFCSub;
//                orderitems.push(item)
//            } else {
//                if (orderitems.length > 0) {

//                    for (index = 0; index < orderitems.length; ++index) {
//                        var item = orderitems[index];
//                        if (item.ItemNumber == i.ItemNumber) {
//                            item.Description = i.Description;
//                            item.FreightClass = i.FreightClass;
//                            item.Weight = i.Weight;
//                            item.PackageType = i.PackageType;
//                            item.PalletCount = i.PalletCount;
//                            item.Quantity = i.Quantity;
//                            item.Stackable = i.Stackable;
//                            item.Length = i.Length;
//                            item.Width = i.Width;
//                            item.Height = i.Height;
//                            item.NMFCItem = i.NMFCItem;
//                            item.NMFCSub = i.NMFCSub;
//                            break;
//                        }
//                    }
//                }
//            }
//            if (orderitems.length > 0) {
//                for (index = 0; index < orderitems.length; ++index) {
//                    var item = orderitems[index];
//                    TotalCases += parseInt(item.Quantity);
//                    TotalWgt += parseFloat(item.Weight);
//                    TotalPlts += parseFloat(item.PalletCount);
//                }
//            }
//            else {
//                TotalCases = 1;
//                TotalWgt = 1;
//                TotalPlts = 1;
//            }
//            $("#txtTotalWgt").data("kendoNumericTextBox").value(TotalWgt);
//            $("#txtTotalPlts").data("kendoNumericTextBox").value(TotalPlts);
//            $("#txtTotalCases").data("kendoNumericTextBox").value(TotalCases);

//            $('#Items').data('kendoGrid').dataSource.read();
//            //wdgtBookPkgGridEdit.close();
//            //var grid = $("#Items").data("kendoNGLGrid");
//            //if (grid) { grid.dataSource.read(); }

//        }

//    }


//    //change event handler
//    function BookPkgPkgDescControl_change(e) {
//        if (ddlBookPkgPkgDescControl) {
//            var iCodeValue = ddlBookPkgPkgDescControl.value();
//            if (iCodeValue) {
//                updateSelectedBookPkgPkgDesc(iCodeValue);
//            }
//        }
//    }
//    //read data to update children when list selection changes        
//    function updateSelectedBookPkgPkgDesc(key) {
//        var oCRUDCtrl = new nglRESTCRUDCtrl();
//        var blnRet = oCRUDCtrl.read("PackageDescription", key, tPage, "readBookPkgPkgDescCallback", "readBookPkgPkgDescAjaxErrorCallback", tPage);
//        return true;
//    }
//    //read selected data call back from list
//    // used to update children when list selection changes       
//    function readBookPkgPkgDescCallback(data) {
//        try {
//            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
//                if (typeof (data.Errors) !== 'undefined' && data.Errors != null) {
//                    //on error do nothing for now

//                    //oResults.error = new Error();
//                    //oResults.error.name = "Read " + this.DataSourceName + " Failure";
//                    //oResults.error.message = data.Errors;
//                    //ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
//                }
//                else if (data.Data != null) {
//                    var record = data.Data[0];
//                    if (typeof (record) !== 'undefined' && record != null && ngl.isObject(record)) {
//                        //AccessorialVariableCode maps to CalcFormula List	CarrTarFeesVariableCode  DropDownList
//                        //get the id
//                        var sVal = '';
//                        var sFieldID = ''
//                        if ("PkgDescDesc" in record) {
//                            sVal = record["PkgDescDesc"];
//                            sFieldID = wdgtBookPkgGridEdit.GetFieldID("Description");
//                            var domItem = $("#" + sFieldID).data("kendoMaskedTextBox");
//                            if (domItem) {
//                                domItem.value(sVal);
//                            }
//                        }

//                        if ("PkgDescFAKClass" in record) {
//                            sVal = record["PkgDescFAKClass"];
//                            sFieldID = wdgtBookPkgGridEdit.GetFieldID("FreightClass");
//                            var domItem = $("#" + sFieldID).data("kendoMaskedTextBox");
//                            if (domItem) { domItem.value(sVal); }
//                        }

//                        if ("PkgDescNMFCClass" in record) {
//                            sVal = record["PkgDescNMFCClass"];
//                            sFieldID = wdgtBookPkgGridEdit.GetFieldID("NMFCItem");
//                            var domItem = $("#" + sFieldID).data("kendoMaskedTextBox");
//                            if (domItem) { domItem.value(sVal); }
//                        }
//                        if ("PkgDescNMFCSubClass" in record) {
//                            sVal = record["PkgDescNMFCSubClass"];
//                            sFieldID = wdgtBookPkgGridEdit.GetFieldID("NMFCSub");
//                            var domItem = $("#" + sFieldID).data("kendoMaskedTextBox");
//                            if (domItem) { domItem.value(sVal); }
//                        }
//                    }
//                }
//            }
//        } catch (err) {
//            ngl.showErrMsg(err.name, err.message, tObj);
//        }
//    }
//    //handle any ajax errors
//    function readBookPkgPkgDescAjaxErrorCallback(xhr, textStatus, error) {
//        ngl.showErrMsg("Read Package Desciption Failure",
//            formatAjaxJSONResponsMsgs(xhr, textStatus, error, 'Failed'),
//            tObj);
//    }

//    // ************** End BookPkgGrid Functions ******************
//    function openAddNewBookPkgGridWindow(e, fk) {

//        if ((typeof (execBeforeBookPkgGridInsert) === 'undefined' || ngl.isFunction(execBeforeBookPkgGridInsert) === false) || (execBeforeBookPkgGridInsert(e, fk, wdgtBookPkgGridEdit) === true)) {
//            wdgtBookPkgGridEdit.data = null;

//            wdgtBookPkgGridEdit.show();
//        }

//    }

//    function openEditBookPkgGridWindow(e) {

//        var data = this.dataItem($(e.currentTarget).closest("tr"));                

//        wdgtBookPkgGridEdit.data = data;
//        wdgtBookPkgGridEdit.show();
//        //wdgtBookPkgGridEdit.read(data.BookPkgControl);

//    }

//    function deleteBookPkgGridRecord(iRet, data) {

//        if (typeof (iRet) === 'undefined' || iRet === null || iRet === 0) { return; }

//        wdgtBookPkgGridEdit.delete(data.BookPkgControl);

//    }

//    function confirmDeleteBookPkgGridRecord(e) {

//        var item = this.dataItem($(e.currentTarget).closest("tr"));

//        if (typeof (item) === 'undefined' || ngl.isObject(item) == false) { return; }

//        ngl.OkCancelConfirmation("Delete Selected Packages Record", "Warning! This action cannot be undone.  Are you sure?", 400, 400, null, "deleteBookPkgGridRecord", item);

//    }



//    var objBookPkgGridDataFields = [                
//    { fieldID: "33389", fieldTagID: "id62120181107153246430266", fieldCaption: "Item Number", fieldName: "ItemNumber", fieldDefaultValue: "", fieldGroupSubType: 13, fieldReadOnly: true, fieldFormat: "", fieldTemplate: "", fieldAllowNull: true, fieldMaxlength: 50, fieldVisible: false, fieldRequired: true, fieldInsertOnly: false, fieldAPIReference: "", fieldAPIFilterID: "", fieldParentTagID: "id621201811071350538611498", fieldSequenceNo: 101, fieldCRUDTagID: "", fieldWidgetAction: "", fieldWidgetActionKey: "", fieldCssClass: "", fieldStyle: "" }
//   , { fieldID: "33580", fieldTagID: "id6229201905011315073981309", fieldCaption: "Pkg Desc", fieldName: "BookPkgPkgDescControl", fieldDefaultValue: "", fieldGroupSubType: 11, fieldReadOnly: false, fieldFormat: "{0:n0}", fieldTemplate: "", fieldAllowNull: false, fieldMaxlength: 50, fieldVisible: true, fieldRequired: true, fieldInsertOnly: false, fieldAPIReference: "vLookupList/GetUserDynamicList", fieldAPIFilterID: "37", fieldParentTagID: "id621201811071350538611498", fieldSequenceNo: 1, fieldCRUDTagID: "", fieldWidgetAction: "", fieldWidgetActionKey: "", fieldCssClass: "", fieldStyle: "" }
//   , { fieldID: "33382", fieldTagID: "id62120181107153246443180", fieldCaption: "Package Type", fieldName: "PackageType", fieldDefaultValue: "", fieldGroupSubType: 11, fieldReadOnly: false, fieldFormat: "{0:n0}", fieldTemplate: "", fieldAllowNull: false, fieldMaxlength: 50, fieldVisible: false, fieldRequired: true, fieldInsertOnly: false, fieldAPIReference: "vLookupList/GetStaticList", fieldAPIFilterID: "64", fieldParentTagID: "id621201811071350538611498", fieldSequenceNo: 2, fieldCRUDTagID: "", fieldWidgetAction: "", fieldWidgetActionKey: "", fieldCssClass: "", fieldStyle: "" }
//   , { fieldID: "33370", fieldTagID: "id62120181107153246431292", fieldCaption: "Count", fieldName: "PalletCount", fieldDefaultValue: "", fieldGroupSubType: 15, fieldReadOnly: false, fieldFormat: "{0:n0}", fieldTemplate: "", fieldAllowNull: false, fieldMaxlength: 50, fieldVisible: true, fieldRequired: false, fieldInsertOnly: false, fieldAPIReference: "", fieldAPIFilterID: "", fieldParentTagID: "id621201811071350538611498", fieldSequenceNo: 3, fieldCRUDTagID: "", fieldWidgetAction: "", fieldWidgetActionKey: "", fieldCssClass: "", fieldStyle: "" }
//   , { fieldID: "33371", fieldTagID: "id62120181107153246432175", fieldCaption: "Desc", fieldName: "Description", fieldDefaultValue: "", fieldGroupSubType: 13, fieldReadOnly: false, fieldFormat: "", fieldTemplate: "", fieldAllowNull: true, fieldMaxlength: 255, fieldVisible: true, fieldRequired: false, fieldInsertOnly: false, fieldAPIReference: "", fieldAPIFilterID: "", fieldParentTagID: "id621201811071350538611498", fieldSequenceNo: 4, fieldCRUDTagID: "", fieldWidgetAction: "", fieldWidgetActionKey: "", fieldCssClass: "", fieldStyle: "" }
//   , { fieldID: "33372", fieldTagID: "id62120181107153246433175", fieldCaption: "FAK", fieldName: "FreightClass", fieldDefaultValue: "", fieldGroupSubType: 13, fieldReadOnly: false, fieldFormat: "", fieldTemplate: "", fieldAllowNull: true, fieldMaxlength: 20, fieldVisible: true, fieldRequired: false, fieldInsertOnly: false, fieldAPIReference: "", fieldAPIFilterID: "", fieldParentTagID: "id621201811071350538611498", fieldSequenceNo: 5, fieldCRUDTagID: "", fieldWidgetAction: "", fieldWidgetActionKey: "", fieldCssClass: "", fieldStyle: "" }
//   , { fieldID: "33373", fieldTagID: "id62120181107153246441176", fieldCaption: "NMFC", fieldName: "NMFCItem", fieldDefaultValue: "", fieldGroupSubType: 13, fieldReadOnly: false, fieldFormat: "", fieldTemplate: "", fieldAllowNull: true, fieldMaxlength: 20, fieldVisible: true, fieldRequired: false, fieldInsertOnly: false, fieldAPIReference: "", fieldAPIFilterID: "", fieldParentTagID: "id621201811071350538611498", fieldSequenceNo: 6, fieldCRUDTagID: "", fieldWidgetAction: "", fieldWidgetActionKey: "", fieldCssClass: "", fieldStyle: "" }
//   , { fieldID: "33374", fieldTagID: "id62120181107153246442177", fieldCaption: "Sub Class", fieldName: "NMFCSub", fieldDefaultValue: "", fieldGroupSubType: 13, fieldReadOnly: false, fieldFormat: "", fieldTemplate: "", fieldAllowNull: true, fieldMaxlength: 20, fieldVisible: true, fieldRequired: false, fieldInsertOnly: false, fieldAPIReference: "", fieldAPIFilterID: "", fieldParentTagID: "id621201811071350538611498", fieldSequenceNo: 7, fieldCRUDTagID: "", fieldWidgetAction: "", fieldWidgetActionKey: "", fieldCssClass: "", fieldStyle: "" }
//   , { fieldID: "33375", fieldTagID: "id62120181107153246437178", fieldCaption: "Length", fieldName: "Length", fieldDefaultValue: "", fieldGroupSubType: 15, fieldReadOnly: false, fieldFormat: "{0:n4}", fieldTemplate: "", fieldAllowNull: false, fieldMaxlength: 50, fieldVisible: true, fieldRequired: false, fieldInsertOnly: false, fieldAPIReference: "", fieldAPIFilterID: "", fieldParentTagID: "id621201811071350538611498", fieldSequenceNo: 8, fieldCRUDTagID: "", fieldWidgetAction: "", fieldWidgetActionKey: "", fieldCssClass: "", fieldStyle: "" }
//   , { fieldID: "33376", fieldTagID: "id62120181107153246447178", fieldCaption: "Width", fieldName: "Width", fieldDefaultValue: "", fieldGroupSubType: 15, fieldReadOnly: false, fieldFormat: "{0:n4}", fieldTemplate: "", fieldAllowNull: false, fieldMaxlength: 50, fieldVisible: true, fieldRequired: false, fieldInsertOnly: false, fieldAPIReference: "", fieldAPIFilterID: "", fieldParentTagID: "id621201811071350538611498", fieldSequenceNo: 9, fieldCRUDTagID: "", fieldWidgetAction: "", fieldWidgetActionKey: "", fieldCssClass: "", fieldStyle: "" }
//   , { fieldID: "33377", fieldTagID: "id62120181107153246436176", fieldCaption: "Height", fieldName: "Height", fieldDefaultValue: "", fieldGroupSubType: 15, fieldReadOnly: false, fieldFormat: "{0:n4}", fieldTemplate: "", fieldAllowNull: false, fieldMaxlength: 50, fieldVisible: true, fieldRequired: false, fieldInsertOnly: false, fieldAPIReference: "", fieldAPIFilterID: "", fieldParentTagID: "id621201811071350538611498", fieldSequenceNo: 10, fieldCRUDTagID: "", fieldWidgetAction: "", fieldWidgetActionKey: "", fieldCssClass: "", fieldStyle: "" }
//   , { fieldID: "33378", fieldTagID: "id62120181107153246446180", fieldCaption: "Wgt", fieldName: "Weight", fieldDefaultValue: "", fieldGroupSubType: 15, fieldReadOnly: false, fieldFormat: "{0:n2}", fieldTemplate: "", fieldAllowNull: false, fieldMaxlength: 50, fieldVisible: true, fieldRequired: false, fieldInsertOnly: false, fieldAPIReference: "", fieldAPIFilterID: "", fieldParentTagID: "id621201811071350538611498", fieldSequenceNo: 11, fieldCRUDTagID: "", fieldWidgetAction: "", fieldWidgetActionKey: "", fieldCssClass: "", fieldStyle: "" }
//   , { fieldID: "33379", fieldTagID: "id62120181107153246444182", fieldCaption: "Stack", fieldName: "Stackable", fieldDefaultValue: "", fieldGroupSubType: 16, fieldReadOnly: false, fieldFormat: "", fieldTemplate: "<input type='checkbox' #= Stackable ? 'checked=checked' : '' # disabled='disabled' ></input>", fieldAllowNull: false, fieldMaxlength: 50, fieldVisible: true, fieldRequired: false, fieldInsertOnly: false, fieldAPIReference: "", fieldAPIFilterID: "", fieldParentTagID: "id621201811071350538611498", fieldSequenceNo: 12, fieldCRUDTagID: "", fieldWidgetAction: "", fieldWidgetActionKey: "", fieldCssClass: "", fieldStyle: "" }
//   , { fieldID: "33388", fieldTagID: "id62120181107153246429175", fieldCaption: "Qty", fieldName: "Quantity", fieldDefaultValue: "", fieldGroupSubType: 15, fieldReadOnly: false, fieldFormat: "{0:n0}", fieldTemplate: "", fieldAllowNull: true, fieldMaxlength: 50, fieldVisible: false, fieldRequired: false, fieldInsertOnly: false, fieldAPIReference: "", fieldAPIFilterID: "", fieldParentTagID: "id621201811071350538611498", fieldSequenceNo: 101, fieldCRUDTagID: "", fieldWidgetAction: "", fieldWidgetActionKey: "", fieldCssClass: "", fieldStyle: "" }
//    ];

//    /// loadDefaults sets up the callbacks cbSelect and cbSave
//    /// all call backs return a reference to this object and a string message as parameters
//    /// CallBackFunction can be used by the caller to handle return information.  it returns a nglEventParameters objec
//    this.loadDefaults = function (ContainerDivID, 
//                                    WindowPageVariable, 
//                                    IDKey, 
//                                    ParentIDKey,
//                                    Theme, 
//                                    CallBackFunction, 
//                                    API, 
//                                    DataSourceName,
//                                    EditErrorMsg,
//                                    EditErrorTitle,
//                                    AddErrorMsg,
//                                    AddErrorTitle) {

//        var tObj = this;
//        this.lastErr = "";
//        this.notifyOnError = true;
//        this.notifyOnSuccess = true;
//        this.notifyOnValidationFailure = true;
//        this.ContainerDivID = ContainerDivID;
//        this.IDKey = IDKey;
//        this.ParentIDKey = ParentIDKey;
//        this.Theme = Theme;
//        this.kendoWindowsObj = WindowPageVariable;
//        this.API = API;
//        this.callback = CallBackFunction;        
//        window[IDKey + "DataType"] = new Object();
//        this.DataType =  window[IDKey + "DataType"];
//        $.each(tObj.dataFields, function (index, item){         
//            tObj.DataType[item.fieldName] = '';            
//        });          
//        this.DataSourceName = DataSourceName;
//        this.EditErrorMsg = EditErrorMsg;
//        this.EditErrorTitle = EditErrorTitle;
//        this.AddErrorMsg = AddErrorMsg;
//        this.AddErrorTitle = AddErrorTitle;
//        this.wndEditObj = WindowPageVariable;
//        this.wdgtEditWindCtrl = new NGLEditWindCtrl();
//        //note: the EditWindCtrl requires a unique callback name for each object
//        //so the callback function name sent to the EditWindCtrl must use the ID Key
//        // we always create the function here.
//        //EditWindCtrl call back
//        var sEDITWinCtrlCB = IDKey + "EDITWinCtrlCB"
//        tPage[IDKey + "EDITWinCtrlCB"] =  function (oResults) {
//            tObj.execEditWidgtCtrlCB(oResults);
//        }
//        this.wdgtEditWindCtrl.loadDefaults(ContainerDivID, WindowPageVariable, IDKey, ParentIDKey, this.dataFields, Theme,sEDITWinCtrlCB, API, IDKey + "DataType", DataSourceName, EditErrorMsg,EditErrorTitle,AddErrorMsg, AddErrorTitle);


//    }
//}
//}

function TrimbleMapData() {
    Container: "";
    MapStyle: "";
    VehicleType: "";
    //MapStops: [];
    //MapStopsData: [];
    //TrackStops: [];
    //TrackStopsData: [];
}

function StopDetails() {
    //StopTitle: "";
    StopDescription: "";
    StopColor: "";
    StopLabel: "";
    StopType: "";
    Address: "";
    City: "";
    State: "";
    Zip: "";
    Lat = 0.0;
    Lng = 0.0;
}

function geoCode(userdata) {
    TrimbleMaps.APIKey = TrimbleAPIKey //'C36349D0A5F5D440AAC0CB8A0287F02C';
    //debugger;
    var lat = 0;
    var lng = 0;
    new TrimbleMaps.Geocoder.geocode({
        address: {
            addr: userdata.Address,
            city: userdata.City,
            state: userdata.State,
            zip: userdata.Zip,
            //region: TrimbleMaps.Common.Region.NA
        },
        listSize: 1,
        success: function (response) {
            lat = response[0]["Coords"]["Lat"];
            lng = response[0]["Coords"]["Lon"];
        },
        failure: function (response) {
        }
    });
    return new TrimbleMaps.LngLat(lng, lat);
}

function GetgeoCoords(userdata) {
    TrimbleMaps.APIKey = TrimbleAPIKey //'C36349D0A5F5D440AAC0CB8A0287F02C';
    var geocoord = "";
    //var geocoord = [];
    var obj = {
        Longitude: 0,
        Latitude: 0
    };
    new TrimbleMaps.Geocoder.geocode({
        address: {
            addr: userdata.Address,
            city: userdata.City,
            state: userdata.State,
            zip: userdata.Zip,
            //region: TrimbleMaps.Common.Region.NA
        },
        listSize: 1,
        success: function (response) {
            //obj.Longitude = response[0]["Coords"]["Lon"];
            //obj.Latitude = response[0]["Coords"]["Lat"];            
            geocoord = (response[0]["Coords"]["Lon"]).toString() + "," + (response[0]["Coords"]["Lat"]).toString();
            //console.log(response);
            //console.log(geocoord);
            //geocoord[0] = response[0]["Coords"]["Lon"];
            //geocoord[1] = response[0]["Coords"]["Lat"];
        },
        failure: function (response) {
            geocoord = (response[0]["Coords"]["Lon"]).toString() + "," + (response[0]["Coords"]["Lat"]).toString();
        }
    });
    //geocoord = (response[0]["Coords"]["Lon"]).toString() + "," + (response[0]["Coords"]["Lat"]).toString();
    //console.log(geocoord);
    return geocoord;
}

function AddMapStop(lng, lat) {
    return new TrimbleMaps.LngLat(lng, lat);
}

//ADDED TRIMBLE MAP FUNCTION TO SHOW TRIMBLE MAP ON PAGES OCT 16, 20202
function showTrimbleMap(Container, MapOptions, MapStops, TrackStops, MapStopsData, TrackStopsData) {
    TrimbleMaps.APIKey = TrimbleAPIKey //'C36349D0A5F5D440AAC0CB8A0287F02C';
    var VehType = "";
    var routType = "";

    const map = new TrimbleMaps.Map({
        container: Container, // container id
        style: TrimbleMaps.Common.Style.TRANSPORTATION, //hosted style id
        center: MapStops[0],// starting position        
        zoom: 3, //4, // starting zoom to region (US)     //arround: MapStops[0] , 
        trackResize: true
    });


    map.addControl(new TrimbleMaps.NavigationControl());

    const scale = new TrimbleMaps.ScaleControl({
        maxWidth: 50,
        unit: 'imperial'
    });

    map.addControl(scale);

    //Set chosen Map stype
    if (MapOptions.MapStyle != "") {
        map.setStyle(TrimbleMaps.Common.Style[MapOptions.MapStyle]);
    }

    var markerHeight = 50, markerRadius = 10, linearOffset = 25;
    var popupOffsets = {
        'top': [0, 0],
        'top-left': [0, 0],
        'top-right': [0, 0],
        'bottom': [0, -markerHeight],
        'bottom-left': [linearOffset, (markerHeight - markerRadius + linearOffset) * -1],
        'bottom-right': [-linearOffset, (markerHeight - markerRadius + linearOffset) * -1],
        'left': [markerRadius, (markerHeight - markerRadius) * -1],
        'right': [-markerRadius, (markerHeight - markerRadius) * -1]
    };

    //Set chosen Vehicle type
    if (MapOptions.VehicleType != "") {
        VehType = MapOptions.VehicleType;
    }

    //Set chosen Route type
    if (MapOptions.RouteType != "") {
        routType = MapOptions.RouteType;
    }

    const myRoute = new TrimbleMaps.Route({
        routeId: "myRoute",
        stops: MapStops,
        VehicleType: VehType,
        RouteType: routType
    });

    //if (ngl.isObject(TrackStops)) {
    //    if (TrackStops.length > 0) {
    //        const myRouteTrack = new TrimbleMaps.Route({
    //            routeId: "myRouteTrack",
    //            stops: TrackStops,
    //            VehicleType: VehType,
    //            RouteType: routType
    //        });
    //    }
    //}

    //MAP IT Features Collection
    const gMapfeatures = [];
    const gsourceMapfeature = [];
    for (var i = 0; i < MapStops.length; i++) {
        if (i == 0) {
            gMapfeatures.push({
                type: 'Feature',
                properties: {
                    name: 'stop' + i,//currently pop up is working for only one pin;Test Ex.Need to check how to give dynamic Name.
                    'description': "<font face='helvetica' color='#000' size='2'><b>" + MapStopsData[i].Address + "</b></font><br><br><font color='888888' size='2'>" + MapStopsData[i].StopDescription + '</font>',
                    id: ""
                },
                geometry: { type: 'Point', coordinates: [MapStops[i]["lng"], MapStops[i]["lat"]] }
            });
            gsourceMapfeature.push({
                type: 'Feature',
                properties: {
                    name: 'stop' + i,//currently pop up is working for only one pin;Test Ex.Need to check how to give dynamic Name.
                    'description': "<font face='helvetica' color='#000' size='2'><b>" + MapStopsData[i].Address + "</b></font><br><br><font color='888888' size='2'>" + MapStopsData[i].StopDescription + '</font>',
                    id: ""
                },
                geometry: { type: 'Point', coordinates: [MapStops[i]["lng"], MapStops[i]["lat"]] }
            });
        } else {
            gMapfeatures.push({
                type: 'Feature',
                properties: {
                    name: 'stop' + i,
                    'description': "<font face='helvetica' color='#000' size='2'><b>" + MapStopsData[i].Address + "</b></font><br><br><font color='888888' size='2'>" + MapStopsData[i].StopDescription + '</font>',
                    id: i
                },
                geometry: {
                    type: 'Point', coordinates: [MapStops[i]["lng"], MapStops[i]["lat"]]
                }
            });
        }
    }
    const geoMapJsonDataF = {
        type: 'geojson',
        data: { type: 'FeatureCollection', features: gMapfeatures }
    };
    const geoMapJsonSourceData = {
        type: 'geojson',
        data: { type: 'FeatureCollection', features: gsourceMapfeature }
    };

    map.on('load', function () {
        //if (ngl.isObject(TrackStops) && TrackStops.length > 0) {
        //    myRouteTrack.addTo(map);
        //}

        myRoute.addTo(map);
        //myRouteTrack.addTo(map);
        //debugger;
        //return;
        //// Load image to use as the marker
        map.addSource('hqMapSource', geoMapJsonDataF);
        map.addSource('hqMapInitSource', geoMapJsonSourceData);

        //TRACK IT Route And Features Collection
        //Track It removed by RHR on 7/9/2021 does not work as expected more work is needed
        //if (ngl.isObject(TrackStops)) {
        //    if (TrackStops.length > 0) {
        //        const gTrackfeatures = [];
        //        const gsourceTrackfeature = [];

        //        const myRouteTrack = new TrimbleMaps.Route({
        //            routeId: "myRouteTrack",
        //            stops: TrackStops,
        //            VehicleType: VehType,
        //            RouteType: routType
        //        });
        //        for (var i = 0; i < TrackStops.length; i++) {
        //            if (i == 0) {
        //                gTrackfeatures.push({
        //                    type: 'Feature',
        //                    properties: {
        //                        name: 'stop' + i,//currently pop up is working for only one pin;Test Ex.Need to check how to give dynamic Name.
        //                        'description': "<font face='helvetica' color='#000' size='2'><b>" + TrackStopsData[i].Address + "</b></font><br><br><font color='888888' size='2'>" + TrackStopsData[i].StopDescription + '</font>',
        //                        id: ""
        //                    },
        //                    geometry: { type: 'Point', coordinates: [TrackStops[i]["lng"], TrackStops[i]["lat"]] }
        //                });
        //                gsourceTrackfeature.push({
        //                    type: 'Feature',
        //                    properties: {
        //                        name: 'stop' + i,//currently pop up is working for only one pin;Test Ex.Need to check how to give dynamic Name.
        //                        'description': "<font face='helvetica' color='#000' size='2'><b>" + TrackStopsData[i].Address + "</b></font><br><br><font color='888888' size='2'>" + TrackStopsData[i].StopDescription + '</font>',
        //                        id: ""
        //                    },
        //                    geometry: { type: 'Point', coordinates: [TrackStops[i]["lng"], TrackStops[i]["lat"]] }
        //                });
        //            } else {
        //                gTrackfeatures.push({
        //                    type: 'Feature',
        //                    properties: {
        //                        name: 'stop' + i,
        //                        'description': "<font face='helvetica' color='#000' size='2'><b>" + TrackStopsData[i].Address + "</b></font><br><br><font color='888888' size='2'>" + TrackStopsData[i].StopDescription + '</font>',
        //                        id: i
        //                    },
        //                    geometry: {
        //                        type: 'Point', coordinates: [TrackStops[i]["lng"], TrackStops[i]["lat"]]
        //                    }
        //                });
        //            }
        //        }

        //        const geoTrackJsonDataF = {
        //            type: 'geojson',
        //            data: { type: 'FeatureCollection', features: gTrackfeatures}
        //        };
        //        const geoTrackJsonSourceData = {
        //            type: 'geojson',
        //            data: {type: 'FeatureCollection', features: gsourceTrackfeature }
        //        };


        //        //myRouteTrack.addTo(map);
        //        map.addSource('hqTrackSource', geoTrackJsonDataF);
        //        map.addSource('hqTrackInitSource', geoTrackJsonSourceData);
        //    }
        //}

        map.loadImage('../Content/marker_blue.png', function (error, image) {

            // Add image to the map
            map.addImage('marker-icon', image);

            // Add layer to render marker based on datasource
            map.addLayer({
                id: 'hqMapPoints',
                source: 'hqMapSource',
                type: 'circle',
                paint: {
                    'circle-radius': 8,
                    'circle-color': '#FFF',
                    'circle-stroke-color': '#33E',
                    'circle-stroke-width': 5
                }
            });


            // Show count for clustered points
            map.addLayer({
                id: 'hqMapPointNum',
                type: 'symbol',
                source: 'hqMapSource',
                layout: {
                    'text-field': '{id}',
                    'text-font': ['Roboto Regular'],
                    'text-size': 11
                },
                paint: {
                    'text-color': '#000'
                }
            });
            // Add layer to render marker based on datasource
            map.addLayer({
                id: 'hqMapInitPoints',
                source: 'hqMapInitSource',
                type: 'circle',
                paint: {
                    'circle-radius': 8,
                    'circle-color': '#FFF',
                    'circle-stroke-color': '#209B00',
                    'circle-stroke-width': 5
                }
            });
            // Listen for clicks on the hqMapPoints layer
            map.on('click', 'hqMapPoints', function (evt) {
                const popupLocation = evt.features[0].geometry.coordinates.slice();
                const popupContent = evt.features[0].properties.description;

                new TrimbleMaps.Popup()
                    .setLngLat(popupLocation)
                    .setHTML(popupContent)
                    .addTo(map);
            });

            // Create a popup, but don't add it to the map yet.
            var popup = new TrimbleMaps.Popup({
                closeButton: false,
                closeOnClick: false
            });


            // Change cursor when hovering over a feature on the hqMapPoints layer
            map.on('mouseenter', 'hqMapPoints', function (e) {
                map.getCanvas().style.cursor = 'pointer';

                var coordinates = e.features[0].geometry.coordinates.slice();
                var description = e.features[0].properties.description;

                while (Math.abs(e.lngLat.lng - coordinates[0]) > 180) {
                    coordinates[0] += e.lngLat.lng > coordinates[0] ? 360 : -360;
                }

                popup.setLngLat(coordinates).setHTML(description).addTo(map);
            });

            // Change cursor back
            map.on('mouseleave', 'hqMapPoints', function () {
                map.getCanvas().style.cursor = '';
                popup.remove();
            });

            //TRACK IT STOP Numbers And Stop color
            if (ngl.isObject(TrackStops)) {
                if (TrackStops.length > 0) {
                    map.addLayer({
                        id: 'hqTrackPoints',
                        source: 'hqTrackSource',
                        type: 'circle',
                        paint: {
                            'circle-radius': 8,
                            'circle-color': '#FFF',
                            'circle-stroke-color': '#33E',
                            'circle-stroke-width': 5
                        }
                    });
                    // Show count for clustered points
                    map.addLayer({
                        id: 'hqTrackPointNum',
                        type: 'symbol',
                        source: 'hqTrackSource',
                        layout: {
                            'text-field': '{id}',
                            'text-font': ['Roboto Regular'],
                            'text-size': 11
                        },
                        paint: {
                            'text-color': '#000'
                        }
                    });
                    // Add layer to render marker based on datasource
                    map.addLayer({
                        id: 'hqTrackInitPoints',
                        source: 'hqTrackInitSource',
                        type: 'circle',
                        paint: {
                            'circle-radius': 8,
                            'circle-color': '#FFF',
                            'circle-stroke-color': '#209B00',
                            'circle-stroke-width': 5
                        }
                    });

                    map.on('click', 'hqTrackPoints', function (evt) {
                        const popupLocation = evt.features[0].geometry.coordinates.slice();
                        const popupContent = evt.features[0].properties.description;

                        new TrimbleMaps.Popup()
                            .setLngLat(popupLocation)
                            .setHTML(popupContent)
                            .addTo(map);
                    });

                    map.on('mouseenter', 'hqTrackPoints', function (e) {
                        map.getCanvas().style.cursor = 'pointer';

                        var coordinates = e.features[0].geometry.coordinates.slice();
                        var description = e.features[0].properties.description;

                        while (Math.abs(e.lngLat.lng - coordinates[0]) > 180) {
                            coordinates[0] += e.lngLat.lng > coordinates[0] ? 360 : -360;
                        }

                        popup.setLngLat(coordinates).setHTML(description).addTo(map);
                    });

                    map.on('mouseleave', 'hqTrackPoints', function () {
                        map.getCanvas().style.cursor = '';
                        popup.remove();
                    });
                }
            }
        });
    });
    //map.addControl(new TrimbleMaps.FullscreenControl());
    //for (var i = 0; i < a.length; i++) {
    //    var popup = new TrimbleMaps.Popup({ offset: popupOffsets })
    //        .setLngLat([a[i].lng, a[i].lat])
    //  .setHTML("<p><b>" + userDatalst[i].PinTitle + "</b><br/><br/>" + userDatalst[i].PinDescription + "</p>")
    //  .addTo(map);
    //}
}

var MapWaypointStopType = {
    Pickup: 0,
    Delivery: 1,
    PickAndDel: 2,
    Track: 3
}

function setMapStyle(mapStyle) {
    switch (stopCategory) {
        case MapWaypointStopType.Pickup:
            StopColor = 'blue';
            break;
        case MapWaypointStopType.Delivery:
            StopColor = 'blue';
            break;
        case MapWaypointStopType.PickAndDel:
            StopColor = 'blue';
            break;
        case MapWaypointStopType.Track:
            StopColor = 'green';
            break;
        default:
            break;
    }
    return StopColor;
}

function getStopColor(stopCategory, stopCompleted) {
    var StopColor = '';
    switch (stopCategory) {
        case MapWaypointStopType.Pickup:
            StopColor = 'blue';
            break;
        case MapWaypointStopType.Delivery:
            StopColor = 'blue';
            break;
        case MapWaypointStopType.PickAndDel:
            StopColor = 'blue';
            break;
        case MapWaypointStopType.Track:
            StopColor = 'green';
            break;
        default:
            break;
    }
    return StopColor;
}

function NGLMessage() {
    Message: "";
    ErrorDetails: "";
    ErrorMessage: "";
    ErrorReason: "";
}