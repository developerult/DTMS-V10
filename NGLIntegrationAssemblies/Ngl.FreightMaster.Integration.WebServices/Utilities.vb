

Public Class Utilities



    Friend Shared Function GetConnectionString() As String

        Return ConfigurationManager.ConnectionStrings("Ngl.FreightMaster.Integration.WebServices.My.MySettings.ConnectionString").ConnectionString

    End Function

    Friend Shared Function GetServerName(ByVal connectionString As String) As String

        Dim cn As New SqlClient.SqlConnection(connectionString)
        Return cn.DataSource

    End Function

    Friend Shared Function GetDatabase(ByVal connectionString As String) As String

        Dim cn As New SqlClient.SqlConnection(connectionString)
        Return cn.Database

    End Function

    Friend Shared Function GetDebugMode() As Boolean
        Dim strDebugMode As String = My.MySettings.Default.DebugMode
        If strDebugMode.ToUpper = "TRUE" Then
            Return True
        Else
            Return False
        End If
    End Function

    Friend Shared Function validateAuthCode(ByVal AuthorizationCode As String) As Boolean
        Try
            If AuthorizationCode = My.Settings.AuthCode Then
                Return True
            Else
                Throw New ApplicationException("Cannot read configuration settings.  Please check that you are providing a valid Authorization Code.")
            End If
        Catch ex As Exception
            Throw
        End Try
    End Function

    Friend Shared Sub populateIntegrationObjectParameters(ByRef oImportExport As clsImportExport)

        Dim connectionString As String = Utilities.GetConnectionString()
        With oImportExport
            .AdminEmail = Utilities.GetSetting("AdminEMail")
            .FromEmail = Utilities.GetSetting("EMailFrom")
            .GroupEmail = Utilities.GetSetting("GroupEMail")
            .Retry = Utilities.GetSetting("Retries")
            .SMTPServer = Utilities.GetSetting("SmtpMailServer")
            .DBServer = Utilities.GetServerName(connectionString)
            .Database = Utilities.GetDatabase(connectionString)
            .ConnectionString = connectionString
            .AuthorizationCode = My.Settings.AuthCode
            .Debug = Utilities.GetDebugMode()
            .WCFAuthCode = Utilities.GetSetting("WCFAuthCode")
            .WCFURL = Utilities.GetSetting("WCFURL")
            .WCFTCPURL = Utilities.GetSetting("WCFTCPURL")
        End With

    End Sub


    Friend Shared Function GetSetting(ByVal Setting As String) As String

        Try
            Select Case Setting
                Case "DebugMode"
                    Return My.Settings.DebugMode
                Case "LogFile"
                    Return My.Settings.LogFile
                Case "AuthCode"
                    Return My.Settings.AuthCode
                Case "AdminEMail"
                    Return My.Settings.AdminEMail
                Case "EMailFrom"
                    Return My.Settings.EMailFrom
                Case "GroupEMail"
                    Return My.Settings.GroupEMail
                Case "OrderNotification"
                    Return My.Settings.OrderNotification
                Case "Retries"
                    Return My.Settings.Retries
                Case "SmtpMailServer"
                    Return My.Settings.SmtpMailServer
                Case "ValidateOrderUniqueness"
                    Return My.Settings.ValidateOrderUniqueness
                Case "WCFAuthCode"
                    Return My.Settings.WCFAuthCode
                Case "WCFURL"
                    Return My.Settings.WCFURL
                Case "WCFTCPURL"
                    Return My.Settings.WCFTCPURL
                Case Else
                    Return ""
            End Select
            'Return System.Configuration.ConfigurationManager.AppSettings("Ngl.FreightMaster.Integration.WebServices.My.MySettings." & Setting)
            '*** Code Changed by RHR 11/23/09 We have reverted back to the web config file ***
            '*** But have excluded the need to pass the Authcode to read settings          ***

            'Dim strkey As String = ""
            'Dim value As String = System.Configuration.ConfigurationManager.AppSettings("Ngl.FreightMaster.Integration.WebServices.My.MySettings." & Setting) 'getXmlSetting(AuthorizationCode, Setting)
            '**************  Code Changed by RHR 11/19/08 Config Settings are now stored in an XML file
            ''strkey = GetSettingName(AuthorizationCode, Setting)
            'strkey = AuthorizationCode & Setting
            ''value = System.Configuration.ConfigurationManager.AppSettings.Get(strkey)
            ''If String.IsNullOrEmpty(value) Then
            ''    Throw New ApplicationException("Cannot read configuration settings.  Please check that you are providing a valid Authorization Code.")
            ''End If
            'Dim pi As System.Reflection.PropertyInfo = My.Settings.GetType.GetProperty(strkey, Reflection.BindingFlags.Public Or Reflection.BindingFlags.Instance)
            'value = pi.GetValue(My.Settings, Nothing).ToString()

            'Return value
        Catch ex As System.ApplicationException
            Throw
        Catch ex As System.NullReferenceException
            Throw New ApplicationException("Cannot read configuration settings.  Please check that you are providing a valid Authorization Code.")
        Catch ex As Exception
            Throw
        End Try

    End Function

    Private Shared Function getXmlSetting(ByVal AuthorizationCode As String, ByVal Setting As String) As String
        Dim dsXML As New System.Data.DataSet
        Dim strPath As String = System.AppDomain.CurrentDomain.BaseDirectory.ToString() & "App_Data\Config.xml"
        Dim strRet As String = ""
        Try
            dsXML.ReadXmlSchema(strPath)
            dsXML.ReadXml(strPath)

            Dim oTable As DataTable = dsXML.Tables(0)
            Dim oDRows As DataRow() = oTable.Select("AuthCode = '" & AuthorizationCode & "'")
            If oDRows.Length > 0 Then
                strRet = oDRows(0)(Setting).ToString
            Else
                Throw New ApplicationException("Cannot read configuration settings.  Please check that you are providing a valid Authorization Code.")
            End If
        Catch ex As Exception
            Throw
        End Try
        Return strRet
    End Function


    Private Shared Function GetSettingName(ByVal AuthorizationCode As String, ByVal Setting As String) As String

        Return String.Format("Ngl.FreightMaster.Integration.WebServices.My.MySettings.{0}{1}", AuthorizationCode, Setting)


    End Function

    Friend Shared Sub LogResults(ByVal ModuleName As String, ByVal Result As Integer, ByVal LastError As String, ByVal AuthorizationCode As String)

        Try
            Using sw As New IO.StreamWriter(My.MySettings.Default.LogFile, True)
                sw.WriteLine(String.Format("{0},{1},{2},{3},{4}", Now.ToString("MM/dd/yyyy hh:mm tt"),
                    ModuleName, Result, LastError, AuthorizationCode))
                sw.Close()
            End Using
        Catch ex As Exception

        End Try

    End Sub

    Friend Shared Sub LogException(ByVal ModuleName As String,
                                   ByVal Result As Integer,
                                   ByVal logMessage As String,
                                   ByVal ex As Exception,
                                   ByVal AuthorizationCode As String,
                                   Optional ByVal strHeader As String = "")
        LogResults(ModuleName, Result, logMessage & ex.ToString, AuthorizationCode)
        Try
            Dim strMsg As String = "<p>" & logMessage & "</p>" & vbCrLf
            If strHeader.Trim.Length > 0 Then
                strMsg = "<h2>" & strHeader & vbCrLf & "</h2>" & strMsg
            End If
            strMsg &= "<hr />" & vbCrLf
            strMsg &= ex.ToString & vbCrLf
            strMsg &= "<hr />" & vbCrLf & vbCrLf & "<p>Using Authorization Code: " & AuthorizationCode & "</p>"

            SendEmail(ModuleName, strMsg)
        Catch e As Exception
            'Because this function is typically called when we are processing exceptions
            'we do nothing when sending an email from the web service 

        End Try


    End Sub

    Friend Shared Sub LogMessage(ByVal ModuleName As String, ByVal Msg As String)

        Try
            Using sw As New IO.StreamWriter(My.MySettings.Default.LogFile, True)
                sw.WriteLine(String.Format("{0},{1},{2}", Now.ToString("MM/dd/yyyy hh:mm tt"), ModuleName, Msg))
                sw.Close()
            End Using
        Catch ex As Exception

        End Try

    End Sub


    Friend Shared Sub SendNotification(ByVal AuthorizationCode As String, ByVal MailTo As String, ByVal MailCc As String, ByVal Subject As String, ByVal Body As String)

        If MailTo = "" Then Exit Sub
        Try
            If My.MySettings.Default.DebugMode.ToLower = "true" Then Exit Sub
        Catch ex As Exception

        End Try
        Dim mailFrom As String = Utilities.GetSetting("EMailFrom")

        Dim sql As String = "INSERT INTO Mail (MailFrom, MailTo, MailCc, Subject, Body, ReadyToSend) "
        sql &= " VALUES ('" & mailFrom & "', '" & MailTo & "', '" & MailCc & "', '" & Subject.Replace("'", "''") & "', '" & Body.Replace("'", "''") & "', 1)"

        Dim cn As New SqlClient.SqlConnection(Utilities.GetConnectionString(AuthorizationCode))
        Dim cm As New SqlClient.SqlCommand(sql, cn)
        Try
            cn.Open()
            cm.ExecuteNonQuery()
        Catch ex As Exception

        Finally
            If cn.State = ConnectionState.Open Then cn.Close()
        End Try

    End Sub

    Public Shared Sub SendEmail(ByVal Subject As String,
                                ByVal Message As String)
        Try
            'Removed by RHR 12/16/2011
            'Dim oCoreEmail As New Ngl.Core.Communication.Email
            'oCoreEmail.SendMail(GetSetting("SmtpMailServer"), GetSetting("AdminEMail"), GetSetting("EMailFrom"), Message, Subject)

            Dim sql As String = "INSERT INTO Mail (MailFrom, MailTo, MailCc, Subject, Body, ReadyToSend) "
            sql &= " VALUES ('" & GetSetting("EMailFrom") & "', '" & GetSetting("AdminEMail") & "', ' ', '" & Subject.Replace("'", "''") & "', '" & Message.Replace("'", "''") & "', 1)"

            Dim cn As New SqlClient.SqlConnection(Utilities.GetConnectionString())
            Dim cm As New SqlClient.SqlCommand(sql, cn)
            Try
                cn.Open()
                cm.ExecuteNonQuery()
            Catch ex As Exception

            Finally
                If cn.State = ConnectionState.Open Then cn.Close()
            End Try

        Catch ex As Exception
            'Because this function is typically called when we are processing exceptions
            'we do nothing when sending an email from the web service 
        End Try
    End Sub

End Class
