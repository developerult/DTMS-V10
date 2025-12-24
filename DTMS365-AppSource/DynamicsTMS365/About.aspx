
<%@ Page Title="About Page" Language="C#"  AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="DynamicsTMS365.About" %>

<!DOCTYPE html>

<html>
    <head >
        <title>DTMS About</title>
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
                            <div id="AboutContent" class="ngl-blueBorder">
                               <div style="padding: 10px;">
                                   <div id="editableCont"></div>
                               </div>
                            </div>
                            <div id="AboutEditor"><textarea id="ContEditor" style="height: 90%; width: 90%;"></textarea></div>
                            <input id="edPgDet" type="hidden" />

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
       
        var resGetAboutEditableContent = function (data) {
            //set the value of the editor and the html page content
            $("#ContEditor").data("kendoEditor").value(data.Content);
            $("#editableCont").html(data.Content);
           
            $('#edPgDet').val(data.PageDetControl);
        }

        function getAboutEditableContent() {
            var e = new editorContent();
            e.PageControl = PageControl;
            e.USec = 0;
            e.EditorName = "ContEditor";
            e.Content = "";
            e.PageDetControl = $('#edPgDet').val();

            getEditorContentNoAuth(JSON.stringify(e), resGetAboutEditableContent);
        }


        var resSaveAboutContentEditor = function (data) {          
            //set the html page content
            var c = $("#ContEditor").data("kendoEditor").value();
            $("#editableCont").html(c);
            
            $("#AboutEditor").hide();
            $("#AboutContent").show();           
        }

        function saveAboutContentEditor() {
            var h = new editorContent();
            h.PageControl = PageControl;
            h.USec = localStorage.NGLvar1452;
            h.EditorName = "ContEditor";
            h.Content = $("#ContEditor").data("kendoEditor").value();
            h.PageDetControl = $('#edPgDet').val();

            saveEditorContent(h, resSaveAboutContentEditor);
        }

        function cancelAboutContentEditor() {            
            getAboutEditableContent();
            $("#AboutEditor").hide();
            $("#AboutContent").show();
        }

        function editPageDynamicContent(p){
            $("#AboutContent").hide();
            $("#AboutEditor").show();                
            $("#ContEditor").data("kendoEditor").refresh();
        }

        function execActionClick(btn, proc){
            if(btn.id == "btnTMSContEdit"){
                editPageDynamicContent(proc);
            }
            
        }

        function register_Click(){
            document.location = "NGLLogin.aspx?caller=About.aspx";
        }

        $(document).ready(function () {  
            var PageMenuTab = <%=PageMenuTab%>;
           
            setTimeout(function () {var PageReadyJS = <%=PageReadyJS%>; }, 10,this);
            setTimeout(function () {  
                menuTreeHighlightPage(); //must be called after PageReadyJS

                $("#AboutEditor").hide();
                $("#AboutContent").show();         
                $('#edPgDet').val(0);
                getAboutEditableContent();
                var divWait = $("#h1Wait");
                if  (typeof (divWait) !== 'undefined' ) {
                    divWait.hide();
                }
            }, 10, this);

        });


    </script>
    <style>


    </style>
    </div>


    </body>

</html>
