<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TestLaneIntegration.aspx.vb" Inherits="Ngl.FreightMaster.Integration.WebServices.TestLaneIntegration" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
        <table>
            <tr>
                <td><asp:Label ID="lblLaneName" runat="server" Text="Name: "></asp:Label></td>
                <td><asp:TextBox ID="tbLaneName" runat="server">NGL Test Lane</asp:TextBox></td>
            </tr>
            <tr>
                <td><asp:Label ID="lblLaneNumber" runat="server" Text="Number: "></asp:Label></td>
                <td><asp:TextBox ID="tbLaneNumber" runat="server">NGL-Test-Lane</asp:TextBox></td>
            </tr>
            <tr>
                <td><asp:Label ID="lblLaneCompAlphaCode" runat="server" Text="Alpha Code:"></asp:Label></td>
                <td><asp:TextBox ID="tbLaneCompAlphaCode" runat="server">NGLTSTComp</asp:TextBox></td>
            </tr>
            <tr>
                <td><asp:Label ID="lblLaneLegalEntity" runat="server" Text="Legal Entity:"></asp:Label></td>
                <td><asp:TextBox ID="tbLaneLegalEntity" runat="server">NGLOne</asp:TextBox></td>
            </tr>
            <tr>
                <td><asp:Label ID="lblLaneDestName" runat="server" Text="Dest Name :"></asp:Label></td>
                <td><asp:TextBox ID="tbLaneDestName" runat="server">Ship To Company</asp:TextBox></td>
            </tr>
            <tr>
                <td><asp:Label ID="lblLaneDestAddress1" runat="server" Text="Street Address:"></asp:Label></td>
                <td><asp:TextBox ID="tbLaneDestAddress1" runat="server">1000 Colonial Parkway</asp:TextBox></td>
            </tr>
            <tr>
                <td><asp:Label ID="lblLaneDestCity" runat="server" Text="City:"></asp:Label></td>
                <td><asp:TextBox ID="tbLaneDestCity" runat="server">Inverness</asp:TextBox></td>
            </tr>
            <tr>
                <td><asp:Label ID="lblLaneDestState" runat="server" Text="State:"></asp:Label></td>
                <td><asp:TextBox ID="tbLaneDestState" runat="server">IL</asp:TextBox></td>
            </tr>
            <tr>
                <td><asp:Label ID="lblLaneDestCountry" runat="server" Text="Country:"></asp:Label></td>
                <td><asp:TextBox ID="tbLaneDestCountry" runat="server">US</asp:TextBox></td>
            </tr>
            <tr>
                <td><asp:Label ID="lblLaneDestZip" runat="server" Text="Zip:"></asp:Label></td>
                <td><asp:TextBox ID="tbLaneDestZip" runat="server">60067</asp:TextBox></td>
            </tr>            
                      
        </table>
        <asp:Button ID="btnRunTest" runat="server" Text="Run Test" />
        
    </div>
        <div style="width:400px; height:400px; overflow-x:auto; overflow-y:auto;">
            <asp:Label ID="lblResults" runat="server" Text=""></asp:Label>
        </div>
    </form>
</body>
</html>
