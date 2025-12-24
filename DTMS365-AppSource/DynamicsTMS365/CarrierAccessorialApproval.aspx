<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CarrierAccessorialApproval.aspx.cs" Inherits="DynamicsTMS365.CarrierAccessorialApproval" %>

<!DOCTYPE html>

<html>
    <head >
        <title>DTMS Carrier Accessorial Approval</title>
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
                            <div class="pane-content">

                                <% Response.Write(PageErrorsOrWarnings); %>
                                <!-- begin Page Content -->
                                <% Response.Write(FastTabsHTML); %>   
                                <!-- End Page Content -->

                                <div class="fast-tab" >
                                    <span id="ExpandFilters" style="display: none;"><a onclick='expandFilters();'><span style="font-size: small;font-weight:bold;" class='k-icon k-i-chevron-down'></span></a></span>
                                    <span id="CollapseFilters" style="display: normal;"><a onclick='collapseFilters();'><span style="font-size: small;font-weight:bold;" class='k-icon k-i-chevron-up'></span></a></span>
                                    <span style="font-size:small; font-weight:bold">Filters</span>&nbsp;&nbsp;<br/>
                                    <div id="divFilters" style="padding: 10px;">
                                        <span>
                                            <label for="ddlCarriers">Carrier:</label>
                                            <input id="ddlCarriers"/>
                                            <label for="ddlFilters">Filter by:</label>
                                            <input id="ddlFilters"/>
                                            <input id="txtFilterVal"/>
                                            <span id="spfilterDates"><label for="txtFilterVal">From:</label><input id="dpFilterFrom"/><label for="dpFilterTo">To:</label><input id="dpFilterTo"/></span>
                                            <span id="spfilterButtons"><a id="btnFilter"></a><a id="btnClearFilter"></a></span>
                                        </span>
                                        <input id="txtSortDirection" type="hidden" /> 
                                        <input id="txtSortField" type="hidden" />
                                        <input id="txtCarrierControlFrom" type="hidden" />                                         
                                    </div>
                                </div>      
                                 <div id="BookFeesPendingGrid"></div>  
                                           
                                <input id="txtCarrierControl" type="hidden" />  
                                <input id="txtLEControl" type="hidden" />                                                  
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

          <div id="wnd">
              <div>
                  <table class="tblResponsive">
                      <tr><td class="tblResponsive-top">Amount</td></tr>
                      <tr><td class="tblResponsive-top"><input id="txtValue" type="number" /></td></tr>
                  </table>
                  <input id="txtBFPControl" type="hidden" />
              </div>
          </div> 

          <div id="wndUnlockCosts">
              <div>
                  <p>Cost are locked on this load.Fees cannot be approved until costs are unlocked. Click continue to unlock all costs on this shipment now. You may also unlock the BFC costs if you prefer.</p>
              </div>
              <div id="divChkBFC" style="margin-left: 5px;">
                    <input type="checkbox" id="chkUnlockBFC" ><label class="k-checkbox-label" for="chkUnlockBFC"><strong>Unlock BFC</strong></label>
                </div>
              <div style="margin-top:5px; text-align:center;">
                  <button class="k-button k-button-solid-base k-button-solid k-button-md k-rounded-md" type="button" id="btnUnlockOk" onclick="btnUnlockOk_Click();">Continue</button>
              </div>
              <input id="txtFeeControl" type="hidden" />
              <input id="txtBookControl" type="hidden" />
          </div>

