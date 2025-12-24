Imports Ngl.FreightMaster.Integration.Configuration
Imports System.Data.SqlClient
Imports System.Xml.Serialization
Imports DTran = Ngl.Core.Utility.DataTransformation

<Serializable()> _
Public Class clsPalletType : Inherits clsDownload


#Region "Constructors"

    Sub New()
        MyBase.new()
    End Sub

    Sub New(ByVal config As Ngl.FreightMaster.Core.UserConfiguration)
        MyBase.New(config)
    End Sub

    Sub New(ByVal admin_email As String, _
            ByVal from_email As String, _
            ByVal group_email As String, _
            ByVal auto_retry As Integer, _
            ByVal smtp_server As String, _
            ByVal db_server As String, _
            ByVal database_catalog As String, _
            ByVal auth_code As String, _
            ByVal debug_mode As Boolean,
            Optional ByVal connection_string As String = "")

        MyBase.New(admin_email, from_email, group_email, auto_retry, smtp_server, db_server, database_catalog, auth_code, debug_mode, connection_string)


    End Sub

#End Region


#Region " Class Variables and Properties "

#End Region


#Region "Private Functions "

    Private Function buildHeaderCollection(ByRef oFields As clsImportFields) As Boolean
        Dim Ret As Boolean = False
        Try
            With oFields
                .Add("PalletType", "PalletType", clsImportField.DataTypeID.gcvdtString, 50, False, clsImportField.PKValue.gcPK) '0
                .Add("PalletTypeDescription", "PalletTypeDescription", clsImportField.DataTypeID.gcvdtString, 50, True) '1
                .Add("PalletTypeWeight", "PalletTypeWeight", clsImportField.DataTypeID.gcvdtFloat, 22, True) '2
                .Add("PalletTypeHeight", "PalletTypeHeight", clsImportField.DataTypeID.gcvdtFloat, 22, True) '3
                .Add("PalletTypeWidth", "PalletTypeWidth", clsImportField.DataTypeID.gcvdtFloat, 22, True) '4
                .Add("PalletTypeDepth", "PalletTypeDepth", clsImportField.DataTypeID.gcvdtFloat, 22, True) '5
                .Add("PalletTypeVolume", "PalletTypeVolume", clsImportField.DataTypeID.gcvdtFloat, 22, True) '5
                .Add("PalletTypeContainer", "PalletTypeContainer", clsImportField.DataTypeID.gcvdtBit, 2, True) '6

            End With
            Log("Pallet Type Header Field Array Loaded.")
            'get the import field flag values
            For ct As Integer = 1 To oFields.Count
                Dim blnUseField As Boolean = True
                Try
                    If oFields(ct).Name = "PalletType" Then
                        'key fields and are always in use
                        blnUseField = True
                    Else
                        blnUseField = getImportFieldFlag(oFields(ct).Name, IntegrationTypes.PalletType)
                    End If
                Catch ex As Exception
                    'throw away any errors in case one or more field names is missing in the Import Field Flag Table
                End Try
                oFields(ct).Use = blnUseField
            Next
            Ret = True
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsPalletType.buildHeaderCollection, could not build the header collection data.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsPalletType.buildHeaderCollection Failed!" & readExceptionMessage(ex))
        End Try
        Return Ret

    End Function

    Private Function importHeaderRecords( _
                ByRef oPTypes As List(Of clsPalletTypeObject), _
                ByRef oFields As clsImportFields) As Boolean
        Dim Ret As Boolean = False
        Try

            Dim intRetryCt As Integer = 0
            Dim strSource As String = "clsPalletType.importHeaderRecords"
            Dim blnDataValidated As Boolean = False
            Dim strErrorMessage As String = ""
            Dim blnInsertRecord As Boolean = True
            Dim blnNoMatchingItem As Boolean = True

            Do
                intRetryCt += 1
                RecordErrors = 0
                TotalRecords = 0
                TotalItems = 0
                Try
                    Log("Importing " & oPTypes.Count & " Pallet Type Records.")
                    For Each oRow As clsPalletTypeObject In oPTypes
                        If Not String.IsNullOrWhiteSpace(oRow.PalletType) Then
                            'If Not oRow Is Nothing Then
                            strErrorMessage = ""
                            blnDataValidated = validateFields(oFields, oRow, strErrorMessage, strSource)
                            'test if the record already exists.  the doesRecordExist returns false only if there is an error or failure
                            If blnDataValidated Then blnDataValidated = doesRecordExist(oFields, _
                                                                                        strErrorMessage, _
                                                                                        blnInsertRecord, _
                                                                                        "Pallet Type " & oFields("PalletType").Value, _
                                                                                        "PalletType")
                            If Not blnDataValidated Then
                                addToErrorTable(oFields, "[dbo].[FileImportErrorLog]", strErrorMessage, "Data Integration DLL", mstrHeaderName)
                                RecordErrors += 1
                            Else
                                'Save the changes to the main table
                                If saveData(oFields, blnInsertRecord, "PalletType") Then TotalRecords += 1
                            End If
                            'Else
                            '    AddToGroupEmailMsg("One of the pallet type records was null or empty and could not be processed.")
                            'End If
                        Else
                            Log("Cannot import pallet type record because PalletType field is empty.")
                        End If
