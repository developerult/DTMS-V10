Imports Microsoft.VisualBasic
Imports System.Data

Namespace FMDataTableAdapters

    Partial Public Class tblPickListTableAdapter
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


    Partial Public Class spgetEDI204ItemDetailsTableAdapter
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

    Partial Public Class spGetEDI204TruckLoadDataTableAdapter
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

    Partial Public Class CarrierEDIISATableAdapter
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

    Partial Public Class CarrierEDIGSTableAdapter
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

    Partial Public Class CarrierEDITableAdapter
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
