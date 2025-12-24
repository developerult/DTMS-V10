Public Class Form1

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try

            Me.rtbResults.Text = ""
            Dim strResults As String = ""
            Dim oGP As New NGL.FM.GPIntegration.clsApplication
            'NGL.FM.GPIntegration.clsApplication()
            Dim sUser As String = Me.tbUser.Text
            Dim sURL As String = Me.tbURL.Text
            'Dim ocomp As NGL.FM.GPIntegration.GPService.Company() = oGP.GetCompanyList(-32768, 32767, sURL, sUser)
            'If Not ocomp Is Nothing AndAlso ocomp.Count() > 0 Then
            '    For Each c In ocomp
            '        strResults &= c.Name & vbCrLf
            '    Next
            '    MessageBox.Show("Success!")
            'End If
            Me.rtbResults.Text = strResults
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub
End Class
