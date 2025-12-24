Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class DoFinalizeAlerts
        Inherits DTOBaseClass


#Region " Data Members"

        Private _CarrierControl As Integer = 0
        <DataMember()> _
        Public Property CarrierControl() As Integer
            Get
                Return _CarrierControl
            End Get
            Set(ByVal value As Integer)
                _CarrierControl = value
            End Set
        End Property

        Private _CarrierNumber As Integer = 0
        <DataMember()> _
        Public Property CarrierNumber() As Integer
            Get
                Return _CarrierNumber
            End Get
            Set(ByVal value As Integer)
                _CarrierNumber = value
            End Set
        End Property

        Private _PerShipmentValue As Decimal = 0
        <DataMember()> _
        Public Property PerShipmentValue() As Decimal
            Get
                Return _PerShipmentValue
            End Get
            Set(ByVal value As Decimal)
                _PerShipmentValue = value
            End Set
        End Property

        Private _TotalExposueValue As Decimal = 0
        <DataMember()> _
        Public Property TotalExposueValue() As Decimal
            Get
                Return _TotalExposueValue
            End Get
            Set(ByVal value As Decimal)
                _TotalExposueValue = value
            End Set
        End Property

        Private _PerShipmentExposure As Decimal = 0
        <DataMember()> _
        Public Property PerShipmentExposure() As Decimal
            Get
                Return _PerShipmentExposure
            End Get
            Set(ByVal value As Decimal)
                _PerShipmentExposure = value
            End Set
        End Property

        Private _AllShipmentExposure As Decimal = 0
        <DataMember()> _
        Public Property AllShipmentExposure() As Decimal
            Get
                Return _AllShipmentExposure
            End Get
            Set(ByVal value As Decimal)
                _AllShipmentExposure = value
            End Set
        End Property

        Private _InsuranceMessage As String = ""
        <DataMember()> _
        Public Property InsuranceMessage() As String
            Get
                Return _InsuranceMessage
            End Get
            Set(ByVal value As String)
                _InsuranceMessage = value
            End Set
        End Property

        Private _ContractExpiresMessage As String = ""
        <DataMember()> _
        Public Property ContractExpiresMessage() As String
            Get
                Return _ContractExpiresMessage
            End Get
            Set(ByVal value As String)
                _ContractExpiresMessage = value
            End Set
        End Property

        Private _ExposureAllMessage As String = ""
        <DataMember()> _
        Public Property ExposureAllMessage() As String
            Get
                Return _ExposureAllMessage
            End Get
            Set(ByVal value As String)
                _ExposureAllMessage = value
            End Set
        End Property

        Private _ExposuerPerShipmentMessage As String = ""
        <DataMember()> _
        Public Property ExposuerPerShipmentMessage() As String
            Get
                Return _ExposuerPerShipmentMessage
            End Get
            Set(ByVal value As String)
                _ExposuerPerShipmentMessage = value
            End Set
        End Property

        Private _InsuranceDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property InsuranceDate() As System.Nullable(Of Date)
            Get
                Return _InsuranceDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _InsuranceDate = value
            End Set
        End Property

        Private _ContractExpiresDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property ContractExpiresDate() As System.Nullable(Of Date)
            Get
                Return _ContractExpiresDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _ContractExpiresDate = value
            End Set
        End Property
#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New DoFinalizeAlerts
            instance = DirectCast(MemberwiseClone(), DoFinalizeAlerts)
            Return instance
        End Function

#End Region

    End Class
End Namespace
