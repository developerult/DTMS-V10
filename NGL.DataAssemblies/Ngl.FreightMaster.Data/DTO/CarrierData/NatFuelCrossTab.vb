Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class NatFuelCrossTab
        Inherits DTOBaseClass


#Region " Data Members"
        Private _NatFuelID As Integer = 0
        <DataMember()> _
        Public Property NatFuelID() As Integer
            Get
                Return _NatFuelID
            End Get
            Set(ByVal value As Integer)
                _NatFuelID = value
            End Set
        End Property

        Private _NatFuelDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property NatFuelDate() As System.Nullable(Of Date)
            Get
                Return _NatFuelDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _NatFuelDate = value
            End Set
        End Property

        Private _NatAverage As Decimal = 0
        <DataMember()> _
        Public Property NatAverage() As Decimal
            Get
                Return _NatAverage
            End Get
            Set(ByVal value As Decimal)
                _NatAverage = value
            End Set
        End Property

        Private _ZoneFuelCost1 As Decimal = 0
        <DataMember()> _
        Public Property ZoneFuelCost1() As Decimal
            Get
                Return _ZoneFuelCost1
            End Get
            Set(ByVal value As Decimal)
                _ZoneFuelCost1 = value
            End Set
        End Property

        Private _ZoneFuelCost2 As Decimal = 0
        <DataMember()> _
        Public Property ZoneFuelCost2() As Decimal
            Get
                Return _ZoneFuelCost2
            End Get
            Set(ByVal value As Decimal)
                _ZoneFuelCost2 = value
            End Set
        End Property

        Private _ZoneFuelCost3 As Decimal = 0
        <DataMember()> _
        Public Property ZoneFuelCost3() As Decimal
            Get
                Return _ZoneFuelCost3
            End Get
            Set(ByVal value As Decimal)
                _ZoneFuelCost3 = value
            End Set
        End Property

        Private _ZoneFuelCost4 As Decimal = 0
        <DataMember()> _
        Public Property ZoneFuelCost4() As Decimal
            Get
                Return _ZoneFuelCost4
            End Get
            Set(ByVal value As Decimal)
                _ZoneFuelCost4 = value
            End Set
        End Property

        Private _ZoneFuelCost5 As Decimal = 0
        <DataMember()> _
        Public Property ZoneFuelCost5() As Decimal
            Get
                Return _ZoneFuelCost5
            End Get
            Set(ByVal value As Decimal)
                _ZoneFuelCost5 = value
            End Set
        End Property

        Private _ZoneFuelCost6 As Decimal = 0
        <DataMember()> _
        Public Property ZoneFuelCost6() As Decimal
            Get
                Return _ZoneFuelCost6
            End Get
            Set(ByVal value As Decimal)
                _ZoneFuelCost6 = value
            End Set
        End Property

        Private _ZoneFuelCost7 As Decimal = 0
        <DataMember()> _
        Public Property ZoneFuelCost7() As Decimal
            Get
                Return _ZoneFuelCost7
            End Get
            Set(ByVal value As Decimal)
                _ZoneFuelCost7 = value
            End Set
        End Property

        Private _ZoneFuelCost8 As Decimal = 0
        <DataMember()> _
        Public Property ZoneFuelCost8() As Decimal
            Get
                Return _ZoneFuelCost8
            End Get
            Set(ByVal value As Decimal)
                _ZoneFuelCost8 = value
            End Set
        End Property

        Private _ZoneFuelCost9 As Decimal = 0
        <DataMember()> _
        Public Property ZoneFuelCost9() As Decimal
            Get
                Return _ZoneFuelCost9
            End Get
            Set(ByVal value As Decimal)
                _ZoneFuelCost9 = value
            End Set
        End Property

        Private _ZoneFuelName1 As String = ""
        <DataMember()> _
        Public Property ZoneFuelName1() As String
            Get
                Return Left(_ZoneFuelName1, 50)
            End Get
            Set(ByVal value As String)
                _ZoneFuelName1 = Left(value, 50)
            End Set
        End Property

        Private _ZoneFuelName2 As String = ""
        <DataMember()> _
        Public Property ZoneFuelName2() As String
            Get
                Return Left(_ZoneFuelName2, 50)
            End Get
            Set(ByVal value As String)
                _ZoneFuelName2 = Left(value, 50)
            End Set
        End Property

        Private _ZoneFuelName3 As String = ""
        <DataMember()> _
        Public Property ZoneFuelName3() As String
            Get
                Return Left(_ZoneFuelName3, 50)
            End Get
            Set(ByVal value As String)
                _ZoneFuelName3 = Left(value, 50)
            End Set
        End Property

        Private _ZoneFuelName4 As String = ""
        <DataMember()> _
        Public Property ZoneFuelName4() As String
            Get
                Return Left(_ZoneFuelName4, 50)
            End Get
            Set(ByVal value As String)
                _ZoneFuelName4 = Left(value, 50)
            End Set
        End Property

        Private _ZoneFuelName5 As String = ""
        <DataMember()> _
        Public Property ZoneFuelName5() As String
            Get
                Return Left(_ZoneFuelName5, 50)
            End Get
            Set(ByVal value As String)
                _ZoneFuelName5 = Left(value, 50)
            End Set
        End Property

        Private _ZoneFuelName6 As String = ""
        <DataMember()> _
        Public Property ZoneFuelName6() As String
            Get
                Return Left(_ZoneFuelName6, 50)
            End Get
            Set(ByVal value As String)
                _ZoneFuelName6 = Left(value, 50)
            End Set
        End Property

        Private _ZoneFuelName7 As String = ""
        <DataMember()> _
        Public Property ZoneFuelName7() As String
            Get
                Return Left(_ZoneFuelName7, 50)
            End Get
            Set(ByVal value As String)
                _ZoneFuelName7 = Left(value, 50)
            End Set
        End Property

        Private _ZoneFuelName8 As String = ""
        <DataMember()> _
        Public Property ZoneFuelName8() As String
            Get
                Return Left(_ZoneFuelName8, 50)
            End Get
            Set(ByVal value As String)
                _ZoneFuelName8 = Left(value, 50)
            End Set
        End Property

        Private _ZoneFuelName9 As String = ""
        <DataMember()> _
        Public Property ZoneFuelName9() As String
            Get
                Return Left(_ZoneFuelName9, 50)
            End Get
            Set(ByVal value As String)
                _ZoneFuelName9 = Left(value, 50)
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New NatFuelCrossTab
            instance = DirectCast(MemberwiseClone(), NatFuelCrossTab)
            Return instance
        End Function

#End Region

    End Class
End Namespace