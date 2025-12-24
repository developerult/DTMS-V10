<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EDIDocSegmentElement.aspx.cs" Inherits="DynamicsTMS365.EDIDocSegmentElement" %>

<!DOCTYPE html>

<html>
<head>
    <title>Manage Doc. Seg Elements</title>         
        <link href="Content/kendoR32023/classic-opal.css" rel="stylesheet" />               
        <link href="Content/NGL/v-8.5.4.001/common.css" rel="stylesheet" />   

    <script src="http://code.jquery.com/jquery-1.12.4.min.js"></script>
    <script src="http://kendo.cdn.telerik.com/2017.2.504/js/kendo.all.min.js"></script>
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
    
    .demo-section label {  
        margin-bottom: 5px;  
        font-weight: bold;  
        display: inline-block;          
    }  
    #employees {  
        width: 270px;  
    }  
    #example .demo-section {  
        max-width: none;  
        width: 515px;  
    }  
 
    #example .k-listbox {  
        width: 236px;  
        height: 310px;  
    }  
 
        #example .k-listbox:first-of-type {  
            width: 270px;  
            margin-right: 1px;  
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
    <script src="Scripts/NGL/v-8.5.4.006/splitterSegment.js"></script>
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

                   <div id="chaildvertical" style="height: 100%; width: 100%;">
                       
                    <div id="chaild-top-pan" style="height: 100%; width: 100%; background-color: white;">
                        <div style="text-align:center; width:100%;"><h3 style="background-color: white;">Add Document Segment Element</h3>
                            <div id="comment" style="display:none">Please Select both Doctype and Segment</div>
                        </div>
                        <div>
                     <div id="dclbl" style="float:left; width:20%; padding:20px; text-align:right;margin-top:5px" ><b>Document Types</b></div>
                     <div id="dcddl" style="float:left; width:20%; padding:20px" ><input id="ddlDocType" style="width: 250px;" /></div>
                     <div id="stlbl" style="float:left; width:20%; padding:20px; text-align:right;margin-top:5px" ><b>Segments</b></div>
                     <div id="stddl" style="float:left; width:20%; padding:20px" ><input id="ddlSegmentType" style="width: 250px;" /></div>
                            </div>
                    </div>
                    <div id="chaildhorizontal" style="height: 100%; width: 100%;">  
                           
                        <div style="height: 100%; width: 100%;">
                            <h3 id="technology" style="margin-bottom:0px; margin-left:10px">Document Elements</h3>  
                            <br />  
                            <select id="listBox" title="Technologie"  "></select> 
                        </div>     
                   
                    <div id="center-pane">
                        <!-- Begin Page Content -->
                        
                        <%--Message--%>
                        <div id="txtScreenMessage">
                        </div>

                        <!-- Grid Fast Tab -->
                        <div id="id260" style="margin-top: 10px;">
                            <div class="fast-tab">
                                <span id="ExpandParSpan" style="display: none;"><a onclick="expandFastTab('ExpandParSpan','CollapseParSpan','ParHeader','ParDetail');"><span style="font-size: small; font-weight: bold;" class="k-icon k-i-chevron-down"></span></a></span>
                                <span id="CollapseParSpan" style="display: normal;"><a onclick="collapseFastTab('ExpandParSpan','CollapseParSpan','ParHeader',null);"><span style="font-size: small; font-weight: bold;" class="k-icon k-i-chevron-up"></span></a></span>
                                <span style="font-size: small; font-weight: bold">EDI Doc Segment Elements</span>
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
                                    <div id="DocSegmentElementGrid"></div>
                                </div>
                            </div>
                        </div>

                        <!-- End Page Content -->

                    </div>
                </div>

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

        <%--Added By SRP on 3/5/2018 EDIElement for KendoWindow--%>
        <% Response.WriteFile("~/Views/DocSegElementAddWindow.html"); %>

        <% Response.Write(AuthLoginNotificationHTML); %>
        <script>
        //************* Page Variables **************************
        var PageControl = '<%=PageControl%>';       
            var tObj = this;
            var tPage = this;
        var oKendoGrid = null;
        var dsFormatingDropDown = kendo.data.DataSource;
        var dsDocTypeDropDown = kendo.data.DataSource;
        var dsSegmentDropDown = kendo.data.DataSource;
        var dsElementList = kendo.data.DataSource;
        var wndMessage = kendo.ui.Window;
        var wndAddDocType = kendo.ui.Window; //Added By SRP on 3/5/2018 EDIElement


        //*************  Call Back Functions  *******************
        function DocSegmentElementGridDataBoundCallBack(e,tGrid){           
            oKendoGrid = tGrid;

            //add databound code here
        }
        //function getParameterByName(name, url) {
        //    if (!url) url = window.location.href;
        //    name = name.replace(/[\[\]]/g, "\\$&");
        //    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        //        results = regex.exec(url);
        //    if (!results) return null;
        //    if (!results[2]) return '';
        //    return decodeURIComponent(results[2].replace(/\+/g, " "));
        //}
        //var doctype = getParameterByName('doctype'); // "lorem"
        //var segment = getParameterByName('segment'); // "" (present with empty value)
        //console.log(segment);

        //*************  Action Functions  **********************
        //function execActionClick(btn, proc){
         
        //    if (btn.id == "btnAddDocSegmentElement") { 
        //        //openDocSegmentElementAddWindow();
        //    }
        //}
        function execActionClick(btn, proc){
            if (btn.id == "btnAddDocSegmentElement") { 
                //var s = new AllFilter();
                //s={"filterName":"None","filterValue":"","page":1,"skip":0,"take":10};
                //var doctype=0;
                //var segment=0;
                //window.location.assign("EDIDocSegmentElement.aspx?filter="+s+"&doctype="+doctype+"&segment="+segment+"");
                //openDocSegmentElementAddWindow();
            }
        }
        
        

        //Added By By SRP on 3/5/2018 EDIElementAddSample
        function openDocSegmentElementAddWindow() {

            //Validation Display

            var  posCheck = $("#position-validation").hasClass("hide-display");
            if (posCheck == false) {
                $("#position-validation").addClass("hide-display");
            }
            var  fnCheck = $("#fn-validation").hasClass("hide-display");
            if (fnCheck == false) {
                $("#fn-validation").addClass("hide-display");
            }

            //This is how you can change the title of the window (in case you want to share the same window for Add and Edit- Note: In grids editing does not have to always be inline)
            $("#wndAddDocSegmentElement").data("kendoWindow").title("Add EDI DocSegment Element");

            //Clear all previous values since this is Add New
            $("#txtDocSegmentElementControl").val(0);
            $("#txtDSEPosition").data("kendoMaskedTextBox").value(0);
            $("#txtDSEDefaultVal").data("kendoMaskedTextBox").value('');
            var ddl3 = $("#ddlFormating").data("kendoDropDownList");
            var doctype=0;
            var segment=0;
            $("#hdndoctype").val(doctype);
            $("#hdnsegment").val(segment);

            wndAddDocType.center().open();
        }


        function openSegmentElementEditWindow(e) {
            //Get the record data from the grid
            var item = this.dataItem($(e.currentTarget).closest("tr")); 
            //console.log(item);
            $("#wndAddDocSegmentElement").data("kendoWindow").title("Edit Segment Element Details");
            $("#txtDSEControl").val(item.DSEControl);
            $("#txtDSEElementControl").val(item.DSEElementControl);
            $("#txtDSEPosition").data("kendoMaskedTextBox").value(item.DSEPosition);
            $("#txtDSEDefaultVal").data("kendoMaskedTextBox").value(item.DSEDefaultVal);
            var ddl1 = $("#ddlDocType").data("kendoDropDownList");
            ddl1.select(function(dataItem) { return item.EDITControl === doctype; });
            var ddl2 = $("#ddlSegmentType").data("kendoDropDownList");
            ddl2.select(function(dataItem) { return item.SegmentControl === segment; });
            var ddl3 = $("#ddlFormating").data("kendoDropDownList");
            ddl3.select(function(dataItem) { return item.DSEFormattingFnControl === item.DSEFormattingFnControl; });
            var ddl4 =  $("#listBox").data("kendoListBox");
            ddl4.select(function(dataItem) { return item.DSEElementControl === item.DSEElementControl; });
            $("#txtSegmentcreatuser").val(item.DSECreateUser);
            $("#txtSegmentcreatedate").val(item.DSECreateDate);
            $("#txtSegmentupdate").val(item.DSEUpdated);
            $("#chksegmentupdate").val(item.DSEDisabled);
            $("#hdndoctype").val(doctype);
            $("#hdnsegment").val(segment);

            wndAddDocType.center().open();
        }

            //Added By SRP  on 07/03/2018 EDIElementAddSample
        function SaveEDIDocType() {
            var otmp = $("#focusCancel").focus();

            //Added by SRP on 07/03/2018 for Input Validation

            var submit = true;
            var posCheck = $("#txtDSEPosition").val();
            if (posCheck <= 0) {
                $("#position-validation").removeClass("hide-display");
                submit = false;
            }
            var fnCheck = $('#ddlFormating').data("kendoDropDownList");
            if (!fnCheck.value()) {
                $("#fn-validation").removeClass("hide-display");
                submit = false;
            }  
            var item = new NGLElement();

            item.DSEControl = $("#txtDSEControl").val();

            if(item.DSEControl == 0){

                item.DSEPosition = $("#txtDSEPosition").val();

                item.DSEDefaultVal = $("#txtDSEDefaultVal").val();
            
                var dataItemlist = $("#listBox").data("kendoListBox").dataItem();
                item.DSEElementControl = $("#txtDSEElementControl").val();
                //item.DSEElementControl = $("#txtDSEElementControl").val();
                var dataItemLT = $("#ddlDocType").data("kendoDropDownList").dataItem();
                item.DSEEDITControl = dataItemLT.EDITControl;
                //item.DSEEDITControl = $("#hdndoctype").val();
                //item.DSEDefaultVal = $("#txtDSEElementControl").data("kendoMaskedTextBox").value();
                var dataItemGT = $("#ddlSegmentType").data("kendoDropDownList").dataItem();
                item.DSESegmentControl = dataItemGT.SegmentControl;
                //item.DSESegmentControl = $("#hdnsegment").val();
                var dataItemLEA = $("#ddlFormating").data("kendoDropDownList").dataItem();
                item.DSEFormattingFnControl = dataItemLEA.Control;

                if (submit == true) {
                    $.ajax({
                        async: false,
                        type: "POST",
                        url: "api/EDIDocSegmentElement/SaveEDIDocSegmentElement",
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
                                    else {
                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                                blnSuccess = true;
                                                refreshDocSegmentElementGrid();
                                                //var s = new AllFilter();
                                                //s={"filterName":"None","filterValue":"","page":1,"skip":0,"take":10};
                                                //var doctype=item.DSEEDITControl;
                                                //var segment=item.DSESegmentControl;
                                                //var dropdownlist = $("#ddlDocType").data("kendoDropDownList");
                                                //dropdownlist.enable(true);
                                                //var dropdownlist = $("#ddlSegmentType").data("kendoDropDownList");
                                                //dropdownlist.enable(true);
                                                //window.location.assign("EDIDocSegmentElement.aspx?filter="+s+"&doctype="+doctype+"&segment="+segment+"");
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

                item.DSEPosition = $("#txtDSEPosition").val();
                item.DSEDefaultVal = $("#txtDSEDefaultVal").val();          
                var dataItemlist = $("#listBox").data("kendoListBox").dataItem();            
                item.DSEElementControl = $("#txtDSEElementControl").val();
                var dataItemLEA = $("#ddlFormating").data("kendoDropDownList").dataItem();
                item.DSEFormattingFnControl = dataItemLEA.Control;
                //item.DSEEDITControl = $("#hdndoctype").val();
                //item.DSESegmentControl = $("#hdnsegment").val();
                //updated code
                var dataItemLT = $("#ddlDocType").data("kendoDropDownList").dataItem();
                item.DSEEDITControl = dataItemLT.EDITControl;
                var dataItemGT = $("#ddlSegmentType").data("kendoDropDownList").dataItem();
                item.DSESegmentControl = dataItemGT.SegmentControl;
                item.DSECreateUser=$("#txtSegmentcreatuser").val();
                item.DSEUpdated=$("#txtSegmentupdate").val();
                item.DSECreateDate= $("#txtSegmentcreatedate").val();
                item.DSEDisabled= $("#chksegmentupdate").is(":checked");
                if (submit == true) {


                    $.ajax({
                        async: false,
                        type: "POST",
                        url: "api/EDIDocSegmentElement/PostSave",
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
                                        ngl.showErrMsg("Update EDI Segment Element Failure", data.Errors, null);
                                    }
                                    else {
                                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                                blnSuccess = true;
                                                refreshDocSegmentElementGrid();
                                            }
                                        }
                                    }
                                }
                                if (blnSuccess === false && blnErrorShown === false) {
                                    if (strValidationMsg.length < 1) { strValidationMsg = "Update EDI Segment Element Failure"; }
                                    ngl.showErrMsg("Update EDI Segment Element Failure", strValidationMsg, null);
                                }
                            } catch (err) {
                                ngl.showErrMsg(err.name, err.description, null);
                            }
                        },
                        error: function (xhr, textStatus, error) {
                            var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Update EDI Segment Failure");
                            ngl.showErrMsg("UUpdate EDI Segment Element Failure", sMsg, null);
                        }
                    });
                    wndAddDocType.close();
                }
            }
        }
        

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


        function refreshDocSegmentElementGrid() {
            //oKendoGrid gets set during DocSegmentElementGridDataBoundCallBack()
            if (typeof (oKendoGrid) !== 'undefined' && ngl.isObject(oKendoGrid)) {
                oKendoGrid.dataSource.read();
            }
           
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
        function DocTypeDropDownEditor(container, options) {
            $('<input required data-text-field="EDITName" data-value-field="EDITControl" data-bind="value:' + options.field + '"/>')
                .appendTo(container)
                .kendoDropDownList({
                    dataTextField: "EDITName",
                    dataValueField: "EDITControl",                    
                    autoBind: true,
                    dataSource: dsDocTypeDropDown,
                    index:1,
                    change: function (e) {
                        var fee = this.dataItem(e.item);
                        options.model.set("EDITControl", fee.ListTypeControl);
                        options.model.set("EDITName", fee.ListTypeName);
                        $('#hdndoctype').val(fee.ListTypeControl);
                    }
                   
                });
        }
        function SegmentTypeDropDownEditor(container, options) {
            $('<input required data-text-field="SegmentName" data-value-field="SegmentControl" data-bind="value:' + options.field + '"/>')
                .appendTo(container)
                .kendoDropDownList({
                    dataTextField: "SegmentName",
                    dataValueField: "SegmentControl",                    
                    autoBind: true,
                    dataSource: dsSegmentDropDown,
                    change: function (e) {
                        var fee = this.dataItem(e.item);
                        options.model.set("SegmentControl", fee.ListTypeControl);
                        options.model.set("SegmentName", fee.ListTypeName);
                        $('#hdnsegment').val(fee.ListTypeControl);
                    }
                });
        }
        function ElementTypeDropDownEditor(container, options) {
            $('<input required data-text-field="ElementName" data-value-field="ElementControl" data-bind="value:' + options.field + '"/>')
                .appendTo(container)
                .kendoDropDownList({
                    dataTextField: "ElementName",
                    dataValueField: "ElementControl",                    
                    autoBind: true,
                    dataSource: dataSource,
                    change: function (e) {
                        var fee = this.dataItem(e.item);
                        options.model.set("ElementControl", fee.ListTypeControl);
                        options.model.set("ElementName", fee.ListTypeName);
                        $('#txtDSEElementControl').val(fee.ListTypeControl);
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
                        //Modified By SRP on 3/5/2018 ElementAddExample
                        content: "<button id='btnAddDocSegmentElement' class='k-button actionBarButton' type='button' onclick='execActionClick(btnAddDocSegmentElement, 160);'><span class='k-icon k-i-add'></span>Add EDI Doc Segment Element</button>"
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
            var l = "<h3>Manage EDI DocSegment Element </h3>";
            $("#txtScreenMessage").html(l);

            //define Kendo widgets
            $("#txtElementName").kendoMaskedTextBox();
            $("#txtDSEElementControl").kendoMaskedTextBox();    //Added By SRP on 3/5/2018 ElementAddExample
            $("#txtDSEPosition").kendoMaskedTextBox();    //Added By SRP on 3/5/2018 ElementAddExample
            $("#txtDSEDefaultVal").kendoMaskedTextBox();    //Added By SRP on 3/5/2018 ElementAddExample
            //$("#ddlFormating").kendoMaskedTextBox(); 
            //var ddl1 = $("#ddlDocType").data("kendoDropDownList");
            ////ddl1.readonly(false);
            ////ddl1.select(0);
            //ddl1.select(function(dataItem) {
            //    return dataItem.value=== 5;
            //});

            ////////////Filters///////////////////
            var EDIDocSegmentElementFilterData = [ 
               { text: "", value: "None" },
               { text: "DocSeg Position", value: "DSEPosition" },
               { text: "DocSeg DefaultVal", value: "DSEDefaultVal" },
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
                    $("#DocSegmentElementGrid").data("kendoGrid").dataSource.read();
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
                    $("#DocSegmentElementGrid").data("kendoGrid").dataSource.read();
                }
            }); 
            
            $("#txtDocSegmentElementFilterVal").data("kendoMaskedTextBox").value("");
            $("#dpDocSegmentElementFilterFrom").data("kendoDatePicker").value("");
            $("#dpDocSegmentElementFilterTo").data("kendoDatePicker").value("");
            $("#spDocSegmentElementFilterText").hide();
            $("#spDocSegmentElementFilterDates").hide();
            $("#spDocSegmentElementFilterButtons").hide();
            var s = new AllFilter();

            //EDI SocTypes
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

            //EDI Segments
            dsSegmentDropDown = new kendo.data.DataSource({
                transport: {
                    read: {
                        url: "api/EDISegment/GetRecords",
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
                        id: "SegmentControl",
                        fields: {
                            SegmentControl: { type: "number" },
                            SegmentName: { type: "string" }, 
                            SegmentDesc: { type: "string" }
                        }
                    }, 
                    errors: "Errors"
                },
                error: function (xhr, textStatus, error) { ngl.showErrMsg("Get Static List JSON Response Error",formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"), null);  this.cancelChanges(); },
                serverPaging: false,
                serverSorting: false,
                serverFiltering: false
            });
            
            //EDI FnFormating
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
                        id: "FormattingFnControl",
                        fields: {
                            FormattingFnControl: { type: "number" },
                            FormattingFnName: { type: "string" }, 
                            FormattingFnDesc: { type: "string" }
                        }
                    }, 
                    errors: "Errors"
                },
                error: function (xhr, textStatus, error) { ngl.showErrMsg("Get Static List JSON Response Error",formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"), null);  this.cancelChanges(); },
                serverPaging: false,
                serverSorting: false,
                serverFiltering: false
            });


                        
        // SegmentElements Lists     
       dataSource = new kendo.data.DataSource({
                transport: {
                    read: {
                        url: "api/EDIElement/GetRecords",
                        contentType: 'application/json; charset=utf-8',
                        dataType: "json",
                        data: { filter: JSON.stringify({"filterName":"None","filterValue":"","page":1,"skip":0,"take":100})}, 
                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }                      
                    },
                    parameterMap: function (options, operation) { return options; 
                       
                            
                    }
                    
                },
                schema: {
                    data: "Data",                    
                    total: "Count",
                    model: { 
                        id: "ElementControl",
                        fields: {
                            ElementControl: { type: "number" },
                            ElementName: { type: "string" }, 
                            ElementDescription: { type: "string" }
                        }
                    }, 
                    errors: "Errors"
                },

                error: function (xhr, textStatus, error) { ngl.showErrMsg("Get Static List JSON Response Error",formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"), null);  this.cancelChanges(); },
                serverPaging: false,
                serverSorting: false,
                serverFiltering: false
            });      
       
            $("#listBox").kendoListBox({  
                dataSource: dataSource, 
                dataTextField:"ElementName",  
                dataValueField:"ElementControl",
                autoBind:false,
                change: function(e) {
                    var element = e.sender.select();
                    var dataItem = e.sender.dataItem(element[0])
                    $('#txtDSEElementControl').val(dataItem.ElementControl);
                    $('#txtElementName').val(dataItem.ElementName);
                    
                    //Added by SRP  on 07/03/2018
                    var doctype = $('#ddlDocType').data("kendoDropDownList");
                    var segmenttype = $('#ddlSegmentType').data("kendoDropDownList");
                    if( !doctype.value() || !segmenttype.value() ){
                        blnErrorShown = true;
                        ngl.showErrMsg("please select both DocType and SegmentType",'Hit Me To Hide Error',null);
                    }else{
                        openDocSegmentElementAddWindow();
                    }
                   
                }
            });  
            dataSource.read(); 
  
            //var num=2;
            $("#ddlDocType").kendoDropDownList({
                dataTextField: "EDITName",
                dataValueField: "EDITControl",
                autoWidth: true,
                optionLabel:"Select DocType",
                filter: "contains",
                dataSource: dsDocTypeDropDown,
                dataBound: function(e) {
                    // get the index of the UnitsInStock cell
                    var columns = e.sender.columns;
                    var columnIndex = this.wrapper.find(".k-grid-header [data-field=" + "UnitsInStock" + "]").index();
                    
                    // iterate the data items and apply row styles where necessary
                    var dataItems = e.sender.dataSource.view();
                    //setting index for selected doctype
                    for (var j = 0; j < dataItems.length; j++) {
                        var discontinued = dataItems[j].get("EDITControl");
                        if (discontinued==doctype) {
                            $("#ddlDocType").data('kendoDropDownList').select(discontinued);
                            var dropdownlist = $("#ddlDocType").data("kendoDropDownList");
                            dropdownlist.enable(false);
                            //$("#ddlDocType option:contains("+discontinued+")").attr("readonly","readonly");
                            //$("#ddlDocType option:contains("+discontinued+")").attr("disabled","disabled");
                        }
                    }
                },
                //index:num,
                change: function (e) {
                    var fee = this.dataItem(e.item);
                    $('#hdndoctype').val(fee.EDITControl);
                    //var element = e.sender.select();
                    //var dataItem = e.sender.dataItem(element[0])
                    //$('#txtDSEElementControl').val(dataItem.ElementControl);
                    //$('#txtElementName').val(dataItem.ElementName);
                    
                    //Added by SRP  on 07/03/2018
                    //var doctype = $('#ddlDocType').data("kendoDropDownList");
                    var segmenttype = $('#ddlSegmentType').data("kendoDropDownList");
                    if(!segmenttype.value() ){
                        $("#comment").css("display", "block");
                        //blnErrorShown = true;
                        //ngl.showErrMsg("please select SegmentType",'Hit Me To Hide Error',null);
                    }else{
                        $("#comment").css("display", " none");
                        //openDocSegmentElementAddWindow();
                        doctype=fee.EDITControl;
                        var dataItemGT = $("#ddlSegmentType").data("kendoDropDownList").dataItem();
                        segment = dataItemGT.SegmentControl;
                        //openDocSegmentElementAddWindow();
                        $.ajax({ 
                            url: '/api/EDIDocSegmentElement/GetRecords/', 
                            contentType: 'application/json; charset=utf-8', 
                            dataType: 'json', 
                            data: { filter: JSON.stringify(s),doctype:doctype,segment:segment }, 
                            headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                            success: function(data) {
                                
                                //options.success(data);
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
                                                    refreshDocSegmentElementGrid();
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
                    }
                }
            });
            
            $("#ddlSegmentType").kendoDropDownList({
                dataTextField: "SegmentName",
                dataValueField: "SegmentControl",
                autoWidth: true,
                optionLabel:"Select SegmentType",
                filter: "contains",
                dataSource: dsSegmentDropDown,
                dataBound: function(e) {
                    // get the index of the UnitsInStock cell
                    var columns = e.sender.columns;
                    var columnIndex = this.wrapper.find(".k-grid-header [data-field=" + "UnitsInStock" + "]").index();
                    
                    // iterate the data items and apply row styles where necessary
                    var dataItems = e.sender.dataSource.view();
                    //setting index for selected segment
                    for (var j = 0; j < dataItems.length; j++) {
                        var discontinuednew = dataItems[j].get("SegmentControl");
                        if (discontinuednew==segment) {
                            $("#ddlSegmentType").data('kendoDropDownList').select(discontinuednew);
                            var dropdownlist = $("#ddlSegmentType").data("kendoDropDownList");
                            dropdownlist.enable(false);
                            //$("#ddlSegmentType option:contains("+discontinuednew+")").attr("disabled","disabled");
                            //$("#ddlSegmentType option:contains("+discontinuednew+")").attr("readonly","true");
                            
                        }
                    }
                },
                change: function (e) {
                    var fee = this.dataItem(e.item);
                    $('#hdnsegment').val(fee.SegmentControl);
                    var fee = this.dataItem(e.item);
                    $('#hdndoctype').val(fee.EDITControl);
                    //var element = e.sender.select();
                    //var dataItem = e.sender.dataItem(element[0])
                    //$('#txtDSEElementControl').val(dataItem.ElementControl);
                    //$('#txtElementName').val(dataItem.ElementName);
                    
                    //Added by SRP  on 07/03/2018
                    var doctype = $('#ddlDocType').data("kendoDropDownList");
                    //var segmenttype = $('#ddlSegmentType').data("kendoDropDownList");
                    if(!doctype.value() ){
                        $("#comment").css("display", "block");
                        //blnErrorShown = true;
                        //ngl.showErrMsg("please select DocType",'Hit Me To Hide Error',null);
                    }else{
                        $("#comment").css("display", " none");
                          segment=fee.SegmentControl;
                        var dataItemLT = $("#ddlDocType").data("kendoDropDownList").dataItem();
                        doctype = dataItemLT.EDITControl;
                        //openDocSegmentElementAddWindow();
                        $.ajax({ 
                            url: '/api/EDIDocSegmentElement/GetRecords/', 
                            contentType: 'application/json; charset=utf-8', 
                            dataType: 'json', 
                            data: { filter: JSON.stringify(s),doctype:doctype,segment:segment }, 
                            headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                            success: function(data) {
                                
                                //options.success(data);
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
                                                    refreshDocSegmentElementGrid();
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
                    }
                }
            });
             
            $("#ddlFormating").kendoDropDownList({
                dataSource: dsFormatingDropDown,
                dataTextField: "Name",
                dataValueField: "Control",
                autoWidth: true,
                optionLabel:"Select FnFormating",
                filter: "contains",
               

            });

          
             

            ////////////Grid///////////////////
            EDIDOCSEGMENTELEMENTT = new kendo.data.DataSource({
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
                        var dataItemLT = $("#ddlDocType").data("kendoDropDownList").dataItem();
                        doctype = dataItemLT.EDITControl;

                        var dataItemGT = $("#ddlSegmentType").data("kendoDropDownList").dataItem();
                        segment = dataItemGT.SegmentControl;
                        $.ajax({ 
                            url: '/api/EDIDocSegmentElement/GetRecords/', 
                            contentType: 'application/json; charset=utf-8', 
                            dataType: 'json', 
                            data: { filter: JSON.stringify(s),doctype:doctype,segment:segment }, 
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
                            url: 'api/EDIDocSegmentElement/PostSave', 
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
                                if (ngl.isFunction(refreshDocSegmentElementGrid)) {
                                    refreshDocSegmentElementGrid();
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
                        url: 'api/EDIDocSegmentElement/DeleteRecord', 
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
                                if (typeof (data) !== '1') {
                                    if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                        blnErrorShown = true;
                                        ngl.showErrMsg("delete EDIDocSegmentElement Failure", data.Errors, null);
                                    }
                                    
                                }
                                
                            } catch (err) {
                                ngl.showErrMsg(err.name, err.description, null);
                            }
                            //refresh the grid
                            if (ngl.isFunction(refreshDocSegmentElementGrid)) {
                                refreshDocSegmentElementGrid();
                            }
                        },
                        error: function (xhr, textStatus, error) {
                            var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure");
                            ngl.showErrMsg("delete EDIDocSegmentElement Failure", sMsg, null); 
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
                            DSEPosition: { type: "number" },
                            DSEDefaultVal: { type: "string" },
                            DSEEDITControl: { type: "number" },
                            DSESegmentControl: { type: "number" },
                            DSEElementControl: { type: "number" },
                            DSEFormattingFnControl: { type: "number" },
                            DSEDisabled: {type:"boolean"},
                            DSEModDate: {type:"date",editable: false },//Updated By SRP on 03/5/2018
                            DSEModUser: {type:"string"}//Updated By SRP on 03/5/2018
                            
                            
                        }
                    },
                    errors: "Errors"
                },
                error: function(xhr, textStatus, error) {
                    ngl.showErrMsg("Access EDIElement Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges();
                }
            });
            
            
            
            $('#DocSegmentElementGrid').kendoGrid({
                dataSource: EDIDOCSEGMENTELEMENTT,
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
                    $("#txtDocSegmentElementSortDirection").val(e.sort.dir);
                    $("#txtDocSegmentElementSortField").val(e.sort.field);
                },
                dataBound: function(e) { 
                    var tObj = this; 
                    if (typeof (DocSegmentElementGridDataBoundCallBack) !== 'undefined' && ngl.isFunction(DocSegmentElementGridDataBoundCallBack)) { DocSegmentElementGridDataBoundCallBack(e,tObj); } 
                },
                resizable: true,
                groupable: true, 
                editable: "inline",
                columns: [
                    { field: "DSEControl", title: "DSEControl", hidden: true },
                    { field: "DSEPosition", title: "DSE Position", hidden: false },
                    { field: "DSEDefaultVal", title: "DSE DefaultVal", hidden: false },
                    { field: "DSEEDITControl", title: "Doc. Type", editor: DocTypeDropDownEditor, hidden: true, template:data=>{ return dsDocTypeDropDown._data[Object.values(dsDocTypeDropDown._data).findIndex(x=> x.EDITControl == data.DSEEDITControl)].EDITName}},  
                    { field: "DSESegmentControl", title: "Segment Type", editor: SegmentTypeDropDownEditor, hidden: true, template:data=>dsSegmentDropDown._data[Object.values(dsSegmentDropDown._data).findIndex(x=> x.SegmentControl == data.DSESegmentControl)].SegmentName},  
                    { field: "DSEElementControl", title: "Element Type", editor: ElementTypeDropDownEditor, hidden: false, template:data=>dataSource._data[Object.values(dataSource._data).findIndex(x=> x.ElementControl == data.DSEElementControl)].ElementName}, 
                    { field: "DSEFormattingFnControl", title: "Formating Function", editor: FormatingDropDownEditor, hidden: false, template:data=>dsFormatingDropDown._data[Object.values(dsFormatingDropDown._data).findIndex(x=> x.Control == data.DSEFormattingFnControl)].Name}, 
                    { field: "DSEDisabled", title: "Active", template: '<input type="checkbox" id="SelectedCB" #= DSEDisabled ? checked="checked" : "" # disabled="disabled" />' , hidden: false },
                    { field: "DSEModDate", title: "Mod Date", template: "#= kendo.toString(kendo.parseDate(DSEModDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') #",hidden: true },//Updated By SRP on 3/5/2018
                    { field: "DSEModUser", title: "Mod User", hidden: true },//Updated By SRP on 3/5/2018
                    //{ command: [{ name: "edit", text:{edit: "", update: "", cancel: ""}},{name: "destroy", text: "" }], title: "Action", width: "100px" }
                    //{ command: [{ name: "edit", text:{edit: "", update: "", cancel: ""}}], title: "Action", width: "100px" }
                    //{command: [{ name: "edit" }, {name: "Delete", imageClass: "k-icon k-i-x", click: function (e) {                        e.preventDefault();
                    //    var dataItem = this.dataItem($(e.target).closest("tr"));
                    //    if (confirm('Do you really want to delete this record?')) {
                    //        var dataSourceNEW = $("#DocSegmentElementGrid").data("kendoGrid").dataSource.view();
                    //        dataSourceNEW.remove(dataItem);
                    //        dataSourceNEW.sync;
                    //    }
                    //}
                    //}], title: "&nbsp;", width: "100px"
                    //}]
                    { command: [{ name: "edit element", text: "", iconClass: "k-icon k-i-pencil", click: openSegmentElementEditWindow},{name: "destroy", text: "" }], title: " ", width: "100px" }
            ]
            });
            
            /////////////Edit Mode Spinner Remove and Not Allow Decimal///////////
            function editNumber(container, options) {
                $('<input data-bind=value:' + options.field + '  onkeypress="return event.charCode >= 48 && event.charCode <= 57" />')
                    .appendTo(container)
                    .kendoNumericTextBox({
                        spinners : false,
                        format  : "{0:n0}",
                    });
            }

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


            //Added By SRP on 3/5/18 DocumentTypeAddExample
            ////////////wndAddEDIDocumentType/////////////////

            kendoWin.height =360;// For Updating Kendowindow height config
            kendoWin.width = 330;// For Updating Kendowindow width config
            
            kendoWinStyle({"padding":"10px 10px 10px 30px"});//For Styling kendo Window
            wndAddDocType = $("#wndAddDocSegmentElement").kendoWindow(kendoWin).data("kendoWindow");

            $("#wndAddDocSegmentElement").data("kendoWindow").wrapper.find(".k-svg-i-save").click(function (e) { SaveEDIDocType(); });

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

