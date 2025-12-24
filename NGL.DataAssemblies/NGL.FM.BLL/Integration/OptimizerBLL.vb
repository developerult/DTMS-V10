Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Data.Linq
Imports System.Data.SqlClient
Imports System.ServiceModel

Imports DAL = NGL.FreightMaster.Data
Imports DTO = NGL.FreightMaster.Data.DataTransferObjects
Imports LTS = NGL.FreightMaster.Data.LTS
Imports DTran = NGL.Core.Utility.DataTransformation
Imports Models = NGL.FreightMaster.Data.Models

Imports NGL.Core.Utility

Imports PCM = NGL.FreightMaster.PCMiler

Imports System.IO
Imports System.Text
Imports System.Threading
Imports NGL.Core.ChangeTracker
Imports NGL.Optimization

Imports eOptMsgType = NGL.FreightMaster.Data.NGLOptMsgData.OptMsgType

'Public Class TSOptimizationCriteria
'    Public Filters As String
'    Public CompControl As Integer
'    Public DefCompNumber As Integer
'End Class

Public Class OptimizerDataFilters
    Public CompName As String
    Public Inbound As Boolean
End Class


Public Class OptimizerBLL : Inherits BLLBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As DAL.WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Me.SourceClass = "OptimizerBLL"
    End Sub

#End Region

#Region " Delegates "

    'Delegate Function
    Public Delegate Sub ProcessDataDelegate(ByVal CompControl As Integer, ByVal Filter As String, ByVal optDataFltr As OptimizerDataFilters)

#End Region


