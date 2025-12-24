<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NextStopCarrier.aspx.cs" Inherits="DynamicsTMS365.NextStopCarrier" %>

<%--Added By LVV on 12/23/16 for v-8.0 Next Stop--%>

<!DOCTYPE html>

<html>
    <head >
        <title>DTMS Next Stop Carrier</title>    
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
          
          <div id="wnd">
              <div class="bidWindow">
                  <ul>
                   <%--   <li style="list-style-type: none;">LT Control</li>
                      <li style="list-style-type: none;"><input id="txtLTControl" /></li>--%>
                      <li style="list-style-type: none;" >Order Number</li>
                      <li style="list-style-type: none;"><input id="txtOrderNumber" /></li>
                      <li style="list-style-type: none;">Line Haul</li>
                      <li style="list-style-type: none;"><input id="curBidLineHaul" /></li>
                      <li style="list-style-type: none;">Fuel</li>
                      <li style="list-style-type: none;"><input id="curBidFuelVar" /></li>
                      <li style="list-style-type: none;">Fuel UOM</li>
                      <li style="list-style-type: none;"><input id="ddlFuelUOM"/></li>
                      <li style="list-style-type: none;">Comments</li>
                      <li style="list-style-type: none;"><input id="txtComments"/></li>
                      <li style="list-style-type: none;"></li>
                      <li style="list-style-type: none;"><button type="button" id="btnSubmitBid" onclick="btnSubmitBid_Click();">Submit Bid&nbsp</button></li>
                  </ul>
                  <input id="txtLTControl" type="hidden"/>
                  <input id="txtSHID" type="hidden"/>
                  <input id="txtOrigState" type="hidden"/>
                  <input id="txtDestState" type="hidden"/>
                  <input id="txtTotalMiles" type="hidden"/>
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


        var oAvailableLoadsGrid = null;
        var oFinalizedBidsGrid = null;
        var oPendingBidsGrid = null;

        var wnd = kendo.ui.Window;

        <% Response.Write(PageCustomJS); %>

        //*************  Call Back Function **********************
        function AvailableLoadsGridDataBoundCallBack(e,tGrid){           
            oAvailableLoadsGrid = tGrid;
        }
        function FinalizedBidsGridDataBoundCallBack(e,tGrid){           
            oFinalizedBidsGrid = tGrid;
        }
        function PendingBidsGridDataBoundCallBack(e,tGrid){           
            oPendingBidsGrid = tGrid;
        }

        //************* Action Menu Functions ********************
        function execActionClick(btn, proc){
            if (btn.id == "btnRefresh" ){ refresh(); }  
            else if(btn.id == "btnResetCurrentUserConfig"){ resetCurrentUserConfig(PageControl); }      
        }

        function refresh(){
            oAvailableLoadsGrid.dataSource.read();
            oPendingBidsGrid.dataSource.read();
            oFinalizedBidsGrid.dataSource.read();
        }

        function btnSubmitBid_Click() {
            //create object with same fields as schema in Bids datasource for the new record getting created
            var lineHaul = $("#curBidLineHaul").data("kendoNumericTextBox").value();
            if (lineHaul == null || lineHaul == 0){
                alert("Line Haul cannot be 0");
                return;
            }
            
            var item = {
                BidLoadTenderControl: $("#txtLTControl").val(),
                BidCarrierControl: 0,
                BidSHID: $("#txtSHID").val(),
                BidLineHaul: lineHaul,  
                BidFuelVariable: $("#curBidFuelVar").data("kendoNumericTextBox").value(), 
                BidFuelUOM: $("#ddlFuelUOM").data("kendoDropDownList").text(),
                BidTotalMiles: $("#txtTotalMiles").val(),
                BidOrigState: $("#txtOrigState").val(),
                BidDestState: $("#txtDestState").val(),
                BidPosted: null,
                BidModDate: null,
                BidComments: $("#txtComments").data("kendoMaskedTextBox").value(),
                BidFuelTotal: 0,
                BidTotalCost: 0
            };               

            $.ajax({
                type: "POST",
                url: "api/NextStopCarrier/PostNSBid",
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                data: JSON.stringify(item),
                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                success: function (data) {     
                    try {
                        var blnSuccess = false;
                        var blnErrorShown = false;
                        var strValidationMsg = "";
                        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                            if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                blnErrorShown = true;
                                ngl.showErrMsg("Post NS Bid Failure", data.Errors, null);
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
                            if (strValidationMsg.length < 1) { strValidationMsg = "Post NS Bid Failure"; }
                            ngl.showErrMsg("Post NS Bid Failure", strValidationMsg, null);
                        }
                    } catch (err) {
                        ngl.showErrMsg(err.name, err.description, null);
                    }
                },
                error: function (xhr, textStatus, error) {
                    var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure");
                    ngl.showErrMsg("Post NS Bid Failure", sMsg, null);                        
                }
            });
               
            wnd.close();               
            return;            
        }

        function openBidWindow(e) {

            var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
            $("#txtLTControl").val(dataItem.LoadTenderControl);
            $("#txtOrderNumber").val(dataItem.LTBookCarrOrderNumber);

            $("#txtSHID").val(dataItem.LTBookSHID);
            $("#txtOrigState").val(dataItem.LTBookOrigState);
            $("#txtDestState").val(dataItem.LTBookDestState);
            $("#txtTotalMiles").val(dataItem.LTBookTotalMiles);

            wnd.center().open();
        }
       
        function deleteNSBid(iRet,data) {
            if (typeof(iRet) === 'undefined' || iRet === null || iRet === 0) { return; } 

            $.ajax({
                type: "DELETE",
                url: "api/NextStopCarrier/DeleteNSBid/" + data.BidControl,
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
                                ngl.showErrMsg("Delete NS Bid Failure", data.Errors, null);
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
                            if (strValidationMsg.length < 1) { strValidationMsg = "Delete NS Bid Failure"; }
                            ngl.showErrMsg("Delete NS Bid Failure", strValidationMsg, null);
                        }
                    } catch (err) {
                        ngl.showErrMsg(err.name, err.description, null);
                    }
                },
                error: function (xhr, textStatus, error) {
                    var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Delete Data Failure");
                    ngl.showErrMsg("Delete NS Bid Failure", sMsg, null);                        
                }
            });
        }

        function confirmDeleteNSBid(e) {
            var item = this.dataItem($(e.currentTarget).closest("tr"));

            if (typeof(item) === 'undefined' || ngl.isObject(item) == false){ return; }

            ngl.OkCancelConfirmation("Withdraw Bid","Warning! This action cannot be undone. Are you sure you want to continue?",400,400,null,"deleteNSBid",item); 
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
                 $("#btnSubmitBid").kendoButton();           
                                   
                 $("#curBidLineHaul").kendoNumericTextBox({                       
                     format: "c",                       
                     decimals: 2,
                     restrictDecimals: true,
                     spinners: false
                 });
                
                 $("#curBidFuelVar").kendoNumericTextBox({         
                     //format: "c",                
                     decimals: 2,                    
                     restrictDecimals: true,                   
                     spinners: false               
                 });       
                              
                 $("#ddlFuelUOM").kendoDropDownList({                        
                     dataTextField: "text",
                     dataValueField: "value",
                     dataSource: [
                         { text: "Flat Rate", value: "0" },
                         { text: "Rate Per Mile", value: "1" },
                         { text: "Percentage", value: "2" }
                     ]
                 });
            
                //$("#txtLTControl").kendoMaskedTextBox();                    
                 $("#txtOrderNumber").kendoMaskedTextBox().data("kendoMaskedTextBox").readonly();                 
                 $("#txtComments").kendoMaskedTextBox();
               
                 wnd = $("#wnd").kendoWindow({                       
                     title: "Create Bid",
                     modal: true,
                     visible: false,
                     close: function(e) {
                         $("#txtLTControl").val("");
                         $("#txtOrderNumber").val("");
                         $("#curBidLineHaul").data("kendoNumericTextBox").value(null);
                         $("#curBidFuelVar").data("kendoNumericTextBox").value(null);
                         $("#ddlFuelUOM").data("kendoDropDownList").value("0");
                         $("#txtComments").data("kendoMaskedTextBox").value("");
                         $("#txtSHID").val("");
                         $("#txtOrigState").val("");
                         $("#txtDestState").val("");
                         $("#txtTotalMiles").val("");                                          
                     }                                                  
                 }).data("kendoWindow");          
            }

        });


    </script>
          
    <style>
        
        .bidWindow {
            text-align: center;
        }

        .bidWindow ul {
            display: inline-block;
            margin: 0;
            padding: 0;
            /* For IE, the outcast */
            zoom:1;
            *display: inline;
        }
        
        .bidWindow li {
            padding: 2px 5px;
        }

        .k-grid tbody tr td {
            overflow: hidden;
            text-overflow: ellipsis;
            white-space: nowrap;
        }
 
    </style>
    </div>


    </body>
</html>