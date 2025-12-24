<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Scheduler.aspx.cs" Inherits="DynamicsTMS365.Scheduler" %>

<!DOCTYPE html>
<html>
<head>
    <title>Scheduler</title>
    <%=cssReference%>
    <style>
        html,
        body {
            height: 100%;
            margin: 0;
            padding: 0;
        }

        html {
            font-size: 12px;
            font-family: Arial, Helvetica, sans-serif;
            overflow: hidden;
        }
    </style>

</head>
<body>
    <%=jssplitter2Scripts%>
    <%=sWaitMessage%>

    <script src="Scripts/NGL/v-8.5.4.006/windowconfig.js"></script>
    <!--added by SRP on 3/8/2018 For Editing KendoWindow Configuration from Javascript -->

    <%--<div id="example" class="ui-container">--%>
    <div id="example" style="height: 100%; width: 100%; margin-top: 2px;">
        <%--<div id="vertical" class="ui-vertical-container">--%>
        <div id="vertical" style="height: 98%; width: 98%;">
            <%--Action Menu TabStrip--%>
            <div id="menu-pane" style="height: 100%; width: 100%; background-color: white;">
                <div id="tab" class="menuBarTab"></div>
            </div>
            <div id="top-pane">
                <%--<div id="horizontal" class="ui-horizontal-container">--%>
                <div id="horizontal" style="height: 100%; width: 100%;">
                    <div id="left-pane">
                        <div class="pane-content">
                            <% Response.Write(MenuControl); %>
                            <div id="menuTree"></div>
                        </div>
                    </div>
                    <div id="center-pane" style="margin-right: 5px;">
                        <% Response.Write(PageErrorsOrWarnings); %>
                        <!-- Begin Page Content -->

                        <%--Message--%>
                        <div class="MTop"></div>

                        <!-- Search Appointments  -->
                        <div id="filterDiv" style="display: none;">
                            <div id="id260" style="margin-top: 1px; float: left; width: auto; height: 50px;">
                                <div id="ParHeader" class="OpenOrders" style="height: 50px;">
                                    <div id="Parwrapper">
                                        <%--Filters--%>
                                        <span>
                                            <label for="ddlAppointmentFilters">&nbsp;</label>
                                            <input id="ddlAppointmentFilters" />
                                            <span id="spAppointmentFilterDdl">
                                                <input id="ddlAppointmentBoundFilter" /></span>
                                            <span id="spAppointmentFilterText">
                                                <input id="txtAppointmentFilterVal" /></span>
                                            <span id="spAppointmentFilterButtons"><a id="btnAppointmentFilter"></a><a id="btnAppointmentClearFilter"></a></span>
                                        </span>
                                        <input id="txtAppointmentSortDirection" type="hidden" />
                                        <input id="txtAppointmentSortField" type="hidden" />
                                    </div>
                                </div>
                            </div>
                            <%-- right:10px; --%>
                            <div id="apptGridContainer" style="top: 65px; left: 325px; width: 50%; height: auto; position: absolute; z-index: 100;">
                                <div id="apptGridContainerHeader" class="k-block">Click here to move</div>
                                <div id="gridAppt"></div>
                            </div>
                        </div>

                        <%-- Search Orders --%>
                        <div id="filterDivOrders" style="display: none;">
                            <div id="id2601" style="margin-top: 1px; float: left; width: auto; height: 50px;">
                                <%--Filters--%>
                                <span>
                                    <label for="ddlOrdFilters">&nbsp;</label>
                                    <input id="ddlOrdFilters" />
                                    <span id="spOrdFilterText">
                                        <input id="txtOrdFilterVal" /></span>
                                    <span id="spOrdFilterDates">
                                        <label for="dpOrdFilterFrom">From:</label>
                                        <input id="dpOrdFilterFrom" />
                                        <label for="dpOrdFilterTo">To:</label>
                                        <input id="dpOrdFilterTo" />
                                    </span>
                                    <span id="spOrdFilterButtons"><a id="btnOrdFilter"></a><a id="btnOrdClearFilter"></a></span>
                                </span>
                                <input id="txtOrdSortDirection" type="hidden" />
                                <input id="ApptCountrol" type="hidden" />
                            </div>
                            <div id="orderGridContainer" style="top: 65px; left: 325px; width: 50%; height: auto; position: absolute; z-index: 100;">
                                <div id="orderGridContainerHeader" class="k-block">Click here to move</div>
                                <div id="grid"></div>
                            </div>
                        </div>
                        <%--                        <div style="margin-top: 1px; float:left;"><input id='ddlwarehouse' style='width:300px'/> <button id='allDocks' class='k-button actionBarButton'>Display Docks</button><li id='submenu'></li> </div>--%>
                        <%--scheduler--%>
                        <div id="schedulerDiv" <%--style="height:calc(100vh - 200px); overflow:hidden; margin-left:2px;"--%> style="float: none; clear: left;">
                            <%--Modified By LVV for 8.3.0.001 on 8/18/20 - Task #202007231832 - Warehouse Scheduler put Action Menu in content management--%>
                            <div>
                                <input id='ddlwarehouse' style='width: 300px' />
                                <button id='allDocks' class='k-button actionBarButton'>Display Docks</button><li id='submenu'></li>
                            </div>
                            <div id="scheduler"></div>
                            <%-- modified by RHR for v-8.5.4.003 changed width from 9% to 100% not clear where 9% came from --%>
                            <ul id="contextMenu" style="width: 100%;"></ul>
                        </div>

                        <!-- End Page Content -->
                    </div>
                </div>
            </div>
            <%-- <div id="bottom-pane" class="k-block ui-horizontal-container">--%>
            <div id="bottom-pane" class="k-block" style="height: 100%; width: 100%;">
                <div class="pane-content">
                    <% Response.Write(PageFooterHTML); %>
                </div>
            </div>
        </div>

        <% Response.WriteFile("~/Views/SchedulerAddEditWindow.html"); %>

        <% Response.Write(PageTemplates); %>

        <%-- HelpWindow needs to go after AuthNotification because it uses the notification object created there --%>
        <% Response.Write(AuthLoginNotificationHTML); %>
        <% Response.WriteFile("~/Views/HelpWindow.html"); %>
        <script type="text/javascript">
        <% Response.Write(ADALPropertiesjs); %>

            var PageControl = '<%=PageControl%>';
            var tObj = this;
            var tPage = this;


        <% Response.Write(NGLOAuth2); %>



        <% Response.Write(PageCustomJS); %>

            //************* Page Variables **************************  
            var dsWarehouse = kendo.data.DataSource;
            var dsAppointmentGrid = kendo.data.DataSource;
            var dsOrdersGrid = kendo.data.DataSource;
            var dsOrdersWndGrid = kendo.data.DataSource;
            var dsApptTrackingFieldsGrid = kendo.data.DataSource;
            var dsApptUserFieldsGrid = kendo.data.DataSource;
            var res = [];
            var CompControl;
            var TrackingFieldsUserFieldsApptControl;
            var cllAPI = false;
            var allDockDoorsSettingsArray = [];
            var dsAllDockDoorsApptTimeSettings = [];
            var Vstart = kendo.date.addDays(kendo.date.today(), -14); //kendo.date.today();
            var Vend = kendo.date.addDays(kendo.date.today(), +14); //kendo.date.today();

            //var Vstart = kendo.date.today();
            //var Vend =  kendo.date.today();

            //************* Action Menu Functions ***************
            //Added By LVV for 8.3.0.001 on 8/18/20 - Task #202007231832 - Warehouse Scheduler put Action Menu in content management
            function execActionClick(btn, proc) {
                if (btn.id == "btnFindMyAppt") { btnFindMyAppt_Click(); }
                else if (btn.id == "btnSearchOrders") { btnSearchOrders_Click(); }
                else if (btn.id == "btnManageScheduler") { btnManageScheduler_Click(); }
                else if (btn.id == "btnRefresh") { refresh(); }
                else if (btn.id == "btnResetCurrentUserConfig") {
                    //resetCurrentUserConfig(PageControl);
                    //Note - this is not the normal behavior of this button. It has been hijaked via instructions from Rob 11/4/20
                    var h = new UserPageSetting();
                    h.UserPSPageControl = 38;
                    h.UserPSName = 'SchedulerPage';
                    $.ajax({
                        async: false,
                        type: "POST",
                        url: "api/UserPageSetting/ResetSpecificUserPageSetting",
                        contentType: "application/json; charset=utf-8",
                        dataType: 'json',
                        data: JSON.stringify(h),
                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                        success: function (data) {
                            try {
                                var blnSuccess = false;
                                var blnErrorShown = false;
                                var strValidationMsg = "";
                                if (typeof (data) != 'undefined' && ngl.isObject(data)) {
                                    if (typeof (data.Errors) != 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                        blnErrorShown = true;
                                        ngl.showErrMsg("Reset Specific User Page Setting Failure", data.Errors, null);
                                    }
                                    else {
                                        if (typeof (data.Data) != 'undefined' && ngl.isArray(data.Data)) {
                                            if (data.Data.length > 0 && typeof (data.Data[0]) != 'undefined') {
                                                blnSuccess = true;
                                                document.location = oredirectUri;
                                            }
                                        }
                                    }
                                }
                                if (blnSuccess === false && blnErrorShown === false) {
                                    if (strValidationMsg.length < 1) { strValidationMsg = "Reset Specific User Page Setting Failure"; }
                                    ngl.showErrMsg("Reset Specific User Page Setting Failure", strValidationMsg, null);
                                }
                            } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                        },
                        error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Delete Data Failure"); ngl.showErrMsg("Reset Specific User Page Setting Failure", sMsg, null); }
                    });
                }
            }

            //Added By LVV for 8.3.0.001 on 8/18/20 - Task #202007231832 - Warehouse Scheduler put Action Menu in content management
            function refresh() { $("#scheduler").data("kendoScheduler").dataSource.read(); }

            var iLastHeightFactor = 45;
            //Added by RHR for v=8.3.0.002 on 11/18/2020 heightFactor must be a positive integer or null
            function resizeSWidget(heightFactor) {
                var swidget = $("#scheduler").data("kendoScheduler");
                if (typeof (heightFactor) == 'undefined' || heightFactor == null || isNaN(heightFactor) == true) {
                    heightFactor = 45;
                }
                iLastHeightFactor = heightFactor;
                var height = parseInt($('#center-pane').css('height')) - parseInt(heightFactor);
                // Size the widget based on the parent height minus a factor.
                swidget.element.height(height);
                swidget.resize(true);
            }

            //Modified By LVV for 8.3.0.001 on 8/18/20 - Task #202007231832 - Warehouse Scheduler put Action Menu in content management
            function btnFindMyAppt_Click() {
                //removed by RHR for v=8.3.0.002 we now call resizeSWidget
                //var swidget = $("#scheduler").data("kendoScheduler");
                //var height = parseInt($('#center-pane').css('height'))-45; // default
                if ($("#filterDiv").is(":visible")) {
                    $("#filterDiv").hide();
                    $("#schedulerDiv").css("margin-top", "1%");
                    //Modified by RHR for v-8.3.0.001 on 09/27/2020 fixed issue where scheduler variable 
                    //does not always referene the correct kendoScheduler object creating an undefined element
                    //now we use a local variable swidget
                    // old code  scheduler.element.height(parseInt($('#center-pane').css('height'))-30);  
                    // old code scheduler.resize(true); 
                    //removed by RHR for v=8.3.0.002 we now call resizeSWidget
                    //swidget.element.height(height);
                    //swidget.resize(true);
                    resizeSWidget(null); //use default when popup is hidden
                } else {
                    $("#filterDiv").show();
                    $("#filterDivOrders").hide();
                    //$("#schedulerDiv").css("margin-top","9.5%");                                
                    $("#schedulerDiv").css("margin-top", "60px");
                    $("#ddlAppointmentFilters").data("kendoDropDownList").value("ApptNumber");
                    $("#txtAppointmentFilterVal").data("kendoMaskedTextBox").value("");
                    $("#ddlAppointmentBoundFilter").data("kendoDropDownList").value(1);
                    $("#spAppointmentFilterDdl").hide();
                    $("#spAppointmentFilterText").show();
                    $("#spAppointmentFilterButtons").show();
                    //$("#gridAppt").data("kendoGrid").dataSource.data([]);
                    $("#gridAppt").data("kendoGrid").dataSource.read();
                    //scheduler.element.height(parseInt($('#center-pane').css('height'))-(parseInt($('#filterDiv').css('height'))+10));
                    //Modified by RHR for v-8.3.0.001 on 09/27/2020 fixed issue where scheduler variable 
                    //does not always referene the correct kendoScheduler object creating an undefined element
                    //now we use a local variable swidget
                    // old code  scheduler.element.height(parseInt($('#center-pane').css('height'))-70);  
                    // old code scheduler.resize(true);     
                    //removed by RHR for v=8.3.0.002 we now call resizeSWidget                
                    //height = parseInt($('#center-pane').css('height'))-90;
                    //// Size the widget to take the whole view.
                    //swidget.element.height(height);
                    //swidget.resize(true);                
                    resizeSWidget(90); //use factor of 90 when the popup is open
                }
            }

            //Modified By LVV for 8.3.0.001 on 8/18/20 - Task #202007231832 - Warehouse Scheduler put Action Menu in content management
            function btnSearchOrders_Click() {
                //removed by RHR for v=8.3.0.002 we now call resizeSWidget 
                //var swidget = $("#scheduler").data("kendoScheduler");
                //var height = parseInt($('#center-pane').css('height'))-45; // default
                if ($("#filterDivOrders").is(":visible")) {
                    $("#filterDivOrders").hide();
                    $("#schedulerDiv").css("margin-top", "1%");
                    //Modified by RHR for v-8.3.0.001 on 09/27/2020 fixed issue where scheduler variable 
                    //does not always referene the correct kendoScheduler object creating an undefined element
                    //now we use a local variable swidget
                    // old code  scheduler.element.height(parseInt($('#center-pane').css('height'))-30);  
                    // old code scheduler.resize(true); 
                    //removed by RHR for v=8.3.0.002 we now call resizeSWidget
                    //swidget.element.height(height);
                    //swidget.resize(true);
                    resizeSWidget(null); //use default factor
                } else {
                    $("#filterDivOrders").show();
                    $("#filterDiv").hide();
                    //$("#schedulerDiv").css("margin-top","9.5%");
                    $("#schedulerDiv").css("margin-top", "60px");
                    $("#ddlOrdFilters").data("kendoDropDownList").value("CNSNumber");
                    $("#txtOrdFilterVal").data("kendoMaskedTextBox").value("");
                    $("#dpOrdFilterFrom").data("kendoDatePicker").value(Vstart);
                    $("#dpOrdFilterTo").data("kendoDatePicker").value(Vend);
                    $("#spOrdFilterText").show();
                    $("#spOrdFilterButtons").show();
                    $("#grid").data("kendoGrid").dataSource.read();


                    //scheduler.element.height(parseInt($('#center-pane').css('height'))-(parseInt($('#filterDivOrders').css('height'))+10));
                    //Modified by RHR for v-8.3.0.001 on 09/27/2020 fixed issue where scheduler variable 
                    //does not always referene the correct kendoScheduler object creating an undefined element
                    //now we use a local variable swidget
                    // old code  scheduler.element.height(parseInt($('#center-pane').css('height'))-70);
                    // old code scheduler.resize(true);                 
                    //removed by RHR for v=8.3.0.002 we now call resizeSWidget
                    //height = parseInt($('#center-pane').css('height'))-90;
                    //// Size the widget to take the whole view.
                    //swidget.element.height(height);
                    //swidget.resize(true);   
                    //swidget.element.height(height);
                    //swidget.resize(true);
                    resizeSWidget(90); //use 90 factor when popup is visible

                }
            }



            //Created by LVV on 11/1/2019
            function generateEndTimeTemplate(strEndTime) {
                var res = strEndTime.split(",");
                var d = new Date(res[0]);
                var t = kendo.toString(d ? d : "", 'HH:mm');
                return t
            }

            //Make the Appt DIV element draggagle:
            dragAppts(document.getElementById("apptGridContainer"));

            function dragAppts(elmnt) {
                var pos1 = 0, pos2 = 0, pos3 = 0, pos4 = 0;
                if (document.getElementById("apptGridContainerHeader")) {
                    /* if present, the header is where you move the DIV from:*/
                    document.getElementById("apptGridContainerHeader").onmousedown = dragMouseDown;
                } else {
                    /* otherwise, move the DIV from anywhere inside the DIV:*/
                    elmnt.onmousedown = dragMouseDown;
                }

                function dragMouseDown(e) {
                    e = e || window.event;
                    e.preventDefault();
                    // get the mouse cursor position at startup:
                    pos3 = e.clientX;
                    pos4 = e.clientY;
                    document.onmouseup = closeDragElement;
                    // call a function whenever the cursor moves:
                    document.onmousemove = elementDrag;
                }

                function elementDrag(e) {
                    e = e || window.event;
                    e.preventDefault();
                    // calculate the new cursor position:
                    pos1 = pos3 - e.clientX;
                    pos2 = pos4 - e.clientY;
                    pos3 = e.clientX;
                    pos4 = e.clientY;
                    // set the element's new position:
                    var iTop = elmnt.offsetTop - pos2;
                    if (iTop < 5) { iTop = 5; }
                    elmnt.style.top = iTop + "px";
                    var iLeft = elmnt.offsetLeft - pos1;
                    if (iLeft < 5) { iLeft = 5; }
                    elmnt.style.left = iLeft + "px";
                }

                function closeDragElement() {
                    /* stop moving when mouse button is released:*/
                    document.onmouseup = null;
                    document.onmousemove = null;
                }
            }

            //Make the order DIV element draggagle:
            dragOrders(document.getElementById("orderGridContainer"));

            function dragOrders(elmnt) {
                var pos1 = 0, pos2 = 0, pos3 = 0, pos4 = 0;
                if (document.getElementById("orderGridContainerHeader")) {
                    /* if present, the header is where you move the DIV from:*/
                    document.getElementById("orderGridContainerHeader").onmousedown = dragMouseDown;
                } else {
                    /* otherwise, move the DIV from anywhere inside the DIV:*/
                    elmnt.onmousedown = dragMouseDown;
                }

                function dragMouseDown(e) {
                    e = e || window.event;
                    e.preventDefault();
                    // get the mouse cursor position at startup:
                    pos3 = e.clientX;
                    pos4 = e.clientY;
                    document.onmouseup = closeDragElement;
                    // call a function whenever the cursor moves:
                    document.onmousemove = elementDrag;
                }

                function elementDrag(e) {
                    e = e || window.event;
                    e.preventDefault();
                    // calculate the new cursor position:
                    pos1 = pos3 - e.clientX;
                    pos2 = pos4 - e.clientY;
                    pos3 = e.clientX;
                    pos4 = e.clientY;
                    // set the element's new position:
                    var iTop = elmnt.offsetTop - pos2;
                    if (iTop < 5) { iTop = 5; }
                    elmnt.style.top = iTop + "px";
                    var iLeft = elmnt.offsetLeft - pos1;
                    if (iLeft < 5) { iLeft = 5; }
                    elmnt.style.left = iLeft + "px";
                }

                function closeDragElement() {
                    /* stop moving when mouse button is released:*/
                    document.onmouseup = null;
                    document.onmousemove = null;
                }
            }

            //Added By LVV 2/28/20 - Added logic so when you click this button to go to Manage Schedule it jump to the warehouse from this page                      
            function btnManageScheduler_Click() {
                var userSettings = {};
                userSettings.CompanyID = $("#ddlwarehouse").val();
                var UserPageSettingsData = new PageSettingModel();
                UserPageSettingsData.name = "ManageSchedulePage";
                UserPageSettingsData.value = JSON.stringify(userSettings);
                $.ajax({
                    url: '/api/ManageSchedule/PostPageSetting/',
                    type: 'Post',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    data: JSON.stringify(UserPageSettingsData),
                    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                    success: function (data) {
                        try {
                            var blnSuccess = false;
                            var blnErrorShown = false;
                            var strValidationMsg = "";
                            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Save User Page Setting Failure", data.Errors, null); }
                                window.location.assign("/ManageSchedule.aspx");
                            }
                        } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                    },
                    error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure"); ngl.showErrMsg("Save User Page Setting Failure", sMsg, null); window.location.assign("/ManageSchedule.aspx"); }
                });
            }


            $(document).ready(function () {
                var PageMenuTab = <%=PageMenuTab%>; //Added By LVV for 8.3.0.001 on 8/18/20 - Task #202007231832 - Warehouse Scheduler put Action Menu in content management           

                if (control != 0) {
                    setTimeout(function () {
                        //add code here to load screen specific information this is only visible when a user is authenticated

                        //ALERTS
                        setTimeout(getAlerts, 60000);

                        $("#tabstrip").kendoTabStrip({ animation: { open: { effects: "fadeIn" } } });

                        ////////////Appointment Filters///////////////////
                        var AMSFilterData = [
                            { text: "", value: "None" },
                            { text: "Appt Number", value: "ApptNumber" },
                            { text: "Order Number", value: "OrdNumber" },
                            { text: "Pro Number", value: "ProNumner" },
                            { text: "PO Number", value: "PoNumber" },
                            { text: "CNS", value: "CNS" },
                            { text: "SHID", value: "SHID" },
                        ];

                        var AMSFilterDataBound = [
                            { text: "Inbound", value: 1 },
                            { text: "Outbound", value: 0 },
                        ];

                        $("#ddlAppointmentBoundFilter").kendoDropDownList({
                            dataTextField: "text",
                            dataValueField: "value",
                            placeholder: "select",
                            dataSource: AMSFilterDataBound,
                        });

                        $("#ddlAppointmentFilters").kendoDropDownList({
                            dataTextField: "text",
                            dataValueField: "value",
                            placeholder: "select",
                            dataSource: AMSFilterData,
                            select: function (e) {
                                var name = e.dataItem.text;
                                var val = e.dataItem.value;
                                $("#txtAppointmentFilterVal").data("kendoMaskedTextBox").value("");
                                $("#ddlAppointmentBoundFilter").data("kendoDropDownList").value(1);
                                switch (val) {
                                    case "None":
                                        $("#spAppointmentFilterDdl").hide(); $("#spAppointmentFilterText").hide(); $("#spAppointmentFilterButtons").hide();
                                        break;
                                    case "ApptNumber":
                                        $("#spAppointmentFilterDdl").hide(); $("#spAppointmentFilterText").show(); $("#spAppointmentFilterButtons").show();
                                        break;
                                    default:
                                        $("#spAppointmentFilterDdl").show(); $("#spAppointmentFilterText").show(); $("#spAppointmentFilterButtons").show();
                                        break;
                                }
                            }
                        });

                        $("#txtAppointmentFilterVal").kendoMaskedTextBox();

                        $("#btnAppointmentFilter").kendoButton({
                            icon: "filter",
                            click: function (e) {
                                var ddlvalue = $("#ddlAppointmentFilters").data("kendoDropDownList").dataItem().value;
                                if (ddlvalue == "ApptNumber" && $("#txtAppointmentFilterVal").data("kendoMaskedTextBox").value() != "") {
                                    if ($("#txtAppointmentFilterVal").data("kendoMaskedTextBox").value().match(/^\d+$/)) {
                                        $("#gridAppt").data("kendoGrid").dataSource.read();
                                    } else { ngl.showWarningMsg("Appt Number Filter value should be number.!", ""); $("#txtAppointmentFilterVal").focus(); }
                                } else if ($("#txtAppointmentFilterVal").data("kendoMaskedTextBox").value() == "") {
                                    ngl.showWarningMsg("Filter value cannot be empty.!", "")
                                    $("#txtAppointmentFilterVal").focus();
                                } else { $("#gridAppt").data("kendoGrid").dataSource.read(); }
                            }
                        });

                        $("#btnAppointmentClearFilter").kendoButton({
                            icon: "filter-clear",
                            click: function (e) {
                                var dropdownlist = $("#ddlAppointmentFilters").data("kendoDropDownList");
                                dropdownlist.select(0);
                                dropdownlist.trigger("change");
                                $("#txtAppointmentFilterVal").data("kendoMaskedTextBox").value("");
                                $("#ddlAppointmentBoundFilter").data("kendoDropDownList").value(1);
                                $("#spAppointmentFilterDdl").hide();
                                $("#spAppointmentFilterText").hide();
                                $("#spAppointmentFilterButtons").hide();
                                $("#gridAppt").data("kendoGrid").dataSource.data([]);
                            }
                        });

                        $("#txtAppointmentFilterVal").data("kendoMaskedTextBox").value("");
                        $("#ddlAppointmentBoundFilter").data("kendoDropDownList").value(1);
                        $("#spAppointmentFilterDdl").hide();
                        $("#spAppointmentFilterText").hide();
                        $("#spAppointmentFilterButtons").hide();

                        ////////////Orders Filters///////////////////
                        var AMSOrderFilterData = [
                            { text: "All", value: "None" },
                            { text: "CNS Number", value: "CNSNumber" },
                            { text: "Order Number", value: "OrdNumber" },
                            { text: "Pro Number", value: "ProNumner" },
                            { text: "PO Number", value: "PoNumber" },
                            { text: "Carrier", value: "Carrier" },
                            { text: "SHID", value: "SHID" }
                        ];

                        $("#ddlOrdFilters").kendoDropDownList({
                            dataTextField: "text",
                            dataValueField: "value",
                            placeholder: "select",
                            dataSource: AMSOrderFilterData,
                            select: function (e) {
                                var name = e.dataItem.text;
                                var val = e.dataItem.value;
                                console.log(val);
                                $("#txtOrdFilterVal").data("kendoMaskedTextBox").value("");
                                switch (val) {
                                    case "None":
                                        /*$("#spOrdFilterText").hide(); $("#spOrdFilterDates").hide(); $("#spOrdFilterButtons").hide();*/
                                        $("#spOrdFilterText").hide(); $("#spOrdFilterDates").show(); $("#spOrdFilterButtons").show();
                                        break;
                                    default:
                                        $("#spOrdFilterText").show(); $("#spOrdFilterDates").show(); $("#spOrdFilterButtons").show();
                                        break;
                                }
                            }
                        });

                        $("#txtOrdFilterVal").kendoMaskedTextBox();
                        $("#dpOrdFilterFrom").kendoDatePicker({
                            change: function () {
                                Vstart = this.value();
                                ////console.log(" Vstart changed to " + Vstart);
                            }
                        });
                        $("#dpOrdFilterTo").kendoDatePicker({
                            change: function () {
                                Vend = this.value();
                                console.log(" Vend changed to " + Vend);
                            }
                        });

                        $("#btnOrdFilter").kendoButton({
                            icon: "filter",
                            click: function (e) {
                                var ddlvalue = $("#ddlOrdFilters").data("kendoDropDownList").dataItem().value;
                                ////console.log(ddlvalue);
                                if (ddlvalue == "None") {
                                    $("#grid").data("kendoGrid").dataSource.read();
                                } else if (ddlvalue == "Carrier" && $("#txtOrdFilterVal").data("kendoMaskedTextBox").value() != "") {
                                    ///    Modified by RHR for v- 8.2.0.118 on 9/9/2019
                                    ///    so Carrier does not need to be  number
                                    ///    changed carriercontrol filter to carrier filter as a string we now 
                                    ///    support looking up the carrier name or the carrier number and find 
                                    ///    the first match
                                    //if ($("#txtOrdFilterVal").data("kendoMaskedTextBox").value().match(/^\d+$/)){
                                    $("#grid").data("kendoGrid").dataSource.read();
                                    //}else{ ngl.showWarningMsg("Carrier Filter value should be number.!",""); $("#txtOrdFilterVal").focus(); }
                                } else if ($("#txtOrdFilterVal").data("kendoMaskedTextBox").value() == "") {
                                    ngl.showWarningMsg("Filter value cannot be empty.!", "")
                                    $("#txtOrdFilterVal").focus();
                                } else { $("#grid").data("kendoGrid").dataSource.read(); }
                            }
                        });

                        $("#btnOrdClearFilter").kendoButton({
                            icon: "filter-clear",
                            click: function (e) {
                                var dropdownlist = $("#ddlOrdFilters").data("kendoDropDownList");
                                dropdownlist.select(0);
                                dropdownlist.trigger("change");
                                $("#txtOrdFilterVal").data("kendoMaskedTextBox").value("");
                                $("#dpOrdFilterFrom").data("kendoDatePicker").value("");
                                $("#dpOrdFilterTo").data("kendoDatePicker").value("");
                                $("#spOrdFilterText").hide();
                                $("#spOrdFilterDates").hide();
                                $("#spOrdFilterButtons").hide();
                                $("#grid").data("kendoGrid").dataSource.read();
                            }
                        });

                        $("#txtOrdFilterVal").data("kendoMaskedTextBox").value("");
                        $("#spOrdFilterText").hide();
                        $("#spOrdFilterDates").hide();
                        $("#spOrdFilterButtons").hide();

                        //*************Hide Both Filter Divs************//
                        $("#filterDiv").hide();
                        $("#filterDivOrders").hide();

                        $("#schedulerDiv").css("margin-top", "1%");


                        //*******All Action Tab Buttons*********//
                        //Modified By LVV for 8.3.0.001 on 8/18/20 - Task #202007231832 - Warehouse Scheduler put Action Menu in content management
                        $("#allDocks").kendoButton({
                            icon: "preview",
                            click: function (e) {
                                if ($("#submenu").is(":visible")) { $('#submenu').css('display', 'none'); } else { $('#submenu').css('display', 'inline'); }
                            }
                        });

                        $('#submenu').css('display', 'none');

                        /////set default message/////
                        $("#txtScreenMessage").html('&nbsp;&nbsp;<h3>Appointments Schedule</h3>');

                        function startChange() {
                            var startDate = start.value();
                            var AlertOrdersArray = $("#ordersWndGrid").data("kendoGrid").dataSource.data();
                            AlertSCheduleDateMisMatch(AlertOrdersArray, startDate);

                            //endDate = end.value();
                            //if (startDate) {
                            //    startDate = new Date(startDate);
                            //    startDate.setDate(startDate.getDate());
                            //    end.min(startDate);
                            //} else if (endDate) {
                            //    start.max(new Date(endDate));
                            //} else {
                            //    endDate = new Date();
                            //    start.max(endDate);
                            //    end.min(endDate);
                            //}
                        }

                        function endChange() {
                            var endDate = end.value(),
                                startDate = start.value();
                            if (endDate) {
                                endDate = new Date(endDate);
                                endDate.setDate(endDate.getDate());
                                start.max(endDate);
                            } else if (startDate) {
                                end.min(new Date(startDate));
                            } else {
                                endDate = new Date();
                                start.max(endDate);
                                end.min(endDate);
                            }
                        }

                        //***********Define Kendo widgets************//
                        $("#txtOrderNumber").kendoMaskedTextBox();
                        $("#txtApptNumber").kendoMaskedTextBox();
                        $("#txtCNS").kendoMaskedTextBox();
                        $("#txtProNumber").kendoMaskedTextBox();

                        $("#txtCarrierName").kendoMaskedTextBox();
                        $("#txtDescription").kendoMaskedTextBox();
                        var start = $("#txtStartTime").kendoDateTimePicker({
                            format: "M/d/yyyy HH:mm",
                            timeFormat: "HH:mm",
                            change: startChange,
                        }).data("kendoDateTimePicker");

                        $("#txtNotes").kendoMaskedTextBox();
                        $("#txtSCAC").kendoMaskedTextBox();
                        var end = $("#txtEndTime").kendoDateTimePicker({
                            format: "M/d/yyyy HH:mm",
                            timeFormat: "HH:mm"
                            //change: endChange,
                        }).data("kendoDateTimePicker");

                        $("#btnSave").kendoButton();
                        $("#btnCancel").kendoButton();

                        var carrierDATSchedulerFrom = $("#DATSchedulerFrom").kendoDateTimePicker({ format: "M/d/yyyy HH:mm", timeFormat: "HH:mm" }).data("kendoDateTimePicker");
                        var carrierDATSchedulerTo = $("#DATSchedulerTo").kendoDateTimePicker({ format: "M/d/yyyy HH:mm", timeFormat: "HH:mm" }).data("kendoDateTimePicker");
                        carrierDATSchedulerFrom.enable(false);
                        carrierDATSchedulerTo.enable(false);
                        var carrierDATStartLoading = $("#DATStartLoading").kendoDateTimePicker({ format: "M/d/yyyy HH:mm", timeFormat: "HH:mm" }).data("kendoDateTimePicker");
                        var carrierDATCheckIn = $("#DATCheckIn").kendoDateTimePicker({ format: "M/d/yyyy HH:mm", timeFormat: "HH:mm" }).data("kendoDateTimePicker");
                        var carrierDATCheckOut = $("#DATCheckOut").kendoDateTimePicker({ format: "M/d/yyyy HH:mm", timeFormat: "HH:mm" }).data("kendoDateTimePicker");
                        var carrierDATFinishLoading = $("#DATFinishLoading").kendoDateTimePicker({ format: "M/d/yyyy HH:mm", timeFormat: "HH:mm" }).data("kendoDateTimePicker");
                        $("#btnSaveCarrier").kendoButton();
                        $("#btnCancelCarrier").kendoButton();

                        $("#txtOrdCNS").kendoMaskedTextBox();
                        $("#txtOrdProNu").kendoMaskedTextBox();
                        $("#txtOrdOrderNu").kendoMaskedTextBox();
                        $("#txtOrdPONu").kendoMaskedTextBox();
                        $("#dtOrdOrderd").kendoDatePicker();
                        $("#dtOrdLoad").kendoDatePicker();
                        $("#dtOrdRequired").kendoDatePicker();
                        $("#txtOrdCases").kendoNumericTextBox({ decimals: 0, format: "#", min: 0 });
                        $("#txtOrdCubes").kendoNumericTextBox({ decimals: 0, format: "#", min: 0 });
                        $("#txtOrdWeight").kendoNumericTextBox({ decimals: 1, min: 0 });
                        $("#txtOrdPalletsIn").kendoNumericTextBox({ decimals: 0, format: "#", min: 0 });
                        $("#txtOrdPalletsOut").kendoNumericTextBox({ decimals: 0, format: "#", min: 0 });
                        $("#txtOrdDesc").kendoMaskedTextBox();

                        $("#txtOrdEquipID").kendoMaskedTextBox();
                        $("#txtOrdOrig").kendoMaskedTextBox();
                        $("#txtOrdOrigAddress").kendoMaskedTextBox();
                        $("#txtOrdOrigCity").kendoMaskedTextBox();
                        $("#txtOrdOrigState").kendoMaskedTextBox();
                        $("#txtOrdOrigZip").kendoMaskedTextBox();
                        $("#txtOrdOrigCountry").kendoMaskedTextBox();
                        $("#txtOrdDest").kendoMaskedTextBox();
                        $("#txtOrdDestAddress").kendoMaskedTextBox();
                        $("#txtOrdDestCity").kendoMaskedTextBox();
                        $("#txtOrdDestState").kendoMaskedTextBox();
                        $("#txtOrdDestZip").kendoMaskedTextBox();
                        $("#txtOrdDestCountry").kendoMaskedTextBox();
                        $("#btnSaveOrd").kendoButton();
                        $("#btnCancelOrd").kendoButton();

                        //*****APC call for Warehouses based on user id*****//
                        $.ajax({
                            url: '/api/vLookupList/GetUserDynamicList/',
                            contentType: 'application/json; charset=utf-8',
                            dataType: 'json',
                            async: false,
                            data: { id: nglUserDynamicLists.CompNEXTrack },
                            headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                            success: function (data) {
                                dsWarehouse = data.Data;
                                try {
                                    var blnSuccess = false;
                                    var blnErrorShown = false;
                                    var strValidationMsg = "";
                                    if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                        if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Get Company Details Failure", data.Errors, null); }
                                        else {
                                            if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                                    blnSuccess = true;
                                                } else { blnSuccess = true; strValidationMsg = "No records were found matching your search criteria"; }
                                            }
                                        }
                                    }
                                    if (blnSuccess === false && blnErrorShown === false) {
                                        if (strValidationMsg.length < 1) { strValidationMsg = "Get Company Details Failure"; }
                                        ngl.showErrMsg("Get Company Details Failure", strValidationMsg, null);
                                    }
                                } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                            },
                            error: function (result) { ngl.showErrMsg("Get Company Details Failure", result, null); }
                        });

                        //*********kendoDropDownList for Comp/Warehouse ***********//
                        $("#ddlwarehouse").kendoDropDownList({
                            dataSource: dsWarehouse,
                            dataTextField: "Name",
                            dataValueField: "Control",
                            autoWidth: true,
                            filter: "contains",
                        });

                        //**********on Change event for warehouse*************//
                        $("#ddlwarehouse").on("change warehouse", function () {
                            //debugger;
                            $('#scheduler').hide();
                            CompControl = $(this).data("kendoDropDownList").dataItem().Control;
                            $.ajax({
                                url: '/api/AMSCompDockDoor/GetRecords/',
                                contentType: 'application/json; charset=utf-8',
                                dataType: 'json',
                                async: false,
                                data: { filter: JSON.stringify({ "filterName": "CompDockCompControl", "filterValue": CompControl, "page": 1, "skip": 0, "take": 100 }) },
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                success: function (data) {
                                    //debugger;
                                    res = data.Data;
                                    try {
                                        var blnSuccess = false;
                                        var blnErrorShown = false;
                                        var strValidationMsg = "";
                                        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                            if (ngl.stringHasValue(data.Errors)) {
                                                blnErrorShown = true;
                                                ngl.showErrMsg("Unexpected Read Dock Doors Failure", data.Errors, null);
                                                //Modified by RHR for v-8.3.0.001 on 09/01/2020  fix bug when no data is returned we now return on error
                                                return;
                                            }
                                            else {
                                                if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                    //Modified by RHR for v-8.3.0.001 on 09/01/2020  fix bug when no data is returned
                                                    //if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) { blnSuccess = true; } else{ blnSuccess = true; strValidationMsg = "No records were found matching your search criteria"; }
                                                    if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                                        blnSuccess = true;
                                                    } else {
                                                        blnSuccess = false;
                                                        strValidationMsg = "Please check that at least one dock door is configured for the selected warehouse.";
                                                    }
                                                }
                                            }
                                        }
                                        if (blnSuccess === false && blnErrorShown === false) {
                                            if (strValidationMsg.length < 1) { strValidationMsg = "Get Dock Doors Failure"; }
                                            ngl.showErrMsg("Cannot locate your dock doors", strValidationMsg, null);
                                            //Modified by RHR for v-8.3.0.001 on 09/01/2020  fix bug when no data is returned we now return on error
                                            return;
                                        }
                                    } catch (err) {
                                        ngl.showErrMsg(err.name, err.description, null);
                                        //Modified by RHR for v-8.3.0.001 on 09/01/2020  fix bug when no data is returned we now return on error
                                        return;
                                    }
                                },
                                error: function (result) {
                                    ngl.showErrMsg("Unexpected Read Dock Doors Failure", result, null);
                                    //Modified by RHR for v-8.3.0.001 on 09/01/2020  fix bug when no data is returned we now return on error
                                    return;
                                }
                            });
                            //Modified by RHR for v-8.3.0.001 on 09/01/2020  fix bug when no data is returned
                            if (typeof (res) !== 'undefined' && ngl.isArray(res) && res.length > 0) {
                                $('#scheduler').show();
                                $("#ddlDockDoorID").data("kendoDropDownList").setDataSource(res);
                                $('#submenu').empty();
                                for (var i = 0; i < res.length; i++) {
                                    $('#submenu').append('<input id="' + res[i].CompDockControl + '" value="' + res[i].CompDockControl + '" style="margin-left:10px" type="checkbox" checked><label for="' + res[i].CompDockControl + '" class="k-checkbox-label">' + res[i].CompDockDockDoorName + '</label>');
                                }
                                scheduler.resources[0].dataSource.data(res);
                                dsAllDockDoorsApptTimeSettings = [];
                                allDockDoorsSettingsArray = [];
                                for (var z = 0; z < res.length; z++) {
                                    var s = new AllFilter();
                                    s.filterName = 'dockDoorControl';
                                    s.filterValue = res[z].CompDockControl;
                                    $.ajax({
                                        url: '/api/AMSCompDockdoor/GetDockApptTimeSettings/',
                                        contentType: 'application/json; charset=utf-8',
                                        dataType: 'json',
                                        async: false,
                                        data: { filter: JSON.stringify(s) },
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                        success: function (data) { if (data.Data != null) { allDockDoorsSettingsArray.push(data.Data[0]); } }
                                    });
                                }
                                dsAllDockDoorsApptTimeSettings = allDockDoorsSettingsArray;
                                startEndDockDoorsTimes();
                                workHours();
                                scheduler.view(scheduler.view().name);
                                BlockOffTime();
                                scheduler.dataSource.read();
                                $("#grid").data("kendoGrid").dataSource.read();
                                //***Save PageSettings*******//
                                InsertOrUpdateCurrentUserPageSetting("SchedulerPage");
                                //Added by RHR for v=8.3.0.002 on 11/18/2020 we now resize when this event takes place
                                resizeSWidget(iLastHeightFactor);
                            }
                        });

                        //************ Appointmnet validation***********//
                        $("#txtFailedMsg").text("Main Error display here!");

                        $("#aatMoreDetails").on("click", function () {
                            if ($("#FailedMsgDetails-UI").is(":visible")) {
                                $("#spanIconMoreDetails").addClass('k-i-plus').removeClass("k-i-minus");
                                $("#FailedMsgDetails-UI").hide();
                            } else { $("#spanIconMoreDetails").addClass('k-i-minus').removeClass("k-i-plus"); $("#FailedMsgDetails-UI").show(); }
                        });

                        $("#txtWndOverridePassword").kendoMaskedTextBox();
                        dsddlRCRequired = new kendo.data.DataSource({
                            serverSorting: true,
                            serverPaging: true,
                            transport: {
                                read: function (options) {
                                    $.ajax({
                                        url: '/api/vLookupList/GetStaticList/',
                                        contentType: 'application/json; charset=utf-8',
                                        dataType: 'json',
                                        async: false,
                                        data: { id: nglStaticLists.SchedulerReasonCodes },
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                        success: function (data) {
                                            options.success(data);
                                            try {
                                                var blnSuccess = false;
                                                var blnErrorShown = false;
                                                var strValidationMsg = "";
                                                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                                    if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Get Reson Code Required Failure", data.Errors, null); }
                                                    else {
                                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) { blnSuccess = true; } else { blnSuccess = true; strValidationMsg = "No records were found matching your search criteria"; }
                                                        }
                                                    }
                                                }
                                                if (blnSuccess === false && blnErrorShown === false) {
                                                    if (strValidationMsg.length < 1) { strValidationMsg = "Get Reson Code Required Failure"; }
                                                    ngl.showErrMsg("Get Reson Code RequiredFailure", strValidationMsg, null);
                                                }
                                            } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                                        },
                                        error: function (result) { ngl.showErrMsg("Get Reson Code Required Failure", result, null); }
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
                                        Description: { type: "string" },
                                    }
                                },
                                errors: "Errors"
                            },
                            error: function (xhr, textStatus, error) { ngl.showErrMsg("Access Reson Code Required Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges(); }
                        });

                        $("#ddlWndOverrideReasonCode").kendoDropDownList({
                            dataSource: dsddlRCRequired,
                            dataTextField: "Description",
                            dataValueField: "Control",
                            optionLabel: "Select...",
                            autoWidth: true,
                            filter: "contains",
                            change: function () { if ($("#ddlWndOverrideReasonCode").val() != "") { $("#ddlWndOverrideReasonCode-validation").addClass("hide-display"); } else { $("#ddlWndOverrideReasonCode-validation").removeClass("hide-display"); } },
                        });

                        $("#txtWndOverridePassword").on("change input", function () {
                            if ($("#txtWndOverridePassword").val() != "") { $("#txtWndOverridePassword-validation").addClass("hide-display"); } else { $("#txtWndOverridePassword-validation").removeClass("hide-display"); }
                        });

                        $("#btnWndOverride").kendoButton({
                            click: function (e) {
                                var submit = true;
                                if ($(".SPRequired_UI").is(":visible")) {
                                    if ($("#txtWndOverridePassword").data("kendoMaskedTextBox").value() == "") { $("#txtWndOverridePassword-validation").removeClass("hide-display"); submit = false; }
                                }
                                if ($(".RCRequired_UI").is(":visible")) {
                                    if ($("#ddlWndOverrideReasonCode").data("kendoDropDownList").value() == "") { $("#ddlWndOverrideReasonCode-validation").removeClass("hide-display"); submit = false; }
                                }
                                if ($("#AppointmentAddEditWnd").is(":visible")) {
                                    if (submit == true) { saveUpdateAppointment(); wndOverride.close(); }
                                } else {
                                    if (submit == true) {
                                        if (subUpdateValidation.BitString !== undefined) { subUpdateValidation.Input = $("#txtWndOverridePassword").val(); subUpdateValidation.ReasonCode = $("#ddlWndOverrideReasonCode").val(); }
                                        var UpdateApptdata = { "Validation": subUpdateValidation, "Appt": subUpdateAppointmentData, "Ord": subUpdateAppointmentOrders, "Flag": false };
                                        UpdateAMSAppointment(UpdateApptdata);
                                        wndOverride.close();
                                    }
                                }
                            }
                        });

                        $("#btnViewAvailability").kendoButton({ click: function (e) { $("#viewAvailableSlotsGrid").data("kendoGrid").dataSource.read(); } });

                        dsAvailableApptData = new kendo.data.DataSource({
                            pageSize: 10,
                            transport: {
                                read: function (options) {
                                    var ordersArray = [];
                                    if ($("#AppointmentAddEditWnd").is(":visible")) { ordersArray = $("#ordersWndGrid").data("kendoGrid").dataSource.data(); } else { ordersArray = subUpdateAppointmentOrders; }
                                    $.ajax({
                                        type: 'Post',
                                        url: 'api/AMSAppointment/GetWarehouseAvailableAppointments',
                                        contentType: 'application/json; charset=utf-8',
                                        dataType: 'json',
                                        data: JSON.stringify(ordersArray),
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                        success: function (data) {
                                            options.success(data)
                                            console.log(data);
                                            try {
                                                var blnSuccess = false;
                                                var blnErrorShown = false;
                                                var strValidationMsg = "";
                                                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                                    if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Get Available Appointments Failure", data.Errors, null); }
                                                    else {
                                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) { blnSuccess = true; } else { blnSuccess = true; strValidationMsg = "No records were found matching your search criteria"; }
                                                            $("#viewAvailableSlotsGrid").show();
                                                        }
                                                    }
                                                }
                                                if (blnSuccess === false && blnErrorShown === false) {
                                                    if (strValidationMsg.length < 1) { strValidationMsg = "Get Available Appointments Failure"; }
                                                    ngl.showErrMsg("Get  Available Appointments Failure", strValidationMsg, null);
                                                }
                                            } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                                        },
                                        error: function (result) { options.error(result); }
                                    });
                                },
                                parameterMap: function (options, operation) { return options; }
                            },
                            schema: {
                                data: "Data",
                                total: "Count",
                                model: {
                                    id: "CarrierNumber",
                                    fields: {
                                        CarrierNumber: { type: "number" },
                                        Warehouse: { type: "string" },
                                        Date: { type: "date" },
                                        StartTime: { type: "date" },
                                        EndTime: { type: "string" }, //NOTE: EndTime in this scenario is a string which gets parsed into an array - never convert it or format it like a date //Modified by LVV on 11/1/2019
                                    }
                                },
                                errors: "Errors"
                            },
                            error: function (xhr, textStatus, error) { ngl.showErrMsg("Access  Available Appointments Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges(); }
                        });

                        $("#viewAvailableSlotsGrid").kendoGrid({
                            autoBind: false,
                            noRecords: true,
                            height: 200,
                            dataSource: dsAvailableApptData,
                            pageable: true,
                            columns: [
                                { command: [{ name: "bookAppt", text: "Book Appt", width: 100, click: BookAppointment }], title: "Action" },
                                { field: "Date", title: "Date", format: "{0:M/d/yyyy}" },
                                { field: "StartTime", title: "Start Time", template: "#= kendo.toString(kendo.parseDate(StartTime, 'HH:mm'), 'HH:mm') #" }, //format: "{0:HH:mm}", //Modified by LVV on 11/1/2019
                                { field: "EndTime", title: "End Time", hidden: true }, //NOTE: EndTime in this scenario is a string which gets parsed into an array - never convert it or format it like a date //Modified by LVV on 11/1/2019
                                //{ field: null, title: "End Time", hidden: true, template: '#=generateEndTimeTemplate(data.EndTime)#' }, //Modified by LVV on 11/1/2019 - don't forget - we don't show this here because EndTime can be different depending on the resource configs
                            ],
                        });

                        var AvailApptControl = 0;
                        function BookAppointment(e) {
                            var BookApptObject = this.dataItem($(e.currentTarget).closest("tr"));
                            BookApptObject.ApptControl = AvailApptControl;
                            BookApptObject.EndTime = BookApptObject.EndTime; //NOTE: EndTime in this scenario is a string which gets parsed into an array - never convert it or format it like a date //Modified by LVV on 11/1/2019
                            //Modified by LVV for v-8.2 on 11/1/2019 we now use ngl.convertDateForWindows to avoid non-ascii characters in date string
                            BookApptObject.StartTime = ngl.convertTimePickerToDateString(BookApptObject.StartTime, ngl.convertDateForWindows(BookApptObject.StartTime, ""), "");

                            $.ajax({
                                url: "api/AMSAppointment/BookWarehouseAppointment",
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                dataType: 'json',
                                data: JSON.stringify(BookApptObject),
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                success: function (data) {
                                    try {
                                        var blnSuccess = false;
                                        var blnErrorShown = false;
                                        var strValidationMsg = "";
                                        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                            if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Book Warehouse Appointment Failure", data.Errors, null); }
                                            else {
                                                if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                    if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                                        blnSuccess = true;
                                                        if (data.Data[0] == false) { ngl.showWarningMsg("Book Warehouse Appointment Failure!", data.Errors, null); }
                                                        else {
                                                            //refresh Grid
                                                            wndOverride.close();
                                                            wndAddEditEvent.close();
                                                            $("#grid").data("kendoGrid").dataSource.read();
                                                            ngl.showSuccessMsg("Book Warehouse Appointment Sucess");
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (blnSuccess === false && blnErrorShown === false) {
                                            if (strValidationMsg.length < 1) { strValidationMsg = "Book Warehouse Appointment Failure"; }
                                            ngl.showErrMsg("Book Warehouse Appointment Failure", strValidationMsg, null);
                                        }
                                    } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                                },
                                error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Book Warehouse Appointment Failure"); ngl.showErrMsg("Book Warehouse Appointment Failure", sMsg, null); }
                            });
                        }

                        //Modified By LVV 9/11/18
                        function AlertSCheduleDateMisMatch(orders, AppointmentStartDate) {
                            if (orders == null || AppointmentStartDate == null) { return; }
                            //now check to see if any of the orders have a schedule date that does not match the load/required date.
                            var ConsolidatedErrorMessage = "";
                            var lateDelivery = false, shipShort = false, dealyInProductrion = false, deliveryBefore = false;
                            var lateDeliveryOrders = "", shipShortOrders = "", dealyInProductrionOrders = "", deliveryBeforeOrders = "";
                            var lateDelOrds = [], shipShortOrds = [], delayProdOrds = [], delBeforeOrds = [];
                            $.each(orders, function (i, order) {
                                {
                                    if (order.OrderType <= 2) //0-2 ordertype is outbound
                                    {
                                        if (order.BookDateLoad !== undefined) {
                                            if (ngl.getShortDateString(AppointmentStartDate, "") !== ngl.getShortDateString(order.BookDateLoad, "")) {
                                                if (new Date(AppointmentStartDate) > new Date(order.BookDateLoad)) //Late Delivery
                                                {
                                                    lateDelivery = true;
                                                    if (jQuery.inArray(order.BookCarrOrderNumber, lateDelOrds) <= -1) {
                                                        lateDelOrds.push(order.BookCarrOrderNumber);
                                                        if (lateDeliveryOrders == "") { lateDeliveryOrders += order.BookCarrOrderNumber; } else { lateDeliveryOrders += ", " + order.BookCarrOrderNumber; }
                                                    }
                                                }
                                                else if (new Date(AppointmentStartDate) < new Date(order.BookDateLoad)) //Ship Short
                                                {
                                                    shipShort = true;
                                                    if (jQuery.inArray(order.BookCarrOrderNumber, shipShortOrds) <= -1) {
                                                        shipShortOrds.push(order.BookCarrOrderNumber);
                                                        if (shipShortOrders == "") { shipShortOrders += order.BookCarrOrderNumber; } else { shipShortOrders += ", " + order.BookCarrOrderNumber; }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else //3-5 is inbound
                                    {
                                        if (order.BookDateRequired !== undefined) {
                                            if (ngl.getShortDateString(AppointmentStartDate, "") !== ngl.getShortDateString(order.BookDateRequired, "")) {
                                                if (new Date(AppointmentStartDate) > new Date(order.BookDateRequired)) //Delay in Production
                                                {
                                                    dealyInProductrion = true;
                                                    if (jQuery.inArray(order.BookCarrOrderNumber, delayProdOrds) <= -1) {
                                                        delayProdOrds.push(order.BookCarrOrderNumber);
                                                        if (dealyInProductrionOrders == "") { dealyInProductrionOrders += order.BookCarrOrderNumber; } else { dealyInProductrionOrders += ", " + order.BookCarrOrderNumber; }
                                                    }
                                                }
                                                else if (new Date(AppointmentStartDate) < new Date(order.BookDateRequired)) //Delivery Before
                                                {
                                                    deliveryBefore = true;
                                                    if (jQuery.inArray(order.BookCarrOrderNumber, delBeforeOrds) <= -1) {
                                                        delBeforeOrds.push(order.BookCarrOrderNumber);
                                                        if (deliveryBeforeOrders == "") { deliveryBeforeOrders += order.BookCarrOrderNumber; } else { deliveryBeforeOrders += ", " + order.BookCarrOrderNumber; }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            });
                            if (lateDelivery || shipShort || dealyInProductrion || deliveryBefore) {
                                if (lateDelivery) { if (ConsolidatedErrorMessage == "") { ConsolidatedErrorMessage += "This change may cause a late delivery for Orders: " + lateDeliveryOrders + "." } else { ConsolidatedErrorMessage += "</br>This change may cause a late delivery for Orders: " + lateDeliveryOrders + "." } };
                                if (shipShort) { if (ConsolidatedErrorMessage == "") { ConsolidatedErrorMessage += "This change may cause order to ship short for Orders: " + shipShortOrders + "." } else { ConsolidatedErrorMessage += "</br>This change may cause order to ship short for Orders: " + shipShortOrders + "." } };
                                if (dealyInProductrion) { if (ConsolidatedErrorMessage == "") { ConsolidatedErrorMessage += "This may cause delay in production/outbound for Orders: " + dealyInProductrionOrders + "." } else { ConsolidatedErrorMessage += "</br>This may cause delay in production/outbound for Orders: " + dealyInProductrionOrders + "." } };
                                if (deliveryBefore) { if (ConsolidatedErrorMessage == "") { ConsolidatedErrorMessage += "This may cause delivery before warehouse is prepared to receive for Orders: " + deliveryBeforeOrders + "." } else { ConsolidatedErrorMessage += "</br>This may cause delivery before warehouse is prepared to receive for Orders: " + deliveryBeforeOrders + "." } };
                                ngl.showWarningMsg("Date Mismatch Warning", ConsolidatedErrorMessage, null);
                            }
                        }

                        //Global variable to store the last dragged book control id
                        var lastDraggedBookControlId = null;

                        var bookControl = "";
                        var carrierName = "";
                        var carrierScac = "";
                        var bookControlIdByDragging = "";

                        //Enable drag option - Just store the id when dragging starts
                        function enableSimpleGridDrag() {
                            bookControlIdByDragging = "";

                            var grid = $("#grid").data("kendoGrid");

                            //Add data attributes to each row
                            grid.tbody.find("tr").each(function () {
                                var data = grid.dataItem(this);
                                $(this)
                                    .attr("data-bookcontrol", data.BookControl)
                                    .css("cursor", "move");

                                bookControlIdByDragging = data.BookControl;
                            });

                            //Detect mouse down - Start of dragging
                            grid.tbody.on("mousedown", "tr", function (e) {
                                var bookControlId = $(this).data("bookcontrol");
                                bookControlIdByDragging = bookControlId;

                                window.lastDraggedBookControl = bookControlId;
                                console.log("🎯 DRAG STARTS - BookControl id:", bookControlId);

                                bookControlIdByDragging = bookControlId;

                                //Ajax call - Starts (only if we have book control id)
                                if (bookControlIdByDragging) {
                                    carrierName = "";
                                    carrierScac = "";

                                    //Get book details passing book control id
                                    $.ajax({
                                        url: 'api/AMSOrder/GetRecordsByBookControl',
                                        contentType: 'application/json; charset=utf-8',
                                        dataType: 'json',
                                        async: false,
                                        data: { filter: JSON.stringify(bookControlIdByDragging) },
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                        success: function (data) {
                                            try {
                                                //Check if data exists and is an array
                                                if (data && data.Data && Array.isArray(data.Data)) {
                                                    //For loop to iterate book item data
                                                    for (var i = 0; i < data.Data.length; i++) {
                                                        var bookItem = data.Data[i];
                                                        console.log("Carrier name:", bookItem.BookItemCarrTarEquipMatName);
                                                        console.log("Carrier scac:", bookItem.BookItemNMFCSubClass);
                                                        console.log("---");

                                                        bookControl = bookItem.BookItemControl;
                                                        carrierName = bookItem.BookItemCarrTarEquipMatName;
                                                        carrierScac = bookItem.BookItemNMFCSubClass;
                                                    }
                                                } else {
                                                    console.log("No data found or data is not in expected format");
                                                }
                                            } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                                        }
                                    });
                                    //Ajax call - Ends
                                }
                            });

                            console.log("✅ Data attribute drag enabled");
                        }

                        //************Add edit popup for Appointment************//
                        function AddEditFunc(e) {
                            enableSimpleGridDrag();

                            e.preventDefault();
                            $("#tabstrip").data("kendoTabStrip").select("li:contains('Appointment Information')");
                            EventAppt = "";
                            var dataSource = this.dataSource;
                            var event = e.event;
                            //validation remove And Default Values
                            $("#apptCarrier-validation").addClass("hide-display");
                            $("#apptSCAC-validation").addClass("hide-display");
                            $("#apptEndDate-validation").addClass("hide-display");
                            $("#apptStartDate-validation").addClass("hide-display");
                            $('#txtStartTime').data('kendoDateTimePicker').enable(true);
                            $('#txtEndTime').data('kendoDateTimePicker').enable(true);
                            $("#chkAllDay").prop('checked', false);
                            $("#compBound input:radio").attr('disabled', false);
                            $("#AddEditOrder").hide();
                            //**********Carrier Data default*********//
                            carrierDATStartLoading.value("");
                            carrierDATFinishLoading.value("");
                            carrierDATCheckIn.value("");
                            carrierDATCheckOut.value("");
                            $("#ApptCountrol").val("")
                            EventAppt = event;
                            $("#ApptCountrol").val(event.AMSApptControl);
                            $("#txtCarrierName").val(carrierName);
                            $("#txtSCAC").val(carrierScac);
                            $("#txtDescription").val(event.AMSApptDescription);
                            start.value(event.start);
                            end.value(event.end);
                            $("#ddlDockDoorID").data("kendoDropDownList").value(event.doorId);
                            $("#txtNotes").val(event.AMSApptNotes);
                            $("#txtOrderNumber").kendoMaskedTextBox().val("");
                            $("#txtApptNumber").kendoMaskedTextBox().val("");
                            $("#txtCNS").kendoMaskedTextBox().val("");
                            $("#txtProNumber").kendoMaskedTextBox().val("");
                            var ApptOrders;
                            var FilterApptControl;
                            if (event.AMSApptControl) {
                                EventFlag = false;
                                $("#AppointmentAddEditWnd").data("kendoWindow").title("Appointment Details");
                                AvailApptControl = event.AMSApptControl;
                                FilterApptControl = event.AMSApptControl;
                                carrierDATSchedulerFrom.value(event.start);
                                carrierDATSchedulerTo.value(event.end);
                                carrierDATStartLoading.value(event.AMSApptStartLoadingDateTime);
                                carrierDATFinishLoading.value(event.AMSApptFinishLoadingDateTime);
                                carrierDATCheckIn.value(event.AMSApptActualDateTime);
                                carrierDATCheckOut.value(event.AMSApptActLoadCompleteDateTime);
                                TrackingFieldsUserFieldsApptControl = event.AMSApptControl;
                                $("#TrackingFieldsGrid").data("kendoGrid").dataSource.read();
                                $("#UserFieldsGrid").data("kendoGrid").dataSource.read();
                            } else {
                                EventFlag = true
                                $("#AppointmentAddEditWnd").data("kendoWindow").title("Add Appointment");
                                AvailApptControl = 0;
                                FilterApptControl = 0;
                                carrierDATSchedulerFrom.value(event.start);
                                carrierDATSchedulerTo.value(event.end);
                                $("#TrackingFieldsGrid").data("kendoGrid").dataSource.data([]);
                                $("#UserFieldsGrid").data("kendoGrid").dataSource.data([]);
                            }
                            $.ajax({
                                url: '/api/AMSOrder/GetRecordsByAppointmentFilter/',
                                contentType: 'application/json; charset=utf-8',
                                dataType: 'json',
                                async: false,
                                data: { filter: JSON.stringify({ "filterValue": FilterApptControl }) },
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                success: function (data) {
                                    ApptOrders = data;
                                    try {
                                        var blnSuccess = false;
                                        var blnErrorShown = false;
                                        var strValidationMsg = "";
                                        if (typeof (data) != 'undefined' && ngl.isObject(data)) {
                                            if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Get Orders Details Failure", data.Errors, null); }
                                            else {
                                                if (typeof (data.Data) != 'undefined' && ngl.isArray(data.Data)) {
                                                    if (data.Data.length > 0 && typeof (data.Data[0]) != 'undefined' && ngl.isObject(data.Data[0])) { blnSuccess = true; } else { blnSuccess = true; strValidationMsg = "No records were found matching your search criteria"; }
                                                }
                                            }
                                        }
                                        if (blnSuccess === false && blnErrorShown === false) {
                                            if (strValidationMsg.length < 1) { strValidationMsg = "Get Orders Details Failure"; }
                                            ngl.showErrMsg("Get Orders Details Failure", strValidationMsg, null);
                                        }
                                    } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                                },
                                error: function (result) { ngl.showErrMsg("Get Orders Details Failure", result, null); }
                            });
                            $("#ordersWndGrid").data("kendoGrid").dataSource.data(ApptOrders.Data);
                            if (ApptOrders.Data.length > 0) {
                                if (ApptOrders.Data[0].BookDestCompControl == CompControl) { //Modified By LVV on 11/6/2019 Ticket #201911041624
                                    $("#rdoInBound").prop('checked', true);
                                    $("#compBound input:radio").attr('disabled', true);
                                } else if (ApptOrders.Data[0].BookOrigCompControl == CompControl) {
                                    $("#rdoOutBound").prop('checked', true);
                                    $("#compBound input:radio").attr('disabled', true);
                                }
                            }
                            sdate = "";
                            wndAddEditEvent.center().open();

                            //Reset dragged data after use
                            draggedOrderData = null;
                        }

                        var sdate;
                        var edate;
                        $("#chkAllDay").on("click", function () {
                            if (!sdate) { sdate = new Date($('#txtStartTime').val()); edate = new Date($('#txtEndTime').val()); }
                            if ($("#chkAllDay").is(":checked")) {
                                $("#txtStartTime").val(kendo.toString(new Date(sdate), "M/d/yyyy") + " " + kendo.toString(new Date("10/10/2018 00:00"), "HH:mm"));
                                $("#txtEndTime").val(kendo.toString(new Date(edate), "M/d/yyyy") + " " + kendo.toString(new Date("10/10/2018 23:59"), "HH:mm"));
                                $('#txtStartTime').data('kendoDateTimePicker').enable(false);
                                $('#txtEndTime').data('kendoDateTimePicker').enable(false);
                            } else {
                                $('#txtStartTime').data('kendoDateTimePicker').enable(true);
                                $('#txtEndTime').data('kendoDateTimePicker').enable(true);
                                $("#txtStartTime").val(kendo.toString(new Date(sdate), "M/d/yyyy HH:mm"));
                                $("#txtEndTime").val(kendo.toString(new Date(edate), "M/d/yyyy HH:mm"));
                            }
                        });

                        var EventAppt = new AMSAppointments();
                        var EventFlag;
                        var ApptValidation = {};

                        function saveUpdateAppointment() {
                            var submit = true;
                            if ($("#txtCarrierName").data("kendoMaskedTextBox").value() == "") { $("#apptCarrier-validation").removeClass("hide-display"); submit = false; }
                            if ($("#txtSCAC").data("kendoMaskedTextBox").value() == "") { $("#apptSCAC-validation").removeClass("hide-display"); submit = false; }
                            var startDate = new Date($('#txtStartTime').val());
                            var endDate = new Date($('#txtEndTime').val());
                            if (startDate == "Invalid Date") { $("#apptStartDate-validation").removeClass("hide-display"); submit = false; }
                            if (endDate == "Invalid Date") { $("#apptEndDate-validation").removeClass("hide-display"); submit = false; }
                            if (startDate > endDate) { ngl.showWarningMsg("Start DateTime can't be greater than End DateTime", ""); submit = false; }
                            if (submit == true) {
                                if (EventFlag == true) {
                                    ////console.log('Event Flag is true');
                                    EventAppt.AMSApptCompControl = CompControl
                                    EventAppt.AMSApptCarrierControl = bookControl;
                                    EventAppt.AMSApptCarrierName = $("#txtCarrierName").data("kendoMaskedTextBox").value();
                                    EventAppt.AMSApptCarrierSCAC = $("#txtSCAC").data("kendoMaskedTextBox").value();
                                    EventAppt.AMSApptDescription = $("#txtDescription").data("kendoMaskedTextBox").value();
                                    EventAppt.AMSApptStartDate = $("#txtStartTime").val();
                                    EventAppt.AMSApptEndDate = $("#txtEndTime").val();
                                    EventAppt.AMSApptDockdoorID = $('#ddlDockDoorID').data("kendoDropDownList").dataItem().CompDockDockDoorID;
                                    EventAppt.AMSApptNotes = $("#txtNotes").data("kendoMaskedTextBox").value();
                                    EventAppt.AMSApptRecurrenceParentControl = null;
                                    EventAppt.AMSApptRecurrence = "";
                                    EventAppt.AMSApptActualDateTime = $("#DATCheckIn").val();
                                    EventAppt.AMSApptStartLoadingDateTime = $("#DATStartLoading").val();
                                    EventAppt.AMSApptFinishLoadingDateTime = $("#DATFinishLoading").val();
                                    EventAppt.AMSApptActLoadCompleteDateTime = $("#DATCheckOut").val();
                                    EventAppt.AMSApptModDate = new Date();
                                    EventAppt.AMSApptStatusCode = 0;
                                    console.log(EventAppt);
                                } else {
                                    ////console.log('Event Flag is false');
                                    EventAppt.AMSApptCarrierName = $("#txtCarrierName").data("kendoMaskedTextBox").value();
                                    EventAppt.AMSApptCarrierSCAC = $("#txtSCAC").data("kendoMaskedTextBox").value();
                                    EventAppt.AMSApptDescription = $("#txtDescription").data("kendoMaskedTextBox").value();
                                    EventAppt.AMSApptStartDate = $("#txtStartTime").val();
                                    EventAppt.AMSApptEndDate = $("#txtEndTime").val();
                                    EventAppt.AMSApptDockdoorID = $('#ddlDockDoorID').data("kendoDropDownList").dataItem().CompDockDockDoorID;
                                    EventAppt.AMSApptNotes = $("#txtNotes").data("kendoMaskedTextBox").value();
                                    EventAppt.AMSApptActualDateTime = $("#DATCheckIn").val();
                                    EventAppt.AMSApptStartLoadingDateTime = $("#DATStartLoading").val();
                                    EventAppt.AMSApptFinishLoadingDateTime = $("#DATFinishLoading").val();
                                    EventAppt.AMSApptActLoadCompleteDateTime = $("#DATCheckOut").val();
                                    EventAppt.AMSApptModDate = new Date
                                    console.log(EventAppt);
                                }
                                var Appt = EventAppt;
                                ////console.log('Map Event to Appt');
                                console.log(Appt);
                                var Flag = EventFlag;
                                var Ord = $("#ordersWndGrid").data("kendoGrid").dataSource.data();
                                for (var i = 0; i < Ord.length; i++) {
                                    if ($("#rdoInBound").is(":checked")) { Ord[i].BookDestCompControl = CompControl; Ord[i].BookOrigCompControl = 0; }
                                    else if ($("#rdoOutBound").is(":checked")) { Ord[i].BookOrigCompControl = CompControl; Ord[i].BookDestCompControl = 0; }
                                }
                                if (ApptValidation.BitString !== undefined) { ApptValidation.Input = $("#txtWndOverridePassword").val(); ApptValidation.ReasonCode = $("#ddlWndOverrideReasonCode").val(); }
                                else {
                                    ApptValidation.SPRequired = false;
                                    ApptValidation.InvalidSP = false;
                                    ApptValidation.RCRequired = false;
                                    ApptValidation.InvalidRC = false;
                                    ApptValidation.Success = false;
                                    ApptValidation.NoOverride = false;
                                    ApptValidation.BitString = null;
                                    ApptValidation.Input = null;
                                    ApptValidation.ReasonCode = null;
                                    //ApptValidation.ReasonDesc   = null;
                                    ApptValidation.FailedMsg = null;
                                    ApptValidation.FailedMsgDetails = null;
                                }
                                ////console.log('Save Appt 1');
                                console.log(Appt);
                                var datanew = { "Validation": ApptValidation, "Appt": Appt, "Ord": Ord, "Flag": Flag };
                                $.ajax({
                                    url: '/api/AMSAppointment/SaveUpdateAppointmentOrders/',
                                    type: 'Post',
                                    contentType: 'application/json; charset=utf-8',
                                    dataType: 'json',
                                    data: JSON.stringify(datanew),
                                    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                    success: function (data) {
                                        try {
                                            var blnSuccess = false;
                                            var blnErrorShown = false;
                                            var strValidationMsg = "";
                                            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                                if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Save Appointment Failure", data.Errors, null); }
                                                else {
                                                    if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                        if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                                            blnSuccess = true;
                                                            if (data.Data[0].Success !== undefined) {
                                                                if (data.Data[0].Success == false) {
                                                                    $("#spanIconMoreDetails").addClass('k-i-minus').removeClass("k-i-plus");
                                                                    $("#FailedMsgDetails-UI").show();
                                                                    $("#txtWndOverridePassword-validation").addClass("hide-display");
                                                                    $("#ddlWndOverrideReasonCode-validation").addClass("hide-display");
                                                                    $(".SPRequired_UI").hide();
                                                                    $(".RCRequired_UI").hide();
                                                                    $(".btnOcerride_UI").hide();
                                                                    if (!data.Data[0].NoOverride) {
                                                                        if (data.Data[0].SPRequired) { $("#txtWndOverridePassword").val(""); $(".SPRequired_UI").show(); }
                                                                        if (data.Data[0].RCRequired) { $("#ddlWndOverrideReasonCode").data("kendoDropDownList").select(0); $(".RCRequired_UI").show(); }
                                                                        if (data.Data[0].SPRequired || data.Data[0].RCRequired) { $(".btnOcerride_UI").show(); }
                                                                    }
                                                                    $("#txtFailedMsg").text(data.Data[0].FailedMsg);
                                                                    if (data.Data[0].FailedMsgDetails.length > 0) {
                                                                        $("#txtFailedMsgDetails").html("");
                                                                        $.each(data.Data[0].FailedMsgDetails, function (i, val) {
                                                                            $("#txtFailedMsgDetails").append('<li>' + val + '</li>');
                                                                        });
                                                                    }
                                                                    $("#viewAvailableSlotsGrid").hide();
                                                                    wndOverride.center().open();
                                                                    ApptValidation = data.Data[0];
                                                                }
                                                            }
                                                            else {
                                                                wndAddEditEvent.close();
                                                                scheduler.dataSource.read();
                                                                $("#grid").data("kendoGrid").dataSource.read();
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            if (blnSuccess === false && blnErrorShown === false) {
                                                if (strValidationMsg.length < 1) { strValidationMsg = "Save Appointment Failure"; }
                                                ngl.showErrMsg("Save Appointment Failure", strValidationMsg, null);
                                            }
                                        } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                                    },
                                    error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure"); ngl.showErrMsg("Save Appointment Failure", sMsg, null); }
                                });
                            } else {
                                //ngl.showWarningMsg("Please ensure to input relevant data in all necessary input fields","");
                            }
                        }

                        $("#btnSave").kendoButton({ icon: "save", click: function (e) { ApptValidation = {}; saveUpdateAppointment(); } });

                        $("#btnSaveCarrier").kendoButton({ icon: "save", click: function (e) { saveUpdateAppointment(); } });

                        $("#txtCarrierName").on("change input", function () {
                            var datatypesval = $(this).val();
                            if (datatypesval != "") { $("#apptCarrier-validation").addClass("hide-display"); } else { $("#apptCarrier-validation").removeClass("hide-display"); }
                        });

                        $("#txtSCAC").on("change input", function () {
                            var datatypesval = $(this).val();
                            if (datatypesval != "") { $("#apptSCAC-validation").addClass("hide-display"); } else { $("#apptSCAC-validation").removeClass("hide-display"); }
                        });

                        $("#txtStartTime").on("change input", function () {
                            var datatypesval = $(this).val();
                            if (datatypesval != "") { $("#apptStartDate-validation").addClass("hide-display"); } else { $("#apptStartDate-validation").removeClass("hide-display"); }
                        });

                        $("#txtEndTime").on("change input", function () {
                            var datatypesval = $(this).val();
                            if (datatypesval != "") { $("#apptEndDate-validation").addClass("hide-display"); } else { $("#apptEndDate-validation").removeClass("hide-display"); }
                        });

                        $("#btnCancel").kendoButton({ icon: "cancel", click: function (e) { wndAddEditEvent.close(); } });

                        $("#btnCancelCarrier").kendoButton({ icon: "cancel", click: function (e) { wndAddEditEvent.close(); } });


                        if ($('#ddlwarehouse').data("kendoDropDownList").dataItem()) {
                            var ddlObj = $('#ddlwarehouse').data("kendoDropDownList").dataItem();
                            if (ddlObj.Control != null && ddlObj.Control != undefined) { CompControl = ddlObj.Control; } else { CompControl = -1; }
                        } else { CompControl = -1; }


                        function scheduler_view_range(e) {
                            var view = e.sender.view();
                            Vstart = kendo.date.addDays(view._startDate, -14); //view._startDate;
                            Vend = kendo.date.addDays(view._endDate, +14); //view._endDate;
                            //Vstart = view._startDate;
                            //Vend = view._endDate;
                            if ($("#dpOrdFilterFrom")) {
                                $("#dpOrdFilterFrom").data("kendoDatePicker").value(Vstart);
                            }
                            if ($("#dpOrdFilterTo")) {
                                $("#dpOrdFilterTo").data("kendoDatePicker").value(Vend);
                            }

                            if (cllAPI == true) {
                                workHours();
                                scheduler.dataSource.read();
                                cllAPI = false;
                                scheduler.view(scheduler.view().name);
                                BlockOffTime();
                                if ($("#filterDivOrders").is(":visible")) { $("#grid").data("kendoGrid").dataSource.read(); }
                                //***Save PageSettings*******//
                                InsertOrUpdateCurrentUserPageSetting("SchedulerPage");
                                //Added by RHR for v=8.3.0.002 on 11/18/2020 we now resize when this event takes place
                                resizeSWidget(iLastHeightFactor);
                            }
                        }

                        //************Drag and Drop Grid data to Scheduler*****************//
                        function createDropArea(scheduler) {
                            console.log("Create Drop Area");

                            scheduler.view().content.kendoDropTargetArea({
                                filter: ".k-scheduler-table td, .k-event",
                                drop: function (e) {
                                    var offset = $(e.dropTarget).offset();
                                    if ($(e.target).hasClass("customNonwork") && scheduler.view().name == "month") { ngl.showWarningMsg("Appointment cannot be created in selected time period!", ""); }
                                    else {
                                        var slot = scheduler.slotByPosition(offset.left, offset.top);
                                        var doorSlot = scheduler.resourcesBySlot(slot);
                                        var orderObject = grid.dataItem(grid.select());

                                        console.log("Testing: " + orderObject.BookConsPrefix);
                                        if (orderObject && slot) {
                                            var DoorObj = {};// res.find(function(x){return x.CompDockDockDoorID == doorSlot.doorId}); //Chrome Will support this one line code to get Object
                                            var k = 0;
                                            while (k < res.length) {
                                                if (res[k].CompDockDockDoorID == doorSlot.doorId) {
                                                    DoorObj = res[k];
                                                    break;
                                                }
                                                k++;
                                            }
                                            $("#AppointmentAddEditWnd").data("kendoWindow").title("Add Appointment");
                                            $("#txtOrderNumber").kendoMaskedTextBox().val("");
                                            $("#txtApptNumber").kendoMaskedTextBox().val("");
                                            $("#txtCNS").kendoMaskedTextBox().val("");
                                            $("#txtProNumber").kendoMaskedTextBox().val("");
                                            //Modified by RHR for v-8.3.0.002 on 10/28/2020 fix issue with stacking
                                            //we must clear all invalid data from previous appointment selection
                                            //stored in variables on the page (bad scope of data but this is all we have to work with for now)
                                            //In the future we need to store this data in a seperate instance for each popup window, not on the page
                                            carrierDATStartLoading.value("");
                                            carrierDATFinishLoading.value("");
                                            carrierDATCheckIn.value("");
                                            carrierDATCheckOut.value("");
                                            // Begin Modified for v-8.5.2.004 on 07/21/
                                            // added logic to support pickup and delivery appointments for transfer orders
                                            // added logic to allow stacking only when company and carrier match
                                            var event;
                                            if ($(e.dropTarget).hasClass("k-event")) {
                                                var event = scheduler.dataSource.getByUid($(e.dropTarget).attr("data-uid"));

                                                ////console.log("Drop Target event from datasource");
                                                if (event) {
                                                    console.log(event);
                                                    EventAppt = event;
                                                } else {
                                                    //console.log("No Drop Target event Data");
                                                    EventAppt.AMSApptControl = 0;
                                                }
                                            } else {
                                                console.log("Did not  Drop onto an event");
                                                EventAppt.AMSApptControl = 0;
                                            }

                                            if (typeof (EventAppt) !== 'undefined' && ngl.isObject(EventAppt)) {

                                                if ((typeof (orderObject.BookOrigCompControl) !== 'undefined'
                                                    && typeof (EventAppt.AMSApptCompControl) !== 'undefined'
                                                    && orderObject.BookOrigCompControl == EventAppt.AMSApptCompControl)
                                                    && (typeof (orderObject.CarrierNumber) !== 'undefined'
                                                        && typeof (EventAppt.AMSApptCarrierSCAC) !== 'undefined'
                                                        && orderObject.CarrierNumber != EventAppt.AMSApptCarrierSCAC
                                                    )) {
                                                    EventAppt.AMSApptControl = 0;
                                                }

                                                if ((typeof (orderObject.BookDestCompControl) !== 'undefined'
                                                    && typeof (EventAppt.AMSApptCompControl) !== 'undefined'
                                                    && orderObject.BookDestCompControl == EventAppt.AMSApptCompControl)
                                                    && (typeof (orderObject.CarrierNumber) !== 'undefined'
                                                        && typeof (EventAppt.AMSApptCarrierSCAC) !== 'undefined'
                                                        && orderObject.CarrierNumber != EventAppt.AMSApptCarrierSCAC
                                                    )) {
                                                    EventAppt.AMSApptControl = 0;
                                                }
                                            }


                                            //} else {
                                            //    EventAppt.AMSApptControl = 0;
                                            //}
                                            // End Modified for v-8.5.2.004 on 07/21/2022
                                            var OrderFilter = {
                                                BookControl: orderObject.BookControl,
                                                AMSCompControl: orderObject.AMSCompControl,
                                                EquipmentID: orderObject.EquipmentID,
                                                BookDateLoad: orderObject.BookDateLoad,
                                                BookDateRequired: orderObject.BookDateRequired,
                                                LaneOriginAddressUse: orderObject.LaneOriginAddressUse,
                                                BookAMSPickupApptControl: orderObject.BookAMSPickupApptControl,
                                                BookAMSDeliveryApptControl: orderObject.BookAMSDeliveryApptControl,
                                                BookOrigCompControl: orderObject.BookOrigCompControl,
                                                BookDestCompControl: orderObject.BookDestCompControl,
                                            };

                                            //Get Grouped Orders By Order
                                            $.ajax({
                                                url: 'api/AMSOrder/GetAMSOrdersGroupedByOrder',
                                                contentType: 'application/json; charset=utf-8',
                                                dataType: 'json',
                                                async: false,
                                                data: { filter: JSON.stringify(OrderFilter) },
                                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                                success: function (data) {
                                                    try {
                                                        var blnSuccess = false;
                                                        var blnErrorShown = false;
                                                        var strValidationMsg = "";
                                                        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                                            if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Get AMSOrdersGrouped Failure", data.Errors, null); }
                                                            else {
                                                                if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                                    if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                                                        blnSuccess = true;
                                                                        if (data.Data.length > 0) { $("#ordersWndGrid").data("kendoGrid").dataSource.data(data.Data); } else { $("#ordersWndGrid").data("kendoGrid").dataSource.data([orderObject]); }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        if (blnSuccess === false && blnErrorShown === false) {
                                                            if (strValidationMsg.length < 1) { strValidationMsg = "Get AMSOrdersGrouped Failure"; }
                                                            ngl.showErrMsg("Get AMSOrdersGrouped Failure", strValidationMsg, null);
                                                        }
                                                    } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                                                }
                                            });
                                            $('#txtStartTime').data('kendoDateTimePicker').enable(true);
                                            $('#txtEndTime').data('kendoDateTimePicker').enable(true);
                                            $("#chkAllDay").prop('checked', false);
                                            if (orderObject.BookDestCompControl == CompControl) { $("#rdoInBound").prop('checked', true); $("#compBound input:radio").attr('disabled', true); } //Modified By LVV on 11/6/2019 Ticket #201911041624
                                            else if (orderObject.BookOrigCompControl == CompControl) { $("#rdoOutBound").prop('checked', true); $("#compBound input:radio").attr('disabled', true); }
                                            $("#txtCarrierName").val(orderObject.CarrierName);
                                            $("#txtSCAC").val(orderObject.CarrierNumber);
                                            $("#txtDescription").val("");
                                            start.value(slot.startDate);
                                            var endTime = new Date(slot.startDate);
                                            endTime.setMinutes(slot.startDate.getMinutes() + DoorObj.AvgApptTime);
                                            end.value(endTime);
                                            // end.value(slot.endDate);
                                            $("#ddlDockDoorID").data("kendoDropDownList").value(doorSlot.doorId);
                                            $("#txtNotes").val("");
                                            carrierDATSchedulerFrom.value(slot.startDate);
                                            carrierDATSchedulerTo.value(slot.endDate);
                                            EventFlag = true;
                                            AvailApptControl = 0;
                                            var alertOrders = $("#ordersWndGrid").data("kendoGrid").dataSource.data();
                                            AlertSCheduleDateMisMatch(alertOrders, slot.startDate);
                                            wndAddEditEvent.center().open();

                                            enableSimpleGridDrag();
                                        }
                                    }
                                }
                            });
                        }


                        function createSchedulerDropArea(scheduler) {
                            // Clean up previous drop targets
                            scheduler.wrapper.find(".k-scheduler-content td").each(function () {
                                var cell = $(this);
                                // Attach Kendo DropTarget to each cell
                                cell.kendoDropTarget({
                                    group: "gridGroup", // same as draggable group
                                    dragenter: function (e) {
                                        cell.addClass("k-state-hover");
                                    },
                                    dragleave: function (e) {
                                        cell.removeClass("k-state-hover");
                                    },
                                    drop: function (e) {
                                        cell.removeClass("k-state-hover");

                                        if (!draggedOrder) return;

                                        // Get scheduler slot info
                                        var slot = scheduler.slotByElement(cell);
                                        if (!slot || !slot.startDate) return;

                                        // 🟢 Create new Scheduler Event (assign order)
                                        scheduler.dataSource.add({
                                            title: "Order " + draggedOrder.BookCarrOrderNumber,
                                            start: slot.startDate,
                                            end: new Date(slot.startDate.getTime() + 60 * 60 * 1000), // +1 hour
                                            doorId: slot.resources && slot.resources.doorId, // adjust field name as needed
                                            BookCarrOrderNumber: draggedOrder.BookCarrOrderNumber,
                                            BookProNumber: draggedOrder.BookProNumber,
                                            BookLoadPONumber: draggedOrder.BookLoadPONumber,
                                            AMSApptCompControl: draggedOrder.BookCompControl
                                        });

                                        // Optional: open scheduler edit popup for confirmation
                                        var added = scheduler.dataSource.data()[scheduler.dataSource.data().length - 1];
                                        scheduler.editEvent(added);
                                    }
                                });
                            });
                        }


                        var subUpdateAppointmentData = {};
                        var subUpdateAppointmentOrders = [];
                        var subUpdateValidation = {};

                        dsScheduler = new kendo.data.SchedulerDataSource({
                            transport: {
                                ServerOperation: true,
                                read: function (options) {
                                    var s = new AllFilter();
                                    s.filterName = Vstart;
                                    s.filterValue = Vend;
                                    s.CompControlFrom = CompControl;
                                    $.ajax({
                                        url: '/api/AMSAppointment/GetRecords/',
                                        contentType: 'application/json; charset=utf-8',
                                        dataType: 'json',
                                        data: { filter: JSON.stringify(s) },
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                        success: function (data) {
                                            options.success(data.Data);
                                            try {
                                                var blnSuccess = false;
                                                var blnErrorShown = false;
                                                var strValidationMsg = "";
                                                if (typeof (data) != 'undefined' && ngl.isObject(data)) {
                                                    if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Get Appointment Details Failure", data.Errors, null); }
                                                    else {
                                                        if (typeof (data.Data) != 'undefined' && ngl.isArray(data.Data)) {
                                                            if (data.Data.length > 0 && typeof (data.Data[0]) != 'undefined' && ngl.isObject(data.Data[0])) { blnSuccess = true; } else { blnSuccess = true; strValidationMsg = "No records were found matching your search criteria"; }
                                                        }
                                                    }
                                                }
                                                if (blnSuccess === false && blnErrorShown === false) {
                                                    if (strValidationMsg.length < 1) { strValidationMsg = "Get Appointment Details Failure"; }
                                                    ngl.showErrMsg("Get Appointment Details Failure", strValidationMsg, null);
                                                }
                                            } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                                        },
                                        error: function (result) { options.error(result); }
                                    });
                                },
                                update: function (options) {
                                    options.data.AMSApptStartDate = kendo.toString(options.data.AMSApptStartDate, "M/d/yyyy HH:mm");
                                    options.data.AMSApptEndDate = kendo.toString(options.data.AMSApptEndDate, "M/d/yyyy HH:mm");
                                    subUpdateAppointmentData = options.data;
                                    AvailApptControl = options.data.AMSApptControl;
                                    $.ajax({
                                        url: '/api/AMSOrder/GetRecordsByAppointmentFilter/',
                                        contentType: 'application/json; charset=utf-8',
                                        dataType: 'json',
                                        async: false,
                                        data: { filter: JSON.stringify({ "filterValue": options.data.AMSApptControl }) },
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                        success: function (data) {
                                            subUpdateAppointmentOrders = data.Data;
                                            try {
                                                var blnSuccess = false;
                                                var blnErrorShown = false;
                                                var strValidationMsg = "";
                                                if (typeof (data) != 'undefined' && ngl.isObject(data)) {
                                                    if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Get Orders Details Failure", data.Errors, null); }
                                                    else {
                                                        if (typeof (data.Data) != 'undefined' && ngl.isArray(data.Data)) {
                                                            if (data.Data.length > 0 && typeof (data.Data[0]) != 'undefined' && ngl.isObject(data.Data[0])) { blnSuccess = true; } else { blnSuccess = true; strValidationMsg = "No records were found matching your search criteria"; }
                                                        }
                                                    }
                                                }
                                                if (blnSuccess === false && blnErrorShown === false) {
                                                    if (strValidationMsg.length < 1) { strValidationMsg = "Get Orders Details Failure"; }
                                                    ngl.showErrMsg("Get Orders Details Failure", strValidationMsg, null);
                                                }
                                            } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                                        },
                                        error: function (result) { ngl.showErrMsg("Get Orders Details Failure", result, null); }
                                    });
                                    subUpdateValidation = {};
                                    var UpdateApptdata = { "Validation": subUpdateValidation, "Appt": options.data, "Ord": subUpdateAppointmentOrders, "Flag": false };
                                    AlertSCheduleDateMisMatch(subUpdateAppointmentOrders, new Date(options.data.AMSApptStartDate));
                                    UpdateAMSAppointment(UpdateApptdata);
                                },
                                create: function (e) { e.success(""); },
                                destroy: function (options) {
                                    $.ajax({
                                        url: 'api/AMSAppointment/DeleteAppointment',
                                        type: 'Post',
                                        contentType: 'application/json; charset=utf-8',
                                        dataType: 'json',
                                        data: JSON.stringify(options.data),
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                        success: function (data) {
                                            try {
                                                var blnSuccess = false;
                                                var blnErrorShown = false;
                                                var strValidationMsg = "";
                                                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                                    if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Delete Appointment Failure", data.Errors, null); }
                                                    else {
                                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                                                blnSuccess = true;
                                                                if (data.Data[0] == false) { ngl.showWarningMsg("Delete Appointment Failure!", "", null); }
                                                            }
                                                        }
                                                    }
                                                }
                                                if (blnSuccess === false && blnErrorShown === false) {
                                                    if (strValidationMsg.length < 1) { strValidationMsg = "Delete Appointment Failure"; }
                                                    ngl.showErrMsg("Delete Appointment Failure", strValidationMsg, null);
                                                }
                                            } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                                            //refresh Scheduler
                                            scheduler.dataSource.read();
                                            $("#grid").data("kendoGrid").dataSource.read();
                                        },
                                        error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Delete Appointment Failure"); ngl.showErrMsg("Delete Appointment Failure", sMsg, null); }
                                    });
                                },
                            },
                            schema: {
                                model: {
                                    id: "AMSApptControl",
                                    fields: {
                                        AMSApptControl: { from: "AMSApptControl", type: "number" },
                                        title: { from: "AMSApptCarrierName" },
                                        start: { type: "date", from: "AMSApptStartDate" },
                                        end: { type: "date", from: "AMSApptEndDate" },
                                        description: { from: "AMSApptDescription" },
                                        recurrenceId: { from: "AMSApptRecurrenceParentControl" },
                                        recurrenceRule: { from: "AMSApptRecurrence" },
                                        doorId: { from: "AMSApptDockdoorID", type: "string", } //Modified By LVV on 8/27/2018 for v-8.3 Scheduler --> The DockDoorID field is a string in the database not an integer
                                    }
                                }
                            }
                        });

                        function UpdateAMSAppointment(UpdateApptdata) {
                            //console.log('Save Appt 2');
                            //console.log(UpdateApptdata);
                            $.ajax({
                                url: '/api/AMSAppointment/SaveUpdateAppointmentOrders/',
                                type: 'Post',
                                contentType: 'application/json; charset=utf-8',
                                dataType: 'json',
                                data: JSON.stringify(UpdateApptdata),
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                success: function (data) {
                                    try {
                                        var blnSuccess = false;
                                        var blnErrorShown = false;
                                        var strValidationMsg = "";
                                        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                            if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Save Appointment Failure", data.Errors, null); }
                                            else {
                                                if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                    if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                                        blnSuccess = true;
                                                        if (data.Data[0].Success !== undefined) {
                                                            if (data.Data[0].Success == false) {
                                                                $("#spanIconMoreDetails").addClass('k-i-minus').removeClass("k-i-plus");
                                                                $("#FailedMsgDetails-UI").show();
                                                                $("#txtWndOverridePassword-validation").addClass("hide-display");
                                                                $("#ddlWndOverrideReasonCode-validation").addClass("hide-display");
                                                                $(".SPRequired_UI").hide();
                                                                $(".RCRequired_UI").hide();
                                                                $(".btnOcerride_UI").hide();
                                                                if (!data.Data[0].NoOverride) {
                                                                    if (data.Data[0].SPRequired) { $("#txtWndOverridePassword").val(""); $(".SPRequired_UI").show(); }
                                                                    if (data.Data[0].RCRequired) { $("#ddlWndOverrideReasonCode").data("kendoDropDownList").select(0); $(".RCRequired_UI").show(); }
                                                                    if (data.Data[0].SPRequired || data.Data[0].RCRequired) { $(".btnOcerride_UI").show(); }
                                                                }
                                                                $("#txtFailedMsg").text(data.Data[0].FailedMsg);
                                                                if (data.Data[0].FailedMsgDetails.length > 0) {
                                                                    $("#txtFailedMsgDetails").html("");
                                                                    $.each(data.Data[0].FailedMsgDetails, function (i, val) {
                                                                        $("#txtFailedMsgDetails").append('<li>' + val + '</li>');
                                                                    });
                                                                }
                                                                $("#viewAvailableSlotsGrid").hide();
                                                                wndOverride.center().open();
                                                                subUpdateValidation = data.Data[0];
                                                            }
                                                        } else { scheduler.dataSource.read(); $("#grid").data("kendoGrid").dataSource.read(); }
                                                    }
                                                }
                                            }
                                        }
                                        if (blnSuccess === false && blnErrorShown === false) {
                                            if (strValidationMsg.length < 1) { strValidationMsg = "Save Appointment Failure"; }
                                            ngl.showErrMsg("Save Appointment Failure", strValidationMsg, null);
                                        }
                                    } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                                },
                                error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure"); ngl.showErrMsg("Save Appointment Failure", sMsg, null); }
                            });
                        }

                        $("#scheduler").on("dblclick", 'td', function (e) {
                            if ($(e.target).hasClass("customNonwork") && scheduler.view().name == "month") {
                                ngl.showWarningMsg("User can't creat Appointment in this time period.!", "");
                                scheduler.edit();
                            }
                        });

                        var stDate = "2018/5/15 06:00";
                        var endDate = "2018/5/15 18:00";

                        var stateScrollOnOff = true;
                        //***********scheduler View***************//
                        var scheduler = $("#scheduler").kendoScheduler({
                            toolbar: ["pdf"],
                            pdf: { fileName: "Scheduler.pdf" },
                            height: parseInt($('#center-pane').css('height')) - 45,  // modified by RHR for v-8.3.0.001 height factor changed to -45
                            date: kendo.date.today(),
                            timezone: "Etc/UTC",
                            workDayStart: new Date(stDate),
                            workDayEnd: new Date(endDate),
                            showWorkHours: true,
                            workWeekEnd: 5,
                            //allDaySlot: false,
                            //footer: false,
                            resize: function (e) { if (e.event.AMSApptControl == 0) { e.preventDefault(); } },
                            moveStart: function (e) { if (e.event.AMSApptControl == 0) { e.preventDefault(); } },
                            add: function (e) {
                                //if (!checkAvailability(e.event.start, e.event.end, e.event)) {
                                //    e.preventDefault();
                                //}
                            },
                            editable: {
                                editRecurringMode: "occurrence" // - directly create exception
                                //editRecurringMode: "series" - directly edit the series
                                //editRecurringMode: "dialog" - default, shows the regular dialog
                            },
                            edit: function (e) {
                                e.preventDefault();
                                var event = e.event;

                                if (event.AMSApptCompControl != undefined) {
                                    if (event.AMSApptControl > 0) { AddEditFunc(e); } else { ngl.showWarningMsg("User cannot schedule appointment in this time slot!", ""); }
                                } else { AddEditFunc(e); }
                            },
                            views: [
                                { type: "day", selected: true, group: { resources: ["Doors"], orientation: "horizontal", date: true } },
                                { type: "week", selected: false, group: { resources: ["Doors"], orientation: "horizontal", date: true } },
                                "month",
                                { type: "timeline", selected: false, group: { resources: ["Doors"], orientation: "horizontal", date: true } }
                            ],
                            eventTemplate: "<div class='event-templatediv'>#if (data.AMSApptLabel) {#<p>#=AMSApptLabel#</p>#}#</div>",
                            dataSource: dsScheduler,
                            navigate: function (e) {
                                //*******Permission to Refresh the Scheduler*********//
                                cllAPI = true;
                            },
                            dataBound: function (e) {
                                var view = this.view();
                                var events = this.dataItems();
                                var eventElement;
                                var event;
                                for (var idx = 0, length = events.length; idx < length; idx++) {
                                    event = events[idx];
                                    eventElement = view.element.find("[data-uid=" + event.uid + "]"); //get event element                    
                                    //set the backgroud of the element
                                    //menu.append([{ text: "Delete", attr:{'data-id':'DeleteEvent'}, cssClass: "myClass2" }]);                           
                                    if (ngl.isNullOrUndefined(event.CompAMSColorCodeSettingColorCode) || event.CompAMSColorCodeSettingColorCode.length < 1) {
                                        //if CompAMSColorCodeSettingColorCode is null use defaults
                                        switch (event.AMSApptStatusCode) {

                                            case 4: //CheckedIn
                                                eventElement.addClass("yellow");
                                                break;
                                            case 7:  //CheckedOut
                                                eventElement.addClass("green");
                                                break;

                                            //Previous code snippet - Starts (20/11/2025)
                                            //case 2:
                                            //    eventElement.addClass("gray");
                                            //    break;
                                            //case 4: //CheckedIn
                                            //    eventElement.addClass("green");
                                            //    break;
                                            //case 5: //StartLoad_Unload
                                            //    eventElement.addClass("darkblue");
                                            //    break;
                                            //case 6: //FinishLoad_Unload
                                            //    eventElement.addClass("yellow");
                                            //    break;
                                            //case 7: //CheckedOut
                                            //    eventElement.addClass("darkred");
                                            //    break;
                                            //case 9:
                                            //case 10:
                                            //case 11:
                                            //case 12:
                                            //case 13:
                                            //case 14:
                                            //    eventElement.addClass("gray");
                                            //    break;
                                            //Previous code snippet - Ends (20/11/2025)

                                            default:
                                                eventElement.css("background-color", "#" + event.color);
                                        }
                                    } else { eventElement.css("background-color", "#" + event.CompAMSColorCodeSettingColorCode); } //else use the color sent back in CompAMSColorCodeSettingColorCode                                            
                                }
                                //create drop area from current View
                                createDropArea(this);

                                //createSchedulerDropArea
                                createSchedulerDropArea(this);

                                scheduler_view_range(e);
                                if (!$("#scrollOnOff").is(":visible")) {
                                    $('<input type="checkbox" id="scrollOnOff"  name="scrollOnOffBOX"><label class="k-checkbox-label" for="scrollOnOff">Auto Scroll</label>').appendTo(this.wrapper.find(".k-scheduler-footer"));
                                    $("#scrollOnOff").prop("checked", stateScrollOnOff);
                                }
                            },
                            resources: [
                                {
                                    field: "doorId",
                                    name: "Doors",
                                    dataValueField: "CompDockDockDoorID",
                                    dataTextField: "CompDockDockDoorName",
                                    dataSource: [],
                                    title: "CompDockDockDoorName"
                                },
                            ]
                        }).data("kendoScheduler");

                        //*************Scheduler Auto Scroll ON/OFF*********//
                        $(document).on("change", "input[name='scrollOnOffBOX']", function () {
                            if ($(this).is(":checked")) {
                                //hide the navigation bars when autoScroll is on (Added By LVV 1/4/19)
                                $('.k-scheduler-navigation').hide();
                                $('.k-scheduler-views').hide();
                                stateScrollOnOff = true;
                                var currentTime = new Date();
                                var hours = currentTime.getHours();
                                var minutes = currentTime.getMinutes();
                                if (minutes >= 30) { minutes = 30; } else { minutes = 0; }
                                scrollToCurrentTime(hours, minutes);
                            } else {
                                //show the navigation bars when autoScroll is off (Added By LVV 1/4/19)
                                $('.k-scheduler-navigation').show();
                                $('.k-scheduler-views').show();
                                stateScrollOnOff = false;
                            }
                            //***Save PageSettings*******//
                            InsertOrUpdateCurrentUserPageSetting("SchedulerPage");
                            //Added by RHR for v=8.3.0.002 on 11/18/2020 we now resize when this event takes place
                            resizeSWidget(iLastHeightFactor);

                        });

                        //************Scheduler Scroll************//
                        var autoScrollMins = '<%=ConfigurationManager.AppSettings["AutoScrollMins"] %>';
                        var autoScrollMilSec = 1000 * 60 * autoScrollMins;
                        window.setInterval(function () {
                            var currentTime = new Date();
                            var hours = currentTime.getHours();
                            var minutes = currentTime.getMinutes();
                            if (minutes >= 30) { minutes = 30; } else { minutes = 0; }
                            if (stateScrollOnOff) { scrollToCurrentTime(hours, minutes); }
                        }, autoScrollMilSec);

                        function scrollToCurrentTime(hours, minutes) {
                            var time = new Date();
                            time.setHours(hours);
                            time.setMinutes(minutes);
                            time.setSeconds(0);
                            time.setMilliseconds(0);
                            $("#scheduler").data("kendoScheduler").dataSource.read();
                            var scheduler = $("#scheduler").getKendoScheduler();
                            scheduler.view("day"); //If autoscroll is on scheduler must show Day View (Added By LVV 1/4/19)
                            var contentDiv = scheduler.element.find("div.k-scheduler-content");
                            var contentDivPosition = contentDiv.position();
                            var rows = contentDiv.find("tr");
                            for (var i = 0; i < rows.length; i++) {
                                var slot = scheduler.slotByElement(rows[i]);
                                var slotTime = kendo.toString(slot.startDate, "HH:mm");
                                var targetTime = kendo.toString(time, "HH:mm");
                                if (targetTime === slotTime) {
                                    var eventDiv = $(rows[i - 4]);
                                    //Modified by RHR for v-8.2 on 09/16/2018
                                    //fixed bug in function scrollToCurrentTime(hours,minutes) {
                                    //if var eventDivOffset = eventDiv.offset(); is null the contentDiv.scrollTop throws a null reference exception.
                                    //We now check for null eventDivOffset  and skip the scroll.
                                    //If this is not the desired effect the code for determining the eventDivOffset should be changed
                                    var eventDivOffset = eventDiv.offset();
                                    if (!eventDivOffset) { return ""; }
                                    contentDiv.scrollTop(eventDivOffset.top + contentDiv.scrollTop() - contentDivPosition.top);
                                    return "";
                                }
                            }
                        };

                        //*******Work Hours*********//
                        var startTimesofWeek = [];
                        var endTimesofWeek = [];
                        var minWeekTime;
                        var maxWeekTime;
                        function workHours() {
                            if (scheduler.view().name == "day") {
                                var day = scheduler.view()._startDate.getDay();
                                scheduler.options.workDayStart = new Date("2018/10/10 " + startTimesofWeek[day]);
                                scheduler.options.workDayEnd = new Date("2018/10/10 " + endTimesofWeek[day]);
                            } else if (scheduler.view().name == "week") { scheduler.options.workDayStart = new Date("2018/10/10 " + minWeekTime); scheduler.options.workDayEnd = new Date("2018/10/10 " + maxWeekTime); }
                        }

                        function startEndDockDoorsTimes() {
                            startTimesofWeek = [];
                            endTimesofWeek = [];
                            for (var day = 0; day < 7; day++) {
                                var minTime = "23:59:59";
                                switch (day) {
                                    case 0:
                                        $.each(dsAllDockDoorsApptTimeSettings, function (i, val) {
                                            if (minTime > kendo.toString(new Date(val.SunStart), "HH:mm:ss")) { minTime = kendo.toString(new Date(val.SunStart), "HH:mm:ss"); }
                                        });
                                        break;
                                    case 1:
                                        $.each(dsAllDockDoorsApptTimeSettings, function (i, val) {
                                            if (minTime > kendo.toString(new Date(val.MonStart), "HH:mm:ss")) { minTime = kendo.toString(new Date(val.MonStart), "HH:mm:ss"); }
                                        });
                                        break;
                                    case 2:
                                        $.each(dsAllDockDoorsApptTimeSettings, function (i, val) {
                                            if (minTime > kendo.toString(new Date(val.TueStart), "HH:mm:ss")) { minTime = kendo.toString(new Date(val.TueStart), "HH:mm:ss"); }
                                        });
                                        break;
                                    case 3:
                                        $.each(dsAllDockDoorsApptTimeSettings, function (i, val) {
                                            if (minTime > kendo.toString(new Date(val.WedStart), "HH:mm:ss")) { minTime = kendo.toString(new Date(val.WedStart), "HH:mm:ss"); }
                                        });
                                        break;
                                    case 4:
                                        $.each(dsAllDockDoorsApptTimeSettings, function (i, val) {
                                            if (minTime > kendo.toString(new Date(val.ThuStart), "HH:mm:ss")) { minTime = kendo.toString(new Date(val.ThuStart), "HH:mm:ss"); }
                                        });
                                        break;
                                    case 5:
                                        $.each(dsAllDockDoorsApptTimeSettings, function (i, val) {
                                            if (minTime > kendo.toString(new Date(val.FriStart), "HH:mm:ss")) { minTime = kendo.toString(new Date(val.FriStart), "HH:mm:ss"); }
                                        });
                                        break;
                                    case 6:
                                        $.each(dsAllDockDoorsApptTimeSettings, function (i, val) {
                                            if (minTime > kendo.toString(new Date(val.SatStart), "HH:mm:ss")) { minTime = kendo.toString(new Date(val.SatStart), "HH:mm:ss"); }
                                        });
                                        break;
                                }
                                startTimesofWeek.push(minTime);
                                var maxTime = "00:00:00";
                                switch (day) {
                                    case 0:
                                        $.each(dsAllDockDoorsApptTimeSettings, function (i, val) {
                                            if (maxTime < kendo.toString(new Date(val.SunEnd), "HH:mm:ss")) { maxTime = kendo.toString(new Date(val.SunEnd), "HH:mm:ss"); }
                                        });
                                        break;
                                    case 1:
                                        $.each(dsAllDockDoorsApptTimeSettings, function (i, val) {
                                            if (maxTime < kendo.toString(new Date(val.MonEnd), "HH:mm:ss")) { maxTime = kendo.toString(new Date(val.MonEnd), "HH:mm:ss"); }
                                        });
                                        break;
                                    case 2:
                                        $.each(dsAllDockDoorsApptTimeSettings, function (i, val) {
                                            if (maxTime < kendo.toString(new Date(val.TueEnd), "HH:mm:ss")) { maxTime = kendo.toString(new Date(val.TueEnd), "HH:mm:ss"); }
                                        });
                                        break;
                                    case 3:
                                        $.each(dsAllDockDoorsApptTimeSettings, function (i, val) {
                                            if (maxTime < kendo.toString(new Date(val.WedEnd), "HH:mm:ss")) { maxTime = kendo.toString(new Date(val.WedEnd), "HH:mm:ss"); }
                                        });
                                        break;
                                    case 4:
                                        $.each(dsAllDockDoorsApptTimeSettings, function (i, val) {
                                            if (maxTime < kendo.toString(new Date(val.ThuEnd), "HH:mm:ss")) { maxTime = kendo.toString(new Date(val.ThuEnd), "HH:mm:ss"); }
                                        });
                                        break;
                                    case 5:
                                        $.each(dsAllDockDoorsApptTimeSettings, function (i, val) {
                                            if (maxTime < kendo.toString(new Date(val.FridEnd), "HH:mm:ss")) { maxTime = kendo.toString(new Date(val.FridEnd), "HH:mm:ss"); }
                                        });
                                        break;
                                    case 6:
                                        $.each(dsAllDockDoorsApptTimeSettings, function (i, val) {
                                            if (maxTime < kendo.toString(new Date(val.SatEnd), "HH:mm:ss")) { maxTime = kendo.toString(new Date(val.SatEnd), "HH:mm:ss"); }
                                        });
                                        break;
                                }
                                endTimesofWeek.push(maxTime);
                            }
                            minWeekTime = "23:59:59";
                            $.each(startTimesofWeek, function (i, tval) {
                                if (minWeekTime > tval) { minWeekTime = tval; }
                            });
                            maxWeekTime = "00:00:00";
                            $.each(endTimesofWeek, function (i, tval) {
                                if (maxWeekTime < tval) { maxWeekTime = tval; }
                            });
                        }

                        //*****Scheduler Gray Out Functions******//
                        var myHours = [
                            { Open: true, Start: 6, End: 18 },
                            { Open: true, Start: 6, End: 18 },
                            { Open: true, Start: 6, End: 18 },
                            { Open: true, Start: 6, End: 18 },
                            { Open: true, Start: 6, End: 18 },
                            { Open: true, Start: 6, End: 18 },
                            { Open: true, Start: 6, End: 18 }
                        ];

                        function BlockOffTime() {
                            if (scheduler.view().name == "day") {
                                var day = scheduler.view()._startDate.getDay();
                                $.each(dsAllDockDoorsApptTimeSettings, function (index, val) {
                                    var tdPosition = index + 1;
                                    switch (day) {
                                        case 0:
                                            myHours[day].Start = kendo.toString(new Date(val.SunStart), "HH.mm");
                                            myHours[day].End = kendo.toString(new Date(val.SunEnd), "HH.mm");
                                            $(".k-scheduler-table tr td:nth-child(" + tdPosition + ")").each(function (i, item) {
                                                var slot = scheduler.slotByElement(item);
                                                if (!isBusinessHour(slot)) { $(item).addClass("customNonwork"); }
                                            });
                                            break;
                                        case 1:
                                            myHours[day].Start = kendo.toString(new Date(val.MonStart), "HH.mm");
                                            myHours[day].End = kendo.toString(new Date(val.MonEnd), "HH.mm");
                                            $(".k-scheduler-table tr td:nth-child(" + tdPosition + ")").each(function (i, item) {
                                                var slot = scheduler.slotByElement(item);
                                                if (!isBusinessHour(slot)) { $(item).addClass("customNonwork"); }
                                            });
                                            break;
                                        case 2:
                                            myHours[day].Start = kendo.toString(new Date(val.TueStart), "HH.mm");
                                            myHours[day].End = kendo.toString(new Date(val.TueEnd), "HH.mm");
                                            $(".k-scheduler-table tr td:nth-child(" + tdPosition + ")").each(function (i, item) {
                                                var slot = scheduler.slotByElement(item);
                                                if (!isBusinessHour(slot)) { $(item).addClass("customNonwork"); }
                                            });
                                            break;
                                        case 3:
                                            myHours[day].Start = kendo.toString(new Date(val.WedStart), "HH.mm");
                                            myHours[day].End = kendo.toString(new Date(val.WedEnd), "HH.mm");
                                            $(".k-scheduler-table tr td:nth-child(" + tdPosition + ")").each(function (i, item) {
                                                var slot = scheduler.slotByElement(item);
                                                if (!isBusinessHour(slot)) { $(item).addClass("customNonwork"); }
                                            });
                                            break;
                                        case 4:
                                            myHours[day].Start = kendo.toString(new Date(val.ThuStart), "HH.mm");
                                            myHours[day].End = kendo.toString(new Date(val.ThuEnd), "HH.mm");
                                            $(".k-scheduler-table tr td:nth-child(" + tdPosition + ")").each(function (i, item) {
                                                var slot = scheduler.slotByElement(item);
                                                if (!isBusinessHour(slot)) { $(item).addClass("customNonwork"); }
                                            });
                                            break;
                                        case 5:
                                            myHours[day].Start = kendo.toString(new Date(val.FriStart), "HH.mm");
                                            myHours[day].End = kendo.toString(new Date(val.FridEnd), "HH.mm");
                                            $(".k-scheduler-table tr td:nth-child(" + tdPosition + ")").each(function (i, item) {
                                                var slot = scheduler.slotByElement(item);
                                                if (!isBusinessHour(slot)) { $(item).addClass("customNonwork"); }
                                            });
                                            break;
                                        case 6:
                                            myHours[day].Start = kendo.toString(new Date(val.SatStart), "HH.mm");
                                            myHours[day].End = kendo.toString(new Date(val.SatEnd), "HH.mm");
                                            $(".k-scheduler-table tr td:nth-child(" + tdPosition + ")").each(function (i, item) {
                                                var slot = scheduler.slotByElement(item);
                                                if (!isBusinessHour(slot)) { $(item).addClass("customNonwork"); }
                                            });
                                            break;
                                    }
                                });
                            }
                            else if (scheduler.view().name == "week") {
                                var tdPosition = 0;
                                for (var day = 0; day < 7; day++) {
                                    $.each(dsAllDockDoorsApptTimeSettings, function (index, val) {
                                        tdPosition++;
                                        switch (day) {
                                            case 0:
                                                myHours[day].Start = kendo.toString(new Date(val.SunStart), "HH.mm");
                                                myHours[day].End = kendo.toString(new Date(val.SunEnd), "HH.mm");
                                                $(".k-scheduler-table tr td:nth-child(" + tdPosition + ")").each(function (i, item) {
                                                    var slot = scheduler.slotByElement(item);
                                                    if (!isBusinessHour(slot)) { $(item).addClass("customNonwork"); }
                                                });
                                                break;
                                            case 1:
                                                myHours[day].Start = kendo.toString(new Date(val.MonStart), "HH.mm");
                                                myHours[day].End = kendo.toString(new Date(val.MonEnd), "HH.mm");
                                                $(".k-scheduler-table tr td:nth-child(" + tdPosition + ")").each(function (i, item) {
                                                    var slot = scheduler.slotByElement(item);
                                                    if (!isBusinessHour(slot)) { $(item).addClass("customNonwork"); }
                                                });
                                                break;
                                            case 2:
                                                myHours[day].Start = kendo.toString(new Date(val.TueStart), "HH.mm");
                                                myHours[day].End = kendo.toString(new Date(val.TueEnd), "HH.mm");
                                                $(".k-scheduler-table tr td:nth-child(" + tdPosition + ")").each(function (i, item) {
                                                    var slot = scheduler.slotByElement(item);
                                                    if (!isBusinessHour(slot)) { $(item).addClass("customNonwork"); }
                                                });
                                                break;
                                            case 3:
                                                myHours[day].Start = kendo.toString(new Date(val.WedStart), "HH.mm");
                                                myHours[day].End = kendo.toString(new Date(val.WedEnd), "HH.mm");
                                                $(".k-scheduler-table tr td:nth-child(" + tdPosition + ")").each(function (i, item) {
                                                    var slot = scheduler.slotByElement(item);
                                                    if (!isBusinessHour(slot)) { $(item).addClass("customNonwork"); }
                                                });
                                                break;
                                            case 4:
                                                myHours[day].Start = kendo.toString(new Date(val.ThuStart), "HH.mm");
                                                myHours[day].End = kendo.toString(new Date(val.ThuEnd), "HH.mm");
                                                $(".k-scheduler-table tr td:nth-child(" + tdPosition + ")").each(function (i, item) {
                                                    var slot = scheduler.slotByElement(item);
                                                    if (!isBusinessHour(slot)) { $(item).addClass("customNonwork"); }
                                                });
                                                break;
                                            case 5:
                                                myHours[day].Start = kendo.toString(new Date(val.FriStart), "HH.mm");
                                                myHours[day].End = kendo.toString(new Date(val.FridEnd), "HH.mm");
                                                $(".k-scheduler-table tr td:nth-child(" + tdPosition + ")").each(function (i, item) {
                                                    var slot = scheduler.slotByElement(item);
                                                    if (!isBusinessHour(slot)) { $(item).addClass("customNonwork"); }
                                                });
                                                break;
                                            case 6:
                                                myHours[day].Start = kendo.toString(new Date(val.SatStart), "HH.mm");
                                                myHours[day].End = kendo.toString(new Date(val.SatEnd), "HH.mm");
                                                $(".k-scheduler-table tr td:nth-child(" + tdPosition + ")").each(function (i, item) {
                                                    var slot = scheduler.slotByElement(item);
                                                    if (!isBusinessHour(slot)) { $(item).addClass("customNonwork"); }
                                                });
                                                break;
                                        }
                                    });
                                }
                            }
                            else if (scheduler.view().name == "month") {
                                $(".k-scheduler-table  td").each(function (i, item) {
                                    var slot = scheduler.slotByElement(item);
                                    if (!isBusinessHour(slot)) { $(item).addClass("customNonwork"); }
                                });
                            }
                            else if (scheduler.view().name == "timelineeee") {
                                var day = scheduler.view()._startDate.getDay();
                                var tdPosition = 0;
                                $.each(dsAllDockDoorsApptTimeSettings, function (index, val) {
                                    tdPosition++;
                                    switch (day) {
                                        case 0:
                                            myHours[day].Start = kendo.toString(new Date(val.SunStart), "HH.mm");
                                            myHours[day].End = kendo.toString(new Date(val.SunEnd), "HH.mm");
                                            $(".k-scheduler-table tr td:nth-child(" + tdPosition + ")").each(function (i, item) {
                                                var slot = scheduler.slotByElement(item);
                                                if (!isBusinessHour(slot)) { $(item).addClass("customNonwork"); }
                                            });
                                            break;
                                        case 1:
                                            myHours[day].Start = kendo.toString(new Date(val.MonStart), "HH.mm");
                                            myHours[day].End = kendo.toString(new Date(val.MonEnd), "HH.mm");
                                            $(".k-scheduler-table tr td:nth-child(" + tdPosition + ")").each(function (i, item) {
                                                var slot = scheduler.slotByElement(item);
                                                if (!isBusinessHour(slot)) { $(item).addClass("customNonwork"); }
                                            });
                                            break;
                                        case 2:
                                            myHours[day].Start = kendo.toString(new Date(val.TueStart), "HH.mm");
                                            myHours[day].End = kendo.toString(new Date(val.TueEnd), "HH.mm");
                                            $(".k-scheduler-table tr td:nth-child(" + tdPosition + ")").each(function (i, item) {
                                                var slot = scheduler.slotByElement(item);
                                                if (!isBusinessHour(slot)) { $(item).addClass("customNonwork"); }
                                            });
                                            break;
                                        case 3:
                                            myHours[day].Start = kendo.toString(new Date(val.WedStart), "HH.mm");
                                            myHours[day].End = kendo.toString(new Date(val.WedEnd), "HH.mm");
                                            $(".k-scheduler-table tr td:nth-child(" + tdPosition + ")").each(function (i, item) {
                                                var slot = scheduler.slotByElement(item);
                                                if (!isBusinessHour(slot)) { $(item).addClass("customNonwork"); }
                                            });
                                            break;
                                        case 4:
                                            myHours[day].Start = kendo.toString(new Date(val.ThuStart), "HH.mm");
                                            myHours[day].End = kendo.toString(new Date(val.ThuEnd), "HH.mm");
                                            $(".k-scheduler-table tr td:nth-child(" + tdPosition + ")").each(function (i, item) {
                                                var slot = scheduler.slotByElement(item);
                                                if (!isBusinessHour(slot)) { $(item).addClass("customNonwork"); }
                                            });
                                            break;
                                        case 5:
                                            myHours[day].Start = kendo.toString(new Date(val.FriStart), "HH.mm");
                                            myHours[day].End = kendo.toString(new Date(val.FridEnd), "HH.mm");
                                            $(".k-scheduler-table tr td:nth-child(" + tdPosition + ")").each(function (i, item) {
                                                var slot = scheduler.slotByElement(item);
                                                if (!isBusinessHour(slot)) { $(item).addClass("customNonwork"); }
                                            });
                                            break;
                                        case 6:
                                            myHours[day].Start = kendo.toString(new Date(val.SatStart), "HH.mm");
                                            myHours[day].End = kendo.toString(new Date(val.SatEnd), "HH.mm");
                                            $(".k-scheduler-table tr td:nth-child(" + tdPosition + ")").each(function (i, item) {
                                                var slot = scheduler.slotByElement(item);
                                                if (!isBusinessHour(slot)) { $(item).addClass("customNonwork"); }
                                            });
                                            break;
                                    }
                                });
                            }
                        }

                        function isBusinessHour(slot) {
                            var businessDay = myHours[slot.startDate.getDay()];
                            if (businessDay.Open) {
                                var slotStart = parseFloat(slot.startDate.getHours() + "." + slot.startDate.getMinutes());
                                var slotEnd = parseFloat(slot.endDate.getHours() + "." + slot.endDate.getMinutes());
                                if (slotStart >= businessDay.Start && slotEnd <= businessDay.End && slotEnd != 0) {
                                    return true; //business hour
                                } else { return false; } //non-business hour
                            } else { return false; } //Closed all day
                        }

                        $("#scheduler").kendoTooltip({
                            filter: ".k-event:not(.k-event-drag-hint) > div, .k-task",
                            position: "top",
                            width: 250,
                            content: function (e) {
                                var target = e.target;
                                var element = target.is(".k-task") ? target : target.parent();
                                var uid = element.attr("data-uid");
                                var scheduler = target.closest("[data-role=scheduler]").data("kendoScheduler");
                                var events = scheduler.occurrenceByUid(uid);
                                var content = "";
                                content = content + "<div><strong>Starts On: </strong>" + kendo.toString(events.start, 'MM/dd/yyyy HH:mm') + "<br /><div><strong>Ends On: </strong>" + kendo.toString(events.end, 'MM/dd/yyyy HH:mm') + "<br /><hr><div> " + events.AMSApptHover + "</div>";
                                return content == "" ? "No events" : content;
                            }
                        });
                        //debugger;
                        //var dsUserPageSettings = {};
                        //Modified by RHR for v-8.2 on 09/15/2018 
                        //added logic to call SchedulerController's GetPageSettings API
                        var dsUserPageSettings = null;
                        var sKey = "SchedulerPage";
                        $.ajax({
                            url: '/api/Scheduler/GetPageSettings/' + sKey,
                            contentType: 'application/json; charset=utf-8',
                            dataType: 'json',
                            async: false,
                            data: { filter: sKey },
                            headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                            success: function (data) {
                                //debugger;
                                dsUserPageSettings = data.Data[0];
                                try {
                                    var blnSuccess = false;
                                    var blnErrorShown = false;
                                    var strValidationMsg = "";
                                    if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                        if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Get User Page Settings Failure", data.Errors, null); }
                                        else {
                                            if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) { blnSuccess = true; }
                                            }
                                        }
                                    }
                                    if (blnSuccess === false && blnErrorShown === false) {
                                        if (strValidationMsg.length < 1) { strValidationMsg = "If this is your first time on this page your settings will be saved for your next visit, if not please contact technical support if you continue to recieve thsi message."; }
                                        ngl.showInfoNotification("Unable to Read Page Settings", strValidationMsg, null);
                                    }
                                } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                            }
                        });
                        //$.ajax({ 
                        //    url: '/api/UserPageSetting/GetPageSettingsForCurrentUser/', 
                        //    contentType: 'application/json; charset=utf-8', 
                        //    dataType: 'json', 
                        //    async: false,
                        //    data: { id : PageControl},
                        //    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                        //    success: function(data) {
                        //        debugger;
                        //        dsUserPageSettings =data.Data[0]; 
                        //        try {                                
                        //            var blnSuccess = false;
                        //            var blnErrorShown = false;
                        //            var strValidationMsg = "";
                        //            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                        //                if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                        //                    blnErrorShown = true;
                        //                    ngl.showErrMsg("Get User Page Settings Failure", data.Errors, null);
                        //                }
                        //                else {
                        //                    if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                        //                        if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                        //                            blnSuccess = true;
                        //                        }
                        //                    }
                        //                    //if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                        //                    //    if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                        //                    //        //this function returns all settings for the page
                        //                    //        //we need to loop through each and find the one where 
                        //                    //        //UserPSName = SchedulerPage                                                              
                        //                    //        $.each(data.Data, function (index, item) {
                        //                    //            if (item.UserPSName == "SchedulerPage"){                             
                        //                    //                dsUserPageSettings = item;   
                        //                    //                blnSuccess = true;
                        //                    //                return;
                        //                    //            }                        
                        //                    //        });   
                        //                    //    }
                        //                    //}
                        //                }
                        //            }
                        //            if (blnSuccess === false && blnErrorShown === false) {
                        //                if (strValidationMsg.length < 1) { strValidationMsg = "Get User Page Settings Failure"; }
                        //                ngl.showErrMsg("Get User Page Settings Failure", strValidationMsg, null);
                        //            }
                        //        } catch (err) {
                        //            ngl.showErrMsg(err.name, err.description, null);
                        //        }
                        //    } 
                        //}); 
                        //debugger;
                        //Modified by RHR for v-8.2 on 09/15/2018 
                        //we now use PageSetting Model (name/value) 
                        //if(typeof (dsUserPageSettings) !== 'undefined' && dsUserPageSettings != null && dsUserPageSettings.UserPSMetaData != undefined){
                        //    //***********UserSettingPage View************//
                        //    var psData = JSON.parse(dsUserPageSettings.UserPSMetaData);
                        //debugger;
                        if (typeof (dsUserPageSettings) !== 'undefined' && dsUserPageSettings != null && dsUserPageSettings.value != undefined) {
                            //***********UserSettingPage View************//
                            var psData = JSON.parse(dsUserPageSettings.value);
                            CompControl = psData.CompanyID;
                            $("#ddlwarehouse").data("kendoDropDownList").value(CompControl);
                            $("#scrollOnOff").prop("checked", psData.ScrollOnOff);
                            stateScrollOnOff = psData.ScrollOnOff;
                            if (stateScrollOnOff === true) {
                                //hide the navigation bars when autoScroll is on (Added By LVV 1/4/19)
                                $('.k-scheduler-navigation').hide();
                                $('.k-scheduler-views').hide();
                            }
                            else { $('.k-scheduler-navigation').show(); $('.k-scheduler-views').show(); } //show the navigation bars when autoScroll is off (Added By LVV 1/4/19)
                            $('#scheduler').hide();
                            $.ajax({
                                url: '/api/AMSCompDockDoor/GetRecords/',
                                contentType: 'application/json; charset=utf-8',
                                dataType: 'json',
                                async: false,
                                data: { filter: JSON.stringify({ "filterName": "CompDockCompControl", "filterValue": CompControl, "page": 1, "skip": 0, "take": 100 }) },
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                success: function (data) {
                                    //debugger;
                                    res = data.Data;
                                    try {
                                        var blnSuccess = false;
                                        var blnErrorShown = false;
                                        var strValidationMsg = "";
                                        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {

                                            if (ngl.stringHasValue(data.Errors)) {
                                                blnErrorShown = true;
                                                ngl.showErrMsg("Unexpected Read Dock Doors Failure", data.Errors, null);
                                                //Modified by RHR for v-8.3.0.001 on 09/01/2020  fix bug when no data is returned we now return on error
                                                return;
                                            }
                                            else {
                                                if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                    if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) { blnSuccess = true; } else {
                                                        //Modified by RHR for v-8.3.0.001 on 09/01/2020  set success to false

                                                        blnSuccess = false;
                                                        strValidationMsg = "Please check that at least one dock door is configured for the selected warehouse.";
                                                    }
                                                }
                                            }
                                        }
                                        if (blnSuccess === false && blnErrorShown === false) {
                                            if (strValidationMsg.length < 1) { strValidationMsg = "Get Dock Doors Failure"; }
                                            ngl.showErrMsg("Cannot locate your dock doors", strValidationMsg, null);
                                            //Modified by RHR for v-8.3.0.001 on 09/01/2020  fix bug when no data is returned we now return on error
                                            return;
                                        }
                                    } catch (err) {
                                        ngl.showErrMsg(err.name, err.description, null);
                                        //Modified by RHR for v-8.3.0.001 on 09/01/2020  fix bug when no data is returned we now return on error
                                        return;
                                    }
                                }
                            });
                            var viewDoors = [];
                            $.each(psData.ViewDockDoors, function (i, value) {
                                $.each(res, function (k, val) {
                                    if (value == val.CompDockControl) { viewDoors.push(val); }
                                });
                            });
                            $('#submenu').empty();
                            for (var i = 0; i < res.length; i++) {
                                var presentDockDoor = false;
                                $.each(viewDoors, function (index, val) {
                                    if (val.CompDockControl == res[i].CompDockControl) { presentDockDoor = true; }
                                });
                                if (presentDockDoor) {
                                    $('#submenu').append('<input id="' + res[i].CompDockControl + '" value="' + res[i].CompDockControl + '" style="margin-left:10px"  type="checkbox" checked><label for="' + res[i].CompDockControl + '" class="k-checkbox-label">' + res[i].CompDockDockDoorName + '</label>');
                                } else { $('#submenu').append('<input id="' + res[i].CompDockControl + '" value="' + res[i].CompDockControl + '" style="margin-left:10px"   type="checkbox" ><label for="' + res[i].CompDockControl + '" class="k-checkbox-label">' + res[i].CompDockDockDoorName + '</label>'); }
                            }
                            if (typeof (viewDoors) !== 'undefined' && ngl.isArray(viewDoors) && viewDoors.length > 0) {
                                $('#scheduler').show();
                                //Modified by RHR for v-8.3.0.001 on 09/01/2020  fix bug when no data is returned we now return on error
                                scheduler.resources[0].dataSource.data(viewDoors);
                                scheduler.view(psData.CalenderView);
                                scheduler.date(new Date(psData.CalenderDate));
                                for (var z = 0; z < res.length; z++) {
                                    var s = new AllFilter();
                                    s.filterName = 'dockDoorControl';
                                    s.filterValue = res[z].CompDockControl;
                                    $.ajax({
                                        url: '/api/AMSCompDockdoor/GetDockApptTimeSettings/',
                                        contentType: 'application/json; charset=utf-8',
                                        dataType: 'json',
                                        async: false,
                                        data: { filter: JSON.stringify(s) },
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                        success: function (data) { if (data.Data != null) { allDockDoorsSettingsArray.push(data.Data[0]); } }
                                    });
                                }
                                $.each(psData.ViewDockDoors, function (i, objval) {
                                    $.each(allDockDoorsSettingsArray, function (sI, sObj) {
                                        if (objval == sObj.DockControl) { dsAllDockDoorsApptTimeSettings.push(sObj); }
                                    });
                                });
                                startEndDockDoorsTimes();
                                workHours();
                                scheduler.view(scheduler.view().name);
                                BlockOffTime();
                            }

                        } else {
                            //***********Deafult drop down warehouse doors loding************//
                            $('#scheduler').hide();
                            $.ajax({
                                url: '/api/AMSCompDockDoor/GetRecords/',
                                contentType: 'application/json; charset=utf-8',
                                dataType: 'json',
                                async: false,
                                data: { filter: JSON.stringify({ "filterName": "CompDockCompControl", "filterValue": CompControl, "page": 1, "skip": 0, "take": 100 }) },
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                success: function (data) {
                                    //debugger;
                                    res = data.Data;
                                    try {
                                        var blnSuccess = false;
                                        var blnErrorShown = false;
                                        var strValidationMsg = "";
                                        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                            if (ngl.stringHasValue(data.Errors)) {
                                                blnErrorShown = true;
                                                ngl.showErrMsg("Unexpected Read Dock Doors Failure", data.Errors, null);
                                                //Modified by RHR for v-8.3.0.001 on 09/01/2020  fix bug when no data is returned we now return on error
                                                return;
                                            }
                                            else {
                                                if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                    if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                                        blnSuccess = true;
                                                    } else {
                                                        //Modified by RHR for v-8.3.0.001 on 09/01/2020  set success to false
                                                        blnSuccess = false;
                                                        strValidationMsg = "Please check that at least one dock door is configured for the selected warehouse.";
                                                    }
                                                }
                                            }
                                        }
                                        if (blnSuccess === false && blnErrorShown === false) {
                                            if (strValidationMsg.length < 1) { strValidationMsg = "Get Dock Doors Failure"; }
                                            ngl.showErrMsg("Cannot locate your dock doors", strValidationMsg, null);
                                            //Modified by RHR for v-8.3.0.001 on 09/01/2020  fix bug when no data is returned we now return on error
                                            return;
                                        }
                                    } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                                }
                            });
                            $('#submenu').empty();
                            //Modified by RHR for v-8.3.0.001 on 09/01/2020  fix bug when no data is returned
                            if (typeof (res) !== 'undefined' && ngl.isArray(res) && res.length > 0) {
                                $('#scheduler').show();
                                for (var i = 0; i < res.length; i++) {
                                    $('#submenu').append('<input id="' + res[i].CompDockControl + '" value="' + res[i].CompDockControl + '" style="margin-left:10px"  type="checkbox" checked><label for="' + res[i].CompDockControl + '" class="k-checkbox-label">' + res[i].CompDockDockDoorName + '</label>');
                                }
                                scheduler.resources[0].dataSource.data(res);
                                for (var z = 0; z < res.length; z++) {
                                    var s = new AllFilter();
                                    s.filterName = 'dockDoorControl';
                                    s.filterValue = res[z].CompDockControl;
                                    $.ajax({
                                        url: '/api/AMSCompDockdoor/GetDockApptTimeSettings/',
                                        contentType: 'application/json; charset=utf-8',
                                        dataType: 'json',
                                        async: false,
                                        data: { filter: JSON.stringify(s) },
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                        success: function (data) { if (data.Data != null) { allDockDoorsSettingsArray.push(data.Data[0]); } }
                                    });
                                }
                                dsAllDockDoorsApptTimeSettings = allDockDoorsSettingsArray;
                                startEndDockDoorsTimes();
                                workHours();
                                scheduler.view(scheduler.view().name);
                                BlockOffTime();
                            }
                        }

                        //*********checkBox on change function**********//
                        $("#submenu").on("change", ":checkbox", function (e) {
                            var checked = $.map($("#submenu :checked"), function (checkbox) {
                                return res.filter(function (x) { return x.CompDockControl == $(checkbox).val() })
                            });
                            if (!checked.length) {
                                $("#submenu input:first:checkbox").prop('checked', true);
                                checked = $.map($("#submenu :checked"), function (checkbox) {
                                    return res.filter(function (x) { return x.CompDockControl == $(checkbox).val() })
                                });
                            }
                            var allDockDoorsSettingsArrayDummy = $.map($("#submenu :checked"), function (checkbox) {
                                return allDockDoorsSettingsArray.filter(function (x) { return x.DockControl == $(checkbox).val() })
                            });
                            dsAllDockDoorsApptTimeSettings = allDockDoorsSettingsArrayDummy;
                            startEndDockDoorsTimes();
                            workHours();
                            scheduler.resources[0].dataSource.data(checked);
                            //******Refresh DockDoors View in a Calender******//
                            scheduler.view(scheduler.view().name);
                            BlockOffTime();
                            //***Save PageSettings*******//
                            InsertOrUpdateCurrentUserPageSetting("SchedulerPage");
                            //Added by RHR for v=8.3.0.002 on 11/18/2020 we now resize when this event takes place
                            resizeSWidget(iLastHeightFactor);
                        });

                        //************ Context Menu in Scheduler*****************//
                        $("#contextMenu").kendoContextMenu({
                            filter: ".k-event, .k-scheduler-table td",
                            target: "#scheduler",
                            height: "auto",
                            select: function (e) {
                                var target = $(e.target);
                                var uid = target.data("uid");
                                var dataSource = scheduler.dataSource;
                                var item = dataSource.getByUid(uid);
                                var attrID = $(e.item).attr('data-id');
                                if (attrID == 'AddEdit' || attrID == 'CarrierDataEvent') {
                                    if (target.hasClass("k-event")) {
                                        var occurrenceByUid = scheduler.occurrenceByUid(target.data("uid"));
                                        scheduler.editEvent(occurrenceByUid);
                                    } else {
                                        var slot = scheduler.slotByElement(target);
                                        var doorSlot = scheduler.resourcesBySlot(slot);
                                        scheduler.addEvent({ start: slot.startDate, end: slot.endDate, doorId: doorSlot.doorId });
                                    }
                                    if (attrID == 'CarrierDataEvent') { $("#tabstrip").data("kendoTabStrip").select("li:last"); }
                                }
                                if (attrID == 'FDBD') {
                                    if (scheduler.options.showWorkHours == false) {
                                        scheduler.options.showWorkHours = true;
                                        scheduler.view(scheduler.view().name);
                                        BlockOffTime();
                                    } else {
                                        scheduler.options.showWorkHours = false;
                                        scheduler.view(scheduler.view().name);
                                        BlockOffTime();
                                    }
                                }
                                if (attrID == 'GTTD') {
                                    scheduler.date(new Date());
                                    scheduler.view(scheduler.view().name);
                                    BlockOffTime();
                                }
                                if (attrID == 'DeleteEvent') {
                                    var oData = scheduler.occurrenceByUid(target.data("uid"));
                                    $.ajax({
                                        url: 'api/AMSAppointment/DeleteAppointment',
                                        type: 'Post',
                                        contentType: 'application/json; charset=utf-8',
                                        dataType: 'json',
                                        data: JSON.stringify(oData),
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                        success: function (data) {
                                            try {
                                                var blnSuccess = false;
                                                var blnErrorShown = false;
                                                var strValidationMsg = "";
                                                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                                    if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Delete Appointment Failure", data.Errors, null); }
                                                    else {
                                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                                                blnSuccess = true;
                                                                if (data.Data[0] == false) { ngl.showWarningMsg("Delete Appointment Failure!", "", null); }
                                                            }
                                                        }
                                                    }
                                                }
                                                if (blnSuccess === false && blnErrorShown === false) {
                                                    if (strValidationMsg.length < 1) { strValidationMsg = "Delete Appointment Failure"; }
                                                    ngl.showErrMsg("Delete Appointment Failure", strValidationMsg, null);
                                                }
                                            } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                                            //refresh Scheduler
                                            scheduler.dataSource.read();
                                            $("#grid").data("kendoGrid").dataSource.read();
                                        },
                                        error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Delete Appointment Failure"); ngl.showErrMsg("Delete Appointment Failure", sMsg, null); }
                                    });
                                }
                                if (attrID == 'startCheckInEvent') {
                                    var oData = scheduler.occurrenceByUid(target.data("uid"));
                                    oData.AMSApptCarrierName = oData.title;
                                    oData.AMSApptDockdoorID = oData.doorId;
                                    oData.AMSApptStartDate = kendo.toString(new Date(oData.start), "MM/dd/yyyy hh:mm tt");
                                    oData.AMSApptEndDate = kendo.toString(new Date(oData.end), "MM/dd/yyyy hh:mm tt");
                                    oData.AMSApptActualDateTime = kendo.toString(new Date(), "MM/dd/yyyy hh:mm tt");
                                    oData.CompAMSColorCodeSettingColorCode = 4;
                                    oData.AMSApptStatusCode = 4;
                                    var UpdateApptStatusData = { Appt: oData, Ord: [], Flag: false };
                                    UpdateApptStatus(UpdateApptStatusData);
                                }
                                if (attrID == 'startLoadEvent') {
                                    var oData = scheduler.occurrenceByUid(target.data("uid"));
                                    oData.AMSApptCarrierName = oData.title;
                                    oData.AMSApptDockdoorID = oData.doorId;
                                    oData.AMSApptStartDate = kendo.toString(new Date(oData.start), "MM/dd/yyyy hh:mm tt");
                                    oData.AMSApptEndDate = kendo.toString(new Date(oData.end), "MM/dd/yyyy hh:mm tt");
                                    oData.AMSApptStartLoadingDateTime = kendo.toString(new Date(), "MM/dd/yyyy hh:mm tt");
                                    var UpdateApptStatusData = { Appt: oData, Ord: [], Flag: false };
                                    UpdateApptStatus(UpdateApptStatusData);
                                }
                                if (attrID == 'finishLoadEvent') {
                                    var oData = scheduler.occurrenceByUid(target.data("uid"));
                                    oData.AMSApptCarrierName = oData.title;
                                    oData.AMSApptDockdoorID = oData.doorId;
                                    oData.AMSApptStartDate = kendo.toString(new Date(oData.start), "MM/dd/yyyy hh:mm tt");
                                    oData.AMSApptEndDate = kendo.toString(new Date(oData.end), "MM/dd/yyyy hh:mm tt");
                                    oData.AMSApptFinishLoadingDateTime = kendo.toString(new Date(), "MM/dd/yyyy hh:mm tt");
                                    var UpdateApptStatusData = { Appt: oData, Ord: [], Flag: false };
                                    UpdateApptStatus(UpdateApptStatusData);
                                }
                                if (attrID == 'checkOutEvent') {
                                    var oData = scheduler.occurrenceByUid(target.data("uid"));
                                    oData.AMSApptCarrierName = oData.title;
                                    oData.AMSApptDockdoorID = oData.doorId;
                                    oData.AMSApptStartDate = kendo.toString(new Date(oData.start), "MM/dd/yyyy hh:mm tt");
                                    oData.AMSApptEndDate = kendo.toString(new Date(oData.end), "MM/dd/yyyy hh:mm tt");
                                    oData.AMSApptActLoadCompleteDateTime = kendo.toString(new Date(), "MM/dd/yyyy hh:mm tt");
                                    oData.CompAMSColorCodeSettingColorCode = 7;
                                    oData.AMSApptStatusCode = 7;
                                    var UpdateApptStatusData = { Appt: oData, Ord: [], Flag: false };
                                    UpdateApptStatus(UpdateApptStatusData);
                                }
                            },
                            open: function (e) {
                                var menu = e.sender;
                                if ($(e.target).hasClass("k-event")) {
                                    var event = scheduler.dataSource.getByUid($(e.target).attr("data-uid"));
                                    if (event.AMSApptControl > 0) {
                                        menu.remove(".myClass1");
                                        menu.append([{ text: "Update Appointment", attr: { 'data-id': 'AddEdit' }, cssClass: "myClass1" }]);
                                        menu.remove(".myClass2");
                                        menu.append([{ text: "Delete", attr: { 'data-id': 'DeleteEvent' }, cssClass: "myClass2" }]);
                                        //menu.remove(".k-separator"); //modified by RHR for v-8.5.4.003 k-separator not supported
                                        //menu.append([{cssClass: "k-separator" }]); //modified by RHR for v-8.5.4.003 k-separator not supported
                                        menu.remove(".myClass3");
                                        menu.append([{ text: "Carrier Data", attr: { 'data-id': 'CarrierDataEvent' }, cssClass: "myClass3" }]);
                                        /*menu.append([{cssClass: "k-separator" }]);*/ //modified by RHR for v-8.5.4.003 k-separator not supported
                                        menu.remove(".myClass4");
                                        menu.append([{ text: "Start CheckIn", attr: { 'data-id': 'startCheckInEvent' }, cssClass: "myClass4" }]);
                                        menu.remove(".myClass5");
                                        menu.append([{ text: "Start Load/Unload", attr: { 'data-id': 'startLoadEvent' }, cssClass: "myClass5" }]);
                                        menu.remove(".myClass6");
                                        menu.append([{ text: "Finish Load/Unload", attr: { 'data-id': 'finishLoadEvent' }, cssClass: "myClass6" }]);
                                        /* menu.append([{cssClass: "k-separator" }]);*/ //modified by RHR for v-8.5.4.003 k-separator not supported
                                        menu.remove(".myClass7");
                                        menu.append([{ text: "CheckOut", attr: { 'data-id': 'checkOutEvent' }, cssClass: "myClass7" }]);
                                    }
                                }
                                else {
                                    //var text = $(e.target).hasClass("k-event") ? "Edit Appointment" : "New Appointment";
                                    menu.remove(".myClass1");
                                    menu.append([{ text: "New Appointment", attr: { 'data-id': 'AddEdit' }, cssClass: "myClass1" }]);
                                    //menu.remove(".k-separator"); //modified by RHR for v-8.5.4.003 k-separator not supported
                                    //menu.append([{cssClass: "k-separator" }]); //modified by RHR for v-8.5.4.003 k-separator not supported
                                    menu.remove(".myClass2");
                                    menu.append([{ text: "Go to today", attr: { 'data-id': 'GTTD' }, cssClass: "myClass2" }]);
                                    menu.remove(".myClass3");
                                    if (scheduler.options.showWorkHours == false) { menu.append([{ text: "Show business hours", attr: { 'data-id': 'FDBD' }, cssClass: "myClass3" }]); } else { menu.append([{ text: "Show full day", attr: { 'data-id': 'FDBD' }, cssClass: "myClass3" }]); }
                                    menu.remove(".myClass4");
                                    menu.remove(".myClass5");
                                    menu.remove(".myClass6");
                                    menu.remove(".myClass7");
                                    if ($(e.target).hasClass("customNonwork") && scheduler.view().name == "month") { $(".myClass1").prop('disabled', true); $(".myClass1").css("color", "lightgrey"); $(".myClass1 > span").css('cursor', 'default'); }
                                }
                            }
                        });

                        //*********Update Appointment Status function********//
                        function UpdateApptStatus(UpdateApptData) {
                            //console.log('Save Appt 3');
                            //console.log(UpdateApptdata);
                            $.ajax({
                                url: '/api/AMSAppointment/SaveUpdateAppointmentOrders/',
                                type: 'Post',
                                contentType: 'application/json; charset=utf-8',
                                dataType: 'json',
                                data: JSON.stringify(UpdateApptData),
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                success: function (data) {
                                    try {
                                        var blnSuccess = false;
                                        var blnErrorShown = false;
                                        var strValidationMsg = "";
                                        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                            if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Save Appointment Failure", data.Errors, null); }
                                            else {
                                                if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                    if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') { blnSuccess = true; scheduler.dataSource.read(); }
                                                }
                                            }
                                        }
                                        if (blnSuccess === false && blnErrorShown === false) {
                                            if (strValidationMsg.length < 1) { strValidationMsg = "Save Appointment Failure"; }
                                            ngl.showErrMsg("Save Appointment Failure", strValidationMsg, null);
                                        }
                                    } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                                },
                                error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure"); ngl.showErrMsg("Save Appointment Failure", sMsg, null); }
                            });
                        }

                        //************Search Appointments Grid*****************//
                        dsAppointmentGrid = new kendo.data.DataSource({
                            transport: {
                                read: function (options) {
                                    var s = new AllFilter();
                                    s.CompControlFrom = CompControl;
                                    s.page = $("#ddlAppointmentBoundFilter").data("kendoDropDownList").value();
                                    s.filterName = $("#ddlAppointmentFilters").data("kendoDropDownList").value();
                                    s.filterValue = $("#txtAppointmentFilterVal").data("kendoMaskedTextBox").value();
                                    $.ajax({
                                        url: '/api/AMSAppointment/GetRecordsByAppointmentFilter/',
                                        contentType: 'application/json; charset=utf-8',
                                        dataType: 'json',
                                        data: { filter: JSON.stringify(s) },
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                        success: function (data) {
                                            options.success(data);
                                            if (data.Data.length > 0) {
                                                scheduler.view("day");
                                                scheduler.date(new Date(data.Data[0].AMSApptStartDate));
                                                scheduler.view(scheduler.view().name);
                                                scheduler.dataSource.read();
                                            }
                                            if (data.Data.length <= 0) {
                                                //ngl.showWarningMsg("No Appointments Found In this Criteria !","")
                                            }
                                            try {
                                                var blnSuccess = false;
                                                var blnErrorShown = false;
                                                var strValidationMsg = "";
                                                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                                    if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Get Appointments Failure", data.Errors, null); }
                                                    else {
                                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) { blnSuccess = true; } else { blnSuccess = true; strValidationMsg = "No records were found matching your search criteria"; }
                                                        }
                                                    }
                                                }
                                                if (blnSuccess === false && blnErrorShown === false) {
                                                    if (strValidationMsg.length < 1) { strValidationMsg = "Get Appointments Failure"; }
                                                    ngl.showErrMsg("Get Appointments Failure", strValidationMsg, null);
                                                }
                                            } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                                        },
                                        error: function (result) { options.error(result); }
                                    });
                                },
                                parameterMap: function (options, operation) { return options; }
                            },
                            schema: {
                                data: "Data",
                                total: "Count",
                                model: {
                                    id: "AMSApptControl",
                                    fields: {
                                        AMSApptControl: { type: "number" },
                                        AMSApptStartDate: { type: "date" },
                                        AMSApptEndDate: { type: "date" },
                                        DockDoorName: { type: "string" },
                                    }
                                },
                                errors: "Errors"
                            },
                            error: function (xhr, textStatus, error) { ngl.showErrMsg("Access Appointments Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges(); }
                        });

                        $("#gridAppt").kendoGrid({
                            noRecords: true,
                            height: 120,
                            autoBind: false,
                            columns: [
                                { field: "AMSApptControl", title: "Appointment ID" },
                                { field: "AMSApptStartDate", title: "Start Date and Time", format: "{0:M/d/yyyy hh:mm tt}" },
                                { field: "AMSApptEndDate", title: "End Date and Time", format: "{0:M/d/yyyy hh:mm tt}" },
                                { field: "DockDoorName", title: "Resource" },
                            ],
                            resizable: true,
                            selectable: true,
                            dataSource: dsAppointmentGrid,
                            dataBound: function (e) {
                                // get the index of the UnitsInStock cell
                                var columns = e.sender.columns;
                                var columnIndex = this.wrapper.find(".k-grid-header [data-field=" + "UnitsInStock" + "]").index();
                                // iterate the data items and apply row styles where necessary
                                var dataItems = e.sender.dataSource.view();
                                for (var j = 0; j < dataItems.length; j++) {
                                    var units = dataItems[j].get("AMSApptStatusCode");
                                    var row = e.sender.tbody.find("[data-uid='" + dataItems[j].uid + "']");
                                    if (units == 0) { row.addClass("zeroColor"); }
                                    if (units == 1) { row.addClass("oneColor"); }
                                    if (units == 2) { row.addClass("twoColor"); }
                                    if (units == 3) { row.css("background-color", "#ff66ff99"); }
                                }
                            }
                        }).data("kendoGrid");

                        //************Search Orders Grid*****************//
                        dsOrdersGrid = new kendo.data.DataSource({
                            pageSize: 5,
                            transport: {
                                read: function (options) {
                                    var s = new AllFilter();
                                    s.CompControlFrom = CompControl;
                                    s.filterFrom = Vstart; //new Date();//$("#dpOrdFilterFrom").data("kendoDatePicker").value();
                                    s.filterTo = Vend; //new Date();//$("#dpOrdFilterTo").data("kendoDatePicker").value();                                         
                                    s.filterName = $("#ddlOrdFilters").data("kendoDropDownList").value();
                                    s.filterValue = $("#txtOrdFilterVal").data("kendoMaskedTextBox").value();
                                    s.skip = 0;
                                    s.take = 5;
                                    s.page = 1;
                                    s.pageSize = 5;
                                    $.ajax({
                                        url: '/api/AMSOrder/GetRecordsByOrdersFilter/',
                                        contentType: 'application/json; charset=utf-8',
                                        dataType: 'json',
                                        data: { filter: JSON.stringify(s) },
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                        success: function (data) {
                                            if ($("#grid").data("kendoGrid").dataSource.pageSize() < 5) { $("#grid").data("kendoGrid").dataSource.pageSize(5); }
                                            options.success(data);
                                            //if(data.Data.length <= 0 && $("#grid").is(":visible")){ ngl.showWarningMsg("No Orders Found In this Criteria.!",""); }
                                            try {
                                                var blnSuccess = false;
                                                var blnErrorShown = false;
                                                var strValidationMsg = "";
                                                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                                    if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Get Orders Failure", data.Errors, null); }
                                                    else {
                                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) { blnSuccess = true; } else { blnSuccess = true; strValidationMsg = "No records were found matching your search criteria"; }
                                                        }
                                                    }

                                                    //Enable to drag grid value for book control id
                                                    enableSimpleGridDrag();
                                                }
                                                if (blnSuccess === false && blnErrorShown === false) {
                                                    if (strValidationMsg.length < 1) { strValidationMsg = "Get Orders Failure"; }
                                                    ngl.showErrMsg("Get Orders Failure", strValidationMsg, null);
                                                }
                                            } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                                        },
                                        error: function (result) { options.error(result); }
                                    });
                                },
                                parameterMap: function (options, operation) { return options; }
                            },
                            schema: {
                                data: "Data",
                                total: "Count",
                                model: {
                                    id: "BookControl",
                                    fields: {
                                        BookControl: { type: "number" },
                                        BookConsPrefix: { type: "string" },
                                        BookDateOrdered: { type: "date" },
                                        BookDateLoad: { type: "date" },
                                        BookDateRequired: { type: "date" },
                                        CarrierSCAC: { type: "string" }, // Modified by RHR for v-8.3.0.002 modified fields 
                                    }
                                },
                                errors: "Errors"
                            },
                            error: function (xhr, textStatus, error) { ngl.showErrMsg("Access Appointments Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges(); }
                        });

                        var grid = $("#grid").kendoGrid({
                            noRecords: true, //Added by LVV on 8/29/2018  // Modified by RHR for v-8.3.0.002 modified fields                      
                            columns: [
                                { field: "BookConsPrefix", title: "CNS", width: "100px" },
                                { field: "BookCarrOrderNumber", title: "Order No", width: "100px" },
                                { field: "CarrierSCAC", title: "SCAC", width: "50px" },
                                { field: "BookDateLoad", title: "Load", format: "{0:M/d/yyyy}", width: "70px" },
                                { field: "BookDateRequired", title: "Required", format: "{0:M/d/yyyy}", width: "70px" },
                                { field: "BookDateOrdered", title: "Ordered", format: "{0:M/d/yyyy}", width: "70px" },
                                { field: "BookLoadPONumber", title: "PO", width: "70px" },
                                { field: "BookProNumber", title: "PRO", width: "70px" },
                                { field: "BookOrderSequence", title: "Seq No", width: "50px" }
                            ],
                            resizable: true,
                            pageable: { refresh: true, },
                            selectable: "row",
                            autoBind: false,
                            dataSource: dsOrdersGrid,
                            dataBound: function (e) {
                                // get the index of the UnitsInStock cell
                                var columns = e.sender.columns;
                                var columnIndex = this.wrapper.find(".k-grid-header [data-field=" + "UnitsInStock" + "]").index();
                                // iterate the data items and apply row styles where necessary
                                var dataItems = e.sender.dataSource.view();
                                for (var j = 0; j < dataItems.length; j++) {
                                    var color = dataItems[j].get("OrderColorCode");
                                    var type = dataItems[j].get("OrderType");
                                    //alert(dataItems[j].get("OrderType"));
                                    var row = e.sender.tbody.find("[data-uid='" + dataItems[j].uid + "']");
                                    if (ngl.isNullOrUndefined(color) || color < 1) {
                                        //if OrderColorCode is null use defaults
                                        switch (type) {
                                            case 1: //SingleLoadNoCNSOutbound
                                            case 4: //SingleLoadNoCNSInbound
                                                row.addClass("darkgoldenrod");
                                                break;
                                            case 2: //ConsolidatedIntegrityOffOutbound
                                            case 3: //ConsolidatedLoadOutbound
                                            case 5: //ConsolidatedIntegrityOffInbound
                                            case 6: //ConsolidatedLoadInbound
                                                row.addClass("darkcyan");
                                                break;
                                            //case 7: //Other
                                            default:
                                                row.css("background-color", "#" + event.color);
                                        }
                                    } else { row.css("background-color", "#" + color); } //else use the color sent back in CompAMSColorCodeSettingColorCode
                                }
                            }
                        }).data("kendoGrid");

                        // optional: store dragged items robustly
                        var draggedItems = [];
                        var draggedOrder = null;

                        grid.table.kendoDraggable({
                            filter: "tbody > tr",
                            group: "gridGroup", // important — to link with scheduler drop group
                            dragstart: function (e) {
                                // store currently selected rows' dataItems
                                //draggedItems = [];
                                //grid.tbody.find(".k-state-selected").each(function () {
                                //    var item = grid.dataItem(this);
                                //    if (item) draggedItems.push(item);
                                //});
                                //add margin to position correctly the tooltip under the pointer
                                gridRowOffset = grid.tbody.find("tr:first").offset();
                                $("#dragTooltip").css("margin-left", e.clientX - gridRowOffset.left - 220);
                            },
                            hint: function (row) {
                                //remove old selection
                                row.parent().find(".k-state-selected").each(function () {
                                    $(this).removeClass("k-state-selected")
                                });
                                //add selected class to the current row
                                row.addClass("k-state-selected");
                                var dataItem = grid.dataItem(row);
                                draggedOrder = dataItem; // store globally
                                var tooltipHtml = "<div class='k-event' id='dragTooltip'><h3 class='k-event-template'>" + dataItem.BookConsPrefix +
                                    "</h3><h3 class='k-event-template'>" + dataItem.BookProNumber +
                                    "</h3><h3 class='k-event-template'>" + dataItem.BookCarrOrderNumber +
                                    "</h3><h3 class='k-event-template'>" + dataItem.BookLoadPONumber + "</h3></div>";
                                return $(tooltipHtml).css("width", 200);
                            },
                            dragend: function () {
                                draggedOrder = null;
                            }
                        });

                        //***Window DockDoor dropdown List***// 
                        $("#ddlDockDoorID").kendoDropDownList({
                            dataSource: res,
                            dataTextField: "CompDockDockDoorName",
                            dataValueField: "CompDockDockDoorID",
                            autoWidth: true,
                            filter: "contains",
                        });

                        if ($("#txtOrderNumber").focus(function () {
                            $("#txtApptNumber").data("kendoMaskedTextBox").value("");
                            $("#txtCNS").data("kendoMaskedTextBox").value("");
                            $("#txtProNumber").data("kendoMaskedTextBox").value("");
                        }));
                        if ($("#txtApptNumber").focus(function () {
                            $("#txtOrderNumber").data("kendoMaskedTextBox").value("");
                            $("#txtCNS").data("kendoMaskedTextBox").value("");
                            $("#txtProNumber").data("kendoMaskedTextBox").value("");
                        }));
                        if ($("#txtCNS").focus(function () {
                            $("#txtOrderNumber").data("kendoMaskedTextBox").value("");
                            $("#txtApptNumber").data("kendoMaskedTextBox").value("");
                            $("#txtProNumber").data("kendoMaskedTextBox").value("");
                        }));
                        if ($("#txtProNumber").focus(function () {
                            $("#txtOrderNumber").data("kendoMaskedTextBox").value("");
                            $("#txtCNS").data("kendoMaskedTextBox").value("");
                            $("#txtApptNumber").data("kendoMaskedTextBox").value("");
                        }));

                        $("#btnSearch").kendoButton({
                            icon: "search",
                            click: function (e) {
                                if ($("#txtOrderNumber").data("kendoMaskedTextBox").value() == "" &&
                                    $("#txtApptNumber").data("kendoMaskedTextBox").value() == "" &&
                                    $("#txtCNS").data("kendoMaskedTextBox").value() == "" &&
                                    $("#txtProNumber").data("kendoMaskedTextBox").value() == "") {
                                    ngl.showWarningMsg("Please input details in atleast one criteria to search orders!", "")
                                    $("#txtOrderNumber").focus();
                                } else {
                                    //$("#ordersWndGrid").data("kendoGrid").dataSource.read();
                                    var s = new AllFilter();
                                    s.CompControlFrom = CompControl;
                                    s.filterFrom = Vstart;
                                    s.filterTo = Vend;

                                    if ($("#txtOrderNumber").data("kendoMaskedTextBox").value() != "") { s.filterName = "OrdNumber"; s.filterValue = $("#txtOrderNumber").data("kendoMaskedTextBox").value(); }
                                    if ($("#txtApptNumber").data("kendoMaskedTextBox").value() != "") { s.filterName = "PoNumber"; s.filterValue = $("#txtApptNumber").data("kendoMaskedTextBox").value(); }
                                    if ($("#txtCNS").data("kendoMaskedTextBox").value() != "") { s.filterName = "CNSNumber"; s.filterValue = $("#txtCNS").data("kendoMaskedTextBox").value(); }
                                    if ($("#txtProNumber").data("kendoMaskedTextBox").value() != "") { s.filterName = "ProNumner"; s.filterValue = $("#txtProNumber").data("kendoMaskedTextBox").value(); }
                                    $.ajax({
                                        url: '/api/AMSOrder/GetRecordsByOrdersFilter/',
                                        contentType: 'application/json; charset=utf-8',
                                        dataType: 'json',
                                        data: { filter: JSON.stringify(s) },
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                        success: function (data) {
                                            if (data.Data.length > 0) {
                                                AlertSCheduleDateMisMatch(data.Data, new Date($("#txtStartTime").val()));
                                                var ordersGridDate = $("#ordersWndGrid").data("kendoGrid").dataSource.data();
                                                ordersGridDate.push(data.Data[0])
                                                var FordersGridDate = onlyUnique(ordersGridDate)
                                                $("#ordersWndGrid").data("kendoGrid").dataSource.data(FordersGridDate);
                                                if ($("#txtCarrierName").val() == "") { $("#txtCarrierName").val(FordersGridDate[0].CarrierName); }
                                                if ($("#txtSCAC").val() == "") { $("#txtSCAC").val(FordersGridDate[0].CarrierNumber); }
                                                if (FordersGridDate[0].BookDestCompControl == CompControl) { //Modified By LVV on 11/6/2019 Ticket #201911041624
                                                    $("#rdoInBound").prop('checked', true);
                                                    $("#compBound input:radio").attr('disabled', true);
                                                } else if (FordersGridDate[0].BookOrigCompControl == CompControl) {
                                                    $("#rdoOutBound").prop('checked', true);
                                                    $("#compBound input:radio").attr('disabled', true);
                                                } else { $("#compBound input:radio").attr('disabled', false); }
                                            }
                                            if (data.Data.length == 0) { ngl.showWarningMsg("No Orders Found In this Criteria.!", ""); }
                                        }
                                    });
                                }
                            }
                        });

                        var searchWndOrdersVld = false;
                        dsOrdersWndGrid = new kendo.data.DataSource({
                            transport: {
                                read: function (options) {
                                    var s = new AllFilter();
                                    s.CompControlFrom = CompControl;
                                    s.filterFrom = Vstart;
                                    s.filterTo = Vend;
                                    if ($("#txtOrderNumber").data("kendoMaskedTextBox").value() != "") { s.filterName = "OrdNumber"; s.filterValue = $("#txtOrderNumber").data("kendoMaskedTextBox").value(); }
                                    if ($("#txtApptNumber").data("kendoMaskedTextBox").value() != "") { s.filterName = "PoNumber"; s.filterValue = $("#txtApptNumber").data("kendoMaskedTextBox").value(); }
                                    if ($("#txtCNS").data("kendoMaskedTextBox").value() != "") { s.filterName = "CNSNumber"; s.filterValue = $("#txtCNS").data("kendoMaskedTextBox").value(); }
                                    if ($("#txtProNumber").data("kendoMaskedTextBox").value() != "") { s.filterName = "ProNumner"; s.filterValue = $("#txtProNumber").data("kendoMaskedTextBox").value(); }
                                    $.ajax({
                                        url: '/api/AMSOrder/GetRecordsByOrdersFilter/',
                                        contentType: 'application/json; charset=utf-8',
                                        dataType: 'json',
                                        data: { filter: JSON.stringify(s) },
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                        success: function (data) {
                                            options.success(data);
                                            Ord = data.Data;
                                            if (data.Data.length == 0 && searchWndOrdersVld == true) { ngl.showWarningMsg("No Orders Found In this Criteria.!", ""); }
                                            searchWndOrdersVld = true;
                                            try {
                                                var blnSuccess = false;
                                                var blnErrorShown = false;
                                                var strValidationMsg = "";
                                                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                                    if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Get Order Failure", data.Errors, null); }
                                                    else {
                                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) { blnSuccess = true; } else { blnSuccess = true; strValidationMsg = "No records were found matching your search criteria"; }
                                                        }
                                                    }
                                                }
                                                if (blnSuccess === false && blnErrorShown === false) {
                                                    if (strValidationMsg.length < 1) { strValidationMsg = "Get Order Failure"; }
                                                    ngl.showErrMsg("Get Order Failure", strValidationMsg, null);
                                                }
                                            } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                                        },
                                        error: function (result) { options.error(result); }
                                    });
                                },
                                destroy: function (options) {
                                    var S = { BookControl: options.data.BookControl, ApptControl: $("#ApptCountrol").val() }
                                    if ($("#ApptCountrol").val() > 0 && (options.data.BookOrigCompControl > 0 || options.data.BookDestCompControl > 0)) {
                                        $.ajax({
                                            url: 'api/AMSAppointment/RemoveBookingFromAppointment',
                                            type: 'Post',
                                            //contentType: 'application/json; charset=utf-8', 
                                            dataType: 'json',
                                            data: { "": JSON.stringify(S) },
                                            headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                            success: function (data) {
                                                try {
                                                    var blnSuccess = false;
                                                    var blnErrorShown = false;
                                                    var strValidationMsg = "";
                                                    if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                                        if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Delete Order Failure", data.Errors, null); }
                                                        else {
                                                            if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                                if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                                                    blnSuccess = true;
                                                                    if (data.Data[0] == false) { ngl.showWarningMsg("Delete Order Failure!", "", null); }
                                                                    else {

                                                                        //$("#grid").data("kendoGrid").dataSource.read();                                                                    
                                                                        ngl.Alert("Window Must Close", "Your changes have been saved. You must manually reopen the window to see your changes.", tPage);
                                                                        wndAddEditEvent.close();
                                                                        scheduler.dataSource.read();
                                                                        $("#grid").data("kendoGrid").dataSource.read();
                                                                        //wndAddEditEvent.close();
                                                                        //$("#viewAvailableSlotsGrid").data("kendoGrid").dataSource.read();
                                                                        //$("#ordersWndGrid").data("kendoGrid").dataSource.read();
                                                                        //kendo.ui.progress($("#ordersWndGrid").element, false);
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    if (blnSuccess === false && blnErrorShown === false) {
                                                        if (strValidationMsg.length < 1) { strValidationMsg = "Delete Order Failure"; }
                                                        ngl.showErrMsg("Delete Order Failure", strValidationMsg, null);
                                                    }
                                                } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                                            },
                                            error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "delete Order Failure"); ngl.showErrMsg("delete Order Failure", sMsg, null); }
                                        });
                                    }
                                },
                                parameterMap: function (options, operation) { return options; }
                            },
                            schema: {
                                data: "Data",
                                total: "Count",
                                model: {
                                    id: "BookControl",
                                    fields: {
                                        BookControl: { type: "number" },
                                        BookConsPrefix: { type: "string" },
                                        BookProNumber: { type: "string" },
                                        BookCarrOrderNumber: { type: "string" },
                                        BookLoadPONumber: { type: "string" },
                                        BookDateOrdered: { type: "date" },
                                        BookDateLoad: { type: "date" },
                                        BookDateRequired: { type: "date" },
                                    }
                                },
                                errors: "Errors"
                            },
                            error: function (xhr, textStatus, error) { ngl.showErrMsg("Access Appointments Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges(); }
                        });

                        $("#ordersWndGrid").kendoGrid({
                            //height:300,
                            noRecords: true,
                            resizable: true,
                            dataSource: dsOrdersWndGrid,
                            autoBind: false,
                            selectable: "single row",
                            editable: "inline",
                            dataBound: function (e) {
                                //var grid = $("#ordersWndGrid").data("kendoGrid");
                                //for (var i = 0; i < grid.columns.length; i++) {
                                //    grid.autoFitColumn(i);
                                //}
                                var tObj = this;
                                tObj.tbody.find("tr[role='row']").each(function () {
                                    var model = tObj.dataItem(this);
                                    if (model.BookControl == 0) {
                                        $(this).find(".k-grid-editOrder").addClass("k-state-disabled");
                                        e.preventDefault();
                                        $(".k-state-disabled").each(function (index) {
                                            $(this).removeClass('k-grid-editOrder')
                                        });
                                    }
                                });
                            },
                            detailInit: detailInit,
                            toolbar: [{ name: "createOrder", text: "Add new Order", iconClass: "k-icon k-i-add" }],
                            columns: [
                                { command: [{ name: "editOrder", text: "", iconClass: "k-icon k-i-pencil", click: openOdereAddEditWindow }, { name: "destroy", text: "", iconClass: "k-icon k-i-trash" }], title: "Action", width: "100px" },
                                { field: "BookConsPrefix", title: "CNS", width: "100px" },
                                { field: "BookProNumber", title: "Pro Number", width: "100px" },
                                { field: "BookCarrOrderNumber", title: "Order Number", width: "100px" },
                                { field: "BookLoadPONumber", title: "PO Number", width: "100px" },
                                { field: "EquipmentID", title: "Equip ID", width: "80px" },
                                { field: "BookDateOrdered", title: "Date Ordered", template: "#= kendo.toString(new Date(BookDateOrdered), 'M/d/yyyy') #", width: "90px" },
                                { field: "BookDateLoad", title: "Date Load", template: "#= kendo.toString(new Date(BookDateLoad), 'M/d/yyyy') #", width: "90px" },
                                { field: "BookDateRequired", title: "Date Required", template: "#= kendo.toString(new Date(BookDateRequired), 'M/d/yyyy') #", width: "90px" },
                                { field: "BookTotalCases", title: "Case", width: "50px" },
                                { field: "BookTotalCube", title: "Cube", width: "50px" },
                                { field: "BookTotalWgt", title: "Weight", width: "50px" },
                                { field: "BookTotalPL", title: "Pallets In", width: "50px" },
                                { field: "BookTotalPX", title: "Pallets Out", width: "50px" },
                                { field: "BookItemDetailDescription", title: "Desc", width: "100px" },
                                { field: "BookOrigName", title: "Orig", width: "150px" },
                                { field: "BookOrigAddress1", title: "Orig Address", width: "150px" },
                                { field: "BookOrigCity", title: "Orig City", width: "150px" },
                                { field: "BookOrigState", title: "Orig State", width: "50px" },
                                { field: "BookOrigZip", title: "Orig Zip", width: "60px" },
                                { field: "BookOrigCountry", title: "Orig Country", width: "50px" },
                                { field: "BookDestName", title: "Dest", width: "150px" },
                                { field: "BookDestAddress1", title: "Dest Address", width: "150px" },
                                { field: "BookDestCity", title: "Dest City", width: "150px" },
                                { field: "BookDestState", title: "Dest State", width: "50px" },
                                { field: "BookDestZip", title: "Dest Zip", width: "60px" },
                                { field: "BookDestCountry", title: "Dest Country", width: "50px" },
                            ],
                        });

                        function detailInit(e) {
                            $("<div/>").appendTo(e.detailCell).kendoGrid({
                                noRecords: true,
                                dataSource: {
                                    transport: {
                                        read: function (options) {
                                            var s = new AllFilter();
                                            s.filterValue = e.data.BookLoadControl;
                                            $.ajax({
                                                url: '/api/AMSOrder/GetBookItemsByBookLoadControl/',
                                                contentType: 'application/json; charset=utf-8',
                                                dataType: 'json',
                                                data: { filter: JSON.stringify(s) },
                                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                                success: function (data) {
                                                    options.success(data);
                                                    Ord = data.Data;
                                                    if (data.Data.length == 0) { ngl.showWarningMsg("No OrderItems Found for this Order.!", ""); }
                                                    wndShowOrdersVal = true;
                                                    try {
                                                        var blnSuccess = false;
                                                        var blnErrorShown = false;
                                                        var strValidationMsg = "";
                                                        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                                            if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Get OrderItems Failure", data.Errors, null); }
                                                            else {
                                                                if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                                    if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) { blnSuccess = true; } else { blnSuccess = true; strValidationMsg = "No records were found matching your search criteria"; }
                                                                }
                                                            }
                                                        }
                                                        if (blnSuccess === false && blnErrorShown === false) {
                                                            if (strValidationMsg.length < 1) { strValidationMsg = "Get OrderItems Failure"; }
                                                            ngl.showErrMsg("Get OrderItems Failure", strValidationMsg, null);
                                                        }
                                                    } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                                                },
                                                error: function (result) { //api/AMSAppointment/RemoveBookingFromAppointment
                                                    options.error(result);
                                                }
                                            });
                                        },
                                        parameterMap: function (options, operation) { return options; }
                                    },
                                    schema: {
                                        data: "Data",
                                        total: "Count",
                                        model: {
                                            id: "BookItemControl",
                                            fields: {
                                                BookItemControl: { type: "number" },
                                                BookItemItemNumber: { type: "string" },
                                                BookItemSize: { type: "string" },
                                                BookItemDescription: { type: "string" },
                                                BookItemBrand: { type: "string" },
                                                BookItemQtyOrdered: { type: "number" }, // Modified by RHR for v-8.3.0.002 modified fields 
                                                BookItemWeight: { type: "number" },     // Modified by RHR for v-8.3.0.002 modified fields 
                                                BookCustItemNumber: { type: "string" },  // Modified by RHR for v-8.3.0.002 modified fields 
                                                BookItemCube: { type: "number" },        // Modified by RHR for v-8.3.0.002 modified fields 
                                                BookItemLotNumber: { type: "string" }    // Modified by RHR for v-8.3.0.002 modified fields                                          

                                            }
                                        },
                                        errors: "Errors"
                                    },
                                    error: function (xhr, textStatus, error) { ngl.showErrMsg("Access OrderItems Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges(); }
                                },
                                columns: [
                                    { field: "BookItemItemNumber", title: "Item Number", width: "50px" },
                                    { field: "BookItemSize", title: "Size", width: "50px" },
                                    { field: "BookItemDescription", title: "Description", width: "150px" },
                                    { field: "BookItemBrand", title: "Brand", width: "50px" },
                                    { field: "BookItemQtyOrdered", title: "Cases", width: "50px" }, // Modified by RHR for v-8.3.0.002 modified fields
                                    { field: "BookItemWeight", title: "Wgt", width: "50px" },
                                    { field: "BookCustItemNumber", title: "Cust Item Number", width: "50px" },
                                    { field: "BookItemCube", title: "Cubes", width: "50px" },
                                    { field: "BookItemLotNumber", title: "Lot", width: "50px" },
                                ]
                            });
                        }

                        //****Add edit BookAdhoc Orders form****// 
                        $("#AddEditOrder").hide();

                        $("#ordersWndGrid .k-grid-createOrder").click(function (e) { openOdereAddEditWindow(e) });

                        var orderItem = new AMSOrders();
                        function openOdereAddEditWindow(e) {
                            $("#txtOrdOrderNu-validation").addClass("hide-display");
                            $("#txtOrdPONu-validation").addClass("hide-display");
                            $("#txtOrdCNS").data("kendoMaskedTextBox").enable(true);
                            $("#txtOrdProNu").data("kendoMaskedTextBox").enable(true);
                            $("#txtOrdOrderNu").data("kendoMaskedTextBox").enable(true);
                            $("#txtOrdPONu").data("kendoMaskedTextBox").enable(true);
                            $("#dtOrdOrderd").data("kendoDatePicker").enable(true);
                            $("#dtOrdLoad").data("kendoDatePicker").enable(true);
                            $("#dtOrdRequired").data("kendoDatePicker").enable(true);
                            $("#txtOrdCases").data("kendoNumericTextBox").enable(true);
                            $("#txtOrdCubes").data("kendoNumericTextBox").enable(true);
                            $("#txtOrdWeight").data("kendoNumericTextBox").enable(true);
                            $("#txtOrdPalletsIn").data("kendoNumericTextBox").enable(true);
                            $("#txtOrdPalletsOut").data("kendoNumericTextBox").enable(true);
                            $("#txtOrdDesc").data("kendoMaskedTextBox").enable(true);
                            $("#txtOrdEquipID").data("kendoMaskedTextBox").enable(true);
                            $("#txtOrdOrig").data("kendoMaskedTextBox").enable(true);
                            $("#txtOrdOrigAddress").data("kendoMaskedTextBox").enable(true);
                            $("#txtOrdOrigCity").data("kendoMaskedTextBox").enable(true);
                            $("#txtOrdOrigState").data("kendoMaskedTextBox").enable(true);
                            $("#txtOrdOrigZip").data("kendoMaskedTextBox").enable(true);
                            $("#txtOrdOrigCountry").data("kendoMaskedTextBox").enable(true);
                            $("#txtOrdDest").data("kendoMaskedTextBox").enable(true);
                            $("#txtOrdDestAddress").data("kendoMaskedTextBox").enable(true);
                            $("#txtOrdDestCity").data("kendoMaskedTextBox").enable(true);
                            $("#txtOrdDestState").data("kendoMaskedTextBox").enable(true);
                            $("#txtOrdDestZip").data("kendoMaskedTextBox").enable(true);
                            $("#txtOrdDestCountry").data("kendoMaskedTextBox").enable(true);
                            orderItem = {};
                            if ($(e.currentTarget).hasClass("k-grid-createOrder")) {
                                $("#AddEditOrderLegend").html("<b>Add new Adhoc Order</b>");
                                orderItem.BookControl = 0;
                                $("#txtOrdCNS").data("kendoMaskedTextBox").value("");
                                $("#txtOrdProNu").data("kendoMaskedTextBox").value("");
                                $("#txtOrdOrderNu").data("kendoMaskedTextBox").value("");
                                $("#txtOrdPONu").data("kendoMaskedTextBox").value("");
                                $("#dtOrdOrderd").val("");
                                $("#dtOrdLoad").val("");
                                $("#dtOrdRequired").val("");
                                $("#txtOrdCases").data("kendoNumericTextBox").value("");
                                $("#txtOrdCubes").data("kendoNumericTextBox").value("");
                                $("#txtOrdWeight").data("kendoNumericTextBox").value("");
                                $("#txtOrdPalletsIn").data("kendoNumericTextBox").value("");
                                $("#txtOrdPalletsOut").data("kendoNumericTextBox").value("");
                                $("#txtOrdDesc").data("kendoMaskedTextBox").value("");
                                $("#txtOrdEquipID").data("kendoMaskedTextBox").value("");
                                $("#txtOrdOrig").data("kendoMaskedTextBox").value("");
                                $("#txtOrdOrigAddress").data("kendoMaskedTextBox").value("");
                                $("#txtOrdOrigCity").data("kendoMaskedTextBox").value("");
                                $("#txtOrdOrigState").data("kendoMaskedTextBox").value("");
                                $("#txtOrdOrigZip").data("kendoMaskedTextBox").value("");
                                $("#txtOrdOrigCountry").data("kendoMaskedTextBox").value("");
                                $("#txtOrdDest").data("kendoMaskedTextBox").value("");
                                $("#txtOrdDestAddress").data("kendoMaskedTextBox").value("");
                                $("#txtOrdDestCity").data("kendoMaskedTextBox").value("");
                                $("#txtOrdDestState").data("kendoMaskedTextBox").value("");
                                $("#txtOrdDestZip").data("kendoMaskedTextBox").value("");
                                $("#txtOrdDestCountry").data("kendoMaskedTextBox").value("");
                                $("#AddEditOrder").show();
                                $("#txtOrdCNS").focus();
                            }
                            else if ($(e.currentTarget).has("td")) {
                                $("#AddEditOrderLegend").html("<b>Order Details</b>");
                                orderItem = this.dataItem($(e.currentTarget).closest("tr"));
                                if (orderItem.BookControl > 0) {
                                    $("#txtOrdCNS").data("kendoMaskedTextBox").value(orderItem.BookConsPrefix);
                                    $("#txtOrdProNu").data("kendoMaskedTextBox").value(orderItem.BookProNumber);
                                    $("#txtOrdOrderNu").data("kendoMaskedTextBox").value(orderItem.BookCarrOrderNumber);
                                    $("#txtOrdPONu").data("kendoMaskedTextBox").value(orderItem.BookLoadPONumber);
                                    $("#dtOrdOrderd").data("kendoDatePicker").value(orderItem.BookDateOrdered);
                                    $("#dtOrdLoad").data("kendoDatePicker").value(orderItem.BookDateLoad);
                                    $("#dtOrdRequired").data("kendoDatePicker").value(orderItem.BookDateRequired);
                                    $("#txtOrdCases").data("kendoNumericTextBox").value(orderItem.BookTotalCases);
                                    $("#txtOrdCubes").data("kendoNumericTextBox").value(orderItem.BookTotalCube);
                                    $("#txtOrdWeight").data("kendoNumericTextBox").value(orderItem.BookTotalWgt);
                                    $("#txtOrdPalletsIn").data("kendoNumericTextBox").value(orderItem.BookTotalPL);
                                    $("#txtOrdPalletsOut").data("kendoNumericTextBox").value(orderItem.BookTotalPX);
                                    $("#txtOrdDesc").data("kendoMaskedTextBox").value(orderItem.BookItemDetailDescription);
                                    $("#txtOrdEquipID").data("kendoMaskedTextBox").value(orderItem.EquipmentID);
                                    $("#txtOrdOrig").data("kendoMaskedTextBox").value(orderItem.BookOrigName);
                                    $("#txtOrdOrigAddress").data("kendoMaskedTextBox").value(orderItem.BookOrigAddress1);
                                    $("#txtOrdOrigCity").data("kendoMaskedTextBox").value(orderItem.BookOrigCity);
                                    $("#txtOrdOrigState").data("kendoMaskedTextBox").value(orderItem.BookOrigState);
                                    $("#txtOrdOrigZip").data("kendoMaskedTextBox").value(orderItem.BookOrigZip);
                                    $("#txtOrdOrigCountry").data("kendoMaskedTextBox").value(orderItem.BookOrigCountry);
                                    $("#txtOrdDest").data("kendoMaskedTextBox").value(orderItem.BookDestName);
                                    $("#txtOrdDestAddress").data("kendoMaskedTextBox").value(orderItem.BookDestAddress1);
                                    $("#txtOrdDestCity").data("kendoMaskedTextBox").value(orderItem.BookDestCity);
                                    $("#txtOrdDestState").data("kendoMaskedTextBox").value(orderItem.BookDestState);
                                    $("#txtOrdDestZip").data("kendoMaskedTextBox").value(orderItem.BookDestZip);
                                    $("#txtOrdDestCountry").data("kendoMaskedTextBox").value(orderItem.BookDestCountry);
                                    $("#txtOrdCNS").data("kendoMaskedTextBox").enable(false);
                                    $("#txtOrdProNu").data("kendoMaskedTextBox").enable(false);
                                    $("#txtOrdOrderNu").data("kendoMaskedTextBox").enable(false);
                                    $("#txtOrdPONu").data("kendoMaskedTextBox").enable(false);
                                    $("#dtOrdOrderd").data("kendoDatePicker").enable(false);
                                    $("#dtOrdLoad").data("kendoDatePicker").enable(false);
                                    $("#dtOrdRequired").data("kendoDatePicker").enable(false);
                                    $("#txtOrdCases").data("kendoNumericTextBox").enable(false);
                                    $("#txtOrdCubes").data("kendoNumericTextBox").enable(false);
                                    $("#txtOrdWeight").data("kendoNumericTextBox").enable(false);
                                    $("#txtOrdPalletsIn").data("kendoNumericTextBox").enable(false);
                                    $("#txtOrdPalletsOut").data("kendoNumericTextBox").enable(false);
                                    $("#txtOrdDesc").data("kendoMaskedTextBox").enable(false);
                                    $("#txtOrdEquipID").data("kendoMaskedTextBox").enable(true);
                                    $("#txtOrdOrig").data("kendoMaskedTextBox").enable(false);
                                    $("#txtOrdOrigAddress").data("kendoMaskedTextBox").enable(false);
                                    $("#txtOrdOrigCity").data("kendoMaskedTextBox").enable(false);
                                    $("#txtOrdOrigState").data("kendoMaskedTextBox").enable(false);
                                    $("#txtOrdOrigZip").data("kendoMaskedTextBox").enable(false);
                                    $("#txtOrdOrigCountry").data("kendoMaskedTextBox").enable(false);
                                    $("#txtOrdDest").data("kendoMaskedTextBox").enable(false);
                                    $("#txtOrdDestAddress").data("kendoMaskedTextBox").enable(false);
                                    $("#txtOrdDestCity").data("kendoMaskedTextBox").enable(false);
                                    $("#txtOrdDestState").data("kendoMaskedTextBox").enable(false);
                                    $("#txtOrdDestZip").data("kendoMaskedTextBox").enable(false);
                                    $("#txtOrdDestCountry").data("kendoMaskedTextBox").enable(false);
                                } else {
                                    $("#txtOrdCNS").data("kendoMaskedTextBox").value(orderItem.BookConsPrefix);
                                    $("#txtOrdProNu").data("kendoMaskedTextBox").value(orderItem.BookProNumber);
                                    $("#txtOrdOrderNu").data("kendoMaskedTextBox").value(orderItem.BookCarrOrderNumber);
                                    $("#txtOrdPONu").data("kendoMaskedTextBox").value(orderItem.BookLoadPONumber);
                                    $("#dtOrdOrderd").data("kendoDatePicker").value(orderItem.BookDateOrdered);
                                    $("#dtOrdLoad").data("kendoDatePicker").value(orderItem.BookDateLoad);
                                    $("#dtOrdRequired").data("kendoDatePicker").value(orderItem.BookDateRequired);
                                    $("#txtOrdCases").data("kendoNumericTextBox").value(orderItem.BookTotalCases);
                                    $("#txtOrdCubes").data("kendoNumericTextBox").value(orderItem.BookTotalCube);
                                    $("#txtOrdWeight").data("kendoNumericTextBox").value(orderItem.BookTotalWgt);
                                    $("#txtOrdPalletsIn").data("kendoNumericTextBox").value(orderItem.BookTotalPL);
                                    $("#txtOrdPalletsOut").data("kendoNumericTextBox").value(orderItem.BookTotalPX);
                                    $("#txtOrdDesc").data("kendoMaskedTextBox").value(orderItem.BookItemDetailDescription);
                                    $("#txtOrdEquipID").data("kendoMaskedTextBox").value(orderItem.EquipmentID);
                                    $("#txtOrdOrig").data("kendoMaskedTextBox").value(orderItem.BookOrigName);
                                    $("#txtOrdOrigAddress").data("kendoMaskedTextBox").value(orderItem.BookOrigAddress1);
                                    $("#txtOrdOrigCity").data("kendoMaskedTextBox").value(orderItem.BookOrigCity);
                                    $("#txtOrdOrigState").data("kendoMaskedTextBox").value(orderItem.BookOrigState);
                                    $("#txtOrdOrigZip").data("kendoMaskedTextBox").value(orderItem.BookOrigZip);
                                    $("#txtOrdOrigCountry").data("kendoMaskedTextBox").value(orderItem.BookOrigCountry);
                                    $("#txtOrdDest").data("kendoMaskedTextBox").value(orderItem.BookDestName);
                                    $("#txtOrdDestAddress").data("kendoMaskedTextBox").value(orderItem.BookDestAddress1);
                                    $("#txtOrdDestCity").data("kendoMaskedTextBox").value(orderItem.BookDestCity);
                                    $("#txtOrdDestState").data("kendoMaskedTextBox").value(orderItem.BookDestState);
                                    $("#txtOrdDestZip").data("kendoMaskedTextBox").value(orderItem.BookDestZip);
                                    $("#txtOrdDestCountry").data("kendoMaskedTextBox").value(orderItem.BookDestCountry);
                                }
                                $("#AddEditOrder").show();
                            }
                        };

                        //*******OnChange Validation for new BookAdhoc Order********//
                        $("#txtOrdOrderNu").on("change input", function () {
                            if ($(this).val() != "") { $("#txtOrdOrderNu-validation").addClass("hide-display"); } else { $("#txtOrdOrderNu-validation").removeClass("hide-display"); }
                        });
                        $("#txtOrdPONu").on("change input", function () {
                            if ($(this).val() != "") { $("#txtOrdPONu-validation").addClass("hide-display"); } else { $("#txtOrdPONu-validation").removeClass("hide-display"); }
                        });

                        //************SAVE new BookAdhoc and BookOrder****************//
                        $("#btnSaveOrd").kendoButton({
                            icon: "save",
                            click: function (e) {
                                var submit = true;
                                var submit = true;
                                if ($("#txtOrdOrderNu").data("kendoMaskedTextBox").value() == "") { $("#txtOrdOrderNu-validation").removeClass("hide-display"); submit = false; }
                                if ($("#txtOrdPONu").data("kendoMaskedTextBox").value() == "") { $("#txtOrdPONu-validation").removeClass("hide-display"); submit = false; }
                                if (submit == true) {
                                    var ordData = new AMSOrders();
                                    if (orderItem.BookControl > 0 || orderItem.BookControl < 0) {
                                        ordData = orderItem;
                                        ordData.BookConsPrefix = $("#txtOrdCNS").data("kendoMaskedTextBox").value();
                                        ordData.BookProNumber = $("#txtOrdProNu").data("kendoMaskedTextBox").value();
                                        ordData.BookCarrOrderNumber = $("#txtOrdOrderNu").data("kendoMaskedTextBox").value();
                                        ordData.BookLoadPONumber = $("#txtOrdPONu").data("kendoMaskedTextBox").value();
                                        ordData.BookDateOrdered = new Date($("#dtOrdOrderd").val());
                                        ordData.BookDateLoad = new Date($("#dtOrdLoad").val());
                                        ordData.BookDateRequired = new Date($("#dtOrdRequired").val());
                                        ordData.BookTotalCases = $("#txtOrdCases").data("kendoNumericTextBox").value();
                                        ordData.BookTotalCube = $("#txtOrdCubes").data("kendoNumericTextBox").value();
                                        ordData.BookTotalWgt = $("#txtOrdWeight").data("kendoNumericTextBox").value();
                                        ordData.BookTotalPL = $("#txtOrdPalletsIn").data("kendoNumericTextBox").value();
                                        ordData.BookTotalPX = $("#txtOrdPalletsOut").data("kendoNumericTextBox").value();
                                        ordData.BookItemDetailDescription = $("#txtOrdDesc").data("kendoMaskedTextBox").value();
                                        ordData.EquipmentID = $("#txtOrdEquipID").data("kendoMaskedTextBox").value();
                                        ordData.BookOrigName = $("#txtOrdOrig").data("kendoMaskedTextBox").value();
                                        ordData.BookOrigAddress1 = $("#txtOrdOrigAddress").data("kendoMaskedTextBox").value();
                                        ordData.BookOrigCity = $("#txtOrdOrigCity").data("kendoMaskedTextBox").value();
                                        ordData.BookOrigState = $("#txtOrdOrigState").data("kendoMaskedTextBox").value();
                                        ordData.BookOrigZip = $("#txtOrdOrigZip").data("kendoMaskedTextBox").value();
                                        ordData.BookOrigCountry = $("#txtOrdOrigCountry").data("kendoMaskedTextBox").value();
                                        ordData.BookDestName = $("#txtOrdDest").data("kendoMaskedTextBox").value();
                                        ordData.BookDestAddress1 = $("#txtOrdDestAddress").data("kendoMaskedTextBox").value();
                                        ordData.BookDestCity = $("#txtOrdDestCity").data("kendoMaskedTextBox").value();
                                        ordData.BookDestState = $("#txtOrdDestState").data("kendoMaskedTextBox").value();
                                        ordData.BookDestZip = $("#txtOrdDestZip").data("kendoMaskedTextBox").value();
                                        ordData.BookDestCountry = $("#txtOrdDestCountry").data("kendoMaskedTextBox").value();
                                    } else {
                                        ordData.BookConsPrefix = $("#txtOrdCNS").data("kendoMaskedTextBox").value();
                                        ordData.BookProNumber = $("#txtOrdProNu").data("kendoMaskedTextBox").value();
                                        ordData.BookCarrOrderNumber = $("#txtOrdOrderNu").data("kendoMaskedTextBox").value();
                                        ordData.BookLoadPONumber = $("#txtOrdPONu").data("kendoMaskedTextBox").value();
                                        ordData.BookDateOrdered = new Date($("#dtOrdOrderd").val());
                                        ordData.BookDateLoad = new Date($("#dtOrdLoad").val());
                                        ordData.BookDateRequired = new Date($("#dtOrdRequired").val());
                                        ordData.BookTotalCases = $("#txtOrdCases").data("kendoNumericTextBox").value();
                                        ordData.BookTotalCube = $("#txtOrdCubes").data("kendoNumericTextBox").value();
                                        ordData.BookTotalWgt = $("#txtOrdWeight").data("kendoNumericTextBox").value();
                                        ordData.BookTotalPL = $("#txtOrdPalletsIn").data("kendoNumericTextBox").value();
                                        ordData.BookTotalPX = $("#txtOrdPalletsOut").data("kendoNumericTextBox").value();
                                        ordData.BookItemDetailDescription = $("#txtOrdDesc").data("kendoMaskedTextBox").value();
                                        ordData.EquipmentID = $("#txtOrdEquipID").data("kendoMaskedTextBox").value();
                                        ordData.BookOrigName = $("#txtOrdOrig").data("kendoMaskedTextBox").value();
                                        ordData.BookOrigAddress1 = $("#txtOrdOrigAddress").data("kendoMaskedTextBox").value();
                                        ordData.BookOrigCity = $("#txtOrdOrigCity").data("kendoMaskedTextBox").value();
                                        ordData.BookOrigState = $("#txtOrdOrigState").data("kendoMaskedTextBox").value();
                                        ordData.BookOrigZip = $("#txtOrdOrigZip").data("kendoMaskedTextBox").value();
                                        ordData.BookOrigCountry = $("#txtOrdOrigCountry").data("kendoMaskedTextBox").value();
                                        ordData.BookDestName = $("#txtOrdDest").data("kendoMaskedTextBox").value();
                                        ordData.BookDestAddress1 = $("#txtOrdDestAddress").data("kendoMaskedTextBox").value();
                                        ordData.BookDestCity = $("#txtOrdDestCity").data("kendoMaskedTextBox").value();
                                        ordData.BookDestState = $("#txtOrdDestState").data("kendoMaskedTextBox").value();
                                        ordData.BookDestZip = $("#txtOrdDestZip").data("kendoMaskedTextBox").value();
                                        ordData.BookDestCountry = $("#txtOrdDestCountry").data("kendoMaskedTextBox").value();
                                    }
                                    var Appt = {};
                                    if (EventAppt.AMSApptControl > 0) {
                                        EventAppt.AMSApptCarrierName = $("#txtCarrierName").data("kendoMaskedTextBox").value();
                                        EventAppt.AMSApptCarrierSCAC = $("#txtSCAC").data("kendoMaskedTextBox").value();
                                        EventAppt.AMSApptDescription = $("#txtDescription").data("kendoMaskedTextBox").value();
                                        EventAppt.AMSApptStartDate = $("#txtStartTime").val();
                                        EventAppt.AMSApptEndDate = $("#txtEndTime").val();
                                        EventAppt.AMSApptDockdoorID = $('#ddlDockDoorID').data("kendoDropDownList").dataItem().CompDockDockDoorID;
                                        EventAppt.AMSApptNotes = $("#txtNotes").data("kendoMaskedTextBox").value();
                                        Appt = EventAppt
                                    }
                                    var Ord = [];
                                    Ord[0] = ordData;
                                    var OrdData = {};
                                    OrdData = { "Appt": Appt, "Ord": Ord };
                                    $.ajax({
                                        url: '/api/AMSOrder/AddOrdersToAppointment/',
                                        type: 'Post',
                                        contentType: 'application/json; charset=utf-8',
                                        dataType: 'json',
                                        data: JSON.stringify(OrdData),
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                        success: function (data) {
                                            try {
                                                var blnSuccess = false;
                                                var blnErrorShown = false;
                                                var strValidationMsg = "";
                                                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                                    if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Add Orders To Appointment Failure", data.Errors, null); }
                                                    else {
                                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                                                blnSuccess = true;
                                                                var ordersGridDate = $("#ordersWndGrid").data("kendoGrid").dataSource.data();
                                                                ordersGridDate.push(data.Data[0])
                                                                var FordersGridDate = onlyUnique(ordersGridDate)
                                                                $("#ordersWndGrid").data("kendoGrid").dataSource.data(FordersGridDate);
                                                                $("#AddEditOrder").hide();
                                                                ngl.showSuccessMsg("Order Saved", "");
                                                            }
                                                        }
                                                    }
                                                }
                                                if (blnSuccess === false && blnErrorShown === false) {
                                                    if (strValidationMsg.length < 1) { strValidationMsg = "Add Orders To Appointment Failure"; }
                                                    ngl.showErrMsg("Add Orders To Appointment Failure", strValidationMsg, null);
                                                }
                                            } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                                        },
                                        error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure"); ngl.showErrMsg("Add Orders To Appointment Failure", sMsg, null); }
                                    });
                                }
                            }
                        });

                        function onlyUnique(mArray) {
                            var varyFresh = [];
                            var DummyFresh = [];
                            for (var i = 0; i < mArray.length; i++) {
                                if (DummyFresh.indexOf(mArray[i].BookControl) == -1) {
                                    DummyFresh.push(mArray[i].BookControl);
                                    varyFresh.push(mArray[i]);
                                }
                            }
                            return varyFresh;
                        }

                        //************CANCEL new BookAdhoc Order****************//
                        $("#btnCancelOrd").kendoButton({
                            icon: "cancel",
                            click: function (e) { $("#AddEditOrder").hide(); }
                        });

                        //************TrackingFieldsGrid****************//
                        dsApptTrackingFieldsGrid = new kendo.data.DataSource({
                            transport: {
                                read: function (options) {
                                    var s = new AllFilter();
                                    s.ApptControl = TrackingFieldsUserFieldsApptControl;
                                    $.ajax({
                                        url: '/api/AMSAppointment/GetAMSAppointmentTrackingDetails/',
                                        contentType: 'application/json; charset=utf-8',
                                        dataType: 'json',
                                        data: { filter: JSON.stringify(s) },
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                        success: function (data) {
                                            options.success(data);
                                            try {
                                                var blnSuccess = false;
                                                var blnErrorShown = false;
                                                var strValidationMsg = "";
                                                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                                    if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Get Appointment Tracking Details Failure", data.Errors, null); }
                                                    else {
                                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) { blnSuccess = true; } else { blnSuccess = true; strValidationMsg = "No records were found matching your search criteria"; }
                                                        }
                                                    }
                                                }
                                                if (blnSuccess === false && blnErrorShown === false) {
                                                    if (strValidationMsg.length < 1) { strValidationMsg = "Get Appointment Tracking Details Failure"; }
                                                    ngl.showErrMsg("Get Appointment Tracking Details Failure", strValidationMsg, null);
                                                }
                                            } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                                        },
                                        error: function (result) { options.error(result); }
                                    });
                                },
                                update: function (options) {
                                    options.data.AMSApptTrackingDateTime = kendo.toString(options.data.AMSApptTrackingDateTime, "M/d/yyyy HH:mm");
                                    $.ajax({
                                        url: 'api/AMSAppointment/UpdateAMSAppointmentTrackingFieldRecord',
                                        type: "POST",
                                        contentType: 'application/json; charset=utf-8',
                                        dataType: 'json',
                                        data: JSON.stringify(options.data),
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                        success: function (data) {
                                            try {
                                                if (data.Data[0]) { $("#TrackingFieldsGrid").data("kendoGrid").dataSource.read(); }
                                            } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                                        },
                                        error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure"); ngl.showErrMsg("Save Appointment Tracking Details Failure", sMsg, null); }
                                    });
                                },
                                parameterMap: function (options, operation) { return options; }
                            },
                            schema: {
                                data: "Data",
                                total: "Count",
                                model: {
                                    id: "AMSApptTrackingControl",
                                    fields: {
                                        AMSApptTrackingControl: { type: "number" },
                                        AMSApptTrackingName: { type: "string", editable: false },
                                        AMSApptTrackingDateTime: { type: "date" },
                                    }
                                },
                                errors: "Errors"
                            },
                            error: function (xhr, textStatus, error) { ngl.showErrMsg("Access Appointments Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges(); }
                        });

                        $("#TrackingFieldsGrid").kendoGrid({
                            height: 230,
                            noRecords: true,
                            autoBind: false,
                            editable: "inline",
                            dataSource: dsApptTrackingFieldsGrid,
                            columns: [
                                { command: [{ name: "edit", text: { edit: "", update: "", cancel: "" } }], title: "Action", width: "80px" },
                                { field: "AMSApptTrackingName", title: "Name" },
                                { field: "AMSApptTrackingDateTime", title: "DateTime", editor: dateTimeEditor, format: "{0:M/d/yyyy hh:mm tt}" },
                            ],
                        });

                        function dateTimeEditor(container, options) {
                            $('<input required data-text-field="Name" data-value-field="Control" data-bind="value:' + options.field + '"/>')
                                .appendTo(container)
                                .kendoDateTimePicker({
                                    format: "M/d/yyyy HH:mm",
                                    timeFormat: "HH:mm",
                                    value: kendo.toString(options.model.AMSApptTrackingDateTime, 'M/d/yyyy HH:mm')
                                });
                        }

                        //************UserFieldsGrid****************//
                        dsApptUserFieldsGrid = new kendo.data.DataSource({
                            transport: {
                                read: function (options) {
                                    var s = new AllFilter();
                                    s.ApptControl = TrackingFieldsUserFieldsApptControl;
                                    $.ajax({
                                        url: '/api/AMSAppointment/GetAMSAppointmentUserFieldDetails/',
                                        contentType: 'application/json; charset=utf-8',
                                        dataType: 'json',
                                        data: { filter: JSON.stringify(s) },
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                        success: function (data) {
                                            options.success(data);
                                            try {
                                                var blnSuccess = false;
                                                var blnErrorShown = false;
                                                var strValidationMsg = "";
                                                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                                    if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Get Appointment User Details Failure", data.Errors, null); }
                                                    else {
                                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) { blnSuccess = true; } else { blnSuccess = true; strValidationMsg = "No records were found matching your search criteria"; }
                                                        }
                                                    }
                                                }
                                                if (blnSuccess === false && blnErrorShown === false) {
                                                    if (strValidationMsg.length < 1) { strValidationMsg = "Get Appointment User Details Failure"; }
                                                    ngl.showErrMsg("Get Appointment User Details Failure", strValidationMsg, null);
                                                }
                                            } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                                        },
                                        error: function (result) { options.error(result); }
                                    });
                                },
                                update: function (options) {
                                    $.ajax({
                                        url: 'api/AMSAppointment/UpdateAMSAppointmentUserFieldRecord',
                                        type: "POST",
                                        contentType: 'application/json; charset=utf-8',
                                        dataType: 'json',
                                        data: JSON.stringify(options.data),
                                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                        success: function (data) {
                                            try {
                                                if (data.Data[0]) { $("#UserFieldsGrid").data("kendoGrid").dataSource.read(); }
                                            } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                                        },
                                        error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure"); ngl.showErrMsg("Save Appointment User Details Failure", sMsg, null); }
                                    });
                                },
                                parameterMap: function (options, operation) { return options; }
                            },
                            schema: {
                                data: "Data",
                                total: "Count",
                                model: {
                                    id: "AMSApptUFDControl",
                                    fields: {
                                        AMSApptUFDControl: { type: "number" },
                                        AMSApptUFDName: { type: "string", editable: false },
                                        AMSApptUFDData: { type: "string" },
                                    }
                                },
                                errors: "Errors"
                            },
                            error: function (xhr, textStatus, error) { ngl.showErrMsg("Access Appointments Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges(); }
                        });

                        $("#UserFieldsGrid").kendoGrid({
                            height: 240,
                            noRecords: true,
                            autoBind: false,
                            editable: "inline",
                            dataSource: dsApptUserFieldsGrid,
                            columns: [
                                { command: [{ name: "edit", text: { edit: "", update: "", cancel: "" } }], title: "Action", width: "80px" },
                                { field: "AMSApptUFDName", title: "Name" },
                                { field: "AMSApptUFDData", title: "Value" },
                            ],
                        });

                        //************Popup Window for ADD EDIT Appointments*****************//
                        var windowElement = $("#AppointmentAddEditWnd").kendoWindow({
                            height: 'auto',
                            minHeight: 400,
                            maxHeight: 600,
                            width: 950,
                            minWidth: 900,
                            //maxWidth:950,
                            modal: true,
                            visible: false,
                            resize: resizeOrdersGrid,
                            close: function (e) { $("#tabstrip").data("kendoTabStrip").select("li:contains('Appointment Information')"); },
                        });
                        wndAddEditEvent = $("#AppointmentAddEditWnd").data("kendoWindow");
                        var windowWrapper = windowElement.closest(".k-window");
                        function resizeOrdersGrid() { $("#ordersWndGrid").css("width", parseInt(windowWrapper.css("width")) - 85 + 'px') }
                        resizeOrdersGrid();

                        //*********Override Window**************//
                        $("#overrideWindow").kendoWindow({
                            height: 'auto',
                            width: '600',
                            modal: true,
                            visible: false,
                            close: function () { scheduler.dataSource.read(); },
                        });

                        wndOverride = $("#overrideWindow").data("kendoWindow");


                        //***********Scroll Current Time********//
                        if (stateScrollOnOff) {
                            var currentTime = new Date();
                            var hours = currentTime.getHours();
                            var minutes = currentTime.getMinutes();
                            if (minutes >= 30) { minutes = 30; } else { minutes = 0; }
                            scrollToCurrentTime(hours, minutes);
                        }
                        //************UserPageSettings SavaUpdate Function***********//               
                        function InsertOrUpdateCurrentUserPageSetting(pSettingName) {
                            var userSettings = {};
                            userSettings.CompanyID = $("#ddlwarehouse").val();
                            var allDocks = [];
                            jQuery.each(dsAllDockDoorsApptTimeSettings, function (i, val) {
                                allDocks.push(val.DockControl);
                            });
                            userSettings.ViewDockDoors = allDocks;
                            userSettings.CalenderView = scheduler.view().name;
                            if (scheduler.view().name == "month") {
                                userSettings.CalenderDate = scheduler.view()._startDate;
                                var numberOfDaysToAdd = 8;
                                userSettings.CalenderDate.setDate(userSettings.CalenderDate.getDate() + numberOfDaysToAdd);
                            } else { userSettings.CalenderDate = scheduler.view()._startDate; }
                            userSettings.ScrollOnOff = stateScrollOnOff;
                            //debugger;
                            //Modified by RHR for v-8.2 on 09/15/2018
                            //fixed bug where dsUserPageSettings is missing 
                            //var UserPageSettingsData = new UserPageSetting();
                            var UserPageSettingsData = new PageSettingModel();
                        //if(typeof(dsUserPageSettings) !== 'undefined' &&  dsUserPageSettings !== null && dsUserPageSettings.UserPSControl > 0){
                        //    UserPageSettingsData.UserPSControl = dsUserPageSettings.UserPSControl;
                        //}
                        <%--UserPageSettingsData.UserPSUserSecurityControl = '<%=UserControl%>';--%>
                            //UserPageSettingsData.UserPSPageControl = PageControl;
                            //UserPageSettingsData.UserPSName = pSettingName;
                            //UserPageSettingsData.UserPSMetaData = JSON.stringify(userSettings);                  
                            UserPageSettingsData.name = pSettingName;
                            UserPageSettingsData.value = JSON.stringify(userSettings);
                            $.ajax({
                                url: '/api/Scheduler/PostPageSetting/',
                                type: 'Post',
                                contentType: 'application/json; charset=utf-8',
                                dataType: 'json',
                                data: JSON.stringify(UserPageSettingsData),
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                success: function (data) {
                                    //debugger;
                                    try {
                                        var blnSuccess = false;
                                        var blnErrorShown = false;
                                        var strValidationMsg = "";
                                        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                            if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Save User Page Setting Failure", data.Errors, null); }
                                            else {
                                                if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                    if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') { blnSuccess = true; }
                                                }
                                            }
                                        }
                                        if (blnSuccess === false && blnErrorShown === false) {
                                            if (strValidationMsg.length < 1) { strValidationMsg = "Save User Page Setting Failure"; }
                                            ngl.showErrMsg("Save User Page Setting Failure", strValidationMsg, null);
                                        }
                                    } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                                },
                                error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure"); ngl.showErrMsg("Save User Page Setting Failure", sMsg, null); }
                            });
                            //$.ajax({ 
                            //    url: '/api/UserPageSetting/InsertOrUpdateCurrentUserPageSetting/',
                            //    type:'Post',
                            //    contentType: 'application/json; charset=utf-8', 
                            //    dataType: 'json', 
                            //    data: JSON.stringify(UserPageSettingsData), 
                            //    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                            //    success: function(data) {
                            //        debugger;
                            //        try {
                            //            var blnSuccess = false;
                            //            var blnErrorShown = false;
                            //            var strValidationMsg = "";
                            //            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                            //                if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                            //                    blnErrorShown = true;
                            //                    ngl.showErrMsg("Save User Page Setting Failure", data.Errors, null);
                            //                }
                            //                else {
                            //                    if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                            //                        if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                            //                            blnSuccess = true;
                            //                        }
                            //                    }
                            //                }
                            //            }
                            //            if (blnSuccess === false && blnErrorShown === false) {
                            //                if (strValidationMsg.length < 1) { strValidationMsg = "Save User Page Setting Failure"; }
                            //                ngl.showErrMsg("Save User Page Setting Failure", strValidationMsg, null);
                            //            }
                            //        } catch (err) {
                            //            ngl.showErrMsg(err.name, err.description, null);
                            //        }
                            //    },
                            //    error: function (xhr, textStatus, error) {
                            //        var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure");
                            //        ngl.showErrMsg("Save User Page Setting Failure", sMsg, null);                        
                            //    }
                            //});
                        }

                        //add code above to load screen specific information this is only visible when a user is authenticated
                    }, 10, this);
                }
                var PageReadyJS = <%=PageReadyJS%>;
                menuTreeHighlightPage(); //must be called after PageReadyJS
                var divWait = $("#h1Wait");
                if (typeof (divWait) !== 'undefined') { divWait.hide(); }
            });
        </script>
        <style>
            .hide-display {
                display: none;
            }

            .breakWord20 {
                word-break: break-all !important;
                word-wrap: break-word !important;
                vertical-align: top;
            }

            .k-grid-header .k-header {
                overflow: visible !important;
                white-space: normal !important;
            }

            #LoopDesc {
                text-overflow: ellipsis !important;
            }

            .ui-container {
                margin: 1%;
                height: 100% !important;
                width: 100% !important;
                margin-top: 2px !important;
            }

            .ui-vertical-container {
                height: 98% !important;
                width: 98% !important;
            }

            .ui-horizontal-container {
                height: 100% !important;
                width: 100% !important;
            }

            .ui-legend-container {
                color: black;
                font-weight: bold;
            }

            .ui-fieldset-container {
                border-color: #BBDCEB;
                border-width: 1px;
                width: 1165px;
                margin: 10px;
                margin-left: 30px;
            }

            .ui-border-container {
                margin: 20px;
                font-size: 1.1em;
            }

            .ui-padding-container {
                padding: 10px;
            }

            /*h2 { margin: 0; font-size: 1em; }*/

            table {
                border-collapse: collapse;
            }

            table, th, td {
                border: 0.3px solid rgba(0%,0%,0%,.7);
                color: black;
            }

            .ui-th-margin {
                width: 125px;
            }

            .ui-td-margin {
                padding-left: 1px;
                width: 200px;
            }

            .ui-button-margin {
                width: 100px;
            }

            .ui-min-margin {
                width: 80px;
            }

            .ui-span-container {
                font-size: small;
                font-weight: bold;
                color: red;
                position: relative;
                bottom: 5px;
            }

            .OR-btn-width {
                width: 125px;
            }

            .MTop {
                margin-top: 15px;
            }

            .MBottom {
                margin-bottom: 15px;
            }

            .MLeft {
                margin-left: 5px;
            }

            /*#btnFindMyAppt { margin-left: 3px; }*/

            fieldset {
                border-color: #BBDCEB;
            }

            .event-templatediv p {
                margin: 5px 8px 2px;
            }

            #scheduler table, td {
                border: 0 solid rgba(0%,0%,0%,7);
            }

            .zeroColor {
                background-color: coral;
            }

            .oneColor {
                background-color: chocolate;
            }

            .twoColor {
                background-color: darkgoldenrod;
            }

            .green {
                background-color: green;
                color: black;
            }

            .yellow {
                background-color: yellow;
                color: black;
            }

            .darkblue {
                background-color: darkblue;
            }

            .darkred {
                background-color: darkred;
            }

            .darkcyan {
                background-color: darkcyan;
            }

            .darkgoldenrod {
                background-color: darkgoldenrod;
            }



            .gray {
                background-color: gray;
            }

            #submenu {
                list-style-type: none;
                padding: 5px;
                height: auto;
                width: auto;
                min-width: 74px;
                background-color: #d9ecf5;
                border: 1px solid #a3d0e4;
                border-radius: 4px;
                display: none;
            }

            #dragTooltip h3 {
                margin: 0;
            }

            /*.ApptAddEditDiv table{
                width: 100%;
            }*/

            #searchOrdersUI .ui-th-margin {
                min-width: 80px;
                max-width: 80px;
            }

            #searchOrdersUI .ui-td-margin {
                padding-left: 1px;
                width: 25%;
                min-width: 100px;
            }

            .ApptAddEditDiv .ui-th-margin {
                min-width: 125px;
                max-width: 125px;
                vertical-align: initial !important;
            }

            .ApptAddEditDiv .ui-td-margin {
                padding-left: 1px;
                width: 50%;
                min-width: 200px;
            }

            #OrdersAddEditUI .ui-th-margin {
                min-width: 125px;
                max-width: 125px;
                vertical-align: initial !important;
            }

            #OrdersAddEditUI .ui-td-margin {
                padding-left: 1px;
                width: 50%;
                min-width: 200px;
            }

            #carrierDataUI .ui-th-margin {
                min-width: 200px;
                max-width: 200px;
            }

            #carrierDataUI .ui-td-margin {
                padding-left: 1px;
                width: 50%;
                min-width: 200px;
            }

            /*#allDocks:hover + #submenu{
                display: inline;
            }
            #submenu:hover {
                display:inline;
            }*/

            /*---------------------------------------------------------*/

            .k-menu {
                color: red;
            }

            }

            .k-today, .k-nonwork-hour, .k-today.k-nonwork-hour {
                background-color: #FFFFFF !important;
            }

                .customNonwork, .k-today.k-nonwork-hour.customNonwork {
                    background-color: #DDD !important;
                }

            .k-grid tbody .k-button {
                min-width: 32px;
                width: 32px;
            }

            .k-grid-bookAppt {
                min-width: 80px !important;
                width: 80px !important;
            }

            .k-grid tbody tr td {
                vertical-align: initial !important;
            }

            #contextMenu {
                height: auto !important;
            }

            .k-button {
                font-weight: bold !important;
            }

            .k-scheduler-header-wrap > table > tbody > tr:last-child {
                background: #d9ecf5 !important;
            }

            .tblResponsive .tblResponsive-top {
                vertical-align: initial !important;
            }

            .k-scheduler-header th {
                text-overflow: initial !important;
                box-sizing: content-box;
                vertical-align: initial !important;
            }

            .k-event:hover .k-event-delete, tr:hover > td > .k-task .k-event-delete {
                display: none !important;
            }

            .k-scheduler-monthview .k-today {
                background: #5f5f5f !important;
            }

            /* Hide toolbar, navigation and footer during export */
            .k-pdf-export .k-scheduler-toolbar,
            .k-pdf-export .k-scheduler-navigation .k-nav-today,
            .k-pdf-export .k-scheduler-navigation .k-nav-prev,
            .k-pdf-export .k-scheduler-navigation .k-nav-next,
            .k-pdf-export .k-scheduler-footer {
                display: none;
            }
        </style>
    </div>
</body>

</html>
