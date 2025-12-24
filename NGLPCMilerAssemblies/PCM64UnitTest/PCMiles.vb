Imports System.EnterpriseServices
Imports System.Runtime.InteropServices
Imports System.Reflection
Imports System.Text
'Imports PCMDLLTONET.PCM
Imports PCMDLLTONET64.PCM
Imports PCMINT = PCMDLLINT64.PCM
'Imports PCMINT = PCMDLLINT.PCM
Imports NGL.Service.PCMiler64

Namespace PCM64UT


    Public Class PCMiles
#Region "Overrides"
        Private _ConnectionString As String

        Public gServerID As Short = 0
        Public gLastError As String = ""
        Public gProcessRunning As Boolean = False


#End Region

#Region "CONSTANTS"
        ' Routing calculation types
        Private Const CALC_PRACTICAL As Short = 0
        Private Const CALC_SHORTEST As Short = 1
        Private Const CALC_NATIONAL As Short = 2
        Private Const CALC_AVOIDTOLL As Short = 3
        Private Const CALC_AIR As Short = 4
        Private Const CALC_53FOOT As Short = 6

        ' Report types
        Private Const RPT_DETAIL As Short = 0
        Private Const RPT_STATE As Short = 1
        Private Const RPT_MILEAGE As Short = 2

        'Distance types
        Private Const DIST_TYPE_MILES As Short = 0
        Private Const DIST_TYPE_KILO As Short = 1
#End Region

#Region "Properties"


        Private _blnDebug As Boolean = False
        Public Property Debug() As Boolean
            Get
                Debug = _blnDebug
            End Get
            Set(ByVal Value As Boolean)
                _blnDebug = Value
            End Set
        End Property

        Private _strLastError As String = ""
        Public ReadOnly Property LastError() As String
            Get
                Return _strLastError & gLastError

            End Get
        End Property

        Private _blnLoggingOn As Boolean = False
        Public Property LoggingOn() As Boolean
            Get
                Return _blnLoggingOn
            End Get
            Set(ByVal value As Boolean)
                _blnLoggingOn = value
            End Set
        End Property


        Private _IOLogStream As System.IO.StreamWriter
        Private _oLog As clsLog
        Public Property oLog() As clsLog
            Get
                Return _oLog
            End Get
            Set(ByVal value As clsLog)
                _oLog = value
            End Set
        End Property

        Private _strLogFileName As String = "C:\NGL-Service-PCMiler-Log.txt"
        Public Property LogFileName() As String
            Get
                Return _strLogFileName
            End Get
            Set(ByVal value As String)
                _strLogFileName = value
            End Set
        End Property

        Private _intKeepLogDays As Integer = 7
        Public Property KeepLogDays() As Integer
            Get
                Return _intKeepLogDays
            End Get
            Set(ByVal value As Integer)
                _intKeepLogDays = value
            End Set
        End Property

        Private _blnSaveOldLog As Boolean = False
        Public Property SaveOldLog() As Boolean
            Get
                Return _blnSaveOldLog
            End Get
            Set(ByVal value As Boolean)
                _blnSaveOldLog = value
            End Set
        End Property

        Private _blnUseZipOnly As Boolean = False
        Public Property UseZipOnly() As Boolean
            Get
                Return _blnUseZipOnly
            End Get
            Set(ByVal value As Boolean)
                _blnUseZipOnly = value
            End Set
        End Property
        Private _dtObjectCreateDate

        Private _PCMBuffers() As String
        Public Property PCMBuffers() As String()
            Get
                Return _PCMBuffers
            End Get
            Set(ByVal value As String())
                _PCMBuffers = value
            End Set
        End Property

#End Region

#Region "Constructors"

        Public Sub New()
            MyBase.New()
            _dtObjectCreateDate = Now
        End Sub


        'Private Sub New(ByVal KeepAlive As Integer)
        '    Me._KeepAlive = KeepAlive
        '    If Me._KeepAlive > 0 Then
        '        AddHandler _PCMTimer.Elapsed, AddressOf OnPCMTimerEvent
        '        ' Set the Interval to 5 seconds.
        '        _PCMTimer.Interval = Me._KeepAlive
        '        _PCMTimer.Enabled = True
        '    End If
        'End Sub

        Protected Overrides Sub finalize()
            PCMilerEnd()
            closeLog()
            MyBase.Finalize()
        End Sub
#End Region

