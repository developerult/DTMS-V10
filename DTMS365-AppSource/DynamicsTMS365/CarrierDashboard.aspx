<%@ Page Language="C#" AutoEventWireup="true" Title="Carrier Dashboard" CodeBehind="CarrierDashboard.aspx.cs" Inherits="DynamicsTMS365.CarrierDashboard" %>

<!DOCTYPE html>

<html>
    <head >
        <title>DTMS Carrier Dashboard</title>
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
                            <h3 style="text-align:center; margin: 20px;">Historical Shipments Past 12 Months</h3>
                            <div style="text-align:center;"><button class='export-pdf k-button'>Export as PDF</button></div>
                            <div style="width:80%; margin: 50px" id="chart"></div>
                            <div style="display:none">
                                <div id="CarrierDashboardContent" class="ngl-blueBorder">
                                   <div style="padding: 10px;">
                                       <div id="editableCont"></div>
                                   </div>
                                </div>
                                <div id="CarrierDashboardEditor">
                                    <textarea id="ContEditor" style="height: 90%; width: 90%;"></textarea>
                                    <input id="edPgDet" type="hidden" />
                                </div>
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


        var resGetCarrierEditableContent = function (data) {
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
            $("#CarrierDashboardContent").hide();
            $("#CarrierDashboardEditor").show();            
            $("#ContEditor").data("kendoEditor").refresh();
        }

        function getCarrierEditableContent() {
            var rt = new editorContent();
            rt.PageControl = PageControl;
            rt.USec = 0;
            rt.EditorName = "ContEditor";
            rt.Content = "";
            rt.PageDetControl = $('#edPgDetRt').val();

            getEditorContentNoAuth(JSON.stringify(rt), resGetCarrierEditableContent);
        }

        var resSaveCarrierContentEditor = function (data) {
            //set the html page content
            var c = $("#ContEditor").data("kendoEditor").value();
            $("#editableCont").html(c);

            $("#CarrierDashboardEditor").hide();
            $("#CarrierDashboardContent").show();
        }

        function saveCarrierContentEditor() {
            var h = new editorContent();
            h.PageControl = PageControl;
            h.USec = localStorage.NGLvar1452;
            h.EditorName = "ContEditor";
            h.Content = $("#ContEditor").data("kendoEditor").value();
            h.PageDetControl = $('#edPgDet').val();

            saveEditorContent(h, resSaveCarrierContentEditor);
        }

        function cancelCarrierContentEditor() {
            getCarrierEditableContent();
            $("#CarrierDashboardEditor").hide();
            $("#CarrierDashboardContent").show();
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
                                fileName: "CarrierDashboard.pdf",
                                proxyURL: "https://demos.telerik.com/kendo-ui/service/export"
                            });
                        });

                    $(".k-splitter .k-scrollable").css("overflow", "auto");
                    $(".export-pdf").show();
                });

                $("#CarrierDashboardEditor").hide();
                $("#CarrierDashboardContent").show();
                getCarrierEditableContent();
                var divWait = $("#h1Wait");
                if (typeof (divWait) !== 'undefined') {
                    divWait.hide();
                }
            }, 10, this);

        });

        function createChart(data) {

            //let newOrder = data.sort((a, b) => a.OTLPerc < b.OTLPerc ? 1 : -1);
            let barDataValues = [];
            let barDataMonths = [];
           
            $.each(data, function (index, value) {
                barDataValues.push(value.Loads);
                barDataMonths.push(value.Month);
            });
            //console.loq(barDataValues);
            //console.loq(barDataMonths);
            $("#chart").kendoChart({
                title: {
                    text: "Loads Shipped by Month"
                },
                series: [{
                    type: "bar",
                    data: barDataValues,
                    labels: {
                        format: "{0}",
                        visible: true,
                        position: "insideEnd"
                    }
                }],
                categoryAxis: {
                    categories: barDataMonths
                },
                valueAxis: [{
                    labels: {
                        format: "{0}"
                    },
                    max: 1000
                }]             
            });

            //$("#chart").kendoChart({
            //    title: {
            //        text: "Loads Shipped by Month"
            //    },
            //    legend: {
            //        visible: false
            //    },
            //    seriesDefaults: {
            //        type: "column"
            //    },
            //    series: [{
            //        name: "FL",
            //        stack: "Company1",
            //        data: [85142, 92454, 98530]
            //    }, {
            //        name: "CA",
            //        stack: "Company2",
            //        data: [51541, 103915, 10589]
            //    }, {
            //        name: "CA",
            //        stack: "Company1",
            //        data: [379788, 569114, 655066]
            //    }, {
            //        name: "FL",
            //        stack: "Company3",
            //        data: [97894, 191015, 210767]
            //    }],
            //    seriesColors: ["#4db9e3", "#73c8e9", "#99d7ef"],
            //    valueAxis: {
            //        labels: {
            //            template: "#= kendo.format('{0:N0}', value / 10) # $"
            //        },
            //        line: {
            //            visible: false
            //        }
            //    },
            //    categoryAxis: {
            //        categories: ['January','July','December'],
            //        majorGridLines: {
            //            visible: false
            //        }
            //    },
            //    tooltip: {
            //        visible: true,
            //        template: "#= series.stack #s, sold items for a month"
            //    }
            //});
        }

        $(document).ready(() =>
            $.ajax({
                async: true,
                type: 'GET',
                url: `/api/CompDashboard/GetCarierDashboard`,
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                success: function (data) {    
                    createChart(data.Data);
                }
            }));
        //$(document).bind("kendo:skinChange", createChart);      

    </script>
    <style>


    </style>
    </div>


    </body>

</html>
