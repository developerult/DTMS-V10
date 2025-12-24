using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using DAL = Ngl.FreightMaster.Data;
using LTS = Ngl.FreightMaster.Data.LTS;
using DTO = Ngl.FreightMaster.Data.DataTransferObjects;
using BLL = NGL.FM.BLL;
using Integration = Ngl.FreightMaster.Integration;

namespace DynamicsTMS365.Controllers
{
    public class BookController : NGLControllerBase
    {

        #region " Properties"
        /// <summary>
        /// This property is used for logging and error tracking.
        /// </summary>
        private string _SourceClass = "DynamicsTMS365.Controllers.BookController";
        public override string SourceClass
        {
            get { return _SourceClass; }
            set { _SourceClass = value; }
        }

        HttpRequest request = HttpContext.Current.Request;

        #endregion


        #region " REST Services"

        


        [HttpGet, ActionName("GetChargesSummaryForSHID")]
        public Models.Response GetChargesSummaryForSHID(string filter)
        {
            var response = new Models.Response(); //new HttpResponseMessage();
            if (!authenticateController(ref response)) { return response; }
            string strHoverOver = "";
            try
            {
                DAL.NGLBookData oBook = new DAL.NGLBookData(Parameters);
                LTS.spGetChargesSummaryForSHIDResult res = oBook.GetChargesSummaryForSHID(filter);
                if (res != null)
                {                
                    string lineHaul = string.Format("{0:c}", res.BookRevCarrierCostSUM);
                    string totalCost = string.Format("{0:c}", res.BookRevTotalCostSUM);

                    var diff = res.BookRevTotalCostSUM - res.BookRevCarrierCostSUM;
                    string totalFees = string.Format("{0:c}", diff);

                    strHoverOver = string.Format("<div><strong>Charges for SHID: {0} </strong><p>Line Haul: {1} </p><p>Total Accessorial: {2}</p><p>Total Cost: {3} </p></div>", filter, lineHaul, totalFees, totalCost);
                    //strHoverOver = string.Format("<strong>Charges for SHID: {0} </strong><table class='tbl'><tr><th class='tbl-topRt'>Line Haul:</th><th class='tbl-topRt'>{1}</th></tr><tr><td class='tbl-topRt'>Total Accessorials:</td><td class='tbl-topRt'>{2}</td></tr><tr><td class='tbl-topRt'>Total Cost:</td><td class='tbl-topRt'>{3}</td></tr></table>", filter, lineHaul, totalFees, totalCost);
                }
                Array d = new string[1] { strHoverOver };
                response = new Models.Response(d, 1);
            }
            catch (Exception ex)
            {
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors = fault.formatMessage();
                return response;
            }
            return response;
        }

        private void populateResponseError(ref Models.Response response, string errMsg) {
            response.StatusCode = HttpStatusCode.InternalServerError;
            response.Errors = errMsg;
        }