#Region " Properties"

    Public gPCMiler As New PCM.PCMiles
    Private _PCMParametersLoaded As Boolean = False
    Private _PCMLoggingOn As Boolean = False
    Private _PCMLogFile As String = ""


    'Private Declare Function SetCurrentDirectoryA Lib "kernel32" (ByVal lpPathName As String) As Integer


    'Private WithEvents _FMBookData As New FMBookData
    '    Private WithEvents _FMCarrierData As New FMCarrierData
    '    Private WithEvents _FMLaneData As New FMLaneData
    '    Private WithEvents _FMCompData As New FMCompData
    '    Private WithEvents _FMParameterData As New FMParameterData
    '    Private WithEvents _FMBatchProcessingData As New FMBatchProcessingData


    Private _TSStops As Integer = 0

    'Private _TSAppPath As String = ""
    'Public Property TSAppPath() As String
    '        Get
    '            If String.IsNullOrEmpty(_TSAppPath) Then
    '                _TSAppPath = getParText("TSAppPath")
    '                If _TSAppPath > " " Then
    '                    If Right(_TSAppPath, 1) <> "\" Then _TSAppPath &= "\"
    '                Else
    '                    _TSAppPath = TSDataPath
    '                End If
    '            End If
    '            Return _TSAppPath
    '        End Get
    '        Set(ByVal value As String)
    '            _TSAppPath = value
    '        End Set
    '    End Property

    'Private _TSDataPath As String = ""
    'Public Property TSDataPath() As String
    '    Get
    '        If String.IsNullOrEmpty(_TSDataPath) Then
    '            _TSDataPath = getParText("TSDataPath")
    '            If _TSDataPath > " " Then
    '                If Right(_TSDataPath, 1) <> "\" Then _TSDataPath &= "\"
    '            Else
    '                'Note: this code will not work on Win Vista or Win 7 or Win Server 2008 because of security
    '                _TSDataPath = If(Right(Application.ApplicationPath, 1) = "\", Application.ApplicationPath, Application.ApplicationPath & "\") & "TSWin\"
    '            End If
    '        End If
    '        Return _TSDataPath
    '    End Get
    '    Set(ByVal value As String)
    '        _TSDataPath = value
    '    End Set
    'End Property

    'Private _TSSolutionFile As String = ""
    'Public Property TSSolutionFile() As String
    '    Get
    '        If String.IsNullOrEmpty(_TSSolutionFile) Then _TSSolutionFile = TSDataPath & "PrintStp.txt"
    '        Return _TSSolutionFile
    '    End Get
    '    Set(ByVal value As String)
    '        _TSSolutionFile = value
    '    End Set
    'End Property

    'Private _TSTimeOut As Double = 0
    'Public Property TSTimeOut() As Double
    '    Get
    '        If _TSTimeOut = 0 Then
    '            _TSTimeOut = getParValue("TSTimeOutVar")
    '        End If
    '        Return _TSTimeOut
    '    End Get
    '    Set(ByVal value As Double)
    '        _TSTimeOut = value
    '    End Set
    'End Property

    'Private _TSWorkTimeLimit As String = ""
    'Public Property WorkTimeLimit As String
    '    Get

    '        If _TSWorkTimeLimit = "" Then
    '            _TSWorkTimeLimit = getParValue("TSWorkTimeLimit").ToString
    '        End If
    '        Return Left(_TSWorkTimeLimit, 6)
    '    End Get
    '    Set(ByVal value As String)
    '        _TSWorkTimeLimit = Left(value, 6)
    '    End Set
    'End Property

    'Private _TSDrivingTimeLimit As String = ""
    'Public Property DrivingTimeLimit As String
    '    Get
    '        If _TSDrivingTimeLimit = "" Then
    '            _TSDrivingTimeLimit = getParValue("TSDriveTimeLimit").ToString
    '        End If
    '        Return Left(_TSDrivingTimeLimit, 6)
    '    End Get
    '    Set(ByVal value As String)
    '        _TSDrivingTimeLimit = Left(value, 6)
    '    End Set
    'End Property

    'Private _TSLayoverPeriod As String = ""
    'Public Property LayoverPeriod As String
    '    Get
    '        If _TSLayoverPeriod = "" Then
    '            _TSLayoverPeriod = getParValue("TSLayoverPeriod").ToString
    '        End If
    '        Return Left(_TSLayoverPeriod, 6)
    '    End Get
    '    Set(ByVal value As String)
    '        _TSLayoverPeriod = Left(value, 6)
    '    End Set
    'End Property

    'Private _TSWaitForStops As Double = 0
    'Public Property TSWaitForStops() As Double
    '    Get
    '        If _TSWaitForStops = 0 Then
    '            _TSWaitForStops = getParValue("TSWaitForStops")
    '        End If
    '        Return _TSWaitForStops
    '    End Get
    '    Set(ByVal value As Double)
    '        _TSWaitForStops = value
    '    End Set
    'End Property



    Private _NGLOptimizerOn As Double = 0
    Public Property NGLOptimizerOn() As Double
        Get
            If _NGLOptimizerOn = 0 Then
                _NGLOptimizerOn = getParValue("NGLOptimizerOn")
            End If
            Return _NGLOptimizerOn
        End Get
        Set(ByVal value As Double)
            _NGLOptimizerOn = value
        End Set
    End Property

    Private _NGLOptimizerDataPath As String
    Public Property NGLOptimizerDataPath As String
        Get
            If String.IsNullOrEmpty(_NGLOptimizerDataPath) Then
                _NGLOptimizerDataPath = getParText("NGLOptimizerDataPath", 0, "C:\Data\NGL\")
                If String.IsNullOrEmpty(_NGLOptimizerDataPath) OrElse _NGLOptimizerDataPath.Length < 1 Then _NGLOptimizerDataPath = "C:\Data\NGL\"
            End If
            If Right(_NGLOptimizerDataPath, 1) <> "\" Then _NGLOptimizerDataPath &= "\"
            Return _NGLOptimizerDataPath
        End Get
        Set(value As String)
            _NGLOptimizerDataPath = value
        End Set
    End Property

    Private _NGLOptimizerDefMaxCubes As Double = 0
    Public Property NGLOptimizerDefMaxCubes() As Double
        Get
            If _NGLOptimizerDefMaxCubes = 0 Then
                _NGLOptimizerDefMaxCubes = getParValue("NGLOptimizerDefMaxCubes")
                If _NGLOptimizerDefMaxCubes < 1 Then _NGLOptimizerDefMaxCubes = 1100
            End If
            Return _NGLOptimizerDefMaxCubes
        End Get
        Set(ByVal value As Double)
            _NGLOptimizerDefMaxCubes = value
        End Set
    End Property

    Private _NGLOptimizerDefMaxPallets As Double = 0
    Public Property NGLOptimizerDefMaxPallets() As Double
        Get
            If _NGLOptimizerDefMaxPallets = 0 Then
                _NGLOptimizerDefMaxPallets = getParValue("NGLOptimizerDefMaxPallets")
                If _NGLOptimizerDefMaxPallets < 1 Then _NGLOptimizerDefMaxPallets = 28
            End If
            Return _NGLOptimizerDefMaxPallets
        End Get
        Set(ByVal value As Double)
            _NGLOptimizerDefMaxPallets = value
        End Set
    End Property

    Private _NGLOptimizerDefMaxWeight As Double = 0
    Public Property NGLOptimizerDefMaxWeight() As Double
        Get
            If _NGLOptimizerDefMaxWeight = 0 Then
                _NGLOptimizerDefMaxWeight = getParValue("NGLOptimizerDefMaxWeight")
                If _NGLOptimizerDefMaxWeight < 1 Then _NGLOptimizerDefMaxWeight = 43000
            End If
            Return _NGLOptimizerDefMaxWeight
        End Get
        Set(ByVal value As Double)
            _NGLOptimizerDefMaxWeight = value
        End Set
    End Property

    Private _NGLOptimizerDefMaxCases As Double = 0
    Public Property NGLOptimizerDefMaxCases() As Double
        Get
            If _NGLOptimizerDefMaxCases = 0 Then
                _NGLOptimizerDefMaxCases = getParValue("NGLOptimizerDefMaxCases")
                If _NGLOptimizerDefMaxCases < 1 Then _NGLOptimizerDefMaxCases = 1250
            End If
            Return _NGLOptimizerDefMaxCases
        End Get
        Set(ByVal value As Double)
            _NGLOptimizerDefMaxCases = value
        End Set
    End Property

    Private _NGLOptimizerMaxStopsPerTruck As Double = 0
    Public Property NGLOptimizerMaxStopsPerTruck() As Double
        Get
            If _NGLOptimizerMaxStopsPerTruck = 0 Then
                _NGLOptimizerMaxStopsPerTruck = getParValue("NGLOptimizerMaxStopsPerTruck")
                If _NGLOptimizerMaxStopsPerTruck < 1 Then _NGLOptimizerMaxStopsPerTruck = 11
            End If
            Return _NGLOptimizerMaxStopsPerTruck
        End Get
        Set(ByVal value As Double)
            _NGLOptimizerMaxStopsPerTruck = value
        End Set
    End Property

    Private _NGLOptimizerDistanceFileName As String
    Public Property NGLOptimizerDistanceFileName As String
        Get
            If String.IsNullOrEmpty(_NGLOptimizerDistanceFileName) Then
                _NGLOptimizerDistanceFileName = getParText("NGLOptimizerDistanceFileName")
                If String.IsNullOrEmpty(_NGLOptimizerDistanceFileName) OrElse _NGLOptimizerDistanceFileName.Length < 1 Then _NGLOptimizerDistanceFileName = "DIS3_data_NGL51.csv"
            End If
            Return _NGLOptimizerDistanceFileName
        End Get
        Set(value As String)
            _NGLOptimizerDistanceFileName = value
        End Set
    End Property

    Private _NGLOptimizerStopFileName As String
    Public Property NGLOptimizerStopFileName As String
        Get
            If String.IsNullOrEmpty(_NGLOptimizerStopFileName) Then
                _NGLOptimizerStopFileName = getParText("NGLOptimizerStopFileName")
                If String.IsNullOrEmpty(_NGLOptimizerStopFileName) OrElse _NGLOptimizerStopFileName.Length < 1 Then _NGLOptimizerStopFileName = "SDF5_data_Dry_NGL51.csv"
            End If
            Return _NGLOptimizerStopFileName
        End Get
        Set(value As String)
            _NGLOptimizerStopFileName = value
        End Set
    End Property

    Private _NGLOptimizerParameterFileName As String
    Public Property NGLOptimizerParameterFileName As String
        Get
            If String.IsNullOrEmpty(_NGLOptimizerParameterFileName) Then
                _NGLOptimizerParameterFileName = getParText("NGLOptimizerParameterFileName")
                If String.IsNullOrEmpty(_NGLOptimizerParameterFileName) OrElse _NGLOptimizerParameterFileName.Length < 1 Then _NGLOptimizerParameterFileName = "Parameter_Values.csv"
            End If
            Return _NGLOptimizerParameterFileName
        End Get
        Set(value As String)
            _NGLOptimizerParameterFileName = value
        End Set
    End Property

    Private _NGLOptimizerResultFileName As String
    Public Property NGLOptimizerResultFileName As String
        Get
            If String.IsNullOrEmpty(_NGLOptimizerResultFileName) Then
                _NGLOptimizerResultFileName = getParText("NGLOptimizerResultFileName")
                If String.IsNullOrEmpty(_NGLOptimizerResultFileName) OrElse _NGLOptimizerResultFileName.Length < 1 Then _NGLOptimizerResultFileName = "DISmin_OUT1.doc"
            End If
            Return _NGLOptimizerResultFileName
        End Get
        Set(value As String)
            _NGLOptimizerResultFileName = value
        End Set
    End Property

    Private _NGLOptResultFile As String = ""
    Public Property NGLOptResultFile() As String
        Get
            If String.IsNullOrEmpty(_NGLOptResultFile) Then _NGLOptResultFile = NGLOptimizerDataPath & NGLOptimizerResultFileName
            Return _NGLOptResultFile
        End Get
        Set(ByVal value As String)
            _NGLOptResultFile = value
        End Set
    End Property

    Private _NGLOptimizerTimeOut As Double = 0
    Public Property NGLOptimizerTimeOut() As Double
        Get
            If _NGLOptimizerTimeOut = 0 Then
                _NGLOptimizerTimeOut = getParValue("NGLOptimizerTimeOut")
                If _NGLOptimizerTimeOut < 1 Then _NGLOptimizerTimeOut = 100
            End If
            Return _NGLOptimizerTimeOut
        End Get
        Set(ByVal value As Double)
            _NGLOptimizerTimeOut = value
        End Set
    End Property

#End Region

#Region "PC Miler"

    'PCMiler Configuration
    Public Sub getPCMilerParameters(ByRef blnLoggingOn As Boolean, ByRef strPCMilerLogFile As String)
        If NGLBatchProcessData.GetParValue("PCMilerLogging", 0) <> 0 Then
            strPCMilerLogFile = NGLBatchProcessData.GetParText("PCMilerLogging", 0)
            If Len(Trim(strPCMilerLogFile)) > 0 Then blnLoggingOn = True
        End If
        If NGLBatchProcessData.GetParValue("GlobalDebugMode", 0) = 0 Then gPCMiler.Debug = False Else gPCMiler.Debug = True
        If NGLBatchProcessData.GetParValue("GlobalKeepLogDays", 0) = 0 Then gPCMiler.KeepLogDays = False Else gPCMiler.KeepLogDays = True
        If NGLBatchProcessData.GetParValue("GlobalSaveOldLogs", 0) = 0 Then gPCMiler.SaveOldLog = False Else gPCMiler.SaveOldLog = True
        gPCMiler.WebServiceURL = NGLBatchProcessData.GetParText("PCMilerWebServiceURL", 0)
        If NGLBatchProcessData.GetParValue("PCMilerUseZipOnly", 0) = 0 Then gPCMiler.UseZipOnly = False Else gPCMiler.UseZipOnly = True
    End Sub

    Public Function UpdateBookConsMultiPickPCMilerTwoWay(ByRef oBatch As DAL.NGLBatchProcessDataProvider, ByVal oFMStops As List(Of PCM.clsFMStopData)) As Boolean
        Dim blnRet As Boolean
        Dim blnProcessNextStop As Boolean = True
        Try
            For Each oStop In oFMStops
                If Not blnProcessNextStop Then Exit For
                Try
                    With oStop
                        blnRet = oBatch.UpdateBookConsMultiPickPCMiler(.BookControl, .LocationisOrigin, .StopNumber, .LegMiles, .LegCost, .LegTime, .LegTolls, .LegESTCHG, True)
                    End With
                Catch ex As Exception
                    blnProcessNextStop = False
                    Exit For
                End Try
            Next
            blnRet = blnProcessNextStop
        Catch sqlEx As FaultException
            'Dim e As New FaultExceptionEventArgs(sqlEx, False, Nothing, sqlEx.Reason.ToString, sqlEx.Detail.Message)
            'Dim callback As New SendOrPostCallback(AddressOf RaiseFaultException)
            'requestingContext.Post(callback, e)
            Return False
        Catch timeoutEx As TimeoutException
            'Dim e As New FaultExceptionEventArgs(timeoutEx, False, Nothing, "Timedout", "OperationTimedout")
            'Dim callback As New SendOrPostCallback(AddressOf RaiseTimeOutException)
            'requestingContext.Post(callback, e)
            Return False
        Catch ex As Exception
            Throw
        End Try
        Return blnRet
    End Function


    Public Function StopResequenceExBatch(ByRef oPCMiler As NGLPCMilerBLL,
                                          ByVal BookConsPrefix As String,
                                          ByVal BookCarrierControl As Integer,
                                          ByVal dblBatchID As Double,
                                          ByRef strFailedAddressWarning As String,
                                          Optional ByVal blnKeepStopNumbers As Boolean = False,
                                          Optional ByVal Silent As Boolean = False,
                                          Optional ByVal SortByStopNumber As Boolean = False) As Integer
        Dim oBatch As New DAL.NGLBatchProcessDataProvider(Parameters)
        Dim oBLL As New NGLBookRevenueBLL(Parameters)
        Dim blnProcessingWindowOpen As Boolean = False
        Dim addressErrorsShowCorrectionWindow As Boolean = False
        Dim intRet As Integer = 0
        Dim blnSuccess As Boolean = False
        Try
            Dim sPCMErrors As New List(Of String)
            If String.IsNullOrEmpty(BookConsPrefix) OrElse BookConsPrefix.Trim.Length < 1 Then
                Return intRet
            End If
            Dim oPCMReturn As PCM.clsPCMReturnEx = oPCMiler.PCMReSyncMultiStop(BookConsPrefix, dblBatchID, sPCMErrors, blnKeepStopNumbers, Silent, SortByStopNumber)
            If oPCMReturn Is Nothing Then Return intRet
            Dim oFMStops As List(Of PCM.clsFMStopData) = TryCast(oPCMReturn.Results, List(Of PCM.clsFMStopData))
            If oFMStops Is Nothing Then Return intRet
            strFailedAddressWarning = oPCMReturn.FailedAddressMessage
            If oFMStops.Count < 1 Then Return intRet
            If UpdateBookConsMultiPickPCMilerTwoWay(oBatch, oFMStops) Then
                If Not blnKeepStopNumbers Then oBatch.UpdateBookConsPickNumber(BookConsPrefix, True) 'update the pick numbers
                If BookCarrierControl > 0 Then oBLL.AssignOrUpdateCarrier(oFMStops(0).BookControl) 'Check for carrier and re-calculate costs
                If oPCMReturn.BadAddressCount > 0 Then intRet = oPCMReturn.BadAddressCount * -1 Else intRet = oFMStops.Count
            End If
        Catch ex As Exception
            Throw
        End Try
        Return intRet
    End Function

#End Region

#Region " Public Methods "
    ''' <summary>
    ''' Run the NGL Optimizer
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <remarks>
    ''' Modified by RHR for v-8.5.4.004 added skip, take, and recordcount rules
    ''' </remarks> 
    Public Sub RunOptimizer365(ByVal filters As Models.AllFilters)
        Dim oDAL As New DAL.NGLBookLoadBoard(Parameters)
        Dim RecordCount = 500
        filters.skip = 0
        filters.take = 500
        Dim LBGridData = oDAL.GetBookLoadBoards(filters, RecordCount)
        If LBGridData?.Count() > 0 Then
            'We can only optimize orders that are in N or P status
            'TODO - NOTE: talk to Rob and Ari and Bill about trans codes
            Dim recordsNP = LBGridData.Where(Function(x) x.BookTranCode.Trim().ToUpper() = "N" OrElse x.BookTranCode.Trim().ToUpper() = "P").ToArray()
            'add a check - if more than 200 records and return a warning and stop (this warning should disappear after a bit)
            Dim compControls = (From t In recordsNP Select t.BookCustCompControl Distinct).ToArray()
            'the optimizer has to be filtered by company so do this for each distinct company we have in the results set of the LoadBoardGrid
            For Each CompControl In compControls
                If IsNGLOptOn(CompControl) Then
                    Dim ltsByComp = recordsNP.Where(Function(x) x.BookCustCompControl = CompControl).ToArray() 'filter the results by company
                    Dim strCompName = ltsByComp(0).CompName
                    If Not ltsByComp Is Nothing Then
                        'TODO Have to group the company groups by Inbound/Outbound
                        'INBOUND
                        Dim ltsCompsInbound = ltsByComp.Where(Function(x) x.LaneOriginAddressUse = 1).ToArray()
                        If Not ltsCompsInbound Is Nothing AndAlso ltsCompsInbound.Length > 0 Then
                            Dim strInboundCtrls As String = ""
                            Dim sep = ""
                            For Each i In ltsCompsInbound
                                strInboundCtrls += (sep + i.BookControl.ToString()) 'get a list of the BookControls
                                sep = ", "
                            Next
                            Dim strBkInboundFilter = String.Format("BookControl in ({0})", strInboundCtrls) 'Create the filter to be passed in to the stored procedures
                            Dim optDataFltr As New OptimizerDataFilters() With {.CompName = strCompName, .Inbound = True}
                            ExecNGLOptimizer(CompControl, strBkInboundFilter, optDataFltr)
                        End If
                        'OUTBOUND
                        Dim ltsCompsOutbound = ltsByComp.Where(Function(x) x.LaneOriginAddressUse = 0).ToArray()
                        If Not ltsCompsOutbound Is Nothing AndAlso ltsCompsOutbound.Length > 0 Then
                            Dim strOutboundCtrls As String = ""
                            Dim sep = ""
                            For Each o In ltsCompsOutbound
                                strOutboundCtrls += (sep + o.BookControl.ToString()) 'get a list of the BookControls
                                sep = ", "
                            Next
                            Dim strBkOutboundFilter = String.Format("BookControl in ({0})", strOutboundCtrls) 'Create the filter to be passed in to the stored procedures
                            Dim optDataFltr As New OptimizerDataFilters() With {.CompName = strCompName, .Inbound = False}
                            ExecNGLOptimizer(CompControl, strBkOutboundFilter, optDataFltr)
                        End If
                    End If
                Else
                    ' fetcher = New ProcessDataDelegate(AddressOf Me.ExecOptimizerAsync)
                End If
            Next
        End If
    End Sub


    ''' <summary>
    ''' Run the NGL Optimizer
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <remarks>
    ''' Modified by RHR for v-8.5.4.004 added skip, take, and recordcount rules
    ''' </remarks>
    Public Sub RunOptimizer365Async(ByVal filters As Models.AllFilters)
        Dim oDAL As New DAL.NGLBookLoadBoard(Parameters)
        Dim RecordCount = 500
        filters.skip = 0
        filters.take = 500
        Dim LBGridData = oDAL.GetBookLoadBoards(filters, RecordCount)
        If LBGridData?.Count() > 0 Then
            'We can only optimize orders that are in N or P status
            'TODO - NOTE: talk to Rob and Ari and Bill about trans codes
            Dim recordsNP = LBGridData.Where(Function(x) x.BookTranCode.Trim().ToUpper() = "N" OrElse x.BookTranCode.Trim().ToUpper() = "P").ToArray()
            'add a check - if more than 200 records and return a warning and stop (this warning should disappear after a bit)
            Dim compControls = (From t In recordsNP Select t.BookCustCompControl Distinct).ToArray()
            'the optimizer has to be filtered by company so do this for each distinct company we have in the results set of the LoadBoardGrid
            For Each CompControl In compControls
                If IsNGLOptOn(CompControl) Then
                    Dim ltsByComp = recordsNP.Where(Function(x) x.BookCustCompControl = CompControl).ToArray() 'filter the results by company
                    Dim strCompName = ltsByComp(0).CompName
                    If Not ltsByComp Is Nothing Then
                        'TODO Have to group the company groups by Inbound/Outbound
                        'INBOUND
                        Dim ltsCompsInbound = ltsByComp.Where(Function(x) x.LaneOriginAddressUse = 1).ToArray()
                        If Not ltsCompsInbound Is Nothing AndAlso ltsCompsInbound.Length > 0 Then
                            Dim strInboundCtrls As String = ""
                            Dim sep = ""
                            For Each i In ltsCompsInbound
                                strInboundCtrls += (sep + i.BookControl.ToString()) 'get a list of the BookControls
                                sep = ", "
                            Next
                            Dim strBkInboundFilter = String.Format("BookControl in ({0})", strInboundCtrls) 'Create the filter to be passed in to the stored procedures
                            Dim optDataFltr As New OptimizerDataFilters() With {.CompName = strCompName, .Inbound = True}
                            Dim fetcher As New ProcessDataDelegate(AddressOf Me.ExecNGLOptimizer)
                            fetcher.BeginInvoke(CompControl, strBkInboundFilter, optDataFltr, Nothing, Nothing) 'Launch thread
                        End If
                        'OUTBOUND
                        Dim ltsCompsOutbound = ltsByComp.Where(Function(x) x.LaneOriginAddressUse = 0).ToArray()
                        If Not ltsCompsOutbound Is Nothing AndAlso ltsCompsOutbound.Length > 0 Then
                            Dim strOutboundCtrls As String = ""
                            Dim sep = ""
                            For Each o In ltsCompsOutbound
                                strOutboundCtrls += (sep + o.BookControl.ToString()) 'get a list of the BookControls
                                sep = ", "
                            Next
                            Dim strBkOutboundFilter = String.Format("BookControl in ({0})", strOutboundCtrls) 'Create the filter to be passed in to the stored procedures
                            Dim optDataFltr As New OptimizerDataFilters() With {.CompName = strCompName, .Inbound = False}
                            Dim fetcher As New ProcessDataDelegate(AddressOf Me.ExecNGLOptimizer)
                            fetcher.BeginInvoke(CompControl, strBkOutboundFilter, optDataFltr, Nothing, Nothing) 'Launch thread
                        End If
                    End If
                Else
                    ' fetcher = New ProcessDataDelegate(AddressOf Me.ExecOptimizerAsync)
                End If
            Next
        End If
    End Sub



    'Public Function DeletePreviousSolution() As Boolean
    '    Dim blnRet As Boolean = False
    '    Try
    '        If File.Exists(TSSolutionFile) Then File.Delete(TSSolutionFile)
    '        blnRet = True
    '    Catch ex As ArgumentException
    '        ProcessesOptimizationException(ex, ex.Message, "E_TSDeletePreviousSolutionFailed")
    '    Catch ex As UnauthorizedAccessException
    '        ProcessesOptimizationException(ex, ex.Message, "E_TSDeletePreviousSolutionFailed")
    '    Catch ex As System.IO.IOException
    '        ProcessesOptimizationException(ex, ex.Message, "E_TSDeletePreviousSolutionFailed")
    '    Catch ex As Exception
    '        Throw
    '    End Try
    '    Return blnRet
    'End Function

    'Public Function DeletePreviousNGLOptSolution() As Boolean
    '    Dim blnRet As Boolean = False
    '    Try
    '        If File.Exists(NGLOptResultFile) Then File.Delete(NGLOptResultFile)
    '        blnRet = True
    '    Catch ex As ArgumentException
    '        ProcessesOptimizationException(ex, ex.Message, "E_NGLOptDeletePreviousSolutionFailed")
    '    Catch ex As UnauthorizedAccessException
    '        ProcessesOptimizationException(ex, ex.Message, "E_NGLOptDeletePreviousSolutionFailed")
    '    Catch ex As System.IO.IOException
    '        ProcessesOptimizationException(ex, ex.Message, "E_NGLOptDeletePreviousSolutionFailed")
    '    Catch ex As Exception
    '        Throw
    '    End Try
    '    Return blnRet
    'End Function

    Public Function LoadNGLOptParameterSettings(ByVal CompControl As Integer, ByVal optDataFltr As OptimizerDataFilters) As Boolean
        Dim blnRet As Boolean = False
        Dim strExDetails = "Unable to load the optimization parameter settings."
        Try
            Dim oLocalize As New DAL.NGLcmLocalizeKeyValuePairData(Parameters)
            strExDetails = oLocalize.GetLocalizedValueByKey("E_NGLOptLoadParameterSettingsFailed", "Unable to load the optimization parameter settings.")
            NGLOptimizerDataPath = getParText("NGLOptimizerDataPath", CompControl)
            NGLOptimizerDefMaxCubes = getParValue("NGLOptimizerDefMaxCubes", CompControl)
            NGLOptimizerDefMaxPallets = getParValue("NGLOptimizerDefMaxPallets", CompControl)
            NGLOptimizerDefMaxWeight = getParValue("NGLOptimizerDefMaxWeight", CompControl)
            NGLOptimizerDefMaxCases = getParValue("NGLOptimizerDefMaxCases", CompControl)
            NGLOptimizerMaxStopsPerTruck = getParValue("NGLOptimizerMaxStopsPerTruck", CompControl)
            NGLOptimizerDistanceFileName = getParText("NGLOptimizerDistanceFileName", CompControl)
            NGLOptimizerStopFileName = getParText("NGLOptimizerStopFileName", CompControl)
            NGLOptimizerParameterFileName = getParText("NGLOptimizerParameterFileName", CompControl)
            NGLOptimizerResultFileName = getParText("NGLOptimizerResultFileName", CompControl)
            NGLOptimizerTimeOut = getParValue("NGLOptimizerTimeOut", CompControl)
            blnRet = True
        Catch ex As ArgumentException
            ProcessesOptimizationException(ex, ex.Message, strExDetails, optDataFltr)
        Catch ex As UnauthorizedAccessException
            ProcessesOptimizationException(ex, ex.Message, strExDetails, optDataFltr)
        Catch ex As System.IO.IOException
            ProcessesOptimizationException(ex, ex.Message, strExDetails, optDataFltr)
        Catch ex As Exception
            Throw
        End Try
        Return blnRet
    End Function

    ''' <summary>
    ''' Reads the NGLOptimizerOn for the provided company and updates the local property value. Returns true if On
    ''' </summary>
    ''' <param name="CompControl"></param>
    ''' <returns>Returns true if on</returns>
    Public Function IsNGLOptOn(Optional ByVal CompControl As Integer = 0) As Boolean
        Dim blnRet As Boolean = False
        NGLOptimizerOn = getParValue("NGLOptimizerOn", CompControl)
        If NGLOptimizerOn = 1 Then blnRet = True
        Return blnRet
    End Function

#End Region

#Region " Private Async Methods"

    'Protected Sub ExecOptimizerAsync(ByVal criteria As TSOptimizationCriteria, ByVal WCFProperties As FMDataProperties, ByVal PaneSettings As PaneSettings)
    'Dim steps As Integer = 12
    'Dim curStep As Integer = 0
    'Dim statusMsg As String = "'"
    'Me.PaneSettings = PaneSettings
    'Me.WCFProperties = WCFProperties
    'Try
    '    'Log Optimization
    '    curStep += 1 : ProcessesOptimizationStatusUpdate("StatusMSGOptUpdateLog", curStep, steps)
    '    'TO DO:  add code to save to the optimization log

    '    'delete any existing solution file
    '    curStep += 1 : ProcessesOptimizationStatusUpdate("StatusMSGOptDeletePrevious", curStep, steps)
    '    If Not DeletePreviousSolution() Then
    '        Me.ProcessesOptimizationComplete(Nothing, "E_OptFailDeletePrev", "", 0, 0)
    '        Return
    '    End If


    '    'get the company optimization settings
    '    curStep += 1 : ProcessesOptimizationStatusUpdate("StatusMSGOptReadCompParameterSettings", curStep, steps)
    '    Dim optcompSettings As OptimizerCompSettings = _FMCompData.GetOptimizerCompSettingsByNumber(criteria.DefCompNumber, WCFProperties, PaneSettings)
    '    If optcompSettings Is Nothing Then
    '        Me.ProcessesOptimizationComplete(Nothing, "E_OptFailGetCompSettings", "", 0, 0)
    '        Return
    '    End If

    '    'update the parameter file with correct company information
    '    curStep += 1 : ProcessesOptimizationStatusUpdate("StatusMSGOptUpdateParameterFile", curStep, steps)
    '    If Not (New clsTSParameter(TSDataPath, "NGL.PAR", optcompSettings)).writeFile Then
    '        Me.ProcessesOptimizationComplete(Nothing, "E_OptFailWritePAR", "", 0, 0)
    '        Return
    '    End If

    '    'get the stop data
    '    curStep += 1 : ProcessesOptimizationStatusUpdate("StatusMSGOptReadStopData", curStep, steps)
    '    Dim vOptStops As ChangeTrackingCollection(Of NGLWCFClient.NGLBookData.vOptimizationStop) = _FMBookData.GetvOptimizationStopData(criteria.Filters, WCFProperties, PaneSettings)
    '    If vOptStops Is Nothing OrElse vOptStops.Count < 1 Then
    '        ProcessesOptimizationException(Nothing, "E_NoData", "MSGOptNoStops")
    '        Return
    '    End If

    '    'Copy the code from ExecNGLOptimizerAsync here
    '    Dim counter As Integer = 1
    '    Dim total As Integer = vOptStops.Count
    '    Dim vOptStopsNoSHID As New ChangeTrackingCollection(Of NGLWCFClient.NGLBookData.vOptimizationStop)
    '    Dim vOptStopsHadSHID As New ChangeTrackingCollection(Of NGLWCFClient.NGLBookData.vOptimizationStop)
    '    For Each i As NGLWCFClient.NGLBookData.vOptimizationStop In vOptStops
    '        If Not i.BookControl = 0 Then
    '            If String.IsNullOrEmpty(i.BookSHID) Then
    '                vOptStopsNoSHID.Add(i)
    '                'Create new status update message indicating that the system is resetting the load to N status
    '                ProcessesOptimizationStatusUpdate("StatusMSGOptResetToNStatus", counter, total)
    '                'Modified by RHR 7/15/14 newTranCode and BookTranCode were backward
    '                Dim newTranCodeResult = _FMBookData.ProcessNewTranCode(i.BookControl,
    '                                                         "N",
    '                                                         i.BookTranCode,
    '                                                         WCFProperties,
    '                                                         0)
    '                If newTranCodeResult.Success = False Then
    '                    'we were unable to optimize the data because we could not reset all the records to N status
    '                    ProcessesOptimizationException(Nothing, "E_NGLRunOptimizationFailed", "MSGOptNoResetToN")
    '                    Return
    '                End If
    '                counter = counter + 1
    '            Else
    '                vOptStopsHadSHID.Add(i)
    '            End If
    '        End If
    '    Next


    '    Dim oStopFile As New clsStopDataFile(TSDataPath, "NGL.SDF")
    '    curStep += 1 : ProcessesOptimizationStatusUpdate("StatusMSGOptWritingStopDataFile", curStep, steps)
    '    If Not generateStopFile(vOptStopsNoSHID, oStopFile) Then
    '        Me.ProcessesOptimizationComplete(Nothing, "E_OptFailDeletePrev", "", 0, 0)
    '        Return
    '    End If


    '    curStep += 1 : ProcessesOptimizationStatusUpdate("StatusMSGOptWritingTruckDataFile", curStep, steps)
    '    If Not generateTruckFile(oStopFile) Then
    '        ProcessesOptimizationException(Nothing, "E_NoData", "MSGOptNoTrucks")
    '        Return
    '    End If

    '    curStep += 1 : ProcessesOptimizationStatusUpdate("StatusMSGOptRunningOptimizer", curStep, steps)
    '    bSetUNCPath(TSAppPath)
    '    Dim startInfo As New ProcessStartInfo("tsw.exe", "CmndLine.ini")
    '    startInfo.WindowStyle = ProcessWindowStyle.Minimized
    '    Process.Start(startInfo)
    '    Dim blnNotFound As Boolean = True
    '    Dim intTry As Integer = 0
    '    Do
    '        intTry += 1 : ProcessesOptimizationStatusUpdate("StatusMSGOptWaitingForSolution", intTry, Me.TSTimeOut)
    '        If File.Exists(TSSolutionFile) Then
    '            blnNotFound = False
    '        End If
    '        If intTry > Me.TSTimeOut Then
    '            ProcessesTimeOutException(Nothing, "E_TSOptimizationTimeOutExceeded", "")
    '            Return
    '        End If
    '        System.Threading.Thread.Sleep(100)
    '    Loop While blnNotFound

    '    curStep += 1 : ProcessesOptimizationStatusUpdate("StatusMSGOptReadingSolution", curStep, steps)
    '    Dim objAllStops As NGL.FreightMaster.PCMiler.clsAllStops = readAllStops(criteria.CompControl)
    '    If objAllStops Is Nothing OrElse objAllStops.COUNT < 1 Then
    '        Me.ProcessesOptimizationComplete(Nothing, "E_OptFailReadSolution", "", 0, 0)
    '        Return
    '    End If

    '    curStep += 1 : ProcessesOptimizationStatusUpdate("StatusMSGOptSavingResultsToDB", curStep, steps)
    '    If Not UpdateTruckStopData(objAllStops) Then
    '        Me.ProcessesOptimizationComplete(Nothing, "E_OptFailSaveSolution", "", 0, 0)
    '        Return
    '    End If

    '    curStep += 1 : ProcessesOptimizationStatusUpdate("StatusMSGOptUpdatingStreetLevelInformation", curStep, steps)
    '    If Not ResyncStops(objAllStops) Then
    '        Me.ProcessesOptimizationComplete(Nothing, "E_OptFailResyncStops", objAllStops.FailedAddressMessage, objAllStops.BadAddressCount, objAllStops.BatchID)
    '        Return
    '    End If

    '    curStep += 1 : ProcessesOptimizationStatusUpdate("StatusMSGOptUpdatingBookingCarrierData", curStep, steps)

    '    Dim MsgDictList As New List(Of NGLWCFClient.NGLBookData.CarrierCostResults)
    '    MsgDictList = updateBookingCarrier(objAllStops)

    '    Me.ProcessesOptimizationComplete(MsgDictList, objAllStops.FailedAddressMessage, objAllStops.BadAddressCount, objAllStops.BatchID, vOptStopsHadSHID, Nothing)

    '    'curStep += 1 : ProcessesOptimizationStatusUpdate("StatusMSGOptSolutionGenerated", curStep, steps)

    'Catch ex As System.ComponentModel.Win32Exception
    '    ProcessesOptimizationException(ex, "E_TSRunOptimizationFailed", ex.Message)
    'Catch ex As ObjectDisposedException
    '    ProcessesOptimizationException(ex, "E_TSRunOptimizationFailed", ex.Message)
    'Catch ex As FileNotFoundException
    '    ProcessesOptimizationException(ex, "E_TSRunOptimizationFailed", ex.Message)
    'Catch ex As InvalidOperationException
    '    ProcessesOptimizationException(ex.InnerException, ex.Message, "")
    'Catch ex As Exception
    '    Me.ProcessesOptimizationComplete(ex, "E_UnExpected", "", 0, 0)
    'Finally
    '    requestingContext = Nothing
    'End Try
    'End Sub


#End Region

#Region " Private Methods"

    ''' <summary>
    ''' Does the real work
    ''' </summary>
    ''' <param name="CompControl"></param>
    ''' <param name="Filter"></param>
    ''' <param name="optDataFltr"></param>
    Protected Sub ExecNGLOptimizer(ByVal CompControl As Integer, ByVal Filter As String, ByVal optDataFltr As OptimizerDataFilters)
        Dim oPCMiler As New NGLPCMilerBLL(Parameters)
        Dim steps As Integer = 12
        Dim curStep As Integer = 0
        Dim statusMsg As String = "'"
        Dim strE_NGLRunOptimizationFailed = "Run Optimization Failed"
        Try
            Dim oLocalize As New DAL.NGLcmLocalizeKeyValuePairData(Parameters)
            Dim strE_NoData = oLocalize.GetLocalizedValueByKey("E_NoData", "No data matches your selected criteria. Please modify your filter(s) and try again.")
            Dim strMSGOptNoStops = oLocalize.GetLocalizedValueByKey("MSGOptNoStops", "The optimization has failed because no stops were available. Please verify that records have been properly selected.")
            Dim strE_OptFailCreateStopDataFile = oLocalize.GetLocalizedValueByKey("E_OptFailCreateStopDataFile", "Failed to create the optimization stop data file.")
            strE_NGLRunOptimizationFailed = oLocalize.GetLocalizedValueByKey("E_NGLRunOptimizationFailed", "Run Optimization Failed")

            'Log Optimization
            curStep += 1 : ProcessOptimizationStatusUpdateKey("StatusMSGOptUpdateLog", "Updating the optimization activity log step", curStep, steps, 0, True, optDataFltr) 'Modified By LVV on 9/16/20 for v-8.3.0.001 - Optimizer 365

            'TO DO:  add code to save to the optimization log

            'get the company optimization settings
            curStep += 1 : ProcessOptimizationStatusUpdateKey("StatusMSGOptReadCompParameterSettings", "Reading optimization parameter settings for company step", curStep, steps, 0, True, optDataFltr) 'Modified By LVV on 9/16/20 for v-8.3.0.001 - Optimizer 365

            Dim oOptCompSet As New DAL.NGLOptimizerCompSettingsData(Parameters)
            Dim optcompSettings As DTO.OptimizerCompSettings = oOptCompSet.GetOptimizerCompSettingsFiltered(CompControl)
            If optcompSettings Is Nothing Then
                ProcessesOptimizationComplete(Nothing, "E_OptFailGetCompSettings", "", 0, 0, optDataFltr)
                Return
            ElseIf Not Me.LoadNGLOptParameterSettings(CompControl, optDataFltr) Then
                ProcessesOptimizationComplete(Nothing, "E_OptFailGetCompSettings", "", 0, 0, optDataFltr)
                Return
            End If

            'get the stop data
            curStep += 1 : ProcessOptimizationStatusUpdateKey("StatusMSGOptReadStopData", "Reading optimization stop data information step", curStep, steps, 0, True, optDataFltr) 'Modified By LVV on 9/16/20 for v-8.3.0.001 - Optimizer 365

            Dim vOptStopsNoSHID() As DTO.vOptimizationStop 'ChangeTrackingCollection(Of NGLWCFClient.NGLBookData.vOptimizationStop)
            Dim vOptStopsHadSHID() As DTO.vOptimizationStop 'New ChangeTrackingCollection(Of NGLWCFClient.NGLBookData.vOptimizationStop)

            vOptStopsNoSHID = NGLBookData.ResetToNStatusSP(Parameters.UserName, Filter, CompControl)
            If vOptStopsNoSHID Is Nothing OrElse vOptStopsNoSHID.Count < 1 Then
                ProcessesOptimizationException(Nothing, strE_NoData, strMSGOptNoStops, optDataFltr)
                Return
            End If

            vOptStopsHadSHID = NGLBookData.ResetToNStatusSPOptHasSHID(Parameters.UserName, Filter, CompControl)
            'If vOptStopsHadSHID Is Nothing OrElse vOptStopsHadSHID.Count < 1 Then
            '    ProcessesOptimizationException(Nothing, "E_NoData", "MSGOptNoStops")
            '    Return
            'End If

            Dim oStopFile As New clsNGLOptStopData(NGLOptimizerDataPath, NGLOptimizerStopFileName, NGLOptimizerDefMaxCases, NGLOptimizerDefMaxCubes, NGLOptimizerDefMaxPallets, NGLOptimizerDefMaxWeight, oPCMiler)
            Dim oNGLOptStops As New List(Of NGLOptStop)
            curStep += 1 : ProcessOptimizationStatusUpdateKey("StatusMSGOptWritingStopDataFile", "Preparing to write optimization stop data to file step", curStep, steps, 0, False, optDataFltr) 'Modified By LVV on 9/16/20 for v-8.3.0.001 - Optimizer 365
            If Not generateNGLOptStopData(oPCMiler, vOptStopsNoSHID, oStopFile, optcompSettings, oNGLOptStops, curStep, optDataFltr) Then
                Me.ProcessesOptimizationComplete(Nothing, "E_OptFailCreateStopDataFile", "", 0, 0, optDataFltr)
                Return
            End If
            Dim oNGLOptPar As New NGLOptPar()
            'update the parameter file 
            curStep += 1 : ProcessOptimizationStatusUpdateKey("StatusMSGOptUpdateParameterFile", "Updating optimization parameter file step", curStep, steps, 0, True, optDataFltr) 'Modified By LVV on 9/16/20 for v-8.3.0.001 - Optimizer 365
            If oStopFile.Items Is Nothing OrElse oStopFile.Items.Count < 2 Then
                If oStopFile.BadStops.Count > 0 Then
                    Dim strBadStopsMsg As String = oStopFile.BadStops.Count.ToString & " Stops could not be loaded. "
                    Dim strSpacer = ""
                    For Each s In oStopFile.BadStops
                        strBadStopsMsg &= strSpacer & "Order # " & s.OrderNumber & ", Pro # " & s.ProNumber & ", " & s.Message
                        strSpacer = vbCrLf '"; "
                    Next
                    ProcessesOptimizationException(Nothing, strE_OptFailCreateStopDataFile, strBadStopsMsg, optDataFltr)
                Else
                    ProcessesOptimizationException(Nothing, strE_NoData, strMSGOptNoStops, optDataFltr)
                End If
                Return
            Else
                With oNGLOptPar
                    .No_Stops = oStopFile.Items.Count - 1
                    .Max_Stops = NGLOptimizerMaxStopsPerTruck
                    .Col_SDF = 9
                    .Max_Cases = NGLOptimizerDefMaxCases
                    .Max_Weight = NGLOptimizerDefMaxWeight
                    .Max_Pallets = NGLOptimizerDefMaxPallets
                    .Max_Cubes = NGLOptimizerDefMaxCubes
                End With
            End If

            Dim oNGLOptDist As Integer(,)
            curStep += 1 : ProcessOptimizationStatusUpdateKey("StatusMSGOptWritingDistanceFile", "Saving optimization distance matrix to file.", curStep, steps, 0, False, optDataFltr) 'Modified By LVV on 9/16/20 for v-8.3.0.001 - Optimizer 365
            If Not buildNGLOptDistanceData(oStopFile, curStep, optDataFltr) Then
                ProcessesOptimizationComplete(Nothing, "E_OptFailCreateDistanceFile", "", 0, 0, optDataFltr)
                Return
            Else
                oNGLOptDist = oStopFile.writeDistanceToArray()
            End If

            curStep += 1 : ProcessOptimizationStatusUpdateKey("StatusMSGOptRunningOptimizer", "Running optimizer step", curStep, steps, 0, True, optDataFltr) 'Modified By LVV on 11/3/20 for v-8.3.0.002 Task #20201027141251 - Optimizer Migration to 365
            Dim oNGLOptLib As New NGLOptLib
            Dim oNGLOptRet As NGLOptRet = oNGLOptLib.processData(oNGLOptPar, oNGLOptStops, oNGLOptDist)

            'Check for errors or warnings
            curStep += 1 : ProcessOptimizationStatusUpdateKey("StatusMSGOptReadingSolution", "Reading optimized solution step", curStep, steps, 0, True, optDataFltr) 'Modified By LVV on 11/3/20 for v-8.3.0.002 Task #20201027141251 - Optimizer Migration to 365
            If oNGLOptRet Is Nothing Then
                ProcessesOptimizationComplete(Nothing, "E_OptFailReadSolution", "", 0, 0, optDataFltr)
                Return
            End If
            Dim Messages As New StringBuilder()
            If Not oNGLOptRet.Errors Is Nothing AndAlso oNGLOptRet.Errors.Count > 0 Then
                Messages.Append("******** ERRORS **********" & vbCrLf)
                For Each e In oNGLOptRet.Errors
                    Messages.Append(e & vbCrLf)
                Next
            End If
            If Not oNGLOptRet.Warnings Is Nothing AndAlso oNGLOptRet.Warnings.Count > 0 Then
                Messages.Append("******** Warnings **********" & vbCrLf)
                For Each w In oNGLOptRet.Warnings
                    Messages.Append(w & vbCrLf)
                Next
            End If
            'TODO: add new message logic to show errors and warnings returned from the optimizer.
            'for now this is removed because it stops the optmizer and does not show the message to the user
            If Messages.Length > 0 Then
                ProcessesOptimizationComplete(Nothing, "E_OptFailReadSolution", Messages.ToString, 0, 0, optDataFltr)
                Return
            End If

            Dim objAllStops As PCM.clsAllStops = processNGLOptResults(CompControl, oStopFile, oNGLOptRet, optDataFltr)

            If objAllStops Is Nothing OrElse objAllStops.COUNT < 1 Then
                Me.ProcessesOptimizationComplete(Nothing, "E_OptFailReadSolution", "", 0, 0, optDataFltr)
                Return
            End If

            curStep += 1 : ProcessOptimizationStatusUpdateKey("StatusMSGOptSavingResultsToDB", "Saving optimized results to database step", curStep, steps, 0, False, optDataFltr) 'Modified By LVV on 11/3/20 for v-8.3.0.002 Task #20201027141251 - Optimizer Migration to 365
            If Not UpdateTruckStopData(objAllStops, curStep, optDataFltr) Then
                ProcessesOptimizationComplete(Nothing, "E_OptFailSaveSolution", "", 0, 0, optDataFltr)
                Return
            End If

            curStep += 1 : ProcessOptimizationStatusUpdateKey("StatusMSGOptUpdatingStreetLevelInformation", "Running PC Miler to update the street level information for optimized solution step", curStep, steps, 0, False, optDataFltr) 'Modified By LVV on 11/3/20 for v-8.3.0.002 Task #20201027141251 - Optimizer Migration to 365
            If Not ResyncStops(objAllStops, oPCMiler, curStep, optDataFltr) Then
                ProcessesOptimizationComplete(Nothing, "E_OptFailResyncStops", objAllStops.FailedAddressMessage, objAllStops.BadAddressCount, objAllStops.BatchID, optDataFltr)
                Return
            End If

            curStep += 1 : ProcessOptimizationStatusUpdateKey("StatusMSGOptUpdatingBookingCarrierData", "Assigning carriers and allocating costs step", curStep, steps, 0, False, optDataFltr) 'Modified By LVV on 11/3/20 for v-8.3.0.002 Task #20201027141251 - Optimizer Migration to 365
            'If Not updateBookingCarrier(objAllStops) Then
            '    Me.ProcessesOptimizationComplete(Nothing, "E_OptFailUpdateBookCarrier", "", 0, 0)
            '    Return
            'End If
            Dim MsgDictList As New List(Of DTO.CarrierCostResults)
            MsgDictList = updateBookingCarrier(objAllStops, curStep, optDataFltr)

            ProcessesOptimizationComplete(MsgDictList, objAllStops.FailedAddressMessage, objAllStops.BadAddressCount, objAllStops.BatchID, vOptStopsHadSHID, oStopFile.BadStops, optDataFltr)

            'Me.ProcessesOptimizationComplete(Nothing, "", objAllStops.FailedAddressMessage, objAllStops.BadAddressCount, objAllStops.BatchID)

            curStep += 1 : ProcessOptimizationStatusUpdateKey("StatusMSGOptSolutionGenerated", "Success! Your optimized solution has been generated!", curStep, steps, 0, True, optDataFltr) 'Modified By LVV on 11/3/20 for v-8.3.0.002 Task #20201027141251 - Optimizer Migration to 365

        Catch ex As System.ComponentModel.Win32Exception
            ProcessesOptimizationException(ex, strE_NGLRunOptimizationFailed, ex.Message, optDataFltr)
        Catch ex As ObjectDisposedException
            ProcessesOptimizationException(ex, strE_NGLRunOptimizationFailed, ex.Message, optDataFltr)
        Catch ex As FileNotFoundException
            ProcessesOptimizationException(ex, strE_NGLRunOptimizationFailed, ex.Message, optDataFltr)
        Catch ex As InvalidOperationException
            ProcessesOptimizationException(ex.InnerException, ex.Message, "", optDataFltr)
        Catch ex As Exception
            ProcessesOptimizationComplete(ex, "E_UnExpected", "", 0, 0, optDataFltr)
        End Try
    End Sub


    Public Function getParValue(ByVal ParKey As String, Optional ByVal CompControl As Integer = 0, Optional ByVal defaultval As Double = 0) As Double
        Dim dblRet As Double = defaultval
        Try
            Dim opar = NGLParameterData.GetParameter(ParKey)
            If Not opar Is Nothing Then
                If CompControl = 0 OrElse opar.ParIsGlobal = True Then
                    dblRet = opar.ParValue 'no company number just return the main result
                Else
                    dblRet = NGLCompParameterData.GetParValue(ParKey, CompControl) 'look up the value in the compParameter list
                End If
            End If
        Catch ex As Exception
            'ignore errors just return default
        End Try
        Return dblRet
    End Function

    Public Function getParText(ByVal ParKey As String, Optional ByVal CompControl As Integer = 0, Optional ByVal defaultval As String = "") As String
        Dim strRet As String = defaultval
        Try
            Dim opar = NGLParameterData.GetParameter(ParKey)
            If Not opar Is Nothing Then
                If CompControl = 0 OrElse opar.ParIsGlobal = True Then
                    strRet = opar.ParText 'no company number just return the main result
                Else
                    strRet = NGLCompParameterData.GetParText(ParKey, CompControl) 'look up the value in the compParameter list
                End If
            End If
        Catch ex As Exception
            'ignore errors just return default
        End Try
        Return strRet
    End Function


    'Private Function generateStopFile(ByRef vOptStops As ChangeTrackingCollection(Of NGLWCFClient.NGLBookData.vOptimizationStop), ByRef oStopFile As clsStopDataFile) As Boolean
    '    Dim intStop As Integer = 0
    '    For Each item In vOptStops
    '        intStop += 1 : ProcessesOptimizationStatusUpdate("StatusMSGOptProcessStopData", intStop, vOptStops.Count)
    '        'get the lane stop data settings
    '        Dim LaneOptSettings As NGLLaneData.LaneOptimizationSDFSettings = _FMLaneData.GetLaneOptimizationSDFSettingsByControl(item.BookODControl, WCFProperties, PaneSettings)
    '        If LaneOptSettings Is Nothing Then Return False 'we cannot continue messages are passed via events
    '        Dim oStop As clsStopRecord = oStopFile.Add(item, LaneOptSettings)
    '        oStop.BookControl = item.BookControl
    '        oStop.ClearCNS = 1
    '        oStop.IndexOf = intStop
    '        oStop.TotalOf = vOptStops.Count
    '    Next
    '    intStop = 0
    '    'This logic has been replaced by ProcessNewTransCode logic
    '    'For Each oStop As clsStopRecord In oStopFile.mcolStops
    '    '    intStop += 1 : ProcessesOptimizationStatusUpdate("StatusMSGOptClearBookingCarrierData", intStop, vOptStops.Count)
    '    '    If Not _FMBookData.ClearCarrierCons(oStop.BookControl, oStop.ClearCNS, WCFProperties, PaneSettings) Then Return False 'we cannot continue messages are passed via events
    '    'Next
    '    _TSStops = vOptStops.Count
    '    Return oStopFile.writeFile()
    'End Function

    'Private Function generateNGLOptStopFile(ByRef vOptStops As ChangeTrackingCollection(Of NGLWCFClient.NGLBookData.vOptimizationStop),
    '                                        ByRef oStopFile As clsNGLOptStopData,
    '                                        ByRef optcompSettings As OptimizerCompSettings) As Boolean
    '    Dim intStop As Integer = 0
    '    Dim DistanceMsg As String = ""
    '    With oStopFile
    '        .Items = New List(Of clsNGLOptStopItem)
    '        .BadStops = New List(Of clsNGLOptStopItem)
    '        Dim intSeq As Integer = 1
    '        DistanceMsg = "Validating Address For Stop: 1"
    '        ProcessesOptimizationStatusUpdate("StatusMSGOptProcessStopData", intSeq, vOptStops.Count)
    '        DistanceMsg = .addStop(New clsNGLOptStopItem(intSeq, 0, "1", PaneSettings.MainInterface.zipClean(optcompSettings.CompStreetZip), 0, 0, 0, 0, 0, 999), intSeq)
    '        For Each item In vOptStops
    '            ProcessesOptimizationStatusUpdate("StatusMSGOptProcessStopData", intSeq, vOptStops.Count)
    '            'CHANGE LVV 8/7/15
    '            DistanceMsg = .addStop(New clsNGLOptStopItem(intSeq, item.BookCarrOrderNumber, item.BookProNumber, item.BookControl, item.BookLoadControl, PaneSettings.MainInterface.zipClean(item.BookDestZip), item.BookLoadCaseQty, item.BookLoadWgt, item.BookLoadPL, item.BookLoadCube, 1, 1, item.BookDestAddress1, item.BookDestCity, item.BookDestState), intSeq)
    '        Next
    '        intStop = 0
    '        'For Each oStop In .Items
    '        '    intStop += 1 : ProcessesOptimizationStatusUpdate("StatusMSGOptClearBookingCarrierData", intStop, .Items.Count)
    '        '    If oStop.BookControl <> 0 Then
    '        '        If Not _FMBookData.ClearCarrierCons(oStop.BookControl, True, WCFProperties, PaneSettings) Then Return False 'we cannot continue messages are passed via events
    '        '    End If
    '        'Next
    '    End With
    '    _TSStops = vOptStops.Count
    '    Return oStopFile.writeFile()
    'End Function

    Private Function zipClean(ByVal strCode As String) As String
        Dim intPos As Integer
        strCode = Trim(stripQuotes(strCode))
        intPos = InStr(1, strCode, "-")
        If intPos Then
            strCode = strCode.Substring(0, intPos - 1)
            If Len(strCode) > 5 Then
                strCode = strCode.Substring(0, 5)
            End If
        End If
        zipClean = strCode
    End Function

    Private Function stripQuotes(ByVal strval As String) As String
        Dim strNewVal As String
        On Error Resume Next
        If strval.Substring(0, 1) = "'" Then
            strNewVal = Mid(strval, 2)
        Else
            strNewVal = strval
        End If
        If Right(strNewVal, 1) = "'" Then
            strNewVal = strNewVal.Substring(0, Len(strNewVal) - 1)
        End If
        stripQuotes = strNewVal
    End Function


    Private Function generateNGLOptStopData(ByRef oPCMiler As NGLPCMilerBLL,
                                            ByRef vOptStops() As DTO.vOptimizationStop,
                                            ByRef oStopFile As clsNGLOptStopData,
                                            ByRef optcompSettings As DTO.OptimizerCompSettings,
                                            ByRef oNGLOptStops As List(Of NGLOptStop),
                                            ByVal optStep As Integer,
                                            ByVal optDataFltr As OptimizerDataFilters) As Boolean
        Dim intStop As Integer = 0
        Dim DistanceMsg As String = ""
        With oStopFile
            .Items = New List(Of clsNGLOptStopItem)
            .BadStops = New List(Of clsNGLOptStopItem)
            Dim intSeq As Integer = 1
            DistanceMsg = "Validating Address For Stop: 1"
            ProcessOptimizationSubStatusUpdateKey("StatusMSGOptProcessStopData", "Processing  optimization stop data for load ", optStep, intSeq, vOptStops.Count, 0, False, optDataFltr) 'Modified By LVV on 9/16/20 for v-8.3.0.001 - Optimizer 365
            DistanceMsg = .addStop(New clsNGLOptStopItem(oPCMiler, intSeq, 0, "1", zipClean(optcompSettings.CompStreetZip), 0, 0, 0, 0, 0, 999), intSeq)
            Dim blnIsComplete = False
            For Each item In vOptStops
                If intSeq = vOptStops.Length Then blnIsComplete = True
                ProcessOptimizationSubStatusUpdateKey("StatusMSGOptProcessStopData", "Processing  optimization stop data for load ", optStep, intSeq, vOptStops.Count, 1, blnIsComplete, optDataFltr) 'Modified By LVV on 9/16/20 for v-8.3.0.001 - Optimizer 365              
                DistanceMsg = .addStop(New clsNGLOptStopItem(oPCMiler, intSeq, item.BookCarrOrderNumber, item.BookProNumber, item.BookControl, item.BookLoadControl, zipClean(item.BookDestZip), item.BookLoadCaseQty, item.BookLoadWgt, item.BookLoadPL, item.BookLoadCube, 1, 1, item.BookDestAddress1, item.BookDestCity, item.BookDestState), intSeq) 'CHANGE LVV 8/7/15
            Next
            intStop = 0
        End With
        _TSStops = vOptStops.Count
        Return oStopFile.populateStopDataObjectFix(oNGLOptStops)
    End Function

    'Private Function generateNGLOptDistanceFile(ByRef oStopFile As clsNGLOptStopData) As Boolean
    '    If oStopFile Is Nothing OrElse oStopFile.Items.Count < 1 Then Return False
    '    Dim intLoopCount As Integer = 0
    '    Dim intDistanceCount As Integer = oStopFile.Items.Count * oStopFile.Items.Count
    '    For Each s In oStopFile.Items
    '        For Each i In oStopFile.Items
    '            intLoopCount += 1
    '            ProcessesOptimizationStatusUpdate("StatusMSGOptProcessDistanceData", intLoopCount, intDistanceCount)
    '            s.getDistance(i)
    '        Next
    '    Next
    '    Return oStopFile.writeDistanceFile(NGLOptimizerDistanceFileName)
    'End Function

    Public Function buildNGLOptDistanceData(ByVal oStopFile As clsNGLOptStopData, ByVal optStep As Integer, ByVal optDataFltr As OptimizerDataFilters) As Boolean
        If oStopFile Is Nothing OrElse oStopFile.Items.Count < 1 Then Return False
        Dim intLoopCount As Integer = 0
        Dim intDistanceCount As Integer = oStopFile.Items.Count * oStopFile.Items.Count
        Dim chunk As Integer = intDistanceCount / 4 'Modified By LVV on 9/16/20 for v-8.3.0.001 - Optimizer 365
        Try
            For Each s In oStopFile.Items
                For Each i In oStopFile.Items
                    intLoopCount += 1
                    If intLoopCount = chunk Then ProcessOptimizationSubStatusUpdateKey("StatusMSGOptProcessDistanceData", "Calculating optimization distance matrix.", optStep, intLoopCount, intDistanceCount, 1, False, optDataFltr) 'Modified By LVV on 9/16/20 for v-8.3.0.001 - Optimizer 365
                    If intLoopCount = (chunk * 2) Then ProcessOptimizationSubStatusUpdateKey("StatusMSGOptProcessDistanceData", "Calculating optimization distance matrix.", optStep, intLoopCount, intDistanceCount, 2, False, optDataFltr) 'Modified By LVV on 9/16/20 for v-8.3.0.001 - Optimizer 365
                    If intLoopCount = (chunk * 3) Then ProcessOptimizationSubStatusUpdateKey("StatusMSGOptProcessDistanceData", "Calculating optimization distance matrix.", optStep, intLoopCount, intDistanceCount, 3, False, optDataFltr) 'Modified By LVV on 9/16/20 for v-8.3.0.001 - Optimizer 365
                    Dim d As Double = 0
                    Dim sFromZip As String = s.Zip
                    Dim sToZip As String = i.Zip
                    If i.Number <> s.Number Then
                        Try
                            d = Math.Round(NGLBookData.GetOptDist(sFromZip, sToZip), 2)
                        Catch ex As Exception
                            'ignore any errors here except when debugging
                            If Debugger.IsAttached Then Debug.WriteLine("Error Debug Message:" & ex.ToString())
                        End Try
                        If d <= 0 Then
                            'use pc miler
                            Dim oPCMiler As New NGLPCMilerBLL(Parameters)
                            d = Math.Round(oPCMiler.PCMGetFlatMiles(sFromZip, sToZip), 2)
                            Try
                                NGLBookData.UpdateOptDist(sFromZip, sToZip, d)
                            Catch ex As Exception
                                'ignore any errors here except when debugging
                                If Debugger.IsAttached Then Debug.WriteLine("Error Debug Message:" & ex.ToString())
                            End Try
                        End If
                    End If
                    '-1 indicates that this is the same address for both orig and dest so the distance is always the minimum 1  
                    'for now we set this to 1 later we may have the ability to use real numbers so we could set it to 0.001
                    If d = -1 Then d = 1
                    s.Distances.Add(New clsNGLOptStopDistance(i.Number, d))
                Next
            Next
            ProcessOptimizationSubStatusUpdateKey("StatusMSGOptProcessDistanceData", "Calculating optimization distance matrix.", optStep, intLoopCount, intDistanceCount, 4, True, optDataFltr) 'Modified By LVV on 9/16/20 for v-8.3.0.001 - Optimizer 365
        Catch ex As Exception
            'ignore any errors here except when debugging
            If Debugger.IsAttached Then Debug.WriteLine("Error Debug Message:" & ex.ToString())
            'add error handler here?
            Return False
        End Try
        Return True
    End Function

    'Private Function generateTruckFile(ByRef oStopFile As clsStopDataFile) As Boolean
    '    Dim oTruckFile As New clsTruckDataFile(TSDataPath, "NGL.TDF", WorkTimeLimit, DrivingTimeLimit, LayoverPeriod)
    '    Dim intStop As Integer = 0
    '    Dim blnTrucksFound As Boolean = False
    '    For Each item In oStopFile.mcolStops
    '        intStop += 1 : ProcessesOptimizationStatusUpdate("StatusMSGOptBuildTruckList", intStop, _TSStops)
    '        Dim carrOptTruckData As ChangeTrackingCollection(Of NGLWCFClient.NGLCarrierData.CarrierOptimizerTruckData) = _FMCarrierData.GetCarrierOptimizerTruckData(item.BookControl, WCFProperties, PaneSettings)
    '        If Not carrOptTruckData Is Nothing AndAlso carrOptTruckData.Count > 0 Then
    '            blnTrucksFound = True
    '            For Each truck In carrOptTruckData
    '                oTruckFile.Add(truck)
    '            Next
    '        End If
    '    Next
    '    If Not blnTrucksFound Then
    '        Return False
    '    Else
    '        Return oTruckFile.writeFile
    '    End If
    'End Function

    Private Function ResyncStops(ByRef objAllStops As PCM.clsAllStops, ByRef oPCMiler As NGLPCMilerBLL, ByVal optStep As Integer, ByVal optDataFltr As OptimizerDataFilters) As Boolean
        Dim blnRet As Boolean = False
        Dim dblBatchID As Double = CDbl(Format(Now(), "MddyyyyHHmmss"))
        objAllStops.BatchID = dblBatchID
        Dim blnIsComplete = False 'Modified By LVV on 11/3/20 for v-8.3.0.002 Task #20201027141251 - Optimizer Migration to 365
        Dim intStopCT As Integer = 0
        For intStopCT = 1 To objAllStops.COUNT
            If intStopCT = objAllStops.COUNT Then blnIsComplete = True
            ProcessOptimizationSubStatusUpdateKey("StatusMSGOptResyncStop", "Resynchronizing stop", optStep, intStopCT, objAllStops.COUNT, 1, blnIsComplete, optDataFltr) 'Modified By LVV on 11/3/20 for v-8.3.0.002 Task #20201027141251 - Optimizer Migration to 365
            Dim objStop = objAllStops.Item(intStopCT)
            With objStop
                Dim strFailedAddressWarning As String = ""
                Dim intPCMReturn = StopResequenceExBatch(oPCMiler, objStop.ConsNumber, 0, dblBatchID, strFailedAddressWarning, False, True, False)
                If intPCMReturn < 0 Then
                    objAllStops.BadAddressCount += 1
                End If
                If Not String.IsNullOrEmpty(strFailedAddressWarning) AndAlso strFailedAddressWarning.Trim.Length > 0 Then
                    objAllStops.FailedAddressMessage &= " " & strFailedAddressWarning
                End If
                If intPCMReturn <> 0 Then blnRet = True
            End With
        Next
        Return blnRet
    End Function

    Private Function UpdateTruckStopData(ByRef objAllStops As PCM.clsAllStops, ByVal optStep As Integer, ByVal optDataFltr As OptimizerDataFilters) As Boolean
        Dim blnRet As Boolean = False
        Dim intStopCT As Integer = 0
        Dim blnComplete = False 'Modified By LVV on 11/3/20 for v-8.3.0.002 Task #20201027141251 - Optimizer Migration to 365
        For intStopCT = 1 To objAllStops.COUNT
            If intStopCT = objAllStops.COUNT Then blnComplete = True
            ProcessOptimizationSubStatusUpdateKey("StatusMSGOptUpdateOptimizedStopData", "Saving updated optimized stop data record", optStep, intStopCT, objAllStops.COUNT, 1, blnComplete, optDataFltr) 'Modified By LVV on 11/3/20 for v-8.3.0.002 Task #20201027141251 - Optimizer Migration to 365
            Dim objStop = objAllStops.Item(intStopCT)
            With objStop
                Try
                    NGLBookData.UpdateTruckStopData(.Stopname, .ID1, .ID2, .TruckDesignator, .SeqNbr, .DistToPrev, .TotalRouteCost, .ConsNumber)
                Catch ex As Exception
                    Return False
                End Try
            End With
        Next
        blnRet = True
        Return blnRet
    End Function

    'Private Function UpdateOptimizedStopData(ByRef objAllStops As NGL.FreightMaster.PCMiler.clsAllStops) As Boolean
    '    Dim blnRet As Boolean = False
    '    Dim intStopCT As Integer = 0
    '    For intStopCT = 1 To objAllStops.COUNT
    '        ProcessesOptimizationStatusUpdate("StatusMSGOptUpdateOptimizedStopData", intStopCT, objAllStops.COUNT)
    '        Dim objStop = objAllStops.Item(intStopCT)
    '        With objStop
    '            If Not _FMBookData.UpdateOptimizedStopData(.ID1, .SeqNbr, .DistToPrev, .ConsNumber, WCFProperties, PaneSettings) Then
    '                Return False
    '            End If
    '        End With
    '    Next
    '    blnRet = True
    '    Return blnRet
    'End Function

    Private Function updateBookingCarrier(ByRef objAllStops As PCM.clsAllStops, ByVal optStep As Integer, ByVal optDataFltr As OptimizerDataFilters) As List(Of DTO.CarrierCostResults)
        Dim oBookBLL As New NGLBookBLL(Parameters)
        Dim intBadCarriers As Integer = 0
        Dim AllConsNumbers As New List(Of String)
        Dim intStopCT As Integer = 0
        Dim MsgDictList As New List(Of DTO.CarrierCostResults)
        'get a unique list of cons numbers       
        For intStopCT = 1 To objAllStops.COUNT
            Dim results As DTO.CarrierCostResults
            ProcessOptimizationSubStatusUpdateKey("StatusMSGOptAllocateCostForTruck", "Assigning carriers and allocating costs for truck", optStep, intStopCT, objAllStops.COUNT, 1, False, optDataFltr) 'Modified By LVV on 11/3/20 for v-8.3.0.002 Task #20201027141251 - Optimizer Migration to 365
            'get a list of all cons numbers
            Dim oStop = objAllStops.Item(intStopCT)
            With oStop
                If Not AllConsNumbers.Contains(.ConsNumber) Then
                    AllConsNumbers.Add(.ConsNumber)
                    'we only run the assign truck stop carrier once for each cns number
                    results = oBookBLL.AssignTruckStopCarrier(.Stopname.Trim, .ID1.Trim, .ID2.Trim, .TruckDesignator.Trim, .SeqNbr, .DistToPrev, .TotalRouteCost, .ConsNumber.Trim)
                    If Not IsNothing(results) Then
                        MsgDictList.Add(results)
                        If Not results.Success Then intBadCarriers += 1
                    End If
                End If
            End With
        Next
        If intBadCarriers > 0 Then ProcessesOptimizationBadCarriers(intBadCarriers.ToString, optDataFltr)
        Return MsgDictList
    End Function

    'Private Sub bSetUNCPath(ByVal szPathToSet As String)
    '    Dim intVal As Integer = SetCurrentDirectoryA(szPathToSet)
    'End Sub

    'Private Function readAllStops(ByVal CompControl As Integer) As NGL.FreightMaster.PCMiler.clsAllStops
    '    Dim intStopNumber As Integer
    '    Dim strStopName As String
    '    Dim strID1 As String
    '    Dim strID2 As String
    '    Dim strTruckName As String
    '    Dim mlngTruckNumber As Integer
    '    Dim intSeqNumber As Integer
    '    Dim dblDistToPrev As Double
    '    Dim dblTotalCost As Double
    '    Dim strRecord As String
    '    Dim objallstops As New NGL.FreightMaster.PCMiler.clsAllStops
    '    Dim strConsNbr As String
    '    Dim oRoutes As New List(Of clsRoute)
    '    Dim strNotLoads As String

    '    Dim dblCT As Double
    '    Dim SolutionData As String()

    '    dblCT = 0
    '    Dim blnDataFound As Boolean = False
    '    Do
    '        dblCT += 1
    '        Try

    '            If File.Exists(TSSolutionFile) Then
    '                SolutionData = File.ReadAllLines(TSSolutionFile)
    '            End If
    '            If Not SolutionData Is Nothing AndAlso SolutionData.Length > 0 Then
    '                'we have data so test if the file is complete
    '                strRecord = SolutionData(SolutionData.Count - 1)
    '                If IsNumeric(Left(strRecord, 1)) Then
    '                    'test if the last record matches the number of stops
    '                    If CInt(Mid(strRecord, 1, 4)) >= _TSStops Then
    '                        blnDataFound = True
    '                        Exit Do
    '                    End If
    '                End If
    '            End If
    '        Catch ex As System.IO.IOException
    '            'do nothing keep waiting. the file may be locked or in use
    '        Catch ex As Exception
    '            Throw
    '        End Try
    '        If Not blnDataFound AndAlso dblCT > TSWaitForStops Then
    '            ProcessesTimeOutException(Nothing, "E_TSOptimizationReadStopsTimeOut", "")
    '            Return Nothing
    '        End If
    '    Loop Until blnDataFound

    '    For Each strRecord In SolutionData
    '        If IsNumeric(Left(strRecord, 1)) Then
    '            'this is an actual record so add it to the class
    '            intStopNumber = CInt(Mid(strRecord, 1, 4))
    '            strStopName = Mid(strRecord, 5, 20)
    '            strID1 = Mid(strRecord, 26, 15)
    '            strID2 = Mid(strRecord, 42, 10)
    '            strTruckName = Trim(Mid(strRecord, 53, 25))
    '            mlngTruckNumber = CLng(Val(Mid(strRecord, 79, 3)))
    '            intSeqNumber = CInt(Val(Mid(strRecord, 83, 4)))
    '            dblDistToPrev = CDbl(Val(Mid(strRecord, 88, 7)))
    '            dblTotalCost = CDbl(Val(Mid(strRecord, 96, 8)))

    '            If strTruckName = "Terminal" Then
    '                strNotLoads = strNotLoads & strStopName & vbCrLf
    '            Else
    '                strConsNbr = lookupRouteConsNumber(strTruckName, oRoutes)
    '                If String.IsNullOrEmpty(strConsNbr) OrElse Len(Trim(strConsNbr)) < 1 Then
    '                    'we need to get the next cons number
    '                    Dim oRoute = New clsRoute
    '                    With oRoute
    '                        .ConsNumber = _FMBatchProcessingData.GetNextConsNumber2Way(CompControl, WCFProperties, PaneSettings)
    '                        .TruckName = strTruckName
    '                        .TruckNumber = mlngTruckNumber
    '                        strConsNbr = .ConsNumber
    '                    End With
    '                    oRoutes.Add(oRoute)
    '                End If
    '                objallstops.Add(
    '                intStopNumber,
    '                strStopName,
    '                strID1,
    '                strID2,
    '                strTruckName,
    '                mlngTruckNumber,
    '                intSeqNumber,
    '                dblDistToPrev,
    '                dblTotalCost,
    '                strConsNbr)
    '            End If

    '        End If
    '    Next
    '    If Not String.IsNullOrEmpty(strNotLoads) Then ProcessesOptimizationNotLoaded(strNotLoads)

    '    Return objallstops

    'End Function

    'Private Function readPartialResults() As String
    '    Dim oResultData As New clsNGLOptResults(NGLOptimizerDataPath, NGLOptimizerResultFileName)
    '    Return oResultData.readPartialFile()
    'End Function

    'Private Function readNGLOptResults(ByVal CompControl As Integer, ByVal oStopFile As clsNGLOptStopData) As NGL.FreightMaster.PCMiler.clsAllStops
    '    Dim intStopNumber As Integer
    '    Dim strStopName As String
    '    Dim strID1 As String
    '    Dim strID2 As String
    '    Dim strTruckName As String
    '    Dim mlngTruckNumber As Integer
    '    Dim intSeqNumber As Integer
    '    Dim dblDistToPrev As Double
    '    Dim dblTotalCost As Double
    '    Dim strRecord As String
    '    Dim objallstops As New NGL.FreightMaster.PCMiler.clsAllStops
    '    Dim strConsNbr As String
    '    Dim oRoutes As New List(Of clsRoute)
    '    Dim strNotLoads As String

    '    Dim dblCT As Double
    '    Dim SolutionData As String()

    '    dblCT = 0
    '    Dim blnDataFound As Boolean = False
    '    Dim oResultData As New clsNGLOptResults(NGLOptimizerDataPath, NGLOptimizerResultFileName)
    '    Do
    '        dblCT += 1
    '        If oResultData.readthefile() Then
    '            If oResultData.ParseReport() Then
    '                'TODO: Add code to test if the report is complete
    '                blnDataFound = True
    '                Exit Do
    '            End If
    '        End If

    '        If Not blnDataFound AndAlso dblCT > NGLOptimizerTimeOut Then
    '            ProcessesTimeOutException(Nothing, "E_NGLOptimizationReadResultTimeOut", "")
    '            Return Nothing
    '        End If
    '    Loop Until blnDataFound

    '    For Each item In oResultData.Items
    '        Dim intStopSeq As Integer = 1
    '        For Each s In item.Stops
    '            'get the saved data from the stopdata 
    '            Dim intNumber As Integer = 0
    '            Integer.TryParse(s, intNumber)
    '            If intNumber > 0 Then
    '                Dim stopitem = (From d In oStopFile.Items Where d.Number = s).FirstOrDefault
    '                If Not stopitem Is Nothing Then
    '                    intStopNumber = intNumber
    '                    strStopName = stopitem.Zip
    '                    strID1 = stopitem.ID
    '                    strID2 = stopitem.BookControl
    '                    mlngTruckNumber = item.TruckNumber
    '                    strTruckName = mlngTruckNumber.ToString
    '                    intSeqNumber = intStopSeq
    '                    strConsNbr = lookupRouteConsNumber(strTruckName, oRoutes)
    '                    If String.IsNullOrEmpty(strConsNbr) OrElse Len(Trim(strConsNbr)) < 1 Then
    '                        'we need to get the next cons number
    '                        Dim oRoute = New clsRoute
    '                        With oRoute
    '                            .ConsNumber = _FMBatchProcessingData.GetNextConsNumber2Way(CompControl, WCFProperties, PaneSettings)
    '                            .TruckName = strTruckName
    '                            .TruckNumber = mlngTruckNumber
    '                            strConsNbr = .ConsNumber
    '                        End With
    '                        oRoutes.Add(oRoute)
    '                    End If
    '                    objallstops.Add(
    '                    intStopNumber,
    '                    strStopName,
    '                    strID1,
    '                    strID2,
    '                    strTruckName,
    '                    mlngTruckNumber,
    '                    intSeqNumber,
    '                    dblDistToPrev,
    '                    dblTotalCost,
    '                    strConsNbr)
    '                    intStopSeq += 1
    '                Else
    '                    strNotLoads &= "Stop Number " & s & " was not loaded because the optimizer failed to find a valid truck." & vbCrLf
    '                End If
    '            End If
    '        Next
    '    Next
    '    If Not oStopFile.BadStops Is Nothing AndAlso oStopFile.BadStops.Count > 0 Then
    '        For Each b In oStopFile.BadStops
    '            strNotLoads &= "Stop Number " & b.ID.ToString & " was not loaded because: " & b.Message & " ; using zip code: " & b.Zip & vbCrLf
    '        Next
    '    End If
    '    If Not String.IsNullOrEmpty(strNotLoads) Then ProcessesOptimizationNotLoaded(strNotLoads)

    '    Return objallstops

    'End Function

    Private Function processNGLOptResults(ByVal CompControl As Integer, ByVal oStopFile As clsNGLOptStopData, ByVal oNGLOptRet As NGLOptRet, ByVal optDataFltr As OptimizerDataFilters) As PCM.clsAllStops
        Dim oBatch As New DAL.NGLBatchProcessDataProvider(Parameters)
        Dim intStopNumber As Integer
        Dim strStopName As String
        Dim strID1 As String
        Dim strID2 As String
        Dim strTruckName As String
        Dim mlngTruckNumber As Integer
        Dim intSeqNumber As Integer
        Dim dblDistToPrev As Double
        Dim dblTotalCost As Double
        Dim objallstops As New PCM.clsAllStops
        Dim strConsNbr As String
        Dim oRoutes As New List(Of clsRoute)
        Dim strNotLoads As String = ""

        If oNGLOptRet Is Nothing OrElse oNGLOptRet.Loads Is Nothing OrElse oNGLOptRet.Loads.Count < 1 Then Return Nothing
        'For Each item In oResultData.Items
        For Each item In oNGLOptRet.Loads
            Dim intStopSeq As Integer = 1
            For Each intNumber As Integer In item.StopsSeq
                Dim stopitem = (From d In oStopFile.Items Where d.Number = intNumber).FirstOrDefault
                If Not stopitem Is Nothing Then
                    intStopNumber = intNumber
                    strStopName = stopitem.Zip
                    strID1 = stopitem.ID
                    strID2 = stopitem.BookControl
                    mlngTruckNumber = item.RouteNumber
                    strTruckName = mlngTruckNumber.ToString
                    intSeqNumber = intStopSeq
                    strConsNbr = lookupRouteConsNumber(strTruckName, oRoutes)
                    If String.IsNullOrEmpty(strConsNbr) OrElse Len(Trim(strConsNbr)) < 1 Then
                        'we need to get the next cons number
                        Dim oRoute = New clsRoute
                        With oRoute
                            .ConsNumber = oBatch.GetNextConsNumber(CompControl)
                            .TruckName = strTruckName
                            .TruckNumber = mlngTruckNumber
                            strConsNbr = .ConsNumber
                        End With
                        oRoutes.Add(oRoute)
                    End If
                    objallstops.Add(
                    intStopNumber,
                    strStopName,
                    strID1,
                    strID2,
                    strTruckName,
                    mlngTruckNumber,
                    intSeqNumber,
                    dblDistToPrev,
                    dblTotalCost,
                    strConsNbr)
                    intStopSeq += 1
                Else
                    strNotLoads &= "Stop Number " & intNumber.ToString & " was not loaded because the optimizer failed to find a valid truck." & vbCrLf
                End If
            Next
        Next
        If Not oStopFile.BadStops Is Nothing AndAlso oStopFile.BadStops.Count > 0 Then
            For Each b In oStopFile.BadStops
                strNotLoads &= "Order Number " & b.OrderNumber & " was not loaded because: " & b.Message & " ; using zip code: " & b.Zip & vbCrLf
            Next
        End If
        If Not String.IsNullOrEmpty(strNotLoads) Then ProcessesOptimizationNotLoaded(strNotLoads, optDataFltr)
        Return objallstops
    End Function


    Private Function lookupRouteConsNumber(ByVal TruckName As String, ByVal List As List(Of clsRoute)) As String
        Dim strRet As String = ""
        If Not List Is Nothing Then
            Dim var = From c In List Where c.TruckName = TruckName Select c
            If var.Count > 0 Then
                Dim obj As clsRoute = var(0)
                strRet = obj.ConsNumber
            End If
        End If
        Return strRet
    End Function


    'Private Sub ProcessesTimeOutException(ByVal e As FaultExceptionEventArgs)
    'Dim o As New OptimizationExceptionEventArgs(e.Error, False, e.UserState, e.Reason, e.Detail)
    'If requestingContext Is Nothing Then
    '    RaiseOptimizationTimeOutException(o)
    'Else
    '    Dim callback As New SendOrPostCallback(AddressOf RaiseOptimizationTimeOutException)
    '    requestingContext.Post(callback, o)
    'End If
    'End Sub

    Private Sub ProcessesTimeOutException(ByVal e As Exception, ByVal reason As String, ByVal optDataFltr As OptimizerDataFilters, Optional ByVal detail As String = "")
        Dim oLocalize As New DAL.NGLcmLocalizeKeyValuePairData(Parameters)
        Dim oOpt As New DAL.NGLOptMsgData(Parameters)
        Dim strWarning As String = ""
        If Not e Is Nothing Then
            strWarning = e.Message
        Else
            If Not String.IsNullOrEmpty(reason) Then strWarning = oLocalize.GetLocalizedValueByKey(reason, reason)
            If Not String.IsNullOrEmpty(detail) Then
                If Not String.IsNullOrEmpty(strWarning) Then strWarning &= vbCrLf
                strWarning &= oLocalize.GetLocalizedValueByKey(detail, detail)
            End If
        End If
        oOpt.SaveOptimizerStatusMessage(0, strWarning, "", 0, True, optDataFltr.CompName, optDataFltr.Inbound, eOptMsgType.ErrorMsg) 'save the message to the db
    End Sub

    'Private Sub ProcessesFaultException(ByVal e As FaultExceptionEventArgs)
    'Dim o As New OptimizationExceptionEventArgs(e.Error, False, e.UserState, e.Reason, e.Detail)
    'If requestingContext Is Nothing Then
    '    RaiseOptimizationException(o)
    'Else
    '    Dim callback As New SendOrPostCallback(AddressOf RaiseOptimizationException)
    '    requestingContext.Post(callback, o)
    'End If
    'End Sub

    ''' <summary>
    ''' This method assumes Reason and Details are already localized before they are passed in as parameters
    ''' </summary>
    ''' <param name="ex"></param>
    ''' <param name="reason"></param>
    ''' <param name="details"></param>
    Private Sub ProcessesOptimizationException(ByVal ex As Exception, ByVal reason As String, ByVal details As String, ByVal optDataFltr As OptimizerDataFilters)
        Dim oLocalize As New DAL.NGLcmLocalizeKeyValuePairData(Parameters)
        Dim oOpt As New DAL.NGLOptMsgData(Parameters)
        Dim strWarning As String = ""
        If Not String.IsNullOrEmpty(reason) Then
            strWarning = reason
            If Not String.IsNullOrEmpty(details) Then strWarning &= vbCrLf & details
            Debug.WriteLine(strWarning)
            oOpt.SaveOptimizerStatusMessage(0, strWarning, "", 0, True, optDataFltr.CompName, optDataFltr.Inbound, eOptMsgType.WarningMsg) 'save the message to the db
        Else
            Debug.WriteLine(ex.Message)
            oOpt.SaveOptimizerStatusMessage(0, strWarning, "", 0, True, optDataFltr.CompName, optDataFltr.Inbound, eOptMsgType.ErrorMsg) 'save the message to the db
        End If
    End Sub

    'Not used anymore, replaced by above
    Private Sub ProcessesOptimizationExceptionKey(ByVal ex As Exception, ByVal reason As String, ByVal details As String, ByVal optDataFltr As OptimizerDataFilters)
        Dim oLocalize As New DAL.NGLcmLocalizeKeyValuePairData(Parameters)
        Dim oOpt As New DAL.NGLOptMsgData(Parameters)
        Dim strWarning As String = ""
        If Not String.IsNullOrEmpty(reason) Then
            If oLocalize.DoesKeyExist(reason) Then strWarning = oLocalize.GetLocalizedValueByKey(reason, reason)
            If Not String.IsNullOrEmpty(details) Then
                If oLocalize.DoesKeyExist(reason) Then strWarning &= vbCrLf & oLocalize.GetLocalizedValueByKey(details, details)
            End If
            Debug.WriteLine(strWarning)
            oOpt.SaveOptimizerStatusMessage(0, strWarning, "", 0, True, optDataFltr.CompName, optDataFltr.Inbound, eOptMsgType.WarningMsg) 'save the message to the db
        Else
            Debug.WriteLine(ex.Message)
            oOpt.SaveOptimizerStatusMessage(0, strWarning, "", 0, True, optDataFltr.CompName, optDataFltr.Inbound, eOptMsgType.ErrorMsg) 'save the message to the db
        End If
    End Sub

    Private Sub ProcessOptimizationStatusUpdateKey(ByVal strKey As String, ByVal strVal As String, ByVal optStep As Integer, ByVal totalSteps As Integer, ByVal progress As Integer, ByVal blnComplete As Boolean, ByVal optDataFltr As OptimizerDataFilters)
        Dim oLocalize As New DAL.NGLcmLocalizeKeyValuePairData(Parameters)
        Dim oOpt As New DAL.NGLOptMsgData(Parameters)
        If String.IsNullOrWhiteSpace(strVal) Then strVal = strKey
        If Not String.IsNullOrEmpty(strVal) Then
            Dim strMsg = oLocalize.GetLocalizedValueByKey(strKey, strVal)
            If Not String.IsNullOrWhiteSpace(strMsg) Then
                Dim strStatusMsg = String.Format("{0} {1} of {2}", strMsg, optStep, totalSteps)
                Debug.WriteLine(strStatusMsg)
                oOpt.SaveOptimizerStatusMessage(optStep, strStatusMsg, "", progress, blnComplete, optDataFltr.CompName, optDataFltr.Inbound, eOptMsgType.StatusMsg) 'save the message to the db
            End If
        End If
    End Sub

    ''' <summary>
    ''' Saves the status message for a sub step under the main optimizer steps.
    ''' A key is passed in for message localization.
    ''' </summary>
    ''' <param name="strKey">Key for localization of the message</param>
    ''' <param name="strVal">Default value associated with the key</param>
    ''' <param name="optStep">Main Optimizer step we are currently processing</param>
    ''' <param name="ctCurrent">The current number of the step in the sub task</param>
    ''' <param name="ctTotal">The total number of steps in the sub task</param>
    ''' <param name="progress">The chunk of progress completed thus far</param>
    ''' <param name="blnComplete">Flag representing if the entire Main Step has been completed</param>
    Private Sub ProcessOptimizationSubStatusUpdateKey(ByVal strKey As String, ByVal strVal As String, ByVal optStep As Integer, ByVal ctCurrent As Integer, ByVal ctTotal As Integer, ByVal progress As Integer, ByVal blnComplete As Boolean, ByVal optDataFltr As OptimizerDataFilters)
        Dim oLocalize As New DAL.NGLcmLocalizeKeyValuePairData(Parameters)
        Dim oOpt As New DAL.NGLOptMsgData(Parameters)
        If String.IsNullOrWhiteSpace(strVal) Then strVal = strKey
        If Not String.IsNullOrEmpty(strVal) Then
            Dim strMsg = oLocalize.GetLocalizedValueByKey(strKey, strVal)
            If Not String.IsNullOrWhiteSpace(strMsg) Then
                Dim strStatusSubMsg = String.Format("{0} {1} of {2}", strMsg, ctCurrent, ctTotal)
                Debug.WriteLine(strStatusSubMsg)
                oOpt.SaveOptimizerStatusMessage(optStep, "", strStatusSubMsg, progress, blnComplete, optDataFltr.CompName, optDataFltr.Inbound, eOptMsgType.StatusMsg) 'save the message to the db
            End If
        End If
    End Sub

    'Not used anymore, replaced by above
    Private Sub ProcessesOptimizationStatusUpdate(ByVal strVal As String, ByVal curstep As Integer, ByVal steps As Integer, Optional ByVal blnIsKey As Boolean = True)
        Dim oLocalize As New DAL.NGLcmLocalizeKeyValuePairData(Parameters)
        Dim strMsg As String = ""
        If Not String.IsNullOrEmpty(strVal) Then
            If blnIsKey Then
                strMsg = oLocalize.GetLocalizedValueByKey(strVal, strVal) 'The key was provided as a parameter so use it to look up the message text
            Else
                strMsg = strVal 'The actual message text was provided as a parameter
            End If

            Dim strStatusMsg = String.Format("{0} {1} of {2}", strMsg, curstep, steps)

            'TODO SAVE THE MESSAGE TO THE DB
            Debug.WriteLine(strStatusMsg)
        End If
    End Sub

    Private Sub ProcessesOptimizationNotLoaded(ByVal message As String, ByVal optDataFltr As OptimizerDataFilters)
        Dim oLocalize As New DAL.NGLcmLocalizeKeyValuePairData(Parameters)
        Dim oOpt As New DAL.NGLOptMsgData(Parameters)
        Dim strMsg = oLocalize.GetLocalizedValueByKey("MSGOptNotLoads", "Optimized Loads Error! Some of your Stops cound not be loaded. Please check your information and try again. {0} ")
        Dim sFmt = String.Format(strMsg, message)
        oOpt.SaveOptimizerStatusMessage(0, sFmt, "", 0, True, optDataFltr.CompName, optDataFltr.Inbound, eOptMsgType.ErrorMsg) 'save the message to the db
    End Sub

    Private Sub ProcessesOptimizationBadCarriers(ByVal message As String, ByVal optDataFltr As OptimizerDataFilters)
        Dim oLocalize As New DAL.NGLcmLocalizeKeyValuePairData(Parameters)
        Dim oOpt As New DAL.NGLOptMsgData(Parameters)
        Dim strMsg = oLocalize.GetLocalizedValueByKey("MSGOptBadCarriers", "Invalid Carriers Have Been Removed! {0} order(s) were optimized but had invalid carriers. Tariffs may not exist for the final stop on each load. Please check your data for missing carrier assignments.")
        Dim sFmt = String.Format(strMsg, message)
        oOpt.SaveOptimizerStatusMessage(0, sFmt, "", 0, True, optDataFltr.CompName, optDataFltr.Inbound, eOptMsgType.ErrorMsg) 'save the message to the db
    End Sub

    Private Sub OptimizationComplete(ByVal Err As Exception, ByVal resyncerror As String, ByVal msgList As List(Of DTO.CarrierCostResults), ByVal failedaddressmessage As String, ByVal badaddresscount As Integer, ByVal badaddresscountBatchID As Double, ByVal optDataFltr As OptimizerDataFilters)
        Dim oLocalize As New DAL.NGLcmLocalizeKeyValuePairData(Parameters)
        Dim oOpt As New DAL.NGLOptMsgData(Parameters)
        Dim strMsg = ""
        Dim intOptMsgType = eOptMsgType.StatusMsg 'Default Status Msg
        Try
            If Not Err Is Nothing Then
                strMsg = Err.Message
                intOptMsgType = eOptMsgType.ErrorMsg 'Error Msg
            ElseIf Not String.IsNullOrEmpty(resyncerror) Then
                strMsg = oLocalize.GetLocalizedValueByKey(resyncerror, resyncerror)
                intOptMsgType = eOptMsgType.ErrorMsg 'Error Msg
            ElseIf msgList IsNot Nothing AndAlso msgList.Count > 0 Then
                'TODO - implment this -'PaneSettings.MainInterface.FMMessage.ShowFMMessageBox(PaneSettings.MainInterface.LocalizeString("TranCodeChangeSuccess"), msgList, PaneSettings)
                strMsg = oLocalize.GetLocalizedValueByKey("StatusMSGOptSolutionGeneratedWarnings", "Success! Your optimized solution has been generated with warnings")
            ElseIf Not String.IsNullOrEmpty(failedaddressmessage) Then
                strMsg = oLocalize.GetLocalizedValueByKey(resyncerror, resyncerror)
                intOptMsgType = eOptMsgType.ErrorMsg 'Error Msg
            Else
                Dim strResyncStopFail = oLocalize.GetLocalizedValueByKey("MSGOptResyncStopsFailure", "The following stops could not be processed by PC Miler.")
                strMsg = strResyncStopFail & vbCrLf & failedaddressmessage
                strMsg += oLocalize.GetLocalizedValueByKey("StatusMSGOptSolutionGenerated", "Success! Your optimized solution has been generated!")
            End If
            If badaddresscount > 0 AndAlso getParValue("PCMilerShowResynAddressWarnings") > 0 Then
                'TODO - implement this
                'Dim win As New AddressWarningReport(PaneSettings.MainInterface, My.Settings.Theme, badaddresscountBatchID)
                'win.Topmost = True
                'win.ShowInTaskbar = True
                'win.ShowDialog()
            End If
            'Me.Dispatcher.BeginInvoke(DispatcherPriority.Background, New SummaryDelegateBS(AddressOf RefreshDataWBadStops), e.BadStops)
        Catch ex As InvalidOperationException
            'Me.PaneSettings.MainInterface.FMMessage.ShowFMWarning(Me.PaneSettings.MainInterface.LocalizeString(ex.Message))
        Catch ex As Exception
            'Me.PaneSettings.MainInterface.FMMessage.ShowFMError(ex.Message)
        Finally
            Debug.WriteLine(strMsg)
            oOpt.SaveOptimizerStatusMessage(0, strMsg, "", 0, True, optDataFltr.CompName, optDataFltr.Inbound, intOptMsgType) 'save the message to the db
        End Try
    End Sub

    Private Sub ProcessesOptimizationComplete(ByVal ex As Exception, ByVal resyncerror As String, ByVal failedaddressmessage As String, ByVal badaddresscount As Integer, ByVal badaddresscountBatchID As Double, ByVal optDataFltr As OptimizerDataFilters)
        OptimizationComplete(ex, resyncerror, Nothing, failedaddressmessage, badaddresscount, badaddresscountBatchID, optDataFltr)
    End Sub

    Private Sub ProcessesOptimizationComplete(ByVal msgList As List(Of DTO.CarrierCostResults), ByVal failedaddressmessage As String, ByVal badaddresscount As Integer, ByVal badaddresscountBatchID As Double, ByVal vOptStopsHadSHID() As DTO.vOptimizationStop, ByVal badaddressList As List(Of clsNGLOptStopItem), ByVal optDataFltr As OptimizerDataFilters)
        OptimizationComplete(Nothing, "", msgList, failedaddressmessage, badaddresscount, badaddresscountBatchID, optDataFltr)
    End Sub

#End Region





End Class

Public Class clsRoute

#Region " Constructors "

    Public Sub New()
        MyBase.New()
    End Sub


#End Region


#Region " Properties"

    Private mstrTruckNumber As String = ""
    Public Property TruckNumber As String
        Get
            Return mstrTruckNumber
        End Get
        Set(ByVal value As String)
            mstrTruckNumber = value
        End Set
    End Property

    Private mstrConsNumber As String = ""
    Public Property ConsNumber As String
        Get
            Return mstrConsNumber
        End Get
        Set(ByVal value As String)
            mstrConsNumber = value
        End Set
    End Property

    Private mstrTruckName As String = ""
    Public Property TruckName As String
        Get
            Return mstrTruckName
        End Get
        Set(ByVal value As String)
            mstrTruckName = value
        End Set
    End Property

#End Region

End Class


Public Class clsNGLOptStopData

#Region " Constructors "

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal path As String, ByVal file As String)
        MyBase.New()
        mstrFilePath = path
        mstrFileName = file
    End Sub

    Public Sub New(ByVal path As String,
                   ByVal file As String,
                   ByVal maxcases As Double,
                   ByVal maxcubes As Double,
                   ByVal maxpallets As Double,
                   ByVal maxweight As Double,
                   ByVal pcMiler As NGLPCMilerBLL)
        MyBase.New()
        mstrFilePath = path
        mstrFileName = file
        Me.MaxCases = maxcases
        Me.MaxCubes = maxcubes
        Me.MaxPallets = maxpallets
        Me.MaxWeight = maxweight
        Me.oPCMiler = pcMiler
    End Sub

