Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports DAL = Ngl.FreightMaster.Data
Imports Ngl.FreightMaster.Integration

<TestClass()> Public Class PayablesObjectWSTest
    Inherits TestBase

    <TestMethod()>
    Public Sub PayablesProcessDataExTest()
        Dim sSource As String = "PayablesProcessDataExTest"
        testParameters.DBServer = "NGLRDP06D\DTMS365"
        testParameters.Database = "NGLMASPROD"
        Dim intResult As Integer = 0
        Dim strLastError As String = ""
        Dim payable As New Ngl.FreightMaster.Integration.clsPayablesObject()
        '31	VJI757972	tendered2
        With payable
            .BookCarrOrderNumber = "EDI1"
            .BookFinAPPayAmt = 9999.99
            .BookFinAPActWgt = 0.0
            .BookFinAPCheck = 0.0
            .BookFinAPPayDate = "2019-02-10"
            .BookFinAPBillNumber = "10254567"
            .BookFinAPBillInvDate = "2018-04-10"
            .CompNumber = 31
            .BookOrderSequence = 0
        End With
        Dim Payables() As Ngl.FreightMaster.Integration.clsPayablesObject = {payable}
        Dim oConfig As New Ngl.FreightMaster.Core.UserConfiguration()
        With oConfig
            .AdminEmail = "rramsey@nextgeneration.com"
            .AutoRetry = 0
            .Database = "NGLMASPROD"
            .DBServer = "NGLRDP06D\DTMS365"
            .Debug = True
            .FromEmail = "system@nextgeneration.com"
            .GroupEmail = "rramsey@nextgeneration.com"
            .LogFile = "C:\Data\TMSLogs\" & sSource & ".log"
            .SMTPServer = "nglmail.ngl.local"
            .UserName = "ngl\rramsey"
            .WSAuthCode = "WSDEV"
            .WCFAuthCode = "WCFDEV"
            .WCFURL = "http://nglwcfdev705.nextgeneration.com"
            .WCFTCPURL = "net.tcp://nglwcfdev705.nextgeneration.com:705"
        End With
        Dim sConnection = "Data Source=nglrdp06d\dtms365;Initial Catalog=NGLMASPROD;User ID=nglweb;Password=5529"
        Dim oPayables As New Ngl.FreightMaster.Integration.clsPayables(oConfig)
        intResult = oPayables.ProcessObjectData(Payables, sConnection)
        strLastError = oPayables.LastError
        If intResult <> 0 Then
            Assert.Fail()
        End If

    End Sub


    <TestMethod()>
    Public Sub PayablesProcessObjectData70DllTest()
        Dim sSource As String = "PayablesProcessObjectData70DllTest"
        testParameters.DBServer = "DESKTOP-0R0EJUB"
        testParameters.Database = "NGLMASPROD"
        Dim intResult As Integer = 0
        Dim strLastError As String = ""
        Dim payable As New NGL.FreightMaster.Integration.clsPayablesObject70()
        Dim lPayables As New List(Of NGL.FreightMaster.Integration.clsPayablesObject70)

        With payable
            .BookCarrOrderNumber = "EDI1"
            .BookFinAPPayAmt = 9999.99
            .BookFinAPActWgt = 0.0
            .BookFinAPCheck = 0.0
            .BookFinAPPayDate = "2022-02-10"
            .BookFinAPBillNumber = "10254567"
            .BookFinAPBillInvDate = "2021-04-10"
            .CompNumber = 31
            .BookOrderSequence = 0
        End With
        lPayables.Add(payable)

        Dim oWCFParameters As New DAL.WCFParameters
        With oWCFParameters
            .UserName = "System Download"
            .Database = testParameters.Database
            .DBServer = testParameters.DBServer
            .ConnectionString = String.Format("Server={0};Database={1};Integrated Security=SSPI", testParameters.DBServer, testParameters.Database)
            .WCFAuthCode = "NGLSystem"
            .ValidateAccess = False
        End With
        Dim ReturnMessage As String
        Dim result As Integer = NGL.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        ReturnMessage = ""
        Try
            Dim sConnectionString = String.Format("Server={0};Database={1};Integrated Security=SSPI", testParameters.DBServer, testParameters.Database)

            Dim oConfig As New NGL.FreightMaster.Core.UserConfiguration()
            With oConfig
                .AdminEmail = "rramsey@nextgeneration.com"
                .AutoRetry = 0
                .Database = oWCFParameters.Database
                .DBServer = oWCFParameters.DBServer
                .Debug = True
                .FromEmail = "system@nextgeneration.com"
                .GroupEmail = "rramsey@nextgeneration.com"
                .LogFile = "C:\Data\TMSLogs\" & sSource & ".log"
                .SMTPServer = "nglmail.ngl.local"
                .UserName = "ngl\rramsey"
                .WSAuthCode = "WSPROD"
                .WCFAuthCode = "WCFPROD"
                .WCFURL = "http://localhost:56533"
                .WCFTCPURL = "net.tcp://localhost:56533:808"
            End With
            Dim oPayData As New NGL.FreightMaster.Integration.clsPayables(oConfig)

            result = oPayData.ProcessObjectData70(lPayables, sConnectionString)
            ReturnMessage = oPayData.LastError
            Select Case result
                Case NGL.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                    Assert.Fail("Data Connection Failure! could not import Payable information:  " & ReturnMessage)
                Case NGL.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
                    Assert.Fail("Integration Failure! could not import Payable information:  " & ReturnMessage)
                Case NGL.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
                    Assert.Fail("Integration Had Errors! Could not import some Payable information:  " & ReturnMessage)
                Case NGL.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataValidationFailure
                    Assert.Fail("Warning!  Data Validation Failure! could not import Payable information:  " & ReturnMessage)
                Case Else
                    ReturnMessage = "Success!"
            End Select


        Catch ex As ApplicationException
            Assert.Fail("Application Exception For " & sSource & ": {0} ", ex.Message)
        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For " & sSource & ": {0} ", ex.Message)
        Finally

        End Try


    End Sub

End Class