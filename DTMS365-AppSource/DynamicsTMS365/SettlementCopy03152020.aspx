<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SettlementCopy03152020.aspx.cs" Inherits="DynamicsTMS365.SettlementCopy03152020" %>

<!DOCTYPE html>

<html>
    <head >
        <title>DTMS Settlement</title>
         <%=cssReference%>  
        <style>
            html,
            body {height:100%; margin:0; padding:0;}

            html {font-size: 12px; font-family: Arial, Helvetica, sans-serif; overflow:hidden;}

        </style>
    </head>
    <body>                   
        <%=jssplitter2Scripts%> 
        <%=sWaitMessage%>    
      <div id="example" style="height: 100%; width: 100%;  margin-top: 2px;">
             <div id="vertical" style="height: 98%; width: 98%; " >                 
                <div id="menu-pane" style="height: 100%; width: 100%; background-color: white; ">
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
                            <div class="k-block k-info-colored" style="margin:5px;">Click&nbsp;<span class="k-icon k-i-pencil" style="color: blue;"></span>&nbsp;to enter the freight charges or Click&nbsp;<span class="k-icon k-i-info-circle" style="color: blue;"></span>&nbsp;to review the status of a previous transaction</div>                            
                            <!-- begin Page Content -->
                            <% Response.Write(FastTabsHTML); %>    
                            <!-- End Page Content -->

                            <input id="txtSHID" type="hidden" />
                            <input id="txtHeaderBookControl" type="hidden" />
                            <input id="txtStatus" type="hidden" />
                            <input id="txtShowPendingFeeFailReason" type="hidden" />
                            <input id="txtAPControl" type="hidden" />
                        </div>
                    </div>
                </div>
                <div id="bottom-pane" class="k-block" style="height: 100%; width: 100%;">
                    <div class="pane-content">
                        <% Response.Write(PageFooterHTML); %> 
                    </div>
                </div>
            </div>

          <% Response.WriteFile("~/Views/ExtFBDataEntryWindowCopy03152020.html"); %>          

          <div id="wndQuickEdit">
              <div>
                  <a id="focusCancel" href="#"></a>
                  <div id="QuickEditGrid"></div>
              </div>
          </div>

          <script type="text/x-kendo-template" id="FBSHIDFeetemplate">
              <div class="tabstrip">
                  <ul>
                    <li class="k-active">Fees</li>
                  </ul>
                  <div>
                    <div class="fees"></div>
                  </div>
              </div>
          </script>

    <% Response.Write(PageTemplates); %>
      
    <%-- HelpWindow needs to go after AuthNotification because it uses the notification object created there --%>
    <% Response.Write(AuthLoginNotificationHTML); %>   
    <% Response.WriteFile("~/Views/HelpWindow.html"); %>       
    <script>
        <% Response.Write(ADALPropertiesjs); %>

        var PageControl = '<%=PageControl%>'; 
        var control = <%=UserControl%>; 
        var tPage = this;
        var tObjPG = this;
        ngl.UserValidated(true,control,oredirectUri);
        var oSettlementGrid = null;
        var tObj = this;        

        <% Response.Write(PageCustomJS); %>
        
        ////var settlementItems = kendo.data.DataSource; 
        var dsFBSHIDGrid = kendo.data.DataSource;
        var dsFeesDropDown = kendo.data.DataSource;
        var dsQuickEdit = kendo.data.DataSource;
        var wndExtFBDataEntry = kendo.ui.Window;
        var wndQuickEdit = kendo.ui.Window;

        var StopFees = new Array();
        var intStopFeeIndex = 0;
        var sEditAccessorialsMessage = '<% Response.Write(EditAccessorialsMessage); %>';
        var sReadOnlyAccessorialsMessage = '<% Response.Write(ReadOnlyAccessorialsMessage); %>';
        /*
        * New Settlement Rules
        *   1. Allow carrier updates to freight bill when booking record's PExp (bookpaycode) is in N or PA 
        *   2. Do not display Fee Status to carrier
        *   3. do not use Fee Status to determine what is editable.
        *   4. The carrier fee Approve Fees page needs to show all fees when a freight bill has not passed audit. in M or PA status.
        *           a) in 8.2 Rob will add the ability to edit the line haul, discount and BFC directly on this page.
        * 
        * As of 5/16/18 v-8.1
        */
            
        //*************  Action Menu Function **********************
        function execActionClick(btn, proc){
            if (btn.id == "btnSettlementQuickEdit"){ refreshQuickEditGrid(); wndQuickEdit.center().open(); }
            else if (btn.id == "btnRefresh" ){ refresh(); }
            else if (btn.id == "btnResetCurrentUserConfig"){ resetCurrentUserConfig(PageControl); }
        }

        function refresh() {    
            ngl.readDataSource(oSettlementGrid);  
            ngl.readDataSource($('#QuickEditGrid').data('kendoGrid'));
        }

        function refreshQuickEditGrid() { ngl.readDataSource($('#QuickEditGrid').data('kendoGrid')); }

        function refreshSettlementGrid() {
            if (typeof (oSettlementGrid) !== 'undefined' && ngl.isObject(oSettlementGrid)) { oSettlementGrid.dataSource.read(); }
        }

        function refreshFBSHIDGrid() { $('#FBSHIDGrid').data('kendoGrid').dataSource.read(); }

        function refreshFeesDropDown() { $('#ddlFees').data('kendoDropDownList').dataSource.read(); }

        //*************  Call Back Function **********************
        function SettlementGridDataBoundCallBack(e,tGrid){
            // get the index of the Status column
            var columns = e.sender.columns;
            var columnIndex = tGrid.wrapper.find(".k-grid-header [data-field=" + "Status" + "]").index();
            oSettlementGrid = tGrid;
            var ds = tGrid.dataSource.data();
            for (var j=0; j < ds.length; j++) {
                if (typeof (ds[j].Status) !== 'undefined' && ds[j].Status != null && ds[j].Status.length > 0) {
                    if (ds[j].Status !== 'N') {
                        var item = tGrid.dataSource.get(ds[j].Control); //Get by ID or any other preferred method
                        //FAILED
                        if (ds[j].Status.toUpperCase() === 'FAILED') {                            
                            //tGrid.tbody.find("tr[data-uid='"+item.uid+"'] .k-grid-Edit").hide(); 
                            //Modified By LVV on 1/12/18 v-8.0 (Task ID 251) - make it readonly (not edit) and change button to Details
                            var btn = tGrid.tbody.find("tr[data-uid='"+item.uid+"'] .k-grid-Edit");
                            btn.html("<span class='k-icon k-i-info-circle'></span>");
                            //Make text red if FAILED
                            var row = e.sender.tbody.find("[data-uid='" + ds[j].uid + "']");
                            var cell = row.children().eq(columnIndex);
                            cell.addClass("red");   
                        }
                        //PENDING
                        if (ds[j].Status.toLowerCase() === 'pending') {
                            //Make text orange in pending status
                            var row = e.sender.tbody.find("[data-uid='" + ds[j].uid + "']");
                            var cell = row.children().eq(columnIndex);
                            cell.addClass("orangered");
                            //Modified By LVV on 5/16/2018 for v-8.1 Settlement Rule 1
                            //////var btn = tGrid.tbody.find("tr[data-uid='"+item.uid+"'] .k-grid-Edit");
                            //////btn.html("<span class='k-icon k-i-info-circle'></span>");
                        }
                    }
                }                
            }
        }  

        
        function GetBookFinAPActWgtBySHIDCallback(response) {
            var tObj = this;       
            //debugger;
            try {
                var blnSuccess = false;
                var blnErrorShown = false;
                var strValidationMsg = "";
                if (typeof (response) !== 'undefined' && ngl.isObject(response)) {               
                    if (typeof (response.Errors) !== 'undefined' && response.Errors != null && response.Errors.length > 0) {
                        var sErrMsg = "Unable to read load shipped weight; the value has been set to zero.  The actual error is: " + response.Errors;
                        $("#txtBookFinAPActWgt").data("kendoNumericTextBox").value(0);                        
                        ngl.showInfoNotification("Read Load Shipped Weight Failure", sErrMsg, tObj);
                    }
                    else {                    
                        if (typeof (response.Data) !== 'undefined' && ngl.isArray(response.Data)) {                       
                            var rData = response.Data;
                            if (typeof (rData) !== 'undefined' && ngl.isObject(rData) && ngl.isArray(rData)) {
                                $("#txtBookFinAPActWgt").data("kendoNumericTextBox").value(rData[0]);                                                               
                            }                        
                        }
                    }
                    calcSettlementTotals();
                } 
            } catch (err) {
                var sErrMsg = "Unable to read load shipped weight; the value has been set to zero.  The actual error is: " + err.description;
                $("#txtBookFinAPActWgt").data("kendoNumericTextBox").value(0);  
                ngl.showErrMsg("Read Existing Carrier Cost Failure", sErrMsg, tObj);
            }           
        }

        function GetBookFinAPActWgtBySHIDAjaxErrorCallback(xhr, textStatus, error) {
            var tObj = this;        
            //debugger;
            var sErrMsg = "Unable to readload shipped weight; the  value has been set to zero.  The actual error is: " + formatAjaxJSONResponsMsgs(xhr, textStatus, error, 'Failed to Read Load Shipped Weight');
            $("#txtBookFinAPActWgt").data("kendoNumericTextBox").value(0);  
            ngl.showErrMsg("Read Existing Carrier Cost Failure", sErrMsg, tObj);
        }


        function GetAPCarrierCostCallback(response) {
            var tObj = this;       
             //debugger;
            try {
                var blnSuccess = false;
                var blnErrorShown = false;
                var strValidationMsg = "";
                if (typeof (response) !== 'undefined' && ngl.isObject(response)) {               
                    if (typeof (response.Errors) !== 'undefined' && response.Errors != null && response.Errors.length > 0) {
                        var sErrMsg = "Unable to read Line Haul; the value has been set to zero.  The actual error is: " + response.Errors;
                        $("#txtLineHaul").data("kendoNumericTextBox").value(0);
                        $("#txtAPControl").val(0);
                        ngl.showInfoNotification("Read Carrier Line Haul Failure", sErrMsg, tObj);
                    }
                    else {                    
                        if (typeof (response.Data) !== 'undefined' && ngl.isArray(response.Data)) {                       
                            var rData = response.Data;
                            if (typeof (rData) !== 'undefined' && ngl.isObject(rData) && ngl.isArray(rData)) {
                                $("#txtLineHaul").data("kendoNumericTextBox").value(rData[0]); 
                                if (rData.length > 1){ $("#txtAPControl").val(rData[1]); } else { $("#txtAPControl").val(0); }                                
                            }                        
                        }
                    }
                    calcSettlementTotals();
                } 
            } catch (err) {
                var sErrMsg = "Unable to read Line Haul; the value has been set to zero.  The actual error is: " + err.description;
                $("#txtLineHaul").data("kendoNumericTextBox").value(0);
                ngl.showErrMsg("Read Carrier Line Haul Failure", sErrMsg, tObj);
            }           
        }

        function GetAPCarrierCostAjaxErrorCallback(xhr, textStatus, error) {
            var tObj = this;        
           // debugger;
            var sErrMsg = "Unable to read Line Haul; the value has been set to zero.  The actual error is: " + formatAjaxJSONResponsMsgs(xhr, textStatus, error, 'Failed to Read Carrier Line Haul');
            $("#txtLineHaul").data("kendoNumericTextBox").value(0);
            ngl.showErrMsg("Read Existing Carrier Cost Failure", sErrMsg, tObj);
        }

        function GetSettlementFuelForSHIDCB(response) {
            var tObj = this;       
            //debugger;
            try {
                var blnSuccess = false;
                var blnErrorShown = false;
                var strValidationMsg = "";
                if (typeof (response) !== 'undefined' && ngl.isObject(response)) {               
                    if (typeof (response.Errors) !== 'undefined' && response.Errors != null && response.Errors.length > 0) {
                        var sErrMsg = "Unable to read total fuel cost; the value has been set to zero.  The actual error is: " + response.Errors;
                        $("#txtTotalFuel").data("kendoNumericTextBox").value(0);
                        ngl.showInfoNotification("Read Carrier Fuel Cost Failure", sErrMsg, tObj);
                    }
                    else {                    
                        if (typeof (response.Data) !== 'undefined' && ngl.isArray(response.Data)) {                       
                            var rData = response.Data;
                            if (typeof (rData) !== 'undefined' && ngl.isObject(rData) && ngl.isArray(rData)) {
                                $("#txtTotalFuel").data("kendoNumericTextBox").value(rData[0]);                              
                            }                        
                        }
                    }
                    calcSettlementTotals()
                } 
            } catch (err) {
                var sErrMsg = "Unable to read total fuel cost; the value has been set to zero.  The actual error is: " + err.description;
                $("#txtTotalFuel").data("kendoNumericTextBox").value(0);
                ngl.showErrMsg("Read Carrier Fuel Cost Failure", sErrMsg, tObj);
            }          
        }

        function GetSettlementFuelForSHIDAjaxErrorCB(xhr, textStatus, error) {
            var tObj = this;        
            //debugger;
            var sErrMsg = "Unable to read total fuel cost; the value has been set to zero.  The actual error is: " + formatAjaxJSONResponsMsgs(xhr, textStatus, error, 'Failed to Read Carrier Fuel Cost');
            $("#txtTotalFuel").data("kendoNumericTextBox").value(0);
            ngl.showErrMsg("Read Carrier Fuel Cost Failure", sErrMsg, tObj);
        }


        //********** ExtFBDEWindow Code Start **********

        function calcSettlementTotals(){
            var len = 0;
            var totalFees = 0;
            var totalCost = 0;
            var lineHaul = 0;
            var totalFuel = 0
            if (typeof (StopFees) !== 'undefined' && ngl.isArray(StopFees)){ var len = StopFees.length; }
            if (len > 0){
                for (var j=0; j < len; j++) {
                    totalFees += StopFees[j].Cost;                                           
                }  
            }           
            var lineHaulTextbox = $("#txtLineHaul").data("kendoNumericTextBox");
            if (lineHaulTextbox && lineHaulTextbox.value() != null){ lineHaul = lineHaulTextbox.value(); }
            var FuelTextbox = $("#txtTotalFuel").data("kendoNumericTextBox");
            if (FuelTextbox && FuelTextbox.value() != null){ totalFuel = FuelTextbox.value(); }
            totalCost = totalFees + lineHaul + totalFuel
            if ($("#txtTotalFees")){ $("#txtTotalFees").data("kendoNumericTextBox").value(totalFees); }
            if ($("#txtTotalCost")){ $("#txtTotalCost").data("kendoNumericTextBox").value(totalCost); }          
            return;
        }

        function calcSettlementTotalsWithFees(totalFees){
            var len = 0;
            var totalCost = 0;
            var lineHaul = 0;
            var totalFuel = 0
            if (typeof (totalFees) === 'undefined' || totalFees == null){ totalFees = 0; }                   
            var lineHaulTextbox = $("#txtLineHaul").data("kendoNumericTextBox");
            if (lineHaulTextbox && lineHaulTextbox.value() != null){ lineHaul = lineHaulTextbox.value(); }
            var FuelTextbox = $("#txtTotalFuel").data("kendoNumericTextBox");
            if (FuelTextbox && FuelTextbox.value() != null){ totalFuel = FuelTextbox.value(); }
            totalCost = totalFees + lineHaul + totalFuel
            if ($("#txtTotalFees")){ $("#txtTotalFees").data("kendoNumericTextBox").value(totalFees); }
            if ($("#txtTotalCost")){ $("#txtTotalCost").data("kendoNumericTextBox").value(totalCost); }           
            return;
        }

        function openExtFBDEWindow(e) {
            //debugger;
            var item = this.dataItem($(e.currentTarget).closest("tr"));
            if (typeof (item) !== 'undefined' && ngl.isObject(item)) {
                if (typeof (item.SHID) !== 'undefined' && item.SHID != null && item.SHID.length > 0) {
                    wndExtFBDataEntry.center().open();
                    kendo.ui.progress(wndExtFBDataEntry.element, true);
                    $("#txtSHID").val(item.SHID);
                    $("#txtAPControl").val(0);
                    $("#txtHeaderBookControl").val(item.Control);
                    //set Invoice Amt and Invoice No from the values in the Grid if they already exist
                    $("#txtLineHaul").data("kendoNumericTextBox").value(0);
                    $("#txtTotalFuel").data("kendoNumericTextBox").value(0);
                    $("#txtInvoiceNumber").data("kendoMaskedTextBox").value(item.InvoiceNumber);                 
                    $("#txtTotalCost").data("kendoNumericTextBox").value(item.InvoiceAmount);
                    $("#txtBookFinAPActWgt").data("kendoNumericTextBox").value(item.BookFinAPActWgt);
                    $("#txtBookCarrBLNumber").data("kendoMaskedTextBox").value(item.BookCarrBLNumber);               
                    var tObj =  window;    
                    var filter = new AllFilter();
                    filter.filterName = 'BookSHID';
                    filter.filterValue = item.SHID;
                    filter.filterFrom = '';
                    filter.filterTo = ''
                    filter.sortName = '';
                    filter.sortDirection = '';
                    filter.page = 1;
                    filter.skip = 0;
                    filter.take = 50;                           
                    var oCRUDCtrl = new nglRESTCRUDCtrl();
                    //debugger;
                    var blnRet = oCRUDCtrl.filteredRead("Settlement/GetBookFinAPActWgtBySHID", filter, tObj, "GetBookFinAPActWgtBySHIDCallback", "GetBookFinAPActWgtBySHIDAjaxErrorCallback");
                    var blnRet = oCRUDCtrl.filteredRead("Settlement/GetAPCarrierCost", filter, tObj, "GetAPCarrierCostCallback", "GetAPCarrierCostAjaxErrorCallback");
                    var blnRet = oCRUDCtrl.filteredRead("Settlement/GetSettlementFuelForSHID", filter, tObj, "GetSettlementFuelForSHIDCB", "GetSettlementFuelForSHIDAjaxErrorCB");              
                    //Load the Grid for this SHID
                    refreshFBSHIDGrid();
                    var strStatus = "";
                    //NUll status functions the same as N 
                    if (typeof (item.Status) === 'undefined' || item.Status == null) { strStatus = "N"; } else{ strStatus = item.Status; }
                    $("#txtStatus").val(strStatus);
                    //CheckAuditMessageVisibility();                    
                    //Modified By LVV on 1/12/18 v-8.0 (Task ID 251) - Include check for FAILED status 
                    //if (strStatus.toLowerCase() === 'pending' || strStatus.toUpperCase() === 'FAILED') {    
                    //Modified By LVV on 5/16/2018 for v-8.1 Settlement Rule 1
                    if (strStatus.toUpperCase() === 'FAILED') {
                        //disable the fields
                        $("#txtInvoiceNumber").data("kendoMaskedTextBox").enable(false);
                        $("#txtBookCarrBLNumber").data("kendoMaskedTextBox").enable(false);
                        $("#txtBookFinAPActWgt").data("kendoNumericTextBox").enable(false);
                        $("#txtLineHaul").data("kendoNumericTextBox").enable(false);
                        $("#txtTotalFuel").data("kendoNumericTextBox").enable(false);
                        $("#txtTotalFees").data("kendoNumericTextBox").enable(false);
                        $("#txtTotalCost").data("kendoNumericTextBox").enable(false);
                        $("#wndExtFBDataEntry").data("kendoWindow").wrapper.find(".k-svg-i-save").hide();
                        $("#spwndExtFBEditMsg").html(sReadOnlyAccessorialsMessage);
                    }
                    else{
                        $("#txtInvoiceNumber").data("kendoMaskedTextBox").enable(true);
                        $("#txtBookCarrBLNumber").data("kendoMaskedTextBox").enable(true);
                        $("#txtBookFinAPActWgt").data("kendoNumericTextBox").enable(true);
                        $("#txtLineHaul").data("kendoNumericTextBox").enable(false);
                        $("#txtTotalFuel").data("kendoNumericTextBox").enable(true);
                        $("#wndExtFBDataEntry").data("kendoWindow").wrapper.find(".k-svg-i-save").show();
                        $("#spwndExtFBEditMsg").html(sEditAccessorialsMessage);
                    }
                }
                else{
                    ngl.showErrMsg("Required Field", "Cannot Open Extended Freight Bill Data Entry Screen because an SHID is required", null);
                    kendo.ui.progress(wndExtFBDataEntry.element, false);
                    wndExtFBDataEntry.close();
                }
            }
        }     

        function getExistingSHIDFees(){
            var SHID = $("#txtSHID").val();
            $.ajax({
                async: false,
                url: 'api/BookFee/GetSettlementFeesForSHID',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: { filter: SHID },
                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                success: function(data) {
                    try {
                        kendo.ui.progress(wndExtFBDataEntry.element, false);

                        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                            if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                blnErrorShown = true;
                                ngl.showErrMsg("GetSettlementFeesForSHID Failure", data.Errors, null);
                            }
                            else {
                                if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                    if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {                                                     
                                        blnSuccess = true;

                                        StopFees.push.apply(StopFees, data.Data); 
                                        intStopFeeIndex = StopFees.length;

                                        var len = StopFees.length;
                                        var totalFees = 0;
                                        var totalCost = 0;

                                        for (var j=0; j < len; j++) {
                                            totalFees += StopFees[j].Cost;                                           
                                        }
                                        totalCost = totalFees + $("#txtLineHaul").data("kendoNumericTextBox").value();
                                        $("#txtTotalFees").data("kendoNumericTextBox").value(totalFees);
                                        $("#txtTotalCost").data("kendoNumericTextBox").value(totalCost);  
                                        try{                                            
                                            var oGrid = $("#FBSHIDGrid").data("kendoGrid");
                                            $( ".k-master-row" ).each(function( index ) {
                                                oGrid.expandRow(this);
                                            });
                                        }  catch (err) {
                                            //do nothing
                                        }                                  
                                    }
                                }
                            }
                        }
                    } catch (err) {
                        ngl.showErrMsg(err.name, err.description, null);
                    }
                },
                error: function (xhr, textStatus, error) { 
                    var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure");
                    ngl.showErrMsg("GetSettlementFeesForSHID JSON Response Error", sMsg, null);   
                    kendo.ui.progress(wndExtFBDataEntry.element, false);
                }
            });
        }

        function CheckAuditMessageVisibility(){
            /// <summary>
            /// GenericResult.Control = CompControl
            /// GenericResult.intField1 = CarrierControl
            /// GenericResult.strField = SHID
            /// </summary>
            /// <param name="filter"></param>
            /// <returns>
            /// GenericResult.blnField1 = ShowAuditFailReason
            /// GenericResult.blnField2 = ShowPendingFeeFailReason
            /// GenericResult.strField = APMessage
            /// </returns>
            
            var s = new GenericResult();
            s.Control = $("#txtLastStopComp").val();
            s.intField1 = $("#txtLastStopCarrier").val();
            s.strField = $("#txtSHID").val();

            $.ajax({
                async: false,
                url: 'api/Settlement/GetAuditMessageVisibility',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: { filter: JSON.stringify(s) },
                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                success: function(data) {
                    try {
                        var blnSuccess = false;
                        var blnErrorShown = false;
                        var strValidationMsg = "";
                        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                            if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                blnErrorShown = true;
                                ngl.showErrMsg("GetAuditMessageVisibility Failure", data.Errors, null);
                            }
                            else {
                                if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                    if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                        blnSuccess = true;
                                        /// <returns>
                                        /// GenericResult.blnField1 = ShowAuditFailReason
                                        /// GenericResult.blnField2 = ShowPendingFeeFailReason
                                        /// GenericResult.strField = APMessage
                                        /// </returns>

                                        $("#txtShowPendingFeeFailReason").val(data.Data[0].blnField2);
                                        if (data.Data[0].blnField1 === false){
                                            //If ShowAuditFailReason = false hide the div
                                            $("#divAuditFailMsg").html("");
                                            $("#divAuditFailMsg").hide();
                                        }
                                        else{
                                            //ShowAuditFailReason = true 
                                            var apMsg = data.Data[0].strField;
                                            if(typeof (apMsg) !== 'undefined' && apMsg != null && apMsg.length > 0){
                                                //If APMessage != null show the msg and div
                                                $("#divAuditFailMsg").html("<p>" + apMsg + "</p>");
                                                $("#divAuditFailMsg").show();
                                            }
                                            else{
                                                //If APMessage = null hide the div
                                                $("#divAuditFailMsg").html("");
                                                $("#divAuditFailMsg").hide();
                                            }                                
                                        }
                                    }
                                }
                            }
                        }
                        if (blnSuccess === false && blnErrorShown === false) {
                            if (strValidationMsg.length < 1) { strValidationMsg = "Book Fees Pending not found"; }
                            ngl.showErrMsg("Get Book Fees Pending Failure", strValidationMsg, null);
                        }
                    } catch (err) {
                        ngl.showErrMsg(err.name, err.description, null);
                    }
                },
                error: function (xhr, textStatus, error) { 
                    var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure");
                    ngl.showErrMsg("GetSettlementFeesForSHID JSON Response Error", sMsg, null);   
                    kendo.ui.progress(wndExtFBDataEntry.element, false);
                }
            });
        }

        //this overloaded function does not close the window when the save is finished
        // and it calls calcSettlementTotals on success
        function saveExtFBDEFuel(){
            calcSettlementTotals();            
            // old ajax message removed by RHR on 8/26/19 left as an example of message formats
            //var sMsg =  ngl.replaceEmptyString(data.Data[0].SuccessTitle,'', '<br>') 
            //            +  ngl.replaceEmptyString(data.Data[0].SuccessMsg,'');
            //if (ngl.isNullOrWhitespace(sMsg) == false)
            //{
            //    sMsg = sMsg + '<br>';
            //}
            //sMsg = sMsg +  ngl.replaceEmptyString(data.Data[0].WarningTitle,'','<br>') 
            //            +  ngl.replaceEmptyString(data.Data[0].WarningMsg,'');
            //$("#divAuditFailMsg").html("<p>" + sMsg + "</p>");                
            //if (ngl.stringHasValue(data.Data[0].ErrMsg)){ 
            //    ngl.showErrMsg(ngl.replaceEmptyString(data.Data[0].ErrTitle,'Save Fuel Error'), data.Data[0].ErrMsg, tObj);
            //}                                                       
        }
     
        function SaveExtFBDE(){
            var otmp = $("#focusCancelFB").focus();
            //if (StopFees.length > 0){
            var s = new SettlementSave();
            // ** TODO LVV ** FINISH POPULATING NEW FIELDS ADDED TO SettlementSave OBJECT
            s.InvoiceNo = $("#txtInvoiceNumber").data("kendoMaskedTextBox").value();
            if (isEmpty(s.InvoiceNo) == true){
                ngl.showWarningMsg ("Cannot Save Changes", "Data Validation Failure.  The Freight Bill Invoice Number is required and cannot be empty", tObj) ;
                return;
            }
            kendo.ui.progress(wndExtFBDataEntry.element, true);
            setTimeout(function (tObj) {                                
                s.InvoiceAmt = $("#txtTotalCost").data("kendoNumericTextBox").value();                
                s.LineHaul = $("#txtLineHaul").data("kendoNumericTextBox").value();
                s.TotalFuel = $("#txtTotalFuel").data("kendoNumericTextBox").value();
                s.BookCarrBLNumber = $("#txtBookCarrBLNumber").data("kendoMaskedTextBox").value();
                s.BookFinAPActWgt = $("#txtBookFinAPActWgt").data("kendoNumericTextBox").value();
                s.Fees = StopFees;
                s.BookSHID = $("#txtSHID").val();
                s.BookControl = $("#txtHeaderBookControl").val();
                s.CarrierControl = $("#txtLastStopCarrier").val();
                s.CompControl = $("#txtLastStopComp").val();
                $.ajax({
                    async: false,
                    type: "POST",
                    url: "api/Settlement/SettlementSave",
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    data: JSON.stringify(s),
                    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                    success: function (data) {     
                        try {
                            kendo.ui.progress(wndExtFBDataEntry.element, false);
                            // ** TODO LVV ** FINISH WRITING THIS CODE BASED ON CONTROLLER RETURN OBJECT
                            var blnSuccess = false;
                            var blnErrorShown = false;
                            var strValidationMsg = "";
                            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {                                
                                if (ngl.isNullOrWhitespace(data.Data[0].SuccessMsg) == false){
                                    var sSuccessMsg =  ngl.replaceEmptyString(data.Data[0].SuccessTitle,'', '<br>') +  ngl.replaceEmptyString(data.Data[0].SuccessMsg,'');
                                    ngl.showSuccessMsg(sSuccessMsg,tObj);
                                }
                                if (ngl.isNullOrWhitespace(data.Data[0].WarningMsg) == false){
                                    ngl.showWarningMsg(ngl.replaceEmptyString(data.Data[0].WarningTitle,'Save Settlement Warning'),data.Data[0].WarningMsg,tObj);
                                }          
                                if (ngl.isNullOrWhitespace(data.Data[0].ErrMsg)  == false){ 
                                    ngl.showErrMsg(ngl.replaceEmptyString(data.Data[0].ErrTitle,'Save Settlement Error'), data.Data[0].ErrMsg, tObj);
                                }                            
                                refreshSettlementGrid(); 
                                kendo.ui.progress(wndExtFBDataEntry.element, false);
                                wndExtFBDataEntry.close();
                            } else {                           
                                if (strValidationMsg.length < 1) { strValidationMsg = "There was a problem while executing Settlement Save"; }
                                ngl.showErrMsg("Settlement Save Failure", strValidationMsg, null);
                            }
                        } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                    },
                    error: function (xhr, textStatus, error) {
                        kendo.ui.progress(wndExtFBDataEntry.element, false);
                        var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure");
                        ngl.showErrMsg("Settlement Save Failure", sMsg, null);                        
                    }
                });
            }, 200, tObj);
            //}
        }
        //********** ExtFBDEWindow Code End **********

        

        function SaveQuickEdit(){
            //var grid = $("#QuickEditGrid").data("kendoGrid")
            var otmp = $("#focusCancel").focus();
            var dsItems = dsQuickEdit.data();
            kendo.ui.progress(wndQuickEdit.element, true);
            setTimeout(function (tObj) { 
                $.ajax({
                    async: false,
                    type: "POST",
                    url: "api/Settlement/SettlementQuickSave",
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    data: JSON.stringify(dsItems),
                    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                    success: function (data) {     
                        try {
                            kendo.ui.progress(wndQuickEdit.element, false);
                            var blnSuccess = false;
                            var blnErrorShown = false;
                            var strValidationMsg = "";
                            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Settlement Quick Save Failure", data.Errors, null); }
                                else {
                                    if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                        if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                            blnSuccess = true;                                       
                                            if (ngl.stringHasValue(data.Data[0].strField)) { ngl.showSuccessMsg(data.Data[0].strField, null); } //success Msg                                      
                                            if (ngl.stringHasValue(data.Data[0].strField2)) { ngl.showInfoNotification("Warning", data.Data[0].strField2, null); } //Warning/Info Msg                                     
                                            if (ngl.stringHasValue(data.Data[0].strField3)) { ngl.showErrMsg("Error", data.Data[0].strField3, null); } //Error Msg
                                            refresh();                                                         
                                        }
                                    }
                                }
                            }
                            if (blnSuccess === false && blnErrorShown === false) {
                                if (strValidationMsg.length < 1) { strValidationMsg = "There was a problem while executing Settlement Quick Save"; }
                                ngl.showErrMsg("Settlement Quick Save Failure", strValidationMsg, null);
                            }
                        } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                    },
                    error: function (xhr, textStatus, error) { kendo.ui.progress(wndQuickEdit.element, false); var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure"); ngl.showErrMsg("Settlement Quick Save Failure", sMsg, null); }
                });
            }, 200, tObj);
        }
        
        function getFeeByIndex(idx) {
            var l = StopFees.length;
            for (var j=0; j < l; j++) {
                if (StopFees[j].FeeIndex == idx) {
                    return StopFees[j];
                }
            }
            return null;
        }

        function getIndexByFeeIndex(feeIndex) {
            var l = StopFees.length;
            for (var j=0; j < l; j++) {
                if (StopFees[j].FeeIndex == feeIndex) {
                    return j;
                }
            }
            return null;
        }

        function FeeDropDownEditor(container, options) {
            $('<input required data-text-field="Caption" data-value-field="AccessorialCode" data-bind="value:' + options.field + '"/>')
                .appendTo(container)
                .kendoDropDownList({
                    dataTextField: "Caption",
                    dataValueField: "AccessorialCode",
                    autoBind: true,
                    dataSource: dsFeesDropDown,
                    //optionLabel: {
                    //    Caption: "--Select Value--",
                    //    AccessorialCode: 0
                    //},
                    change: function (e) {
                        var fee = this.dataItem(e.item);
                        options.model.set("AccessorialCode", fee.AccessorialCode);
                        options.model.set("Caption", fee.Caption);
                    }
                });
        }



        $(function () {
            //wire focus of all numerictextbox widgets on the page
            $("input[type=number]").bind("focus", function () {
                var input = $(this);
                clearTimeout(input.data("selectTimeId")); //stop started time out if any
                var selectTimeId = setTimeout(function() { input.select(); });
                input.data("selectTimeId", selectTimeId);
            }).blur(function(e) {
                clearTimeout($(this).data("selectTimeId")); //stop started timeout
            });
        });

        $(document).ready(function () {
            var PageMenuTab = <%=PageMenuTab%>;
            control = <%=UserControl%>;                     
            if (ngl.UserValidated(true,control,oredirectUri)){ return; }

            if (control != 0){
                $("#txtSHID").val("");              
                $("#txtLastStopComp").val(0);
                $("#txtLastStopCompLE").val("");
                $("#txtLastStopCarrier").val(0);
                $("#txtHeaderBookControl").val(0);
                
                //txtBookCarrBLNumber txtBookFinAPActWgt txtInvoiceAmount txtInvoiceNumber
                $("#txtInvoiceNumber").kendoMaskedTextBox();
                $("#txtBookCarrBLNumber").kendoMaskedTextBox();
                $("#txtBookFinAPActWgt").kendoNumericTextBox();               
                $("#txtLineHaul").kendoNumericTextBox({ format: "{0:c2}", spinners: false});
                var otxtLineHaul = $("#txtLineHaul").data("kendoNumericTextBox");
                otxtLineHaul.readonly();
                $("#txtTotalFuel").kendoNumericTextBox({
                    format: "{0:c2}",
                    change: function(e) { saveExtFBDEFuel(); }
                });  
                //$("#txtTotalFuel").kendoNumericTextBox({ format: "{0:c2}",  });
                $("#txtTotalFees").kendoNumericTextBox({ format: "{0:c2}", spinners: false });
                $("#txtTotalCost").kendoNumericTextBox({ format: "{0:c2}", spinners: false });
                
                //clear all the values from the filters
                $("#txtInvoiceNumber").data("kendoMaskedTextBox").value("");
                $("#txtBookCarrBLNumber").data("kendoMaskedTextBox").value("");
                $("#txtBookFinAPActWgt").data("kendoNumericTextBox").value(0);
                $("#txtLineHaul").data("kendoNumericTextBox").value(0);
                $("#txtTotalFees").data("kendoNumericTextBox").value(0);
                $("#txtTotalCost").data("kendoNumericTextBox").value(0);
                
                //disable the total fields
                $("#txtTotalFees").data("kendoNumericTextBox").enable(false);
                $("#txtTotalCost").data("kendoNumericTextBox").enable(false);
                
                //***** BEGIN FBSHIDGrid CODE *****
                dsFBSHIDGrid = new kendo.data.DataSource({ 
                    serverSorting: true,
                    serverPaging: true,
                    pageSize: 10,
                    transport: { 
                        read: function(options) {                           
                            var SHID = $("#txtSHID").val();
                            $.ajax({
                                url: 'api/Settlement/GetFBSHIDGrid365',
                                contentType: 'application/json; charset=utf-8',
                                dataType: 'json',
                                data: { filter: SHID },
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                success: function(data) {
                                    try {
                                        // notify the data source that the request succeeded
                                        options.success(data);

                                        var blnSuccess = false;
                                        var blnErrorShown = false;
                                        var strValidationMsg = "";
                                        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                            if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                                blnErrorShown = true;
                                                ngl.showErrMsg("GetFBSHIDGrid365 Failure", data.Errors, null);
                                            }
                                            else {
                                                if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                    if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {                                                     
                                                        blnSuccess = true;
                                                        var ds = data.Data;

                                                        //Get the data from the Last Stop
                                                        var lastStop = ds[ds.length - 1];
                                                        $("#txtLastStopComp").val(lastStop.BookCustCompControl);
                                                        $("#txtLastStopCompLE").val(lastStop.CompLegalEntity);
                                                        $("#txtLastStopCarrier").val(lastStop.BookCarrierControl);

                                                        //Refresh the list of Fees based on data from Last Stop
                                                        dsFeesDropDown.read();
                                                        
                                                        //clear the rest of the values
                                                        $("#txtTotalFees").data("kendoNumericTextBox").value(0);    
                                                        intStopFeeIndex = 0;
                                                        StopFees = [];
                                                        //get any existing fees from BookFees and BookFeesPending tables
                                                        getExistingSHIDFees();

                                                        CheckAuditMessageVisibility();
                                                                                                                                                            
                                                    }
                                                }
                                            }
                                        }
                                        if (blnSuccess === false && blnErrorShown === false) {
                                            if (strValidationMsg.length < 1) { strValidationMsg = "Book record not found"; }
                                            ngl.showErrMsg("GetFBSHIDGrid365 Failure", strValidationMsg, null);
                                            kendo.ui.progress(wndExtFBDataEntry.element, false);
                                        }
                                    } catch (err) {
                                        ngl.showErrMsg(err.name, err.description, null);
                                        kendo.ui.progress(wndExtFBDataEntry.element, false);
                                    }
                                },
                                error: function (xhr, textStatus, error) { 
                                    var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure");
                                    ngl.showErrMsg("GetFBSHIDGrid365 JSON Response Error", sMsg, null);  
                                    this.cancelChanges(); 
                                    kendo.ui.progress(wndExtFBDataEntry.element, false);
                                }
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
                                BookConsPrefix: { type: "string" },
                                BookProNumber: { type: "string" },  
                                BookCarrOrderNumber: { type: "string" },  
                                BookStopNo: { type: "number" },
                                BookSHID: { type: "string" },
                                BookFinAPActWgt: { type: "number" },
                                BookCarrBLNumber: { type: "string" },
                                CompName: { type: "string" },
                                CarrierName: { type: "string" },
                                CompLegalEntity: { type: "string" }
                            }
                        }, 
                        errors: "Errors" 
                    }, 
                    error: function (xhr, textStatus, error) { 
                        var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure");
                        ngl.showErrMsg("GetBooksBySHID JSON Response Error", sMsg, null);  
                        this.cancelChanges(); 
                    }                   
                });
            
                $('#FBSHIDGrid').kendoGrid({ 
                    autoBind: false,
                    dataSource: dsFBSHIDGrid, 
                    sortable: false,
                    pageable: false, 
                    resizable: true, 
                    groupable: false, 
                    columns: [
                        {field: "BookControl", title: "Control", hidden: true},
                        {field: "BookConsPrefix", title: "CNS"},
                        {field: "BookProNumber", title: "Pro Number"},
                        {field: "BookCarrOrderNumber", title: "Order No"},
                        {field: "BookStopNo", title: "Stop No"},
                        {field: "BookSHID", title: "SHID"},
                        {field: "BookFinAPActWgt", title: "Load Shipped Wgt"},
                        {field: "BookCarrBLNumber", title: "BOL Number"}
                    ],
                    dataBound: function() {
                        //this.expandRow(this.tbody.find("tr.k-master-row"));                        
                    },
                    detailTemplate: kendo.template($("#FBSHIDFeetemplate").html()),
                    detailInit: function(e) {
                        var detailRow = e.detailRow;
                        detailRow.find(".tabstrip").kendoTabStrip({
                            animation: {
                                open: { effects: "fadeIn" }
                            }
                        });
                        detailRow.find(".fees").kendoGrid({
                            dataSource: {
                                autoSync: true,
                                serverPaging: false, 
                                serverSorting: false, 
                                serverFiltering: false,
                                transport: {
                                    read: function(d){
                                        //debugger;
                                        var temp = new Array(); 
                                        //modified by RHR for v-8.2 on 6/6/2019 to handle null StopFees
                                        //not sure if this creates a problem with the temp array logic below
                                        if (typeof (StopFees) === 'undefined' || ngl.isArray(StopFees) === false){
                                            StopFees = [];
                                        }
                                        var l = StopFees.length;
                                        var totalFees = 0;
                                        var totalCost = 0;
                                        //Note: 
                                        for (var j=0; j < l; j++) {
                                            if (StopFees[j].BookControl == e.data.BookControl) { 
                                                temp.push(StopFees[j]);
                                            }
                                            totalFees += StopFees[j].Cost;                                           
                                        }

                                        calcSettlementTotalsWithFees(totalFees);
                                        //modified by RHR for v-8.2 on 6/6/2019
                                        //moved formula to calcSettlementTotalsWithFees function
                                        //totalCost = totalFees + $("#txtLineHaul").data("kendoNumericTextBox").value();
                                        //$("#txtTotalFees").data("kendoNumericTextBox").value(totalFees);
                                        //$("#txtTotalCost").data("kendoNumericTextBox").value(totalCost);
                                        d.success(temp);
                                    },
                                    create: function (d) {
                                        //debugger;
                                        var ddl = dsFeesDropDown.data();
                                        if(ddl.length < 1){
                                            var msg = "No Accessorials are set up for Legal Entity " + e.data.CompLegalEntity + " and Carrier " + e.data.CarrierName; 
                                            if(ngl.isNullOrWhitespace(e.data.CompLegalEntity)){
                                                //$("#txtLastStopCompLE").val()
                                                var msg = "No Accessorials are available for Company " + e.data.CompName + " because the company does not have an associated Legal Entity. Please make sure the Company Legal Entity field in FM TMS is not null.";  
                                            }                            
                                            ngl.showErrMsg("Error", msg, null);
                                            return;
                                        }
                                        intStopFeeIndex += 1;

                                        var item = new SettlementFee();
                                        item.Control = 0;
                                        item.BookControl = e.data.BookControl;
                                        item.Minimum = d.data.Minimum;
                                        item.Cost = d.data.Cost;
                                        item.AccessorialCode = d.data.AccessorialCode;
                                        item.Caption = d.data.Caption;
                                        item.AutoApprove = false;
                                        item.AllowCarrierUpdates = true;
                                        item.FeeIndex = intStopFeeIndex;     
                                        item.Pending = true;  
                                        item.BookCarrOrderNumber = e.data.BookCarrOrderNumber;
                                        // save data item to the original datasource
                                        StopFees.push(item);
                                        d.success(item);
                                    },
                                    update: function(d){
                                        //debugger;
                                        // locate item in original datasource and update it
                                        StopFees[getFeeByIndex(d.data.FeeIndex)] = d.data;                                       
                                        d.success();
                                    },
                                    destroy: function(d) {
                                        //debugger;
                                        if (typeof (d.data.Pending) !== 'undefined' && d.data.Pending != null && d.data.Pending === false) { return; } //Do not allow delete for BookFees
                                      
                                        if (typeof (d.data.Control) !== 'undefined' && d.data.Control != null && d.data.Control > 0) {
                                            //call the function to delete it from db
                                            //just delete the record no fancy stuff required
                                            var feeIndex = d.data.FeeIndex;
                                            var url = 'api/BookFee/DeleteSettlementBFP/' + d.data.Control;

                                            $.ajax({
                                        
                                                contentType: 'application/json; charset=utf-8',
                                                dataType: 'json',
                                                type: "DELETE",
                                                data: { id: d.data.Control },
                                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                                success: function(data) {
                                                    try {
                                                        var blnSuccess = false;
                                                        var blnErrorShown = false;
                                                        var strValidationMsg = "";
                                                        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                                            if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                                                blnErrorShown = true;
                                                                ngl.showErrMsg("Delete Pending Fee Failure", data.Errors, null);
                                                            }
                                                            else {
                                                                if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                                    if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {                                                     
                                                                        blnSuccess = true;

                                                                        StopFees.splice(getIndexByFeeIndex(feeIndex), 1);
                                                                        d.success(StopFees);                                                                                                                                                             
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        if (blnSuccess === false && blnErrorShown === false) {
                                                            if (strValidationMsg.length < 1) { strValidationMsg = "Delete Pending Fee"; }
                                                            ngl.showErrMsg("Delete Pending Fee Failure", strValidationMsg, null);
                                                        }
                                                    } catch (err) {
                                                        ngl.showErrMsg(err.name, err.description, null);
                                                    }
                                                },
                                                error: function (xhr, textStatus, error) { 
                                                    var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Delete Data Failure");
                                                    ngl.showErrMsg("DeleteSettlementBFP JSON Response Error", sMsg, null);  
                                                    this.cancelChanges(); 
                                                }
                                            });

                                        }
                                        else{                                        
                                            StopFees.splice(getIndexByFeeIndex(d.data.FeeIndex), 1);
                                            d.success(StopFees); 
                                        }
                                        
                                    },
                                    parameterMap: function (options, operation) { return options; }
                                },   
                                sync: function(e) {
                                    calcSettlementTotals();
                                    //modified by RHR for v-8.2 on 6/6/2019
                                    //moved formula to calcSettlementTotals function
                                    //var len = StopFees.length;
                                    //var totalFees = 0;
                                    //var totalCost = 0;
                                    
                                    //for (var j=0; j < len; j++) {
                                    //    totalFees += StopFees[j].Cost; 
                                    //}
                                    
                                    //totalCost = totalFees + $("#txtLineHaul").data("kendoNumericTextBox").value();
                                    //$("#txtTotalFees").data("kendoNumericTextBox").value(totalFees);
                                    //$("#txtTotalCost").data("kendoNumericTextBox").value(totalCost);
                                },
                                schema: {
                                    model: {
                                        id: "FeeIndex",
                                        fields: {
                                            FeeIndex: { type: "number", editable: false },
                                            Control: { type: "number", editable: false },
                                            BookControl: { type: "number", editable: false },
                                            Minimum: { type: "number", editable: true  },
                                            Cost: { type: "number", editable: true, validation: { required: true, min: 0} },
                                            AccessorialCode: { type: "number", editable: true },
                                            Caption: { type: "string", editable: true },
                                            AutoApprove: { type: "boolean", editable: false },
                                            AllowCarrierUpdates: { type: "boolean", editable: false },
                                            Pending: { type: "boolean", editable: false },
                                            Msg: { type: "string", editable: false }
                                        }
                                    },
                                    errors: "Errors"
                                }, 
                                error: function (xhr, textStatus, error) {
                                    var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure");
                                    ngl.showErrMsg("GetBookFeesPendingByBookControl Failure", sMsg, null);                        
                                }
                            },
                            toolbar:[{ name: "create", text: "Add Fee" }],
                            resizeable: true, 
                            scrollable: true, 
                            sortable: false, 
                            pageable: false,    
                            editable: true,
                            //editable: "inline",
                            columns: [
                                        {field: "FeeIndex", title: "FeeIndex", hidden: true },
                                        {field: "Control", title: "Control", hidden: true },
                                        {field: "BookControl", title: "Book Control", hidden: true },
                                        {field: "Caption", title: "Fee", hidden: false, editor: FeeDropDownEditor },
                                        {field: "Minimum", title: "Minimum", format: "{0:c2}", hidden: true },
                                        {field: "Cost", title: "Cost", format: "{0:c2}", hidden: false },
                                        {field: "AccessorialCode", title: "AccessorialCode", hidden: true },                                      
                                        {field: "AutoApprove", title: "AutoApprove", hidden: true },
                                        {field: "AllowCarrierUpdates", title: "AllowCarrierUpdates", hidden: true },                                       
                                        {field: "Pending", title: "Fee Status", template: '#= Pending ? "Pending" : "Approved" #', hidden: true }, //Modified By LVV on 5/16/2018 for v-8.1 Settlement Rule 2
                                        {field: "Msg", title: "Msg", hidden: false },
                                        //{ command: ["destroy"], title: "&nbsp;" }
                                        { command: [{className: "cm-icononly-button", name: "destroy", text: "", iconClass: "k-icon k-i-trash" }], title: "Actions", width: "75px" }                                  
                                        //{ command: [{ className: "cm-icononly-button", name: "edit", text:{edit: "", update: "", cancel: ""}},{className: "cm-icononly-button", name: "destroy", text: "" }], title: "Actions", width: "75px" }                                   
                            ],
                            edit: function (e) {
                                //debugger;
                                var status = $("#txtStatus").val();
                                //Modified By LVV on 1/12/18 v-8.0 (Task ID 251) - Include check for FAILED status 
                                //if (typeof (status) !== 'undefined' && status != null && (status.toLowerCase() === 'pending' || status.toUpperCase() === 'FAILED')) {
                                //Modified By LVV on 5/16/2018 for v-8.1 Settlement Rule 1
                                if (typeof (status) !== 'undefined' && status != null && (status.toUpperCase() === 'FAILED')) {
                                    //not editable in FAILED status 
                                    this.closeCell();
                                    return;
                                }

                                var blnIsEditable = true;

                                //Modified By LVV on 5/16/2018 for v-8.1 Settlement Rule 3
                                //If this is a BookFee record (approved) then the user cannot edit the record
                                //var pending = e.model.Pending;
                                //if (typeof (pending) !== 'undefined' && pending != null && pending === false) {
                                //    this.closeCell();
                                //    return;
                                //}

                                //If AllowCarrierUpdates is false then the user cannot edit the record
                                var allowUpdates = e.model.AllowCarrierUpdates;
                                if (typeof (allowUpdates) !== 'undefined' && allowUpdates != null && allowUpdates === false) {
                                    this.closeCell();
                                }
                                else{
                                    var columnIndex = this.cellIndex(e.container);
                                    var fieldName = this.thead.find("th").eq(columnIndex).data("field");

                                    //If this is record that already existed in the database (Control > 0) the user cannot edit the Accessorial Code (aka the dropdown editor under "Caption" field)
                                    var ctrl = e.model.Control;
                                    if (typeof (ctrl) === 'undefined' || ctrl == null){ ctrl = 0; }
                                    if (ctrl > 0 && fieldName === "Caption"){
                                        this.closeCell();
                                        blnIsEditable = false;
                                    }

                                    //The user cannot edit the field AccessorialCode because it can only be set through the dropdown selector
                                    if (fieldName === "AccessorialCode"){ this.closeCell(); blnIsEditable = false; }
                                    
                                    if (blnIsEditable === true){
                                        //No restrictions on editing so execute the "Select All" code
                                        var inputColumn = e.container.find("input"); 
                                        inputColumn.bind("focus", function () {
                                            var input = $(this);
                                            clearTimeout(input.data("selectTimeId")); //stop started time out if any                                    
                                            var selectTimeId = setTimeout(function() { input.select(); }); 
                                            input.data("selectTimeId", selectTimeId);
                                        }).blur(function(e) { clearTimeout($(this).data("selectTimeId")); }); //stop started timeout
                                    }                                                                                                                                     
                                }
                            },
                            dataBound: function(e) { 
                                //this.expandRow(this.tbody.find("tr.k-master-row"));           
                                //Show/Hide fee grid Msg column based on value of ShowPendingFeeFailReason
                                var tf = $("#txtShowPendingFeeFailReason").val();
                                if(tf === "false"){
                                    this.hideColumn("Msg");
                                }
                                else{
                                    this.showColumn("Msg");
                                }

                                var ds = this.dataSource.data();

                                var status = $("#txtStatus").val();
                                //Modified By LVV on 1/12/18 v-8.0 (Task ID 251) - Include check for FAILED status 
                                //if (typeof (status) !== 'undefined' && status != null && (status.toLowerCase() === 'pending') || status.toUpperCase() === 'FAILED') {
                                //Modified By LVV on 5/16/2018 for v-8.1 Settlement Rule 1
                                if (typeof (status) !== 'undefined' && status != null && status.toUpperCase() === 'FAILED') {                                   
                                    ////Do not allow delete in PA Status
                                    //Do not allow delete in FAILED Status
                                    for (var j=0; j < ds.length; j++) {                                      
                                        var item = this.dataSource.get(ds[j].FeeIndex); //Get by ID or any other preferred method
                                        this.tbody.find("tr[data-uid='"+item.uid+"'] .k-grid-delete").hide();                                        
                                    }
                                }
                                else{             
                                    //Determine if can delete
                                    //debugger;
                                    for (var j=0; j < ds.length; j++) {
                                        if (typeof (ds[j].Pending) !== 'undefined' && ds[j].Pending != null && ds[j].Pending === true) {
                                            //Only allow delete if BFP.AllowCarrierUpdates is true
                                            if (typeof (ds[j].AllowCarrierUpdates) !== 'undefined' && ds[j].AllowCarrierUpdates != null && ds[j].AllowCarrierUpdates === false) {
                                                var item = this.dataSource.get(ds[j].FeeIndex); //Get by ID or any other preferred method
                                                this.tbody.find("tr[data-uid='"+item.uid+"'] .k-grid-delete").hide();                                                 
                                            }
                                        }
                                        else{
                                            //Do not allow delete for BookFees
                                            var item = this.dataSource.get(ds[j].FeeIndex); //Get by ID or any other preferred method
                                            this.tbody.find("tr[data-uid='"+item.uid+"'] .k-grid-delete").hide();
                                        }
                                    }
                                }
                            }                           
                        });
                    },
                    detailExpand: function(e) {
                        var detailGrid = e.detailRow.parents(".k-widget.k-grid");
                        var status = $("#txtStatus").val();

                        //Modified By LVV on 1/12/18 v-8.0 (Task ID 251) - Include check for FAILED status 
                        //if (typeof (status) !== 'undefined' && status != null && (status.toLowerCase() === 'pending' || status.toUpperCase() === 'FAILED')) {
                        //Modified By LVV on 5/16/2018 for v-8.1 Settlement Rule 1
                        if (typeof (status) !== 'undefined' && status != null && (status.toUpperCase() === 'FAILED')) {
                            ////Do not allow Add in PA Status       
                            //Do not allow Add in FAILED Status  
                            detailGrid.find(".k-grid-toolbar").hide();
                        }
                        else{ detailGrid.find(".k-grid-toolbar").show(); }

                        //var tf = $("#txtShowPendingFeeFailReason").val();
                        //if(tf === "false"){
                            
                        //    var detailRow = e.detailRow;
                        //    detailRow.hideColumn("Msg");
                        //}
                        //else{
                        //    //this.expandRow(this.tbody.find("tr.k-master-row").first());
                        //    //detailGrid.showColumn("Msg");
                        //    this.showColumn("Msg");
                        //}                                               
                    }
                });
                //***** END FBSHIDGrid CODE *****
                
                //***** BEGIN wndExtFBDataEntry CODE *****
                wndExtFBDataEntry = $("#wndExtFBDataEntry").kendoWindow({
                    title: "Detailed Freight Bill Entry",
                    width: 800,
                    height: 500,
                    minWidth: 275,
                    actions: ["save", "Minimize", "Maximize", "Close"],
                    modal: true,
                    visible: false,
                    close: function(e) {
                        //clear all the values
                        $("#txtSHID").val("");    
                        $("#txtHeaderBookControl").val(0);
                        $("#txtLastStopComp").val(0);
                        $("#txtLastStopCompLE").val("");
                        $("#txtLastStopCarrier").val(0);                       
                        $("#txtInvoiceNumber").data("kendoMaskedTextBox").value("");
                        $("#txtBookCarrBLNumber").data("kendoMaskedTextBox").value("");
                        $("#txtBookFinAPActWgt").data("kendoNumericTextBox").value(0);
                        $("#txtLineHaul").data("kendoNumericTextBox").value(0);
                        $("#txtTotalFees").data("kendoNumericTextBox").value(0);
                        $("#txtTotalCost").data("kendoNumericTextBox").value(0);  
                        intStopFeeIndex = 0;
                        StopFees = [];
                    }
                }).data("kendoWindow");
                //Modified by RHR for Kendo v-2018 xxx must reference .parent() on save button click.
                $("#wndExtFBDataEntry").data("kendoWindow").wrapper.find(".k-svg-i-save").parent().click(function (e) { SaveExtFBDE() });
                //***** END wndExtFBDataEntry CODE *****
          
                dsFeesDropDown = new kendo.data.DataSource({
                    transport: {
                        read: {
                            ////url: "api/Accessorials/GetCLAXForSettlement",
                            url: "api/LECarrierAccessorial/GetLECAForSettlement", //REPLACED
                            headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                            type: "GET",
                            dataType: 'json',
                            data: function() {
                                var g = new GenericResult();
                                g.intField1 = $("#txtLastStopComp").val();
                                g.intField2 = $("#txtLastStopCarrier").val();
                                g.strField = $("#txtLastStopCompLE").val();

                                return { filter: JSON.stringify(g) };
                            }                       
                        },
                        parameterMap: function (options, operation) { return options; }
                    },
                    schema: {
                        data: "Data",
                        total: "Count",
                        model: { 
                            id: "AccessorialCode",
                            fields: {
                                AccessorialCode: { type: "number" },
                                Caption: { type: "string" }
                            }
                        }, 
                        errors: "Errors"
                    },
                    error: function (xhr, textStatus, error) { 
                        //debugger;
                        var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure");
                        ngl.showErrMsg("Get LECA For Settlement JSON Response Error", sMsg, null);  
                        this.cancelChanges(); 
                    },
                    serverPaging: false,
                    serverSorting: false,
                    serverFiltering: false
                });
                
                dsQuickEdit = new kendo.data.DataSource({
                    serverSorting: true,
                    serverPaging: true, 
                    pageSize: 10,
                    transport: { 
                        read: function(options) {                        
                            $.ajax({ 
                                url: 'api/Settlement/GetSettlementQuickEdit', 
                                contentType: 'application/json; charset=utf-8', 
                                dataType: 'json', 
                                data: { filter: "" }, 
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                                success: function(data) { 
                                    options.success(data); 
                                    if (typeof (data) !== 'undefined' && ngl.isObject(data) && typeof (data.Errors) !== 'undefined' &&  data.Errors != null) {  } 
                                }, 
                                error: function(result) { options.error(result); } 
                            }); 
                        }, 
                        parameterMap: function(options, operation) { return options; } 
                    },  
                    schema: {
                        data: "Data",
                        total: "Count",
                        model: {
                            id: "Control",
                            fields: {
                                SHID: { type: "string", editable: false },
                                CnsNumber: { type: "string", editable: false },
                                ProNumber: { type: "string", editable: false },
                                OrderNumber: { type: "string", editable: false },
                                PickupName: { type: "string", editable: false },
                                CarrierName: { type: "string", editable: false },
                                DestinationName: { type: "string", editable: false },
                                Status: { type: "string", editable: false },
                                DeliveredDate: { type: "date", editable: false },
                                ContractedCost: { type: "number", editable: false },
                                InvoiceNumber: { type: "string" },
                                InvoiceAmount: { type: "number" },
                                CarrierPro: { type: "string", editable: false },
                                BookFinAPActWgt: { type: "number", editable: false },
                                BookCarrBLNumber: { type: "string", editable: false },
                                BookCarrierControl: { type: "number", editable: false },
                                Control: { type: "number", editable: false }
                            }
                        },
                        errors: "Errors"
                    },
                    error: function(xhr, textStatus, error) { ngl.showErrMsg("Access vSettlementGrid365 Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges(); }
                });

                $('#QuickEditGrid').kendoGrid({ 
                    dataSource: dsQuickEdit,                
                    pageable: { info: false },
                    editable: "incell",
                    edit: function (e) {
                        var status = e.model.Status;
                        if (typeof (status) !== 'undefined' && status != null && status.length > 0 && status != 'N') {
                            this.closeCell();
                        }
                        else{
                            var inputColumn = e.container.find("input");                           
                            inputColumn.bind("focus", function () {
                                var input = $(this);
                                clearTimeout(input.data("selectTimeId")); //stop started time out if any                               
                                var selectTimeId = setTimeout(function() { input.select(); });                                
                                input.data("selectTimeId", selectTimeId);
                            }).blur(function(e) {
                                clearTimeout($(this).data("selectTimeId")); //stop started timeout
                            });
                        }                        
                    },                    
                    resizable: true, 
                    groupable: false, 
                    columns: [
                        {field: "Control", title: "Control", hidden: true },
                        {field: "BookCarrierControl", title: "BookCarrierControl", hidden: true },
                        {field: "CarrierName", title: "Carrier Name" },
                        {field: "SHID", title: "SHID" },
                        {field: "InvoiceNumber", title: "Invoice Number" },
                        {field: "InvoiceAmount", title: "Invoice Amount", format: "{0:c2}" }, 
                        {field: "ContractedCost", title: "Contracted Cost", format: "{0:c2}" },
                        {field: "Status", title: "Status", hidden: true },
                        //{field: "CnsNumber", title: "CNS Pool Number", hidden: true },
                        //{field: "ProNumber", title: "PRO Number", hidden: true },
                        //{field: "OrderNumber", title: "Order Number", hidden: true },
                        //{field: "PickupName", title: "Pickup Origin", hidden: true },
                        //{field: "DestinationName", title: "Destination", hidden: true },              
                        //{field: "DeliveredDate", title: "Delivered Date", template: "#= kendo.toString(kendo.parseDate(DeliveredDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') #", hidden: true },                                                    
                        //{field: "CarrierPro", title: "CarrierPro", hidden: true }
                    ]
                });

                wndQuickEdit = $("#wndQuickEdit").kendoWindow({
                    title: "Quick Freight Bill Entry",
                    width: 600,
                    //height: 500,
                    actions: ["save", "Minimize", "Maximize", "Close"],
                    modal: true,
                    visible: false              
                }).data("kendoWindow");          
                $("#wndQuickEdit").data("kendoWindow").wrapper.find(".k-svg-i-save").parent().click(function (e) { SaveQuickEdit() }); //Modified by RHR for Kendo v-2018 xxx must reference .parent() on save button click.                        
                          
            }
            var PageReadyJS = <%=PageReadyJS%>; 
            menuTreeHighlightPage(); //must be called after PageReadyJS
            var divWait = $("#h1Wait");
            if (typeof (divWait) !== 'undefined' ) { divWait.hide(); }
        });
    </script>
    <style>
        .k-grid tbody tr td { overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }         
        .k-tooltip{ max-height: 500px; max-width: 450px; overflow-y: auto; }    
        .k-grid tbody .k-grid-Edit { min-width: 0; }     
        .k-grid tbody .k-grid-Edit .k-icon { margin: 0; }
    </style>
    </div>

    </body>
</html>