GetNextHeaderRecord:
                    Next
                    Return True
                Catch ex As Exception
                    If intRetryCt > Me.Retry Then
                        ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsPalletType.importHeaderRecords, attempted to import Pallet Type records from Data Integration DLL " & intRetryCt.ToString & " times without success.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
                        Log("NGL.FreightMaster.Integration.clsPalletType.importHeaderRecords Failed!" & readExceptionMessage(ex))
                    Else
                        Log("NGL.FreightMaster.Integration.clsPalletType.importHeaderRecords Failure Retry = " & intRetryCt.ToString)
                    End If
                End Try
                'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.                
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsPalletType.importHeaderRecords, Could not import from Data Integration DLL.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsPalletType.importHeaderRecords Failed!" & readExceptionMessage(ex))
        End Try
        Return Ret

    End Function

#End Region


#Region "Public Functions "

    Public Function ProcessObjectData( _
                    ByVal oPTypes As List(Of clsPalletTypeObject), _
                    ByVal strConnection As String) As ProcessDataReturnValues

        Dim intRet As ProcessDataReturnValues = ProcessDataReturnValues.nglDataIntegrationFailure
        Dim strMsg As String = ""
        Dim strTitle As String = ""
        Dim strSource As String = "clsPalletType.ProcessObjectData"
        Dim strHeaderTable As String = "PalletType"
        Me.HeaderName = "Pallet Type"
        Me.ImportTypeKey = IntegrationTypes.PalletType
        Me.CreatedDate = Now.ToString
        Me.CreateUser = "Data Integration DLL"
        Me.Source = "Pallet Type Data Integration"

        Me.DBConnection = strConnection
        'try the connection
        If Not Me.openConnection Then
            Return ProcessDataReturnValues.nglDataConnectionFailure
        End If

        'set the error date time stamp and other Defaults
        'Header Information
        Dim oFields As New clsImportFields
        If Not buildHeaderCollection(oFields) Then Return ProcessDataReturnValues.nglDataIntegrationFailure
        Try
            'Import the Header Records
            importHeaderRecords(oPTypes, oFields)
            strTitle = "Process Data Complete"
            If GroupEmailMsg.Trim.Length > 0 Then
                LogError("Process PO Data Warning", "The following errors or warnings were reported some " & Me.HeaderName & " records may not have been processed correctly." & GroupEmailMsg, GroupEmail)
            End If
            If ITEmailMsg.Trim.Length > 0 Then
                LogError("Process PO Data Failure", "The following errors or warnings were reported some " & Me.HeaderName & " records may not have been processed correctly.." & ITEmailMsg, AdminEmail)
            End If
            If Me.TotalRecords > 0 Then
                strMsg = "Success!  " & Me.TotalRecords & " " & Me.HeaderName & " records were imported."
                intRet = ProcessDataReturnValues.nglDataIntegrationComplete

                If Me.RecordErrors > 0 Or Me.ItemErrors > 0 Then
                    strTitle = "Process Data Complete With Errors"
                    If Me.RecordErrors > 0 Then
                        strMsg &= vbCrLf & vbCrLf & "ERROR!  " & Me.RecordErrors & " " & Me.HeaderName & " records could not be imported.  Please run the Import Error Report for more information."
                    End If
                    intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
                End If
            Else
                strMsg = "No " & Me.HeaderName & " records were imported."
                intRet = ProcessDataReturnValues.nglDataIntegrationFailure
            End If
            Log(strMsg)
        Catch ex As Exception
            LogException("Process " & Me.HeaderName & " Data Failure", "Could not process the requested " & Me.HeaderName & " data.", AdminEmail, ex, "NGL.FreightMaster.Integration.clsPalletType.ProcessData")
        Finally
            Try
                closeConnection()
            Catch ex As Exception

            End Try
        End Try
        Return intRet


    End Function

#End Region

End Class
