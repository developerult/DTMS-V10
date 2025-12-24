<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TariffRatesMatrix.aspx.cs" Inherits="DynamicsTMS365.TariffRatesMatrix" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>DTMS Tariff Rates Matrix</title>
    
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
    </style>
</head>
<body>  
    <script src="https://code.jquery.com/jquery-3.4.1.min.js"></script>
    <script src="Scripts/kendoR32023/kendo.all.min.js"></script>
    <script>kendo.ui['Button'].fn.options['size'] = "small";</script>
    <script src="Scripts/Stuk-jszip/dist/jszip.min.js"></script>
    <script src="https://secure.aadcdn.microsoftonline-p.com/lib/1.0.14/js/adal.min.js"></script>
    <script src="Scripts/NGL/v-8.5.4.006/core.js"></script>
    <script src="Scripts/NGL/v-8.5.4.006/NGLobjects.js"></script>
    <script src="Scripts/NGL/v-8.5.4.006/NGLCtrls.js"></script>
    <script src="Scripts/NGL/v-8.5.4.006/splitter2.js"></script>
    <script src="Scripts/NGL/v-8.5.4.006/SSOA.js"></script>    
    <div id="example" style="height: 100%; width: 100%;  margin-top: 2px; overflow:scroll;">
        <div style="height: auto; width: auto; margin: 2px; padding:2px; " >    
            <div style="height: auto; width: 98%; margin:2px; padding:2px; "> 
        <div class="ngl-blueBorder"  style="height: auto; width:95%; margin: 10px; padding:10px;">
            <div >
                <a id="focusCancel" href="#"></a>
                <div id="trWait" style="margin: 10px; z-index: 99999; top: 10px; left: 10px; display: none;"><span style="vertical-align:middle;">Working Please Wait&nbsp;</span><img style="vertical-align:middle;" border="0" alt="Waiting" src="../Content/NGL/loading5.gif" /></div>
                <div id="summaryInfo" class="k-block k-info-colored">Summary</div>
                <br />
                <div id="tools">
                    <button type="button" id="export" onclick="exportToExcel()">Export</button>
                    &nbsp
                    <button type="button" id="clear" onclick="clearAllRates()">Clear</button>
                    &nbsp
                    <button type="button" id="delete" onclick="DeleteAllRates()">Delete</button>
                    &nbsp
                    <button type="button" id="save" onclick="SaveAllRates()">Save</button>
                    &nbsp
                    <button type="button" id="rates" onclick="location.href = 'TariffRates';">Go to Rates</button>
                </div>

                <p>Please enter the Tarrif Rates or enter the number of rows to import. select all rows,  copy from excel, and paste using (Ctrl + V). On error check number of rows</p>
                <p>Note: Expect to wait for large numbers of rows; processing time may not be reported.</p>
                <hr />
                <div style="display: flex;">
                    <div>From <input  id="EffectiveDate" title="datepicker" style="width: 70%" /></div>
                    <div>To: <input id="EffectiveTo" title="datepicker" style="width: 70%" /></div>
                    <div>Rows: <input id="txtRows" title="rowPicker" style="width: 70%" value="300" /></div>
                    <div><button type="button" id="setdRows" onclick="setSheetRows()">Set Rows</button></div>
                </div>
                <hr />
                <div id="TariffRatesMatrixGrid" style="width: auto"></div>
            </div>
            <div>
                <div id="exportToExcelGrid" style="display: none"></div>
            </div>
        </div>        
        <div >
            <div id="spreadsheet" style="width: auto;"></div>
        </div>
            </div>
            </div>
        <% Response.Write(AuthLoginNotificationHTML); %>

        <script>
            var dsActiveCarriers = kendo.data.DataSource;
            var simplesource = [];
            var datasource = kendo.data.DataSource;
            //var wndTariffRatesMatrix = kendo.ui.Window;
            var AllRecords;
            var AllRecordsData;
           
            var PageControl = '<%=PageControl%>';
            var CarrTarEquipMatCarrTarControl;
            var CarrTarEquipMatCarrTarMatBPControl;
            var CarrTarEquipMatName;
            var CarrTarEquipControl;
            var iRows = 300;
            var tObj = this;
            var tPage = this;
            var values;
            var changevariable = [];
            var countforchange = 0;


            var vCarrTarEquipMatPivotDetailDataJsonSet = (values, index) => ({
                CarrTarEquipMatOrigZip: values[index][0],
                CarrTarEquipMatCountry: values[index][1],
                CarrTarEquipMatState: values[index][2],
                CarrTarEquipMatCity: values[index][3],
                CarrTarEquipMatFromZip: values[index][4] ? values[index][4].toString() : '',
                CarrTarEquipMatToZip: values[index][5] ? values[index][5].toString() : '',
                LaneNumber: values[index][6],
                CarrTarEquipMatClass: values[index][7],
                CarrTarEquipMatMin: values[index][8],
                CarrTarEquipMatMaxDays: values[index][9],
                Val1: values[index][10],
                Val2: values[index][11],
                Val3: values[index][12],
                Val4: values[index][13],
                Val5: values[index][14],
                Val6: values[index][15],
                Val7: values[index][16],
                Val8: values[index][17],
                Val9: values[index][18],
                Val10: values[index][19]
            });

            var vCarrTarEquipMatPivotDetailOrigDataJsonSet = (values, index) => ({
                CarrTarEquipMatOrigZip: values[index][0] ? values[index][0].toString() : '', // Modified by RHR for v-8.5.4.001 on 06/14/2023 new Origin Rate shop logic
                CarrTarEquipMatCountry: values[index][1],
                CarrTarEquipMatState: values[index][2],
                CarrTarEquipMatCity: values[index][3],
                CarrTarEquipMatFromZip: values[index][4] ? values[index][4].toString() : '',
                CarrTarEquipMatToZip: values[index][5] ? values[index][5].toString() : '',
                LaneNumber: values[index][6],
                CarrTarEquipMatClass: values[index][7],
                CarrTarEquipMatMin: values[index][8],
                CarrTarEquipMatMaxDays: values[index][9],
                Val1: values[index][10],
                Val2: values[index][11],
                Val3: values[index][12],
                Val4: values[index][13],
                Val5: values[index][14],
                Val6: values[index][15],
                Val7: values[index][16],
                Val8: values[index][17],
                Val9: values[index][18],
                Val10: values[index][19]
            });

            var vCarrTarEquipMatPivotTruckLoadDataJsonSet = (values, index) => ({
                CarrTarEquipMatOrigZip: values[index][0] ? values[index][0].toString() : '',
                CarrTarEquipMatCountry: values[index][1],
                CarrTarEquipMatState: values[index][2],
                CarrTarEquipMatCity: values[index][3],
                CarrTarEquipMatFromZip: values[index][4] ? values[index][4].toString() : '',
                CarrTarEquipMatToZip: values[index][5] ? values[index][5].toString() : '',
                LaneNumber: values[index][6],
                CarrTarEquipMatMin: values[index][7],
                CarrTarEquipMatMaxDays: values[index][8],
                Rate: values[index][9]
            });


            var vCarrTarEquipMatPivotTruckLoadOrigDataJsonSet = (values, index) => ({
                CarrTarEquipMatOrigZip: values[index][0] ? values[index][0].toString() : '', // Modified by RHR for v-8.5.4.001 on 06/14/2023 new Origin Rate shop logic
                CarrTarEquipMatCountry: values[index][1],
                CarrTarEquipMatState: values[index][2],
                CarrTarEquipMatCity: values[index][3],
                CarrTarEquipMatFromZip: values[index][4] ? values[index][4].toString() : '',
                CarrTarEquipMatToZip: values[index][5] ? values[index][5].toString() : '',
                LaneNumber: values[index][6],
                CarrTarEquipMatMin: values[index][7],
                CarrTarEquipMatMaxDays: values[index][8],
                Rate: values[index][9],
            });

            var vCarrTarEquipMatPivotTruckLoadRowsSet = [
                {
                    height: 25,
                    cells: [
                        {
                            value: "Orig Zip/PC", background: "rgb(167,214,255)", bold: "true", textAlign: "center", color: "rgb(0,62,117)",
                        },
                        {
                            value: "Country", background: "rgb(167,214,255)", bold: "true", textAlign: "center", color: "rgb(0,62,117)",
                        },
                        {
                            value: "State", background: "rgb(167,214,255)", bold: "true", textAlign: "center", color: "rgb(0,62,117)",
                        },
                        {
                            value: "City", background: "rgb(167,214,255)", bold: "true", textAlign: "center", color: "rgb(0,62,117)",
                        },
                        {
                            value: "Start Zip/PC", bold: "true", background: "rgb(167,214,255)", textAlign: "center", color: "rgb(0,62,117)",
                        },
                        {
                            value: "Stop Zip/PC", bold: "true", background: "rgb(167,214,255)", textAlign: "center", color: "rgb(0,62,117)",
                        },
                        {
                            value: "Lane Number", bold: "true", background: "rgb(167,214,255)", textAlign: "center", color: "rgb(0,62,117)"
                        },
                        {
                            value: "Min Value", bold: "true", background: "rgb(167,214,255)", textAlign: "center", color: "rgb(0,62,117)"
                        },
                        {
                            value: "Max Trans Days", bold: "true", background: "rgb(167,214,255)", textAlign: "center", color: "rgb(0,62,117)"
                        },
                        {
                            value: "Per Mile", bold: "true", background: "rgb(167,214,255)", textAlign: "center", color: "rgb(0,62,117)"
                        }
                    ]
                },
            ];

            var vCarrTarEquipMatPivotTruckLoadOrigRowsSet = [
                {
                    height: 25,
                    cells: [
                        {
                            value: "Orig Zip/PC", background: "rgb(167,214,255)", bold: "true", textAlign: "center", color: "rgb(0,62,117)",
                        },
                        {
                            value: "Country", background: "rgb(167,214,255)", bold: "true", textAlign: "center", color: "rgb(0,62,117)",
                        },
                        {
                            value: "State", background: "rgb(167,214,255)", bold: "true", textAlign: "center", color: "rgb(0,62,117)",
                        },
                        {
                            value: "City", background: "rgb(167,214,255)", bold: "true", textAlign: "center", color: "rgb(0,62,117)",
                        },
                        {
                            value: "Start Zip/PC", bold: "true", background: "rgb(167,214,255)", textAlign: "center", color: "rgb(0,62,117)",
                        },
                        {
                            value: "Stop Zip/PC", bold: "true", background: "rgb(167,214,255)", textAlign: "center", color: "rgb(0,62,117)",
                        },
                        {
                            value: "Lane Number", bold: "true", background: "rgb(167,214,255)", textAlign: "center", color: "rgb(0,62,117)"
                        },
                        {
                            value: "Min Value", bold: "true", background: "rgb(167,214,255)", textAlign: "center", color: "rgb(0,62,117)"
                        },
                        {
                            value: "Max Trans Days", bold: "true", background: "rgb(167,214,255)", textAlign: "center", color: "rgb(0,62,117)"
                        },
                        {
                            value: "Per Mile", bold: "true", background: "rgb(167,214,255)", textAlign: "center", color: "rgb(0,62,117)"
                        }
                    ]
                },
            ];

            var vCarrTarEquipMatPivotDetailLoadRowsSet = [
                {
                    height: 25,
                    cells: [
                        {
                            value: "Country", background: "rgb(167,214,255)", bold: "true", textAlign: "center", color: "rgb(0,62,117)",
                        },
                        {
                            value: "State", background: "rgb(167,214,255)", bold: "true", textAlign: "center", color: "rgb(0,62,117)",
                        },
                        {
                            value: "City", background: "rgb(167,214,255)", bold: "true", textAlign: "center", color: "rgb(0,62,117)",
                        },
                        {
                            value: "Start Zip/PC", bold: "true", background: "rgb(167,214,255)", textAlign: "center", color: "rgb(0,62,117)",
                        },
                        {
                            value: "Stop Zip/PC", bold: "true", background: "rgb(167,214,255)", textAlign: "center", color: "rgb(0,62,117)",
                        },
                        {
                            value: "Lane Number", bold: "true", background: "rgb(167,214,255)", textAlign: "center", color: "rgb(0,62,117)"
                        },
                        {
                            value: "Class", bold: "true", background: "rgb(167,214,255)", textAlign: "center", color: "rgb(0,62,117)"
                        },
                        {
                            value: "Min Value", bold: "true", background: "rgb(167,214,255)", textAlign: "center", color: "rgb(0,62,117)"
                        },
                        {
                            value: "Max Trans Days", bold: "true", background: "rgb(167,214,255)", textAlign: "center", color: "rgb(0,62,117)"
                        },
                        {
                            value: "500", bold: "true", background: "rgb(167,214,255)", textAlign: "center", color: "rgb(0,62,117)"
                        },
                        {
                            value: "1000", bold: "true", background: "rgb(167,214,255)", textAlign: "center", color: "rgb(0,62,117)"
                        },
                        {
                            value: "5000", bold: "true", background: "rgb(167,214,255)", textAlign: "center", color: "rgb(0,62,117)"
                        },
                        {
                            value: "10000", bold: "true", background: "rgb(167,214,255)", textAlign: "center", color: "rgb(0,62,117)"
                        },
                        {
                            value: "20000", bold: "true", background: "rgb(167,214,255)", textAlign: "center", color: "rgb(0,62,117)"
                        },
                        {
                            value: "50000", bold: "true", background: "rgb(167,214,255)", textAlign: "center", color: "rgb(0,62,117)"
                        },
                        {
                            value: "0", bold: "true", background: "rgb(167,214,255)", textAlign: "center", color: "rgb(0,62,117)"
                        },
                        {
                            value: "0", bold: "true", background: "rgb(167,214,255)", textAlign: "center", color: "rgb(0,62,117)"
                        },
                        {
                            value: "0", bold: "true", background: "rgb(167,214,255)", textAlign: "center", color: "rgb(0,62,117)"
                        },
                        {
                            value: "0", bold: "true", background: "rgb(167,214,255)", textAlign: "center", color: "rgb(0,62,117)"
                        }
                    ]
                },
            ];


            var vCarrTarEquipMatPivotDetailOrigLoadRowsSet = [
                {
                    height: 25,
                    cells: [
                        {
                            value: "Orig Zip/PC", background: "rgb(167,214,255)", bold: "true", textAlign: "center", color: "rgb(0,62,117)",
                        },
                        {
                            value: "Country", background: "rgb(167,214,255)", bold: "true", textAlign: "center", color: "rgb(0,62,117)",
                        },
                        {
                            value: "State", background: "rgb(167,214,255)", bold: "true", textAlign: "center", color: "rgb(0,62,117)",
                        },
                        {
                            value: "City", background: "rgb(167,214,255)", bold: "true", textAlign: "center", color: "rgb(0,62,117)",
                        },
                        {
                            value: "Start Zip/PC", bold: "true", background: "rgb(167,214,255)", textAlign: "center", color: "rgb(0,62,117)",
                        },
                        {
                            value: "Stop Zip/PC", bold: "true", background: "rgb(167,214,255)", textAlign: "center", color: "rgb(0,62,117)",
                        },
                        {
                            value: "Lane Number", bold: "true", background: "rgb(167,214,255)", textAlign: "center", color: "rgb(0,62,117)"
                        },
                        {
                            value: "Class", bold: "true", background: "rgb(167,214,255)", textAlign: "center", color: "rgb(0,62,117)"
                        },
                        {
                            value: "Min Value", bold: "true", background: "rgb(167,214,255)", textAlign: "center", color: "rgb(0,62,117)"
                        },
                        {
                            value: "Max Trans Days", bold: "true", background: "rgb(167,214,255)", textAlign: "center", color: "rgb(0,62,117)"
                        },
                        {
                            value: "500", bold: "true", background: "rgb(167,214,255)", textAlign: "center", color: "rgb(0,62,117)"
                        },
                        {
                            value: "1000", bold: "true", background: "rgb(167,214,255)", textAlign: "center", color: "rgb(0,62,117)"
                        },
                        {
                            value: "5000", bold: "true", background: "rgb(167,214,255)", textAlign: "center", color: "rgb(0,62,117)"
                        },
                        {
                            value: "10000", bold: "true", background: "rgb(167,214,255)", textAlign: "center", color: "rgb(0,62,117)"
                        },
                        {
                            value: "20000", bold: "true", background: "rgb(167,214,255)", textAlign: "center", color: "rgb(0,62,117)"
                        },
                        {
                            value: "50000", bold: "true", background: "rgb(167,214,255)", textAlign: "center", color: "rgb(0,62,117)"
                        },
                        {
                            value: "0", bold: "true", background: "rgb(167,214,255)", textAlign: "center", color: "rgb(0,62,117)"
                        },
                        {
                            value: "0", bold: "true", background: "rgb(167,214,255)", textAlign: "center", color: "rgb(0,62,117)"
                        },
                        {
                            value: "0", bold: "true", background: "rgb(167,214,255)", textAlign: "center", color: "rgb(0,62,117)"
                        },
                        {
                            value: "0", bold: "true", background: "rgb(167,214,255)", textAlign: "center", color: "rgb(0,62,117)"
                        }
                    ]
                },
            ];

            var dataJsonSet = {
                "57": vCarrTarEquipMatPivotTruckLoadDataJsonSet,
                "58": vCarrTarEquipMatPivotDetailDataJsonSet,
                "59": vCarrTarEquipMatPivotTruckLoadDataJsonSet,
                "60": vCarrTarEquipMatPivotDetailDataJsonSet,
                "162": vCarrTarEquipMatPivotTruckLoadOrigDataJsonSet,
                "163": vCarrTarEquipMatPivotDetailOrigDataJsonSet,
                "164": vCarrTarEquipMatPivotTruckLoadOrigDataJsonSet,
                "165": vCarrTarEquipMatPivotDetailOrigDataJsonSet,
            }

            var controllerName = {
                "57": "TariffRateDistance",
                "58": "TariffRateClass",
                "59": "TariffRateFlat",
                "60": "TariffRateUOM",
                "162": "TariffRateDistance",
                "163": "TariffRateClass",
                "164": "TariffRateFlat",
                "165": "TariffRateUOM",
            }

            //var spreadsheetColumnSelection = {
            //    "57": "A2:I10000",
            //    "58": "A2:R10000",
            //    "59": "A2:I10000",
            //    "60": "A2:R10000",
            //}

            //Modified by RHR for v-8.5.0.002 on 01/05/2022 added row variable
            var spreadsheetColumnSelection = {
                "57": "A2:J",
                "58": "A2:R",
                "59": "A2:I",
                "60": "A2:R",
                "162": "A2:J",
                "163": "A2:T",
                "164": "A2:J",
                "165": "A2:T",
            }

            var rowsSet = {
                "57": vCarrTarEquipMatPivotTruckLoadRowsSet,
                "58": vCarrTarEquipMatPivotDetailLoadRowsSet,
                "59": vCarrTarEquipMatPivotTruckLoadRowsSet,
                "60": vCarrTarEquipMatPivotDetailLoadRowsSet,
                "162": vCarrTarEquipMatPivotTruckLoadOrigRowsSet,
                "163": vCarrTarEquipMatPivotDetailOrigLoadRowsSet,
                "164": vCarrTarEquipMatPivotTruckLoadOrigRowsSet,
                "165": vCarrTarEquipMatPivotDetailOrigLoadRowsSet,

            }

            var columnsSet = {
                "57": [
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    }
                ],
                "58": [
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    }
                ],
                "59": [
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    }
                ],
                "60": [
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    }
                ],
                "162": [
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    }
                ],
                "163": [
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    }
                ],
                "164": [
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    }
                ],
                "165": [
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    },
                    {
                        width: 150
                    }
                ]
            }

            function setSheetRows() {
                var iNewRows = $("#txtRows").val();
                if (isNaN(iNewRows)) { iNewRows = 300; }
                iRows = iNewRows;
                GetAllData();
            }

            //function openImportTariffRatesWnd(iPageControl) {
            //    ActPageControl = iPageControl;
            //    wndTariffRatesMatrix.center().open();

               
            //}

        //function onRender(arg) {
        //    $("#trWait").show();
        //    var spreadsheetView = $('.k-spreadsheet-view');
        //    spreadsheetView.unbind('select');
        //    spreadsheetView.unbind('paste');

        //    spreadsheetView.on('paste', function (e) {
        //        var element = $('.k-spreadsheet');

        //        kendo.ui.progress(element, true);

        //        setTimeout(function () {
        //            onPaste(e);
        //            kendo.ui.progress(element, false);
        //        }, 1000);
        //    });
        //    spreadsheetView.on('select', function (e) {
        //        //var element = $('.k-spreadsheet');

        //        //kendo.ui.progress(element, true);

        //        //setTimeout(function () {
        //        //    kendo.ui.progress(element, false);
        //        //    alert("select");
        //        //}, 1000);
        //    });
        //}


        function onPaste(arg) {
            //values = arg.clipboardContent.data.map(function (row) {
            //    return row.map(function (cell) {
            //        return cell.value;summar
            //    });
            //});
            //$("#trWait").show();
            //var windowWidget = $("#wndTariffRatesMatrix").data("kendoWindow");
            //    kendo.ui.progress(windowWidget.element, true);
            // alert('processing');
            //setTimeout(function (tObj) {
            //debugger;
            //values = arg.clipboardContent.data.map(function (row) {
            //    return row.map(function (cell) {
            //        console.log('cell value');
            //        console.log(cell.value);
            //        return cell.value;

            //    });
            //});

            /*kendo.ui.progress(windowWidget.element, false);*/
            //$("#trWait").hide();
            //}, 10, this);
        }

        //function onPaste(e) {
        //    e.preventDefault()

        //    var currentRange = e.range;
        //    var fullData = e.clipboardContent.data;
        //    var mergedCells = e.clipboardContent.mergedCells;
        //    var topLeft = currentRange.topLeft();
        //    var initialRow = topLeft.row;
        //    var initialCol = topLeft.col;
        //    var origRef = e.clipboardContent.origRef;
        //    var numberOfRows = origRef.bottomRight.row - origRef.topLeft.row + 1;
        //    var numberOfCols = origRef.bottomRight.col - origRef.topLeft.col + 1;
        //    var spread = e.sender;
        //    var sheet = spread.activeSheet();
        //    var rangeToPaste = sheet.range(initialRow, initialCol, numberOfRows, numberOfCols);
        //    console.log('Range: ' + initialRow + ', ' + initialCol + ', ' + numberOfRows + ', ' + numberOfCols)
        //    sheet.batch(function () {
        //        for (var i = 0; i < fullData.length; i += 1) {
        //            var currentFullData = fullData[i];

        //            for (var j = 0; j < currentFullData.length; j += 1) {
        //                var range = sheet.range(initialRow + i, initialCol + j);
        //                var value = currentFullData[j].value;

        //                if (value !== null) {
        //                    range.input(value);
        //                    range.format(null);
        //                }
        //            }
        //        }
        //    });

        //    sheet.select(rangeToPaste);

        //    for (var i = 0; i < mergedCells.length; i += 1) {
        //        var initialMergedRange = sheet.range(mergedCells[i]);
        //        var mergeTopLeft = initialMergedRange.topLeft();
        //        var mergeInitialRow = mergeTopLeft.row + initialRow;
        //        var mergedInitialCol = mergeTopLeft.col + initialCol;
        //        var mergedNumberOfRows = initialMergedRange.values.length;
        //        var mergedNumberOfCols = initialMergedRange.values()[0].length;

        //        sheet.range(mergeInitialRow, mergedInitialCol, mergedNumberOfRows, mergedNumberOfCols).merge();
        //    }
        //}

        function onChange(arg) {
            if (values == null) {
                values = "";
            }

            var sheet = $("#TariffRatesMatrixGrid").data("kendoSpreadsheet");
            changevariable.push(arg.range.value());
        }

        function GetAllData() {
            var element = $('.k-spreadsheet');
            if (element) {
                //Rob test progress issue
                kendo.ui.progress(element, true);
            }
            //console.log("read GetCompleteRecord");
            //console.log(`${controllerName[PageControl]}/GetCompleteRecord`);
            //var oCRUDCtrl = new nglRESTCRUDCtrl();
            //var blnRet = oCRUDCtrl.read(`${controllerName[PageControl]}/GetCompleteRecord`, 0, tPage, "GetTariffRatesMatrixSuccessCallback", "TariffRatesGetDataErrorCB", true);
            //this.read = function (
            var strRestService = `${controllerName[PageControl]}/GetCompleteRecord`;
            var intControl = 0;
            var successCallBack = "GetTariffRatesMatrixSuccessCallback";
            var errorCallBack = "TariffRatesGetDataErrorCB";
            var doasync = false
            var blnRet = true;
            if (typeof (doasync) === 'undefined' || doasync === null || doasync !== true) { doasync === false; }
            if (typeof (intControl) != 'undefined' && intControl != null) {
                try {
                    $.ajax({
                        async: doasync,
                        type: 'GET',
                        url: 'api/' + strRestService + '/' + intControl,
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                        success: function (data) { tObj[successCallBack](data); },
                        error: function (xhr, textStatus, error) { tObj[errorCallBack](xhr, textStatus, error); }
                    });
                } catch (err) {
                    blnRet = false;
                    ngl.showErrMsg("Read from Database Failure", err.message, tObj);
                }
            } else {
                blnRet = false;
                ngl.showErrMsg("Read from Database Failure", "The selected key value for the record cannot be empty", tObj);
            }
            //return blnRet;
            // }

            }


            function GetTariffSummary(CarrTarControl) {
                $('#summaryInfo').empty();
                if (typeof (CarrTarControl) != 'undefined' && CarrTarControl != null) {
                    try {
                        $.ajax({
                            async: true,
                            type: 'GET',
                            url: 'api/Tariff/GetCarrierTariffSummary/' + CarrTarControl,
                            contentType: "application/json; charset=utf-8",
                            dataType: 'json',
                            headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                            success: function (data) {
                                if (data.Errors != null) {
                                    if (data.StatusCode === 203) { ngl.showErrMsg("Authorization Timeout", data.Errors, null); }
                                    else { ngl.showErrMsg("Access Denied", data.Errors, null); }
                                } else {
                                    if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                        if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                            var summaryCellInfo = '<span>Carrier Name: ' + data.Data[0].CarrierName + '</span>&nbsp&nbsp<span> Comp Name: ' + data.Data[0].CompName + ' </span>&nbsp&nbsp<span>Tariff Name: ' + data.Data[0].CarrTarName + '</span>';
                                            $('#summaryInfo').append(`<div style="margin-right: 10px; font-weight: bold; width: auto; display: inline-flex">${summaryCellInfo}</div>`);
                                        }
                                    }
                                }
                            },
                            error: function (xhr, textStatus, error) {
                                var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure");
                                ngl.showErrMsg("Read Tariff Summary Error", sMsg, tObj);
                            }
                        });
                    } catch (err) {
                        ngl.showErrMsg("Read Tariff Summary Error", err.message, tObj);
                    }
                }
            }

        function GetTariffRatesMatrixSuccessCallback(data, errTitle) {
            try {
                var blnSuccess = false;
                var blnErrorShown = false;
                var strValidationMsg = "";
                //ngl.showInfoNotification('Processing GetTariffRatesMatrixSuccessCallback', 'GetTariffRatesMatrixSuccessCallback received ', tObj);

                var element = $('.k-spreadsheet');
                if (element) {
                    //Rob test progress issue
                    kendo.ui.progress(element, false);
                }
                //console.log("mat data");
                console.log(data);
                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                    if (ngl.stringHasValue(data.Errors)) {
                        blnErrorShown = true;
                        ngl.showErrMsg(errTitle, data.Errors, null);
                    }
                    else {
                        if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                            if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined') {
                                blnSuccess = true;
                                if (data.Data[0].CarrTarEquipMatControl == 0) {
                                    //console.log("No Equipment");
                                    ngl.showErrMsg('No Rates Found', 'Please check the tariff and service,  you may need to delete the service or roll back to a previous version.', tObj);
                                }
                                if (data.Data[0].CarrTarEquipMatCarrTarControl != 0) {
                                    GetTariffSummary(data.Data[0].CarrTarEquipMatCarrTarControl);
                                }
                                //else {
                                //    ngl.showInfoNotification('Processing Updated Rates', 'GetTariffRatesMatrixSuccessCallback received ' + data.Data.length + ' records', tObj);

                                //}
                                AllRecords = data.Data;
                                //console.log('All Records');
                                console.log(AllRecords);
                                for (var i = 0; i < data.Data.length; i++) {
                                    if (data.Data[i].CarrTarEquipMatCarrTarControl) {
                                        CarrTarEquipMatCarrTarControl = data.Data[i].CarrTarEquipMatCarrTarControl;
                                    }
                                    if (data.Data[i].CarrTarEquipMatCarrTarMatBPControl) {
                                        CarrTarEquipMatCarrTarMatBPControl = data.Data[i].CarrTarEquipMatCarrTarMatBPControl;
                                    }
                                    if (data.Data[i].CarrTarEquipMatName) {
                                        CarrTarEquipMatName = data.Data[i].CarrTarEquipMatName;
                                    }
                                    //CarrTarEquipControl
                                    if (data.Data[i].CarrTarEquipMatCarrTarEquipControl) {
                                        CarrTarEquipControl = data.Data[i].CarrTarEquipMatCarrTarEquipControl;
                                    }
                                    simplesource.push(data.Data[i].TariffRatesControl);
                                }
                            } else {
                                ngl.showErrMsg('No Rates Found', 'Please check the tariff and service,  you may need to delete the service or roll back to a previous version.', tObj);
                            }
                        }
                    }
                }

                if (AllRecords && ngl.isArray(AllRecords) && AllRecords.length > 0) {
                    //debugger;
                    console.log(AllRecords);

                    if (iRows < AllRecords.length) {
                        iRows = AllRecords.length;
                        $("#txtRows").val(iRows);
                    }

                    AllRecordsData = AllRecords.map(row => {

                        if (PageControl == '57' || PageControl == '59') {
                            return {
                                CarrTarEquipMatOrigZip: row.CarrTarEquipMatOrigZip,
                                CarrTarEquipMatCountry: row.CarrTarEquipMatCountry,
                                CarrTarEquipMatState: row.CarrTarEquipMatState,
                                CarrTarEquipMatCity: row.CarrTarEquipMatCity,
                                CarrTarEquipMatFromZip: row.CarrTarEquipMatFromZip ? row.CarrTarEquipMatFromZip.toString() : '',
                                CarrTarEquipMatToZip: row.CarrTarEquipMatToZip ? row.CarrTarEquipMatToZip.toString() : '',
                                LaneNumber: row.LaneNumber,
                                CarrTarEquipMatMin: row.CarrTarEquipMatMin,
                                CarrTarEquipMatMaxDays: row.CarrTarEquipMatMaxDays,
                                Rate: row.Rate
                            }
                        } else if (PageControl == '58' || PageControl == '60') {
                            return {
                                CarrTarEquipMatOrigZip: row.CarrTarEquipMatOrigZip,
                                CarrTarEquipMatCountry: row.CarrTarEquipMatCountry,
                                CarrTarEquipMatState: row.CarrTarEquipMatState,
                                CarrTarEquipMatCity: row.CarrTarEquipMatCity,
                                CarrTarEquipMatFromZip: row.CarrTarEquipMatFromZip ? row.CarrTarEquipMatFromZip.toString() : '',
                                CarrTarEquipMatToZip: row.CarrTarEquipMatToZip ? row.CarrTarEquipMatToZip.toString() : '',
                                LaneNumber: row.LaneNumber,
                                CarrTarEquipMatClass: row.CarrTarEquipMatClass,
                                CarrTarEquipMatMin: row.CarrTarEquipMatMin,
                                CarrTarEquipMatMaxDays: row.CarrTarEquipMatMaxDays,
                                Val1: row.Val1,
                                Val2: row.Val2,
                                Val3: row.Val3,
                                Val4: row.Val4,
                                Val5: row.Val5,
                                Val6: row.Val6,
                                Val7: row.Val7,
                                Val8: row.Val8,
                                Val9: row.Val9,
                                Val10: row.Val10
                            }
                        } else if (PageControl == '162' || PageControl == '164') {
                            //console.log('162 or 164 row');
                            //console.log(row);
                            return {
                                CarrTarEquipMatOrigZip: row.CarrTarEquipMatOrigZip, // Modified by RHR for v-8.5.4.001 on 06/14/2023 new Origin Rate shop logic
                                CarrTarEquipMatCountry: row.CarrTarEquipMatCountry,
                                CarrTarEquipMatState: row.CarrTarEquipMatState,
                                CarrTarEquipMatCity: row.CarrTarEquipMatCity,
                                CarrTarEquipMatFromZip: row.CarrTarEquipMatFromZip ? row.CarrTarEquipMatFromZip.toString() : '',
                                CarrTarEquipMatToZip: row.CarrTarEquipMatToZip ? row.CarrTarEquipMatToZip.toString() : '',
                                LaneNumber: row.LaneNumber,
                                CarrTarEquipMatMin: row.CarrTarEquipMatMin,
                                CarrTarEquipMatMaxDays: row.CarrTarEquipMatMaxDays,
                                Rate: row.Rate
                            }
                        } else if (PageControl == '163' || PageControl == '165') {
                            return {
                                CarrTarEquipMatOrigZip: row.CarrTarEquipMatOrigZip, // Modified by RHR for v-8.5.4.001 on 06/14/2023 new Origin Rate shop logic
                                CarrTarEquipMatCountry: row.CarrTarEquipMatCountry,
                                CarrTarEquipMatState: row.CarrTarEquipMatState,
                                CarrTarEquipMatCity: row.CarrTarEquipMatCity,
                                CarrTarEquipMatFromZip: row.CarrTarEquipMatFromZip ? row.CarrTarEquipMatFromZip.toString() : '',
                                CarrTarEquipMatToZip: row.CarrTarEquipMatToZip ? row.CarrTarEquipMatToZip.toString() : '',
                                LaneNumber: row.LaneNumber,
                                CarrTarEquipMatClass: row.CarrTarEquipMatClass,
                                CarrTarEquipMatMin: row.CarrTarEquipMatMin,
                                CarrTarEquipMatMaxDays: row.CarrTarEquipMatMaxDays,
                                Val1: row.Val1,
                                Val2: row.Val2,
                                Val3: row.Val3,
                                Val4: row.Val4,
                                Val5: row.Val5,
                                Val6: row.Val6,
                                Val7: row.Val7,
                                Val8: row.Val8,
                                Val9: row.Val9,
                                Val10: row.Val10
                            }
                        } else {
                            return {
                                CarrTarEquipMatOrigZip: row.CarrTarEquipMatOrigZip,
                                CarrTarEquipMatCountry: row.CarrTarEquipMatCountry,
                                CarrTarEquipMatState: row.CarrTarEquipMatState,
                                CarrTarEquipMatCity: row.CarrTarEquipMatCity,
                                CarrTarEquipMatFromZip: row.CarrTarEquipMatFromZip ? row.CarrTarEquipMatFromZip.toString() : '',
                                CarrTarEquipMatToZip: row.CarrTarEquipMatToZip ? row.CarrTarEquipMatToZip.toString() : '',
                                LaneNumber: row.LaneNumber,
                                CarrTarEquipMatMin: row.CarrTarEquipMatMin,
                                CarrTarEquipMatMaxDays: row.CarrTarEquipMatMaxDays,
                                Rate: row.Rate
                            }
                        }
                    });

                    $("#exportToExcelGrid").kendoGrid({
                        columns: dataJsonSet[PageControl],
                        dataSource: AllRecordsData
                    });

                    let localDataSource = new kendo.data.DataSource({
                        data: AllRecordsData,
                        change: function (e) {
                            //console.log('change');
                            //setTimeout(() => {
                            //    var sheet = $("#TariffRatesMatrixGrid").getKendoSpreadsheet().activeSheet();
                            //    if (PageControl == '57' || PageControl == '58' || PageControl == '59' || PageControl == '60') {
                            //        sheet.range('D2:D300').format('@')
                            //        sheet.range('E2:E300').format('@')
                            //    } else if (PageControl == '162' || PageControl == '163' || PageControl == '164' || PageControl == '165') {
                            //        sheet.range('A2:D300').format('@')
                            //        sheet.range('E2:D300').format('@')
                            //        sheet.range('F2:E300').format('@')
                            //    } else {
                            //        sheet.range('D2:D300').format('@')
                            //        sheet.range('E2:E300').format('@')
                            //    }
                            //});
                        },
                    });

                    createSpreadSheet(localDataSource);
                    removeContextMenuFromSpreadSheet();
                }

                if (blnSuccess === false && blnErrorShown === false) {
                    if (strValidationMsg.length < 1) { strValidationMsg = errTitle; }
                    ngl.showErrMsg(errTitle, strValidationMsg, null);
                }


            } catch (err) {
                ngl.showErrMsg('Unexpected  GetTariffRatesMatrixSuccessCallback Error', err.toString(), tObj);
            }
            $("#trWait").hide();
        }

        function removeContextMenuFromSpreadSheet() {
            $('#TariffRatesMatrixGrid').find('.k-spreadsheet-fixed-container').off('contextmenu');
        }

        //function getAllDataSuccessCallback(data) {
        //    GetTariffRatesMatrixSuccessCallback(data, "Get Tariff Rates Failure");
        //}

        function TariffRatesGetDataErrorCB(results) {
            ngl.showErrMsg("Get Tariff Rates Failure", 'TariffRatesGetDataErrorCB', tPage);

            $("#trWait").hide();
            //console.log("Ajax Call back Error");
            //console.log(results);
            var element = $('.k-spreadsheet');
            if (element) {
                //Rob test progress issue
                kendo.ui.progress(element, false);
            }
            ngl.showErrMsg("Get Tariff Rates Failure", formatAjaxJSONResponsMsgs(xhr, textStatus, error, 'Failed'), tPage);
        }
        var bSaveRatesMarix = false;
        function ClearSpreadsheet(obj) {
            if (obj == 1) {
                //$("#trWait").show();
                bSaveRatesMarix = true;
                //setTimeout(function () {
                DeleteCarrTarEquipMatRecords(true);
                /* SaveRatesMatrix();*/
                // $("#trWait").hide();
                //}, 10,tObj);
            }
        }

        function DeleteRates(obj) {
            if (obj == 1) {
                $("#trWait").show();
                bSaveRatesMarix = false;
                //setTimeout(function () {
                DeleteCarrTarEquipMatRecords();
                /* GetAllData();*/
                //$("#trWait").hide();
                //}, 10, tObj);
            }
        }

        function createSpreadSheet(localDataSource) {
            $(function () {
                $("#TariffRatesMatrixGrid").empty();
                $("#TariffRatesMatrixGrid").kendoSpreadsheet({
                    excel: {
                        proxyURL: "https://demos.telerik.com/kendo-ui/service/export"
                    },
                    toolbar: {
                        insert: false,
                        data: false
                    },
                    pdf: {
                        proxyURL: "https://demos.telerik.com/kendo-ui/service/export"
                    },
                    dataBound: function (e) {
                        console.log('Databound ');
                        var sheet = $("#TariffRatesMatrixGrid").getKendoSpreadsheet().activeSheet();
                        if (PageControl == '57' || PageControl == '58' || PageControl == '59' || PageControl == '60') {
                            sheet.range('D2:D' + iRows).format('@')
                            sheet.range('E2:E' + iRows).format('@')
                        } else if (PageControl == '162' || PageControl == '163' || PageControl == '164' || PageControl == '165') {
                            sheet.range('A2:D' + iRows).format('@')
                            sheet.range('E2:E' + iRows).format('@')
                            sheet.range('F2:F' + iRows).format('@')
                        } else {
                            sheet.range('D2:D' + iRows).format('@')
                            sheet.range('E2:E' + iRows).format('@')
                        }
                    },
                    //change: function (e) {
                    //    //setTimeout(() => {
                    //    //    var sheet = $("#TariffRatesMatrixGrid").getKendoSpreadsheet().activeSheet();
                    //    //    if (PageControl == '57' || PageControl == '58' || PageControl == '59' || PageControl == '60') {
                    //    //        sheet.range('D2:D300').format('@')
                    //    //        sheet.range('E2:E300').format('@')
                    //    //    } else if (PageControl == '162' || PageControl == '163' || PageControl == '164' || PageControl == '165') {
                    //    //        sheet.range('A2:D300').format('@')
                    //    //        sheet.range('E2:D300').format('@')
                    //    //        sheet.range('F2:E300').format('@')
                    //    //    } else {
                    //    //        sheet.range('D2:D300').format('@')
                    //    //        sheet.range('E2:E300').format('@')
                    //    //    }
                    //    //});
                    //},
                    render: function (e) {
                        var spreadsheetView = $('.k-spreadsheet-view');

                        spreadsheetView.unbind('paste');

                        spreadsheetView.on('paste', function (e) {
                            var element = $('.k-spreadsheet');

                            kendo.ui.progress(element, true);

                            setTimeout(function () {                                
                                    var sheet = $("#TariffRatesMatrixGrid").getKendoSpreadsheet().activeSheet();
                                    if (PageControl == '57' || PageControl == '58' || PageControl == '59' || PageControl == '60') {
                                        sheet.range('D2:D' + iRows ).format('@')
                                        sheet.range('E2:E' + iRows).format('@')
                                    } else if (PageControl == '162' || PageControl == '163' || PageControl == '164' || PageControl == '165') {
                                        sheet.range('A2:D' + iRows).format('@')
                                        sheet.range('E2:E' + iRows).format('@')
                                        sheet.range('F2:F' + iRows).format('@')
                                    } else {
                                        sheet.range('D2:D' + iRows).format('@')
                                        sheet.range('E2:E' + iRows).format('@')
                                    }
                                kendo.ui.progress(element, false);
                                //console.log('render end');
                            }, 1);
                        });
                    },
                    //render: function (e) {

                    //    var spreadsheetView = $('.k-spreadsheet-view');

                    //    spreadsheetView.unbind('select');

                    //    spreadsheetView.unbind('paste');

                    //    spreadsheetView.on('paste', function (e) {
                    //        // onPaste(e);
                    //        //$("#trWait").show();
                    //        //var element = $('.k-spreadsheet');

                    //        //kendo.ui.progress(element, true);

                    //        //setTimeout(function () {
                    //        //    //debugger;
                    //        //    //console.log('e');
                    //        //    //console.log(e);
                    //        //    //onPaste(e);

                    //        //    kendo.ui.progress(element, false);

                    //        //    $("#trWait").hide();
                    //        //}, 1, this);
                    //        //var fullData = e.clipboardContent.data;
                    //        //for (var i = 0; i < fullData.length; i += 1) {
                    //        //    var currentFullData = fullData[i];

                    //        //    for (var j = 0; j < currentFullData.length; j += 1) {
                    //        //        /*var range = sheet.range(initialRow + i, initialCol + j);*/
                    //        //        var value = currentFullData[j].value;
                    //        //        console.log('cell value');
                    //        //        console.log(value);
                    //        //        //if (value !== null) {
                    //        //        //    range.input(value);
                    //        //        //    range.format(null);
                    //        //        //}
                    //        //    }
                    //        //}
                    //        ////values = e.clipboardContent.data.map(function (row) {
                    //        ////    return row.map(function (cell) {
                    //        ////        console.log('cell value');
                    //        ////        console.log(cell.value);
                    //        ////        return cell.value;

                    //        ////    });
                    //        ////});
                    //    });
                    //    spreadsheetView.on('select', function (e) {
                    //        var element = $('.k-spreadsheet');
                    //        //Rob test progress issue
                    //        //kendo.ui.progress(element, true);
                    //        //Rob test progress issue
                    //        //setTimeout(function () {
                    //        //    kendo.ui.progress(element, false);
                    //        //}, 100, this);
                    //    });
                    //},
                    sheets: [
                        {
                            name: "Tariff Rates matrix",
                            cellRange: 10000,
                            dataSource: localDataSource,
                            rows: rowsSet[PageControl],
                            columns: columnsSet[PageControl],

                        },
                    ],
                    rows: iRows,
                });
                //}).data("kendoSpreadsheet").one("render", function (e) {
                //    e.sender.sheets().forEach(function (sheet) {
                //        sheet.range(0, rowsSet[PageControl][0].cells.length, sheet._rows._count, sheet._columns._count).enable(false);
                //    })
                //});
            });
        }



        function DeleteCarrTarEquipMatRecords(bDeleteAll) {


            var oCRUDCtrl = new nglRESTCRUDCtrl();
            var sURL = `${controllerName[PageControl]}/DeleteCarrTarEquipMatRates`;
            if (typeof (bDeleteAll) != 'undefined' && bDeleteAll != null && bDeleteAll == true) {
                var sURL = `${controllerName[PageControl]}/DeleteAllCarrTarEquipMatRates`;
            }
            var blnRet = oCRUDCtrl.delete(sURL, CarrTarEquipControl, tObj, "DeleteCarrTarEquipMatRecordsSuccessCallback", "DeleteCarrTarEquipMatRecordsAjaxErrorCallback", true);


        }


        function DeleteCarrTarEquipMatRecordsSuccessCallback(data) {

            try {
                var element = $('.k-spreadsheet');
                if (element) {
                    //Rob test progress issue
                    // kendo.ui.progress(element, false);
                }
            } catch (err) {
                ngl.showInfoNotification("Cancel Progress Failure", "Your process is complete but the wait cursor could not be turned off, please refresh the page if the wait cursor does not go away.", null)
            }
            try {
                if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                    if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                        if (data.StatusCode === 203) { ngl.showErrMsg("Authorization Timeout", data.Errors, tObj); }
                        else { ngl.showErrMsg("Access Denied", data.Errors, tObj); }
                    } else {
                        if (bSaveRatesMarix == true) {
                            SaveRatesMatrix();
                        } else {
                            GetAllData();
                        }

                    }
                }
            } catch (err) {
                ngl.showErrMsg(err.name, err.message, tObj);
            }
            $("#trWait").hide();
        }

        function DeleteCarrTarEquipMatRecordsAjaxErrorCallback(xhr, textStatus, error) {
            try {
                var element = $('.k-spreadsheet');
                if (element) {
                    //Rob test progress issue
                    //kendo.ui.progress(element, false);
                } else {
                    ngl.showErrMsg("No Progress Element", '.k-spreadsheet', tObj);
                }
            } catch (err) {
                ngl.showInfoNotification("Cancel Progress Failure", "Your process is complete but the wait cursor could not be turned off, please refresh the page if the wait cursor does not go away.", null)
            }
            ngl.showErrMsg("Delete Tariff Rates Details Failure", formatAjaxJSONResponsMsgs(xhr, textStatus, error, 'Failed'), tObj);
            GetAllData();
            $("#trWait").hide();
            }


            function updatingTariffRatesGridCB(data) {
                //console.log('updatingTariffRatesGridCB');

                //console.log('return data');
                //console.log(data);
               
                //console.log(data);
                var oResults = new nglEventParameters();
                oResults.source = "updatingTariffRatesGridCB";
                oResults.widget = tObj;
                oResults.msg = 'Failed'; //set default to Failed   

                try {
                    var element = $('.k-spreadsheet');
                    if (element) {
                        //Rob test progress issue
                        kendo.ui.progress(element, false);
                    }
                } catch (err) {
                    ngl.showInfoNotification("Cancel Progress Failure", "Your process is complete but the wait cursor could not be turned off, please refresh the page if the wait cursor does not go away.", null)
                }
                try {
                    var blnSuccess = false;
                    var blnErrorShown = false;
                    var strValidationMsg = "";
                    if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                        if (typeof (data.Errors) !== 'undefined' && data.Errors != null && data.Errors.length > 0) {
                            oResults.error = new Error();
                            oResults.error.name = "Save Tariff Rates Failure";
                            oResults.error.message = data.Errors;
                            blnErrorShown = true;
                            ngl.showErrMsg(oResults.error.name, oResults.error.message, null);
                        }
                        else {

                            if (typeof (data.Data) !== 'undefined' && data.Data != null && ngl.isArray(data.Data) && data.Data[0] == true) {
                                blnSuccess = true;
                               // $("#trWait").hide();
                                //console.log('calling upateGridAfterUpdate');
                                upateGridAfterUpdate();
                                //return;
                            } else {
                                blnErrorShown = true;
                                oResults.error = new Error();
                                oResults.error.name = "Unable to save your Tariff Rate changes";
                                oResults.error.message = "The save procedure returned false, please refresh your data and try again.";
                                ngl.showErrMsg(oResults.error.name, oResults.error.message, null);
                            }

                        }
                    }
                    //if (blnSuccess === false && blnErrorShown === false) {
                    //    oResults.error = new Error();
                    //    oResults.error.name = "Save Tariff Rate Failure";
                    //    oResults.error.message = "No results are available.";
                    //    ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);
                    //}
                } catch (err) {
                    oResults.error = err;
                    ngl.showErrMsg(err.name, err.message, null);

                }
                //$("#trWait").hide();
            }

            function SaveRatesMatrix() {
                var spreadsheet = $("#TariffRatesMatrixGrid").data("kendoSpreadsheet");
                var values = spreadsheet.sheets()[0].range(spreadsheetColumnSelection[PageControl] + iRows.toString()).values();
                var dataJson;
                if (values == null || values == "undefined" || values == undefined) {
                    ngl.showErrMsg("Update Tariff Rates", "Please paste the Tariff Rates (Ctrl + V) and try again", "");
                    return;
                }
                else {
                    if (values != "") {
                        //$("#trWait").show();
                        
                        var element = $('.k-spreadsheet');
                        if (element) {
                            //Rob test progress issue
                            kendo.ui.progress(element, true);
                        }

                        //Rob test progress issue
                        //kendo.ui.progress(element, true);

                        //console.log('Start Wait')
                        //setTimeout(function () {
                        //alert('Processing Changes');
                        //debugger;
                        var result = [];
                        //console.log('Values');
                        console.log(values);
                        
                        for (var i = 0; i < values.length; i++) {
                            //console.log('reading ' + i.toString() + ' of ' + values.length.toString());
                            if (values[i][0] != null) {
                                //console.log('Values: ');
                                console.log("Value: " + values[i][9]);
                                var objectBuilder = dataJsonSet[PageControl];                               
                                dataJson = objectBuilder(values, i);
                                
                                if (dataJson) {
                                    result.push(dataJson);
                                }
                            }
                        }
                       

                        if (result.length > 0) {
                            //console.log('call UpdatingTariffRatesGrid');
                            console.log(result);
                            UpdatingTariffRatesGrid(result);
                            return;
                        } else {
                            try {
                                var element = $('.k-spreadsheet');
                                if (element) {
                                    //Rob test progress issue
                                    //kendo.ui.progress(element, false);
                                }
                            } catch (err) {
                                // do nothing here
                            }
                            ngl.showErrMsg("Update Tariff Rates Failed", "No Data. Please paste the Tariff Rates (Ctrl + V) and try again", "");
                        }
                        //$("#trWait").hide();
                        //}, 1,tObj);
                    }
                }

                //console.log("spreadsheetColumnSelection[PageControl]" + iRows.toString());
                //console.log(PageControl);
                //console.log(spreadsheetColumnSelection[PageControl] + iRows.toString());
                if (spreadsheet.sheets()[0]) {
                    spreadsheet.sheets()[0].range(spreadsheetColumnSelection[PageControl] + iRows.toString()).clear();
                }

            }


            function upateGridAfterUpdate() {
                //window.location.reload();
                //return;
                //$("#trWait").hide();
                //ngl.showSuccessMsg("Tariff Rates updated successfully test.", null);
                
                GetAllData();
               
                //$("#wndTariffRatesMatrix").data("kendoWindow").refresh();
                // $("#wndTariffRatesMatrix").data("kendoWindow").close();
                //Window.loadWindow();

                //Rob Test nee to replace
                //refreshTariffRatesGrid();

                //GetAllData();

            }

            function updatingTariffRatesGridAjaxErrorCB(xhr, textStatus, error) {
                var oResults = new nglEventParameters();
                //console.log('updatingTariffRatesGridAjaxErrorCB');
                //var tObj = this;
                try {
                    var element = $('.k-spreadsheet');
                    if (element) {
                        //Rob test progress issue
                        kendo.ui.progress(element, false);
                    }
                } catch (err) {
                    ngl.showInfoNotification("Cancel Progress Failure", "Your process is complete but the wait cursor could not be turned off, please refresh the page if the wait cursor does not go away.", null)
                }
                //kendo.ui.progress($(document.body), false);
                oResults.source = "saveAjaxErrorCallback";
                oResults.widget = tObj;
                oResults.msg = 'Failed'; //set default to Failed 

                oResults.error = new Error();
                oResults.error.name = "Save " + this.DataSourceName + " Failure"
                oResults.error.message = formatAjaxJSONResponsMsgs(xhr, textStatus, error, oResults.msg);
                ngl.showErrMsg(oResults.error.name, oResults.error.message, tObj);

            }

            function UpdatingTariffRatesGrid(DATAJSON) {
                let url;
                let from = $("#EffectiveDate").val();
                let to = $("#EffectiveTo").val();
                

                if (to) {
                    url = `${controllerName[PageControl]}/PostSpreadSheet?CarrTarEquipMatCarrTarControl=${CarrTarEquipMatCarrTarControl}&CarrTarEquipMatCarrTarMatBPControl=${CarrTarEquipMatCarrTarMatBPControl}&CarrTarEquipMatName=${CarrTarEquipMatName}&EffectiveDate=${from}&EffectiveTo=${to}`;

                } else {
                    url = `${controllerName[PageControl]}/PostSpreadSheet?CarrTarEquipMatCarrTarControl=${CarrTarEquipMatCarrTarControl}&CarrTarEquipMatCarrTarMatBPControl=${CarrTarEquipMatCarrTarMatBPControl}&CarrTarEquipMatName=${CarrTarEquipMatName}&EffectiveDate=${from}`;
                }
                ////console.log('url');
                ////console.log(url);
                ////console.log('data');
                ////console.log(DATAJSON);
                
                //var oCRUDCtrl = new nglRESTCRUDCtrl();
                ////if (
                //oCRUDCtrl.update(url, DATAJSON, tObj, "updatingTariffRatesGridCB", "updatingTariffRatesGridAjaxErrorCB",false);
                //    //== false) {

                ////}
                //console.log('UpdatingTariffRatesGrid');
                //return;
                // Modified removed CRUDCtrl logic
                //this.update = function (
                var strRestService = url;
                var oChanges = DATAJSON;
                var uccessCallBack = "updatingTariffRatesGridCB"
                var errorCallBack = "updatingTariffRatesGridAjaxErrorCB";
                //console.log('oChanges');
                //console.log(JSON.stringify(oChanges));
                var doasync = false
                    var blnRet = true;
                    if (typeof (doasync) === 'undefined' || doasync === null || doasync !== true) { doasync = false; }
                    if (typeof (oChanges) !== 'undefined' && ngl.isObject(oChanges)) {
                        try {
                            //console.log('Changes');
                            //console.log(oChanges.BookModDate);
                            //console.log('Changes to JSON');
                            //console.log(JSON.stringify(oChanges));
                            $.ajax({
                                async: doasync,
                                type: 'POST',
                                url: 'api/' + strRestService,
                                contentType: 'application/json; charset=utf-8',
                                dataType: 'json',
                                data: JSON.stringify(oChanges),
                                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                                success: function (data) {     
                                    //console.log('updatingTariffRatesGridCB No Exit');
                                    //return;
                                    updatingTariffRatesGridCB(data);
                                    //tObj[successCallBack](data);
                                },
                                error: function (xhr, textStatus, error) { updatingTariffRatesGridAjaxErrorCB(xhr, textStatus, error); }
                            });
                        } catch (err) {
                            ngl.showErrMsg("Save to Database Failure", err.message, tObj);
                        }
                    } else {
                        blnRet = false;
                        ngl.showErrMsg("Save to Database Failure", "The changes to the record cannot be empty", tObj);
                    }
 
              
            }

             function exportToExcel() {
                 var grid = $("#exportToExcelGrid").data("kendoGrid");
                 grid.saveAsExcel();
                 e.preventDefault();
             }

            function clearAllRates() {
               // alert('Clear?');
             //e.preventDefault();
             ngl.OkCancelConfirmation(
                 "Replace all Rates",
                 "Are you sure?  All  rates for this tariff will be replaced with the data in the spreadsheet?",
                 400,
                 400,
                 tObj,
                 "ClearSpreadsheet");

         }

            function DeleteAllRates() {
                //alert('Delete?');
             //e.preventDefault();
             ngl.OkCancelConfirmation(
                 "Delete Tariff Rates",
                 "Are you sure?  All but one rate for this tariff will be deleted?",
                 400,
                 400,
                 tObj,
                 "DeleteRates");

         }

            function SaveAllRates() {
             //$("#trWait").show();
             //setTimeout(function () {
             SaveRatesMatrix();
             //$("#trWait").hide();
             //}, 100, this);
         }


            $(document).ready(function () {

                //$('a.k-window-action[aria-label="Save"]').prop('title', 'Save');
                //$('a.k-window-action[aria-label="Delete"]').prop('title', 'Delete');
                //$('a.k-window-action[aria-label="Strip-all-formatting"]').prop('title', 'Delete and save');
                //$('a.k-window-action[aria-label="Excel"]').prop('title', 'Export in Excel');

                $("#EffectiveDate").kendoDatePicker({ value: new Date() });
                $("#EffectiveTo").kendoDatePicker();

                //$('#summaryInfo').empty();

                //for (var i = 0; i < 3; i++) {
                //    let summaryCellInfo = $($($('[id ^=divid][id $=Summary]')[0]).find('div td')[i]).text();
                //    $('#summaryInfo').append(`<div style="margin-right: 10px; font-weight: bold; width: 32%; display: inline-flex">${summaryCellInfo}</div>`);
                //}

                GetAllData();


                $(".k-spreadsheet-sheets-bar-add .k-button .k-button-icon").hide();

                    //wndTariffRatesMatrix = $("#wndTariffRatesMatrix").kendoWindow({
                    //    title: "Import Tariff Rates",
                    //    width: "60%",
                    //    height: "80%",
                    //    actions: ["File-Excel", "Strip-all-formatting", "Trash", "Save", "Minimize", "Maximize", "Close"],
                    //    modal: true,
                    //    visible: false,
                    //    close: function (e) {
                    //        reloadPage();
                    //    }
                    //}).data("kendoWindow");

                    //$("#wndTariffRatesMatrix").data("kendoWindow").wrapper.find(".k-svg-i-file-excel").parent().click(function (e) {
                    //    var grid = $("#exportToExcelGrid").data("kendoGrid");
                    //    grid.saveAsExcel();
                    //    e.preventDefault();
                    //});

                    //$("#wndTariffRatesMatrix").data("kendoWindow").wrapper.find(".k-svg-i-strip-all-formatting").parent().click(function (e) {
                    //    e.preventDefault();
                    //    ngl.OkCancelConfirmation(
                    //        "Replace all Rates",
                    //        "Are you sure?  All  rates for this tariff will be replaced with the data in the spreadsheet?",
                    //        400,
                    //        400,
                    //        tObj,
                    //        "ClearSpreadsheet");

                    //});

                    //$("#wndTariffRatesMatrix").data("kendoWindow").wrapper.find(".k-svg-i-trash").parent().click(function (e) {
                    //    e.preventDefault();
                    //    ngl.OkCancelConfirmation(
                    //        "Delete Tariff Rates",
                    //        "Are you sure?  All but one rate for this tariff will be deleted?",
                    //        400,
                    //        400,
                    //        tObj,
                    //        "DeleteRates");

                    //});

                    //$("#wndTariffRatesMatrix").data("kendoWindow").wrapper.find(".k-svg-i-save").parent().click(function (e) {

                    //    //$("#trWait").show();
                    //    //setTimeout(function () {
                    //    Window.SaveRatesMatrix();
                    //    //$("#trWait").hide();
                    //    //}, 100, this);
                    //});



            });


        </script>
      <style>
        #TariffRatesMatrixGrid .k-tabstrip-wrapper {
            display: none;
        }

        .k-spreadsheet .k-spreadsheet-sheets-bar {
            display: none;
        }

        .k-spreadsheet .k-dirty {
            display: none;
        }
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
