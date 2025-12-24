<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CarrierScheduler.aspx.cs" Inherits="DynamicsTMS365.CarrierScheduler" %>

<!DOCTYPE html>

<html>
<head>
    <title>DTMS Carrier Scheduler</title>
    <%=cssReference%>
    <style>
        html,
        body {height: 100%;margin: 0;padding: 0;}
        html {font-size: 12px;font-family: Arial, Helvetica, sans-serif;overflow: hidden;}       
    </style>
</head>
<body>
    <%=jssplitter2Scripts%> 
    <%=sWaitMessage%> 

    <script src="Scripts/NGL/v-8.5.4.006/windowconfig.js"></script>
    <!--added by SRP on 3/8/2018 For Editing KendoWindow Configuration from Javascript -->

    <div id="example" style="height: 100%; width: 100%;  margin-top: 2px;">
        <div id="vertical" style="height: 98%; width: 98%; ">
            <div id="menu-pane" style="height: 100%; width: 100%; background-color: white;">
                <div id="tab" class="menuBarTab"></div>
            </div>
            <div id="top-pane">
                <div id="horizontal" style="height: 100%; width: 100%; ">
                    <div id="left-pane">
                        <div class="pane-content">
                            <% Response.Write(MenuControl); %>
                            <div id="menuTree"></div>
                        </div>
                    </div>
                    <div id="center-pane">
                        <% Response.Write(PageErrorsOrWarnings); %>
                        <!-- Begin Page Content -->
                        <div style="margin: 10px; margin-left: 0;"><b>Carriers - Appointment Scheduling</b></div>
                        <div id="id260" class="ui-id260-container">
                            <div class="fast-tab">
                                <span id="ExpandParSpan" style="display: none;"><a onclick="expandFastTab('ExpandParSpan','CollapseParSpan','ParHeader','ParDetail');"><span style="font-size: small; font-weight: bold;" class="k-icon k-i-chevron-down"></span></a></span>
                                <span id="CollapseParSpan" style="display: normal;"><a onclick="collapseFastTab('ExpandParSpan','CollapseParSpan','ParHeader',null);"><span style="font-size: small; font-weight: bold;" class="k-icon k-i-chevron-up"></span></a></span>
                                <span style="font-size: small; font-weight: bold;">Pending Orders Summary</span>
                            </div>
                            <div id="ParHeader" style="margin-top: 15px;">
                                <fieldset>
                                    <legend><b>Pending Orders Summary</b></legend>
                                    <div style="margin-top: 10px;">
                                        <input type="radio" id="barchart" value="column" name="charttype" class="k-radio" checked="checked" /><label class="k-radio-label" for="barchart"><b>Bar chart</b></label>
                                       <%-- <input type="radio" id="pichart" value="pie" name="charttype" class=" k-radio" /><label class="k-radio-label" for="pichart"><b>Pie chart</b></label>--%>
                                        <input type="radio" id="linechart" value="line" name="charttype" class=" k-radio" style="margin-left: 10px;" /><label class="k-radio-label" for="linechart"><b>Line chart</b></label>
                                    </div>
                                    <div style="float: left; width: 30%; height: 40%"><div id="daySummaryChart"></div></div>
                                    <div style="float: left; width: 70%; height: 40%"><div id="weekSummaryChart"></div></div>
                                </fieldset>
                            </div>
                        </div>
                        <div id="Appointmentschedule" style="margin-top: 16px;">
                            <div id="tabstrip">
                                <ul>
                                    <li class="k-active"><b>Unscheduled Orders</b></li>
                                    <li><b>Booked Appointments</b></li>
                                </ul>
                                <div>
                                    <div>                                                                                      
                                        <%--Unscheduled Orders - Pickup--%>         
                                        <div class="fast-tab">                       
                                            <span id="ExpandOrdPickSpan" style="display: none;"><a onclick='expandOrdPick();'><span style="font-size: small;font-weight:bold;" class='k-icon k-i-chevron-down'></span></a></span>                        
                                            <span id="CollapseOrdPickSpan" style="display: normal;"><a onclick='collapseOrdPick();'><span style="font-size: small;font-weight:bold;" class='k-icon k-i-chevron-up'></span></a></span>                        
                                            <span style="font-size:small; font-weight:bold">Unscheduled Orders - Pick Up</span>&nbsp;&nbsp;<br />                        
                                            <div id="FastTabDivOrdPick" style="padding: 10px; width:calc(100% - 20px); height:100%;">                                         
                                                <div class="fast-tab">                                                                   
                                                    <span id="ExpandOrdPickFltrSpan" style="display: none;"><a onclick='expandOrdPickFltr();'><span style="font-size: small;font-weight:bold;" class='k-icon k-i-chevron-down'></span></a></span>                                                                    
                                                    <span id="CollapseOrdPickFltrSpan" style="display: normal;"><a onclick='collapseOrdPickFltr();'><span style="font-size: small;font-weight:bold;" class='k-icon k-i-chevron-up'></span></a></span>                                                                   
                                                    <span style="font-size:small; font-weight:bold">Filters</span>&nbsp;&nbsp;<br />                                                                   
                                                    <div id="FastTabDivOrdPickFltr" style="padding: 10px; width: calc(100% - 20px); height: 100%;">
                                                        <div style="margin-left: 5px;">
                                                            <span>
                                                                <label for="ddlOrdFilters">&nbsp;Filter by:</label><input id="ddlOrdFilters" />
                                                                <span id="spOrdFilterText"><input id="txtOrdFilterVal" /></span>
                                                                <span id="spOrdFilterDate"><input id="txtOrdFilterDateVal" /></span>
                                                                <span id="spOrdFilterButtons"><a id="btnOrdFilter"></a><a id="btnOrdClearFilter"></a></span>
                                                                <input id="txtOrdSortField" type="hidden" /> <input id="txtOrdSortDirection" type="hidden" />
                                                            </span>
                                                        </div>
                                                    </div>                                                          
                                                </div>                                   
                                                <div id="scheduleApptPickupGrid"></div>                                                               
                                            </div>                   
                                        </div>

                                        <p></p>
                                                                                                                                                                            
                                        <%--Unscheduled Orders - Delivery--%>                     
                                        <div class="fast-tab">
                                            <span id="ExpandOrdDelSpan" style="display: none;"><a onclick='expandOrdDel();'><span style="font-size: small; font-weight: bold;" class='k-icon k-i-chevron-down'></span></a></span>
                                            <span id="CollapseOrdDelSpan" style="display: normal;"><a onclick='collapseOrdDel();'><span style="font-size: small; font-weight: bold;" class='k-icon k-i-chevron-up'></span></a></span>
                                            <span style="font-size: small; font-weight: bold">Unscheduled Orders - Delivery</span>&nbsp;&nbsp;<br />
                                            <div id="FastTabDivOrdDel" style="padding: 10px; width: calc(100% - 20px); height: 100%;">
                                                <div class="fast-tab">
                                                    <span id="ExpandOrdDelFltrSpan" style="display: none;"><a onclick='expandOrdDelFltr();'><span style="font-size: small; font-weight: bold;" class='k-icon k-i-chevron-down'></span></a></span>
                                                    <span id="CollapseOrdDelFltrSpan" style="display: normal;"><a onclick='collapseOrdDelFltr();'><span style="font-size: small; font-weight: bold;" class='k-icon k-i-chevron-up'></span></a></span>
                                                    <span style="font-size: small; font-weight: bold">Filters</span>&nbsp;&nbsp;<br />
                                                    <div id="FastTabDivOrdDelFltr" style="padding: 10px; width: calc(100% - 20px); height: 100%;">
                                                        <div style="margin-left: 5px;">
                                                            <span>
                                                                <label for="ddlOrdDeliveryFilters">&nbsp;Filter by:</label><input id="ddlOrdDeliveryFilters" />
                                                                <span id="spOrdDeliveryFilterText"><input id="txtOrdDeliveryFilterVal" /></span>
                                                                <span id="spOrdDeliveryFilterDate"><input id="txtOrdDeliveryFilterDateVal" /></span>
                                                                <span id="spOrdDeliveryFilterButtons"><a id="btnOrdDeliveryFilter"></a><a id="btnOrdDeliveryClearFilter"></a></span>
                                                                <input id="txtOrdDeliverySortField" type="hidden" /> <input id="txtOrdDeliverySortDirection" type="hidden" />
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div id="scheduleapptdeliveryGrid"></div>
                                            </div>
                                        </div>                                                                                                             
                                    </div>
                                </div>
                                <div>
                                    <%--Booked Appointments - Pick Up--%>                                                 
                                    <div class="fast-tab">
                                        <span id="ExpandBookedPickSpan" style="display: none;"><a onclick='expandBookedPick();'><span style="font-size: small; font-weight: bold;" class='k-icon k-i-chevron-down'></span></a></span>
                                        <span id="CollapseBookedPickSpan" style="display: normal;"><a onclick='collapseBookedPick();'><span style="font-size: small; font-weight: bold;" class='k-icon k-i-chevron-up'></span></a></span>
                                        <span style="font-size: small; font-weight: bold">Booked Appointments - Pick Up</span>&nbsp;&nbsp;<br />
                                        <div id="FastTabDivBookedPick" style="padding: 10px; width: calc(100% - 20px); height: 100%;">
                                            <div class="fast-tab">
                                                <span id="ExpandBookedPickFltrSpan" style="display: none;"><a onclick='expandBookedPickFltr();'><span style="font-size: small; font-weight: bold;" class='k-icon k-i-chevron-down'></span></a></span>
                                                <span id="CollapseBookedPickFltrSpan" style="display: normal;"><a onclick='collapseBookedPickFltr();'><span style="font-size: small; font-weight: bold;" class='k-icon k-i-chevron-up'></span></a></span>
                                                <span style="font-size: small; font-weight: bold">Filters</span>&nbsp;&nbsp;<br />
                                                <div id="FastTabDivBookedPickFltr" style="padding: 10px; width: calc(100% - 20px); height: 100%;">
                                                    <div style="margin-left: 5px;">
                                                        <span>
                                                            <label for="ddlBookedOrdFilters">&nbsp;Filter by:</label><input id="ddlBookedOrdFilters" />
                                                            <span id="spBookedOrdFilterText"><input id="txtBookedOrdFilterVal" /></span>
                                                            <span id="spBookedOrdFilterDate"><input id="txtBookedOrdFilterDateVal" /></span>
                                                            <span id="spBookedOrdFilterButtons"><a id="btnBookedOrdFilter"></a><a id="btnBookedOrdClearFilter"></a></span>
                                                            <input id="txtBookedOrdSortField" type="hidden" /> <input id="txtBookedOrdSortDirection" type="hidden" />
                                                        </span>
                                                    </div>
                                                </div>
                                            </div>
                                            <div id="bookedAppointmnetpickupGrid"></div>
                                        </div>
                                    </div>
                                 
                                    <p></p>

                                    <%--Booked Appointments - Delivery--%>                     
                                    <div class="fast-tab">
                                        <span id="ExpandBookedDelSpan" style="display: none;"><a onclick='expandBookedDel();'><span style="font-size: small; font-weight: bold;" class='k-icon k-i-chevron-down'></span></a></span>
                                        <span id="CollapseBookedDelSpan" style="display: normal;"><a onclick='collapseBookedDel();'><span style="font-size: small; font-weight: bold;" class='k-icon k-i-chevron-up'></span></a></span>
                                        <span style="font-size: small; font-weight: bold">Booked Appointments - Delivery</span>&nbsp;&nbsp;<br />
                                        <div id="FastTabDivBookedDel" style="padding: 10px; width: calc(100% - 20px); height: 100%;">
                                            <div class="fast-tab">
                                                <span id="ExpandBookedDelFltrSpan" style="display: none;"><a onclick='expandBookedDelFltr();'><span style="font-size: small; font-weight: bold;" class='k-icon k-i-chevron-down'></span></a></span>
                                                <span id="CollapseBookedDelFltrSpan" style="display: normal;"><a onclick='collapseBookedDelFltr();'><span style="font-size: small; font-weight: bold;" class='k-icon k-i-chevron-up'></span></a></span>
                                                <span style="font-size: small; font-weight: bold">Filters</span>&nbsp;&nbsp;<br />
                                                <div id="FastTabDivBookedDelFltr" style="padding: 10px; width: calc(100% - 20px); height: 100%;">
                                                    <div style="margin-left: 5px;">
                                                        <span>
                                                            <label for="ddlBookedOrdDeliveryFilters">&nbsp;Filter by:</label><input id="ddlBookedOrdDeliveryFilters" />
                                                            <span id="spBookedOrdDeliveryFilterText"><input id="txtBookedOrdDeliveryFilterVal" /></span>
                                                            <span id="spBookedOrdDeliveryFilterDate"><input id="txtBookedOrdDeliveryFilterDateVal" /></span>
                                                            <span id="spBookedOrdDeliveryFilterButtons"><a id="btnBookedOrdDeliveryFilter"></a><a id="btnBookedOrdDeliveryClearFilter"></a></span>
                                                            <input id="txtBookedOrdDeliverySortField" type="hidden" /> <input id="txtBookedOrdDeliverySortDirection" type="hidden" />
                                                        </span>
                                                    </div>
                                                </div>
                                            </div>
                                            <div id="bookedAppointmnetdeliveryGrid"></div>
                                        </div>
                                    </div>                                   
                                </div>
                            </div> <%--end div tabstrip--%>
                        </div>
                        <!-- End Page Content -->
                    </div>
                </div>
            </div>
            <div id="bottom-pane" class="k-block" style="height: 100%; width: 100%;">
                <div class="pane-content">
                    <% Response.Write(PageFooterHTML); %>                    
                </div>
            </div>
        </div>

        <% Response.WriteFile("~/Views/CarrierSchedulingApptSelect.html"); %>

        <% Response.Write(PageTemplates); %>

        <%-- HelpWindow needs to go after AuthNotification because it uses the notification object created there --%>
        <% Response.Write(AuthLoginNotificationHTML); %>  
        <% Response.WriteFile("~/Views/HelpWindow.html"); %>  
        <script>
            <% Response.Write(ADALPropertiesjs); %>

            var PageControl = '<%=PageControl%>';      
            var tObj = this;
            var tPage = this;  

        <% Response.Write(NGLOAuth2); %>



            <% Response.Write(PageCustomJS); %>

            //************* Page Variables **************************
            var OrderSummaryDays = '<%=ConfigurationManager.AppSettings["OrderSummaryDays"] %>';
            var fromEmail = '<%=ConfigurationManager.AppSettings["SmtpFromAddress"] %>';

            var dsScheduleApptPickUpOrdersData = kendo.data.DataSource;
            var dsBookedAppoinmentPickupData = kendo.data.DataSource;
            var dsBookedAppoinmentDeliveryData = kendo.data.DataSource;
            var dsAvailableApptData = kendo.data.DataSource;
            var dsScheduleApptDeliveryOrdersData = kendo.data.DataSource;

            //************* Fast Tab Functions **************************
            function expandOrdPick() { $("#FastTabDivOrdPick").show(); $("#ExpandOrdPickSpan").hide(); $("#CollapseOrdPickSpan").show(); }
            function collapseOrdPick() { $("#FastTabDivOrdPick").hide(); $("#ExpandOrdPickSpan").show(); $("#CollapseOrdPickSpan").hide(); }
            function expandOrdPickFltr() { $("#FastTabDivOrdPickFltr").show(); $("#ExpandOrdPickFltrSpan").hide(); $("#CollapseOrdPickFltrSpan").show(); }
            function collapseOrdPickFltr() { $("#FastTabDivOrdPickFltr").hide(); $("#ExpandOrdPickFltrSpan").show(); $("#CollapseOrdPickFltrSpan").hide(); }

            function expandOrdDel() { $("#FastTabDivOrdDel").show(); $("#ExpandOrdDelSpan").hide(); $("#CollapseOrdDelSpan").show(); }
            function collapseOrdDel() { $("#FastTabDivOrdDel").hide(); $("#ExpandOrdDelSpan").show(); $("#CollapseOrdDelSpan").hide(); }
            function expandOrdDelFltr() { $("#FastTabDivOrdDelFltr").show(); $("#ExpandOrdDelFltrSpan").hide(); $("#CollapseOrdDelFltrSpan").show(); }
            function collapseOrdDelFltr() { $("#FastTabDivOrdDelFltr").hide(); $("#ExpandOrdDelFltrSpan").show(); $("#CollapseOrdDelFltrSpan").hide(); }

            function expandBookedPick() { $("#FastTabDivBookedPick").show(); $("#ExpandBookedPickSpan").hide(); $("#CollapseBookedPickSpan").show(); }
            function collapseBookedPick() { $("#FastTabDivBookedPick").hide(); $("#ExpandBookedPickSpan").show(); $("#CollapseBookedPickSpan").hide(); }
            function expandBookedPickFltr() { $("#FastTabDivBookedPickFltr").show(); $("#ExpandBookedPickFltrSpan").hide(); $("#CollapseBookedPickFltrSpan").show(); }
            function collapseBookedPickFltr() { $("#FastTabDivBookedPickFltr").hide(); $("#ExpandBookedPickFltrSpan").show(); $("#CollapseBookedPickFltrSpan").hide(); }

            function expandBookedDel() { $("#FastTabDivBookedDel").show(); $("#ExpandBookedDelSpan").hide(); $("#CollapseBookedDelSpan").show(); }
            function collapseBookedDel() { $("#FastTabDivBookedDel").hide(); $("#ExpandBookedDelSpan").show(); $("#CollapseBookedDelSpan").hide(); }
            function expandBookedDelFltr() { $("#FastTabDivBookedDelFltr").show(); $("#ExpandBookedDelFltrSpan").hide(); $("#CollapseBookedDelFltrSpan").show(); }
            function collapseBookedDelFltr() { $("#FastTabDivBookedDelFltr").hide(); $("#ExpandBookedDelFltrSpan").show(); $("#CollapseBookedDelFltrSpan").hide(); }


            $(document).ready(function () {
                var PageMenuTab = <%=PageMenuTab%>;
               
                if (control != 0){
                    setTimeout(function () {
                        //add code here to load screen specific information this is only visible when a user is authenticated

                        //ALERTS
                        setTimeout(getAlerts, 60000);
                    
                        $("#tabstrip").kendoTabStrip({
                            height: 'auto',
                            animation: {
                                open: { effects: "fadeIn" }
                            }
                        });

                        //***********Filters ddl Object**************//
                        var PickGridFilterData = [
                                { value: "None", text: "" },
                                { value: "BookSHID", text: "SHID" },
                                { value: "BookConsPrefix", text: "CNS Number" },
                                { value: "BookCarrOrderNumber", text: "Order Number" },
                                { value: "BookProNumber", text: "Pro Number" },
                                { value: "Warehouse", text: "Warehouse" },
                                { value: "BookDateLoad", text: "Load Date" },
                                { value: "ScheduledDate", text: "Scheduled Date" }
                        ];
                        var DelGridFilterData = [
                                { value: "None", text: "" },
                                { value: "BookSHID", text: "SHID" },
                                { value: "BookConsPrefix", text: "CNS Number" },
                                { value: "BookCarrOrderNumber", text: "Order Number" },
                                { value: "BookProNumber", text: "Pro Number" },
                                { value: "Warehouse", text: "Warehouse" },
                                { value: "BookDateRequired", text: "Required Date" },
                                { value: "ScheduledDate", text: "Scheduled Date" }
                        ];

                        /////*************Appointment pickup Filters***************//
                        $("#txtOrdFilterVal").kendoMaskedTextBox();
                        $("#txtOrdFilterDateVal").kendoDatePicker();
                        $("#ddlOrdFilters").kendoDropDownList({
                            dataTextField: "text",
                            dataValueField: "value",
                            placeholder: "select",
                            dataSource: PickGridFilterData,
                            select: function (e) {
                                var name = e.dataItem.text;
                                var val = e.dataItem.value;
                                $("#txtOrdFilterVal").data("kendoMaskedTextBox").value("");
                                $("#txtOrdFilterDateVal").data("kendoDatePicker").value("");
                                switch (val) {
                                    case "None":
                                        $("#spOrdFilterText").hide();
                                        $("#spOrdFilterDate").hide();
                                        $("#spOrdFilterButtons").hide();
                                        break;
                                    case "BookDateLoad":
                                        $("#spOrdFilterText").hide();
                                        $("#spOrdFilterDate").show();
                                        $("#spOrdFilterButtons").show();
                                        break;
                                    case "ScheduledDate":
                                        $("#spOrdFilterText").hide();
                                        $("#spOrdFilterDate").show();
                                        $("#spOrdFilterButtons").show();
                                        break;
                                    default:
                                        $("#spOrdFilterText").show();
                                        $("#spOrdFilterDate").hide();
                                        $("#spOrdFilterButtons").show();
                                        break;
                                }
                            }
                        });
                        $("#btnOrdFilter").kendoButton({
                            icon: "filter",
                            click: function (e) {
                                $("#scheduleApptPickupGrid").data("kendoGrid").dataSource.read();
                            }
                        });
                        $("#btnOrdClearFilter").kendoButton({
                            icon: "filter-clear",
                            click: function (e) {
                                var dropdownlist = $("#ddlOrdFilters").data("kendoDropDownList");
                                dropdownlist.select(0);
                                dropdownlist.trigger("change");
                                $("#txtOrdFilterVal").data("kendoMaskedTextBox").value("");
                                $("#txtOrdFilterDateVal").data("kendoDatePicker").value("");
                                $("#spOrdFilterText").hide();
                                $("#spOrdFilterDate").hide();
                                $("#spOrdFilterButtons").hide();
                                $("#scheduleApptPickupGrid").data("kendoGrid").dataSource.read();
                            }
                        });
                        $("#spOrdFilterText").hide();
                        $("#spOrdFilterDate").hide();
                        $("#spOrdFilterButtons").hide();

                        /////*************Appointment Delivery Filters***************//
                        $("#txtOrdDeliveryFilterVal").kendoMaskedTextBox();
                        $("#txtOrdDeliveryFilterDateVal").kendoDatePicker();
                        $("#ddlOrdDeliveryFilters").kendoDropDownList({
                            dataTextField: "text",
                            dataValueField: "value",
                            placeholder: "select",
                            dataSource: DelGridFilterData,
                            select: function (e) {
                                var name = e.dataItem.text;
                                var val = e.dataItem.value;
                                $("#txtOrdDeliveryFilterVal").data("kendoMaskedTextBox").value("");
                                $("#txtOrdDeliveryFilterDateVal").data("kendoDatePicker").value("");
                                switch (val) {
                                    case "None":
                                        $("#spOrdDeliveryFilterText").hide();
                                        $("#spOrdDeliveryFilterDate").hide();
                                        $("#spOrdDeliveryFilterButtons").hide();
                                        break;
                                    case "BookDateRequired":
                                        $("#spOrdDeliveryFilterText").hide();
                                        $("#spOrdDeliveryFilterDate").show();
                                        $("#spOrdDeliveryFilterButtons").show();
                                        break;
                                    case "ScheduledDate":
                                        $("#spOrdDeliveryFilterText").hide();
                                        $("#spOrdDeliveryFilterDate").show();
                                        $("#spOrdDeliveryFilterButtons").show();
                                        break;
                                    default:
                                        $("#spOrdDeliveryFilterText").show();
                                        $("#spOrdDeliveryFilterDate").hide();
                                        $("#spOrdDeliveryFilterButtons").show();
                                        break;
                                }
                            }
                        });
                        $("#btnOrdDeliveryFilter").kendoButton({
                            icon: "filter",
                            click: function (e) {
                                var ddlvalue = $("#ddlOrdDeliveryFilters").data("kendoDropDownList").dataItem().value;
                                $("#scheduleapptdeliveryGrid").data("kendoGrid").dataSource.read();
                            }
                        });
                        $("#btnOrdDeliveryClearFilter").kendoButton({
                            icon: "filter-clear",
                            click: function (e) {
                                var dropdownlist = $("#ddlOrdDeliveryFilters").data("kendoDropDownList");
                                dropdownlist.select(0);
                                dropdownlist.trigger("change");
                                $("#txtOrdDeliveryFilterVal").data("kendoMaskedTextBox").value("");
                                $("#txtOrdDeliveryFilterDateVal").data("kendoDatePicker").value("");
                                $("#spOrdDeliveryFilterText").hide();
                                $("#spOrdDeliveryFilterDate").hide();
                                $("#spOrdDeliveryFilterButtons").hide();
                                $("#scheduleapptdeliveryGrid").data("kendoGrid").dataSource.read();
                            }
                        });
                        $("#spOrdDeliveryFilterText").hide();
                        $("#spOrdDeliveryFilterDate").hide();
                        $("#spOrdDeliveryFilterButtons").hide();

                        /////*************Booked Appointment Pickup Filters***************//
                        $("#txtBookedOrdFilterVal").kendoMaskedTextBox();
                        $("#txtBookedOrdFilterDateVal").kendoDatePicker();
                        $("#ddlBookedOrdFilters").kendoDropDownList({
                            dataTextField: "text",
                            dataValueField: "value",
                            placeholder: "select",
                            dataSource: PickGridFilterData,
                            select: function (e) {
                                var name = e.dataItem.text;
                                var val = e.dataItem.value;
                                $("#txtBookedOrdFilterVal").data("kendoMaskedTextBox").value("");
                                $("#txtBookedOrdFilterDateVal").data("kendoDatePicker").value("");
                                switch (val) {
                                    case "None":
                                        $("#spBookedOrdFilterText").hide();
                                        $("#spBookedOrdFilterDate").hide();
                                        $("#spBookedOrdFilterButtons").hide();
                                        break;
                                    case "BookDateLoad":
                                        $("#spBookedOrdFilterText").hide();
                                        $("#spBookedOrdFilterDate").show();
                                        $("#spBookedOrdFilterButtons").show();
                                        break;
                                    case "ScheduledDate":
                                        $("#spBookedOrdFilterText").hide();
                                        $("#spBookedOrdFilterDate").show();
                                        $("#spBookedOrdFilterButtons").show();
                                        break;
                                    default:
                                        $("#spBookedOrdFilterText").show();
                                        $("#spBookedOrdFilterDate").hide();
                                        $("#spBookedOrdFilterButtons").show();
                                        break;
                                }
                            }
                        });
                        $("#btnBookedOrdFilter").kendoButton({ icon: "filter", click: function (e){ $("#bookedAppointmnetpickupGrid").data("kendoGrid").dataSource.read(); } });
                        $("#btnBookedOrdClearFilter").kendoButton({
                            icon: "filter-clear",
                            click: function (e) {
                                var dropdownlist = $("#ddlBookedOrdFilters").data("kendoDropDownList");
                                dropdownlist.select(0);
                                dropdownlist.trigger("change");
                                $("#txtBookedOrdFilterVal").data("kendoMaskedTextBox").value("");
                                $("#txtBookedOrdFilterDateVal").data("kendoDatePicker").value("");
                                $("#spBookedOrdFilterText").hide();
                                $("#spBookedOrdFilterDate").hide();
                                $("#spBookedOrdFilterButtons").hide();
                                $("#bookedAppointmnetpickupGrid").data("kendoGrid").dataSource.read();
                            }
                        });
                        $("#spBookedOrdFilterText").hide();
                        $("#spBookedOrdFilterDate").hide();
                        $("#spBookedOrdFilterButtons").hide();

                        /////*************Booked Appointment Delivery Filters***************//
                        $("#txtBookedOrdDeliveryFilterVal").kendoMaskedTextBox();
                        $("#txtBookedOrdDeliveryFilterDateVal").kendoDatePicker();
                        $("#ddlBookedOrdDeliveryFilters").kendoDropDownList({
                            dataTextField: "text",
                            dataValueField: "value",
                            placeholder: "select",
                            dataSource: DelGridFilterData,
                            select: function (e) {
                                var name = e.dataItem.text;
                                var val = e.dataItem.value;
                                $("#txtBookedOrdDeliveryFilterVal").data("kendoMaskedTextBox").value("");
                                $("#txtBookedOrdDeliveryFilterDateVal").data("kendoDatePicker").value("");
                                switch (val) {
                                    case "None":
                                        $("#spBookedOrdDeliveryFilterText").hide();
                                        $("#spBookedOrdDeliveryFilterDate").hide();
                                        $("#spBookedOrdDeliveryFilterButtons").hide();
                                        break;
                                    case "BookDateRequired":
                                        $("#spBookedOrdDeliveryFilterText").hide();
                                        $("#spBookedOrdDeliveryFilterDate").show();
                                        $("#spBookedOrdDeliveryFilterButtons").show();
                                        break;
                                    case "ScheduledDate":
                                        $("#spBookedOrdDeliveryFilterText").hide();
                                        $("#spBookedOrdDeliveryFilterDate").show();
                                        $("#spBookedOrdDeliveryFilterButtons").show();
                                        break;
                                    default:
                                        $("#spBookedOrdDeliveryFilterText").show();
                                        $("#spBookedOrdDeliveryFilterDate").hide();
                                        $("#spBookedOrdDeliveryFilterButtons").show();
                                        break;
                                }
                            }
                        });
                        $("#btnBookedOrdDeliveryFilter").kendoButton({ icon: "filter", click: function (e) { $("#bookedAppointmnetdeliveryGrid").data("kendoGrid").dataSource.read(); } });
                        $("#btnBookedOrdDeliveryClearFilter").kendoButton({
                            icon: "filter-clear",
                            click: function (e) {
                                var dropdownlist = $("#ddlBookedOrdDeliveryFilters").data("kendoDropDownList");
                                dropdownlist.select(0);
                                dropdownlist.trigger("change");
                                $("#txtBookedOrdDeliveryFilterVal").data("kendoMaskedTextBox").value("");
                                $("#txtBookedOrdDeliveryFilterDateVal").data("kendoDatePicker").value("");
                                $("#spBookedOrdDeliveryFilterText").hide();
                                $("#spBookedOrdDeliveryFilterDate").hide();
                                $("#spBookedOrdDeliveryFilterButtons").hide();
                                $("#bookedAppointmnetdeliveryGrid").data("kendoGrid").dataSource.read();
                            }
                        });
                        $("#spBookedOrdDeliveryFilterText").hide();
                        $("#spBookedOrdDeliveryFilterDate").hide();
                        $("#spBookedOrdDeliveryFilterButtons").hide();

                        ////// Filter Ends////////////

                        dsScheduleApptPickUpOrdersData = new kendo.data.DataSource({                          
                            serverSorting: true, 
                            serverPaging: true, 
                            pageSize: 5,
                            transport: {
                                read: function (options) {
                                    var s = new AllFilter();
                                    s.sortName = $("#txtOrdSortField").val();
                                    s.sortDirection = $("#txtOrdSortDirection").val();
                                    s.page = options.data.page;
                                    s.skip = options.data.skip;
                                    s.take = options.data.take;                                  
                                    s.filterName = $("#ddlOrdFilters").data("kendoDropDownList").value();
                                    switch (s.filterName) {
                                        case "None":
                                            s.filterName = "";
                                            s.filterValue = "";
                                            break;
                                        case "BookDateLoad":
                                            //Modified By LVV on 8/29/2018 -- bug fix #4
                                            s.filterValue = "";
                                            s.filterFrom = $("#txtOrdFilterDateVal").val();
                                            s.filterTo = $("#txtOrdFilterDateVal").val();
                                            break;
                                        case "ScheduledDate":
                                            //Modified By LVV on 8/29/2018 -- bug fix #4
                                            s.filterValue = "";
                                            s.filterFrom = $("#txtOrdFilterDateVal").val();
                                            s.filterTo = $("#txtOrdFilterDateVal").val();
                                            break;
                                        default:
                                            s.filterValue = $("#txtOrdFilterVal").data("kendoMaskedTextBox").value();
                                            break;
                                    }
                                    $.ajax({
                                        url: '/api/AMSCarrier/GetCarrierPickUpOrdersForAppt/',
                                        contentType: 'application/json; charset=utf-8',
                                        dataType: 'json',
                                        data: { filter: JSON.stringify(s) },
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                        success: function (data) {
                                            options.success(data);
                                            console.log(data);
                                            try {
                                                var blnSuccess = false;
                                                var blnErrorShown = false;
                                                var strValidationMsg = "";
                                                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                                    if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Get Resources Failure", data.Errors, null); }
                                                    else {
                                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) { blnSuccess = true; }
                                                            else { blnSuccess = true; strValidationMsg = "No records were found matching your search criteria"; }
                                                        }
                                                    }
                                                }
                                                if (blnSuccess === false && blnErrorShown === false) {
                                                    if (strValidationMsg.length < 1) { strValidationMsg = "Get Resources Failure"; }
                                                    ngl.showErrMsg("Get Resources Failure", strValidationMsg, null);
                                                }
                                            } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                                        },
                                        error: function (result) { options.error(result); }
                                    });
                                },
                                update: function (options) {
                                    var vData = {};
                                    vData.BookControl = options.data.BookControl;
                                    vData.ApptControl = 0;
                                    vData.EquipID = options.data.BookCarrTrailerNo;
                                    vData.IsPickup = options.data.IsPickup;
                                    vData.Success = false;
                                    vData.IsAdd = false;
                                    vData.ErrMsg = "";

                                    EquipIDValidation = vData;
                                    EditEquipIDOrder = options.data;

                                    var oData = { "order": EditEquipIDOrder, "equipID": EquipIDValidation };
                                    updateEquipID(oData);
                                },
                                parameterMap: function (options, operation) { return options; }
                            },
                            schema: {
                                data: "Data",
                                total: "Count",
                                model: {
                                    id: "BookControl",
                                    fields: {
                                        BookControl: { type: "number", editable: false },
                                        BookSHID: { type: "string", editable: false },
                                        BookConsPrefix: { type: "string", editable: false },
                                        BookCarrOrderNumber: { type: "string", editable: false },
                                        BookProNumber: { type: "string", editable: false },
                                        BookLoadPONumber: { type: "string", editable: false },
                                        BookCarrTrailerNo: { type: "string" },
                                        BookDateLoad: { type: "date", editable: false },
                                        BookDateRequired: { type: "date", editable: false },
                                        ScheduledDate: { type: "date", editable: false },
                                        ScheduledTime: { type: "date", editable: false },
                                        BookAMSPickupApptControl: { type: "string", editable: false },
                                        BookAMSDeliveryApptControl: { type: "string", editable: false },
                                        Warehouse: { type: "string", editable: false },
                                        Address1: { type: "string", editable: false },
                                        Address2: { type: "string", editable: false },
                                        City: { type: "string", editable: false },
                                        State: { type: "string", editable: false },
                                        Country: { type: "string", editable: false },
                                        Zip: { type: "string", editable: false },
                                        BookCarrierControl: { type: "string", editable: false },
                                        CarrierName: { type: "string", editable: false },
                                        CarrierNumber: { type: "string", editable: false },
                                        BookShipCarrierProNumber: { type: "string", editable: false },
                                        BookShipCarrierNumber: { type: "string", editable: false },
                                        BookShipCarrierName: { type: "string", editable: false },
                                        BookCarrierContControl: { type: "string", editable: false },
                                        BookCarrierContact: { type: "string", editable: false },
                                        BookCarrierContactPhone: { type: "string", editable: false },
                                        CompNEXTrack: { type: "string", editable: false },
                                        BookCustCompControl: { type: "string", editable: false },
                                        CompNumber: { type: "string", editable: false },
                                        Inbound: { type: "boolean", editable: false },
                                        BookTranCode: { type: "string", editable: false },
                                        BookTotalCases: { type: "string", editable: false },
                                        BookTotalWgt: { type: "string", editable: false },
                                        BookTotalPL: { type: "string", editable: false },
                                        BookTotalCube: { type: "string", editable: false },
                                        BookWhseAuthorizationNo: { type: "string", editable: false },
                                        BookCarrDockPUAssigment: { type: "string", editable: false },
                                        BookCarrDockDelAssignment: { type: "string", editable: false },
                                        BookNotesVisable1: { type: "string", editable: false },
                                        BookNotesVisable2: { type: "string", editable: false },
                                        BookNotesVisable3: { type: "string", editable: false },
                                        LaneNumber: { type: "string", editable: false },
                                        LaneControl: { type: "string", editable: false },
                                        IsTransfer: { type: "boolean", editable: false },
                                        IsPickup: { type: "boolean", editable: false },
                                        BookOrigCompControl: { type: "number", editable: false }
                                    }
                                },
                                errors: "Errors"
                            },
                            error: function (xhr, textStatus, error) { ngl.showErrMsg("Access schedule appointment pickup Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges(); }
                        });

                        $("#scheduleApptPickupGrid").kendoGrid({         
                            noRecords: { template: "<p>No records available.</p>" },
                            toolbar: ["excel"],
                            excel: { fileName: "unschedPick.xlsx", allPages: true },
                            groupable: true,
                            resizable: true,
                            reorderable: true,                          
                            sortable: { mode: "single", allowUnsort: true },                           
                            sort: function (e) {
                                if (!e.sort.dir) { e.sort.dir == ""; e.sort.field == ""; }
                                if (!e.sort.field) { e.sort.field == ""; }
                                $("#txtOrdSortDirection").val(e.sort.dir);
                                $("#txtOrdSortField").val(e.sort.field);
                            },
                            pageable: { pageSizes: [5, 10, 15, 20, 25, 50] },
                            //height: 200,
                            dataSource: dsScheduleApptPickUpOrdersData,
                            editable: "inline",
                            dataBound: function (e) {
                                var tObj = this;
                                tObj.tbody.find("tr[role='row']").each(function () {
                                    var model = tObj.dataItem(this);
                                    if (model.Inbound == true) {
                                        $(this).find(".k-grid-edit").addClass("k-state-disabled");
                                        e.preventDefault();
                                        $(".k-state-disabled").each(function (index) {
                                            $(this).removeClass('k-grid-edit')
                                        });
                                    }
                                });
                            },
                            columns: [
                                { command: [{ className: "cm-icononly-button", name: "edit", text: { edit: "", update: "", cancel: "" }, iconClass: "k-icon k-i-pencil" }, { name: "selectAppt", text: "Select Appt", click: ViewAvailAppt }], title: "Action", width: 140 },
                                { field: "BookControl", hidden: true },
                                { field: "BookSHID", title: "SHID" },
                                { field: "BookConsPrefix", title: "CNS" },
                                { field: "BookCarrOrderNumber", title: "Order No" },
                                { field: "BookProNumber", title: "PRO", hidden: true },
                                { field: "BookLoadPONumber", title: "PO", hidden: true },
                                { field: "BookCarrTrailerNo", title: "Equip ID" },                               
                                { field: "BookAMSPickupApptControl", hidden: true },
                                { field: "BookAMSDeliveryApptControl", hidden: true },
                                { field: "Warehouse", title: "Warehouse", width: "110px" },
                                { field: "Address1", title: "Address 1", width: "125px" },
                                { field: "Address2", hidden: true },
                                { field: "City" },
                                { field: "State", width: "50px" },
                                { field: "Zip", width: "50px" },
                                { field: "Country", hidden: true },                                  
                                { field: "ScheduledDate", title: "Sched Date", format: "{0:M/d/yyyy}" },
                                { field: "ScheduledTime", title: "Sched Time", format: "{0:hh:mm tt}" },
                                { field: "BookDateLoad", title: "Load Date", format: "{0:M/d/yyyy}" },
                                { field: "BookDateRequired", title: "Req Date", format: "{0:M/d/yyyy}", hidden: true },
                                { field: "BookCarrierControl", hidden: true },
                                { field: "CarrierName", title: "Carrier Name", hidden: true },
                                { field: "CarrierNumber", title: "", hidden: true },
                                { field: "BookShipCarrierProNumber", title: "Assigned Carrier Pro", hidden: true },
                                { field: "BookShipCarrierNumber", title: "Assigned Carrier No", hidden: true },
                                { field: "BookShipCarrierName", title: "Assigned Carrier Name", hidden: true },
                                { field: "BookCarrierContControl", hidden: true },
                                { field: "BookCarrierContact", hidden: true },
                                { field: "BookCarrierContactPhone", hidden: true },
                                { field: "CompNEXTrack", hidden: true },
                                { field: "BookCustCompControl", title: "Comp Control", hidden: true },
                                { field: "CompNumber", title: "Comp No", hidden: true },
                                { field: "Inbound", hidden: true },
                                { field: "BookTranCode", hidden: true },
                                { field: "BookTotalCases", hidden: true },
                                { field: "BookTotalWgt", hidden: true },
                                { field: "BookTotalPL", hidden: true },
                                { field: "BookTotalCube", hidden: true },
                                { field: "BookWhseAuthorizationNo", hidden: true },
                                { field: "BookCarrDockPUAssigment", hidden: true },
                                { field: "BookCarrDockDelAssignment", hidden: true },
                                { field: "BookNotesVisable1", hidden: true },
                                { field: "BookNotesVisable2", hidden: true },
                                { field: "BookNotesVisable3", hidden: true },
                                { field: "LaneNumber", hidden: true },
                                { field: "LaneControl", hidden: true },
                                { field: "IsTransfer", hidden: true },
                                { field: "IsPickup", hidden: true },
                                { field: "BookOrigCompControl", hidden: true }
                            ],
                        });

                        var EditEquipIDOrder = {};
                        var EquipIDValidation = {};
                        $("#btnEditEquipIDSubmitChanges").kendoButton({
                            icon: 'save',
                            click: function () {
                                var oData = { "order": EditEquipIDOrder, "equipID": EquipIDValidation };
                                updateEquipID(oData);
                                wndEditOrdersEqiuipmentID.close();
                            }
                        });

                        function updateEquipID(oData) {
                            $.ajax({
                                type: "POST",
                                url: '/api/AMSCarrier/SaveEquipmentIDForOrder/',
                                contentType: "application/json; charset=utf-8",
                                dataType: 'json',
                                data: JSON.stringify(oData),
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                success: function (data) {
                                    try {
                                        var blnSuccess = false;
                                        var blnErrorShown = false;
                                        var strValidationMsg = "";
                                        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                            if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Save schedule appointment pickup  Failure", data.Errors, null); }
                                            else {
                                                if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                    if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                                        blnSuccess = true;
                                                        if (!data.Data[0].Success) {
                                                            if (data.Data[0].IsAdd) {
                                                                $("#lblEditEquipIDMessage").text(data.Data[0].ErrMsg);
                                                                $("#btnEditEquipIDSubmitChanges").show();
                                                            } else {
                                                                $("#lblEditEquipIDMessage").text(data.Data[0].ErrMsg);
                                                                $("#btnEditEquipIDSubmitChanges").hide();
                                                                //refresh the grids
                                                                $("#scheduleApptPickupGrid").data("kendoGrid").dataSource.read();
                                                                $("#scheduleapptdeliveryGrid").data("kendoGrid").dataSource.read();
                                                            }
                                                            EquipIDValidation = data.Data[0];
                                                            wndEditOrdersEqiuipmentID.center().open();
                                                        } else {
                                                            //refresh the grids
                                                            $("#scheduleApptPickupGrid").data("kendoGrid").dataSource.read();
                                                            $("#scheduleapptdeliveryGrid").data("kendoGrid").dataSource.read();
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (blnSuccess === false && blnErrorShown === false) {
                                            if (strValidationMsg.length < 1) { strValidationMsg = "Save schedule appointment pickup  Failure"; }
                                            ngl.showErrMsg("Save schedule appointment pickup  Failure", strValidationMsg, null);
                                        }
                                    } catch (err) { ngl.showErrMsg(err.name, err.description, null); }                           
                                },
                                error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save schedule appointment pickup  Failure"); ngl.showErrMsg("Save schedule appointment pickup  Failure", sMsg, null); }
                            });
                        }

                        kendoWindow.height = 'auto';
                        kendoWindow.width = '850';
                        wndcarrierselect = $("#selectCarrierAppt").kendoWindow(kendoWindow).data("kendoWindow");

                        var eObject = new EmailObject();
               
                        $("#txtEmailComments").kendoMaskedTextBox();
                        $("#btnSubmitReqEmail").kendoButton({
                            icon: "email",
                            click: function () {
                                //eObject.emailTo = "";
                                eObject.emailFrom = fromEmail;
                                eObject.emailCc = "";
                                eObject.emailBcc = "";
                                //eObject.emailSubject ="";
                                eObject.emailBody = eObject.emailBody + $("#txtEmailComments").val();

                                sendReqEmail(eObject)
                                wndRequestEmail.close();
                            }
                        });

                        function sendReqEmail(eObject) {
                            $.ajax({
                                url: "api/Email/GenerateEmail",
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                dataType: 'json',
                                data: JSON.stringify(eObject),
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                success: function (data) {
                                    try {
                                        var blnSuccess = false;
                                        var blnErrorShown = false;
                                        var strValidationMsg = "";
                                        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                            if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Sent Request Email Failure", data.Errors, null); }
                                            else { if (data.StatusCode == 200){ blnSuccess = true; } }
                                        }
                                        if (blnSuccess === false && blnErrorShown === false) {
                                            if (strValidationMsg.length < 1) { strValidationMsg = "Sent Request Email Failure"; }
                                            ngl.showErrMsg("Sent Request Email Failure", strValidationMsg, null);
                                        }
                                    } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                                },
                                error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Sent Request Email Failure"); ngl.showErrMsg("Sent Request Email Failure", sMsg, null); }
                            });
                        }

                        kendoWindow.height = 'auto';
                        kendoWindow.width = '440';
                        wndRequestEmail = $("#ReqEmailForNewEditDelete").kendoWindow(kendoWindow).data("kendoWindow");

                        function ViewAvailAppt(e) {
                            BookObject = this.dataItem($(e.currentTarget).closest("tr"));
                 
                            $("#lblUOOrderNo").text(BookObject.BookCarrOrderNumber);
                            $("#lblUOEquipID").text(BookObject.BookCarrTrailerNo ? BookObject.BookCarrTrailerNo : "");
                            $("#lblUOLoadDate").text(kendo.toString(BookObject.BookDateLoad, 'M/d/yyyy'));
                            $("#lblUODeliveryDate").text(kendo.toString(BookObject.BookDateRequired, 'M/d/yyyy'));
                            $("#lblUOSheduled").text(kendo.toString(BookObject.ScheduledDate ? BookObject.ScheduledDate : "", 'M/d/yyyy') + " " + kendo.toString(BookObject.ScheduledTime ? BookObject.ScheduledTime : "", 'HH:mm'));
                            $("#lblUOSHID").text(BookObject.BookSHID);
                            $("#lblUOCNS").text(BookObject.BookConsPrefix);
                            $("#lblUOPickup").prop("checked", BookObject.IsPickup).prop("disabled", true);
                            $("#lblUOInbound").prop("checked", BookObject.Inbound).prop("disabled", true);
                            $("#lblUOTransfer").prop("checked", BookObject.IsTransfer).prop("disabled", true);

                            $("#avialbaleApptGrid").data("kendoGrid").dataSource.data([]);
                            $("#avialbaleApptGrid").data("kendoGrid").dataSource.read();
                        }

                        //**********popup  Available Appt Data grid**********//
                        var BookObject = {};
                        dsAvailableApptData = new kendo.data.DataSource({
                            pageSize: 10,
                            transport: {
                                read: function (options) {
                                    var s = new AllFilter();
                                    s.CarrierControl = BookObject.BookCarrierControl,
                                    s.CompControl = BookObject.BookCustCompControl,
                                    s.BookControl = BookObject.BookControl,
                                    s.EquipmentID = BookObject.BookCarrTrailerNo,
                                    s.BookDateLoad = BookObject.BookDateLoad,
                                    s.BookDateRequired = BookObject.BookDateRequired,
                                    s.IsPickup = BookObject.IsPickup,
                                    s.Inbound = BookObject.Inbound,
                                    s.Warehouse = BookObject.Warehouse,
                                    s.CarrierName = BookObject.CarrierName, //Added By LVV 8/31/18
                                    s.CarrierNumber = BookObject.CarrierNumber, //Added By LVV 8/31/18
                                    s.ScheduledDate = BookObject.ScheduledDate //Added By LVV 8/31/18
                                    s.ScheduledTime = BookObject.ScheduledTime //Added By LVV 8/31/18
                                    s.SHID = BookObject.BookSHID //Added By LVV 10/17/18
                                    s.BookOrigCompControl = BookObject.BookOrigCompControl,
                                    s.BookDestCompControl = BookObject.BookDestCompControl,

                                    $.ajax({
                                        url: '/api/AMSCarrier/GetCarrierAvailableAppointments/',
                                        contentType: 'application/json; charset=utf-8',
                                        dataType: 'json',
                                        data: { filter: JSON.stringify(s) },
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                        success: function (data) {
                                            options.success(data);
                                            console.log(data.Data);
                                            try {
                                                var blnSuccess = false;
                                                var blnErrorShown = false;
                                                var strValidationMsg = "";
                                                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                                    if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Get Available Appointments Failure", data.Errors, null); }
                                                    else {
                                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                                                blnSuccess = true;
                                                                if (data.Data[0].Message != undefined) {
                                                                    eObject.emailTo = data.Data[0].RequestSendToEmail;
                                                                    eObject.emailSubject = data.Data[0].Subject;
                                                                    eObject.emailBody = data.Data[0].Body;
                                                                    $("#lblEmailMessage").text(data.Data[0].Message?data.Data[0].Message:"");
                                                                    //$("#lblEmailBody").text(data.Data[0].Body);
                                                                    $("#lblEmailBody").html(data.Data[0].Body);
                                                                    $("#txtEmailComments").val("");

                                                                    wndRequestEmail.title(data.Data[0].Subject);
                                                                    wndRequestEmail.center().open();
                                                                } else {
                                                                    wndcarrierselect.title("Carrier Scheduling - Select Appointment");
                                                                    wndcarrierselect.center().open();
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                if (blnSuccess === false && blnErrorShown === false) {
                                                    if (strValidationMsg.length < 1) { strValidationMsg = "Get available Resources Failure"; }
                                                    ngl.showErrMsg("Get available Resources Failure", strValidationMsg, null);
                                                }
                                            } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
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
                                    id: "CarrierNumber",
                                    fields: {
                                        CarrierControl: { type: "number" },
                                        CarrierNumber: { type: "number" },
                                        CarrierName: { type: "string" },
                                        CompControl: { type: "number" },
                                        Warehouse: { type: "string" },
                                        Date: { type: "date" },
                                        StartTime: { type: "date" },
                                        EndTime: { type: "string" },
                                        Docks: { type: "string" },
                                        Books: { type: "string" },                                                                              
                                        ////EndTime: { type: "date" },
                                    }
                                },
                                errors: "Errors"
                            },
                            error: function (xhr, textStatus, error) { ngl.showErrMsg("Access available Appointment Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges(); }
                        });

                        $("#avialbaleApptGrid").kendoGrid({
                            noRecords: { template: "<p>No records available.</p>" },
                            autoBind: false,
                            height: 200,
                            dataSource: dsAvailableApptData,
                            pageable: true,
                            columns: [
                                { command: [{ name: "bookAppt", text: "Book Appt", width: 100, click: BookAppointment }], title: "Action" },
                                { field: "Warehouse", title: "Warehouse" },
                                { field: "Date", title: "Date", format: "{0:M/d/yyyy}" },
                                { field: "StartTime", title: "Start Time", format: "{0:HH:mm}" },
                                { field: "EndTime", title: "End Time", hidden: true },
                                { field: "Docks", title: "Docks", hidden: true },
                                { field: "Books", title: "Books", hidden: true },
                                { field: "CarrierControl", title: "CarrierControl", hidden: true },
                                { field: "CarrierNumber", title: "CarrierNumber", hidden: true },
                                { field: "CarrierName", title: "CarrierName", hidden: true },
                                { field: "CompControl", title: "CompControl", hidden: true },
                            ],
                        });

                        function BookAppointment(e) {
                            var BookApptObject = this.dataItem($(e.currentTarget).closest("tr"));                                   
                            //Modified By LVV on 8/27/2018 for v-8.3 Scheduler - Added code to fix the problem with the dates being converted from different time zones
                            //Modified by RHR for v-8.2 on 09/19/2018 we now use ngl.convertDateForWindows to avoid non-ascii characters in date string
                            BookApptObject.EndTime = BookApptObject.EndTime;
                            BookApptObject.StartTime = ngl.convertTimePickerToDateString(BookApptObject.StartTime, ngl.convertDateForWindows(BookApptObject.StartTime, ""), "");
                            console.log(BookApptObject);
                            $.ajax({
                                url: "api/AMSCarrier/BookCarrierAppointment",
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                dataType: 'json',
                                data: JSON.stringify(BookApptObject),
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                success: function (data) {
                                    try {
                                        var blnSuccess = false;
                                        var blnErrorShown = false;
                                        var strValidationMsg = "";
                                        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                            if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Book Carrier Appointment Failure", data.Errors, null); }
                                            else {
                                                if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                    if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                                        blnSuccess = true;
                                                        if (data.Data[0] == false) { ngl.showWarningMsg("Book Carrier Appointment Failure!", "", null); } 
                                                        else {
                                                            //refresh Grid
                                                            wndcarrierselect.close();
                                                            ngl.showSuccessMsg("Book Carrier Appointment Sucess");
                                                            $("#scheduleApptPickupGrid").data("kendoGrid").dataSource.read();
                                                            $("#scheduleapptdeliveryGrid").data("kendoGrid").dataSource.read();
                                                            $("#bookedAppointmnetpickupGrid").data("kendoGrid").dataSource.read();
                                                            $("#bookedAppointmnetdeliveryGrid").data("kendoGrid").dataSource.read();
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (blnSuccess === false && blnErrorShown === false) {
                                            if (strValidationMsg.length < 1) { strValidationMsg = "Book Carrier Appointment Failure"; }
                                            ngl.showErrMsg("Book Carrier Appointment Failure", strValidationMsg, null);
                                        }
                                    } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                                },
                                error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Book Carrier Appointment Failure"); ngl.showErrMsg("Book Carrier Appointment Failure", sMsg, null); }
                            });
                        }

                        dsScheduleApptDeliveryOrdersData = new kendo.data.DataSource({
                            serverSorting: true, 
                            serverPaging: true, 
                            pageSize: 5,
                            transport: {
                                read: function (options) {
                                    var s = new AllFilter();
                                    s.sortName = $("#txtOrdDeliverySortField").val();
                                    s.sortDirection = $("#txtOrdDeliverySortDirection").val();
                                    s.page = options.data.page;
                                    s.skip = options.data.skip;
                                    s.take = options.data.take;
                                    s.filterName = $("#ddlOrdDeliveryFilters").data("kendoDropDownList").value();
                                    switch (s.filterName) {
                                        case "None":
                                            s.filterName = "";
                                            s.filterValue = "";
                                            break;
                                        case "BookDateLoad":
                                            //Modified By LVV on 8/29/2018 -- bug fix #4
                                            s.filterValue = "";
                                            s.filterFrom = $("#txtOrdDeliveryFilterDateVal").val();
                                            s.filterTo = $("#txtOrdDeliveryFilterDateVal").val();
                                            break;
                                        case "BookDateRequired":
                                            //Modified By LVV on 8/29/2018 -- bug fix #4
                                            s.filterValue = "";
                                            s.filterFrom = $("#txtOrdDeliveryFilterDateVal").val();
                                            s.filterTo = $("#txtOrdDeliveryFilterDateVal").val();
                                            break;
                                        case "ScheduledDate":
                                            //Modified By LVV on 8/29/2018 -- bug fix #4
                                            s.filterValue = "";
                                            s.filterFrom = $("#txtOrdDeliveryFilterDateVal").val();
                                            s.filterTo = $("#txtOrdDeliveryFilterDateVal").val();
                                            break;
                                        default:
                                            s.filterValue = $("#txtOrdDeliveryFilterVal").data("kendoMaskedTextBox").value();
                                            break;
                                    }
                                    $.ajax({
                                        url: '/api/AMSCarrier/GetCarrierDeliveryOrdersForAppt/',
                                        contentType: 'application/json; charset=utf-8',
                                        dataType: 'json',
                                        data: { filter: JSON.stringify(s) },
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                        success: function (data) {
                                            console.log(data);
                                            options.success(data);
                                            try {
                                                var blnSuccess = false;
                                                var blnErrorShown = false;
                                                var strValidationMsg = "";
                                                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                                    if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Get Resources Failure", data.Errors, null); }
                                                    else {
                                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) { blnSuccess = true; }
                                                            else { blnSuccess = true; strValidationMsg = "No records were found matching your search criteria"; }
                                                        }
                                                    }
                                                }
                                                if (blnSuccess === false && blnErrorShown === false) {
                                                    if (strValidationMsg.length < 1) { strValidationMsg = "Get Resources Failure"; }
                                                    ngl.showErrMsg("Get Resources Failure", strValidationMsg, null);
                                                }
                                            } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                                        },
                                        error: function (result) { options.error(result); }
                                    });
                                },
                                update: function (options) {
                                    var vData = {};
                                    vData.BookControl = options.data.BookControl;
                                    vData.ApptControl = 0;
                                    vData.EquipID = options.data.BookCarrTrailerNo;
                                    vData.IsPickup = options.data.IsPickup;
                                    vData.Success = false;
                                    vData.IsAdd = false;
                                    vData.ErrMsg = "";

                                    EquipIDValidation = vData;
                                    EditEquipIDOrder = options.data;

                                    var oData = { "order": EditEquipIDOrder, "equipID": EquipIDValidation };
                                    updateEquipID(oData);
                                },
                                parameterMap: function (options, operation) { return options; }
                            },
                            schema: {
                                data: "Data",
                                total: "Count",
                                model: {
                                    id: "BookControl",
                                    fields: {
                                        BookControl: { type: "number", editable: false },
                                        BookSHID: { type: "string", editable: false },
                                        BookConsPrefix: { type: "string", editable: false },
                                        BookCarrOrderNumber: { type: "string", editable: false },
                                        BookProNumber: { type: "string", editable: false },
                                        BookLoadPONumber: { type: "string", editable: false },
                                        BookCarrTrailerNo: { type: "string" },
                                        BookDateLoad: { type: "date", editable: false },
                                        BookDateRequired: { type: "date", editable: false },
                                        ScheduledDate: { type: "date", editable: false },
                                        ScheduledTime: { type: "date", editable: false },
                                        BookAMSPickupApptControl: { type: "string", editable: false },
                                        BookAMSDeliveryApptControl: { type: "string", editable: false },
                                        Warehouse: { type: "string", editable: false },
                                        Address1: { type: "string", editable: false },
                                        Address2: { type: "string", editable: false },
                                        City: { type: "string", editable: false },
                                        State: { type: "string", editable: false },
                                        Country: { type: "string", editable: false },
                                        Zip: { type: "string", editable: false },
                                        BookCarrierControl: { type: "string", editable: false },
                                        CarrierName: { type: "string", editable: false },
                                        CarrierNumber: { type: "string", editable: false },
                                        BookShipCarrierProNumber: { type: "string", editable: false },
                                        BookShipCarrierNumber: { type: "string", editable: false },
                                        BookShipCarrierName: { type: "string", editable: false },
                                        BookCarrierContControl: { type: "string", editable: false },
                                        BookCarrierContact: { type: "string", editable: false },
                                        BookCarrierContactPhone: { type: "string", editable: false },
                                        CompNEXTrack: { type: "string", editable: false },
                                        BookCustCompControl: { type: "string", editable: false },
                                        CompNumber: { type: "string", editable: false },
                                        Inbound: { type: "boolean", editable: false },
                                        BookTranCode: { type: "string", editable: false },
                                        BookTotalCases: { type: "string", editable: false },
                                        BookTotalWgt: { type: "string", editable: false },
                                        BookTotalPL: { type: "string", editable: false },
                                        BookTotalCube: { type: "string", editable: false },
                                        BookWhseAuthorizationNo: { type: "string", editable: false },
                                        BookCarrDockPUAssigment: { type: "string", editable: false },
                                        BookCarrDockDelAssignment: { type: "string", editable: false },
                                        BookNotesVisable1: { type: "string", editable: false },
                                        BookNotesVisable2: { type: "string", editable: false },
                                        BookNotesVisable3: { type: "string", editable: false },
                                        LaneNumber: { type: "string", editable: false },
                                        LaneControl: { type: "string", editable: false },
                                        IsTransfer: { type: "boolean", editable: false },
                                        IsPickup: { type: "boolean", editable: false },
                                        BookDestCompControl: { type: "number", editable: false }
                                    }
                                },
                                errors: "Errors"
                            },
                            error: function (xhr, textStatus, error) { ngl.showErrMsg("Access schedule appointment delivery  Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges(); }
                        });

                        $("#scheduleapptdeliveryGrid").kendoGrid({
                            noRecords: { template: "<p>No records available.</p>" },
                            toolbar: ["excel"],
                            excel: { fileName: "unschedDel.xlsx", allPages: true },
                            groupable: true,
                            resizable: true,
                            reorderable: true,                          
                            sortable: { mode: "single", allowUnsort: true },                           
                            sort: function (e) {
                                if (!e.sort.dir) { e.sort.dir == ""; e.sort.field == ""; }
                                if (!e.sort.field) { e.sort.field == ""; }
                                $("#txtOrdDeliverySortDirection").val(e.sort.dir);
                                $("#txtOrdDeliverySortField").val(e.sort.field);
                            },
                            pageable: { pageSizes: [5, 10, 15, 20, 25, 50] },
                            //height: 200,
                            dataSource: dsScheduleApptDeliveryOrdersData,
                            editable: 'inline',
                            resizable: true,
                            dataBound: function (e) {
                                var tObj = this;
                                tObj.tbody.find("tr[role='row']").each(function () {
                                    var model = tObj.dataItem(this);
                                    if (model.IsTransfer == true || model.Inbound == false) {
                                        $(this).find(".k-grid-edit").addClass("k-state-disabled");
                                        e.preventDefault();
                                        $(".k-state-disabled").each(function (index) {
                                            $(this).removeClass('k-grid-edit')
                                        });
                                    }
                                });
                            },
                            columns: [
                                  { command: [{ className: "cm-icononly-button", name: "edit", text: { edit: "", update: "", cancel: "" }, iconClass: "k-icon k-i-pencil" }, { name: "selectAppt", text: "Select Appt", click: ViewAvailAppt }], title: "Action", width: 140 },
                                  { field: "BookControl", hidden: true },
                                  { field: "BookSHID", title: "SHID" },
                                  { field: "BookConsPrefix", title: "CNS" },
                                  { field: "BookCarrOrderNumber", title: "Order No" },
                                  { field: "BookProNumber", title: "PRO", hidden: true },
                                  { field: "BookLoadPONumber", title: "PO", hidden: true },
                                  { field: "BookCarrTrailerNo", title: "Equip ID" },                                
                                  { field: "BookAMSPickupApptControl", hidden: true },
                                  { field: "BookAMSDeliveryApptControl", hidden: true },
                                  { field: "Warehouse", title: "Warehouse", width: "110px" },
                                  { field: "Address1", title: "Address 1", width: "125px" },
                                  { field: "Address2", hidden: true },
                                  { field: "City" },
                                  { field: "State", width: "50px" },
                                  { field: "Zip", width: "50px" },
                                  { field: "Country", hidden: true },                                                                 
                                  { field: "ScheduledDate", title: "Sched Date", format: "{0:M/d/yyyy}" },
                                  { field: "ScheduledTime", title: "Sched Time", format: "{0:hh:mm tt}" },
                                  { field: "BookDateRequired", title: "Req Date", format: "{0:M/d/yyyy}" },
                                  { field: "BookDateLoad", title: "Load Date", format: "{0:M/d/yyyy}", hidden: true },                                 
                                  { field: "BookCarrierControl", hidden: true },
                                  { field: "CarrierName", title: "Carrier Name", hidden: true },
                                  { field: "CarrierNumber", title: "", hidden: true },
                                  { field: "BookShipCarrierProNumber", title: "Assigned Carrier Pro", hidden: true },
                                  { field: "BookShipCarrierNumber", title: "Assigned Carrier No", hidden: true },
                                  { field: "BookShipCarrierName", title: "Assigned Carrier Name", hidden: true },
                                  { field: "BookCarrierContControl", hidden: true },
                                  { field: "BookCarrierContact", hidden: true },
                                  { field: "BookCarrierContactPhone", hidden: true },
                                  { field: "CompNEXTrack", hidden: true },
                                  { field: "BookCustCompControl", title: "Comp Control", hidden: true },
                                  { field: "CompNumber", title: "Comp No", hidden: true },
                                  { field: "Inbound", hidden: true },
                                  { field: "BookTranCode", hidden: true },
                                  { field: "BookTotalCases", hidden: true },
                                  { field: "BookTotalWgt", hidden: true },
                                  { field: "BookTotalPL", hidden: true },
                                  { field: "BookTotalCube", hidden: true },
                                  { field: "BookWhseAuthorizationNo", hidden: true },
                                  { field: "BookCarrDockPUAssigment", hidden: true },
                                  { field: "BookCarrDockDelAssignment", hidden: true },
                                  { field: "BookNotesVisable1", hidden: true },
                                  { field: "BookNotesVisable2", hidden: true },
                                  { field: "BookNotesVisable3", hidden: true },
                                  { field: "LaneNumber", hidden: true },
                                  { field: "LaneControl", hidden: true },
                                  { field: "IsTransfer", hidden: true },
                                  { field: "IsPickup", hidden: true },
                            ],
                        });

                        //Booked Appointments Tab...
                        dsBookedAppoinmentPickupData = new kendo.data.DataSource({
                            serverSorting: true, 
                            serverPaging: true, 
                            pageSize: 5,
                            transport: {
                                read: function (options) {
                                    var s = new AllFilter();
                                    s.sortName = $("#txtBookedOrdSortField").val();
                                    s.sortDirection = $("#txtBookedOrdSortDirection").val();
                                    s.page = options.data.page;
                                    s.skip = options.data.skip;
                                    s.take = options.data.take;
                                    s.filterName = $("#ddlBookedOrdFilters").data("kendoDropDownList").value();
                                    switch (s.filterName) {
                                        case "None":
                                            s.filterName = "";
                                            s.filterValue = "";
                                            break;
                                        case "BookDateLoad":
                                            //Modified By LVV on 8/29/2018 -- bug fix #4
                                            s.filterValue = "";
                                            s.filterFrom = $("#txtBookedOrdFilterDateVal").val();
                                            s.filterTo = $("#txtBookedOrdFilterDateVal").val();
                                            break;
                                        case "BookDateRequired":
                                            //Modified By LVV on 8/29/2018 -- bug fix #4
                                            s.filterValue = "";
                                            s.filterFrom = $("#txtBookedOrdFilterDateVal").val();
                                            s.filterTo = $("#txtBookedOrdFilterDateVal").val();
                                            break;
                                        case "ScheduledDate":
                                            //Modified By LVV on 8/29/2018 -- bug fix #4
                                            s.filterValue = "";
                                            s.filterFrom = $("#txtBookedOrdFilterDateVal").val();
                                            s.filterTo = $("#txtBookedOrdFilterDateVal").val();
                                            break;
                                        default:
                                            s.filterValue = $("#txtBookedOrdFilterVal").data("kendoMaskedTextBox").value();
                                            break;
                                    }
                                    $.ajax({
                                        url: '/api/AMSCarrier/GetCarrierBookedPickupOrders/',
                                        contentType: 'application/json; charset=utf-8',
                                        dataType: 'json',
                                        data: { filter: JSON.stringify(s) },
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                        success: function (data) {
                                            options.success(data);
                                            console.log(data);
                                            try {
                                                var blnSuccess = false;
                                                var blnErrorShown = false;
                                                var strValidationMsg = "";
                                                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                                    if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Get Resources Failure", data.Errors, null); }
                                                    else {
                                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) { blnSuccess = true; }
                                                            else { blnSuccess = true; strValidationMsg = "No records were found matching your search criteria"; }
                                                        }
                                                    }
                                                }
                                                if (blnSuccess === false && blnErrorShown === false) {
                                                    if (strValidationMsg.length < 1) { strValidationMsg = "Get Resources Failure"; }
                                                    ngl.showErrMsg("Get Resources Failure", strValidationMsg, null);
                                                }
                                            } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
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
                                    id: "BookControl",
                                    fields: {
                                        BookControl: { type: "number", editable: false },
                                        BookSHID: { type: "string", editable: false },
                                        BookConsPrefix: { type: "string", editable: false },
                                        BookCarrOrderNumber: { type: "string", editable: false },
                                        BookProNumber: { type: "string", editable: false },
                                        BookLoadPONumber: { type: "string", editable: false },
                                        BookCarrTrailerNo: { type: "string" },
                                        BookDateLoad: { type: "date", editable: false },
                                        BookDateRequired: { type: "date", editable: false },
                                        ScheduledDate: { type: "date", editable: false },
                                        ScheduledTime: { type: "date", editable: false },
                                        BookAMSPickupApptControl: { type: "string", editable: false },
                                        BookAMSDeliveryApptControl: { type: "string", editable: false },
                                        Warehouse: { type: "string", editable: false },
                                        Address1: { type: "string", editable: false },
                                        Address2: { type: "string", editable: false },
                                        City: { type: "string", editable: false },
                                        State: { type: "string", editable: false },
                                        Country: { type: "string", editable: false },
                                        Zip: { type: "string", editable: false },
                                        BookCarrierControl: { type: "string", editable: false },
                                        CarrierName: { type: "string", editable: false },
                                        CarrierNumber: { type: "string", editable: false },
                                        BookShipCarrierProNumber: { type: "string", editable: false },
                                        BookShipCarrierNumber: { type: "string", editable: false },
                                        BookShipCarrierName: { type: "string", editable: false },
                                        BookCarrierContControl: { type: "string", editable: false },
                                        BookCarrierContact: { type: "string", editable: false },
                                        BookCarrierContactPhone: { type: "string", editable: false },
                                        CompNEXTrack: { type: "string", editable: false },
                                        BookCustCompControl: { type: "string", editable: false },
                                        CompNumber: { type: "string", editable: false },
                                        Inbound: { type: "boolean", editable: false },
                                        BookTranCode: { type: "string", editable: false },
                                        BookTotalCases: { type: "string", editable: false },
                                        BookTotalWgt: { type: "string", editable: false },
                                        BookTotalPL: { type: "string", editable: false },
                                        BookTotalCube: { type: "string", editable: false },
                                        BookWhseAuthorizationNo: { type: "string", editable: false },
                                        BookCarrDockPUAssigment: { type: "string", editable: false },
                                        BookCarrDockDelAssignment: { type: "string", editable: false },
                                        BookNotesVisable1: { type: "string", editable: false },
                                        BookNotesVisable2: { type: "string", editable: false },
                                        BookNotesVisable3: { type: "string", editable: false },
                                        LaneNumber: { type: "string", editable: false },
                                        LaneControl: { type: "string", editable: false },
                                        IsTransfer: { type: "boolean", editable: false },
                                        IsPickup: { type: "boolean", editable: false },
                                        BookOrigCompControl: { type: "number", editable: false }
                                    }
                                },
                                errors: "Errors"
                            },
                            error: function (xhr, textStatus, error) { ngl.showErrMsg("Access booked Appointmnet pickup Grid Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges(); }
                        });

                        $("#bookedAppointmnetpickupGrid").kendoGrid({
                            noRecords: { template: "<p>No records available.</p>" },
                            toolbar: ["excel"],
                            excel: { fileName: "bookedPick.xlsx", allPages: true },
                            groupable: true,
                            resizable: true,
                            reorderable: true,                          
                            sortable: { mode: "single", allowUnsort: true },                           
                            sort: function (e) {
                                if (!e.sort.dir) { e.sort.dir == ""; e.sort.field == ""; }
                                if (!e.sort.field) { e.sort.field == ""; }
                                $("#txtBookedOrdSortDirection").val(e.sort.dir);
                                $("#txtBookedOrdSortField").val(e.sort.field);
                            },
                            pageable: { pageSizes: [5, 10, 15, 20, 25, 50] },
                            //height: 200,
                            // editable: 'inline',
                            dataSource: dsBookedAppoinmentPickupData,
                            //dataBound: function (e) {
                            //    var tObj = this;
                            //    tObj.tbody.find("tr[role='row']").each(function () {
                            //        var model = tObj.dataItem(this);
                            //        if (model.Inbound == true) {
                            //            $(this).find(".k-grid-edit").addClass("k-state-disabled");
                            //            e.preventDefault();
                            //            $(".k-state-disabled").each(function (index) {
                            //                $(this).removeClass('k-grid-edit')
                            //            });
                            //        }
                            //    });
                            //},
                            columns: [
                                { command: [{ className: "cm-icononly-button", name: "edit", text: { edit: "", update: "", cancel: "" }, click: ViewAvailApptForBookedApptEdit }, { className: "cm-icononly-button", name: "delete", text: "", iconClass: "k-icon k-i-trash", click: ViewAvailApptForBookedApptDelete }], title: "Action", width: 120 },
                                { field: "BookControl", hidden: true },
                                { field: "BookSHID", title: "SHID" },
                                { field: "BookConsPrefix", title: "CNS" },
                                { field: "BookCarrOrderNumber", title: "Order No" },
                                { field: "BookProNumber", title: "PRO", hidden: true },
                                { field: "BookLoadPONumber", title: "PO", hidden: true },
                                { field: "BookCarrTrailerNo", title: "Equip ID" },                             
                                { field: "BookAMSPickupApptControl", hidden: true },
                                { field: "BookAMSDeliveryApptControl", hidden: true },
                                { field: "Warehouse", title: "Warehouse", width: "110px" },
                                { field: "Address1", title: "Address 1", width: "125px" },
                                { field: "Address2", hidden: true },
                                { field: "City" },
                                { field: "State", width: "50px" },
                                { field: "Zip", width: "50px" },
                                { field: "Country", hidden: true },                                
                                { field: "ScheduledDate", title: "Sched Date", format: "{0:M/d/yyyy}" },
                                { field: "ScheduledTime", title: "Sched Time", format: "{0:hh:mm tt}" },
                                { field: "BookDateLoad", title: "Load Date", format: "{0:M/d/yyyy}" },
                                { field: "BookDateRequired", title: "Req Date", format: "{0:M/d/yyyy}", hidden: true },                               
                                { field: "BookCarrierControl", hidden: true },
                                { field: "CarrierName", title: "Carrier Name", hidden: true },
                                { field: "CarrierNumber", title: "", hidden: true },
                                { field: "BookShipCarrierProNumber", title: "Assigned Carrier Pro", hidden: true },
                                { field: "BookShipCarrierNumber", title: "Assigned Carrier No", hidden: true },
                                { field: "BookShipCarrierName", title: "Assigned Carrier Name", hidden: true },
                                { field: "BookCarrierContControl", hidden: true },
                                { field: "BookCarrierContact", hidden: true },
                                { field: "BookCarrierContactPhone", hidden: true },
                                { field: "CompNEXTrack", hidden: true },
                                { field: "BookCustCompControl", title: "Comp Control", hidden: true },
                                { field: "CompNumber", title: "Comp No", hidden: true },
                                { field: "Inbound", hidden: true },
                                { field: "BookTranCode", hidden: true },
                                { field: "BookTotalCases", hidden: true },
                                { field: "BookTotalWgt", hidden: true },
                                { field: "BookTotalPL", hidden: true },
                                { field: "BookTotalCube", hidden: true },
                                { field: "BookWhseAuthorizationNo", hidden: true },
                                { field: "BookCarrDockPUAssigment", hidden: true },
                                { field: "BookCarrDockDelAssignment", hidden: true },
                                { field: "BookNotesVisable1", hidden: true },
                                { field: "BookNotesVisable2", hidden: true },
                                { field: "BookNotesVisable3", hidden: true },
                                { field: "LaneNumber", hidden: true },
                                { field: "LaneControl", hidden: true },
                                { field: "IsTransfer", hidden: true },
                                { field: "IsPickup", hidden: true },
                            ],
                        });
               
                        function ViewAvailApptForBookedApptEdit(e) {
                            BookBookedObject = this.dataItem($(e.currentTarget).closest("tr"));
                            UpdateApptControler = 0;
                            if(BookBookedObject.IsPickup){ UpdateApptControler = BookBookedObject.BookAMSPickupApptControl; } 
                            else { UpdateApptControler = BookBookedObject.BookAMSDeliveryApptControl; }            

                            $("#lblBOOrderNo").text(BookBookedObject.BookCarrOrderNumber);
                            $("#lblBOEquipID").text(BookBookedObject.BookCarrTrailerNo ? BookObject.BookCarrTrailerNo : "");
                            $("#lblBOLoadDate").text(kendo.toString(BookBookedObject.BookDateLoad, 'M/d/yyyy'));
                            $("#lblBODeliveryDate").text(kendo.toString(BookBookedObject.BookDateRequired, 'M/d/yyyy'));
                            $("#lblBOSheduled").text(kendo.toString(BookBookedObject.ScheduledDate ? BookBookedObject.ScheduledDate : "", 'M/d/yyyy') + " " + kendo.toString(BookBookedObject.ScheduledTime ? BookBookedObject.ScheduledTime : "", 'HH:mm'));
                            $("#lblBOSHID").text(BookBookedObject.BookSHID);
                            $("#lblBOCNS").text(BookBookedObject.BookConsPrefix);
                            $("#lblBOPickup").prop("checked", BookBookedObject.IsPickup).prop("disabled", true);
                            $("#lblBOInbound").prop("checked", BookBookedObject.Inbound).prop("disabled", true);
                            $("#lblBOTransfer").prop("checked", BookBookedObject.IsTransfer).prop("disabled", true);
                   
                            $("#availApptForBookedApptGrid").data("kendoGrid").dataSource.data([]);
                            $("#availApptForBookedApptGrid").data("kendoGrid").dataSource.read();
                        };

                        var BookBookedObject = {};
                        dsAvailableApptforBookedApptData = new kendo.data.DataSource({
                            pageSize: 10,
                            transport: {
                                read: function (options) {
                                    var filters = new AllFilter();
                                    filters.CarrierControl = BookBookedObject.BookCarrierControl;
                                    filters.CompControl = BookBookedObject.BookCustCompControl;
                                    filters.BookControl = BookBookedObject.BookControl.toString();;
                                    filters.SHID = BookBookedObject.BookSHID;
                                    filters.EquipmentID = BookBookedObject.BookCarrTrailerNo ? BookBookedObject.BookCarrTrailerNo : "";
                                    filters.BookDateLoad = BookBookedObject.BookDateLoad;
                                    filters.BookDateRequired = BookBookedObject.BookDateRequired;
                                    filters.IsPickup = BookBookedObject.IsPickup;
                                    filters.Inbound = BookBookedObject.Inbound;
                                    filters.Warehouse = BookBookedObject.Warehouse;
                                    filters.CarrierName = BookBookedObject.CarrierName;
                                    filters.IsDelete = false;
                                    filters.BookAMSPickupApptControl = BookBookedObject.BookAMSPickupApptControl ? BookBookedObject.BookAMSPickupApptControl : 0;
                                    filters.BookAMSDeliveryApptControl = BookBookedObject.BookAMSDeliveryApptControl ? BookBookedObject.BookAMSDeliveryApptControl : 0;
                                    filters.CarrierNumber = BookBookedObject.CarrierNumber; //Added By LVV 8/31/18
                                    $.ajax({
                                        url: '/api/AMSCarrier/GetModifiedCarrierAvlblAppointments/',
                                        contentType: 'application/json; charset=utf-8',
                                        dataType: 'json',
                                        data: { filter: JSON.stringify(filters) },
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                        success: function (data) {
                                            options.success(data);
                                            console.log(data.Data);
                                            try {
                                                var blnSuccess = false;
                                                var blnErrorShown = false;
                                                var strValidationMsg = "";
                                                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                                    if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Get Available Appointments Failure", data.Errors, null); }
                                                    else {
                                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                                                blnSuccess = true;
                                                                if (data.Data[0].blnMustRequestAppt != undefined) {
                                                                    if (data.Data[0].blnMustRequestAppt) {
                                                                        eObject.emailTo = data.Data[0].RequestSendToEmail;
                                                                        eObject.emailSubject = data.Data[0].Subject;
                                                                        eObject.emailBody = data.Data[0].Body;
                                                                        $("#lblEmailMessage").text(data.Data[0].Message?data.Data[0].Message:"");
                                                                        //$("#lblEmailBody").text(data.Data[0].Body);
                                                                        $("#lblEmailBody").html(data.Data[0].Body);
                                                                        $("#txtEmailComments").val("");

                                                                        wndRequestEmail.title(data.Data[0].Subject);
                                                                        wndRequestEmail.center().open();
                                                                    } else {
                                                                        $("#bookedAppointmnetpickupGrid").data("kendoGrid").dataSource.read();
                                                                        $("#bookedAppointmnetdeliveryGrid").data("kendoGrid").dataSource.read();
                                                                    }
                                                                } else {
                                                                    wndReqAvailApptForCarrierBookedAppt.title("Carrier Sheduling - Select Appointment");
                                                                    wndReqAvailApptForCarrierBookedAppt.center().open();
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                if (blnSuccess === false && blnErrorShown === false) {
                                                    if (strValidationMsg.length < 1) { strValidationMsg = "Get available Resources Failure"; }
                                                    ngl.showErrMsg("Get available Resources Failure", strValidationMsg, null);
                                                }
                                            } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
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
                                    id: "CarrierNumber",
                                    fields: {
                                        CarrierControl: { type: "number" },
                                        CarrierNumber: { type: "number" },
                                        CarrierName: { type: "string" },
                                        CompControl: { type: "number" },
                                        Warehouse: { type: "string" },
                                        Date: { type: "date" },
                                        StartTime: { type: "date" },
                                        EndTime: { type: "string" },
                                        Docks: { type: "string" },
                                        Books: { type: "string" }
                                    }
                                },
                                errors: "Errors"
                            },
                            error: function (xhr, textStatus, error) { ngl.showErrMsg("Access available Appointment Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges(); }
                        });

                        $("#availApptForBookedApptGrid").kendoGrid({
                            noRecords: { template: "<p>No records available.</p>" },
                            autoBind: false,
                            height: 200,
                            dataSource: dsAvailableApptforBookedApptData,
                            pageable: true,
                            columns: [
                                { command: [{ name: "bookAppt", text: "Book Appt", width: 100, click: UpdateBookAppointment }], title: "Action" },
                                { field: "Warehouse", title: "Warehouse" },
                                { field: "Date", title: "Date", format: "{0:M/d/yyyy}" },
                                { field: "StartTime", title: "Start Time", format: "{0:HH:mm}" },
                                { field: "EndTime", title: "End Time", hidden: true },
                                { field: "Docks", title: "Docks", hidden: true },
                                { field: "Books", title: "Books", hidden: true },
                                { field: "CarrierControl", title: "CarrierControl", hidden: true },
                                { field: "CarrierNumber", title: "CarrierNumber", hidden: true },
                                { field: "CarrierName", title: "CarrierName", hidden: true },
                                { field: "CompControl", title: "CompControl", hidden: true },
                            ],
                        });
               
                        //PopUps Window
                        kendoWindow.height = 'auto';
                        kendoWindow.width = '850';
                        wndReqAvailApptForCarrierBookedAppt = $("#reqAvailApptForCarrierBookedAppt").kendoWindow(kendoWindow).data("kendoWindow");

                        var UpdateApptControler;
                        function UpdateBookAppointment(e) {
                            var oBookObject = this.dataItem($(e.currentTarget).closest("tr"));
                   
                            var UpdateBookApptObject = new AMSCarrierAvailableSlots();
                            UpdateBookApptObject.ApptControl = UpdateApptControler;
                            UpdateBookApptObject.Docks = oBookObject.Docks;
                            UpdateBookApptObject.StartTime = ngl.convertTimePickerToDateString(oBookObject.StartTime, ngl.getShortDateString(oBookObject.StartTime, ""), "");
                            UpdateBookApptObject.EndTime = oBookObject.EndTime;
                            UpdateBookApptObject.Warehouse = oBookObject.Warehouse;
                            UpdateBookApptObject.CarrierControl = oBookObject.CarrierControl;
                            UpdateBookApptObject.CompControl = oBookObject.CompControl;
                            UpdateBookApptObject.CarrierNumber = oBookObject.CarrierNumber;
                            UpdateBookApptObject.CarrierName = oBookObject.CarrierName;
                            UpdateBookApptObject.Books = oBookObject.Books;
                            UpdateBookApptObject.Date = ngl.convertTimePickerToDateString(oBookObject.Date, ngl.getShortDateString(oBookObject.Date, ""), "");

                            $.ajax({
                                url: "api/AMSCarrier/UpdateBookedCarrierAppointment",
                                type: "POST",
                                //contentType: "application/json; charset=utf-8",
                                dataType: 'json',
                                data: {"":JSON.stringify(UpdateBookApptObject)},
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                success: function (data) {
                                    try {
                                        var blnSuccess = false;
                                        var blnErrorShown = false;
                                        var strValidationMsg = "";
                                        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                            if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Update Book Carrier Appointment Failure", data.Errors, null); }
                                            else {
                                                if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                    if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                                        blnSuccess = true;
                                                        if (data.Data[0] == false) { ngl.showWarningMsg("Update Book Carrier Appointment Failure!", "", null); } 
                                                        else {
                                                            //refresh Grids
                                                            wndReqAvailApptForCarrierBookedAppt.close();
                                                            ngl.showSuccessMsg("Update Book Carrier Appointment Sucess");
                                                            $("#scheduleApptPickupGrid").data("kendoGrid").dataSource.read();
                                                            $("#scheduleapptdeliveryGrid").data("kendoGrid").dataSource.read();
                                                            $("#bookedAppointmnetpickupGrid").data("kendoGrid").dataSource.read();
                                                            $("#bookedAppointmnetdeliveryGrid").data("kendoGrid").dataSource.read();
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (blnSuccess === false && blnErrorShown === false) {
                                            if (strValidationMsg.length < 1) { strValidationMsg = "Update Book Carrier Appointment Failure"; }
                                            ngl.showErrMsg("Update Book Carrier Appointment Failure", strValidationMsg, null);
                                        }
                                    } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                                },
                                error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Update Book Carrier Appointment Failure"); ngl.showErrMsg("Update Book Carrier Appointment Failure", sMsg, null); }
                            });
                        }

                        function ViewAvailApptForBookedApptDelete(e) {
                            var dataObject = this.dataItem($(e.currentTarget).closest("tr"));

                            var filters = new AllFilter();
                            filters.CarrierControl = dataObject.BookCarrierControl;
                            filters.CompControl = dataObject.BookCustCompControl;
                            filters.BookControl = dataObject.BookControl.toString();;
                            filters.SHID = dataObject.BookSHID;
                            filters.EquipmentID = dataObject.BookCarrTrailerNo ? dataObject.BookCarrTrailerNo : "";
                            filters.BookDateLoad = dataObject.BookDateLoad;
                            filters.BookDateRequired = dataObject.BookDateRequired;
                            filters.IsPickup = dataObject.IsPickup;
                            filters.Inbound = dataObject.Inbound;
                            filters.Warehouse = dataObject.Warehouse;
                            filters.CarrierName = dataObject.CarrierName;
                            filters.IsDelete = true;
                            filters.BookAMSPickupApptControl = dataObject.BookAMSPickupApptControl ? dataObject.BookAMSPickupApptControl : 0;
                            filters.BookAMSDeliveryApptControl = dataObject.BookAMSDeliveryApptControl ? dataObject.BookAMSDeliveryApptControl : 0;
                            filters.CarrierNumber = dataObject.CarrierNumber; //Added By LVV 8/31/18
                            $.ajax({
                                url: '/api/AMSCarrier/GetModifiedCarrierAvlblAppointments/',
                                contentType: 'application/json; charset=utf-8',
                                dataType: 'json',
                                data: { filter: JSON.stringify(filters) },
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                success: function (data) {
                                    console.log(data.Data);
                                    try {
                                        var blnSuccess = false;
                                        var blnErrorShown = false;
                                        var strValidationMsg = "";
                                        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                            if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Delete Booked Appointments Failure", data.Errors, null); }
                                            else {
                                                if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                    if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                                        blnSuccess = true;
                                                        if (data.Data[0].blnMustRequestAppt != undefined) {
                                                            if (data.Data[0].blnMustRequestAppt) {
                                                                eObject.emailTo = data.Data[0].RequestSendToEmail;
                                                                eObject.emailSubject = data.Data[0].Subject;
                                                                eObject.emailBody = data.Data[0].Body;
                                                                $("#lblEmailMessage").text(data.Data[0].Message ? data.Data[0].Message : "");
                                                                //$("#lblEmailBody").text(data.Data[0].Body);
                                                                $("#lblEmailBody").html(data.Data[0].Body);
                                                                $("#txtEmailComments").val("");

                                                                wndRequestEmail.title(data.Data[0].Subject);
                                                                wndRequestEmail.center().open();
                                                            } else {
                                                                $("#scheduleApptPickupGrid").data("kendoGrid").dataSource.read();
                                                                $("#scheduleapptdeliveryGrid").data("kendoGrid").dataSource.read();
                                                                $("#bookedAppointmnetpickupGrid").data("kendoGrid").dataSource.read();
                                                                $("#bookedAppointmnetdeliveryGrid").data("kendoGrid").dataSource.read();
                                                            }
                                                        } else {
                                                            $("#scheduleApptPickupGrid").data("kendoGrid").dataSource.read();
                                                            $("#scheduleapptdeliveryGrid").data("kendoGrid").dataSource.read();
                                                            $("#bookedAppointmnetpickupGrid").data("kendoGrid").dataSource.read();
                                                            $("#bookedAppointmnetdeliveryGrid").data("kendoGrid").dataSource.read();
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (blnSuccess === false && blnErrorShown === false) {
                                            if (strValidationMsg.length < 1) { strValidationMsg = "Delete Booked Appointments Failure"; }
                                            ngl.showErrMsg("Delete Booked Appointments Failure", strValidationMsg, null);
                                        }
                                    } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                                },
                                error: function (result) { options.error(result); }
                            });
                        };

                        dsBookedAppoinmentDeliveryData = new kendo.data.DataSource({
                            serverSorting: true, 
                            serverPaging: true, 
                            pageSize: 5,
                            transport: {
                                read: function (options) {
                                    var s = new AllFilter();
                                    s.sortName = $("#txtBookedOrdDeliverySortField").val();
                                    s.sortDirection = $("#txtBookedOrdDeliverySortDirection").val();
                                    s.page = options.data.page;
                                    s.skip = options.data.skip;
                                    s.take = options.data.take;
                                    s.filterName = $("#ddlBookedOrdDeliveryFilters").data("kendoDropDownList").value();
                                    switch (s.filterName) {
                                        case "None":
                                            s.filterName = "";
                                            s.filterValue = "";
                                            break;
                                        case "BookDateLoad":
                                            //Modified By LVV on 8/29/2018 -- bug fix #4
                                            s.filterValue = "";
                                            s.filterFrom = $("#txtBookedOrdDeliveryFilterDateVal").val();
                                            s.filterTo = $("#txtBookedOrdDeliveryFilterDateVal").val();
                                            break;
                                        case "BookDateRequired":
                                            //Modified By LVV on 8/29/2018 -- bug fix #4
                                            s.filterValue = "";
                                            s.filterFrom = $("#txtBookedOrdDeliveryFilterDateVal").val();
                                            s.filterTo = $("#txtBookedOrdDeliveryFilterDateVal").val();
                                            break;
                                        case "ScheduledDate":
                                            //Modified By LVV on 8/29/2018 -- bug fix #4
                                            s.filterValue = "";
                                            s.filterFrom = $("#txtBookedOrdDeliveryFilterDateVal").val();
                                            s.filterTo = $("#txtBookedOrdDeliveryFilterDateVal").val();
                                            break;
                                        default:
                                            s.filterValue = $("#txtBookedOrdDeliveryFilterVal").data("kendoMaskedTextBox").value();
                                            break;
                                    }
                                    $.ajax({
                                        url: '/api/AMSCarrier/GetCarrierBookedDelOrders/',
                                        contentType: 'application/json; charset=utf-8',
                                        dataType: 'json',
                                        data: { filter: JSON.stringify(s) },
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                        success: function (data) {
                                            console.log(data);
                                            options.success(data);
                                            try {
                                                var blnSuccess = false;
                                                var blnErrorShown = false;
                                                var strValidationMsg = "";
                                                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                                    if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Get Resources Failure", data.Errors, null); }
                                                    else {
                                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) { blnSuccess = true; }
                                                            else { blnSuccess = true; strValidationMsg = "No records were found matching your search criteria"; }
                                                        }
                                                    }
                                                }
                                                if (blnSuccess === false && blnErrorShown === false) {
                                                    if (strValidationMsg.length < 1) { strValidationMsg = "Get Resources Failure"; }
                                                    ngl.showErrMsg("Get Resources Failure", strValidationMsg, null);
                                                }
                                            } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
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
                                    id: "BookControl",
                                    fields: {
                                        BookControl: { type: "number" },
                                        BookSHID: { type: "string" },
                                        BookConsPrefix: { type: "string" },
                                        BookCarrOrderNumber: { type: "string" },
                                        BookProNumber: { type: "string" },
                                        BookLoadPONumber: { type: "string" },
                                        BookCarrTrailerNo: { type: "string" },
                                        BookDateLoad: { type: "date", editable: false },
                                        BookDateRequired: { type: "date", editable: false },
                                        ScheduledDate: { type: "date", editable: false },
                                        ScheduledTime: { type: "date", editable: false },
                                        BookAMSPickupApptControl: { type: "string" },
                                        BookAMSDeliveryApptControl: { type: "string" },
                                        Warehouse: { type: "string" },
                                        Address1: { type: "string" },
                                        Address2: { type: "string" },
                                        City: { type: "string" },
                                        State: { type: "string" },
                                        Country: { type: "string" },
                                        Zip: { type: "string" },
                                        BookCarrierControl: { type: "string" },
                                        CarrierName: { type: "string" },
                                        CarrierNumber: { type: "string" },
                                        BookShipCarrierProNumber: { type: "string" },
                                        BookShipCarrierNumber: { type: "string" },
                                        BookShipCarrierName: { type: "string" },
                                        BookCarrierContControl: { type: "string" },
                                        BookCarrierContact: { type: "string" },
                                        BookCarrierContactPhone: { type: "string" },
                                        CompNEXTrack: { type: "string" },
                                        BookCustCompControl: { type: "string" },
                                        CompNumber: { type: "string" },
                                        Inbound: { type: "boolean", editable: false },
                                        BookTranCode: { type: "string", editable: false },
                                        BookTotalCases: { type: "string", editable: false },
                                        BookTotalWgt: { type: "string", editable: false },
                                        BookTotalPL: { type: "string", editable: false },
                                        BookTotalCube: { type: "string", editable: false },
                                        BookWhseAuthorizationNo: { type: "string", editable: false },
                                        BookCarrDockPUAssigment: { type: "string", editable: false },
                                        BookCarrDockDelAssignment: { type: "string", editable: false },
                                        BookNotesVisable1: { type: "string", editable: false },
                                        BookNotesVisable2: { type: "string", editable: false },
                                        BookNotesVisable3: { type: "string", editable: false },
                                        LaneNumber: { type: "string", editable: false },
                                        LaneControl: { type: "string", editable: false },
                                        IsTransfer: { type: "boolean", editable: false },
                                        IsPickup: { type: "boolean", editable: false },
                                        BookDestCompControl: { type: "number", editable: false },
                                    }
                                },
                                errors: "Errors"
                            },
                            error: function (xhr, textStatus, error) { ngl.showErrMsg("Access booked Appointmnet delivery Grid Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges(); }
                        });

                        $("#bookedAppointmnetdeliveryGrid").kendoGrid({
                            //noRecords: true,
                            //template: "<p>No data available on current page. Current page is: #=this.dataSource.page()#</p>"
                            noRecords: { template: "<p>No records available.</p>" },
                            toolbar: ["excel"],
                            excel: { fileName: "bookedDel.xlsx", allPages: true },
                            groupable: true,
                            resizable: true,
                            reorderable: true,                          
                            sortable: { mode: "single", allowUnsort: true },                           
                            sort: function (e) {
                                if (!e.sort.dir) { e.sort.dir == ""; e.sort.field == ""; }
                                if (!e.sort.field) { e.sort.field == ""; }
                                $("#txtBookedOrdDeliverySortDirection").val(e.sort.dir);
                                $("#txtBookedOrdDeliverySortField").val(e.sort.field);
                            },
                            pageable: { pageSizes: [5, 10, 15, 20, 25, 50] },
                            //height: 200,
                            //editable: 'inline',
                            dataSource: dsBookedAppoinmentDeliveryData,
                            //dataBound: function (e) {
                            //    var tObj = this;
                            //    tObj.tbody.find("tr[role='row']").each(function () {
                            //        var model = tObj.dataItem(this);
                            //        if (model.IsTransfer == true || model.Inbound == false) {
                            //            $(this).find(".k-grid-edit").addClass("k-state-disabled");
                            //            e.preventDefault();
                            //            $(".k-state-disabled").each(function (index) {
                            //                $(this).removeClass('k-grid-edit')
                            //            });
                            //        }
                            //    });
                            //},
                            columns: [
                                  { command: [{ className: "cm-icononly-button", name: "edit", text: { edit: "", update: "", cancel: "" }, click: ViewAvailApptForBookedApptEdit }, { className: "cm-icononly-button", name: "delete", text: "", iconClass: "k-icon k-i-trash", click: ViewAvailApptForBookedApptDelete }], title: "Action", width: 120 },
                                  { field: "BookControl", hidden: true },
                                  { field: "BookSHID", title: "SHID" },
                                  { field: "BookConsPrefix", title: "CNS" },
                                  { field: "BookCarrOrderNumber", title: "Order No" },
                                  { field: "BookProNumber", title: "PRO", hidden: true },
                                  { field: "BookLoadPONumber", title: "PO", hidden: true },
                                  { field: "BookCarrTrailerNo", title: "Equip ID" },                                                                                                    
                                  { field: "BookAMSPickupApptControl", hidden: true },
                                  { field: "BookAMSDeliveryApptControl", hidden: true },
                                  { field: "Warehouse", title: "Warehouse", width: "110px" },
                                  { field: "Address1", title: "Address 1", width: "125px" },
                                  { field: "Address2", hidden: true },
                                  { field: "City" },
                                  { field: "State", width: "50px" },
                                  { field: "Zip", width: "50px" },
                                  { field: "Country", hidden: true },
                                  { field: "ScheduledDate", title: "Sched Date", format: "{0:M/d/yyyy}" },
                                  { field: "ScheduledTime", title: "Sched Time", format: "{0:hh:mm tt}" },
                                  { field: "BookDateRequired", title: "Req Date", format: "{0:M/d/yyyy}" },
                                  { field: "BookDateLoad", title: "Load Date", format: "{0:M/d/yyyy}", hidden: true },                                 
                                  { field: "BookCarrierControl", hidden: true },
                                  { field: "CarrierName", title: "Carrier Name", hidden: true },
                                  { field: "CarrierNumber", title: "", hidden: true },
                                  { field: "BookShipCarrierProNumber", title: "Assigned Carrier Pro", hidden: true },
                                  { field: "BookShipCarrierNumber", title: "Assigned Carrier No", hidden: true },
                                  { field: "BookShipCarrierName", title: "Assigned Carrier Name", hidden: true },
                                  { field: "BookCarrierContControl", hidden: true },
                                  { field: "BookCarrierContact", hidden: true },
                                  { field: "BookCarrierContactPhone", hidden: true },
                                  { field: "CompNEXTrack", hidden: true },
                                  { field: "BookCustCompControl", title: "Comp Control", hidden: true },
                                  { field: "CompNumber", title: "Comp No", hidden: true },
                                  { field: "Inbound", hidden: true },
                                  { field: "BookTranCode", hidden: true },
                                  { field: "BookTotalCases", hidden: true },
                                  { field: "BookTotalWgt", hidden: true },
                                  { field: "BookTotalPL", hidden: true },
                                  { field: "BookTotalCube", hidden: true },
                                  { field: "BookWhseAuthorizationNo", hidden: true },
                                  { field: "BookCarrDockPUAssigment", hidden: true },
                                  { field: "BookCarrDockDelAssignment", hidden: true },
                                  { field: "BookNotesVisable1", hidden: true },
                                  { field: "BookNotesVisable2", hidden: true },
                                  { field: "BookNotesVisable3", hidden: true },
                                  { field: "LaneNumber", hidden: true },
                                  { field: "LaneControl", hidden: true },
                                  { field: "IsTransfer", hidden: true },
                                  { field: "IsPickup", hidden: true },
                            ],
                        });

                        //popups
                        kendoWindow.height = 'auto';
                        kendoWindow.width = 'auto';
                        wndEditOrdersEqiuipmentID = $("#editOrdersEquipmentID").kendoWindow(kendoWindow).data("kendoWindow");

                        //Carrier Order Summary For "week" and "day" Charts

                        $(document).on("change", "input[name='charttype']", function () {
                            createDaysChart(this.value);
                            createWeeksChart(this.value);
                        });

                        dsAMSOrdersCharts = new kendo.data.DataSource({
                            //pageSize: 20,
                            transport: {
                                read: function (options) {
                                    var cFilter = new AllFilter();
                                    //cFilter.CarrierControlFrom = AMSCarrierControl;
                                    cFilter.take = OrderSummaryDays;
                                    $.ajax({
                                        url: '/api/AMSCarrier/GetCarrierOrderSummaryForChart/',
                                        contentType: 'application/json; charset=utf-8',
                                        dataType: 'json',
                                        data: { filter: JSON.stringify(cFilter) },
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                        success: function (data) {
                                            options.success(data);
                                            console.log(data);
                                            try {
                                                var blnSuccess = false;
                                                var blnErrorShown = false;
                                                var strValidationMsg = "";
                                                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                                    if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Get Carrier Order Summary Failure", data.Errors, null); }
                                                    else {
                                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) { blnSuccess = true; }
                                                            else { blnSuccess = true; strValidationMsg = "No records were found matching your search criteria"; }
                                                        }
                                                    }
                                                }
                                                if (blnSuccess === false && blnErrorShown === false) {
                                                    if (strValidationMsg.length < 1) { strValidationMsg = "Get Carrier Order Summary Failure"; }
                                                    ngl.showErrMsg("Get Carrier Order Summary Failure", strValidationMsg, null);
                                                }
                                            } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
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
                                    id: "OrderDays",
                                    fields: {
                                        OrderDays: { type: "string", editable: false },
                                        pickup: { type: "number", editable: false },
                                        delivery: { type: "number", editable: false },
                                    }
                                },
                                errors: "Errors"
                            },
                            error: function (xhr, textStatus, error) { ngl.showErrMsg("Access Carrier Order Summary Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges(); }
                        });

                        function createDaysChart(type) {
                            $("#daySummaryChart").kendoChart({
                                theme: "blueOpal",
                                dataSource: dsAMSOrdersCharts,
                                autoBind: false,
                                title: { text: "Unscheduled Orders Over 72 Hours", font: "bold 12px Arial, Helvetica, sans-serif", align: "left" },
                                legend: { visible: true },
                                seriesDefaults: { type: type },
                                series: [
                                    {field: "pickup", categoryField: "OrderDays", name: "pickup"}, 
                                    {field: "delivery", categoryField: "OrderDays", name: "delivery"}
                                ],
                                categoryAxis: {
                                    majorGridLines: { visible: false },
                                    max: 3
                                },
                                valueAxis: {
                                    labels: { format: "{0:0}" },
                                },
                                chartArea: { height: 180 },
                                tooltip: { visible: true, format: "{0}", template: "#=category# - #= value #" }
                            });
                        }
                        $(document).ready(createDaysChart("column"));

                        function createWeeksChart(type) {
                            $("#weekSummaryChart").kendoChart({
                                theme: "blueOpal",
                                dataSource: dsAMSOrdersCharts,
                                title: { text: "Future Unscheduled Orders", font: "bold 12px Arial, Helvetica, sans-serif", align: "left" },
                                legend: { visible: true },
                                seriesDefaults: { type: type },
                                series: [
                                    {field: "pickup", categoryField: "OrderDays", name: "pickup"}, 
                                    {field: "delivery", categoryField: "OrderDays", name: "delivery" }
                                ],
                                categoryAxis: {
                                    majorGridLines: { visible: false },
                                    min: 3
                                },
                                valueAxis: {
                                    labels: { format: "{0:0}" },
                                },
                                chartArea: { height: 180 },
                                tooltip: { visible: true, format: "{0}", template: "#=category# - #= value #" }
                            });
                        }
                        $(document).ready(createWeeksChart("column"));
            
                        collapseOrdPickFltr();
                        collapseOrdDelFltr();
                        collapseBookedPickFltr();
                        collapseBookedDelFltr();
                                    
                  //add code above to load screen specific information this is only visible when a user is authenticated
                  }, 10,this);
                }
                setTimeout(function () {var PageReadyJS = <%=PageReadyJS%>}, 10,this);
                setTimeout(function () {   
                    menuTreeHighlightPage(); //must be called after PageReadyJS
                    var divWait = $("#h1Wait");
                    if (typeof (divWait) !== 'undefined' ) { divWait.hide(); }
                }, 10, this);

            });

        </script>

        <style>
                                        
        .hide-display { display: none; }

        .k-grid-header .k-header { overflow: visible !important; white-space: normal !important; }

        fieldset { border-color: #BBDCEB; }

        /*---------------------------------------------*/

        .k-grid tbody tr td { overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }
        
        .k-tooltip{ max-height: 500px; max-width: 450px; overflow-y: auto; }


        .k-grid-selectAppt { min-width: 80px !important; width: 80px !important; }

        .k-grid-bookAppt { min-width: 80px !important; width: 80px !important; }

        .tblResponsive .tblResponsive-top { vertical-align: initial !important; }

        </style>

    </div>
</body>
</html>
