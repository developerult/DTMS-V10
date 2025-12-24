Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class AMSOrderList
        Inherits DTOBaseClass

#Region " Data Members"


        Private _BookControl As Integer = 0
        <DataMember()> _
        Public Property BookControl() As Integer
            Get
                Return _BookControl
            End Get
            Set(ByVal value As Integer)
                _BookControl = value
            End Set
        End Property

        Private _BookAMSPickupApptControl As Integer = 0
        <DataMember()> _
        Public Property BookAMSPickupApptControl As Integer
            Get
                Return _BookAMSPickupApptControl
            End Get
            Set(value As Integer)
                _BookAMSPickupApptControl = value
            End Set
        End Property

        Private _BookAMSDeliveryApptControl As Integer = 0
        <DataMember()> _
        Public Property BookAMSDeliveryApptControl As Integer
            Get
                Return _BookAMSDeliveryApptControl
            End Get
            Set(value As Integer)
                _BookAMSDeliveryApptControl = value
            End Set
        End Property

        Private _BookItemDetailDescription As String
        <DataMember()> _
        Public Property BookItemDetailDescription As String
            Get
                Return Left(_BookItemDetailDescription, 4000)
            End Get
            Set(value As String)
                _BookItemDetailDescription = Left(value, 4000)
            End Set
        End Property

        Private _BookCustCompControl As Integer = 0
        <DataMember()> _
        Public Property BookCustCompControl() As Integer
            Get
                Return _BookCustCompControl
            End Get
            Set(ByVal value As Integer)
                _BookCustCompControl = value
            End Set
        End Property

        Private _AMSCompControl As Integer = 0
        <DataMember()> _
        Public Property AMSCompControl() As Integer
            Get
                Return _AMSCompControl
            End Get
            Set(ByVal value As Integer)
                _AMSCompControl = value
            End Set
        End Property

        Private _BookCarrierControl As Integer = 0
        <DataMember()> _
        Public Property BookCarrierControl() As Integer
            Get
                Return _BookCarrierControl
            End Get
            Set(ByVal value As Integer)
                _BookCarrierControl = value
            End Set
        End Property

        Private _BookCarrierContact As String = ""
        <DataMember()> _
        Public Property BookCarrierContact() As String
            Get
                Return Left(_BookCarrierContact, 30)
            End Get
            Set(ByVal value As String)
                _BookCarrierContact = Left(value, 30)
            End Set
        End Property

        Private _BookCarrierContactPhone As String = ""
        <DataMember()> _
        Public Property BookCarrierContactPhone() As String
            Get
                Return Left(_BookCarrierContactPhone, 20)
            End Get
            Set(ByVal value As String)
                _BookCarrierContactPhone = Left(value, 20)
            End Set
        End Property

        Private _BookProNumber As String = ""
        <DataMember()> _
        Public Property BookProNumber() As String
            Get
                Return Left(_BookProNumber, 20)
            End Get
            Set(ByVal value As String)
                _BookProNumber = Left(value, 20)
            End Set
        End Property

        Private _BookConsPrefix As String = ""
        <DataMember()> _
        Public Property BookConsPrefix() As String
            Get
                Return Left(_BookConsPrefix, 20)
            End Get
            Set(ByVal value As String)
                _BookConsPrefix = Left(value, 20)
            End Set
        End Property

        Private _NotUsedBookCarrOrderNumberSeq As String
        <DataMember()> _
        Public Property BookCarrOrderNumberSeq() As String
            Get
                Return Me.BookCarrOrderNumber & "-" & Me.BookOrderSequence.ToString
            End Get
            Set(ByVal value As String)
                _NotUsedBookCarrOrderNumberSeq = value
            End Set
        End Property

        Private _BookOrderSequence As Integer = 0
        <DataMember()> _
        Public Property BookOrderSequence() As Integer
            Get
                Return _BookOrderSequence
            End Get
            Set(ByVal value As Integer)
                _BookOrderSequence = value
            End Set
        End Property

        Private _BookCarrOrderNumber As String = ""
        <DataMember()> _
        Public Property BookCarrOrderNumber() As String
            Get
                Return _BookCarrOrderNumber
            End Get
            Set(ByVal value As String)
                _BookCarrOrderNumber = value
            End Set
        End Property

        Private _BookLoadPONumber As String = ""
        <DataMember()> _
        Public Property BookLoadPONumber() As String
            Get
                Return _BookLoadPONumber
            End Get
            Set(ByVal value As String)
                _BookLoadPONumber = value
            End Set
        End Property

        Private _BookLoadControl As Integer
        <DataMember()> _
        Public Property BookLoadControl() As Integer
            Get
                Return _BookLoadControl
            End Get
            Set(ByVal value As Integer)
                _BookLoadControl = value
            End Set
        End Property

        Private _BookOrigCompControl As System.Nullable(Of Integer)
        <DataMember()> _
        Public Property BookOrigCompControl() As System.Nullable(Of Integer)
            Get
                Return _BookOrigCompControl
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                _BookOrigCompControl = value
            End Set
        End Property

        Private _BookOrigName As String = ""
        <DataMember()> _
        Public Property BookOrigName() As String
            Get
                Return Left(_BookOrigName, 40)
            End Get
            Set(ByVal value As String)
                _BookOrigName = Left(value, 40)
            End Set
        End Property

        Private _BookOrigAddress1 As String = ""
        <DataMember()> _
        Public Property BookOrigAddress1() As String
            Get
                Return Left(_BookOrigAddress1, 40)
            End Get
            Set(value As String)
                If (String.Equals(Me._BookOrigAddress1, value) = False) Then
                    Me._BookOrigAddress1 = Left(value, 40)
                End If
            End Set
        End Property

        Private _BookOrigCity As String = ""
        <DataMember()> _
        Public Property BookOrigCity() As String
            Get
                Return Left(_BookOrigCity, 25)
            End Get
            Set(value As String)
                If (String.Equals(Me._BookOrigCity, value) = False) Then
                    Me._BookOrigCity = Left(value, 25)
                End If
            End Set
        End Property

        Private _BookOrigState As String = ""
        <DataMember()> _
        Public Property BookOrigState() As String
            Get
                Return Left(Me._BookOrigState, 2)
            End Get
            Set(value As String)
                If (String.Equals(Me._BookOrigState, value) = False) Then
                    Me._BookOrigState = Left(value, 2)
                End If
            End Set
        End Property

        Private _BookOrigCountry As String = ""
        <DataMember()> _
        Public Property BookOrigCountry() As String
            Get
                Return Left(Me._BookOrigCountry, 30)
            End Get
            Set(value As String)
                If (String.Equals(Me._BookOrigCountry, value) = False) Then
                    Me._BookOrigCountry = Left(value, 30)
                End If
            End Set
        End Property

        Private _BookOrigZip As String = ""
        <DataMember()> _
        Public Property BookOrigZip() As String
            Get
                Return Left(Me._BookOrigZip, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
            End Get
            Set(value As String)
                If (String.Equals(Me._BookOrigZip, value) = False) Then
                    Me._BookOrigZip = Left(value, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
                End If
            End Set
        End Property

        Private _BookOrigPhone As String = ""
        <DataMember()> _
        Public Property BookOrigPhone() As String
            Get
                Return Left(Me._BookOrigPhone, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
            End Get
            Set(value As String)
                If (String.Equals(Me._BookOrigPhone, value) = False) Then
                    Me._BookOrigPhone = Left(value, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
                End If
            End Set
        End Property


        Private _BookDestCompControl As System.Nullable(Of Integer)
        <DataMember()> _
        Public Property BookDestCompControl() As System.Nullable(Of Integer)
            Get
                Return _BookDestCompControl
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                _BookDestCompControl = value
            End Set
        End Property

        Private _BookDestName As String = ""
        <DataMember()> _
        Public Property BookDestName() As String
            Get
                Return Left(_BookDestName, 40)
            End Get
            Set(ByVal value As String)
                _BookDestName = Left(value, 40)
            End Set
        End Property

        Private _BookDestAddress1 As String = ""
        <DataMember()> _
        Public Property BookDestAddress1() As String
            Get
                Return Left(_BookDestAddress1, 40)
            End Get
            Set(value As String)
                If (String.Equals(Me._BookDestAddress1, value) = False) Then
                    Me._BookDestAddress1 = Left(value, 40)
                End If
            End Set
        End Property

        Private _BookDestCity As String = ""
        <DataMember()> _
        Public Property BookDestCity() As String
            Get
                Return Left(_BookDestCity, 25)
            End Get
            Set(value As String)
                If (String.Equals(Me._BookDestCity, value) = False) Then
                    Me._BookDestCity = Left(value, 25)
                End If
            End Set
        End Property

        Private _BookDestState As String = ""
        <DataMember()> _
        Public Property BookDestState() As String
            Get
                Return Left(Me._BookDestState, 2)
            End Get
            Set(value As String)
                If (String.Equals(Me._BookDestState, value) = False) Then
                    Me._BookDestState = Left(value, 2)
                End If
            End Set
        End Property

        Private _BookDestCountry As String = ""
        <DataMember()> _
        Public Property BookDestCountry() As String
            Get
                Return Left(Me._BookDestCountry, 30)
            End Get
            Set(value As String)
                If (String.Equals(Me._BookDestCountry, value) = False) Then
                    Me._BookDestCountry = Left(value, 30)
                End If
            End Set
        End Property

        Private _BookDestZip As String = ""
        <DataMember()> _
        Public Property BookDestZip() As String
            Get
                Return Left(Me._BookDestZip, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
            End Get
            Set(value As String)
                If (String.Equals(Me._BookDestZip, value) = False) Then
                    Me._BookDestZip = Left(value, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
                End If
            End Set
        End Property

        Private _BookDestPhone As String = ""
        <DataMember()> _
        Public Property BookDestPhone() As String
            Get
                Return Left(Me._BookDestPhone, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
            End Get
            Set(value As String)
                If (String.Equals(Me._BookDestPhone, value) = False) Then
                    Me._BookDestPhone = Left(value, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
                End If
            End Set
        End Property

        Private _BookDateOrdered As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookDateOrdered() As System.Nullable(Of Date)
            Get
                Return _BookDateOrdered
            End Get
            Set(value As System.Nullable(Of Date))
                _BookDateOrdered = value
            End Set
        End Property

        Private _BookDateLoad As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookDateLoad() As System.Nullable(Of Date)
            Get
                Return _BookDateLoad
            End Get
            Set(value As System.Nullable(Of Date))
                _BookDateLoad = value
            End Set
        End Property

        Private _BookDateRequired As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookDateRequired() As System.Nullable(Of Date)
            Get
                Return _BookDateRequired
            End Get
            Set(value As System.Nullable(Of Date))
                _BookDateRequired = value
            End Set
        End Property

        Private _BookTotalCases As System.Nullable(Of Integer)
        <DataMember()> _
        Public Property BookTotalCases() As System.Nullable(Of Integer)
            Get
                Return _BookTotalCases
            End Get
            Set(value As System.Nullable(Of Integer))
                _BookTotalCases = value
            End Set
        End Property

        Private _BookTotalWgt As System.Nullable(Of Double)
        <DataMember()> _
        Public Property BookTotalWgt() As System.Nullable(Of Double)
            Get
                Return _BookTotalWgt
            End Get
            Set(value As System.Nullable(Of Double))
                _BookTotalWgt = value
            End Set
        End Property

        Private _BookTotalPL As System.Nullable(Of Double)
        <DataMember()> _
        Public Property BookTotalPL() As System.Nullable(Of Double)
            Get
                Return _BookTotalPL
            End Get
            Set(value As System.Nullable(Of Double))
                _BookTotalPL = value
            End Set
        End Property

        Private _BookTotalCube As System.Nullable(Of Double)
        <DataMember()> _
        Public Property BookTotalCube() As System.Nullable(Of Double)
            Get
                Return _BookTotalCube
            End Get
            Set(value As System.Nullable(Of Double))
                _BookTotalCube = value
            End Set
        End Property

        Private _BookTotalPX As System.Nullable(Of Integer)
        <DataMember()> _
        Public Property BookTotalPX() As System.Nullable(Of Integer)
            Get
                Return _BookTotalPX
            End Get
            Set(value As System.Nullable(Of Integer))
                _BookTotalPX = value
            End Set
        End Property

        Private _BookStopNo As System.Nullable(Of Integer)
        <DataMember()> _
        Public Property BookStopNo() As System.Nullable(Of Integer)
            Get
                Return _BookStopNo
            End Get
            Set(value As System.Nullable(Of Integer))
                _BookStopNo = value
            End Set
        End Property

        Private _BookRouteConsFlag As Boolean
        <DataMember()> _
        Public Property BookRouteConsFlag() As Boolean
            Get
                Return _BookRouteConsFlag
            End Get
            Set(value As Boolean)
                _BookRouteConsFlag = value
            End Set
        End Property

        Private _BookODControl As Integer = 0
        <DataMember()> _
        Public Property BookODControl() As Integer
            Get
                Return _BookODControl
            End Get
            Set(ByVal value As Integer)
                _BookODControl = value
            End Set
        End Property

        Private _BookShipCarrierProNumber As String = ""
        <DataMember()> _
        Public Property BookShipCarrierProNumber() As String
            Get
                Return Left(_BookShipCarrierProNumber, 20)
            End Get
            Set(ByVal value As String)
                _BookShipCarrierProNumber = Left(value, 20)
            End Set
        End Property

        Private _BookShipCarrierProNumberRaw As String = ""
        <DataMember()> _
        Public Property BookShipCarrierProNumberRaw() As String
            Get
                Return Left(_BookShipCarrierProNumberRaw, 20)
            End Get
            Set(ByVal value As String)
                _BookShipCarrierProNumberRaw = Left(value, 20)
            End Set
        End Property

        Private _BookShipCarrierProControl As System.Nullable(Of Integer)
        <DataMember()> _
        Public Property BookShipCarrierProControl() As System.Nullable(Of Integer)
            Get
                Return _BookShipCarrierProControl
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                _BookShipCarrierProControl = value
            End Set
        End Property

        Private _BookShipCarrierName As String = ""
        <DataMember()> _
        Public Property BookShipCarrierName() As String
            Get
                Return Left(_BookShipCarrierName, 60)
            End Get
            Set(ByVal value As String)
                _BookShipCarrierName = Left(value, 60)
            End Set
        End Property

        Private _BookShipCarrierNumber As String = ""
        <DataMember()> _
        Public Property BookShipCarrierNumber() As String
            Get
                Return Left(_BookShipCarrierNumber, 80)
            End Get
            Set(ByVal value As String)
                _BookShipCarrierNumber = Left(value, 80)
            End Set
        End Property

        Private _CarrierNumber As Integer = 0
        <DataMember()> _
        Public Property CarrierNumber() As Integer
            Get
                Return _CarrierNumber
            End Get
            Set(ByVal value As Integer)
                _CarrierNumber = value
            End Set
        End Property

        Private _CarrierName As String = ""
        <DataMember()> _
        Public Property CarrierName() As String
            Get
                Return Left(_CarrierName, 40)
            End Get
            Set(ByVal value As String)
                _CarrierName = Left(value, 40)
            End Set
        End Property

        Private _CarrierSCAC As String = ""
        <DataMember()> _
        Public Property CarrierSCAC() As String
            Get
                Return Left(_CarrierSCAC, 4)
            End Get
            Set(ByVal value As String)
                _CarrierSCAC = Left(value, 4)
            End Set
        End Property

        Private _LaneNumber As String = ""
        <DataMember()> _
        Public Property LaneNumber() As String
            Get
                Return Left(_LaneNumber, 50)
            End Get
            Set(ByVal value As String)
                _LaneNumber = Left(value, 50)
            End Set
        End Property

        Private _LaneOriginAddressUse As System.Nullable(Of Boolean)
        <DataMember()> _
        Public Property LaneOriginAddressUse() As System.Nullable(Of Boolean)
            Get
                Return _LaneOriginAddressUse
            End Get
            Set(value As System.Nullable(Of Boolean))
                _LaneOriginAddressUse = value
            End Set
        End Property

        ''Private _OrderTypeNotUsed As Integer
        ''''' <summary>
        ''''' order types:
        ''''' 0 = Single Load No CNS Outbound
        ''''' 1 = Consolidated Integrity Off Outbound
        ''''' 2 = Consolidated Load Outbound
        ''''' 3 = Single Load No CNS Inbound
        ''''' 4 = Consolidated Integrity Off Inbound
        ''''' 5 = Consolidated Load Inbound
        ''''' 
        ''''' </summary>
        ''''' <value></value>
        ''''' <returns></returns>
        ''''' <remarks>
        ''''' The query for this data must perform a test on inbound or outbound company data at least one must match the AMS company selected
        ''''' </remarks>
        ''<DataMember()> _
        ''Public Property OrderType() As Integer
        ''    Get
        ''        Dim intOrderType As Integer = 0
        ''        If Me.BookOrigCompControl = AMSCompControl Then
        ''            'OUTBOUND
        ''            If String.IsNullOrEmpty(BookConsPrefix.Trim) OrElse BookConsPrefix = "9999" Then
        ''                'this is a Single Load No CNS Outbound
        ''                intOrderType = 1 '0
        ''            ElseIf BookRouteConsFlag = False Then
        ''                'this is a Consolidated Integrity Off Outbound
        ''                intOrderType = 2 '1
        ''            Else
        ''                intOrderType = 3 '2 ConsolidatedLoadOutbound
        ''            End If
        ''        Else
        ''            'INBOUND
        ''            If String.IsNullOrEmpty(BookConsPrefix.Trim) OrElse BookConsPrefix = "9999" Then
        ''                'this is a Single Load No CNS Inbound
        ''                intOrderType = 4 '3
        ''            ElseIf BookRouteConsFlag = False Then
        ''                'this is a Consolidated Integrity Off Inbound
        ''                intOrderType = 5 '4
        ''            Else
        ''                intOrderType = 6 ' 5 ConsolidatedLoadInbound
        ''            End If
        ''        End If
        ''        Return intOrderType
        ''    End Get
        ''    Set(value As Integer)
        ''        _OrderTypeNotUsed = value
        ''    End Set
        ''End Property

        Private _OrderTypeMsgNotUsed As String
        <DataMember()> _
        Public Property OrderTypeMsg() As String
            Get
                Dim strOrderTypeMsg As String = "MSG_SingleLoadNoCNSOutbound"
                Dim intOrdertype As Integer = OrderType
                Select Case intOrdertype
                    Case 0
                        strOrderTypeMsg = "MSG_SingleLoadNoCNSOutbound"
                    Case 1
                        strOrderTypeMsg = "MSG_ConsolidatedLoadIntegrityOffOutbound"
                    Case 2
                        strOrderTypeMsg = "MSG_ConsolidatedLoadOutbound"
                    Case 3
                        strOrderTypeMsg = "MSG_SingleLoadNoCNSInbound"
                    Case 4
                        strOrderTypeMsg = "MSG_ConsolidatedLoadIntegrityOffInbound"
                    Case Else
                        strOrderTypeMsg = "MSG_ConsolidatedLoadInbound"
                End Select
                Return strOrderTypeMsg
            End Get
            Set(ByVal value As String)
                _OrderTypeMsgNotUsed = value
            End Set
        End Property


        'Added By LVV On 7/19/18 For v-8.3 TMS365 Scheduler
        Private _OrderType As Integer = 0
        <DataMember()>
        Public Property OrderType() As Integer
            Get
                Return _OrderType
            End Get
            Set(ByVal value As Integer)
                _OrderType = value
            End Set
        End Property

        Private _OrderColorCode As String = ""
        <DataMember()>
        Public Property OrderColorCode() As String
            Get
                Return Left(_OrderColorCode, 10)
            End Get
            Set(ByVal value As String)
                _OrderColorCode = Left(value, 10)
            End Set
        End Property

        Private _EquipmentID As String = ""
        <DataMember()>
        Public Property EquipmentID() As String
            Get
                Return Left(_EquipmentID, 50)
            End Get
            Set(ByVal value As String)
                _EquipmentID = Left(value, 50)
            End Set
        End Property

        Private _BookSHID As String = ""
        <DataMember()>
        Public Property BookSHID() As String
            Get
                Return Left(_BookSHID, 50)
            End Get
            Set(ByVal value As String)
                _BookSHID = Left(value, 50)
            End Set
        End Property

#End Region



#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New AMSOrderList
            instance = DirectCast(MemberwiseClone(), AMSOrderList)
            Return instance
        End Function

#End Region
    End Class
End Namespace