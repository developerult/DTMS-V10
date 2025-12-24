Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class AMSAppointment
        Inherits DTOBaseClass

#Region " Data Members"


        Private _AMSApptControl As Integer
        <DataMember()> _
        Public Property AMSApptControl() As Integer
            Get
                Return Me._AMSApptControl
            End Get
            Set(value As Integer)
                If ((Me._AMSApptControl = value) _
                   = False) Then
                    Me._AMSApptControl = value
                    Me.SendPropertyChanged("AMSApptControl")
                End If
            End Set
        End Property

        Private _AMSApptCompControl As Integer
        <DataMember()> _
        Public Property AMSApptCompControl As Integer
            Get
                Return _AMSApptCompControl
            End Get
            Set(value As Integer)
                If ((Me._AMSApptCompControl = value) _
                   = False) Then
                    Me._AMSApptCompControl = value
                    Me.SendPropertyChanged("AMSApptCompControl")
                End If
            End Set
        End Property

        Private _AMSApptCarrierControl As Integer
        <DataMember()> _
        Public Property AMSApptCarrierControl() As Integer
            Get
                Return Me._AMSApptCarrierControl
            End Get
            Set(value As Integer)
                If ((Me._AMSApptCarrierControl = value) _
                   = False) Then
                    Me._AMSApptCarrierControl = value
                    Me.SendPropertyChanged("AMSApptCarrierControl")
                End If
            End Set
        End Property

        Private _AMSApptCarrierSCAC As String
        <DataMember()> _
        Public Property AMSApptCarrierSCAC() As String
            Get
                Return Left(Me._AMSApptCarrierSCAC, 20)
            End Get
            Set(value As String)
                If (String.Equals(Me._AMSApptCarrierSCAC, value) = False) Then
                    Me._AMSApptCarrierSCAC = Left(value, 20)
                    Me.SendPropertyChanged("AMSApptCarrierSCAC")
                End If
            End Set
        End Property

        Private _AMSApptCarrierName As String
        <DataMember()> _
        Public Property AMSApptCarrierName() As String
            Get
                Return Left(Me._AMSApptCarrierName, 40)
            End Get
            Set(value As String)
                If (String.Equals(Me._AMSApptCarrierName, value) = False) Then
                    Me._AMSApptCarrierName = Left(value, 40)
                    Me.SendPropertyChanged("AMSApptCarrierName")
                End If
            End Set
        End Property

        Private _AMSApptDescription As String
        <DataMember()> _
        Public Property AMSApptDescription() As String
            Get
                Return Left(Me._AMSApptDescription, 20)
            End Get
            Set(value As String)
                If (String.Equals(Me._AMSApptDescription, value) = False) Then
                    Me._AMSApptDescription = Left(value, 20)
                    Me.SendPropertyChanged("AMSApptDescription")
                End If
            End Set
        End Property

        Private _AMSApptStartDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property AMSApptStartDate() As System.Nullable(Of Date)
            Get
                Return Me._AMSApptStartDate
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._AMSApptStartDate.Equals(value) = False) Then
                    Me._AMSApptStartDate = value
                    Me.SendPropertyChanged("AMSApptStartDate")
                End If
            End Set
        End Property

        Private _AMSApptEndDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property AMSApptEndDate() As System.Nullable(Of Date)
            Get
                Return Me._AMSApptEndDate
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._AMSApptEndDate.Equals(value) = False) Then
                    Me._AMSApptEndDate = value
                    Me.SendPropertyChanged("AMSApptEndDate")
                End If
            End Set
        End Property

        Private _AMSApptTimeZone As String
        <DataMember()> _
        Public Property AMSApptTimeZone() As String
            Get
                Return Left(Me._AMSApptTimeZone, 50)
            End Get
            Set(value As String)
                If (String.Equals(Me._AMSApptTimeZone, value) = False) Then
                    Me._AMSApptTimeZone = Left(value, 50)
                    Me.SendPropertyChanged("AMSApptTimeZone")
                End If
            End Set
        End Property

        Private _AMSApptRecurrenceParentControl As System.Nullable(Of Integer)
        <DataMember()> _
        Public Property AMSApptRecurrenceParentControl() As System.Nullable(Of Integer)
            Get
                Return Me._AMSApptRecurrenceParentControl
            End Get
            Set(value As System.Nullable(Of Integer))
                If (Me._AMSApptRecurrenceParentControl.Equals(value) = False) Then
                    Me._AMSApptRecurrenceParentControl = value
                    Me.SendPropertyChanged("AMSApptRecurrenceParentControl")
                End If
            End Set
        End Property

        Private _AMSApptRecurrence As String
        <DataMember()> _
        Public Property AMSApptRecurrence() As String
            Get
                Return Left(Me._AMSApptRecurrence, 1024)
            End Get
            Set(value As String)
                If (String.Equals(Me._AMSApptRecurrence, value) = False) Then
                    Me._AMSApptRecurrence = Left(value, 1024)
                    Me.SendPropertyChanged("AMSApptRecurrence")
                End If
            End Set
        End Property

        Private _AMSApptActualDateTime As System.Nullable(Of Date)
        <DataMember()> _
        Public Property AMSApptActualDateTime() As System.Nullable(Of Date)
            Get
                Return Me._AMSApptActualDateTime
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._AMSApptActualDateTime.Equals(value) = False) Then
                    Me._AMSApptActualDateTime = value
                    Me.SendPropertyChanged("AMSApptActualDateTime")
                End If
            End Set
        End Property

        Private _AMSApptStartLoadingDateTime As System.Nullable(Of Date)
        <DataMember()> _
        Public Property AMSApptStartLoadingDateTime() As System.Nullable(Of Date)
            Get
                Return Me._AMSApptStartLoadingDateTime
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._AMSApptStartLoadingDateTime.Equals(value) = False) Then
                    Me._AMSApptStartLoadingDateTime = value
                    Me.SendPropertyChanged("AMSApptStartLoadingDateTime")
                End If
            End Set
        End Property

        Private _AMSApptFinishLoadingDateTime As System.Nullable(Of Date)
        <DataMember()> _
        Public Property AMSApptFinishLoadingDateTime() As System.Nullable(Of Date)
            Get
                Return Me._AMSApptFinishLoadingDateTime
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._AMSApptFinishLoadingDateTime.Equals(value) = False) Then
                    Me._AMSApptFinishLoadingDateTime = value
                    Me.SendPropertyChanged("AMSApptFinishLoadingDateTime")
                End If
            End Set
        End Property

        Private _AMSApptActLoadCompleteDateTime As System.Nullable(Of Date)
        <DataMember()> _
        Public Property AMSApptActLoadCompleteDateTime() As System.Nullable(Of Date)
            Get
                Return Me._AMSApptActLoadCompleteDateTime
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._AMSApptActLoadCompleteDateTime.Equals(value) = False) Then
                    Me._AMSApptActLoadCompleteDateTime = value
                    Me.SendPropertyChanged("AMSApptActLoadCompleteDateTime")
                End If
            End Set
        End Property

        Private _AMSApptModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property AMSApptModDate() As System.Nullable(Of Date)
            Get
                Return Me._AMSApptModDate
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._AMSApptModDate.Equals(value) = False) Then
                    Me._AMSApptModDate = value
                    Me.SendPropertyChanged("AMSApptModDate")
                End If
            End Set
        End Property

        Private _AMSApptModUser As String
        <DataMember()> _
        Public Property AMSApptModUser() As String
            Get
                Return Left(Me._AMSApptModUser, 100)
            End Get
            Set(value As String)
                If (String.Equals(Me._AMSApptModUser, value) = False) Then
                    Me._AMSApptModUser = Left(value, 100)
                    Me.SendPropertyChanged("AMSApptModUser")
                End If
            End Set
        End Property

        Private _AMSApptUpdated As Byte()
        <DataMember()> _
        Public Property AMSApptUpdated() As Byte()
            Get
                Return _AMSApptUpdated
            End Get
            Set(ByVal value As Byte())
                _AMSApptUpdated = value
            End Set
        End Property

        Private _AMSApptNotes As String
        <DataMember()> _
        Public Property AMSApptNotes() As String
            Get
                Return Left(Me._AMSApptNotes, 4000)
            End Get
            Set(value As String)
                If (String.Equals(Me._AMSApptNotes, value) = False) Then
                    Me._AMSApptNotes = Left(value, 4000)
                    Me.SendPropertyChanged("AMSApptNotes")
                End If
            End Set
        End Property
         
        Private _AMSApptDockdoorID As String
        <DataMember()> _
        Public Property AMSApptDockdoorID() As String
            Get
                Return Left(Me._AMSApptDockdoorID, 20)
            End Get
            Set(value As String)
                If (String.Equals(Me._AMSApptDockdoorID, value) = False) Then
                    Me._AMSApptDockdoorID = Left(value, 20)
                    Me.SendPropertyChanged("AMSApptDockdoorID")
                End If
            End Set
        End Property

        Private _AMSApptStatusCode As Integer
        <DataMember()> _
        Public Property AMSApptStatusCode As Integer
            Get
                Return _AMSApptStatusCode
            End Get
            Set(value As Integer)
                _AMSApptStatusCode = value
            End Set
        End Property

        Private _AMSApptLabel As String
        <DataMember()> _
        Public Property AMSApptLabel() As String
            Get
                Return Left(Me._AMSApptLabel, 500)
            End Get
            Set(value As String)
                If (String.Equals(Me._AMSApptLabel, value) = False) Then
                    Me._AMSApptLabel = Left(value, 500)
                    Me.SendPropertyChanged("AMSApptLabel")
                End If
            End Set
        End Property

        Private _AMSApptHover As String
        <DataMember()> _
        Public Property AMSApptHover() As String
            Get
                Return Left(Me._AMSApptHover, 4000)
            End Get
            Set(value As String)
                If (String.Equals(Me._AMSApptHover, value) = False) Then
                    Me._AMSApptHover = Left(value, 4000)
                    Me.SendPropertyChanged("AMSApptHover")
                End If
            End Set
        End Property


        Private _AMSApptOrderCount As Integer
        <DataMember()>
        Public Property AMSApptOrderCount As Integer
            Get
                Return _AMSApptOrderCount
            End Get
            Set(value As Integer)
                _AMSApptOrderCount = value
            End Set
        End Property

        'Added By LVV On 5/30/18 For v-8.3 TMS365 Scheduler
        Private _CompAMSColorCodeSettingColorCode As String
        <DataMember()>
        Public Property CompAMSColorCodeSettingColorCode() As String
            Get
                Return Left(Me._CompAMSColorCodeSettingColorCode, 10)
            End Get
            Set(value As String)
                If (String.Equals(Me._CompAMSColorCodeSettingColorCode, value) = False) Then
                    Me._CompAMSColorCodeSettingColorCode = Left(value, 10)
                    Me.SendPropertyChanged("CompAMSColorCodeSettingColorCode")
                End If
            End Set
        End Property

        Private _DockDoorName As String
        <DataMember()>
        Public Property DockDoorName() As String
            Get
                Return Left(Me._DockDoorName, 50)
            End Get
            Set(value As String)
                If (String.Equals(Me._DockDoorName, value) = False) Then
                    Me._DockDoorName = Left(value, 50)
                    Me.SendPropertyChanged("DockDoorName")
                End If
            End Set
        End Property

#End Region



#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New AMSAppointment
            instance = DirectCast(MemberwiseClone(), AMSAppointment)
            Return instance
        End Function

#End Region
    End Class
End Namespace
