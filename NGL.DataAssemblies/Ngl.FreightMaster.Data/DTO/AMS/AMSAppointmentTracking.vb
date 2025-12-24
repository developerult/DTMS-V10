Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class AMSAppointmentTracking
        Inherits DTOBaseClass

#Region " Data Members"


        Private _AMSApptTrackingControl As Integer
        <DataMember()> _
        Public Property AMSApptTrackingControl() As Integer
            Get
                Return Me._AMSApptTrackingControl
            End Get
            Set(value As Integer)
                If ((Me._AMSApptTrackingControl = value) _
                   = False) Then
                    Me._AMSApptTrackingControl = value
                    Me.SendPropertyChanged("AMSApptTrackingControl")
                End If
            End Set
        End Property

        Private _AMSApptTrackingApptControl As Integer
        <DataMember()> _
        Public Property AMSApptTrackingApptControl As Integer
            Get
                Return _AMSApptTrackingApptControl
            End Get
            Set(value As Integer)
                If ((Me._AMSApptTrackingApptControl = value) _
                   = False) Then
                    Me._AMSApptTrackingApptControl = value
                    Me.SendPropertyChanged("AMSApptTrackingApptControl")
                End If
            End Set
        End Property

        Private _AMSApptTrackingCompAMSApptTrackingSettingControl As Integer
        <DataMember()> _
        Public Property AMSApptTrackingCompAMSApptTrackingSettingControl As Integer
            Get
                Return _AMSApptTrackingCompAMSApptTrackingSettingControl
            End Get
            Set(value As Integer)
                If ((Me._AMSApptTrackingCompAMSApptTrackingSettingControl = value) _
                   = False) Then
                    Me._AMSApptTrackingCompAMSApptTrackingSettingControl = value
                    Me.SendPropertyChanged("AMSApptTrackingCompAMSApptTrackingSettingControl")
                End If
            End Set
        End Property

        Private _AMSApptTrackingName As String = ""
        <DataMember()> _
        Public Property AMSApptTrackingName() As String
            Get
                Return Left(_AMSApptTrackingName, 20)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._AMSApptTrackingName, value) = False) Then
                    Me._AMSApptTrackingName = Left(value, 20)
                    Me.SendPropertyChanged("AMSApptTrackingName")
                End If
            End Set
        End Property

        Private _AMSApptTrackingDesc As String = ""
        <DataMember()> _
        Public Property AMSApptTrackingDesc() As String
            Get
                Return Left(_AMSApptTrackingDesc, 255)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._AMSApptTrackingDesc, value) = False) Then
                    Me._AMSApptTrackingDesc = Left(value, 255)
                    Me.SendPropertyChanged("AMSApptTrackingDesc")
                End If
            End Set
        End Property


        Private _AMSApptTrackingDateTime As System.Nullable(Of Date)
        <DataMember()> _
        Public Property AMSApptTrackingDateTime() As System.Nullable(Of Date)
            Get
                Return Me._AMSApptTrackingDateTime
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._AMSApptTrackingDateTime.Equals(value) = False) Then
                    Me._AMSApptTrackingDateTime = value
                    Me.SendPropertyChanged("AMSApptTrackingDateTime")
                End If
            End Set
        End Property


        Private _AMSApptTrackingModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property AMSApptTrackingModDate() As System.Nullable(Of Date)
            Get
                Return Me._AMSApptTrackingModDate
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._AMSApptTrackingModDate.Equals(value) = False) Then
                    Me._AMSApptTrackingModDate = value
                    Me.SendPropertyChanged("AMSApptTrackingModDate")
                End If
            End Set
        End Property

        Private _AMSApptTrackingModUser As String
        <DataMember()> _
        Public Property AMSApptTrackingModUser() As String
            Get
                Return Left(Me._AMSApptTrackingModUser, 100)
            End Get
            Set(value As String)
                If (String.Equals(Me._AMSApptTrackingModUser, value) = False) Then
                    Me._AMSApptTrackingModUser = Left(value, 100)
                    Me.SendPropertyChanged("AMSApptTrackingModUser")
                End If
            End Set
        End Property

        Private _AMSApptTrackingUpdated As Byte()
        <DataMember()> _
        Public Property AMSApptTrackingUpdated() As Byte()
            Get
                Return _AMSApptTrackingUpdated
            End Get
            Set(ByVal value As Byte())
                _AMSApptTrackingUpdated = value
            End Set
        End Property


#End Region



#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New AMSAppointmentTracking
            instance = DirectCast(MemberwiseClone(), AMSAppointmentTracking)
            Return instance
        End Function

#End Region
    End Class
End Namespace
