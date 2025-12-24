Imports NGL.FMWCFProxy
Imports NGL.FMWCFProxy.NGLIntegrationData
Imports NGL.IntegrationServices.NGLISBaseClass
Imports System.Text.RegularExpressions
Imports System.Reflection

Public MustInherit Class NGLISBaseClass

#Region " Constructors "
    Public Sub New()
        MyBase.New()
    End Sub
    Public Sub New(ByVal wwobject As Object)
        MyBase.New()

    End Sub
#End Region

#Region "Enums and Constants"

    Public Enum ProcessDataReturnValues
        nglDataIntegrationComplete
        nglDataConnectionFailure
        nglDataValidationFailure
        nglDataIntegrationFailure
        nglDataIntegrationHadErrors
    End Enum

    Public Enum IntegrationTypes As Integer
        Carrier = 0
        Lane
        Company
        Payables
        Schedule
        LegacyFreightBillImport
        FreightBillImport
        Book
        PickList
        APExport
        FreightBillExport
        OpenPayables
        EDI214
        EDI204
        EDI990
        NONE
    End Enum

    Public Enum IntegrationMethods
        ConvertStandardCSV
        WebStandard
    End Enum
    'Error Constants
    Public Const gclngErrorNumber1 As Short = 20001
    Public Const gcstrErrorDesc1 As String = "cannot locate database server."
    Public Const gclngErrorNumber2 As Short = 20002
    Public Const gcstrErrorDesc2 As String = "Null value not allowed."
    Public Const gclngErrorNumber3 As Short = 20003
    Public Const gcstrErrorDesc3 As String = "Invalid data type."
    Public Const gclngErrorNumber4 As Short = 20004
    Public Const gcstrErrorDesc4 As String = "Text data is too long."
#End Region

