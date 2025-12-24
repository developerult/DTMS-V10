<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageSecurity.aspx.cs" Inherits="DynamicsTMS365.ManageSecurity" %>

<!DOCTYPE html>

<html>
<head>
    <title>DTMS Manage Security</title>
    <%=cssReference%>
    <style>
        html,
        body { height: 100%; margin: 0; padding: 0; }
        html { font-size: 12px; font-family: Arial, Helvetica, sans-serif; overflow: hidden; }
    </style>
</head>
<body>
    <%=jssplitter2Scripts%>
    <%=sWaitMessage%>
    <div id="example" style="height: 100%; width: 100%; margin-top: 2px;">
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

                        <%--<button class="k-button k-button-solid-base k-button-solid k-button-md k-rounded-md" type="button" onclick="openMngFormConfigWzrd();">Manage Screens</button>
                        <button class="k-button k-button-solid-base k-button-solid k-button-md k-rounded-md" type="button" onclick="openMngProcConfigWzrd();">Manage Actions</button>
                        <button class="k-button k-button-solid-base k-button-solid k-button-md k-rounded-md" type="button" onclick="openMngReportConfigWzrd();">Manage Reports</button>--%>

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

        <%--selectionGrid--%>
        <div id="wndEditRoleSecTypes">           
            <div>
                <strong id="lblSelectionGrid" style="color:red;"></strong>
                <div id="selectRoleSecTypesGrid"></div>                
            </div>
            <input id="txtEditRoleSecTypeGroupControl" type="hidden" />
        </div>

        <% Response.WriteFile("~/Views/MngRoleConfigWzrdWnd.html"); %>
        <% Response.Write(PageTemplates); %>

        <%-- HelpWindow needs to go after AuthNotification because it uses the notification object created there --%>
        <% Response.Write(AuthLoginNotificationHTML); %>
        <% Response.WriteFile("~/Views/HelpWindow.html"); %>

        <script>
        <% Response.Write(ADALPropertiesjs); %>

            var PageControl = '<%=PageControl%>'; 
            var oGroupSecurityGrid = null;
            var oFormConfigGrid = null;
            var oProcConfigGrid = null;
            var oReportConfigGrid = null;
            var tObj = this;
            var tPage = this;           
        

        <% Response.Write(NGLOAuth2); %>

            var iGroupPK = 0;
            var groupSecurityGridSelectedRow;
            var groupSecurityGridSelectedRowDataItem; 

            var wndEditRoleSecTypes = kendo.ui.Window; //selectionGrid
         
            <% Response.Write(PageCustomJS); %>


            function saveGroupPK() {
                try {
                    groupSecurityGridSelectedRow = oGroupSecurityGrid.select();
                    if (typeof (groupSecurityGridSelectedRow) === 'undefined' || groupSecurityGridSelectedRow == null) { ngl.showValidationMsg("Record Selection Required", "Please select a Role to continue", tPage); return false; }                             
                    groupSecurityGridSelectedRowDataItem = oGroupSecurityGrid.dataItem(groupSecurityGridSelectedRow); //Get the dataItem for the selected row
                    if (typeof (groupSecurityGridSelectedRowDataItem) === 'undefined' || groupSecurityGridSelectedRowDataItem == null) { ngl.showValidationMsg("Record Selection Required", "Please select a Role to continue", tPage); return false; } 
                    if ("UserGroupsControl" in groupSecurityGridSelectedRowDataItem){                
                        iGroupPK = groupSecurityGridSelectedRowDataItem.UserGroupsControl;
                        var setting = {name:'pk', value: iGroupPK.toString()};
                        var oCRUDCtrl = new nglRESTCRUDCtrl();
                        var blnRet = oCRUDCtrl.update("ManageRoleConfig/PostPageSetting", setting, tPage, "savePostPageSettingSuccessCallback", "savePostPageSettingAjaxErrorCallback",tPage);
                        return true;
                    } else { ngl.showValidationMsg("Record Selection Required", "Invalid Record Identifier, please select a Role to continue", tPage); return false; }
                } catch (err) { ngl.showErrMsg("Cannot Continue", "There was an unexpected error, please reload the screen. If this continues please contact technical support. Error: " + err.message, tPage); }           
            }
    
            function isGroupSelected() {
                if (typeof (iGroupPK) === 'undefined' || iGroupPK === null || iGroupPK === 0) { return saveGroupPK(); }
                return true;
            }


            //*************  execActionClick  ****************
            function execActionClick(btn, proc){                
                if (btn.id == "btnManageScreens" ){ 
                    if (typeof (tPage["wdgtFormConfigWndDialog"]) !== 'undefined' && ngl.isObject(tPage["wdgtFormConfigWndDialog"])){
                        tPage["wdgtFormConfigWndDialog"].show();                
                    } else{alert("Missing HTML Element (wdgtFormConfigWndDialog is undefined)");} //Add better error handling here if cm stuff is missing
                }
                else if (btn.id == "btnManageActions" ){ 
                    if (typeof (tPage["wdgtProcConfigWndDialog"]) !== 'undefined' && ngl.isObject(tPage["wdgtProcConfigWndDialog"])){
                        tPage["wdgtProcConfigWndDialog"].show();                
                    } else{alert("Missing HTML Element (wdgtProcConfigWndDialog is undefined)");} //Add better error handling here if cm stuff is missing
                } 
                else if (btn.id == "btnManageReports" ){
                    if (typeof (tPage["wdgtReportConfigWndDialog"]) !== 'undefined' && ngl.isObject(tPage["wdgtReportConfigWndDialog"])){
                        tPage["wdgtReportConfigWndDialog"].show();                
                    } else{alert("Missing HTML Element (wdgtReportConfigWndDialog is undefined)");} //Add better error handling here if cm stuff is missing
                } 
                else if (btn.id == "btnMassUpdateScreens" ){ openMngFormConfigWzrd(); } 
                else if (btn.id == "btnMassUpdateActions" ){ openMngProcConfigWzrd(); } 
                else if (btn.id == "btnMassUpdateReports" ){ openMngReportConfigWzrd(); } 
                else if (btn.id == "btnRefreshGroupDefaultSecurity" ){ if (isGroupSelected() === true) { UpdateRolePermissions(); } }
                else if (btn.id == "btnRefreshAllGroupDefaultSecurity" ){ MassUpdateRolePermissions(); }
                else if (btn.id == "btnRefresh" ){ refresh(); } 
                else if (btn.id == "btnResetCurrentUserConfig"){ resetCurrentUserConfig(PageControl); }
            }

            function refresh(){ 
                ngl.readDataSource(oGroupSecurityGrid); 
                ngl.readDataSource(oFormConfigGrid);
                ngl.readDataSource(oProcConfigGrid);
                ngl.readDataSource(oReportConfigGrid);
            }

            var blnGroupSecurityGridChangeBound = false;
            function groupSecurityGridDataBoundCallBack(e,tGrid){           
                oGroupSecurityGrid = tGrid;       
                if (blnGroupSecurityGridChangeBound == false){
                    oGroupSecurityGrid.bind("change", saveGroupPK);
                    blnGroupSecurityGridChangeBound = true;
                } 
            }

            function FormConfigGridDataBoundCallBack(e,tGrid){           
                oFormConfigGrid = tGrid;       
            }

            function ProcConfigGridDataBoundCallBack(e,tGrid){           
                oProcConfigGrid = tGrid;       
            }

            function ReportConfigGridDataBoundCallBack(e,tGrid){           
                oReportConfigGrid = tGrid;       
            }


            function savePostPageSettingSuccessCallback(results){
                //for now do nothing when we save the pk
            }
            function savePostPageSettingAjaxErrorCallback(xhr, textStatus, error){
                //for now do nothing when we save the pk
            }


            //*************  Role Configuration Wizard Functions ****************
            function openMngFormConfigWzrd(){ openMngRoleConfigWzrdWindow(0); }
            function openMngProcConfigWzrd(){ openMngRoleConfigWzrdWindow(1); }
            function openMngReportConfigWzrd(){ openMngRoleConfigWzrdWindow(2); }


            //*************  Selection Grid Functions ****************       
            function openGroupSecTypeEdit(e){
                var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
                var groupControl = 0;
                if ("UserGroupsControl" in dataItem){ groupControl = dataItem.UserGroupsControl } else { ngl.showValidationMsg("Group Record Required", "Invalid Record Identifier, please select a record to continue", tPage); return false; }
                $("#txtEditRoleSecTypeGroupControl").val(groupControl);
                $("#selectRoleSecTypesGrid").data("kendoGrid").clearSelection();
                $('#selectRoleSecTypesGrid').data('kendoGrid').dataSource.read();
                wndEditRoleSecTypes.center().open();
            }

            /* Summary - Calls the REST method to update the Security Type Configuration for the Group */
            function ConfirmSaveGroupSecTypes(iRet) {
                if (typeof (iRet) === 'undefined' || iRet === null || iRet === 0) { return; } //Chose the Cancel action
                //Chose the Ok action
                if (typeof (groupSecTypeData) !== 'undefined' && ngl.isArray(groupSecTypeData.BitPositionsOn) && groupSecTypeData.BitPositionsOn.length > 0) {
                    $.ajax({
                        type: 'POST',
                        url: 'api/ManageRoleConfig/SaveSecTypesForGroup',
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        data: JSON.stringify(groupSecTypeData),
                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                        success: function (data) {
                            try {
                                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                    if (ngl.stringHasValue(data.Errors)) { ngl.showErrMsg("Save Group Security Type Configuration Failure", data.Errors, null); }
                                    else { wndEditRoleSecTypes.close(); refresh(); }
                                }
                            } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                        },
                        error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure"); ngl.showErrMsg("Save Group Security Type Configuration Failure", sMsg, null); }
                    });
                }
            }

            /* Summary - Verifies that the user wants to save the Group Security Type Configuration and if so calls ConfirmSaveGroupSecTypes() */
            var groupSecTypeData = null;
            function SaveGroupSecTypes() {
                groupSecTypeData = null; //clear any old values
                var selectedSecTypes = $("#selectRoleSecTypesGrid").data("kendoGrid").selectedKeyNames(); //get selected security types
                if (typeof (selectedSecTypes) !== 'undefined' && ngl.isArray(selectedSecTypes) && selectedSecTypes.length > 0) {
                    groupSecTypeData = new SelectableGridSave();
                    groupSecTypeData.BitPositionsOn = selectedSecTypes;
                    groupSecTypeData.Control = $("#txtEditRoleSecTypeGroupControl").val();
                    //perform confirmation
                    var title = "Save Group Security Type Config";
                    ngl.OkCancelConfirmation(
                        title,
                        "This action will overwrite the existing Security Type configuration for the selected Group. Are you sure you want to proceed?",
                        400,
                        400,
                        null,
                        "ConfirmSaveGroupSecTypes");
                } else { ngl.showWarningMsg("Selection Required", "Please select at least one Group", null); }
            }


            /* Summary - Calls the REST method to update the Role Center Permissions for the Group based on the Security Type Configuration */
            function ConfirmUpdateRolePermissions(iRet) {
                if (typeof (iRet) === 'undefined' || iRet === null || iRet === 0) { return; } //Chose the Cancel action
                //Chose the Ok action          
                $.ajax({
                    type: 'GET',
                    url: 'api/ManageRoleConfig/UpdateGroupBasedOnSecurityType/' + iGroupPK,
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                    success: function (data) {
                        try {
                            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                if (ngl.stringHasValue(data.Errors)) { ngl.showErrMsg("Update Role Permissions Failure", data.Errors, null); }
                                else { ngl.showSuccessMsg("Update Role Permissions - Success!", null); }
                            }
                        } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                    },
                    error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure"); ngl.showErrMsg("Update Role Permissions Failure", sMsg, null); }
                });
            }

            /* Summary - Verifies that the user wants to update the Role Center Permissions for the Group based on the Security Type Configuration if so calls ConfirmUpdateRolePermissions() */
            function UpdateRolePermissions() {
                //perform confirmation
                var title = "Update Role Permissions";
                ngl.OkCancelConfirmation(
                    title,
                    "This action will overwrite any unlocked Screen/Action/Report Permissions for the selected Role based on the Security Type Configuration. Then it will update all users associated with that Role. Are you sure you want to proceed?",
                    400,
                    400,
                    null,
                    "ConfirmUpdateRolePermissions");
            }

            /* Summary - Calls the REST method to mass update the Role Center Permissions for all Groups based on the Security Type Configuration */
            function ConfirmMassUpdateRolePermissions(iRet) {
                if (typeof (iRet) === 'undefined' || iRet === null || iRet === 0) { return; } //Chose the Cancel action
                //Chose the Ok action          
                $.ajax({
                    type: 'GET',
                    url: 'api/ManageRoleConfig/UpdateAllGroupsBasedOnSecurityType',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                    success: function (data) {
                        try {
                            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                if (ngl.stringHasValue(data.Errors)) { ngl.showErrMsg("Mass Update Role Permissions Failure", data.Errors, null); }
                                else { ngl.showSuccessMsg("Mass Update Role Permissions - Success!", null); }
                            }
                        } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                    },
                    error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure"); ngl.showErrMsg("Mass Update Role Permissions Failure", sMsg, null); }
                });
            }

            /* Summary - Verifies that the user wants to update the Role Center Permissions for the Group based on the Security Type Configuration if so calls ConfirmUpdateRolePermissions() */
            function MassUpdateRolePermissions() {
                //perform confirmation
                var title = "Mass Update Role Permissions";
                ngl.OkCancelConfirmation(
                    title,
                    "This action will overwrite any unlocked Screen/Action/Report Permissions for all Roles based on the Security Type Configurations. Are you sure you want to proceed?",
                    400,
                    400,
                    null,
                    "ConfirmMassUpdateRolePermissions");
            }

            $(document).ready(function () {
                var PageMenuTab = <%=PageMenuTab%>;                    
                                                          
                if (control != 0){

                    //Selection Grid
                    $("#selectRoleSecTypesGrid").kendoGrid({
                        autoBind: false,
                        dataSource: {
                            pageSize: 10,
                            transport: {
                                read: function (options) {
                                    $.ajax({
                                        url: 'api/ManageRoleConfig/GetConfigSecTypesForGroup/' + $("#txtEditRoleSecTypeGroupControl").val(),
                                        contentType: 'application/json; charset=utf-8',
                                        dataType: 'json',
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                        success: function (data) {
                                            options.success(data);
                                            if (typeof (data) !== 'undefined' && ngl.isObject(data) && typeof (data.Errors) !== 'undefined' && data.Errors != null) { ngl.showErrMsg('Access Denied', data.Errors, null); }
                                        },
                                        error: function (result) { options.error(result); }
                                    });
                                },
                            },
                            schema: {
                                data: "Data",
                                total: "Count",
                                model: {
                                    id: "SGItemBitPos",
                                    fields: {
                                        SGItemBitPos: { type: "number" },
                                        SGItemCaption: { type: "string" },
                                        SGItemOn: { type: "boolean" }
                                    }
                                }
                            }
                        },
                        pageable: true,
                        scrollable: false,
                        persistSelection: true,
                        sortable: true,
                        columns: [
                            { selectable: true, width: 50 },
                            { field: "SGItemCaption", title: "Name", hidden: false },
                            { field: "SGItemBitPos", title: "BitPos", hidden: true },
                            { field: "SGItemOn", title: "On", hidden: true }
                        ],
                        dataBound: function (e) {
                            var view = this.dataSource.view();
                            for(var i = 0; i < view.length;i++){
                                if(typeof (view[i].SGItemOn) !== 'undefined' && view[i].SGItemOn != null && view[i].SGItemOn === true){
                                    this.tbody.find("tr[data-uid='" + view[i].uid + "']")
                                    .addClass("k-state-selected")
                                    .find(".k-checkbox")
                                    .attr("checked","checked");
                                }
                            }
                        }
                    });

                    //RULE: Each item can be assigned to any one or more types. Everyone is the default and as soon as any other security type is selected Everyone is unselected. 
                    var grid = $("#selectRoleSecTypesGrid").data("kendoGrid");
                    //bind click event to the checkbox
                    grid.table.on("click", ".k-checkbox" , selectRow);
                    //on click of the checkbox:
                    function selectRow() {
                        var checked = this.checked,
                        row = $(this).closest("tr"),
                        grid = $("#selectRoleSecTypesGrid").data("kendoGrid"),
                        dataItem = grid.dataItem(row);

                        if(dataItem.SGItemBitPos !== 1){
                            if(checked){
                                var view = grid.dataSource.view();
                                for(var i = 0; i < view.length;i++){
                                    if(typeof (view[i].SGItemBitPos) !== 'undefined' && view[i].SGItemBitPos != null && view[i].SGItemBitPos === 1){
                                        grid.tbody.find("tr[data-uid='" + view[i].uid + "']")
                                        .removeClass("k-state-selected")
                                        .find(".k-checkbox")
                                        .removeAttr('checked');
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    ////////////wndEditRoleSecTypes///////////////////
                    wndEditRoleSecTypes = $("#wndEditRoleSecTypes").kendoWindow({
                        title: "Manage Role Security Types",
                        modal: true,
                        visible: false,
                        height: '80%',
                        //width: '60%',
                        minWidth: 500,
                        actions: ["save", "Minimize", "Maximize", "Close"], //'arrow-right',
                        //close: function (e) { resetDivSSOAMUManualEntry(); }
                    }).data("kendoWindow");
                    $("#wndEditRoleSecTypes").data("kendoWindow").wrapper.find(".k-svg-i-save").parent().click(function (e) { SaveGroupSecTypes(); });
           
                } 
                var PageReadyJS = <%=PageReadyJS%>;
                menuTreeHighlightPage(); //must be called after PageReadyJS
                var divWait = $("#h1Wait");                          
                if (typeof (divWait) !== 'undefined' ) { divWait.hide(); }        
         });
        </script>
        <style>
            .k-grid tbody tr td { overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }
            .k-tooltip { max-height: 500px; max-width: 450px; overflow-y: auto; }
            .k-grid tbody .k-grid-Edit { min-width: 0; }               
            .k-grid tbody .k-grid-Edit .k-icon { margin: 0; }
            /*This fixes thead icon alignment in thead grid buttons. Now the right side doesn't get cut off and it looks more centered. Should probably add this code to common.css */    
            .k-button-icontext .k-icon, .k-button-icontext .k-image, .k-button-icontext .k-sprite { margin-right:0; } 
        </style>
    </div>
</body>
</html>