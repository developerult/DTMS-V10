Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports System.Data.Linq
Imports System.Data.Objects
Imports System.Collections.ObjectModel

Imports LTS = Ngl.FreightMaster.Data.LTS
Imports Serilog.Core
Imports Microsoft.VisualBasic.Logging

<DataContract()>
Public Class SqlFaultInfo


    Public Enum FaultReasons
        None
        E_DataConflict
        E_SQLException
        E_InvalidOperationException
        E_UnExpected
        E_DataValidationFailure 'Data validation failure!  Your request could not be completed because it violates one or more business rules or data requirements.
        E_DataAccessError
        E_CreateRecordFailure
        E_DBLoginFailure
        E_AuthCodeFail
        E_AccessDenied 'Access Denied!
        E_ProcessProcedureFailure 'The system was unable to execute the requested procedure.   You should manually refresh your data to be sure you have the latest changes.
        E_ReadAuthenticationCodeError
        E_AccessGranted
        E_ExportAPDataFailure
        E_FTPAPDataFailure
        E_DBConnectionFailure
        E_DataAccessFailure 'There was a problem processing your request to one or more data components.
    End Enum

    Public Enum FaultInfoMsgs
        None
        E_SQLExceptionMSG 'The warnings below were reported from the database.  This may help to correct the problem.
        E_NoData
        E_UnExpectedMSG
        E_ApplicationException
        E_BatchDataNotFound
        E_BatchProcessError 'There was an error while executing the selected batch process.
        E_CreateRecordFailure
        E_CreditHold
        E_CreditNotSetMsg
        E_CreditOverLimit
        E_CultureNotSupported
        E_DataConflict
        E_DataInUse 'The data is being used by one or more dependent records. Check the last error message for more information.
        E_DBConnectionFailure
        E_DefaultCompRequired
        E_FailedToExecute
        E_FileTooLarge
        E_FTPConnectionError
        E_FTPUploadFailure
        E_InvalidFileOrFolderName
        E_InvalidFilterSelection
        E_InvalidKeyField 'One of more of the key fields already exists in the database. 
        E_InvalidOperationException 'We're sorry but your request is not valid.
        E_InvalidPassword
        E_InvalidRequest 'Invalid Request
        E_InvalidUser
        E_LaneNumberRequired
        E_LicenseViolation
        E_LicenseWarning
        E_LoadScreenSecurityFailed
        E_MaxNbrOfCarriers
        E_MaxUsersAssigned
        E_MessageServiceNotAvailable
        E_NETAddressFail
        E_NGLOptDeletePreviousSolutionFailed
        E_NGLOptLoadParameterSettingsFailed
        E_NoLookUpAvailable
        E_NotAuthProcedure
        E_NotAuthReport
        E_NotAuthScreen
        E_NotAuthService
        E_NotSupported
        E_PathToLong
        E_ProcessRunning
        E_ReadAuthenticationCodeError
        E_RecordDeleted
        E_SourceNotValid
        E_UpdateNotAllowed
        E_CannotUpdateTariffApproved 'Cannot modify contracts once approved.
        E_InvalidKeyFilterMetaData 'One or more key record values are invalid or cannot be found.
        E_AssignCarrierFailed   'Cannot assign the desired transportation provider.
        E_InvalidParentKeyField 'A reference to the parent record is required.
        E_InvalidRecordKeyField 'A key reference to the record is missing
        E_InvalidParentOrRecordKeyField 'A key reference to the record or the parent record is missing
        E_MethodOrProperyDepreciated 'The method or property has been depreciated and is no longer available.
        E_AccessDenied 'Access Denied!
        'Modified by RHR for v-8.1 on 04/06/2018'
        E_CreateNewDependentRecordFailed 'The automatic creation of dependent records failed. Please complete the process manually.'
    End Enum

    Public Enum FaultDetailsKey
        None
        E_NoDetails
        E_ExceptionMsgDetails 'Application Exception: {0}
        E_ServerMsgDetails 'Server Message: {0}
        E_ContractApporvedDetails 'Approved {0}, By {1}
        E_ContractRejectedDetails 'Rejected {0}, By {1}
        E_EquipMatPivotKeyFilterDetails 'Cannot read carrier tariff contract rates because the key record {0} value {1} is not valid.
        E_EquipMatPivotAllKeyFilterDetails 'Cannot read carreir tariff contract rates because none of the key record values are valid.
        E_EquipMatDetKeyFieldNewValidationDetails 'Cannot save new Carrier Equipment Matrix Detail data.  The {0} value {1} is already in use for the selected matrix."
        E_OrderIdentificationDetailsWCNS 'Book Control: {0} Order Number: {1} Sequence: {2} Pro: {3} CNS: {4}
        E_OrderIdentificationDetails 'Book Control: {0} Order Number: {1} Sequence: {2} Pro: {3} 
        E_CannotSaveProtectedDataDetails 'Cannot save changes the {0} value {1} is protected and cannot be modified.
        E_CannotDeleteProtectedDataDetails 'Cannot delete data the {0} value {1} is protected and cannot be modified.
        E_CannotSaveKeyAlreadyExists 'Cannot save changes to {0}.  The unique key {1} value {2} already exists.
        E_InvalidClassException 'Cannot process your request because the class {0} is not a valid {1} library.
        E_CannotSaveParentKeyRequired 'Cannot save your changes because the {0} value {1} is required.
        E_NoTariffAvailable 'A tariff is not available for the select route CNS: {0}.
        E_InvalidTranCode   'The Trans Code {0} does not allow updates to transportation providers.
        E_CostsAreLocked    'The costs are locked for book pro number: {0}.
        E_SQLWarningDetails 'The procedure, {0}, returned the following warning: number {1} message {2} 
        E_SystemFaliedToGeneratedKeyField 'The system could not generate a new {0}
        E_FieldRequired 'The '{0}' is required and cannot be empty.
        E_RecordOnHold 'The requested record is on hold and cannot be processed.
        E_CannotSaveRequiredValueDoesNotMatch 'Cannot save changes to {0}. The key field {1} must be unique; the value {2} is already in use for AlphaCode {3}.
        E_CannotSaveKeyValuesAlreadyExist 'Cannot save changes to {0}. A record with one or more of the following key values already exist: Keys {1} Values {2}.
        E_CannotSaveKeyFieldsRequired 'Cannot save changes to {0}.  The following key fields are required: {1}. 
        E_CannotSaveRecordInUseDetails 'Cannot save data the {0} value {1} is being used and cannot be modified.
        E_CannotDeleteRecordInUseDetails 'Cannot delete data the {0} value {1} is being used and cannot be modified.
        'Added By LVV on 8/11/16 for v-7.0.5.110 Ticket #2323
        E_CannotDeleteEquipCodeRecordInUseDetailsMany 'Cannot delete data the Equipment Code {0} is being used by {1} tariffs and cannot be modified.
        E_CannotDeleteEquipCodeRecordInUseDetails 'Cannot delete data the Equipment Code {0} is being used by the following tariffs and cannot be modified: {1}
    End Enum


    Private _Message As String = ""
    <DataMember()>
    Public Property Message() As String
        Get
            Return _Message
        End Get
        Set(ByVal value As String)
            _Message = value
        End Set
    End Property

    Private _Details As String = ""
    <DataMember()>
    Public Property Details() As String
        Get
            Return _Details
        End Get
        Set(ByVal value As String)
            _Details = value
        End Set
    End Property

    Private _DetailsList As New List(Of String)
    Public Property DetailsList() As List(Of String)
        Get
            _DetailsList = New List(Of String)
            For Each t In SQLFaultInfoDetails
                _DetailsList.Add(t.Text)
            Next
            Return _DetailsList
        End Get
        Set(ByVal value As List(Of String))
            SQLFaultInfoDetails = New List(Of DataTransferObjects.SQLFaultInfoDetail)
            For Each s In value
                _SQLFaultInfoDetails.Add(New DataTransferObjects.SQLFaultInfoDetail(s))
            Next
        End Set
    End Property

    Private _SQLFaultInfoDetails As New List(Of DataTransferObjects.SQLFaultInfoDetail)
    <DataMember()>
    Public Property SQLFaultInfoDetails() As List(Of DataTransferObjects.SQLFaultInfoDetail)
        Get
            Return _SQLFaultInfoDetails
        End Get
        Set(ByVal value As List(Of DataTransferObjects.SQLFaultInfoDetail))
            _SQLFaultInfoDetails = value
        End Set
    End Property


    Public Shared Function getFaultReason(ByVal item As FaultReasons) As String
        Dim strReason = "E_UnExpected"
        Try
            Dim Enumerator As Type = GetType(FaultReasons)
            strReason = [Enum].GetName(Enumerator, item)
        Catch ex As System.ArgumentNullException
            'enum type or value is nothing so return default
            Return strReason
        Catch ex As System.ArgumentException
            'the item is not valid so return default
            Return strReason
        Catch ex As Exception
            Return strReason
        End Try

        Return strReason
    End Function

    Public Shared Function getFaultMessage(ByVal item As FaultInfoMsgs) As String
        Dim strReturn = "E_UnExpectedMSG"
        Try
            Dim Enumerator As Type = GetType(FaultInfoMsgs)
            strReturn = [Enum].GetName(Enumerator, item)
        Catch ex As System.ArgumentNullException
            'enum type or value is nothing so return default
            Return strReturn
        Catch ex As System.ArgumentException
            'the item is not valid so return default
            Return strReturn
        Catch ex As Exception
            Return strReturn
        End Try

        Return strReturn
    End Function

    Public Shared Function getFaultDetailsKey(ByVal item As FaultDetailsKey) As String
        Dim strReturn = "E_ExceptionMsgDetails"
        Try
            Dim Enumerator As Type = GetType(FaultDetailsKey)
            strReturn = [Enum].GetName(Enumerator, item)
        Catch ex As System.ArgumentNullException
            'enum type or value is nothing so return default
            Return strReturn
        Catch ex As System.ArgumentException
            'the item is not valid so return default
            Return strReturn
        Catch ex As Exception
            Return strReturn
        End Try

        Return strReturn
    End Function

    ''' <summary>
    ''' Concatenate entire message into one Non Localized Message
    ''' Message + stringFormat(Details, DetailsListArray)
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' use overload to include reason message
    ''' </remarks>
    Public Overrides Function ToString() As String
        Dim strRet As String = Me.Message 'just return the default message unformatted on error
        Try
            'get the Reason
            Dim strDetails As String = ""
            Dim mMessage As String = ""
            Dim mDetails As String = ""
            strDetails = getAlertInfoNotLocalized(Me.Message, Me.Details, Me.DetailsList, "", mMessage, mDetails)
            strRet = String.Concat("Message: ", mMessage, " Details: ", strDetails)
        Catch ex As Exception
            'ignore any errors here
            Serilog.Log.Logger.Error(ex, "Error in ToString")
        End Try
        Return strRet
    End Function

    ''' <summary>
    ''' Concatenate entire message into one Non Localized Message
    ''' Reason + Message + stringFormat(Details, DetailsListArray)
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function ToString(ByVal sReason As String) As String
        Dim strRet As String = Me.Message 'just return the default message unformatted on error
        Try
            'get the Reason
            Dim strDetails As String = ""
            Dim mReason As String = ""
            Dim mMessage As String = ""
            Dim mDetails As String = ""
            strDetails = getAlertInfoNotLocalized(Me.Message, Me.Details, Me.DetailsList, sReason, mMessage, mDetails, mReason)
            strRet = String.Concat("Reason: ", mReason, " Message: ", mMessage, " Details: ", strDetails)
        Catch ex As Exception
            Serilog.Log.Logger.Error(ex, "Error in ToString")
        End Try
        Return strRet
    End Function

    Public Function getMsgForLogs(Optional ByVal sReason As String = "") As String
        If String.IsNullOrWhiteSpace(sReason) Then
            Return ToString()
        Else
            Return ToString(sReason)
        End If
    End Function


    Public Shared Function getFaultReasonsNotLocalizedString(ByVal item As FaultReasons, Optional ByVal sdefault As String = "N/A") As String
        Dim strReturn = sdefault
        Try
            Select Case item
                Case FaultReasons.E_DataConflict
                    strReturn = "The Data Has Changed Since You Started Editing!"
                Case FaultReasons.E_SQLException
                    strReturn = "An Unexpected SQL Error Has Occurred!  Check details for more information.   You should manually refresh your data to be sure you have the latest changes."
                Case FaultReasons.E_InvalidOperationException
                    strReturn = "We're sorry but your request is not valid."
                Case FaultReasons.E_UnExpected
                    strReturn = "An Unexpected Error Has Occurred!  Check details for more information.   You should manually refresh your data to be sure you have the latest changes."
                Case FaultReasons.E_DataValidationFailure
                    strReturn = "Data validation failure!  Your request could not be completed because it violates one or more business rules or data requirements."
                Case FaultReasons.E_DataAccessError
                    strReturn = "Database Access Error!  You should manually refresh your data to be sure you have the latest changes."
                Case FaultReasons.E_CreateRecordFailure
                    strReturn = "Cannot Create New Record"
                Case FaultReasons.E_DBLoginFailure
                    strReturn = "Cannot connect to database. Login failure."
                Case FaultReasons.E_AuthCodeFail
                    strReturn = "Warning!  Invalid Authentication Code.  Please make changes to your configuration settings, exit the application and try again.  Please remember that Authentication Codes are case sensitive.  If  you continue to have problems contact technical support."
                Case FaultReasons.E_AccessDenied
                    strReturn = "Access Denied!"
                Case FaultReasons.E_ProcessProcedureFailure
                    strReturn = "The system was unable to execute the requested procedure.   You should manually refresh your data to be sure you have the latest changes."
                Case FaultReasons.E_ProcessProcedureFailure
                    strReturn = "The system was unable to execute the requested procedure.   You should manually refresh your data to be sure you have the latest changes."
                Case FaultReasons.E_ReadAuthenticationCodeError
                    strReturn = "Read Authentication Code Error!"
                Case FaultReasons.E_AccessGranted
                    strReturn = "Access granted to the FreightMaster server."
                Case FaultReasons.E_ExportAPDataFailure
                    strReturn = "Your audit was successful but the AP Data could not be exported to the configured file.  Please check your AP Export File Settings."
                Case FaultReasons.E_FTPAPDataFailure
                    strReturn = "Your audit was successful and the AP Data was written to a file but the FTP Upload failed.  Your data will be uploaded again the next time you run the audit."
                Case FaultReasons.E_DBConnectionFailure
                    strReturn = "Cannot connect to database.  Database access failure."
                Case FaultReasons.E_DataAccessFailure
                    strReturn = "There was a problem processing your request to one or more data components."
            End Select

        Catch ex As System.ArgumentNullException
            'enum type or value is nothing so return default
            Return strReturn
        Catch ex As System.ArgumentException
            'the item is not valid so return default
            Return strReturn
        Catch ex As Exception
            Serilog.Log.Logger.Error(ex, "Error in getFaultReasonsNotLocalizedString")
        End Try

        Return strReturn
    End Function

    Public Shared Function getFaultInfoMsgsNotLocalizedString(ByVal item As FaultInfoMsgs, Optional ByVal sdefault As String = "N/A") As String
        Dim strReturn = sdefault
        Try
            Select Case item
                Case FaultInfoMsgs.E_SQLExceptionMSG
                    strReturn = "The warnings below were reported from the database.  This may help to correct the problem."
                Case FaultInfoMsgs.E_NoData
                    strReturn = "No data matches your selected criteria.  Please modify your filter(s) and try again."
                Case FaultInfoMsgs.E_UnExpectedMSG
                    strReturn = "Unexpected errors are rare and typically need to be addressed as soon as possible.  The details below may assist you in correcting the problem."
                Case FaultInfoMsgs.E_ApplicationException
                    strReturn = "The application could not complete your request.  See details for more information."
                Case FaultInfoMsgs.E_BatchDataNotFound
                    strReturn = "Some data in your batch could not be found.  These records have not been modified."
                Case FaultInfoMsgs.E_BatchProcessError
                    strReturn = "There was an error while executing the selected batch process."
                Case FaultInfoMsgs.E_CreateRecordFailure
                    strReturn = "Cannot Create New Record"
                Case FaultInfoMsgs.E_CreditHold
                    strReturn = "CREDIT HOLD"
                Case FaultInfoMsgs.E_CreditNotSetMsg
                    strReturn = "*WARNING* This Customer Is On Credit Hold Do Not Place Order!"
                Case FaultInfoMsgs.E_CreditOverLimit
                    strReturn = "*WARNING* This Customer Is Over Their Credit Limit Check With Accounting!"
                Case FaultInfoMsgs.E_CultureNotSupported
                    strReturn = ""
                Case FaultInfoMsgs.E_DataConflict
                    strReturn = "The Data Has Changed Since You Started Editing!"
                Case FaultInfoMsgs.E_DataInUse
                    strReturn = "The data is being used by one or more dependent records. Check the last error message for more information."
                Case FaultInfoMsgs.E_DBConnectionFailure
                    strReturn = "Cannot connect to database.  Database access failure."
                Case FaultInfoMsgs.E_DefaultCompRequired
                    strReturn = ""
                Case FaultInfoMsgs.E_FailedToExecute
                    strReturn = "Failed to execute your request on the database"
                Case FaultInfoMsgs.E_FileTooLarge
                    strReturn = "The file is too large or is not valid."
                Case FaultInfoMsgs.E_FTPConnectionError
                    strReturn = "A connection to the FTP server could not be established."
                Case FaultInfoMsgs.E_FTPUploadFailure
                    strReturn = "The FTP Upload failed to complete."
                Case FaultInfoMsgs.E_InvalidFileOrFolderName
                    strReturn = "The file or folder name is invalid or cannot be found."
                Case FaultInfoMsgs.E_InvalidFilterSelection
                    strReturn = "One or more of the filters you have provided is not valid for this search criteria. check for zero values or empty dates."
                Case FaultInfoMsgs.E_InvalidKeyField
                    strReturn = "One of more of the key fields already exists in the database. "
                Case FaultInfoMsgs.E_InvalidOperationException
                    strReturn = "We're sorry but your request is not valid."
                Case FaultInfoMsgs.E_InvalidPassword
                    strReturn = "Warning! The password is not valid.  You are not authorized to access the database."
                Case FaultInfoMsgs.E_InvalidRequest
                    strReturn = "Invalid Request"
                Case FaultInfoMsgs.E_InvalidUser
                    strReturn = "Warning! The user name is not valid.  You are not authorized to access the database."
                Case FaultInfoMsgs.E_LaneNumberRequired
                    strReturn = ""
                Case FaultInfoMsgs.E_LicenseViolation
                    strReturn = "Your License Has Expired ... Please Call Next Generation Logistics,Inc. For A Renewal!"
                Case FaultInfoMsgs.E_LicenseWarning
                    strReturn = "Your License Will Expire in 30 Days or Less ... Please Call Next Generation Logistics,Inc. For A Renewal!"
                Case FaultInfoMsgs.E_LoadScreenSecurityFailed
                    strReturn = "Cannot load application! There was a problem with the security settings for your profile.  Please contact your system administrator.  The application will now close.  "
                Case FaultInfoMsgs.E_MaxNbrOfCarriers
                    strReturn = "You have exceeded the maximum number of Carriers allowed.  Please contact your system administrator."
                Case FaultInfoMsgs.E_MaxUsersAssigned
                    strReturn = "Warning! FreightMaster software license exception.  You have reached the maximum number of users allowed according to you license agreement.  Please contact technical support.  Thank You!"
                Case FaultInfoMsgs.E_MessageServiceNotAvailable
                    strReturn = "We’re sorry but messaging and instant alerts are not currently available.  Please check your configuration settings."
                Case FaultInfoMsgs.E_NETAddressFail
                    strReturn = "Warning! FreightMaster server authentication failure.  Please call technical support and provide them with the following error code: NET_ADDRESS_FAILURE.  Thank You!"
                Case FaultInfoMsgs.E_NGLOptDeletePreviousSolutionFailed
                    strReturn = "Unable to delete the previous optimization solution data."
                Case FaultInfoMsgs.E_NGLOptLoadParameterSettingsFailed
                    strReturn = "Unable to load the optimization parameter settings."
                Case FaultInfoMsgs.E_NoLookUpAvailable
                    strReturn = "The selected look up view is not available."
                Case FaultInfoMsgs.E_NotAuthProcedure
                    strReturn = "You are not authorized to execute this procedure."
                Case FaultInfoMsgs.E_NotAuthReport
                    strReturn = "You are not authorized to execute this report."
                Case FaultInfoMsgs.E_NotAuthScreen
                    strReturn = "You are not authorized to access this screen."
                Case FaultInfoMsgs.E_NotAuthService
                    strReturn = "You are not authorized to access the requested service."
                Case FaultInfoMsgs.E_NotSupported
                    strReturn = "The selected operation is not supported using the current interface."
                Case FaultInfoMsgs.E_PathToLong
                    strReturn = "The specified path, file name, or both exceed the system-defined maximum length. Paths must be less than 248 characters, and file names must be less than 260 characters."
                Case FaultInfoMsgs.E_ProcessRunning
                    strReturn = "The selected procedure is already running.  Only one instance of this process is allowed to execute at one time.  Please wait for the previous process to complete and try again."
                Case FaultInfoMsgs.E_ReadAuthenticationCodeError
                    strReturn = "Read Authentication Code Error!"
                Case FaultInfoMsgs.E_RecordDeleted
                    strReturn = "Cannot save changes to the current record because it is no longer available in the database.  Someone may have deleted it."
                Case FaultInfoMsgs.E_SourceNotValid
                    strReturn = "The source object is not a valid data type for this operation."
                Case FaultInfoMsgs.E_UpdateNotAllowed
                    strReturn = "Updates to the selected record are not allowed.  Please delete the current  record then add a new record with the desired changes."
                Case FaultInfoMsgs.E_CannotUpdateTariffApproved
                    strReturn = "Cannot modify contracts once approved."
                Case FaultInfoMsgs.E_InvalidKeyFilterMetaData
                    strReturn = "One or more key record values are invalid or cannot be found."
                Case FaultInfoMsgs.E_AssignCarrierFailed
                    strReturn = "Cannot assign the desired transportation provider."
                Case FaultInfoMsgs.E_InvalidParentKeyField
                    strReturn = "A reference to the parent record is required."
                Case FaultInfoMsgs.E_InvalidRecordKeyField
                    strReturn = "A key reference to the record is missing"
                Case FaultInfoMsgs.E_InvalidParentOrRecordKeyField
                    strReturn = "A key reference to the record or the parent record is missing"
                Case FaultInfoMsgs.E_MethodOrProperyDepreciated
                    strReturn = "The method or property has been depreciated and is no longer available."
                Case FaultInfoMsgs.E_AccessDenied
                    strReturn = "Access Denied!"
                Case FaultInfoMsgs.E_CreateNewDependentRecordFailed
                    strReturn = "The automatic creation of dependent records failed. Please complete the process manually."
            End Select

        Catch ex As System.ArgumentNullException
            'enum type or value is nothing so return default
            Return strReturn
        Catch ex As System.ArgumentException
            'the item is not valid so return default
            Return strReturn
        Catch ex As Exception
            Serilog.Log.Logger.Error(ex, "Error in getFaultInfoMsgsNotLocalizedString")
        End Try

        Return strReturn
    End Function

    Public Shared Function getFaultDetailsKeyNotLocalizedString(ByVal item As FaultDetailsKey, Optional ByVal sdefault As String = "N/A") As String
        Dim strReturn = sdefault
        Try
            Select Case item
                Case FaultDetailsKey.E_NoDetails
                    strReturn = ""
                Case FaultDetailsKey.E_ExceptionMsgDetails
                    strReturn = "Application Exception: {0}"
                Case FaultDetailsKey.E_ServerMsgDetails
                    strReturn = "Server Message: {0}"
                Case FaultDetailsKey.E_ContractApporvedDetails
                    strReturn = "Approved {0}, By {1}"
                Case FaultDetailsKey.E_ContractRejectedDetails
                    strReturn = "Rejected {0}, By {1}"
                Case FaultDetailsKey.E_EquipMatPivotKeyFilterDetails
                    strReturn = "Cannot read carrier tariff contract rates because the key record {0} value {1} is not valid."
                Case FaultDetailsKey.E_EquipMatPivotAllKeyFilterDetails
                    strReturn = "Cannot read carreir tariff contract rates because none of the key record values are valid."
                Case FaultDetailsKey.E_EquipMatDetKeyFieldNewValidationDetails
                    strReturn = "Cannot save new Carrier Equipment Matrix Detail data.  The {0} value {1} is already in use for the selected matrix."
                Case FaultDetailsKey.E_OrderIdentificationDetailsWCNS
                    strReturn = "Book Control: {0} Order Number: {1} Sequence: {2} Pro: {3} CNS: {4}"
                Case FaultDetailsKey.E_OrderIdentificationDetails
                    strReturn = "Book Control: {0} Order Number: {1} Sequence: {2} Pro: {3}"
                Case FaultDetailsKey.E_CannotSaveProtectedDataDetails
                    strReturn = "Cannot save changes the {0} value {1} is protected and cannot be modified."
                Case FaultDetailsKey.E_CannotDeleteProtectedDataDetails
                    strReturn = "Cannot delete data the {0} value {1} is protected and cannot be modified."
                Case FaultDetailsKey.E_CannotSaveKeyAlreadyExists
                    strReturn = "Cannot save changes to {0}.  The unique key {1} value {2} already exists."
                Case FaultDetailsKey.E_InvalidClassException
                    strReturn = "Cannot process your request because the class {0} is not a valid {1} library."
                Case FaultDetailsKey.E_CannotSaveParentKeyRequired
                    strReturn = "Cannot save your changes because the {0} value {1} is required"
                Case FaultDetailsKey.E_NoTariffAvailable
                    strReturn = "A tariff is not available for the select route CNS: {0}."
                Case FaultDetailsKey.E_InvalidTranCode
                    strReturn = "The Trans Code {0} does not allow updates to transportation providers."
                Case FaultDetailsKey.E_CostsAreLocked
                    strReturn = "The costs are locked for book pro number: {0}."
                Case FaultDetailsKey.E_SQLWarningDetails
                    strReturn = "The procedure, {0}, returned the following warning: number {1} message {2}"
                Case FaultDetailsKey.E_SystemFaliedToGeneratedKeyField
                    strReturn = "The system could not generate a new {0}"
                Case FaultDetailsKey.E_FieldRequired
                    strReturn = "'The '{0}' is required and cannot be empty."
                Case FaultDetailsKey.E_RecordOnHold
                    strReturn = "The requested record is on hold and cannot be processed."
                Case FaultDetailsKey.E_CannotSaveRequiredValueDoesNotMatch
                    strReturn = "Cannot save changes to {0}. The key field {1} must be unique; the value {2} is already in use for AlphaCode {3}."
                Case FaultDetailsKey.E_CannotSaveKeyValuesAlreadyExist
                    strReturn = "Cannot save changes to {0}. A record with one or more of the following key values already exist: Keys {1} Values {2}."
                Case FaultDetailsKey.E_CannotSaveKeyFieldsRequired
                    strReturn = " Cannot save changes to {0}.  The following key fields are required: {1}."
                Case FaultDetailsKey.E_CannotDeleteRecordInUseDetails
                    strReturn = " Cannot delete data the {0} value {1} is being used and cannot be modified."
                Case FaultDetailsKey.E_CannotDeleteEquipCodeRecordInUseDetailsMany
                    'Added By LVV on 8/11/16 for v-7.0.5.110 Ticket #2323 
                    strReturn = "Cannot delete data the Equipment Code {0} is being used by {1} tariffs and cannot be modified."
                Case FaultDetailsKey.E_CannotDeleteEquipCodeRecordInUseDetails
                    'Added By LVV on 8/11/16 for v-7.0.5.110 Ticket #2323 
                    strReturn = "Cannot delete data the Equipment Code {0} is being used by the following tariffs and cannot be modified: {1}"
            End Select

        Catch ex As System.ArgumentNullException
            'enum type or value is nothing so return default
            Return strReturn
        Catch ex As System.ArgumentException
            'the item is not valid so return default
            Return strReturn
        Catch ex As Exception
            Serilog.Log.Logger.Error(ex, "Error in getFaultDetailsKeyNotLocalizedString")
        End Try

        Return strReturn
    End Function

    Public Shared Function getFaultReasonsEnumFromString(ByVal strEnum As String) As FaultReasons
        Dim enmVal As FaultReasons = FaultReasons.None
        [Enum].TryParse(strEnum, enmVal)
        Return enmVal
    End Function

    Public Shared Function getFaultInfoMsgsEnumFromString(ByVal strEnum As String) As FaultInfoMsgs
        Dim enmVal As FaultInfoMsgs = FaultInfoMsgs.None
        [Enum].TryParse(strEnum, enmVal)
        Return enmVal
    End Function

    Public Shared Function getFaultDetailsKeyEnumFromString(ByVal strEnum As String) As FaultDetailsKey
        Dim enmVal As FaultDetailsKey = FaultDetailsKey.None
        [Enum].TryParse(strEnum, enmVal)
        Return enmVal
    End Function

    ''' <summary>
    ''' Shared function used to format the Details using the not localized version of the text
    ''' and inserting Details List where provided.  The function also will return the not localized 
    ''' versions of Message, Details and Reason codes to the caller using the optional by reference 
    ''' parameters NotLocalizedMessage,NotLocalizedDetails,NotLocalizedReason
    ''' </summary>
    ''' <param name="Message"></param>
    ''' <param name="Details"></param>
    ''' <param name="DetailsList"></param>
    ''' <param name="Reason"></param>
    ''' <param name="NotLocalizedMessage"></param>
    ''' <param name="NotLocalizedDetails"></param>
    ''' <param name="NotLocalizedReason"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function getAlertInfoNotLocalized(ByVal Message As String, ByVal Details As String, DetailsList As List(Of String), ByVal Reason As String, Optional ByRef NotLocalizedMessage As String = "", Optional ByRef NotLocalizedDetails As String = "", Optional ByRef NotLocalizedReason As String = "") As String
        Dim strFormatted As String
        If Not String.IsNullOrWhiteSpace(Message) Then NotLocalizedMessage = getFaultInfoMsgsNotLocalizedString(getFaultInfoMsgsEnumFromString(Message), Message)
        If Not String.IsNullOrWhiteSpace(Details) Then NotLocalizedDetails = getFaultDetailsKeyNotLocalizedString(getFaultDetailsKeyEnumFromString(Details), Details)
        If Not String.IsNullOrWhiteSpace(Reason) Then NotLocalizedReason = getFaultReasonsNotLocalizedString(getFaultReasonsEnumFromString(Reason), Reason)
        If NotLocalizedDetails.Contains("{0}") AndAlso Not DetailsList Is Nothing AndAlso DetailsList.Count > 0 Then
            strFormatted = " " & String.Format(NotLocalizedDetails, DetailsList.ToArray())
        Else
            strFormatted = " " & NotLocalizedDetails & If(IsNothing(DetailsList), "", " " & String.Join(",", DetailsList))
        End If
        Return strFormatted
    End Function

