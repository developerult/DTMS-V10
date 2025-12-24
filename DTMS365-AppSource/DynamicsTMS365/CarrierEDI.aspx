<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CarrierEDI.aspx.cs" Inherits="DynamicsTMS365.CarrierEDI" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>DTMS Carrier EDI</title>
    <%=cssReference%>
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
    </style>
</head>
<body>
    <%=jssplitter2Scripts%>
    <%=sWaitMessage%>
    <div id="example" style="height: 100%; width: 100%;  margin-top: 2px;">
        <div id="vertical" style="height: 98%; width: 98%; ">
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
         var oCarrierEDIGrid = null;
         var tObj = this;
         var tPage = this;
         var oCarrierEquipList = null;

        <% Response.Write(NGLOAuth2); %>


         <% Response.Write(PageCustomJS); %>
         function execActionClick(btn, proc){
             //if(btn.id == "btnDeleteCarEDI"){
             //    if (typeof (wdgtCarrierEDIEdit) !== 'undefined' && ngl.isObject(wdgtCarrierEDIEdit) === true) {
             //        wdgtCarrierEDIEdit.delete();
             //    }

             //}

              if (btn.id == "btnRefresh") {
                  ngl.readDataSource(oCarrierEDIGrid);//oCarrierEDIGrid.dataSource.read();
             }
             else if(btn.id == "btnResetCurrentUserConfig"){
                 resetCurrentUserConfig(PageControl);
             }
             else if (btn.id == "btnOpenCarrier") {
                 location.href="LECarrierMaint";
             }
         }

         $(document).ready(function () {
             var PageMenuTab = <%=PageMenuTab%>;



            if (control != 0){
                //setTimeout(function () {
                //    //add code here to load screen specific information this is only visible when a user is authenticated
                //}, 1,this);

            }

           var PageReadyJS = <%=PageReadyJS%>;
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
        </style>

    </div>
</body>
</html>



