<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MailLog.aspx.cs" Inherits="DynamicsTMS365.MailLog" %>

<!DOCTYPE html>


<html xmlns="http://www.w3.org/1999/xhtml">
<head >
    <title>DTMS Mail Log Manager</title>        
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
         var oMailLogGrid = null;
         var wnd = kendo.ui.Window; 
         var tObj = this;
         var tPage = this;
         var iMailLogPK = 0;

        <% Response.Write(NGLOAuth2); %>

         
         <% Response.Write(PageCustomJS); %>

         
         
         function savePostPageSettingSuccessCallback(results){
             //for now do nothing when we save the pk
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
                                         if(typeof (iMailLogPK) === 'undefined' || iMailLogPK == null || iMailLogPK === 0){ iMailLogPK = userPageSettings.value; }                                 
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

         

         function saveMailLogPK() {
             try {
                 var row = oMailLogGrid.select();
                 if (typeof (row) === 'undefined' || row == null) {              
                     ngl.showValidationMsg("TMS Mail Required", "Please select a TMS Mail Record to continue", tPage);
                     return false;
                 }               
                 //Get the dataItem for the selected row
                 var dataItem = oMailLogGrid.dataItem(row);
                 if (typeof (dataItem) === 'undefined' || dataItem == null) {              
                     ngl.showValidationMsg("TMS Mail Required", "Please select a TMS Mail Record to continue", tPage);
                     return false;
                 }  
                 if ("MailControl" in dataItem){
                
                     iMailLogPK = dataItem.MailControl;
                     var setting = {name:'pk', value: iMailLogPK.toString()};
                     var oCRUDCtrl = new nglRESTCRUDCtrl();
                     var blnRet = oCRUDCtrl.update("MailLog/PostPageSetting", setting, tPage, "savePostPageSettingSuccessCallback", "savePostPageSettingSuccessCallbackAjaxErrorCallback",tPage);
                     return true;
                 } else {
                     ngl.showValidationMsg("TMS Mail Required", "Please select a TMS Mail Record to continue", tPage);
                     return false;
                 }

             } catch (err) {
                 ngl.showErrMsg("Cannot Continue", "There was an unexpected error, please reload the screen.  If this continues please contact technical support. Error: " + err.message, tPage);
             }
            
         }
         function isMailLogSelected() {
             if (typeof (iMailLogPK) === 'undefined' || iMailLogPK === null || iMailLogPK === 0) {
                 return saveMailLogPK();
             }
             return true;
         }
         function execActionClick(btn, proc){            
             if(btn.id == "btnRefresh" ){ 
                 oMailLogGrid.dataSource.read();
             } else if(btn.id == "btnResetCurrentUserConfig"){ resetCurrentUserConfig(PageControl); }
         }
         var blnMailLogGridChangeBound = false;
         //*************  Call Back Functions ****************
         function MailLogGridDataBoundCallBack(e,tGrid){           
             oMailLogGrid = tGrid;
             if (blnMailLogGridChangeBound == false){
                 oMailLogGrid.bind("change", saveMailLogPK);
                 blnMailLogGridChangeBound = true;
             }
             //if iMailLogPK is not 0 select that row in the grid
             if (typeof (iMailLogPK) !== 'undefined' && iMailLogPK !== null && iMailLogPK !== 0) {
                 var rows = oMailLogGrid.items();
                 $(rows).each(function(e) {
                     var row = this;
                     var dataItem = oMailLogGrid.dataItem(row);
                     if (dataItem.MailControl == iMailLogPK) { 
                         oMailLogGrid.select(row); 
                         return false;
                     }
                 });
             }

             //example of how to read data from the widget?
             //lblBalanceID =  wdgtMailLogGridEdit.GetFieldID("lblBalanceTotal");
             //wdgtMailLogGridEdit.SetFieldDefault("lblBalanceTotal",'$100.00');
             //calculate_balance('$100.00');
         }

        
         $(document).ready(function () {
             var PageMenuTab = <%=PageMenuTab%>;
                      
           
            if (control != 0){
                //setTimeout(function () {
                //    //add code here to load screen specific information this is only visible when a user is authenticated
                //}, 1,this);

                //read the current selected primary key for this user so we can select it in the grid
                getPageSettings(tPage, "MailLog", "pk", false);
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