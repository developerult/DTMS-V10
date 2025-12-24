Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports System
Imports Ngl.FM.CarTar
Imports DAL = Ngl.FreightMaster.Data
Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects
Imports BLL = Ngl.FM.BLL

<TestClass()> Public Class NGLCompDataProviderTests

    Inherits TestBase


    <TestMethod()> _
    Public Sub GetCompCreditsSecurityTest()
        'connect to  database before getting book records overwrite the defaul
        testParameters.DBServer = "NGLRDP06D"
        testParameters.Database = "NGLMAS2013DEV"
        testParameters.UserName = "NGL\rramsey"

        Dim strTestName As String = "GetCompCreditsSecurityTest"

        Try
            'GetCompCredits(Optional ByVal page As Integer = 1,Optional ByVal pagesize As Integer = 1000) As DTO.CompCredit()
            Dim page As Integer = 1
            Dim pagesize As Integer = 1000

            Dim target As New DAL.NGLCompCreditData(testParameters)
            Dim results As DTO.CompCredit() = target.GetCompCredits(page, pagesize)
            If results Is Nothing OrElse results.Count() < 1 Then
                Assert.Fail(strTestName & " failed,  no records.")
            End If
            System.Diagnostics.Debug.WriteLine(strTestName & " found " & results.Count() & " records.")
        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("Unexpected Error For " & strTestName & ": {0} ", ex.Message)
        Finally
            'place clean up code here
        End Try

    End Sub
End Class
