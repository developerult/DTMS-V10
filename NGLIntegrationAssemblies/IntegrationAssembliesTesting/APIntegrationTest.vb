Imports System
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Tar = Ngl.FM.CarTar
Imports DAL = Ngl.FreightMaster.Data
Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects
Imports BLL = Ngl.FM.BLL
Imports LTS = Ngl.FreightMaster.Data.LTS
Imports Ngl.FreightMaster.Integration
Imports Ngl.FreightMaster.Integration.Configuration

<TestClass()> Public Class APIntegrationTest
    Inherits TestBase




    <TestMethod()>
    Public Sub TestreadAPDataData80()
        Dim oConfig As New Ngl.FreightMaster.Core.UserConfiguration()
        With oConfig
            .AdminEmail = "laurenvanvleet@nextgeneration.com"
            .AutoRetry = 0
            .Database = "NGLMASTBS"
            .DBServer = "DESKTOP-0R0EJUB"
            .Debug = True
            .FromEmail = "rramsey@nextgeneration.com"
            .GroupEmail = "rramsey@nextgeneration.com"
            .KeepLogDays = 1
            .LogFile = "C:\Data\TMSLog.txt"
            .SaveOldLog = False
            .SMTPServer = "nglMail.nextgeneration.com"
            .Source = "TestreadAPDataData80"
            .WCFAuthCode = "NGLSystem"
            .WSAuthCode = "WSDEV"
            .ConnectionString = "Server=DESKTOP-0R0EJUB;User ID=nglweb;Password=5529;Database=NGLMASTBS"
        End With

        Dim strResults As String = "Fail"
        Dim oEDIInbound As New Ngl.FreightMaster.Integration.clsEDIInput(oConfig)

        Dim ap As New clsAPExportData80
        Dim apExport As New Ngl.FreightMaster.Integration.clsAPExport(oConfig)
        Dim Headers() As Ngl.FreightMaster.Integration.clsAPExportObject80
        Dim Details() As Ngl.FreightMaster.Integration.clsAPExportDetailObject80
        Dim Fees() As Ngl.FreightMaster.Integration.clsAPExportFeeObject80

        'set the default value to false
        Dim RetVal = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        Dim ReturnMessage = ""
        Dim sSource As String = "DTMSERPIntegration.GetAPData80"
        Dim sDataType As String = "AP"
        Dim strCriteria As String = ""
        Dim MaxRetry = 10
        Dim RetryMinutes = 1
        Dim MaxRowsReturned = 100
        Dim AutoConfirmation = False
        Dim CompLegalEntity = "TRACHTE-LIVE"


        Try
            With apExport
                .MaxRowsReturned = MaxRowsReturned
                .AutoConfirmation = AutoConfirmation
            End With
            RetVal = apExport.readObjectData80(Headers, oConfig.ConnectionString, MaxRetry, RetryMinutes, CompLegalEntity, Fees, Details)
            If Not String.IsNullOrWhiteSpace(apExport.LastError) Then
                Assert.Fail(apExport.LastError)
            End If

            ap.Headers = Headers
            ap.Details = Details
            ap.Fees = Fees
        Catch ex As Exception
            Assert.Fail(ex.Message)
        End Try


    End Sub


End Class


<Serializable()>
Public Class clsAPExportData80
    Public Headers() As Ngl.FreightMaster.Integration.clsAPExportObject80
    Public Details() As Ngl.FreightMaster.Integration.clsAPExportDetailObject80
    Public Fees() As Ngl.FreightMaster.Integration.clsAPExportFeeObject80
End Class
