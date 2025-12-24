<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrderPreview.aspx.cs" Inherits="DynamicsTMS365.OrderPreview" %>

<!DOCTYPE html>

<html>
    <head >
        <title>DTMS Order Preview</title>         
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
                                    <!--Radio Buttons-->
                                    <div id="divOPFilterRadioButtons" style="float: left;">
                                        <input type="radio" name="rbgOPFilter" id="rbNatAcct" value="nat">Nat Acct
                                        <input type="radio" name="rbgOPFilter" id="rbCompany" value="comp">Company
                                    </div>
                                    <div class="tblResponsive-wrap" style="float: none;">&nbsp;</div>
                                    <div id="divNatAcctFilter" style="float: left;">
                                        <table class="tblResponsive">
                                            <tr><td class="tblResponsive-top">Nat Acct</td></tr>
                                            <tr><td class="tblResponsive-top"><input id="ddlNatAcctNumbers" style="width: 250px;" /></td></tr>
                                        </table>
                                    </div>
                                    <div id="divCompFilter" style="float: left;">                                      
                                        <table class="tblResponsive">
                                            <tr><td class="tblResponsive-top">Company</td></tr>
                                            <tr><td class="tblResponsive-top"><input id="ddlCompany" style="width: 250px;" /></td></tr>
                                        </table>
                                    </div>
                                    <div id="divFrtFilter" style="float: left;">                                  
                                        <table class="tblResponsive">
                                            <tr><td class="tblResponsive-top">Trans Type</td></tr>
                                            <tr><td class="tblResponsive-top"><input id="ddlFrtType" style="width: 250px;" /></td></tr>
                                        </table>
                                    </div>
                                </div>
                                <div class="tblResponsive-wrap" style="float: none;">&nbsp;</div>
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


            <div id="wndViewChanges">
                <div id="viewChangesGrid"></div>
            </div>


    <% Response.Write(PageTemplates); %>
    <%-- HelpWindow needs to go after AuthNotification because it uses the notification object created there --%>
    <% Response.Write(AuthLoginNotificationHTML); %>   
    <% Response.WriteFile("~/Views/HelpWindow.html"); %>          
    <script>    
        <% Response.Write(ADALPropertiesjs); %>

        var PageControl = '<%=PageControl%>';             
        var oPOHdrGrid = null;        
        var tObj = this;
        var tPage = this;           
       

        <% Response.Write(NGLOAuth2); %>

        
        var iBookPK = 0;
        var pohdrGridSelectedRow; 
        var pohdrGridSelectedRowDataItem;   
        var selectedOrderNumber = "";
        var selectedOrderSequence = 0;
        var selectedDefaultCustomer = 0;
        var ddlNatAcctNumbers002;
        var ddlCompany002;
        var ddlFrtType002;
        var pgCompNatNumberFltr = 0;
        var pgCompNumberFltr = 0;
        var pgFrtTypeFltr = 0;
        var blnNatAcctCheckedFltr = false;
        var blnCompCheckedFltr = false;
        <% Response.Write(PageCustomJS); %>

        var wndViewChanges = kendo.ui.Window;

        //*************  execActionClick  ****************
        function execActionClick(btn, proc){           
            if(btn.id == "btnOPProcessAll"){ ProcessAll(); }                  
            ////else if(btn.id == "btnOpenCarrier"){ if (isOrderSelected() === true) {location.href = "LECarrierMaint";} } 
            else if (btn.id == "btnViewImportChanges" ){ if (isOrderSelected() === true){ViewImportChanges();} }
            else if (btn.id == "btnRefresh" ){ refresh(); }
            else if (btn.id == "btnResetCurrentUserConfig"){ resetCurrentUserConfig(PageControl); }
        }

         
        //*************  Action Menu Functions ****************
        function refresh(){ oPOHdrGrid.dataSource.read(); }
        function refreshPOHdrGrid(){ oPOHdrGrid.dataSource.read(); }

        function getSelectedOrder() {          
            try {
                var strTitle = "Order Record Required";
                var strMsgShort = "Please select an Order to continue";
                var strMsgLong = "Invalid Record Identifier - " + strMsgShort;              
                pohdrGridSelectedRow = oPOHdrGrid.select(); //Get the selected row
                if (typeof (pohdrGridSelectedRow) === 'undefined' || pohdrGridSelectedRow == null) { ngl.showValidationMsg(strTitle, strMsgShort, tPage); return false; }                               
                pohdrGridSelectedRowDataItem = oPOHdrGrid.dataItem(pohdrGridSelectedRow); //Get the dataItem for the selected row
                if (typeof (pohdrGridSelectedRowDataItem) === 'undefined' || pohdrGridSelectedRowDataItem == null) { ngl.showValidationMsg(strTitle, strMsgShort, tPage); return false; }
                //Verify the key fields are in the record
                if ("POHDROrderNumber" in pohdrGridSelectedRowDataItem){ selectedOrderNumber = pohdrGridSelectedRowDataItem.POHDROrderNumber; } else { ngl.showValidationMsg(strTitle, strMsgLong, tPage); return false; }
                if ("POHDROrderSequence" in pohdrGridSelectedRowDataItem){ selectedOrderSequence = pohdrGridSelectedRowDataItem.POHDROrderSequence; } else{ ngl.showValidationMsg(strTitle, strMsgLong, tPage); return false; }
                if ("POHDRDefaultCustomer" in pohdrGridSelectedRowDataItem){ selectedDefaultCustomer = pohdrGridSelectedRowDataItem.POHDRDefaultCustomer; } else{ ngl.showValidationMsg(strTitle, strMsgLong, tPage); return false; }
            } catch (err) { ngl.showErrMsg("Cannot Continue", "There was an unexpected error, please reload the screen. If this continues please contact technical support. Error: " + err.message, tPage); return false; }        
            return true;
        }   
        function isOrderSelected() {
            if (typeof (pohdrGridSelectedRowDataItem) === 'undefined' || pohdrGridSelectedRowDataItem === null) { return getSelectedOrder(); }
            return true;
        }

        
        //*************  Call Back Functions ****************
        var blnPOHdrGridChangeBound = false;
        function POHdrGridDataBoundCallBack(e,tGrid){           
            oPOHdrGrid = tGrid;
            if (blnPOHdrGridChangeBound == false){
                oPOHdrGrid.bind("change", getSelectedOrder);
                blnPOHdrGridChangeBound = true;
            }        
            //oPOHdrGrid.bind("detailInit", grid_detailInit);

            //Modified By LVV on 2/4/20 - Change button to reflect Hold Status per Ari John and Rob
            // get the index of the HoldStatus column
            var columns = e.sender.columns;
            var columnIndex = tGrid.wrapper.find(".k-grid-header [data-field=" + "POHDRHoldLoad" + "]").index();
            var ds = tGrid.dataSource.data();
            for (var j=0; j < ds.length; j++) {
                if (typeof (ds[j].POHDRHoldLoad) !== 'undefined' && ds[j].POHDRHoldLoad != null) {
                    if (ds[j].POHDRHoldLoad === false) {
                        var item = tGrid.dataSource.get(ds[j].POHdrControl); //Get by ID or any other preferred method                      
                        var btn = tGrid.tbody.find("tr[data-uid='"+item.uid+"'] .k-grid-btnHold");   
                        var pbtn = tGrid.tbody.find("tr[data-uid='"+item.uid+"'] .k-grid-btnProcess");   
                        //btn.html("<span> Hold </span>");//btn.html("<span style='color:green;'>Hold</span>");
                        btn.removeAttr("style")
                        pbtn.removeAttr("style")
                    }
                    else{
                        var item = tGrid.dataSource.get(ds[j].POHdrControl); //Get by ID or any other preferred method                      
                        var btn = tGrid.tbody.find("tr[data-uid='"+item.uid+"'] .k-grid-btnHold");                          
                        var pbtn = tGrid.tbody.find("tr[data-uid='"+item.uid+"'] .k-grid-btnProcess");   
                        btn.css("color", "red");
                        pbtn.css("color", "red");
                       // btn.html("<span style='color:red;'>*HOLD*</span>");
                        ////disable the Process button
                        //tGrid.tbody.find("tr[data-uid='"+item.uid+"'] .k-grid-btnProcess").prop('disabled', true); //Disable the ConfigureFee button
                        //tGrid.tbody.find("tr[data-uid='"+item.uid+"'] .k-grid-btnProcess").addClass("k-state-disabled"); //Add the diabled class to the css (change button color)                       
                        //tGrid.tbody.find("tr[data-uid='"+item.uid+"'] .k-grid-btnProcess").removeClass("cm-greenicon-button"); //remove green color
                        //tGrid.tbody.find("tr[data-uid='"+item.uid+"'] .k-grid-btnProcess").addClass("cm-icononly-button"); //add back icononly styling
                    }
                }                
            }
        }
        function POHdrGridGetStringData(s)
        {           
            blnFirstLoad = false;
            var f = new OrderPreviewFilters(); 
            f.NatAcctChecked = blnNatAcctCheckedFltr;
            f.CompChecked = blnCompCheckedFltr;
            f.NatAcctDDLValue = pgCompNatNumberFltr;
            f.CompDDLValue = pgCompNumberFltr;
            f.FrtTypDDLValue = pgFrtTypeFltr;      
            s.Data = JSON.stringify(f);
            return JSON.stringify(f);
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
                                    if(psData.NatAcctDDLValue !== 0 || psData.CompDDLValue !== 0){
                                        if (psData.NatAcctChecked === true){                                           
                                            $("#rbNatAcct").prop("checked", true);
                                            $("#rbCompany").prop("checked", false);
                                            $("#divNatAcctFilter").show();
                                            $("#divCompFilter").hide();
                                            blnNatAcctCheckedFltr = true;
                                            blnCompCheckedFltr = false;
                                            pgCompNatNumberFltr = psData.NatAcctDDLValue;
                                            pgCompNumberFltr = 0; //pgCompNumberFltr = psData.CompDDLValue;                       
                                        }
                                        else{
                                            $("#rbNatAcct").prop("checked", false);
                                            $("#rbCompany").prop("checked", true);
                                            $("#divCompFilter").show();
                                            $("#divNatAcctFilter").hide();
                                            blnNatAcctCheckedFltr = false;
                                            blnCompCheckedFltr = true;
                                            pgCompNatNumberFltr = 0; //pgCompNatNumberFltr = psData.NatAcctDDLValue;
                                            pgCompNumberFltr = psData.CompDDLValue;
                                        }
                                        pgFrtTypeFltr = psData.FrtTypDDLValue; 
                                        //set the ddl 
                                        ddlNatAcctNumbers002.select(function(dataItem) { return dataItem.Description === psData.NatAcctDDLValue; });
                                        ddlCompany002.select(function(dataItem) { return dataItem.Description === psData.CompDDLValue; });
                                        ddlFrtType002.select(function(dataItem) { return dataItem.Control === psData.FrtTypDDLValue; });                                                               
                                    }                                                                                                                                      
                                }                                   
                            }                              
                        }                            
                    }                        
                }                      
                if (blnSuccess === false && blnErrorShown === false) {  
                    if (strValidationMsg.length < 1) { strValidationMsg = "If this is your first time on this page your settings will be saved for your next visit, if not please contact technical support if you continue to receive this message."; }
                    ngl.showInfoNotification("Unable to Read Page Settings", strValidationMsg, null); 
                    $("#rbNatAcct").prop("checked", true);
                    $("#rbCompany").prop("checked", false); 
                    $("#divNatAcctFilter").show();
                    $("#divCompFilter").hide();                                     
                    ddlNatAcctNumbers002.select(0);
                    ddlCompany002.select(0);
                    ddlFrtType002.select(0);
                    var dataItemNatAcct =ddlNatAcctNumbers002.dataItem();
                    var dataItemFrtType =ddlFrtType002.dataItem();
                    blnNatAcctCheckedFltr = true;
                    blnCompCheckedFltr = false;
                    pgCompNumberFltr = 0;
                    pgCompNatNumberFltr = dataItemNatAcct.Description;
                    pgFrtTypeFltr = dataItemFrtType.Control;
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

        function ProcessSingleSuccessCallback(data) {
            var oResults = new nglEventParameters();
            oResults.source = "ProcessSingleSuccessCallback";
            oResults.widget = this;
            oResults.msg = 'Failed'; //set default to Failed     
            var tObj = this;
            try {
                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                    if (ngl.stringHasValue(data.Errors)) {
                        oResults.error = new Error();
                        oResults.error.name = "Process Single Failure";
                        oResults.error.message = data.Errors;
                        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                    }else{ oResults.msg = 'Success, Order Processed'; }
                }
            }catch (err) { oResults.error = err; ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj); }        
            if (ngl.isFunction(refreshPOHdrGrid)) { refreshPOHdrGrid(); }
        }
        function ProcessSingleAjaxErrorCallback(xhr, textStatus, error) {
            var oResults = new nglEventParameters();
            var tObj = this;
            oResults.source = "ProcessSingleAjaxErrorCallback";
            oResults.widget = this;
            oResults.msg = 'Failed'; //set default to Failed  
            oResults.error = new Error();
            oResults.error.name = "Process Single Failure"
            oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
            ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);           
            if (ngl.isFunction(refreshPOHdrGrid)) { refreshPOHdrGrid(); }
        }

        function ToggleHoldStatusSuccessCallback(data) {
            var oResults = new nglEventParameters();
            oResults.source = "ToggleHoldStatusSuccessCallback";
            oResults.widget = this;
            oResults.msg = 'Failed'; //set default to Failed     
            var tObj = this;
            try {
                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                    if (ngl.stringHasValue(data.Errors)) {
                        oResults.error = new Error();
                        oResults.error.name = "Toggle Hold Status Failure";
                        oResults.error.message = data.Errors;
                        ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                    }else{ oResults.msg = 'Success, Hold Status Updated'; }
                }
            }catch (err) { oResults.error = err; ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj); }        
            if (ngl.isFunction(refreshPOHdrGrid)) { refreshPOHdrGrid(); }
        }
        function ToggleHoldStatusAjaxErrorCallback(xhr, textStatus, error) {
            var oResults = new nglEventParameters();
            var tObj = this;
            oResults.source = "ToggleHoldStatusAjaxErrorCallback";
            oResults.widget = this;
            oResults.msg = 'Failed'; //set default to Failed  
            oResults.error = new Error();
            oResults.error.name = "Toggle Hold Status Failure"
            oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
            ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);           
            if (ngl.isFunction(refreshPOHdrGrid)) { refreshPOHdrGrid(); }
        }


        //*************  Page Level Functions ****************
        function ConfirmProcessSingle(iRet){          
            if (typeof (iRet) === 'undefined' || iRet === null  || iRet === 0 ) { return; } //Chose the Cancel action           
            //Chose the Ok action
            if(typeof (processSingleOrder) !== 'undefined' && processSingleOrder != null && ngl.isObject(processSingleOrder)){ ImportPOHdr(processSingleOrder); } //Code Change PFM 6/27/2012 - All Process Methods will not be syncronous instead of asyncronous due to the Routing Guide enhancements.
        }

        var processSingleOrder = null;
        function ProcessSingle(e)
        {           
            var item = this.dataItem($(e.currentTarget).closest("tr"));               
            processSingleOrder = null; //clear any old values
            processSingleOrder = item;   
            var strModVerify = item.POHDRModVerify.toUpperCase();
            if (item.POHDRHoldLoad === false) {
                if (item.POHDRModVerify !== "No Pro") {
                    if (strModVerify === "FINALIZED" || strModVerify === "NEW TRAN-F") {
                        var title = "Cannot Import Finalized Order";
                        var msg = "The selected Booking Order has already been finalized. You must un-finalize the order before it can be imported again." + "</br></br>" + "If you have already un-finalized the order select OK to import the record. Warning! All costs on the entire load will be recalculated."; //LocalizeString("ModVerifyFinalizedInformation") + LocalizeString("ModVerifyFinalizedQuestion")
                        ngl.OkCancelConfirmation(title, msg, 400, 400, null, "ConfirmProcessSingle"); 
                    }else if(strModVerify === "DELETED") {
                        var title = "Remove Deleted Order From Download Screen"; //LocalizeString("ModVerifyDeletedTitle")
                        var msg = "The selected Booking Order has already been deleted. Do you want to remove this item from the download?"; //LocalizeString("ModVerifyDeletedQuestion")
                        ngl.OkCancelConfirmation(title, msg, 400, 400, null, "ConfirmProcessSingle"); 
                    }else if(strModVerify === "DELETE-B") {
                        var title = "Delete Selected Order From TMS"; //LocalizeString("ModVerifyDeleteBTitle")
                        var msg = "The selected Booking Order has been marked for deletion. Are you sure you want to delete this Booking Record?" + "</br></br>" + "This process cannot be undone!"; //LocalizeString("ModVerifyDeleteBQuestion") + LocalizeString("ModVerifyDeleteBInformation")
                        ngl.OkCancelConfirmation(title, msg, 400, 400, null, "ConfirmProcessSingle"); 
                    }else if(strModVerify === "DELETE-F") {
                        var title = "Deleted Selected Finalized Order From TMS"; //LocalizeString("ModVerifyDeleteFTitle")
                        var msg = "The selected Booking Order has been marked for deletion but has also been finalized. You should double check the status of this order before it is processed. Are you sure you want to delete this Booking Record?" + "</br></br>" + "This process cannot be undone!"; //LocalizeString("ModVerifyDeleteFQuestion") + LocalizeString("ModVerifyDeleteFInformation")
                        ngl.OkCancelConfirmation(title, msg, 400, 400, null, "ConfirmProcessSingle"); 
                    }else if(strModVerify === "NO LANE") {
                        var title = "Import Booking Record Warning!"; //LocalizeString("ModVerifyNoLaneTitle")
                        var msg = "The selected Booking Order was imported with a missing lane number. You must add the lane before it can be imported." + "</br></br>" + "If you have already added the lane select OK to import the record."; //LocalizeString("ModVerifyNoLaneInformation1") + LocalizeString("ModVerifyNoLaneInformation2")
                        ngl.OkCancelConfirmation(title, msg, 400, 400, null, "ConfirmProcessSingle"); 
                    }else if (strModVerify === "SPLIT ORDER") {
                        //Code Change PFM 8/2/2013 - Adding Split Order Logic. Cannot import split ordrs at all. All of the splits must be manually edited, then this staging record must be deleted.
                        var title = "Import Booking Record Warning!"; //LocalizeString("ModVerifySplitOrder")
                        var msg = "The selected Booking Order is a split order and cannot be import automatically. You must manually modify the splits and delete this record."; //LocalizeString("ModVerifySplitOrderInfo")                       
                        ngl.Alert(title, msg, 400, 400);
                    }else if (strModVerify === "SPLIT NEW COMP") {
                        //Code Change PFM 8/2/2013 - Adding Split Order Logic. Cannot import split ordrs at all. All of the splits must be manually edited, then this staging record must be deleted.
                        var title = "Import Booking Record Warning!"; //LocalizeString("ModVerifySplitOrder")
                        var msg = "The selected Booking Order is a split order with a new Company and cannot be import automatically. You must manually modify the splits and delete this record."; //LocalizeString("ModVerifySplitOrderNewCompInfo")                       
                        ngl.Alert(title, msg, 400, 400);
                    }else if (strModVerify === "SPLIT NEW TRAN") {
                        //Code Change PFM 8/2/2013 - Adding Split Order Logic. Cannot import split ordrs at all. All of the splits must be manually edited, then this staging record must be deleted.
                        var title = "Import Booking Record Warning!"; //LocalizeString("ModVerifySplitOrder")
                        var msg = "The selected Booking Order is a split order with a new Transport Type and cannot be import automatically. You must manually modify the splits and delete this record."; //LocalizeString("ModVerifySplitOrderNewTranInfo")                       
                        ngl.Alert(title, msg, 400, 400);
                    }else if (strModVerify === "SPLIT DELETED") {
                        //Code Change PFM 8/2/2013 - Adding Split Order Logic. Cannot import split ordrs at all. All of the splits must be manually edited, then this staging record must be deleted.
                        var title = "Import Booking Record Warning!"; //LocalizeString("ModVerifySplitOrder")
                        var msg = "The selected Booking Order is a split order that has been deleted or canceled and cannot be import automatically. You must manually modify the splits and delete each sequence of this order for this company or lane. It is possible that the shipping information has changed so take caution that the correct records are deleted."; //LocalizeString("ModVerifySplitOrderDeletedInfo")                       
                        ngl.Alert(title, msg, 400, 400);
                    }else {
                        //Code Change PFM 6/27/2012 - All Process Methods will not be syncronous instead of asyncronous due to the Routing Guide enhancements.
                        ImportPOHdr(item);
                    }
                }else { ImportPOHdr(item); } //Code Change PFM 6/27/2012 - All Process Methods will not be syncronous instead of asyncronous due to the Routing Guide enhancements.
            }else{
                var title = "Order On Hold"; //LocalizeString("ModVerifyHoldTitle")
                var msg = "This order has been placed on hold and cannot be processed."; //LocalizeString("ModVerifyHoldInformation")                       
                ngl.Alert(title, msg, 400, 400);
            }
        }

        function ImportPOHdr(order){
            var intDefCompNumber = 0;
            if (typeof (order.POHDRDefaultCustomer) !== 'undefined' && order.POHDRDefaultCustomer != null){ intDefCompNumber = order.POHDRDefaultCustomer }         
            var gr = new GenericResult();
            gr.strField = order.POHDRModVerify;
            gr.strField2 = order.POHDROrderNumber;
            gr.strField3 = order.POHDRvendor;
            gr.strField4 = order.POHDRPRONumber;
            gr.intField1 = order.POHDROrderSequence;
            gr.intField2 = intDefCompNumber;
            var oCRUDCtrl = new nglRESTCRUDCtrl();
            var blnRet = oCRUDCtrl.update("POHdr/ProcessSingle", gr, tPage, "ProcessSingleSuccessCallback", "ProcessSingleAjaxErrorCallback", true);             
        }

        function Hold(e){
            var item = this.dataItem($(e.currentTarget).closest("tr"));
            var oCRUDCtrl = new nglRESTCRUDCtrl();
            var blnRet = oCRUDCtrl.update("POHdr/ToggleHoldStatus", item, tPage, "ToggleHoldStatusSuccessCallback", "ToggleHoldStatusAjaxErrorCallback", true);
        }
        

        function ConfirmProcessAll(iRet){          
            if (typeof (iRet) === 'undefined' || iRet === null  || iRet === 0 ) { return; } //Chose the Cancel action           
            //Chose the Ok action
            $.ajax({
                async: true,
                type: 'POST',
                url: 'api/POHdr/ProcessAll',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                success: function (data) { refreshPOHdrGrid(); },
                error: function (xhr, textStatus, error) { ngl.showErrMsg("Process All JSON Response Error",formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"), null);  this.cancelChanges(); }
            });
        }
        function ProcessAll(){
            var title = "Process All"; //LocalizeString("ProcessAll")
            var msg = "This will Process All orders on the current page, would you like to continue?"; //LocalizeString("QuestionProcessAllPagesOrderPreview")
            ngl.OkCancelConfirmation(title, msg, 400, 400, null, "ConfirmProcessAll");
        }

        function ViewImportChanges(){
            if (pohdrGridSelectedRowDataItem.POHDRModVerify !== "No Pro" && pohdrGridSelectedRowDataItem.POHDRPRONumber.length > 3){
                $('#viewChangesGrid').data('kendoGrid').dataSource.read();
                wndViewChanges.center().open();
            } else{ ngl.showValidationMsg("Pro Number Required", "Cannot view changes for an order with No Pro", tPage); return false; }
        }


        function ConfirmDeletePOHdr(iRet){          
            if (typeof (iRet) === 'undefined' || iRet === null  || iRet === 0 ) { return;}          
            if (typeof (oPOHdrDeleteItem) !== 'undefined' && ngl.isObject(oPOHdrDeleteItem)) {
                $.ajax({
                    type: "POST",
                    url: "api/POHdr/DeletePOHdr",
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    data: JSON.stringify(oPOHdrDeleteItem),
                    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                    success: function (data) {     
                        try {
                            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                if (ngl.stringHasValue(data.Errors)) { ngl.showErrMsg("Delete POHdr Failure", data.Errors, null); }
                            }
                        } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                        if (ngl.isFunction(refreshPOHdrGrid)) { refreshPOHdrGrid(); }
                    },
                    error: function (xhr, textStatus, error) {
                        var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Delete Data Failure");
                        ngl.showErrMsg("Delete POHdr Failure", sMsg, null);                      
                        if (ngl.isFunction(refreshPOHdrGrid)) { refreshPOHdrGrid(); }
                    }
                }); 
            }
        }
       
        var oPOHdrDeleteItem = null; 
        function DeletePOHdr(e){
            oPOHdrDeleteItem = null; //clear any old values
            oPOHdrDeleteItem = this.dataItem($(e.currentTarget).closest("tr")); 
            if (typeof (oPOHdrDeleteItem) !== 'undefined' && ngl.isObject(oPOHdrDeleteItem)) {
                ngl.OkCancelConfirmation(
                       "Delete Selected Record",
                       "This action cannot be undone. Are you sure you want to continue?",
                       400,
                       400,
                       null,
                       "ConfirmDeletePOHdr");
            };      
        }


        //OnChange event handler for radio buttons
        $('input[type=radio][name=rbgOPFilter]').on('change', function() {
            switch($(this).val()) {
                case 'nat':                    
                    $("#divNatAcctFilter").show();
                    $("#divCompFilter").hide();
                    pgCompNumberFltr = 0;
                    pgCompNatNumberFltr = $("#ddlNatAcctNumbers").data("kendoDropDownList").value();
                    blnNatAcctCheckedFltr = true;
                    blnCompCheckedFltr = false;
                    if(blnFirstLoad === false){ refreshPOHdrGrid(); }
                    break;
                case 'comp':
                    $("#divCompFilter").show();
                    $("#divNatAcctFilter").hide();
                    pgCompNatNumberFltr = 0;
                    pgCompNumberFltr = $("#ddlCompany").data("kendoDropDownList").value();
                    blnNatAcctCheckedFltr = false;
                    blnCompCheckedFltr = true;
                    if(blnFirstLoad === false){ refreshPOHdrGrid(); }
                    break;
            }
        });

        function grid_detailInit(e) {
            var detailRow = e.detailRow;
            detailRow.find(".tabstrip").kendoTabStrip({
                animation: {
                    open: { effects: "fadeIn" }
                }
            });                        
            detailRow.find(".details").kendoGrid({
                dataSource: {
                    serverSorting: true,
                    serverPaging: true,
                    pageSize: 10,
                    transport: { 
                        read: function(options) { 
                            var s = new AllFilter(); 
                            s.Data = e.data.POHDROrderNumber;
                            s.ParentControl = e.data.POHDROrderSequence;
                            s.CompNumberFrom = e.data.POHDRDefaultCustomer;
                            $.ajax({ 
                                url: 'api/POItem/GetRecords/' + s, 
                                contentType: 'application/json; charset=utf-8', 
                                dataType: 'json', 
                                data: { filter: JSON.stringify(s) }, 
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                                success: function(data) { 
                                    options.success(data); 
                                    if (data.Errors != null) { 
                                        if (data.StatusCode === 203){ ngl.showErrMsg("Authorization Timeout", data.Errors, null); } else { ngl.showErrMsg("Access Denied", data.Errors, null); } 
                                    } 
                                }, 
                                error: function(result) { options.error(result); } 
                            }); 
                        }, 
                        parameterMap: function (options, operation) { return options; }
                    },  
                    schema: { 
                        data: "Data",  
                        total: "Count", 
                        model: { 
                            id: "POItemControl",
                            fields: {                                       
                                POItemControl: { type: "string", editable: false },
                                ItemPONumber: { type: "string" },
                                ItemNumber: { type: "string", editable: false },
                                Pack: { type: "integer", editable: false },
                                Size: { type: "string", editable: false },
                                Description: { type: "string", editable: false },
                                FreightCost: { type: "number", editable: false },
                                ItemCost: { type: "number", editable: false },
                                QtyOrdered: { type: "integer", editable: false },
                                Weight: { type: "number", editable: false },
                                Cube: { type: "integer", editable: false },
                                FixOffInvAllow: { type: "number", editable: false },
                                FixFrtAllow: { type: "number", editable: false },
                                Brand: { type: "string", editable: false },
                                CostCenter: { type: "string", editable: false },
                                CreatedUser: { type: "string", editable: false },
                                CreatedDate: { type: "date", editable: false },
                                CustItemNumber: { type: "string", editable: false },
                                CustomerNumber: { type: "integer", editable: false },
                                GTIN: { type: "string", editable: false },
                                Hazmat: { type: "string", editable: false },
                                LotExpirationDate: { type: "date", editable: false },
                                LotNumber: { type: "string", editable: false },
                                POOrderSequence: { type: "integer", editable: false },
                                PalletType: { type: "string", editable: false },
                                POItemHazmatTypeCode: { type: "string", editable: false },
                                POItem49CFRCode: { type: "string", editable: false },
                                POItemIATACode: { type: "string", editable: false },
                                POItemDOTCode: { type: "string", editable: false },
                                POItemMarineCode: { type: "string", editable: false },
                                POItemNMFCClass: { type: "string", editable: false },
                                POItemFAKClass: { type: "string", editable: false },
                                POItemLimitedQtyFlag: { type: "boolean", editable: false },
                                POItemPallets: { type: "number", editable: false },
                                POItemTies: { type: "number", editable: false },
                                POItemHighs: { type: "number", editable: false },
                                POItemQtyPalletPercentage: { type: "number", editable: false },
                                POItemQtyLength: { type: "number", editable: false },
                                POItemQtyWidth: { type: "number", editable: false },
                                POItemQtyHeight: { type: "number", editable: false },
                                POItemStackable: { type: "boolean", editable: false },
                                POItemLevelOfDensity: { type: "integer", editable: false },
                                POItemCompLegalEntity: { type: "string", editable: false },
                                POItemCompAlphaCode: { type: "string", editable: false },
                                POItemNMFCSubClass: { type: "string", editable: false },
                                POItemUser1: { type: "string", editable: false },
                                POItemUser2: { type: "string", editable: false },
                                POItemUser3: { type: "string", editable: false },
                                POItemUser4: { type: "string", editable: false },
                                POItemCommCode: { type: "string", editable: false },
                                POItemCustomerPO: { type: "string", editable: false },
                                POItemLocationCode: { type: "string", editable: false },
                                POItemUpdated: { type: "string" }
                            }
                        }, 
                        errors: "Errors" 
                    }, 
                    error: function (xhr, textStatus, error) { ngl.showErrMsg("Get POItems JSON Response Error",formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"), null);  this.cancelChanges(); } 
                },
                pageable: { pageSizes: [5, 10, 50, "all"] },
                resizable: true, 
                groupable: false, 
                columns: [
                    {field: "POItemControl", title: "POItemControl", hidden: true },
                    {field: "ItemPONumber", title: "PO Number" },
                    {field: "ItemNumber", title: "Number" },
                    {field: "Pack", title: "Pack", hidden: true },
                    {field: "Size", title: "Size" },
                    {field: "Description", title: "Description" },
                    {field: "FreightCost", title: "Freight Cost", format: "{0:c2}" },
                    {field: "ItemCost", title: "Cost", format: "{0:c2}" },
                    {field: "QtyOrdered", title: "Unit Qty" },
                    {field: "Weight", title: "Wgt" },
                    {field: "Cube", title: "Volume" },
                    {field: "FixOffInvAllow", title: "Off Inv", format: "{0:n2}" },
                    {field: "FixFrtAllow", title: "Fix Frt Allow", format: "{0:c2}" },
                    {field: "Brand", title: "Brand", hidden: true },
                    {field: "CostCenter", title: "CostCenter", hidden: true },
                    {field: "CreatedUser", title: "CreatedUser", hidden: true },
                    {field: "CreatedDate", title: "CreatedDate", hidden: true },
                    {field: "CustItemNumber", title: "CustItemNumber", hidden: true },
                    {field: "CustomerNumber", title: "CustomerNumber", hidden: true },
                    {field: "GTIN", title: "GTIN", hidden: true },
                    {field: "Hazmat", title: "Hazmat", hidden: true },
                    {field: "LotExpirationDate", title: "LotExpirationDate", hidden: true },
                    {field: "LotNumber", title: "LotNumber", hidden: true },
                    {field: "POOrderSequence", title: "POOrderSequence", hidden: true },
                    {field: "PalletType", title: "PalletType", hidden: true },
                    {field: "POItemHazmatTypeCode", title: "POItemHazmatTypeCode", hidden: true },
                    {field: "POItem49CFRCode", title: "POItem49CFRCode", hidden: true },
                    {field: "POItemIATACode", title: "POItemIATACode", hidden: true },
                    {field: "POItemDOTCode", title: "POItemDOTCode", hidden: true },
                    {field: "POItemMarineCode", title: "POItemMarineCode", hidden: true },
                    {field: "POItemNMFCClass", title: "POItemNMFCClass", hidden: true },
                    {field: "POItemFAKClass", title: "POItemFAKClass", hidden: true },
                    {field: "POItemLimitedQtyFlag", title: "POItemLimitedQtyFlag", hidden: true },
                    {field: "POItemPallets", title: "POItemPallets", hidden: true },
                    {field: "POItemTies", title: "POItemTies", hidden: true },
                    {field: "POItemHighs", title: "POItemHighs", hidden: true },
                    {field: "POItemQtyPalletPercentage", title: "POItemQtyPalletPercentage", hidden: true },
                    {field: "POItemQtyLength", title: "POItemQtyLength", hidden: true },
                    {field: "POItemQtyWidth", title: "POItemQtyWidth", hidden: true },
                    {field: "POItemQtyHeight", title: "POItemQtyHeight", hidden: true },
                    {field: "POItemStackable", title: "POItemStackable", hidden: true },
                    {field: "POItemLevelOfDensity", title: "POItemLevelOfDensity", hidden: true },
                    {field: "POItemCompLegalEntity", title: "POItemCompLegalEntity", hidden: true },
                    {field: "POItemCompAlphaCode", title: "POItemCompAlphaCode", hidden: true },
                    {field: "POItemNMFCSubClass", title: "POItemNMFCSubClass", hidden: true },
                    {field: "POItemUser1", title: "POItemUser1", hidden: true },
                    {field: "POItemUser2", title: "POItemUser2", hidden: true },
                    {field: "POItemUser3", title: "POItemUser3", hidden: true },
                    {field: "POItemUser4", title: "POItemUser4", hidden: true },
                    {field: "POItemCommCode", title: "POItemCommCode", hidden: true },
                    {field: "POItemCustomerPO", title: "POItemCustomerPO", hidden: true },
                    {field: "POItemLocationCode", title: "POItemLocationCode", hidden: true }
                ]
            });
        }

        function getDDLVars() {
            ddlNatAcctNumbers002 = $("#ddlNatAcctNumbers").data("kendoDropDownList");
            ddlCompany002 = $("#ddlCompany").data("kendoDropDownList");
            ddlFrtType002 = $("#ddlFrtType").data("kendoDropDownList");
        }

         $(document).ready(function () {                   
             var PageMenuTab = <%=PageMenuTab%>;           
            
             if (control != 0){

                 $("#ddlNatAcctNumbers").kendoDropDownList({
                    dataTextField: "Name",
                    dataValueField: "Description",
                    autoWidth: true,
                    filter: "contains",
                    dataSource: {
                        serverFiltering: false,
                        transport: {
                            read: {
                                async: false,
                                url: "api/vLookupList/GetUserDynamicList/" + nglUserDynamicLists.NatAcctNumber,
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
                        error: function (xhr, textStatus, error) { ngl.showErrMsg("Get Nat Acct Numbers JSON Response Error",formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"), null);  this.cancelChanges(); }
                    },
                    select: function(e) {
                        var value = e.dataItem.Description;                       
                        if (ngl.isNullOrWhitespace(value)){ value = 0; } //Value must be int so if value is empty string or null set it to 0
                        pgCompNatNumberFltr = value;
                        if(blnFirstLoad === false){ refreshPOHdrGrid(); }                       
                    }
                 });

                 $("#ddlCompany").kendoDropDownList({
                     dataTextField: "Name",
                     dataValueField: "Description",
                     autoWidth: true,
                     filter: "contains",
                     dataSource: {
                         serverFiltering: false,
                         transport: {
                             read: {
                                 async: false,
                                 url: "api/vLookupList/GetUserDynamicList/" + nglUserDynamicLists.Comp,
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
                         error: function (xhr, textStatus, error) { ngl.showErrMsg("Get Company JSON Response Error",formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"), null);  this.cancelChanges(); }
                     },
                     select: function(e) {
                         var value = e.dataItem.Description;                       
                         if (ngl.isNullOrWhitespace(value)){ value = 0; } //Value must be int so if value is empty string or null set it to 0
                         pgCompNumberFltr = value;
                         if(blnFirstLoad === false){ refreshPOHdrGrid(); }                       
                     }
                 });           

                 $("#ddlFrtType").kendoDropDownList({
                     dataTextField: "Name",
                     dataValueField: "Control",
                     autoWidth: true,
                     filter: "contains",
                     dataSource: {
                         serverFiltering: false,
                         transport: {
                             read: {
                                 async: false,
                                 url: "api/vLookupList/GetStaticList/" + nglStaticLists.BookTransType,
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
                         error: function (xhr, textStatus, error) { ngl.showErrMsg("Get Frt Type JSON Response Error",formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"), null);  this.cancelChanges(); }
                     },
                     select: function(e) {
                         var value = e.dataItem.Control;                       
                         if (ngl.isNullOrWhitespace(value)){ value = 0; } //Value must be int so if value is empty string or null set it to 0
                         pgFrtTypeFltr = value;   
                         if(blnFirstLoad === false){ refreshPOHdrGrid(); }                  
                     }
                 }); 

                 $("#viewChangesGrid").kendoGrid({
                     autoBind: false,
                     noRecords: true,                 
                     resizable: true,
                     groupable: false,
                     sortable: true,
                     pageable: { pageSizes: [5, 10, 50, "all"] },
                     dataSource: {
                         serverSorting: true,
                         serverPaging: true,
                         pageSize: 10,
                         transport: { 
                             read: function(options) { 
                                 var s = new GenericResult();
                                 s.strField = selectedOrderNumber;
                                 s.intField1 = selectedOrderSequence;
                                 s.intField2 = selectedDefaultCustomer;                          
                                 $.ajax({ 
                                     url: 'api/POHdr/ViewImportChanges/' + s, 
                                     contentType: 'application/json; charset=utf-8', 
                                     dataType: 'json', 
                                     data: { filter: JSON.stringify(s) }, 
                                     headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                                     success: function(data) { 
                                         options.success(data); 
                                         if (data.Errors != null) { 
                                             if (data.StatusCode === 203){ ngl.showErrMsg("Authorization Timeout", data.Errors, null); } else { ngl.showErrMsg("Access Denied", data.Errors, null); } 
                                         } 
                                     }, 
                                     error: function(result) { options.error(result); } 
                                 }); 
                             }, 
                             parameterMap: function (options, operation) { return options; }
                         },  
                         schema: { 
                             data: "Data",  
                             total: "Count", 
                             model: { 
                                 id: "ID",
                                 fields: {                                       
                                     ID: { type: "integer", editable: false },
                                     FieldName: { type: "string", editable: false },
                                     Caption: { type: "string", editable: false },
                                     OriginalValue: { type: "string", editable: false },
                                     ModifiedValue: { type: "string", editable: false },
                                     ValueType: { type: "string", editable: false }
                                 }
                             }, 
                             errors: "Errors" 
                         }, 
                         error: function (xhr, textStatus, error) { ngl.showErrMsg("Get View Import Changes JSON Response Error",formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"), null);  this.cancelChanges(); } 
                     },
                     columns: [ 
                         { field: "ID", title: "ID", hidden: true }, 
                         { field: "FieldName", title: "Field Name", hidden: true }, 
                         { field: "Caption", title: "Field" },
                         { field: "ModifiedValue", title: "Modifications" },
                         { field: "OriginalValue", title: "Existing" },
                         { field: "ValueType", title: "Value Type", hidden: true },
                     ]
                 });

                 ////////////wndViewChanges///////////////////
                 wndViewChanges = $("#wndViewChanges").kendoWindow({
                     title: "View Import Changes",
                     modal: true,
                     visible: false,
                     //height: 400,
                     width: 500,
                     close: function(e) {
                         //clear values
                         pohdrGridSelectedRowDataItem = null;
                         selectedOrderNumber = "";
                         selectedOrderSequence = 0;
                         selectedDefaultCustomer = 0;
                         $('#viewChangesGrid').data('kendoGrid').dataSource.read();                       
                     }
                 }).data("kendoWindow");
        

                 getDDLVars();
                 getPageSettings(tPage, "POHdr", "OrderPrevFltr", false);
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