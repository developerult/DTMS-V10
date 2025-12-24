Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class vOptimizationStop
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

        Private _BookDateLoad As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookDateLoad() As System.Nullable(Of Date)
            Get
                Return _BookDateLoad
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookDateLoad = value
            End Set
        End Property

        Private _BookStopNo As Short = 0
        <DataMember()> _
        Public Property BookStopNo() As Short
            Get
                Return _BookStopNo
            End Get
            Set(ByVal value As Short)
                _BookStopNo = value
            End Set
        End Property

        Private _BookDateRequired As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookDateRequired() As System.Nullable(Of Date)
            Get
                Return _BookDateRequired
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookDateRequired = value
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

        Private _CompControl As Integer = 0
        <DataMember()> _
        Public Property CompControl() As Integer
            Get
                Return _CompControl
            End Get
            Set(ByVal value As Integer)
                _CompControl = value
            End Set
        End Property

        Private _CompNumber As Integer = 0
        <DataMember()> _
        Public Property CompNumber() As Integer
            Get
                Return _CompNumber
            End Get
            Set(ByVal value As Integer)
                _CompNumber = value
            End Set
        End Property

        Private _CompName As String = ""
        <DataMember()> _
        Public Property CompName() As String
            Get
                Return Left(_CompName, 40)
            End Get
            Set(ByVal value As String)
                _CompName = Left(value, 40)
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

        Private _BookDestCity As String = ""
        <DataMember()> _
        Public Property BookDestCity() As String
            Get
                Return Left(_BookDestCity, 25)
            End Get
            Set(ByVal value As String)
                _BookDestCity = Left(value, 25)
            End Set
        End Property

        Private _BookDestState As String = ""
        <DataMember()> _
        Public Property BookDestState() As String
            Get
                Return Left(_BookDestState, 2)
            End Get
            Set(ByVal value As String)
                _BookDestState = Left(value, 2)
            End Set
        End Property

        Private _BookDestZip As String = ""
        <DataMember()> _
        Public Property BookDestZip() As String
            Get
                Return Left(_BookDestZip, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
            End Get
            Set(ByVal value As String)
                _BookDestZip = Left(value, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
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

        Private _BookLoadCaseQty As Integer = 0
        <DataMember()> _
        Public Property BookLoadCaseQty() As Integer
            Get
                Return _BookLoadCaseQty
            End Get
            Set(ByVal value As Integer)
                _BookLoadCaseQty = value
            End Set
        End Property

        Private _BookLoadWgt As Double = 0
        <DataMember()> _
        Public Property BookLoadWgt() As Double
            Get
                Return _BookLoadWgt
            End Get
            Set(ByVal value As Double)
                _BookLoadWgt = value
            End Set
        End Property

        Private _BookLoadCube As Integer = 0
        <DataMember()> _
        Public Property BookLoadCube() As Integer
            Get
                Return _BookLoadCube
            End Get
            Set(ByVal value As Integer)
                _BookLoadCube = value
            End Set
        End Property

        Private _BookLoadPL As Double = 0
        <DataMember()> _
        Public Property BookLoadPL() As Double
            Get
                Return _BookLoadPL
            End Get
            Set(ByVal value As Double)
                _BookLoadPL = value
            End Set
        End Property

        Private _BookLoadPX As Integer = 0
        <DataMember()> _
        Public Property BookLoadPX() As Integer
            Get
                Return _BookLoadPX
            End Get
            Set(ByVal value As Integer)
                _BookLoadPX = value
            End Set
        End Property

        Private _BookDestAddress1 As String = ""
        <DataMember()> _
        Public Property BookDestAddress1() As String
            Get
                Return Left(_BookDestAddress1, 40)
            End Get
            Set(ByVal value As String)
                _BookDestAddress1 = Left(value, 40)
            End Set
        End Property

        Private _BookDestAddress2 As String = ""
        <DataMember()> _
        Public Property BookDestAddress2() As String
            Get
                Return Left(_BookDestAddress2, 40)
            End Get
            Set(ByVal value As String)
                _BookDestAddress2 = Left(value, 40)
            End Set
        End Property

        Private _LaneLatitude As Double = 0
        <DataMember()> _
        Public Property LaneLatitude() As Double
            Get
                Return _LaneLatitude
            End Get
            Set(ByVal value As Double)
                _LaneLatitude = value
            End Set
        End Property

        Private _LaneLongitude As Double = 0
        <DataMember()> _
        Public Property LaneLongitude() As Double
            Get
                Return _LaneLongitude
            End Get
            Set(ByVal value As Double)
                _LaneLongitude = value
            End Set
        End Property

        Private _SpecialCodes As String = ""
        <DataMember()> _
        Public Property SpecialCodes() As String
            Get
                Return Left(_SpecialCodes, 100)
            End Get
            Set(ByVal value As String)
                _SpecialCodes = Left(value, 100)
            End Set
        End Property

        Private _LaneFixedTime As String = ""
        <DataMember()> _
        Public Property LaneFixedTime() As String
            Get
                Return Left(_LaneFixedTime, 50)
            End Get
            Set(ByVal value As String)
                _LaneFixedTime = Left(value, 50)
            End Set
        End Property

        Private _BookLoadControl As Integer = 0
        <DataMember()> _
        Public Property BookLoadControl() As Integer
            Get
                Return _BookLoadControl
            End Get
            Set(ByVal value As Integer)
                _BookLoadControl = value
            End Set
        End Property

        Private _BookHoldLoad As Integer = 0
        <DataMember()> _
        Public Property BookHoldLoad() As Integer
            Get
                Return _BookHoldLoad
            End Get
            Set(ByVal value As Integer)
                _BookHoldLoad = value
            End Set
        End Property


        Private _LaneControl As Integer = 0
        <DataMember()> _
        Public Property LaneControl() As Integer
            Get
                Return _LaneControl
            End Get
            Set(ByVal value As Integer)
                _LaneControl = value
            End Set
        End Property

        Private _ActualWgt As Double = 0
        <DataMember()> _
        Public Property ActualWgt() As Double
            Get
                Return _ActualWgt
            End Get
            Set(ByVal value As Double)
                _ActualWgt = value
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

        Private _BookCarrOrderNumber As String = ""
        <DataMember()> _
        Public Property BookCarrOrderNumber() As String
            Get
                Return Left(_BookCarrOrderNumber, 20)
            End Get
            Set(ByVal value As String)
                _BookCarrOrderNumber = Left(value, 20)
            End Set
        End Property


        Private _BookFinARInvoiceDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookFinARInvoiceDate() As System.Nullable(Of Date)
            Get
                Return _BookFinARInvoiceDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookFinARInvoiceDate = value
            End Set
        End Property

        Private _BookDateOrdered As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookDateOrdered() As System.Nullable(Of Date)
            Get
                Return _BookDateOrdered
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookDateOrdered = value
            End Set
        End Property

        Private _BookTranCode As String = ""
        <DataMember()> _
        Public Property BookTranCode() As String
            Get
                Return Left(_BookTranCode, 3)
            End Get
            Set(ByVal value As String)
                _BookTranCode = Left(value, 3)
            End Set
        End Property

        Private _BookSHID As String = ""
        <DataMember()> _
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
            Dim instance As New vOptimizationStop
            instance = DirectCast(MemberwiseClone(), vOptimizationStop)
            Return instance
        End Function

#End Region
    End Class
End Namespace
