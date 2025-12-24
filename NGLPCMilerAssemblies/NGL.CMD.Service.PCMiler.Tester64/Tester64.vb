Module Tester64


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

    Dim strLastError As String = ""
    Dim gLastError As String = ""
    Dim Debug As Boolean = True
    Dim KeepLogDays As Integer = 1
    Dim SaveOldLog As Boolean = False
    Dim UseZipOnly As Boolean = False

    Sub Main()


        Try
            Dim BaddAddresses As clsPCMBadAddresses
            Dim oRet As New clsAllStops
            oRet = getPracticalMiles(New clsAddress With {.strZip = "60611"}, _
                                    New clsAddress With {.strZip = "37726"}, _
                                    PCMEX_Route_Type.ROUTE_TYPE_PRACTICAL, _
                                   PCMEX_Dist_Type.DIST_TYPE_MILES, _
                                   0, _
                                    0, _
                                    0, _
                                    "SO1234", _
                                    "NA", _
                                    0, _
                                    "123456789", _
                                    False, _
                                    BaddAddresses, _
                                    False, _
                                    "")

            If Not oRet Is Nothing Then
                Console.WriteLine(oRet.TotalMiles)
            Else
                Console.WriteLine("Nothing")
            End If

            If Not String.IsNullOrEmpty(strLastError) Then
                Console.WriteLine(strLastError)
            End If
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        Finally
            Console.WriteLine("Press Enter To Continue...")
            Console.ReadLine()
        End Try

    End Sub



    Function getPracticalMiles(ByVal objOrig As clsAddress, _
                                ByVal objDest As clsAddress, _
                                ByVal Route_Type As PCMEX_Route_Type, _
                                ByVal Dist_Type As PCMEX_Dist_Type, _
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


        Dim oclsAllStops As clsAllStops = Nothing
        Dim oPCM As New NGL.Service.PCMiler64.PCMiles With {.Debug = Debug, _
                                                            .LoggingOn = LoggingOn, _
                                                            .KeepLogDays = KeepLogDays, _
                                                            .SaveOldLog = SaveOldLog, _
                                                            .LogFileName = LogFileName, _
                                                            .UseZipOnly = UseZipOnly}

        strLastError = ""
        gLastError = ""
        Try

            Dim strLastError As String = ""
            Dim arrBadAddresses() As NGL.Service.PCMiler64.clsPCMBadAddress
            Dim oOrig As New NGL.Service.PCMiler64.clsAddress With {.strAddress = objOrig.strAddress, _
                                                                    .strCity = objOrig.strCity, _
                                                                    .strState = objOrig.strState, _
                                                                    .strZip = objOrig.strZip}

            Dim oDest As New NGL.Service.PCMiler64.clsAddress With {.strAddress = objDest.strAddress, _
                                                                    .strCity = objDest.strCity, _
                                                                    .strState = objDest.strState, _
                                                                    .strZip = objDest.strZip}


            Dim oGlobalStopData As NGL.Service.PCMiler64.clsGlobalStopData = oPCM.getPracticalMiles(oOrig, _
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
                                        arrBadAddresses)
            strLastError = oPCM.LastError

            If Not arrBadAddresses Is Nothing AndAlso arrBadAddresses.Length > 0 Then
                For i As Integer = 0 To arrBadAddresses.Length - 1
                    Dim oBadOrig As New clsAddress With {.strAddress = arrBadAddresses(i).objOrig.strAddress, _
                                                         .strCity = arrBadAddresses(i).objOrig.strCity, _
                                                         .strState = arrBadAddresses(i).objOrig.strState, _
                                                         .strZip = arrBadAddresses(i).objOrig.strZip}
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
                    If BaddAddresses Is Nothing Then BaddAddresses = New clsPCMBadAddresses
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

        Catch ex As Exception
            strLastError = ex.Message
        End Try
        Return oclsAllStops

    End Function


End Module