#End Region

#Region " Properties"

    Dim oPCMiler As NGLPCMilerBLL

    Private mstrFilePath As String = "C:\Data\NGL\"
    Public Property FilePath() As String
        Get
            If Right(mstrFilePath, 1) <> "\" Then mstrFilePath &= "\"
            Return mstrFilePath
        End Get
        Set(ByVal value As String)
            mstrFilePath = value
        End Set
    End Property

    Private mstrFileName As String = "Stops.csv"
    Public Property FileName() As String
        Get
            Return mstrFileName
        End Get
        Set(ByVal value As String)
            mstrFileName = value
        End Set
    End Property

    Public Items As New List(Of clsNGLOptStopItem)

    Public BadStops As New List(Of clsNGLOptStopItem)


    Public ReadOnly Property StopFile() As String
        Get
            Return FilePath & FileName
        End Get
    End Property


    Private _MaxCubes As Double = 0
    Public Property MaxCubes() As Double
        Get
            Return _MaxCubes
        End Get
        Set(ByVal value As Double)
            _MaxCubes = value
        End Set
    End Property

    Private _MaxPallets As Double = 0
    Public Property MaxPallets() As Double
        Get
            Return _MaxPallets
        End Get
        Set(ByVal value As Double)
            _MaxPallets = value
        End Set
    End Property

    Private _MaxWeight As Double = 0
    Public Property MaxWeight() As Double
        Get
            Return _MaxWeight
        End Get
        Set(ByVal value As Double)
            _MaxWeight = value
        End Set
    End Property

    Private _MaxCases As Double = 0
    Public Property MaxCases() As Double
        Get
            Return _MaxCases
        End Get
        Set(ByVal value As Double)
            _MaxCases = value
        End Set
    End Property

