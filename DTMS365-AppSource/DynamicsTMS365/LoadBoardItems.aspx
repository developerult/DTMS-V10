<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoadBoardItems.aspx.cs" Inherits="DynamicsTMS365.LoadBoardItems" %>

<!DOCTYPE html>


<html xmlns="http://www.w3.org/1999/xhtml">
<head >
    <title>DTMS Load Board Items</title>        
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
         //Modified by RHR for v-8.5.4.004 on 12/06/2023 new Key table properties
         //used for query string links
         // not all values are used on all pages
         var BookControlKey = '<%=BookControlKey%>';
         var LaneControlKey = '<%=LaneControlKey%>';
         var CarrierControlKey = '<%=CarrierControlKey%>';
         var CompControlKey = '<%=CompControlKey%>';
         var TariffControlKey = '<%=TariffControlKey%>';

         var tObj = this;
         var tPage = this;
         var oLoadBoardItems = null;  

        <% Response.Write(NGLOAuth2); %>

        

         function execBeforeLoadBoardItemsGridInsert(){
             // prepare for new record like: add code to update the default values for specific fields
             // see TariffRates.aspx for examples
             return true;
           
         }
         <% Response.Write(PageCustomJS); %>
         function execActionClick(btn, proc) {
             if (btn.id == "btnRefresh") { refresh(); }
             else if (btn.id == "btnResetCurrentUserConfig") { resetCurrentUserConfig(PageControl); }
             else { ngl.pgNavigation(btn.id, true, BookControlKey); }// modified by RHR for v-8.5.2.006 on 12/22/2022 added page navigation menu method
         }
         // modified by RHR for v-8.5.2.006 on 12/22/2022 added refresh method
         function refresh() {
             ngl.readDataSource(oLoadBoardItems);
         }

         //*************  Call Back Functions ****************
         function LoadBoardItemsGridDataBoundCallBack(e,tGrid){           
             oLoadBoardItems = tGrid;
             //Modified by RHR for v-8.5.4.004 on 12/06/2023 new Key table properties
             if (BookControlKey && BookControlKey != 0) {
                 if (wdgtvLoadBoardSummarySummary) { wdgtvLoadBoardSummarySummary.read(BookControlKey); }
             }
         }
           
         // widget call back         
         function LoadBoardItemsGridCB(oResults){   
             if (!oResults) { return;}
             if (oResults.source == "showWidgetCallback"   ){
                 //process any logic needed when the widget is displayed (opened)                 
             }             
         }
         
         // ************** End Call Back functions Functions ******************
       
         
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