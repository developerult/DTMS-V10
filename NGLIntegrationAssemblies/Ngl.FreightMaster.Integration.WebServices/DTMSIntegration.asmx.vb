Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports Ngl.FreightMaster.Integration
Imports System.Xml.Serialization
Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects


' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://dtmsintegration.nextgeneration.com/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class DTMSIntegration
    Inherits System.Web.Services.WebService
    'Note: replace all instances of  ''' <c>ClassLibrary1.TraceExtension()</c> 
    'With <ClassLibrary1.TraceExtension()> to enable SOAP XML Logs.  
    'Should only be run For diagnostics Or In test systems.

    Private mstrLastError As String = ""

    ''' <c>ClassLibrary1.TraceExtension()</c>
    <WebMethod()>
    Public Function LastError() As String
        Return mstrLastError
    End Function
    ''' <c>ClassLibrary1.TraceExtension()</c>
    <WebMethod()>
    Public Function GetERPSetting(ByVal AuthorizationCode As String,
                                  ByVal ERPSettingControl As Integer,
                                  ByRef RetVal As Integer,
                                  ByRef ReturnMessage As String) As DTO.ERPSetting
        Dim sProcedureName As String = "Read Dynamics TMS ERP Setting"
        Dim sErrorLogMsg As String = "Cannot " & sProcedureName & " data using control # " & ERPSettingControl.ToString & ".  "
        Dim oData As New DTO.ERPSetting
        Dim DTMS As New Ngl.FreightMaster.Integration.clsDTMSIntegration
        'set the default value to false
        RetVal = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        Try
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return oData
            Utilities.populateIntegrationObjectParameters(DTMS)
            oData = DTMS.GetERPSetting(ERPSettingControl, Utilities.GetConnectionString())
            If Not oData Is Nothing AndAlso oData.ERPSettingControl <> 0 Then RetVal = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete
            mstrLastError = DTMS.LastError
            ReturnMessage = mstrLastError
        Catch ex As Exception
            mstrLastError = ex.Message
            ReturnMessage = mstrLastError
            Utilities.LogException("DTMSIntegration.GetERPSetting Failure", RetVal, sErrorLogMsg, ex, AuthorizationCode, sProcedureName)
        End Try
        Return oData
    End Function
    ''' <c>ClassLibrary1.TraceExtension()</c>
    <WebMethod()>
    Public Function GetERPSettings(ByVal AuthorizationCode As String,
                                  ByRef RetVal As Integer,
                                  ByRef ReturnMessage As String) As DTO.ERPSetting()
        Dim sProcedureName As String = "Read Dynamics TMS ERP Settings"
        Dim sErrorLogMsg As String = "Cannot " & sProcedureName & " data.  "
        Dim oData As DTO.ERPSetting()
        Dim DTMS As New Ngl.FreightMaster.Integration.clsDTMSIntegration
        'set the default value to false
        RetVal = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        Try
#Disable Warning BC42104 ' Variable 'oData' is used before it has been assigned a value. A null reference exception could result at runtime.
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return oData
#Enable Warning BC42104 ' Variable 'oData' is used before it has been assigned a value. A null reference exception could result at runtime.
            Utilities.populateIntegrationObjectParameters(DTMS)
            oData = DTMS.GetERPSettings(Utilities.GetConnectionString())
            If Not oData Is Nothing AndAlso oData.Count > 0 AndAlso oData(0).ERPSettingControl <> 0 Then RetVal = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete
            mstrLastError = DTMS.LastError
            ReturnMessage = mstrLastError
        Catch ex As Exception
            mstrLastError = ex.Message
            ReturnMessage = mstrLastError
            Utilities.LogException("DTMSIntegration.GetERPSettings Failure", RetVal, sErrorLogMsg, ex, AuthorizationCode, sProcedureName)
        End Try
        Return oData
    End Function
    ''' <c>ClassLibrary1.TraceExtension()</c>
    <WebMethod()>
    Public Function GetERPSettingsByLegalEntity(ByVal AuthorizationCode As String,
                                                ByVal LegalEntity As String,
                                                ByRef RetVal As Integer,
                                                ByRef ReturnMessage As String) As DTO.ERPSetting()
        Dim sProcedureName As String = "Read Dynamics TMS ERP Settings by Legal Entity"
        Dim sErrorLogMsg As String = "Cannot " & sProcedureName & " data using Legal Entity: " & LegalEntity & ".  "
        Dim oData As DTO.ERPSetting()
        Dim DTMS As New Ngl.FreightMaster.Integration.clsDTMSIntegration
        'set the default value to false
        RetVal = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        Try
