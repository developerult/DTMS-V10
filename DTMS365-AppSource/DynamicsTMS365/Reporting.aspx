<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Reporting.aspx.cs" Inherits="DynamicsTMS365.Reporting" %>

<!DOCTYPE html>

<html>
    <head >
        <title>DTMS Reports</title>         
         <%=cssReference%>             
        <style>
            html,
            body {height:100%; margin:0; padding:0;}
            html {font-size: 12px; font-family: Arial, Helvetica, sans-serif; overflow:hidden;}
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

                            <div id="divContainer" style="height:100%; width:100%;">
                                <div id="reportTreeView" style="height:100%; width:50%;"></div>                           
                                <div style="height:100%; width:50%;">
                                    <iframe id="ifReport" src="ViewReport.aspx?hideHeader=true&refreshReport=false" style="height: 100%; width: 100%; padding:0; margin:0; border: none"></iframe>
                                </div>
                            </div>
                            

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
    <script>
       
        <% Response.Write(ADALPropertiesjs); %>

        var PageControl = '<%=PageControl%>';           
        var tObj = this;
        var tPage = this;           
       

        <% Response.Write(NGLOAuth2); %>

        
        <% Response.Write(PageCustomJS); %>

        function execActionClick(btn, proc){            
            if (btn.id == "btnRefresh" ){ oTariffGrid.dataSource.read(); } 
            else if(btn.id == "btnResetCurrentUserConfig"){ resetCurrentUserConfig(PageControl); }
        }

        //*************  Call Back Functions ****************
        function onSelect(e) {
            var selected = this.text(e.node);           
            var data = this.dataItem(e.node);
            if (data != null) {                   
                if (data.Name == null || data.Name == "") { return; }                       
                if (data.Control == null || data.Control == "" || data.Control == 0) { return; }
                renderReport(data.Name, data.Control);                    
            }
        }

        function openInNewTab(url) {
            var win = window.open(url, '_blank');
            win.focus();
        }

        function renderReport(name, control) {
            var ireport = $('#ifReport');
            if (ireport != null) {
                var url = location.protocol + "//" + location.host + "/ViewReport.aspx?hideHeader=true&reportname=" + name + "&reportcontrol=" + control + "&refreshReport=true&UN=" + localStorage.NGLvar1455;
                ireport.attr('src', url);
            }
        }

        function expandReportTree(){
            var treeview = $("#reportTreeView").data("kendoTreeView");          
            treeview.expand(".k-item"); //expand all loaded items
        }

        $(document).ready(function () {         
            var PageMenuTab = <%=PageMenuTab%>;
           

            if (control != 0){

                $("#divContainer").kendoSplitter({
                    panes: [
                        { collapsible: true, size: "40%" },
                        { collapsible: false, size: "60%"}
                    ]
                });

                var ds = new kendo.data.HierarchicalDataSource({
                    transport: {
                        read: {
                            url: "api/Reports/GetReportsTreeFlat",
                            type: "GET",
                            contentType: "application/json; charset=utf-8",
                            dataType: 'json',
                            headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },      
                            success: function (data) { expandReportTree(); },
                            error: function (xhr, textStatus, error) { tObj[errorCallBack](xhr, textStatus, error); }
                        }
                    },
                    schema: {
                        data: "Data",
                        total: "Count",
                        model: {
                            id: "TreeID",
                            hasChildren: "HasChildren",
                            children: "Items"
                        }
                    }
                });

                $("#reportTreeView").kendoTreeView({
                    dataSource: ds,
                    dataTextField: "Name",
                    //dataUrlField: "Name",
                    dataBound: function(e) {
                        var treeview = $("#reportTreeView").data("kendoTreeView");          
                        treeview.expand(treeview.findByText("Reports"));
                    },
                    select: onSelect
                });

            }         
            var PageReadyJS = <%=PageReadyJS%>;
            menuTreeHighlightPage(); //must be called after PageReadyJS
            var divWait = $("#h1Wait");            
            if (typeof (divWait) !== 'undefined') { divWait.hide(); }         
        });
    </script>
          <style>           
              .k-grid tbody tr td { overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }            
              .k-tooltip{ max-height: 500px; max-width: 450px; overflow-y: auto; }       
        </style>   
      </div>    
    </body>
</html>
