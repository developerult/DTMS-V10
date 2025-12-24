Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

'Added By LVV 2/29/16 for v-7.0.5.1 EDI Migration
Namespace DataTransferObjects
    <DataContract()> _
    Public Class tbl210EDIFees
        Inherits DTOBaseClass

#Region " Data Members "

        Private _EDI210FeesControl As Integer = 0
        <DataMember()> _
        Public Property EDI210FeesControl() As Integer
            Get
                Return _EDI210FeesControl
            End Get
            Set(ByVal value As Integer)
                _EDI210FeesControl = value
            End Set
        End Property

        Private _EDI210Fees210EDIControl As Integer = 0
        <DataMember()> _
        Public Property EDI210Fees210EDIControl() As Integer
            Get
                Return _EDI210Fees210EDIControl
            End Get
            Set(ByVal value As Integer)
                _EDI210Fees210EDIControl = value
            End Set
        End Property

        Private _FeeName As String = ""
        <DataMember()> _
        Public Property FeeName() As String
            Get
                Return Left(_FeeName, 50)
            End Get
            Set(ByVal value As String)
                _FeeName = Left(value, 50)
            End Set
        End Property

        Private _FeeCost As Decimal = 0
        <DataMember()> _
        Public Property FeeCost() As Decimal
            Get
                Return _FeeCost
            End Get
            Set(ByVal value As Decimal)
                _FeeCost = value
            End Set
        End Property

        Private _EDICode As String = ""
        <DataMember()> _
        Public Property EDICode() As String
            Get
                Return Left(_EDICode, 50)
            End Get
            Set(ByVal value As String)
                _EDICode = Left(value, 50)
            End Set
        End Property

        Private _EDI210FeesModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property EDI210FeesModDate() As System.Nullable(Of Date)
            Get
                Return _EDI210FeesModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _EDI210FeesModDate = value
                Me.SendPropertyChanged("EDI210FeesModDate")
            End Set
        End Property

        Private _EDI210FeesModUser As String = ""
        <DataMember()> _
        Public Property EDI210FeesModUser() As String
            Get
                Return Left(_EDI210FeesModUser, 100)
            End Get
            Set(ByVal value As String)
                _EDI210FeesModUser = Left(value, 100)
                Me.SendPropertyChanged("EDI210FeesModUser")
            End Set
        End Property

        Private _EDI210FeesUpdated As Byte()
        <DataMember()> _
        Public Property EDI210FeesUpdated() As Byte()
            Get
                Return _EDI210FeesUpdated
            End Get
            Set(ByVal value As Byte())
                _EDI210FeesUpdated = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tbl210EDIFees
            instance = DirectCast(MemberwiseClone(), tbl210EDIFees)
            Return instance
        End Function

#End Region



    End Class
End Namespace
