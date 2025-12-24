Imports System
Imports NGL.IntegrationServices.Utilities
Imports NGL.IntegrationServices.NglLaneWebService

Public Class StandardLaneHeaders
    Implements ILanes

    Private _Errors As String = ""
    Public Property Errors() As String Implements ILanes.Errors
        Get
            Return _Errors
        End Get
        Set(ByVal value As String)
            _Errors = value
        End Set
    End Property

    Private _Items As List(Of Object)
    Public Property Items() As List(Of Object) Implements ILanes.Items
        Get
            Return _Items
        End Get
        Set(ByVal value As List(Of Object))
            _Items = value
        End Set
    End Property

    Public Function ReadFromFile(ByVal fileName As String) As List(Of Object) Implements ILanes.ReadFromFile
        Items = New List(Of Object)
        Try

            Dim strData As String = ""
            Dim errMsg As String = ""
            Dim HRecords() As String = splitLines(readFileToEnd(fileName, errMsg))
            If Not HRecords Is Nothing AndAlso HRecords.Count > 0 Then
                For Each r In HRecords
                    Try
                        If r Is Nothing OrElse r.Trim.Length = 0 Then Continue For
                        Dim strRow() As String = DecodeCSV(r)
                        If strRow.Length >= 55 Then
                            Dim H As New StandardLaneHeader
                            With H

                                .LaneNumber = strRow(0)
                                .LaneName = strRow(1)
                                .LaneNumberMaster = strRow(2)
                                .LaneNameMaster = strRow(3)
                                .LaneCompNumber = strRow(4)
                                .LaneDefaultCarrierUse = strRow(5)
                                .LaneDefaultCarrierNumber = strRow(6)
                                .LaneOrigCompNumber = strRow(7)
                                .LaneOrigName = strRow(8)
                                .LaneOrigAddress1 = strRow(9)
                                .LaneOrigAddress2 = strRow(10)
                                .LaneOrigAddress3 = strRow(11)
                                .LaneOrigCity = strRow(12)
                                .LaneOrigState = strRow(13)
                                .LaneOrigCountry = strRow(14)
                                .LaneOrigZip = strRow(15)
                                .LaneOrigContactPhone = strRow(16)
                                .LaneOrigContactPhoneExt = strRow(17)
                                .LaneOrigContactFax = strRow(18)
                                .LaneDestCompNumber = strRow(19)
                                .LaneDestName = strRow(20)
                                .LaneDestAddress1 = strRow(21)
                                .LaneDestAddress2 = strRow(22)
                                .LaneDestAddress3 = strRow(23)
                                .LaneDestCity = strRow(24)
                                .LaneDestState = strRow(25)
                                .LaneDestCountry = strRow(26)
                                .LaneDestZip = strRow(27)
                                .LaneDestContactPhone = strRow(28)
                                .LaneDestContactPhoneExt = strRow(29)
                                .LaneDestContactFax = strRow(30)
                                .LaneConsigneeNumber = strRow(31)
                                .LaneRecMinIn = strRow(32)
                                .LaneRecMinUnload = strRow(33)
                                .LaneRecMinOut = strRow(34)
                                .LaneAppt = strRow(35)
                                .LanePalletExchange = strRow(36)
                                .LanePalletType = strRow(37)
                                .LaneBenchMiles = strRow(38)
                                .LaneBFC = strRow(39)
                                .LaneBFCType = strRow(40)
                                .LaneRecHourStart = strRow(41)
                                .LaneRecHourStop = strRow(42)
                                .LaneDestHourStart = strRow(43)
                                .LaneDestHourStop = strRow(44)
                                .LaneComments = strRow(45)
                                .LaneCommentsConfidential = strRow(46)
                                .LaneLatitude = strRow(47)
                                .LaneLongitude = strRow(48)
                                .LaneTempType = strRow(49)
                                .LaneTransType = strRow(50)
                                .LanePrimaryBuyer = strRow(51)
                                .LaneAptDelivery = strRow(52)
                                .BrokerNumber = strRow(53)
                                .BrokerName = strRow(54)


                            End With

                            Items.Add(H)
                        Else
                            Errors &= "StandardLaneHeaders.ReadFromFile Error; invalid row not enough columns:" & vbCrLf & "     " & r & vbCrLf & vbCrLf
                        End If
                    Catch ex As ApplicationException
                        Errors &= "StandardLaneHeaders.ReadFromFile parse CSV Error: " & ex.Message & vbCrLf & vbCrLf
                    End Try
                Next
            Else
                Errors &= errMsg
            End If

        Catch ex As Exception
            Errors &= "StandardLaneHeaders.ReadFromFile Unexpected Error: " & ex.Message & vbCrLf & vbCrLf
        End Try
        Return Items

    End Function

    ''' <summary>
    ''' Throws unhandled exceptions
    ''' </summary> 
    ''' <param name="ErrMsg"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function buildLaneObjects(ByRef ErrMsg As String) As List(Of clsLaneObject) Implements ILanes.buildLaneObjects
        Dim blnRet As Boolean = False
        Dim lanes As New List(Of clsLaneObject)
        Try 
            For Each H As StandardLaneHeader In Items

                Dim oH As New clsLaneObject
                With oH

                    .LaneNumber = H.LaneNumber
                    .LaneName = H.LaneName
                    .LaneNumberMaster = H.LaneNumberMaster
                    .LaneNameMaster = H.LaneNameMaster
                    .LaneCompNumber = H.LaneCompNumber
                    Boolean.TryParse(H.LaneDefaultCarrierUse, .LaneDefaultCarrierUse)
                    Integer.TryParse(H.LaneDefaultCarrierNumber, .LaneDefaultCarrierNumber)
                    .LaneOrigCompNumber = H.LaneOrigCompNumber
                    .LaneOrigName = H.LaneOrigName
                    .LaneOrigAddress1 = H.LaneOrigAddress1
                    .LaneOrigAddress2 = H.LaneOrigAddress2
                    .LaneOrigAddress3 = H.LaneOrigAddress3
                    .LaneOrigCity = H.LaneOrigCity
                    .LaneOrigState = H.LaneOrigState
                    .LaneOrigCountry = H.LaneOrigCountry
                    .LaneOrigZip = H.LaneOrigZip
                    .LaneOrigContactPhone = H.LaneOrigContactPhone
                    .LaneOrigContactPhoneExt = H.LaneOrigContactPhoneExt
                    .LaneOrigContactFax = H.LaneOrigContactFax
                    .LaneDestCompNumber = H.LaneDestCompNumber
                    .LaneDestName = H.LaneDestName
                    .LaneDestAddress1 = H.LaneDestAddress1
                    .LaneDestAddress2 = H.LaneDestAddress2
                    .LaneDestAddress3 = H.LaneDestAddress3
                    .LaneDestCity = H.LaneDestCity
                    .LaneDestState = H.LaneDestState
                    .LaneDestCountry = H.LaneDestCountry
                    .LaneDestZip = H.LaneDestZip
                    .LaneDestContactPhone = H.LaneDestContactPhone
                    .LaneDestContactPhoneExt = H.LaneDestContactPhoneExt
                    .LaneDestContactFax = H.LaneDestContactFax
                    .LaneConsigneeNumber = H.LaneConsigneeNumber
                    Integer.TryParse(H.LaneRecMinIn, .LaneRecMinIn)
                    Integer.TryParse(H.LaneRecMinUnload, .LaneRecMinUnload)
                    Integer.TryParse(H.LaneRecMinOut, .LaneRecMinOut)
                    Boolean.TryParse(H.LaneAppt, .LaneAppt)
                    Boolean.TryParse(H.LanePalletExchange, .LanePalletExchange)
                    .LanePalletType = H.LanePalletType
                    Integer.TryParse(H.LaneBenchMiles, .LaneBenchMiles)
                    Double.TryParse(H.LaneBFC, .LaneBFC)
                    .LaneBFCType = H.LaneBFCType
                    .LaneRecHourStart = H.LaneRecHourStart
                    .LaneRecHourStop = H.LaneRecHourStop
                    .LaneDestHourStart = H.LaneDestHourStart
                    .LaneDestHourStop = H.LaneDestHourStop
                    .LaneComments = H.LaneComments
                    .LaneCommentsConfidential = H.LaneCommentsConfidential
                    Double.TryParse(H.LaneLatitude, .LaneLatitude)
                    Double.TryParse(H.LaneLongitude, .LaneLongitude)
                    Short.TryParse(H.LaneTempType, .LaneTempType)
                    Short.TryParse(H.LaneTransType, .LaneTransType)
                    .LanePrimaryBuyer = H.LanePrimaryBuyer
                    Boolean.TryParse(H.LaneAptDelivery, .LaneAptDelivery)
                    .BrokerNumber = H.BrokerNumber
                    .BrokerName = H.BrokerName
                     
                End With

                lanes.Add(oH)

            Next
            blnRet = True
        Catch ex As Exception
            Throw
        End Try
        Return lanes
    End Function
     
    Public Overrides Function ToString() As String
        Dim strRet As String = ""
        For Each H In Items
            strRet &= H.ToString & vbCrLf & vbCrLf
        Next
        Return strRet
    End Function