End Class

<DataContract(), KnownType(GetType(SqlConflictFaultInfo)), KnownType(GetType(EntityConflictFaultInfo))> _
Public Class ConflictFaultInfo
    Private _Message As String
    <DataMember()> _
    Public Property Message() As String
        Get
            Return _Message
        End Get
        Set(ByVal value As String)
            _Message = value
        End Set
    End Property

    Private _TableName As String
    <DataMember()> _
    Public Property TableName() As String
        Get
            Return _TableName
        End Get
        Set(ByVal value As String)
            _TableName = value
        End Set
    End Property

    Private _ConflictData As New List(Of KeyValuePair(Of String, String))
    Friend Property ConflictData() As List(Of KeyValuePair(Of String, String))
        Get
            Return _ConflictData
        End Get
        Set(ByVal value As List(Of KeyValuePair(Of String, String)))
            _ConflictData = value
        End Set
    End Property

    Public Sub LogConflictDetails()

        For Each item As KeyValuePair(Of String, String) In ConflictData
            Console.WriteLine("{0} - {1}", item.Key, item.Value)
        Next item
    End Sub

    Public Sub LogConflictDetails(ByVal Message As String, ByVal Parameters As WCFParameters)
        'We now pass the message to this funciton so we do not need to parse the ConflictData
        'Dim strSeperator As String = " "
        'For Each item As KeyValuePair(Of String, String) In ConflictData
        '    Message &= strSeperator & String.Format("{0} - {1}", item.Key, item.Value)
        '    strSeperator = "; "
        'Next item
        Utilities.SaveAppError(Message, Parameters)
    End Sub

    Protected Function GetKeyValue(ByVal obj As Object) As String
        If TypeOf obj Is LTS.PalletType Then
            Return String.Format("ID: {0} PalletType: {1} ", (CType(obj, LTS.PalletType)).ID, (CType(obj, LTS.PalletType)).PalletType)
        Else
            Throw New Exception("Unexpected entity type: " & obj.GetType().Name)
        End If
    End Function
