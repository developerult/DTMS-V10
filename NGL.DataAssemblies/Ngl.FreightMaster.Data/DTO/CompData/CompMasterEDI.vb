Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class CompMasterEDI
        Inherits DTOBaseClass


#Region " Data Members"
        Private _CompMasterEDIControl As Integer = 0
        <DataMember()> _
        Public Property CompMasterEDIControl() As Integer
            Get
                Return _CompMasterEDIControl
            End Get
            Set(ByVal value As Integer)
                _CompMasterEDIControl = value
            End Set
        End Property

        Private _CompMasterEDIXaction As String = ""
        <DataMember()> _
        Public Property CompMasterEDIXaction() As String
            Get
                Return Left(_CompMasterEDIXaction, 3)
            End Get
            Set(ByVal value As String)
                _CompMasterEDIXaction = Left(value, 3)
            End Set
        End Property

        Private _CompMasterEDIComment As String = ""
        <DataMember()> _
        Public Property CompMasterEDIComment() As String
            Get
                Return Left(_CompMasterEDIComment, 50)
            End Get
            Set(ByVal value As String)
                _CompMasterEDIComment = Left(value, 50)
            End Set
        End Property

        Private _CompMasterEDISecurityQual As String = ""
        <DataMember()> _
        Public Property CompMasterEDISecurityQual() As String
            Get
                Return Left(_CompMasterEDISecurityQual, 2)
            End Get
            Set(ByVal value As String)
                _CompMasterEDISecurityQual = Left(value, 2)
            End Set
        End Property

        Private _CompMasterEDISecurityCode As String = ""
        <DataMember()> _
        Public Property CompMasterEDISecurityCode() As String
            Get
                Return Left(_CompMasterEDISecurityCode, 10)
            End Get
            Set(ByVal value As String)
                _CompMasterEDISecurityCode = Left(value, 10)
            End Set
        End Property

        Private _CompMasterEDIPartnerQual As String = ""
        <DataMember()> _
        Public Property CompMasterEDIPartnerQual() As String
            Get
                Return Left(_CompMasterEDIPartnerQual, 2)
            End Get
            Set(ByVal value As String)
                _CompMasterEDIPartnerQual = Left(value, 2)
            End Set
        End Property

        Private _CompMasterEDIPartnerCode As String = ""
        <DataMember()> _
        Public Property CompMasterEDIPartnerCode() As String
            Get
                Return Left(_CompMasterEDIPartnerCode, 15)
            End Get
            Set(ByVal value As String)
                _CompMasterEDIPartnerCode = Left(value, 15)
            End Set
        End Property

        Private _CompMasterEDIISASequence As Integer = 0
        <DataMember()> _
        Public Property CompMasterEDIISASequence() As Integer
            Get
                Return _CompMasterEDIISASequence
            End Get
            Set(ByVal value As Integer)
                _CompMasterEDIISASequence = value
            End Set
        End Property

        Private _CompMasterEDISequence As Integer = 0
        <DataMember()> _
        Public Property CompMasterEDISequence() As Integer
            Get
                Return _CompMasterEDISequence
            End Get
            Set(ByVal value As Integer)
                _CompMasterEDISequence = value
            End Set
        End Property

        Private _CompMasterEDIEmailNotificationOn As Boolean = False
        <DataMember()> _
        Public Property CompMasterEDIEmailNotificationOn() As Boolean
            Get
                Return _CompMasterEDIEmailNotificationOn
            End Get
            Set(ByVal value As Boolean)
                _CompMasterEDIEmailNotificationOn = value
            End Set
        End Property

        Private _CompMasterEDIEmailAddress As String = ""
        <DataMember()> _
        Public Property CompMasterEDIEmailAddress() As String
            Get
                Return Left(_CompMasterEDIEmailAddress, 255)
            End Get
            Set(ByVal value As String)
                _CompMasterEDIEmailAddress = Left(value, 255)
            End Set
        End Property

        Private _CompMasterEDIAcknowledgementRequested As Boolean = False
        <DataMember()> _
        Public Property CompMasterEDIAcknowledgementRequested() As Boolean
            Get
                Return _CompMasterEDIAcknowledgementRequested
            End Get
            Set(ByVal value As Boolean)
                _CompMasterEDIAcknowledgementRequested = value
            End Set
        End Property

        Private _CompMasterEDIAcceptOn997 As Boolean = False
        <DataMember()> _
        Public Property CompMasterEDIAcceptOn997() As Boolean
            Get
                Return _CompMasterEDIAcceptOn997
            End Get
            Set(ByVal value As Boolean)
                _CompMasterEDIAcceptOn997 = value
            End Set
        End Property

        Private _CompMasterEDIMethodOfPayment As String = ""
        <DataMember()> _
        Public Property CompMasterEDIMethodOfPayment() As String
            Get
                Return Left(_CompMasterEDIMethodOfPayment, 2)
            End Get
            Set(ByVal value As String)
                _CompMasterEDIMethodOfPayment = Left(value, 2)
            End Set
        End Property

        Private _CompMasterEDIUpdated As Byte()
        <DataMember()>
        Public Property CompMasterEDIUpdated() As Byte()
            Get
                Return _CompMasterEDIUpdated
            End Get
            Set(ByVal value As Byte())
                _CompMasterEDIUpdated = value
            End Set
        End Property

        Private _CompMasterEDIModDate As System.Nullable(Of Date) 'added for CompMasterEDI new changes suhas 12AUG2020'
        <DataMember()>
        Public Property CompMasterEDIModDate() As System.Nullable(Of Date)
            Get
                Return _CompMasterEDIModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _CompMasterEDIModDate = value
            End Set
        End Property

        Private _CompMasterEDIModUser As String = "" 'added for CompMasterEDI new changes suhas 12AUG2020'
        <DataMember()>
        Public Property CompMasterEDIModUser() As String
            Get
                Return Left(_CompMasterEDIModUser, 100)
            End Get
            Set(ByVal value As String)
                _CompMasterEDIModUser = Left(value, 100)
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CompMasterEDI
            instance = DirectCast(MemberwiseClone(), CompMasterEDI)
            Return instance
        End Function

#End Region

    End Class
End Namespace