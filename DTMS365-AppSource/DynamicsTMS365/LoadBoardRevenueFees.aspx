<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoadBoardRevenueFees.aspx.cs" Inherits="DynamicsTMS365.LoadBoardRevenueFees" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head >
    <title>DTMS Load Board Revenue Fees</title>       
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
                                <div><span>Menu</span></div>
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
         var BookControlKey = '<%=BookControlKey%>';
         var LaneControlKey = '<%=LaneControlKey%>';
         var CarrierControlKey = '<%=CarrierControlKey%>';
         var CompControlKey = '<%=CompControlKey%>';
         var TariffControlKey = '<%=TariffControlKey%>';

         var tObj = this;
         var tPage = this;           
       

        <% Response.Write(NGLOAuth2); %>

         var oBookRevFeesGrid = null;
         var oTariffFeesGrid = null;
         var oLaneFeesGrid = null;
         var oOrderFeesGrid = null;
         var oPendingFeesGrid = null;




         function execBeforeOrderFeesGridInsert() {
             var blnRet = false;
             var iBookControl = getBookControl("Order Specific Fee");
             if (typeof (iBookControl) !== 'undefined' && iBookControl !== null && iBookControl !== 0) {
                 try {
                     if (typeof (objOrderFeesGridDataFields) !== 'undefined' && objOrderFeesGridDataFields != null) {
                         blnRowSelected = true;
                         $.each(objOrderFeesGridDataFields, function (index, item) {
                             if (item.fieldName == "BFBookControl") {
                                 item.fieldDefaultValue = iBookControl;
                                 blnRet = true;
                                 return blnRet;
                             }
                         });
                     }

                 } catch (err) {
                     ngl.showErrMsg("Save new Order Specific Fee Failure. Contact Technical Support.", err, tPage);
                 }
             }
             return blnRet;
         }

         function getBookControl(type) {
             var iBookControl = 0;
             try {

                 if (typeof (objvLoadBoardSummaryDataFields) !== 'undefined' && objvLoadBoardSummaryDataFields != null) {

                     $.each(objvLoadBoardSummaryDataFields, function (index, item) {
                         if (item.fieldName == "BookControl") {
                             var htmlID = item.fieldTagID
                             if (!htmlID) {
                                 return iCarrTarControl
                             } else {
                                 iBookControl = $("#" + htmlID).val();
                                 return iBookControl;
                             }
                         }
                     });
                 }

             } catch (err) {
                 ngl.showErrMsg("Create new " + type + " record Failure.  Invalid Booking Data.", err, tPage);
             }
             return iBookControl;
         }





         function toolbar_click() {
             alert('hello');
         }
         <% Response.Write(PageCustomJS); %>

         function execActionClick(btn, proc) {
             if (btn.id == "btnRefresh") { refresh(); }
             else if (btn.id == "btnResetCurrentUserConfig") { resetCurrentUserConfig(PageControl); }
             else { ngl.pgNavigation(btn.id, true, BookControlKey); } // modified by RHR forfor v-8.5.4.004 on 12/06/2023 added page navigation menu method
         }
        
         function refresh() {
             var iBookControl = getBookControl("Refresh");
             ngl.readDataSource(oBookRevFeesGrid);
             ngl.readDataSource(oTariffFeesGrid);
             ngl.readDataSource(oLaneFeesGrid);
             ngl.readDataSource(oOrderFeesGrid);
             ngl.readDataSource(oBookRevFeesGrid);
             if (wdgtvLoadBoardSummarySummary) { wdgtvLoadBoardSummarySummary.read(iBookControl); }
             if (wdgtvLoadBoardShippingSummarySummary) { wdgtvLoadBoardShippingSummarySummary.read(iBookControl); }
             //if (wdgtTariffFeesDataEdit) { wdgtTariffFeesDataEdit.read(0); }
             //if (wdgtLaneFeesDataEdit) { wdgtLaneFeesDataEdit.read(0); }
             //if (wdgtOrderFeesDataEdit) { wdgtOrderFeesDataEdit.read(0); }
         }


         //*************  Call Back Functions 
         //oBookRevFeesGrid
         function BookRevFeesGridDataBoundCallBack(e, tGrid) {
             oBookRevFeesGrid = tGrid;
             //Modified by RHR for v-8.5.4.004 on 12/06/2023 new Key table properties
             if (BookControlKey && BookControlKey != 0) {
                 if (wdgtvLoadBoardSummarySummary) { wdgtvLoadBoardSummarySummary.read(BookControlKey); }
                 if (wdgtvLoadBoardShippingSummarySummary) { wdgtvLoadBoardShippingSummarySummary.read(BookControlKey); }

             }
         }
         function TariffFeesGridDataBoundCallBack(e, tGrid) {
             oTariffFeesGrid = tGrid;

             var columns = e.sender.columns;
             var columnIndex = tGrid.wrapper.find(".k-grid-header [data-field=" + "BookHoldLoad" + "]").index();
             var ds = tGrid.dataSource.data();
             for (var j = 0; j < ds.length; j++) {
                 if (typeof (ds[j].BFOverRidden) !== 'undefined' && ds[j].BFOverRidden != null) {
                     if (ds[j].BFOverRidden == true) {                         
                         var item = tGrid.dataSource.get(ds[j].BFControl); //Get by ID or any other preferred method  
                        // debugger;
                         var trRow = $("tr[data-uid='" + item.uid + "']");
                         trRow.css("background-color", "IndianRed");
                         var btnEdit = trRow.find('.k-grid-EditTariffFeesGridCRUDCtrl')
                         btnEdit.hide();
                         
                     }                     
                 }
             }

         }
         //*************  Call Back Functions ****************
         function LaneFeesGridDataBoundCallBack(e, tGrid) {
             oLaneFeesGrid = tGrid;

             var columns = e.sender.columns;
             var columnIndex = tGrid.wrapper.find(".k-grid-header [data-field=" + "BookHoldLoad" + "]").index();
             var ds = tGrid.dataSource.data();
             for (var j = 0; j < ds.length; j++) {
                 if (typeof (ds[j].BFOverRidden) !== 'undefined' && ds[j].BFOverRidden != null) {
                     if (ds[j].BFOverRidden == true) {
                         var item = tGrid.dataSource.get(ds[j].BFControl); //Get by ID or any other preferred method  
                         //debugger;
                         var trRow = $("tr[data-uid='" + item.uid + "']");
                         trRow.css("background-color", "IndianRed");
                         var btnEdit = trRow.find('.k-grid-EditLaneFeesCRUDCtrl')
                         btnEdit.hide();
                     }
                 }
             }
         }
         //*************  Call Back Functions ****************
         function OrderFeesGridDataBoundCallBack(e, tGrid) {
             oOrderFeesGrid = tGrid;
         }
         //*************  Call Back Functions ****************
         function PendingFeesGridDataBoundCallBack(e, tGrid) {
             oPendingFeesGrid = tGrid;
         }
         



         $(document).ready(function () {
             var PageMenuTab = <%=PageMenuTab%>;
                      
            
            if (control != 0){
                //setTimeout(function () {
                    //add code here to load screen specific information this is only visible when a user is authenticated
                    //var oCRUDCtrl = new nglRESTCRUDCtrl();
                    //var blnRet = oCRUDCtrl.read("Tariff/GetCarrierTariffSummary", 6717, tObj, "readCurrentContractSuccessCallback", "readCurrentContractAjaxErrorCallback");
                //}, 1,this);

            }
             var PageReadyJS = <%=PageReadyJS%>
                 //setTimeout(function () {
                 menuTreeHighlightPage(); //must be called after PageReadyJS
             var divWait = $("#h1Wait");
             if (typeof (divWait) !== 'undefined') {
                 divWait.hide();
             }
             //}, 1, this);

         });

     </script>

    <style>
       /* span[id*='Collapseid']{
            background: LightGreen;
        }
        span[id*='Expandid']{
            background: LightGrey;
        }
       */

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

