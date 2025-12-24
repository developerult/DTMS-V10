Imports System.IO
Imports System.ServiceModel
Imports NGL.FreightMaster.Core
Imports NGL.Core.Communication
Imports DTran = NGL.Core.Utility.DataTransformation
Imports NGL.Core.Communication.Email
Imports NGL.Core.Communication.General
Imports NData = NGL.FreightMaster.Data
Imports DTO = NGL.FreightMaster.Data.DataTransferObjects
Imports LTS = NGL.FreightMaster.Data.LTS
Imports NGL.Core

Public Class clsApplication : Inherits NGL.FreightMaster.Core.NGLCommandLineBaseClass
    Protected oConfig As New NGL.FreightMaster.Core.UserConfiguration
    Protected DBCon As New System.Data.SqlClient.SqlConnection
    Public Sub ProcessData()
        Me.openLog()
        Me.Log(Source & " Applicaiton Start")
        'use the database name as part of the source
        displayParameterData()
        fillConfig()

        Dim strMSG As String = ""
        Dim strEmailError As String = ""

        Try
            Log("Begin Process Data ")

            'get a list of loads to be processed
            Dim strSQL As String = "SELECT POHDROrderNumber, POHDROrderSequence, POHDRDefaultCustomer, POHDRPRONumber, POHDRvendor, POHDRModVerify, CompControl FROM dbo.POHdr Inner Join dbo.Comp on dbo.POHdr.POHDRDefaultCustomer = dbo.Comp.CompNumber Where dbo.POHdr.POHDRHoldLoad = 0 AND dbo.Comp.CompSilentTender = 1 Order By CompControl"
            Dim oQuery As New NGL.Core.Data.Query(Me.DBServer, Me.Database)
            Dim dblVal As Double = 0
            'Dim strAllowSilent As String = oQuery.getScalarValue(oCon, "Select top 1 ParValue from dbo.Parameter where parkey = 'GlobalAllowSilentTendering'")
            Dim strAllowSilent As String = oQuery.getScalarValue(DBCon, "Select top 1 ParValue from dbo.Parameter where parkey = 'GlobalAllowSilentTendering'")

            Double.TryParse(strAllowSilent, dblVal)
            If dblVal <> 1 Then
                'Silent Tendering is off so return false
                Log("GlobalAllowSilentTendering Is Off Cannot Continue!")
                Return
            End If
            Dim strDeleteLoadsOnSilent As String = oQuery.getScalarValue(DBCon, "Select top 1 ParValue from dbo.Parameter where parkey = 'GlobalDeleteLoadsOnSilentTendering'")
            Dim blnDeleteLoads As Boolean = False
            Double.TryParse(strDeleteLoadsOnSilent, dblVal)
            If dblVal = 1 Then
                'Delete Loads is on
                blnDeleteLoads = True
            End If
            Log("GlobalDeleteLoadsOnSilentTendering = " & dblVal.ToString)
            Dim intSilentTenderDelay As Integer = CInt(oQuery.getScalarValue(DBCon, "Select top 1 ParValue from dbo.Parameter where parkey = 'GlobalSilentTenderingDelay'"))
            Log("GlobalSilentTenderingDelay = " & intSilentTenderDelay.ToString)
            Dim oQR As NGL.Core.Data.QueryResult = oQuery.ExecuteWithFill(strSQL)
            If Not oQR.Exception Is Nothing Then
                Log("NGL.FM.CMDLine.SilentTenderLoads Read Silent Tender Loads Data Warning!  Cannot Continue: " & vbCrLf & readExceptionMessage(oQR.Exception))
                Return
            End If
            Dim dt As System.Data.DataTable = oQR.Data
            Dim blnLoadsProcessedForCompany As Boolean = False
            Dim intLastComp As Integer = 0
            Dim intFinalizedForComp As Integer = 0
            Dim strTenderedLoads As New List(Of String)
            Dim intDeletedForComp As Integer = 0
            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                Dim strOrderNumber As String = ""
                Dim strBookProNumber As String = ""
                Dim strModVerify As String = ""
                Dim strVendorNumber As String = ""
                Dim intOrderSequence As Integer = 0
                Dim intDefCompNumber As Integer = 0
                Dim intCompControl As Integer = 0
                Log("Processing " & dt.Rows.Count & " Records.")
                For Each oRow As System.Data.DataRow In dt.Rows
                    'Get the next Comp Control Number
                    intCompControl = 0
                    Integer.TryParse(DTran.getDataRowString(oRow, "CompControl", "0"), intCompControl)
                    'Check if the company has changed.
                    If intCompControl <> intLastComp Then
                        Log("Sending Email for Company Control " & intLastComp)
                        If intLastComp > 0 And blnLoadsProcessedForCompany Then
                            'send email
                            Dim strEmail As String = ""
                            Try
                                strEmail = oQuery.getScalarValue(DBCon, "Select dbo.udfGetCompContNotifyEmails(" & intLastComp & ") as Emails", 3)
                            Catch ex As Exception
                                Log("NGL.FM.CMDLine.SilentTenderLoads an unexpected error occurred while attempting to get the company notify contact email information for company number " & intDefCompNumber.ToString & ".  " & readExceptionMessage(ex))
                            End Try
                            If String.IsNullOrEmpty(strEmail) OrElse strEmail.Trim.Length < 5 Then
                                strEmail = AdminEmail
                            End If
                            Dim strBody As String = "<h2>Silent Tender Load for  Company Number " & intDefCompNumber.ToString & "</h2>" & vbCrLf
                            strBody &= "<h3>The following loads were processed:</h3>" & vbCrLf
                            For Each s In strTenderedLoads
                                strBody &= s & vbCrLf
                            Next
                            SendMail(SMTPServer, strEmail, FromEmail, strBody, "Loads Silent Tendered")
                            Log("Email Message Sent: " & strBody)
                            'Clear the message string list
                            strTenderedLoads = New List(Of String)
                            'Reset the number of loads finalized
                            intFinalizedForComp = 0
                            'Reset the loads processed flag to false
                            blnLoadsProcessedForCompany = False
                        End If
                        'Reset the last comp control
                        intLastComp = intCompControl
                    End If
                    'Get the current data for this row
                    strOrderNumber = DTran.getDataRowString(oRow, "POHDROrderNumber", "")
                    strBookProNumber = DTran.getDataRowString(oRow, "POHDRPRONumber", "")
                    strModVerify = DTran.getDataRowString(oRow, "POHDRModVerify", "")
                    strVendorNumber = DTran.getDataRowString(oRow, "POHDRvendor", "")
                    intOrderSequence = 0
                    Integer.TryParse(DTran.getDataRowString(oRow, "POHDROrderSequence", "0"), intOrderSequence)
                    intDefCompNumber = 0
                    Integer.TryParse(DTran.getDataRowString(oRow, "POHDRDefaultCustomer", "0"), intDefCompNumber)
                    'Check the Mod Verify setting and process the data as needed
                    Dim strErrMsg As String = ""
                    Dim blnErrTenderingLoad As Boolean = False
                    Dim blnSkipLoad As Boolean = False
                    Log("Current Data: " & vbCrLf _
                        & "Pro Number: " & strBookProNumber & vbCrLf _
                        & "Order Number: " & strOrderNumber & vbCrLf _
                        & "Mod Verify: " & strModVerify)

                    Select Case strModVerify
                        Case "No Pro"
                            blnErrTenderingLoad = Not runWriteNewBookingWithData(strOrderNumber, intOrderSequence, intDefCompNumber, strErrMsg)
                        Case "FINALIZED"
                            blnErrTenderingLoad = Not runProcessFinalizedData(strOrderNumber, intOrderSequence, intDefCompNumber, strVendorNumber, strErrMsg)
                        Case "DELETED"
                            blnErrTenderingLoad = Not runRemoveDeletedWithData(strOrderNumber, intOrderSequence, intDefCompNumber, strErrMsg)
                            'We do not need to notify the users because this order was already deleted it only existed in the staging table.
                            blnSkipLoad = True
                        Case "DELETE-B"
                            If blnDeleteLoads Then
                                blnErrTenderingLoad = Not runDeleteOrderWithData(strBookProNumber, strOrderNumber, intOrderSequence, intDefCompNumber, strErrMsg)
                            Else
                                blnSkipLoad = True
                            End If
                        Case "DELETE-F"
                            blnSkipLoad = True
                        Case "NO LANE"
                            blnSkipLoad = True
                        Case "NEW TRAN-F"
                            blnSkipLoad = True
                        Case "NEW TRAN"
                            blnSkipLoad = True
                        Case "NEW COMP"
                            blnSkipLoad = True
                        Case Else
                            blnErrTenderingLoad = Not runUpdatePOModificationWithData(strOrderNumber, intOrderSequence, intDefCompNumber, strVendorNumber, strErrMsg)
                    End Select
                    If Not blnErrTenderingLoad Then
                        If Not blnSkipLoad Then
                            blnLoadsProcessedForCompany = True
                            strTenderedLoads.Add("<p>Order Number: " & strOrderNumber & " Seq: " & intOrderSequence.ToString & " Type: " & strModVerify & " Company: " & intDefCompNumber & "</p>")
                        End If
                    Else
                        'Process Error Message and continue
                        Log("NGL.FM.CMDLine.SilentTenderLoads there was a problem while attempting to silent tender Order Number: " & strOrderNumber & " Seq: " & intOrderSequence.ToString & " Type: " & strModVerify & " Company: " & intDefCompNumber & ". The actual error is: " & strErrMsg)
                    End If
                    If intSilentTenderDelay > 0 Then
                        If Debug Then Log("Start Silent Tender Delay: " & Date.Now.ToString)
                        System.Threading.Thread.Sleep(intSilentTenderDelay)
                        If Debug Then Log("End Silent Tender Delay: " & Date.Now.ToString)
                    End If
                Next
                'Finally process the last company data
                If intLastComp > 0 And blnLoadsProcessedForCompany Then
                    Log("Sending Email for Company Control " & intLastComp)
                    'send email
                    Dim strEmail As String = ""
                    Try
                        strEmail = oQuery.getScalarValue(DBCon, "Select dbo.udfGetCompContNotifyEmails(" & intLastComp & ") as Emails", 3)
                    Catch ex As Exception
                        Log("NGL.FM.CMDLine.SilentTenderLoads an unexpected error occurred while attempting to get the company notify contact email information for company number " & intDefCompNumber.ToString & ".  " & readExceptionMessage(ex))
                    End Try
                    If String.IsNullOrEmpty(strEmail) OrElse strEmail.Trim.Length < 5 Then
                        strEmail = AdminEmail
                    End If
                    Dim strBody As String = "<h2>Silent Tender Load for  Company Number " & intDefCompNumber.ToString & "</h2>" & vbCrLf
                    strBody &= "<h3>The following loads were processed:</h3>" & vbCrLf
                    For Each s In strTenderedLoads
                        strBody &= s & vbCrLf
                    Next
                    SendMail(SMTPServer, strEmail, FromEmail, strBody, "Loads Silent Tendered")
                    Log("Email Message Sent: " & strBody)
                End If
            End If
            Log("Process Data Complete")
        Catch ex As NGL.Core.DatabaseRetryExceededException
            LogError(Me.Source & " Process Silent Tender Loads Data Warning", "NGL.FM.CMDLine.SilentTenderLoads could not silent tender loads because of a retry exceeded failure.", Me.AdminEmail, ex)
        Catch ex As NGL.Core.DatabaseLogInException
            LogError(Me.Source & " Process Silent Tender Loads Data Warning", "NGL.FM.CMDLine.SilentTenderLoads could not silent tender loads because of a database login failure.", Me.AdminEmail, ex)
        Catch ex As NGL.Core.DatabaseInvalidException
            LogError(Me.Source & " Process Silent Tender Loads Data Warning", "NGL.FM.CMDLine.SilentTenderLoads could not silent tender loads because of a database access failure.", Me.AdminEmail, ex)
        Catch ex As Exception
            LogError(Me.Source & " Process System Alert Messages Failure", "An unexpected error has occurred while processing system alert messages.", Me.AdminEmail, ex)
        Finally
            Me.closeLog(0)
            Try
                DBCon.Close()
            Catch ex As Exception

            End Try
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

    Public Function SendMail(ByVal strServer As String, ByVal strTo As String, ByVal strFrom As String, ByVal strBody As String, ByVal strSubject As String) As Boolean
        Dim oEmail As New NGL.Core.Communication.Email

        Return oEmail.SendMail(strServer, strTo, strFrom, strBody, strSubject)

    End Function


    Private Function runWriteNewBookingWithData(ByVal strOrderNumber As String, _
                                                    ByVal intOrderSequence As Integer, _
                                                    ByVal intDefCompNumber As Integer, _
                                                    ByRef strMSG As String) As Boolean


        Dim blnDataErr As Boolean = False
        Dim blnRet As Boolean = False
        'Dim oCon As New System.Data.SqlClient.SqlConnection

        Try
            'Check Data
            If String.IsNullOrEmpty(strOrderNumber) OrElse strOrderNumber.Trim.Length < 1 Then
                blnDataErr = True
                strMSG = "The 'Order Number' value is invalid."
            End If
            If intDefCompNumber = 0 Then
                If blnDataErr Then
                    strMSG &= " And "
                End If
                blnDataErr = True
                strMSG &= "The 'Default Company' value is invalid."
            End If
            If blnDataErr Then Return False
            Dim oQuery As New Ngl.Core.Data.Query(Me.DBServer, Me.Database)
            'Get the Next Pro Number 
            Dim strProBase As String = oQuery.getScalarValue(DBCon, "Exec spGetNextProBase", 3)
            Dim strProAbrev As String = oQuery.getScalarValue(DBCon, "Exec spGetProAbrev 0," & intDefCompNumber.ToString, 3)
            Dim strNewPro As String = strProAbrev & strProBase
            If String.IsNullOrEmpty(strNewPro) OrElse strNewPro.Trim.Length < 1 Then
                strMSG &= "The 'New Pro Number' value is invalid."
                Return False
            End If
            'Update the status of the record to received.
            Dim oCmd As New System.Data.SqlClient.SqlCommand
            oCmd.Parameters.AddWithValue("@NewPRONumber", strNewPro)
            oCmd.Parameters.AddWithValue("@PROBase", strProBase)
            oCmd.Parameters.AddWithValue("@pohdrOrderNumber", strOrderNumber)
            oCmd.Parameters.AddWithValue("@POHDROrderSequence", intOrderSequence)
            oCmd.Parameters.AddWithValue("@POHdrDefCustNumber", intDefCompNumber)
            blnRet = oQuery.execNGLStoredProcedure(DBCon, oCmd, "dbo.WriteNewBooking", 3)
        Catch ex As Ngl.Core.DatabaseRetryExceededException
            strMSG = "Write New Booking Data Failure! Failed to execute dbo.WriteNewBooking stored procedure without success: " & ex.Message
            Return False
        Catch ex As Ngl.Core.DatabaseLogInException
            strMSG = "Write New Booking Data Failure! Database login failure: " & ex.Message
            Return False
        Catch ex As Ngl.Core.DatabaseInvalidException
            strMSG = "Write New Booking Data Failure! Database access failure : " & ex.Message
            Return False
        Catch ex As Exception
            Throw
            Return False
        End Try

        Return blnRet

    End Function


    Private Function runProcessFinalizedData(ByVal strOrderNumber As String, _
                                                    ByVal intOrderSequence As Integer, _
                                                    ByVal intDefCompNumber As Integer, _
                                                    ByVal strVendorNumber As String, _
                                                    ByRef strMSG As String) As Boolean


        Dim blnDataErr As Boolean = False
        Dim blnRet As Boolean = False
        'Dim oCon As New System.Data.SqlClient.SqlConnection

        Try
            'Check Data
            If String.IsNullOrEmpty(strOrderNumber) OrElse strOrderNumber.Trim.Length < 1 Then
                blnDataErr = True
                strMSG = "The 'Order Number' value is invalid."
            End If
            If intDefCompNumber = 0 Then
                If blnDataErr Then
                    strMSG &= " And "
                End If
                blnDataErr = True
                strMSG &= "The 'Default Company' value is invalid."
            End If
            If String.IsNullOrEmpty(strVendorNumber) OrElse strVendorNumber.Trim.Length < 1 Then
                If blnDataErr Then
                    strMSG &= " And "
                End If
                blnDataErr = True
                strMSG &= "The 'Lane Number' value is invalid."
            End If
            If blnDataErr Then Return False

            Dim oQuery As New Ngl.Core.Data.Query(Me.DBServer, Me.Database)
            'Update the existing PO Data.
            Dim oCmd As New System.Data.SqlClient.SqlCommand
            oCmd.Parameters.AddWithValue("@POHdrOrderNumber", strOrderNumber)
            oCmd.Parameters.AddWithValue("@POHDROrderSequence", intOrderSequence)
            oCmd.Parameters.AddWithValue("@POHdrDefCustNumber", intDefCompNumber)
            oCmd.Parameters.AddWithValue("@POHDRvendor", strVendorNumber)
            Log("Calling Silent Tender Stored Procedure; dbo.spSilentTenderFinalized for Order Number " & strOrderNumber)
            blnRet = oQuery.execNGLStoredProcedure(DBCon, oCmd, "dbo.spSilentTenderFinalized", 3)
        Catch ex As Ngl.Core.DatabaseRetryExceededException
            strMSG = "Silent Tender Finalized Load Failure! Failed to execute dbo.spSilentTenderFinalized stored procedure without success: " & ex.Message
            Return False
        Catch ex As Ngl.Core.DatabaseLogInException
            strMSG = "Silent Tender Finalized Load Failure! Database login failure: " & ex.Message
            Return False
        Catch ex As Ngl.Core.DatabaseInvalidException
            strMSG = "USilent Tender Finalized Load Failure! Database access failure : " & ex.Message
            Return False
        Catch ex As Exception
            Throw
            Return False
        End Try

        Return blnRet

    End Function


    Private Function runRemoveDeletedWithData(ByVal strOrderNumber As String, _
                                                    ByVal intOrderSequence As Integer, _
                                                    ByVal intDefCompNumber As Integer, _
                                                    ByRef strMSG As String) As Boolean


        Dim blnDataErr As Boolean = False
        Dim blnRet As Boolean = False
        'Dim oCon As New System.Data.SqlClient.SqlConnection

        Try
            'Check Data
            If String.IsNullOrEmpty(strOrderNumber) OrElse strOrderNumber.Trim.Length < 1 Then
                blnDataErr = True
                strMSG = "The 'Order Number' value is invalid."
            End If
            If intDefCompNumber = 0 Then
                If blnDataErr Then
                    strMSG &= " And "
                End If
                blnDataErr = True
                strMSG &= "The 'Default Company' value is invalid."
            End If
            If blnDataErr Then Return False

            Dim oQuery As New Ngl.Core.Data.Query(Me.DBServer, Me.Database)
            'Update the existing PO Data.
            Dim oCmd As New System.Data.SqlClient.SqlCommand
            oCmd.Parameters.AddWithValue("@POHdrOrderNumber", strOrderNumber)
            oCmd.Parameters.AddWithValue("@POHDROrderSequence", intOrderSequence)
            oCmd.Parameters.AddWithValue("@POHDRDefaultCustomer", intDefCompNumber)
            blnRet = oQuery.execNGLStoredProcedure(DBCon, oCmd, "dbo.spRemoveDeletedPOByComp", 3)
        Catch ex As Ngl.Core.DatabaseRetryExceededException
            strMSG = "Remove Deleted Booking Data Failure! Failed to execute dbo.spRemoveDeletedPOByComp stored procedure without success: " & ex.Message
            Return False
        Catch ex As Ngl.Core.DatabaseLogInException
            strMSG = "Remove Deleted Booking Data Failure! Database login failure: " & ex.Message
            Return False
        Catch ex As Ngl.Core.DatabaseInvalidException
            strMSG = "Remove Deleted Booking Data Failure! Database access failure : " & ex.Message
            Return False
        Catch ex As Exception
            Throw
            Return False
        End Try

        Return blnRet

    End Function


    Private Function runDeleteOrderWithData(ByVal strBookProNumber As String, _
                                                    ByVal strOrderNumber As String, _
                                                    ByVal intOrderSequence As Integer, _
                                                    ByVal intDefCompNumber As Integer, _
                                                    ByRef strMSG As String) As Boolean


        Dim blnDataErr As Boolean = False
        Dim blnRet As Boolean = False
        'Dim oCon As New System.Data.SqlClient.SqlConnection

        Try
            'Check Data
            If String.IsNullOrEmpty(strBookProNumber) OrElse strBookProNumber.Trim.Length < 1 Then
                blnDataErr = True
                strMSG = "The 'PRO Number' value is invalid."
            End If

            If String.IsNullOrEmpty(strOrderNumber) OrElse strOrderNumber.Trim.Length < 1 Then
                If blnDataErr Then
                    strMSG &= " And "
                End If
                blnDataErr = True
                strMSG &= "The 'Order Number' value is invalid."
            End If
            If intDefCompNumber = 0 Then
                If blnDataErr Then
                    strMSG &= " And "
                End If
                blnDataErr = True
                strMSG &= "The 'Default Company' value is invalid."
            End If
            If blnDataErr Then Return False

            Dim oQuery As New Ngl.Core.Data.Query(Me.DBServer, Me.Database)
            'Update the existing PO Data.
            Dim oCmd As New System.Data.SqlClient.SqlCommand
            oCmd.Parameters.AddWithValue("@BookProNumber", strBookProNumber)
            If oQuery.execNGLStoredProcedure(DBCon, oCmd, "dbo.spDeleteBookingByPro", 3) Then
                blnRet = runRemoveDeletedWithData(strOrderNumber, intOrderSequence, intDefCompNumber, strMSG)
            Else
                blnRet = False
            End If
        Catch ex As Ngl.Core.DatabaseRetryExceededException
            strMSG = "Delete Booking Data Failure! Failed to execute dbo.spDeleteBookingByPro stored procedure without success: " & ex.Message
            Return False
        Catch ex As Ngl.Core.DatabaseLogInException
            strMSG = "Delete Booking Data Failure! Database login failure: " & ex.Message
            Return False
        Catch ex As Ngl.Core.DatabaseInvalidException
            strMSG = "Delete Booking Data Failure! Database access failure : " & ex.Message
            Return False
        Catch ex As Exception
            Throw
            Return False
        End Try

        Return blnRet

    End Function

    Private Function runUpdatePOModificationWithData(ByVal strOrderNumber As String, _
                                                    ByVal intOrderSequence As Integer, _
                                                    ByVal intDefCompNumber As Integer, _
                                                    ByVal strVendorNumber As String, _
                                                    ByRef strMSG As String) As Boolean


        Dim blnDataErr As Boolean = False
        Dim blnRet As Boolean = False
        'Dim oCon As New System.Data.SqlClient.SqlConnection

        Try
            'Check Data
            If String.IsNullOrEmpty(strOrderNumber) OrElse strOrderNumber.Trim.Length < 1 Then
                blnDataErr = True
                strMSG = "The 'Order Number' value is invalid."
            End If
            If intDefCompNumber = 0 Then
                If blnDataErr Then
                    strMSG &= " And "
                End If
                blnDataErr = True
                strMSG &= "The 'Default Company' value is invalid."
            End If
            If String.IsNullOrEmpty(strVendorNumber) OrElse strVendorNumber.Trim.Length < 1 Then
                If blnDataErr Then
                    strMSG &= " And "
                End If
                blnDataErr = True
                strMSG &= "The 'Lane Number' value is invalid."
            End If
            If blnDataErr Then Return False

            Dim oQuery As New Ngl.Core.Data.Query(Me.DBServer, Me.Database)
            'Update the existing PO Data.
            Dim oCmd As New System.Data.SqlClient.SqlCommand
            oCmd.Parameters.AddWithValue("@POHdrOrderNumber", strOrderNumber)
            oCmd.Parameters.AddWithValue("@POHDROrderSequence", intOrderSequence)
            oCmd.Parameters.AddWithValue("@POHdrDefCustNumber", intDefCompNumber)
            oCmd.Parameters.AddWithValue("@POHDRvendor", strVendorNumber)
            blnRet = oQuery.execNGLStoredProcedure(DBCon, oCmd, "dbo.spUpdateBookingRecord", 3)
        Catch ex As Ngl.Core.DatabaseRetryExceededException
            strMSG = "Update PO Modifications Data Failure! Failed to execute dbo.spUpdateBookingRecord stored procedure without success: " & ex.Message
            Return False
        Catch ex As Ngl.Core.DatabaseLogInException
            strMSG = "Update PO Modifications Data Failure! Database login failure: " & ex.Message
            Return False
        Catch ex As Ngl.Core.DatabaseInvalidException
            strMSG = "Update PO Modifications Data Failure! Database access failure : " & ex.Message
            Return False
        Catch ex As Exception
            Throw
            Return False
        End Try

        Return blnRet

    End Function

    'Private Function GetSystemAlerts(ByVal MaxReturn As Integer) As DTO.tblSystemError()

    '    Using db As New NData.NGLMASSYSDataContext(ConnectionString)
    '        Try
    '            'if there are no null values to deal with we can build the array directly
    '            Dim Errors() As DTO.tblSystemError = ( _
    '                From t In db.tblSystemErrors _
    '                Where t.ErrorAlertSent = 0
    '                Order By t.CurrentDate Descending _
    '                Select New DTO.tblSystemError With {.ErrID = t.ErrID, _
    '                                                    .Message = t.Message, _
    '                                                    .Record = t.Record, _
    '                                                    .CurrentUser = t.CurrentUser, _
    '                                                    .CurrentDate = t.CurrentDate, _
    '                                                    .ErrorNumber = t.ErrorNumber, _
    '                                                    .ErrorSeverity = t.ErrorSeverity, _
    '                                                    .ErrorState = t.ErrorState, _
    '                                                    .ErrorProcedure = t.ErrorProcedure, _
    '                                                    .ErrorLineNbr = t.ErrorLineNbr}).Take(MaxReturn).ToArray()
    '            Return Errors
    '        Catch ex As System.Data.SqlClient.SqlException
    '            Throw New System.ApplicationException(Me.Source.ToString & ex.Message, ex)
    '        Catch ex As InvalidOperationException
    '            Throw New System.ApplicationException("E_NoData", ex)
    '        Catch ex As Exception
    '            Throw
    '        End Try

    '        Return Nothing

    '    End Using

    'End Function

    'Private Function UpdateSystemAlert(ByVal ErrID As Int64) As Boolean
    '    Dim blnRet As Boolean = False
    '    Try
    '        Dim strSQL As String = "Update dbo.tblSystemErrors set ErrorAlertSent = 1, ErrorAlertSentDate = '" & Date.Now.ToString & "' Where ErrID = " & ErrID
    '        'Create a data connection 
    '        Dim oCon As New System.Data.SqlClient.SqlConnection
    '        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)

    '        Try

    '            If oQuery.executeSQLQuery(oCon, strSQL, 1) Then
    '                blnRet = True
    '            End If


    '        Catch ex As Ngl.Core.DatabaseRetryExceededException
    '            Me.LogException(Me.Source & ".UpdateSystemAlert: Status update failed to execute (retry exceeded)." & vbCrLf & strSQL, ex)
    '        Catch ex As System.Data.SqlClient.SqlException
    '            Me.LogException(Me.Source & ".UpdateSystemAlert: SQL Exception." & vbCrLf & strSQL, ex)
    '        Catch ex As Exception
    '            Throw
    '        Finally
    '            oQuery = Nothing
    '        End Try
    '    Catch ex As Exception
    '        Throw
    '    End Try
    '    Return blnRet
    'End Function

End Class
