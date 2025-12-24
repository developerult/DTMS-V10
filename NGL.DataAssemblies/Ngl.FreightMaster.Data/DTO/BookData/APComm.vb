Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class APComm
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

        Private _BookRevCommCost As Decimal = 0
        <DataMember()> _
        Public Property BookRevCommCost() As Decimal
            Get
                Return _BookRevCommCost
            End Get
            Set(ByVal value As Decimal)
                _BookRevCommCost = value
            End Set
        End Property

        Private _BookFinCommStd As Decimal = 0
        <DataMember()> _
        Public Property BookFinCommStd() As Decimal
            Get
                Return _BookFinCommStd
            End Get
            Set(ByVal value As Decimal)
                _BookFinCommStd = value
            End Set
        End Property

        Private _BookFinCommAct As Decimal = 0
        <DataMember()> _
        Public Property BookFinCommAct() As Decimal
            Get
                Return _BookFinCommAct
            End Get
            Set(ByVal value As Decimal)
                _BookFinCommAct = value
            End Set
        End Property

        Private _BookFinCommPayDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookFinCommPayDate() As System.Nullable(Of Date)
            Get
                Return _BookFinCommPayDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookFinCommPayDate = value
            End Set
        End Property

        Private _BookFinCommPayAmt As Decimal = 0
        <DataMember()> _
        Public Property BookFinCommPayAmt() As Decimal
            Get
                Return _BookFinCommPayAmt
            End Get
            Set(ByVal value As Decimal)
                _BookFinCommPayAmt = value
            End Set
        End Property

        Private _BookFinCommtCheck As String = ""
        <DataMember()> _
        Public Property BookFinCommtCheck() As String
            Get
                Return Left(_BookFinCommtCheck, 15)
            End Get
            Set(ByVal value As String)
                _BookFinCommtCheck = Left(value, 15)
            End Set
        End Property

        Private _BookFinCommCreditAmt As Decimal = 0
        <DataMember()> _
        Public Property BookFinCommCreditAmt() As Decimal
            Get
                Return _BookFinCommCreditAmt
            End Get
            Set(ByVal value As Decimal)
                _BookFinCommCreditAmt = value
            End Set
        End Property

        Private _BookFinCommCreditPayDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookFinCommCreditPayDate() As System.Nullable(Of Date)
            Get
                Return _BookFinCommCreditPayDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookFinCommCreditPayDate = value
            End Set
        End Property

        Private _BookFinCommLoadCount As Integer = 0
        <DataMember()> _
        Public Property BookFinCommLoadCount() As Integer
            Get
                Return _BookFinCommLoadCount
            End Get
            Set(ByVal value As Integer)
                _BookFinCommLoadCount = value
            End Set
        End Property

        Private _BookFinCommGLNumber As String = ""
        <DataMember()> _
        Public Property BookFinCommGLNumber() As String
            Get
                Return Left(_BookFinCommGLNumber, 50)
            End Get
            Set(ByVal value As String)
                _BookFinCommGLNumber = Left(value, 50)
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

        Private _BookCommCompControl As Integer = 0
        <DataMember()> _
        Public Property BookCommCompControl() As Integer
            Get
                Return _BookCommCompControl
            End Get
            Set(ByVal value As Integer)
                _BookCommCompControl = value
            End Set
        End Property

        Private _BookModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookModDate() As System.Nullable(Of Date)
            Get
                Return _BookModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookModDate = value
            End Set
        End Property

        Private _BookModUser As String = ""
        <DataMember()> _
        Public Property BookModUser() As String
            Get
                Return Left(_BookModUser, 100)
            End Get
            Set(ByVal value As String)
                _BookModUser = Left(value, 100)
            End Set
        End Property

        Private _BookUpdated As Byte()
        <DataMember()> _
        Public Property BookUpdated() As Byte()
            Get
                Return _BookUpdated
            End Get
            Set(ByVal value As Byte())
                _BookUpdated = value
            End Set
        End Property
#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New APComm
            instance = DirectCast(MemberwiseClone(), APComm)
            Return instance
        End Function

#End Region

    End Class

End Namespace
