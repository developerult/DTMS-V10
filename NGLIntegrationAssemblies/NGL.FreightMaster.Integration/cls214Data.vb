Imports NDT = Ngl.Core.Utility.DataTransformation
Imports BLL = Ngl.FM.BLL
Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects
Imports LTS = Ngl.FreightMaster.Data.LTS
Imports DAL = Ngl.FreightMaster.Data


''' <summary>
''' Header information stored in the 214 document shared by all stops
''' </summary>
''' <remarks></remarks>
Public Class cls214LoadData

    Private _SHID As String
    Public Property SHID() As String
        Get
            Return _SHID
        End Get
        Set(ByVal value As String)
            _SHID = value
        End Set
    End Property

    Private _CarrierPartnerCode As String
    Public Property CarrierPartnerCode() As String
        Get
            Return _CarrierPartnerCode
        End Get
        Set(ByVal value As String)
            _CarrierPartnerCode = value
        End Set
    End Property

    Private _CompPartnerCode As String
    Public Property CompPartnerCode() As String
        Get
            Return _CompPartnerCode
        End Get
        Set(ByVal value As String)
            _CompPartnerCode = value
        End Set
    End Property

    Private _ShipCarrierProNumber As String
    Public Property ShipCarrierProNumber() As String
        Get
            Return _ShipCarrierProNumber
        End Get
        Set(ByVal value As String)
            _ShipCarrierProNumber = value
        End Set
    End Property

    Private _ShipCarrierNumber As String
    Public Property ShipCarrierNumber() As String
        Get
            Return _ShipCarrierNumber
        End Get
        Set(ByVal value As String)
            _ShipCarrierNumber = value
        End Set
    End Property

    Private _ShipCarrierName As String
    Public Property ShipCarrierName() As String
        Get
            Return _ShipCarrierName
        End Get
        Set(ByVal value As String)
            _ShipCarrierName = value
        End Set
    End Property

    Private _DefaultLoadStatusCode As Integer = 214
    Public Property DefaultLoadStatusCode() As Integer
        Get
            Return _DefaultLoadStatusCode
        End Get
        Set(ByVal value As Integer)
            _DefaultLoadStatusCode = value
        End Set
    End Property

    Private _DefaultLoadStatusControl As Integer
    Public Property DefaultLoadStatusControl() As Integer
        Get
            Return _DefaultLoadStatusControl
        End Get
        Set(ByVal value As Integer)
            _DefaultLoadStatusControl = value
        End Set
    End Property



    Private _Stops As List(Of cls214StopData)
    Public Property Stops() As List(Of cls214StopData)
        Get
            If _Stops Is Nothing Then _Stops = New List(Of cls214StopData)
            Return _Stops
        End Get
        Set(ByVal value As List(Of cls214StopData))
            _Stops = value
        End Set
    End Property


End Class

''' <summary>
''' Stop Information from 
''' </summary>
''' <remarks></remarks>
Public Class cls214StopData

    Private _LoadComments As String
    Public Property LoadComments() As String
        Get
            Return _LoadComments
        End Get
        Set(ByVal value As String)
            _LoadComments = value
        End Set
    End Property

    Private _EventComments As String
    Public Property EventComments() As String
        Get
            Return _EventComments
        End Get
        Set(ByVal value As String)
            _EventComments = value
        End Set
    End Property

    Private _EventDate As Date?
    Public Property EventDate() As Date?
        Get
            Return _EventDate
        End Get
        Set(ByVal value As Date?)
            _EventDate = value
        End Set
    End Property

    Private _EventTime As Date?
    Public Property EventTime() As Date?
        Get
            Return _EventTime
        End Get
        Set(ByVal value As Date?)
            _EventTime = value
        End Set
    End Property

    Private _StatusDetails As New List(Of clsEDILoadStatusData)
    Public Property StatusDetails() As List(Of clsEDILoadStatusData)
        Get
            If _StatusDetails Is Nothing Then _StatusDetails = New List(Of clsEDILoadStatusData)
            Return _StatusDetails
        End Get
        Set(ByVal value As List(Of clsEDILoadStatusData))
            _StatusDetails = value
        End Set
    End Property

    Private _Orders As List(Of cls214OrderData)
    Public Property Orders() As List(Of cls214OrderData)
        Get
            If _Orders Is Nothing Then _Orders = New List(Of cls214OrderData)
            Return _Orders
        End Get
        Set(ByVal value As List(Of cls214OrderData))
            _Orders = value
        End Set
    End Property

    ''    create Private variable
    ''create Public Function for updating And one For reading (the Private Property)
    Private _CarrierComments As String

    Public Sub setCarrierComments(ByVal strVal As String)
        _CarrierComments = strVal
    End Sub

    Public Function getCarrierComments() As String
        Return _CarrierComments
    End Function



End Class