        [HttpPost, ActionName("CreateBookingOrder")]
        public Models.Response CreateBookingOrder([FromBody]DAL.Models.RateRequestOrder order)
        {
            var response = new Models.Response();
            if (!authenticateController(ref response)) { return response; }
            if (order == null) { response.populateDefaultInvalidFilterResponseMessage(); return response; }
            try
            {
                if (order.Pickup == null || order.Stops == null) { populateResponseError(ref response, "Origin and Destination are required"); return response; }
                if (order.Stops[0] == null) { populateResponseError(ref response, "Destination is required"); return response; }
                //**RULE: Inbound = True --> Destination. Inbound = False --> Origin

                //VALIDATE AND SELECT THE COMPANY BASED ON INBOUND/OUTBOUND STATUS, ADDRESS INFO, AND THE CURRENT USER'S ASSOCIATED LEGAL ENTITY
                LTS.Comp comp;
                if (order.Inbound) {
                    comp = NGLCompData.GetCompFilteredByAddressAndUserLE(order.Stops[0].CompAddress1, order.Stops[0].CompCity, order.Stops[0].CompState, order.Stops[0].CompPostalCode);
                    if (comp == null) { populateResponseError(ref response, "Destination address must match an existing company"); return response; }
                }
                else {
                    comp = NGLCompData.GetCompFilteredByAddressAndUserLE(order.Pickup.CompName, order.Pickup.CompAddress1, order.Pickup.CompCity, order.Pickup.CompState, order.Pickup.CompPostalCode);
                    if (comp == null) { populateResponseError(ref response, "Origin address must match an existing company"); return response; }
                }

                //USE THE ADDRESS INFORMATION AND INBOUND/OUTBOUND STATUS TO SEE IF A LANE EXISTS
                //Create the parameter objects
                Integration.clsAddressInfo origin = new Integration.clsAddressInfo(){ CompNumber = "0", AddrName = order.Pickup.CompName.Trim(), Addr1 = order.Pickup.CompAddress1.Trim(), City = order.Pickup.CompCity.Trim(), State = order.Pickup.CompState.Trim(), Zip = order.Pickup.CompPostalCode.Trim(), Country = order.Pickup.CompCountry.Trim() };
                Integration.clsAddressInfo destination = new Integration.clsAddressInfo(){ CompNumber = "0", AddrName = order.Stops[0].CompName.Trim(), Addr1 = order.Stops[0].CompAddress1.Trim(), City = order.Stops[0].CompCity.Trim(), State = order.Stops[0].CompState.Trim(), Zip = order.Stops[0].CompPostalCode.Trim(), Country = order.Stops[0].CompCountry.Trim() };
                if (order.Inbound) { destination.CompNumber = comp.CompNumber.Value.ToString(); } else { origin.CompNumber = comp.CompNumber.Value.ToString(); }
                //create the class
                string smtpFromAddress = System.Configuration.ConfigurationManager.AppSettings["SmtpFromAddress"];
                string fromEmail = !string.IsNullOrWhiteSpace(smtpFromAddress) ? smtpFromAddress : "DoNotReply@nextgeneration.com";
                bool blnDebug = System.Diagnostics.Debugger.IsAttached ? true : false;
                Integration.clsLane oLaneIntegration = new Integration.clsLane(Parameters.UserEmail, fromEmail, Parameters.UserEmail, 0,"", Parameters.DBServer, Parameters.Database, Parameters.WCFAuthCode, blnDebug, Parameters.ConnectionString);
                //call the method
                string laneNumber = oLaneIntegration.doesLaneExist(origin, destination, 0, 0, order.Inbound);
                //if lane does not exist we have to create a new one                             
                if (string.IsNullOrWhiteSpace(laneNumber))
                {
                    //clean any weird characters
                    destination.AddrName = destination.AddrName.Replace("'", "’");
                    origin.AddrName = origin.AddrName.Replace("'", "’");
                    destination.AddrName = destination.AddrName.Replace("\"", "");
                    origin.AddrName = origin.AddrName.Replace("\"", "");
                    //Outbound = CompAbrev - WHNAme - Zip                
                    //Inbound = WHNAme - Zip - CompAbrev
                    // Code Block in 30-10-2025 by Ayman
                    //if (order.Inbound) { laneNumber = destination.AddrName + "-" + destination.Zip + "-" + comp.CompAbrev; } else { laneNumber = comp.CompAbrev + "-" + origin.AddrName + "-" + origin.Zip; }
                    
                    if (order.Inbound) { laneNumber = comp.CompAbrev + "_" + origin.AddrName + "_"+  destination.AddrName + "_" + destination.Zip; } else { laneNumber = comp.CompAbrev + "_" + origin.AddrName + "_" + destination.AddrName + "_" + destination.Zip; }
                }


                //POPULATE THE PARAMETER DATA OBJECTS
                List<Integration.clsBookHeaderObject80> oOrders = new List<Integration.clsBookHeaderObject80>();
                //int frtRate = (order.Inbound == true ? 5 : 4); //POFrt 4 if outbound or 5 if inbound
                int frtRate = order.BookTransType;
                Integration.clsBookHeaderObject80 hdr = new Integration.clsBookHeaderObject80()
                {
                    POCompLegalEntity = comp.CompLegalEntity,
                    POCompAlphaCode = comp.CompAlphaCode, //: (S)String 50 characters.One of the PODefaultCustomer or the POCompAlphaCode is required.The POCompAlphaCode is prefered.If the POCompAlphaCode is provided the PODefaultCustomer should be zero.
                    PODefaultCustomer = "0", //(O)String 50 characters.If provided this must match a Company Number assigned by the TMS system.
                    PONumber = order.Stops[0].BookCarrOrderNumber,
                    POOrderSequence = 0,
                    POVendor = laneNumber, //(R)String 50 characters.The value must equal a valid LaneNumber value.This field is used as part of the unique primary key for the table.
                    POdate = DateTime.Now.ToShortDateString(), //(S)String 25 characters.May be empty but highly recommended that it is included.Order Date maps to BookDateOrdered.
                    POShipdate = order.Pickup.LoadDate, //(R)String 25.Date the order is expected to ship.Maps to BookDateLoad.
                    POBuyer = "", //(O)String 10 characters.Name of buyer is availble.
                    //POFrt = (short)order.BookTransType,
                    POFrt = (short)frtRate, //4 if outbound or 5 if inbound
                    POWgt = order.Pickup.TotalWgt, //(R)Double.Total weight for all items on the order; maps to BookTotalWgt.
                    POCube = order.Pickup.TotalCube,//(R)Integer.Total cubes for all items on the order; maps to BookTotalCube.
                    POQty = order.Pickup.TotalCases,// (R)Integer.Total quantity for all items on the order; maps to BookTotalCases.
                    POPallets = (int)order.Pickup.TotalPL,// (R)Integer.Total Pallets for all items on the order; must be a positive number; maps to BookTotalPL.
                    POReqDate = order.Pickup.RequiredDate,// (S)String 25 characters.This is the Requested and/ or the Required delivery date.  For new orders both the Requested and the Required date are mapped to the POReqDate; for modified orders the Requested date does not change and the Required only changes when it is provided.Blank POReqDate on modified orders are ignored.Maps to BookDateRequired and BookDateRequested.
                    POTemp = order.CommCodeType,// (S)String 1 character.Identifies the temperature of the order.Note: Due to backward compatibility, POTemp field requires an alpha code which will map to the same numeric code provide in the LaneTempType.
                                //o   F = 1 for Frozen
                                //o   R = 2 for Refrigerated 35 - 38
                                //o   D = 3 for Dry
                                //o   M = 4 for Mixed Frozen and Dry
                                //o   H = 5 for Hazmat
                                //o   G = 6 for General
                                //o   U = 7 for Unknown
                                //o   C = 8 Cooler
                    POCustomerPO = "", //(S)String 20 characters.Used if the Customer PO is different from the POnumber (primary key).  If blank the system will automatically assign the POnumber value to this field.Maps to the BookLoadPONumber value.
                    POStatusFlag = 5,
                    POOrigLegalEntity = (order.Inbound == true ? "" : comp.CompLegalEntity),// (O/R) String 50 characters.If a POOrigCompAlphaCode is provided the POOrigLegalEntity is required.The value is included to ensure correct mapping to the the shipping location.This value is only used when the POStatusFlag is 4, 5, or 6.
                    POOrigCompNumber = "0",// (O) String 50 characters.This value is used to support creation of new lanes or alternate address assignment for an order.Follows lane integration rules when creation of a new lane is required.If POOrigCompAlphaCode is provided this value should be set to zero.  If POInbound is False this value will be used to create the LaneCompNumber. This value is only used when the POStatusFlag is 4, 5, or 6.
                    POOrigCompAlphaCode = (order.Inbound == true ? "" : comp.CompAlphaCode),// (O) String 50 characters. This value is used to support creation of new lanes or alternate address assignment for an order.Follows lane integration rules when creation of a new lane is required.If the POOrigCompNumber is provided this value should empty.If POInbound is False this value will be used to create the LaneCompAlphaCode.  Set the LaneOrigCompNumber field to zero when providing a LaneOrigCompAlphaCode. This value is only used when the POStatusFlag is 4, 5, or 6.
                    POOrigName = order.Pickup.CompName,
                    POOrigAddress1 = order.Pickup.CompAddress1,
                    POOrigAddress2 = order.Pickup.CompAddress2,
                    POOrigAddress3 = order.Pickup.CompAddress3,
                    POOrigCity = order.Pickup.CompCity,
                    POOrigState = order.Pickup.CompState,
                    POOrigCountry = order.Pickup.CompCountry,
                    POOrigZip = order.Pickup.CompPostalCode,
                    //•	POOrigContactPhone: (O) String 15 characters.This value is used to support creation of new lanes or alternate address assignment for an order.Follows lane integration rules when creation of a new lane is required.This value is only used when the POStatusFlag is 4, 5, or 6.
                    //•	POOrigContactPhoneExt: (O) String 50 characters.This value is used to support creation of new lanes or alternate address assignment for an order.Follows lane integration rules when creation of a new lane is required.This value is only used when the POStatusFlag is 4, 5, or 6.
                    //•	POOrigContactFax: (O) String 15 characters.This value is used to support creation of new lanes or alternate address assignment for an order.Follows lane integration rules when creation of a new lane is required.This value is only used when the POStatusFlag is 4, 5, or 6.
                    PODestLegalEntity = (order.Inbound == true ? comp.CompLegalEntity : ""),// (O/R) String 50 characters.If a PODestCompAlphaCode is provided the PODestLegalEntity is required.The value is included to ensure correct mapping to the the receiving location.This value is only used when the POStatusFlag is 4, 5, or 6.
                    PODestCompNumber = "0",//(O) String 50 characters.This value is used to support creation of new lanes or alternate address assignment for an order.Follows lane integration rules when creation of a new lane is required.If PODestCompAlphaCode is provided this value should be set to zero.  If POInbound is True this value will be used to create the LaneCompNumber.  This value is only used when the POStatusFlag is 4, 5, or 6.
                    PODestCompAlphaCode = (order.Inbound == true ? comp.CompAlphaCode : ""),// (O) String 50 characters. This value is used to support creation of new lanes or alternate address assignment for an order.Follows lane integration rules when creation of a new lane is required.If the PODestCompNumber is provided this value should empty.If POInbound is True this value will be used to create the LaneCompAlphaCode.  Set the LaneDestCompNumber field to zero when providing a LaneDestCompAlphaCode. This value is only used when the POStatusFlag is 4, 5, or 6.
                    PODestName = order.Stops[0].CompName,
                    PODestAddress1 = order.Stops[0].CompAddress1,
                    PODestAddress2 = order.Stops[0].CompAddress2,
                    PODestAddress3 = order.Stops[0].CompAddress3,
                    PODestCity = order.Stops[0].CompCity,
                    PODestState = order.Stops[0].CompState,
                    PODestCountry = order.Stops[0].CompCountry,
                    PODestZip = order.Stops[0].CompPostalCode,
                    //•	PODestContactPhone: (O) String 15 characters.This value is used to support creation of new lanes or alternate address assignment for an order.Follows lane integration rules when creation of a new lane is required.This value is only used when the POStatusFlag is 4, 5, or 6.
                    //•	PODestContactPhoneExt: (O) String 50 characters.This value is used to support creation of new lanes or alternate address assignment for an order.Follows lane integration rules when creation of a new lane is required.This value is only used when the POStatusFlag is 4, 5, or 6.
                    //•	PODestContactFax: (O) String 15 characters.This value is used to support creation of new lanes or alternate address assignment for an order.Follows lane integration rules when creation of a new lane is required.This value is only used when the POStatusFlag is 4, 5, or 6.
                    POInbound = order.Inbound, //(O) Boolean True of False.This value is used to support creation of new lanes or alternate address assignment for an order.Follows lane integration rules when creation of a new lane is required.Maps to the LaneOriginAddressUse field.This value is only used when the POStatusFlag is 4, 5, or 6.
                    //•	PODefaultRouteSequence: (O) Integer.When the POStatusFlag is 5, or 6 and the creation of a new lane is required the system follows lane integration rules and maps to the LaneDefaultRouteSequence field.If a new lane is not required and a PORouteGuideNumber is provided this value takes precedence over the default lane settings.
                    //•	PORouteGuideNumber: (O) String 50 characters. When the POStatusFlag is 5, or 6 and the creation of a new lane is required the system follows lane integration rules and maps to the LaneDefaultRouteSequence field.If a new lane is not required  this value takes precedence over the default lane settings. 
                    //•	POModeTypeControl: (O) Integer.  This value is not currently used but is scheduled to be completed in v-8.2. When complete this value will allow setting the LaneModeTypeControl on new lanes and it will allow overrides to the lane settings on new orders where a previous lane already exists.The default value is 3 or use one of the following:
                    //o	  1 – Air
                    //o   2 – Rail
                    //o   3 – Road
                    //o   4 – Sea
                    //o   5 – Service 
                };
                oOrders.Add(hdr);

                List<Integration.clsBookDetailObject80> oDetails = new List<Integration.clsBookDetailObject80>();
                foreach (var item in order.Stops[0].Items)
                {
                    int qty = 0;
                    int.TryParse(item.Quantity, out qty);
                    Integration.clsBookDetailObject80 det = new Integration.clsBookDetailObject80()
                    {
                        POItemCompLegalEntity = comp.CompLegalEntity,//: (R)String 50 characters.Represents an alpha numeric code unique to a legal entity.  Shipping locations are assigned to a legal entity.
                        POItemCompAlphaCode = comp.CompAlphaCode, //(S)String 50 characters.Must match the POCompAlphaCode value provided in the parent parent header record.
                        CustomerNumber = "0", //(O)String 50 characters.Must match the PODefaultCustomer value provided in the parent header record.
                        ItemPONumber = order.Stops[0].BookCarrOrderNumber, //(R)String 20 characters long.Must match the PONumber value in the provided parent header record.
                        POOrderSequence = 0,
                        ItemNumber = item.ItemNumber,    //(R)String 50 characters long.Unique item reference number.
                        QtyOrdered = qty,    //(S)Integer.Quantity of items ordered.
                        Weight = (double)item.Weight, //(S)Double.Total weight of this item.
                        //Cube = 0, //(S)Integer.Total Cubes of this item.                              
                        //Pack: (0) 8 bit integer.                             
                        //Size: (0) String 255 characters.User configurable text associated with this item.
                        Description = item.Description, //(S)String 255 characters.
                        //Hazmat = "", //(0) String 1 character.Use “h” for hazmat and blank for non - hazmat.
                        //Brand: (0) String 255 characters.
                        //CustItemNumber = "", //(0) String 50 characters.Customer specific item number.
                        PalletType = item.PackageType, //(S)String 1 character.User configureable via the pallet type, some default values are provided.Some examples are:
                                         //N for Normal Grade Pallet 48x40
                                         //C for Chep Pallet
                                         //S Slip Sheets
                        //POItemHazmatTypeCode: (0) String 20 characters.
                        //POItem49CFRCode: (0) String 20 characters.
                        //POItemIATACode: (0) String 20 characters.
                        //POItemDOTCode: (0) String 20 characters.
                        //POItemMarineCode: (0) String 20 characters.
                        POItemNMFCClass = item.NMFCItem, //(0) String 20 characters.
                        POItemFAKClass = item.FreightClass, //(0) String 20 characters.
                        POItemPallets = item.PalletCount, //(S)Double.Decimal value representing the number of pallets needed, based on the pallet type.
                        //POItemQtyPalletPercentage = "",//(0) Double.
                        POItemQtyLength = (double)item.Length, //(0) Double.
                        POItemQtyWidth = (double)item.Width, //(0) Double.
                        POItemQtyHeight = (double)item.Height, //(0) Double.
                        POItemStackable = item.Stackable, //(0) Boolean(True / False).
                        POItemNMFCSubClass = item.NMFCSub, //(0) String 20 characters.represents a sub class for NMFC class codes, optional typically blank.
                        POItemWeightUnitOfMeasure = item.WeightUnit, //(0) String 100 characters long. Optional unit of measure reference defined for future use.
                        //POItemCubeUnitOfMeasure = "", //(0) String 100 characters long. Optional unit of measure reference defined for future use
                        //POItemDimensionUnitOfMeasure = "", //(0) String 100 characters long. Optional unit of measure dimension reference defined for future use.
                    };
                    oDetails.Add(det);
                }

                //CALL THE METHOD
                Integration.clsBook oBookIntegration = new Integration.clsBook();                
                var eProcessDataReturnValues = oBookIntegration.ProcessObjectData(oOrders, oDetails, ConnectionString);
                if (eProcessDataReturnValues == Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete)
                {
                    Array d = new bool[1] { true };
                    response = new Models.Response(d, 1);
                } else {
                    populateResponseError(ref response, "There was a problem creating the order"); }
            }
            catch (Exception ex)
            {
                FaultExceptionEventArgs fault = Utilities.ManageExceptions(ref ex);
                response.StatusCode = fault.StatusCode;
                response.Errors = fault.formatMessage();
                return response;
            }
            return response;
        }


        
        #endregion
    }
}