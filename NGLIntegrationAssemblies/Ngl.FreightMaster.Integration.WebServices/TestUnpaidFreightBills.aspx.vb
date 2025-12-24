Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Xml.Serialization
Imports Ngl.FreightMaster.Integration.Configuration
Imports LTS = Ngl.FreightMaster.Data.LTS
Imports DAL = Ngl.FreightMaster.Data


Public Class TestUnpainFreightBills
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub btnRunTest_Click(sender As Object, e As EventArgs) Handles btnRunTest.Click
        Dim strLastError As String = ""
        Dim lUnpaid As New List(Of Ngl.FreightMaster.Integration.APUnpaidFreightBills)
        Dim apExport As New Ngl.FreightMaster.Integration.clsAPExport()
        'set the default value to false
        Dim WSResult As Integer = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationFailure
        Dim strCriteria As String = ""
        lbResults.Items.Clear()
        Try

            Utilities.populateIntegrationObjectParameters(apExport)
            With apExport
                .MaxRowsReturned = 20
            End With
            lUnpaid = apExport.GetUnpaidFreightBills(Utilities.GetConnectionString(), Me.tbLegalEntity.Text)
            If Not lUnpaid Is Nothing Then
                WSResult = Ngl.FreightMaster.Integration.Configuration.ProcessDataReturnValues.nglDataIntegrationComplete
                If lUnpaid.Count() > 0 Then
                    For Each upaid As Ngl.FreightMaster.Integration.APUnpaidFreightBills In lUnpaid
                        lbResults.Items.Add("Freight Bill: " & upaid.BookFinAPBillNumber & " Amount: " & upaid.ActualCost.ToString("c"))
                    Next
                End If

            End If
            strLastError = apExport.LastError

        Catch ex As Exception
            strLastError = ex.Message
        End Try
        Me.lblError.Text = strLastError
    End Sub
End Class