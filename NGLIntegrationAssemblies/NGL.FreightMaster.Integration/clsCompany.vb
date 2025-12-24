Imports System.Data.SqlClient
Imports Ngl.FreightMaster.Integration.Configuration
Imports DTran = Ngl.Core.Utility.DataTransformation
Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects
Imports LTS = Ngl.FreightMaster.Data.LTS
Imports DAL = Ngl.FreightMaster.Data
Imports BLL = Ngl.FM.BLL
Imports System.ServiceModel

<Serializable()> _
Public Class clsCompany : Inherits clsDownload

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

    Private _oWCfParameters As DAL.WCFParameters
    Public Property oWCFParameters() As DAL.WCFParameters
        Get
            If (_oWCfParameters Is Nothing) Then
                _oWCfParameters = New DAL.WCFParameters()
                With _oWCfParameters
                    .UserName = "System Download"
                    .Database = Me.Database
                    .DBServer = Me.DBServer
                    .ConnectionString = Me.ConnectionString
                    .WCFAuthCode = "NGLSystem"
                    .ValidateAccess = False
                End With
            End If
            Return _oWCfParameters
        End Get
        Set(ByVal value As DAL.WCFParameters)
            _oWCfParameters = value
        End Set
    End Property

    Private _oPCMiler As BLL.NGLPCMilerBLL
    Public Property oPCMiler() As BLL.NGLPCMilerBLL
        Get
            If _oPCMiler Is Nothing Then
                _oPCMiler = New BLL.NGLPCMilerBLL(oWCFParameters)
            End If
            Return _oPCMiler
        End Get
        Set(ByVal value As BLL.NGLPCMilerBLL)
            _oPCMiler = value
        End Set
    End Property

    Public Function getDataSet() As CompanyData
        Return New CompanyData
    End Function


    Public Function ProcessObjectData( _
                    ByVal oCompanies() As clsCompanyHeaderObject, _
                    ByVal oContacts() As clsCompanyContactObject, _
                    ByVal strConnection As String, _
                    Optional ByVal oCalendar() As clsCompanyCalendarObject = Nothing) As ProcessDataReturnValues
        Dim intRet As ProcessDataReturnValues = ProcessDataReturnValues.nglDataIntegrationFailure
        Dim oHTable As New CompanyData.CompanyHeaderDataTable
        Dim oDTable As New CompanyData.CompanyContactDataTable
        Dim oCTable As New CompanyData.CompanyCalendarDataTable
        Dim dtVal As Date

        Try
            For Each oItem As clsCompanyHeaderObject In oCompanies
                Try
                    Dim oRow As CompanyData.CompanyHeaderRow = oHTable.NewCompanyHeaderRow
                    With oRow
                        .CompNumber = Left(oItem.CompNumber, 50)
                        .CompName = Left(oItem.CompName, 40)
                        .CompNatName = Left(oItem.CompNatName, 40)
                        .CompNatNumber = oItem.CompNatNumber
                        .CompStreetAddress1 = Left(oItem.CompStreetAddress1, 40)
                        .CompStreetAddress2 = Left(oItem.CompStreetAddress2, 40)
                        .CompStreetAddress3 = Left(oItem.CompStreetAddress3, 40)
                        .CompStreetCity = Left(oItem.CompStreetCity, 25)
                        .CompStreetCountry = Left(oItem.CompStreetCountry, 30)
                        .CompStreetState = Left(oItem.CompStreetState, 2)
                        .CompStreetZip = Left(oItem.CompStreetZip, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
                        .CompMailAddress1 = Left(oItem.CompMailAddress1, 40)
                        .CompMailAddress2 = Left(oItem.CompMailAddress2, 40)
                        .CompMailAddress3 = Left(oItem.CompMailAddress3, 40)
                        .CompMailCity = Left(oItem.CompMailCity, 25)
                        .CompMailCountry = Left(oItem.CompMailCountry, 30)
                        .CompMailState = Left(oItem.CompMailState, 2)
                        .CompMailZip = Left(oItem.CompMailZip, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
                        .CompWeb = Left(oItem.CompWeb, 255)
                        .CompEmail = Left(oItem.CompEmail, 30)
                        .CompDirections = Left(oItem.CompDirections, 500)
                        .CompAbrev = Left(oItem.CompAbrev, 3)
                        .CompActive = oItem.CompActive
                        .CompNEXTrack = oItem.CompNEXTrack
                        .CompNEXTStopAcctNo = Left(oItem.CompNEXTStopAcctNo, 50)
                        .CompNEXTStopPsw = Left(oItem.CompNEXTStopPsw, 50)
                        .CompNextstopSubmitRFP = oItem.CompNextstopSubmitRFP
                        .CompFAAShipID = Left(oItem.CompFAAShipID, 50)
                        If validateDateWS(oItem.CompFAAShipDate, dtVal) Then
                            .CompFAAShipDate = exportDateToString(dtVal.ToString)
                        End If
                        .CompFinDuns = Left(oItem.CompFinDuns, 11)
                        .CompFinTaxID = Left(oItem.CompFinTaxID, 20)
                        .CompFinPaymentForm = oItem.CompFinPaymentForm
                        .CompFinSIC = Left(oItem.CompFinSIC, 8)
                        .CompFinPaymentDiscount = oItem.CompFinPaymentDiscount
                        .CompFinPaymentDays = oItem.CompFinPaymentDays
                        .CompFinPaymentNetDays = oItem.CompFinPaymentNetDays
                        .CompFinCommTerms = Left(oItem.CompFinCommTerms, 15)
                        .CompFinCommTermsPer = oItem.CompFinCommTermsPer
                        .CompFinCreditLimit = oItem.CompFinCreditLimit
                        .CompFinCreditUsed = oItem.CompFinCreditUsed
                        .CompFinInvPrnCode = oItem.CompFinInvPrnCode
                        .CompFinInvEMailCode = oItem.CompFinInvEMailCode
                        .CompFinCurType = oItem.CompFinCurType
                        .CompFinFBToleranceHigh = oItem.CompFinFBToleranceHigh
                        .CompFinFBToleranceLow = oItem.CompFinFBToleranceLow
                        If validateDateWS(oItem.CompFinCustomerSince, dtVal) Then
                            .CompFinCustomerSince = exportDateToString(dtVal.ToString)
                        End If
                        .CompFinCardType = Left(oItem.CompFinCardType, 50)
                        .CompFinCardName = Left(oItem.CompFinCardName, 50)
                        .CompFinCardExpires = Left(oItem.CompFinCardExpires, 50)
                        .CompFinCardAuthorizor = Left(oItem.CompFinCardAuthorizor, 50)
                        .CompFinCardAuthPassword = Left(oItem.CompFinCardAuthPassword, 50)
                        .CompFinUseImportFrtCost = oItem.CompFinUseImportFrtCost
                        .CompFinBkhlFlatFee = oItem.CompFinBkhlFlatFee
                        .CompFinBkhlCostPerc = oItem.CompFinBkhlCostPerc
                        .CompLatitude = oItem.CompLatitude
                        .CompLongitude = oItem.CompLongitude
                        .CompMailTo = Left(oItem.CompMailTo, 500)
                    End With
                    oHTable.AddCompanyHeaderRow(oRow)
                Catch ex As FaultException(Of DAL.SqlFaultInfo)
                    'In the new 70 project this is where we will capture the invalid CompanyNumber 
                    Throw
                Catch ex As Exception
                    LogException("Process Object Data Partial Failure", "Could not import Company Number " & oItem.CompNumber, AdminEmail, ex, "NGL.FreightMaster.Integration.clsCompany.ProcessObjectData Failure")
                End Try

            Next
            Try
                For Each oDetail As clsCompanyContactObject In oContacts
                    Dim oRow As CompanyData.CompanyContactRow = oDTable.NewCompanyContactRow
                    With oRow
                        .CompNumber = Left(oDetail.CompNumber, 50)
                        .CompContName = Left(oDetail.CompContName, 25)
                        .CompContTitle = Left(oDetail.CompContTitle, 25)
                        .CompContPhone = Left(oDetail.CompContPhone, 15)
                        .CompContFax = Left(oDetail.CompContFax, 15)
                        .CompCont800 = Left(oDetail.CompCont800, 15)
                        .CompContEmail = Left(oDetail.CompContEmail, 50)
                    End With
                    oDTable.AddCompanyContactRow(oRow)
                Next
            Catch ex As Exception
                LogException("Process Object Data Warning (import not affected)", "Invalid or missing company contact information", AdminEmail, ex, "NGL.FreightMaster.Integration.clsCompany.ProcessObjectData Warning (import not affected).")
            End Try

            Try
                If Not oCalendar Is Nothing Then
                    For Each oCalendarItem As clsCompanyCalendarObject In oCalendar
                        Dim oRow As CompanyData.CompanyCalendarRow = oCTable.NewCompanyCalendarRow
                        With oRow
                            .CompNumber = Left(oCalendarItem.CompNumber, 50)
                            .Month = oCalendarItem.Month
                            .Day = oCalendarItem.Day
                            .Open = oCalendarItem.Open
                            If ValidateTimeWS(oCalendarItem.StartTime) Then
                                .StartTime = oCalendarItem.StartTime
                            End If
                            If ValidateTimeWS(oCalendarItem.EndTime) Then
                                .EndTime = oCalendarItem.EndTime
                            End If
                            .IsHoliday = oCalendarItem.IsHoliday
                        End With
                        oCTable.AddCompanyCalendarRow(oRow)
                    Next
                End If
            Catch ex As Exception
                LogException("Process Object Data Warning (import not affected)", "Invalid or missing company calendar information", AdminEmail, ex, "NGL.FreightMaster.Integration.clsCompany.ProcessObjectData Warning (import not affected).")
            End Try

            intRet = ProcessData(oHTable, oDTable, strConnection, oCTable)
        Catch ex As Exception
            LogException("Process Object Data Failure", "Company import system error", AdminEmail, ex, "NGL.FreightMaster.Integration.clsCompany.ProcessObjectData Failure")
        End Try


        Return intRet


    End Function

    ''' <summary>
    ''' Import Company information
    ''' </summary>
    ''' <param name="oItem"></param>
    ''' <param name="oCompWCFData"></param>
    ''' <param name="oAllowUpdatePar"></param>
    ''' <param name="oCurrencyDictionary"></param>
    ''' <param name="oCurrencyWCFData"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.4.0.003 on 07/19/2021  added logic for trimble API for Lat Long
    ''' </remarks>
    Public Function ProcessCompanyHeader70(ByRef oItem As clsCompanyHeaderObject70,
                                           ByRef oCompWCFData As DAL.NGLCompData,
                                           ByRef oAllowUpdatePar As AllowUpdateParameters,
                                           ByRef oCurrencyDictionary As Dictionary(Of String, Integer),
                                           ByRef oCurrencyWCFData As DAL.NGLCurrencyData) As ProcessCompanyHeader70Result
        Dim intRet As ProcessDataReturnValues
        Dim dtVal As Date
        Dim result As New ProcessCompanyHeader70Result
        result.successFlag = False

        Try
            Dim strValMsg As String = ""
            Dim strDatakey As String = ""
            'Use the new method GetCompFilteredByLegalEntity to return a DTO.Comp object and store it as oRow
            Dim oRow As DTO.Comp = oCompWCFData.GetCompFilteredByLegalEntity(oItem.CompLegalEntity, oItem.CompAlphaCode, oItem.CompNumber)
            Dim insertFlag As Boolean
            If oRow Is Nothing Then oRow = New DTO.Comp
            If oRow.CompControl = 0 Then
                insertFlag = True
                If Not oCompWCFData.ValidateCompBeforeInsert(oItem.CompNumber,
                                                             oItem.CompName,
                                                             oItem.CompLegalEntity,
                                                             oItem.CompAlphaCode,
                                                             oItem.CompAbrev,
                                                             strValMsg) Then
                    result.processCompanyHeaderFailed(oItem.CompName, oItem.CompNumber, oItem.CompAlphaCode, oItem.CompLegalEntity)
                    result.strRetMsg = "Cannot insert new company where " & buildDataKeyString(result.dicInvalidKeys) & " because: " & strValMsg & "."
                    Me.LastError = result.strRetMsg
                    AddToGroupEmailMsg(result.strRetMsg)
                    Return result
                End If
            Else
                insertFlag = False
                'bug fixed by RHR 2/18/14. the test would always fail if the compnumber was empty or zero even if the
                'Alpha Code and Legal Entity were used to look up the record.
                'So, when performing an update the key fields have been evaluated in the query above
                'we simply need to update the record to match what was returned in oRow (like CompNumber)
                oItem.CompNumber = oRow.CompNumber
                oItem.CompAlphaCode = oRow.CompAlphaCode
                oItem.CompLegalEntity = oRow.CompLegalEntity
                oItem.CompAbrev = oRow.CompAbrev
                'we do not need to perform this validation because we are forcing the 
                'system to use the keys that already exist
                'If Not oCompWCFData.ValidateCompBeforeUpdate(oRow.CompControl,
                '                                             oItem.CompNumber,
                '                                             oItem.CompName,
                '                                             oItem.CompLegalEntity,
                '                                             oItem.CompAlphaCode,
                '                                             oItem.CompAbrev,
                '                                             strValMsg) Then
                '    result.processCompanyHeaderFailed(oItem.CompName, oItem.CompNumber, oItem.CompAlphaCode, oItem.CompLegalEntity)
                '    AddToGroupEmailMsg("Cannot update company where " & buildDataKeyString(result.dicInvalidKeys) & " because: " & strValMsg & ".")
                '    Return result
                'End If
            End If
            oAllowUpdatePar.insertFlag = insertFlag
            result.insertFlag = insertFlag
            With oRow
                .CompLegalEntity = oItem.CompLegalEntity
                .CompNumber = oItem.CompNumber
                .CompAlphaCode = oItem.CompAlphaCode
                If AllowUpdate("CompAbrev", oAllowUpdatePar) Then
                    oRow.CompAbrev = oItem.CompAbrev
                End If

                If AllowUpdate("CompName", oAllowUpdatePar) Then .CompName = oItem.CompName
                If AllowUpdate("CompNatName", oAllowUpdatePar) Then .CompNatName = oItem.CompNatName
                If AllowUpdate("CompNatNumber", oAllowUpdatePar) Then .CompNatNumber = oItem.CompNatNumber
                If AllowUpdate("CompStreetAddress1", oAllowUpdatePar) Then .CompStreetAddress1 = oItem.CompStreetAddress1
                If AllowUpdate("CompStreetAddress2", oAllowUpdatePar) Then .CompStreetAddress2 = oItem.CompStreetAddress2
                If AllowUpdate("CompStreetAddress3", oAllowUpdatePar) Then .CompStreetAddress3 = oItem.CompStreetAddress3
                If AllowUpdate("CompStreetCity", oAllowUpdatePar) Then .CompStreetCity = oItem.CompStreetCity
                If AllowUpdate("CompStreetCountry", oAllowUpdatePar) Then .CompStreetCountry = oItem.CompStreetCountry
                If AllowUpdate("CompStreetState", oAllowUpdatePar) Then .CompStreetState = oItem.CompStreetState
                If AllowUpdate("CompStreetZip", oAllowUpdatePar) Then .CompStreetZip = oItem.CompStreetZip
                If AllowUpdate("CompMailAddress1", oAllowUpdatePar) Then .CompMailAddress1 = oItem.CompMailAddress1
                If AllowUpdate("CompMailAddress2", oAllowUpdatePar) Then .CompMailAddress2 = oItem.CompMailAddress2
                If AllowUpdate("CompMailAddress3", oAllowUpdatePar) Then .CompMailAddress3 = oItem.CompMailAddress3
                If AllowUpdate("CompMailCity", oAllowUpdatePar) Then .CompMailCity = oItem.CompMailCity
                If AllowUpdate("CompMailCountry", oAllowUpdatePar) Then .CompMailCountry = oItem.CompMailCountry
                If AllowUpdate("CompMailState", oAllowUpdatePar) Then .CompMailState = oItem.CompMailState
                If AllowUpdate("CompMailZip", oAllowUpdatePar) Then .CompMailZip = oItem.CompMailZip
                If AllowUpdate("CompWeb", oAllowUpdatePar) Then .CompWeb = oItem.CompWeb
                If AllowUpdate("CompEmail", oAllowUpdatePar) Then .CompEmail = oItem.CompEmail
                If AllowUpdate("CompDirections", oAllowUpdatePar) Then .CompDirections = oItem.CompDirections
                If AllowUpdate("CompActive", oAllowUpdatePar) Then .CompActive = oItem.CompActive
                If AllowUpdate("CompNEXTrack", oAllowUpdatePar) Then .CompNEXTrack = oItem.CompNEXTrack
                If AllowUpdate("CompNEXTStopAcctNo", oAllowUpdatePar) Then .CompNEXTStopAcctNo = oItem.CompNEXTStopAcctNo
                If AllowUpdate("CompNEXTStopPsw", oAllowUpdatePar) Then .CompNEXTStopPsw = oItem.CompNEXTStopPsw
                If AllowUpdate("CompNextstopSubmitRFP", oAllowUpdatePar) Then .CompNextstopSubmitRFP = oItem.CompNextstopSubmitRFP
                If AllowUpdate("CompFAAShipID", oAllowUpdatePar) Then .CompFAAShipID = oItem.CompFAAShipID
                If validateDateWS(oItem.CompFAAShipDate, dtVal) Then
                    .CompFAAShipDate = exportDateToString(dtVal.ToString)
                End If
                If AllowUpdate("CompFinDuns", oAllowUpdatePar) Then .CompFinDuns = oItem.CompFinDuns
                If AllowUpdate("CompFinTaxID", oAllowUpdatePar) Then .CompFinTaxID = oItem.CompFinTaxID
                If AllowUpdate("CompFinPaymentForm", oAllowUpdatePar) Then .CompFinPaymentForm = oItem.CompFinPaymentForm
                If AllowUpdate("CompFinSIC", oAllowUpdatePar) Then .CompFinSIC = oItem.CompFinSIC
                If AllowUpdate("CompFinPaymentDiscount", oAllowUpdatePar) Then .CompFinPaymentDiscount = oItem.CompFinPaymentDiscount
                If AllowUpdate("CompFinPaymentDays", oAllowUpdatePar) Then .CompFinPaymentDays = oItem.CompFinPaymentDays
                If AllowUpdate("CompFinPaymentNetDays", oAllowUpdatePar) Then .CompFinPaymentNetDays = oItem.CompFinPaymentNetDays
                If AllowUpdate("CompFinCommTerms", oAllowUpdatePar) Then .CompFinCommTerms = oItem.CompFinCommTerms
                If AllowUpdate("CompFinCommTermsPer", oAllowUpdatePar) Then .CompFinCommTermsPer = oItem.CompFinCommTermsPer
                If AllowUpdate("CompFinCreditLimit", oAllowUpdatePar) Then .CompFinCreditLimit = oItem.CompFinCreditLimit
                If AllowUpdate("CompFinCreditUsed", oAllowUpdatePar) Then .CompFinCreditUsed = oItem.CompFinCreditUsed
                If AllowUpdate("CompFinInvPrnCode", oAllowUpdatePar) Then .CompFinInvPrnCode = oItem.CompFinInvPrnCode
                If AllowUpdate("CompFinInvEMailCode", oAllowUpdatePar) Then .CompFinInvEMailCode = oItem.CompFinInvEMailCode
                If AllowUpdate("CompFinFBToleranceHigh", oAllowUpdatePar) Then .CompFinFBToleranceHigh = oItem.CompFinFBToleranceHigh
                If AllowUpdate("CompFinFBToleranceLow", oAllowUpdatePar) Then .CompFinFBToleranceLow = oItem.CompFinFBToleranceLow
                If Not String.IsNullOrEmpty(oItem.CompCurrencyType) AndAlso oItem.CompCurrencyType.Trim.Length > 0 Then
                    'Lookup the dbo.Currency.CurrencyType using oItem.Comp.CurrencyType and store the results in CompFinCurType
                    'If inserting a new record set the default value of CompFinCurType to USD
                    If oCurrencyDictionary.ContainsKey(oItem.CompCurrencyType) Then
                        .CompFinCurType = oCurrencyDictionary(oItem.CompCurrencyType)
                    Else
                        Try
                            Dim oCurrency = oCurrencyWCFData.GetCurrenciesFiltered(oItem.CompCurrencyType)
                            If Not oCurrency Is Nothing AndAlso oCurrency.Count > 0 AndAlso oCurrency(0).ID > 0 Then
                                oCurrencyDictionary.Add(oItem.CompCurrencyType, oCurrency(0).ID)
                                .CompFinCurType = oCurrency(0).ID
                            Else
                                .CompFinCurType = oCurrencyDictionary("USD")
                                AddToGroupEmailMsg("Currency Type Not Found, " & oItem.CompCurrencyType & ", could not be found for Company " & oItem.CompName & "; using USD by default.")
                                intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
                            End If
                        Catch ex As Exception
                            AddToGroupEmailMsg("Currency Type Not Found, " & oItem.CompCurrencyType & ", could not be found for Company " & oItem.CompName & "; using USD by default.")
                            intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
                        End Try
                    End If
                Else
                    If insertFlag Then
                        .CompFinCurType = oCurrencyDictionary("USD")
                    End If
                End If
                If validateDateWS(oItem.CompFinCustomerSince, dtVal) Then
                    .CompFinCustomerSince = exportDateToString(dtVal.ToString)
                End If
                If AllowUpdate("CompFinCardType", oAllowUpdatePar) Then .CompFinCardType = oItem.CompFinCardType
                If AllowUpdate("CompFinCardName", oAllowUpdatePar) Then .CompFinCardName = oItem.CompFinCardName
                If AllowUpdate("CompFinCardExpires", oAllowUpdatePar) Then .CompFinCardExpires = oItem.CompFinCardExpires
                If AllowUpdate("CompFinCardAuthorizor", oAllowUpdatePar) Then .CompFinCardAuthorizor = oItem.CompFinCardAuthorizor
                If AllowUpdate("CompFinCardAuthPassword", oAllowUpdatePar) Then .CompFinCardAuthPassword = oItem.CompFinCardAuthPassword
                If AllowUpdate("CompFinUseImportFrtCost", oAllowUpdatePar) Then .CompFinUseImportFrtCost = oItem.CompFinUseImportFrtCost
                If AllowUpdate("CompFinBkhlFlatFee", oAllowUpdatePar) Then .CompFinBkhlFlatFee = oItem.CompFinBkhlFlatFee
                If AllowUpdate("CompFinBkhlCostPerc", oAllowUpdatePar) Then .CompFinBkhlCostPerc = oItem.CompFinBkhlCostPerc
                If (AllowUpdate("CompStreetZip", oAllowUpdatePar) AndAlso AllowUpdate("CompLatitude", oAllowUpdatePar) AndAlso AllowUpdate("CompLongitude", oAllowUpdatePar)) Or insertFlag Then
                    'Begin Modified by RHR for v-8.4.0.003 on 07/19/2021 
                    If calcLatLong80(oItem.CompNumber, oItem.CompLatitude, oItem.CompLongitude, oItem.CompStreetAddress1, oItem.CompStreetCity, oItem.CompStreetState, oItem.CompStreetZip, oItem.CompStreetCountry) Then
                        .CompLatitude = oItem.CompLatitude
                        .CompLongitude = oItem.CompLongitude
                    End If
                    ' Removed  by RHR for v-8.4.0.003 on 07/19/2021 
                    'If calcLatLong70(oItem.CompNumber, oItem.CompStreetZip, oItem.CompLatitude, oItem.CompLongitude) Then
                    '    .CompLatitude = oItem.CompLatitude
                    '    .CompLongitude = oItem.CompLongitude
                    'End If
                    'End Modified by RHR for v-8.4.0.003 on 07/19/2021 
                End If
                If AllowUpdate("CompMailTo", oAllowUpdatePar) Then .CompMailTo = oItem.CompMailTo
                If AllowUpdate("CompTimeZone", oAllowUpdatePar) Then .CompTimeZone = oItem.CompTimeZone
                If AllowUpdate("CompRailStationName", oAllowUpdatePar) Then .CompRailStationName = oItem.CompRailStationName
                If AllowUpdate("CompRailSPLC", oAllowUpdatePar) Then .CompRailSPLC = oItem.CompRailSPLC
                If AllowUpdate("CompRailFSAC", oAllowUpdatePar) Then .CompRailFSAC = oItem.CompRailFSAC
                If AllowUpdate("CompRail333", oAllowUpdatePar) Then .CompRail333 = oItem.CompRail333
                If AllowUpdate("CompRailR260", oAllowUpdatePar) Then .CompRailR260 = oItem.CompRailR260
                If AllowUpdate("CompIsTransLoad", oAllowUpdatePar) Then .CompIsTransLoad = oItem.CompIsTransLoad
                If AllowUpdate("CompUser1", oAllowUpdatePar) Then .CompUser1 = oItem.CompUser1
                If AllowUpdate("CompUser2", oAllowUpdatePar) Then .CompUser2 = oItem.CompUser2
                If AllowUpdate("CompUser3", oAllowUpdatePar) Then .CompUser3 = oItem.CompUser3
                If AllowUpdate("CompUser4", oAllowUpdatePar) Then .CompUser4 = oItem.CompUser4
            End With
            'Update or Create new Record
            If insertFlag Then
                oRow.TrackingState = Ngl.Core.ChangeTracker.TrackingInfo.Created
                oRow = oCompWCFData.CreateRecord(oRow)
                result.successFlag = True
            Else
                oRow.TrackingState = Ngl.Core.ChangeTracker.TrackingInfo.Updated
                oRow = oCompWCFData.UpdateRecord(oRow)
                result.successFlag = True
            End If
            If Not oRow Is Nothing Then result.CompControl = oRow.CompControl
        Catch ex As FaultException(Of DAL.SqlFaultInfo)
            result.processCompanyHeaderFailed(oItem.CompName, oItem.CompNumber, oItem.CompAlphaCode, oItem.CompLegalEntity)
            If ex.Detail.Message = "E_InvalidKeyFilterMetaData" Then
                AddToGroupEmailMsg(String.Format("Cannot save changes to {0}. The key field {1} must be unique; the value {2} is already in use for AlphaCode {3}.", ex.Detail.DetailsList.ToArray()))
            ElseIf ex.Detail.Message = "E_InvalidRecordKeyField" Then
                AddToGroupEmailMsg(String.Format("The '{0}' is required and cannot be empty.", ex.Detail.DetailsList.ToArray()))
            Else
                AddToGroupEmailMsg("Could not import Company where " & buildDataKeyString(result.dicInvalidKeys) & " because: " & ex.Message & ".")
            End If
        Catch ex As Exception
            result.processCompanyHeaderFailed(oItem.CompName, oItem.CompNumber, oItem.CompAlphaCode, oItem.CompLegalEntity)
            AddToGroupEmailMsg("Could not import Company where " & buildDataKeyString(result.dicInvalidKeys) & " because: " & ex.Message & ".")
        End Try

        Return result
    End Function

    ''' <summary>
    ''' Takes the Company Information from a webservice xml data and formats the results in preparation for WCF create and update methods
    ''' </summary>
    ''' <param name="oCompanies"></param>
    ''' <param name="oContacts"></param>
    ''' <param name="strConnection"></param>
    ''' <param name="oCalendar"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.2.1.008 on 06/03/2020 add logic to read parameter to map alpha code to company name (alt. Location Code)
    ''' </remarks>
    Public Function ProcessObjectData70(
                    ByVal oCompanies As List(Of clsCompanyHeaderObject70),
                    ByVal oContacts As List(Of clsCompanyContactObject70),
                    ByVal strConnection As String,
                    ByVal oCalendar As List(Of clsCompanyCalendarObject70)) As clsIntegrationUpdateResults

        Dim finalResult As New clsIntegrationUpdateResults
        Dim intRet As ProcessDataReturnValues = ProcessDataReturnValues.nglDataIntegrationComplete
        Dim dtVal As Date
        Dim resultObject = New ProcessCompanyHeader70Result
        Try
            oWCFParameters.ConnectionString = strConnection
            Dim strValMsg As String = ""
            Dim strDatakey As String = ""
            Dim oCompWCFData = New DAL.NGLCompData(oWCFParameters)
            Dim oCompContWCFData = New DAL.NGLCompContData(oWCFParameters)
            Dim oCompCalWCFData = New DAL.NGLCompCalData(oWCFParameters)
            Dim oCurrencyWCFData = New DAL.NGLCurrencyData(oWCFParameters)
            Dim oCurrencyDictionary As New Dictionary(Of String, Integer)
            oCurrencyDictionary.Add("USD", 1)
            Dim dictionaryList As New List(Of Dictionary(Of String, String))
            'Modified by RHR 2/17/14 by setting success to false we can test if all of the records fail
            'by setting success to true if any company was imported,  this indicates at least one record
            'was processed (see below)
            Dim successFlag As Boolean = False

            'Create a new allow update parameter object
            Dim oAllowUpdatePar As New AllowUpdateParameters With {.WCFParameters = oWCFParameters, .ImportType = IntegrationTypes.Company}
            'Header
            For Each oItem As clsCompanyHeaderObject70 In oCompanies
                If Not oItem Is Nothing Then
                    'Modified by RHR for v-8.2.1.008 on 06/03/2020 add logic to read parameter to map alpha code to company name (alt. Location Code)
                    If Me.getParValue("UseLocCodeForTMSCompName", 0) = 1 And Not String.IsNullOrWhiteSpace(oItem.CompAlphaCode) Then
                        oItem.CompName = oItem.CompAlphaCode
                    End If
                    resultObject = ProcessCompanyHeader70(oItem, oCompWCFData, oAllowUpdatePar, oCurrencyDictionary, oCurrencyWCFData)
                    If resultObject.successFlag Then
                        'Modified  by RHR 7/15/2015 we always return the control numbers for testing and debugging 
                        'not just on insert
                        'If resultObject.insertFlag Then finalResult.ControlNumbers.Add(resultObject.CompControl)
                        finalResult.ControlNumbers.Add(resultObject.CompControl)
                        'at least one company record was processed so set success = true
                        successFlag = True
                    Else
                        dictionaryList.Add(resultObject.dicInvalidKeys)
                        'we never change the value of successFlag on failure 
                        'because the default is already false, once it is true 
                        'we never change it back to false

                        'Modified by RHR 2/18/14  the default value of intRet is ProcessDataReturnValues.nglDataIntegrationComplete
                        'We only need to update this if one of the company imports fails
                        intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
                    End If
                    intRet = resultObject.intRet
                Else
                    AddToGroupEmailMsg("One of the company header records was null or empty and could not be processed.")
                    intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
                End If
            Next
            If successFlag = False Then
                'this only happens if all companies fail to import (no need to continue with details)
                finalResult.ReturnValue = ProcessDataReturnValues.nglDataIntegrationFailure
                Return finalResult
            End If

            'Details
            If Not oContacts Is Nothing AndAlso oContacts.Count > 0 Then
                For Each oDetail As clsCompanyContactObject70 In oContacts
                    If Not oDetail Is Nothing Then
                        Try
                            'For details reset the insertFlag to false for the AllowUpdateParameter object
                            'Test the list of keyField dictionaries for records to skip. Only call the stored procedure if oDetail key fields are not in the list
                            oAllowUpdatePar.insertFlag = False
                            Dim blnCanImport As Boolean = True
                            Dim strLogMsg As String = ""
                            For Each oDictionaryItem As Dictionary(Of String, String) In dictionaryList
                                'Rules:
                                '1. if any comp number matches we cannot import unless it is 0
                                '2. If the Comp alpha code and the comp Legal Entity match we cannot import
                                If oDictionaryItem("Comp Number") <> 0 AndAlso oDictionaryItem("Comp Number") = oDetail.CompNumber Then
                                    blnCanImport = False
                                    strLogMsg = "Comp Number = " & oDetail.CompNumber
                                    Exit For
                                ElseIf oDictionaryItem("Comp Alpha Code") = oDetail.CompAlphaCode And oDictionaryItem("Comp Legal Entity") = oDetail.CompLegalEntity Then
                                    blnCanImport = False
                                    strLogMsg = "Comp Alpha Code = " & oDetail.CompAlphaCode
                                    Exit For
                                End If
                            Next
                            If blnCanImport Then
                                oCompContWCFData.InsertOrUpdateCompContact70(oDetail.CompLegalEntity, oDetail.CompNumber, oDetail.CompAlphaCode, oDetail.CompContName, oDetail.CompContTitle, AllowUpdate("CompContTitle", oAllowUpdatePar), oDetail.CompCont800, AllowUpdate("CompCont800", oAllowUpdatePar), oDetail.CompContPhone, AllowUpdate("CompContPhone", oAllowUpdatePar), oDetail.CompContPhoneExt, AllowUpdate("CompContPhoneExt", oAllowUpdatePar), oDetail.CompContFax, AllowUpdate("CompContFax", oAllowUpdatePar), oDetail.CompContEmail, AllowUpdate("CompContEmail", oAllowUpdatePar))
                            Else
                                Log("Skipping detail record where " & strLogMsg & " because header record failed to import")
                                intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
                            End If
                        Catch ex As FaultException(Of DAL.SqlFaultInfo)
                            If ex.Detail.Message = "E_ServerMsgDetails" AndAlso Not ex.Detail.DetailsList Is Nothing AndAlso ex.Detail.DetailsList.Count > 0 Then
                                AddToGroupEmailMsg(String.Format("Update Company Contact information error. Server Message: {0}", ex.Detail.DetailsList))
                                intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
                            Else
                                AddToGroupEmailMsg("Invalid or missing company contact information")
                                intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
                            End If
                        Catch ex As Exception
                            AddToGroupEmailMsg("Invalid or missing company contact information")
                            intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
                        End Try
                    Else
                        AddToGroupEmailMsg("One of the company contact records was null or empty and could not be processed.")
                        intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
                    End If
                Next
            End If
            'Calander
            If Not oCalendar Is Nothing AndAlso oCalendar.Count > 0 Then
                For Each oCalItem As clsCompanyCalendarObject70 In oCalendar
                    If Not oCalItem Is Nothing Then
                        Try
                            oAllowUpdatePar.insertFlag = False
                            Dim blnCanImport As Boolean = True
                            Dim strLogMsg As String = ""
                            For Each oDictionaryItem As Dictionary(Of String, String) In dictionaryList
                                'Rules:
                                '1. if any comp number matches we cannot import unless it is 0
                                '2. If the Comp alpha code and the comp Legal Entity match we cannot import
                                If oDictionaryItem("Comp Number") <> 0 AndAlso oDictionaryItem("Comp Number") = oCalItem.CompNumber Then
                                    blnCanImport = False
                                    strLogMsg = "Comp Number = " & oCalItem.CompNumber
                                    Exit For
                                ElseIf oDictionaryItem("Comp Alpha Code") = oCalItem.CompAlphaCode And oDictionaryItem("Comp Legal Entity") = oCalItem.CompLegalEntity Then
                                    blnCanImport = False
                                    strLogMsg = "Comp Alpha Code = " & oCalItem.CompAlphaCode
                                    Exit For
                                End If
                            Next
                            If blnCanImport Then
                                If validateDateWS(oCalItem.StartTime, dtVal) Then
                                    oCalItem.StartTime = exportDateToString(dtVal.ToString)
                                Else
                                    oCalItem.StartTime = ""
                                End If
                                If validateDateWS(oCalItem.EndTime, dtVal) Then
                                    oCalItem.EndTime = exportDateToString(dtVal.ToString)
                                Else
                                    oCalItem.EndTime = ""
                                End If
                                oCompCalWCFData.InsertOrUpdateCompCal70(oCalItem.CompLegalEntity, oCalItem.CompNumber, oCalItem.CompAlphaCode, oCalItem.Month, AllowUpdate("Month", oAllowUpdatePar), oCalItem.Day, AllowUpdate("Day", oAllowUpdatePar), oCalItem.Open, AllowUpdate("Open", oAllowUpdatePar), oCalItem.StartTime, AllowUpdate("StartTime", oAllowUpdatePar), oCalItem.EndTime, AllowUpdate("EndTime", oAllowUpdatePar), oCalItem.IsHoliday, AllowUpdate("IsHoliday", oAllowUpdatePar))
                            Else
                                Log("Skipping detail record where " & strLogMsg & " because header record failed to import")
                                intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
                            End If
                        Catch ex As FaultException(Of DAL.SqlFaultInfo)
                            If ex.Detail.Message = "E_ServerMsgDetails" AndAlso Not ex.Detail.DetailsList Is Nothing AndAlso ex.Detail.DetailsList.Count > 0 Then
                                AddToGroupEmailMsg(String.Format("Update Company Calendar information error. Server Message: {0}", ex.Detail.DetailsList))
                                intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
                            Else
                                AddToGroupEmailMsg("Invalid or missing company calendar information")
                                intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
                            End If
                        Catch ex As Exception
                            'modify to add to either group or IT emailMsg and let finally code do the actual sending (do this for all LogExceptions)
                            AddToGroupEmailMsg("Invalid or missing company calendar information")
                            intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
                        End Try
                    Else
                        AddToGroupEmailMsg("One of the company calendar records was null or empty and could not be processed.")
                        intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
                    End If
                Next
            End If

        Catch ex As Exception
            AddToITEmailMsg("Company import system error")
            intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
        Finally
            If GroupEmailMsg.Trim.Length > 0 Then
                LogGroupEmailError("Process Company Data Warning", "The following errors or warnings were reported some company records may not have been processed correctly." & GroupEmailMsg)
            End If
            If ITEmailMsg.Trim.Length > 0 Then
                LogAdminEmailError("Process Company Data Failure", "The following errors or warnings were reported some company records may not have been processed correctly." & ITEmailMsg)
            End If
        End Try

        finalResult.ReturnValue = intRet
        Return finalResult

    End Function

    Public Function ProcessData(
                ByVal oCompanies As CompanyData.CompanyHeaderDataTable,
                ByVal oContacts As CompanyData.CompanyContactDataTable,
                ByVal strConnection As String,
                Optional ByVal oCalendar As CompanyData.CompanyCalendarDataTable = Nothing) As ProcessDataReturnValues

        Dim intRet As ProcessDataReturnValues = ProcessDataReturnValues.nglDataIntegrationFailure

        Dim strTitle As String = ""
        Dim strMsg As String = ""
        Dim strHeaderTable As String = "Comp"
        Dim strItemTable As String = "CompCont"
        Dim strCalendarTable As String = "CompCal"
        Me.HeaderName = "Company"
        Me.CalendarName = "Company Calendar"
        Me.ItemName = "Company Contact"
        Me.ImportTypeKey = IntegrationTypes.Company
        Me.CreatedDate = Now.ToString
        Me.CreateUser = "Data Integration DLL"
        Me.Source = "Company Data Integration"

        Me.DBConnection = strConnection
        'try the connection
        If Not Me.openConnection Then
            Return ProcessDataReturnValues.nglDataConnectionFailure
        End If


        'set the error date time stamp and other Defaults
        'Header Information
        Dim oFields As New clsImportFields
        If Not buildHeaderCollection(oFields) Then Return ProcessDataReturnValues.nglDataIntegrationFailure
        'Item/Detail Information
        Dim oItems As New clsImportFields
        If Not buildItemCollection(oItems) Then Return ProcessDataReturnValues.nglDataIntegrationFailure
        'Hours and days of operation
        Dim oCals As New clsImportFields
        If Not buildCalendarCollection(oCals) Then Return ProcessDataReturnValues.nglDataIntegrationFailure
        Try
            'Import the Header Records
            If importHeaderRecords(oCompanies, oFields) Then
                'Now Import the Details
                'Check if the detail file exists
                If Not oContacts Is Nothing AndAlso oContacts.Count > 0 Then
                    importItemRecords(oContacts, oItems)
                Else
                    Log("No company contact records were provided.")
                End If
                'Now Import the Calendar
                'Check if the calendar file exists
                If Not oCalendar Is Nothing AndAlso oCalendar.Count > 0 Then
                    importCalendarRecords(oCalendar, oCals)
                Else
                    Log("Calendar information was not provided.")
                End If
            End If
            strTitle = "Process Data Complete"
            If GroupEmailMsg.Trim.Length > 0 Then
                LogError("Process Company Data Warning", "The following errors or warnings were reported some company records may not have been processed correctly." & GroupEmailMsg, GroupEmail)
            End If
            If ITEmailMsg.Trim.Length > 0 Then
                LogError("Process Company Data Failure", "The following errors or warnings were reported some company records may not have been processed correctly." & ITEmailMsg, AdminEmail)
            End If
            If Me.TotalRecords > 0 Then
                intRet = ProcessDataReturnValues.nglDataIntegrationComplete
                strMsg = "Success!  " & Me.TotalRecords & " " & Me.HeaderName & " records were imported."
                If Me.TotalItems > 0 Then
                    strMsg &= vbCrLf & vbCrLf & " And " & Me.TotalItems & " " & Me.ItemName & " records were imported."
                    'intRet = ProcessDataReturnValues.nglDataIntegrationComplete
                End If
                If Me.TotalCalendarRecords > 0 Then
                    strMsg &= vbCrLf & vbCrLf & " And " & Me.TotalCalendarRecords & " " & Me.CalendarName & " records were imported."
                    'intRet = ProcessDataReturnValues.nglDataIntegrationComplete
                End If
                If Me.RecordErrors > 0 Or Me.ItemErrors > 0 Or Me.CalendarErrors > 0 Then
                    strTitle = "Process Data Complete With Errors"
                    If Me.RecordErrors > 0 Then
                        strMsg &= vbCrLf & vbCrLf & "ERROR!  " & Me.RecordErrors & " " & Me.HeaderName & " records could not be imported.  Please run the Import Error Report for more information."
                    End If
                    If Me.ItemErrors > 0 Then
                        strMsg &= vbCrLf & vbCrLf & "ERROR!  " & Me.ItemErrors & " " & Me.ItemName & " records could not be imported.  Please run the Import Error Report for more information."
                    End If
                    If Me.CalendarErrors > 0 Then
                        strMsg &= vbCrLf & vbCrLf & "ERROR!  " & Me.CalendarErrors & " " & Me.CalendarName & " records could not be imported.  Please run the Import Error Report for more information."
                    End If
                    intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
                End If
            Else
                strMsg = "No " & Me.HeaderName & " records were imported."
                intRet = ProcessDataReturnValues.nglDataIntegrationFailure
            End If
            Log(strMsg)
        Catch ex As Exception
            LogException("Process Company Data Failure", "Could not process the requested carrier data.", AdminEmail, ex, "NGL.FreightMaster.Integration.clsCompany.ProcessData")
        Finally
            closeConnection()
        End Try
        Return intRet
    End Function


    'create a new callatlong70 that uses CompStreetZip as string, , complat as double by ref, complong as dbl byref, 
    'use the headerfielddic true/false flag to determine if this is updateable (before calling this method) 
    ''' <summary>
    ''' Depricated, we now use Trimble logic call calcLatLong80
    ''' </summary>
    ''' <param name="CompNumber"></param>
    ''' <param name="CompStreetZip"></param>
    ''' <param name="CompLatitude"></param>
    ''' <param name="CompLongitude"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.4.0.003 on 07/19/2021 depricated
    '''     Call calcMiles80 instead
    ''' </remarks>
    Private Function calcLatLong70(ByVal CompNumber As Integer, ByVal CompStreetZip As String, ByRef CompLatitude As Double, ByRef CompLongitude As Double) As Boolean
        Dim boolReturn As Boolean = False
        Dim strlocation As String = zipClean(CompStreetZip)
        Return calcLatLong80(CompNumber, CompLatitude, CompLongitude, strlocation)
        'removed by RHR for v-8.4.0.003 on 07/19/2021 
        'Try
        '    strlocation = zipClean(CompStreetZip)
        '    If UsePCMiler Then
        '        Using oPCmiles As New Ngl.Service.PCMiler64.PCMiles
        '            'oPCmiles.Debug = Me.Debug
        '            'oPCmiles.LoggingOn = False
        '            If oPCmiles.getGeoCode(strlocation, CompLatitude, CompLongitude) Then
        '                boolReturn = True
        '                If Debug Then Log("Lat/Long = " & CompLatitude.ToString & "/" & CompLongitude.ToString)
        '            Else
        '                ITEmailMsg &= "<br />" & Source & " Warning: There was a problem with PC Miler in NGL.FreightMaster.Integration.clsCompany.calLatLong70 (import not affected), could not calculate lat-long for company number: " & CompNumber.ToString & ".<br />" & vbCrLf & oPCmiles.LastError & "<br />" & vbCrLf
        '                Log("NGL.FreightMaster.Integration.clsCompany.calLatLong70 ( PC Miler ) Warning!")

        '            End If
        '        End Using
        '    End If
        'Catch ex As Exception
        '    ITEmailMsg &= "<br />" & Source & " Warning: There was an unexpected error in NGL.FreightMaster.Integration.clsCompany.calLatLong (import not affected), could not calculate lat-long for company  number: " & CompNumber.ToString & ".<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
        '    Log("NGL.FreightMaster.Integration.clsCompany.calLatLong Unexpected Error: " & ex.ToString)
        'End Try
        'Return boolReturn

    End Function


    ''' <summary>
    ''' Wrapper for calcLatLong80 used to generate the address details
    ''' </summary>
    ''' <param name="CompNumber"></param>
    ''' <param name="dLattitude"></param>
    ''' <param name="dLongitude"></param>
    ''' <param name="sStreet"></param>
    ''' <param name="sCity"></param>
    ''' <param name="sState"></param>
    ''' <param name="sZip"></param>
    ''' <param name="sCountry"></param>
    ''' <remarks>
    ''' Created by RHR for v-8.4.0.003 on 07/19/2021 
    ''' </remarks>
    Private Function calcLatLong80(ByVal CompNumber As Integer, ByRef dLattitude As Double, ByRef dLongitude As Double, ByVal sStreet As String, ByVal sCity As String, ByVal sState As String, ByVal sZip As String, ByVal sCountry As String, Optional ByRef strPCMLastError As String = "") As Boolean
        Dim sAddress = oPCMiler.concateAddress(sStreet, sCity, sState, sZip, sCountry)
        Return calcLatLong80(CompNumber, dLattitude, dLongitude, sAddress, strPCMLastError)
    End Function

    ''' <summary>
    ''' Gets Lat Long for address use oPCMiler.concateAddress to generate a valid address
    ''' </summary>
    ''' <param name="CompNumber"></param>
    ''' <param name="dLattitude"></param>
    ''' <param name="dLongitude"></param>
    ''' <param name="sAddress"></param>
    ''' <param name="strPCMLastError"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR for v-8.4.0.003 on 07/19/2021 
    ''' </remarks>
    Private Function calcLatLong80(ByVal CompNumber As Integer, ByRef dLattitude As Double, ByRef dLongitude As Double, ByVal sAddress As String, Optional ByRef strPCMLastError As String = "") As Boolean
        Dim blnGetGeoCode As Boolean = False
        Try
            blnGetGeoCode = oPCMiler.getGeoCode(sAddress, dLattitude, dLongitude, strPCMLastError, False, "")

            If (Not blnGetGeoCode Or Not String.IsNullOrEmpty(strPCMLastError)) Then
                ITEmailMsg &= "<br />" & Source & " Warning: There was a problem with PC Miler in NGL.FreightMaster.Integration.clsCompany.calLatLong80 (import not affected), could not calculate lat-long for company number: " & CompNumber.ToString & ".<br />" & vbCrLf & strPCMLastError & "<br />" & vbCrLf
                Log("NGL.FreightMaster.Integration.clsCompany.calLatLong80 ( PC Miler ) Warning!")
            Else
                If Debug Then Log("Lat/Long = " & dLattitude.ToString & "/" & dLongitude.ToString)
            End If

        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Warning: There was an unexpected error in NGL.FreightMaster.Integration.clsCompany.calLatLong (import not affected), could not calculate lat-long for company  number: " & CompNumber.ToString & ".<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsCompany.calLatLong Unexpected Error: " & ex.ToString)
        End Try
        Return blnGetGeoCode

    End Function

    Private Sub calcLatLong(ByRef oFields As clsImportFields)
        Dim blnGetGeoCode As Boolean = False
        Dim strlocation As String = ""
        Try
            'Test if a zip code is possible and the 
            'the import flags for lat and long are false
            'we only modify if the import flag is false else
            'the source controls the Lat Long
            If oFields("CompStreetZip").Use _
                AndAlso oFields("CompLatitude").Use = False _
                AndAlso oFields("CompLongitude").Use = False Then
                strlocation = zipClean(oFields("CompStreetZip").Value)
                Dim dblLat As Double = 0
                Dim dblLong As Double = 0
                Dim strPCMLastError As String = ""
                ' Begin Modified by RHR for v-8.4.0.003 on 07/19/2021 call new CalcLatLong80 method for trimble API
                If calcLatLong80(oFields("LaneNumber").Value, dblLat, dblLong, strlocation, strPCMLastError) Then
                    oFields("CompLatitude").Value = CStr(dblLat)
                    oFields("CompLatitude").Use = True
                    oFields("CompLongitude").Value = CStr(dblLong)
                    oFields("CompLongitude").Use = True
                    If Debug Then Log("Lat/Long = " & dblLat.ToString & "/" & dblLong.ToString)
                End If
                ' End Modified by RHR for v-8.4.0.003 on 07/19/2021 
                'Removed Modified by RHR for v-8.4.0.003 on 07/19/2021
                'If UsePCMiler Then
                '    Using oPCmiles As New Ngl.Service.PCMiler64.PCMiles
                '        'oPCmiles.Debug = Me.Debug
                '        'oPCmiles.LoggingOn = False
                '        If oPCmiles.getGeoCode(strlocation, dblLat, dblLong) Then
                '            oFields("CompLatitude").Value = CStr(dblLat)
                '            oFields("CompLatitude").Use = True
                '            oFields("CompLongitude").Value = CStr(dblLong)
                '            oFields("CompLongitude").Use = True
                '            If Debug Then Log("Lat/Long = " & dblLat.ToString & "/" & dblLong.ToString)
                '        Else
                '            ITEmailMsg &= "<br />" & Source & " Warning: There was a problem with PC Miler in NGL.FreightMaster.Integration.clsCompany.calLatLong (import not affected), could not calculate lat-long for company number: " & oFields("CompNumber").Value.ToString & ".<br />" & vbCrLf & oPCmiles.LastError & "<br />" & vbCrLf
                '            Log("NGL.FreightMaster.Integration.clsCompany.calLatLong ( PC Miler ) Warning!")

                '        End If
                '    End Using
                'End If
            End If
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Warning: There was an unexpected error in NGL.FreightMaster.Integration.clsCompany.calLatLong (import not affected), could not calculate lat-long for company  number: " & oFields("CompNumber").Value.ToString & ".<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsCompany.calLatLong Unexpected Error: " & ex.ToString)
        End Try

    End Sub

    Private Function createNewCompNumber(ByVal oNumber As clsImportField) As Boolean

        Dim Ret As Boolean = False
        Dim strValues As String = ""
        Dim blnFirstField As Boolean = True
        Dim intCompNumber As Integer = 0
        Try
            Dim intRetryCt As Integer = 0
            'Get the current highest company number available
            Dim strSQL As String = "Select top 1 CompNumber from Comp Order By CompNumber Desc"
            Do
                intRetryCt += 1
                Dim cmd As New SqlCommand
                Dim drTemp As SqlDataReader
                Try
                    'check the active db connection
                    If Me.openConnection() Then
                        With cmd
                            .Connection = DBCon
                            .CommandTimeout = 300
                            .CommandText = strSQL
                            .CommandType = CommandType.Text
                            drTemp = .ExecuteReader()
                        End With
                        If drTemp.HasRows Then
                            drTemp.Read()
                            intCompNumber = Val(nz(drTemp.Item("CompNumber"), 0))
                            drTemp.Close()
                        End If
                        'add 1 to the value
                        intCompNumber += 1
                        'build execute string to insert record into dbo.AlphaCompanyXref
                        strSQL = "Insert Into dbo.AlphaCompanyXref (ACXCompNumber,ACXAlphaNumber)"
                        strSQL &= " Values ( " & intCompNumber & " , '" & stripQuotes(oNumber.Value) & "')"
                        'debug.print " Insert AlphaCompanyXref SQL = " & strSQL
                        cmd.CommandText = strSQL
                        cmd.ExecuteScalar()
                        LogError("Company Data Download Notification (import not affected)", "NGL.FreightMaster.Integration.clsCompany.createNewCompNumber: A new company number was created (import were not affected)" & vbCrLf & "The alpha company number " & oNumber.Value.ToString & " did not exist in the AlphaCompanyXref table.  A new cross reference was created using numeric company number " & intCompNumber.ToString & ".", GroupEmail)
                        oNumber.Value = intCompNumber
                        Ret = True
                        Exit Do
                    Else
                        If intRetryCt > Me.Retry Then
                            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsCompany.createNewCompNumber: Open database connection failure, attempted to create a database connection " & intRetryCt.ToString & " times without success.  Could not write to AlphaCompanyXref table..<br />" & vbCrLf
                            Log("NGL.FreightMaster.Integration.clsCompany.createNewCompNumber Failed!")
                        Else
                            Log("NGL.FreightMaster.Integration.clsCompany.createNewCompNumber Open DB Connection Failure Retry = " & intRetryCt.ToString)
                        End If
                    End If
                Catch ex As Exception
                    If intRetryCt > Me.Retry Then
                        ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.lsCompany.createNewCompNumber, attempted to write to  AlphaCompanyXref table " & intRetryCt.ToString & " times without success.<br />" & vbCrLf & readExceptionMessage(ex) & "<hr \>" & vbCrLf & strSQL & "<hr \>" & vbCrLf
                        Log("NGL.FreightMaster.Integration.lsCompany.createNewCompNumber Failed!" & readExceptionMessage(ex))
                    Else
                        Log("NGL.FreightMaster.Integration.lsCompany.createNewCompNumber Failure Retry = " & intRetryCt.ToString)

                    End If
                Finally
                    Try
#Disable Warning BC42104 ' Variable 'drTemp' is used before it has been assigned a value. A null reference exception could result at runtime.
                        drTemp.Close()
#Enable Warning BC42104 ' Variable 'drTemp' is used before it has been assigned a value. A null reference exception could result at runtime.
                    Catch ex As Exception

                    End Try
                    Try
                        cmd.Cancel()
                    Catch ex As Exception

                    End Try
                End Try
                'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsCompany.createNewCompNumber, Could not write to AlphaCompanyXref table.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsCompany.createNewCompNumber Failed!" & readExceptionMessage(ex))
        End Try
        Return Ret
    End Function

    Private Function importHeaderRecords(
        ByRef oCompanies As CompanyData.CompanyHeaderDataTable,
        ByRef oFields As clsImportFields) As Boolean
        Dim Ret As Boolean = False
        Try

            Dim intRetryCt As Integer = 0
            Dim blnUseLaneLat As Boolean = oFields("CompLatitude").Use
            Dim blnUseLaneLong As Boolean = oFields("CompLongitude").Use
            Dim strSource As String = "clsCompany.importHeaderRecords"
            Dim blnDataValidated As Boolean = False
            Dim strErrorMessage As String = ""
            Dim blnInsertRecord As Boolean = True
            Dim blnNewCompNumber As Boolean = False
            Do
                intRetryCt += 1
                RecordErrors = 0
                TotalRecords = 0
                Try
                    Try
                        Dim lngMax As Long = oCompanies.Count
                        Log("Importing " & lngMax & " Company Header Records.")
                        For Each oRow As CompanyData.CompanyHeaderRow In oCompanies
                            'Reset the data types and values to defaults for the following fields 
                            'at the top of each loop to handle alpha vs numeric data changes
                            oFields("CompNumber").Name = "CompNumber"
                            oFields("CompNumber").DataType = clsImportField.DataTypeID.gcvdtString
                            oFields("CompNumber").Length = 50
                            oFields("CompNumber").Null = True
                            strErrorMessage = ""
                            blnNewCompNumber = False
                            blnDataValidated = validateFields(oFields, oRow, strErrorMessage, strSource)
                            'Check for alpha company compatibility (note the company field tye will be changed to an integer on success)
                            If blnDataValidated Then
                                blnDataValidated = validateCompany(oFields("CompNumber"),
                                                                    strErrorMessage,
                                                                    oCX,
                                                                    strSource)
                                'Check if we need to create a new company record 
                                If Not blnDataValidated AndAlso (Not IsNumeric(stripQuotes(oFields("CompNumber").Value)) Or stripQuotes(oFields("CompNumber").Value) = "0") Then
                                    'we do not have a valid company number so we need to create one. and send an email to all about the new value
                                    If Not createNewCompNumber(oFields("CompNumber")) Then
                                        blnDataValidated = False
                                        strErrorMessage &= "  And a new Company Number could not be created in the Alpha Xref Table for company number " & oFields("CompNumber").Value
                                    Else
                                        'We have to revalidate the company number (this will confirm that the new alpa xref exists)
                                        blnDataValidated = validateCompany(oFields("CompNumber"),
                                                                    strErrorMessage,
                                                                    oCX,
                                                                    strSource)
                                        blnNewCompNumber = True
                                        blnInsertRecord = True
                                    End If
                                End If
                            End If
                            'test if the record already exists.
                            If blnDataValidated And Not blnNewCompNumber Then blnDataValidated = doesRecordExist(oFields,
                                                                                                strErrorMessage,
                                                                                                blnInsertRecord,
                                                                                                "Company number " & oFields("CompNumber").Value,
                                                                                                "Comp")


                            If Not blnDataValidated Then
                                addToErrorTable(oFields, "[dbo].[FileImportErrorLog]", strErrorMessage, "Data Integration DLL", mstrHeaderName)
                                RecordErrors += 1
                            Else
                                'Save the use values for lat long because calcLatLong changes them
                                blnUseLaneLat = oFields("CompLatitude").Use
                                blnUseLaneLong = oFields("CompLongitude").Use
                                'Get the Lat Long and miles if properly configured
                                calcLatLong(oFields)
                                'Save the changes to the main table
                                If saveData(oFields, blnInsertRecord, "Comp", "CompModUser", "CompModDate") Then
                                    TotalRecords += 1
                                End If
                                'put the values back the way they were
                                oFields("CompLatitude").Use = blnUseLaneLat
                                oFields("CompLongitude").Use = blnUseLaneLong
                            End If
                        Next
                        Return True
                    Catch ex As Exception
                        Throw
                    Finally

                    End Try
                Catch ex As Exception
                    If intRetryCt > Me.Retry Then
                        ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsCompany.importHeaderRecords, attempted to import company header records from Data Integration DLL " & intRetryCt.ToString & " times without success.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
                        Log("NGL.FreightMaster.Integration.clsCompany.importHeaderRecords Failed!" & readExceptionMessage(ex))
                    Else
                        Log("NGL.FreightMaster.Integration.clsCompany.importHeaderRecords Failure Retry = " & intRetryCt.ToString)
                    End If
                End Try
                'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.                
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsCompany.importHeaderRecords, Could not import from Data Integration DLL.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsCompany.importHeaderRecords Failed!" & readExceptionMessage(ex))
        End Try
        Return Ret

    End Function

    Private Function importItemRecords(
        ByRef oContacts As CompanyData.CompanyContactDataTable,
        ByRef oItems As clsImportFields) As Boolean
        Dim Ret As Boolean = False
        Try

            Dim intRetryCt As Integer = 0
            Dim strSource As String = "clsCompany.importItemRecords"
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
                        Log("Importing " & lngMax & " Company Contact Records.")
                        For Each oRow As CompanyData.CompanyContactRow In oContacts
                            'Reset the data types and values to defaults for the following fields 
                            'at the top of each loop to handle alpha vs numeric data changes
                            'Reset the data types and values to defaults for the following fields 
                            'at the top of each loop to handle alpha vs numeric data changes
                            oItems("CompNumber").Name = "CompNumber"
                            oItems("CompNumber").DataType = clsImportField.DataTypeID.gcvdtString
                            oItems("CompNumber").Length = 50
                            oItems("CompNumber").Null = True
                            strErrorMessage = ""
                            blnDataValidated = validateFields(oItems, oRow, strErrorMessage, strSource)
                            'Check for alpha company compatibility (note the company field tye will be changed to an integer on success)
                            If blnDataValidated Then blnDataValidated = validateCompany(oItems("CompNumber"),
                                                                                                strErrorMessage,
                                                                                                oCX,
                                                                                                strSource)
                            'Get the parent table key information
                            If blnDataValidated Then blnDataValidated = lookupFKValues(oItems,
                                                                                                strErrorMessage,
                                                                                                "Comp",
                                                                                                strSource,
                                                                                                "Company contact record for Company number " & oItems("CompNumber").Value)
                            'test if the record already exists.
                            If blnDataValidated Then blnDataValidated = doesRecordExist(oItems,
                                                                                                strErrorMessage,
                                                                                                blnInsertRecord,
                                                                                                "Company contact record for Company number " & oItems("CompNumber").Value,
                                                                                                "CompCont")
                            If Not blnDataValidated Then
                                addToErrorTable(oItems, "[dbo].[FileDetailsErrorLog]", strErrorMessage, "Data Integration DLL", ItemName)
                                ItemErrors += 1
                            Else
                                'Save the changes
                                If saveData(oItems, blnInsertRecord, "CompCont", "", "") Then
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
                        ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsCompany.importItemRecords; attempted to import company contact records for company number" & oItems("CompNumber").Value & " " & intRetryCt.ToString & " times without success.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
                        Log("NGL.FreightMaster.Integration.clsCompany.importItemRecords Failed!")
                        Me.ItemErrors += 1
                    Else
                        Log("NGL.FreightMaster.Integration.clsCompany.importItemRecords Failure Retry = " & intRetryCt.ToString)

                    End If
                End Try
                'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.                
        Catch ex As Exception
            Me.ItemErrors += 1
            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsCompany.importItemRecords; could not import company contact records for company number" & oItems("CompNumber").Value & ".<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsCompany.importItemRecords Failed!" & readExceptionMessage(ex))
        End Try
        Return Ret

    End Function

    Private Function importCalendarRecords(
        ByRef oCalendar As CompanyData.CompanyCalendarDataTable,
        ByRef oItems As clsImportFields) As Boolean
        Dim Ret As Boolean = False
        Try

            Dim intRetryCt As Integer = 0
            Dim strSource As String = "clsCompany.importCalendarRecords"
            Dim blnDataValidated As Boolean = False
            Dim strErrorMessage As String = ""
            Dim blnInsertRecord As Boolean = True

            Do
                intRetryCt += 1
                CalendarErrors = 0
                TotalCalendarRecords = 0
                Try

                    Try
                        Dim lngMax As Long = oCalendar.Count
                        Log("Importing " & lngMax & " Company Calendar Records.")
                        For Each oRow As CompanyData.CompanyCalendarRow In oCalendar
                            'Reset the data types and values to defaults for the following fields 
                            'at the top of each loop to handle alpha vs numeric data changes
                            'Reset the data types and values to defaults for the following fields 
                            'at the top of each loop to handle alpha vs numeric data changes
                            oItems("CompNumber").Name = "CompNumber"
                            oItems("CompNumber").DataType = clsImportField.DataTypeID.gcvdtString
                            oItems("CompNumber").Length = 50
                            oItems("CompNumber").Null = True
                            strErrorMessage = ""
                            blnDataValidated = validateFields(oItems, oRow, strErrorMessage, strSource)
                            'Check for alpha company compatibility (note the company field tye will be changed to an integer on success)
                            If blnDataValidated Then blnDataValidated = validateCompany(oItems("CompNumber"),
                                                                                                strErrorMessage,
                                                                                                oCX,
                                                                                                strSource)
                            'Get the parent table key information
                            If blnDataValidated Then blnDataValidated = lookupFKValues(oItems,
                                                                                                strErrorMessage,
                                                                                                "Comp",
                                                                                                strSource,
                                                                                                "Company calendar record for Company number " & oItems("CompNumber").Value)
                            'test if the record already exists.
                            If blnDataValidated Then blnDataValidated = doesRecordExist(oItems,
                                                                                                strErrorMessage,
                                                                                                blnInsertRecord,
                                                                                                "Company calendar record for Company number " & oItems("CompNumber").Value,
                                                                                                "CompCal")

                            If Not blnDataValidated Then
                                addToErrorTable(oItems, "[dbo].[FileDetailsErrorLog]", strErrorMessage, "Data Integration DLL", "Company Calendar")
                                CalendarErrors += 1
                            Else
                                'Save the changes
                                If saveData(oItems, blnInsertRecord, "CompCal", "", "") Then
                                    TotalCalendarRecords += 1
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
                        ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsCompany.importCalendarRecords; attempted to import company calendar records for company number" & oItems("CompNumber").Value & " " & intRetryCt.ToString & " times without success.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
                        Log("NGL.FreightMaster.Integration.clsCompany.importCalendarRecords Failed!" & readExceptionMessage(ex))
                        Me.CalendarErrors += 1
                    Else
                        Log("NGL.FreightMaster.Integration.clsCompany.importCalendarRecords Failure Retry = " & intRetryCt.ToString)

                    End If
                End Try
                'We only get here if an exception is thrown or the db connection is not open and intRetryCt <= 3
            Loop Until intRetryCt > Me.Retry 'this should never happen the code is here to show our intention.                
        Catch ex As Exception
            Me.CalendarErrors += 1
            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsCompany.importCalendarRecords; Could not import company calendar records for company number" & oItems("CompNumber").Value & ".<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsCompany.importCalendarRecords Failed!" & readExceptionMessage(ex))
        End Try
        Return Ret

    End Function

    Private Function buildHeaderCollection(ByRef oFields As clsImportFields) As Boolean
        Dim Ret As Boolean = False
        Try
            With oFields
                .Add("CompNumber", "CompNumber", clsImportField.DataTypeID.gcvdtString, 50, False, clsImportField.PKValue.gcPK)
                .Add("CompName", "CompName", clsImportField.DataTypeID.gcvdtString, 40, False)
                .Add("CompNatNumber", "CompNatNumber", clsImportField.DataTypeID.gcvdtLongInt, 11, True)
                .Add("CompNatName", "CompNatName", clsImportField.DataTypeID.gcvdtString, 40, True)
                .Add("CompStreetAddress1", "CompStreetAddress1", clsImportField.DataTypeID.gcvdtString, 40, True)
                .Add("CompStreetAddress2", "CompStreetAddress2", clsImportField.DataTypeID.gcvdtString, 40, True)
                .Add("CompStreetAddress3", "CompStreetAddress3", clsImportField.DataTypeID.gcvdtString, 40, True)
                .Add("CompStreetCity", "CompStreetCity", clsImportField.DataTypeID.gcvdtString, 25, True)
                .Add("CompStreetState", "CompStreetState", clsImportField.DataTypeID.gcvdtString, 8, True)
                .Add("CompStreetCountry", "CompStreetCountry", clsImportField.DataTypeID.gcvdtString, 30, True)
                .Add("CompStreetZip", "CompStreetZip", clsImportField.DataTypeID.gcvdtString, 20, True)  'Modified by RHR for v-8.4.003 on 06/25/2021
                .Add("CompMailAddress1", "CompMailAddress1", clsImportField.DataTypeID.gcvdtString, 40, True)
                .Add("CompMailAddress2", "CompMailAddress2", clsImportField.DataTypeID.gcvdtString, 40, True)
                .Add("CompMailAddress3", "CompMailAddress3", clsImportField.DataTypeID.gcvdtString, 40, True)
                .Add("CompMailCity", "CompMailCity", clsImportField.DataTypeID.gcvdtString, 25, True)
                .Add("CompMailState", "CompMailState", clsImportField.DataTypeID.gcvdtString, 8, True)
                .Add("CompMailCountry", "CompMailCountry", clsImportField.DataTypeID.gcvdtString, 30, True)
                .Add("CompMailZip", "CompMailZip", clsImportField.DataTypeID.gcvdtString, 20, True)  'Modified by RHR for v-8.4.003 on 06/25/2021
                .Add("CompWeb", "CompWeb", clsImportField.DataTypeID.gcvdtString, 255, True)
                .Add("CompEmail", "CompEmail", clsImportField.DataTypeID.gcvdtString, 50, True)
                .Add("CompDirections", "CompDirections", clsImportField.DataTypeID.gcvdtString, 500, True)
                .Add("CompAbrev", "CompAbrev", clsImportField.DataTypeID.gcvdtString, 3, True)
                .Add("CompActive", "CompActive", clsImportField.DataTypeID.gcvdtBit, 2, True)
                .Add("CompNEXTrack", "CompNEXTrack", clsImportField.DataTypeID.gcvdtBit, 2, True)
                .Add("CompNEXTStopAcctNo", "CompNEXTStopAcctNo", clsImportField.DataTypeID.gcvdtString, 50, True)
                .Add("CompNEXTStopPsw", "CompNEXTStopPsw", clsImportField.DataTypeID.gcvdtString, 50, True)
                .Add("CompNextstopSubmitRFP", "CompNextstopSubmitRFP", clsImportField.DataTypeID.gcvdtBit, 2, True)
                .Add("CompFAAShipID", "CompFAAShipID", clsImportField.DataTypeID.gcvdtString, 50, True)
                .Add("CompFAAShipDate", "CompFAAShipDate", clsImportField.DataTypeID.gcvdtDate, 22, True)
                .Add("CompFinDuns", "CompFinDuns", clsImportField.DataTypeID.gcvdtString, 11, True)
                .Add("CompFinTaxID", "CompFinTaxID", clsImportField.DataTypeID.gcvdtString, 20, True)
                .Add("CompFinPaymentForm", "CompFinPaymentForm", clsImportField.DataTypeID.gcvdtSmallInt, 11, True)
                .Add("CompFinSIC", "CompFinSIC", clsImportField.DataTypeID.gcvdtString, 8, True)
                .Add("CompFinPaymentDiscount", "CompFinPaymentDiscount", clsImportField.DataTypeID.gcvdtSmallInt, 6, True)
                .Add("CompFinPaymentDays", "CompFinPaymentDays", clsImportField.DataTypeID.gcvdtSmallInt, 6, True)
                .Add("CompFinPaymentNetDays", "CompFinPaymentNetDays", clsImportField.DataTypeID.gcvdtSmallInt, 6, True)
                .Add("CompFinCommTerms", "CompFinCommTerms", clsImportField.DataTypeID.gcvdtString, 15, True)
                .Add("CompFinCommTermsPer", "CompFinCommTermsPer", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("CompFinCreditLimit", "CompFinCreditLimit", clsImportField.DataTypeID.gcvdtLongInt, 11, True)
                .Add("CompFinCreditUsed", "CompFinCreditUsed", clsImportField.DataTypeID.gcvdtLongInt, 6, True)
                .Add("CompFinInvPrnCode", "CompFinInvPrnCode", clsImportField.DataTypeID.gcvdtBit, 2, True)
                .Add("CompFinInvEMailCode", "CompFinInvEMailCode", clsImportField.DataTypeID.gcvdtBit, 2, True)
                .Add("CompFinCurType", "CompFinCurType", clsImportField.DataTypeID.gcvdtLongInt, 11, True)
                .Add("CompFinFBToleranceHigh", "CompFinFBToleranceHigh", clsImportField.DataTypeID.gcvdtFloat, 22, True, clsImportField.PKValue.gcIgnore)
                .Add("CompFinFBToleranceLow", "CompFinFBToleranceLow", clsImportField.DataTypeID.gcvdtFloat, 22, True, clsImportField.PKValue.gcIgnore)
                .Add("CompFinCustomerSince", "CompFinCustomerSince", clsImportField.DataTypeID.gcvdtDate, 22, True)
                .Add("CompFinCardType", "CompFinCardType", clsImportField.DataTypeID.gcvdtString, 50, True)
                .Add("CompFinCardName", "CompFinCardName", clsImportField.DataTypeID.gcvdtString, 50, True)
                .Add("CompFinCardExpires", "CompFinCardExpires", clsImportField.DataTypeID.gcvdtString, 50, True)
                .Add("CompFinCardAuthorizor", "CompFinCardAuthorizor", clsImportField.DataTypeID.gcvdtString, 50, True)
                .Add("CompFinCardAuthPassword", "CompFinCardAuthPassword", clsImportField.DataTypeID.gcvdtString, 50, True)
                .Add("CompFinUseImportFrtCost", "CompFinUseImportFrtCost", clsImportField.DataTypeID.gcvdtBit, 2, True)
                .Add("CompFinBkhlFlatFee", "CompFinBkhlFlatFee", clsImportField.DataTypeID.gcvdtMoney, 21, True)
                .Add("CompFinBkhlCostPerc", "CompFinBkhlCostPerc", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("CompLatitude", "CompLatitude", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("CompLongitude", "CompLongitude", clsImportField.DataTypeID.gcvdtFloat, 22, True)
                .Add("CompMailTo", "CompMailTo", clsImportField.DataTypeID.gcvdtString, 500, True)
            End With
            Log("Header Field Array Loaded.")
            'get the import field flag values
            For ct As Integer = 1 To oFields.Count
                Dim blnUseField As Boolean = True
                Try
                    If oFields(ct).Name = "CompNumber" Then
                        'These are key fields and are always in use
                        blnUseField = True
                    Else
                        blnUseField = getImportFieldFlag(oFields(ct).Name, IntegrationTypes.Company)
                    End If
                Catch ex As Exception
                    'throw away any errors in case one or more field names is missing in the Import Field Flag Table
                End Try
                oFields(ct).Use = blnUseField
            Next
            Ret = True
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsCompany.buildHeaderCollection, could not build the header collection data.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsCompany.buildHeaderCollection Failed!" & readExceptionMessage(ex))
        End Try
        Return Ret

    End Function

    Private Function buildItemCollection(ByRef oItems As clsImportFields) As Boolean
        Dim Ret As Boolean = False
        Try
            With oItems
                .Add("CompContCompControl", "CompContCompControl", clsImportField.DataTypeID.gcvdtLongInt, 11, False, clsImportField.PKValue.gcFK, 2, "CompNumber", "CompControl")
                .Add("CompNumber", "CompNumber", clsImportField.DataTypeID.gcvdtString, 50, False, clsImportField.PKValue.gcHK)
                .Add("CompContName", "CompContName", clsImportField.DataTypeID.gcvdtString, 25, True, clsImportField.PKValue.gcPK)
                .Add("CompContTitle", "CompContTitle", clsImportField.DataTypeID.gcvdtString, 25, True)
                .Add("CompCont800", "CompCont800", clsImportField.DataTypeID.gcvdtString, 50, True)
                .Add("CompContPhone", "CompContPhone", clsImportField.DataTypeID.gcvdtString, 15, True)
                .Add("CompContFax", "CompContFax", clsImportField.DataTypeID.gcvdtString, 15, True)
                .Add("CompContEMail", "CompContEMail", clsImportField.DataTypeID.gcvdtString, 50, True)
            End With
            Log("Company Contact Field Array Loaded.")
            'get the item  field flag values
            For ct As Integer = 1 To oItems.Count
                Dim blnUseField As Boolean = True
                Try
                    If oItems(ct).Name = "CompContCompControl" Or oItems(ct).Name = "CompNumber" Or oItems(ct).Name = "CompContName" Then
                        'These are key fields and are always in use
                        blnUseField = True
                    Else
                        blnUseField = getImportFieldFlag(oItems(ct).Name, IntegrationTypes.Company)
                    End If
                Catch ex As Exception
                    'throw away any errors in case one or more field names is missing in the Import Field Flag Table
                End Try
                oItems(ct).Use = blnUseField
            Next
            Ret = True
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsCompany.buildItemCollection, could not build the item collection data.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsCompany.buildItemCollection Failed!" & readExceptionMessage(ex))
        End Try
        Return Ret

    End Function


    Private Function buildCalendarCollection(ByRef oCalendar As clsImportFields) As Boolean
        Dim Ret As Boolean = False
        Try
            With oCalendar
                .Add("CompCalCompControl", "CompCalCompControl", clsImportField.DataTypeID.gcvdtLongInt, 11, False, clsImportField.PKValue.gcFK, 2, "CompNumber", "CompControl")
                .Add("CompNumber", "CompNumber", clsImportField.DataTypeID.gcvdtString, 50, False, clsImportField.PKValue.gcHK)
                .Add("Month", "CompCalMonth", clsImportField.DataTypeID.gcvdtLongInt, 11, False, clsImportField.PKValue.gcPK)
                .Add("Day", "CompCalDay", clsImportField.DataTypeID.gcvdtLongInt, 11, False, clsImportField.PKValue.gcPK)
                .Add("Open", "CompCalOpen", clsImportField.DataTypeID.gcvdtBit, 2, False)
                .Add("StartTime", "CompCalStartTime", clsImportField.DataTypeID.gcvdtTime, 22, True)
                .Add("EndTime", "CompCalEndTime", clsImportField.DataTypeID.gcvdtTime, 22, True)
                .Add("IsHoliday", "CompCalIsHoliday", clsImportField.DataTypeID.gcvdtBit, 2, False)
            End With
            Log("Company Calendar Field Array Loaded.")
            'get the item  field flag values
            For ct As Integer = 1 To oCalendar.Count
                Dim blnUseField As Boolean = True
                Try
                    If oCalendar(ct).Name = "CompCalCompControl" Or oCalendar(ct).Name = "CompNumber" Or oCalendar(ct).Name = "CompCalMonth" Or oCalendar(ct).Name = "CompCalDay" Then
                        'These are key fields and are always in use
                        blnUseField = True
                    Else
                        blnUseField = getImportFieldFlag(oCalendar(ct).Name, IntegrationTypes.Company)
                    End If
                Catch ex As Exception
                    'throw away any errors in case one or more field names is missing in the Import Field Flag Table
                End Try
                oCalendar(ct).Use = blnUseField
            Next
            Ret = True
        Catch ex As Exception
            ITEmailMsg &= "<br />" & Source & " Failure: NGL.FreightMaster.Integration.clsCompany.buildCalendarCollection, could not build the calendar collection data.<br />" & vbCrLf & readExceptionMessage(ex) & "<br />" & vbCrLf
            Log("NGL.FreightMaster.Integration.clsCompany.buildCalendarCollection Failed!" & readExceptionMessage(ex))
        End Try
        Return Ret

    End Function

End Class

Public Class ProcessCompanyHeader70Result


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

    Private _CompControl As Integer = 0
    Public Property CompControl() As Integer
        Get
            Return _CompControl
        End Get
        Set(ByVal value As Integer)
            _CompControl = value
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

    'Private _Comp
    'Public Property Comp() As DTO.Comp
    '    Get
    '        Return _Comp
    '    End Get
    '    Set(ByVal value As DTO.Comp)
    '        _Comp = value
    '    End Set
    'End Property

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

    Private _strRetMsg As String
    Public Property strRetMsg() As String
        Get
            Return _strRetMsg
        End Get
        Set(ByVal value As String)
            _strRetMsg = value
        End Set
    End Property



    Public Sub processCompanyHeaderFailed(ByVal CompName As String, ByVal CompNumber As Integer, ByVal CompAlphaCode As String, ByVal CompLegalEntity As String)
        If Me.dicInvalidKeys Is Nothing OrElse Me.dicInvalidKeys.Count < 1 Then
            'add the key fields to the dictionary
            Me.dicInvalidKeys = New Dictionary(Of String, String) From {{"Comp Name", CompName}, {"Comp Number", CompNumber.ToString()}, {"Comp Alpha Code", CompAlphaCode}, {"Comp Legal Entity", CompLegalEntity}}
        End If
        intRet = ProcessDataReturnValues.nglDataIntegrationHadErrors
        successFlag = False
    End Sub

End Class
