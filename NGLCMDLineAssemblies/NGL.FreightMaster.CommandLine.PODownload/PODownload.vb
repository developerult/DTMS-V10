
Imports ngl.FreightMaster.FMLib
Imports ngl.FreightMaster.FMLib.General
Imports ngl.FreightMaster.FMLib.dbUtilities



Public Class PODownload
    'Change this flag to false when debugging is done.
    Public Shared mblnDebug As Boolean = False
    Public Shared mstrUserName As String = ""
    Public Shared mstrPassword As String = ""
    Public Shared mstrServer As String = ""
    Public Shared mstrLocalFolder As String = ""
    Public Shared mstrRemoteFolder As String = ""
    Public Shared mstrLocalBackupFolder As String = ""
    Public Shared mstrRemoteBackupFolder As String = ""
    Public Shared mstrPOFilter As String = "PO*.*"
    Public Shared mstrLaneFilter As String = "Lane*.*"
    Public Shared mstrCompFilter As String = "Comp*.*"
    Public Shared mstrCompContFilter As String = "CompanyContact*.*"
    Public Shared mstrCarrContFilter As String = "CarrierContact*.*"
    Public Shared mstrCarrFilter As String = "Carr*.*"
    Public Shared mstrSchedFilter As String = "Sched*.*"
    Public Shared mstrPayFilter As String = "Pay*.*"
    Public Shared mstrPOHeaderFilter As String = "POHeader*.*"
    Public Shared mstrPODetailFilter As String = "PODetail*.*"
    Public Shared mstrExternalProcessingFile As String = ""
    Public Shared mstrInternalProcessingFile As String = ""
    Public Shared mstrResultsFile As String = ""
    Public Shared mstrINIKey As String = "NGL"
    Public Shared mstrTransferType As String = "d"
    Public Shared mintAutoRetry As Integer = 3
    Public Shared mstrDatabase As String = ""
    Public Shared mstrDBServer As String = ""
    Public Shared mstrAdminEmail As String = ""
    Public Shared mstrGroupEmail As String = ""
    Public Shared mstrFromEmail As String = "info@maxximu.com"
    Public Shared mstrBatchFormat As String = ""
    Public Shared mstrSMTPServer As String = "smtp.gmail.com"
    Public Shared mblnGenerateHASH As Boolean = True
    Public Shared mstrHashFileName As String = "HashPOTotals"
    Public Shared mintKeepLogDays As Integer = 30
    Public Shared mblnSaveOldLog As Boolean = True
    Public Shared mstrWCFURL As String = ""
    Public Shared mstrWCFTCPURL As String = ""
    Public Shared mstrWCFAuthCode As String = "NGLSystem"

    Private Shared objF As New clsStandardFunctions
    Protected Shared objFTP As New FMLib.FTP
    Protected Shared EvtLog As New System.Diagnostics.EventLog

    Public Shared ReadOnly Property DBInfo() As String
        Get
            Return "Server: " & mstrDBServer & " | " & "Database: " & mstrDatabase

        End Get
    End Property



    Public Shared Function readParameters() As Integer
        'Note:  Return values
        '1 Success use parameters provided
        '0 Only show help message

        Dim strParameters() As String = System.Environment.GetCommandLineArgs
        Dim strVal As String
        Dim strHelpMsg As String
        Try
            strHelpMsg = vbCrLf _
                & "Usage:" & vbCrLf _
                & " FMFTPManager.exe [/?] " _
                & " [-u:username] [-p:password] [-k:inikey] "
            strHelpMsg &= vbCrLf & vbCrLf _
                & "Options:" & vbCrLf _
                & "    /?" & vbTab & vbTab & vbTab & "Show this help screen." & vbCrLf _
                & "    -u:username" & vbTab & vbTab & "FTP User Name." & vbCrLf _
                & "    -p:password" & vbTab & vbTab & "FTP Password." & vbCrLf _
                & "    -k:inikey" & vbTab & vbTab & "INI File Key (default = NGL)." _
                & "    -d" & vbTab & vbTab & "Debug Flag." _
                & vbCrLf & vbCrLf & "Note:  Spaces are required between parameters but not allowed inside of them."
            'If strParameters.Length < 2 Then
            '    gstrParameters = strParameters
            '    Return True
            'End If
            For Each strVal In strParameters
                Dim strSwitch As String = Left(strVal, 2)
                Select Case strSwitch
                    Case "/?"
                        Console.WriteLine(strHelpMsg)
                        Console.WriteLine("Press Enter To Continue")
                        Console.ReadLine()
                        Return 0
                    Case "-u"
                        mstrUserName = strVal.Substring(3)
                    Case "-p"
                        mstrPassword = strVal.Substring(3)
                    Case "-k"
                        mstrINIKey = strVal.Substring(3)
                    Case "-d"
                        mblnDebug = True
                End Select

            Next
            ' Read the INI File
            If mblnDebug Then
                Console.WriteLine("Ini File = " & appPath() & "\FreightMaster.ini")
            End If
            Dim objIniFile As New IniFile(appPath() & "\FreightMaster.ini")
            mstrLocalFolder = objIniFile.GetString(mstrINIKey, "Download Local Folder", "(none)")
            mstrRemoteFolder = objIniFile.GetString(mstrINIKey, "Download Remote Folder", "(none)")
            mstrLocalBackupFolder = objIniFile.GetString(mstrINIKey, "Download Local Backup Folder", "(none)")
            mstrRemoteBackupFolder = objIniFile.GetString(mstrINIKey, "Download Remote Backup Folder", "(none)")

            mstrInternalProcessingFile = objIniFile.GetString(mstrINIKey, "Internal Processing File", "(none)")
            mstrExternalProcessingFile = objIniFile.GetString(mstrINIKey, "External Processing File", "(none)")
            mstrResultsFile = objIniFile.GetString(mstrINIKey, "Download Results File", "(none)")
            mintAutoRetry = objIniFile.GetInteger(mstrINIKey, "Auto Retry", 3)
            mstrPOFilter = objIniFile.GetString(mstrINIKey, "PO Filter", mstrPOFilter)
            mstrLaneFilter = objIniFile.GetString(mstrINIKey, "Lane Filter", mstrLaneFilter)
            mstrCompFilter = objIniFile.GetString(mstrINIKey, "Company Filter", mstrCompFilter)
            mstrCompContFilter = objIniFile.GetString(mstrINIKey, "Company Contact Filter", mstrCompContFilter)
            mstrCarrFilter = objIniFile.GetString(mstrINIKey, "Carrier Filter", mstrCarrFilter)
            mstrCarrContFilter = objIniFile.GetString(mstrINIKey, "Carrier Contact Filter", mstrCarrContFilter)
            mstrPayFilter = objIniFile.GetString(mstrINIKey, "Payables Filter", mstrPayFilter)
            mstrSchedFilter = objIniFile.GetString(mstrINIKey, "Schedule Filter", mstrSchedFilter)
            mstrServer = objIniFile.GetString(mstrINIKey, "FTP Server IP", "")
            mstrDatabase = objIniFile.GetString(mstrINIKey, "Database", "")
            mstrDBServer = objIniFile.GetString(mstrINIKey, "DBServer", "")
            mstrAdminEmail = objIniFile.GetString(mstrINIKey, "AdminEmail", "info@maxximu.com")
            mstrGroupEmail = objIniFile.GetString(mstrINIKey, "GroupEmail", "info@maxximu.com")
            mstrFromEmail = objIniFile.GetString(mstrINIKey, "FromEmail", mstrFromEmail)
            mstrBatchFormat = objIniFile.GetString(mstrINIKey, "BatchFormat", "yyMMddhhmm")
            mstrPOHeaderFilter = objIniFile.GetString(mstrINIKey, "POHeaderFilter", "POHeader*.*")
            mstrPODetailFilter = objIniFile.GetString(mstrINIKey, "PODetailFilter", "PODetail*.*")
            mstrSMTPServer = objIniFile.GetString(mstrINIKey, "SMTPServer", "smtp.gmail.com")
            mblnGenerateHASH = objIniFile.GetString(mstrINIKey, "GenerateHASH", mblnGenerateHASH.ToString).ToLower
            mstrHashFileName = objIniFile.GetString(mstrINIKey, "downloadHashFile", "HashPOTotals")
            mblnSaveOldLog = objIniFile.GetString(mstrINIKey, "SaveOldLog", mblnSaveOldLog.ToString).ToLower
            mintKeepLogDays = objIniFile.GetInteger(mstrINIKey, "KeepLogDays", mintKeepLogDays)
            mstrWCFURL = objIniFile.GetString(mstrINIKey, "WCFURL", "")
            mstrWCFTCPURL = objIniFile.GetString(mstrINIKey, "WCFTCPURL", "")
            mstrWCFAuthCode = objIniFile.GetString(mstrINIKey, "WCFAuthCode", "")

            If Len(Trim(mstrServer)) > 0 And (mstrUserName.Length < 1 Or mstrPassword.Length < 1) Then
                If mblnDebug Then
                    Console.WriteLine("Invalid login information. Please provide username, password and FTP Server IP")
                    Console.WriteLine("Press Enter To Continue")
                    Console.ReadLine()
                End If
                EvtLog.WriteEntry("Invalid login information", EventLogEntryType.Error)
                LogError("PODownload Error", "PODownload.readParameters: Invalid login information", mstrAdminEmail)
                Return 0
            Else
                Return 1
            End If
        Catch ex As Exception
            Throw New ApplicationException("FTPUpload.readParameters Failure! ", ex)
        End Try


    End Function

    Public Shared Sub Main()
        Dim sSource As String = "FreightMaster PO Download "
        Dim sErrMsg As String = "Unknown Error!"

        EvtLog.Log = "Application"
        EvtLog.Source = sSource
        Try
            If mblnDebug Then
                EvtLog.WriteEntry("Application Start", EventLogEntryType.Information)
            End If

        Catch ex As Exception

        End Try

        Try
            Dim objLaneClass As clsLane = Nothing
            Dim objBookClass As clsBook = Nothing
            Dim strCreateUser As String = "System Importer"
            Dim strCreatedDate As String = Now.ToString
            Dim blnNewLane As Boolean = False
            Dim blnNewPO As Boolean = False
            Dim objHash As New clsHashTotals
            Dim strBatchNumber As String = ""
            Dim strImportFileName As String = ""
            Dim strImportItemFileName As String = ""
            Dim strImportName As String = ""
            Dim strImportItemName As String = ""
            Dim intRetValue As Integer = 0
            'Read Parameters and Configuration Settings
            If readParameters() = 0 Then
                Exit Sub
            End If
            'use the database name as part of the source
            sSource &= " - " & DBInfo
            EvtLog.Source = sSource
            'set global class library settings in FMLib.General Shared Library
            gblnDebug = mblnDebug
            gstrDatabase = mstrDatabase
            gstrServer = mstrDBServer
            gblnSilent = True
            displayParameterData()
            'set up the global FTP properties
            objFTP = New FTP(mstrUserName _
                , mstrPassword _
                , mstrServer _
                , mstrLocalFolder _
                , mstrRemoteFolder _
                , mstrLocalBackupFolder _
                , mstrRemoteBackupFolder _
                , mstrCompFilter _
                , mstrExternalProcessingFile _
                , mstrInternalProcessingFile _
                , mstrResultsFile _
                , mstrResultsFile _
                , mintAutoRetry)
            objFTP.KeepLogDays = mintKeepLogDays
            objFTP.SaveOldLog = mblnSaveOldLog
            Dim strErrMsg As String = ""
            objFTP.Debug = mblnDebug
            '********** BEGIN FTP DOWNLOAD IF A SERVER IS PROVIDED **********
            If Len(Trim(mstrServer)) > 0 Then 'We only download data if an FTP Server is provided else we just use the local network files
                If Not objFTP.startBatch() Then
                    strErrMsg = "PO Dowload Error! Unable to start FTP batch operation: " & objFTP.LastError
                    If mblnDebug Then Console.WriteLine(strErrMsg)
                    EvtLog.WriteEntry(strErrMsg, EventLogEntryType.Error)
                    LogError("PODownload Error", "PODownload.Main " & vbCrLf & strErrMsg, mstrAdminEmail)
                    objFTP.endBatch(objFTP.Results)
                    Exit Sub
                End If
            End If
            Dim blnFilesDownloaded As Boolean = False
            Try
                '******** COMPANY DOWNLOAD *********
                If Not download(strBatchNumber, intRetValue, mstrCompFilter, "Company") Then Exit Sub
                '******* CARRIER DOWNLOAD ***********
                If Not download(strBatchNumber, intRetValue, mstrCarrFilter, "Carrier") Then Exit Sub
                '******* LANE DOWNLOAD ***********
                If Not download(strBatchNumber, intRetValue, mstrLaneFilter, "Lane") Then Exit Sub
                '******* SCHEDULE DOWNLOAD ***********
                If Not download(strBatchNumber, intRetValue, mstrSchedFilter, "Schedule") Then Exit Sub
                '******* PAYABLES DOWNLOAD ***********
                If Not download(strBatchNumber, intRetValue, mstrPayFilter, "Payables") Then Exit Sub
                '******* ORDERS DOWNLOAD ***********
                If Not download(strBatchNumber, intRetValue, mstrPOFilter, "PO Data") Then Exit Sub
                blnFilesDownloaded = True
            Catch ex As ApplicationException
                Throw

            Catch ex As Exception
                Throw New ApplicationException("PODownload.downloadfiles Failure!", ex)
            Finally
                If Len(Trim(mstrServer)) > 0 Then 'We only download data if an FTP Server is provided else we just use the local network files
                    If Not objFTP.endBatch(intRetValue) Then
                        strErrMsg = "PO Dowload Error! Unable to end FTP batch operation: " & objFTP.LastError
                        If mblnDebug Then Console.WriteLine(strErrMsg)
                        EvtLog.WriteEntry(strErrMsg, EventLogEntryType.Error)
                        LogError("PODownload Error", "PODownload.Main " & vbCrLf & strErrMsg, mstrAdminEmail)
                    End If
                End If

            End Try
            If Not blnFilesDownloaded Then Exit Sub

            '********** END FTP DOWNLOAD **********
            '
            '********** Begin MERGE DATA **********
            '
            '***********  COMPANY MERGE  ***********
            'Merge All the item detail files if needed
            If Not MergeData("CompanyContact.txt", mstrCompContFilter, "CompanyContact") Then Exit Sub
            'Merge All the header data  in case there are more than one file
            If Not MergeData("Company.txt", mstrCompFilter, "Company") Then Exit Sub
            '***********  CARRIER MERGE  ***********
            'Merge All the item detail files if needed
            If Not MergeData("CarrierContact.txt", mstrCarrContFilter, "CarrierContact") Then Exit Sub
            'Merge All the header data  in case there are more than one file
            If Not MergeData("Carrier.txt", mstrCarrFilter, "Carrier") Then Exit Sub
            '***********  LANE MERGE  ***********
            'Merge All the header data in case there are more than one file
            If Not MergeData("Lane.txt", mstrLaneFilter, "Lane") Then Exit Sub
            '***********  SCHEDULE MERGE  ***********
            'Merge All the header data in case there are more than one file
            If Not MergeData("Schedule.txt", mstrSchedFilter, "Schedule") Then Exit Sub
            '***********  PAYABLES MERGE  ***********
            'Merge All the header data in case there are more than one file
            If Not MergeData("payables.txt", mstrPayFilter, "Payables") Then Exit Sub
            '***********  ORDERS MERGE  ***********
            'Merge All the item detail files if needed
            If Not MergeData("PODetail.txt", mstrPODetailFilter, "PO Detail") Then Exit Sub
            'Merge All the header data  in case there are more than one file
            If Not MergeData("POHeader.txt", mstrPOHeaderFilter, "PO Header") Then Exit Sub
            '********** END MERGE FILES ************
            '
            '********** Begin Data Import **********
            '
            '***********  COMPANY IMPORT ***********
            Dim oComp As New clsCompany
            With oComp
                .Debug = mblnDebug
                .LogFile = objF.buildPath(mstrLocalFolder, mstrResultsFile)
                .Silent = True
                .CreatedDate = strCreatedDate
                .CreateUser = strCreateUser
                .HeaderName = "Company"
                .ItemName = "CompanyContact"
                .ImportTypeKey = gcImportComp 'for company Error Tracking
                .GroupEmail = mstrGroupEmail
                .AdminEmail = mstrAdminEmail
                .SMTPServer = mstrSMTPServer
                .Retry = mintAutoRetry
                .FromEmail = mstrFromEmail
                .KeepLogDays = mintKeepLogDays
                .SaveOldLog = mblnSaveOldLog
                .Database = mstrDatabase
                .DBServer = mstrDBServer
                .WCFAuthCode = mstrWCFAuthCode
                .WCFURL = mstrWCFURL
                .WCFTCPURL = mstrWCFTCPURL
                .Source = "PODownload Company"
                .openLog()
                .FileImport("Company.txt", "CompanyContact.txt", mstrLocalFolder)
                .closeLog(.Results)
                'Get the totals
                objHash.TotalCompanies = .TotalRecords
            End With
            'Backup and Delete the Company File
            MoveFile(objF.buildPath(mstrLocalFolder, "Company.txt"), objF.buildPath(mstrLocalBackupFolder, objF.timeStampFileName("Company.txt")))
            'Backup and Delete the Company File
            MoveFile(objF.buildPath(mstrLocalFolder, "CompanyContact.txt"), objF.buildPath(mstrLocalBackupFolder, objF.timeStampFileName("CompanyContact.txt")))
            '***********  CARRIER IMPORT ***********
            Dim oCarr As New clsCarrier
            With oCarr
                .Debug = mblnDebug
                .LogFile = objF.buildPath(mstrLocalFolder, mstrResultsFile)
                .Silent = True
                .CreatedDate = strCreatedDate
                .CreateUser = strCreateUser
                .HeaderName = "Carrier"
                .ItemName = "CarrierContact"
                .ImportTypeKey = gcImportCarrier 'for company Error Tracking
                .GroupEmail = mstrGroupEmail
                .AdminEmail = mstrAdminEmail
                .SMTPServer = mstrSMTPServer
                .Retry = mintAutoRetry
                .FromEmail = mstrFromEmail
                .KeepLogDays = mintKeepLogDays
                .SaveOldLog = mblnSaveOldLog
                .Database = mstrDatabase
                .DBServer = mstrDBServer
                .WCFAuthCode = mstrWCFAuthCode
                .WCFURL = mstrWCFURL
                .WCFTCPURL = mstrWCFTCPURL
                .Source = "PODownload Carrier"
                .openLog()
                .FileImport("Carrier.txt", "CarrierContact.txt", mstrLocalFolder)
                .closeLog(.Results)
                'Get the totals
                objHash.TotalCarriers = .TotalRecords
            End With
            'Backup and Delete the Header File
            MoveFile(objF.buildPath(mstrLocalFolder, "Carrier.txt"), objF.buildPath(mstrLocalBackupFolder, objF.timeStampFileName("Carrier.txt")))
            'Backup and Delete the Item File
            MoveFile(objF.buildPath(mstrLocalFolder, "CarrierContact.txt"), objF.buildPath(mstrLocalBackupFolder, objF.timeStampFileName("CarrierContact.txt")))
            '***********  LANE IMPORT ***********
            objLaneClass = New clsLane
            With objLaneClass
                .Debug = mblnDebug
                .LogFile = objF.buildPath(mstrLocalFolder, mstrResultsFile)
                .Silent = True
                .CreatedDate = strCreatedDate
                .CreateUser = strCreateUser
                .HeaderName = "Lane"
                .ItemName = "None"
                .ImportTypeKey = gcImportLane 'for Lane Error Tracking
                .GroupEmail = mstrGroupEmail
                .AdminEmail = mstrAdminEmail
                .SMTPServer = mstrSMTPServer
                .Retry = mintAutoRetry
                .FromEmail = mstrFromEmail
                .KeepLogDays = mintKeepLogDays
                .SaveOldLog = mblnSaveOldLog
                .Database = mstrDatabase
                .DBServer = mstrDBServer
                .WCFAuthCode = mstrWCFAuthCode
                .WCFURL = mstrWCFURL
                .WCFTCPURL = mstrWCFTCPURL
                .Source = "PODownload Lane"
                .openLog()
                .FileImport("Lane.txt", "", mstrLocalFolder)
                .closeLog(.Results)
                'Get the totals
                objHash.TotalLanes = .TotalRecords
            End With
            'Backup and Delete the Lane File
            MoveFile(objF.buildPath(mstrLocalFolder, "Lane.txt"), objF.buildPath(mstrLocalBackupFolder, objF.timeStampFileName("Lane.txt")))
            '***********  SCHEDULE IMPORT ***********
            Dim oSched As New clsSchedule
            With oSched
                .Debug = mblnDebug
                .LogFile = objF.buildPath(mstrLocalFolder, mstrResultsFile)
                .Silent = True
                .CreatedDate = strCreatedDate
                .CreateUser = strCreateUser
                .HeaderName = "Schedule"
                .ItemName = "None"
                .ImportTypeKey = gcImportSchedule 'for  Error Tracking
                .GroupEmail = mstrGroupEmail
                .AdminEmail = mstrAdminEmail
                .SMTPServer = mstrSMTPServer
                .Retry = mintAutoRetry
                .FromEmail = mstrFromEmail
                .KeepLogDays = mintKeepLogDays
                .SaveOldLog = mblnSaveOldLog
                .Database = mstrDatabase
                .DBServer = mstrDBServer
                .WCFAuthCode = mstrWCFAuthCode
                .WCFURL = mstrWCFURL
                .WCFTCPURL = mstrWCFTCPURL
                .Source = "PODownload Schedule"
                .openLog()
                .FileImport("Schedule.txt", "", mstrLocalFolder)
                .closeLog(.Results)
                'Get the totals
                objHash.TotalSchedules = .TotalRecords
            End With
            'Backup and Delete the File
            MoveFile(objF.buildPath(mstrLocalFolder, "Schedule.txt"), objF.buildPath(mstrLocalBackupFolder, objF.timeStampFileName("Schedule.txt")))
            '***********  PAYABLES IMPORT ***********
            Dim oPay As New clsPayables
            With oPay
                .Debug = mblnDebug
                .LogFile = objF.buildPath(mstrLocalFolder, mstrResultsFile)
                .Silent = True
                .CreatedDate = strCreatedDate
                .CreateUser = strCreateUser
                .HeaderName = "Payables"
                .ItemName = "None"
                .ImportTypeKey = gcImportPayables 'for  Error Tracking
                .GroupEmail = mstrGroupEmail
                .AdminEmail = mstrAdminEmail
                .SMTPServer = mstrSMTPServer
                .Retry = mintAutoRetry
                .FromEmail = mstrFromEmail
                .KeepLogDays = mintKeepLogDays
                .SaveOldLog = mblnSaveOldLog
                .Database = mstrDatabase
                .DBServer = mstrDBServer
                .WCFAuthCode = mstrWCFAuthCode
                .WCFURL = mstrWCFURL
                .WCFTCPURL = mstrWCFTCPURL
                .Source = "PODownload Payables"
                .openLog()
                .FileImport("payables.txt", "", mstrLocalFolder)
                .closeLog(.Results)
                'Get the totals
                objHash.TotalPayables = .TotalRecords
            End With
            'Backup and Delete the File
            MoveFile(objF.buildPath(mstrLocalFolder, "payables.txt"), objF.buildPath(mstrLocalBackupFolder, objF.timeStampFileName("payables.txt")))
            '***********  ORDERS IMPORT  ***********                                                 
            objBookClass = New clsBook
            With objBookClass
                .Debug = mblnDebug
                .LogFile = objF.buildPath(mstrLocalFolder, mstrResultsFile)
                .Silent = True
                .CreatedDate = strCreatedDate
                .CreateUser = strCreateUser
                .HeaderName = "POHeader"
                .ItemName = "PODetail"
                .ImportTypeKey = gcimportBook
                .GroupEmail = mstrGroupEmail
                .AdminEmail = mstrAdminEmail
                .SMTPServer = mstrSMTPServer
                .Retry = mintAutoRetry
                .FromEmail = mstrFromEmail
                .KeepLogDays = mintKeepLogDays
                .SaveOldLog = mblnSaveOldLog
                .Database = mstrDatabase
                .DBServer = mstrDBServer
                .WCFAuthCode = mstrWCFAuthCode
                .WCFURL = mstrWCFURL
                .WCFTCPURL = mstrWCFTCPURL
                .Source = "PODownload Booking"
                .openLog()
                .FileImport("POHeader.txt", "PODetail.txt", mstrLocalFolder)
                .closeLog(.Results)
                objHash.TotalOrders = .TotalOrders
                objHash.TotalDetails = .TotalDetails
                objHash.HashTotalOrders = .HashTotalOrders
                objHash.HashTotalDetails = .HashTotalDetails
                objHash.TotalQty = .TotalQty
                objHash.TotalWeight = .TotalWeight
            End With
            'Backup and Delete the POHeader File
            MoveFile(objF.buildPath(mstrLocalFolder, "POHeader.txt"), objF.buildPath(mstrLocalBackupFolder, objF.timeStampFileName("POHeader.txt")))
            'Backup and Delete the POHeader File
            MoveFile(objF.buildPath(mstrLocalFolder, "PODetail.txt"), objF.buildPath(mstrLocalBackupFolder, objF.timeStampFileName("PODetail.txt")))
            '********** END DATA IMPORT ************
            '
            '********** BAGIN GENERATE HASH **********
            If mblnGenerateHASH And strBatchNumber.Length > 0 Then
                objHash.Write(objF.buildPath(mstrLocalFolder, mstrHashFileName & strBatchNumber & ".txt"))
                If Len(Trim(mstrServer)) > 0 Then 'We only upload data if an FTP Server is provided else we just use the local network files
                    objFTP.FileFilter = mstrHashFileName & "*.txt"
                    If Not objFTP.uploadFiles(False, False, False, True) Then
                        strErrMsg = "FTP Upload Error! Unable to upload " & mstrHashFileName & " file to remote FTP site: " & objFTP.LastError
                        If mblnDebug Then Console.WriteLine(strErrMsg)
                        LogError("PODownload Error", "PODownload.Main " & vbCrLf & strErrMsg, mstrAdminEmail)
                    End If
                End If
            End If


        Catch ex As ApplicationException
            If mblnDebug Then
                Console.WriteLine("Error! " & ex.ToString)
            End If
            sErrMsg = formatErrMsg(ex, sSource)
            LogError("PODownload Failure", "PODownload.Main " & vbCrLf & sErrMsg, mstrAdminEmail)
            EvtLog.WriteEntry(sErrMsg, EventLogEntryType.Error)

        Catch ex As Exception
            If mblnDebug Then
                Console.WriteLine("Error! " & ex.ToString)
            End If
            sErrMsg = formatErrMsg(ex, sSource)
            LogError("PODownload Failure", "PODownload.Main " & vbCrLf & sErrMsg, mstrAdminEmail)
            EvtLog.WriteEntry(sErrMsg, EventLogEntryType.Error)
        Finally
            'delete local processing files
            Try
                If System.IO.File.Exists(objF.buildPath(mstrLocalFolder, mstrInternalProcessingFile)) Then
                    System.IO.File.Delete(objF.buildPath(mstrLocalFolder, mstrInternalProcessingFile))
                End If
            Catch ex As Exception
            End Try

            If mblnDebug Then
                Console.WriteLine("Press Enter To Continue")
                Console.ReadLine()
            End If
        End Try
    End Sub

    Public Shared Function appPath() As String
        Return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location)
    End Function

    Public Shared Function GetBatchNumber(ByVal strFileFilter As String) As String
        Dim strFile As String = ""
        Dim strBatchNumber As String = ""

        Try
            strFile = Dir(mstrLocalFolder & "\" & strFileFilter)
            If strFile <> "" Then
                Console.WriteLine("File Filter = " & strFileFilter)
                Console.WriteLine("File Name = " & strFile)
                Dim strParts() As String = strFile.Split(".")
                Console.WriteLine("Name Only = " & strParts(0))
                Dim intLenName As Integer = Len(strParts(0))
                Dim intLenBatchNumber As Integer = Len(mstrBatchFormat)
                If intLenName > intLenBatchNumber Then
                    strBatchNumber = strParts(0).Substring(intLenName - intLenBatchNumber)
                End If
            End If

            Return strBatchNumber
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Shared Sub LogError(ByVal strSubject As String, ByVal logMessage As String, ByVal strMailTo As String)
        Try
            SendMail(mstrSMTPServer, strMailTo, mstrFromEmail, logMessage & vbCrLf & DBInfo, strSubject)

        Catch ex As Exception

        End Try

    End Sub

    Public Shared Function download(ByRef strBatchNumber As String, _
            ByRef intRetValue As Integer, _
            ByVal strItemFilter As String, _
            ByVal strImportName As String) As Boolean
        Dim Ret As Boolean = False
        Dim strErrMsg As String = ""
        Try
            If Len(Trim(mstrServer)) > 0 Then 'We only download data if an FTP Server is provided else we just use the local network files
                objFTP.FileFilter = strItemFilter
                If Not objFTP.downloadFiles(True) Then
                    strErrMsg = "PO Dowload Error! Unable to download " & strImportName & " data Files: " & objFTP.LastError
                    If mblnDebug Then Console.WriteLine(strErrMsg)
                    EvtLog.WriteEntry(strErrMsg, EventLogEntryType.Error)
                    LogError("PODownload Error", "PODownload.download" & vbCrLf & strErrMsg, mstrAdminEmail)
                    Return False
                End If
                If mblnDebug Then Console.WriteLine("Number of " & strImportName & " Files Transfered = " & objFTP.Results.ToString)
            End If
            'Get the batch number from the first file if needed
            If strBatchNumber.Trim.Length = 0 Then
                strBatchNumber = GetBatchNumber(strItemFilter)
            End If
            'Save the return value
            intRetValue += objFTP.Results
            Ret = True
        Catch ex As ApplicationException
            Throw
        Catch ex As Exception
            Throw New ApplicationException("PODownload.download Failure!", ex)
        End Try
        Return Ret

    End Function


    Public Shared Function MergeData(ByVal strFileName As String, _
            ByVal strFilter As String, _
            ByVal strName As String) As Boolean
        Dim Ret As Boolean = False
        Dim strErrMsg As String = ""
        Try
            'Merge All the item detail files if needed
            If strFilter.Trim.Length > 0 Then
                objFTP.FileFilter = strFilter
                If Not objFTP.appendToFile(strFileName) Then
                    strErrMsg = "Merge Data Error! Unable to append " & strName & " data to " & strFileName & " file: " & objFTP.LastError
                    If mblnDebug Then Console.WriteLine(strErrMsg)
                    LogError("PODownload Error", "PODownload.MergeData" & vbCrLf & strErrMsg, mstrAdminEmail)
                    Return False
                End If
                If mblnDebug Then
                    Console.WriteLine("Number of " & strName & " files appended to " & strFileName & " = " & objFTP.Results.ToString & ".")
                End If
            End If
            Ret = True
        Catch ex As ApplicationException
            Throw
        Catch ex As Exception
            Throw New ApplicationException("PODownload.MergeData Failure!", ex)
        End Try
        Return Ret
    End Function

    Protected Shared Sub displayParameterData()
        If mblnDebug Then
            Console.WriteLine("User Name: " & mstrUserName)
            Console.WriteLine("Password: " & mstrPassword)
            Console.WriteLine("FTP Server: " & mstrServer)
            Console.WriteLine("Local Folder: " & mstrLocalFolder)
            Console.WriteLine("Remote Folder: " & mstrRemoteFolder)
            Console.WriteLine("Local Backup Folder: " & mstrLocalBackupFolder)
            Console.WriteLine("Remote Backup Folder: " & mstrRemoteBackupFolder)
            Console.WriteLine("Internal Processing File: " & mstrInternalProcessingFile)
            Console.WriteLine("External Processing File: " & mstrExternalProcessingFile)
            Console.WriteLine("Results File: " & mstrResultsFile)
            Console.WriteLine("INI Key: " & mstrINIKey)
            Console.WriteLine("Transfer Type: " & mstrTransferType)
            Console.WriteLine("Auto Retry: " & mintAutoRetry)
            Console.WriteLine("Database: " & mstrDatabase)
            Console.WriteLine("DB Server: " & mstrDBServer)
            Console.WriteLine("PO Filter: " & mstrPOFilter)
            Console.WriteLine("Lane Filter: " & mstrLaneFilter)
            Console.WriteLine("Company Filter: " & mstrCompFilter)
            Console.WriteLine("Company Contact Filter: " & mstrCompContFilter)
            Console.WriteLine("Carrier Filter: " & mstrCarrFilter)
            Console.WriteLine("Carrier Contact Filter: " & mstrCarrContFilter)
            Console.WriteLine("Schedule Filter: " & mstrSchedFilter)
            Console.WriteLine("Payables Filter: " & mstrPayFilter)
            Console.WriteLine("POHeaderFilter: " & mstrPOHeaderFilter)
            Console.WriteLine("PODetailFilter: " & mstrPODetailFilter)
            Console.WriteLine("Admin Email: " & mstrAdminEmail)
            Console.WriteLine("Group Email: " & mstrGroupEmail)
            Console.WriteLine("Batch Format: " & mstrBatchFormat)
            Console.WriteLine("SMTP Server: " & mstrSMTPServer)
            Console.WriteLine("Generate Hash: " & mblnGenerateHASH.ToString)
        End If

    End Sub

End Class