<%--<table class='tbl'><tr><th class='tbl-topRt'>Line Haul:</th><th class='tbl-topRt'>{1}</th></tr><tr><td class='tbl-topRt'>Total Accessorials:</td><td class='tbl-topRt'>{2}</td></tr><tr><td class='tbl-topRt'>Total Cost:</td><td class='tbl-topRt'>{3}</td></tr></table>--%>
    

    <% Response.WriteFile("~/Views/AccessorialSettingsWindow44.html"); %>
    <% Response.Write(PageTemplates); %>  
    <%-- HelpWindow needs to go after AuthNotification because it uses the notification object created there --%>
    <% Response.Write(AuthLoginNotificationHTML); %>   
    <% Response.WriteFile("~/Views/HelpWindow.html"); %>        
    <script>
        <% Response.Write(ADALPropertiesjs); %>

        var PageControl = '<%=PageControl%>'; 
        var tPage = this;
        var oKendoGrid = null;
        var tObj = this;

        <% Response.Write(NGLOAuth2); %>


        <% Response.Write(PageCustomJS); %>
        
        var wnd = kendo.ui.Window;
        var wndUnlockCosts = kendo.ui.Window;
        var BFPItems = kendo.data.DataSource;     
        var wndAccessorial = kendo.ui.Window;

        //*************  Call Back Function **********************
        function BookFeesPendingGridDataBoundCallBack(e,tGrid){
            var columns = e.sender.columns;
            var columnIndex = tGrid.wrapper.find(".k-grid-header [data-field=" + "DoesCLAXConfigExist" + "]").index();
            oKendoGrid = tGrid;
            var ds = tGrid.dataSource.data();
            for (var j=0; j < ds.length; j++) {
                if (typeof (ds[j].DoesCLAXConfigExist) !== 'undefined' && ds[j].DoesCLAXConfigExist != null) {
                    if (ds[j].DoesCLAXConfigExist !== false) {
                        var item = tGrid.dataSource.get(ds[j].BFPControl); //Get by ID or any other preferred method                           
                        //tGrid.tbody.find("tr[data-uid='"+item.uid+"'] .k-grid-ConfigureFee").hide();                            
                        tGrid.tbody.find("tr[data-uid='"+item.uid+"'] .k-grid-ConfigureFee").prop('disabled', true); //Disable the ConfigureFee button
                        tGrid.tbody.find("tr[data-uid='"+item.uid+"'] .k-grid-ConfigureFee").addClass("k-state-disabled"); //Add the diabled class to the css (change button color)
                    }
                }                
            }
        }

        
        function expandFilters() { $("#divFilters").show(); $("#ExpandFilters").hide(); $("#CollapseFilters").show(); }
        function collapseFilters() { $("#divFilters").hide(); $("#ExpandFilters").show(); $("#CollapseFilters").hide(); }

        function execActionClick(btn, proc){
            if(btn.id == "btnRefresh" ){ refresh(); }  
            else if(btn.id == "btnResetCurrentUserConfig"){ resetCurrentUserConfig(PageControl); }
        }

        function refresh(){
            ngl.readDataSource(oKendoGrid);
        }

        function refreshBookFeesPendingGrid() {
            $('#BookFeesPendingGrid').data('kendoGrid').dataSource.read();
        }

        function openEditWindow(e) {
            var item = this.dataItem($(e.currentTarget).closest("tr"));             
            $("#txtValue").data("kendoNumericTextBox").value(item.BFPValue);
            $("#txtBFPControl").val(item.BFPControl);
            wnd.center().open();
        }

        function Save() {           
            $('#txtValue').blur(); //This is so the value of the numericTextBox actually gets changed
            var item = new GenericResult();
            item.Control = $("#txtBFPControl").val();
            item.decField1 = $("#txtValue").data("kendoNumericTextBox").value();                        
            $.ajax({
                async: false,
                type: "POST",
                url: "api/BookFee/SaveBookFeesPendingValue",
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
                            if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Save Book Fees Pending Failure", data.Errors, null); }
                            else {
                                if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                    if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                        blnSuccess = true;
                                        if (ngl.isFunction(refreshBookFeesPendingGrid)) { refreshBookFeesPendingGrid(); }
                                    }
                                }
                            }
                        }
                        if (blnSuccess === false && blnErrorShown === false) {
                            if (strValidationMsg.length < 1) { strValidationMsg = "Book Fees Pending not found"; }
                            ngl.showErrMsg("Save Book Fees Pending Failure", strValidationMsg, null);
                        }
                    } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                },
                error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure"); ngl.showErrMsg("Save Book Fees Pending Failure", sMsg, null); }
            });
            wnd.close();           
        }


        function approveFeePrompt(e){
            var item = this.dataItem($(e.currentTarget).closest("tr"));
            $("#txtFeeControl").val(item.BFPControl);
            $("#txtBookControl").val(item.BFPBookControl); 
            //if (item.BookLockAllCosts === true || item.BookLockBFCCost === true)
            if (item.BookLockAllCosts === true){
                //all costs are locked so we need to show the window
                var t = "SHID: " + item.BookSHID;
                $("#wndUnlockCosts").data("kendoWindow").title(t);                               
                if (item.BookLockBFCCost === true){ $("#chkUnlockBFC").prop('checked', false); $("#divChkBFC").show(); } else{ $("#chkUnlockBFC").prop('checked', false); $("#divChkBFC").hide(); }
                wndUnlockCosts.center().open();
            }
            else{ var blnUnlockBFC = false; approveFee(blnUnlockBFC); }         
        }

        function btnUnlockOk_Click(){
            var blnUnlockBFC = false;
            if ($('#chkUnlockBFC').is(":checked")) { blnUnlockBFC = true; }
            approveFee(blnUnlockBFC);
            wndUnlockCosts.close(); 
        }

        function approveFee(blnUnlockBFC){
            kendo.ui.progress($(document.body), true);
            var gr = new GenericResult();
            gr.Control = $("#txtFeeControl").val();
            gr.intField1 = $("#txtBookControl").val();
            gr.blnField = blnUnlockBFC;                          
            $.ajax({
                type: "POST",
                url: "api/BookFee/ApproveBookFeePending",
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                data: JSON.stringify(gr),
                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                success: function (data) {     
                    try {
                        kendo.ui.progress($(document.body), false);
                        refreshBookFeesPendingGrid();
                        var blnSuccess = false;
                        var blnErrorShown = false;
                        var strValidationMsg = "";
                        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                            if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Approve Pending Fee", data.Errors, null); }
                            else {
                                if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                    if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                        blnSuccess = true;
                                        ngl.showSuccessMsg("Success!", null);
                                        if (ngl.isFunction(refreshBookFeesPendingGrid)) { refreshBookFeesPendingGrid(); }
                                    }
                                }
                            }
                        }
                        if (blnSuccess === false && blnErrorShown === false) {
                            if (strValidationMsg.length < 1) { strValidationMsg = "Approve Book Fee Pending Failure"; }
                            ngl.showErrMsg("Approve Book Fee Pending Failure", strValidationMsg, null);
                        }
                    } catch (err) {
                        ngl.showErrMsg(err.name, err.description, null);
                        kendo.ui.progress($(document.body), false);
                    }
                },
                error: function (xhr, textStatus, error) {
                    var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure");
                    ngl.showErrMsg("Approve Book Fee Pending Failure", sMsg, null); 
                    kendo.ui.progress($(document.body), false);
                }
            });
        }

        
        function openAccessorialAddWindow(e){
            var item = this.dataItem($(e.currentTarget).closest("tr"));
            var carrierName = "";
            var carrierControl = 0;
            var legalEntity = "";
            var leaControl = 0;
            if (typeof (item) !== 'undefined' && item != null) { 
                if ('CarrierControl' in item) { carrierControl = item.CarrierControl; }
                if ('CarrierName' in item) { carrierName = item.CarrierName; }
                if ('LegalEntity' in item) { legalEntity = item.LegalEntity; }  
                if ('LEAControl' in item) { leaControl = item.LEAControl; } 
            } else { ngl.showErrMsg("Carrier Required", "", null); return; }            
            if (typeof (carrierControl) === 'undefined' || carrierControl == null || carrierControl == 0) { ngl.showErrMsg("Carrier Required", "CarrierControl cannot be 0", null); return; } //The CarrierControl cannot be 0       
            $("#txtCarrierControl").val(carrierControl); //Save the CarrierControl so it can be accessed by the SaveAccessorial function
            $("#txtLEControl").val(leaControl);
            $("#wndAccessorial").data("kendoWindow").title("Add New Accessorial");
            var c = "<h3>Carrier: " + carrierName + "</h3>";
            $("#lblCarr").html(c);
            var l = "<h3>Legal Entity: " + legalEntity + "</h3>";
            $("#lblLEAcc").html(l);
            var dropdownlist = $("#ddAccessorialCode").data("kendoDropDownList");
            dropdownlist.select(function(dataItem) { return dataItem.AccessorialCode === item.BFPAccessorialCode; });
            var dItem = dropdownlist.dataItem();
            //dropdownlist.readonly();
            if (typeof (dItem) !== 'undefined' && dItem != null && ngl.isObject(dItem)) { 
                if ('AccessorialAutoApprove' in dItem) { if(dItem.AccessorialAutoApprove){ $("#chkAutoApprove").prop('checked', true); }else{ $("#chkAutoApprove").prop('checked', false); } }
                if ('AccessorialAllowCarrierUpdates' in dItem) { if(dItem.AccessorialAllowCarrierUpdates){ $("#chkAllowCarrierUpdates").prop('checked', true); }else{ $("#chkAllowCarrierUpdates").prop('checked', false); } }
                if ('AccessorialVisible' in dItem) { if(dItem.AccessorialVisible){ $("#chkAccessorialVisible").prop('checked', true); }else{ $("#chkAccessorialVisible").prop('checked', false); } }
                if ('AccessorialCaption' in dItem) { $("#txtCaption").val(dItem.AccessorialCaption); }
                if ('AccessorialEDICode' in dItem) { $("#txtEDICode").val(dItem.AccessorialEDICode); }
            }                   
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
            var item = new CarrierLegalAccessorialXref();
            item.LEAdminControl = $("#txtLEControl").val();
            item.CarrierControl = $("#txtCarrierControl").val();
            item.Caption = $("#txtCaption").data("kendoMaskedTextBox").value();
            item.EDICode = $("#txtEDICode").data("kendoMaskedTextBox").value();
            item.AutoApprove = tfAutoApprove;
            item.AllowCarrierUpdates = tfAllowCarrierUpdates;
            item.AccessorialVisible = tfAccessorialVisible;
            var dataItem = $("#ddAccessorialCode").data("kendoDropDownList").dataItem();
            item.AccessorialCode = dataItem.AccessorialCode;
            item.ApproveToleranceLow = $("#txtApproveToleranceLow").data("kendoNumericTextBox").value(); 
            item.ApproveToleranceHigh = $("#txtApproveToleranceHigh").data("kendoNumericTextBox").value();
            item.ApproveTolerancePerLow = $("#txtApproveTolerancePerLow").data("kendoNumericTextBox").value();
            item.ApproveTolerancePerHigh = $("#txtApproveTolerancePerHigh").data("kendoNumericTextBox").value();
            item.AverageValue = $("#txtAverageValue").data("kendoNumericTextBox").value();
            item.DynamicAverageValue = tfDynamicAverageValue;              
            $.ajax({
                async: false,
                type: "POST",
                url: "api/LECarrierAccessorial/SaveLECarrierAccessorial",
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
                            if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Save LE Carrier Accessorial Failure", data.Errors, null); }
                            else {
                                if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                    if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                        blnSuccess = true;                                       
                                        if (ngl.stringHasValue(data.Data[0].ErrMsg)){ ngl.showErrMsg(ngl.replaceEmptyString(data.Data[0].ErrTitle,'Save LE Carrier Accessorial Error'), data.Data[0].ErrMsg, null); return; }
                                        if (ngl.stringHasValue(data.Data[0].SuccessMsg)){ var sSuccessMsg = ngl.replaceEmptyString(data.Data[0].SuccessTitle,'', '<br>') + ngl.replaceEmptyString(data.Data[0].SuccessMsg,''); ngl.showSuccessMsg(sSuccessMsg, null); }
                                        if (ngl.stringHasValue(data.Data[0].WarningMsg)){ ngl.showWarningMsg(ngl.replaceEmptyString(data.Data[0].WarningTitle,'Save LE Carrier Accessorial Warning'), data.Data[0].WarningMsg, null); }                                   
                                        refreshBookFeesPendingGrid();
                                        wndAccessorial.close();
                                    }
                                }
                            }
                        }
                        if (blnSuccess === false && blnErrorShown === false) {
                            if (strValidationMsg.length < 1) { strValidationMsg = "Save LE Carrier Accessorial Failure"; }
                            ngl.showErrMsg("Save LE Carrier Accessorial Failure", strValidationMsg, null);
                        }
                    } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                },
                error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure"); ngl.showErrMsg("Save LE Carrier Accessorial Failure", sMsg, null); }
            });                      
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

        $(document).ready(function () {
            var PageMenuTab = <%=PageMenuTab%>;
           
 
            if (control != 0){

                ///////////CARRIERS DDL/////////////
                $("#ddlCarriers").kendoDropDownList({
                    dataTextField: "Name",
                    dataValueField: "Control",
                    optionLabel: { Name: "Show All", Control: "" },
                    autoWidth: true,
                    filter: "contains",
                    dataSource: {
                        serverFiltering: false,
                        transport: {
                            read: {
                                url: "api/BookFee/GetvBFPApprovalCarriers",
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
                    },
                    select: function(e) {
                        //var name = e.dataItem.text;
                        var value = e.dataItem.Control;                      
                        if (ngl.isNullOrWhitespace(value)){ value = 0; } //Value must be int so if value is empty string or null set it to 0
                        $("#txtCarrierControlFrom").val(value);                      
                        refreshBookFeesPendingGrid();
                    }
                });

                $("#txtValue").kendoNumericTextBox({ format: "{0:c2}" });

                $("#txtFilterVal").kendoMaskedTextBox();
                $("#dpFilterFrom").kendoDatePicker();
                $("#dpFilterTo").kendoDatePicker();
                $("#btnFilter").kendoButton({
                    icon: "filter",
                    click: function(e) {
                        var dataItem = $("#ddlFilters").data("kendoDropDownList").dataItem();
                        if(dataItem.value === "BFPModDate"){
                            var dtFrom = $("#dpFilterFrom").data("kendoDatePicker").value();
                            var dtTo = $("#dpFilterTo").data("kendoDatePicker").value();
                            var fields = "";
                            var strSp = "";                    
                            if (!dtFrom){ fields += (strSp + "Filter From date"); strSp = ", "; }
                            if (!dtTo){ fields += (strSp + "Filter To date"); strSp = ", "; }
                            if (!ngl.isNullOrWhitespace(fields)){ var alertMsg = fields + " cannot be null"; ngl.showErrMsg("Required Fields", alertMsg, null); return; }
                        }
                        refreshBookFeesPendingGrid();
                    }
                });
                $("#btnClearFilter").kendoButton({
                    icon: "filter-clear",
                    click: function(e) {
                        //set the dropdownlist back to no filter
                        var dropdownlist = $("#ddlFilters").data("kendoDropDownList");
                        dropdownlist.select(0);
                        dropdownlist.trigger("change");
                        //clear all the values from the filters
                        $("#txtFilterVal").data("kendoMaskedTextBox").value("");
                        $("#dpFilterFrom").data("kendoDatePicker").value("");
                        $("#dpFilterTo").data("kendoDatePicker").value("");
                        //Hide all the filters
                        $("#txtFilterVal").hide();
                        $("#spfilterDates").hide();
                        $("#spfilterButtons").hide();
                        //Refresh the grid
                        $('#BookFeesPendingGrid').data('kendoGrid').dataSource.read();
                    }
                });

                //clear filter values
                $("#txtFilterVal").data("kendoMaskedTextBox").value("");
                $("#dpFilterFrom").data("kendoDatePicker").value("");
                $("#dpFilterTo").data("kendoDatePicker").value("");
                //hide all filters
                $("#txtFilterVal").hide();
                $("#spfilterDates").hide();
                $("#spfilterButtons").hide();

                var filterData = [
                    { text: "", value: "None" },
                    { text: "Order Number", value: "BookCarrOrderNumber" },
                    { text: "Pro Number", value: "BookProNumber" },
                    { text: "CNS Number", value: "BookConsPrefix" },
                    { text: "SHID", value: "BookSHID" },
                    { text: "ModDate", value: "BFPModDate" },
                    { text: "Dest Name", value: "BookDestName" }
                ];

                $("#ddlFilters").kendoDropDownList({
                    dataTextField: "text",
                    dataValueField: "value",
                    dataSource: filterData,
                    select: function(e) {
                        var name = e.dataItem.text;
                        var val = e.dataItem.value;
                        //clear filter values
                        $("#txtFilterVal").data("kendoMaskedTextBox").value("");
                        $("#dpFilterFrom").data("kendoDatePicker").value("");
                        $("#dpFilterTo").data("kendoDatePicker").value("");
                        //show appropriate filters based on dropdownlist selection
                        switch(val) {
                            case "None":
                                $("#txtFilterVal").hide();
                                $("#spfilterDates").hide(); 
                                $("#spfilterButtons").hide();
                                break;
                            case "BFPModDate":
                                $("#txtFilterVal").hide();
                                $("#spfilterDates").show();
                                $("#spfilterButtons").show();
                                break;
                            default:
                                $("#txtFilterVal").show();
                                $("#spfilterDates").hide();
                                $("#spfilterButtons").show();
                                break;
                        }                     
                    }
                });
                         
           
                BFPItems = new kendo.data.DataSource({ 
                    //serverSorting: true,
                    serverPaging: true,
                    pageSize: 10,
                    transport: { 
                        read: function(options) {                           
                            var s = new AllFilter();
                            s.filterName = $("#ddlFilters").data("kendoDropDownList").value();
                            s.filterValue = $("#txtFilterVal").data("kendoMaskedTextBox").value();
                            s.filterFrom = $("#dpFilterFrom").data("kendoDatePicker").value();
                            s.filterTo = $("#dpFilterTo").data("kendoDatePicker").value();
                            //s.sortName = $("#txtSortField").val();
                            //s.sortDirection = $("#txtSortDirection").val();
                            s.sortName = "";
                            s.sortDirection = "";
                            s.page = options.data.page;
                            s.skip = options.data.skip;
                            s.take = options.data.take;
                            s.CarrierControlFrom = $("#txtCarrierControlFrom").val();  
                            if (ngl.isNullOrWhitespace(s.CarrierControlFrom)){ s.CarrierControlFrom = 0; }
                            $.ajax({
                                url: 'api/BookFee/GetBookFeesPending',
                                contentType: 'application/json; charset=utf-8',
                                dataType: 'json',
                                data: { filter: JSON.stringify(s) },
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                success: function (data) {
                                    try {
                                        var blnSuccess = false;
                                        var blnErrorShown = false;
                                        var strValidationMsg = "";
                                        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                            if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Get Book Fees Pending Failure", data.Errors, null); }
                                            else {
                                                if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                    if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                                        blnSuccess = true;                                                 
                                                        options.success(data); //notify the data source that the request succeeded                  
                                                    }
                                                    //If the request returned nothing in this case that is not an error
                                                    if (data.Data.length === 0) {
                                                        blnSuccess = true;                                                
                                                        options.success(data); //notify the data source that the request succeeded                  
                                                    }
                                                }
                                            }
                                        }
                                        if (blnSuccess === false && blnErrorShown === false) {
                                            if (strValidationMsg.length < 1) { strValidationMsg = "Book Fees Pending not found"; }
                                            ngl.showErrMsg("Get Book Fees Pending Failure", strValidationMsg, null);
                                        }
                                    } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                                },
                                error: function (xhr, textStatus, error) {                                  
                                    var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure");
                                    ngl.showErrMsg("Get Book Fees Pending Failure", sMsg, null);   
                                    options.error(error);
                                }
                            });
                        },                   
                        parameterMap: function (options, operation) { return options; } 
                    }, 
                    schema: { 
                        data: "Data",  
                        total: "Count", 
                        model: { 
                            id: "BFPControl",
                            fields: {
                                BFPControl: { type: "number" },
                                BFPBookControl: { type: "number" },
                                BFPMinimum: { type: "money" },
                                BFPValue: { type: "money" },
                                BFPVariable: { type: "number" },
                                BFPAccessorialCode: { type: "number" },
                                AccessorialName: { type: "string" },
                                BFPAccessorialFeeTypeControl: { type: "number" },
                                FeeTypeDesc: { type: "string" },
                                BFPOverRidden: { type: "boolean" },
                                BFPVariableCode: { type: "number" },
                                VariableCodeName: { type: "string" },
                                VariableCodeDesc: { type: "string" },
                                BFPVisible: { type: "boolean" },
                                BFPAutoApprove: { type: "boolean" },
                                BFPAllowCarrierUpdates: { type: "boolean" },
                                BFPCaption: { type: "string" },
                                BFPEDICode: { type: "string" },
                                BFPTaxable: { type: "boolean" },
                                BFPIsTax: { type: "boolean" },
                                BFPTaxSortOrder: { type: "number" },
                                BFPBOLText: { type: "string" },
                                BFPBOLPlacement: { type: "string" },
                                BFPAccessorialFeeAllocationTypeControl: { type: "number" },
                                FeeAllocationTypeName: { type: "string" },
                                FeeAllocationTypeDesc: { type: "string" },
                                BFPTarBracketTypeControl: { type: "number" },
                                BracketTypeName: { type: "string" },
                                BracketTypeDesc: { type: "string" },
                                BFPAccessorialFeeCalcTypeControl: { type: "number" },
                                FeeCalcTypeName: { type: "string" },
                                FeeCalcTypeDesc: { type: "string" },
                                BFPModDate: { type: "date" },
                                BFPModUser: { type: "string" },
                                BookSHID: { type: "string" },
                                BookCarrOrderNumber: { type: "string" },
                                BookProNumber: { type: "string" },
                                BookConsPrefix: { type: "string" },
                                CarrierControl: { type: "number" },
                                CarrierNumber: { type: "number" },
                                CarrierName: { type: "string" },
                                CarrierSCAC: { type: "string" },
                                BookLockAllCosts: { type: "boolean" },
                                BookLockBFCCost: { type: "boolean" },
                                BookDestName: { type: "string" },
                                TenderedCost: { type: "money" },
                                BFPMessage: { type: "string" },
                                DoesCLAXConfigExist: { type: "boolean" },
                                LEAControl: { type: "number" },
                                LegalEntity: { type: "string" }
                            }
                        }, 
                        errors: "Errors" 
                    }, 
                    error: function (e) { alert(e.errors); this.cancelChanges(); }                   
                });
            
                $('#BookFeesPendingGrid').kendoGrid({ 
                    dataSource: BFPItems, 
                    // ** Don't allow the users to change the sort on this grid ** 11/15/17 per Ari and John
                    //sortable: { mode: "single", allowUnsort: true },
                    //sort: function(e) {
                    //    if (!e.sort.dir){ e.sort.dir == ""; e.sort.field == ""; }
                    //    if (!e.sort.field){ e.sort.field == ""; }
                    //    $("#txtSortDirection").val(e.sort.dir);
                    //    $("#txtSortField").val(e.sort.field);
                    //},               
                    dataBound: function(e) { 
                        var tObj = this; 
                        if (typeof (BookFeesPendingGridDataBoundCallBack) !== 'undefined' && ngl.isFunction(BookFeesPendingGridDataBoundCallBack)) { BookFeesPendingGridDataBoundCallBack(e,tObj); } 
                    },
                    //pageable: true, 
                    pageable: { pageSizes: [5, 10, 50, "all"] },
                    resizable: true, 
                    groupable: true, 
                    columns: [                                              
                        { command: [
                            { name: "Edit", text: "", iconClass: "k-icon k-i-pencil", className: "cm-icononly-button", click: openEditWindow }, 
                            { name: "Approve", text: "Approve", iconClass: "k-icon k-i-check", click: approveFeePrompt },
                            //{ name: "ConfigureFee", text: "Add Config", iconClass: "k-icon k-i-gear", click: openAccessorialAddWindow }  //Modified By LVV on 1/14/20 - Rob said hide this button                                              
                        ], title: "Actions", width: 135 },
                        {field: "CarrierName", title: "Carrier Name", width: 100 },
                        {field: "BFPControl", title: "BPF Control", hidden: true },
                        {field: "BFPBookControl", title: "Book Control", hidden: true },
                        {field: "BookSHID", title: "SHID", attributes: { "class": "toolTipCharges" } },
                        {field: "BookCarrOrderNumber", title: "Order No"},
                        {field: "BookProNumber", title: "Pro"},
                        {field: "BookConsPrefix", title: "CNS"},
                        {field: "BFPMinimum", title: "Minimum", format: "{0:c2}", hidden: true},
                        {field: "BFPValue", title: "Amount", format: "{0:c2}" },
                        {field: "TenderedCost", title: "Tendered Cost", format: "{0:c2}" },
                        {field: "BFPVariable", title: "Variable", hidden: true},                     
                        {field: "BFPAccessorialCode", title: "Accessorial Code", hidden: true },
                        {field: "AccessorialName", title: "Accessorial Name", hidden: true },
                        {field: "BFPAccessorialFeeTypeControl", title: "FeeTypeControl", hidden: true },
                        {field: "FeeTypeDesc", title: "Fee Type Desc", hidden: true },
                        {field: "BFPOverRidden", title: "Overridden", hidden: true },                       
                        {field: "BFPVariableCode", title: "Variable Code", hidden: true },
                        {field: "VariableCodeName", title: "Variable Code Name", hidden: true },
                        {field: "VariableCodeDesc", title: "Variable Code Desc", hidden: true },
                        {field: "BFPVisible", title: "Visible", hidden: true },
                        {field: "BFPAutoApprove", title: "Auto Approve", hidden: true },
                        {field: "BFPAllowCarrierUpdates", title: "Allow Carrier Updates", hidden: true },
                        {field: "BFPCaption", title: "Caption"},
                        {field: "BFPEDICode", title: "EDI Code", hidden: true },
                        {field: "BFPTaxable", title: "Taxable", hidden: true },
                        {field: "BFPIsTax", title: "Is Tax", hidden: true },
                        {field: "BFPTaxSortOrder", title: "Tax Sort Order", hidden: true },
                        {field: "BFPBOLText", title: "BOL Text", hidden: true },
                        {field: "BFPBOLPlacement", title: "BOL Placement", hidden: true },
                        {field: "BFPAccessorialFeeAllocationTypeControl", title: "AllocationTypeControl", hidden: true },
                        {field: "FeeAllocationTypeName", title: "Allocation Type Name", hidden: true },
                        {field: "FeeAllocationTypeDesc", title: "Allocation Type Desc", hidden: true },
                        {field: "BFPTarBracketTypeControl", title: "TarBracketTypeControl", hidden: true },
                        {field: "BracketTypeName", title: "Bracket Type Name", hidden: true },
                        {field: "BracketTypeDesc", title: "Bracket Type Desc", hidden: true },
                        {field: "BFPAccessorialFeeCalcTypeControl", title: "CalcTypeControl", hidden: true },
                        {field: "FeeCalcTypeName", title: "Fee Calc Type Name", hidden: true },
                        {field: "FeeCalcTypeDesc", title: "Fee Calc Type Desc", hidden: true },
                        {field: "BFPModDate", title: "Mod Date", template: "#= kendo.toString(kendo.parseDate(BFPModDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') #"},
                        {field: "BFPModUser", title: "Mod User", hidden: true },
                        {field: "BookDestName", title: "Dest Name", hidden: false },                 
                        {field: "BookLockAllCosts", title: "Lock All Costs", template: '<input type="checkbox" #= BookLockAllCosts ? "checked=checked" : "" # disabled="disabled" ></input>', hidden: false },                      
                        {field: "BookLockBFCCost", title: "Lock BFC Cost", template: '<input type="checkbox" #= BookLockBFCCost ? "checked=checked" : "" # disabled="disabled" ></input>', hidden: false },       
                        {field: "BFPMessage", title: "Message", hidden: false, width: 150 },           
                        {field: "DoesCLAXConfigExist", title: "Fee Has Config", template: '<input type="checkbox" #= DoesCLAXConfigExist ? "checked=checked" : "" # disabled="disabled" ></input>', hidden: true }
                    ]
                });


                $("#BookFeesPendingGrid").kendoTooltip({
                    filter: ".k-grid-content td.toolTipCharges",
                    position: "right",
                    content: function(e){   
                        var d = $("#BookFeesPendingGrid").data("kendoGrid").dataItem(e.target.closest("tr"));
                        var content = "";
                    
                        $.ajax({
                            async: false,
                            url: 'api/Book/GetChargesSummaryForSHID',
                            contentType: 'application/json; charset=utf-8',
                            dataType: 'json',
                            data: { filter: d.BookSHID },
                            headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                            success: function(data) {
                                try {
                                    var blnSuccess = false;
                                    var blnErrorShown = false;
                                    var strValidationMsg = "";
                                    if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                        if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("GetChargesSummaryForSHID Failure", data.Errors, null); }
                                        else {
                                            if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {                                                     
                                                    blnSuccess = true;
                                                    content = data.Data[0];                                                                                                                                                             
                                                }
                                            }
                                        }
                                    }
                                    if (blnSuccess === false && blnErrorShown === false) {
                                        if (strValidationMsg.length < 1) { strValidationMsg = "Book record not found"; }
                                        ngl.showErrMsg("GetChargesSummaryForSHID Failure", strValidationMsg, null);
                                    }
                                } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                            },
                            error: function (xhr, textStatus, error) { 
                                var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure");
                                ngl.showErrMsg("GetChargesSummaryForSHID JSON Response Error", sMsg, null);  
                                //this.cancelChanges(); 
                            }
                        });

                        return content;
                    }
                }).data("kendoTooltip");


                wnd = $("#wnd").kendoWindow({
                    title: "Edit",
                    modal: true,
                    visible: false,
                    actions: ["save", "Minimize", "Maximize", "Close"],
                }).data("kendoWindow");
                //Modified by RHR for Kendo v-2018 xxx must reference .parent() on save button click.
                $("#wnd").data("kendoWindow").wrapper.find(".k-svg-i-save").parent().click(function (e) { Save(); });


                wndUnlockCosts = $("#wndUnlockCosts").kendoWindow({
                    width: 250,
                    modal: true,
                    visible: false              
                }).data("kendoWindow");
          

                ////////wndAccessorial//////////////
                wndAccessorial = $("#wndAccessorial").kendoWindow({
                    title: "Add",
                    height: 500,
                    width: 500,
                    minWidth: 500,
                    modal: true,
                    visible: false,
                    actions: ["save", "Minimize", "Maximize", "Close"],
                }).data("kendoWindow");
                //Modified by RHR for Kendo v-2018 xxx must reference .parent() on save button click.
                $("#wndAccessorial").data("kendoWindow").wrapper.find(".k-svg-i-save").parent().click(function (e) { SaveAccessorial(); }); 


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
                            if ('AccessorialCaption' in item) { $("#txtCaption").val(item.AccessorialCaption); }
                            //if ('AccessorialAutoApprove' in item) { if(item.AccessorialAutoApprove){ $("#chkAutoApprove").prop('checked', true); }else{ $("#chkAutoApprove").prop('checked', false); } }
                            //if ('AccessorialAllowCarrierUpdates' in item) { if(item.AccessorialAllowCarrierUpdates){ $("#chkAllowCarrierUpdates").prop('checked', true); }else{ $("#chkAllowCarrierUpdates").prop('checked', false); } }
                            //if ('AccessorialVisible' in item) { if(item.AccessorialVisible){ $("#chkAccessorialVisible").prop('checked', true); }else{ $("#chkAccessorialVisible").prop('checked', false); } }                            
                            //if ('AccessorialEDICode' in item) { $("#txtEDICode").val(item.AccessorialEDICode); }
                        }                 
                    }
                });


                //Kendo Definitions
                $("#txtCaption").kendoMaskedTextBox();
                $("#txtEDICode").kendoMaskedTextBox();
                $("#txtApproveToleranceLow").kendoNumericTextBox({ format: "{0:c2}" });
                $("#txtApproveToleranceHigh").kendoNumericTextBox({ format: "{0:c2}" });
                $("#txtApproveTolerancePerLow").kendoNumericTextBox({ format: "p0", factor: 100, min: 0, max: 1, step: 0.01 });
                $("#txtApproveTolerancePerHigh").kendoNumericTextBox({ format: "p0", factor: 100, min: 0, max: 1, step: 0.01 });               

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
            }
            var PageReadyJS = <%=PageReadyJS%>;
            menuTreeHighlightPage(); //must be called after PageReadyJS
            var divWait = $("#h1Wait");
            if (typeof (divWait) !== 'undefined') { divWait.hide(); }
        });
    </script>
            <style>
                /*.k-grid tbody tr td { overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }
                .k-tooltip { max-height: 500px; max-width: 450px; overflow-y: auto; }*/                    
                .k-grid tbody .k-grid-Edit { min-width: 0; }
                .k-grid tbody .k-grid-Edit .k-icon { margin: 0; }                         
            </style>   
      </div>  
    </body>
</html>
