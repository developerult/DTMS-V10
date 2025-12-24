Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblSingleSignOnAccount
        Inherits DTOBaseClass


#Region " Data Members"

        Private _SSOAControl As Integer = 0
        <DataMember()> _
        Public Property SSOAControl() As Integer
            Get
                Return Me._SSOAControl
            End Get
            Set(value As Integer)
                If ((Me._SSOAControl = value) _
                   = False) Then
                    Me._SSOAControl = value
                    Me.SendPropertyChanged("SSOAControl")
                End If
            End Set
        End Property



        Private _SSOAName As String
        <DataMember()> _
        Public Property SSOAName() As String
            Get
                Return Left(Me._SSOAName, 500)
            End Get
            Set(value As String)
                If (String.Equals(Me._SSOAName, value) = False) Then
                    Me._SSOAName = Left(value, 500)
                    Me.SendPropertyChanged("SSOAName")
                End If
            End Set
        End Property

        Private _SSOADesc As String
        <DataMember()> _
        Public Property SSOADesc() As String
            Get
                Return Left(Me._SSOADesc, 100)
            End Get
            Set(value As String)
                If (String.Equals(Me._SSOADesc, value) = False) Then
                    Me._SSOADesc = Left(value, 100)
                    Me.SendPropertyChanged("SSOADesc")
                End If
            End Set
        End Property

        Private _SSOAClientID As String
        <DataMember()> _
        Public Property SSOAClientID() As String
            Get
                Return Left(Me._SSOAClientID, 500)
            End Get
            Set(value As String)
                If (String.Equals(Me._SSOAClientID, value) = False) Then
                    Me._SSOAClientID = Left(value, 500)
                    Me.SendPropertyChanged("SSOAClientID")
                End If
            End Set
        End Property

        Private _SSOALoginURL As String
        <DataMember()> _
        Public Property SSOALoginURL() As String
            Get
                Return Left(Me._SSOALoginURL, 1000)
            End Get
            Set(value As String)
                If (String.Equals(Me._SSOALoginURL, value) = False) Then
                    Me._SSOALoginURL = Left(value, 1000)
                    Me.SendPropertyChanged("SSOALoginURL")
                End If
            End Set
        End Property

        Private _SSOADataURL As String
        <DataMember()> _
        Public Property SSOADataURL() As String
            Get
                Return Left(Me._SSOADataURL, 1000)
            End Get
            Set(value As String)
                If (String.Equals(Me._SSOADataURL, value) = False) Then
                    Me._SSOADataURL = Left(value, 1000)
                    Me.SendPropertyChanged("SSOADataURL")
                End If
            End Set
        End Property

        Private _SSOARedirectURL As String
        <DataMember()> _
        Public Property SSOARedirectURL() As String
            Get
                Return Left(Me._SSOARedirectURL, 1000)
            End Get
            Set(value As String)
                If (String.Equals(Me._SSOARedirectURL, value) = False) Then
                    Me._SSOARedirectURL = Left(value, 1000)
                    Me.SendPropertyChanged("SSOARedirectURL")
                End If
            End Set
        End Property

        Private _SSOAClientSecret As String
        <DataMember()> _
        Public Property SSOAClientSecret() As String
            Get
                Return Left(Me._SSOAClientSecret, 500)
            End Get
            Set(value As String)
                If (String.Equals(Me._SSOAClientSecret, value) = False) Then
                    Me._SSOAClientSecret = Left(value, 500)
                    Me.SendPropertyChanged("SSOAClientSecret")
                End If
            End Set
        End Property

        Private _SSOAAuthCode As String
        <DataMember()> _
        Public Property SSOAAuthCode() As String
            Get
                Return Left(Me._SSOAAuthCode, 1000)
            End Get
            Set(value As String)
                If (String.Equals(Me._SSOAAuthCode, value) = False) Then
                    Me._SSOAAuthCode = Left(value, 1000)
                    Me.SendPropertyChanged("SSOAAuthCode")
                End If
            End Set
        End Property

        Private _SSOAModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property SSOAModDate() As System.Nullable(Of Date)
            Get
                Return Me._SSOAModDate
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._SSOAModDate.Equals(value) = False) Then
                    Me._SSOAModDate = value
                    Me.SendPropertyChanged("SSOAModDate")
                End If
            End Set
        End Property

        Private _SSOAModUser As String
        <DataMember()> _
        Public Property SSOAModUser() As String
            Get
                Return Left(Me._SSOAModUser, 100)
            End Get
            Set(value As String)
                If (String.Equals(Me._SSOAModUser, value) = False) Then
                    Me._SSOAModUser = Left(value, 100)
                    Me.SendPropertyChanged("SSOAModUser")
                End If
            End Set
        End Property


        Private _SSOAUpdated As Byte()
        <DataMember()>
        Public Property SSOAUpdated() As Byte()
            Get
                Return Me._SSOAUpdated
            End Get
            Set(value As Byte())
                Me._SSOAUpdated = value
            End Set
        End Property



#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblSingleSignOnAccount
            instance = DirectCast(MemberwiseClone(), tblSingleSignOnAccount)
            Return instance
        End Function

#End Region

    End Class
End Namespace
