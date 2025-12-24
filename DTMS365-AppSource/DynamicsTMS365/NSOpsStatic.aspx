<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NSOpsStatic.aspx.cs" Inherits="DynamicsTMS365.NSOpsStatic" %>

<!DOCTYPE html>

<html>
    <head >
        <title>DTMS Next Stop Ops</title>        
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
      <div id="example" style="height: 99%; width: 99%;  margin-top: 2px;">
          <div id="vertical" style="height: 98%; width: 98%;">
              <div id="menu-pane" style="height: 100%; width: 100%;">
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
 
    <% Response.Write(PageTemplates); %>

    <%-- HelpWindow needs to go after AuthNotification because it uses the notification object created there --%>
    <% Response.Write(AuthLoginNotificationHTML); %>  
    <% Response.WriteFile("~/Views/HelpWindow.html"); %>  
    <script>
       <% Response.Write(ADALPropertiesjs); %>
      
        var PageControl = '<%=PageControl%>';
        var tPage = this;  
        var tObj = this;
       

        <% Response.Write(NGLOAuth2); %>

        

        var oPendingLoadsGrid = null;
        var oActiveLoadsGrid = null;
        var oLoadHistoryGrid = null;

        <% Response.Write(PageCustomJS); %>

        //*************  Call Back Function **********************
        function PendingLoadsGridDataBoundCallBack(e,tGrid){           
            oPendingLoadsGrid = tGrid;
        }
        function ActiveLoadsGridDataBoundCallBack(e,tGrid){           
            oActiveLoadsGrid = tGrid;
        }
        function LoadHistoryGridDataBoundCallBack(e,tGrid){           
            oLoadHistoryGrid = tGrid;
        }


        //************* Action Menu Functions ********************
        function execActionClick(btn, proc){
            if (btn.id == "btnRefresh" ){ refresh(); }  
            else if(btn.id == "btnResetCurrentUserConfig"){ resetCurrentUserConfig(PageControl); } 
            else if(btn.id == "btnSuperDelete"){ confirmSuperDelete(); }
        }

        function refresh(){
            oPendingLoadsGrid.dataSource.read();
            oActiveLoadsGrid.dataSource.read();
            oLoadHistoryGrid.dataSource.read();
        }
   
        function acceptNSBid(e) {
            var item = this.dataItem($(e.currentTarget).closest("tr"));

            if (typeof(item) === 'undefined' || ngl.isObject(item) == false){ return; }

            var detailGridWrapper = this.wrapper;         
            var parentRow = detailGridWrapper.closest("tr.k-detail-row").prev("tr"); // GET PARENT ROW ELEMENT   
            var parentGrid = parentRow.closest("[data-role=nglgrid]").data("kendoNGLGrid");            
            var parentModel = parentGrid.dataItem(parentRow); // GET THE PARENT ROW MODEL           
            var ltBookControl = parentModel.LTBookControl; // ACCESS THE PARENT ROW MODEL ATTRIBUTES
         
            kendo.ui.progress($(document.body), true);

            var b = new NSAcceptBid();
            b.LTBookControl = ltBookControl;
            b.BidControl = item.BidControl;
            b.BidLoadTenderControl = item.BidLoadTenderControl;
            b.BidBidTypeControl = item.BidBidTypeControl;
            b.BidCarrierControl = item.BidCarrierControl;
            b.BidLineHaul = item.BidLineHaul;
            b.BidFuelTotal = item.BidFuelTotal;
            b.BidFuelVariable = item.BidFuelVariable;
            b.BidFuelUOM = item.BidFuelUOM;
            b.BidTotalCost = item.BidTotalCost;
            b.BidBookCarrTarEquipMatControl = item.BidBookCarrTarEquipMatControl;
            b.BidBookCarrTarEquipControl = item.BidBookCarrTarEquipControl;
            b.BidBookModeTypeControl = item.BidBookModeTypeControl;
            
            $.ajax({
                type: "Post",
                url: "api/NSOpsStatic/AcceptNSBid",
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                data: JSON.stringify(b),
                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                success: function (data) {     
                    try {
                        var blnSuccess = false;
                        var blnErrorShown = false;
                        var strValidationMsg = "";
                        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                            if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                blnErrorShown = true;
                                ngl.showErrMsg("Accept NS Bid Failure", data.Errors, null);
                            }
                            else {
                                if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                    if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                        blnSuccess = true;
                                        refresh();
                                    }
                                }
                            }
                        }
                        if (blnSuccess === false && blnErrorShown === false) {
                            if (strValidationMsg.length < 1) { strValidationMsg = "Accept NS Bid Failure"; }
                            ngl.showErrMsg("Accept NS Bid Failure", strValidationMsg, null);
                        }
                    } catch (err) {
                        ngl.showErrMsg(err.name, err.description, null);
                    }
                    kendo.ui.progress($(document.body), false);
                },
                error: function (xhr, textStatus, error) {
                    var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure");
                    ngl.showErrMsg("Accept NS Bid Failure", sMsg, null);  
                    kendo.ui.progress($(document.body), false);
                }
            });          
        }

        function deleteNSLoad(iRet,data){
            if (typeof(iRet) === 'undefined' || iRet === null || iRet === 0) { return; } 

            kendo.ui.progress($(document.body), true);
            
            $.ajax({
                type: "Delete",
                url: "api/NSOpsStatic/DeleteNSLoad/" + data.LTBookControl,
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                success: function (data) {     
                    try {
                        var blnSuccess = false;
                        var blnErrorShown = false;
                        var strValidationMsg = "";
                        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                            if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                blnErrorShown = true;
                                ngl.showErrMsg("Delete NS Load Failure", data.Errors, null);
                            }
                            else {
                                if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                    if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                        blnSuccess = true;
                                        refresh();
                                    }
                                }
                            }
                        }
                        if (blnSuccess === false && blnErrorShown === false) {
                            if (strValidationMsg.length < 1) { strValidationMsg = "Delete NS Load Failure"; }
                            ngl.showErrMsg("Delete NS Load Failure", strValidationMsg, null);
                        }
                    } catch (err) {
                        ngl.showErrMsg(err.name, err.description, null);
                    }
                    kendo.ui.progress($(document.body), false);
                },
                error: function (xhr, textStatus, error) {
                    var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Delete Data Failure");
                    ngl.showErrMsg("Delete NS Load Failure", sMsg, null);  
                    kendo.ui.progress($(document.body), false);
                }
            });

        }


        function confirmDeleteNSLoad(e) {
            var item = this.dataItem($(e.currentTarget).closest("tr"));

            if (typeof(item) === 'undefined' || ngl.isObject(item) == false){ return; }

            ngl.OkCancelConfirmation("Delete NEXTStop Posting","Warning! This action cannot be undone. Are you sure you want to continue?",400,400,null,"deleteNSLoad",item); 
        }



        function confirmSuperDelete() {
            try {
                var row = oActiveLoadsGrid.select();
                if (typeof (row) === 'undefined' || row == null) { ngl.showValidationMsg("Active Bid Record Required", "Please select a header record in Active Bids to continue", tPage); return; }                               
                var dataItem = oActiveLoadsGrid.dataItem(row); //Get the dataItem for the selected row
                if (typeof (dataItem) === 'undefined' || dataItem == null) { ngl.showValidationMsg("Active Bid Record Required", "Please select a header record in Active Bids to continue", tPage); return; }                 
                if ("LoadTenderControl" in dataItem){                
                    var msg = "Deletes the LoadTender and associated Bid records by archiving them in the tables. Does not effect the Booking record - does not reset to N status. Should only be used to delete records that have errors and cannot be deleted normally by a user. Are you sure you want to continue?";
                    ngl.OkCancelConfirmation("Super Delete NEXTStop Posting", msg, 400, 400, null, "superDelete", dataItem.LoadTenderControl); 
                } else { ngl.showValidationMsg("Active Bid Record Required", "Invalid Record Identifier, please select a Active Bid to continue", tPage); return false; }
            } catch (err) { ngl.showErrMsg("Cannot Continue", "There was an unexpected error, please reload the screen. If this continues please contact technical support. Error: " + err.message, tPage); }           
        }

        function superDelete(iRet,data) {
            $.ajax({
                type: "Delete",
                url: "api/NSOpsStatic/SuperDeleteNSLoad/" + data,
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                success: function (data) {     
                    try {
                        var blnSuccess = false;
                        var blnErrorShown = false;
                        var strValidationMsg = "";
                        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                            if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Super Delete NS Load Failure", data.Errors, null); }
                            else {
                                if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                    if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') { blnSuccess = true; refresh(); }
                                }
                            }
                        }
                        if (blnSuccess === false && blnErrorShown === false) {
                            if (strValidationMsg.length < 1) { strValidationMsg = "Super Delete NS Load Failure"; }
                            ngl.showErrMsg("Super Delete NS Load Failure", strValidationMsg, null);
                        }
                    } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                    //kendo.ui.progress($(document.body), false);
                },
                error: function (xhr, textStatus, error) {
                    var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Delete Data Failure");
                    ngl.showErrMsg("Super Delete NS Load Failure", sMsg, null);  
                    //kendo.ui.progress($(document.body), false);
                }
            });      
        }
    


        $(document).ready(function () {
            var PageMenuTab = <%=PageMenuTab%>;
          

            var PageReadyJS = <%=PageReadyJS%>; 
            menuTreeHighlightPage(); //must be called after PageReadyJS
            var divWait = $("#h1Wait");
            if  (typeof (divWait) !== 'undefined' ) {
                divWait.hide();
            }

            if (control != 0){
           

            }

        });


    </script>
    <style>

        .k-grid tbody tr td {
            overflow: hidden;
            text-overflow: ellipsis;
            white-space: nowrap;
        }
                
        /*.k-icon.k-i-save { color: blue; }*/
        
    </style>
    </div>

</body>
</html>

