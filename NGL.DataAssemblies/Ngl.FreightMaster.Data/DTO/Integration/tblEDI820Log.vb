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
    Public Class tblEDI820Log
        Inherits DTOBaseClass

#Region " Data Members "

        Private _EDI820LogControl As Integer = 0
        <DataMember()> _
        Public Property EDI820LogControl() As Integer
            Get
                Return _EDI820LogControl
            End Get
            Set(ByVal value As Integer)
                _EDI820LogControl = value
            End Set
        End Property

        Private _BookFinARPayDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookFinARPayDate() As System.Nullable(Of Date)
            Get
                Return _BookFinARPayDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookFinARPayDate = value
            End Set
        End Property

        Private _BookFinARPayAmt As Decimal = 0
        <DataMember()> _
        Public Property BookFinARPayAmt() As Decimal
            Get
                Return _BookFinARPayAmt
            End Get
            Set(ByVal value As Decimal)
                _BookFinARPayAmt = value
            End Set
        End Property

        Private _BookFinARCheck As String = ""
        <DataMember()> _
        Public Property BookFinARCheck() As String
            Get
                Return Left(_BookFinARCheck, 50)
            End Get
            Set(ByVal value As String)
                _BookFinARCheck = Left(value, 50)
            End Set
        End Property

        Private _BookFinARBalance As Decimal = 0
        <DataMember()> _
        Public Property BookFinARBalance() As Decimal
            Get
                Return _BookFinARBalance
            End Get
            Set(ByVal value As Decimal)
                _BookFinARBalance = value
            End Set
        End Property

        Private _EDI820LogMessage As String = ""
        <DataMember()> _
        Public Property EDI820LogMessage() As String
            Get
                Return _EDI820LogMessage
            End Get
            Set(ByVal value As String)
                _EDI820LogMessage = value
            End Set
        End Property

        Private _EDI820InvoiceNumber As String = ""
        <DataMember()> _
        Public Property EDI820InvoiceNumber() As String
            Get
                Return Left(_EDI820InvoiceNumber, 20)
            End Get
            Set(ByVal value As String)
                _EDI820InvoiceNumber = Left(value, 20)
            End Set
        End Property

        Private _EDI820BookControl As Integer = 0
        <DataMember()> _
        Public Property EDI820BookControl() As Integer
            Get
                Return _EDI820BookControl
            End Get
            Set(ByVal value As Integer)
                _EDI820BookControl = value
            End Set
        End Property

        Private _EDI820CompControl As Integer = 0
        <DataMember()> _
        Public Property EDI820CompControl() As Integer
            Get
                Return _EDI820CompControl
            End Get
            Set(ByVal value As Integer)
                _EDI820CompControl = value
            End Set
        End Property

        Private _EDI820CompNumber As Integer = 0
        <DataMember()> _
        Public Property EDI820CompNumber() As Integer
            Get
                Return _EDI820CompNumber
            End Get
            Set(ByVal value As Integer)
                _EDI820CompNumber = value
            End Set
        End Property

        Private _EDI820CompName As String = ""
        <DataMember()> _
        Public Property EDI820CompName() As String
            Get
                Return Left(_EDI820CompName, 40)
            End Get
            Set(ByVal value As String)
                _EDI820CompName = Left(value, 40)
            End Set
        End Property

        Private _EDI820CarrierControl As Integer = 0
        <DataMember()> _
        Public Property EDI820CarrierControl() As Integer
            Get
                Return _EDI820CarrierControl
            End Get
            Set(ByVal value As Integer)
                _EDI820CarrierControl = value
            End Set
        End Property

        Private _EDI820CarrierNumber As Integer = 0
        <DataMember()> _
        Public Property EDI820CarrierNumber() As Integer
            Get
                Return _EDI820CarrierNumber
            End Get
            Set(ByVal value As Integer)
                _EDI820CarrierNumber = value
            End Set
        End Property

        Private _EDI820LogModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property EDI820LogModDate() As System.Nullable(Of Date)
            Get
                Return _EDI820LogModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _EDI820LogModDate = value
            End Set
        End Property

        Private _EDI820LogModUser As String = ""
        <DataMember()> _
        Public Property EDI820LogModUser() As String
            Get
                Return Left(_EDI820LogModUser, 100)
            End Get
            Set(ByVal value As String)
                _EDI820LogModUser = Left(value, 100)
            End Set
        End Property

        Private _EDI820LogUpdated As Byte()
        <DataMember()> _
        Public Property EDI820LogUpdated() As Byte()
            Get
                Return _EDI820LogUpdated
            End Get
            Set(ByVal value As Byte())
                _EDI820LogUpdated = value
            End Set
        End Property

        Private _EDI820LogFileName820 As String = ""
        <DataMember()> _
        Public Property EDI820LogFileName820() As String
            Get
                Return Left(_EDI820LogFileName820, 255)
            End Get
            Set(ByVal value As String)
                _EDI820LogFileName820 = Left(value, 255)
                Me.SendPropertyChanged("EDI820LogFileName820")
            End Set
        End Property

        Private _EDI820CarrierName As String = ""
        <DataMember()> _
        Public Property EDI820CarrierName() As String
            Get
                Return Left(_EDI820CarrierName, 40)
            End Get
            Set(ByVal value As String)
                _EDI820CarrierName = Left(value, 40)
            End Set
        End Property


#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblEDI820Log
            instance = DirectCast(MemberwiseClone(), tblEDI820Log)
            Return instance
        End Function

#End Region



    End Class
End Namespace
