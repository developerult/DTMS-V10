<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NGLAccounting.aspx.cs" Inherits="DynamicsTMS365.NGLAccounting" %>

<!DOCTYPE html>

<html>
    <head >
        <title>DTMS NGL Accounting</title>
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
                           
                            <div style="float: left;">                                       
                                <table class="tblResponsive">                                            
                                    <tr><td class="tblResponsive-top">CNS</td></tr>                                           
                                    <tr><td class="tblResponsive-top"><input id="txtCNSFltr" style="width: 250px;" /></td></tr>                                        
                                </table>                                    
                            </div>                                
                            <div class="tblResponsive-wrap" style="float: none;">&nbsp;</div>

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


            <% Response.WriteFile("~/Views/SelectContactWindow.html"); %>
            <% Response.Write(PageTemplates); %>

            <%-- HelpWindow needs to go after AuthNotification because it uses the notification object created there --%>
            <% Response.Write(AuthLoginNotificationHTML); %>
            <% Response.WriteFile("~/Views/HelpWindow.html"); %>
            <script>
                <% Response.Write(ADALPropertiesjs); %>
                var PageControl = '<%=PageControl%>'; 
                var oConsSummaryGrid = null;
                var tObj = this;
                var tPage = this; 
                var tObjPG = this;             
       

        <% Response.Write(NGLOAuth2); %>

                     
                var pgCNSFltr = "";
                <% Response.Write(PageCustomJS); %>

                //~~~~~~~~~~SelectContactWndCtrl~~~~~~~~~~~~~~~~~~~
                var wndSelectContactWndCtrl = kendo.ui.Window;
                var wdgtSelectContactWndCtrl = new SelectContactWndCtrl()
                function wdgtSelectContactWndCtrlSaveCB(results){
                    try {
                        if (typeof (results) !== 'undefined' && ngl.isObject(results)) {                  
                                //update the informtion on the page.
                            $.each(objEditPPRWndDataFields, function (index, item) {
                                    var blnUpdate = false;
                                    var dataItem = '';
                                    switch (item.fieldName) {
                                        case 'BookCarrierContControl':                                           
                                            dataItem = results['ContactControl'];
                                            blnUpdate = true;
                                            break;
                                        case 'BookCarrierContact':
                                            dataItem = results['ContactName'];
                                            blnUpdate = true;
                                            break;
                                        case 'BookCarrierContactPhone':
                                            dataItem = results['ContactPhone'];
                                            blnUpdate = true;
                                            break;
                                    }     
                                    if (blnUpdate == true) { updateWidget(dataItem, item.fieldTagID, item.fieldGroupSubType, item.fieldVisible); }
                                }); 	                                                                                
                        } else{ ngl.showErrMsg("Carrier Contact Selection Failure", "", null); }
                    } catch (err) { ngl.showErrMsg(err.name, err.message, null); }
                }
                //~~~~~~~~~~End SelectContactWndCtrl~~~~~~~~~~~~~~~~


                //************* Action Menu Functions ********************
                function execActionClick(btn, proc){
                    if(btn.id == "btnAddPPR" ){ duplicatePPR(); } 
                    else if(btn.id == "btnRefresh" ){ refresh(); }  
                    else if(btn.id == "btnResetCurrentUserConfig"){ resetCurrentUserConfig(PageControl); }                      
                }

                function refresh(){ 
                    ngl.readDataSource(oConsSummaryGrid);
                    if(wdgtsumCNSTotalsSummary){ wdgtsumCNSTotalsSummary.read(0); } 
                }
             
                //************* Call Back Functions **********************        
                function savePostPageSettingSuccessCallback(results){ } //for now do nothing when we save the pk
                function savePostPageSettingAjaxErrorCallback(xhr, textStatus, error){ } //for now do nothing when we save the pk
                
                function consSummaryGridDataBoundCallBack(e,tGrid){
                    oConsSummaryGrid = tGrid;
                }

                var blnFirstLoadGridFltr = true;
                function consSummaryGridGetStringData(s)
                {           
                    blnFirstLoadGridFltr = false;
                    s.Data = pgCNSFltr;
                    return pgCNSFltr;
                }

                function duplicatePPRSuccessCallback (data) {  
                    try {
                        var blnSuccess = false;
                        var blnErrorShown = false;
                        var strValidationMsg = "";                                   
                        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {                                        
                            if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Create PPR Failure", data.Errors, null); }                                      
                            else {                                            
                                if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {                                               
                                    if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') { blnSuccess = true; refresh(); }                                            
                                }                                        
                            }                                   
                        }                                    
                        if (blnSuccess === false && blnErrorShown === false) {                                        
                            if (strValidationMsg.length < 1) { strValidationMsg = "Create PPR Failure"; }
                            ngl.showErrMsg("Create PPR Failure", strValidationMsg, null);                                   
                        }                               
                    } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                }

                function duplicatePPRAjaxErrorCallback (xhr, textStatus, error) {
                    var Msg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, 'Failed');
                    ngl.showErrMsg("Create PPR JSON Response Error", Msg, null);
                }

                var blnFirstLoad = true;
                function readGetPageSettingSuccessCallback(data) {
                    var dsUserPageSettings = null;                          
                    try {
                        var blnSuccess = false;
                        var blnErrorShown = false;                       
                        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {                          
                            if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Get User Page Settings Failure", data.Errors, null); }                          
                            else {                               
                                if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {                                   
                                    if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                        blnSuccess = true;
                                        dsUserPageSettings = data.Data[0]; 
                                        if(typeof (dsUserPageSettings) !== 'undefined' && dsUserPageSettings != null && dsUserPageSettings.value != undefined){
                                            var d = JSON.parse(dsUserPageSettings.value);            
                                            pgCNSFltr = d.Data; 
                                            $("#txtCNSFltr").data("kendoMaskedTextBox").value(pgCNSFltr);                                                                                                                                     
                                        }                                   
                                    }                              
                                }                            
                            }                        
                        }                      
                        if (blnSuccess === false && blnErrorShown === false) {  
                            var strValidationMsg = "If this is your first time on this page your settings will be saved for your next visit, if not please contact technical support if you continue to receive this message.";
                            ngl.showInfoNotification("Unable to Read Page Settings", strValidationMsg, null);  
                            pgCNSFltr = "";
                            $("#txtCNSFltr").data("kendoMaskedTextBox").value(pgCNSFltr);                            
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

             
                //************* Page Functions ***************************
                function duplicatePPR(){
                    if (!ngl.stringHasValue(pgCNSFltr)){ ngl.showErrMsg("Create PPR Failure", "CNS is required", null); return;} 
                    var oCRUDCtrl = new nglRESTCRUDCtrl();
                    var oChanges = { BookControl: 0, BookConsPrefix: pgCNSFltr};                            
                    var blnRet = oCRUDCtrl.update("NGLAccountingPPR/Post", oChanges, tObj, "duplicatePPRSuccessCallback", "duplicatePPRAjaxErrorCallback");                                                                    
                }

                function consSumGridEdit(e){
                    var item = this.dataItem($(e.currentTarget).closest("tr"));
                    if (typeof (item) !== 'undefined' && ngl.isObject(item)) {
                        if(!("CompNumber" in item)){ ngl.showErrMsg("Missing Required Field", "CompNumber is required", null); return; }
                        if(!("BookControl" in item)){ ngl.showErrMsg("Missing Required Field", "BookControl is required", null); return; }
                        var bookPK = item.BookControl;                           
                        if(item.CompNumber === 5000){                  
                            if (typeof(tPage["wdgtEditPPRWndDialog"]) !== 'undefined' && ngl.isObject(tPage["wdgtEditPPRWndDialog"])){   
                                tPage["wdgtEditPPRWndDialog"].read(bookPK);   
                            } else{ alert("wdgtEditPPRWndDialog is undefined - Check the CM configuration for EditPPRWnd"); }                                                                                                                          
                        } else{
                            if (typeof(tPage["wdgtEditNonPPRWndDialog"]) !== 'undefined' && ngl.isObject(tPage["wdgtEditNonPPRWndDialog"])){   
                                tPage["wdgtEditNonPPRWndDialog"].read(bookPK);   
                            } else{ alert("wdgtEditNonPPRWndDialog is undefined - Check the CM configuration for EditNonPPRWnd"); }                                 
                        } 
                    }             
                }

                  

                //************* CM Widget Page Functions ***************************
                var oCarrierList = null;
                var oBookFinAPGLList = null;
                var blnFirstLoad = true;
                function NGLPopupWindCtrlListChanged(e,tList,source){
                    //TODO LVV: FIGURE OUT HOW TO MAKE SURE THE CORRECT LISTS ARE BEING MODIFIED BECAUSE WE HAVE MULTIPLE POPUP WINDOWS WITH SAME DDL FIELDS -- VERIFY THIS WORKS (UNIQUE)
                    //note if we have more than one list on the page we need to process the correct one
                    if(!tList || !source) { return; }
                    var sID = tList.element[0].id;
                    var sName = source.GetFieldName(sID);
                    if (!sName) { return; }
                    if (sName === 'BookCarrierControl'){
                        oCarrierList = tList;
                        var item = $("#" + sID)
                        if (!item) { return; }
                        var sKey = item.data("kendoDropDownList").value();
                        if (blnFirstLoad === false){
                            wdgtSelectContactWndCtrl.eContactType = contactTypeEnum.Carrier;
                            wdgtSelectContactWndCtrl.FilterControl = sKey;
                            wdgtSelectContactWndCtrl.show();  
                        }
                        blnFirstLoad = false;
                    }
                    else if (sName === 'BookFinAPGLNumber'){
                        oBookFinAPGLList = tList;
                        var item = $("#" + sID)
                        if (!item) { return; }
                        var sKey = item.data("kendoDropDownList").value();                        
                        try {                                                                                                                                            
                            //update the informtion on the page.
                            $.each(objEditPPRWndDataFields, function (index, item) {
                                var blnUpdate = false;
                                var dataItem = '';
                                switch (item.fieldName) {
                                    case 'BookFinAPGLNumber':
                                        dataItem = sKey;
                                        blnUpdate = true;
                                        break;
                                }     
                                if (blnUpdate == true) { updateWidget(dataItem, item.fieldTagID, item.fieldGroupSubType, item.fieldVisible); }
                            });                                                                                                       
                        } catch (err) { ngl.showErrMsg(err.name, err.message, null); }
                    }           
                }

                function updateWidget(dataItem, fieldTagID, fieldGroupSubType, fieldVisible){   
                    if (fieldGroupSubType === nglGroupSubTypeEnum.Checkbox) {
                        if (dataItem !== true) {dataItem = false;}
                        $("#" + fieldTagID).prop('checked', dataItem); 
                    } else if (fieldGroupSubType === nglGroupSubTypeEnum.kendoSwitch) {
                        if (dataItem !== true) {dataItem = false;}
                        $("#" + fieldTagID).data("kendoSwitch").check(dataItem);
                    } else if (fieldGroupSubType === nglGroupSubTypeEnum.NumericTextBox){
                        if (fieldVisible === false){ $("#" + fieldTagID).val(dataItem); } else{ $("#" + fieldTagID).data("kendoNumericTextBox").value(dataItem); }                        
                    } else if (fieldGroupSubType === nglGroupSubTypeEnum.DatePicker) {
                        $("#" + fieldTagID).data("kendoDatePicker").value(dataItem);
                    } else if (fieldGroupSubType === nglGroupSubTypeEnum.DateTimePicker) {
                        $("#" + fieldTagID).data("kendoDateTimePicker").value(dataItem);
                    } else if (fieldGroupSubType === nglGroupSubTypeEnum.TimePicker) {
                        $("#" + fieldTagID).data("kendoTimePicker").value(dataItem); 
                    } else if (fieldGroupSubType === nglGroupSubTypeEnum.Editor) {
                        $("#" + fieldTagID).data("kendoEditor").value(dataItem);
                    } else if (fieldGroupSubType === nglGroupSubTypeEnum.ColorPicker) {
                        $("#" + fieldTagID).data("kendoColorPicker").value({ value: dataItem });
                    } else if (fieldGroupSubType === nglGroupSubTypeEnum.DropDownList) {
                        $("#" + fieldTagID).data("kendoDropDownList").value(dataItem);
                    } else if (fieldGroupSubType === nglGroupSubTypeEnum.MaskedTextBox) {
                        if (fieldVisible === false){ $("#" + fieldTagID).val(dataItem); } else{ $("#" + fieldTagID).data("kendoMaskedTextBox").value(dataItem); }
                    } else {
                        $("#" + fieldTagID).value(dataItem);
                    }
                }

                function EditPPRWndCB(oResults){          
                    if(oResults.widget.sNGLCtrlName === "wdgtEditPPRWndDialog" && oResults.source === "saveSuccessCallback"){
                        refresh();
                        oResults.widget.executeActions("close");
                    }          
                }

                function EditNonPPRWndCB(oResults){          
                    if(oResults.widget.sNGLCtrlName === "wdgtEditNonPPRWndDialog" && oResults.source === "saveSuccessCallback"){
                        refresh();
                        oResults.widget.executeActions("close");
                    }          
                }


                $(function () {
                    //wire focus of all numerictextbox widgets on the page
                    $("input[type=number]").bind("focus", function () {
                        var input = $(this);
                        clearTimeout(input.data("selectTimeId")); //stop started time out if any
                        var selectTimeId = setTimeout(function(){ input.select(); });
                        input.data("selectTimeId", selectTimeId);
                    }).blur(function(e) {
                        clearTimeout($(this).data("selectTimeId")); //stop started timeout
                    });
                })

                $(document).ready(function () {
                    var PageMenuTab = <%=PageMenuTab%>;
                                 
                    if (control != 0){     
                        

                        $("#txtCNSFltr").kendoMaskedTextBox({
                            change: function() {
                                var value = this.value();
                                pgCNSFltr = value;
                                if(blnFirstLoadGridFltr === false){ refresh(); }
                            }
                        });
                        
                        ////////////SelectContactWndCtrl///////////////////
                        wdgtSelectContactWndCtrl = new SelectContactWndCtrl(); 
                        wdgtSelectContactWndCtrl.loadDefaults(wndSelectContactWndCtrl, wdgtSelectContactWndCtrlSaveCB);   


                        getPageSettings(tPage, "NGLAccountingConsSum", "consSummaryGridFilter", false);  

                    }
                    var PageReadyJS = <%=PageReadyJS%>;
                    menuTreeHighlightPage(); //must be called after PageReadyJS
                    var divWait = $("#h1Wait");
                    if (typeof(divWait) !== 'undefined') { divWait.hide(); }
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
