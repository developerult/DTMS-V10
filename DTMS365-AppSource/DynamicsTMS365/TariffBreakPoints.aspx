<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TariffBreakPoints.aspx.cs" Inherits="DynamicsTMS365.TariffBreakPoints" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head >
    <title>DTMS Tariff Break Points</title>        
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

        
         var oTariffBreakPointGrid = null;
         <% Response.Write(PageCustomJS); %>
         function execActionClick(btn, proc){
             if(btn.id == "btnOpenContract" ){ location.href = "Tariff";
             }else if (btn.id == "btnOpenServices" ){ location.href = "TariffServices";
             }else if (btn.id == "btnOpenRates" ){ location.href = "TariffRates";
             }else if (btn.id == "btnOpenExceptions" ){ location.href = "TariffExceptions";
             }else if (btn.id == "btnOpenFees" ){ location.href = "TariffFees";
             }else if (btn.id == "btnOpenFuel" ){ location.href = "TariffFuel";
             }else if (btn.id == "btnOpenNoDrive" ){ location.href = "TariffNoDriveDays";
             }else if (btn.id == "btnOpenHDMs" ){ location.href = "TariffHDMs";
             }else if (btn.id == "btnRefresh" ){                                                 
                 wdgtvCarrierTariffSummarySummary.read(0);
                 wdgtNGLEditOnPageWdgtEdit.read(0);
             }  
             if(btn.id == "btnResetCurrentUserConfig"){
                 resetCurrentUserConfig(PageControl);
             }
         }

         //*************  Call Back Functions ****************
         function TariffBreakPointGridDataBoundCallBack(e,tGrid){           
             oTariffBreakPointGrid = tGrid;
         }
       
         
         $(document).ready(function () {
             var PageMenuTab = <%=PageMenuTab%>;
                      
            
            if (control != 0){
                setTimeout(function () {
                    //add code here to load screen specific information this is only visible when a user is authenticated
                }, 1,this);

            }
            setTimeout(function () {var PageReadyJS = <%=PageReadyJS%>}, 1,this);
             setTimeout(function () {
               menuTreeHighlightPage(); //must be called after PageReadyJS
                var divWait = $("#h1Wait");
                if  (typeof (divWait) !== 'undefined' ) {
                    divWait.hide();
                }
            }, 1, this);

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