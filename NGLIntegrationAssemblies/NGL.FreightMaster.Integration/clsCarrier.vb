Imports NGL.FreightMaster.Integration.Configuration
Imports DTran = Ngl.Core.Utility.DataTransformation
Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects
Imports LTS = Ngl.FreightMaster.Data.LTS
Imports DAL = Ngl.FreightMaster.Data
Imports System.ServiceModel

<Serializable()> _
Public Class clsCarrier : Inherits clsDownload


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

#Region "Methods"

    Public Function getDataSet() As CarrierData
        Return New CarrierData
    End Function

    Public Function ProcessObjectData( _
                ByVal oCarriers() As clsCarrierHeaderObject, _
                ByVal oContacts() As clsCarrierContactObject, _
                ByVal strConnection As String) As ProcessDataReturnValues
        Dim intRet As ProcessDataReturnValues = ProcessDataReturnValues.nglDataIntegrationFailure
        Dim oHTable As New CarrierData.CarrierHeaderDataTable
        Dim oDTable As New CarrierData.CarrierContactDataTable
        Dim dtVal As Date
        Try
            For Each oItem As clsCarrierHeaderObject In oCarriers
                Dim oRow As CarrierData.CarrierHeaderRow = oHTable.NewCarrierHeaderRow
                With oRow
                    .CarrierNumber = oItem.CarrierNumber
                    .CarrierName = Left(oItem.CarrierName, 40)
                    .CarrierStreetAddress1 = Left(oItem.CarrierStreetAddress1, 40)
                    .CarrierStreetAddress2 = Left(oItem.CarrierStreetAddress2, 40)
                    .CarrierStreetAddress3 = Left(oItem.CarrierStreetAddress3, 40)
                    .CarrierStreetCity = Left(oItem.CarrierStreetCity, 25)
                    .CarrierStreetCountry = Left(oItem.CarrierStreetCountry, 30)
                    .CarrierStreetState = Left(oItem.CarrierStreetState, 2)
                    .CarrierStreetZip = Left(oItem.CarrierStreetZip, 10)
                    .CarrierMailAddress1 = Left(oItem.CarrierMailAddress1, 40)
                    .CarrierMailAddress2 = Left(oItem.CarrierMailAddress2, 40)
                    .CarrierMailAddress3 = Left(oItem.CarrierMailAddress3, 40)
                    .CarrierMailCity = Left(oItem.CarrierMailCity, 25)
                    .CarrierMailCountry = Left(oItem.CarrierMailCountry, 30)
                    .CarrierMailState = Left(oItem.CarrierMailState, 2)
                    .CarrierMailZip = Left(oItem.CarrierMailZip, 10)
                    .CarrierTypeCode = Left(oItem.CarrierTypeCode, 1)
                    .CarrierSCAC = Left(oItem.CarrierSCAC, 4)
                    .CarrierWebSite = Left(oItem.CarrierWebSite, 255)
                    .CarrierEmail = Left(oItem.CarrierEmail, 30)
                    If validateDateWS(oItem.CarrierQualInsuranceDate, dtVal) Then
                        .CarrierQualInsuranceDate = exportDateToString(dtVal.ToString)
                    End If
                    .CarrierQualQualified = oItem.CarrierQualQualified
                    .CarrierQualAuthority = Left(oItem.CarrierQualAuthority, 15)
                    .CarrierQualContract = oItem.CarrierQualContract
                    If validateDateWS(oItem.CarrierQualSignedDate, dtVal) Then
                        .CarrierQualSignedDate = exportDateToString(dtVal.ToString)
                    End If
                    If validateDateWS(oItem.CarrierQualContractExpiresDate, dtVal) Then
                        .CarrierQualContractExpiresDate = exportDateToString(dtVal.ToString)
                    End If
                End With
                oHTable.AddCarrierHeaderRow(oRow)
            Next
            Try
                For Each oDetail As clsCarrierContactObject In oContacts
                    Dim oRow As CarrierData.CarrierContactRow = oDTable.NewCarrierContactRow
                    With oRow
                        .CarrierNumber = oDetail.CarrierNumber
                        .CarrierContName = Left(oDetail.CarrierContName, 25)
                        .CarrierContTitle = Left(oDetail.CarrierContTitle, 25)
                        .CarrierContactPhone = Left(oDetail.CarrierContactPhone, 15)
                        .CarrierContactFax = Left(oDetail.CarrierContactFax, 15)
                        .CarrierContact800 = Left(oDetail.CarrierContact800, 15)
                        .CarrierContactEMail = Left(oDetail.CarrierContactEMail, 50)
                    End With
                    oDTable.AddCarrierContactRow(oRow)
                Next
            Catch ex As Exception
                LogException("Process Object Data Warning (import not affected)", "Invalid or missing carrier calendar information", AdminEmail, ex, "NGL.FreightMaster.Integration.clsCarrier.ProcessObjectData Warning (import not affected).")
            End Try
            intRet = ProcessData(oHTable, oDTable, strConnection)
        Catch ex As Exception
            LogException("Process Object Data Failure", "Order import system error", AdminEmail, ex, "NGL.FreightMaster.Integration.clsCarrier.ProcessObjectData Failure")
        End Try
        Return intRet


    End Function

    Public Function ProcessCarrierHeader70(ByRef oItem As clsCarrierHeaderObject70, _
                                           ByRef oCarrierWCFData As DAL.NGLCarrierData, _
                                           ByRef oAllowUpdatePar As AllowUpdateParameters, _
                                           ByRef oCurrencyDictionary As Dictionary(Of String, Integer), _
                                           ByRef oCurrencyWCFData As DAL.NGLCurrencyData) As ProcessCarrierHeader70Result
        Dim intRet As ProcessDataReturnValues
        Dim dtVal As Date
        Dim result As New ProcessCarrierHeader70Result
        result.successFlag = False

        Try
            Dim strValMsg As String = ""
            Dim strDatakey As String = ""
            Dim oRow As DTO.Carrier = oCarrierWCFData.GetCarrierFilteredByLegalEntity(oItem.CarrierLegalEntity, oItem.CarrierAlphaCode, oItem.CarrierNumber)
            Dim insertFlag As Boolean
            If oRow Is Nothing Then oRow = New DTO.Carrier
            If oRow.CarrierControl = 0 Then
                insertFlag = True
                If Not oCarrierWCFData.ValidateCarrierBeforeInsert(oItem.CarrierNumber,
                                                                   oItem.CarrierName,
                                                                   oItem.CarrierLegalEntity,
                                                                   oItem.CarrierAlphaCode,
                                                                   strValMsg) Then
                    result.processCarrierHeaderFailed(oItem.CarrierName, oItem.CarrierNumber, oItem.CarrierAlphaCode, oItem.CarrierLegalEntity)
                    Me.LastError = "Cannot insert new carrier where " & buildDataKeyString(result.dicInvalidKeys) & " because: " & strValMsg & "."
                    AddToGroupEmailMsg(Me.LastError)
                    Return result
                End If
            Else
                insertFlag = False

                oItem.CarrierNumber = oRow.CarrierNumber
                oItem.CarrierAlphaCode = oRow.CarrierAlphaCode
                oItem.CarrierLegalEntity = oRow.CarrierLegalEntity            
            End If
            oAllowUpdatePar.insertFlag = insertFlag
            result.insertFlag = insertFlag
            With oRow
                .CarrierLegalEntity = oItem.CarrierLegalEntity
                .CarrierNumber = oItem.CarrierNumber
                .CarrierAlphaCode = oItem.CarrierAlphaCode
                If AllowUpdate("CarrierName", oAllowUpdatePar) Then .CarrierName = oItem.CarrierName
                If AllowUpdate("CarrierStreetAddress1", oAllowUpdatePar) Then .CarrierStreetAddress1 = oItem.CarrierStreetAddress1
                If AllowUpdate("CarrierStreetAddress2", oAllowUpdatePar) Then .CarrierStreetAddress2 = oItem.CarrierStreetAddress2
                If AllowUpdate("CarrierStreetAddress3", oAllowUpdatePar) Then .CarrierStreetAddress3 = oItem.CarrierStreetAddress3
                If AllowUpdate("CarrierStreetCity", oAllowUpdatePar) Then .CarrierStreetCity = oItem.CarrierStreetCity
                If AllowUpdate("CarrierStreetCountry", oAllowUpdatePar) Then .CarrierStreetCountry = oItem.CarrierStreetCountry
                If AllowUpdate("CarrierStreetState", oAllowUpdatePar) Then .CarrierStreetState = oItem.CarrierStreetState
                If AllowUpdate("CarrierStreetZip", oAllowUpdatePar) Then .CarrierStreetZip = oItem.CarrierStreetZip
                If AllowUpdate("CarrierMailAddress1", oAllowUpdatePar) Then .CarrierMailAddress1 = oItem.CarrierMailAddress1
                If AllowUpdate("CarrierMailAddress2", oAllowUpdatePar) Then .CarrierMailAddress2 = oItem.CarrierMailAddress2
                If AllowUpdate("CarrierMailAddress3", oAllowUpdatePar) Then .CarrierMailAddress3 = oItem.CarrierMailAddress3
                If AllowUpdate("CarrierMailCity", oAllowUpdatePar) Then .CarrierMailCity = oItem.CarrierMailCity
                If AllowUpdate("CarrierMailCountry", oAllowUpdatePar) Then .CarrierMailCountry = oItem.CarrierMailCountry
                If AllowUpdate("CarrierMailState", oAllowUpdatePar) Then .CarrierMailState = oItem.CarrierMailState
                If AllowUpdate("CarrierMailZip", oAllowUpdatePar) Then .CarrierMailZip = oItem.CarrierMailZip
                If AllowUpdate("CarrierTypeCode", oAllowUpdatePar) Then .CarrierTypeCode = oItem.CarrierTypeCode
                If AllowUpdate("CarrierSCAC", oAllowUpdatePar) Then .CarrierSCAC = oItem.CarrierSCAC
                If AllowUpdate("CarrierWebSite", oAllowUpdatePar) Then .CarrierWebSite = oItem.CarrierWebSite
                If AllowUpdate("CarrierEmail", oAllowUpdatePar) Then .CarrierEmail = oItem.CarrierEmail
                If validateDateWS(oItem.CarrierQualInsuranceDate, dtVal) Then
                    .CarrierQualInsuranceDate = exportDateToString(dtVal.ToString)
                End If
                If AllowUpdate("CarrierQualQualified", oAllowUpdatePar) Then .CarrierQualQualified = oItem.CarrierQualQualified
                If AllowUpdate("CarrierQualAuthority", oAllowUpdatePar) Then .CarrierQualAuthority = oItem.CarrierQualAuthority
                If AllowUpdate("CarrierQualContract", oAllowUpdatePar) Then .CarrierQualContract = oItem.CarrierQualContract
                If validateDateWS(oItem.CarrierQualSignedDate, dtVal) Then
                    .CarrierQualSignedDate = exportDateToString(dtVal.ToString)
                End If
                If validateDateWS(oItem.CarrierQualContractExpiresDate, dtVal) Then
                    .CarrierQualContractExpiresDate = exportDateToString(dtVal.ToString)
                End If
                If Not String.IsNullOrEmpty(oItem.CarrierCurrencyType) AndAlso oItem.CarrierCurrencyType.Trim.Length > 0 Then
                    'Lookup the dbo.Currency.CurrencyType using oItem.Carrier.CurrencyType and store the results in CarrierCurType
                    'If inserting a new record set the default value of CarrierCurType to USD
                    If oCurrencyDictionary.ContainsKey(oItem.CarrierCurrencyType) Then
                        .CarrierCurType = oCurrencyDictionary(oItem.CarrierCurrencyType)
                    Else
                        Try
                            Dim oCurrency = oCurrencyWCFData.GetCurrenciesFiltered(oItem.CarrierCurrencyType)
                            If Not oCurrency Is Nothing AndAlso oCurrency.Count > 0 AndAlso oCurrency(0).ID > 0 Then
                                oCurrencyDictionary.Add(oItem.CarrierCurrencyType, oCurrency(0).ID)
                                .CarrierCurType = oCurrency(0).ID
                            Else
                                .CarrierCurType = oCurrencyDictionary("USD")
                                AddToGroupEmailMsg("Currency Type Not Found, " & oItem.CarrierCurrencyType & ", could not be found for Carrier " & oItem.CarrierName & "; using USD by default.")
                                intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
                            End If
                        Catch ex As Exception
                            AddToGroupEmailMsg("Currency Type Not Found, " & oItem.CarrierCurrencyType & ", could not be found for Carrier " & oItem.CarrierName & "; using USD by default.")
                        End Try
                    End If
                Else
                    If insertFlag Then
                        .CarrierCurType = oCurrencyDictionary("USD")
                    End If
                End If
                If AllowUpdate("CarrierUser1", oAllowUpdatePar) Then .CarrierUser1 = oItem.CarrierUser1
                If AllowUpdate("CarrierUser2", oAllowUpdatePar) Then .CarrierUser2 = oItem.CarrierUser2
                If AllowUpdate("CarrierUser3", oAllowUpdatePar) Then .CarrierUser3 = oItem.CarrierUser3
                If AllowUpdate("CarrierUser4", oAllowUpdatePar) Then .CarrierUser4 = oItem.CarrierUser4
            End With
            'Update or Create new Record
            If insertFlag Then
                oRow.TrackingState = Ngl.Core.ChangeTracker.TrackingInfo.Created
                oRow = oCarrierWCFData.CreateRecord(oRow)
                result.successFlag = True
            Else
                oRow.TrackingState = Ngl.Core.ChangeTracker.TrackingInfo.Updated
                oRow = oCarrierWCFData.UpdateRecord(oRow)
                result.successFlag = True
            End If
            If Not oRow Is Nothing Then result.CarrierControl = oRow.CarrierControl
        Catch ex As FaultException(Of DAL.SqlFaultInfo)
            result.processCarrierHeaderFailed(oItem.CarrierName, oItem.CarrierNumber, oItem.CarrierAlphaCode, oItem.CarrierLegalEntity)
            If ex.Detail.Message = "E_InvalidKeyFilterMetaData" Then
                AddToGroupEmailMsg(String.Format("Cannot save changes to {0}. The key field {1} must be unique; the value {2} is already in use for AlphaCode {3}.", ex.Detail.DetailsList.ToArray()))
            ElseIf ex.Detail.Message = "E_InvalidRecordKeyField" Then
                AddToGroupEmailMsg(String.Format("The '{0}' is required and cannot be empty.", ex.Detail.DetailsList.ToArray()))
            Else
                'try to get the not localized version of the string
                Dim oMsgEnm = DAL.SqlFaultInfo.getFaultReasonsEnumFromString(ex.Detail.Message)
                Dim sMsgNotLocalized = DAL.SqlFaultInfo.getFaultInfoMsgsNotLocalizedString(oMsgEnm, ex.Detail.Message)
                AddToGroupEmailMsg("Could not import Carrier where " & buildDataKeyString(result.dicInvalidKeys) & " because: " & sMsgNotLocalized & ".")
            End If
        Catch ex As Exception
            result.processCarrierHeaderFailed(oItem.CarrierName, oItem.CarrierNumber, oItem.CarrierAlphaCode, oItem.CarrierLegalEntity)
            AddToGroupEmailMsg("Could not import Company where " & buildDataKeyString(result.dicInvalidKeys) & " because: " & ex.Message & ".")
        End Try

        Return result

    End Function

    ''' <summary>
    ''' Takes the Carrier Information from a webservice xml data and formats the results in preparation for WCF create and update methods
    ''' </summary>
    ''' <param name="oCarriers"></param>
    ''' <param name="oContacts"></param>
    ''' <param name="strConnection"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Rules:
    ''' 1. Carrier Numbers must be unique in FM.  If the number is not unique by legal entity in the ERP system the Carrier Alpha Code and Legal Entity should be used.
    ''' 2. If the Carrier Number is blank or zero the system will attempt to use the Carrier Alpha Code and Legal Entity
    ''' 3. When using a Carrier Alpha Code a Legal Entity must be provided
    ''' 4. If the Carrier Number, Alpha Code, and Legal Entity are blank or zero cannot import log exception and send email
    ''' 5. If the Carrier Nunmber is blank or zero we must calculate the next available carrier number when inserting a new record using 1 + the highest available carrier number.
    ''' 7. The Carrier Contact key fields (Number, Alpha Code, and Legal Entity) must match the header record exactly
    ''' 6. If the Carrier Header object cannot be imported we skip the Carrier Contacts for that header record
    ''' </remarks>
    Public Function ProcessObjectData70( _
                ByVal oCarriers As List(Of clsCarrierHeaderObject70), _
                ByVal oContacts As List(Of clsCarrierContactObject70), _
                ByVal strConnection As String) As clsIntegrationUpdateResults

        Dim finalResult As New clsIntegrationUpdateResults
        Dim intRet As ProcessDataReturnValues = ProcessDataReturnValues.nglDataIntegrationComplete
        Dim resultObject As New ProcessCarrierHeader70Result
        Try
            'create a new instance of the NGLCarrierData class 
            Dim oWCFParameters As New DAL.WCFParameters
            With oWCFParameters
                .UserName = "System Download"
                .Database = Me.Database
                .DBServer = Me.DBServer
                .ConnectionString = strConnection
                .WCFAuthCode = "NGLSystem"
                .ValidateAccess = False
            End With

            Dim strValMsg As String = ""
            Dim strDatakey As String = ""
            Dim oCarrierWCFData = New DAL.NGLCarrierData(oWCFParameters)
            Dim oCarrierContWCFData = New DAL.NGLCarrierContData(oWCFParameters)
            Dim oCurrencyWCFData = New DAL.NGLCurrencyData(oWCFParameters)
            Dim oCurrencyDictionary As New Dictionary(Of String, Integer)
            oCurrencyDictionary.Add("USD", 1)
            Dim dictionaryList As New List(Of Dictionary(Of String, String))
            Dim successFlag As Boolean = False

            'Create a new allow update parameter object
            Dim oAllowUpdatePar As New AllowUpdateParameters With {.WCFParameters = oWCFParameters, .ImportType = IntegrationTypes.Carrier}
            'Header
            For Each oItem As clsCarrierHeaderObject70 In oCarriers
                If Not oItem Is Nothing Then
                    resultObject = ProcessCarrierHeader70(oItem, oCarrierWCFData, oAllowUpdatePar, oCurrencyDictionary, oCurrencyWCFData)
                    If resultObject.successFlag Then
                        'Modified  by RHR 7/15/2015 we always return the control numbers for testing and debugging 
                        'not just on insert
                        'If resultObject.insertFlag Then finalResult.ControlNumbers.Add(resultObject.CarrierControl)
                        finalResult.ControlNumbers.Add(resultObject.CarrierControl)
                        'at least one company record was processed so set success = true
                        successFlag = True
                    Else
                        dictionaryList.Add(resultObject.dicInvalidKeys)
                        'we never change the value of successFlag on failure 
                        'because the default is already false, once it is true 
                        'we never change it back to false

                        'Modified by RHR 2/18/14  the default value of intRet is ProcessDataReturnValues.nglDataIntegrationComplete
                        'We only need to update this if one of the carrier imports fails
                        intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
                    End If
                    intRet = resultObject.intRet
                Else
                    AddToGroupEmailMsg("One of the carrier header records was null or empty and could not be processed.")
                    intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
                End If
            Next
            If successFlag = False Then
                'this only happens if all carriers fail to import (no need to continue with details)
                finalResult.ReturnValue = ProcessDataReturnValues.nglDataIntegrationFailure
                Return finalResult
            End If

            'Details
            If Not oContacts Is Nothing AndAlso oContacts.Count > 0 Then
                For Each oDetail As clsCarrierContactObject70 In oContacts
                    If Not oDetail Is Nothing Then
                        Try
                            'For details reset the insertFlag to false for the AllowUpdateParameter object
                            'Test the list of keyField dictionaries for records to skip. Only call the stored procedure if oDetail key fields are not in the list
                            oAllowUpdatePar.insertFlag = False
                            Dim blnCanImport As Boolean = True
                            Dim strLogMsg As String = ""
                            'Rule 6:
                            For Each oDictionaryItem As Dictionary(Of String, String) In dictionaryList
                                'Rules:
                                '1. if any carrier number matches we cannot import unless it is 0
                                '2. If the Carrier alpha code and the carrier Legal Entity match we cannot import
                                If oDictionaryItem("Carrier Number") <> 0 AndAlso oDictionaryItem("Carrier Number") = oDetail.CarrierNumber Then
                                    blnCanImport = False
                                    strLogMsg = "Carrier Number = " & oDetail.CarrierNumber
                                    Exit For
                                ElseIf oDictionaryItem("Carrier Alpha Code") = oDetail.CarrierAlphaCode And oDictionaryItem("Carrier Legal Entity") = oDetail.CarrierLegalEntity Then
                                    blnCanImport = False
                                    strLogMsg = "Carrier Alpha Code = " & oDetail.CarrierAlphaCode
                                    Exit For
                                End If
                            Next
                            If blnCanImport Then
                                oCarrierContWCFData.InsertOrUpdateCarrierContact70(oDetail.CarrierLegalEntity, oDetail.CarrierNumber, oDetail.CarrierAlphaCode, oDetail.CarrierContName, oDetail.CarrierContTitle, AllowUpdate("CarrierContTitle", oAllowUpdatePar), oDetail.CarrierContact800, AllowUpdate("CarrierContact800", oAllowUpdatePar), oDetail.CarrierContactPhone, AllowUpdate("CarrierContactPhone", oAllowUpdatePar), oDetail.CarrierContPhoneExt, AllowUpdate("CarrierContPhoneExt", oAllowUpdatePar), oDetail.CarrierContactFax, AllowUpdate("CarrierContactFax", oAllowUpdatePar), oDetail.CarrierContactEMail, AllowUpdate("CarrierContactEMail", oAllowUpdatePar))
                            Else
                                Log("Skipping detail record where " & strLogMsg & " because header record failed to import")
                                intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
                            End If
                        Catch ex As FaultException(Of DAL.SqlFaultInfo)
                            If ex.Detail.Message = "E_ServerMsgDetails" AndAlso Not ex.Detail.DetailsList Is Nothing AndAlso ex.Detail.DetailsList.Count > 0 Then
                                AddToGroupEmailMsg(String.Format("Update Carrier Contact information error. Server Message: {0}", ex.Detail.DetailsList))
                                intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
                            Else
                                AddToGroupEmailMsg("Invalid or missing carrier contact information")
                                intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
                            End If
                        Catch ex As Exception
                            AddToGroupEmailMsg("Invalid or missing carrier contact information")
                            intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
                        End Try
                    Else
                        AddToGroupEmailMsg("One of the carrier contact records was null or empty and could not be processed.")
                        intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
                    End If
                Next
            End If

        Catch ex As Exception
            AddToITEmailMsg("Carrier import system error")
            intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
        Finally
            If GroupEmailMsg.Trim.Length > 0 Then
                LogGroupEmailError("Process Carrier Data Warning", "The following errors or warnings were reported some carrier records may not have been processed correctly." & GroupEmailMsg)
            End If
            If ITEmailMsg.Trim.Length > 0 Then
                LogAdminEmailError("Process Carrier Data Failure", "The following errors or warnings were reported some carrier records may not have been processed correctly." & ITEmailMsg)
            End If
        End Try

        finalResult.ReturnValue = intRet

        Return finalResult

    End Function

    Public Function ProcessData( _
                    ByVal oCarriers As CarrierData.CarrierHeaderDataTable, _
                    ByVal oContacts As CarrierData.CarrierContactDataTable, _
                    ByVal strConnection As String) As ProcessDataReturnValues


        Dim intRet As ProcessDataReturnValues = ProcessDataReturnValues.nglDataIntegrationFailure
        Dim strTitle As String = ""
        Dim strMsg As String = ""
        Dim strHeaderTable As String = "Carrier"
        Dim strItemTable As String = "CarrierCont"
        Me.HeaderName = "Carrier"
        Me.ItemName = "Carrier Contact"
        Me.ImportTypeKey = IntegrationTypes.Carrier
        Me.CreatedDate = Now.ToString
        Me.CreateUser = "Data Integration DLL"
        Me.Source = "Carrier Data Integration"

        Me.DBConnection = strConnection
        'try the connection
        If Not Me.openConnection Then
            Return ProcessDataReturnValues.nglDataConnectionFailure
        End If


        'Header Information
        Dim oFields As New clsImportFields
        If Not buildHeaderCollection(oFields) Then Return ProcessDataReturnValues.nglDataIntegrationFailure
        'Item/Detail Information
        Dim oItems As New clsImportFields
        If Not buildItemCollection(oItems) Then Return ProcessDataReturnValues.nglDataIntegrationFailure
        Try
            'Import the Header Records
            If importHeaderRecords(oCarriers, oFields) Then
                'Now Import the Details
                'Check if the detail file exists
                If Not oContacts Is Nothing AndAlso oContacts.Count > 0 Then
                    importItemRecords(oContacts, oItems)
                Else
                    Log("No carrier contact records were provided.")
                End If
            End If
            strTitle = "Process Data Complete"
            If GroupEmailMsg.Trim.Length > 0 Then
                LogError("Process Carrier Data Warning", "The following errors or warnings were reported some carrier records may not have been processed correctly." & GroupEmailMsg, GroupEmail)
            End If
            If ITEmailMsg.Trim.Length > 0 Then
                LogError("Process Carrier Data Failure", "The following errors or warnings were reported some carrier records may not have been processed correctly." & ITEmailMsg, AdminEmail)
            End If
            If Me.TotalRecords > 0 Then
                intRet = ProcessDataReturnValues.nglDataIntegrationComplete
                strMsg = "Success!  " & Me.TotalRecords & " " & Me.HeaderName & " records were imported."
                If Me.TotalItems > 0 Then
                    strMsg &= vbCrLf & vbCrLf & " And " & Me.TotalItems & " " & Me.ItemName & " records were imported."
                    'intRet = ProcessDataReturnValues.nglDataIntegrationComplete
                End If
                If Me.RecordErrors > 0 Or Me.ItemErrors > 0 Then
                    strTitle = "Process Data Complete With Errors"
                    If Me.RecordErrors > 0 Then
                        strMsg &= vbCrLf & vbCrLf & "ERROR!  " & Me.RecordErrors & " " & Me.HeaderName & " records could not be imported.  Please run the Import Error Report for more information."
                    End If
                    If Me.ItemErrors > 0 Then
                        strMsg &= vbCrLf & vbCrLf & "ERROR!  " & Me.ItemErrors & " " & Me.ItemName & " records could not be imported.  Please run the Import Error Report for more information."
                    End If
                    intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
                End If
            Else
                strMsg = "No " & Me.HeaderName & " records were imported."
                intRet = ProcessDataReturnValues.nglDataIntegrationFailure
            End If
            Log(strMsg)

        Catch ex As Exception
            LogException("Process Carrier Data Failure", "Could not process the requested carrier data.", AdminEmail, ex, "NGL.FreightMaster.Integration.clsCarrier.ProcessData")
        Finally
            closeConnection()
        End Try
        Return intRet

    End Function

    Private Function importHeaderRecords( _
        ByRef oCarriers As CarrierData.CarrierHeaderDataTable, _
        ByRef oFields As clsImportFields) As Boolean
        Dim Ret As Boolean = False
        Try

            'now get the Carrier Header Records
            Dim strSource As String = "clsCarrier.importHeaderRecords"
            Dim blnDataValidated As Boolean = False
            Dim strErrorMessage As String = ""
            Dim blnInsertRecord As Boolean = True
            Dim intRetryCt As Integer = 0

            Do
                intRetryCt += 1
                RecordErrors = 0
                TotalRecords = 0
                Try
                    Try
                        Dim lngMax As Long = oCarriers.Count
                        Log("Importing " & lngMax & " Carrier Header Records.")
                        'Do Until objImportRS.EOF
                        For Each oRow As CarrierData.CarrierHeaderRow In oCarriers
                            strErrorMessage = ""
                            blnDataValidated = validateFields(oFields, oRow, strErrorMessage, strSource)
                            'test if the record already exists.
                            If blnDataValidated Then blnDataValidated = doesRecordExist(oFields, _
                                                                                                strErrorMessage, _
                                                                                                blnInsertRecord, _
                                                                                                "Carrier number " & oFields("CarrierNumber").Value, _
                                                                                                "Carrier")
                            If Not blnDataValidated Then
                                addToErrorTable(oFields, "[dbo].[FileImportErrorLog]", strErrorMessage, "Data Integration DLL", mstrHeaderName)
                                RecordErrors += 1
                            Else
                                'Save the changes to the main table
                                If saveData(oFields, blnInsertRecord, "Carrier", "CarrierModUser", "CarrierModDate") Then
                                    TotalRecords += 1
                                End If
                            End If
                        Next
                        Return True
                    Catch ex As Exception
                        Throw
                    Finally

                    End Try
                Catch ex As Exception
                    If intRetryCt > Me.Retry Then
                        ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsCarrier.importHeaderRecords, attempted to import carrier header records from Data Integration DLL " & intRetryCt.ToString & " times without success.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
                        Log("NGL.FreightMaster.Integration.clsCarrier.importHeaderRecords Failed!" & readExceptionMessage(ex))
                    Else
                        Log("NGL.FreightMaster.Integration.clsCarrier.importHeaderRecords Failure Retry = " & intRetryCt.ToString)
                    End If
                End Try
                'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.                
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsCarrier.importHeaderRecords, Could not import from Data Integration DLL.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsCarrier.importHeaderRecords Failed!" & readExceptionMessage(ex))
        End Try
        Return Ret

    End Function

    Private Function importItemRecords( _
        ByRef oContacts As CarrierData.CarrierContactDataTable, _
        ByRef oItems As clsImportFields) As Boolean
        Dim Ret As Boolean = False
        Try
            'now get the Carrier Contact Records
            Dim intRetryCt As Integer = 0
            Dim strSource As String = "clsCarrier.importItemRecords"
            Dim blnDataValidated As Boolean = False
            Dim strErrorMessage As String = ""
            Dim blnInsertRecord As Boolean = True

            Do
                intRetryCt += 1
                ItemErrors = 0
                TotalItems = 0
                Try
                    Try
                        Dim lngMax As Long = oContacts.Count
                        Log("Importing " & lngMax & " Carrier Contact Records.")
                        For Each oRow As CarrierData.CarrierContactRow In oContacts
                            strErrorMessage = ""
                            blnDataValidated = validateFields(oItems, oRow, strErrorMessage, strSource)
                            'Get the parent table key information
                            If blnDataValidated Then blnDataValidated = lookupFKValues(oItems, _
                                                                                                strErrorMessage, _
                                                                                                "Carrier", _
                                                                                                strSource, _
                                                                                                "Carrier contact record for Carrier number " & oItems("CarrierNumber").Value)
                            'test if the record already exists.
                            If blnDataValidated Then blnDataValidated = doesRecordExist(oItems, _
                                                                                                strErrorMessage, _
                                                                                                blnInsertRecord, _
                                                                                                "Carrier contact record for Carrier number " & oItems("CarrierNumber").Value, _
                                                                                                "CarrierCont")
                            If Not blnDataValidated Then
                                addToErrorTable(oItems, "[dbo].[FileDetailsErrorLog]", strErrorMessage, "Data Integration DLL", ItemName)
                                ItemErrors += 1
                            Else
                                'Save the changes
                                If saveData(oItems, blnInsertRecord, "CarrierCont", "", "") Then
                                    TotalItems += 1
                                End If
                            End If
                        Next
                        Return True
                    Catch ex As Exception
                        Throw
                    Finally

                    End Try
                Catch ex As Exception
                    If intRetryCt > Me.Retry Then
                        ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsCarrier.importItemRecords, attempted to import carrier item records from Data Integration DLL " & intRetryCt.ToString & " times without success.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
                        Log("NGL.FreightMaster.Integration.clsCarrier.importItemRecords Failed!" & readExceptionMessage(ex))
                        Me.ItemErrors += 1
                    Else
                        Log("NGL.FreightMaster.Integration.clsCarrier.importItemRecords Failure Retry = " & intRetryCt.ToString)
                    End If
                End Try
                'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.                
        Catch ex As Exception
            Me.ItemErrors += 1
            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsCarrier.importItemRecords, Could not import from Data Integration DLL.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsCarrier.importItemRecords Failed!" & readExceptionMessage(ex))
        End Try
        Return Ret

    End Function

    Private Function buildHeaderCollection(ByRef oFields As clsImportFields) As Boolean
        Dim Ret As Boolean = False
        Try
            With oFields
                .Add("CarrierNumber", "CarrierNumber", clsImportField.DataTypeID.gcvdtLongInt, 11, False, clsImportField.PKValue.gcPK)
                .Add("CarrierName", "CarrierName", clsImportField.DataTypeID.gcvdtString, 40, False)
                .Add("CarrierStreetAddress1", "CarrierStreetAddress1", clsImportField.DataTypeID.gcvdtString, 40, True)
                .Add("CarrierStreetAddress2", "CarrierStreetAddress2", clsImportField.DataTypeID.gcvdtString, 40, True)
                .Add("CarrierStreetAddress3", "CarrierStreetAddress3", clsImportField.DataTypeID.gcvdtString, 40, True)
                .Add("CarrierStreetCity", "CarrierStreetCity", clsImportField.DataTypeID.gcvdtString, 25, True)
                .Add("CarrierStreetState", "CarrierStreetState", clsImportField.DataTypeID.gcvdtString, 8, True)
                .Add("CarrierStreetCountry", "CarrierStreetCountry", clsImportField.DataTypeID.gcvdtString, 30, True)
                .Add("CarrierStreetZip", "CarrierStreetZip", clsImportField.DataTypeID.gcvdtString, 10, True)
                .Add("CarrierMailAddress1", "CarrierMailAddress1", clsImportField.DataTypeID.gcvdtString, 40, True)
                .Add("CarrierMailAddress2", "CarrierMailAddress2", clsImportField.DataTypeID.gcvdtString, 40, True)
                .Add("CarrierMailAddress3", "CarrierMailAddress3", clsImportField.DataTypeID.gcvdtString, 40, True)
                .Add("CarrierMailCity", "CarrierMailCity", clsImportField.DataTypeID.gcvdtString, 25, True)
                .Add("CarrierMailState", "CarrierMailState", clsImportField.DataTypeID.gcvdtString, 8, True)
                .Add("CarrierMailCountry", "CarrierMailCountry", clsImportField.DataTypeID.gcvdtString, 30, True)
                .Add("CarrierMailZip", "CarrierMailZip", clsImportField.DataTypeID.gcvdtString, 10, True)
                .Add("CarrierTypeCode", "CarrierTypeCode", clsImportField.DataTypeID.gcvdtString, 1, True)
                .Add("CarrierSCAC", "CarrierSCAC", clsImportField.DataTypeID.gcvdtString, 4, True)
                .Add("CarrierWebSite", "CarrierWebSite", clsImportField.DataTypeID.gcvdtString, 255, True)
                .Add("CarrierEmail", "CarrierEmail", clsImportField.DataTypeID.gcvdtString, 50, True)
                .Add("CarrierQualInsuranceDate", "CarrierQualInsuranceDate", clsImportField.DataTypeID.gcvdtDate, 22, True)
                .Add("CarrierQualQualified", "CarrierQualQualified", clsImportField.DataTypeID.gcvdtBit, 2, True)
                .Add("CarrierQualAuthority", "CarrierQualAuthority", clsImportField.DataTypeID.gcvdtString, 15, True)
                .Add("CarrierQualContract", "CarrierQualContract", clsImportField.DataTypeID.gcvdtBit, 2, True)
                .Add("CarrierQualSignedDate", "CarrierQualSignedDate", clsImportField.DataTypeID.gcvdtDate, 22, True)
                .Add("CarrierQualContractExpiresDate", "CarrierQualContractExpiresDate", clsImportField.DataTypeID.gcvdtDate, 22, True)
            End With
            Log("Header Field Array Loaded.")
            'get the import field flag values
            For ct As Integer = 1 To oFields.Count
                Dim blnUseField As Boolean = True
                Try
                    If oFields(ct).Name = "CarrierNumber" Then
                        'this field is always in use
                        blnUseField = True
                    Else
                        blnUseField = getImportFieldFlag(oFields(ct).Name, IntegrationTypes.Carrier)
                    End If

                Catch ex As Exception
                    'throw away any errors in case one or more field names is missing in the Import Field Flag Table
                End Try
                oFields(ct).Use = blnUseField
            Next
            Ret = True
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsCarrier.buildHeaderCollection, could not build the header collection data.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsCarrier.buildHeaderCollection Failed!" & readExceptionMessage(ex))
        End Try
        Return Ret

    End Function

    Private Function buildItemCollection(ByRef oItems As clsImportFields) As Boolean
        Dim Ret As Boolean = False
        Try
            With oItems
                .Add("CarrierContCarrierControl", "CarrierContCarrierControl", clsImportField.DataTypeID.gcvdtLongInt, 11, False, clsImportField.PKValue.gcFK, 2, "CarrierNumber", "CarrierControl")
                .Add("CarrierNumber", "CarrierNumber", clsImportField.DataTypeID.gcvdtString, 50, False, clsImportField.PKValue.gcHK)
                .Add("CarrierContName", "CarrierContName", clsImportField.DataTypeID.gcvdtString, 25, True, clsImportField.PKValue.gcPK)
                .Add("CarrierContTitle", "CarrierContTitle", clsImportField.DataTypeID.gcvdtString, 25, True)
                .Add("CarrierContact800", "CarrierContact800", clsImportField.DataTypeID.gcvdtString, 50, True)
                .Add("CarrierContactPhone", "CarrierContactPhone", clsImportField.DataTypeID.gcvdtString, 20, True) 'Modified by RHR for v-8.4.003 on 06/25/2021
                .Add("CarrierContactFax", "CarrierContactFax", clsImportField.DataTypeID.gcvdtString, 15, True)
                .Add("CarrierContactEMail", "CarrierContactEMail", clsImportField.DataTypeID.gcvdtString, 50, True)
            End With
            'get the item  field flag values
            For ct As Integer = 1 To oItems.Count
                Dim blnUseField As Boolean = True
                Try
                    If oItems(ct).Name = "CarrierContCarrierControl" Or oItems(ct).Name = "CarrierNumber" Or oItems(ct).Name = "CarrierContName" Then
                        'These are key fields and are always in use
                        blnUseField = True
                    Else
                        blnUseField = getImportFieldFlag(oItems(ct).Name, IntegrationTypes.Carrier)
                    End If
                Catch ex As Exception
                    'throw away any errors in case one or more field names is missing in the Import Field Flag Table
                End Try
                oItems(ct).Use = blnUseField
            Next
            Ret = True
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsCarrier.buildItemCollection, could not build the item collection data.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsCarrier.buildItemCollection Failed!" & readExceptionMessage(ex))
        End Try
        Return Ret

    End Function

