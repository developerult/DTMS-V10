<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EDIDocumentType.aspx.cs" Inherits="DynamicsTMS365.EDIDocumentType" %>

<!DOCTYPE html>

<html>
    <head >
        <title>Manage EDI Document Types</title>           
        <link href="Content/kendoR32023/classic-opal.css" rel="stylesheet" />               
        <link href="Content/NGL/v-8.5.4.001/common.css" rel="stylesheet" />                 
        <style>

html,

body
{height:100%; margin:0; padding:0;}

html
{font-size: 12px; font-family: Arial, Helvetica, sans-serif; overflow:hidden;}
.hide-display{
    display:none;
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
        .ui-id260-container {
            margin-top: 10px; 
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
        <script src="Scripts/NGL/v-8.5.4.006/menuitems.js"></script><!-- added by SN on 14/02/2018 For Dynamically adding menu items for all pages -->
        <script src="Scripts/NGL/v-8.5.4.006/windowconfig.js"></script><!--added by SN on 14/02/2018 For Editing KendoWindow Configuration from Javascript -->

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
                              <div><span>EDI - MAINTENANCE</span></div><%--Page Navigation Menu Tree--%>
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
                                  <span id="ExpandParSpan" style="display:none;"><a onclick="expandFastTab('ExpandParSpan','CollapseParSpan','ParHeader','ParDetail');"><span class="k-icon k-i-chevron-down ui-span-container"></span></a></span>
                                  <span id="CollapseParSpan" style="display:normal;"><a onclick="collapseFastTab('ExpandParSpan','CollapseParSpan','ParHeader',null);"><span class="k-icon k-i-chevron-up ui-span-container"></span></a></span> 
                                  <span class="ui-span-container">EDI Document Types</span>
                              </div>
                              <div id="ParHeader" class="OpenOrders">
                                  <div id="Parwrapper">
                                      <%--Filters--%>
                                      <div id="ParFilterFastTab">
                                          <span id="ExpandParFilterFastTabSpan" style="display:none;"><a onclick="expandFastTab('ExpandParFilterFastTabSpan','CollapseParFilterFastTabSpan','ParFilterFastTabHeader',null);"><span class="k-icon k-i-chevron-down ui-span-container"></span></a></span>
                                          <span id="CollapseParFilterFastTabSpan" style="display:normal;"><a onclick="collapseFastTab('ExpandParFilterFastTabSpan','CollapseParFilterFastTabSpan','ParFilterFastTabHeader',null);"><span class="k-icon k-i-chevron-up ui-span-container"></span></a></span>
                                          <span class="ui-span-container"> Filters </span>
                                          <div id="ParFilterFastTabHeader" class="ui-padding-container">
                                              <span>
                                                  <label for="ddlEDITypeFilters"> Filter by:</ label>
                                                  <input id="ddlEDITypeFilters" />
                                                  <span id="spEDITypeFilterText"><input id="txtEDITypeFilterVal" /></span>
                                                  <span id="spEDITypeFilterDates" >
                                                      <label for="dpEDITypeFilterFrom" > From:</ label>
                                                      <input id="dpEDITypeFilterFrom" />
                                                      <label for="dpEDITypeFilterTo" > To:</ label>
                                                      <input id="dpEDITypeFilterTo" />
                                                  </span>
                                                  <span id="spEDITypeFilterButtons" ><a id="btnEDITypeFilter" ></a><a id="btnEDITypeClearFilter" ></a></span>
                                              </span>
                                              <input id="txtEDITypeSortDirection" type="hidden" />
                                              <input id="txtEDITypeSortField" type="hidden" />
                                          </div>
                                      </div>
                                      <%--Grid--%>
                                      <div id="EDITypeGrid"></div>
                                  </div>
                              </div>
                          </div> 
                          
                          <!-- End Page Content -->
                      </div>
                  </div>
              </div>
              <div id="bottom-pane" class="k-block ui-horizontal-container-document">
                  <div class="pane-content">
                      <div><span><p>If you experience problems with this site, call (847)963-0007 24/7 or email our support group at <a href='mailto: support@nextgeneration.com'>support@nextgeneration.com</a></p></span></div> 
                  </div>
              </div>
          </div>

          <%--Popup Window HTML--%>
          <div id="wndMessage">
              <div>
                  <h2>Enter a Message</h2>
                  <input id="txtUserInput" style="width:250px;"/>
              </div>
          </div>

          <%--Added By SN on 2/14/18 EDIDocumentType for KendoWindow--%>
          <% Response.WriteFile("~/Views/DocumentTypeAddWindow.html"); %>
    
     <% Response.Write(AuthLoginNotificationHTML); %>   
    <script>
        //************* Page Variables **************************
        var PageControl = '<%=PageControl%>';       
        var tObj = this;
        var tPage = this;
        var oKendoGrid = null;
        var wndMessage = kendo.ui.Window;
        var wndAddDocType = kendo.ui.Window; //Added By SN on 2/14/18 EDIDocumentType


        //*************  Call Back Functions  *******************
        function EDITypeGridDataBoundCallBack(e,tGrid){           
            oKendoGrid = tGrid;
            //add databound code here
        }


        //*************  Action Functions  **********************
        function execActionClick(btn, proc){
         
            if(btn.id == "btnAddDocumentType"){ //Added By SN on 2/12/18 EDITypeAddExample
                openDocumentTypeAddWindow();
            }
        }

        //Added By SN on 2/14/18 EDITypeAddExample
        function openDocumentTypeAddWindow() {

            //Validation Display
            var valcheck = $("#name-validation").hasClass("hide-display");

            if (valcheck == false) {
                $("#name-validation").addClass("hide-display");
            }
            //This is how you can change the title of the window (in case you want to share the same window for Add and Edit- Note: In grids editing does not have to always be inline)
            $("#wndAddDocumentType").data("kendoWindow").title("Add EDI Document Type");

            //Clear all previous values since this is Add New
            $("#txtDocumentTypeControl").val(0);
            $("#txtDocTypeName").data("kendoMaskedTextBox").value("");
            //$("#txtDocTypeCode").data("kendoMaskedTextBox").value("");
            $("#txtDocTypeDescription").data("kendoMaskedTextBox").value("");


            wndAddDocType.center().open();
        }
       
        //Added By SN on 2/14/18 EDITypeAddExample
        function SaveEDIDocType() {
            var otmp = $("#focusCancel").focus();

            //Created by SN on 02/19/2018 for Input Validation
            
            var submit = true;
            var tName = $("#txtDocTypeName").val();
            
            if (tName == "") {
                
                $("#name-validation").removeClass("hide-display");
                submit = false;
            }
            else {
                submit = true;
                $("#name-validation").addClass("hide-display");
            }
            var item = new NGLDocType();

            item.EDITControl = $("#txtDocumentTypeControl").val();

            item.EDITName = $("#txtDocTypeName").data("kendoMaskedTextBox").value();
            
            item.EDITDescription = $("#txtDocTypeDescription").data("kendoMaskedTextBox").value();
            
       
            if (submit == true) {

           
            $.ajax({
                async: false,
                type: "POST",
                url: "api/EDIDocumentType/SaveEDIDocument",
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
                                ngl.showErrMsg("Save EDIDocumentType Failure", data.Errors, null);
                            }
                            else {
                                if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                    if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                        blnSuccess = true;
                                        refreshEDITypeGrid();
                                    }
                                }
                            }
                        }
                        if (blnSuccess === false && blnErrorShown === false) {
                            if (strValidationMsg.length < 1) { strValidationMsg = "Save EDIDocumentType Failure"; }
                            ngl.showErrMsg("Save EDIDocumentType Failure", strValidationMsg, null);
                        }
                    } catch (err) {
                        ngl.showErrMsg(err.name, err.description, null);
                    }
                },
                error: function (xhr, textStatus, error) {
                    var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure");
                    ngl.showErrMsg("Save EDIDocumentType Failure", sMsg, null);                        
                }
                });
            wndAddDocType.close();         
            }

             
        }
        $("#txtDocTypeName").on("change input", function () {
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


        function refreshEDITypeGrid() {
            //oKendoGrid gets set during EDITypeGridDataBoundCallBack()
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
                        //Modified By SN on 2/12/18 EDITypeAddExample
                        content: "<button id='btnAddDocumentType' class='k-button actionBarButton' type='button'><span class='k-icon k-i-add'></span>Add EDI Document Type</button>"
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
            var l = "<h3>Manage EDI Document Types </h3>";
            $("#txtScreenMessage").html(l);

            //define Kendo widgets
            $("#txtUserInput").kendoMaskedTextBox();
            $("#txtDocTypeName").kendoMaskedTextBox();    //Added By SN on 2/12/18 EDITypeAddExample
            $("#txtDocTypeCode").kendoMaskedTextBox();    //Added By SN on 2/12/18 EDITypeAddExample
            $("#txtDocTypeDescription").kendoMaskedTextBox();    //Added By SN on 2/12/18 EDITypeAddExample

            ////////////Filters///////////////////
            var EDITypeFilterData = [ 
               { text: "", value: "None" },
               { text: "Doc Type Name", value: "EDITName" },
               { text: "Doc Type Description", value: "EDITDescription" },
            ];
            
            $("#ddlEDITypeFilters").kendoDropDownList({
                dataTextField: "text",
                dataValueField: "value",
                dataSource: EDITypeFilterData,
                select: function(e) {
                    var name = e.dataItem.text; 
                    var val = e.dataItem.value; 
                    $("#txtEDITypeFilterVal").data("kendoMaskedTextBox").value("");
                    $("#dpEDITypeFilterFrom").data("kendoDatePicker").value("");
                    $("#dpEDITypeFilterTo").data("kendoDatePicker").value("");
                    switch(val){
                        case "None":
                            $("#spEDITypeFilterText").hide();
                            $("#spEDITypeFilterDates").hide();
                            $("#spEDITypeFilterButtons").hide(); 
                            break; 
                        case "NoDatesAvailable":
                            $("#spEDITypeFilterText").hide();
                            $("#spEDITypeFilterDates").show();
                            $("#spEDITypeFilterButtons").show();
                            break;
                        default:
                            $("#spEDITypeFilterText").show();
                            $("#spEDITypeFilterDates").hide();
                            $("#spEDITypeFilterButtons").show();
                            break;
                    }
                }
            });
            
            $("#txtEDITypeFilterVal").kendoMaskedTextBox(); 
            $("#dpEDITypeFilterFrom").kendoDatePicker(); 
            $("#dpEDITypeFilterTo").kendoDatePicker(); 
            $("#btnEDITypeFilter").kendoButton({
                icon: "filter",
                click: function(e) { 
                    var dataItem = $("#ddlEDITypeFilters").data("kendoDropDownList").dataItem(); 
                    
                    if (1 === 0){ 
                        var dtFrom = $("#dpEDITypeFilterFrom").data("kendoDatePicker").value(); 
                        if (!dtFrom) { ngl.showErrMsg("Required Fields", "Filter From date cannot be null", null); return;}
                    } 
                    $("#EDITypeGrid").data("kendoGrid").dataSource.read();
                }
            }); 
            
            $("#btnEDITypeClearFilter").kendoButton({
                icon: "filter-clear",
                click: function(e) {
                    var dropdownlist = $("#ddlEDITypeFilters").data("kendoDropDownList"); 
                    dropdownlist.select(0);
                    dropdownlist.trigger("change");
                    $("#txtEDITypeFilterVal").data("kendoMaskedTextBox").value("");
                    $("#dpEDITypeFilterFrom").data("kendoDatePicker").value(""); 
                    $("#dpEDITypeFilterTo").data("kendoDatePicker").value(""); 
                    $("#spEDITypeFilterText").hide(); 
                    $("#spEDITypeFilterDates").hide(); 
                    $("#spEDITypeFilterButtons").hide();
                    $("#EDITypeGrid").data("kendoGrid").dataSource.read();
                }
            }); 
            
            $("#txtEDITypeFilterVal").data("kendoMaskedTextBox").value("");
            $("#dpEDITypeFilterFrom").data("kendoDatePicker").value("");
            $("#dpEDITypeFilterTo").data("kendoDatePicker").value("");
            $("#spEDITypeFilterText").hide();
            $("#spEDITypeFilterDates").hide();
            $("#spEDITypeFilterButtons").hide();

             
            ////////////Grid///////////////////
            EDIType = new kendo.data.DataSource({
                serverSorting: true, 
                serverPaging: true, 
                pageSize: 10,
                transport: { 
                    read: function(options) { 
                        var s = new AllFilter();

                        s.filterName = $("#ddlEDITypeFilters").data("kendoDropDownList").value();
                        s.filterValue = $("#txtEDITypeFilterVal").data("kendoMaskedTextBox").value();

                        s.page = options.data.page;
                        s.skip = options.data.skip;
                        s.take = options.data.take;

                        $.ajax({ 
                            url: '/api/EDIDocumentType/GetRecords/', 
                            contentType: 'application/json; charset=utf-8', 
                            dataType: 'json', 
                            data: { filter: JSON.stringify(s) }, 
                            headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                            success: function(data) {
                                
                                options.success(data);
                                // Updated by SN on 02/21/2018
                                try {                                    
                                    var blnSuccess = false;
                                    var blnErrorShown = false;
                                    var strValidationMsg = "";
                                    if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                                        if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                            blnErrorShown = true;
                                            ngl.showErrMsg("Get EDIDocumentType Failure", data.Errors, null);
                                        }
                                        else {
                                            if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                                    blnSuccess = true;
                                                }
                                                else{
                                                    blnSuccess = true;
                                                    strValidationMsg = "No records were found matching your search criteria";
                                                }
                                            }
                                        }
                                    }
                                    if (blnSuccess === false && blnErrorShown === false) {
                                        if (strValidationMsg.length < 1) { strValidationMsg = "Get EDIDocumentType Failure"; }
                                        ngl.showErrMsg("Get EDIDocumentType Failure", strValidationMsg, null);
                                    }
                                } catch (err) {
                                    ngl.showErrMsg(err.name, err.description, null);
                                }
                               
                            }, 
                            error: function(result) { 
                                options.error(result);
                                
                            } 
                        }); 
                    },      
                    create: function(options) {
                        $.ajax({
                            async: false,
                            type: "POST",
                            url: "api/EDIDocumentType/SaveEDIDocument",
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
                                        
                                            if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                                if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                                    blnSuccess = true;
                                                    refreshEDITypeGrid();
                                                }
                                            }
                                       
                                    }
                                    if (blnSuccess === false && blnErrorShown === false) {
                                        if (strValidationMsg.length < 1) { strValidationMsg = "Save EDIDocumentType Failure"; }
                                        ngl.showErrMsg("Save EDIDocumentType Failure", strValidationMsg, null);
                                    }
                                } catch (err) {
                                    ngl.showErrMsg(err.name, err.description, null);
                                }
                            },
                            error: function (xhr, textStatus, error) {
                                var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure");
                                ngl.showErrMsg("Save EDIDocumentType Failure", sMsg, null);                        
                            }
                        });
                    },
                    update: function(options) {
                        $.ajax({ 
                            url: 'api/EDIDocumentType/PostSave', 
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
                                            ngl.showErrMsg("Save EDIDocumentType Failure", data.Errors, null);
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
                                        if (strValidationMsg.length < 1) { strValidationMsg = "Save EDIDocumentType Failure"; }
                                        ngl.showErrMsg("Save EDIDocumentType Failure", strValidationMsg, null);
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
                                var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure");
                                ngl.showErrMsg("Save EDIType Failure", sMsg, null); 
                            } 
                        });
                    },
                    destroy: function(options) {
                        $.ajax({
                            url: 'api/EDIDocumentType/DeleteRecord', 
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
                                        ngl.showWarningMsg("Documents already exist with this document type, it cannot be deleted!", "", null);
                                        
                                    }
                                    if(typeof (data) !== 'undefined' && ngl.isObject(data)){
                                        
                                        if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                                            blnErrorShown = true;
                                            ngl.showWarningMsg("Documents already exist with this document type, it cannot be deleted!", data.Errors, null);
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
                                var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "delete EDIDocumentType Failure");
                                ngl.showErrMsg("delete EDIDocumentType Failure", sMsg, null); 
                            } 
                        });
                    },
                    parameterMap: function(options, operation) { return options; } 
                },  
                schema: {
                    data: "Data",
                    total: "Count",
                    model: {
                        id: "EDITControl",
                        fields: {
                            EDITControl: { type: "number" },
                            EDITName: { type: "string",
                                validation: {
                                    required: {
                                        message: "Document Type name is required."
                                    }
                                    
                                }
                            },
                            EDITDescription: { type: "string" },
                            EDITDisabled: {type:"boolean"},
                            EDITModDate: {type:"date",editable: false },//Updated By SN on 02/21/2018
                            EDITModUser: {type:"string"}//Updated By SN on 02/21/2018
                            
                            
                        }
                    },
                    errors: "Errors"
                },
                error: function(xhr, textStatus, error) {
                    ngl.showErrMsg("Access EDIDocumentType Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges();
                }
            });
            var grid = $("#EDITypeGrid").kendoGrid({
                dataSource: EDIType,
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
                    $("#txtEDITypeSortDirection").val(e.sort.dir);
                    $("#txtEDITypeSortField").val(e.sort.field);
                },
                dataBound: function(e) { 
                    var tObj = this; 
                    if (typeof (EDITypeGridDataBoundCallBack) !== 'undefined' && ngl.isFunction(EDITypeGridDataBoundCallBack)) { EDITypeGridDataBoundCallBack(e,tObj); } 
                },
                resizable: true,
                groupable: true, 
                editable: "inline",
                columns: [
                    { field: "EDITControl", title: "EDITControl ", hidden: true },
                    { field: "EDITName", width: 250, title: "Document Type Name", hidden: false },
                    { field: "EDITDescription", width: 300, title: "Document Type Description", hidden: false,template:"#if(EDITDescription.length>150){# # var myContent =EDITDescription; #  # var dcontent = myContent.substring(0,150)+'...'; # <span>#=kendo.toString(dcontent)#</span> #}else{# <span>#=EDITDescription#</span> #}#",class: "breakWord20", width: 200, editor: textareaEditor},                    
                    { field: "EDITDisabled", width: 100, title: "Disabled", template: '<input type="checkbox" id="SelectedCB" #= EDITDisabled ? checked="checked" : "" # disabled="disabled" />' , hidden: false },
                    { field: "EDITModDate", title: "Mod Date", template: "#= kendo.toString(kendo.parseDate(EDITModDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') #",hidden: true },//Updated By SN on 02/21/2018
                    { field: "EDITModUser", title: "Mod User", hidden: true },//Updated By SN on 02/21/2018
                    { command: [{ name: "edit", text:{edit: "", update: "", cancel: ""}},{name: "destroy", text: "", imageClass: "k-i-delete", iconClass: "k-icon"}], title: "Action", width: "100px" }
                ]
            }).data("kendoGrid");
            $( "#btnAddDocumentType").click(function() {
                grid.options.editable = "inline";
                grid.addRow();
                grid.options.editable = "inline";
                
            });
            $("#EDITypeGrid").kendoTooltip({
                filter: ".k-grid-edit",
                content: "Edit Document&nbsp;Type"
            });

            $("#EDITypeGrid").kendoTooltip({
                filter: ".k-grid-delete",
                content: "Delete Document&nbsp;Type"
            });

            function onGridEditing(arg) {
                arg.container.find("input[name='EDITName']").attr('maxlength', '50');
                arg.container.find("input[name='EDITDescription']").attr('maxlength', '255');
            }
            /////////////Edit Mode textarea for description///////////
            function textareaEditor(container, options) {
                $('<textarea data-bind="value: ' + options.field + '" cols="75" rows="3" class="k-textbox"></textarea>')
                    .appendTo(container);
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

            ////////////wndAddEDIDocumentType/////////////////

            kendoWin.height = 250;// For Updating Kendowindow height config
            kendoWin.width = 300;// For Updating Kendowindow width config
            
            kendoWinStyle({"padding":"10px 10px 10px 30px"});//For Styling kendo Window
            wndAddDocType = $("#wndAddDocumentType").kendoWindow(kendoWin).data("kendoWindow");

            $("#wndAddDocumentType").data("kendoWindow").wrapper.find(".k-svg-i-save").click(function (e) { SaveEDIDocType(); });
            

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
