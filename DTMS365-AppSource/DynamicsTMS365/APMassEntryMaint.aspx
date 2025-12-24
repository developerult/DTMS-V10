<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="APMassEntryMaint.aspx.cs" Inherits="DynamicsTMS365.APMassEntryMaint" %>

<!DOCTYPE html>

<html>
    <head >
        <title>DTMS AP Mass Entry Maint</title>         
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

                            <div>
                                <div style="float: left;">
                                    <div style="float: left;">
                                        <table class="tblResponsive">
                                            <tr><td class="tblResponsive-top">Carrier</td></tr>
                                            <tr><td class="tblResponsive-top"><input id="ddlFltrCarriers" style="width: 250px;" /></td></tr>
                                        </table>
                                    </div>
                                    <div style="float: left;">                                       
                                        <table class="tblResponsive">
                                            <tr><td class="tblResponsive-top">&nbsp;</td></tr>
                                            <tr><td class="tblResponsive-top"><input id="ddlAPAuditFltrs" style="width: 250px;" /></td></tr>
                                        </table>
                                    </div>                                                              
                                    <div style="float:left;">
                                        <table class="tblResponsive">
                                            <tr><td class="tblResponsive-top">Received Date From</td></tr>                                                                        
                                            <tr><td class="tblResponsive-top"><input id="dpAPReceivedDateFrom" /></td></tr>                                    
                                        </table>
                                    </div>
                                    <div style="float:left;">
                                        <table class="tblResponsive">
                                            <tr><td class="tblResponsive-top">Received Date To</td></tr>                                                                        
                                            <tr><td class="tblResponsive-top"><input id="dpAPReceivedDateTo" /></td></tr>                                    
                                        </table>
                                    </div>
                                </div>
                                <div class="tblResponsive-wrap" style="float: none; margin-left: 5px;">
                                To reduce the expected cost to match the billed cost edit the AP record and enter a reduction amount with a reason code.
                                To increase the expected cost to match the billed cost open the load board, select the shipmet, navigate to fees and enter an Invoice Adjusment flat amount.
                                </div>
                            </div>                            
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
        var oAPAuditApprovalGrid = null;        
        var tObj = this;
        var tPage = this;  

        <% Response.Write(NGLOAuth2); %>

        var iAPControl = 0;
        var apAuditApprovalGridSelectedRow; 
        var apAuditApprovalGridSelectedRowDataItem;   
        var pgLEControl = 0;
        var pgLEName = "";
        var pgCarrierControlFltr = 0;
        var pgAPAuditFltrsFltr = 0;
        var pgAPReceivedDateFrom = null;
        var pgAPReceivedDateTo = null;
        var ddlAPAuditFltrs;
        var ddlFltrCarriers;
        var today = kendo.parseDate(new Date(), "MM/dd/yyyy");
        <% Response.Write(PageCustomJS); %>

        //*************  execActionClick  ****************
        function execActionClick(btn, proc){         
            if (btn.id == "btnRunAudit" ){ runAudit(); }
            else if (btn.id == "btnRefresh" ){ refresh(); }
            else if (btn.id == "btnResetCurrentUserConfig"){ resetCurrentUserConfig(PageControl); }
        }
      
        //*************  Action Menu Functions ****************
        function refresh(){ oAPAuditApprovalGrid.dataSource.read(); }
        function refreshAPAuditApprovalGrid(){ oAPAuditApprovalGrid.dataSource.read(); }

        function saveAPAuditApprovalPK() {
            try {
                apAuditApprovalGridSelectedRow = oAPAuditApprovalGrid.select(); //Get the selected row
                if (typeof (apAuditApprovalGridSelectedRow) === 'undefined' || apAuditApprovalGridSelectedRow == null) { ngl.showValidationMsg("APMassEntry Record Required", "Please select a APMassEntry to continue", tPage); return false; }                               
                apAuditApprovalGridSelectedRowDataItem = oAPAuditApprovalGrid.dataItem(apAuditApprovalGridSelectedRow); //Get the dataItem for the selected row
                if (typeof (apAuditApprovalGridSelectedRowDataItem) === 'undefined' || apAuditApprovalGridSelectedRowDataItem == null) { ngl.showValidationMsg("APMassEntry Record Required", "Please select a APMassEntry to continue", tPage); return false; } 
                if ("APControl" in apAuditApprovalGridSelectedRowDataItem){                
                    iAPControl = apAuditApprovalGridSelectedRowDataItem.APControl;
                    var setting = {name:'pk', value: iAPControl.toString()};
                    var oCRUDCtrl = new nglRESTCRUDCtrl();
                    var blnRet = oCRUDCtrl.update("APMassEntry/PostPageSetting", setting, tPage, "savePostPageSettingSuccessCallback", "savePostPageSettingAjaxErrorCallback", tPage);
                    return true;
                } else { ngl.showValidationMsg("APMassEntry Record Required", "Invalid Record Identifier, please select a APMassEntry to continue", tPage); return false; }
            } catch (err) { ngl.showErrMsg("Cannot Continue", "There was an unexpected error, please reload the screen. If this continues please contact technical support. Error: " + err.message, tPage); }           
        }    
        function isAPAuditApprovalSelected() {
            if (typeof (iAPControl) === 'undefined' || iAPControl === null || iAPControl === 0) { return saveAPAuditApprovalPK(); }
            return true;
        }
        
        //*************  Call Back Functions ****************
        var blnAPAuditApprovalGridChangeBound = false;
        function APAuditApprovalGridDataBoundCallBack(e,tGrid){           
            oAPAuditApprovalGrid = tGrid;
            if (blnAPAuditApprovalGridChangeBound == false){
                oAPAuditApprovalGrid.bind("change", saveAPAuditApprovalPK);
                blnAPAuditApprovalGridChangeBound = true;
            }             
            var ds = tGrid.dataSource.data(); 
            for (var j=0; j < ds.length; j++) {
                if (typeof (ds[j]) !== 'undefined' && ds[j] != null && ngl.isObject(ds[j])) {
                    if (typeof (ds[j].APControl) !== 'undefined' && ds[j].APControl != null){                    
                        var item = tGrid.dataSource.get(ds[j].APControl); //Get by ID or any other preferred method
                        if (typeof (ds[j].APPayCode) !== 'undefined' && ds[j].APPayCode != null){
                            //For now any freight bill where the bookpexp is N can be deleted
                            if (ds[j].APPayCode.toUpperCase() !== "N") {                                             
                                tGrid.tbody.find("tr[data-uid='"+item.uid+"'] .k-grid-DeleteAPAuditAprvGridCRUDCtrl").prop('disabled', true); //Disable the Delete button
                                tGrid.tbody.find("tr[data-uid='"+item.uid+"'] .k-grid-DeleteAPAuditAprvGridCRUDCtrl").addClass("k-state-disabled"); //Add the disabled class to the css (change button color)
                            }
                            //When booking record status is AA cannot edit
                            if (ds[j].APPayCode.toUpperCase() === "AA") {                                             
                                tGrid.tbody.find("tr[data-uid='"+item.uid+"'] .k-grid-EditAPAuditAprvGridCRUDCtrl").prop('disabled', true); //Disable the Edit button
                                tGrid.tbody.find("tr[data-uid='"+item.uid+"'] .k-grid-EditAPAuditAprvGridCRUDCtrl").addClass("k-state-disabled"); //Add the disabled class to the css (change button color)
                            }
                        }
                        //When APExportFlag is true cannot edit
                        if (typeof (ds[j].APExportFlag) !== 'undefined' && ds[j].APExportFlag != null) {
                            if (ds[j].APExportFlag === true) {                       
                                tGrid.tbody.find("tr[data-uid='"+item.uid+"'] .k-grid-EditAPAuditAprvGridCRUDCtrl").prop('disabled', true); //Disable the Edit button
                                tGrid.tbody.find("tr[data-uid='"+item.uid+"'] .k-grid-EditAPAuditAprvGridCRUDCtrl").addClass("k-state-disabled"); //Add the disabled class to the css (change button color)
                            }
                        }
                    }
                }              
            }
        }

        function APAuditApprovalGridGetStringData(s)
        {           
            blnFirstLoad = false;
            var f = new AuditFilters(); 
            f.CarrierDDLValue = pgCarrierControlFltr;
            f.APAuditFltrsDDLValue = pgAPAuditFltrsFltr;
            f.APReceivedDateFrom = pgAPReceivedDateFrom;
            f.APReceivedDateTo = pgAPReceivedDateTo;
            s.Data = JSON.stringify(f);
            return JSON.stringify(f);
        }

        var GetLEAdminNotAsyncCB = function (data) {
            pgLEControl = data[0].LEAdminControl;
            pgLEName = data[0].LegalEntity;       
        }

        function savePostPageSettingSuccessCallback(results){ return; } //for now do nothing when we save the pk
        function savePostPageSettingAjaxErrorCallback(xhr, textStatus, error){ return; } //for now do nothing when we save the pk

        var blnFirstLoad = true;
        function readGetPageSettingSuccessCallback(data) {
            var oResults = new nglEventParameters();
            var tObj = this;
            oResults.source = "readGetPageSettingSuccessCallback";
            oResults.msg = 'Failed'; //set default to Failed         
            oResults.CRUD = "read";
            oResults.widget = tObj;
            var dsUserPageSettings = null;                          
            try {
                var blnSuccess = false;
                var blnErrorShown = false;
                var strValidationMsg = "";                        
                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {                          
                    if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Get User Page Settings Failure", data.Errors, null); }                          
                    else {                               
                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {                                   
                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                blnSuccess = true;
                                dsUserPageSettings = data.Data[0]; 
                                oResults.msg = "Success";
                                if(typeof (dsUserPageSettings) !== 'undefined' && dsUserPageSettings != null && dsUserPageSettings.value != undefined){
                                    var d = JSON.parse(dsUserPageSettings.value);         
                                    var psData = JSON.parse(d.Data);
                                    //set the ddl
                                    pgCarrierControlFltr = psData.CarrierDDLValue;
                                    pgAPAuditFltrsFltr = psData.APAuditFltrsDDLValue;                                   
                                    ddlFltrCarriers.select(function(dataItem) { return dataItem.Description === psData.CarrierDDLValue; });
                                    ddlAPAuditFltrs.select(function(dataItem) { return dataItem.Control === psData.APAuditFltrsDDLValue; });
                                    //set the dps
                                    pgAPReceivedDateFrom = ngl.getShortDateString(psData.APReceivedDateFrom, today);
                                    pgAPReceivedDateTo = ngl.getShortDateString(psData.APReceivedDateTo, today);
                                    $("#dpAPReceivedDateFrom").data("kendoDatePicker").value(pgAPReceivedDateFrom);                                    
                                    $("#dpAPReceivedDateTo").data("kendoDatePicker").value(pgAPReceivedDateTo);    
                                    canRunAudit(pgAPAuditFltrsFltr);
                                }                                   
                            }                              
                        }                            
                    }                        
                }                      
                if (blnSuccess === false && blnErrorShown === false) {                          
                    if (strValidationMsg.length < 1) { strValidationMsg = "If this is your first time on this page your settings will be saved for your next visit, if not please contact technical support if you continue to receive this message."; }
                    ngl.showInfoNotification("Unable to Read Page Settings", strValidationMsg, null);                                     
                    $("#ddlFltrCarriers").data("kendoDropDownList").select(0);
                    $("#ddlAPAuditFltrs").data("kendoDropDownList").select(0);
                    var dataItemFltrCarriers = $("#ddlFltrCarriers").data("kendoDropDownList").dataItem();
                    var dataItemAPAuditFltrs = $("#ddlAPAuditFltrs").data("kendoDropDownList").dataItem();
                    pgCarrierControlFltr = dataItemFltrCarriers.Description;
                    pgAPAuditFltrsFltr = dataItemAPAuditFltrs.Control;   
                    pgAPReceivedDateFrom = today;
                    pgAPReceivedDateTo = today;
                    $("#dpAPReceivedDateFrom").data("kendoDatePicker").value(pgAPReceivedDateFrom);
                    $("#dpAPReceivedDateTo").data("kendoDatePicker").value(pgAPReceivedDateTo);               
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

        function RunAuditSuccessCallback(data) {
            var oResults = new nglEventParameters();          
            oResults.source = "RunAuditSuccessCallback";
            oResults.msg = 'Failed'; //set default to Failed         
            oResults.CRUD = "update";                         
            try {
                var blnSuccess = false;
                var blnErrorShown = false;
                var strValidationMsg = "";                        
                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {                          
                    if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Run Audit Failure", data.Errors, null); }                          
                    else {                               
                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {                                   
                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                blnSuccess = data.Data[0].Success; //blnSuccess = true;
                                oResults.msg = "Success";                       
                                if (ngl.stringHasValue(data.Data[0].ErrMsg)){ blnErrorShown = true; ngl.showErrMsg(ngl.replaceEmptyString(data.Data[0].ErrTitle,'Run Audit Error'), data.Data[0].ErrMsg, null); }                              
                                if (ngl.stringHasValue(data.Data[0].WarningMsg)){ blnErrorShown = true; ngl.showWarningMsg(ngl.replaceEmptyString(data.Data[0].WarningTitle,'Run Audit Warning'), data.Data[0].WarningMsg, null); }                              
                                if (blnErrorShown === false){ ngl.showSuccessMsg(ngl.replaceEmptyString(data.Data[0].SuccessMsg,'Success!'), null); }                                                          
                                refreshAPAuditApprovalGrid();
                            }                              
                        }                            
                    }                        
                }                      
                if (blnSuccess === false && blnErrorShown === false) {                          
                    if (strValidationMsg.length < 1) { strValidationMsg = "Run Audit Failure"; }
                    ngl.showErrMsg("Run Audit Failure", strValidationMsg, null);
                }                   
            } catch (err) { ngl.showErrMsg(err.name, err.description, null); }       
        }
        function RunAuditAjaxErrorCallback(xhr, textStatus, error) {
            var oResults = new nglEventParameters(); 
            oResults.source = "RunAuditAjaxErrorCallback";         
            oResults.msg = 'Failed'; //set default to Failed  
            oResults.CRUD = "update"
            oResults.error = new Error();
            oResults.error.name = "Run Audit Failure"
            oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
            ngl.showErrMsg(oResults.error.name, oResults.error.message, null);
            refreshAPAuditApprovalGrid();
        }

        //*************  Page Level Functions ****************      
        function getDDLVars() {
            ddlFltrCarriers = $("#ddlFltrCarriers").data("kendoDropDownList");
            ddlAPAuditFltrs = $("#ddlAPAuditFltrs").data("kendoDropDownList");
        }

        function runAudit() {
            var f = oAPAuditApprovalGrid.dataSource.data();
            var oCRUDCtrl = new nglRESTCRUDCtrl();
            blnRet = oCRUDCtrl.update("APMassEntry/RunAudit", f, tObj, "RunAuditSuccessCallback", "RunAuditAjaxErrorCallback");
        }

        function canRunAudit(value) {
            switch(value) {
                case 0: //Normal
                    $('#btnRunAudit').prop('disabled', false);
                    break;
                case 1: //Matched
                    $('#btnRunAudit').prop('disabled', false);
                    break;
                case 2: //Approved
                    $('#btnRunAudit').prop('disabled', true);
                    break;
                case 3: //Electronic
                    $('#btnRunAudit').prop('disabled', false);
                    break;
                case 4: //AllErrors
                    $('#btnRunAudit').prop('disabled', true);
                    break;
                default:
                    $('#btnRunAudit').prop('disabled', false);
                    break;
            }
        }
      

        $(document).ready(function () {
            var PageMenuTab = <%=PageMenuTab%>;
          

            //NOTE: This has to be done before PageReadyJS because the grid is dependant on the LEControl value                            
            getLEAdminNotAsync(0, GetLEAdminNotAsyncCB); //Get the LE for the user

            if (control != 0){
                
                $("#ddlFltrCarriers").kendoDropDownList({
                    dataTextField: "Name",
                    dataValueField: "Description",
                    autoWidth: true,
                    filter: "contains",
                    dataSource: {
                        serverFiltering: false,
                        transport: {
                            read: function(options) {
                                var v = new  vLookupListCriteria();
                                v.id = nglGlobalDynamicLists.LECarrier;
                                v.sortKey  = 1;
                                v.criteria = pgLEControl;
                                $.ajax({
                                    async: false,
                                    url: "api/vLookupList/GetGlobalDynamicListFiltered",
                                    contentType: "application/json; charset=utf-8",
                                    dataType: 'json',
                                    data: {filter: JSON.stringify(v)},
                                    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },                                        
                                    success: function (data) { options.success(data); },
                                    error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Get Assigned LECarrier Failure"); ngl.showErrMsg("Get Assigned LECarrier", sMsg, null); }
                                });
                            }
                        },
                        schema: { 
                            data: "Data",
                            total: "Count",
                            model: { 
                                id: "Control",
                                fields: {
                                    Control: { type: "number" },
                                    Name: { type: "string" }, 
                                    Description: { type: "string" }
                                }
                            }, 
                            errors: "Errors"
                        },
                        error: function (xhr, textStatus, error) { ngl.showErrMsg("Get Carriers JSON Response Error",formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"), null); this.cancelChanges(); }
                    },
                    select: function(e) {
                        var value = e.dataItem.Description;                       
                        if (ngl.isNullOrWhitespace(value)){ value = 0; } //Value must be int so if value is empty string or null set it to 0
                        pgCarrierControlFltr = value;    
                        if(blnFirstLoad === false){ refreshAPAuditApprovalGrid(); }
                    }
                });
             
                $("#ddlAPAuditFltrs").kendoDropDownList({
                    dataTextField: "Name",
                    dataValueField: "Control",
                    autoWidth: true,
                    filter: "contains",
                    dataSource: {
                        serverFiltering: false,
                        transport: {
                            read: {
                                async: false,
                                url: "api/vLookupList/GetStaticList/" + nglStaticLists.APAuditFltrs,
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }
                            }, 
                        },
                        schema: { 
                            data: "Data",
                            total: "Count",
                            model: { 
                                id: "Control",
                                fields: {
                                    Control: { type: "number" },
                                    Name: { type: "string" }, 
                                    Description: { type: "string" }
                                }
                            }, 
                            errors: "Errors"
                        },
                        error: function (xhr, textStatus, error) { ngl.showErrMsg("Get APAuditFltrs JSON Response Error",formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"), null); this.cancelChanges(); }
                    },
                    select: function(e) {
                        var value = e.dataItem.Control;                       
                        if (ngl.isNullOrWhitespace(value)){ value = 0; } //Value must be int so if value is empty string or null set it to 0
                        pgAPAuditFltrsFltr = value;        
                        canRunAudit(value);
                        if(blnFirstLoad === false){ refreshAPAuditApprovalGrid(); }
                    }
                });

                $("#dpAPReceivedDateFrom").kendoDatePicker({ 
                    change: function() {
                        var value = this.value();
                        pgAPReceivedDateFrom = value; //value is the selected date in the datepicker
                        if(blnFirstLoad === false){ refreshAPAuditApprovalGrid(); }
                    }                   
                });
                $("#dpAPReceivedDateTo").kendoDatePicker({ 
                    change: function() {
                        var value = this.value();
                        pgAPReceivedDateTo = value; //value is the selected date in the datepicker
                        if(blnFirstLoad === false){ refreshAPAuditApprovalGrid(); }
                    }
                });

                getDDLVars();
                getPageSettings(tPage, "APMassEntry", "APAuditAprvlFltr", false);
            }
            var PageReadyJS = <%=PageReadyJS%>;
            menuTreeHighlightPage(); //must be called after PageReadyJS
            var divWait = $("#h1Wait");               
            if (typeof (divWait) !== 'undefined') { divWait.hide(); }         
        });
        </script>
            <style>
                .k-grid tbody tr td { overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }

                .k-tooltip { max-height: 500px; max-width: 450px; overflow-y: auto; }

                .k-grid tbody .k-grid-Edit { min-width: 0; }
                
                .k-grid tbody .k-grid-Edit .k-icon { margin: 0; }                             
            </style>  
        </div>  
    </body>
</html>
