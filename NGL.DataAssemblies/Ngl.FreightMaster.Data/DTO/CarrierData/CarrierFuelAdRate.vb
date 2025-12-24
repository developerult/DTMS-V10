Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class CarrierFuelAdRate
        Inherits DTOBaseClass


#Region " Data Members"
        Private _CarrFuelAdRatesControl As Integer = 0
        <DataMember()> _
        Public Property CarrFuelAdRatesControl() As Integer
            Get
                Return _CarrFuelAdRatesControl
            End Get
            Set(ByVal value As Integer)
                _CarrFuelAdRatesControl = value
            End Set
        End Property

        Private _CarrFuelAdRatesCarrFuelAdControl As Integer = 0
        <DataMember()> _
        Public Property CarrFuelAdRatesCarrFuelAdControl() As Integer
            Get
                Return _CarrFuelAdRatesCarrFuelAdControl
            End Get
            Set(ByVal value As Integer)
                _CarrFuelAdRatesCarrFuelAdControl = value
            End Set
        End Property

        Private _CarrFuelAdRatesPriceFrom As Decimal = 0
        <DataMember()> _
        Public Property CarrFuelAdRatesPriceFrom() As Decimal
            Get
                Return _CarrFuelAdRatesPriceFrom
            End Get
            Set(ByVal value As Decimal)
                _CarrFuelAdRatesPriceFrom = value
            End Set
        End Property

        Private _CarrFuelAdRatesPriceTo As Decimal = 0
        <DataMember()> _
        Public Property CarrFuelAdRatesPriceTo() As Decimal
            Get
                Return _CarrFuelAdRatesPriceTo
            End Get
            Set(ByVal value As Decimal)
                _CarrFuelAdRatesPriceTo = value
            End Set
        End Property

        Private _CarrFuelAdRatesPerMile As Decimal = 0
        <DataMember()> _
        Public Property CarrFuelAdRatesPerMile() As Decimal
            Get
                Return _CarrFuelAdRatesPerMile
            End Get
            Set(ByVal value As Decimal)
                _CarrFuelAdRatesPerMile = value
            End Set
        End Property

        Private _CarrFuelAdRatesPercent As Decimal = 0
        <DataMember()> _
        Public Property CarrFuelAdRatesPercent() As Decimal
            Get
                Return _CarrFuelAdRatesPercent
            End Get
            Set(ByVal value As Decimal)
                _CarrFuelAdRatesPercent = value
            End Set
        End Property

        Private _CarrFuelAdRatesEffDate As Date = Now
        <DataMember()> _
        Public Property CarrFuelAdRatesEffDate() As Date
            Get
                Return _CarrFuelAdRatesEffDate
            End Get
            Set(ByVal value As Date)
                _CarrFuelAdRatesEffDate = value
            End Set
        End Property

        Private _CarrFuelAdRatesModUser As String = ""
        <DataMember()> _
        Public Property CarrFuelAdRatesModUser() As String
            Get
                Return Left(_CarrFuelAdRatesModUser, 100)
            End Get
            Set(ByVal value As String)
                _CarrFuelAdRatesModUser = Left(value, 100)
            End Set
        End Property

        Private _CarrFuelAdRatesModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CarrFuelAdRatesModDate() As System.Nullable(Of Date)
            Get
                Return _CarrFuelAdRatesModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _CarrFuelAdRatesModDate = value
            End Set
        End Property

        Private _CarrFuelAdRatesUpdated As Byte()
        <DataMember()> _
        Public Property CarrFuelAdRatesUpdated() As Byte()
            Get
                Return _CarrFuelAdRatesUpdated
            End Get
            Set(ByVal value As Byte())
                _CarrFuelAdRatesUpdated = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CarrierFuelAdRate
            instance = DirectCast(MemberwiseClone(), CarrierFuelAdRate)
            Return instance
        End Function

#End Region

    End Class
End Namespace