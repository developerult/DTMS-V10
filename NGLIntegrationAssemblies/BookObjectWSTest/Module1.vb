Module Module1

    Enum WebServiceReturnValues
        nglDataIntegrationComplete
        nglDataConnectionFailure
        nglDataValidationFailure
        nglDataIntegrationFailure
        nglDataIntegrationHadErrors
    End Enum
    Sub Main()

        Dim sSource As String = "BookObjectWSTest"

        Dim intResult As Integer = 0
        Dim strLastError As String = ""

        Dim oBook As New BookObjectWS.BookObject
        Try
            ' oBook.Url = "http://nglwsdev70.nextgeneration.com/BookObject.ASMX"
            oBook.Url = "http://NGLWSTEST.Juice4U.local/BookObject.ASMX"
            oBook.Timeout = -1

            'Test creating a new Lane via web services
            Dim bookObj As New BookTestObject()
            'bookObj = createJuiceBookRecord(6, False, False)
            bookObj.fillJuiceBookTestData()


            'send the new POHdr data through the web services (70 version)

            intResult = oBook.ProcessData70("WSTEST", bookObj.BookHeaders(), bookObj.BookDetails(), strLastError)

            Select Case intResult
                Case WebServiceReturnValues.nglDataConnectionFailure
                    Console.WriteLine("Database Connection Failure Error: " & strLastError)
                Case WebServiceReturnValues.nglDataIntegrationFailure
                    Console.WriteLine("Data Integration Failure Error: " & strLastError)
                Case WebServiceReturnValues.nglDataIntegrationHadErrors
                    'Assert.Fail("Some Errors: " & strLastError)
                Case WebServiceReturnValues.nglDataValidationFailure
                    Console.WriteLine("Data Validation Failure Error: " & strLastError)
                Case WebServiceReturnValues.nglDataIntegrationComplete
                    'Main.insertMessage("Success! Data imported.")
                    Console.WriteLine("Success!")
                Case Else
                    Console.WriteLine("Invalid Return Value.")
            End Select

        Catch ex As ApplicationException
            Console.WriteLine("Application Exception For {0}: {1} ", sSource, ex.Message)

        Catch ex As Exception
            Console.WriteLine("Unexpected Error For {0}: {1} ", sSource, ex.Message)
        Finally
            Console.WriteLine("Press Enter To Continue")
            Console.ReadLine()
        End Try

    End Sub

End Module
