<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewReport.aspx.cs" Inherits="DynamicsTMS365.ViewReport" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html>

<html>
<body>   

    <form id="form2" runat="server"  onload="form2_Load1">
       <%-- <label>ReportPath <%=sReportPath %></label>
        <label>ReportServerUrl "<%=sServerReport %>"</label>--%>
        <div style="height: 100%; width: 100%;">
            <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" EnablePageMethods="true"></asp:ScriptManager>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <rsweb:ReportViewer ID="ReportViewer1" runat="server" ProcessingMode="Remote" SizeToReportContent="true" OnInit="ReportViewer1_Init" OnReportError="ReportViewer1_ReportError" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" >
                        <ServerReport ReportPath="/FMStdReports/CAR007-Carrier Master Address" ReportServerUrl="http://nglrdp07d/ReportServer/" />
                    </rsweb:ReportViewer>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <script></script>
        <style></style>
    </form>
</body>
</html>
