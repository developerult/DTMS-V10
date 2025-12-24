Imports NGL.FMWCFProxy.NGLIntegrationData
Imports NGL.IntegrationServices.NglLaneWebService

Public Class ConvertCSV
    Inherits NGLISBaseClass
    Implements IIntegration



#Region "Constructors"

    Public Sub New(ByVal wwobject As Object)
        MyBase.New(wwobject)

        Dim params As WCFWSParameters = Nothing
        params = TryCast(wwobject, WCFWSParameters)

        Me.FileName = params.FileName
        Me.FilePathDirectory = params.FilePathDirectory
        Me.HeaderFileName = params.FileName
        Me.DetailFileName = params.FileName2
        Me.WebServiceURL = params.WSURL
        Me.WebServiceAuthCode = params.WSAuthCode
        Me.LaneWebServiceURLExtension = params.WSLaneExtentionURL
        Me.WSBookExtURL = params.WSBookEXTURL
        Me.WCFParametersProp = params.WCFParametersProp
        Me.WCFURL = params.WCFURL
        Me.AddDeliveryDays = params.AddDeliveryDays
        Me.Schemas = params.Schemas
        Me.IntegrationType = params.IntegrationType
    End Sub

#End Region

#Region "Inherited Methods"

    Public Sub ProcessData() Implements IIntegration.ProcessData

        Dim url As New Uri(Me.WebServiceURL)
        LogFile = My.Computer.FileSystem.SpecialDirectories.CurrentUserApplicationData & "\" & url.Host & ".log"
        Me.openLog()
        Select Case Me.IntegrationType
            Case NGLISBaseClass.IntegrationTypes.Book
                Me.ExecProcessOrderData(Me.HeaderFileName, Me.DetailFileName)
            Case NGLISBaseClass.IntegrationTypes.Lane
                Me.ExecProcessLaneData(Me.FileName, Me.FilePathDirectory)
        End Select
        Me.closeLog(Me.Results)
        ProcessesDataLog("Complete")
        ''Backup and Delete the Lane File
        'MoveFile(objF.buildPath(mstrLocalFolder, "Lane.txt"), objF.buildPath(mstrLocalBackupFolder, objF.timeStampFileName("Lane.txt")))
    End Sub

    Public Overrides Sub ExecProcessOrderData(ByVal strHeaderFile As String, ByVal strDetailFile As String)
        Dim statusMsg As String = ""
        Dim Headers As IHeaders = Nothing
        Dim Details As IDetails = Nothing
        Dim ErrMsg As String = ""
        strHeaderFile = Me.buildPath(Me.FilePathDirectory, strHeaderFile)
        strDetailFile = Me.buildPath(Me.FilePathDirectory, strDetailFile)
        Try
            'try to create by reflection
            Details = NGLIDetailssFactory(Me.Schemas)
            Headers = NGLIHeadersFactory(Me.Schemas)
            If Details Is Nothing Or Headers Is Nothing Then
                ProcessesDataException(New ApplicationException(ErrMsg), "Create header and Detail by Reflection failed Failed", ErrMsg)
                Return
            End If

            'read the details data from the file
            Log("Process Data -- Read Detail File Data: readFileToEnd")
            ProcessesDataLog("Process Data -- Read Detail File Data: readFileToEnd")
            Dim DetailsData As String = Utilities.readFileToEnd(strDetailFile, ErrMsg)
            'check for errors
            If Not String.IsNullOrEmpty(ErrMsg) Then
                ProcessesDataException(New ApplicationException(ErrMsg), "ReadFileToEnd Details Data Failed", ErrMsg)
                Return
            End If

            'parse details
            Log("Process Data -- Parse Detail Data: " & strDetailFile)
            ProcessesDataLog("Process Data -- Parse Detail Data: " & strDetailFile)
            Details.ReadFromFile(strDetailFile)
            'check for errors
            If Not String.IsNullOrEmpty(Details.Errors) Then
                ProcessesDataException(New ApplicationException(Details.Errors), "Parse Detail Data Failed", Details.Errors)
                Return
            End If

            'Read Header File
            Log("Process Data -- Read Header File Data")
            ProcessesDataLog("Process Data -- Read Header File Data")
            Headers.ReadFromFile(strHeaderFile, Details.Items)
            If Not String.IsNullOrEmpty(Headers.Errors) Then
                ProcessesDataException(New ApplicationException(Headers.Errors), "ReadFromFile Header Data Failed", Headers.Errors)
                Return
            End If

            ErrMsg = ""

            'Process the po data   
            ErrMsg = ""
            Dim oBook As New BookObjectIntegration(Me.WebServiceAuthCode, Utilities.createValidURL(Me.WebServiceURL, Me.WSBookExtURL))
            Dim headerList As New List(Of NGLBookWebService.clsBookHeaderObject)
            Dim detailList As New List(Of NGLBookWebService.clsBookDetailObject)

            Dim blnRet = Headers.buildBookObjectData(Headers, ErrMsg, headerList, detailList)
            If Not String.IsNullOrEmpty(ErrMsg) Then
                ProcessesDataException(Nothing, "Process PO Record Errors or Warnings", ErrMsg)
            End If
            ErrMsg = ""
            'Finally we can add data using the web service
            If oBook.AddOrders(headerList.ToArray, detailList.ToArray) <> 0 Then
                If Not String.IsNullOrEmpty(oBook.LastError) Then
                    ProcessesDataException(Nothing, "Add PO Record Failed", oBook.LastError)
                    Return
                End If
            End If

            Return

        Catch ex As Exception
            Me.ProcessesDataException(ex)
        Finally

        End Try
    End Sub

    Public Overrides Sub ExecProcessLaneData(ByRef strHeaderFile As String, ByRef strDataPath As String)

        Dim statusMsg As String = ""
        Dim ilanes As ILanes = Nothing
        Dim ErrMsg As String = ""
        Try
            Dim strLaneFile As String = Me.buildPath(strDataPath, strHeaderFile)
            If Not System.IO.File.Exists(strLaneFile) Then
                ProcessesDataException(New Exception("File Not Found"), "The import lane file " & strLaneFile & "does not exists.")
                Return
            End If

            'try to create by reflection
            ilanes = NGLILanesFactory(Me.Schemas)
            If ilanes Is Nothing Or ilanes Is Nothing Then
                ProcessesDataException(New ApplicationException(ErrMsg), "Create ilanes by Reflection failed Failed", ErrMsg)
                Return
            End If

            Log("Process Data -- Read Lane Data")
            ProcessesDataLog("Process Data -- Read Lane Data")
            ilanes.ReadFromFile(strLaneFile)
            If Not String.IsNullOrEmpty(ilanes.Errors) Then
                ProcessesDataException(New ApplicationException(ilanes.Errors), "Process Data Failed", ilanes.Errors)
                Return
            End If

            ErrMsg = ""

            'Process the lane data   
            ErrMsg = ""
            Dim oLane As New LaneObjectIntegration(WebServiceAuthCode, Utilities.createValidURL(Me.WebServiceURL, Me.LaneWebServiceURLExtension))
            Dim laneList As List(Of clsLaneObject) = ilanes.buildLaneObjects(ErrMsg)
            If Not String.IsNullOrEmpty(ErrMsg) Then
                ProcessesDataException(Nothing, "Process PO Record Errors or Warnings", ErrMsg)
            End If
            ErrMsg = ""
            'Finally we can add data using the web service
            If oLane.AddLaness(laneList.ToArray) <> 0 Then
                If Not String.IsNullOrEmpty(oLane.LastError) Then
                    ProcessesDataException(Nothing, "Add PO Record Failed", oLane.LastError)
                    Return
                End If
            End If

            Return

        Catch ex As Exception
            Me.ProcessesDataException(ex)
        Finally

        End Try
    End Sub

