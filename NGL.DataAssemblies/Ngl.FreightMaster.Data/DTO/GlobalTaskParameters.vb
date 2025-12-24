Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization

Namespace DataTransferObjects

    <DataContract()> _
    Public Class GlobalTaskParameters : Implements ICloneable


#Region " Implements ICloneable"
        Private Function ICloneable_Clone() As Object Implements ICloneable.Clone
            Return Clone()
        End Function
#End Region

#Region " Data Members"


        Private _GlobalAutoRetry As Integer = 3
        <DataMember()> _
        Public Property GlobalAutoRetry() As Integer
            Get
                Return _GlobalAutoRetry
            End Get
            Set(ByVal value As Integer)
                _GlobalAutoRetry = value
            End Set
        End Property
        
        Private _GlobalAdminEmail As String = "support@nextgeneration.com"
        <DataMember()> _
        Public Property GlobalAdminEmail() As String
            Get
                Return _GlobalAdminEmail
            End Get
            Set(ByVal value As String)
                _GlobalAdminEmail = value
            End Set
        End Property

        Private _GlobalGroupEmail As String = "support@nextgeneration.com"
        <DataMember()> _
        Public Property GlobalGroupEmail() As String
            Get
                Return _GlobalGroupEmail
            End Get
            Set(ByVal value As String)
                _GlobalGroupEmail = value
            End Set
        End Property

        Private _GlobalFromEmail As String = "system@nextgeneration.com"
        <DataMember()> _
        Public Property GlobalFromEmail() As String
            Get
                Return _GlobalFromEmail
            End Get
            Set(ByVal value As String)
                _GlobalFromEmail = value
            End Set
        End Property

        Private _GlobalSMTPServer As String = "Mail.NGL.Local"
        <DataMember()> _
        Public Property GlobalSMTPServer() As String
            Get
                Return _GlobalSMTPServer
            End Get
            Set(ByVal value As String)
                _GlobalSMTPServer = value
            End Set
        End Property

        Private _GlobalSaveOldLogs As Boolean = False
        <DataMember()> _
        Public Property GlobalSaveOldLogs() As Boolean
            Get
                Return _GlobalSaveOldLogs
            End Get
            Set(ByVal value As Boolean)
                _GlobalSaveOldLogs = value
            End Set
        End Property

        Private _GlobalKeepLogDays As Integer = 7
        <DataMember()> _
        Public Property GlobalKeepLogDays() As Integer
            Get
                Return _GlobalKeepLogDays
            End Get
            Set(ByVal value As Integer)
                _GlobalKeepLogDays = value
            End Set
        End Property

        Private _GlobalDefaultLoadAcceptAllowedMinutes As Integer = 120
        <DataMember()> _
        Public Property GlobalDefaultLoadAcceptAllowedMinutes() As Integer
            Get
                Return _GlobalDefaultLoadAcceptAllowedMinutes
            End Get
            Set(ByVal value As Integer)
                _GlobalDefaultLoadAcceptAllowedMinutes = value
            End Set
        End Property

        Private _GlobalDebugMode As Boolean = False
        <DataMember()> _
        Public Property GlobalDebugMode() As Boolean
            Get
                Return _GlobalDebugMode
            End Get
            Set(ByVal value As Boolean)
                _GlobalDebugMode = value
            End Set
        End Property
        Private _GlobalFuelIndexUpdateEmailNotificationValue As Boolean = False
        <DataMember()> _
        Public Property GlobalFuelIndexUpdateEmailNotificationValue() As Boolean
            Get
                Return _GlobalFuelIndexUpdateEmailNotificationValue
            End Get
            Set(ByVal value As Boolean)
                _GlobalFuelIndexUpdateEmailNotificationValue = value
            End Set
        End Property
        Private _GlobalFuelIndexUpdateEmailNotification As String = ""
        <DataMember()> _
        Public Property GlobalFuelIndexUpdateEmailNotification() As String
            Get
                Return _GlobalFuelIndexUpdateEmailNotification
            End Get
            Set(ByVal value As String)
                _GlobalFuelIndexUpdateEmailNotification = value
            End Set
        End Property
        Private _GlobalCarrierContractExpiredEmailNotificationValue As Boolean = False
        <DataMember()> _
        Public Property GlobalCarrierContractExpiredEmailNotificationValue() As Boolean
            Get
                Return _GlobalCarrierContractExpiredEmailNotificationValue
            End Get
            Set(ByVal value As Boolean)
                _GlobalCarrierContractExpiredEmailNotificationValue = value
            End Set
        End Property
        Private _GlobalCarrierContractExpiredEmailNotification As String = ""
        <DataMember()> _
        Public Property GlobalCarrierContractExpiredEmailNotification() As String
            Get
                Return _GlobalCarrierContractExpiredEmailNotification
            End Get
            Set(ByVal value As String)
                _GlobalCarrierContractExpiredEmailNotification = value
            End Set
        End Property
        Private _GlobalCarrierExposureAllEmailNotificationValue As Boolean = False
        <DataMember()> _
        Public Property GlobalCarrierExposureAllEmailNotificationValue() As Boolean
            Get
                Return _GlobalCarrierExposureAllEmailNotificationValue
            End Get
            Set(ByVal value As Boolean)
                _GlobalCarrierExposureAllEmailNotificationValue = value
            End Set
        End Property
        Private _GlobalCarrierExposureAllEmailNotification As String = ""
        <DataMember()> _
        Public Property GlobalCarrierExposureAllEmailNotification() As String
            Get
                Return _GlobalCarrierExposureAllEmailNotification
            End Get
            Set(ByVal value As String)
                _GlobalCarrierExposureAllEmailNotification = value
            End Set
        End Property
        Private _GlobalCarrierExposurePerShipmentEmailNotificationValue As Boolean = False
        <DataMember()> _
        Public Property GlobalCarrierExposurePerShipmentEmailNotificationValue() As Boolean
            Get
                Return _GlobalCarrierExposurePerShipmentEmailNotificationValue
            End Get
            Set(ByVal value As Boolean)
                _GlobalCarrierExposurePerShipmentEmailNotificationValue = value
            End Set
        End Property
        Private _GlobalCarrierExposurePerShipmentEmailNotification As String = ""
        <DataMember()> _
        Public Property GlobalCarrierExposurePerShipmentEmailNotification() As String
            Get
                Return _GlobalCarrierExposurePerShipmentEmailNotification
            End Get
            Set(ByVal value As String)
                _GlobalCarrierExposurePerShipmentEmailNotification = value
            End Set
        End Property
        Private _GlobalCarrierInsuranceExpiredEmailNotificationValue As Boolean = False
        <DataMember()> _
        Public Property GlobalCarrierInsuranceExpiredEmailNotificationValue() As Boolean
            Get
                Return _GlobalCarrierInsuranceExpiredEmailNotificationValue
            End Get
            Set(ByVal value As Boolean)
                _GlobalCarrierInsuranceExpiredEmailNotificationValue = value
            End Set
        End Property
        Private _GlobalCarrierInsuranceExpiredEmailNotification As String = ""
        <DataMember()> _
        Public Property GlobalCarrierInsuranceExpiredEmailNotification() As String
            Get
                Return _GlobalCarrierInsuranceExpiredEmailNotification
            End Get
            Set(ByVal value As String)
                _GlobalCarrierInsuranceExpiredEmailNotification = value
            End Set
        End Property
        Private _GlobalOutdatedNoLanePOEmailNotificationValue As Boolean = False
        <DataMember()> _
        Public Property GlobalOutdatedNoLanePOEmailNotificationValue() As Boolean
            Get
                Return _GlobalOutdatedNoLanePOEmailNotificationValue
            End Get
            Set(ByVal value As Boolean)
                _GlobalOutdatedNoLanePOEmailNotificationValue = value
            End Set
        End Property
        Private _GlobalOutdatedNoLanePOEmailNotification As String = ""
        <DataMember()> _
        Public Property GlobalOutdatedNoLanePOEmailNotification() As String
            Get
                Return _GlobalOutdatedNoLanePOEmailNotification
            End Get
            Set(ByVal value As String)
                _GlobalOutdatedNoLanePOEmailNotification = value
            End Set
        End Property
        Private _GlobalOutdatedNStatusEmailNotificationValue As Boolean = False
        <DataMember()> _
        Public Property GlobalOutdatedNStatusEmailNotificationValue() As Boolean
            Get
                Return _GlobalOutdatedNStatusEmailNotificationValue
            End Get
            Set(ByVal value As Boolean)
                _GlobalOutdatedNStatusEmailNotificationValue = value
            End Set
        End Property
        Private _GlobalOutdatedNStatusEmailNotification As String = ""
        <DataMember()> _
        Public Property GlobalOutdatedNStatusEmailNotification() As String
            Get
                Return _GlobalOutdatedNStatusEmailNotification
            End Get
            Set(ByVal value As String)
                _GlobalOutdatedNStatusEmailNotification = value
            End Set
        End Property
        Private _GlobalPOsWaitingEmailNotificationValue As Boolean = False
        <DataMember()> _
        Public Property GlobalPOsWaitingEmailNotificationValue() As Boolean
            Get
                Return _GlobalPOsWaitingEmailNotificationValue
            End Get
            Set(ByVal value As Boolean)
                _GlobalPOsWaitingEmailNotificationValue = value
            End Set
        End Property
        Private _GlobalPOsWaitingEmailNotification As String = ""
        <DataMember()> _
        Public Property GlobalPOsWaitingEmailNotification() As String
            Get
                Return _GlobalPOsWaitingEmailNotification
            End Get
            Set(ByVal value As String)
                _GlobalPOsWaitingEmailNotification = value
            End Set
        End Property
        Private _NEXTStopAcctNo As String = ""
        <DataMember()> _
        Public Property NEXTStopAcctNo() As String
            Get
                Return _NEXTStopAcctNo
            End Get
            Set(ByVal value As String)
                _NEXTStopAcctNo = value
            End Set
        End Property

        Private _NEXTStopContact As String = ""
        <DataMember()> _
        Public Property NEXTStopContact() As String
            Get
                Return _NEXTStopContact
            End Get
            Set(ByVal value As String)
                _NEXTStopContact = value
            End Set
        End Property

        Private _NEXTStopHotLoadAccountName As String = ""
        <DataMember()> _
        Public Property NEXTStopHotLoadAccountName() As String
            Get
                Return _NEXTStopHotLoadAccountName
            End Get
            Set(ByVal value As String)
                _NEXTStopHotLoadAccountName = value
            End Set
        End Property

        Private _NEXTStopHotLoadContact As String = ""
        <DataMember()> _
        Public Property NEXTStopHotLoadContact() As String
            Get
                Return _NEXTStopHotLoadContact
            End Get
            Set(ByVal value As String)
                _NEXTStopHotLoadContact = value
            End Set
        End Property

        Private _NEXTStopHotLoadURL As String = ""
        <DataMember()> _
        Public Property NEXTStopHotLoadURL() As String
            Get
                Return _NEXTStopHotLoadURL
            End Get
            Set(ByVal value As String)
                _NEXTStopHotLoadURL = value
            End Set
        End Property

        Private _NEXTStopPhone As String = ""
        <DataMember()> _
        Public Property NEXTStopPhone() As String
            Get
                Return _NEXTStopPhone
            End Get
            Set(ByVal value As String)
                _NEXTStopPhone = value
            End Set
        End Property

        Private _NEXTStopURL As String = ""
        <DataMember()> _
        Public Property NEXTStopURL() As String
            Get
                Return _NEXTStopURL
            End Get
            Set(ByVal value As String)
                _NEXTStopURL = value
            End Set
        End Property

        Private _NEXTrackURL As String = ""
        <DataMember()> _
        Public Property NEXTrackURL() As String
            Get
                Return _NEXTrackURL
            End Get
            Set(ByVal value As String)
                _NEXTrackURL = value
            End Set
        End Property


        Private _NEXTRackDatabase As String = ""
        <DataMember()> _
        Public Property NEXTRackDatabase() As String
            Get
                Return _NEXTRackDatabase
            End Get
            Set(ByVal value As String)
                _NEXTRackDatabase = value
            End Set
        End Property

        Private _NEXTRackDatabaseServer As String = ""
        <DataMember()> _
        Public Property NEXTRackDatabaseServer() As String
            Get
                Return _NEXTRackDatabaseServer
            End Get
            Set(ByVal value As String)
                _NEXTRackDatabaseServer = value
            End Set
        End Property

        Private _GlobalSMTPUser As String = ""
        <DataMember()> _
        Public Property GlobalSMTPUser() As String
            Get
                Return _GlobalSMTPUser
            End Get
            Set(ByVal value As String)
                _GlobalSMTPUser = value
            End Set
        End Property

        Private _GlobalSMTPPass As String = ""
        <DataMember()> _
        Public Property GlobalSMTPPass() As String
            Get
                Return _GlobalSMTPPass
            End Get
            Set(ByVal value As String)
                _GlobalSMTPPass = value
            End Set
        End Property

        Private _ReportServerURL As String = ""
        <DataMember()> _
        Public Property ReportServerURL() As String
            Get
                Return _ReportServerURL
            End Get
            Set(ByVal value As String)
                _ReportServerURL = value
            End Set
        End Property

        Private _ReportServerUser As String = ""
        <DataMember()> _
        Public Property ReportServerUser() As String
            Get
                Return _ReportServerUser
            End Get
            Set(ByVal value As String)
                _ReportServerUser = value
            End Set
        End Property

        Private _ReportServerPass As String = ""
        <DataMember()> _
        Public Property ReportServerPass() As String
            Get
                Return _ReportServerPass
            End Get
            Set(ByVal value As String)
                _ReportServerPass = value
            End Set
        End Property

        Private _ReportServerDomain As String = ""
        <DataMember()> _
        Public Property ReportServerDomain() As String
            Get
                Return _ReportServerDomain
            End Get
            Set(ByVal value As String)
                _ReportServerDomain = value
            End Set
        End Property

        'Added By LVV 2/18/16 v-7.0.5.0
        Private _GlobalSMTPUseDefaultCredentials As Boolean = True
        <DataMember()> _
        Public Property GlobalSMTPUseDefaultCredentials() As Boolean
            Get
                Return _GlobalSMTPUseDefaultCredentials
            End Get
            Set(ByVal value As Boolean)
                _GlobalSMTPUseDefaultCredentials = value
            End Set
        End Property

        'Added By LVV 2/18/16 v-7.0.5.0
        Private _GlobalSMTPEnableSSL As Boolean = False
        <DataMember()> _
        Public Property GlobalSMTPEnableSSL() As Boolean
            Get
                Return _GlobalSMTPEnableSSL
            End Get
            Set(ByVal value As Boolean)
                _GlobalSMTPEnableSSL = value
            End Set
        End Property

        'Added By LVV 2/18/16 v-7.0.5.0
        Private _GlobalSMTPTargetName As String = ""
        <DataMember()> _
        Public Property GlobalSMTPTargetName() As String
            Get
                Return _GlobalSMTPTargetName
            End Get
            Set(ByVal value As String)
                _GlobalSMTPTargetName = value
            End Set
        End Property

        'Added By LVV 2/18/16 v-7.0.5.0
        Private _GlobalSMTPPort As Integer = 25
        <DataMember()> _
        Public Property GlobalSMTPPort() As Integer
            Get
                Return _GlobalSMTPPort
            End Get
            Set(ByVal value As Integer)
                _GlobalSMTPPort = value
            End Set
        End Property
         
#End Region

#Region " Public Methods"
        Public Function Clone() As GlobalTaskParameters
            Dim instance As New GlobalTaskParameters
            instance = DirectCast(MemberwiseClone(), GlobalTaskParameters)
            Return instance
        End Function

#End Region
    End Class

End Namespace