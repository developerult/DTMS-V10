Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class CompDockDoor
        Inherits DTOBaseClass


#Region " Data Members"
        Private _CompDockControl As Integer = 0
        <DataMember()> _
        Public Property CompDockControl() As Integer
            Get
                Return _CompDockControl
            End Get
            Set(ByVal value As Integer)
                _CompDockControl = value
            End Set
        End Property

        Private _CompDockCompControl As Integer = 0
        <DataMember()> _
        Public Property CompDockCompControl() As Integer
            Get
                Return _CompDockCompControl
            End Get
            Set(ByVal value As Integer)
                _CompDockCompControl = value
            End Set
        End Property

        Private _CompDockDockDoorID As String = ""
        ''' <summary>
        ''' Text Reference to the Company Dock Door
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Modified by RHR for v-8.2.1 on 10/16/2019 
        '''     changed logic to trim string to 20 characters as the 
        '''     table only supports 20 characters not 50
        ''' </remarks>
        <DataMember()> _
        Public Property CompDockDockDoorID() As String
            Get
                Return Left(_CompDockDockDoorID, 20)
            End Get
            Set(ByVal value As String)
                _CompDockDockDoorID = Left(value, 20)
            End Set
        End Property

        Private _CompDockDockDoorName As String = ""
        <DataMember()> _
        Public Property CompDockDockDoorName() As String
            Get
                Return Left(_CompDockDockDoorName, 20)
            End Get
            Set(ByVal value As String)
                _CompDockDockDoorName = Left(value, 20)
            End Set
        End Property

        Private _CompDockDoorModUser As String = ""
        <DataMember()> _
        Public Property CompDockDoorModUser() As String
            Get
                Return Left(_CompDockDoorModUser, 100)
            End Get
            Set(ByVal value As String)
                _CompDockDoorModUser = Left(value, 100)
            End Set
        End Property

        Private _CompDockDoorModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CompDockDoorModDate() As System.Nullable(Of Date)
            Get
                Return _CompDockDoorModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _CompDockDoorModDate = value
            End Set
        End Property

        Private _CompDockDoorUpdated As Byte()
        <DataMember()> _
        Public Property CompDockDoorUpdated() As Byte()
            Get
                Return _CompDockDoorUpdated
            End Get
            Set(ByVal value As Byte())
                _CompDockDoorUpdated = value
            End Set
        End Property

        'Added By LVV on 6/27/18 for v-8.3 TMS365 Scheduler

        Private _CompDockBookingSeq As Integer = 0
        <DataMember()>
        Public Property CompDockBookingSeq() As Integer
            Get
                Return _CompDockBookingSeq
            End Get
            Set(ByVal value As Integer)
                _CompDockBookingSeq = value
            End Set
        End Property

        Private _CompDockValidation As Boolean = True
        <DataMember()>
        Public Property CompDockValidation() As Boolean
            Get
                Return _CompDockValidation
            End Get
            Set(ByVal value As Boolean)
                _CompDockValidation = value
            End Set
        End Property

        Private _CompDockOverrideAlert As Boolean = True
        <DataMember()>
        Public Property CompDockOverrideAlert() As Boolean
            Get
                Return _CompDockOverrideAlert
            End Get
            Set(ByVal value As Boolean)
                _CompDockOverrideAlert = value
            End Set
        End Property

        Private _CompDockOverridePwd As String = ""
        <DataMember()>
        Public Property CompDockOverridePwd() As String
            Get
                Return Left(_CompDockOverridePwd, 100)
            End Get
            Set(ByVal value As String)
                _CompDockOverridePwd = Left(value, 100)
            End Set
        End Property

        Private _CompDockNotificationAlert As Boolean = True
        <DataMember()>
        Public Property CompDockNotificationAlert() As Boolean
            Get
                Return _CompDockNotificationAlert
            End Get
            Set(ByVal value As Boolean)
                _CompDockNotificationAlert = value
            End Set
        End Property

        Private _CompDockNotificationEmail As String = ""
        <DataMember()>
        Public Property CompDockNotificationEmail() As String
            Get
                Return Left(_CompDockNotificationEmail, 100)
            End Get
            Set(ByVal value As String)
                _CompDockNotificationEmail = Left(value, 100)
            End Set
        End Property

        Private _AvgApptTime As Integer
        <DataMember()>
        Public Property AvgApptTime() As Integer
            Get
                Return _AvgApptTime
            End Get
            Set(ByVal value As Integer)
                _AvgApptTime = value
            End Set
        End Property

        'Added By LVV on 3/5/20 Scheduler Inbound/Outbound Dock Enhancement
        Private _CompDockInbound As Boolean = False
        <DataMember()>
        Public Property CompDockInbound() As Boolean
            Get
                Return _CompDockInbound
            End Get
            Set(ByVal value As Boolean)
                _CompDockInbound = value
            End Set
        End Property


#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CompDockDoor
            instance = DirectCast(MemberwiseClone(), CompDockDoor)
           
            Return instance
        End Function

#End Region

    End Class
End Namespace