#End Region

#Region " Public Methods"


    Public Function populateStopDataObject(ByRef oNGLOptStops As List(Of NGLOptStop)) As Boolean
        Dim blnRet As Boolean = False
        Try
            If oNGLOptStops Is Nothing Then oNGLOptStops = New List(Of NGLOptStop)
            Dim strData As New List(Of String)
            For Each i In Items
                oNGLOptStops.Add(New NGLOptStop With {
                                 .StopNumber = i.Number _
                                 , .ID1 = i.ID _
                                 , .ZipCode = i.Zip _
                                 , .Cases = i.Cases _
                                 , .Weight = i.Weight _
                                 , .Pallets = i.Pallets _
                                 , .Cubes = i.Cubes _
                                 , .Temp = i.Type _
                                 , .Code = i.SpecialCode})
            Next
            blnRet = True
        Catch ex As ArgumentException
            Throw New InvalidOperationException("E_OptSWriteStopDataFileFailure", ex)
        Catch ex As UnauthorizedAccessException
            Throw New InvalidOperationException("E_OptSWriteStopDataFileFailure", ex)
        Catch ex As System.Security.SecurityException
            Throw New InvalidOperationException("E_OptSWriteStopDataFileFailure", ex)
        Catch ex As System.IO.IOException
            Throw New InvalidOperationException("E_OptSWriteStopDataFileFailure", ex)
        Catch ex As InvalidOperationException
            Throw
        Catch ex As Exception
            Throw
        End Try
        Return blnRet
    End Function

    'CHANGE LVV 8/7/15
    Public Function populateStopDataObjectFix(ByRef oNGLOptStops As List(Of NGLOptStop)) As Boolean
        Dim blnRet As Boolean = False
        Try
            If oNGLOptStops Is Nothing Then oNGLOptStops = New List(Of NGLOptStop)
            Dim strData As New List(Of String)
            For Each i In Items
                oNGLOptStops.Add(New NGLOptStop With {
                                 .StopNumber = i.Number _
                                 , .ID1 = i.ID _
                                 , .ZipCode = i.Zip _
                                 , .Address1 = i.Address1 _
                                 , .City = i.City _
                                 , .State = i.State _
                                 , .Country = i.Country _
                                 , .Cases = i.Cases _
                                 , .Weight = i.Weight _
                                 , .Pallets = i.Pallets _
                                 , .Cubes = i.Cubes _
                                 , .Temp = i.Type _
                                 , .Code = i.SpecialCode})
            Next
            blnRet = True
        Catch ex As ArgumentException
            Throw New InvalidOperationException("E_OptSWriteStopDataFileFailure", ex)
        Catch ex As UnauthorizedAccessException
            Throw New InvalidOperationException("E_OptSWriteStopDataFileFailure", ex)
        Catch ex As System.Security.SecurityException
            Throw New InvalidOperationException("E_OptSWriteStopDataFileFailure", ex)
        Catch ex As System.IO.IOException
            Throw New InvalidOperationException("E_OptSWriteStopDataFileFailure", ex)
        Catch ex As InvalidOperationException
            Throw
        Catch ex As Exception
            Throw
        End Try
        Return blnRet
    End Function


    Public Function writeFile() As Boolean
        Dim blnRet As Boolean = False
        Try
            If File.Exists(StopFile) Then File.Delete(StopFile)
            Dim strData As New List(Of String)
            For Each i In Items
                strData.Add(i.ToString)
            Next
            File.WriteAllLines(StopFile, strData.ToArray)
            blnRet = True
        Catch ex As ArgumentException
            Throw New InvalidOperationException("E_OptSWriteStopDataFileFailure", ex)
        Catch ex As UnauthorizedAccessException
            Throw New InvalidOperationException("E_OptSWriteStopDataFileFailure", ex)
        Catch ex As System.Security.SecurityException
            Throw New InvalidOperationException("E_OptSWriteStopDataFileFailure", ex)
        Catch ex As System.IO.IOException
            Throw New InvalidOperationException("E_OptSWriteStopDataFileFailure", ex)
        Catch ex As InvalidOperationException
            Throw
        Catch ex As Exception
            Throw
        End Try
        Return blnRet
    End Function

    Public Function addStop(ByRef oStop As clsNGLOptStopItem, ByRef intSeq As Integer) As String
        Dim strRet As String = ""
        Dim strSep As String = ""
        Try
            strRet = "Validating Address For Stop: " & oStop.ID
            'Validate the capacity
            Dim blnCapacityValid As Boolean = True
            If MaxCases > 0 AndAlso oStop.Cases > MaxCases Then
                blnCapacityValid = False
                oStop.Message = "too many cases"
                strSep = " and "
            End If

            If MaxCubes > 0 AndAlso oStop.Cubes > MaxCubes Then
                blnCapacityValid = False
                oStop.Message &= strSep & "too many cubes"
                strSep = " and "
            End If

            If MaxPallets > 0 AndAlso oStop.Pallets > MaxPallets Then
                blnCapacityValid = False
                oStop.Message &= strSep & "too many pallets"
                strSep = " and "
            End If

            If MaxWeight > 0 AndAlso oStop.Weight > MaxWeight Then
                blnCapacityValid = False
                oStop.Message &= strSep & "too much weight"
                strSep = " and "
            End If
            If blnCapacityValid Then
                If oPCMiler.PCMGetFlatMiles(oStop.Zip, "60067") > 0 Then
                    Items.Add(oStop)
                    intSeq += 1
                Else
                    oStop.Message = "invalid postal code cannot calculate miles"
                    BadStops.Add(oStop)
                End If
            Else
                BadStops.Add(oStop)
            End If
        Catch ex As Exception
            oStop.Message &= strSep & ex.Message
            BadStops.Add(oStop)
            strSep = " and "
            strRet = ex.Message
        End Try
        Return strRet
    End Function

    Public Function getDistanceAndWriteFile(ByVal FileName As String) As Boolean
        Dim blnRet As Boolean = False
        Try
            If File.Exists(Me.FilePath & FileName) Then File.Delete(Me.FilePath & FileName)
            Dim strData As New List(Of String)
            For Each s In Items
                s.getDistance(Items)
                Dim strRecord As String = ""
                Dim strSep As String = ""
                For Each d In s.Distances
                    strRecord &= strSep & d.Miles.ToString
                    strSep = ","
                Next
                If Not String.IsNullOrEmpty(strRecord) Then strData.Add(strRecord)
            Next
            File.WriteAllLines(Me.FilePath & FileName, strData.ToArray)
            blnRet = True
        Catch ex As ArgumentException
            Throw New InvalidOperationException("E_OptSWriteStopDataFileFailure", ex)
        Catch ex As UnauthorizedAccessException
            Throw New InvalidOperationException("E_OptSWriteStopDataFileFailure", ex)
        Catch ex As System.Security.SecurityException
            Throw New InvalidOperationException("E_OptSWriteStopDataFileFailure", ex)
        Catch ex As System.IO.IOException
            Throw New InvalidOperationException("E_OptSWriteStopDataFileFailure", ex)
        Catch ex As InvalidOperationException
            Throw
        Catch ex As Exception
            Throw
        End Try
        Return blnRet
    End Function

    Public Function writeDistanceFile(ByVal FileName As String) As Boolean
        Dim blnRet As Boolean = False
        Try
            If File.Exists(Me.FilePath & FileName) Then File.Delete(Me.FilePath & FileName)
            Dim strData As New List(Of String)
            For Each s In Items
                Dim strRecord As String = ""
                Dim strSep As String = ""
                For Each d In s.Distances
                    strRecord &= strSep & d.Miles.ToString
                    strSep = ","
                Next
                strRecord &= strSep & s.Number
                If Not String.IsNullOrEmpty(strRecord) Then strData.Add(strRecord)
            Next
            File.WriteAllLines(Me.FilePath & FileName, strData.ToArray)
            blnRet = True
        Catch ex As ArgumentException
            Throw New InvalidOperationException("E_OptSWriteStopDataFileFailure", ex)
        Catch ex As UnauthorizedAccessException
            Throw New InvalidOperationException("E_OptSWriteStopDataFileFailure", ex)
        Catch ex As System.Security.SecurityException
            Throw New InvalidOperationException("E_OptSWriteStopDataFileFailure", ex)
        Catch ex As System.IO.IOException
            Throw New InvalidOperationException("E_OptSWriteStopDataFileFailure", ex)
        Catch ex As InvalidOperationException
            Throw
        Catch ex As Exception
            Throw
        End Try
        Return blnRet
    End Function

    Public Function writeDistanceToArray() As Integer(,)
        Dim distancefile(Me.Items.Count, Me.Items.Count) As Integer
        Try
            Dim strData As New List(Of String)
            For i As Integer = 0 To Me.Items.Count - 1
                Dim strRecord As String = ""
                Dim strSep As String = ""
                Dim c As Integer = 1
                For Each d In Me.Items(i).Distances
                    distancefile(i + 1, c) = d.Miles
                    c += 1
                Next
            Next
        Catch ex As ArgumentException
            Throw New InvalidOperationException("E_OptSWriteStopDataFileFailure", ex)
        Catch ex As UnauthorizedAccessException
            Throw New InvalidOperationException("E_OptSWriteStopDataFileFailure", ex)
        Catch ex As System.Security.SecurityException
            Throw New InvalidOperationException("E_OptSWriteStopDataFileFailure", ex)
        Catch ex As System.IO.IOException
            Throw New InvalidOperationException("E_OptSWriteStopDataFileFailure", ex)
        Catch ex As InvalidOperationException
            Throw
        Catch ex As Exception
            Throw
        End Try
        Return distancefile
    End Function

