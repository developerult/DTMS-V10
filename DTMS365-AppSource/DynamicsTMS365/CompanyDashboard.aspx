<%@ Page Language="C#" Title="Company Dashboard" AutoEventWireup="true" CodeBehind="CompanyDashboard.aspx.cs" Inherits="DynamicsTMS365.CompanyDashboard" %>

<!DOCTYPE html>

<html>
    <head >
        <title>DTMS Company Dashboard</title>
         <%=cssReference%>  
        <style>
html,

body
{
    height:100%;
    margin:0;
    padding:0;
}

html
{font-size: 12px; font-family: Arial, Helvetica, sans-serif; overflow:hidden;}

.flex-row-container {
    display: flex;
    flex-wrap: wrap;
    align-items: center;
    justify-content: center;
    margin:15px 10px;
}
.flex-row-container > .flex-row-item {
    flex: 1 1 25%; /*grow | shrink | basis */
    margin:5px;
}

.flex-row-item {
  border: 1px solid #f76707;
  display: flex;
  flex-direction: column;
  align-items: center;
}

.flex-row-item span {
    margin: 10px 0;
    font: 16px Arial, Helvetica, sans-serif;
    color: #8e8e8e;

}

.flex-row-container-chart {
    display: flex;
    flex-wrap: wrap;
    align-items: center;
    justify-content: center;
    margin:10px;
}
.flex-row-container-chart > .flex-row-item-chart {
    flex: 1 1 45%; /*grow | shrink | basis */
    margin:5px;
}

@media screen and (max-width: 765px) {
  .flex-row-container-chart > .flex-row-item-chart {
    flex: 1 1 50%; /*grow | shrink | basis */
    margin:5px;
}
}

