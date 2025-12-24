Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblLog
        Inherits DTOBaseClass


#Region " Data Members"
        Private _LogControl As Long
        <DataMember()> _
        Public Property LogControl() As Long
            Get
                Return Me._LogControl
            End Get
            Set(ByVal value As Long)
                If ((Me._LogControl = value) _
                   = False) Then
                    Me._LogControl = value
                    Me.SendPropertyChanged("LogControl")
                End If
            End Set
        End Property

        Private _LogMessage As String
        <DataMember()> _
        Public Property LogMessage() As String
            Get
                Return Me._LogMessage
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._LogMessage, value) = False) Then
                    Me._LogMessage = value
                    Me.SendPropertyChanged("LogMessage")
                End If
            End Set
        End Property

        Private _LogTime As System.Nullable(Of Date)
        <DataMember()> _
        Public Property LogTime() As System.Nullable(Of Date)
            Get
                Return Me._LogTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                If (Me._LogTime.Equals(value) = False) Then
                    Me._LogTime = value
                    Me.SendPropertyChanged("LogTime")
                End If
            End Set
        End Property

        Private _LogUser As String
        <DataMember()> _
        Public Property LogUser() As String
            Get
                Return Me._LogUser
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._LogUser, value) = False) Then
                    Me._LogUser = value
                    Me.SendPropertyChanged("LogUser")
                End If
            End Set
        End Property

        Private _LogSource As String
        <DataMember()> _
        Public Property LogSource() As String
            Get
                Return Me._LogSource
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._LogSource, value) = False) Then
                    Me._LogSource = value
                    Me.SendPropertyChanged("LogSource")
                End If
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblLog
            instance = DirectCast(MemberwiseClone(), tblLog)
            Return instance
        End Function

#End Region

    End Class
End Namespace

