<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EDIElement.aspx.cs" Inherits="DynamicsTMS365.EDIElement" %>

<!DOCTYPE html>

<html>
<head>
    <title>Manage EDI Elements</title>         
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

    <div id="example" style="height: 100%; width: 100%; margin-top: 2px;">
        <div id="vertical" style="height: 98%; width: 98%;">
            <%--Action Menu TabStrip--%>
            <div id="menu-pane" style="height: 100%; width: 100%; background-color: white;">
                <div id="tab" class="menuBarTab"></div>
            </div>
            <div id="top-pane">
                <div id="horizontal" style="height: 100%; width: 100%;">
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
                                <span id="ExpandParSpan" style="display: none;"><a onclick="expandFastTab('ExpandParSpan','CollapseParSpan','ParHeader','ParDetail');"><span style="font-size: small; font-weight: bold;" class="k-icon k-i-chevron-down"></span></a></span>
                                <span id="CollapseParSpan" style="display: normal;"><a onclick="collapseFastTab('ExpandParSpan','CollapseParSpan','ParHeader',null);"><span style="font-size: small; font-weight: bold;" class="k-icon k-i-chevron-up"></span></a></span>
                                <span style="font-size: small; font-weight: bold">EDI Elements</span>
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
                                                <label for="ddlEDIElementFilters">Filter by:</label>
                                                <input id="ddlEDIElementFilters" />
                                                <span id="spElementFilterText">
                                                    <input id="txtElementFilterVal" /></span>
                                                <span id="spElementFilterDates">
                                                    <label for="dpElementFilterFrom">From:</label>
                                                    <input id="dpElementFilterFrom" />
                                                    <label for="dpElementFilterTo">To:</label>
                                                    <input id="dpElementFilterTo" />
                                                </span>
                                                <span id="spElementFilterButtons"><a id="btnElementFilter"></a><a id="btnElementClearFilter"></a></span>
                                            </span>
                                            <input id="txtElementSortDirection" type="hidden" />
                                            <input id="txtElementSortField" type="hidden" />
                                        </div>
                                    </div>
                                    <%--Grid--%>
                                    <div id="ElementGrid"></div>
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
        <div id="wndMessage">
            <div>
                <h2>Enter a Message</h2>
                <input id="txtUserInput" style="width: 250px;" />
            </div>
        </div>

        <%--Added By SRP on 2/22/2018 EDIElement for KendoWindow--%>
        <% Response.WriteFile("~/Views/ElementAddWindow.html"); %>

        <% Response.Write(AuthLoginNotificationHTML); %>
        <script>
        //************* Page Variables **************************
        var PageControl = '<%=PageControl%>';       
            var tObj = this;
            var tPage = this;
        var oKendoGrid = null;

        var dsDataTypeDropDown = kendo.data.DataSource;
        var dsValidationDropDown = kendo.data.DataSource;
        var dsFormatingDropDown = kendo.data.DataSource;
        var wndMessage = kendo.ui.Window;
        var wndAddDocType = kendo.ui.Window; //Added By SRP on 2/22/2018 EDIElement


        //*************  Call Back Functions  *******************
        function ElementGridDataBoundCallBack(e,tGrid){           
            oKendoGrid = tGrid;

            //add databound code here
        }


        //*************  Action Functions  **********************
        function execActionClick(btn, proc){
         
            if (btn.id == "btnAddElement") { //Added By SRP on 2/22/2018 EDIElementAddExample
                openElementAddWindow();
            }
        }

        //Added By By SRP on 2/22/2018 EDIElementAddSample
        function openElementAddWindow() {

            //Validation Display
            var valcheck = $("#name-validation").hasClass("hide-display");

            if (valcheck == false) {
                $("#name-validation").addClass("hide-display");
            }
            //This is how you can change the title of the window (in case you want to share the same window for Add and Edit- Note: In grids editing does not have to always be inline)
            $("#wndAddElement").data("kendoWindow").title("Add EDI Element");

            //Clear all previous values since this is Add New
            $("#txtElementControl").val(0);
            $("#txtElementName").data("kendoMaskedTextBox").value("");
            $("#txtElementDescription").data("kendoMaskedTextBox").value("");
            $("#txtElementMinLength").data("kendoMaskedTextBox").value("1");
            $("#txtElementMaxLength").data("kendoMaskedTextBox").value(0);
            var ddl1 = $("#ddlEditType").data("kendoDropDownList");
            //ddl1.select(function(dataItem) { return dataItem.Control === 4; });
            ddl1.readonly(false);
            ddl1.select(0);

            var ddl2 = $("#ddlValidationType").data("kendoDropDownList");
            ddl2.readonly(false);
            ddl2.select(0);

            var ddl3 = $("#ddlFormating").data("kendoDropDownList");
            ddl3.readonly(false);
            ddl3.select(0);

            

            wndAddDocType.center().open();
        }

        function openElementEditWindow(e) {
            //Get the record data from the grid
            var item = this.dataItem($(e.currentTarget).closest("tr")); 
            
            $("#wndAddElement").data("kendoWindow").title("Edit Element Details");
            $("#txtElemencreatuser").val(item.ElementCreateUser);
            $("#txtElementcreatedate").val(item.ElementCreateDate);
            $("#txtElementupdate").val(item.ElementUpdated);
            $("#txtElementControl").val(item.ElementControl);
            $("#txtElementName").data("kendoMaskedTextBox").value(item.ElementName);
            $("#txtElementDescription").data("kendoMaskedTextBox").value(item.ElementDescription);            
            $("#txtElementMinLength").data("kendoMaskedTextBox").value(item.ElementMinLength);
            $("#txtElementMaxLength").data("kendoMaskedTextBox").value(item.ElementMaxLength);
            $("#chksegmentupdate").val(item.ElementDisabled);
            var ddl1 = $("#ddlEditType").data("kendoDropDownList");
            ddl1.value(parseInt(item.ElementEDIDataTypeControl));

            var ddl2 = $("#ddlValidationType").data("kendoDropDownList");
            ddl2.value(parseInt(item.ElementValidationTypeControl));

            var ddl3 = $("#ddlFormating").data("kendoDropDownList");
            ddl3.value(parseInt(item.ElementFormattingFnControl));


            wndAddDocType.center().open();
        }

        //Added By By SRP on 2/22/2018 EDIElementAddSample
        function SaveEDIDocType() {
            var otmp = $("#focusCancel").focus();

            //Created by SRP on 2/28/2018 for Input Validation
            

            var submit = true;
            var tName = $("#txtElementName").val();
            var sMinC = $("#txtElementMinLength").val();
            var sMaxC = $("#txtElementMaxLength").val();
            if (tName == "") {

                $("#name-validation").removeClass("hide-display");
                submit = false;
            }
            
            var tlist = $("#ddlEditType").val();

            if (tlist =="") {

                $("#edittype-validation").removeClass("hide-display");
                submit = false;
            }
            if (sMinC >sMaxC) {
                
                $("#maxnew-validation").removeClass("hide-display");
                $("#txtSegmentMinCount").data("kendoMaskedTextBox").value('');
                $("#txtSegmentMaxCount").data("kendoMaskedTextBox").value('');
                submit = false;
            }
            var item = new NGLElement();

            item.ElementControl = $("#txtElementControl").val();

            if(item.ElementControl == 0){
                item.ElementName = $("#txtElementName").data("kendoMaskedTextBox").value();
                item.ElementDescription = $("#txtElementDescription").data("kendoMaskedTextBox").value();           
                //var valcheck=$('#ddlValidationType').data("kendoDropDownList");
                //if(valcheck.value()== 1)
                //{
                //    $('#fn-validation').removeClass("hide-display");
                //    submit=false
                //}

                //var fncheck=$('#ddlFormating').data("kendoDropDownList");
                //if(fncheck.value() == 1)
                //{
                //    $('#fn-formating').removeClass("hide-display");
                //    submit=false
                //}
                var dataItemLT = $("#ddlEditType").data("kendoDropDownList").dataItem();
                item.ElementEDIDataTypeControl = dataItemLT.Control;

                var dataItemGT = $("#ddlValidationType").data("kendoDropDownList").dataItem();
                item.ElementValidationTypeControl = dataItemGT.Control;

                var dataItemLEA = $("#ddlFormating").data("kendoDropDownList").dataItem();
                item.ElementFormattingFnControl = dataItemLEA.Control;

                item.ElementMinLength = $("#txtElementMinLength").data("kendoMaskedTextBox").value();

                item.ElementMaxLength = $("#txtElementMaxLength").data("kendoMaskedTextBox").value();
                item.ElementDisabled= $("#chksegmentupdate").is(":checked");
                if (submit == true) {

                    $.ajax({
                        async: false,
                        type: "POST",
                        url: "api/EDIElement/SaveEDIElement",
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
                                        ngl.showErrMsg("Save EDIElement Failure", data.Errors, null);
                                    }
                                    else if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                        blnSuccess = true;
                                        refreshElementGrid();
                                    }
                                }
                                if (blnSuccess === false && blnErrorShown === false) {
                                    if (strValidationMsg.length < 1) { strValidationMsg = "Save EDIElement Failure"; }
                                    ngl.showErrMsg("Save EDIElement Failure", strValidationMsg, null);
                                }
                            } catch (err) {
                                ngl.showErrMsg(err.name, err.description, null);
                            }
                        },
                        error: function (xhr, textStatus, error) {
                            var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure");
                            ngl.showErrMsg("Save EDIElement Failure", sMsg, null);
                        }
                    });
                    wndAddDocType.close();
                }
            }
            else{
                
                item.ElementName = $("#txtElementName").data("kendoMaskedTextBox").value();
                item.ElementCreateUser=$("#txtElemencreatuser").val();
                item.ElementUpdated=$("#txtElementupdate").val();
                item.ElementCreateDate= $("#txtElementcreatedate").val();
                item.ElementDescription = $("#txtElementDescription").data("kendoMaskedTextBox").value();         
                item.ElementDisabled= $("#chksegmentupdate").is(":checked");
                var dataItemLT = $("#ddlEditType").data("kendoDropDownList").dataItem();
                console.log(dataItemLT.Control);
                item.ElementEDIDataTypeControl = dataItemLT.Control;

                var dataItemGT = $("#ddlValidationType").data("kendoDropDownList").dataItem();
                item.ElementValidationTypeControl = dataItemGT.Control;

                var dataItemLEA = $("#ddlFormating").data("kendoDropDownList").dataItem();
                item.ElementFormattingFnControl = dataItemLEA.Control;

                item.ElementMinLength = $("#txtElementMinLength").data("kendoMaskedTextBox").value();

                item.ElementMaxLength = $("#txtElementMaxLength").data("kendoMaskedTextBox").value();

                if (submit == true) {


                    $.ajax({
                        async: false,
                        type: "POST",
                        url: "api/EDIElement/PostSave",
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
                                        ngl.showErrMsg("Save EDIElement Failure", data.Errors, null);
                                    }
                                    else if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                                blnSuccess = true;
                                                refreshElementGrid();
                                            }
                                       
                                    
                                }
                                if (blnSuccess === false && blnErrorShown === false) {
                                    if (strValidationMsg.length < 1) { strValidationMsg = "Save EDIElement Failure"; }
                                    ngl.showErrMsg("Save EDIElement Failure", strValidationMsg, null);
                                }
                            } catch (err) {
                                ngl.showErrMsg(err.name, err.description, null);
                            }
                        },
                        error: function (xhr, textStatus, error) {
                            var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure");
                            ngl.showErrMsg("Save EDIElement Failure", sMsg, null);
                        }
                    });
                    wndAddDocType.close();
                }
            }

        }
        $("#txtElementName").on("change input", function () {
            var name = $(this).val();
            if (name != "") {
                $("#name-validation").addClass("hide-display");
            }
            else {
                $("#name-validation").removeClass("hide-display");
            }

        });

        $("#ddlEditType").on("change input", function () {
            var datatypesval = $(this).val();
            if (datatypesval == 0) {
                $("#datatype-validation").addClass("hide-display");
            }
            else {
                $("#datatype-validation").removeClass("hide-display");
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


        function refreshElementGrid() {
            //oKendoGrid gets set during ElementGridDataBoundCallBack()
            if (typeof (oKendoGrid) !== 'undefined' && ngl.isObject(oKendoGrid)) {
                oKendoGrid.dataSource.read();
            }
           
        }

        function DataTypeDropDownEditor(container, options) {
            $('<input required data-text-field="Name" data-value-field="Control" data-bind="value:' + options.field + '"/>')
                .appendTo(container)
                .kendoDropDownList({
                    dataTextField: "Name",
                    dataValueField: "Control",
                    autoBind: true,
                    dataSource: dsDataTypeDropDown,
                    change: function (e) {
                        var fee = this.dataItem(e.item);
                        options.model.set("Control", fee.ListTypeControl);
                        options.model.set("Name", fee.ListTypeName);
                    }
                });
        }

        function ValidationDropDownEditor(container, options) {
            $('<input required data-text-field="Name" data-value-field="Control" data-bind="value:' + options.field + '"/>')
                .appendTo(container)
                .kendoDropDownList({
                    dataTextField: "Name",
                    dataValueField: "Control",
                    autoBind: true,
                    dataSource: dsValidationDropDown,
                    change: function (e) {
                        var fee = this.dataItem(e.item);
                        options.model.set("Control", fee.ListTypeControl);
                        options.model.set("Name", fee.ListTypeName);
                        
                    }
                });
        }

        function FormatingDropDownEditor(container, options) {
            $('<input required data-text-field="Name" data-value-field="Control" data-bind="value:' + options.field + '"/>')
                .appendTo(container)
                .kendoDropDownList({
                    dataTextField: "Name",
                    dataValueField: "Control",                    
                    autoBind: true,
                    dataSource: dsFormatingDropDown,
                    change: function (e) {
                        var fee = this.dataItem(e.item);
                        options.model.set("Control", fee.ListTypeControl);
                        options.model.set("Name", fee.ListTypeName);
                    }
                });
        }
        //<button id='btnChangeMessage' class='k-button actionBarButton' type='button' onclick='execActionClick(btnChangeMessage, 160);'><span class='k-icon k-i-track-changes-enable'></span>Edit Message</button><button id='btnSpeak' class='k-button actionBarButton' type='button' onclick='execActionClick(btnSpeak, 160);'><span class='k-icon k-i-volume-up'></span>Speak</button><button id='btnExampleAction3' class='k-button actionBarButton' type='button' onclick='execActionClick(btnExampleAction3, 160);'><span class='k-icon k-i-copy'></span>Example Action 3</button>
       

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
                        content: "<button id='btnAddElement' class='k-button actionBarButton' type='button' onclick='execActionClick(btnAddElement, 160);'><span class='k-icon k-i-add'></span>Add EDI Element</button>"
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
            var l = "<h3>Manage EDI Elements </h3>";
            $("#txtScreenMessage").html(l);

            //define Kendo widgets
            $("#txtUserInput").kendoMaskedTextBox();
            $("#txtElementName").kendoMaskedTextBox();    //Added By SRP on 2/22/2018 ElementAddExample
            //$("#txtDocTypeCode").kendoMaskedTextBox();    //Added By SRP on 2/22/2018 ElementAddExample
            $("#txtElementDescription").kendoMaskedTextBox();    //Added By SRP on 2/22/2018 ElementAddExample
            $("#txtElementMinLength").kendoMaskedTextBox();    //Added By SRP on 2/22/2018 ElementAddExample
            $("#txtElementMaxLength").kendoMaskedTextBox();    //Added By SRP on 2/22/2018 ElementAddExample

            ////////////Filters///////////////////
            var EDIElementFilterData = [ 
               { text: "", value: "None" },
               { text: "Element Name", value: "ElementName" },
               { text: "ElementDescription", value: "ElementDescription" },
            ];
            
            $("#ddlEDIElementFilters").kendoDropDownList({
                dataTextField: "text",
                dataValueField: "value",
                placeholder:"select",
                dataSource: EDIElementFilterData,
                select: function(e) {
                    var name = e.dataItem.text; 
                    var val = e.dataItem.value; 
                    $("#txtElementFilterVal").data("kendoMaskedTextBox").value("");
                    $("#dpElementFilterFrom").data("kendoDatePicker").value("");
                    $("#dpElementFilterTo").data("kendoDatePicker").value("");
                    switch(val){
                        case "None":
                            $("#spElementFilterText").hide();
                            $("#spElementFilterDates").hide();
                            $("#spElementFilterButtons").hide(); 
                            break; 
                        case "NoDatesAvailable":
                            $("#spElementFilterText").hide();
                            $("#spElementFilterDates").show();
                            $("#spElementFilterButtons").show();
                            break;
                        default:
                            $("#spElementFilterText").show();
                            $("#spElementFilterDates").hide();
                            $("#spElementFilterButtons").show();
                            break;
                    }
                }
            });
            
            $("#txtElementFilterVal").kendoMaskedTextBox(); 
            $("#dpElementFilterFrom").kendoDatePicker(); 
            $("#dpElementFilterTo").kendoDatePicker(); 
            $("#btnElementFilter").kendoButton({
                icon: "filter",
                click: function(e) { 
                    var dataItem = $("#ddlEDIElementFilters").data("kendoDropDownList").dataItem(); 
                    
                    if (1 === 0){ 
                        var dtFrom = $("#dpElementFilterFrom").data("kendoDatePicker").value(); 
                        if (!dtFrom) { ngl.showErrMsg("Required Fields", "Filter From date cannot be null", null); return;}
                    } 
                    $("#ElementGrid").data("kendoGrid").dataSource.read();
                }
            }); 
            
            $("#btnElementClearFilter").kendoButton({
                icon: "filter-clear",
                click: function(e) {
                    var dropdownlist = $("#ddlEDIElementFilters").data("kendoDropDownList"); 
                    dropdownlist.select(0);
                    dropdownlist.trigger("change");
                    $("#txtElementFilterVal").data("kendoMaskedTextBox").value("");
                    $("#dpElementFilterFrom").data("kendoDatePicker").value(""); 
                    $("#dpElementFilterTo").data("kendoDatePicker").value(""); 
                    $("#spElementFilterText").hide(); 
                    $("#spElementFilterDates").hide(); 
                    $("#spElementFilterButtons").hide();
                    $("#ElementGrid").data("kendoGrid").dataSource.read();
                }
            }); 
            
            $("#txtElementFilterVal").data("kendoMaskedTextBox").value("");
            $("#dpElementFilterFrom").data("kendoDatePicker").value("");
            $("#dpElementFilterTo").data("kendoDatePicker").value("");
            $("#spElementFilterText").hide();
            $("#spElementFilterDates").hide();
            $("#spElementFilterButtons").hide();
            
            dsDataTypeDropDown = new kendo.data.DataSource({
                transport: {
                    read: {
                        url: "api/vLookupList/GetStaticList/64",
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
                            Control: { type: "number" },
                            Name: { type: "string" }, 
                            Description: { type: "string" }
                        }
                    }, 
                    errors: "Errors"
                },
                error: function (xhr, textStatus, error) { ngl.showErrMsg("Get Static List JSON Response Error",formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"), null);  this.cancelChanges(); },
                serverPaging: false,
                serverSorting: false,
                serverFiltering: false
            });

            dsValidationDropDown = new kendo.data.DataSource({
                transport: {
                    read: {
                        url: "api/vLookupList/GetStaticList/66",
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
                            Control: { type: "number" },
                            Name: { type: "string" }, 
                            Description: { type: "string" }
                        }
                    }, 
                    errors: "Errors"
                },
                error: function (xhr, textStatus, error) { ngl.showErrMsg("Get Static List JSON Response Error",formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"), null);  this.cancelChanges(); },
                serverPaging: false,
                serverSorting: false,
                serverFiltering: false
            });

            dsFormatingDropDown = new kendo.data.DataSource({
                transport: {
                    read: {

                        url: "api/vLookupList/GetStaticList/65",
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
                            Control: { type: "number" },
                            Name: { type: "string" }, 
                            Description: { type: "string" }
                        }
                    }, 
                    errors: "Errors"
                },
                error: function (xhr, textStatus, error) { ngl.showErrMsg("Get Static List JSON Response Error",formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"), null);  this.cancelChanges(); },
                serverPaging: false,
                serverSorting: false,
                serverFiltering: false
            });
            
            
            $("#ddlEditType").kendoDropDownList({
                dataTextField: "Name",
                dataValueField: "Control",
                optionLabel:"Select Data Type",
                autoWidth: true,
                filter: "contains",
                index:1,
                dataSource: dsDataTypeDropDown
            });

            
            $("#ddlValidationType").kendoDropDownList({
                dataTextField: "Name",
                dataValueField: "Control",
                autoWidth: true,
                filter: "contains",
                dataSource: dsValidationDropDown

                
            });

             
            $("#ddlFormating").kendoDropDownList({
                dataTextField: "Name",
                dataValueField: "Control",
                autoWidth: true,
                filter: "contains",
                dataSource: dsFormatingDropDown
            });

            ////////////Grid///////////////////
            EDIELEMENT =   ({
                serverSorting: true, 
                serverPaging: true, 
                pageSize: 10,
                transport: { 
                    read: function(options) { 
                        var s = new AllFilter();

                        s.filterName = $("#ddlEDIElementFilters").data("kendoDropDownList").value();
                        s.filterValue = $("#txtElementFilterVal").data("kendoMaskedTextBox").value();

                        s.page = options.data.page;
                        s.skip = options.data.skip;
                        s.take = options.data.take;
                        $.ajax({ 
                            url: '/api/EDIElement/GetRecords/', 
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
                                            ngl.showErrMsg("Get EDIElement Failure", data.Errors, null);
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
                                        if (strValidationMsg.length < 1) { strValidationMsg = "Get EDIElement Failure"; }
                                        ngl.showErrMsg("Get EDIElement Failure", strValidationMsg, null);
                                    }
                                } catch (err) {
                                    ngl.showErrMsg(err.name, err.description, null);
                                }
                               
                            }, 
                            error: function (xhr, textStatus, error) {
                                var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Get Data Failure");
                                ngl.showErrMsg("Get EDIElement Failure", sMsg, null); 
                                
                            } 
                        }); 
                    },      
                   
                    update: function(options) {
                        
                        $.ajax({ 
                            url: 'api/EDIElement/PostSave', 
                            type: "POST",
                            contentType: 'application/json; charset=utf-8', 
                            dataType: 'json', 
                            //data: { filter: JSON.stringify(options.data) }, 
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
                                //refresh the grid
                                if (ngl.isFunction(refreshElementGrid)) {
                                    refreshElementGrid();
                                }
                            },
                            error: function (xhr, textStatus, error) {
                                var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure");
                                ngl.showErrMsg("Save EDIElement Failure", sMsg, null); 
                            } 
                        });
                    },
                    destroy: function(options) {
                        $.ajax({
                            url: 'api/EDIElement/DeleteRecord', 
                            type: 'DELETE',
                            contentType: 'application/json; charset=utf-8', 
                            dataType: 'json', 
                            //data: { filter: JSON.stringify(options.data) }, 
                            data: JSON.stringify(options.data),
                            headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                            success: function (data) {     
                                try {                                    
                                    var blnSuccess = false;
                                    var blnErrorShown = false;
                                    var strValidationMsg = "";
                                    if (data == false) {
                                        ngl.showWarningMsg("Elements already exist with this element, it cannot be deleted!", "", null);
                                        
                                    }
                                    if(typeof (data) !== 'undefined' && ngl.isObject(data)){
                                        
                                        if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                            blnErrorShown = true;
                                            ngl.showWarningMsg("Elements already exist with this element, it cannot be deleted!", data.Errors, null);
                                        }
                                    
                                    }
                                
                                } catch (err) {
                                    ngl.showErrMsg(err.name, err.description, null);
                                }
                                //refresh the grid
                                if (ngl.isFunction(refreshEDITypeGrid)) {
                                    refreshEDITypeGrid();
                                }
                            },
                            error: function (xhr, textStatus, error) {
                                var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "delete EDIElement Failure");
                                ngl.showErrMsg("delete EDIElement Failure", sMsg, null); 
                            } 
                        });
                    },
                    parameterMap: function(options, operation) { return options; } 
                },  
                schema: {
                    data: "Data",
                    total: "Count",
                    model: {
                        id: "ElementControl",
                        fields: {
                            ElementControl: { type: "number" },
                            ElementName: { type: "string" },
                            
                            ElementDescription: { type: "string" },
                            ElementEDIDataTypeControl: { type: "string" },
                            ElementValidationTypeControl: { type: "string" },
                            ElementFormattingFnControl: { type: "string" },
                            
                            ElementMinLength: { type: "number" },
                            ElementMaxLength: { type: "number" },
                            ElementDisabled: {type:"boolean"},
                            ElementModDate: {type:"date",editable: false },//Updated By SRP on 02/22/2018
                            ElementModUser: {type:"string"}//Updated By SRP on 02/22/2018
                            
                        }
                    },
                    errors: "Errors"
                },
                error: function(xhr, textStatus, error) {
                    ngl.showErrMsg("Access EDIElement Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges();
                }
            });
            

            $('#ElementGrid').kendoGrid({
                dataSource: EDIELEMENT,
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
                    $("#txtElementSortDirection").val(e.sort.dir);
                    $("#txtElementSortField").val(e.sort.field);
                },
                dataBound: function(e) { 
                    var tObj = this; 
                    if (typeof (ElementGridDataBoundCallBack) !== 'undefined' && ngl.isFunction(ElementGridDataBoundCallBack)) { ElementGridDataBoundCallBack(e,tObj); } 
                },
                resizable: true,
                groupable: true, 
                editable: "inline",
                columns: [
                    { field: "ElementControl", title: "ElementControl ", hidden: true },
                    { field: "ElementName", width: 300, title: "Element Name", hidden: false },
                    { field: "ElementDescription", width: 300, title: "Element Description", hidden: false,template:"#if(ElementDescription.length>50){# # var myContent =ElementDescription; #  # var dcontent = myContent.substring(0,50)+'...'; # <span>#=kendo.toString(dcontent)#</span> #}else{# <span>#=ElementDescription#</span> #}#"  },                    
                    //created Displaying Dropdown list selected records by SRP and SN on 23/2/2018
                    { field: "ElementEDIDataTypeControl", title: "Data Type",editor: DataTypeDropDownEditor, hidden: false , template:data=>dsDataTypeDropDown._data[Object.values(dsDataTypeDropDown._data).findIndex(x=> x.Control == data.ElementEDIDataTypeControl)].Name},
                    { field: "ElementValidationTypeControl", title: "Validation Type",editor: ValidationDropDownEditor, hidden: false , template:data=>dsValidationDropDown._data[Object.values(dsValidationDropDown._data).findIndex(x=> x.Control == data.ElementValidationTypeControl)].Name},
                    { field: "ElementFormattingFnControl", title: "Formating Function", editor: FormatingDropDownEditor, hidden: false , template:data=>dsFormatingDropDown._data[Object.values(dsFormatingDropDown._data).findIndex(x=> x.Control == data.ElementFormattingFnControl)].Name}, 
                    { field: "ElementMinLength", title: "Minimum Length", hidden: true },
                    { field: "ElementMaxLength", title: "Maximum Length", hidden: true },
                    { field: "ElementDisabled", title: "Disabled", template: '<input type="checkbox" id="SelectedCB" #= ElementDisabled ? checked="checked" : "" # disabled="disabled" />' , hidden: false },
                    { field: "ElementModDate", title: "Mod Date", template: "#= kendo.toString(kendo.parseDate(ElementModDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') #",hidden: true },//Updated By SN on 02/21/2018
                    { field: "ElementModUser", title: "Mod User", hidden: true },//Updated By SRP on 02/21/2018
                    { field: "ElementCreateDate", title: "Create Date", template: "#= kendo.toString(kendo.parseDate(ElementCreateDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') #",hidden: true },//Updated By SN on 02/21/2018
                    { field: "ElementCreateUser", title: "Create User", hidden: true },//Updated By SRP on 02/21/2018
                    { field: "ElementUpdated", title: "Element Updated", hidden: true },//Updated By SRP on 02/21/2018
                   //{ command: [{ name: "edit", text:{edit: "", update: "", cancel: ""}}], title: "Action", width: "100px" }
                   //{ command: [{ name: "edit", text:{edit: "", update: "", cancel: ""}},{name: "destroy", text: "" }], title: "Action", width: "100px" }
                    //Added By SRP on 2/22/18 EDIElement
                    //NOTE: If you wanted to use a popup window for editing, you could do the command this way:
                    { command: [{ name: "edit element", text: "", iconClass: "k-icon k-i-pencil", click: openElementEditWindow},{name: "destroy", text: "" }], title: " ", width: "100px" }
                ]
            });
            
            //function onGridEditing(arg) {
            //    arg.container.find("input[name='ElementName']").attr('maxlength', '50');
            //    arg.container.find("input[name='ElementDescription']").attr('maxlength', '255');
            //}
            
            ////////////wndMessage///////////////////
           
            wndMessage = $("#wndMessage").kendoWindow({
                title: "Message Window",
                height: 100,
                width: 270,
                modal: true,
                visible: false,
                actions: ["save", "Minimize", "Maximize", "Close"],
            }).data("kendoWindow");

            $("#wndMessage").data("kendoWindow").wrapper.find(".k-svg-i-save").click(function (e) { SaveMessage(); }); 

            //Added By SRP on 2/22/18 DocumentTypeAddExample
            ////////////wndAddEDIDocumentType/////////////////

            kendoWin.height =550;// For Updating Kendowindow height config
            kendoWin.width = 400;// For Updating Kendowindow width config
            
            kendoWinStyle({"padding":"10px 10px 10px 30px"});//For Styling kendo Window
            wndAddDocType = $("#wndAddElement").kendoWindow(kendoWin).data("kendoWindow");

            $("#wndAddElement").data("kendoWindow").wrapper.find(".k-svg-i-save").click(function (e) { SaveEDIDocType(); });

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
