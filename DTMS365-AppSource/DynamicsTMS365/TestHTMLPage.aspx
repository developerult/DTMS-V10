<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestHTMLPage.aspx.cs" Inherits="DynamicsTMS365.TestHTMLPage" %>

<!DOCTYPE html>

<html>
    <head >
        <title>Dynamics TMS 365 Lane Maintenance</title>
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
                            <% Response.Write(PageErrorsOrWarnings); %>

                            <div id="divTitleLE"></div>

                            <!-- begin Page Content -->
                            <% Response.Write(FastTabsHTML); %>  
                            <div id="grid"></div>  
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
        var control = <%=UserControl%>; 
        ngl.UserValidated(true,control,oredirectUri); 
        var tObj = this;
        var tPage = this;

        var data = [
{ id: 1, text: "text 1", position: 0 },
{ id: 2, text: "text 2", position: 1 },
{ id: 3, text: "text 3", position: 2 }
        ]

       

      

            
        
        <% Response.Write(PageCustomJS); %>
        //***************** Widgets ******************************
            
        $(document).ready(function () {
            var PageMenuTab = <%=PageMenuTab%>;
            control = <%=UserControl%>;            
            if (ngl.UserValidated(true,control,oredirectUri)) { return; }

 
            if (control != 0){

                var dataSource = new kendo.data.DataSource({
                    data: data,        
                    schema: {
                        model: {
                            id: "id",
                            fields: {
                                id: { type: "number" },
                                text: { type: "string" },
                                position: { type: "number" }
                            }
                        }
                    }
                });

                var grid = $("#grid").kendoGrid({
                    dataSource: dataSource,  
                    scrollable: false,    
                    columns: [{ field: "id"},{ field:  "text"}, { field: "position"}]            
                }).data("kendoGrid");

                $(grid.element).kendoDraggable({
                    filter: "tr",
                    hint: function (e) {
                        var item = $('<div class="k-grid k-widget" style="background-color: DarkOrange; color: black;"><table><tbody><tr>' + e.html() + '</tr></tbody></table></div>');
                        return item;
                    },
                    group: "gridGroup1",
                });


                //grid.table.kendoDraggable({
                //    filter: "tbody > tr",
                //    group: "gridGroup",
                //    hint: function(e) {
                //        return $('<div class="k-grid k-widget"><table><tbody><tr>' + e.html() + '</tr></tbody></table></div>');
                //    }
                //});

                grid.wrapper.kendoDropTarget({
                    drop: function (e) {
                        var idragID = e.draggable.currentTarget.data("uid");

                        var tdest = $(e.target).parent();
        
                        if (tdest.is("th") || tdest.is("thead") || tdest.is("span") || tdest.parent().is("th")) {
                            return;
                        } else {                            
                            tdestID = grid.dataSource.getByUid(tdest.data("uid"));
                        }
                        if (typeof (tdestID) !== 'undefined' ){
                            alert (tdestID.id);
                            var dragdataItem = dataSource.getByUid(idragID);
                            //if (typeof (dragdataItem === 'undefined') ){ return;}
                            alert (dragdataItem.id);
                            if (tdestID.id === dragdataItem.id) {return;}
                            alert ('change ' + dragdataItem.id + ' to ' +  tdestID.id);
                            var tmp = dragdataItem.get("position");
                            dragdataItem.set("position", tdestID.get("position"));
                            tdestID.set("position", tmp);
            
                                        dataSource.sort({ field: "position", dir: "asc" });

                        } else { return; }
                        //var idestID =   $($(e.target).parent()).data("uid");  
                        //alert(idestID);
                        //var idesttID =   $(e.target).data("uid"); 
                        //alert(idesttID);
                        //if (idragID === idestID) {
                        //    alert('Bad');
                        //    return;
                        //};
                        
                        
                        
                        //var destdataItem = dataSource.getByUid(idestID);
                        //alert (destdataItem.id);
                            
                        //        dest = dataSource.get(dest.parent().data("id"));

                        //        //not on same item
                        //        if (target.get("id") !== dest.get("id")) {
                        //            //reorder the items
                        //            var tmp = target.get("position");
                        //            target.set("position", dest.get("position"));
                        //            dest.set("position", tmp);
            
                        //            dataSource.sort({ field: "position", dir: "asc" });
                        //        }                
                    },
                    group: "gridGroup1",
                });


                //grid.table.find("tbody > tr").kendoDropTarget({
                //    group: "gridGroup",
                //    drop: function(e) {   
                //        var current = $(e.draggable.currentTarget);
                //        var tid  = current.data("id");
                //        var target = dataSource.get(tid)
                //        var dest = $(e.target);
                //        //var target = dataSource.get($(e.draggable.currentTarget).data("id")), dest = $(e.target);
        
                //        if (dest.is("th")) {
                //            return;
                //        }       
                //        dest = dataSource.get(dest.parent().data("id"));

                //        //not on same item
                //        if (target.get("id") !== dest.get("id")) {
                //            //reorder the items
                //            var tmp = target.get("position");
                //            target.set("position", dest.get("position"));
                //            dest.set("position", tmp);
            
                //            dataSource.sort({ field: "position", dir: "asc" });
                //        }                
                //    }
                //});
               

            }

            var PageReadyJS = <%=PageReadyJS%>; 
            setTimeout(function () {        
                menuTreeHighlightPage(); //must be called after PageReadyJS
                var divWait = $("#h1Wait");
                if (typeof (divWait) !== 'undefined') { divWait.hide(); }
            }, 10, this);
        });
    </script>
    <style>
        .k-grid tbody tr td { overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }
    </style>   
      </div>
</body>
</html>
