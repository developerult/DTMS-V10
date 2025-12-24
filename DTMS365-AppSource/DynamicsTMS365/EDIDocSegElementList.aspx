<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EDIDocSegElementList.aspx.cs" Inherits="DynamicsTMS365.EDIDocSegElementList" %>

<!DOCTYPE html>

<html>
<head>
    <title>DocSegElementsList</title>         
        <link href="Content/kendoR32023/classic-opal.css" rel="stylesheet" />               
        <link href="Content/NGL/v-8.5.4.001/common.css" rel="stylesheet" />   

   <style>
       html,body {
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
    <div id="example" style="height: 100%; width: 100%; margin-top: 2px;">
        <div id="vertical" style="height: 98%; width: 98%;">
            <div id="menu-pane" style="height: 100%; width: 100%; background-color: white;">
                <div id="tab" class="menuBarTab"></div>
            </div>
            <%--Action Menu TabStrip--%>
            <div id="top-pane">
                <div id="horizontal" style="height: 100%; width: 100%;">
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
                                <span id="ExpandParSpan" style="display: none;"><a onclick="expandFastTab('ExpandParSpan','CollapseParSpan','ParHeader','ParDetail');"><span style="font-size: small; font-weight: bold;" class="k-icon k-i-chevron-down"></span></a></span>
                                <span id="CollapseParSpan" style="display: normal;"><a onclick="collapseFastTab('ExpandParSpan','CollapseParSpan','ParHeader',null);"><span style="font-size: small; font-weight: bold;" class="k-icon k-i-chevron-up"></span></a></span>
                                <span style="font-size: small; font-weight: bold">EDI Document Segment Elements</span>
                            </div>
                            <div id="ParHeader" class="OpenOrders">
                                <div id="Parwrapper">
                                    <%--Filters--%>
                                    <div id="ParFilterFastTab">
                                        <span id="ExpandParFilterFastTabSpan" style="display: none;"><a onclick="expandFastTab('ExpandParFilterFastTabSpan','CollapseParFilterFastTabSpan','ParFilterFastTabHeader',null);"><span style="font-size: small; font-weight: bold;" class="k-icon k-i-chevron-down"></span></a></span>
                                        <span id="CollapseParFilterFastTabSpan" style="display: normal;"><a onclick="collapseFastTab('ExpandParFilterFastTabSpan','CollapseParFilterFastTabSpan','ParFilterFastTabHeader',null);"><span style="font-size: small; font-weight: bold;" class="k-icon k-i-chevron-up"></span></a></span>
                                        <span style="font-size: small; font-weight: bold">Filters </span>
                                        <div id="ParFilterFastTabHeader" style="padding: 10px;">
                                            <span>
                                                <label for="ddlEDIDocSegmentElementFilters">Filter by:</label>
                                                <input id="ddlEDIDocSegmentElementFilters" />
                                                <span id="spDocSegmentElementFilterText">
                                                    <input id="txtDocSegmentElementFilterVal" /></span>
                                                <span id="spDocSegmentElementFilterDates">
                                                    <label for="dpDocSegmentElementFilterFrom">From:</label>
                                                    <input id="dpDocSegmentElementFilterFrom" />
                                                    <label for="dpDocSegmentElementFilterTo">To:</label>
                                                    <input id="dpDocSegmentElementFilterTo" />
                                                </span>
                                                <span id="spDocSegmentElementFilterButtons"><a id="btnDocSegmentElementFilter"></a><a id="btnDocSegmentElementClearFilter"></a></span>
                                            </span>
                                            <input id="txtDocSegmentElementSortDirection" type="hidden" />
                                            <input id="txtDocSegmentElementSortField" type="hidden" />
                                        </div>
                                    </div>
                                    <%--Grid--%>
                                    <div id="DocSegElementListGrid"></div>
                                </div>
                            </div>
                        </div>

                        <!-- End Page Content -->
                </div>
              </div>
            </div>
            <div id="bottom-pane" class="k-block" style="height: 100%; width: 100%;">
                <div class="pane-content">
                    <div><span>
                        <p>If you experience problems with this site, call (847)963-0007 24/7 or email our support group at <a href='mailto: support@nextgeneration.com'>support@nextgeneration.com</a></p>
                    </span></div>
                </div>
            </div>
        </div>

        <%--Popup Window HTML--%>
      <%--  <div id="wndMessage">
            <div>
                <h2>Enter a Message</h2>
                <input id="txtUserInput" style="width: 250px;" />
            </div>
        </div>--%>

        <%--Added By SRP on 3/5/2018 EDIElement for KendoWindow--%>
        <% Response.WriteFile("~/Views/DocSegElementAddWindow.html"); %>

        <%--      You can either write the window HTML directly on the aspx page (like wndMessage above), or you can write it in a separate html file
          in the EDIMaintTool/Views folder and call Response.WriteFile (like AnimalAddWindow). 
          When using the latter, each window must have its own .html file. 
          I write window Views primarily for the following reasons: 
          If my HTML code is long and I don't want to clutter up my aspx page
          If I want to use/share the same window code in multiple aspx pages.
          However you prefer to do it is up to you --%>


        <% Response.Write(AuthLoginNotificationHTML); %>

        <script>
        
        //*************  Call Back Functions  *******************
        function DocSegElementListGridDataBoundCallBack(e,tGrid){           
            oKendoGrid = tGrid;
            //add databound code here
        }

        //*************  Action Functions  **********************
        function execActionClick(btn, proc){
            if (btn.id == "btnAddDocSegmentElement") { 
                var s = new AllFilter();
                s={"filterName":"None","filterValue":"","page":1,"skip":0,"take":10};
                var doctype=0;
                var segment=0;
                window.location.assign("EDIDocSegmentElement.aspx");
                //openDocSegmentElementAddWindow();
            }
        }

        function openSegmentElementEditWindow(e) {
            //Get the record data from the grid
            var s = new AllFilter();
            s={"filterName":"None","filterValue":"","page":1,"skip":0,"take":10};
            var item = this.dataItem($(e.currentTarget).closest("tr"));
            var doctype=item.EDITControl;
            var segment=item.SegmentControl;
            window.location.assign("EDIDocSegmentElement.aspx?filter="+s+"&doctype="+doctype+"&segment="+segment+"");
            
        }

        function refreshDocSegElementListGrid() {
            //oKendoGrid gets set during DocSegElementListGridDataBoundCallBack()
            if (typeof (oKendoGrid) !== 'undefined' && ngl.isObject(oKendoGrid)) {
                oKendoGrid.dataSource.read();
            }
           
        }

        

        $(document).ready(function () {
            //********Action tab and Add Button**********//
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
                        content: "<button id='btnAddDocSegmentElement' class='k-button actionBarButton' type='button' onclick='execActionClick(btnAddDocSegmentElement, 160);'><span class='k-icon k-i-add'></span>Add EDI Document Segment Element</button>"
                    }
                ]
            }).data('kendoTabStrip').select(0);
            
            //*************Menu Tree List*********//
            var PageReadyJS = $('#menuTree').kendoTreeView({
                dataUrlField: 'LinksTo',
                dataSource: {
                    data:menuitems.data
                    
                },
                loadOnDemand: false
            }).data('kendoTreeView'), handleTextBox = function (callback) { return function (e) { if (e.type != 'keypress' || kendo.keys.ENTER == e.keyCode) { callback(e); } }; };

            control = <%=UserControl%>;           

            //set default message
            var l = "<h3 style=' margin-left:10px'>Manage EDI Document Segment Elements</h3>";
            $("#txtScreenMessage").html(l);

            ///**********Filters**************///
            var EDIDocSegmentElementFilterData = [ 
               { text: "", value: "None" },
               { text: "Document Type", value: "EDITName" },
               { text: "Segment Name", value: "SegmentName" },
            ];
            
            $("#ddlEDIDocSegmentElementFilters").kendoDropDownList({
                dataTextField: "text",
                dataValueField: "value",
                placeholder:"select",
                dataSource: EDIDocSegmentElementFilterData,
                select: function(e) {
                    var name = e.dataItem.text; 
                    var val = e.dataItem.value; 
                    $("#txtDocSegmentElementFilterVal").data("kendoMaskedTextBox").value("");
                    $("#dpDocSegmentElementFilterFrom").data("kendoDatePicker").value("");
                    $("#dpDocSegmentElementFilterTo").data("kendoDatePicker").value("");
                    switch(val){
                        case "None":
                            $("#spDocSegmentElementFilterText").hide();
                            $("#spDocSegmentElementFilterDates").hide();
                            $("#spDocSegmentElementFilterButtons").hide(); 
                            break; 
                        case "NoDatesAvailable":
                            $("#spDocSegmentElementFilterText").hide();
                            $("#spDocSegmentElementFilterDates").show();
                            $("#spDocSegmentElementFilterButtons").show();
                            break;
                        default:
                            $("#spDocSegmentElementFilterText").show();
                            $("#spDocSegmentElementFilterDates").hide();
                            $("#spDocSegmentElementFilterButtons").show();
                            break;
                    }
                }
            });
            
            $("#txtDocSegmentElementFilterVal").kendoMaskedTextBox(); 
            $("#dpDocSegmentElementFilterFrom").kendoDatePicker(); 
            $("#dpDocSegmentElementFilterTo").kendoDatePicker(); 
            $("#btnDocSegmentElementFilter").kendoButton({
                icon: "filter",
                click: function(e) { 
                    var dataItem = $("#ddlEDIDocSegmentElementFilters").data("kendoDropDownList").dataItem(); 
                    
                    if (1 === 0){ 
                        var dtFrom = $("#dpDocSegmentElementFilterFrom").data("kendoDatePicker").value(); 
                        if (!dtFrom) { ngl.showErrMsg("Required Fields", "Filter From date cannot be null", null); return;}
                    } 
                    $("#DocSegElementListGrid").data("kendoGrid").dataSource.read();
                }
            }); 
            
            $("#btnDocSegmentElementClearFilter").kendoButton({
                icon: "filter-clear",
                click: function(e) {
                    var dropdownlist = $("#ddlEDIDocSegmentElementFilters").data("kendoDropDownList"); 
                    dropdownlist.select(0);
                    dropdownlist.trigger("change");
                    $("#txtDocSegmentElementFilterVal").data("kendoMaskedTextBox").value("");
                    $("#dpDocSegmentElementFilterFrom").data("kendoDatePicker").value(""); 
                    $("#dpDocSegmentElementFilterTo").data("kendoDatePicker").value(""); 
                    $("#spDocSegmentElementFilterText").hide(); 
                    $("#spDocSegmentElementFilterDates").hide(); 
                    $("#spDocSegmentElementFilterButtons").hide();
                    $("#DocSegElementListGrid").data("kendoGrid").dataSource.read();
                }
            }); 
            
            $("#txtDocSegmentElementFilterVal").data("kendoMaskedTextBox").value("");
            $("#dpDocSegmentElementFilterFrom").data("kendoDatePicker").value("");
            $("#dpDocSegmentElementFilterTo").data("kendoDatePicker").value("");
            $("#spDocSegmentElementFilterText").hide();
            $("#spDocSegmentElementFilterDates").hide();
            $("#spDocSegmentElementFilterButtons").hide();
            var s = new AllFilter();


            ////////////Grid///////////////////
            EDIType = new kendo.data.DataSource({
                serverSorting: true, 
                serverPaging: true, 
                pageSize: 10,
                transport: { 
                    read: function(options) { 
                        var s = new AllFilter();

                        s.filterName = $("#ddlEDIDocSegmentElementFilters").data("kendoDropDownList").value();
                        s.filterValue = $("#txtDocSegmentElementFilterVal").data("kendoMaskedTextBox").value();

                        s.page = options.data.page;
                        s.skip = options.data.skip;
                        s.take = options.data.take;
                        console.log(options.data);

                        $.ajax({ 
                            url: '/api/EDIDocSegElementList/GetRecords/',
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
                                            ngl.showErrMsg("Get DocSegmentElement Failure", data.Errors, null);
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
                                        if (strValidationMsg.length < 1) { strValidationMsg = "Get DocSegmentElement Failure"; }
                                        ngl.showErrMsg("Get DocSegmentElement Failure", strValidationMsg, null);
                                    }
                                } catch (err) {
                                    ngl.showErrMsg(err.name, err.description, null);
                                }
                               
                            }, 
                            error: function (xhr, textStatus, error) {
                                var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Get Data Failure");
                                ngl.showErrMsg("Get DocSegmentElement Failure", sMsg, null); 
                                
                            } 
                        }); 
                    },
                    update: function(options) {
                        
                        $.ajax({ 
                            url: 'api/EDIDocSegmentElement/GetRecords', 
                            type: "GET",
                            contentType: 'application/json; charset=utf-8', 
                            dataType: 'json', 
                            data: { filter: JSON.stringify(s)},
                            headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                            success: function (data) {     
                                try {                                    
                                    var blnSuccess = false;
                                    var blnErrorShown = false;
                                    var strValidationMsg = "";
                                    if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                        if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                            blnErrorShown = true;
                                            ngl.showErrMsg("Save EDIElement Failure", data.Errors, null);
                                        }
                                        else {
                                            if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                                    blnSuccess = true;
                                                    //do other stuff if needed
                                                }
                                            }
                                        }
                                    }
                                    if (blnSuccess === false && blnErrorShown === false) {
                                        if (strValidationMsg.length < 1) { strValidationMsg = "Save EDIElement Failure"; }
                                        ngl.showErrMsg("Save EDIElement Failure", strValidationMsg, null);
                                    }
                                } catch (err) {
                                    ngl.showErrMsg(err.name, err.description, null);
                                }
                                //*********refresh the grid************//
                                if (ngl.isFunction(refreshDocSegElementListGrid)) {
                                    refreshDocSegElementListGrid();
                                }
                            },
                            error: function (xhr, textStatus, error) {
                                var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure");
                                ngl.showErrMsg("Save EDIElement Failure", sMsg, null); 
                            } 
                        });
                    },
                    parameterMap: function(options, operation) { return options; } 
                },  
                schema: {
                    data: "Data",
                    total: "Count",
                    model: {
                        id: "DSEControl",
                        fields: {
                            DSEControl: { type: "number" },
                            EDITName: { type: "string" },
                            SegmentName: { type: "string" },
                            EDITControl: { type: "int" },
                            SegmentControl: { type: "int" },
                            DSEDisabled: {type:"boolean"},
                        }
                    },
                    errors: "Errors"
                },
                error: function(xhr, textStatus, error) {
                    ngl.showErrMsg("Access EDIElement Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges();
                }
            });
            
            $('#DocSegElementListGrid').kendoGrid({
                dataSource: EDIType,
                pageable: true,
                sortable: {
                    mode: "single",
                    allowUnsort: true
                },
             //**********edit: onGridEditing*********//
                height: 300,
                sort: function(e) {
                    if (!e.sort.dir) { e.sort.dir == ""; e.sort.field == ""; }
                    if (!e.sort.field) { e.sort.field == ""; }
                    $("#txtDocSegmentElementSortDirection").val(e.sort.dir);
                    $("#txtDocSegmentElementSortField").val(e.sort.field);
                },
                dataBound: function(e) { 
                    var tObj = this; 
                    if (typeof (DocSegElementListGridDataBoundCallBack) !== 'undefined' && ngl.isFunction(DocSegElementListGridDataBoundCallBack)) { DocSegElementListGridDataBoundCallBack(e,tObj); } 
                },
                resizable: true,
                groupable: true, 
                editable: "inline",
                columns: [
                    { field: "DSEControl", title: "DSEControl", hidden: true },
                    { field: "EDITName", title: "Document Type", hidden: false,},
                    { field: "SegmentName", title: "Segment Name ", hidden: false },
                    { field: "EDITControl", title: "EDITControl", hidden: false,},
                    { field: "SegmentControl", title: "SegmentControl", hidden: false },
                    { field: "DSEDisabled", title: "Disabled", template: '<input type="checkbox" id="SelectedCB" #= DSEDisabled ? checked="checked" : "" # disabled="disabled" />' , hidden: false },
                    //{ command: [{ name: "edit", text:{edit: "",update: "", cancel: ""}}], title: "Action", width: "100px" }
                    { command: [{ name: "edit element", text: "", iconClass: "k-icon k-i-pencil", click: openSegmentElementEditWindow},{name: "destroy", text: "" }], title: " ", width: "100px" }
                ],
            });

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
