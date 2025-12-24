<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SampleParameters.aspx.cs" Inherits="DynamicsTMS365.SampleParameters" %>

<!DOCTYPE html>

<html>
    <head >
        <title>DTMS Sample Parameters</title>           
        <link href="Content/kendoR32023/classic-opal.css" rel="stylesheet" />               
        <link href="Content/NGL/v-8.5.4.001/common.css" rel="stylesheet" />                 
        <style>

html,

body
{height:100%; margin:0; padding:0;}

html
{font-size: 12px; font-family: Arial, Helvetica, sans-serif; overflow:hidden;}

</style>
 
    </head>
    <body>       
       <script src="https://code.jquery.com/jquery-3.4.1.min.js"></script>  
        <script src="Scripts/kendoR32023/kendo.all.min.js"></script>
<script>kendo.ui['Button'].fn.options['size'] = "small";</script>
        <script src="https://secure.aadcdn.microsoftonline-p.com/lib/1.0.14/js/adal.min.js"></script>
        <script src="Scripts/NGL/v-8.5.4.006/core.js"></script>
        <script src="Scripts/NGL/v-8.5.4.006/NGLobjects.js"></script>
        <script src="Scripts/NGL/v-8.5.4.006/SSOA.js"></script> 

     <div id="example" style="height: 100%; width: 100%;  margin-top: 2px;">
             <div >
             <% Response.Write(PageErrorsOrWarnings); %>
                    <br /><br />
                    <div class="ngl-blueBorderFullPage" style="min-width: 450px;">  
                        <div style="margin: 10px;">                    
                            
                            <span style="margin:6px; vertical-align: middle;" >
                                <a id="aLogoURL" href="<% Response.Write(HomeTabHrefURL); %>"><img id="imgLogo" border="0" alt="Public Web" src="<% Response.Write(HomeTabLogo); %>" ></a>
                            </span>     
                       <%--     <h3>Dynamics TMS 365 and NGL NEXTrack&trade; Collaboration Portal Login Options</h3>
                            <h4 id="WelcomeMessage">If you need an account just fill out the form below.  One of our support representatives will contact you shortly.  Please note that all fields are required.</h4>                         --%>

                            <hr />   
                                                                                     
                            <div id="id260172201712120729428876610">
                                <div class="fast-tab">
                                    <span id="ExpandParSpan" style="display:none;"><a onclick="expandFastTab('ExpandParSpan','CollapseParSpan','ParHeader','ParDetail');"><span style="font-size:small;font-weight:bold;" class="k-icon k-i-chevron-down"></span></a></span>
                                    <span id="CollapseParSpan" style="display:normal;"><a onclick="collapseFastTab('ExpandParSpan','CollapseParSpan','ParHeader',null);"><span style="font-size:small;font-weight:bold;" class="k-icon k-i-chevron-up"></span></a></span> 
                                    <span style="font-size:small;font-weight:bold"> Parameters</span>
                                </div>
                                <div id="ParHeader" class="OpenOrders">
                                    <div id="Parwrapper">
                                        <div id="ParFilterFastTab">
                                            <span id="ExpandParFilterFastTabSpan" style="display:none;"><a onclick="expandFastTab('ExpandParFilterFastTabSpan','CollapseParFilterFastTabSpan','ParFilterFastTabHeader',null);"><span style="font-size:small;font-weight:bold;" class="k-icon k-i-chevron-down"></span></a></span>
                                            <span id="CollapseParFilterFastTabSpan" style="display:normal;"><a onclick="collapseFastTab('ExpandParFilterFastTabSpan','CollapseParFilterFastTabSpan','ParFilterFastTabHeader',null);"><span style="font-size:small;font-weight:bold;" class="k-icon k-i-chevron-up"></span></a></span>
                                            <span style="font-size:small;font-weight:bold"> Filters </span>
                                            <div id="ParFilterFastTabHeader" style="padding: 10px;">
                                                <span>
                                                    <label for="ddlParFilters"> Filter by:</ label>
                                                    <input id="ddlParFilters" />
                                                    <span id="spParfilterText"><input id="txtParFilterVal" /></span>
                                                    <span id="spParfilterDates" >
                                                        <label for="dpParFilterFrom" > From:</ label>
                                                        <input id="dpParFilterFrom" />
                                                        <label for="dpParFilterTo" > To:</ label>
                                                        <input id="dpParFilterTo" />
                                                    </span>
                                                    <span id="spParfilterButtons" ><a id="btnParFilter" ></a><a id="btnParClearFilter" ></a></span>
                                                </span>                                                
                                                <input id="txtParSortDirection" type="hidden" />
                                                <input id="txtParSortField" type="hidden" />
                                            </div>
                                        </div>
                                        <div id="ParGrid"></div>
                                    </div>
                                </div>
                            </div>  

                           
                        </div>   
                        <div style="position:relative; clear:both; float:none; display:inline-block; margin:10px;" id="bottom-pane"  >
                            <hr />
                            <div style="margin:5px,5px,5px,5px; padding:5px,5px,5px,5px; border:solid  #7bd2f6 2px; background-color: #7bd2f6; border-radius: 10px;" >
                                <% Response.Write(PageFooterHTML); %> 
                            </div>
                            <br />
                        </div>                       
                    </div>                              
        </div>

    
     <% Response.Write(AuthLoginNotificationHTML); %>      
    <script>
       
        
        var PageControl = '<%=PageControl%>';            
        var tObj = this;
        var tPage = this;
       
        $(document).ready(function () {
            control = <%=UserControl%>;

            var ParfilterData = [ 
                { text: "", value: "None" },
                { text: "ParKey", value: "ParKey" },
                { text: "Text", value: "ParText" },
                //{ text: "ParDescription", value: "ParDescription" },
                //{ text: "ParCategoryControl", value: "ParCategoryControl" },
                //{ text: "Is Global", value: "ParIsGlobal" },
                { text: "ParValue", value: "ParValue" },
                //{ text: "rowguid", value: "rowguid" },
                //{ text: "ParOLE", value: "ParOLE" },
                //{ text: "ParUpdated", value: "ParUpdated" },
                //{ text: "msrepl_tran_version", value: "msrepl_tran_version" } 
            ];
            
            $("#ddlParFilters").kendoDropDownList({
                dataTextField: "text",
                dataValueField: "value",
                dataSource: ParfilterData,
                select: function(e) {
                    var name = e.dataItem.text; 
                    var val = e.dataItem.value; 
                    $("#txtParFilterVal").data("kendoMaskedTextBox").value("");
                    $("#dpParFilterFrom").data("kendoDatePicker").value("");
                    $("#dpParFilterTo").data("kendoDatePicker").value("");
                    switch(val){
                        case "None":
                            $("#spParfilterText").hide();
                            $("#spParfilterDates").hide();
                            $("#spParfilterButtons").hide(); 
                            break; 
                        case "NoDatesAvailable":
                            $("#spParfilterText").hide();
                            $("#spParfilterDates").show();
                            $("#spParfilterButtons").show();
                            break;
                        default:
                            $("#spParfilterText").show();
                            $("#spParfilterDates").hide();
                            $("#spParfilterButtons").show();
                            break;
                    }
                }
            });
            
            $("#txtParFilterVal").kendoMaskedTextBox(); 
            $("#dpParFilterFrom").kendoDatePicker(); 
            $("#dpParFilterTo").kendoDatePicker(); 
            $("#btnParFilter").kendoButton({
                icon: "filter",
                click: function(e) { 
                    var dataItem = $("#ddlParFilters").data("kendoDropDownList").dataItem(); 
                    if (1 === 0){ 
                        var dtFrom = $("#dpParFilterFrom").data("kendoDatePicker").value(); 
                        if (!dtFrom) { ngl.showErrMsg("Required Fields", "Filter From date cannot be null", null); return;}
                    } 
                    $("#ParGrid").data("kendoGrid").dataSource.read();
                }
            }); 
            
            $("#btnParClearFilter").kendoButton({
                icon: "filter-clear",
                click: function(e) {
                    var dropdownlist = $("#ddlParFilters").data("kendoDropDownList"); 
                    dropdownlist.select(0);
                    dropdownlist.trigger("change");
                    $("#txtParFilterVal").data("kendoMaskedTextBox").value("");
                    $("#dpParFilterFrom").data("kendoDatePicker").value(""); 
                    $("#dpParFilterTo").data("kendoDatePicker").value(""); 
                    $("#spParfilterText").hide(); 
                    $("#spParfilterDates").hide(); 
                    $("#spParfilterButtons").hide();
                    $("#ParGrid").data("kendoGrid").dataSource.read();
                }
            }); 
            
            $("#txtParFilterVal").data("kendoMaskedTextBox").value("");
            $("#dpParFilterFrom").data("kendoDatePicker").value("");
            $("#dpParFilterTo").data("kendoDatePicker").value("");
            $("#spParfilterText").hide();
            $("#spParfilterDates").hide();
            $("#spParfilterButtons").hide();

            Parameter = new kendo.data.DataSource({
                serverSorting: true, 
                serverPaging: true, 
                pageSize: 10,
                transport: { 
                    read: function(options) { 
                        var s = new AllFilter();
                        s.filterName = $("#ddlParFilters").data("kendoDropDownList").value();
                        s.filterValue = $("#txtParFilterVal").data("kendoMaskedTextBox").value();
                        s.filterFrom = $("#dpParFilterFrom").data("kendoDatePicker").value();
                        s.filterTo = $("#dpParFilterTo").data("kendoDatePicker").value();
                        s.sortName = $("#txtParSortField").val();s.sortDirection = $("#txtParSortDirection").val();
                        s.page = options.data.page;
                        s.skip = options.data.skip;s.take = options.data.take;

                        $.ajax({ 
                            url: 'api/Parameter/GetRecords/' + s, 
                            contentType: 'application/json; charset=utf-8', 
                            dataType: 'json', 
                            data: { filter: JSON.stringify(s) }, 
                            headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                            success: function(data) { 
                                options.success(data); 
                                if (data.Errors != null) { 
                                    if (data.StatusCode === 203){ 
                                        ngl.showErrMsg("Authorization Timeout", data.Errors, null); 
                                    } 
                                    else 
                                    { 
                                        ngl.showErrMsg("Access Denied", data.Errors, null); 
                                    } 
                                } 
                            }, 
                            error: function(result) { 
                                options.error(result); 
                            } 
                        }); 
                    },           
                    parameterMap: function(options, operation) { return options; } 
                },  
                schema: {
                    data: "Data",
                    total: "Count",
                    model: {
                        id: "ParKey",
                        fields: {
                            ParKey: { type: "string" },
                            ParText: { type: "string" },
                            ParDescription: { type: "string" },
                            ParCategoryControl: { type: "number" },
                            ParIsGlobal: { type: "bool" },
                            ParValue: { type: "number" },
                            rowguid: { type: "string" },
                            ParOLE: { type: "string" },
                            ParUpdated: { type: "string" },
                            msrepl_tran_version: { type: "string" }
                        }
                    },
                    errors: "Errors"
                },
                error: function(xhr, textStatus, error) {
                    ngl.showErrMsg("Access Parameter Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges();
                }
            });


            $('#ParGrid').kendoGrid({
                dataSource: Parameter,
                pageable: true,
                sortable: {
                    mode: "single",
                    allowUnsort: true
                },
                sort: function(e) {
                    if (!e.sort.dir) { e.sort.dir == ""; e.sort.field == ""; }
                    if (!e.sort.field) { e.sort.field == ""; }
                    $("#txtParSortDirection").val(e.sort.dir);
                    $("#txtParSortField").val(e.sort.field);
                },
                dataBound: function(e) { 
                    var tObj = this; 
                    if (typeof (ParGridDataBoundCallBack) !== 'undefined' && ngl.isFunction(ParGridDataBoundCallBack)) { ParGridDataBoundCallBack(e,tObj); } 
                },
                resizable: true,
                groupable: true, 
                editable: "inline",
                columns: [
                    {field: "ParKey", title: "ParKey"},
                    {field: "ParText", title: "Text"},
                    {field: "ParDescription", title: "ParDescription", hidden: true },
                    {field: "ParCategoryControl", title: "ParCategoryControl", hidden: true },
                    {field: "ParIsGlobal", title: "Is Global", template: '<input type="checkbox" #= ParIsGlobal ? "checked=checked" : "" # disabled="disabled" ></input>' },
                    {field: "ParValue", title: "ParValue"},
                    {field: "rowguid", title: "rowguid", hidden: true },
                    {field: "ParOLE", title: "ParOLE", hidden: true },
                    {field: "ParUpdated", title: "ParUpdated", hidden: true },
                    {field: "msrepl_tran_version", title: "msrepl_tran_version", hidden: true },
                    { command: [{ name: "edit", text:{edit: "", update: "", cancel: ""}},{name: "destroy", text: "" }], title: "Actions", width: "100px" } 
                ]
            });



           
        });


    </script>

         <style>
             
             .k-grid tbody .k-button {
                 min-width: 18px;
                 width: 28px;
             }

       </style>
    
    </div>


    </body>

</html>

