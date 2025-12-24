Imports System.IO
Imports NGL.Core
Imports NGL.FreightMaster.Core
Imports NGL.Core.Communication
Imports BLL = NGL.FM.BLL


Public Class clsApplication : Inherits NGL.FreightMaster.Core.NGLCommandLineBaseClass
    Protected oConfig As New NGL.FreightMaster.Core.UserConfiguration

    Private _strAuthNbrFTPRoot As String = My.Settings.FTPURL
    ''' <summary>
    ''' Used to hold the FTP root path for the location of the new Auth Code File
    ''' Each Database will pass the AuthName value as the actual file name like NGLMAS2002.txt
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Property AuthNbrFTPRoot() As String
        Get
            Return _strAuthNbrFTPRoot
        End Get
        Set(ByVal value As String)
            _strAuthNbrFTPRoot = value
        End Set
    End Property

    ''' <summary>
    ''' Process Data
    ''' </summary>
    ''' <remarks>
    ''' Modified By LVV On 8/19/20 For v-8.3.0.001 - Task#20200817144456 - Modify the License File Integration Command Line Utility
    '''  Added logic to read the module license key from the license file. The license file has been modified to includes 
    '''  a bitwise number after the user count. This number is passed to the BLL to turn modules on or off.
    '''  Added logic to download the Page Footer Messages from the DMZ/REST service
    ''' </remarks>
    Public Sub ProcessData()
        Me.openLog()
        Me.Log(Source & " Applicaiton Start")
        'use the database name as part of the source
        displayParameterData()
        fillConfig()
        Dim oQuery As New NGL.Core.Data.Query
        oQuery.Database = Me.oConfig.Database
        oQuery.Server = Me.oConfig.DBServer
        If Not oQuery.testConnection() Then
            LogError(Source & " Database Connection Failure", "Actual error reported: " & oQuery.LastError & vbCrLf & vbCrLf & Me.DBInfo, Me.AdminEmail)
            Return
        End If
        Dim strFTPRoot As String = ""
        Try
            Log("Reading Auth Name From Database")
            Dim strSQL As String = "Select top 1 AuthName from dbo.Auth "
            Dim objCon As New System.Data.SqlClient.SqlConnection
            Dim strAuthName As String = oQuery.getScalarValue(objCon, strSQL)
            AuthNbrFTPRoot = AuthNbrFTPRoot.Trim
            If Right(AuthNbrFTPRoot, 1) <> "/" Then AuthNbrFTPRoot &= "/"
            strFTPRoot = AuthNbrFTPRoot & strAuthName & "/"
            Dim strFTPFile As String = strFTPRoot & "Authorization.txt"
            Log("Connecting to FTP SERVER:" & strFTPFile)


            Dim strLocaPath As String = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)

            Dim filename As String = strLocaPath & "\Authorization.txt"
            'DownLoad Data
            Dim c As New NetworkCredential("NextGenAuth", "@TMS!!2010")
            Dim request2 As FtpWebRequest = DirectCast(FtpWebRequest.Create(strFTPFile), FtpWebRequest)
            request2.Credentials = c
            request2.Method = WebRequestMethods.Ftp.DownloadFile
            Dim response2 As FtpWebResponse = DirectCast(request2.GetResponse(), FtpWebResponse)
            Dim responseStream As Stream = response2.GetResponseStream()
            Dim reader As StreamReader = New StreamReader(responseStream)

            Dim strAuthLine As String = "" 'the line read from the license file StreamReader
            Dim arrAuth As String() 'array to hold the values split from the csv

            Dim strAuthNumber As String = "" 'auth number
            Dim strAuthUsers As String = "" 'the number of allowed users FreightMaster can have
            Dim strModuleKey As String = "" 'Added By LVV On 8/19/20 For v-8.3.0.001 - Task#20200817144456 - Modify the License File Integration Command Line Utility

            strAuthLine = reader.ReadLine 'read the line
            arrAuth = strAuthLine.Split(",") 'split the line with a comma delimiter
            'set the authNumber and authUsers, and strModuleKey
            If arrAuth.Count > 1 Then
                strAuthNumber = arrAuth(0)
                strAuthUsers = arrAuth(1)
                'Added By LVV On 8/19/20 For v-8.3.0.001 - Task#20200817144456 - Modify the License File Integration Command Line Utility
                If arrAuth.Count > 2 Then strModuleKey = arrAuth(2) 'The code still needs to work if this value does not yet exist in the license file (This Module feature will be rolled out over time)
            Else
                Log("Execution Auth Number and User Access Failed, either the auth number or auth users is not present.")
            End If
            responseStream.Close()
            Log("Auth Number: " & strAuthNumber)
            Log("Auth Users: " & strAuthUsers)
            Log("Module License Key: " & strModuleKey) 'Added By LVV On 8/19/20 For v-8.3.0.001 - Task#20200817144456 - Modify the License File Integration Command Line Utility
            If Me.Debug Then
                Console.WriteLine("The License Will Now Expire on " & DecodeAuth(strAuthNumber))
            End If
            'Now update the database with the new auth number and new auth Users
            strSQL = "Update dbo.Auth set AuthNumber = '" & strAuthNumber & "' , AuthRef = " & strAuthUsers & " Where AuthName = '" & strAuthName & "'"
            oQuery.Execute(strSQL)
            'now we want to call the stored procedure to make sure the number of users are correct.
            Dim con As SqlClient.SqlConnection = oQuery.getNewConnection()
            Dim objCom As New SqlClient.SqlCommand
            Dim retErrorNbg As Integer = 0
            Dim retMsg As String = ""
            Try
                Dim strRetVal As String = ""
                With objCom
                    .Connection = con
                    .CommandTimeout = 600
                    .CommandText = "spTestUserAccess"
                    .CommandType = CommandType.StoredProcedure
                End With
                oQuery.execNGLStoredProcedure(con, objCom, "spTestUserAccess", 3, True, retMsg, retErrorNbg)
                If retErrorNbg = 2 Then
                    'Error - go ahead and email the message.
                    LogError(Source & " User Access Error", "There is a discrepancy between the authorized number of users and the actual number of users.  The actual error is: " & retMsg & vbCrLf & vbCrLf, Me.AdminEmail & ";info@maxximu.com")
                End If
                If retErrorNbg = 0 Then
                    'success
                End If
            Catch ex As Exception
                LogError(Source & " Unexpected Error", "An unexpected error has occurred while attempting to update the authorization number.  The actual error is: " & ex.Message & vbCrLf & vbCrLf, Me.AdminEmail)
            End Try
            'Added By LVV On 8/19/20 For v-8.3.0.001 - Task#20200817144456 - Modify the License File Integration Command Line Utility
            If WCFParameters Is Nothing Then WCFParameters = New FreightMaster.Data.WCFParameters
            With WCFParameters
                .UserName = "nglweb"
                .Database = Me.oConfig.Database
                .DBServer = Me.oConfig.DBServer
                .WCFAuthCode = "NGLSystem"
                .ConnectionString = Me.oConfig.ConnectionString
            End With
            Dim oBLL As New BLL.LicenseModuleBLL(WCFParameters)
            Try
                'Only do this if the Module License Key exists in the license file
                If Not String.IsNullOrWhiteSpace(strModuleKey) Then
                    Dim intLicenseKey As Integer = 0
                    If Integer.TryParse(strModuleKey, intLicenseKey) Then
                        oBLL.ConfigureModules(intLicenseKey)
                    Else
                        Log("Turn Modules On/Off Failed - Could not parse string Module Licnese key to an integer")
                    End If
                End If
            Catch ex As Exception
                LogError(Source & " Unexpected Error", "An unexpected error has occurred while attempting to activate or deactivate the Modules according to the Module License Key. The actual error is: " & ex.Message & vbCrLf & vbCrLf, Me.AdminEmail)
            End Try
            'Added By LVV On 8/19/20 For v-8.3.0.001 - Task#20200817144456 - Modify the License File Integration Command Line Utility
            Try
                oBLL.DownloadPageFooterMessages()
            Catch ex As Exception
                'modified by RHR for v-8.4.0.003 on 10/07/2021 removed email response to errror
                ' This functionality is not fully implemented so some errors are being reported each night
                '  for now we log the error in the log file but do not send an email
                ' LogError(Source & " Unexpected Error", "An unexpected error has occurred while attempting to update the Page Footer Messages from the DMZ. The actual error is: " & ex.Message & vbCrLf & vbCrLf, Me.AdminEmail)
                Log("An unexpected error has occurred while attempting to update the Page Footer Messages from the DMZ. The actual error is: " & ex.Message)
            End Try
            Log("Process Data Complete")
        Catch ex As System.Net.WebException
            LogError(Source & " FTP Connection Error", "There was a problem while connecting to the FTP site " & strFTPRoot & ". Please check that you are using the correct information. The actual error is: " & ex.Message & vbCrLf & vbCrLf, Me.AdminEmail)
        Catch ex As Exception
            LogError(Source & " Unexpected Error", "An unexpected error has occurred while attempting to update the authorization number.  The actual error is: " & ex.Message & vbCrLf & vbCrLf, Me.AdminEmail)
        Finally
            'do nothing
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



    Public Function DecodeAuth(ByVal strAuthNbr As String) As String

        Dim passnumber As Double = 0
        Dim passresult As Double = 0
        Dim passtext1 As String = ""
        Dim passtext2 As String = "*** NONE ***"
        Dim passfraction As Double = 0


        If Len(strAuthNbr) > 0 Then
            passnumber = CDbl(Val(strAuthNbr))
            passresult = passnumber - 11111111111.0#
            passresult = passresult / 24124
            passfraction = passresult - Int(passresult)
            If passfraction > 0 Then passresult = 19000101
            passtext1 = Trim(Str(passresult))
            passtext2 = Mid$(passtext1, 5, 2) & "/" & Mid$(passtext1, 7, 2) & "/" & Left$(passtext1, 4)

        End If

        Return passtext2

    End Function


End Class
