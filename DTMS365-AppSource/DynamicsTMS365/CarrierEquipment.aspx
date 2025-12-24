<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CarrierEquipment.aspx.cs" Inherits="DynamicsTMS365.CarrierEquipment" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head >
    <title>DTMS Carrier Equipment</title>        
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
         var oCarEquipGrid = null;
         var iCarEquipPK = 0;         
         var tObjPG = this;
         var pgLEControl = 0;
         var pgLEName = "";  

        <% Response.Write(NGLOAuth2); %>


         function execBeforeCarEquipsGridInsert(){
             // prepare for new record like: add code to update the default values for specific fields
             // see TariffRates.aspx for examples
             return true;
           
         }
         <% Response.Write(PageCustomJS); %>

         function saveCarEquipPK() { 
                     try {               
                 loadvCarrierTrucksSelectedRow = oCarEquipGrid.select();
                 if (typeof (loadvCarrierTrucksSelectedRow) === 'undefined' || loadvCarrierTrucksSelectedRow == null) { ngl.showValidationMsg("CarEquip Record Required", "Please select a CarEquip to continue", tPage); return false; }                             
                 loadvCarrierTrucksSelectedRowDataItem = oCarEquipGrid.dataItem(loadvCarrierTrucksSelectedRow); //Get the dataItem for the selected row
                 if (typeof (loadvCarrierTrucksSelectedRowDataItem) === 'undefined' || loadvCarrierTrucksSelectedRowDataItem == null) { ngl.showValidationMsg("CarEquip Record Required", "Please select a CarEquip to continue", tPage); return false; } 
                 if ("CarrierTruckControl" in loadvCarrierTrucksSelectedRowDataItem){                
                     iCarEquipPK = loadvCarrierTrucksSelectedRowDataItem.CarrierTruckControl;
                     var setting = {name:'pk', value: iCarEquipPK.toString()};
                     var oCRUDCtrl = new nglRESTCRUDCtrl();
                     var blnRet = oCRUDCtrl.update("CarrierEquipment/PostPageSetting", setting, tPage, "savePostPageSettingSuccessCallback", "savePostPageSettingAjaxErrorCallback",tPage);
                     //check if the TranCode is PB or PC and enable or disable the Print BOL action button (We can print the BOL for both PB or PC)  
                     return true;
                 } else { ngl.showValidationMsg("CarEquip Record Required", "Invalid Record Identifier, please select a CarEquip to continue", tPage); return false; }
             } catch (err) { ngl.showErrMsg("Cannot Continue", "There was an unexpected error, please reload the screen. If this continues please contact technical support. Error: " + err.message, tPage); }           
         }

         function isCarEquipSelected() {
             if (typeof (iCarEquipPK) === 'undefined' || iCarEquipPK === null || iCarEquipPK === 0) { return saveCarEquipPK(); }
             return true;
         }
         function execActionClick(btn, proc){
             if(btn.id == "btnOpenLane" ){ location.href = "Lane";           
             }else if (btn.id == "btnRefresh" ){                 
                 ngl.readDataSource(oCarEquips);
             }  
             if(btn.id == "btnResetCurrentUserConfig"){
                 resetCurrentUserConfig(PageControl);
             }
         }

         var blnCarEquipGridChangeBound = false;
         //*************  Call Back Functions ****************
         function vCarrierTrucksDataBoundCallBack(e,tGrid){             
             oCarEquipGrid = tGrid;
             if (blnCarEquipGridChangeBound == false){
                 oCarEquipGrid.bind("change", saveCarEquipPK);
                 blnvCarrierTrucksChangeBound = true;
             }        
             //if iLELanePK is not 0 select that row in the grid
             if (typeof (iCarEquipPK) !== 'undefined' && iCarEquipPK !== null && iCarEquipPK !== 0) {
                 var rows = oCarEquipGrid.items();
                 $(rows).each(function(e) {
                     var row = this;
                     var dataItem = oCarEquipGrid.dataItem(row);
                     if (dataItem.LaneControl == iCarEquipPK) { oCarEquipGrid.select(row); }
                 });
             }
         }
           
         // widget call back         
         function CarEquipsGridCB(oResults){   
             if (!oResults) { return;}
             if (oResults.source == "showWidgetCallback"   ){
                 //process any logic needed when the widget is displayed (opened)                 
             }             
         }
         
         // ************** End Call Back functions Functions ******************

         //************* Action Menu Functions ********************
         function execActionClick(btn, proc){
             if(btn.id == "btnOpenCarrier"){ location.href = "LECarrierMaint"; }            
             else if(btn.id == "btnRefresh" || btn === "Refresh" ){ ngl.readDataSource(oCarEquips); }
             else if(btn.id == "btnResetCurrentUserConfig"){ resetCurrentUserConfig(PageControl); }
             //else if(btn.id == "btnCarEquip"){ 
             //    if (isLELaneSelected() === true) {
             //        OpenItem(pgLEControl);
             //    }
             //}
         }
       
         if (typeof (iLELanePK) !== 'undefined' && iLELanePK !== null && iLELanePK !== 0) {
             var rows = oCarEquipGrid.items();
             $(rows).each(function(e) {
                 var row = this;
                 var dataItem = oCarEquipGrid.dataItem(row);
                 if (dataItem.LaneControl == iLELanePK) { oCarEquipGrid.select(row); }
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