#End Region

End Class

Public Class clsNGLOptStopItem

#Region "Constructors"

    Public Sub New()
        MyBase.New()
    End Sub

    'CHANGE LVV 8/7/15
    Public Sub New(ByVal pcMiler As NGLPCMilerBLL,
                   ByVal intNumber As Integer,
                   ByVal intBookControl As Integer,
                   ByVal strID As String,
                   ByVal strZip As String,
                   ByVal intCases As Integer,
                   ByVal intWeight As Integer,
                   ByVal intPallets As Integer,
                   ByVal intCubes As Integer,
                   ByVal intType As Integer,
                   ByVal intSpecialCode As Integer,
                   Optional ByVal strAddress1 As String = "",
                   Optional ByVal strCity As String = "",
                   Optional ByVal strState As String = "",
                   Optional ByVal strCountry As String = "")
        MyBase.New()
        Me.Number = intNumber
        Me.BookControl = intBookControl
        Me.ID = strID
        Me.Zip = strZip
        Me.Cases = intCases
        Me.Weight = intWeight
        Me.Pallets = intPallets
        Me.Cubes = intCubes
        Me.Type = intType
        Me.SpecialCode = intSpecialCode
        Me.Address1 = strAddress1
        Me.City = strCity
        Me.State = strState
        Me.Country = strCountry
        Me.oPCMiler = pcMiler
    End Sub

    'CHANGE LVV 8/7/15
    Public Sub New(ByVal pcMiler As NGLPCMilerBLL,
                   ByVal intNumber As Integer,
                   ByVal strOrderNumber As String,
                   ByVal strProNumber As String,
                   ByVal intBookControl As Integer,
                   ByVal strID As String,
                   ByVal strZip As String,
                   ByVal intCases As Integer,
                   ByVal intWeight As Integer,
                   ByVal intPallets As Integer,
                   ByVal intCubes As Integer,
                   ByVal intType As Integer,
                   ByVal intSpecialCode As Integer,
                   Optional ByVal strAddress1 As String = "",
                   Optional ByVal strCity As String = "",
                   Optional ByVal strState As String = "",
                   Optional ByVal strCountry As String = "")
        MyBase.New()
        Me.Number = intNumber
        Me.OrderNumber = strOrderNumber
        Me.ProNumber = strProNumber
        Me.BookControl = intBookControl
        Me.ID = strID
        Me.Zip = strZip
        Me.Cases = intCases
        Me.Weight = intWeight
        Me.Pallets = intPallets
        Me.Cubes = intCubes
        Me.Type = intType
        Me.SpecialCode = intSpecialCode
        Me.Address1 = strAddress1
        Me.City = strCity
        Me.State = strState
        Me.Country = strCountry
        Me.oPCMiler = pcMiler
    End Sub

