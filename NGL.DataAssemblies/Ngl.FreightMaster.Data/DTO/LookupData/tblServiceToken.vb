Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblServiceToken
        Inherits DTOBaseClass


#Region " Data Members"


        Private _ServiceTokenControl As Integer = 0
        <DataMember()> _
        Public Property ServiceTokenControl() As Integer
            Get
                Return Me._ServiceTokenControl
            End Get
            Set(value As Integer)
                If ((Me._ServiceTokenControl = value) _
                   = False) Then
                    Me._ServiceTokenControl = value
                    Me.SendPropertyChanged("ServiceTokenControl")
                End If
            End Set
        End Property

        Private _ServiceTokenServiceTypeControl As Integer = 0
        <DataMember()> _
        Public Property ServiceTokenServiceTypeControl() As Integer
            Get
                Return Me._ServiceTokenServiceTypeControl
            End Get
            Set(value As Integer)
                If ((Me._ServiceTokenServiceTypeControl = value) _
                   = False) Then
                    Me._ServiceTokenServiceTypeControl = value
                    Me.SendPropertyChanged("ServiceTokenServiceTypeControl")
                End If
            End Set
        End Property

        Private _ServiceToken As String
        <DataMember()> _
        Public Property ServiceToken() As String
            Get
                Return Left(Me._ServiceToken, 4000)
            End Get
            Set(value As String)
                If (String.Equals(Me._ServiceToken, value) = False) Then
                    Me._ServiceToken = Left(value, 4000)
                    Me.SendPropertyChanged("ServiceToken")
                End If
            End Set
        End Property

        Private _ServiceTokenBookControl As Integer = 0
        <DataMember()> _
        Public Property ServiceTokenBookControl() As Integer
            Get
                Return Me._ServiceTokenBookControl
            End Get
            Set(value As Integer)
                If ((Me._ServiceTokenBookControl = value) _
                   = False) Then
                    Me._ServiceTokenBookControl = value
                    Me.SendPropertyChanged("ServiceTokenBookControl")
                End If
            End Set
        End Property

        Private _ServiceTokenCompControl As Integer = 0
        <DataMember()> _
        Public Property ServiceTokenCompControl() As Integer
            Get
                Return Me._ServiceTokenCompControl
            End Get
            Set(value As Integer)
                If ((Me._ServiceTokenCompControl = value) _
                   = False) Then
                    Me._ServiceTokenCompControl = value
                    Me.SendPropertyChanged("ServiceTokenCompControl")
                End If
            End Set
        End Property

        Private _ServiceTokenCarrierControl As Integer = 0
        <DataMember()> _
        Public Property ServiceTokenCarrierControl() As Integer
            Get
                Return Me._ServiceTokenCarrierControl
            End Get
            Set(value As Integer)
                If ((Me._ServiceTokenCarrierControl = value) _
                   = False) Then
                    Me._ServiceTokenCarrierControl = value
                    Me.SendPropertyChanged("ServiceTokenCarrierControl")
                End If
            End Set
        End Property

        Private _ServiceTokenCarrierContControl As Integer = 0
        <DataMember()> _
        Public Property ServiceTokenCarrierContControl() As Integer
            Get
                Return Me._ServiceTokenCarrierContControl
            End Get
            Set(value As Integer)
                If ((Me._ServiceTokenCarrierContControl = value) _
                   = False) Then
                    Me._ServiceTokenCarrierContControl = value
                    Me.SendPropertyChanged("ServiceTokenCarrierContControl")
                End If
            End Set
        End Property

        Private _ServiceTokenLaneControl As Integer = 0
        <DataMember()> _
        Public Property ServiceTokenLaneControl() As Integer
            Get
                Return Me._ServiceTokenLaneControl
            End Get
            Set(value As Integer)
                If ((Me._ServiceTokenLaneControl = value) _
                   = False) Then
                    Me._ServiceTokenLaneControl = value
                    Me.SendPropertyChanged("ServiceTokenLaneControl")
                End If
            End Set
        End Property

        Private _ServiceTokenAltKeyControl As Integer = 0
        <DataMember()> _
        Public Property ServiceTokenAltKeyControl() As Integer
            Get
                Return Me._ServiceTokenAltKeyControl
            End Get
            Set(value As Integer)
                If ((Me._ServiceTokenAltKeyControl = value) _
                   = False) Then
                    Me._ServiceTokenAltKeyControl = value
                    Me.SendPropertyChanged("ServiceTokenAltKeyControl")
                End If
            End Set
        End Property

        Private _ServiceTokenCode As String
        <DataMember()> _
        Public Property ServiceTokenCode() As String
            Get
                Return Left(Me._ServiceTokenCode, 20)
            End Get
            Set(value As String)
                If (String.Equals(Me._ServiceTokenCode, value) = False) Then
                    Me._ServiceTokenCode = Left(value, 20)
                    Me.SendPropertyChanged("ServiceTokenCode")
                End If
            End Set
        End Property

        Private _ServiceTokenSendEmail As Boolean = False
        <DataMember()> _
        Public Property ServiceTokenSendEmail() As Boolean
            Get
                Return Me._ServiceTokenSendEmail
            End Get
            Set(value As Boolean)
                If ((Me._ServiceTokenSendEmail = value) _
                   = False) Then
                    Me._ServiceTokenSendEmail = value
                    Me.SendPropertyChanged("ServiceTokenSendEmail")
                End If
            End Set
        End Property

        Private _ServiceTokenBookTrackComment As String
        <DataMember()> _
        Public Property ServiceTokenBookTrackComment() As String
            Get
                Return Left(Me._ServiceTokenBookTrackComment, 255)
            End Get
            Set(value As String)
                If (String.Equals(Me._ServiceTokenBookTrackComment, value) = False) Then
                    Me._ServiceTokenBookTrackComment = Left(value, 255)
                    Me.SendPropertyChanged("ServiceTokenBookTrackComment")
                End If
            End Set
        End Property

        Private _ServiceTokenBookTrackStatus As Integer = 0
        <DataMember()> _
        Public Property ServiceTokenBookTrackStatus() As Integer
            Get
                Return Me._ServiceTokenBookTrackStatus
            End Get
            Set(value As Integer)
                If ((Me._ServiceTokenBookTrackStatus = value) _
                   = False) Then
                    Me._ServiceTokenBookTrackStatus = value
                    Me.SendPropertyChanged("ServiceTokenBookTrackStatus")
                End If
            End Set
        End Property

        Private _ServiceTokenNotificationEMailAddress As String
        <DataMember()> _
        Public Property ServiceTokenNotificationEMailAddress() As String
            Get
                Return Left(Me._ServiceTokenNotificationEMailAddress, 255)
            End Get
            Set(value As String)
                If (String.Equals(Me._ServiceTokenNotificationEMailAddress, value) = False) Then
                    Me._ServiceTokenNotificationEMailAddress = Left(value, 255)
                    Me.SendPropertyChanged("ServiceTokenNotificationEMailAddress")
                End If
            End Set
        End Property

        Private _ServiceTokenNotificationEMailAddressCc As String
        <DataMember()> _
        Public Property ServiceTokenNotificationEMailAddressCc() As String
            Get
                Return Left(Me._ServiceTokenNotificationEMailAddressCc, 255)
            End Get
            Set(value As String)
                If (String.Equals(Me._ServiceTokenNotificationEMailAddressCc, value) = False) Then
                    Me._ServiceTokenNotificationEMailAddressCc = Left(value, 255)
                    Me.SendPropertyChanged("ServiceTokenNotificationEMailAddressCc")
                End If
            End Set
        End Property

        Private _ServiceTokenUserName As String
        <DataMember()> _
        Public Property ServiceTokenUserName() As String
            Get
                Return Left(Me._ServiceTokenUserName, 100)
            End Get
            Set(value As String)
                If (String.Equals(Me._ServiceTokenUserName, value) = False) Then
                    Me._ServiceTokenUserName = Left(value, 100)
                    Me.SendPropertyChanged("ServiceTokenUserName")
                End If
            End Set
        End Property

        Private _ServiceTokenExpirationDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property ServiceTokenExpirationDate() As System.Nullable(Of Date)
            Get
                Return Me._ServiceTokenExpirationDate
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._ServiceTokenExpirationDate.Equals(value) = False) Then
                    Me._ServiceTokenExpirationDate = value
                    Me.SendPropertyChanged("ServiceTokenExpirationDate")
                End If
            End Set
        End Property

        Private _ServiceTokenModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property ServiceTokenModDate() As System.Nullable(Of Date)
            Get
                Return Me._ServiceTokenModDate
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._ServiceTokenModDate.Equals(value) = False) Then
                    Me._ServiceTokenModDate = value
                    Me.SendPropertyChanged("ServiceTokenModDate")
                End If
            End Set
        End Property

        Private _ServiceTokenModUser As String
        <DataMember()> _
        Public Property ServiceTokenModUser() As String
            Get
                Return Left(Me._ServiceTokenModUser, 100)
            End Get
            Set(value As String)
                If (String.Equals(Me._ServiceTokenModUser, value) = False) Then
                    Me._ServiceTokenModUser = Left(value, 100)
                    Me.SendPropertyChanged("ServiceTokenModUser")
                End If
            End Set
        End Property

        Private _ServiceTokenUpdated As Byte()
        <DataMember()> _
        Public Property ServiceTokenUpdated() As Byte()
            Get
                Return Me._ServiceTokenUpdated
            End Get
            Set(value As Byte())
                Me._ServiceTokenUpdated = value
            End Set
        End Property


#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblServiceToken
            instance = DirectCast(MemberwiseClone(), tblServiceToken)
            Return instance
        End Function

#End Region

    End Class
End Namespace
