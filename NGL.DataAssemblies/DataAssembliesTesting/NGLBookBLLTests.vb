Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports System
Imports NGL.FM.CarTar
Imports DAL = NGL.FreightMaster.Data
Imports DTO = NGL.FreightMaster.Data.DataTransferObjects
Imports BLL = NGL.FM.BLL

<TestClass()> Public Class NGLBookBLLTests
    Inherits TestBase

    <TestMethod()> Public Sub TestMethod1()

        'connect to  database before getting book records overwrite the defaul
        testParameters.DBServer = "NGLRDP06D"
        testParameters.Database = "NGLMASDEV7051"
        testParameters.UserName = "NGL\Lauren Van Vleet"

        Dim target As BLL.NGLBookBLL = New BLL.NGLBookBLL(testParameters)
        Dim oBookRev As New DAL.NGLBookRevenueData(testParameters)

        '902587, 902588, 902589, 902590, 902591, 902592,
        '902594, 902595, 902596, 902597
        Dim BookControl As Integer = 902594
        Dim NewTranCode As String = "N"
        Dim OldTranCode As String = "PC"
        'target.ProcessNewTransCode(BookControl, NewTranCode, OldTranCode)

        Dim strBookTrackComment = " Trans Code changed From " & OldTranCode & " to " & NewTranCode


        Dim oBookRevs = oBookRev.GetBookRevenuesWDetailsFiltered(BookControl, True)

        Dim oSelectedBooking As DTO.BookRevenue = oBookRevs.Where(Function(x) x.BookControl = BookControl).FirstOrDefault()
        Dim UserName As String = testParameters.UserName
        Dim results As New DTO.WCFResults
        results = Nothing
        If results Is Nothing Then results = New DTO.WCFResults With {.Key = BookControl, .Success = True}
        results.setAction(DTO.WCFResults.ActionEnum.DoNothing)

        target.AcceptOrRejectLoad(oBookRevs, oSelectedBooking, oSelectedBooking.BookCarrierControl, oSelectedBooking.BookCarrierContControl, BLL.NGLBookBLL.AcceptRejectEnum.Expired, True, strBookTrackComment & " Load Rejected", "0", "", "", BLL.NGLBookBLL.AcceptRejectModeEnum.DAT, UserName, results)


    End Sub

End Class