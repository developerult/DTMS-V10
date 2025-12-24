Imports System
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.ServiceModel
Imports Ngl.FreightMaster.Core
Imports NGL.Core.Communication
Imports DTran = NGL.Core.Utility.DataTransformation
Imports NGL.Core.Communication.Email
Imports NGL.Core.Communication.General
Imports NData = NGL.FreightMaster.Data

Public MustInherit Class EDIFileCore : Inherits NGL.FreightMaster.Core.NGLCommandLineBaseClass
    Protected oConfig As New NGL.FreightMaster.Core.UserConfiguration
#Region " Properties "

#Region " Log Data Properties "
    Protected mstrCreatedDate As String = Date.Now.ToString
    Public Property CreatedDate() As String
        Get
            Return mstrCreatedDate
        End Get
        Set(ByVal Value As String)
            mstrCreatedDate = Value
        End Set
    End Property

    Protected mstrCreateUser As String = "System Data Integration"
    Public Property CreateUser() As String
        Get
            Return mstrCreateUser
        End Get
        Set(ByVal Value As String)
            mstrCreateUser = Value
        End Set
    End Property
#End Region

#Region " Return Values "
    Private _intRowsAffected As Integer = 0
    Public Property RowsAffected() As Integer
        Get
            Return _intRowsAffected
        End Get
        Protected Set(ByVal value As Integer)
            _intRowsAffected = value
        End Set
    End Property


    Private mintItemErrors As Integer = 0
    Public Property ItemErrors() As Integer
        Get
            ItemErrors = mintItemErrors
        End Get
        Protected Set(ByVal Value As Integer)
            mintItemErrors = Value
        End Set
    End Property
    Private mintRecordErrors As Integer = 0
    Public Property RecordErrors() As Integer
        Get
            RecordErrors = mintRecordErrors
        End Get
        Protected Set(ByVal Value As Integer)
            mintRecordErrors = Value
        End Set
    End Property

    Private mintTotalRecords As Integer = 0
    Public Property TotalRecords() As Integer
        Get
            TotalRecords = mintTotalRecords
        End Get
        Set(ByVal Value As Integer)
            mintTotalRecords = Value
        End Set
    End Property

    Private mintTotalWarnings As Integer = 0
    Public Property TotalWarnings() As Integer
        Get
            TotalWarnings = mintTotalWarnings
        End Get
        Set(ByVal Value As Integer)
            mintTotalWarnings = Value
        End Set
    End Property

    Private mintTotalFailures As Integer = 0
    Public Property TotalFailures() As Integer
        Get
            TotalFailures = mintTotalFailures
        End Get
        Set(ByVal Value As Integer)
            mintTotalFailures = Value
        End Set
    End Property

#End Region

#Region " Email Properties "
    Private _strEmailMsg As String = ""
    Protected Property EmailMsg() As String
        Get
            Return _strEmailMsg
        End Get
        Set(ByVal value As String)
            _strEmailMsg = value
        End Set
    End Property
#End Region

