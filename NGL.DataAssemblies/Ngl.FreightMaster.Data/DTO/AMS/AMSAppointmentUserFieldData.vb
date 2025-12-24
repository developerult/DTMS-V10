Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class AMSAppointmentUserFieldData
        Inherits DTOBaseClass

#Region " Data Members"


        Private _AMSApptUFDControl As Integer
        <DataMember()> _
        Public Property AMSApptUFDControl() As Integer
            Get
                Return Me._AMSApptUFDControl
            End Get
            Set(value As Integer)
                If ((Me._AMSApptUFDControl = value) _
                   = False) Then
                    Me._AMSApptUFDControl = value
                    Me.SendPropertyChanged("AMSApptUFDControl")
                End If
            End Set
        End Property

        Private _AMSApptUFDApptControl As Integer
        <DataMember()> _
        Public Property AMSApptUFDApptControl As Integer
            Get
                Return _AMSApptUFDApptControl
            End Get
            Set(value As Integer)
                If ((Me._AMSApptUFDApptControl = value) _
                   = False) Then
                    Me._AMSApptUFDApptControl = value
                    Me.SendPropertyChanged("AMSApptUFDApptControl")
                End If
            End Set
        End Property

        Private _AMSApptUFDCompAMSUserFieldSettingControl As Integer
        <DataMember()> _
        Public Property AMSApptUFDCompAMSUserFieldSettingControl As Integer
            Get
                Return _AMSApptUFDCompAMSUserFieldSettingControl
            End Get
            Set(value As Integer)
                If ((Me._AMSApptUFDCompAMSUserFieldSettingControl = value) _
                   = False) Then
                    Me._AMSApptUFDCompAMSUserFieldSettingControl = value
                    Me.SendPropertyChanged("AMSApptUFDCompAMSUserFieldSettingControl")
                End If
            End Set
        End Property

        Private _AMSApptUFDName As String = ""
        <DataMember()> _
        Public Property AMSApptUFDName() As String
            Get
                Return Left(_AMSApptUFDName, 20)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._AMSApptUFDName, value) = False) Then
                    Me._AMSApptUFDName = Left(value, 20)
                    Me.SendPropertyChanged("AMSApptUFDName")
                End If
            End Set
        End Property

        Private _AMSApptUFDDesc As String = ""
        <DataMember()> _
        Public Property AMSApptUFDDesc() As String
            Get
                Return Left(_AMSApptUFDDesc, 255)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._AMSApptUFDDesc, value) = False) Then
                    Me._AMSApptUFDDesc = Left(value, 255)
                    Me.SendPropertyChanged("AMSApptUFDDesc")
                End If
            End Set
        End Property


        Private _AMSApptUFDData As String = ""
        <DataMember()> _
        Public Property AMSApptUFDData() As String
            Get
                Return Me._AMSApptUFDData
            End Get
            Set(value As String)
                If (String.Equals(Me._AMSApptUFDData, value) = False) Then
                    Me._AMSApptUFDData = Left(value, 255)
                    Me.SendPropertyChanged("AMSApptUFDData")
                End If
            End Set
        End Property


        Private _AMSApptUFDModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property AMSApptUFDModDate() As System.Nullable(Of Date)
            Get
                Return Me._AMSApptUFDModDate
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._AMSApptUFDModDate.Equals(value) = False) Then
                    Me._AMSApptUFDModDate = value
                    Me.SendPropertyChanged("AMSApptUFDModDate")
                End If
            End Set
        End Property

        Private _AMSApptUFDModUser As String
        <DataMember()> _
        Public Property AMSApptUFDModUser() As String
            Get
                Return Left(Me._AMSApptUFDModUser, 100)
            End Get
            Set(value As String)
                If (String.Equals(Me._AMSApptUFDModUser, value) = False) Then
                    Me._AMSApptUFDModUser = Left(value, 100)
                    Me.SendPropertyChanged("AMSApptUFDModUser")
                End If
            End Set
        End Property

        Private _AMSApptUFDUpdated As Byte()
        <DataMember()> _
        Public Property AMSApptUFDUpdated() As Byte()
            Get
                Return _AMSApptUFDUpdated
            End Get
            Set(ByVal value As Byte())
                _AMSApptUFDUpdated = value
            End Set
        End Property


#End Region



#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New AMSAppointmentUserFieldData
            instance = DirectCast(MemberwiseClone(), AMSAppointmentUserFieldData)
            Return instance
        End Function

#End Region
    End Class
End Namespace