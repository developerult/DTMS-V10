Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class CarrierFuelAddendum
        Inherits DTOBaseClass


#Region " Data Members"
        Private _CarrFuelAdControl As Integer = 0
        <DataMember()> _
        Public Property CarrFuelAdControl() As Integer
            Get
                Return _CarrFuelAdControl
            End Get
            Set(ByVal value As Integer)
                _CarrFuelAdControl = value
            End Set
        End Property

        Private _CarrFuelAdCarrierControl As Integer = 0
        <DataMember()> _
        Public Property CarrFuelAdCarrierControl() As Integer
            Get
                Return _CarrFuelAdCarrierControl
            End Get
            Set(ByVal value As Integer)
                _CarrFuelAdCarrierControl = value
            End Set
        End Property

        Private _CarrFuelAdCarrTarControl As Integer = 0
        <DataMember()> _
        Public Property CarrFuelAdCarrTarControl() As Integer
            Get
                Return _CarrFuelAdCarrTarControl
            End Get
            Set(ByVal value As Integer)
                _CarrFuelAdCarrTarControl = value
            End Set
        End Property

        Private _CarrFuelAdCarrTarEquipControl As Integer = 0
        <DataMember()> _
        Public Property CarrFuelAdCarrTarEquipControl() As Integer
            Get
                Return _CarrFuelAdCarrTarEquipControl
            End Get
            Set(ByVal value As Integer)
                _CarrFuelAdCarrTarEquipControl = value
            End Set
        End Property

        Private _CarrFuelAdUseNatAvg As Boolean = False
        <DataMember()> _
        Public Property CarrFuelAdUseNatAvg() As Boolean
            Get
                Return _CarrFuelAdUseNatAvg
            End Get
            Set(ByVal value As Boolean)
                _CarrFuelAdUseNatAvg = value
            End Set
        End Property

        Private _CarrFuelAdUseZoneAvg As Boolean = False
        <DataMember()> _
        Public Property CarrFuelAdUseZoneAvg() As Boolean
            Get
                Return _CarrFuelAdUseZoneAvg
            End Get
            Set(ByVal value As Boolean)
                _CarrFuelAdUseZoneAvg = value
            End Set
        End Property

        Private _CarrFuelAdUseRatePerMile As Boolean = False
        <DataMember()> _
        Public Property CarrFuelAdUseRatePerMile() As Boolean
            Get
                Return _CarrFuelAdUseRatePerMile
            End Get
            Set(ByVal value As Boolean)
                _CarrFuelAdUseRatePerMile = value
            End Set
        End Property

        Private _CarrFuelAdDefFuelRate As Decimal = 0
        <DataMember()> _
        Public Property CarrFuelAdDefFuelRate() As Decimal
            Get
                Return _CarrFuelAdDefFuelRate
            End Get
            Set(ByVal value As Decimal)
                _CarrFuelAdDefFuelRate = value
            End Set
        End Property

        Private _CarrFuelAdModUser As String = ""
        <DataMember()> _
        Public Property CarrFuelAdModUser() As String
            Get
                Return Left(_CarrFuelAdModUser, 100)
            End Get
            Set(ByVal value As String)
                _CarrFuelAdModUser = Left(value, 100)
            End Set
        End Property

        Private _CarrFuelAdModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CarrFuelAdModDate() As System.Nullable(Of Date)
            Get
                Return _CarrFuelAdModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _CarrFuelAdModDate = value
            End Set
        End Property

        Private _CarrFuelAdUpdated As Byte()
        <DataMember()> _
        Public Property CarrFuelAdUpdated() As Byte()
            Get
                Return _CarrFuelAdUpdated
            End Get
            Set(ByVal value As Byte())
                _CarrFuelAdUpdated = value
            End Set
        End Property

        Private _CarrierFuelAdExes As New List(Of CarrierFuelAdEx)
        <DataMember()> _
        Public Property CarrierFuelAdExes() As List(Of CarrierFuelAdEx)
            Get
                Return _CarrierFuelAdExes
            End Get
            Set(ByVal value As List(Of CarrierFuelAdEx))
                _CarrierFuelAdExes = value
            End Set
        End Property

        Private _CarrierFuelAdRates As New List(Of CarrierFuelAdRate)
        <DataMember()> _
        Public Property CarrierFuelAdRates() As List(Of CarrierFuelAdRate)
            Get
                Return _CarrierFuelAdRates
            End Get
            Set(ByVal value As List(Of CarrierFuelAdRate))
                _CarrierFuelAdRates = value
            End Set
        End Property


#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CarrierFuelAddendum
            instance = DirectCast(MemberwiseClone(), CarrierFuelAddendum)
            instance.CarrierFuelAdExes = Nothing
            For Each item In CarrierFuelAdExes
                instance.CarrierFuelAdExes.Add(DirectCast(item.Clone, CarrierFuelAdEx))
            Next
            instance.CarrierFuelAdRates = Nothing
            For Each item In CarrierFuelAdRates
                instance.CarrierFuelAdRates.Add(DirectCast(item.Clone, CarrierFuelAdRate))
            Next
            Return instance
        End Function

#End Region

    End Class
End Namespace
