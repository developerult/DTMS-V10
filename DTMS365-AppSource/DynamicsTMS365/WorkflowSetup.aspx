<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkflowSetup.aspx.cs" Inherits="DynamicsTMS365.WorkflowSetup" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head >
    <title>DTMS Workflow Setup</title>        
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
         var oWorkflowSetupGrid = null;
         var oWorkflowParProcessGrid = null;
         var oWorkflowParProcessOptionGrid = null;
         var oWorkflowParProcOptTxtItemGrid = null;
         var oWorkflowParProcOptValItemGrid = null;
         // grid processing parameters
         var blnWorkflowSetupGridChangeBound = false;
         var blnWorkflowParProcessGridChangeBound = false;
         var blnWorkflowParProcessOptionGridChangeBound = false;
         var blnWorkflowParProcOptTxtItemGridChangeBound = false;
         var blnWorkflowParProcOptValItemGridChangeBound = false;
         // data primary keys parents and children       
         var iWorkflowSetupPK = 0;
         var iWorkflowParProcessPK = 0;
         var iWorkflowParProcessOptionPK = 0;
         var iWorkflowParProcOptTxtItemPK = 0;
         var iWorkflowParProcOptValItemPK = 0;

         var wnd, detailsTemplate;


        <% Response.Write(NGLOAuth2); %>

         
         <% Response.Write(PageCustomJS); %>

         
         
         function savePostPageSettingSuccessCallback(results){
            // wdgtWorkflowSetupummarySummary.read(iWorkflowSetupPK);
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
                                         if(typeof (iWorkflowSetupPK) === 'undefined' || iWorkflowSetupPK == null || iWorkflowSetupPK === 0){ iWorkflowSetupPK = userPageSettings.value; }                                 
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
                         var blnRet = oCRUDCtrl.update("WorkflowSetup/PostPageSetting", setting, tPage, "savePostPageSettingSuccessCallback", "savePostPageSettingSuccessCallbackAjaxErrorCallback", tPage);
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
                 oWorkflowSetupGrid.dataSource.read();
                 // check if we need to refresh all the child data?
                 //oWorkflowParProcessGrid.dataSource.read();
                 //oWorkflowParProcessOptionGrid.dataSource.read();
                 //oWorkflowParProcOptTxtItemGrid.dataSource.read();
                 //oWorkflowParProcOptValItemGrid.dataSource.read();

             } else if(btn.id == "btnResetCurrentUserConfig"){ resetCurrentUserConfig(PageControl); }
         }
         
         //************* Begin Grid Configuration: Call Back Functions and primary key selection methods 
         // Note: grid names and primary key names must match the content management configuration and the controller class for each grid
         //tblParCategory oWorkflowSetupGrid key name = pk
         function WorkflowSetupGridDataBoundCallBack(e,tGrid){           
             oWorkflowSetupGrid = tGrid;
             if (blnWorkflowSetupGridChangeBound == false){
                 oWorkflowSetupGrid.bind("change", saveWorkflowSetupPK);
                 blnWorkflowSetupGridChangeBound = true;
             }
             //if iWorkflowSetupPK is not 0 select that row in the grid
             if (typeof (iWorkflowSetupPK) !== 'undefined' && iWorkflowSetupPK !== null && iWorkflowSetupPK !== 0) {
                 var rows = oWorkflowSetupGrid.items();
                 $(rows).each(function(e) {
                     var row = this;
                     var dataItem = oWorkflowSetupGrid.dataItem(row);
                     if (dataItem.RunTaskControl == iWorkflowSetupPK) { 
                         oWorkflowSetupGrid.select(row); 
                         return false;
                     }
                 });
             }

             //example of how to read data from the widget?
             //lblBalanceID =  wdgtWorkflowSetupGridEdit.GetFieldID("lblBalanceTotal");
             //wdgtWorkflowSetupGridEdit.SetFieldDefault("lblBalanceTotal",'$100.00');
             //calculate_balance('$100.00');
         }
         //save primary key for tblParCategory as pk located in selected oWorkflowSetupGrid
         function saveWorkflowSetupPK() {
             return saveWorkflowPK(oWorkflowSetupGrid, "Workflow Setup Type", "Please select a Workflow Setup Type Record to continue.", "pk", iWorkflowSetupPK, "ParCatControl");
         }
         function isWorkflowSetupSelected() {
             if (typeof (iWorkflowSetupPK) === 'undefined' || iWorkflowSetupPK === null || iWorkflowSetupPK === 0) {
                 return saveWorkflowSetupPK();
             }
             return true;
         }

         //tblParProcess oWorkflowParProcessGrid key name = ParProcControl
         function WorkflowParProcessGridDataBoundCallBack(e, tGrid) {
             oWorkflowParProcessGrid = tGrid;
             //alert("ParProcessGridDataBound");
             if (blnWorkflowParProcessGridChangeBound == false) {
                 oWorkflowParProcessGrid.bind("change", saveWorkflowParProcessPK);
                 blnWorkflowParProcessGridChangeBound = true;
             }
             //if iWorkflowParProcessPK is not 0 select that row in the grid
             if (typeof (iWorkflowParProcessPK) !== 'undefined' && iWorkflowParProcessPK !== null && iWorkflowParProcessPK !== 0) {
                 var rows = oWorkflowParProcessGrid.items();
                 $(rows).each(function (e) {
                     var row = this;
                     var dataItem = oWorkflowParProcessGrid.dataItem(row);
                     if (dataItem.ParProcControl == iWorkflowParProcessPK) {
                         oWorkflowParProcessGrid.select(row);
                         return false;
                     }
                 });
             }

             // for auto refresh of grid we must set the crlKendoNGLGrid = to the current grid identified by the call back
             //  this addresses the issue for nested tab strip controls where the grids are creatd dynamically and may not 
             //  have a static ID.  All variable names must match content management naming conventions
             wdgtWorkflowParProcessGridEdit.ctrlKendoNGLGrid = oWorkflowParProcessGrid;

         }
         //save primary key for tblParProcess as ParProcControl located in selected oWorkflowParProcessGrid
         function saveWorkflowParProcessPK() {            
             return saveWorkflowPK(oWorkflowParProcessGrid, "Workflow Process", "Please select a Workflow Process Record to continue.", "ParProcControl", iWorkflowParProcessPK, "ParProcControl");
         }
         function isWorkflowSetupSelected() {
             if (typeof (iWorkflowSetupPK) === 'undefined' || iWorkflowParProcessPK === null || iWorkflowParProcessPK === 0) {
                 return saveWorkflowParProcessPK();
             }
             return true;
         }

         //tblParProcessOption oWorkflowParProcessOptionGrid key name = ParProcOptControl
         function WorkflowParProcessOptionGridDataBoundCallBack(e, tGrid) {
             oWorkflowParProcessOptionGrid = tGrid;
             if (blnWorkflowParProcessOptionGridChangeBound == false) {
                 oWorkflowParProcessOptionGrid.bind("change", saveWorkflowParProcessOptionPK);
                 blnWorkflowParProcessOptionGridChangeBound = true;
             }
             //if iWorkflowParProcessOptionPK is not 0 select that row in the grid
             if (typeof (iWorkflowParProcessOptionPK) !== 'undefined' && iWorkflowParProcessOptionPK !== null && iWorkflowParProcessOptionPK !== 0) {
                 var rows = oWorkflowParProcessOptionGrid.items();
                 $(rows).each(function (e) {
                     var row = this;
                     var dataItem = oWorkflowParProcessOptionGrid.dataItem(row);
                     if (dataItem.ParProcOptControl == iWorkflowParProcessOptionPK) {
                         oWorkflowParProcessOptionGrid.select(row);
                         return false;
                     }
                 });
             }
             // for auto refresh of grid we must set the crlKendoNGLGrid = to the current grid identified by the call back
             //  this addresses the issue for nested tab strip controls where the grids are creatd dynamically and may not 
             //  have a static ID.  All variable names must match content management naming conventions
             wdgtWorkflowParProcessOptionGridEdit.ctrlKendoNGLGrid = oWorkflowParProcessOptionGrid;

         }
         //save primary key for tblParProcessOption as ParProcOptControl located in selected oWorkflowParProcessOptionGrid
         function saveWorkflowParProcessOptionPK() {
             return saveWorkflowPK(oWorkflowParProcessOptionGrid, "Workflow Process Option", "Please select a Workflow Process Option Record to continue.", "ParProcOptControl", iWorkflowParProcessOptionPK, "ParProcOptControl");
         }
         function isWorkflowSetupSelected() {
             if (typeof (iWorkflowSetupPK) === 'undefined' || iWorkflowParProcessOptionPK === null || iWorkflowParProcessOptionPK === 0) {
                 return saveWorkflowParProcessOptionPK();
             }
             return true;
         }

         //tblParProcOptTxtItem oWorkflowParProcOptTxtItemGrid key name = ParProcOptTIControl
         function WorkflowParProcOptTxtItemGridDataBoundCallBack(e, tGrid) {
             oWorkflowParProcOptTxtItemGrid = tGrid;
             if (blnWorkflowParProcOptTxtItemGridChangeBound == false) {
                 oWorkflowParProcOptTxtItemGrid.bind("change", saveWorkflowParProcOptTxtItemPK);
                 blnWorkflowParProcOptTxtItemGridChangeBound = true;
             }
             //if iWorkflowParProcOptTxtItemPK is not 0 select that row in the grid
             if (typeof (iWorkflowParProcOptTxtItemPK) !== 'undefined' && iWorkflowParProcOptTxtItemPK !== null && iWorkflowParProcOptTxtItemPK !== 0) {
                 var rows = oWorkflowParProcOptTxtItemGrid.items();
                 $(rows).each(function (e) {
                     var row = this;
                     var dataItem = oWorkflowParProcOptTxtItemGrid.dataItem(row);
                     if (dataItem.ParProcOptTIControl == iWorkflowParProcOptTxtItemPK) {
                         oWorkflowParProcOptTxtItemGrid.select(row);
                         return false;
                     }
                 });
             }

         }
         //save primary key for tblParProcOptTxtItem as ParProcOptTIControl located in selected oWorkflowParProcOptTxtItemGrid
         function saveWorkflowParProcOptTxtItemPK() {
             return saveWorkflowPK(oWorkflowParProcOptTxtItemGrid, "Workflow Process", "Please select a Workflow Process Record to continue.", "ParProcOptTIControl", iWorkflowParProcOptTxtItemPK, "ParProcOptTIControl");
         }
         function isWorkflowSetupSelected() {
             if (typeof (iWorkflowSetupPK) === 'undefined' || iWorkflowParProcOptTxtItemPK === null || iWorkflowParProcOptTxtItemPK === 0) {
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
         function isWorkflowSetupSelected() {
             if (typeof (iWorkflowSetupPK) === 'undefined' || iWorkflowParProcOptValItemPK === null || iWorkflowParProcOptValItemPK === 0) {
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
                getPageSettings(tPage, "WorkflowSetup", "pk", false);
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