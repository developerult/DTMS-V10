<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PODViewer.aspx.cs" Inherits="DynamicsTMS365.PODViewer" %>

<!DOCTYPE html>

<html>
    <head >
        <title>DTMS File Viewer</title>
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
                           
        <%=jsnosplitterScripts%> 
        <%=sWaitMessage%>    
               
      <div id="example" style="height: 100%; width: 100%;  margin-top: 2px;">
         <% Response.Write(PageErrorsOrWarnings); %>   
        <div id="FileViewerContent" class="ngl-blueBorder" style=" position:absolute;  width:80%; height:80%;top: 5%;  left: 5%; "> 
        <br />                      
        <form id="form1" runat="server" style="height: 95%; width: 95%;">
        <div style="height: 95%; width: 95%;  margin-top: 5px; margin-left: 5px; ">
            <br />
            <br />
            <asp:Label ID="Label2" runat="server" Text="Source: "  style="margin-left: 5px;" ></asp:Label>
            <asp:Label ID="lblFileName" runat="server" style="margin-left: 5px;"></asp:Label>
            <br />
            <asp:Label ID="Label3" runat="server" Text="Type:&nbsp;&nbsp;&nbsp;&nbsp;  " style="margin-left: 5px;"></asp:Label>
            <asp:Label ID="lblImageType" runat="server" style="margin-left: 5px;" ></asp:Label>
            <br />
            <br />
            <asp:Image ID="imgFile" runat="server" style="margin-left: 5px;" ></asp:Image>
        </div>
       </form>
     <br />

</div>
<% Response.Write(AuthLoginNotificationHTML); %>   
<script>
 <% Response.Write(ADALPropertiesjs); %>

var PageControl = '<%=PageControl%>';            

var tObj = this;
var tPage = this;           


<% Response.Write(NGLOAuth2); %>

    $(document).ready(function () {  
        setTimeout(function () {

            var divWait = $("#h1Wait");
            if (typeof (divWait) !== 'undefined') {
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
