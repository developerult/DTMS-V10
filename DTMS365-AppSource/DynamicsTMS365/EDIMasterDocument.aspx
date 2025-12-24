<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EDIMasterDocument.aspx.cs" Inherits="DynamicsTMS365.EDIMasterDocument" %>

<!DOCTYPE html>

<html>
<head>
    <title>Master Document</title>         
        <link href="Content/kendoR32023/classic-opal.css" rel="stylesheet" />               
        <link href="Content/NGL/v-8.5.4.001/common.css" rel="stylesheet" />   
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

        .hide-display {
            display: none;
        }
        .disable{

        }
        .disable:before{
            color:#726c6c;
        }
        .ui-container {
            height: 100% !important; width: 100% !important; margin-top: 2px !important;
        }
        .ui-vertical-container {
            height: 98% !important; width: 98% !important; 
        }
        /*.ui-sidemenu-container {
            height: 100% !important; width: 100% !important; background-color: white !important;
        }*/
        .ui-horizontal-container {
            height: 100% !important; width: 100% !important;
        }
        .ui-padding-container {
            padding: 10px;
        }
        .ui-span-container {
            font-size: small; font-weight: bold;
        }

    </style>

</head>
<body>
    <script src="https://code.jquery.com/jquery-3.4.1.min.js"></script>
    <script src="Scripts/kendoR32023/kendo.all.min.js"></script>