#Disable Warning BC42104 ' Variable 'oData' is used before it has been assigned a value. A null reference exception could result at runtime.
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return oData
#Enable Warning BC42104 ' Variable 'oData' is used before it has been assigned a value. A null reference exception could result at runtime.
            Utilities.populateIntegrationObjectParameters(DTMS)
            oData = DTMS.GetERPSettingsByLegalEntity(LegalEntity, Utilities.GetConnectionString())
            If Not oData Is Nothing AndAlso oData.Count > 0 AndAlso oData(0).ERPSettingControl <> 0 Then RetVal = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete
            mstrLastError = DTMS.LastError
            ReturnMessage = mstrLastError
        Catch ex As Exception
            mstrLastError = ex.Message
            ReturnMessage = mstrLastError
            Utilities.LogException("DTMSIntegration.GetERPSettingsByLegalEntity Failure", RetVal, sErrorLogMsg, ex, AuthorizationCode, sProcedureName)
        End Try
        Return oData
    End Function
    ''' <c>ClassLibrary1.TraceExtension()</c>
    <WebMethod()>
    Public Function GetERPSettingsByLegalEntityAndERPType(ByVal AuthorizationCode As String,
                                                ByVal LegalEntity As String,
                                                ByVal ERPTypeControl As Integer,
                                                ByRef RetVal As Integer,
                                                ByRef ReturnMessage As String) As DTO.ERPSetting()
        Dim sProcedureName As String = "Read Dynamics TMS ERP Settings by Legal Entity and ERP Type"
        Dim sErrorLogMsg As String = "Cannot " & sProcedureName & " data using Legal Entity: " & LegalEntity & " and ERP Type Control # " & ERPTypeControl.ToString() & ".  "
        Dim oData As DTO.ERPSetting()
        Dim DTMS As New Ngl.FreightMaster.Integration.clsDTMSIntegration
        'set the default value to false
        RetVal = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        Try
#Disable Warning BC42104 ' Variable 'oData' is used before it has been assigned a value. A null reference exception could result at runtime.
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return oData
#Enable Warning BC42104 ' Variable 'oData' is used before it has been assigned a value. A null reference exception could result at runtime.
            Utilities.populateIntegrationObjectParameters(DTMS)
            oData = DTMS.GetERPSettingsByLegalEntityAndERPType(LegalEntity, ERPTypeControl, Utilities.GetConnectionString())
            If Not oData Is Nothing AndAlso oData.Count > 0 AndAlso oData(0).ERPSettingControl <> 0 Then RetVal = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete
            mstrLastError = DTMS.LastError
            ReturnMessage = mstrLastError
        Catch ex As Exception
            mstrLastError = ex.Message
            ReturnMessage = mstrLastError
            Utilities.LogException("DTMSIntegration.GetERPSettingsByLegalEntityAndERPType Failure", RetVal, sErrorLogMsg, ex, AuthorizationCode, sProcedureName)
        End Try
        Return oData
    End Function
    ''' <c>ClassLibrary1.TraceExtension()</c>
    <WebMethod()>
    Public Function GetIntegration(ByVal AuthorizationCode As String,
                                  ByVal IntegrationControl As Integer,
                                  ByRef RetVal As Integer,
                                  ByRef ReturnMessage As String) As DTO.Integration
        Dim sProcedureName As String = "Read Dynamics TMS Integration Configuration"
        Dim sErrorLogMsg As String = "Cannot " & sProcedureName & " data using control # " & IntegrationControl.ToString & ".  "
        Dim oData As New DTO.Integration
        Dim DTMS As New Ngl.FreightMaster.Integration.clsDTMSIntegration
        'set the default value to false
        RetVal = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        Try
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return oData
            Utilities.populateIntegrationObjectParameters(DTMS)
            oData = DTMS.GetIntegration(IntegrationControl, Utilities.GetConnectionString())
            If Not oData Is Nothing AndAlso oData.IntegrationControl <> 0 Then RetVal = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete
            mstrLastError = DTMS.LastError
            ReturnMessage = mstrLastError
        Catch ex As Exception
            mstrLastError = ex.Message
            ReturnMessage = mstrLastError
            Utilities.LogException("DTMSIntegration.GetIntegration Failure", RetVal, sErrorLogMsg, ex, AuthorizationCode, sProcedureName)
        End Try
        Return oData
    End Function
    ''' <c>ClassLibrary1.TraceExtension()</c>
    <WebMethod()>
    Public Function GetIntegrations(ByVal AuthorizationCode As String,
                                  ByRef RetVal As Integer,
                                  ByRef ReturnMessage As String) As DTO.Integration()

        Dim sProcedureName As String = "Read Dynamics TMS Integration Settings"
        Dim sErrorLogMsg As String = "Cannot " & sProcedureName & " data.  "
        Dim oData As DTO.Integration()
        Dim DTMS As New Ngl.FreightMaster.Integration.clsDTMSIntegration
        'set the default value to false
        RetVal = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        Try
#Disable Warning BC42104 ' Variable 'oData' is used before it has been assigned a value. A null reference exception could result at runtime.
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return oData
#Enable Warning BC42104 ' Variable 'oData' is used before it has been assigned a value. A null reference exception could result at runtime.
            Utilities.populateIntegrationObjectParameters(DTMS)
            oData = DTMS.GetIntegrations(Utilities.GetConnectionString())
            If Not oData Is Nothing AndAlso oData.Count > 0 AndAlso oData(0).IntegrationControl <> 0 Then RetVal = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete
            mstrLastError = DTMS.LastError
            ReturnMessage = mstrLastError
        Catch ex As Exception
            mstrLastError = ex.Message
            ReturnMessage = mstrLastError
            Utilities.LogException("DTMSIntegration.GetIntegrations Failure", RetVal, sErrorLogMsg, ex, AuthorizationCode, sProcedureName)
        End Try
        Return oData
    End Function
    ''' <c>ClassLibrary1.TraceExtension()</c>
    <WebMethod()>
    Public Function GetIntegrationsByERPSetting(ByVal AuthorizationCode As String,
                                                ByVal ERPSettingControl As Integer,
                                                ByRef RetVal As Integer,
                                                ByRef ReturnMessage As String) As DTO.Integration()

        Dim sProcedureName As String = "Read Dynamics TMS Integration Configuration by ERP Setting Control"
        Dim sErrorLogMsg As String = "Cannot " & sProcedureName & " data using ERP Setting Control: " & ERPSettingControl.ToString() & ".  "
        Dim oData As DTO.Integration()
        Dim DTMS As New Ngl.FreightMaster.Integration.clsDTMSIntegration
        'set the default value to false
        RetVal = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        Try
