Imports System.IO
Imports System.ServiceModel
Imports System.Reflection

Imports Ngl.FreightMaster.Core
Imports Ngl.Core.Communication
Imports DTran = Ngl.Core.Utility.DataTransformation
Imports Ngl.Core.Communication.Email
Imports Ngl.Core.Communication.General
Imports Ngl.Core
Imports TMS = Ngl.FreightMaster.Integration
Imports DAL = Ngl.FreightMaster.Data
Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects
Imports BLL = Ngl.FM.BLL
Imports LTS = Ngl.FreightMaster.Data.LTS

Public Class clsDTMSIntegration : Inherits clsUpload


    Public Function GetERPSetting(ByVal Control As Integer, ByVal strConnection As String) As DTO.ERPSetting
        Dim oDTMSSetting As DTO.ERPSetting
        Dim oWCFParameters = getSystemWCFParameter(strConnection)
        Dim sProcedureName As String = "Read Dynamics TMS ERP Setting"
        Try
            oDTMSSetting = New DAL.NGLERPSettingData(oWCFParameters).GetERPSettingFiltered(Control)
        Catch ex As FaultException(Of DAL.SqlFaultInfo)
            Dim strMsg As String = Source & " " & sProcedureName & " Failed: " & vbCrLf & ex.Detail.getMsgForLogs(ex.Reason.ToString())
            LogError(Source & " Warning: " & sProcedureName & " Failed", strMsg, Me.AdminEmail)
        Catch ex As Exception
            LogException(Source & " Warning: " & sProcedureName & " Failed", sProcedureName & " Failed", Me.AdminEmail, ex)
        End Try
#Disable Warning BC42104 ' Variable 'oDTMSSetting' is used before it has been assigned a value. A null reference exception could result at runtime.
        Return oDTMSSetting
#Enable Warning BC42104 ' Variable 'oDTMSSetting' is used before it has been assigned a value. A null reference exception could result at runtime.
    End Function

    Public Function GetERPSettings(ByVal strConnection As String) As DTO.ERPSetting()
        Dim oDTMSSettings As DTO.ERPSetting()
        Dim oWCFParameters = getSystemWCFParameter(strConnection)
        Dim sProcedureName As String = "Read Dynamics TMS ERP Settings"
        Try
            oDTMSSettings = New DAL.NGLERPSettingData(oWCFParameters).GetRecordsFiltered()
        Catch ex As FaultException(Of DAL.SqlFaultInfo)
            Dim strMsg As String = Source & " " & sProcedureName & " Failed: " & vbCrLf & ex.Detail.getMsgForLogs(ex.Reason.ToString())
            LogError(Source & " Warning: " & sProcedureName & " Failed", strMsg, Me.AdminEmail)
        Catch ex As Exception
            LogException(Source & " Warning: " & sProcedureName & " Failed", sProcedureName & " Failed", Me.AdminEmail, ex)
        End Try
#Disable Warning BC42104 ' Variable 'oDTMSSettings' is used before it has been assigned a value. A null reference exception could result at runtime.
        Return oDTMSSettings
#Enable Warning BC42104 ' Variable 'oDTMSSettings' is used before it has been assigned a value. A null reference exception could result at runtime.
    End Function

    Public Function GetERPSettingsByLegalEntity(ByVal LegalEntity As String, ByVal strConnection As String) As DTO.ERPSetting()
        Dim oDTMSSettings As DTO.ERPSetting()
        Dim oWCFParameters = getSystemWCFParameter(strConnection)
        Dim sProcedureName As String = "Read Dynamics TMS ERP Settings by Legal Entity"
        With oWCFParameters
            .UserName = "System Download"
            .Database = Me.Database
            .DBServer = Me.DBServer
            .ConnectionString = strConnection
            .WCFAuthCode = "NGLSystem"
            .ValidateAccess = False
        End With
        Try
            oDTMSSettings = New DAL.NGLERPSettingData(oWCFParameters).GetERPSettingsFiltered(LegalEntity)
        Catch ex As FaultException(Of DAL.SqlFaultInfo)
            Dim strMsg As String = Source & " " & sProcedureName & " Failed: " & vbCrLf & ex.Detail.getMsgForLogs(ex.Reason.ToString())
            LogError(Source & " Warning: " & sProcedureName & " Failed", strMsg, Me.AdminEmail)
        Catch ex As Exception
            LogException(Source & " Warning: " & sProcedureName & " Failed", sProcedureName & " Failed", Me.AdminEmail, ex)
        End Try
#Disable Warning BC42104 ' Variable 'oDTMSSettings' is used before it has been assigned a value. A null reference exception could result at runtime.
        Return oDTMSSettings
