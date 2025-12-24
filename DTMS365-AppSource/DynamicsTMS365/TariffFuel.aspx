<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TariffFuel.aspx.cs" Inherits="DynamicsTMS365.TariffFuel" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head >
    <title>DTMS Tariff Fuel</title>        
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
                                                    
                            <div id="lblMatrixInUse" style="text-align:center;"><p>Using Tariff Specific Fuel Matrix</p></div>

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
    <% Response.WriteFile("~/Views/TariffFuelMatrixWindow.html"); %>   
 
     <script>
        <% Response.Write(ADALPropertiesjs); %>

        var PageControl = '<%=PageControl%>'; 
        
          var tObj = this;
          var tPage = this;
                    
       

        <% Response.Write(NGLOAuth2); %>

        
         var oTariffFuelAdRateGrid = null;
         
         var oTariffFuelAdExGrid = null;
         function execBeforeTariffFuelGridInsert(){
             // prepare for new record like: add code to update the default values for specific fields
             // see TariffRates.aspx for examples
           
         }
         <% Response.Write(PageCustomJS); %>
         function execActionClick(btn, proc){
             if(btn.id == "btnOpenContract" ){ location.href = "Tariff"; }
             else if (btn.id == "btnOpenServices" ){ location.href = "TariffServices"; }
             else if (btn.id == "btnOpenRates" ){ location.href = "TariffRates"; }
             else if (btn.id == "btnOpenExceptions" ){ location.href = "TariffExceptions"; }
             else if (btn.id == "btnOpenFees" ){ location.href = "TariffFees"; }
             else if (btn.id == "btnOpenFuel" ){ location.href = "TariffFuel"; }
             else if (btn.id == "btnOpenNoDrive" ){ location.href = "TariffNoDriveDays"; }
             else if (btn.id == "btnOpenHDMs" ){ location.href = "TariffHDMs"; }
             else if (btn.id == "btnFillFromTemplate"){
                 var sFillFromTemplateCTRLName = "wdgtNewFuelAdTemplWndDialog";
                 if (typeof (tPage["wdgtNewFuelAdTemplWorkFlowOptionCtrlEdit"]) !== 'undefined' && ngl.isObject(tPage["wdgtNewFuelAdTemplWorkFlowOptionCtrlEdit"])){
                     tPage["wdgtNewFuelAdTemplWorkFlowOptionCtrlEdit"].ReadUserSettings = true;
                 }
                 if (typeof (tPage["wdgtNewFuelAdTemplWndDialog"]) !== 'undefined' && ngl.isObject(tPage["wdgtNewFuelAdTemplWndDialog"])){
                     tPage["wdgtNewFuelAdTemplWndDialog"].clearWdgtHTML();
                     tPage["wdgtNewFuelAdTemplWndDialog"].read(0);
                 } else{alert("Missing HTML Element (wdgtNewFuelAdTemplWndDialog is undefined)");} //Add better error handling here if cm stuff is missing                     
             }
             else if (btn.id == "btnUpdateAllActiveLoads"){
                 ngl.OkCancelConfirmation(
                       "Update Active Loads Fuel Costs",
                       "This is a batch process that will run in the background, it may take some time to complete. You can continue working normally. It cannot be undone. Are you sure you want to continue?",
                       400,
                       400,
                       tPage,
                       "UpdateAllActiveLoads");                      
             }                 
             else if(btn === "Saved"){ 
                 if (typeof (tPage[proc]) !== 'undefined' && ngl.isObject(tPage[proc])) {
                     if (tPage[proc].sNGLCtrlName ==  "wdgtNewFuelAdTemplWndDialog"){
                         var iCarrTarControlElmt = wdgtvCarrierTariffSummarySummary.GetFieldID("CarrTarControl");
                         var elmt = $("#" + iCarrTarControlElmt); 
                         var sSummaryCarrTarControlID = "0";
                         if (typeof (elmt) !== 'undefined' && elmt != null){
                             sSummaryCarrTarControlID = elmt.val().toString();
                         }
                         var oCRUDCtrl = new nglRESTCRUDCtrl();
                         oCRUDCtrl.filteredPost("TariffFuelAddendum/CreateNewFuelAddendumFromTemplate", sSummaryCarrTarControlID, tObj, "saveNewFuelAddendumFromTemplateCallback", "saveNewFuelAddendumFromTemplateAjaxErrorCallback", true) ;
                         tPage[proc].executeActions("close");                         
                     }
                 }             
             }
             else if (btn.id == "btnDeleteCarSpecificFuelMatrix"){
                 ngl.OkCancelConfirmation(
                       "Delete Carrier Specific Fuel Matrix",
                       "This action will Delete the Tariff Specific Fuel Matrix and start using the Carrier Default Fuel Matrix. Are you sure you want to continue?",
                       400,
                       400,
                       tPage,
                       "DeleteCarSpecificFuelMatrix");

             }
             else if (btn.id == "btnImportCarSpecificFuelMatrix"){ openImportFuelWnd(PageControl);}
             else if (btn.id == "btnRefresh" ){ refresh(); }   
             else if(btn.id == "btnResetCurrentUserConfig"){ resetCurrentUserConfig(PageControl); }
         }

         function refresh(){
             wdgtvCarrierTariffSummarySummary.read(0); 
             wdgtTariffFuelAddendumEdit.read(0);
             oTariffFuelAdRateGrid.dataSource.read();
             oTariffFuelAdExGrid.dataSource.read();
         }

         //*************  Call Back Functions ****************
         function TariffFuelAdRateGridDataBoundCallBack(e,tGrid){           
             oTariffFuelAdRateGrid = tGrid;           
         }

         function TariffFuelAdExGridDataBoundCallBack(e,tGrid){           
             oTariffFuelAdExGrid = tGrid;
         }

         function saveNewFuelAddendumFromTemplateCallback(data){
             var oResults = new nglEventParameters();
             oResults.source = "saveNewFuelAddendumFromTemplateCallback";
             oResults.msg = 'Failed'; //set default to Failed               
             oResults.widget = tObj;          
             try {
                 var blnSuccess = false;
                 var blnErrorShown = false;
                 var strValidationMsg = "";
                 if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                     if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                         oResults.error = new Error();
                         oResults.error.name = "Create tariff fuel addendum from template Failure";
                         oResults.error.message = data.Errors;
                         blnErrorShown = true;
                         ngl.showErrMsg(oResults.error.name, oResults.error.message, null);
                     }
                     else {
                         if (typeof (data.Data) !== 'undefined' && data.Data != null && ngl.isArray(data.Data) && data.Data[0] == true) {
                             blnSuccess = true;
                             oResults.datatype = "bool";
                             oResults.data = data.Data[0];
                             oResults.msg = "Success"
                             ngl.showSuccessMsg("Success your fuel addendum has been updated", null);
                         } else {
                             blnErrorShown = true;
                             oResults.error = new Error();
                             oResults.error.name = "Unable to create your tariff fuel addendum";
                             oResults.error.message = "The procedure returned false, please refresh your data and try again.";
                             ngl.showErrMsg(oResults.error.name, oResults.error.message, null);
                         }
                     }
                 }
                 if (blnSuccess === false && blnErrorShown === false) {
                     oResults.error = new Error();
                     oResults.error.name = "Create tariff fuel addendum from template Failure";
                     oResults.error.message = "No results are available.";
                     ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                 }
             } catch (err) { oResults.error = err; ngl.showErrMsg(err.name, err.message, null); }
             //execActionClick('btnRefresh', null);
             refresh();
         }
         function saveNewFuelAddendumFromTemplateAjaxErrorCallback(xhr, textStatus, error){         
             var oResults = new nglEventParameters();         
             oResults.source = "saveNewFuelAddendumFromTemplateAjaxErrorCallback";
             oResults.msg = 'Failed'; //set default to Failed 
             oResults.widget = tObj;            
             oResults.error = new Error();
             oResults.error.name = "Create tariff fuel addendum from template Failure";
             oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
             ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
             //execActionClick('btnRefresh', null);
             refresh();
         }
       
         function UpdateAllActiveLoads(iRet) {
             if (typeof (iRet) === 'undefined' || iRet === null  || iRet === 0 ) { return;}
           
             if (typeof (wdgtvCarrierTariffSummarySummary) !== 'undefined' && ngl.isObject(wdgtvCarrierTariffSummarySummary)) {
                 var iCarrTarControlElmt = wdgtvCarrierTariffSummarySummary.GetFieldID("CarrTarControl");
                 var elmt = $("#" + iCarrTarControlElmt); 
                 var sSummaryCarrTarControlID = "0";
                 if (typeof (elmt) !== 'undefined' && elmt != null){
                     sSummaryCarrTarControlID = elmt.val().toString();
                 }
                 if (sSummaryCarrTarControlID == 0) { ngl.showErrMsg("Update All Carrier Fuel Fees From Tariff Failure", "Could not read the selected Tariff information.  Please return to the contract page and select a valid tariff record", null); return; }
                 var oCRUDCtrl = new nglRESTCRUDCtrl();
                 oCRUDCtrl.filteredPost("TariffFuelAddendum/UpdateCarrierFuelFeesByContract", sSummaryCarrTarControlID, tObj, "saveCarrierFuelFeesCallback", "saveCarrierFuelFeesAjaxErrorCallback", true) ;
             }
         }
         
         function saveCarrierFuelFeesCallback(data){
             var oResults = new nglEventParameters();
             oResults.source = "saveCarrierFuelFeesCallback";
             oResults.msg = 'Failed'; //set default to Failed               
             oResults.widget = tObj;         
             try {
                 var blnSuccess = false;
                 var blnErrorShown = false;
                 var strValidationMsg = "";
                 if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                     if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                         oResults.error = new Error();
                         oResults.error.name = "Update All Carrier Fuel Fees From Tariff Failure";
                         oResults.error.message = data.Errors;
                         blnErrorShown = true;
                         ngl.showErrMsg(oResults.error.name, oResults.error.message, null);
                     }
                     else {
                         if (typeof (data.Data) !== 'undefined' && data.Data != null && ngl.isArray(data.Data) && data.Data[0] == true) {
                             blnSuccess = true;
                             oResults.datatype = "bool";
                             oResults.data = data.Data[0];
                             oResults.msg = "Success"
                             ngl.showSuccessMsg("Success your Fuel Fees are being updated", null);
                         } else {
                             blnErrorShown = true;
                             oResults.error = new Error();
                             oResults.error.name = "Unable to Update All Carrier Fuel Fees From Tariff";
                             oResults.error.message = "The procedure returned false, please refresh your data and try again.";
                             ngl.showErrMsg(oResults.error.name, oResults.error.message, null);
                         }
                     }
                 }
                 if (blnSuccess === false && blnErrorShown === false) {
                     oResults.error = new Error();
                     oResults.error.name = "Update All Carrier Fuel Fees From Tariff Failure";
                     oResults.error.message = "No results are available.";
                     ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                 }
             } catch (err) { oResults.error = err; ngl.showErrMsg(err.name, err.message, null); }            
         }
         function saveCarrierFuelFeesAjaxErrorCallback(xhr, textStatus, error){         
             var oResults = new nglEventParameters();          
             oResults.source = "saveCarrierFuelFeesAjaxErrorCallback";
             oResults.msg = 'Failed'; //set default to Failed 
             oResults.widget = tObj;            
             oResults.error = new Error();
             oResults.error.name = "Update All Carrier Fuel Fees From Tariff Failure";
             oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
             ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
         }


         function DeleteCarSpecificFuelMatrix(iRet) {
             if (typeof (iRet) === 'undefined' || iRet === null  || iRet === 0 ) { return;}         
             if (typeof (wdgtTariffFuelAddendumEdit) !== 'undefined' && ngl.isObject(wdgtTariffFuelAddendumEdit)) {
                 if (typeof (wdgtTariffFuelAddendumEdit.data) !== 'undefined' && wdgtTariffFuelAddendumEdit.data != null && ngl.isObject(wdgtTariffFuelAddendumEdit.data)) {
                     var intCarrFuelAdControl = 0;
                     if('CarrFuelAdControl' in wdgtTariffFuelAddendumEdit.data){ intCarrFuelAdControl = wdgtTariffFuelAddendumEdit.data.CarrFuelAdControl; }                    
                     var tObj = tPage;
                     var oCRUDCtrl = new nglRESTCRUDCtrl();
                     var blnRet = oCRUDCtrl.delete("TariffFuelAddendum/DELETE", intCarrFuelAdControl, tObj, "deleteCarSpecificFuelMatrixCallback", "deleteCarSpecificFuelMatrixAjaxErrorCallback")
                 }
             }
         }
         
         function deleteCarSpecificFuelMatrixCallback(data){
             var oResults = new nglEventParameters();
             oResults.source = "deleteCarSpecificFuelMatrixCallback";
             oResults.msg = 'Failed'; //set default to Failed               
             oResults.widget = tObj;     
             try {
                 var blnSuccess = false;
                 var blnErrorShown = false;
                 var strValidationMsg = "";
                 if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                     if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                         oResults.error = new Error();
                         oResults.error.name = "Delete Carrier Specific Fuel Matrix Failure";
                         oResults.error.message = data.Errors;
                         blnErrorShown = true;
                         ngl.showErrMsg(oResults.error.name, oResults.error.message, null);
                     }
                     else {
                         if (typeof (data.Data) !== 'undefined' && data.Data != null && ngl.isArray(data.Data) && data.Data[0] == true) {
                             blnSuccess = true;
                             oResults.datatype = "bool";
                             oResults.data = data.Data[0];
                             oResults.msg = "Success"
                             ngl.showSuccessMsg("Delete Carrier Specific Fuel Matrix Success", null);
                         } else {
                             blnErrorShown = true;
                             oResults.error = new Error();
                             oResults.error.name = "Unable to Delete Carrier Specific Fuel Matrix";
                             oResults.error.message = "The procedure returned false, please refresh your data and try again.";
                             ngl.showErrMsg(oResults.error.name, oResults.error.message, null);
                         }
                     }
                 }
                 if (blnSuccess === false && blnErrorShown === false) {
                     oResults.error = new Error();
                     oResults.error.name = "Delete Carrier Specific Fuel Matrix Failure";
                     oResults.error.message = "No results are available.";
                     ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                 }
             } catch (err) { oResults.error = err; ngl.showErrMsg(err.name, err.message, null); } 
             refresh();
         }
         function deleteCarSpecificFuelMatrixAjaxErrorCallback(xhr, textStatus, error){    
             var oResults = new nglEventParameters();      
             oResults.source = "deleteCarSpecificFuelMatrixAjaxErrorCallback";
             oResults.msg = 'Failed'; //set default to Failed 
             oResults.widget = tObj;            
             oResults.error = new Error();
             oResults.error.name = "Delete Carrier Specific Fuel Matrix Failure";
             oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
             ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
             refresh();
         }

         function TariffFuelAddendumCB(oResults){
             if (oResults.source == "readSuccessCallback")
             {
                 if(typeof (oResults) !== 'undefined' && oResults != null){
                     var blnUsingCarDefault = false; //Using Tariff Specific Fuel Matrix
                     
                     //If results are null or undefined Then Using Carrier Default Fuel Matrix
                     //Else If results are not null but CarrFuelAdCarrTarControl = 0 Then Using Carrier Default Fuel Matrix
                     //Else Using Tariff Specific Fuel Matrix 
                     if(typeof (oResults.data) !== 'undefined' && oResults.data != null && ngl.isArray(oResults.data) && oResults.data.length > 0 ){ 
                         if (!("CarrFuelAdCarrTarControl" in oResults.data[0])){ ngl.showValidationMsg("Tariff Fuel Addendum Data Error", "Data Record is missing the field CarrFuelAdCarrTarControl", tPage); return; }
                         if(oResults.data[0].CarrFuelAdCarrTarControl === 0){ blnUsingCarDefault = true; } //If results are not null but CarrFuelAdCarrTarControl = 0 Then Using Carrier Default Fuel Matrix                   
                     } else{ blnUsingCarDefault = true; } //If results are null or undefined Then Using Carrier Default Fuel Matrix
                 
                     //Get the ID's of the Divs that contain the Grids and Fast Tabs and the Tariff Fuel Addendum
                     //oTariffFuelAdRateGrid
                     var topWrapperIDAd;
                     if(typeof (oTariffFuelAdRateGrid) !== 'undefined' && oTariffFuelAdRateGrid != null){
                         var psAd = oTariffFuelAdRateGrid.wrapper.parents();                     
                         for (var j = 0; j < psAd.length; j++) {
                             if(psAd[j].id === "center-pane" && j > 1){ topWrapperIDAd = psAd[j-1].id; }                        
                         }
                     }
                     //oTariffFuelAdExGrid
                     var topWrapperIDEx;
                     if(typeof (oTariffFuelAdExGrid) !== 'undefined' && oTariffFuelAdExGrid != null){
                         var psEx = oTariffFuelAdExGrid.wrapper.parents();                 
                         for (var j = 0; j < psEx.length; j++) {
                             if(psEx[j].id === "center-pane" && j > 1){ topWrapperIDEx = psEx[j-1].id; }                        
                         }
                     }
                     //wdgtTariffFuelAddendumEdit
                     var topWrapperIDTFA;
                     if (wdgtTariffFuelAddendumEdit) { topWrapperIDTFA = wdgtTariffFuelAddendumEdit.ContainerDivID; }

                     //When using the Default Carrier Fuel Matrix do not display the detail grids and/or fast tabs or the Tariff Fuel Addendum
                     //Add message (in red above Carrier Tariff Summary) informing the users which matrix is being used
                     if(blnUsingCarDefault){                
                         if(document.getElementById(topWrapperIDAd)){ $("#" + topWrapperIDAd).hide(); }
                         if(document.getElementById(topWrapperIDEx)){ $("#" + topWrapperIDEx).hide(); }          
                         if(document.getElementById(topWrapperIDTFA)){ $("#" + topWrapperIDTFA).hide(); }
                         $("#lblMatrixInUse").html("<p>Using Carrier Default Fuel Matrix</p>");
                     } 
                     else {
                         if(document.getElementById(topWrapperIDAd)){ $("#" + topWrapperIDAd).show(); }
                         if(document.getElementById(topWrapperIDEx)){ $("#" + topWrapperIDEx).show(); }  
                         if(document.getElementById(topWrapperIDTFA)){ $("#" + topWrapperIDTFA).show(); } 
                         $("#lblMatrixInUse").html("<p>Using Tariff Specific Fuel Matrix</p>");
                     }
                     //var gridWrapper = oTariffFuelAdRateGrid.wrapper;
                     //var gridParent = gridWrapper.parent();
                     //var gridParentParent = gridParent.parent();
                     //var gridParentParentParent = gridParentParent.parent();
                     ////alert(winWrapper.parent().attr('id'));
                 }  
             }
         }
       
         
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
             if (typeof (divWait) !== 'undefined') { divWait.hide(); }
         });
    </script>
    <style>
        .k-grid tbody tr td { overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }
         
        .k-tooltip{ max-height: 500px; max-width: 450px; overflow-y: auto; }
       
        .k-grid tbody .k-grid-Edit { min-width: 0; }
    
        .k-grid tbody .k-grid-Edit .k-icon { margin: 0; }

                  .RateITOptions ul {
                margin: 0;
                padding: 0;
                max-width: 255px;
            }

                .RateITOptions ul li {
                    margin: 0;
                    padding: 10px 0px 0px 20px;
                    min-height: 25px;
                    line-height: 25px;
                    vertical-align: middle;
                    /*border: 1px solid rgba(128,128,128,.5);*/
                    border-top: 1px solid rgba(128,128,128,.5);
                }

            .RateITOptions {
                min-width: 220px;
                padding: 0;
                position: relative;
            }

                .RateITOptions ul li .km-switch {
                    float: right;
                }

            .RateITOptions-head {
                height: 50px;
                background-color: skyblue;
            }
    </style>
    
    </div>
</body>
</html>
