<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LELaneHDMMaint.aspx.cs" Inherits="DynamicsTMS365.LELaneHDMMaint" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head >
    <title>DTMS Lane HDM Fee Maintenance</title>        
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
    <% Response.WriteFile("~/Views/LaneHDMMatrixWindow.html"); %>

     <script>
        <% Response.Write(ADALPropertiesjs); %>

        var PageControl = '<%=PageControl%>'; 
          var tObj = this;
         var tPage = this;         
         var oLELaneHDMGrid = null;
         var iLELaneHDMPK = 0;
         var HDMName = '';
         var HDMDesc = '';
         var CarrierName = '';
         var tObjPG = this;
         var pgLEControl = 0;
         var pgLEName = "";      

        <% Response.Write(NGLOAuth2); %>

        

         function execBeforeLELaneHDMGridInsert(){
             // prepare for new record like: add code to update the default values for specific fields
             // see TariffRates.aspx for examples
             return true;
           
         }
         <% Response.Write(PageCustomJS); %>

         function savePostPageSettingSuccessCallback(results){
             //for now do nothing when we save the pk
         }
         function savePostPageSettingAjaxErrorCallback(xhr, textStatus, error){
             //for now do nothing when we save the pk
         }

         function saveLELaneHDMPK() {            
             try {
                 loadLELaneHDMGridSelectedRow = oLELaneHDMGrid.select();
                 if (typeof (loadLELaneHDMGridSelectedRow) === 'undefined' || loadLELaneHDMGridSelectedRow == null) { ngl.showValidationMsg("Lane HDM Record Required", "Please select a record to continue", tPage); return false; }                             
                 loadLELaneHDMGridSelectedRowDataItem = oLELaneHDMGrid.dataItem(loadLELaneHDMGridSelectedRow); //Get the dataItem for the selected row
                 if (typeof (loadLELaneHDMGridSelectedRowDataItem) === 'undefined' || loadLELaneHDMGridSelectedRowDataItem == null) { ngl.showValidationMsg("Lane HDM Record Required", "Please select a record to continue", tPage); return false; } 
                 if ("HDMName" in loadLELaneHDMGridSelectedRowDataItem) { HDMName = loadLELaneHDMGridSelectedRowDataItem.HDMName; } else { HDMName = 'Undefined';}
                 if ("HDMDesc" in loadLELaneHDMGridSelectedRowDataItem) { HDMDesc = loadLELaneHDMGridSelectedRowDataItem.HDMDesc; } else { HDMDesc = 'N/A'; }
                 if ("CarrierName" in loadLELaneHDMGridSelectedRowDataItem) { CarrierName = loadLELaneHDMGridSelectedRowDataItem.CarrierName; } else { CarrierName = 'Any'; }
                 if ("HDMControl" in loadLELaneHDMGridSelectedRowDataItem) {
                     iLELaneHDMPK = loadLELaneHDMGridSelectedRowDataItem.HDMControl;
                     var setting = {name:'pk', value: iLELaneHDMPK.toString()};
                     var oCRUDCtrl = new nglRESTCRUDCtrl();
                     var blnRet = oCRUDCtrl.update("LELaneHDMMaint/PostPageSetting", setting, tPage, "savePostPageSettingSuccessCallback", "savePostPageSettingAjaxErrorCallback",tPage);
                     //check if the TranCode is PB or PC and enable or disable the Print BOL action button (We can print the BOL for both PB or PC)  
                     return true;
                 } else { ngl.showValidationMsg("Lane HDM Record Required", "Invalid Record Identifier, please select a record to continue", tPage); return false; }
             } catch (err) { ngl.showErrMsg("Cannot Continue", "There was an unexpected error, please reload the screen. If this continues please contact technical support. Error: " + err.message, tPage); }           
         }

         function isLELaneHDMSelected() {
             if (typeof (iLELaneHDMPK) === 'undefined' || iLELaneHDMPK === null || iLELaneHDMPK === 0) { return saveLELaneHDMPK(); }
             return true;
         }
         function execActionClick(btn, proc){
             if(btn.id == "btnOpenLane" ){ location.href = "Lane";           
             }else if (btn.id == "btnRefresh" ){                 
                 ngl.readDataSource(oLELaneHDMGrid);
             }  
             if(btn.id == "btnResetCurrentUserConfig"){
                 resetCurrentUserConfig(PageControl);
             }
         }

         var blnLELaneHDMGridChangeBound = false;
         //*************  Call Back Functions ****************
         function LELaneHDMGridDataBoundCallBack(e,tGrid){ 
             oLELaneHDMGrid = tGrid;  
             if (blnLELaneHDMGridChangeBound == false){
                 oLELaneHDMGrid.bind("change", saveLELaneHDMPK);
                 blnLELaneHDMGridChangeBound = true;
             }        
             //if iLELaneHDMPK is not 0 select that row in the grid
             if (typeof (iLELaneHDMPK) !== 'undefined' && iLELaneHDMPK !== null && iLELaneHDMPK !== 0) {
                 var rows = oLELaneHDMGrid.items();
                 $(rows).each(function(e) {
                     var row = this;
                     var dataItem = oLELaneHDMGrid.dataItem(row);
                     if (dataItem.HDMControl == iLELaneHDMPK) { oLELaneHDMGrid.select(row); }
                 });
             }
         }
           
         // widget call back         
         function LELaneHDMGridCB(oResults){   
             if (!oResults) { return;}
             if (oResults.source == "showWidgetCallback"   ){
                 //process any logic needed when the widget is displayed (opened)                 
             }             
         }
         
         // ************** End Call Back functions Functions ******************

         //************* Action Menu Functions ********************
         function execActionClick(btn, proc){
             if (btn.id == "btnLELaneMaint") { location.href = "LELaneMaint"; }
             else if (btn.id == "btnLaneProfileFees") { location.href = "LELaneProfileFees"; }
             else if (btn.id == "btnLanePreferredCarriers") { location.href = "LELanePreferredCarriers"; }
             else if (btn.id == "btnLELaneFees") { location.href = "LELaneFees"; }
             else if (btn.id == "btnRefresh" || btn === "Refresh") { ngl.readDataSource(oLELaneHDMGrid); }
             else if (btn.id == "btnResetCurrentUserConfig") { resetCurrentUserConfig(PageControl); }
             else if (btn.id === "btnImportHDMLocationMatrix") { openImportLaneHDMWnd(iLELaneHDMPK); }
         }       
         
         if (typeof (iLELaneHDMPK) !== 'undefined' && iLELaneHDMPK !== null && iLELaneHDMPK !== 0) {
             var rows = oLELaneHDMGrid.items();
             $(rows).each(function(e) {
                 var row = this;
                 var dataItem = oLELaneHDMGrid.dataItem(row);
                 if (dataItem.HDMControl == iLELaneHDMPK) { oLELaneHDMGrid.select(row); }
             });
         }
         
         $(document).ready(function () {
             var PageMenuTab = <%=PageMenuTab%>;
           
           
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
