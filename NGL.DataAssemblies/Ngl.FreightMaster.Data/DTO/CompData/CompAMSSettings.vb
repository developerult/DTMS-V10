Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class CompAMSSettings
        Inherits DTOBaseClass

        Public Enum ColorCodeType
            ApptStatus
            ApptType
        End Enum

        'DEPRECIATED By LVV on 6/28/18 for v-8.3 TMS365 Scheduler
        'We now reference the lookup table tblAMSApptStatus
        ''Public Enum ApptStatusColorCodeKey
        ''    Unassigned
        ''    OutOfService
        ''    Assigned
        ''    CheckedIn
        ''    StartLoad_Unload
        ''    FinishLoad_Unload
        ''    CheckedOut
        ''    Other
        ''End Enum

        'DEPRECIATED By LVV on 6/28/18 for v-8.3 TMS365 Scheduler
        'We now reference the lookup table tblAMSApptType
        ''Public Enum ApptTypeColorCodeKey
        ''    Normal
        ''    SingleLoadNoCNSOutbound
        ''    ConsolidatedIntegrityOffOutbound
        ''    ConsolidatedLoadOutbound
        ''    SingleLoadNoCNSInbound
        ''    ConsolidatedIntegrityOffInbound
        ''    ConsolidatedLoadInbound
        ''    Other
        ''End Enum


#Region " Data Members"
        Private _CompAMSSettingsCompControl As Integer = 0
        <DataMember()> _
        Public Property CompAMSSettingsCompControl As Integer
            Get
                Return _CompAMSSettingsCompControl
            End Get
            Set(ByVal value As Integer)
                _CompAMSSettingsCompControl = value
            End Set
        End Property

        Private _CompAMSColorCodeSettings As List(Of CompAMSColorCodeSetting)
        <DataMember()> _
        Public Property CompAMSColorCodeSettings() As List(Of CompAMSColorCodeSetting)
            Get
                Return _CompAMSColorCodeSettings
            End Get
            Set(ByVal value As List(Of CompAMSColorCodeSetting))
                _CompAMSColorCodeSettings = value
            End Set
        End Property


        Private _CompAMSApptTrackingSettings As List(Of CompAMSApptTrackingSetting)
        <DataMember()> _
        Public Property CompAMSApptTrackingSettings() As List(Of CompAMSApptTrackingSetting)
            Get
                Return _CompAMSApptTrackingSettings
            End Get
            Set(ByVal value As List(Of CompAMSApptTrackingSetting))
                _CompAMSApptTrackingSettings = value
            End Set
        End Property

        Private _CompAMSUserFieldSettings As List(Of CompAMSUserFieldSetting)
        <DataMember()> _
        Public Property CompAMSUserFieldSettings() As List(Of CompAMSUserFieldSetting)
            Get
                Return _CompAMSUserFieldSettings
            End Get
            Set(ByVal value As List(Of CompAMSUserFieldSetting))
                _CompAMSUserFieldSettings = value
            End Set
        End Property

       
#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CompAMSSettings
            instance = DirectCast(MemberwiseClone(), CompAMSSettings)
            instance.CompAMSUserFieldSettings = Nothing
            For Each item In CompAMSUserFieldSettings
                instance.CompAMSUserFieldSettings.Add(DirectCast(item.Clone, CompAMSUserFieldSetting))
            Next
            instance.CompAMSApptTrackingSettings = Nothing
            For Each item In CompAMSApptTrackingSettings
                instance.CompAMSApptTrackingSettings.Add(DirectCast(item.Clone, CompAMSApptTrackingSetting))
            Next
            instance.CompAMSColorCodeSettings = Nothing
            For Each item In CompAMSColorCodeSettings
                instance.CompAMSColorCodeSettings.Add(DirectCast(item.Clone, CompAMSColorCodeSetting))
            Next
            Return instance
        End Function

#End Region

    End Class
End Namespace