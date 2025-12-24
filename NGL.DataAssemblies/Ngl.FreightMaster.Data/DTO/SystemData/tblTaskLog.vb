Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblTaskLog
        Inherits DTOBaseClass


#Region " Data Members"
        Private _TaskControl As Integer
        <DataMember()> _
        Public Property TaskControl() As Integer
            Get
                Return Me._TaskControl
            End Get
            Set(ByVal value As Integer)
                If ((Me._TaskControl = value) _
                   = False) Then
                    Me._TaskControl = value
                    Me.SendPropertyChanged("TaskControl")
                End If
            End Set
        End Property

        Private _TaskName As String
        <DataMember()> _
        Public Property TaskName() As String
            Get
                Return Me._TaskName
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._TaskName, value) = False) Then
                    Me._TaskName = value
                    Me.SendPropertyChanged("TaskName")
                End If
            End Set
        End Property

        Private _TaskDesc As String
        <DataMember()> _
        Public Property TaskDesc() As String
            Get
                Return Me._TaskDesc
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._TaskDesc, value) = False) Then
                    Me._TaskDesc = value
                    Me.SendPropertyChanged("TaskDesc")
                End If
            End Set
        End Property

        Private _TaskType As System.Nullable(Of Integer)
        <DataMember()> _
        Public Property TaskType() As System.Nullable(Of Integer)
            Get
                Return Me._TaskType
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                If (Me._TaskType.Equals(value) = False) Then
                    Me._TaskType = value
                    Me.SendPropertyChanged("TaskType")
                End If
            End Set
        End Property

        Private _TaskCommand As String
        <DataMember()> _
        Public Property TaskCommand() As String
            Get
                Return Me._TaskCommand
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._TaskCommand, value) = False) Then
                    Me._TaskCommand = value
                    Me.SendPropertyChanged("TaskCommand")
                End If
            End Set
        End Property

        Private _TaskLastRanOn As System.Nullable(Of Date)
        <DataMember()> _
        Public Property TaskLastRanOn() As System.Nullable(Of Date)
            Get
                Return Me._TaskLastRanOn
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                If (Me._TaskLastRanOn.Equals(value) = False) Then
                    Me._TaskLastRanOn = value
                    Me.SendPropertyChanged("TaskLastRanOn")
                End If
            End Set
        End Property

        Private _TaskRanOn As System.Nullable(Of Date)
        <DataMember()> _
        Public Property TaskRanOn() As System.Nullable(Of Date)
            Get
                Return Me._TaskRanOn
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                If (Me._TaskRanOn.Equals(value) = False) Then
                    Me._TaskRanOn = value
                    Me.SendPropertyChanged("TaskRanOn")
                End If
            End Set
        End Property

        Private _TaskMessage As String
        <DataMember()> _
        Public Property TaskMessage() As String
            Get
                Return Me._TaskMessage
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._TaskMessage, value) = False) Then
                    Me._TaskMessage = value
                    Me.SendPropertyChanged("TaskMessage")
                End If
            End Set
        End Property
#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblTaskLog
            instance = DirectCast(MemberwiseClone(), tblTaskLog)
            Return instance
        End Function

#End Region

    End Class
End Namespace

