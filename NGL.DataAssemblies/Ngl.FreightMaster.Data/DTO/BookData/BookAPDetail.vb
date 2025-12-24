Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class BookAPDetail
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

        Private _BookCarrBLNumber As String = ""
        <DataMember()> _
        Public Property BookCarrBLNumber() As String
            Get
                Return Left(_BookCarrBLNumber, 20)
            End Get
            Set(ByVal value As String)
                _BookCarrBLNumber = Left(value, 20)
            End Set
        End Property

        Private _BookFinAPActWgt As Integer = 0
        <DataMember()> _
        Public Property BookFinAPActWgt() As Integer
            Get
                Return _BookFinAPActWgt
            End Get
            Set(ByVal value As Integer)
                _BookFinAPActWgt = value
            End Set
        End Property

        Private _BookRevTotalCost As Decimal = 0
        <DataMember()> _
        Public Property BookRevTotalCost() As Decimal
            Get
                Return _BookRevTotalCost
            End Get
            Set(ByVal value As Decimal)
                _BookRevTotalCost = value
            End Set
        End Property

        Private _BookFinAPBillNumber As String = ""
        <DataMember()> _
        Public Property BookFinAPBillNumber() As String
            Get
                Return Left(_BookFinAPBillNumber, 50)
            End Get
            Set(ByVal value As String)
                _BookFinAPBillNumber = Left(value, 50)
            End Set
        End Property

        Private _BookFinAPBillNoDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookFinAPBillNoDate() As System.Nullable(Of Date)
            Get
                Return _BookFinAPBillNoDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookFinAPBillNoDate = value
            End Set
        End Property

        Private _BookFinAPBillInvDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookFinAPBillInvDate() As System.Nullable(Of Date)
            Get
                Return _BookFinAPBillInvDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookFinAPBillInvDate = value
            End Set
        End Property

        Private _BookFinAPPayDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property BookFinAPPayDate() As System.Nullable(Of Date)
            Get
                Return _BookFinAPPayDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _BookFinAPPayDate = value
            End Set
        End Property

        Private _BookFinAPPayAmt As Decimal = 0
        <DataMember()> _
        Public Property BookFinAPPayAmt() As Decimal
            Get
                Return _BookFinAPPayAmt
            End Get
            Set(ByVal value As Decimal)
                _BookFinAPPayAmt = value
            End Set
        End Property

        Private _BookFinAPCheck As String = ""
        <DataMember()> _
        Public Property BookFinAPCheck() As String
            Get
                Return Left(_BookFinAPCheck, 15)
            End Get
            Set(ByVal value As String)
                _BookFinAPCheck = Left(value, 15)
            End Set
        End Property

        Private _BookFinAPGLNumber As String = ""
        <DataMember()> _
        Public Property BookFinAPGLNumber() As String
            Get
                Return Left(_BookFinAPGLNumber, 50)
            End Get
            Set(ByVal value As String)
                _BookFinAPGLNumber = Left(value, 50)
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

        Private _BookPayCode As String = ""
        <DataMember()> _
        Public Property BookPayCode() As String
            Get
                Return Left(_BookPayCode, 3)
            End Get
            Set(ByVal value As String)
                _BookPayCode = Left(value, 3)
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
            Dim instance As New BookAPDetail
            instance = DirectCast(MemberwiseClone(), BookAPDetail)
            Return instance
        End Function

#End Region

    End Class
End Namespace
