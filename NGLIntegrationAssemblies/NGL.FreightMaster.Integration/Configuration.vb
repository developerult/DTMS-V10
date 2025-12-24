
<Serializable()>
Public Class Configuration
#Region " Class Variables and Properties "

    'Constants
    Public Const gcHTMLNEWLINE As String = vbCrLf & "<br />"


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
    Public Const gcIgnore As String = "4"
    Public Const gcHK As String = "3"
    Public Const gcFK As String = "2"
    Public Const gcPK As String = "1"
    Public Const gcNK As String = "0"


    'Error Constants
    Public Const gclngErrorNumber1 As Short = 20001
    Public Const gcstrErrorDesc1 As String = "Cannot locate database server."
    Public Const gclngErrorNumber2 As Short = 20002
    Public Const gcstrErrorDesc2 As String = "Null value not allowed."
    Public Const gclngErrorNumber3 As Short = 20003
    Public Const gcstrErrorDesc3 As String = "Invalid data type."
    Public Const gclngErrorNumber4 As Short = 20004
    Public Const gcstrErrorDesc4 As String = "Text data is too long."
    Public Const gclngErrorNumber5 As Short = 20005
    Public Const gcstrErrorDesc5 As String = "Unexpected error."



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

    Public Enum ProcessDataReturnValues
        nglDataIntegrationComplete
        nglDataConnectionFailure
        nglDataValidationFailure
        nglDataIntegrationFailure
        nglDataIntegrationHadErrors
    End Enum

    Public Enum IntegrationTypes As Integer
        Carrier = 0
        Lane
        Company
        Payables
        Schedule
        LegacyFreightBillImport
        FreightBillImport
        Book
        PickList
        APExport
        FreightBillExport
        OpenPayables
        EDI214
        EDI204
        EDI990
        PalletType
        Hazmat
        EDI210 'Added by LVV 3/2/16 for v-7.0.5.1 EDI Migration
        OrderChangeValidation 'Added by RHR for v-7.0.6.105 on 11/02/2017
    End Enum

#End Region


#Region " Constructors "


#End Region