#Enable Warning BC42104 ' Variable 'oDTMSSettings' is used before it has been assigned a value. A null reference exception could result at runtime.
    End Function

    Public Function GetERPSettingsByLegalEntityAndERPType(ByVal LegalEntity As String, ByVal ERPTypeControl As Integer, ByVal strConnection As String) As DTO.ERPSetting()
        Dim oDTMSSettings As DTO.ERPSetting()
        Dim oWCFParameters = getSystemWCFParameter(strConnection)
        Dim sProcedureName As String = "Read Dynamics TMS ERP Settings by Legal Entity and ERP Type"
        Try
            oDTMSSettings = New DAL.NGLERPSettingData(oWCFParameters).GetERPSettingsFiltered(LegalEntity, ERPTypeControl)
        Catch ex As FaultException(Of DAL.SqlFaultInfo)
            Dim strMsg As String = Source & " " & sProcedureName & " Failed: " & vbCrLf & ex.Detail.getMsgForLogs(ex.Reason.ToString())
            LogError(Source & " Warning: " & sProcedureName & " Failed", strMsg, Me.AdminEmail)
        Catch ex As Exception
            LogException(Source & " Warning: " & sProcedureName & " Failed", sProcedureName & " Failed", Me.AdminEmail, ex)
        End Try
#Disable Warning BC42104 ' Variable 'oDTMSSettings' is used before it has been assigned a value. A null reference exception could result at runtime.
        Return oDTMSSettings
#Enable Warning BC42104 ' Variable 'oDTMSSettings' is used before it has been assigned a value. A null reference exception could result at runtime.
    End Function

    Public Function GetIntegration(ByVal Control As Integer, ByVal strConnection As String) As DTO.Integration
        Dim oDTMSSetting As DTO.Integration
        Dim oWCFParameters = getSystemWCFParameter(strConnection)
        Dim sProcedureName As String = "Read Dynamics TMS Integration Configuration"
        Try
            oDTMSSetting = New DAL.NGLIntegrationData(oWCFParameters).GetIntegrationFiltered(Control)
        Catch ex As FaultException(Of DAL.SqlFaultInfo)
            Dim strMsg As String = Source & " " & sProcedureName & " Failed: " & vbCrLf & ex.Detail.getMsgForLogs(ex.Reason.ToString())
            LogError(Source & " Warning: " & sProcedureName & " Failed", strMsg, Me.AdminEmail)
        Catch ex As Exception
            LogException(Source & " Warning: " & sProcedureName & " Failed", sProcedureName & " Failed", Me.AdminEmail, ex)
        End Try
#Disable Warning BC42104 ' Variable 'oDTMSSetting' is used before it has been assigned a value. A null reference exception could result at runtime.
        Return oDTMSSetting
#Enable Warning BC42104 ' Variable 'oDTMSSetting' is used before it has been assigned a value. A null reference exception could result at runtime.
    End Function

    Public Function GetIntegrations(ByVal strConnection As String) As DTO.Integration()
        Dim oDTMSSettings() As DTO.Integration
        Dim oWCFParameters = getSystemWCFParameter(strConnection)
        Dim sProcedureName As String = "Read Dynamics TMS Integration Configurations"
        Try
            oDTMSSettings = New DAL.NGLIntegrationData(oWCFParameters).GetRecordsFiltered()
        Catch ex As FaultException(Of DAL.SqlFaultInfo)
            Dim strMsg As String = Source & " " & sProcedureName & " Failed: " & vbCrLf & ex.Detail.getMsgForLogs(ex.Reason.ToString())
            LogError(Source & " Warning: " & sProcedureName & " Failed", strMsg, Me.AdminEmail)
        Catch ex As Exception
            LogException(Source & " Warning: " & sProcedureName & " Failed", sProcedureName & " Failed", Me.AdminEmail, ex)
        End Try
#Disable Warning BC42104 ' Variable 'oDTMSSettings' is used before it has been assigned a value. A null reference exception could result at runtime.
        Return oDTMSSettings
