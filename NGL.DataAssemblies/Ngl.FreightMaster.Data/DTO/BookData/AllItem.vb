Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class AllItem
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

        <DataMember()> _
        Public Property StopNumber() As Nullable(Of Integer)
            Get
                Return m_StopNumber
            End Get
            Set(ByVal value As Nullable(Of Integer))
                m_StopNumber = value
            End Set
        End Property
        Private m_StopNumber As Nullable(Of Integer)

        <DataMember()> _
        Public Property PurchaseOrderNumber() As String
            Get
                Return m_PurchaseOrderNumber
            End Get
            Set(ByVal value As String)
                m_PurchaseOrderNumber = value
            End Set
        End Property
        Private m_PurchaseOrderNumber As String

        <DataMember()> _
        Public Property OrderNumber() As String
            Get
                Return m_OrderNumber
            End Get
            Set(ByVal value As String)
                m_OrderNumber = value
            End Set
        End Property
        Private m_OrderNumber As String

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
        Public Property RequestedToArrive() As Nullable(Of DateTime)
            Get
                Return m_RequestedToArrive
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                m_RequestedToArrive = value
            End Set
        End Property
        Private m_RequestedToArrive As Nullable(Of DateTime)


        Private _CarrierData As BookCarrier
        <DataMember()> Public Property CarrierData() As BookCarrier
            Get
                Return _CarrierData
            End Get
            Set(ByVal value As BookCarrier)
                _CarrierData = value
            End Set
        End Property


        ''Replace these datetime fields with the BookCarrierData object.               
        ''aan:>>
        ''Pickup Information
        '<DataMember()> _
        'Public Property PickupScheduledAppointmentDate() As DateTime
        '    Get
        '        Return m_PickupScheduledAppointmentDate
        '    End Get
        '    Set(ByVal value As DateTime)
        '        m_PickupScheduledAppointmentDate = value
        '    End Set
        'End Property
        'Private m_PickupScheduledAppointmentDate As DateTime
        ''aan: BookCarrScheduleDate

        '<DataMember()> Public Property PickupScheduledAppointmentTime() As DateTime
        '    Get
        '        Return m_PickupScheduledAppointmentTime
        '    End Get
        '    Set(ByVal value As DateTime)
        '        m_PickupScheduledAppointmentTime = value
        '    End Set
        'End Property
        'Private m_PickupScheduledAppointmentTime As DateTime

        ''aan: BookCarrScheduleTime
        '<DataMember()> Public Property PickupActualArrivalDate() As DateTime
        '    Get
        '        Return m_PickupActualArrivalDate
        '    End Get
        '    Set(ByVal value As DateTime)
        '        m_PickupActualArrivalDate = value
        '    End Set
        'End Property
        'Private m_PickupActualArrivalDate As DateTime

        ''aan: BookCarrActualDate
        '<DataMember()> Public Property PickupActualArrivalTime() As DateTime
        '    Get
        '        Return m_PickupActualArrivalTime
        '    End Get
        '    Set(ByVal value As DateTime)
        '        m_PickupActualArrivalTime = value
        '    End Set
        'End Property
        'Private m_PickupActualArrivalTime As DateTime

        ''aan: BookCarrActualime
        '<DataMember()> Public Property PickupStartLoadingDate() As DateTime
        '    Get
        '        Return m_PickupStartLoadingDate
        '    End Get
        '    Set(ByVal value As DateTime)
        '        m_PickupStartLoadingDate = value
        '    End Set
        'End Property
        'Private m_PickupStartLoadingDate As DateTime

        ''aan: BookCarrStartLoadingDate
        '<DataMember()> Public Property PickupStartLoadingTime() As DateTime
        '    Get
        '        Return m_PickupStartLoadingTime
        '    End Get
        '    Set(ByVal value As DateTime)
        '        m_PickupStartLoadingTime = value
        '    End Set
        'End Property
        'Private m_PickupStartLoadingTime As DateTime

        ''aan: BookCarrStartLoadingTime     
        '<DataMember()> Public Property PickupFinishLoadingDate() As DateTime
        '    Get
        '        Return m_PickupFinishLoadingDate
        '    End Get
        '    Set(ByVal value As DateTime)
        '        m_PickupFinishLoadingDate = value
        '    End Set
        'End Property
        'Private m_PickupFinishLoadingDate As DateTime

        ''aan: BookCarrFinishLoadingDate
        '<DataMember()> Public Property PickupFinishLoadingTime() As DateTime
        '    Get
        '        Return m_PickupFinishLoadingTime
        '    End Get
        '    Set(ByVal value As DateTime)
        '        m_PickupFinishLoadingTime = value
        '    End Set
        'End Property
        'Private m_PickupFinishLoadingTime As DateTime

        ''aan: BookCarrFinishLoadingTime
        '<DataMember()> Public Property PickupActLoadCompleteDate() As DateTime
        '    Get
        '        Return m_PickupActLoadCompleteDate
        '    End Get
        '    Set(ByVal value As DateTime)
        '        m_PickupActLoadCompleteDate = value
        '    End Set
        'End Property
        'Private m_PickupActLoadCompleteDate As DateTime

        ''aan: [BookCarrActLoadComplete Date]
        '<DataMember()> Public Property PickupActLoadCompleteTime() As DateTime
        '    Get
        '        Return m_PickupActLoadCompleteTime
        '    End Get
        '    Set(ByVal value As DateTime)
        '        m_PickupActLoadCompleteTime = value
        '    End Set
        'End Property
        'Private m_PickupActLoadCompleteTime As DateTime

        ''aan: BookCarrActLoadCompleteTime
        '<DataMember()> Public Property PickupDockPUAssignment() As String
        '    Get
        '        Return m_PickupDockPUAssignment
        '    End Get
        '    Set(ByVal value As String)
        '        m_PickupDockPUAssignment = value
        '    End Set
        'End Property
        'Private m_PickupDockPUAssignment As String

        ''aan: BookCarrDockPUAssigment    
        ''Delivery Information
        '<DataMember()> Public Property DeliveryScheduledAppointmentDate() As DateTime
        '    Get
        '        Return m_DeliveryScheduledAppointmentDate
        '    End Get
        '    Set(ByVal value As DateTime)
        '        m_DeliveryScheduledAppointmentDate = value
        '    End Set
        'End Property
        'Private m_DeliveryScheduledAppointmentDate As DateTime

        ''aan: BookCarrApptDate
        '<DataMember()> Public Property DeliveryScheduledAppointmentTime() As DateTime
        '    Get
        '        Return m_DeliveryScheduledAppointmentTime
        '    End Get
        '    Set(ByVal value As DateTime)
        '        m_DeliveryScheduledAppointmentTime = value
        '    End Set
        'End Property
        'Private m_DeliveryScheduledAppointmentTime As DateTime

        ''aan: BookCarrApptTime
        '<DataMember()> Public Property DeliveryActualArrivalDate() As DateTime
        '    Get
        '        Return m_DeliveryActualArrivalDate
        '    End Get
        '    Set(ByVal value As DateTime)
        '        m_DeliveryActualArrivalDate = value
        '    End Set
        'End Property
        'Private m_DeliveryActualArrivalDate As DateTime

        ''aan: BookCarrActDate
        '<DataMember()> Public Property DeliveryActualArrivalTime() As DateTime
        '    Get
        '        Return m_DeliveryActualArrivalTime
        '    End Get
        '    Set(ByVal value As DateTime)
        '        m_DeliveryActualArrivalTime = value
        '    End Set
        'End Property
        'Private m_DeliveryActualArrivalTime As DateTime

        ''aan: BookCarrActTime
        '<DataMember()> Public Property DeliveryStartUnloadingDate() As DateTime
        '    Get
        '        Return m_DeliveryStartUnloadingDate
        '    End Get
        '    Set(ByVal value As DateTime)
        '        m_DeliveryStartUnloadingDate = value
        '    End Set
        'End Property
        'Private m_DeliveryStartUnloadingDate As DateTime

        ''aan: BookCarrStartUnloadingDate
        '<DataMember()> Public Property DeliveryStartUnloadingTime() As DateTime
        '    Get
        '        Return m_DeliveryStartUnloadingTime
        '    End Get
        '    Set(ByVal value As DateTime)
        '        m_DeliveryStartUnloadingTime = value
        '    End Set
        'End Property
        'Private m_DeliveryStartUnloadingTime As DateTime

        ''aan: BookCarrStartUnloadingTime       
        '<DataMember()> Public Property DeliveryFinishUnloadingDate() As DateTime
        '    Get
        '        Return m_DeliveryFinishUnloadingDate
        '    End Get
        '    Set(ByVal value As DateTime)
        '        m_DeliveryFinishUnloadingDate = value
        '    End Set
        'End Property
        'Private m_DeliveryFinishUnloadingDate As DateTime

        ''aan: BookCarrFinishUnloadingDate
        '<DataMember()> Public Property DeliveryFinishUnloadingTime() As DateTime
        '    Get
        '        Return m_DeliveryFinishUnloadingTime
        '    End Get
        '    Set(ByVal value As DateTime)
        '        m_DeliveryFinishUnloadingTime = value
        '    End Set
        'End Property
        'Private m_DeliveryFinishUnloadingTime As DateTime

        ''aan: BookCarrFinishUnloadingTime
        '<DataMember()> Public Property DeliveryActUnloadCompDate() As DateTime
        '    Get
        '        Return m_DeliveryActUnloadCompDate
        '    End Get
        '    Set(ByVal value As DateTime)
        '        m_DeliveryActUnloadCompDate = value
        '    End Set
        'End Property
        'Private m_DeliveryActUnloadCompDate As DateTime
        ''aan: BookCarrActUnloadCompDate
        '<DataMember()> Public Property DeliveryActUnloadCompTime() As DateTime
        '    Get
        '        Return m_DeliveryActUnloadCompTime
        '    End Get
        '    Set(ByVal value As DateTime)
        '        m_DeliveryActUnloadCompTime = value
        '    End Set
        'End Property
        'Private m_DeliveryActUnloadCompTime As DateTime
        ''aan: BookCarrActUnloadCompTime
        '<DataMember()> Public Property DeliveryDockDelAssignment() As String
        '    Get
        '        Return m_DeliveryDockDelAssignment
        '    End Get
        '    Set(ByVal value As String)
        '        m_DeliveryDockDelAssignment = value
        '    End Set
        'End Property
        'Private m_DeliveryDockDelAssignment As String
        ''aan: BookCarrDockDelAssignment     
        ''Trailer Information
        '<DataMember()> Public Property TrailerNumber() As String
        '    Get
        '        Return m_TrailerNumber
        '    End Get
        '    Set(ByVal value As String)
        '        m_TrailerNumber = value
        '    End Set
        'End Property
        'Private m_TrailerNumber As String
        ''aan: BookCarrTrailerNo
        '<DataMember()> Public Property SealNumber() As String
        '    Get
        '        Return m_SealNumber
        '    End Get
        '    Set(ByVal value As String)
        '        m_SealNumber = value
        '    End Set
        'End Property
        'Private m_SealNumber As String
        ''aan: BookCarrSealNo  
        '<DataMember()> Public Property DriverNumber() As String
        '    Get
        '        Return m_DriverNumber
        '    End Get
        '    Set(ByVal value As String)
        '        m_DriverNumber = value
        '    End Set
        'End Property
        'Private m_DriverNumber As String
        ''aan: BookCarrDriverNo
        '<DataMember()> Public Property DriverName() As String
        '    Get
        '        Return m_DriverName
        '    End Get
        '    Set(ByVal value As String)
        '        m_DriverName = value
        '    End Set
        'End Property
        'Private m_DriverName As String
        ''aan: BookCarrDriverName
        '<DataMember()> Public Property TripNumber() As String
        '    Get
        '        Return m_TripNumber
        '    End Get
        '    Set(ByVal value As String)
        '        m_TripNumber = value
        '    End Set
        'End Property
        'Private m_TripNumber As String
        ''aan: BookCarrTripNo
        '<DataMember()> Public Property RouteNumber() As String
        '    Get
        '        Return m_RouteNumber
        '    End Get
        '    Set(ByVal value As String)
        '        m_RouteNumber = value
        '    End Set
        'End Property
        'Private m_RouteNumber As String
        ''aan: BookCarrRouteNo
        ''Warehouse information
        '<DataMember()> Public Property WhseAuthorizationNumber() As String
        '    Get
        '        Return m_WhseAuthorizationNumber
        '    End Get
        '    Set(ByVal value As String)
        '        m_WhseAuthorizationNumber = value
        '    End Set
        'End Property
        'Private m_WhseAuthorizationNumber As String
        ''aan: BookWhseAuthorizationNo
        ''aan:<<
        <DataMember()> Public Property AssignedCarrier() As String
            Get
                Return m_AssignedCarrier
            End Get
            Set(ByVal value As String)
                m_AssignedCarrier = value
            End Set
        End Property
        Private m_AssignedCarrier As String

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
        <DataMember()> Public Property Comments() As String
            Get
                Return m_Comments
            End Get
            Set(ByVal value As String)
                m_Comments = value
            End Set
        End Property
        Private m_Comments As String
        'aan:>>
        <DataMember()> Public Property Status() As Integer
            Get
                Return m_Status
            End Get
            Set(ByVal value As Integer)
                m_Status = value
            End Set
        End Property
        Private m_Status As Integer
        'aan:<<
        <DataMember()> Public Property BookNotes1() As String
            Get
                Return m_BookNotes1
            End Get
            Set(ByVal value As String)
                m_BookNotes1 = value
            End Set
        End Property
        Private m_BookNotes1 As String
        <DataMember()> Public Property BookNotes2() As String
            Get
                Return m_BookNotes2
            End Get
            Set(ByVal value As String)
                m_BookNotes2 = value
            End Set
        End Property
        Private m_BookNotes2 As String
        <DataMember()> Public Property BookNotes3() As String
            Get
                Return m_BookNotes3
            End Get
            Set(ByVal value As String)
                m_BookNotes3 = value
            End Set
        End Property
        Private m_BookNotes3 As String

        <DataMember()> Public Property AssignedProNumber() As String
            Get
                Return m_AssignedProNumber
            End Get
            Set(ByVal value As String)
                m_AssignedProNumber = value
            End Set
        End Property
        Private m_AssignedProNumber As String


        Private _BookShipCarrierProNumberRaw As String = ""
        <DataMember()> _
        Public Property BookShipCarrierProNumberRaw() As String
            Get
                Return Left(_BookShipCarrierProNumberRaw, 20)
            End Get
            Set(ByVal value As String)
                _BookShipCarrierProNumberRaw = Left(value, 20)
            End Set
        End Property

        Private _BookShipCarrierProControl As System.Nullable(Of Integer)
        <DataMember()> _
        Public Property BookShipCarrierProControl() As System.Nullable(Of Integer)
            Get
                Return _BookShipCarrierProControl
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                _BookShipCarrierProControl = value
            End Set
        End Property

        <DataMember()> Public Property AssignedCarrierNumber() As String
            Get
                Return m_AssignedCarrierNumber
            End Get
            Set(ByVal value As String)
                m_AssignedCarrierNumber = value
            End Set
        End Property
        Private m_AssignedCarrierNumber As String
        <DataMember()> Public Property AssignedCarrierName() As String
            Get
                Return m_AssignedCarrierName
            End Get
            Set(ByVal value As String)
                m_AssignedCarrierName = value
            End Set
        End Property
        Private m_AssignedCarrierName As String
        'aan:>>
        <DataMember()> Public Property AssignedCarrierContact() As String
            Get
                Return m_AssignedCarrierContact
            End Get
            Set(ByVal value As String)
                m_AssignedCarrierContact = value
            End Set
        End Property
        Private m_AssignedCarrierContact As String
        <DataMember()> Public Property AssignedCarrierContactPhone() As String
            Get
                Return m_AssignedCarrierContactPhone
            End Get
            Set(ByVal value As String)
                m_AssignedCarrierContactPhone = value
            End Set
        End Property
        Private m_AssignedCarrierContactPhone As String

        Private _BookPickupStopNumber As Integer
        <DataMember()> Public Property BookPickupStopNumber() As Integer
            Get
                Return _BookPickupStopNumber
            End Get
            Set(ByVal value As Integer)
                _BookPickupStopNumber = value
            End Set
        End Property

        <DataMember()> _
        Public Property ApplyToAllDestinations() As Boolean
            Get
                Return m_ApplyToAllDestinations
            End Get
            Set(ByVal value As Boolean)
                m_ApplyToAllDestinations = value
            End Set
        End Property
        Private m_ApplyToAllDestinations As Boolean = False

        <DataMember()> _
        Public Property ApplyToAllPickups() As Boolean
            Get
                Return m_ApplyToAllPickups
            End Get
            Set(ByVal value As Boolean)
                m_ApplyToAllPickups = value
            End Set
        End Property
        Private m_ApplyToAllPickups As Boolean = False


        Private m_ApplyCommentsToCNS As Boolean = False
        <DataMember()> _
        Public Property ApplyCommentsToCNS() As Boolean
            Get
                Return m_ApplyCommentsToCNS
            End Get
            Set(ByVal value As Boolean)
                m_ApplyCommentsToCNS = value
            End Set
        End Property

        <DataMember()> _
        Public Property BookModDate() As Nullable(Of DateTime)
            Get
                Return m_ModDate
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                m_ModDate = value
            End Set
        End Property
        Private m_ModDate As Nullable(Of DateTime)

        <DataMember()> Public Property BookModUser() As String
            Get
                Return m_ModUSer
            End Get
            Set(ByVal value As String)
                m_ModUSer = value
            End Set
        End Property
        Private m_ModUSer As String

        Private _BookAMSPickupApptControl As Integer
        <DataMember()> _
        Public Property BookAMSPickupApptControl As Integer
            Get
                Return _BookAMSPickupApptControl
            End Get
            Set(value As Integer)
                _BookAMSPickupApptControl = value
            End Set
        End Property

        Private _BookAMSDeliveryApptControl As Integer
        <DataMember()> _
        Public Property BookAMSDeliveryApptControl As Integer
            Get
                Return _BookAMSDeliveryApptControl
            End Get
            Set(value As Integer)
                _BookAMSDeliveryApptControl = value
            End Set
        End Property

        Private _BookLoadControl As Integer = 0
        <DataMember()> _
        Public Property BookLoadControl As Integer
            Get
                Return _BookLoadControl
            End Get
            Set(value As Integer)
                _BookLoadControl = value
            End Set
        End Property

        'Added by LVV on 7/29/16 for v-7.0.5.110 Task #14 NxT Search Filters
        Private m_SHID As String
        <DataMember()> Public Property SHID() As String
            Get
                Return m_SHID
            End Get
            Set(ByVal value As String)
                m_SHID = value
            End Set
        End Property

        Private m_CarrierPro As String
        <DataMember()> Public Property CarrierPro() As String
            Get
                Return m_CarrierPro
            End Get
            Set(ByVal value As String)
                m_CarrierPro = value
            End Set
        End Property


        'Added By LVV on 9/19/19 for Bing Maps
        Private _CommentLocation As tblStop 'The location tag associated with a Comments message (optional)
        Private _CommentDate As Date? 'The Date associated with the location tag associated with a Comments message (optional)
        Private _CommentTime As Date? 'The Time associated with the location tag associated with a Comments message (optional)


