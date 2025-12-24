Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports System
Imports DAL = NGL.FreightMaster.Data
Imports DTO = NGL.FreightMaster.Data.DataTransferObjects
Imports BLL = NGL.FM.BLL

<TestClass()> Public Class SystemDataTest
    Inherits TestBase

    <TestMethod()>
    Public Sub TestMethod1()
        Dim target As New DAL.NGLEmailData(testParameters)

        Dim alreadySent As Boolean = False
        Dim blnOnlyErrors As Boolean = False
        Dim refIdType As Integer? = 1
        Dim refID As String = "SHIDTest"
        Dim datefilterType As Ngl.FreightMaster.Data.Utilities.NGLDateFilterType = Ngl.FreightMaster.Data.Utilities.NGLDateFilterType.DateAdded
        Dim StartDate As DateTime? = "2016-08-01 00:00:00"
        Dim EndDate As DateTime? = "2016-09-11 00:00:00"

        'target.GetMailsFailedFiltered()
        'GetMailsFiltered705110(ByVal readytosend As Boolean?,
        '                                      ByVal blnOnlyErrors As Boolean,
        '                                      ByVal refIdType As Integer?,
        '                                      ByVal refID As String,
        '                                      Optional ByVal datefilterType As Utilities.NGLDateFilterType = Utilities.NGLDateFilterType.None,
        '                                      Optional ByVal StartDate As DateTime? = Nothing,
        '                                      Optional ByVal EndDate As DateTime? = Nothing,
        '                                      Optional ByVal page As Integer = 1,
        '                                      Optional ByVal pagesize As Integer = 1000,
        '                                      Optional ByVal skip As Integer = 0,
        '                                      Optional ByVal take As Integer = 0) 

        Dim res = target.GetMailsFiltered705110(alreadySent, blnOnlyErrors, refIdType, refID, datefilterType, StartDate, EndDate)

        res.Count()

    End Sub



End Class