#Enable Warning BC42104 ' Variable 'oDTMSSettings' is used before it has been assigned a value. A null reference exception could result at runtime.
    End Function

    Public Function GetIntegrationsByERPSetting(ByVal ERPSettingControl As Integer, ByVal strConnection As String) As DTO.Integration()
        Dim oDTMSSettings() As DTO.Integration
        Dim oWCFParameters = getSystemWCFParameter(strConnection)
        Dim sProcedureName As String = "Read Dynamics TMS Integration Configurations by ERP Setting Control"
        Try
            oDTMSSettings = New DAL.NGLIntegrationData(oWCFParameters).GetIntegrationsFiltered(ERPSettingControl)
        Catch ex As FaultException(Of DAL.SqlFaultInfo)
            Dim strMsg As String = Source & " " & sProcedureName & " Failed: " & vbCrLf & ex.Detail.getMsgForLogs(ex.Reason.ToString())
            LogError(Source & " Warning: " & sProcedureName & " Failed", strMsg, Me.AdminEmail)
        Catch ex As Exception
            LogException(Source & " Warning: " & sProcedureName & " Failed", sProcedureName & " Failed", Me.AdminEmail, ex)
        End Try
#Disable Warning BC42104 ' Variable 'oDTMSSettings' is used before it has been assigned a value. A null reference exception could result at runtime.
        Return oDTMSSettings
#Enable Warning BC42104 ' Variable 'oDTMSSettings' is used before it has been assigned a value. A null reference exception could result at runtime.
    End Function

    Public Function GetIntegrationsByERPSettingAndIntegrationType(ByVal ERPSettingControl As Integer, ByVal IntegrationTypeControl As Integer, ByVal strConnection As String) As DTO.Integration()
        Dim oDTMSSettings() As DTO.Integration
        Dim oWCFParameters = getSystemWCFParameter(strConnection)
        Dim sProcedureName As String = "Read Dynamics TMS Integration Configurations by ERP Setting Control and Integration Type Control"
        Try
            oDTMSSettings = New DAL.NGLIntegrationData(oWCFParameters).GetIntegrationsFiltered(ERPSettingControl, IntegrationTypeControl)
        Catch ex As FaultException(Of DAL.SqlFaultInfo)
            Dim strMsg As String = Source & " " & sProcedureName & " Failed: " & vbCrLf & ex.Detail.getMsgForLogs(ex.Reason.ToString())
            LogError(Source & " Warning: " & sProcedureName & " Failed", strMsg, Me.AdminEmail)
        Catch ex As Exception
            LogException(Source & " Warning: " & sProcedureName & " Failed", sProcedureName & " Failed", Me.AdminEmail, ex)
        End Try
#Disable Warning BC42104 ' Variable 'oDTMSSettings' is used before it has been assigned a value. A null reference exception could result at runtime.
        Return oDTMSSettings
#Enable Warning BC42104 ' Variable 'oDTMSSettings' is used before it has been assigned a value. A null reference exception could result at runtime.
    End Function

    Public Function getvERPIntegrationSettings(ByVal LegalEntity As String, ByVal ERPTypeControl As Integer, ByVal strConnection As String) As DTO.vERPIntegrationSetting()
        Dim oDTMSSettings() As DTO.vERPIntegrationSetting
        Dim oWCFParameters = getSystemWCFParameter(strConnection)
        Try
            oDTMSSettings = New DAL.NGLERPSettingData(oWCFParameters).getvERPIntegrationSettings(LegalEntity, ERPTypeControl)
        Catch ex As FaultException(Of DAL.SqlFaultInfo)
            Dim strMsg As String = Source & " Read Dynamics TMS Integration Settings FailedL: " & vbCrLf & ex.Detail.getMsgForLogs(ex.Reason.ToString())
            LogError(Source & " Warning:  Read Dynamics TMS Integration Settings Failed", strMsg, Me.AdminEmail)
        Catch ex As Exception
            LogException(Source & " Warning:  Read Dynamics TMS Integration Settings Failed", "Read Dynamics TMS Integration Settings Failed", Me.AdminEmail, ex)
        End Try
#Disable Warning BC42104 ' Variable 'oDTMSSettings' is used before it has been assigned a value. A null reference exception could result at runtime.
        Return oDTMSSettings
#Enable Warning BC42104 ' Variable 'oDTMSSettings' is used before it has been assigned a value. A null reference exception could result at runtime.
    End Function

    Public Function getvERPIntegrationSettingsByName(ByVal LegalEntity As String, ByVal ERPTypeName As String, ByVal strConnection As String) As DTO.vERPIntegrationSetting()
        Dim oDTMSSettings() As DTO.vERPIntegrationSetting
        Dim oWCFParameters = getSystemWCFParameter(strConnection)
        Try
            oDTMSSettings = New DAL.NGLERPSettingData(oWCFParameters).getvERPIntegrationSettings(LegalEntity, ERPTypeName)
        Catch ex As FaultException(Of DAL.SqlFaultInfo)
            Dim strMsg As String = Source & " Read Dynamics TMS Integration Settings FailedL: " & vbCrLf & ex.Detail.getMsgForLogs(ex.Reason.ToString())
            LogError(Source & " Warning:  Read Dynamics TMS Integration Settings Failed", strMsg, Me.AdminEmail)
        Catch ex As Exception
            LogException(Source & " Warning:  Read Dynamics TMS Integration Settings Failed", "Read Dynamics TMS Integration Settings Failed", Me.AdminEmail, ex)
        End Try