#Region " General "

    Private _WCFPars As NData.WCFParameters
    Protected ReadOnly Property WCFPars As NData.WCFParameters
        Get
            If _WCFPars Is Nothing Then
                _WCFPars = New NData.WCFParameters
                With _WCFPars
                    .Database = Me.Database
                    .DBServer = Me.DBServer
                    .ConnectionString = Me.ConnectionString
                    .UserName = Me.CreateUser
                    'We use NGLSystem when calling the data library directly from managed code
                    'This forces the data library to use integrated windows authentication and 
                    'bypasses the WCFAuthCode requirement
                    .WCFAuthCode = "NGLSystem"
                End With
            End If
            Return _WCFPars
        End Get
    End Property

    Private _intTimeout As Integer = 0
    Public Property CommandTimeOut() As Integer
        Get
            If _intTimeout < 300 Then
                _intTimeout = 300
            End If
            Return _intTimeout
        End Get
        Set(ByVal value As Integer)
            _intTimeout = value
        End Set
    End Property

    Private mintTotalItems As Integer = 0
    Public Property TotalItems() As Integer
        Get
            TotalItems = mintTotalItems
        End Get
        Protected Set(ByVal Value As Integer)
            mintTotalItems = Value
        End Set
    End Property


    Protected mstrItemName As String = ""
    Public Property ItemName() As String
        Get
            Return mstrItemName
        End Get
        Set(ByVal Value As String)
            mstrItemName = Value
        End Set
    End Property

    Protected mstrHeaderName As String = ""
    Public Property HeaderName() As String
        Get
            Return mstrHeaderName
        End Get
        Set(ByVal Value As String)
            mstrHeaderName = Value
        End Set
    End Property

    Private _FileData As String = ""
    Public Property FileData() As String
        Get
            Return _FileData
        End Get
        Set(ByVal value As String)
            _FileData = value
        End Set
    End Property

    Private _fileFilter As String = ""
    Public Property FileFilter() As String
        Get
            Return _fileFilter
        End Get
        Set(ByVal value As String)
            _fileFilter = value
        End Set
    End Property

    Private _FileName As String = ""
    Public Property FileName() As String
        Get
            'If _FileName.Trim.Length < 5 Then _FileName = buildFileName()
            Return _FileName
        End Get
        Set(ByVal value As String)
            _FileName = value
        End Set
    End Property


    Private _BackupFileName As String = ""
    Public Property BackupFileName() As String
        Get
            If _BackupFileName.Trim.Length < 5 Then _BackupFileName = buildBackupFileName()
            Return _BackupFileName
        End Get
        Set(ByVal value As String)
            _BackupFileName = value
        End Set
    End Property

    ''' <summary>
    ''' Inbound EDI Configuration path and filename 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 On 6/2/2017
    ''' </remarks>
    Public ReadOnly Property InboundConfigFileName() As String
        Get
            Return Me.SFun.buildPath(InboundFilesFolder, "EDIConfig.txt")
        End Get
    End Property

    ''' <summary>
    ''' Outbound EDI Configuration path and filename 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.6.105 On 6/2/2017
    ''' </remarks>
    Public ReadOnly Property OutboundConfigFileName() As String
        Get
            Return Me.SFun.buildPath(OutboundFilesFolder, "EDIConfig.txt")
        End Get
    End Property

    Public RowData As String = ""
    Public RowCount As Integer = 0
#End Region

#Region "EDIParameters"
    Public CarrierEDIControl As Integer = 0
    Public InboundFilesFolder As String = "C:\"
    Public OutboundFilesFolder As String = "C:\"
    Public BackupFolder As String = "C:\"
    Public EDILogFile As String = "C:\EDILog.txt"
    Public StartTime As String = "09:00 AM"
    Public EndTime As String = "05:00 PM"
    Public DaysOfWeek As String = "Mon,Tue,Wed,Thu,Fri"
    Public SendMinutesOutbound As Integer = 30
    Public FileNameBaseOutbound As String = "EDIOut"
    Public FileNameBaseInbound As String = "EDIIn"
    Public CarrierNumber As String = ""
    Public CarrierControl As Integer = 0
    Public CarrierName As String = ""
    Public AuthorizationCode As String = ""
    Public EDIEmailNotificationOn As Boolean = False
    Public EDIEmailAddress As String = ""
    Public LastOutboundTransmission As Date = Date.Now.AddDays(-1)
    Public CarrierEDISend997 As Boolean = False 'Added By LVV On 10/10/19 - We didn't want to keep sending error emails for if the 997 config does not exist, or if the AcknowledgementRequested flag is off for 210 and 214 (support case 201910021431)

#End Region

#Region "Modual Level Objects"
    Protected mioFailed As System.IO.StreamWriter
    Protected moFields As clsIntegrationFields
    Protected moBadRows As clsBadRows
    Public Property BadRows() As clsBadRows
        Get
            Return moBadRows
        End Get
        Set(ByVal value As clsBadRows)
            moBadRows = value
        End Set
    End Property

#End Region

#End Region

#Region " Protected Methods "

#End Region