#End Region

#Region " Public Methods"

        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New AllItem
            instance = DirectCast(MemberwiseClone(), AllItem)
            Return instance
        End Function

        ''' <summary>Get the location tag associated with a Comments message</summary>
        ''' <returns>DTO.tblStop</returns>
        ''' <remarks>Added By LVV on 9/19/19 for Bing Maps</remarks>
        Public Function getCommentLocation() As tblStop
            Return _CommentLocation
        End Function

        ''' <summary>Set the location tag associated with a Comments message</summary>
        ''' <param name="location"></param>
        ''' <remarks>Added By LVV on 9/19/19 for Bing Maps</remarks>
        Public Sub setCommentLocation(ByVal location As tblStop)
            _CommentLocation = location
        End Sub

        ''' <summary>Set the Date associated with the location tag associated with a Comments message</summary>
        ''' <param name="dt"></param> 
        ''' <remarks>Added By LVV on 10/9/19 for Bing Maps</remarks>
        Public Sub setCommentDate(ByVal dt As Date?)
            _CommentDate = dt
        End Sub

        ''' <summary>Get the Date associated with the location tag associated with a Comments message</summary>
        ''' <returns></returns>
        ''' <remarks>Added By LVV on 10/9/19 for Bing Maps</remarks>
        Public Function getCommentDate() As Date?
            Return _CommentDate
        End Function

        ''' <summary>Set the Time associated with the location tag associated with a Comments message</summary>
        ''' <param name="time"></param>
        ''' <remarks>Added By LVV on 10/9/19 for Bing Maps</remarks>
        Public Sub setCommentTime(ByVal time As Date?)
            _CommentTime = time
        End Sub

        ''' <summary>Get the Time associated with the location tag associated with a Comments message</summary>
        ''' <returns></returns>
        ''' <remarks>Added By LVV on 10/9/19 for Bing Maps</remarks>
        Public Function getCommentTime() As Date?
            Return _CommentTime
        End Function

#End Region

    End Class
End Namespace