#Region " Properties"

    Protected mintImportTypeKey As Integer = 0
    Protected mstrKey As String
    Protected mobjLastException As Exception
    Protected mstrAppName As String
    Protected mstrAppKey As String
    Protected mioLog As System.IO.StreamWriter
    Protected mobjLog As clsLog
    Public Shared mstrLocalBackupFolder As String = ""
    Public Shared mstrRemoteBackupFolder As String = ""
    Protected WithEvents fmFileImportErrorLogData As New FMFileImportErrorLog(New NGLSynchronizationContext(System.Threading.SynchronizationContext.Current))

    Private _WCFParameters As NGL.FMWCFProxy.NGLIntegrationData.WCFParameters
    Public Property WCFParametersProp() As NGL.FMWCFProxy.NGLIntegrationData.WCFParameters
        Get
            Return _WCFParameters
        End Get
        Set(ByVal value As NGL.FMWCFProxy.NGLIntegrationData.WCFParameters)
            _WCFParameters = value
        End Set
    End Property

    Private _WCFURL As String
    Public Property WCFURL() As String
        Get
            Return _WCFURL
        End Get
        Set(ByVal value As String)
            _WCFURL = value
        End Set
    End Property

    Public Property ImportTypeKey() As Integer
        Get
            Return mintImportTypeKey
        End Get
        Set(ByVal Value As Integer)
            mintImportTypeKey = Value
        End Set
    End Property

    Private _AddDeliveryDays As Integer = 0
    Public Property AddDeliveryDays() As Integer
        Get
            Return _AddDeliveryDays
        End Get
        Set(ByVal value As Integer)
            _AddDeliveryDays = value
        End Set
    End Property


    Protected mblnSaveOldLog As Boolean = True
    Public Property SaveOldLog() As Boolean
        Get
            Return mblnSaveOldLog
        End Get
        Set(ByVal value As Boolean)
            mblnSaveOldLog = value
        End Set
    End Property

    Private _SilentLogging As Boolean
    Public Property SilentLogging() As Boolean
        Get
            Return _SilentLogging
        End Get
        Set(ByVal value As Boolean)
            _SilentLogging = value
        End Set
    End Property


    Private _ConfigDataRows As DataRow()
    Friend Property ConfigDataRows As DataRow()
        Get
            Return _ConfigDataRows
        End Get
        Set(value As DataRow())
            _ConfigDataRows = value
        End Set
    End Property

    Private _ConnectionString As String = "" ' My.settings is not always accessible via class libraries My.Settings.NGLMAS 'the default value uses the configuration setting NGLMAS
    Public Property ConnectionString() As String
        Get
            Return _ConnectionString
        End Get
        Set(ByVal value As String)
            _ConnectionString = value
        End Set
    End Property

    Private _IsAuthorized As Boolean = True
    Public Property IsAuthorized() As Boolean
        Get
            Return _IsAuthorized
        End Get
        Set(ByVal value As Boolean)
            _IsAuthorized = value
        End Set
    End Property

    Private _Parameters As WCFParameters
    Public ReadOnly Property Parameters() As WCFParameters
        Get
            Return _Parameters
        End Get
    End Property

    Public ReadOnly Property DBInfo() As String
        Get
            Return "Server: " & Me.DBServer & vbCrLf & "Database: " & Me.Database
        End Get
    End Property

    Public ReadOnly Property DBServer() As String
        Get
            Return Me.Parameters.DBServer
        End Get
    End Property

    Public ReadOnly Property Database() As String
        Get
            Return Me.Parameters.Database
        End Get
    End Property

    Dim mstrLogFile As String
    Public Property LogFile() As String
        Get
            Return mstrLogFile
        End Get
        Set(ByVal Value As String)
            mstrLogFile = Value
        End Set
    End Property

    Dim mblnDebug As Boolean
    Public Property Debug() As Boolean
        Get
            Return mblnDebug
        End Get
        Set(ByVal Value As Boolean)
            mblnDebug = Value
        End Set
    End Property

    Dim mstrCreatedDate As Date = Now
    Public Property CreatedDate() As Date
        Get
            Return mstrCreatedDate
        End Get
        Set(ByVal Value As Date)
            mstrCreatedDate = Value
        End Set
    End Property

    Dim mstrCreateUser As String = "system download"
    Public Property CreateUser() As String
        Get
            Return mstrCreateUser
        End Get
        Set(ByVal Value As String)
            mstrCreateUser = Value
        End Set
    End Property

    Dim _mstrLocalFolder As String
    Public Property LocalFolder() As String
        Get
            Return _mstrLocalFolder
        End Get
        Set(ByVal Value As String)
            _mstrLocalFolder = Value
        End Set
    End Property

    Protected mstrHeaderName As String = ""
    Public Property HeaderFileName() As String
        Get
            Return mstrHeaderName
        End Get
        Set(ByVal Value As String)
            mstrHeaderName = Value
        End Set
    End Property

    Protected mstrItemName As String = ""
    Public Property DetailFileName() As String
        Get
            Return mstrItemName
        End Get
        Set(ByVal Value As String)
            mstrItemName = Value
        End Set
    End Property

    Protected mstrSource As String = ""
    Public Property Source() As String
        Get
            Return mstrSource
        End Get
        Set(ByVal Value As String)
            mstrSource = Value
        End Set
    End Property

    Private _fileFilter As String
    Public Property FileFilter() As String
        Get
            Return _fileFilter
        End Get
        Set(ByVal value As String)
            _fileFilter = value
        End Set
    End Property

    Private _strLastErr As String = ""
    Public Property LastError() As String
        Get
            Return _strLastErr

        End Get
        Protected Set(ByVal Value As String)
            _strLastErr = Value
        End Set
    End Property

    Private _FromEmail As String = "donotreply@nextgeneration.com"
    Public Property FromEmail() As String
        Get
            Return _FromEmail
        End Get
        Set(ByVal value As String)
            _FromEmail = value
        End Set
    End Property

    Private mstrGroupEmail As String = "support@nextgeneration.com"
    Public Property GroupEmail() As String
        Get
            GroupEmail = mstrGroupEmail
        End Get
        Set(ByVal Value As String)
            mstrGroupEmail = Value
        End Set
    End Property

    Private mstrAdminEmail As String = "support@nextgeneration.com"
    Public Property AdminEmail() As String
        Get
            AdminEmail = mstrAdminEmail
        End Get
        Set(ByVal Value As String)
            mstrAdminEmail = Value
        End Set
    End Property

    Private _KeepLogDays As Integer = 10
    Public Property KeepLogDays() As Integer
        Get
            Return _KeepLogDays
        End Get
        Set(ByVal Value As Integer)
            _KeepLogDays = Value
        End Set
    End Property

    Private mintTotalRecords As Integer = 0
    Public Property TotalRecords() As Integer
        Get
            TotalRecords = mintTotalRecords
        End Get
        Protected Set(ByVal Value As Integer)
            mintTotalRecords = Value
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

    Dim mintResults As Integer
    Public Property Results() As Integer
        Get
            Return mintResults
        End Get
        Set(ByVal Value As Integer)
            mintResults = Value
        End Set
    End Property

    Private _WebServiceURL As String = ""
    Public Property WebServiceURL() As String
        Get
            Return _WebServiceURL
        End Get
        Set(ByVal value As String)
            _WebServiceURL = value
        End Set
    End Property

    Private _LaneWebServiceURLExtension As String = ""
    Public Property LaneWebServiceURLExtension() As String
        Get
            Return _LaneWebServiceURLExtension
        End Get
        Set(ByVal value As String)
            _LaneWebServiceURLExtension = value
        End Set
    End Property

    Private _WsBookExtURL As String
    Public Property WSBookExtURL() As String
        Get
            Return _WsBookExtURL
        End Get
        Set(ByVal value As String)
            _WsBookExtURL = value
        End Set
    End Property

    Private _WebServiceAuthCode As String = ""
    Public Property WebServiceAuthCode() As String
        Get
            Return _WebServiceAuthCode
        End Get
        Set(ByVal value As String)
            _WebServiceAuthCode = value
        End Set
    End Property

    Private _FileName As String
    Public Property FileName() As String
        Get
            Return _FileName
        End Get
        Set(ByVal value As String)
            _FileName = value
        End Set
    End Property

    Private _Schemas As New SchemaSource
    Public Property Schemas() As SchemaSource
        Get
            Return _Schemas
        End Get
        Set(ByVal value As SchemaSource)
            _Schemas = value
        End Set
    End Property

    Private _FilePathDirectory As String
    Public Property FilePathDirectory() As String
        Get
            Return _FilePathDirectory
        End Get
        Set(ByVal value As String)
            _FilePathDirectory = value
        End Set
    End Property

    Private _IntegrationTypes As IntegrationTypes
    Public Property IntegrationType() As IntegrationTypes
        Get
            Return _IntegrationTypes
        End Get
        Set(ByVal value As IntegrationTypes)
            _IntegrationTypes = value
        End Set
    End Property

