Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class FuelSurchargeResult
        Inherits DTOBaseClass


#Region " Data Members"
         
        Private _CarrierControl As Nullable(Of Integer) = 0
        <DataMember()> _
        Public Property CarrierControl() As Nullable(Of Integer)
            Get
                Return _CarrierControl
            End Get
            Set(ByVal value As Nullable(Of Integer))
                _CarrierControl = value
            End Set
        End Property
         
        Private _CarrTarControl As Nullable(Of Integer) = 0
        <DataMember()> _
        Public Property CarrTarControl() As Nullable(Of Integer)
            Get
                Return _CarrTarControl
            End Get
            Set(ByVal value As Nullable(Of Integer))
                _CarrTarControl = value
            End Set
        End Property

        Private _FuelSurcharge As Nullable(Of Decimal) = 0
        <DataMember()> _
        Public Property FuelSurcharge() As Nullable(Of Decimal)
            Get
                Return _FuelSurcharge
            End Get
            Set(ByVal value As Nullable(Of Decimal))
                _FuelSurcharge = value
            End Set
        End Property

        Private _UseRatePerMile As Nullable(Of Boolean) = False
        <DataMember()> _
        Public Property UseRatePerMile() As Nullable(Of Boolean)
            Get
                Return _UseRatePerMile
            End Get
            Set(ByVal value As Nullable(Of Boolean))
                _UseRatePerMile = value
            End Set
        End Property

        Private _CarrTarEquipControl As Nullable(Of Integer) = 0
        <DataMember()> _
        Public Property CarrTarEquipControl() As Nullable(Of Integer)
            Get
                Return _CarrTarEquipControl
            End Get
            Set(ByVal value As Nullable(Of Integer))
                _CarrTarEquipControl = value
            End Set
        End Property

        Private _STATE As String
        <DataMember()> _
        Public Property STATE() As String
            Get
                Return _STATE
            End Get
            Set(ByVal value As String)
                _STATE = value
            End Set
        End Property

        Private _EffectiveDate As Nullable(Of Date)
        <DataMember()> _
        Public Property EffectiveDate() As Nullable(Of Date)
            Get
                Return _EffectiveDate
            End Get
            Set(ByVal value As Nullable(Of Date))
                _EffectiveDate = value
            End Set
        End Property
         
        Private _AvgFuelPrice As Nullable(Of Decimal) = 0
        <DataMember()> _
        Public Property AvgFuelPrice() As Nullable(Of Decimal)
            Get
                Return _AvgFuelPrice
            End Get
            Set(ByVal value As Nullable(Of Decimal))
                _AvgFuelPrice = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New FuelSurchargeResult
            instance = DirectCast(MemberwiseClone(), FuelSurchargeResult)
          
            Return instance
        End Function

#End Region

    End Class
End Namespace
