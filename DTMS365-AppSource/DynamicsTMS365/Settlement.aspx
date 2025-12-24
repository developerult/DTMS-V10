<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Settlement.aspx.cs" Inherits="DynamicsTMS365.Settlement" %>

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
                            <div class="k-block k-info-colored" style="margin:5px;">Click&nbsp;<span class="k-icon k-i-pencil" ></span>&nbsp;to enter the freight charges or Click&nbsp;<span class="k-icon k-i-info-circle" style="color: blue;"></span>&nbsp;to review the status of a previous transaction</div>                            
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

          <% Response.WriteFile("~/Views/ExtFBDataEntryWindow.html"); %>          

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
        var tPage = this;
        var tObjPG = this;
        var oSettlementGrid = null;
        var tObj = this;            
        

        <% Response.Write(NGLOAuth2); %>

               

        <% Response.Write(PageCustomJS); %>
        
        var dsQuickEdit = kendo.data.DataSource;        
        var wndQuickEdit = kendo.ui.Window;

        
        var sEditAccessorialsMessage = '<% Response.Write(EditAccessorialsMessage); %>';
        var sReadOnlyAccessorialsMessage = '<% Response.Write(ReadOnlyAccessorialsMessage); %>';

        var bShowOrderNbrOnQuickEdit = '<% Response.Write(sShowOrderNbrOnQuickEdit); %>';
        var bHideOrderNbrOnQuickEdit = true;
        if (bShowOrderNbrOnQuickEdit == "true") { bHideOrderNbrOnQuickEdit = false;}        
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
            try {
                var sErrMsg = "Unable to read load shipped weight; the value has been set to zero. The actual error is: ";
                if (typeof (response) !== 'undefined' && ngl.isObject(response)) {               
                    if (typeof (response.Errors) !== 'undefined' && response.Errors != null && response.Errors.length > 0) {
                        $("#txtBookFinAPActWgt").data("kendoNumericTextBox").value(0);                        
                        ngl.showInfoNotification("Read Load Shipped Weight Failure", (sErrMsg + response.Errors), tObj);
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
                $("#txtBookFinAPActWgt").data("kendoNumericTextBox").value(0);  
                ngl.showErrMsg("Read Read Load Shipped Failure", (sErrMsg + err.description), tObj);
            }           
        }
        function GetBookFinAPActWgtBySHIDAjaxErrorCallback(xhr, textStatus, error) {
            var tObj = this;    
            var sErrMsg = "Unable to readload shipped weight; the value has been set to zero. The actual error is: " + formatAjaxJSONResponsMsgs(xhr, textStatus, error, 'Failed to Read Load Shipped Weight');
            $("#txtBookFinAPActWgt").data("kendoNumericTextBox").value(0);  
            ngl.showErrMsg("Read Read Load Shipped Failure", sErrMsg, tObj);
        }

        function GetSettlementBookTotalMilesBySHIDCallback(response) {
            var tObj = this;   
            try {
                var sErrMsg = "Unable to read load total miles; the value has been set to zero. The actual error is: ";
                if (typeof (response) !== 'undefined' && ngl.isObject(response)) {               
                    if (typeof (response.Errors) !== 'undefined' && response.Errors != null && response.Errors.length > 0) {
                        $("#txtTotalMiles").data("kendoNumericTextBox").value(0);                        
                        ngl.showInfoNotification("Read Load Total Miles Failure", (sErrMsg + response.Errors), tObj);
                    }
                    else {                    
                        if (typeof (response.Data) !== 'undefined' && ngl.isArray(response.Data)) {                       
                            var rData = response.Data;
                            if (typeof (rData) !== 'undefined' && ngl.isObject(rData) && ngl.isArray(rData)) {
                                $("#txtTotalMiles").data("kendoNumericTextBox").value(rData[0]);                                                               
                            }                        
                        }
                    }
                    //calcSettlementTotals();
                } 
            } catch (err) {
                $("#txtTotalMiles").data("kendoNumericTextBox").value(0);  
                ngl.showErrMsg("Read Load Total Miles Failure", (sErrMsg + err.description), tObj);
            }           
        }
        function GetSettlementBookTotalMilesBySHIDAjaxErrorCallback(xhr, textStatus, error) {
            var tObj = this;    
            var sErrMsg = "Unable to read load total miles; the value has been set to zero. The actual error is: " + formatAjaxJSONResponsMsgs(xhr, textStatus, error, 'Failed to Read Load Total Miles');
            $("#txtTotalMiles").data("kendoNumericTextBox").value(0);  
            ngl.showErrMsg("Read Load Total Miles Failure", sErrMsg, tObj);
        }

        function GetAPCarrierCostCallback(response) {
            var tObj = this;
            try {
                var blnSuccess = false;
                var blnErrorShown = false;
                var sErrMsg = "Unable to read Line Haul; the value has been set to zero. The actual error is: ";
                if (typeof (response) !== 'undefined' && ngl.isObject(response)) {               
                    if (typeof (response.Errors) !== 'undefined' && response.Errors != null && response.Errors.length > 0) {
                        $("#txtLineHaul").data("kendoNumericTextBox").value(0);
                        $("#txtAPControl").val(0);
                        ngl.showInfoNotification("Read Carrier Line Haul Failure", (sErrMsg + response.Errors), tObj);
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
                $("#txtLineHaul").data("kendoNumericTextBox").value(0);
                ngl.showErrMsg("Read Carrier Line Haul Failure", (sErrMsg + err.description), tObj);
            }           
        }
        function GetAPCarrierCostAjaxErrorCallback(xhr, textStatus, error) {
            var tObj = this;   
            var sErrMsg = "Unable to read Line Haul; the value has been set to zero. The actual error is: " + formatAjaxJSONResponsMsgs(xhr, textStatus, error, 'Failed to Read Carrier Line Haul');
            $("#txtLineHaul").data("kendoNumericTextBox").value(0);
            ngl.showErrMsg("Read Existing Carrier Cost Failure", sErrMsg, tObj);
        }

        function GetSettlementFuelForSHIDCB(response) {
            var tObj = this; 
            try {
                var blnSuccess = false;
                var blnErrorShown = false;
                var sErrMsg = "Unable to read total fuel cost; the value has been set to zero. The actual error is: ";
                if (typeof (response) !== 'undefined' && ngl.isObject(response)) {               
                    if (typeof (response.Errors) !== 'undefined' && response.Errors != null && response.Errors.length > 0) {
                        $("#txtTotalFuel").data("kendoNumericTextBox").value(0);
                        ngl.showInfoNotification("Read Carrier Fuel Cost Failure", (sErrMsg + response.Errors), tObj);
                    }
                    else {                    
                        if (typeof (response.Data) !== 'undefined' && ngl.isArray(response.Data)) {                       
                            var rData = response.Data;
                            if (typeof (rData) !== 'undefined' && ngl.isObject(rData) && ngl.isArray(rData)) { $("#txtTotalFuel").data("kendoNumericTextBox").value(rData[0]); }                        
                        }
                    }
                    calcSettlementTotals()
                } 
            } catch (err) {
                $("#txtTotalFuel").data("kendoNumericTextBox").value(0);
                ngl.showErrMsg("Read Carrier Fuel Cost Failure", (sErrMsg + err.description), tObj);
            }          
        }
        function GetSettlementFuelForSHIDAjaxErrorCB(xhr, textStatus, error) {
            var tObj = this;  
            var sErrMsg = "Unable to read total fuel cost; the value has been set to zero. The actual error is: " + formatAjaxJSONResponsMsgs(xhr, textStatus, error, 'Failed to Read Carrier Fuel Cost');
            $("#txtTotalFuel").data("kendoNumericTextBox").value(0);
            ngl.showErrMsg("Read Carrier Fuel Cost Failure", sErrMsg, tObj);
        }


        //********** ExtFBDEWindow Code Start **********

        
        //Depreciated
        function calcSettlementTotalsWithFees(totalFees){
            alert("calcSettlementTotalsWithFees Depreciated");
            ////var len = 0;
            ////var totalCost = 0;
            ////var lineHaul = 0;
            ////var totalFuel = 0
            ////if (typeof (totalFees) === 'undefined' || totalFees == null){ totalFees = 0; }                   
            ////var lineHaulTextbox = $("#txtLineHaul").data("kendoNumericTextBox");
            ////if (lineHaulTextbox && lineHaulTextbox.value() != null){ lineHaul = lineHaulTextbox.value(); }
            ////var FuelTextbox = $("#txtTotalFuel").data("kendoNumericTextBox");
            ////if (FuelTextbox && FuelTextbox.value() != null){ totalFuel = FuelTextbox.value(); }
            ////totalCost = totalFees + lineHaul + totalFuel
            ////if ($("#txtTotalFees")){ $("#txtTotalFees").data("kendoNumericTextBox").value(totalFees); }
            ////if ($("#txtTotalCost")){ $("#txtTotalCost").data("kendoNumericTextBox").value(totalCost); }           
            ////return;
        }

        function openExtFBDEWindow(e) {
            var item = this.dataItem($(e.currentTarget).closest("tr"));
            if (typeof (item) !== 'undefined' && ngl.isObject(item)) {
                if (typeof (item.SHID) !== 'undefined' && item.SHID != null && item.SHID.length > 0) {
                    wndExtFBDataEntry.center().open().maximize();
                    kendo.ui.progress(wndExtFBDataEntry.element, true);
                    $("#txtSHID").val(item.SHID);
                    $("#txtAPControl").val(0);
                    $("#txtHeaderBookControl").val(item.Control);
                    //set Invoice Amt and Invoice No from the values in the Grid if they already exist
                    $("#txtLineHaul").data("kendoNumericTextBox").value(0);                    
                    $("#txtTotalMiles").data("kendoNumericTextBox").value(0);
                    $("#txtTotalFuel").data("kendoNumericTextBox").value(0);
                    $("#txtInvoiceNumber").data("kendoMaskedTextBox").value(item.InvoiceNumber);
                    //Modified by RHR for v-8.5.3.006 on 02/17/2023 added Invoice Date
                    var todayDate = new Date();
                    $('#txtInvoiceDate').data("kendoDatePicker").value(todayDate);
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
                    var blnRet = oCRUDCtrl.filteredRead("Settlement/GetBookFinAPActWgtBySHID", filter, tObj, "GetBookFinAPActWgtBySHIDCallback", "GetBookFinAPActWgtBySHIDAjaxErrorCallback");
                    var blnRet = oCRUDCtrl.filteredRead("Settlement/GetAPCarrierCost", filter, tObj, "GetAPCarrierCostCallback", "GetAPCarrierCostAjaxErrorCallback");
                    var blnRet = oCRUDCtrl.filteredRead("Settlement/GetSettlementFuelForSHID", filter, tObj, "GetSettlementFuelForSHIDCB", "GetSettlementFuelForSHIDAjaxErrorCB");              
                    blnRet = oCRUDCtrl.filteredRead("Settlement/GetSettlementBookTotalMilesBySHID", filter, tObj, "GetSettlementBookTotalMilesBySHIDCallback", "GetSettlementBookTotalMilesBySHIDAjaxErrorCallback");              

                    //Load the Grids for this SHID                   
                    getSettlementFBDEData();
                    //Modified By LVV on 5/16/2018 for v-8.1 Settlement Rule 1 - Allow carrier updates to freight bill when booking record's PExp (bookpaycode) is in N or PA
                    var strStatus = "";                     
                    if (typeof (item.Status) === 'undefined' || item.Status == null) { strStatus = "N"; } else{ strStatus = item.Status; } //NUll status functions the same as N
                    $("#txtStatus").val(strStatus);                                                         
                    if (strStatus.toUpperCase() === 'FAILED') {
                        //disable the fields
                        $("#txtInvoiceNumber").data("kendoMaskedTextBox").enable(false);
                        //Modified by RHR for v-8.5.3.006 on 02/17/2023 added Invoice Date
                        $("#txtInvoiceDate").data("kendoDatePicker").enable(false);
                        $("#txtBookCarrBLNumber").data("kendoMaskedTextBox").enable(false);
                        $("#txtBookFinAPActWgt").data("kendoNumericTextBox").enable(false);
                        $("#txtLineHaul").data("kendoNumericTextBox").enable(false);                        
                        $("#txtTotalMiles").data("kendoNumericTextBox").enable(false);
                        $("#txtTotalFuel").data("kendoNumericTextBox").enable(false);
                        $("#txtTotalFees").data("kendoNumericTextBox").enable(false);
                        $("#txtTotalCost").data("kendoNumericTextBox").enable(false);
                        $("#wndExtFBDataEntry").data("kendoWindow").wrapper.find(".k-svg-i-save").hide();
                        $("#spwndExtFBEditMsg").html(sReadOnlyAccessorialsMessage);
                    }
                    else{
                        $("#txtInvoiceNumber").data("kendoMaskedTextBox").enable(true);
                        //Modified by RHR for v-8.5.3.006 on 02/17/2023 added Invoice Date
                        $("#txtInvoiceDate").data("kendoDatePicker").enable(true)
                        $("#txtBookCarrBLNumber").data("kendoMaskedTextBox").enable(true);
                        $("#txtBookFinAPActWgt").data("kendoNumericTextBox").enable(true);
                        $("#txtLineHaul").data("kendoNumericTextBox").enable(true);
                        $("#txtTotalMiles").data("kendoNumericTextBox").enable(false);
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

        //Depreciated
        function CheckAuditMessageVisibility(){
            alert("CheckAuditMessageVisibility Depreciated");

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
            
            ////var s = new GenericResult();
            ////s.Control = $("#txtLastStopCompCtrl").val();
            ////s.intField1 = $("#txtLastStopCarrierCtrl").val();
            ////s.strField = $("#txtSHID").val();

            ////$.ajax({
            ////    async: false,
            ////    url: 'api/Settlement/GetAuditMessageVisibility',
            ////    contentType: 'application/json; charset=utf-8',
            ////    dataType: 'json',
            ////    data: { filter: JSON.stringify(s) },
            ////    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
            ////    success: function(data) {
            ////        try {
            ////            var blnSuccess = false;
            ////            var blnErrorShown = false;
            ////            var strValidationMsg = "";
            ////            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
            ////                if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
            ////                    blnErrorShown = true;
            ////                    ngl.showErrMsg("GetAuditMessageVisibility Failure", data.Errors, null);
            ////                }
            ////                else {
            ////                    if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
            ////                        if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
            ////                            blnSuccess = true;
            ////                            /// <returns>
            ////                            /// GenericResult.blnField1 = ShowAuditFailReason
            ////                            /// GenericResult.blnField2 = ShowPendingFeeFailReason
            ////                            /// GenericResult.strField = APMessage
            ////                            /// </returns>

            ////                            $("#txtShowPendingFeeFailReason").val(data.Data[0].blnField2);
            ////                            if (data.Data[0].blnField1 === false){
            ////                                //If ShowAuditFailReason = false hide the div
            ////                                $("#divAuditFailMsg").html("");
            ////                                $("#divAuditFailMsg").hide();
            ////                            }
            ////                            else{
            ////                                //ShowAuditFailReason = true 
            ////                                var apMsg = data.Data[0].strField;
            ////                                if(typeof (apMsg) !== 'undefined' && apMsg != null && apMsg.length > 0){
            ////                                    //If APMessage != null show the msg and div
            ////                                    $("#divAuditFailMsg").html("<p>" + apMsg + "</p>");
            ////                                    $("#divAuditFailMsg").show();
            ////                                }
            ////                                else{
            ////                                    //If APMessage = null hide the div
            ////                                    $("#divAuditFailMsg").html("");
            ////                                    $("#divAuditFailMsg").hide();
            ////                                }                                
            ////                            }
            ////                        }
            ////                    }
            ////                }
            ////            }
            ////            if (blnSuccess === false && blnErrorShown === false) {
            ////                if (strValidationMsg.length < 1) { strValidationMsg = "Book Fees Pending not found"; }
            ////                ngl.showErrMsg("Get Book Fees Pending Failure", strValidationMsg, null);
            ////            }
            ////        } catch (err) {
            ////            ngl.showErrMsg(err.name, err.description, null);
            ////        }
            ////    },
            ////    error: function (xhr, textStatus, error) { 
            ////        var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure");
            ////        ngl.showErrMsg("GetSettlementFeesForSHID JSON Response Error", sMsg, null);   
            ////        kendo.ui.progress(wndExtFBDataEntry.element, false);
            ////    }
            ////});
        }

        //this overloaded function does not close the window when the save is finished and it calls calcSettlementTotals on success
        function saveExtFBDEFuel(){
            calcSettlementTotals();            
            // old ajax message removed by RHR on 8/26/19 left as an example of message formats
            //var sMsg =  ngl.replaceEmptyString(data.Data[0].SuccessTitle,'', '<br>') +  ngl.replaceEmptyString(data.Data[0].SuccessMsg,'');
            //if (ngl.isNullOrWhitespace(sMsg) == false) { sMsg = sMsg + '<br>'; }
            //sMsg = sMsg +  ngl.replaceEmptyString(data.Data[0].WarningTitle,'','<br>') +  ngl.replaceEmptyString(data.Data[0].WarningMsg,'');
            //$("#divAuditFailMsg").html("<p>" + sMsg + "</p>");                
            //if (ngl.stringHasValue(data.Data[0].ErrMsg)){ ngl.showErrMsg(ngl.replaceEmptyString(data.Data[0].ErrTitle,'Save Fuel Error'), data.Data[0].ErrMsg, tObj); }                                                       
        }


        function SaveExtFBDE() {
            ngl.OkCancelConfirmation(
                "Save Freight Bill Charges",
                'Only one freight bill is allowed per shipment.  You have entered a total amount of ' + $("#txtTotalCost").data("kendoNumericTextBox").value() + ' for invoice number ' + $("#txtInvoiceNumber").data("kendoMaskedTextBox").value() + '.  Press OK to continue.',
                400,
                400,
                tPage,
                "execSaveExtFBDE");
        }
        //Modified by RHR for v-8.5.2.007 on 06/21/2023 added iRet to support cancel in OkCancelConfirmation message
        function execSaveExtFBDE(iRet) {
            if (typeof (iRet) === 'undefined' || iRet === null || iRet === 0) { return; }
            var otmp = $("#focusCancelFB").focus();
            var s = new SettlementSave();
            s.InvoiceNo = $("#txtInvoiceNumber").data("kendoMaskedTextBox").value();
            if (isEmpty(s.InvoiceNo) == true){ ngl.showWarningMsg ("Cannot Save Changes", "Data Validation Failure. The Freight Bill Invoice Number is required and cannot be empty", tObj); return; }
            kendo.ui.progress(wndExtFBDataEntry.element, true);
            setTimeout(function (tObj) {               
                //Add Validation Logic for Required Fields
                var oValidationResults = validateFees();
                if (typeof (oValidationResults) === 'undefined' || !ngl.isObject(oValidationResults)) { ngl.showValidationMsg("Cannot validate Settlement Fees", "Invalid validation procedure, please contact technical support", tObj); return; } 
                else { if (oValidationResults.Success === false) { ngl.Alert("Validation Failure - Missing Required Fields", oValidationResults.Message, 400, 400); kendo.ui.progress(wndExtFBDataEntry.element, false); return; } }
                var StopFees = new Array();
                StopFees.push.apply(StopFees, LoadFees);
                StopFees.push.apply(StopFees, OrderFees);
                StopFees.push.apply(StopFees, OrigFees);
                StopFees.push.apply(StopFees, DestFees);
                s.InvoiceAmt = $("#txtTotalCost").data("kendoNumericTextBox").value();                
                s.LineHaul = $("#txtLineHaul").data("kendoNumericTextBox").value();
                s.TotalFuel = $("#txtTotalFuel").data("kendoNumericTextBox").value();
                s.BookCarrBLNumber = $("#txtBookCarrBLNumber").data("kendoMaskedTextBox").value();
                s.BookFinAPActWgt = $("#txtBookFinAPActWgt").data("kendoNumericTextBox").value();
                s.Fees = StopFees;
                s.BookSHID = $("#txtSHID").val();
                s.BookControl = $("#txtHeaderBookControl").val();
                s.CarrierControl = $("#txtLastStopCarrierCtrl").val();
                s.CompControl = $("#txtLastStopCompCtrl").val();
                //Modified by RHR for v-8.5.3.006 on 02/17/2023 added Invoice Date
                s.APBillDate = ngl.convertDateTimeToDateString($("#txtInvoiceDate").data("kendoDatePicker").value(), null, null);              
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
                            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                if (ngl.stringHasValue(data.Errors)) { ngl.showErrMsg("Settlement Save Failure", data.Errors, null); }
                                else {
                                    if (typeof (data.Data) !== 'undefined' && data.Data != null && ngl.isArray(data.Data)) {
                                        if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                            if (ngl.isNullOrWhitespace(data.Data[0].SuccessMsg) == false){ var sSuccessMsg = ngl.replaceEmptyString(data.Data[0].SuccessTitle,'', '<br>') + ngl.replaceEmptyString(data.Data[0].SuccessMsg,''); ngl.showSuccessMsg(sSuccessMsg,tObj); }                                
                                            if (ngl.isNullOrWhitespace(data.Data[0].WarningMsg) == false){ ngl.showWarningMsg(ngl.replaceEmptyString(data.Data[0].WarningTitle,'Save Settlement Warning'), data.Data[0].WarningMsg, tObj); }                                         
                                            if (ngl.isNullOrWhitespace(data.Data[0].ErrMsg) == false){ ngl.showErrMsg(ngl.replaceEmptyString(data.Data[0].ErrTitle,'Save Settlement Error'), data.Data[0].ErrMsg, tObj); }                            
                                            refreshSettlementGrid();
                                            kendo.ui.progress(wndExtFBDataEntry.element, false);
                                            wndExtFBDataEntry.close();
                                        }
                                    }
                                } 
                            } else { ngl.showErrMsg("Settlement Save Failure", "There was a problem while executing Settlement Save", null); }
                        } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                    },
                    error: function (xhr, textStatus, error) {
                        kendo.ui.progress(wndExtFBDataEntry.element, false);
                        ngl.showErrMsg("Settlement Save Failure", formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure"), null);                        
                    }
                });
            }, 200, tObj);
        }
        //********** ExtFBDEWindow Code End **********

        

        function SaveQuickEdit(){
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
                            var blnSuccess = false, blnErrorShown = false;
                            var strValidationMsg = "There was a problem while executing Settlement Quick Save";
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
                            if (blnSuccess === false && blnErrorShown === false) { ngl.showErrMsg("Settlement Quick Save Failure", strValidationMsg, null); }
                        } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                    },
                    error: function (xhr, textStatus, error) { kendo.ui.progress(wndQuickEdit.element, false); var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure"); ngl.showErrMsg("Settlement Quick Save Failure", sMsg, null); }
                });
            }, 200, tObj);
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
           
            if (control != 0){
                $("#txtSHID").val("");              
                $("#txtHeaderBookControl").val(0);
                                                          
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
                        if (typeof (status) !== 'undefined' && status != null && status.length > 0 && status != 'N') { this.closeCell(); }
                        else{
                            var inputColumn = e.container.find("input");                           
                            inputColumn.bind("focus", function () {
                                var input = $(this);
                                clearTimeout(input.data("selectTimeId")); //stop started time out if any                               
                                var selectTimeId = setTimeout(function() { input.select(); });                                
                                input.data("selectTimeId", selectTimeId);
                            }).blur(function(e) { clearTimeout($(this).data("selectTimeId")); }); //stop started timeout
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
                        { field: "OrderNumber", title: "Order Number", hidden: bHideOrderNbrOnQuickEdit },
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