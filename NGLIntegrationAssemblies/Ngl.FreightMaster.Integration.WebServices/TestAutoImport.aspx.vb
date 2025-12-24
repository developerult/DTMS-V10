Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports NGL.FreightMaster.Integration
Imports System.Xml.Serialization
Imports NGL.FreightMaster.Integration.Configuration

Public Class TestAutoImport
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Function ProcessData(ByVal AuthorizationCode As String, _
                                ByVal strOrderNumber As String, _
                                ByRef ReturnMessage As String) As ProcessDataReturnValues
        Dim result As Integer = ProcessDataReturnValues.nglDataIntegrationFailure
        ReturnMessage = ""
        Dim sSource As String = "TestAutoImport.ProcessData"
        Dim sDataType As String = "Book"
        Try
            If String.IsNullOrWhiteSpace(strOrderNumber) Then
                ReturnMessage = "Order Number is Required"
                Utilities.LogResults(sSource, 0, ReturnMessage, AuthorizationCode)
                result = ProcessDataReturnValues.nglDataIntegrationComplete
                Return result
            End If
            Dim book As New NGL.FreightMaster.Integration.clsBook
            If Not Utilities.validateAuthCode(AuthorizationCode) Then Return result
            Utilities.populateIntegrationObjectParameters(book)
            result = book.ProcessAutoImportTest(strOrderNumber, Utilities.GetConnectionString(), ReturnMessage)
            Utilities.LogResults(sSource, result, ReturnMessage, AuthorizationCode)

        Catch ex As Exception
            ReturnMessage = ex.Message
            Utilities.LogResults(sSource, 10000, ex.Message & vbCrLf & ex.StackTrace, AuthorizationCode)
            Utilities.LogException(sSource, result, "Cannot process " & sDataType & " data.  ", ex, AuthorizationCode, "Process " & sDataType & " Data Failure")
        Finally
            
        End Try
        Return result
    End Function

    Private Sub btnRunTest_Click(sender As Object, e As EventArgs) Handles btnRunTest.Click
        Me.lblError.Text = ""
        Me.lbResults.Items.Clear()
        Dim strOrderNumber As String = Me.tbOrderNumber.Text
        Dim strAuthCode As String = Me.tbAuthCode.Text
        Dim blnValidated As Boolean = True
        If String.IsNullOrWhiteSpace(strOrderNumber) Then
            blnValidated = False
            lbResults.Items.Add("Blank Order Number Not Allowed")
        End If

        If String.IsNullOrWhiteSpace(strAuthCode) Then
            blnValidated = False
            lbResults.Items.Add("Blank Auth Code Not Allowed")
        End If
        Dim ReturnMessage As String = ""
        If blnValidated Then
            Try

                Dim results As ProcessDataReturnValues = ProcessData(strAuthCode, strOrderNumber, ReturnMessage)

                Select Case results
                    Case ProcessDataReturnValues.nglDataConnectionFailure
                        lbResults.Items.Add("Data Connection Failure")
                    Case ProcessDataReturnValues.nglDataIntegrationFailure
                        lbResults.Items.Add("Integration Failure")
                    Case ProcessDataReturnValues.nglDataIntegrationHadErrors
                        lbResults.Items.Add("Some Errors")
                    Case ProcessDataReturnValues.nglDataValidationFailure
                        lbResults.Items.Add("Data or User Validation Failure")
                    Case Else
                        lbResults.Items.Add("Success!")
                End Select

            Catch ex As Exception
                Me.lblError.Text = ex.ToString()
            End Try
        End If

    End Sub
End Class