#End Region

#Region "Properties"

    Dim oPCMiler As NGLPCMilerBLL

    Public Number As Integer = 0
    Public ID As String
    Public Zip As String
    'CHANGE LVV 8/7/15
    Public Address1 As String = ""
    Public City As String = ""
    Public State As String = ""
    Public Country As String = ""
    Public OrderNumber As String = ""
    Public ProNumber As String = ""

    Private _Cases As Integer = 1
    Public Property Cases As Integer
        Get
            If _Cases < 1 Then _Cases = 1
            Return _Cases
        End Get
        Set(value As Integer)
            _Cases = value
        End Set
    End Property

    Private _Weight As Integer = 1
    Public Property Weight As Integer
        Get
            If _Weight < 1 Then _Weight = 1
            Return _Weight
        End Get
        Set(value As Integer)
            _Weight = value
        End Set
    End Property

    Private _Pallets As Integer = 1
    Public Property Pallets As Integer
        Get
            If _Pallets < 1 Then _Pallets = 1
            Return _Pallets
        End Get
        Set(value As Integer)
            _Pallets = value
        End Set
    End Property

    Private _Cubes As Integer = 1
    Public Property Cubes As Integer
        Get
            If _Cubes < 1 Then _Cubes = 1
            Return _Cubes
        End Get
        Set(value As Integer)
            _Cubes = value
        End Set
    End Property

    Private _Message As String = ""
    Public Property Message As String
        Get
            Return _Message
        End Get
        Set(value As String)
            _Message = value
        End Set
    End Property

    Public Type As Integer = 1
    Public SpecialCode As Integer = 0
    Public BookControl As Integer = 0

    Public Distances As New List(Of clsNGLOptStopDistance)

