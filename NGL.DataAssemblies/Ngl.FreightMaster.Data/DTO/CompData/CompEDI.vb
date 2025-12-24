Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class CompEDI
        Inherits DTOBaseClass


#Region " Data Members"
        Private _CompEDIControl As Integer = 0
        <DataMember()> _
        Public Property CompEDIControl() As Integer
            Get
                Return _CompEDIControl
            End Get
            Set(ByVal value As Integer)
                _CompEDIControl = value
            End Set
        End Property

        Private _CompEDICompControl As Integer = 0
        <DataMember()> _
        Public Property CompEDICompControl() As Integer
            Get
                Return _CompEDICompControl
            End Get
            Set(ByVal value As Integer)
                _CompEDICompControl = value
            End Set
        End Property

        Private _CompEDIXaction As String = ""
        <DataMember()> _
        Public Property CompEDIXaction() As String
            Get
                Return Left(_CompEDIXaction, 3)
            End Get
            Set(ByVal value As String)
                _CompEDIXaction = Left(value, 3)
            End Set
        End Property

        Private _CompEDIComment As String = ""
        <DataMember()> _
        Public Property CompEDIComment() As String
            Get
                Return Left(_CompEDIComment, 50)
            End Get
            Set(ByVal value As String)
                _CompEDIComment = Left(value, 50)
            End Set
        End Property

        Private _CompEDISecurityQual As String = ""
        <DataMember()> _
        Public Property CompEDISecurityQual() As String
            Get
                Return Left(_CompEDISecurityQual, 2)
            End Get
            Set(ByVal value As String)
                _CompEDISecurityQual = Left(value, 2)
            End Set
        End Property

        Private _CompEDISecurityCode As String = ""
        <DataMember()> _
        Public Property CompEDISecurityCode() As String
            Get
                Return Left(_CompEDISecurityCode, 10)
            End Get
            Set(ByVal value As String)
                _CompEDISecurityCode = Left(value, 10)
            End Set
        End Property

        Private _CompEDIPartnerQual As String = ""
        <DataMember()> _
        Public Property CompEDIPartnerQual() As String
            Get
                Return Left(_CompEDIPartnerQual, 2)
            End Get
            Set(ByVal value As String)
                _CompEDIPartnerQual = Left(value, 2)
            End Set
        End Property

        Private _CompEDIPartnerCode As String = ""
        <DataMember()> _
        Public Property CompEDIPartnerCode() As String
            Get
                Return Left(_CompEDIPartnerCode, 15)
            End Get
            Set(ByVal value As String)
                _CompEDIPartnerCode = Left(value, 15)
            End Set
        End Property

        Private _CompEDIISASequence As Integer = 0
        <DataMember()> _
        Public Property CompEDIISASequence() As Integer
            Get
                Return _CompEDIISASequence
            End Get
            Set(ByVal value As Integer)
                _CompEDIISASequence = value
            End Set
        End Property

        Private _CompEDISequence As Integer = 0
        <DataMember()> _
        Public Property CompEDISequence() As Integer
            Get
                Return _CompEDISequence
            End Get
            Set(ByVal value As Integer)
                _CompEDISequence = value
            End Set
        End Property

        Private _CompEDIEmailNotificationOn As Boolean = False
        <DataMember()> _
        Public Property CompEDIEmailNotificationOn() As Boolean
            Get
                Return _CompEDIEmailNotificationOn
            End Get
            Set(ByVal value As Boolean)
                _CompEDIEmailNotificationOn = value
            End Set
        End Property

        Private _CompEDIEmailAddress As String = ""
        <DataMember()> _
        Public Property CompEDIEmailAddress() As String
            Get
                Return Left(_CompEDIEmailAddress, 255)
            End Get
            Set(ByVal value As String)
                _CompEDIEmailAddress = Left(value, 255)
            End Set
        End Property

        Private _CompEDIAcknowledgementRequested As Boolean = False
        <DataMember()> _
        Public Property CompEDIAcknowledgementRequested() As Boolean
            Get
                Return _CompEDIAcknowledgementRequested
            End Get
            Set(ByVal value As Boolean)
                _CompEDIAcknowledgementRequested = value
            End Set
        End Property

        Private _CompEDIAcceptOn997 As Boolean = True
        <DataMember()> _
        Public Property CompEDIAcceptOn997() As Boolean
            Get
                Return _CompEDIAcceptOn997
            End Get
            Set(ByVal value As Boolean)
                _CompEDIAcceptOn997 = value
            End Set
        End Property

        Private _CompEDIMethodOfPayment As String = ""
        <DataMember()>
        Public Property CompEDIMethodOfPayment() As String
            Get
                Return Left(_CompEDIMethodOfPayment, 2)
            End Get
            Set(ByVal value As String)
                _CompEDIMethodOfPayment = Left(value, 2)
            End Set
        End Property

        Private _CompEDIUpdated As Byte()
        <DataMember()>
        Public Property CompEDIUpdated() As Byte()
            Get
                Return _CompEDIUpdated
            End Get
            Set(ByVal value As Byte())
                _CompEDIUpdated = value
            End Set
        End Property

        Private _CompEDIModDate As System.Nullable(Of Date) 'added for CompEDI new changes ManoRama 12AUG2020'
        <DataMember()>
        Public Property CompEDIModDate() As System.Nullable(Of Date)
            Get
                Return _CompEDIModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _CompEDIModDate = value
            End Set
        End Property

        Private _CompEDIModUser As String = "" 'added for CompEDI new changes ManoRama 12AUG2020'
        <DataMember()>
        Public Property CompEDIModUser() As String
            Get
                Return Left(_CompEDIModUser, 100)
            End Get
            Set(ByVal value As String)
                _CompEDIModUser = Left(value, 100)
            End Set
        End Property
#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CompEDI
            instance = DirectCast(MemberwiseClone(), CompEDI)
            Return instance
        End Function

#End Region

    End Class
End Namespace