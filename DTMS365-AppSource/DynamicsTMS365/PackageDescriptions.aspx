<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PackageDescriptions.aspx.cs" Inherits="DynamicsTMS365.PackageDescriptions" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head >
        <title>DTMS Package Descriptions</title>         
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
        var oPackageDescGrid = null;
        var wnd = kendo.ui.Window; 
        var tObj = this;
         var tPage = this;           
       

        <% Response.Write(NGLOAuth2); %>

        
         var iPackageDescPK = 0;
        <% Response.Write(PageCustomJS); %>
        function savePostPageSettingSuccessCallback(results){
            //for now do nothing when we save the pk
        }
        function savePostPageSettingSuccessCallbackAjaxErrorCallback(xhr, textStatus, error){
            //for now do nothing when we save the pk
        }

        function savePackageDescPK() {
            try {
                var row = oPackageDescGrid.select();
                if (typeof (row) === 'undefined' || row == null) {              
                    ngl.showValidationMsg("Package Description Required", "Please select a Package Description to continue", tPage);
                    return false;
                }               
                //Get the dataItem for the selected row
                var dataItem = oTariffGrid.dataItem(row);
                if (typeof (dataItem) === 'undefined' || dataItem == null) {              
                    ngl.showValidationMsg("Package Description Required", "Please select a Package Description to continue", tPage);
                    return false;
                }  
                if ("PkgDescControl" in dataItem){
                
                    iPackageDescPK = dataItem.PkgDescControl;
                    var setting = {name:'pk', value: iPackageDescPK.toString()};
                    var oCRUDCtrl = new nglRESTCRUDCtrl();
                    var blnRet = oCRUDCtrl.update("PackageDescription/PostPageSetting", setting, tPage, "savePostPageSettingSuccessCallback", "savePostPageSettingSuccessCallbackAjaxErrorCallback",tPage);
                    return true;
                } else {
                    ngl.showValidationMsg("Package Description Required", "Invlaid Record Identifier, please select a Package Description to continue", tPage);
                    return false;
                }

            } catch (err) {
                ngl.showErrMsg("Cannot Continue", "There was an unexpected error, please reload the screen.  If this continues please contact technical support. Error: " + err.message, tPage);
            }
            
        }

        function execActionClick(btn, proc){            
            if(btn.id == "btnRefresh" ){ 
                oPackageDescGrid.dataSource.read();
            } else if(btn.id == "btnResetCurrentUserConfig"){ resetCurrentUserConfig(PageControl); }
        }

        var blnPackageDescGridChangeBound = false;
        //*************  Call Back Functions ****************
        function PackageDescGridDataBoundCallBack(e,tGrid){           
            oPackageDescGrid = tGrid;
            if (blnPackageDescGridChangeBound == false){
                oPackageDescGrid.bind("change", savePackageDescPK);
                blnPackageDescGridChangeBound = true;
            }   
        }
         $(document).ready(function () {
           
            var PageMenuTab = <%=PageMenuTab%>;
            
            if (control != 0){

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
