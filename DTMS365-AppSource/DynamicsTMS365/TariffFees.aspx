<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TariffFees.aspx.cs" Inherits="DynamicsTMS365.TariffFees" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head >
    <title>DTMS Tariff Fees</title>        
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

        
         var oTariffFeesGrid = null;
         function execBeforeTariffFeesGridInsert(){
             // prepare for new record like: add code to update the default values for specific fields
             // see TariffRates.aspx for examples
             return true;
           
         }
         <% Response.Write(PageCustomJS); %>
         function execActionClick(btn, proc){
             if(btn.id == "btnOpenContract" ){ location.href = "Tariff";
             }else if (btn.id == "btnOpenServices" ){ location.href = "TariffServices";
             }else if (btn.id == "btnOpenRates" ){ location.href = "TariffRates";
             }else if (btn.id == "btnOpenExceptions" ){ location.href = "TariffExceptions";
             //}else if (btn.id == "btnAddFees" ){ openAddNewTariffFeesGridWindow();
             }else if (btn.id == "btnOpenFuel" ){ location.href = "TariffFuel";
             }else if (btn.id == "btnOpenNoDrive" ){ location.href = "TariffNoDriveDays";
             }else if (btn.id == "btnOpenHDMs" ){ location.href = "TariffHDMs";
             }else if (btn.id == "btnRefresh" ){ 
                 wdgtvCarrierTariffSummarySummary.read(0);
                 //Modified by RHR for v-8.2 on 02/20/2019
                 ngl.readDataSource(oTariffFeesGrid);
             }  
             if(btn.id == "btnResetCurrentUserConfig"){
                 resetCurrentUserConfig(PageControl);
             }
         }

         //*************  Call Back Functions ****************
         function TariffFeesGridDataBoundCallBack(e,tGrid){           
             oTariffFeesGrid = tGrid;
         }
         // ************** Start CarrTarFeesAccessorialCode Functions ******************
         //Widget object is wdgtBookPkgGridEdit
         var iCarrTarFeesAccessorialCodeID = '0';
         var ddlCarrTarFeesAccessorialCode = undefined;
         // widget call back         
         function TariffFeesGridCB(oResults){   
             if (!oResults) { return;}
             if (oResults.source == "showWidgetCallback"   ){
                 //if (oResults.source == "showWidgetCallback"  && iCarrTarFeesAccessorialCodeID == '0'  ){
                 iCarrTarFeesAccessorialCodeID = wdgtTariffFeesGridEdit.GetFieldID("CarrTarFeesAccessorialCode");
                 ddlCarrTarFeesAccessorialCode = $("#" + iCarrTarFeesAccessorialCodeID).data("kendoDropDownList");
                 if (ddlCarrTarFeesAccessorialCode){
                     ddlCarrTarFeesAccessorialCode.bind("change", CarrTarFeesAccessorialCode_change);
                 }
             }             
         }
         //change event handler
         function CarrTarFeesAccessorialCode_change(e) {
             if (ddlCarrTarFeesAccessorialCode){
                 var iCodeValue = ddlCarrTarFeesAccessorialCode.value();
                 if (iCodeValue){
                     updateSelectedAccessorialCode(iCodeValue);
                 }
             }
         }
         //read data to update children when list selection changes
         function updateSelectedAccessorialCode(key){           
             var oCRUDCtrl = new nglRESTCRUDCtrl();
             var blnRet = oCRUDCtrl.read("AccessorialMaint", key, tPage, "readAccessorialCodeSuccessCallback", "readAccessorialCodeAjaxErrorCallback",tPage);
             return true;
         }
         //read selected data call back from list
         // used to update children when list selection changes        
         function readAccessorialCodeSuccessCallback(data){
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
                             if ("AccessorialVariableCode" in record) {
                                 sVal = record["AccessorialVariableCode"];
                                 sFieldID = wdgtTariffFeesGridEdit.GetFieldID("CarrTarFeesVariableCode");
                                 var ddlCarrTarFeesVariableCode = $("#" + sFieldID).data("kendoDropDownList");
                                 if (ddlCarrTarFeesVariableCode && isNaN(sVal) == false){
                                     ddlCarrTarFeesVariableCode.value(sVal);
                                 }
                             }
                             //AccessorialMinimum		Minimum			CarrTarFeesMinimum
                             if ("AccessorialMinimum" in record) {
                                 sVal = record["AccessorialMinimum"];
                                 sFieldID = wdgtTariffFeesGridEdit.GetFieldID("CarrTarFeesMinimum");
                                 var tCarrTarFeesMinimum = $("#" + sFieldID).data("kendoNumericTextBox");
                                 if (tCarrTarFeesMinimum && isNaN(sVal) == false ){
                                     tCarrTarFeesMinimum.value(sVal);
                                 }
                             }
                             //AccessorialVariable		FormulaVariable		CarrTarFeesVariable
                             if ("AccessorialVariable" in record) {
                                 sVal = record["AccessorialVariable"];
                                 sFieldID = wdgtTariffFeesGridEdit.GetFieldID("CarrTarFeesVariable");
                                 var tCarrTarFeesVariable = $("#" + sFieldID).data("kendoNumericTextBox");
                                 if (tCarrTarFeesVariable && isNaN(sVal) == false ){
                                     tCarrTarFeesVariable.value(sVal);
                                 }
                             }
                             //AccessorialEDICode		EDI Code		CarrTarFeesEDICode
                             if ("AccessorialEDICode" in record) {
                                 sVal = record["AccessorialEDICode"];
                                 sFieldID = wdgtTariffFeesGridEdit.GetFieldID("CarrTarFeesEDICode");
                                 var tCarrTarFeesEDICode = $("#" + sFieldID).data("kendoMaskedTextBox");
                                 if (tCarrTarFeesEDICode ){
                                     tCarrTarFeesEDICode.value(sVal);
                                 }
                             }
                             //CarrTarFeesAccessorialProfileSpecific
                             sFieldID = wdgtTariffFeesGridEdit.GetFieldID("CarrTarFeesAccessorialProfileSpecific");
                             var tCarrTarFeesAccessorialProfileSpecific = $("#" + sFieldID).data("kendoMaskedTextBox");
                             if (tCarrTarFeesAccessorialProfileSpecific ){
                                 if ("AccessorialProfileSpecific" in record) {                                                                    
                                     tCarrTarFeesAccessorialProfileSpecific.value(record["AccessorialProfileSpecific"]);                                    
                                 }
                                 else {
                                         tCarrTarFeesAccessorialProfileSpecific.value(false);
                                 }
                             }                             
                         }
                     }
                 } 
             } catch (err) {
                 ngl.showErrMsg(err.name, err.message, tObj);
             }          
         }
         //handle any ajax errors
         function readBookPkgPkgDescAjaxErrorCallback(xhr, textStatus, error){
             ngl.showErrMsg("Read Selected Accessorial Details Failure",
                 formatAjaxJSONResponsMsgs(xhr, textStatus, error, 'Failed'), 
                 tObj);
         }

         // ************** End CarrTarFeesAccessorialCode Functions ******************
       
         
         $(document).ready(function () {
             var PageMenuTab = <%=PageMenuTab%>;
                     
            
            if (control != 0){
                //setTimeout(function () {
                //    //add code here to load screen specific information this is only visible when a user is authenticated
                //}, 1,this);

            }
            var PageReadyJS = <%=PageReadyJS%>
            menuTreeHighlightPage(); //must be called after PageReadyJS
            var divWait = $("#h1Wait");
            if  (typeof (divWait) !== 'undefined' ) {
                divWait.hide();
            }
           
           

        });


    </script>
    <style>

        .k-grid tbody tr td {
            overflow: hidden;
            text-overflow: ellipsis;
            white-space: nowrap;
        }

         .k-tooltip{
            max-height: 500px;
            max-width: 450px;
            overflow-y: auto;
        }
       
        .k-grid tbody .k-grid-Edit {
        min-width: 0;
      }

      .k-grid tbody .k-grid-Edit .k-icon {
        margin: 0;
      }
    </style>
    
    </div>
</body>
</html>
