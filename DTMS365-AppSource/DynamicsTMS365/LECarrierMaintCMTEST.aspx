<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LECarrierMaintCMTEST.aspx.cs" Inherits="DynamicsTMS365.LECarrierMaintCMTEST" %>

<!DOCTYPE html>

<html>
    <head >
        <title>DTMS LE Carrier Maintenance</title>
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
                <div id="menu-pane" style="height: 100%; width: 100%; background-color: white;"> 
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
                            <div id="pageContent" class="pane-content">

                                <% Response.Write(PageErrorsOrWarnings); %>
                                
                                <div id="divTitleLE"></div>

                                <!-- begin Page Content -->
                                <% Response.Write(FastTabsHTML); %>    
                                <!-- End Page Content -->

                            </div>                          
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
    <% Response.WriteFile("~/Views/CarrierSettingsWindow44.html"); %>          
    <% Response.WriteFile("~/Views/AccessorialSettingsWindow44.html"); %>

          <div id="wndCopyLEConfig">
              <div id="divCopyToLE"></div>
              <div><strong>Copy Config From</strong></div>
              <div>
                  <input id="ddlLEToCopy" style="width: 200px;" />
              </div>
              <div style="margin-top: 5px;">
                  <button class="k-button k-button-solid-base k-button-solid k-button-md k-rounded-md" type="button" id="btnCopyLEOk" onclick="btnCopyLEOk_Click();">Copy Config</button>
              </div>
          </div>

          <div id="wndBillToComp">
              <div id="lblCarrierName"></div>
              <div>
                  <input id="ddlBillToComps" style="width:200px;" />
              </div>
              <div style="margin-top:5px;">
                  <button class="k-button k-button-solid-base k-button-solid k-button-md k-rounded-md" type="button" onclick="btnSetBillToComp_Click();">Set</button>
              </div>
          </div>

          <div id="wndCopyLECarConfig">
              <div style="margin-bottom:5px;"><strong>Copy From: </strong><strong id="lblCopyFromLECarName"></strong></div>
              <div>
                  <strong>Copy To: </strong>
                  <div id="copyToGrid"></div>
              </div>
              <div style="margin-top:5px;">
                  <button class="k-button k-button-solid-base k-button-solid k-button-md k-rounded-md" type="button" id="btnCopyOk" onclick="btnCopyOk_Click();">Copy Config</button>
              </div>
              <input id="txtCopyFromLECarControl" type="hidden" />
          </div>

    <% Response.Write(PageTemplates); %>

    <%-- HelpWindow needs to go after AuthNotification because it uses the notification object created there --%>
    <% Response.Write(AuthLoginNotificationHTML); %> 
    <% Response.WriteFile("~/Views/HelpWindow.html"); %>        
    <script>    
        <% Response.Write(ADALPropertiesjs); %>

        var PageControl = '<%=PageControl%>';
        var oLECarSetGrid = null;
        var tObjPG = this;
        var pgLEControl = 0;
        var pgLEName = "";
        var tObj = this;
        var tPage = this;        
        var iLECarPK = 0;
        var selectedRowCarrierName = ""; 
        var leCarSetGridSelectedRowDataItem;  

        <% Response.Write(NGLOAuth2); %>

        
        <% Response.Write(PageCustomJS); %>

        var dsAccessorials = kendo.data.DataSource;
        var wndCarSet = kendo.ui.Window;
        var wndAccessorial = kendo.ui.Window;      
        var wndCopyLEConfig = kendo.ui.Window;
        var wndBillToComp = kendo.ui.Window;
        var wndCopyLECarConfig = kendo.ui.Window;

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
                oLECarSetGrid.dataSource.page(1);
                refreshLECarSetGrid();
            }
        }    
        //functions
        function changeLegalEntity() { oChangeLEDialogCtrl.show(); }


        //************* Primary Key Functions ********************
        function saveLECarPK() {
            try {
                var row = oLECarSetGrid.select();
                if (typeof (row) === 'undefined' || row == null) {              
                    ngl.showValidationMsg("Carrier Record Required", "Please select a Carrier to continue", tPage);
                    return false;
                }                   
                //Get the dataItem for the selected row
                leCarSetGridSelectedRowDataItem = oLECarSetGrid.dataItem(row);
                if (typeof (leCarSetGridSelectedRowDataItem) === 'undefined' || leCarSetGridSelectedRowDataItem == null) {              
                    ngl.showValidationMsg("Carrier Record Required", "Please select a Carrier to continue", tPage);
                    return false;
                }  
                if ("LECarControl" in leCarSetGridSelectedRowDataItem){        
                    //save the name of the selected carrier
                    if("CarrierName" in leCarSetGridSelectedRowDataItem){selectedRowCarrierName = leCarSetGridSelectedRowDataItem.CarrierName;}else{selectedRowCarrierName = "";}
                    iLECarPK = leCarSetGridSelectedRowDataItem.LECarControl;
                    var setting = {name:'pk', value: iLECarPK.toString()};
                    var oCRUDCtrl = new nglRESTCRUDCtrl();
                    var blnRet = oCRUDCtrl.update("LegalEntityCarrier/PostPageSetting", setting, tPage, "savePostPageSettingSuccessCallback", "savePostPageSettingAjaxErrorCallback",tPage);
                    return true;
                } else {
                    ngl.showValidationMsg("Carrier Record Required", "Invalid Record Identifier, please select a Carrier to continue", tPage);
                    return false;
                }
            } catch (err) {
                ngl.showErrMsg("Cannot Continue", "There was an unexpected error, please reload the screen. If this continues please contact technical support. Error: " + err.message, tPage);
            }           
        }

        function isLECarSelected() {
            if (typeof (iLECarPK) === 'undefined' || iLECarPK === null || iLECarPK === 0) { return saveLECarPK(); }
            return true;
        }



        //************* Action Menu Functions ********************
        function execActionClick(btn, proc){
            if(btn.id == "btnChangeLE"){ changeLegalEntity(); }
            else if(btn.id == "btnAddCarrier"){ openCarSetAddWindow(); }
            else if(btn.id == "btnAddCarAccessorial44"){ if (isLECarSelected() === true) {openAccessorialAddWindow();} }
            //else if(btn.id == "btnCopyCAMConfig"){ openCopyConfigWnd(); }
            else if(btn.id == "btnCopyLEConfig"){ openCopyLEConfigWnd(); }
            //else if(btn.id == "btnAddCarrCont44"){ openContactAddWindow(); }
            else if(btn.id == "btnEditBillToComp44"){ if (isLECarSelected() === true) {openEditBillToCompWindow();} }
            else if(btn.id == "btnCopyLECarFeeConfig"){ if (isLECarSelected() === true) {openCopyLECarFeeWindow();} }
            else if(btn.id == "btnRefresh" ){ oLECarSetGrid.dataSource.read(); }  
            else if(btn.id == "btnResetCurrentUserConfig"){ resetCurrentUserConfig(PageControl); }
        }

        //************* Call Back Functions **********************        
        var blnLECarSetGridChangeBound = false;
        function LECarSetGridDataBoundCallBack(e,tGrid){           
            oLECarSetGrid = tGrid;
            if (blnLECarSetGridChangeBound == false){
                oLECarSetGrid.bind("change", saveLECarPK);
                blnLECarSetGridChangeBound = true;
            }
        }
       
        function LECarSetGridGetStringData(s){ 
            s.LEAdminControl = pgLEControl;
            return '';
        }

        function execBeforecontactsInsert(e,fk,w){      
            //We need to populate the CarrierContCarrierControl in the new CarrierCont record so we get that from the header row
            var parentRow = $(e.currentTarget).closest("tr.k-detail-row").prev("tr"); // GET PARENT ROW ELEMENT    
            var parentGrid = parentRow.closest("[data-role=nglgrid]").data("kendoNGLGrid");            
            var parentModel = parentGrid.dataItem(parentRow); // GET THE PARENT ROW MODEL           
            var carrierControl = parentModel.CarrierControl; // ACCESS THE PARENT ROW MODEL ATTRIBUTES
            if (!carrierControl) { alert("No Carrier Control Field - cannot insert record"); return false;}
            return w.SetFieldDefault("CarrierContCarrierControl",carrierControl.toString());            
        }

        function savePostPageSettingSuccessCallback(results){
            //for now do nothing when we save the pk
        }
        function savePostPageSettingAjaxErrorCallback(xhr, textStatus, error){
            //for now do nothing when we save the pk
           
        }

        function DeleteLECarrierSuccessCallback(data) {
            var oResults = new nglEventParameters();
            oResults.source = "DeleteLECarrierSuccessCallback";
            oResults.widget = this;
            oResults.msg = 'Failed'; //set default to Failed     
            this.rData = null;
            var tObj = this;
            try {
                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                    if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                        oResults.error = new Error();
                        oResults.error.name = "Delete LE Carrier Failure";
                        oResults.error.message = data.Errors;
                        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                    }
                    else{
                        //this.rData = data.Data;
                        oResults.msg = 'Success';
                    }
                }
            } catch (err) {
                oResults.error = err
                ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
            }
            
            if (ngl.isFunction(refreshLECarSetGrid)) { refreshLECarSetGrid(); }
        }
        function DeleteLECarrierAjaxErrorCallback(xhr, textStatus, error) {
            var oResults = new nglEventParameters();
            var tObj = this;
            oResults.source = "DeleteLECarrierAjaxErrorCallback";
            oResults.widget = this;
            oResults.msg = 'Failed'; //set default to Failed  
            oResults.error = new Error();
            oResults.error.name = "Delete LE Carrier Failure"
            oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
            ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
            
            if (ngl.isFunction(refreshLECarSetGrid)) { refreshLECarSetGrid(); }
        }

        function PostLECarBillToCompSuccessCallback(data) {
            var oResults = new nglEventParameters();
            oResults.source = "DeleteLECarrierSuccessCallback";
            oResults.widget = this;
            oResults.msg = 'Failed'; //set default to Failed     
            this.rData = null;
            var tObj = this;
            try {
                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                    if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                        oResults.error = new Error();
                        oResults.error.name = "Save Carrier Billing Company Failure";
                        oResults.error.message = data.Errors;
                        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                    }
                    else{
                        oResults.msg = 'Success';
                        wndBillToComp.close();
                    }
                }
            } catch (err) {
                oResults.error = err
                ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
            }
            
            if (ngl.isFunction(refreshLECarSetGrid)) { refreshLECarSetGrid(); }
        }
        function PostLECarBillToCompAjaxErrorCallback(xhr, textStatus, error) {
            var oResults = new nglEventParameters();
            var tObj = this;
            oResults.source = "PostLECarBillToCompAjaxErrorCallback";
            oResults.widget = this;
            oResults.msg = 'Failed'; //set default to Failed  
            oResults.error = new Error();
            oResults.error.name = "Save Carrier Billing Company Failure"
            oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
            ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
            
            if (ngl.isFunction(refreshLECarSetGrid)) { refreshLECarSetGrid(); }
        }


        //************* Page Functions **********************
        function refreshLECarSetGrid() {
            if (typeof (oLECarSetGrid) !== 'undefined' && ngl.isObject(oLECarSetGrid)) { oLECarSetGrid.dataSource.read(); }
        }

        var GetLEAdminNotAsyncCB = function (data) {
            pgLEControl = data[0].LEAdminControl;
            pgLEName = data[0].LegalEntity;      
            $("#divTitleLE").html("<h2>" + pgLEName + "</h2>"); 
        }


        ////////////////////////////////////
        ////////CARRIER SETTINGS///////////
        function openCarSetAddWindow(){            
            $("#wndCarSet").data("kendoWindow").title("Add New Carrier"); //Change Title of Window     
            if (pgLEControl == 0) { return; } //Validate LE  
            //Set LE label
            var l = "<h3>Legal Entity: " + pgLEName + "</h3>";
            $("#lblLE").html(l);
            //Carrier ddl
            var dropdownlist = $("#ddlCarriers").data("kendoDropDownList");
            dropdownlist.readonly(false);
            dropdownlist.select(0);
            //DispatchType ddl
            var ddl = $("#ddlDispatchType").data("kendoDropDownList");
            ddl.select(function(dataItem) { return dataItem.Control === 1; });
            ddl.readonly(false);
            //Reset values
            $("#chkRateShopOnly").prop('checked', false);
            $("#chkAPIDispatching").prop('checked', false);
            $("#chkAPIStatusUpdates").prop('checked', false);
            $("#chkShowAuditFailReason").prop('checked', false);
            $("#chkShowPendingFeeFailReason").prop('checked', false);
            $("#txtBillToCompControl").val(0);
            $("#txtCarrierAccountRef").data("kendoMaskedTextBox").value("");
            //open window
            wndCarSet.center().open();
        }

        function openCarSetEditWindow(e) {
            var item = this.dataItem($(e.currentTarget).closest("tr")); //Get the item for the row         
            $("#wndCarSet").data("kendoWindow").title("Edit Carrier Settings"); //Change Title of Window 
            //Set LE label
            var l = "<h3>Legal Entity: " + pgLEName + "</h3>";
            $("#lblLE").html(l);
            //Carrier ddl
            var dropdownlist = $("#ddlCarriers").data("kendoDropDownList");
            dropdownlist.select(function(dataItem) { return dataItem.Control === item.CarrierControl; });
            dropdownlist.readonly();
            //DispatchType ddl
            var ddl = $("#ddlDispatchType").data("kendoDropDownList");
            ddl.select(function(dataItem) { return dataItem.Control === item.DispatchTypeControl; });
            ddl.readonly();
            //set values from item
            if (item.RateShopOnly) { $("#chkRateShopOnly").prop('checked', true); } else{ $("#chkRateShopOnly").prop('checked', false); }
            if (item.APIDispatching) { $("#chkAPIDispatching").prop('checked', true); } else{ $("#chkAPIDispatching").prop('checked', false); }         
            if (item.APIStatusUpdates) { $("#chkAPIStatusUpdates").prop('checked', true); } else{ $("#chkAPIStatusUpdates").prop('checked', false); }         
            if (item.ShowAuditFailReason) { $("#chkShowAuditFailReason").prop('checked', true); } else{ $("#chkShowAuditFailReason").prop('checked', false); }        
            if (item.ShowPendingFeeFailReason) { $("#chkShowPendingFeeFailReason").prop('checked', true); } else{ $("#chkShowPendingFeeFailReason").prop('checked', false); }        
            $("#txtBillToCompControl").val(item.BillToCompControl);
            $("#txtCarrierAccountRef").data("kendoMaskedTextBox").value(item.CarrierAccountRef);
            //open window
            wndCarSet.center().open();
        }

        function SaveCarrierSetting() {
            var otmp = $("#focusCancel").focus();
            var tfRateShopOnly = false;
            var tfAPIDispatching = false;
            var tfAPIStatusUpdates = false;
            var tfShowAuditFailReason = false;
            var tfShowPendingFeeFailReason = false;
            if ($('#chkRateShopOnly').is(":checked")) { tfRateShopOnly = true; }             
            if ($('#chkAPIDispatching').is(":checked")) { tfAPIDispatching = true; }  
            if ($('#chkAPIStatusUpdates').is(":checked")) { tfAPIStatusUpdates = true; }
            if ($('#chkShowAuditFailReason').is(":checked")) { tfShowAuditFailReason = true; }
            if ($('#chkShowPendingFeeFailReason').is(":checked")) { tfShowPendingFeeFailReason = true; }
            
            var item = new CarrierDispatchSettings();
            item.LEAdminControl = pgLEControl;
            var dataItemC = $("#ddlCarriers").data("kendoDropDownList").dataItem();
            item.CarrierControl = dataItemC.Control;
            var dataItemDT = $("#ddlDispatchType").data("kendoDropDownList").dataItem();
            item.DispatchTypeControl = dataItemDT.Control;
            item.RateShopOnly = tfRateShopOnly;
            item.APIDispatching = tfAPIDispatching;
            item.APIStatusUpdates = tfAPIStatusUpdates;
            item.ShowAuditFailReason = tfShowAuditFailReason;
            item.ShowPendingFeeFailReason = tfShowPendingFeeFailReason;
            item.BillToCompControl = $("#txtBillToCompControl").val();
            item.CarrierAccountRef = $("#txtCarrierAccountRef").data("kendoMaskedTextBox").value();
                
            $.ajax({
                async: true,
                type: "POST",
                url: "api/LegalEntityCarrier/SaveLegalEntityCarrier",
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
                            if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                blnErrorShown = true;
                                ngl.showErrMsg("Save Carrier Dispatch Settings Failure", data.Errors, null);
                            }
                            else {
                                if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                    if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                        blnSuccess = true;
                                        refreshLECarSetGrid();
                                        wndCarSet.close();
                                    }
                                }
                            }
                        }
                        if (blnSuccess === false && blnErrorShown === false) {
                            if (strValidationMsg.length < 1) { strValidationMsg = "Save Carrier Dispatch Settings Failure"; }
                            ngl.showErrMsg("Save Carrier Dispatch Settings Failure", strValidationMsg, null);
                        }
                    } catch (err) {
                        ngl.showErrMsg(err.name, err.description, null);
                    }
                },
                error: function (xhr, textStatus, error) {
                    var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure");
                    ngl.showErrMsg("Save Carrier Dispatch Settings Failure", sMsg, null);                        
                }
            });                      
        }

        function ConfirmDeleteLECarrier(iRet){          
            if (typeof (iRet) === 'undefined' || iRet === null  || iRet === 0 ) { return;}
           
            if (typeof (oLECarDeleteItem) !== 'undefined' && ngl.isObject(oLECarDeleteItem)) {
                var lecControl = 0;
                if ('LECarControl' in oLECarDeleteItem) { lecControl = oLECarDeleteItem.LECarControl; } 
                if (lecControl == 0) { ngl.showErrMsg("Delete Error", "Could not get LECarControl from the record", null); return; }

                var oCRUDCtrl = new nglRESTCRUDCtrl();
                var blnRet = oCRUDCtrl.delete("LegalEntityCarrier/DeleteLegalEntityCarrier", lecControl, tPage, "DeleteLECarrierSuccessCallback", "DeleteLECarrierAjaxErrorCallback", true);            
            }
        }
       
        var oLECarDeleteItem = null; 
        function deleteLECarrier(e){
            oLECarDeleteItem = null; //clear any old values
            oLECarDeleteItem = this.dataItem($(e.currentTarget).closest("tr")); 
            if (typeof (oLECarDeleteItem) !== 'undefined' && ngl.isObject(oLECarDeleteItem)) {
                ngl.OkCancelConfirmation(
                       "Delete Selected Carrier",
                       "This action cannot be undone. Are you sure you want to continue?",
                       400,
                       400,
                       null,
                       "ConfirmDeleteLECarrier");
            };      
        }


        ////////////////////////////////////
        //////////ACCESSORIALS/////////////
        function openAccessorialAddWindow(){
            //Reset the control value
            $("#txtLECAControl").val(0);
            $("#txtLECALECarControl").val(0);                
            $("#txtLECAUpdated").val("");
            //Set the title of the window
            $("#wndAccessorial").data("kendoWindow").title("Add New Accessorial");
            //Set the labels for the window
            var c = "<h3>Carrier: " + selectedRowCarrierName + "</h3>";
            $("#lblCarr").html(c);
            var l = "<h3>Legal Entity: " + pgLEName + "</h3>";
            $("#lblLEAcc").html(l);

            $("#txtLECALECarControl").val(iLECarPK);     

            var dropdownlist = $("#ddAccessorialCode").data("kendoDropDownList");
            dropdownlist.readonly(false);
            dropdownlist.select(0);

            $("#chkAutoApprove").prop('checked', false);
            $("#chkAllowCarrierUpdates").prop('checked', false);
            $("#chkAccessorialVisible").prop('checked', true);
            $("#txtCaption").data("kendoMaskedTextBox").value("");
            $("#txtEDICode").data("kendoMaskedTextBox").value("");
            
            $("#chkDynamicAverageValue").prop('checked', false);
            //Enable the AverageValue and set it to 0
            $("#txtAverageValue").data("kendoNumericTextBox").enable(true);
            $("#txtAverageValue").data("kendoNumericTextBox").value(0);
            //Disable Percent Tolerances and set them to 0
            $("#txtApproveTolerancePerLow").data("kendoNumericTextBox").enable(false);
            $("#txtApproveTolerancePerHigh").data("kendoNumericTextBox").enable(false);
            $("#txtApproveTolerancePerLow").data("kendoNumericTextBox").value(0);
            $("#txtApproveTolerancePerHigh").data("kendoNumericTextBox").value(0);
            //Enable Flat Rate Tolerances and set them to 0
            $("#txtApproveToleranceLow").data("kendoNumericTextBox").enable(true);
            $("#txtApproveToleranceHigh").data("kendoNumericTextBox").enable(true);
            $("#txtApproveToleranceLow").data("kendoNumericTextBox").value(0);
            $("#txtApproveToleranceHigh").data("kendoNumericTextBox").value(0);
                      
            wndAccessorial.center().open();
        }

        function openAccessorialEditWindow(e) {   
            //Reset the control value
            $("#txtLECAControl").val(0);
            $("#txtLECALECarControl").val(0);
            $("#txtLECAUpdated").val("");
            var item = this.dataItem($(e.currentTarget).closest("tr")); 
            //set the title of the window
            $("#wndAccessorial").data("kendoWindow").title("Edit Accessorial");
            //set the lables for the window
            var c = "<h3>Carrier: " + selectedRowCarrierName + "</h3>";
            $("#lblCarr").html(c);
            var l = "<h3>Legal Entity: " + pgLEName + "</h3>";
            $("#lblLEAcc").html(l);

            var dropdownlist = $("#ddAccessorialCode").data("kendoDropDownList");
            dropdownlist.select(function(dataItem) { return dataItem.AccessorialCode === item.AccessorialCode; });
            dropdownlist.readonly();

            if (item.AutoApprove) { $("#chkAutoApprove").prop('checked', true); } else{ $("#chkAutoApprove").prop('checked', false); }
            if (item.AllowCarrierUpdates) { $("#chkAllowCarrierUpdates").prop('checked', true); } else{ $("#chkAllowCarrierUpdates").prop('checked', false); }
            if (item.AccessorialVisible) { $("#chkAccessorialVisible").prop('checked', true); } else{ $("#chkAccessorialVisible").prop('checked', false); }
            $("#txtCaption").data("kendoMaskedTextBox").value(item.Caption);
            $("#txtEDICode").data("kendoMaskedTextBox").value(item.EDICode); 
            //save the control so it can be accessed by the save function
            $("#txtLECAControl").val(item.LECAControl);
            $("#txtLECALECarControl").val(item.LECALECarControl);
            $("#txtLECAUpdated").val(item.LECAUpdated); 

            if (item.DynamicAverageValue) { 
                $("#chkDynamicAverageValue").prop('checked', true); 
                //Disable AverageValue and set it to 0
                $("#txtAverageValue").data("kendoNumericTextBox").enable(false);
                $("#txtAverageValue").data("kendoNumericTextBox").value(0);
                //Disable Flat Rate Tolerances and set them to 0
                $("#txtApproveToleranceLow").data("kendoNumericTextBox").enable(false);
                $("#txtApproveToleranceHigh").data("kendoNumericTextBox").enable(false); 
                $("#txtApproveToleranceLow").data("kendoNumericTextBox").value(0);
                $("#txtApproveToleranceHigh").data("kendoNumericTextBox").value(0);                          
                //Enable Percent Tolerances and set them to the database value
                $("#txtApproveTolerancePerLow").data("kendoNumericTextBox").enable(true);
                $("#txtApproveTolerancePerHigh").data("kendoNumericTextBox").enable(true);
                $("#txtApproveTolerancePerLow").data("kendoNumericTextBox").value(item.ApproveTolerancePerLow);
                $("#txtApproveTolerancePerHigh").data("kendoNumericTextBox").value(item.ApproveTolerancePerHigh);
            } 
            else{ 
                $("#chkDynamicAverageValue").prop('checked', false);
                $("#txtAverageValue").data("kendoNumericTextBox").value(item.AverageValue);
                if(item.AverageValue > 0){
                    //Enable Percent Tolerances and set them to the database value
                    $("#txtApproveTolerancePerLow").data("kendoNumericTextBox").enable(true);
                    $("#txtApproveTolerancePerHigh").data("kendoNumericTextBox").enable(true);
                    $("#txtApproveTolerancePerLow").data("kendoNumericTextBox").value(item.ApproveTolerancePerLow);
                    $("#txtApproveTolerancePerHigh").data("kendoNumericTextBox").value(item.ApproveTolerancePerHigh);
                    //Disable Flat Rate Tolerances and set them to 0
                    $("#txtApproveToleranceLow").data("kendoNumericTextBox").enable(false);
                    $("#txtApproveToleranceHigh").data("kendoNumericTextBox").enable(false);
                    $("#txtApproveToleranceLow").data("kendoNumericTextBox").value(0);
                    $("#txtApproveToleranceHigh").data("kendoNumericTextBox").value(0);
                }
                if(item.AverageValue === 0){
                    //Disable Percent Tolerances and set them to 0
                    $("#txtApproveTolerancePerLow").data("kendoNumericTextBox").enable(false);
                    $("#txtApproveTolerancePerHigh").data("kendoNumericTextBox").enable(false);
                    $("#txtApproveTolerancePerLow").data("kendoNumericTextBox").value(0);
                    $("#txtApproveTolerancePerHigh").data("kendoNumericTextBox").value(0);
                    //Enable Flat Rate Tolerances and set them to the database value
                    $("#txtApproveToleranceLow").data("kendoNumericTextBox").enable(true);
                    $("#txtApproveToleranceHigh").data("kendoNumericTextBox").enable(true);
                    $("#txtApproveToleranceLow").data("kendoNumericTextBox").value(item.ApproveToleranceLow);
                    $("#txtApproveToleranceHigh").data("kendoNumericTextBox").value(item.ApproveToleranceHigh);                    
                }
            }                  
            wndAccessorial.center().open();
        }

        function SaveAccessorial() {
            var otmp = $("#focusCancelAcc").focus();

            var tfAutoApprove = false;
            var tfAllowCarrierUpdates = false;
            var tfAccessorialVisible = false;
            var tfDynamicAverageValue = false;            
            if ($('#chkAutoApprove').is(":checked")) { tfAutoApprove = true; }               
            if ($('#chkAllowCarrierUpdates').is(":checked")) { tfAllowCarrierUpdates = true; }  
            if ($('#chkAccessorialVisible').is(":checked")) { tfAccessorialVisible = true; }
            if ($('#chkDynamicAverageValue').is(":checked")) { tfDynamicAverageValue = true; }

            var item = new LECarrierAccessorial();
            item.LECAControl = $("#txtLECAControl").val();
            item.LECALECarControl = $("#txtLECALECarControl").val();

            var dataItem = $("#ddAccessorialCode").data("kendoDropDownList").dataItem();
            item.LECAAccessorialCode = dataItem.AccessorialCode;

            item.LECACaption = $("#txtCaption").data("kendoMaskedTextBox").value();
            item.LECAEDICode = $("#txtEDICode").data("kendoMaskedTextBox").value();
            item.LECAAutoApprove = tfAutoApprove;
            item.LECAAllowCarrierUpdates = tfAllowCarrierUpdates;
            item.LECAAccessorialVisible = tfAccessorialVisible;
            item.LECADynamicAverageValue = tfDynamicAverageValue;
            item.LECAAverageValue = $("#txtAverageValue").data("kendoNumericTextBox").value();
            item.LECAApproveToleranceLow = $("#txtApproveToleranceLow").data("kendoNumericTextBox").value(); 
            item.LECAApproveToleranceHigh = $("#txtApproveToleranceHigh").data("kendoNumericTextBox").value();
            item.LECAApproveTolerancePerLow = $("#txtApproveTolerancePerLow").data("kendoNumericTextBox").value();
            item.LECAApproveTolerancePerHigh = $("#txtApproveTolerancePerHigh").data("kendoNumericTextBox").value();
            item.LECAUpdated = $("#txtLECAUpdated").val();
                           
            $.ajax({
                type: "POST",
                url: "api/LECarrierAccessorial/Post",
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
                            if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                blnErrorShown = true;
                                ngl.showErrMsg("Save LE Carrier Accessorial Failure", data.Errors, null);
                            }
                            else {
                                if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                    if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                        blnSuccess = true;
                                        ngl.showSuccessMsg("Save Accessorial Success!",null);
                                        refreshLECarSetGrid();
                                        wndAccessorial.close(); 
                                    }
                                }
                            }
                        }
                        if (blnSuccess === false && blnErrorShown === false) {
                            if (strValidationMsg.length < 1) { strValidationMsg = "Save LE Carrier Accessorial Failure"; }
                            ngl.showErrMsg("Save LE Carrier Accessorial Failure", strValidationMsg, null);
                        }
                    } catch (err) {
                        ngl.showErrMsg(err.name, err.description, null);
                    }
                },
                error: function (xhr, textStatus, error) {
                    var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure");
                    ngl.showErrMsg("Save LE Carrier Accessorial Failure", sMsg, null);                        
                }
            });        
        }

        function deleteLECA(e){ //REPLACE
            var item = this.dataItem($(e.currentTarget).closest("tr"));
            var lecaControl = 0;

            if (typeof (item) !== 'undefined' && item != null) { 
                if ('LECAControl' in item) { lecaControl = item.LECAControl; } //REPLACE
            } 
            if (lecaControl == 0) { ngl.showErrMsg("Delete Error", "Could not get LECAControl from the record", null); return; }

            $.ajax({
                async: false,
                url: "api/LECarrierAccessorial/DeleteLECarrierAccessorials/" + lecaControl,
                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                type: "DELETE",
                success: function (data) {     
                    try {
                        var blnSuccess = false;
                        var blnErrorShown = false;
                        var strValidationMsg = "";
                        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                            if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                blnErrorShown = true;
                                ngl.showErrMsg("Delete LE Carrier Accessorial Failure", data.Errors, null);
                            }
                        }
                    } catch (err) {
                        ngl.showErrMsg(err.name, err.description, null);
                    }
                },
                error: function (xhr, textStatus, error) {
                    var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Delete Data Failure");
                    ngl.showErrMsg("Delete LE Carrier Accessorial Failure", sMsg, null);                        
                }
            });

            refreshLECarSetGrid();
        }

        $('#chkDynamicAverageValue').click(function(){
            if($(this).is(':checked')){
                //Disable AverageValue and set it to 0
                $("#txtAverageValue").data("kendoNumericTextBox").enable(false);
                $("#txtAverageValue").data("kendoNumericTextBox").value(0);
                //Disable Flat Rate Tolerances and set them to 0
                $("#txtApproveToleranceLow").data("kendoNumericTextBox").enable(false);
                $("#txtApproveToleranceHigh").data("kendoNumericTextBox").enable(false);
                $("#txtApproveToleranceLow").data("kendoNumericTextBox").value(0);
                $("#txtApproveToleranceHigh").data("kendoNumericTextBox").value(0);                             
                //Enable Percent Tolerances
                $("#txtApproveTolerancePerLow").data("kendoNumericTextBox").enable(true);
                $("#txtApproveTolerancePerHigh").data("kendoNumericTextBox").enable(true);
            } else {
                $("#txtAverageValue").data("kendoNumericTextBox").enable(true);
                
                //Enable Percent Tolerances
                $("#txtApproveTolerancePerLow").data("kendoNumericTextBox").enable(true);
                $("#txtApproveTolerancePerHigh").data("kendoNumericTextBox").enable(true);
                //Disable Flat Rate Tolerances and set them to the database value
                $("#txtApproveToleranceLow").data("kendoNumericTextBox").enable(false);
                $("#txtApproveToleranceHigh").data("kendoNumericTextBox").enable(false);
                //$("#txtApproveToleranceLow").data("kendoNumericTextBox").value(item.ApproveToleranceLow);
                //$("#txtApproveToleranceHigh").data("kendoNumericTextBox").value(item.ApproveToleranceHigh);
            }
        });
       

        ////////////wndBillToComp///////////////////
        function refreshBillToCompsDropDown() { $('#ddlBillToComps').data('kendoDropDownList').dataSource.read(); }

        function openEditBillToCompWindow(){           
            //refresh the ddl
            refreshBillToCompsDropDown();
            //set the Carrier label         
            var c = "<h3>Carrier: " + selectedRowCarrierName + "</h3>";
            $("#lblCarrierName").html(c);
            //set the ddl
            var dropdownlist = $("#ddlBillToComps").data("kendoDropDownList");
            dropdownlist.select(function(dataItem) { return dataItem.Control === leCarSetGridSelectedRowDataItem.BillToCompControl; });
            //open window           
            wndBillToComp.center().open();
        }

        function btnSetBillToComp_Click(){
            var compControl = $("#ddlBillToComps").data("kendoDropDownList").value();
                    
            var gr = new GenericResult();
            gr.Control = iLECarPK;
            gr.intField1 = compControl;

            var oCRUDCtrl = new nglRESTCRUDCtrl();
            var blnRet = oCRUDCtrl.update("LegalEntityCarrier/SetLECarrierBillToComp", gr, tPage, "PostLECarBillToCompSuccessCallback", "PostLECarBillToCompAjaxErrorCallback", true);             
        }

        ///////////COPY CARRIER FEE CONFIG///////////// 
        function openCopyLECarFeeWindow() {
            $("#txtCopyFromLECarControl").val(iLECarPK);
            $("#lblCopyFromLECarName").html(selectedRowCarrierName);
            $("#copyToGrid").data("kendoGrid").clearSelection();
            $("#copyToGrid").data("kendoGrid").dataSource.read();
            wndCopyLECarConfig.center().open();
        }

        function btnCopyOk_Click(){
            var msgPrompt = "This action will overwrite the existing Accessorial configuration for the selected Carriers. Are you sure that you want to proceed?";                
            kendo.confirm(msgPrompt).then(function () {
                //kendo.alert("You chose the Ok action.");
                var copyToLECars = $("#copyToGrid").data("kendoGrid").selectedKeyNames();
                if(typeof (copyToLECars) !== 'undefined' && ngl.isArray(copyToLECars) && copyToLECars.length > 0){
                    var s = new GenericResult();  
                    s.intArray = copyToLECars;
                    s.intField1 = $("#txtCopyFromLECarControl").val();
                    
                    $.ajax({
                        type: 'POST',
                        url: 'api/LECarrierAccessorial/CopyLECarrierAccessorialConfig',
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        data: JSON.stringify(s),
                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                        success: function (data) { wndCopyLECarConfig.close(); refreshLECarSetGrid(); },
                        //error: function (xhr, textStatus, error) { tObj[errorCallBack](xhr, textStatus, error); }
                    });

                }
            }, function () {
                //kendo.alert("You chose to Cancel action.");
                wndCopyLECarConfig.close();
                refreshLECarSetGrid();
            });
           
        }

        ///////////COPY LEGAL ENTITY CONFIG/////////////       
        function refreshLEToCopyDropDown() { $('#ddlLEToCopy').data('kendoDropDownList').dataSource.read(); }

        function openCopyLEConfigWnd(){
            if (pgLEControl == 0) {  
                ngl.showErrMsg("Legal Entity Required", "", null);
                wndCopyLEConfig.close();
                return; 
            }                    
            var h = "<h2>" + pgLEName + "</h2>";
            $("#divCopyToLE").html(h);
            refreshLEToCopyDropDown();
            wndCopyLEConfig.center().open();
        }

        function btnCopyLEOk_Click(){
            if($("#ddlLEToCopy").data("kendoDropDownList").value() == pgLEControl){
                ngl.showErrMsg("Cannot Copy Own Config", "Cannot Copy Own Config", null);
                return; 
            }           
            var msgPrompt = "This action will overwrite any existing configurations for " + pgLEName + ". Are you sure that you want to proceed?";               
            kendo.confirm(msgPrompt).then(function () {                    
                //kendo.alert("You chose the Ok action.");
                execCopyLEConfig();               
            }, function () {                    
                //kendo.alert("You chose to Cancel action.");
                wndCopyLEConfig.close();
                refreshLECarSetGrid();                
            });          
        }

        function execCopyLEConfig() {
            var CopyToLE = pgLEControl;
            var CopyFromLE = $("#ddlLEToCopy").data("kendoDropDownList").value();
            var name = $("#ddlLEToCopy").data("kendoDropDownList").text();
                        
            var gr = new GenericResult();
            gr.intField1 = CopyToLE;
            gr.intField2 = CopyFromLE;
            
            $.ajax({
                async: false,
                type: "POST",
                url: "api/LegalEntity/CopyLECAMConfig",
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                data: JSON.stringify(gr),
                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                success: function (data) {     
                    try {
                        var blnSuccess = false;
                        var blnErrorShown = false;
                        var strValidationMsg = "";
                        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                            if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                blnErrorShown = true;
                                ngl.showErrMsg("Copy Legal Entity CAM Config Failure", data.Errors, null);
                            }
                            else {
                                if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                    if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                        blnSuccess = true;
                                        refreshLECarSetGrid();
                                    }
                                }
                            }
                        }
                        if (blnSuccess === false && blnErrorShown === false) {
                            if (strValidationMsg.length < 1) { strValidationMsg = "Copy Legal Entity CAM Config Failure"; }
                            ngl.showErrMsg("Copy Legal Entity CAM Config Failure", strValidationMsg, null);
                        }
                    } catch (err) {
                        ngl.showErrMsg(err.name, err.description, null);
                    }
                },
                error: function (xhr, textStatus, error) {
                    var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure");
                    ngl.showErrMsg("Copy Legal Entity CAM Config Failure", sMsg, null);                        
                }
            });           
            wndCopyLEConfig.close();
        }


        $(function () {
            //wire focus of all numerictextbox widgets on the page
            $("input[type=number]").bind("focus", function () {
                var input = $(this);
                clearTimeout(input.data("selectTimeId")); //stop started time out if any

                var selectTimeId = setTimeout(function()  {
                    input.select();
                });

                input.data("selectTimeId", selectTimeId);
            }).blur(function(e) {
                clearTimeout($(this).data("selectTimeId")); //stop started timeout
            });
        })

        $(document).ready(function () {  
            var PageMenuTab = <%=PageMenuTab%>;
          
            //NOTE: This has to be done before PageReadyJS because the grid is dependant on the LEControl value                
            //Get the LE for the user
            getLEAdminNotAsync(0, GetLEAdminNotAsyncCB);

            var kmask = new kendoMasks();
            kmask.loadDefaultMasks();     
            var phoneMask = kmask.getMask("phone_number");
            
            if (control != 0){
                setTimeout(function () {  
                
                
                    ////////////ChangeLEDialogCtrl///////////////////               
                    oChangeLEDialogCtrl = new ChangeLEDialogCtrl();                
                    oChangeLEDialogCtrl.loadDefaults(wndChangeLEDialog, oChangeLEDialogSaveCB);

                     
                    //Kendo Definitions
                    $("#txtCarrierAccountRef").kendoMaskedTextBox();
                    $("#txtCaption").kendoMaskedTextBox();
                    $("#txtEDICode").kendoMaskedTextBox();
                    $("#txtApproveToleranceLow").kendoNumericTextBox({ format: "{0:c2}" });
                    $("#txtApproveToleranceHigh").kendoNumericTextBox({ format: "{0:c2}" });
                    $("#txtApproveTolerancePerLow").kendoNumericTextBox({
                        format: "p0",
                        factor: 100,
                        min: 0,
                        max: 1,
                        step: 0.01
                    });
                    $("#txtApproveTolerancePerHigh").kendoNumericTextBox({
                        format: "p0",
                        factor: 100,
                        min: 0,
                        max: 1,
                        step: 0.01
                    });                 
                    $("#txtAverageValue").kendoNumericTextBox({
                        change: function() {
                            var value = this.value();
                            if(value > 0){
                                $("#txtApproveTolerancePerLow").data("kendoNumericTextBox").enable(true);
                                $("#txtApproveTolerancePerHigh").data("kendoNumericTextBox").enable(true);
                                $("#txtApproveToleranceLow").data("kendoNumericTextBox").enable(false);
                                $("#txtApproveToleranceHigh").data("kendoNumericTextBox").enable(false);

                                $("#txtApproveToleranceLow").data("kendoNumericTextBox").value(0);
                                $("#txtApproveToleranceHigh").data("kendoNumericTextBox").value(0);
                            }
                            if(value === 0){
                                $("#txtApproveTolerancePerLow").data("kendoNumericTextBox").enable(false);
                                $("#txtApproveTolerancePerHigh").data("kendoNumericTextBox").enable(false);
                                $("#txtApproveToleranceLow").data("kendoNumericTextBox").enable(true);
                                $("#txtApproveToleranceHigh").data("kendoNumericTextBox").enable(true);

                                $("#txtApproveTolerancePerLow").data("kendoNumericTextBox").value(0);
                                $("#txtApproveTolerancePerHigh").data("kendoNumericTextBox").value(0);
                            }
                        }
                    });

                
                    ///////////CARRIERS DDL/////////////
                    $("#ddlCarriers").kendoDropDownList({
                        dataTextField: "Name",
                        dataValueField: "Control",
                        autoWidth: true,
                        filter: "contains",
                        dataSource: {
                            serverFiltering: false,
                            transport: {
                                read: {
                                    url: "api/vLookupList/GetUserDynamicList/13",
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
                            error: function (xhr, textStatus, error) { ngl.showErrMsg("Get Carriers JSON Response Error",formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"), null);  this.cancelChanges(); }
                        }
                    });

                    /////////DISPATCH TYPE DDL//////////
                    $("#ddlDispatchType").kendoDropDownList({
                        dataTextField: "Name",
                        dataValueField: "Control",
                        autoWidth: true,
                        filter: "contains",
                        dataSource: {
                            serverFiltering: false,
                            transport: {
                                read: {
                                    url: "api/vLookupList/GetStaticList/62",
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
                            error: function (xhr, textStatus, error) { ngl.showErrMsg("Get DispatchType JSON Response Error",formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"), null);  this.cancelChanges(); }
                        }
                    });
                  
                    //////////ACCESSORIALS DDL//////////
                    dsAccessorials = new kendo.data.DataSource({
                        transport: {
                            read: {
                                url: "api/Accessorials/GetAccessorials",
                                dataType: 'json',
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                type: "GET"
                            },
                            parameterMap: function (options, operation) { return options; }
                        },
                        schema: {
                            data: "Data",
                            total: "Count",
                            model: {
                                id: "AccessorialCode",
                                fields: {
                                    AccessorialCode: { type: "number" },
                                    AccessorialName: { type: "string" },
                                    AccessorialAutoApprove: { type: "bool" },
                                    AccessorialAllowCarrierUpdates: { type: "bool" },
                                    AccessorialVisible: { type: "bool" },                               
                                    AccessorialCaption: { type: "string" },
                                    AccessorialEDICode: { type: "string" }
                                }
                            },
                            errors: "Errors"
                        },
                        error: function (xhr, textStatus, error) { ngl.showErrMsg("Get Accessorials JSON Response Error",formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"), null);  this.cancelChanges(); },
                        serverPaging: false,
                        serverSorting: false,
                        serverFiltering: false
                    });

                    $("#ddAccessorialCode").kendoDropDownList({
                        dataTextField: "AccessorialName",
                        dataValueField: "AccessorialCode",
                        dataSource: dsAccessorials,
                        select: function(e) {
                            var item = e.dataItem;
                            if (typeof (item) !== 'undefined' && item != null && ngl.isObject(item)) { 
                                if ('AccessorialAutoApprove' in item) { 
                                    if (item.AccessorialAutoApprove) { $("#chkAutoApprove").prop('checked', true); } else{ $("#chkAutoApprove").prop('checked', false); }
                                }
                                if ('AccessorialAllowCarrierUpdates' in item) { 
                                    if (item.AccessorialAllowCarrierUpdates) { $("#chkAllowCarrierUpdates").prop('checked', true); } else{ $("#chkAllowCarrierUpdates").prop('checked', false); }                           
                                }
                                if ('AccessorialVisible' in item) { 
                                    if (item.AccessorialVisible) { $("#chkAccessorialVisible").prop('checked', true); } else{ $("#chkAccessorialVisible").prop('checked', false); }                           
                                }
                                if ('AccessorialCaption' in item) { 
                                    $("#txtCaption").val(item.AccessorialCaption);
                                }
                                if ('AccessorialEDICode' in item) { 
                                    $("#txtEDICode").val(item.AccessorialEDICode);
                                }
                            }                 
                        }
                    });

                    ///////BILL TO COMP BY LE DDL/////////
                    dsBillToComps = new kendo.data.DataSource({
                        transport: {
                            read: {
                                url: function(options) {
                                    return "api/vLookupList/GetUserDynamicList/21"; //CompActive = 21
                                },
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }
                            },
                            parameterMap: function (options, operation) { return options; }
                        },
                        schema: {
                            data: "Data",
                            total: "Count",
                            model: {
                                id: "Control",
                                fields: {
                                    Name: { editable: false },
                                    Control: { editable: false }
                                }
                            },
                            errors: "Errors"
                        },
                        error: function (xhr, textStatus, error) { ngl.showErrMsg("GetBillToComp JSON Response Error",formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"), null);  this.cancelChanges(); },
                        serverPaging: false,
                        serverSorting: false,
                        serverFiltering: false
                    });
                
                    $("#ddlBillToComps").kendoDropDownList({
                        autoBind: true,
                        dataTextField: "Name",
                        dataValueField: "Control",
                        dataSource: dsBillToComps,
                        optionLabel: {
                            Name: "N/A",
                            Control: 0
                        }
                    });

                    ///////LEGAL ENTITY TO COPY DDL/////////
                    dsLEToCopy = new kendo.data.DataSource({
                        transport: {
                            read: {
                                url: function(options) {
                                    return "api/LegalEntity/GetLEWithCAMConfigsToCopy/";
                                },
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }
                            },
                            parameterMap: function (options, operation) { return options; }
                        },
                        schema: {
                            data: "Data",
                            total: "Count",
                            model: {
                                id: "Control",
                                fields: {
                                    Control: { editable: false },
                                    Name: { editable: false },                
                                    Description: { editable: false }
                                }
                            },
                            errors: "Errors"
                        },
                        error: function (xhr, textStatus, error) { ngl.showErrMsg("GetLEWithCAMConfigsToCopy JSON Response Error",formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"), null);  this.cancelChanges(); },
                        serverPaging: false,
                        serverSorting: false,
                        serverFiltering: false
                    });
                
                    $("#ddlLEToCopy").kendoDropDownList({
                        autoBind: true,
                        dataTextField: "Name",
                        dataValueField: "Control",
                        dataSource: dsLEToCopy
                    });


                    ////////wndAccessorial//////////////
                    wndAccessorial = $("#wndAccessorial").kendoWindow({
                        title: "Edit/Add",
                        height: 500,
                        width: 500,
                        minWidth: 500,
                        modal: true,
                        visible: false,
                        actions: ["save", "Minimize", "Maximize", "Close"],
                    }).data("kendoWindow");                       
                    //Modified by RHR for Kendo v-2018 xxx must reference .parent() on save button click.
                    $("#wndAccessorial").data("kendoWindow").wrapper.find(".k-svg-i-save").parent().click(function (e) { SaveAccessorial(); }); 

                    //////////wndCarSet/////////////////
                    wndCarSet = $("#wndCarSet").kendoWindow({
                        title: "Edit/Add",
                        height: 485,
                        width: 475,
                        modal: true,
                        visible: false,
                        actions: ["save", "Minimize", "Maximize", "Close"],
                    }).data("kendoWindow");
                    //Modified by RHR for Kendo v-2018 xxx must reference .parent() on save button click.
                    $("#wndCarSet").data("kendoWindow").wrapper.find(".k-svg-i-save").parent().click(function (e) { SaveCarrierSetting(); }); 

                    ////////wndBillToComp//////////////
                    wndBillToComp = $("#wndBillToComp").kendoWindow({
                        title: "Set Bill To Company",
                        modal: true,
                        visible: false,
                    }).data("kendoWindow");


                    $("#copyToGrid").kendoGrid({
                        dataSource: {
                            pageSize: 10,
                            transport: {                            
                                read: function(options) { 
                                    var s = new AllFilter();                                    
                                    s.page = options.data.page;
                                    s.skip = options.data.skip;
                                    s.take = options.data.take;
                                    if (typeof (LECarSetGridGetStringData) !== 'undefined' && ngl.isFunction(LECarSetGridGetStringData)) {s.Data = LECarSetGridGetStringData(s); } else { s.Data = '';}; 
                                    $.ajax({ 
                                        url: 'api/LegalEntityCarrier/GetRecords/', 
                                        contentType: 'application/json; charset=utf-8', 
                                        dataType: 'json', 
                                        data: { filter: JSON.stringify(s) }, 
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                                        success: function(data) { 
                                            options.success(data); 
                                            if (typeof (data) !== 'undefined' && ngl.isObject(data) && typeof (data.Errors) !== 'undefined' &&  data.Errors != null) { ngl.showErrMsg('Access Denied', data.Errors, null); }
                                        }, 
                                        error: function(result) { options.error(result); } 
                                    }); 
                                },
                            },
                            schema: {
                                data: "Data",
                                total: "Count",
                                model: {
                                    id: "LECarControl"
                                }
                            }
                        },
                        pageable: true,
                        scrollable: false,
                        persistSelection: true,
                        sortable: true,
                        columns: [
                            { selectable: true, width: 50 },
                            { field:"CarrierName", title: "Name" },
                            { field: "DispatchTypeName", title:"Dispatch Type", hidden: false }
                        ]
                    });


                    /////////wndCopyLECarConfig//////////////
                    wndCopyLECarConfig = $("#wndCopyLECarConfig").kendoWindow({
                        title: "Copy Carrier Accessorial Config",
                        modal: true,
                        visible: false              
                    }).data("kendoWindow");

                    /////////wndCopyLEConfig//////////////
                    wndCopyLEConfig = $("#wndCopyLEConfig").kendoWindow({
                        title: "Copy Legal Entity Config",
                        modal: true,
                        visible: false              
                    }).data("kendoWindow");


                }, 10,this);  
            }

             var PageReadyJS = <%=PageReadyJS%>; 
            //setTimeout(function () {        
                menuTreeHighlightPage(); //must be called after PageReadyJS
                var divWait = $("#h1Wait");
                if  (typeof (divWait) !== 'undefined' ) {
                    divWait.hide();
                }
            //}, 10, this);

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
                
        .k-grid tbody .k-grid-Edit { min-width: 0; }

      
        .k-grid tbody .k-grid-Edit .k-icon { margin: 0; }
    </style> 
    </div>
    </body>
</html>
