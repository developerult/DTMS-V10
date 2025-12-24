Imports Microsoft.VisualBasic
Imports System.Data

Namespace SystemSecurityDataTableAdapters
    Partial Public Class tblReportParTypeTableAdapter
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

    Partial Public Class tblReportParTableAdapter
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

    Partial Public Class tblReportListTableAdapter
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

    Partial Public Class tblReportSecurityXrefTableAdapter
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

    Partial Public Class tblUserSecurityTableAdapter
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

