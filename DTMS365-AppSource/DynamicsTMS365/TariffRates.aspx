<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TariffRates.aspx.cs" Inherits="DynamicsTMS365.TariffRates" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>DTMS Tariff Rates</title>
    <%=cssReference%>
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
    </style>
</head>
<body>
    <%=jssplitter2Scripts%>
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
        <% Response.WriteFile("~/Views/TariffRatesMatrixWindow.html"); %>

        <script>
        <% Response.Write(ADALPropertiesjs); %>

            var PageControl = '<%=PageControl%>';
            var tObj = this;
            var oTariffRatesGrid = null;
            var tPage = this;


        <% Response.Write(NGLOAuth2); %>
            
            function execBeforeTariffRatesGridInsert() {
                //try to get the shared defalult values from the current selected value if available
                //if not available the server will use the current tarriff settings by default
                var blnRowSelected = false;
                try {


                    var row = oTariffRatesGrid.select();
                    if (typeof (row) !== 'undefined' && row !== null) {
                        var dataItem = oTariffRatesGrid.dataItem(row);
                        if (typeof (dataItem) !== 'undefined' && dataItem !== null) {
                            if (typeof (objTariffRatesGridDataFields) !== 'undefined' && objTariffRatesGridDataFields != null) {
                                blnRowSelected = true;
                                //var pdataFields = objTariffRatesGridDataFields; //wdgtTariffRatesGridEdit.dataFields
                                $.each(objTariffRatesGridDataFields, function (index, item) {
                                    if (item.fieldName == "CarrTarEquipMatCarrTarControl") {
                                        if ("CarrTarEquipMatCarrTarControl" in dataItem) {
                                            item.fieldDefaultValue = dataItem.CarrTarEquipMatCarrTarControl;
                                        }
                                    } else if (item.fieldName == "CarrTarEquipMatCarrTarEquipControl") {
                                        if ("CarrTarEquipMatCarrTarEquipControl" in dataItem) {
                                            item.fieldDefaultValue = dataItem.CarrTarEquipMatCarrTarEquipControl;
                                        }
                                    } else if (item.fieldName == "CarrTarEquipMatCarrTarMatBPControl") {
                                        if ("CarrTarEquipMatCarrTarMatBPControl" in dataItem) {
                                            item.fieldDefaultValue = dataItem.CarrTarEquipMatCarrTarMatBPControl;
                                        }
                                    } else if (item.fieldName == "CarrTarEquipMatClassTypeControl") {
                                        if ("CarrTarEquipMatClassTypeControl" in dataItem) {
                                            item.fieldDefaultValue = dataItem.CarrTarEquipMatClassTypeControl;
                                        }
                                    } else if (item.fieldName == "CarrTarEquipMatTarRateTypeControl") {
                                        if ("CarrTarEquipMatTarRateTypeControl" in dataItem) {
                                            item.fieldDefaultValue = dataItem.CarrTarEquipMatTarRateTypeControl;
                                        }
                                    } else if (item.fieldName == "CarrTarEquipMatTarBracketTypeControl") {
                                        if ("CarrTarEquipMatTarBracketTypeControl" in dataItem) {
                                            item.fieldDefaultValue = dataItem.CarrTarEquipMatTarBracketTypeControl;
                                        }
                                    }
                                });
                            }


                        }

                    }
                    if (blnRowSelected === false) {
                        var wdth = ($(window).width() / 10) * 6;
                        var hgt = ($(window).height() / 10) * 6;
                        ngl.Alert("Read Current Rate Defaults", "We could not read the current rate default settings. Please select an existing rate from the grid, these settings will be used to set up defaults for the new rate.", wdth, hgt);

                    }

                } catch (err) {
                    ngl.showErrMsg("Create new rate record Failure", err, tPage);
                }
                return blnRowSelected;
            }
         <% Response.Write(PageCustomJS); %>
            function execActionClick(btn, proc) {
                if (btn.id == "btnOpenContract") {
                    location.href = "Tariff";
                } else if (btn.id == "btnOpenServices") {
                    location.href = "TariffServices";
                } else if (btn.id == "btnAddRate") {
                    if (typeof (execBeforeTariffRatesGridInsert) !== 'undefined' && ngl.isFunction(execBeforeTariffRatesGridInsert)) {

                        if (execBeforeTariffRatesGridInsert() === true) {
                            openAddNewTariffRatesGridWindow();
                        }
                    } else {
                        openAddNewTariffRatesGridWindow();
                    }
                } else if (btn.id === "btnImportSpecificTariffRates") {
                    location.href = "TariffRatesMatrix";
                    //openImportTariffRatesWnd(PageControl);
                } else if (btn.id == "btnOpenExceptions") {
                    location.href = "TariffExceptions";
                } else if (btn.id == "btnOpenFees") {
                    location.href = "TariffFees";
                } else if (btn.id == "btnOpenFuel") {
                    location.href = "TariffFuel";
                } else if (btn.id == "btnOpenNoDrive") {
                    location.href = "TariffNoDriveDays";
                } else if (btn.id == "btnOpenHDMs") {
                    location.href = "TariffHDMs";
                } else if (btn.id == "btnOpenBreakPoints") {
                    location.href = "TariffBreakPoints";
                } else if (btn.id == "btnRefresh") {
                    wdgtvCarrierTariffSummarySummary.read(0);
                    oTariffRatesGrid.dataSource.read();
                }
                if (btn.id == "btnResetCurrentUserConfig") {
                    resetCurrentUserConfig(PageControl);
                }
            }

            function getBreakPointSuccessCallback(data) {
                //debugger;
                try {
                    if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                        if (typeof (data.Errors) !== 'undefined' && data.Errors != null) {
                            ngl.showErrMsg("Read Break Point Header Failure ", data.Errors, tPage);
                        }
                        else {
                            if (data.Data != null && ngl.isArray(data.Data) == true) {
                                var container = oTariffRatesGrid.wrapper
                                for (var i = 1; i < 11; i++) {
                                    var sCaption = data.Data[0]["Val" + i.toString()];
                                    if (typeof (sCaption) !== 'undefined' && sCaption != null) {
                                        var head = container.find("[data-field='Val" + i.toString() + "'] .k-link")
                                        if (typeof (head) !== 'undefined' && head !== null) {
                                            head.html(sCaption);
                                        }
                                        //update the field array data if found
                                        if (typeof (objTariffRatesGridDataFields) !== 'undefined' && objTariffRatesGridDataFields != null) {
                                            //var pdataFields = objTariffRatesGridDataFields; //wdgtTariffRatesGridEdit.dataFields
                                            $.each(objTariffRatesGridDataFields, function (index, item) {
                                                if (item.fieldName == "Val" + i.toString()) {
                                                    item.fieldCaption = "Break Point " + sCaption;
                                                }
                                            });
                                        }
                                    }

                                }

                            }
                        }
                    }
                } catch (err) {
                    ngl.showErrMsg("Read Break Point Header Failure", err, tPage);
                }
            }

            function getBreakPointAjaxErrorCallback(results) {
                //for now do nothing when we save the pk
                ngl.showErrMsg("Read Break Point Header Failure", formatAjaxJSONResponsMsgs(xhr, textStatus, error, 'Failed'), tPage);
            }

            function reloadPage() {
                 setTimeout(function () {
                     location.reload();
                }, 100,tObj);
            }

            function refreshTariffRatesGrid() {
             
               wdgtvCarrierTariffSummarySummary.read(0);
               oTariffRatesGrid.dataSource.read();                
            }

            //*************  Call Back Functions ****************
            function TariffRatesGridDataBoundCallBack(e, tGrid) {               
                oTariffRatesGrid = tGrid;
                var CarrTarEquipMatCarrTarMatBPControl = 0;
                var data = oTariffRatesGrid.dataSource.data();
                //debugger;
                if (typeof (data) !== 'undefined' && data !== null) {
                    var row = data[0];
                    if (typeof (row) !== 'undefined' && row !== null) {
                        if ("CarrTarEquipMatCarrTarMatBPControl" in row) {
                            CarrTarEquipMatCarrTarMatBPControl = row.CarrTarEquipMatCarrTarMatBPControl;
                        }
                    }

                }
                if (CarrTarEquipMatCarrTarMatBPControl != 0) {
                    var oCRUDCtrl = new nglRESTCRUDCtrl();
                    var blnRet = oCRUDCtrl.read("TariffRateClass/GetBreakPoint", CarrTarEquipMatCarrTarMatBPControl, tPage, "getBreakPointSuccessCallback", "getBreakPointAjaxErrorCallback", true);

                }
            }


            $(document).ready(function () {
                var PageMenuTab = <%=PageMenuTab%>;

             if (control != 0) {
                 //setTimeout(function () {
                 //    //add code here to load screen specific information this is only visible when a user is authenticated
                 //}, 1,this);

             }
             var PageReadyJS = <%=PageReadyJS%>

                 menuTreeHighlightPage(); //must be called after PageReadyJS
             var divWait = $("#h1Wait");
             if (typeof (divWait) !== 'undefined') {
                 divWait.hide();
             }

         });


        </script>
        <style>
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
        </style>

    </div>
</body>
</html>