#Disable Warning BC42104 ' Variable 'oData' is used before it has been assigned a value. A null reference exception could result at runtime.
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return oData
#Enable Warning BC42104 ' Variable 'oData' is used before it has been assigned a value. A null reference exception could result at runtime.
            Utilities.populateIntegrationObjectParameters(DTMS)
            oData = DTMS.GetIntegrationsByERPSetting(ERPSettingControl, Utilities.GetConnectionString())
            If Not oData Is Nothing AndAlso oData.Count > 0 AndAlso oData(0).ERPSettingControl <> 0 Then RetVal = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete
            mstrLastError = DTMS.LastError
            ReturnMessage = mstrLastError
        Catch ex As Exception
            mstrLastError = ex.Message
            ReturnMessage = mstrLastError
            Utilities.LogException("DTMSIntegration.GetIntegrationsByERPSetting Failure", RetVal, sErrorLogMsg, ex, AuthorizationCode, sProcedureName)
        End Try
        Return oData
    End Function
    ''' <c>ClassLibrary1.TraceExtension()</c>
    <WebMethod()>
    Public Function GetIntegrationsByERPSettingAndIntegrationType(ByVal AuthorizationCode As String,
                                                ByVal ERPSettingControl As Integer,
                                                ByVal IntegrationTypeControl As Integer,
                                                ByRef RetVal As Integer,
                                                ByRef ReturnMessage As String) As DTO.Integration()

        Dim sProcedureName As String = "Read Dynamics TMS Integration Configuration by ERP Setting Control and Integration Type Control"
        Dim sErrorLogMsg As String = "Cannot " & sProcedureName & " data using ERP Setting Control: " & ERPSettingControl.ToString() & ".  "
        Dim oData As DTO.Integration()
        Dim DTMS As New Ngl.FreightMaster.Integration.clsDTMSIntegration
        'set the default value to false
        RetVal = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        Try
#Disable Warning BC42104 ' Variable 'oData' is used before it has been assigned a value. A null reference exception could result at runtime.
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return oData
#Enable Warning BC42104 ' Variable 'oData' is used before it has been assigned a value. A null reference exception could result at runtime.
            Utilities.populateIntegrationObjectParameters(DTMS)
            oData = DTMS.GetIntegrationsByERPSettingAndIntegrationType(ERPSettingControl, IntegrationTypeControl, Utilities.GetConnectionString())
            If Not oData Is Nothing AndAlso oData.Count > 0 AndAlso oData(0).ERPSettingControl <> 0 Then RetVal = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete
            mstrLastError = DTMS.LastError
            ReturnMessage = mstrLastError
        Catch ex As Exception
            mstrLastError = ex.Message
            ReturnMessage = mstrLastError
            Utilities.LogException("DTMSIntegration.GetIntegrationsByERPSettingAndIntegrationType Failure", RetVal, sErrorLogMsg, ex, AuthorizationCode, sProcedureName)
        End Try
        Return oData
    End Function
    ''' <c>ClassLibrary1.TraceExtension()</c>
    <WebMethod()>
    Public Function getvERPIntegrationSettings(ByVal AuthorizationCode As String,
                                                ByVal LegalEntity As String,
                                                ByVal ERPTypeControl As Integer,
                                                ByRef RetVal As Integer,
                                                ByRef ReturnMessage As String) As DTO.vERPIntegrationSetting()

        Dim sProcedureName As String = "Read Dynamics TMS ERP Integration Settings View by Legal Entity and ERP Type"
        Dim sErrorLogMsg As String = "Cannot " & sProcedureName & " data using Legal Entity: " & LegalEntity & " and ERP Type Control # " & ERPTypeControl.ToString() & ".  "
        Dim oData As DTO.vERPIntegrationSetting()
        Dim DTMS As New Ngl.FreightMaster.Integration.clsDTMSIntegration
        'set the default value to false
        RetVal = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        Try
