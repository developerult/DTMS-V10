<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EDILoops.aspx.cs" Inherits="DynamicsTMS365.EDILoops" %>

<!DOCTYPE html>

<html>
<head>
    <title>Manage EDI Loops</title>         
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

        #LoopDesc {
            text-overflow: ellipsis !important;
        }
        .ui-container {
            height: 100%; width: 100%; margin-top: 2px;
        }
        .ui-vertical-container {
            height: 98%; width: 98%; 
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
    <!-- added by SRP on 3/8/2018 For Dynamically adding menu items for all pages -->
    <script src="Scripts/NGL/v-8.5.4.006/windowconfig.js"></script>
    <!--added by SRP on 3/8/2018 For Editing KendoWindow Configuration from Javascript -->

    <div id="example" class="ui-container">
        <div id="vertical" class="ui-vertical-container" >
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
                        <div id="id260" style="margin-top: 10px;">
                            <div class="fast-tab">
                                <span id="ExpandParSpan" style="display: none;"><a onclick="expandFastTab('ExpandParSpan','CollapseParSpan','ParHeader','ParDetail');"><span class="k-icon k-i-chevron-down ui-span-container"></span></a></span>
                                <span id="CollapseParSpan" style="display: normal;"><a onclick="collapseFastTab('ExpandParSpan','CollapseParSpan','ParHeader',null);"><span class="k-icon k-i-chevron-up ui-span-container"></span></a></span>
                                <span class="ui-span-container">EDI Loops</span>
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
                                                <label for="ddlEDILoopFilters">Filter by:</label>
                                                <input id="ddlEDILoopFilters" />
                                                <span id="spEDILoopFilterText">
                                                    <input id="txtEDILoopFilterVal" /></span>
                                                <span id="spEDILoopFilterDates">
                                                    <label for="dpEDILoopFilterFrom">From:</label>
                                                    <input id="dpEDILoopFilterFrom" />
                                                    <label for="dpEDILoopFilterTo">To:</label>
                                                    <input id="dpEDILoopFilterTo" />
                                                </span>
                                                <span id="spEDILoopFilterButtons"><a id="btnEDILoopFilter"></a><a id="btnEDILoopClearFilter"></a></span>
                                            </span>
                                            <input id="txtEDILoopSortDirection" type="hidden" />
                                            <input id="txtEDILoopSortField" type="hidden" />
                                        </div>
                                    </div>
                                    <%--Grid--%>
                                    <div id="EDILoopGrid"></div>
                                </div>
                            </div>
                        </div>

                        <!-- End Page Content -->
                    </div>
                </div>
            </div>
            <div id="bottom-pane" class="k-block ui-horizontal-container">
                <div class="pane-content">
                    <div>
                        <span>
                            <p>If you experience problems with this site, call (847)963-0007 24/7 or email our support group at <a href='mailto: support@nextgeneration.com'>support@nextgeneration.com</a></p>
                        </span>
                    </div>
                </div>
            </div>
        </div>

        <%--  <%--Popup Window HTML--%>
        <div id="wndMessage">
            <div>
                <h2>Enter a Message</h2>
                <input id="txtUserInput" style="width: 250px;" />
            </div>
        </div>

        <%--Added By SRP on 3/8/18 EDILoop for KendoWindow--%>
        <% Response.WriteFile("~/Views/EDILoopAddWindow.html"); %>

        <% Response.Write(AuthLoginNotificationHTML); %>
        <script>
        //************* Page Variables **************************
        var PageControl = '<%=PageControl%>';       
            var tObj = this;
            var tPage = this;
        var oKendoGrid = null;

        var dsListTypeDropDown = kendo.data.DataSource;
        var dsLGTDropDown = kendo.data.DataSource;
        var dsLEADropDown = kendo.data.DataSource;
        var wndMessage = kendo.ui.Window;
        var wndAddDocType = kendo.ui.Window; //Added By SRP on 3/8/18 EDILoop


        //*************  Call Back Functions  *******************
        function EDILoopGridDataBoundCallBack(e,tGrid){           
            oKendoGrid = tGrid;

            //add databound code here
        }


        //*************  Action Functions  **********************
        function execActionClick(btn, proc){
         
            if(btn.id == "btnAddLoop"){ //Added By SRP on 3/8/18 EDILoopAdd
                openLoopAddWindow();
            }
        }

        function openLoopAddWindow() {

            //Validation Display
            var namecheck = $("#name-validation").hasClass("hide-display");

            if (namecheck == false) {
                $("#name-validation").addClass("hide-display");
            }
            var mincheck = $("#min-validation").hasClass("hide-display");

            if (mincheck == false) {
                $("#min-validation").addClass("hide-display");
            }
            var maxcheck = $("#max-validation").hasClass("hide-display");

            
            if (mincheck == false) {
                $("#min-validation").addClass("hide-display");
            }
            //This is how you can change the title of the window (in case you want to share the same window for Add and Edit- Note: In grids editing does not have to always be inline)
            $("#wndAddEDILoop").data("kendoWindow").title("Add EDI Loop");

            //Clear all previous values since this is Add New
            $("#txtLoopControl").val(0);
            $("#txtLoopName").data("kendoMaskedTextBox").value("");
            $("#txtLoopDescription").data("kendoMaskedTextBox").value("");
            $("#txtLoopMinCount").kendoNumericTextBox({
                min: 0
            });
            $("#txtLoopMaxCount").kendoNumericTextBox({
                min: 0
            });
            wndAddDocType.center().open();
        }

       
       
        //Added By SRP on 3/8/18 EDILoopAdd
        function SaveEDILoop() {
            var otmp = $("#focusCancel").focus();

            var submit = true;
            var LName = $("#txtLoopName").val();
            var LMinC = $("#txtLoopMinCount").val();
            var LmaxC = $("#txtLoopMaxCount").val();
            
            if (LName == "") {
                
                $("#name-validation").removeClass("hide-display");
                submit = false;
            }
           if (LMinC <= 0) {
                
               $("#min-validation").removeClass("hide-display");
               $("#txtLoopMinCount").data("kendoMaskedTextBox").value('');
                submit = false;
              
            }
            if (LmaxC <= 0) {
                
                $("#max-validation").removeClass("hide-display");
                $("#txtLoopMaxCount").data("kendoMaskedTextBox").value('');
                submit = false;
            }
            if (LMinC >LmaxC) {
                
                $("#maxnew-validation").removeClass("hide-display");
                $("#txtLoopMinCount").data("kendoMaskedTextBox").value('');
                $("#txtLoopMaxCount").data("kendoMaskedTextBox").value('');
                submit = false;
            }
            
           
            var item = new NGLDocType();

            item.LoopControl = $("#txtLoopControl").val();

            item.LoopName = $("#txtLoopName").data("kendoMaskedTextBox").value();
            
            item.LoopDesc = $("#txtLoopDescription").data("kendoMaskedTextBox").value();

            item.LoopMinCount = $("#txtLoopMinCount").data("kendoMaskedTextBox").value();

            item.LoopMaxCount = $("#txtLoopMaxCount").data("kendoMaskedTextBox").value();           
       
            if (submit == true) {           
            $.ajax({
                async: false,
                type: "POST",
                url: "api/EDILoops/SaveEDILoop",
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
                                ngl.showErrMsg("Save EDILoop Failure", data.Errors, null);
                            }
                            else {
                                if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                    if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                        blnSuccess = true;
                                        refreshEDILoopGrid();
                                    }
                                }
                            }
                        }
                        if (blnSuccess === false && blnErrorShown === false) {
                            if (strValidationMsg.length < 1) { strValidationMsg = "Save EDILoop Failure"; }
                            ngl.showErrMsg("Save EDILoop Failure", strValidationMsg, null);
                        }
                    } catch (err) {
                        ngl.showErrMsg(err.name, err.description, null);
                    }
                },
                error: function (xhr, textStatus, error) {
                    var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure");
                    ngl.showErrMsg("Save EDILoop Failure", sMsg, null);                        
                }
                });
            wndAddDocType.close();         
            }             
        }
        $("#txtLoopName").on("change input", function () {
            var name = $(this).val();
            if (name != "") {
                $("#name-validation").addClass("hide-display");
            }
            else {
                $("#name-validation").removeClass("hide-display");
            }

        });

        function Speak(){
            alert("This is a sample action where the popup would say 'Woof!' or 'Meow!' etc. based on which record in the grid was selected by the user."); 
        }

        function ExampleAction3_Click(){
            alert("Example Action 3 does some stuff...");
        }

        function SaveMessage(){
            //get the data from the window
            var userMsg = $("#txtUserInput").data("kendoMaskedTextBox").value();

            //"save" the data 
            var l = "<h3>Message: " + userMsg + "</h3>";
            $("#txtScreenMessage").html(l);

            //close the window
            wndMessage.close(); 
        }


        function refreshEDILoopGrid() {
            //oKendoGrid gets set during EDILoopGridDataBoundCallBack()
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
                        //Modified By SRP on 3/8/18 EDILoopAddExample
                        content: "<button id='btnAddLoop' class='k-button actionBarButton' type='button'><span class='k-icon k-i-add'></span>Add EDI Loop</button>"
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
            var l = "<h3>Manage EDI Loops</h3>";
            $("#txtScreenMessage").html(l);

            //define Kendo widgets
            $("#txtLoopName").kendoMaskedTextBox();    //Added By SRP on 3/8/18 EDILoopAddExample
            $("#txtLoopDescription").kendoMaskedTextBox();    //Added By SRP on 3/8/18 EDILoopAddExample
            $("#txtLoopMinCount").kendoMaskedTextBox();  
            $("#txtLoopMaxCount").kendoMaskedTextBox(); 
            
            ////////////Filters///////////////////
            var EDILoopFilterData = [ 
               { text: "", value: "None" },
               { text: "Loop Name", value: "LoopName" },
               { text: "Loop Description", value: "LoopDesc" },
            ];
            
            $("#ddlEDILoopFilters").kendoDropDownList({
                dataTextField: "text",
                dataValueField: "value",
                dataSource: EDILoopFilterData,
                select: function(e) {
                    var name = e.dataItem.text; 
                    var val = e.dataItem.value; 
                    $("#txtEDILoopFilterVal").data("kendoMaskedTextBox").value("");
                    $("#dpEDILoopFilterFrom").data("kendoDatePicker").value("");
                    $("#dpEDILoopFilterTo").data("kendoDatePicker").value("");
                    switch(val){
                        case "None":
                            $("#spEDILoopFilterText").hide();
                            $("#spEDILoopFilterDates").hide();
                            $("#spEDILoopFilterButtons").hide(); 
                            break; 
                        case "NoDatesAvailable":
                            $("#spEDILoopFilterText").hide();
                            $("#spEDILoopFilterDates").show();
                            $("#spEDILoopFilterButtons").show();
                            break;
                        default:
                            $("#spEDILoopFilterText").show();
                            $("#spEDILoopFilterDates").hide();
                            $("#spEDILoopFilterButtons").show();
                            break;
                    }
                }
            });
            
            $("#txtEDILoopFilterVal").kendoMaskedTextBox(); 
            $("#dpEDILoopFilterFrom").kendoDatePicker(); 
            $("#dpEDILoopFilterTo").kendoDatePicker(); 
            $("#btnEDILoopFilter").kendoButton({
                icon: "filter",
                click: function(e) { 
                    var dataItem = $("#ddlEDILoopFilters").data("kendoDropDownList").dataItem(); 
                    
                    if (1 === 0){ 
                        var dtFrom = $("#dpEDILoopFilterFrom").data("kendoDatePicker").value(); 
                        if (!dtFrom) { ngl.showErrMsg("Required Fields", "Filter From date cannot be null", null); return;}
                    } 
                    $("#EDILoopGrid").data("kendoGrid").dataSource.read();
                }
            }); 
            
            $("#btnEDILoopClearFilter").kendoButton({
                icon: "filter-clear",
                click: function(e) {
                    var dropdownlist = $("#ddlEDILoopFilters").data("kendoDropDownList"); 
                    dropdownlist.select(0);
                    dropdownlist.trigger("change");
                    $("#txtEDILoopFilterVal").data("kendoMaskedTextBox").value("");
                    $("#dpEDILoopFilterFrom").data("kendoDatePicker").value(""); 
                    $("#dpEDILoopFilterTo").data("kendoDatePicker").value(""); 
                    $("#spEDILoopFilterText").hide(); 
                    $("#spEDILoopFilterDates").hide(); 
                    $("#spEDILoopFilterButtons").hide();
                    $("#EDILoopGrid").data("kendoGrid").dataSource.read();
                }
            }); 
            
            $("#txtEDILoopFilterVal").data("kendoMaskedTextBox").value("");
            $("#dpEDILoopFilterFrom").data("kendoDatePicker").value("");
            $("#dpEDILoopFilterTo").data("kendoDatePicker").value("");
            $("#spEDILoopFilterText").hide();
            $("#spEDILoopFilterDates").hide();
            $("#spEDILoopFilterButtons").hide();

             
            ////////////Grid///////////////////

            EDILoop = new kendo.data.DataSource({
                serverSorting: true, 
                serverPaging: true, 
                pageSize: 10,
                transport: { 
                    read: function(options) { 
                        var s = new AllFilter();

                        s.filterName = $("#ddlEDILoopFilters").data("kendoDropDownList").value();
                        s.filterValue = $("#txtEDILoopFilterVal").data("kendoMaskedTextBox").value();

                        s.page = options.data.page;
                        s.skip = options.data.skip;
                        s.take = options.data.take;

                        $.ajax({ 
                            url: '/api/EDILoops/GetRecords/', 
                            contentType: 'application/json; charset=utf-8', 
                            dataType: 'json', 
                            data: { filter: JSON.stringify(s) }, 
                            headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                            success: function(data) {
                                
                                options.success(data);
                                // Updated by SRP on 3/8/2018
                                try {                                    
                                    var blnSuccess = false;
                                    var blnErrorShown = false;
                                    var strValidationMsg = "";
                                    if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                        if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                            blnErrorShown = true;
                                            ngl.showErrMsg("Get EDILoop Failure", data.Errors, null);
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
                                        if (strValidationMsg.length < 1) { strValidationMsg = "Get EDILoop Failure"; }
                                        ngl.showErrMsg("Get EDILoop Failure", strValidationMsg, null);
                                    }
                                } catch (err) {
                                    ngl.showErrMsg(err.name, err.description, null);
                                }
                               
                            }, 
                            error: function (xhr, textStatus, error) {
                                var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Get Data Failure");
                                ngl.showErrMsg("Get EDILoop Failure", sMsg, null); 
                                
                            } 
                        }); 
                    },      
                    create: function(options) {
                        $.ajax({
                            async: false,
                            type: "POST",
                            url: "api/EDILoops/SaveEDILoop",
                            contentType: "application/json; charset=utf-8",
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
                                            ngl.showErrMsg("Save EDILoop Failure", data.Errors, null);
                                        }
                                        else {
                                            if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                                    blnSuccess = true;
                                                    refreshEDILoopGrid();
                                                }
                                            }
                                        }
                                    }
                                    if (blnSuccess === false && blnErrorShown === false) {
                                        if (strValidationMsg.length < 1) { strValidationMsg = "Save EDILoop Failure"; }
                                        ngl.showErrMsg("Save EDILoop Failure", strValidationMsg, null);
                                    }
                                } catch (err) {
                                    ngl.showErrMsg(err.name, err.description, null);
                                }
                            },
                            error: function (xhr, textStatus, error) {
                                var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure");
                                ngl.showErrMsg("Save EDILoop Failure", sMsg, null);                        
                            }
                        });
                    },
                    update: function(options) {
                        $.ajax({ 
                            url: 'api/EDILoops/PostSave', 
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
                                            ngl.showErrMsg("Save1 EDILoop Failure", data.Errors, null);
                                        }
                                        else {
                                            if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                                    blnSuccess = true;
                                                }
                                            }
                                        }
                                    }
                                    if (blnSuccess === false && blnErrorShown === false) {
                                        if (strValidationMsg.length < 1) { strValidationMsg = "Save EDILoop Failure"; }
                                        ngl.showErrMsg("Save EDILoop1 Failure", strValidationMsg, null);
                                    }
                                } catch (err) {
                                    ngl.showErrMsg(err.name, err.description, null);
                                }
                                //refresh the grid
                                if (ngl.isFunction(refreshEDILoopGrid)) {
                                    refreshEDILoopGrid();
                                }
                            },
                            error: function (xhr, textStatus, error) {
                                var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure");
                                ngl.showErrMsg("Save EDILoop Failure1", sMsg, null); 
                            } 
                        });
                    },
                    destroy: function(options) {
                        $.ajax({
                            url: 'api/EDILoops/DeleteRecord', 
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
                                        ngl.showWarningMsg("The loop is already associated in document, it cannot be deleted!", "", null);
                                        
                                    }
                                    if(typeof (data) !== 'undefined' && ngl.isObject(data)){
                                        
                                        if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                            blnErrorShown = true;
                                            refreshEDILoopGrid();
                                            ngl.showWarningMsg("The loop is already associated in document, it cannot be deleted!", data.Errors, null);
                                        }
                                    }
                                
                                } catch (err) {
                                }
                                if (ngl.isFunction(refreshEDILoopGrid)) {
                                    refreshEDILoopGrid();
                                }
                            },
                            error: function (xhr, textStatus, error) {
                                var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "delete EDILoops Failure");
                                ngl.showErrMsg("delete EDILoops Failure", sMsg, null); 
                            } 
                        });
                    },
                    parameterMap: function(options, operation) { return options; } 
                },  
                schema: {
                    data: "Data",
                    total: "Count",
                    model: {
                        id: "LoopControl",
                        fields: {
                            LoopControl: { type: "number" },
                            LoopName: { type: "string",
                                validation: {
                                    required: {
                                        message: "Loop Name is required."
                                    }
                                    
                                }
                            },
                            LoopDesc: { type: "string" },
                            LoopMinCount: { type: "number", validation: { required: true, min: 0} },
                            LoopMaxCount: { type: "number", validation: { required: true, min: 1}  },
                            LoopDisabled: {type:"boolean"},
                            LoopModDate: {type:"date",editable: false },
                            LoopModUser: {type:"string",editable: false}
                        }
                    },
                    errors: "Errors"
                },
                error: function(xhr, textStatus, error) {
                    ngl.showErrMsg("Access EDILoop Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges();
                }
            });

                var grid = $("#EDILoopGrid").kendoGrid({
                dataSource: EDILoop,
                pageable: true,
                sortable: {
                    mode: "single",
                    allowUnsort: true
                },
                edit: onGridEditing,
                height: 300,
                sort: function(e) {
                    if (!e.sort.dir) { e.sort.dir == ""; e.sort.field == ""; }
                    if (!e.sort.field) { e.sort.field == ""; }
                    $("#txtEDILoopSortDirection").val(e.sort.dir);
                    $("#txtEDILoopSortField").val(e.sort.field);
                },
                dataBound: function(e) { 
                    var tObj = this; 
                    if (typeof (EDILoopGridDataBoundCallBack) !== 'undefined' && ngl.isFunction(EDILoopGridDataBoundCallBack)) { EDILoopGridDataBoundCallBack(e,tObj); } 
                },
                resizable: true,
                groupable: true, 
                editable: "inline",
                columns: [
                    { field: "LoopControl",  title: "LoopControl", hidden: true },
                    { field: "LoopName", title: "Loop Name", width: 250, hidden: false },
                    { field: "LoopDesc", title: "Loop Description", width: 300,  hidden: false, template:"#if(LoopDesc.length>50){# # var myContent =LoopDesc; #  # var dcontent = myContent.substring(0,50)+'...'; # <span>#=kendo.toString(dcontent)#</span> #}else{# <span>#=LoopDesc#</span> #}#" ,class: "breakWord20", width: 200, editor: textareaEditor},  
                    { field: "LoopMinCount",  title: "Mininum Loops Allowed", width: 100, hidden: false, editor:editNumber  },
                    { field: "LoopMaxCount", title: "Maximum Loops Allowed", width: 100, hidden: false, editor:editNumber },
                    { field: "LoopDisabled", title: "Disabled", width: 100, template: '<input type="checkbox" id="SelectedCB" #= LoopDisabled ? checked="checked" : "" # disabled="disabled" />' , hidden: false },
                    { field: "LoopModDate", title: "Mod Date", template: "#= kendo.toString(kendo.parseDate(LoopModDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') #",hidden: true },//Updated By SRP on 3/8/2018
                    { field: "LoopModUser", title: "Mod User", hidden: true },
                    { command: [{ name: "edit", text:{edit: "", update: "", cancel: ""}},{name: "destroy", text: "",  imageClass: "k-i-delete", iconClass: "k-icon"}], title: "Action", width: "100px" }
                ]
                
            }).data("kendoGrid");
                function myEditorFunction (container, options) {
                    $('<input/>')
                        .appendTo(container)
                        .kendoNumericTextBox({
                            min: 0
                        });
                }
                $("#EDILoopGrid").kendoTooltip({
                    filter: ".k-grid-edit",
                    content: "Edit Loop&nbsp;Details"
                });

                $("#EDILoopGrid").kendoTooltip({
                    filter: ".k-grid-delete",
                    content: "Delete Loop"
                });
            
                $( "#btnAddLoop").click(function() {
                grid.options.editable = "inline";
                grid.addRow();
                grid.options.editable = "inline";
                
                });
            function onGridEditing(arg) {
                arg.container.find("input[name='LoopName']").attr('maxlength', '50');
                arg.container.find("input[name='LoopDesc']").attr('maxlength', '255');
                arg.container.find("input[name='LoopMinCount']").attr('maxlength', '9'); 
                arg.container.find("input[name='LoopMaxCount']").attr('maxlength', '9'); 
            }
            /////////////Edit Mode textarea for description///////////
            function textareaEditor(container, options) {
                $('<textarea data-bind="value: ' + options.field + '" cols="45" class="k-textbox" rows="3"></textarea>')
                    .appendTo(container);
            }
            ///////////Edit Mode Spinner Remove and Not Allow Decimal///////////
            function editNumber(container, options) {
                $('<input data-bind=value:' + options.field + '  onkeypress="return event.charCode >= 48 && event.charCode <= 57" />')
                    .appendTo(container)
                    .kendoNumericTextBox({
                        format  : "{0:n0}",
                        min: 0
                    });
            }

            ////////////wnd Message///////////////////
            wndMessage = $("#wndMessage").kendoWindow({
                title: "Message Window",
                height: 100,
                width: 270,
                modal: true,
                visible: false,
                actions: ["save", "Minimize", "Maximize", "Close"],
            }).data("kendoWindow");

            $("#wndMessage").data("kendoWindow").wrapper.find(".k-svg-i-save").click(function (e) { SaveMessage(); }); 
           
            ////////////wnd AddEDILoop/////////////////

            kendoWin.height = 425;
            kendoWin.width = 300;
            
            kendoWinStyle({"padding":"10px 10px 10px 30px"});//For Styling kendo Window
            wndAddDocType = $("#wndAddEDILoop").kendoWindow(kendoWin).data("kendoWindow");
            $("#wndAddEDILoop").data("kendoWindow").wrapper.find(".k-svg-i-save").click(function (e) { SaveEDILoop(); });
        });

        </script>
        <style>
            .k-grid tbody .k-button {
                min-width: 18px;
                width: 28px;
            }

            .k-grid tbody tr td {
                vertical-align: top;
            }
        </style>
    </div>
</body>

</html>
