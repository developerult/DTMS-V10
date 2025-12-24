Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class BookAdhocLoad
        Inherits DTOBaseClass


#Region " Data Members"
        Private _BookAdhocLoadControl As Integer = 0
        <DataMember()> _
        Public Property BookAdhocLoadControl() As Integer
            Get
                Return _BookAdhocLoadControl
            End Get
            Set(ByVal value As Integer)
                _BookAdhocLoadControl = value
            End Set
        End Property

        Private _BookAdhocLoadBookAdhocControl As Integer = 0
        <DataMember()> _
        Public Property BookAdhocLoadBookAdhocControl() As Integer
            Get
                Return _BookAdhocLoadBookAdhocControl
            End Get
            Set(ByVal value As Integer)
                _BookAdhocLoadBookAdhocControl = value
            End Set
        End Property

        Private _BookAdhocLoadBuy As String = ""
        <DataMember()> _
        Public Property BookAdhocLoadBuy() As String
            Get
                Return Left(_BookAdhocLoadBuy, 10)
            End Get
            Set(ByVal value As String)
                _BookAdhocLoadBuy = Left(value, 10)
            End Set
        End Property

        Private _BookAdhocLoadPONumber As String = ""
        <DataMember()> _
        Public Property BookAdhocLoadPONumber() As String
            Get
                Return Left(_BookAdhocLoadPONumber, 20)
            End Get
            Set(ByVal value As String)
                _BookAdhocLoadPONumber = Left(value, 20)
            End Set
        End Property

        Private _BookAdhocLoadVendor As String = ""
        <DataMember()> _
        Public Property BookAdhocLoadVendor() As String
            Get
                Return Left(_BookAdhocLoadVendor, 40)
            End Get
            Set(ByVal value As String)
                _BookAdhocLoadVendor = Left(value, 40)
            End Set
        End Property

        Private _BookAdhocLoadCaseQty As Integer = 0
        <DataMember()> _
        Public Property BookAdhocLoadCaseQty() As Integer
            Get
                Return _BookAdhocLoadCaseQty
            End Get
            Set(ByVal value As Integer)
                _BookAdhocLoadCaseQty = value
            End Set
        End Property

        Private _BookAdhocLoadWgt As Double = 0
        <DataMember()> _
        Public Property BookAdhocLoadWgt() As Double
            Get
                Return _BookAdhocLoadWgt
            End Get
            Set(ByVal value As Double)
                _BookAdhocLoadWgt = value
            End Set
        End Property

        Private _BookAdhocLoadCube As Integer = 0
        <DataMember()> _
        Public Property BookAdhocLoadCube() As Integer
            Get
                Return _BookAdhocLoadCube
            End Get
            Set(ByVal value As Integer)
                _BookAdhocLoadCube = value
            End Set
        End Property

        Private _BookAdhocLoadPL As Double = 0
        <DataMember()> _
        Public Property BookAdhocLoadPL() As Double
            Get
                Return _BookAdhocLoadPL
            End Get
            Set(ByVal value As Double)
                _BookAdhocLoadPL = value
            End Set
        End Property

        Private _BookAdhocLoadPX As Integer = 0
        <DataMember()> _
        Public Property BookAdhocLoadPX() As Integer
            Get
                Return _BookAdhocLoadPX
            End Get
            Set(ByVal value As Integer)
                _BookAdhocLoadPX = value
            End Set
        End Property

        Private _BookAdhocLoadPType As String = ""
        <DataMember()> _
        Public Property BookAdhocLoadPType() As String
            Get
                Return Left(_BookAdhocLoadPType, 50)
            End Get
            Set(ByVal value As String)
                _BookAdhocLoadPType = Left(value, 50)
            End Set
        End Property

        Private _BookAdhocLoadCom As String
        <DataMember()> _
        Public Property BookAdhocLoadCom() As String
            Get
                Return _BookAdhocLoadCom
            End Get
            Set(ByVal value As String)
                _BookAdhocLoadCom = value
            End Set
        End Property

        Private _BookAdhocLoadPUOrigin As String = ""
        <DataMember()> _
        Public Property BookAdhocLoadPUOrigin() As String
            Get
                Return Left(_BookAdhocLoadPUOrigin, 25)
            End Get
            Set(ByVal value As String)
                _BookAdhocLoadPUOrigin = Left(value, 25)
            End Set
        End Property

        Private _BookAdhocLoadBFC As Decimal = 0
        <DataMember()> _
        Public Property BookAdhocLoadBFC() As Decimal
            Get
                Return _BookAdhocLoadBFC
            End Get
            Set(ByVal value As Decimal)
                _BookAdhocLoadBFC = value
            End Set
        End Property

        Private _BookAdhocLoadTotCost As Decimal = 0
        <DataMember()> _
        Public Property BookAdhocLoadTotCost() As Decimal
            Get
                Return _BookAdhocLoadTotCost
            End Get
            Set(ByVal value As Decimal)
                _BookAdhocLoadTotCost = value
            End Set
        End Property

        Private _BookAdhocLoadComments As String = ""
        <DataMember()> _
        Public Property BookAdhocLoadComments() As String
            Get
                Return Left(_BookAdhocLoadComments, 100)
            End Get
            Set(ByVal value As String)
                _BookAdhocLoadComments = Left(value, 100)
            End Set
        End Property

        Private _BookAdhocLoadStopSeq As Short = 0
        <DataMember()> _
        Public Property BookAdhocLoadStopSeq() As Short
            Get
                Return _BookAdhocLoadStopSeq
            End Get
            Set(ByVal value As Short)
                _BookAdhocLoadStopSeq = value
            End Set
        End Property

        Private _BookAdhocLoadModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookAdhocLoadModDate() As System.Nullable(Of Date)
            Get
                Return _BookAdhocLoadModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookAdhocLoadModDate = value
            End Set
        End Property

        Private _BookAdhocLoadModUser As String = ""
        <DataMember()> _
        Public Property BookAdhocLoadModUser() As String
            Get
                Return Left(_BookAdhocLoadModUser, 100)
            End Get
            Set(ByVal value As String)
                _BookAdhocLoadModUser = Left(value, 100)
            End Set
        End Property

        Private _BookAdhocLoadUpdated As Byte()
        <DataMember()> _
        Public Property BookAdhocLoadUpdated() As Byte()
            Get
                Return _BookAdhocLoadUpdated
            End Get
            Set(ByVal value As Byte())
                _BookAdhocLoadUpdated = value
            End Set
        End Property

        Private _BookAdhocItems As List(Of BookAdhocItem)
        <DataMember()> _
        Public Property BookAdhocItems() As List(Of BookAdhocItem)
            Get
                Return _BookAdhocItems
            End Get
            Set(ByVal value As List(Of BookAdhocItem))
                _BookAdhocItems = value
            End Set
        End Property
#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New BookAdhocLoad
            instance = DirectCast(MemberwiseClone(), BookAdhocLoad)
            instance.BookAdhocItems = Nothing
            For Each item In BookAdhocItems
                instance.BookAdhocItems.Add(DirectCast(item.Clone, BookAdhocItem))
            Next
            Return instance
        End Function

#End Region

    End Class
End Namespace