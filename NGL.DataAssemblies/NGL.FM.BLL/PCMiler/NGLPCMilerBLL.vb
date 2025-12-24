Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Data.Linq
Imports System.Data.SqlClient
Imports System.ServiceModel
Imports PCM = NGL.FreightMaster.PCMiler
Imports DAL = NGL.FreightMaster.Data
Imports DTO = NGL.FreightMaster.Data.DataTransferObjects
Imports LTS = NGL.FreightMaster.Data.LTS
Imports DTran = NGL.Core.Utility.DataTransformation
Imports TRM = NGLTrimbleServices
Imports System.Windows.Forms

''' <summary>
''' PC Miler and Trimble API Entry Point
''' </summary>
''' <remarks>
''' Modified by RHR for v-8.4 on 06/08/2021 added logic for Trimble API
''' 
''' </remarks>
Public Class NGLPCMilerBLL : Inherits BLLBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As DAL.WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Me.SourceClass = "NGLPCMilerBLL"
    End Sub

#End Region

#Region " Properties "

    Public gPCMiler As New PCM.PCMiles

    Public gTRMPCMiler As New TRM.PCMiles(False)

    Private _IsPCMilerActive As Boolean
    Public Property IsPCMilerActive() As Boolean
        Get
            Return _IsPCMilerActive
        End Get
        Set(ByVal value As Boolean)
            _IsPCMilerActive = value
        End Set
    End Property
    ''' <summary>
    ''' A list of all parameters by company
    ''' NGLPCMSearchCompControl identifies the company to search for
    ''' NGLPCMParameterCompControl identifies if the company control was found in the parameter table; 
    ''' a value of 0 for NGLPCMParameterCompControl indicates that a company level parameter is not available
    ''' and the system is using the global defaults
    ''' </summary>
    ''' <remarks></remarks>
    Private _NGLPCMParameters As New List(Of LTS.spGetPCMParametersResult)
    Public Property NGLPCMParameters() As List(Of LTS.spGetPCMParametersResult)
        Get
            Return _NGLPCMParameters
        End Get
        Set(ByVal value As List(Of LTS.spGetPCMParametersResult))
            _NGLPCMParameters = value
        End Set
    End Property

    ''' <summary>
    ''' this is the active pcm parameter selected by calling loadPCMParameters with the desired
    ''' company control nunmber
    ''' </summary>
    ''' <remarks></remarks>
    Private _NGLPCMParameter As New LTS.spGetPCMParametersResult
    Public Property NGLPCMParameter() As LTS.spGetPCMParametersResult
        Get
            Return _NGLPCMParameter
        End Get
        Set(ByVal value As LTS.spGetPCMParametersResult)
            _NGLPCMParameter = value
        End Set
    End Property


    ''' <summary>
    ''' Callers must call loadPCMParameters with the correct company control number before 
    ''' attempting to access this property 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property NGLUsePCMiler() As Boolean
        Get
            If Not Me.NGLPCMParameter Is Nothing Then
                Return If(Me.NGLPCMParameter.NGLUsePCMiler, False)
            Else
                Return False
            End If
        End Get
    End Property


    ''' <summary>
    ''' Callers must call loadPCMParameters with the correct company control number before 
    ''' attempting to access this property 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property NGLPCMWebServiceURL() As String
        Get
            If Not Me.NGLPCMParameter Is Nothing Then
                Return If(Me.NGLPCMParameter.NGLPCMWebServiceURL, "")
            Else
                Return ""
            End If
        End Get
    End Property

    ''' <summary>
    ''' Callers must call loadPCMParameters with the correct company control number before 
    ''' attempting to access this property 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property NGLPCMDebugMode() As Boolean
        Get
            If Not Me.NGLPCMParameter Is Nothing Then
                Return If(Me.NGLPCMParameter.NGLPCMDebugMode, False)
            Else
                Return False
            End If
        End Get
    End Property

    ''' <summary>
    ''' Callers must call loadPCMParameters with the correct company control number before 
    ''' attempting to access this property 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property NGLPCMKeepLogDays() As Integer
        Get
            If Not Me.NGLPCMParameter Is Nothing Then
                Return If(Me.NGLPCMParameter.NGLPCMKeepLogDays, 7)
            Else
                Return 7
            End If
        End Get
    End Property

    ''' <summary>
    ''' Callers must call loadPCMParameters with the correct company control number before 
    ''' attempting to access this property 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property NGLPCMSaveOldLog() As Boolean
        Get
            If Not Me.NGLPCMParameter Is Nothing Then
                Return If(Me.NGLPCMParameter.NGLPCMSaveOldLog, False)
            Else
                Return False
            End If
        End Get
    End Property

    ''' <summary>
    ''' Callers must call loadPCMParameters with the correct company control number before 
    ''' attempting to access this property 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property NGLPCMLoggingOn() As Boolean
        Get
            If Not Me.NGLPCMParameter Is Nothing Then
                Return If(Me.NGLPCMParameter.NGLPCMLoggingOn, False)
            Else
                Return False
            End If
        End Get
    End Property

    ''' <summary>
    ''' Callers must call loadPCMParameters with the correct company control number before 
    ''' attempting to access this property 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property NGLPCMLogFile() As String
        Get
            If Not Me.NGLPCMParameter Is Nothing Then
                Return If(Me.NGLPCMParameter.NGLPCMLogFile, "NA")
            Else
                Return "NA"
            End If
        End Get
    End Property

    ''' <summary>
    ''' Callers must call loadPCMParameters with the correct company control number before 
    ''' attempting to access this property 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property NGLPCMUseZipOnly() As Boolean
        Get
            If Not Me.NGLPCMParameter Is Nothing Then
                Return If(Me.NGLPCMParameter.NGLPCMUseZipOnly, False)
            Else
                Return False
            End If
        End Get
    End Property

    ''' <summary>
    ''' Callers must call loadPCMParameters with the correct company control number before 
    ''' attempting to access this property 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property NGLPCMRouteType() As Integer
        Get
            If Not Me.NGLPCMParameter Is Nothing Then
                Return If(Me.NGLPCMParameter.NGLPCMRouteType, 1)
            Else
                Return 1
            End If
        End Get
    End Property

    ''' <summary>
    ''' Callers must call loadPCMParameters with the correct company control number before 
    ''' attempting to access this property 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property NGLPCMDistType() As Integer
        Get
            If Not Me.NGLPCMParameter Is Nothing Then
                Return If(Me.NGLPCMParameter.NGLPCMDistType, 0)
            Else
                Return 0
            End If
        End Get
    End Property



    Private _TrimbleAPIURL As String
    ''' <summary>
    ''' Callers must use getTrimbleAPISettings to populate data
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.4 on 06/08/2021 Trimble API Settings
    ''' </remarks>
    Public Property TrimbleAPIURL() As String
        Get
            Return _TrimbleAPIURL
        End Get
        Set(ByVal value As String)
            _TrimbleAPIURL = value
        End Set
    End Property

    Private _TrimbleAPIKey As String ' leave blank so system will read from db ssoa account = "C36349D0A5F5D440AAC0CB8A0287F02C" ' default for testing
    ''' <summary>
    ''' Callers must use getTrimbleAPISettings to populate data
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.4 on 06/08/2021 Trimble API Settings
    ''' </remarks>
    Public Property TrimbleAPIKey() As String
        Get
            Return _TrimbleAPIKey
        End Get
        Set(ByVal value As String)
            _TrimbleAPIKey = value
        End Set
    End Property

    Private _TrimbleAPIKeyQueryString As String
    ''' <summary>
    ''' Callers must use getTrimbleAPISettings to populate data
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.4 on 06/08/2021 Trimble API Settings
    ''' </remarks>
    Public Property TrimbleAPIKeyQueryString() As String
        Get
            Return _TrimbleAPIKeyQueryString
        End Get
        Set(ByVal value As String)
            _TrimbleAPIKeyQueryString = value
        End Set
    End Property

    Private _UseTrimbleAPI As Boolean
    ''' <summary>
    ''' Callers must use getTrimbleAPISettings to populate data
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.4 on 06/08/2021 Trimble API Settings
    ''' </remarks>
    Public Property UseTrimbleAPI() As Boolean
        Get
            Return _UseTrimbleAPI
        End Get
        Set(ByVal value As Boolean)
            _UseTrimbleAPI = value
        End Set
    End Property

    Private _TrimbleUserControl As Integer
    ''' <summary>
    ''' Used to Cache Trimble Settings by user.  If the user has not changed we can use the current properties.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.4 on 06/08/2021 Trimble API Settings
    ''' </remarks>
    Public Property TrimbleUserControl() As Integer
        Get
            Return _TrimbleUserControl
        End Get
        Set(ByVal value As Integer)
            _TrimbleUserControl = value
        End Set
    End Property

#End Region


#Region " Delegates "
    'Delegate Function
    Public Delegate Sub ProcessBadAddressesDelegate(ByVal oPCMBadAddresses As PCM.clsPCMBadAddresses)

#End Region
#Region "DAL Wrapper Methods"

    Public Function GetParameterList() As DTO.Parameter()
        Return NGLParameterData.GetRecordsFiltered()
    End Function

    Public Function GetCompParameterValue(ByVal key As String, Optional ByVal compControl As Integer = 0) As Double
        Return NGLCompParameterRefSystemData.GetCompParameterFiltered(CompControl:=compControl, CompParKey:=key).CompParValue
    End Function

    Public Function GetCompParameterText(ByVal key As String, Optional ByVal compControl As Integer = 0) As String
        Return NGLCompParameterRefSystemData.GetCompParameterFiltered(CompControl:=compControl, CompParKey:=key).CompParText
    End Function

    Public Function GetParameterValue(ByVal key As String) As Double
        Return NGLParameterData.GetParValue(key)
    End Function

    Public Function GetParameterText(ByVal key As String) As String
        Return NGLParameterData.GetParText(key)
    End Function

    Public Function selectDTOPCMAddressData(ByVal clsAddress As Object) As DTO.PCMAddress
        Dim oDTO As New DTO.PCMAddress
        Dim skipObjs As New List(Of String)
        oDTO = DTran.CopyMatchingFields(oDTO, clsAddress, skipObjs)
        Return oDTO
    End Function

    Public Function selectTrmFMStopDataData(ByVal source As PCM.clsFMStopData) As TRM.clsFMStopData
        Dim oReturn As New TRM.clsFMStopData
        Dim skipObjs As New List(Of String)
        oReturn = DTran.CopyMatchingFields(oReturn, source, skipObjs)
        Return oReturn
    End Function

    Public Function selectTrmBadAddressData(ByVal source As PCM.clsPCMBadAddress) As TRM.clsPCMBadAddress
        Dim oReturn As New TRM.clsPCMBadAddress
        Dim skipObjs As New List(Of String)
        oReturn = DTran.CopyMatchingFields(oReturn, source, skipObjs)
        Return oReturn
    End Function

    Public Function selectPCMBadAddressData(ByVal source As TRM.clsPCMBadAddress) As PCM.clsPCMBadAddress
        Dim oReturn As New PCM.clsPCMBadAddress
        Dim skipObjs As New List(Of String)
        oReturn = DTran.CopyMatchingFields(oReturn, source, skipObjs)
        Return oReturn
    End Function

    Public Function selectTrmReportRecordData(ByVal source As PCM.clsPCMReportRecord) As TRM.clsPCMReportRecord
        Dim oReturn As New TRM.clsPCMReportRecord
        Dim skipObjs As New List(Of String)
        oReturn = DTran.CopyMatchingFields(oReturn, source, skipObjs)
        Return oReturn
    End Function

    Public Function selectPCMReportRecordData(ByVal source As TRM.clsPCMReportRecord) As PCM.clsPCMReportRecord
        Dim oReturn As New PCM.clsPCMReportRecord
        Dim skipObjs As New List(Of String)
        oReturn = DTran.CopyMatchingFields(oReturn, source, skipObjs)
        Return oReturn
    End Function


    ''' <summary>
    ''' Copies the clsAllStop results from PCMiler to the DTO PCMAllStops object
    ''' </summary>
    ''' <param name="pcmClsAllStops"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR 2/8/2016 v-7.0.5.0
    '''   fixed bug where null reference exception was being thrown if the pcmClsAllStops object was nothing
    '''   this could happen if the URL to PC Miler server was wrong or not accessible.  by throwing the 
    '''   null reference exception the user was never notified that the URL was wrong so they could not 
    '''   correct the problem, typically a parameter setting.  by checking for nothing the previously defined
    '''   exception handleres will display a messages about the invalid URL to PC Miler or other associated 
    '''   messages
    ''' </remarks>
    Public Function selectDTOData(ByVal pcmClsAllStops As PCM.clsAllStops) As DTO.PCMAllStops
        Dim oDTO As New DTO.PCMAllStops
        If Not pcmClsAllStops Is Nothing Then 'Modified by RHR 2/8/2016 v-7.0.5.0
            With oDTO
                .AutoCorrectBadLaneZipCodes = pcmClsAllStops.AutoCorrectBadLaneZipCodes
                .BadAddressCount = pcmClsAllStops.BadAddressCount
                .BatchID = pcmClsAllStops.BatchID
                .DestZip = pcmClsAllStops.DestZip
                .FailedAddressMessage = pcmClsAllStops.FailedAddressMessage
                .LastError = pcmClsAllStops.LastError
                .OriginZip = pcmClsAllStops.OriginZip
                .TotalMiles = pcmClsAllStops.TotalMiles
            End With
        End If
        Return oDTO
    End Function

    Public Function selectDTODataFromTRM(ByVal pcmClsAllStops As TRM.clsAllStops) As PCM.clsAllStops
        Dim oData As New PCM.clsAllStops
        If Not pcmClsAllStops Is Nothing Then 'Modified by RHR 2/8/2016 v-7.0.5.0
            With oData
                .AutoCorrectBadLaneZipCodes = pcmClsAllStops.AutoCorrectBadLaneZipCodes
                .BadAddressCount = pcmClsAllStops.BadAddressCount
                .BatchID = pcmClsAllStops.BatchID
                .DestZip = pcmClsAllStops.DestZip
                .FailedAddressMessage = pcmClsAllStops.FailedAddressMessage
                .OriginZip = pcmClsAllStops.OriginZip
                .TotalMiles = pcmClsAllStops.TotalMiles
            End With
        End If
        Return oData
    End Function

    Public Function convertTRMBadAddressToPCM(arrBadAddresses() As TRM.clsPCMBadAddress) As PCM.clsPCMBadAddresses
        Dim BaddAddresses As New PCM.clsPCMBadAddresses()
        If Not arrBadAddresses Is Nothing AndAlso arrBadAddresses.Length > 0 Then
            For i As Integer = 0 To arrBadAddresses.Length - 1
                If (Not arrBadAddresses(i) Is Nothing AndAlso Not arrBadAddresses(i).objOrig Is Nothing) Then
                    Dim oBadOrig As New PCM.clsAddress
                    With oBadOrig
                        .strAddress = arrBadAddresses(i).objOrig.strAddress
                        .strCity = arrBadAddresses(i).objOrig.strCity
                        .strState = arrBadAddresses(i).objOrig.strState
                        .strZip = arrBadAddresses(i).objOrig.strZip
                    End With
                    Dim oBadDest As New PCM.clsAddress
                    If (Not Not arrBadAddresses(i).objDest Is Nothing) Then
                        With oBadDest
                            .strAddress = arrBadAddresses(i).objDest.strAddress
                            .strCity = arrBadAddresses(i).objDest.strCity
                            .strState = arrBadAddresses(i).objDest.strState
                            .strZip = arrBadAddresses(i).objDest.strZip
                        End With
                    End If

                    Dim oBadPCMOrig As New PCM.clsAddress
                    If (Not Not arrBadAddresses(i).objPCMOrig Is Nothing) Then
                        With oBadPCMOrig
                            .strAddress = arrBadAddresses(i).objPCMOrig.strAddress
                            .strCity = arrBadAddresses(i).objPCMOrig.strCity
                            .strState = arrBadAddresses(i).objPCMOrig.strState
                            .strZip = arrBadAddresses(i).objPCMOrig.strZip
                        End With
                    End If

                    Dim oBadPCMDest As New PCM.clsAddress
                    If (Not Not arrBadAddresses(i).objPCMDest Is Nothing) Then
                        With oBadPCMDest
                            .strAddress = arrBadAddresses(i).objPCMDest.strAddress
                            .strCity = arrBadAddresses(i).objPCMDest.strCity
                            .strState = arrBadAddresses(i).objPCMDest.strState
                            .strZip = arrBadAddresses(i).objPCMDest.strZip
                        End With
                    End If

                    BaddAddresses.Add(arrBadAddresses(i).BookControl,
                        arrBadAddresses(i).LaneControl,
                        oBadOrig,
                        oBadDest,
                        oBadPCMOrig,
                        oBadPCMDest,
                        arrBadAddresses(i).Message,
                        arrBadAddresses(i).BatchID)
                End If

            Next
        End If
        Return BaddAddresses
    End Function