Public Class clsEDILoadStatusData

    Private _DocTypeName As String = "214"
    Public Property DocTypeName() As String
        Get
            Return _DocTypeName
        End Get
        Set(ByVal value As String)
            _DocTypeName = value
        End Set
    End Property

    Private _EDITypeDocControl As Integer
    Public Property EDIDocTypeControl() As Integer
        Get
            Return _EDITypeDocControl
        End Get
        Set(ByVal value As Integer)
            _EDITypeDocControl = value
        End Set
    End Property

    Private _EDIElementName As String
    Public Property EDIElementName() As String
        Get
            Return _EDIElementName
        End Get
        Set(ByVal value As String)
            _EDIElementName = value
        End Set
    End Property

    Private _EDIElementControl As Integer
    Public Property EDIElementControl() As Integer
        Get
            Return _EDIElementControl
        End Get
        Set(ByVal value As Integer)
            _EDIElementControl = value
        End Set
    End Property


    Private _EventDate As Date?
    Public Property EventDate() As Date?
        Get
            Return _EventDate
        End Get
        Set(ByVal value As Date?)
            _EventDate = value
        End Set
    End Property

    Private _EventTime As Date?
    Public Property EventTime() As Date?
        Get
            Return _EventTime
        End Get
        Set(ByVal value As Date?)
            _EventTime = value
        End Set
    End Property


    Private _EventCode As String
    Public Property EventCode() As String
        Get
            Return _EventCode
        End Get
        Set(ByVal value As String)
            _EventCode = value
        End Set
    End Property

    Private _EventComment As String
    Public Property EventComment() As String
        Get
            Return _EventComment
        End Get
        Set(ByVal value As String)
            _EventComment = value
        End Set
    End Property


    Private _EDIStatusCodes As DTO.EDIStatusCodes
    Public Property EDIStatusCodes() As DTO.EDIStatusCodes
        Get
            Return _EDIStatusCodes
        End Get
        Set(ByVal value As DTO.EDIStatusCodes)
            _EDIStatusCodes = value
        End Set
    End Property

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal eDoc As String, ByVal eName As String, ByVal eDate As Date, ByVal eTime As Date, ByVal eCode As String, ByVal eComment As String)
        MyBase.New()
        DocTypeName = eDoc
        EDIElementName = eName
        EventDate = eDate
        EventTime = eTime
        EventCode = eCode
        EventComment = eComment
    End Sub


End Class

Public Class cls214OrderData

    Private _BookControl As Integer = 0
    Public Property BookControl() As Integer
        Get
            Return _BookControl
        End Get
        Set(ByVal value As Integer)
            _BookControl = value
        End Set
    End Property


    Private _OrderReference As String
    Public Property OrderReference() As String
        Get
            Return _OrderReference
        End Get
        Set(ByVal value As String)
            _OrderReference = value
        End Set
    End Property

    Private _OrderNumber As String
    Public Property OrderNumber() As String
        Get
            Return _OrderNumber
        End Get
        Set(ByVal value As String)
            _OrderNumber = value
        End Set
    End Property

    Private _OrderSequence As Integer
    Public Property OrderSequence() As Integer
        Get
            Return _OrderSequence
        End Get
        Set(ByVal value As Integer)
            _OrderSequence = value
        End Set
    End Property

    Private _ApptSchedulingUpdateRequired As Boolean = False
    Public Property ApptSchedulingUpdateRequired() As Boolean
        Get
            Return _ApptSchedulingUpdateRequired
        End Get
        Set(ByVal value As Boolean)
            _ApptSchedulingUpdateRequired = value
        End Set
    End Property

    Private _CarrierChanged As Boolean = False
    Public Property CarrierChanged() As Boolean
        Get
            Return _CarrierChanged
        End Get
        Set(ByVal value As Boolean)
            _CarrierChanged = value
        End Set
    End Property




    Private _Tracks As List(Of DTO.BookTrack)
    Public Property Tracks() As List(Of DTO.BookTrack)
        Get
            If _Tracks Is Nothing Then _Tracks = New List(Of DTO.BookTrack)
            Return _Tracks
        End Get
        Set(ByVal value As List(Of DTO.BookTrack))
            _Tracks = value
        End Set
    End Property

    Public Sub splitOrderReference()
        Dim strSeq As String = ""
        Dim strSep As String = ""
        Dim sON() As String = OrderReference.Trim.Split("-")

        If sON.Length > 1 Then
            'We allow for extra "-"  in the order number like SO-0001-1
            For i As Integer = 0 To sON.Length - 1
                If i < sON.Length - 1 Then
                    OrderNumber &= strSep & sON(i)
                    strSep = "-"
                Else
                    Integer.TryParse(sON(i).Trim, OrderSequence)
                End If
            Next
        Else
            'This should not happen but we test for a missing sequence number in case of a mix up
            'The default sequence number of zero is used
            OrderNumber = OrderReference.Trim
            OrderSequence = 0
        End If
    End Sub



End Class