#End Region

#Region "Public Methods"

    'CHANGE LVV 8/7/15
    Public Overrides Function ToString() As String
        Dim strRet = Number & "," & ID & "," & Zip & "," & Address1 & "," & City & "," & State & "," & Country & "," & Cases & "," & Weight & "," & Pallets & "," & Cubes & "," & Type & "," & SpecialCode
        Return strRet
    End Function

    Public Sub getDistance(ByVal stops As List(Of clsNGLOptStopItem))
        For Each s In stops
            Dim d As Double = 0
            If s.Number <> Number Then
                d = Math.Round(oPCMiler.PCMGetFlatMiles(Me.Zip, s.Zip), 0)
            End If
            If d = -1 Then d = 5000
            Distances.Add(New clsNGLOptStopDistance(s.Number, d))
        Next
    End Sub

    Public Sub getDistance(ByVal s As clsNGLOptStopItem)
        Dim d As Double = 0
        If s.Number <> Number Then
            d = Math.Round(oPCMiler.PCMGetFlatMiles(Me.Zip, s.Zip), 2)
        End If
        '-1 indicates that this is the same address for both orig and dest so the distance is always the minimum 1  
        'for now we set this to 1 later we may have the ability to use real numbers so we could set it to 0.001
        If d = -1 Then d = 1
        Distances.Add(New clsNGLOptStopDistance(s.Number, d))
    End Sub

#End Region

End Class


Public Class clsNGLOptStopDistance

    Public StopNumber As Integer
    Public Miles As Double

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal s As Integer, ByVal d As Double)
        StopNumber = s
        Miles = d
    End Sub

End Class