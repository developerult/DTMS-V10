<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TestPCMiler.aspx.vb" Inherits="Ngl.FreightMaster.Integration.WebServices.TestPCMiler" %>

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
                <td><asp:Label ID="lblStop1Zip" runat="server" Text="Stop 1 Zip: "></asp:Label></td>
                <td><asp:TextBox ID="tbStop1Zip" runat="server">60619</asp:TextBox></td>
            </tr>
            <tr>
                <td><asp:Label ID="lblStop2Zip" runat="server" Text="Stop 2 Zip:"></asp:Label></td>
                <td><asp:TextBox ID="tbStop2Zip" runat="server">60067</asp:TextBox></td>
            </tr>
            <tr>
                <td><asp:Label ID="lblStop3Zip" runat="server" Text="Stop 3 Zip:"></asp:Label></td>
                <td><asp:TextBox ID="tbStop3Zip" runat="server">61101</asp:TextBox></td>
            </tr>
            <tr>
                <td><asp:Label ID="lblStop4Zip" runat="server" Text="Stop 4 Zip:"></asp:Label></td>
                <td><asp:TextBox ID="tbStop4Zip" runat="server">61635</asp:TextBox></td>
            </tr>
            
        </table>
        <asp:Button ID="btnRunTest" runat="server" Text="Run Test" />
        
    </div>
        <div style="width:400px; height:400px; overflow-x:auto; overflow-y:auto;">
            <asp:ListBox ID="lbResults" runat="server" style="min-width:100%; min-height:100%;" ></asp:ListBox>
            <%--<asp:ListBox ID="ListBox1" runat="server" Width="400" Height="300" ></asp:ListBox>--%>
        </div>
    </form>
</body>
</html>
