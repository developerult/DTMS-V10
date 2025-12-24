Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class AllItemLite
        Inherits DTOBaseClass

#Region " Data Members"

        <DataMember()> _
        Public Property Control() As Integer
            Get
                Return m_Control
            End Get
            Set(ByVal value As Integer)
                m_Control = value
            End Set
        End Property
        Private m_Control As Integer

        <DataMember()> _
        Public Property ProNumber() As String
            Get
                Return m_ProNumber
            End Get
            Set(ByVal value As String)
                m_ProNumber = value
            End Set
        End Property
        Private m_ProNumber As String

        <DataMember()> _
        Public Property CnsNumber() As String
            Get
                Return m_CnsNumber
            End Get
            Set(ByVal value As String)
                m_CnsNumber = value
            End Set
        End Property
        Private m_CnsNumber As String

        Private _OrderNumber As String = ""
        <DataMember()> _
        Public Property OrderNumber() As String
            Get
                Return _OrderNumber
            End Get
            Set(ByVal value As String)
                _OrderNumber = value
            End Set
        End Property

          
        Private _CarrierName As String = ""
        <DataMember()> _
        Public Property CarrierName() As String
            Get
                Return _CarrierName
            End Get
            Set(ByVal value As String)
                _CarrierName = value
            End Set
        End Property

        <DataMember()> _
        Public Property CarrierNumber() As Integer
            Get
                Return _CarrierNumber
            End Get
            Set(ByVal value As Integer)
                _CarrierNumber = value
            End Set
        End Property
        Private _CarrierNumber As Integer

        <DataMember()> _
        Public Property CarrierControl() As Integer
            Get
                Return m_CarrierControl
            End Get
            Set(ByVal value As Integer)
                m_CarrierControl = value
            End Set
        End Property
        Private m_CarrierControl As Integer

        <DataMember()> _
        Public Property ScheduledToLoad() As Nullable(Of DateTime)
            Get
                Return m_ScheduledToLoad
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                m_ScheduledToLoad = value
            End Set
        End Property
        Private m_ScheduledToLoad As Nullable(Of DateTime)

        <DataMember()> _
        Public Property PickupScheduledAppointmentDate() As Nullable(Of DateTime)
            Get
                Return m_PickupScheduledAppointmentDate
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                m_PickupScheduledAppointmentDate = value
            End Set
        End Property
        Private m_PickupScheduledAppointmentDate As Nullable(Of DateTime)

        <DataMember()> Public Property DestinationName() As String
            Get
                Return m_DestinationName
            End Get
            Set(ByVal value As String)
                m_DestinationName = value
            End Set
        End Property
        Private m_DestinationName As String
        <DataMember()> Public Property DestinationCity() As String
            Get
                Return m_DestinationCity
            End Get
            Set(ByVal value As String)
                m_DestinationCity = value
            End Set
        End Property
        Private m_DestinationCity As String
        <DataMember()> Public Property DestinationState() As String
            Get
                Return m_DestinationState
            End Get
            Set(ByVal value As String)
                m_DestinationState = value
            End Set
        End Property
        Private m_DestinationState As String

        <DataMember()> Public Property OrigName() As String
            Get
                Return m_OrigName
            End Get
            Set(ByVal value As String)
                m_OrigName = value
            End Set
        End Property
        Private m_OrigName As String
        <DataMember()> Public Property OrigCity() As String
            Get
                Return m_OrigCity
            End Get
            Set(ByVal value As String)
                m_OrigCity = value
            End Set
        End Property
        Private m_OrigCity As String
        <DataMember()> Public Property OrigState() As String
            Get
                Return m_OrigState
            End Get
            Set(ByVal value As String)
                m_OrigState = value
            End Set
        End Property
        Private m_OrigState As String

        <DataMember()> Public Property PickupStartLoadingDate() As Nullable(Of DateTime)
            Get
                Return m_PickupStartLoadingDate
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                m_PickupStartLoadingDate = value
            End Set
        End Property
        Private m_PickupStartLoadingDate As Nullable(Of DateTime)

        'aan: BookCarrStartLoadingDate
        <DataMember()> Public Property PickupStartLoadingTime() As Nullable(Of DateTime)
            Get
                Return m_PickupStartLoadingTime
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                m_PickupStartLoadingTime = value
            End Set
        End Property
        Private m_PickupStartLoadingTime As Nullable(Of DateTime)

        'aan: BookCarrStartLoadingTime     
        <DataMember()> Public Property PickupFinishLoadingDate() As Nullable(Of DateTime)
            Get
                Return m_PickupFinishLoadingDate
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                m_PickupFinishLoadingDate = value
            End Set
        End Property
        Private m_PickupFinishLoadingDate As Nullable(Of DateTime)

        'aan: BookCarrFinishLoadingDate
        <DataMember()> Public Property PickupFinishLoadingTime() As Nullable(Of DateTime)
            Get
                Return m_PickupFinishLoadingTime
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                m_PickupFinishLoadingTime = value
            End Set
        End Property
        Private m_PickupFinishLoadingTime As Nullable(Of DateTime)

        <DataMember()> Public Property DeliveryStartUnloadingDate() As Nullable(Of DateTime)
            Get
                Return m_DeliveryStartUnloadingDate
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                m_DeliveryStartUnloadingDate = value
            End Set
        End Property
        Private m_DeliveryStartUnloadingDate As Nullable(Of DateTime)

        'aan: BookCarrStartUnloadingDate
        <DataMember()> Public Property DeliveryStartUnloadingTime() As Nullable(Of DateTime)
            Get
                Return m_DeliveryStartUnloadingTime
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                m_DeliveryStartUnloadingTime = value
            End Set
        End Property
        Private m_DeliveryStartUnloadingTime As Nullable(Of DateTime)

        'aan: BookCarrStartUnloadingTime       
        <DataMember()> Public Property DeliveryFinishUnloadingDate() As Nullable(Of DateTime)
            Get
                Return m_DeliveryFinishUnloadingDate
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                m_DeliveryFinishUnloadingDate = value
            End Set
        End Property
        Private m_DeliveryFinishUnloadingDate As Nullable(Of DateTime)

        'aan: BookCarrFinishUnloadingDate
        <DataMember()> Public Property DeliveryFinishUnloadingTime() As Nullable(Of DateTime)
            Get
                Return m_DeliveryFinishUnloadingTime
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                m_DeliveryFinishUnloadingTime = value
            End Set
        End Property
        Private m_DeliveryFinishUnloadingTime As Nullable(Of DateTime)



        <DataMember()> Public Property PickupDockPUAssignment() As String
            Get
                Return m_PickupDockPUAssignment
            End Get
            Set(ByVal value As String)
                m_PickupDockPUAssignment = value
            End Set
        End Property
        Private m_PickupDockPUAssignment As String

        <DataMember()> Public Property WhseAuthorizationNumber() As String
            Get
                Return m_WhseAuthorizationNumber
            End Get
            Set(ByVal value As String)
                m_WhseAuthorizationNumber = value
            End Set
        End Property
        Private m_WhseAuthorizationNumber As String

#End Region

#Region " Public Methods"

        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New AllItem
            instance = DirectCast(MemberwiseClone(), AllItem)
            Return instance
        End Function

#End Region

    End Class
End Namespace

