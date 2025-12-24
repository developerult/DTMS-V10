Public Class TestCompWebService
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub btnRunTest_Click(sender As Object, e As EventArgs) Handles btnRunTest.Click
        Try
            Dim oHeaders As New List(Of Ngl.FreightMaster.Integration.clsCompanyHeaderObject70)
            Dim oHeader = readHeaderInfo()
            oHeaders.Add(oHeader)
            '***********  the code below could throw a null reference exception because oHeaders(1) is nothing
            'Dim oHeaders(1) As NGL.FreightMaster.Integration.clsCompanyHeaderObject70
            'Dim oHeader = readHeaderInfo()
            'oHeaders(0) = oHeader
            processData70(oHeaders.tolist())
        Catch ex As Exception
            Me.lblError.Text = ex.Message
        End Try
    End Sub

    Private Function readHeaderInfo() As clsCompanyHeaderObject70
        Dim blnActive As Boolean = True
        Boolean.TryParse(tbCompActive.Text, blnActive)
        Dim oHeader As New clsCompanyHeaderObject70 With { _
            .CompName = tbCompName.Text _
            , .CompAlphaCode = tbCompAlphaCode.Text _
            , .CompLegalEntity = tbCompLegalEntity.Text _
            , .CompStreetAddress1 = tbCompStreetAddress1.Text _
            , .CompStreetCity = tbCompStreetCity.Text _
            , .CompStreetState = tbCompStreetState.Text _
            , .CompStreetCountry = tbCompStreetCountry.Text _
            , .CompStreetZip = tbCompStreetZip.Text _
            , .CompAbrev = tbCompAbrev.Text _
            , .CompActive = blnActive _
            }
        Return oHeader
    End Function

    Private Sub processData70(ByVal CompanyHeaders As List(Of NGL.FreightMaster.Integration.clsCompanyHeaderObject70))


        Dim CompanyContacts As New List(Of NGL.FreightMaster.Integration.clsCompanyContactObject70)
        Dim CompanyCalendar As New List(Of NGL.FreightMaster.Integration.clsCompanyCalendarObject70)
        Dim ReturnMessage As String
        Dim result As Integer = 0
        Dim oRes As New clsIntegrationUpdateResults
        ReturnMessage = ""
        Try
            If CompanyHeaders Is Nothing OrElse CompanyHeaders.Count < 1 Then
                Me.lblResults.Text = "Empty Header"
                Return
            End If
            Dim company As New NGL.FreightMaster.Integration.clsCompany
            Utilities.populateIntegrationObjectParameters(company)
            oRes = company.ProcessObjectData70(CompanyHeaders, CompanyContacts, Utilities.GetConnectionString(), CompanyCalendar)
            result = oRes.ReturnValue
            ReturnMessage = company.LastError
            Select Case oRes.ReturnValue
                Case Configuration.ProcessDataReturnValues.nglDataConnectionFailure
                    Me.lblResults.Text = "Data Connection Failure: " & ReturnMessage
                Case Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
                    Me.lblResults.Text = "Data Connection Failure: " & ReturnMessage
                Case Configuration.ProcessDataReturnValues.nglDataIntegrationHadErrors
                    Me.lblResults.Text = "Data Integration Had Errors: " & ReturnMessage
                Case Configuration.ProcessDataReturnValues.nglDataValidationFailure
                    Me.lblResults.Text = "Data Validation Errors: " & ReturnMessage
                Case Else
                    Me.lblResults.Text = "Success"
            End Select

        Catch ex As Exception
            Me.lblError.Text = ex.Message
        Finally


        End Try

    End Sub

End Class