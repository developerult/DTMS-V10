<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PODUpload.aspx.cs" Inherits="DynamicsTMS365.PODUpload" %>

<!DOCTYPE html>

<html>
    <head >
        <title>DTMS File Upload</title>
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
                           
<%--        <%=jsnosplitterScripts%> --%>
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
                        <div id="FileUploadContent" class="ngl-blueBorder" > 
                            <br />                       
                            <form id="form1" runat="server">
                            <div style="padding:10px;">
                                <asp:HiddenField ID ="hdnImageTypeCode" runat="server" ClientIDMode="Static"/>
                                <asp:HiddenField ID ="hdnSourcePath" runat="server" ClientIDMode="Static"/>

                                <asp:Label ID="Label2" runat="server" Text="Label">Select an Image Type</asp:Label>
                                <br />                                 
                                <input id="dsImageTypeCode" value="1" style="height: 25px; width: 25%;background-color:#7bd2f6; "/>
                                <br /> <br />
                                <asp:Label ID="lblUser" runat="server" Text="Label">Select The Image to Upload</asp:Label>
                                <br /> 
                                <asp:FileUpload ID="FileUpload1" runat="server" accept=".png,.jpg,.jpeg,.gif,.txt,.pdf" style="width:300px;background-color:#7bd2f6; "/>
                                <br />  <br />
                                <asp:Label ID="Label1" runat="server" Text="Label">Files are restricted to png,jpg,jpeg,gif,txt and pdf</asp:Label>
                                <br /> <br />
                                <asp:Button ID="btnUpload" runat="server" Text="Upload Image" OnClick="btnUpload_Click" style="background-color:#7bd2f6;" />
                                <br /> <br />
                                <asp:Button ID="btnReset" runat="server" Text="Reset"  style="background-color:#7bd2f6;" />
                                <br /> <br />
                                <asp:Label ID="Label3" runat="server" Text="Label">Status</asp:Label>
                                 <br />
                                <asp:Label ID="lblMessage" runat="server" BorderStyle="None" Text="Waiting for File" ></asp:Label>
                                 <br /> <br />
                                <asp:Button ID="btnViewFile" runat="server" Text="View Upload Image" OnClick="btnViewFile_Click" style="background-color:#7bd2f6;"  Visible="false"/>
                                 <br />
                                <P>The system accepts images or pdf documents.  
                                    <br />Pdf documents are preferred. 
                                    <br />You may upload more than one file, but you may only view one file at a time.
                                     <br />After selecting "View Uploaded Image" you will need to manually 
                                     <br />navigate back to this page to upload more files.
                                     <br />
                                     <br />
                                    The system will automatically link the file to the previously selected booking record.
                                </P>
                                </div>
                            </form>
                             <br />
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
        <% Response.Write(AuthLoginNotificationHTML); %>   
        <% Response.WriteFile("~/Views/HelpWindow.html"); %>  
        <script>
         <% Response.Write(ADALPropertiesjs); %>
        var PageControl = '<%=PageControl%>';          
        var tObj = this;
        var tPage = this;   

        <% Response.Write(NGLOAuth2); %>

        
            var dsImageTypeCodeDataSource = kendo.data.DataSource;

            function dsImageTypeCodechanged() {
                //alert("cmPagechanged")
                //debugger;
                var iValue = $("#dsImageTypeCode").val();
                $('#hdnImageTypeCode').val(iValue);
            }

        $(document).ready(function () {  
            var PageMenuTab = <%=PageMenuTab%>;
            setTimeout(function () { var PageReadyJS = <%=PageReadyJS%>; }, 10, this);

            dsImageTypeCodeDataSource = new kendo.data.DataSource({
                transport: {
                    read: {
                        url: "api/vLookupList/GetGlobalDynamicList/16",
                        headers: { "Authorization": localStorage.NGLvar1454, "USC": localStorage.NGLvar1452 }
                    },
                    parameterMap: function (options, operation) { return options; }
                },
                schema: {
                    data: "Data",
                    total: "Count",
                    model: {
                        id: "Control",
                        fields: {
                            Name: { editable: false },
                            Description: { editable: false }
                        }
                    },
                    errors: "Errors"
                },
                error: function (xhr, textStatus, error) { ngl.showErrMsg("Get Image Type List  Error", formatAjaxJSONResponsMsgs(xhr, textStatus, error, "Read Data Failure"), null); this.cancelChanges(); },
                serverPaging: false,
                serverSorting: false,
                serverFiltering: false
            });

            var dsImageTypeCodeList = $("#dsImageTypeCode").kendoDropDownList({
                dataTextField: "Name",
                //dataTextField: "Description",
                dataValueField: "Control",
                dataSource: dsImageTypeCodeDataSource,
                change: dsImageTypeCodechanged,
                dataBound: dsImageTypeCodechanged,
            });

            $("#dsImageTypeCode").data("kendoDropDownList").dataSource.read();
            setTimeout(function () {  
                menuTreeHighlightPage(); //must be called after PageReadyJS

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
