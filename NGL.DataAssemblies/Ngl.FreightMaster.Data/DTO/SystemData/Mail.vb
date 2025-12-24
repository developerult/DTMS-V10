Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class Mail
        Inherits DTOBaseClass


#Region " Data Members"

        Private _MailControl As Integer
        <DataMember()> _
        Public Property MailControl() As String
            Get
                Return _MailControl
            End Get
            Set(ByVal value As String)
                _MailControl = value
            End Set
        End Property

        Private _MailFrom As String
        <DataMember()> _
        Public Property MailFrom() As String
            Get
                Return Left(Me._MailFrom, 100)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._MailFrom, value) = False) Then
                    Me._MailFrom = Left(value, 100)
                    Me.SendPropertyChanged("MailFrom")
                End If
            End Set
        End Property

        Private _MailTo As String
        <DataMember()> _
        Public Property MailTo() As String
            Get
                Return Left(Me._MailTo, 1000)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._MailTo, value) = False) Then
                    Me._MailTo = Left(value, 1000)
                    Me.SendPropertyChanged("MailTo")
                End If
            End Set
        End Property

        Private _MailCc As String
        <DataMember()> _
        Public Property MailCc() As String
            Get
                Return Left(Me._MailCc, 1000)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._MailCc, value) = False) Then
                    Me._MailCc = Left(value, 1000)
                    Me.SendPropertyChanged("MailCc")
                End If
            End Set
        End Property

        Private _Subject As String
        <DataMember()> _
        Public Property Subject() As String
            Get
                Return Left(Me._Subject, 100)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._Subject, value) = False) Then
                    Me._Subject = Left(value, 100)
                    Me.SendPropertyChanged("Subject")
                End If
            End Set
        End Property

        Private _Body As String
        <DataMember()> _
        Public Property Body() As String
            Get
                Return Left(Me._Body, 4000)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._Body, value) = False) Then
                    Me._Body = Left(value, 4000)
                    Me.SendPropertyChanged("Body")
                End If
            End Set
        End Property

        Private _DateAdded As System.Nullable(Of Date)
        <DataMember()> _
        Public Property DateAdded() As System.Nullable(Of Date)
            Get
                Return _DateAdded
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                If (Me._DateAdded.Equals(value) = False) Then
                    Me._DateAdded = value
                    Me.SendPropertyChanged("DateAdded")
                End If
            End Set
        End Property

        Private _ReadyToSend As Boolean = False
        <DataMember()> _
        Public Property ReadyToSend() As Boolean
            Get
                Return _ReadyToSend
            End Get
            Set(ByVal value As Boolean)
                If (Not Me._ReadyToSend = value) Then
                    Me._ReadyToSend = value
                    Me.SendPropertyChanged("ReadyToSend")
                End If
            End Set
        End Property

        Private _DateSent As System.Nullable(Of Date)
        <DataMember()> _
        Public Property DateSent() As System.Nullable(Of Date)
            Get
                Return _DateSent
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                If (Me._DateSent.Equals(value) = False) Then
                    Me._DateSent = value
                    Me.SendPropertyChanged("DateSent")
                End If
            End Set
        End Property

        Private _Result As String
        <DataMember()> _
        Public Property Result() As String
            Get
                Return Left(Me._Result, 1000)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._Result, value) = False) Then
                    Me._Result = Left(value, 1000)
                    Me.SendPropertyChanged("Result")
                End If
            End Set
        End Property


        Private _SubjectLocalized As String
        <DataMember()> _
        Public Property SubjectLocalized() As String
            Get
                Return Left(Me._SubjectLocalized, 100)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._SubjectLocalized, value) = False) Then
                    Me._SubjectLocalized = Left(value, 100)
                    Me.SendPropertyChanged("SubjectLocalized")
                End If
            End Set
        End Property

        Private _BodyLocalized As String
        <DataMember()> _
        Public Property BodyLocalized() As String
            Get
                Return Left(Me._BodyLocalized, 100)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BodyLocalized, value) = False) Then
                    Me._BodyLocalized = Left(value, 100)
                    Me.SendPropertyChanged("BodyLocalized")
                End If
            End Set
        End Property

        Private _SubjectKeys As String
        <DataMember()> _
        Public Property SubjectKeys() As String
            Get
                Return Left(Me._SubjectKeys, 4000)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._SubjectKeys, value) = False) Then
                    Me._SubjectKeys = Left(value, 4000)
                    Me.SendPropertyChanged("SubjectKeys")
                End If
            End Set
        End Property

        Private _BodyKeys As String
        <DataMember()> _
        Public Property BodyKeys() As String
            Get
                Return Left(Me._BodyKeys, 4000)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._BodyKeys, value) = False) Then
                    Me._BodyKeys = Left(value, 4000)
                    Me.SendPropertyChanged("BodyKeys")
                End If
            End Set
        End Property

        'Added By LVV on 8/15/16 for v-7.0.5.110 Enhance Mail Table
        Private _RefIDType As Integer
        <DataMember()> _
        Public Property RefIDType() As Integer
            Get
                Return _RefIDType
            End Get
            Set(ByVal value As Integer)
                _RefIDType = value
            End Set
        End Property

        'Added By LVV on 8/15/16 for v-7.0.5.110 Enhance Mail Table
        Private _RefID As String
        <DataMember()> _
        Public Property RefID() As String
            Get
                Return Left(Me._RefID, 50)
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._RefID, value) = False) Then
                    Me._RefID = Left(value, 50)
                    Me.SendPropertyChanged("RefID")
                End If
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New Mail
            instance = DirectCast(MemberwiseClone(), Mail)
            Return instance
        End Function

#End Region

    End Class
End Namespace
