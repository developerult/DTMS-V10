<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoadPlanning.aspx.cs" Inherits="DynamicsTMS365.LoadPlanning" %>
<!DOCTYPE html>
<html>
   <head>
      <meta charset="utf-8" />
      <%--Bing Maps--%>
      <title>DTMS Load Planning</title>
      <%=cssReference%>
      <%--    <script src="https://code.jquery.com/jquery-1.12.4.min.js"></script>
         <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
         
         
         <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.0/js/bootstrap.min.js"></script>
         
         <script src="https://kendo.cdn.telerik.com/2020.2.617/js/kendo.all.min.js"></script>
         <script src="Scripts/NGL/v-8.5.4.006/core.js"></script>
         <script src="Scripts/NGL/v-8.5.4.006/NGLobjects.js"></script>
         <script src="Scripts/NGL/v-8.5.4.006/SSOA.js"></script>
         <link href="Content/NGL/v-8.5.4.001/jquery.gridly.css" rel="stylesheet" type="text/css" />
          --%>
      <style>
         html,
         body {
         height: 100%;
         margin: 0;
         padding: 0;
         }
         html {
         font-size: 12px;
         font-family: Arial, Helvetica, sans-serif;
         overflow: hidden;
         }
         .demo-section .k-tabstrip .k-content {
         height: 140px;
         }
         .k-footer-template td:nth-child(1) {
         overflow: visible;
         white-space: nowrap;
         }
         .k-footer-template td:nth-child(1),
         .k-footer-template td:nth-child(2),
         .k-footer-template td:nth-child(3),
         .k-footer-template td:nth-child(4) {
         border-width: 0;
         }
         .demo-section * + h4 {
         margin-top: 2em;
         }
         .demo-section .k-tabstrip .k-content {
         height: 140px;
         }
         .k-altr {
         background-color: #ffffff;
         }
         .loadDet {
         width: 150px;
         }
         .loadDetr {
         width: 50px;
         float: right;
         text-align:right;
         }
         .loadDeter {
         padding-left:20px;
         width: 50px;
         float:right;
         }
         /* #grid { width : 490px; }  .k-grid tbody tr{ height: 38px; }  */
         .k-grid tbody tr td {
         overflow: hidden;
         text-overflow: ellipsis;
         white-space: nowrap;
         }
         .k-tooltip {
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
         #centerDiv {
         position: relative;
         }
         .center-grid-div {
         width: 200px;
         /*height: 300px;*/
         overflow: auto;
         }
         .width200 {
         width: 200px !important;
         }
         .width250 {
         width: 250px !important;
         }
         .width280 {
         width: 280px !important;
         }
         #centerDiv table, #NewLoadsgrid table, #ImportQueue table{
         border-collapse: separate;
         border-spacing: 10px 15px;
         }
         td.details{
         padding: 10px;
         border-left-width: 1px !important;
         border: 1px solid #2c4f60;
         border-radius: 5px;
         }
         #centerDiv table.k-footer-template{
         }
         #grid1FilterFastTab{
         margin-left:0px !important;
         }
         #Lvertical{
         border-width:1px !important;
         }
         #grdgrid1filters{
         margin-top:2em;
         }
         #newTruckgrid .dragOnGrid {
         box-shadow: 0 0 11px rgb(19 125 169);
         background: #1d6f91;
         }

         .resizetable{
             width: 246px !important;
         }
         .resizeImage{
             width: 51px;
         }
        .horizontalView {
             margin: 2em;
             width: 100%; height: 426px;
         }

         .horizontalView .resizetable{
            display: grid;
    grid-template-columns: repeat(auto-fit, minmax(var(--column-width-min), 1fr));
         }
         .horizontalView .resizetr {
             padding: 25px;
         }
        .horizontalView .resizetd{
                padding: 1em;
        }
        .createCNSDiv{
            width: 70%;
            margin: auto;
            margin-top: 2em;
        }
     #grid1 .k-animation-container{
         left: -127px !important;
     }
      </style>
   </head>
   <body>
      <%=jssplitter2Scripts%>
      <%=BingMapsJS%>  <%--Bing Maps--%>
      <%=sWaitMessage%>
      <div id="LBStampCNS" style="display: none; position: absolute; top: 8px; left: 320px; margin: auto; border: 2px solid red; width: auto; text-align: center; padding: 2px; z-index: 5000;">
         <span id="LBStampMsg">Test Msg</span>
      </div>
      <div id="example" style="height: 100%; width: 100%; margin-top: 2px;">
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
                     <div id="pageContent" class="pane-content">
                        <br />
                        <div class="fast-tab" style="">
                           <%--<span id="ExpandFilters" style="display: none;"><a onclick='expandFilters();'><span style="font-size: small; font-weight: bold;" class='k-icon k-i-chevron-down'></span></a></span>
                              <span id="CollapseFilters" style="display: normal;"><a onclick='collapseFilters();'><span style="font-size: small; font-weight: bold;" class='k-icon k-i-chevron-up'></span></a></span>--%>
                           <%-- <span style="font-size: small; font-weight: bold">Load Planning Filters</span>&nbsp;&nbsp;<br />--%>
                           <p style="padding: 10px;">When no filters are applied, the system will use a 14 day load date filter window based on the current date</p>
                           <div id="divFilters" >
                              <asp:Label id="lblChkIds" runat="server"></asp:Label>
                              <asp:Label id="Label1" runat="server"></asp:Label>
                              <div id="id39111201806290836427404336wrapper">
                                 <div id="divid39111201806290836427404336filterContent">  </div>
                              </div>
                              <input id="txtSortDirection" type="hidden" />
                              <input id="txtSortField" type="hidden" />
                              <input id="txtCarrierControlFrom" type="hidden" />
                           </div>
                        </div>
                        <div>
                           <br />
                        </div>
                        <br />
                     </div>
                     <div id="Lvertical" style="height: 100%;">
                        <div id="Ltop-pane">
                           <div id="Lhorizontal" style="height: 100%; width: 100%;">
                              <div id="Lleft-pane">
                                 <div class="pane-content" id="leftgrid">
                                    <h3>&nbsp;Summaries</h3>
                                    <div id="elementID" class="k-header k-grid-toolbar">
                                    </div>
                                    <div id="grid1" style="width: 100%; float: left; height: 500px"></div>
                                 </div>
                              </div>
                              <div id="Lcenter-pane" style="height: 100%;">
                                 <div class="pane-content">
                                    <table border="0" width="100%" style="border: 1px solid #fff !important">
                                       <tr>
                                          <td colspan="3" style="border: 1px solid #ffffff !important;">
                                             <div class="d-flex drag-title createCNSDiv" >
                                                <table width="100%" border="0" style="padding:15px;border-radius: 15px;">
                                                   <tbody>
                                                      <tr>
                                                         <td style="border: 1px solid #ffffff !important" align="center">
                                                            <b>Drop Load to create CNS</b>
                                                            <div id="newTruckgrid" >
                                                            </div>
                                                         </td>
                                                      </tr>
                                                   </tbody>
                                                </table>
                                             </div>
                                          </td>
                                       </tr>
                                       <tr>
                                          <td style="border: 1px solid #ffffff !important">&nbsp;<b>Details</b></td>
                                          <td id="ShowColumns" style="border: 1px solid #ffffff !important">
                                             &nbsp;
                                             <h4>
                                                Show &nbsp;
                                                <select id="showTrucks" onchange="refreshGridly()">
                                                   <option value="2">2</option>
                                                   <option value="3">3</option>
                                                   <option selected value="4">4</option>
                                                   <option value="5">5</option>
                                                   <option value="6">6</option>
                                                </select>
                                                &nbsp;columns<%--&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Select column size--%>
                                                <select id="TruckSize" onchange="setTruckSize()" style="display:none;">
                                                   <option value="200">Small</option>
                                                   <option selected value="250">Medium</option>
                                                   <option value="280">Large</option>
                                                </select>
                                             </h4>
                                             &nbsp;&nbsp;&nbsp;&nbsp;
                                          </td>
                                          <td style="border: 1px solid #ffffff !important" align="right"><a class="k-in k-link" href="javascript:ResizePanes();">
                                             <span id="rotateIcon" class="k-icon k-i-menu"></span>
                                             <span id="paneAction">Collapse All Panes</span>
                                          </td>
                                       </tr>
                                    </table>
                                    <div id="centerDiv" class="target" style="width: 100%;">
                                    </div>
                                 </div>
                              </div>
                              <div id="Lright-pane" style="height: 100%;">
                                 <h3>&nbsp;New Loads(<span id="TotNLcnt"></span>)</h3>
                                 <div id="tabstrip">
                                    <ul>
                                       <li class="k-active">New Loads(<span id="NLcnt"></span>)<%--(Count of new Loads)--%>
                                       </li>
                                       <li>Import Queue(<span id="ILcnt"></span>)
                                       </li>
                                    </ul>
                                    <div>
                                       <div>
                                          <div class="pane-content" id="NewLoadsgrid">
                                          </div>
                                       </div>
                                    </div>
                                    <div>
                                       <div class="pane-content" id="ImportQueue"></div>
                                    </div>
                                 </div>
                              </div>
                           </div>
                        </div>
                     </div>
                     <%-- <% Response.Write(FastTabsHTML); %>--%>
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
          
             <%-- Modified by RHR for v-8.5.3.007 on 2023-02-03 updated references:  
              Dispatching Dialog logic 
              Dispatch Report
              BOL Report
              Trimble Map
              Removed Bing Maps--%>
            <%-- <% Response.WriteFile("~/Views/MapWindow.html"); %> --%>
          <%--Bing Maps--%>
         
         <% Response.WriteFile("~/Views/DispatchingDialog.html"); %>
         <% Response.WriteFile("~/Views/DispatchReport.html"); %>
         <% Response.WriteFile("~/Views/BOLReport.html"); %>
         <% Response.WriteFile("~/Views/MapItTrimble.html"); %>
         <% Response.WriteFile("~/Views/HelpWindow.html"); %>
         <% Response.WriteFile("~/Views/wndTruckActiveCarrier.html"); %>
         <% Response.WriteFile("~/Views/LPModifyWindow.html"); %>
         <% Response.WriteFile("~/Views/LPCarrierContactWindow.html"); %>
         <% Response.WriteFile("~/Views/LPCreateBookingWindow.html"); %>
         <script id="rowTemplate" type="text/x-kendo-tmpl">
            <tr data-uid="#: uid #">
              <td class="details">
                <span class="loadDet"><b> #: SolutionDetailOrderNumber # </b> - #: SolutionDetailOrderSequence #</span>
                <span class="loadDetr">#:  SolutionDetailStopNo # </span><br>
                <span> #: SolutionDetailDestName # </span><br>
                <span class="loadDet"> #: SolutionDetailDestCity #,</span>
                <span class="loadDetr"> #: SolutionDetailDestState # </span><br>
                <span>#: SolutionDetailDefaultRouteSequence #</span><br>
              </td>
            </tr>
         </script>
         <script id="altRowTemplate" type="text/x-kendo-tmpl">
            <tr class="k-alt" data-uid="#: uid #">
              <td class="details">
                <span class="loadDet"><b> #: SolutionDetailOrderNumber # </b> - #: SolutionDetailOrderSequence #</span>
                <span class="loadDetr">#:  SolutionDetailStopNo # </span><br>
                <span> #: SolutionDetailDestName # </span><br>
                <span class="loadDet"> #: SolutionDetailDestCity #,</span>
                <span class="loadDetr"> #: SolutionDetailDestState # </span><br>
                <span>#: SolutionDetailDefaultRouteSequence #</span><br>
              </td>
            </tr>
         </script>
         <script id="NLoads-rowTemplate" type="text/x-kendo-tmpl">
            <tr data-uid="#: uid #">
              <td class="details">
                <span class="loadDet"><b> #: SolutionDetailOrderNumber # </b></span>
                <span style="text-align: right;">#:  SolutionDetailStopNo # </span>
                <span class="loadDeter">Wgt #: SolutionDetailTotalWgt #&nbsp;</span><br>
                <span> #: SolutionDetailDestName # </span>
                <span class="loadDeter">Pits #: SolutionDetailTotalPL #&nbsp;</span><br>
                <span class="loadDet"> #: SolutionDetailDestCity #,</span>
                <span> #: SolutionDetailDestState # </span>
                <span class="loadDeter">Quantity #: SolutionDetailTotalCube #&nbsp;</span><br>
                <span>#: SolutionDetailDefaultRouteSequence #</span>
                <span class="loadDeter">Volume #: SolutionDetailTotalCube #&nbsp;</span><br>
              </td>
            </tr>
         </script>
         <script id="NLoads-altRowTemplate" type="text/x-kendo-tmpl">
            <tr class="k-alt" data-uid="#: uid #">
              <td class="details">
                <span class="loadDet"><b> #: SolutionDetailOrderNumber # </b></span>
                <span style="text-align: right;">#:  SolutionDetailStopNo # </span>
                <span class="loadDeter">Wgt #: SolutionDetailTotalWgt #&nbsp;</span><br>
                <span> #: SolutionDetailDestName # </span>
                <span class="loadDeter">Pits #: SolutionDetailTotalPL #&nbsp;</span><br>
                <span class="loadDet"> #: SolutionDetailDestCity #,</span>
                <span> #: SolutionDetailDestState # </span>
                <span class="loadDeter">Quantity #: SolutionDetailTotalCube #&nbsp;</span><br>
                <span>#: SolutionDetailDefaultRouteSequence #</span>
                <span class="loadDeter">Volume #: SolutionDetailTotalCube #&nbsp;</span><br>
              </td>
            </tr>
         </script>
         <script id="IQLoads-rowTemplate" type="text/x-kendo-tmpl">
            <tr data-uid="#: uid #">
              <td class="details">
                <span class="loadDet"><b> #: SolutionDetailOrderNumber # </b></span>
                <span style="text-align: right;">#:  SolutionDetailStopNo # </span>
                <span class="loadDeter">Wgt #: SolutionDetailTotalWgt #&nbsp;</span><br>
                <span> #: SolutionDetailDestName # </span>
                <span class="loadDeter">Pits #: SolutionDetailTotalPL #&nbsp;</span><br>
                <span class="loadDet"> #: SolutionDetailDestCity #,</span>
                <span> #: SolutionDetailDestState # </span>
                <span class="loadDeter">Quantity #: SolutionDetailTotalCube #&nbsp;</span><br>
                <span>#: SolutionDetailDefaultRouteSequence #</span>
                <span class="loadDeter">Volume #: SolutionDetailTotalCube #&nbsp;</span><br>
              </td>
            </tr>
         </script>
         <script id="IQLoads-altRowTemplate" type="text/x-kendo-tmpl">
            <tr class="k-alt" data-uid="#: uid #">
              <td class="details">
                <span class="loadDet"><b> #: SolutionDetailOrderNumber # </b></span>
                <span style="text-align: right;">#:  SolutionDetailStopNo # </span>
                <span class="loadDeter">Wgt #: SolutionDetailTotalWgt #&nbsp;</span><br>
                <span> #: SolutionDetailDestName # </span>
                <span class="loadDeter">Pits #: SolutionDetailTotalPL #&nbsp;</span><br>
                <span class="loadDet"> #: SolutionDetailDestCity #,</span>
                <span> #: SolutionDetailDestState # </span>
                <span class="loadDeter">Quantity #: SolutionDetailTotalCube #&nbsp;</span><br>
                <span>#: SolutionDetailDefaultRouteSequence #</span>
                <span class="loadDeter">Volume #: SolutionDetailTotalCube #&nbsp;</span><br>
              </td>
            </tr>
         </script>
         <script src="Scripts/jquery.gridly.js" type="text/javascript"></script>
          
<style>
    #optMsgGrid .k-loading-image { background-image: none; }
    #optMsgGrid .k-loading-color { opacity: 0; }
</style><script type="text/x-kendo-template" id=BidDetailAdjErrTemplate>
    <div class="tabstrip">
    <ul>
    <li class="k-active">Adjustments</li>
    <li>Errors</li>
    </ul>
    <div>
    <div class="adjustments"></div>
    </div><div><div class="errors"></div></div></div></script>

<script type="text/x-kendo-template" id="GridDetailAccessorialAndPackageTemplate">
    <div class="tabstrip">
    <ul>
    <li class="k-active">Packages or Pallets</li>
    <li >Accessorials</li>
    </ul>
    <div>
    <div  class="packages" ></div>
    </div>
    <div><div class="accessorials" ></div></div>
    </div>