#Region "Methods"


        Public Sub AddBaddAddressToArray(ByVal BookControl As Integer,
                            ByVal LaneControl As Integer,
                            ByVal objOrig As clsAddress,
                            ByVal objDest As clsAddress,
                            ByVal objPCMOrig As clsAddress,
                            ByVal objPCMDest As clsAddress,
                            ByVal Message As String,
                            ByVal BatchID As Double,
                            ByRef arrBaddAddresses() As clsPCMBadAddress)




            Dim oBadAddress As New clsPCMBadAddress
            With oBadAddress
                .BookControl = BookControl
                .LaneControl = LaneControl
                .objOrig = objOrig
                .objDest = objDest
                .objPCMOrig = objPCMOrig
                .objPCMDest = objPCMDest
                .Message = Message
                .BatchID = BatchID
            End With
            If arrBaddAddresses Is Nothing Then
                ReDim Preserve arrBaddAddresses(0)
            Else
                ReDim Preserve arrBaddAddresses(arrBaddAddresses.Length)
            End If
            arrBaddAddresses(arrBaddAddresses.Length - 1) = oBadAddress
        End Sub

        Public Sub AddStopToArray(ByVal shtStopNumber As Short,
                    ByVal strStopName As String,
                    ByVal strID1 As String,
                    ByVal strID2 As String,
                    ByVal strTruckName As String,
                    ByVal intTruckNumber As Integer,
                    ByVal shtSeqNbr As Short,
                    ByVal dblDistToPrev As Double,
                    ByVal dblTotalRouteCost As Double,
                    ByVal strConsNumber As String,
                    ByRef arrAllStops() As clsAllStop)
            'create a new object
            Dim oStop As New clsAllStop
            With oStop
                .StopNumber = shtStopNumber
                .Stopname = strStopName
                .ID1 = strID1
                .ID2 = strID2
                .TruckDesignator = strTruckName
                .TruckNumber = intTruckNumber
                .SeqNbr = shtSeqNbr
                .DistToPrev = dblDistToPrev
                .TotalRouteCost = dblTotalRouteCost
                .ConsNumber = strConsNumber
            End With
            If arrAllStops Is Nothing Then
                ReDim Preserve arrAllStops(0)
            Else
                ReDim Preserve arrAllStops(arrAllStops.Length)
            End If
            arrAllStops(arrAllStops.Length - 1) = oStop
        End Sub

        Public Sub AddPCMDataStopToArray(ByVal BookControl As Integer,
                                ByVal BookCustCompControl As Integer,
                                ByVal BookLoadControl As Integer,
                                ByVal BookODControl As Integer,
                                ByVal BookStopNo As Integer,
                                ByVal RouteType As Integer,
                                ByVal DistType As Integer,
                                ByVal BookOrigZip As String,
                                ByVal BookDestZip As String,
                                ByVal BookOrigAddress1 As String,
                                ByVal BookDestAddress1 As String,
                                ByVal BookOrigCity As String,
                                ByVal BookDestCity As String,
                                ByVal BookOrigState As String,
                                ByVal BookDestState As String,
                                ByVal BookProNumber As String,
                                ByVal LaneOriginAddressUse As Boolean,
                                ByRef arrPCMDataStops() As clsPCMDataStop)
            'create a new object
            Dim oPCMDataStop As New clsPCMDataStop
            With oPCMDataStop
                .BookControl = BookControl
                .BookCustCompControl = BookCustCompControl
                .BookLoadControl = BookLoadControl
                .BookODControl = BookODControl
                .BookStopNo = BookStopNo
                .RouteType = RouteType
                .DistType = DistType
                .BookOrigZip = BookOrigZip
                .BookDestZip = BookDestZip
                .BookOrigAddress1 = BookOrigAddress1
                .BookDestAddress1 = BookDestAddress1
                .BookOrigCity = BookOrigCity
                .BookDestCity = BookDestCity
                .BookOrigState = BookOrigState
                .BookDestState = BookDestState
                .BookProNumber = BookProNumber
                .LaneOriginAddressUse = LaneOriginAddressUse
            End With
            If arrPCMDataStops Is Nothing Then
                ReDim Preserve arrPCMDataStops(0)
            Else
                ReDim Preserve arrPCMDataStops(arrPCMDataStops.Length)
            End If
            arrPCMDataStops(arrPCMDataStops.Length - 1) = oPCMDataStop

        End Sub


        <AutoComplete()>
        Public Function About() As String
            Dim strAbout As String
            strAbout = "Component Title: " &
                System.Reflection.Assembly.GetExecutingAssembly.GetName.Name _
                & ", Version: " _
                & CStr(System.Diagnostics.FileVersionInfo.GetVersionInfo(
                    System.Reflection.Assembly.GetExecutingAssembly.Location).FileVersion) _
                & ", Module Name: " & System.Diagnostics.FileVersionInfo.GetVersionInfo(
                    System.Reflection.Assembly.GetExecutingAssembly.Location).InternalName
            'Log(strAbout)
            Return strAbout
        End Function

        Public Sub openLog()
            If Not String.IsNullOrEmpty(LogFileName) Then
                _oLog = New clsLog
                _oLog.Debug = _blnDebug
                _IOLogStream = _oLog.Open(LogFileName, KeepLogDays, SaveOldLog)
            End If
        End Sub

        Public Sub closeLog(Optional ByVal intReturn As Integer = 0)

            If Not Me.LoggingOn Then Return
            Try
                If IsNothing(_oLog) Then
                    Return
                End If
                'log the number of seconds the app was running
                Dim tsSpan As TimeSpan = Now.Subtract(Me._dtObjectCreateDate)
                Dim strSeconds As String = tsSpan.Seconds.ToString
                Log("NGL.Server.PCMiler was running for " & strSeconds & " seconds.")
                If intReturn <> 0 Then
                    _oLog.closeLog(intReturn, _IOLogStream)
                Else
                    _oLog.closeLog(_IOLogStream)
                End If

            Catch ex As Exception
                'ignore any errors when closing the log file
            End Try

        End Sub

        Public Sub Log(ByVal logMessage As String)
            If Not Me.LoggingOn Then
                'If Me.Debug Then Console.WriteLine("Logging is off.  The log message is: " & logMessage)
                Return
            End If

            'Write to log file
            Try
                If IsNothing(_oLog) Then openLog()
                _oLog.Write(logMessage, _IOLogStream)
            Catch ex As Exception
                ''ignore any errors while writing to the log
                'If Me.Debug Then
                '    Console.WriteLine("Save Log to file failure (ignored at run time): {0}", ex.ToString)
                '    Console.WriteLine("The log message is: {0}", logMessage)
                'End If
                'Throw

            End Try
        End Sub

        'Sub setKeepAlive(ByVal KeepAlive As Integer) Implements setKeepAlive
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
        '            gLastError = ex.Message & "Cannot execute PCMSCloseServer."
        '        Finally
        '            gServerID = 0
        '        End Try
        '    End If
        'End Sub

        Private Sub LogError(ByVal strMsg As String, Optional ByVal e As Exception = Nothing)
            Dim strErr As String = ""
            If Not IsNothing(e) Then
                If Me.Debug Then
                    Me._strLastError = strMsg & " " & e.ToString
                Else
                    Me._strLastError = strMsg & " " & e.Message
                End If
                strMsg &= " " & e.ToString
            Else
                Me._strLastError = strMsg
            End If
            If Me.LoggingOn Then
                Log(strMsg)
            End If
        End Sub

        Private Function CityName(ByVal zipcode As String) As String
            Dim buffer As String = ""
            Dim Zip As String = ""
            Try
                buffer = FirstMatch(zipcode)
            Catch ex As Exception
                LogError("Cannot execute CityName. ", ex)
            End Try
            Return buffer
        End Function

        Public Function CityToLatLong(ByRef cityZip As String) As String
            Dim buffer As StringBuilder = New StringBuilder(100)
            Dim Ret As Short = 0
            Dim strRet As String = ""
            _strLastError = ""
            Try
                If Not gServerID > 0 Then
                    If Not PCMilerStart() > 0 Then
                        Return strRet
                    End If
                End If


                Ret = PCMSCityToLatLong(gServerID, cityZip, buffer, 100)

                ' Check for errors
                If Ret = -1 Then
                    LogError("Cannot execute CityToLatLong the address," & cityZip & ", is not valid")
                Else
                    strRet = buffer.ToString
                End If

            Catch ex As Exception
                LogError("Cannot execute CityToLatLong. ", ex)
            End Try
            Return strRet

        End Function

        <AutoComplete()>
        Public Function cityStateZipLookup(ByVal zip As String) As clsAddress()
            Dim tripID As Integer = 0
            Dim numMatches As Short = 0
            Dim addrbuffer As StringBuilder = New StringBuilder(100)
            Dim citybuffer As StringBuilder = New StringBuilder(100)
            Dim statebuffer As StringBuilder = New StringBuilder(3)
            Dim zipbuffer As StringBuilder = New StringBuilder(15)
            Dim countybuffer As StringBuilder = New StringBuilder(100)
            Dim addrint As Integer = 100
            Dim cityint As Integer = 100
            Dim stateint As Integer = 3
            Dim zipint As Integer = 15
            Dim countyint As Integer = 100
            Dim Ret As Integer = 0
            Dim strRet As String = ""
            _strLastError = ""
            Dim result As New List(Of clsAddress)
            Try
                If Not gServerID > 0 Then
                    If Not PCMilerStart() > 0 Then
                        Return Nothing
                    End If
                End If

                ' Start up a trip to use for matching
                tripID = PCMSNewTrip(gServerID)
                If 0 = tripID Then Return Nothing
                ' Lookup and get the first match
                numMatches = PCMSLookup(tripID, zip, 0)
                If numMatches >= 1 Then
                    For i As Integer = 0 To numMatches - 1
                        Ret = PCMSGetFmtMatch2(tripID, i, addrbuffer, addrint, citybuffer, cityint, statebuffer, stateint, zipbuffer, zipint, countybuffer, countyint)
                        If Ret >= 1 Then
                            Dim addr As New clsAddress()
                            If addrbuffer IsNot Nothing AndAlso addrbuffer.Length > 0 Then
                                addr.strAddress = addrbuffer.ToString
                            End If
                            If citybuffer IsNot Nothing AndAlso citybuffer.Length > 0 Then
                                addr.strCity = citybuffer.ToString
                            End If
                            If statebuffer IsNot Nothing AndAlso statebuffer.Length > 0 Then
                                addr.strState = statebuffer.ToString
                            End If
                            If zipbuffer IsNot Nothing AndAlso zipbuffer.Length > 0 Then
                                addr.strZip = zipbuffer.ToString
                            End If
                            result.Add(addr)
                        End If
                    Next
                End If

                ' Free up the trip
                If 0 <> tripID Then PCMSDeleteTrip(tripID)

            Catch ex As Exception
                LogError("Cannot execute FirstMatch. ", ex)
            End Try
            If Not result Is Nothing AndAlso result.Count > 0 Then
                Return result.ToArray()
            Else
                Return Nothing
            End If
        End Function



        Private Function FirstMatch(ByVal CityZip As String) As String
            Dim tripID As Integer = 0
            Dim numMatches As Short = 0
            Dim buffer As StringBuilder = New StringBuilder(100)
            Dim Ret As Short = 0
            Dim strRet As String = ""
            _strLastError = ""
            Try
                If Not gServerID > 0 Then
                    If Not PCMilerStart() > 0 Then
                        Return strRet
                    End If
                End If

                ' Start up a trip to use for matching
                tripID = PCMSNewTrip(gServerID)
                If 0 = tripID Then Return ""
                ' Lookup and get the first match
                numMatches = PCMSLookup(tripID, CityZip, 1)
                If numMatches >= 1 Then
                    Ret = PCMSGetMatch(tripID, 0, buffer, 100)
                    strRet = buffer.ToString
                End If

                ' Free up the trip
                If 0 <> tripID Then PCMSDeleteTrip(tripID)

            Catch ex As Exception
                LogError("Cannot execute FirstMatch. ", ex)
            End Try
            Return strRet
        End Function

        <AutoComplete()>
        Public Function FullName(ByVal CityNameOrZipCode As String) As String

            Return FirstMatch(CityNameOrZipCode)

        End Function

        <AutoComplete()>
        Public Function getGeoCode(ByVal location As String, ByRef dblLat As Double, ByRef dblLong As Double) As Boolean
            Dim blnRet As Boolean = False
            Dim strRet As String = ""
            _strLastError = ""
            Try
                gProcessRunning = True

                If Not gServerID > 0 Then
                    If Not PCMilerStart() > 0 Then
                        Return False
                    End If
                End If

                strRet = CityToLatLong(location)
                If Trim(strRet) > "" Then
                    If strRet.Trim.Length > 16 Then
                        dblLat = convertLatLongToDec(Left(strRet, 8))
                        dblLong = convertLatLongToDec(Mid(strRet, 10, 8))
                        blnRet = True
                    Else
                        _strLastError = "Cannot get lat long from location data: " & strRet.Trim
                        blnRet = False
                    End If
                Else
                    blnRet = False
                End If


            Catch ex As System.AccessViolationException
                LogError("Cannot execute getGeoCode: PC Miler is no longer running.", ex)
            Catch ex As Exception
                LogError("Cannot execute getGeoCode. ", ex)
            Finally
                gProcessRunning = False
            End Try

            Return blnRet


        End Function

        <AutoComplete()>
        Public Function LatLongToCity(ByVal latlong As String) As String
            Dim buffer As StringBuilder = New StringBuilder(100)
            Dim Ret As Short = 0
            Dim strRet As String = "-1"
            _strLastError = ""
            Try
                If Not gServerID > 0 Then
                    If Not PCMilerStart() > 0 Then
                        Return strRet
                    End If
                End If

                Ret = PCMSLatLongToCity(gServerID, latlong, buffer, 100)
                ' Check for errors
                If -1 <> Ret Then strRet = buffer.ToString

            Catch ex As Exception
                LogError("Cannot execute LatLongToCity. ", ex)
            End Try
            Return strRet
        End Function

        ''' <summary>
        ''' Returns Stop Data with calculated miles according to the PCM parameter settings.  
        ''' Practical Miles is only the default
        ''' </summary>
        ''' <param name="objOrig"></param>
        ''' <param name="objDest"></param>
        ''' <param name="Route_Type"></param>
        ''' <param name="Dist_Type"></param>
        ''' <param name="intCompControl"></param>
        ''' <param name="intBookControl"></param>
        ''' <param name="intLaneControl"></param>
        ''' <param name="strItemNumber"></param>
        ''' <param name="strItemType"></param>
        ''' <param name="dblAutoCorrectBadLaneZipCodes"></param>
        ''' <param name="dblBatchID"></param>
        ''' <param name="blnBatch"></param>
        ''' <param name="arrBaddAddresses"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' Modified by RHR v-7.0.5.100 05/25/2016 
        '''   added logic to check trip id and retry 3 times if possible.
        ''' Modified by RHR for v-7.0.6.101 on 02/09/2017
        '''   we now call the overload getPracticalMilesEx
        ''' </remarks>
        <AutoComplete()>
        Function getPracticalMiles(ByVal objOrig As clsAddress,
                                ByVal objDest As clsAddress,
                                ByVal Route_Type As INGL_Service_PCMiler.PCMEX_Route_Type,
                                ByVal Dist_Type As INGL_Service_PCMiler.PCMEX_Dist_Type,
                                ByVal intCompControl As Integer,
                                ByVal intBookControl As Integer,
                                ByVal intLaneControl As Integer,
                                ByVal strItemNumber As String,
                                ByVal strItemType As String,
                                ByVal dblAutoCorrectBadLaneZipCodes As Double,
                                ByVal dblBatchID As Double,
                                ByVal blnBatch As Boolean,
                                ByRef arrBaddAddresses() As clsPCMBadAddress) As clsGlobalStopData
            Return getPracticalMilesEx(
                objOrig,
                objDest,
                Route_Type,
                Dist_Type,
                INGL_Service_PCMiler.PCMEX_CALCTYPE.CALCTYPE_NONE,
                INGL_Service_PCMiler.PCMEX_Opt_Flags.OPT_NONE,
                INGL_Service_PCMiler.PCMEX_Veh_Type.CALCEX_VEH_TRUCK,
                intCompControl,
                intBookControl,
                intLaneControl,
                strItemNumber,
                strItemType,
                dblAutoCorrectBadLaneZipCodes,
                dblBatchID,
                blnBatch,
                arrBaddAddresses)
        End Function

        ''' <summary>
        ''' Returns Stop Data with calculated miles according to the PCM parameter settings.  
        ''' Practical Miles is only the default.  Provides support for PCMSSetCalcTypeEx for use 
        ''' with extended calculation options like shortest 53Foot routing
        ''' </summary>
        ''' <param name="objOrig"></param>
        ''' <param name="objDest"></param>
        ''' <param name="Route_Type"></param>
        ''' <param name="Dist_Type"></param>
        ''' <param name="EXCALC_Type"></param>
        ''' <param name="EXOpt_Flags"></param>
        ''' <param name="EXVeh_Type"></param>
        ''' <param name="intCompControl"></param>
        ''' <param name="intBookControl"></param>
        ''' <param name="intLaneControl"></param>
        ''' <param name="strItemNumber"></param>
        ''' <param name="strItemType"></param>
        ''' <param name="dblAutoCorrectBadLaneZipCodes"></param>
        ''' <param name="dblBatchID"></param>
        ''' <param name="blnBatch"></param>
        ''' <param name="arrBaddAddresses"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-7.0.6.101 on 2/9/2017 
        '''   provides support for PCMSSetCalcTypeEx for use 
        '''   with extended calculation options like shortest -- 53Foot
        ''' </remarks>
        <AutoComplete()>
        Function getPracticalMilesEx(ByVal objOrig As clsAddress,
                                ByVal objDest As clsAddress,
                                ByVal Route_Type As INGL_Service_PCMiler.PCMEX_Route_Type,
                                ByVal Dist_Type As INGL_Service_PCMiler.PCMEX_Dist_Type,
                                ByVal EXCALC_Type As INGL_Service_PCMiler.PCMEX_CALCTYPE,
                                ByVal EXOpt_Flags As INGL_Service_PCMiler.PCMEX_Opt_Flags,
                                ByVal EXVeh_Type As INGL_Service_PCMiler.PCMEX_Veh_Type,
                                ByVal intCompControl As Integer,
                                ByVal intBookControl As Integer,
                                ByVal intLaneControl As Integer,
                                ByVal strItemNumber As String,
                                ByVal strItemType As String,
                                ByVal dblAutoCorrectBadLaneZipCodes As Double,
                                ByVal dblBatchID As Double,
                                ByVal blnBatch As Boolean,
                                ByRef arrBaddAddresses() As clsPCMBadAddress) As clsGlobalStopData

            Dim Ret As Integer = 0
            Dim mi As Integer = 0
            Dim intTrip1 As Integer = 0
            Dim intDash As Integer = 0
            Dim strOrigStopName As String = ""
            Dim strDestStopName As String = ""
            Dim dblTotalMiles As Double = 0
            'Dim Ret As Long
            'Dim objAllStops As clsGlobalStopData
            Dim objGlobalStopData As New clsGlobalStopData
            Dim objPCMOrig As New clsAddress
            Dim objPCMDest As New clsAddress
            Dim blnAddressValid As Boolean = True
            _strLastError = ""
            Try
                gProcessRunning = True
                objGlobalStopData.BadAddressCount = 0
                objGlobalStopData.FailedAddressMessage = ""
                objGlobalStopData.BatchID = dblBatchID
                blnAddressValid = True
                If Not gServerID > 0 Then
                    If Not PCMilerStart() > 0 Then
                        Return Nothing
                    End If
                End If

                intTrip1 = PCMSNewTrip(gServerID)
                'Modified by RHR v-7.0.5.100 05/25/2016
                ' added logic to check trip id and retry 
                ' 3 times if possible.
                If intTrip1 < 1 Then
                    For intRetry As Integer = 1 To 3
                        PCMilerEnd()
                        If Not PCMilerStart() Then Return Nothing
                        intTrip1 = PCMSNewTrip(gServerID)
                        If intTrip1 > 0 Then Exit For
                    Next
                End If
                'Modified by RHR v-7.0.5.100 05/25/2016
                '  We now throw an invalid operation exception if the trip id is not valid
                If intTrip1 < 1 Then Throw New InvalidOperationException("Invalid Trip ID: Cannot connect to PC Miler.  Please wait and try again.")

                If EXCALC_Type <> INGL_Service_PCMiler.PCMEX_CALCTYPE.CALCTYPE_NONE Then
                    Call PCMSSetCalcTypeEx(intTrip1, EXCALC_Type, EXOpt_Flags, EXVeh_Type)
                Else
                    Call PCMSSetCalcType(intTrip1, Route_Type)
                End If


                Call PCMSSetMiles(intTrip1)
                If Dist_Type = INGL_Service_PCMiler.PCMEX_Dist_Type.DIST_TYPE_KILO Then
                    Call PCMSSetKilometers(intTrip1)
                End If
                PCMSClearStops(intTrip1)
                objPCMOrig = New clsAddress
                objPCMDest = New clsAddress
                objGlobalStopData.AutoCorrectBadLaneZipCodes = dblAutoCorrectBadLaneZipCodes
                Me._strLastError = ""
                AddStop(objOrig,
                    objDest,
                    objPCMOrig,
                    objPCMDest,
                    True,
                    objGlobalStopData,
                    blnAddressValid,
                    strOrigStopName,
                    strDestStopName,
                    intTrip1,
                    strItemNumber,
                    strItemType,
                    intBookControl,
                    intLaneControl,
                    arrBaddAddresses)
                If Not String.IsNullOrEmpty(Me._strLastError) Then
                    LogError("Cannot execute getPracticalMiles. " & Me._strLastError)
                    objGlobalStopData = Nothing
                Else
                    If blnAddressValid Then
                        Ret = PCMSOptimize(intTrip1)
                        mi = PCMSCalculate(intTrip1)
                        ' Check for errors before converting tenths to miles
                        If -1 <> mi Then dblTotalMiles = mi / 10.0!
                        objGlobalStopData.TotalMiles = dblTotalMiles
                    End If
                End If

            Catch ex As System.AccessViolationException
                LogError("Cannot get miles for " & strOrigStopName & " To " & strDestStopName & ": PC Miler is no longer running.", ex)
                objGlobalStopData = Nothing
            Catch ex As Exception
                LogError("Cannot get miles for " & strOrigStopName & " To " & strDestStopName & ": ", ex)
                objGlobalStopData = Nothing
            Finally
                If intTrip1 > 0 Then
                    Try
                        PCMSDeleteTrip(intTrip1)
                    Catch ex As Exception

                    End Try
                End If
                gProcessRunning = False
                'If Not blnBatch Then PCMilerEnd()
            End Try
            Return objGlobalStopData

        End Function

        <AutoComplete()>
        Public Function Miles(ByVal Origin As String, ByVal Destination As String) As Single
            Dim mi As Integer = 0
            Dim oo As String = ""
            Dim dd As String = ""
            Dim sglRet As Single = -1.0!
            _strLastError = ""
            Try
                If Not gServerID > 0 Then
                    If Not PCMilerStart() > 0 Then
                        Return sglRet
                    End If
                End If

                mi = PCMSCalcDistance(gServerID, Origin, Destination)
                ' Check for errors before converting tenths to miles
                If mi > 0 Then
                    sglRet = mi / 10.0!
                End If


            Catch ex As System.AccessViolationException
                LogError("Cannot get miles for " & Origin & " To " & Destination & ": PC Miler is no longer running.", ex)
            Catch ex As Exception
                LogError("Cannot not calculate miles. Please check your results for errors.", ex)
            End Try
            Return sglRet

        End Function


        Public Function PCMilerEnd() As Short
            Dim retVal As Short = 0
            Try
                retVal = PCMSCloseServer(gServerID)
                'Console.WriteLine("PCMiler End Executed")
            Catch ex As Exception
                LogError("Cannot execute PCMilerEnd.", ex)
            Finally
                gServerID = 0
            End Try

            Return retVal

        End Function

        Public Function PCMilerStart() As Short
            Dim errBuff As StringBuilder = New StringBuilder(100)
            Dim Ret As Short = 0
            If Not gServerID > 0 Then
                Try
                    gServerID = PCMSOpenServer(0, 0)
                    If Not gServerID > 0 Then
                        Ret = PCMSGetErrorString(PCMSGetError(), errBuff, 100)
                        LogError("PCMiler could not start. Cannot execute PCMilerStart.  " & errBuff.ToString.Replace(Chr(0), "").Trim)
                    End If
                Catch ex As System.Runtime.InteropServices.SEHException
                    LogError("PCMiler could not start. The application is not available or may not be configured correctly.")
                Catch ex As Exception
                    LogError("PCMiler could not start. Cannot execute PCMilerStart.  ", ex)
                End Try
            End If

            Return gServerID

        End Function

        ' Return the currently open server's ID for use in other functions
        Public Function Server() As Short
            Server = gServerID
        End Function

        <AutoComplete()>
        Public Function zipcode(ByVal CityName As String) As String
            Dim strRet As String = ""
            strRet = FirstMatch(CityName)
            Return strRet.Substring(0, 5)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="arrStopData"></param>
        ''' <param name="strConsNumber"></param>
        ''' <param name="dblBatchID"></param>
        ''' <param name="blnKeepStopNumbers"></param>
        ''' <param name="arrAllStops"></param>
        ''' <param name="arrBaddAddresses"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' Modified by RHR v-7.0.5.100 05/25/2016 
        '''   added logic to check trip id and retry 3 times if possible.
        ''' </remarks>
        <AutoComplete()>
        Public Function PCMReSync(ByVal arrStopData() As clsPCMDataStop,
                            ByVal strConsNumber As String,
                            ByVal dblBatchID As Double,
                            ByVal blnKeepStopNumbers As Boolean,
                            ByRef arrAllStops() As clsAllStop,
                            ByRef arrBaddAddresses() As clsPCMBadAddress) As clsGlobalStopData
            Dim Ret As Integer = 0
            Dim i As Integer = 0
            Dim lines As Short = 0
            Dim intTrip1 As Integer = 0
            Dim buff As StringBuilder = New StringBuilder(255)
            Dim colPCMStops As Collection
            Dim objPCMStop As clsPCMStop
            Dim strZip As String = ""
            Dim strStreet As String = ""
            Dim strCity As String = ""
            Dim strState As String = ""
            Dim strStopName As String = ""
            Dim strCityState() As String
            Dim strOrigStopName As String = ""
            Dim strDestStopName As String = ""
            Dim blnInbound As Boolean = False
            Dim strDupZips() As String
            Dim strDupStreets() As String
            Dim colDupStops As Collection
            Dim shtDupStreetsCt As Short = 0
            Dim shtLoopCt As Short = 0
            Dim blnAddOrigin As Boolean = True
            Dim blnSkip As Boolean = False
            Dim shtPos As Short = 0
            Dim oGlobalStopData As New clsGlobalStopData
            Dim shtStopCT As Short = 0
            Dim blnMatchFound As Boolean = False
            Dim dblLegMiles As Double = 0
            Dim dblTotalMiles As Double = 0
            Dim dblLegCost As Double = 0
            Dim dblTotalCost As Double = 0
            Dim strLegTime As String = ""
            Dim strTotalTime As String = ""
            Dim strZipFound As String = ""
            Dim strStreetsFound As String = ""
            Dim shtFieldCt As Short = 0
            Dim blnAddToClass As Boolean = True
            Dim shtStopNbr As Short = 0
            Dim strMatchedStreet As String = ""
            Dim objOrig As New clsAddress
            Dim objDest As New clsAddress
            Dim objPCMOrig As New clsAddress
            Dim objPCMDest As New clsAddress
            Dim blnAddressValid As Boolean = True
            Dim Reseq_Type As Integer = 1
            'Dim EvtLog As New System.Diagnostics.EventLog
            'EvtLog.Log = "Application"
            'EvtLog.Source = "NGL.Service.PCMiler"
            'EvtLog.WriteEntry("PCMReSync Running", EventLogEntryType.Information)
            _strLastError = ""
            Try
                'Set global parameters
                gProcessRunning = True
                oGlobalStopData.BadAddressCount = 0
                oGlobalStopData.FailedAddressMessage = ""
                oGlobalStopData.BatchID = dblBatchID
                'Check if PC Miler is running; start if if needed
                If Not gServerID > 0 Then
                    If Not PCMilerStart() > 0 Then
                        Return Nothing
                    End If
                End If
                'Get a trip id
                intTrip1 = PCMSNewTrip(gServerID)
                'Modified by RHR v-7.0.5.100 05/25/2016
                ' added logic to check trip id and retry 
                ' 3 times if possible.
                If intTrip1 < 1 Then
                    For intRetry As Integer = 1 To 3
                        PCMilerEnd()
                        If Not PCMilerStart() Then Return Nothing
                        intTrip1 = PCMSNewTrip(gServerID)
                        If intTrip1 > 0 Then Exit For
                    Next
                End If
                'Modified by RHR v-7.0.5.100 05/25/2016
                '  We now throw an invalid operation exception if the trip id is not valid
                If intTrip1 < 1 Then Throw New InvalidOperationException("Invalid Trip ID: Cannot connect to PC Miler.  Please wait and try again.")

                'Create a new stops collection
                colPCMStops = New Collection
                If arrStopData.Length > 0 Then
                    'set up parameters
                    PCMSSetCalcType(intTrip1, arrStopData(0).RouteType)
                    PCMSSetMiles(intTrip1)
                    If arrStopData(0).DistType = 1 Then
                        'we are using Kilometers
                        PCMSSetKilometers(intTrip1)
                    End If
                    blnInbound = arrStopData(0).LaneOriginAddressUse
                End If
                'Process the stops
                For intStops As Integer = 0 To arrStopData.Length - 1
                    'loop through each stop data record
                    Dim oStop As clsPCMDataStop = arrStopData(intStops)
                    shtStopNbr = oStop.BookStopNo
                    'set up the address objects
                    objOrig = New clsAddress
                    objDest = New clsAddress
                    If oStop.LaneOriginAddressUse Then
                        objOrig.strZip = oStop.BookDestZip
                        objOrig.strAddress = oStop.BookDestAddress1
                        objOrig.strCity = oStop.BookDestCity
                        objOrig.strState = oStop.BookDestState
                        objDest.strZip = oStop.BookOrigZip
                        objDest.strAddress = oStop.BookOrigAddress1
                        objDest.strCity = oStop.BookOrigCity
                        objDest.strState = oStop.BookOrigState
                    Else
                        objOrig.strZip = oStop.BookOrigZip
                        objOrig.strAddress = oStop.BookOrigAddress1
                        objOrig.strCity = oStop.BookOrigCity
                        objOrig.strState = oStop.BookOrigState
                        objDest.strZip = oStop.BookDestZip
                        objDest.strAddress = oStop.BookDestAddress1
                        objDest.strCity = oStop.BookDestCity
                        objDest.strState = oStop.BookDestState
                    End If
                    'initialize the PC Miler Address Objects
                    objPCMOrig = New clsAddress
                    objPCMDest = New clsAddress
                    'Add the Stop
                    AddStop(objOrig,
                                    objDest,
                                    objPCMOrig,
                                    objPCMDest,
                                    blnAddOrigin,
                                    oGlobalStopData,
                                    blnAddressValid,
                                    strOrigStopName,
                                    strDestStopName,
                                    intTrip1,
                                    oStop.BookProNumber,
                                    "PRO Number",
                                    oStop.BookControl,
                                    0,
                                    arrBaddAddresses)
                    'If the address is valid then add the stop to the collection
                    If blnAddressValid Then
                        'add to collection
                        objPCMStop = New clsPCMStop
                        With objPCMStop
                            .BookLoadControl = oStop.BookLoadControl
                            .Zip = objDest.strZip
                            .City = objOrig.strCity
                            .State = objDest.strState
                            .Street = objDest.strAddress
                            If strDestStopName = objDest.strZip Then
                                'There was a problem with the address so clear the street (we are using zip code)
                                .PCMilerStreet = ""
                            Else
                                .PCMilerStreet = objPCMDest.strAddress
                            End If
                            .PCMilerCity = objPCMDest.strCity
                            .PCMilerState = objPCMDest.strState
                            If blnKeepStopNumbers Then
                                .StopNumber = shtStopNbr
                            End If
                            .LoopCt = shtLoopCt
                            .BookODControl = oStop.BookODControl
                        End With
                        'add the stop to the collection
                        colPCMStops.Add(objPCMStop, "k" & oStop.BookLoadControl.ToString)
                    End If

                Next
                'test for bad zips if any exist we cannot continue
                If Trim(oGlobalStopData.FailedAddressMessage) <> "" Then
                    _strLastError &= oGlobalStopData.FailedAddressMessage & vbCrLf & "The requested operation has been canceled!"
                    Return Nothing
                End If
                'If we are not using manual stop number call the PCM Resequence and Optimize routine
                If Not blnKeepStopNumbers Then
                    'resequence  and optimize the stops
                    PCMSSetResequence(intTrip1, Reseq_Type)
                    Ret = PCMSOptimize(intTrip1)
                End If
                Ret = PCMSCalculate(intTrip1)
                lines = PCMSNumRptLines(intTrip1, RPT_MILEAGE)
                shtStopCT = 0
                shtDupStreetsCt = 0
                ReDim strDupZips(0)
                ReDim strDupStreets(0)
                colDupStops = New Collection
                'we skip the first line because it is the origin.

                For i = 1 To lines - 1
                    shtStopCT = shtStopCT + 1
                    blnSkip = False
                    Ret = PCMSGetRptLine(intTrip1, RPT_MILEAGE, i, buff, 254)
                    shtPos = 1
                    Dim strings() As String
                    strings = Split(buff.ToString, Chr(9))
                    shtFieldCt = UBound(strings)

                    'Leg Miles
                    'Total Miles
                    'Leg Cost
                    'Total Cost
                    'Leg Hours
                    'Total Hours
                    'Leg Tolls
                    'Total Tolls
                    'Leg Est. GHG
                    'Total Est. CHG

                    If shtFieldCt = 7 Then
                        strTotalTime = strings(shtFieldCt)
                        strLegTime = strings(shtFieldCt - 1)
                        dblTotalCost = Val(strings(shtFieldCt - 2))
                        dblLegCost = Val(strings(shtFieldCt - 3))
                        dblTotalMiles = Val(strings(shtFieldCt - 4))
                        dblLegMiles = Val(strings(shtFieldCt - 5))
                        If InStr(1, strings(0), ",") Then
                            'the first field is a city state so we use the street address as a match
                            strCityState = Split(strings(0), ",")
                            strZip = ""
                            strStreet = Trim(simpleStreetScrubber(strings(1)))
                            strStopName = Trim(strings(0))
                        Else
                            'the first field is the zip code and the second is the city and state
                            strStopName = Trim(strings(1))
                            strCityState = Split(strings(1), ",")
                            strZip = Trim(strings(0))
                            strStreet = ""
                        End If
                        strCity = Trim(strCityState(0))
                        strState = Trim(strCityState(1))
                    ElseIf shtFieldCt = 6 Then
                        strTotalTime = strings(shtFieldCt)
                        strLegTime = strings(shtFieldCt - 1)
                        dblTotalCost = Val(strings(shtFieldCt - 2))
                        dblLegCost = Val(strings(shtFieldCt - 3))
                        dblTotalMiles = Val(strings(shtFieldCt - 4))
                        dblLegMiles = Val(strings(shtFieldCt - 5))
                        'only the zip code is provided
                        strZip = Trim(strings(0))
                        strStreet = ""
                        strCity = ""
                        strState = ""
                        strStopName = Trim(strings(0))
                    Else
                        If shtFieldCt > 7 Then
                            ' *** Modified by RHR v-5.1.3 for PC Miler 24

                            strTotalTime = strings(7)
                            strLegTime = strings(6)
                            dblTotalCost = Val(strings(5))
                            dblLegCost = Val(strings(4))
                            dblTotalMiles = Val(strings(3))
                            dblLegMiles = Val(strings(2))

                            ''********** OLD CODE ***************
                            ''we have all the data
                            'strZip = Trim(strings(0))
                            'strCityState = Split(strings(1), ",")
                            'strCity = Trim(strCityState(0))
                            'strState = Trim(strCityState(1))
                            'strStreet = Trim(simpleStreetScrubber(strings(2)))
                            'strStopName = Trim(strings(1))
                            ''**********************************
                            '************************ Modified v-4.7.3 by RHR to adapt to PC Miler 22 report format issues (missing zip code) *********************
                            If InStr(1, strings(0), ",") Then
                                'the first field is a city state so we use the street address as a match
                                strCityState = Split(strings(0), ",")
                                strZip = ""
                                strStreet = Trim(simpleStreetScrubber(strings(1)))
                                strStopName = Trim(strings(0))
                            Else
                                'the first field is the zip code and the second is the city and state
                                strStopName = Trim(strings(1))
                                strCityState = Split(strings(1), ",")
                                strZip = Trim(strings(0))
                                strStreet = ""
                            End If

                            'If InStr(1, strings(0), ",") Then
                            '    'the first field is a city state so we use the street address as a match
                            '    strCityState = Split(strings(0), ",")
                            '    strZip = ""
                            '    strStreet = Trim(simpleStreetScrubber(strings(1)))
                            '    strStopName = Trim(strings(0))
                            'Else
                            '    'we have all the data
                            '    strZip = Trim(strings(0))
                            '    strCityState = Split(strings(1), ",")
                            '    strStreet = Trim(simpleStreetScrubber(strings(2)))
                            '    strStopName = Trim(strings(1))
                            'End If
                            strCity = Trim(strCityState(0))
                            strState = Trim(strCityState(1))
                            '****************************************************************************************************************************************
                        Else
                            blnSkip = True
                        End If
                    End If

                    If blnSkip = False And strStreet > " " Then
                        For shtLoopCt = 0 To shtDupStreetsCt
                            If strStreet = strDupStreets(shtLoopCt) Then
                                blnSkip = True
                                Exit For
                            End If
                        Next
                    End If
                    If blnSkip Then
                        shtStopCT = shtStopCT - 1
                    Else
                        objPCMStop = New clsPCMStop
                        blnMatchFound = False
                        strMatchedStreet = ""
                        For Each objPCMStop In colPCMStops
                            blnAddToClass = False
                            With objPCMStop
                                If Not .Matched Then
                                    If strStreet > " " Then
                                        'check for a match on the street city and state
                                        If Trim(UCase(.PCMilerCity)) = Trim(UCase(strCity)) And Trim(UCase(.PCMilerState)) = Trim(UCase(strState)) And Trim(UCase(.PCMilerStreet)) = Trim(UCase(strStreet)) Then
                                            strZipFound = ""
                                            strStreetsFound = strStreet
                                            blnAddToClass = True
                                        End If
                                    Else
                                        'we need to use the zip code
                                        If Trim(.PCMilerStreet) = "" And Trim(UCase(.Zip)) = Trim(UCase(strZip)) Then
                                            strStreetsFound = ""
                                            If blnMatchFound Then
                                                'Test for Matched Street
                                                If Trim(UCase(strMatchedStreet)) = Trim(UCase(.Street)) Then
                                                    blnAddToClass = True
                                                End If
                                            Else
                                                blnAddToClass = True
                                                'loop through all of the duplicate zip codes to
                                                'see if this street address has already been used
                                                Dim objDupStop As New clsPCMStop
                                                For Each objDupStop In colDupStops
                                                    If Trim(UCase(objDupStop.Zip)) = Trim(UCase(strZip)) And Trim(UCase(objDupStop.Street)) = Trim(UCase(.Street)) Then
                                                        'the object exists so do not add it
                                                        blnAddToClass = False
                                                        Exit For
                                                    End If
                                                Next
                                            End If
                                            If blnAddToClass Then
                                                strZipFound = strZip
                                                strMatchedStreet = .Street
                                                colDupStops.Add(objPCMStop)
                                            End If
                                        End If
                                    End If
                                    If blnAddToClass Then
                                        If blnMatchFound Then
                                            .LegMiles = 0
                                            .LegCost = 0
                                            .LegTime = 0
                                        Else
                                            .LegMiles = dblLegMiles
                                            .LegCost = dblLegCost
                                            .LegTime = strLegTime
                                        End If
                                        .StopName = strStopName
                                        .TotalMiles = dblTotalMiles
                                        .TotalCost = dblTotalCost
                                        .TotalTime = strTotalTime
                                        If Not blnKeepStopNumbers Then
                                            .StopNumber = shtStopCT
                                        End If
                                        .Matched = True
                                        blnMatchFound = True
                                    End If
                                End If
                            End With
                        Next
                        If Not blnMatchFound Then shtStopCT = shtStopCT - 1
                        If strStreetsFound > " " Then
                            If shtDupStreetsCt = 0 And strDupStreets(0) = "" Then
                                strDupStreets(0) = strStreetsFound
                            Else
                                shtDupStreetsCt = shtDupStreetsCt + 1
                                ReDim Preserve strDupStreets(shtDupStreetsCt)
                                strDupStreets(shtDupStreetsCt) = strStreetsFound
                            End If
                        End If
                    End If
                Next i

                If blnInbound And Not blnKeepStopNumbers Then
                    'We need to reverse the stop numbers.
                    objPCMStop = New clsPCMStop
                    For Each objPCMStop In colPCMStops
                        objPCMStop.StopNumber = (shtStopCT - objPCMStop.StopNumber) + 1
                    Next
                End If
                'now add all values to the AllStops Collection
                For Each objPCMStop In colPCMStops
                    AddStopToArray(
                          objPCMStop.LoopCt,
                          objPCMStop.StopName,
                          objPCMStop.BookLoadControl.ToString,
                          objPCMStop.BookODControl.ToString,
                          "0",
                          1,
                          objPCMStop.StopNumber,
                          objPCMStop.LegMiles,
                          objPCMStop.TotalCost,
                          strConsNumber,
                          arrAllStops)
                Next

            Catch ex As System.AccessViolationException
                LogError("Cannot execute PCMResync: PC Miler is no longer running.", ex)
                oGlobalStopData = Nothing
            Catch ex As Exception
                LogError("Cannot execute PCMResync. ", ex)
                oGlobalStopData = Nothing
            Finally
                Try
                    PCMSDeleteTrip(intTrip1)
                Catch ex As Exception

                End Try
                gProcessRunning = False
                'Try
                '    PCMilerEnd()
                'Catch ex As Exception

                'End Try
            End Try
            Return oGlobalStopData

        End Function
        <AutoComplete()>
        Public Function getRouteMiles(ByRef sRoute As clsSimpleStop()) As clsPCMReturn
            Dim oPCMReturn As New clsPCMReturn
            Dim intTrip As Integer = 0
            Try
                gProcessRunning = True
                'Start PC Miler
                If Not gServerID > 0 Then
                    If Not PCMilerStart() > 0 Then
                        Return Nothing
                    End If
                End If
                intTrip = PCMSNewTrip(gServerID)
                Call PCMSSetMiles(intTrip)
                PCMSClearStops(intTrip)
                For i As Integer = 0 To sRoute.Length - 1
                    'Test if data has been added to the array @ i
                    If Not sRoute(i) Is Nothing Then
                        'Check if the address info is valid.
                        If PCMSCheckPlaceName(intTrip, sRoute(i).Address) > 0 Then
                            If PCMSAddStop(intTrip, sRoute(i).Address) < 1 Then
                                'The address cannot be found so return an error                    
                                oPCMReturn.RetVal = 0
                                oPCMReturn.Message = "Cannot get route miles! The address " & sRoute(i).Address & " cannot be added to the route or it does not exists in the PCMiler address database."
                                Exit For
                            Else
                                oPCMReturn.RetVal = i
                            End If
                        Else
                            oPCMReturn.RetVal = 0
                            oPCMReturn.Message = "Cannot get route miles! The address " & sRoute(i).Address & " does not exists in the PCMiler address database."
                            Exit For
                        End If
                    End If
                Next
                Dim intLegID As Integer = 0
                If oPCMReturn.RetVal > 0 Then
                    PCMSCalculate(intTrip)
                    'Now Get the Data Back
                    For i As Integer = 0 To sRoute.Length - 1
                        'Test if data has been added to the array @ i
                        If Not sRoute(i) Is Nothing AndAlso sRoute(i).StopNumber > 0 Then
                            'update the leg info
                            Dim sLegData As New PCMINT.legInfoType
                            PCMINT.PCMSGetLegInfo(intTrip, intLegID, sLegData)
                            With sRoute(i)
                                .LegMiles = sLegData.legMiles
                                .LegCost = sLegData.legCost
                                .LegHours = sLegData.legHours
                                .TotalMiles = sLegData.totMiles
                                .TotalCost = sLegData.totCost
                                .TotalHours = sLegData.totHours
                            End With
                            intLegID += 1
                        End If
                    Next
                End If
            Catch ex As System.AccessViolationException
                LogError("Cannot get Route Miles: PC Miler is no longer running.", ex)
                oPCMReturn.RetVal = 0
                oPCMReturn.Message = "Error while getting Route Miles: " & ex.Message & ". PC Miler is no longer running."
            Catch ex As Exception
                LogError("Cannot get Route Miles: ", ex)
                oPCMReturn.RetVal = 0
                oPCMReturn.Message = "Error while getting Route Miles: " & ex.Message & "."
            Finally
                If intTrip > 0 Then
                    Try
                        PCMSDeleteTrip(intTrip)
                    Catch ex As Exception

                    End Try
                End If
                gProcessRunning = False
            End Try

            Return oPCMReturn
        End Function


        Private Function convertLatLongToDec(ByVal strLatLong As String) As Double

            Dim strhemisphere As String = "W"
            Dim intdegrees As Integer = 0
            Dim dblminutes As Double = 0
            Dim dblseconds As Double = 0
            Dim intmulitplier As Short = 0
            Dim strDegrees As String = ""
            Dim strMinutes As String = ""
            Dim strSeconds As String = ""
            If strLatLong.Length > 2 Then strDegrees = Left(strLatLong, 3)
            If strLatLong.Length > 5 Then strMinutes = Mid(strLatLong, 4, 2)
            If strLatLong.Length > 7 Then strSeconds = Mid(strLatLong, 6, 2)
            If strLatLong.Length > 8 Then strhemisphere = Right(strLatLong, 1)
            Integer.TryParse(strDegrees, intdegrees)
            Double.TryParse(strMinutes, dblminutes)
            Double.TryParse(strSeconds, dblseconds)


            'intdegrees = CShort(Left(strLatLong, 3))
            'dblminutes = CDbl(Mid(strLatLong, 4, 2))
            'dblseconds = CDbl(Mid(strLatLong, 6, 2))
            strhemisphere = Right(strLatLong, 1)
            If dblminutes <> 0 Then dblminutes = dblminutes / 60.0#
            If dblseconds <> 0 Then dblseconds = dblseconds / 3600.0#

            If strhemisphere = "W" Or strhemisphere = "S" Then
                intmulitplier = -1
            Else
                intmulitplier = 1
            End If

            Return (intdegrees + dblminutes + dblseconds) * intmulitplier


        End Function



        '-----------------------------------------------------------
        'NOTE: PCMilerStart must be called by the calling function.
        '-----------------------------------------------------------
        ''' <summary>
        ''' Checks if the address is valid with PC Miler.  attemtpts to find an alternative if possible.
        ''' new procedures should use validaAddressEX instead of validateAddress. 
        ''' NOTE: PCMilerStart must be called by the calling function.
        ''' </summary>
        ''' <param name="intTrip1"></param>
        ''' <param name="strAddressType"></param>
        ''' <param name="strItemNumber"></param>
        ''' <param name="strItemType"></param>
        ''' <param name="objSource"></param>
        ''' <param name="objPCM"></param>
        ''' <param name="strStopName"></param>
        ''' <param name="strWarnings"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' Modified by RHR 7.0.5.100 05/19/2016
        '''   fixed issues with canadian postal code parsing using PC Miler buffer returned
        '''   fixed issue with parsing state or region when county is not provided
        ''' Modified by RHR v-7.0.5.100 05/25/2016
        '''   added logic to manage PCM error codes
        ''' </remarks>
        Private Function validateAddress(ByVal intTrip1 As Integer,
                                        ByVal strAddressType As String,
                                        ByVal strItemNumber As String,
                                        ByVal strItemType As String,
                                        ByRef objSource As clsAddress,
                                        ByRef objPCM As clsAddress,
                                        ByRef strStopName As String,
                                        ByRef strWarnings As String) As Boolean
            Dim blnMatchFound As Boolean = False
            Dim strPCMilerCityState() As String
            Dim buffer As StringBuilder = New StringBuilder(256)
            Dim buff As String = ""
            Dim strMatchZip As String = ""
            Dim strZip As String = ""
            Dim intDash As Integer = 0
            'set default value of function to false
            validateAddress = False
            intDash = InStr(1, objSource.strZip, "-")
            If intDash Then
                strZip = Left(objSource.strZip, intDash - 1)
            Else
                strZip = objSource.strZip
            End If
            strStopName = objSource.strZip
            If PCMSCheckPlaceName(intTrip1, strZip) > 0 Then
                blnMatchFound = True
            Else
                blnMatchFound = False
                'Modified by RHR v-7.0.5.100 05/25/2016
                Dim intErr As Integer = PCMSGetError()
                If intErr <> 0 Then
                    strWarnings &= "PC Miler Check Place Name Failure: " & getPCMError(intErr) & ".  Please try again later.  "
                Else
                    strWarnings &= "The " & strAddressType & " postal code, " & strZip & " ,for " & strItemType & " " & strItemNumber & " is not valid.  All postal codes are required." & vbCrLf
                End If
                Return False
            End If
            objPCM.strAddress = "** Address Does Not Exist **" 'objSource.strAddress
            objPCM.strCity = ""
            objPCM.strState = ""
            objPCM.strZip = ""
            If blnMatchFound Then objPCM.strZip = strZip
            If UseZipOnly Then Return True
            'Get the PCmiler street address
            Dim strSource As String
            strSource = strZip & " " & objSource.strCity & ", " & objSource.strState & ";  " & objSource.strAddress
            Dim intRetVal As Integer
            intRetVal = PCMSLookup(intTrip1, strSource, 2)
            If intRetVal < 1 Then
                intRetVal = PCMSLookup(intTrip1, strZip & ";" & objSource.strAddress, 2)
            End If
            If intRetVal > 0 Then
                If PCMSGetMatch(intTrip1, 0, buffer, 254) > 0 Then
                    buff = buffer.ToString
                    'Debug.Print buff
                    savePCMBuffer(buff)
                    If InStr(1, buff, ";") Then
                        objPCM.strAddress = Trim(simpleStreetScrubber(Replace(Mid(buff, InStr(1, buff, ";") + 1, Len(buff)), Chr(0), "")))
                        If InStr(1, objPCM.strAddress, "&") Then
                            objPCM.strAddress = "** Address Does Not Exist **"
                            strWarnings = strWarnings & "The " & strAddressType & " address for " & strItemType & " " & strItemNumber & " does not exist in PCMiler. Using FreightMaster postal code for routing." & vbCrLf
                        Else
                            strStopName = Trim(Replace(buff, Chr(0), ""))
                            '--------------------------------------------
                            'Start Modifiy by RHR 7.0.5.100 05/19/2016
                            '--------------------------------------------
                            'test for US vs Canadian postal codes                        
                            Dim strTestZip As String = buff.Split(" ")(0)
                            Dim strNewCAZip As String = ""
                            If IsNumeric(Left(Trim(strTestZip), 5)) Then
                                'this is a standard US zip code so use the returned value
                                objPCM.strZip = Left(Trim(strTestZip), 5)
                                strMatchZip = Left(strZip, 5)
                                buff = splitCityStateCounty(buff, strTestZip.Length() + 1)
                            Else
                                'check for canadian postal code --Modifiy by RHR for 7.0.5.102 on 12/21/2016
                                strTestZip = Left(buff, 7)
                                If isValidCAZip(strTestZip, strNewCAZip) Then
                                    objPCM.strZip = strNewCAZip
                                    strMatchZip = strZip.Trim()
                                    buff = splitCityStateCounty(buff, strTestZip.Length() + 1)
                                Else
                                    strMatchZip = strZip
                                    objPCM.strZip = strZip
                                    buff = splitCityStateCounty(buff, 1)
                                End If
                            End If

                            '--------------------------------------------
                            'End Modifiy by RHR 7.0.5.100 05/19/2016
                            '--------------------------------------------
                            strPCMilerCityState = Split(buff, ",")
                            objPCM.strCity = Trim(strPCMilerCityState(0))
                            objPCM.strState = Trim(strPCMilerCityState(1))
                            'Compare Our Address with PC Milers Best Match.
                            'If issues we return a value in strMessage to be logged in the database
                            If blnMatchFound Then
                                If LCase(strMatchZip) <> LCase(objPCM.strZip) Then
                                    'the zip code does not match so log a warning that the
                                    'address may not match PCMiler Database using postal code
                                    'for origin
                                    strWarnings = strWarnings & "The " & strAddressType & " postal code for " & strItemType & " " & strItemNumber & " does not match the PCMiler postal code. Using FreightMaster postal code for routing." & vbCrLf
                                    'set stop name back to zip code
                                    strStopName = objSource.strZip
                                Else
                                    'check if the state matches we only use full addressing if states match
                                    If LCase(objPCM.strState) <> LCase(objSource.strState) Then
                                        strWarnings = strWarnings & "The " & strAddressType & " State for " & strItemType & " " & strItemNumber & " does not match the PCMiler State. Using FreightMaster postal code for routing." & vbCrLf
                                        'set stop name back to zip code
                                        strStopName = objSource.strZip
                                    ElseIf LCase(objPCM.strAddress) <> LCase(objSource.strAddress) Then
                                        strWarnings = strWarnings & "The " & strAddressType & " Street Address for " & strItemType & " " & strItemNumber & " does not match the PCMiler Street Address. Using PCMiler Street Address for routing." & vbCrLf
                                        blnMatchFound = True
                                    Else
                                        blnMatchFound = True
                                    End If
                                End If
                            Else
                                'the postal code could not be found we assume user error on
                                'postal code so check city and state for match (if they do not match
                                'the entire address fails and falls into the bad postal code trap farther down)
                                If objPCM.strCity = objSource.strCity Or objPCM.strState = objSource.strState Then
                                    strWarnings = strWarnings & "The " & strAddressType & " Postal Code Is Not Valid Using Closest Match (Please Correct) for " & strItemType & " " & strItemNumber & ".  Using PCMiler address for routing." & vbCrLf
                                    blnMatchFound = True
                                Else
                                    'set stop name back to zip code
                                    strStopName = objSource.strZip
                                    strWarnings = strWarnings & "The " & strAddressType & " Address and postal code for " & strItemType & " " & strItemNumber & " is not valid.  The load could not be routed." & vbCrLf
                                End If
                            End If
                        End If
                    Else
                        strWarnings = strWarnings & "There was a problem with the " & strAddressType & " address for " & strItemType & " " & strItemNumber & " . Using FreightMaster postal code for routing." & vbCrLf
                    End If
                End If
            Else
                'Modified by RHR v-7.0.5.100 06/03/2016
                Dim intErr As Integer = PCMSGetError()
                Dim strPCMilerError As String = "cannot access PC Miler; the system may be busy; please try again later."
                If intErr <> 0 Then
                    strPCMilerError = getPCMError(intErr)
                End If
                If blnMatchFound Then
                    strWarnings &= " The " & strAddressType & " Street Address for " & strItemType & " " & strItemNumber & " cannot be found for the postal code provided. Using FreightMaster postal code for routing." & vbCrLf
                Else
                    strWarnings &= "PC Miler Lookup Address Failure: " & strPCMilerError
                    Return False
                End If
            End If

            validateAddress = blnMatchFound

        End Function

        Private Function AddStop(ByRef objOrig As clsAddress,
                                ByRef objDest As clsAddress,
                                ByRef objPCMOrig As clsAddress,
                                ByRef objPCMDest As clsAddress,
                                ByRef blnAddOrigin As Boolean,
                                ByRef objGlobalStopData As clsGlobalStopData,
                                ByRef blnAddressValid As Boolean,
                                ByRef strOrigStopName As String,
                                ByRef strDestStopName As String,
                                ByVal intTrip1 As Integer,
                                ByVal strItemNumber As String,
                                ByVal strItemType As String,
                                ByVal intBookControl As Integer,
                                ByVal intLaneControl As Integer,
                                ByRef arrBaddAddresses() As clsPCMBadAddress) As Boolean
            Dim blnRet As Boolean = False
            Dim strOriginAddressWarnings As String = ""
            Dim strWarnings As String = ""
            Dim blnLogBadAddress As Boolean = False
            Dim blnOriginAddressValid As Boolean = True
            Dim Ret As Long = 0
            'set default value of function to blnRet (false)
            AddStop = blnRet
            blnAddressValid = False
            '********** Validate and Add the Origin Address *********************
            strOriginAddressWarnings = ""
            If validateAddress(intTrip1,
                            "Origin",
                            strItemNumber,
                            strItemType,
                            objOrig,
                            objPCMOrig,
                            strOrigStopName,
                            strOriginAddressWarnings) Then
                'NOTE: Warnings are not logged until we process the Destination Address Below

                If Len(Trim(strOriginAddressWarnings)) > 0 Then
                    If InStr(1, strWarnings, "(Please Correct)") > 0 And objGlobalStopData.AutoCorrectBadLaneZipCodes = 1 Then
                        If Len(Trim(objPCMOrig.strZip)) > 0 Then
                            objGlobalStopData.OriginZip = objPCMOrig.strZip
                            strOriginAddressWarnings = ""
                        Else
                            blnLogBadAddress = True
                        End If
                    Else
                        blnLogBadAddress = True
                    End If
                End If
            Else
                blnLogBadAddress = True
                If blnAddOrigin Then
                    blnOriginAddressValid = False
                    objGlobalStopData.FailedAddressMessage = objGlobalStopData.FailedAddressMessage & strOriginAddressWarnings
                End If
            End If
            If blnAddOrigin Then
                Ret = PCMSAddStop(intTrip1, strOrigStopName)
                If Ret < 1 Then
                    'The stopname cannot be found so reset to zipcode only
                    If strOrigStopName = objOrig.strZip Then
                        'This is a total failure
                        blnOriginAddressValid = False
                        objGlobalStopData.FailedAddressMessage = objGlobalStopData.FailedAddressMessage & "The origin address cannot be found and the postal code is not valid.  Cannot route load."
                        objPCMOrig.strAddress = "** Address Does Not Exist **"
                        objPCMOrig.strCity = ""
                        objPCMOrig.strState = ""
                        objPCMOrig.strZip = ""
                    Else
                        'try to use the zip code
                        strOrigStopName = objOrig.strZip
                        objPCMOrig.strAddress = "** Address Does Not Exist **"
                        objPCMOrig.strCity = ""
                        objPCMOrig.strState = ""
                        objPCMOrig.strZip = objOrig.strZip
                        Ret = PCMSAddStop(intTrip1, strOrigStopName)
                        If Ret < 1 Then
                            'Give up
                            objPCMOrig.strZip = ""
                            blnOriginAddressValid = False
                            objGlobalStopData.FailedAddressMessage = objGlobalStopData.FailedAddressMessage & "The origin address cannot be found and the postal code is not valid.  Cannot route load."
                        Else
                            blnOriginAddressValid = True
                            blnAddOrigin = False
                        End If
                    End If
                Else
                    blnOriginAddressValid = True
                    blnAddOrigin = False
                End If
            End If
            strWarnings = ""
            If validateAddress(intTrip1,
                                "Destination",
                                strItemNumber,
                                strItemType,
                                objDest,
                                objPCMDest,
                                strDestStopName,
                                strWarnings) Then
                If blnOriginAddressValid Then
                    Ret = PCMSAddStop(intTrip1, strDestStopName)
                    If Ret < 1 Then
                        If strDestStopName = objDest.strZip Then
                            objGlobalStopData.FailedAddressMessage = objGlobalStopData.FailedAddressMessage & "The destination address cannot be found and the postal code is not valid.  Cannot route load."
                            objPCMDest.strAddress = "** Address Does Not Exist **"
                            objPCMDest.strCity = ""
                            objPCMDest.strState = ""
                            objPCMDest.strZip = ""
                        Else
                            'Try to use the zip code
                            strDestStopName = objDest.strZip
                            objPCMDest.strAddress = "** Address Does Not Exist **"
                            objPCMDest.strCity = ""
                            objPCMDest.strState = ""
                            objPCMDest.strZip = objDest.strZip
                            Ret = PCMSAddStop(intTrip1, strDestStopName)
                            If Ret < 1 Then
                                'Give up
                                objGlobalStopData.FailedAddressMessage = objGlobalStopData.FailedAddressMessage & "The destination address cannot be found and the postal code is not valid.  Cannot route load."
                                objPCMDest.strZip = ""
                            Else
                                blnAddressValid = True
                            End If
                        End If
                    Else
                        blnAddressValid = True
                    End If
                End If
                'We need clarificaiton on what is being done here!!!
                If Len(Trim(strWarnings)) > 0 Then
                    If InStr(1, strWarnings, "(Please Correct)") > 0 And objGlobalStopData.AutoCorrectBadLaneZipCodes = 1 Then
                        If Len(Trim(objPCMDest.strZip)) > 0 Then
                            objGlobalStopData.DestZip = objPCMDest.strZip
                            strWarnings = ""
                        Else
                            blnLogBadAddress = True
                        End If
                    Else
                        blnLogBadAddress = True
                    End If
                End If
            Else
                objGlobalStopData.FailedAddressMessage = objGlobalStopData.FailedAddressMessage & strWarnings
                blnLogBadAddress = True
            End If
            If blnLogBadAddress Then
                'add the bad address to the array
                AddBaddAddressToArray(intBookControl, intLaneControl, objOrig, objDest, objPCMOrig, objPCMDest, strOriginAddressWarnings & strWarnings, objGlobalStopData.BatchID, arrBaddAddresses)
            End If

            Return True

        End Function

        Private Function simpleStreetScrubber(ByVal strStreet As String) As String

            Try
                strStreet = " " & LCase(strStreet) & " "
                If InStr(1, strStreet, " dr ", vbTextCompare) > 0 Then
                    strStreet = Replace(strStreet, " dr ", " drive ", 1, 1, vbTextCompare)
                End If
                If InStr(1, strStreet, " ave ", vbTextCompare) > 0 Then
                    strStreet = Replace(strStreet, " ave ", " avenue ", 1, 1, vbTextCompare)
                End If
                If InStr(1, strStreet, " blvd ", vbTextCompare) > 0 Then
                    strStreet = Replace(strStreet, " blvd ", " boulevard ", 1, 1, vbTextCompare)
                End If
                If InStr(1, strStreet, " st ", vbTextCompare) > 0 Then
                    strStreet = Replace(strStreet, " st ", " street ", 1, 1, vbTextCompare)
                End If
                If InStr(1, strStreet, " e ", vbTextCompare) > 0 Then
                    strStreet = Replace(strStreet, " e ", " east ", 1, 1, vbTextCompare)
                End If
                If InStr(1, strStreet, " w ", vbTextCompare) > 0 Then
                    strStreet = Replace(strStreet, " w ", " west ", 1, 1, vbTextCompare)
                End If
                If InStr(1, strStreet, " s ", vbTextCompare) > 0 Then
                    strStreet = Replace(strStreet, " s ", " south ", 1, 1, vbTextCompare)
                End If
                If InStr(1, strStreet, " n ", vbTextCompare) > 0 Then
                    strStreet = Replace(strStreet, " n ", " north ", 1, 1, vbTextCompare)
                End If
                If InStr(1, strStreet, " cir ", vbTextCompare) > 0 Then
                    strStreet = Replace(strStreet, " cir ", " circle ", 1, 1, vbTextCompare)
                End If
                If InStr(1, strStreet, " ct ", vbTextCompare) > 0 Then
                    strStreet = Replace(strStreet, " ct ", " court ", 1, 1, vbTextCompare)
                End If
                If InStr(1, strStreet, " ne ", vbTextCompare) > 0 Then
                    strStreet = Replace(strStreet, " ne ", " northeast ", 1, 1, vbTextCompare)
                End If
                If InStr(1, strStreet, " nw ", vbTextCompare) > 0 Then
                    strStreet = Replace(strStreet, " nw ", " northwest ", 1, 1, vbTextCompare)
                End If
                If InStr(1, strStreet, " pkwy ", vbTextCompare) > 0 Then
                    strStreet = Replace(strStreet, " pkwy ", " parkway ", 1, 1, vbTextCompare)
                End If
                If InStr(1, strStreet, " rd ", vbTextCompare) > 0 Then
                    strStreet = Replace(strStreet, " rd ", " road ", 1, 1, vbTextCompare)
                End If
                If InStr(1, strStreet, " trl ", vbTextCompare) > 0 Then
                    strStreet = Replace(strStreet, " trl ", " trail ", 1, 1, vbTextCompare)
                End If
                If InStr(1, strStreet, " sq ", vbTextCompare) > 0 Then
                    strStreet = Replace(strStreet, " sq ", " square ", 1, 1, vbTextCompare)
                End If
            Catch ex As Exception
                'do nothing
            End Try
            Return strStreet

        End Function
