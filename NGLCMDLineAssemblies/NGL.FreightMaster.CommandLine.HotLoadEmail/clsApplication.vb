Imports System.IO
Imports NGL.Core
Imports NGL.FreightMaster.Core
Imports NGL.Core.Communication
Imports System.Data
Imports System.Data.SqlClient

Public Class clsApplication : Inherits NGL.FreightMaster.Core.NGLCommandLineBaseClass
    Protected oConfig As New NGL.FreightMaster.Core.UserConfiguration

    Public Sub ProcessData()
        Me.openLog()
        Me.Log(Source & " Applicaiton Start")
        'use the database name as part of the source
        displayParameterData()
        fillConfig()
        
        Dim sqlStatement As String = String.Empty
        Dim strStatementInner As String = String.Empty
        Dim sql As String = String.Empty
        Dim strTempType, strTraiffType, strBookProNumber, strBookCarrOrderNumber, strBookLoadPONumber As String
        Dim strDestName, StrDestCity, strDestState, strCarrierAddress As String
        sqlStatement = " select   BookControl,BookCarrOrderNumber,BookLoadPONumber,BookProNumber,BookDestZip,BookCustCompControl,LaneOriginAddressUse,LaneTempType,BookDestName,BookDestCity,BookDestState from Book inner join lane "
        sqlStatement = sqlStatement & " on  Book.BookODControl=Lane.LaneControl "
        sqlStatement = sqlStatement & " Inner Join BookLoad  on BookLoad.BookLoadBookControl=Book.BookControl "
        sqlStatement = sqlStatement & "  where isnull(Book.BookHotLoad,0) = 1 and isnull(BookHotLoadSent,0) = 0 "
        Dim cn As New SqlConnection(ConnectionString)
        Dim con As New SqlConnection(ConnectionString)
        Dim cm As New SqlCommand(sqlStatement, cn)
        Try
            cn.Open()
            Dim dr As SqlDataReader = cm.ExecuteReader
            While dr.Read
                strCarrierAddress = String.Empty
                strBookProNumber = dr("BookProNumber")
                strBookLoadPONumber = IIf(IsDBNull(dr.Item("BookLoadPONumber")), "", dr.Item("BookLoadPONumber"))
                strBookCarrOrderNumber = IIf(IsDBNull(dr.Item("BookCarrOrderNumber")), "", dr.Item("BookCarrOrderNumber"))
                strDestName = dr("BookDestName")
                StrDestCity = dr("BookDestCity")
                strDestState = dr("BookDestState")
                strCarrierAddress = strDestName & "," & StrDestCity & "," & strDestState
                con.Open()
                Dim cmd As New SqlCommand("spHotLoadEmail", con)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@ProNumber", strBookProNumber)
                cmd.Parameters.AddWithValue("@OrderDestZip", dr("BookDestZip"))
                cmd.Parameters.AddWithValue("@CompControl", dr("BookCustCompControl"))
                cmd.Parameters.AddWithValue("@TempType", dr("LaneTempType"))
                If dr("LaneOriginAddressUse") = False Then
                    cmd.Parameters.AddWithValue("@TariffType", "O")
                Else
                    cmd.Parameters.AddWithValue("@TariffType", "I")
                End If
                Dim dr1 As SqlDataReader = cmd.ExecuteReader()
                While dr1.Read
                    HotLoadMail(dr1, strBookProNumber, strBookCarrOrderNumber, strCarrierAddress, strBookLoadPONumber)
                End While
                If con.State = ConnectionState.Open Then con.Close()
            End While
            Log("Process Data Complete")
        Catch ex As Exception
            LogError(Source & " Unexpected Error", "An unexpected error has occurred while attempting to email hot load information.  The actual error is: " & ex.Message & vbCrLf & vbCrLf, Me.AdminEmail)

        Finally
            If cn.State = ConnectionState.Open Then cn.Close()
        End Try
    End Sub

    Public Sub fillConfig()
        Try
            With oConfig
                .AdminEmail = Me.AdminEmail
                .AutoRetry = Me.AutoRetry
                .Database = Me.Database
                .DBServer = Me.DBServer
                .Debug = Me.Debug
                .FromEmail = Me.FromEmail
                .GroupEmail = Me.GroupEmail
                .INIKey = Me.INIKey
                .KeepLogDays = Me.KeepLogDays
                .ResultsFile = Me.ResultsFile
                .LogFile = Me.LogFile
                .SaveOldLog = Me.SaveOldLog
                .SMTPServer = Me.SMTPServer
                .Source = Me.Source
            End With

        Catch ex As Exception
            Throw New ApplicationException(Source & " Fill Configuration Failure", ex)
        End Try
    End Sub

    Private Sub HotLoadMail(ByVal row As SqlDataReader, ByVal strProNumber As String, ByVal strBookCarrOrderNumber As String, ByVal strCarrierAddress As String, ByVal strBookLoadPONumber As String)

        Dim body As String = ""
        Dim subject As String = ""
        Dim carrierEMail As String = ""
        Dim sql As String = ""
        Dim strSubject As String = ""

        Dim strCarrierContEmails As String = ""
        Dim strCarrierNEXTStopAcct As String = ""
        Dim strBodyText As String = String.Empty

        Dim cn As New SqlConnection()
        Try

            strCarrierContEmails = IIf(IsDBNull(row.Item("CarrierContactEmail")), "", row.Item("CarrierContactEmail"))
            strCarrierNEXTStopAcct = IIf(IsDBNull(row.Item("CarrierNEXTStopAcct")), "", row.Item("CarrierNEXTStopAcct"))
            subject = "Hot Load Notification For Order " & strBookCarrOrderNumber & " PO# " & strBookLoadPONumber & "  To   " & strCarrierAddress & ""
            strBodyText = " Dear Transport Partner, " & vbCrLf & vbCrLf
            strBodyText = strBodyText & " Our System has Identified you as a potential carrier for the above mentioned shipment " & strProNumber & "" & vbCrLf & vbCrLf
            strBodyText = strBodyText & " If you are interested in this opportunity,Please login to our portal by clicking the link below " & vbCrLf & vbCrLf
            strBodyText = strBodyText & Me.NEXTStopHotLoadURL & vbCrLf & vbCrLf
            strBodyText = strBodyText & " Your Account Number has been Indentified as  '" & strCarrierNEXTStopAcct & "'" & vbCrLf & vbCrLf & vbCrLf & vbCrLf
            strBodyText = strBodyText & " Best Regards," & vbCrLf & vbCrLf & vbCrLf
            strBodyText = strBodyText & Me.NEXTStopHotLoadAccountName & vbCrLf & vbCrLf
            strBodyText = strBodyText & Me.NEXTStopHotLoadContact
            Dim strCCEmail As String = ""
            If Me.Debug Then strCCEmail = Me.AdminEmail
            If strCarrierContEmails <> "" Then
                SendMail(SMTPServer, FromEmail, strBodyText, strCarrierContEmails, strCCEmail, subject)
            End If
        Catch ex As Exception
            LogException(Source & " Hot Load Email Failure", "Hot Load Problem with Mail Server:", Me.AdminEmail, ex, Source)
        Finally
            If cn.State = ConnectionState.Open Then cn.Close()
        End Try

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="strServer"></param>
    ''' <param name="strFrom"></param>
    ''' <param name="strBody"></param>
    ''' <param name="strTo"></param>
    ''' <param name="strCC"></param>
    ''' <param name="strSubject"></param>
    ''' <returns>Boolean</returns>
    ''' <remarks>
    ''' Modified By LVV 2/18/16 v-7.0.5.0
    ''' Changed call to SendMail to use one from Core
    ''' </remarks>
    Public Function SendMail(ByVal strServer As String, _
            ByVal strFrom As String, _
            ByVal strBody As String, _
            ByVal strTo As String, _
            ByVal strCC As String, _
            ByVal strSubject As String) As Boolean
        'Modified By LVV 2/18/16 v-7.0.5.0
        Dim oEmail As New Ngl.Core.Communication.Email
        Try
            oEmail.SendMail(Me.SMTPServer, strTo, strFrom, strBody, strSubject, strCC, SMTPUseDefaultCredentials:=GlobalSMTPUseDefaultCredentials, SMTPUser:=GlobalSMTPUser, SMTPPass:=GlobalSMTPPass, SMTPEnableSSL:=GlobalSMTPEnableSSL, SMTPTargetName:=GlobalSMTPTargetName, SMTPPort:=GlobalSMTPPort)
        Catch ex As Exception
            Return False
        End Try

        'Dim email As New System.Net.Mail.MailMessage(strFrom, strTo)
        'Dim client As New System.Net.Mail.SmtpClient(strServer)
        'Try

        '    email.Body = strBody
        '    email.Subject = strSubject
        '    If strCC <> "" Then
        '        Dim cc As New System.Net.Mail.MailAddress(strCC)
        '        email.CC.Add(cc)
        '    End If

        '    ' email.Bcc = strCC
        '    client.UseDefaultCredentials = True
        '    client.Send(email)
        '    Return True

        'Catch ex As Exception
        '    Return False
        'End Try
    End Function

    Public Function ToSql(ByVal text As String, Optional ByVal IncludeTrailingComma As Boolean = True, Optional ByVal AllowNull As Boolean = True) As String

        If text = String.Empty And AllowNull Then
            text = "NULL"
        Else
            text = "'" & text.Replace("'", "''").Replace(";", "") & "'"
        End If

        If IncludeTrailingComma Then text &= ","
        Return text
    End Function

    Public Function nz(ByVal strVal As String, ByVal strDefault As String) As String

        If Len(Trim(strVal & " ")) < 1 Then
            nz = strDefault
        Else

            nz = strVal & ""
        End If
    End Function

End Class