<script>kendo.ui['Button'].fn.options['size'] = "small";</script>
    <script src="https://secure.aadcdn.microsoftonline-p.com/lib/1.0.14/js/adal.min.js"></script>
    <script src="Scripts/NGL/v-8.5.4.006/core.js"></script>
    <script src="Scripts/NGL/v-8.5.4.006/NGLobjects.js"></script>
    <script src="Scripts/NGL/v-8.5.4.006/splitter2.js"></script>
    <script src="Scripts/NGL/v-8.5.4.006/menuitems.js"></script>
    <!-- added by SRP on 2/22/2018 For Dynamically adding menu items for all pages -->
    <script src="Scripts/NGL/v-8.5.4.006/windowconfig.js"></script>
    <!--added by SRP on 2/22/2018 For Editing KendoWindow Configuration from Javascript -->

    <div id="example" class="ui-container">
        <div id="vertical" class="ui-vertical-container">
            <%--Action Menu TabStrip--%>
            <div id="menu-pane" style="height: 100%; width: 100%; background-color: white;">
                <div id="tab" class="menuBarTab"></div>
            </div>
            <div id="top-pane">
                <div id="horizontal" class="ui-horizontal-container">
                    <div id="left-pane">
                        <div class="pane-content">
                            <div><span>Menu</span></div>
                            <%--Page Navigation Menu Tree--%>
                            <div id="menuTree"></div>
                        </div>
                    </div>
                    <div id="center-pane">
                        <!-- Begin Page Content -->

                        <%--Message--%>
                        <div id="txtScreenMessage"></div>

                        <!-- Grid Fast Tab -->
                        <div id="id260" style="margin-top: 10px;">
                            <div class="fast-tab">
                                <span id="ExpandParSpan" style="display: none;"><a onclick="expandFastTab('ExpandParSpan','CollapseParSpan','ParHeader','ParDetail');"><span class="k-icon k-i-chevron-down ui-span-container"></span></a></span>
                                <span id="CollapseParSpan" style="display: normal;"><a onclick="collapseFastTab('ExpandParSpan','CollapseParSpan','ParHeader',null);"><span class="k-icon k-i-chevron-up ui-span-container"></span></a></span>
                                <span class="ui-span-container">Master Documents</span>
                            </div>
                            <div id="ParHeader" class="OpenOrders">
                                <div id="Parwrapper">
                                    <%--Filters--%>
                                    <div id="ParFilterFastTab">
                                        <span id="ExpandParFilterFastTabSpan" style="display: none;"><a onclick="expandFastTab('ExpandParFilterFastTabSpan','CollapseParFilterFastTabSpan','ParFilterFastTabHeader',null);"><span class="k-icon k-i-chevron-down ui-span-container"></span></a></span>
                                        <span id="CollapseParFilterFastTabSpan" style="display: normal;"><a onclick="collapseFastTab('ExpandParFilterFastTabSpan','CollapseParFilterFastTabSpan','ParFilterFastTabHeader',null);"><span class="k-icon k-i-chevron-up ui-span-container"></span></a></span>
                                        <span class="ui-span-container">Filters </span>
                                        <div id="ParFilterFastTabHeader" class="ui-padding-container">
                                            <span>
                                                <label for="ddlEDIMasterDocFilters">Filter by:</label>
                                                <input id="ddlEDIMasterDocFilters" />
                                                <span id="spMasterDocFilterText">
                                                    <input id="txtMasterDocFilterVal" /></span>
                                                <span id="spMasterDocFilterDates">
                                                    <label for="dpMasterDocFilterFrom">From:</label>
                                                    <input id="dpMasterDocFilterFrom" />
                                                    <label for="dpMasterDocFilterTo">To:</label>
                                                    <input id="dpMasterDocFilterTo" />
                                                </span>
                                                <span id="spMasterDocFilterButtons"><a id="btnMasterDocFilter"></a><a id="btnMasterDocClearFilter"></a></span>
                                            </span>
                                            <input id="txtMasterDocSortDirection" type="hidden" />
                                            <input id="txtMasterDocSortField" type="hidden" />
                                        </div>
                                    </div>
                                    <%--Grid--%>
                                    <div id="MasterDocGrid"></div>
                                </div>
                            </div>
                        </div>

                        <!-- End Page Content -->

                    </div>
                </div>
            </div>
            <div id="bottom-pane" class="k-block ui-horizontal-container">
                <div class="pane-content">
                    <div><span>
                        <p>If you experience problems with this site, call (847)963-0007 24/7 or email our support group at <a href='mailto: support@nextgeneration.com'>support@nextgeneration.com</a></p>
                    </span></div>
                </div>
            </div>
        </div>


        <%--Added By SRP on 2/22/2018 EDIMasterDocument for KendoWindow--%>
        <% Response.WriteFile("~/Views/EDIMasterDocumentAddWindow.html"); %>

        <% Response.Write(AuthLoginNotificationHTML); %>
        <script>
        //************* Page Variables **************************
            var PageControl = '<%=PageControl%>';
            var tObj = this;
            var tPage = this;
        var oKendoGrid = null;
        var dsDocTypeDropDown = kendo.data.DataSource;
        var wndMessage = kendo.ui.Window;
        var wndAddMasterDoc = kendo.ui.Window; 
        var ddlTransaction = [{ TrControl:true, TrName:"InBound"},{ TrControl:false, TrName:"OutBound"}];
        var ddlPublish = [{ PbControl:false, PbName:"No"},{ PbControl:true, PbName:"Yes"}];

        //*************  Call Back Functions  *******************
        function MasterDocGridDataBoundCallBack(e,tGrid){           
            oKendoGrid = tGrid;
        }

        //*************  Action Functions  **********************
        function execActionClick(btn, proc){
         
            if (btn.id == "btnAddMasterDoc") {
                openMasterDocAddWindow();
            }
        }

        //Added By By S on 2/22/2018 EDIElementAddSample
        function openMasterDocAddWindow() {
            //Validation Display
            var Doccheck = $("#name-validation").hasClass("hide-display");
            if (Doccheck == false) {
                $("#name-validation").addClass("hide-display");
            }
            // title of the window 
            $("#wndAddMasterDoc").data("kendoWindow").title("Add Master Document");

            //Clear all previous values since this is Add New
            $("#txtMasterDocControl").val(0);

            var ddlMasterDocEDITControl = $("#ddlMasterDocEDITControl").data("kendoDropDownList");
            ddlMasterDocEDITControl.readonly(false);
            ddlMasterDocEDITControl.select(0);
           
            var ddlMasterDocInbound = $("#ddlMasterDocInbound").data("kendoDropDownList");
            ddlMasterDocInbound.readonly(false);
            ddlMasterDocInbound.select(0);
      
            var ddlMasterDocPublished = $("#ddlMasterDocPublished").data("kendoDropDownList");
            ddlMasterDocPublished.readonly(false);
            ddlMasterDocPublished.select(0);

            $("#chkMasterDocDisabled").prop('checked', false);
            wndAddMasterDoc.center().open();
        }
          //***********Edit MDoc************//
        function editMDoc(e){
            var eitem = this.dataItem($(e.currentTarget).closest("tr")); 
            //Validation Display
            var Doccheck = $("#name-validation").hasClass("hide-display");
            if (Doccheck == false) {
                $("#name-validation").addClass("hide-display");
            }

            // title of the window 
            $("#wndAddMasterDoc").data("kendoWindow").title("Edit Master Document");

            $("#txtMasterDocControl").val(eitem.MasterDocControl);
            var ddl= $("#ddlMasterDocEDITControl").data("kendoDropDownList");
            ddl.value(parseInt(eitem.MasterDocEDITControl));

            var ddl= $("#ddlMasterDocInbound").data("kendoDropDownList");
            ddl.value(Boolean(eitem.MasterDocInbound));

            var ddl= $("#ddlMasterDocPublished").data("kendoDropDownList");
            ddl.value(Boolean(eitem.MasterDocPublished));

            $("#txtMasterDocCreateDate").val(eitem.MasterDocCreateDate);
            $("#txtMasterDocCreateUser").val(eitem.MasterDocCreateUser);
            $("#txtMasterDocModDate").val(eitem.MasterDocModDate);
            $("#txtMasterDocModUser").val(eitem.MasterDocModUser);
            $("#txtMasterDocModUpdated").val(eitem.MasterDocModUpdated);
            $("#chkMasterDocDisabled").prop('checked', eitem.MasterDocDisabled);

            wndAddMasterDoc.center().open();
        }

        //Added By By SRP on 2/22/2018 EDIElementAddSample
        function SaveEDIMasterDoc() {
            var otmp = $("#focusCancel").focus();

            //Created by SRP on 02/19/2018 for Input Validation

            var submit = true;
            var Document = $("#ddlMasterDocEDITControl").val();

            if (Document =="") {

                $("#name-validation").removeClass("hide-display");
                submit = false;
            }
           
            var item = new NGLElement();

            var MasterDocControl = $("#txtMasterDocControl").val();

            if(MasterDocControl == 0){

            item.MasterDocControl = $("#txtMasterDocControl").val();
            var dataItem1 = $("#ddlMasterDocEDITControl").data("kendoDropDownList").dataItem();
            item.MasterDocEDITControl  = dataItem1.EDITControl;

            var dataItem2 = $("#ddlMasterDocInbound").data("kendoDropDownList").dataItem();
            item.MasterDocInbound  = dataItem2.TrControl;

            var dataItem3 = $("#ddlMasterDocPublished").data("kendoDropDownList").dataItem();
            item.MasterDocPublished  = dataItem3.PbControl;

            item.MasterDocDisabled= $("#chkMasterDocDisabled").is(":checked");
        
            if (submit == true) {
                var checkMasterAvaile = false;
                $.ajax({
                    async: false,
                    url: "api/EDIMasterDocument/CheckExist/",
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    data: { MasterDocEDITControl:item.MasterDocEDITControl,MasterDocInbound:item.MasterDocInbound },
                    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                    success: function (data) {
                        try {
                            var blnSuccess = false;
                            var blnErrorShown = false;
                            var strValidationMsg = "";
                            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                    blnErrorShown = true;
                                    ngl.showErrMsg("EDI MasterDoc Failure", data.Errors, null);
                                }
                                else {
                                    if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                        if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                            blnSuccess = true;
                                            submit = false;
                                            ngl.showWarningMsg("A Master document already exists.", strValidationMsg);
                                        }else{
                                            checkMasterAvaile = true;
                                        }
                                    }
                                }
                            }
                            if (blnSuccess === false && blnErrorShown === false) {
                           
                            }
                        } catch (err) {
                            submit = false;
                            ngl.showErrMsg(err.name, err.description, null);
                        }
                    },
                    error: function (xhr, textStatus, error) {
                        var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save EDI MasterDoc Failure");
                        ngl.showErrMsg("EDI MasterDoc Failure", sMsg, null);
                    }
                });
                if(checkMasterAvaile == true){
                    $.ajax({
                        async: false,
                        type: "POST",
                        url: "api/EDIMasterDocument/SaveEDIMasterDocument/",
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
                                        ngl.showErrMsg("Save EDI MasterDoc Failure", data.Errors, null);
                                    }
                                    else {
                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                                blnSuccess = true;
                                                refreshMasterDocGrid();
                                            }
                                            else
                                            {
                                                ngl.showWarningMsg("A document already exists.<br><br>Please check the list of Document Master.", strValidationMsg);
                                            }
                                        }
                                    }
                                }
                                if (blnSuccess === false && blnErrorShown === false) {
                                    if (strValidationMsg.length < 1) { strValidationMsg = "Save EDI MasterDoc Failure"; }
                                    ngl.showErrMsg("Save EDI MasterDoc Failure", strValidationMsg, null);
                                }
                            } catch (err) {
                                ngl.showErrMsg(err.name, err.description, null);
                            }
                        },
                        error: function (xhr, textStatus, error) {
                            var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save EDI MasterDoc Failure");
                            ngl.showErrMsg("Save EDI MasterDoc Failure", sMsg, null);
                        }
                    });
                    wndAddMasterDoc.close();
                  }
                }
            }else{

                item.MasterDocControl = $("#txtMasterDocControl").val();
                var dataItem1 = $("#ddlMasterDocEDITControl").data("kendoDropDownList").dataItem();
                item.MasterDocEDITControl  = dataItem1.EDITControl;

                var dataItem2 = $("#ddlMasterDocInbound").data("kendoDropDownList").dataItem();
                item.MasterDocInbound  = dataItem2.TrControl;

                var dataItem3 = $("#ddlMasterDocPublished").data("kendoDropDownList").dataItem();
                item.MasterDocPublished  = dataItem3.PbControl;

                item.MasterDocCreateDate = $("#txtMasterDocCreateDate").val();
                item.MasterDocCreateUser = $("#txtMasterDocCreateUser").val();
                item.MasterDocModDate = $("#txtMasterDocModDate").val();
                item.MasterDocModUser = $("#txtMasterDocModUser").val();
                item.MasterDocModUpdated = $("#txtMasterDocModUpdated").val();

                item.MasterDocDisabled= $("#chkMasterDocDisabled").is(":checked");
                $.ajax({
                    async: false,
                    url: "api/EDIMasterDocument/GetRecord/",
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    data: { MasterDocControl:item.MasterDocControl },
                    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                    success: function (data) {
                        try {
                            var blnSuccess = false;
                            var blnErrorShown = false;
                            var strValidationMsg = "";
                            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                    blnErrorShown = true;
                                    ngl.showErrMsg("EDI MasterDoc Failure", data.Errors, null);
                                }
                                else {
                                    if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                        if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                            blnSuccess = true;
                                            submit = true;
                                            if(data.Data[0].MasterDocEDITControl!=item.MasterDocEDITControl && data.Data[0].MasterDocInbound!=item.MasterDocInbound)
                                            {$.ajax({
                                                async: false,
                                                url: "api/EDIMasterDocument/CheckExist/",
                                                contentType: "application/json; charset=utf-8",
                                                dataType: 'json',
                                                data: { MasterDocEDITControl:data.Data[0].MasterDocEDITControl,MasterDocInbound:data.Data[0].MasterDocInbound },
                                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                                success: function (data) {
                                                    try {
                                                        var blnSuccess = false;
                                                        var blnErrorShown = false;
                                                        var strValidationMsg = "";
                                                        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                                            if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                                                blnErrorShown = true;
                                                                ngl.showErrMsg("EDI MasterDoc Failure", data.Errors, null);
                                                            }
                                                            else {
                                                                if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                                    if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                                                        blnSuccess = true;
                                                                        submit = false;
                                                                        ngl.showWarningMsg("A Master document already exists.", strValidationMsg);
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        if (blnSuccess === false && blnErrorShown === false) {
                           
                                                        }
                                                    } catch (err) {
                                                        submit = false;
                                                        ngl.showErrMsg(err.name, err.description, null);
                                                    }
                                                },
                                                error: function (xhr, textStatus, error) {
                                                    var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save EDI MasterDoc Failure");
                                                    ngl.showErrMsg("EDI MasterDoc Failure", sMsg, null);
                                                }
                                            });
                                            }
                                        }
                                    }
                                }
                            }
                            if (blnSuccess === false && blnErrorShown === false) {
                           
                            }
                        } catch (err) {
                            submit = false;
                            ngl.showErrMsg(err.name, err.description, null);
                        }
                    },
                    error: function (xhr, textStatus, error) {
                        var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save EDI MasterDoc Failure");
                        ngl.showErrMsg("EDI MasterDoc Failure", sMsg, null);
                    }
                });
                
                if (submit == true) {
                    $.ajax({
                        async: false,
                        type: "POST",
                        url: "api/EDIMasterDocument/PostSave",
                        contentType: "application/json; charset=utf-8",
                        dataType: 'json',
                        data: JSON.stringify(item),
                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                        success: function (data) {
                            try {
                                var blnSuccess = false;
                                var blnErrorShown = false;
                                var strValidationMsg = "";
                                if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                    blnSuccess = true;
                                    refreshMasterDocGrid();
                                           
                                }
                                else{
                                    if (blnSuccess === false && blnErrorShown === false) {
                                        if (strValidationMsg.length < 1) { strValidationMsg = "Update EDI MasterDoc Failure"; }
                                        ngl.showErrMsg("Update EDI MasterDoc Failure", strValidationMsg, null);
                                    }
                                }
                            } catch (err) {
                                ngl.showErrMsg(err.name, err.description, null);
                            }
                        },
                        error: function (xhr, textStatus, error) {
                            var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Update EDI MasterDoc Failure");
                            ngl.showErrMsg("Update EDI MasterDoc Failure", sMsg, null);
                        }
                    });
                    wndAddMasterDoc.close();
                }

           }
            

        }

        $("#ddlMasterDocEDITControl").on("change input", function () {
            var datatypesval = $(this).val();
            if (datatypesval != 0) {
                $("#name-validation").addClass("hide-display");
            }
            else {
                $("#name-validation").removeClass("hide-display");
            }

        });

        function refreshMasterDocGrid() {
            //oKendoGrid gets set during MasterDocGridDataBoundCallBack()
            if (typeof (oKendoGrid) !== 'undefined' && ngl.isObject(oKendoGrid)) {
                oKendoGrid.dataSource.read();
            }
           
        }
            //*************EditDocument*************//
        function EditDocument(e){
            var d = this.dataItem($(e.currentTarget).closest("tr")); 

            var ddl= $("#lblMasterDocEDIName").data("kendoDropDownList");
            ddl.value(parseInt(d.MasterDocEDITControl));
            var MDocName = $("#lblMasterDocEDIName").data("kendoDropDownList").text();

            window.location.assign("EDIMasterDocStructLoops?DocControl="+d.MasterDocControl+"&MDocName="+MDocName+"&TPDocInbound="+d.MasterDocInbound+""); 
        }
            //**********Publish Change in Data**********//
        function publishDocument(e) {
            var pitem = this.dataItem($(e.currentTarget).closest("tr"));
            

            var MasterDocControl = pitem.MasterDocControl
            var MasterDocEDITControl = pitem.MasterDocEDITControl;
            var MasterDocInbound = pitem.MasterDocInbound;
            var MasterDocPublished = pitem.MasterDocPublished;
            var MasterDocDisabled = pitem.MasterDocDisabled;
            var MasterDocCreateDate = pitem.MasterDocCreateDate;
            var MasterDocCreateUser = pitem.MasterDocCreateUser;
            var MasterDocModDate = pitem.MasterDocModDate;
            var MasterDocModUser = pitem.MasterDocModUser;
            var MasterDocModUpdated = pitem.MasterDocModUpdated;

            if (MasterDocPublished == true){
                
                var publish = confirm("Are you sure, you want to unpublish this document?");
                $.ajax({
                    async: false,
                    url: "api/EDITPDocument/CheckAvailableTPDoc/",
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    data: { MasterDocEDITControl:MasterDocEDITControl,MasterDocInbound:MasterDocInbound },
                    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                    success: function (data) {
                        try {
                            var blnSuccess = false;
                            var blnErrorShown = false;
                            var strValidationMsg = "";
                            if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                    blnErrorShown = true;
                                    ngl.showErrMsg("EDI MasterDoc Failure", data.Errors, null);
                                }
                                else {
                                    if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                        if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                            blnSuccess = true;
                                            submit = false;
                                            ngl.showWarningMsg("The Trading Partner document is already exist, It cannot be unpublished.", strValidationMsg);
                                        }
                                        else
                                        {
                                            var item = new NGLElement();
                                            item.MasterDocControl=MasterDocControl;
                                            item.MasterDocEDITControl=MasterDocEDITControl;
                                            item.MasterDocInbound=MasterDocInbound;
                                            item.MasterDocPublished = false;
                                            item.MasterDocDisabled=MasterDocDisabled;
                                            item.MasterDocCreateDate=MasterDocCreateDate;
                                            item.MasterDocCreateUser=MasterDocCreateUser;
                                            item.MasterDocModDate=MasterDocModDate;
                                            item.MasterDocModUser=MasterDocModUser;
                                            item.MasterDocModUpdated=MasterDocModUpdated;

                                            $.ajax({
                                                async: false,
                                                type: "POST",
                                                url: "api/EDIMasterDocument/PostSave",
                                                contentType: "application/json; charset=utf-8",
                                                dataType: 'json',
                                                data: JSON.stringify(item),
                                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                                success: function (data) {
                                                    try {
                                                        var blnSuccess = false;
                                                        var blnErrorShown = false;
                                                        var strValidationMsg = "";

                                                        if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                                            blnSuccess = true;
                                                            refreshMasterDocGrid();
                                           
                                                        }
                                                        else{
                                                            if (blnSuccess === false && blnErrorShown === false) {
                                                                if (strValidationMsg.length < 1) { strValidationMsg = "Update EDI MasterDoc Failure"; }
                                                                ngl.showErrMsg("Update EDI MasterDoc Failure", strValidationMsg, null);
                                                            }
                                                        }
                                                    } catch (err) {
                                                        ngl.showErrMsg(err.name, err.description, null);
                                                    }
                                                },
                                                error: function (xhr, textStatus, error) {
                                                    var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Update EDI MasterDoc Failure");
                                                    ngl.showErrMsg("Update EDI MasterDoc Failure", sMsg, null);
                                                }
                                            });
                                        }
                                    }
                                }
                            }
                            if (blnSuccess === false && blnErrorShown === false) {
                           
                            }
                        } catch (err) {
                            submit = false;
                            ngl.showErrMsg(err.name, err.description, null);
                        }
                    },
                    error: function (xhr, textStatus, error) {
                        var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save EDI MasterDoc Failure");
                        ngl.showErrMsg("EDI MasterDoc Failure", sMsg, null);
                    }
                });
            }
            else{
                var publish = confirm("Are you sure, you want to publish this document?");
                if (publish == true) {
                    var item = new NGLElement();
                    item.MasterDocControl=MasterDocControl;
                    item.MasterDocEDITControl=MasterDocEDITControl;
                    item.MasterDocInbound=MasterDocInbound;
                    item.MasterDocPublished = true;
                    item.MasterDocDisabled=MasterDocDisabled;
                    item.MasterDocCreateDate=MasterDocCreateDate;
                    item.MasterDocCreateUser=MasterDocCreateUser;
                    item.MasterDocModDate=MasterDocModDate;
                    item.MasterDocModUser=MasterDocModUser;
                    item.MasterDocModUpdated=MasterDocModUpdated;

                    $.ajax({
                        async: false,
                        type: "POST",
                        url: "api/EDIMasterDocument/PostSave",
                        contentType: "application/json; charset=utf-8",
                        dataType: 'json',
                        data: JSON.stringify(item),
                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                        success: function (data) {
                            try {
                                var blnSuccess = false;
                                var blnErrorShown = false;
                                var strValidationMsg = "";

                                if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                    blnSuccess = true;
                                    refreshMasterDocGrid();
                                           
                                }
                                else{
                                    if (blnSuccess === false && blnErrorShown === false) {
                                        if (strValidationMsg.length < 1) { strValidationMsg = "Update EDI MasterDoc Failure"; }
                                        ngl.showErrMsg("Update EDI MasterDoc Failure", strValidationMsg, null);
                                    }
                                }
                            } catch (err) {
                                ngl.showErrMsg(err.name, err.description, null);
                            }
                        },
                        error: function (xhr, textStatus, error) {
                            var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Update EDI MasterDoc Failure");
                            ngl.showErrMsg("Update EDI MasterDoc Failure", sMsg, null);
                        }
                    });
                }
            }
        }
    
            //**************Editor DropDowns**********//

        function editDocumentDropDown(container, options) {
            $('<input required data-text-field="EDITName" data-value-field="EDITControl" data-bind="value:' + options.field + '"/>')
                .appendTo(container)
                .kendoDropDownList({
                    dataTextField: "EDITName",
                    dataValueField: "EDITControl",
                    autoBind: true,
                    dataSource: dsDocTypeDropDown,
                    change: function (e) {
                        var fee = this.dataItem(e.item);
                        options.model.set("EDITControl", fee.ListTypeControl);
                        options.model.set("EDITName", fee.ListTypeName);
                    }
                });
        }

        function editTransactionDropDown(container, options) {
            $('<input required data-text-field="TrName" data-value-field="TrControl" data-bind="value:' + options.field + '"/>')
                .appendTo(container)
                .kendoDropDownList({
                    dataTextField: "TrName",
                    dataValueField: "TrControl",
                    autoBind: true,
                    dataSource: ddlTransaction,
                    change: function (e) {
                        var fee = this.dataItem(e.item);
                        options.model.set("TrControl", fee.ListTypeControl);
                        options.model.set("TrName", fee.ListTypeName);
                    }
                });
        }

        function editPublishDropDown(container, options) {
            $('<input required data-text-field="PbName" data-value-field="PbControl" data-bind="value:' + options.field + '"/>')
                .appendTo(container)
                .kendoDropDownList({
                    dataTextField: "PbName",
                    dataValueField: "PbControl",
                    autoBind: true,
                    dataSource: ddlPublish,
                    change: function (e) {
                        var fee = this.dataItem(e.item);
                        options.model.set("PbControl", fee.ListTypeControl);
                        options.model.set("PbName", fee.ListTypeName);
                    }
                });
        }
        
        
        $(document).ready(function () {

            var PageMenuTab = $('#tab').kendoTabStrip({
                animation: { 
                    open: { 
                        effects: 'fadeIn'
                    } 
                },
                dataTextField: 'text',
                dataContentField: 'content',
                dataSource: [
                    { 
                        text: 'Actions',
                        //Modified By SRP on 2/22/2018 ElementAddExample
                        content: "<button id='btnAddMasterDoc' class='k-button actionBarButton' type='button' onclick='execActionClick(btnAddMasterDoc, 160);'><span class='k-icon k-i-add'></span>Add Master Document</button>"
                    }
                ]
            }).data('kendoTabStrip').select(0);
            
            var PageReadyJS = $('#menuTree').kendoTreeView({
                dataUrlField: 'LinksTo',
                dataSource: {
                    data:menuitems.data
                    
                },
                loadOnDemand: false
            }).data('kendoTreeView'), handleTextBox = function (callback) { return function (e) { if (e.type != 'keypress' || kendo.keys.ENTER == e.keyCode) { callback(e); } }; };


            control = <%=UserControl%>;           

            //set default message
            var l = "<h3>Manage Master Documents</h3>";
            $("#txtScreenMessage").html(l);

           
            ////////////Filters///////////////////
            var EDIElementFilterData = [ 
               { text: "", value: "None" },
               { text: "Document Name", value: "EDITName" },
               { text: "Transaction Direction", value: "MasterDocInbound" },
            ];
            
            $("#ddlEDIMasterDocFilters").kendoDropDownList({
                dataTextField: "text",
                dataValueField: "value",
                placeholder:"select",
                dataSource: EDIElementFilterData,
                select: function(e) {
                    var name = e.dataItem.text; 
                    var val = e.dataItem.value; 
                    $("#txtMasterDocFilterVal").data("kendoMaskedTextBox").value("");
                    $("#dpMasterDocFilterFrom").data("kendoDatePicker").value("");
                    $("#dpMasterDocFilterTo").data("kendoDatePicker").value("");
                    switch(val){
                        case "None":
                            $("#spMasterDocFilterText").hide();
                            $("#spMasterDocFilterDates").hide();
                            $("#spMasterDocFilterButtons").hide(); 
                            break; 
                        case "NoDatesAvailable":
                            $("#spMasterDocFilterText").hide();
                            $("#spMasterDocFilterDates").show();
                            $("#spMasterDocFilterButtons").show();
                            break;
                        default:
                            $("#spMasterDocFilterText").show();
                            $("#spMasterDocFilterDates").hide();
                            $("#spMasterDocFilterButtons").show();
                            break;
                    }
                }
            });
            
            $("#txtMasterDocFilterVal").kendoMaskedTextBox(); 
            $("#dpMasterDocFilterFrom").kendoDatePicker(); 
            $("#dpMasterDocFilterTo").kendoDatePicker(); 
            $("#btnMasterDocFilter").kendoButton({
                icon: "filter",
                click: function(e) { 
                    var dataItem = $("#ddlEDIMasterDocFilters").data("kendoDropDownList").dataItem(); 
                    
                    if (1 === 0){ 
                        var dtFrom = $("#dpMasterDocFilterFrom").data("kendoDatePicker").value(); 
                        if (!dtFrom) { ngl.showErrMsg("Required Fields", "Filter From date cannot be null", null); return;}
                    } 
                    $("#MasterDocGrid").data("kendoGrid").dataSource.read();
                }
            }); 
            
            $("#btnMasterDocClearFilter").kendoButton({
                icon: "filter-clear",
                click: function(e) {
                    var dropdownlist = $("#ddlEDIMasterDocFilters").data("kendoDropDownList"); 
                    dropdownlist.select(0);
                    dropdownlist.trigger("change");
                    $("#txtMasterDocFilterVal").data("kendoMaskedTextBox").value("");
                    $("#dpMasterDocFilterFrom").data("kendoDatePicker").value(""); 
                    $("#dpMasterDocFilterTo").data("kendoDatePicker").value(""); 
                    $("#spMasterDocFilterText").hide(); 
                    $("#spMasterDocFilterDates").hide(); 
                    $("#spMasterDocFilterButtons").hide();
                    $("#MasterDocGrid").data("kendoGrid").dataSource.read();
                }
            }); 
            
            $("#txtMasterDocFilterVal").data("kendoMaskedTextBox").value("");
            $("#dpMasterDocFilterFrom").data("kendoDatePicker").value("");
            $("#dpMasterDocFilterTo").data("kendoDatePicker").value("");
            $("#spMasterDocFilterText").hide();
            $("#spMasterDocFilterDates").hide();
            $("#spMasterDocFilterButtons").hide();

           
          
            /////////////DropDown Data Source's/////////////
            
            dsDocTypeDropDown = new kendo.data.DataSource({
                transport: {
                    read: {
                        url: "api/EDIDocumentType/GetRecords",
                        contentType: 'application/json; charset=utf-8',
                        dataType: "json",
                        data: { filter: JSON.stringify({"filterName":"None","filterValue":"","page":1,"skip":0,"take":100}) }, 
                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }                      
                    },
                    parameterMap: function (options, operation) { return options; }
                },
                schema: {
                    data: "Data",
                    total: "Count",
                    model: { 
                        id: "EDITControl",
                        fields: {
                            EDITControl: { type: "number" },
                            EDITName: { type: "string" }, 
                            EDITDescription: { type: "string" }
                        }
                    }, 
                    errors: "Errors"
                },
                error: function (xhr, textStatus, error) { ngl.showErrMsg("Get Static List JSON Response Error",formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"), null);  this.cancelChanges(); },
                serverPaging: false,
                serverSorting: false,
                serverFiltering: false
            });
           
            //**********ADD DropDowns**********//
            $("#ddlMasterDocEDITControl").kendoDropDownList({
                dataSource: dsDocTypeDropDown,
                dataTextField: "EDITName",
                dataValueField: "EDITControl",
                optionLabel:"Select Document",
                autoWidth: true,
                filter: "contains",
            });

            $("#ddlMasterDocInbound").kendoDropDownList({
                dataSource: ddlTransaction,
                dataTextField: "TrName",
                dataValueField: "TrControl",
                autoWidth: true,
                filter: "contains",
            });

            $("#ddlMasterDocPublished").kendoDropDownList({
                dataSource: ddlPublish,
                dataTextField: "PbName",
                dataValueField: "PbControl",
                autoWidth: true,
                filter: "contains",
            });

            $("#lblMasterDocEDIName").kendoDropDownList({
                dataSource: dsDocTypeDropDown,
                dataTextField: "EDITName",
                dataValueField: "EDITControl",
                autoWidth: true,
                filter: "contains",
            });
            var Hideddl= $("#lblMasterDocEDIName").data("kendoDropDownList");
            Hideddl.wrapper.hide();



            ////////////Grid///////////////////
            EDIMasterDoc = new kendo.data.DataSource({
                serverSorting: true, 
                serverPaging: true, 
                pageSize: 10,
                transport: { 
                    read: function(options) { 
                        var s = new AllFilter();
                        s.filterName = $("#ddlEDIMasterDocFilters").data("kendoDropDownList").value();
                        s.filterValue = $("#txtMasterDocFilterVal").data("kendoMaskedTextBox").value();
                        s.page = options.data.page;
                        s.skip = options.data.skip;
                        s.take = options.data.take;
                        if(s.filterName !="None")
                        {
                            
                            $.ajax({ 
                                url: '/api/EDIMasterDocument/GetMasterEDIDocRecordsByMasterID/', 
                                contentType: 'application/json; charset=utf-8', 
                                dataType: 'json', 
                                data: { filter: JSON.stringify(s) }, 
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                                success: function(data) {
                                
                                    options.success(data);
                                    // Updated by SRP on 02/22/2018
                                    try {                                    
                                        var blnSuccess = false;
                                        var blnErrorShown = false;
                                        var strValidationMsg = "";
                                        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                            if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                                blnErrorShown = true;
                                                ngl.showErrMsg("Get EDI MasterDoc Failure", data.Errors, null);
                                            }
                                            else {
                                                if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                    if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                                        blnSuccess = true;
                                                        //do other stuff if needed
                                                    }
                                                    else{
                                                        blnSuccess = true;
                                                    }
                                                }
                                            }
                                        }
                                        if (blnSuccess === false && blnErrorShown === false) {
                                            if (strValidationMsg.length < 1) { strValidationMsg = "Get EDI MasterDoc Failure"; }
                                            ngl.showErrMsg("Get EDI MasterDoc Failure", strValidationMsg, null);
                                        }
                                    } catch (err) {
                                        ngl.showErrMsg(err.name, err.description, null);
                                    }
                               
                                }, 
                                error: function (xhr, textStatus, error) {
                                    var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Get EDI MasterDoc Failure");
                                    ngl.showErrMsg("Get EDI MasterDoc Failure", sMsg, null); 
                                
                                } 
                            });
                        }
                        else{
                           

                            $.ajax({ 
                                url: '/api/EDIMasterDocument/GetRecords/', 
                                contentType: 'application/json; charset=utf-8', 
                                dataType: 'json', 
                                data: { filter: JSON.stringify(s) }, 
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                                success: function(data) {
                                
                                    options.success(data);
                                    // Updated by SRP on 02/22/2018
                                    try {                                    
                                        var blnSuccess = false;
                                        var blnErrorShown = false;
                                        var strValidationMsg = "";
                                        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                            if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                                blnErrorShown = true;
                                                ngl.showErrMsg("Get EDI MasterDoc Failure", data.Errors, null);
                                            }
                                            else {
                                                if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                    if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                                        blnSuccess = true;
                                                        //do other stuff if needed
                                                    }
                                                    else{
                                                        blnSuccess = true;
                                                    }
                                                }
                                            }
                                        }
                                        if (blnSuccess === false && blnErrorShown === false) {
                                            if (strValidationMsg.length < 1) { strValidationMsg = "Get EDI MasterDoc Failure"; }
                                            ngl.showErrMsg("Get EDI MasterDoc Failure", strValidationMsg, null);
                                        }
                                    } catch (err) {
                                        ngl.showErrMsg(err.name, err.description, null);
                                    }
                               
                                }, 
                                error: function (xhr, textStatus, error) {
                                    var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Get EDI MasterDoc Failure");
                                    ngl.showErrMsg("Get EDI MasterDoc Failure", sMsg, null); 
                                
                                } 
                            });
                        }
                    },      
                   
                    destroy: function(options) {
                        
                            $.ajax({
                                url: 'api/EDIMasterDocument/DeleteMasterRecord', 
                                type: 'DELETE',
                                contentType: 'application/json; charset=utf-8', 
                                dataType: 'json', 
                                data: JSON.stringify(options.data),
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                                success: function (data) {     
                                    try {                                    
                                        var blnSuccess = false;
                                        var blnErrorShown = false;
                                        var strValidationMsg = "";
                                        if (data == 0) {
                                            //ngl.showWarningMsg("Master Document already exist with others, it cannot be deleted!", "", null);
                                        
                                        }
                                        if(typeof (data) !== 'undefined' && ngl.isObject(data)){
                                        
                                            if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                                blnErrorShown = true;
                                                //ngl.showWarningMsg("Master Document already exist with others, it cannot be deleted!", data.Errors, null);
                                            }
                                    
                                        }
                                
                                    } catch (err) {
                                        ngl.showErrMsg(err.name, err.description, null);
                                    }
                                    //refresh the grid
                                    if (ngl.isFunction(refreshMasterDocGrid)) {
                                        refreshMasterDocGrid();
                                    }
                                },
                                error: function (xhr, textStatus, error) {
                                    var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Delete EDI MasterDoc Failure");
                                    ngl.showErrMsg("Delete EDI MasterDoc Failure", sMsg, null); 
                                } 
                            });
                        //}
                    },
                    parameterMap: function(options, operation) { return options; } 
                },  
                schema: {
                    data: "Data",
                    total: "Count",
                    model: {
                        id: "MasterDocControl",
                        fields: {
                            MasterDocControl: { type: "number" },
                            MasterDocEDITControl: { type: "number" },
                            MasterDocInbound: { type: "boolean" },
                            MasterDocPublished: { type: "boolean" },
                            MasterDocDisabled: {type:"boolean"},
                            MasterDocModDate: {type:"date"},
                            MasterDocModUser: {type:"string"}
                        }
                    },
                    errors: "Errors"
                },
                error: function(xhr, textStatus, error) {
                    ngl.showErrMsg("Access EDI MasterDoc Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges();
                }
            });
            
            $('#MasterDocGrid').kendoGrid({
                dataSource: EDIMasterDoc,
                pageable: true,
                sortable: {
                    mode: "single",
                    allowUnsort: true
                },
                //edit: onGridEditing,
                height: 300,
                sort: function(e) {
                    if (!e.sort.dir) { e.sort.dir == ""; e.sort.field == ""; }
                    if (!e.sort.field) { e.sort.field == ""; }
                    $("#txtMasterDocSortDirection").val(e.sort.dir);
                    $("#txtMasterDocSortField").val(e.sort.field);
                },
                dataBound: function(e) { 

                    var tObj = this; 
                    tObj.tbody.find("tr[role='row']").each(function () {
                        var model = tObj.dataItem(this);
                        if (model.MasterDocPublished == true) {
                            $(this).find(".k-i-page-properties").addClass("k-state-disabled");
                            $(this).find(".k-grid-EditDoc").addClass("k-state-disabled");
                            $(this).find(".k-grid-delete").addClass("k-state-disabled");
                            $(this).find(".k-i-upload").removeClass("k-i-upload").addClass("k-i-download");
                            e.preventDefault();
                            $( ".k-state-disabled" ).each(function( index ) {
                                $(this).removeClass('k-grid-delete')
                                $(this).removeClass('k-grid-EditDoc')
                            });
                        }
                    });
                    if (typeof (MasterDocGridDataBoundCallBack) !== 'undefined' && ngl.isFunction(MasterDocGridDataBoundCallBack)) { MasterDocGridDataBoundCallBack(e,tObj); } 
                },
                resizable: true,
                groupable: true, 

                editable: "inline",
                columns: [
                    { field: "MasterDocControl", title: "MasterDocControl ", hidden: true },
                    { field: "MasterDocEDITControl", title: "Document Name", hidden: false, editor: editDocumentDropDown, template:data=>dsDocTypeDropDown._data[Object.values(dsDocTypeDropDown._data).findIndex(x=> x.EDITControl == data.MasterDocEDITControl)].EDITName},
                    { field: "MasterDocInbound", title: "Transaction Direction",hidden: false, editor: editTransactionDropDown, template:data=>ddlTransaction[Object.values(ddlTransaction).findIndex(x=> x.TrControl == data.MasterDocInbound)].TrName},
                    { field: "MasterDocPublished", title: "Published",  hidden: false, editor: editPublishDropDown, template:data=>ddlPublish[Object.values(ddlPublish).findIndex(x=> x.PbControl == data.MasterDocPublished)].PbName},
                    { field: "MasterDocDisabled", title: "Disabled", template: '<input type="checkbox" id="SelectedCB" #= MasterDocDisabled ? checked="checked" : "" # disabled="disabled" />' , hidden: false },
                    { field: "MasterDocModDate", title: "Mod Date", template: "#= kendo.toString(kendo.parseDate(MasterDocModDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') #",hidden: true },//Updated By SN on 02/21/2018
                    { field: "MasterDocModUser", title: "Mod User", hidden: true },//Updated By SN on 02/21/2018
                    { command: [{ name: "editMDoc", text: "", iconClass: "k-icon k-i-pencil", click: editMDoc},{name: "EditDoc", text:"", className: "k-grid-EditDoc", iconClass: "k-icon k-i-file-wrench", click: EditDocument},{name: "Publish", text:"", iconClass: "k-icon k-i-upload" , click: publishDocument },{name: "destroy", text: "", imageClass: "k-i-delete", iconClass: "k-icon" }], title: "Action", width: "150px" }
                   
                    
                ],
            });
            $("#MasterDocGrid").kendoTooltip({
                filter: ".k-grid-editMDoc",
                content: "Edit&nbsp;Master&nbsp;Document"
            });

            $("#MasterDocGrid").kendoTooltip({
                filter: ".k-grid-EditDoc",
                content: "Edit&nbsp;Document&nbsp;Structure"
            });

            $("#MasterDocGrid").kendoTooltip({
                filter: ".k-grid-Publish",
                content: "Publish&nbsp;Master&nbsp;Document"
            });

            $("#MasterDocGrid").kendoTooltip({
                filter: ".k-i-download",
                content: "Unpublish&nbsp;Master&nbsp;Document"
            });

            $("#MasterDocGrid").kendoTooltip({
                filter: ".k-grid-delete",
                content: "Delete&nbsp;Master&nbsp;Document"
            });
         
            //*********Window Add Master Document*********//
            kendoWin.height =300;// For Updating Kendowindow height config
            kendoWin.width = 280;// For Updating Kendowindow width config
            
            kendoWinStyle({"padding":"10px 10px 10px 30px"});//For Styling kendo Window
            wndAddMasterDoc = $("#wndAddMasterDoc").kendoWindow(kendoWin).data("kendoWindow");

            $("#wndAddMasterDoc").data("kendoWindow").wrapper.find(".k-svg-i-save").click(function (e) { SaveEDIMasterDoc(); });
        });
        </script>
        <style>
            .k-grid tbody .k-button {
                min-width: 18px;
                width: 28px;
            }
            .k-grid tbody tr td 
            {
                vertical-align: top
            }
        </style>
    </div>
</body>

</html>
