<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="All.aspx.cs" Inherits="DynamicsTMS365.All" %>

<!DOCTYPE html>

<html>
    <head >
        <title>DTMS All</title>
         <%=cssReference%>  
         <style>
            html,
            body {height:100%; margin:0; padding:0;}
            html {font-size: 12px; font-family: Arial, Helvetica, sans-serif; overflow:hidden;}
        </style>
        <meta charset="utf-8" /> <%--Bing Maps--%>
    </head>
    <body>      
        <%=jssplitter2Scripts%>
        <%=BingMapsJS%>  <%--Bing Maps--%>
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

    <% Response.WriteFile("~/Views/AllEditWindow.html"); %>
    <% Response.WriteFile("~/Views/DispatchReport.html"); %>
    <% Response.WriteFile("~/Views/BOLReport.html"); %> 
          
    <% Response.WriteFile("~/Views/MapItTrimble.html"); %>

    <%--<% Response.WriteFile("~/Views/MapWindow.html"); %> --%><%--Bing Maps--%>

    <% Response.Write(PageTemplates); %>
               
    <%-- HelpWindow needs to go after AuthNotification because it uses the notification object created there --%>
    <% Response.Write(AuthLoginNotificationHTML); %>   
    <% Response.WriteFile("~/Views/HelpWindow.html"); %>        
    <script>
        <% Response.Write(ADALPropertiesjs); %>

        var PageControl = '<%=PageControl%>'; 
        var tPage = this;
        var tObjPG = this; 
        var tObj = this;
        var oAllGrid = null;    
        var UserGroupCategory = '<%=UserGroupCategory%>';
        <% Response.Write(NGLOAuth2); %>
      

        var iBookPK = 0; //Control = BookControl  
        var allGridSelectedRow;
        var allGridSelectedRowDataItem; 
        var blnCanPrintBOL = false; //page level variable to determine if selected record can print BOL 
        var blnCanPrintDispatch = false; //page level variable to determine if selected record can print Dispatch 
        var blnBtnDispatchReportClicked = false; //indicates whether the Dispatch Report was generated as result of Dispatching a load or from clicking the Report Menu button. Impacts whether BOL is automatically shown or not
        
       
          
        
        <% Response.Write(PageCustomJS); %>

        // start widgit configuration
        var winBOLReport  = kendo.ui.Window;
        var oBOLReportCtrl = new BOLReportCtrl();
        var winDispatchReport = kendo.ui.Window;
        var oDispatchReportCtrl = new DispatchingReportCtrl();
        
            
        function saveAllPK() {
            try {
                allGridSelectedRow = oAllGrid.select();
                if (typeof (allGridSelectedRow) === 'undefined' || allGridSelectedRow == null) { ngl.showValidationMsg("All Record Required", "Please select a record to continue", tPage); return false; }                               
                allGridSelectedRowDataItem = oAllGrid.dataItem(allGridSelectedRow); //Get the dataItem for the selected row
                if (typeof (allGridSelectedRowDataItem) === 'undefined' || allGridSelectedRowDataItem == null) { ngl.showValidationMsg("All Record Required", "Please select a record to continue", tPage); return false; } 
                if ("Control" in allGridSelectedRowDataItem){     //Control = BookControl           
                    iBookPK = allGridSelectedRowDataItem.Control;
                    var setting = {name:'pk', value: iBookPK.toString()};
                    var oCRUDCtrl = new nglRESTCRUDCtrl();
                    var blnRet = oCRUDCtrl.update("AllItem/PostPageSetting", setting, tPage, "savePostPageSettingSuccessCallback", "savePostPageSettingAjaxErrorCallback",tPage);
                    //check if the TranCode is PB or PC and enable or disable the Print BOL action button (We can print the BOL for both PB or PC)  
                    if ("TranCode" in allGridSelectedRowDataItem){                
                        var tranCode = allGridSelectedRowDataItem.TranCode;
                        //BOL Report enable/disable
                        if(tranCode.toUpperCase() === "PC" || tranCode.toUpperCase() === "PB"){ $('#btnCAR120').prop('disabled', false); blnCanPrintBOL = true; }
                        else { $('#btnCAR120').prop('disabled', true); blnCanPrintBOL = false; }
                        //Disptach Report enable/disable
                        if(tranCode.toUpperCase() === "N" || tranCode.toUpperCase() === "P"){ $('#btnDispatchReport').prop('disabled', true); blnCanPrintDispatch = false; }
                        else { $('#btnDispatchReport').prop('disabled', false); blnCanPrintDispatch = true; }
                    } else { $('#btnCAR120').prop('disabled', true); blnCanPrintBOL = false; $('#btnDispatchReport').prop('disabled', true); blnCanPrintDispatch = false; }
                    return true;
                } else { ngl.showValidationMsg("All Record Required", "Invalid Record Identifier, please select a record to continue", tPage); return false; }
            } catch (err) { ngl.showErrMsg("Cannot Continue", "There was an unexpected error, please reload the screen. If this continues please contact technical support. Error: " + err.message, tPage); }           
        }
    
        function isAllRecordSelected() {
            if (typeof (iBookPK) === 'undefined' || iBookPK === null || iBookPK === 0) { return saveAllPK(); }
            return true;
        }


        //************* Action Mapping **********************
        function execActionClick(btn, proc) {
            
            if(btn.id == "btnMapIt" ){ if (isAllRecordSelected() === true){BingMapsCaller();} } //Bing Maps
            else if (btn.id == "btnPOD") { uploadPOD(); }
            else if (btn.id == "btnBookingImages") { openFiles(); }
            else if(btn.id == "btnRefresh" ){ refresh(); }
            else if(btn.id == "btnResetCurrentUserConfig"){ resetCurrentUserConfig(PageControl); }
        }

        //************* Report Mapping **********************
        function execReportClick(btn, proc){
            if(btn.id == "btnCAR120" ){ if (isAllRecordSelected() === true && blnCanPrintBOL === true){PrintSelectedBOL();} }
            else if(btn.id == "btnDispatchReport" ){ if (isAllRecordSelected() === true && blnCanPrintDispatch === true){openDispatchReport();} }
            else if(btn.id == "btnCAR106" ){ if (isAllRecordSelected() === true){openLoadTenderReport(iBookPK);} }          
        }

        function BingMapsCaller() { 
            var wgt = 0;
            if ("BookFinAPActWgt" in allGridSelectedRowDataItem) { wgt = allGridSelectedRowDataItem.BookFinAPActWgt ; } 
            MapIt(iBookPK,wgt);
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

        function uploadPOD(){
            if (typeof (iBookPK) !== 'undefined' || iBookPK !== null || iBookPK !== 0) {
                window.open("../PODUpload?bookcontrol="+iBookPK, "_blank");
            }
        }

        function openFiles() {    
            
            if (typeof (iBookPK) !== 'undefined' && iBookPK !== null && iBookPK !== 0) {
                var setting = { name: 'ParentControl', value: iBookPK.toString() };
                var oCRUDCtrl = new nglRESTCRUDCtrl();
                var blnRet = oCRUDCtrl.update("BookingImages/PostPageSetting", setting, tPage, "savePostPageSettingSuccessCallback", "savePostPageSettingAjaxErrorCallback", tPage,false);
                location.href = "BookingImages";
            } else {
                var setting = { name: 'ParentControl', value: "0" };
                var oCRUDCtrl = new nglRESTCRUDCtrl();
                var blnRet = oCRUDCtrl.update("BookingImages/PostPageSetting", setting, tPage, "savePostPageSettingSuccessCallback", "savePostPageSettingAjaxErrorCallback", tPage, false);
                ngl.showValidationMsg("No Record Selected", "Please select a record and try again", tPage );
            }

        }

        function openLoadTenderReport(){
            var pro = "";
            if ("ProNumber" in allGridSelectedRowDataItem) { pro = allGridSelectedRowDataItem.ProNumber; } 
            var currentDate = new Date();
            var ExpirationDate = new Date(currentDate.getTime() + (30 * 60 * 1000));
            var oCRUDCtrl = new nglRESTCRUDCtrl();
            var oNewToken = {
                ServiceTokenServiceTypeControl: 3,
                ServiceToken: Date.now(),
                ServiceTokenBookControl: 0,
                ServiceTokenCompControl: 0,
                ServiceTokenCarrierControl: 0,
                ServiceTokenCarrierContControl: 0,
                ServiceTokenLaneControl: 0,
                ServiceTokenAltKeyControl: 0,
                ServiceTokenCode: pro,
                ServiceTokenSendEmail: false,
                ServiceTokenBookTrackStatus: 0,
                ServiceTokenUserName: localStorage.NGLvar1455,
                ServiceTokenExpirationDate: ngl.formatDateTime(ExpirationDate, '', 't'),
            };
            var blnRet = oCRUDCtrl.update("ServiceToken/POST", oNewToken, tObj, "PostLoadTenderTokenSuccessCallback", "PostLoadTenderTokenErrorCallback");
        }


        function PostLoadTenderTokenErrorCallback(xhr, textStatus, error) {
            var oResults = new nglEventParameters();
            var tObj = this;

            oResults.source = "View Load Tender Report";
            oResults.msg = 'Failed'; //set default to Failed  
            oResults.error = new Error();
            oResults.error.name = "Request Failure"
            oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
            ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);

        }


        function PostLoadTenderTokenSuccessCallback(data) {
            var pro = "";
            if ("ProNumber" in allGridSelectedRowDataItem) { pro = allGridSelectedRowDataItem.ProNumber; }
            var sToken = "";
            var bSuccess = false;
            try {
                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {

                    if (data.Errors != null) {
                        oResults.error = new Error();
                        if (data.StatusCode === 203) { oResults.error.name = "Authorization Timeout"; } else { oResults.error.name = "Access Denied"; }
                        oResults.error.message = data.Errors;
                        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                    }
                    else {

                        if (data.Data != null) {
                            dToken = data.Data[0];
                            pro = dToken.ServiceTokenCode;
                            sToken = dToken.ServiceToken;
                            bSuccess = true;
                        }
                        else {
                            oResults.error = new Error();
                            oResults.error.name = "Invalid Request";
                            oResults.error.message = "No Data available.";
                            ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                        }
                    }
                } else { oResults.msg = "Cannot show report"; ngl.showSuccessMsg(oResults.msg, tObj); }
            } catch (err) { oResults.error = err; ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj); }
            if (bSuccess == true) {
                var url = location.protocol + "//" + location.host + "/ViewReport.aspx?hideHeader=true&reportname=CAR106-Load Tender Sheet&reportcontrol=224&refreshReport=true&UN=" + sToken + "&PAR1=Enter_PRO_Number&PAR1Val=" + pro
                openInNewTab(url);
            }

        }



        function PrintSelectedBOL(){
            if (typeof (iBookPK) !== 'undefined' || iBookPK !== null || iBookPK !== 0) {
                if (typeof (oBOLReportCtrl) !== 'undefined' && ngl.isObject(oBOLReportCtrl)) {
                    oBOLReportCtrl.readByBookControl(iBookPK)
                }; 
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
                if (typeof(results.data) !== 'undefined' && ngl.isObject(results.data) && typeof(results.data.ErrorCode) !== 'undefined' && results.data.ErrorCode !== null ){                
                    if(!ngl.isNullOrWhitespace(results.data.ErrorCode) && !isNaN(results.data.ErrorCode)){
                        //get the bid type
                        if (typeof(results.data.DispatchBidType) !== 'undefined' && results.data.DispatchBidType !== null && !ngl.isNullOrWhitespace(results.data.DispatchBidType) && !isNaN(results.data.DispatchBidType)){
                            //possible bid types
                            //1	NextStop
                            //2	NGLTar
                            //3	P44
                            //4	Spot
                            if (results.data.DispatchBidType.toString() == "2" || results.data.DispatchBidType.toString() == "3"){
                                //if we have a loadTenderControl reopen the quote window so user can select a new quote
                                if (typeof(results.data.LoadTenderControl) !== 'undefined' && results.data.LoadTenderControl !== null && !ngl.isNullOrWhitespace(results.data.LoadTenderControl) && !isNaN(results.data.LoadTenderControl)){
                                    if (typeof (tPage["wdgtQuotesWndDialog"]) !== 'undefined' && ngl.isObject(tPage["wdgtQuotesWndDialog"])){
                                        sBidLoadTenderControlVal = results.data.LoadTenderControl;
                                        tPage["wdgtQuotesWndDialog"].read(results.data.LoadTenderControl);
                                    } 
                                }
                                return;
                            } else { execActionClick("Refresh"); return; } //just refresh the data and exit
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



        function refresh() {    
            ngl.readDataSource(oAllGrid);         
        }

        function refreshAllGrid() {
            if (typeof (oAllGrid) !== 'undefined' && ngl.isObject(oAllGrid)) { oAllGrid.dataSource.read(); }
        }

        //*************  Call Back Functions ****************
        var blnAllGridChangeBound = false;
        function AllGridDataBoundCallBack(e,tGrid){           
            oAllGrid = tGrid;
            if (blnAllGridChangeBound == false){
                oAllGrid.bind("change", saveAllPK);
                blnAllGridChangeBound = true;
            }        
            //if iBookPK is not 0 select that row in the grid
            if (typeof (iBookPK) !== 'undefined' && iBookPK !== null && iBookPK !== 0) {
                var rows = oAllGrid.items();
                $(rows).each(function(e) {
                    var row = this;
                    var dataItem = oAllGrid.dataItem(row);
                    if (dataItem.Control == iBookPK) { oAllGrid.select(row); }
                });
            }
        }

        function AllGridToolTipCallBack(e, tGrid) {
           
            var content = "";
            var d = tGrid.dataItem(e.target.closest("tr"));             
            //alert(d.BookItemOrderNumbers);
            //console.log(e.target);
            //alert(e.target.id + " and " + $(e.target).attr('class'));
            var cl = $(e.target).attr('class');
            //alert("Tool Tip Target: " + cl);
            switch (cl)
            {
                case "toolTipBookTracks k-table-td":
                    $.ajax({
                        async: false,
                        type: "GET",
                        url: "api/BookTrack/GetAllCommentsHoverOverData/" + d.Control,
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                        success: function (data) { 
                            try {
                                var blnSuccess = false;
                                var blnErrorShown = false;
                                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                    if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Get Comments Failure", data.Errors, null); }
                                    else {
                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') { blnSuccess = true; content = data.Data[0]; }
                                        }
                                    }
                                }
                                if (blnSuccess === false && blnErrorShown === false) { ngl.showErrMsg("Get Comments Failure", "No results are available.", null); }
                            } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                        },
                        error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Get ToolTip Data Failure"); ngl.showErrMsg("Get ToolTip Data Failure", sMsg, null); }
                    });
                    break;
                case "toolTipApptInfo k-table-td":   
                    content ="<div style='margin: 5px 0px 5px 5px;'><div><div style='margin-bottom:5px;'><span style='display: block; text-align: left;margin-bottom:5px;'><strong>Carrier Name: </strong>" + d.AssignedCarrierName 
                        + "</span><span style='display: block; text-align: left;margin-bottom:5px;'><strong>Invoice: </strong>" + d.CarrierPro 
                        + "</span></div><table class='k-widget tbl'><tr><th class='k-grid-header tbl-top'></th><th class='k-grid-header tbl-topBold'>Sched Appt Date</th>"
                        + "<th class='k-grid-header tbl-topBold'>Sched Appt Time</th><th class='k-grid-header tbl-topBold'>Check-In Date</th><th class='k-grid-header tbl-topBold'>Check-In Time</th></tr>" 
                        + "<tr><td class='k-grid-header tbl-topBoldRt'>Pickup Info:</td><td class='k-widget tbl-top'>" + kendo.toString(d.BookCarrScheduleDate,'MM/dd/yyyy') + "</td><td class='k-widget tbl-top'>" + kendo.toString(d.BookCarrScheduleTime,'h:mm tt') + "</td>" 
                        + "<td class='k-widget tbl-top'>" + kendo.toString(d.BookCarrActualDate,'MM/dd/yyyy') + "</td><td class='k-widget tbl-top'>" + kendo.toString(d.BookCarrActualTime,'h:mm tt') + "</td></tr>" 
                        + "<tr><td class='k-grid-header tbl-topBoldRt'>Delivery Info:</td><td class='k-widget tbl-top'>" + kendo.toString(d.BookCarrApptDate,'MM/dd/yyyy') + "</td><td class='k-widget tbl-top'>" + kendo.toString(d.BookCarrApptTime,'h:mm tt') + "</td>" 
                        + "<td class='k-widget tbl-top'>" + kendo.toString(d.BookCarrActDate,'MM/dd/yyyy') + "</td><td class='k-widget tbl-top'>" + kendo.toString(d.BookCarrActTime,'h:mm tt') + "</td></tr></table></div></div>";
                    break;
                default:
                    content = "";
                    break;
            }
            return content;
        }

        function savePostPageSettingSuccessCallback(results){
            //for now do nothing when we save the pk
        }
        function savePostPageSettingAjaxErrorCallback(xhr, textStatus, error){
            //for now do nothing when we save the pk
        }


        $(document).ready(function () {
            var PageMenuTab = <%=PageMenuTab%>;
            
            if (control != 0){                
                setTimeout(function () {    
                    
                    //default this to disabled until the user selects a record
                    $('#btnCAR120').prop('disabled', true); 
                    blnCanPrintBOL = false;
                    $('#btnDispatchReport').prop('disabled', true); 
                    blnCanPrintDispatch = false;

                    oBOLReportCtrl = new BOLReportCtrl(); 
                    oBOLReportCtrl.loadDefaults(winBOLReport,oBOLReportSelectCB,oBOLReportSaveCB,oBOLReportCloseCB,oBOLReportReadCB);

                    oDispatchReportCtrl = new DispatchingReportCtrl();
                    oDispatchReportCtrl.loadDefaults(winDispatchReport,oDispatchingReportSelectCB,oDispatchingReportSaveCB,oDispatchingReportCloseCB,oDispatchingReportReadCB);

                }, 10,this);
            }

            setTimeout(function () {var PageReadyJS = <%=PageReadyJS%>; }, 10,this);
            setTimeout(function () {        
                menuTreeHighlightPage(); //must be called after PageReadyJS
                var divWait = $("#h1Wait");
                if (typeof (divWait) !== 'undefined' ) { divWait.hide(); }
            }, 10, this);

        });


    </script>
    
          <style>
              .k-grid tbody tr td { overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }

              .k-tooltip { max-height: 500px; max-width: 450px; min-width: 200px; overflow-y: auto; background-color: lightgray; }

              .k-grid tbody .k-grid-Edit { min-width: 0; }
                  
              .k-grid tbody .k-grid-Edit .k-icon { margin: 0; }
          </style>   

      </div>
    </body>
</html>