End Class

Public Class StandardLaneHeader

    Friend LaneNumber As String
    Friend LaneName As String
    Friend LaneNumberMaster As String
    Friend LaneNameMaster As String
    Friend LaneCompNumber As String
    Friend LaneDefaultCarrierUse As String
    Friend LaneDefaultCarrierNumber As String
    Friend LaneOrigCompNumber As String
    Friend LaneOrigName As String
    Friend LaneOrigAddress1 As String
    Friend LaneOrigAddress2 As String
    Friend LaneOrigAddress3 As String
    Friend LaneOrigCity As String
    Friend LaneOrigState As String
    Friend LaneOrigCountry As String
    Friend LaneOrigZip As String
    Friend LaneOrigContactPhone As String
    Friend LaneOrigContactPhoneExt As String
    Friend LaneOrigContactFax As String
    Friend LaneDestCompNumber As String
    Friend LaneDestName As String
    Friend LaneDestAddress1 As String
    Friend LaneDestAddress2 As String
    Friend LaneDestAddress3 As String
    Friend LaneDestCity As String
    Friend LaneDestState As String
    Friend LaneDestCountry As String
    Friend LaneDestZip As String
    Friend LaneDestContactPhone As String
    Friend LaneDestContactPhoneExt As String
    Friend LaneDestContactFax As String
    Friend LaneConsigneeNumber As String
    Friend LaneRecMinIn As String
    Friend LaneRecMinUnload As String
    Friend LaneRecMinOut As String
    Friend LaneAppt As String
    Friend LanePalletExchange As String
    Friend LanePalletType As String
    Friend LaneBenchMiles As String
    Friend LaneBFC As String
    Friend LaneBFCType As String
    Friend LaneRecHourStart As String
    Friend LaneRecHourStop As String
    Friend LaneDestHourStart As String
    Friend LaneDestHourStop As String
    Friend LaneComments As String
    Friend LaneCommentsConfidential As String
    Friend LaneLatitude As String
    Friend LaneLongitude As String
    Friend LaneTempType As String
    Friend LaneTransType As String
    Friend LanePrimaryBuyer As String
    Friend LaneAptDelivery As String
    Friend BrokerNumber As String
    Friend BrokerName As String




End Class
