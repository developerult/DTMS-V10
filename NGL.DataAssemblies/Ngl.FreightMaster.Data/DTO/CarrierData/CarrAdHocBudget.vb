Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class CarrAdHocBudget
        Inherits DTOBaseClass


#Region " Data Members"
        Private _CarrAdHocBudControl As Integer = 0
        <DataMember()> _
        Public Property CarrAdHocBudControl() As Integer
            Get
                Return _CarrAdHocBudControl
            End Get
            Set(ByVal value As Integer)
                _CarrAdHocBudControl = value
            End Set
        End Property

        Private _CarrAdHocBudCarrAdHocControl As Integer = 0
        <DataMember()> _
        Public Property CarrAdHocBudCarrAdHocControl() As Integer
            Get
                Return _CarrAdHocBudCarrAdHocControl
            End Get
            Set(ByVal value As Integer)
                _CarrAdHocBudCarrAdHocControl = value
            End Set
        End Property

        Private _CarrAdHocBudModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CarrAdHocBudModDate() As System.Nullable(Of Date)
            Get
                Return _CarrAdHocBudModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _CarrAdHocBudModDate = value
            End Set
        End Property

        Private _CarrAdHocBudModUser As String = ""
        <DataMember()> _
        Public Property CarrAdHocBudModUser() As String
            Get
                Return Left(_CarrAdHocBudModUser, 100)
            End Get
            Set(ByVal value As String)
                _CarrAdHocBudModUser = Left(value, 100)
            End Set
        End Property

        Private _CarrAdHocBudExpMo1 As Decimal = 0
        <DataMember()> _
        Public Property CarrAdHocBudExpMo1() As Decimal
            Get
                Return _CarrAdHocBudExpMo1
            End Get
            Set(ByVal value As Decimal)
                _CarrAdHocBudExpMo1 = value
            End Set
        End Property

        Private _CarrAdHocBudExpMo2 As Decimal = 0
        <DataMember()> _
        Public Property CarrAdHocBudExpMo2() As Decimal
            Get
                Return _CarrAdHocBudExpMo2
            End Get
            Set(ByVal value As Decimal)
                _CarrAdHocBudExpMo2 = value
            End Set
        End Property

        Private _CarrAdHocBudExpMo3 As Decimal = 0
        <DataMember()> _
        Public Property CarrAdHocBudExpMo3() As Decimal
            Get
                Return _CarrAdHocBudExpMo3
            End Get
            Set(ByVal value As Decimal)
                _CarrAdHocBudExpMo3 = value
            End Set
        End Property

        Private _CarrAdHocBudExpMo4 As Decimal = 0
        <DataMember()> _
        Public Property CarrAdHocBudExpMo4() As Decimal
            Get
                Return _CarrAdHocBudExpMo4
            End Get
            Set(ByVal value As Decimal)
                _CarrAdHocBudExpMo4 = value
            End Set
        End Property

        Private _CarrAdHocBudExpMo5 As Decimal = 0
        <DataMember()> _
        Public Property CarrAdHocBudExpMo5() As Decimal
            Get
                Return _CarrAdHocBudExpMo5
            End Get
            Set(ByVal value As Decimal)
                _CarrAdHocBudExpMo5 = value
            End Set
        End Property

        Private _CarrAdHocBudExpMo6 As Decimal = 0
        <DataMember()> _
        Public Property CarrAdHocBudExpMo6() As Decimal
            Get
                Return _CarrAdHocBudExpMo6
            End Get
            Set(ByVal value As Decimal)
                _CarrAdHocBudExpMo6 = value
            End Set
        End Property

        Private _CarrAdHocBudExpMo7 As Decimal = 0
        <DataMember()> _
        Public Property CarrAdHocBudExpMo7() As Decimal
            Get
                Return _CarrAdHocBudExpMo7
            End Get
            Set(ByVal value As Decimal)
                _CarrAdHocBudExpMo7 = value
            End Set
        End Property

        Private _CarrAdHocBudExpMo8 As Decimal = 0
        <DataMember()> _
        Public Property CarrAdHocBudExpMo8() As Decimal
            Get
                Return _CarrAdHocBudExpMo8
            End Get
            Set(ByVal value As Decimal)
                _CarrAdHocBudExpMo8 = value
            End Set
        End Property

        Private _CarrAdHocBudExpMo9 As Decimal = 0
        <DataMember()> _
        Public Property CarrAdHocBudExpMo9() As Decimal
            Get
                Return _CarrAdHocBudExpMo9
            End Get
            Set(ByVal value As Decimal)
                _CarrAdHocBudExpMo9 = value
            End Set
        End Property

        Private _CarrAdHocBudExpMo10 As Decimal = 0
        <DataMember()> _
        Public Property CarrAdHocBudExpMo10() As Decimal
            Get
                Return _CarrAdHocBudExpMo10
            End Get
            Set(ByVal value As Decimal)
                _CarrAdHocBudExpMo10 = value
            End Set
        End Property

        Private _CarrAdHocBudExpMo11 As Decimal = 0
        <DataMember()> _
        Public Property CarrAdHocBudExpMo11() As Decimal
            Get
                Return _CarrAdHocBudExpMo11
            End Get
            Set(ByVal value As Decimal)
                _CarrAdHocBudExpMo11 = value
            End Set
        End Property

        Private _CarrAdHocBudExpMo12 As Decimal = 0
        <DataMember()> _
        Public Property CarrAdHocBudExpMo12() As Decimal
            Get
                Return _CarrAdHocBudExpMo12
            End Get
            Set(ByVal value As Decimal)
                _CarrAdHocBudExpMo12 = value
            End Set
        End Property

        Private _CarrAdHocBudExpTotal As Decimal = 0
        <DataMember()> _
        Public Property CarrAdHocBudExpTotal() As Decimal
            Get
                Return _CarrAdHocBudExpTotal
            End Get
            Set(ByVal value As Decimal)
                _CarrAdHocBudExpTotal = value
            End Set
        End Property

        Private _CarrAdHocBudActMo1 As Decimal = 0
        <DataMember()> _
        Public Property CarrAdHocBudActMo1() As Decimal
            Get
                Return _CarrAdHocBudActMo1
            End Get
            Set(ByVal value As Decimal)
                _CarrAdHocBudActMo1 = value
            End Set
        End Property

        Private _CarrAdHocBudActMo2 As Decimal = 0
        <DataMember()> _
        Public Property CarrAdHocBudActMo2() As Decimal
            Get
                Return _CarrAdHocBudActMo2
            End Get
            Set(ByVal value As Decimal)
                _CarrAdHocBudActMo2 = value
            End Set
        End Property

        Private _CarrAdHocBudActMo3 As Decimal = 0
        <DataMember()> _
        Public Property CarrAdHocBudActMo3() As Decimal
            Get
                Return _CarrAdHocBudActMo3
            End Get
            Set(ByVal value As Decimal)
                _CarrAdHocBudActMo3 = value
            End Set
        End Property

        Private _CarrAdHocBudActMo4 As Decimal = 0
        <DataMember()> _
        Public Property CarrAdHocBudActMo4() As Decimal
            Get
                Return _CarrAdHocBudActMo4
            End Get
            Set(ByVal value As Decimal)
                _CarrAdHocBudActMo4 = value
            End Set
        End Property

        Private _CarrAdHocBudActMo5 As Decimal = 0
        <DataMember()> _
        Public Property CarrAdHocBudActMo5() As Decimal
            Get
                Return _CarrAdHocBudActMo5
            End Get
            Set(ByVal value As Decimal)
                _CarrAdHocBudActMo5 = value
            End Set
        End Property

        Private _CarrAdHocBudActMo6 As Decimal = 0
        <DataMember()> _
        Public Property CarrAdHocBudActMo6() As Decimal
            Get
                Return _CarrAdHocBudActMo6
            End Get
            Set(ByVal value As Decimal)
                _CarrAdHocBudActMo6 = value
            End Set
        End Property

        Private _CarrAdHocBudActMo7 As Decimal = 0
        <DataMember()> _
        Public Property CarrAdHocBudActMo7() As Decimal
            Get
                Return _CarrAdHocBudActMo7
            End Get
            Set(ByVal value As Decimal)
                _CarrAdHocBudActMo7 = value
            End Set
        End Property

        Private _CarrAdHocBudActMo8 As Decimal = 0
        <DataMember()> _
        Public Property CarrAdHocBudActMo8() As Decimal
            Get
                Return _CarrAdHocBudActMo8
            End Get
            Set(ByVal value As Decimal)
                _CarrAdHocBudActMo8 = value
            End Set
        End Property

        Private _CarrAdHocBudActMo9 As Decimal = 0
        <DataMember()> _
        Public Property CarrAdHocBudActMo9() As Decimal
            Get
                Return _CarrAdHocBudActMo9
            End Get
            Set(ByVal value As Decimal)
                _CarrAdHocBudActMo9 = value
            End Set
        End Property

        Private _CarrAdHocBudActMo10 As Decimal = 0
        <DataMember()> _
        Public Property CarrAdHocBudActMo10() As Decimal
            Get
                Return _CarrAdHocBudActMo10
            End Get
            Set(ByVal value As Decimal)
                _CarrAdHocBudActMo10 = value
            End Set
        End Property

        Private _CarrAdHocBudActMo11 As Decimal = 0
        <DataMember()> _
        Public Property CarrAdHocBudActMo11() As Decimal
            Get
                Return _CarrAdHocBudActMo11
            End Get
            Set(ByVal value As Decimal)
                _CarrAdHocBudActMo11 = value
            End Set
        End Property

        Private _CarrAdHocBudActMo12 As Decimal = 0
        <DataMember()> _
        Public Property CarrAdHocBudActMo12() As Decimal
            Get
                Return _CarrAdHocBudActMo12
            End Get
            Set(ByVal value As Decimal)
                _CarrAdHocBudActMo12 = value
            End Set
        End Property

        Private _CarrAdHocBudActTotal As Decimal = 0
        <DataMember()> _
        Public Property CarrAdHocBudActTotal() As Decimal
            Get
                Return _CarrAdHocBudActTotal
            End Get
            Set(ByVal value As Decimal)
                _CarrAdHocBudActTotal = value
            End Set
        End Property

        Private _CarrAdHocBudgetUpdated As Byte()
        <DataMember()> _
        Public Property CarrAdHocBudgetUpdated() As Byte()
            Get
                Return _CarrAdHocBudgetUpdated
            End Get
            Set(ByVal value As Byte())
                _CarrAdHocBudgetUpdated = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CarrAdHocBudget
            instance = DirectCast(MemberwiseClone(), CarrAdHocBudget)
            Return instance
        End Function

#End Region

    End Class
End Namespace