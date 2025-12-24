Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects
Imports BLL = Ngl.FM.BLL 
Imports DAL = Ngl.FreightMaster.Data

<TestClass()>
Public Class LaneTransLoadXrefDetBLLTest
    Inherits TestBase

    Protected Sub deleteBook(ByVal book As DTO.Book)
        Try
            Dim oBook As New DAL.NGLBookData(testParameters)
            oBook.DeleteRecord(book)
        Catch ex As Exception

        End Try
    End Sub
    Protected Function addBookWDetails(ByVal book As DTO.Book) As DTO.Book
        Try
            Dim oBook As New BLL.NGLBookBLL(testParameters)
            Return oBook.CreateBookWithDetails(book)
        Catch ex As Exception

        End Try
    End Function

    Protected Function getBookRevByBookControl(ByVal iBookControl As Integer) As DTO.BookRevenue
        Try
            Dim oBook As New DAL.NGLBookRevenueData(testParameters)
            Return oBook.GetBookRevenueWDetailsFiltered(iBookControl)
        Catch ex As Exception
            Return Nothing 'no data is available
        End Try
    End Function

    Protected Function getBookWDetails(ByVal bookProNumber As String) As DTO.Book
        Try
            Dim oBook As New DAL.NGLBookData(testParameters)
            Return oBook.GetBookFiltered(BookProNumber:=bookProNumber)
        Catch ex As Exception
            Return Nothing 'no data is available
        End Try
    End Function


    Protected Function saveBookRevWDetails(ByVal bookrev As DTO.BookRevenue) As DTO.BookRevenue
        Try
            Dim oBook As New DAL.NGLBookRevenueData(testParameters)
            Return oBook.UpdateRecordWithDetails(bookrev)
        Catch ex As Exception
            Return Nothing 'no data is available
        End Try
    End Function

    ''' <summary>
    ''' Process Transload Logic
    ''' </summary>
    ''' <remarks></remarks>
    <TestMethod()>
    Public Sub TestProcessTransLoadFacility()
        testParameters.DBServer = "NGLRDP06D"
        testParameters.Database = "SHIELDSMASUnitTest"
        Dim originalProNumber As String = "YAK757265"
        ' If LaneTransLoadXrefDetBLL.processTransLoadFacility(newBooking.SolutionDetailBookControl) Then Return
        Dim originalBook As DTO.Book = getBookWDetails(originalProNumber)
        If originalBook Is Nothing OrElse originalBook.BookControl = 0 Then
            Assert.Fail("TestProcessTransLoadFacility - could not find book record.")
            Return
        End If
        Try
            Dim transloadBLL As New BLL.NGLLaneTransLoadXrefDetBLL(testParameters)
            'this should return true because we setup the transload facilitys in the lanes in the databas already
            If transloadBLL.processTransLoadFacility(originalBook.BookControl) = False Then
                Assert.Fail("TestProcessTransLoadFacility - could not process transload when it should")
                Return
            End If
            Assert.Fail("TestProcessTransLoadFacility - not fully tested.")
        Catch ex As Exception
            Assert.Fail("TestProcessTransLoadFacility - could not process transload when it should")

        Finally
            Try
                'delete the book and start over.
                deleteBook(originalBook)
                Dim newBook As DTO.Book = addBookWDetails(originalBook)
                Dim bookRev As DTO.BookRevenue = getBookRevByBookControl(newBook.BookControl)
                bookRev.ResetToNStatus()
                saveBookRevWDetails(bookRev)
            Catch ex As Exception

            End Try
        End Try

    End Sub

End Class