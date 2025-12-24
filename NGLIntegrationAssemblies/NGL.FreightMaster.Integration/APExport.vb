

Partial Public Class APExport
    Partial Class APExportDataDataTable

        Private Sub APExportDataDataTable_ColumnChanging(ByVal sender As System.Object, ByVal e As System.Data.DataColumnChangeEventArgs) Handles Me.ColumnChanging
            If (e.Column.ColumnName = Me.PrevSentDateColumn.ColumnName) Then
                'Add user code here
            End If

        End Sub

    End Class

End Class
