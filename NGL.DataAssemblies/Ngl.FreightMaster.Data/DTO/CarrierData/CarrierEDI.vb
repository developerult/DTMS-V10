Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class CarrierEDI
        Inherits DTOBaseClass


#Region " Data Members"
        Private _CarrierEDIControl As Integer = 0
        <DataMember()> _
        Public Property CarrierEDIControl() As Integer
            Get
                Return _CarrierEDIControl
            End Get
            Set(ByVal value As Integer)
                _CarrierEDIControl = value
            End Set
        End Property

        Private _CarrierEDICarrierControl As Integer = 0
        <DataMember()> _
        Public Property CarrierEDICarrierControl() As Integer
            Get
                Return _CarrierEDICarrierControl
            End Get
            Set(ByVal value As Integer)
                _CarrierEDICarrierControl = value
            End Set
        End Property

        Private _CarrierEDIXaction As String = ""
        <DataMember()> _
        Public Property CarrierEDIXaction() As String
            Get
                Return Left(_CarrierEDIXaction, 3)
            End Get
            Set(ByVal value As String)
                _CarrierEDIXaction = Left(value, 3)
            End Set
        End Property

        Private _CarrierEDIComment As String = ""
        <DataMember()> _
        Public Property CarrierEDIComment() As String
            Get
                Return Left(_CarrierEDIComment, 50)
            End Get
            Set(ByVal value As String)
                _CarrierEDIComment = Left(value, 50)
            End Set
        End Property

        Private _CarrierEDISecurityQual As String = ""
        <DataMember()> _
        Public Property CarrierEDISecurityQual() As String
            Get
                Return Left(_CarrierEDISecurityQual, 2)
            End Get
            Set(ByVal value As String)
                _CarrierEDISecurityQual = Left(value, 2)
            End Set
        End Property

        Private _CarrierEDISecurityCode As String = ""
        <DataMember()> _
        Public Property CarrierEDISecurityCode() As String
            Get
                Return Left(_CarrierEDISecurityCode, 10)
            End Get
            Set(ByVal value As String)
                _CarrierEDISecurityCode = Left(value, 10)
            End Set
        End Property

        Private _CarrierEDIPartnerQual As String = ""
        <DataMember()> _
        Public Property CarrierEDIPartnerQual() As String
            Get
                Return Left(_CarrierEDIPartnerQual, 2)
            End Get
            Set(ByVal value As String)
                _CarrierEDIPartnerQual = Left(value, 2)
            End Set
        End Property

        Private _CarrierEDIPartnerCode As String = ""
        <DataMember()> _
        Public Property CarrierEDIPartnerCode() As String
            Get
                Return Left(_CarrierEDIPartnerCode, 15)
            End Get
            Set(ByVal value As String)
                _CarrierEDIPartnerCode = Left(value, 15)
            End Set
        End Property

        Private _CarrierEDIISASequence As Integer = 0
        <DataMember()> _
        Public Property CarrierEDIISASequence() As Integer
            Get
                Return _CarrierEDIISASequence
            End Get
            Set(ByVal value As Integer)
                _CarrierEDIISASequence = value
            End Set
        End Property

        Private _CarrierEDIGSSequence As Integer = 0
        <DataMember()> _
        Public Property CarrierEDIGSSequence() As Integer
            Get
                Return _CarrierEDIGSSequence
            End Get
            Set(ByVal value As Integer)
                _CarrierEDIGSSequence = value
            End Set
        End Property

        Private _CarrierEDIEmailNotificationOn As Boolean = False
        <DataMember()> _
        Public Property CarrierEDIEmailNotificationOn() As Boolean
            Get
                Return _CarrierEDIEmailNotificationOn
            End Get
            Set(ByVal value As Boolean)
                _CarrierEDIEmailNotificationOn = value
            End Set
        End Property

        Private _CarrierEDIEmailAddress As String = ""
        <DataMember()> _
        Public Property CarrierEDIEmailAddress() As String
            Get
                Return Left(_CarrierEDIEmailAddress, 255)
            End Get
            Set(ByVal value As String)
                _CarrierEDIEmailAddress = Left(value, 255)
            End Set
        End Property

        Private _CarrierEDIAcknowledgementRequested As Boolean = False
        <DataMember()> _
        Public Property CarrierEDIAcknowledgementRequested() As Boolean
            Get
                Return _CarrierEDIAcknowledgementRequested
            End Get
            Set(ByVal value As Boolean)
                _CarrierEDIAcknowledgementRequested = value
            End Set
        End Property

        Private _CarrierEDIAcceptOn997 As Boolean = False
        <DataMember()> _
        Public Property CarrierEDIAcceptOn997() As Boolean
            Get
                Return _CarrierEDIAcceptOn997
            End Get
            Set(ByVal value As Boolean)
                _CarrierEDIAcceptOn997 = value
            End Set
        End Property

        Private _CarrierEDITestCode As String = ""
        <DataMember()> _
        Public Property CarrierEDITestCode() As String
            Get
                Return Left(_CarrierEDITestCode, 1)
            End Get
            Set(ByVal value As String)
                _CarrierEDITestCode = Left(value, 1)
            End Set
        End Property

        Private _CarrierEDIUpdated As Byte()
        <DataMember()> _
        Public Property CarrierEDIUpdated() As Byte()
            Get
                Return _CarrierEDIUpdated
            End Get
            Set(ByVal value As Byte())
                _CarrierEDIUpdated = value
            End Set
        End Property

        Private _CarrierEDIInboundFolder As String = ""
        <DataMember()> _
        Public Property CarrierEDIInboundFolder() As String
            Get
                Return Left(_CarrierEDIInboundFolder, 255)
            End Get
            Set(ByVal value As String)
                _CarrierEDIInboundFolder = Left(value, 255)
            End Set
        End Property

        Private _CarrierEDIBackupFolder As String = ""
        <DataMember()> _
        Public Property CarrierEDIBackupFolder() As String
            Get
                Return Left(_CarrierEDIBackupFolder, 255)
            End Get
            Set(ByVal value As String)
                _CarrierEDIBackupFolder = Left(value, 255)
            End Set
        End Property

        Private _CarrierEDILogFile As String = ""
        <DataMember()> _
        Public Property CarrierEDILogFile() As String
            Get
                Return Left(_CarrierEDILogFile, 255)
            End Get
            Set(ByVal value As String)
                _CarrierEDILogFile = Left(value, 255)
            End Set
        End Property

        Private _CarrierEDIStartTime As String = ""
        <DataMember()> _
        Public Property CarrierEDIStartTime() As String
            Get
                Return Left(_CarrierEDIStartTime, 20)
            End Get
            Set(ByVal value As String)
                _CarrierEDIStartTime = Left(value, 20)
            End Set
        End Property

        Private _CarrierEDIEndTime As String = ""
        <DataMember()> _
        Public Property CarrierEDIEndTime() As String
            Get
                Return Left(_CarrierEDIEndTime, 20)
            End Get
            Set(ByVal value As String)
                _CarrierEDIEndTime = Left(value, 20)
            End Set
        End Property

        Private _CarrierEDIDaysOfWeek As String = ""
        <DataMember()> _
        Public Property CarrierEDIDaysOfWeek() As String
            Get
                Return Left(_CarrierEDIDaysOfWeek, 50)
            End Get
            Set(ByVal value As String)
                _CarrierEDIDaysOfWeek = Left(value, 50)
            End Set
        End Property

        Private _CarrierEDISendMinutesOutbound As Integer = 0
        <DataMember()> _
        Public Property CarrierEDISendMinutesOutbound() As Integer
            Get
                Return _CarrierEDISendMinutesOutbound
            End Get
            Set(ByVal value As Integer)
                _CarrierEDISendMinutesOutbound = value
            End Set
        End Property

        Private _CarrierEDIFileNameBaseOutbound As String = ""
        <DataMember()> _
        Public Property CarrierEDIFileNameBaseOutbound() As String
            Get
                Return Left(_CarrierEDIFileNameBaseOutbound, 20)
            End Get
            Set(ByVal value As String)
                _CarrierEDIFileNameBaseOutbound = Left(value, 20)
            End Set
        End Property

        Private _CarrierEDIFileNameBaseInbound As String = ""
        <DataMember()> _
        Public Property CarrierEDIFileNameBaseInbound() As String
            Get
                Return Left(_CarrierEDIFileNameBaseInbound, 20)
            End Get
            Set(ByVal value As String)
                _CarrierEDIFileNameBaseInbound = Left(value, 20)
            End Set
        End Property

        Private _CarrierEDIWebServiceAuthKey As String = ""
        <DataMember()> _
        Public Property CarrierEDIWebServiceAuthKey() As String
            Get
                Return Left(_CarrierEDIWebServiceAuthKey, 50)
            End Get
            Set(ByVal value As String)
                _CarrierEDIWebServiceAuthKey = Left(value, 50)
            End Set
        End Property

        Private _CarrierEDINGLEDIInputWebURL As String = ""
        <DataMember()> _
        Public Property CarrierEDINGLEDIInputWebURL() As String
            Get
                Return Left(_CarrierEDINGLEDIInputWebURL, 100)
            End Get
            Set(ByVal value As String)
                _CarrierEDINGLEDIInputWebURL = Left(value, 100)
            End Set
        End Property

        Private _CarrierEDINGLEDI204OutputWebURL As String = ""
        <DataMember()> _
        Public Property CarrierEDINGLEDI204OutputWebURL() As String
            Get
                Return Left(_CarrierEDINGLEDI204OutputWebURL, 1000)
            End Get
            Set(ByVal value As String)
                _CarrierEDINGLEDI204OutputWebURL = Left(value, 1000)
            End Set
        End Property

        Private _CarrierEDIOutboundFolder As String = ""
        <DataMember()> _
        Public Property CarrierEDIOutboundFolder() As String
            Get
                Return Left(_CarrierEDIOutboundFolder, 255)
            End Get
            Set(ByVal value As String)
                _CarrierEDIOutboundFolder = Left(value, 255)
            End Set
        End Property

        Private _CarrierEDILastOutboundTransmission As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CarrierEDILastOutboundTransmission() As System.Nullable(Of Date)
            Get
                Return _CarrierEDILastOutboundTransmission
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _CarrierEDILastOutboundTransmission = value
            End Set
        End Property

        Private _CarrierEDIFTPOutboundFolder As String = ""
        <DataMember()> _
        Public Property CarrierEDIFTPOutboundFolder() As String
            Get
                Return Left(_CarrierEDIFTPOutboundFolder, 255)
            End Get
            Set(ByVal value As String)
                _CarrierEDIFTPOutboundFolder = Left(value, 255)
            End Set
        End Property

        Private _CarrierEDIFTPBackupFolder As String = ""
        <DataMember()> _
        Public Property CarrierEDIFTPBackupFolder() As String
            Get
                Return Left(_CarrierEDIFTPBackupFolder, 255)
            End Get
            Set(ByVal value As String)
                _CarrierEDIFTPBackupFolder = Left(value, 255)
            End Set
        End Property

        Private _CarrierEDIFTPInboundFolder As String = ""
        <DataMember()> _
        Public Property CarrierEDIFTPInboundFolder() As String
            Get
                Return Left(_CarrierEDIFTPInboundFolder, 255)
            End Get
            Set(ByVal value As String)
                _CarrierEDIFTPInboundFolder = Left(value, 255)
            End Set
        End Property

        Private _CarrierEDIFTPServer As String = ""
        <DataMember()> _
        Public Property CarrierEDIFTPServer() As String
            Get
                Return Left(_CarrierEDIFTPServer, 255)
            End Get
            Set(ByVal value As String)
                _CarrierEDIFTPServer = Left(value, 255)
            End Set
        End Property

        Private _CarrierEDIFTPUserName As String = ""
        <DataMember()> _
        Public Property CarrierEDIFTPUserName() As String
            Get
                Return Left(_CarrierEDIFTPUserName, 255)
            End Get
            Set(ByVal value As String)
                _CarrierEDIFTPUserName = Left(value, 255)
            End Set
        End Property

        Private _CarrierEDIFTPPassword As String = ""
        <DataMember()> _
        Public Property CarrierEDIFTPPassword() As String
            Get
                Return Left(_CarrierEDIFTPPassword, 255)
            End Get
            Set(ByVal value As String)
                _CarrierEDIFTPPassword = Left(value, 255)
            End Set
        End Property

        Private _CarrierEDIWebServiceURL As String = ""
        <DataMember()> _
        Public Property CarrierEDIWebServiceURL() As String
            Get
                Return Left(_CarrierEDIWebServiceURL, 255)
            End Get
            Set(ByVal value As String)
                _CarrierEDIWebServiceURL = Left(value, 255)
            End Set
        End Property
        Private _CarrierEDIModDate As System.Nullable(Of Date) 'added for CarrierEDI new changes ManoRama 19AUG2020'
        <DataMember()>
        Public Property CarrierEDIModDate() As System.Nullable(Of Date)
            Get
                Return _CarrierEDIModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _CarrierEDIModDate = value
            End Set
        End Property

        Private _CarrierEDIModUser As String = "" 'added for CarrierEDI new changes ManoRama 19AUG2020'
        <DataMember()>
        Public Property CarrierEDIModUser() As String
            Get
                Return Left(_CarrierEDIModUser, 100)
            End Get
            Set(ByVal value As String)
                _CarrierEDIModUser = Left(value, 100)
            End Set
        End Property
#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CarrierEDI
            instance = DirectCast(MemberwiseClone(), CarrierEDI)
            Return instance
        End Function

#End Region

    End Class
End Namespace
