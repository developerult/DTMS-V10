Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker


Namespace DataTransferObjects
    <DataContract()> _
    Public Class tblAlertMessage
        Inherits DTOBaseClass

#Region " Data Members"

        Private _AlertMessageControl As Long
        <DataMember()> _
        Public Property AlertMessageControl As Long
            Get
                Return _AlertMessageControl
            End Get
            Set(value As Long)
                If ((Me._AlertMessageControl = value) _
                   = False) Then
                    Me._AlertMessageControl = value
                    Me.SendPropertyChanged("AlertMessageControl")
                End If
            End Set
        End Property

        Private _AlertMessageProcedureControl As Integer
        <DataMember()> _
        Public Property AlertMessageProcedureControl As Integer
            Get
                Return _AlertMessageProcedureControl
            End Get
            Set(value As Integer)
                If ((Me._AlertMessageProcedureControl = value) _
                   = False) Then
                    Me._AlertMessageProcedureControl = value
                    Me.SendPropertyChanged("AlertMessageProcedureControl")
                End If
            End Set
        End Property

        Private _AlertMessageCompControl As Integer
        <DataMember()> _
        Public Property AlertMessageCompControl As Integer
            Get
                Return _AlertMessageCompControl
            End Get
            Set(value As Integer)
                If ((Me._AlertMessageCompControl = value) _
                   = False) Then
                    Me._AlertMessageCompControl = value
                    Me.SendPropertyChanged("AlertMessageCompControl")
                End If
            End Set
        End Property

        Private _AlertMessageCompNumber As Integer
        <DataMember()> _
        Public Property AlertMessageCompNumber As Integer
            Get
                Return _AlertMessageCompNumber
            End Get
            Set(value As Integer)
                If ((Me._AlertMessageCompNumber = value) _
                   = False) Then
                    Me._AlertMessageCompNumber = value
                    Me.SendPropertyChanged("AlertMessageCompNumber")
                End If
            End Set
        End Property

        Private _AlertMessageCarrierControl As Integer
        <DataMember()> _
        Public Property AlertMessageCarrierControl As Integer
            Get
                Return _AlertMessageCarrierControl
            End Get
            Set(value As Integer)
                If ((Me._AlertMessageCarrierControl = value) _
                   = False) Then
                    Me._AlertMessageCarrierControl = value
                    Me.SendPropertyChanged("AlertMessageCarrierControl")
                End If
            End Set
        End Property

        Private _AlertMessageCarrierNumber As Integer
        <DataMember()> _
        Public Property AlertMessageCarrierNumber As Integer
            Get
                Return _AlertMessageCarrierNumber
            End Get
            Set(value As Integer)
                If ((Me._AlertMessageCarrierNumber = value) _
                   = False) Then
                    Me._AlertMessageCarrierNumber = value
                    Me.SendPropertyChanged("AlertMessageCarrierNumber")
                End If
            End Set
        End Property

        Private _AlertMessageName As String
        <DataMember()> _
        Public Property AlertMessageName() As String
            Get
                Return Left(Me._AlertMessageName, 100)
            End Get
            Set(value As String)
                If (String.Equals(Me._AlertMessageName, value) = False) Then
                    Me._AlertMessageName = Left(value, 100)
                    Me.SendPropertyChanged("AlertMessageName")
                End If
            End Set
        End Property

        Private _AlertMessageDescription As String
        <DataMember()> _
        Public Property AlertMessageDescription() As String
            Get
                Return Left(Me._AlertMessageDescription, 4000)
            End Get
            Set(value As String)
                If (String.Equals(Me._AlertMessageDescription, value) = False) Then
                    Me._AlertMessageDescription = Left(value, 4000)
                    Me.SendPropertyChanged("AlertMessageDescription")
                End If
            End Set
        End Property

        Private _AlertMessageSubject As String
        <DataMember()> _
        Public Property AlertMessageSubject() As String
            Get
                Return Left(Me._AlertMessageSubject, 255)
            End Get
            Set(value As String)
                If (String.Equals(Me._AlertMessageSubject, value) = False) Then
                    Me._AlertMessageSubject = Left(value, 255)
                    Me.SendPropertyChanged("AlertMessageSubject")
                End If
            End Set
        End Property

        Private _AlertMessageBody As String
        <DataMember()> _
        Public Property AlertMessageBody() As String
            Get
                Return Me._AlertMessageBody
            End Get
            Set(value As String)
                If (String.Equals(Me._AlertMessageBody, value) = False) Then
                    Me._AlertMessageBody = value
                    Me.SendPropertyChanged("AlertMessageBody")
                End If
            End Set
        End Property

        Private _AlertMessageClientUpdated As Boolean
        <DataMember()> _
        Public Property AlertMessageClientUpdated As Boolean
            Get
                Return _AlertMessageClientUpdated
            End Get
            Set(value As Boolean)
                If ((Me._AlertMessageClientUpdated = value) = False) Then
                    Me._AlertMessageClientUpdated = value
                    Me.SendPropertyChanged("AlertMessageClientUpdated")
                End If
            End Set
        End Property

        Private _AlertMessageEmailsSent As Boolean
        <DataMember()> _
        Public Property AlertMessageEmailsSent As Boolean
            Get
                Return _AlertMessageEmailsSent
            End Get
            Set(value As Boolean)
                If ((Me._AlertMessageEmailsSent = value)  = False) Then
                    Me._AlertMessageEmailsSent = value
                    Me.SendPropertyChanged("AlertMessageEmailsSent")
                End If
            End Set
        End Property

        Private _AlertMessageNote1 As String
        <DataMember()> _
        Public Property AlertMessageNote1() As String
            Get
                Return Left(Me._AlertMessageNote1, 255)
            End Get
            Set(value As String)
                If (String.Equals(Me._AlertMessageNote1, value) = False) Then
                    Me._AlertMessageNote1 = Left(value, 255)
                    Me.SendPropertyChanged("AlertMessageNote1")
                End If
            End Set
        End Property

        Private _AlertMessageNote2 As String
        <DataMember()> _
        Public Property AlertMessageNote2() As String
            Get
                Return Left(Me._AlertMessageNote2, 255)
            End Get
            Set(value As String)
                If (String.Equals(Me._AlertMessageNote2, value) = False) Then
                    Me._AlertMessageNote2 = Left(value, 255)
                    Me.SendPropertyChanged("AlertMessageNote2")
                End If
            End Set
        End Property

        Private _AlertMessageNote3 As String
        <DataMember()> _
        Public Property AlertMessageNote3() As String
            Get
                Return Left(Me._AlertMessageNote3, 255)
            End Get
            Set(value As String)
                If (String.Equals(Me._AlertMessageNote3, value) = False) Then
                    Me._AlertMessageNote3 = Left(value, 255)
                    Me.SendPropertyChanged("AlertMessageNote3")
                End If
            End Set
        End Property

        Private _AlertMessageNote4 As String
        <DataMember()> _
        Public Property AlertMessageNote4() As String
            Get
                Return Left(Me._AlertMessageNote4, 255)
            End Get
            Set(value As String)
                If (String.Equals(Me._AlertMessageNote4, value) = False) Then
                    Me._AlertMessageNote4 = Left(value, 255)
                    Me.SendPropertyChanged("AlertMessageNote4")
                End If
            End Set
        End Property


        Private _AlertMessageNote5 As String
        <DataMember()> _
        Public Property AlertMessageNote5() As String
            Get
                Return Left(Me._AlertMessageNote5, 255)
            End Get
            Set(value As String)
                If (String.Equals(Me._AlertMessageNote5, value) = False) Then
                    Me._AlertMessageNote5 = Left(value, 255)
                    Me.SendPropertyChanged("AlertMessageNote5")
                End If
            End Set
        End Property

        Private _AlertMessageModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property AlertMessageModDate() As System.Nullable(Of Date)
            Get
                Return Me._AlertMessageModDate
            End Get
            Set(value As System.Nullable(Of Date))
                If (Me._AlertMessageModDate.Equals(value) = False) Then
                    Me._AlertMessageModDate = value
                    Me.SendPropertyChanged("AlertMessageModDate")
                End If
            End Set
        End Property

        Private _AlertMessageModUser As String
        <DataMember()> _
        Public Property AlertMessageModUser() As String
            Get
                Return Left(Me._AlertMessageModUser, 100)
            End Get
            Set(value As String)
                If (String.Equals(Me._AlertMessageModUser, value) = False) Then
                    Me._AlertMessageModUser = Left(value, 100)
                    Me.SendPropertyChanged("AlertMessageModUser")
                End If
            End Set
        End Property

        Private _AlertMessageUpdated As Byte()
        <DataMember()> _
        Public Property AlertMessageUpdated() As Byte()
            Get
                Return _AlertMessageUpdated
            End Get
            Set(ByVal value As Byte())
                _AlertMessageUpdated = value
            End Set
        End Property

#End Region



#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblAlertMessage
            instance = DirectCast(MemberwiseClone(), tblAlertMessage)
            Return instance
        End Function

#End Region
    End Class
End Namespace

