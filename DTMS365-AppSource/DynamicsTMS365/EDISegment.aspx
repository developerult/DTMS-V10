<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EDISegment.aspx.cs" Inherits="DynamicsTMS365.EDISegment" %>

<!DOCTYPE html>

<html>
<head>
    <title>Manage EDI Segments</title>         
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
        .breakWord20 {
        word-break: break-all !important;
        word-wrap: break-word !important;
        vertical-align: top;
        }

        .k-grid-header .k-header {
            overflow: visible !important;
            white-space: normal !important;
        }
        .ui-container {
            height: 100% !important; width: 100% !important; margin-top: 2px !important;
        }
        .ui-vertical-container {
            height: 98% !important; width: 98% !important; 
        }
        
        .ui-horizontal-container {
            height: 100% !important; width: 100% !important;
        }
        .ui-padding-container {
            padding: 10px;
        }
        .ui-id260-container {
            margin-top: 10px; 
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
    <!-- added by SRP on 3/5/2018 For Dynamically adding menu items for all pages -->
    <script src="Scripts/NGL/v-8.5.4.006/windowconfig.js"></script>
    <!--added by SRP on 3/5/2018 For Editing KendoWindow Configuration from Javascript -->

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
                            <div><span>EDI - MAINTENANCE</span></div>
                            <%--Page Navigation Menu Tree--%>
                            <div id="menuTree"></div>
                        </div>
                    </div>
                    <div id="center-pane">
                        <!-- Begin Page Content -->

                        <%--Message--%>
                        <div id="txtScreenMessage"></div>

                        <!-- Grid Fast Tab -->
                        <div id="id260" class="ui-id260-container">
                            <div class="fast-tab">
                                <span id="ExpandParSpan" style="display: none;"><a onclick="expandFastTab('ExpandParSpan','CollapseParSpan','ParHeader','ParDetail');"><span class="k-icon k-i-chevron-down ui-span-container"></span></a></span>
                                <span id="CollapseParSpan" style="display: normal;"><a onclick="collapseFastTab('ExpandParSpan','CollapseParSpan','ParHeader',null);"><span class="k-icon k-i-chevron-up ui-span-container"></span></a></span>
                                <span class="ui-span-container">EDI Segments</span>
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
                                                <label for="ddlEDISegmentFilters">Filter by:</label>
                                                <input id="ddlEDISegmentFilters" />
                                                <span id="spSegmentFilterText">
                                                    <input id="txtSegmentFilterVal" /></span>
                                                <span id="spSegmentFilterDates">
                                                    <label for="dpSegmentFilterFrom">From:</label>
                                                    <input id="dpSegmentFilterFrom" />
                                                    <label for="dpSegmentFilterTo">To:</label>
                                                    <input id="dpSegmentFilterTo" />
                                                </span>
                                                <span id="spSegmentFilterButtons"><a id="btnSegmentFilter"></a><a id="btnSegmentClearFilter"></a></span>
                                            </span>
                                            <input id="txtSegmentSortDirection" type="hidden" />
                                            <input id="txtSegmentSortField" type="hidden" />
                                        </div>
                                    </div>
                                    <%--Grid--%>
                                    <div id="SegmentGrid"></div>
                                </div>
                            </div>
                        </div>

                        <!-- End Page Content -->

                    </div>
                </div>
            </div>
            <div id="bottom-pane" class="k-block ui-horizontal-container-segment">
                <div class="pane-content">
                    <div><span>
                        <p>If you experience problems with this site, call (847)963-0007 24/7 or email our support group at <a href='mailto: support@nextgeneration.com'>support@nextgeneration.com</a></p>
                    </span></div>
                </div>
            </div>
        </div>

        <%--Popup Window HTML--%>
        <div id="wndMessage">
            <div>
                <h2>Enter a Message</h2>
                <input id="txtUserInput" style="width: 250px;" />
            </div>
        </div>

        <%--Added By SRP on 3/5/2018 EDIElement for KendoWindow--%>
        <% Response.WriteFile("~/Views/SegmentAddWindow.html"); %>

        <% Response.Write(AuthLoginNotificationHTML); %>
        <script>
        //************* Page Variables **************************
        var PageControl = '<%=PageControl%>';       
            var tObj = this;
            var tPage = this;
        var oKendoGrid = null;
        
        var wndMessage = kendo.ui.Window;
        var wndAddDocType = kendo.ui.Window; //Added By SRP on 3/5/2018 EDIElement


        //*************  Call Back Functions  *******************
        function SegmentGridDataBoundCallBack(e,tGrid){           
            oKendoGrid = tGrid;
            //add databound code here
        }


        //*************  Action Functions  **********************
        function execActionClick(btn, proc){
         
            if (btn.id == "btnAddSegment") { //Added By SRP on 3/5/2018 EDISegmentAddExample
                openSegmentAddWindow();
            }
        }

        //Added By By SRP on 3/5/2018 EDIElementAddSample
        function openSegmentAddWindow() {

            //Validation Display
            var valcheck = $("#name-validation").hasClass("hide-display");
            var valmincheck = $("#min-validation").hasClass("hide-display");
            var valmaxcheck = $("#max-validation").hasClass("hide-display");
            var valmaxemptycheck = $("#maxnew-validation").hasClass("hide-display");
            if (valcheck == false) {
                $("#name-validation").addClass("hide-display");
            }
            if (valmincheck == false) {
                $("#min-validation").addClass("hide-display");
            }
            if (valmaxcheck == false) {
                $("#max-validation").addClass("hide-display");
            }
            if (valmaxcheck == false) {
                $("#maxnew-validation").addClass("hide-display");
            }
            $("#maxnew-validation").addClass("hide-display");
            //This is how you can change the title of the window (in case you want to share the same window for Add and Edit- Note: In grids editing does not have to always be inline)
            $("#wndAddSegment").data("kendoWindow").title("Add EDI Segment");

            //Clear all previous values since this is Add New
            $("#txtSegmentControl").val(0);
            $("#txtSegmentName").data("kendoMaskedTextBox").value("");
            $("#txtSegmentDesc").data("kendoMaskedTextBox").value("");
            $("#txtSegmentMinCount").data("kendoMaskedTextBox").value(0);
            $("#txtSegmentMaxCount").data("kendoMaskedTextBox").value(0);                    
            $("#chksegmentupdate").val(0);
            $("#chksegmentupdate").prop('checked', false);
            wndAddDocType.center().open();
        }

        function openSegmentEditWindow(e) {
            //Get the record data from the grid
            var item = this.dataItem($(e.currentTarget).closest("tr")); 
            //Validation Display
            var valcheck = $("#name-validation").hasClass("hide-display");
            var valmincheck = $("#min-validation").hasClass("hide-display");
            var valmaxcheck = $("#max-validation").hasClass("hide-display");
            var valmaxemptycheck = $("#maxnew-validation").hasClass("hide-display");
            if (valcheck == false) {
                $("#name-validation").addClass("hide-display");
            }
            if (valmincheck == false) {
                $("#min-validation").addClass("hide-display");
            }
            if (valmaxcheck == false) {
                $("#max-validation").addClass("hide-display");
            }
            if (valmaxcheck == false) {
                $("#maxnew-validation").addClass("hide-display");
            }
            $("#maxnew-validation").addClass("hide-display");
            //**********title of Edit Window***********//

            $("#wndAddSegment").data("kendoWindow").title("Edit EDI Segment");
            $("#txtSegmentControl").val(item.SegmentControl);
            $("#txtSegmentName").data("kendoMaskedTextBox").value(item.SegmentName);
            $("#txtSegmentDesc").data("kendoMaskedTextBox").value(item.SegmentDesc);
            $("#txtSegmentMinCount").data("kendoMaskedTextBox").value(item.SegmentMinCount);
            $("#txtSegmentMaxCount").data("kendoMaskedTextBox").value(item.SegmentMaxCount);
            $("#txtSegmentcreatuser").val(item.SegmentCreateUser);
            $("#txtSegmentcreatedate").val(item.SegmentCreateDate);
            $("#txtSegmentupdate").val(item.SegmentUpdated);
            $("#chksegmentupdate").prop('checked', item.SegmentDisabled);
            wndAddDocType.center().open();
        }

        //Added By By SRP on 3/5/2018 EDIElementAddSample
        function SaveEDISegment() {
            var otmp = $("#focusCancel").focus();

            //Created by SRP on 02/19/2018 for Input Validation
            var submit = true;
            var tName = $("#txtSegmentName").val();
            var sMinC = $("#txtSegmentMinCount").val();
            var sMaxC = $("#txtSegmentMaxCount").val();

            if (tName == "") {

                $("#name-validation").removeClass("hide-display");
                submit = false;
            }
            
            if (sMinC <= 0) {
                
                $("#min-validation").removeClass("hide-display");
                $("#txtSegmentMinCount").data("kendoMaskedTextBox").value('');
                submit = false;
              
            }
            if (sMaxC <= 0) {
                
                $("#max-validation").removeClass("hide-display");
                $("#txtSegmentMaxCount").data("kendoMaskedTextBox").value('');
                submit = false;
            }
            if (sMinC >sMaxC) {
                
                $("#maxnew-validation").removeClass("hide-display");
                submit = false;
            }
            var item = new NGLElement();

            item.SegmentControl = $("#txtSegmentControl").val();
            //Saving Records
            if(item.SegmentControl == 0){

                item.SegmentName = $("#txtSegmentName").data("kendoMaskedTextBox").value();
                item.SegmentDesc = $("#txtSegmentDesc").data("kendoMaskedTextBox").value();
                item.SegmentMinCount = $("#txtSegmentMinCount").data("kendoMaskedTextBox").value();
                item.SegmentMaxCount = $("#txtSegmentMaxCount").data("kendoMaskedTextBox").value();
                item.SegmentDisabled= $("#chksegmentupdate").is(":checked");
                if (submit == true) {


                    $.ajax({
                        async: false,
                        type: "POST",
                        url: "api/EDISegment/SaveEDISegment",
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
                                        ngl.showErrMsg("Save EDI Segment Failure", data.Errors, null);
                                    }
                                    else {
                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                                blnSuccess = true;
                                                refreshSegmentGrid();
                                            }
                                        }
                                    }
                                }
                                if (blnSuccess === false && blnErrorShown === false) {
                                    if (strValidationMsg.length < 1) { strValidationMsg = "Save EDI Segment Failure"; }
                                    ngl.showErrMsg("Save EDI Segment Failure", strValidationMsg, null);
                                }
                            } catch (err) {
                                ngl.showErrMsg(err.name, err.description, null);
                            }
                        },
                        error: function (xhr, textStatus, error) {
                            var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure");
                            ngl.showErrMsg("Save EDI Segment Failure", sMsg, null);
                        }
                    });
                    wndAddDocType.close();
                }
            }
            else{
                //Updating Records
                item.SegmentName = $("#txtSegmentName").data("kendoMaskedTextBox").value();
                item.SegmentDesc = $("#txtSegmentDesc").data("kendoMaskedTextBox").value();
                item.SegmentMinCount = $("#txtSegmentMinCount").data("kendoMaskedTextBox").value();
                item.SegmentMaxCount = $("#txtSegmentMaxCount").data("kendoMaskedTextBox").value();
                item.SegmentCreateUser=$("#txtSegmentcreatuser").val();
                item.SegmentUpdated=$("#txtSegmentupdate").val();
                item.SegmentCreateDate= $("#txtSegmentcreatedate").val();
                item.SegmentDisabled= $("#chksegmentupdate").is(":checked");
                if (submit == true) {


                    $.ajax({
                        async: false,
                        type: "POST",
                        url: "api/EDISegment/PostSave",
                        contentType: "application/json; charset=utf-8",
                        dataType: 'json',
                        data: JSON.stringify(item),
                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                        success: function (data) {
                            try {
                                var blnSuccess = false;
                                var blnErrorShown = false;
                                var strValidationMsg = "";
                                 if (data.Data.length > 0 && typeof (data.Data[0]) != 'undefined') {
                                                blnSuccess = true;
                                                refreshSegmentGrid();
                                           
                                }
                                if (blnSuccess === false && blnErrorShown === false) {
                                    if (strValidationMsg.length < 1) { strValidationMsg = "Update EDI Segment Failure"; }
                                    ngl.showErrMsg("Save EDI Segment Element Failure", strValidationMsg, null);
                                }
                            } catch (err) {
                                ngl.showErrMsg(err.name, err.description, null);
                            }
                        },
                        error: function (xhr, textStatus, error) {
                            var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Update EDI Segment Failure");
                            ngl.showErrMsg("Update EDI Segment Failure", sMsg, null);
                        }
                    });
                    wndAddDocType.close();
                }
            }
        }
        $("#txtSegmentName").on("change input", function () {
            var name = $(this).val();
            if (name != "") {
                $("#name-validation").addClass("hide-display");
            }
            else {
                $("#name-validation").removeClass("hide-display");
            }

        });

        function Speak() {
            alert("This is a sample action where the popup would say 'Woof!' or 'Meow!' etc. based on which record in the grid was selected by the user.");
        }

        function ExampleAction3_Click() {
            alert("Example Action 3 does some stuff...");
        }

        function SaveMessage() {
            //get the data from the window
            var userMsg = $("#txtUserInput").data("kendoMaskedTextBox").value();

            //"save" the data 
            var l = "<h3>Message: " + userMsg + "</h3>";
            $("#txtScreenMessage").html(l);

            //close the window
            wndMessage.close();
        }


        function refreshSegmentGrid() {
            //oKendoGrid gets set during SegmentGridDataBoundCallBack()
            if (typeof (oKendoGrid) !== 'undefined' && ngl.isObject(oKendoGrid)) {
                oKendoGrid.dataSource.read();
            }
           
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
                        //Modified By SRP on 3/5/2018 SegmentAddExample
                        content: "<button id='btnAddSegment' class='k-button actionBarButton' type='button' onclick='execActionClick(btnAddSegment, 160);'><span class='k-icon k-i-add'></span>Add EDI Segment</button>"
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
            var l = "<h3>Manage EDI Segments </h3>";
            $("#txtScreenMessage").html(l);

            //define Kendo widgets
            $("#txtUserInput").kendoMaskedTextBox();
            $("#txtSegmentName").kendoMaskedTextBox();    //Added By SRP on 3/5/2018 SegmentAddExample
            $("#txtSegmentDesc").kendoMaskedTextBox();    //Added By SRP on 3/5/2018 SegmentAddExample
            $("#txtSegmentMinCount").kendoMaskedTextBox();    //Added By SRP on 3/5/2018 SegmentAddExample
            $("#txtSegmentMaxCount").kendoMaskedTextBox();    //Added By SRP on 3/5/2018 SegmentAddExample

            ////////////Filters///////////////////
            var EDISegmentFilterData = [ 
               { text: "", value: "None" },
               { text: "Segment Name", value: "SegmentName" },
               { text: "Segment Description", value: "SegmentDesc" },
            ];
            
            $("#ddlEDISegmentFilters").kendoDropDownList({
                dataTextField: "text",
                dataValueField: "value",
                placeholder:"select",
                dataSource: EDISegmentFilterData,
                select: function(e) {
                    var name = e.dataItem.text; 
                    var val = e.dataItem.value; 
                    $("#txtSegmentFilterVal").data("kendoMaskedTextBox").value("");
                    $("#dpSegmentFilterFrom").data("kendoDatePicker").value("");
                    $("#dpSegmentFilterTo").data("kendoDatePicker").value("");
                    switch(val){
                        case "None":
                            $("#spSegmentFilterText").hide();
                            $("#spSegmentFilterDates").hide();
                            $("#spSegmentFilterButtons").hide(); 
                            break; 
                        case "NoDatesAvailable":
                            $("#spSegmentFilterText").hide();
                            $("#spSegmentFilterDates").show();
                            $("#spSegmentFilterButtons").show();
                            break;
                        default:
                            $("#spSegmentFilterText").show();
                            $("#spSegmentFilterDates").hide();
                            $("#spSegmentFilterButtons").show();
                            break;
                    }
                }
            });
            
            $("#txtSegmentFilterVal").kendoMaskedTextBox(); 
            $("#dpSegmentFilterFrom").kendoDatePicker(); 
            $("#dpSegmentFilterTo").kendoDatePicker(); 
            $("#btnSegmentFilter").kendoButton({
                icon: "filter",
                click: function(e) { 
                    var dataItem = $("#ddlEDISegmentFilters").data("kendoDropDownList").dataItem(); 
                    
                    if (1 === 0){ 
                        var dtFrom = $("#dpSegmentFilterFrom").data("kendoDatePicker").value(); 
                        if (!dtFrom) { ngl.showErrMsg("Required Fields", "Filter From date cannot be null", null); return;}
                    } 
                    $("#SegmentGrid").data("kendoGrid").dataSource.read();
                }
            }); 
            
            $("#btnSegmentClearFilter").kendoButton({
                icon: "filter-clear",
                click: function(e) {
                    var dropdownlist = $("#ddlEDISegmentFilters").data("kendoDropDownList"); 
                    dropdownlist.select(0);
                    dropdownlist.trigger("change");
                    $("#txtSegmentFilterVal").data("kendoMaskedTextBox").value("");
                    $("#dpSegmentFilterFrom").data("kendoDatePicker").value(""); 
                    $("#dpSegmentFilterTo").data("kendoDatePicker").value(""); 
                    $("#spSegmentFilterText").hide(); 
                    $("#spSegmentFilterDates").hide(); 
                    $("#spSegmentFilterButtons").hide();
                    $("#SegmentGrid").data("kendoGrid").dataSource.read();
                }
            }); 
            
            $("#txtSegmentFilterVal").data("kendoMaskedTextBox").value("");
            $("#dpSegmentFilterFrom").data("kendoDatePicker").value("");
            $("#dpSegmentFilterTo").data("kendoDatePicker").value("");
            $("#spSegmentFilterText").hide();
            $("#spSegmentFilterDates").hide();
            $("#spSegmentFilterButtons").hide();        
           
            ////////////Grid///////////////////
            EDISEGMENT = new kendo.data.DataSource({
                serverSorting: true, 
                serverPaging: true, 
                pageSize: 10,
                transport: { 
                    read: function(options) { 
                        var s = new AllFilter();

                        s.filterName = $("#ddlEDISegmentFilters").data("kendoDropDownList").value();
                        s.filterValue = $("#txtSegmentFilterVal").data("kendoMaskedTextBox").value();

                        s.page = options.data.page;
                        s.skip = options.data.skip;
                        s.take = options.data.take;

                        $.ajax({ 
                            url: '/api/EDISegment/GetRecords/', 
                            contentType: 'application/json; charset=utf-8', 
                            dataType: 'json', 
                            data: { filter: JSON.stringify(s) }, 
                            headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                            success: function(data) {
                                
                                options.success(data);
                                // Updated by SRP on 03/5/2018
                                try {                                    
                                    var blnSuccess = false;
                                    var blnErrorShown = false;
                                    var strValidationMsg = "";
                                    if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                        if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                            blnErrorShown = true;
                                            ngl.showErrMsg("Get EDISegment Failure", data.Errors, null);
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
                                        if (strValidationMsg.length < 1) { strValidationMsg = "Get EDISegment Failure"; }
                                        ngl.showErrMsg("Get EDISegment Failure", strValidationMsg, null);
                                    }
                                } catch (err) {
                                    ngl.showErrMsg(err.name, err.description, null);
                                }
                               
                            }, 
                            error: function (xhr, textStatus, error) {
                                var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Get Data Failure");
                                ngl.showErrMsg("Get EDISegment Failure", sMsg, null); 
                                
                            } 
                        }); 
                    },      
                   
                    update: function(options) {
                        
                        $.ajax({ 
                            url: 'api/EDISegment/PostSave', 
                            type: "POST",
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
                                        if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                            blnErrorShown = true;
                                            ngl.showErrMsg("Save EDISegment Failure", data.Errors, null);
                                        }
                                        else {
                                            if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                                    blnSuccess = true;
                                                    //do other stuff if needed
                                                    refreshSegmentGrid();
                                                }
                                            }
                                        }
                                    }
                                    if (blnSuccess === false && blnErrorShown === false) {
                                        if (strValidationMsg.length < 1) { strValidationMsg = "Save EDIElement Failure"; }
                                        ngl.showErrMsg("Save EDISegment Failure", strValidationMsg, null);
                                    }
                                } catch (err) {
                                    ngl.showErrMsg(err.name, err.description, null);
                                }
                                //refresh the grid
                                if (ngl.isFunction(refreshSegmentGrid)) {
                                    refreshSegmentGrid();
                                }
                            },
                            error: function (xhr, textStatus, error) {
                                var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure");
                                ngl.showErrMsg("Save EDISegment Failure", sMsg, null); 
                            } 
                        });
                    },
                    destroy: function(options) {
                        $.ajax({
                            url: 'api/EDISegment/DeleteRecord', 
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
                                    if (data == false) {
                                        ngl.showWarningMsg("Segment is already used in document, it cannot be deleted!", "", null);
                                        
                                    }
                                    if(typeof (data) !== 'undefined' && ngl.isObject(data)){
                                        
                                        if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                            blnErrorShown = true;
                                            refreshSegmentGrid();
                                            ngl.showWarningMsg("Segment is already used in document, it cannot be deleted!", data.Errors, null);
                                        }
                                    
                                    }
                                
                                } catch (err) {
                                    ngl.showErrMsg(err.name, err.description, null);
                                }
                                //refresh the grid
                                if (ngl.isFunction(refreshSegmentGrid)) {
                                    refreshSegmentGrid();
                                }
                            },
                            error: function (xhr, textStatus, error) {
                                var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "delete SegmentTpe Failure");
                                ngl.showErrMsg("delete SegmentTpe Failure", sMsg, null); 
                            } 
                        });
                    },
                    parameterMap: function(options, operation) { return options; } 
                },  
                schema: {
                    data: "Data",
                    total: "Count",
                    model: {
                        id: "SegmentControl",
                        fields: {
                            SegmentControl: { type: "number" },
                            SegmentName: { type: "string" },
                            
                            SegmentDesc: { type: "string" },
                            
                            SegmentMinCount: { type: "number" },
                            SegmentMaxCount: { type: "number" },
                            SegmentDisabled: {type:"boolean"},
                            SegmentModDate: {type:"date",editable: false },//Updated By SRP on 03/5/2018
                            SegmentModUser: {type:"string"}//Updated By SRP on 03/5/2018
                        }
                    },
                    errors: "Errors"
                },
                error: function(xhr, textStatus, error) {
                    ngl.showErrMsg("Access EDIElement Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges();
                }
            });
            
            $('#SegmentGrid').kendoGrid({
                dataSource: EDISEGMENT,
                pageable: true,
                sortable: {
                    mode: "single",
                    allowUnsort: true
                },
                height: 300,
                sort: function(e) {
                    if (!e.sort.dir) { e.sort.dir == ""; e.sort.field == ""; }
                    if (!e.sort.field) { e.sort.field == ""; }
                    $("#txtSegmentSortDirection").val(e.sort.dir);
                    $("#txtSegmentSortField").val(e.sort.field);
                },
                dataBound: function(e) { 
                    var tObj = this; 
                    if (typeof (SegmentGridDataBoundCallBack) !== 'undefined' && ngl.isFunction(SegmentGridDataBoundCallBack)) { SegmentGridDataBoundCallBack(e,tObj); } 
                },
                resizable: true,
                groupable: true, 
                editable: "inline",
                columns: [
                    { field: "SegmentControl", title: "SegmentControl", hidden: true },
                    { field: "SegmentName", title: "Segment Name", width: 200, hidden: false },                    
                    { field: "SegmentDesc", title: "Segment Description", width: 300, hidden: false,template:"#if(SegmentDesc.length>50){# # var myContent =SegmentDesc; #  # var dcontent = myContent.substring(0,50)+'...'; # <span>#=kendo.toString(dcontent)#</span> #}else{# <span>#=SegmentDesc#</span> #}#" ,class: "breakWord20", width: 200, editor: textareaEditor},                                   
                    { field: "SegmentMinCount", title: "Segment MinCount", width: 100, hidden: true },
                    { field: "SegmentMaxCount", title: "Element MaxLength", width: 100, hidden: true },
                    { field: "SegmentDisabled", title: "Disabled", width: 100, template: '<input type="checkbox" id="SelectedCB" #= SegmentDisabled ? checked="checked" : "" # disabled="disabled" />' , hidden: false },
                    { field: "SegmentModDate", title: "Mod Date", template: "#= kendo.toString(kendo.parseDate(SegmentModDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') #",hidden: true },//Updated By SN on 02/21/2018
                    { field: "SegmentModUser", title: "Mod User", hidden: true },//Updated By SRP on 3/5/2018
                    { command: [{ name: "editSegment", text: "", iconClass: "k-icon k-i-pencil", click: openSegmentEditWindow},{name: "destroy", text: "", imageClass: "k-i-delete", iconClass: "k-icon" }], title: " ", width: "100px" }
                ]
            });

            $("#SegmentGrid").kendoTooltip({
                filter: ".k-grid-editSegment",
                content: "Edit&nbsp;Segment&nbsp;Details"
            });

            $("#SegmentGrid").kendoTooltip({
                filter: ".k-grid-delete",
                content: "Delete&nbsp;Segment"
            });
            
            function onGridEditing(arg) {
                arg.container.find("input[name='SegmentName']").attr('maxlength', '50');
                arg.container.find("input[name='SegmentDesc']").attr('maxlength', '255');
            }
            
            ////////////wndMessage///////////////////
            /////////////Edit Mode textarea for description///////////
            function textareaEditor(container, options) {
                $('<textarea data-bind="value: ' + options.field + '" cols="20" rows="4"></textarea>')
                    .appendTo(container);
            }
           
            wndMessage = $("#wndMessage").kendoWindow({
                title: "Message Window",
                height: 90,
                width: 100,
                modal: true,
                visible: false,
                actions: ["save", "Minimize", "Maximize", "Close"],
            }).data("kendoWindow");

            $("#wndMessage").data("kendoWindow").wrapper.find(".k-svg-i-save").click(function (e) { SaveMessage(); }); 

            kendoWin.height =450;// For Updating Kendowindow height config
            kendoWin.width = 350;// For Updating Kendowindow width config
            
            kendoWinStyle({"padding":"10px 10px 10px 30px"});//For Styling kendo Window
            wndAddDocType = $("#wndAddSegment").kendoWindow(kendoWin).data("kendoWindow");

            $("#wndAddSegment").data("kendoWindow").wrapper.find(".k-svg-i-save").click(function (e) { SaveEDISegment(); });
            

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
