Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblTarAgent
        Inherits DTOBaseClass


#Region " Data Members"

        Private _TarAgentControl As Integer = 0
        <DataMember()> _
        Public Property TarAgentControl() As Integer
            Get
                Return Me._TarAgentControl
            End Get
            Set(value As Integer)
                If ((Me._TarAgentControl = value) _
                   = False) Then
                    Me._TarAgentControl = value
                    Me.SendPropertyChanged("TarAgentControl")
                End If
            End Set
        End Property



        Private _TarAgentName As String
        <DataMember()> _
        Public Property TarAgentName() As String
            Get
                Return Left(Me._TarAgentName, 50)
            End Get
            Set(value As String)
                If (String.Equals(Me._TarAgentName, value) = False) Then
                    Me._TarAgentName = Left(value, 50)
                    Me.SendPropertyChanged("TarAgentName")
                End If
            End Set
        End Property

        Private _TarAgentDesc As String
        <DataMember()> _
        Public Property TarAgentDesc() As String
            Get
                Return Left(Me._TarAgentDesc, 255)
            End Get
            Set(value As String)
                If (String.Equals(Me._TarAgentDesc, value) = False) Then
                    Me._TarAgentDesc = Left(value, 255)
                    Me.SendPropertyChanged("TarAgentDesc")
                End If
            End Set
        End Property

        Private _TarAgentModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property TarAgentModDate() As System.Nullable(Of Date)
            Get
                Return Me._TarAgentModDate
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._TarAgentModDate.Equals(value) = False) Then
                    Me._TarAgentModDate = value
                    Me.SendPropertyChanged("TarAgentModDate")
                End If
            End Set
        End Property

        Private _TarAgentModUser As String
        <DataMember()> _
        Public Property TarAgentModUser() As String
            Get
                Return Left(Me._TarAgentModUser, 100)
            End Get
            Set(value As String)
                If (String.Equals(Me._TarAgentModUser, value) = False) Then
                    Me._TarAgentModUser = Left(value, 100)
                    Me.SendPropertyChanged("TarAgentModUser")
                End If
            End Set
        End Property


        Private _TarAgentUpdated As Byte()
        <DataMember()> _
        Public Property TarAgentUpdated() As Byte()
            Get
                Return Me._TarAgentUpdated
            End Get
            Set(value As Byte())
                Me._TarAgentUpdated = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblTarAgent
            instance = DirectCast(MemberwiseClone(), tblTarAgent)
            Return instance
        End Function

#End Region

    End Class
End Namespace
