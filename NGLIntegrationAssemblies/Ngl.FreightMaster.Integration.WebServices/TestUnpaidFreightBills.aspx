<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TestUnpaidFreightBills.aspx.vb" Inherits="Ngl.FreightMaster.Integration.WebServices.TestUnpainFreightBills" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
    <p><asp:Label ID="lblLegalEntity" runat="server" Text="Enter Legal Entity: "></asp:Label></p>
    <p><asp:TextBox ID="tbLegalEntity" runat="server">TWO</asp:TextBox></p>
    <p><asp:Button ID="btnRunTest" runat="server" Text="Run Test" /></p>
    </div>
        <div style="width:400px; height:400px; overflow-x:auto; overflow-y:auto;">
            <asp:ListBox ID="lbResults" runat="server" style="min-width:100%; min-height:100%;" ></asp:ListBox>
            <%--<asp:ListBox ID="ListBox1" runat="server" Width="400" Height="300" ></asp:ListBox>--%>
        </div>
    </form>
</body>
</html>
