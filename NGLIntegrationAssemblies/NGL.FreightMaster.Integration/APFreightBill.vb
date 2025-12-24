

Partial Public Class APFreightBill
    Partial Class APFreightBillDataDataTable

        Private Sub APFreightBillDataDataTable_ColumnChanging(ByVal sender As System.Object, ByVal e As System.Data.DataColumnChangeEventArgs) Handles Me.ColumnChanging
            If (e.Column.ColumnName = Me.APCarrierNumberColumn.ColumnName) Then
                'Add user code here
            End If

        End Sub

    End Class

End Class
