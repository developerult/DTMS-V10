Imports Microsoft.VisualBasic
Imports System.Data

Namespace ApplicationDataTableAdapters
    Partial Public Class ParameterTableAdapter
        Public Sub SetCommandTimeOut(ByVal intTimeOut As Integer)
            If CommandCollection Is Nothing OrElse CommandCollection.Count() < 1 Then Return
            For Each command As IDbCommand In CommandCollection
                command.CommandTimeout = intTimeOut
            Next
        End Sub

        Public Sub SetConnectionString(ByVal strConnection As String)
            Me.Connection.ConnectionString = strConnection
            If CommandCollection Is Nothing OrElse CommandCollection.Count() < 1 Then Return
            For Each Command As IDbCommand In CommandCollection
                Command.Connection.ConnectionString = strConnection
            Next
        End Sub

    End Class

    Partial Public Class CompParameterTableAdapter
        Public Sub SetCommandTimeOut(ByVal intTimeOut As Integer)
            If CommandCollection Is Nothing OrElse CommandCollection.Count() < 1 Then Return
            For Each command As IDbCommand In CommandCollection
                command.CommandTimeout = intTimeOut
            Next
        End Sub

        Public Sub SetConnectionString(ByVal strConnection As String)
            Me.Connection.ConnectionString = strConnection
            If CommandCollection Is Nothing OrElse CommandCollection.Count() < 1 Then Return
            For Each Command As IDbCommand In CommandCollection
                Command.Connection.ConnectionString = strConnection
            Next
        End Sub

    End Class

    Partial Public Class vFormSecurityValuesTableAdapter
        Public Sub SetCommandTimeOut(ByVal intTimeOut As Integer)
            If CommandCollection Is Nothing OrElse CommandCollection.Count() < 1 Then Return
            For Each command As IDbCommand In CommandCollection
                command.CommandTimeout = intTimeOut
            Next
        End Sub

        Public Sub SetConnectionString(ByVal strConnection As String)
            Me.Connection.ConnectionString = strConnection
            If CommandCollection Is Nothing OrElse CommandCollection.Count() < 1 Then Return
            For Each Command As IDbCommand In CommandCollection
                Command.Connection.ConnectionString = strConnection
            Next
        End Sub

    End Class

    Partial Public Class vReportSecurityValuesTableAdapter
        Public Sub SetCommandTimeOut(ByVal intTimeOut As Integer)
            If CommandCollection Is Nothing OrElse CommandCollection.Count() < 1 Then Return
            For Each command As IDbCommand In CommandCollection
                command.CommandTimeout = intTimeOut
            Next
        End Sub

        Public Sub SetConnectionString(ByVal strConnection As String)
            Me.Connection.ConnectionString = strConnection
            If CommandCollection Is Nothing OrElse CommandCollection.Count() < 1 Then Return
            For Each Command As IDbCommand In CommandCollection
                Command.Connection.ConnectionString = strConnection
            Next
        End Sub

    End Class

    Partial Public Class vProcedureSecurityValuesTableAdapter
        Public Sub SetCommandTimeOut(ByVal intTimeOut As Integer)
            If CommandCollection Is Nothing OrElse CommandCollection.Count() < 1 Then Return
            For Each command As IDbCommand In CommandCollection
                command.CommandTimeout = intTimeOut
            Next
        End Sub

        Public Sub SetConnectionString(ByVal strConnection As String)
            Me.Connection.ConnectionString = strConnection
            If CommandCollection Is Nothing OrElse CommandCollection.Count() < 1 Then Return
            For Each Command As IDbCommand In CommandCollection
                Command.Connection.ConnectionString = strConnection
            Next
        End Sub

    End Class


    Partial Public Class spGeneratePickListOrderTableAdapter
        Public Sub SetCommandTimeOut(ByVal intTimeOut As Integer)
            If CommandCollection Is Nothing OrElse CommandCollection.Count() < 1 Then Return
            For Each command As IDbCommand In CommandCollection
                command.CommandTimeout = intTimeOut
            Next
        End Sub

        Public Sub SetConnectionString(ByVal strConnection As String)
            Me.Connection.ConnectionString = strConnection
            If CommandCollection Is Nothing OrElse CommandCollection.Count() < 1 Then Return
            For Each Command As IDbCommand In CommandCollection
                Command.Connection.ConnectionString = strConnection
            Next
        End Sub

    End Class


    Partial Public Class tblRunTaskTableAdapter
        Public Sub SetCommandTimeOut(ByVal intTimeOut As Integer)
            If CommandCollection Is Nothing OrElse CommandCollection.Count() < 1 Then Return
            For Each command As IDbCommand In CommandCollection
                command.CommandTimeout = intTimeOut
            Next
        End Sub

        Public Sub SetConnectionString(ByVal strConnection As String)
            Me.Connection.ConnectionString = strConnection
            If CommandCollection Is Nothing OrElse CommandCollection.Count() < 1 Then Return
            For Each Command As IDbCommand In CommandCollection
                Command.Connection.ConnectionString = strConnection
            Next
        End Sub

    End Class

    Partial Public Class tblTaskLogTableAdapter
        Public Sub SetCommandTimeOut(ByVal intTimeOut As Integer)
            If CommandCollection Is Nothing OrElse CommandCollection.Count() < 1 Then Return
            For Each command As IDbCommand In CommandCollection
                command.CommandTimeout = intTimeOut
            Next
        End Sub

        Public Sub SetConnectionString(ByVal strConnection As String)
            Me.Connection.ConnectionString = strConnection
            If CommandCollection Is Nothing OrElse CommandCollection.Count() < 1 Then Return
            For Each Command As IDbCommand In CommandCollection
                Command.Connection.ConnectionString = strConnection
            Next
        End Sub
    End Class

End Namespace