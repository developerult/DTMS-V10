
Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblSystemError
        Inherits DTOBaseClass


#Region " Data Members"
        Private _ErrID As Long
        <DataMember()> _
        Public Property ErrID() As Long
            Get
                Return Me._ErrID
            End Get
            Set(ByVal value As Long)
                If ((Me._ErrID = value) _
                   = False) Then
                    Me._ErrID = value
                    Me.SendPropertyChanged("ErrID")
                End If
            End Set
        End Property

        Private _Message As String
        <DataMember()> _
        Public Property Message() As String
            Get
                Return Me._Message
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._Message, value) = False) Then
                    Me._Message = value
                    Me.SendPropertyChanged("Message")
                End If
            End Set
        End Property

        Private _Record As String
        <DataMember()> _
        Public Property Record() As String
            Get
                Return Me._Record
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._Record, value) = False) Then
                    Me._Record = value
                    Me.SendPropertyChanged("Record")
                End If
            End Set
        End Property
        Private _CurrentUser As String
        <DataMember()> _
        Public Property CurrentUser() As String
            Get
                Return Me._CurrentUser
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._CurrentUser, value) = False) Then
                    Me._CurrentUser = value
                    Me.SendPropertyChanged("CurrentUser")
                End If
            End Set
        End Property
        Private _CurrentDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CurrentDate() As System.Nullable(Of Date)
            Get
                Return Me._CurrentDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                If (Me._CurrentDate.Equals(value) = False) Then
                    Me._CurrentDate = value
                    Me.SendPropertyChanged("CurrentDate")
                End If
            End Set
        End Property
        Private _ErrorNumber As System.Nullable(Of Integer)
        <DataMember()> _
        Public Property ErrorNumber() As System.Nullable(Of Integer)
            Get
                Return Me._ErrorNumber
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                If (Me._ErrorNumber.Equals(value) = False) Then
                    Me._ErrorNumber = value
                    Me.SendPropertyChanged("ErrorNumber")
                End If
            End Set
        End Property
        Private _ErrorSeverity As System.Nullable(Of Integer)
        <DataMember()> _
        Public Property ErrorSeverity() As System.Nullable(Of Integer)
            Get
                Return Me._ErrorSeverity
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                If (Me._ErrorSeverity.Equals(value) = False) Then
                    Me._ErrorSeverity = value
                    Me.SendPropertyChanged("ErrorSeverity")
                End If
            End Set
        End Property
        Private _ErrorState As System.Nullable(Of Integer)
        <DataMember()> _
        Public Property ErrorState() As System.Nullable(Of Integer)
            Get
                Return Me._ErrorState
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                If (Me._ErrorState.Equals(value) = False) Then
                    Me._ErrorState = value
                    Me.SendPropertyChanged("ErrorState")
                End If
            End Set
        End Property

        Private _ErrorProcedure As String
        <DataMember()> _
        Public Property ErrorProcedure() As String
            Get
                Return Me._ErrorProcedure
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._ErrorProcedure, value) = False) Then
                    Me._ErrorProcedure = value
                    Me.SendPropertyChanged("ErrorProcedure")
                End If
            End Set
        End Property
        Private _ErrorLineNbr As System.Nullable(Of Integer)
        <DataMember()> _
        Public Property ErrorLineNbr() As System.Nullable(Of Integer)
            Get
                Return Me._ErrorLineNbr
            End Get
            Set(ByVal value As System.Nullable(Of Integer))
                If (Me._ErrorLineNbr.Equals(value) = False) Then
                    Me._ErrorLineNbr = value
                    Me.SendPropertyChanged("ErrorLineNbr")
                End If
            End Set
        End Property
        Private _ErrorAlertSent As Boolean
        <DataMember()> _
        Public Property ErrorAlertSent() As Boolean
            Get
                Return Me._ErrorAlertSent
            End Get
            Set(ByVal value As Boolean)
                If ((Me._ErrorAlertSent = value) _
                   = False) Then
                    Me._ErrorAlertSent = value
                    Me.SendPropertyChanged("ErrorAlertSent")
                End If
            End Set
        End Property

        Private _ErrorAlertSentDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property ErrorAlertSentDate() As System.Nullable(Of Date)
            Get
                Return Me._ErrorAlertSentDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                If (Me._ErrorAlertSentDate.Equals(value) = False) Then
                    Me._ErrorAlertSentDate = value
                    Me.SendPropertyChanged("ErrorAlertSentDate")
                End If
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblSystemError
            instance = DirectCast(MemberwiseClone(), tblSystemError)
            Return instance
        End Function

#End Region

    End Class
End Namespace

