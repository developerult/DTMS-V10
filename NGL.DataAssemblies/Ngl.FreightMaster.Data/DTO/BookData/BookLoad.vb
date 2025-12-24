Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker
Imports SerilogTracing

Namespace DataTransferObjects
    <DataContract()>
    Public Class BookLoad
        Inherits DTOBaseClass


#Region " Data Members"
        Private _BookLoadControl As Integer = 0
        <DataMember()>
        Public Property BookLoadControl() As Integer
            Get
                Return _BookLoadControl
            End Get
            Set(ByVal value As Integer)
                _BookLoadControl = value
            End Set
        End Property

        Private _BookLoadBookControl As Integer = 0
        <DataMember()>
        Public Property BookLoadBookControl() As Integer
            Get
                Return _BookLoadBookControl
            End Get
            Set(ByVal value As Integer)
                _BookLoadBookControl = value
            End Set
        End Property

        Private _BookLoadBuy As String = ""
        <DataMember()>
        Public Property BookLoadBuy() As String
            Get
                Return Left(_BookLoadBuy, 10)
            End Get
            Set(ByVal value As String)
                _BookLoadBuy = Left(value, 10)
            End Set
        End Property

        Private _BookLoadPONumber As String = ""
        <DataMember()>
        Public Property BookLoadPONumber() As String
            Get
                Return Left(_BookLoadPONumber, 20)
            End Get
            Set(ByVal value As String)
                _BookLoadPONumber = Left(value, 20)
            End Set
        End Property

        Private _BookLoadVendor As String = ""
        <DataMember()>
        Public Property BookLoadVendor() As String
            Get
                Return Left(_BookLoadVendor, 40)
            End Get
            Set(ByVal value As String)
                _BookLoadVendor = Left(value, 40)
            End Set
        End Property

        Private _BookLoadCaseQty As Integer = 0
        <DataMember()>
        Public Property BookLoadCaseQty() As Integer
            Get
                Return _BookLoadCaseQty
            End Get
            Set(ByVal value As Integer)
                _BookLoadCaseQty = value
            End Set
        End Property

        Private _BookLoadWgt As Double = 0
        <DataMember()>
        Public Property BookLoadWgt() As Double
            Get
                Return _BookLoadWgt
            End Get
            Set(ByVal value As Double)
                _BookLoadWgt = value
            End Set
        End Property

        Private _BookLoadCube As Integer = 0
        <DataMember()>
        Public Property BookLoadCube() As Integer
            Get
                Return _BookLoadCube
            End Get
            Set(ByVal value As Integer)
                _BookLoadCube = value
            End Set
        End Property

        Private _BookLoadPL As Double = 0
        <DataMember()>
        Public Property BookLoadPL() As Double
            Get
                Return _BookLoadPL
            End Get
            Set(ByVal value As Double)
                _BookLoadPL = value
            End Set
        End Property

        Private _BookLoadPX As Integer = 0
        <DataMember()>
        Public Property BookLoadPX() As Integer
            Get
                Return _BookLoadPX
            End Get
            Set(ByVal value As Integer)
                _BookLoadPX = value
            End Set
        End Property

        Private _BookLoadPType As String = ""
        <DataMember()>
        Public Property BookLoadPType() As String
            Get
                Return Left(_BookLoadPType, 50)
            End Get
            Set(ByVal value As String)
                _BookLoadPType = Left(value, 50)
            End Set
        End Property

        Private _BookLoadCom As String
        <DataMember()>
        Public Property BookLoadCom() As String
            Get
                If Len(Trim(_BookLoadCom)) < 1 Then _BookLoadCom = "D"
                Return _BookLoadCom
            End Get
            Set(ByVal value As String)
                _BookLoadCom = value
            End Set
        End Property

        Private _BookLoadPUOrigin As String = ""
        <DataMember()>
        Public Property BookLoadPUOrigin() As String
            Get
                Return Left(_BookLoadPUOrigin, 25)
            End Get
            Set(ByVal value As String)
                _BookLoadPUOrigin = Left(value, 25)
            End Set
        End Property

        Private _BookLoadBFC As Decimal = 0
        <DataMember()>
        Public Property BookLoadBFC() As Decimal
            Get
                Return _BookLoadBFC
            End Get
            Set(ByVal value As Decimal)
                _BookLoadBFC = value
            End Set
        End Property

        Private _BookLoadTotCost As Decimal = 0
        <DataMember()>
        Public Property BookLoadTotCost() As Decimal
            Get
                Return _BookLoadTotCost
            End Get
            Set(ByVal value As Decimal)
                _BookLoadTotCost = value
            End Set
        End Property

        Private _BookLoadActCost As Decimal? = 0
        <DataMember()>
        Public Property BookLoadActCost() As Decimal?
            Get
                Return _BookLoadActCost
            End Get
            Set(ByVal value As Decimal?)
                _BookLoadActCost = value
            End Set
        End Property

        Private _BookLoadComments As String = ""
        <DataMember()>
        Public Property BookLoadComments() As String
            Get
                Return Left(_BookLoadComments, 100)
            End Get
            Set(ByVal value As String)
                _BookLoadComments = Left(value, 100)
            End Set
        End Property

        Private _BookLoadStopSeq As Short = 0
        <DataMember()>
        Public Property BookLoadStopSeq() As Short
            Get
                Return _BookLoadStopSeq
            End Get
            Set(ByVal value As Short)
                _BookLoadStopSeq = value
            End Set
        End Property

        Private _BookLoadModDate As System.Nullable(Of Date)
        <DataMember()>
        Public Property BookLoadModDate() As System.Nullable(Of Date)
            Get
                Return _BookLoadModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookLoadModDate = value
            End Set
        End Property

        Private _BookLoadModUser As String = ""
        <DataMember()>
        Public Property BookLoadModUser() As String
            Get
                Return Left(_BookLoadModUser, 100)
            End Get
            Set(ByVal value As String)
                _BookLoadModUser = Left(value, 100)
            End Set
        End Property

        Private _BookLoadUpdated As Byte()
        <DataMember()>
        Public Property BookLoadUpdated() As Byte()
            Get
                Return _BookLoadUpdated
            End Get
            Set(ByVal value As Byte())
                _BookLoadUpdated = value
            End Set
        End Property

        Private _BookItems As List(Of BookItem)
        <DataMember()>
        Public Property BookItems() As List(Of BookItem)
            Get
                Return _BookItems
            End Get
            Set(ByVal value As List(Of BookItem))
                _BookItems = value
            End Set
        End Property
