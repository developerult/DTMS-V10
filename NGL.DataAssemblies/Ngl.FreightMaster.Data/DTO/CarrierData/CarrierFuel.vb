Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker
Imports Serilog

Namespace DataTransferObjects
    <DataContract()> _
    Public Class CarrierFuel
        Inherits DTOBaseClass

        Public Sub New()
            Me.Logger = Me.Logger.ForContext(of CarrierFuel)
        End Sub

#Region " Data Members"
        Private _CarrierFuelControl As Integer = 0
        <DataMember()> _
        Public Property CarrierFuelControl() As Integer
            Get
                Return _CarrierFuelControl
            End Get
            Set(ByVal value As Integer)
                _CarrierFuelControl = value
            End Set
        End Property

        Private _CarrierFuelCarrierControl As Integer = 0
        <DataMember()> _
        Public Property CarrierFuelCarrierControl() As Integer
            Get
                Return _CarrierFuelCarrierControl
            End Get
            Set(ByVal value As Integer)
                _CarrierFuelCarrierControl = value
            End Set
        End Property

        Private _CarrierFuelState As String = ""
        <DataMember()> _
        Public Property CarrierFuelState() As String
            Get
                Return Left(_CarrierFuelState, 2)
            End Get
            Set(ByVal value As String)
                _CarrierFuelState = Left(value, 2)
            End Set
        End Property

        Private _CarrierFuelStatePercent As Double = 0
        <DataMember()> _
        Public Property CarrierFuelStatePercent() As Double
            Get
                Return _CarrierFuelStatePercent
            End Get
            Set(ByVal value As Double)
                _CarrierFuelStatePercent = value
            End Set
        End Property

        Private _CarrierFuelEffectiveDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CarrierFuelEffectiveDate() As System.Nullable(Of Date)
            Get
                Return _CarrierFuelEffectiveDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _CarrierFuelEffectiveDate = value
            End Set
        End Property

        Private _CarrierFuelUpdated As Byte()
        <DataMember()> _
        Public Property CarrierFuelUpdated() As Byte()
            Get
                Return _CarrierFuelUpdated
            End Get
            Set(ByVal value As Byte())
                _CarrierFuelUpdated = value
            End Set
        End Property

        Private _CarrierFuelStates As List(Of CarrierFuelState)
        <DataMember()> _
        Public Property CarrierFuelStates() As List(Of CarrierFuelState)
            Get
                Return _CarrierFuelStates
            End Get
            Set(ByVal value As List(Of CarrierFuelState))
                _CarrierFuelStates = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CarrierFuel
            instance = DirectCast(MemberwiseClone(), CarrierFuel)
            instance.CarrierFuelStates = Nothing
            For Each item In CarrierFuelStates
                instance.CarrierFuelStates.Add(DirectCast(item.Clone, CarrierFuelState))
            Next
            Return instance
        End Function

#End Region

    End Class
End Namespace