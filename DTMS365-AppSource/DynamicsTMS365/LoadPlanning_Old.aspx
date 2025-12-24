<%@ Page Title="Load Planning Page" Language="C#"  AutoEventWireup="true" CodeBehind="LoadPlanning_Old.aspx.cs" Inherits="DynamicsTMS365.LoadPlanning_Old" %>

<!DOCTYPE html>

<html>
    <head >
        <title>DTMS Load Planning</title>           
        <link href="Content/kendoR32023/classic-opal.css" rel="stylesheet" />               
        <link href="Content/NGL/v-8.5.4.001/common.css" rel="stylesheet" />   
        <style>

 

html,

body

{

    height:100%;

    margin:0;

    padding:0;

}

 

html

{
    font-size: 12px; 
    font-family: Arial, Helvetica, sans-serif; 
    overflow:hidden;

}

 

</style>
    </head>
    <body>       
         <script src="https://code.jquery.com/jquery-3.4.1.min.js"></script>              
        <script src="Scripts/kendoR32023/kendo.all.min.js"></script>
<script>kendo.ui['Button'].fn.options['size'] = "small";</script>
        <script src="https://secure.aadcdn.microsoftonline-p.com/lib/1.0.14/js/adal.min.js"></script>
        <script src="Scripts/NGL/v-8.5.4.006/core.js"></script>
        <script src="Scripts/NGL/v-8.5.4.006/NGLobjects.js"></script>        
        <script src="Scripts/NGL/v-8.5.4.006/SSOA.js"></script>

      <div id="example" style="height: 100%; width: 100%;  margin-top: 2px;">
             <div id="vertical" style="height: 98%; width: 98%; " >                 
                <div id="menu-pane" style="height: 100%; width: 100%; background-color: white; ">
                    <div class="pane-content"> 
                        <span style="float:left; display:inline-block;">                       
                            <span style="margin:6px; vertical-align: middle;">
                                <a href="Default.aspx"><img border="0" alt="Home" src="Content/NGL/Home32.png" width="32" height="32"></a>
                            </span>
                            <span style="margin:6px; vertical-align: middle;" >
                                <a href="http://www.nextgeneration.com"><img border="0" alt="NGL" src="../Content/NGL/nextracklogo.GIF" ></a>
                            </span>
                        </span>
                        <span style="float:right; display:inline-block;">
                            <span style="margin:6px; vertical-align: middle;">
                                    <a href="Settings.aspx"><img border="0" alt="Settings" src="Content/NGL/Settings32.png" width="32" height="32"></a>
                                </span>
                                <span style="margin:6px; vertical-align: middle;" >
                                    <a href="http://nglwcfdev705.nextgeneration.com/usermanual"><img border="0" alt="Help" src="../Content/NGL/Help32.png" ></a>
                                </span>
                        </span>
                    </div>
                </div>
                <div id="top-pane">
                  <div id="horizontal" style="height: 100%; width: 100%; ">
                        <div id="left-pane">
                            <div class="pane-content">
                                <div><span>LTL Orders</span>&nbsp;&nbsp;&nbsp;&nbsp;<span>&nbsp;Zoom&nbsp;&nbsp;<button id="btnZoomInLTLOrders" onclick="zoomInLTLOrders();"><img border="0" alt="Zoon In" src="../Content/NGL/MoveUpArrowBlue16.png" width="16" height="16" ></button>&nbsp;&nbsp;<button id="btnZoomOutLTLOrders" onclick="zoomOutLTLOrders();"><img border="0" alt="Zoon Out" src="../Content/NGL/MoveDownArrowBlue16.png" width="16" height="16" ></button></span></div>
                                <div class="ltl k-content wide" >
                                    <div id="LTLlistView"></div>
                                </div>
                                <div id="pager" class="k-pager-wrap"></div>
                            </div>
                        </div>
                        <div id="center-pane">                            
                            <% Response.Write(PageErrorsOrWarnings); %>
                            <div class="pane-content">
                                <div><span>Selected Order</span>&nbsp;&nbsp;
                                    <span><button id="btnClearLTLOrder" onclick="btnClearLTLOrder_Click();"><img border="0" alt="Clear" src="../Content/NGL/Redo16.png" width="16" height="16" ></button>&nbsp;&nbsp;
                                        <button id="btnRunLTLOrders" onclick="btnRunLTLOrders_Click();"><img border="0" alt="Process LTL Orders" src="../Content/NGL/QueryRun16.png" width="16" height="16" ></button>&nbsp;&nbsp;
                                        <button id="btnAddLTLOrder" onclick="btnAddLTLOrder_Click();"><img border="0" alt="Add LTL Order to Load" src="../Content/NGL/AddBlue16.png" width="16" height="16" ></button>&nbsp;&nbsp;
                                        <button id="btnCalculateLoad" onclick="btnCalculateLoad_Click();"><img border="0" alt="Calculate Load" src="../Content/NGL/StackofCoinsSilver16.png" width="16" height="16" ></button>
                                        <button id="btnSaveLoad" onclick="btnSaveLoad_Click();"><img border="0" alt="Save Load" src="../Content/NGL/Save16.png" width="16" height="16" ></button>
                                    </span>
                                    &nbsp;
                                    <span>&nbsp;Zoom&nbsp;&nbsp;<button id="btnZoomInStops" onclick="zoomInStops();"><img border="0" alt="Zoon In" src="../Content/NGL/MoveUpArrowBlue16.png" width="16" height="16" ></button>&nbsp;&nbsp;<button id="btnZoomOutStops" onclick="zoomOutStops();"><img border="0" alt="Zoon Out" src="../Content/NGL/MoveDownArrowBlue16.png" width="16" height="16" ></button></span>
                                </div>                               
                                <div id="bookingData" class="stopInfo" >               
                                        <input id="txtShipKey" type="hidden" />
                                        <input id="txtBookProNumber" type="hidden" />
                                        <input id="txtBookConsPrefix" type="hidden" />
                                        <input id="txtBookControl" type="hidden" />
                                        <input id="txtLaneOriginAddressUse" type="hidden" /> 
                                        <input id="txtBookSHID" type="hidden" /> 
                                    <div class="stopInfo">
                                        <section id="bookOrig"  class="stopInfo">
                                            Order&nbsp;Date<br />
                                            <input id="txtBookDateOrdered" type="text" class="stopInfo" /><br />
                                            Origin<br />
                                            <input id="txtOrigName" type="text" class="stopInfo" /><br />                                        
                                            <input id="txtOrigStreet" type="text" class="stopInfo" /><br />
                                            <input id="txtOrigCity" type="text" class="stopInfo" /><br />
                                            <input id="txtOrigState" type="text" class="stopInfo"/><br />
                                            <input id="txtOrigZip" type="text" class="stopInfo"/><br />
                                            <input id="txtOrigCountry" type="text" class="stopInfo"/><br />
                                        </section>
                                        <section id="bookData" class="stopInfo">
                                            Load&nbsp;Date<br />
                                            <input id="txtBookDateLoad" type="text" class="stopInfo"/><br />
                                            Order Info<br />
                                            <input id="txtBookCarrOrderNumber" type="text"class="stopInfo" /><br /> 
                                            <input id="txtCarrierName" type="text" class="stopInfo"/><br />                                       
                                            <input id="txtBookTotalCases" type="text" class="stopInfo" /><br />
                                            <input id="txtBookTotalWgt" type="text" class="stopInfo" /><br />
                                            <input id="txtBookTotalPL" type="text" class="stopInfo"/><br />
                                            <input id="txtBookTotalCube" type="text" class="stopInfo"/><br />                                       
                                        </section>
                                        <aside id="bookDest" class="stopInfo">
                                            Required&nbsp;Date<br />
                                            <input id="txtBookDateRequired" type="text" class="stopInfo"/><br /> 
                                            Destination<br />
                                            <input id="txtDestName" type="text" class="stopInfo"/><br />                                        
                                            <input id="txtDestStreet" type="text" class="stopInfo" /><br />
                                            <input id="txtDestCity" type="text" class="stopInfo"/><br />
                                            <input id="txtDestState" type="text" class="stopInfo"/><br />
                                            <input id="txtDestZip" type="text" class="stopInfo"/><br />
                                            <input id="txtDestCountry" type="text" class="stopInfo"/><br />
                                        </aside>
                                    </div>
                                </div>
                                <div  style="clear: both;" class="stopInfo">
                                    Build Load
                                    <br />
                                    <div class="stop k-content wide" id="NewStopslistView" style="float: left; margin-right: 4px;"></div>
                                    <div class="stop k-content wide"  id="ExistingLoadlistView" style="float:left;"></div>
                                </div>
                            </div>
                        </div>
                        <div id="right-pane">
                            <div class="pane-content">
                                <div><span>Consolidated Orders</span>&nbsp;&nbsp;<span >&nbsp;Zoom&nbsp;&nbsp;<button id="btnZoomInOpenCNSGrid" onclick="zoomInOpenCNSGrid();"><img border="0" alt="Zoon In" src="../Content/NGL/MoveUpArrowBlue16.png" width="16" height="16" ></button>&nbsp;&nbsp;<button id="btnZoomOutOpenCNSGrid" onclick="zoomOutOpenCNSGrid();"><img border="0" alt="Zoon Out" src="../Content/NGL/MoveDownArrowBlue16.png" width="16" height="16" ></button></span> </div> 
                                <div class="OpenCNS">
                                    <div id="OpenCNSGrid"></div>
                                </div>
                                <div id="OpenCNSpager" class="k-pager-wrap"></div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="middle-pane" >
                    <div class="pane-content">
                        <div><span>Rates</span></div>                            
                        <div id="carriersGrid"></div>
                    </div>
                </div>
                <div id="bottom-pane" style="height: 100%; width: 100%; background-color: #daecf4; ">
                    <div class="pane-content">
                        <div><span><p>This secure site exists to provide On-Line Tendering, Shipment Accept/Reject, Shipment Status, Shipment Tracking, Shipment Settlement and Proof of Delivery (POD) information. If you experience problems with this site, call (847)963-0007 24/7 or email our support group at <a href="mailto:support.nextgeneration.com">support.nextgeneration.com</a></p></span></div>
                    </div>
                </div>
            </div>

    <script type="text/x-kendo-tmpl" id="template">
        <div class="product-view k-widget">
                <span>#:BookOrigName#&nbsp;#:BookOrigCity#&nbsp;#:BookOrigState#&nbsp;<b>#:BookOrigZip#</b></span>
                <br>
                <span>#:BookDestName#&nbsp;#:BookDestCity#&nbsp;#:BookDestState#&nbsp;<b>#:BookDestZip#</b></span>
            </div>
    </script>
    
    <script type="text/x-kendo-tmpl" id="altTemplate">
        <div class="product-view k-widget alt">
            <span>#:BookOrigName#&nbsp;#:BookOrigCity#&nbsp;#:BookOrigState#&nbsp;<b>#:BookOrigZip#</b></span>
            <br>
            <span>#:BookDestName#&nbsp;#:BookDestCity#&nbsp;#:BookDestState#&nbsp;<b>#:BookDestZip#</b></span>
        </div>
    </script>

    <script type="text/x-kendo-tmpl" id="StopslistTemplate">
        <div class="stop-view k-widget">
                <span>#:Address#</span>
                <br>
                <span>#:City#&nbsp;<b>#:Zip#</b></span>
                <br>
                <span>Route Sequence:#:SequenceNo#&nbsp;<b>#:PickupOrDelivery#</b></span>
            </div>
    </script>

   <script type="text/x-kendo-tmpl" id="altStopslistTemplate">
        <div class="stop-view k-widget alt">
                <span>#:Address#</span>
                <br>
                <span>#:City#&nbsp;<b>#:Zip#</b></span>
                <br>
                <span>Route Sequence:#:SequenceNo#&nbsp;<b>#:PickupOrDelivery#</b></span>
            </div>
    </script>

    <script type="text/x-kendo-template" id="openOrdertemplate">
        <div class="tabstrip">
            <ul>
                <li class="k-active">
                    Orders
                </li>
                <li>
                    Equipment Information
                </li>
            </ul>
            <div>
                <div class="orders"></div>
            </div>
            <div>
                <div class='employee-details'>
                    <ul>
                        <li><label>Max Cases:</label>#:MaxLaneTLCases#</li>
                        <li><label>Max Weight:</label>#:MaxLaneTLWGT#</li>
                        <li><label>Max Pallets:</label>#:MaxLaneTLPL#</li>
                        <li><label>Max Cubes:</label>#:MaxLaneTLCube#</li>
                    </ul>
                </div>
            </div>
        </div>

    </script>
            
    <script>
          //start ADAL properties
        var opostLogoutRedirectUri = '<% Response.Write(WebBaseURI); %>';
        var oredirectUri = '<% Response.Write(WebBaseURI); %>' + getCurentFileName(); 
        var oidaClient = '<% Response.Write(idaClientId); %>';  //
        var oAuth2instasnce = 'https://login.microsoftonline.com/';
        var oAuth2tenant = 'common';
        loadAuthContext();
        //NOTE:   validateUser(); must be called in the docuemnt.ready function
        //End ADAL properties

        var PageControl = '<%=PageControl%>'; 
        var control = 0;
        var tObj = this;
        var tPage = this;
        var oLoadStops = [];
        var oExistingLoadStops = [];
        var SelectedLoadShipKey = "";
        var StopsDataSource = kendo.data.DataSource;
        var ExistingStopsDataSource = kendo.data.DataSource;
        var ExistingLoadDataSource = kendo.data.DataSource;
        var LTLdataSource = kendo.data.DataSource;
        var OpenCNSdataSource = kendo.data.DataSource;
        var P44RateQuotedataSource = kendo.data.DataSource;
        var OpenCNSGridFontSize = 10;
        var LTLOrderFontSize = 10;
        var StopInfoFontSize = 10;
        

        function zoomInStops() {
            StopInfoFontSize = StopInfoFontSize + 1;
            $(".stopInfo").css('font-size', StopInfoFontSize);
            $("#NewStopslistView").css("fontSize", StopInfoFontSize);
            $("#ExistingLoadlistView").css("fontSize", StopInfoFontSize);
        }

        function zoomOutStops() {
            StopInfoFontSize = StopInfoFontSize - 1;
            $(".stopInfo").css('font-size', StopInfoFontSize);
            $("#NewStopslistView").css("fontSize", StopInfoFontSize);
            $("#ExistingLoadlistView").css("fontSize", StopInfoFontSize);
        }
        
        function zoomInLTLOrders() {
            LTLOrderFontSize = LTLOrderFontSize + 1;
            $("#LTLlistView").css("fontSize", LTLOrderFontSize);
        }

        function zoomOutLTLOrders() {
            LTLOrderFontSize = LTLOrderFontSize - 1;
            $("#LTLlistView").css("fontSize", LTLOrderFontSize);
        }

        function zoomInOpenCNSGrid() {
            OpenCNSGridFontSize = OpenCNSGridFontSize + 1;
            $("#OpenCNSGrid").css("fontSize", OpenCNSGridFontSize);
        }

        function zoomOutOpenCNSGrid() {
            OpenCNSGridFontSize = OpenCNSGridFontSize - 1;
            $("#OpenCNSGrid").css("fontSize", OpenCNSGridFontSize);
        }

        function btnClearLTLOrder_Click() {
            oLoadStops = [];
            StopsDataSource.data(oLoadStops);
            return;
        }

        function btnRunLTLOrders_Click() {
            return;
        }


        function LoadStop(shipkey) {
            this.BookControl = 0;
            this.ShipKey = shipkey;
            this.BookProNumber = "";
            this.BookConsPrefix = "";
            this.BookSHID = "";
            this.OrderNumber = "";
            this.LaneOriginAddressUse = false;
            this.Address = "";
            this.City = "";
            this.State = "";
            this.Zip = "";
            this.BookDateLoad = "";
            this.BookDateRequired = "";
            this.BookTotalCases = 0;
            this.BookTotalWgt = 0;
            this.BookTotalPL = 0;
            this.BookTotalCube = 0;
            this.SequenceNo = 1;
            this.StopNo = 0;
            this.PickNo = 0;
            this.isPickup = true;
            this.PickupOrDelivery = "";
        }


        function btnAddLTLOrder_Click() {
            var pickup = new LoadStop($("#txtShipKey").val());
            pickup.BookControl = $("#txtBookControl").val();
            pickup.LaneOriginAddressUse = $("#txtLaneOriginAddressUse").val();
            pickup.Address = $("#txtOrigStreet").val();
            pickup.City = $("#txtOrigCity").val();
            pickup.State = $("#txtOrigState").val();
            pickup.Zip = $("#txtOrigZip").val();
            pickup.BookDateLoad = $("#txtBookDateLoad").val();
            pickup.BookDateRequired = $("#txtBookDateRequired").val();
            pickup.OrderNumber = $("#txtOrderNumber").val();
            pickup.BookSHID = $("#txtBookSHID").val();
            pickup.BookConsPrefix = $("#txtBookTotalCases").val();
            pickup.BookProNumber = $("#txtBookProNumber").val();
            pickup.BookTotalCases = $("#txtBookTotalCases").val();
            pickup.BookTotalWgt = $("#txtBookTotalWgt").val();
            pickup.BookTotalPL = $("#txtBookTotalPL").val();
            pickup.BookTotalCube = $("#txtBookTotalCube").val();
            pickup.isPickup = true;
            pickup.PickupOrDelivery = "Pickup";
            oLoadStops.push(pickup);
            var delivery = new LoadStop($("#txtShipKey").val());
            delivery.BookControl = $("#txtBookControl").val();
            delivery.LaneOriginAddressUse = $("#txtLaneOriginAddressUse").val();
            delivery.Address = $("#txtDestStreet").val();
            delivery.City = $("#txtDestCity").val();
            delivery.State = $("#txtDestState").val();
            delivery.Zip = $("#txtDestZip").val();
            delivery.BookDateLoad = $("#txtBookDateLoad").val();
            delivery.BookDateRequired = $("#txtBookDateRequired").val();
            delivery.OrderNumber = $("#txtOrderNumber").val();
            delivery.BookSHID = $("#txtBookSHID").val();
            delivery.BookConsPrefix = $("#txtBookConsPrefix").val();
            delivery.BookProNumber = $("#txtBookProNumber").val();
            delivery.BookTotalCases = $("#txtBookTotalCases").val();
            delivery.BookTotalWgt = $("#txtBookTotalWgt").val();
            delivery.BookTotalPL = $("#txtBookTotalPL").val();
            delivery.BookTotalCube = $("#txtBookTotalCube").val();
            delivery.isPickup = false;
            delivery.PickupOrDelivery = "Delivery";
            oLoadStops.push(delivery);
            StopsDataSource.data(oLoadStops);
            return;
        }

        function btnCalculateLoad_Click() {
            var BookControl = $("#txtBookControl").val();
            P44RateQuotedataSource = new kendo.data.DataSource({
                transport: {
                    read: "api/P44RateQuote/" + BookControl,
                    parameterMap: function (options, operation) { return options; }
                },
                schema: {
                    data: "Data",
                    total: "Count",
                    model: {
                        id: "BookControl"                        
                    },
                    errors: "Errors"
                },
                error: function (e) {
                    alert(e.errors);
                    this.cancelChanges();
                },
                serverPaging: false,
                serverSorting: false,
                serverFiltering: false
            })
            $('#carriersGrid').empty();
            $("#carriersGrid").kendoGrid({
                theme: "[blueopal]",
                columns: [{
                        field: "Vendor",
                        title: "Provider",
                        template: "<div class='P44CarrierPhoto'" +
                                    "style='background-image: url(../Content/NGL/CarrierLogos/#:data.SCAC#logo.png);'></div>" +
                                "<div class='P44CarrierVendor'>#: Vendor  #</div>",
                        width: 75
                    },
                    { field: "Mode", title: "Mode", width: 75 },
                    { field: "TotalCost", title: "Total Cost", width: 100 },
                    { field: "TransitTime", title: "Transit Time", width: 100 },
                    { field: "InterLine", title: "InterLine", width: 100 },
                    { field: "errors", title: "errors", width: 200 },
                    { field: "Adjustments", title: "Adjustments", width: 500 }
                ],
                dataSource: P44RateQuotedataSource,
                sortable: true,
                resizable: true,
            });
            //$('#carriersGrid').data('kendoGrid').dataSource.id = BookControl;
            ////$('#carriersGrid').data('kendoGrid').dataSource = P44RateQuotedataSource;
            //$('#carriersGrid').data('kendoGrid').dataSource.read(BookControl);
            //$('#carriersGrid').data('kendoGrid').refresh();

            //P44RateQuotedataSource.read();

            //alert(JSON.stringify({ griditems: StopsDataSource.view() }));
            //alert(JSON.stringify(StopsDataSource.view()));

            return;
        }

        function btnSaveLoad_Click() {
            return;
        }
        
        function AddExistingOrder(data) {
            var pickup = new LoadStop(data.ShipKey);
            pickup.BookControl = data.BookControl;
            pickup.LaneOriginAddressUse = data.LaneOriginAddressUse;
            pickup.Address = data.BookOrigAddress1;
            pickup.City = data.BookOrigCity;
            pickup.State = data.BookOrigState;
            pickup.Zip = data.BookOrigZip;
            pickup.BookDateLoad = kendo.toString(kendo.parseDate(data.BookDateLoad), 'MM/dd/yyyy'), 'MM/dd/yyyy';
            pickup.BookDateRequired = kendo.toString(kendo.parseDate(data.BookDateRequired), 'MM/dd/yyyy'), 'MM/dd/yyyy';
            pickup.OrderNumber = data.OrderNumber;
            pickup.BookSHID = data.BookSHID;
            pickup.BookConsPrefix = data.BookConsPrefix;
            pickup.BookProNumber = data.BookProNumber;
            pickup.BookTotalCases = data.BookTotalCases;
            pickup.BookTotalWgt = data.BookTotalWgt;
            pickup.BookTotalPL = data.BookTotalPL;
            pickup.BookTotalCube = data.BookTotalCube;
            pickup.isPickup = true;
            pickup.PickupOrDelivery = "Pickup";
            oExistingLoadStops.push(pickup);
            var delivery = new LoadStop(data.shipkey);
            delivery.BookControl = data.BookControl;
            delivery.LaneOriginAddressUse = data.LaneOriginAddressUse;
            delivery.Address = data.BookDestAddress1;
            delivery.City = data.BookDestCity;
            delivery.State = data.BookDestState;
            delivery.Zip = data.BookDestZip;
            delivery.BookDateLoad = kendo.toString(kendo.parseDate(data.BookDateLoad), 'MM/dd/yyyy'), 'MM/dd/yyyy';
            delivery.BookDateRequired = kendo.toString(kendo.parseDate(data.BookDateRequired), 'MM/dd/yyyy'), 'MM/dd/yyyy';
            delivery.OrderNumber = data.OrderNumber;
            delivery.BookSHID = data.BookSHID;
            delivery.BookConsPrefix = data.BookConsPrefix;
            delivery.BookProNumber = data.BookProNumber;
            delivery.BookTotalCases = data.BookTotalCases;
            delivery.BookTotalWgt = data.BookTotalWgt;
            delivery.BookTotalPL = data.BookTotalPL;
            delivery.BookTotalCube = data.BookTotalCube;
            delivery.isPickup = false;
            delivery.PickupOrDelivery = "Delivery";
            oExistingLoadStops.push(delivery);
            //The caller must udpate the ExistingStopsDataSource to improve perfomance
            ExistingStopsDataSource.data(oExistingLoadStops);
            return;
        }


        function ltlSelected() {
            var data = LTLdataSource.view(),
                //selected = $.map(this.select(), function (item) {
                //    return data[$(item).index()].BookOrigName;
                //});
            selectedItem = $.map(this.select(), function (item) {
                return data[$(item).index()];
            });
            //var sBookOrigStreet = selectedItem[0].BookOrigAddress1;
            //$("#txtOrigName").text = selected;
            //kendo.toString(kendo.parseDate(LoadDate, 'yyyy-MM-dd'), 'MM/dd/yyyy')
            $("#txtBookDateOrdered").val(kendo.toString(kendo.parseDate(selectedItem[0].BookDateOrdered), 'MM/dd/yyyy'), 'MM/dd/yyyy');
            $("#txtBookDateLoad").val(kendo.toString(kendo.parseDate(selectedItem[0].BookDateLoad), 'MM/dd/yyyy'), 'MM/dd/yyyy');
            //$("#txtBookDateLoad").val(selectedItem[0].BookDateLoad);
            $("#txtBookDateRequired").val(kendo.toString(kendo.parseDate(selectedItem[0].BookDateRequired), 'MM/dd/yyyy'), 'MM/dd/yyyy');
            //$("#txtBookDateRequired").val(selectedItem[0].BookDateRequired);
            $("#txtShipKey").val(selectedItem[0].ShipKey);
            $("#txtBookProNumber").val(selectedItem[0].BookProNumber);
            $("#txtBookConsPrefix").val(selectedItem[0].BookConsPrefix);
            $("#txtBookControl").val(selectedItem[0].BookControl);
            $("#txtLaneOriginAddressUse").val(selectedItem[0].LaneOriginAddressUse);

            $("#txtBookSHID").val(selectedItem[0].BookSHID);
            
            $("#txtOrigName").val(selectedItem[0].BookOrigName);
            $("#txtOrigStreet").val(selectedItem[0].BookOrigAddress1);
            $("#txtOrigCity").val(selectedItem[0].BookOrigCity);
            $("#txtOrigState").val(selectedItem[0].BookOrigState);
            $("#txtOrigCountry").val(selectedItem[0].BookOrigCountry);
            $("#txtOrigZip").val(selectedItem[0].BookOrigZip);

            $("#txtBookCarrOrderNumber").val(selectedItem[0].BookCarrOrderNumber);
            $("#txtCarrierName").val(selectedItem[0].CarrierName);
            $("#txtBookTotalCases").val(selectedItem[0].BookTotalCases);
            $("#txtBookTotalWgt").val(selectedItem[0].BookTotalWgt);
            $("#txtBookTotalPL").val(selectedItem[0].BookTotalPL);
            $("#txtBookTotalCube").val(selectedItem[0].BookTotalCube);

            $("#txtDestName").val(selectedItem[0].BookDestName);
            $("#txtDestStreet").val(selectedItem[0].BookDestAddress1);
            $("#txtDestCity").val(selectedItem[0].BookDestCity);
            $("#txtDestState").val(selectedItem[0].BookDestState);
            $("#txtDestCountry").val(selectedItem[0].BookDestCountry);
            $("#txtDestZip").val(selectedItem[0].BookDestZip);
            //kendoConsole.log("Selected: " + selected.length + " item(s), [" + selected.join(", ") + "]");
        }

        //function openCNSLoadSelected() {
            
        //    var data = LTLdataSource.view(),             
        //        SelectedLoadShipKey = $.map(this.select(), function (item) {
        //            return data[$(item).index()].ShipKey;
        //        });
        //    fillExistingLoadStops();
           
        //}


        function fillExistingLoadStops() {

            oExistingLoadStops = [];
            ExistingStopsDataSource.data(oExistingLoadStops);
            if (SelectedLoadShipKey == null || SelectedLoadShipKey.length < 1) { return; }

            ExistingLoadDataSource = new kendo.data.DataSource({
                transport: {
                    read: "api/BookCNSDetails/" + SelectedLoadShipKey,
                    parameterMap: function (options, operation) { return options; }
                },
                schema: {
                    data: "Data",
                    total: "Count",
                    model: {
                        id: "ShipKey",
                        fields: {

                            Orig: { editable: false },
                            Street: { editable: false },
                            City: { editable: false, nullable: true },
                            State: { editable: false, nullable: true },
                            Dest: { editable: false, nullable: true },
                            DestStreet: { editable: false, nullable: true },
                            DestCity: { editable: false, nullable: true },
                            DestState: { editable: false, nullable: true }
                        }
                    },
                    errors: "Errors"
                },
                error: function (e) {
                    alert(e.errors);
                    this.cancelChanges();
                },
                serverPaging: false,
                serverSorting: false,
                serverFiltering: false,
                change: function () {
                    var data = this.data(); // or this.view();

                    for (var i = 0; i < data.length; i++) {

                        AddExistingOrder(data[i]);

                    }
                }
            })
            ExistingLoadDataSource.read();
            //ExistingStopsDataSource.data(oExistingLoadStops);
        }

        $(window).resize(function () {
            $("#vertical").data("kendoSplitter").trigger("resize")
        });
        $(document).ready(function () {
            control = <%=UserControl%>;            
            if (ngl.UserValidated(true,control,oredirectUri))
            {
                return;
            }
             <% Response.Write(PageCustomJS); %>
            $("#vertical").kendoSplitter({
                orientation: "vertical",
                panes: [
                    { collapsible: true, size: "40px" },
                    { collapsible: false },
                    { collapsible: true, size: "175px" },
                    { collapsible: false, resizable: false, size: "50px" }
                ]
            });

            $("#horizontal").kendoSplitter({
                panes: [
                    { collapsible: true },
                    { collapsible: false, size: "600px" },
                    { collapsible: true }
                ]
            });

            StopsDataSource = new kendo.data.DataSource({
                data: oLoadStops,
            });

            $("#NewStopslistView").kendoListView({
                theme: "[blueopal]",
                dataSource: StopsDataSource,
                selectable: "single",
                template: kendo.template($("#StopslistTemplate").html()),
                altTemplate: kendo.template($("#altStopslistTemplate").html())
            });

            ExistingStopsDataSource = new kendo.data.DataSource({
                data: oExistingLoadStops,
            });

            $("#ExistingLoadlistView").kendoListView({
                theme: "[blueopal]",
                dataSource: ExistingStopsDataSource,
                selectable: "single",
                template: kendo.template($("#StopslistTemplate").html()),
                altTemplate: kendo.template($("#altStopslistTemplate").html())
            });


            LTLdataSource = new kendo.data.DataSource({
                transport: {
                    read: "api/BookLTLs",
                    update: {
                        url: function (order) {
                            return "api/BookLTLs/" + order.BookControl
                        },
                        type: "POST"
                    },
                    destroy: {
                        url: function (order) {
                            return "api/BookLTLs/" + order.BookControl
                        },
                        type: "DELETE"
                    },
                    parameterMap: function (options, operation) {

                        // if the current operation is an update
                        //if (operation === "update") {
                        //    // create a new JavaScript date object based on the current
                        //    // CarrierModDate parameter value
                        //    var d = new Date(options.CarrierModDate);
                        //    // overwrite the CarrierModDate value with a formatted value that WebAPI
                        //    // will be able to convert
                        //    options.CarrierModDate = kendo.toString(new Date(options.CarrierModDate), "MM/dd/yyyy");
                        //}

                        // ALWAYS return options
                        return options;
                    }
                },
                schema: {
                    data: "Data",
                    total: "Count",
                    model: {
                        id: "BookControl",
                        fields: {
                            Control: { editable: false },
                            PRO: {
                                editable: true, nullable: false, validation: {
                                    required: {
                                        message: "Please enter a unique PRO Number to identify this order"
                                    }
                                }
                            },
                            CNS: { editable: true, nullable: false, validation: { required: true } },
                            Warehouse: { editable: true, nullable: true },
                            Destination: { editable: false }
                        }
                    },
                    errors: "Errors"
                },
                error: function (e) {
                    alert(e.errors);
                    this.cancelChanges();
                },
                pageSize: 20,
                serverPaging: true
            });

            OpenCNSdataSource = new kendo.data.DataSource({
                transport: {
                    read: "api/BookOpenCNS",
                    update: {
                        url: function (load) {
                            return "api/BookOpenCNS/" + load.SHID
                        },
                        type: "POST"
                    },
                    destroy: {
                        url: function (load) {
                            return "api/BookOpenCNS/" + load.SHID
                        },
                        type: "DELETE"
                    },
                    parameterMap: function (options, operation) {

                        // if the current operation is an update
                        //if (operation === "update") {
                        //    // create a new JavaScript date object based on the current
                        //    // CarrierModDate parameter value
                        //    var d = new Date(options.CarrierModDate);
                        //    // overwrite the CarrierModDate value with a formatted value that WebAPI
                        //    // will be able to convert
                        //    options.CarrierModDate = kendo.toString(new Date(options.CarrierModDate), "MM/dd/yyyy");
                        //}

                        // ALWAYS return options
                        return options;
                    }
                },
                schema: {
                    data: "Data",
                    total: "Count",
                    model: {
                        id: "ShipKey",
                        fields: {
                            SHID: { editable: false },
                            Carrier: { editable: false },
                            Cases: { editable: false, nullable: true, type: "number" },
                            Weight: { editable: false, nullable: true, type: "number" },
                            Pallets: { editable: false, nullable: true, type: "number" },
                            Cubes: { editable: false, nullable: true, type: "number" },
                            Loading: { editable: false, nullable: true, type: "date" },
                            Required: { editable: false, nullable: true, type: "date" }
                        }
                    },
                    errors: "Errors"
                },
                error: function (e) {
                    alert(e.errors);
                    this.cancelChanges();
                },
                pageSize: 20,
                serverPaging: true
            });

            P44RateQuotedataSource = new kendo.data.DataSource({
                transport: {
                    read: "api/P44RateQuote/" + 0,
                    parameterMap: function (options, operation) { return options; }
                },
                schema: {
                    data: "Data",
                    total: "Count",
                    model: {
                        id: "BookControl"
                    },
                    errors: "Errors"
                },
                error: function (e) {
                    alert(e.errors);
                    this.cancelChanges();
                },
                serverPaging: false,
                serverSorting: false,
                serverFiltering: false
            })


            $("#pager").kendoPager({
                theme: "[blueopal]",
                dataSource: LTLdataSource
            });

            $("#OpenCNSpager").kendoPager({
                theme: "[blueopal]",
                dataSource: OpenCNSdataSource
            });

            var LTLlistView = $("#LTLlistView").kendoListView({
                theme: "[blueopal]",
                dataSource: LTLdataSource,
                selectable: "single",
                change: ltlSelected,
                template: kendo.template($("#template").html()),
                altTemplate: kendo.template($("#altTemplate").html())
            }).data("kendoListView");


            var OpenCNSGrid = $("#OpenCNSGrid").kendoGrid({
                theme: "[blueopal]",
                columns: [
                    { field: "ShipKey", title: "SHID", width: 75 },
                    { field: "CarrierName", title: "Carrier", width: 150 },
                    { field: "TotalCases", title: "Cases", width: 75 },
                    { field: "TotalWgt", title: "Weight", width: 75 },
                    { field: "TotalPL", title: "Pallets", width: 75 },
                    { field: "TotalCube", title: "Cubes", width: 75 },
                    { field: "LoadDate", title: "Loading", width: 100, template: "#= kendo.toString(kendo.parseDate(LoadDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') #" },
                    { field: "RequiredDate", title: "Required", width: 100, template: "#= kendo.toString(kendo.parseDate(RequiredDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') #" }
                ],
                dataSource: OpenCNSdataSource,
                height: 310,
                sortable: true,
                resizable: true,
                groupable: true,
                detailTemplate: kendo.template($("#openOrdertemplate").html()),
                detailInit: OpenOrderdetailInit,
                selectable: "single, row",
                change: function (e) {

                    var selectedRows = this.select();
                    if (selectedRows != null && selectedRows.length > 0) {
                        var dataItem = this.dataItem(selectedRows[0]);

                        SelectedLoadShipKey = dataItem.ShipKey;
                        fillExistingLoadStops();
                    }                   

                }
            });




        });

        function OpenOrderdetailInit(e) {
            var detailRow = e.detailRow;

            detailRow.find(".tabstrip").kendoTabStrip({
                animation: {
                    open: { effects: "fadeIn" }
                }
            });

            detailRow.find(".orders").kendoGrid({
                theme: "[blueopal]",
                dataSource: {
                    transport: {
                        read: "api/BookCNSDetails/" + e.data.ShipKey,
                        parameterMap: function (options, operation) { return options; }
                    },
                    schema: {
                        data: "Data",
                        total: "Count",
                        model: {
                            id: "ShipKey",
                            fields: {
                                Orig: { editable: false },
                                Street: { editable: false },
                                City: { editable: false, nullable: true },
                                State: { editable: false, nullable: true },
                                Dest: { editable: false, nullable: true },
                                DestStreet: { editable: false, nullable: true },
                                DestCity: { editable: false, nullable: true },
                                DestState: { editable: false, nullable: true }
                            }
                        },
                        errors: "Errors"
                    },
                    error: function (e) {
                        alert(e.errors);
                        this.cancelChanges();
                    },
                    serverPaging: false,
                    serverSorting: false,
                    serverFiltering: false
                },
                resizeable: true,
                scrollable: true,
                sortable: false,
                pageable: false,
                columns: [
                    { field: "BookOrigName", title: "Orig", width: 100 },
                    { field: "BookOrigAddress1", title: "Street", width: 100 },
                    { field: "BookOrigCity", title: "City", width: 75 },
                    { field: "BookOrigState", title: "State", width: 75 },
                    { field: "BookDestName", title: "Dest", width: 100 },
                    { field: "BookDestAddress1", title: "Street", width: 100 },
                    { field: "BookDestCity", title: "City", width: 75 },
                    { field: "BookDestState", title: "State", width: 75 }
                ]
            });
        }


        //$(function () {


        //    $("#LTLlistView").kendoListView({
        //        template: kendo.template($("#template").html()),
        //        editTemplate: kendo.template($("#editTemplate").html()),
        //        datasource: LTLdataSource,
        //        sortable: true,
        //        pageable: true,
        //        groupable: true,
        //        editable: "inline",
        //    });

        //});

        $(function () {
            $("#carriersGrid").kendoGrid({
                theme: "[blueopal]",
                columns: [{
                        field: "Vendor",
                        title: "Provider",
                        template: "<div class='P44CarrierPhoto'" +
                                    "style='background-image: url(../Content/NGL/CarrierLogos/#:data.SCAC#logo.png);'></div>" +
                                "<div class='P44CarrierVendor'>#: Vendor  #</div>",
                        width: 75
                    },
                    { field: "Mode", title: "Mode", width: 75 },
                    { field: "TotalCost", title: "Total Cost", width: 100 },
                    { field: "TransitTime", title: "Transit Time", width: 100 },
                    { field: "InterLine", title: "InterLine", width: 100 },
                    { field: "errors", title: "errors", width: 200},
                    { field: "Adjustments", title: "Adjustments", width: 500 }
                ],
                dataSource: P44RateQuotedataSource,
                sortable: true,
                resizable: true,
            });

        });


        //$(function () {


        //});

    </script>
    <style>
        .P44CarrierPhoto {
        display: inline-block;
        width: 32px;
        height: 32px;
        background-size: 32px 32px;
        background-position: center center;
        vertical-align: middle;
        line-height: 32px;
        box-shadow: inset 0 0 1px #999, inset 0 0 10px rgba(0,0,0,.2);
        margin-left: 5px;
    }

    .P44CarrierVendor {
        display: inline-block;
        vertical-align: middle;
        line-height: 32px;
        padding-left: 3px;
    }
        
        .k-grid {
            font-size: 10px;
        }
        .k-grid td { 
            line-height: 1em;
        }
                #vertical {
                    height: auto;
                    margin: 0 auto;
                }
                #menu-pane { background-color: rgba(60, 70, 80, 0.15); }
                #middle-pane { background-color: rgba(60, 70, 80, 0.10); }
                #bottom-pane { background-color: rgba(60, 70, 80, 0.15); }
                #left-pane, #center-pane, #right-pane  { background-color: rgba(60, 70, 80, 0.05); }

                .pane-content {
                    padding: 0 10px;
                }
        .ltl
        {
            width: auto;
            height: 315px;
            overflow: auto;
            position: relative;            
            font-size: 10px;
        }

        .OpenCNS
        {
            width: auto;
            height: 315px;
            overflow: auto;
            position: relative;
        }
        .k-pager-wrap
        {
            border-top: 0;          
        }
        .product-view
        {
            float: left;
            width: 300px;
            height: auto;
            box-sizing: border-box;
            border-top: 0;
            position: relative;
        }

        .stopInfo{
            font-size: 10px;
        }

        .stop-view
        {
            float: left;
            width: 225px;
            height: auto;
            box-sizing: border-box;
            border-top: 0;
            position: relative;
        }
        .stop-view alt { background-color: #daecf4;}

        .stop
        {
            width: 225px;
            height: auto;
            overflow: auto;
            position: relative;            
            font-size: 10px;
        }
        .k-listview .k-state-selected { background-color: #13688c; }

        .product-view:nth-child(even) {
            border-left-width: 0;
        }
        .product-view dl
        {
            margin: 10px 10px 0;
            padding: 0;
            overflow: hidden;
        }
        .product-view dt, dd
        {
            margin: 0;
            padding: 0;
            width: 100%;
            line-height: 24px;
            font-size: 18px;
        }
        .product-view dt
        {
            font-size: 11px;
            height: 16px;
            line-height: 16px;
            text-transform: uppercase;
            opacity: 0.5;
        }
        
        .product-view dd
        {
            height: 46px;
            overflow: hidden;
            white-space: nowrap;
            text-overflow: ellipsis;

        }
        
        .product-view dd .k-widget,
        .product-view dd .k-textbox {
            font-size: 12px;
        }
        .k-listview
        {
            border-width: 1px 0 0;
            padding: 0;
            overflow: hidden;
        }
        .edit-buttons
        {
            position: absolute;
            bottom: 0;
            left: 0;
            right: 0;
            text-align: right;
            padding: 5px;
            background-color: rgba(0,0,0,0.1);
        }
        
        span.k-invalid-msg
        {
            position: absolute;
            margin-left: 6px;
        }
        
        .k-add-button {
            margin-bottom: 2em;
        }

        .alt { background-color: #daecf4;}
        
        @media only screen and (max-width : 620px) {
        
            .product-view
            {
                width: 100%;
            }
            .product-view:nth-child(even) {
                border-left-width: 1px;
            }
        }

         .k-detail-cell .k-tabstrip .k-content {
                    padding: 0.2em;
                }
                .employee-details ul
                {
                    list-style:none;
                    font-style:italic;
                    margin: 15px;
                    padding: 0;
                }
                .employee-details ul li
                {
                    margin: 0;
                    line-height: 1.7em;
                }

                .employee-details label
                {
                    display:inline-block;
                    width:90px;
                    padding-right: 10px;
                    text-align: right;
                    font-style:normal;
                    font-weight:bold;
                }

                #bookOrig {
	                width: 150px;
	                float: left;
	                padding: 2px 2px;
                }

                #bookData {
	                width: 150px; /* Account for margins + border values */
	                float: left;
	                padding: 2px 2px;
                }

                #bookDest {
	                width: 150px;
	                padding: 2px 2px;
	                float: left;
                }


            </style>
    </div>


    </body>

</html>