#End Region

End Class

Public Class ProcessCarrierHeader70Result


    Private _dicInvalidKeys As New Dictionary(Of String, String)
    Public Property dicInvalidKeys() As Dictionary(Of String, String)
        Get
            If _dicInvalidKeys Is Nothing Then _dicInvalidKeys = New Dictionary(Of String, String)
            Return _dicInvalidKeys
        End Get
        Set(ByVal value As Dictionary(Of String, String))
            _dicInvalidKeys = value
        End Set
    End Property

    Private _CarrierControl As Integer = 0
    Public Property CarrierControl() As Integer
        Get
            Return _CarrierControl
        End Get
        Set(ByVal value As Integer)
            _CarrierControl = value
        End Set
    End Property

    Private _insertFlag As Boolean = False
    Public Property insertFlag() As Boolean
        Get
            Return _insertFlag
        End Get
        Set(ByVal value As Boolean)
            _insertFlag = value
        End Set
    End Property

    Private _successFlag As Boolean
    Public Property successFlag() As Boolean
        Get
            Return _successFlag
        End Get
        Set(ByVal value As Boolean)
            _successFlag = value
        End Set
    End Property

    Private _intRet As ProcessDataReturnValues
    Public Property intRet() As ProcessDataReturnValues
        Get
            Return _intRet
        End Get
        Set(ByVal value As ProcessDataReturnValues)
            _intRet = value
        End Set
    End Property

    Public Sub processCarrierHeaderFailed(ByVal CarrierName As String, ByVal CarrierNumber As Integer, ByVal CarrierAlphaCode As String, ByVal CarrierLegalEntity As String)
        If Me.dicInvalidKeys Is Nothing OrElse Me.dicInvalidKeys.Count < 1 Then
            'add the key fields to the dictionary
            Me.dicInvalidKeys = New Dictionary(Of String, String) From {{"Carrier Name", CarrierName}, {"Carrier Number", CarrierNumber.ToString()}, {"Carrier Alpha Code", CarrierAlphaCode}, {"Carrier Legal Entity", CarrierLegalEntity}}
        End If
        intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
        successFlag = False
    End Sub

End Class