#End Region

#Region " Trimble to PCM Convrters"

    'Private Function convertclsAddress() As Boolean

    'End Function
#End Region


#Region " Public Methods"
    ''' <summary>
    ''' Returns a list of PCM or Trimble Address from the postalCode string provided
    ''' </summary>
    ''' <param name="postalCode"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.4 on 06/08/2021 added logic for Trimble API
    ''' </remarks>
    Public Function cityStateZipLookup(ByVal postalCode As String) As DTO.PCMAddress()
        Dim pcmResult As PCM.clsAddress()
        Dim oRet As DTO.PCMAddress()
        loadPCMParameters()
        Try
            If getTrimbleAPISettings() Then
                'Note: we should be passing in all of the PCM Parameters to the gTRMPCMiler object
                '       check on how this is being used.
                Dim bUseTLS12 As Boolean = If(NGLParameterData.GetParValue("GlobalTurnOnTLs12ForAPIs", 0) = 0, False, True)
                gTRMPCMiler = New TRM.PCMiles(bUseTLS12)
                gTRMPCMiler.APIKey = Me.TrimbleAPIKey
                Dim trmResults = gTRMPCMiler.cityStateZipLookup(postalCode, Me.NGLPCMLoggingOn, Me.NGLPCMLogFile)
                If trmResults Is Nothing Then Return Nothing
                oRet = (From d In trmResults Select selectDTOPCMAddressData(d)).ToArray()
            Else
                pcmResult = gPCMiler.cityStateZipLookup(postalCode, Me.NGLPCMLoggingOn, Me.NGLPCMLogFile)
                If pcmResult Is Nothing Then Return Nothing
                oRet = (From d In pcmResult Select selectDTOPCMAddressData(d)).ToArray()
            End If
        Catch ex As Exception
            'some sort of unknow error.
            'lets log it and move on.
            logSystemError(ex, "NGLPCMilerBLL.cityStateZipLookup", "postalCode=" & postalCode, FreightMaster.Data.sysErrorParameters.sysErrorState.UserLevelFault, FreightMaster.Data.sysErrorParameters.sysErrorSeverity.Unexpected)
            Return Nothing
        End Try
        If Not String.IsNullOrEmpty(gPCMiler.LastError) Then
            throwInvalidOperatonException(String.Format("PC Miler city state by zip lookup Error for postal code {0} : {1}", postalCode, gPCMiler.LastError))
        End If
        Return oRet
    End Function

    ''' <summary>
    ''' Returns a PCM or Trimble Full Address from the provided address string
    ''' </summary>
    ''' <param name="addressstring"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.4 on 06/08/2021 added logic for Trimble API
    ''' </remarks>
    Public Function GetAddressFirstMatch(ByVal addressstring As String) As String
        loadPCMParameters()
        Dim match As String = ""
        Dim strLastError As String = ""
        If getTrimbleAPISettings() Then
            Dim bUseTLS12 As Boolean = If(NGLParameterData.GetParValue("GlobalTurnOnTLs12ForAPIs", 0) = 0, False, True)
            gTRMPCMiler = New TRM.PCMiles(bUseTLS12)
            gTRMPCMiler.APIKey = Me.TrimbleAPIKey
            match = gTRMPCMiler.FullName(addressstring, False, "")
            strLastError = gTRMPCMiler.LastError
        Else
            match = gPCMiler.FullName(addressstring, False, "")
            strLastError = gPCMiler.LastError
        End If

        If Not String.IsNullOrEmpty(strLastError) Then
            throwInvalidOperatonException(String.Format("Unable to find a matching address for {0} : {1}", addressstring, strLastError))
        End If
        Return match
    End Function

    ''' <summary>
    ''' Must be called after loadPCMParameters is called
    ''' </summary>
    ''' <param name="routeType"></param>
    ''' <param name="distanceType"></param>
    ''' <remarks></remarks>
    Public Sub loadCustomParameters(Optional ByVal routeType As Integer = -1, Optional ByVal distanceType As Integer = -1)
        If routeType <> -1 And Not Me.NGLPCMParameter Is Nothing Then 'use custom setting
            Me.NGLPCMParameter.NGLPCMRouteType = routeType
        End If
        If distanceType <> -1 And Not Me.NGLPCMParameter Is Nothing Then 'use custome distance type 
            Me.NGLPCMParameter.NGLPCMDistType = distanceType
        End If
    End Sub


    ''' <summary>
    ''' Searches for an existing match in the NGLPCMParameters list or reads from the database
    ''' loading the first match into the NGLPCMParameter object adding new items to the list as 
    ''' needed.  Improves performance by limiting the number of calls to the database in batch operations
    ''' when multiple calls to pcmiler are needed.
    ''' </summary>
    ''' <param name="CompControl"></param>
    ''' <remarks></remarks>
    Public Sub loadPCMParameters(Optional ByVal CompControl As Integer = 0)
        'for now we always set conmpcontrol to zero
        CompControl = 0
        If Me.NGLPCMParameters Is Nothing OrElse Me.NGLPCMParameters.Count() < 1 Then
            'get the parameters fronm the database else we use the current settings
            Me.NGLPCMParameter = NGLParameterData.GetPCMParameters(CompControl)
            Me.NGLPCMParameters.Add(NGLPCMParameter)
        Else
            'see if we have an existing match
            If Me.NGLPCMParameters.Any(Function(x) x.NGLPCMSearchCompControl = CompControl) Then
                Me.NGLPCMParameter = Me.NGLPCMParameters.Where(Function(x) x.NGLPCMSearchCompControl = CompControl).FirstOrDefault()
            Else
                'load from the database
                Me.NGLPCMParameter = NGLParameterData.GetPCMParameters(CompControl)
                Me.NGLPCMParameters.Add(NGLPCMParameter)
            End If
        End If
        With Me.NGLPCMParameter
            gPCMiler.WebServiceURL = If(.NGLPCMWebServiceURL, "")
            gPCMiler.Debug = If(.NGLPCMDebugMode, False)
            gPCMiler.KeepLogDays = If(.NGLPCMKeepLogDays, 7)
            gPCMiler.SaveOldLog = If(.NGLPCMSaveOldLog, False)
            gPCMiler.UseZipOnly = If(.NGLPCMUseZipOnly, False)
        End With
    End Sub

    Public Function getParValue(ByVal ParKey As String, ByRef oData() As DTO.Parameter, Optional ByVal CompControl As Integer = 0) As Double
        Dim dblRet As Double = 0

        If oData Is Nothing Then
            Return dblRet
        End If

        Dim pars = From p In oData Where p.ParKey = ParKey
        If Not pars Is Nothing AndAlso pars.Count > 0 Then
            'we have a match so use the first item in the list (there should be no duplicates anyway)
            Dim opar As DTO.Parameter = pars(0)
            If CompControl = 0 OrElse opar.ParIsGlobal = True Then
                'no company number just return the main result
                dblRet = opar.ParValue
            Else
                'look up the value in the compParameter list
                Dim oCompPars = From d In opar.CompParameters Where d.CompParCompControl = CompControl
                If Not oCompPars Is Nothing AndAlso oCompPars.Count > 0 Then
                    'return the first item in the list (there should be no duplicates anyway)
                    dblRet = oCompPars(0).CompParValue
                Else
                    'just return the global value
                    dblRet = opar.ParValue
                End If
            End If
        End If

        Return dblRet

    End Function

    Public Function getParText(ByVal ParKey As String, ByRef oData() As DTO.Parameter, Optional ByVal CompControl As Integer = 0) As String
        Dim strRet As String = ""
        If oData Is Nothing Then
            Return strRet
        End If

        Dim pars = From p In oData Where p.ParKey.ToUpper = ParKey.ToUpper
        If Not pars Is Nothing AndAlso pars.Count > 0 Then
            'we have a match so use the first item in the list (there should be no duplicates anyway)
            Dim opar As DTO.Parameter = pars(0)
            If CompControl = 0 OrElse opar.ParIsGlobal = True Then
                'no company number just return the main result
                strRet = opar.ParText
            Else
                'look up the value in the compParameter list
                Dim oCompPars = From d In opar.CompParameters Where d.CompParCompControl = CompControl
                If Not oCompPars Is Nothing AndAlso oCompPars.Count > 0 Then
                    'return the first item in the list (there should be no duplicates anyway)
                    strRet = oCompPars(0).CompParText
                Else
                    'just return the global value
                    strRet = opar.ParText
                End If
            End If
        End If

        Return strRet

    End Function

    Public Function SetIsPCMilerActive() As Boolean
        Me.IsPCMilerActive = NGLUsePCMiler
        Return NGLUsePCMiler
    End Function

    Public Function concateAddress(ByVal oAddress As PCM.clsAddress) As String
        If oAddress Is Nothing Then Return "N/A"
        Return String.Format("{0}, {1}, {2}  {3}", oAddress.strAddress, oAddress.strCity, oAddress.strState, oAddress.strZip)
    End Function
    Public Function concateAddress(ByVal oAddress As TRM.clsAddress) As String
        If oAddress Is Nothing Then Return "N/A"
        Return String.Format("{0}, {1}, {2}  {3}", oAddress.strAddress, oAddress.strCity, oAddress.strState, oAddress.strZip)
    End Function

    Public Function concateAddress(ByVal sStreet As String, sCity As String, sState As String, sZip As String, sCountry As String) As String

        Return String.Format("{0}, {1}, {2}  {3}  {4}", sStreet, sCity, sState, sZip, sCountry)
    End Function

    ''' <summary>
    ''' Calls PCMiler or Trimble API to get the practical miles for a load
    ''' </summary>
    ''' <param name="oPCMOrig"></param>
    ''' <param name="oPCMDest"></param>
    ''' <param name="routeType"></param>
    ''' <param name="distanceType"></param>
    ''' <param name="intCompControl"></param>
    ''' <param name="intBookControl"></param>
    ''' <param name="intLaneControl"></param>
    ''' <param name="strItemNumber"></param>
    ''' <param name="strItemType"></param>
    ''' <param name="dblAutoCorrectBadLaneZipCodes"></param>
    ''' <param name="dblBatchID"></param>
    ''' <param name="blnBatch"></param>
    ''' <param name="BaddAddresses"></param>
    ''' <param name="LoggingOn"></param>
    ''' <param name="LogFileName"></param>
    ''' <param name="sLastError"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.4.x on 02/05/2022 fixed missing street address
    ''' </remarks>
    Public Function GetPracticalMiles(ByVal oPCMOrig As PCM.clsAddress,
                                      ByVal oPCMDest As PCM.clsAddress,
                                      ByVal routeType As Integer,
                                      ByVal distanceType As Integer,
                                      ByVal intCompControl As Integer,
                                      ByVal intBookControl As Integer,
                                      ByVal intLaneControl As Integer,
                                      ByVal strItemNumber As String,
                                      ByVal strItemType As String,
                                      ByVal dblAutoCorrectBadLaneZipCodes As Double,
                                      ByVal dblBatchID As Double,
                                      ByVal blnBatch As Boolean,
                                      ByRef BaddAddresses As PCM.clsPCMBadAddresses,
                                      ByVal LoggingOn As Boolean,
                                      ByVal LogFileName As String,
                                      Optional ByRef sLastError As String = "") As PCM.clsAllStops

        loadPCMParameters()

        loadCustomParameters(routeType, distanceType)
        Dim oPCMAllStops As New PCM.clsAllStops
        If getTrimbleAPISettings() Then
            Dim oTRMOrig As New TRM.clsAddress ' PCM_clsAddress
            Dim oTRMDest As New TRM.clsAddress
            Dim oPCMBadAddresses As New PCM.clsPCMBadAddresses
            Dim arrBadAddresses(1) As TRM.clsPCMBadAddress
            oTRMOrig.strZip = oPCMOrig.strZip
            oTRMOrig.strCity = oPCMOrig.strCity
            oTRMOrig.strState = oPCMOrig.strState
            oTRMOrig.strAddress = oPCMOrig.strAddress
            'oTRMOrig.strCountry = oPCMOrig.str

            oTRMDest.strZip = oPCMDest.strZip
            oTRMDest.strCity = oPCMDest.strCity
            oTRMDest.strState = oPCMDest.strState
            oTRMDest.strAddress = oPCMDest.strAddress ' Modified by RHR for v-8.4.x on 02/05/20224

            Dim bUseTLS12 As Boolean = If(NGLParameterData.GetParValue("GlobalTurnOnTLs12ForAPIs", 0) = 0, False, True)
            Dim TRMPCMiles = New TRM.PCMiles(bUseTLS12)
            TRMPCMiles.APIKey = Me.TrimbleAPIKey
            Dim oData = TRMPCMiles.getPracticalMiles(TrimbleAPIKey, oTRMOrig, oTRMDest, NGLPCMRouteType, NGLPCMDistType, 0, 0, 0, intCompControl, intBookControl, intLaneControl, strItemNumber, strItemType, dblAutoCorrectBadLaneZipCodes, dblBatchID, blnBatch, arrBadAddresses)
            sLastError = TRMPCMiles.LastError
            oPCMAllStops = selectDTODataFromTRM(oData)
            If (sLastError.Length > 0 AndAlso oPCMAllStops.TotalMiles = 0 AndAlso arrBadAddresses(0) Is Nothing) Then
                BaddAddresses.Add(intBookControl, intLaneControl, oPCMOrig, oPCMDest, Nothing, Nothing, sLastError, dblBatchID)
            Else
                BaddAddresses = convertTRMBadAddressToPCM(arrBadAddresses)
            End If

        Else
            oPCMAllStops = gPCMiler.getPracticalMiles(oPCMOrig,
                                                                 oPCMDest,
                                                                 routeType,
                                                                 distanceType,
                                                                 intCompControl,
                                                                 intBookControl,
                                                                 intLaneControl,
                                                                 strItemNumber,
                                                                 strItemType,
                                                                 dblAutoCorrectBadLaneZipCodes,
                                                                 dblBatchID,
                                                                 blnBatch,
                                                                 BaddAddresses,
                                                                 LoggingOn,
                                                                 LogFileName)
            sLastError = oPCMAllStops.LastError
        End If

        Return oPCMAllStops
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="record"></param>
    ''' <param name="routeType"></param>
    ''' <param name="distanceType"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.4.x on 02/05/2022 fixed missing street address
    ''' </remarks>
    Public Function GetPracticalMiles(ByVal record As DTO.BookRevenue, Optional ByVal routeType As Integer = -1, Optional ByVal distanceType As Integer = -1) As DTO.PCMAllStops
        If record Is Nothing Then Return Nothing

        Dim oPCMOrig As New PCM.clsAddress ' PCM_clsAddress
        Dim oPCMDest As New PCM.clsAddress
        Dim oPCMBadAddresses As New PCM.clsPCMBadAddresses

        oPCMOrig.strZip = record.BookOrigZip
        oPCMOrig.strCity = record.BookOrigCity
        oPCMOrig.strState = record.BookOrigState
        oPCMOrig.strAddress = record.BookOrigAddress1 ' Modified by RHR for v-8.4.x on 02/05/2022

        oPCMDest.strZip = record.BookDestZip
        oPCMDest.strCity = record.BookDestCity
        oPCMDest.strState = record.BookDestState
        oPCMDest.strAddress = record.BookDestAddress1 ' Modified by RHR for v-8.4.x on 02/05/2022
        Dim dblBatchID As Double
        dblBatchID = CDbl(Format(Now(), "MddyyyyHHmmss"))

        Dim oData As DTO.PCMAllStops = selectDTOData(Me.GetPracticalMiles(oPCMOrig,
                                                                 oPCMDest,
                                                                 Me.NGLPCMRouteType,
                                                                 Me.NGLPCMDistType,
                                                                 record.BookCustCompControl,
                                                                 record.BookControl,
                                                                 record.BookODControl,
                                                                 "",
                                                                 "",
                                                                 0,
                                                                 dblBatchID,
                                                                 False,
                                                                 oPCMBadAddresses,
                                                                 Me.NGLPCMLoggingOn,
                                                                 Me.NGLPCMLogFile))

        If Not String.IsNullOrEmpty(oData.LastError) Then
            throwInvalidOperatonException(String.Format("PC Miler Get Distance Error from {0} to {1} : {2}",
                                                        concateAddress(oPCMOrig), concateAddress(oPCMDest), oData.LastError))
        End If

        Return oData

    End Function

    ''' <summary>
    ''' strAddress must be in the following format:
    ''' Zip  City, State; Street 
    ''' </summary>
    ''' <param name="strAddress"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function PCMValidateAddress(ByVal strAddress As String) As Boolean
        Dim blnRet As Boolean = False
        Try
            loadPCMParameters()
            If getTrimbleAPISettings() Then
                Dim bUseTLS12 As Boolean = If(NGLParameterData.GetParValue("GlobalTurnOnTLs12ForAPIs", 0) = 0, False, True)
                Dim TRMPCMiles = New TRM.PCMiles(bUseTLS12)
                TRMPCMiles.APIKey = Me.TrimbleAPIKey
                blnRet = TRMPCMiles.PCMValidateAddress(strAddress)
                If Not String.IsNullOrEmpty(TRMPCMiles.LastError) Then
                    throwInvalidOperatonException(String.Format("PC Miler ValidateAddress Error for {0} : {1}", strAddress, TRMPCMiles.LastError))
                End If
            Else
                blnRet = gPCMiler.PCMValidateAddress(strAddress)
                If Not String.IsNullOrEmpty(gPCMiler.LastError) Then
                    throwInvalidOperatonException(String.Format("PC Miler ValidateAddress Error for {0} : {1}", strAddress, gPCMiler.LastError))
                End If
            End If


        Catch ex As FaultException
            Throw
        Catch ex As InvalidOperationException
            Throw New ApplicationException(String.Format("PC Miler Connection Error for {0}", strAddress, ex.Message))

        Catch ex As Exception
            Throw New ApplicationException(String.Format("PC Miler Get Miles Error for {0}", strAddress, ex.Message))
        End Try
        Return blnRet
    End Function

    Public Function PCMGetFlatMiles(ByVal record As DTO.BookRevenue, Optional ByVal routeType As Integer = -1) As Double
        'Dim origin As String = firstRec.BookOrigCity & ", " & firstRec.BookOrigState & " " & firstRec.BookOrigZip & " " & firstRec.BookOrigCountry
        'Dim dest As String = firstRec.BookDestCity & ", " & firstRec.BookDestState & " " & firstRec.BookDestZip & " " & firstRec.BookDestCountry
        Dim origin As String = record.BookOrigZip & " " & record.BookOrigCity & ", " & record.BookOrigState
        Dim dest As String = record.BookDestZip & " " & record.BookDestCity & ", " & record.BookDestState
        Return PCMGetFlatMiles(origin, dest)
    End Function

    Public Function PCMGetFlatMiles(ByVal origin As String, ByVal dest As String) As Double
        Dim dMiles As Double = 0
        Try
            loadPCMParameters()
            If getTrimbleAPISettings() Then
                Dim bUseTLS12 As Boolean = If(NGLParameterData.GetParValue("GlobalTurnOnTLs12ForAPIs", 0) = 0, False, True)
                Dim TRMPCMiles = New TRM.PCMiles(bUseTLS12)
                TRMPCMiles.APIKey = Me.TrimbleAPIKey
                dMiles = TRMPCMiles.Miles(origin, dest)
                If Not String.IsNullOrEmpty(TRMPCMiles.LastError) Then
                    throwInvalidOperatonException(String.Format("PC Miler Miles Error for orig {0} to dest {3} : {1}", origin, TRMPCMiles.LastError, dest))
                End If
            Else
                dMiles = gPCMiler.Miles(origin, dest, NGLPCMLoggingOn, NGLPCMLogFile)
                If Not String.IsNullOrEmpty(gPCMiler.LastError) Then
                    Throw New ApplicationException(String.Format("PC Miler Miles Error for orig {0} to dest {1} : {2}", origin, dest, gPCMiler.LastError))
                End If
            End If

        Catch ex As InvalidOperationException
            Throw New ApplicationException(String.Format("PC Miler Connection Error for {0} to {1} : {2}", origin, dest, ex.Message))

        Catch ex As Exception
            Throw New ApplicationException(String.Format("PC Miler Get Miles Error for {0} to {1} : {2}", origin, dest, ex.Message))
            'Throw
        End Try



        Return dMiles
    End Function

    Public Function getGeoCode(ByVal strlocation As String, ByRef dblLat As Double, ByRef dblLong As Double, ByRef LastError As String, ByVal blnLoggingOn As Boolean, ByVal strPCMilerLogFile As String) As Boolean
        Dim blnRet As Boolean = False
        Try
            loadPCMParameters()
            If getTrimbleAPISettings() Then
                Dim bUseTLS12 As Boolean = If(NGLParameterData.GetParValue("GlobalTurnOnTLs12ForAPIs", 0) = 0, False, True)
                Dim TRMPCMiles = New TRM.PCMiles(bUseTLS12)
                TRMPCMiles.APIKey = Me.TrimbleAPIKey
                blnRet = TRMPCMiles.getGeoCode(strlocation, dblLat, dblLong)
                LastError = TRMPCMiles.LastError
            Else
                blnRet = gPCMiler.getGeoCode(strlocation, dblLat, dblLong, blnLoggingOn, strPCMilerLogFile)
                LastError = gPCMiler.LastError
            End If

        Catch ex As FaultException
            Throw
        Catch ex As InvalidOperationException
            Throw New ApplicationException(String.Format("PC Miler Error for {0}", strlocation, ex.Message))

        Catch ex As Exception
            Throw New ApplicationException(String.Format("PC Miler Error for {0}", strlocation, ex.Message))
        End Try
        Return blnRet
    End Function

    Function PCMReSyncMultiStop(ByVal BookConsPrefix As String,
                      ByVal dblBatchID As Double,
                      ByRef sPCMErrors As List(Of String),
                      Optional ByVal blnKeepStopNumbers As Boolean = False,
                      Optional ByVal Silent As Boolean = False,
                      Optional ByVal SortByStopNumber As Boolean = False,
                      Optional ByRef oBadAddresses As PCM.clsPCMBadAddresses = Nothing) As PCM.clsPCMReturnEx
        Dim oPCMReturn As New PCM.clsPCMReturnEx
        Dim intCompControl As Integer
        If sPCMErrors Is Nothing Then sPCMErrors = New List(Of String)
        Dim oFMStopData As New List(Of PCM.clsFMStopData)
        If oBadAddresses Is Nothing Then oBadAddresses = New PCM.clsPCMBadAddresses
        Dim oPCMReportRecords As New List(Of PCM.clsPCMReportRecord)
        Dim strLastError As String = ""
        Dim Results As DTO.vBookMultiPick() = NGLvBookMultiPickData.GetvBookMultiPickRecords(BookConsPrefix)
        If Results Is Nothing OrElse Results.Count() < 1 Then Return Nothing
        'get the PCMiler settings for the first stop normally this is the origin we can only have one set of values
        intCompControl = Results(0).MultiPickCustCompControl
        loadPCMParameters(intCompControl)
        'Dim intStopNumber As Integer
        For Each oStop In Results
            Dim oFMStop As New PCM.clsFMStopData
            With oFMStop
                .BookControl = oStop.MultiPickBookControl
                .BookCustCompControl = oStop.MultiPickCustCompControl
                .BookODControl = oStop.MultiPickODControl
                .BookProNumber = oStop.MultiPickProNumber
                .City = oStop.MultiPickCity
                .DistType = Me.NGLPCMDistType
                .LocationisOrigin = oStop.MultiPickLocationisOrigin
                .RouteType = Me.NGLPCMRouteType
                .State = oStop.MultiPickState
                .StopNumber = oStop.MultiPickStopNumber
                .Street = oStop.MultiPickAddress1
                .Zip = oStop.MultiPickZip
                .Country = oStop.MultiPickCountry
            End With
            oFMStopData.Add(oFMStop)
        Next
        If getTrimbleAPISettings() Then
            ' Call Trimble API Wrapper
            oPCMReturn = TRMReSyncMultiStop(oFMStopData, oBadAddresses, oPCMReportRecords, dblBatchID, blnKeepStopNumbers, strLastError)
        Else
            'send  the data to PC Miler
            oPCMReturn = gPCMiler.PCMReSyncMultiStop(oFMStopData, oBadAddresses, oPCMReportRecords, dblBatchID, blnKeepStopNumbers, Me.NGLPCMLoggingOn, Me.NGLPCMLogFile)
        End If
        If Not String.IsNullOrWhiteSpace(strLastError) Then sPCMErrors.Add(strLastError)
        'test for errors
        If oPCMReturn Is Nothing And Not String.IsNullOrWhiteSpace(gPCMiler.LastError) Then
            sPCMErrors.Add(gPCMiler.LastError)
            Return Nothing
        Else
            If Not String.IsNullOrWhiteSpace(gPCMiler.LastError) Then
                sPCMErrors.Add(gPCMiler.LastError)
                Return Nothing
            End If
            If oBadAddresses.COUNT > 0 Then
                If Silent Then
                    oPCMReturn.BadAddressCount = oBadAddresses.COUNT
                    logBadAddressesAsync(oBadAddresses)
                Else
                    logBadAddresses(oBadAddresses, oPCMReturn)
                End If
            End If
        End If
        Return oPCMReturn
    End Function

    ''' <summary>
    ''' Trimble Multi-stop resequence 
    ''' </summary>
    ''' <param name="StopData"></param>
    ''' <param name="BadAddresses"></param>
    ''' <param name="ReportRecords"></param>
    ''' <param name="dblBatchID"></param>
    ''' <param name="blnKeepStopNumbers"></param>
    ''' <param name="strLastError"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.5.2.004 on 07/21/2022 added default Results = to  stopdata 
    ''' </remarks>
    Public Function TRMReSyncMultiStop(ByVal StopData As List(Of PCM.clsFMStopData),
                               ByRef BadAddresses As PCM.clsPCMBadAddresses,
                               ByRef ReportRecords As List(Of PCM.clsPCMReportRecord),
                               ByVal dblBatchID As Double,
                               ByVal blnKeepStopNumbers As Boolean,
                               ByRef strLastError As String) As PCM.clsPCMReturnEx

        'If Me.Debug Then Return PCMReSyncMultiStopDebug(StopData, BadAddresses, ReportRecords, dblBatchID, blnKeepStopNumbers, LoggingOn, LogFileName)
        Dim oPCMReturnEx As New PCM.clsPCMReturnEx()
        With oPCMReturnEx
            .AutoCorrectBadLaneZipCodes = GetParameterValue("AutoCorrectBadLaneZipCodes")
            .BatchID = dblBatchID
            'Modified by RHR for v-8.5.2.004 on 07/21/2022 added default Results = to  stopdata 
            .Results = StopData
        End With
        Dim trmFMStops = (From d In StopData Select selectTrmFMStopDataData(d)).ToArray()
        Dim trmPCMReportRecords As TRM.clsPCMReportRecord()
        Dim oGlobalStopData As New NGL.FreightMaster.PCMiler.PCM.clsGlobalStopData()
        strLastError = ""
        Dim Stops = New List(Of TRM.TrimbleServiceReference.StopLocation)
        Dim lBadAddresses = New List(Of TRM.TrimbleServiceReference.StopLocation)
        Dim lMatchingAddresses = New List(Of TRM.TrimbleServiceReference.StopLocation)
        Dim bInvalidRoute As Boolean = False

        Dim bUseTLS12 As Boolean = If(NGLParameterData.GetParValue("GlobalTurnOnTLs12ForAPIs", 0) = 0, False, True)
        Dim TRMAPI = New TRM.TrimbleAPI(bUseTLS12)
        TRMAPI.APIKey = Me.TrimbleAPIKey
        Try
            Dim iNameSeq As Int16 = 0
            For Each s In StopData
                s.StopName = s.BookControl & "-" & iNameSeq.ToString()
                Dim oStop As TRM.TrimbleServiceReference.StopLocation = TRMAPI.PCMCreateStop(s.Street, s.City, s.State, s.Zip, "", s.StopName, "", lBadAddresses, lMatchingAddresses)
                If (oStop Is Nothing) Then bInvalidRoute = True
                Stops.Add(oStop)
                iNameSeq += 1
            Next
            If Not lBadAddresses Is Nothing AndAlso lBadAddresses.Count > 0 Then
                oPCMReturnEx.FailedAddressMessage = "The system found " & lBadAddresses.Count.ToString() & " bad addresses.  Please check the information and try again."
                oPCMReturnEx.BadAddressCount = lBadAddresses.Count

                For Each iBadAddress As TRM.TrimbleServiceReference.StopLocation In lBadAddresses
                    'get the stop data
                    Dim oStop As PCM.clsFMStopData = StopData.Where(Function(x) x.StopName = iBadAddress.Label).FirstOrDefault()
                    If Not oStop Is Nothing Then

                        Dim oOrig As New PCM.clsAddress()
                        Dim oDest As New PCM.clsAddress()
                        Dim oPCMOrig As New PCM.clsAddress()
                        Dim oPCMDest As New PCM.clsAddress()
                        If oStop.LocationisOrigin Then
                            oOrig.strAddress = oStop.Street
                            oOrig.strCity = oStop.City
                            oOrig.strState = oStop.State
                            oOrig.strZip = oStop.Zip
                            oPCMOrig.strAddress = iBadAddress.Address.StreetAddress
                            oPCMOrig.strCity = iBadAddress.Address.City
                            oPCMOrig.strState = iBadAddress.Address.State
                            oPCMOrig.strZip = iBadAddress.Address.Zip
                        Else
                            oDest.strAddress = oStop.Street
                            oDest.strCity = oStop.City
                            oDest.strState = oStop.State
                            oDest.strZip = oStop.Zip
                            oPCMDest.strAddress = iBadAddress.Address.StreetAddress
                            oPCMDest.strCity = iBadAddress.Address.City
                            oPCMDest.strState = iBadAddress.Address.State
                            oPCMDest.strZip = iBadAddress.Address.Zip
                        End If
                        BadAddresses.Add(oStop.BookControl, oStop.BookODControl, oOrig, oDest, oPCMOrig, oPCMDest, "Cannot Find Address", dblBatchID)
                    End If
                Next
                CreateBadAddressList(StopData, BadAddresses, dblBatchID)
            End If
            If (Not bInvalidRoute) Then
                Dim oPar As New TRM.clsTrimbleReportParams()
                '.DistType = Me.NGLPCMDistType
                oPar.DistanceUnits = Me.NGLPCMDistType
                oPar.HubRouting = False
                oPar.UseTollData = True
                oPar.FuelUnits = 0
                If blnKeepStopNumbers Then
                    oPar.RouteOptimization = 0
                Else
                    oPar.RouteOptimization = 1
                End If
                oPar.RouteType = Me.NGLPCMRouteType
                oPar.TruckStyle = NGLTrimbleServices.TrimbleServiceReference.TruckStyle.FiftyThreeSemiTrailer
                oPar.APIKey = Me.TrimbleAPIKey
                Dim oData = TRMAPI.PCMMileageReport(Stops.ToArray(), oPar)

                If Not oData Is Nothing Then
                    Dim iStop As Integer = 0
                    Dim iSequence As Integer = 0
                    Dim sLastAddress As String = ""
                    Dim sThisAddress As String = ""
                    'UpdateFMStopListFromTrm
                    For Each line In oData.ReportLines
                        System.Diagnostics.Debug.WriteLine("Location ID: {0} Miles: {1}", line.Stop.Label, line.TMiles)
                        Dim FMStop As PCM.clsFMStopData = StopData.Where(Function(x) x.StopName = line.Stop.Label).FirstOrDefault()
                        If FMStop Is Nothing Then
                            'Log errors and bad address data
                            iSequence += 1
                            Continue For
                        End If
                        With FMStop
                            sThisAddress = String.Format("{0} | {1} | {2} | {3}", FMStop.Street, FMStop.City, FMStop.State, FMStop.Zip)
                            If sThisAddress <> sLastAddress Then
                                iStop += 1
                                sLastAddress = sThisAddress
                            End If
                            .AddressValid = True
                            .LegCost = Double.Parse(line.LCostMile)
                            '.LegESTCHG = line.l
                            .LegMiles = Double.Parse(line.LMiles)
                            .LegTime = line.LHours
                            .LegTolls = line.LTolls
                            If (lBadAddresses.Any(Function(x) x.Label = line.Stop.Label)) Then
                                .LogBadAddress = True
                                Dim lPCMStop As New TRM.TrimbleServiceReference.StopLocation()
                                'TODO:  Add logic for muliple PCMiler addresses
                                '       Add logic for different types of errors like bad zip etc...
                                lPCMStop = lMatchingAddresses.Where(Function(x) x.Label = line.Stop.Label).FirstOrDefault()
                                If (Not lPCMStop Is Nothing) Then
                                    .PCMilerCity = lPCMStop.Address.City
                                    .PCMilerState = lPCMStop.Address.State
                                    .PCMilerStreet = lPCMStop.Address.StreetAddress
                                    .PCMilerZip = lPCMStop.Address.Zip
                                End If
                                .Warning = "Check Address"
                            Else
                                .Matched = True 'find out how this is used?
                            End If
                            .SeqNumber = iStop 'Find out what is different between seqnumber and stopnumber?
                            If Not blnKeepStopNumbers Then
                                .StopNumber = iStop
                            End If
                            .TotalCost = line.TCostMile ' oData.ReportLines.Sum(Function(x) x.LCostMile)
                            '.TotalESTCHG = trmStop.TotalESTCHG
                            .TotalMiles = line.TMiles ' oData.ReportLines.Sum(Function(x) x.LMiles)
                            .TotalTime = line.THours '  oData.ReportLines.Sum(Function(x) x.LHours)
                            .TotalTolls = line.TTolls ' oData.ReportLines.Sum(Function(x) x.LTolls)
                        End With
                        iSequence += 1
                    Next


                    If Not oGlobalStopData Is Nothing Then
                        Dim iMinStop As Integer = StopData.Min(Function(x) x.StopNumber)
                        Dim iMaxStop As Integer = StopData.Max(Function(x) x.StopNumber)
                        Dim oOrig = StopData.Where(Function(x) x.StopNumber = iMinStop).FirstOrDefault()
                        Dim oDest = StopData.Where(Function(x) x.StopNumber = iMaxStop).FirstOrDefault()
                        'oPCMReturnEx = New PCM.clsPCMReturnEx
                        With oPCMReturnEx
                            If Not oDest Is Nothing Then
                                If Not String.IsNullOrWhiteSpace(oDest.Zip) Then
                                    .DestZip = oDest.Zip
                                Else
                                    .DestZip = "00000"
                                End If
                                .TotalMiles = oDest.TotalMiles
                            Else
                                .DestZip = "00000"
                                .TotalMiles = StopData.Sum(Function(x) x.LegMiles)
                            End If
                            If Not lBadAddresses Is Nothing AndAlso lBadAddresses.Count() > 0 Then
                                .FailedAddressMessage = "Check Address Information"
                            End If
                            If Not oOrig Is Nothing AndAlso Not String.IsNullOrWhiteSpace(oOrig.Zip) Then
                                .OriginZip = oOrig.Zip
                            Else
                                .OriginZip = "00000"
                            End If
                            .Results = StopData
                        End With
                    End If
                End If
            Else

                oPCMReturnEx.Message = "Invalid Route Address Information"
                strLastError &= "Found " + lBadAddresses.Count().ToString() + " bad addresses"
                System.Diagnostics.Debug.WriteLine("Found " + lBadAddresses.Count().ToString() + " bad addresses")
            End If


        Catch ex As System.Net.WebException
            strLastError = formatTRMException(ex, "TRMReSyncMultiStop")
        Catch ex As Exception
            strLastError = "Cannot execute TRMReSyncMultiStop. " & ex.Message
        End Try
        Return oPCMReturnEx
    End Function

    Private Function formatTRMException(ByVal ex As Exception, ByVal strSource As String) As String

        Return "The PCMiler " & strSource & " procedure is not available." & vbCrLf & "There may be a problem with your URL: " & Me.TrimbleAPIURL & vbCrLf & ". Please check your single sign on settings for your  Trimble API Account.  The actual error message is: " & vbCrLf & ex.Message
    End Function


    Private Sub UpdateFMStopListFromTrm(ByRef StopData As List(Of PCM.clsFMStopData), ByVal arrFMStops As TRM.clsFMStopData())

        Dim intBookControl As Integer = 0
        Dim IsOrigin As Boolean
        For Each trmStop In arrFMStops
            intBookControl = trmStop.BookControl
            IsOrigin = trmStop.LocationisOrigin
            'find the matching record
            Dim FMStop As PCM.clsFMStopData = (From d In StopData Where d.BookControl = intBookControl And d.LocationisOrigin = IsOrigin Select d).First()
            If Not FMStop Is Nothing Then
                With FMStop
                    .AddressValid = trmStop.AddressValid
                    .LegCost = trmStop.LegCost
                    .LegESTCHG = trmStop.LegESTCHG
                    .LegMiles = trmStop.LegMiles
                    .LegTime = trmStop.LegTime
                    .LegTolls = trmStop.LegTolls
                    .LogBadAddress = trmStop.LogBadAddress
                    .Matched = trmStop.Matched
                    .PCMilerCity = trmStop.PCMilerCity
                    .PCMilerState = trmStop.PCMilerState
                    .PCMilerStreet = trmStop.PCMilerStreet
                    .PCMilerZip = trmStop.PCMilerZip
                    .SeqNumber = trmStop.SeqNumber
                    .StopName = trmStop.StopName
                    .StopNumber = trmStop.StopNumber
                    .TotalCost = trmStop.TotalCost
                    .TotalESTCHG = trmStop.TotalESTCHG
                    .TotalMiles = trmStop.TotalMiles
                    .TotalTime = trmStop.TotalTime
                    .TotalTolls = trmStop.TotalTolls
                    .Warning = trmStop.Warning
                End With
            End If
        Next
    End Sub


    Private Sub CreateBadAddressList(ByVal oStops As List(Of PCM.clsFMStopData), ByRef BadAddresses As PCM.clsPCMBadAddresses, ByVal dblBatchID As Double)

        Dim BadBookControls As New List(Of Integer)
        Dim oBadOrig As New PCM.clsAddress
        Dim oBadDest As New PCM.clsAddress
        Dim oBadPCMOrig As New PCM.clsAddress
        Dim oBadPCMDest As New PCM.clsAddress
        Dim intBookControl As Integer
        Dim oBadStops As List(Of PCM.clsFMStopData) = (From d In oStops Where d.LogBadAddress = True Select d).ToList

        If oBadStops Is Nothing OrElse oBadStops.Count < 1 Then Return

        For Each oStop As PCM.clsFMStopData In oBadStops

            intBookControl = oStop.BookControl
            If Not BadBookControls.Contains(intBookControl) Then

                'Add the bad address
                If oStop.LocationisOrigin Then
                    oBadOrig = New PCM.clsAddress
                    With oBadOrig
                        .strAddress = oStop.Street
                        .strCity = oStop.City
                        .strState = oStop.State
                        .strZip = oStop.Zip
                    End With
                    oBadPCMOrig = New PCM.clsAddress
                    With oBadPCMOrig
                        .strAddress = oStop.PCMilerStreet
                        .strCity = oStop.PCMilerCity
                        .strState = oStop.PCMilerState
                        .strZip = oStop.PCMilerZip
                    End With
                    'Lookup the destination
                    Dim oDest As PCM.clsFMStopData = (From d In oStops Where d.BookControl = intBookControl And d.LocationisOrigin = False Select d).First()
                    If Not oDest Is Nothing Then
                        oBadDest = New PCM.clsAddress
                        With oBadDest
                            .strAddress = oDest.Street
                            .strCity = oDest.City
                            .strState = oDest.State
                            .strZip = oDest.Zip
                        End With
                        oBadPCMDest = New PCM.clsAddress
                        With oBadPCMDest
                            .strAddress = oDest.PCMilerStreet
                            .strCity = oDest.PCMilerCity
                            .strState = oDest.PCMilerState
                            .strZip = oDest.PCMilerZip
                        End With
                    End If
                Else
                    oBadDest = New PCM.clsAddress
                    With oBadDest
                        .strAddress = oStop.Street
                        .strCity = oStop.City
                        .strState = oStop.State
                        .strZip = oStop.Zip
                    End With
                    oBadPCMDest = New PCM.clsAddress
                    With oBadPCMDest
                        .strAddress = oStop.PCMilerStreet
                        .strCity = oStop.PCMilerCity
                        .strState = oStop.PCMilerState
                        .strZip = oStop.PCMilerZip
                    End With
                    'Lookup the origin
                    Dim oOrig As PCM.clsFMStopData = (From d In oStops Where d.BookControl = intBookControl And d.LocationisOrigin = True Select d).First()
                    If Not oOrig Is Nothing Then
                        oBadOrig = New PCM.clsAddress
                        With oBadOrig
                            .strAddress = oOrig.Street
                            .strCity = oOrig.City
                            .strState = oOrig.State
                            .strZip = oOrig.Zip
                        End With
                        oBadPCMOrig = New PCM.clsAddress
                        With oBadPCMOrig
                            .strAddress = oOrig.PCMilerStreet
                            .strCity = oOrig.PCMilerCity
                            .strState = oOrig.PCMilerState
                            .strZip = oOrig.PCMilerZip
                        End With
                    End If
                End If
                BadAddresses.Add(intBookControl,
                   oStop.BookODControl,
                   oBadOrig,
                   oBadDest,
                   oBadPCMOrig,
                   oBadPCMDest,
                   oStop.Warning,
                   dblBatchID)
                'Add the bad control number to the list
                BadBookControls.Add(intBookControl)
            End If




        Next

    End Sub






