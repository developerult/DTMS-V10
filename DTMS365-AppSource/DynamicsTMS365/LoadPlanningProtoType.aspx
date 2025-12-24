<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoadPlanningProtoType.aspx.cs" Inherits="DynamicsTMS365.LoadPlanningProtoType" %>

<!DOCTYPE html>

<html>
<head>
    
    <meta charset="utf-8" />
    <%--Bing Maps--%>
    <title>DTMS Load Planning</title>
    <%=cssReference%>
    <script src="https://code.jquery.com/jquery-1.12.4.min.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    
    
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.0/js/bootstrap.min.js"></script>

    <script src="https://kendo.cdn.telerik.com/2020.2.617/js/kendo.all.min.js"></script>
    <script src="Scripts/NGL/v-8.5.4.006/core.js"></script>
    <script src="Scripts/NGL/v-8.5.4.006/NGLobjects.js"></script>
    <script src="Scripts/NGL/v-8.5.4.006/SSOA.js"></script>
    <link href="Content/NGL/v-8.5.4.001/jquery.gridly.css" rel="stylesheet" type="text/css" />
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

        .demo-section .k-tabstrip .k-content {
            height: 140px;
        }

        .k-footer-template td:nth-child(1) {
            overflow: visible;
            white-space: nowrap;
        }

        .k-footer-template td:nth-child(1),
        .k-footer-template td:nth-child(2),
        .k-footer-template td:nth-child(3),
        .k-footer-template td:nth-child(4) {
            border-width: 0;
        }

        .demo-section * + h4 {
            margin-top: 2em;
        }

        .demo-section .k-tabstrip .k-content {
            height: 140px;
        }

        .k-altr {
            background-color: #ffffff;
        }
        .loadDet {
            width: 150px;
        }
        .loadDetr {
            width: 50px;
            float:right;
            text-align:right;
        }
    </style>
    <style>
        #centerDiv {
            position: relative;
        }
        .center-grid-div {
            width: 300px;
            /*height: 300px;*/
            overflow: auto;
        }
    </style>