</script>


         <script>
            <% Response.Write(ADALPropertiesjs); %>
            var PageControl = '<%=PageControl%>';
            var tObj = this;
             var tPage = this;
            var vSummaryLoadsGrid365 = null;
            
            <% Response.Write(NGLOAuth2); %>
            
            
            $("#tabstrip").kendoTabStrip({
               tabPosition: "top",
               animation: { open: { effects: "fadeIn" } }
            });
            TruckKeys=[],TruckPageSize=0,IsCollapse=new Boolean(false),TruckSize=0; 
             $(document).ready(function () {
                 $("#sp" + tObj.IDKey + "filterText").hide();
                 $("#sp" + tObj.IDKey + "filterDates").hide();
                 $("#sp" + tObj.IDKey + "filterButtons").hide();
             
               let cookieValue = getDetailsVerticalViewCookie();
               if (cookieValue)
                   Rotate(cookieValue);
            
               $("#Lvertical").kendoSplitter({
                   orientation: "vertical",
                   panes: [
                       { collapsible: false },
                       { collapsible: false },
                       { collapsible: false }
                   ],
                   resize: function(e) {
                       $('#centerDiv').gridly({
                           base: 650, // px 
                           gutter: 5, // px
                           columns: 16,
                           draggable: {
                               zIndex: 800,
                               selector: '.drag-title'
                           }
                       });
                   }
               });  
            
               $("#Lhorizontal").kendoSplitter({
                   panes: [
                       { collapsible: true, size: "30%" },
                       { collapsible: false },
                       { collapsible: true, size: "25%"}
                   ],
                   resize: function(e) {
                       $('#centerDiv').gridly({
                           base: 60, // px 
                           gutter: 5, // px
                           columns: 16,
                           draggable: {
                               zIndex: 800,
                               selector: '.drag-title'
                           }
                       });
                   }
               });
            });
            
            //Active carrier's Add truck fucntion
            function btnActAddTruck(){
               var control = $("#ddlActCar").data("kendoDropDownList").value();
               var name = $("#ddlActCar").data("kendoDropDownList").text();
               if (ngl.isNullOrUndefined(control) || control === "") { control = 0; }                     
               var dropdownlist = $("#ddlActCar").data("kendoDropDownList");        
               var dataItem = dropdownlist.dataItem(); // get the dataItem corresponding to the selectedIndex.
               var dataJSON = { SolDetailCompControl: control, SolDetailBookControl:inewTruckBookPK,solTruckKey:inewTruckBookPKType };
               //kendo.ui.progress($(document.body), true); 
               $.ajax({
                   type:"POST",
                   url: "api/LoadPlanning/AddNewTruckItem/",
                   contentType: "application/json; charset=utf-8",
                   dataType: 'json',
                   data: JSON.stringify(dataJSON),
                   headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                   success: function(data) {
            
                       if (data.Errors != null) { 
            
                           if (data.StatusCode === 203){ 
                               ngl.showErrMsg("Authorization Timeout", data.Errors, null); 
                           } 
                           else 
                           { 
                               ngl.showErrMsg("Access Denied", data.Errors, null); 
                           } 
                       } else {    
                          
                            $("#wndTruckActCar").data("kendoWindow").close();
                           //Refreshes New Loads grid
                            $('#NewLoadsgrid').data('kendoNGLGrid').dataSource.read();
                            $('#NewLoadsgrid').data('kendoNGLGrid').refresh();
                           //Refreshes Import Queue grid
                            $('#ImportQueue').data('kendoNGLGrid').dataSource.read();
                            $('#ImportQueue').data('kendoNGLGrid').refresh();
                           TotLcnt = parseInt($("#ILcnt").text()) + parseInt($("#NLcnt").text());
                           $("#TotNLcnt").text(TotLcnt);
                           ngl.showSuccessMsg("Created New Truck and Dropped Load");
                           document.location.reload();
            
                       } 
                   },
                   error: function (xhr, textStatus, error) {
                       //kendo.ui.progress($(document.body), false); 
                       var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Add New Truck Item Failure");
                       ngl.showErrMsg("Add New Truck Item Failure", sMsg, null);
            
                   }
               });
            
               //var oCRUDCtrl = new nglRESTCRUDCtrl();
               //var blnRet = oCRUDCtrl.read("LoadPlanning/AddNewTruckItem", inewTruckBookPK, tPage, "AddTrcukSuccessCallback", "AddTruckAjaxErrorCallback",true);
            }
            
            function AddTrcukSuccessCallback(data){ alert("Success"); }        
            function AddTruckAjaxErrorCallback(data){ 
              // debugger;
               ngl.showErrMsg("Add Truck Failure", formatAjaxJSONResponsMsgs(xhr, textStatus, error,'Failed'), tPage);
            }
            
            function getDetailsVerticalViewCookie() {
               //read cookie
              let name = "LoadPanningDetailsVerticalView=";
              let decodedCookie = decodeURIComponent(document.cookie);
              let ca = decodedCookie.split(';');
              for(let i = 0; i <ca.length; i++) {
                let c = ca[i];
                while (c.charAt(0) == ' ') {
                  c = c.substring(1);
                }
                if (c.indexOf(name) == 0) {
                  return c.substring(name.length, c.length);
                }
              }
              return false;
            }
            
            $("#rotateIcon").click(function (e) {
               if ($('span.k-i-columns').length > 0) {
                   //delete cookie
                   document.cookie = 'LoadPanningDetailsVerticalView=; Max-Age=0'
                   Rotate(false);
               }
               else {
                   Rotate(true);
                   //set cookie
                   document.cookie = "LoadPanningDetailsVerticalView=true; expires=Fri, 31 Dec 9999 23:59:59 GMT; path=/";
               }
            });
            
            function Rotate(addEvent) {
          
                var verticalview = getDetailsVerticalViewCookie();
                if (addEvent || verticalview) {
                   $('#rotateIcon').removeClass('k-i-menu').addClass('k-i-columns');
                   $('#ShowColumns').css('visibility', 'hidden');
                   $('div#centerDiv div.center-grid-div').removeClass('width250').removeClass('center-grid-div').css({ "display": "flex", "flex-direction": "row" });
                   $('div#centerDiv tbody').data('role', 'rowgroup').css({ "display": "flex", "flex-direction": "row" });
                   $('div#centerDiv div[id^="dy_grid_"]').css({ "display": "flex" });
                   $('div#centerDiv div[id^="dy_grid_"] table').css({ "table-layout": "inherit" });
                  // $('div#centerDiv').css({ "height": "260px" });
                   $('div#centerDiv table.width250').removeClass('width250');
                   var pxValue = 0;
                   $('#centerDiv').find('div[id^="dy_div_"]').each(function () {
                       $(this).css({
                           "top": `${pxValue}px`,
                           "left": "0px",
                       });
                       pxValue += 210;
                   });
                   $('div#centerDiv div[id^="dy_div_"] .k-grid-content').css({ "overflow": "inherit" });
            
               }
               else {
            
                   $('#rotateIcon').removeClass('k-i-columns').addClass('k-i-menu');
            
                   $('#ShowColumns').css('visibility', 'visible');
            
                   $('div#centerDiv div[id^="dy_div_"]').css('display', '')
                   $('div#centerDiv div[id^="dy_div_"]').css('flex-direction', '')
                   $('div#centerDiv div[id^="dy_div_"]').addClass('width250').addClass('center-grid-div');
            
                   $('div#centerDiv tbody').data('role', 'rowgroup').css('display', '');
                   $('div#centerDiv tbody').data('role', 'rowgroup').css('flex-direction', '');
            
                   $('div#centerDiv div[id^="dy_grid_"]').data('role', 'rowgroup').css('display','');
            
                   $('div#centerDiv tbody').data('role', 'rowgroup').removeAttr("display");
            
                   $('div#centerDiv div[id^="dy_grid_"] table').removeAttr("table-layout");
            
                   $('div#centerDiv div[id^="dy_grid_"] table').css({ "table-layout": "fixed" });
            
                   $('div#centerDiv').css({ "height": "500px" });
            
                   $('div#centerDiv table.width250').addClass('width250');
                                       
                   var pxValue = 0;
                   $('#centerDiv').find('div[id^="dy_div_"]').each(function () {
                       $(this).css({
                           "top": `0px`,
                           "left": `${pxValue}px`,
                       });
                       pxValue += 300;
                   });
            
                   $('div#centerDiv div[id^="dy_div_"] .k-grid-content').css({ "overflow": "auto" });
            
                   refreshGridly();
               }
            }
            
            function ResizePanes()
            {
               paneMode = $("#paneAction").text();
               if(paneMode == "Collapse All Panes"|| IsCollapse==='True') {//Added for PageSettings
                   $("#centerDiv").addClass('horizontalView');
                    $(".createCNSDiv").css({width: "25%"});
                   IsCollapse=true;                    
                   var TMSHsplitter = $("#horizontal").data("kendoSplitter");
                   // modify the size of the menu and content pane
                   TMSHsplitter.options.panes[0].size = "0px";
                   TMSHsplitter.options.panes[1].size = "1875px";
                   // force layout readjustment
                   TMSHsplitter.resize(true);
            
                   var TMSVsplitter = $("#vertical").data("kendoSplitter");
                   // modify the size of the bottom pane
                   TMSVsplitter.options.panes[2].size = "0px";
                   // force layout readjustment
                   TMSVsplitter.resize(true);
            
                   // get Splitter object
                   var splitter = $("#Lhorizontal").data("kendoSplitter");
                   // modify the size of the summaries and new loads pane
                   splitter.options.panes[0].size = "0%";
                   splitter.options.panes[2].size = "0%";
                   // force layout readjustment
                   splitter.resize(true);
            
                   $("#paneAction").text("Expand All Panes");
                   IsCollapse=true;//Added for PageSettings
                   saveSelectedPageChanges(TruckKeys,TruckPageSize,IsCollapse,TruckSize);//Added for PageSettings
               }
               else {
                     $("#centerDiv").removeClass('horizontalView');
                   var TMSHsplitter = $("#horizontal").data("kendoSplitter");
                   // modify the size of the menu and content pane
                   TMSHsplitter.options.panes[0].size = "150px";
                   TMSHsplitter.options.panes[1].size = "1725px";
                   // force layout readjustment
                   TMSHsplitter.resize(true);
            
                   var TMSVsplitter = $("#vertical").data("kendoSplitter");
                   // modify the size of the bottom pane
                   TMSVsplitter.options.panes[2].size = "41px";
                   // force layout readjustment
                   TMSVsplitter.resize(true);
            
                   // get Splitter object
                   var splitter = $("#Lhorizontal").data("kendoSplitter");
                   // modify the size of the summaries and new loads pane
                   splitter.options.panes[0].size = "30%";
                   splitter.options.panes[2].size = "25%";
                   // force layout readjustment
                   splitter.resize(true);
            
                   $("#paneAction").text("Collapse All Panes");
                   IsCollapse=false;//Added for PageSettings
                   saveSelectedPageChanges(TruckKeys,TruckPageSize,IsCollapse,TruckSize);//Added for PageSettings
               }
            }
            
            function refreshGridly() {
               var dispTrucks = 4;
               if($("#showTrucks").val() != "")
                   dispTrucks = $("#showTrucks").val();
            
               dispTrucks = dispTrucks*4;
            
               TruckPageSize=$("#showTrucks").val();//Added for PageSettings
               saveSelectedPageChanges(TruckKeys,TruckPageSize,IsCollapse,TruckSize);//Added for PageSettings
               $('#centerDiv').gridly('refresh', {
                   base: 60, // px 
                   gutter: 5, // px
                   columns: dispTrucks,
                   draggable: {
                       zIndex: 800,
                       selector: '.drag-title'
                   }
               });
            }
            
            function setTruckSize() {
               var curgrid = "";
               //$('[id^=dy_grid]').each(function() {
               $(".center-grid-div").each(function(){
                   //Removes current width style class for trucks
                   $("#"+this.id).removeClass("width200");
                   $("#"+this.id).removeClass("width250");
                   $("#"+this.id).removeClass("width280");
                   $("#t"+this.id).removeClass("width200");
                   $("#t"+this.id).removeClass("width250");
                   $("#t"+this.id).removeClass("width280");
            
                   //sets new width class for the truck
                   $("#"+this.id).addClass("width" + $("#TruckSize").val());
                   $("#t" + this.id).addClass("width" + $("#TruckSize").val());
                   TruckSize=$("#TruckSize").val();
                   $('#centerDiv').gridly('refresh', {
                       gutter: 5, // px
                       draggable: {
                           zIndex: 800,
                           selector: '.drag-title'
                       }
                   });
               });
            
               saveSelectedPageChanges(TruckKeys,TruckPageSize,IsCollapse,TruckSize);//Added for PageSettings
            }
            
            <% Response.Write(ADALPropertiesjs); %>
            
            var PageControl = '<%=PageControl%>';
            var wnd = kendo.ui.Window;
            var tObj = this;
            var tPage = this;
            var iBookPK = 0;
            var iTruckPK = 0;
            var iCompControl = 0;
            var iTruckCompControl = 0;
            var inewTruckBookPK = 0;
            var inewTruckBookPKType = "";
            var BookTotalWgt = 0;
            var blnCanPrintBOL = false; //page level variable to determine if selected record can print BOL 
            var blnCanPrintDispatch = false; //page level variable to determine if selected record can print Dispatch 
            var blnBtnDispatchReportClicked = false; //indicates whether the Dispatch Report was generated as result of Dispatching a load or from clicking the Report Menu button. Impacts whether BOL is automatically shown or not
            var pgLEControl = 0;
            var blnAutoMiles = false;
            var blnAutoCalculate = false;
            var blnAutoStopResequence = false;
            var sStampingCNS = "";
            <% Response.Write(NGLOAuth2); %>
            
            
            <% Response.Write(PageCustomJS); %>
            
            //*************  Action Menu Functions ****************
            function BingMapsCaller() { 
            //var wgt = 0;
            //if ("BookTotalWgt" in loadBoardGridSelectedRowDataItem) { wgt = loadBoardGridSelectedRowDataItem.BookTotalWgt; }           
            MapIt(iBookPK,BookTotalWgt);
            }
            
            function isBookingSelected() {
                if (typeof (iBookPK) === 'undefined' || iBookPK === null || iBookPK === 0) { return saveBookPK(); }
                return true;
            }
            
            function saveBookPK() {
              try {
                   if (iBookPK == null || iBookPK == 0) { ngl.showValidationMsg("Booking Record Required", "Please select a Booking to continue", tPage); return false; }
                  var iOldBookPK = iBookPK;
                  iBookPK = loadBoardGridSelectedRowDataItem.BookControl;
                   if (iBookPK > 0){
                       var setting = {name:'pk', value: iBookPK.toString()};
                       var oCRUDCtrl = new nglRESTCRUDCtrl();
                       var blnRet = oCRUDCtrl.update("LoadPlanning/PostPageSetting", setting, tPage, "savePostPageSettingSuccessCallback", "savePostPageSettingAjaxErrorCallback",tPage);
                       return true;
                   } else { ngl.showValidationMsg("Booking Record Required", "Invalid Record Identifier, please select a Booking to continue", tPage); return false; }
               } catch (err) { ngl.showErrMsg("Cannot Continue", "There was an unexpected error, please reload the screen. If this continues please contact technical support. Error: " + err.message, tPage); }           
            }
            
            function stopReseqGetMilesSuccessCallback(data, errTitle){           
            try {
               kendo.ui.progress($(document.body), false);                
               var blnSuccess = false;
               var blnErrorShown = false;
               var strValidationMsg = "";
               if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                   if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg(errTitle, data.Errors, null); }
                   else {
                       if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                           if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                               blnSuccess = true;                             
                               if (data.Data[0].Success === true) {
                                   if (ngl.stringHasValue(data.Data[0].SuccessMsg)){ 
            
                                       //Invoked to refresh the loads in truck and display updated stop/sequence number
                                       var ds = $("#dy_grid_" + iTruckPK + "___0").data().kendoNGLGrid.dataSource;
            
                                       $.ajax({
                                           url: "api/LoadPlanning/GetLoadsInTruck/",
                                           contentType: "application/json; charset=utf-8",
                                           dataType: 'json',
                                           data: {CompControl: JSON.stringify(iTruckCompControl), TruckKey: JSON.stringify(iTruckPK)},
                                           headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                                           success: function(data) {
            
                                               if (data.Errors != null) { 
            
                                                   if (data.StatusCode === 203){ 
                                                       ngl.showErrMsg("Authorization Timeout", data.Errors, null); 
                                                   } 
                                                   else 
                                                   { 
                                                       ngl.showErrMsg("Access Denied", data.Errors, null); 
                                                   } 
                                               } else {
                                                            
                                                   //console.log(data.Data);
                                                   ds.data(data.Data);
                                                   ds.sync();
                                               } 
                                           },
                                           error: function (xhr, textStatus, error) {
                                               var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure");
                                               ngl.showErrMsg("Load Planning - Orders in Truck", sMsg, null);
                                           }
                                       });
            
                                       ngl.showSuccessMsg(data.Data[0].SuccessMsg, null); 
                                   }                                   
                               }
                               else{
                                   if (ngl.stringHasValue(data.Data[0].ErrMsg)){ ngl.showErrMsg(errTitle, data.Data[0].ErrMsg, null); }
                               }
                               if (ngl.stringHasValue(data.Data[0].WarningMsg)){ ngl.showWarningMsg("", data.Data[0].WarningMsg, null); }
                           }
                       }
                   }
               }
               if (blnSuccess === false && blnErrorShown === false) {
                   if (strValidationMsg.length < 1) { strValidationMsg = errTitle; }
                   ngl.showErrMsg(errTitle, strValidationMsg, null);
               }
               execActionClick("Refresh");
            } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
            }
            function stopResequenceSuccessCallback(data){ stopReseqGetMilesSuccessCallback(data, "Stop Resequence Failure"); }
            function stopResequenceAjaxErrorCallback(results){ ngl.showErrMsg("Stop Resequence Failure", formatAjaxJSONResponsMsgs(xhr ,textStatus, error,'Failed'), tPage); }
            function getMilesSuccessCallback(data){ stopReseqGetMilesSuccessCallback(data, "Get Miles Failure"); }        
            function getMilesAjaxErrorCallback(results){ ngl.showErrMsg("Get Miles Failure", formatAjaxJSONResponsMsgs(xhr, textStatus, error,'Failed'), tPage); } 
               
            function StopResequence(iBookPK){
            kendo.ui.progress($(document.body), true);            
            var oCRUDCtrl = new nglRESTCRUDCtrl();
            var blnRet = oCRUDCtrl.read("LoadPlanning/StopResequence", iBookPK, tPage, "stopResequenceSuccessCallback", "stopResequenceAjaxErrorCallback",true);
            
            }
            
            function getMiles(iBookPK){
            kendo.ui.progress($(document.body), true);
            var oCRUDCtrl = new nglRESTCRUDCtrl();
            var blnRet = oCRUDCtrl.read("LoadPlanning/GetMiles", iBookPK, tPage, "getMilesSuccessCallback", "getMilesAjaxErrorCallback",true);
            } 
            
            let loadBoardGridSelectedRowDataItem = {
            BookControl: 0,
            BookTranCode: 0,
            BookDateOrdered: "",
            BookDateRequired: "",
            BookDateLoad: "",
            BookFinARInvoiceDate: "",
            BookCarrierControl: 0,
            BookLockAllCosts: "",
            BookSHID: 0,
            BookDateDelivered: "",
            BookRouteConsFlag: 0,
            OnCreditHold: 0
            };
            
            function BookingTenderWorkFlowOptionCtrlCB(oResults){                     
            try {            
               //This parameter has a reference to the workFlowSettings in oResults.data when oResults.datatype = "WorkFlowSetting";
               //Check that oResults.source = "readUserSettingsSuccessCallback";
               //And oResults.msg = "Success"           
               //You can then use custom code to modify the visibility of the workFlowSettings before the load HTML method is called.  Just make sure you do not call any asynchronous methods or you will need to refresh the page.           
               //I suggest you read any settings needed before you call read for the widget then when the call back is triggered you have everything you need to show or hide check boxes,  like the trans code or other options.       
               if (oResults){
                   if (oResults.source === "readUserSettingsSuccessCallback" && oResults.msg === "Success"){
                       if (oResults.datatype === "WorkFlowSetting"){                      
                           var actions = getAvailableBookingActions();
                           var l = oResults.data.length;                                               
                           for (var j=0; j < l; j++) { 
                               var fName = oResults.data[j].fieldName;      
                               if (fName.substring(0, 2) === "yn"){    
                                   //check if the name of the workflow object is in the actions array
                                   if($.inArray(fName, actions) >= 0){ oResults.data[j].fieldVisible = true; oResults.data[j].fieldLockVisibility = "false"; } else{ oResults.data[j].fieldVisible = false; oResults.data[j].fieldLockVisibility = "true"; }                                                             
                               }                      
                           }                    
                       }                                                      
                   }          
               }     
            } catch (err) { ngl.showErrMsg("Cannot Continue", "There was an unexpected error, please reload the screen. If this continues please contact technical support. Error: " + err.message, tPage); }       
            }
            
            function getAvailableBookingActions(){   
            //Get the dataItem for the selected row
            if (typeof (loadBoardGridSelectedRowDataItem) === 'undefined' || loadBoardGridSelectedRowDataItem == null) { ngl.showValidationMsg("Booking Record Required", "Please select a Booking to continue", tPage); return false; }
            var actions = new Array();           
            var bkControl = 0, carrierControl = 0, routeConsFlag = 0;
            var tranCode = "", dtOrdered = "", dtRequired = "", dtLoad = "", dtInvoice = "", dtDelivered = "", shid = "";
            var lockAllCosts = true, blnNEXTStop = false, blnDAT = false;
            if ("BookControl" in loadBoardGridSelectedRowDataItem){ bkControl = loadBoardGridSelectedRowDataItem.BookControl; } 
            if ("BookTranCode" in loadBoardGridSelectedRowDataItem){ tranCode = loadBoardGridSelectedRowDataItem.BookTranCode; } 
            if ("BookDateOrdered" in loadBoardGridSelectedRowDataItem){ dtOrdered = loadBoardGridSelectedRowDataItem.BookDateOrdered; } 
            if ("BookDateRequired" in loadBoardGridSelectedRowDataItem){ dtRequired = loadBoardGridSelectedRowDataItem.BookDateRequired; } 
            if ("BookDateLoad" in loadBoardGridSelectedRowDataItem){ dtLoad = loadBoardGridSelectedRowDataItem.BookDateLoad; } 
            if ("BookFinARInvoiceDate" in loadBoardGridSelectedRowDataItem){ dtInvoice = loadBoardGridSelectedRowDataItem.BookFinARInvoiceDate; } 
            if ("BookCarrierControl" in loadBoardGridSelectedRowDataItem){ carrierControl = loadBoardGridSelectedRowDataItem.BookCarrierControl; } 
            if ("BookLockAllCosts" in loadBoardGridSelectedRowDataItem){ lockAllCosts = loadBoardGridSelectedRowDataItem.BookLockAllCosts; } 
            if ("BookSHID" in loadBoardGridSelectedRowDataItem){ shid = loadBoardGridSelectedRowDataItem.BookSHID; } 
            if ("BookDateDelivered" in loadBoardGridSelectedRowDataItem){ dtDelivered = loadBoardGridSelectedRowDataItem.BookDateDelivered; } 
            if ("BookRouteConsFlag" in loadBoardGridSelectedRowDataItem){ routeConsFlag = loadBoardGridSelectedRowDataItem.BookRouteConsFlag; } 
            if ("NEXTStop" in loadBoardGridSelectedRowDataItem){ blnNEXTStop = loadBoardGridSelectedRowDataItem.NEXTStop; } 
            if ("DAT" in loadBoardGridSelectedRowDataItem){ blnDAT = loadBoardGridSelectedRowDataItem.DAT; } 
            var blnCanSingle = false;
            var dtNow = Date.now();
            if(dtDelivered){ if(dtDelivered < dtNow && routeConsFlag == 0){ blnCanSingle = true; } }
            if (bkControl == 0){ return; } //make everything invisible
            if (typeof (tranCode) === 'undefined' || tranCode == null || tranCode.length < 1){ return; } //make everything invisible
            if (!dtLoad){ return; } //make everything invisible
            if (!dtOrdered){ return; } //make everything invisible
            if (!dtRequired){ return; } //make everything invisible
            //Select Case BookTranCode
            if (tranCode.toUpperCase() === "N"){                
               actions.push("ynRemoveOrder");
            }
            else if(tranCode.toUpperCase() === "P") {               
               if (carrierControl > 0) {
                   actions.push("ynUnassignProvider");                      
                   if (lockAllCosts === false){ if(ngl.stringHasValue(shid)){ actions.push("ynReject"); } } //they have to reject the load if an shid is already assigned so the system will create a new shid for the new carrier. 
                   if (blnNEXTStop === false && blnDAT === false) {
                       actions.push("ynRemoveOrder");
                       actions.push("ynTender");
                       actions.push("ynAccept");
                       actions.push("ynInvoice");
                       if (blnCanSingle === true){ actions.push("ynInvoiceSingle"); } 
                   }
               }   
            }
            else if(tranCode.toUpperCase() === "PC") {               
               if (carrierControl > 0) {
                   actions.push("ynReject");                   
                   if (blnNEXTStop === false && blnDAT === false) { 
                       actions.push("ynModify"); 
                       actions.push("ynAccept");
                       actions.push("ynInvoice");
                       if (blnCanSingle === true) { actions.push("ynInvoiceSingle"); }  
                   }                 
               }
            }
            else if(tranCode.toUpperCase() === "PB") {               
               if (carrierControl > 0) {
                   actions.push("ynDrop");
                   if (blnNEXTStop === false && blnDAT === false) {
                       actions.push("ynModify");
                       actions.push("ynInvoice");                 
                       if (blnCanSingle === true) { actions.push("ynInvoiceSingle"); }
                   }                 
               }
            }
            else if(tranCode.toUpperCase() === "I") {              
               if (carrierControl > 0) {
                   actions.push("ynReject");
                   actions.push("ynModify");                    
                   if (ngl.stringHasValue(dtInvoice) === true) {
                       actions.push("ynInvoiceComplete");                                      
                       if (blnCanSingle === true) { actions.push("ynInvoiceCompleteSingle"); }                   
                   }                   
               }
            }
            else if(tranCode.toUpperCase() === "IC") {                  
               if (carrierControl > 0) {
                   actions.push("ynReject");
                   actions.push("ynModify");
                   actions.push("ynInvoice");                   
                   if (blnCanSingle === true) { actions.push("ynInvoiceSingle"); }                  
               }         
            }
            return actions
            }
            
            // returns an integer 
            // -1 indicates that the booing is not selected or is not valid 
            //    a message is already displayed so the caller should just return
            //  0 indicates that the booking options dialog is required
            //  1 indicates success and to show the Rate It Options Dialog
            function readyToAssignCarrier(){   
            //Get the dataItem for the selected row
            var iRet = 1;
            if (typeof (loadBoardGridSelectedRowDataItem) === 'undefined' || loadBoardGridSelectedRowDataItem == null) { ngl.showValidationMsg("Booking Record Required", "Please select a Booking to continue", tPage); return -1; }             
            var bkControl = 0, carrierControl = 0, routeConsFlag = 0;
            var tranCode = "", dtOrdered = "", dtRequired = "", dtLoad = "", dtInvoice = "", dtDelivered = "", shid = "";
            var lockAllCosts = true;
            var blnOnCreditHold = false; //Added By LVV on 7/14/20 for v-8.3.0.001 Task #20200609155542 - Credit Hold
            if ("BookControl" in loadBoardGridSelectedRowDataItem){ bkControl = loadBoardGridSelectedRowDataItem.BookControl; } 
            if ("BookTranCode" in loadBoardGridSelectedRowDataItem){ tranCode = loadBoardGridSelectedRowDataItem.BookTranCode; } 
            if ("BookDateOrdered" in loadBoardGridSelectedRowDataItem){ dtOrdered = loadBoardGridSelectedRowDataItem.BookDateOrdered; } 
            if ("BookDateRequired" in loadBoardGridSelectedRowDataItem){ dtRequired = loadBoardGridSelectedRowDataItem.BookDateRequired; } 
            if ("BookDateLoad" in loadBoardGridSelectedRowDataItem){ dtLoad = loadBoardGridSelectedRowDataItem.BookDateLoad; } 
            if ("BookFinARInvoiceDate" in loadBoardGridSelectedRowDataItem){ dtInvoice = loadBoardGridSelectedRowDataItem.BookFinARInvoiceDate; } 
            if ("BookCarrierControl" in loadBoardGridSelectedRowDataItem){ carrierControl = loadBoardGridSelectedRowDataItem.BookCarrierControl; } 
            if ("BookLockAllCosts" in loadBoardGridSelectedRowDataItem){ lockAllCosts = loadBoardGridSelectedRowDataItem.BookLockAllCosts; } 
            if ("BookSHID" in loadBoardGridSelectedRowDataItem){ shid = loadBoardGridSelectedRowDataItem.BookSHID; } 
            if ("BookDateDelivered" in loadBoardGridSelectedRowDataItem){ dtDelivered = loadBoardGridSelectedRowDataItem.BookDateDelivered; } 
            if ("BookRouteConsFlag" in loadBoardGridSelectedRowDataItem){ routeConsFlag = loadBoardGridSelectedRowDataItem.BookRouteConsFlag; }
            if ("OnCreditHold" in loadBoardGridSelectedRowDataItem){ blnOnCreditHold = loadBoardGridSelectedRowDataItem.OnCreditHold; } //Added By LVV on 7/14/20 for v-8.3.0.001 Task #20200609155542 - Credit Hold
            if(blnOnCreditHold){ ngl.showValidationMsg("Credit Hold Warning", "Cannot Rate a load that is on Credit Hold", tPage); return -1; } //Added By LVV on 7/14/20 for v-8.3.0.001 Task #20200609155542 - Credit Hold
               
            var dtNow = Date.now();
            var sMessage = "Because ";
            var sSpacer = "";
            if (dtDelivered){ 
               if(dtDelivered < dtNow && routeConsFlag == 0){ iRet = -1; sMessage = sMessage +   "the selected Booking has already been delivered"; sSpacer = " , and "; } 
            }
            if (bkControl == 0){ iRet = -1; sMessage = sMessage +  sSpacer +  "the selected Booking record has not been saved"; sSpacer = " , and "; } 
            if (typeof (tranCode) === 'undefined' || tranCode == null || tranCode.length < 1){ iRet = -1; sMessage = sMessage +  sSpacer +  "the tran code is not valid"; sSpacer = " , and "; } 
            if (!dtLoad){ iRet = -1; sMessage = sMessage +  sSpacer +  "the load date is not valid"; sSpacer = " , and "; } 
            if (!dtOrdered){ iRet = -1; sMessage = sMessage +  sSpacer +  "the order date is not valid"; sSpacer = " , and "; } 
            if (!dtRequired){ iRet = -1; sMessage = sMessage +  sSpacer +  "the required date is not valid"; sSpacer = " , and "; }            
            if (iRet === -1) { ngl.showValidationMsg("Cannot Change Carrier", sMessage, tPage); return  iRet; }          
            //Select Case BookTranCode
            if(tranCode.toUpperCase() === "N" || tranCode.toUpperCase() === "P"){ return 1; } else{ return 0; }                     
            }
            
            function BookingTenderWndCB(oResults){          
            if(oResults.widget.sNGLCtrlName === "wdgtBookingTenderWndDialog" && oResults.source === "saveSuccessCallback"){
               var iThisBookControl = iBookPK;
               //oLoadBoardGrid.dataSource.read();
               //modified by RHR on 2/27/2019 for v-8.2  to be consistent with other functionality
               //just in case we add more data or logic to refresh option.
               execActionClick("Refresh");
               oResults.widget.executeActions("close");
            
               //Refreshes the truck to show updated list of orders after removing an order
               refreshTruck(iTruckCompControl,iTruckPK);
            
               //Refreshes New Loads grid
               $('#NewLoadsgrid').data('kendoNGLGrid').dataSource.read();
               $('#NewLoadsgrid').data('kendoNGLGrid').refresh();
               //Refreshes Import Queue grid
               $('#ImportQueue').data('kendoNGLGrid').dataSource.read();
               $('#ImportQueue').data('kendoNGLGrid').refresh();
            
               //Invoked to refresh the loads in truck and display updated stop/sequence number
               //var ds = $("#NewLoadsgrid").data().kendoGrid.dataSource;
            
               //var s = objid39111201806290836427404336Filters.data;
            
               //s.page = options.data.page;
               //s.skip = options.data.skip;
               //s.take = options.data.take;
               ////s.PageSize = 10;
            
               //$.ajax({ 
               //    url: 'api/LoadPlanning/GetRecordsByNewOrdersFilter/', 
               //    contentType: 'application/json; charset=utf-8', 
               //    dataType: 'json', 
               //    data: { filter: JSON.stringify(s),PageStatus:JSON.stringify(PageStatus) }, 
               //    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
               //    success: function(data) { 
               //        options.success(data);
               //        //console.log(data);
               //        PageStatus=1; 
               //        $("#NLcnt").text(data.Data.length);
               //        TotLcnt = data.Data.length;
               //        if (data.Errors != null) { 
            
               //            if (data.StatusCode === 203){ 
               //                ngl.showErrMsg("Authorization Timeout", data.Errors, null); 
               //            } 
               //            else 
               //            { 
               //                ngl.showErrMsg("Access Denied", data.Errors, null); 
               //            } 
               //        } else {
               //            ds.data(data.Data);
               //            ds.sync();
               //        }
               //    }, 
            
               //    error: function(result) { 
               //        options.error(result); 
               //    } 
               //});
            
               if (blnShowRateItAfterBookingOptions === true) {
                   blnShowRateItAfterBookingOptions = false;
                   ngl.showValidationMsg("Retry Required ", "Please select a Booking record and try to rate the load again", tPage);
               }
               //if (blnShowRateItAfterBookingOptions === true) {
               //    blnShowRateItAfterBookingOptions = false;
               //    setTimeout(function (iThisBookControl) {
               //        //we are in the middle of selecting a new carrier so hopefully we can continue 
               //        //now and it will work. first try to select the previous booking record
               //        var oGrid1 = $('#id621201809251302564013007')  //.kendoNGLGrid tPage.oLoadBoardGrid;
               //        var oGrid = tPage.oLoadBoardGrid;
               //        //var data = oGrid.data("kendoNGLGrid");
               //        var oRow6 = tPage.oLoadBoardGrid.table.find('tr[data-id="' + iThisBookControl + '"]')
               //        if ("BookControl" in oRow6) {
               //            //tPage.loadBoardGridSelectedRowDataItem = oRow1;
               //            var iiiBookPK = oRow6.BookControl;
               //        }
               //        var oRow3 = $('#id621201809251302564013007').data("kendoNGLGrid").table.find('tr[data-id="' + iThisBookControl + '"]')
               //        var oRow4 = data.table.find('tr[data-id="' + iThisBookControl + '"]')
               //        var oRow5 = oGrid.data("kendoNGLGrid").table.find('tr[data-id="' + iThisBookControl + '"]')
               //        var data1 = oGrid1.data("kendoNGLGrid");
               //        var oRow2 = data1.table.find('tr[data-id="' + iThisBookControl + '"]')
               //        var oRow1 = $('#id621201809251302564013007').data("kendoNGLGrid").table.find('tr[data-id="' + iThisBookControl + '"]')
               //        if ("BookControl" in oRow1) {
               //            tPage.loadBoardGridSelectedRowDataItem = oRow1;
               //            tPage.iBookPK = oRow1.BookControl;
               //            var setting = { name: 'pk', value: iBookPK.toString() };
               //            var oCRUDCtrl = new nglRESTCRUDCtrl();
               //            var blnRet = oCRUDCtrl.update("LoadBoard/PostPageSetting", setting, tPage, "savePostPageSettingSuccessCallback", "savePostPageSettingAjaxErrorCallback", tPage);
               //            setTimeout(function () {
               //                tPage.execActionClick("GetRates");
               //            }, 1000);
               //        }
               //        return;
               //        if (typeof (oGrid) !== 'undefined' && ngl.isObject(oGrid)) {
               //            var oRow = oGrid.table.find('tr[data-id="' + iThisBookControl + '"]');
               //            if (typeof (oRow) !== 'undefined' && ngl.isObject(oRow)) {
               //                oGrid.select(oRow);
               //                //tPage.saveBookPK();
               //                tPage.loadBoardGridSelectedRowDataItem = oRow;// oLoadBoardGrid.dataItem(loadBoardGridSelectedRow);
               //                if (typeof (tPage.loadBoardGridSelectedRowDataItem) === 'undefined' || tPage.loadBoardGridSelectedRowDataItem == null) {
               //                    ngl.showValidationMsg("Cannot Auto Select the Previous Booking Record ", "Please select a Booking to continue", tPage);
               //                    return false;
               //                }
               //                if ("BookControl" in tPage.loadBoardGridSelectedRowDataItem) {
               //                    tPage.iBookPK = tPage.loadBoardGridSelectedRowDataItem.BookControl;
               //                    var setting = { name: 'pk', value: iBookPK.toString() };
               //                    var oCRUDCtrl = new nglRESTCRUDCtrl();
               //                    var blnRet = oCRUDCtrl.update("LoadBoard/PostPageSetting", setting, tPage, "savePostPageSettingSuccessCallback", "savePostPageSettingAjaxErrorCallback", tPage);
               //                } else {
               //                    ngl.showValidationMsg("Cannot Auto Select the Previous Booking Record ", "Please select a Booking to continue", tPage);
               //                    return false;
               //                }
               //                setTimeout(function () {
               //                    tPage.execActionClick("GetRates");
               //                }, 1000);
               //            }
               //        }
               //    }, 1000, iThisBookControl);
               //}
            }          
            }
            
            // start widgit configuration
            var winDispatchingDialog = kendo.ui.Window;
            var winDispatchReport = kendo.ui.Window;
            var winBOLReport  = kendo.ui.Window;
            var oDispatchingDialogCtrl = new DispatchingDialogCtrl();     
            var oDispatchReportCtrl = new DispatchingReportCtrl();
            var oBOLReportCtrl = new BOLReportCtrl();
            var oDispatchData = new Dispatch();
            var iSelectedLoadTenderControl
            //create widgit call backs
            function oDispatchingDialogSelectCB(results){  
            var data = new Dispatch();
            if (typeof (oDispatchingDialogCtrl)  === 'undefined' || oDispatchingDialogCtrl == null  ) { return;}
            if (typeof (oDispatchingDialogCtrl.data) === 'undefined' ||  oDispatchingDialogCtrl.data == null ) { return;}
            try{
               data = oDispatchingDialogCtrl.data;
               //if (blnDispatchedFromPageQuote == true) {
               //    $("#txtorigCompName").data("kendoMaskedTextBox").value(data.Origin.Name);
               //    $("#txtorigCompAddress1").data("kendoMaskedTextBox").value(data.Origin.Address1);
               //    $("#txtOrigPhone").val(data.Origin.Contact.ContactPhone);
               //    $("#txtOrigContactName").val(data.Origin.Contact.ContactName);
               //    $("#txtOrigContactEmail").val(data.Origin.Contact.ContactEmail);                      
               //    $("#txtdestCompName").data("kendoMaskedTextBox").value(data.Destination.Name);
               //    $("#txtdestCompAddress1").data("kendoMaskedTextBox").value(data.Destination.Address1);
               //    $("#txtDestPhone").val(data.Destination.Contact.ContactPhone);
               //    $("#txtDestContactName").val(data.Destination.Contact.ContactName);
               //    $("#txtDestContactEmail").val(data.Destination.Contact.ContactEmail);
               //} 
            } catch (err) {
               //do nothing
            }        
            }
            function oDispatchingDialogSaveCB(results){
            var data = new Dispatch();
            if (typeof (results)  === 'undefined' || results == null  ) { return;}
            if (typeof (results.data) === 'undefined' ||  results.data == null || ngl.isArray(results.data) == false) { return;}
            try{
               data = results.data[0];           
               //if (blnDispatchedFromPageQuote == true) {
               //    $("#txtorigCompAddress1").val(data.Origin.Address1);
               //    $("#txtOrigPhone").val(data.Origin.Contact.ContactPhone);
               //    $("#txtOrigContactName").val(data.Origin.Contact.ContactName);
               //    $("#txtOrigContactEmail").val(data.Origin.Contact.ContactEmail);
               //    $("#txtdestCompAddress1").val(data.Destination.Address1);
               //    $("#txtDestPhone").val(data.Destination.Contact.ContactPhone);
               //    $("#txtDestContactName").val(data.Destination.Contact.ContactName);
               //    $("#txtDestContactEmail").val(data.Destination.Contact.ContactEmail);
               //}
               oDispatchReportCtrl.data = results.data;
               oDispatchReportCtrl.show();
            } catch (err) {
               //do nothing
            }
            }
            function oDispatchingDialogCloseCB(results){ 
            var data = new Dispatch();
            if (typeof (oDispatchingDialogCtrl)  === 'undefined' || oDispatchingDialogCtrl == null  ) { return;}
            if (typeof (oDispatchingDialogCtrl.data) === 'undefined' ||  oDispatchingDialogCtrl.data == null ) { return;}
            try{
               data = oDispatchingDialogCtrl.data;         
               //if (blnDispatchedFromPageQuote == true) {
               //    $("#txtOrigPhone").val(data.Origin.Contact.ContactPhone);
               //    $("#txtOrigContactName").val(data.Origin.Contact.ContactName);
               //    $("#txtOrigContactEmail").val(data.Origin.Contact.ContactEmail);
               //    $("#txtDestPhone").val(data.Destination.Contact.ContactPhone);
               //    $("#txtDestContactName").val(data.Destination.Contact.ContactName);
               //    $("#txtDestContactEmail").val(data.Destination.Contact.ContactEmail);
               //}
            } catch (err) {
               //do nothing
            }
            }
            function oDispatchingDialogReadCB(results){ 
            var data = new Dispatch();
            if (typeof (oDispatchingDialogCtrl)  === 'undefined' || oDispatchingDialogCtrl == null  ) { return;}
            if (typeof (oDispatchingDialogCtrl.data) === 'undefined' ||  oDispatchingDialogCtrl.data == null ) { return;}
            try{
               data = oDispatchingDialogCtrl.data;            
               //if (blnDispatchedFromPageQuote == true) {
               //    $("#txtOrigPhone").val(data.Origin.Contact.ContactPhone);
               //    $("#txtOrigContactName").val(data.Origin.Contact.ContactName);
               //    $("#txtOrigContactEmail").val(data.Origin.Contact.ContactEmail);
               //    $("#txtDestPhone").val(data.Destination.Contact.ContactPhone);
               //    $("#txtDestContactName").val(data.Destination.Contact.ContactName);
               //    $("#txtDestContactEmail").val(data.Destination.Contact.ContactEmail); 
               //}
            } catch (err) {
               //do nothing
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
               if (typeof (results.data) !== 'undefined' && ngl.isObject(results.data) && typeof (results.data.ErrorCode) !== 'undefined' &&  results.data.ErrorCode !== null ){
                  
                   if(!ngl.isNullOrWhitespace(results.data.ErrorCode) && !isNaN( results.data.ErrorCode)){
                       //get the bid type
                       if ( typeof (results.data.DispatchBidType) !== 'undefined' &&  results.data.DispatchBidType !== null && !ngl.isNullOrWhitespace(results.data.DispatchBidType) && !isNaN( results.data.DispatchBidType)){
                           //possible bid types
                           //1	NextStop
                           //2	NGLTar
                           //3	P44
                           //4	Spot
                           if (results.data.DispatchBidType.toString() == "2" || results.data.DispatchBidType.toString() == "3"){
                               //if we havea  loadTenderControl reopen the quote window so user can select a new quote
                               if ( typeof (results.data.LoadTenderControl) !== 'undefined' &&  results.data.LoadTenderControl !== null && !ngl.isNullOrWhitespace(results.data.LoadTenderControl) && !isNaN( results.data.LoadTenderControl)){
                                   if (typeof (tPage["wdgtQuotesWndDialog"]) !== 'undefined' && ngl.isObject(tPage["wdgtQuotesWndDialog"])){
                                       sBidLoadTenderControlVal = results.data.LoadTenderControl;
                                       tPage["wdgtQuotesWndDialog"].read(results.data.LoadTenderControl);
                                   } 
                               }
                               return;
                           } else {
                               //just refresh the data and exit
                               execActionClick("Refresh");
                               return;
                           }
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
            
            //End widgit configuration
            
             var oQuoteSelectedBid = null;
             function dispatchSelectedQuote(e) {
                 //debugger;
                 //alert('do dispatch')
                 oQuoteSelectedBid = this.dataItem($(e.currentTarget).closest("tr"));
                 if (typeof (oQuoteSelectedBid) !== 'undefined' && ngl.isObject(oQuoteSelectedBid)) {
                     oDispatchingDialogCtrl.read(oQuoteSelectedBid.BidControl)
                     if (typeof (tPage["wdgtQuotesWndDialog"]) !== 'undefined' && ngl.isObject(tPage["wdgtQuotesWndDialog"])) {
                         tPage["wdgtQuotesWndDialog"].executeActions("close");
                     }
                     //ngl.OkCancelConfirmation(
                     //       "Replace Load Information With Selected",
                     //       "This action will replace the current load information with the select address and item details",
                     //       400,
                     //       400,
                     //       null,
                     //       "ConfirmCopyHistoricalQuote");
                 };
             }

             // Begin Modified by RHR for v-8.5.3.007 on 06/22/2023 added Select/Assign carrier option
             function assignSelectedQuote(e) {
                 oQuoteSelectedBid = this.dataItem($(e.currentTarget).closest("tr"));
                 if (typeof (oQuoteSelectedBid) !== 'undefined' && ngl.isObject(oQuoteSelectedBid)) {
                     intControl = oQuoteSelectedBid.BidControl
                     if (typeof (tPage["wdgtQuotesWndDialog"]) !== 'undefined' && ngl.isObject(tPage["wdgtQuotesWndDialog"])) {
                         tPage["wdgtQuotesWndDialog"].executeActions("close");
                     }
                     kendo.ui.progress($("#center-pane"), true);
                     if (typeof (intControl) != 'undefined' && intControl != null) {
                         var oCRUDCtrl = new nglRESTCRUDCtrl();
                         blnRet = oCRUDCtrl.read("Dispatching/AssignBid", intControl, tObj, "assignBidSuccessCallback", "assignBidAjaxErrorCallback");
                     }
                 };
             }

             function assignBidSuccessCallback(data) {
                 var oResults = new nglEventParameters();
                 kendo.ui.progress($("#center-pane"), false);
                 oResults.source = "assignBidSuccessCallback";
                 oResults.widget = tObj;
                 oResults.msg = 'Failed'; //set default to Failed  
                 try {
                     if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                         if (typeof (data.Errors) !== 'undefined' && data.Errors != null) {
                             oResults.error = new Error();
                             oResults.error.name = "Assign Carrier Data Failure";
                             oResults.error.message = data.Errors;
                             ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                         }
                         else {

                             if (data.Data != null) {
                                 oResults.data = data.Data;
                                 oResults.datatype = "Assign Carrier";
                                 oResults.msg = "Success"
                                 //this.data = data.Data[0];
                                 //this.show();
                             }
                             else {
                                 oResults.error = new Error();
                                 oResults.error.name = "Invalid Request";
                                 oResults.error.message = "No Data available.";
                                 ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                             }
                         }
                     } else {
                         oResults.msg = "Success but no data was returned. Please refresh your page and try again.";
                         ngl.showSuccessMsg(oResults.msg, tObj);
                     }
                 } catch (err) {
                     oResults.error = err
                 }
                 execActionClick("Refresh");

             }

             function assignBidAjaxErrorCallback(xhr, textStatus, error) {
                 //var windowWidget = $("#winDispatchDialog").data("kendoWindow");
                 //kendo.ui.progress(windowWidget.element, false);
                 var oResults = new nglEventParameters();
                 kendo.ui.progress($("#center-pane"), false);
                 oResults.source = "assignBidAjaxErrorCallback";
                 oResults.widget = tObj;
                 oResults.msg = 'Failed'; //set default to Failed  
                 oResults.error = new Error();
                 oResults.error.name = "Assign Carrier Data Failure"
                 oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
                 ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                 execActionClick("Refresh");
             }
            // End Modified by RHR for v-8.5.3.007 on 06/22/2023 added Select/Assign carrier option
            
            function PrintSelectedBOL(){
            if (typeof (iBookPK) !== 'undefined' || iBookPK !== null || iBookPK !== 0) {
               if (typeof (oBOLReportCtrl) !== 'undefined' && ngl.isObject(oBOLReportCtrl)) { oBOLReportCtrl.readByBookControl(iBookPK); }
            }                         
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
            
            function openLoadTenderReport(){ ngl.showInfoNotification("Feature Unavailable", "Load Tender Report coming soon", null); }
            
            
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
            
            function AutoMilesSuccessCallback(data) { coreSuccessCallBack(data, "Auto Miles"); }
            function AutoMilesAjaxErrorCallback(xhr, textStatus, error) { coreErrorCallBack(xhr, textStatus, error, "Auto Miles Failure"); }
            
            function AutoMiles(iBookPK, btnId) {
                if (blnAutoMiles == false) {
                    blnAutoMiles = true;
                    if (blnAutoStopResequence == true) {
                        blnAutoStopResequence = false;
                    }
                } else {
                    blnAutoMiles = false;
                }
                updateLBStampMsg();
            }
            function AutoStopSuccessCallback(data) { coreSuccessCallBack(data, "Auto Stop"); }
            function AutoStopAjaxErrorCallback(xhr, textStatus, error) { coreErrorCallBack(xhr, textStatus, error, "Auto Stop Failure"); }
            function AutoStop(iBookPK, btnId) {
                if (blnAutoStopResequence == false) {
                    blnAutoStopResequence = true;
                    if (blnAutoMiles == true) {
                        blnAutoMiles = false;
                    }
                } else {
                    blnAutoStopResequence = false;
                }
                updateLBStampMsg();
            }
            
            function AutoRecalculateSuccessCallback(data) { coreSuccessCallBack(data, "Auto Recalculate"); }
            function AutoRecalculateAjaxErrorCallback(xhr, textStatus, error) { coreErrorCallBack(xhr, textStatus, error, "Auto Recalculate Failure"); }
            function AutoRecalculate(iBookPK, btnId) {
                if (blnAutoCalculate == false) {
                    blnAutoCalculate = true;
                } else {
                    blnAutoCalculate = false;
                }
                updateLBStampMsg();
            
                //$("#LBStampMsg").html("Auto Recalculate - Btn ID: " + btnId);
                //$("#LBStampCNS").show();
            }
            
            //var blnAuto
            function updateLBStampMsg() {
                var sLbSStampMsg = "";
                var blnShowStampMsg = false;
                var sSpacer = "";
                if (sStampingCNS.length > 0) {
                    sLbSStampMsg = "Stamping Active for CNS # " + sStampingCNS;
                    sSpacer = " | ";
                    blnShowStampMsg = true;
                }
                if (blnAutoCalculate == true) {
                    sLbSStampMsg = sLbSStampMsg + sSpacer + " Auto Calculate Active ";
                    sSpacer = " | ";
                    blnShowStampMsg = true;
                }
                if (blnAutoMiles == true) {
                    sLbSStampMsg = sLbSStampMsg + sSpacer + " Auto Miles Active ";
                    sSpacer = " | ";
                    blnShowStampMsg = true;
                }
                if (blnAutoStopResequence == true) {
                    sLbSStampMsg = sLbSStampMsg + sSpacer + " Auto Stop Resync Active ";
                    sSpacer = " | ";
                    blnShowStampMsg = true;
                }
            
                if (blnShowStampMsg == true) {
                    $("#LBStampMsg").html(sLbSStampMsg);
                    $("#LBStampCNS").show();
                } else {
                    $("#LBStampMsg").html("");
                    $("#LBStampCNS").hide();
                }
            }

             function coreErrorCallBack(xhr, textStatus, error, errTitle) {
                 //debugger;
                 kendo.ui.progress(oLoadBoardGrid.element, false);
                 ngl.showErrMsg(errTitle, formatAjaxJSONResponsMsgs(xhr, textStatus, error, 'Failed'), tPage);
             }

             function coreSuccessCallBack(data, errTitle, refresh) {
                 try {
                     //debugger;
                     refresh = typeof refresh !== 'undefined' ? refresh : true;
                     kendo.ui.progress(oLoadBoardGrid.element, false);
                     var blnSuccess = false;
                     var blnErrorShown = false;
                     var strValidationMsg = "";
                     var blnShowErrWarnDialog = true;
                     if (typeof (data) !== 'undefined' && ngl.isObject(data)) {

                         if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg(errTitle, data.Errors, null); }
                         else {
                             if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                 if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                     blnSuccess = true;
                                     // Modified by RHR for v-8.5.3.006 on 01/04/2023 added debug code in console
                                     //console.log("Check Success!");
                                     //console.log(data.Data[0].Success);
                                     if (data.Data[0].Success === true) {
                                         blnShowErrWarnDialog = false;
                                         if (ngl.stringHasValue(data.Data[0].SuccessMsg)) { ngl.showSuccessMsg(data.Data[0].SuccessMsg, null); }
                                     }
                                     else {
                                         //console.log("Check ErrMsg!");
                                         //console.log(data.Data[0].Success);
                                         if (ngl.stringHasValue(data.Data[0].ErrMsg)) {
                                             /*console.log(data.Data[0].ErrMsg);*/
                                             ngl.showErrMsg(errTitle, data.Data[0].ErrMsg, null);
                                         }
                                         //else {
                                         //    console.log("No ErrMsg!");
                                         //}
                                     }
                                     /*console.log("Check WarningMsg!");*/
                                     if (ngl.stringHasValue(data.Data[0].WarningMsg)) {
                                         /*console.log(data.Data[0].WarningMsg);*/
                                         ngl.showWarningMsg("", data.Data[0].WarningMsg, null);
                                     }
                                     //else {
                                     //    console.log("No WarningMsg!");
                                     //}

                                 }
                             }
                         }
                         if (typeof (data.Log) !== 'undefined' && blnShowErrWarnDialog == true) {
                             //errWarnData = data;
                             //openNGLErrWarnMsgLogCtrlDialog();  
                             if (data.Messages) { ngl.showWarningMsg("Details", data.Messages, tPage, 20000); }

                         }
                     }
                     if (blnSuccess === false && blnErrorShown === false) {
                         if (strValidationMsg.length < 1) { strValidationMsg = errTitle; }
                         ngl.showErrMsg(errTitle, strValidationMsg, null);
                     }
                     if (refresh == true) {
                         execActionClick("Refresh");
                     }

                 } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
             }


            var blnShowRateItAfterBookingOptions = false;
            
            
            function execActionClick(btn, proc) {
                // added navigation logic by Kanna
                if (btn.id == "btnOpenCompany") {
                    if (isBookingSelected() === true) {
                        if ("BookCustCompControl" in loadBoardGridSelectedRowDataItem) {
                            var compPK = loadBoardGridSelectedRowDataItem.BookCustCompControl;
                            var setting = { name: 'pk', value: compPK.toString() };
                            var oCRUDCtrl = new nglRESTCRUDCtrl();
                            var blnRet = oCRUDCtrl.update("LECompMaint/PostPageSetting", setting, tPage, "savePostPageSettingSuccessCallback", "savePostPageSettingAjaxErrorCallback", tPage);
                            location.href = "LECompMaint?CompNumber=" + compPK;
                        }
                      
                    }
                }
                else if (btn.id == "btnOpenLane") {
                    if (isBookingSelected() === true) {
                        if ("BookODControl" in loadBoardGridSelectedRowDataItem) {
                            var lanePK = loadBoardGridSelectedRowDataItem.BookODControl;
                            var setting = { name: 'pk', value: lanePK.toString() };
                            var oCRUDCtrl = new nglRESTCRUDCtrl();
                            var blnRet = oCRUDCtrl.update("LaneDetail/PostPageSetting", setting, tPage, "savePostPageSettingSuccessCallback", "savePostPageSettingAjaxErrorCallback", tPage);
                        }
                        location.href = "LaneDetail";
                    }
                } else if (btn.id == "btnOpenCarrier") {
                    if (isBookingSelected() === true) {
                        if ("BookCarrierControl" in loadBoardGridSelectedRowDataItem) {
                            location.href = "LECarrierMaint?carrControl=" + loadBoardGridSelectedRowDataItem.BookCarrierControl;
                        }
                    }
                }
                else if (btn.id == "btnOpenItem") {
                    if (isBookingSelected() === true) {
                        if ("BookControl" in loadBoardGridSelectedRowDataItem) {
                            var setting = { name: 'pk', value: iBookPK.toString() };
                            var oCRUDCtrl = new nglRESTCRUDCtrl();
                            var blnRet = oCRUDCtrl.update("LoadBoard/PostPageSetting", setting, tPage, "savePostPageSettingSuccessCallback", "savePostPageSettingAjaxErrorCallback", tPage);
                        }
                        location.href = "LoadBoardItems";
                    }
                }
                else if (btn.id == "btnOpenRevFees") {
                    if (isBookingSelected() === true) {
                        if ("BookControl" in loadBoardGridSelectedRowDataItem) {
                            var setting = { name: 'pk', value: iBookPK.toString() };
                            var oCRUDCtrl = new nglRESTCRUDCtrl();
                            var blnRet = oCRUDCtrl.update("LoadBoard/PostPageSetting", setting, tPage, "savePostPageSettingSuccessCallback", "savePostPageSettingAjaxErrorCallback", tPage);
                        }
                        location.href = "LoadBoardRevenueFees";
                    }
                }
                else if (btn.id == "btnOpenRevenue") {
                    if (isBookingSelected() === true) {
                        if ("BookControl" in loadBoardGridSelectedRowDataItem) {
                            var setting = { name: 'pk', value: iBookPK.toString() };
                            var oCRUDCtrl = new nglRESTCRUDCtrl();
                            var blnRet = oCRUDCtrl.update("LoadBoard/PostPageSetting", setting, tPage, "savePostPageSettingSuccessCallback", "savePostPageSettingAjaxErrorCallback", tPage);
                        }
                        location.href = "LoadBoardRevenue";
                    }
                }
                else if (btn.id == "btnOpenLoadRevenue") {
                    if (isBookingSelected() === true) {
                        if ("BookControl" in loadBoardGridSelectedRowDataItem) {
                            var setting = { name: 'pk', value: iBookPK.toString() };
                            var oCRUDCtrl = new nglRESTCRUDCtrl();
                            var blnRet = oCRUDCtrl.update("LoadBoard/PostPageSetting", setting, tPage, "savePostPageSettingSuccessCallback", "savePostPageSettingAjaxErrorCallback", tPage);
                        }
                        location.href = "LoadBoardRevenue";
                    }
                }
                else if (btn.id == "btnOpenLoadStatus") {
                    if (isBookingSelected() === true) {
                        if ("BookControl" in loadBoardGridSelectedRowDataItem) {
                            var setting = { name: 'pk', value: iBookPK.toString() };
                            var oCRUDCtrl = new nglRESTCRUDCtrl();
                            var blnRet = oCRUDCtrl.update("LoadBoard/PostPageSetting", setting, tPage, "savePostPageSettingSuccessCallback", "savePostPageSettingAjaxErrorCallback", tPage);
                        }
                        location.href = "LoadBoardLoadStatus";
                    }
                }
                else if (btn.id == "btnOpenLoadNotes") {
                    if (isBookingSelected() === true) {
                        if ("BookControl" in loadBoardGridSelectedRowDataItem) {
                            var setting = { name: 'pk', value: iBookPK.toString() };
                            var oCRUDCtrl = new nglRESTCRUDCtrl();
                            var blnRet = oCRUDCtrl.update("LoadBoard/PostPageSetting", setting, tPage, "savePostPageSettingSuccessCallback", "savePostPageSettingAjaxErrorCallback", tPage);
                        }
                        location.href = "LoadBoardNotes";
                    }
                }
                else if (btn.id == "btnOpenLoadCarData") {
                    if (isBookingSelected() === true) {
                        if ("BookControl" in loadBoardGridSelectedRowDataItem) {
                            var setting = { name: 'pk', value: iBookPK.toString() };
                            var oCRUDCtrl = new nglRESTCRUDCtrl();
                            var blnRet = oCRUDCtrl.update("LoadBoard/PostPageSetting", setting, tPage, "savePostPageSettingSuccessCallback", "savePostPageSettingAjaxErrorCallback", tPage);
                        }
                        location.href = "LoadBoardCarrierData";
                    }
                }
                else if (btn.id == "btnOpenLoadBoard") {
                    if (isBookingSelected() === true) {
                        if ("BookControl" in loadBoardGridSelectedRowDataItem) {
                            var setting = { name: 'pk', value: iBookPK.toString() };
                            var oCRUDCtrl = new nglRESTCRUDCtrl();
                            var blnRet = oCRUDCtrl.update("LoadBoard/PostPageSetting", setting, tPage, "savePostPageSettingSuccessCallback", "savePostPageSettingAjaxErrorCallback", tPage);
                            
                            location.href = "LoadBoard?BookConsPrefix=" + loadBoardGridSelectedRowDataItem.BookConsPrefix;
                        }
                      
                    }
                }
                else if (btn.id == "btnOpenTariff") { 
                    if (isBookingSelected() === true) { 
                        if ("BookCarrTarControl" in loadBoardGridSelectedRowDataItem) {
                            if (loadBoardGridSelectedRowDataItem.BookCarrTarControl == 0) {
                                ngl.showWarningMsg("Tariff", "A tariff is not assigned to this load ", null);
                                return false;
                            }
                            location.href = "Tariff?tarcontrol=" + loadBoardGridSelectedRowDataItem.BookCarrTarControl;
                        }
                    } 
                }
                else if (btn.id == "btnRoutingGuide") {
                    if (isBookingSelected() === true) {
                        if ("RouteGuideControl" in loadBoardGridSelectedRowDataItem) {
                            var RouteGuideControlPK = loadBoardGridSelectedRowDataItem.RouteGuideControl;
                            var setting = { name: 'pk', value: RouteGuideControlPK.toString() };
                            var oCRUDCtrl = new nglRESTCRUDCtrl();
                            var blnRet = oCRUDCtrl.update("RouteGuide/PostPageSetting", setting, tPage, "savePostPageSettingSuccessCallback", "savePostPageSettingAjaxErrorCallback", tPage);
                            location.href = "RouteGuide";
                        }
                      
                    }
                }
                else if (btn.id == "btnCarrierPro") {
                    if (isBookingSelected() === true) {
                        if ("CarrierNumber" in loadBoardGridSelectedRowDataItem) {
                            var CarrierNumber = loadBoardGridSelectedRowDataItem.RouteGuideControl;
                            var setting = { name: 'pk', value: CarrierNumber.toString() };
                            var oCRUDCtrl = new nglRESTCRUDCtrl();
                            var blnRet = oCRUDCtrl.update("CarrierProNumbers/PostPageSetting", setting, tPage, "savePostPageSettingSuccessCallback", "savePostPageSettingAjaxErrorCallback", tPage);
                            location.href = "CarrierProNumbers";
                        }
            
                    }
                }
                else if (btn.id == "btnOpenCarCont") {
                    if (isBookingSelected() === true) {
                        if ("CarrierControl" in loadBoardGridSelectedRowDataItem) {
                            var CarrierControlPK = loadBoardGridSelectedRowDataItem.CarrierControl;
                            var setting = { name: 'pk', value: CarrierControlPK.toString() };
                            var oCRUDCtrl = new nglRESTCRUDCtrl();
                            var blnRet = oCRUDCtrl.update("LELanePreferredCarriers/PostPageSetting", setting, tPage, "savePostPageSettingSuccessCallback", "savePostPageSettingAjaxErrorCallback", tPage);
                            btnLPCarrierContact_Click(CarrierControlPK);
                        }
            
                    }
                }
                else if (btn.id == "btnCostMetrics") {
                    if (isBookingSelected() === true) {
                        if ("BookControl" in loadBoardGridSelectedRowDataItem) {
                            var setting = { name: 'pk', value: iBookPK.toString() };
                            var oCRUDCtrl = new nglRESTCRUDCtrl();
                            var blnRet = oCRUDCtrl.update("LoadBoard/PostPageSetting", setting, tPage, "savePostPageSettingSuccessCallback", "savePostPageSettingAjaxErrorCallback", tPage);
                        }
                        location.href = "LoadBoardRevenue";
                    }
                }
                else if (btn.id == "btnModify") {
                    if (isBookingSelected() === true) {
                        if ("BookControl" in loadBoardGridSelectedRowDataItem) {
                            var setting = { name: 'pk', value: iBookPK.toString() };
                            var oCRUDCtrl = new nglRESTCRUDCtrl();
                            var blnRet = oCRUDCtrl.update("LoadBoard/PostPageSetting", setting, tPage, "savePostPageSettingSuccessCallback", "savePostPageSettingAjaxErrorCallback", tPage);
                        }
            
                        btnLPModify_Click();
                    }
                    
                }
            
                else if (btn.id == "btnCreateBooking") { btnManageItems_Click(); }
                else if (btn.id == "btnCalculate") { if (isBookingSelected() === true) { CalculateTruck(iTruckCompControl, iTruckPK); } }
              else if(btn.id == "btnCalculateLineHaul" ){ if (isBookingSelected() === true) { CalculateTruckLineHaul(iTruckCompControl, iTruckPK); } }
              else if(btn.id == "btnBookingOptions" ){ 
               if (isBookingSelected() === true) {
                   var sBookingTenderDialogCTRLName = "wdgtBookingTenderWndDialog";
                   if (typeof (tPage["wdgtBookingTenderWorkFlowOptionCtrlEdit"]) !== 'undefined' && ngl.isObject(tPage["wdgtBookingTenderWorkFlowOptionCtrlEdit"])){
                       tPage["wdgtBookingTenderWorkFlowOptionCtrlEdit"].ReadUserSettings = false;
                   }
                   if (typeof (tPage["wdgtBookingTenderWndDialog"]) !== 'undefined' && ngl.isObject(tPage["wdgtBookingTenderWndDialog"])){
                       tPage["wdgtBookingTenderWndDialog"].clearWdgtHTML();
                       tPage["wdgtBookingTenderWndDialog"].read(iBookPK);
                   } else{alert("Missing HTML Element (wdgtBookingTenderWndDialog is undefined)");} //Add better error handling here if cm stuff is missing
               } 
            }
            else if(btn === "open_spot_dispatch_page"){ 
               if (typeof (tPage[proc]) !== 'undefined' && ngl.isObject(tPage[proc])) {
                   tPage[proc].executeActions("save");
                   tPage[proc].close();
               }
               //add code to open the spot dispatch page    
            }   
            else if (btn === "saveRateIT") {
                if (typeof (tPage[proc]) !== 'undefined' && ngl.isObject(tPage[proc])) {
                    //alert("Save Rate IT");
                    //tPage[proc].executeActions("save");
                    //tPage[proc].save();
                }
            }    
                else if (btn === "openNGLErrWarnMsgLogCtrlDialog") { openNGLErrWarnMsgLogCtrlDialog(); }
            else if(btn.id == "btnGetMiles") {if (isBookingSelected() === true){getMiles(iBookPK);}}//added by suhas 02/09/20
            else if (btn.id == "btnStopResequence" ){ if (isBookingSelected() === true){StopResequence(iBookPK);} } //added by suhas 02/09/20
            else if(btn.id == "btnOptimize" ){ ngl.showSuccessMsg("Coming Soon"); }
            //else if(btn.id == "btnMapIt") { if (isBookingSelected() === true){BingMapsCaller();} }
            else if (btn.id == "btnMapIt") { ngl.showSuccessMsg("Coming Soon"); }
            else if (btn.id == "btnAutoMiles") { if (isBookingSelected() === true) { AutoMiles(iBookPK, btn.id); } } 
            else if (btn.id == "btnAutoStop") { AutoStop(iBookPK, btn.id); }
            else if (btn.id == "btnAutoRecalculate") { AutoRecalculate(iBookPK, btn.id); }
            else if(btn.id == "btnGetRates" || btn === "GetRates"){ 
               //ngl.showSuccessMsg("Coming Soon"); 
               if (isBookingSelected() === true) {
                   var iReady = readyToAssignCarrier();
                   if (iReady === -1) { 
                       blnShowRateItAfterBookingOptions = false; 
                       return;
                   }
                   if (iReady === 1){
                       blnShowRateItAfterBookingOptions = false;
                       if (typeof (tPage["wdgtRateITDialog"]) !== 'undefined' && ngl.isObject(tPage["wdgtRateITDialog"])) {
                          tPage["wdgtRateITDialog"].read(iBookPK);
                      } 
                   } else {
                        blnShowRateItAfterBookingOptions = true;
                       execActionClick("BookingTenderOptions");
                   }                   
               }
            }
            else if (btn === "saveBookingTenderOptions") {
                    if (typeof (tPage[proc]) !== 'undefined' && ngl.isObject(tPage[proc])) {
                        //console.log(proc);
                        tPage[proc].executeActions("close");
                        if (typeof (tPage["wdgtBookingTenderWorkFlowOptionCtrlEdit"]) !== 'undefined' && ngl.isObject(tPage["wdgtBookingTenderWorkFlowOptionCtrlEdit"])) {
                            tPage["wdgtBookingTenderWorkFlowOptionCtrlEdit"].ReadUserSettings = false;
                            if (typeof (errWarnData) !== 'undefined' && ngl.isObject(errWarnData)) {

                                if (typeof (errWarnData.Log) !== 'undefined') {
                                    openNGLErrWarnMsgLogCtrlDialog();
                                }
                            }

                        }
                        //tPage[proc].close();
                        //oLoadBoardGrid.dataSource.read();
                        //if (blnShowRateItAfterBookingOptions === true) {
                        //    var iThisBookControl = iBookPK;
                        //    execActionClick("Refresh");
                        //    var oRow = oLoadBoardGrid.table.find('tr[data-id="' + iBookPK + '"]');
                        //    if (typeof (oRow) !== 'undefined' && ngl.isObject(oRow)) {
                        //        oLoadBoardGrid.select(oRow);
                        //        saveBookPK();
                        //        execActionClick("GetRates");
                        //    }                    
                        //}
                    }
                }
            else if(btn === "Saved"){ 
               //kendo.ui.progress($(document.body), false);
               if (typeof (tPage[proc]) !== 'undefined' && ngl.isObject(tPage[proc])) {
                   if (tPage[proc].sNGLCtrlName ==  "wdgtRateITDialog"){
                       execActionClick("Refresh");
                       if (typeof (tPage[proc].rData) !== 'undefined' && tPage[proc].rData != null && ngl.isArray(tPage[proc].rData) ){
                           //rData for RateIT returns  oGResult where
                           //oGResult.Control = a control number like LoadTenderControl 
                           //oGResult.strField = the field name like  "LoadTenderControl"
                           //oGResult.strField2 = the option selected like  "RateITPostWorkFlowGroup"
                           var oGResult = tPage[proc].rData[0];
                           if (typeof (oGResult) !== 'undefined' && oGResult != null && ngl.isObject(oGResult)){
                               if (!oGResult.Control || oGResult.Control == 0) {
                                   ngl.showWarningMsg("No Quotes Are Available", "Cannot Generate Quotes, check that the  DATCarrierNumber system parameter is correct", tPage);
                                   return;
                               }
                               
                               switch (oGResult.strField2) {
                                   case "RateITWorkFlowSwitchSpotRate":
                                       //this is a spot rate
                                       tPage[proc].executeActions("close");
                                       oDispatchingDialogCtrl.read(oGResult.Control)
                                       break;
                                   case "RateITRatingWorkFlowGroup":  
                                       //this is a quote selection
                                       //open the quote selection window similar to rate shopping
                                       //use the new QuotesWnd dialog
                                       tPage[proc].executeActions("close");
                                       if (typeof (tPage["wdgtQuotesWndDialog"]) !== 'undefined' && ngl.isObject(tPage["wdgtQuotesWndDialog"])){
                                           sBidLoadTenderControlVal = oGResult.Control;
                                           tPage["wdgtQuotesWndDialog"].read(oGResult.Control);
                                       } 
                                       tPage[proc].executeActions("close");
                                       break;
                                   case "RateITPostWorkFlowGroup":
                                       //this is a post to NEXTStop or DAT
                                       //TODO: find out what to show users?     
                                       //Added By LVV on 2/27/29 for bug fix - close the window and refresh the grid
                                       tPage[proc].executeActions("close");
                                       refresh();
                                       break;
                                   default:
                                       break;
                               }
                           }                          
                       }                       
                   }
               }             
            }     
            else if (btn.id == "btnRefresh" || btn === "Refresh") { refresh(); } 
            if(btn.id == "btnResetCurrentUserConfig"){
               saveSelectedTrucks("");
               resetCurrentUserConfig(PageControl);
            }
               //inewTruckBookPKType NO/ImportQue Order
            else if (btn.id == "btnAddTruck") {
                   if (inewTruckBookPK == 0) {
                       ngl.showErrMsg("Add New Truck", "Please select a Booking to continue", null);
                   }
                   else {
                       openActCarriersWnd(PageControl);
                   }
               }
             }
             function refresh() {
                 ngl.readDataSource($('#grid1').data("kendoNGLGrid"));
                 ngl.readDataSource($('#NewLoadsgrid').data("kendoNGLGrid"));
                 ngl.readDataSource($('#ImportQueue').data("kendoNGLGrid"));
             }
             var oQuoteSelectedBid = null;
             function dispatchQuoteButton(e) {
                 //debugger;
                 //alert('do dispatch')
                 oQuoteSelectedBid = this.dataItem($(e.currentTarget).closest("tr"));
                 if (typeof (oQuoteSelectedBid) !== 'undefined' && ngl.isObject(oQuoteSelectedBid)) {
                     oDispatchingDialogCtrl.read(oQuoteSelectedBid.BidControl)
                     if (typeof (tPage["wdgtQuotesWndDialog"]) !== 'undefined' && ngl.isObject(tPage["wdgtQuotesWndDialog"])) {
                         tPage["wdgtQuotesWndDialog"].executeActions("close");
                     }
                     //ngl.OkCancelConfirmation(
                     //       "Replace Load Information With Selected",
                     //       "This action will replace the current load information with the select address and item details",
                     //       400,
                     //       400,
                     //       null,
                     //       "ConfirmCopyHistoricalQuote");
                 };
             }
            
            
            function RateITCB(oResults){             
               if (typeof (oResults) === 'undefined' || ngl.isObject(oResults) === false) { return;}
               if(oResults.widget.sNGLCtrlName === "wdgtRateITDialog" && oResults.source === "saveSuccessCallback"){
                   if (typeof (oResults.Dialog) !== 'undefined' && oResults.Dialog != null ) { oResults.Dialog.data("kendoDialog").toFront(); }             
               }          
            }
            
            //*************  Action Menu Functions ****************
            function BingMapsCaller() { 
               var wgt = 0;
               if ("BookTotalWgt" in loadBoardGridSelectedRowDataItem) { wgt = loadBoardGridSelectedRowDataItem.BookTotalWgt; } 
               MapIt(iBookPK,wgt);
            }
            
            function savePostPageSettingSuccessCallback(results){
            //for now do nothing when we save the pk
            }
            function savePostPageSettingAjaxErrorCallback(xhr, textStatus, error){
            //for now do nothing when we save the pk
             }
             var oQuotesGrid = null;
             var sBidLoadTenderControlVal = "0"
             function QuotesGetStringData(s) {
                 //NOTE: This code is no longer needed in this scenario but I left the code here as an example in case we need to use it in the future
                 //var f = new FilterDetails();
                 //if (isNaN(s.iFilterID)) { s.iFilterID = 0;}
                 //s.iFilterID = s.iFilterID + 1;
                 //f.filterID = s.iFilterID; f.filterCaption = "BidLoadTenderControl"; f.filterName = "BidLoadTenderControl"; f.filterValueFrom = sBidLoadTenderControlVal; f.filterValueTo = ""; f.filterFrom = ""; f.filterTo = "";
                 ////s.pushToAllFilters(f);
                 //if (!s.FilterValues) { s.FilterValues = [f]; } else { if(!s.filterValuesContains(f.filterName)){ s.FilterValues.push(f);} }

                 s.ParentControl = sBidLoadTenderControlVal;
                 return '';
             }
             function QuotesDataBoundCallBack(e, tGrid) {
                 kendo.ui.progress($(document.body), false);
                 oQuotesGrid = tGrid;
                 var oDataSource = oQuotesGrid.dataSource;
                 var totalRecords = oDataSource.total();
                 var ipNoRatesFoundMsg = "p" + wdgtQuotesWndDialog.GetFieldID("pNoRatesFoundMsg");
                 var pNoRatesFoundPar = $("#" + ipNoRatesFoundMsg);
                 if (typeof (pNoRatesFoundPar) !== 'undefined') {
                     if (typeof (totalRecords) !== 'undefined' && totalRecords != null && !isNaN(totalRecords) && totalRecords > 0) {
                         //pNoRatesFoundPar.text('The system found ' + totalRecords.toString() + ' quotes');
                         //pNoRatesFoundPar.html('The system found ' + totalRecords.toString() + ' quotes');
                         pNoRatesFoundPar.text("");
                     } else {
                         pNoRatesFoundPar.css({ "color": "red", "font-weight": "bold", "font-size": "large" })
                         pNoRatesFoundPar.text("No quotes were found for this load using the loads temperature and the loads mode of delivery.  All carrier validation rules were applied so the carriers may not be qualified; check the contract and insurance informaton.  We also optimized by capacity, so check that a tariff exists for the capacity of the load.  Please check your configuration and try again.");
                     }
                 }

             }
            
            function CalculateTruck(CompControl, truckKey) {
               var dataJSON = { SolDetailCompControl: CompControl, solTruckKey:truckKey };
               $.ajax({
                   type: "POST",
                   url: "api/LoadPlanning/CalculateTruck/",
                   contentType: "application/json; charset=utf-8",
                   dataType: 'json',
                   data: JSON.stringify(dataJSON),
                   headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                   success: function(data) {
            
                       if (data.Errors != null) {
            
                           if (data.StatusCode === 203){ 
                               ngl.showErrMsg("Authorization Timeout", data.Errors, null); 
                           } 
                           else 
                           { 
                               ngl.showErrMsg("Access Denied", data.Errors, null); 
                           } 
                       }
                       else {                         
                           refreshTruck(CompControl, truckKey);
                           ngl.showSuccessMsg("Calculate completed successfully");
                       }
                   },
                   error: function (xhr, textStatus, error) {
                       var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Calculate Failure");
                       ngl.showErrMsg("Load Planning - Calculate", sMsg, null);
                   }
               });
            }
            
            function CalculateTruckLineHaul(CompControl, truckKey) {
               var dataJSON = { SolDetailCompControl: CompControl, solTruckKey:truckKey };
               $.ajax({
                   type: "POST",
                   url: "api/LoadPlanning/CalculateTruckLineHaul/",
                   contentType: "application/json; charset=utf-8",
                   dataType: 'json',
                   data: JSON.stringify(dataJSON),
                   headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                   success: function(data) {
            
                       if (data.Errors != null) {
            
                           if (data.StatusCode === 203){ 
                               ngl.showErrMsg("Authorization Timeout", data.Errors, null); 
                           } 
                           else 
                           { 
                               ngl.showErrMsg("Access Denied", data.Errors, null); 
                           }
                       }
                       else { 
                           refreshTruck(CompControl, truckKey);
                           ngl.showSuccessMsg("Calculate using Line Haul completed successfully");
                       }
                   },
                   error: function (xhr, textStatus, error) {
                       var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Calculate Failure");
                       ngl.showErrMsg("Load Planning - Calculate", sMsg, null);
                   }
               });
            }
            
            function refreshTruck(iTruckCompControl,iTruckPK)
               {
               //Invoked to refresh the loads in truck and display updated stop/sequence number
                   var ds = $("#dy_grid_" + iTruckPK + "___0").data().kendoNGLGrid.dataSource;
            
               $.ajax({
                   url: "api/LoadPlanning/GetLoadsInTruck/",
                   contentType: "application/json; charset=utf-8",
                   dataType: 'json',
                   data: {CompControl: JSON.stringify(iTruckCompControl), TruckKey: JSON.stringify(iTruckPK)},
                   headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                   success: function(data) {
            
                       if (data.Errors != null) { 
            
                           if (data.StatusCode === 203){ 
                               ngl.showErrMsg("Authorization Timeout", data.Errors, null); 
                           } 
                           else 
                           { 
                               ngl.showErrMsg("Access Denied", data.Errors, null); 
                           } 
                       } else {
                                                            
                           //console.log(data.Data);
                           ds.data(data.Data);
                           ds.sync();
                       } 
                   },
                   error: function (xhr, textStatus, error) {
                       var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure");
                       ngl.showErrMsg("Load Planning - Orders in Truck", sMsg, null);
                   }
               });
            }
            
            function saveSelectedTrucks(TruckKeys) {
               var chkbxTruckKeys = {  SolDetailCompControl: TruckKeys };
               $.ajax({ 
                   type: "POST",
                   url: 'api/LoadPlanning/SavePageChanges/', 
                   contentType: 'application/json; charset=utf-8', 
                   dataType: 'json',                       
                   data:   JSON.stringify(chkbxTruckKeys) , 
                   headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                   success: function(data) { 
                       if (data.Errors != null) {
                           if (data.StatusCode === 203){ 
                               ngl.showErrMsg("Authorization Timeout", data.Errors, null); 
                           } 
                           else 
                           { 
                               ngl.showErrMsg("Access Denied", data.Errors, null); 
                           } 
                       } 
                   },
                   error: function(result) { 
                       options.error(result); 
                   } 
               }); 
            }
            function saveSelectedPageChanges(TruckKeys,TruckPageSize,IsCollapse,TruckSize) {//Added for PageSettings
               var chkbxTruckKeys = {  SolDetailCompControl: TruckKeys ,TruckPageSize:TruckPageSize,IsCollapse:IsCollapse,TruckSize:TruckSize};
               $.ajax({ 
                   type: "POST",
                   url: 'api/LoadPlanning/SavePageChanges/', 
                   contentType: 'application/json; charset=utf-8', 
                   dataType: 'json',                       
                   data:   JSON.stringify(chkbxTruckKeys) , 
                   headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                   success: function(data) { 
                       if (data.Errors != null) {
                           if (data.StatusCode === 203){ 
                               ngl.showErrMsg("Authorization Timeout", data.Errors, null); 
                           } 
                           else 
                           { 
                               ngl.showErrMsg("Access Denied", data.Errors, null); 
                           } 
                       } 
                   },
                   error: function(result) { 
                       options.error(result); 
                   } 
               }); 
            }
            let checkiDValues=[];
            TruckKeys=[],TruckPageSize=0,IsCollapse=new Boolean(false),TruckSize=0;
            $(document).ready(function () {
            var PageMenuTab = <%=PageMenuTab%>;
            
            
            if (control != 0){
                $('#btnCAR120').prop('disabled', true);
                blnCanPrintBOL = false;
                $('#btnDispatchReport').prop('disabled', true);
                blnCanPrintDispatch = false;


                oDispatchingDialogCtrl = new DispatchingDialogCtrl();
                oDispatchingDialogCtrl.loadDefaults(winDispatchingDialog, oDispatchingDialogSelectCB, oDispatchingDialogSaveCB, oDispatchingDialogCloseCB, oDispatchingDialogReadCB);

                oDispatchReportCtrl = new DispatchingReportCtrl();
                oDispatchReportCtrl.loadDefaults(winDispatchReport, oDispatchingReportSelectCB, oDispatchingReportSaveCB, oDispatchingReportCloseCB, oDispatchingReportReadCB);

                oBOLReportCtrl = new BOLReportCtrl();
                oBOLReportCtrl.loadDefaults(winBOLReport, oBOLReportSelectCB, oBOLReportSaveCB, oBOLReportCloseCB, oBOLReportReadCB);

               //var initialTruckKeys = ["2-CNS106523-1-6-0","14-SOJ-10-6-1-6-0","15-CNS110274-1-6-0"];
               var initialTruckKeys = [];
               var consignments;
               var TotLcnt = 0;
            
               var objid39111201806290836427404336Filters = new AllFiltersCtrl('True', 'loadPlanning');
               var objid39111201806290836427404336FilterData = [
                   { filterName: "CompName", filterCaption: "Warehouse", filterValueFrom: "", filterValueTo: "", filterFrom: "", filterTo: "", filterIsDate: false },
                   { filterName: "CompNumber", filterCaption: "Warehouse Number", filterValueFrom: "", filterValueTo: "", filterFrom: "", filterTo: "", filterIsDate: false },
                   { filterName: "CarrierName", filterCaption: "Carrier Name", filterValueFrom: "", filterValueTo: "", filterFrom: "", filterTo: "", filterIsDate: false },
                   { filterName: "CarrierNumber", filterCaption: "Carrier Number", filterValueFrom: "", filterValueTo: "", filterFrom: "", filterTo: "", filterIsDate: false },
                   { filterName: "BookDateLoad", filterCaption: "Load Date", filterValueFrom: "", filterValueTo: "", filterFrom: "", filterTo: "", filterIsDate: true },
                   { filterName: "BookDateRequired", filterCaption: "Required Date", filterValueFrom: "", filterValueTo: "", filterFrom: "", filterTo: "", filterIsDate: true },
                   { filterName: "BookOrigName", filterCaption: "Orig Name", filterValueFrom: "", filterValueTo: "", filterFrom: "", filterTo: "", filterIsDate: false },
                   { filterName: "BookOrigAddress1", filterCaption: "Orig Address", filterValueFrom: "", filterValueTo: "", filterFrom: "", filterTo: "", filterIsDate: false },
                   { filterName: "BookOrigCity", filterCaption: "Orig City", filterValueFrom: "", filterValueTo: "", filterFrom: "", filterTo: "", filterIsDate: false },
                   { filterName: "BookOrigState", filterCaption: "Orig State 1", filterValueFrom: "", filterValueTo: "", filterFrom: "", filterTo: "", filterIsDate: false },
                   { filterName: "BookOrigState2", filterCaption: "Orig State 2", filterValueFrom: "", filterValueTo: "", filterFrom: "", filterTo: "", filterIsDate: false },
                   { filterName: "BookOrigZip", filterCaption: "Orig Zip", filterValueFrom: "", filterValueTo: "", filterFrom: "", filterTo: "", filterIsDate: false },
                   { filterName: "BookOrigCountry", filterCaption: "Orig Country", filterValueFrom: "", filterValueTo: "", filterFrom: "", filterTo: "", filterIsDate: false },
                   { filterName: "BookDestName", filterCaption: "Dest Name", filterValueFrom: "", filterValueTo: "", filterFrom: "", filterTo: "", filterIsDate: false },
                   { filterName: "BookDestAddress1", filterCaption: "Dest Address", filterValueFrom: "", filterValueTo: "", filterFrom: "", filterTo: "", filterIsDate: false },
                   { filterName: "BookDestCity", filterCaption: "Dest City", filterValueFrom: "", filterValueTo: "", filterFrom: "", filterTo: "", filterIsDate: false },
                   { filterName: "BookDestState", filterCaption: "Dest State 1", filterValueFrom: "", filterValueTo: "", filterFrom: "", filterTo: "", filterIsDate: false },
                   { filterName: "BookDestState2", filterCaption: "Dest State 2", filterValueFrom: "", filterValueTo: "", filterFrom: "", filterTo: "", filterIsDate: false },
                   { filterName: "BookDestZip", filterCaption: "Dest Zip", filterValueFrom: "", filterValueTo: "", filterFrom: "", filterTo: "", filterIsDate: false },
                   { filterName: "BookDestCountry", filterCaption: "Dest Country" , filterValueFrom: "",filterValueTo: "",filterFrom: "",filterTo: "",filterIsDate: false },
                   { filterName: "BookTranCode", filterCaption: "TranCode", filterValueFrom: "", filterValueTo: "", filterFrom: "", filterTo: "", filterIsDate: false },
                   { filterName: "BookTransType", filterCaption: "Trans Type", filterValueFrom: "", filterValueTo: "", filterFrom: "", filterTo: "", filterIsDate: false },
                   { filterName: "LaneNumber", filterCaption: "Lane Number", filterValueFrom: "", filterValueTo: "", filterFrom: "", filterTo: "", filterIsDate: false },
                   { filterName: "LaneName", filterCaption: "Lane Name", filterValueFrom: "", filterValueTo: "", filterFrom: "", filterTo: "", filterIsDate: false },
                   { filterName: "BookConsPrefix", filterCaption: "Consolidation Number", filterValueFrom: "", filterValueTo: "", filterFrom: "", filterTo: "", filterIsDate: false },
                   { filterName: "BookSHID", filterCaption: "Shipment ID", filterValueFrom: "", filterValueTo: "", filterFrom: "", filterTo: "", filterIsDate: false },
                   { filterName: "PageSize", filterCaption: "Page Size", filterValueFrom: "", filterValueTo: "", filterFrom: "", filterTo: "", filterIsDate: false }
               ];
            
               objid39111201806290836427404336Filters.loadDefaults("divFilters", "grid1", objid39111201806290836427404336FilterData, 'blueopal',
                   function (results) {
                       $("#centerDiv").empty();
                        PageStatus = 1;
                       var oKendoGrid1 = $('#grid1').data("kendoNGLGrid");
                       if (typeof (oKendoGrid1) !== 'undefined' && ngl.isObject(oKendoGrid1)) { oKendoGrid1.dataSource.read(); }
                       var oKendoGrid = $('#NewLoadsgrid').data("kendoNGLGrid");
                       if (typeof (oKendoGrid) !== 'undefined' && ngl.isObject(oKendoGrid)) { oKendoGrid.dataSource.read(); }
                       return;
                   }, 'Load Planning Filters');
               objid39111201806290836427404336Filters.show();
                objid39111201806290836427404336Filters.addSavedFilters();

               //objid39111201806290836427404336Filters.loadDefaults("divFilters", "NewLoadsgrid", objid39111201806290836427404336FilterData, 'blueopal', function (results) {  PageStatus=1;var oKendoGrid = $('#NewLoadsgrid').data("kendoNGLGrid"); if (typeof (oKendoGrid) !== 'undefined' && ngl.isObject(oKendoGrid)) { oKendoGrid.dataSource.read();  } return; }); objid39111201806290836427404336Filters.show();


               //var s = $("#grid1FilterFastTab span").eq(4);
               //s.html("Load Planning Filters");
            
               $.ajax({ 
                   url: 'api/LoadPlanning/GetPageSettings?filter=SummaryLoadsFltr',
                   contentType: 'application/json; charset=utf-8', 
                   dataType: 'json', 
                   //  data: { filter: JSON.stringify(s) ,PageStatus:JSON.stringify(PageStatus)}, 
                   headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                   success: function(data) { 
                       //  debugger; }
                       var value = JSON.parse(data.Data[0].value);
                       var filterValues = [];
                       if (typeof value.FilterValues !== 'undefined') {
                           filterValues = value.FilterValues;
                           objid39111201806290836427404336Filters.show();
                           objid39111201806290836427404336Filters.addSavedFilters(filterValues);
                           objid39111201806290836427404336Filters.reapplyFilter();
                           consignments = [];
                           $("#centerDiv").empty();
                        }else{
                             
                       }
                       PageStatus=1;
                       if (data.Errors != null) {                                   
                           if (data.StatusCode === 203){ 
                               ngl.showErrMsg("Authorization Timeout", data.Errors, null); 
                           } 
                           else 
                           { 
                               ngl.showErrMsg("Access Denied", data.Errors, null); 
                           } 
                       } 
                   }, 
            
                   error: function(result) { 
                       // options.error(result); 
                   } 
               });     
            
              
                
                
            
               function navigationType(){
            
                   var result;
                   var p;
            
                   if (window.performance.navigation) {
                       result=window.performance.navigation;
                       if (result==255){result=4} // 4 is my invention!
                   }
            
                   if (window.performance.getEntriesByType("navigation")){
                       p=window.performance.getEntriesByType("navigation")[0].type;
            
                       if (p=='navigate'){result=0}
                       if (p=='reload'){result=1}
                       if (p=='back_forward'){result=2}
                       if (p=='prerender'){result=3} //3 is my invention!
                   }
                   return result;
               }
               PageStatus=100;
               if(PageStatus==100){
                   PageStatus=navigationType();
               }
               else{
                   PageStatus=1;
               }
                PageSize = 10;

               

               vSummaryLoadsGrid365 = new kendo.data.DataSource({
                   serverSorting: true, 
                   serverPaging: true, 
                   pageSize: 10,
                   transport: { 
                       read: function (options) {  
                          var s = objid39111201806290836427404336Filters.data;
                           s.page = options.data.page;
                           s.skip = options.data.skip;
                           s.take = options.data.take;

                           $.ajax({ 
                               url: 'api/LoadPlanning/GetRecordsBySummeryFilter/', 
                               contentType: 'application/json; charset=utf-8', 
                               dataType: 'json', 
                               data: { filter: JSON.stringify(s), PageStatus:JSON.stringify(PageStatus) }, 
                               headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                               success: function(data) { 
                                   consignments = data.Data;
                                   $("#centerDiv").empty();
                                 //console.log(cons[0]["SolutionTruckConsPrefix"]);
                                   options.success(data);
                                   if (data.Errors != null) { 
                                       PageStatus=1;
                                       if (data.StatusCode === 203){ 
                                           ngl.showErrMsg("Authorization Timeout", data.Errors, null); 
                                       } 
                                       else 
                                       { 
                                           ngl.showErrMsg("Access Denied", data.Errors, null); 
                                       } 
                                   } 
                                   initailizeDefaultGrid();
                               }, 
            
                               error: function(result) { 
                                   options.error(result); 
                               } 
                           }); 
            
                       },           
                       parameterMap: function(options, operation) { return options; } 
                       
                   },  
                   schema: {
                       data: "Data",
                       total: "Count",
                       model: {
                           id: "SolutionTruckControl",
                           fields: {
                               SolutionTruckControl: { type: "number" },
                               SolutionTruckConsPrefix: { type: "string" },
                               SolutionTruckTotalWgt: { type: "number" },
                               SolutionTruckTotalCube: { type: "number" },
                               SolutionTruckSolutionControl: { type: "number" },
                               SolutionTruckStaticRouteControl: { type: "number" },
                               SolutionTruckAttributeControl: { type: "number" },
                               SolutionTruckAttributeTypeControl: { type: "string" },
                               SolutionTruckCom: { type: "string" },
                               SolutionTruckRouteConsFlag: { type: "bool" },
                               SolutionTruckCarrierControl: { type: "number" },
                               SolutionTruckCarrierNumber: { type: "number" },
                               SolutionTruckCarrierName: { type: "string" },
                               SolutionTruckCarrierTruckControl: { type: "number" },
                               SolutionTruckCarrierTruckDescription: { type: "string" },
                               SolutionTruckTotalCases: { type: "number" },
                               SolutionTruckTotalPL: { type: "number" },
                               SolutionTruckTotalPX: { type: "number" },
                               SolutionTruckTotalBFC: { type: "number" },
                               SolutionTruckTotalOrders: { type: "number" },
                               SolutionTruckTotalCost: { type: "number" },
                               SolutionTruckTotalMiles: { type: "number" },
                               SolutionTruckCarrierEquipmentCodes: { type: "string" },
                               SolutionTruckRouteTypeCode: { type: "number" },
                               SolutionTruckTransType: { type: "number" },
                               SolutionTruckCommitted: { type: "bool" },
                               SolutionTruckCommittedDate: { type: "date" },
                               SolutionTruckCapacityPreference: { type: "number" },
                               SolutionTruckMinCases: { type: "number" },
                               SolutionTruckSplitCases: { type: "string" },
                               SolutionTruckMaxCases: { type: "number" },
                               SolutionTruckMinWgt: { type: "number" },
                               SolutionTruckSplitWgt: { type: "number" },
                               SolutionTruckMaxWgt: { type: "number" },
                               SolutionTruckMinCubes: { type: "number" },
                               SolutionTruckSplitCubes: { type: "number" },
                               SolutionTruckMaxCubes: { type: "number" },
                               SolutionTruckMinPlts: { type: "number" },
                               SolutionTruckSplitPlts: { type: "number" },
                               SolutionTruckMaxPlts: { type: "number" },
                               SolutionTruckTrucksAvailable: { type: "number" },
                               SolutionTruckIsHazmat: { type: "bool" },
                               SolutionTruckLaneNumbers: { type: "string" },
                               SolutionTruckLaneNames: { type: "string" },
                               SolutionTruckBookNotes: { type: "string" },
                               SolutionTruckModDate: { type: "date" },
                               SolutionTruckModUser: { type: "string" },
                               SolutionTruckUpdated: { type: "date" }
                           }
                       },
                       errors: "Errors"
                   },
                   error: function(xhr, textStatus, error) {
                       ngl.showErrMsg("Access vSummaryLoadsGrid365 Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges();
                   }
               });
              // console.log("vSummaryLoadsGrid365", vSummaryLoadsGrid365)
               $('#grid1').kendoNGLGrid({
                   theme: "blueopal",
                   toolbarColumnMenu: true,
                   dataSource: vSummaryLoadsGrid365,                   
                   autoBind: true,
                   pageable: { pageSizes: [5, 10, 15, 20, 25, 50]},
                   sortable: true,
                   resizable: true,
                   groupable: true, 
                   columns: [
                       { template: "<input type='checkbox' class='left-grid-row-checkbox checkbox' click='selectRow()' />" },
                       { field: "SolutionTruckControl", title: "SolutionTruckControl", hidden: true, PageDetPageControl: 26, PageDetControl: 103909 },
                       { field: "SolutionTruckConsPrefix", title: "CNS", showhide: 1, PageDetPageControl: 26, PageDetControl: 103908 },
                       { field: "SolutionTruckTotalWgt", title: "Wgt", showhide: 1, PageDetPageControl: 26, PageDetControl: 103939 },
                       { field: "SolutionTruckTotalPL", title: "PL", showhide: 1, PageDetPageControl: 26, PageDetControl: 103937 },
                       { field: "SolutionTruckTotalCube", title: "Volume", showhide: 1, PageDetPageControl: 26, PageDetControl: 103934 },
                       { field: "SolutionTruckCarrierName", title: "Carrier Name", showhide: 1, PageDetPageControl: 26, PageDetControl: 103901 },
                       { field: "SolutionTruckCarrierControl", title: "Carrier Control", showhide: 1, hidden: true, PageDetPageControl: 26, PageDetControl: 103899 },
                       { field: "SolutionTruckCarrierNumber", title: "Carrier Number", showhide: 1, hidden: true, PageDetPageControl: 26, PageDetControl: 103902 },
                       { field: "SolutionTruckCarrierTruckControl", title: "Carrier Truck Control", showhide: 1, hidden: true, PageDetPageControl: 26, PageDetControl: 103903 },
                       { field: "SolutionTruckCarrierTruckDescription", title: "Carrier Truck Description", showhide: 1, hidden: true, PageDetPageControl: 26, PageDetControl: 103904 },
                       { field: "SolutionTruckCom", title: "Solution Truck Com", showhide: 1, hidden: true, PageDetPageControl: 26, PageDetControl: 103905 },
                       { field: "SolutionTruckCommitted", title: "Committed", showhide: 1, hidden: true, PageDetPageControl: 26, PageDetControl: 103906 },
                       { field: "SolutionTruckCommittedDate", title: "Committed Date", showhide: 1, hidden: true, PageDetPageControl: 26, PageDetControl: 103907 },
                       { field: "SolutionTruckTotalCases", title: "Cases", showhide: 1, hidden: true, PageDetPageControl: 26, PageDetControl: 103932 },
                       { field: "SolutionTruckTotalPX", title: "PX", showhide: 1, hidden: true, PageDetPageControl: 26, PageDetControl: 103938 },
                       { field: "SolutionTruckTotalBFC", title: "BFC", showhide: 1, hidden: true, PageDetPageControl: 26, PageDetControl: 103931 },
                       { field: "SolutionTruckTotalOrders", title: "Orders", showhide: 1, hidden: true, PageDetPageControl: 26, PageDetControl: 103936 },
                       { field: "SolutionTruckTotalCost", title: "Cost", showhide: 1, hidden: true, PageDetPageControl: 26, PageDetControl: 103933 },
                       { field: "SolutionTruckTotalMiles", title: "Miles", showhide: 1, hidden: true, PageDetPageControl: 26, PageDetControl: 103935 },
                       { field: "SolutionTruckCarrierEquipmentCodes", title: "Carrier Equip Code", showhide: 1, hidden: true, PageDetPageControl: 26, PageDetControl: 103900 },
                       { field: "SolutionTruckRouteTypeCode", title: "RouteType Code", showhide: 1, hidden: true, PageDetPageControl: 26, PageDetControl: 103924 },
                       { field: "SolutionTruckTransType", title: "Trans Type", showhide: 1, hidden: true, PageDetPageControl: 26, PageDetControl: 103940 },
                       { field: "SolutionTruckCapacityPreference", title: "Capacity Preference", showhide: 1, hidden: true, PageDetPageControl: 26, PageDetControl: 103898 },
                       { field: "SolutionTruckMinCases", title: "Min Cases", showhide: 1, hidden: true, PageDetPageControl: 26, PageDetControl: 103917 },
                       { field: "SolutionTruckSplitCases", title: "Split Cases", showhide: 1, hidden: true, PageDetPageControl: 26, PageDetControl: 103926 },
                       { field: "SolutionTruckMaxCases", title: "Max Cases", showhide: 1, hidden: true, PageDetPageControl: 26, PageDetControl: 103913 },
                       { field: "SolutionTruckMinWgt", title: "Min Wgt", showhide: 1, hidden: true, PageDetPageControl: 26, PageDetControl: 103920 },
                       { field: "SolutionTruckSplitWgt", title: "Split Wgt", showhide: 1, hidden: true, PageDetPageControl: 26, PageDetControl: 103929 },
                       { field: "SolutionTruckMaxWgt", title: "Max Wgt", showhide: 1, hidden: true, PageDetPageControl: 26, PageDetControl: 103916 },
                       { field: "SolutionTruckMinCubes", title: "Min Cubes", showhide: 1, hidden: true, PageDetPageControl: 26, PageDetControl: 103918 },
                       { field: "SolutionTruckSplitCubes", title: "Split Cubes", showhide: 1, hidden: true, PageDetPageControl: 26, PageDetControl: 103927 },
                       { field: "SolutionTruckMaxCubes", title: "Max Cubes", showhide: 1, hidden: true, PageDetPageControl: 26, PageDetControl: 103914 },
                       { field: "SolutionTruckTrucksAvailable", title: "Trucks Aval", showhide: 1, hidden: true, PageDetPageControl: 26, PageDetControl: 103941 },
                       { field: "SolutionTruckIsHazmat", title: "IsHazmat", showhide: 1, hidden: true, PageDetPageControl: 26, PageDetControl: 103910 },
                       { field: "SolutionTruckLaneNumbers", title: "Lane Number", showhide: 1, PageDetPageControl: 26, PageDetControl: 103912 },
                       { field: "SolutionTruckLaneNames", title: "Lane Names", showhide: 1, hidden: true, PageDetPageControl: 26, PageDetControl: 103911 },
                       { field: "SolutionTruckBookNotes", title: "Book Notes", showhide: 1, hidden: true, PageDetPageControl: 26, PageDetControl: 103897 },
                       { field: "SolutionTruckModDate", title: "Mod Date", showhide: 1, hidden: true, PageDetPageControl: 26, PageDetControl: 103921 },
                       { field: "SolutionTruckModUser", title: "Mod User", showhide: 1, hidden: true, PageDetPageControl: 26, PageDetControl: 103922 },
                       { field: "SolutionTruckUpdated", title: "Updated", showhide: 1, hidden: true, PageDetPageControl: 26, PageDetControl: 103942 },
                       { field: "SolutionTruckSolutionControl", title: "Solution Control", showhide: 1, hidden: true, PageDetPageControl: 26, PageDetControl: 103925 },
                       { field: "SolutionTruckStaticRouteControl", title: "Static Route Control", showhide: 1, hidden: true, PageDetPageControl: 26, PageDetControl: 103930 },
                       { field: "SolutionTruckAttributeControl", title: "Attribute Control", showhide: 1, hidden: true, PageDetPageControl: 26, PageDetControl: 103895 },
                       { field: "SolutionTruckAttributeTypeControl", title: "AttributeType Control", showhide: 1, hidden: true, PageDetPageControl: 26, PageDetControl: 103896 },
                       { field: "SolutionTruckRouteConsFlag", title: "RouteCons Flag", showhide: 1, hidden: true, PageDetPageControl: 26, PageDetControl: 103923 },
                       { field: "SolutionTruckMaxPlts", title: "Max Plts", showhide: 1, hidden: true, PageDetPageControl: 26, PageDetControl: 103915 },
                       { field: "SolutionTruckMinPlts", title: "Min Plts", showhide: 1, hidden: true, PageDetPageControl: 26, PageDetControl: 103919 },
                       { field: "SolutionTruckSplitPlts", title: "Split Plts", showhide: 1, hidden: true, PageDetPageControl: 26, PageDetControl: 103928 }
                   ]
              
               });
            
               var grid = $("#grid1").data("kendoNGLGrid");
               
               vNewLoadsGrid365 = new kendo.data.DataSource({
                   serverSorting: true, 
                   serverPaging: true, 
                   pageSize: 10,
                   transport: { 
                       read: function(options) { 
                           //debugger;
                           //var s = new AllFilter();
                           var s = objid39111201806290836427404336Filters.data;
            
                           s.page = options.data.page;
                           s.skip = options.data.skip;
                           s.take = options.data.take;
                           //s.PageSize = 10;
            
                           $.ajax({ 
            
                               url: 'api/LoadPlanning/GetRecordsByNewOrdersFilter/', 
                               contentType: 'application/json; charset=utf-8', 
                               dataType: 'json', 
                               data: { filter: JSON.stringify(s),PageStatus:JSON.stringify(PageStatus) }, 
                               headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                               success: function(data) { 
                                   options.success(data);
                                   //console.log(data);
                                   PageStatus=1; 
                                   $("#NLcnt").text(data.Data.length);
                                   TotLcnt = parseInt($("#ILcnt").text()) + parseInt($("#NLcnt").text());
                                   $("#TotNLcnt").text(TotLcnt);
                                   
                                  
                                   if (data.Errors != null) { 
            
                                       if (data.StatusCode === 203){ 
                                           ngl.showErrMsg("Authorization Timeout", data.Errors, null); 
                                       } 
                                       else 
                                       { 
                                           ngl.showErrMsg("Access Denied", data.Errors, null); 
                                       } 
                                   } 
                               }, 
            
                               error: function(result) { 
                                   options.error(result); 
                               } 
                           }); 
            
                       },           
                       parameterMap: function(options, operation) { return options; } 
                   },  
                   schema: {
                       data: "Data",
                       total: "Count",
                       model: {
                           id: "Control",
                           fields: {
                               Control: { type: "number" },
                               SolutionDetailConsPrefix: { type: "string" },
                               SolutionDetailTotalWgt: { type: "number" },                               
                               SolutionDetailTotalPL: { type: "number" },
                               SolutionDetailTotalCube: { type: "number" },
                               SolutionDetailCarrierName: { type: "string" }
                             
                           }
                       },
                       errors: "Errors"
                   },
                   error: function(xhr, textStatus, error) {
                       ngl.showErrMsg("Access vNewLoadsGrid365 Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges();
                   }
               });
            
               var NewLoadsgrid = $('#NewLoadsgrid').kendoNGLGrid({
                   theme: "blueopal",
                   toolbarColumnMenu:true,
                   dataSource: vNewLoadsGrid365,
                   autoBind: true,
                   pageable: { pageSizes: [5, 10, 15, 20, 25, 50]},
                   sortable: true,
                   scrollable: true,
                   resizable: true,
                   groupable: true, 
                   selectable: "row",
                   rowTemplate: kendo.template($("#NLoads-rowTemplate").html()),
                   altRowTemplate: kendo.template($("#NLoads-altRowTemplate").html()),
                   columns: [         
                       {field: "SolutionDetailOrderNumber", title: "New Loads",showhide: 1},
                       //{field: "SolutionDetailTotalWgt", title: "Wgt",showhide: 1},
                       //{field: "SolutionDetailTotalPL", title: "Pits",showhide: 1},
                       //{field: "SolutionDetailTotalCube", title: "Volume",showhide: 1},               
                       //{field: "SolutionDetailDestName", title: "Destination",showhide: 1},
                       //{field: "SolutionDetailDestCity", title: "City",showhide: 1},
                       //{field: "SolutionDetailDestState", title: "State",showhide: 1}
                   ]               
               }).data("kendoNGLGrid");
            
               vImportQueueGrid365 = new kendo.data.DataSource({
                   serverSorting: true, 
                   serverPaging: true, 
                   pageSize: 10,     
                   pageable: { pageSizes: [5, 10, 15, 20, 25, 50]},
                   transport: { 
                       read: function(options) { 
                           //debugger;
                           //var s = new AllFilter();
                           var s = objid39111201806290836427404336Filters.data;
            
                           // s.sortName = $("#txtSettlementGridSortField").val();s.sortDirection = $("#txtSettlementGridSortDirection").val();
                           s.page = options.data.page;
                           s.skip = options.data.skip;
                           s.take = options.data.take;
            
                           $.ajax({ 
            
                               url: 'api/LoadPlanning/GetRecordsByImportQueueFilter/', 
                               contentType: 'application/json; charset=utf-8', 
                               dataType: 'json', 
                               data: { filter: JSON.stringify(s),PageStatus:JSON.stringify(PageStatus) }, 
                               headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                               success: function(data) { 
                                   options.success(data);
                                   $("#ILcnt").text(data.Data.length);
                                   TotLcnt = parseInt($("#ILcnt").text()) + parseInt($("#NLcnt").text());
                                   $("#TotNLcnt").text(TotLcnt);
            
                                   if (data.Errors != null) { 
            
                                       if (data.StatusCode === 203){ 
                                           ngl.showErrMsg("Authorization Timeout", data.Errors, null); 
                                       } 
                                       else 
                                       { 
                                           ngl.showErrMsg("Access Denied", data.Errors, null); 
                                       } 
                                   } 
                               }, 
            
                               error: function(result) { 
                                   options.error(result); 
                               } 
                           }); 
            
                       },           
                       parameterMap: function(options, operation) { return options; } 
                   },  
                   schema: {
                       data: "Data",
                       total: "Count",
                       model: {
                           id: "Control",
                           fields: {
                               Control: { type: "number" },
                               SolutionDetailConsPrefix: { type: "string" },
                               SolutionDetailTotalWgt: { type: "number" },                               
                               SolutionDetailTotalPL: { type: "number" },
                               SolutionDetailTotalCube: { type: "number" },
                               SolutionDetailCarrierName: { type: "string" }
                             
                           }
                       },
                       errors: "Errors"
                   },
                   error: function(xhr, textStatus, error) {
                       ngl.showErrMsg("Access ImportQueue Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges();
                   }
               });
               
               var ImportQueueGrid = $('#ImportQueue').kendoNGLGrid({
                   theme: "blueopal",
                   dataSource: vImportQueueGrid365,
                   autoBind: true,
                   pageable: { pageSizes: [5, 10, 15, 20, 25, 50]},
                   sortable: true,
                   resizable: true,
                   groupable: true,
                   selectable: "row",
                   rowTemplate: kendo.template($("#IQLoads-rowTemplate").html()),
                   altRowTemplate: kendo.template($("#IQLoads-altRowTemplate").html()),
                   columns: [
                       {field: "SolutionDetailOrderNumber", title: "New Loads",showhide: 1},
                       //{field: "SolutionDetailConsPrefix", title: "Order No."},
                       //{field: "SolutionDetailTotalWgt", title: "Wgt"},
                       //{field: "SolutionDetailTotalPL", title: "Pits"},
                       //{field: "SolutionDetailTotalCube", title: "Volume"},               
                       //{field: "SolutionDetailDestName", title: "Destination",showhide: 1},
                       //{field: "SolutionDetailDestCity", title: "City",showhide: 1},
                       //{field: "SolutionDetailDestState", title: "State",showhide: 1}
                   ]
              
               }).data("kendoNGLGrid");
            
               var newTruckGrid = $("#newTruckgrid").kendoNGLGrid({
                   dragAndDrop: true,
                   width: 400,
                   height: 50,
                   dataSource: new kendo.data.DataSource({
                       data: [] 
                   }),
                   groupable: false,
                   sortable: true,
                   selectable: false,
                   columns: [
                   ]
               }).data("kendoNGLGrid");
               function droptargetOnDragEnter(e) {
                   $("#newTruckgrid").find('.k-grid-content').addClass("dragOnGrid");
            
               }
               function droptargetOnDragLeave(e) {
                   $("#newTruckgrid").find('.k-grid-content').removeClass("dragOnGrid");
               }
               function droptargetOnDrop(e) {
                    $("#newTruckgrid").find('.k-grid-content').removeClass("dragOnGrid");
               }
            
               function createNewCNS(e) {
                   if ($(e.draggable.currentTarget).hasClass("k-footer-template"))
                       return;
                   if (!e.draggable.currentTarget.data("uid"))
                       return;
            
                   var ds = $(e.draggable.element[0]).data().kendoNGLGrid.dataSource;
                   var dataItem = ds.getByUid(e.draggable.currentTarget.data("uid"));
            
                   if (dataItem["SolutionDetailBookControl"] > 0) {
                       var IQgrid = $("#ImportQueue").getKendoNGLGrid();
                       inewTruckBookPK = dataItem["SolutionDetailBookControl"];
                       inewTruckBookPKType = "NO";
                       IQgrid.clearSelection();
                   }
                   if (dataItem["SolutionDetailPOHdrControl"] < 0) {
                       var NOgrid = $("#NewLoadsgrid").getKendoNGLGrid();
                       inewTruckBookPK = dataItem["SolutionDetailPOHdrControl"];
                       inewTruckBookPKType = "IQ";
                       NOgrid.clearSelection();
                   }
                   //console.log(inewTruckBookPK);
                   //console.log(inewTruckBookPKType);
                   if (inewTruckBookPK != 0) {
                       openActCarriersWnd(PageControl);
                   }
               }
            
              // newTruckGrid.wrapper.find('table').attr('data-role', 'droptarget');
               newTruckGrid.wrapper.kendoDropTarget({
                   group: "gridGroup2",
                   dragenter: droptargetOnDragEnter,
                   dragleave: droptargetOnDragLeave,
                   drop: function (e) {
                       createNewCNS(e);
                   }
               });
            
               newTruckGrid.wrapper.kendoDropTarget({
                   group: "gridGroup1",
                   dragenter: droptargetOnDragEnter,
                   dragleave: droptargetOnDragLeave,
                   drop: function (e) {
                       createNewCNS(e);
                   }
               });
            
               $(ImportQueueGrid.element).kendoDraggable({
                   filter: "tr",
                   hint: function (e) {
                       var item = $('<div class="k-grid k-widget" style="background-color: MediumVioletRed; color: black;"><table><tbody><tr> ' + e.html() + '</tr></tbody></table></div>');
                       return item;
                   },
                   group: "gridGroup1"
               });
            
               $(NewLoadsgrid.element).kendoDraggable({
                   filter: "tr",
                   hint: function (e) {
                       var item = $('<div class="k-grid k-widget" style="background-color: MediumVioletRed; color: black;"><table><tbody><tr>' + e.html() + '</tr></tbody></table></div>');
                       return item;
                   },
                   group: "gridGroup1"
               });
              
               grid.table.on("click", ".checkbox" , selectRow);               
            
               var checkedIds = {};
               chkbxIds=[];      
                function selectRow() {  
                    refreshGridly();
                   var checked = this.checked,
                   row = $(this).closest("tr"),
                   dataItem = grid.dataItem(row);
                   checkedIds[dataItem.SolutionTruckConsPrefix] = checked;
                  //console.log("selectRow", dataItem)
                  if (checked) {
                       
                       CreateDynamicGrid(dataItem);
                       //-select the row
                       row.addClass("k-state-selected");
                       chkbxIds+=dataItem.SolutionTruckKey+",";//Saving checkbox selection for the trucks in db    
                   } else {  
                       //Saving checkbox selection for the trucks in db    
                       chkbxIds=chkbxIds.split(',');
                       chkbxIds = jQuery.grep(chkbxIds, function(value) {
                           return value != dataItem.SolutionTruckKey;
                       });
                       chkbxIds=chkbxIds.join(',');
                       //Saving checkbox selection for the trucks in db    //
                       RemoveGrid(dataItem);
                       //-remove selection
                       row.removeClass("k-state-selected");
                   }
                   //Saving checkbox selection for the trucks in db                    
                   //var chkbxTruckKeys = {  SolDetailCompControl: chkbxIds };
                   //console.log(chkbxIds);
                   
                   //saveSelectedTrucks(chkbxIds);
                   TruckKeys = chkbxIds;//Added for PageSettings
                   saveSelectedPageChanges(chkbxIds,TruckPageSize,IsCollapse,TruckSize);//Added for PageSettings
                   //End of Saving checkbox selection for the trucks in db *********8
                   
                   // check related to Cols/Rows showing of Details 
                   let spanMenuIcon = $('span').hasClass('k-i-columns');
                   if (spanMenuIcon == true) {
                       Rotate(true);
                   }
               }
            
               function CreateDynamicGrid(dataItem){
                   //debugger;
                   var gridId = "dy_grid_"+ dataItem.SolutionTruckKey + "___" + dataItem.SolutionTruckCompControl;
                   var divId = "dy_div_" + dataItem.SolutionTruckKey;
                   if ($("#" + divId).length > 0) {
                       return false;
                   }
                   $( "#centerDiv" ).append($("<div id="+divId+" class='center-grid-div width250'><div class='d-flex drag-title'><table id='t" + divId + "' border='0' class='width250 resizetable'><tr class='resizetr'><td class='resizetd' align='center'>"+
                       "<a href='LECarrierMaint?carrControl=" + encodeURIComponent(dataItem.SolutionTruckCarrierControl) + "'><img id='carrierLogo_"+dataItem.SolutionTruckKey + "'  class='resizeImage' /></a></td>" +
                       "<td class='resizetd' ><h3><a target='_blank' href='LoadBoard?BookConsPrefix=" + dataItem.SolutionTruckConsPrefix + "'>" + dataItem.SolutionTruckConsPrefix +"</a> </h3><br/>"+dataItem.SolutionTruckCarrierName+"</td></tr></table></div>" +
                       "<div id="+gridId+" data-cid=" + dataItem.SolutionTruckKey + "></div><br/>"));
                   var imgURL = '../Content/NGL/CarrierLogos/' + dataItem.SolutionTruckCarrierSCAC + 'logo.png';
                   $('#carrierLogo_' + dataItem.SolutionTruckKey).load(imgURL, function(response, status, xhr) {
                       if (status == "error") 
                           $(this).attr('src', '../Content/NGL/CarrierLogos/NGLSlogo.png');
                       else
                           $(this).attr('src', imgURL);
                   });
            
                   $('#centerDiv').gridly();
                   let newobj = consignments.filter(function (x) {
                       return x.SolutionTruckKey == dataItem.SolutionTruckKey
                   })[0];

                   var db1 = [];
                    for (var i = 0; i < newobj.SolutionDetails.length; ++i) {
                       var newsolution = newobj.SolutionDetails[i];
                       //newsolution.SolutionTruckTotalWgt = newsolution.SolutionDetailTotalWgt;
                       db1.push(newsolution);
                   }
                    //Array.prototype.push.apply(db1, newobj.SolutionDetails);
                   var newdatasource = new kendo.data.DataSource({
                       data: db1
                   });
                   var dynamicGrid = $("#"+gridId).kendoNGLGrid({
                       theme: "blueopal",
                       //width: 230,
                       dataSource: newdatasource,
                       rowTemplate: kendo.template($("#rowTemplate").html()),
                       altRowTemplate: kendo.template($("#altRowTemplate").html()),
                       dataBound: function(e){
                           //var gridId1= $(e.sender.element[0]).attr("id");
                           //$('#'+gridId1+' .k-grid-content').height(180);                            
                           var items = e.sender.items();
                           var id = $(e.sender.element[0]).attr("data-cid");
                        
                           let obj = consignments.filter(function (x) {
                               return x.SolutionTruckKey == id
                           })[0];
                          
                           var summary = {
                               Wgt : 0,
                               Pits : 0,
                               Quantity : 0,
                               Volume : 0,
                               Distance: obj["SolutionTruckTotalMiles"],
                               TotalCost: obj["SolutionTruckTotalCost"]
            
                           };
                           items.each(function(){
                               var dt = e.sender.dataItem(this);
                               summary.Wgt += dt.SolutionDetailTotalWgt;
                               summary.Pits += dt.SolutionDetailTotalPL;
                               summary.Quantity += dt.SolutionDetailTotalCases;
                               summary.Volume += dt.SolutionDetailTotalCube;
                               summary.Distance += dt.SolutionDetailDestMiles;
                               summary.TotalCost += dt.SolutionDetailDestPCMCost;
                           });
            
                           var wrapper = e.sender.element.find(".summaryWrapper");
                           for (var prop in summary) {
                               //.toFixed(2);
                               if(prop == "TotalCost") {
                                   wrapper.append("<span>"+ prop + ": $"+summary[prop].toFixed(2) +"</span><br>");
                               }
                               else if(prop == "Distance") {
                                   wrapper.append("<span>"+ prop + ": "+summary[prop].toFixed(2) +"</span><br>");
                               } else {
                                   wrapper.append("<span>"+ prop + ": "+summary[prop]+"</span><br>");
                               }
                           }
                       },
                       columns: [
                         { 
                             field: "SolutionTruckConsPrefix" , title: "",
                             footerTemplate: "<span class='summaryWrapper'></span>" 
                         }
                       ]
                   }).data("kendoNGLGrid");
                   //$(".k-grid .k-grid-header").hide();
                   $(dynamicGrid.element).kendoDraggable({
                       filter: "tr",
                       hint: function (e) {
                           var item = $('<div class="k-grid k-widget" style="background-color: lightskyblue; color: black;"><table><tbody><tr>' + e.html() + '</tr></tbody></table></div>');
                           return item;
                       },
                       group: "gridGroup2",
                   });
            
                   //ImportQueueGrid.wrapper.kendoDropTarget({
                   //    drop: function (e) {
                   //        if($(e.draggable.currentTarget).hasClass("k-footer-template"))
                   //            return;
                   //        var ds = $(e.draggable.element[0]).data().kendoNGLGrid.dataSource;
                   //        var dataItem = ds.getByUid(e.draggable.currentTarget.data("uid"));
                   //        if (dataItem.source == "NewLoadsgrid") {
                   //            dataItem.source = "";
                   //            ds.remove(dataItem);
                   //            vImportQueueGrid365.add(dataItem);
                   //            $('#centerDiv').gridly();
                   //        }
                   //    },
                   //    group: "gridGroup2",
                   //});
            
                   //NewLoadsgrid.wrapper.kendoDropTarget({
                   //    drop: function (e) {
                   //        if($(e.draggable.currentTarget).hasClass("k-footer-template"))
                   //            return;
                   //        var ds = $(e.draggable.element[0]).data().kendoNGLGrid.dataSource;
                   //        var dataItem = ds.getByUid(e.draggable.currentTarget.data("uid"));
                   //        if (dataItem.source == "ImportQueue") {
                   //            dataItem.source = "";
                   //            ds.remove(dataItem);
                   //            vNewLoadsGrid365.add(dataItem);
                   //            $('#centerDiv').gridly();
                   //        }
                   //    },
                   //    group: "gridGroup2",
                   //});
            
                   dynamicGrid.table.kendoDropTarget({
                       group: "gridGroup2",
                       drop: function (e) {
            
                           if($(e.draggable.currentTarget).hasClass("k-footer-template"))
                               return;
                           e.draggable.hint.hide();
                           var ds = $(e.draggable.element[0]).data().kendoNGLGrid.dataSource;
                           var target = ds.getByUid($(e.draggable.currentTarget).data("uid")),
                           dest = $(document.elementFromPoint(e.clientX, e.clientY));
                  
                           if(!target || target == undefined)
                               return;
                           var sourcegridid= $(e.draggable.element[0]).attr("id");
                           var grids = dest.parents(".k-grid");
                           var destgridid = "";
                           if(grids.length > 0){
                               destgridid = $(grids[0]).attr("id");
                           }
                           if (dest.is("th") || !$(e.draggable.currentTarget).is("tr") || $(e.draggable.currentTarget).hasClass("k-footer-template")) {
                               return;
                           }
            
                           if(sourcegridid != destgridid){
                               // will go here if destination grid is not empty
                               var desds = $("#"+destgridid).data().kendoNGLGrid.dataSource;
                               if(!target.get("uid"))
                                   return;
                               var dataItem = ds.getByUid(target.get("uid"));
            
                               var bookcompcontrol = destgridid.replace("dy_grid_", "").split("___");
                               var desttruckKey = bookcompcontrol[0];
                               var CompControl = bookcompcontrol[1];
                               var LoadControl = dataItem["SolutionDetailBookControl"];
                               var OrigtruckKey = sourcegridid.replace("dy_grid_", "").split("___")[0];
                               var OrigCompControl = sourcegridid.replace("dy_grid_", "").split("___")[1];
                               //console.log(dataItem);
                               //console.log(desds);
                               //console.log(CompControl);
                               //console.log(desttruckKey);
                               //console.log(LoadControl);
                               //console.log(OrigCompControl);
                               //console.log(OrigtruckKey);
                               
                               ReassignLoadToTruck(LoadControl, CompControl, desttruckKey, OrigtruckKey, OrigCompControl);
            
                               ds.remove(dataItem);
                               desds.add(dataItem);
            
                               //console.log(ds);
                               //console.log(desds);
                               //console.log("Origin");
                               //console.log(OrigtruckKey);
                               //console.log("Dest");
                               //console.log(desttruckKey);
                             
                               //Invoked to refresh the truck with updated load details once the load is added
                               $.ajax({
                                   url: "api/LoadPlanning/GetLoadsInTruck/",
                                   contentType: "application/json; charset=utf-8",
                                   dataType: 'json',
                                   data: {CompControl: JSON.stringify(OrigCompControl), TruckKey: JSON.stringify(OrigtruckKey)},
                                   headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                                   success: function(data) {
            
                                       if (data.Errors != null) { 
            
                                           if (data.StatusCode === 203){ 
                                               ngl.showErrMsg("Authorization Timeout", data.Errors, null); 
                                           } 
                                           else 
                                           { 
                                               ngl.showErrMsg("Access Denied", data.Errors, null); 
                                           } 
                                       } else {
                                                            
                                           //console.log(data.Data);
                                           ds.data(data.Data);
                                           ds.sync();
                                       } 
                                   },
                                   error: function (xhr, textStatus, error) {
                                       var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure");
                                       ngl.showErrMsg("Load Planning - Orders in Truck", sMsg, null);
                                   }
                               });
            
                               //Invoked to refresh the truck with updated load details once the load is removed
                               $.ajax({
                                   url: "api/LoadPlanning/GetLoadsInTruck/",
                                   contentType: "application/json; charset=utf-8",
                                   dataType: 'json',
                                   data: {CompControl: JSON.stringify(CompControl), TruckKey: JSON.stringify(desttruckKey)},
                                   headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                                   success: function(data) {
            
                                       if (data.Errors != null) { 
            
                                           if (data.StatusCode === 203){ 
                                               ngl.showErrMsg("Authorization Timeout", data.Errors, null); 
                                           } 
                                           else 
                                           { 
                                               ngl.showErrMsg("Access Denied", data.Errors, null); 
                                           } 
                                       } else {
                                                            
                                           //console.log(data.Data);
                                           desds.data(data.Data);
                                           desds.sync();
                                       } 
                                   },
                                   error: function (xhr, textStatus, error) {
                                       var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure");
                                       ngl.showErrMsg("Load Planning - Orders in Truck", sMsg, null);
                                   }
                               });
                           }
                           else{
                               dest = ds.getByUid(dest.parents("tr").data("uid"));
                               //not on same item
                               if (target.get("uid") !== dest.get("uid")) {
                                   //reorder the items
                                   var index = ds.indexOf(target);
                                   var index1 = ds.indexOf(dest);
                                   //console.log(index);
                                   //console.log(index1);
            
                                   var closestGridElement = e.sender.element.closest('[data-role="grid"]');
                                   var id = closestGridElement.attr('id');
                                   var truckcontrol = id.replace("dy_grid_", "").split("___");
                                   var bookControl = truckcontrol[1];
                                   var truckKey = truckcontrol[0];
                                   //console.log(truckKey);
            
                                   ds.data().splice(index1, 0, ds.data().splice(index, 1)[0]);
                                   ds.sync();
            
                                   // Invoked when a load is re-ordered with in same truck the stop/sequence number is saved and recalculated
                                   var dataJSON = { SolDetailBookControl: target["SolutionDetailBookControl"], SolDetailCompControl: target["SolutionDetailCompControl"], solTruckKey:truckKey, drpIndex:index1 };
                                   $.ajax({
                                       type: "POST",
                                       url: "api/LoadPlanning/UpdateLoadIndex/",
                                       contentType: "application/json; charset=utf-8",
                                       dataType: 'json',
                                       data: JSON.stringify(dataJSON),
                                       headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                       success: function(data) {
                                           if (data.Errors != null) {
            
                                               if (data.StatusCode === 203){ngl.showErrMsg("Authorization Timeout", data.Errors, null);}
                                               else {ngl.showErrMsg("Access Denied", data.Errors, null);}
                                           } 
                                           else {
                                               //Invoked to refresh the loads in truck with updated stop/sequence number
                                               var dataLJSON = { SolDetailCompControl: bookControl, solTruckKey:truckKey };
                                               $.ajax({
                                                   url: "api/LoadPlanning/GetLoadsInTruck/",
                                                   contentType: "application/json; charset=utf-8",
                                                   dataType: 'json',
                                                   data: {CompControl: JSON.stringify(bookControl), TruckKey: JSON.stringify(truckKey)},
                                                   headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                                                   success: function(data) {
            
                                                       if (data.Errors != null) { 
            
                                                           if (data.StatusCode === 203){ 
                                                               ngl.showErrMsg("Authorization Timeout", data.Errors, null); 
                                                           } 
                                                           else 
                                                           { 
                                                               ngl.showErrMsg("Access Denied", data.Errors, null); 
                                                           } 
                                                       } else {
                                                            
                                                           //console.log(data.Data);
                                                           ds.data(data.Data);
                                                           ds.sync();
                                                       } 
                                                   },
                                                   error: function (xhr, textStatus, error) {
                                                       var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure");
                                                       ngl.showErrMsg("Load Planning - Orders in Truck", sMsg, null);
                                                   }
                                               });
                                           }
                                       },
                                       error: function (xhr, textStatus, error) {
                                           var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure");
                                           ngl.showErrMsg("Load Planning - Re-Order Load in Truck", sMsg, null);
                                       }
                                   });
                               }
                           }
            
                           $('div#centerDiv div[id^="dy_div_"] .k-grid-content').css({ "overflow": "inherit" });
                       }
                   });
                   
                   //Captures  selected load, resets earlier load and makes it selected load on page for action menu items & stores in page variables
                   dynamicGrid.table.on("click", "tr" , function(e) {
                       row = $(this).closest("tr"),
                       dataItem = dynamicGrid.dataItem(row);
                       console.log("dataItem", dataItem);
                       //Gets truck key of grid in which load is selected
                       var selectedTruck = row.closest(".k-grid").data("kendoNGLGrid").element.attr("id");
                       var selectedLoad = dataItem["SolutionDetailBookControl"];
                       
                       loadBoardGridSelectedRowDataItem.BookControl = dataItem["SolutionDetailBookControl"];
                       loadBoardGridSelectedRowDataItem.BookTranCode = dataItem["SolutionDetailTranCode"];
                       loadBoardGridSelectedRowDataItem.BookDateOrdered = dataItem["SolutionDetailDateOrdered"];
                       loadBoardGridSelectedRowDataItem.BookDateRequired = dataItem["SolutionDetailDateRequired"];
                       loadBoardGridSelectedRowDataItem.BookDateLoad = dataItem["SolutionDetailDateLoad"];
                       loadBoardGridSelectedRowDataItem.BookFinARInvoiceDate = dataItem["SolutionDetailBookFinARInvoiceDate"];
                       loadBoardGridSelectedRowDataItem.BookCarrierControl = dataItem["SolutionDetailCarrierControl"];
                       loadBoardGridSelectedRowDataItem.BookLockAllCosts = dataItem["SolutionDetailLockAllCosts"];
                       loadBoardGridSelectedRowDataItem.BookSHID = dataItem["SolutionDetailBookSHID"];
                       loadBoardGridSelectedRowDataItem.BookCustCompControl = dataItem["SolutionDetailCompControl"];
                       loadBoardGridSelectedRowDataItem.BookODControl = dataItem["SolutionDetailODControl"];
                       loadBoardGridSelectedRowDataItem.CarrierControl = dataItem["SolutionDetailCarrierControl"];
                       loadBoardGridSelectedRowDataItem.CarrierNumber = dataItem["SolutionDetailCarrierNumber"];
            
                       loadBoardGridSelectedRowDataItem.BookCarrOrderNumber = dataItem["SolutionDetailOrderNumber"];
                       loadBoardGridSelectedRowDataItem.BookOrderSequenced = dataItem["SolutionDetailOrderSequence"];
                       loadBoardGridSelectedRowDataItem.BookConsPrefix = dataItem["SolutionDetailConsPrefix"];
                       loadBoardGridSelectedRowDataItem.BookRouteTypeCode = dataItem["SolutionDetailRouteTypeCode"];
                       loadBoardGridSelectedRowDataItem.BookProNumber = dataItem["SolutionDetailProNumber"];
                       loadBoardGridSelectedRowDataItem.RouteGuideControl = dataItem["SolutionDetailRouteGuideControl"];
                       loadBoardGridSelectedRowDataItem.BookCarrTarControl = dataItem["SolutionDetailBookCarrTarControl"];
                       
            
                       
                       
                       
                       //loadBoardGridSelectedRowDataItem.BookDateDelivered = "";
                       loadBoardGridSelectedRowDataItem.BookRouteConsFlag = dataItem["SolutionDetailRouteConsFlag"];
                       //loadBoardGridSelectedRowDataItem.OnCreditHold = 0;
            
                       BookTotalWgt = dataItem["SolutionDetailTotalWgt"];
                       iBookPK = selectedLoad;
                      
                       
                       if (isBookingSelected() === true) {
                          var setting = {name:'pk', value: iBookPK.toString()};
                           var oCRUDCtrl = new nglRESTCRUDCtrl();
                           var blnRet = oCRUDCtrl.update("LoadPlanning/PostPageSetting", setting, tPage, "savePostPageSettingSuccessCallback", "savePostPageSettingAjaxErrorCallback", tPage);
                       }
            
                       iTruckPK = selectedTruck.replace("dy_grid_", "").split("___")[0];
                       iCompControl = dataItem["SolutionDetailCompControl"];
                       iTruckCompControl = selectedTruck.replace("dy_grid_", "").split("___")[1];
                       //console.log(row.closest(".k-grid").data("kendoGrid").element.attr("id"));
                       //console.log(dataItem["SolutionDetailBookControl"]);
                       //tdataItem = dynamicGrid.dataItem(row);
            
                       //Removes the selected class from loads in same truck (kendo grid)
                       //var curgrid = row.closest(".k-grid").data("kendoGrid");
                       //curgrid.tbody.children('tr').removeClass('k-state-selected');
            
                       //Removes the selected class from loads in other trucks (kendo grids)
                       $('[id^=dy_grid]').each(function() {
                           //console.log(this.id);
                           var curgrid = $(this).closest(".k-grid").data("kendoNGLGrid");
                           //console.log(curgrid.attr("id"));
                           if (typeof curgrid !== 'undefined') {
                               curgrid.tbody.children('tr').removeClass('k-state-selected');
                           }
                       });
                       // set the row as selected
                       row.addClass("k-state-selected");
            
                       //divs  = $('.k-grid')
                       //for(ind in divs){
                       //    div = divs[ind];
                       //    //console.log(div.id);
                       //    //do whatever you want
                       //}
                   });
            
                   dynamicGrid.wrapper.kendoDropTarget({
                       drop: function (e) {
                           if($(e.draggable.currentTarget).hasClass("k-footer-template"))
                               return;
                           var truckKey = 0;
                           var bookCompControl = 0;
                           var LoadControl = 0;
                           var bookcompcontrol = "";
                           var ds = $(e.draggable.element[0]).data().kendoNGLGrid.dataSource;
                           var dataItem = ds.getByUid(e.draggable.currentTarget.data("uid"));
                           ds.remove(dataItem);
                           //console.log(dataItem);
                           //This gives the grid identifier
                           var closestGridElement = e.sender.element.closest('[data-role="grid"]');
                           var id = closestGridElement.context.id;
                           //console.log($(e.draggable.element[0]).attr("id"));
                          
                           if ($(e.draggable.element[0]).attr("id") == "NewLoadsgrid") {
                               
                               bookcompcontrol = id.replace("dy_grid_", "").split("___");
                               var gridId = id;
                               var desds = $("#" + id).data().kendoNGLGrid.dataSource;
                               truckKey = bookcompcontrol[0];
                               bookCompControl = bookcompcontrol[1];
                               LoadControl = dataItem["SolutionDetailBookControl"];
                               //console.log(truckKey);
                               //console.log(bookCompControl);
                               //console.log(LoadControl);
                              
                               // Invoked when a load is added to the truck from new loads list
                               AddLoadToTruck(LoadControl, bookCompControl, truckKey);
                               $("#NLcnt").text(vNewLoadsGrid365.view().length);
                               TotLcnt = vNewLoadsGrid365.view().length + vImportQueueGrid365.view().length;
                               $("#TotNLcnt").text(TotLcnt);
            
                               //Invoked to refresh loads in truck with update stop numbers after adding load to truck
                               $.ajax({
                                   url: "api/LoadPlanning/GetLoadsInTruck/",
                                   contentType: "application/json; charset=utf-8",
                                   dataType: 'json',
                                   data: {CompControl: JSON.stringify(bookCompControl), TruckKey: JSON.stringify(truckKey)},
                                   headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                                   success: function(data) {
            
                                       if (data.Errors != null) { 
            
                                           if (data.StatusCode === 203){ 
                                               ngl.showErrMsg("Authorization Timeout", data.Errors, null); 
                                           } 
                                           else 
                                           { 
                                               ngl.showErrMsg("Access Denied", data.Errors, null); 
                                           } 
                                       } else {
                                                            
                                           //console.log(data.Data);
                                           desds.data(data.Data);
                                           desds.sync();
                                       } 
                                   },
                                   error: function (xhr, textStatus, error) {
                                       var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure");
                                       ngl.showErrMsg("Load Planning - Orders in Truck", sMsg, null);
                                   }
                               });
            
            
                           } else {
                              
                               bookcompcontrol = id.replace("dy_grid_", "").split("___");
                               truckKey = bookcompcontrol[0];
                               bookCompControl = bookcompcontrol[1];
                               LoadControl = dataItem["SolutionDetailPOHdrControl"];
                               //console.log(truckKey);
                               //console.log(bookCompControl);
                               //console.log(LoadControl);
            
                               // Invoked when a load is added to the truck from new loads list
                               AddLoadToTruck(LoadControl, bookCompControl, truckKey);
                               $("#ILcnt").text(vImportQueueGrid365.view().length);
                               TotLcnt = vNewLoadsGrid365.view().length + vImportQueueGrid365.view().length;
                               $("#TotNLcnt").text(TotLcnt);
            
                               //Invoked to refresh loads in truck with update stop numbers after adding load to truck
                               $.ajax({
                                   url: "api/LoadPlanning/GetLoadsInTruck/",
                                   contentType: "application/json; charset=utf-8",
                                   dataType: 'json',
                                   data: {CompControl: JSON.stringify(bookCompControl), TruckKey: JSON.stringify(truckKey)},
                                   headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                                   success: function(data) {
            
                                       if (data.Errors != null) { 
            
                                           if (data.StatusCode === 203){ 
                                               ngl.showErrMsg("Authorization Timeout", data.Errors, null); 
                                           } 
                                           else 
                                           { 
                                               ngl.showErrMsg("Access Denied", data.Errors, null); 
                                           } 
                                       } else {
                                                            
                                           //console.log(data.Data);
                                           desds.data(data.Data);
                                           desds.sync();
                                       } 
                                   },
                                   error: function (xhr, textStatus, error) {
                                       var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure");
                                       ngl.showErrMsg("Load Planning - Orders in Truck", sMsg, null);
                                   }
                               });
                           }
                           //debugger;
                           //This gives the datasource name of kendo grid
                           //console.log(newdatasource);
                           dataItem.source = $(e.draggable.element[0]).attr("id");
                           //console.log(dataItem);
                           newdatasource.add(dataItem);
                           $('#centerDiv').gridly();
                       },
                       group: "gridGroup1",
                   });                    
                   dynamicGrid.wrapper.kendoDropTarget({
                       drop: function (e) {
                           if($(e.draggable.currentTarget).hasClass("k-footer-template"))
                               return;
                           if(!e.draggable.currentTarget.data("uid"))
                               return;
                           // will go here if destination grid is empty
                           var ds = $(e.draggable.element[0]).data().kendoNGLGrid.dataSource;
                           var dataItem = ds.getByUid(e.draggable.currentTarget.data("uid"));
                           //console.log(dataItem);
                           ds.remove(dataItem);
                           dataItem.source = $(e.draggable.element[0]).attr("id");
                           newdatasource.add(dataItem);
                           var destgriddata = $(e.sender.element.closest('[data-role="grid"]')).data();
                           //console.log(destgriddata);
                           $('#centerDiv').gridly();
                       },
                       group: "gridGroup2",
                   });
               }
            
               //Chosen order in New Orders grid is set to page level variable and used to create new truck
               $("#NewLoadsgrid tbody").on("click", "tr", function(e) {
                   var rowElement = this;
                   var row = $(rowElement);
                   var grid = $("#NewLoadsgrid").getKendoNGLGrid();
                   var IQgrid = $("#ImportQueue").getKendoNGLGrid();
                   nrow = $(this).closest("tr"),
                   dataItem = grid.dataItem(nrow);
                   //console.log(dataItem);
            
                   if(nrow !== 'undefined') {
                       inewTruckBookPK = dataItem["SolutionDetailBookControl"];
                       inewTruckBookPKType = "NO";
                       IQgrid.clearSelection();
                       console.log(inewTruckBookPK);
                       console.log(inewTruckBookPKType);
                   }
            
                   if (row.hasClass("k-state-selected")) {
            
                       var selected = grid.select();
            
                       selected = $.grep(selected,function(x){
                           var itemToRemove = grid.dataItem(row);
                           var currentItem = grid.dataItem(x);
            
                           return itemToRemove.OrderID != currentItem.OrderID
                       })
            
                       grid.clearSelection();
            
                       grid.select(selected);
            
                       e.stopPropagation();
                   }else{
                       grid.select(row)
            
                       e.stopPropagation();
                   }
               });
               //Chosen order in Import Queue grid is set to page level variable and used to create new truck
               $("#ImportQueue tbody").on("click", "tr", function(e) {
                   var rowElement = this;
                   var row = $(rowElement);
                   var grid = $("#ImportQueue").getKendoNGLGrid();
                   var NOgrid = $("#NewLoadsgrid").getKendoNGLGrid();
                   nrow = $(this).closest("tr"),
                   dataItem = grid.dataItem(nrow);
                   //console.log(dataItem);
            
                   if(nrow !== 'undefined') {
                       inewTruckBookPK = dataItem["SolutionDetailPOHdrControl"];
                       inewTruckBookPKType = "IQ";
                       NOgrid.clearSelection();
                       console.log(inewTruckBookPK);
                       console.log(inewTruckBookPKType);
                   }
            
                   if (row.hasClass("k-state-selected")) {
            
                       var selected = grid.select();
            
                       selected = $.grep(selected,function(x){
                           var itemToRemove = grid.dataItem(row);
                           var currentItem = grid.dataItem(x);
            
                           return itemToRemove.OrderID != currentItem.OrderID
                       })
            
                       grid.clearSelection();
            
                       grid.select(selected);
            
                       e.stopPropagation();
                   }else{
                       grid.select(row)
            
                       e.stopPropagation();
                   }
               });
            
               function RemoveGrid(dataItem) {
                   //debugger;
                   var divId = "dy_div_" + dataItem.SolutionTruckKey;
                   var gridId = "dy_grid_" + dataItem.SolutionTruckKey;
                   //var data = $("#" + gridId).data().kendoGrid.dataSource.data();
                   //data.forEach(function (item, index) {
                   //    console.log(item.source);
                   //    if(item.source == "NewLoadsgrid"){
                   //        item.source = "";
                   //        vNewLoadsGrid365.add(item);
                   //    }
                   //    else if(item.source == "ImportQueue"){
                   //        item.source = "";
                   //        vImportQueueGrid365.add(item);
                   //    }
                   //});
                   $("#" + divId).remove();
                   $("#" + gridId).remove();
                   $('#centerDiv').gridly();
               }
               //Read  Page Settings
               var  pSettings=[];
               $.ajax({ 
                   type: "GET",
                   url: 'api/LoadPlanning/GetPageChanges/', 
                   contentType: 'application/json; charset=utf-8', 
                   dataType: 'json',                     
                   headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                   success: function(data) { 
                       if(data!=null && data.Data.length>0){
                         //retrieving values from page settings                           
                           pSettings=data.Data[0].value.split(',');
                           IsCollapse=pSettings[0];
                           TruckPageSize=pSettings[1];
                           TruckSize=pSettings[2];
                           $('#showTrucks option[value="'+TruckPageSize+'"]').attr("selected", "selected");
                           $('#TruckSize option[value="'+TruckSize+'"]').attr("selected", "selected");
                           for (var i = 0; i <= pSettings.length; i++) {
                               if(i>=3){
                                   initialTruckKeys+=pSettings[i]+",";
                               }
                           }
                           //console.log(pSettings);
                           //console.log(initialTruckKeys);
                           TruckKeys=initialTruckKeys.toString().trim(','); 
                           //console.log(TruckKeys);
                           initialTruckKeys=TruckKeys;
                           if(TruckPageSize!="" && TruckPageSize!='undefined'){
                               refreshGridly();
                           }
                           
                           if(IsCollapse==='True'){
                               ResizePanes();
                           }
                       }
                       if (data.Errors != null) {
                           if (data.StatusCode === 203){ 
                               ngl.showErrMsg("Authorization Timeout", data.Errors, null); 
                           } 
                           else 
                           { 
                               ngl.showErrMsg("Access Denied", data.Errors, null); 
                           }
                       } 
                   },
                   error: function(result) { 
                       options.error(result); 
                   } 
               }); 
               //End of Read  Page Settings ****
            
               function initailizeDefaultGrid(){
                   var data = grid.dataSource._data;
                   data.forEach(function(item){
                       if(initialTruckKeys.indexOf(item.SolutionTruckKey) > -1){
                           var row = grid.tbody.find("tr[data-uid='" + item.uid + "']");
                           var checkbox = $(row).find(".left-grid-row-checkbox");
                           checkbox.click();
                           if(TruckSize!="" && TruckSize!='undefined'){
                               setTruckSize();
                           }
            
                       }
                   });
                  // grid.table.on("click", ".checkbox", selectRow);               
               }
             
                function AddLoadToTruck(LoadControl, CompControl, truckKey) {
                    kendo.ui.progress($(document.body), true);
                   var dataJSON = { SolDetailBookControl: LoadControl, SolDetailCarControl: CompControl, solTruckKey:truckKey, drpIndex:"50" };
                   $.ajax({
                       type: "POST",
                       url: "api/LoadPlanning/LoadItemDropToTruck/",
                       contentType: "application/json; charset=utf-8",
                       dataType: 'json',
                       data: JSON.stringify(dataJSON),
                       headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                       success: function(data) {
                           kendo.ui.progress($(document.body), false);
                           if (data.Errors != null) {

                               if (data.StatusCode === 203) {
                                   ngl.showErrMsg("Authorization Timeout", data.Errors, null);
                               }
                               else {
                                   ngl.showErrMsg("Access Denied", data.Errors, null);
                               }
                           } else {
                               ngl.showSuccessMsg("Sucessfully Added Load To Truck");
                           }
                       },
                       error: function (xhr, textStatus, error) {
                           kendo.ui.progress($(document.body), false);
                           var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure");
                           ngl.showErrMsg("Load Planning - Add Order to Truck", sMsg, null);
                       }
                   });
               }
            
                function ReassignLoadToTruck(LoadControl, CompControl, truckKey, OrigtruckKey, OrigCompControl) {
                    kendo.ui.progress($(document.body), true);
                   var dataJSON = { SolDetailBookControl: LoadControl, SolDetailCompControl: CompControl, solTruckKey:truckKey, drpIndex:"50", origTruckKey: OrigtruckKey, origCompControl: OrigCompControl };
                   $.ajax({
                       type: "POST",
                       url: "api/LoadPlanning/ReassignLoadToTruck/",
                       contentType: "application/json; charset=utf-8",
                       dataType: 'json',
                       data: JSON.stringify(dataJSON),
                       headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                       success: function(data) {
                           kendo.ui.progress($(document.body), false);
                           if (data.Errors != null) {

                               if (data.StatusCode === 203) {
                                   ngl.showErrMsg("Authorization Timeout", data.Errors, null);
                               }
                               else {
                                   ngl.showErrMsg("Access Denied", data.Errors, null);
                               }
                           } else {
                               ngl.showSuccessMsg("Sucessfully Re-assigned load to truck");
                           } 
                       },
                       error: function (xhr, textStatus, error) {
                           kendo.ui.progress($(document.body), false);
                           var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure");
                           ngl.showErrMsg("Load Planning - Add Order to Truck", sMsg, null);
                       }
                   });
               }
            }
             
            setTimeout(function () {var PageReadyJS = <%=PageReadyJS%>; }, 10, this);
                setTimeout(function () {
                    menuTreeHighlightPage(); //must be called after PageReadyJS
                    var divWait = $("#h1Wait");
                    if (typeof (divWait) !== 'undefined') { divWait.hide(); }
                }, 10, this);
            });


         </script>
      </div>
   </body>
</html>