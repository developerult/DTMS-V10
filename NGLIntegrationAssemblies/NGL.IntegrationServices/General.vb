Option Strict Off
Option Explicit On 
#Disable Warning BC40056 ' Namespace or type specified in the Imports 'ngl.FreightMaster.FMLib.dbUtilities' doesn't contain any public member or cannot be found. Make sure the namespace or the type is defined and contains at least one public member. Make sure the imported element name doesn't use any aliases.
Imports ngl.FreightMaster.FMLib.dbUtilities
#Enable Warning BC40056 ' Namespace or type specified in the Imports 'ngl.FreightMaster.FMLib.dbUtilities' doesn't contain any public member or cannot be found. Make sure the namespace or the type is defined and contains at least one public member. Make sure the imported element name doesn't use any aliases.

Public Class General
    Public Shared SysCompanyName, SysString As String
    Public Shared SysValue As Double
    Public Shared PassHelp, PassTitle As String
    Public Shared PassDate As Date
    Public Shared gdtDefault As Date
    Public Shared NextProNumberValue As Integer
    Public Shared NextCONSNumberValue As Integer
    Public Shared NextCONSNumberText As String
    Public Shared SearchBookControl, SearchClaimControl As Integer
    Public Shared SearchLanecontrol, SearchCarrierControl As Integer
    Public Shared SearchType As String
    Public Shared AuthDate As Date
    Public Shared gReportType As Short
    Public Shared DefaultCarrierNumber, DefaultCustomerNumber, NewProNumber As String
    Public Shared mstrCompFilterByCompNumber As String
    Public Shared mstrCompFilterByCompControl As String
    Public Shared mstrCurrentUser As String
    Public Shared mstrReportSecurity As String
    Public Shared mstrFormSecurity As String
    Public Shared mstrProcedureSecurity As String
    Public Shared glngSelectedTruck As Integer
    Public Shared glngMasterTruck As Integer
    Public Shared glngSelectedCarrier As Integer
    Public Shared glngBookControl As Integer
    Public Shared glngLaneControl As Integer
    Public Shared gstrSelectedCons As String
    Public Shared mlngEmailInvoiceCompNumber As Integer
    Public Shared mintBatchImportMaster As Short
    Public Shared glngSearchProControlResult As Integer
    Public Shared gstrPassword As String
    Public Shared glngSetParameterValue As Integer
    Public Shared gblnSilent As Boolean = True
    Public Shared gstrLastErrorMessage As String = ""
    Public Shared Sel_Time As Object
    Public Shared gblnIsConsole As Boolean = False
    'debug property
    Public Shared gblnDebug As Boolean = False

    'general constants
    Public Const gcstrVersion As String = "4.2"
    Public Const gcstrLastMod As String = "05/23/2006" 'Begin development on v 4.2 of FreightMaster.Net

    'message box constants
    Public Const gcstrMessage1 As String = "You must save the current record before you can continue."
    Public Const gcstrMessage2 As String = "You should save the current record before you can continue."
    Public Const gcstrMessage3 As String = "Please select an existing record or complete the entry of a new record before using this option."
    Public Const gcstrNoReportDataCheckContact As String = "Sorry, nothing found to print." & vbCrLf & vbCrLf & "Please verify that the Carrier Contact Name on the Booking Load Maintenance Screen is correct.  Use the Carrier Contact Selection dialog box, the telephone button, to select a valid contact name."
    Public Const gcstrNoReportDataCheckParameters As String = "Sorry, nothing found to print." & vbCrLf & vbCrLf & "Please verify that all required parameters are completed correctly and that you have permission to view this information."


    'message box title constants
    Public Const gcstrTitle1 As String = "Save Current Record?"
    Public Const gcstrNoReportDataTitle As String = "Printing of the report is canceled!"

    'mouse pointer constants
    Public Const gcMouseDefault As Short = 0
    Public Const gcMouseNormal As Short = 1
    Public Const gcMouseText As Short = 3
    Public Const gcMouseVResize As Short = 7
    Public Const gcMouseHResize As Short = 9
    Public Const gcMouseBusy As Short = 11

    'Error Constants
    Public Const gclngErrorNumber1 As Short = 20001
    Public Const gcstrErrorDesc1 As String = "cannot locate database server."
    Public Const gclngErrorNumber2 As Short = 20002
    Public Const gcstrErrorDesc2 As String = "Null value not allowed."
    Public Const gclngErrorNumber3 As Short = 20003
    Public Const gcstrErrorDesc3 As String = "Invalid data type."
    Public Const gclngErrorNumber4 As Short = 20004
    Public Const gcstrErrorDesc4 As String = "Text data is too long."


    Public Shared gblnAuthorized As Boolean


    'Public Constants Used For Data Integration
    Public Const gcvdtDate As Short = 0
    Public Const gcvdtLongInt As Short = 1
    Public Const gcvdtSmallInt As Short = 2
    Public Const gcvdtString As Short = 3
    Public Const gcvdtFloat As Short = 4
    Public Const gcvdtTinyInt As Short = 5
    Public Const gcvdtBit As Short = 6
    Public Const gcvdtMoney As Short = 7
    Public Const gcvdtTime As Short = 8

    'Public Constants Used For Load and Carrier Cost Calculations
    Public Const gcOtherCost As Short = 0
    Public Const gcCarrierCost As Short = 1
    Public Const gcTotalCost As Short = 2
    Public Const gcBilledBFC As Short = 3
    Public Const gcLoadSavings As Short = 4
    Public Const gcCommCost As Short = 5
    Public Const gcGrossRevenue As Short = 6
    'Import/Export/Data Entry Types (Source)
    Public Const gcImportCarrier As Short = 0
    Public Const gcImportLane As Short = 1
    Public Const gcImportComp As Short = 2
    Public Const gcImportPayables As Short = 3
    Public Const gcImportSchedule As Short = 4
    Public Const gcimportFrtBill As Short = 5
    Public Const gcimportBook As Short = 6
    Public Const gcDataEntryFrtBill As Short = 0
    Public Const gcDataEntryFrtBillAudit As Short = 1
    Public Const gcExportAP As Short = 0
    Public Const gcHK As String = "3"
    Public Const gcFK As String = "2"
    Public Const gcPK As String = "1"
    Public Const gcNK As String = "0"

    Public Const GCFIELD_VALUE As Short = 0
    Public Const GCFIELD_NAME As Short = 1
    Public Const GCFIELD_DATATYPE As Short = 2
    Public Const GCFIELD_LENGTH As Short = 3
    Public Const GCFIELD_NULL As Short = 4
    Public Const GCFIELD_PK As Short = 5
    Public Const GCFIELD_FK_INDEX As Short = 6
    Public Const GCFIELD_PARENT_FIELD As Short = 7
    Public Const GCFIELD_FIELD_USE As Short = 8

    Public Enum ImportFileTypes As Integer
        ietCarrier = 0
        ietLane
        ietComp
        ietPayables
        ietSchedule
        ietFrtBill
    End Enum

    Public Enum ExportFileTypes
        ietAP = 0
    End Enum

    Public Enum DataEntryFileTypes
        ietAPFrtBillDE = 0
        ietAPFrtBillAuditDE
    End Enum



    Public Enum ValidateDataType
        vdtDate
        vdtLongInt
        vdtSmallInt
        vdtString
        vdtFloat
        vdtTinyInt
        vdtBit
        vdtMoney
        vdtTime
    End Enum

    Public Enum OptimizerTypes
        optTruckStops
        optPCMiler
        optFaxServer
        optany
    End Enum

    Public Shared Function nz(ByVal objADODBField As ADODB.Field, ByVal defaultvalue As Object) As Object


        If objADODBField Is Nothing Or IsDBNull(objADODBField.Value) Or objADODBField.Value.ToString.Trim.Length < 1 Then
            Return defaultvalue
        Else
            Return objADODBField.Value
        End If

    End Function


    Public Shared Function nz(ByVal strVal As String, ByVal strDefault As String) As String

        If Len(Trim(strVal & " ")) < 1 Then
            nz = strDefault
        Else

            nz = strVal & ""
        End If
    End Function

    Public Shared Function nz(ByVal objADODBField As ADODB.Field, ByVal strDefault As String) As String

        If IsDBNull(objADODBField.Value) Or Len(Trim(objADODBField.Value & " ")) < 1 Then
            Return strDefault
        Else

            Return objADODBField.Value
        End If

    End Function
  
    Public Shared Function formatErrMsg(ByVal lngErrNumber As Integer, ByVal strErrMsg As String, ByVal strSource As String) As String

        On Error Resume Next

        formatErrMsg = "Warning! An error has occurred." & vbCrLf & "Error # " & lngErrNumber & vbCrLf & strErrMsg & vbCrLf & vbCrLf & "Source: " & strSource



    End Function

    Public Shared Function FormatErrMsg(ByVal oException As Exception, ByVal strSource As String) As String
        Dim strMsg As String = ""
        Try
            strMsg = "Error:"
        Catch ex As Exception

        End Try


        Dim outerDetail As String = ""
        Try
            outerDetail = oException.ToString
        Catch ex As Exception

        End Try

        Dim inner As Exception = Nothing
        Try
            inner = oException.InnerException
        Catch ex As Exception

        End Try

        Dim innerDetail As String = ""
        If Not inner Is Nothing Then
            Try
                innerDetail = inner.ToString
            Catch ex As Exception

            End Try
        End If

        Try
            If outerDetail.Trim.Length > 0 Then
                strMsg &= vbCrLf & outerDetail & vbCrLf
            End If
            If innerDetail.Trim.Length > 0 Then
                strMsg &= vbCrLf & innerDetail & vbCrLf
            End If
        Catch ex As Exception

        End Try
        strMsg &= vbCrLf & "Source: " & strSource & vbCrLf

        Return strMsg

    End Function



    Public Shared Function validateSQLValue(ByRef objADOField As ADODB.Field, ByVal enumDataType As ValidateDataType, Optional ByVal strSource As String = "General", Optional ByVal strErrMsg As String = "", Optional ByVal blnAllowNull As Boolean = False, Optional ByVal intLength As Short = 1) As String

        Dim lngErrorNumber As Integer
        Dim strErrdesc As String
        Dim lngval As Integer
        Dim intVal As Short
        Dim dtVal As Date
        Dim dblVal As Double
        Dim curVal As Decimal
        Dim strReturn As String

        On Error GoTo ErrorHandler

        'set default return value to empty string
        validateSQLValue = "''"

        Dim stryear As String
        Dim strmonth As String
        Dim strday As String
        Dim strhour As String
        Dim strminute As String
        If enumDataType = ValidateDataType.vdtDate Then
            'test for date value
            If IsDBNull(objADOField.Value) Or Val(nz(objADOField, "0")) = 0 Then
                If blnAllowNull Then
                    validateSQLValue = "NULL"
                    GoTo CleanExit
                Else
                    lngErrorNumber = gclngErrorNumber2
                    strErrdesc = gcstrErrorDesc2
                    GoTo RaiseError
                End If
            ElseIf IsDate(objADOField.Value) Then
                dtVal = objADOField.Value
                validateSQLValue = "'" & dtVal & "'"
                GoTo CleanExit
            Else
                If InStr(1, objADOField.Value, "-") Or InStr(1, objADOField.Value, "\") Then
                    lngErrorNumber = gclngErrorNumber3
                    strErrdesc = gcstrErrorDesc3
                    GoTo RaiseError
                Else
                    stryear = Right(objADOField.Value, 4)
                    strReturn = Left(objADOField.Value, Len(objADOField.Value) - 4)
                    strday = Right(strReturn, 2)
                    strmonth = Left(strReturn, Len(strReturn) - 2)
                    'strReturn = VB6.Format(strmonth & "-" & strday & "-" & stryear, "short date")
                    On Error Resume Next
                    strReturn = DateTime.ParseExact(strmonth & "/" & strday & "/" & stryear, "M/d/yyyy", Nothing).ToString  'default is short date

                    If IsDate(strReturn) Then
                        dtVal = strReturn
                        validateSQLValue = "'" & dtVal & "'"
                    Else
                        lngErrorNumber = gclngErrorNumber3
                        strErrdesc = gcstrErrorDesc3
                        GoTo RaiseError
                    End If
                    On Error GoTo ErrorHandler
                End If
            End If
        ElseIf enumDataType = ValidateDataType.vdtTime Then
            'test for Time value
            If IsDBNull(objADOField.Value) Or Val(nz(objADOField, "0")) = 0 Then
                If blnAllowNull Then
                    validateSQLValue = "NULL"
                    GoTo CleanExit
                Else
                    lngErrorNumber = gclngErrorNumber2
                    strErrdesc = gcstrErrorDesc2
                    GoTo RaiseError
                End If
            ElseIf IsDate(objADOField.Value) Then
                dtVal = objADOField.Value
                validateSQLValue = "'" & dtVal & "'"
                GoTo CleanExit
            Else
                If InStr(1, objADOField.Value, "-") Or InStr(1, objADOField.Value, "\") Or InStr(1, objADOField.Value, ":") Then
                    lngErrorNumber = gclngErrorNumber3
                    strErrdesc = gcstrErrorDesc3
                    GoTo RaiseError
                Else
                    strminute = Right(objADOField.Value, 2)
                    strhour = Left(objADOField.Value, 2)
                    'strReturn = VB6.Format(strhour & ":" & strminute, "short time")
                    On Error Resume Next
                    strReturn = DateTime.Parse("1/1/2000 " & strhour & ":" & strminute).ToShortTimeString()
                    If IsDate(strReturn) Then
                        dtVal = strReturn
                        validateSQLValue = "'" & dtVal & "'"
                    Else
                        lngErrorNumber = gclngErrorNumber3
                        strErrdesc = gcstrErrorDesc3
                        GoTo RaiseError
                    End If
                    On Error GoTo ErrorHandler
                End If
            End If
        ElseIf enumDataType = ValidateDataType.vdtFloat Then
            'test for float value
            If IsDBNull(objADOField.Value) Then
                If blnAllowNull Then
                    validateSQLValue = "NULL"
                    GoTo CleanExit
                Else
                    lngErrorNumber = gclngErrorNumber2
                    strErrdesc = gcstrErrorDesc2
                    GoTo RaiseError
                End If
            ElseIf IsNumeric(objADOField.Value) Then
                dblVal = objADOField.Value
                validateSQLValue = CStr(dblVal)
                GoTo CleanExit
            Else
                lngErrorNumber = gclngErrorNumber3
                strErrdesc = gcstrErrorDesc3
                GoTo RaiseError
            End If
        ElseIf enumDataType = ValidateDataType.vdtMoney Then
            'test for currency value
            If IsDBNull(objADOField.Value) Then
                If blnAllowNull Then
                    validateSQLValue = "NULL"
                    GoTo CleanExit
                Else
                    lngErrorNumber = gclngErrorNumber2
                    strErrdesc = gcstrErrorDesc2
                    GoTo RaiseError
                End If
            ElseIf IsNumeric(objADOField.Value) Then
                curVal = objADOField.Value
                validateSQLValue = CStr(curVal)
                GoTo CleanExit
            Else
                lngErrorNumber = gclngErrorNumber3
                strErrdesc = gcstrErrorDesc3
                GoTo RaiseError
            End If
        ElseIf enumDataType = ValidateDataType.vdtLongInt Then
            'test for long integer value
            If IsDBNull(objADOField.Value) Then
                If blnAllowNull Then
                    validateSQLValue = "NULL"
                    GoTo CleanExit
                Else
                    lngErrorNumber = gclngErrorNumber2
                    strErrdesc = gcstrErrorDesc2
                    GoTo RaiseError
                End If
            ElseIf IsNumeric(objADOField.Value) Then
                lngval = objADOField.Value
                validateSQLValue = CStr(lngval)
                GoTo CleanExit
            Else
                lngErrorNumber = gclngErrorNumber3
                strErrdesc = gcstrErrorDesc3
                GoTo RaiseError
            End If
        ElseIf enumDataType = ValidateDataType.vdtSmallInt Then
            'test for small integer value
            If IsDBNull(objADOField.Value) Then
                If blnAllowNull Then
                    validateSQLValue = "NULL"
                    GoTo CleanExit
                Else
                    lngErrorNumber = gclngErrorNumber2
                    strErrdesc = gcstrErrorDesc2
                    GoTo RaiseError
                End If
            ElseIf IsNumeric(objADOField.Value) Then
                intVal = objADOField.Value
                validateSQLValue = CStr(intVal)
                GoTo CleanExit
            Else
                lngErrorNumber = gclngErrorNumber3
                strErrdesc = gcstrErrorDesc3
                GoTo RaiseError
            End If
        ElseIf enumDataType = ValidateDataType.vdtTinyInt Then
            'test for tiny integer value
            If IsDBNull(objADOField.Value) Then
                If blnAllowNull Then
                    validateSQLValue = "NULL"
                    GoTo CleanExit
                Else
                    lngErrorNumber = gclngErrorNumber2
                    strErrdesc = gcstrErrorDesc2
                    GoTo RaiseError
                End If
            ElseIf IsNumeric(objADOField.Value) Then
                intVal = objADOField.Value
                If intVal < 0 Or intVal > 255 Then
                    lngErrorNumber = gclngErrorNumber3
                    strErrdesc = gcstrErrorDesc3
                    GoTo RaiseError
                End If
                validateSQLValue = CStr(intVal)
                GoTo CleanExit
            Else
                lngErrorNumber = gclngErrorNumber3
                strErrdesc = gcstrErrorDesc3
                GoTo RaiseError
            End If
        ElseIf enumDataType = ValidateDataType.vdtBit Then
            'test for Bit integer value
            If IsDBNull(objADOField.Value) Then
                If blnAllowNull Then
                    validateSQLValue = "NULL"
                    GoTo CleanExit
                Else
                    lngErrorNumber = gclngErrorNumber2
                    strErrdesc = gcstrErrorDesc2
                    GoTo RaiseError
                End If
            ElseIf IsNumeric(objADOField.Value) Then
                intVal = objADOField.Value
                Select Case intVal
                    Case -1
                        intVal = 1
                    Case 0, 1
                        intVal = intVal
                    Case Else
                        lngErrorNumber = gclngErrorNumber3
                        strErrdesc = gcstrErrorDesc3
                        GoTo RaiseError
                End Select
                validateSQLValue = CStr(intVal)
                GoTo CleanExit
            Else
                lngErrorNumber = gclngErrorNumber3
                strErrdesc = gcstrErrorDesc3
                GoTo RaiseError
            End If
        Else
            'we test for a string (text) value
            If IsDBNull(objADOField.Value) Then
                If blnAllowNull Then
                    validateSQLValue = "NULL"
                    GoTo CleanExit
                Else
                    lngErrorNumber = gclngErrorNumber2
                    strErrdesc = gcstrErrorDesc2
                    GoTo RaiseError
                End If
            ElseIf Len(objADOField.Value) <= intLength Then

                validateSQLValue = "'" & padQuotes(objADOField.Value) & "'"
                GoTo CleanExit
            Else
                lngErrorNumber = gclngErrorNumber4
                strErrdesc = gcstrErrorDesc4
                GoTo RaiseError
            End If

        End If



CleanExit:
        On Error Resume Next
        Exit Function

ErrorHandler:
        If Err.Number = 13 Then
            'type conversion error
            lngErrorNumber = gclngErrorNumber3
            strErrdesc = gcstrErrorDesc3
        Else
            lngErrorNumber = Err.Number
            strSource = Err.Source & "; validateSQLValue ;" & strSource
            strErrdesc = Err.Description
        End If
        Resume RaiseError
RaiseError:
        On Error GoTo 0
        Err.Raise(lngErrorNumber, strSource, strErrdesc & "  " & strErrMsg)


    End Function

    Public Shared Function validateObjectValue(ByRef objADOField As ADODB.Field, ByVal enumDataType As ValidateDataType, Optional ByVal strSource As String = "General", Optional ByVal strErrMsg As String = "", Optional ByVal blnAllowNull As Boolean = False, Optional ByVal intLength As Short = 1) As String

        Dim lngErrorNumber As Integer
        Dim strErrdesc As String
        Dim lngval As Integer
        Dim intVal As Short
        Dim dtVal As Date
        Dim dblVal As Double
        Dim curVal As Decimal
        Dim strReturn As String

        On Error GoTo ErrorHandler

        'set default return value to empty string
        validateObjectValue = ""

        Dim stryear As String
        Dim strmonth As String
        Dim strday As String
        Dim strhour As String
        Dim strminute As String
        If enumDataType = ValidateDataType.vdtDate Then
            'test for date value
            If IsDBNull(objADOField.Value) Or Val(nz(objADOField, "0")) = 0 Then
                If blnAllowNull Then
                    validateObjectValue = ""
                    GoTo CleanExit
                Else
                    lngErrorNumber = gclngErrorNumber2
                    strErrdesc = gcstrErrorDesc2
                    GoTo RaiseError
                End If
            ElseIf IsDate(objADOField.Value) Then
                dtVal = objADOField.Value
                validateObjectValue = dtVal
                GoTo CleanExit
            Else
                If InStr(1, objADOField.Value, "-") Or InStr(1, objADOField.Value, "\") Then
                    lngErrorNumber = gclngErrorNumber3
                    strErrdesc = gcstrErrorDesc3
                    GoTo RaiseError
                Else
                    stryear = Right(objADOField.Value, 4)
                    strReturn = Left(objADOField.Value, Len(objADOField.Value) - 4)
                    strday = Right(strReturn, 2)
                    strmonth = Left(strReturn, Len(strReturn) - 2)
                    'strReturn = VB6.Format(strmonth & "-" & strday & "-" & stryear, "short date")
                    On Error Resume Next
                    strReturn = DateTime.ParseExact(strmonth & "/" & strday & "/" & stryear, "M/d/yyyy", Nothing).ToString  'default is short date

                    If IsDate(strReturn) Then
                        dtVal = strReturn
                        validateObjectValue = dtVal
                    Else
                        lngErrorNumber = gclngErrorNumber3
                        strErrdesc = gcstrErrorDesc3
                        GoTo RaiseError
                    End If
                    On Error GoTo ErrorHandler
                End If
            End If
        ElseIf enumDataType = ValidateDataType.vdtTime Then
            'test for Time value
            If IsDBNull(objADOField.Value) Or Val(nz(objADOField, "0")) = 0 Then
                If blnAllowNull Then
                    validateObjectValue = ""
                    GoTo CleanExit
                Else
                    lngErrorNumber = gclngErrorNumber2
                    strErrdesc = gcstrErrorDesc2
                    GoTo RaiseError
                End If
            ElseIf IsDate(objADOField.Value) Then
                dtVal = objADOField.Value
                validateObjectValue = dtVal
                GoTo CleanExit
            Else
                If InStr(1, objADOField.Value, "-") Or InStr(1, objADOField.Value, "\") Or InStr(1, objADOField.Value, ":") Then
                    lngErrorNumber = gclngErrorNumber3
                    strErrdesc = gcstrErrorDesc3
                    GoTo RaiseError
                Else
                    strminute = Right(objADOField.Value, 2)
                    strhour = Left(objADOField.Value, 2)
                    'strReturn = VB6.Format(strhour & ":" & strminute, "short time")
                    On Error Resume Next
                    strReturn = DateTime.Parse("1/1/2000 " & strhour & ":" & strminute).ToShortTimeString()
                    If IsDate(strReturn) Then
                        dtVal = strReturn
                        validateObjectValue = dtVal
                    Else
                        lngErrorNumber = gclngErrorNumber3
                        strErrdesc = gcstrErrorDesc3
                        GoTo RaiseError
                    End If
                    On Error GoTo ErrorHandler
                End If
            End If
        ElseIf enumDataType = ValidateDataType.vdtFloat Then
            'test for float value
            If IsDBNull(objADOField.Value) Then
                If blnAllowNull Then
                    validateObjectValue = ""
                    GoTo CleanExit
                Else
                    lngErrorNumber = gclngErrorNumber2
                    strErrdesc = gcstrErrorDesc2
                    GoTo RaiseError
                End If
            ElseIf IsNumeric(objADOField.Value) Then
                dblVal = objADOField.Value
                validateObjectValue = CStr(dblVal)
                GoTo CleanExit
            Else
                lngErrorNumber = gclngErrorNumber3
                strErrdesc = gcstrErrorDesc3
                GoTo RaiseError
            End If
        ElseIf enumDataType = ValidateDataType.vdtMoney Then
            'test for currency value
            If IsDBNull(objADOField.Value) Then
                If blnAllowNull Then
                    validateObjectValue = ""
                    GoTo CleanExit
                Else
                    lngErrorNumber = gclngErrorNumber2
                    strErrdesc = gcstrErrorDesc2
                    GoTo RaiseError
                End If
            ElseIf IsNumeric(objADOField.Value) Then
                curVal = objADOField.Value
                validateObjectValue = CStr(curVal)
                GoTo CleanExit
            Else
                lngErrorNumber = gclngErrorNumber3
                strErrdesc = gcstrErrorDesc3
                GoTo RaiseError
            End If
        ElseIf enumDataType = ValidateDataType.vdtLongInt Then
            'test for long integer value
            If IsDBNull(objADOField.Value) Then
                If blnAllowNull Then
                    validateObjectValue = ""
                    GoTo CleanExit
                Else
                    lngErrorNumber = gclngErrorNumber2
                    strErrdesc = gcstrErrorDesc2
                    GoTo RaiseError
                End If
            ElseIf IsNumeric(objADOField.Value) Then
                lngval = objADOField.Value
                validateObjectValue = CStr(lngval)
                GoTo CleanExit
            Else
                lngErrorNumber = gclngErrorNumber3
                strErrdesc = gcstrErrorDesc3
                GoTo RaiseError
            End If
        ElseIf enumDataType = ValidateDataType.vdtSmallInt Then
            'test for small integer value
            If IsDBNull(objADOField.Value) Then
                If blnAllowNull Then
                    validateObjectValue = ""
                    GoTo CleanExit
                Else
                    lngErrorNumber = gclngErrorNumber2
                    strErrdesc = gcstrErrorDesc2
                    GoTo RaiseError
                End If
            ElseIf IsNumeric(objADOField.Value) Then
                intVal = objADOField.Value
                validateObjectValue = CStr(intVal)
                GoTo CleanExit
            Else
                lngErrorNumber = gclngErrorNumber3
                strErrdesc = gcstrErrorDesc3
                GoTo RaiseError
            End If
        ElseIf enumDataType = ValidateDataType.vdtTinyInt Then
            'test for tiny integer value
            If IsDBNull(objADOField.Value) Then
                If blnAllowNull Then
                    validateObjectValue = ""
                    GoTo CleanExit
                Else
                    lngErrorNumber = gclngErrorNumber2
                    strErrdesc = gcstrErrorDesc2
                    GoTo RaiseError
                End If
            ElseIf IsNumeric(objADOField.Value) Then
                intVal = objADOField.Value
                If intVal < 0 Or intVal > 255 Then
                    lngErrorNumber = gclngErrorNumber3
                    strErrdesc = gcstrErrorDesc3
                    GoTo RaiseError
                End If
                validateObjectValue = CStr(intVal)
                GoTo CleanExit
            Else
                lngErrorNumber = gclngErrorNumber3
                strErrdesc = gcstrErrorDesc3
                GoTo RaiseError
            End If
        ElseIf enumDataType = ValidateDataType.vdtBit Then
            'test for Bit integer value
            If IsDBNull(objADOField.Value) Then
                If blnAllowNull Then
                    validateObjectValue = ""
                    GoTo CleanExit
                Else
                    lngErrorNumber = gclngErrorNumber2
                    strErrdesc = gcstrErrorDesc2
                    GoTo RaiseError
                End If
            ElseIf IsNumeric(objADOField.Value) Then
                intVal = objADOField.Value
                Select Case intVal
                    Case -1
                        intVal = 1
                    Case 0, 1
                        intVal = intVal
                    Case Else
                        lngErrorNumber = gclngErrorNumber3
                        strErrdesc = gcstrErrorDesc3
                        GoTo RaiseError
                End Select
                validateObjectValue = CStr(intVal)
                GoTo CleanExit
            Else
                lngErrorNumber = gclngErrorNumber3
                strErrdesc = gcstrErrorDesc3
                GoTo RaiseError
            End If
        Else
            'we test for a string (text) value
            If IsDBNull(objADOField.Value) Then
                If blnAllowNull Then
                    validateObjectValue = ""
                    GoTo CleanExit
                Else
                    lngErrorNumber = gclngErrorNumber2
                    strErrdesc = gcstrErrorDesc2
                    GoTo RaiseError
                End If
            ElseIf Len(objADOField.Value) <= intLength Then

                validateObjectValue = objADOField.Value
                GoTo CleanExit
            Else
                lngErrorNumber = gclngErrorNumber4
                strErrdesc = gcstrErrorDesc4
                GoTo RaiseError
            End If

        End If



CleanExit:
        On Error Resume Next
        Exit Function

ErrorHandler:
        If Err.Number = 13 Then
            'type conversion error
            lngErrorNumber = gclngErrorNumber3
            strErrdesc = gcstrErrorDesc3
        Else
            lngErrorNumber = Err.Number
            strSource = Err.Source & "; validateObjectValue ;" & strSource
            strErrdesc = Err.Description
        End If
        Resume RaiseError
RaiseError:
        On Error GoTo 0
        Err.Raise(lngErrorNumber, strSource, strErrdesc & "  " & strErrMsg)


    End Function


    Public Shared Function padSpaces(ByVal strVal As String) As String


        On Error Resume Next

        If IsDBNull(strVal) Or Trim(strVal) = "" Then
            padSpaces = "''"
        Else
            padSpaces = strVal
        End If

    End Function


    Public Shared Function padQuotes(ByVal strVal As String) As String

        ' This function pads an extra single quote in strings containing
        'quotes for REM proper SQL searching.

        Dim strBodyBuild As String
        Dim strBody As String
        Dim lngLength As Integer
        Dim i As Short

        On Error Resume Next

        strBodyBuild = ""
        strBody = strVal
        'test for single quotes
        If InStr(1, strBody, "'") Then
            lngLength = Len(strBody)
            For i = 1 To lngLength
                strBodyBuild = strBodyBuild & Mid(strBody, i, 1)
                If Mid(strBody, i, 1) = Chr(39) Then
                    strBodyBuild = strBodyBuild & Mid(strBody, i, 1)
                End If
            Next
            strBody = strBodyBuild
        Else
            strBody = strVal
        End If

        padQuotes = strBody

    End Function
      


    Public Shared Function zipClean(ByVal strCode As String) As String

        Dim intPos As Short
        strCode = Trim(stripQuotes(strCode))
        intPos = InStr(1, strCode, "-")
        If intPos Then
            strCode = Left(strCode, intPos - 1)
            If Len(strCode) > 5 Then
                strCode = Left(strCode, 5)
            End If
        End If

        zipClean = strCode

    End Function


    Public Shared Function stripQuotes(ByVal strVal As String) As String

        Dim strNewVal As String = strVal

        On Error Resume Next

        If Left(strVal, 1) = "'" Then
            strNewVal = Mid(strVal, 2)
        Else
            strNewVal = strVal
        End If

        If Right(strNewVal, 1) = "'" Then
            strNewVal = Left(strNewVal, Len(strNewVal) - 1)
        End If

        Return strNewVal

    End Function

    Public Shared Function stripDblQuotes(ByVal strVal As String) As String

        Dim strNewVal As String = strVal

        On Error Resume Next

        If Left(strVal, 1) = Chr(34) Then
            strNewVal = Mid(strVal, 2)
        Else
            strNewVal = strVal
        End If

        If Right(strNewVal, 1) = Chr(34) Then
            strNewVal = Left(strNewVal, Len(strNewVal) - 1)
        End If

        Return strNewVal

    End Function





     
    Public Shared Function MoveFile(ByVal strOrigFile As String, ByVal strDestFile As String) As Boolean
        'Specify the directories you want to manipulate.
        Dim fi As System.IO.FileInfo = New System.IO.FileInfo(strOrigFile)
        Dim fi2 As System.IO.FileInfo = New System.IO.FileInfo(strDestFile)

        Try
            'Ensure that the target does not exist.
            fi2.Delete()
            'Copy the file.
            fi.CopyTo(strDestFile)
            fi.Delete()
            Return True

        Catch
            Return False
        End Try

    End Function


    Public Shared Function SendMail(ByVal strServer As String, ByVal strTo As String, ByVal strFrom As String, ByVal strBody As String, ByVal strSubject As String) As Boolean
        'Dim email As New System.Web.Mail.MailMessage
        ''Dim email As New System.Net.Mail.MailMessage
        'Try
        '    email.To = strTo
        '    email.From = strFrom
        '    email.Body = strBody
        '    email.Subject = strSubject
        '    email.BodyFormat = Web.Mail.MailFormat.Text
        '    System.Web.Mail.SmtpMail.SmtpServer = strServer
        '    System.Web.Mail.SmtpMail.Send(email)
        '    Return True
        'Catch ex As Exception
        '    Return False
        'End Try
#Disable Warning BC42353 ' Function 'SendMail' doesn't return a value on all code paths. Are you missing a 'Return' statement?
    End Function
#Enable Warning BC42353 ' Function 'SendMail' doesn't return a value on all code paths. Are you missing a 'Return' statement?
End Class