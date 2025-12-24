<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CarrierSchedulerGrouped.aspx.cs" Inherits="DynamicsTMS365.CarrierSchedulerGrouped" %>

<!DOCTYPE html>

<html>
<head>
    <title>DTMS Carrier Scheduler Grouped</title>
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
                                <span style="font-size: small; font-weight: bold;" >Pending Orders Summary</span>
                            </div>
                            <div id="ParHeader" style="margin-top: 15px;">
                                <fieldset>
                                    <legend><b>Pending Orders Summary</b></legend>
                                    <div style="margin-top: 10px;">
                                        <input type="radio" id="barchart" value="column" name="charttype" class="k-radio" checked="checked" /><label class="k-radio-label" for="barchart"><b>Bar chart</b></label>                                    
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
                                            <span id="spExpandUOPick" style="display: none;"><a onclick='expandUOPick();'><span style="font-size: small;font-weight:bold;" class='k-icon k-i-chevron-down'></span></a></span>                        
                                            <span id="spCollapseUOPick" style="display: normal;"><a onclick='collapseUOPick();'><span style="font-size: small;font-weight:bold;" class='k-icon k-i-chevron-up'></span></a></span>                        
                                            <span style="font-size:small; font-weight:bold">Unscheduled Orders - Pick Up</span>&nbsp;&nbsp;<br />                        
                                            <div id="divUOPickFT" style="padding: 10px; width:calc(100% - 20px); height:100%;">                                         
                                                <div class="fast-tab">                                                                   
                                                    <span id="spExpandUOPFltr" style="display: none;"><a onclick='expandUOPFltr();'><span style="font-size: small;font-weight:bold;" class='k-icon k-i-chevron-down'></span></a></span>                                                                    
                                                    <span id="spCollapseUOPFltr" style="display: normal;"><a onclick='collapseUOPFltr();'><span style="font-size: small;font-weight:bold;" class='k-icon k-i-chevron-up'></span></a></span>                                                                   
                                                    <span style="font-size:small; font-weight:bold">Filters</span>&nbsp;&nbsp;<br />                                                                   
                                                    <div id="divUOPFltrFT" style="padding: 10px; width: calc(100% - 20px); height: 100%;">
                                                        <div style="margin-left: 5px;">
                                                            <span>
                                                                <label for="ddlUOPFilters">&nbsp;Filter by:</label><input id="ddlUOPFilters" />
                                                                <span id="spUOPFilterText"><input id="txtUOPFilterVal" /></span>
                                                                <span id="spUOPFilterDate"><input id="txtUOPFilterDateVal" /></span>
                                                                <span id="spUOPFilterButtons"><a id="btnUOPFilter"></a><a id="btnUOPClearFilter"></a></span>
                                                                <input id="txtUOPSortField" type="hidden" /><input id="txtUOPSortDirection" type="hidden" />
                                                            </span>
                                                        </div>
                                                    </div>                                                          
                                                </div>                                   
                                                <div id="UnschedOrderPickGrid"></div>                                                               
                                            </div>                   
                                        </div>

                                        <p></p>
                                                                                                                                                                            
                                        <%--Unscheduled Orders - Delivery--%>                     
                                        <div class="fast-tab">
                                            <span id="spExpandUODel" style="display: none;"><a onclick='expandUODel();'><span style="font-size: small; font-weight: bold;" class='k-icon k-i-chevron-down'></span></a></span>
                                            <span id="spCollapseUODel" style="display: normal;"><a onclick='collapseUODel();'><span style="font-size: small; font-weight: bold;" class='k-icon k-i-chevron-up'></span></a></span>
                                            <span style="font-size: small; font-weight: bold">Unscheduled Orders - Delivery</span>&nbsp;&nbsp;<br />
                                            <div id="divUODelFT" style="padding: 10px; width: calc(100% - 20px); height: 100%;">
                                                <div class="fast-tab">
                                                    <span id="spExpandUODFltr" style="display: none;"><a onclick='expandUODFltr();'><span style="font-size: small; font-weight: bold;" class='k-icon k-i-chevron-down'></span></a></span>
                                                    <span id="spCollapseUODFltr" style="display: normal;"><a onclick='collapseUODFltr();'><span style="font-size: small; font-weight: bold;" class='k-icon k-i-chevron-up'></span></a></span>
                                                    <span style="font-size: small; font-weight: bold">Filters</span>&nbsp;&nbsp;<br />
                                                    <div id="divUODFltrFT" style="padding: 10px; width: calc(100% - 20px); height: 100%;">
                                                        <div style="margin-left: 5px;">
                                                            <span>
                                                                <label for="ddlUODFilters">&nbsp;Filter by:</label><input id="ddlUODFilters" />
                                                                <span id="spUODFilterText"><input id="txtUODFilterVal" /></span>
                                                                <span id="spUODFilterDate"><input id="txtUODFilterDateVal" /></span>
                                                                <span id="spUODFilterButtons"><a id="btnUODFilter"></a><a id="btnUODClearFilter"></a></span>
                                                                <input id="txtUODSortField" type="hidden" /> <input id="txtUODSortDirection" type="hidden" />
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div id="UnschedOrderDelGrid"></div>
                                            </div>
                                        </div>                                                                                                             
                                    </div>
                                </div>
                                <div>
                                    <%--Booked Appointments - Pick Up--%>                                                 
                                    <div class="fast-tab">
                                        <span id="spExpandBAPick" style="display: none;"><a onclick='expandBAPick();'><span style="font-size: small; font-weight: bold;" class='k-icon k-i-chevron-down'></span></a></span>
                                        <span id="spCollapseBAPick" style="display: normal;"><a onclick='collapseBAPick();'><span style="font-size: small; font-weight: bold;" class='k-icon k-i-chevron-up'></span></a></span>
                                        <span style="font-size: small; font-weight: bold">Booked Appointments - Pick Up</span>&nbsp;&nbsp;<br />
                                        <div id="divBAPickFT" style="padding: 10px; width: calc(100% - 20px); height: 100%;">
                                            <div class="fast-tab">
                                                <span id="spExpandBAPFltr" style="display: none;"><a onclick='expandBAPFltr();'><span style="font-size: small; font-weight: bold;" class='k-icon k-i-chevron-down'></span></a></span>
                                                <span id="spCollapseBAPFltr" style="display: normal;"><a onclick='collapseBAPFltr();'><span style="font-size: small; font-weight: bold;" class='k-icon k-i-chevron-up'></span></a></span>
                                                <span style="font-size: small; font-weight: bold">Filters</span>&nbsp;&nbsp;<br />
                                                <div id="divBAPFltrFT" style="padding: 10px; width: calc(100% - 20px); height: 100%;">
                                                    <div style="margin-left: 5px;">
                                                        <span>
                                                            <label for="ddlBAPFilters">&nbsp;Filter by:</label><input id="ddlBAPFilters" />
                                                            <span id="spBAPFilterText"><input id="txtBAPFilterVal" /></span>
                                                            <span id="spBAPFilterDate"><input id="txtBAPFilterDateVal" /></span>
                                                            <span id="spBAPFilterButtons"><a id="btnBAPFilter"></a><a id="btnBAPClearFilter"></a></span>
                                                            <input id="txtBAPSortField" type="hidden" /> <input id="txtBAPSortDirection" type="hidden" />
                                                        </span>
                                                    </div>
                                                </div>
                                            </div>
                                            <div id="BookedApptPickGrid"></div>
                                        </div>
                                    </div>
                                 
                                    <p></p>

                                    <%--Booked Appointments - Delivery--%>                     
                                    <div class="fast-tab">
                                        <span id="spExpandBADel" style="display: none;"><a onclick='expandBADel();'><span style="font-size: small; font-weight: bold;" class='k-icon k-i-chevron-down'></span></a></span>
                                        <span id="spCollapseBADel" style="display: normal;"><a onclick='collapseBADel();'><span style="font-size: small; font-weight: bold;" class='k-icon k-i-chevron-up'></span></a></span>
                                        <span style="font-size: small; font-weight: bold">Booked Appointments - Delivery</span>&nbsp;&nbsp;<br />
                                        <div id="divBADelFT" style="padding: 10px; width: calc(100% - 20px); height: 100%;">
                                            <div class="fast-tab">
                                                <span id="spExpandBADFltr" style="display: none;"><a onclick='expandBADFltr();'><span style="font-size: small; font-weight: bold;" class='k-icon k-i-chevron-down'></span></a></span>
                                                <span id="spCollapseBADFltr" style="display: normal;"><a onclick='collapseBADFltr();'><span style="font-size: small; font-weight: bold;" class='k-icon k-i-chevron-up'></span></a></span>
                                                <span style="font-size: small; font-weight: bold">Filters</span>&nbsp;&nbsp;<br />
                                                <div id="divBADFltrFT" style="padding: 10px; width: calc(100% - 20px); height: 100%;">
                                                    <div style="margin-left: 5px;">
                                                        <span>
                                                            <label for="ddlBADFilters">&nbsp;Filter by:</label><input id="ddlBADFilters" />
                                                            <span id="spBADFilterText"><input id="txtBADFilterVal" /></span>
                                                            <span id="spBADFilterDate"><input id="txtBADFilterDateVal" /></span>
                                                            <span id="spBADFilterButtons"><a id="btnBADFilter"></a><a id="btnBADClearFilter"></a></span>
                                                            <input id="txtBADSortField" type="hidden" /> <input id="txtBADSortDirection" type="hidden" />
                                                        </span>
                                                    </div>
                                                </div>
                                            </div>
                                            <div id="BookedApptDelGrid"></div>
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


        <div id="wndViewCarAvailApptUOGrouped">
            <div>
                <fieldset>
                    <legend><b>Order Details</b></legend>
                    <div style="margin:10px;">
                        <table class="tblResponsive" style="width: 100%;">
                            <tr>
                                <td class="tblResponsive-top" style="width: 20%;"><label><b>SHID: </b></label><label><b id="txtUOSHID"></b></label></td>
                                <td class="tblResponsive-top" style="width: 20%;"><label><b>CNS: </b></label><label><b id="txtUOCNS"></b></label></td>
                                <td class="tblResponsive-top" style="width: 20%;"><label><b>Equip ID: </b></label><label><b id="txtUOEquipID"></b></label></td>
                            </tr>
                            <tr>
                                <td class="tblResponsive-top" style="width: 20%;"><label><b id="lblUOLRDate"></b></label><label><b id="txtUOLRDate"></b></label></td>
                                <td class="tblResponsive-top" style="width: 20%;"><label><b>Scheduled: </b></label><label><b id="txtUOScheduled"></b></label></td>
                            </tr>                           
                        </table>
                    </div>
                </fieldset>
            </div>
            <div style="margin-top: 15px;">
                <fieldset>
                    <legend><b>Available Appointments</b></legend>
                    <div style="margin-top: 15px;" align="center">
                        <div id="AvailableApptsUOGrid"></div>
                    </div>
                </fieldset>
            </div>
        </div>

        <div id="wndEditEquipIDGrouped">
            <div>
                <fieldset>
                    <legend><b>Edit EquipmentID</b></legend>
                    <div style="margin-top: 15px;" align="center">
                        <h2 style="margin: 0; font-size: 1em;" id="lblEditEquipIDMsg"></h2>
                    </div>
                    <div style="margin-top: 15px;" align="center">
                        <button id="btnEditEquipIDSubmit">Save EquipmentID</button>
                    </div>
                </fieldset>
            </div>
        </div>

        <div id="wndViewCarAvailApptBAGrouped">
            <div>
                <fieldset>
                    <legend><b>Order Details</b></legend>
                    <div style="margin:10px;">
                        <table class="tblResponsive" style="width: 100%;">
                            <tr>
                                <td class="tblResponsive-top" style="width: 20%;"><label><b>SHID: </b></label><label><b id="txtBASHID"></b></label></td>
                                <td class="tblResponsive-top" style="width: 20%;"><label><b>CNS: </b></label><label><b id="txtBACNS"></b></label></td>
                                <td class="tblResponsive-top" style="width: 20%;"><label><b>Equip ID: </b></label><label><b id="txtBAEquipID"></b></label></td>                                
                            </tr>
                            <tr>
                                <td class="tblResponsive-top" style="width: 20%;"><label><b id="lblBALRDate"></b></label><label><b id="txtBALRDate"></b></label></td>
                                <td class="tblResponsive-top" style="width: 20%;"><label><b>Scheduled: </b></label><label><b id="txtBAScheduled"></b></label></td>                                                              
                            </tr>
                        </table>
                    </div>
                </fieldset>
            </div>
            <div style="margin-top: 15px;">
                <fieldset>
                    <legend><b>Available Appointments</b></legend>
                    <div style="margin-top: 15px;" align="center">
                        <div id="AvailableApptsBAGrid"></div>
                    </div>
                </fieldset>
            </div>
        </div>

        <div id="wndRequestEmail">
            <div align="center">      
                <h2 style="margin:0; font-size:1em;" id="txtEmailMsg"></h2>  
            </div>  
            <div>        
                <fieldset>          
                    <legend><b>Body</b></legend>           
                    <div align="center">               
                        <h2 style="margin:0; font-size:1em;" id="txtEmailBody"></h2>           
                    </div>      
                </fieldset>   
            </div>  
            <div style="margin-top: 15px;">       
                <fieldset>           
                    <legend><b>Additional Comments</b></legend>          
                    <div style="margin-top: 15px;" align="center">               
                        <textarea id="txtEmailComments" rows="8" cols="20" style="width:300px;"></textarea>            
                    </div>            
                    <div style="margin-top: 15px;" align="center">                
                        <button id="btnSubmitReqEmail">Submit Request</button>          
                    </div>      
                </fieldset>            
            </div>
        </div>

        <script type="text/x-kendo-template" id="AMSCarGroupedDetTemplate">
            <div class="tabstrip">
                <ul><li class="k-active">Orders</li></ul>
                <div><div class="orders"></div></div>
            </div>
        </script>

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

            var dsUnschedOrderPickUp = kendo.data.DataSource;
            var dsUnschedOrderDelivery = kendo.data.DataSource;
            var dsBookedApptPickup = kendo.data.DataSource;
            var dsBookedApptDelivery = kendo.data.DataSource;
            var dsAvailableApptsUO = kendo.data.DataSource;
            var dsAvailableApptsBA = kendo.data.DataSource;
            var wndViewCarAvailApptUOGrouped = kendo.ui.Window;
            var wndRequestEmail = kendo.ui.Window;
            var wndViewCarAvailApptBAGrouped = kendo.ui.Window;
            var wndEditEquipIDGrouped = kendo.ui.Window;


            //************* Action Menu Functions ********************
            function execActionClick(btn, proc){
                if (btn.id == "btnRefresh" ){ refresh(); }
                else if (btn.id == "btnResetCurrentUserConfig"){ resetCurrentUserConfig(PageControl); }
            }

            function refresh(){
                ngl.readDataSource($('#UnschedOrderPickGrid').data('kendoGrid'));
                ngl.readDataSource($('#UnschedOrderDelGrid').data('kendoGrid'));
                ngl.readDataSource($('#BookedApptPickGrid').data('kendoGrid'));
                ngl.readDataSource($('#BookedApptDelGrid').data('kendoGrid'));

                ngl.readDataSource($("#daySummaryChart").data("kendoChart"));
                ngl.readDataSource($("#weekSummaryChart").data("kendoChart"));
            }

            function refreshGridsOnly(){
                ngl.readDataSource($('#UnschedOrderPickGrid').data('kendoGrid'));
                ngl.readDataSource($('#UnschedOrderDelGrid').data('kendoGrid'));
                ngl.readDataSource($('#BookedApptPickGrid').data('kendoGrid'));
                ngl.readDataSource($('#BookedApptDelGrid').data('kendoGrid'));
            }
            

            //************* Fast Tab Functions **************************
            function expandUOPick() { $("#divUOPickFT").show(); $("#spExpandUOPick").hide(); $("#spCollapseUOPick").show(); }
            function collapseUOPick() { $("#divUOPickFT").hide(); $("#spExpandUOPick").show(); $("#spCollapseUOPick").hide(); }
            function expandUOPFltr() { $("#divUOPFltrFT").show(); $("#spExpandUOPFltr").hide(); $("#spCollapseUOPFltr").show(); }
            function collapseUOPFltr() { $("#divUOPFltrFT").hide(); $("#spExpandUOPFltr").show(); $("#spCollapseUOPFltr").hide(); }

            function expandUODel() { $("#divUODelFT").show(); $("#spExpandUODel").hide(); $("#spCollapseUODel").show(); }
            function collapseUODel() { $("#divUODelFT").hide(); $("#spExpandUODel").show(); $("#spCollapseUODel").hide(); }
            function expandUODFltr() { $("#divUODFltrFT").show(); $("#spExpandUODFltr").hide(); $("#spCollapseUODFltr").show(); }
            function collapseUODFltr() { $("#divUODFltrFT").hide(); $("#spExpandUODFltr").show(); $("#spCollapseUODFltr").hide(); }

            function expandBAPick() { $("#divBAPickFT").show(); $("#spExpandBAPick").hide(); $("#spCollapseBAPick").show(); }
            function collapseBAPick() { $("#divBAPickFT").hide(); $("#spExpandBAPick").show(); $("#spCollapseBAPick").hide(); }
            function expandBAPFltr() { $("#divBAPFltrFT").show(); $("#spExpandBAPFltr").hide(); $("#spCollapseBAPFltr").show(); }
            function collapseBAPFltr() { $("#divBAPFltrFT").hide(); $("#spExpandBAPFltr").show(); $("#spCollapseBAPFltr").hide(); }

            function expandBADel() { $("#divBADelFT").show(); $("#spExpandBADel").hide(); $("#spCollapseBADel").show(); }
            function collapseBADel() { $("#divBADelFT").hide(); $("#spExpandBADel").show(); $("#spCollapseBADel").hide(); }
            function expandBADFltr() { $("#divBADFltrFT").show(); $("#spExpandBADFltr").hide(); $("#spCollapseBADFltr").show(); }
            function collapseBADFltr() { $("#divBADFltrFT").hide(); $("#spExpandBADFltr").show(); $("#spCollapseBADFltr").hide(); }


            //************* Page Functions **************************
            function updateEquipID(oData) {
                $.ajax({
                    type: "POST",
                    url: 'api/CarrierSchedulerGrouped/SaveEquipmentIDForOrderGrouped',
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
                                if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Save Equipment ID Failure", data.Errors, null); }
                                else {
                                    if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                        if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                            blnSuccess = true;
                                            if (!data.Data[0].Success) {
                                                if (data.Data[0].IsAdd) {
                                                    $("#lblEditEquipIDMsg").text(data.Data[0].ErrMsg);
                                                    $("#btnEditEquipIDSubmit").show();
                                                } else {
                                                    $("#lblEditEquipIDMsg").text(data.Data[0].ErrMsg);
                                                    $("#btnEditEquipIDSubmit").hide();                                                    
                                                    refreshGridsOnly(); //refresh the grids
                                                }
                                                EquipIDValidation = data.Data[0];
                                                wndEditEquipIDGrouped.center().open();
                                            } else { refreshGridsOnly(); } //refresh the grids
                                        }
                                    }
                                }
                            }
                            if (blnSuccess === false && blnErrorShown === false) {
                                if (strValidationMsg.length < 1) { strValidationMsg = "Save Equipment ID Failure"; }
                                ngl.showErrMsg("Save Equipment ID Failure", strValidationMsg, null);
                            }
                        } catch (err) { ngl.showErrMsg(err.name, err.description, null); }                         
                    },
                    error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Equipment ID Failure"); ngl.showErrMsg("Save Equipment ID Failure", sMsg, null); }
                });
            }

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
                                if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                    blnErrorShown = true;
                                    ngl.showErrMsg("Sent Request Email Failure", data.Errors, null);
                                }
                                else {
                                    if (data.StatusCode == 200) {
                                        blnSuccess = true;
                                    }
                                }
                            }
                            if (blnSuccess === false && blnErrorShown === false) {
                                if (strValidationMsg.length < 1) { strValidationMsg = "Sent Request Email Failure"; }
                                ngl.showErrMsg("Sent Request Email Failure", strValidationMsg, null);
                            }
                        } catch (err) {
                            ngl.showErrMsg(err.name, err.description, null);
                        }
                    },
                    error: function (xhr, textStatus, error) {
                        var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Sent Request Email Failure");
                        ngl.showErrMsg("Sent Request Email Failure", sMsg, null);
                    }
                });
            }


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
                                open: {
                                    effects: "fadeIn"
                                }
                            }
                        });

                        //*********** Filter Data **************//
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

                        //************* Unscheduled Orders Pickup Filters ***************//
                        $("#txtUOPFilterVal").kendoMaskedTextBox();
                        $("#txtUOPFilterDateVal").kendoDatePicker();
                        $("#ddlUOPFilters").kendoDropDownList({
                            dataTextField: "text",
                            dataValueField: "value",
                            placeholder: "select",
                            dataSource: PickGridFilterData,
                            select: function (e) {
                                var name = e.dataItem.text;
                                var val = e.dataItem.value;
                                $("#txtUOPFilterVal").data("kendoMaskedTextBox").value("");
                                $("#txtUOPFilterDateVal").data("kendoDatePicker").value("");
                                switch (val) {
                                    case "None":
                                        $("#spUOPFilterText").hide();
                                        $("#spUOPFilterDate").hide();
                                        $("#spUOPFilterButtons").hide();
                                        break;
                                    case "BookDateLoad":
                                        $("#spUOPFilterText").hide();
                                        $("#spUOPFilterDate").show();
                                        $("#spUOPFilterButtons").show();
                                        break;
                                    case "BookDateRequired":
                                        $("#spUOPFilterText").hide();
                                        $("#spUOPFilterDate").show();
                                        $("#spUOPFilterButtons").show();
                                        break;
                                    case "ScheduledDate":
                                        $("#spUOPFilterText").hide();
                                        $("#spUOPFilterDate").show();
                                        $("#spUOPFilterButtons").show();
                                        break;
                                    default:
                                        $("#spUOPFilterText").show();
                                        $("#spUOPFilterDate").hide();
                                        $("#spUOPFilterButtons").show();
                                        break;
                                }
                            }
                        });
                        $("#btnUOPFilter").kendoButton({ icon: "filter", click: function(e){ $("#UnschedOrderPickGrid").data("kendoGrid").dataSource.read(); } });
                        $("#btnUOPClearFilter").kendoButton({
                            icon: "filter-clear",
                            click: function (e) {
                                var dropdownlist = $("#ddlUOPFilters").data("kendoDropDownList");
                                dropdownlist.select(0);
                                dropdownlist.trigger("change");
                                $("#txtUOPFilterVal").data("kendoMaskedTextBox").value("");
                                $("#txtUOPFilterDateVal").data("kendoDatePicker").value("");
                                $("#spUOPFilterText").hide();
                                $("#spUOPFilterDate").hide();
                                $("#spUOPFilterButtons").hide();
                                $("#UnschedOrderPickGrid").data("kendoGrid").dataSource.read();
                            }
                        });
                        $("#spUOPFilterText").hide();
                        $("#spUOPFilterDate").hide();
                        $("#spUOPFilterButtons").hide();

                        //************* Unscheduled Orders Delivery Filters ***************//
                        $("#txtUODFilterVal").kendoMaskedTextBox();
                        $("#txtUODFilterDateVal").kendoDatePicker();
                        $("#ddlUODFilters").kendoDropDownList({
                            dataTextField: "text",
                            dataValueField: "value",
                            placeholder: "select",
                            dataSource: DelGridFilterData,
                            select: function (e) {
                                var name = e.dataItem.text;
                                var val = e.dataItem.value;
                                $("#txtUODFilterVal").data("kendoMaskedTextBox").value("");
                                $("#txtUODFilterDateVal").data("kendoDatePicker").value("");
                                switch (val) {
                                    case "None":
                                        $("#spUODFilterText").hide();
                                        $("#spUODFilterDate").hide();
                                        $("#spUODFilterButtons").hide();
                                        break;
                                    case "BookDateRequired":
                                        $("#spUODFilterText").hide();
                                        $("#spUODFilterDate").show();
                                        $("#spUODFilterButtons").show();
                                        break;
                                    case "ScheduledDate":
                                        $("#spUODFilterText").hide();
                                        $("#spUODFilterDate").show();
                                        $("#spUODFilterButtons").show();
                                        break;
                                    default:
                                        $("#spUODFilterText").show();
                                        $("#spUODFilterDate").hide();
                                        $("#spUODFilterButtons").show();
                                        break;
                                }
                            }
                        });
                        $("#btnUODFilter").kendoButton({ icon: "filter", click: function(e){ $("#UnschedOrderDelGrid").data("kendoGrid").dataSource.read(); } });
                        $("#btnUODClearFilter").kendoButton({
                            icon: "filter-clear",
                            click: function (e) {
                                var dropdownlist = $("#ddlUODFilters").data("kendoDropDownList");
                                dropdownlist.select(0);
                                dropdownlist.trigger("change");
                                $("#txtUODFilterVal").data("kendoMaskedTextBox").value("");
                                $("#txtUODFilterDateVal").data("kendoDatePicker").value("");
                                $("#spUODFilterText").hide();
                                $("#spUODFilterDate").hide();
                                $("#spUODFilterButtons").hide();
                                $("#UnschedOrderDelGrid").data("kendoGrid").dataSource.read();
                            }
                        });
                        $("#spUODFilterText").hide();
                        $("#spUODFilterDate").hide();
                        $("#spUODFilterButtons").hide();

                        //************* Booked Appointment Pickup Filters ***************//
                        $("#txtBAPFilterVal").kendoMaskedTextBox();
                        $("#txtBAPFilterDateVal").kendoDatePicker();
                        $("#ddlBAPFilters").kendoDropDownList({
                            dataTextField: "text",
                            dataValueField: "value",
                            placeholder: "select",
                            dataSource: PickGridFilterData,
                            select: function (e) {
                                var name = e.dataItem.text;
                                var val = e.dataItem.value;
                                $("#txtBAPFilterVal").data("kendoMaskedTextBox").value("");
                                $("#txtBAPFilterDateVal").data("kendoDatePicker").value("");
                                switch (val) {
                                    case "None":
                                        $("#spBAPFilterText").hide();
                                        $("#spBAPFilterDate").hide();
                                        $("#spBAPFilterButtons").hide();
                                        break;
                                    case "BookDateLoad":
                                        $("#spBAPFilterText").hide();
                                        $("#spBAPFilterDate").show();
                                        $("#spBAPFilterButtons").show();
                                        break;
                                    case "ScheduledDate":
                                        $("#spBAPFilterText").hide();
                                        $("#spBAPFilterDate").show();
                                        $("#spBAPFilterButtons").show();
                                        break;
                                    default:
                                        $("#spBAPFilterText").show();
                                        $("#spBAPFilterDate").hide();
                                        $("#spBAPFilterButtons").show();
                                        break;
                                }
                            }
                        });
                        $("#btnBAPFilter").kendoButton({ icon: "filter", click: function(e){ $("#BookedApptPickGrid").data("kendoGrid").dataSource.read(); } });
                        $("#btnBAPClearFilter").kendoButton({
                            icon: "filter-clear",
                            click: function (e) {
                                var dropdownlist = $("#ddlBAPFilters").data("kendoDropDownList");
                                dropdownlist.select(0);
                                dropdownlist.trigger("change");
                                $("#txtBAPFilterVal").data("kendoMaskedTextBox").value("");
                                $("#txtBAPFilterDateVal").data("kendoDatePicker").value("");
                                $("#spBAPFilterText").hide();
                                $("#spBAPFilterDate").hide();
                                $("#spBAPFilterButtons").hide();
                                $("#BookedApptPickGrid").data("kendoGrid").dataSource.read();
                            }
                        });
                        $("#spBAPFilterText").hide();
                        $("#spBAPFilterDate").hide();
                        $("#spBAPFilterButtons").hide();

                        //************* Booked Appointment Delivery Filters ***************//
                        $("#txtBADFilterVal").kendoMaskedTextBox();
                        $("#txtBADFilterDateVal").kendoDatePicker();
                        $("#ddlBADFilters").kendoDropDownList({
                            dataTextField: "text",
                            dataValueField: "value",
                            placeholder: "select",
                            dataSource: DelGridFilterData,
                            select: function (e) {
                                var name = e.dataItem.text;
                                var val = e.dataItem.value;
                                $("#txtBADFilterVal").data("kendoMaskedTextBox").value("");
                                $("#txtBADFilterDateVal").data("kendoDatePicker").value("");
                                switch (val) {
                                    case "None":
                                        $("#spBADFilterText").hide();
                                        $("#spBADFilterDate").hide();
                                        $("#spBADFilterButtons").hide();
                                        break;
                                    case "BookDateRequired":
                                        $("#spBADFilterText").hide();
                                        $("#spBADFilterDate").show();
                                        $("#spBADFilterButtons").show();
                                        break;
                                    case "ScheduledDate":
                                        $("#spBADFilterText").hide();
                                        $("#spBADFilterDate").show();
                                        $("#spBADFilterButtons").show();
                                        break;
                                    default:
                                        $("#spBADFilterText").show();
                                        $("#spBADFilterDate").hide();
                                        $("#spBADFilterButtons").show();
                                        break;
                                }
                            }
                        });
                        $("#btnBADFilter").kendoButton({ icon: "filter", click: function(e){ $("#BookedApptDelGrid").data("kendoGrid").dataSource.read(); } });
                        $("#btnBADClearFilter").kendoButton({
                            icon: "filter-clear",
                            click: function (e) {
                                var dropdownlist = $("#ddlBADFilters").data("kendoDropDownList");
                                dropdownlist.select(0);
                                dropdownlist.trigger("change");
                                $("#txtBADFilterVal").data("kendoMaskedTextBox").value("");
                                $("#txtBADFilterDateVal").data("kendoDatePicker").value("");
                                $("#spBADFilterText").hide();
                                $("#spBADFilterDate").hide();
                                $("#spBADFilterButtons").hide();
                                $("#BookedApptDelGrid").data("kendoGrid").dataSource.read();
                            }
                        });
                        $("#spBADFilterText").hide();
                        $("#spBADFilterDate").hide();
                        $("#spBADFilterButtons").hide();


                        //************* Edit EquipID ***************//
                        var EditEquipIDOrder = {};
                        var EquipIDValidation = {};
                        $("#btnEditEquipIDSubmit").kendoButton({
                            icon: 'save',
                            click: function () {
                                var oData = { "order": EditEquipIDOrder, "equipID": EquipIDValidation };
                                updateEquipID(oData);
                                wndEditEquipIDGrouped.close();
                            }
                        });


                        //************* Email ***************//
                        var eObject = new EmailObject();               
                        $("#txtEmailComments").kendoMaskedTextBox();
                        $("#btnSubmitReqEmail").kendoButton({
                            icon: "email",
                            click: function () {
                                eObject.emailFrom = fromEmail;
                                eObject.emailCc = "";
                                eObject.emailBcc = "";
                                eObject.emailBody = eObject.emailBody + $("#txtEmailComments").val();
                                sendReqEmail(eObject)
                                wndRequestEmail.close();
                            }
                        });



                        //********** AvailableApptsUOGrid **********//
                        var UODataItem = {};
                        var blnUOIsPickup;
                        dsAvailableApptsUO = new kendo.data.DataSource({
                            pageSize: 10,
                            transport: {
                                read: function (options) {
                                    var whseControl = 0;
                                    var dt = null;
                                    if(blnUOIsPickup) { whseControl = UODataItem.BookOrigCompControl; dt = UODataItem.BookDateLoad; }else{ whseControl = UODataItem.BookDestCompControl; dt = UODataItem.BookDateRequired; }
                                    
                                    var gr = new GenericResult();                                   
                                    gr.blnField = blnUOIsPickup;
                                    gr.strField = UODataItem.BookSHID;                                   
                                    gr.strField2 = UODataItem.Warehouse;
                                    gr.intField1 = whseControl;
                                    gr.dtField = ngl.formatDate(dt, '', 't'); //dt;
                                    $.ajax({
                                        url: 'api/CarrierSchedulerGrouped/GetCarAvailableApptsUOGrouped',
                                        contentType: 'application/json; charset=utf-8',
                                        dataType: 'json',
                                        data: { filter: JSON.stringify(gr) },
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                        success: function (data) {
                                            options.success(data);
                                            console.log(data.Data);
                                            try {
                                                var blnSuccess = false;
                                                var blnErrorShown = false;
                                                var strValidationMsg = "";
                                                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                                    if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Get Available Appointments UO Failure", data.Errors, null); }
                                                    else {
                                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                                                blnSuccess = true;
                                                                if (data.Data[0].Message != undefined) {
                                                                    eObject.emailTo = data.Data[0].RequestSendToEmail;
                                                                    eObject.emailSubject = data.Data[0].Subject;
                                                                    eObject.emailBody = data.Data[0].Body;
                                                                    $("#txtEmailMsg").text(data.Data[0].Message?data.Data[0].Message:"");
                                                                    $("#txtEmailBody").html(data.Data[0].Body);
                                                                    $("#txtEmailComments").val("");
                                                                    wndRequestEmail.title(data.Data[0].Subject);
                                                                    wndRequestEmail.center().open();
                                                                } else {
                                                                    wndViewCarAvailApptUOGrouped.title("Carrier Scheduling - Select Appointment");
                                                                    wndViewCarAvailApptUOGrouped.center().open();
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                if (blnSuccess === false && blnErrorShown === false) {
                                                    if (strValidationMsg.length < 1) { strValidationMsg = "Get Available Appointments UO Failure"; }
                                                    ngl.showErrMsg("Get Available Appointments UO Failure", strValidationMsg, null);
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
                            error: function (xhr, textStatus, error) { ngl.showErrMsg("Access Available Appointment UO Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges(); }
                        });

                        $("#AvailableApptsUOGrid").kendoGrid({
                            noRecords: { template: "<p>No records available.</p>" },
                            autoBind: false,
                            height: 200,
                            dataSource: dsAvailableApptsUO,
                            pageable: true,
                            columns: [
                                { command: [{ name: "bookAppt", text: "Book Appt", width: 100, click: BookAppointmentUO }], title: "Action" },
                                { field: "Warehouse", title: "Warehouse", template: "<span title='${Warehouse}'>${Warehouse}</span>" },
                                { field: "Date", title: "Date", format: "{0:M/d/yyyy}" },
                                { field: "StartTime", title: "Start Time", template: "#= kendo.toString(kendo.parseDate(StartTime, 'HH:mm'), 'HH:mm') #" }, //format: "{0:HH:mm}" //Modified by LVV on 11/1/2019
                                { field: "EndTime", title: "End Time", hidden: true },
                                { field: "Docks", title: "Docks", hidden: true },
                                { field: "Books", title: "Books", hidden: true },
                                { field: "CarrierControl", title: "CarrierControl", hidden: true },
                                { field: "CarrierNumber", title: "CarrierNumber", hidden: true },
                                { field: "CarrierName", title: "CarrierName", hidden: true },
                                { field: "CompControl", title: "CompControl", hidden: true },
                            ],
                        });

                        function BookAppointmentUO(e) {
                            var BookApptObject = this.dataItem($(e.currentTarget).closest("tr"));                                   
                            //Modified By LVV on 8/27/2018 for v-8.3 Scheduler - Added code to fix the problem with the dates being converted from different time zones
                            //Modified by RHR for v-8.2 on 09/19/2018 we now use ngl.convertDateForWindows to avoid non-ascii characters in date string
                            BookApptObject.EndTime = BookApptObject.EndTime;
                            BookApptObject.StartTime = ngl.convertTimePickerToDateString(BookApptObject.StartTime, ngl.convertDateForWindows(BookApptObject.StartTime, ""), "");
                            console.log(BookApptObject);
                            $.ajax({
                                url: "api/CarrierSchedulerGrouped/CarrierScheduleApptForUOGrouped",
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
                                            if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Schedule Appointment Failure", data.Errors, null); }
                                            else {
                                                if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                    if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                                        blnSuccess = true;
                                                        if (data.Data[0] == false) { ngl.showWarningMsg("Schedule Appointment Failure!", "", null); } 
                                                        else { wndViewCarAvailApptUOGrouped.close(); ngl.showSuccessMsg("Book Carrier Appointment Sucess"); refresh(); } //refresh Grid
                                                    }
                                                }
                                            }
                                        }
                                        if (blnSuccess === false && blnErrorShown === false) {
                                            if (strValidationMsg.length < 1) { strValidationMsg = "Schedule Appointment Failure"; }
                                            ngl.showErrMsg("Schedule Appointment Failure", strValidationMsg, null);
                                        }
                                    } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                                },
                                error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Schedule Appointment Failure"); ngl.showErrMsg("Schedule Appointment Failure", sMsg, null); }
                            });
                        }
                 
                        //************* UnschedOrderPickGrid ***************//
                        dsUnschedOrderPickUp = new kendo.data.DataSource({                          
                            serverSorting: true, 
                            serverPaging: true, 
                            pageSize: 5,
                            transport: {
                                read: function (options) {
                                    var s = new AllFilter();
                                    s.sortName = $("#txtUOPSortField").val();
                                    s.sortDirection = $("#txtUOPSortDirection").val();
                                    s.page = options.data.page;
                                    s.skip = options.data.skip;
                                    s.take = options.data.take;                                  
                                    s.filterName = $("#ddlUOPFilters").data("kendoDropDownList").value();
                                    switch (s.filterName) {
                                        case "None":
                                            s.filterName = "";
                                            s.filterValue = "";
                                            break;
                                        case "BookDateLoad":
                                            s.filterValue = "";
                                            s.filterFrom = $("#txtUOPFilterDateVal").val();
                                            s.filterTo = $("#txtUOPFilterDateVal").val();
                                            break;
                                        case "ScheduledDate":
                                            s.filterValue = "";
                                            s.filterFrom = $("#txtUOPFilterDateVal").val();
                                            s.filterTo = $("#txtUOPFilterDateVal").val();
                                            break;
                                        default:
                                            s.filterValue = $("#txtUOPFilterVal").data("kendoMaskedTextBox").value();
                                            break;
                                    }
                                    $.ajax({
                                        url: 'api/CarrierSchedulerGrouped/GetAMSCarrierUOPickGrouped',
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
                                                    if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Get Unscheduled Orders Pickup Failure", data.Errors, null); }
                                                    else {
                                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) { blnSuccess = true; }
                                                            else { blnSuccess = true; strValidationMsg = "No records were found matching your search criteria"; }
                                                        }
                                                    }
                                                }
                                                if (blnSuccess === false && blnErrorShown === false) {
                                                    if (strValidationMsg.length < 1) { strValidationMsg = "Get Unscheduled Orders Pickup Failure"; }
                                                    ngl.showErrMsg("Get Unscheduled Orders Pickup Failure", strValidationMsg, null);
                                                }
                                            } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                                        },
                                        error: function (result) { options.error(result); }
                                    });
                                },
                                update: function (options) {
                                    var vData = {};
                                    vData.ApptControl = 0;
                                    vData.EquipID = options.data.BookCarrTrailerNo;
                                    vData.IsPickup = true;
                                    vData.Success = false;
                                    vData.IsAdd = false;
                                    vData.ErrMsg = "";

                                    EquipIDValidation = vData;
                                    EditEquipIDOrder = options.data.BookSHID;

                                    var oData = { "SHID": EditEquipIDOrder, "equipIDValidation": EquipIDValidation };
                                    updateEquipID(oData);
                                },
                                parameterMap: function (options, operation) { return options; }
                            },
                            schema: {
                                data: "Data",
                                total: "Count",
                                model: {
                                    id: "BookSHID",
                                    fields: {
                                        BookSHID: { type: "string", editable: false },
                                        BookConsPrefix: { type: "string", editable: false },
                                        BookCarrTrailerNo: { type: "string" },
                                        BookDateLoad: { type: "date", editable: false },
                                        ScheduledDate: { type: "date", editable: false },
                                        ScheduledTime: { type: "date", editable: false },
                                        Warehouse: { type: "string", editable: false },
                                        Address1: { type: "string", editable: false },
                                        Address2: { type: "string", editable: false },
                                        City: { type: "string", editable: false },
                                        State: { type: "string", editable: false },
                                        Zip: { type: "string", editable: false },
                                        Country: { type: "string", editable: false },
                                        BookCarrierControl: { type: "string", editable: false },
                                        CarrierName: { type: "string", editable: false },
                                        CarrierNumber: { type: "string", editable: false },
                                        BookOrigCompControl: { type: "number", editable: false }                                        
                                    }
                                },
                                errors: "Errors"
                            },
                            error: function (xhr, textStatus, error) { ngl.showErrMsg("Access Unscheduled Orders Pickup Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges(); }
                        });

                        $("#UnschedOrderPickGrid").kendoGrid({         
                            noRecords: { template: "<p>No records available.</p>" },
                            toolbar: ["excel"],
                            excel: { fileName: "unschedPickGrouped.xlsx", allPages: true },
                            groupable: true,
                            resizable: true,
                            reorderable: true,                          
                            sortable: { mode: "single", allowUnsort: true },                           
                            sort: function (e) {
                                if (!e.sort.dir) { e.sort.dir == ""; e.sort.field == ""; }
                                if (!e.sort.field) { e.sort.field == ""; }
                                $("#txtUOPSortDirection").val(e.sort.dir);
                                $("#txtUOPSortField").val(e.sort.field);
                            },
                            pageable: { pageSizes: [5, 10, 15, 20, 25, 50] },
                            dataSource: dsUnschedOrderPickUp,
                            editable: "inline",
                            columns: [
                                { command: [{ className: "cm-icononly-button", name: "edit", text: { edit: "", update: "", cancel: "" }, iconClass: "k-icon k-i-pencil" }, { name: "selectAppt", text: "Select Appt", click: ViewUOPickAvailAppt }], title: "Action", width: 150 },
                                { field: "BookSHID", title: "SHID", template: "<span title='${BookSHID}'>${BookSHID}</span>" },
                                { field: "BookConsPrefix", title: "CNS", template: "<span title='${BookConsPrefix}'>${BookConsPrefix}</span>", hidden: true },
                                { field: "BookCarrTrailerNo", title: "Equip ID", template: "<span title='${BookCarrTrailerNo}'>${BookCarrTrailerNo}</span>" },
                                { field: "Warehouse", title: "Warehouse", width: 110, template: "<span title='${Warehouse}'>${Warehouse}</span>"},
                                { field: "Address1", title: "Address 1", width: 125, template: "<span title='${Address1}'>${Address1}</span>" },
                                { field: "Address2", hidden: true },
                                { field: "City", template: "<span title='${City}'>${City}</span>" },
                                { field: "State", width: 50 },
                                { field: "Zip", width: 50 },
                                { field: "Country", hidden: true },
                                { field: "BookCarrierControl", hidden: true },
                                { field: "CarrierName", title: "Carrier Name", hidden: true },
                                { field: "CarrierNumber", title: "", hidden: true },
                                { field: "ScheduledDate", title: "Sched Date", format: "{0:M/d/yyyy}" },
                                { field: "ScheduledTime", title: "Sched Time", template: "#= kendo.toString(kendo.parseDate(ScheduledTime, 'HH:mm'), 'HH:mm') #" }, //format: "{0:hh:mm tt}" //Modified by LVV on 11/1/2019
                                { field: "BookDateLoad", title: "Load Date", format: "{0:M/d/yyyy}" },
                                { field: "BookOrigCompControl", title: "", hidden: true },
                            ],
                            detailTemplate: kendo.template($("#AMSCarGroupedDetTemplate").html()),
                            detailInit: function(e) {
                                var detailRow = e.detailRow;
                                detailRow.find(".tabstrip").kendoTabStrip({
                                    animation: {
                                        open: { effects: "fadeIn" }
                                    }
                                });
                                detailRow.find(".orders").kendoGrid({
                                    dataSource: {
                                        serverPaging: false, serverSorting: false, serverFiltering: false,
                                        transport: {
                                            read: {
                                                url: "api/CarrierSchedulerGrouped/GetUOPickDetailsGrouped", 
                                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                                type: "GET",
                                                data: function() {
                                                    var gr = new GenericResult();
                                                    gr.strField = e.data.BookSHID;
                                                    gr.strField2 = e.data.Warehouse;
                                                    gr.intField1 = e.data.BookOrigCompControl;
                                                    gr.dtField = ngl.formatDate(e.data.BookDateLoad, '', 't'); //e.data.BookDateLoad;
                                                    return { filter: JSON.stringify(gr) };
                                                }
                                            },
                                            parameterMap: function (options, operation) { return options; }
                                        },                           
                                        schema: {
                                            data: "Data",
                                            total: "Count",
                                            model: {
                                                id: "BookSHID",
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
                                                    IsPickup: { type: "boolean", editable: false }
                                                }
                                            },
                                            errors: "Errors"
                                        },
                                        error: function (xhr, textStatus, error) { ngl.showErrMsg("Access Unscheduled Orders Pickup Detail Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges(); }                                       
                                    },
                                    resizeable: true, scrollable: true, sortable: false, pageable: false,
                                    columns: [
                                        //{ field: "BookControl", hidden: true },
                                        //{ field: "BookSHID", title: "SHID", hidden: true },
                                        { field: "BookConsPrefix", title: "CNS", template: "<span title='${BookConsPrefix}'>${BookConsPrefix}</span>" },
                                        { field: "BookCarrOrderNumber", title: "Order No", template: "<span title='${BookCarrOrderNumber}'>${BookCarrOrderNumber}</span>" },
                                        { field: "BookProNumber", title: "PRO", template: "<span title='${BookProNumber}'>${BookProNumber}</span>" },
                                        { field: "BookLoadPONumber", title: "PO", template: "<span title='${BookLoadPONumber}'>${BookLoadPONumber}</span>" },
                                        { field: "BookCarrTrailerNo", title: "Equip ID", template: "<span title='${BookCarrTrailerNo}'>${BookCarrTrailerNo}</span>" },                                                             
                                        { field: "ScheduledDate", title: "Sched Date", format: "{0:M/d/yyyy}" },
                                        { field: "ScheduledTime", title: "Sched Time", template: "#= kendo.toString(kendo.parseDate(ScheduledTime, 'HH:mm'), 'HH:mm') #" }, //Modified by LVV on 11/1/2019
                                        { field: "BookDateLoad", title: "Load Date", format: "{0:M/d/yyyy}" },
                                        { field: "Inbound", hidden: true },
                                        { field: "BookTotalCases", hidden: true },
                                        { field: "BookTotalWgt", hidden: true },
                                        { field: "BookTotalPL", hidden: true },
                                        { field: "BookTotalCube", hidden: true },
                                        { field: "BookNotesVisable1", template: "<span title='${BookNotesVisable1}'>${BookNotesVisable1}</span>", hidden: true },
                                        { field: "BookNotesVisable2", template: "<span title='${BookNotesVisable2}'>${BookNotesVisable2}</span>", hidden: true },
                                        { field: "BookNotesVisable3", template: "<span title='${BookNotesVisable3}'>${BookNotesVisable3}</span>", hidden: true }                                        
                                    ],
                                });                   
                            }
                        });

                        function ViewUOPickAvailAppt(e) {
                            UODataItem = this.dataItem($(e.currentTarget).closest("tr"));                            
                 
                            $("#txtUOEquipID").text(UODataItem.BookCarrTrailerNo ? UODataItem.BookCarrTrailerNo : "");
                            //This is set to Load Date on Pickup
                            $("#lblUOLRDate").text("Load Date: ");
                            $("#txtUOLRDate").text(kendo.toString(UODataItem.BookDateLoad, 'M/d/yyyy'));
                            $("#txtUOScheduled").text(kendo.toString(UODataItem.ScheduledDate ? UODataItem.ScheduledDate : "", 'M/d/yyyy') + " " + kendo.toString(UODataItem.ScheduledTime ? UODataItem.ScheduledTime : "", 'HH:mm'));
                            $("#txtUOSHID").text(UODataItem.BookSHID);
                            $("#txtUOCNS").text(UODataItem.BookConsPrefix);

                            blnUOIsPickup = true;

                            $("#AvailableApptsUOGrid").data("kendoGrid").dataSource.data([]);
                            $("#AvailableApptsUOGrid").data("kendoGrid").dataSource.read();
                        }
                                   
                        //************* UnschedOrderDelGrid ***************//
                        dsUnschedOrderDelivery = new kendo.data.DataSource({
                            serverSorting: true, 
                            serverPaging: true, 
                            pageSize: 5,
                            transport: {
                                read: function (options) {
                                    var s = new AllFilter();
                                    s.sortName = $("#txtUODSortField").val();
                                    s.sortDirection = $("#txtUODSortDirection").val();
                                    s.page = options.data.page;
                                    s.skip = options.data.skip;
                                    s.take = options.data.take;
                                    s.filterName = $("#ddlUODFilters").data("kendoDropDownList").value();
                                    switch (s.filterName) {
                                        case "None":
                                            s.filterName = "";
                                            s.filterValue = "";
                                            break;
                                        case "BookDateRequired":
                                            s.filterValue = "";
                                            s.filterFrom = $("#txtUODFilterDateVal").val();
                                            s.filterTo = $("#txtUODFilterDateVal").val();
                                            break;
                                        case "ScheduledDate":
                                            s.filterValue = "";
                                            s.filterFrom = $("#txtUODFilterDateVal").val();
                                            s.filterTo = $("#txtUODFilterDateVal").val();
                                            break;
                                        default:
                                            s.filterValue = $("#txtUODFilterVal").data("kendoMaskedTextBox").value();
                                            break;
                                    }
                                    $.ajax({
                                        url: 'api/CarrierSchedulerGrouped/GetAMSCarrierUODelGrouped',
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
                                                    if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Get Unscheduled Orders Delivery Failure", data.Errors, null); }
                                                    else {
                                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {  blnSuccess = true; }
                                                            else { blnSuccess = true; strValidationMsg = "No records were found matching your search criteria"; }
                                                        }
                                                    }
                                                }
                                                if (blnSuccess === false && blnErrorShown === false) {
                                                    if (strValidationMsg.length < 1) { strValidationMsg = "Get Unscheduled Orders Delivery Failure"; }
                                                    ngl.showErrMsg("Get Unscheduled Orders Delivery Failure", strValidationMsg, null);
                                                }
                                            } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                                        },
                                        error: function (result) { options.error(result); }
                                    });
                                },
                                update: function (options) {
                                    var vData = {};
                                    vData.ApptControl = 0;
                                    vData.EquipID = options.data.BookCarrTrailerNo;
                                    vData.IsPickup = false;
                                    vData.Success = false;
                                    vData.IsAdd = false;
                                    vData.ErrMsg = "";

                                    EquipIDValidation = vData;
                                    EditEquipIDOrder = options.data.BookSHID;

                                    var oData = { "SHID": EditEquipIDOrder, "equipIDValidation": EquipIDValidation };
                                    updateEquipID(oData);
                                },
                                parameterMap: function (options, operation) { return options; }
                            },
                            schema: {
                                data: "Data",
                                total: "Count",
                                model: {
                                    id: "BookSHID",
                                    fields: {
                                        BookSHID: { type: "string", editable: false },
                                        BookConsPrefix: { type: "string", editable: false },
                                        BookCarrTrailerNo: { type: "string" },
                                        BookDateRequired: { type: "date", editable: false },
                                        ScheduledDate: { type: "date", editable: false },
                                        ScheduledTime: { type: "date", editable: false },
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
                                        BookDestCompControl: { type: "number", editable: false }
                                    }
                                },
                                errors: "Errors"
                            },
                            error: function (xhr, textStatus, error) { ngl.showErrMsg("Access Unscheduled Orders Delivery Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges(); }
                        });

                        $("#UnschedOrderDelGrid").kendoGrid({
                            noRecords: { template: "<p>No records available.</p>" },
                            toolbar: ["excel"],
                            excel: { fileName: "unschedDelGrouped.xlsx", allPages: true },
                            groupable: true,
                            resizable: true,
                            reorderable: true,                          
                            sortable: { mode: "single", allowUnsort: true },                           
                            sort: function (e) {
                                if (!e.sort.dir) { e.sort.dir == ""; e.sort.field == ""; }
                                if (!e.sort.field) { e.sort.field == ""; }
                                $("#txtUODSortDirection").val(e.sort.dir);
                                $("#txtUODSortField").val(e.sort.field);
                            },
                            pageable: { pageSizes: [5, 10, 15, 20, 25, 50] },
                            dataSource: dsUnschedOrderDelivery,
                            editable: 'inline',
                            resizable: true,
                            columns: [
                                  { command: [{ className: "cm-icononly-button", name: "edit", text: { edit: "", update: "", cancel: "" }, iconClass: "k-icon k-i-pencil" }, { name: "selectAppt", text: "Select Appt", click: ViewUODelAvailAppt }], title: "Action", width: 150 },
                                  { field: "BookSHID", title: "SHID", template: "<span title='${BookSHID}'>${BookSHID}</span>" },
                                  { field: "BookConsPrefix", title: "CNS", template: "<span title='${BookConsPrefix}'>${BookConsPrefix}</span>", hidden: true },
                                  { field: "BookCarrTrailerNo", title: "Equip ID", template: "<span title='${BookCarrTrailerNo}'>${BookCarrTrailerNo}</span>" },
                                  { field: "Warehouse", title: "Warehouse", width: 110, template: "<span title='${Warehouse}'>${Warehouse}</span>" },
                                  { field: "Address1", title: "Address 1", width: 125, template: "<span title='${Address1}'>${Address1}</span>" },
                                  { field: "Address2", hidden: true },
                                  { field: "City", template: "<span title='${City}'>${City}</span>" },
                                  { field: "State", width: 50 },
                                  { field: "Zip", width: 50 },
                                  { field: "Country", hidden: true },
                                  { field: "BookCarrierControl", hidden: true },
                                  { field: "CarrierName", title: "Carrier Name", hidden: true },
                                  { field: "CarrierNumber", title: "", hidden: true },
                                  { field: "ScheduledDate", title: "Sched Date", format: "{0:M/d/yyyy}" },
                                  { field: "ScheduledTime", title: "Sched Time", template: "#= kendo.toString(kendo.parseDate(ScheduledTime, 'HH:mm'), 'HH:mm') #" }, //Modified by LVV on 11/1/2019
                                  { field: "BookDateRequired", title: "Req Date", format: "{0:M/d/yyyy}" },
                                  { field: "BookDestCompControl", hidden: true }                                  
                            ],
                            detailTemplate: kendo.template($("#AMSCarGroupedDetTemplate").html()),
                            detailInit: function(e) {
                                var detailRow = e.detailRow;
                                detailRow.find(".tabstrip").kendoTabStrip({
                                    animation: {
                                        open: { effects: "fadeIn" }
                                    }
                                });
                                detailRow.find(".orders").kendoGrid({
                                    dataSource: {
                                        serverPaging: false, serverSorting: false, serverFiltering: false,
                                        transport: {
                                            read: {
                                                url: "api/CarrierSchedulerGrouped/GetUODelDetailsGrouped", 
                                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                                type: "GET",
                                                data: function() {
                                                    var gr = new GenericResult();
                                                    gr.strField = e.data.BookSHID;
                                                    gr.strField2 = e.data.Warehouse;
                                                    gr.intField1 = e.data.BookDestCompControl;
                                                    gr.dtField = ngl.formatDate(e.data.BookDateRequired, '', 't'); //e.data.BookDateRequired;
                                                    return { filter: JSON.stringify(gr) };
                                                }
                                            },
                                            parameterMap: function (options, operation) { return options; }
                                        },                           
                                        schema: {
                                            data: "Data",
                                            total: "Count",
                                            model: {
                                                id: "BookSHID",
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
                                                    IsPickup: { type: "boolean", editable: false }
                                                }
                                            },
                                            errors: "Errors"
                                        },
                                        error: function (xhr, textStatus, error) {
                                            ngl.showErrMsg("Access Unscheduled Orders Delivery Detail Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges();
                                        }                                       
                                    },
                                    resizeable: true, scrollable: true, sortable: false, pageable: false,
                                    columns: [
                                        //{ field: "BookControl", hidden: true },
                                        //{ field: "BookSHID", title: "SHID", hidden: true },
                                        { field: "BookConsPrefix", title: "CNS", template: "<span title='${BookConsPrefix}'>${BookConsPrefix}</span>" },
                                        { field: "BookCarrOrderNumber", title: "Order No", template: "<span title='${BookCarrOrderNumber}'>${BookCarrOrderNumber}</span>" },
                                        { field: "BookProNumber", title: "PRO", template: "<span title='${BookProNumber}'>${BookProNumber}</span>" },
                                        { field: "BookLoadPONumber", title: "PO", template: "<span title='${BookLoadPONumber}'>${BookLoadPONumber}</span>" },
                                        { field: "BookCarrTrailerNo", title: "Equip ID", template: "<span title='${BookCarrTrailerNo}'>${BookCarrTrailerNo}</span>" },                                                                
                                        { field: "ScheduledDate", title: "Sched Date", format: "{0:M/d/yyyy}" },
                                        { field: "ScheduledTime", title: "Sched Time", template: "#= kendo.toString(kendo.parseDate(ScheduledTime, 'HH:mm'), 'HH:mm') #" }, //Modified by LVV on 11/1/2019                                       
                                        { field: "BookDateRequired", title: "Req Date", format: "{0:M/d/yyyy}" },
                                        { field: "Inbound", hidden: true },
                                        { field: "BookTotalCases", hidden: true },
                                        { field: "BookTotalWgt", hidden: true },
                                        { field: "BookTotalPL", hidden: true },
                                        { field: "BookTotalCube", hidden: true },
                                        { field: "BookNotesVisable1", template: "<span title='${BookNotesVisable1}'>${BookNotesVisable1}</span>", hidden: true },
                                        { field: "BookNotesVisable2", template: "<span title='${BookNotesVisable2}'>${BookNotesVisable2}</span>", hidden: true },
                                        { field: "BookNotesVisable3", template: "<span title='${BookNotesVisable3}'>${BookNotesVisable3}</span>", hidden: true }                                        
                                    ],
                                });                   
                            }
                        });

                        function ViewUODelAvailAppt(e) {
                            UODataItem = this.dataItem($(e.currentTarget).closest("tr"));
                 
                            $("#txtUOEquipID").text(UODataItem.BookCarrTrailerNo ? UODataItem.BookCarrTrailerNo : "");
                            //This is set to Delivery Date on Delivery
                            $("#lblUOLRDate").text("Delivery Date: ");
                            $("#txtUOLRDate").text(kendo.toString(UODataItem.BookDateRequired, 'M/d/yyyy'));
                            $("#txtUOScheduled").text(kendo.toString(UODataItem.ScheduledDate ? UODataItem.ScheduledDate : "", 'M/d/yyyy') + " " + kendo.toString(UODataItem.ScheduledTime ? UODataItem.ScheduledTime : "", 'HH:mm'));
                            $("#txtUOSHID").text(UODataItem.BookSHID);
                            $("#txtUOCNS").text(UODataItem.BookConsPrefix);

                            blnUOIsPickup = false;

                            $("#AvailableApptsUOGrid").data("kendoGrid").dataSource.data([]);
                            $("#AvailableApptsUOGrid").data("kendoGrid").dataSource.read();
                        }




                        //********** AvailableApptsBAGrid **********//
                        var BADataItem  = {};
                        var blnBAIsPickup;
                        dsAvailableApptsBA = new kendo.data.DataSource({
                            pageSize: 10,
                            transport: {
                                read: function (options) {
                                    var whseControl = 0;
                                    var dt = null;
                                    if(blnBAIsPickup) { whseControl = BADataItem.BookOrigCompControl; dt = BADataItem.BookDateLoad; }else{ whseControl = BADataItem.BookDestCompControl; dt = BADataItem.BookDateRequired; }
                                    
                                    var gr = new GenericResult(); 
                                    gr.blnField = blnBAIsPickup;
                                    gr.blnField1 = false; //IsDelete
                                    gr.strField = BADataItem.BookSHID;
                                    gr.strField2 = BADataItem.Warehouse;
                                    gr.intField1 = whseControl;
                                    gr.dtField = ngl.formatDate(dt, '', 't'); //dt;                                   
                                    $.ajax({
                                        url: 'api/CarrierSchedulerGrouped/GetModifyOptionCarrierBAGrouped',
                                        contentType: 'application/json; charset=utf-8',
                                        dataType: 'json',
                                        data: { filter: JSON.stringify(gr) },
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                        success: function (data) {
                                            options.success(data);
                                            console.log(data.Data);
                                            try {
                                                var blnSuccess = false;
                                                var blnErrorShown = false;
                                                var strValidationMsg = "";
                                                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                                    if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Get Available Appointments BA Failure", data.Errors, null); }
                                                    else {
                                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                                                blnSuccess = true;
                                                                if (data.Data[0].blnMustRequestAppt != undefined) {
                                                                    if (data.Data[0].blnMustRequestAppt) {
                                                                        eObject.emailTo = data.Data[0].RequestSendToEmail;
                                                                        eObject.emailSubject = data.Data[0].Subject;
                                                                        eObject.emailBody = data.Data[0].Body;
                                                                        $("#txtEmailMsg").text(data.Data[0].Message?data.Data[0].Message:"");
                                                                        $("#txtEmailBody").html(data.Data[0].Body);
                                                                        $("#txtEmailComments").val("");
                                                                        wndRequestEmail.title(data.Data[0].Subject);
                                                                        wndRequestEmail.center().open();
                                                                    } else {
                                                                        $("#BookedApptPickGrid").data("kendoGrid").dataSource.read();
                                                                        $("#BookedApptDelGrid").data("kendoGrid").dataSource.read();
                                                                    }
                                                                } else {
                                                                    wndViewCarAvailApptBAGrouped.title("Carrier Scheduling - Select Appointment");
                                                                    wndViewCarAvailApptBAGrouped.center().open();
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                if (blnSuccess === false && blnErrorShown === false) {
                                                    if (strValidationMsg.length < 1) { strValidationMsg = "Get Available Appointments BA Failure"; }
                                                    ngl.showErrMsg("Get Available Appointments BA Failure", strValidationMsg, null);
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
                            error: function (xhr, textStatus, error) { ngl.showErrMsg("Access Available Appointment BA Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges(); }
                        });

                        $("#AvailableApptsBAGrid").kendoGrid({
                            noRecords: { template: "<p>No records available.</p>" },
                            autoBind: false,
                            height: 200,
                            dataSource: dsAvailableApptsBA,
                            pageable: true,
                            columns: [
                                { command: [{ name: "bookAppt", text: "Book Appt", width: 100, click: UpdateBookedAppointment }], title: "Action" },
                                { field: "Warehouse", title: "Warehouse", template: "<span title='${Warehouse}'>${Warehouse}</span>" },
                                { field: "Date", title: "Date", format: "{0:M/d/yyyy}" },
                                { field: "StartTime", title: "Start Time", template: "#= kendo.toString(kendo.parseDate(StartTime, 'HH:mm'), 'HH:mm') #" }, //format: "{0:HH:mm}", //Modified by LVV on 11/1/2019
                                { field: "EndTime", title: "End Time", hidden: true },
                                { field: "Docks", title: "Docks", hidden: true },
                                { field: "Books", title: "Books", hidden: true },
                                { field: "CarrierControl", title: "CarrierControl", hidden: true },
                                { field: "CarrierNumber", title: "CarrierNumber", hidden: true },
                                { field: "CarrierName", title: "CarrierName", hidden: true },
                                { field: "CompControl", title: "CompControl", hidden: true },
                            ],
                        });
               
                        function UpdateBookedAppointment(e) {
                            var tsDataItem = this.dataItem($(e.currentTarget).closest("tr"));
                    
                            var s = new AMSCarrierAvailableSlots();                         
                            s.Docks = tsDataItem.Docks;
                            //Modified by LVV for v-8.2 on 11/1/2019 we now use ngl.convertDateForWindows to avoid non-ascii characters in date string
                            s.StartTime = ngl.convertTimePickerToDateString(tsDataItem.StartTime, ngl.convertDateForWindows(tsDataItem.StartTime, ""), "");                      
                            s.EndTime = tsDataItem.EndTime;
                            s.Warehouse = tsDataItem.Warehouse;
                            s.CarrierControl = tsDataItem.CarrierControl;
                            s.CompControl = tsDataItem.CompControl;
                            s.CarrierNumber = tsDataItem.CarrierNumber;
                            s.CarrierName = tsDataItem.CarrierName;
                            s.Books = tsDataItem.Books;
                            //Modified by LVV for v-8.2 on 11/1/2019 we now use ngl.convertDateForWindows to avoid non-ascii characters in date string
                            s.Date = ngl.convertTimePickerToDateString(tsDataItem.Date, ngl.convertDateForWindows(tsDataItem.Date, ""), "");

                            var w = new AMSCarrierBAWrapper(); 
                            w.AMSCarrierAvailableSlots = s;
                            w.blnIsPickup = blnBAIsPickup; 

                            //Modified by LVV on 11/1/2019
                            $.ajax({
                                url: "api/CarrierSchedulerGrouped/UpdateCarrierBookedAppointmentGrouped",
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                dataType: 'json',
                                data: JSON.stringify(w),
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                success: function (data) {
                                    try {
                                        var blnSuccess = false;
                                        var blnErrorShown = false;
                                        var strValidationMsg = "";
                                        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                            if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Update Appointment Failure", data.Errors, null); }
                                            else {
                                                if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                    if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                                        blnSuccess = true;
                                                        if (data.Data[0] == false) { ngl.showWarningMsg("Update Appointment Failure!", "", null); } 
                                                        else { wndViewCarAvailApptBAGrouped.close(); ngl.showSuccessMsg("Update Appointment Success"); refresh(); } //refresh Grids
                                                    }
                                                }
                                            }
                                        }
                                        if (blnSuccess === false && blnErrorShown === false) {
                                            if (strValidationMsg.length < 1) { strValidationMsg = "Update Appointment Failure"; }
                                            ngl.showErrMsg("Update Appointment Failure", strValidationMsg, null);
                                        }
                                    } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                                },
                                error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Update Appointment Failure"); ngl.showErrMsg("Update Appointment Failure", sMsg, null); }
                            });
                        }

                        //************* BookedApptPickGrid ***************//
                        dsBookedApptPickup = new kendo.data.DataSource({
                            serverSorting: true, 
                            serverPaging: true, 
                            pageSize: 5,
                            transport: {
                                read: function (options) {
                                    var s = new AllFilter();
                                    s.sortName = $("#txtBAPSortField").val();
                                    s.sortDirection = $("#txtBAPSortDirection").val();
                                    s.page = options.data.page;
                                    s.skip = options.data.skip;
                                    s.take = options.data.take;
                                    s.filterName = $("#ddlBAPFilters").data("kendoDropDownList").value();
                                    switch (s.filterName) {
                                        case "None":
                                            s.filterName = "";
                                            s.filterValue = "";
                                            break;
                                        case "BookDateLoad":
                                            s.filterValue = "";
                                            s.filterFrom = $("#txtBAPFilterDateVal").val();
                                            s.filterTo = $("#txtBAPFilterDateVal").val();
                                            break;
                                        case "ScheduledDate":
                                            s.filterValue = "";
                                            s.filterFrom = $("#txtBAPFilterDateVal").val();
                                            s.filterTo = $("#txtBAPFilterDateVal").val();
                                            break;
                                        default:
                                            s.filterValue = $("#txtBAPFilterVal").data("kendoMaskedTextBox").value();
                                            break;
                                    }
                                    $.ajax({
                                        url: 'api/CarrierSchedulerGrouped/GetAMSCarrierBAPickGrouped',
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
                                                    if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Get Booked Appointments Pickup Failure", data.Errors, null); }
                                                    else {
                                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) { blnSuccess = true; }
                                                            else { blnSuccess = true; strValidationMsg = "No records were found matching your search criteria"; }
                                                        }
                                                    }
                                                }
                                                if (blnSuccess === false && blnErrorShown === false) {
                                                    if (strValidationMsg.length < 1) { strValidationMsg = "Get Booked Appointments Pickup Failure"; }
                                                    ngl.showErrMsg("Get Booked Appointments Pickup Failure", strValidationMsg, null);
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
                                    id: "BookSHID",
                                    fields: {
                                        BookSHID: { type: "string", editable: false },
                                        BookConsPrefix: { type: "string", editable: false },
                                        BookCarrTrailerNo: { type: "string" },                                       
                                        BookDateLoad: { type: "date", editable: false },
                                        ScheduledDate: { type: "date", editable: false },
                                        ScheduledTime: { type: "date", editable: false },
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
                                        BookOrigCompControl: { type: "number", editable: false }
                                    }
                                },
                                errors: "Errors"
                            },
                            error: function (xhr, textStatus, error) { ngl.showErrMsg("Access Booked Appointmnets Pickup Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges(); }
                        });

                        $("#BookedApptPickGrid").kendoGrid({
                            noRecords: { template: "<p>No records available.</p>" },
                            toolbar: ["excel"],
                            excel: { fileName: "bookedPickGrouped.xlsx", allPages: true },
                            groupable: true,
                            resizable: true,
                            reorderable: true,                          
                            sortable: { mode: "single", allowUnsort: true },                           
                            sort: function (e) {
                                if (!e.sort.dir) { e.sort.dir == ""; e.sort.field == ""; }
                                if (!e.sort.field) { e.sort.field == ""; }
                                $("#txtBAPSortDirection").val(e.sort.dir);
                                $("#txtBAPSortField").val(e.sort.field);
                            },
                            pageable: { pageSizes: [5, 10, 15, 20, 25, 50] },
                            dataSource: dsBookedApptPickup,
                            columns: [
                                { command: [{ className: "cm-icononly-button", name: "edit", text: "", iconClass: "k-icon k-i-trash", click: ViewBAPickAvailAppt }, { className: "cm-icononly-button", name: "delete", text: "", iconClass: "k-icon k-i-trash", click: DeleteBAPickGrouped }], title: "Action", width: 120 },
                                { field: "BookSHID", title: "SHID", template: "<span title='${BookSHID}'>${BookSHID}</span>" },
                                { field: "BookConsPrefix", title: "CNS", template: "<span title='${BookConsPrefix}'>${BookConsPrefix}</span>", hidden: true },
                                { field: "BookCarrTrailerNo", title: "Equip ID", template: "<span title='${BookCarrTrailerNo}'>${BookCarrTrailerNo}</span>" },
                                { field: "Warehouse", title: "Warehouse", width: 110, template: "<span title='${Warehouse}'>${Warehouse}</span>" },
                                { field: "Address1", title: "Address 1", width: 125, template: "<span title='${Address1}'>${Address1}</span>" },
                                { field: "Address2", hidden: true },
                                { field: "City", template: "<span title='${City}'>${City}</span>" },
                                { field: "State", width: 50 },
                                { field: "Zip", width: 50 },
                                { field: "Country", hidden: true },                               
                                { field: "BookCarrierControl", hidden: true },
                                { field: "CarrierName", title: "Carrier Name", hidden: true },
                                { field: "CarrierNumber", title: "", hidden: true },          
                                { field: "BookDateLoad", title: "Load Date", format: "{0:M/d/yyyy}" },
                                { field: "ScheduledDate", title: "Sched Date", format: "{0:M/d/yyyy}" },
                                { field: "ScheduledTime", title: "Sched Time", template: "#= kendo.toString(kendo.parseDate(ScheduledTime, 'HH:mm'), 'HH:mm') #" }, //Modified by LVV on 11/1/2019
                                { field: "BookOrigCompControl", hidden: true },                               
                            ],
                            detailTemplate: kendo.template($("#AMSCarGroupedDetTemplate").html()),
                            detailInit: function(e) {
                                var detailRow = e.detailRow;
                                detailRow.find(".tabstrip").kendoTabStrip({
                                    animation: {
                                        open: { effects: "fadeIn" }
                                    }
                                });
                                detailRow.find(".orders").kendoGrid({
                                    dataSource: {
                                        serverPaging: false, serverSorting: false, serverFiltering: false,
                                        transport: {
                                            read: {
                                                url: "api/CarrierSchedulerGrouped/GetBAPickDetailsGrouped", 
                                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                                type: "GET",
                                                data: function() {
                                                    var gr = new GenericResult();
                                                    gr.strField = e.data.BookSHID;
                                                    gr.strField2 = e.data.Warehouse;
                                                    gr.intField1 = e.data.BookOrigCompControl;
                                                    gr.dtField = ngl.formatDate(e.data.BookDateLoad, '', 't'); //e.data.BookDateLoad;
                                                    return { filter: JSON.stringify(gr) };
                                                }
                                            },
                                            parameterMap: function (options, operation) { return options; }
                                        },                           
                                        schema: {
                                            data: "Data",
                                            total: "Count",
                                            model: {
                                                id: "BookSHID",
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
                                                    IsPickup: { type: "boolean", editable: false }
                                                }
                                            },
                                            errors: "Errors"
                                        },
                                        error: function (xhr, textStatus, error) { ngl.showErrMsg("Access Booked Appointments Pickup Detail Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges(); }                                       
                                    },
                                    resizeable: true, scrollable: true, sortable: false, pageable: false,
                                    columns: [
                                        //{ field: "BookControl", hidden: true },
                                        //{ field: "BookSHID", title: "SHID", hidden: true },
                                        { field: "BookConsPrefix", title: "CNS", template: "<span title='${BookConsPrefix}'>${BookConsPrefix}</span>" },
                                        { field: "BookCarrOrderNumber", title: "Order No", template: "<span title='${BookCarrOrderNumber}'>${BookCarrOrderNumber}</span>" },
                                        { field: "BookProNumber", title: "PRO", template: "<span title='${BookProNumber}'>${BookProNumber}</span>" },
                                        { field: "BookLoadPONumber", title: "PO", template: "<span title='${BookLoadPONumber}'>${BookLoadPONumber}</span>" },
                                        { field: "BookCarrTrailerNo", title: "Equip ID", template: "<span title='${BookCarrTrailerNo}'>${BookCarrTrailerNo}</span>" },                                              
                                        { field: "BookDateLoad", title: "Load Date", format: "{0:M/d/yyyy}" },
                                        { field: "ScheduledDate", title: "Sched Date", format: "{0:M/d/yyyy}" },
                                        { field: "ScheduledTime", title: "Sched Time", template: "#= kendo.toString(kendo.parseDate(ScheduledTime, 'HH:mm'), 'HH:mm') #" }, //Modified by LVV on 11/1/2019                                       
                                        { field: "Inbound", hidden: true },
                                        { field: "BookTotalCases", hidden: true },
                                        { field: "BookTotalWgt", hidden: true },
                                        { field: "BookTotalPL", hidden: true },
                                        { field: "BookTotalCube", hidden: true },
                                        { field: "BookNotesVisable1", template: "<span title='${BookNotesVisable1}'>${BookNotesVisable1}</span>", hidden: true },
                                        { field: "BookNotesVisable2", template: "<span title='${BookNotesVisable2}'>${BookNotesVisable2}</span>", hidden: true },
                                        { field: "BookNotesVisable3", template: "<span title='${BookNotesVisable3}'>${BookNotesVisable3}</span>", hidden: true }                                        
                                    ],
                                });                   
                            }
                        });
               
                        function ViewBAPickAvailAppt(e) {
                            alert('ViewBAPickAvailAppt');
                            BADataItem = this.dataItem($(e.currentTarget).closest("tr"));             

                            $("#txtBAEquipID").text(BADataItem.BookCarrTrailerNo ? BADataItem.BookCarrTrailerNo : "");
                            //This is set to Load Date on Pickup
                            $("#lblBALRDate").text("Load Date: ");
                            $("#txtBALRDate").text(kendo.toString(BADataItem.BookDateLoad, 'M/d/yyyy'));
                            $("#txtBAScheduled").text(kendo.toString(BADataItem.ScheduledDate ? BADataItem.ScheduledDate : "", 'M/d/yyyy') + " " + kendo.toString(BADataItem.ScheduledTime ? BADataItem.ScheduledTime : "", 'HH:mm'));
                            $("#txtBASHID").text(BADataItem.BookSHID);
                            $("#txtBACNS").text(BADataItem.BookConsPrefix);
                   
                            blnBAIsPickup = true;

                            $("#AvailableApptsBAGrid").data("kendoGrid").dataSource.data([]);
                            $("#AvailableApptsBAGrid").data("kendoGrid").dataSource.read();
                        };

                        function DeleteBAPickGrouped(e) {
                            
                            var dataObject = this.dataItem($(e.currentTarget).closest("tr"));
                            var gr = new GenericResult();
                            gr.strField = dataObject.BookSHID;
                            gr.blnField = true; //blnBAIsPickup
                            gr.blnField1 = true; //IsDelete
                            $.ajax({
                                url: 'api/CarrierSchedulerGrouped/GetModifyOptionCarrierBAGrouped',
                                contentType: 'application/json; charset=utf-8',
                                dataType: 'json',
                                data: { filter: JSON.stringify(gr) },
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                success: function (data) {
                                    console.log(data.Data);
                                    try {
                                        var blnSuccess = false;
                                        var blnErrorShown = false;
                                        var strValidationMsg = "";
                                        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                            if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Delete Booked Appointment Grouped Failure", data.Errors, null); }
                                            else {
                                                if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                    if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                                        blnSuccess = true;
                                                        if (data.Data[0].blnMustRequestAppt != undefined) {
                                                            if (data.Data[0].blnMustRequestAppt) {
                                                                eObject.emailTo = data.Data[0].RequestSendToEmail;
                                                                eObject.emailSubject = data.Data[0].Subject;
                                                                eObject.emailBody = data.Data[0].Body;
                                                                $("#txtEmailMsg").text(data.Data[0].Message ? data.Data[0].Message : "");
                                                                $("#txtEmailBody").html(data.Data[0].Body);
                                                                $("#txtEmailComments").val("");
                                                                wndRequestEmail.title(data.Data[0].Subject);
                                                                wndRequestEmail.center().open();
                                                            } else { refresh(); }
                                                        } else { refresh(); }
                                                    }
                                                }
                                            }
                                        }
                                        if (blnSuccess === false && blnErrorShown === false) {
                                            if (strValidationMsg.length < 1) { strValidationMsg = "Delete Booked Appointment Grouped Failure"; }
                                            ngl.showErrMsg("Delete Booked Appointment Grouped Failure", strValidationMsg, null);
                                        }
                                    } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                                },
                                error: function (result) { options.error(result); }
                            });
                        }
              
                        //************* BookedApptDelGrid ***************//
                        dsBookedApptDelivery = new kendo.data.DataSource({
                            serverSorting: true, 
                            serverPaging: true, 
                            pageSize: 5,
                            transport: {
                                read: function (options) {
                                    var s = new AllFilter();
                                    s.sortName = $("#txtBADSortField").val();
                                    s.sortDirection = $("#txtBADSortDirection").val();
                                    s.page = options.data.page;
                                    s.skip = options.data.skip;
                                    s.take = options.data.take;
                                    s.filterName = $("#ddlBADFilters").data("kendoDropDownList").value();
                                    switch (s.filterName) {
                                        case "None":
                                            s.filterName = "";
                                            s.filterValue = "";
                                            break;
                                        case "BookDateRequired":
                                            s.filterValue = "";
                                            s.filterFrom = $("#txtBADFilterDateVal").val();
                                            s.filterTo = $("#txtBADFilterDateVal").val();
                                            break;
                                        case "ScheduledDate":
                                            s.filterValue = "";
                                            s.filterFrom = $("#txtBADFilterDateVal").val();
                                            s.filterTo = $("#txtBADFilterDateVal").val();
                                            break;
                                        default:
                                            s.filterValue = $("#txtBADFilterVal").data("kendoMaskedTextBox").value();
                                            break;
                                    }
                                    $.ajax({
                                        url: 'api/CarrierSchedulerGrouped/GetAMSCarrierBADelGrouped',
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
                                                    if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Get Booked Appointments Delivery Failure", data.Errors, null); }
                                                    else {
                                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) { blnSuccess = true; }
                                                            else { blnSuccess = true; strValidationMsg = "No records were found matching your search criteria"; }
                                                        }
                                                    }
                                                }
                                                if (blnSuccess === false && blnErrorShown === false) {
                                                    if (strValidationMsg.length < 1) { strValidationMsg = "Get Booked Appointments Delivery Failure"; }
                                                    ngl.showErrMsg("Get Booked Appointments Delivery Failure", strValidationMsg, null);
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
                                    id: "BookSHID",
                                    fields: {
                                        BookSHID: { type: "string" },
                                        BookConsPrefix: { type: "string" },
                                        BookCarrTrailerNo: { type: "string" },
                                        BookDateRequired: { type: "date", editable: false },
                                        ScheduledDate: { type: "date", editable: false },
                                        ScheduledTime: { type: "date", editable: false },
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
                                        BookDestCompControl: { type: "number", editable: false }
                                    }
                                },
                                errors: "Errors"
                            },
                            error: function (xhr, textStatus, error) { ngl.showErrMsg("Access Booked Appointmnets Delivery Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges(); }
                        });

                        $("#BookedApptDelGrid").kendoGrid({
                            //noRecords: true,
                            //template: "<p>No data available on current page. Current page is: #=this.dataSource.page()#</p>"
                            noRecords: { template: "<p>No records available.</p>" },
                            toolbar: ["excel"],
                            excel: { fileName: "bookedDelGrouped.xlsx", allPages: true },
                            groupable: true,
                            resizable: true,
                            reorderable: true,                          
                            sortable: { mode: "single", allowUnsort: true },                           
                            sort: function (e) {
                                if (!e.sort.dir) { e.sort.dir == ""; e.sort.field == ""; }
                                if (!e.sort.field) { e.sort.field == ""; }
                                $("#txtBADSortDirection").val(e.sort.dir);
                                $("#txtBADSortField").val(e.sort.field);
                            },
                            pageable: { pageSizes: [5, 10, 15, 20, 25, 50] },
                            dataSource: dsBookedApptDelivery,
                            columns: [
                                  { command: [{ className: "cm-icononly-button", name: "edit", text: { edit: "", update: "", cancel: "" }, click: ViewBADelAvailAppt }, { className: "cm-icononly-button", name: "delete", text: "", iconClass: "k-icon k-i-trash", click: DeleteBADelGrouped }], title: "Action", width: 120 },
                                  { field: "BookSHID", title: "SHID", template: "<span title='${BookSHID}'>${BookSHID}</span>" },
                                  { field: "BookConsPrefix", title: "CNS", template: "<span title='${BookConsPrefix}'>${BookConsPrefix}</span>", hidden: true },
                                  { field: "BookCarrTrailerNo", title: "Equip ID", template: "<span title='${BookCarrTrailerNo}'>${BookCarrTrailerNo}</span>" },
                                  { field: "Warehouse", title: "Warehouse", width: 110, template: "<span title='${Warehouse}'>${Warehouse}</span>" },
                                  { field: "Address1", title: "Address 1", width: 125, template: "<span title='${Address1}'>${Address1}</span>" },
                                  { field: "Address2", hidden: true },
                                  { field: "City", template: "<span title='${City}'>${City}</span>" },
                                  { field: "State", width: 50 },
                                  { field: "Zip", width: 50 },
                                  { field: "Country", hidden: true },                                 
                                  { field: "BookCarrierControl", hidden: true },
                                  { field: "CarrierName", title: "Carrier Name", hidden: true },
                                  { field: "CarrierNumber", title: "", hidden: true },
                                  { field: "BookDateRequired", title: "Req Date", format: "{0:M/d/yyyy}" },
                                  { field: "ScheduledDate", title: "Sched Date", format: "{0:M/d/yyyy}" },
                                  { field: "ScheduledTime", title: "Sched Time", template: "#= kendo.toString(kendo.parseDate(ScheduledTime, 'HH:mm'), 'HH:mm') #" }, //Modified by LVV on 11/1/2019
                                  { field: "BookDestCompControl", hidden: true }
                            ],
                            detailTemplate: kendo.template($("#AMSCarGroupedDetTemplate").html()),
                            detailInit: function(e) {
                                var detailRow = e.detailRow;
                                detailRow.find(".tabstrip").kendoTabStrip({
                                    animation: {
                                        open: { effects: "fadeIn" }
                                    }
                                });
                                detailRow.find(".orders").kendoGrid({
                                    dataSource: {
                                        serverPaging: false, serverSorting: false, serverFiltering: false,
                                        transport: {
                                            read: {
                                                url: "api/CarrierSchedulerGrouped/GetBADelDetailsGrouped", 
                                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                                type: "GET",
                                                data: function() {
                                                    var gr = new GenericResult();
                                                    gr.strField = e.data.BookSHID;
                                                    gr.strField2 = e.data.Warehouse;
                                                    gr.intField1 = e.data.BookDestCompControl;
                                                    gr.dtField = ngl.formatDate(e.data.BookDateRequired, '', 't'); //e.data.BookDateRequired;
                                                    return { filter: JSON.stringify(gr) };
                                                }
                                            },
                                            parameterMap: function (options, operation) { return options; }
                                        },                           
                                        schema: {
                                            data: "Data",
                                            total: "Count",
                                            model: {
                                                id: "BookSHID",
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
                                                    IsPickup: { type: "boolean", editable: false }
                                                }
                                            },
                                            errors: "Errors"
                                        },
                                        error: function (xhr, textStatus, error) { ngl.showErrMsg("Access Booked Appointments Delivery Detail Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges(); }                                       
                                    },
                                    resizeable: true, scrollable: true, sortable: false, pageable: false,
                                    columns: [
                                        //{ field: "BookControl", hidden: true },
                                        //{ field: "BookSHID", title: "SHID", hidden: true },
                                        { field: "BookConsPrefix", title: "CNS", template: "<span title='${BookConsPrefix}'>${BookConsPrefix}</span>" },
                                        { field: "BookCarrOrderNumber", title: "Order No", template: "<span title='${BookCarrOrderNumber}'>${BookCarrOrderNumber}</span>" },
                                        { field: "BookProNumber", title: "PRO", template: "<span title='${BookProNumber}'>${BookProNumber}</span>" },
                                        { field: "BookLoadPONumber", title: "PO", template: "<span title='${BookLoadPONumber}'>${BookLoadPONumber}</span>" },
                                        { field: "BookCarrTrailerNo", title: "Equip ID", template: "<span title='${BookCarrTrailerNo}'>${BookCarrTrailerNo}</span>" },                               
                                        { field: "BookDateRequired", title: "Req Date", format: "{0:M/d/yyyy}" },
                                        { field: "ScheduledDate", title: "Sched Date", format: "{0:M/d/yyyy}" },
                                        { field: "ScheduledTime", title: "Sched Time", template: "#= kendo.toString(kendo.parseDate(ScheduledTime, 'HH:mm'), 'HH:mm') #" }, //Modified by LVV on 11/1/2019                                                                               
                                        { field: "Inbound", hidden: true },
                                        { field: "BookTotalCases", hidden: true },
                                        { field: "BookTotalWgt", hidden: true },
                                        { field: "BookTotalPL", hidden: true },
                                        { field: "BookTotalCube", hidden: true },
                                        { field: "BookNotesVisable1", template: "<span title='${BookNotesVisable1}'>${BookNotesVisable1}</span>", hidden: true },
                                        { field: "BookNotesVisable2", template: "<span title='${BookNotesVisable2}'>${BookNotesVisable2}</span>", hidden: true },
                                        { field: "BookNotesVisable3", template: "<span title='${BookNotesVisable3}'>${BookNotesVisable3}</span>", hidden: true }                                        
                                    ],
                                });                   
                            }
                        });

                        function ViewBADelAvailAppt(e) {
                            BADataItem = this.dataItem($(e.currentTarget).closest("tr"));           

                            $("#txtBAEquipID").text(BADataItem.BookCarrTrailerNo ? BADataItem.BookCarrTrailerNo : "");
                            //This is set to Delivery Date on Delivery
                            $("#lblBALRDate").text("Delivery Date: ");
                            $("#txtBALRDate").text(kendo.toString(BADataItem.BookDateRequired, 'M/d/yyyy'));
                            $("#txtBAScheduled").text(kendo.toString(BADataItem.ScheduledDate ? BADataItem.ScheduledDate : "", 'M/d/yyyy') + " " + kendo.toString(BADataItem.ScheduledTime ? BADataItem.ScheduledTime : "", 'HH:mm'));
                            $("#txtBASHID").text(BADataItem.BookSHID);
                            $("#txtBACNS").text(BADataItem.BookConsPrefix);

                            blnBAIsPickup = false;
                   
                            $("#AvailableApptsBAGrid").data("kendoGrid").dataSource.data([]);
                            $("#AvailableApptsBAGrid").data("kendoGrid").dataSource.read();
                        };
                     
                        function DeleteBADelGrouped(e) {
                            var dataObject = this.dataItem($(e.currentTarget).closest("tr"));
                            var gr = new GenericResult();
                            gr.strField = dataObject.BookSHID;
                            gr.blnField = false; //blnBAIsPickup
                            gr.blnField1 = true; //IsDelete
                            $.ajax({
                                url: 'api/CarrierSchedulerGrouped/GetModifyOptionCarrierBAGrouped',
                                contentType: 'application/json; charset=utf-8',
                                dataType: 'json',
                                data: { filter: JSON.stringify(gr) },
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                success: function (data) {
                                    console.log(data.Data);
                                    try {
                                        var blnSuccess = false;
                                        var blnErrorShown = false;
                                        var strValidationMsg = "";
                                        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                            if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Delete Booked Appointment Grouped Failure", data.Errors, null); }
                                            else {
                                                if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                    if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                                        blnSuccess = true;
                                                        if (data.Data[0].blnMustRequestAppt != undefined) {
                                                            if (data.Data[0].blnMustRequestAppt) {
                                                                eObject.emailTo = data.Data[0].RequestSendToEmail;
                                                                eObject.emailSubject = data.Data[0].Subject;
                                                                eObject.emailBody = data.Data[0].Body;
                                                                $("#txtEmailMsg").text(data.Data[0].Message ? data.Data[0].Message : "");
                                                                $("#txtEmailBody").html(data.Data[0].Body);
                                                                $("#txtEmailComments").val("");
                                                                wndRequestEmail.title(data.Data[0].Subject);
                                                                wndRequestEmail.center().open();
                                                            } else { refresh(); }
                                                        } else { refresh(); }
                                                    }
                                                }
                                            }
                                        }
                                        if (blnSuccess === false && blnErrorShown === false) {
                                            if (strValidationMsg.length < 1) { strValidationMsg = "Delete Booked Appointment Grouped Failure"; }
                                            ngl.showErrMsg("Delete Booked Appointment Grouped Failure", strValidationMsg, null);
                                        }
                                    } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                                },
                                error: function (result) { options.error(result); }
                            });
                        };




                        //************* wndViewCarAvailApptUOGrouped ***************//
                        wndViewCarAvailApptUOGrouped = $("#wndViewCarAvailApptUOGrouped").kendoWindow({
                            title: "Edit/Add",
                            height: 'auto',
                            width: 850,
                            modal: true,
                            visible: false,
                            actions: ["Minimize", "Maximize", "Close"]
                        }).data("kendoWindow");

                        //************* wndRequestEmail ***************//
                        wndRequestEmail = $("#wndRequestEmail").kendoWindow({
                            title: "Edit/Add",
                            height: 'auto',
                            width: 440,
                            modal: true,
                            visible: false,
                            actions: ["Minimize", "Maximize", "Close"]
                        }).data("kendoWindow");

                        //************* wndViewCarAvailApptBAGrouped ***************//
                        wndViewCarAvailApptBAGrouped = $("#wndViewCarAvailApptBAGrouped").kendoWindow({
                            title: "Edit/Add",
                            height: 'auto',
                            width: 850,
                            modal: true,
                            visible: false,
                            actions: ["Minimize", "Maximize", "Close"]
                        }).data("kendoWindow");

                        //************* wndEditEquipIDGrouped ***************//
                        wndEditEquipIDGrouped = $("#wndEditEquipIDGrouped").kendoWindow({
                            title: "Edit/Add",
                            height: 'auto',
                            width: 'auto',
                            modal: true,
                            visible: false,
                            actions: ["Minimize", "Maximize", "Close"]
                        }).data("kendoWindow");



                        //************* Carrier Order Summary For "week" and "day" Charts ***************//
                        $(document).on("change", "input[name='charttype']", function () {
                            createDaysChart(this.value);
                            createWeeksChart(this.value);
                        });

                        dsAMSOrdersCharts = new kendo.data.DataSource({
                            transport: {
                                read: function (options) {
                                    var cFilter = new AllFilter();
                                    cFilter.take = OrderSummaryDays;
                                    $.ajax({
                                        url: 'api/CarrierSchedulerGrouped/GetCarrierOrderSummaryForChartGrouped',
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
                                                    if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Get Carrier Order Summary Grouped Failure", data.Errors, null); }
                                                    else {
                                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) { blnSuccess = true; }
                                                            else { blnSuccess = true; strValidationMsg = "No records were found matching your search criteria"; }
                                                        }
                                                    }
                                                }
                                                if (blnSuccess === false && blnErrorShown === false) {
                                                    if (strValidationMsg.length < 1) { strValidationMsg = "Get Carrier Order Summary Grouped Failure"; }
                                                    ngl.showErrMsg("Get Carrier Order Summary Grouped Failure", strValidationMsg, null);
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
                            error: function (xhr, textStatus, error) { ngl.showErrMsg("Access Carrier Order Summary Grouped Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges(); }
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
                                    {field: "delivery", categoryField: "OrderDays", name: "delivery"}
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



                        collapseUOPFltr();
                        collapseUODFltr();
                        collapseBAPFltr();
                        collapseBADFltr();
                                    
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
