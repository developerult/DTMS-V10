<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoadBoardBing.aspx.cs" Inherits="DynamicsTMS365.LoadBoardBing" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>>Dynamics TMS 365 Load Board With Bing</title>
    <%=cssReference%>
    <style>
        html,
        body { height: 100%; margin: 0; padding: 0; }
        html { font-size: 12px; font-family: Arial, Helvetica, sans-serif; overflow: hidden; }
    </style>
    <meta charset="utf-8" />
    <%--Bing Maps--%>
</head>
<body>
    <%=jssplitter2Scripts%>
    <%=BingMapsJS%>  <%--Bing Maps--%>
    <%=sWaitMessage%>
    <div id="example" style="height: 100%; width: 100%; margin-top: 2px;">
        <div id="vertical" style="height: 98%; width: 98%;">
            <div id="menu-pane" style="height: 100%; width: 100%; background-color: white;">
                <div id="tab" class="menuBarTab"></div>
            </div>
            <div id="top-pane">
                <div id="horizontal" style="height: 100%; width: 100%;">
                    <div id="left-pane">
                        <div class="pane-content">
                            <% Response.Write(MenuControl); %>
                            <div id="menuTree"></div>
                        </div>
                    </div>
                    <div id="center-pane">
                        <% Response.Write(PageErrorsOrWarnings); %>
                        <!-- begin Page Content -->
                        <% Response.Write(FastTabsHTML); %>
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

        <% Response.WriteFile("~/Views/DispatchingDialog.html"); %>
        <% Response.WriteFile("~/Views/DispatchReport.html"); %>
        <% Response.WriteFile("~/Views/BOLReport.html"); %>

        <% Response.WriteFile("~/Views/MapWindow.html"); %> <%--Bing Maps--%>
        <% Response.WriteFile("~/Views/LBCreateBookingWnd.html"); %> <%--Added By LVV on 4/20/20 LBDemo--%>   
        <% Response.WriteFile("~/Views/OptimizerWnd.html"); %> <%--Added By LVV on 9/16/20 for v-8.3.0.001 - Optimizer 365 --%>      
        <% Response.Write(PageTemplates); %>

        <%-- HelpWindow needs to go after AuthNotification because it uses the notification object created there --%>
        <% Response.Write(AuthLoginNotificationHTML); %>
        <% Response.WriteFile("~/Views/HelpWindow.html"); %>
        <script>                     
        <% Response.Write(ADALPropertiesjs); %>

            var PageControl = '<%=PageControl%>';         
            var oLoadBoardGrid = null;
            var wnd = kendo.ui.Window; 
            var tObj = this;
            var tPage = this;
            var iBookPK = 0;
            var loadBoardGridSelectedRow; //Note this only gets populated via a call to saveBookPK(), not necessarily every time a record is selected (We should probably put in a callback in cm grid so we can put the logic in a select method for the grid so we know it gets refreshed every time based on grid select event and not call to saveBookPK(). However this works for now in this scenario)
            var loadBoardGridSelectedRowDataItem; 
            var blnCanPrintBOL = false; //page level variable to determine if selected record can print BOL 
            var blnCanPrintDispatch = false; //page level variable to determine if selected record can print Dispatch 
            var blnBtnDispatchReportClicked = false; //indicates whether the Dispatch Report was generated as result of Dispatching a load or from clicking the Report Menu button. Impacts whether BOL is automatically shown or not
                    
        

        <% Response.Write(NGLOAuth2); %>

        
        <% Response.Write(PageCustomJS); %>
            // start widgit configuration
            var winDispatchingDialog = kendo.ui.Window;
            var winDispatchReport = kendo.ui.Window;
            var winBOLReport  = kendo.ui.Window;
            var oDispatchingDialogCtrl = new DispatchingDialogCtrl();     
            var oDispatchReportCtrl = new DispatchingReportCtrl();
            var oBOLReportCtrl = new BOLReportCtrl();
            var oDispatchData = new Dispatch();
            var iSelectedLoadTenderControl
            //create widgit call backs
            function oDispatchingDialogSelectCB(results){  
                //debugger;
                var data = new Dispatch();
                if (typeof (oDispatchingDialogCtrl)  === 'undefined' || oDispatchingDialogCtrl == null  ) { return;}
                if (typeof (oDispatchingDialogCtrl.data) === 'undefined' ||  oDispatchingDialogCtrl.data == null ) { return;}
                try{
                    data = oDispatchingDialogCtrl.data;
                    //if (blnDispatchedFromPageQuote == true) {
                    //    $("#txtorigCompName").data("kendoMaskedTextBox").value(data.Origin.Name);
                    //    $("#txtorigCompAddress1").data("kendoMaskedTextBox").value(data.Origin.Address1);
                    //    $("#txtOrigPhone").val(data.Origin.Contact.ContactPhone);
                    //    $("#txtOrigContactName").val(data.Origin.Contact.ContactName);
                    //    $("#txtOrigContactEmail").val(data.Origin.Contact.ContactEmail);                      
                    //    $("#txtdestCompName").data("kendoMaskedTextBox").value(data.Destination.Name);
                    //    $("#txtdestCompAddress1").data("kendoMaskedTextBox").value(data.Destination.Address1);
                    //    $("#txtDestPhone").val(data.Destination.Contact.ContactPhone);
                    //    $("#txtDestContactName").val(data.Destination.Contact.ContactName);
                    //    $("#txtDestContactEmail").val(data.Destination.Contact.ContactEmail);
                    //} 
                } catch (err) {
                    //do nothing
                }        
            }
            function oDispatchingDialogSaveCB(results){
                //debugger;
                var data = new Dispatch();
                if (typeof (results)  === 'undefined' || results == null  ) { return;}
                if (typeof (results.data) === 'undefined' ||  results.data == null || ngl.isArray(results.data) == false) { return;}
                try{
                    data = results.data[0];           
                    //if (blnDispatchedFromPageQuote == true) {
                    //    $("#txtorigCompAddress1").val(data.Origin.Address1);
                    //    $("#txtOrigPhone").val(data.Origin.Contact.ContactPhone);
                    //    $("#txtOrigContactName").val(data.Origin.Contact.ContactName);
                    //    $("#txtOrigContactEmail").val(data.Origin.Contact.ContactEmail);
                    //    $("#txtdestCompAddress1").val(data.Destination.Address1);
                    //    $("#txtDestPhone").val(data.Destination.Contact.ContactPhone);
                    //    $("#txtDestContactName").val(data.Destination.Contact.ContactName);
                    //    $("#txtDestContactEmail").val(data.Destination.Contact.ContactEmail);
                    //}
                    oDispatchReportCtrl.data = results.data;
                    oDispatchReportCtrl.show();
                } catch (err) {
                    //do nothing
                }
            }
            function oDispatchingDialogCloseCB(results){ 
                //debugger;
                var data = new Dispatch();
                if (typeof (oDispatchingDialogCtrl)  === 'undefined' || oDispatchingDialogCtrl == null  ) { return;}
                if (typeof (oDispatchingDialogCtrl.data) === 'undefined' ||  oDispatchingDialogCtrl.data == null ) { return;}
                try{
                    data = oDispatchingDialogCtrl.data;         
                    //if (blnDispatchedFromPageQuote == true) {
                    //    $("#txtOrigPhone").val(data.Origin.Contact.ContactPhone);
                    //    $("#txtOrigContactName").val(data.Origin.Contact.ContactName);
                    //    $("#txtOrigContactEmail").val(data.Origin.Contact.ContactEmail);
                    //    $("#txtDestPhone").val(data.Destination.Contact.ContactPhone);
                    //    $("#txtDestContactName").val(data.Destination.Contact.ContactName);
                    //    $("#txtDestContactEmail").val(data.Destination.Contact.ContactEmail);
                    //}
                } catch (err) {
                    //do nothing
                }
            }
            function oDispatchingDialogReadCB(results){ 
                //debugger;
                var data = new Dispatch();
                if (typeof (oDispatchingDialogCtrl)  === 'undefined' || oDispatchingDialogCtrl == null  ) { return;}
                if (typeof (oDispatchingDialogCtrl.data) === 'undefined' ||  oDispatchingDialogCtrl.data == null ) { return;}
                try{
                    data = oDispatchingDialogCtrl.data;            
                    //if (blnDispatchedFromPageQuote == true) {
                    //    $("#txtOrigPhone").val(data.Origin.Contact.ContactPhone);
                    //    $("#txtOrigContactName").val(data.Origin.Contact.ContactName);
                    //    $("#txtOrigContactEmail").val(data.Origin.Contact.ContactEmail);
                    //    $("#txtDestPhone").val(data.Destination.Contact.ContactPhone);
                    //    $("#txtDestContactName").val(data.Destination.Contact.ContactName);
                    //    $("#txtDestContactEmail").val(data.Destination.Contact.ContactEmail); 
                    //}
                } catch (err) {
                    //do nothing
                }
            }

            function oBOLReportSelectCB(results){ return; }
            function oBOLReportSaveCB(results){ return; }
            function oBOLReportCloseCB(results){ return; }
            function oBOLReportReadCB(results){ return; }

            function oDispatchingReportSelectCB(results){ return; }
            function oDispatchingReportSaveCB(results){ return; }
            function oDispatchingReportCloseCB(results){          
                try{
                    var blnTryAgain = false;
                    if (typeof (results.data) !== 'undefined' && ngl.isObject(results.data) && typeof (results.data.ErrorCode) !== 'undefined' &&  results.data.ErrorCode !== null ){
                   
                        if(!ngl.isNullOrWhitespace(results.data.ErrorCode) && !isNaN( results.data.ErrorCode)){
                            //get the bid type
                            if ( typeof (results.data.DispatchBidType) !== 'undefined' &&  results.data.DispatchBidType !== null && !ngl.isNullOrWhitespace(results.data.DispatchBidType) && !isNaN( results.data.DispatchBidType)){
                                //possible bid types
                                //1	NextStop
                                //2	NGLTar
                                //3	P44
                                //4	Spot
                                if (results.data.DispatchBidType.toString() == "2" || results.data.DispatchBidType.toString() == "3"){
                                    //if we havea  loadTenderControl reopen the quote window so user can select a new quote
                                    if ( typeof (results.data.LoadTenderControl) !== 'undefined' &&  results.data.LoadTenderControl !== null && !ngl.isNullOrWhitespace(results.data.LoadTenderControl) && !isNaN( results.data.LoadTenderControl)){
                                        if (typeof (tPage["wdgtQuotesWndDialog"]) !== 'undefined' && ngl.isObject(tPage["wdgtQuotesWndDialog"])){
                                            sBidLoadTenderControlVal = results.data.LoadTenderControl;
                                            tPage["wdgtQuotesWndDialog"].read(results.data.LoadTenderControl);
                                        } 
                                    }
                                    return;
                                } else {
                                    //just refresh the data and exit
                                    execActionClick("Refresh");
                                    return;
                                }
                            }                       
                        }                  
                    }
                    //if we get here just show the BOL and refresh the data               
                    if(!blnBtnDispatchReportClicked){ PrintSelectedBOL(); } //not clicked so show BOL aka report shown after dispatching
                    blnBtnDispatchReportClicked = false; //reset the value
                    execActionClick("Refresh");                
                } catch (err) {
                    //do nothing
                }
                return;
            }
            function oDispatchingReportReadCB(results){ return; }
        
            //End widgit configuration

            function savePostPageSettingSuccessCallback(results){
                //for now do nothing when we save the pk
            }
            function savePostPageSettingAjaxErrorCallback(xhr, textStatus, error){
                //for now do nothing when we save the pk
            }

            function readGetPageSettingSuccessCallback(data) {
                var oResults = new nglEventParameters();
                var tObj = this;
                oResults.source = "readGetPageSettingSuccessCallback";
                oResults.msg = 'Failed'; //set default to Failed         
                oResults.CRUD = "read";
                oResults.widget = tObj;
                var userPageSettings = null;                          
                try {                      
                    if (typeof (data) !== 'undefined' && ngl.isObject(data)) {                          
                        if (ngl.stringHasValue(data.Errors)) { ngl.showErrMsg("Get User Page Settings Failure", data.Errors, null); }                          
                        else {                               
                            if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {                                   
                                if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                    oResults.msg = "Success";
                                    userPageSettings = data.Data[0];                                 
                                    if(typeof (userPageSettings) !== 'undefined' && userPageSettings != null && userPageSettings.value != undefined){
                                        if (userPageSettings.name === "pk"){ 
                                            if(typeof (iBookPK) === 'undefined' || iBookPK == null || iBookPK === 0){ iBookPK = userPageSettings.value; }                                 
                                        }                                                                                                                                  
                                    }                                   
                                }                              
                            }                            
                        }                        
                    }                                       
                } catch (err) { ngl.showErrMsg(err.name, err.description, null); }             
            }
            function readGetPageSettingAjaxErrorCallback(xhr, textStatus, error) {
                var oResults = new nglEventParameters();
                var tObj = this;
                oResults.source = "readGetPageSettingAjaxErrorCallback";
                oResults.msg = 'Failed'; //set default to Failed        
                oResults.CRUD = "read"
                oResults.widget = tObj;
                oResults.error = new Error();
                oResults.error.name = "Read Page Settings Failure"
                oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
                ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj); 
            }

            var oQuoteSelectedBid = null; 
            function dispatchSelectedQuote(e){
                //debugger;
                //alert('do dispatch')
                oQuoteSelectedBid = this.dataItem($(e.currentTarget).closest("tr")); 
                if (typeof (oQuoteSelectedBid) !== 'undefined' && ngl.isObject(oQuoteSelectedBid)) {
                    oDispatchingDialogCtrl.read(oQuoteSelectedBid.BidControl)
                    if (typeof (tPage["wdgtQuotesWndDialog"]) !== 'undefined' && ngl.isObject(tPage["wdgtQuotesWndDialog"])){                 
                        tPage["wdgtQuotesWndDialog"].executeActions("close");
                    } 
                    //ngl.OkCancelConfirmation(
                    //       "Replace Load Information With Selected",
                    //       "This action will replace the current load information with the select address and item details",
                    //       400,
                    //       400,
                    //       null,
                    //       "ConfirmCopyHistoricalQuote");
                };       
            }

            function PrintSelectedBOL(){
                if (typeof (iBookPK) !== 'undefined' || iBookPK !== null || iBookPK !== 0) {
                    if (typeof (oBOLReportCtrl) !== 'undefined' && ngl.isObject(oBOLReportCtrl)) { oBOLReportCtrl.readByBookControl(iBookPK); }
                }                         
            }

            function openDispatchReport(){
                if (typeof (iBookPK) !== 'undefined' || iBookPK !== null || iBookPK !== 0) {
                    if (typeof (oDispatchReportCtrl) !== 'undefined' && ngl.isObject(oDispatchReportCtrl)) {
                        blnBtnDispatchReportClicked = true; //clicked
                        var oCRUDCtrl = new nglRESTCRUDCtrl();
                        var blnRet = oCRUDCtrl.read("Dispatching/GetDispatchReportData", iBookPK, tPage, "GetDispatchReportDataSuccessCallback", "GetDispatchReportDataAjaxErrorCallback",true);  
                    }; 
                }           
            }

            function openLoadTenderReport(){ ngl.showInfoNotification("Feature Unavailable", "Load Tender Report coming soon", null); }


            function GetDispatchReportDataSuccessCallback(data) {
                var oResults = new nglEventParameters();
                oResults.source = "GetDispatchReportDataSuccessCallback";
                oResults.widget = this;
                oResults.msg = 'Failed'; //set default to Failed     
                this.rData = null;
                var tObj = this;
                try {
                    if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                        if (ngl.stringHasValue(data.Errors)) {
                            oResults.error = new Error();
                            oResults.error.name = "Get Dispatch Report Data Failure";
                            oResults.error.message = data.Errors;
                            ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                        }
                        else{ 
                            if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                    blnSuccess = true;
                                    oResults.msg = 'Success';
                                    var dArray = [];
                                    for (i = 0; i < data.Data.length; i++) {
                                        var d = new Dispatch();
                                        d = data.Data[i];    
                                        dArray.push(d);
                                    }
                                    oDispatchReportCtrl.data = dArray;
                                    oDispatchReportCtrl.show();
                                }
                            }
                        }
                    }
                    if (blnSuccess === false && blnErrorShown === false) {
                        if (strValidationMsg.length < 1) { strValidationMsg = "Get Dispatch Report Data Failure"; }
                        ngl.showErrMsg("Get Dispatch Report Data Failure", strValidationMsg, null);
                    }
                } catch (err) { oResults.error = err; ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj); }           
            }
            function GetDispatchReportDataAjaxErrorCallback(xhr, textStatus, error) {
                var oResults = new nglEventParameters();
                var tObj = this;
                oResults.source = "GetDispatchReportDataAjaxErrorCallback";
                oResults.widget = this;
                oResults.msg = 'Failed'; //set default to Failed  
                oResults.error = new Error();
                oResults.error.name = "Get Dispatch Report Data Failure"
                oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
                ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
            }

            function saveBookPK() {
                try {
                    loadBoardGridSelectedRow = oLoadBoardGrid.select();
                    if (typeof (loadBoardGridSelectedRow) === 'undefined' || loadBoardGridSelectedRow == null) { ngl.showValidationMsg("Booking Record Required", "Please select a Booking to continue", tPage); return false; }                             
                    loadBoardGridSelectedRowDataItem = oLoadBoardGrid.dataItem(loadBoardGridSelectedRow); //Get the dataItem for the selected row
                    if (typeof (loadBoardGridSelectedRowDataItem) === 'undefined' || loadBoardGridSelectedRowDataItem == null) { ngl.showValidationMsg("Booking Record Required", "Please select a Booking to continue", tPage); return false; } 
                    if ("BookControl" in loadBoardGridSelectedRowDataItem){                
                        iBookPK = loadBoardGridSelectedRowDataItem.BookControl;
                        var setting = {name:'pk', value: iBookPK.toString()};
                        var oCRUDCtrl = new nglRESTCRUDCtrl();
                        var blnRet = oCRUDCtrl.update("LoadBoard/PostPageSetting", setting, tPage, "savePostPageSettingSuccessCallback", "savePostPageSettingAjaxErrorCallback",tPage);
                        //check if the TranCode is PB or PC and enable or disable the Print BOL action button (We can print the BOL for both PB or PC)  
                        if ("BookTranCode" in loadBoardGridSelectedRowDataItem){                
                            var tranCode = loadBoardGridSelectedRowDataItem.BookTranCode;                       
                            if(tranCode.toUpperCase() === "PC" || tranCode.toUpperCase() === "PB"){ $('#btnCAR120').prop('disabled', false); blnCanPrintBOL = true; } else { $('#btnCAR120').prop('disabled', true); blnCanPrintBOL = false; } //BOL Report enable/disable                      
                            if(tranCode.toUpperCase() === "N" || tranCode.toUpperCase() === "P"){ $('#btnDispatchReport').prop('disabled', true); blnCanPrintDispatch = false; } else { $('#btnDispatchReport').prop('disabled', false); blnCanPrintDispatch = true; } //Disptach Report enable/disable                   
                        } else { $('#btnCAR120').prop('disabled', true); blnCanPrintBOL = false; $('#btnDispatchReport').prop('disabled', true); blnCanPrintDispatch = false; }
                        return true;
                    } else { ngl.showValidationMsg("Booking Record Required", "Invalid Record Identifier, please select a Booking to continue", tPage); return false; }
                } catch (err) { ngl.showErrMsg("Cannot Continue", "There was an unexpected error, please reload the screen. If this continues please contact technical support. Error: " + err.message, tPage); }           
            }
    
            function isBookingSelected() {
                if (typeof (iBookPK) === 'undefined' || iBookPK === null || iBookPK === 0) { return saveBookPK(); }
                return true;
            }

            var oQuotesGrid = null;
            var  sBidLoadTenderControlVal = "0"
            function QuotesGetStringData(s)
            {                  
                //NOTE: This code is no longer needed in this scenario but I left the code here as an example in case we need to use it in the future
                //var f = new FilterDetails();
                //if (isNaN(s.iFilterID)) { s.iFilterID = 0;}
                //s.iFilterID = s.iFilterID + 1;
                //f.filterID = s.iFilterID; f.filterCaption = "BidLoadTenderControl"; f.filterName = "BidLoadTenderControl"; f.filterValueFrom = sBidLoadTenderControlVal; f.filterValueTo = ""; f.filterFrom = ""; f.filterTo = "";
                ////s.pushToAllFilters(f);
                //if (!s.FilterValues) { s.FilterValues = [f]; } else { if(!s.filterValuesContains(f.filterName)){ s.FilterValues.push(f);} }
            
                s.ParentControl = sBidLoadTenderControlVal;
                return '';
            }

            function QuotesDataBoundCallBack(e,tGrid){            
                kendo.ui.progress($(document.body), false);          
                oQuotesGrid = tGrid;
                var oDataSource =  oQuotesGrid.dataSource;
                var totalRecords = oDataSource.total();
                var ipNoRatesFoundMsg = "p" + wdgtQuotesWndDialog.GetFieldID("pNoRatesFoundMsg");
                var pNoRatesFoundPar = $("#" + ipNoRatesFoundMsg);
                if (typeof (pNoRatesFoundPar) !== 'undefined' ){
                    if (typeof (totalRecords) !== 'undefined' && totalRecords != null && !isNaN(totalRecords)  && totalRecords > 0 ){
                        //pNoRatesFoundPar.text('The system found ' + totalRecords.toString() + ' quotes');
                        //pNoRatesFoundPar.html('The system found ' + totalRecords.toString() + ' quotes');
                        pNoRatesFoundPar.text("");
                    } else {
                        pNoRatesFoundPar.css({"color": "red","font-weight": "bold","font-size": "large" })
                        pNoRatesFoundPar.text("No quotes were found for this load using the loads temperature and the loads mode of delivery.  All carrier validation rules were applied so the carriers may not be qualified; check the contract and insurance informaton.  We also optimized by capacity, so check that a tariff exists for the capacity of the load.  Please check your configuration and try again.");
                    }
                } 
           
            }

            var blnShowRateItAfterBookingOptions = false;
            //*************  execActionClick  ****************
            function execActionClick(btn, proc){  
                //debugger;          
                if(btn.id == "btnAddBooking"){ openAddNewLoadBoardGridWindow(); } 
                else if(btn.id == "btnOpenLane"){ 
                    //debugger;
                    if (isBookingSelected() === true) {
                        if ("BookODControl" in loadBoardGridSelectedRowDataItem){
                            var lanePK = loadBoardGridSelectedRowDataItem.BookODControl;
                            var setting = {name:'pk', value: lanePK.toString()};
                            var oCRUDCtrl = new nglRESTCRUDCtrl();
                            var blnRet = oCRUDCtrl.update("LaneDetail/PostPageSetting", setting, tPage, "savePostPageSettingSuccessCallback", "savePostPageSettingAjaxErrorCallback", tPage);
                        }
                        location.href = "LaneDetail";
                    }
                } 
                else if(btn === "open_spot_dispatch_page"){ 
                    if (typeof (tPage[proc]) !== 'undefined' && ngl.isObject(tPage[proc])) {
                        tPage[proc].executeActions("save");
                        tPage[proc].close();
                    }
                    //add code to open the spot dispatch page    
                }   
                else if(btn === "saveRateIT"){ 
                    if (typeof (tPage[proc]) !== 'undefined' && ngl.isObject(tPage[proc])) {
                        //alert("Save Rate IT");
                        tPage[proc].executeActions("save");
                        tPage[proc].save();
                    }             
                }    
                else if(btn === "Saved"){ 
                    //kendo.ui.progress($(document.body), false);
                    if (typeof (tPage[proc]) !== 'undefined' && ngl.isObject(tPage[proc])) {
                        if (tPage[proc].sNGLCtrlName ==  "wdgtRateITDialog"){
                            execActionClick("Refresh");
                            if (typeof (tPage[proc].rData) !== 'undefined' && tPage[proc].rData != null && ngl.isArray(tPage[proc].rData) ){
                                //rData for RateIT returns  oGResult where
                                //oGResult.Control = a control number like LoadTenderControl 
                                //oGResult.strField = the field name like  "LoadTenderControl"
                                //oGResult.strField2 = the option selected like  "RateITPostWorkFlowGroup"
                                var oGResult = tPage[proc].rData[0];
                                if (typeof (oGResult) !== 'undefined' && oGResult != null && ngl.isObject(oGResult)){
                                    if (!oGResult.Control || oGResult.Control == 0) {
                                        ngl.showWarningMsg("No Quotes Are Available", "Cannot Generate Quotes, check that the  DATCarrierNumber system parameter is correct", tPage);
                                        return;
                                    }
                                
                                    switch (oGResult.strField2) {
                                        case "RateITWorkFlowSwitchSpotRate":
                                            //this is a spot rate
                                            tPage[proc].executeActions("close");
                                            oDispatchingDialogCtrl.read(oGResult.Control)
                                            break;
                                        case "RateITRatingWorkFlowGroup":  
                                            //this is a quote selection
                                            //open the quote selection window similar to rate shopping
                                            //use the new QuotesWnd dialog
                                            tPage[proc].executeActions("close");
                                            if (typeof (tPage["wdgtQuotesWndDialog"]) !== 'undefined' && ngl.isObject(tPage["wdgtQuotesWndDialog"])){
                                                sBidLoadTenderControlVal = oGResult.Control;
                                                tPage["wdgtQuotesWndDialog"].read(oGResult.Control);
                                            } 
                                            tPage[proc].executeActions("close");
                                            break;
                                        case "RateITPostWorkFlowGroup":
                                            //this is a post to NEXTStop or DAT
                                            //TODO: find out what to show users?     
                                            //Added By LVV on 2/27/29 for bug fix - close the window and refresh the grid
                                            tPage[proc].executeActions("close");
                                            refresh();
                                            break;
                                        default:
                                            break;
                                    }
                                }                          
                            }                       
                        }
                    }             
                }        
                else if(btn.id == "btnOpenCompany"){
                    //debugger;
                    if (isBookingSelected() === true) {
                        if ("BookCustCompControl" in loadBoardGridSelectedRowDataItem){
                            var compPK = loadBoardGridSelectedRowDataItem.BookCustCompControl;
                            var setting = {name:'pk', value: compPK.toString()};
                            var oCRUDCtrl = new nglRESTCRUDCtrl();
                            var blnRet = oCRUDCtrl.update("CompDetail/PostPageSetting", setting, tPage, "savePostPageSettingSuccessCallback", "savePostPageSettingAjaxErrorCallback", tPage);
                        }
                        location.href = "CompanyDetail";
                    }
                } 
                else if(btn.id == "btnOpenCarrier"){ if (isBookingSelected() === true) {location.href = "LECarrierMaint";} } 
                else if(btn.id == "btnGetRates" || btn === "GetRates"){
                    if (isBookingSelected() === true) {
                        var iReady = readyToAssignCarrier();
                        if (iReady === -1) { 
                            blnShowRateItAfterBookingOptions = false; 
                            return;
                        }
                        if (iReady === 1){
                            blnShowRateItAfterBookingOptions = false;
                            if (typeof (tPage["wdgtRateITDialog"]) !== 'undefined' && ngl.isObject(tPage["wdgtRateITDialog"])){ tPage["wdgtRateITDialog"].read(iBookPK); } 
                        } else {
                            blnShowRateItAfterBookingOptions = true;
                            execActionClick("BookingTenderOptions");
                        }                   
                    } 
                }
                else if(btn.id == "btnChangeSHID" || btn === "ChangeSHID"){
                    if (isBookingSelected() === true) {
                        if (typeof (tPage["wdgtChangeSHIDDialog"]) !== 'undefined' && ngl.isObject(tPage["wdgtChangeSHIDDialog"])){ tPage["wdgtChangeSHIDDialog"].read(iBookPK); }         
                    } 
                }
                else if(btn.id === "btnBookingTenderOptions" || btn === "BookingTenderOptions" ){
                    if (isBookingSelected() === true) {
                        var sBookingTenderDialogCTRLName = "wdgtBookingTenderWndDialog";
                        if (typeof (tPage["wdgtBookingTenderWorkFlowOptionCtrlEdit"]) !== 'undefined' && ngl.isObject(tPage["wdgtBookingTenderWorkFlowOptionCtrlEdit"])){
                            tPage["wdgtBookingTenderWorkFlowOptionCtrlEdit"].ReadUserSettings = false;
                        }
                        if (typeof (tPage["wdgtBookingTenderWndDialog"]) !== 'undefined' && ngl.isObject(tPage["wdgtBookingTenderWndDialog"])){
                            tPage["wdgtBookingTenderWndDialog"].clearWdgtHTML();
                            tPage["wdgtBookingTenderWndDialog"].read(iBookPK);
                        } else{alert("Missing HTML Element (wdgtBookingTenderWndDialog is undefined)");} //Add better error handling here if cm stuff is missing
                    } 
                }
                else if(btn === "saveBookingTenderOptions"){ 
                    if (typeof (tPage[proc]) !== 'undefined' && ngl.isObject(tPage[proc])) {
                        tPage[proc].executeActions("close");
                        //tPage[proc].close();
                        //oLoadBoardGrid.dataSource.read();
                        //if (blnShowRateItAfterBookingOptions === true) {
                        //    var iThisBookControl = iBookPK;
                        //    execActionClick("Refresh");
                        //    var oRow = oLoadBoardGrid.table.find('tr[data-id="' + iBookPK + '"]');
                        //    if (typeof (oRow) !== 'undefined' && ngl.isObject(oRow)) {
                        //        oLoadBoardGrid.select(oRow);
                        //        saveBookPK();
                        //        execActionClick("GetRates");
                        //    }                    
                        //}
                    }             
                } 
                else if (btn.id == "btnStopResequence" ){ if (isBookingSelected() === true){stopResequence(iBookPK);} }
                else if (btn.id == "btnGetMiles" ){ if (isBookingSelected() === true){getMiles(iBookPK);} }
                else if (btn.id == "btnMapIt" ){ if (isBookingSelected() === true){BingMapsCaller();} } //Bing Maps
                else if (btn.id == "btnTrackIt" ){ if (isBookingSelected() === true){TrackIt();} } //Bing Maps
                else if (btn.id == "btnRefresh" || btn === "Refresh" ){ refresh(); }
                else if (btn.id == "btnResetCurrentUserConfig"){ resetCurrentUserConfig(PageControl); }
                else if (btn.id == "btnOpenRevenue"){ if (isBookingSelected() === true){OpenRevenue(iBookPK);} }
                else if (btn.id == "btnOpenItem"){ if (isBookingSelected() === true){OpenItem(iBookPK);} }
                else if (btn.id == "btnDuplicatePro"){ if (isBookingSelected() === true){DuplicatePro(iBookPK);} }
                else if (btn.id == "btnNewSequence"){ if (isBookingSelected() === true){NewSequence(iBookPK);} }
                else if (btn.id == "btnOpenLoadRevenue"){ if (isBookingSelected() === true){OpenLoadRevenue(iBookPK);} }
                else if (btn.id == "btnCreateBooking"){ btnManageItems_Click(); } //Added By LVV on 4/20/20 LBDemo
                else if (btn.id == "btnOpenLoadStatus"){ OpenLoadStatus(); } //Added By LVV on 4/27/20               
                else if (btn.id == "btnOpenLoadNotes"){ OpenLoadNotes(); } //Added By LVV on 6/23/20 for v-8.2.1.008 Task #20200609162832 - Create Book Notes page and add Navigation item to the Load Board
                else if (btn.id == "btnOpenLoadCarData"){ OpenLoadCarrierData(); } //Added By LVV on 6/23/20 for v-8.2.1.008 Task #20200609162842 - Create Carrier Data Page
                else if(btn.id == "btnAdjustCredit" || btn === "AdjustCredit"){
                    //Added By LVV on 7/14/20 for v-8.3.0.001 Task #20200609155542 - Credit Hold
                    if (isBookingSelected() === true) {
                        if ("BookCustCompControl" in loadBoardGridSelectedRowDataItem){
                            var compControl = loadBoardGridSelectedRowDataItem.BookCustCompControl;
                            if (typeof (tPage["wdgtAdjustCreditWndDialog"]) !== 'undefined' && ngl.isObject(tPage["wdgtAdjustCreditWndDialog"])){ tPage["wdgtAdjustCreditWndDialog"].read(compControl); }  
                        }
                    } 
                }
                else if(btn.id == "btnNewClaim"){ if (isBookingSelected() === true) {createNewClaim(); } }
                else if (btn.id == "btnRunOptimizer"){ 
                    var intGridRecords = oLoadBoardGrid.dataSource.total();                 
                    openOptimizerWnd(intGridRecords);                    
                } //Added By LVV on 9/16/20 for v-8.3.0.001 - Optimizer 365 
                else if(btn.id == "btnOpenDAT"){ location.href = "DAT"; } //Added By LVV on 9/30/20 for v-8.3.0.001 Task #20200930125350 - DAT Migration
            }

            function CreateNewClaimCallBack(data){ location.href = "Claims"; }
            function CreateNewClaimAjaxErrorCallback(xhr, textStatus, error){ ngl.showErrMsg("Create New Claim Failure", formatAjaxJSONResponsMsgs(xhr, textStatus, error,'Failed'), tPage); }

            function createNewClaim(){

                kendo.ui.progress(oLoadBoardGrid.element, true);
                var oCRUDCtrl = new nglRESTCRUDCtrl();
                var blnRet = oCRUDCtrl.read("Claims/GetNewClaim", iBookPK, tPage, "CreateNewClaimCallBack", "CreateNewClaimAjaxErrorCallback",true);
            }

            //************* Report Mapping **********************
            function execReportClick(btn, proc){
                if(btn.id == "btnCAR120" ){ if (isBookingSelected() === true && blnCanPrintBOL === true){PrintSelectedBOL();} }        
                else if(btn.id == "btnDispatchReport" ){ if (isBookingSelected() === true && blnCanPrintDispatch === true){openDispatchReport();} }
                else if(btn.id == "btnCAR106" ){ if (isBookingSelected() === true){openLoadTenderReport(iBookPK);} }
            }

            function refresh() { ngl.readDataSource(oLoadBoardGrid); }

            var blnLoadBoardGridChangeBound = false;
            //*************  Call Back Functions ****************
            function LoadBoardGridDataBoundCallBack(e,tGrid){           
                oLoadBoardGrid = tGrid;
                if (blnLoadBoardGridChangeBound == false){
                    oLoadBoardGrid.bind("change", saveBookPK);
                    blnLoadBoardGridChangeBound = true;
                }        
                //if iBookPK is not 0 select that row in the grid
                if (typeof (iBookPK) !== 'undefined' && iBookPK !== null && iBookPK !== 0) {
                    var rows = oLoadBoardGrid.items();
                    $(rows).each(function(e) {
                        var row = this;
                        var dataItem = oLoadBoardGrid.dataItem(row);
                        if (dataItem.BookControl == iBookPK) { 
                            oLoadBoardGrid.select(row); 

                        }
                    });
                }
            }

            var spotRateFastTab = new NGLFastTabCtrl();
            var spotRateDetails = new NGLWorkFlowSectionCtrl()
    
            function BookingTenderWorkFlowOptionCtrlCB(oResults){                     
                try {            
                    //This parameter has a reference to the workFlowSettings in oResults.data when oResults.datatype = "WorkFlowSetting";
                    //Check that oResults.source = "readUserSettingsSuccessCallback";
                    //And oResults.msg = "Success"           
                    //You can then use custom code to modify the visibility of the workFlowSettings before the load HTML method is called.  Just make sure you do not call any asynchronous methods or you will need to refresh the page.           
                    //I suggest you read any settings needed before you call read for the widget then when the call back is triggered you have everything you need to show or hide check boxes,  like the trans code or other options.       
                    if (oResults){
                        if (oResults.source === "readUserSettingsSuccessCallback" && oResults.msg === "Success"){
                            if (oResults.datatype === "WorkFlowSetting"){                      
                                var actions = getAvailableBookingActions();
                                var l = oResults.data.length;                                               
                                for (var j=0; j < l; j++) { 
                                    var fName = oResults.data[j].fieldName;      
                                    if (fName.substring(0, 2) === "yn"){    
                                        //check if the name of the workflow object is in the actions array
                                        if($.inArray(fName, actions) >= 0){ oResults.data[j].fieldVisible = true; oResults.data[j].fieldLockVisibility = "false"; } else{ oResults.data[j].fieldVisible = false; oResults.data[j].fieldLockVisibility = "true"; }                                                             
                                    }                      
                                }                    
                            }                                                      
                        }          
                    }     
                } catch (err) { ngl.showErrMsg("Cannot Continue", "There was an unexpected error, please reload the screen. If this continues please contact technical support. Error: " + err.message, tPage); }       
            }

            function getAvailableBookingActions(){   
                //Get the dataItem for the selected row
                if (typeof (loadBoardGridSelectedRowDataItem) === 'undefined' || loadBoardGridSelectedRowDataItem == null) { ngl.showValidationMsg("Booking Record Required", "Please select a Booking to continue", tPage); return false; }
                var actions = new Array();           
                var bkControl = 0, carrierControl = 0, routeConsFlag = 0;
                var tranCode = "", dtOrdered = "", dtRequired = "", dtLoad = "", dtInvoice = "", dtDelivered = "", shid = "";
                var lockAllCosts = true, blnNEXTStop = false, blnDAT = false;
                if ("BookControl" in loadBoardGridSelectedRowDataItem){ bkControl = loadBoardGridSelectedRowDataItem.BookControl; } 
                if ("BookTranCode" in loadBoardGridSelectedRowDataItem){ tranCode = loadBoardGridSelectedRowDataItem.BookTranCode; } 
                if ("BookDateOrdered" in loadBoardGridSelectedRowDataItem){ dtOrdered = loadBoardGridSelectedRowDataItem.BookDateOrdered; } 
                if ("BookDateRequired" in loadBoardGridSelectedRowDataItem){ dtRequired = loadBoardGridSelectedRowDataItem.BookDateRequired; } 
                if ("BookDateLoad" in loadBoardGridSelectedRowDataItem){ dtLoad = loadBoardGridSelectedRowDataItem.BookDateLoad; } 
                if ("BookFinARInvoiceDate" in loadBoardGridSelectedRowDataItem){ dtInvoice = loadBoardGridSelectedRowDataItem.BookFinARInvoiceDate; } 
                if ("BookCarrierControl" in loadBoardGridSelectedRowDataItem){ carrierControl = loadBoardGridSelectedRowDataItem.BookCarrierControl; } 
                if ("BookLockAllCosts" in loadBoardGridSelectedRowDataItem){ lockAllCosts = loadBoardGridSelectedRowDataItem.BookLockAllCosts; } 
                if ("BookSHID" in loadBoardGridSelectedRowDataItem){ shid = loadBoardGridSelectedRowDataItem.BookSHID; } 
                if ("BookDateDelivered" in loadBoardGridSelectedRowDataItem){ dtDelivered = loadBoardGridSelectedRowDataItem.BookDateDelivered; } 
                if ("BookRouteConsFlag" in loadBoardGridSelectedRowDataItem){ routeConsFlag = loadBoardGridSelectedRowDataItem.BookRouteConsFlag; } 
                if ("NEXTStop" in loadBoardGridSelectedRowDataItem){ blnNEXTStop = loadBoardGridSelectedRowDataItem.NEXTStop; } 
                if ("DAT" in loadBoardGridSelectedRowDataItem){ blnDAT = loadBoardGridSelectedRowDataItem.DAT; } 
                var blnCanSingle = false;
                var dtNow = Date.now();
                if(dtDelivered){ if(dtDelivered < dtNow && routeConsFlag == 0){ blnCanSingle = true; } }
                if (bkControl == 0){ return; } //make everything invisible
                if (typeof (tranCode) === 'undefined' || tranCode == null || tranCode.length < 1){ return; } //make everything invisible
                if (!dtLoad){ return; } //make everything invisible
                if (!dtOrdered){ return; } //make everything invisible
                if (!dtRequired){ return; } //make everything invisible
                //Select Case BookTranCode
                if (tranCode.toUpperCase() === "N"){                
                    actions.push("ynRemoveOrder");
                }
                else if(tranCode.toUpperCase() === "P") {               
                    if (carrierControl > 0) {
                        actions.push("ynUnassignProvider");                      
                        if (lockAllCosts === false){ if(ngl.stringHasValue(shid)){ actions.push("ynReject"); } } //they have to reject the load if an shid is already assigned so the system will create a new shid for the new carrier. 
                        if (blnNEXTStop === false && blnDAT === false) {
                            actions.push("ynRemoveOrder");
                            actions.push("ynTender");
                            actions.push("ynAccept");
                            actions.push("ynInvoice");                                       
                            if (blnCanSingle === true){ actions.push("ynInvoiceSingle"); } 
                        }
                    }   
                }
                else if(tranCode.toUpperCase() === "PC") {               
                    if (carrierControl > 0) {
                        actions.push("ynReject");                   
                        if (blnNEXTStop === false && blnDAT === false) { 
                            actions.push("ynModify"); 
                            actions.push("ynAccept");
                            actions.push("ynInvoice");
                            if (blnCanSingle === true) { actions.push("ynInvoiceSingle"); }  
                        }                 
                    }
                }
                else if(tranCode.toUpperCase() === "PB") {               
                    if (carrierControl > 0) {
                        actions.push("ynDrop");
                        if (blnNEXTStop === false && blnDAT === false) {
                            actions.push("ynModify");
                            actions.push("ynInvoice");                 
                            if (blnCanSingle === true) { actions.push("ynInvoiceSingle"); }
                        }                 
                    }
                }
                else if(tranCode.toUpperCase() === "I") {              
                    if (carrierControl > 0) {
                        actions.push("ynReject");
                        actions.push("ynModify");                    
                        if (ngl.stringHasValue(dtInvoice) === true) {
                            actions.push("ynInvoiceComplete");                                      
                            if (blnCanSingle === true) { actions.push("ynInvoiceCompleteSingle"); }                   
                        }                   
                    }
                }
                else if(tranCode.toUpperCase() === "IC") {                  
                    if (carrierControl > 0) {
                        actions.push("ynReject");
                        actions.push("ynModify");
                        actions.push("ynInvoice");                   
                        if (blnCanSingle === true) { actions.push("ynInvoiceSingle"); }                  
                    }         
                }
                return actions
            }

            // returns an integer 
            // -1 indicates that the booing is not selected or is not valid 
            //    a message is already displayed so the caller should just return
            //  0 indicates that the booking options dialog is required
            //  1 indicates success and to show the Rate It Options Dialog
            function readyToAssignCarrier(){   
                //Get the dataItem for the selected row
                var iRet = 1;
                if (typeof (loadBoardGridSelectedRowDataItem) === 'undefined' || loadBoardGridSelectedRowDataItem == null) { ngl.showValidationMsg("Booking Record Required", "Please select a Booking to continue", tPage); return -1; }             
                var bkControl = 0, carrierControl = 0, routeConsFlag = 0;
                var tranCode = "", dtOrdered = "", dtRequired = "", dtLoad = "", dtInvoice = "", dtDelivered = "", shid = "";
                var lockAllCosts = true;
                var blnOnCreditHold = false; //Added By LVV on 7/14/20 for v-8.3.0.001 Task #20200609155542 - Credit Hold
                if ("BookControl" in loadBoardGridSelectedRowDataItem){ bkControl = loadBoardGridSelectedRowDataItem.BookControl; } 
                if ("BookTranCode" in loadBoardGridSelectedRowDataItem){ tranCode = loadBoardGridSelectedRowDataItem.BookTranCode; } 
                if ("BookDateOrdered" in loadBoardGridSelectedRowDataItem){ dtOrdered = loadBoardGridSelectedRowDataItem.BookDateOrdered; } 
                if ("BookDateRequired" in loadBoardGridSelectedRowDataItem){ dtRequired = loadBoardGridSelectedRowDataItem.BookDateRequired; } 
                if ("BookDateLoad" in loadBoardGridSelectedRowDataItem){ dtLoad = loadBoardGridSelectedRowDataItem.BookDateLoad; } 
                if ("BookFinARInvoiceDate" in loadBoardGridSelectedRowDataItem){ dtInvoice = loadBoardGridSelectedRowDataItem.BookFinARInvoiceDate; } 
                if ("BookCarrierControl" in loadBoardGridSelectedRowDataItem){ carrierControl = loadBoardGridSelectedRowDataItem.BookCarrierControl; } 
                if ("BookLockAllCosts" in loadBoardGridSelectedRowDataItem){ lockAllCosts = loadBoardGridSelectedRowDataItem.BookLockAllCosts; } 
                if ("BookSHID" in loadBoardGridSelectedRowDataItem){ shid = loadBoardGridSelectedRowDataItem.BookSHID; } 
                if ("BookDateDelivered" in loadBoardGridSelectedRowDataItem){ dtDelivered = loadBoardGridSelectedRowDataItem.BookDateDelivered; } 
                if ("BookRouteConsFlag" in loadBoardGridSelectedRowDataItem){ routeConsFlag = loadBoardGridSelectedRowDataItem.BookRouteConsFlag; }
                if ("OnCreditHold" in loadBoardGridSelectedRowDataItem){ blnOnCreditHold = loadBoardGridSelectedRowDataItem.OnCreditHold; } //Added By LVV on 7/14/20 for v-8.3.0.001 Task #20200609155542 - Credit Hold
                if(blnOnCreditHold){ ngl.showValidationMsg("Credit Hold Warning", "Cannot Rate a load that is on Credit Hold", tPage); return -1; } //Added By LVV on 7/14/20 for v-8.3.0.001 Task #20200609155542 - Credit Hold
                
                var dtNow = Date.now();
                var sMessage = "Because ";
                var sSpacer = "";
                if (dtDelivered){ 
                    if(dtDelivered < dtNow && routeConsFlag == 0){ iRet = -1; sMessage = sMessage +   "the selected Booking has already been delivered"; sSpacer = " , and "; } 
                }
                if (bkControl == 0){ iRet = -1; sMessage = sMessage +  sSpacer +  "the selected Booking record has not been saved"; sSpacer = " , and "; } 
                if (typeof (tranCode) === 'undefined' || tranCode == null || tranCode.length < 1){ iRet = -1; sMessage = sMessage +  sSpacer +  "the tran code is not valid"; sSpacer = " , and "; } 
                if (!dtLoad){ iRet = -1; sMessage = sMessage +  sSpacer +  "the load date is not valid"; sSpacer = " , and "; } 
                if (!dtOrdered){ iRet = -1; sMessage = sMessage +  sSpacer +  "the order date is not valid"; sSpacer = " , and "; } 
                if (!dtRequired){ iRet = -1; sMessage = sMessage +  sSpacer +  "the required date is not valid"; sSpacer = " , and "; }            
                if (iRet === -1) { ngl.showValidationMsg("Cannot Change Carrier", sMessage, tPage); return  iRet; }          
                //Select Case BookTranCode
                if(tranCode.toUpperCase() === "N" || tranCode.toUpperCase() === "P"){ return 1; } else{ return 0; }                     
            }


            function BookingTenderWndCB(oResults){          
                if(oResults.widget.sNGLCtrlName === "wdgtBookingTenderWndDialog" && oResults.source === "saveSuccessCallback"){
                    var iThisBookControl = iBookPK;
                    //oLoadBoardGrid.dataSource.read();
                    //modified by RHR on 2/27/2019 for v-8.2  to be consistent with other functionality
                    //just in case we add more data or logic to refresh option.
                    execActionClick("Refresh");
                    oResults.widget.executeActions("close");

                    if (blnShowRateItAfterBookingOptions === true) {
                        blnShowRateItAfterBookingOptions = false;
                        ngl.showValidationMsg("Retry Required ", "Please select a Booking record and try to rate the load again", tPage);
                    }
                    //if (blnShowRateItAfterBookingOptions === true) {
                    //    blnShowRateItAfterBookingOptions = false;
                    //    setTimeout(function (iThisBookControl) {
                    //        //we are in the middle of selecting a new carrier so hopefully we can continue 
                    //        //now and it will work. first try to select the previous booking record
                    //        var oGrid1 = $('#id621201809251302564013007')  //.kendoNGLGrid tPage.oLoadBoardGrid;
                    //        var oGrid = tPage.oLoadBoardGrid;
                    //        //var data = oGrid.data("kendoNGLGrid");
                    //        var oRow6 = tPage.oLoadBoardGrid.table.find('tr[data-id="' + iThisBookControl + '"]')
                    //        if ("BookControl" in oRow6) {
                    //            //tPage.loadBoardGridSelectedRowDataItem = oRow1;
                    //            var iiiBookPK = oRow6.BookControl;
                    //        }
                    //        var oRow3 = $('#id621201809251302564013007').data("kendoNGLGrid").table.find('tr[data-id="' + iThisBookControl + '"]')
                    //        var oRow4 = data.table.find('tr[data-id="' + iThisBookControl + '"]')
                    //        var oRow5 = oGrid.data("kendoNGLGrid").table.find('tr[data-id="' + iThisBookControl + '"]')
                    //        var data1 = oGrid1.data("kendoNGLGrid");
                    //        var oRow2 = data1.table.find('tr[data-id="' + iThisBookControl + '"]')
                    //        var oRow1 = $('#id621201809251302564013007').data("kendoNGLGrid").table.find('tr[data-id="' + iThisBookControl + '"]')
                    //        if ("BookControl" in oRow1) {
                    //            tPage.loadBoardGridSelectedRowDataItem = oRow1;
                    //            tPage.iBookPK = oRow1.BookControl;
                    //            var setting = { name: 'pk', value: iBookPK.toString() };
                    //            var oCRUDCtrl = new nglRESTCRUDCtrl();
                    //            var blnRet = oCRUDCtrl.update("LoadBoard/PostPageSetting", setting, tPage, "savePostPageSettingSuccessCallback", "savePostPageSettingAjaxErrorCallback", tPage);
                    //            setTimeout(function () {
                    //                tPage.execActionClick("GetRates");
                    //            }, 1000);
                    //        }
                    //        return;
                    //        if (typeof (oGrid) !== 'undefined' && ngl.isObject(oGrid)) {
                    //            var oRow = oGrid.table.find('tr[data-id="' + iThisBookControl + '"]');
                    //            if (typeof (oRow) !== 'undefined' && ngl.isObject(oRow)) {
                    //                oGrid.select(oRow);
                    //                //tPage.saveBookPK();
                    //                tPage.loadBoardGridSelectedRowDataItem = oRow;// oLoadBoardGrid.dataItem(loadBoardGridSelectedRow);
                    //                if (typeof (tPage.loadBoardGridSelectedRowDataItem) === 'undefined' || tPage.loadBoardGridSelectedRowDataItem == null) {
                    //                    ngl.showValidationMsg("Cannot Auto Select the Previous Booking Record ", "Please select a Booking to continue", tPage);
                    //                    return false;
                    //                }
                    //                if ("BookControl" in tPage.loadBoardGridSelectedRowDataItem) {
                    //                    tPage.iBookPK = tPage.loadBoardGridSelectedRowDataItem.BookControl;
                    //                    var setting = { name: 'pk', value: iBookPK.toString() };
                    //                    var oCRUDCtrl = new nglRESTCRUDCtrl();
                    //                    var blnRet = oCRUDCtrl.update("LoadBoard/PostPageSetting", setting, tPage, "savePostPageSettingSuccessCallback", "savePostPageSettingAjaxErrorCallback", tPage);
                    //                } else {
                    //                    ngl.showValidationMsg("Cannot Auto Select the Previous Booking Record ", "Please select a Booking to continue", tPage);
                    //                    return false;
                    //                }
                    //                setTimeout(function () {
                    //                    tPage.execActionClick("GetRates");
                    //                }, 1000);
                    //            }
                    //        }
                    //    }, 1000, iThisBookControl);
                    //}
                }          
            }

            function RateITCB(oResults){             
                if (typeof (oResults) === 'undefined' || ngl.isObject(oResults) === false) { return;}
                if(oResults.widget.sNGLCtrlName === "wdgtRateITDialog" && oResults.source === "saveSuccessCallback"){
                    if (typeof (oResults.Dialog) !== 'undefined' && oResults.Dialog != null ) { oResults.Dialog.data("kendoDialog").toFront(); }             
                }          
            }

        
            function ChangeSHIDCB(oResults){          
                if(oResults.widget.sNGLCtrlName === "wdgtChangeSHIDDialog" && oResults.source === "saveSuccessCallback"){
                    var iThisBookControl = iBookPK;
                    //oLoadBoardGrid.dataSource.read();
                    //modified by RHR on 2/27/2019 for v-8.2  to be consistent with other functionality
                    //just in case we add more data or logic to refresh option.
                    execActionClick("Refresh");
                    oResults.widget.executeActions("close");
                }          
            }

            
            //Created By LVV on 7/14/20 for v-8.3.0.001 Task #20200609155542 - Credit Hold
            function AdjustCreditWndCB(oResults){          
                if(oResults.widget.sNGLCtrlName === "wdgtAdjustCreditWndDialog" && oResults.source === "saveSuccessCallback"){
                    execActionClick("Refresh");
                    oResults.widget.executeActions("close");
                }          
            }


            //*************  Action Menu Functions ****************
            function BingMapsCaller() { 
                var wgt = 0;
                if ("BookTotalWgt" in loadBoardGridSelectedRowDataItem) { wgt = loadBoardGridSelectedRowDataItem.BookTotalWgt; } 
                MapIt(iBookPK,wgt);
            }


            function stopReseqGetMilesSuccessCallback(data, errTitle){
                try {
                    kendo.ui.progress(oLoadBoardGrid.element, false);                
                    var blnSuccess = false;
                    var blnErrorShown = false;
                    var strValidationMsg = "";
                    if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                        if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg(errTitle, data.Errors, null); }
                        else {
                            if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                    blnSuccess = true;                             
                                    if (data.Data[0].Success === true) {
                                        if (ngl.stringHasValue(data.Data[0].SuccessMsg)){ ngl.showSuccessMsg(data.Data[0].SuccessMsg, null); }                                   
                                    }
                                    else{
                                        if (ngl.stringHasValue(data.Data[0].ErrMsg)){ ngl.showErrMsg(errTitle, data.Data[0].ErrMsg, null); }
                                    }
                                    if (ngl.stringHasValue(data.Data[0].WarningMsg)){ ngl.showWarningMsg("", data.Data[0].WarningMsg, null); }
                                }
                            }
                        }
                    }
                    if (blnSuccess === false && blnErrorShown === false) {
                        if (strValidationMsg.length < 1) { strValidationMsg = errTitle; }
                        ngl.showErrMsg(errTitle, strValidationMsg, null);
                    }
                    execActionClick("Refresh");
                } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
            }

            function stopResequenceSuccessCallback(data){ stopReseqGetMilesSuccessCallback(data, "Stop Resequence Failure"); }
            function stopResequenceAjaxErrorCallback(xhr, textStatus, error){ ngl.showErrMsg("Stop Resequence Failure", formatAjaxJSONResponsMsgs(xhr, textStatus, error,'Failed'), tPage); }
            function getMilesSuccessCallback(data){ stopReseqGetMilesSuccessCallback(data, "Get Miles Failure"); }
            function getMilesAjaxErrorCallback(xhr, textStatus, error){ ngl.showErrMsg("Get Miles Failure", formatAjaxJSONResponsMsgs(xhr, textStatus, error,'Failed'), tPage); }

            function stopResequence(iBookPK){
                kendo.ui.progress(oLoadBoardGrid.element, true);
                var oCRUDCtrl = new nglRESTCRUDCtrl();
                var blnRet = oCRUDCtrl.read("LoadBoard/StopResequence", iBookPK, tPage, "stopResequenceSuccessCallback", "stopResequenceAjaxErrorCallback",true);
            }

            function getMiles(iBookPK){
                kendo.ui.progress(oLoadBoardGrid.element, true);
                var oCRUDCtrl = new nglRESTCRUDCtrl();
                var blnRet = oCRUDCtrl.read("LoadBoard/GetMiles", iBookPK, tPage, "getMilesSuccessCallback", "getMilesAjaxErrorCallback",true);
            }

            function OpenRevenue(iBookPK){
                if (isBookingSelected() === true) {location.href = "LoadBoardRevenue";}
                //kendo.ui.progress(oLoadBoardGrid.element, true);
                //var oCRUDCtrl = new nglRESTCRUDCtrl();
                //var blnRet = oCRUDCtrl.read("LoadBoard/GetMiles", iBookPK, tPage, "getMilesSuccessCallback", "getMilesAjaxErrorCallback",true);
            }

            function OpenLoadRevenue(iBookPK){
                if (isBookingSelected() === true) {location.href = "LoadBoardLoadRevenue";}
                //kendo.ui.progress(oLoadBoardGrid.element, true);
                //var oCRUDCtrl = new nglRESTCRUDCtrl();
                //var blnRet = oCRUDCtrl.read("LoadBoard/GetMiles", iBookPK, tPage, "getMilesSuccessCallback", "getMilesAjaxErrorCallback",true);
            }

            //Added By LVV on 4/27/20
            function OpenLoadStatus(){ if (isBookingSelected() === true) {location.href = "LoadBoardLoadStatus";} }

            //Added By LVV on 6/23/20 for v-8.2.1.008 Task #20200609162832 - Create Book Notes page and add Navigation item to the Load Board
            function OpenLoadNotes(){ if (isBookingSelected() === true) {location.href = "LoadBoardNotes";} }

            //Added By LVV on 6/23/20 for v-8.2.1.008 Task #20200609162842 - Create Carrier Data Page
            function OpenLoadCarrierData(){ if (isBookingSelected() === true) {location.href = "LoadBoardCarrierData";} }


            function OpenItem(iBookPK){
                if (isBookingSelected() === true) {location.href = "LoadBoardItems";}
                //alert("Open Item");
                //kendo.ui.progress(oLoadBoardGrid.element, true);
                //var oCRUDCtrl = new nglRESTCRUDCtrl();
                //var blnRet = oCRUDCtrl.read("LoadBoard/GetMiles", iBookPK, tPage, "getMilesSuccessCallback", "getMilesAjaxErrorCallback",true);
            }
        
            function DuplicatePro(iBookPK){
                alert("Duplicate Pro");
                //kendo.ui.progress(oLoadBoardGrid.element, true);
                //var oCRUDCtrl = new nglRESTCRUDCtrl();
                //var blnRet = oCRUDCtrl.read("LoadBoard/GetMiles", iBookPK, tPage, "getMilesSuccessCallback", "getMilesAjaxErrorCallback",true);
            }
        
            function NewSequence(iBookPK){
                alert("New Sequence");
                //kendo.ui.progress(oLoadBoardGrid.element, true);
                //var oCRUDCtrl = new nglRESTCRUDCtrl();
                //var blnRet = oCRUDCtrl.read("LoadBoard/GetMiles", iBookPK, tPage, "getMilesSuccessCallback", "getMilesAjaxErrorCallback",true);
            }

            // ************** Start BookPkgGrid Functions ******************
        
            //Widget object is wdgtBookPkgGridEdit
            var iBookPkgPkgDescControlID = '0';
            var ddlBookPkgPkgDescControl = undefined;
            //widget call back
            function BookPkgGridCB(oResults){
                if (!oResults) { return;}
                if (oResults.source == "showWidgetCallback"   ){
                    //if (oResults.source == "showWidgetCallback"  && iCarrTarFeesAccessorialCodeID == '0'  ){
                    iBookPkgPkgDescControlID = wdgtBookPkgGridEdit.GetFieldID("BookPkgPkgDescControl");
                    ddlBookPkgPkgDescControl = $("#" + iBookPkgPkgDescControlID).data("kendoDropDownList");
                    if (ddlBookPkgPkgDescControl){ ddlBookPkgPkgDescControl.bind("change", BookPkgPkgDescControl_change); }
                }        
            }
            //change event handler
            function BookPkgPkgDescControl_change(e) {
                if (ddlBookPkgPkgDescControl){
                    var iCodeValue = ddlBookPkgPkgDescControl.value();
                    if (iCodeValue){ updateSelectedBookPkgPkgDesc(iCodeValue); }
                }
            }
            //read data to update children when list selection changes        
            function updateSelectedBookPkgPkgDesc(key){           
                var oCRUDCtrl = new nglRESTCRUDCtrl();
                var blnRet = oCRUDCtrl.read("PackageDescription", key, tPage, "readBookPkgPkgDescCallback", "readBookPkgPkgDescAjaxErrorCallback",tPage);
                return true;
            }        
            //read selected data call back from list
            // used to update children when list selection changes       
            function readBookPkgPkgDescCallback(data){
                try {
                    if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                        if (typeof (data.Errors) !== 'undefined' && data.Errors != null) {
                            //on error do nothing for now
                            //oResults.error = new Error();
                            //oResults.error.name = "Read " + this.DataSourceName + " Failure";
                            //oResults.error.message = data.Errors;
                            //ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                        }
                        else if (data.Data != null) {
                            var record = data.Data[0];                                                   
                            if (typeof (record) !== 'undefined' && record != null && ngl.isObject(record)) {
                                //AccessorialVariableCode maps to CalcFormula List	CarrTarFeesVariableCode  DropDownList
                                //get the id
                                var sVal = '';
                                var sFieldID = ''   
                                if ("PkgDescDesc" in record) {
                                    sVal = record["PkgDescDesc"];
                                    sFieldID = wdgtBookPkgGridEdit.GetFieldID("BookPkgDescription");
                                    var domItem = $("#" + sFieldID).data("kendoMaskedTextBox");
                                    if (domItem ){ domItem.value(sVal); }
                                }                           
                                if ("PkgDescFAKClass" in record) {
                                    sVal = record["PkgDescFAKClass"];
                                    sFieldID = wdgtBookPkgGridEdit.GetFieldID("BookPkgFAKClass");
                                    var domItem = $("#" + sFieldID).data("kendoMaskedTextBox");
                                    if (domItem ){ domItem.value(sVal); }
                                }                           
                                if ("PkgDescNMFCClass" in record) {
                                    sVal = record["PkgDescNMFCClass"];
                                    sFieldID = wdgtBookPkgGridEdit.GetFieldID("BookPkgNMFCClass");
                                    var domItem = $("#" + sFieldID).data("kendoMaskedTextBox");
                                    if (domItem ){ domItem.value(sVal); }
                                }
                                if ("PkgDescNMFCSubClass" in record) {
                                    sVal = record["PkgDescNMFCSubClass"];
                                    sFieldID = wdgtBookPkgGridEdit.GetFieldID("BookPkgNMFCSubClass");
                                    var domItem = $("#" + sFieldID).data("kendoMaskedTextBox");
                                    if (domItem ){ domItem.value(sVal); }
                                }
                            }
                        }
                    } 
                } catch (err) { ngl.showErrMsg(err.name, err.message, tObj); }          
            }
            //handle any ajax errors
            function readBookPkgPkgDescAjaxErrorCallback(xhr, textStatus, error){
                ngl.showErrMsg("Read Package Desciption Failure", formatAjaxJSONResponsMsgs(xhr, textStatus, error, 'Failed'), tObj);
            }

            // ************** End BookPkgGrid Functions ******************



            $(document).ready(function () {         
                var PageMenuTab = <%=PageMenuTab%>;             
                
            
                if (control != 0){
                    //default this to disabled until the user selects a record
                    $('#btnCAR120').prop('disabled', true); 
                    blnCanPrintBOL = false;
                    $('#btnDispatchReport').prop('disabled', true); 
                    blnCanPrintDispatch = false;
              

                    oDispatchingDialogCtrl = new DispatchingDialogCtrl();
                    oDispatchingDialogCtrl.loadDefaults(winDispatchingDialog,oDispatchingDialogSelectCB,oDispatchingDialogSaveCB,oDispatchingDialogCloseCB,oDispatchingDialogReadCB);
          
                    oDispatchReportCtrl = new DispatchingReportCtrl();
                    oDispatchReportCtrl.loadDefaults(winDispatchReport,oDispatchingReportSelectCB,oDispatchingReportSaveCB,oDispatchingReportCloseCB,oDispatchingReportReadCB);

                    oBOLReportCtrl = new BOLReportCtrl(); 
                    oBOLReportCtrl.loadDefaults(winBOLReport,oBOLReportSelectCB,oBOLReportSaveCB,oBOLReportCloseCB,oBOLReportReadCB);

                    getPageSettings(tPage, "LoadBoard", "pk", false);
                }             
                var PageReadyJS = <%=PageReadyJS%>;
                menuTreeHighlightPage(); //must be called after PageReadyJS
                var divWait = $("#h1Wait");                         
                if (typeof (divWait) !== 'undefined') { divWait.hide(); }          
            });
        </script>
        <style>
            /* #grid { width : 490px; }  .k-grid tbody tr{ height: 38px; }  */
            .k-grid tbody tr td { overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }

            .k-tooltip { max-height: 500px; max-width: 450px; overflow-y: auto; }

            .k-grid tbody .k-grid-Edit { min-width: 0; }

                .k-grid tbody .k-grid-Edit .k-icon { margin: 0; }

            .RateITOptions ul { margin: 0; padding: 0; max-width: 255px; }

                .RateITOptions ul li {
                    margin: 0;
                    padding: 10px 0px 0px 20px;
                    min-height: 25px;
                    line-height: 25px;
                    vertical-align: middle;
                    /*border: 1px solid rgba(128,128,128,.5);*/
                    border-top: 1px solid rgba(128,128,128,.5);
                }

            .RateITOptions { min-width: 220px; padding: 0; position: relative; }

                .RateITOptions ul li .km-switch { float: right; }

            .RateITOptions-head { height: 50px; background-color: skyblue; }
        </style>
    </div>
</body>
</html>