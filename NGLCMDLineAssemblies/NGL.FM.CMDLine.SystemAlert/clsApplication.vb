Imports System.IO
Imports System.ServiceModel
Imports NGL.FreightMaster.Core
Imports NGL.Core.Communication
Imports DTran = NGL.Core.Utility.DataTransformation
Imports NGL.Core.Communication.Email
Imports NGL.Core.Communication.General
Imports NData = Ngl.FreightMaster.Data
Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects
Imports LTS = Ngl.FreightMaster.Data.LTS
Imports Ngl.Core

Public Class clsApplication : Inherits Ngl.FreightMaster.Core.NGLCommandLineBaseClass
    Protected oConfig As New Ngl.FreightMaster.Core.UserConfiguration

    Public Sub ProcessData()
        Me.openLog()
        Me.Log(Source & " Applicaiton Start")
        'use the database name as part of the source
        displayParameterData()
        fillConfig()

        Try
            Log("Begin Process Data ")
            Dim oList As DTO.tblSystemError()
            Try
                Log("Read next 10 system alerts ")
                oList = GetSystemAlerts(10)
            Catch ex As ApplicationException
                If ex.Message = "E_NoData" Then
                    Log("No new system alerts have been reported ")
                Else
                    LogError(Me.Source & " Get System Alerts Failure", "Could not read system alerts due to a sql exception.", Me.AdminEmail, ex)
                End If
                Return
            Catch ex As Exception
                Throw
            End Try
            Dim strUpdateErrors As String = ""
            If Not oList Is Nothing AndAlso oList.Count > 0 Then
                Log("Sending Alert Emails ")
                Dim strEmailMessage As String = "<h2>New System Alerts Found</h2>"
                For Each oAlert In oList
                    strEmailMessage &= vbCrLf & "<hr />" & vbCrLf
                    With oAlert
                        strEmailMessage &= "ID: " & .ErrID.ToString & "<br />" & vbCrLf
                        If Not String.IsNullOrEmpty(.Message) Then strEmailMessage &= "Message: " & .Message & "<br />" & vbCrLf
                        If Not String.IsNullOrEmpty(.Message) Then strEmailMessage &= "Record: " & .Record & "<br />" & vbCrLf
                        If Not String.IsNullOrEmpty(.Message) Then strEmailMessage &= "User: " & .CurrentUser & "<br />" & vbCrLf
                        If .CurrentDate.HasValue Then strEmailMessage &= "Date: " & .CurrentDate.Value.ToString & "<br />" & vbCrLf
                        If .ErrorNumber.HasValue Then strEmailMessage &= "Error Number: " & .ErrorNumber.Value.ToString & "<br />" & vbCrLf
                        If .ErrorSeverity.HasValue Then strEmailMessage &= "Severity: " & .ErrorSeverity.Value.ToString & "<br />" & vbCrLf
                        If .ErrorState.HasValue Then strEmailMessage &= "State: " & .ErrorState.Value.ToString & "<br />" & vbCrLf
                        If Not String.IsNullOrEmpty(.Message) Then strEmailMessage &= "Procedure: " & .ErrorProcedure & "<br />" & vbCrLf
                        If .ErrorLineNbr.HasValue Then strEmailMessage &= "Line Nbr: " & .ErrorLineNbr.Value.ToString & "<br />" & vbCrLf
                        strEmailMessage &= "<hr />"

                        'update the status as read
                        If Not UpdateSystemAlert(.ErrID) Then
                            strUpdateErrors &= "There was a problem updating the sent status for system alert ID " & .ErrID & ".  This message may be retransmitted durring the next batch." & "<br />" & vbCrLf
                            Log("A System Alert, ID: " & .ErrID & ", was not updated as sent; duplicate messages are possible.")
                        End If
                    End With
                Next
                If Not String.IsNullOrEmpty(strUpdateErrors) Then
                    strUpdateErrors = vbCrLf & "<hr />" & vbCrLf & strUpdateErrors & "<hr />" & vbCrLf
                    strEmailMessage &= strUpdateErrors
                End If
                LogError(Me.Source & " System Alerts Found", strEmailMessage, Me.AdminEmail)
            Else
                Log("No new system alerts have been reported ")
            End If


            Log("Process Data Complete")
        Catch ex As Exception
            LogError(Me.Source & " Process System Alert Messages Failure", "An unexpected error has occurred while processing system alert messages.", Me.AdminEmail, ex)
        Finally
            Me.closeLog(0)
        End Try
    End Sub

    Public Sub fillConfig()
        Try
            With oConfig
                .AdminEmail = Me.AdminEmail
                .AutoRetry = Me.AutoRetry
                .ConnectionString = ConnectionString
                .Database = Me.Database
                .DBServer = Me.DBServer
                .Debug = Me.Debug
                .FromEmail = Me.FromEmail
                .GroupEmail = Me.GroupEmail
                .INIKey = Me.INIKey
                .KeepLogDays = Me.KeepLogDays
                .ResultsFile = Me.ResultsFile
                .LogFile = Me.LogFile
                .SaveOldLog = Me.SaveOldLog
                .SMTPServer = Me.SMTPServer
                .Source = Me.Source
            End With

        Catch ex As Exception
            Throw New ApplicationException(Source & " Fill Configuration Failure", ex)
        End Try
    End Sub

    Private Function GetSystemAlerts(ByVal MaxReturn As Integer) As DTO.tblSystemError()

        Using db As New NData.NGLMASSYSDataContext(ConnectionString)
            Try
                'if there are no null values to deal with we can build the array directly
                Dim Errors() As DTO.tblSystemError = ( _
                    From t In db.tblSystemErrors _
                    Where t.ErrorAlertSent = 0
                    Order By t.CurrentDate Descending _
                    Select New DTO.tblSystemError With {.ErrID = t.ErrID, _
                                                        .Message = t.Message, _
                                                        .Record = t.Record, _
                                                        .CurrentUser = t.CurrentUser, _
                                                        .CurrentDate = t.CurrentDate, _
                                                        .ErrorNumber = t.ErrorNumber, _
                                                        .ErrorSeverity = t.ErrorSeverity, _
                                                        .ErrorState = t.ErrorState, _
                                                        .ErrorProcedure = t.ErrorProcedure, _
                                                        .ErrorLineNbr = t.ErrorLineNbr}).Take(MaxReturn).ToArray()
                Return Errors
            Catch ex As System.Data.SqlClient.SqlException
                Throw New System.ApplicationException(Me.Source.ToString & ex.Message, ex)
            Catch ex As InvalidOperationException
                Throw New System.ApplicationException("E_NoData", ex)
            Catch ex As Exception
                Throw
            End Try

            Return Nothing

        End Using

    End Function

    Private Function UpdateSystemAlert(ByVal ErrID As Int64) As Boolean
        Dim blnRet As Boolean = False
        Try
            Dim strSQL As String = "Update dbo.tblSystemErrors set ErrorAlertSent = 1, ErrorAlertSentDate = '" & Date.Now.ToString & "' Where ErrID = " & ErrID
            'Create a data connection 
            Dim oCon As New System.Data.SqlClient.SqlConnection
            Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)

            Try

                If oQuery.executeSQLQuery(oCon, strSQL, 1) Then
                    blnRet = True
                End If

          
            Catch ex As Ngl.Core.DatabaseRetryExceededException
                Me.LogException(Me.Source & ".UpdateSystemAlert: Status update failed to execute (retry exceeded)." & vbCrLf & strSQL, ex)
            Catch ex As System.Data.SqlClient.SqlException
                Me.LogException(Me.Source & ".UpdateSystemAlert: SQL Exception." & vbCrLf & strSQL, ex)
            Catch ex As Exception
                Throw
            Finally
                oQuery = Nothing
            End Try
        Catch ex As Exception
            Throw
        End Try
        Return blnRet
    End Function







End Class