#End Region

#Region "Private Methods"


    Private Sub logBadAddressesAsync(ByVal oPCMBadAddresses As PCM.clsPCMBadAddresses)
        Dim fetcher As New ProcessBadAddressesDelegate(AddressOf Me.ExeclogBadAddressesAsync)
        ' Launch thread
        fetcher.BeginInvoke(oPCMBadAddresses, Nothing, Nothing)
    End Sub

    Private Sub ExeclogBadAddressesAsync(ByVal oPCMBadAddresses As PCM.clsPCMBadAddresses)
        Dim oPCMReturnEX As New PCM.clsPCMReturnEx
        logBadAddresses(oPCMBadAddresses, oPCMReturnEX)
    End Sub

    Private Sub logBadAddresses(ByVal oPCMBadAddresses As PCM.clsPCMBadAddresses, ByRef oPCMReturnEX As PCM.clsPCMReturnEx)
        Dim oPCMBadAddress As PCM.clsPCMBadAddress
        For intBadAddressCT = 1 To oPCMBadAddresses.COUNT
            oPCMBadAddress = oPCMBadAddresses.Item(intBadAddressCT)
            logBadAddress(oPCMBadAddress, oPCMReturnEX)
        Next
    End Sub

    Private Sub logBadAddress(ByVal oPCMBadAddress As PCM.clsPCMBadAddress,
                              ByRef oPCMReturnEX As PCM.clsPCMReturnEx)
        Try
            Dim intRet = NGLLaneData.AddBadAddress(oPCMBadAddress.BookControl,
                                       oPCMBadAddress.LaneControl,
                                       Left(oPCMBadAddress.objOrig.strAddress, 40),
                                       Left(oPCMBadAddress.objOrig.strCity, 25),
                                       Left(oPCMBadAddress.objOrig.strState, 2),
                                       Left(oPCMBadAddress.objOrig.strZip, 10),
                                       "",
                                       Left(oPCMBadAddress.objDest.strAddress, 40),
                                       Left(oPCMBadAddress.objDest.strCity, 25),
                                       Left(oPCMBadAddress.objDest.strState, 2),
                                       Left(oPCMBadAddress.objDest.strZip, 10),
                                       "",
                                       Left(oPCMBadAddress.objPCMOrig.strAddress, 40),
                                       Left(oPCMBadAddress.objPCMOrig.strCity, 25),
                                       Left(oPCMBadAddress.objPCMOrig.strState, 2),
                                       Left(oPCMBadAddress.objPCMOrig.strZip, 10),
                                       "",
                                       Left(oPCMBadAddress.objPCMDest.strAddress, 40),
                                       Left(oPCMBadAddress.objPCMDest.strCity, 25),
                                       Left(oPCMBadAddress.objPCMDest.strState, 2),
                                       Left(oPCMBadAddress.objPCMDest.strZip, 10),
                                       "",
                                       Left(oPCMBadAddress.Message, 1000),
                                       oPCMBadAddress.BatchID)

            If intRet > 0 Then
                oPCMReturnEX.BadAddressControls(oPCMReturnEX.BadAddressCount) = intRet
                oPCMReturnEX.BadAddressCount = oPCMReturnEX.BadAddressCount + 1
            End If
        Catch ex As FaultException
            'ignore any fault exceptons when logging bad address data?
        Catch ex As Exception
            Throw
        End Try

    End Sub

    ''' <summary>
    ''' Reads the Single Sign On accout for this user and updates the Trimble Key Properties, data is cached,  send optional parameter to force a refresh
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.4 on 06/08/2021 read Trimble API Settings
    ''' </remarks>
    Private Function getTrimbleAPISettings(Optional ByVal blnRefresh = False) As Boolean
        Dim blnRet As Boolean = False ' As String = "TrimbleAPIKey = 'NA'; " & vbLf

        If String.IsNullOrWhiteSpace(Me.TrimbleAPIKey) OrElse Me.TrimbleUserControl <> Me.Parameters.UserControl Then blnRefresh = True
        'we only read the data 
        If blnRefresh Then
            Dim oSec As DAL.NGLtblSingleSignOnAccountData = New DAL.NGLtblSingleSignOnAccountData(Parameters)
            Me.UseTrimbleAPI = False
            Me.TrimbleAPIKey = ""
            Me.TrimbleAPIKeyQueryString = ""
            Me.TrimbleAPIURL = ""
            ' for now we do not have users specific Trimble API settings
            'Dim SSOA As DTO.WCFResults() = oSec.GetSingleSignOnAccountByUser(Me.Parameters.UserControl, DAL.Utilities.SSOAAccount.Trimble)
            'If SSOA?.Length > 0 Then
            '    If SSOA(0).KeyFields?.Count > 0 Then
            '        Me.TrimbleAPIURL = SSOA(0).KeyFields("SSOALoginURL")
            '        Me.TrimbleAPIKey = SSOA(0).KeyFields("RefID")
            '        Me.TrimbleUserControl = Me.Parameters.UserControl ' just a safety net to ensure we are using the corrrect user credentials when data is cached
            '        If Not String.IsNullOrWhiteSpace(Me.TrimbleAPIKey) Then
            '            Me.TrimbleAPIKeyQueryString = "TrimbleAPIKey = '" & Me.TrimbleAPIKey & "'; " & vbLf
            '            Me.UseTrimbleAPI = True
            '            blnRet = True
            '        End If
            '    End If
            'End If
            Dim SSOA As DTO.tblSingleSignOnAccount = oSec.GettblSingleSignOnAccount(DAL.Utilities.SSOAAccount.Trimble)

            If Not String.IsNullOrWhiteSpace(SSOA.SSOAAuthCode) Then
                Me.TrimbleAPIURL = SSOA.SSOALoginURL
                Me.TrimbleAPIKey = SSOA.SSOAAuthCode
                Me.TrimbleUserControl = Me.Parameters.UserControl ' just a safety net to ensure we are using the corrrect user credentials when data is cached
                Me.TrimbleAPIKeyQueryString = "TrimbleAPIKey = '" & Me.TrimbleAPIKey & "'; " & vbLf
                Me.UseTrimbleAPI = True
                blnRet = True
            End If
        End If

        Return Me.UseTrimbleAPI
    End Function

#End Region

End Class