#End Region

#Region "General Methods"

    Private Sub ProcessesDataException(ByVal ex As Exception, Optional ByVal reason As String = "", Optional ByVal details As String = "")

        If Not ex Is Nothing Then
            LogException("Ngl.IntegrationServices.CSVIntegration", 1, reason, ex, Me.WebServiceAuthCode)
            Dim message As String = String.Format("{0},{1},{2},{3},{4}", Now.ToString("MM/dd/yyyy hh:mm tt"), _
                    "Ngl.IntegrationServices.CSVIntegration", 1, LastError, WebServiceAuthCode)
            Dim messageArg As New MessageEventArgs(message & " " & reason, ex)
            RaiseMessageEvent(messageArg)
        Else
            LogException("Ngl.Data.WebReceiver", 1, reason, New ApplicationException(details), Me.WebServiceAuthCode)
            Dim message As String = String.Format("{0},{1},{2},{3},{4}", Now.ToString("MM/dd/yyyy hh:mm tt"), _
                    "Ngl.IntegrationServices.CSVIntegration", 1, LastError, WebServiceAuthCode)
            Dim messageArg As New MessageEventArgs(message & " " & reason, ex)
            RaiseMessageEvent(messageArg)
        End If

    End Sub

    Private Sub ProcessesDataLog(ByVal reason As String)
 
            Dim message As String = String.Format("{0},{1},{2},{3},{4}", Now.ToString("MM/dd/yyyy hh:mm tt"), _
                    "Ngl.IntegrationServices.CSVIntegration", 1, LastError, WebServiceAuthCode)
        Dim messageArg As New MessageEventArgs(message & " " & reason, Nothing)
            RaiseMessageEvent(messageArg)


    End Sub

#End Region

    Protected Overrides Sub RaiseMessageEvent(e As MessageEventArgs)
        RaiseEvent MessageEvent(Me, e)
    End Sub

    Public Event MessageEvent(sender As Object, e As MessageEventArgs) Implements IIntegration.MessageEvent
End Class