#End Region

#Region "New Methods for v-5.1.4"


        ''' <summary>
        ''' strAddress must be in the following format:
        ''' Zip  City, State; Street
        ''' </summary>
        ''' <param name="strAddress"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' Modified by RHR v-7.0.5.100 05/25/2016 
        '''   added logic to check trip id and retry 3 times if possible.
        ''' </remarks>
        Public Function PCMValidateAddress(ByVal strAddress As String) As Boolean
            Dim intTrip1 As Integer = 0
            _strLastError = ""
            Dim blnRet As Boolean = False
            Try
                gProcessRunning = True
                If Not gServerID > 0 Then
                    If Not PCMilerStart() > 0 Then
                        Return Nothing
                    End If
                End If
                'create a new PCMiler trip
                intTrip1 = PCMSNewTrip(gServerID)
                'Modified by RHR v-7.0.5.100 05/25/2016
                ' added logic to check trip id and retry 
                ' 3 times if possible.
                If intTrip1 < 1 Then
                    For intRetry As Integer = 1 To 3
                        PCMilerEnd()
                        If Not PCMilerStart() Then Return Nothing
                        intTrip1 = PCMSNewTrip(gServerID)
                        If intTrip1 > 0 Then Exit For
                    Next
                End If
                'Modified by RHR v-7.0.5.100 05/25/2016
                '  We now throw an invalid operation exception if the trip id is not valid
                If intTrip1 < 1 Then Throw New InvalidOperationException("Invalid Trip ID: Cannot connect to PC Miler.  Please wait and try again.")

                Dim intRetVal As Integer = PCMSLookup(intTrip1, strAddress, 2)
                If intRetVal > 0 Then blnRet = True

            Catch ex As System.AccessViolationException
                LogError("Cannot execute PCMValidateAddress: PC Miler is no longer running.", ex)
            Catch ex As Exception
                LogError("Cannot execute PCMValidateAddress. ", ex)
            Finally
                Try
                    PCMSDeleteTrip(intTrip1)
                Catch ex As Exception

                End Try
                gProcessRunning = False
            End Try
            Return blnRet
        End Function

        ''' <summary>
        ''' Checks if the address is valid with PC Miler.  attemtpts to find an alternative if possible.
        ''' NOTE: PCMilerStart must be called by the calling function.
        ''' This method replaces validateAddress as of v-5.1.4
        ''' </summary>
        ''' <param name="intTrip1"></param>
        ''' <param name="oSource"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' Modified by RHR 7.0.5.100 05/19/2016
        '''   fixed issues with canadian postal code parsing using PC Miler buffer returned
        '''   fixed issue with parsing state or region when county is not provided
        ''' Modified by RHR v-7.0.5.100 05/25/2016
        '''   added logic to manage PCM error codes
        ''' </remarks>
        Private Function validateAddressEx(ByVal intTrip1 As Integer,
                                           ByRef oSource As clsFMStopData) As Boolean
            Dim blnMatchFound As Boolean = False
            Dim strPCMilerCityState() As String
            Dim buffer As StringBuilder = New StringBuilder(256)
            Dim buff As String = ""
            Dim strMatchZip As String = ""
            Dim strZip As String = ""
            Dim intDash As Integer = 0
            Dim strAddressType As String = "Destination"

            With oSource
                If .LocationisOrigin Then strAddressType = "Origin"
                intDash = InStr(1, .Zip, "-")
                If intDash Then
                    strZip = Left(.Zip, intDash - 1)
                Else
                    strZip = .Zip
                End If
                .StopName = .Zip
                If PCMSCheckPlaceName(intTrip1, strZip) > 0 Then
                    blnMatchFound = True
                Else
                    blnMatchFound = False
                    'Modified by RHR v-7.0.5.100 05/25/2016
                    Dim intErr As Integer = PCMSGetError()
                    If intErr <> 0 Then
                        .Warning &= "PC Miler Check Place Name Failure: " & getPCMError(intErr) & ".  Please try again later.  "
                    Else
                        .Warning &= "The " & strAddressType & " postal code, " & strZip & " ,for PRO Number " & .BookProNumber & " is not valid.  All postal codes are required." & vbCrLf
                    End If
                    Return False
                End If
                .PCMilerState = "** Address Does Not Exist **" 'objSource.strAddress
                .PCMilerCity = ""
                .PCMilerState = ""
                .PCMilerZip = ""
                If blnMatchFound Then .Zip = strZip
                'public configuration property UseZipOnly is configured by the caller typically in the parameter table
                'If true we do not attempt to lookup the street address
                If UseZipOnly Then Return True
                'Get the PCmiler street address
                Dim strSource As String
                strSource = strZip & " " & .City & ", " & .State & ";  " & .Street
                Dim intRetVal As Integer
                intRetVal = PCMSLookup(intTrip1, strSource, 2)
                If intRetVal < 1 Then
                    intRetVal = PCMSLookup(intTrip1, strZip & ";" & .Street, 2)
                End If
                If intRetVal > 0 Then
                    If PCMSGetMatch(intTrip1, 0, buffer, 254) > 0 Then
                        buff = buffer.ToString
                        'Debug.Print buff
                        savePCMBuffer(buff)
                        If InStr(1, buff, ";") Then
                            .PCMilerStreet = Trim(simpleStreetScrubber(Replace(Mid(buff, InStr(1, buff, ";") + 1, Len(buff)), Chr(0), "")))
                            If InStr(1, .PCMilerStreet, "&") Then
                                .PCMilerStreet = "** Address Does Not Exist **"
                                .Warning = .Warning & "The " & strAddressType & " address for PRO Number " & .BookProNumber & " does not exist in PCMiler. Using FreightMaster postal code for routing." & vbCrLf
                            Else
                                .StopName = Trim(Replace(buff, Chr(0), ""))
                                '--------------------------------------------
                                'Start Modifiy by RHR 7.0.5.100 05/19/2016
                                '--------------------------------------------
                                'test for US vs Canadian postal codes
                                'PC Miler will sometimes return a postal code as the first portion fo the buffer before the city name so 
                                'we need to use this if available and split it out from the city
                                Dim strTestZip As String = buff.Split(" ")(0)
                                'strTestZip = Trim(Left(buff, 5))
                                Dim strNewCAZip As String = ""
                                If IsNumeric(Left(Trim(strTestZip), 5)) Then
                                    'this is a standard US zip code so use the returned value
                                    .PCMilerZip = Left(Trim(strTestZip), 5) '  Trim(Left(buff, 6))
                                    strMatchZip = Left(strZip, 5)
                                    buff = splitCityStateCounty(buff, strTestZip.Length() + 1)
                                Else
                                    'check for canadian postal code --Modifiy by RHR for 7.0.5.102 on 12/21/2016
                                    strTestZip = Left(buff, 7)
                                    If isValidCAZip(strTestZip, strNewCAZip) Then
                                        .PCMilerZip = strNewCAZip
                                        strMatchZip = strZip.Trim()
                                        buff = splitCityStateCounty(buff, strTestZip.Length() + 1)
                                    Else
                                        strMatchZip = strZip
                                        .PCMilerZip = strZip
                                        buff = splitCityStateCounty(buff, 1)
                                    End If
                                End If

                                '--------------------------------------------
                                'End Modifiy by RHR 7.0.5.100 05/19/2016
                                '--------------------------------------------
                                strPCMilerCityState = Split(buff, ",")
                                .PCMilerCity = Trim(strPCMilerCityState(0))
                                .PCMilerState = Trim(strPCMilerCityState(1))
                                'Compare Our Address with PC Milers Best Match.
                                'If issues we return a value in strMessage to be logged in the database
                                If blnMatchFound Then
                                    If LCase(strMatchZip) <> LCase(.PCMilerZip) Then
                                        'the zip code does not match so log a warning that the
                                        'address may not match PCMiler Database using postal code
                                        'for origin
                                        .Warning &= " The " & strAddressType & " postal code for PRO Number " & .BookProNumber & " does not match the PCMiler postal code. Using FreightMaster postal code for routing." & vbCrLf
                                        'set stop name back to zip code
                                        .StopName = .Zip
                                    Else
                                        'check if the state matches we only use full addressing if states match
                                        If LCase(.PCMilerState) <> LCase(.State) Then
                                            .Warning &= " The " & strAddressType & " State for PRO Number " & .BookProNumber & " does not match the PCMiler State. Using FreightMaster postal code for routing." & vbCrLf
                                            'set stop name back to zip code
                                            .StopName = .Zip
                                        ElseIf LCase(.PCMilerStreet) <> LCase(.Street) Then
                                            .Warning &= " The " & strAddressType & " Street Address for PRO Number" & .BookProNumber & " does not match the PCMiler Street Address. Using PCMiler Street Address for routing." & vbCrLf
                                            blnMatchFound = True
                                        Else
                                            blnMatchFound = True
                                        End If
                                    End If
                                Else
                                    'the postal code could not be found we assume user error on
                                    'postal code so check city and state for match (if they do not match
                                    'the entire address fails and falls into the bad postal code trap farther down)
                                    If .PCMilerCity = .City Or .PCMilerState = .State Then
                                        .Warning &= " The " & strAddressType & " Postal Code Is Not Valid Using Closest Match (Please Correct) for PRO Number " & .BookProNumber & ".  Using PCMiler address for routing." & vbCrLf
                                        blnMatchFound = True
                                    Else
                                        'set stop name back to zip code
                                        .StopName = .Zip
                                        .Warning &= " The " & strAddressType & " Address and postal code for PRO Number" & .BookProNumber & " is not valid.  The load could not be routed." & vbCrLf
                                    End If
                                End If
                            End If
                        Else
                            .Warning &= " There was a problem with the " & strAddressType & " address for PRO Number " & .BookProNumber & " . Using FreightMaster postal code for routing." & vbCrLf
                        End If
                    End If
                Else
                    'Modified by RHR v-7.0.5.100 06/03/2016
                    Dim intErr As Integer = PCMSGetError()
                    Dim strPCMilerError As String = "cannot access PC Miler; the system may be busy; please try again later."
                    If intErr <> 0 Then
                        strPCMilerError = getPCMError(intErr)
                    End If
                    If blnMatchFound Then
                        .Warning &= " The " & strAddressType & " Street Address for PRO Number " & .BookProNumber & " cannot be found for the postal code provided. Using FreightMaster postal code for routing." & vbCrLf
                    Else
                        .Warning &= "PC Miler Lookup Address Failure: " & strPCMilerError
                        Return False
                    End If
                End If
            End With
            Return blnMatchFound

        End Function

        Private Sub savePCMBuffer(ByVal buff As String)
            If String.IsNullOrEmpty(buff) Then Return
            Dim lBuff As New List(Of String)
            If Not PCMBuffers Is Nothing Then
                lBuff = PCMBuffers.ToList()
            End If
            lBuff.Add(buff)
            PCMBuffers = lBuff.ToArray()
        End Sub



        Private Function splitCityStateCounty(ByVal buff As String, ByVal intStart As Integer) As String
            Dim sRet As String = buff
            Try
                If intStart >= buff.Length Then intStart = buff.Length - 1
                If intStart < 1 Then intStart = 1
                If InStr(1, buff, ";") > 0 Then
                    buff = buff.Split(";")(0)
                End If
                sRet = Mid(buff, intStart)

            Catch ex As Exception
                'on error just return sRet as default
            End Try
            Return sRet

        End Function


        ''' <summary>
        ''' tests that zipOriginal matches the following format A#A #A#  where A = a charactere between A and Z and # is a number between 0 an 9.
        ''' if a space does not exist after the first 3 characters one is inserted.  the newly formatted postal code is returned in zipNew
        ''' the function returns true if the format works or false if it fails.
        ''' </summary>
        ''' <param name="zipOriginal"></param>
        ''' <param name="zipNew"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR 05/18/2016
        ''' </remarks>
        Private Function isValidCAZip(ByVal zipOriginal As String, ByRef zipNew As String) As Boolean
            Dim blnRet As Boolean = False
            Try
                If String.IsNullOrEmpty(zipOriginal) Then Return False
                Dim strTest As String = Trim(zipOriginal)
                If strTest.Length < 6 Or strTest.Length > 7 Then Return False
                Dim intCt As Integer = 0
                Dim strNew As String = ""
                If strTest.Length < 7 Then
                    'add a space after the first 3 characters
                    strTest = Left(strTest, 3) & " " & Right(strTest, 3)
                End If
                'all character tests expect upper case letters
                strTest = strTest.ToUpper()
                For i As Integer = 0 To strTest.Length - 1
                    Dim c As Char = strTest(i)
                    Select Case i
                        Case 0
                            If Not isCharCAZipAlpha(c) Then Return False
                        Case 1
                            If Not isCharCAZipNumeric(c) Then Return False
                        Case 2
                            If Not isCharCAZipAlpha(c) Then Return False
                        Case 3
                            If Not c = " " Then Return False
                        Case 4
                            If Not isCharCAZipNumeric(c) Then Return False
                        Case 5
                            If Not isCharCAZipAlpha(c) Then Return False
                        Case 6
                            If Not isCharCAZipNumeric(c) Then Return False
                        Case Else
                            Return False
                    End Select
                Next
                'if we get here the zip is a valid CA Postal Code
                blnRet = True
                zipNew = strTest
            Catch ex As Exception
                'skip erorrs and return false
            End Try

            Return blnRet
        End Function



        ''' <summary>
        ''' test the upper case value of the character it must be between A and Z
        ''' </summary>
        ''' <param name="c"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function isCharCAZipAlpha(ByVal c As Char) As Boolean
            If c >= "A" AndAlso c <= "Z" Then
                Return True
            Else
                Return False
            End If
        End Function

        Private Function isCharCAZipNumeric(ByVal c As Char) As Boolean
            Dim intVal As Integer = -1
            If Not Integer.TryParse(c.ToString(), intVal) Then Return False
            If intVal >= 0 AndAlso intVal <= 9 Then
                Return True
            Else
                Return False
            End If
        End Function

        Private Function AddStopEx(ByVal intTrip1 As Integer,
                                   ByRef oFMStop As clsFMStopData,
                                   ByRef objGlobalStopData As clsGlobalStopData) As Boolean
            Dim blnRet As Boolean = True
            Dim Ret As Long = 0
            Dim strAddressType As String = "Destination"
            With oFMStop
                If .LocationisOrigin Then strAddressType = "Origin"
                .AddressValid = False
                .Warning = ""
                If validateAddressEx(intTrip1, oFMStop) Then
                    Ret = PCMSAddStop(intTrip1, .StopName)
                    If Ret < 1 Then
                        'The stopname cannot be found so reset to zipcode only
                        If .StopName = .Zip Then
                            'This is a total failure
                            blnRet = False
                            objGlobalStopData.FailedAddressMessage = objGlobalStopData.FailedAddressMessage & "The " & strAddressType & " address cannot be found and the postal code is not valid.  Cannot route load."
                            .PCMilerStreet = "** Address Does Not Exist **"
                            .PCMilerCity = ""
                            .PCMilerState = ""
                            .PCMilerZip = ""
                        Else
                            'try to use the zip code
                            .StopName = .Zip
                            .PCMilerStreet = "** Address Does Not Exist **"
                            .PCMilerCity = ""
                            .PCMilerState = ""
                            .PCMilerZip = .Zip
                            Ret = PCMSAddStop(intTrip1, .StopName)
                            If Ret < 1 Then
                                'Give up
                                .PCMilerZip = ""
                                blnRet = False
                                objGlobalStopData.FailedAddressMessage = objGlobalStopData.FailedAddressMessage & "The " & strAddressType & " address cannot be found and the postal code is not valid.  Cannot route load."
                            Else
                                .AddressValid = True
                            End If
                        End If
                    Else
                        .AddressValid = True
                    End If
                    If Len(Trim(.Warning)) > 0 Then
                        If InStr(1, .Warning, "(Please Correct)") > 0 And objGlobalStopData.AutoCorrectBadLaneZipCodes = 1 Then
                            If Len(Trim(.PCMilerZip)) > 0 Then
                                If .LocationisOrigin Then
                                    objGlobalStopData.OriginZip = .PCMilerZip
                                Else
                                    objGlobalStopData.DestZip = .PCMilerZip
                                End If
                                .Warning = ""
                            Else
                                .LogBadAddress = True
                            End If
                        Else
                            .LogBadAddress = True
                        End If
                    End If
                Else
                    .LogBadAddress = True
                    .AddressValid = False
                    blnRet = False
                    objGlobalStopData.FailedAddressMessage &= .Warning
                End If
            End With
            Return blnRet

        End Function


        ''' <summary>
        ''' This method replaces PCMReSync as of v-5.4.1
        ''' It now uses all origin addresses as part of the solution and returns 
        ''' Miles associated with multi-pick and multi-stop data
        ''' </summary>
        ''' <param name="arrFMStops"></param>
        ''' <param name="dblBatchID"></param>
        ''' <param name="blnKeepStopNumbers"></param>
        ''' <param name="arrPCMReportRecords"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' Rules for routing:
        ''' The blnKeepStopNumbers flag forces miles to be calculated
        ''' In the order in which they are transmitted.  This includes both 
        ''' Origin and Destination addressing and works for inbund or outbound
        ''' loads
        ''' Modified by RHR for v-7.0.6.101 on 2/9/2017  to call overload PCMReSyncMultiStopEx
        ''' </remarks>
        Public Function PCMReSyncMultiStop(ByRef arrFMStops As clsFMStopData(),
                            ByVal dblBatchID As Double,
                            ByVal blnKeepStopNumbers As Boolean,
                            ByRef arrPCMReportRecords As clsPCMReportRecord()) As clsGlobalStopData
            Return PCMReSyncMultiStopEx(arrFMStops,
                                        INGL_Service_PCMiler.PCMEX_CALCTYPE.CALCTYPE_NONE,
                                        INGL_Service_PCMiler.PCMEX_Opt_Flags.OPT_NONE, INGL_Service_PCMiler.PCMEX_Veh_Type.CALCEX_VEH_TRUCK,
                                        dblBatchID,
                                        blnKeepStopNumbers,
                                        arrPCMReportRecords)

        End Function

        ''' <summary>
        ''' Extended Overload which provides support for PCMSSetCalcTypeEx for use 
        ''' with extended calculation options like shortest 53Foot routing
        ''' </summary>
        ''' <param name="arrFMStops"></param>
        ''' <param name="EXCALC_Type"></param>
        ''' <param name="EXOpt_Flags"></param>
        ''' <param name="EXVeh_Type"></param>
        ''' <param name="dblBatchID"></param>
        ''' <param name="blnKeepStopNumbers"></param>
        ''' <param name="arrPCMReportRecords"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR for v-7.0.6.101 on 2/9/2017 
        '''   provides support for PCMSSetCalcTypeEx for use 
        '''   with extended calculation options like shortest 53Foot routing
        ''' </remarks>
        Public Function PCMReSyncMultiStopEx(ByRef arrFMStops As clsFMStopData(),
                                             ByVal EXCALC_Type As INGL_Service_PCMiler.PCMEX_CALCTYPE,
                                             ByVal EXOpt_Flags As INGL_Service_PCMiler.PCMEX_Opt_Flags,
                                             ByVal EXVeh_Type As INGL_Service_PCMiler.PCMEX_Veh_Type,
                                             ByVal dblBatchID As Double,
                                             ByVal blnKeepStopNumbers As Boolean,
                                             ByRef arrPCMReportRecords As clsPCMReportRecord()) As clsGlobalStopData

            Dim oFMStops As List(Of clsFMStopData) = New List(Of clsFMStopData)(arrFMStops)
            Dim oPCMReportRecords As List(Of clsPCMReportRecord)
            Dim oGlobalStopData = PCMReSyncMultiStop(oFMStops,
                                                     EXCALC_Type,
                                                     EXOpt_Flags,
                                                     EXVeh_Type,
                                                     dblBatchID,
                                                     blnKeepStopNumbers,
                                                     oPCMReportRecords)
            If Not oFMStops Is Nothing AndAlso oFMStops.Count > 0 Then arrFMStops = oFMStops.ToArray
            If Not oPCMReportRecords Is Nothing AndAlso oPCMReportRecords.Count > 0 Then arrPCMReportRecords = oPCMReportRecords.ToArray

            Return oGlobalStopData

        End Function




        ''' <summary>
        ''' This method replaces PCMReSync as of v-5.4.1
        ''' It now uses all origin addresses as part of the solution and returns 
        ''' Miles associated with multi-pick and multi-stop data
        ''' </summary>
        ''' <param name="oFMStops"></param>
        ''' <param name="dblBatchID"></param>
        ''' <param name="blnKeepStopNumbers"></param>
        ''' <param name="oPCMReportRecords"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' Rules for routing:
        ''' The blnKeepStopNumbers flag forces miles to be calculated
        ''' In the order in which they are transmitted.  This includes both 
        ''' Origin and Destination addressing and works for inbund or outbound
        ''' loads
        ''' Modified by RHR v-7.0.5.100 05/25/2016 
        '''   added logic to check trip id and retry 3 times if possible.
        ''' Modified by RHR for v-7.0.6.101 on 02/09/2017
        '''   provides support for PCMSSetCalcTypeEx for use 
        '''   with extended calculation options like shortest 53Foot routing
        '''  </remarks>
        Private Function PCMReSyncMultiStop(ByRef oFMStops As List(Of clsFMStopData),
                                            ByVal EXCALC_Type As INGL_Service_PCMiler.PCMEX_CALCTYPE,
                                            ByVal EXOpt_Flags As INGL_Service_PCMiler.PCMEX_Opt_Flags,
                                            ByVal EXVeh_Type As INGL_Service_PCMiler.PCMEX_Veh_Type,
                                            ByVal dblBatchID As Double,
                                            ByVal blnKeepStopNumbers As Boolean,
                                            ByRef oPCMReportRecords As List(Of clsPCMReportRecord)) As clsGlobalStopData

            Dim Ret As Integer = 0
            Dim i As Integer = 0
            Dim intTrip1 As Integer = 0
            Dim strOrigStopName As String = ""
            Dim strDestStopName As String = ""
            Dim shtDupStreetsCt As Short = 0
            Dim shtLoopCt As Short = 0
            Dim blnAddOrigin As Boolean = True
            Dim oGlobalStopData As New clsGlobalStopData
            Dim strMatchedStreet As String = ""
            Dim objOrig As New clsAddress
            Dim objDest As New clsAddress
            Dim objPCMOrig As New clsAddress
            Dim objPCMDest As New clsAddress
            Dim blnAddressValid As Boolean = True
            Dim Reseq_Type As Integer = 1
            _strLastError = ""

            Try
                If oFMStops Is Nothing OrElse oFMStops.Count < 1 Then Return Nothing
                gProcessRunning = True
                oGlobalStopData.BadAddressCount = 0
                oGlobalStopData.FailedAddressMessage = ""
                oGlobalStopData.BatchID = dblBatchID
                If Not gServerID > 0 Then
                    If Not PCMilerStart() > 0 Then
                        Return Nothing
                    End If
                End If
                'create a new PCMiler trip
                intTrip1 = PCMSNewTrip(gServerID)
                'Modified by RHR v-7.0.5.100 05/25/2016
                ' added logic to check trip id and retry 
                ' 3 times if possible.
                If intTrip1 < 1 Then
                    For intRetry As Integer = 1 To 3
                        PCMilerEnd()
                        If Not PCMilerStart() Then Return Nothing
                        intTrip1 = PCMSNewTrip(gServerID)
                        If intTrip1 > 0 Then Exit For
                    Next
                End If
                'Modified by RHR v-7.0.5.100 05/25/2016
                '  We now throw an invalid operation exception if the trip id is not valid
                If intTrip1 < 1 Then Throw New InvalidOperationException("Invalid Trip ID: Cannot connect to PC Miler.  Please wait and try again.")

                'set up parameters all stops on the load must be similar
                'Modified by RHR for v-7.0.6.101 on 2/9/2017 
                If EXCALC_Type <> INGL_Service_PCMiler.PCMEX_CALCTYPE.CALCTYPE_NONE Then
                    Call PCMSSetCalcTypeEx(intTrip1, EXCALC_Type, EXOpt_Flags, EXVeh_Type)
                Else
                    Call PCMSSetCalcType(intTrip1, oFMStops(0).RouteType)
                End If

                PCMSSetMiles(intTrip1)
                If oFMStops(0).DistType = 1 Then
                    'we are using Kilometers
                    PCMSSetKilometers(intTrip1)
                End If
                'Modified by RHR v-7.0.5.100 05/25/2016
                'added logic to get the most optimal origin on multi-pick loads
                'if we are not keeping stop numbers
                If Not blnKeepStopNumbers Then
                    Dim oReports As New List(Of System.Collections.Generic.List(Of clsPCMReportRecord))
                    Dim intOrigins As Integer = oFMStops.Where(Function(x) x.LocationisOrigin = True).Count()
                    If intOrigins = 0 Then
                        'just add the stops normally
                        If Not buildTrip(oFMStops, oPCMReportRecords, oGlobalStopData, intTrip1, blnKeepStopNumbers) Then Return Nothing
                    Else
                        Dim blnTripAdded As Boolean = False
                        Dim lOriginStopKeys As New List(Of String)
                        'create an array of the original stops so we have an immutable index
                        'sorted by LocationisOrigin
                        Dim StopIndexed() As clsFMStopData = (From s In oFMStops Order By s.LocationisOrigin Select s).ToArray()
                        If Not StopIndexed Is Nothing AndAlso StopIndexed.Count() > 0 Then
                            For index As Integer = 0 To StopIndexed.Count() - 1
                                Dim oStop = StopIndexed(index)
                                If oStop.LocationisOrigin Then
                                    'we reprocess each origin to get optimal miles
                                    Dim strStopKey As String = String.Concat(oStop.Street, oStop.City, oStop.State, oStop.Zip).ToLower()
                                    If Not lOriginStopKeys.Contains(strStopKey) Then
                                        'we have not processed this origin so save the key and build a trip
                                        lOriginStopKeys.Add(strStopKey)
                                        Dim oSortedStops As New List(Of clsFMStopData)
                                        Dim oSortedReportRecords As New List(Of clsPCMReportRecord)
                                        Dim oSortedGlobalStopData As New clsGlobalStopData
                                        oSortedStops.Add(oStop) 'add this origin first
                                        For ii As Integer = 0 To StopIndexed.Count() - 1
                                            If ii <> index Then
                                                oSortedStops.Add(StopIndexed(ii))
                                            End If
                                        Next
                                        'build the trim
                                        Dim blnBuildTrip = buildTrip(oSortedStops, oSortedReportRecords, oSortedGlobalStopData, intTrip1, blnKeepStopNumbers)
                                        If blnBuildTrip Then
                                            blnTripAdded = True
                                            If Not oSortedReportRecords Is Nothing AndAlso oSortedReportRecords.Count() > 0 Then
                                                If oPCMReportRecords Is Nothing Then
                                                    oPCMReportRecords = oSortedReportRecords
                                                    oGlobalStopData = oSortedGlobalStopData
                                                ElseIf oPCMReportRecords.Count() > 0 Then
                                                    If oSortedReportRecords(oSortedReportRecords.Count() - 1).TotalMiles < oPCMReportRecords(oPCMReportRecords.Count() - 1).TotalMiles Then
                                                        oPCMReportRecords = oSortedReportRecords
                                                        oGlobalStopData = oSortedGlobalStopData
                                                    End If
                                                End If
                                            End If
                                        End If
                                    End If
                                End If
                            Next
                            'if we could not load any stop and if we have an error just return nothing
                            If Not blnTripAdded AndAlso Not String.IsNullOrEmpty(_strLastError) Then Return Nothing

                        End If
                    End If
                End If
                'Modified by RHR v-7.0.5.100 05/25/2016
                '  included a default routing option and calls the new method buildTrip
                If oPCMReportRecords Is Nothing OrElse oPCMReportRecords.Count() < 1 Then
                    'we do not have a solution so just add the stops normally (this always happens when blnKeepStopNumbers is true)
                    If Not buildTrip(oFMStops, oPCMReportRecords, oGlobalStopData, intTrip1, blnKeepStopNumbers) Then Return Nothing
                End If
                'We now loop through each stop and lookup the correct match in the PCMS report list                           
                strMatchedStreet = ""
                Dim strMatchedStops As New List(Of String)
                For Each oPCMStop As clsFMStopData In oFMStops
                    Dim MStreet = oPCMStop.PCMilerStreet
                    Dim MCity = oPCMStop.PCMilerCity
                    Dim MState = oPCMStop.PCMilerState
                    Dim MZip = oPCMStop.Zip
                    Dim MStopSeq = oPCMStop.SeqNumber
                    Dim MStopNbr = oPCMStop.StopNumber
                    Dim oMatch As List(Of clsPCMReportRecord)
                    If blnKeepStopNumbers Then
                        'We do not change the sequence so we lookup the data based on the sequence number
                        oMatch = (From d In oPCMReportRecords
                                  Where
                                     d.Street = MStreet _
                                     And d.State = MState _
                                     And d.City = MCity _
                                     And d.SeqNumber = MStopSeq
                                  Select d).ToList
                        'If no street match fall back to zip code
                        If oMatch Is Nothing OrElse oMatch.Count < 1 Then
                            oMatch = (From d In oPCMReportRecords
                                      Where
                                         d.Zip = MZip _
                                         And d.SeqNumber = MStopSeq
                                      Select d).ToList
                        End If
                        'If no zip code match fall back to city and state
                        If oMatch Is Nothing OrElse oMatch.Count < 1 Then
                            oMatch = (From d In oPCMReportRecords
                                      Where
                                         d.State = MState _
                                         And d.City = MCity _
                                         And d.SeqNumber = MStopSeq
                                      Select d).ToList
                        End If
                        If oMatch Is Nothing OrElse oMatch.Count < 1 Then
                            'there is a problem and we cannot get the miles for this stop so set the miles and stop number to zero
                            oPCMStop.StopNumber = 0
                            oPCMStop.LegMiles = 0
                            oPCMStop.TotalMiles = 0
                            oPCMStop.LegCost = 0
                            oPCMStop.TotalCost = 0
                            oPCMStop.LegTime = ""
                            oPCMStop.TotalTime = ""
                            '****************** NOTE:  we need to provide more information to the user about this problem. ********************
                        Else
                            'update the stop data with the first match found
                            'oPCMStop.StopNumber = oMatch(0).StopNumber
                            oPCMStop.LegMiles = oMatch(0).LegMiles
                            oPCMStop.TotalMiles = oMatch(0).TotalMiles
                            oPCMStop.LegCost = oMatch(0).LegCost
                            oPCMStop.TotalCost = oMatch(0).TotalCost
                            oPCMStop.LegTime = oMatch(0).LegTime
                            oPCMStop.TotalTime = oMatch(0).TotalTime
                            oPCMStop.Matched = True
                        End If
                    Else
                        'Stops have been changed so we need to determine if duplicate stops exist for the same address and which one gets the miles
                        'remembering that miles for the second load at the same stop are zero. Currently we make this decision in the order they are 
                        'transmitted to us
                        oMatch = (From d In oPCMReportRecords
                                  Where
                                     d.Street = MStreet _
                                     And d.State = MState _
                                     And d.City = MCity
                                  Select d).ToList
                        'If no street match fall back to zip code
                        If oMatch Is Nothing OrElse oMatch.Count < 1 Then
                            oMatch = (From d In oPCMReportRecords
                                      Where
                                         d.Zip = MZip
                                      Select d).ToList
                        End If
                        'If no zip code match fall back to city and state
                        If oMatch Is Nothing OrElse oMatch.Count < 1 Then
                            oMatch = (From d In oPCMReportRecords
                                      Where
                                         d.State = MState _
                                         And d.City = MCity
                                      Select d).ToList
                        End If
                        If oMatch Is Nothing OrElse oMatch.Count < 1 Then
                            'there is a problem and we cannot get the miles for this stop so set the miles and stop number to zero
                            oPCMStop.StopNumber = 0
                            oPCMStop.LegMiles = 0
                            oPCMStop.TotalMiles = 0
                            oPCMStop.LegCost = 0
                            oPCMStop.TotalCost = 0
                            oPCMStop.LegTime = ""
                            oPCMStop.TotalTime = ""
                            '****************** NOTE:  we need to provide more information to the user about this problem. ********************
                        Else
                            'Check if the stop has already been assigned miles
                            strMatchedStreet = oMatch(0).Street & oMatch(0).City & oMatch(0).State & oMatch(0).Zip
                            If Not strMatchedStops Is Nothing AndAlso strMatchedStops.Count > 0 AndAlso strMatchedStops.Contains(strMatchedStreet) Then
                                'a duplicate is found so all costs and miles are zero
                                oPCMStop.StopNumber = oMatch(0).StopNumber + 1
                                oPCMStop.LegMiles = 0
                                oPCMStop.TotalMiles = oMatch(0).TotalMiles
                                oPCMStop.LegCost = 0
                                oPCMStop.TotalCost = oMatch(0).TotalCost
                                oPCMStop.LegTime = ""
                                oPCMStop.TotalTime = oMatch(0).TotalTime
                                oPCMStop.Matched = True
                            Else
                                oPCMStop.StopNumber = oMatch(0).StopNumber + 1
                                oPCMStop.LegMiles = oMatch(0).LegMiles
                                oPCMStop.TotalMiles = oMatch(0).TotalMiles
                                oPCMStop.LegCost = oMatch(0).LegCost
                                oPCMStop.TotalCost = oMatch(0).TotalCost
                                oPCMStop.LegTime = oMatch(0).LegTime
                                oPCMStop.TotalTime = oMatch(0).TotalTime
                                oPCMStop.Matched = True
                            End If
                            If strMatchedStops Is Nothing Then strMatchedStops = New List(Of String)
                            strMatchedStops.Add(strMatchedStreet)
                        End If
                    End If
                Next

            Catch ex As System.AccessViolationException
                LogError("Cannot execute PCMReSyncMultiStop: PC Miler is no longer running.", ex)
                oGlobalStopData = Nothing
            Catch ex As Exception
                LogError("Cannot execute PCMReSyncMultiStop. ", ex)
                oGlobalStopData = Nothing
            Finally
                Try
                    PCMSDeleteTrip(intTrip1)
                Catch ex As Exception

                End Try
                gProcessRunning = False
            End Try
            Return oGlobalStopData

        End Function


        ''' <summary>
        ''' Checks the PCMiler error number and returns the message if one exists
        ''' If an Error number is not provided the method will search for the last one
        ''' using PCMSGetError
        ''' </summary>
        ''' <param name="intErr"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR v-7.0.5.100 05/25/2016
        '''   new method checks for a PC Miler Error messages
        ''' </remarks>
        Private Function getPCMError(Optional ByVal intErr As Integer = 0) As String
            Dim buff As StringBuilder = New StringBuilder(255)
            Dim strError As String = ""
            Try
                If intErr = 0 Then intErr = PCMSGetError()
                If intErr <> 0 Then
                    Dim ret = PCMSGetErrorString(intErr, buff, 255)
                    strError = buff.ToString()
                End If
            Catch ex As Exception
                'return an empty string on error this is not a valid PCM error
            End Try

            Return strError

        End Function


        ''' <summary>
        ''' Clears all stops on trip and addes new stops to trip, 
        ''' calls Resequence, optimize and calculte.  Results are returned 
        ''' in oPCMReportRecords and oGlobalStopData.  Returns false on error
        ''' Caller must check LastError property
        ''' </summary>
        ''' <param name="oFMStops"></param>
        ''' <param name="oPCMReportRecords"></param>
        ''' <param name="oGlobalStopData"></param>
        ''' <param name="intTrip1"></param>
        ''' <param name="blnKeepStopNumbers"></param>
        ''' <param name="Reseq_Type"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR v-7.0.5.100 05/25/2016
        '''  created method stup to encapsulte logic for easier 
        '''  coding and readability
        ''' </remarks>
        Private Function buildTrip(ByVal oFMStops As List(Of clsFMStopData),
                                   ByRef oPCMReportRecords As List(Of clsPCMReportRecord),
                                   ByRef oGlobalStopData As clsGlobalStopData,
                                   ByVal intTrip1 As Integer,
                                   ByVal blnKeepStopNumbers As Boolean,
                                   Optional ByVal Reseq_Type As Integer = 1) As Boolean

            Dim Ret As Integer = 0
            Dim blnSuccess As Boolean = True
            'clear all stops and reload
            Ret = PCMSClearStops(intTrip1)
            'We use a for to loop to be sure we process the data in the correct order
            For intStops As Integer = 0 To oFMStops.Count - 1
                'loop through each stop data record
                Dim oStop As clsFMStopData = oFMStops(intStops)
                oStop.SeqNumber = intStops
                'Add the Stop
                AddStopEx(intTrip1, oStop, oGlobalStopData)
            Next
            'test for bad zips if any exist we cannot continue
            If Trim(oGlobalStopData.FailedAddressMessage) <> "" Then
                _strLastError &= oGlobalStopData.FailedAddressMessage & vbCrLf & "The requested operation has been canceled!"
                Return False
            End If
            'Determine if we need to optimize the route (resync)
            If Not blnKeepStopNumbers Then
                'resequence  and optimize the stops
                PCMSSetResequence(intTrip1, Reseq_Type)
                Ret = PCMSOptimize(intTrip1)
            End If
            'Calculate the miles
            Ret = PCMSCalculate(intTrip1)
            'We now store the report data in a list that can be referenced by the caller.
            oPCMReportRecords = getPCMSReport(intTrip1)
            Return True
        End Function

        ''' <summary>
        ''' This method is used to read the results from PC Miler and return them
        ''' as a list of clsPCMReportRecord objects
        ''' </summary>
        ''' <param name="intTrip1"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function getPCMSReport(ByVal intTrip1 As Integer) As List(Of clsPCMReportRecord)
            Dim lines As Integer = PCMSNumRptLines(intTrip1, RPT_MILEAGE)
            Dim oReports As New List(Of clsPCMReportRecord)
            '**** PCMSGetRptLine Record Layout for PC Miler 24 Tab delimited record ****'
            'NOTE tabs are represented by the |  this value does not actually exist in the data; also additional spaces have been
            'added arround the | to make it easier to read. Tolls are also possible if purchased they would show up betweed Total Time and Leg Est CHG
            'If a valid street address is used (10 columns):
            '  City, State |         Street         |Leg Miles|Total Miles|Leg  Cost|Total Cost|Leg Time|Total Time|Leg EST CHG|Total EST CHG|
            'Grandview, WA | 171 North Forsell Road | 1064.5  |   1792.8  | 1266.39 |  2245.31 |  16:51 |   30:26  |   3820.3  |    6633.3   |
            'If defaulting to zip code (10 columns)
            '  Zip |  City, State, County  |Leg Miles|Total Miles|Leg  Cost|Total Cost|Leg Time|Total Time|Leg EST CHG|Total EST CHG|
            '98930 | Grandview, WA, Yakima |  1040.0 |  1784.2   | 1279.02 |  2238.52 |  17:04 |  30:23   |  3847.8   |   6601.5    |
            Dim intSeqNbr As Integer = -1
            Dim shtStopCT As Short = -1
            Dim blnSkip As Boolean
            Dim shtPos As Short
            Dim shtFieldCt As Short
            Dim strCityState() As String
            Dim buff As StringBuilder = New StringBuilder(255)
            Dim strDupStreets As New List(Of String)
            Dim strPreviousStop As String
            Dim strCurrentStop As String
            For i As Integer = 0 To lines - 1
                intSeqNbr += 1
                Dim oRecord As New clsPCMReportRecord
                shtStopCT += 1
                blnSkip = False
                Dim Ret = PCMSGetRptLine(intTrip1, RPT_MILEAGE, i, buff, 254)
                shtPos = 1
                Dim strings() As String
                strings = Split(buff.ToString, Chr(9))
                shtFieldCt = strings.Length
                'Leg Miles
                'Total Miles
                'Leg Cost
                'Total Cost
                'Leg Hours
                'Total Hours
                'Leg Tolls
                'Total Tolls
                'Leg Est. GHG
                'Total Est. CHG

                If shtFieldCt = 7 Then
                    With oRecord
                        .TotalTime = strings(shtFieldCt)
                        .LegTime = strings(shtFieldCt - 1)
                        .TotalCost = Val(strings(shtFieldCt - 2))
                        .LegCost = Val(strings(shtFieldCt - 3))
                        .TotalMiles = Val(strings(shtFieldCt - 4))
                        .LegMiles = Val(strings(shtFieldCt - 5))
                        If InStr(1, strings(0), ",") Then
                            'the first field is a city state so we use the street address as a match
                            strCityState = Split(strings(0), ",")
                            .Zip = ""
                            .Street = Trim(simpleStreetScrubber(strings(1)))
                            .StopName = Trim(strings(0))
                        Else
                            'the first field is the zip code and the second is the city and state
                            .StopName = Trim(strings(1))
                            strCityState = Split(strings(1), ",")
                            .Zip = Trim(strings(0))
                            .Street = ""
                        End If
                        .City = Trim(strCityState(0))
                        .State = Trim(strCityState(1))
                    End With
                ElseIf shtFieldCt = 6 Then
                    With oRecord
                        .TotalTime = strings(shtFieldCt)
                        .LegTime = strings(shtFieldCt - 1)
                        .TotalCost = Val(strings(shtFieldCt - 2))
                        .LegCost = Val(strings(shtFieldCt - 3))
                        .TotalMiles = Val(strings(shtFieldCt - 4))
                        .LegMiles = Val(strings(shtFieldCt - 5))
                        'only the zip code is provided
                        .Zip = Trim(strings(0))
                        .Street = ""
                        .City = ""
                        .State = ""
                        .StopName = Trim(strings(0))
                    End With
                Else
                    '  PC Miler 24
                    If shtFieldCt > 7 Then
                        With oRecord
                            .TotalTime = strings(7)
                            .LegTime = strings(6)
                            .TotalCost = Val(strings(5))
                            .LegCost = Val(strings(4))
                            .TotalMiles = Val(strings(3))
                            .LegMiles = Val(strings(2))
                            If InStr(1, strings(0), ",") Then
                                'the first field is a city state so we use the street address as a match
                                strCityState = Split(strings(0), ",")
                                .Zip = ""
                                .Street = Trim(simpleStreetScrubber(strings(1)))
                                .StopName = Trim(strings(0))
                            Else
                                'the first field is the zip code and the second is the city and state
                                .StopName = Trim(strings(1))
                                strCityState = Split(strings(1), ",")
                                .Zip = Trim(strings(0))
                                .Street = ""
                            End If
                            .City = Trim(strCityState(0))
                            .State = Trim(strCityState(1))
                        End With
                    Else
                        blnSkip = True
                    End If
                End If
                strCurrentStop = oRecord.Street & oRecord.City & oRecord.State & oRecord.Zip
                'check ifthis stop has already been added and reverse the stop counter if needed
                If Not blnSkip Then
                    If Not String.IsNullOrEmpty(strPreviousStop) And strPreviousStop = strCurrentStop Then shtStopCT += -1
                    oRecord.SeqNumber = intSeqNbr
                    oRecord.StopNumber = shtStopCT
                    strPreviousStop = strCurrentStop
                    oReports.Add(oRecord)
                End If
            Next i
            Return oReports
        End Function

#End Region
    End Class

End Namespace
