    <%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tendered.aspx.cs" Inherits="DynamicsTMS365.Tendered" %>

<!DOCTYPE html>


<html>
    <head >
        <title>DTMS Tendered</title>
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
                            <div class="k-block k-info-colored" style="margin:5px;">Click&nbsp;<span class="k-icon k-i-check" ></span>&nbsp;to accept or Click&nbsp;<span class="k-icon k-i-cancel" style="color: blue;"></span>&nbsp;to reject one or more loads below</div>                            
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
  
          <div id="winRejectLoad">
              <div style="margin-left:10%; margin-right:10%">
                  <input id="txtBookControl" type="hidden" />
                  <input id="txtAcceptRejectCode" type="hidden" />
                  <input id="txtBookTrackComment" type="hidden" />
                  <input id="txtwndRLCarrierControl" type="hidden" /> <%--Added By LVV on 4/8/19 - for user associated carrier enhancement--%>
                  <input id="txtwndRLCarrierContControl" type="hidden" /> <%--Added By LVV on 4/8/19 - for user associated carrier enhancement--%>
                  <div>
                      <div style="float: left;">
                          <table class="tblResponsive">
                              <tr>
                                  <td class="tblResponsive-top">Please enter a reason for rejecting this load</td>
                              </tr>
                              <tr>
                                  <td class="tblResponsive-top"><input id="txtReason" style="width:100%;"/></td>
                              </tr>
                          </table>
                      </div>     
                  </div>  
              </div>
          </div>
          
    <% Response.WriteFile("~/Views/BOLReport.html"); %> <%--Added By LVV on 6/15/20 for v-8.2.1.008 Task #20200609164354 - Add the ability to reprint BOL and Load Tender Reports--%>
    <% Response.Write(PageTemplates); %>
      
    <%-- HelpWindow needs to go after AuthNotification because it uses the notification object created there --%>
    <% Response.Write(AuthLoginNotificationHTML); %>   
    <% Response.WriteFile("~/Views/HelpWindow.html"); %>       
    <script>
       <% Response.Write(ADALPropertiesjs); %>

        var PageControl = '<%=PageControl%>'; 
        var tPage = this;
        var tObjPG = this;
        var oPageObject = this;
        var winRejectLoad = kendo.ui.Window;
        var tObj = this;
                   
        

        <% Response.Write(NGLOAuth2); %>

        
        var oTenderedGrid = null;
        var winBOLReport  = kendo.ui.Window; //Added By LVV on 6/15/20 for v-8.2.1.008 Task #20200609164354 - Add the ability to reprint BOL and Load Tender Reports
        var oBOLReportCtrl = new BOLReportCtrl(); //Added By LVV on 6/15/20 for v-8.2.1.008 Task #20200609164354 - Add the ability to reprint BOL and Load Tender Reports
        var iBookPK = 0; //Control = BookControl  
        var tenderedGridSelectedRow;
        var tenderedGridSelectedRowDataItem;

        <% Response.Write(PageCustomJS); %>


        //************* Action Menu Functions ***************
        function execActionClick(btn, proc){
            if (btn.id == "btnRefresh" ){ refresh(); }
            else if(btn.id == "btnResetCurrentUserConfig"){ resetCurrentUserConfig(PageControl); }
        }

        //************* Report Mapping **********************
        function execReportClick(btn, proc){
            if(btn.id == "btnCAR120" ){ if (isTenderedRecordSelected() === true){ PrintSelectedBOL(); } } //Added By LVV on 6/15/20 for v-8.2.1.008 Task #20200609164354 - Add the ability to reprint BOL and Load Tender Reports
            else if(btn.id == "btnCAR106" ){ if (isTenderedRecordSelected() === true){ openLoadTenderReport(iBookPK); } } //Added By LVV on 6/15/20 for v-8.2.1.008 Task #20200609164354 - Add the ability to reprint BOL and Load Tender Reports
        }


        function refresh() { ngl.readDataSource(oTenderedGrid); }

        function saveTenderedPK() {
            try {
                tenderedGridSelectedRow = oTenderedGrid.select();
                if (typeof (tenderedGridSelectedRow) === 'undefined' || tenderedGridSelectedRow == null) { ngl.showValidationMsg("Tendered Record Required", "Please select a record to continue", tPage); return false; }                               
                tenderedGridSelectedRowDataItem = oTenderedGrid.dataItem(tenderedGridSelectedRow); //Get the dataItem for the selected row
                if (typeof (tenderedGridSelectedRowDataItem) === 'undefined' || tenderedGridSelectedRowDataItem == null) { ngl.showValidationMsg("Tendered Record Required", "Please select a record to continue", tPage); return false; } 
                if ("BookControl" in tenderedGridSelectedRowDataItem){          
                    iBookPK = tenderedGridSelectedRowDataItem.BookControl;
                    var setting = {name:'pk', value: iBookPK.toString()};
                    var oCRUDCtrl = new nglRESTCRUDCtrl();
                    var blnRet = oCRUDCtrl.update("Tendered/PostPageSetting", setting, tPage, "savePostPageSettingSuccessCallback", "savePostPageSettingAjaxErrorCallback",tPage);
                    return true;
                } else { ngl.showValidationMsg("Tendered Record Required", "Invalid Record Identifier, please select a record to continue", tPage); return false; }
            } catch (err) { ngl.showErrMsg("Cannot Continue", "There was an unexpected error, please reload the screen. If this continues please contact technical support. Error: " + err.message, tPage); }           
        }
    
        function isTenderedRecordSelected() {
            if (typeof (iBookPK) === 'undefined' || iBookPK === null || iBookPK === 0) { return saveTenderedPK(); }
            return true;
        }


        //************* Call Back Functions ****************
        var blnTenderedGridChangeBound = false;
        function TenderedGridDataBoundCallBack(e,tGrid){           
            oTenderedGrid = tGrid;
            if (blnTenderedGridChangeBound == false){
                oTenderedGrid.bind("change", saveTenderedPK);
                blnTenderedGridChangeBound = true;
            }  
        }

        function loadAcceptedCallBack(){          
            ngl.showSuccessMsg("The selected load has been accepted!");
            if (typeof (oTenderedGrid) !== 'undefined' && ngl.isObject(oTenderedGrid)) { oTenderedGrid.dataSource.read(); } //reload the data in the grid
        }

        function loadRejectCallBack(data) {
            try {
                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                    if (ngl.stringHasValue(data.Errors)) {
                            ngl.showErrMsg("The Load Rejection has Failed", data.Errors, tObj);
                    }
                    else {
                        ngl.showSuccessMsg("The selected load has been rejected!");
                    }
                }
            } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
            if (typeof (oTenderedGrid) !== 'undefined' && ngl.isObject(oTenderedGrid)) { oTenderedGrid.dataSource.read(); } //reload the data in the grid
        }

        function saveAjaxErrorCallback(xhr, textStatus, error){       
            var tObj = this;           
            var serrormessage = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Cannot process your request, please try again.");
            ngl.showErrMsg("Accept or Reject Error", serrormessage, tObj);
        }
       
        //Added By LVV on 6/15/20 for v-8.2.1.008 Task #20200609164354 - Add the ability to reprint BOL and Load Tender Reports
        function oBOLReportSelectCB(results){ return; }
        function oBOLReportSaveCB(results){ return; }
        function oBOLReportCloseCB(results){ return; }
        function oBOLReportReadCB(results){ return; }

        function savePostPageSettingSuccessCallback(results){
            //for now do nothing when we save the pk
        }
        function savePostPageSettingAjaxErrorCallback(xhr, textStatus, error){
            //for now do nothing when we save the pk
        }


        //************* Page Level Functions ****************
        function acceptLoad(e){          
            var item = this.dataItem($(e.currentTarget).closest("tr"));        
            if (typeof (item) !== 'undefined' && ngl.isObject(item)) {           
                var ardata = new AcceptorReject();
                ardata.BookControl = item.BookControl;             
                ardata.AcceptRejectCode = 0;  //accepted
                ardata.BookTrackComment = '';
                ardata.CarrierControl = item.BookCarrierControl; //Added By LVV on 4/8/19 - for user associated carrier enhancement
                ardata.CarrierContControl= item.BookCarrierContControl; //Added By LVV on 4/8/19 - for user associated carrier enhancement
                var restServiceWidget = new nglRESTCRUDCtrl();
                restServiceWidget.update("Tendered/PostSave", ardata, oPageObject, "loadAcceptedCallBack", "saveAjaxErrorCallback");
            }
        }

        function rejectLoad(e){
            var item = this.dataItem($(e.currentTarget).closest("tr"));
            if (typeof (item) !== 'undefined' && ngl.isObject(item)) {                
                var ardata = new AcceptorReject();       
                ardata.BookControl = item.BookControl;
                ardata.AcceptRejectCode = 1;  //rejected
                //ardata.BookTrackComment = 'Rejected';
                ardata.CarrierControl = item.BookCarrierControl; //Added By LVV on 4/8/19 - for user associated carrier enhancement
                ardata.CarrierContControl = item.BookCarrierContControl; //Added By LVV on 4/8/19 - for user associated carrier enhancement               
                //add the arData variables to the popup window
                $("#txtBookControl").val(ardata.BookControl);
                $("#txtAcceptRejectCode").val(ardata.AcceptRejectCode);
                //$("#txtBookTrackComment").val(ardata.BookTrackComment);
                $("#txtwndRLCarrierControl").val(ardata.CarrierControl); //Added By LVV on 4/8/19 - for user associated carrier enhancement
                $("#txtwndRLCarrierContControl").val(ardata.CarrierContControl); //Added By LVV on 4/8/19 - for user associated carrier enhancement               
                winRejectLoad.center().open(); //show popup window
            }
        }

        function SaveRejectReason(){
            //Get the data from the window
            var ardata = new AcceptorReject();
            ardata.BookControl = $("#txtBookControl").val();
            ardata.AcceptRejectCode = $("#txtAcceptRejectCode").val();
            ardata.BookTrackComment = $("#txtReason").val();
            ardata.CarrierControl = $("#txtwndRLCarrierControl").val(); //Added By LVV on 4/8/19 - for user associated carrier enhancement
            ardata.CarrierContControl= $("#txtwndRLCarrierContControl").val(); //Added By LVV on 4/8/19 - for user associated carrier enhancement
            //validate that reason text is not null
            if (typeof (ardata.BookTrackComment) === 'undefined' || ardata.BookTrackComment == null) {
                ngl.showErrMsg("A Reject Reason is Required", "Enter a Reason in order to continue with the Reject", null);   
                return;
            }
            //Next validate that reason text is longer than 4 characters long
            if (ardata.BookTrackComment.length < 4) {
                ngl.showErrMsg("A Reject Reason is Required", "Enter a Reason in order to continue with the Reject", null);
                return;
            }                                  
            //move the code below to the OK buttion in popup window 
            var restServiceWidget = new nglRESTCRUDCtrl();
            restServiceWidget.update("Tendered/PostSave", ardata, oPageObject, "loadRejectCallBack", "saveAjaxErrorCallback");
            winRejectLoad.close();        
        }


        //Added By LVV on 6/15/20 for v-8.2.1.008 Task #20200609164354 - Add the ability to reprint BOL and Load Tender Reports
        function openLoadTenderReport() {
            var pro = "";
            if ("BookProNumber" in tenderedGridSelectedRowDataItem) { pro = tenderedGridSelectedRowDataItem.BookProNumber; } 
            var currentDate = new Date();
            var ExpirationDate = new Date(currentDate.getTime() + (30 * 60 * 1000));
            var oCRUDCtrl = new nglRESTCRUDCtrl();
            var oNewToken = {
                ServiceTokenServiceTypeControl:  3,
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
            if ("BookProNumber" in tenderedGridSelectedRowDataItem) { pro = tenderedGridSelectedRowDataItem.BookProNumber; }
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

          //Added By LVV on 6/15/20 for v-8.2.1.008 Task #20200609164354 - Add the ability to reprint BOL and Load Tender Reports
        function PrintSelectedBOL(){
            if (typeof (iBookPK) !== 'undefined' || iBookPK !== null || iBookPK !== 0) {
                if (typeof (oBOLReportCtrl) !== 'undefined' && ngl.isObject(oBOLReportCtrl)) {
                    oBOLReportCtrl.readByBookControl(iBookPK)
                }; 
            }                         
        }

        


        $(document).ready(function () {
            var PageMenuTab = <%=PageMenuTab%>;
           
            if (control !=0){ 
                setTimeout(function () {      

                    //Added By LVV on 6/15/20 for v-8.2.1.008 Task #20200609164354 - Add the ability to reprint BOL and Load Tender Reports
                    oBOLReportCtrl = new BOLReportCtrl(); 
                    oBOLReportCtrl.loadDefaults(winBOLReport,oBOLReportSelectCB,oBOLReportSaveCB,oBOLReportCloseCB,oBOLReportReadCB);

                
                    //***** BEGIN winRejectLoad CODE *****

                    $("#txtReason").kendoMaskedTextBox();

                    winRejectLoad = $("#winRejectLoad").kendoWindow({
                        title: "Reject Reason",
                        width: 400,
                        height: 400,
                        minWidth: 200,
                        actions: ["save", "Minimize", "Maximize", "Close"],
                        modal: true,
                        visible: false,
                        close: function(e) {
                            //clear all the values
                            $("#txtBookControl").val(0);
                            $("#txtAcceptRejectCode").val(0);
                            $("#txtBookTrackComment").val("");
                            $("#txtwndRLCarrierControl").val(0); //Added By LVV on 4/8/19 - for user associated carrier enhancement
                            $("#txtwndRLCarrierContControl").val(0); //Added By LVV on 4/8/19 - for user associated carrier enhancement
                        }
                    }).data("kendoWindow");              
                    //Modified by RHR for Kendo v-2018 xxx must reference .parent() on save button click.
                    $("#winRejectLoad").data("kendoWindow").wrapper.find(".k-svg-i-save").parent().click(function (e) { SaveRejectReason() });               
                    //***** END winRejectLoad CODE *****
                }, 10,this);          
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

        /*This fixes thead icon alignment in thead grid buttons. Now the right side doesn't get cut off and it looks more centered. Should probably add this code to common.css */
        .k-button-icontext .k-icon, .k-button-icontext .k-image, .k-button-icontext .k-sprite { margin-right:0; } 

    </style>
    </div>
    </body>
</html>
