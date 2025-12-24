Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class ELMDetail
        Inherits DTOBaseClass


#Region " Data Members"

        Private _ELMControl As Integer = 0
        <DataMember()> _
        Public Property ELMControl() As Integer
            Get
                Return _ELMControl
            End Get
            Set(ByVal value As Integer)
                _ELMControl = value
            End Set
        End Property

        Private _ELMInitial As String = ""
        <DataMember()> _
        Public Property ELMInitial() As String
            Get
                Return Left(_ELMInitial, 100)
            End Get
            Set(ByVal value As String)
                _ELMInitial = Left(value, 100)
            End Set
        End Property

        Private _ELMNumber As String = ""
        <DataMember()> _
        Public Property ELMNumber() As String
            Get
                Return Left(_ELMNumber, 100)
            End Get
            Set(ByVal value As String)
                _ELMNumber = Left(value, 100)
            End Set
        End Property

        Private _ELMSentDateTime As System.Nullable(Of Date)
        <DataMember()> _
        Public Property ELMSentDateTime() As System.Nullable(Of Date)
            Get
                Return _ELMSentDateTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _ELMSentDateTime = value
            End Set
        End Property

        Private _ELMSightingCity As String = ""
        <DataMember()> _
        Public Property ELMSightingCity() As String
            Get
                Return Left(_ELMSightingCity, 100)
            End Get
            Set(ByVal value As String)
                _ELMSightingCity = Left(value, 100)
            End Set
        End Property

        Private _ELMSightingStateProvinceCountry As String = ""
        <DataMember()> _
        Public Property ELMSightingStateProvinceCountry() As String
            Get
                Return Left(_ELMSightingStateProvinceCountry, 100)
            End Get
            Set(ByVal value As String)
                _ELMSightingStateProvinceCountry = Left(value, 100)
            End Set
        End Property

        Private _ELMSightingDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property ELMSightingDate() As System.Nullable(Of Date)
            Get
                Return _ELMSightingDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _ELMSightingDate = value
            End Set
        End Property

        Private _ELMSightingHour As String = ""
        <DataMember()> _
        Public Property ELMSightingHour() As String
            Get
                Return Left(_ELMSightingHour, 100)
            End Get
            Set(ByVal value As String)
                _ELMSightingHour = Left(value, 100)
            End Set
        End Property

        Private _ELMSightingMinute As String = ""
        <DataMember()> _
        Public Property ELMSightingMinute() As String
            Get
                Return Left(_ELMSightingMinute, 100)
            End Get
            Set(ByVal value As String)
                _ELMSightingMinute = Left(value, 100)
            End Set
        End Property

        Private _ELMSightingSPLC As String = ""
        <DataMember()> _
        Public Property ELMSightingSPLC() As String
            Get
                Return Left(_ELMSightingSPLC, 100)
            End Get
            Set(ByVal value As String)
                _ELMSightingSPLC = Left(value, 100)
            End Set
        End Property

        Private _ELMStatus As String = ""
        <DataMember()> _
        Public Property ELMStatus() As String
            Get
                Return Left(_ELMStatus, 100)
            End Get
            Set(ByVal value As String)
                _ELMStatus = Left(value, 100)
            End Set
        End Property

        Private _ELMSightingEvent As String = ""
        <DataMember()> _
        Public Property ELMSightingEvent() As String
            Get
                Return Left(_ELMSightingEvent, 100)
            End Get
            Set(ByVal value As String)
                _ELMSightingEvent = Left(value, 100)
            End Set
        End Property

        Private _ELMDestinationCity As String = ""
        <DataMember()> _
        Public Property ELMDestinationCity() As String
            Get
                Return Left(_ELMDestinationCity, 100)
            End Get
            Set(ByVal value As String)
                _ELMDestinationCity = Left(value, 100)
            End Set
        End Property

        Private _ELMDestinationStateProvinceCountry As String = ""
        <DataMember()> _
        Public Property ELMDestinationStateProvinceCountry() As String
            Get
                Return Left(_ELMDestinationStateProvinceCountry, 100)
            End Get
            Set(ByVal value As String)
                _ELMDestinationStateProvinceCountry = Left(value, 100)
            End Set
        End Property

        Private _ELMIDCode As String = ""
        <DataMember()> _
        Public Property ELMIDCode() As String
            Get
                Return Left(_ELMIDCode, 100)
            End Get
            Set(ByVal value As String)
                _ELMIDCode = Left(value, 100)
            End Set
        End Property

        Private _ELMReportingSCAC As String = ""
        <DataMember()> _
        Public Property ELMReportingSCAC() As String
            Get
                Return Left(_ELMReportingSCAC, 100)
            End Get
            Set(ByVal value As String)
                _ELMReportingSCAC = Left(value, 100)
            End Set
        End Property

        Private _ELMAEIReidIndicator As String = ""
        <DataMember()> _
        Public Property ELMAEIReidIndicator() As String
            Get
                Return Left(_ELMAEIReidIndicator, 100)
            End Get
            Set(ByVal value As String)
                _ELMAEIReidIndicator = Left(value, 100)
            End Set
        End Property

        Private _ELMETADestinationCity As String = ""
        <DataMember()> _
        Public Property ELMETADestinationCity() As String
            Get
                Return Left(_ELMETADestinationCity, 100)
            End Get
            Set(ByVal value As String)
                _ELMETADestinationCity = Left(value, 100)
            End Set
        End Property

        Private _ELMETADestinationStateProvinceCountry As String = ""
        <DataMember()> _
        Public Property ELMETADestinationStateProvinceCountry() As String
            Get
                Return Left(_ELMETADestinationStateProvinceCountry, 100)
            End Get
            Set(ByVal value As String)
                _ELMETADestinationStateProvinceCountry = Left(value, 100)
            End Set
        End Property

        Private _ELMETADateTime As System.Nullable(Of Date)
        <DataMember()> _
        Public Property ELMETADateTime() As System.Nullable(Of Date)
            Get
                Return _ELMETADateTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _ELMETADateTime = value
            End Set
        End Property

        Private _ELMEventTypeCode As String = ""
        <DataMember()> _
        Public Property ELMEventTypeCode() As String
            Get
                Return Left(_ELMEventTypeCode, 100)
            End Get
            Set(ByVal value As String)
                _ELMEventTypeCode = Left(value, 100)
            End Set
        End Property

        Private _ELMGrossWeight As String = ""
        <DataMember()> _
        Public Property ELMGrossWeight() As String
            Get
                Return Left(_ELMGrossWeight, 100)
            End Get
            Set(ByVal value As String)
                _ELMGrossWeight = Left(value, 100)
            End Set
        End Property

        Private _ELMTareWeight As String = ""
        <DataMember()> _
        Public Property ELMTareWeight() As String
            Get
                Return Left(_ELMTareWeight, 100)
            End Get
            Set(ByVal value As String)
                _ELMTareWeight = Left(value, 100)
            End Set
        End Property

        Private _ELMNETWeight As String
        <DataMember()> _
        Public Property ELMNETWeight() As String
            Get
                Return Left(_ELMNETWeight, 100)
            End Get
            Set(ByVal value As String)
                _ELMNETWeight = Left(value, 100)
            End Set
        End Property

        Private _ELMWeightCode As String = ""
        <DataMember()> _
        Public Property ELMWeightCode() As String
            Get
                Return Left(_ELMWeightCode, 100)
            End Get
            Set(ByVal value As String)
                _ELMWeightCode = Left(value, 100)
            End Set
        End Property

        Private _ELMWeightDateTime As System.Nullable(Of Date)
        <DataMember()> _
        Public Property ELMWeightDateTime() As System.Nullable(Of Date)
            Get
                Return _ELMWeightDateTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _ELMWeightDateTime = value
            End Set
        End Property

        Private _ELMWeightLocationCity As String = ""
        <DataMember()> _
        Public Property ELMWeightLocationCity() As String
            Get
                Return Left(_ELMWeightLocationCity, 100)
            End Get
            Set(ByVal value As String)
                _ELMWeightLocationCity = Left(value, 100)
            End Set
        End Property

        Private _ELMWeightLocationStateProvinceCountry As String = ""
        <DataMember()> _
        Public Property ELMWeightLocationStateProvinceCountry() As String
            Get
                Return Left(_ELMWeightLocationStateProvinceCountry, 100)
            End Get
            Set(ByVal value As String)
                _ELMWeightLocationStateProvinceCountry = Left(value, 100)
            End Set
        End Property

        Private _ELMWayBillDateTime As System.Nullable(Of Date)
        <DataMember()> _
        Public Property ELMWayBillDateTime() As System.Nullable(Of Date)
            Get
                Return _ELMWayBillDateTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _ELMWayBillDateTime = value
            End Set
        End Property

        Private _ELMWeightIndicator As String = ""
        <DataMember()> _
        Public Property ELMWeightIndicator() As String
            Get
                Return Left(_ELMWeightIndicator, 100)
            End Get
            Set(ByVal value As String)
                _ELMWeightIndicator = Left(value, 100)
            End Set
        End Property

        Private _ELMAllowance As String = ""
        <DataMember()> _
        Public Property ELMAllowance() As String
            Get
                Return Left(_ELMAllowance, 100)
            End Get
            Set(ByVal value As String)
                _ELMAllowance = Left(value, 100)
            End Set
        End Property


        Private _ELMBookControl As Integer = 0
        <DataMember()> _
        Public Property ELMBookControl() As Integer
            Get
                Return _ELMBookControl
            End Get
            Set(ByVal value As Integer)
                _ELMBookControl = value
            End Set
        End Property

        Private _ELMActionModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property ELMActionModDate() As System.Nullable(Of Date)
            Get
                Return _ELMActionModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _ELMActionModDate = value
            End Set
        End Property

        Private _ELMActionModUser As String = ""
        <DataMember()> _
        Public Property ELMActionModUser() As String
            Get
                Return Left(_ELMActionModUser, 100)
            End Get
            Set(ByVal value As String)
                _ELMActionModUser = Left(value, 100)
            End Set
        End Property

        Private _ELMActionUpdated As Byte()
        <DataMember()> _
        Public Property ELMActionUpdated() As Byte()
            Get
                Return _ELMActionUpdated
            End Get
            Set(ByVal value As Byte())
                _ELMActionUpdated = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New ELMDetail
            instance = DirectCast(MemberwiseClone(), ELMDetail)
            Return instance
        End Function

#End Region

    End Class
End Namespace

