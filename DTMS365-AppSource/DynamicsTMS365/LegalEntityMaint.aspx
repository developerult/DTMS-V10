<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LegalEntityMaint.aspx.cs" Inherits="DynamicsTMS365.LegalEntityMaint" %>

<!DOCTYPE html>

<html>
    <head >
        <title>DTMS Legal Entity Maintenance</title>
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

                    
          <div id="wndAMSCarAuto">   
              <div id="lblLE"></div>                  
              <div style="float: left;">                  
                  <div style="float: left;">                        
                      <table class="tblResponsive">                           
                          <tr>                               
                              <td class="tblResponsive-top"></td>                           
                          </tr>                         
                          <tr>                            
                              <td class="tblResponsive-top"><div style="margin-left: 5px;"><input type="checkbox" id="chkAMSCarAuto" ><label class="k-checkbox-label" for="chkAMSCarAuto"><strong>Allow Carrier Appointment Automation</strong></label></div></td>                            
                          </tr>                       
                      </table>                    
                  </div>                
              </div>                
              <div class="tblResponsive-wrap" style="float: none;">&nbsp;</div>
              <input id="txtSelectedLEControl" type="hidden" />
          </div>

          <div id="wndLEChangeOrderLaneComp">   
              <div id="lblLEToChange"></div>                  
              <div style="float: left;">                  
                  <div style="float: left;">                        
                      <table class="tblResponsive">                           
                          <tr>                               
                              <td class="tblResponsive-top">
                                  Remap Origin and Destination data for this Legal Entity.
                                  <br />
                                  Company/Warehouse location data will be replaced. 
                                  <br />
                                  for all orders in 'N' or 'P' status
                                  <br />
                                  and all active Lanes.
                                  <br />
                                  This procedure will change all reference for Old Company to New Company
                                  <br />
                                  <strong style="color:red">Warning this process cannot be undone!</strong>
                              </td>                           
                          </tr> 
                          <tr>                            
                              <td class="tblResponsive-top">
                                  <div style="margin-left: 5px;">
                                    <label><strong>Select Old Company to Change</strong></label>
                                    <br />
                                    <input id="ddlOldComp" />
                                </div>
                              </td>                            
                          </tr>                           
                          <tr>                            
                              <td class="tblResponsive-top">
                                  <div style="margin-left: 5px;">
                                    <label><strong>Select New Company to Map Orig and Dest</strong></label>
                                    <br />
                                    <input id="ddlNewComp" />
                                </div>
                              </td>                            
                          </tr>                       
                      </table>                    
                  </div>                
              </div>                
              <div class="tblResponsive-wrap" style="float: none;">&nbsp;</div>
              <input id="txtChangeLEControl" type="hidden" />
          </div>

    <% Response.WriteFile("~/Views/LegalEntityEditWindow.html"); %>       
    <% Response.Write(PageTemplates); %> 
      
    <%-- HelpWindow needs to go after AuthNotification because it uses the notification object created there --%>
    <% Response.Write(AuthLoginNotificationHTML); %>   
    <% Response.WriteFile("~/Views/HelpWindow.html"); %>       
    <script>

        <% Response.Write(ADALPropertiesjs); %>

        var PageControl = '<%=PageControl%>'; 
        var oLEAdminGrid = null;
        var wndLEAEdit = kendo.ui.Window;
        var wndAMSCarAuto = kendo.ui.Window;        
        var wndLEChangeOrderLaneComp = kendo.ui.Window;
        var tObj = this;
        var tPage = this;      
        var blnIsEdit;
        var iLEAdminPK = 0;
        var leAdminGridSelectedRow;
        var leAdminGridSelectedRowDataItem; 
        var sLECompName = "Undefined";
        var sCompNumber = "0";
        var iLECompControl = 0;

        <% Response.Write(NGLOAuth2); %>

        

        <% Response.Write(PageCustomJS); %>
      
        //************* Action Menu Functions ********************
        function execActionClick(btn, proc){
            if(btn.id == "btnAddLE"){ openLEAAddWindow(); }
            else if (btn.id == "btnAMSCarAuto" ){ if (isLEAdminSelected() === true){openAMSCarAutoWnd();} }
            else if (btn.id == "btnRefresh" || btn === "Refresh" ){ refresh(); }
            else if(btn.id == "btnResetCurrentUserConfig"){ resetCurrentUserConfig(PageControl); }
            else if(btn.id == "btnChangeOrderLaneComp"){ openLEChangeOrderLaneCompWnd(); }
            else if (btn.id == "btnResetSecuritySettings") { ResetSecuritySettings(); }
            else if (btn.id == "btnAllUsersMustChangePassword") { AllUsersMustChangePassword(); }
        }

        function refresh() { ngl.readDataSource(oLEAdminGrid); }

        function refreshLEAdminGrid() { ngl.readDataSource(oLEAdminGrid); }

        

        function saveLEPK() {
            try {
                leAdminGridSelectedRow = oLEAdminGrid.select();
                if (typeof (leAdminGridSelectedRow) === 'undefined' || leAdminGridSelectedRow == null) { ngl.showValidationMsg("Legal Entity Record Required", "Please select a Legal Entity to continue", tPage); return false; }                              
                leAdminGridSelectedRowDataItem = oLEAdminGrid.dataItem(leAdminGridSelectedRow); //Get the dataItem for the selected row
                if (typeof (leAdminGridSelectedRowDataItem) === 'undefined' || leAdminGridSelectedRowDataItem == null) { ngl.showValidationMsg("Legal Entity Record Required", "Please select a Legal Entity to continue", tPage); return false; } 
                if ("LEAdminControl" in leAdminGridSelectedRowDataItem){                
                    iLEAdminPK = leAdminGridSelectedRowDataItem.LEAdminControl;
                    var setting = {name:'pk', value: iLEAdminPK.toString()};
                    var oCRUDCtrl = new nglRESTCRUDCtrl();
                    var blnRet = oCRUDCtrl.update("LegalEntity/PostPageSetting", setting, tPage, "savePostPageSettingSuccessCallback", "savePostPageSettingAjaxErrorCallback", tPage);                    
                    if ("CompName" in leAdminGridSelectedRowDataItem){
                        sLECompName = leAdminGridSelectedRowDataItem.CompName;
                    }
                    if ("CompNumber" in leAdminGridSelectedRowDataItem){
                        sCompNumber = leAdminGridSelectedRowDataItem.CompNumber;
                    }
                    if ("LECompControl" in leAdminGridSelectedRowDataItem){
                        iLECompControl = leAdminGridSelectedRowDataItem.LECompControl;
                    }
                    return true;
                } else { ngl.showValidationMsg("Legal Entity Record Required", "Invalid Record Identifier, please select a Legal Entity to continue", tPage); return false; }
            } catch (err) { ngl.showErrMsg("Cannot Continue", "There was an unexpected error, please reload the screen. If this continues please contact technical support. Error: " + err.message, tPage); }           
        }
    
        function isLEAdminSelected() {
            if (typeof (iLEAdminPK) === 'undefined' || iLEAdminPK === null || iLEAdminPK === 0) { return saveLEPK(); }
            return true;
        }
       
        //*************  Call Back Function **********************
        var blnLEAdminGridChangeBound = false;
        function LEAdminGridDataBoundCallBack(e,tGrid){           
            oLEAdminGrid = tGrid;
            if (blnLEAdminGridChangeBound == false){
                oLEAdminGrid.bind("change", saveLEPK);
                blnLEAdminGridChangeBound = true;
            } 
        }
       
        function savePostPageSettingSuccessCallback(results){
            //for now do nothing when we save the pk
        }
        function savePostPageSettingAjaxErrorCallback(xhr, textStatus, error){
            //for now do nothing when we save the pk
        }

        //************* Functions **********************
        function openAMSCarAutoWnd() {
            
            if (typeof (oLEAdminGrid) !== 'undefined' && ngl.isObject(oLEAdminGrid)) {                    
                if (typeof (leAdminGridSelectedRowDataItem) !== 'undefined' && leAdminGridSelectedRowDataItem != null) { 
                    if ('LEAdminControl' in leAdminGridSelectedRowDataItem) { $("#txtSelectedLEControl").val(leAdminGridSelectedRowDataItem.LEAdminControl); }
                    if ('LegalEntity' in leAdminGridSelectedRowDataItem) { $("#lblLE").html("<h3>" + leAdminGridSelectedRowDataItem.LegalEntity + "</h3>"); }
                    if ('LEAdminCarApptAutomation' in leAdminGridSelectedRowDataItem) {
                        if (leAdminGridSelectedRowDataItem.LEAdminCarApptAutomation) { $("#chkAMSCarAuto").prop('checked', true); } else { $("#chkAMSCarAuto").prop('checked', false); }
                    }                    
                } else { ngl.showErrMsg("Legal Entity Required", "Please select a record", null); return; }
                wndAMSCarAuto.center().open();
            }        
        }
      
        function SaveAMSCarAuto() {   
            var tfAMSCarAuto = false;
            if ($('#chkAMSCarAuto').is(":checked")) { tfAMSCarAuto = true; }
            var item = new InsertOrUpdateLE(); 
            item.LEAdminControl = $("#txtSelectedLEControl").val();       
            item.LEAdminCarApptAutomation = tfAMSCarAuto;      
            $.ajax({
                async: false,
                type: "POST",
                url: "api/LegalEntity/ChangeAMSCarrierAuto",
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                data: JSON.stringify(item),
                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                success: function (data) {     
                    try {
                        var blnSuccess = false;
                        var blnErrorShown = false;
                        var strValidationMsg = "";
                        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                            if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Change AMS Carrier Automation Failure", data.Errors, null); }
                            else {
                                if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                    if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                        blnSuccess = true;
                                        refreshLEAdminGrid();
                                    }
                                }
                            }
                        }
                        if (blnSuccess === false && blnErrorShown === false) {
                            if (strValidationMsg.length < 1) { strValidationMsg = "Change AMS Carrier Automation Failure"; }
                            ngl.showErrMsg("Change AMS Carrier Automation Failure", strValidationMsg, null);
                        }
                    } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                },
                error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure"); ngl.showErrMsg("Change AMS Carrier Automation Failure", sMsg, null); }
            });
            wndAMSCarAuto.close();           
        }

        function openLEChangeOrderLaneCompWnd() {
            
            if (typeof (oLEAdminGrid) !== 'undefined' && ngl.isObject(oLEAdminGrid)) {                    
                if (typeof (leAdminGridSelectedRowDataItem) !== 'undefined' && leAdminGridSelectedRowDataItem != null) { 
                    if ('LEAdminControl' in leAdminGridSelectedRowDataItem) { $("#txtSelectedLEControl").val(leAdminGridSelectedRowDataItem.LEAdminControl); }
                    if ('LegalEntity' in leAdminGridSelectedRowDataItem) { $("#lblLEToChange").html("<h3>" + leAdminGridSelectedRowDataItem.LegalEntity + "</h3>"); }
                                       
                } else { ngl.showErrMsg("Legal Entity Required", "Please select a record", null); return; }
                wndLEChangeOrderLaneComp.center().open();
            }        
        }
      
        function SavewndLEOrderLaneCompChanges() {   
            var item = new GenericResult();            
            var dataOldComp = $("#ddlOldComp").data("kendoDropDownList").dataItem();
            if (typeof (dataOldComp.Control) === 'undefined' || dataOldComp.Control === null || dataOldComp.Control < 1 ){ ngl.showWarningMsg("Cannot Change Order or Lane Company Details", "Old Company Data is not available. Please try again", tObj); return;}
            item.intField1 = dataOldComp.Control;
            var dataNewComp = $("#ddlNewComp").data("kendoDropDownList").dataItem();
            if (typeof (dataNewComp.Control) === 'undefined' || dataNewComp.Control === null || dataNewComp.Control < 1 ){ ngl.showWarningMsg("Cannot Change Order or Lane Company Details", "New Company Data is not available. Please try again", tObj); return;}
            item.intField2 = dataNewComp.Control;
            ngl.OkCancelConfirmation(
                       "Change Order or Lane Company Details",
                       "This process will replace all old company origin and destination information with new company data for all orders in N or P status and all Active Lanes. It may take some time to complete. This process cannot be undone. Are you sure you want to continue?",
                       400,
                       400,
                       tPage,
                       "UpdateLEOrderLaneCompChanges",
                       item);    

            wndLEChangeOrderLaneComp.close();           
        }
        function UpdateLEOrderLaneCompChanges(iRet,item) {
            if (typeof (iRet) === 'undefined' || iRet === null  || iRet === 0 ) { return;}           
            if (typeof (item) !== 'undefined' && ngl.isObject(item)) {
                
            
            $.ajax({
                async: false,
                type: "POST",
                url: "api/LegalEntity/ChangeOrdersLanesComp",
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                data: JSON.stringify(item),
                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                success: function (data) {     
                    try {
                        var blnSuccess = false;
                        var blnErrorShown = false;
                        var strValidationMsg = "";
                        // data contains a Models.Response object the Models.Response.Data contains an array of  Models.GenericResult
                        // if the update fails data.Errors will contain the message from the server 
                        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                            if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Change Order or Lane Company Details Failure", data.Errors, null); }
                            else {
                                //if no errors and Models.Response.Data has one item this is a success.  The data may be use in the future for more messages but for now it is ignored
                                if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                    if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                        blnSuccess = true;
                                        refreshLEAdminGrid();
                                    }
                                }
                            }
                        }
                        if (blnSuccess === false && blnErrorShown === false) {
                            if (strValidationMsg.length < 1) { strValidationMsg = "Change Order or Lane Company Details Failure"; }
                            ngl.showErrMsg("Change Order or Lane Company Details Failure", strValidationMsg, null);
                        }
                    } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                },
                error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure"); ngl.showErrMsg("Change Order or Lane Company Details Failure", sMsg, null); }
            });
            }
        }

        function ResetSecuritySettings(){
          
            if (typeof (iLEAdminPK) === 'undefined' || iLEAdminPK === null || iLEAdminPK == 0 ){ ngl.showWarningMsg("Cannot Reset Security Settings", "Please select a Legal Entity and try again", tObj); return;}

            ngl.OkCancelConfirmation(
                    "Reset Security Settings",
                    "This process will reset all the security settings to default for the selected Legal Entity, it may take some time to complete. It cannot be undone. Are you sure you want to continue?",
                    400,
                    400,
                    tPage,
                    "execResetSecuritySettings",
                    iLEAdminPK);           
        }

        function execResetSecuritySettings(iRet, iKey) {
            if (typeof (iRet) === 'undefined' || iRet === null  || iRet === 0 ) { return;}   
            if (typeof (iKey) === 'undefined' || iKey === null  || iKey === 0 ) { ngl.showWarningMsg("Cannot Reset Security Settings", "The selected Legal Entity was lost.  Please select a Legal Entity and try again", tObj); return;}           

            var item = new GenericResult();
            item.Control = iKey; //this is the primary key

            var oCRUDCtrl = new nglRESTCRUDCtrl();
            var blnRet = oCRUDCtrl.update("LegalEntity/ResetSecuritySettings", item, tObj, "ResetSecuritySettingsCallback", "ResetSecuritySettingsAjaxErrorCallback") ;
        }

        function ResetSecuritySettingsCallback(data) {
            var oResults = new nglEventParameters();
            oResults.source = "ResetSecuritySettingsCallback";
            oResults.widget = this;
            oResults.msg = 'Failed'; //set default to Failed     
           
            try {
                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                    if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                        oResults.error = new Error();
                        oResults.error.name = "Reset Security Settings Failure";
                        oResults.error.message = data.Errors;
                        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                    } else {
                        oResults.msg = "Success." + "  All security settings have been reset to default."
                        ngl.showSuccessMsg(oResults.msg, tObj);
                    }
                }
            } catch (err) {
                oResults.error = err
                ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
            }
          
        }

        function ResetSecuritySettingsAjaxErrorCallback(xhr, textStatus, error) {
            var oResults = new nglEventParameters();
            var tObj = this;
            oResults.source = "ResetSecuritySettingsAjaxErrorCallback";
            oResults.widget = this;
            oResults.msg = 'Failed'; //set default to Failed  
            oResults.error = new Error();
            oResults.error.name = "Reset Security Settings Failure"
            oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
            ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);          

        }

        //AllUsersMustChangePassword logic

        function AllUsersMustChangePassword() {

            if (typeof (iLEAdminPK) === 'undefined' || iLEAdminPK === null || iLEAdminPK == 0) { ngl.showWarningMsg("Cannot force all users to change password", "Please select a Legal Entity and try again", tObj); return; }
            blnActivate = true;
            ngl.OkCancelConfirmation(
                "Force All Users to Change Passord",
                "This process will set a flag for all users to force them to change their passwords,  you must be logged in as an administrator for the selected Legal Entity.  Do not run this if you are a Super User. This process can only be reversed by NGL. Are you sure you want to continue?",
                400,
                400,
                tPage,
                "execAllUsersMustChangePassword",
                blnActivate);
        }

        function execAllUsersMustChangePassword(iRet, blnActivate) {
            if (typeof (iRet) === 'undefined' || iRet === null || iRet === 0) { return; }
            //if (typeof (iKey) === 'undefined' || blnActivate === null) { ngl.showWarningMsg("Cannot force all users to change password", "Please select a Legal Entity and try again", tObj); return; }
            if (typeof (iLEAdminPK) === 'undefined' || iLEAdminPK === null || iLEAdminPK == 0) { ngl.showWarningMsg("Cannot force all users to change password", "Please select a Legal Entity and try again", tObj); return; }

            var item = new GenericResult();
            item.blnField1 = blnActivate;
            item.Control = iLEAdminPK

            var oCRUDCtrl = new nglRESTCRUDCtrl();
            var blnRet = oCRUDCtrl.update("LegalEntity/AllUsersMustChangePassword", item, tObj, "AllUsersMustChangePasswordCallback", "AllUsersMustChangePasswordAjaxErrorCallback");
        }

        function AllUsersMustChangePasswordCallback(data) {
            var oResults = new nglEventParameters();
            oResults.source = "AllUsersMustChangePasswordCallback";
            oResults.widget = this;
            oResults.msg = 'Failed'; //set default to Failed     

            try {
                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                    console.log(data);
                    if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                        oResults.error = new Error();
                        oResults.error.name = "Force All Users to Change Password Failure";
                        oResults.error.message = data.Errors;
                        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                    } else {
                        oResults.msg = "Success." + "  All users must now change their password."
                        ngl.showSuccessMsg(oResults.msg, tObj);
                    }
                }
            } catch (err) {
                oResults.error = err
                ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
            }

        }

        function AllUsersMustChangePasswordAjaxErrorCallback(xhr, textStatus, error) {
            var oResults = new nglEventParameters();
            var tObj = this;
            oResults.source = "AllUsersMustChangePasswordAjaxErrorCallback";
            oResults.widget = this;
            oResults.msg = 'Failed'; //set default to Failed  
            oResults.error = new Error();
            oResults.error.name = "Force All Users to Change Password Failure"
            oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
            ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);

        }

        // Add Legal Entity Logic
        function openLEAAddWindow(){            
            $("#wndLEAEdit").data("kendoWindow").title("Add New Legal Entity"); //Change the window title to "Add"
            blnIsEdit = false;
            //Clear radion button group
            $('input[name=rbgCreateLE]').attr('checked',false);
            $("#divCompRadioButtons").show();
            //Hide both until make choice
            $("#divExistingComp").hide();
            $("#divNewComp").hide();
            //Hide "Last Assigned" on Add
            $("#divLastAssigned").hide();
            //Clear LEAControl for insert new
            $("#txtLEAControl").val(0); 
            //set default values
            $("#txtCNSNumberLow").data("kendoNumericTextBox").value(1111);
            $("#txtCNSNumberHigh").data("kendoNumericTextBox").value(99999);
            $("#txtLEAdminSecurityLevel").data("kendoNumericTextBox").value(1);
            $("#txtCNSPrefix").data("kendoMaskedTextBox").value("CNS");
            $("#txtAutoAssignOrderSeqSeed").data("kendoNumericTextBox").value(0);
            $("#chkAllowCreateOrderSeq").prop('checked', true);
            $("#chkNewCompActive").prop('checked', true);
            $("#chkNewCompContTender").prop('checked', false);
            $("#txtLEAdminCarrierAcceptLoadMins").data("kendoNumericTextBox").value(0);
            $("#txtLEAdminExpiredLoadsTo").data("kendoMaskedTextBox").value("");
            $("#txtLEAdminExpiredLoadsCc").data("kendoMaskedTextBox").value("");          
            var dropdownlist = $("#ddlComps").data("kendoDropDownList");
            dropdownlist.readonly(false);
            dropdownlist.select(0);
            //Show all the red "required field" * on Add
            $(".redRequiredAsterik").show();
            wndLEAEdit.center().open();
        }

        function openLEAEditWindow(e) {           
            $("#wndLEAEdit").data("kendoWindow").title("Edit Legal Entity"); //Change the window title to "Edit"
            blnIsEdit = true;
            //Default check radion button group (so validateRequiredFields wont throw error on Save)
            //$('input[id=rbExisting]').attr('checked',true);
            $("#rbExisting").prop("checked", true);
            //Hide "New Comp" section
            $("#divNewComp").hide();
            $("#divCompRadioButtons").hide();
            $("#divExistingComp").show();
            //Get the record from the Grid
            var item = this.dataItem($(e.currentTarget).closest("tr")); 
            if(!('LEAdminControl' in item)){ ngl.showErrMsg("Unexpected Error - Contact IT", "Cannot get LEAdminControl from the grid. Check to make sure all field names match across all objects.", null); return; }
            if(!('LECompControl' in item)){ ngl.showErrMsg("Unexpected Error - Contact IT", "Cannot get LECompControl from the grid. Check to make sure all field names match across all objects.", null); return; }
            if(!('LegalEntity' in item)){ ngl.showErrMsg("Unexpected Error - Contact IT", "Cannot get LegalEntity from the grid. Check to make sure all field names match across all objects.", null); return; }
            //Save the Control from the record
            $("#txtLEAControl").val(item.LEAdminControl);                           
            //Set the value of the Company from the record and set the ddl to readonly (not editable)
            var dropdownlist = $("#ddlComps").data("kendoDropDownList");
            dropdownlist.select(function(dataItem) { return dataItem.Control === item.LECompControl; });
            //dropdownlist.readonly();
            //Set the value of Legal Entity from the record (this is the only user editable field)
            $("#txtLegalEntity").data("kendoMaskedTextBox").value(item.LegalEntity);  
            //Set the other values from the record                               
            $("#txtCNSNumberLow").data("kendoNumericTextBox").value(item.CNSNumberLow);
            $("#txtCNSNumberHigh").data("kendoNumericTextBox").value(item.CNSNumberHigh);
            if (item.LEAdminSecurityLevel) {
                alert(item.LEAdminSecurityLevel.toString());
                $("#txtLEAdminSecurityLevel").data("kendoNumericTextBox").value(item.LEAdminSecurityLevel);
            }
            $("#txtCNSNumber").data("kendoNumericTextBox").value(item.LEAdminCNSNumber);
            $("#txtPRONumber").data("kendoNumericTextBox").value(item.LEAdminPRONumber);
            $("#txtCNSPrefix").data("kendoMaskedTextBox").value(item.CNSPrefix);
            if ('AllowCreateOrderSeq' in item) {
                if (item.AllowCreateOrderSeq) { $("#chkAllowCreateOrderSeq").prop('checked', true); } else { $("#chkAllowCreateOrderSeq").prop('checked', false); }
            }
            $("#txtAutoAssignOrderSeqSeed").data("kendoNumericTextBox").value(item.AutoAssignOrderSeqSeed);
            $("#txtBOLLegalText").data("kendoMaskedTextBox").value(item.LEAdminBOLLegalText);
            $("#txtDispatchLegalText").data("kendoMaskedTextBox").value(item.LEAdminDispatchLegalText);
            //Show "Last Assigned" on Edit as ReadOnly
            $("#divLastAssigned").show();
            $("#txtCNSNumber").data("kendoNumericTextBox").readonly();
            $("#txtPRONumber").data("kendoNumericTextBox").readonly();
            //Only show the AMS settings if Carrier Automation is turned on (only super user can turn this field on/off)
            if (item.LEAdminCarApptAutomation) { $("#chkCarApptAutomation").prop('checked', true); $("#divAMSSettings").show(); } else { $("#chkCarApptAutomation").prop('checked', false); $("#divAMSSettings").hide(); }
            //Get the rest of the fields
            if (item.LEAdminAllowApptEdit) { $("#chkAllowApptEdit").prop('checked', true); } else { $("#chkAllowApptEdit").prop('checked', false); }
            if (item.LEAdminAllowApptDelete) { $("#chkAllowApptDelete").prop('checked', true); } else { $("#chkAllowApptDelete").prop('checked', false); }
            $("#txtLEAdminApptModCutOffMinutes").data("kendoNumericTextBox").value(item.LEAdminApptModCutOffMinutes);
            $("#txtLEAdminDefaultLastLoadTime").data("kendoMaskedTextBox").value(item.LEAdminDefaultLastLoadTime);
            //$("#txtLEAdminApptNotSetAlertMinutes").data("kendoNumericTextBox").value(item.LEAdminApptNotSetAlertMinutes); //LVV Changed 1/4/19 - Commented out for now because this functionality does not actually exist yet
            $("#txtLEAdminCarrierAcceptLoadMins").data("kendoNumericTextBox").value(item.LEAdminCarrierAcceptLoadMins);
            $("#txtLEAdminExpiredLoadsTo").data("kendoMaskedTextBox").value(item.LEAdminExpiredLoadsTo);
            $("#txtLEAdminExpiredLoadsCc").data("kendoMaskedTextBox").value(item.LEAdminExpiredLoadsCc);
            //Hide all the red "required field" * on Edit
            $(".redRequiredAsterik").hide();
            wndLEAEdit.center().open();
        }

        function SaveLEA() {
            var otmp = $("#focusCancel").focus();
            //make sure all required fields are populated on Add
            if(!blnIsEdit) {  
                var alertMsg = "";
                alertMsg = validateRequiredFields();
                if (!ngl.isNullOrWhitespace(alertMsg)){
                    //kendo.ui.progress($(document.body), false);
                    ngl.showErrMsg("Required Fields", alertMsg, null);
                    return;
                }
            }
            var item = new InsertOrUpdateLE();         
            //LE fields
            item.LEAdminControl = $("#txtLEAControl").val(); 
            item.LegalEntity = $("#txtLegalEntity").data("kendoMaskedTextBox").value();
            item.LEAdminCNSPrefix = $("#txtCNSPrefix").data("kendoMaskedTextBox").value();
            item.LEAdminCNSNumberLow = $("#txtCNSNumberLow").data("kendoNumericTextBox").value();
            item.LEAdminCNSNumberHigh = $("#txtCNSNumberHigh").data("kendoNumericTextBox").value();
            item.LEAdminSecurityLevel = $("#txtLEAdminSecurityLevel").data("kendoNumericTextBox").value();
            item.LEAdminCNSNumber = $("#txtCNSNumber").data("kendoNumericTextBox").value();
            item.LEAdminPRONumber = $("#txtPRONumber").data("kendoNumericTextBox").value();
            var tfAllowCreateOrderSeq = false;
            if ($('#chkAllowCreateOrderSeq').is(":checked")) { tfAllowCreateOrderSeq = true; }
            item.LEAdminAllowCreateOrderSeq = tfAllowCreateOrderSeq;
            item.LEAdminAutoAssignOrderSeqSeed = $("#txtAutoAssignOrderSeqSeed").data("kendoNumericTextBox").value();        
            var tfLEAdminAllowApptEdit = false;
            var tfLEAdminAllowApptDelete = false;
            if ($('#chkAllowApptEdit').is(":checked")) { tfLEAdminAllowApptEdit = true; }
            if ($('#chkAllowApptDelete').is(":checked")) { tfLEAdminAllowApptDelete = true; }
            item.LEAdminApptModCutOffMinutes = $("#txtLEAdminApptModCutOffMinutes").data("kendoNumericTextBox").value();
            item.LEAdminDefaultLastLoadTime = $("#txtLEAdminDefaultLastLoadTime").data("kendoMaskedTextBox").value();
            //item.LEAdminApptNotSetAlertMinutes = $("#txtLEAdminApptNotSetAlertMinutes").data("kendoNumericTextBox").value(); //LVV Changed 1/4/19 - Commented out for now because this functionality does not actually exist yet
            item.LEAdminAllowApptEdit = tfLEAdminAllowApptEdit;
            item.LEAdminAllowApptDelete = tfLEAdminAllowApptDelete;
            var tfCarApptAutomation = false;
            if ($('#chkCarApptAutomation').is(":checked")) { tfCarApptAutomation = true; }
            item.LEAdminCarApptAutomation = tfCarApptAutomation;
            item.LEAdminBOLLegalText = $("#txtBOLLegalText").data("kendoMaskedTextBox").value();
            item.LEAdminDispatchLegalText = $("#txtDispatchLegalText").data("kendoMaskedTextBox").value();
            item.LEAdminCarrierAcceptLoadMins = $("#txtLEAdminCarrierAcceptLoadMins").data("kendoNumericTextBox").value();
            item.LEAdminExpiredLoadsTo = $("#txtLEAdminExpiredLoadsTo").data("kendoMaskedTextBox").value();
            item.LEAdminExpiredLoadsCc = $("#txtLEAdminExpiredLoadsCc").data("kendoMaskedTextBox").value();
            //Comp fields
            item.CompControl = 0;
            if(document.getElementById('rbExisting').checked) {
                var dataItemC = $("#ddlComps").data("kendoDropDownList").dataItem();
                item.CompControl = dataItemC.Control;
            }
            item.CompName = $("#txtNewCompName").data("kendoMaskedTextBox").value();
            item.CompAlphaCode = $("#txtNewCompAlphaCode").data("kendoMaskedTextBox").value();
            item.CompAbrev = $("#txtNewCompProAbrv").data("kendoMaskedTextBox").value();
            item.CompWebsite = $("#txtNewCompWebsite").data("kendoMaskedTextBox").value();
            item.CompEmail = $("#txtNewCompEmail").data("kendoMaskedTextBox").value();
            item.CompAddress1 = $("#txtNewCompAddress1").data("kendoMaskedTextBox").value();
            item.CompAddress2 = $("#txtNewCompAddress2").data("kendoMaskedTextBox").value();
            item.CompAddress3 = $("#txtNewCompAddress3").data("kendoMaskedTextBox").value();
            item.CompCity = $("#txtNewCompCity").data("kendoMaskedTextBox").value();
            item.CompState = $("#txtNewCompState").data("kendoMaskedTextBox").value();
            item.CompZip = $("#txtNewCompZip").data("kendoMaskedTextBox").value();
            item.CompCountry = $("#txtNewCompCountry").data("kendoMaskedTextBox").value();
            var tfCompActive = false;
            if ($('#chkNewCompActive').is(":checked")) { tfCompActive = true; }
            item.CompActive = tfCompActive;
            //CompCont fields
            item.CompContName = $("#txtNewCompContName").data("kendoMaskedTextBox").value();
            item.CompContTitle = $("#txtNewCompContTitle").data("kendoMaskedTextBox").value();
            item.CompCont800 = $("#txtNewCompCont800").data("kendoMaskedTextBox").value();
            item.CompContPhone = $("#txtNewCompContPhone").data("kendoMaskedTextBox").value();
            item.CompContPhoneExt = $("#txtNewCompContPhoneExt").data("kendoMaskedTextBox").value();
            item.CompContFax = $("#txtNewCompContFax").data("kendoMaskedTextBox").value();
            item.CompContEmail = $("#txtNewCompContEmail").data("kendoMaskedTextBox").value();
            var tfCompContTender = false;
            if ($('#chkNewCompContTender').is(":checked")) { tfCompContTender = true; } 
            item.CompContTender = tfCompContTender;             
            $.ajax({
                async: false,
                type: "POST",
                url: "api/LegalEntity/InsertOrUpdateLegalEntity",
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                data: JSON.stringify(item),
                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                success: function (data) {     
                    try {
                        var blnSuccess = false;
                        var blnErrorShown = false;
                        var strValidationMsg = "";
                        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                            if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Save Legal Entity Failure", data.Errors, null); }
                            else {
                                if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                    if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                        blnSuccess = true;
                                        refreshLEAdminGrid();
                                    }
                                }
                            }
                        }
                        if (blnSuccess === false && blnErrorShown === false) {
                            if (strValidationMsg.length < 1) { strValidationMsg = "Save Legal Entity Failure"; }
                            ngl.showErrMsg("Save Legal Entity Failure", strValidationMsg, null);
                        }
                    } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                },
                error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure"); ngl.showErrMsg("Save Legal Entity Failure", sMsg, null); }
            });
            wndLEAEdit.close();           
        }
       
        //** NOTE: WE NEED TO TALK MORE ABOUT WHAT IT MEANS TO DELETE A LEGALENTITY **
        function deleteLEA(e){
            ////var item = this.dataItem($(e.currentTarget).closest("tr"));
            ////var lEAControl = 0;
            ////var msgPrompt = "Are you sure that you want to delete this record?";
            ////kendo.confirm(msgPrompt).then(function () {
            ////    //kendo.alert("You chose the Ok action.");
            ////    if (typeof (item) !== 'undefined' && item != null) { 
            ////        if ('LEAdminControl' in item) { lEAControl = item.LEAdminControl; }
            ////    } 
            ////    if (lEAControl == 0) { ngl.showErrMsg("Delete Error", "Could not get LEAdminControl from the record", null); return; }
            ////    $.ajax({
            ////        async: false,
            ////        url: "api/LegalEntity/DeleteLegalEntity/" + lEAControl,
            ////        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
            ////        contentType: "application/json; charset=utf-8",
            ////        dataType: 'json',
            ////        type: "DELETE",
            ////        success: function (data) {     
            ////            try {
            ////                var blnSuccess = false;
            ////                var blnErrorShown = false;
            ////                var strValidationMsg = "";
            ////                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
            ////                    if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
            ////                        blnErrorShown = true;
            ////                        ngl.showErrMsg("Delete Legal Entity Failure", data.Errors, null);
            ////                    }
            ////                    else {
            ////                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
            ////                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
            ////                                blnSuccess = true;
            ////                            }
            ////                        }
            ////                    }
            ////                }
            ////                if (blnSuccess === false && blnErrorShown === false) {
            ////                    if (strValidationMsg.length < 1) { strValidationMsg = "Delete Legal Entity Failure"; }
            ////                    ngl.showErrMsg("Delete Legal Entity Failure", strValidationMsg, null);
            ////                }
            ////            } catch (err) {
            ////                ngl.showErrMsg(err.name, err.description, null);
            ////            }
            ////        },
            ////        error: function (xhr, textStatus, error) {
            ////            var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Delete Data Failure");
            ////            ngl.showErrMsg("Delete Legal Entity Failure", sMsg, null);                        
            ////        }
            ////    });
            ////    refreshLEAdminGrid();
            ////}, function () {
            ////    //kendo.alert("You chose to Cancel action.");
            ////    refreshLEAdminGrid();
            ////});
        }

        //OnChange event handler for radio buttons
        $('input[type=radio][name=rbgCreateLE]').on('change', function() {
            switch($(this).val()) {
                case 'existing':
                    $("#divExistingComp").show();
                    $("#divNewComp").hide();
                    break;
                case 'new':
                    $("#divNewComp").show();
                    $("#divExistingComp").hide();
                    break;
            }
        });

        function validateRequiredFields(){
            var fields = "";
            var strSp = "";
            var intTemp = 0;
            //An associated company is required
            if(!document.getElementById('rbExisting').checked && !document.getElementById('rbCreateNew').checked){
                fields = "Company Required. Either select and existing company or create a new one.";
                return fields;
            }                  
            //Legal Entity required fields
            if (ngl.isNullOrWhitespace($("#txtLegalEntity").data("kendoMaskedTextBox").value())){ fields += (strSp + "Legal Entity"); strSp = ", "; }
            if (ngl.isNullOrWhitespace($("#txtCNSPrefix").data("kendoMaskedTextBox").value())){ fields += (strSp + "CNS Prefix"); strSp = ", "; }
            intTemp = $("#txtCNSNumberLow").data("kendoNumericTextBox").value()
            if (typeof (intTemp) === 'undefined' || intTemp === null || intTemp < 1 ){ fields += (strSp + "CNS Number Low"); strSp = ", "; }
            intTemp = 0;
            intTemp = $("#txtCNSNumberHigh").data("kendoNumericTextBox").value()
            if (typeof (intTemp) === 'undefined' || intTemp === null || intTemp < 1 ){ fields += (strSp + "CNS Number High"); strSp = ", "; }
            intTemp = 0;
            intTemp = $("#txtAutoAssignOrderSeqSeed").data("kendoNumericTextBox").value()
            if (typeof (intTemp) === 'undefined' || intTemp === null || intTemp < 0 ){ fields += (strSp + "Auto Assign Order Seq Seed"); strSp = ", "; }
            //Company
            if(document.getElementById('rbExisting').checked) {
                //Use Existing Comp required fields
                var dataItemC = $("#ddlComps").data("kendoDropDownList").dataItem();
                if (typeof (dataItemC.Control) === 'undefined' || dataItemC.Control === null || dataItemC.Control < 1 ){ fields += (strSp + "Existing Company"); strSp = ", "; }
            }else if(document.getElementById('rbCreateNew').checked) {          
                //Create New Comp required fields
                if (ngl.isNullOrWhitespace($("#txtNewCompName").data("kendoMaskedTextBox").value())){ fields += (strSp + "Comp Name"); strSp = ", "; }
                if (ngl.isNullOrWhitespace($("#txtNewCompAlphaCode").data("kendoMaskedTextBox").value())){ fields += (strSp + "Location Code"); strSp = ", "; }
                if (ngl.isNullOrWhitespace($("#txtNewCompProAbrv").data("kendoMaskedTextBox").value())){ fields += (strSp + "Pro Abrv"); strSp = ", "; }
                if (ngl.isNullOrWhitespace($("#txtNewCompEmail").data("kendoMaskedTextBox").value())){ fields += (strSp + "Comp Email"); strSp = ", "; }
                if (ngl.isNullOrWhitespace($("#txtNewCompAddress1").data("kendoMaskedTextBox").value())){ fields += (strSp + "Address 1"); strSp = ", "; }
                if (ngl.isNullOrWhitespace($("#txtNewCompCity").data("kendoMaskedTextBox").value())){ fields += (strSp + "City"); strSp = ", "; }
                if (ngl.isNullOrWhitespace($("#txtNewCompState").data("kendoMaskedTextBox").value())){ fields += (strSp + "State"); strSp = ", "; }
                if (ngl.isNullOrWhitespace($("#txtNewCompZip").data("kendoMaskedTextBox").value())){ fields += (strSp + "Postal Code"); strSp = ", "; }
                if (ngl.isNullOrWhitespace($("#txtNewCompCountry").data("kendoMaskedTextBox").value())){ fields += (strSp + "Country"); strSp = ", "; }
                if (ngl.isNullOrWhitespace($("#txtNewCompContName").data("kendoMaskedTextBox").value())){ fields += (strSp + "Contact Name"); strSp = ", "; }
                if (ngl.isNullOrWhitespace($("#txtNewCompContEmail").data("kendoMaskedTextBox").value())){ fields += (strSp + "Contact Email"); strSp = ", "; }
            }
            return fields;
        }


        $(document).ready(function () {
            var PageMenuTab = <%=PageMenuTab%>;
            
          
            if (control != 0){
                //$("#txtLegalEntity").kendoMaskedTextBox();  GET INITIALIZED IN THE WINDOW VIEW LegalEntityEditWindow.html
                $("#txtLEAControl").val(0);

                ///////////COMPANY DDL/////////////
                $("#ddlComps").kendoDropDownList({
                    dataTextField: "Name",
                    dataValueField: "Control",
                    autoWidth: true,
                    filter: "contains",
                    dataSource: {
                        serverFiltering: false,
                        transport: {
                            read: {
                                url: "api/vLookupList/GetUserDynamicList/20",
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }
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
                        error: function (xhr, textStatus, error) { ngl.showErrMsg("Get Comps JSON Response Error",formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"), null);  this.cancelChanges(); }
                    }
                });

                ///////////Old COMPANY DDL/////////////
                $("#ddlOldComp").kendoDropDownList({
                    dataTextField: "Name",
                    dataValueField: "Control",
                    autoWidth: true,
                    filter: "contains",
                    dataSource: {
                        serverFiltering: false,
                        transport: {
                            read: {
                                url: "api/vLookupList/GetUserDynamicList/20",
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }
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
                        error: function (xhr, textStatus, error) { ngl.showErrMsg("Get Comps JSON Response Error",formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"), null);  this.cancelChanges(); }
                    }
                });

                
                //////////New COMPANY DDL/////////////
                $("#ddlNewComp").kendoDropDownList({
                    dataTextField: "Name",
                    dataValueField: "Control",
                    autoWidth: true,
                    filter: "contains",
                    dataSource: {
                        serverFiltering: false,
                        transport: {
                            read: {
                                url: "api/vLookupList/GetUserDynamicList/20",
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }
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
                        error: function (xhr, textStatus, error) { ngl.showErrMsg("Get Comps JSON Response Error",formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"), null);  this.cancelChanges(); }
                    }
                });

                //////////wndLEAEdit/////////////////
                wndLEAEdit = $("#wndLEAEdit").kendoWindow({
                    title: "Edit/Add",
                    height: ($(window).height() * .9),
                    width: 600,
                    modal: true,
                    visible: false,
                    actions: ["save", "Minimize", "Maximize", "Close"],
                    close: function(e) {
                        //clear all the values
                        $("#txtLEAControl").val(0);                                  
                        $("#txtLegalEntity").data("kendoMaskedTextBox").value("");
                        $("#txtCNSNumberLow").data("kendoNumericTextBox").value(1111);
                        $("#txtCNSNumberHigh").data("kendoNumericTextBox").value(99999);
                        $("#txtLEAdminSecurityLevel").data("kendoNumericTextBox").value(1);
                        $("#txtBOLLegalText").data("kendoMaskedTextBox").value("");
                        $("#txtDispatchLegalText").data("kendoMaskedTextBox").value("");
                        $("#txtLEAdminCarrierAcceptLoadMins").data("kendoNumericTextBox").value(0);
                        $("#txtLEAdminExpiredLoadsTo").data("kendoMaskedTextBox").value("");
                        $("#txtLEAdminExpiredLoadsCc").data("kendoMaskedTextBox").value("");                   
                        $("#chkCarApptAutomation").prop('checked', false);
                        $("#chkAllowApptEdit").prop('checked', false);
                        $("#chkAllowApptDelete").prop('checked', false);
                        $("#txtLEAdminDefaultLastLoadTime").data("kendoMaskedTextBox").value("15:00");
                        $("#txtLEAdminApptModCutOffMinutes").data("kendoNumericTextBox").value(2880);
                        //$("#txtLEAdminApptNotSetAlertMinutes").data("kendoNumericTextBox").value(2880); //LVV Changed 1/4/19 - Commented out for now because this functionality does not actually exist yet
                        $("#divAMSSettings").hide();
                        //Check "AllowCreateOrderSeq"
                        $("#chkAllowCreateOrderSeq").prop('checked', true);
                        $("#txtAutoAssignOrderSeqSeed").data("kendoNumericTextBox").value(0);
                        $("#txtCNSNumber").data("kendoNumericTextBox").value(1111);
                        $("#txtPRONumber").data("kendoNumericTextBox").value(1111);
                        $("#txtCNSPrefix").data("kendoMaskedTextBox").value("CNS");
                        $("#txtNewCompName").data("kendoMaskedTextBox").value("");
                        $("#txtNewCompAlphaCode").data("kendoMaskedTextBox").value("");
                        $("#txtNewCompProAbrv").data("kendoMaskedTextBox").value("");
                        $("#txtNewCompWebsite").data("kendoMaskedTextBox").value("");
                        $("#txtNewCompEmail").data("kendoMaskedTextBox").value("");
                        $("#txtNewCompAddress1").data("kendoMaskedTextBox").value("");
                        $("#txtNewCompAddress2").data("kendoMaskedTextBox").value("");
                        $("#txtNewCompAddress3").data("kendoMaskedTextBox").value("");
                        $("#txtNewCompCity").data("kendoMaskedTextBox").value("");
                        $("#txtNewCompState").data("kendoMaskedTextBox").value("");
                        $("#txtNewCompZip").data("kendoMaskedTextBox").value("");
                        $("#txtNewCompCountry").data("kendoMaskedTextBox").value("");
                        $("#txtNewCompContName").data("kendoMaskedTextBox").value("");
                        $("#txtNewCompContTitle").data("kendoMaskedTextBox").value("");
                        $("#txtNewCompCont800").data("kendoMaskedTextBox").value("");
                        $("#txtNewCompContPhone").data("kendoMaskedTextBox").value("");
                        $("#txtNewCompContPhoneExt").data("kendoMaskedTextBox").value("");
                        $("#txtNewCompContFax").data("kendoMaskedTextBox").value("");
                        $("#txtNewCompContEmail").data("kendoMaskedTextBox").value("");
                    }
                }).data("kendoWindow");
                //Modified by RHR for Kendo v-2018 xxx must reference .parent() on save button click.
                $("#wndLEAEdit").data("kendoWindow").wrapper.find(".k-svg-i-save").parent().click(function (e) { SaveLEA(); }); 
                $("#divAMSSettings").hide();

                //////////wndAMSCarAuto/////////////////
                wndAMSCarAuto = $("#wndAMSCarAuto").kendoWindow({
                    title: "AMS Carrier Self Service",
                    //height: ($(window).height() * .9),
                    width: 275,
                    modal: true,
                    visible: false,
                    actions: ["save", "Minimize", "Maximize", "Close"],
                    close: function(e) {
                        //clear all the values
                        $("#txtSelectedLEControl").val(0);  
                        $("#chkAMSCarAuto").prop('checked', false);                 
                    }
                }).data("kendoWindow");

                //Modified by RHR for Kendo v-2018 xxx must reference .parent() on save button click.
                $("#wndAMSCarAuto").data("kendoWindow").wrapper.find(".k-svg-i-save").parent().click(function (e) { SaveAMSCarAuto(); }); 

                //////////wndLEChangeOrderLaneComp/////////////////
                wndLEChangeOrderLaneComp = $("#wndLEChangeOrderLaneComp").kendoWindow({
                    title: "Change Order and Lane Company Mapping",
                    height: "60%", //($(window).height() * .9),
                    width: "60%",
                    modal: true,
                    visible: false,
                    actions: ["save", "Minimize", "Maximize", "Close"],
                    close: function(e) {
                        //clear all the values
                        //$("#txtSelectedLEControl").val(0);  
                        //$("#chkAMSCarAuto").prop('checked', false);                 
                    }
                }).data("kendoWindow");

                //Modified by RHR for Kendo v-2018 xxx must reference .parent() on save button click.
                $("#wndLEChangeOrderLaneComp").data("kendoWindow").wrapper.find(".k-svg-i-save").parent().click(function (e) { SavewndLEOrderLaneCompChanges(); }); 
            
            
            }
            var PageReadyJS = <%=PageReadyJS%>; 
            menuTreeHighlightPage(); //must be called after PageReadyJS
            var divWait = $("#h1Wait");
            if (typeof (divWait) !== 'undefined') { divWait.hide(); }
        });


    </script>
    <style>
        .k-grid tbody tr td { overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }
    </style>   
      </div>
    </body>
</html>

