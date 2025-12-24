Imports System
Imports System.IO
Imports System.Linq
Imports System.ServiceModel
Imports NGL.FMWCFProxy.NGLBookData
Imports NGL.IntegrationServices.NGLBookWebService

Public Class BookObjectIntegration

#Region "Properties and Enums"


    Public Enum WebServiceReturnValues
        nglDataIntegrationComplete
        nglDataConnectionFailure
        nglDataValidationFailure
        nglDataIntegrationFailure
        nglDataIntegrationHadErrors
    End Enum
     
    Public LastError As String = ""

    Private wsURL As String
    Private wsAuth As String
    
#End Region

#Region "Public Methods"

    Public Function AddOrders(ByVal oHeaders() As clsBookHeaderObject, ByVal oDetails() As clsBookDetailObject) As Integer
        Dim intResult As Integer = 0
        Dim strLastError As String = ""
        Dim oBookObjectService As New BookObject
        oBookObjectService.Url = Me.wsURL
        intResult = oBookObjectService.ProcessDataEx(Me.wsAuth, oHeaders, oDetails, strLastError)

        Select Case intResult
            Case WebServiceReturnValues.nglDataConnectionFailure
                LastError = "Database Connection Failure Error: " & strLastError
            Case WebServiceReturnValues.nglDataIntegrationFailure
                LastError = "Data Integration Failure Error: " & strLastError
            Case WebServiceReturnValues.nglDataIntegrationHadErrors
                LastError = "Some Errors: " & strLastError
            Case WebServiceReturnValues.nglDataValidationFailure
                LastError = "Data Validation Failure Error: " & strLastError
            Case Else
                LastError = "Invalid Return Value."
        End Select
        oBookObjectService = Nothing
        Return intResult

    End Function
      
    Public Function ConvertToWCFProperties(ByRef WCFProperty As Object, ByVal TOwcfProp As Object)
        WCFProperty.Database = TOwcfProp.Database
        WCFProperty.DBServer = TOwcfProp.DBServer
        WCFProperty.UserName = TOwcfProp.UserName
        WCFProperty.UserRemotePassword = TOwcfProp.UserRemotePassword
        WCFProperty.WCFAuthCode = TOwcfProp.WCFAuthCode
        WCFProperty.CompControl = TOwcfProp.CompControl
        WCFProperty.FormControl = TOwcfProp.FormControl
        WCFProperty.FormName = TOwcfProp.FormName
        WCFProperty.ProcedureControl = TOwcfProp.ProcedureControl
        WCFProperty.ProcedureName = TOwcfProp.ProcedureName
        WCFProperty.ReportControl = TOwcfProp.ReportControl
        WCFProperty.ReportName = TOwcfProp.ReportName
        WCFProperty.ValidateAccess = TOwcfProp.ValidateAccess
        Return WCFProperty
    End Function
     
    Public Sub New(ByVal wsAuthCode As String, ByVal WsURL As String)
        Me.wsAuth = wsAuthCode
        Me.wsURL = WsURL
    End Sub

#End Region

End Class