#Region " Public Methods "
    '********Merge from 6.0.4.70*************

    ''' <summary>
    ''' Read the text data from the file identified in the FileName property
    ''' strip out any cr-lf characters and store the single line of data 
    ''' into the FileData property.  Returns true only if it is possible to 
    ''' read the data.  If the file is empty we create an Email Error Message and return false
    ''' </summary>
    ''' <param name="strErrMsg"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Function Read(Optional ByVal strErrMsg As String = "") As Boolean
        Dim strRetMsg As String = ""

        Dim oFile As New System.IO.FileInfo(FileName)
        Dim lastwrite As Date = Date.Now
        Dim lastSize As Long = -1
        Dim blnStillWriting As Boolean = True
        Dim blnReadyToRead As Boolean = False
        Dim intRetry As Integer = 0
        Dim dtStartTime As Date = Date.Now()
        Dim blnRet As Boolean = False
        Try
            If oFile.Exists Then
                If Verbose Then Log("File exists checking if data can be processed")
                Do While blnStillWriting
                    oFile.Refresh()
                    If lastSize < 0 Then
                        'first read
                        blnStillWriting = True
                        lastSize = oFile.Length
                        If Verbose Then Log("The initial file size is = " & lastSize)
                    ElseIf lastSize < oFile.Length Then
                        blnStillWriting = True
                        lastSize = oFile.Length
                        If lastSize > 0 And Verbose Then Log("File size has changed,  new file size = " & lastSize)
                    ElseIf oFile.LastWriteTime > lastwrite Then
                        blnStillWriting = True
                        lastwrite = oFile.LastWriteTime
                        If Verbose Then Log("File was last changed on " & lastwrite.ToShortDateString & "  " & lastwrite.ToShortTimeString)
                    ElseIf lastSize < 1 Then
                        If Verbose Then Log("File is still empty")
                        blnStillWriting = True
                    Else
                        blnStillWriting = False
                    End If

                    If intRetry > 9 Then
                        If lastSize < 1 Then
                            strRetMsg = strErrMsg & " failed.  The file " & FileName & " is still empty after  " & (Date.Now() - dtStartTime).Seconds.ToString & " seconds; skipping file until next cycle.  To prevent this error in the future you must manually correct or delete the file."
                        Else
                            strRetMsg = strErrMsg & " failed.  The file " & FileName & " is still changing after  " & (Date.Now() - dtStartTime).Seconds.ToString & " seconds; skipping file until next cycle.  To prevent this error in the future you must manually correct or delete the file."
                        End If
                        LogError(Me.Source & " Cannot Read EDI Data", strRetMsg, Me.AdminEmail)
                        Return False
                        Exit Do
                    Else
                        intRetry += 1
                        If blnStillWriting Or lastSize < 1 Then
                            System.Threading.Thread.Sleep(1000)
                            If intRetry > 1 Then
                                'we always retry on the first try so we do not log the first attempt 
                                Log(String.Format("Reading file {0} retry # {1}", FileName, intRetry))
                            End If
                        End If
                    End If
                Loop

                If lastSize > 0 Then
                    Using sr = oFile.OpenText
                        If Verbose Then Log("Reading each line of text from the file")
                        Dim intLines As Integer = 1
                        Do While sr.Peek() >= 0
                            If Verbose Then Log("Reading Line # " & intLines)
                            'the ReadLine method removes all cr-lf characters from the file returning only one long string of data
                            FileData &= sr.ReadLine() & " "
                            intLines += 1
                        Loop
                        sr.Close()
                        If Verbose Then Log("File, " & FileName & ", is now closed.")
                    End Using
                    If String.IsNullOrEmpty(FileData) OrElse FileData.Trim.Length < 1 Then
                        strRetMsg = strErrMsg & " failed.  Cannot read the data from the file " & FileName & ". "
                        LogError(Me.Source & " Cannot Read EDI Data", strRetMsg, Me.AdminEmail)
                        Return False
                    End If
                Else
                    strRetMsg = strErrMsg & " failed.  The file " & FileName & " is still empty after " & (Date.Now() - dtStartTime).Seconds.ToString & " seconds.  To prevent this error in the future you must manually correct or delete the file."
                    LogError(Me.Source & " Cannot Read EDI Data", strRetMsg, Me.AdminEmail)
                    Return False
                End If
            Else
                strRetMsg = strErrMsg & " failed.  The file " & FileName & " does not exist or cannot be opened."
                FileData = ""
                LogError(Me.Source & " Cannot Read EDI Data", strRetMsg, Me.AdminEmail)
                Return False
            End If
            If Verbose Then Log("Data has been read from file, " & FileName & ", ready to backup and save.")
            blnRet = True
        Catch ex As System.OutOfMemoryException
            'The file size is too large
            strRetMsg = strErrMsg & " failed.  The file " & FileName & " is too large to load into the available memory.  Please check the file for errors.  To prevent this error in the future you must manually correct or delete the file."
            FileData = ""
            LogError(Me.Source & " Cannot Read EDI Data", strRetMsg, Me.AdminEmail)
        Catch ex As System.IO.IOException
            strRetMsg = strErrMsg & " failed.  The file " & FileName & " could not be read due to the following exception: " & vbCrLf & readExceptionMessage(ex) & vbCrLf & "To prevent this error in the future you must manually correct or delete the file."
            FileData = ""
            LogError(Me.Source & " Cannot Read EDI Data", strRetMsg, Me.AdminEmail)
        Catch ex As Exception
            Throw
        End Try
        Return blnRet
    End Function


    ''' <summary>
    ''' Read data from the file identified in the FileName property 
    ''' and save the contents to the FileData property
    ''' </summary>
    ''' <param name="strErrMsg"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' From 6047: This method has been depreciated.  the old code can be removed after version 6.0.4
    ''' Changed name to Read_OLD 1/23/18
    ''' </remarks>
    Public Overridable Function Read_OLD(Optional ByVal strErrMsg As String = "") As Boolean
        Dim blnRet As Boolean = False
        Try
            'now open the file and read the data
            Using sr As New StreamReader(FileName)
                FileData = sr.ReadToEnd
            End Using
            blnRet = True
        Catch ex As System.OutOfMemoryException
            'The file size is too large
            Dim strMsg As String = strErrMsg & " read file data failed.  The file " & FileName & " is too large to load into the available memory.  Please check the file for errors."
            LogError(Me.Source & " Cannot Read EDI Data", strMsg, Me.AdminEmail)
        Catch ex As System.IO.IOException
            Dim strMsg As String = strErrMsg & " read file data failed.  The file " & FileName & " could not be read due to the following exception: " & vbCrLf & readExceptionMessage(ex)
            LogError(Me.Source & " Cannot Read EDI Data", strMsg, Me.AdminEmail)
        Catch ex As Exception
            Throw
        End Try
        Return blnRet
    End Function

    '********Merge from 6.0.4.70*************

    ''' <summary>
    ''' Checks if the file identified in the FileName property exists
    ''' If not it creates it.  Writes data from FileData property to
    ''' the file.
    ''' </summary>
    ''' <param name="strErrMsg"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Function Save(Optional ByVal strErrMsg As String = "") As Boolean
        Dim blnRet As Boolean = False
        Try

            If String.IsNullOrEmpty(FileData) OrElse FileData.Trim.Length < 100 Then Return True

            Dim fi As FileInfo = New FileInfo(FileName)
            'create the file if it does not exist
            If Not File.Exists(FileName) Then
                Using w As StreamWriter = fi.CreateText
                    w.Close()
                End Using
            End If
            'now open the file and read the data
            Using sw As New StreamWriter(FileName)
                sw.Write(FileData)
                sw.Flush()
            End Using
            blnRet = True
        Catch ex As System.ObjectDisposedException
            'The file size is too large
            Dim strMsg As String = strErrMsg & " write file data failed.  The file " & FileName & " is closed, is no longer available or has reached its maximum capacity."
            LogError(Me.Source & " Cannot Read EDI Data", strMsg, Me.AdminEmail)

        Catch ex As System.NotSupportedException
            'The file size is too large
            Dim strMsg As String = strErrMsg & " write file data failed.  The file " & FileName & " does not support writing or has reached its maximum capacity."
            LogError(Me.Source & " Cannot Read EDI Data", strMsg, Me.AdminEmail)
        Catch ex As System.IO.IOException
            Dim strMsg As String = strErrMsg & " write file data failed.  Could not write to file " & FileName & " due to the following exception: " & vbCrLf & readExceptionMessage(ex)
            LogError(Me.Source & " Cannot Read EDI Data", strMsg, Me.AdminEmail)
        Catch ex As Exception
            Throw
        End Try
        Return blnRet
    End Function

    Public Function buildBackupFileName() As String
        Dim strRet As String = ""
        Try
            Dim fi As FileInfo = New FileInfo(FileName)
            Dim strFile As String = fi.Name
            strRet = Me.SFun.buildPath(BackupFolder, strFile)

        Catch ex As Exception
            If Debug Then Console.WriteLine("Build Backup File Name Error (ignored at run time): " & ex.ToString)
        End Try
        Return strRet
    End Function


    Public Function ReadCarrierEDIConfig(ByVal EDIXaction As String) As Boolean
        Dim blnRet As Boolean = False
        Dim oCarrierEDI As New NData.NGLCarrierEDIData(Me.WCFPars)
        Try
            'if action allowed get the configuration data (returns an array of CarrierEDI object
            Dim oList As NData.DataTransferObjects.CarrierEDI() = oCarrierEDI.GetCarrierEDIsFiltered(Me.CarrierControl, EDIXaction)
            If Not oList Is Nothing AndAlso oList.Count > 0 Then
                'We read the data from the first record returned
                With oList(0)
                    Me.CarrierEDIControl = .CarrierEDIControl
                    Me.InboundFilesFolder = .CarrierEDIInboundFolder
                    Me.OutboundFilesFolder = .CarrierEDIOutboundFolder
                    Me.BackupFolder = .CarrierEDIBackupFolder
                    Me.EDILogFile = .CarrierEDILogFile
                    Me.StartTime = .CarrierEDIStartTime
                    Me.EndTime = .CarrierEDIEndTime
                    Me.DaysOfWeek = .CarrierEDIDaysOfWeek
                    Me.SendMinutesOutbound = .CarrierEDISendMinutesOutbound
                    Me.FileNameBaseOutbound = .CarrierEDIFileNameBaseOutbound
                    Me.FileNameBaseInbound = .CarrierEDIFileNameBaseInbound
                    If .CarrierEDILastOutboundTransmission.HasValue Then
                        Me.LastOutboundTransmission = .CarrierEDILastOutboundTransmission.Value
                    Else
                        Me.LastOutboundTransmission = Date.Now.AddDays(-1)
                    End If
                    Me.EDIEmailNotificationOn = .CarrierEDIEmailNotificationOn
                    Me.EDIEmailAddress = .CarrierEDIEmailAddress
                    Me.CarrierEDISend997 = .CarrierEDIAcknowledgementRequested 'Added By LVV On 10/10/19 - We didn't want to keep sending error emails for if the 997 config does not exist, or if the AcknowledgementRequested flag is off for 210 and 214 (support case 201910021431)
                End With
                blnRet = True
            Else
                blnRet = False
            End If
        Catch sqlEx As FaultException(Of NData.SqlFaultInfo)
            LogError(Source & " Read Carrier EDI Config Failure", "SQL Exception: " & sqlEx.Detail.Message, Me.AdminEmail)
        Catch timeoutEx As TimeoutException
            LogError(Source & " Read Carrier EDI Config Failure", "Time Out Exception", Me.AdminEmail)
        Catch ex As Exception
            Throw
        End Try
        Return blnRet
    End Function

    Public Function SaveCarrierEDIConfig(ByVal EDIXaction As String) As Boolean
        Dim blnRet As Boolean = False
        Dim oCarrierEDI As New NData.NGLCarrierEDIData(Me.WCFPars)
        ''returns true if the action is allowed
        'blnRet = oCarrierEDI.GetCarrierEDIOption(Me.CarrierControl, EDIXaction)
        Try
            'if action allowed get the configuration data (returns an array of CarrierEDI object
            Dim oData As NData.DataTransferObjects.CarrierEDI = oCarrierEDI.GetCarrierEDIFiltered(Me.CarrierEDIControl)
            If Not oData Is Nothing Then
                oData.CarrierEDILastOutboundTransmission = Me.LastOutboundTransmission
                oCarrierEDI.UpdateRecordNoReturn(oData)
                blnRet = True
            Else
                blnRet = False
            End If
        Catch sqlEx As FaultException(Of NData.SqlFaultInfo)
            LogError(Source & " Read Carrier EDI Config Failure", "SQL Exception: " & sqlEx.Detail.Message, Me.AdminEmail)
        Catch timeoutEx As TimeoutException
            LogError(Source & " Read Carrier EDI Config Failure", "Time Out Exception", Me.AdminEmail)
        Catch ex As Exception
            Throw
        End Try
        Return blnRet
    End Function


#End Region

End Class
