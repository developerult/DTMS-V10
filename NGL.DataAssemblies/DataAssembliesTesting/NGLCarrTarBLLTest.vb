Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports System
Imports NGL.FM.CarTar
Imports DAL = NGL.FreightMaster.Data
Imports DTO = NGL.FreightMaster.Data.DataTransferObjects
Imports BLL = NGL.FM.BLL

<TestClass()> Public Class NGLCarrTarBLLTest
    Inherits TestBase

#Region "Test Cloning"
   
#End Region

    <TestMethod> _
    Public Sub TestImportCSVRates()
        'connect to  database before getting any records overwrite the defaul
        testParameters.DBServer = "NGLRDP05D"
        testParameters.Database = "SHIELDSMASPROD"
        Dim blnRunNextTest As Boolean = True
        Dim processName As Guid = Guid.NewGuid()

        Dim bll As New BLL.NGLCarrTarBLL(testParameters)
        bll.importRatesCSV("scnrates.csv", False, "ngl\paul molenda", processName.ToString, "Flat")




    End Sub



End Class
