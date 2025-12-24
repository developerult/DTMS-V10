Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports System
Imports NGL.FM.CarTar
Imports DAL = NGL.FreightMaster.Data
Imports DTO = NGL.FreightMaster.Data.DataTransferObjects
Imports BLL = NGL.FM.BLL

<TestClass()> Public Class NGLLookupDataTests
    Inherits TestBase

    <TestMethod> _
    Public Sub GetLaneListTest()
        Try
            Dim target As New DAL.NGLLookupDataProvider(testParameters)
            Dim dtModDate As Date = Date.Now()
            Dim lLanes = target.GetViewLookupList("Lane", 1, Nothing, Nothing).ToList()
            If lLanes Is Nothing OrElse lLanes.Count() < 1 Then
                Assert.Fail("GetLaneListTest Failed no Lanes")
            End If
            'update one of the lanes
            Dim intLaneControl As Integer = lLanes(0).Control
            Dim dtUserChangedData As Date = Date.Now()
            target.executeSQL("Update Lane set LaneModDate = '" & dtUserChangedData.ToString() & "' Where Lane.LaneControl = " & intLaneControl)

            Dim lModLanes = target.GetViewLookupList("Lane", 1, Nothing, dtModDate).ToList()
            If lModLanes Is Nothing OrElse lModLanes.Count() < 1 Then
                Assert.Fail("GetLaneListTest Failed no Modified Lanes")
            End If
            For Each l In lModLanes
                If lLanes.Any(Function(x) x.Control = l.Control) Then
                    System.Diagnostics.Debug.WriteLine("Modified Lane Already Exists: " & l.Name)
                Else
                    System.Diagnostics.Debug.WriteLine("Modified Lane Added to Lookup List: " & l.Name)
                    'add mod to lanes
                    lLanes.Add(l)
                End If
            Next



        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("GetLaneListTest Unexpected Error: " & ex.Message)
        Finally

        End Try
    End Sub



    <TestMethod> _
    Public Sub GetModBookMaintListTest()
        Try
            Dim target As New DAL.NGLLookupDataProvider(testParameters)
            Dim dtModDate As Date = Date.Now()
            Dim intDaysBack As Integer = -365
            Dim lvLookup As List(Of DTO.vBookMaintLookup) = target.GetBookMaintFilters(0, Date.Now.ToShortDateString, intDaysBack).ToList()
            If lvLookup Is Nothing OrElse lvLookup.Count() < 1 Then
                Assert.Fail("GetModBookMaintListTest Failed no Bookings")
            End If
            'update one of the orders
            Dim intControl As Integer = lvLookup(0).BookControl
            Dim dtUserChangedData As Date = Date.Now()
            target.executeSQL("Update Book set BookModDate = '" & dtUserChangedData.ToString() & "' Where Book.BookControl = " & intControl)

            Dim lModRecords = target.GetBookMaintFilters(0, Date.Now.ToShortDateString, intDaysBack, dtModDate).ToList()
            If lModRecords Is Nothing OrElse lModRecords.Count() < 1 Then
                Assert.Fail("GetModBookMaintListTest Failed no Modified Bookings")
            End If
            For Each l In lModRecords
                If lvLookup.Any(Function(x) x.BookControl = l.BookControl) Then
                    System.Diagnostics.Debug.WriteLine("Modified Booking Already Exists, Pro#: " & l.BookProNumber)
                Else
                    System.Diagnostics.Debug.WriteLine("Modified Booking Added to Lookup List, Pro#: " & l.BookProNumber)
                    'add mod to lanes
                    lvLookup.Add(l)
                End If
            Next



        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("GetModBookMaintListTest Unexpected Error: " & ex.Message)
        Finally

        End Try
    End Sub


    <TestMethod> _
    Public Sub GetModBookMaintListByOrderNumberTest()
        Try
            Dim target As New DAL.NGLLookupDataProvider(testParameters)
            Dim dtModDate As Date = Date.Now()
            Dim intDaysBack As Integer = -365
            Dim lvLookup As List(Of DTO.vBookMaintLookup) = target.GetBookMaintFilters(0, Date.Now.ToShortDateString, intDaysBack).ToList()
            If lvLookup Is Nothing OrElse lvLookup.Count() < 1 Then
                Assert.Fail("GetModBookMaintListByOrderNumberTest Failed no Bookings")
            End If
            'get one of the order using the order number
            Dim lOrderLookup = target.GetBookMaintItemByOrderNumber(lvLookup(0).BookCarrOrderNumber, lvLookup(0).BookOrderSequence, lvLookup(0).CompNumber)
            If lOrderLookup Is Nothing OrElse lOrderLookup.BookControl = 0 Then
                Assert.Fail("GetModBookMaintListByOrderNumberTest Failed invalid Order number")
            End If
            Dim intControl As Integer = lOrderLookup.BookControl
            'clear the object
            lOrderLookup = Nothing
            'Check if the order has changed (should return nothing or an empty record)
            lOrderLookup = target.GetBookMaintItemByOrderNumber(lvLookup(0).BookCarrOrderNumber, lvLookup(0).BookOrderSequence, lvLookup(0).CompNumber, dtModDate)
            If Not lOrderLookup Is Nothing AndAlso lOrderLookup.BookControl <> 0 Then
                Assert.Fail("GetModBookMaintListByOrderNumberTest Failed order number returned but was not modified")
            End If
            'clear the object
            lOrderLookup = Nothing
            Dim dtUserChangedData As Date = Date.Now()
            target.executeSQL("Update Book set BookModDate = '" & dtUserChangedData.ToString() & "' Where Book.BookControl = " & intControl)
            'this time the order was modified so we should get a record back
            lOrderLookup = target.GetBookMaintItemByOrderNumber(lvLookup(0).BookCarrOrderNumber, lvLookup(0).BookOrderSequence, lvLookup(0).CompNumber, dtModDate)
            If lOrderLookup Is Nothing OrElse lOrderLookup.BookControl = 0 Then
                Assert.Fail("GetModBookMaintListByOrderNumberTest Failed modified order number was not returned")
            End If
            
            If lvLookup.Any(Function(x) x.BookControl = lOrderLookup.BookControl) Then
                System.Diagnostics.Debug.WriteLine("Modified Booking Already Exists, Pro#: " & lOrderLookup.BookProNumber)
            Else
                System.Diagnostics.Debug.WriteLine("Modified Booking Added to Lookup List, Pro#: " & lOrderLookup.BookProNumber)
                'add mod to lanes
                lvLookup.Add(lOrderLookup)
            End If
        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("GetModBookMaintListByOrderNumberTest Unexpected Error: " & ex.Message)
        Finally

        End Try
    End Sub



    <TestMethod> _
    Public Sub GetModBookMaintListByPRONumberTest()
        Try
            Dim target As New DAL.NGLLookupDataProvider(testParameters)
            Dim dtModDate As Date = Date.Now()
            Dim intDaysBack As Integer = -365
            Dim lvLookup As List(Of DTO.vBookMaintLookup) = target.GetBookMaintFilters(0, Date.Now.ToShortDateString, intDaysBack).ToList()
            If lvLookup Is Nothing OrElse lvLookup.Count() < 1 Then
                Assert.Fail("GetModBookMaintListByPRONumberTest Failed no Bookings")
            End If
            'get one of the order using the order number
            Dim lPROLookup = target.GetBookMaintItemByProNumber(lvLookup(0).BookProNumber)
            If lPROLookup Is Nothing OrElse lPROLookup.BookControl = 0 Then
                Assert.Fail("GetModBookMaintListByPRONumberTest Failed invalid Order number")
            End If
            Dim intControl As Integer = lPROLookup.BookControl
            'clear the object
            lPROLookup = Nothing
            'Check if the order has changed (should return nothing or an empty record)
            lPROLookup = target.GetBookMaintItemByProNumber(lvLookup(0).BookProNumber, dtModDate)
            If Not lPROLookup Is Nothing AndAlso lPROLookup.BookControl <> 0 Then
                Assert.Fail("GetModBookMaintListByPRONumberTest Failed order number returned but was not modified")
            End If
            'clear the object
            lPROLookup = Nothing
            Dim dtUserChangedData As Date = Date.Now()
            target.executeSQL("Update Book set BookModDate = '" & dtUserChangedData.ToString() & "' Where Book.BookControl = " & intControl)
            'this time the order was modified so we should get a record back
            lPROLookup = target.GetBookMaintItemByProNumber(lvLookup(0).BookProNumber, dtModDate)
            If lPROLookup Is Nothing OrElse lPROLookup.BookControl = 0 Then
                Assert.Fail("GetModBookMaintListByPRONumberTest Failed modified order number was not returned")
            End If

            If lvLookup.Any(Function(x) x.BookControl = lPROLookup.BookControl) Then
                System.Diagnostics.Debug.WriteLine("Modified Booking Already Exists, Pro#: " & lPROLookup.BookProNumber)
            Else
                System.Diagnostics.Debug.WriteLine("Modified Booking Added to Lookup List, Pro#: " & lPROLookup.BookProNumber)
                'add mod to lanes
                lvLookup.Add(lPROLookup)
            End If
        Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("GetModBookMaintListByPRONumberTest Unexpected Error: " & ex.Message)
        Finally

        End Try
    End Sub

    <TestMethod>
    Public Sub GetAccessorialsByCarrierByLegalEntityTest()
        'Try
        'Dim target As New DAL.NGLLookupDataProvider(testParameters)

        'Dim res = target.GetAccessorialsByCarrierByLegalEntity(0, "A")

        'If res Is Nothing OrElse res.Count() < 1 Then
        '        Assert.Fail("GetAccessorialsByCarrierByLegalEntityTest Failed no records")
        '    End If




        'Catch ex As Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException
        '    Throw
        'Catch ex As Exception
        '    Assert.Fail("GetAccessorialsByCarrierByLegalEntityTest Unexpected Error: " & ex.Message)
        'Finally

        'End Try
    End Sub

    <TestMethod>
    Public Sub GetCarrierActiveListTest()
        Try
            testParameters.Database = "NGLMASOPSTEST"
            testParameters.DBServer = "NGLRDP06D\DTMS365"
            'testParameters.WCFAuthCode = "WCFTEST"
            Dim target As New DAL.NGLLookupDataProvider(testParameters)
            Dim dtModDate As Date = Date.Now()
            Dim lLanes = target.GetViewLookupList("CarrierActive", 1, Nothing, Nothing).ToList()
            If lLanes Is Nothing OrElse lLanes.Count() < 1 Then
                Assert.Fail("GetCarrierActiveListTest Failed no Active Carriers")
            End If
        Catch ex As AssertFailedException
            Throw
        Catch ex As Exception
            Assert.Fail("GetCarrierActiveListTest Unexpected Error: " & ex.Message)
        Finally
        End Try
    End Sub



End Class