#Region " Methods "
    ''' <summary>
    ''' Returns the Text file name associated with the Integration Type
    ''' </summary>
    ''' <param name="enmVal"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-7.0.6.105 on 11/02/2017
    '''   added new file names based on new IntegrationTypes enums
    ''' </remarks>
    Public Shared Function getImportFileName(ByVal enmVal As IntegrationTypes) As String
        Select Case enmVal

            Case IntegrationTypes.Carrier
                Return "Carrier"
            Case IntegrationTypes.Lane
                Return "Lane"
            Case IntegrationTypes.Company
                Return "Company"
            Case IntegrationTypes.Payables
                Return "Payables"
            Case IntegrationTypes.Schedule
                Return "Schedule"
            Case IntegrationTypes.LegacyFreightBillImport
                Return "LegacyFreightBillImport"
            Case IntegrationTypes.FreightBillImport
                Return "FreightBillImport"
            Case IntegrationTypes.Book
                Return "Book"
            Case IntegrationTypes.PickList
                Return "PickList"
            Case IntegrationTypes.APExport
                Return "APExport"
            Case IntegrationTypes.FreightBillExport
                Return "FreightBillExport"
            Case IntegrationTypes.OpenPayables
                Return "OpenPayables"
            Case IntegrationTypes.EDI214
                Return "EDI214"
            Case IntegrationTypes.EDI204
                Return "EDI204"
            Case IntegrationTypes.EDI990
                Return "EDI990"
            Case IntegrationTypes.PalletType
                Return "PalletType"
            Case IntegrationTypes.Hazmat
                Return "Hazmat"
            Case IntegrationTypes.EDI210
                Return "EDI210"
            Case IntegrationTypes.OrderChangeValidation
                Return "OrderChangeValidation"
            Case Else
                Return ""
        End Select
    End Function


    ''' <summary>
    ''' We have now moved all the logic to NGL.Core.Utility.DataTransformation
    ''' this method is left as a wrapper for older code
    ''' </summary>
    ''' <param name="strVal"></param>
    ''' <param name="fixWidth"></param>
    ''' <param name="strFiller"></param>
    ''' <param name="blnFillLeft"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function buildFixWidth(ByVal strVal As String,
                                         ByVal fixWidth As Integer,
                                         ByVal strFiller As String,
                                         Optional ByVal blnFillLeft As Boolean = True) As String
        Return Ngl.Core.Utility.DataTransformation.buildFixWidth(strVal, fixWidth, strFiller, blnFillLeft)
    End Function
    ''' <summary>
    ''' We have now moved all the logic to NGL.Core.Utility.DataTransformation
    ''' this method is left as a wrapper for older code
    ''' </summary>
    ''' <param name="strVal"></param>
    ''' <param name="segSeperator"></param>
    ''' <param name="sebSeperator"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function cleanEDI(ByVal strVal As String, Optional ByVal segSeperator As String = "~", Optional ByVal sebSeperator As String = ":") As String
        Return Ngl.Core.Utility.DataTransformation.cleanEDI(strVal, sebSeperator, sebSeperator)
    End Function
    ''' <summary>
    ''' We have now moved all the logic to NGL.Core.Utility.DataTransformation
    ''' this method is left as a wrapper for older code
    ''' </summary>
    ''' <param name="oField"></param>
    ''' <param name="defaultvalue"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function nz(ByVal oField As Object, ByVal defaultvalue As Object) As Object
        Return Ngl.Core.Utility.DataTransformation.NZ(oField, defaultvalue)
        'Old logic saved in case we have errors
        'Dim blnRetDefault As Boolean = False
        'Try
        '    If oField Is Nothing Then
        '        Return defaultvalue
        '    End If
        'Catch ex As Exception

        'End Try

        'Try

        '    If IsDBNull(oField) Then
        '        Return defaultvalue
        '    End If
        'Catch ex As Exception

        'End Try

        'If oField.ToString.Trim.Length < 1 Then
        '    Return defaultvalue
        'Else
        '    Return oField
        'End If

    End Function


    ''' <summary>
    ''' We have now moved all the logic to NGL.Core.Utility.DataTransformation
    ''' this method is left as a wrapper for older code
    ''' </summary>
    ''' <param name="oRow"></param>
    ''' <param name="FieldName"></param>
    ''' <param name="defaultvalue"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function nz(ByVal oRow As System.Data.DataRow, ByVal FieldName As String, ByVal defaultvalue As Object) As Object
        Return Ngl.Core.Utility.DataTransformation.NZ(oRow, FieldName, defaultvalue)
    End Function

    ''' <summary>
    ''' We have now moved all the logic to NGL.Core.Utility.DataTransformation
    ''' this method is left as a wrapper for older code
    ''' </summary>
    ''' <param name="oField"></param>
    ''' <param name="sFormat"></param>
    ''' <param name="defaultvalue"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function nzDate(ByVal oField As Object, ByVal sFormat As String, ByVal defaultvalue As String) As String
        Return Ngl.Core.Utility.DataTransformation.nzDate(oField, sFormat, defaultvalue)
    End Function

    ''' <summary>
    ''' We have now moved all the logic to NGL.Core.Utility.DataTransformation
    ''' this method is left as a wrapper for older code
    ''' </summary>
    ''' <param name="oRow"></param>
    ''' <param name="FieldName"></param>
    ''' <param name="sFormat"></param>
    ''' <param name="defaultvalue"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function nzDate(ByVal oRow As System.Data.DataRow, ByVal FieldName As String, ByVal sFormat As String, ByVal defaultvalue As String) As String
        Return Ngl.Core.Utility.DataTransformation.nzDate(oRow, FieldName, sFormat, defaultvalue)
    End Function


    Public Shared Function exportDateToString(ByVal strDate As String, Optional ByVal strFormat As String = "MM/dd/yyyy HH:mm:ss") As String
        Dim strRet As String = ""
        Try
            If IsDate(strDate) Then
                Dim dtVal As Date = strDate
                strRet = Format(dtVal, strFormat)
            End If
        Catch ex As Exception
            'do nothing
        End Try
        Return strRet
    End Function

    Public Shared Function convertStringDateToNullableDate(ByVal strDate As String) As Date?
        Dim dtRet As Date? = Nothing
        Dim dtVal As Date
        Try
            If Date.TryParse(strDate, dtVal) Then
                dtRet = dtVal
            End If
        Catch ex As Exception
            'do nothing
        End Try
        Return dtRet
    End Function


    Public Shared Function validateSQLValue(ByRef oField As Object,
                                            ByVal enumDataType As ValidateDataType,
                                            Optional ByVal strSource As String = "NGL.FreightMaster.Integration",
                                            Optional ByVal strErrMsg As String = "",
                                            Optional ByVal blnAllowNull As Boolean = False,
                                            Optional ByVal intLength As Short = 1) As String

        Dim lngErrorNumber As Integer = 0
        Dim strErrdesc As String = ""
        Dim lngval As Integer = 0
        Dim intVal As Short = 0
        Dim blnVal As Boolean = False
        Dim dtVal As Date
        Dim dblVal As Double = 0
        Dim curVal As Decimal = 0
        Dim stryear As String = ""
        Dim strmonth As String
        Dim strday As String
        Dim strhour As String
        Dim strminute As String
        Dim strReturn As String = "''"


        Try
            If enumDataType = ValidateDataType.vdtDate Then
                'test for date value
                If IsDBNull(oField) Or Val(nz(oField, "0")) = 0 Then
                    If blnAllowNull Then
                        Return "NULL"
                    Else
                        Throw New System.ApplicationException("Error # " & gclngErrorNumber2.ToString & ". " & gcstrErrorDesc2 & " " & strErrMsg)
                    End If
                ElseIf IsDate(oField) Then
                    Return "'" & oField.ToString & "'"
                Else
                    If InStr(1, oField.ToString, "-") Or InStr(1, oField.Value, "\") Then
                        Throw New System.ApplicationException("Error # " & gclngErrorNumber3.ToString & ". " & gcstrErrorDesc3 & " " & strErrMsg)
                    Else
                        stryear = Right(oField.ToString, 4)
                        strReturn = Left(oField.ToString, Len(oField.Value) - 4)
                        strday = Right(strReturn, 2)
                        strmonth = Left(strReturn, Len(strReturn) - 2)
                        If Not validateDate(strmonth & "/" & strday & "/" & stryear, "M/d/yyyy", dtVal) Then
                            Throw New System.ApplicationException("Error # " & gclngErrorNumber3.ToString & ". " & gcstrErrorDesc3 & " " & strErrMsg)
                        End If
                        Return "'" & dtVal & "'"
                    End If
                End If
            ElseIf enumDataType = ValidateDataType.vdtTime Then
                'test for Time value
                If IsDBNull(oField) Or Val(nz(oField, "0")) = 0 Then
                    If blnAllowNull Then
                        Return "NULL"
                    Else
                        Throw New System.ApplicationException("Error # " & gclngErrorNumber2.ToString & ". " & gcstrErrorDesc2 & " " & strErrMsg)
                    End If
                ElseIf IsDate(oField) Then
                    Return "'" & oField.ToString & "'"
                Else
                    If InStr(1, oField.ToString, "-") Or InStr(1, oField.ToString, "\") Or InStr(1, oField.ToString, ":") Then
                        Throw New System.ApplicationException("Error # " & gclngErrorNumber3.ToString & ". " & gcstrErrorDesc3 & " " & strErrMsg)
                    Else
                        strminute = Right(oField.ToString, 2)
                        strhour = Left(oField.ToString, 2)
                        If Not DateTime.TryParse("1/1/2000 " & strhour & ":" & strminute, dtVal) Then
                            Throw New System.ApplicationException("Error # " & gclngErrorNumber3.ToString & ". " & gcstrErrorDesc3 & " " & strErrMsg)
                        End If
                        Return "'" & dtVal.ToShortTimeString & "'"
                    End If
                End If
            ElseIf enumDataType = ValidateDataType.vdtFloat Then
                'test for float value
                If IsDBNull(oField) Then
                    If blnAllowNull Then
                        Return "NULL"
                    Else
                        Throw New System.ApplicationException("Error # " & gclngErrorNumber2.ToString & ". " & gcstrErrorDesc2 & " " & strErrMsg)
                    End If
                ElseIf Double.TryParse(oField.ToString, dblVal) Then
                    Return dblVal.ToString
                Else
                    Throw New System.ApplicationException("Error # " & gclngErrorNumber3.ToString & ". " & gcstrErrorDesc3 & " " & strErrMsg)
                End If
            ElseIf enumDataType = ValidateDataType.vdtMoney Then
                'test for currency value
                If IsDBNull(oField) Then
                    If blnAllowNull Then
                        Return "NULL"
                    Else
                        Throw New System.ApplicationException("Error # " & gclngErrorNumber2.ToString & ". " & gcstrErrorDesc2 & " " & strErrMsg)
                    End If
                ElseIf Double.TryParse(oField.ToString, dblVal) Then
                    curVal = dblVal
                    Return curVal.ToString
                Else
                    Throw New System.ApplicationException("Error # " & gclngErrorNumber3.ToString & ". " & gcstrErrorDesc3 & " " & strErrMsg)
                End If
            ElseIf enumDataType = ValidateDataType.vdtLongInt Then
                'test for long integer value
                If IsDBNull(oField) Then
                    If blnAllowNull Then
                        Return "NULL"
                    Else
                        Throw New System.ApplicationException("Error # " & gclngErrorNumber2.ToString & ". " & gcstrErrorDesc2 & " " & strErrMsg)
                    End If
                ElseIf Integer.TryParse(oField.ToString, lngval) Then
                    Return lngval.ToString
                Else
                    Throw New System.ApplicationException("Error # " & gclngErrorNumber3.ToString & ". " & gcstrErrorDesc3 & " " & strErrMsg)
                End If
            ElseIf enumDataType = ValidateDataType.vdtSmallInt Then
                'test for small integer value
                If IsDBNull(oField) Then
                    If blnAllowNull Then
                        Return "NULL"
                    Else
                        Throw New System.ApplicationException("Error # " & gclngErrorNumber2.ToString & ". " & gcstrErrorDesc2 & " " & strErrMsg)
                    End If
                ElseIf Short.TryParse(oField.ToString, intVal) Then
                    Return intVal.ToString
                Else
                    Throw New System.ApplicationException("Error # " & gclngErrorNumber3.ToString & ". " & gcstrErrorDesc3 & " " & strErrMsg)
                End If
            ElseIf enumDataType = ValidateDataType.vdtTinyInt Then
                'test for tiny integer value
                If IsDBNull(oField) Then
                    If blnAllowNull Then
                        Return "NULL"
                    Else
                        Throw New System.ApplicationException("Error # " & gclngErrorNumber2.ToString & ". " & gcstrErrorDesc2 & " " & strErrMsg & " Allow NULL is False")
                    End If
                ElseIf Short.TryParse(oField.ToString, intVal) Then
                    If intVal < 0 Or intVal > 255 Then
                        Throw New System.ApplicationException("Error # " & gclngErrorNumber3.ToString & ". " & gcstrErrorDesc3 & " " & strErrMsg & " Value: " & intVal.ToString())
                    End If
                    Return intVal.ToString
                Else
                    Throw New System.ApplicationException("Error # " & gclngErrorNumber3.ToString & ". " & gcstrErrorDesc3 & " " & strErrMsg & " Value: " & intVal.ToString())
                End If
            ElseIf enumDataType = ValidateDataType.vdtBit Then
                '
                'test for Bit integer value
                If IsDBNull(oField) Then
                    If blnAllowNull Then
                        Return "NULL"
                    Else
                        Throw New System.ApplicationException("Error # " & gclngErrorNumber2.ToString & ". " & gcstrErrorDesc2 & " " & strErrMsg)
                    End If
                Else
                    'test if this value is a zero or a one integer
                    Dim strVal = oField.ToString
                    If Integer.TryParse(strVal, intVal) Then
                        If intVal <> 0 And intVal <> 1 And intVal <> -1 Then
                            Throw New System.ApplicationException("Error # " & gclngErrorNumber3.ToString & ". " & gcstrErrorDesc3 & " " & strErrMsg)
                        End If
                        If intVal = 0 Then
                            Return intVal.ToString
                        Else
                            Return "1"
                        End If

                    ElseIf Boolean.TryParse(oField.ToString, blnVal) Then
                        Select Case blnVal
                            Case True
                                intVal = 1
                            Case False
                                intVal = 0
                            Case Else
                                Throw New System.ApplicationException("Error # " & gclngErrorNumber3.ToString & ". " & gcstrErrorDesc3 & " " & strErrMsg)
                        End Select
                        Return intVal.ToString
                    Else
                        Throw New System.ApplicationException("Error # " & gclngErrorNumber3.ToString & ". " & gcstrErrorDesc3 & " " & strErrMsg)
                    End If

                End If
            Else
                'we test for a string (text) value
                If IsDBNull(oField) Then
                    If blnAllowNull Then
                        Return "NULL"
                    Else
                        Throw New System.ApplicationException("Error # " & gclngErrorNumber2.ToString & ". " & gcstrErrorDesc2 & " " & strErrMsg)
                    End If
                ElseIf Len(oField.ToString) <= intLength Then
                    Return "'" & padQuotes(oField.ToString) & "'"
                Else
                    Throw New System.ApplicationException("Error # " & gclngErrorNumber3.ToString & ". " & gcstrErrorDesc3 & " " & strErrMsg)
                End If
            End If
        Catch ex As System.ApplicationException
            Throw
        Catch ex As System.InvalidCastException
            Throw New System.ApplicationException("Error # " & gclngErrorNumber3.ToString & ". " & gcstrErrorDesc3 & " " & ex.Message & " " & strErrMsg)
        Catch ex As Exception
            Throw New System.ApplicationException("Error # " & gclngErrorNumber5.ToString & ". " & gcstrErrorDesc5 & " " & ex.Message & " " & strErrMsg)
        End Try
        Return strReturn

    End Function

    Public Shared Function validateDate(ByVal strDate As String, ByVal strFormat As String, ByRef dtVal As Date) As Boolean
        Dim blnRet As Boolean = False
        Try
            dtVal = DateTime.ParseExact(strDate, strFormat, Nothing).ToString
            Return True
        Catch ex As System.ArgumentNullException
            Return False
        Catch ex As System.FormatException
            Return False
        Catch ex As Exception
            Throw
        End Try
        Return blnRet
    End Function

    Public Shared Function validateDateWS(ByVal strDate As String, ByRef dtVal As Date) As Boolean

        Dim blnRet As Boolean = False
        Try

            'test for date value
            If IsDBNull(strDate) Or Val(nz(strDate, "0")) = 0 Then
                Return False
            ElseIf IsDate(strDate) Then
                dtVal = CDate(strDate)
                Return True
            Else
                strDate = Trim(strDate)
                If InStr(1, strDate, "-") Or InStr(1, strDate, "\") Then
                    Return False
                Else
                    If Len(strDate) <> 8 Then
                        Return False
                    Else
                        Dim strYear As String = Right(strDate, 4)
                        Dim strReturn As String = Left(strDate, Len(strDate) - 4)
                        Dim strDay As String = Right(strReturn, 2)
                        Dim strMonth = Left(strReturn, Len(strReturn) - 2)
                        strReturn = strMonth & "/" & strDay & "/" & strYear
                        If validateDate(strReturn, "M/d/yyyy", dtVal) Then  'default is short date
                            Return True
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Throw
        End Try
        Return blnRet

    End Function

    Public Shared Function ValidateTimeWS(ByRef strVal As String, Optional ByVal strDefault As String = "") As Boolean
        Dim blnRet As Boolean = False
        Dim strhour As String
        Dim strminute As String
        Dim strseconds As String
        Dim dtVal As Date

        'test for Time value
        If IsDBNull(strVal) Or Val(nz(strVal, "0")) = 0 Then
            If Not String.IsNullOrEmpty(strDefault.Trim) AndAlso IsDate(strDefault) Then
                strVal = strDefault
                blnRet = True
            End If
        ElseIf IsDate(strVal) Then
            blnRet = True
        Else
            If InStr(1, strVal, "-") Or InStr(1, strVal, "\") Or InStr(1, strVal, ":") Then
                'the IsDate function should have worked on a valid time string
                blnRet = False
            ElseIf strVal.Length = 4 Then
                'convert the time format HHmm to a string containing the name of the day of the week, 
                'the name of the month, the numeric day of the hours, minutes equivalent 
                'to the time value of this instance using the date Jan 1st 2000 
                strminute = Right(strVal.ToString, 2)
                strhour = Left(strVal.ToString, 2)
                If DateTime.TryParse("1/1/2000 " & strhour & ":" & strminute, dtVal) Then
                    strVal = dtVal.ToShortTimeString
                    blnRet = True
                End If
            ElseIf strVal.Length = 8 Then
                'convert the time format HHmmss to a string containing the name of the day of the week, 
                'the name of the month, the numeric day of the hours, minutes, and seconds equivalent 
                'to the time value of this instance using the date Jan 1st 2000
                strseconds = Right(strVal.ToString, 2)
                strminute = strVal.Substring(2, 2)
                strhour = Left(strVal.ToString, 2)
                If DateTime.TryParse("1/1/2000 " & strhour & ":" & strminute & ":" & strseconds, dtVal) Then
                    strVal = dtVal.ToShortTimeString
                    blnRet = True
                End If
            Else
                blnRet = False
            End If
        End If
        Return blnRet
    End Function
    ''' <summary>
    '''We have now moved all the logic to NGL.Core.Utility.DataTransformation
    ''' this method is left as a wrapper for older code 
    ''' </summary>
    ''' <param name="strVal"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function padQuotes(ByVal strVal As String) As String
        Return Ngl.Core.Utility.DataTransformation.padQuotes(strVal)
    End Function
    ''' <summary>
    ''' We have now moved all the logic to NGL.Core.Utility.DataTransformation
    ''' this method is left as a wrapper for older code
    ''' </summary>
    ''' <param name="strVal"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function padSpaces(ByVal strVal As String) As String
        Return Ngl.Core.Utility.DataTransformation.padSpaces(strVal)
    End Function

    ''Removed by RHR 
    '' ''' <summary>
    '' ''' We have now moved all the logic to NGL.Core.Communication.Email
    '' ''' this method is left as a wrapper for older code
    '' ''' </summary>
    '' ''' <param name="strServer"></param>
    '' ''' <param name="strTo"></param>
    '' ''' <param name="strFrom"></param>
    '' ''' <param name="strBody"></param>
    '' ''' <param name="strSubject"></param>
    '' ''' <returns></returns>
    '' ''' <remarks></remarks>
    'Public Shared Function SendMail(ByVal strServer As String, ByVal strTo As String, ByVal strFrom As String, ByVal strBody As String, ByVal strSubject As String) As Boolean
    '    Dim oEmail As New Ngl.Core.Communication.Email

    '    return oEmail.SendMail(strServer, strTo, strFrom, strBody, strSubject) 
    '    'Try

    '    '    email.To = strTo
    '    '    email.From = strFrom
    '    '    email.Body = strBody
    '    '    email.Subject = strSubject
    '    '    email.BodyFormat = Web.Mail.MailFormat.Html
    '    '    System.Web.Mail.SmtpMail.SmtpServer = strServer
    '    '    System.Web.Mail.SmtpMail.Send(email)
    '    '    Return True
    '    'Catch ex As Exception
    '    '    Return False
    '    'End Try
    'End Function
    ''' <summary>
    ''' Removes single quotes from the beginning and the end of any string. 
    ''' We have now moved all the logic to NGL.Core.Utility.DataTransformation
    ''' this method is left as a wrapper for older code 
    ''' </summary>
    ''' <param name="strVal"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function stripQuotes(ByVal strVal As String) As String
        Return Ngl.Core.Utility.DataTransformation.stripQuotes(strVal)
    End Function
    ''' <summary>
    ''' We have now moved all the logic to NGL.Core.Utility.DataTransformation
    ''' this method is left as a wrapper for older code 
    ''' </summary>
    ''' <param name="strCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function zipClean(ByVal strCode As String) As String
        Return Ngl.Core.Utility.DataTransformation.zipClean(strCode)
    End Function
    ''' <summary>
    ''' We have now moved all the logic to NGL.Core.Utility.DataTransformation
    ''' this method is left as a wrapper for older code 
    ''' </summary>
    ''' <param name="oRow"></param>
    ''' <param name="strField"></param>
    ''' <param name="strDefault"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function getDataRowString(ByRef oRow As System.Data.DataRow, ByVal strField As String, Optional ByVal strDefault As String = "") As String
        Return Ngl.Core.Utility.DataTransformation.getDataRowString(oRow, strField, strDefault)
    End Function
    ''' <summary>
    ''' We have now moved all the logic to NGL.Core.Utility.DataTransformation
    ''' this method is left as a wrapper for older code 
    ''' </summary>
    ''' <param name="oRow"></param>
    ''' <param name="strField"></param>
    ''' <param name="oDefault"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function getSQLDataReaderValue(ByRef oRow As System.Data.SqlClient.SqlDataReader, ByVal strField As String, Optional ByVal oDefault As Object = Nothing) As Object
        Return Ngl.Core.Utility.DataTransformation.getSQLDataReaderValue(oRow, strField, oDefault)
    End Function
    ''' <summary>
    ''' We have now moved all the logic to NGL.Core.Utility.DataTransformation
    ''' this method is left as a wrapper for older code 
    ''' </summary>
    ''' <param name="oRow"></param>
    ''' <param name="strField"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function getDataRowValue(ByRef oRow As System.Data.DataRow, ByVal strField As String, Optional ByVal oDefault As Object = Nothing) As Object
        Return Ngl.Core.Utility.DataTransformation.getDataRowValue(oRow, strField, oDefault)
    End Function
    ''' <summary>
    ''' We have now moved all the logic to NGL.Core.Utility.DataTransformation
    ''' this method is left as a wrapper for older code 
    ''' </summary>
    ''' <param name="oRow"></param>
    ''' <param name="strField"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function getDataRowOrigValue(ByRef oRow As System.Data.DataRow, ByVal strField As String) As Object
        Return Ngl.Core.Utility.DataTransformation.getDataRowOrigValue(oRow, strField)
    End Function

    Public Shared Function createWCFParameters(ByVal strDBServer As String, ByVal strDatabase As String, ByVal strConnectionString As String) As Ngl.FreightMaster.Data.WCFParameters
        Dim oWCFParameters As New Ngl.FreightMaster.Data.WCFParameters
        With oWCFParameters
            .UserName = "System Download"
            .Database = strDatabase
            .DBServer = strDBServer
            .ConnectionString = strConnectionString
            .WCFAuthCode = "NGLSystem"
            .ValidateAccess = False
        End With
        Return oWCFParameters
    End Function

#End Region

End Class
