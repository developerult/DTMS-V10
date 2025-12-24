

Module GlobalSettings
    Public gServerID As Short = 0
    Public gLastError As String = ""
    Public gProcessRunning As Boolean = False

    Public Sub AddBaddAddressToArray(ByVal BookControl As Integer, _
                            ByVal LaneControl As Integer, _
                            ByVal objOrig As clsAddress, _
                            ByVal objDest As clsAddress, _
                            ByVal objPCMOrig As clsAddress, _
                            ByVal objPCMDest As clsAddress, _
                            ByVal Message As String, _
                            ByVal BatchID As Double, _
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

    Public Sub AddStopToArray(ByVal shtStopNumber As Short, _
                    ByVal strStopName As String, _
                    ByVal strID1 As String, _
                    ByVal strID2 As String, _
                    ByVal strTruckName As String, _
                    ByVal intTruckNumber As Integer, _
                    ByVal shtSeqNbr As Short, _
                    ByVal dblDistToPrev As Double, _
                    ByVal dblTotalRouteCost As Double, _
                    ByVal strConsNumber As String, _
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

    Public Sub AddPCMDataStopToArray(ByVal BookControl As Integer, _
                                ByVal BookCustCompControl As Integer, _
                                ByVal BookLoadControl As Integer, _
                                ByVal BookODControl As Integer, _
                                ByVal BookStopNo As Integer, _
                                ByVal RouteType As Integer, _
                                ByVal DistType As Integer, _
                                ByVal BookOrigZip As String, _
                                ByVal BookDestZip As String, _
                                ByVal BookOrigAddress1 As String, _
                                ByVal BookDestAddress1 As String, _
                                ByVal BookOrigCity As String, _
                                ByVal BookDestCity As String, _
                                ByVal BookOrigState As String, _
                                ByVal BookDestState As String, _
                                ByVal BookProNumber As String, _
                                ByVal LaneOriginAddressUse As Boolean, _
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

End Module
