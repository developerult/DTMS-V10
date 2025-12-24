Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class CarrierBudget
        Inherits DTOBaseClass


#Region " Data Members"
        Private _CarrierBudControl As Integer = 0
        <DataMember()> _
        Public Property CarrierBudControl() As Integer
            Get
                Return _CarrierBudControl
            End Get
            Set(ByVal value As Integer)
                _CarrierBudControl = value
            End Set
        End Property

        Private _CarrierBudCarrierControl As Integer = 0
        <DataMember()> _
        Public Property CarrierBudCarrierControl() As Integer
            Get
                Return _CarrierBudCarrierControl
            End Get
            Set(ByVal value As Integer)
                _CarrierBudCarrierControl = value
            End Set
        End Property

        Private _CarrierBudModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CarrierBudModDate() As System.Nullable(Of Date)
            Get
                Return _CarrierBudModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _CarrierBudModDate = value
            End Set
        End Property

        Private _CarrierBudModUser As String = ""
        <DataMember()> _
        Public Property CarrierBudModUser() As String
            Get
                Return Left(_CarrierBudModUser, 100)
            End Get
            Set(ByVal value As String)
                _CarrierBudModUser = Left(value, 100)
            End Set
        End Property

        Private _CarrierBudExpMo1 As Decimal = 0
        <DataMember()> _
        Public Property CarrierBudExpMo1() As Decimal
            Get
                Return _CarrierBudExpMo1
            End Get
            Set(ByVal value As Decimal)
                _CarrierBudExpMo1 = value
            End Set
        End Property

        Private _CarrierBudExpMo2 As Decimal = 0
        <DataMember()> _
        Public Property CarrierBudExpMo2() As Decimal
            Get
                Return _CarrierBudExpMo2
            End Get
            Set(ByVal value As Decimal)
                _CarrierBudExpMo2 = value
            End Set
        End Property

        Private _CarrierBudExpMo3 As Decimal = 0
        <DataMember()> _
        Public Property CarrierBudExpMo3() As Decimal
            Get
                Return _CarrierBudExpMo3
            End Get
            Set(ByVal value As Decimal)
                _CarrierBudExpMo3 = value
            End Set
        End Property

        Private _CarrierBudExpMo4 As Decimal = 0
        <DataMember()> _
        Public Property CarrierBudExpMo4() As Decimal
            Get
                Return _CarrierBudExpMo4
            End Get
            Set(ByVal value As Decimal)
                _CarrierBudExpMo4 = value
            End Set
        End Property

        Private _CarrierBudExpMo5 As Decimal = 0
        <DataMember()> _
        Public Property CarrierBudExpMo5() As Decimal
            Get
                Return _CarrierBudExpMo5
            End Get
            Set(ByVal value As Decimal)
                _CarrierBudExpMo5 = value
            End Set
        End Property

        Private _CarrierBudExpMo6 As Decimal = 0
        <DataMember()> _
        Public Property CarrierBudExpMo6() As Decimal
            Get
                Return _CarrierBudExpMo6
            End Get
            Set(ByVal value As Decimal)
                _CarrierBudExpMo6 = value
            End Set
        End Property

        Private _CarrierBudExpMo7 As Decimal = 0
        <DataMember()> _
        Public Property CarrierBudExpMo7() As Decimal
            Get
                Return _CarrierBudExpMo7
            End Get
            Set(ByVal value As Decimal)
                _CarrierBudExpMo7 = value
            End Set
        End Property

        Private _CarrierBudExpMo8 As Decimal = 0
        <DataMember()> _
        Public Property CarrierBudExpMo8() As Decimal
            Get
                Return _CarrierBudExpMo8
            End Get
            Set(ByVal value As Decimal)
                _CarrierBudExpMo8 = value
            End Set
        End Property

        Private _CarrierBudExpMo9 As Decimal = 0
        <DataMember()> _
        Public Property CarrierBudExpMo9() As Decimal
            Get
                Return _CarrierBudExpMo9
            End Get
            Set(ByVal value As Decimal)
                _CarrierBudExpMo9 = value
            End Set
        End Property

        Private _CarrierBudExpMo10 As Decimal = 0
        <DataMember()> _
        Public Property CarrierBudExpMo10() As Decimal
            Get
                Return _CarrierBudExpMo10
            End Get
            Set(ByVal value As Decimal)
                _CarrierBudExpMo10 = value
            End Set
        End Property

        Private _CarrierBudExpMo11 As Decimal = 0
        <DataMember()> _
        Public Property CarrierBudExpMo11() As Decimal
            Get
                Return _CarrierBudExpMo11
            End Get
            Set(ByVal value As Decimal)
                _CarrierBudExpMo11 = value
            End Set
        End Property

        Private _CarrierBudExpMo12 As Decimal = 0
        <DataMember()> _
        Public Property CarrierBudExpMo12() As Decimal
            Get
                Return _CarrierBudExpMo12
            End Get
            Set(ByVal value As Decimal)
                _CarrierBudExpMo12 = value
            End Set
        End Property

        Private _CarrierBudExpTotal As Decimal = 0
        <DataMember()> _
        Public Property CarrierBudExpTotal() As Decimal
            Get
                Return _CarrierBudExpTotal
            End Get
            Set(ByVal value As Decimal)
                _CarrierBudExpTotal = value
            End Set
        End Property

        Private _CarrierBudActMo1 As Decimal = 0
        <DataMember()> _
        Public Property CarrierBudActMo1() As Decimal
            Get
                Return _CarrierBudActMo1
            End Get
            Set(ByVal value As Decimal)
                _CarrierBudActMo1 = value
            End Set
        End Property

        Private _CarrierBudActMo2 As Decimal = 0
        <DataMember()> _
        Public Property CarrierBudActMo2() As Decimal
            Get
                Return _CarrierBudActMo2
            End Get
            Set(ByVal value As Decimal)
                _CarrierBudActMo2 = value
            End Set
        End Property

        Private _CarrierBudActMo3 As Decimal = 0
        <DataMember()> _
        Public Property CarrierBudActMo3() As Decimal
            Get
                Return _CarrierBudActMo3
            End Get
            Set(ByVal value As Decimal)
                _CarrierBudActMo3 = value
            End Set
        End Property

        Private _CarrierBudActMo4 As Decimal = 0
        <DataMember()> _
        Public Property CarrierBudActMo4() As Decimal
            Get
                Return _CarrierBudActMo4
            End Get
            Set(ByVal value As Decimal)
                _CarrierBudActMo4 = value
            End Set
        End Property

        Private _CarrierBudActMo5 As Decimal = 0
        <DataMember()> _
        Public Property CarrierBudActMo5() As Decimal
            Get
                Return _CarrierBudActMo5
            End Get
            Set(ByVal value As Decimal)
                _CarrierBudActMo5 = value
            End Set
        End Property

        Private _CarrierBudActMo6 As Decimal = 0
        <DataMember()> _
        Public Property CarrierBudActMo6() As Decimal
            Get
                Return _CarrierBudActMo6
            End Get
            Set(ByVal value As Decimal)
                _CarrierBudActMo6 = value
            End Set
        End Property

        Private _CarrierBudActMo7 As Decimal = 0
        <DataMember()> _
        Public Property CarrierBudActMo7() As Decimal
            Get
                Return _CarrierBudActMo7
            End Get
            Set(ByVal value As Decimal)
                _CarrierBudActMo7 = value
            End Set
        End Property

        Private _CarrierBudActMo8 As Decimal = 0
        <DataMember()> _
        Public Property CarrierBudActMo8() As Decimal
            Get
                Return _CarrierBudActMo8
            End Get
            Set(ByVal value As Decimal)
                _CarrierBudActMo8 = value
            End Set
        End Property

        Private _CarrierBudActMo9 As Decimal = 0
        <DataMember()> _
        Public Property CarrierBudActMo9() As Decimal
            Get
                Return _CarrierBudActMo9
            End Get
            Set(ByVal value As Decimal)
                _CarrierBudActMo9 = value
            End Set
        End Property

        Private _CarrierBudActMo10 As Decimal = 0
        <DataMember()> _
        Public Property CarrierBudActMo10() As Decimal
            Get
                Return _CarrierBudActMo10
            End Get
            Set(ByVal value As Decimal)
                _CarrierBudActMo10 = value
            End Set
        End Property

        Private _CarrierBudActMo11 As Decimal = 0
        <DataMember()> _
        Public Property CarrierBudActMo11() As Decimal
            Get
                Return _CarrierBudActMo11
            End Get
            Set(ByVal value As Decimal)
                _CarrierBudActMo11 = value
            End Set
        End Property

        Private _CarrierBudActMo12 As Decimal = 0
        <DataMember()> _
        Public Property CarrierBudActMo12() As Decimal
            Get
                Return _CarrierBudActMo12
            End Get
            Set(ByVal value As Decimal)
                _CarrierBudActMo12 = value
            End Set
        End Property

        Private _CarrierBudActTotal As Decimal = 0
        <DataMember()> _
        Public Property CarrierBudActTotal() As Decimal
            Get
                Return _CarrierBudActTotal
            End Get
            Set(ByVal value As Decimal)
                _CarrierBudActTotal = value
            End Set
        End Property

        Private _CarrierBudgetUpdated As Byte()
        <DataMember()> _
        Public Property CarrierBudgetUpdated() As Byte()
            Get
                Return _CarrierBudgetUpdated
            End Get
            Set(ByVal value As Byte())
                _CarrierBudgetUpdated = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CarrierBudget
            instance = DirectCast(MemberwiseClone(), CarrierBudget)
            Return instance
        End Function

#End Region

    End Class
End Namespace