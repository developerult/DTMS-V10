Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports NGL.Core
Imports NGL.FreightMaster.Core
Imports NGL.Core.Communication
Imports Ngl.Core.Utility
Imports Ngl.Core.Communication.General
Imports NGLData = NGL.FreightMaster.Data
Imports BLL = NGL.FM.BLL
Imports DTO = NGL.FreightMaster.Data.DataTransferObjects

Public Class clsApplication : Inherits NGL.FreightMaster.Core.NGLCommandLineBaseClass
    Protected oConfig As New NGL.FreightMaster.Core.UserConfiguration
    Public Shared sErrMsg As String = "Unknown Error!"
    Public wcfParameters As New NGLData.WCFParameters
    Public Shared ExpiredLoadsTo As String = ""
    Public Shared ExpiredLoadsCc As String = ""
    Public Shared CarrierLoadAcceptanceAllowedMinutes As Integer = 0
    Public Shared consolidatedLoads As New Generic.List(Of String)
    Public Shared strCNSNumber As String = ""

    'these three parameters come from the db
    ' Public Shared mintAllowedMinutes As Integer = "0" 'objIniFile.GetInteger(mstrINIKey, "AllowedMinutes", mintAllowedMinutes)
    'Public Shared mstrWebDBServer As String = ""
    'Public Shared mstrWebDatabase As String = ""



    'Dim _connectionString As String
    'Dim _reportPath As String
    'Dim _notificationTo As String
    'Dim _notificationCC As String
    'Public Shared mblnDebug As Boolean = False
    'Public Shared mstrUserName As String = ""
    'Public Shared mstrPassword As String = ""
    'Public Shared mstrServer As String = ""
    'Public Shared mstrLocalFolder As String = ""
    'Public Shared mstrRemoteFolder As String = ""
    'Public Shared mstrLocalBackupFolder As String = ""
    'Public Shared mstrRemoteBackupFolder As String = ""
    'Public Shared mstrPOFilter As String = "PO*.*"
    'Public Shared mstrLaneFilter As String = "Lane*.*"
    'Public Shared mstrCompFilter As String = "Comp*.*"
    'Public Shared mstrCompContFilter As String = "CompanyContact*.*"
    'Public Shared mstrCarrContFilter As String = "CarrierContact*.*"
    'Public Shared mstrCarrFilter As String = "Carr*.*"
    'Public Shared mstrSchedFilter As String = "Sched*.*"
    'Public Shared mstrPayFilter As String = "Pay*.*"
    'Public Shared mstrPOHeaderFilter As String = "POHeader*.*"
    'Public Shared mstrPODetailFilter As String = "PODetail*.*"
    'Public Shared mstrExternalProcessingFile As String = ""
    'Public Shared mstrInternalProcessingFile As String = ""
    'Public Shared mstrResultsFile As String = ""
    'Public Shared mstrINIKey As String = "NGL"
    'Public Shared mstrTransferType As String = "d"
    'Public Shared mintAutoRetry As Integer = 3
    'Public Shared mstrDatabase As String = ""
    'Public Shared mstrDBServer As String = ""
    'Public Shared mstrAdminEmail As String = ""
    'Public Shared mstrGroupEmail As String = ""
    'Public Shared mstrFromEmail As String = "system@nextgeneration.com"
    'Public Shared mstrBatchFormat As String = ""
    ' Public Shared mstrSMTPServer As String = "mail.ngl.local"
    'Public Shared mblnGenerateHASH As Boolean = True
    'Public Shared mstrHashFileName As String = "HashPOTotals"
    'Public Shared mintKeepLogDays As Integer = 30
    'Public Shared mblnSaveOldLog As Boolean = True
    'Public Shared mstrWebDBServer As String = ""
    'Public Shared mstrWebDatabase As String = ""
    'Public Shared sSource As String = "Load Expiration "

    'Private Shared objF As New clsStandardFunctions
    'Protected Shared objFTP As New NGL.FreightMaster.FMLib.FTP
    'Protected Shared EvtLog As New System.Diagnostics.EventLog
   

    

    Public Sub ProcessData()

        Me.openLog()
        Me.Log(Source & " Applicaiton Start")
        'use the database name as part of the source
        displayParameterData()
        fillConfig()
        setWCF()
        Dim oQuery As New NGL.Core.Data.Query
        oQuery.Database = Me.oConfig.Database
        oQuery.Server = Me.oConfig.DBServer
        If Not oQuery.testConnection() Then
            LogError(Source & " Database Connection Failure", "Actual error reported: " & oQuery.LastError & vbCrLf & vbCrLf & Me.DBInfo, Me.AdminEmail)
            Return
        End If

        
        Try
            Log("Begin Process Data ")
            Me.ProcessExpirations()
            Log("Process Data Complete")
        Catch ex As Exception
            LogError(Source & " Unexpected ProcessExpirations", "An unexpected error has occurred while attempting to ProcessExpirations.  The actual error is: " & ex.Message & vbCrLf & vbCrLf & Me.DBInfo, Me.AdminEmail)
        Finally

        End Try

    End Sub

    Public Sub fillConfig()
        Try
            With oConfig
                .AdminEmail = Me.AdminEmail
                .AutoRetry = Me.AutoRetry
                .ConnectionString = ConnectionString
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
    Public Sub setWCF()
        If wcfParameters Is Nothing Then wcfParameters = New NGLData.WCFParameters
        With wcfParameters
            .UserName = "nglweb"
            .Database = Database
            .DBServer = DBServer
            .WCFAuthCode = "NGLSystem"
            .ConnectionString = "Data Source=" & .DBServer & ";Initial Catalog=" & .Database & ";Integrated Security=True"
        End With
    End Sub
    Public Function ToSql(ByVal text As String, Optional ByVal IncludeTrailingComma As Boolean = True, Optional ByVal AllowNull As Boolean = True) As String

        If text = String.Empty And AllowNull Then
            text = "NULL"
        Else
            text = "'" & text.Replace("'", "''").Replace(";", "") & "'"
        End If

        If IncludeTrailingComma Then text &= ","
        Return text
    End Function
    Private Function ProcessCarrierAcceptOrRejectData(ByVal BookControl As Integer _
                                                    , ByVal CarrierControl As Integer _
                                                    , ByVal CarrierContControl As Integer _
                                                    , ByVal AcceptRejectCode As Integer _
                                                    , ByVal sendEMail As Boolean _
                                                    , ByVal BookTrackComment As String _
                                                    , ByVal BookTrackStatus As Integer _
                                                    , ByVal WebUserName As String _
                                                    , ByVal NotificationEMailAddress As String _
                                                    , ByVal NotificationEMailAddressCc As String) As Boolean


        Dim blnRet As Boolean = False
        Dim strCriteria As String = ""

        If strCNSNumber <> "" AndAlso Not consolidatedLoads.Contains(strCNSNumber) Then
            consolidatedLoads.Add(strCNSNumber)
        End If

        Dim oCon As New SqlConnection(ConnectionString)
        Try
            oCon.Open()
            Dim intRetryCt As Integer = 0
            Do
                intRetryCt += 1
                Dim objCom As New SqlCommand
                Try
                    Dim lngErrNumber As Long
                    Dim strRetVal As String = ""
                    With objCom
                        .Connection = oCon
                        .CommandTimeout = 600
                        .Parameters.Add("@BookControl", SqlDbType.Int)
                        .Parameters("@BookControl").Value = BookControl
                        .Parameters.Add("@CarrierControl", SqlDbType.Int)
                        .Parameters("@CarrierControl").Value = CarrierControl
                        .Parameters.Add("@CarrierContControl", SqlDbType.Int)
                        .Parameters("@CarrierContControl").Value = CarrierContControl
                        .Parameters.Add("@AcceptRejectCode", SqlDbType.Int)
                        .Parameters("@AcceptRejectCode").Value = AcceptRejectCode
                        .Parameters.Add("@sendEMail", SqlDbType.Bit)
                        .Parameters("@sendEMail").Value = sendEMail
                        .Parameters.Add("@BookTrackComment", SqlDbType.NVarChar, 255)
                        If String.IsNullOrEmpty(BookTrackComment) Then
                            .Parameters("@BookTrackComment").Value = DBNull.Value
                        Else
                            .Parameters("@BookTrackComment").Value = ToSql(BookTrackComment, False)
                        End If
                        .Parameters.Add("@BookTrackStatus", SqlDbType.Int)
                        .Parameters("@BookTrackStatus").Value = BookTrackStatus
                        .Parameters.Add("@WebUserName", SqlDbType.NVarChar, 256)
                        If String.IsNullOrEmpty(WebUserName) Then
                            .Parameters("@WebUserName").Value = DBNull.Value
                        Else
                            .Parameters("@WebUserName").Value = ToSql(WebUserName, False)
                        End If
                        .Parameters.Add("@NotificationEMailAddress", SqlDbType.NVarChar, 500)
                        If String.IsNullOrEmpty(NotificationEMailAddress) Then
                            .Parameters("@NotificationEMailAddress").Value = DBNull.Value
                        Else
                            .Parameters("@NotificationEMailAddress").Value = ToSql(NotificationEMailAddress, False)
                        End If
                        .Parameters.Add("@NotificationEMailAddressCc", SqlDbType.NVarChar, 500)
                        If String.IsNullOrEmpty(NotificationEMailAddressCc) Then
                            .Parameters("@NotificationEMailAddressCc").Value = DBNull.Value
                        Else
                            .Parameters("@NotificationEMailAddressCc").Value = ToSql(NotificationEMailAddress, False)
                        End If
                        .Parameters.Add("@RetMsg", SqlDbType.NVarChar, 1000)
                        .Parameters("@RetMsg").Direction = ParameterDirection.Output
                        .Parameters.Add("@ErrNumber", SqlDbType.Int)
                        .Parameters("@ErrNumber").Direction = ParameterDirection.Output
                        .CommandText = "spCarrierAcceptOrRejectLoad"
                        .CommandType = CommandType.StoredProcedure
                        .ExecuteNonQuery()
                        strRetVal = Trim(.Parameters("@RetMsg").Value.ToString)
                        If IsDBNull(.Parameters("@ErrNumber").Value) Then
                            lngErrNumber = 0
                        Else
                            lngErrNumber = .Parameters("@ErrNumber").Value
                        End If
                    End With
                    Try
                        If lngErrNumber <> 0 Then
                            Throw New ApplicationException("Process Carrier Expiration Error #" & lngErrNumber & ": " & strRetVal)
                        End If
                    Catch ex As Exception
                        'do nothing here
                    End Try
                    blnRet = True
                    Exit Do
                Catch ex As ApplicationException
                    Throw
                Catch ex As Exception
                    If intRetryCt > 3 Then
                        Throw New ApplicationException("Process Carrier Accept or Reject Failure: " & ex.Message)
                    End If
                Finally
                    Try
                        objCom.Cancel()
                        objCom = Nothing
                    Catch ex As Exception

                    End Try
                End Try
                'We only get here if an exception is thrown and intRetryCt <= 3
            Loop Until intRetryCt > 3 'this should never happen the code is here to show our intention.
        Catch ex As ApplicationException
            Throw
        Catch ex As Exception
            Throw New ApplicationException(ex.Message, ex.InnerException)
        Finally
            Try
                If oCon.State = System.Data.ConnectionState.Open Then oCon.Close()
                oCon = Nothing
            Catch ex As Exception

            End Try
        End Try
        Return blnRet
    End Function


    Public Sub getWebDefaults(ByVal CompControl As Integer)

        Dim sqlStatement As String = "Select CarrierLoadAcceptanceAllowedMinutes, ExpiredLoadsTo, ExpiredLoadsCc From dbo.CompanyConfiguration Where CompanyId = " & CompControl
        Dim cn As New SqlConnection(OpenConnectionWeb())
        ' Dim cn As New SqlConnection(OpenConnection())
        Dim cm As New SqlCommand(sqlStatement, cn)
        Dim da As New SqlDataAdapter(cm)
        Dim dt As New DataTable
        Try
            cn.Open()
            da.Fill(dt)
            If dt.Rows.Count < 1 Then
                'get the data for company zero (defaults)
                sqlStatement = "Select CarrierLoadAcceptanceAllowedMinutes, ExpiredLoadsTo, ExpiredLoadsCc From dbo.CompanyConfiguration Where CompanyId = 0"
                cm.CommandText = sqlStatement
                da = New SqlDataAdapter(cm)
                dt = New DataTable
                da.Fill(dt)
            End If
        Catch ex As Exception
            sErrMsg = formatErrMsg(ex, Source)
            LogError("Load Expired Failure", "Load Expired Calling Stored Proc " & vbCrLf & sErrMsg & vbCrLf & vbCrLf & Me.NEXTRackDatabase, Me.NEXTRackDatabaseServer)
            EvtLog.WriteEntry(sErrMsg, EventLogEntryType.Error)
        Finally
            If cn.State = ConnectionState.Open Then cn.Close()
        End Try
        If dt.Rows.Count < 1 Then
            CarrierLoadAcceptanceAllowedMinutes = Me.GlobalDefaultLoadAcceptAllowedMinutes
            ExpiredLoadsTo = ""
            ExpiredLoadsCc = ""
        Else
            CarrierLoadAcceptanceAllowedMinutes = DataTransformation.FixNullInteger(dt.Rows(0).Item("CarrierLoadAcceptanceAllowedMinutes"))
            ExpiredLoadsTo = nz(dt.Rows(0).Item("ExpiredLoadsTo"), "")
            ExpiredLoadsCc = nz(dt.Rows(0).Item("ExpiredLoadsCc"), "")
        End If
    End Sub

    Private Function HasExpired(ByVal loadAssigned As Object) As Boolean

        If IsDate(loadAssigned) = False Then Return False

        Dim minutesSinceOffered As Integer = _
            DateDiff(DateInterval.Minute, CDate(loadAssigned), Now)
        If Me._GlobalDefaultLoadAcceptAllowedMinutes = 0 Then
            Return False
        ElseIf minutesSinceOffered >= CarrierLoadAcceptanceAllowedMinutes Then
            Return True
        End If

        Return False

    End Function

    Private Sub ExpireLoad(ByVal row As DataRow)

        Console.WriteLine("Expiring Load " & row("BookControl"))
        Dim proNumber As String = row("BookProNumber").ToString
        Dim bookControl As Integer = row("BookControl")
        Dim carrierNumber As Integer = row("CarrierNumber")
        Dim CarrierControl As Integer = row("CarrierControl")
        Dim BookCarrierContControl As Integer = row("BookCarrierContControl")
        Dim carrEmail As String = nz(row("CarrierEMail"), "")
        strCNSNumber = nz(row("BookConsPrefix"), "").ToString
        Dim blnSendEmail As Boolean = True
        If strCNSNumber <> "" AndAlso consolidatedLoads.Contains(strCNSNumber) Then blnSendEmail = False

        Try
            'NOTE:  We pass 0 as the carrier and carrier contact to allow for cascading dispatching
            ProcessCarrierAcceptOrRejectData(bookControl _
                                        , CarrierControl _
                                        , BookCarrierContControl _
                                        , 2 _
                                        , blnSendEmail _
                                        , "Web Tendered Load Has Expired" _
                                        , 0 _
                                        , "ProcessExpiredLoads" _
                                        , ExpiredLoadsTo _
                                        , ExpiredLoadsCc)
        Catch ex As Exception
            sErrMsg = formatErrMsg(ex, Source)
            LogError("Load Expired Failure", String.Format("Unable to process expired load for Pro number {0} assigned to carrier number {1}.  The actual error message is:", proNumber, carrierNumber) & vbCrLf & vbCrLf & sErrMsg & vbCrLf & vbCrLf & Me.DBInfo, Me.AdminEmail)
            EvtLog.WriteEntry(sErrMsg, EventLogEntryType.Error)
        End Try
    End Sub


    Public Shared Function nz(ByVal source As Object, ByVal defaultvalue As Object) As Object


        If source Is Nothing Or source Is DBNull.Value Then
            Return defaultvalue
        ElseIf Len(Trim(source & " ")) < 1 Then
            Return defaultvalue
        Else
            Return source
        End If

    End Function

    Public Sub ProcessExpirations()
         
        Dim bookBll As New BLL.NGLBookBLL(wcfParameters)
        'Dim list As Object = bookBll.BookRecordsPendingAcceptance()
        Dim sqlStatement As String = ""
        sqlStatement = "[spBookRecordsPendingAcceptance] " 'NOTE: Replace this statement with a call to NGL.FM.BLL.NGLBookBLL.BookRecordsPendingAcceptance'
        Dim cn As New SqlConnection(Me.ConnectionString)

        Dim cm As New SqlCommand(sqlStatement, cn)
        Dim da As New SqlDataAdapter(cm)
        Dim dt As New DataTable
        Try
            cn.Open()
            da.Fill(dt)
        Catch ex As Exception
            sErrMsg = formatErrMsg(ex, Source)
            LogError(Source & " Load Expired Failure", "Load Expired Calling Stored Proc: " & sErrMsg & vbCrLf & vbCrLf & Me.DBInfo, Me.AdminEmail)
            EvtLog.WriteEntry(sErrMsg, EventLogEntryType.Error)
        Finally
            If cn.State = ConnectionState.Open Then cn.Close()
        End Try


        For Each row As DataRow In dt.Rows
            getWebDefaults(DataTransformation.FixNullInteger(row.Item("BookCustCompControl")))

            If HasExpired(row("BookTrackDate")) Then
                ExpireLoad(row)
            End If
        Next

    End Sub


    Private _EventLogger As EventLog
    Public Property EvtLog() As EventLog
        Get
            Return _EventLogger
        End Get
        Set(ByVal value As EventLog)
            _EventLogger = value
        End Set
    End Property

    Public Function OpenConnectionWeb() As String
        Return "Data Source=" & nz(Me.NEXTRackDatabaseServer, "NGLSQL01T") & ";" & "Initial Catalog=" & nz(Me.NEXTRackDatabase, "NGLWebDev") & ";" & "Integrated Security=SSPI"
    End Function
End Class