#Disable Warning BC42104 ' Variable 'oDTMSSettings' is used before it has been assigned a value. A null reference exception could result at runtime.
        Return oDTMSSettings
#Enable Warning BC42104 ' Variable 'oDTMSSettings' is used before it has been assigned a value. A null reference exception could result at runtime.
    End Function

    Public Function getvERPIntegrationSetting(ByVal LegalEntity As String, ByVal ERPTypeControl As Integer, ByVal IntegrationTypeControl As Integer, ByVal strConnection As String) As DTO.vERPIntegrationSetting
        Dim oDTMSSetting As DTO.vERPIntegrationSetting
        Dim oWCFParameters = getSystemWCFParameter(strConnection)
        Try
            oDTMSSetting = New DAL.NGLERPSettingData(oWCFParameters).getvERPIntegrationSetting(LegalEntity, ERPTypeControl, IntegrationTypeControl)
        Catch ex As FaultException(Of DAL.SqlFaultInfo)
            Dim strMsg As String = Source & " Read Dynamics TMS Integration Settings FailedL: " & vbCrLf & ex.Detail.getMsgForLogs(ex.Reason.ToString())
            LogError(Source & " Warning:  Read Dynamics TMS Integration Settings Failed", strMsg, Me.AdminEmail)
        Catch ex As Exception
            LogException(Source & " Warning:  Read Dynamics TMS Integration Settings Failed", "Read Dynamics TMS Integration Settings Failed", Me.AdminEmail, ex)
        End Try
#Disable Warning BC42104 ' Variable 'oDTMSSetting' is used before it has been assigned a value. A null reference exception could result at runtime.
        Return oDTMSSetting
#Enable Warning BC42104 ' Variable 'oDTMSSetting' is used before it has been assigned a value. A null reference exception could result at runtime.
    End Function

    Public Function getvERPIntegrationSettingByName(ByVal LegalEntity As String, ByVal ERPTypeName As String, ByVal IntegrationTypeName As String, ByVal strConnection As String) As DTO.vERPIntegrationSetting
        Dim oDTMSSetting As DTO.vERPIntegrationSetting
        Dim oWCFParameters = getSystemWCFParameter(strConnection)
       
        Try
            oDTMSSetting = New DAL.NGLERPSettingData(oWCFParameters).getvERPIntegrationSetting(LegalEntity, ERPTypeName, IntegrationTypeName)
        Catch ex As FaultException(Of DAL.SqlFaultInfo)
            Dim strMsg As String = Source & " Read Dynamics TMS Integration Settings FailedL: " & vbCrLf & ex.Detail.getMsgForLogs(ex.Reason.ToString())
            LogError(Source & " Warning:  Read Dynamics TMS Integration Settings Failed", strMsg, Me.AdminEmail)
        Catch ex As Exception
            LogException(Source & " Warning:  Read Dynamics TMS Integration Settings Failed", "Read Dynamics TMS Integration Settings Failed", Me.AdminEmail, ex)
        End Try
#Disable Warning BC42104 ' Variable 'oDTMSSetting' is used before it has been assigned a value. A null reference exception could result at runtime.
        Return oDTMSSetting
#Enable Warning BC42104 ' Variable 'oDTMSSetting' is used before it has been assigned a value. A null reference exception could result at runtime.
    End Function

    Public Function GettblERPType(ByVal Control As Integer, ByVal strConnection As String) As DTO.tblERPType
        Dim oDTMSSetting As DTO.tblERPType
        Dim oWCFParameters = getSystemWCFParameter(strConnection)
        Dim sProcedureName As String = "Read Dynamics TMS ERP Type"
        Try
            oDTMSSetting = New DAL.NGLtblERPTypeData(oWCFParameters).GettblERPTypeFiltered(Control)
        Catch ex As FaultException(Of DAL.SqlFaultInfo)
            Dim strMsg As String = Source & " " & sProcedureName & " Failed: " & vbCrLf & ex.Detail.getMsgForLogs(ex.Reason.ToString())
            LogError(Source & " Warning: " & sProcedureName & " Failed", strMsg, Me.AdminEmail)
        Catch ex As Exception
            LogException(Source & " Warning: " & sProcedureName & " Failed", sProcedureName & " Failed", Me.AdminEmail, ex)
        End Try
