<!DOCTYPE html>

<html>
    <head >
        <title>DTMS Rate Shopping</title>         
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

    overflow:hidden;

}

 

</style>
    </head>
    <body>       
        <script src="https://code.jquery.com/jquery-3.4.1.min.js"></script>  
        <%--<script src="Scripts/kendoR32023/2014.1.318/jquery.min.js"></script>--%>
        <%--<script src="Scripts/kendoR32023/2014.1.318/kendo.web.min.js"></script>  --%>
        <%--<script src="Scripts/kendoR32023/kendo.webcomponents.min.js"></script>--%>
        <%--<script src="http://cdn.kendostatic.com/2015.2.624/js/kendo.all.min.js"></script>--%>
        <script src="Scripts/kendoR32023/kendo.all.min.js"></script>
<script>kendo.ui['Button'].fn.options['size'] = "small";</script>

      <div id="example" style="height: 100%; width: 100%;  margin-top: 2px; overflow:scroll;">      
        <div id="menu-pane" style="height: 50px; width: 100%; background-color: white; ">
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
        <div id="order-pane">
            <div id="horizontal" style="height: auto; width: 100%; ">                        
                <div id="center-pane">
                    <div class="pane-content">
                        <div>
                            <span id="ExpandOrderSpan" style="display:none;">&nbsp;&nbsp;<img id="imgExpandOrder" onclick="expandOrder();"  border="0" alt="Expand" src="../Content/NGL/expand.png" width="12" height="12"  ></span>
                            <span id="CollapseOrderSpan" style="display:normal;">&nbsp;&nbsp;<img id="imgCollapseOrder" onclick="collapseOrder();" border="0" alt="Collapse" src="../Content/NGL/collapse.png" width="12" height="12" ></span>
                            <span>Selected Order</span>&nbsp;&nbsp;
                            <span><button id="btnCalculateLoad" onclick="btnCalculateLoad_Click();"><img border="0" alt="Calculate Load" src="../Content/NGL/StackofCoinsSilver16.png" width="16" height="16" ></button></span>                            
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
                                    <table class="stopInfo" style="width:100%">
                                        <tr>
                                        <th></th>
                                        <th>Order Date</th>
                                        <th>Order No.</th>
                                        <th>Provider</th> 
                                        <th>Cases</th>
                                        <th>Wgt.</th>
                                        <th>Pallets</th> 
                                        <th>Vol.</th>
                                        </tr>                                              
                                        <tr>
                                        <td  ><div style="width:100px;">Order Info</div></td>
                                        <td><input id="txtBookDateOrdered" type="text"class="stopInfo" /></td>
                                        <td><input id="txtBookCarrOrderNumber" type="text"class="stopInfo" /></td>
                                        <td><input id="txtCarrierName" type="text" class="stopInfo"/></td> 
                                        <td><input id="txtBookTotalCases" type="text" class="stopInfo" /></td>
                                        <td><input id="txtBookTotalWgt" type="text" class="stopInfo" /></td>
                                        <td><input id="txtBookTotalPL" type="text" class="stopInfo"/></td> 
                                        <td><input id="txtBookTotalCube" type="text" class="stopInfo"/></td>
                                        </tr>                                              
                                    </table>  
                                    <table class="stopInfo" style="width:100%">
                                        <tr>
                                        <th></th>
                                        <th>Load Date</th>
                                        <th>Location</th>
                                        <th>Street</th> 
                                        <th>City</th>
                                        <th>State</th>
                                        <th>Postal Code</th> 
                                        <th>Country</th>
                                        </tr>
                                        <tr>
                                        <td ><div style="width:100px;">Origin</div></td>
                                        <td><input id="txtBookDateLoad" type="text" class="stopInfo" /></td>
                                        <td><input id="txtOrigName" type="text" class="stopInfo" /></td>
                                        <td><input id="txtOrigStreet" type="text" class="stopInfo" /></td> 
                                        <td><input id="txtOrigCity" type="text" class="stopInfo" /></td>
                                        <td><input id="txtOrigState" type="text" class="stopInfo"/></td>
                                        <td><input id="txtOrigZip" type="text" class="stopInfo"/></td> 
                                        <td><input id="txtOrigCountry" type="text" class="stopInfo"/></td>
                                        </tr>
                                    </table> 
                                    <table class="stopInfo" style="width:100%">
                                        <tr>
                                        <th></th>
                                        <th>Required Date</th>
                                        <th>Location</th>
                                        <th>Street</th> 
                                        <th>City</th>
                                        <th>State</th>
                                        <th>Postal Code</th> 
                                        <th>Country</th>
                                        </tr>
                                        <tr>
                                        <td ><div style="width:100px;">Destination</div></td>
                                        <td><input id="txtBookDateRequired" type="text" class="stopInfo"/></td>
                                        <td><input id="txtDestName" type="text" class="stopInfo"/></td>
                                        <td><input id="txtDestStreet" type="text" class="stopInfo" /></td> 
                                        <td><input id="txtDestCity" type="text" class="stopInfo" /></td>
                                        <td><input id="txtDestState" type="text" class="stopInfo"/></td>
                                        <td><input id="txtDestZip" type="text" class="stopInfo"/></td> 
                                        <td><input id="txtDestCountry" type="text" class="stopInfo"/></td>
                                        </tr>
                                    </table> 
                                </section>
                            </div>
                        </div>
                    </div>
                </div>
                        
            </div>
        </div>
            <div id="orders-pane">
            <div class="pane-content" style="height: auto; width: 100%; ">
                <div>
                    <span id="ExpandOrdersSpan" style="display:none;">&nbsp;&nbsp;<img id="imgExpandOrders" onclick="expandOrders();"  border="0" alt="Expand" src="../Content/NGL/expand.png" width="12" height="12"  ></span>
                    <span id="CollapseOrdersSpan" style="display:normal;">&nbsp;&nbsp;<img id="imgCollapseOrders" onclick="collapseOrders();" border="0" alt="Collapse" src="../Content/NGL/collapse.png" width="12" height="12" ></span>
                    <span>Orders</span>
                </div> 
                <div id="OrdersDiv" class="OpenOrders">
                    <div id="OpenOrdersGrid"></div>
                </div>
            </div>
        </div>
        <div id="results-pane" >
            <div class="pane-content" style="height: auto; width: 100%; ">
                <div>
                    <span id="ExpandRatesSpan" style="display:none;">&nbsp;&nbsp;<img id="imgExpandRates" onclick="expandRates();"  border="0" alt="Expand" src="../Content/NGL/expand.png" width="12" height="12"  ></span>
                    <span id="CollapseRatesSpan" style="display:normal;">&nbsp;&nbsp;<img id="imgCollapseRates" onclick="collapseRates();" border="0" alt="Collapse" src="../Content/NGL/collapse.png" width="12" height="12" ></span>
                    <span>Rates</span>
                </div>                  
                <div id="RatesDiv" class="OpenOrders">             
                    <div id="carriersGrid"></div>
                </div>
            </div>
        </div>
   
    
    

   

    <script type="text/x-kendo-template" id="openOrdertemplate">
        <div class="tabstrip">
            <ul>
                <li class="k-active">
                    Item Details
                </li
                <li >
                    Equipment Information
                </li>
            </ul>
            <div>
                <div class="details"></div>
            </div>
            <div>
                <div class='order-details'>
                    <ul>
                        <li><label>Cases:</label>#:BookTotalCases#</li>
                        <li><label>Weight:</label>#:BookTotalWgt#</li>
                        <li><label>Pallets:</label>#:BookTotalPL#</li>
                        <li><label>Cubes:</label>#:BookTotalCube#</li>
                    </ul>
                </div>
            </div>
        </div>

    </script>

    <script type="text/x-kendo-template" id="rateQuoteDetailstemplate">
        <div class="tabstrip">
            <ul>
                <li class="k-active">
                    Adjustments
                </li>
                <li>
                    Errors
                </li>
            </ul>            
            <div>
                <div class="adjustments"></div>
            </div>
            <div>
                <div class="rateerrors"></div>
            </div>
        </div>

    </script>
            
    <script>
        var oLoadStops = [];
        var oExistingLoadStops = [];
        var SelectedLoadShipKey = "";
        var StopsDataSource = kendo.data.DataSource;
        var ExistingStopsDataSource = kendo.data.DataSource;
        var ExistingLoadDataSource = kendo.data.DataSource;
        var LTLdataSource = kendo.data.DataSource;
        var OpenOrdersdataSource = kendo.data.DataSource;
        var P44RateQuotedataSource = kendo.data.DataSource;
        var OpenOrdersGridFontSize = 10;
        var LTLOrderFontSize = 10;
        var StopInfoFontSize = 10;
        var SelectedBookControl = 0;

        function RateAdjustment(ID,Class,Wgt,Desc,Code,Amt,Rate) {
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

        function expandOrders() {
            $("#OrdersDiv").show();
            $("#ExpandOrdersSpan").hide();
            $("#CollapseOrdersSpan").show();
        }

        function collapseOrders() {
            $("#OrdersDiv").hide();
            $("#ExpandOrdersSpan").show();
            $("#CollapseOrdersSpan").hide();
        }
        function expandOrder() {
            $("#bookingData").show();
            $("#ExpandOrderSpan").hide();
            $("#CollapseOrderSpan").show();
        }

        function collapseOrder() {
            $("#bookingData").hide();
            $("#ExpandOrderSpan").show();
            $("#CollapseOrderSpan").hide();
        }

        function expandRates() {
            $("#RatesDiv").show();
            $("#ExpandRatesSpan").hide();
            $("#CollapseRatesSpan").show();
        }

        function collapseRates() {
            $("#RatesDiv").hide();
            $("#ExpandRatesSpan").show();
            $("#CollapseRatesSpan").hide();
        }
        
       

        function zoomInOpenOrdersGrid() {
            OpenOrdersGridFontSize = OpenOrdersGridFontSize + 1;
            $("#OpenOrdersGrid").css("fontSize", OpenOrdersGridFontSize);
        }

        function zoomOutOpenOrdersGrid() {
            OpenOrdersGridFontSize = OpenOrdersGridFontSize - 1;
            $("#OpenOrdersGrid").css("fontSize", OpenOrdersGridFontSize);
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
                columns: [
                    { field: "Mode", title: "Mode", width: 75 },
                    { field: "SCAC", title: "SCAC", width: 75 },
                    { field: "TotalCost", title: "Total Cost", width: 100 },
                    { field: "TransitTime", title: "Transit Time", width: 100 },
                    { field: "InterLine", title: "InterLine", width: 100 },
                    { field: "ErrorCount", title: "Errors", width: 50 },
                    { field: "AdjustmentCount", title: "Adjustments", width: 50  }
                ],
                dataSource: P44RateQuotedataSource, 
                detailTemplate: kendo.template($("#rateQuoteDetailstemplate").html()),
                detailInit: OpenRateQuoteDetailsInit,
                sortable: true,
                resizable: true,
                groupable: true,
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


        function orderSelected() {
            var data = OpenOrdersdataSource.view(),
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

        //function OpenOrdersLoadSelected() {
            
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

        
        $(document).ready(function () {
            
           
                                     
            OpenOrdersdataSource = new kendo.data.DataSource({
                transport: {
                    read: "api/BookLTLs",                    
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
                        id: "BookItemControl"                        
                    },
                    errors: "Errors"
                },
                error: function (e) {
                    alert(e.errors);
                    this.cancelChanges();
                },
                pageSize: 10,
                serverPaging: true,
                sortable: true,
                pageable: true,
                groupable: true,
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


            

            

            
            var OpenOrdersGrid = $("#OpenOrdersGrid").kendoGrid({
                columns: [
                    { field: "BookDateLoad", title: "Loading", width: 100, template: "#= kendo.toString(kendo.parseDate(BookDateLoad, 'yyyy-MM-dd'), 'MM/dd/yyyy') #" },
                    { field: "BookDateRequired", title: "Required", width: 100, template: "#= kendo.toString(kendo.parseDate(BookDateRequired, 'yyyy-MM-dd'), 'MM/dd/yyyy') #" },
                    { field: "BookCarrOrderNumber", title: "No.", width: 100 },
                    { field: "BookDestName", title: "Sell-to", width: 100 },
                    { field: "BookOrigName", title: "Location", width: 100 },
                    { field: "BookTotalCases", title: "Cases", width: 75 },
                    { field: "BookTotalWgt", title: "Weight", width: 75 },
                    { field: "BookTotalPL", title: "Pallets", width: 75 },
                    { field: "BookTotalCube", title: "Cubes", width: 75 }
                ],
                dataSource: OpenOrdersdataSource,
                height: 250,
                pageable: true,
                sortable: true,
                resizable: true,
                groupable: true,
                detailTemplate: kendo.template($("#openOrdertemplate").html()),
                detailInit: OpenOrderdetailInit,
                selectable: "single, row",
                change: orderSelected
            });

            

        });

        function OpenOrderdetailInit(e) {
            var detailRow = e.detailRow;

            detailRow.find(".tabstrip").kendoTabStrip({
                animation: {
                    open: { effects: "fadeIn" }
                }
            });

            detailRow.find(".details").kendoGrid({
                dataSource: {
                    transport: {
                        read: "api/BookItemDetails/" + e.data.BookControl,
                        parameterMap: function (options, operation) { return options; }
                    },
                    schema: {
                        data: "Data",
                        total: "Count",
                        model: {
                            id: "BookItemControl"
                            
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
                    { field: "BookLoadPONumber", title: "Cust PO", width: 100 },
                    { field: "BookItemItemNumber", title: "Item No.", width: 100 },
                    { field: "BookItemDescription", title: "Description", width: 75 },
                    { field: "BookItemQtyOrdered", title: "Qty.", width: 75 },
                    { field: "BookItemWeight", title: "Wgt.", width: 100 },
                    { field: "BookItemCube", title: "Vol.", width: 100 },
                    { field: "BookItemPallets", title: "Plts", width: 75 },
                    { field: "BookItemFAKClass", title: "FAK", width: 75 },
                    { field: "BookItemNMFCClass", title: "NMFC", width: 75 },
                    { field: "BookItemMarineCode", title: "Marine", width: 75 },
                    { field: "BookItemDOTCode", title: "DOT", width: 75 },
                    { field: "BookItemIATACode", title: "IATA", width: 75 },
                    { field: "BookItem49CFRCode", title: "49CFR", width: 75 }
                ]
            });
        }

        function OpenRateQuoteDetailsInit(e) {
            var eRateAdjustments = [];
            var eRateErrors = [];
            var eRateAdjustmentDataSource = kendo.data.DataSource;
            var eRateErrorsDataSource = kendo.data.DataSource;
            var detailRow = e.detailRow;

            detailRow.find(".tabstrip").kendoTabStrip({
                animation: {
                    open: { effects: "fadeIn" }
                }
            });
            eRateAdjustments.push(new RateAdjustment("One",e.data.AdjfreightClass1,e.data.Adjweight1,e.data.Adjdescription1,e.data.AdjdescriptionCode1,e.data.Adjamount1,e.data.Adjrate1));
            eRateAdjustments.push(new RateAdjustment("Two",e.data.AdjfreightClass2,e.data.Adjweight2,e.data.Adjdescription2,e.data.AdjdescriptionCode2,e.data.Adjamount2,e.data.Adjrate2));
            eRateAdjustments.push(new RateAdjustment("Three", e.data.AdjfreightClass3, e.data.Adjweight3, e.data.Adjdescription3, e.data.AdjdescriptionCode3, e.data.Adjamount3, e.data.Adjrate3));
            eRateAdjustments.push(new RateAdjustment("Four", e.data.AdjfreightClass4, e.data.Adjweight4, e.data.Adjdescription4, e.data.AdjdescriptionCode4, e.data.Adjamount4, e.data.Adjrate4));
            eRateAdjustments.push(new RateAdjustment("Five", e.data.AdjfreightClass5, e.data.Adjweight5, e.data.Adjdescription5, e.data.AdjdescriptionCode5, e.data.Adjamount5, e.data.Adjrate5));
            eRateAdjustmentDataSource = new kendo.data.DataSource({
                data: eRateAdjustments,
            });

            eRateErrors.push(new RateErrors("One", e.data.errorCode1, e.data.errorMessage1, e.data.eMessage1, e.data.errorfieldName1));
            eRateErrors.push(new RateErrors("Two", e.data.errorCode2, e.data.errorMessage2, e.data.eMessage2, e.data.errorfieldName2));
            eRateErrors.push(new RateErrors("Three", e.data.errorCode3, e.data.errorMessage3, e.data.eMessage3, e.data.errorfieldName3));
            eRateErrors.push(new RateErrors("Four", e.data.errorCode4, e.data.errorMessage4, e.data.eMessage4, e.data.errorfieldName4));
            eRateErrors.push(new RateErrors("Five", e.data.errorCode5, e.data.errorMessage5, e.data.eMessage5, e.data.errorfieldName5));
            eRateErrorsDataSource = new kendo.data.DataSource({
                data: eRateErrors,
            });


            detailRow.find(".adjustments").kendoGrid({
                dataSource: eRateAdjustmentDataSource,
                resizeable: true,
                scrollable: true,
                sortable: false,
                pageable: false,
                columns: [
                    { field: "index", title: " ", width: 35 },
                    { field: "freightClass", title: "Class", width: 50 },
                    { field: "weight", title: "Wgt.", width: 50 },
                    { field: "description", title: "Desc.", width: 200 },
                    { field: "descriptionCode", title: "Code", width: 50 },
                    { field: "amount", title: "Amt.", width: 50 },
                    { field: "rate", title: "Rate", width: 50 }
                ]
            });

            detailRow.find(".rateerrors").kendoGrid({
                dataSource: eRateErrorsDataSource,
                resizeable: true,
                scrollable: true,
                sortable: false,
                pageable: false,
                columns: [
                    { field: "index", title: " ", width: 35 },
                    { field: "errorCode", title: "Code", width: 50 },
                    { field: "errorMessage", title: "Msg", width: 100 },
                    { field: "eMessage", title: "Details", width: 100 },
                    { field: "errorfieldName", title: "Field Name", width: 75 }
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
                columns: [
                    { field: "Mode", title: "Mode", width: 75 },
                    { field: "SCAC", title: "SCAC", width: 75 },
                    { field: "TotalCost", title: "Total Cost", width: 100 },
                    { field: "TransitTime", title: "Transit Time", width: 100 },
                    { field: "InterLine", title: "InterLine", width: 100 },
                    { field: "ErrorCount", title: "Errors", width: 50 },
                    { field: "AdjustmentCount", title: "Adjustments", width: 50 }
                ],
                dataSource: P44RateQuotedataSource,
                detailTemplate: kendo.template($("#rateQuoteDetailstemplate").html()),
                detailInit: OpenRateQuoteDetailsInit,
                sortable: true,
                resizable: true,
                groupable: true,
            });

        });


        //$(function () {


        //});

    </script>
    <style>
        
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
                #order-pane { background-color: lightblue; }
                #orders-pane { background-color: white; }
                #results-pane { background-color: white; }
                #left-pane, #center-pane, #right-pane  { background-color: rgba(60, 70, 80, 0.05); }

                .pane-content {
                    padding: 0 10px;
                }
        .ltl
        {
            width: auto;
            height: 350px;
            overflow: auto;
            position: relative;            
            font-size: 10px;
        }

        .OpenOrders
        {
            width: 99%;
            height: auto;
            overflow: auto;
            position: relative;
        }
        .k-pager-wrap
        {
            border-top: 0;          
        }
        .product-view
        {            
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
            width: 225px;
            height: auto;
            box-sizing: border-box;
            border-top: 0;
            position: relative;
        }
        .stop-view alt { background-color: #EEE;}

        .stop
        {
            width: 225px;
            height: auto;
            overflow: auto;
            position: relative;            
            font-size: 10px;
        }
        .k-listview .k-state-selected { background-color: #6699ff; }

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

        .alt { background-color: #EEE;}
        
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
                .order-details ul
                {
                    list-style:none;
                    font-style:italic;
                    margin: 15px;
                    padding: 0;
                }
                .order-details ul li
                {
                    margin: 0;
                    line-height: 1.7em;
                }

                .order-details label
                {
                    display:inline-block;
                    width:90px;
                    padding-right: 10px;
                    text-align: right;
                    font-style:normal;
                    font-weight:bold;
                }

              

                


            </style>
    </div>


    </body>

</html>
