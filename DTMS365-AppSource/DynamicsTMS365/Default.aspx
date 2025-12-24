<%@ Page Title="Home Page" Language="C#"  AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="DynamicsTMS365._Default" %>

<!DOCTYPE html>

<html>
    <head >
        <title>DTMS Home</title>  
         <%=cssReference%>              
        <style>

html,

body
{height:100%; margin:0; padding:0;}

html
{font-size: 12px; font-family: Arial, Helvetica, sans-serif; overflow:hidden;}
</style>
 
    </head>
    <body>     
        <%=jssplitter2Scripts%>
        <%=sWaitMessage%>        

      <div id="example" style="height: 100%; width: 100%;  margin-top: 2px;">
             <div id="vertical" style="height: 98%; width: 98%; " >                 
                <div id="menu-pane" style="height: 100%; width: 100%;">
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
                          <div id="divHomePageWrapper" style="display:none;" >
                              <div class="ngl-blueBorder" style="min-width: 225px;">
                                  <div style="padding: 10px;">
                                      <div style="position: relative; float: left; display: inline-block; min-width:200px; width: 35%; margin-right:25px;">

                                          <div id="divHomeAdminCont"></div>

                                      </div>
                                      <div style="position: relative; float: left; display: inline-block; width: calc(65% - 25px); margin-top: 10px;">
                                          
                                          <div id="divHomeAdminContRt"></div>

                                      </div>
                                  </div>
                              </div>
                          </div>

                          <div id="divHomeEditWrapper" style="display:none;" >
                              <div style="padding:10px;">
                                  <button id="btnExitEditMode" type="button">Exit Edit Mode</button>
                              </div>


                              <div class="fast-tab">
                                  <span id="ExpandLeftEditSpan" style="display: none;"><a onclick='expandLeftEdit();'><span style="font-size: small;font-weight:bold;" class='k-icon k-i-chevron-down'></span></a></span>
                                  <span id="CollapseLeftEditSpan" style="display: normal;"><a onclick='collapseLeftEdit();'><span style="font-size: small;font-weight:bold;" class='k-icon k-i-chevron-up'></span></a></span>
                                  <span style="font-size: small; font-weight: bold">Left Editor</span>&nbsp;&nbsp;<br />
                                  <div id="LeftEditDiv" style="padding-bottom:10px;">

                                      <textarea id="txtHomeEditor"></textarea>
                                      <input id="edPgDet" type="hidden" />

                                  </div>
                              </div>

                              <div class="fast-tab">
                                  <span id="ExpandRightEditSpan" style="display: none;"><a onclick='expandRightEdit();'><span style="font-size: small;font-weight:bold;" class='k-icon k-i-chevron-down'></span></a></span>
                                  <span id="CollapseRightEditSpan" style="display: normal;"><a onclick='collapseRightEdit();'><span style="font-size: small;font-weight:bold;" class='k-icon k-i-chevron-up'></span></a></span>
                                  <span style="font-size: small; font-weight: bold">Right Editor</span>&nbsp;&nbsp;<br />
                                  <div id="RightEditDiv" style="padding-bottom:10px;">

                                      <textarea id="txtHomeEditorRt"></textarea>
                                      <input id="edPgDetRt" type="hidden" />

                                  </div>
                              </div>

                          </div>

                      </div>
                    </div>
                </div>
                <div id="bottom-pane" class="k-block" style="height: 100%; width: 100%;">
                    <div class="pane-content" style="margin:10px;">
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

        
        <% Response.Write(PageCustomJS); %>

        //userBottomPaneSize =  "25px"; 
        //userBottomPaneCollapsed =  true; 
        //userMenuPaneSize = "200px"; 
        //userMenuCollapsed = false; 
        //userLeftPaneSize =  "400px"; 
        //userLeftPaneCollapsed = true;

        function expandLeftEdit() {
            $("#LeftEditDiv").show();
            $("#ExpandLeftEditSpan").hide();
            $("#CollapseLeftEditSpan").show();
        }

        function collapseLeftEdit() {
            $("#LeftEditDiv").hide();
            $("#ExpandLeftEditSpan").show();
            $("#CollapseLeftEditSpan").hide();
        }

        function expandRightEdit() {
            $("#RightEditDiv").show();
            $("#ExpandRightEditSpan").hide();
            $("#CollapseRightEditSpan").show();
        }

        function collapseRightEdit() {
            $("#RightEditDiv").hide();
            $("#ExpandRightEditSpan").show();
            $("#CollapseRightEditSpan").hide();
        }

        function execActionClick(btn, proc) {
            if (btn.id == "btnTMSContEdit") { editPageDynamicContent(); }
            else if (btn.id == "btnResetPaneSettings") { resetPaneSettings(); document.location = oredirectUri; }
            else if (btn.id == "btnResetTheme") { resetTheme(); location.reload(); }

        }


        if (typeof (control) !== 'undefined' && control !== null && control != 0) {
            setTimeout(function () {
                var setting = "[{ name: 'userMenuPaneSize', value: '150px' }, { name: 'userMenuCollapsed', value: 'false' }, { name: 'userBottomPaneSize', value: '35px' }, { name: 'userBottomPaneCollapsed', value: 'true' }, { name: 'userLeftPaneSize', value: '150px' }, { name: 'userLeftPaneCollapsed', value: 'false' }]";
                var tObj = this;
                var oCRUDCtrl = new nglRESTCRUDCtrl();
                blnRet = oCRUDCtrl.filteredPostJSON("cmPage/PostPaneSetting", setting, tObj, "splitter2SavePaneSettingsSuccessCallback", "splitter2SavePaneSettingsAjaxErrorCallback", true);
            }, 1, tObj);
        }

        function editPageDynamicContent(){
            $("#divHomePageWrapper").hide();
            $("#divHomeEditWrapper").show();                
            $("#txtHomeEditor").data("kendoEditor").refresh();
            $("#txtHomeEditorRt").data("kendoEditor").refresh();
        }

        function displayPageUI(){
            $("#divHomePageWrapper").show();
            $("#divHomeEditWrapper").hide();
        }

        function displayEditor(){
            $("#divHomePageWrapper").hide();
            $("#divHomeEditWrapper").show();
        }

        var resGetHomeEditableContent = function (data) {
            //set the value of the editor and the html page content
            $("#txtHomeEditor").data("kendoEditor").value(data.Content);
            $("#divHomeAdminCont").html(data.Content);
           
            $('#edPgDet').val(data.PageDetControl);
        }

        function getHomeEditableContent() {
            var e = new editorContent();
            e.PageControl = PageControl;
            e.USec = 0;
            e.EditorName = "txtHomeEditor";
            e.Content = "";
            e.PageDetControl = $('#edPgDet').val();

            getEditorContentNoAuth(JSON.stringify(e), resGetHomeEditableContent);
        }       

        var resSaveHomeContentEditor = function (data) {          
            //set the html page content
            var c = $("#txtHomeEditor").data("kendoEditor").value();
            $("#divHomeAdminCont").html(c);          
        }

        function saveHomeContentEditor() {
            var h = new editorContent();
            h.PageControl = PageControl;
            h.USec = localStorage.NGLvar1452;
            h.EditorName = "txtHomeEditor";
            h.Content = $("#txtHomeEditor").data("kendoEditor").value();
            h.PageDetControl = $('#edPgDet').val();

            saveEditorContent(h, resSaveHomeContentEditor);
        }

        function cancelHomeContentEditor() {            
            getHomeEditableContent();
        }

        var resGetHomeEditableContentRt = function (data) {
            //set the value of the editor and the html page content
            $("#txtHomeEditorRt").data("kendoEditor").value(data.Content);
            $("#divHomeAdminContRt").html(data.Content);
           
            $('#edPgDetRt').val(data.PageDetControl);
        }

        function getHomeEditableContentRt() {
            var rt = new editorContent();
            rt.PageControl = PageControl;
            rt.USec = 0;
            rt.EditorName = "txtHomeEditorRt";
            rt.Content = "";
            rt.PageDetControl = $('#edPgDetRt').val();

            getEditorContentNoAuth(JSON.stringify(rt), resGetHomeEditableContentRt);
        }

        var resSaveHomeContentEditorRt = function (data) {          
            //set the html page content
            var c = $("#txtHomeEditorRt").data("kendoEditor").value();
            $("#divHomeAdminContRt").html(c);          
        }

        function saveHomeContentEditorRt() {
            var h = new editorContent();
            h.PageControl = PageControl;
            h.USec = localStorage.NGLvar1452;
            h.EditorName = "txtHomeEditorRt";
            h.Content = $("#txtHomeEditorRt").data("kendoEditor").value();
            h.PageDetControl = $('#edPgDetRt').val();

            saveEditorContent(h, resSaveHomeContentEditorRt);
        }

        function cancelHomeContentEditorRt() {            
            getHomeEditableContentRt();
        }    

        //btnResetTheme
        function resetTheme() {
            var u = new User();
            u.UserSecurityControl = 0; // this is updated in the API service          
            u.UserTheme365 = 'classic-opal';
            var urls = "api/Users/UpdateUserTheme365";
            $.ajax({
                async: false,
                type: "POST",
                url: urls,
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                data: JSON.stringify(u),
                headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 },
                success: function (data) {
                    try {
                        var blnSuccess = false;
                        var blnErrorShown = false;
                        var strValidationMsg = "";
                        if (typeof (data) !== 'undefined' && ngl.isObject(data)) {
                            if (ngl.stringHasValue(data.Errors)) { blnErrorShown = true; ngl.showErrMsg("Reset User Theme Failure", data.Errors, null); }
                            else {
                                if (typeof (data.Data) !== 'undefined' && ngl.isArray(data.Data)) {
                                    if (data.Data.length > 0 && typeof (data.Data[0]) !== 'undefined' && ngl.isObject(data.Data[0])) {
                                        blnSuccess = true;
                                        if (ngl.isFunction(resultFunc)) { resultFunc(data.Data[0]); }
                                    }
                                }
                            }
                        }
                        if (blnSuccess === false && blnErrorShown === false) {
                            if (strValidationMsg.length < 1) { strValidationMsg = "There was a problem while saving the User Settings"; }
                            ngl.showErrMsg("Save User Settings Failure", strValidationMsg, null);
                        }
                    } catch (err) { ngl.showErrMsg(err.name, err.description, null); }
                },
                error: function (xhr, textStatus, error) { var sMsg = formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Save Data Failure"); ngl.showErrMsg("Save User Settings Failure", sMsg, null); }
            });
        }
       
        $(document).ready(function () {            
            var PageMenuTab = <%=PageMenuTab%>;
            
            setTimeout(function () {
                $("#divHomePageWrapper").show();
                $("#divHomeEditWrapper").show();
                $("#btnExitEditMode").kendoButton({
                    icon: "close",
                    click: function(e) {
                        getHomeEditableContent();
                        getHomeEditableContentRt();
                        displayPageUI();
                    }
                });
           
                displayPageUI();
                $('#edPgDet').val(0);
                $('#edPgDetRt').val(0);
                getHomeEditableContent();
                getHomeEditableContentRt();
            }, 100,this);
            

                
            var PageReadyJS = <%=PageReadyJS%>;

        //setTimeout(function () {        
            menuTreeHighlightPage(); //must be called after PageReadyJS
            var divWait = $("#h1Wait");
            if  (typeof (divWait) !== 'undefined' ) {
                divWait.hide();
            }
        //}, 10, this);
          
            

        });


    </script>
    
    </div>


    </body>

</html>