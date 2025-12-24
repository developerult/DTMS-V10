Public Class TestLaneIntegration
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub


    Private Sub btnRunTest_Click(sender As Object, e As EventArgs) Handles btnRunTest.Click
        Try
            Dim oHeaders As New List(Of Ngl.FreightMaster.Integration.clsLaneObject70)
            Dim oHeader = readHeaderInfo()
            oHeaders.add(oHeader)
            processData70(oHeaders)
        Catch ex As Exception
            Me.lblError.Text = ex.Message
        End Try
    End Sub

    Private Function readHeaderInfo() As clsLaneObject70
       
        Dim oHeader As New clsLaneObject70 With { _
            .LaneName = tbLaneName.Text _
            , .LaneNumber = tbLaneNumber.Text _
            , .LaneLegalEntity = tbLaneLegalEntity.Text _
            , .LaneCompAlphaCode = tbLaneCompAlphaCode.Text _
            , .LaneOrigCompAlphaCode = tbLaneCompAlphaCode.Text _
            , .LaneDestName = tbLaneDestName.Text _
            , .LaneDestAddress1 = tbLaneDestAddress1.Text _
            , .LaneDestCity = tbLaneDestCity.Text _
            , .LaneDestState = tbLaneDestState.Text _
            , .LaneDestCountry = tbLaneDestCountry.Text _
            , .LaneDestZip = tbLaneDestZip.Text _
            , .LaneOriginAddressUse = False _
            }
        Return oHeader
    End Function

    Private Sub processData70(ByVal LaneHeaders As List(Of Ngl.FreightMaster.Integration.clsLaneObject70))


        Dim LaneCalendar As New List(Of Ngl.FreightMaster.Integration.clsLaneCalendarObject70)
        Dim ReturnMessage As String
        Dim result As Integer = 0
        Dim oRes As New clsIntegrationUpdateResults
        ReturnMessage = ""
        Try
            If LaneHeaders Is Nothing OrElse LaneHeaders.Count < 1 Then
                Me.lblResults.Text = "Empty Header"
                Return
            End If
            Dim lane As New Ngl.FreightMaster.Integration.clsLane
            Utilities.populateIntegrationObjectParameters(lane)
            oRes = lane.ProcessObjectData70(LaneHeaders, Utilities.GetConnectionString(), LaneCalendar)
            result = oRes.ReturnValue
            ReturnMessage = lane.LastError
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