#Disable Warning BC42104 ' Variable 'oData' is used before it has been assigned a value. A null reference exception could result at runtime.
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return oData
#Enable Warning BC42104 ' Variable 'oData' is used before it has been assigned a value. A null reference exception could result at runtime.
            Utilities.populateIntegrationObjectParameters(DTMS)
            oData = DTMS.getvERPIntegrationSettings(LegalEntity, ERPTypeControl, Utilities.GetConnectionString())
            If Not oData Is Nothing AndAlso oData.Count > 0 AndAlso oData(0).IntegrationControl <> 0 Then RetVal = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete
            mstrLastError = DTMS.LastError
            ReturnMessage = mstrLastError
        Catch ex As Exception
            mstrLastError = ex.Message
            ReturnMessage = mstrLastError
            Utilities.LogException("DTMSIntegration.getvERPIntegrationSettings Failure", RetVal, sErrorLogMsg, ex, AuthorizationCode, sProcedureName)
        End Try
        Return oData
    End Function
    ''' <c>ClassLibrary1.TraceExtension()</c>
    <WebMethod()>
    Public Function getvERPIntegrationSettingsByName(ByVal AuthorizationCode As String,
                                                     ByVal LegalEntity As String,
                                                     ByVal ERPTypeName As String,
                                                    ByRef RetVal As Integer,
                                                    ByRef ReturnMessage As String) As DTO.vERPIntegrationSetting()

        Dim sProcedureName As String = "Read Dynamics TMS ERP Integration Settings View by Legal Entity and ERP Type Name"
        Dim sErrorLogMsg As String = "Cannot " & sProcedureName & " data using Legal Entity: " & LegalEntity & " and ERP Type Name: " & ERPTypeName & ".  "
        Dim oData As DTO.vERPIntegrationSetting()
        Dim DTMS As New Ngl.FreightMaster.Integration.clsDTMSIntegration
        'set the default value to false
        RetVal = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        Try
#Disable Warning BC42104 ' Variable 'oData' is used before it has been assigned a value. A null reference exception could result at runtime.
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return oData
#Enable Warning BC42104 ' Variable 'oData' is used before it has been assigned a value. A null reference exception could result at runtime.
            Utilities.populateIntegrationObjectParameters(DTMS)
            oData = DTMS.getvERPIntegrationSettingsByName(LegalEntity, ERPTypeName, Utilities.GetConnectionString())
            If Not oData Is Nothing AndAlso oData.Count > 0 AndAlso oData(0).IntegrationControl <> 0 Then RetVal = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete
            mstrLastError = DTMS.LastError
            ReturnMessage = mstrLastError
        Catch ex As Exception
            mstrLastError = ex.Message
            ReturnMessage = mstrLastError
            Utilities.LogException("DTMSIntegration.getvERPIntegrationSettingsByName Failure", RetVal, sErrorLogMsg, ex, AuthorizationCode, sProcedureName)
        End Try
        Return oData
    End Function
    ''' <c>ClassLibrary1.TraceExtension()</c>
    <WebMethod()>
    Public Function getvERPIntegrationSetting(ByVal AuthorizationCode As String,
                                                ByVal LegalEntity As String,
                                                ByVal ERPTypeControl As Integer,
                                                ByVal IntegrationTypeControl As Integer,
                                                ByRef RetVal As Integer,
                                                ByRef ReturnMessage As String) As DTO.vERPIntegrationSetting
        Dim sProcedureName As String = "Read Dynamics TMS ERP Integration Setting View by Legal Entity and ERP Type and Integration Type Control"
        Dim sErrorLogMsg As String = "Cannot " & sProcedureName & " data using Legal Entity: " & LegalEntity & " and ERP Type Control # " & ERPTypeControl.ToString() & " and Integration Type Control # " & IntegrationTypeControl.ToString() & ".  "
        Dim oData As New DTO.vERPIntegrationSetting
        Dim DTMS As New Ngl.FreightMaster.Integration.clsDTMSIntegration
        'set the default value to false
        RetVal = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        Try
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return oData
            Utilities.populateIntegrationObjectParameters(DTMS)
            oData = DTMS.getvERPIntegrationSetting(LegalEntity, ERPTypeControl, IntegrationTypeControl, Utilities.GetConnectionString())
            If Not oData Is Nothing AndAlso oData.IntegrationControl <> 0 Then RetVal = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete
            mstrLastError = DTMS.LastError
            ReturnMessage = mstrLastError
        Catch ex As Exception
            mstrLastError = ex.Message
            ReturnMessage = mstrLastError
            Utilities.LogException("DTMSIntegration.getvERPIntegrationSetting Failure", RetVal, sErrorLogMsg, ex, AuthorizationCode, sProcedureName)
        End Try
        Return oData
    End Function
    ''' <c>ClassLibrary1.TraceExtension()</c>
    <WebMethod()>
    Public Function getvERPIntegrationSettingByName(ByVal AuthorizationCode As String,
                                                ByVal LegalEntity As String,
                                                ByVal ERPTypeName As String,
                                                ByVal IntegrationTypeName As String,
                                                ByRef RetVal As Integer,
                                                ByRef ReturnMessage As String) As DTO.vERPIntegrationSetting
        Dim sProcedureName As String = "Read Dynamics TMS ERP Integration Setting View by Legal Entity and ERP Type Name and Integration Type Name"
        Dim sErrorLogMsg As String = "Cannot " & sProcedureName & " data using Legal Entity: " & LegalEntity & " and ERP Type Name: " & ERPTypeName & " and Integration Type Name: " & IntegrationTypeName & ".  "
        Dim oData As New DTO.vERPIntegrationSetting
        Dim DTMS As New Ngl.FreightMaster.Integration.clsDTMSIntegration
        'set the default value to false
        RetVal = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        Try
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return oData
            Utilities.populateIntegrationObjectParameters(DTMS)
            oData = DTMS.getvERPIntegrationSettingByName(LegalEntity, ERPTypeName, IntegrationTypeName, Utilities.GetConnectionString())
            If Not oData Is Nothing AndAlso oData.IntegrationControl <> 0 Then RetVal = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete
            mstrLastError = DTMS.LastError
            ReturnMessage = mstrLastError
        Catch ex As Exception
            mstrLastError = ex.Message
            ReturnMessage = mstrLastError
            Utilities.LogException("DTMSIntegration.getvERPIntegrationSettingByName Failure", RetVal, sErrorLogMsg, ex, AuthorizationCode, sProcedureName)
        End Try
        Return oData
    End Function
    ''' <c>ClassLibrary1.TraceExtension()</c>
    <WebMethod()>
    Public Function GettblERPType(ByVal AuthorizationCode As String,
                                  ByVal ERPTypeControl As Integer,
                                  ByRef RetVal As Integer,
                                  ByRef ReturnMessage As String) As DTO.tblERPType
        Dim sProcedureName As String = "Read Dynamics TMS ERP Type"
        Dim sErrorLogMsg As String = "Cannot " & sProcedureName & " using ERP Type Control # " & ERPTypeControl & ".  "
        Dim oData As New DTO.tblERPType
        Dim DTMS As New Ngl.FreightMaster.Integration.clsDTMSIntegration
        'set the default value to false
        RetVal = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        Try
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return oData
            Utilities.populateIntegrationObjectParameters(DTMS)
            oData = DTMS.GettblERPType(ERPTypeControl, Utilities.GetConnectionString())
            If Not oData Is Nothing AndAlso oData.ERPTypeControl <> 0 Then RetVal = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete
            mstrLastError = DTMS.LastError
            ReturnMessage = mstrLastError
        Catch ex As Exception
            mstrLastError = ex.Message
            ReturnMessage = mstrLastError
            Utilities.LogException("DTMSIntegration.GettblERPType Failure", RetVal, sErrorLogMsg, ex, AuthorizationCode, sProcedureName)
        End Try
        Return oData
    End Function

    ''' <c>ClassLibrary1.TraceExtension()</c>
    <WebMethod()>
    Public Function GettblERPTypes(ByVal AuthorizationCode As String,
                                                ByRef RetVal As Integer,
                                                ByRef ReturnMessage As String) As DTO.tblERPType()
        Dim sProcedureName As String = "Read Dynamics TMS ERP Types"
        Dim sErrorLogMsg As String = "Cannot " & sProcedureName & ".  "
        Dim oData As DTO.tblERPType()
        Dim DTMS As New Ngl.FreightMaster.Integration.clsDTMSIntegration
        'set the default value to false
        RetVal = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        Try
#Disable Warning BC42104 ' Variable 'oData' is used before it has been assigned a value. A null reference exception could result at runtime.
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return oData
#Enable Warning BC42104 ' Variable 'oData' is used before it has been assigned a value. A null reference exception could result at runtime.
            Utilities.populateIntegrationObjectParameters(DTMS)
            oData = DTMS.GettblERPTypes(Utilities.GetConnectionString())
            If Not oData Is Nothing AndAlso oData.Count() > 0 AndAlso oData(0).ERPTypeControl <> 0 Then RetVal = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete
            mstrLastError = DTMS.LastError
            ReturnMessage = mstrLastError
        Catch ex As Exception
            mstrLastError = ex.Message
            ReturnMessage = mstrLastError
            Utilities.LogException("DTMSIntegration.GettblERPTypes Failure", RetVal, sErrorLogMsg, ex, AuthorizationCode, sProcedureName)
        End Try
        Return oData
    End Function

    ''' <c>ClassLibrary1.TraceExtension()</c>
    <WebMethod()>
    Public Function GettblIntegrationType(ByVal AuthorizationCode As String,
                                          ByVal IntegrationTypeControl As Integer,
                                          ByRef RetVal As Integer,
                                          ByRef ReturnMessage As String) As DTO.tblIntegrationType
        Dim sProcedureName As String = "Read Dynamics TMS Integration Type"
        Dim sErrorLogMsg As String = "Cannot " & sProcedureName & " using Integration Type Control # " & IntegrationTypeControl.ToString() & ".  "
        Dim oData As New DTO.tblIntegrationType
        Dim DTMS As New Ngl.FreightMaster.Integration.clsDTMSIntegration
        'set the default value to false
        RetVal = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        Try
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return oData
            Utilities.populateIntegrationObjectParameters(DTMS)
            oData = DTMS.GettblIntegrationType(IntegrationTypeControl, Utilities.GetConnectionString())
            If Not oData Is Nothing AndAlso oData.IntegrationTypeControl <> 0 Then RetVal = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete
            mstrLastError = DTMS.LastError
            ReturnMessage = mstrLastError
        Catch ex As Exception
            mstrLastError = ex.Message
            ReturnMessage = mstrLastError
            Utilities.LogException("DTMSIntegration.GettblIntegrationType Failure", RetVal, sErrorLogMsg, ex, AuthorizationCode, sProcedureName)
        End Try
        Return oData
    End Function

    ''' <c>ClassLibrary1.TraceExtension()</c>
    <WebMethod()>
    Public Function GettblIntegrationTypes(ByVal AuthorizationCode As String,
                                                ByRef RetVal As Integer,
                                                ByRef ReturnMessage As String) As DTO.tblIntegrationType()
        Dim sProcedureName As String = "Read Dynamics TMS Integration Types"
        Dim sErrorLogMsg As String = "Cannot " & sProcedureName & ".  "
        Dim oData As DTO.tblIntegrationType()
        Dim DTMS As New Ngl.FreightMaster.Integration.clsDTMSIntegration
        'set the default value to false
        RetVal = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        Try
#Disable Warning BC42104 ' Variable 'oData' is used before it has been assigned a value. A null reference exception could result at runtime.
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return oData
#Enable Warning BC42104 ' Variable 'oData' is used before it has been assigned a value. A null reference exception could result at runtime.
            Utilities.populateIntegrationObjectParameters(DTMS)
            oData = DTMS.GettblIntegrationTypes(Utilities.GetConnectionString())
            If Not oData Is Nothing AndAlso oData.Count() > 0 AndAlso oData(0).IntegrationTypeControl <> 0 Then RetVal = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete
            mstrLastError = DTMS.LastError
            ReturnMessage = mstrLastError
        Catch ex As Exception
            mstrLastError = ex.Message
            ReturnMessage = mstrLastError
            Utilities.LogException("DTMSIntegration.GettblIntegrationTypes Failure", RetVal, sErrorLogMsg, ex, AuthorizationCode, sProcedureName)
        End Try
        Return oData
    End Function

    ''' <summary>
    ''' Web Method used to read the global task parameters from the parameter table in the databaser
    ''' </summary>
    ''' <param name="AuthorizationCode"></param>
    ''' <param name="ReturnMessage"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-7.0.5.102 on 11/14/2016
    '''   Web Method to read global task parameter so direct call to db is not required.
    ''' </remarks>
    ''' <c>ClassLibrary1.TraceExtension()</c>
    <WebMethod()>
    Public Function GetGlobalTaskParameters(ByVal AuthorizationCode As String, ByRef ReturnMessage As String) As DTO.GlobalTaskParameters
        Dim oRet As New DTO.GlobalTaskParameters()
        Dim sSource As String = "DTMSIntegration.GetGlobalTaskParameters"
        Dim sDataType As String = "Global Task Parameters"
        Try
            If Not Utilities.validateAuthCode(AuthorizationCode) Then
                ReturnMessage = "Cannot read configuration settings.  Please check that you are providing a valid Authorization Code."
                Return oRet
            End If
            Dim WCFParameters As New Ngl.FreightMaster.Data.WCFParameters With _
                {
                .ConnectionString = Utilities.GetConnectionString(),
                .DBServer = Utilities.GetServerName(Utilities.GetConnectionString()),
                .Database = Utilities.GetDatabase(Utilities.GetConnectionString()),
                .WCFAuthCode = "NGLSystem",
                .ValidateAccess = False,
                .UserName = ""
                }
            'set up some defaults
            With oRet
                .GlobalAdminEmail = Utilities.GetSetting("AdminEMail")
                .GlobalFromEmail = Utilities.GetSetting("EMailFrom")
                .GlobalGroupEmail = Utilities.GetSetting("GroupEMail")
                .GlobalAutoRetry = Utilities.GetSetting("Retries")
                .GlobalSMTPServer = Utilities.GetSetting("SmtpMailServer")
                .GlobalDebugMode = Utilities.GetDebugMode()
            End With
            Dim oSystem As New Ngl.FreightMaster.Data.NGLSystemDataProvider(WCFParameters)
            oRet = oSystem.GetGlobalTaskParameters()
        Catch ex As Exception
            ReturnMessage = ex.Message
            Utilities.LogResults(sSource, 10000, ex.Message & vbCrLf & ex.StackTrace, AuthorizationCode)
            Utilities.LogException(sSource, Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors, "Cannot process " & sDataType & " data.  ", ex, AuthorizationCode, "Process " & sDataType & " Data Failure")
        Finally
            Try
                mstrLastError = ReturnMessage
            Catch ex As Exception

            End Try
        End Try
        Return oRet
    End Function


End Class