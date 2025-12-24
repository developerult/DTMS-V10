<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LaneDetail.aspx.cs" Inherits="DynamicsTMS365.LaneDetail" %>

<!DOCTYPE html>

<html>
    <head >
        <title>DTMS Lane Detail</title>
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
                var tObj = this;
                var tPage = this; 
                var tObjPG = this;      

        <% Response.Write(NGLOAuth2); %>

        
                <% Response.Write(PageCustomJS); %>

                //************* Action Menu Functions ********************
                function execActionClick(btn, proc){
                    if (btn.id == "btnRefresh") { refresh(); }
                    else if (btn.id == "btnResetCurrentUserConfig") { resetCurrentUserConfig(PageControl); }
                    else if (btn.id == "btnCalcLaneLatLongMiles") { RecalculateLatLongMiles(); }//Added By LVV on 6/17/20 for v-8.2.1.008 Task#202005151417 - Company/Warehouse Maintenance Changes
                    else { ngl.pgNavigation(btn.id, true); }// modified by RHR for v-8.5.2.006 on 12/22/2022 added page navigation menu method
                }

                
                /* Summary - Calls the REST method to recalculate the lattitude and longitude */
                function ConfirmRecalculateLatLongMiles(iRet) {
                    if (typeof (iRet) === 'undefined' || iRet === null || iRet === 0) { return; } //Chose the Cancel action
                    //Chose the Ok action        
                    //kendo.ui.progress(oLELaneGrid.element, true);
                    $.ajax({
                        type: 'GET',
                        url: 'api/LaneDetail/RecalculateLatLongMiles/0',
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                        success: function (data) {
                            //kendo.ui.progress(oLELaneGrid.element, false); 
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



                function refresh(){ 
                    if(wdgtepLaneDetailsEdit){ wdgtepLaneDetailsEdit.read(0); } 
                }
             
                //************* Call Back Functions **********************        
                function savePostPageSettingSuccessCallback(results){
                    //for now do nothing when we save the pk
                }
                function savePostPageSettingAjaxErrorCallback(xhr, textStatus, error){
                    //for now do nothing when we save the pk           
                }      

                function readLaneOrigCompSuccessCallback(data){                        
                    try {
                        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                            if (ngl.stringHasValue(data.Errors)) { ngl.showErrMsg("Origin Company DDL Failure", data.Errors, tPage); }
                            else {                        
                                if (data.Data != null) {                                                       
                                    var record = data.Data[0];
                                    if(!record) { return; }
                                    //update the informtion on the page.
                                    $.each(objepLaneDetailsDataFields, function (index, item) {
                                        var blnUpdate = false;
                                        var dataItem = '';
                                        switch (item.fieldName) {
                                            case 'LaneOrigName':
                                                dataItem = record['CompName'];
                                                blnUpdate = true;
                                                break;
                                            case 'LaneOrigAddress1':
                                                dataItem = record['CompStreetAddress1'];
                                                blnUpdate = true;
                                                break;
                                            case 'LaneOrigAddress2':
                                                dataItem = record['CompStreetAddress2'];
                                                blnUpdate = true;
                                                break;
                                            case 'LaneOrigAddress3':
                                                dataItem = record['CompStreetAddress3'];
                                                blnUpdate = true;
                                                break;
                                            case 'LaneOrigCity':
                                                dataItem = record['CompStreetCity'];
                                                blnUpdate = true;
                                                break;
                                            case 'LaneOrigState':
                                                dataItem = record['CompStreetState'];
                                                blnUpdate = true;
                                                break;
                                            case 'LaneOrigZip':
                                                dataItem = record['CompStreetZip'];
                                                blnUpdate = true;
                                                break;
                                            case 'LaneOrigCountry':
                                                dataItem = record['CompStreetCountry'];
                                                blnUpdate = true;
                                                break;
                                        }     
                                        if (blnUpdate == true) { updateWidget(dataItem, item.fieldTagID, item.fieldGroupSubType); }
                                    }); 	 
                                } else { ngl.showErrMsg("Origin Company DDL Failure", "No Data available", tPage); }
                            }
                        } 
                    } catch (err) { ngl.showErrMsg(err.name, err.message, tPage); }
                }
                function readLaneOrigCompAjaxErrorCallback(xhr, textStatus, error){ ngl.showErrMsg("Origin Company DDL Failure", formatAjaxJSONResponsMsgs(xhr, textStatus, error, 'Failed'), tPage); }

                function readLaneDestCompSuccessCallback(data){                        
                    try {
                        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                            if (ngl.stringHasValue(data.Errors)) { ngl.showErrMsg("Destination Company DDL Failure", data.Errors, tPage); }
                            else {                        
                                if (data.Data != null) {                                                       
                                    var record = data.Data[0];
                                    if(!record) { return; }
                                    //update the informtion on the page.
                                    $.each(objepLaneDetailsDataFields, function (index, item) {
                                        var blnUpdate = false;
                                        var dataItem = '';
                                        switch (item.fieldName) {
                                            case 'LaneDestName':
                                                dataItem = record['CompName'];
                                                blnUpdate = true;
                                                break;
                                            case 'LaneDestAddress1':
                                                dataItem = record['CompStreetAddress1'];
                                                blnUpdate = true;
                                                break;
                                            case 'LaneDestAddress2':
                                                dataItem = record['CompStreetAddress2'];
                                                blnUpdate = true;
                                                break;
                                            case 'LaneDestAddress3':
                                                dataItem = record['CompStreetAddress3'];
                                                blnUpdate = true;
                                                break;
                                            case 'LaneDestCity':
                                                dataItem = record['CompStreetCity'];
                                                blnUpdate = true;
                                                break;
                                            case 'LaneDestState':
                                                dataItem = record['CompStreetState'];
                                                blnUpdate = true;
                                                break;
                                            case 'LaneDestZip':
                                                dataItem = record['CompStreetZip'];
                                                blnUpdate = true;
                                                break;
                                            case 'LaneDestCountry':
                                                dataItem = record['CompStreetCountry'];
                                                blnUpdate = true;
                                                break;
                                        }     
                                        if (blnUpdate == true) { updateWidget(dataItem, item.fieldTagID, item.fieldGroupSubType); }
                                    }); 	 
                                } else { ngl.showErrMsg("Destination Company DDL Failure", "No Data available", tPage); }
                            }
                        } 
                    } catch (err) { ngl.showErrMsg(err.name, err.message, tPage); }
                }
                function readLaneDestCompAjaxErrorCallback(xhr, textStatus, error){ ngl.showErrMsg("Destination Company DDL Failure", formatAjaxJSONResponsMsgs(xhr, textStatus, error, 'Failed'), tPage); }


                //************* Page Functions ***************************
                var oLaneOrigCompList = null;
                var oLaneDestCompList = null;
                var oLaneLTTransTypeList = null;
                function NGLEDITOnPageListChanged(e,tList,source){
                    //note if we have more than one list on the page we need to process the correct one
                    if(!tList || !source) { return; }
                    var sID = tList.element[0].id;
                    var sName = source.GetFieldName(sID);
                    if (!sName) { return; }
                    if (sName === 'LaneOrigCompControl'){
                        oLaneOrigCompList = tList;
                        var item = $("#" + sID)
                        if (!item) { return; }
                        var sKey = item.data("kendoDropDownList").value();
                        return updateLaneOrigAddressInfo(sKey);
                    }
                    else if (sName === 'LaneDestCompControl'){
                        oLaneDestCompList = tList;
                        var item = $("#" + sID)
                        if (!item) { return; }
                        var sKey = item.data("kendoDropDownList").value();
                        return updateLaneDestAddressInfo(sKey);
                    }
             
                }

                function updateLaneOrigAddressInfo(key){           
                    var oCRUDCtrl = new nglRESTCRUDCtrl();
                    var blnRet = oCRUDCtrl.read("LaneDetail/GetCompany", key, tPage, "readLaneOrigCompSuccessCallback", "readLaneOrigCompAjaxErrorCallback",tPage);
                    return true;
                }

                function updateLaneDestAddressInfo(key){           
                    var oCRUDCtrl = new nglRESTCRUDCtrl();
                    var blnRet = oCRUDCtrl.read("LaneDetail/GetCompany", key, tPage, "readLaneDestCompSuccessCallback", "readLaneDestCompAjaxErrorCallback",tPage);
                    return true;
                }

                function updateWidget(dataItem, fieldTagID, fieldGroupSubType){           
                    if (fieldGroupSubType === nglGroupSubTypeEnum.Checkbox) {
                        if (dataItem !== true) {dataItem = false;}
                        $("#" + fieldTagID).prop('checked', dataItem); 
                    } else if (fieldGroupSubType === nglGroupSubTypeEnum.kendoSwitch) {
                        if (dataItem !== true) {dataItem = false;}
                        $("#" + fieldTagID).data("kendoSwitch").check(dataItem);
                    } else if (fieldGroupSubType === nglGroupSubTypeEnum.NumericTextBox){
                        $("#" + fieldTagID).data("kendoNumericTextBox").value(dataItem);
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
                        $("#" + fieldTagID).data("kendoMaskedTextBox").value(dataItem);
                    } else {
                        $("#" + fieldTagID).value(dataItem);
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
            
                    }
                    var PageReadyJS = <%=PageReadyJS%>;
                    menuTreeHighlightPage(); //must be called after PageReadyJS
                    var divWait = $("#h1Wait");
                    if (typeof(divWait) !== 'undefined') { divWait.hide(); }
                });
            </script>
            <style>
                .k-grid tbody tr td { overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }                
                .k-tooltip{ max-height: 500px; max-width: 450px; overflow-y: auto; }              
                .k-grid tbody .k-grid-Edit { min-width: 0; }                         
                .k-grid tbody .k-grid-Edit .k-icon { margin: 0; }       
            </style>
        </div>
    </body>
</html>
