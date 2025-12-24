Imports System
Imports System.IO
Imports System.Linq
Imports System.ServiceModel
Imports NGL.FMWCFProxy.NGLLaneData
Imports NGL.IntegrationServices.NglLaneWebService

Public Class LaneObjectIntegration

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
    Private wsAuthCode As String
    
   

#End Region

#Region "Public Methods"

    Public Function AddLaness(ByVal oLaness() As clsLaneObject) As Integer
        Dim intResult As Integer = 0
        Dim strLastError As String = ""
        Dim oLaneObjectService As New LaneObject
        oLaneObjectService.Url = Me.wsURL
        intResult = oLaneObjectService.ProcessDataEx(Me.wsAuthCode, oLaness, strLastError)

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
        oLaneObjectService = Nothing
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

  

    Public Sub New(ByVal auth As String, ByVal webServiceURL As String)
        Me.wsAuthCode = auth
        Me.wsURL = webServiceURL

    End Sub

#End Region

End Class
