Imports System.Runtime.InteropServices
Imports System.Text

<Guid("05AFE104-3ED7-4d41-8220-A69E5AC4A1D1"), _
InterfaceType(ComInterfaceType.InterfaceIsDual)> _
Public Interface IPCMiles

#Region "Enums"
    Enum PCMEX_Route_Type As Integer
        ROUTE_TYPE_PRACTICAL = 0
        ROUTE_TYPE_SHORTEST = 1
        ROUTE_TYPE_NATIONAL = 2
        ROUTE_TYPE_AVOIDTOLL = 3
        ROUTE_TYPE_AIR = 4
        ROUTE_TYPE_53FOOT = 6
    End Enum

    Enum PCMEX_Opt_Flags As Integer
        OPT_NONE = 0
        OPT_AVOIDTOLL = 256
        OPT_NATIONAL = 512
        CALCEX_OPT_FIFTYTHREE = 1024
    End Enum

    Enum PCMEX_Veh_Type As Integer
        CALCEX_VEH_TRUCK = 0
        CALCEX_VEH_AUTO = 16777216
    End Enum

    Enum PCMEX_Dist_Type As Integer
        DIST_TYPE_MILES = 0
        DIST_TYPE_KILO = 1
    End Enum
#End Region

#Region "Properties"
    'ReadOnly Property ServerID() As Short
    'ReadOnly Property MilesErrors() As String
    ReadOnly Property LastError() As String
    Property Debug() As Boolean
    Property KeepLogDays() As Integer
    Property SaveOldLog() As Boolean
    Property WebServiceURL() As String
    Property UseZipOnly As Boolean
#End Region

#Region "Methods"
    'Sub setKeepAlive(ByVal KeepAlive As Integer)
    'Function getMilesErrors() As String
    'Sub clearMilesErrors()
    'Function CityName(ByVal zipcode As String) As String
    Function CityToLatLong(ByRef cityZip As String, _
                                ByVal LoggingOn As Boolean, _
                                ByVal LogFileName As String) As String
    'Function FirstMatch(ByVal CityZip As String) As String
    Function FullName(ByVal CityNameOrZipCode As String, _
                                ByVal LoggingOn As Boolean, _
                                ByVal LogFileName As String) As String
    Function getGeoCode(ByVal location As String, _
                                ByRef dblLat As Double, _
                                ByRef dblLong As Double, _
                                ByVal LoggingOn As Boolean, _
                                ByVal LogFileName As String) As Boolean
    'Function getGeoCodeBatch(ByVal location As Object, ByRef dblLat As Double, ByRef dblLong As Double, ByVal blnUsePCMiler As Boolean) As Boolean
    Function LatLongToCity(ByVal latlong As String, _
                                ByVal LoggingOn As Boolean, _
                                ByVal LogFileName As String) As String
    Function getPracticalMiles(ByVal objOrig As clsAddress, _
                                ByVal objDest As clsAddress, _
                                ByVal Route_Type As IPCMiles.PCMEX_Route_Type, _
                                ByVal Dist_Type As IPCMiles.PCMEX_Dist_Type, _
                                ByVal intCompControl As Integer, _
                                ByVal intBookControl As Integer, _
                                ByVal intLaneControl As Integer, _
                                ByVal strItemNumber As String, _
                                ByVal strItemType As String, _
                                ByVal dblAutoCorrectBadLaneZipCodes As Double, _
                                ByVal dblBatchID As Double, _
                                ByVal blnBatch As Boolean, _
                                ByRef BaddAddresses As clsPCMBadAddresses, _
                                ByVal LoggingOn As Boolean, _
                                ByVal LogFileName As String) As clsAllStops
    Function Miles(ByVal Origin As String, _
                                ByVal Destination As String, _
                                ByVal LoggingOn As Boolean, _
                                ByVal LogFileName As String) As Single
    'Function PCMilerEnd() As Short
    'Function PCMilerStart() As Short
    'Function Server() As Short
    Function zipcode(ByVal CityName As String, _
                                ByVal LoggingOn As Boolean, _
                                ByVal LogFileName As String) As String
    Function PCMReSync(ByVal StopData As clsPCMDataStops, _
                                ByVal strConsNumber As String, _
                                ByVal dblBatchID As Double, _
                                ByVal blnKeepStopNumbers As Boolean, _
                                ByRef BaddAddresses As clsPCMBadAddresses, _
                                ByVal LoggingOn As Boolean, _
                                ByVal LogFileName As String) As clsAllStops
    'Function PCMReSyncTest(ByVal strConsNumber As String, ByVal dblBatchID As Double, ByVal blnKeepStopNumbers As Boolean) As clsAllStops
    Function getRouteMiles(ByRef sRoute As clsSimpleStop(), _
                                ByVal LoggingOn As Boolean, _
                                ByVal LogFileName As String) As clsPCMReturn
#End Region

#Region "New methods added in v-5.1.4"

    Function PCMValidateAddress(ByVal strAddress As String) As Boolean

    Function PCMReSyncMultiStop(ByVal StopData As List(Of clsFMStopData), _
                               ByRef BadAddresses As clsPCMBadAddresses, _
                               ByRef ReportRecords As List(Of clsPCMReportRecord), _
                               ByVal dblBatchID As Double, _
                               ByVal blnKeepStopNumbers As Boolean, _
                               ByVal LoggingOn As Boolean, _
                               ByVal LogFileName As String) As clsPCMReturnEx
#End Region
End Interface

<Guid("2422902F-7B54-4c4a-8E17-5707C823CA91"), _
ClassInterface(ClassInterfaceType.None), _
ProgId("NGL.FreightMaster.PCMiler.PCMiles")> _
Public Class PCMiles : Implements IPCMiles

#Region "CONSTANTS"
    ' Routing calculation types
    Public Const CALC_PRACTICAL As Short = 0
    Public Const CALC_SHORTEST As Short = 1
    Public Const CALC_NATIONAL As Short = 2
    Public Const CALC_AVOIDTOLL As Short = 3
    Public Const CALC_AIR As Short = 4
    Public Const CALC_53FOOT As Short = 6

    ' Report types
    Public Const RPT_DETAIL As Short = 0
    Public Const RPT_STATE As Short = 1
    Public Const RPT_MILEAGE As Short = 2

    'Distance types
    Public Const DIST_TYPE_MILES As Short = 0
    Public Const DIST_TYPE_KILO As Short = 1
#End Region


#Region "Properties"
    'Private _PCMTimer As New System.Timers.Timer()
    'Private _KeepAlive As Integer = 0
    'Public ReadOnly Property ServerID() As Short Implements IPCMiles.ServerID
    '    Get
    '        Return gServerID
    '    End Get
    'End Property

    'Private _strMilesErrors As String = ""
    'Public ReadOnly Property MilesErrors() As String Implements IPCMiles.MilesErrors
    '    Get
    '        Return _strMilesErrors

    '    End Get
    'End Property

    Private _strLastError As String = ""
    Public ReadOnly Property LastError() As String Implements IPCMiles.LastError
        Get
            Return _strLastError & gLastError

        End Get
    End Property

    Private _blnDebug As Boolean = False
    Public Property Debug() As Boolean Implements IPCMiles.Debug
        Get
            Debug = _blnDebug
        End Get
        Set(ByVal Value As Boolean)
            _blnDebug = Value
        End Set
    End Property


    Private _intKeepLogDays As Integer = 7
    Public Property KeepLogDays() As Integer Implements IPCMiles.KeepLogDays
        Get
            Return _intKeepLogDays
        End Get
        Set(ByVal value As Integer)
            _intKeepLogDays = value
        End Set
    End Property

    Private _blnSaveOldLog As Boolean = False
    Public Property SaveOldLog() As Boolean Implements IPCMiles.SaveOldLog
        Get
            Return _blnSaveOldLog
        End Get
        Set(ByVal value As Boolean)
            _blnSaveOldLog = value
        End Set
    End Property

    Private _strWebServiceURL As String = "http://nglwsprod.nextgeneration.com/PCMiler.asmx"
    Public Property WebServiceURL() As String Implements IPCMiles.WebServiceURL
        Get
            Return _strWebServiceURL
        End Get
        Set(ByVal value As String)
            _strWebServiceURL = value
        End Set
    End Property

    Private _blnUseZipOnly As Boolean = False
    Public Property UseZipOnly() As Boolean Implements IPCMiles.UseZipOnly
        Get
            Return _blnUseZipOnly
        End Get
        Set(ByVal value As Boolean)
            _blnUseZipOnly = value
        End Set
    End Property

    Private SimpleStopList As New List(Of clsSimpleStop)

#End Region

#Region "Constructors"
    Public Sub New()
        MyBase.New()
    End Sub

    'Public Sub New(ByVal KeepAlive As Integer)
    '    Me._KeepAlive = KeepAlive
    '    If Me._KeepAlive > 0 Then
    '        AddHandler _PCMTimer.Elapsed, AddressOf OnPCMTimerEvent
    '        ' Set the Interval to 5 seconds.
    '        _PCMTimer.Interval = Me._KeepAlive
    '        _PCMTimer.Enabled = True
    '    End If
    'End Sub

    Protected Overrides Sub finalize()
        'PCMilerEnd()
        MyBase.Finalize()
    End Sub
