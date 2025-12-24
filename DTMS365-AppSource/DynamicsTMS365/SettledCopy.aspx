<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SettledCopy.aspx.cs" Inherits="DynamicsTMS365.SettledCopy" %>


<!DOCTYPE html>

<html>
    <head >
        <title>DTMS Settled</title>         
        <link href="Content/kendoR32023/classic-opal.css" rel="stylesheet" />               
        <link href="Content/NGL/v-8.5.4.001/common.css" rel="stylesheet" />   
        <style>
            html,
            body {height:100%; margin:0; padding:0;}

            html {font-size: 12px; font-family: Arial, Helvetica, sans-serif; overflow:hidden;}

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
        <script src="Scripts/NGL/v-8.5.4.006/SSOA.js"></script>

      <div id="example" style="height: 100%; width: 100%;  margin-top: 2px;">
             <div id="vertical" style="height: 98%; width: 98%; " >                 
                <div id="menu-pane" style="height: 100%; width: 100%; background-color: white; ">
                    <div id="tab" class="menuBarTab"></div> 
                </div>
                <div id="top-pane">
                  <div id="horizontal" style="height: 100%; width: 100%; ">
                        <div id="left-pane">
                            <div class="pane-content">
                                <div><span>Menu</span></div>
                                <div id="menuTree"></div>                                                               
                            </div>
                        </div>
                        <div id="center-pane">
                            <div class="pane-content">

                                <div class="fast-tab" >
                                    <span id="ExpandFilters" style="display: none;"><a onclick='expandFilters();'><span style="font-size: small;font-weight:bold;" class='k-icon k-i-chevron-down'></span></a></span>
                                    <span id="CollapseFilters" style="display: normal;"><a onclick='collapseFilters();'><span style="font-size: small;font-weight:bold;" class='k-icon k-i-chevron-up'></span></a></span>
                                    <span style="font-size:small; font-weight:bold">Filters</span>&nbsp;&nbsp;<br/>
                                    <div id="divFilters" style="padding: 10px;">
                                        <span>
                                            <label for="ddlFilters">Filter by:</label>
                                            <input id="ddlFilters"/>
                                            <input id="txtFilterVal"/>
                                            <span id="spfilterDates">
                                                <label for="txtFilterVal">From:</label>
                                                <input id="dpFilterFrom"/>
                                                <label for="dpFilterTo">To:</label>
                                                <input id="dpFilterTo"/>
                                            </span>
                                            <span id="spfilterButtons">
                                                <a id="btnFilter"></a>
                                                <a id="btnClearFilter"></a>
                                            </span>
                                        </span>

                                        <input id="txtSortDirection" type="hidden" /> 
                                        <input id="txtSortField" type="hidden" /> 
                                    </div>
                                </div>      
                                 <div id="SettledGrid"></div>                                
                            </div>
                        </div>
                    </div>
                </div>
                <div id="bottom-pane" class="k-block" style="height: 100%; width: 100%;">
                    <div class="pane-content">
                        <% Response.Write(PageFooterHTML); %> 
                    </div>
                </div>
            </div>
          
      
    <%-- HelpWindow needs to go after AuthNotification because it uses the notification object created there --%>
    <% Response.Write(AuthLoginNotificationHTML); %>   
    <% Response.WriteFile("~/Views/HelpWindow.html"); %>      
    <script>
        //start ADAL properties
        var opostLogoutRedirectUri = '<% Response.Write(WebBaseURI); %>';
        var oredirectUri = '<% Response.Write(WebBaseURI); %>' + getCurentFileName(); 
        var oidaClient = '<% Response.Write(idaClientId); %>';  //
        var oAuth2instasnce = 'https://login.microsoftonline.com/';
        var oAuth2tenant = 'common';
        loadAuthContext();
        //NOTE:   validateUser(); must be called in the docuemnt.ready function
        //End ADAL properties

        var PageControl = '<%=PageControl%>'; 
        var control = 0;
 var tObj = this;
         var tPage = this;
        var settledItems = kendo.data.DataSource;     
        
        function expandFilters() {
            $("#divFilters").show();
            $("#ExpandFilters").hide();
            $("#CollapseFilters").show();
        }

        function collapseFilters() {
            $("#divFilters").hide();
            $("#ExpandFilters").show();
            $("#CollapseFilters").hide();
        }

        function execActionClick(btn, proc){
            alert("execActionClick" + btn.id + " " + proc);
        }

        function refreshSettledGrid() {
            $('#SettledGrid').data('kendoGrid').dataSource.read();
        }


        $(document).ready(function () {
            var PageMenuTab = <%=PageMenuTab%>;

            var blnReload = validateUser();
            control = <%=UserControl%>;
            if(blnReload == true && control == 0){
                var uc = localStorage.NGLvar1452;
                document.location = oredirectUri + "?uc=" + uc;
                return;
            }
 
            var PageReadyJS = <%=PageReadyJS%>; 
            menuTreeHighlightPage(); //must be called after PageReadyJS

            if (control != 0){

                $("#txtFilterVal").kendoMaskedTextBox();
                $("#dpFilterFrom").kendoDatePicker();
                $("#dpFilterTo").kendoDatePicker();
                $("#btnFilter").kendoButton({
                    icon: "filter",
                    click: function(e) {

                        var dataItem = $("#ddlFilters").data("kendoDropDownList").dataItem();
                        if(dataItem.value === "BookFinAPPayDate"){
                            var dtFrom = $("#dpFilterFrom").data("kendoDatePicker").value();
                            if (!dtFrom){
                                showErrorNotification("Required Fields", "Filter From date cannot be null");
                                return;
                            }
                        }
                        refreshSettledGrid();
                    }
                });
                $("#btnClearFilter").kendoButton({
                    icon: "filter-clear",
                    click: function(e) {

                        //set the dropdownlist back to no filter
                        var dropdownlist = $("#ddlFilters").data("kendoDropDownList");
                        dropdownlist.select(0);
                        dropdownlist.trigger("change");
                        //clear all the values from the filters
                        $("#txtFilterVal").data("kendoMaskedTextBox").value("");
                        $("#dpFilterFrom").data("kendoDatePicker").value("");
                        $("#dpFilterTo").data("kendoDatePicker").value("");

                        //Hide all the filters
                        $("#txtFilterVal").hide();
                        $("#spfilterDates").hide();
                        $("#spfilterButtons").hide();
                        //Refresh the grid
                        $('#SettledGrid').data('kendoGrid').dataSource.read();
                    }
                });


                //clear filter values
                $("#txtFilterVal").data("kendoMaskedTextBox").value("");
                $("#dpFilterFrom").data("kendoDatePicker").value("");
                $("#dpFilterTo").data("kendoDatePicker").value("");
                //hide all filters
                $("#txtFilterVal").hide();
                $("#spfilterDates").hide();
                $("#spfilterButtons").hide();
            
                var filterData = [
                    { text: "", value: "None" },
                    { text: "Invoice Number", value: "BookFinAPBillNumber" },
                    { text: "Paid Date", value: "BookFinAPPayDate" },
                    { text: "Check Number", value: "BookFinAPCheck" },
                    { text: "Pro Number", value: "BookProNumber" },
                    { text: "CNS Number", value: "BookConsPrefix" },
                    { text: "SHID", value: "BookSHID" },
                    { text: "Carrier Pro", value: "BookShipCarrierProNumber" }                             
                ];

                $("#ddlFilters").kendoDropDownList({
                    dataTextField: "text",
                    dataValueField: "value",
                    dataSource: filterData,
                    select: function(e) {
                        var name = e.dataItem.text;
                        var val = e.dataItem.value;
                        //clear filter values
                        $("#txtFilterVal").data("kendoMaskedTextBox").value("");
                        $("#dpFilterFrom").data("kendoDatePicker").value("");
                        $("#dpFilterTo").data("kendoDatePicker").value("");

                        //show appropriate filters based on dropdownlist selection
                        switch(val) {
                            case "None":
                                $("#txtFilterVal").hide();
                                $("#spfilterDates").hide(); 
                                $("#spfilterButtons").hide();
                                break;
                            case "BookFinAPPayDate":
                                $("#txtFilterVal").hide();
                                $("#spfilterDates").show();
                                $("#spfilterButtons").show();
                                break;
                            default:
                                $("#txtFilterVal").show();
                                $("#spfilterDates").hide();
                                $("#spfilterButtons").show();
                                break;
                        }                     
                    }
                });
                         
           
                settledItems = new kendo.data.DataSource({ 
                    serverSorting: true,
                    serverPaging: true,
                    pageSize: 10,
                    transport: { 
                        read: function(options) {
                            
                            var s = new AllFilter();
                            s.filterName = $("#ddlFilters").data("kendoDropDownList").value();
                            s.filterValue = $("#txtFilterVal").data("kendoMaskedTextBox").value();
                            s.filterFrom = $("#dpFilterFrom").data("kendoDatePicker").value();
                            s.filterTo = $("#dpFilterTo").data("kendoDatePicker").value();
                            s.sortName = $("#txtSortField").val();
                            s.sortDirection = $("#txtSortDirection").val();
                            s.page = options.data.page;
                            s.skip = options.data.skip;
                            s.take = options.data.take;

                            $.ajax({
                                url: 'api/Settled/GetSettledItems',
                                contentType: 'application/json; charset=utf-8',
                                dataType: 'json',
                                data: { filter: JSON.stringify(s) },
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                success: function(data) {
                                    // notify the data source that the request succeeded
                                    options.success(data);

                                    if (data.Errors != null) {
                                        if (data.StatusCode === 203) {
                                            showErrorNotification("Authorization Timeout", data.Errors);
                                        }
                                        else {
                                            showErrorNotification("Access Denied", data.Errors);
                                        }               
                                    }
                                },
                                error: function(result) {
                                    // notify the data source that the request failed
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
                            id: "Control",
                            fields: {
                                Control: { type: "number" },
                                InvoiceNumber: { type: "string" },
                                ContractedCost: { type: "number" },
                                PaidCost: { type: "number" },
                                InvoiceAmount: { type: "number" },
                                PaidDate: { type: "date" },
                                CheckNumber: { type: "string" },
                                ProNumber: { type: "string" },
                                CnsNumber: { type: "string" },
                                SHID: { type: "string" },
                                CarrierPro: { type: "string" }                               
                            }
                        }, 
                        errors: "Errors" 
                    }, 
                    error: function (e) { alert(e.errors); this.cancelChanges(); }                   
                });
            
                $('#SettledGrid').kendoGrid({ 
                    height: 500,
                    dataSource: settledItems, 
                    sortable: {
                        mode: "single",
                        allowUnsort: true
                    },
                    sort: function(e) {

                        if (!e.sort.dir){ e.sort.dir == ""; e.sort.field == ""; }
                        if (!e.sort.field){ e.sort.field == ""; }

                        $("#txtSortDirection").val(e.sort.dir);
                        $("#txtSortField").val(e.sort.field);
                    },
                    pageable: true, 
                    resizable: true, 
                    groupable: false, 
                    columns: [
                        {field: "Control", title: "Control", hidden: true},
                        {field: "InvoiceNumber", title: "Invoice Number"},
                        {field: "ContractedCost", title: "Contracted Cost"},
                        {field: "PaidCost", title: "Paid Cost"},
                        {field: "InvoiceAmount", title: "Invoice Amount"},
                        {field: "PaidDate", title: "Paid Date", template: "#= kendo.toString(kendo.parseDate(PaidDate, 'yyyy-MM-dd'), 'MM/dd/yyyy') #"},
                        {field: "CheckNumber", title: "Check Number"},
                        {field: "ProNumber", title: "PRO Number"},
                        {field: "CnsNumber", title: "CNS Pool Number"},                       
                        {field: "SHID", title: "SHID"},
                        {field: "CarrierPro", title: "CarrierPro"},
                        {field: "BookFinAPActWgt", title: "Load Shipped Wgt"},
                        {field: "BookCarrBLNumber", title: "BOL Number"}
                    ]
                });
          

            }
        });


    </script>
    <style>

    </style>
    </div>


    </body>

</html>
