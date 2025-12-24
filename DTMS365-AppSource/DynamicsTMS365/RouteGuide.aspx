<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RouteGuide.aspx.cs" Inherits="DynamicsTMS365.RouteGuide" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head >
    <title>DTMS Route Guide</title>        
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
         var wnd = kendo.ui.Window;
         var tObj = this;
         var tPage = this;
         // data grids parents and children
         var oRouteGuideGrid = null;
         var oRouteGuideCarrGrid = null;
         var oRouteGuideEquipGrid = null;
         // grid processing parameters
         var blnRouteGuideGridChangeBound = false;
         var blnRouteGuideCarrGridChangeBound = false;
         var blnRouteGuideEquipGridChangeBound = false;
         // data primary keys parents and children       
         var iRouteGuidePK = 0;
         var iRouteGuideCarrPK = 0;
         var iRouteGuideEquipPK = 0;

         var wnd, detailsTemplate;


        <% Response.Write(NGLOAuth2); %>

         
         <% Response.Write(PageCustomJS); %>

         
         
         function savePostPageSettingSuccessCallback(results){
            // wdgtRouteGuideummarySummary.read(iRouteGuidePK);
         }
         function savePostPageSettingSuccessCallbackAjaxErrorCallback(xhr, textStatus, error){
             //for now do nothing when we save the pk
         }

         
         function readGetPageSettingSuccessCallback(data) {
             var oResults = new nglEventParameters();
             var tObj = this;
             oResults.source = "readGetPageSettingSuccessCallback";
             oResults.msg = 'Failed'; //set default to Failed         
             oResults.CRUD = "read";
             oResults.widget = tObj;
             var userPageSettings = null;                          
             try {                      
                 if (typeof (data) !== 'undefined' && ngl.isObject(data)) {                          
                     if (ngl.stringHasValue(data.Errors)) { ngl.showErrMsg("Get User Page Settings Failure", data.Errors, null); }                          
                     else {                               
                         if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {                                   
                             if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                 oResults.msg = "Success";
                                 userPageSettings = data.Data[0];                                 
                                 if(typeof (userPageSettings) !== 'undefined' && userPageSettings != null && userPageSettings.value != undefined){
                                     if (userPageSettings.name === "pk"){ 
                                         if(typeof (iRouteGuidePK) === 'undefined' || iRouteGuidePK == null || iRouteGuidePK === 0){ iRouteGuidePK = userPageSettings.value; }                                 
                                     }                                                                                                                                  
                                 }                                   
                             }                              
                         }                            
                     }                        
                 }                                       
             } catch (err) { ngl.showErrMsg(err.name, err.description, null); }             
         }

         function readGetPageSettingAjaxErrorCallback(xhr, textStatus, error) {
             var oResults = new nglEventParameters();
             var tObj = this;
             oResults.source = "readGetPageSettingAjaxErrorCallback";
             oResults.msg = 'Failed'; //set default to Failed        
             oResults.CRUD = "read"
             oResults.widget = tObj;
             oResults.error = new Error();
             oResults.error.name = "Read Page Settings Failure"
             oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
             ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj); 
         }

         function saveWorkflowPK(grid,sVTitle, sVMsg, sKeyName,iKeyParameter, sKeyColumn) {
             try {
                 
                 if (grid) {
                     if (typeof (sVTitle) === 'undefined' || sVTitle == null) { sVTitle = "Workflow Error"; }
                     if (typeof (sVMsg) === 'undefined' || sVMsg == null) { sVMsg = "Please select a Workflow Record to continue"; }

                     var row = grid.select();
                     if (typeof (row) === 'undefined' || row == null) {
                         ngl.showValidationMsg(sVTitle, sVMsg, tPage);
                         return false;
                     }
                     //Get the dataItem for the selected row
                     var dataItem = grid.dataItem(row);
                     if (typeof (dataItem) === 'undefined' || dataItem == null) {
                         ngl.showValidationMsg(sVMsg, sVMsg, tPage);
                         return false;
                     }
                     if (sKeyColumn in dataItem) {
                         
                         // this line was replaced by the find jquery method below  iKeyParameter = dataItem.RunTaskControl;
                         iKeyParameter = dataItem[sKeyColumn];
                         var setting = { name: sKeyName, value: iKeyParameter.toString() };
                         var oCRUDCtrl = new nglRESTCRUDCtrl();
                         var blnRet = oCRUDCtrl.update("RouteGuide/PostPageSetting", setting, tPage, "savePostPageSettingSuccessCallback", "savePostPageSettingSuccessCallbackAjaxErrorCallback", tPage);
                         return true;
                     } else {
                         ngl.showValidationMsg("Cannot Continue", "The workflow primary key data is not available, please reload the screen.  If this continues please contact technical support.", tPage);
                         return false;
                     }

                 } else {
                     ngl.showErrMsg("Cannot Continue", "The workflow data is not available, please reload the screen.  If this continues please contact technical support.", tPage);
                     return false;
                 }
             } catch (err) {
                 ngl.showErrMsg("Cannot Continue", "There was an unexpected error, please reload the screen.  If this continues please contact technical support. Error: " + err.message, tPage);
                 return false;
             }

         }
         



         function execActionClick(btn, proc){            
             if(btn.id == "btnRefresh" ){ 
                 oRouteGuideGrid.dataSource.read();
                 // check if we need to refresh all the child data?
                 //oRouteGuideCarrGrid.dataSource.read();
                 //oRouteGuideEquipGrid.dataSource.read();

             } else if(btn.id == "btnResetCurrentUserConfig"){ resetCurrentUserConfig(PageControl); }
         }
         
         //************* Begin Grid Configuration: Call Back Functions and primary key selection methods 
         // Note: grid names and primary key names must match the content management configuration and the controller class for each grid
         //tblParCategory oRouteGuideGrid key name = pk
         function RouteGuideGridDataBoundCallBack(e,tGrid){           
             oRouteGuideGrid = tGrid;
             if (blnRouteGuideGridChangeBound == false){
                 oRouteGuideGrid.bind("change", saveRouteGuidePK);
                 blnRouteGuideGridChangeBound = true;
             }
             //if iRouteGuidePK is not 0 select that row in the grid
             if (typeof (iRouteGuidePK) !== 'undefined' && iRouteGuidePK !== null && iRouteGuidePK !== 0) {
                 var rows = oRouteGuideGrid.items();
                 $(rows).each(function(e) {
                     var row = this;
                     var dataItem = oRouteGuideGrid.dataItem(row);
                     if (dataItem.RunTaskControl == iRouteGuidePK) { 
                         oRouteGuideGrid.select(row); 
                         return false;
                     }
                 });
             }

             //example of how to read data from the widget?
             //lblBalanceID =  wdgtRouteGuideGridEdit.GetFieldID("lblBalanceTotal");
             //wdgtRouteGuideGridEdit.SetFieldDefault("lblBalanceTotal",'$100.00');
             //calculate_balance('$100.00');
         }
         //save primary key for tblParCategory as pk located in selected oRouteGuideGrid
         function saveRouteGuidePK() {
             return saveWorkflowPK(oRouteGuideGrid, "Workflow Setup Type", "Please select a Workflow Setup Type Record to continue.", "pk", iRouteGuidePK, "ParCatControl");
         }
         function isRouteGuideSelected() {
             if (typeof (iRouteGuidePK) === 'undefined' || iRouteGuidePK === null || iRouteGuidePK === 0) {
                 return saveRouteGuidePK();
             }
             return true;
         }

         //tblParProcess oRouteGuideCarrGrid key name = ParProcControl
         function RouteGuideCarrGridDataBoundCallBack(e, tGrid) {
             oRouteGuideCarrGrid = tGrid;
             //alert("ParProcessGridDataBound");
             if (blnRouteGuideCarrGridChangeBound == false) {
                 oRouteGuideCarrGrid.bind("change", saveRouteGuideCarrPK);
                 blnRouteGuideCarrGridChangeBound = true;
             }
             //if iRouteGuideCarrPK is not 0 select that row in the grid
             if (typeof (iRouteGuideCarrPK) !== 'undefined' && iRouteGuideCarrPK !== null && iRouteGuideCarrPK !== 0) {
                 var rows = oRouteGuideCarrGrid.items();
                 $(rows).each(function (e) {
                     var row = this;
                     var dataItem = oRouteGuideCarrGrid.dataItem(row);
                     if (dataItem.ParProcControl == iRouteGuideCarrPK) {
                         oRouteGuideCarrGrid.select(row);
                         return false;
                     }
                 });
             }

             // for auto refresh of grid we must set the crlKendoNGLGrid = to the current grid identified by the call back
             //  this addresses the issue for nested tab strip controls where the grids are creatd dynamically and may not 
             //  have a static ID.  All variable names must match content management naming conventions
             wdgtRouteGuideCarrGridEdit.ctrlKendoNGLGrid = oRouteGuideCarrGrid;

         }
         //save primary key for tblParProcess as ParProcControl located in selected oRouteGuideCarrGrid
         function saveRouteGuideCarrPK() {            
             return saveWorkflowPK(oRouteGuideCarrGrid, "Workflow Process", "Please select a Workflow Process Record to continue.", "ParProcControl", iRouteGuideCarrPK, "ParProcControl");
         }
         function isRouteGuideSelected() {
             if (typeof (iRouteGuidePK) === 'undefined' || iRouteGuideCarrPK === null || iRouteGuideCarrPK === 0) {
                 return saveRouteGuideCarrPK();
             }
             return true;
         }

         //tblParProcessOption oRouteGuideEquipGrid key name = ParProcOptControl
         function RouteGuideEquipGridDataBoundCallBack(e, tGrid) {
             oRouteGuideEquipGrid = tGrid;
             if (blnRouteGuideEquipGridChangeBound == false) {
                 oRouteGuideEquipGrid.bind("change", saveRouteGuideEquipPK);
                 blnRouteGuideEquipGridChangeBound = true;
             }
             //if iRouteGuideEquipPK is not 0 select that row in the grid
             if (typeof (iRouteGuideEquipPK) !== 'undefined' && iRouteGuideEquipPK !== null && iRouteGuideEquipPK !== 0) {
                 var rows = oRouteGuideEquipGrid.items();
                 $(rows).each(function (e) {
                     var row = this;
                     var dataItem = oRouteGuideEquipGrid.dataItem(row);
                     if (dataItem.ParProcOptControl == iRouteGuideEquipPK) {
                         oRouteGuideEquipGrid.select(row);
                         return false;
                     }
                 });
             }
             // for auto refresh of grid we must set the crlKendoNGLGrid = to the current grid identified by the call back
             //  this addresses the issue for nested tab strip controls where the grids are creatd dynamically and may not 
             //  have a static ID.  All variable names must match content management naming conventions
             wdgtRouteGuideEquipGridEdit.ctrlKendoNGLGrid = oRouteGuideEquipGrid;

         }
         //save primary key for tblParProcessOption as ParProcOptControl located in selected oRouteGuideEquipGrid
         function saveRouteGuideEquipPK() {
             return saveWorkflowPK(oRouteGuideEquipGrid, "Workflow Process Option", "Please select a Workflow Process Option Record to continue.", "ParProcOptControl", iRouteGuideEquipPK, "ParProcOptControl");
         }
         function isRouteGuideSelected() {
             if (typeof (iRouteGuidePK) === 'undefined' || iRouteGuideEquipPK === null || iRouteGuideEquipPK === 0) {
                 return saveRouteGuideEquipPK();
             }
             return true;
         }

         
         function isRouteGuideSelected() {
             if (typeof (iRouteGuidePK) === 'undefined' || iWorkflowParProcOptTxtItemPK === null || iWorkflowParProcOptTxtItemPK === 0) {
                 return saveWorkflowParProcOptTxtItemPK();
             }
             return true;
         }

         //tblParProcOptValItem oWorkflowParProcOptValItemGrid key name = ParProcOptVIControl
         function WorkflowParProcOptValItemGridDataBoundCallBack(e, tGrid) {
             oWorkflowParProcOptValItemGrid = tGrid;
             if (blnWorkflowParProcOptValItemGridChangeBound == false) {
                 oWorkflowParProcOptValItemGrid.bind("change", saveWorkflowParProcOptValItemPK);
                 blnWorkflowParProcOptValItemGridChangeBound = true;
             }
             //if iWorkflowParProcOptValItemPK is not 0 select that row in the grid
             if (typeof (iWorkflowParProcOptValItemPK) !== 'undefined' && iWorkflowParProcOptValItemPK !== null && iWorkflowParProcOptValItemPK !== 0) {
                 var rows = oWorkflowParProcOptValItemGrid.items();
                 $(rows).each(function (e) {
                     var row = this;
                     var dataItem = oWorkflowParProcOptValItemGrid.dataItem(row);
                     if (dataItem.ParProcOptVIControl == iWorkflowParProcOptValItemPK) {
                         oWorkflowParProcOptValItemGrid.select(row);
                         return false;
                     }
                 });
             }

         }
         //save primary key for tblParProcOptValItem as ParProcOptVIControl located in selected oWorkflowParProcOptValItemGrid
         function saveWorkflowParProcOptValItemPK() {
             return saveWorkflowPK(oWorkflowParProcOptValItemGrid, "Workflow Process", "Please select a Workflow Process Record to continue.", "ParProcOptVIControl", iWorkflowParProcOptValItemPK, "ParProcOptVIControl");
         }
         function isRouteGuideSelected() {
             if (typeof (iRouteGuidePK) === 'undefined' || iWorkflowParProcOptValItemPK === null || iWorkflowParProcOptValItemPK === 0) {
                 return saveWorkflowParProcOptValItemPK();
             }
             return true;
         }

         //************* End Grid Configuration: Call Back Functions and primary key selection methods 

         $(document).ready(function () {
             var PageMenuTab = <%=PageMenuTab%>;
                      
           
            if (control != 0){
                //setTimeout(function () {
                //    //add code here to load screen specific information this is only visible when a user is authenticated
                //}, 1,this);

                //read the current selected primary key for this user so we can select it in the grid
                getPageSettings(tPage, "RouteGuide", "pk", false);
            }
            
           var PageReadyJS = <%=PageReadyJS%>;
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