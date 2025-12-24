
Imports TMS = Ngl.FreightMaster.Integration

Module Module1

    Sub Main()
        processOrderData()
    End Sub


    Private Function processOrderData() As Boolean
        Dim blnRet As Boolean = False
        Try

            'Log("Begin Process Order Data ")
            Dim oBookIntegration As TMS.clsBook
            oBookIntegration = New Ngl.FreightMaster.Integration.clsBook()
            'populateIntegrationObjectParameters(oBookIntegration)
            Dim oBookHeaders As New List(Of TMS.clsBookHeaderObject70)
            Dim oBookDetails As New List(Of TMS.clsBookDetailObject70)
            'Dim oNAVWebService = New NAVService.DynamicsTMSWebServices()
            'oNAVWebService.UseDefaultCredentials = True
            'Dim oNavOrders = New NAVService.DynamicsTMSBookings()
            'oNAVWebService.GetBookings(oNavOrders, True, False)
            'Dim strSkip As New List(Of String)
            'Dim strItemSkip As New List(Of String)
            'For Each c In oNavOrders.Booking
            '    If Not c Is Nothing AndAlso Not String.IsNullOrWhiteSpace(c.PONumber) Then
            '        Dim oHeader = New TMS.clsBookHeaderObject70
            '        CopyMatchingFields(oHeader, c, strSkip)
            '        oBookHeaders.Add(oHeader)
            '        For Each item In c.Items
            '            If Not item Is Nothing AndAlso Not String.IsNullOrWhiteSpace(item.ItemPONumber) Then
            '                Dim oItem As New TMS.clsBookDetailObject70
            '                CopyMatchingFields(oHeader, item, strItemSkip)
            '                oBookDetails.Add(oItem)
            '            End If
            '        Next
            '    End If
            'Next
            'save changes to database 
            'Dim oResults As TMS.Configuration.ProcessDataReturnValues = oBookIntegration.ProcessObjectData(oBookHeaders, oBookDetails, "")
            'Dim sLastError As String = oBookIntegration.LastError
            'Select Case oResults
            '    Case TMS.Configuration.ProcessDataReturnValues.nglDataConnectionFailure
            '        Log("Error Data Connection Failure! could not import Order information:  " & sLastError)
            '    Case TMS.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
            '        Log("Error Integration Failure! could not import Order information:  " & sLastError)
            '    Case TMS.Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
            '        Log("Warning Integration Had Errors! could not import some Order information:  " & sLastError)
            '    Case TMS.Configuration.ProcessDataReturnValues.nglDataValidationFailure
            '        Log("Error Data Validation Failure! could not import Order information:  " & sLastError)
            '    Case Else
            '        'success
            '        Dim strNumbers As String = "" 'To do read order numbers from NAV data files
            '        Log("Success! the following Order Numbers were processed: " & strNumbers)
            '        'TODO: add code to send confirmation back to NAV that the orders were processed
            '        'mark process and success
            '        blnRet = True
            'End Select
            Log("Process Order Data Complete")
            'TODO: add additional error handlers as needed
        Catch ex As Exception
            Throw
        End Try

        Return blnRet
    End Function

    Private Function Log(ByVal sMsg As String)
        Console.WriteLine(sMsg)
#Disable Warning BC42105 ' Function 'Log' doesn't return a value on all code paths. A null reference exception could occur at run time when the result is used.
    End Function
#Enable Warning BC42105 ' Function 'Log' doesn't return a value on all code paths. A null reference exception could occur at run time when the result is used.
End Module