End Class

<DataContract()> _
Public Class SqlConflictFaultInfo
    Inherits ConflictFaultInfo


    Public Sub New()
        MyBase.new()
    End Sub

    Public Sub New(ByVal conflicts As List(Of KeyValuePair(Of String, String)))
        Me.ConflictData = conflicts
        Me.Message = GetConflictData(Me.ConflictData)
    End Sub


    Public Sub New(ByVal conflicts As ChangeConflictCollection)

        ' Get each entity name and value
        ConflictData = ( _
          From c In conflicts _
          Select New KeyValuePair(Of String, String)(c.Object.GetType().Name, GetKeyValue(c.Object))).ToList()
        Message = GetConflictData(ConflictData)
    End Sub

    Public Sub New(ByRef db As System.Data.Linq.DataContext)
        For Each occ As ObjectChangeConflict In db.ChangeConflicts
            Dim metatable As System.Data.Linq.Mapping.MetaTable = db.Mapping.GetTable(occ.Object.GetType)
            Dim entityInConflict As Object = occ.Object
            Me.TableName = metatable.TableName
            For Each mcc As MemberChangeConflict In occ.MemberConflicts
                Dim currVal = mcc.CurrentValue
                Dim origVal = mcc.OriginalValue
                Dim databaseVal = mcc.DatabaseValue
                If currVal Is Nothing Then currVal = "{NULL}"
                If origVal Is Nothing Then origVal = "{NULL}"
                If databaseVal Is Nothing Then databaseVal = "{NULL}"
                Dim MemberName As String = mcc.Member.Name.ToString
                If Right(MemberName, 7).ToLower <> "updated" Then
                    'we ignore the updated time stamp field as we know this is already different
                    Dim strMsg As String = "current value: " & currVal.ToString & "; " & "original value: " & origVal.ToString & "; " & "database value: " & databaseVal.ToString
                    Dim oConflicts As New KeyValuePair(Of String, String)(mcc.Member.Name, strMsg)
                    Me.ConflictData.Add(oConflicts)
                End If
            Next
        Next
        Message = GetConflictData(ConflictData)
    End Sub

    Private Function GetConflictData(ByVal conflictData As List(Of KeyValuePair(Of String, String))) As String
        Dim sb As New StringBuilder()
        For Each item As KeyValuePair(Of String, String) In conflictData
            sb.Append(String.Format("{0}: {1}{2}", item.Key, item.Value, vbCrLf))
        Next item
        Return sb.ToString()
    End Function
End Class

<DataContract()> _
Public Class EntityConflictFaultInfo
    Inherits ConflictFaultInfo
    Public Sub New(ByVal conflicts As ReadOnlyCollection(Of ObjectStateEntry))
        ' Get each entity name and value
        ConflictData = ( _
          From c In conflicts _
          Select New KeyValuePair(Of String, String)(c.Entity.GetType().Name, GetKeyValue(c.Entity))).ToList()
        Message = GetConflictData(ConflictData)
    End Sub

    Private Function GetConflictData(ByVal conflictData As List(Of KeyValuePair(Of String, String))) As String
        Dim sb As New StringBuilder()
        For Each item As KeyValuePair(Of String, String) In conflictData
            sb.Append(String.Format("{0}: {1}", item.Key, item.Value))
        Next item
        Return sb.ToString()
    End Function
End Class

