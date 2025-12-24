Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports System
Imports Ngl.FM.CarTar
Imports DAL = Ngl.FreightMaster.Data
Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects
Imports BLL = Ngl.FM.BLL
Imports System.Web.Script.Serialization

<TestClass()> Public Class OptimizerBLLTests
    Inherits TestBase

    <TestMethod()> Public Sub RunOptimizer365Test()
        'Set up the test connection
        testParameters.DBServer = "NGLRDP07D"
        testParameters.Database = "NGLMASPROD"
        testParameters.ConnectionString = "Server=NGLRDP07D;User ID=nglweb;Password=5529;Database=NGLMASPROD"
        testParameters.UserName = "ngl\lauren van vleet"
        testParameters.UserControl = 8
        Dim oUPS As DAL.NGLUserPageSettingData = New DAL.NGLUserPageSettingData(testParameters)
        Dim target As BLL.OptimizerBLL = New BLL.OptimizerBLL(testParameters)

        Dim f = New DAL.Models.AllFilters()
        Dim ups = oUPS.GetPageSettingsForCurrentUser(62, "AllRecordsFilter")
        If (ups?.Count() > 0) Then f = New JavaScriptSerializer().Deserialize(Of DAL.Models.AllFilters)(ups(0).UserPSMetaData)
        f.page = 1
        f.take = 1000 '//Return all On one page, don't want paging here
        target.RunOptimizer365(f)
    End Sub

End Class