#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New BookLoad
            Using Logger.StartActivity("BookLoad.Clone")
                instance = DirectCast(MemberwiseClone(), BookLoad)
                instance.BookItems = Nothing
                For Each item In BookItems
                    instance.BookItems.Add(DirectCast(item.Clone, BookItem))
                Next
            End Using
            
            Return instance
        End Function

        Public Overrides Function ToString() As String
            Return $"BookLoadControl: {BookLoadControl}, BookControl: {BookLoadBookControl}, BookLoadBuy: {BookLoadBuy}, BookLoadPONumber: {BookLoadPONumber}, BookLoadVendor: {BookLoadVendor}, BookLoadCaseQty: {BookLoadCaseQty}, BookLoadWgt: {BookLoadWgt}, BookLoadCube: {BookLoadCube}, BookLoadPL: {BookLoadPL}, BookLoadPX: {BookLoadPX}, BookLoadPType: {BookLoadPType}, BookLoadCom: {BookLoadCom}, BookLoadPUOrigin: {BookLoadPUOrigin}, BookLoadBFC: {BookLoadBFC}, BookLoadTotCost: {BookLoadTotCost}, BookLoadActCost: {BookLoadActCost}, BookLoadComments: {BookLoadComments}, BookLoadStopSeq: {BookLoadStopSeq}, BookLoadModDate: {BookLoadModDate}, BookLoadModUser: {BookLoadModUser}, BookLoadUpdated: {BookLoadUpdated}"
        End Function
#End Region

    End Class
End Namespace