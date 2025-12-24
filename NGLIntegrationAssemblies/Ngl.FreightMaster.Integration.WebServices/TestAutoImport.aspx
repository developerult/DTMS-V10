<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TestAutoImport.aspx.vb" Inherits="Ngl.FreightMaster.Integration.WebServices.TestAutoImport" %>

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
                <td><asp:Label ID="lblOrderNumber" runat="server" Text="Enter Order Number: "></asp:Label></td>
                <td><asp:TextBox ID="tbOrderNumber" runat="server"> </asp:TextBox></td>
                <td><asp:Label ID="lblAuthCode" runat="server" Text="Enter WS Auth Code: "></asp:Label></td>
                <td><asp:TextBox ID="tbAuthCode" runat="server">WSPROD</asp:TextBox></td>
            </tr>
            
            
        </table>
        <asp:Button ID="btnRunTest" runat="server" Text="Run Test" />
        
    </div>
        <div style="width:400px; height:400px; overflow-x:auto; overflow-y:auto;">
            <asp:ListBox ID="lbResults" runat="server" style="min-width:100%; min-height:100%;" ></asp:ListBox>
            <%--<asp:ListBox ID="ListBox1" runat="server" Width="400" Height="300" ></asp:ListBox>--%>
        </div>
    </form>
    </div>
    </form>
</body>
</html>