#End Region
#Region "Methods"

    'Sub setKeepAlive(ByVal KeepAlive As Integer) Implements IPCMiles.setKeepAlive
    '    Me._KeepAlive = KeepAlive
    '    If Me._KeepAlive > 0 Then
    '        AddHandler _PCMTimer.Elapsed, AddressOf OnPCMTimerEvent
    '        ' Set the Interval to 5 seconds.
    '        _PCMTimer.Interval = Me._KeepAlive
    '        _PCMTimer.Enabled = True
    '    End If
    'End Sub

    '' Specify what you want to happen when the Elapsed event is raised.
    'Private Shared Sub OnPCMTimerEvent(ByVal source As Object, ByVal e As System.Timers.ElapsedEventArgs)
    '    If gServerID > 0 AndAlso gProcessRunning = False Then
    '        Try
    '            gLastError = ""
    '            PCMSCloseServer(gServerID)
    '            'Console.WriteLine("PCMiler closed")
    '        Catch ex As Exception
    '            gLastError =  getErrorMessage(ex) & "Cannot execute PCMSCloseServer."
    '        Finally
    '            gServerID = 0
    '        End Try
    '    End If
    'End Sub


    'Function PCMReSyncTest(ByVal strConsNumber As String, ByVal dblBatchID As Double, ByVal blnKeepStopNumbers As Boolean) As clsAllStops Implements IPCMiles.PCMReSyncTest
    '    Return New clsAllStops
    'End Function

    'Public Function getMilesErrors() As String Implements IPCMiles.getMilesErrors
    '    getMilesErrors = _strMilesErrors
    'End Function

    'Public Sub clearMilesErrors() Implements IPCMiles.clearMilesErrors
    '    _strMilesErrors = ""
    'End Sub

    'Private Function CityName(ByVal zipcode As String) As String
    '    Dim buffer As String = ""
    '    Dim Zip As String = ""
    '    Try
    '        buffer = FirstMatch(zipcode)
    '    Catch ex As Exception
    '        Me._strLastError = "Cannot execute CityName. " &  getErrorMessage(ex)
    '    End Try
    '    Return buffer
    'End Function

    Public Function CityToLatLong(ByRef cityZip As String, _
                                ByVal LoggingOn As Boolean, _
                                ByVal LogFileName As String) As String Implements IPCMiles.CityToLatLong
        Dim strRet As String = ""
        Dim oPCM As New PCM.PCMiler
        _strLastError = ""
        gLastError = ""
        Try
            oPCM.Url = Me.WebServiceURL
            Dim strLastError As String = ""
            strRet = oPCM.CityToLatLong(cityZip, _
                                        Debug, _
                                        LoggingOn, _
                                        KeepLogDays, _
                                        SaveOldLog, _
                                        LogFileName, _
                                        strLastError)
            If Not String.IsNullOrEmpty(strLastError) Then _strLastError = strLastError
        Catch ex As System.Net.WebException
            _strLastError = formatWebException(ex, "CityToLatLong")
        Catch ex As Exception
            _strLastError = "Cannot execute CityToLatLong. " & getErrorMessage(ex)
        End Try
        Return strRet
    End Function

    Public Function cityStateZipLookup(ByVal postalCode As String, _
                                ByVal LoggingOn As Boolean, _
                                ByVal LogFileName As String) As clsAddress()
        Dim LastError As String
        Dim oRet As New List(Of clsAddress)
        Dim oPCM As New PCM.PCMiler
        Try
            oPCM.Url = Me.WebServiceURL
            Dim oData As NGL.FreightMaster.PCMiler.PCM.clsAddress() = oPCM.cityStateZipLookup(postalCode, _
                                        Debug, _
                                        LoggingOn, _
                                        KeepLogDays, _
                                        SaveOldLog, _
                                        LogFileName, _
                                        LastError)
            If Not String.IsNullOrEmpty(LastError) Then _strLastError = LastError
            If Not oData Is Nothing AndAlso oData.Length > 0 Then
                For Each a As PCM.clsAddress In oData
                    oRet.Add(New clsAddress With {.strAddress = a.strAddress, .strCity = a.strCity, .strState = a.strState, .strZip = a.strZip})
                Next
            End If
        Catch ex As System.Net.WebException
            _strLastError = formatWebException(ex, "CityToLatLong")
        Catch ex As Exception
            _strLastError = "Cannot execute CityToLatLong. " & getErrorMessage(ex)
        End Try
        If Not oRet Is Nothing AndAlso oRet.Count > 0 Then
            Return oRet.ToArray()
        Else
            Return Nothing
        End If
    End Function

    Public Function FullName(ByVal CityNameOrZipCode As String, _
                                ByVal LoggingOn As Boolean, _
                                ByVal LogFileName As String) As String Implements IPCMiles.FullName

        Dim strRet As String = ""
        Dim oPCM As New PCM.PCMiler
        _strLastError = ""
        gLastError = ""
        Try
            oPCM.Url = Me.WebServiceURL
            Dim strLastError As String = ""
            strRet = oPCM.FullName(CityNameOrZipCode, _
                                        Debug, _
                                        LoggingOn, _
                                        KeepLogDays, _
                                        SaveOldLog, _
                                        LogFileName, _
                                        strLastError)
            If Not String.IsNullOrEmpty(strLastError) Then _strLastError = strLastError

        Catch ex As System.Net.WebException
            _strLastError = formatWebException(ex, "FullName")
        Catch ex As Exception
            _strLastError = "Cannot execute FullName. " & getErrorMessage(ex)
        End Try
        Return strRet

    End Function

    Public Function getGeoCode(ByVal location As String, _
                                ByRef dblLat As Double, _
                                ByRef dblLong As Double, _
                                ByVal LoggingOn As Boolean, _
                                ByVal LogFileName As String) As Boolean Implements IPCMiles.getGeoCode

        'If Me.Debug Then Return getGeoCodeDebug(location, dblLat, dblLong, LoggingOn, LogFileName)

        Dim blnRet As Boolean = False
        Dim oPCM As New PCM.PCMiler
        _strLastError = ""
        gLastError = ""
        Try
            oPCM.Url = Me.WebServiceURL
            Dim strLastError As String = ""
            blnRet = oPCM.getGeoCode(location, _
                                        dblLat, _
                                        dblLong, _
                                        Debug, _
                                        LoggingOn, _
                                        KeepLogDays, _
                                        SaveOldLog, _
                                        LogFileName, _
                                        strLastError)
            If Not String.IsNullOrEmpty(strLastError) Then _strLastError = strLastError
        Catch ex As System.Net.WebException
            _strLastError = formatWebException(ex, "getGeoCode")
        Catch ex As Exception
            _strLastError = "Cannot execute getGeoCode. " & getErrorMessage(ex)
        End Try
        Return blnRet

    End Function


    'Private Function getGeoCodeDebug(ByVal location As String, _
    '                            ByRef dblLat As Double, _
    '                            ByRef dblLong As Double, _
    '                            ByVal LoggingOn As Boolean, _
    '                            ByVal LogFileName As String) As Boolean

    '    Dim blnRet As Boolean = False
    '    Dim oPCM As New NGL.Service.PCMiler.Debug.PCMiles
    '    _strLastError = ""
    '    gLastError = ""
    '    Try
    '        Dim strLastError As String = ""
    '        With oPCM
    '            .Debug = True
    '            .LoggingOn = LoggingOn
    '            .KeepLogDays = KeepLogDays
    '            .SaveOldLog = SaveOldLog
    '            .LogFileName = LogFileName
    '            .UseZipOnly = UseZipOnly
    '        End With

    '        blnRet = oPCM.getGeoCode(location, _
    '                                    dblLat, _
    '                                    dblLong)
    '        If Not String.IsNullOrEmpty(oPCM.LastError) Then _strLastError = oPCM.LastError
    '    Catch ex As System.Net.WebException
    '        _strLastError = formatWebException(ex, "getGeoCode")
    '    Catch ex As Exception
    '        _strLastError = "Cannot execute getGeoCode. " & getErrorMessage(ex)
    '    Finally
    '        oPCM.PCMilerEnd()
    '    End Try
    '    Return blnRet

    'End Function

    'Public Function getGeoCodeBatch(ByVal location As Object, ByRef dblLat As Double, ByRef dblLong As Double, ByVal blnUsePCMiler As Boolean) As Boolean Implements IPCMiles.getGeoCodeBatch

    '    Dim strRet As String = ""
    '    Dim blnRet As Boolean = False
    '    Try
    '        gProcessRunning = True
    '        If Not blnUsePCMiler Then Return False

    '        If Not gServerID > 0 Then
    '            If Not PCMilerStart() > 0 Then
    '                _strLastError = "Cannot execute getGeoCodeBatch.   PC Miler is not running.  " & Me._strLastError
    '                Return False
    '            End If
    '        End If
    '        strRet = CityToLatLong(location)
    '        If Trim(strRet) > "" Then
    '            dblLat = convertLatLongToDec(Left(strRet, 8))
    '            dblLong = convertLatLongToDec(Mid(strRet, 10, 8))
    '            blnRet = True
    '        End If

    '    Catch ex As System.AccessViolationException
    '        Me._strLastError = "Cannot execute getGeoCodeBatch: PC Miler is no longer running.  Please check your PCMilerKeepAlive parameter setting."
    '    Catch ex As Exception
    '        Me._strLastError = "Cannot execute getGeoCodeBatch. " &  getErrorMessage(ex)
    '    Finally
    '        gProcessRunning = False
    '    End Try
    '    Return blnRet
    'End Function

    Public Function LatLongToCity(ByVal latlong As String, _
                                ByVal LoggingOn As Boolean, _
                                ByVal LogFileName As String) As String Implements IPCMiles.LatLongToCity

        Dim strRet As String = ""
        Dim oPCM As New PCM.PCMiler
        _strLastError = ""
        gLastError = ""
        Try
            oPCM.Url = Me.WebServiceURL
            Dim strLastError As String = ""
            strRet = oPCM.LatLongToCity(latlong, _
                                        Debug, _
                                        LoggingOn, _
                                        KeepLogDays, _
                                        SaveOldLog, _
                                        LogFileName, _
                                        strLastError)
            If Not String.IsNullOrEmpty(strLastError) Then _strLastError = strLastError

        Catch ex As System.Net.WebException
            _strLastError = formatWebException(ex, "LatLongToCity")
        Catch ex As Exception
            _strLastError = "Cannot execute LatLongToCity. " & getErrorMessage(ex)
        End Try
        Return strRet
    End Function

    Function getPracticalMiles(ByVal objOrig As clsAddress, _
                                ByVal objDest As clsAddress, _
                                ByVal Route_Type As IPCMiles.PCMEX_Route_Type, _
                                ByVal Dist_Type As IPCMiles.PCMEX_Dist_Type, _
                                ByVal intCompControl As Integer, _
                                ByVal intBookControl As Integer, _
                                ByVal intLaneControl As Integer, _
                                ByVal strItemNumber As String, _
                                ByVal strItemType As String, _
                                ByVal dblAutoCorrectBadLaneZipCodes As Double, _
                                ByVal dblBatchID As Double, _
                                ByVal blnBatch As Boolean, _
                                ByRef BaddAddresses As clsPCMBadAddresses, _
                                ByVal LoggingOn As Boolean, _
                                ByVal LogFileName As String) As clsAllStops Implements IPCMiles.getPracticalMiles


        Dim oclsAllStops As clsAllStops = Nothing
        Dim oPCM As New PCM.PCMiler
        _strLastError = ""
        gLastError = ""
        Try
            oPCM.Url = Me.WebServiceURL
            Dim strLastError As String = ""
            Dim arrBadAddresses() As PCM.clsPCMBadAddress
            Dim oOrig As New PCM.clsAddress
            With oOrig
                .strAddress = objOrig.strAddress
                .strCity = objOrig.strCity
                .strState = objOrig.strState
                .strZip = objOrig.strZip
            End With
            Dim oDest As New PCM.clsAddress
            With oDest
                .strAddress = objDest.strAddress
                .strCity = objDest.strCity
                .strState = objDest.strState
                .strZip = objDest.strZip
            End With
            Dim oGlobalStopData As PCM.clsGlobalStopData = oPCM.getPracticalMilesEX(oOrig, _
                                        oDest, _
                                        Route_Type, _
                                        Dist_Type, _
                                        intCompControl, _
                                        intBookControl, _
                                        intLaneControl, _
                                        strItemNumber, _
                                        strItemType, _
                                        dblAutoCorrectBadLaneZipCodes, _
                                        dblBatchID, _
                                        blnBatch, _
                                        arrBadAddresses, _
                                        Debug, _
                                        LoggingOn, _
                                        KeepLogDays, _
                                        SaveOldLog, _
                                        LogFileName, _
                                        UseZipOnly,
                                        strLastError)
            If Not String.IsNullOrEmpty(strLastError) Then _strLastError = strLastError
            If Not arrBadAddresses Is Nothing AndAlso arrBadAddresses.Length > 0 Then
                For i As Integer = 0 To arrBadAddresses.Length - 1
                    Dim oBadOrig As New clsAddress
                    With oBadOrig
                        .strAddress = arrBadAddresses(i).objOrig.strAddress
                        .strCity = arrBadAddresses(i).objOrig.strCity
                        .strState = arrBadAddresses(i).objOrig.strState
                        .strZip = arrBadAddresses(i).objOrig.strZip
                    End With
                    Dim oBadDest As New clsAddress
                    With oBadDest
                        .strAddress = arrBadAddresses(i).objDest.strAddress
                        .strCity = arrBadAddresses(i).objDest.strCity
                        .strState = arrBadAddresses(i).objDest.strState
                        .strZip = arrBadAddresses(i).objDest.strZip
                    End With
                    Dim oBadPCMOrig As New clsAddress
                    With oBadPCMOrig
                        .strAddress = arrBadAddresses(i).objPCMOrig.strAddress
                        .strCity = arrBadAddresses(i).objPCMOrig.strCity
                        .strState = arrBadAddresses(i).objPCMOrig.strState
                        .strZip = arrBadAddresses(i).objPCMOrig.strZip
                    End With
                    Dim oBadPCMDest As New clsAddress
                    With oBadPCMDest
                        .strAddress = arrBadAddresses(i).objPCMDest.strAddress
                        .strCity = arrBadAddresses(i).objPCMDest.strCity
                        .strState = arrBadAddresses(i).objPCMDest.strState
                        .strZip = arrBadAddresses(i).objPCMDest.strZip
                    End With

                    BaddAddresses.Add(arrBadAddresses(i).BookControl, _
                        arrBadAddresses(i).LaneControl, _
                        oBadOrig, _
                        oBadDest, _
                        oBadPCMOrig, _
                        oBadPCMDest, _
                        arrBadAddresses(i).Message, _
                        arrBadAddresses(i).BatchID)
                Next
            End If
            If Not oGlobalStopData Is Nothing Then
                oclsAllStops = New clsAllStops
                With oclsAllStops
                    .AutoCorrectBadLaneZipCodes = oGlobalStopData.AutoCorrectBadLaneZipCodes
                    .BatchID = oGlobalStopData.BatchID
                    .DestZip = oGlobalStopData.DestZip
                    .FailedAddressMessage = oGlobalStopData.FailedAddressMessage
                    .OriginZip = oGlobalStopData.OriginZip
                    .TotalMiles = oGlobalStopData.TotalMiles
                End With
            End If

        Catch ex As System.Net.WebException
            _strLastError = formatWebException(ex, "getPracticalMiles")
        Catch ex As Exception
            _strLastError = "Cannot execute getPracticalMiles. " & getErrorMessage(ex)
        End Try
        Return oclsAllStops

    End Function

    Public Function Miles(ByVal Origin As String, _
                                ByVal Destination As String, _
                                ByVal LoggingOn As Boolean, _
                                ByVal LogFileName As String) As Single Implements IPCMiles.Miles

        Dim sglMiles As Single = 0
        Dim oPCM As New PCM.PCMiler
        _strLastError = ""
        gLastError = ""
        Try
            oPCM.Url = Me.WebServiceURL
            Dim strLastError As String = ""
            sglMiles = oPCM.Miles(Origin, _
                                        Destination, _
                                        Debug, _
                                        LoggingOn, _
                                        KeepLogDays, _
                                        SaveOldLog, _
                                        LogFileName, _
                                        strLastError)
            If Not String.IsNullOrEmpty(strLastError) Then _strLastError = strLastError

        Catch ex As System.Net.WebException
            _strLastError = formatWebException(ex, "Miles")
        Catch ex As Exception
            _strLastError = "Cannot execute Miles. " & getErrorMessage(ex)
        End Try
        Return sglMiles

    End Function

    'Public Function PCMilerEnd() As Short Implements IPCMiles.PCMilerEnd
    '    Dim retVal As Short = 0
    '    Try
    '        retVal = PCMSCloseServer(gServerID)
    '        'Console.WriteLine("PCMiler End Executed")
    '    Catch ex As Exception
    '        _strLastError = ex.ToString & "Cannot execute PCMilerEnd."
    '    Finally
    '        gServerID = 0
    '    End Try

    '    Return retVal

    'End Function

    'Public Function PCMilerStart() As Short Implements IPCMiles.PCMilerStart
    '    Dim errBuff As StringBuilder = New StringBuilder(100)
    '    Dim Ret As Short = 0
    '    If Not gServerID > 0 Then
    '        Try
    '            gServerID = PCMSOpenServer(0, 0)
    '            If Not gServerID > 0 Then
    '                Ret = PCMSGetErrorString(PCMSGetError(), errBuff, 100)
    '                _strLastError = "PCMiler could not start. Cannot execute PCMilerStart.  " & errBuff.ToString.Replace(Chr(0), "").Trim
    '            End If
    '        Catch ex As System.Runtime.InteropServices.SEHException
    '            _strLastError = "PCMiler could not start. The application is not available or may not be configured correctly. Please open PC Miler on your workstation to confirm that it is working properly.  Then select the Restart PC Miler menu item using the Optimization pull down menu."
    '        Catch ex As Exception
    '            _strLastError = "PCMiler could not start. Cannot execute PCMilerStart.  " &  getErrorMessage(ex)
    '        End Try
    '    End If

    '    Return gServerID

    'End Function

    '' Return the currently open server's ID for use in other functions
    'Private Function Server() As Short Implements IPCMiles.Server
    '    Server = gServerID
    'End Function

    Public Function zipcode(ByVal CityName As String, _
                                ByVal LoggingOn As Boolean, _
                                ByVal LogFileName As String) As String Implements IPCMiles.zipcode

        Dim strRet As String = ""
        Dim oPCM As New PCM.PCMiler
        _strLastError = ""
        gLastError = ""
        Try
            oPCM.Url = Me.WebServiceURL
            Dim strLastError As String = ""
            strRet = oPCM.zipcode(CityName, _
                                        Debug, _
                                        LoggingOn, _
                                        KeepLogDays, _
                                        SaveOldLog, _
                                        LogFileName, _
                                        strLastError)
            If Not String.IsNullOrEmpty(strLastError) Then _strLastError = strLastError

        Catch ex As System.Net.WebException
            _strLastError = formatWebException(ex, "zipcode")
        Catch ex As Exception
            _strLastError = "Cannot execute zipcode. " & getErrorMessage(ex)
        End Try
        Return strRet
    End Function

    ' ''' <summary>
    ' ''' Debug version of Method
    ' ''' </summary>
    ' ''' <param name="StopData"></param>
    ' ''' <param name="strConsNumber"></param>
    ' ''' <param name="dblBatchID"></param>
    ' ''' <param name="blnKeepStopNumbers"></param>
    ' ''' <param name="BaddAddresses"></param>
    ' ''' <param name="LoggingOn"></param>
    ' ''' <param name="LogFileName"></param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Function PCMReSync(ByVal StopData As clsPCMDataStops, _
    '                            ByVal strConsNumber As String, _
    '                            ByVal dblBatchID As Double, _
    '                            ByVal blnKeepStopNumbers As Boolean, _
    '                            ByRef BaddAddresses As clsPCMBadAddresses, _
    '                            ByVal LoggingOn As Boolean, _
    '                            ByVal LogFileName As String) As clsAllStops Implements IPCMiles.PCMReSync

    '    Dim oclsAllStops As clsAllStops = Nothing
    '    'Dim oPCM As New PCM.PCMiler
    '    _strLastError = ""
    '    gLastError = ""
    '    Try
    '        'oPCM.Url = Me.WebServiceURL
    '        Dim strLastError As String = ""
    '        Dim arrStopData() As NGL.Interfaces.clsPCMDataStop ' PCM.clsPCMDataStop
    '        For i As Integer = 1 To StopData.COUNT
    '            If arrStopData Is Nothing Then
    '                ReDim Preserve arrStopData(0)
    '            Else
    '                ReDim Preserve arrStopData(arrStopData.Length)
    '            End If
    '            Dim oStop As New NGL.Interfaces.clsPCMDataStop ' PCM.clsPCMDataStop
    '            With oStop
    '                .BookControl = StopData.Item(i).BookControl
    '                .BookCustCompControl = StopData.Item(i).BookCustCompControl
    '                .BookDestAddress1 = StopData.Item(i).BookDestAddress1
    '                .BookDestCity = StopData.Item(i).BookDestCity
    '                .BookDestState = StopData.Item(i).BookDestState
    '                .BookDestZip = StopData.Item(i).BookDestZip
    '                .BookLoadControl = StopData.Item(i).BookLoadControl
    '                .BookODControl = StopData.Item(i).BookODControl
    '                .BookOrigAddress1 = StopData.Item(i).BookOrigAddress1
    '                .BookOrigCity = StopData.Item(i).BookOrigCity
    '                .BookOrigState = StopData.Item(i).BookOrigState
    '                .BookOrigZip = StopData.Item(i).BookOrigZip
    '                .BookProNumber = StopData.Item(i).BookProNumber
    '                .BookStopNo = StopData.Item(i).BookStopNo
    '                .DistType = StopData.Item(i).DistType
    '                .LaneOriginAddressUse = StopData.Item(i).LaneOriginAddressUse
    '                .RouteType = StopData.Item(i).RouteType
    '            End With
    '            arrStopData(arrStopData.Length - 1) = oStop
    '        Next

    '        Dim arrAllStops() As NGL.Interfaces.clsAllStop ' PCM.clsAllStop
    '        Dim arrBadAddresses() As NGL.Interfaces.clsPCMBadAddress ' PCM.clsPCMBadAddress
    '        Dim oGlobalStopData As New NGL.Service.PCMiler.Debug.clsGlobalStopData
    '        Try
    '            Dim oPCmiles As New NGL.Service.PCMiler.Debug.PCMiles

    '            'Dim oPCmiles As New PCMiles
    '            oPCmiles.Debug = True
    '            oPCmiles.LoggingOn = LoggingOn
    '            oPCmiles.KeepLogDays = KeepLogDays
    '            oPCmiles.SaveOldLog = SaveOldLog
    '            oPCmiles.LogFileName = LogFileName
    '            oPCmiles.UseZipOnly = UseZipOnly
    '            oGlobalStopData = oPCmiles.PCMReSync(arrStopData, strConsNumber, dblBatchID, blnKeepStopNumbers, arrAllStops, arrBadAddresses)
    '            _strLastError = oPCmiles.LastError

    '        Catch ex As Exception
    '            _strLastError = ex.Message
    '        End Try

    '        'Dim oGlobalStopData As PCM.clsGlobalStopData = oPCM.PCMReSync(arrStopData, _
    '        '                            strConsNumber, _
    '        '                            dblBatchID, _
    '        '                            blnKeepStopNumbers, _
    '        '                            arrAllStops, _
    '        '                            arrBadAddresses, _
    '        '                            Debug, _
    '        '                            LoggingOn, _
    '        '                            KeepLogDays, _
    '        '                            SaveOldLog, _
    '        '                            LogFileName, _
    '        '                            strLastError)
    '        'If Not String.IsNullOrEmpty(strLastError) Then _strLastError = strLastError
    '        If Not arrBadAddresses Is Nothing AndAlso arrBadAddresses.Length > 0 Then
    '            For i As Integer = 0 To arrBadAddresses.Length - 1
    '                Dim oBadOrig As New clsAddress
    '                With oBadOrig
    '                    .strAddress = arrBadAddresses(i).objOrig.strAddress
    '                    .strCity = arrBadAddresses(i).objOrig.strCity
    '                    .strState = arrBadAddresses(i).objOrig.strState
    '                    .strZip = arrBadAddresses(i).objOrig.strZip
    '                End With
    '                Dim oBadDest As New clsAddress
    '                With oBadDest
    '                    .strAddress = arrBadAddresses(i).objDest.strAddress
    '                    .strCity = arrBadAddresses(i).objDest.strCity
    '                    .strState = arrBadAddresses(i).objDest.strState
    '                    .strZip = arrBadAddresses(i).objDest.strZip
    '                End With
    '                Dim oBadPCMOrig As New clsAddress
    '                With oBadPCMOrig
    '                    .strAddress = arrBadAddresses(i).objPCMOrig.strAddress
    '                    .strCity = arrBadAddresses(i).objPCMOrig.strCity
    '                    .strState = arrBadAddresses(i).objPCMOrig.strState
    '                    .strZip = arrBadAddresses(i).objPCMOrig.strZip
    '                End With
    '                Dim oBadPCMDest As New clsAddress
    '                With oBadPCMDest
    '                    .strAddress = arrBadAddresses(i).objPCMDest.strAddress
    '                    .strCity = arrBadAddresses(i).objPCMDest.strCity
    '                    .strState = arrBadAddresses(i).objPCMDest.strState
    '                    .strZip = arrBadAddresses(i).objPCMDest.strZip
    '                End With

    '                BaddAddresses.Add(arrBadAddresses(i).BookControl, _
    '                    arrBadAddresses(i).LaneControl, _
    '                    oBadOrig, _
    '                    oBadDest, _
    '                    oBadPCMOrig, _
    '                    oBadPCMDest, _
    '                    arrBadAddresses(i).Message, _
    '                    arrBadAddresses(i).BatchID)
    '            Next
    '        End If
    '        If Not oGlobalStopData Is Nothing Then
    '            oclsAllStops = New clsAllStops
    '            With oclsAllStops
    '                .AutoCorrectBadLaneZipCodes = oGlobalStopData.AutoCorrectBadLaneZipCodes
    '                .BatchID = oGlobalStopData.BatchID
    '                .DestZip = oGlobalStopData.DestZip
    '                .FailedAddressMessage = oGlobalStopData.FailedAddressMessage
    '                .OriginZip = oGlobalStopData.OriginZip
    '                .TotalMiles = oGlobalStopData.TotalMiles
    '                If Not arrAllStops Is Nothing AndAlso arrAllStops.Length > 0 Then
    '                    For i As Integer = 0 To arrAllStops.Length - 1
    '                        .Add(arrAllStops(i).StopNumber, arrAllStops(i).Stopname, arrAllStops(i).ID1, arrAllStops(i).ID2, arrAllStops(i).TruckDesignator, arrAllStops(i).TruckNumber, arrAllStops(i).SeqNbr, arrAllStops(i).DistToPrev, arrAllStops(i).TotalRouteCost, arrAllStops(i).ConsNumber)
    '                    Next
    '                End If
    '            End With
    '        End If
    '    Catch ex As System.Net.WebException
    '        _strLastError = formatWebException(ex, "PCMReSync")
    '    Catch ex As Exception
    '        _strLastError = "Cannot execute PCMReSync. " & getErrorMessage(ex)
    '    End Try
    '    Return oclsAllStops

    'End Function


    Public Function PCMReSync(ByVal StopData As clsPCMDataStops, _
                                ByVal strConsNumber As String, _
                                ByVal dblBatchID As Double, _
                                ByVal blnKeepStopNumbers As Boolean, _
                                ByRef BaddAddresses As clsPCMBadAddresses, _
                                ByVal LoggingOn As Boolean, _
                                ByVal LogFileName As String) As clsAllStops Implements IPCMiles.PCMReSync

        Dim oclsAllStops As clsAllStops = Nothing
        Dim oPCM As New PCM.PCMiler
        _strLastError = ""
        gLastError = ""
        Try
            oPCM.Url = Me.WebServiceURL
            Dim strLastError As String = ""
            Dim arrStopData() As PCM.clsPCMDataStop
            For i As Integer = 1 To StopData.COUNT
                If arrStopData Is Nothing Then
                    ReDim Preserve arrStopData(0)
                Else
                    ReDim Preserve arrStopData(arrStopData.Length)
                End If
                Dim oStop As New PCM.clsPCMDataStop
                With oStop
                    .BookControl = StopData.Item(i).BookControl
                    .BookCustCompControl = StopData.Item(i).BookCustCompControl
                    .BookDestAddress1 = StopData.Item(i).BookDestAddress1
                    .BookDestCity = StopData.Item(i).BookDestCity
                    .BookDestState = StopData.Item(i).BookDestState
                    .BookDestZip = StopData.Item(i).BookDestZip
                    .BookLoadControl = StopData.Item(i).BookLoadControl
                    .BookODControl = StopData.Item(i).BookODControl
                    .BookOrigAddress1 = StopData.Item(i).BookOrigAddress1
                    .BookOrigCity = StopData.Item(i).BookOrigCity
                    .BookOrigState = StopData.Item(i).BookOrigState
                    .BookOrigZip = StopData.Item(i).BookOrigZip
                    .BookProNumber = StopData.Item(i).BookProNumber
                    .BookStopNo = StopData.Item(i).BookStopNo
                    .DistType = StopData.Item(i).DistType
                    .LaneOriginAddressUse = StopData.Item(i).LaneOriginAddressUse
                    .RouteType = StopData.Item(i).RouteType
                End With
                arrStopData(arrStopData.Length - 1) = oStop
            Next

            Dim arrAllStops() As PCM.clsAllStop
            Dim arrBadAddresses() As PCM.clsPCMBadAddress
            Dim oGlobalStopData As PCM.clsGlobalStopData = oPCM.PCMReSyncEX(arrStopData, _
                                        strConsNumber, _
                                        dblBatchID, _
                                        blnKeepStopNumbers, _
                                        arrAllStops, _
                                        arrBadAddresses, _
                                        Debug, _
                                        LoggingOn, _
                                        KeepLogDays, _
                                        SaveOldLog, _
                                        LogFileName, _
                                        UseZipOnly, _
                                        strLastError)
            If Not String.IsNullOrEmpty(strLastError) Then _strLastError = strLastError
            If Not arrBadAddresses Is Nothing AndAlso arrBadAddresses.Length > 0 Then
                For i As Integer = 0 To arrBadAddresses.Length - 1
                    Dim oBadOrig As New clsAddress
                    With oBadOrig
                        .strAddress = arrBadAddresses(i).objOrig.strAddress
                        .strCity = arrBadAddresses(i).objOrig.strCity
                        .strState = arrBadAddresses(i).objOrig.strState
                        .strZip = arrBadAddresses(i).objOrig.strZip
                    End With
                    Dim oBadDest As New clsAddress
                    With oBadDest
                        .strAddress = arrBadAddresses(i).objDest.strAddress
                        .strCity = arrBadAddresses(i).objDest.strCity
                        .strState = arrBadAddresses(i).objDest.strState
                        .strZip = arrBadAddresses(i).objDest.strZip
                    End With
                    Dim oBadPCMOrig As New clsAddress
                    With oBadPCMOrig
                        .strAddress = arrBadAddresses(i).objPCMOrig.strAddress
                        .strCity = arrBadAddresses(i).objPCMOrig.strCity
                        .strState = arrBadAddresses(i).objPCMOrig.strState
                        .strZip = arrBadAddresses(i).objPCMOrig.strZip
                    End With
                    Dim oBadPCMDest As New clsAddress
                    With oBadPCMDest
                        .strAddress = arrBadAddresses(i).objPCMDest.strAddress
                        .strCity = arrBadAddresses(i).objPCMDest.strCity
                        .strState = arrBadAddresses(i).objPCMDest.strState
                        .strZip = arrBadAddresses(i).objPCMDest.strZip
                    End With

                    BaddAddresses.Add(arrBadAddresses(i).BookControl, _
                        arrBadAddresses(i).LaneControl, _
                        oBadOrig, _
                        oBadDest, _
                        oBadPCMOrig, _
                        oBadPCMDest, _
                        arrBadAddresses(i).Message, _
                        arrBadAddresses(i).BatchID)
                Next
            End If
            If Not oGlobalStopData Is Nothing Then
                oclsAllStops = New clsAllStops
                With oclsAllStops
                    .AutoCorrectBadLaneZipCodes = oGlobalStopData.AutoCorrectBadLaneZipCodes
                    .BatchID = oGlobalStopData.BatchID
                    .DestZip = oGlobalStopData.DestZip
                    .FailedAddressMessage = oGlobalStopData.FailedAddressMessage
                    .OriginZip = oGlobalStopData.OriginZip
                    .TotalMiles = oGlobalStopData.TotalMiles
                    If Not arrAllStops Is Nothing AndAlso arrAllStops.Length > 0 Then
                        For i As Integer = 0 To arrAllStops.Length - 1
                            .Add(arrAllStops(i).StopNumber, arrAllStops(i).Stopname, arrAllStops(i).ID1, arrAllStops(i).ID2, arrAllStops(i).TruckDesignator, arrAllStops(i).TruckNumber, arrAllStops(i).SeqNbr, arrAllStops(i).DistToPrev, arrAllStops(i).TotalRouteCost, arrAllStops(i).ConsNumber)
                        Next
                    End If
                End With
            End If
        Catch ex As System.Net.WebException
            _strLastError = formatWebException(ex, "PCMReSync")
        Catch ex As Exception
            _strLastError = "Cannot execute PCMReSync. " & getErrorMessage(ex)
        End Try
        Return oclsAllStops

    End Function



    Public Function getRouteMiles(ByRef sRoute As clsSimpleStop(), _
                                ByVal LoggingOn As Boolean, _
                                ByVal LogFileName As String) As clsPCMReturn Implements IPCMiles.getRouteMiles
        Dim oclsPCMRet As New clsPCMReturn
        Dim oPCM As New PCM.PCMiler
        _strLastError = ""
        gLastError = ""
        Try
            oPCM.Url = Me.WebServiceURL
            Dim strLastError As String = ""
            If Not sRoute Is Nothing AndAlso sRoute.Length > 0 Then
                Dim intStops As Integer = sRoute.Length - 1
                Dim arrRoute(intStops) As PCM.clsSimpleStop
                For i As Integer = 0 To intStops
                    arrRoute(i) = New PCM.clsSimpleStop
                    With arrRoute(i)
                        .Address = sRoute(i).Address
                        .StopNumber = sRoute(i).StopNumber
                    End With
                Next
                Dim oPCMRetVal As PCM.clsPCMReturn = oPCM.getRouteMiles(arrRoute, _
                                                                        Debug, _
                                                                        LoggingOn, _
                                                                        KeepLogDays, _
                                                                        SaveOldLog, _
                                                                        LogFileName, _
                                                                        strLastError)
                If Not String.IsNullOrEmpty(strLastError) Then _strLastError = strLastError
                If Not oPCMRetVal Is Nothing Then
                    oclsPCMRet.Message = oPCMRetVal.Message
                    oclsPCMRet.RetVal = oPCMRetVal.RetVal
                End If
                If Not arrRoute Is Nothing Then
                    For i As Integer = 0 To intStops
                        If Not arrRoute(i) Is Nothing Then
                            With sRoute(i)
                                .LegCost = arrRoute(i).LegCost
                                .LegHours = arrRoute(i).LegHours
                                .LegMiles = arrRoute(i).LegMiles
                                .TotalCost = arrRoute(i).TotalCost
                                .TotalHours = arrRoute(i).TotalHours
                                .TotalMiles = arrRoute(i).TotalMiles
                            End With
                        End If
                    Next
                End If
            Else
                oclsPCMRet.Message = "No stop data was provided!"
                oclsPCMRet.RetVal = 0
            End If

        Catch ex As System.Net.WebException
            _strLastError = formatWebException(ex, "getRouteMiles")
        Catch ex As Exception
            _strLastError = "Cannot execute getRouteMiles. " & getErrorMessage(ex)
        End Try
        Return oclsPCMRet

    End Function



    'Private Function convertLatLongToDec(ByVal strLatLong As String) As Double

    '    Dim strhemisphere As String = "W"
    '    Dim intdegrees As Integer = 0
    '    Dim dblminutes As Double = 0
    '    Dim dblseconds As Double = 0
    '    Dim intmulitplier As Short = 0
    '    Dim strDegrees As String = ""
    '    Dim strMinutes As String = ""
    '    Dim strSeconds As String = ""
    '    If strLatLong.Length > 2 Then strDegrees = Left(strLatLong, 3)
    '    If strLatLong.Length > 5 Then strMinutes = Mid(strLatLong, 4, 2)
    '    If strLatLong.Length > 7 Then strSeconds = Mid(strLatLong, 6, 2)
    '    If strLatLong.Length > 8 Then strhemisphere = Right(strLatLong, 1)
    '    Integer.TryParse(strDegrees, intdegrees)
    '    Double.TryParse(strMinutes, dblminutes)
    '    Double.TryParse(strSeconds, dblseconds)


    '    'intdegrees = CShort(Left(strLatLong, 3))
    '    'dblminutes = CDbl(Mid(strLatLong, 4, 2))
    '    'dblseconds = CDbl(Mid(strLatLong, 6, 2))
    '    strhemisphere = Right(strLatLong, 1)
    '    If dblminutes <> 0 Then dblminutes = dblminutes / 60.0#
    '    If dblseconds <> 0 Then dblseconds = dblseconds / 3600.0#

    '    If strhemisphere = "W" Or strhemisphere = "S" Then
    '        intmulitplier = -1
    '    Else
    '        intmulitplier = 1
    '    End If

    '    Return (intdegrees + dblminutes + dblseconds) * intmulitplier


    'End Function



    ''-----------------------------------------------------------
    ''NOTE: PCMilerStart must be called by the calling function.
    ''-----------------------------------------------------------
    'Private Function validateAddress(ByVal intTrip1 As Integer, _
    '                                ByVal strAddressType As String, _
    '                                ByVal strItemNumber As String, _
    '                                ByVal strItemType As String, _
    '                                ByRef objSource As clsAddress, _
    '                                ByRef objPCM As clsAddress, _
    '                                ByRef strStopName As String, _
    '                                ByRef strWarnings As String) As Boolean
    '    Dim blnMatchFound As Boolean = False
    '    Dim strPCMilerCityState() As String
    '    Dim buffer As StringBuilder = New StringBuilder(256)
    '    Dim buff As String = ""
    '    Dim strMatchZip As String = ""
    '    Dim strZip As String = ""
    '    Dim intDash As Integer = 0
    '    'set default value of function to false
    '    validateAddress = False
    '    intDash = InStr(1, objSource.strZip, "-")
    '    If intDash Then
    '        strZip = Left(objSource.strZip, intDash - 1)
    '    Else
    '        strZip = objSource.strZip
    '    End If
    '    strStopName = objSource.strZip
    '    If PCMSCheckPlaceName(intTrip1, strZip) > 0 Then
    '        blnMatchFound = True
    '    Else
    '        blnMatchFound = False
    '        strWarnings &= "The " & strAddressType & " postal code, " & strZip & " ,for " & strItemType & " " & strItemNumber & " is not valid.  All postal codes are required." & vbCrLf
    '        Return False
    '    End If
    '    objPCM.strAddress = "** Address Does Not Exist **" 'objSource.strAddress
    '    objPCM.strCity = ""
    '    objPCM.strState = ""
    '    objPCM.strZip = ""
    '    If blnMatchFound Then objPCM.strZip = strZip
    '    'Get the PCmiler street address
    '    Dim strSource As String
    '    strSource = strZip & " " & objSource.strCity & ", " & objSource.strState & ";  " & objSource.strAddress
    '    Dim intRetVal As Integer
    '    intRetVal = PCMSLookup(intTrip1, strSource, 2)
    '    If intRetVal < 1 Then
    '        intRetVal = PCMSLookup(intTrip1, strZip & ";" & objSource.strAddress, 2)
    '    End If
    '    If intRetVal > 0 Then
    '        If PCMSGetMatch(intTrip1, 0, buffer, 254) > 0 Then
    '            buff = buffer.ToString
    '            'Debug.Print buff
    '            If InStr(1, buff, ";") Then
    '                objPCM.strAddress = Trim(simpleStreetScrubber(Replace(Mid(buff, InStr(1, buff, ";") + 1, Len(buff)), Chr(0), "")))
    '                If InStr(1, objPCM.strAddress, "&") Then
    '                    objPCM.strAddress = "** Address Does Not Exist **"
    '                    strWarnings = strWarnings & "The " & strAddressType & " address for " & strItemType & " " & strItemNumber & " does not exist in PCMiler. Using FreightMaster postal code for routing." & vbCrLf
    '                Else
    '                    strStopName = Trim(Replace(buff, Chr(0), ""))
    '                    'test for US vs Canadian postal codes
    '                    Dim strTestZip As String
    '                    '                If Len(Trim(strZip)) < 1 Then
    '                    '                    'no zip code provided so test if first 5 characters is a number
    '                    '                    strTestZip = Trim(Left(buff, 5))
    '                    '                Else
    '                    '                    strTestZip = strZip
    '                    '                End If
    '                    strTestZip = Trim(Left(buff, 5))
    '                    If IsNumeric(strTestZip) Then
    '                        objPCM.strZip = Trim(Left(buff, 6))
    '                        strMatchZip = Left(strZip, 5)
    '                        buff = Mid(buff, 6, InStr(1, buff, ";"))
    '                    Else
    '                        strMatchZip = strZip
    '                        objPCM.strZip = strZip
    '                        buff = Mid(buff, 1, InStr(1, buff, ";") - 1)
    '                    End If
    '                    strPCMilerCityState = Split(buff, ",")
    '                    objPCM.strCity = Trim(strPCMilerCityState(0))
    '                    objPCM.strState = Trim(strPCMilerCityState(1))
    '                    'Compare Our Address with PC Milers Best Match.
    '                    'If issues we return a value in strMessage to be logged in the database
    '                    If blnMatchFound Then
    '                        If LCase(strMatchZip) <> LCase(objPCM.strZip) Then
    '                            'the zip code does not match so log a warning that the
    '                            'address may not match PCMiler Database using postal code
    '                            'for origin
    '                            strWarnings = strWarnings & "The " & strAddressType & " postal code for " & strItemType & " " & strItemNumber & " does not match the PCMiler postal code. Using FreightMaster postal code for routing." & vbCrLf
    '                            'set stop name back to zip code
    '                            strStopName = objSource.strZip
    '                        Else
    '                            'check if the state matches we only use full addressing if states match
    '                            If LCase(objPCM.strState) <> LCase(objSource.strState) Then
    '                                strWarnings = strWarnings & "The " & strAddressType & " State for " & strItemType & " " & strItemNumber & " does not match the PCMiler State. Using FreightMaster postal code for routing." & vbCrLf
    '                                'set stop name back to zip code
    '                                strStopName = objSource.strZip
    '                            ElseIf LCase(objPCM.strAddress) <> LCase(objSource.strAddress) Then
    '                                strWarnings = strWarnings & "The " & strAddressType & " Street Address for " & strItemType & " " & strItemNumber & " does not match the PCMiler Street Address. Using PCMiler Street Address for routing." & vbCrLf
    '                                blnMatchFound = True
    '                            Else
    '                                blnMatchFound = True
    '                            End If
    '                        End If
    '                    Else
    '                        'the postal code could not be found we assume user error on
    '                        'postal code so check city and state for match (if they do not match
    '                        'the entire address fails and falls into the bad postal code trap farther down)
    '                        If objPCM.strCity = objSource.strCity Or objPCM.strState = objSource.strState Then
    '                            strWarnings = strWarnings & "The " & strAddressType & " Postal Code Is Not Valid Using Closest Match (Please Correct) for " & strItemType & " " & strItemNumber & ".  Using PCMiler address for routing." & vbCrLf
    '                            blnMatchFound = True
    '                        Else
    '                            'set stop name back to zip code
    '                            strStopName = objSource.strZip
    '                            strWarnings = strWarnings & "The " & strAddressType & " Address and postal code for " & strItemType & " " & strItemNumber & " is not valid.  The load could not be routed." & vbCrLf
    '                        End If
    '                    End If
    '                End If
    '            Else
    '                strWarnings = strWarnings & "There was a problem with the " & strAddressType & " address for " & strItemType & " " & strItemNumber & " . Using FreightMaster postal code for routing." & vbCrLf
    '            End If
    '        End If
    '    Else
    '        If blnMatchFound Then
    '            strWarnings = strWarnings & "The " & strAddressType & " Street Address for " & strItemType & " " & strItemNumber & " cannot be found for the postal code provided. Using FreightMaster postal code for routing." & vbCrLf
    '        End If
    '    End If

    '    validateAddress = blnMatchFound

    'End Function


    'Private Function AddStop(ByRef objOrig As clsAddress, _
    '                        ByRef objDest As clsAddress, _
    '                        ByRef objPCMOrig As clsAddress, _
    '                        ByRef objPCMDest As clsAddress, _
    '                        ByRef blnAddOrigin As Boolean, _
    '                        ByRef objAllStops As clsAllStops, _
    '                        ByRef blnAddressValid As Boolean, _
    '                        ByRef strOrigStopName As String, _
    '                        ByRef strDestStopName As String, _
    '                        ByVal intTrip1 As Integer, _
    '                        ByVal strItemNumber As String, _
    '                        ByVal strItemType As String, _
    '                        ByVal intBookControl As Integer, _
    '                        ByVal intLaneControl As Integer, _
    '                        ByRef BaddAddresses As clsPCMBadAddresses) As Boolean
    '    Dim blnRet As Boolean = False
    '    Dim strOriginAddressWarnings As String = ""
    '    Dim strWarnings As String = ""
    '    Dim blnLogBadAddress As Boolean = False
    '    Dim blnOriginAddressValid As Boolean = True
    '    Dim Ret As Long = 0
    '    'set default value of function to blnRet (false)
    '    AddStop = blnRet
    '    blnAddressValid = False
    '    '********** Validate and Add the Origin Address *********************
    '    strOriginAddressWarnings = ""
    '    If validateAddress(intTrip1, _
    '                    "Origin", _
    '                    strItemNumber, _
    '                    strItemType, _
    '                    objOrig, _
    '                    objPCMOrig, _
    '                    strOrigStopName, _
    '                    strOriginAddressWarnings) Then
    '        'NOTE: Warnings are not logged until we process the Destination Address Below

    '        If Len(Trim(strOriginAddressWarnings)) > 0 Then
    '            If InStr(1, strWarnings, "(Please Correct)") > 0 And objAllStops.AutoCorrectBadLaneZipCodes = 1 Then
    '                If Len(Trim(objPCMOrig.strZip)) > 0 Then
    '                    objAllStops.OriginZip = objPCMOrig.strZip
    '                    strOriginAddressWarnings = ""
    '                Else
    '                    blnLogBadAddress = True
    '                End If
    '            Else
    '                blnLogBadAddress = True
    '            End If
    '        End If
    '    Else
    '        blnLogBadAddress = True
    '        If blnAddOrigin Then
    '            blnOriginAddressValid = False
    '            objAllStops.FailedAddressMessage = objAllStops.FailedAddressMessage & strOriginAddressWarnings
    '        End If
    '    End If
    '    If blnAddOrigin Then
    '        Ret = PCMSAddStop(intTrip1, strOrigStopName)
    '        If Ret < 1 Then
    '            'The stopname cannot be found so reset to zipcode only
    '            If strOrigStopName = objOrig.strZip Then
    '                'This is a total failure
    '                blnOriginAddressValid = False
    '                objAllStops.FailedAddressMessage = objAllStops.FailedAddressMessage & "The origin address cannot be found and the postal code is not valid.  Cannot route load."
    '                objPCMOrig.strAddress = "** Address Does Not Exist **"
    '                objPCMOrig.strCity = ""
    '                objPCMOrig.strState = ""
    '                objPCMOrig.strZip = ""
    '            Else
    '                'try to use the zip code
    '                strOrigStopName = objOrig.strZip
    '                objPCMOrig.strAddress = "** Address Does Not Exist **"
    '                objPCMOrig.strCity = ""
    '                objPCMOrig.strState = ""
    '                objPCMOrig.strZip = objOrig.strZip
    '                Ret = PCMSAddStop(intTrip1, strOrigStopName)
    '                If Ret < 1 Then
    '                    'Give up
    '                    objPCMOrig.strZip = ""
    '                    blnOriginAddressValid = False
    '                    objAllStops.FailedAddressMessage = objAllStops.FailedAddressMessage & "The origin address cannot be found and the postal code is not valid.  Cannot route load."
    '                Else
    '                    blnOriginAddressValid = True
    '                    blnAddOrigin = False
    '                End If
    '            End If
    '        Else
    '            blnOriginAddressValid = True
    '            blnAddOrigin = False
    '        End If
    '    End If
    '    strWarnings = ""
    '    If validateAddress(intTrip1, _
    '                        "Destination", _
    '                        strItemNumber, _
    '                        strItemType, _
    '                        objDest, _
    '                        objPCMDest, _
    '                        strDestStopName, _
    '                        strWarnings) Then
    '        If blnOriginAddressValid Then
    '            Ret = PCMSAddStop(intTrip1, strDestStopName)
    '            If Ret < 1 Then
    '                If strDestStopName = objDest.strZip Then
    '                    objAllStops.FailedAddressMessage = objAllStops.FailedAddressMessage & "The destination address cannot be found and the postal code is not valid.  Cannot route load."
    '                    objPCMDest.strAddress = "** Address Does Not Exist **"
    '                    objPCMDest.strCity = ""
    '                    objPCMDest.strState = ""
    '                    objPCMDest.strZip = ""
    '                Else
    '                    'Try to use the zip code
    '                    strDestStopName = objDest.strZip
    '                    objPCMDest.strAddress = "** Address Does Not Exist **"
    '                    objPCMDest.strCity = ""
    '                    objPCMDest.strState = ""
    '                    objPCMDest.strZip = objDest.strZip
    '                    Ret = PCMSAddStop(intTrip1, strDestStopName)
    '                    If Ret < 1 Then
    '                        'Give up
    '                        objAllStops.FailedAddressMessage = objAllStops.FailedAddressMessage & "The destination address cannot be found and the postal code is not valid.  Cannot route load."
    '                        objPCMDest.strZip = ""
    '                    Else
    '                        blnAddressValid = True
    '                    End If
    '                End If
    '            Else
    '                blnAddressValid = True
    '            End If
    '        End If
    '        If Len(Trim(strWarnings)) > 0 Then
    '            If InStr(1, strWarnings, "(Please Correct)") > 0 And objAllStops.AutoCorrectBadLaneZipCodes = 1 Then
    '                If Len(Trim(objPCMDest.strZip)) > 0 Then
    '                    objAllStops.DestZip = objPCMDest.strZip
    '                    strWarnings = ""
    '                Else
    '                    blnLogBadAddress = True
    '                End If
    '            Else
    '                blnLogBadAddress = True
    '            End If
    '        End If
    '    Else
    '        objAllStops.FailedAddressMessage = objAllStops.FailedAddressMessage & strWarnings
    '        blnLogBadAddress = True
    '    End If
    '    If blnLogBadAddress Then
    '        'add the bad address to the collection
    '        BaddAddresses.Add(intBookControl, intLaneControl, objOrig, objDest, objPCMOrig, objPCMDest, strOriginAddressWarnings & strWarnings, objAllStops.BatchID)
    '    End If

    '    Return True

    'End Function

    'Private Function simpleStreetScrubber(ByVal strStreet As String) As String

    '    Try
    '        strStreet = " " & LCase(strStreet) & " "
    '        If InStr(1, strStreet, " dr ", vbTextCompare) > 0 Then
    '            strStreet = Replace(strStreet, " dr ", " drive ", 1, 1, vbTextCompare)
    '        End If
    '        If InStr(1, strStreet, " ave ", vbTextCompare) > 0 Then
    '            strStreet = Replace(strStreet, " ave ", " avenue ", 1, 1, vbTextCompare)
    '        End If
    '        If InStr(1, strStreet, " blvd ", vbTextCompare) > 0 Then
    '            strStreet = Replace(strStreet, " blvd ", " boulevard ", 1, 1, vbTextCompare)
    '        End If
    '        If InStr(1, strStreet, " st ", vbTextCompare) > 0 Then
    '            strStreet = Replace(strStreet, " st ", " street ", 1, 1, vbTextCompare)
    '        End If
    '        If InStr(1, strStreet, " e ", vbTextCompare) > 0 Then
    '            strStreet = Replace(strStreet, " e ", " east ", 1, 1, vbTextCompare)
    '        End If
    '        If InStr(1, strStreet, " w ", vbTextCompare) > 0 Then
    '            strStreet = Replace(strStreet, " w ", " west ", 1, 1, vbTextCompare)
    '        End If
    '        If InStr(1, strStreet, " s ", vbTextCompare) > 0 Then
    '            strStreet = Replace(strStreet, " s ", " south ", 1, 1, vbTextCompare)
    '        End If
    '        If InStr(1, strStreet, " n ", vbTextCompare) > 0 Then
    '            strStreet = Replace(strStreet, " n ", " north ", 1, 1, vbTextCompare)
    '        End If
    '        If InStr(1, strStreet, " cir ", vbTextCompare) > 0 Then
    '            strStreet = Replace(strStreet, " cir ", " circle ", 1, 1, vbTextCompare)
    '        End If
    '        If InStr(1, strStreet, " ct ", vbTextCompare) > 0 Then
    '            strStreet = Replace(strStreet, " ct ", " court ", 1, 1, vbTextCompare)
    '        End If
    '        If InStr(1, strStreet, " ne ", vbTextCompare) > 0 Then
    '            strStreet = Replace(strStreet, " ne ", " northeast ", 1, 1, vbTextCompare)
    '        End If
    '        If InStr(1, strStreet, " nw ", vbTextCompare) > 0 Then
    '            strStreet = Replace(strStreet, " nw ", " northwest ", 1, 1, vbTextCompare)
    '        End If
    '        If InStr(1, strStreet, " pkwy ", vbTextCompare) > 0 Then
    '            strStreet = Replace(strStreet, " pkwy ", " parkway ", 1, 1, vbTextCompare)
    '        End If
    '        If InStr(1, strStreet, " rd ", vbTextCompare) > 0 Then
    '            strStreet = Replace(strStreet, " rd ", " road ", 1, 1, vbTextCompare)
    '        End If
    '        If InStr(1, strStreet, " trl ", vbTextCompare) > 0 Then
    '            strStreet = Replace(strStreet, " trl ", " trail ", 1, 1, vbTextCompare)
    '        End If
    '        If InStr(1, strStreet, " sq ", vbTextCompare) > 0 Then
    '            strStreet = Replace(strStreet, " sq ", " square ", 1, 1, vbTextCompare)
    '        End If
    '    Catch ex As Exception
    '        'do nothing
    '    End Try
    '    Return strStreet

    'End Function

    Private Function getErrorMessage(ByRef ex As Exception) As String
        Dim strRet As String = ""
        If Me.Debug Then strRet = ex.ToString Else strRet = ex.Message
        Return strRet
    End Function

    Private Function formatWebException(ByVal ex As Exception, ByVal strSource As String) As String
        Return "The PCMiler " & strSource & " procedure is not available." & vbCrLf & "There was a problem connecting to " & Me.WebServiceURL & vbCrLf & "Please check your company level parameter settings.  The actual error message is: " & vbCrLf & getErrorMessage(ex)
    End Function

    Public Sub AddSimpleStop(ByVal Address As String, ByVal StopNumber As Integer)
        Try
            Dim oSimpleStop As New clsSimpleStop
            With oSimpleStop
                .Address = Address
                .StopNumber = StopNumber
            End With
            If SimpleStopList Is Nothing Then
                SimpleStopList = New List(Of clsSimpleStop)
            End If

            SimpleStopList.Add(oSimpleStop)

        Catch ex As Exception
            _strLastError = "Cannot execute AddSimpleStop. " & getErrorMessage(ex)
        End Try

    End Sub

    Public Sub ClearSimpleStops()
        SimpleStopList = New List(Of clsSimpleStop)
    End Sub

    Public Function GetSimpleStopLegMiles(ByVal StopNumber As Integer) As Double
        Try
            If SimpleStopList Is Nothing OrElse SimpleStopList.Count < 1 Then
                Return 0
            Else
                For Each oStop As clsSimpleStop In SimpleStopList
                    If oStop.StopNumber = StopNumber Then
                        Return oStop.LegMiles
                    End If
                Next
            End If
        Catch ex As Exception
            _strLastError = "Cannot execute GetSimpleStopLegMiles. " & getErrorMessage(ex)
        End Try
        Return 0
    End Function

    Public Function GetSimpleStopTotalMiles(ByVal StopNumber As Integer) As Double
        Try
            If SimpleStopList Is Nothing OrElse SimpleStopList.Count < 1 Then
                Return 0
            Else
                For Each oStop As clsSimpleStop In SimpleStopList
                    If oStop.StopNumber = StopNumber Then
                        Return oStop.TotalMiles
                    End If
                Next
            End If
        Catch ex As Exception
            _strLastError = "Cannot execute GetSimpleStopTotalMiles. " & getErrorMessage(ex)
        End Try
        Return 0
    End Function

    Public Function CalculateSimpleStopData() As clsPCMReturn
        Dim oPCMRet As New clsPCMReturn
        Try
            If SimpleStopList Is Nothing OrElse SimpleStopList.Count < 1 Then
                With oPCMRet
                    .Message = "Nothing to do.  Please add some simple stop data."
                    .RetVal = 0
                End With
            End If
            Dim oStops() As clsSimpleStop
            oStops = SimpleStopList.ToArray
            oPCMRet = getRouteMiles(oStops, False, "")
            If oPCMRet.RetVal > 0 AndAlso oStops.Length > 0 Then
                For i As Integer = 0 To oStops.Length - 1
                    For Each oStop As clsSimpleStop In SimpleStopList
                        With oStop
                            If .StopNumber = oStops(i).StopNumber Then
                                .LegCost = oStops(i).LegCost
                                .LegHours = oStops(i).LegHours
                                .LegMiles = oStops(i).LegMiles
                                .TotalCost = oStops(i).TotalCost
                                .TotalHours = oStops(i).TotalHours
                                .TotalMiles = oStops(i).TotalMiles
                                Exit For
                            End If
                        End With
                    Next
                Next
            End If
        Catch ex As Exception
            _strLastError = "Cannot execute CalculateSimpleStopData. " & getErrorMessage(ex)
        End Try
        Return oPCMRet
    End Function
#End Region

#Region "New Methods Added for v-5.1.4"


    Public Function PCMValidateAddress(ByVal strAddress As String) As Boolean Implements IPCMiles.PCMValidateAddress


        _strLastError = ""
        gLastError = ""
        Dim blnRet As Boolean = False
        Try
            'If Me.Debug Then
            '    Dim oPCmiles As New NGL.Service.PCMiler.Debug.PCMiles
            '    oPCmiles.Debug = True
            '    blnRet = oPCmiles.PCMValidateAddress(strAddress)
            '    If Not String.IsNullOrEmpty(oPCmiles.LastError) Then _strLastError = oPCmiles.LastError
            'Else
            Dim oPCM As New PCM.PCMiler
            oPCM.Url = Me.WebServiceURL
            Dim strLastError As String = ""
            blnRet = oPCM.PCMValidateAddress(strAddress, _
                                    Debug, _
                                    False, _
                                    KeepLogDays, _
                                    SaveOldLog, _
                                    "C:\PCMLog.txt", _
                                    strLastError)
            If Not String.IsNullOrEmpty(strLastError) Then _strLastError = strLastError
            'End If
        Catch ex As System.Net.WebException
            _strLastError = formatWebException(ex, "PCMValidateAddress")
        Catch ex As Exception
            _strLastError = "Cannot execute PCMValidateAddress. " & getErrorMessage(ex)
        End Try
        Return blnRet
    End Function


    ''' <summary>
    ''' The StopDate, BadAddresses and ReportRecords are passed by reference and 
    ''' results are returned to the caller.  The return vlaue clsPCMReturnEx as 
    ''' information about failed processes previously contained in the clsAllStops 
    ''' object.  Stop are now processed by address and not by pro number. 
    ''' </summary>
    ''' <param name="StopData"></param>
    ''' <param name="BadAddresses"></param>
    ''' <param name="ReportRecords"></param>
    ''' <param name="dblBatchID"></param>
    ''' <param name="blnKeepStopNumbers"></param>
    ''' <param name="LoggingOn"></param>
    ''' <param name="LogFileName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function PCMReSyncMultiStop(ByVal StopData As List(Of clsFMStopData), _
                               ByRef BadAddresses As clsPCMBadAddresses, _
                               ByRef ReportRecords As List(Of clsPCMReportRecord), _
                               ByVal dblBatchID As Double, _
                               ByVal blnKeepStopNumbers As Boolean, _
                               ByVal LoggingOn As Boolean, _
                               ByVal LogFileName As String) As clsPCMReturnEx Implements IPCMiles.PCMReSyncMultiStop

        'If Me.Debug Then Return PCMReSyncMultiStopDebug(StopData, BadAddresses, ReportRecords, dblBatchID, blnKeepStopNumbers, LoggingOn, LogFileName)
        Dim oPCMReturnEx As clsPCMReturnEx
        Dim oPCM As New PCM.PCMiler
        _strLastError = ""
        gLastError = ""
        Try
            oPCM.Url = Me.WebServiceURL
            Dim strLastError As String = ""
            Dim oGlobalStopData As New NGL.FreightMaster.PCMiler.PCM.clsGlobalStopData
            Dim arrFMStops As NGL.FreightMaster.PCMiler.PCM.clsFMStopData() = CreateFMStopArray(StopData)
            Dim arrPCMReports As NGL.FreightMaster.PCMiler.PCM.clsPCMReportRecord()
            Try
                'Dim oPCmiles As New NGL.Service.PCMiler.Debug.PCMiles
                ''Dim oPCmiles As New PCMiles
                'oPCmiles.Debug = True
                'oPCmiles.LoggingOn = LoggingOn
                'oPCmiles.KeepLogDays = KeepLogDays
                'oPCmiles.SaveOldLog = SaveOldLog
                'oPCmiles.LogFileName = LogFileName
                'oPCmiles.UseZipOnly = UseZipOnly
                oGlobalStopData = oPCM.PCMReSyncMultiStop(arrFMStops, _
                                                          dblBatchID, _
                                                          blnKeepStopNumbers, _
                                                          arrPCMReports, _
                                                          Debug, _
                                                          LoggingOn, _
                                                          KeepLogDays, _
                                                          SaveOldLog, _
                                                          LogFileName, _
                                                          UseZipOnly, _
                                                          strLastError)

                If Not String.IsNullOrEmpty(strLastError) Then _strLastError = strLastError

            Catch ex As Exception
                _strLastError = ex.Message
            End Try
            If Not arrFMStops Is Nothing AndAlso arrFMStops.Count > 0 Then UpdateFMStopList(StopData, arrFMStops)
            If Not arrPCMReports Is Nothing AndAlso arrPCMReports.Count > 0 Then UpdateFMReportRecords(ReportRecords, New List(Of NGL.FreightMaster.PCMiler.PCM.clsPCMReportRecord)(arrPCMReports))
            CreateBadAddressList(StopData, BadAddresses, dblBatchID)

            If Not oGlobalStopData Is Nothing Then
                oPCMReturnEx = New clsPCMReturnEx
                With oPCMReturnEx
                    .AutoCorrectBadLaneZipCodes = oGlobalStopData.AutoCorrectBadLaneZipCodes
                    .BatchID = oGlobalStopData.BatchID
                    .DestZip = oGlobalStopData.DestZip
                    .FailedAddressMessage = oGlobalStopData.FailedAddressMessage
                    .OriginZip = oGlobalStopData.OriginZip
                    .TotalMiles = oGlobalStopData.TotalMiles
                    .Results = StopData
                End With
            End If
        Catch ex As System.Net.WebException
            _strLastError = formatWebException(ex, "PCMReSyncMultiStop")
        Catch ex As Exception
            _strLastError = "Cannot execute PCMReSyncMultiStop. " & getErrorMessage(ex)
        End Try
        Return oPCMReturnEx
    End Function

    ' ''' <summary>
    ' ''' Debug version of Method
    ' ''' </summary>
    ' ''' <param name="StopData"></param>
    ' ''' <param name="dblBatchID"></param>
    ' ''' <param name="blnKeepStopNumbers"></param>
    ' ''' <param name="BadAddresses"></param>
    ' ''' <param name="LoggingOn"></param>
    ' ''' <param name="LogFileName"></param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Private Function PCMReSyncMultiStopDebug(ByVal StopData As List(Of clsFMStopData), _
    '                            ByRef BadAddresses As clsPCMBadAddresses, _
    '                            ByRef ReportRecords As List(Of clsPCMReportRecord), _
    '                            ByVal dblBatchID As Double, _
    '                            ByVal blnKeepStopNumbers As Boolean, _
    '                            ByVal LoggingOn As Boolean, _
    '                            ByVal LogFileName As String) As clsPCMReturnEx

    '    Dim oPCMReturnEx As clsPCMReturnEx
    '    _strLastError = ""
    '    gLastError = ""
    '    Try

    '        Dim strLastError As String = ""
    '        Dim oIFMStops As List(Of NGL.Interfaces.clsFMStopData) = CreateFMStopInterfaceList(StopData)
    '        Dim oGlobalStopData As New NGL.Service.PCMiler.Debug.clsGlobalStopData
    '        Dim oPCMReportRecords As New List(Of NGL.Interfaces.clsPCMReportRecord)
    '        Try
    '            Dim oPCmiles As New NGL.Service.PCMiler.Debug.PCMiles
    '            'Dim oPCmiles As New PCMiles
    '            oPCmiles.Debug = True
    '            oPCmiles.LoggingOn = LoggingOn
    '            oPCmiles.KeepLogDays = KeepLogDays
    '            oPCmiles.SaveOldLog = SaveOldLog
    '            oPCmiles.LogFileName = LogFileName
    '            oPCmiles.UseZipOnly = UseZipOnly
    '            oGlobalStopData = oPCmiles.PCMReSyncMultiStop(oIFMStops, _
    '                                                   dblBatchID, _
    '                                                   blnKeepStopNumbers, _
    '                                                   oPCMReportRecords)

    '            _strLastError = oPCmiles.LastError

    '        Catch ex As Exception
    '            _strLastError = ex.Message
    '        End Try
    '        UpdateFMStopList(StopData, oIFMStops)
    '        UpdateFMReportRecords(ReportRecords, oPCMReportRecords)
    '        CreateBadAddressList(StopData, BadAddresses, dblBatchID)

    '        If Not oGlobalStopData Is Nothing Then
    '            oPCMReturnEx = New clsPCMReturnEx
    '            With oPCMReturnEx
    '                .AutoCorrectBadLaneZipCodes = oGlobalStopData.AutoCorrectBadLaneZipCodes
    '                .BatchID = oGlobalStopData.BatchID
    '                .DestZip = oGlobalStopData.DestZip
    '                .FailedAddressMessage = oGlobalStopData.FailedAddressMessage
    '                .OriginZip = oGlobalStopData.OriginZip
    '                .TotalMiles = oGlobalStopData.TotalMiles
    '                .Results = StopData
    '            End With
    '        End If
    '    Catch ex As System.Net.WebException
    '        _strLastError = formatWebException(ex, "PCMReSyncMultiStopDebug")
    '    Catch ex As Exception
    '        _strLastError = "Cannot execute PCMReSyncMultiStopDebug. " & getErrorMessage(ex)
    '    End Try
    '    Return oPCMReturnEx

    'End Function

    Private Sub CreateBadAddressList(ByVal oStops As List(Of clsFMStopData), ByRef BadAddresses As clsPCMBadAddresses, ByVal dblBatchID As Double)

        Dim BadBookControls As New List(Of Integer)
        Dim oBadOrig As New clsAddress
        Dim oBadDest As New clsAddress
        Dim oBadPCMOrig As New clsAddress
        Dim oBadPCMDest As New clsAddress
        Dim intBookControl As Integer
        Dim oBadStops As List(Of clsFMStopData) = (From d In oStops Where d.LogBadAddress = True Select d).ToList

        If oBadStops Is Nothing OrElse oBadStops.Count < 1 Then Return

        For Each oStop As clsFMStopData In oBadStops

            intBookControl = oStop.BookControl
            If Not BadBookControls.Contains(intBookControl) Then

                'Add the bad address
                If oStop.LocationisOrigin Then
                    oBadOrig = New clsAddress
                    With oBadOrig
                        .strAddress = oStop.Street
                        .strCity = oStop.City
                        .strState = oStop.State
                        .strZip = oStop.Zip
                    End With
                    oBadPCMOrig = New clsAddress
                    With oBadPCMOrig
                        .strAddress = oStop.PCMilerStreet
                        .strCity = oStop.PCMilerCity
                        .strState = oStop.PCMilerState
                        .strZip = oStop.PCMilerZip
                    End With
                    'Lookup the destination
                    Dim oDest As clsFMStopData = (From d In oStops Where d.BookControl = intBookControl And d.LocationisOrigin = False Select d).First()
                    If Not oDest Is Nothing Then
                        oBadDest = New clsAddress
                        With oBadDest
                            .strAddress = oDest.Street
                            .strCity = oDest.City
                            .strState = oDest.State
                            .strZip = oDest.Zip
                        End With
                        oBadPCMDest = New clsAddress
                        With oBadPCMDest
                            .strAddress = oDest.PCMilerStreet
                            .strCity = oDest.PCMilerCity
                            .strState = oDest.PCMilerState
                            .strZip = oDest.PCMilerZip
                        End With
                    End If
                Else
                    oBadDest = New clsAddress
                    With oBadDest
                        .strAddress = oStop.Street
                        .strCity = oStop.City
                        .strState = oStop.State
                        .strZip = oStop.Zip
                    End With
                    oBadPCMDest = New clsAddress
                    With oBadPCMDest
                        .strAddress = oStop.PCMilerStreet
                        .strCity = oStop.PCMilerCity
                        .strState = oStop.PCMilerState
                        .strZip = oStop.PCMilerZip
                    End With
                    'Lookup the origin
                    Dim oOrig As clsFMStopData = (From d In oStops Where d.BookControl = intBookControl And d.LocationisOrigin = True Select d).First()
                    If Not oOrig Is Nothing Then
                        oBadOrig = New clsAddress
                        With oBadOrig
                            .strAddress = oOrig.Street
                            .strCity = oOrig.City
                            .strState = oOrig.State
                            .strZip = oOrig.Zip
                        End With
                        oBadPCMOrig = New clsAddress
                        With oBadPCMOrig
                            .strAddress = oOrig.PCMilerStreet
                            .strCity = oOrig.PCMilerCity
                            .strState = oOrig.PCMilerState
                            .strZip = oOrig.PCMilerZip
                        End With
                    End If
                End If
                BadAddresses.Add(intBookControl, _
                   oStop.BookODControl, _
                   oBadOrig, _
                   oBadDest, _
                   oBadPCMOrig, _
                   oBadPCMDest, _
                   oStop.Warning, _
                   dblBatchID)
                'Add the bad control number to the list
                BadBookControls.Add(intBookControl)
            End If




        Next

    End Sub

    Private Function CreateFMStopInterfaceList(ByVal StopData As List(Of clsFMStopData)) As List(Of NGL.Interfaces.clsFMStopData)
        Dim oIFMStops As New List(Of NGL.Interfaces.clsFMStopData)
        For i As Integer = 0 To StopData.Count - 1

            Dim oStop As New NGL.Interfaces.clsFMStopData
            With oStop
                .BookControl = StopData.Item(i).BookControl
                .BookCustCompControl = StopData.Item(i).BookCustCompControl
                .BookLoadControl = StopData.Item(i).BookLoadControl
                .BookODControl = StopData.Item(i).BookODControl
                .BookProNumber = StopData.Item(i).BookProNumber
                .LaneOriginAddressUse = StopData.Item(i).LaneOriginAddressUse
                .StopNumber = StopData.Item(i).StopNumber
                .RouteType = StopData.Item(i).RouteType
                .DistType = StopData.Item(i).DistType
                .Zip = StopData.Item(i).Zip
                .City = StopData.Item(i).City
                .State = StopData.Item(i).State
                .Street = StopData.Item(i).Street
                .LocationisOrigin = StopData.Item(i).LocationisOrigin
            End With
            oIFMStops.Add(oStop)
        Next
        Return oIFMStops
    End Function

    Private Function CreateFMStopArray(ByVal StopData As List(Of clsFMStopData)) As NGL.FreightMaster.PCMiler.PCM.clsFMStopData()

        Dim arrFMStops As NGL.FreightMaster.PCMiler.PCM.clsFMStopData() = (From d In StopData _
                                                                           Select New NGL.FreightMaster.PCMiler.PCM.clsFMStopData _
                                                                           With {.BookControl = d.BookControl _
                                                                               , .BookCustCompControl = d.BookCustCompControl _
                                                                               , .BookLoadControl = d.BookLoadControl _
                                                                               , .BookODControl = d.BookODControl _
                                                                               , .BookProNumber = d.BookProNumber _
                                                                               , .LaneOriginAddressUse = d.LaneOriginAddressUse _
                                                                               , .StopNumber = d.StopNumber _
                                                                               , .RouteType = d.RouteType _
                                                                               , .DistType = d.DistType _
                                                                               , .Zip = d.Zip _
                                                                               , .City = d.City _
                                                                               , .State = d.State _
                                                                               , .Street = d.Street _
                                                                               , .LocationisOrigin = d.LocationisOrigin}).ToArray

        Return arrFMStops
    End Function

    Private Sub UpdateFMStopList(ByRef StopData As List(Of clsFMStopData), ByVal oIFMStops As List(Of NGL.Interfaces.clsFMStopData))

        Dim intBookControl As Integer = 0
        Dim IsOrigin As Boolean
        For Each oStop As NGL.Interfaces.clsFMStopData In oIFMStops
            intBookControl = oStop.BookControl
            IsOrigin = oStop.LocationisOrigin
            'find the matching record
            Dim FMStop As clsFMStopData = (From d In StopData Where d.BookControl = intBookControl And d.LocationisOrigin = IsOrigin Select d).First()
            If Not FMStop Is Nothing Then
                With FMStop
                    .AddressValid = oStop.AddressValid
                    .LegCost = oStop.LegCost
                    .LegESTCHG = oStop.LegESTCHG
                    .LegMiles = oStop.LegMiles
                    .LegTime = oStop.LegTime
                    .LegTolls = oStop.LegTolls
                    .LogBadAddress = oStop.LogBadAddress
                    .Matched = oStop.Matched
                    .PCMilerCity = oStop.PCMilerCity
                    .PCMilerState = oStop.PCMilerState
                    .PCMilerStreet = oStop.PCMilerStreet
                    .PCMilerZip = oStop.PCMilerZip
                    .SeqNumber = oStop.SeqNumber
                    .StopName = oStop.StopName
                    .StopNumber = oStop.StopNumber
                    .TotalCost = oStop.TotalCost
                    .TotalESTCHG = oStop.TotalESTCHG
                    .TotalMiles = oStop.TotalMiles
                    .TotalTime = oStop.TotalTime
                    .TotalTolls = oStop.TotalTolls
                    .Warning = oStop.Warning
                End With
            End If
        Next
    End Sub

    Private Sub UpdateFMStopList(ByRef StopData As List(Of clsFMStopData), ByVal arrFMStops As NGL.FreightMaster.PCMiler.PCM.clsFMStopData())

        Dim intBookControl As Integer = 0
        Dim IsOrigin As Boolean
        For Each oStop As NGL.FreightMaster.PCMiler.PCM.clsFMStopData In arrFMStops
            intBookControl = oStop.BookControl
            IsOrigin = oStop.LocationisOrigin
            'find the matching record
            Dim FMStop As clsFMStopData = (From d In StopData Where d.BookControl = intBookControl And d.LocationisOrigin = IsOrigin Select d).First()
            If Not FMStop Is Nothing Then
                With FMStop
                    .AddressValid = oStop.AddressValid
                    .LegCost = oStop.LegCost
                    .LegESTCHG = oStop.LegESTCHG
                    .LegMiles = oStop.LegMiles
                    .LegTime = oStop.LegTime
                    .LegTolls = oStop.LegTolls
                    .LogBadAddress = oStop.LogBadAddress
                    .Matched = oStop.Matched
                    .PCMilerCity = oStop.PCMilerCity
                    .PCMilerState = oStop.PCMilerState
                    .PCMilerStreet = oStop.PCMilerStreet
                    .PCMilerZip = oStop.PCMilerZip
                    .SeqNumber = oStop.SeqNumber
                    .StopName = oStop.StopName
                    .StopNumber = oStop.StopNumber
                    .TotalCost = oStop.TotalCost
                    .TotalESTCHG = oStop.TotalESTCHG
                    .TotalMiles = oStop.TotalMiles
                    .TotalTime = oStop.TotalTime
                    .TotalTolls = oStop.TotalTolls
                    .Warning = oStop.Warning
                End With
            End If
        Next
    End Sub

    Private Sub UpdateFMReportRecords(ByRef ReportRecords As List(Of clsPCMReportRecord), ByVal oPCMReportRecords As List(Of NGL.Interfaces.clsPCMReportRecord))


        ReportRecords = (From d In oPCMReportRecords Select New clsPCMReportRecord With {.StopNumber = d.StopNumber _
                                                                                        , .SeqNumber = d.SeqNumber _
                                                                                        , .LegMiles = d.LegMiles _
                                                                                        , .TotalMiles = d.TotalMiles _
                                                                                        , .LegCost = d.LegCost _
                                                                                        , .TotalCost = d.TotalCost _
                                                                                        , .LegTime = d.LegTime _
                                                                                        , .TotalTime = d.TotalTime _
                                                                                        , .LegTolls = d.LegTolls _
                                                                                        , .TotalTolls = d.TotalTolls _
                                                                                        , .LegESTCHG = d.LegESTCHG _
                                                                                        , .TotalESTCHG = d.TotalESTCHG _
                                                                                        , .Zip = d.Zip _
                                                                                        , .City = d.City _
                                                                                        , .State = d.State _
                                                                                        , .Street = d.Street _
                                                                                        , .StopName = d.StopName}).ToList

    End Sub

    Private Sub UpdateFMReportRecords(ByRef ReportRecords As List(Of clsPCMReportRecord), ByVal oPCMReportRecords As List(Of NGL.FreightMaster.PCMiler.PCM.clsPCMReportRecord))


        ReportRecords = (From d In oPCMReportRecords Select New clsPCMReportRecord With {.StopNumber = d.StopNumber _
                                                                                        , .SeqNumber = d.SeqNumber _
                                                                                        , .LegMiles = d.LegMiles _
                                                                                        , .TotalMiles = d.TotalMiles _
                                                                                        , .LegCost = d.LegCost _
                                                                                        , .TotalCost = d.TotalCost _
                                                                                        , .LegTime = d.LegTime _
                                                                                        , .TotalTime = d.TotalTime _
                                                                                        , .LegTolls = d.LegTolls _
                                                                                        , .TotalTolls = d.TotalTolls _
                                                                                        , .LegESTCHG = d.LegESTCHG _
                                                                                        , .TotalESTCHG = d.TotalESTCHG _
                                                                                        , .Zip = d.Zip _
                                                                                        , .City = d.City _
                                                                                        , .State = d.State _
                                                                                        , .Street = d.Street _
                                                                                        , .StopName = d.StopName}).ToList

    End Sub
#End Region

End Class
