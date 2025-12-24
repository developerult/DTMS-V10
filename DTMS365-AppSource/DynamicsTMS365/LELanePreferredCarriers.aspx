<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LELanePreferredCarriers.aspx.cs" Inherits="DynamicsTMS365.LELanePreferredCarriers" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head >
    <title>DTMS Lane Preferred Carriers</title>        
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
         <div id="divContactsWindow"></div>
     <% Response.Write(PageTemplates); %>
        
    <%-- HelpWindow needs to go after AuthNotification because it uses the notification object created there --%>
    <% Response.Write(AuthLoginNotificationHTML); %>   
    <% Response.WriteFile("~/Views/HelpWindow.html"); %>  
  <% Response.WriteFile("~/Views/AvailableCarrierWnd.html");%> 
     <% Response.WriteFile("~/Views/SelectContactsWd.html");%> 
     <script>
        <% Response.Write(ADALPropertiesjs); %>

        var PageControl = '<%=PageControl%>'; 
          var tObj = this;
         var tPage = this;         
         var oLELanePrefCarGrid = null;
         var iLanePrefCarPK = 0;         
         var tObjPG = this;
         var pgLEControl = 0;
         var pgLEName = "";
          oSelectContactCtrl = null;   

        <% Response.Write(NGLOAuth2); %>

                

         function execBeforeLanePrefCarsGridInsert(){
             // prepare for new record like: add code to update the default values for specific fields
             // see TariffRates.aspx for examples
             return true;
           
         }
         <% Response.Write(PageCustomJS); %>

         var winCarrierContactDialog = kendo.ui.Window;
         var  sBidLoadTenderControlVal = "0"
         function savePostPageSettingSuccessCallback(results){
            
         }
         var wndChangeLEDialog = kendo.ui.Window;
         var oChangeLEDialogCtrl = new ChangeLEDialogCtrl()
         function savePostPageSettingAjaxErrorCallback(xhr, textStatus, error){
             //for now do nothing when we save the pk 
         }
    
         function saveLELanePrefCarPK() {
             try {
                 loadLELanePrefCarGridSelectedRow = oLELanePrefCarGrid.select();
                 if (typeof (loadLELanePrefCarGridSelectedRow) === 'undefined' || loadLELanePrefCarGridSelectedRow == null) { ngl.showValidationMsg("LanePrefferedCarrier Record Required", "Please select a LanePrefferedCarrier to continue", tPage); return false; }                             
                 loadLELanePrefCarGridSelectedRowDataItem = oLELanePrefCarGrid.dataItem(loadLELanePrefCarGridSelectedRow); //Get the dataItem for the selected row
                 if (typeof (loadLELanePrefCarGridSelectedRowDataItem) === 'undefined' || loadLELanePrefCarGridSelectedRowDataItem == null) { ngl.showValidationMsg("LanePrefferedCarrier Record Required", "Please select a LanePrefferedCarrier to continue", tPage); return false; } 
                 if ("LLTCControl" in loadLELanePrefCarGridSelectedRowDataItem){                
                     iLanePrefCarPK = loadLELanePrefCarGridSelectedRowDataItem.LLTCControl;
                     var setting = {name:'pk', value: iLanePrefCarPK.toString()};
                     var oCRUDCtrl = new nglRESTCRUDCtrl();
                     var blnRet = oCRUDCtrl.update("LELanePreferredCarriers/PostPageSetting", setting, tPage, "savePostPageSettingSuccessCallback", "savePostPageSettingAjaxErrorCallback",tPage);
                     //check if the TranCode is PB or PC and enable or disable the Print BOL action button (We can print the BOL for both PB or PC)  
                     return true;
                 }   else { ngl.showValidationMsg("LanePrefferedCarrier Record Required", "Invalid Record Identifier, please select a LanePrefferedCarrier to continue", tPage); return false; }
             } catch (err) { ngl.showErrMsg("Cannot Continue", "There was an unexpected error, please reload the screen. If this continues please contact technical support. Error: " + err.message, tPage); }           
         }

         function isLELanePrefCarSelected() {
             if (typeof (iLanePrefCarPK) === 'undefined' || iLanePrefCarPK === null || iLanePrefCarPK === 0) { return saveLELanePrefCarPK(); }
             return true;
         }
         
         var blnLELanePrefCarGridChangeBound = false;
         //*************  Call Back Functions ****************
         function LanePreferredCarriersGridDataBoundCallBack(e,tGrid){            
             oLELanePrefCarGrid = tGrid;  
             if (blnLELanePrefCarGridChangeBound == false){
                 oLELanePrefCarGrid.bind("change", saveLELanePrefCarPK);
                 blnLELanePrefCarGridChangeBound = true;
             }        
             //if iLanePrefCarPK is not 0 select that row in the grid
             if (typeof (iLanePrefCarPK) !== 'undefined' && iLanePrefCarPK !== null && iLanePrefCarPK !== 0) {
                 var rows = oLELanePrefCarGrid.items();
                 $(rows).each(function(e) {
                     var row = this;
                     var dataItem = oLELanePrefCarGrid.dataItem(row);
                     if (dataItem.LaneControl == iLanePrefCarPK) { oLELanePrefCarGrid.select(row); }
                 });
             }
         }
           
       
         
         var wnd, detailsTemplate;
         //function openContactDetails(e) {
         //    debugger;
         //    var item = this.dataItem($(e.currentTarget).closest("tr")); 
         //    console.log(item.LLTCControl);
         //    console.log(item.LLTCCarrierControl);
         //    console.log(item);
         //    openSelectContactsWnd(item.LLTCCarrierControl,item.LLTCControl);
         //}
         function openLinkToTariff (e) {
           
             // read the select row
             var item = this.dataItem($(e.currentTarget).closest("tr"));
             if (item.LLTCTariffControl == 0) {
                 ngl.showValidationMsg("View Tariff","The tariff is not valid");
                 return false;
             }
             else { 
                 window.open("../Tariff?tarcontrol="+item.LLTCTariffControl, "_blank");}
            
         }


         function refresh(){
             ngl.readDataSource(oLELanePrefCarGrid);
         }
         // ************** End Call Back functions Functions ******************

         var blnShowCarrierAfterPrefCarOptions = false;
         //************* Action Menu Functions ********************
         function execActionClick(btn, proc){            
             if(btn.id == "btnLELaneMaint"){ location.href = "LELaneMaint"; }  
             else if (btn.id == "btnActiveCarriers"){ openActiveCarriersWnd(pgLEControl); } 
             else if(btn.id == "btnLaneProfileFees"){ location.href = "LELaneProfileFees"; } 
             else if(btn.id == "btnLaneFee"){ location.href = "LELaneFees"; }  
             else if (btn.id == "btnSelectContacts"){
                
                 if (isLELanePrefCarSelected() === true) {                    
                     //if ("LLTCControl" in loadLELanePrefCarGridSelectedRowDataItem ){
                     //    PK = loadLELanePrefCarGridSelectedRowDataItem.LLTCControl;
                     //    var setting = {name:'pk', value: PK.toString()};
                     //}
                     var iCarrierControl = 0;
                     if ("LLTCCarrierControl" in loadLELanePrefCarGridSelectedRowDataItem ){
                         iCarrierControl = loadLELanePrefCarGridSelectedRowDataItem.LLTCCarrierControl;
                         //var setting = {name:'CK', value: CK.toString()};
                     }
                     console.log("iCArrierControl : " +  iCarrierControl)
                     openSelectContactsWnd(iCarrierControl,iLanePrefCarPK);
                 }                
                 //else { ngl.showValidationMsg("LanePrefferedCarrier Record Required", "Invalid Record Identifier, please select a LanePrefferedCarrier to continue", tPage); return false; }
                 
                
             }           
             else if(btn.id == "btnRefresh" || btn === "Refresh" ){ ngl.readDataSource(oLELanePrefCarGrid); }
             else if(btn.id == "btnResetCurrentUserConfig"){ resetCurrentUserConfig(PageControl); }
                 // else if(btn.id == "btnOpenCompany"){ ngl.showSuccessMsg("Coming Soon");}            
                 // else if(btn.id == "btnOpenCarrier"){ngl.showSuccessMsg("Coming Soon");}             
             else if(btn.id == "btnOpenCompany"){
                 if (isLELanePrefCarSelected() === true) {
                     if ("LLTCCarrierContControl" in loadLELanePrefCarGridSelectedRowDataItem ){
                         var compPK = loadLELanePrefCarGridSelectedRowDataItem.LLTCCarrierContControl;
                         var setting = {name:'pk', value: compPK.toString()};
                         var oCRUDCtrl = new nglRESTCRUDCtrl();
                         var blnRet = oCRUDCtrl.update("CompDetail/PostPageSetting", setting, tPage, "savePostPageSettingSuccessCallback", "savePostPageSettingAjaxErrorCallback", tPage);
                     }
                     location.href = "CompanyDetail";
                 }
             }
             else if(btn.id == "btnOpenCarrier"){              
                 if (isLELanePrefCarSelected() === true) {location.href = "LECarrierMaint";}               
             }
              
            
         }
         if (typeof (iLanePrefCarPK) !== 'undefined' && iLanePrefCarPK !== null && iLanePrefCarPK !== 0) {
             var rows = oLELanePrefCarGrid.items();
             $(rows).each(function(e) {
                 var row = this;
                 var dataItem = oLELanePrefCarGrid.dataItem(row);
                 if (dataItem.LaneControl == iLanePrefCarPK) { oLELanePrefCarGrid.select(row); }
             });
         }
     
         $(document).ready(function () {
             var PageMenuTab = <%=PageMenuTab%>;
             
             //if (control != 0){
             //    setTimeout(function () {  
             //        debugger;
             //        ////////////ChangeLEDialogCtrl///////////////////
             //        oChangeLEDialogCtrl = new ChangeLEDialogCtrl();
             //        oChangeLEDialogCtrl.loadDefaults(wndChangeLEDialog, oChangeLEDialogSaveCB);

             //        ////////////LaneEAWndCtrl///////////////////
             //        wdgtLaneEA = new LaneEAWndCtrl(); 
             //        wdgtLaneEA.screenLEControl = pgLEControl;
             //        wdgtLaneEA.loadDefaults(wndLaneEA, wdgtLaneEASaveDeleteCB, wdgtLaneEASaveDeleteCB);
            
             //    }, 10,this);           

             //    // getPageSettings(tPage, "LELaneMaint", "pk", false);
             //}       
             setTimeout(function () {var PageReadyJS = <%=PageReadyJS%>; }, 10,this);
             setTimeout(function () {        
                 menuTreeHighlightPage(); //must be called after PageReadyJS
                 var divWait = $("#h1Wait");
                 if (typeof (divWait) !== 'undefined') { divWait.hide(); }
             }, 10, this);
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
