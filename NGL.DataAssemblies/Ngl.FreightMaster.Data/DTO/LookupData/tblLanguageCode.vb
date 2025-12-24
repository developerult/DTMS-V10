Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()>
    Public Class tblLanguageCode
        Inherits DTOBaseClass


#Region " Data Members"

        Private _LCControl As Integer = 0
        <DataMember()>
        Public Property LCControl() As Integer
            Get
                Return Me._LCControl
            End Get
            Set(value As Integer)
                If ((Me._LCControl = value) _
                   = False) Then
                    Me._LCControl = value
                    Me.SendPropertyChanged("LCControl")
                End If
            End Set
        End Property



        Private _LCName As String
        <DataMember()>
        Public Property LCName() As String
            Get
                Return Left(Me._LCName, 50)
            End Get
            Set(value As String)
                If (String.Equals(Me._LCName, value) = False) Then
                    Me._LCName = Left(value, 50)
                    Me.SendPropertyChanged("LCName")
                End If
            End Set
        End Property

        Private _LCDesc As String
        <DataMember()>
        Public Property LCDesc() As String
            Get
                Return Left(Me._LCDesc, 100)
            End Get
            Set(value As String)
                If (String.Equals(Me._LCDesc, value) = False) Then
                    Me._LCDesc = Left(value, 100)
                    Me.SendPropertyChanged("LCDesc")
                End If
            End Set
        End Property

        Private _LCCode As String
        <DataMember()>
        Public Property LCCode() As String
            Get
                Return Left(Me._LCCode, 20)
            End Get
            Set(value As String)
                If (String.Equals(Me._LCCode, value) = False) Then
                    Me._LCCode = Left(value, 20)
                    Me.SendPropertyChanged("LCCode")
                End If
            End Set
        End Property

        Private _LCModDate As System.Nullable(Of Date)
        <DataMember()>
        Public Property LCModDate() As System.Nullable(Of Date)
            Get
                Return Me._LCModDate
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._LCModDate.Equals(value) = False) Then
                    Me._LCModDate = value
                    Me.SendPropertyChanged("LCModDate")
                End If
            End Set
        End Property

        Private _LCModUser As String
        <DataMember()>
        Public Property LCModUser() As String
            Get
                Return Left(Me._LCModUser, 100)
            End Get
            Set(value As String)
                If (String.Equals(Me._LCModUser, value) = False) Then
                    Me._LCModUser = Left(value, 100)
                    Me.SendPropertyChanged("LCModUser")
                End If
            End Set
        End Property


        Private _LCUpdated As Byte()
        <DataMember()>
        Public Property LCUpdated() As Byte()
            Get
                Return Me._LCUpdated
            End Get
            Set(value As Byte())
                Me._LCUpdated = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblLanguageCode
            instance = DirectCast(MemberwiseClone(), tblLanguageCode)
            Return instance
        End Function

#End Region

    End Class
End Namespace