#Disable Warning BC42104 ' Variable 'oDTMSSetting' is used before it has been assigned a value. A null reference exception could result at runtime.
        Return oDTMSSetting
#Enable Warning BC42104 ' Variable 'oDTMSSetting' is used before it has been assigned a value. A null reference exception could result at runtime.
    End Function

    Public Function GettblERPTypes(ByVal strConnection As String) As DTO.tblERPType()
        Dim oDTMSSettings() As DTO.tblERPType
        Dim oWCFParameters = getSystemWCFParameter(strConnection)
        Dim sProcedureName As String = "Read Dynamics TMS ERP Types"
        Try
            oDTMSSettings = New DAL.NGLtblERPTypeData(oWCFParameters).GetRecordsFiltered()
        Catch ex As FaultException(Of DAL.SqlFaultInfo)
            Dim strMsg As String = Source & " " & sProcedureName & " Failed: " & vbCrLf & ex.Detail.getMsgForLogs(ex.Reason.ToString())
            LogError(Source & " Warning: " & sProcedureName & " Failed", strMsg, Me.AdminEmail)
        Catch ex As Exception
            LogException(Source & " Warning: " & sProcedureName & " Failed", sProcedureName & " Failed", Me.AdminEmail, ex)
        End Try
#Disable Warning BC42104 ' Variable 'oDTMSSettings' is used before it has been assigned a value. A null reference exception could result at runtime.
        Return oDTMSSettings
#Enable Warning BC42104 ' Variable 'oDTMSSettings' is used before it has been assigned a value. A null reference exception could result at runtime.
    End Function

    Public Function GettblIntegrationType(ByVal Control As Integer, ByVal strConnection As String) As DTO.tblIntegrationType
        Dim oDTMSSetting As DTO.tblIntegrationType
        Dim oWCFParameters = getSystemWCFParameter(strConnection)
        Dim sProcedureName As String = "Read Dynamics TMS Integration Type"
        Try
            oDTMSSetting = New DAL.NGLtblIntegrationTypeData(oWCFParameters).GettblIntegrationTypeFiltered(Control)
        Catch ex As FaultException(Of DAL.SqlFaultInfo)
            Dim strMsg As String = Source & " " & sProcedureName & " Failed: " & vbCrLf & ex.Detail.getMsgForLogs(ex.Reason.ToString())
            LogError(Source & " Warning: " & sProcedureName & " Failed", strMsg, Me.AdminEmail)
        Catch ex As Exception
            LogException(Source & " Warning: " & sProcedureName & " Failed", sProcedureName & " Failed", Me.AdminEmail, ex)
        End Try
#Disable Warning BC42104 ' Variable 'oDTMSSetting' is used before it has been assigned a value. A null reference exception could result at runtime.
        Return oDTMSSetting
#Enable Warning BC42104 ' Variable 'oDTMSSetting' is used before it has been assigned a value. A null reference exception could result at runtime.
    End Function

    Public Function GettblIntegrationTypes(ByVal strConnection As String) As DTO.tblIntegrationType()
        Dim oDTMSSettings() As DTO.tblIntegrationType
        Dim oWCFParameters = getSystemWCFParameter(strConnection)
        Dim sProcedureName As String = "Read Dynamics TMS Integration Types"
        Try
            oDTMSSettings = New DAL.NGLtblIntegrationTypeData(oWCFParameters).GetRecordsFiltered()
        Catch ex As FaultException(Of DAL.SqlFaultInfo)
            Dim strMsg As String = Source & " " & sProcedureName & " Failed: " & vbCrLf & ex.Detail.getMsgForLogs(ex.Reason.ToString())
            LogError(Source & " Warning: " & sProcedureName & " Failed", strMsg, Me.AdminEmail)
        Catch ex As Exception
            LogException(Source & " Warning: " & sProcedureName & " Failed", sProcedureName & " Failed", Me.AdminEmail, ex)
        End Try
#Disable Warning BC42104 ' Variable 'oDTMSSettings' is used before it has been assigned a value. A null reference exception could result at runtime.
        Return oDTMSSettings
#Enable Warning BC42104 ' Variable 'oDTMSSettings' is used before it has been assigned a value. A null reference exception could result at runtime.
    End Function

    Private Function getSystemWCFParameter(ByVal strConnection As String) As DAL.WCFParameters
        Dim oWCFParameters As New DAL.WCFParameters
        With oWCFParameters
            .Database = Me.Database
            .DBServer = Me.DBServer
            .ConnectionString = strConnection
            .WCFAuthCode = "NGLSystem"
            .ValidateAccess = False
            .UserName = "System Download"
        End With
        Return oWCFParameters
    End Function

End Class