#End Region

#Region " Events"
    Public Delegate Sub MessageEventHandler(ByVal sender As Object, ByVal e As MessageEventArgs)

    Protected MustOverride Sub RaiseMessageEvent(ByVal e As MessageEventArgs)
     

#End Region

#Region " General Methods"

    Public MustOverride Sub ExecProcessLaneData(ByRef strHeaderFile As String, ByRef strDataPath As String)

    Public MustOverride Sub ExecProcessOrderData(ByVal strHeaderFile As String, ByVal strDetailFile As String)

    ''' <summary>
    ''' Creates an instance of the desired Details object by Reflection.  
    ''' Pass in the File name like NGL.IntegrationServices and the Class name like CSVIntegration.  
    ''' This will create a new object of type StandardHeaders.
    ''' 
    ''' If will first load the assembly dll file, the create the object.
    ''' </summary> 
    ''' <returns>NGLISBaseClass</returns>
    ''' <remarks>Throws System.InvalidCastException if source is not a valid NGLISBaseClass. Throws unknown exceptions</remarks> 
    Public Shared Function NGLISBaseClassFactory(ByVal schemaSource As SchemaSource, _
                                                 ByVal db As String, _
                                          ByVal dbServer As String, _
                                          ByVal wcfURL As String, _
                                          ByVal wcfAuthCode As String, _
                                          ByVal wsURL As String, _
                                          ByVal wSLaneExtentionURL As String, _
                                          ByVal wSBookExtentionURL As String, _
                                          ByVal wsAuthCode As String, _
                                          ByVal filePathDirectory As String, _
                                          ByVal fileName As String, _
                                          ByVal fileName2 As String, _
                                          ByVal integrationType As IntegrationTypes) As NGLISBaseClass
        Try
            Dim t As Type = Nothing
            'if the file name is different than the dll we are in, then lets load the new dll.
            If schemaSource.CSVObjectFileNameSource <> "NGL.IntegrationServices" Then
                Dim assem As Assembly = Assembly.Load(schemaSource.CSVObjectFileNameSource)
                For Each item In assem.GetTypes
                    If item.Name = schemaSource.CSVObjectSource Then
                        t = item
                        Exit For
                    End If
                    Dim i As Integer = 0
                Next
            Else
                t = Type.[GetType]("NGL.IntegrationServices." & schemaSource.CSVObjectSource)
            End If

            Dim params As New NGL.FMWCFProxy.NGLIntegrationData.WCFParameters
            params.Database = db
            params.DBServer = dbServer
            params.UserName = t.Assembly.ManifestModule.ToString
            params.WCFAuthCode = wcfAuthCode

            Dim wwsWCF As New WCFWSParameters
            wwsWCF.WCFParametersProp = params
            wwsWCF.WSAuthCode = wsAuthCode
            wwsWCF.WSURL = wsURL
            wwsWCF.WCFURL = wcfURL
            wwsWCF.FileName = fileName
            wwsWCF.FileName2 = fileName2
            wwsWCF.FilePathDirectory = filePathDirectory
            wwsWCF.IntegrationType = integrationType
            wwsWCF.WSLaneExtentionURL = wSLaneExtentionURL
            wwsWCF.WSBookEXTURL = wSBookExtentionURL
            wwsWCF.Schemas = schemaSource
            Dim newC As NGLISBaseClass = TryCast(Activator.CreateInstance(t, New Object() {wwsWCF}), NGLISBaseClass)
            If newC Is Nothing Then Throw New System.InvalidCastException("The class " & schemaSource.CSVObjectSource & " is not a valid NGLISBaseClass")

            Return newC
        Catch ex As System.NullReferenceException
            Throw New System.InvalidCastException("The class " & schemaSource.CSVObjectSource & " is not a valid NGLISBaseClass")
        Catch ex As System.ArgumentNullException
            Throw New System.InvalidCastException("The class " & schemaSource.CSVObjectSource & " cannot be found or is not a valid NGLISBaseClass")
        Catch ex As System.MissingMethodException
            Throw New System.InvalidCastException("The class " & schemaSource.CSVObjectSource & " does not support the required constructor.  It may not be a valid NGLISBaseClass")
        Catch ex As Exception
            Throw
        End Try
        Return Nothing

    End Function

    ''' <summary>
    ''' Creates an instance of the desired Headers object by Reflection.  
    ''' Pass in the File name like NGL.IntegrationServices and the Class name like StandardHeaders.  
    ''' This will create a new object of type StandardHeaders.
    ''' 
    ''' If will first load the assembly dll file, the create the object.
    ''' </summary>
    ''' <param name="schemaSource"></param>
    ''' <returns></returns>
    ''' <remarks>Throws System.InvalidCastException if source is not a valid NGLISBaseClass. Throws unknown exceptions</remarks> 
    Public Shared Function NGLIHeadersFactory(ByVal schemaSource As SchemaSource) As IHeaders
        Try
            Dim t As Type = Nothing
            'if the file name is different than the dll we are in, then lets load the new dll.
            If schemaSource.HeadersObjectFileNameSchemaSource <> "NGL.IntegrationServices" Then
                Dim assem As Assembly = Assembly.Load(schemaSource.HeadersObjectFileNameSchemaSource)
                For Each item In assem.GetTypes
                    If item.Name = schemaSource.headerSchemaSource Then
                        t = item
                        Exit For
                    End If
                    Dim i As Integer = 0
                Next
            Else
                t = Type.[GetType]("NGL.IntegrationServices." & schemaSource.headerSchemaSource)
            End If

            Dim newC As IHeaders = TryCast(Activator.CreateInstance(t, New Object() {}), IHeaders)
            If newC Is Nothing Then Throw New System.InvalidCastException("The class " & schemaSource.headerSchemaSource & " is not a valid IHeaderObject")

            Return newC
        Catch ex As System.NullReferenceException
            Throw New System.InvalidCastException("The class " & schemaSource.headerSchemaSource & " is not a valid IHeaderObject")
        Catch ex As System.ArgumentNullException
            Throw New System.InvalidCastException("The class " & schemaSource.headerSchemaSource & " cannot be found or is not a valid IHeaderObject")
        Catch ex As System.MissingMethodException
            Throw New System.InvalidCastException("The class " & schemaSource.headerSchemaSource & " does not support the required constructor.  It may not be a valid IHeaderObject")
        Catch ex As Exception
            Throw
        End Try
        Return Nothing

    End Function

    ''' <summary>
    ''' Creates an instance of the desired Details object by Reflection.  
    ''' Pass in the File name like NGL.IntegrationServices and the Class name like StandardDetails.  
    ''' This will create a new object of type StandardHeaders.
    ''' 
    ''' If will first load the assembly dll file, the create the object.
    ''' </summary>
    ''' <param name="schemaSource"></param>
    ''' <returns>Throws System.InvalidCastException if source is not a valid NGLISBaseClass.  Throws unknown exceptions</returns>
    ''' <remarks></remarks>
    Public Shared Function NGLIDetailssFactory(ByVal schemaSource As SchemaSource) As IDetails
        Try
            Dim t As Type = Nothing
            'if the file name is different than the dll we are in, then lets load the new dll.
            If schemaSource.DetailsObjectFileNameSchemaSource <> "NGL.IntegrationServices" Then
                Dim assem As Assembly = Assembly.Load(schemaSource.DetailsObjectFileNameSchemaSource)
                For Each item In assem.GetTypes
                    If item.Name = schemaSource.DetailSchemaSource Then
                        t = item
                        Exit For
                    End If
                    Dim i As Integer = 0
                Next
            Else
                t = Type.[GetType]("NGL.IntegrationServices." & schemaSource.DetailSchemaSource)
            End If

            Dim newC As IDetails = TryCast(Activator.CreateInstance(t, New Object() {}), IDetails)
            If newC Is Nothing Then Throw New System.InvalidCastException("The class " & schemaSource.DetailSchemaSource & " is not a valid IDetailsObject")

            Return newC
        Catch ex As System.NullReferenceException
            Throw New System.InvalidCastException("The class " & schemaSource.DetailSchemaSource & " is not a valid IDetailsObject")
        Catch ex As System.ArgumentNullException
            Throw New System.InvalidCastException("The class " & schemaSource.DetailSchemaSource & " cannot be found or is not a valid IDetailsObject")
        Catch ex As System.MissingMethodException
            Throw New System.InvalidCastException("The class " & schemaSource.DetailSchemaSource & " does not support the required constructor.  It may not be a valid IDetailsObject")
        Catch ex As Exception
            Throw
        End Try
        Return Nothing

    End Function

    ''' <summary>
    ''' Creates an instance of the desired Lane Header object by Reflection.  
    ''' Pass in the File name like NGL.IntegrationServices and the Class name like StandardLaneHeaders.  
    ''' This will create a new object of type StandardHeaders.
    ''' 
    ''' If will first load the assembly dll file, the create the object.
    ''' </summary>
    ''' <param name="schemaSource"></param> 
    ''' <returns>Throws System.InvalidCastException if source is not a valid NGLISBaseClass.  Throws unknown exceptions</returns>
    Public Shared Function NGLILanesFactory(ByVal schemaSource As SchemaSource) As ILanes
        Try
            Dim t As Type = Nothing
            'if the file name is different than the dll we are in, then lets load the new dll.
            If schemaSource.LaneObjectFileNameSchemaSource <> "NGL.IntegrationServices" Then
                Dim assem As Assembly = Assembly.Load(schemaSource.LaneObjectFileNameSchemaSource)
                For Each item In assem.GetTypes
                    If item.Name = schemaSource.LaneSchemaSource Then
                        t = item
                        Exit For
                    End If
                    Dim i As Integer = 0
                Next
            Else
                t = Type.[GetType]("NGL.IntegrationServices." & schemaSource.LaneSchemaSource)
            End If

            Dim newC As ILanes = TryCast(Activator.CreateInstance(t, New Object() {}), ILanes)
            If newC Is Nothing Then Throw New System.InvalidCastException("The class " & schemaSource.LaneSchemaSource & " is not a valid ILanesObject")

            Return newC
        Catch ex As System.NullReferenceException
            Throw New System.InvalidCastException("The class " & schemaSource.LaneSchemaSource & " is not a valid ILanesObject")
        Catch ex As System.ArgumentNullException
            Throw New System.InvalidCastException("The class " & schemaSource.LaneSchemaSource & " cannot be found or is not a valid ILanesObject")
        Catch ex As System.MissingMethodException
            Throw New System.InvalidCastException("The class " & schemaSource.LaneSchemaSource & " does not support the required constructor.  It may not be a valid ILanesObject")
        Catch ex As Exception
            Throw
        End Try
        Return Nothing

    End Function

    Public Sub Configure(ByVal debug As Boolean, _
                         ByVal mstrLocalFolder As String, _
                         ByVal mstrResultsFile As String, _
                         ByVal GroupEmail As String, _
                         ByVal AdminEmail As String, _
                         ByVal keepLogDays As String, _
                         ByVal parameters As WCFParameters, _
                         ByVal laneWebServiceURL As String, _
                         ByVal importFileType As Integer)
        Me.Debug = debug
        Me.LogFile = Me.buildPath(mstrLocalFolder, mstrResultsFile)

        Me.GroupEmail = GroupEmail
        Me.AdminEmail = AdminEmail
        Me.KeepLogDays = keepLogDays
        Me.Source = "PODownload Lane"
        Me._Parameters = parameters
        Me.WebServiceURL = laneWebServiceURL
        Me.ImportTypeKey = importFileType
    End Sub

    Public Function buildPath(ByVal strFolder As String, ByVal strFileName As String) As String
        Try
            If strFolder.Substring(strFolder.Length - 1, 1) <> "\" Then
                strFolder = strFolder & "\"
            End If
            Return strFolder & strFileName
        Catch ex As Exception
            mobjLastException = ex
            Return ""
        End Try

    End Function

    Public Sub openLog()
        Try
            If mstrLogFile.Length > 0 Then
                mobjLog = New clsLog
                mobjLog.Debug = mblnDebug
                mioLog = mobjLog.Open(mstrLogFile, KeepLogDays, SaveOldLog)
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub closeLog(ByVal intReturn As Integer)
        Try
            If IsNothing(mobjLog) Then
                Exit Sub
            End If
            mobjLog.closeLog(intReturn, mioLog)
        Catch ex As Exception
            'ignore any errors when closing the log file
        End Try
    End Sub

    Public Sub Log(ByVal logMessage As String)

        Try
            If IsNothing(mobjLog) Then
                Exit Sub
            End If
            mobjLog.Write(logMessage, mioLog)
        Catch ex As Exception
            'ignore any errors while writing to the log
        End Try

    End Sub

    Protected Function validateLaneRequirements(ByVal laneName As String, ByVal laneNumber As String)
        Dim success As Boolean = False
        If laneName Is Nothing OrElse laneName.Length = 0 Then
            Return success
        End If
        If laneNumber Is Nothing OrElse laneNumber.Length = 0 Then
            Return success
        End If
        Return True
    End Function

    Public Shared Function saveErrMsg(ByRef strMsg As String) As String

        'On Error Resume Next

        'gstrLastErrorMessage = Left(strMsg.Trim, 999)
        'gDBCon.Execute("Exec spAppendAppError '" & padQuotes(gstrLastErrorMessage) & "'")
        'saveErrMsg = gstrLastErrorMessage



