<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LELaneMaint.aspx.cs" Inherits="DynamicsTMS365.LELaneMaint" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head >
        <title>DTMS Lane Maintenance</title>
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

                            <div id="divTitleLE"></div>

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

    <% Response.WriteFile("~/Views/ChangeLEDialog.html"); %>
    <% Response.WriteFile("~/Views/LaneEAWindow.html"); %>
    <% Response.WriteFile("~/Views/CompContEAWindow.html"); %>
    <% Response.Write(PageTemplates); %>
      
    <%-- HelpWindow needs to go after AuthNotification because it uses the notification object created there --%>
    <% Response.Write(AuthLoginNotificationHTML); %>   
    <% Response.WriteFile("~/Views/HelpWindow.html"); %>       
    <script>
        <% Response.Write(ADALPropertiesjs); %>

        var PageControl = '<%=PageControl%>';  
        var oLELaneGrid = null;
        var iLanePK = 0;
        var tObj = this;
        var tPage = this;
        var tObjPG = this;
        var pgLEControl = 0;
        var pgLEName = ""; 

        <% Response.Write(NGLOAuth2); %>

        
        
        <% Response.Write(PageCustomJS); %>
        //***************** Widgets ******************************

        //~~~~~~~~~~ChangeLEDialogCtrl~~~~~~~~~~~~~~~~~~~
        var wndChangeLEDialog = kendo.ui.Window;
        var oChangeLEDialogCtrl = new ChangeLEDialogCtrl()
        //Widgit call backs
        function oChangeLEDialogSaveCB(results){
            if (typeof (results) !== 'undefined' && results != null) { 
                if ('LEAdminControl' in results) { pgLEControl = results.LEAdminControl; }
                if ('LegalEntity' in results) { pgLEName = results.LegalEntity; }             
                $("#divTitleLE").html("<h2>" + pgLEName + "</h2>");             
                //We need to reset the page before refresh to avoid problematic behavior since we are completely changing the data returned (not just filtering or sorting from one retured record set)
                //oLELaneGrid.dataSource.page(1);
                refreshLELaneGrid();
                wdgtLaneEA.screenLEControl = pgLEControl;
                wdgtLaneEA.refreshLookupLists();
            }
        }    
        //functions
        function changeLegalEntity() { oChangeLEDialogCtrl.show(); }

        //~~~~~~~~~~LaneEAWndCtrl~~~~~~~~~~~~~~~~~~~~~~~~
        var wndLaneEA = kendo.ui.Window;
        var wdgtLaneEA = new LaneEAWndCtrl()
        //Widgit call backs
        function wdgtLaneEASaveDeleteCB(results){                     
            try {
                if (typeof (results) !== 'undefined' && ngl.isObject(results)) {
                    if(results.source === "saveSuccessCallback"){
                        var data = results.data;
                        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                            if (ngl.stringHasValue(data.Errors)) { ngl.showErrMsg("Lane Save Failure", data.Errors, null); }
                            else {
                                if (typeof (data) !== 'undefined' && data != null && ngl.isArray(data)) {
                                    if (data.length > 0 && typeof (data[0]) !== 'undefined') {
                                        if (data[0].Success == true && ngl.isNullOrWhitespace(data[0].SuccessMsg) == false){ var sSuccessMsg = ngl.replaceEmptyString(data[0].SuccessTitle,'', '<br>') + ngl.replaceEmptyString(data[0].SuccessMsg,''); ngl.showSuccessMsg(sSuccessMsg,tObj); }                                
                                        if (ngl.isNullOrWhitespace(data[0].WarningMsg) == false){ ngl.showWarningMsg(ngl.replaceEmptyString(data[0].WarningTitle,'Save Lane Warning'), data[0].WarningMsg, tObj); }                                         
                                        if (ngl.isNullOrWhitespace(data[0].ErrMsg) == false){ ngl.showErrMsg(ngl.replaceEmptyString(data[0].ErrTitle,'Save Lane Error'), data[0].ErrMsg, tObj); }                                                   
                                    }                   
                                }    
                            }
                        }
                    }
                } else { ngl.showErrMsg("Save Lane Failure", "There was a problem while executing Lane Save", null); }
            } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
            //We need to reset the page before refresh to avoid problematic behavior since we are completely changing the data returned (not just filtering or sorting from one retured record set)
            //oLELaneGrid.dataSource.page(1);
            refreshLELaneGrid();
        }
        //functions
        function openAddNewLaneWindow() {
            //clear the data and set the LE to the selected value from the screen for Add New
            wdgtLaneEA.data = null;
            wdgtLaneEA.screenLEName = pgLEName;
            ////pgLEControl
            wdgtLaneEA.show();       
        }
        //--Added By RHR on 01/12/2024 for v-8.5.4.004 for setting up transload lanes
        function openDuplicateLaneWindow() {
            //clear the data and set the LE to the selected value from the screen for Add New
            loadLELaneGridSelectedRow = oLELaneGrid.select();
            if (typeof (loadLELaneGridSelectedRow) === 'undefined' || loadLELaneGridSelectedRow == null) { ngl.showValidationMsg("Lane Record Required", "Please select a Lane to continue", tPage); return false; }
            var loadLELaneGridSelectedRowDataItem = oLELaneGrid.dataItem(loadLELaneGridSelectedRow); //Get the dataItem for the selected row
            if (typeof (loadLELaneGridSelectedRowDataItem) === 'undefined' || loadLELaneGridSelectedRowDataItem == null) { ngl.showValidationMsg("Lane Record Required", "Please select a Lane to continue", tPage); return false; }
            if ("LaneControl" in loadLELaneGridSelectedRowDataItem) {
                loadLELaneGridSelectedRowDataItem.LaneControl = 0;
            } else { ngl.showValidationMsg("Invalid Lane Record Required", "Please select a Lane to continue", tPage); return false; }
            if ("LaneNumber" in loadLELaneGridSelectedRowDataItem) {
                loadLELaneGridSelectedRowDataItem.LaneNumber = '';
            } else { ngl.showValidationMsg("Invalid Lane Record Required", "Please select a Lane to continue", tPage); return false; }
            if ("LaneName" in loadLELaneGridSelectedRowDataItem) {
                loadLELaneGridSelectedRowDataItem.LaneName = 'Duplicate';
            } else { ngl.showValidationMsg("Invalid Lane Record Required", "Please select a Lane to continue", tPage); return false; }
            wdgtLaneEA.data = loadLELaneGridSelectedRowDataItem;
            wdgtLaneEA.screenLEName = pgLEName;
            ////pgLEControl
            wdgtLaneEA.show();
        }
        function openEditLaneWindow(e) {
            var item = this.dataItem($(e.currentTarget).closest("tr")); 
            //console.log('item');
            //console.log(item)
            wdgtLaneEA.data = item;
            //console.log("Lane Data");
            //console.log(item);
            wdgtLaneEA.show();            
        }
        function deleteLane(e){
           // debugger;
            var item = this.dataItem($(e.currentTarget).closest("tr"));
            wdgtLaneEA.delete(item); 
        }


        //************* Action Menu Functions ********************
        function execActionClick(btn, proc){
            if(btn.id == "btnChangeLE"){ changeLegalEntity(); }
            else if (btn.id == "btnAddLane31") { openAddNewLaneWindow(); } 
            else if (btn.id == "btnDublicateLane") { openDuplicateLaneWindow(); }   //--Added By RHR on 01/12/2024 for v-8.5.4.004 for setting up transload lanes 
            else if(btn.id == "btnRefresh" || btn === "Refresh" ){ refresh(); }
            else if(btn.id == "btnResetCurrentUserConfig"){ resetCurrentUserConfig(PageControl); }
            else if(btn.id == "btnLaneFee"){ OpenItem(pgLEControl);}
            else if(btn.id == "btnLaneProfileFees"){ OpenLaneProfileFees(pgLEControl);}
            else if(btn.id == "btnLanePreferredCarriers"){ OpenLanePreferredCarriers(pgLEControl);}
            else if(btn.id == "btnCalcLaneLatLongMiles"){ if (isLELaneSelected() === true) { RecalculateLatLongMiles(); } } //Added By LVV on 6/17/20 for v-8.2.1.008 Task#202005151417 - Company/Warehouse Maintenance Changes


        }

        
        /* Summary - Calls the REST method to recalculate the lattitude and longitude */
        function ConfirmRecalculateLatLongMiles(iRet) {
            if (typeof (iRet) === 'undefined' || iRet === null || iRet === 0) { return; } //Chose the Cancel action
            //Chose the Ok action        
            kendo.ui.progress(oLELaneGrid.element, true);
            $.ajax({
                type: 'GET',
                url: 'api/Lane/RecalculateLatLongMiles/' + iLELanePK,
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                success: function (data) {
                    kendo.ui.progress(oLELaneGrid.element, false); 
                    try {
                        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                            if (ngl.stringHasValue(data.Errors)) { ngl.showErrMsg("Recalculate Lat Long Failure", data.Errors, null); }
                            else {                               
                                if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {                                   
                                    if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                        blnSuccess = data.Data[0].Success;
                                        if (data.Data[0].Success === true){ ngl.showSuccessMsg(ngl.replaceEmptyString(data.Data[0].SuccessMsg,'Success!'), null); }
                                        if (ngl.stringHasValue(data.Data[0].ErrMsg)){ ngl.showErrMsg(ngl.replaceEmptyString(data.Data[0].ErrTitle,'Recalculate Lat Long Error'), data.Data[0].ErrMsg, null); }                                        
                                        if (ngl.stringHasValue(data.Data[0].WarningMsg)){ ngl.showWarningMsg(ngl.replaceEmptyString(data.Data[0].WarningTitle,'Recalculate Lat Long Warning'), data.Data[0].WarningMsg, null); }                                                                                                                             
                                        refresh();
                                    }                              
                                }                            
                            }
                        }
                    } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                },
                error: function (xhr, textStatus, error) { kendo.ui.progress(oLELaneGrid.element, false); var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure"); ngl.showErrMsg("Recalculate Lat Long Failure", sMsg, null); }
            });
        }

        /* Summary - Verifies that the user wants to recalculate the lattitude and longitude and if so calls ConfirmRecalculateLatLongMiles() */
        function RecalculateLatLongMiles() {
            //perform confirmation
            var title = "Recalculate Lattitude, Longitude and Miles";
            ngl.OkCancelConfirmation(
                title,
                "This action will overwrite any existing Lattitude, Longitude and Miles information for the Lane. Are you sure you want to proceed?",
                400,
                400,
                null,
                "ConfirmRecalculateLatLongMiles");
        }


        function OpenItem(pgLEControl){
            if (isLELaneSelected() === true) {location.href = "LELaneFees";}           
        }

        function OpenLaneProfileFees(pgLEControl){
            if (isLELaneSelected() === true) {location.href = "LELaneProfileFees";}          
        }
        function OpenLanePreferredCarriers(pgLEControl){          
            if (isLELaneSelected() === true) {location.href = "LELanePreferredCarriers";}             
        }
       
        function refresh() { ngl.readDataSource(oLELaneGrid); }

        function refreshLELaneGrid() { ngl.readDataSource(oLELaneGrid); }
       
        var blnLELaneGridChangeBound = false;
        //************* Call Back Functions **********************
        function LELaneGridDataBoundCallBack(e,tGrid){           
            oLELaneGrid = tGrid;
            if (blnLELaneGridChangeBound == false){
                oLELaneGrid.bind("change", saveLELanePK);
                blnLELaneGridChangeBound = true;
            }        
            //if iLELanePK is not 0 select that row in the grid
            if (typeof (iLELanePK) !== 'undefined' && iLELanePK !== null && iLELanePK !== 0) {
                var rows = oLELaneGrid.items();
                $(rows).each(function(e) {
                    var row = this;
                    console.log("Lane Grid Row");
                    console.log(row);
                    var dataItem = oLELaneGrid.dataItem(row);
                    if (dataItem.LaneControl == iLELanePK) { oLELaneGrid.select(row); }
                });
            }
        }
       
        function LELaneGridGetStringData(s)
        { 
            s.LEAdminControl = pgLEControl;
            //s.ParentControl = 0;
            //s.BookControl = 0;
            //s.LaneControl = 0;
            return '';
        }

        var GetLEAdminNotAsyncCB = function (data) {
            pgLEControl = data[0].LEAdminControl;
            pgLEName = data[0].LegalEntity;      
            $("#divTitleLE").html("<h2>" + pgLEName + "</h2>"); 
        }

        function savePostPageSettingSuccessCallback(results){
            //for now do nothing when we save the pk
        }
        function savePostPageSettingAjaxErrorCallback(xhr, textStatus, error){
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
                                        if(typeof (iLELanePK) === 'undefined' || iLELanePK == null || iLELanePK === 0){ iLELanePK = userPageSettings.value; }                                 
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
       

        $(function () {
            //wire focus of all numerictextbox widgets on the page
            $("input[type=number]").bind("focus", function () {
                var input = $(this);
                clearTimeout(input.data("selectTimeId")); //stop started time out if any
                var selectTimeId = setTimeout(function() { input.select(); });
                input.data("selectTimeId", selectTimeId);
            }).blur(function(e) {
                clearTimeout($(this).data("selectTimeId")); //stop started timeout
            });
        })


        function saveLELanePK() {
            try {
                loadLELaneGridSelectedRow = oLELaneGrid.select();
                if (typeof (loadLELaneGridSelectedRow) === 'undefined' || loadLELaneGridSelectedRow == null) { ngl.showValidationMsg("Lane Record Required", "Please select a Lane to continue", tPage); return false; }                             
                loadLELaneGridSelectedRowDataItem = oLELaneGrid.dataItem(loadLELaneGridSelectedRow); //Get the dataItem for the selected row
                if (typeof (loadLELaneGridSelectedRowDataItem) === 'undefined' || loadLELaneGridSelectedRowDataItem == null) { ngl.showValidationMsg("Lane Record Required", "Please select a Lane to continue", tPage); return false; } 
                if ("LaneControl" in loadLELaneGridSelectedRowDataItem){                
                    iLELanePK = loadLELaneGridSelectedRowDataItem.LaneControl;
                    var setting = {name:'pk', value: iLELanePK.toString()};
                    var oCRUDCtrl = new nglRESTCRUDCtrl();
                    var blnRet = oCRUDCtrl.update("Lane/PostPageSetting", setting, tPage, "savePostPageSettingSuccessCallback", "savePostPageSettingAjaxErrorCallback",tPage);
                    //check if the TranCode is PB or PC and enable or disable the Print BOL action button (We can print the BOL for both PB or PC)  
                    return true;
                } else { ngl.showValidationMsg("Lane Record Required", "Invalid Record Identifier, please select a Lane to continue", tPage); return false; }
            } catch (err) { ngl.showErrMsg("Cannot Continue", "There was an unexpected error, please reload the screen. If this continues please contact technical support. Error: " + err.message, tPage); }           
        }

        function isLELaneSelected() {
            if (typeof (iLELanePK) === 'undefined' || iLELanePK === null || iLanePK === 0) { return saveLELanePK(); }
            return true;
        }

        $(document).ready(function () {
            var PageMenuTab = <%=PageMenuTab%>;
            
            //NOTE: This has to be done before PageReadyJS because the grid is dependant on the LEControl value                
            //Get the LE for the user
            getLEAdminNotAsync(0, GetLEAdminNotAsyncCB);
 
            if (control != 0){
                setTimeout(function () {  
                    //debugger;
                    ////////////ChangeLEDialogCtrl///////////////////
                    oChangeLEDialogCtrl = new ChangeLEDialogCtrl();
                    oChangeLEDialogCtrl.loadDefaults(wndChangeLEDialog, oChangeLEDialogSaveCB);

                    ////////////LaneEAWndCtrl///////////////////
                    wdgtLaneEA = new LaneEAWndCtrl(); 
                    wdgtLaneEA.screenLEControl = pgLEControl;
                    wdgtLaneEA.loadDefaults(wndLaneEA, wdgtLaneEASaveDeleteCB, wdgtLaneEASaveDeleteCB);
            
                }, 10,this);           

               // getPageSettings(tPage, "LELaneMaint", "pk", false);
            }       
            setTimeout(function () {var PageReadyJS = <%=PageReadyJS%>; }, 10,this);
            setTimeout(function () {        
                menuTreeHighlightPage(); //must be called after PageReadyJS
                var divWait = $("#h1Wait");
                if (typeof (divWait) !== 'undefined') { divWait.hide(); }
            }, 10, this);
        });
    </script>
    <style>
        .k-grid tbody tr td { overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }
    </style>   
      </div>
    </body>
</html>