.flex-row-item-chart {
  border: 1px solid #f76707;
}
</style>
    </head>
    <body>                     
        <%=jssplitter2Scripts%> 
        <%=sWaitMessage%>              
      <div id="example" style="height: 100%; width: 100%;  margin-top: 2px;">
             <div id="vertical" style="height: 98%; width: 98%; " >                 
                <div id="menu-pane" style="height: 100%; width: 100%; background-color: white; ">
                    <div id="tab" class="menuBarTab"></div>
                </div>
                <div id="top-pane">
                  <div id="horizontal" style="height: 100%; width: 100%; ">
                        <div id="left-pane">
                            <div class="pane-content">
                             <% Response.Write(MenuControl); %>
                            <div id="menuTree"></div>                                
                            </div>
                        </div>
                        <div id="center-pane">                           
                            <% Response.Write(PageErrorsOrWarnings); %>    
                            <div style="display:none">
                                <div id="CompanyDashboardContent" class="ngl-blueBorder" >
                                   <div style="padding: 10px;">
                                       <div id="editableCont"></div>
                                   </div>
                                </div>
                                <div id="CompanyDashboardEditor">
                                    <textarea id="ContEditor" style="height: 90%; width: 90%;"></textarea>
                                    <input id="edPgDet" type="hidden" />
                                </div>
                                <br />
                            </div>

                            <h3 style="text-align:center; margin: 20px;">Top 10 Customer Delivery Appointment Performance Ranked by Weight</h3>
                            <div style="text-align:center;"><button class='export-pdf k-button'>Export as PDF</button></div>
                            <div class="flex-row-container">
                            <div class="flex-row-item">
                                <span id="last365days">Last 365 Days</span>
                                <div id="grid0"></div>
                            </div>
                            <div class="flex-row-item">
                                <span id="last30days">Last 30 Days</span>
                                <div id="grid1"></div></div>
                            <div class="flex-row-item">
                                <span id="last7days">Last 7 Days</span>
                                <div id="grid2"></div></div>
                            </div>
                             <div class="flex-row-container-chart">
                              <div class="flex-row-item-chart"><div id="barChart"></div></div>
                              <div class="flex-row-item-chart"><div id="pieChart"></div></div>
                             </div>

                        </div>
                    </div>
                </div>
                <div id="bottom-pane" style="height: 100%; width: 100%; background-color: #daecf4; ">
                    <div class="pane-content">
                        <% Response.Write(PageFooterHTML); %> 
                    </div>
                </div>
            </div>

    <%-- HelpWindow needs to go after AuthNotification because it uses the notification object created there --%>
    <% Response.Write(AuthLoginNotificationHTML); %>   
    <% Response.WriteFile("~/Views/HelpWindow.html"); %>  
    <script>
         <% Response.Write(ADALPropertiesjs); %>

        var PageControl = '<%=PageControl%>';
        var tObj = this;
        var tPage = this;

        <% Response.Write(NGLOAuth2); %>

        var resGetCompanyDashboardEditableContent = function (data) {
            //set the value of the editor and the html page content
            $("#ContEditor").data("kendoEditor").value(data.Content);
            $("#editableCont").html(data.Content);

            $('#edPgDet').val(data.PageDetControl);
        }

        function execActionClick(btn, proc) {
            if (btn.id == "btnTMSContEdit") { editPageDynamicContent(); }
            else if (btn.id == "btnResetPaneSettings") { resetPaneSettings(); document.location = oredirectUri; }
        }

        function editPageDynamicContent() {
            $("#CompanyDashboardContent").hide();
            $("#CompanyDashboardEditor").show();
            $("#ContEditor").data("kendoEditor").refresh();
        }

        function getCompanyDashboardEditableContent() {
            var rt = new editorContent();
            rt.PageControl = PageControl;
            rt.USec = 0;
            rt.EditorName = "ContEditor";
            rt.Content = "";
            rt.PageDetControl = $('#edPgDetRt').val();

            getEditorContentNoAuth(JSON.stringify(rt), resGetCompanyDashboardEditableContent);
        }

        var resSaveCompanyDashboardContentEditor = function (data) {
            //set the html page content
            var c = $("#ContEditor").data("kendoEditor").value();
            $("#editableCont").html(c);

            $("#CompanyDashboardEditor").hide();
            $("#CompanyDashboardContent").show();
        }

        function saveCompanyDashboardContentEditor() {
            var h = new editorContent();
            h.PageControl = PageControl;
            h.USec = localStorage.NGLvar1452;
            h.EditorName = "ContEditor";
            h.Content = $("#ContEditor").data("kendoEditor").value();
            h.PageDetControl = $('#edPgDet').val();

            saveEditorContent(h, resSaveCompanyDashboardContentEditor);
        }

        function cancelCompanyDashboardContentEditor() {
            getCompanyDashboardEditableContent();
            $("#CompanyDashboardEditor").hide();
            $("#CompanyDashboardContent").show();
        }

        $(document).ready(function () {
            var PageMenuTab = <%=PageMenuTab%>;

            setTimeout(function () { var PageReadyJS = <%=PageReadyJS%>; }, 10, this);
            setTimeout(function () {
                menuTreeHighlightPage(); //must be called after PageReadyJS

                $(".export-pdf").click(function () {
                    // Convert the DOM element to a drawing using kendo.drawing.drawDOM
                    $(".k-splitter .k-scrollable").css("overflow", "overlay");
                    $(".export-pdf").hide();

                    kendo.drawing.drawDOM($("#center-pane"))
                            .then(function (group) {
                            return kendo.drawing.exportPDF(group, {
                                paperSize: "auto",
                                margin: { left: "1cm", top: "1cm", right: "1cm", bottom: "1cm" }, multiPage: true
                            });
                        })
                        .done(function (data) {
                            // Save the PDF file
                            kendo.saveAs({
                                dataURI: data,
                                fileName: "CompanyDashboard.pdf",
                                proxyURL: "https://demos.telerik.com/kendo-ui/service/export"
                            });
                        });

                    $(".k-splitter .k-scrollable").css("overflow", "auto");
                    $(".export-pdf").show();
                });

                $("#CompanyDashboardEditor").hide();
                $("#CompanyDashboardContent").show();
                getCompanyDashboardEditableContent();

                var divWait = $("#h1Wait");
                if (typeof (divWait) !== 'undefined') {
                    divWait.hide();
                }
            }, 10, this);

            var date = new Date();
            date.setDate(date.getDate() - 7);

            var dateToday = new Date();
            var dateTodayText = kendo.toString(dateToday, "d"); //dateToday.getDate() + "/" + (dateToday.getMonth() + 1) + "/" + dateToday.getFullYear();

            var dateText7days = kendo.toString(date, "d");  //date.getDate() + "/" + (date.getMonth() + 1) + "/" + date.getFullYear();

            $('#last7days').append(` - ${dateText7days} - ${dateTodayText}`);

            date = new Date();
            date.setDate(date.getDate() - 30);
            var dateText30days = kendo.toString(date, "d"); 

            $('#last30days').append(` - ${dateText30days} - ${dateTodayText}`);

            date = new Date();
            date.setDate(date.getDate() - 365);
            var dateText365days = kendo.toString(date, "d");  // date.getDate() + "/" + (date.getMonth() + 1) + "/" + date.getFullYear();

            $('#last365days').append(` - ${dateText365days} - ${dateTodayText}`)

        });

        function createGrid(id, data) {
            $(id).kendoTreeList({
                dataBound: function () {
                    for (var i = 1; i < this.columns.length; i++) {
                        this.autoFitColumn(i);
                    }
                },
                columns: [
                    {
                        field: "CompName",
                        title: "Delivery Location",
                        footerTemplate: "Overall Totals:"
                    },
                    {
                        field: "TotalWgt",
                        title: "Weight",
                        footerTemplate: "#: kendo.toString(sum, '\\#\\#,\\#') #",
                        format: "{0:##,#}",
                        headerAttributes: {
                            style: "text-align: center"
                        }
                    },
                    {
                        field: "TotalOrders", title: "Ord", footerTemplate: "#: kendo.toString(sum, '\\#\\#,\\#') #", format: "{0:##,#}",
                        headerAttributes: {
                            style: "text-align: center"
                        } },
                    {
                        field: "TotalOnTime", title: "OT", footerTemplate: "#: kendo.toString(sum, '\\#\\#,\\#') #", format: "{0:##,#}",
                        headerAttributes: {
                            style: "text-align: center"
                        } },
                    {
                        field: "TotalLate", title: "Late", footerTemplate: "#: kendo.toString(sum, '\\#\\#,\\#') #", format: "{0:##,#}",
                        headerAttributes: {
                            style: "text-align: center"
                        } },
                    {
                        field: "OTLPerc", title: "OT %", footerTemplate: "#=kendo.format('{0:p}', average / 100)#", format: '{0} %',
                        headerAttributes: {
                            style: "text-align: center"
                        } },
                ],
                dataSource: {
                    data: data,
                    aggregate: [
                        { field: "TotalWgt", aggregate: "sum" },
                        { field: "TotalOrders", aggregate: "sum" },
                        { field: "TotalOnTime", aggregate: "sum" },
                        { field: "TotalLate", aggregate: "sum" },
                        { field: "OTLPerc", aggregate: "average" }
                    ]
                }
            });  
        }     

        function createCharts(data) {

            let newOrder = data.sort((a, b) => a.OTLPerc < b.OTLPerc ? 1 : -1);
            let barDataValues = [];
            let barDataCompanies = [];

            $.each(newOrder, function (index, value) {
                    barDataValues.push(value.OTLPerc);
                    barDataCompanies.push(value.CompName);
                });

            $("#barChart").kendoChart({
                title: {
                    text: "Rolling 365 Days On Time Delivery Appointment Performance For Top 10 Customers"
                },
                series: [{
                    type: "bar",
                    data: barDataValues,
                    labels: {
                        format: "{0}%",
                        visible: true,
                        position: "insideEnd"
                    }
                }],
                categoryAxis: {
                    categories: barDataCompanies
                },
                valueAxis: [{
                    labels: {
                        format: "{0}%"
                    },
                    max: 100
                }]
            });

            const averageOnTime = (barDataValues.reduce((acc, c) => acc + c, 0)) / barDataValues.length;

            var pieData = [
                {
                    "source": "Late",
                    "percentage": 100 - averageOnTime,
                    "explode": true,
                    "color": "orange"
                },
                {
                    "source": "On Time",
                    "percentage": averageOnTime,
                    "color": "green"
                }
            ];


            $("#pieChart").kendoChart({
                title: {
                    text: "Rolling 365 Days On Time Delivery Appointment Performance For Customers Overall"
                },
                legend: {
                    visible: false
                },
                dataSource: {
                    data: pieData
                },
                series: [{
                    type: "pie",
                    field: "percentage",
                    categoryField: "source",
                    explodeField: "explode",

                    labels: {
                        format: "{0:##,#}dd%",
                        template: "${ category } #: kendo.toString(value, '\\#\\#,\\#\\#') #%",
                        visible: true,
                        position: "center"
                    }
                }],
                seriesColors: ["#03a9f4", "#ff9800", "#fad84a", "#4caf50"]
            });
        }

        $(document).ready(() =>
            $.ajax({
            async: true,
            type: 'GET',
            url: `/api/CompDashboard/GetCompDashboard`,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                success: function (data) {

                    for (var i = 0; i < 3; i++) {
                        createGrid(`#grid${i}`, data.Data[i]);
                    }

                    createCharts(data.Data[0]);
            }
            }));

        $(document).bind("kendo:skinChange", createGrid);
        $(document).bind("kendo:skinChange", createCharts);

    </script>
    <style>


    </style>
    </div>


    </body>

</html>
