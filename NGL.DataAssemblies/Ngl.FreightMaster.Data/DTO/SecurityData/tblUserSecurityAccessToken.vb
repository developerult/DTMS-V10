Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblUserSecurityAccessToken
        Inherits DTOBaseClass


#Region " Data Members"

        Private _USATControl As Integer = 0
        <DataMember()> _
        Public Property USATControl() As Integer
            Get
                Return Me._USATControl
            End Get
            Set(value As Integer)
                If ((Me._USATControl = value) _
                   = False) Then
                    Me._USATControl = value
                    Me.SendPropertyChanged("USATControl")
                End If
            End Set
        End Property

        Private _USATUserSecurityControl As Integer = 0
        <DataMember()> _
        Public Property USATUserSecurityControl() As Integer
            Get
                Return Me._USATUserSecurityControl
            End Get
            Set(value As Integer)
                If ((Me._USATUserSecurityControl = value) _
                   = False) Then
                    Me._USATUserSecurityControl = value
                    Me.SendPropertyChanged("USATUserSecurityControl")
                End If
            End Set
        End Property

        Private _USATSSOAControl As Integer = 0
        <DataMember()> _
        Public Property USATSSOAControl() As Integer
            Get
                Return Me._USATSSOAControl
            End Get
            Set(value As Integer)
                If ((Me._USATSSOAControl = value) _
                   = False) Then
                    Me._USATSSOAControl = value
                    Me.SendPropertyChanged("USATSSOAControl")
                End If
            End Set
        End Property

        Private _USATUserID As String
        <DataMember()> _
        Public Property USATUserID() As String
            Get
                Return Left(Me._USATUserID, 255)
            End Get
            Set(value As String)
                If (String.Equals(Me._USATUserID, value) = False) Then
                    Me._USATUserID = Left(value, 255)
                    Me.SendPropertyChanged("USATUserID")
                End If
            End Set
        End Property

        Private _USATToken As String
        <DataMember()> _
        Public Property USATToken() As String
            Get
                Return Left(Me._USATToken, 4000)
            End Get
            Set(value As String)
                If (String.Equals(Me._USATToken, value) = False) Then
                    Me._USATToken = Left(value, 4000)
                    Me.SendPropertyChanged("USATToken")
                End If
            End Set
        End Property

        Private _USATExpires As Date
        <DataMember()> _
        Public Property USATExpires() As Date
            Get
                Return Me._USATExpires
            End Get
            Set(value As Date)
                If (Me._USATExpires.Equals(value) = False) Then
                    Me._USATExpires = value
                    Me.SendPropertyChanged("USATExpires")
                End If
            End Set
        End Property

        Private _USATModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property USATModDate() As System.Nullable(Of Date)
            Get
                Return Me._USATModDate
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._USATModDate.Equals(value) = False) Then
                    Me._USATModDate = value
                    Me.SendPropertyChanged("USATModDate")
                End If
            End Set
        End Property

        Private _USATModUser As String
        <DataMember()> _
        Public Property USATModUser() As String
            Get
                Return Left(Me._USATModUser, 100)
            End Get
            Set(value As String)
                If (String.Equals(Me._USATModUser, value) = False) Then
                    Me._USATModUser = Left(value, 100)
                    Me.SendPropertyChanged("USATModUser")
                End If
            End Set
        End Property


        Private _USATUpdated As Byte()
        <DataMember()> _
        Public Property USATUpdated() As Byte()
            Get
                Return Me._USATUpdated
            End Get
            Set(value As Byte())
                Me._USATUpdated = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblUserSecurityAccessToken
            instance = DirectCast(MemberwiseClone(), tblUserSecurityAccessToken)
            Return instance
        End Function

#End Region

    End Class
End Namespace
