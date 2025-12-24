<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GlobalListMaint.aspx.cs" Inherits="DynamicsTMS365.GlobalListsMaint" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head >
    <title>DTMS Global List</title>       
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

        
         var oCarrierEquipCodesGrid = null;
         var oChartOfAccountGrid = null;
         var oTariffInterlineGrid = null;
         var oTariffMinChargeGrid = null;
         var oTariffMinWeightGrid = null;
         var oTariffNonServiceGrid = null;
        

         function toolbar_click(){
             alert('hello');
         }
         <% Response.Write(PageCustomJS); %>
         function execActionClick(btn, proc){
             if (btn.id == "btnRefresh" ){
                 if (oCarrierEquipCodesGrid) { oCarrierEquipCodesGrid.dataSource.read(); }
                 if (oChartOfAccountGrid) { oChartOfAccountGrid.dataSource.read(); }
             }    
             if(btn.id == "btnResetCurrentUserConfig"){
                 resetCurrentUserConfig(PageControl);
             }
         }

         
         //************* Begin Call Back Functions ****************
         function CarrierEquipCodesGridDataBoundCallBack(e,tGrid){           
             oCarrierEquipCodesGrid = tGrid;
         }

         function ChartOfAccountGridDataBoundCallBack(e, tGrid) {
             oChartOfAccountGrid = tGrid;
         }
         //************* End Call Back Functions ****************
               
         
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
                if  (typeof (divWait) !== 'undefined' ) {
                    divWait.hide();
                }
            //}, 1, this);

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
