Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class CarrierFuelState
        Inherits DTOBaseClass


#Region " Data Members"
        Private _CarrierFuelStateControl As Integer = 0
        <DataMember()> _
        Public Property CarrierFuelStateControl() As Integer
            Get
                Return _CarrierFuelStateControl
            End Get
            Set(ByVal value As Integer)
                _CarrierFuelStateControl = value
            End Set
        End Property

        Private _CarrierFuelStateFuelControl As Integer = 0
        <DataMember()> _
        Public Property CarrierFuelStateFuelControl() As Integer
            Get
                Return _CarrierFuelStateFuelControl
            End Get
            Set(ByVal value As Integer)
                _CarrierFuelStateFuelControl = value
            End Set
        End Property

        Private _CarrierFuelStatePercentage As Double = 0
        <DataMember()> _
        Public Property CarrierFuelStatePercentage() As Double
            Get
                Return _CarrierFuelStatePercentage
            End Get
            Set(ByVal value As Double)
                _CarrierFuelStatePercentage = value
            End Set
        End Property

        Private _CarrierFuelStateEffective As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CarrierFuelStateEffective() As System.Nullable(Of Date)
            Get
                Return _CarrierFuelStateEffective
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _CarrierFuelStateEffective = value
            End Set
        End Property

        Private _CarrierFuelStateUpdated As Byte()
        <DataMember()> _
        Public Property CarrierFuelStateUpdated() As Byte()
            Get
                Return _CarrierFuelStateUpdated
            End Get
            Set(ByVal value As Byte())
                _CarrierFuelStateUpdated = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CarrierFuelState
            instance = DirectCast(MemberwiseClone(), CarrierFuelState)
            Return instance
        End Function

#End Region

    End Class
End Namespace