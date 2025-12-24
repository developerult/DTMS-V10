Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports Ngl.FreightMaster.Integration
Imports System.Xml.Serialization

<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class Book
    Inherits System.Web.Services.WebService

    Private mstrLastError As String = ""

    <WebMethod()> _
    Public Function LastError() As String
        Return mstrLastError
    End Function

    <WebMethod()> _
    Public Function GetDataSet() As Ngl.FreightMaster.Integration.BookData
        Dim book As New Ngl.FreightMaster.Integration.clsBook
        Return book.getDataSet()
    End Function

    <WebMethod()> _
    Public Function ProcessData(ByVal AuthorizationCode As String, _
            ByVal OrderHeaderTable As Ngl.FreightMaster.Integration.BookData.BookHeaderDataTable, _
            ByVal OrderDetailTable As Ngl.FreightMaster.Integration.BookData.BookDetailDataTable) As Integer

        Dim s As String = ""
        Return ProcessDataEx(AuthorizationCode, OrderHeaderTable, OrderDetailTable, s)
    End Function

    <WebMethod()> _
    Public Function ProcessDataEx(ByVal AuthorizationCode As String, _
            ByVal OrderHeaderTable As Ngl.FreightMaster.Integration.BookData.BookHeaderDataTable, _
            ByVal OrderDetailTable As Ngl.FreightMaster.Integration.BookData.BookDetailDataTable, _
            ByRef ReturnMessage As String) As Integer
        Dim result As Integer = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        ReturnMessage = ""
        Try
            Dim book As New Ngl.FreightMaster.Integration.clsBook

            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return result
            Utilities.populateIntegrationObjectParameters(book)
            book.OrderNotificationEmail = Utilities.GetSetting("OrderNotification")
            book.ValidateOrderUniqueness = Utilities.GetSetting("ValidateOrderUniqueness")
            result = book.ProcessData(OrderHeaderTable, OrderDetailTable, Utilities.GetConnectionString())
            ReturnMessage = book.LastError
            Dim sLogMsg As String = "Processing " & OrderHeaderTable.Rows.Count & " Book Header Records"
            If book.Debug Then
                sLogMsg &= String.Format(", AdminEmail: {0}, MailServer: {1}", book.AdminEmail, book.SMTPServer)
            End If
            Utilities.LogResults(sLogMsg, result, ReturnMessage, AuthorizationCode)
        Catch ex As Exception
            ReturnMessage = ex.Message
            Utilities.LogException("Book.ProcessDataEx Failure", result, "Cannot process book data.  ", ex, AuthorizationCode, "Process Book Data Failure")
        Finally
            Try
                mstrLastError = ReturnMessage
            Catch ex As Exception

            End Try
        End Try
        Return result
    End Function

    Private Function DoesOrderExist(ByVal AuthorizationCode As String, _
            ByVal OrderHeaderTable As Ngl.FreightMaster.Integration.BookData.BookHeaderDataTable, _
            ByVal OrderDetailTable As Ngl.FreightMaster.Integration.BookData.BookDetailDataTable) As Boolean

        Dim row As Ngl.FreightMaster.Integration.BookData.BookHeaderRow = OrderHeaderTable.Rows(0)
        Dim PoNumber As String = row.PONumber
        Dim OrderHeaderTableXml As String = GetXml(OrderHeaderTable)
        Dim OrderDetailTableXml As String = GetXml(OrderDetailTable)

        'Order date replaced with wild card to handle Veggie Juice
        Dim OrderHeaderTableXmlWithoutOrderDate As String = _
            Text.RegularExpressions.Regex.Replace(OrderHeaderTableXml, "<POdate>([^<]*)</POdate>", "%", RegexOptions.Multiline)


        Dim sb As New Text.StringBuilder
        sb.AppendLine("IF NOT EXISTS (SELECT * FROM OrderImportAudit WHERE PoNumber = '" & PoNumber.Replace("'", "''") & "' AND AuthorizationCode = '" & AuthorizationCode & "')")
        sb.AppendLine("     BEGIN")
        sb.AppendLine("         INSERT INTO OrderImportAudit (PoNumber, AuthorizationCode, HeaderXml, DetailXml)")
        sb.AppendLine("         VALUES ('" & PoNumber.Replace("'", "''") & "', ")
        sb.AppendLine("                 '" & AuthorizationCode & "',")
        sb.AppendLine("                 '" & OrderHeaderTableXml.Replace("'", "''") & "',")
        sb.AppendLine("                 '" & OrderDetailTableXml.Replace("'", "''") & "')")
        sb.AppendLine("         SELECT 0")
        sb.AppendLine("     END")
        sb.AppendLine("ELSE IF EXISTS (SELECT * FROM OrderImportAudit WHERE PoNumber = '" & PoNumber.Replace("'", "''") & "' AND AuthorizationCode = '" & AuthorizationCode & "' ")
        sb.AppendLine("                     AND HeaderXml LIKE '%" & OrderHeaderTableXmlWithoutOrderDate.ToString.Replace("'", "''") & "' ")
        sb.AppendLine("                     AND DetailXml LIKE '%" & OrderDetailTableXml.ToString.Replace("'", "''") & "')")
        sb.AppendLine("     BEGIN")
        sb.AppendLine("         SELECT 1")
        sb.AppendLine("     END")
        sb.AppendLine("ELSE ")
        sb.AppendLine("     BEGIN")
        sb.AppendLine("         UPDATE OrderImportAudit ")
        sb.AppendLine("         SET HeaderXml = '" & OrderHeaderTableXml.Replace("'", "''") & "', ")
        sb.AppendLine("             DetailXml = '" & OrderDetailTableXml.Replace("'", "''") & "', ")
        sb.AppendLine("             Modified = GETDATE()")
        sb.AppendLine("         WHERE PoNumber = '" & PoNumber.Replace("'", "''") & "' ")
        sb.AppendLine("             AND AuthorizationCode = '" & AuthorizationCode & "'")
        sb.AppendLine("         SELECT 0")
        sb.AppendLine("     END")

        Dim orderExists As Boolean = True
        Dim cn As New System.Data.SqlClient.SqlConnection(Utilities.GetConnectionString(AuthorizationCode))
        Dim cm As New System.Data.SqlClient.SqlCommand(sb.ToString, cn)
        Dim result As Boolean = False
        Try
            cn.Open()
            result = cm.ExecuteScalar
        Catch ex As Exception

        Finally
            If cn.State = ConnectionState.Open Then cn.Close()
        End Try

        Return result

    End Function

    Private Function GetXml(ByVal dt As DataTable) As String

        Dim ms As New IO.MemoryStream
        Dim ser As New XmlSerializer(dt.GetType())
        ser.Serialize(ms, dt)
        ms.Position = 0
        Dim bytes(ms.Length) As Byte
        ms.Read(bytes, 0, ms.Length)
        Dim s As String = Encoding.ASCII.GetString(bytes)
        Return s

    End Function

End Class