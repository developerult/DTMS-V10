<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TestCompIntegration.aspx.vb" Inherits="Ngl.FreightMaster.Integration.WebServices.TestCompWebService" %>

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
                <td><asp:Label ID="lblCompName" runat="server" Text="Name: "></asp:Label></td>
                <td><asp:TextBox ID="tbCompName" runat="server">NGL Test Company</asp:TextBox></td>
            </tr>
            <tr>
                <td><asp:Label ID="lblCompAlphaCode" runat="server" Text="Alpha Code:"></asp:Label></td>
                <td><asp:TextBox ID="tbCompAlphaCode" runat="server">NGLTSTComp</asp:TextBox></td>
            </tr>
            <tr>
                <td><asp:Label ID="lblCompLegalEntity" runat="server" Text="Legal Entity:"></asp:Label></td>
                <td><asp:TextBox ID="tbCompLegalEntity" runat="server">NGLOne</asp:TextBox></td>
            </tr>
            <tr>
                <td><asp:Label ID="lblCompStreetAddress1" runat="server" Text="Street Address:"></asp:Label></td>
                <td><asp:TextBox ID="tbCompStreetAddress1" runat="server">1611 Colonial Parkway</asp:TextBox></td>
            </tr>
            <tr>
                <td><asp:Label ID="lblCompStreetCity" runat="server" Text="City:"></asp:Label></td>
                <td><asp:TextBox ID="tbCompStreetCity" runat="server">Inverness</asp:TextBox></td>
            </tr>
            <tr>
                <td><asp:Label ID="lblCompStreetState" runat="server" Text="State:"></asp:Label></td>
                <td><asp:TextBox ID="tbCompStreetState" runat="server">IL</asp:TextBox></td>
            </tr>
            <tr>
                <td><asp:Label ID="lblCompStreetCountry" runat="server" Text="Country:"></asp:Label></td>
                <td><asp:TextBox ID="tbCompStreetCountry" runat="server">US</asp:TextBox></td>
            </tr>
            <tr>
                <td><asp:Label ID="lblCompStreetZip" runat="server" Text="Zip:"></asp:Label></td>
                <td><asp:TextBox ID="tbCompStreetZip" runat="server">60067</asp:TextBox></td>
            </tr>            
            <tr>
                <td><asp:Label ID="lblCompAbrev" runat="server" Text="Abrev:"></asp:Label></td>
                <td><asp:TextBox ID="tbCompAbrev" runat="server">NGL</asp:TextBox></td>
            </tr>            
            <tr>
                <td><asp:Label ID="lblCompActive" runat="server" Text="Active:"></asp:Label></td>
                <td><asp:TextBox ID="tbCompActive" runat="server">true</asp:TextBox></td>
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