#Disable Warning BC42105 ' Function 'saveErrMsg' doesn't return a value on all code paths. A null reference exception could occur at run time when the result is used.
    End Function
#Enable Warning BC42105 ' Function 'saveErrMsg' doesn't return a value on all code paths. A null reference exception could occur at run time when the result is used.


    Sub LogResults(ByVal ModuleName As String, ByVal Result As Integer, ByVal LastError As String, ByVal AuthorizationCode As String)

        Try
            Dim message As String = String.Format("{0},{1},{2},{3},{4}", Now.ToString("MM/dd/yyyy hh:mm tt"), _
                    ModuleName, Result, LastError, AuthorizationCode)
            Using sw As New IO.StreamWriter(Me.LogFile, True)
                sw.WriteLine(message)
                sw.Close()
            End Using
        Catch ex As Exception

        End Try

    End Sub

    Sub LogException(ByVal ModuleName As String, _
                                   ByVal Result As Integer, _
                                   ByVal logMessage As String, _
                                   ByVal ex As Exception, _
                                   ByVal AuthorizationCode As String, _
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

    Sub LogMessage(ByVal ModuleName As String, ByVal Msg As String)

        Try
            Using sw As New IO.StreamWriter(Me.LogFile, True)
                sw.WriteLine(String.Format("{0},{1},{2}", Now.ToString("MM/dd/yyyy hh:mm tt"), ModuleName, Msg))
                sw.Close()
            End Using
        Catch ex As Exception

        End Try

    End Sub

    Public Sub SendEmail(ByVal Subject As String, _
                                ByVal Message As String)
        Try

            Dim oBatch As New BatchProcessingIntegration(ConvertToWCFProperties(New NGLBatchProcessingData.WCFParameters, Me.WCFParametersProp), Me.WCFURL)

            If Not oBatch.SendToNGLEmailService(Me.GroupEmail, Me.AdminEmail, "", Subject, Message) Then
                If Not String.IsNullOrEmpty(oBatch.LastError) Then LogMessage("Send Email", "Failure: " & oBatch.LastError)
            End If

        Catch ex As Exception
            'Because this function is typically called when we are processing exceptions
            'we do nothing when sending an email from the web service 
        End Try
    End Sub

    Public Function ConvertToWCFProperties(ByRef WCFProperty As Object, ByVal TOwcfProp As Object)
        WCFProperty.Database = TOwcfProp.Database
        WCFProperty.DBServer = TOwcfProp.DBServer
        WCFProperty.UserName = TOwcfProp.UserName
        WCFProperty.UserRemotePassword = TOwcfProp.UserRemotePassword
        WCFProperty.WCFAuthCode = TOwcfProp.WCFAuthCode
        WCFProperty.CompControl = TOwcfProp.CompControl
        WCFProperty.FormControl = TOwcfProp.FormControl
        WCFProperty.FormName = TOwcfProp.FormName
        WCFProperty.ProcedureControl = TOwcfProp.ProcedureControl
        WCFProperty.ProcedureName = TOwcfProp.ProcedureName
        WCFProperty.ReportControl = TOwcfProp.ReportControl
        WCFProperty.ReportName = TOwcfProp.ReportName
        WCFProperty.ValidateAccess = TOwcfProp.ValidateAccess
        Return WCFProperty
    End Function

#End Region

#Region "FmFileImportErrorLogData Methods"

    Protected Function AddNewImportErrorLog(ByVal errorLog As FileImportErrorLog) As WCFResult
        Dim result As New WCFResult
        Try
            result = fmFileImportErrorLogData.AddNewRecord(errorLog, (New FMDataProperties()).ConvertToWCFProperties(New NGLIntegrationData.WCFParameters), Nothing)
        Catch iop As InvalidOperationException
            Log("AddNewImportErrorLog InvalidOperationException: " & iop.Message)
        Catch ex As Exception
            Log("AddNewImportErrorLog Exception: " & ex.Message)
        End Try
        Return result
    End Function

    Private Sub fmFileImportErrorLogData_FaultException(sender As Object, e As FMWCFProxy.FaultExceptionEventArgs) Handles fmFileImportErrorLogData.FaultException
        Log(e.Detail & " " & e.Reason)
    End Sub

    Private Sub fmFileImportErrorLogData_TimeOutException(sender As Object, e As FMWCFProxy.FaultExceptionEventArgs) Handles fmFileImportErrorLogData.TimeOutException
        Log(e.Detail & " " & e.Reason)
    End Sub

#End Region


    'Public Shared Function getHeaderFileName(ByVal detailfilename As String) As String
    '    Return addPathToFile(My.Settings.HeaderFilePrefix & Regex.Split(Regex.Split(detailfilename, My.Settings.FileNameExtension)(0), My.Settings.DetailFilePrefix)(1) & My.Settings.FileNameExtension)
    'End Function


    'Public Shared Function getDetailFileName(ByVal filename As String) As String
    '    Return addPathToFile(filename)
    'End Function

    'Public Shared Function addPathToFile(ByVal filename As String) As String
    '    Dim strPath As String = My.Settings.LocalFilePath
    '    If Not Right(strPath, 1) = "\" Then strPath &= "\"
    '    Return strPath & filename
    'End Function
End Class

Public Class WCFWSParameters

    Private _WSAuthCode As String
    Public Property WSAuthCode() As String
        Get
            Return _WSAuthCode
        End Get
        Set(ByVal value As String)
            _WSAuthCode = value
        End Set
    End Property

    Private _WSURL As String
    Public Property WSURL() As String
        Get
            Return _WSURL
        End Get
        Set(ByVal value As String)
            _WSURL = value
        End Set
    End Property

    Private _WSLaneExtentionURL As String
    Public Property WSLaneExtentionURL() As String
        Get
            Return _WSLaneExtentionURL
        End Get
        Set(ByVal value As String)
            _WSLaneExtentionURL = value
        End Set
    End Property

    Private _WsBookExtenstionURL As String
    Public Property WSBookEXTURL() As String
        Get
            Return _WsBookExtenstionURL
        End Get
        Set(ByVal value As String)
            _WsBookExtenstionURL = value
        End Set
    End Property

    Private _WCFURL As String
    Public Property WCFURL() As String
        Get
            Return _WCFURL
        End Get
        Set(ByVal value As String)
            _WCFURL = value
        End Set
    End Property

    Private _FileName As String
    Public Property FileName() As String
        Get
            Return _FileName
        End Get
        Set(ByVal value As String)
            _FileName = value
        End Set
    End Property

    Private _FileName2 As String
    Public Property FileName2() As String
        Get
            Return _FileName2
        End Get
        Set(ByVal value As String)
            _FileName2 = value
        End Set
    End Property

    Private _FilePathDirectory As String
    Public Property FilePathDirectory() As String
        Get
            Return _FilePathDirectory
        End Get
        Set(ByVal value As String)
            _FilePathDirectory = value
        End Set
    End Property

    Private _IntegrationTypes As IntegrationTypes
    Public Property IntegrationType() As IntegrationTypes
        Get
            Return _IntegrationTypes
        End Get
        Set(ByVal value As IntegrationTypes)
            _IntegrationTypes = value
        End Set
    End Property

    Private _AddDeliveryDays As Integer = 0
    Public Property AddDeliveryDays() As Integer
        Get
            Return _AddDeliveryDays
        End Get
        Set(ByVal value As Integer)
            _AddDeliveryDays = value
        End Set
    End Property


    Private _WCFParameters As NGL.FMWCFProxy.NGLIntegrationData.WCFParameters
    Public Property WCFParametersProp() As NGL.FMWCFProxy.NGLIntegrationData.WCFParameters
        Get
            Return _WCFParameters
        End Get
        Set(ByVal value As NGL.FMWCFProxy.NGLIntegrationData.WCFParameters)
            _WCFParameters = value
        End Set
    End Property

    Private _Schemas As New SchemaSource
    Public Property Schemas() As SchemaSource
        Get
            Return _Schemas
        End Get
        Set(ByVal value As SchemaSource)
            _Schemas = value
        End Set
    End Property

End Class

Public Class SchemaSource
    Public headerSchemaSource As String = ""
    Public DetailSchemaSource As String = ""
    Public LaneSchemaSource As String = ""
    Public CSVObjectSource As String = ""
    Public CSVObjectFileNameSource As String = ""
    Public LaneObjectFileNameSchemaSource As String = ""
    Public DetailsObjectFileNameSchemaSource As String = ""
    Public HeadersObjectFileNameSchemaSource As String = ""
End Class

Public Class MessageEventArgs

    Public Sub New(ByVal message As String, Optional ByVal [error] As Exception = Nothing)
        Me.Message = message
        Me.ExceptionObject = [error]
    End Sub

    Private _Exception As Object
    Public Property ExceptionObject() As Object
        Get
            Return _Exception
        End Get
        Set(ByVal value As Object)
            _Exception = value
        End Set
    End Property

    Private _Message As String
    Public Property Message() As String
        Get
            Return _Message
        End Get
        Set(ByVal value As String)
            _Message = value
        End Set
    End Property

End Class