</head>
<body>

    <%=jssplitter2Scripts%>
    <%=BingMapsJS%>  <%--Bing Maps--%>
    <%=sWaitMessage%>
    <div id="example" style="height: 100%; width: 100%; margin-top: 2px;">
        <div id="vertical" style="height: 98%; width: 98%;">
            <div id="menu-pane" style="height: 100%; width: 100%; background-color: white;">
                <div id="tab" class="menuBarTab"></div>
            </div>
            <div id="top-pane">
                <div id="horizontal" style="height: 100%; width: 100%;">
                    <div id="left-pane">
                        <div class="pane-content">
                            <% Response.Write(MenuControl); %>
                            <div id="menuTree"></div>
                        </div>
                    </div>
                    <div id="center-pane">
                        <% Response.Write(PageErrorsOrWarnings); %>
                        <!-- begin Page Content -->
                        <div id="pageContent" class="pane-content">
                            <br />
                            <div class="fast-tab" style="padding-left: 200px;">
                                <%--<span id="ExpandFilters" style="display: none;"><a onclick='expandFilters();'><span style="font-size: small; font-weight: bold;" class='k-icon k-i-chevron-down'></span></a></span>
                                <span id="CollapseFilters" style="display: normal;"><a onclick='collapseFilters();'><span style="font-size: small; font-weight: bold;" class='k-icon k-i-chevron-up'></span></a></span>--%>
                               <%-- <span style="font-size: small; font-weight: bold">Load Planning Filters</span>&nbsp;&nbsp;<br />--%>
                                <div id="divFilters" style="padding: 10px;">
                                    <asp:Label id="lblChkIds" runat="server"></asp:Label>
                                      <asp:Label id="Label1" runat="server"></asp:Label>
                                       <div id="divid39111201806290836427404336filterContent">
                                        <div id="id39111201806290836427404336wrapper">
                                            <div id="id39111201806290836427404336FilterFastTab" style="margin-left: 50px;"><span id="Expandid39111201806290836427404336FilterFastTabSpan" style="display: none;"><a onclick="expandFastTab('Expandid39111201806290836427404336FilterFastTabSpan','Collapseid39111201806290836427404336FilterFastTabSpan','id39111201806290836427404336FilterFastTabHeader',null);"><span style="font - size: small; font - weight: bold;" class="k-icon k-i-chevron-down"></span></a></span><span id="Collapseid39111201806290836427404336FilterFastTabSpan" style="display: normal;"><a onclick="collapseFastTab('Expandid39111201806290836427404336FilterFastTabSpan','Collapseid39111201806290836427404336FilterFastTabSpan','id39111201806290836427404336FilterFastTabHeader',null);"><span style="font - size: small; font - weight: bold;" class="k-icon k-i-chevron-up"></span></a></span><span style="font-size: small; font-weight: bold">Load Filters </span>
                                                <div id="id39111201806290836427404336FilterFastTabHeader" style="padding: 10px; margin-left: 5px;"><span>
                                                    <label for="ddlid39111201806290836427404336Filters" id="ddlid39111201806290836427404336Filters_label">Select Filter:</label><span title="" class="k-widget k-dropdown k-header" unselectable="on" role="listbox" aria-haspopup="true" aria-expanded="false" tabindex="0" aria-owns="ddlid39111201806290836427404336Filters_listbox" aria-labelledby="ddlid39111201806290836427404336Filters_label" aria-live="polite" aria-disabled="false" aria-busy="false" aria-activedescendant="d3b1319a-c1c1-4134-8518-1f6a7e0af694" style=""><span unselectable="on" class="k-dropdown-wrap k-state-default"><span unselectable="on" class="k-input">Carrier</span><span unselectable="on" class="k-select" aria-label="select"><span class="k-icon k-i-caret-alt-down"></span></span></span><input id="ddlid39111201806290836427404336Filters" data-role="dropdownlist" style="display: none;"></span><span id="spid39111201806290836427404336filterText" style="display: none;"><label for="txtid39111201806290836427404336FilterVal"> From:</label><span class="k-widget k-maskedtextbox" style=""><input id="txtid39111201806290836427404336FilterVal" data-role="maskedtextbox" class="k-textbox" autocomplete="off" style="width: 100%;"><span class="k-icon k-i-exclamation-circle"></span></span><label for="txtid39111201806290836427404336FilterValTo"> To:</label><span class="k-widget k-maskedtextbox" style=""><input id="txtid39111201806290836427404336FilterValTo" data-role="maskedtextbox" class="k-textbox" autocomplete="off" style="width: 100%;"><span class="k-icon k-i-exclamation-circle"></span></span></span><span id="spid39111201806290836427404336filterDates" style="display: none;"><label for="dpid39111201806290836427404336FilterFrom"> From:</label><span class="k-widget k-datepicker k-header" style=""><span class="k-picker-wrap k-state-default"><input id="dpid39111201806290836427404336FilterFrom" data-role="datepicker" type="text" class="k-input" role="combobox" aria-expanded="false" aria-owns="dpid39111201806290836427404336FilterFrom_dateview" aria-disabled="false" style="width: 100%;"><span unselectable="on" class="k-select" aria-label="select" role="button" aria-controls="dpid39111201806290836427404336FilterFrom_dateview"><span class="k-icon k-i-calendar"></span></span></span></span><label for="dpid39111201806290836427404336FilterTo"> To:<!-- label--><span class="k-widget k-datepicker k-header" style=""><span class="k-picker-wrap k-state-default"><input id="dpid39111201806290836427404336FilterTo" data-role="datepicker" type="text" class="k-input" role="combobox" aria-expanded="false" aria-owns="dpid39111201806290836427404336FilterTo_dateview" aria-disabled="false" style="width: 100%;"><span unselectable="on" class="k-select" aria-label="select" role="button" aria-controls="dpid39111201806290836427404336FilterTo_dateview"><span class="k-icon k-i-calendar"></span></span></span></span></label></span><span id="spid39111201806290836427404336filterButtons" style="display: none;"><a id="btnid39111201806290836427404336AddFilter" data-role="button" class="k-button k-button-icon" role="button" aria-disabled="false" tabindex="0"><span class="k-icon k-i-plus"></span></a><a id="btnid39111201806290836427404336Filter" data-role="button" class="k-button k-button-icon" role="button" aria-disabled="false" tabindex="0"><span class="k-icon k-i-filter"></span></a><a id="btnid39111201806290836427404336ClearFilter" data-role="button" class="k-button k-button-icon" role="button" aria-disabled="false" tabindex="0"><span class="k-icon k-i-filter-clear"></span></a></span></span>
                                                    <div id="grdid39111201806290836427404336filters" data-role="grid" class="k-grid k-widget k-display-block k-editable" style="">
                                                        <div class="k-grid-header" style="padding-right: 18px;">
                                                            <div class="k-grid-header-wrap k-auto-scrollable" data-role="resizable">
                                                                <table role="grid">
                                                                    <colgroup>
                                                                        <col style="width: 75px">
                                                                        <col>
                                                                        <col>
                                                                        <col>
                                                                        <col>
                                                                        <col>
                                                                    </colgroup>
                                                                    <thead role="rowgroup">
                                                                        <tr role="row">
                                                                            <th scope="col" id="1fb89d21-6b1c-40d2-9ba9-cd9435dfbe4e" rowspan="1" data-index="0" class="k-header" style="">Actions</th>
                                                                            <th scope="col" role="columnheader" data-field="filterID" aria-haspopup="true" rowspan="1" data-title="ID" data-index="1" id="ae7a28e6-a6fb-4014-ab8f-8fce4c15314f" style="display: none" class="k-header" data-role="columnsorter"><a class="k-link" href="#">ID</a></th>
                                                                            <th scope="col" role="columnheader" data-field="filterCaption" aria-haspopup="true" rowspan="1" data-title="Filter" data-index="2" id="bc5e2fbf-8e8d-404f-81f9-f6d73adeba67" class="k-header" data-role="columnsorter" style=""><a class="k-link" href="#">Filter</a></th>
                                                                            <th scope="col" role="columnheader" data-field="filterName" aria-haspopup="true" rowspan="1" data-title="Name" data-index="3" id="31a435f1-c570-4eb5-acf4-29303d53f6e6" style="display: none" class="k-header" data-role="columnsorter"><a class="k-link" href="#">Name</a></th>
                                                                            <th scope="col" role="columnheader" data-field="filterValueFrom" aria-haspopup="true" rowspan="1" data-title="Val From" data-index="4" id="74693f4f-0531-420c-974f-399777dc8a20" class="k-header" data-role="columnsorter" style=""><a class="k-link" href="#">Val From</a></th>
                                                                            <th scope="col" role="columnheader" data-field="filterValueTo" aria-haspopup="true" rowspan="1" data-title="Val To" data-index="5" id="eee94fb8-8666-40dc-a1eb-6b9eb4f2c098" class="k-header" data-role="columnsorter"><a class="k-link" href="#">Val To</a></th>
                                                                            <th scope="col" role="columnheader" data-field="filterFrom" aria-haspopup="true" rowspan="1" data-title="Date From" data-index="6" id="ff8959ac-e2b3-4b26-9537-01be3a81a6c5" class="k-header" data-role="columnsorter"><a class="k-link" href="#">Date From</a></th>
                                                                            <th scope="col" role="columnheader" data-field="filterTo" aria-haspopup="true" rowspan="1" data-title="Date To" data-index="7" id="27f840d6-14f3-4d73-b500-9ca1cc5828dc" class="k-header" data-role="columnsorter" style=""><a class="k-link" href="#">Date To</a></th>
                                                                        </tr>
                                                                    </thead>
                                                                </table>
                                                                <div class="k-resize-handle" style="top: 0px; left: 492px; height: 30px; width: 9px; display: block;">
                                                                    <div class="k-resize-handle-inner"></div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="k-grid-content k-auto-scrollable">
                                                            <table role="grid" data-role="selectable" class="k-selectable">
                                                                <colgroup>
                                                                    <col style="width: 75px">
                                                                    <col>
                                                                    <col>
                                                                    <col>
                                                                    <col>
                                                                    <col>
                                                                </colgroup>
                                                                <tbody role="rowgroup"></tbody>
                                                            </table>
                                                            <div class="k-grid-content-expander" style="width: 1123px;"></div>
                                                        </div>
                                                    </div>
                                                    <input id="txtid39111201806290836427404336SortDirection" type="hidden"><input id="txtid39111201806290836427404336SortField" type="hidden"></div>
                                            </div>
                                        </div>
                                    </div>

                                    <input id="txtSortDirection" type="hidden" />
                                    <input id="txtSortField" type="hidden" />
                                    <input id="txtCarrierControlFrom" type="hidden" />
                                </div>
                            </div>
                            <div>
                                <br />
                            </div>


                            <br />
                    </div>
                        <div id="Lvertical" style="height: 100%;">
                            <div id="Ltop-pane">
                                <div id="Lhorizontal" style="height: 100%; width: 100%;">
                                    <div id="Lleft-pane">
                                        <div class="pane-content" id="leftgrid">
                                            <h3>&nbsp;Summaries</h3>
                                            <div id="elementID" class="k-header k-grid-toolbar">
                                            </div>
                                            <div id="grid1" style="width: 100%; float: left; height: 500px"></div>

                                        </div>
                                    </div>
                                    <div id="Lcenter-pane" style="height: 100%;">
                                        <div class="pane-content">
                                            <h3>&nbsp;Details</h3>
                                            <div id="centerDiv" style="width: 100%;">
                                            </div>
                                        </div>
                                    </div>
                                    <div id="Lright-pane" style="height: 100%;">
                                        <h3>&nbsp;New Loads(<span id="TotNLcnt"></span>)</h3>
                                        <div id="tabstrip">                                            
                                            <ul>
                                                <li class="k-active">New Loads(<span id="NLcnt"></span>)<%--(Count of new Loads)--%>
                                                </li>
                                                <li>Import Queue(<span id="ILcnt"></span>)
                                                </li>
                                            </ul>
                                            <div>
                                                <div>
                                                    <div class="pane-content" id="NewLoadsgrid">
                                                    </div>
                                                </div>
                                            </div>

                                            <div>
                                                <div class="pane-content" id="ImportQueue"></div>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <% Response.Write(FastTabsHTML); %>
                        <!-- End Page Content -->
                    </div>
                </div>
            </div>
            <div id="bottom-pane" class="k-block" style="height: 100%; width: 100%;">
                <div class="pane-content">
                    <% Response.Write(PageFooterHTML); %>
                </div>
            </div>
        </div>

        <% Response.Write(PageTemplates); %>

        <%-- HelpWindow needs to go after AuthNotification because it uses the notification object created there --%>
        <% Response.Write(AuthLoginNotificationHTML); %>
        <% Response.WriteFile("~/Views/HelpWindow.html"); %>

        <script id="rowTemplate" type="text/x-kendo-tmpl">
            <tr data-uid="#: uid #">
              <td class="details">
                <span class="loadDet"><b> #: SolutionDetailOrderNumber # </b> - #: SolutionDetailOrderSequence #</span>
                <span class="loadDetr">#:  SolutionDetailStopNo # </span><br>
                <span> #: SolutionDetailDestName # </span><br>
                <span class="loadDet"> #: SolutionDetailDestCity #,</span>
                <span class="loadDetr"> #: SolutionDetailDestState # </span><br>
                <span>#: SolutionDetailDefaultRouteSequence #</span><br>
              </td>
           </tr>
        </script>
        <script id="altRowTemplate" type="text/x-kendo-tmpl">
            <tr class="k-alt" data-uid="#: uid #">
              <td class="details">
                <span class="loadDet"><b> #: SolutionDetailOrderNumber # </b> - #: SolutionDetailOrderSequence #</span>
                <span class="loadDetr">#:  SolutionDetailStopNo # </span><br>
                <span> #: SolutionDetailDestName # </span><br>
                <span class="loadDet"> #: SolutionDetailDestCity #,</span>
                <span class="loadDetr"> #: SolutionDetailDestState # </span><br>
                <span>#: SolutionDetailDefaultRouteSequence #</span><br>
              </td>
           </tr>
        </script>


        <script type="text/javascript">

            ////Starting Of Filters **********************
          
           
            //$("#btnFilter").kendoButton({ icon: "filter", click: function (e) { $("#grid1").data("kendoNGLGrid").dataSource.read(); } });
          
        </script>

        <script src="Scripts/jquery.gridly.js" type="text/javascript"></script>
        <script>
            $("#tabstrip").kendoTabStrip({
                tabPosition: "top",
                animation: { open: { effects: "fadeIn" } }
            });
            $(document).ready(function() {
                $("#Lvertical").kendoSplitter({
                    orientation: "vertical",
                    panes: [
                        { collapsible: false },
                        { collapsible: false },
                        { collapsible: false }
                    ],
                    resize: function(e) {
                        $('#centerDiv').gridly({
                            base: 60, // px 
                            gutter: 20, // px
                            columns: 8,
                            draggable: {
                                zIndex: 800,
                                selector: '.drag-title'
                            }
                        });
                    }
                });  

                $("#Lhorizontal").kendoSplitter({
                    panes: [
                        { collapsible: true },
                        { collapsible: false },
                        { collapsible: true}
                    ],
                    resize: function(e) {
                        $('#centerDiv').gridly({
                            base: 60, // px 
                            gutter: 20, // px
                            columns: 8,
                            draggable: {
                                zIndex: 800,
                                selector: '.drag-title'
                            }
                        });
                    }
                });
            });
        </script>
        <script>
            $(document).ready(function () {
                var consignments;
                var TotLcnt = 0;

                var objid39111201806290836427404336Filters = new AllFiltersCtrl(); 
                var objid39111201806290836427404336FilterData = [               
       { filterName: "CompanyName", filterCaption: "Warehouse" , filterValueFrom: "",filterValueTo: "",filterFrom: "",filterTo: "",filterIsDate: false },
       { filterName: "CompanyNumber", filterCaption: "Company Number" , filterValueFrom: "",filterValueTo: "",filterFrom: "",filterTo: "",filterIsDate: false },
       { filterName: "CarrierName", filterCaption: "Carrier Name" , filterValueFrom: "",filterValueTo: "",filterFrom: "",filterTo: "",filterIsDate: false },
       { filterName: "CarrierNumber", filterCaption: "Carrier Number" , filterValueFrom: "",filterValueTo: "",filterFrom: "",filterTo: "",filterIsDate: false },
       { filterName: "LoadDate", filterCaption: "Load Date" , filterValueFrom: "",filterValueTo: "",filterFrom: "",filterTo: "",filterIsDate: true },
       { filterName: "RequiredDate", filterCaption: "Required Date" , filterValueFrom: "",filterValueTo: "",filterFrom: "",filterTo: "",filterIsDate: true },
       { filterName: "Address", filterCaption: "Address" , filterValueFrom: "",filterValueTo: "",filterFrom: "",filterTo: "",filterIsDate: false },
       { filterName: "OrigCity", filterCaption: "Orig City" , filterValueFrom: "",filterValueTo: "",filterFrom: "",filterTo: "",filterIsDate: false },
       { filterName: "OrigState", filterCaption: "Orig State" , filterValueFrom: "",filterValueTo: "",filterFrom: "",filterTo: "",filterIsDate: false },
       { filterName: "OrigCountry", filterCaption: "Orig Country" , filterValueFrom: "",filterValueTo: "",filterFrom: "",filterTo: "",filterIsDate: false },
       { filterName: "OrigZip", filterCaption: "Orig Zip" , filterValueFrom: "",filterValueTo: "",filterFrom: "",filterTo: "",filterIsDate: false },
       { filterName: "DestCity", filterCaption: "Dest City" , filterValueFrom: "",filterValueTo: "",filterFrom: "",filterTo: "",filterIsDate: false },
       { filterName: "DestState", filterCaption: "Dest State" , filterValueFrom: "",filterValueTo: "",filterFrom: "",filterTo: "",filterIsDate: false },
       { filterName: "Destountry", filterCaption: "Dest Country" , filterValueFrom: "",filterValueTo: "",filterFrom: "",filterTo: "",filterIsDate: false },
       { filterName: "DestZip", filterCaption: "Dest Zip" , filterValueFrom: "",filterValueTo: "",filterFrom: "",filterTo: "",filterIsDate: false },
       { filterName: "OptionalOriginCompanyName", filterCaption: "Orig Company Name" , filterValueFrom: "",filterValueTo: "",filterFrom: "",filterTo: "",filterIsDate: false },
       { filterName: "OptionalOriginCompanyNumber", filterCaption: "Orig Company Number" , filterValueFrom: "",filterValueTo: "",filterFrom: "",filterTo: "",filterIsDate: false },
       { filterName: "TranCode", filterCaption: "TranCode" , filterValueFrom: "",filterValueTo: "",filterFrom: "",filterTo: "",filterIsDate: false },
       { filterName: "TransType", filterCaption: "Trans Type" , filterValueFrom: "",filterValueTo: "",filterFrom: "",filterTo: "",filterIsDate: false },
        { filterName: "LaneNumber", filterCaption: "Lane Number" , filterValueFrom: "",filterValueTo: "",filterFrom: "",filterTo: "",filterIsDate: false },
       { filterName: "LaneName", filterCaption: "Lane Name" , filterValueFrom: "",filterValueTo: "",filterFrom: "",filterTo: "",filterIsDate: false },        
       //{ filterName: "HoldAfter", filterCaption: "Hold After (1 or 0)" , filterValueFrom: "",filterValueTo: "",filterFrom: "",filterTo: "",filterIsDate: false },
       { filterName: "PageSize", filterCaption: "Page Size" , filterValueFrom: "",filterValueTo: "",filterFrom: "",filterTo: "",filterIsDate: false }];

                objid39111201806290836427404336Filters.loadDefaults("divFilters", "grid1", objid39111201806290836427404336FilterData, 'blueopal',
                    function (results) 
                    {  PageStatus=1;
                        var oKendoGrid1 = $('#grid1').data("kendoNGLGrid"); 
                        if (typeof (oKendoGrid1) !== 'undefined' && ngl.isObject(oKendoGrid1)) { oKendoGrid1.dataSource.read();  } 
                        var oKendoGrid = $('#NewLoadsgrid').data("kendoNGLGrid");
                        if (typeof (oKendoGrid) !== 'undefined' && ngl.isObject(oKendoGrid)) { oKendoGrid.dataSource.read();  } return;
                    });
                objid39111201806290836427404336Filters.show();
                //objid39111201806290836427404336Filters.loadDefaults("divFilters", "NewLoadsgrid", objid39111201806290836427404336FilterData, 'blueopal', function (results) {  PageStatus=1;var oKendoGrid = $('#NewLoadsgrid').data("kendoNGLGrid"); if (typeof (oKendoGrid) !== 'undefined' && ngl.isObject(oKendoGrid)) { oKendoGrid.dataSource.read();  } return; }); objid39111201806290836427404336Filters.show();
          
                var s=$("#grid1FilterFastTab span").eq(4);
                s.html("Load Planning Filters");
           
                $.ajax({ 
                    url: 'api/LoadPlanning/GetFilterSettings/', 
                    contentType: 'application/json; charset=utf-8', 
                    dataType: 'json', 
                    //  data: { filter: JSON.stringify(s) ,PageStatus:JSON.stringify(PageStatus)}, 
                    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                    success: function(data) { 
                      //  debugger;
                        if(   PageStatus!=1){                                    
                            objid39111201806290836427404336Filters.show();
                            objid39111201806290836427404336Filters.addSavedFilters(data.Data);                                    
                        }
                        else{
                            //objid39111201806290836427404336Filters.hide();
                            
                        }
                         PageStatus=1;
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
                        // options.error(result); 
                    } 
                }); 

                //        },           
                //        parameterMap: function(options, operation) { return options; } 
                //    },  
               
                    
                    
                
                //    error: function(xhr, textStatus, error) {
                //        ngl.showErrMsg("Access filterGrid365 Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges();
                //    }
                //});
                //$("#grdgrid1filters").kendoGrid({dataSource:vFilterGrid365});
                //$("#grdgrid1filters").kendoGrid({
                //    dataSource: {
                //        type: "odata",
                //        transport: {
                //            read: "api/LoadPlanning/GetFilterSettings/"
                //        },
                //        schema: {
                //            model: {
                //                fields: {
                //                    filterID: { type: "number" },
                //                    Actions:{type:"string"},
                //                    filterValueFrom: { type: "string" },
                //                    filterValueFrom: { type: "string" },
                //                    filterValueFrom: { type: "string" },
                //                    filterValueTo: { type: "string" },
                //                    filterFrom: { type: "string" },
                //                    filterTo: { type: "string" }
                //                }
                //            }
                //        }
                //       // pageSize: 20
                //        //serverPaging: true,
                //        //serverFiltering: true,
                //        //serverSorting: true
                //    },     
                
                //  //  sortable: true,
                //  //  pageable: true,
                //    columns: [
                //        {field: "filterID", title: "ID", hidden: true },
                //        {field:"",title:"Actions",template:"<a role='button' class='k-button k-button-icontext cm-icononly-button k-grid-delete' href='#'><span class='k-icon k-i-x'></span></a>"},
                //        {field: "filterValueFrom", title: "Filter"},
                //       {field: "filterValueFrom", title: "Name"},
                //        {field: "filterValueFrom", title: "Val From"},
                //        {field: "filterValueTo", title: "Val To"},               
                //       {field: "filterFrom", title: "Date From"},
                //       {field: "filterTo", title: "Date To"}

                //    ]
                //});
            
                function navigationType(){

                    var result;
                    var p;

                    if (window.performance.navigation) {
                        result=window.performance.navigation;
                        if (result==255){result=4} // 4 is my invention!
                    }

                    if (window.performance.getEntriesByType("navigation")){
                        p=window.performance.getEntriesByType("navigation")[0].type;

                        if (p=='navigate'){result=0}
                        if (p=='reload'){result=1}
                        if (p=='back_forward'){result=2}
                        if (p=='prerender'){result=3} //3 is my invention!
                    }
                    return result;
                }
                PageStatus=100;
                if(PageStatus==100){
                    PageStatus=navigationType();
                }
                else{
                    PageStatus=1;
                }
                PageSize = 10;


                vSummaryLoadsGrid365 = new kendo.data.DataSource({
                    serverSorting: true, 
                    serverPaging: true, 
                    pageSize: 10,
                    transport: { 
                        read: function(options) {                           
                            var s = objid39111201806290836427404336Filters.data;                            
                            s.page = options.data.page;
                            s.skip = options.data.skip;
                            s.take = options.data.take;

                            $.ajax({ 

                                url: 'api/LoadPlanning/GetRecordsBySummeryFilter/', 
                                contentType: 'application/json; charset=utf-8', 
                                dataType: 'json', 
                                data: { filter: JSON.stringify(s), PageStatus:JSON.stringify(PageStatus) }, 
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                                success: function(data) { 
                                    consignments = data.Data;
                                    //console.log(cons[0]["SolutionTruckConsPrefix"]);
                                    options.success(data); 
                                    console.log(data);
                                    if (data.Errors != null) { 
                                        PageStatus=1;
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
                            id: "SolutionTruckControl",
                            fields: {
                                SolutionTruckControl: { type: "number" },
                                SolutionTruckConsPrefix: { type: "string" },
                                SolutionTruckTotalWgt: { type: "number" },                               
                                SolutionTruckTotalPL: { type: "number" },
                                SolutionTruckTotalCube: { type: "number" },
                                SolutionTruckCarrierName: { type: "string" },
                                SolutionTruckSolutionControl:{ type: "number" },
                                SolutionTruckStaticRouteControl: { type: "number" },
                                SolutionTruckAttributeControl: { type: "number" },
                                SolutionTruckAttributeTypeControl: { type: "string" },
                                SolutionTruckCom: { type: "string" },
                                SolutionTruckConsPrefix: { type: "string" },
                                SolutionTruckRouteConsFlag: { type: "bool" },
                                SolutionTruckCarrierControl: { type: "number" },
                                SolutionTruckCarrierNumber: { type: "number" },
                                SolutionTruckCarrierName: { type: "string" },
                                SolutionTruckCarrierTruckControl: { type: "number" },
                                SolutionTruckCarrierTruckDescription: { type: "string" },
                                SolutionTruckTotalCases: { type: "number" },
                                SolutionTruckTotalWgt: { type: "number" },
                                SolutionTruckTotalPL: { type: "number" },
                                SolutionTruckTotalCube: { type: "number" },
                                SolutionTruckTotalPX: { type: "number" },
                                SolutionTruckTotalBFC: { type: "number" },
                                SolutionTruckTotalOrders: { type: "number" },
                                SolutionTruckTotalCost: { type: "number" },
                                SolutionTruckTotalMiles: { type: "number" },
                                SolutionTruckCarrierEquipmentCodes: { type: "string" },
                                SolutionTruckRouteTypeCode: { type: "number" },
                                SolutionTruckTransType: { type: "number" },
                                SolutionTruckCommitted: { type: "bool" },
                                SolutionTruckCommittedDate: { type: "date" },
                                SolutionTruckCapacityPreference: { type: "number" },
                                SolutionTruckMinCases: { type: "number" },
                                SolutionTruckSplitCases: { type: "string" },
                                SolutionTruckMaxCases: { type: "number" },
                                SolutionTruckMinWgt: { type: "number" },
                                SolutionTruckSplitWgt: { type: "number" },
                                SolutionTruckMaxWgt: { type: "number" },
                                SolutionTruckMinCubes: { type: "number" },
                                SolutionTruckSplitCubes: { type: "number" },
                                SolutionTruckMaxCubes: { type: "number" },
                                SolutionTruckMinPlts: { type: "number" },
                                SolutionTruckSplitPlts: { type: "number" },
                                SolutionTruckMaxPlts: { type: "number" },
                                SolutionTruckTrucksAvailable: { type: "number" },
                                SolutionTruckIsHazmat: { type: "bool" },
                                SolutionTruckLaneNumbers: { type: "string" },
                                SolutionTruckLaneNames: { type: "string" },
                                SolutionTruckBookNotes: { type: "string" },
                                SolutionTruckModDate: { type: "date" },
                                SolutionTruckModUser: { type: "string" },
                                SolutionTruckUpdated: { type: "date" }
                              
                            }
                        },
                        errors: "Errors"
                    },
                    error: function(xhr, textStatus, error) {
                        ngl.showErrMsg("Access vSummaryLoadsGrid365 Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges();
                    }
                });
                $('#grid1').kendoNGLGrid({
                    theme: "blueopal",
                    toolbarColumnMenu: true,
                    dataSource: vSummaryLoadsGrid365,                   
                    autoBind: true,
                    pageable: { pageSizes: [5, 10, 15, 20, 25, 50]},
                    sortable: true,
                    resizable: true,
                    groupable: true, 
                    columns: [
                     { template: "<input type='checkbox' class='checkbox' click='selectRow()' />" },
        {field: "SolutionTruckControl", title: "SolutionTruckControl", hidden: true,PageDetPageControl: 26, PageDetControl: 103909 },
        {field: "SolutionTruckConsPrefix", title: "CNS",showhide: 1,PageDetPageControl: 26, PageDetControl: 103908},
        {field: "SolutionTruckTotalWgt", title: "Wgt",showhide: 1,PageDetPageControl: 26, PageDetControl: 103939},
        {field: "SolutionTruckTotalPL", title: "PL",showhide: 1,PageDetPageControl: 26, PageDetControl: 103937},
        {field: "SolutionTruckTotalCube", title: "Volume",showhide: 1,PageDetPageControl: 26, PageDetControl: 103934},               
        {field: "SolutionTruckCarrierName", title: "Carrier Name",showhide: 1,PageDetPageControl: 26, PageDetControl: 103901},
        {field: "SolutionTruckCarrierControl", title: "Carrier Control",showhide: 1,hidden: true,PageDetPageControl: 26, PageDetControl: 103899},
        {field: "SolutionTruckCarrierNumber", title: "Carrier Number",showhide: 1,hidden: true,PageDetPageControl: 26, PageDetControl: 103902},
        {field: "SolutionTruckCarrierTruckControl", title: "Carrier Truck Control",showhide: 1,hidden: true,PageDetPageControl: 26, PageDetControl: 103903},
        {field: "SolutionTruckCarrierTruckDescription", title: "CarrierTruck Description",showhide: 1,hidden: true,PageDetPageControl: 26, PageDetControl: 103904},
        {field: "SolutionTruckCom", title: "SolutionTruckCom",showhide: 1,hidden: true,PageDetPageControl: 26, PageDetControl: 103905},
        {field: "SolutionTruckCommitted", title: "Committed",showhide: 1,hidden: true,PageDetPageControl: 26, PageDetControl: 103906},               
        {field: "SolutionTruckCommittedDate", title: "Committed Date",showhide: 1,hidden: true,PageDetPageControl: 26, PageDetControl: 103907},
        {field: "SolutionTruckTotalCases", title: "Qty",showhide: 1,hidden: true,PageDetPageControl: 26, PageDetControl: 103932},               
        {field: "SolutionTruckTotalPX", title: "PX",showhide: 1,hidden: true,PageDetPageControl: 26, PageDetControl: 103938},
        {field: "SolutionTruckTotalBFC", title: "BFC",showhide: 1,hidden: true,PageDetPageControl: 26, PageDetControl: 103931},
        {field: "SolutionTruckTotalOrders", title: "Orders",showhide: 1,hidden: true,PageDetPageControl: 26, PageDetControl: 103936},
        {field: "SolutionTruckTotalCost", title: "Cost",showhide: 1,hidden: true,PageDetPageControl: 26, PageDetControl: 103933},
        {field: "SolutionTruckTotalCube", title: "Volume",showhide: 1,hidden: true,PageDetPageControl: 26, PageDetControl: 103934},               
        {field: "SolutionTruckTotalMiles", title: "Miles",showhide: 1,hidden: true,PageDetPageControl: 26, PageDetControl: 103935},
        {field: "SolutionTruckCarrierEquipmentCodes", title: "Carrier Equip Code",showhide: 1,hidden: true,PageDetPageControl: 26, PageDetControl: 103900},
        {field: "SolutionTruckRouteTypeCode", title: "RouteType Code",showhide: 1,hidden: true,PageDetPageControl: 26, PageDetControl: 103924},
        {field: "SolutionTruckTransType", title: "Trans Type",showhide: 1,hidden: true,PageDetPageControl: 26, PageDetControl: 103940},
        {field: "SolutionTruckCapacityPreference", title: "Capacity Preference",showhide: 1,hidden: true,PageDetPageControl: 26, PageDetControl: 103898},
        {field: "SolutionTruckMinCases", title: "Min Cases",showhide: 1,hidden: true,PageDetPageControl: 26, PageDetControl: 103917},               
        {field: "SolutionTruckSplitCases", title: "Split Cases",showhide: 1,hidden: true,PageDetPageControl: 26, PageDetControl: 103926},
        {field: "SolutionTruckMaxCases", title: "Max Cases",showhide: 1,hidden: true,PageDetPageControl: 26, PageDetControl: 103913},
        {field: "SolutionTruckMinWgt", title: "Min Wgt",showhide: 1,hidden: true,PageDetPageControl: 26, PageDetControl: 103920},
        {field: "SolutionTruckSplitWgt", title: "Split Wgt",showhide: 1,hidden: true,PageDetPageControl: 26, PageDetControl: 103929},
        {field: "SolutionTruckMaxWgt", title: "Max Wgt",showhide: 1,hidden: true,PageDetPageControl: 26, PageDetControl: 103916},               
        {field: "SolutionTruckMinCubes", title: "Min Cubes",showhide: 1,hidden: true,PageDetPageControl: 26, PageDetControl: 103918},
        {field: "SolutionTruckSplitCubes", title: "Split Cubes",showhide: 1,hidden: true,PageDetPageControl: 26, PageDetControl: 103927},
        {field: "SolutionTruckMaxCubes", title: "Max Cubes",showhide: 1,hidden: true,PageDetPageControl: 26, PageDetControl: 103914},
        {field: "SolutionTruckTrucksAvailable", title: "Trucks Aval",showhide: 1,hidden: true,PageDetPageControl: 26, PageDetControl: 103941},
        {field: "SolutionTruckIsHazmat", title: "IsHazmat",showhide: 1,hidden: true,PageDetPageControl: 26, PageDetControl: 103910},               
        {field: "SolutionTruckLaneNumbers", title: "Lane Number",showhide: 1,PageDetPageControl: 26, PageDetControl: 103912},
        {field: "SolutionTruckLaneNames", title: "Lane Names",showhide: 1,hidden: true,PageDetPageControl: 26, PageDetControl: 103911},
        {field: "SolutionTruckBookNotes", title: "Book Notes",showhide: 1,hidden: true,PageDetPageControl: 26, PageDetControl: 103897},
        {field: "SolutionTruckModDate", title: "Mod Date",showhide: 1,hidden: true,PageDetPageControl: 26, PageDetControl: 103921},               
        {field: "SolutionTruckModUser", title: "Mod User",showhide: 1,hidden: true,PageDetPageControl: 26, PageDetControl: 103922},
        {field: "SolutionTruckUpdated", title: "Updated",showhide: 1,hidden: true,PageDetPageControl: 26, PageDetControl: 103942},
        {field: "SolutionTruckSolutionControl", title: "Solution Control",showhide: 1,hidden: true,PageDetPageControl: 26, PageDetControl: 103925},
        {field: "SolutionTruckStaticRouteControl", title: "Static Route Control",showhide: 1,hidden: true,PageDetPageControl: 26, PageDetControl: 103930},
        {field: "SolutionTruckAttributeControl", title: "Attribute Control",showhide: 1,hidden: true,PageDetPageControl: 26, PageDetControl: 103895},               
        {field: "SolutionTruckAttributeTypeControl", title: "AttributeType Control",showhide: 1,hidden: true,PageDetPageControl: 26, PageDetControl: 103896},            
        {field: "SolutionTruckRouteConsFlag", title: "RouteCons Flag",showhide: 1,hidden: true,PageDetPageControl: 26, PageDetControl: 103923},            
        {field: "SolutionTruckMaxPlts", title: "Max Plts",showhide: 1,hidden: true,PageDetPageControl: 26, PageDetControl: 103915},
        {field: "SolutionTruckMinPlts", title: "Max Plts",showhide: 1,hidden: true,PageDetPageControl: 26, PageDetControl: 103919},
        {field: "SolutionTruckSplitPlts", title: "Split Cubes",showhide: 1,hidden: true,PageDetPageControl: 26, PageDetControl: 103928}
                    ]
               
                });

                var grid = $("#grid1").data("kendoNGLGrid");
                
                vNewLoadsGrid365 = new kendo.data.DataSource({
                    serverSorting: true, 
                    serverPaging: true, 
                    pageSize: 10,
                    transport: { 
                        read: function(options) { 
                            //debugger;
                            //var s = new AllFilter();
							var s = objid39111201806290836427404336Filters.data;

                            s.page = options.data.page;
                            s.skip = options.data.skip;
                            s.take = options.data.take;
                            s.PageSize = 10;

                            $.ajax({ 

                                url: 'api/LoadPlanning/GetRecordsByNewOrdersFilter/', 
                                contentType: 'application/json; charset=utf-8', 
                                dataType: 'json', 
                                data: { filter: JSON.stringify(s),PageStatus:JSON.stringify(PageStatus) }, 
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                                success: function(data) { 
                                    options.success(data);
									PageStatus=1; 
                                    $("#NLcnt").text(data.Data.length);
                                    TotLcnt = data.Data.length;
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
                            id: "Control",
                            fields: {
                                Control: { type: "number" },
                                SolutionDetailConsPrefix: { type: "string" },
                                SolutionDetailTotalWgt: { type: "number" },                               
                                SolutionDetailTotalPL: { type: "number" },
                                SolutionDetailTotalCube: { type: "number" },
                                SolutionDetailCarrierName: { type: "string" }
                              
                            }
                        },
                        errors: "Errors"
                    },
                    error: function(xhr, textStatus, error) {
                        ngl.showErrMsg("Access vNewLoadsGrid365 Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges();
                    }
                });
                console.log(vNewLoadsGrid365);
                var grid2 = $('#NewLoadsgrid').kendoNGLGrid({
                    theme: "blueopal",
                    dataSource: vNewLoadsGrid365,
                    toolbarColumnMenu:true,
                    pageable: true,
                    autoBind: true,
                    pageable: { pageSizes: [5, 10, 15, 20, 25, 50]},
                    sortable: true,
                    resizable: true,
                    groupable: true, 
                    columns: [         
                        {field: "SolutionDetailOrderNumber", title: "Order No.",showhide: 1},
                        {field: "SolutionDetailTotalWgt", title: "Wgt",showhide: 1},
                        {field: "SolutionDetailTotalPL", title: "Pits",showhide: 1},
                        {field: "SolutionDetailTotalCube", title: "Volume",showhide: 1},               
                        {field: "SolutionDetailDestName", title: "Destination",showhide: 1},
                        {field: "SolutionDetailDestCity", title: "City",showhide: 1},
                        {field: "SolutionDetailDestState", title: "State",showhide: 1}
                    ]
               
                }).data("kendoNGLGrid");

         
                vImportQueueGrid365 = new kendo.data.DataSource({
                    serverSorting: true, 
                    serverPaging: true, 
                    pageSize: 10,     
                    pageable: { pageSizes: [5, 10, 15, 20, 25, 50]},
                    transport: { 
                        read: function(options) { 
                            //debugger;
                            //var s = new AllFilter();
							var s = objid39111201806290836427404336Filters.data;

                            // s.sortName = $("#txtSettlementGridSortField").val();s.sortDirection = $("#txtSettlementGridSortDirection").val();
                            s.page = options.data.page;
                            s.skip = options.data.skip;
                            s.take = options.data.take;

                            $.ajax({ 

                                url: 'api/LoadPlanning/GetRecordsByImportQueueFilter/', 
                                contentType: 'application/json; charset=utf-8', 
                                dataType: 'json', 
                                data: { filter: JSON.stringify(s) }, 
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                                success: function(data) { 
                                    options.success(data); 
                                    $("#ILcnt").text(data.Data.length);
                                    TotLcnt = TotLcnt + data.Data.length;
                                    $("#TotNLcnt").text(TotLcnt);

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
                            id: "Control",
                            fields: {
                                Control: { type: "number" },
                                SolutionDetailConsPrefix: { type: "string" },
                                SolutionDetailTotalWgt: { type: "number" },                               
                                SolutionDetailTotalPL: { type: "number" },
                                SolutionDetailTotalCube: { type: "number" },
                                SolutionDetailCarrierName: { type: "string" }
                              
                            }
                        },
                        errors: "Errors"
                    },
                    error: function(xhr, textStatus, error) {
                        ngl.showErrMsg("Access ImpoertQueue Data Failed", formatAjaxJSONResponsMsgs(xhr, textStatus, error, " cannot complete your request"), null); this.cancelChanges();
                    }
                });
                
                var grid3 = $('#ImportQueue').kendoNGLGrid({
                    theme: "blueopal",
                    dataSource: vImportQueueGrid365,
                    pageable: true,
                    autoBind: true,
                    pageable: { pageSizes: [5, 10, 15, 20, 25, 50]},
                    sortable: true,
                    resizable: true,
                    groupable: true, 
                    columns: [        
                        {field: "SolutionDetailConsPrefix", title: "Order No."},
                        {field: "SolutionDetailTotalWgt", title: "Wgt"},
                        {field: "SolutionDetailTotalPL", title: "Pits"},
                        {field: "SolutionDetailTotalCube", title: "Volume"},               
                        {field: "SolutionDetailDestName", title: "Destination",showhide: 1},
                        {field: "SolutionDetailDestCity", title: "City",showhide: 1},
                        {field: "SolutionDetailDestState", title: "State",showhide: 1}
                    ]
               
                }).data("kendoNGLGrid");

                $(grid3.element).kendoDraggable({
                    filter: "tr",
                    hint: function (e) {
                        var item = $('<div class="k-grid k-widget" style="background-color: MediumVioletRed; color: black;"><table><tbody><tr>' + e.html() + '</tr></tbody></table></div>');
                        return item;
                    },
                    group: "gridGroup1",
                });

                $(grid2.element).kendoDraggable({
                    filter: "tr",
                    hint: function (e) {
                        var item = $('<div class="k-grid k-widget" style="background-color: MediumVioletRed; color: black;"><table><tbody><tr>' + e.html() + '</tr></tbody></table></div>');
                        return item;
                    },
                    group: "gridGroup1",
                });
               
                grid.table.on("click", ".checkbox" , selectRow);               

                var checkedIds = {};
                function selectRow() {                  
                    var checked = this.checked,
                    row = $(this).closest("tr"),
                    dataItem = grid.dataItem(row);
                    checkedIds[dataItem.SolutionTruckConsPrefix] = checked;
                    //console.log(checkedIds);
                    console.log(row);
                    if (checked) {
                        CreateDynamicGrid(dataItem);
                        //-select the row
                        row.addClass("k-state-selected");
                    } else {
                        RemoveGrid(dataItem);
                        //-remove selection
                        row.removeClass("k-state-selected");
                    }
                }               


                function CreateDynamicGrid(dataItem){
                    //debugger;
                    var gridId = "dy_grid_"+ dataItem.SolutionTruckKey + "___" + dataItem.SolutionTruckCompControl;
                    var divId = "dy_div_"+dataItem.SolutionTruckKey;
                    $( "#centerDiv" ).append($("<div id="+divId+" class='center-grid-div'><div class='d-flex drag-title'><table border='0' width=280'><tr><td><img src='../Content/NGL/CarrierLogos/NGLSlogo.png' height='30px' width='30px' class='mr-2' /></td>" +
                                                "<td><h3>"+dataItem.SolutionTruckConsPrefix+" </h3><br/>"+dataItem.SolutionTruckCarrierName+"</td></tr></table></div>" +
                                                "<div id="+gridId+" data-cid=" + dataItem.SolutionTruckKey + "></div><br/>"));

                    $('#centerDiv').gridly();
                    let newobj = consignments.filter(function (x) {
                        return x.SolutionTruckKey == dataItem.SolutionTruckKey
                    })[0];
                    var db1 = [];
                    for (var i = 0; i < newobj.SolutionDetails.length; ++i) {
                        var newsolution = newobj.SolutionDetails[i];
                        //newsolution.SolutionTruckTotalWgt = newsolution.SolutionDetailTotalWgt;
                        db1.push(newsolution);
                    }
                    //Array.prototype.push.apply(db1, newobj.SolutionDetails);
                    var newdatasource = new kendo.data.DataSource({
                        data: db1
                    });

                    //console.log(newdatasource);

                    var gridnew = $("#"+gridId).kendoGrid({
                        theme: "blueopal",
                        width: 280,
                        dataSource: newdatasource,
                        rowTemplate: kendo.template($("#rowTemplate").html()),
                        altRowTemplate: kendo.template($("#altRowTemplate").html()),
                        dataBound: function(e){
                            //var gridId1= $(e.sender.element[0]).attr("id");
                            //$('#'+gridId1+' .k-grid-content').height(180);                            
                            var items = e.sender.items();
                            var id = $(e.sender.element[0]).attr("data-cid");
                            ////debugger;
                            let obj = consignments.filter(function (x) {
                                return x.SolutionTruckKey == id
                            })[0];
                           
                            var summary = {
                                Wgt : 0,
                                Pits : 0,
                                Quantity : 0,
                                Volume : 0,
                                Distance: obj["SolutionTruckTotalMiles"],
                                TotalCost: obj["SolutionTruckTotalCost"]

                            };
                            items.each(function(){
                                var dt = e.sender.dataItem(this);
                                summary.Wgt += dt.SolutionDetailTotalWgt;
                                summary.Pits += dt.SolutionDetailTotalPL;
                                summary.Quantity += dt.SolutionDetailTotalCases;
                                summary.Volume += dt.SolutionDetailTotalCube;
                                //summary.Distance += dt.SolutionTruckTotalMiles;
                                //summary.TotalCost += dt.SolutionTruckTotalCost;
                            });

                            var wrapper = e.sender.element.find(".summaryWrapper");
                            for (var prop in summary) {
                                wrapper.append("<span>"+ prop + ": "+summary[prop]+"</span><br>");
                            }
                        },
                        columns: [
                          { 
                              field: "SolutionTruckConsPrefix" , title: "Load",
                              footerTemplate: "<span class='summaryWrapper'></span>" 
                          }
                        ]
                    }).data("kendoGrid");


                    $(gridnew.element).kendoDraggable({
                        filter: "tr",
                        hint: function (e) {
                            var item = $('<div class="k-grid k-widget" style="background-color: lightskyblue; color: black;"><table><tbody><tr>' + e.html() + '</tr></tbody></table></div>');
                            return item;
                        },
                        group: "gridGroup2",
                    });


                    grid3.wrapper.kendoDropTarget({
                        drop: function (e) {
                            var ds = $(e.draggable.element[0]).data().kendoNGLGrid.dataSource;
                            var dataItem = ds.getByUid(e.draggable.currentTarget.data("uid"));
                            if (dataItem.source == "NewLoadsgrid") {
                                dataItem.source = "";
                                ds.remove(dataItem);
                                vImportQueueGrid365.add(dataItem);
                                $('#centerDiv').gridly();
                            }
                        },
                        group: "gridGroup2",
                    });

                    grid2.wrapper.kendoDropTarget({
                        drop: function (e) {
                            var ds = $(e.draggable.element[0]).data().kendoNGLGrid.dataSource;
                            var dataItem = ds.getByUid(e.draggable.currentTarget.data("uid"));
                            if (dataItem.source == "ImportQueue") {
                                dataItem.source = "";
                                ds.remove(dataItem);
                                vNewLoadsGrid365.add(dataItem);
                                $('#centerDiv').gridly();
                            }
                        },
                        group: "gridGroup2",
                    });

                    gridnew.table.kendoDropTarget({
                        group: "gridGroup2",
                        drop: function (e) {
                            e.draggable.hint.hide();
                            var ds = $(e.draggable.element[0]).data().kendoGrid.dataSource;
                            var target = ds.getByUid($(e.draggable.currentTarget).data("uid")),
                              dest = $(document.elementFromPoint(e.clientX, e.clientY));
			
                            var sourcegridid= $(e.draggable.element[0]).attr("id");
                            var grids = dest.parents(".k-grid");
                            var destgridid = "";
                            if(grids.length > 0){
                                destgridid = $(grids[0]).attr("id");
                            }
                            if (dest.is("th") || !$(e.draggable.currentTarget).is("tr")) {
                                return;
                            }
			  
                            if(sourcegridid != destgridid){
                                // will go here if destination grid is not empty
                                var desds = $("#"+destgridid).data().kendoGrid.dataSource;
                                var dataItem = ds.getByUid(target.get("uid"));
                                console.log(dataItem);
                                console.log(desds);
                                ds.remove(dataItem);
                                desds.add(dataItem);
                            }
                            else{
                                dest = ds.getByUid(dest.parent().data("uid"));

                                //not on same item
                                if (target.get("uid") !== dest.get("uid")) {
                                    //reorder the items
                                    var index = ds.indexOf(target);
                                    var index1 = ds.indexOf(dest);
                                    console.log(index);
                                    console.log(index1);
                                    ds.data().splice(index1, 0, ds.data().splice(index, 1)[0]);
                                    ds.sync();
                                }
                            }
                        }
                    });

                    gridnew.wrapper.kendoDropTarget({
                        drop: function (e) {
                            var truckKey = 0;
                            var bookCompControl = 0;
                            var LoadControl = 0;
                            var bookcompcontrol = "";
                            var ds = $(e.draggable.element[0]).data().kendoNGLGrid.dataSource;
                            var dataItem = ds.getByUid(e.draggable.currentTarget.data("uid"));
                            ds.remove(dataItem);
                            console.log(dataItem);
                            //This gives the grid identifier
                            var closestGridElement = e.sender.element.closest('[data-role="grid"]');
                            var id = closestGridElement.attr('id');
                            //console.log($(e.draggable.element[0]).attr("id"));
                            if($(e.draggable.element[0]).attr("id") == "NewLoadsgrid") {
                                bookcompcontrol = id.replace("dy_grid_", "").split("___");
                                var gridId = id;
                                truckKey = bookcompcontrol[0];
                                bookCompControl = bookcompcontrol[1];
                                LoadControl = dataItem["SolutionDetailBookControl"];
                                //console.log(truckKey);
                                //console.log(bookCompControl);
                                //console.log(LoadControl);

                                var dataJSON = { SolDetailBookControl: LoadControl, SolDetailCompControl: bookCompControl, solTruckKey:truckKey, drpIndex:"0" };
                                $.ajax({
                                    type: "POST",
                                    url: "api/LoadPlanning/LoadItemDropToTruck/",
                                    contentType: "application/json; charset=utf-8",
                                    dataType: 'json',
                                    data: JSON.stringify(dataJSON),
                                    headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }, 
                                    success: function(data) {

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
                                    error: function (xhr, textStatus, error) {
                                        var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure");
                                        ngl.showErrMsg("Load Planning - Add Order to Truck", sMsg, null);
                                    }
                                });


                            } else {
                                bookcompcontrol = id.replace("dy_grid_", "").split("___");
                                truckKey = bookcompcontrol[0];
                                bookCompControl = bookcompcontrol[1];
                                LoadControl = dataItem["SolutionDetailPOHdrControl"];
                                //console.log(truckKey);
                                //console.log(bookCompControl);
                                //console.log(LoadControl);
                            }
                            //debugger;
                            //This gives the datasource name of kendo grid
                            //console.log(newdatasource);
                            dataItem.source = $(e.draggable.element[0]).attr("id");
                            console.log(dataItem);
                            newdatasource.add(dataItem);
                            $('#centerDiv').gridly();
                        },
                        group: "gridGroup1",
                    });                    
                    gridnew.wrapper.kendoDropTarget({
                        drop: function (e) {
                            // will go here if destination grid is empty
                            var ds = $(e.draggable.element[0]).data().kendoGrid.dataSource;
                            var dataItem = ds.getByUid(e.draggable.currentTarget.data("uid"));
                            console.log(dataItem);
                            ds.remove(dataItem);
                            dataItem.source = $(e.draggable.element[0]).attr("id");
                            newdatasource.add(dataItem);
                            var destgriddata = $(e.sender.element.closest('[data-role="grid"]')).data();
                            console.log(destgriddata);
                            $('#centerDiv').gridly();
                        },
                        group: "gridGroup2",
                    });
                }

                function RemoveGrid(dataItem) {
                    //debugger;
                    var divId = "dy_div_" + dataItem.SolutionTruckKey;
                    var gridId = "dy_grid_" + dataItem.SolutionTruckKey;
                    //var data = $("#" + gridId).data().kendoGrid.dataSource.data();
                    //data.forEach(function (item, index) {
                    //    console.log(item.source);
                    //    if(item.source == "NewLoadsgrid"){
                    //        item.source = "";
                    //        vNewLoadsGrid365.add(item);
                    //    }
                    //    else if(item.source == "ImportQueue"){
                    //        item.source = "";
                    //        vImportQueueGrid365.add(item);
                    //    }         
                    //});
                    $("#" + divId).remove();
                    //$('#centerDiv').gridly();
                }


            });
        </script>


        <script>

        <% Response.Write(ADALPropertiesjs); %>

            var PageControl = '<%=PageControl%>';
            var control = <%=UserControl%>;
            ngl.UserValidated(true,control,oredirectUri);
            var wnd = kendo.ui.Window;
            var tObj = this;
            var tPage = this;


        <% Response.Write(PageCustomJS); %>

            


            let checkiDValues=[];
            $(document).ready(function () {
                var PageMenuTab = <%=PageMenuTab%>;
                control = <%=UserControl%>;
                if (ngl.UserValidated(true,control,oredirectUri)) { return; }

                if (control != 0){
                }
                var PageReadyJS = <%=PageReadyJS%>;
                menuTreeHighlightPage(); //must be called after PageReadyJS
                var divWait = $("#h1Wait");
                if (typeof (divWait) !== 'undefined') { divWait.hide(); }
            });
        </script>

        <style>
            /* #grid { width : 490px; }  .k-grid tbody tr{ height: 38px; }  */
            .k-grid tbody tr td {
                overflow: hidden;
                text-overflow: ellipsis;
                white-space: nowrap;
            }

            .k-tooltip {
                max-height: 500px;
                max-width: 450px;
                overflow-y: auto;
            }

            .k-grid tbody .k-grid-Edit {
                min-width: 0;
            }

                .k-grid tbody .k-grid-Edit .k-icon {
                    margin: 0;
                }

            .RateITOptions ul {
                margin: 0;
                padding: 0;
                max-width: 255px;
            }

                .RateITOptions ul li {
                    margin: 0;
                    padding: 10px 0px 0px 20px;
                    min-height: 25px;
                    line-height: 25px;
                    vertical-align: middle;
                    /*border: 1px solid rgba(128,128,128,.5);*/
                    border-top: 1px solid rgba(128,128,128,.5);
                }

            .RateITOptions {
                min-width: 220px;
                padding: 0;
                position: relative;
            }

                .RateITOptions ul li .km-switch {
                    float: right;
                }

            .RateITOptions-head {
                height: 50px;
                background-color: skyblue;
            }
        </style>
    </div>
</body>
</html>
