Imports System.Data.Linq
Imports System.ServiceModel
Imports NGL.Core.ChangeTracker
Imports Ngl.Core.Utility
Imports Ngl.FreightMaster.Data.DataTransferObjects

Public Class NGLCarrierData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Logger = Logger.ForContext(Of NGLCarrierData)
        Dim db As New NGLMASCarrierDataContext(ConnectionString)
        Me.LinqTable = db.Carriers
        Me.LinqDB = db
        Me.SourceClass = "NGLCarrierData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If _LinqTable Is Nothing Then
                Dim db As New NGLMASCarrierDataContext(ConnectionString)
                _LinqTable = db.Carriers
                _LinqDB = db
            End If

            Return _LinqTable
        End Get
        Set(ByVal value As Object)
            _LinqTable = value
        End Set
    End Property

#End Region

#Region "Public Methods"

    Public Overrides Function GetRecordFiltered(Optional ByVal Control As Integer = 0) As DataTransferObjects.DTOBaseClass
        Return GetCarrierFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetCarriersFiltered()
    End Function

    'Modified By LVV on 10/14/16 for v-7.0.5.110 Carrier Contact Changes
    Public Function GetCarrierFiltered(Optional ByVal Control As Integer = 0, Optional ByVal Number As Integer = 0, Optional ByVal Name As String = "") As DataTransferObjects.Carrier
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                Dim oDLO As New DataLoadOptions
                oDLO.LoadWith(Of LTS.Carrier)(Function(t As LTS.Carrier) t.CarrierConts)
                oDLO.LoadWith(Of LTS.Carrier)(Function(t As LTS.Carrier) t.CarrierBudgets)
                'oDLO.LoadWith(Of LTS.Carrier)(Function(t As LTS.Carrier) t.CarrierEDIs)
                'oDLO.LoadWith(Of LTS.Carrier)(Function(t As LTS.Carrier) t.CarrierTrucks)
                db.LoadOptions = oDLO
                'Get the newest record that matches the provided criteria
                Dim Carrier As DataTransferObjects.Carrier = (
                        From t In db.Carriers
                        Where
                        (t.CarrierControl = If(Control = 0, t.CarrierControl, Control)) _
                        And
                        (t.CarrierNumber = If(Number = 0, t.CarrierNumber, Number)) _
                        And
                        (Name Is Nothing OrElse String.IsNullOrEmpty(Name) OrElse t.CarrierName = Name)
                        Order By t.CarrierControl Descending
                        Select New DataTransferObjects.Carrier With {.CarrierControl = t.CarrierControl _
                        , .CarrierNumber = If(t.CarrierNumber.HasValue, t.CarrierNumber.Value, 0) _
                        , .CarrierName = t.CarrierName _
                        , .CarrierStreetAddress1 = t.CarrierStreetAddress1 _
                        , .CarrierStreetAddress2 = t.CarrierStreetAddress2 _
                        , .CarrierStreetAddress3 = t.CarrierStreetAddress3 _
                        , .CarrierStreetCity = t.CarrierStreetCity _
                        , .CarrierStreetState = t.CarrierStreetState _
                        , .CarrierStreetCountry = t.CarrierStreetCountry _
                        , .CarrierStreetZip = t.CarrierStreetZip _
                        , .CarrierMailAddress1 = t.CarrierMailAddress1 _
                        , .CarrierMailAddress2 = t.CarrierMailAddress2 _
                        , .CarrierMailAddress3 = t.CarrierMailAddress3 _
                        , .CarrierMailCity = t.CarrierMailCity _
                        , .CarrierMailState = t.CarrierMailState _
                        , .CarrierMailCountry = t.CarrierMailCountry _
                        , .CarrierMailZip = t.CarrierMailZip _
                        , .CarrierModDate = t.CarrierModDate _
                        , .CarrierModUser = t.CarrierModUser _
                        , .CarrierSCAC = t.CarrierSCAC _
                        , .CarrierAccountNo = t.CarrierAccountNo _
                        , .CarrierTypeCode = t.CarrierTypeCode _
                        , .CarrierGeneralInfo = t.CarrierGeneralInfo _
                        , .CarrierActive = If(t.CarrierActive.HasValue, t.CarrierActive.Value, False) _
                        , .CarrierWebSite = t.CarrierWebSite _
                        , .CarrierEmail = t.CarrierEmail _
                        , .CarrierNEXTStopAcct = t.CarrierNEXTStopAcct _
                        , .CarrierNEXTStopPwd = t.CarrierNEXTStopPwd _
                        , .CarrierGradReliability = If(t.CarrierGradReliability.HasValue, t.CarrierGradReliability.Value, 0) _
                        , .CarrierGradBillingAccuracy = If(t.CarrierGradBillingAccuracy.HasValue, t.CarrierGradBillingAccuracy.Value, 0) _
                        , .CarrierGradFinancialStrength = If(t.CarrierGradFinancialStrength.HasValue, t.CarrierGradFinancialStrength.Value, 0) _
                        , .CarrierGradEquipmentCondition = If(t.CarrierGradEquipmentCondition.HasValue, t.CarrierGradEquipmentCondition.Value, 0) _
                        , .CarrierGradContactAttitude = If(t.CarrierGradContactAttitude.HasValue, t.CarrierGradContactAttitude.Value, 0) _
                        , .CarrierGradDriverAttitude = If(t.CarrierGradDriverAttitude.HasValue, t.CarrierGradDriverAttitude.Value, 0) _
                        , .CarrierGradClaimFrequency = If(t.CarrierGradClaimFrequency.HasValue, t.CarrierGradClaimFrequency.Value, 0) _
                        , .CarrierGradClaimPayment = If(t.CarrierGradClaimPayment.HasValue, t.CarrierGradClaimPayment.Value, 0) _
                        , .CarrierGradGeographicCoverage = If(t.CarrierGradGeographicCoverage.HasValue, t.CarrierGradGeographicCoverage.Value, 0) _
                        , .CarrierGradCustomerService = If(t.CarrierGradCustomerService.HasValue, t.CarrierGradCustomerService.Value, 0) _
                        , .CarrierGradPriceChangeNotification = If(t.CarrierGradPriceChangeNotification.HasValue, t.CarrierGradPriceChangeNotification.Value, 0) _
                        , .CarrierGradPriceChangeFrequency = If(t.CarrierGradPriceChangeFrequency.HasValue, t.CarrierGradPriceChangeFrequency.Value, 0) _
                        , .CarrierGradPriceAggressiveness = If(t.CarrierGradPriceAggressiveness.HasValue, t.CarrierGradPriceAggressiveness.Value, 0) _
                        , .CarrierGradAverage = If(t.CarrierGradAverage.HasValue, t.CarrierGradAverage.Value, 0) _
                        , .CarrierQualInsuranceDate = t.CarrierQualInsuranceDate _
                        , .CarrierQualQualified = If(t.CarrierQualQualified.HasValue, t.CarrierQualQualified.Value, False) _
                        , .CarrierQualAuthority = t.CarrierQualAuthority _
                        , .CarrierQualContract = If(t.CarrierQualContract.HasValue, t.CarrierQualContract.Value, False) _
                        , .CarrierQualSignedDate = t.CarrierQualSignedDate _
                        , .CarrierQualContractExpiresDate = t.CarrierQualContractExpiresDate _
                        , .CarrierQualMaxPerShipment = If(t.CarrierQualMaxPerShipment.HasValue, t.CarrierQualMaxPerShipment.Value, 0) _
                        , .CarrierQualMaxAllShipments = If(t.CarrierQualMaxAllShipments.HasValue, t.CarrierQualMaxAllShipments.Value, 0) _
                        , .CarrierQualCurAllExposure = If(t.CarrierQualCurAllExposure.HasValue, t.CarrierQualCurAllExposure.Value, 0) _
                        , .CarrierCodeVal1 = If(t.CarrierCodeVal1.HasValue, t.CarrierCodeVal1.Value, 0) _
                        , .CarrierCodeVal2 = If(t.CarrierCodeVal2.HasValue, t.CarrierCodeVal2.Value, 0) _
                        , .CarrierTruckDefault = If(t.CarrierTruckDefault.HasValue, t.CarrierTruckDefault.Value, 0) _
                        , .CarrierAllowWebTender = If(t.CarrierAllowWebTender.HasValue, t.CarrierAllowWebTender.Value, False) _
                        , .CarrierIgnoreTariff = t.CarrierIgnoreTariff _
                        , .CarrierSmartWayPartnerType = t.CarrierSmartWayPartnerType _
                        , .CarrierSmartWayScore = If(t.CarrierSmartWayScore.HasValue, t.CarrierSmartWayScore.Value, 0) _
                        , .CarrierSmartWayPartner = t.CarrierSmartWayPartner _
                        , .CarrierAutoFinalize = t.CarrierAutoFinalize _
                        , .CarrierUseStdFuelAddendum = t.CarrierUseStdFuelAddendum _
                        , .CarrierAutoAssignProNumber = t.CarrierAutoAssignProNumber _
                        , .CarrierQualUSDot = t.CarrierQualUSDot _
                        , .CarrierQualCSAScore = t.CarrierQualCSAScore _
                        , .CarrierCalcOnTimeServiceLevel = t.CarrierCalcOnTimeServiceLevel _
                        , .CarrierAssignedOnTimeServiceLevel = t.CarrierAssignedOnTimeServiceLevel _
                        , .CarrierCalcOnTimeNoMonthsUsed = t.CarrierCalcOnTimeNoMonthsUsed _
                        , .CarrierAlphaCode = t.CarrierAlphaCode _
                        , .CarrierLegalEntity = t.CarrierLegalEntity _
                        , .CarrierUpdated = t.CarrierUpdated.ToArray() _
                        , .CarrierConts = (
                        From d In t.CarrierConts
                        Select New DataTransferObjects.CarrierCont With {.CarrierContControl = d.CarrierContControl _
                        , .CarrierContCarrierControl = d.CarrierContCarrierControl _
                        , .CarrierContName = d.CarrierContName _
                        , .CarrierContTitle = d.CarrierContTitle _
                        , .CarrierContactPhone = d.CarrierContactPhone _
                        , .CarrierContactFax = d.CarrierContactFax _
                        , .CarrierContPhoneExt = d.CarrierContPhoneExt _
                        , .CarrierContact800 = d.CarrierContact800 _
                        , .CarrierContactEMail = d.CarrierContactEMail _
                        , .CarrierContactDefault = d.CarrierContactDefault _
                        , .CarrierContUpdated = d.CarrierContUpdated.ToArray()}).ToList() _
                        , .CarrierBudgets = (
                        From b In t.CarrierBudgets
                        Select New DataTransferObjects.CarrierBudget With {.CarrierBudControl = b.CarrierBudControl _
                        , .CarrierBudCarrierControl = b.CarrierBudCarrierControl _
                        , .CarrierBudModDate = b.CarrierBudModDate _
                        , .CarrierBudModUser = b.CarrierBudModUser _
                        , .CarrierBudExpMo1 = If(b.CarrierBudExpMo1.HasValue, b.CarrierBudExpMo1.Value, 0) _
                        , .CarrierBudExpMo2 = If(b.CarrierBudExpMo2.HasValue, b.CarrierBudExpMo2.Value, 0) _
                        , .CarrierBudExpMo3 = If(b.CarrierBudExpMo3.HasValue, b.CarrierBudExpMo3.Value, 0) _
                        , .CarrierBudExpMo4 = If(b.CarrierBudExpMo4.HasValue, b.CarrierBudExpMo4.Value, 0) _
                        , .CarrierBudExpMo5 = If(b.CarrierBudExpMo5.HasValue, b.CarrierBudExpMo5.Value, 0) _
                        , .CarrierBudExpMo6 = If(b.CarrierBudExpMo6.HasValue, b.CarrierBudExpMo6.Value, 0) _
                        , .CarrierBudExpMo7 = If(b.CarrierBudExpMo7.HasValue, b.CarrierBudExpMo7.Value, 0) _
                        , .CarrierBudExpMo8 = If(b.CarrierBudExpMo8.HasValue, b.CarrierBudExpMo8.Value, 0) _
                        , .CarrierBudExpMo9 = If(b.CarrierBudExpMo9.HasValue, b.CarrierBudExpMo9.Value, 0) _
                        , .CarrierBudExpMo10 = If(b.CarrierBudExpMo10.HasValue, b.CarrierBudExpMo10.Value, 0) _
                        , .CarrierBudExpMo11 = If(b.CarrierBudExpMo11.HasValue, b.CarrierBudExpMo11.Value, 0) _
                        , .CarrierBudExpMo12 = If(b.CarrierBudExpMo12.HasValue, b.CarrierBudExpMo12.Value, 0) _
                        , .CarrierBudExpTotal = If(b.CarrierBudExpTotal.HasValue, b.CarrierBudExpTotal.Value, 0) _
                        , .CarrierBudActMo1 = If(b.CarrierBudActMo1.HasValue, b.CarrierBudActMo1.Value, 0) _
                        , .CarrierBudActMo2 = If(b.CarrierBudActMo2.HasValue, b.CarrierBudActMo2.Value, 0) _
                        , .CarrierBudActMo3 = If(b.CarrierBudActMo3.HasValue, b.CarrierBudActMo3.Value, 0) _
                        , .CarrierBudActMo4 = If(b.CarrierBudActMo4.HasValue, b.CarrierBudActMo4.Value, 0) _
                        , .CarrierBudActMo5 = If(b.CarrierBudActMo5.HasValue, b.CarrierBudActMo5.Value, 0) _
                        , .CarrierBudActMo6 = If(b.CarrierBudActMo6.HasValue, b.CarrierBudActMo6.Value, 0) _
                        , .CarrierBudActMo7 = If(b.CarrierBudActMo7.HasValue, b.CarrierBudActMo7.Value, 0) _
                        , .CarrierBudActMo8 = If(b.CarrierBudActMo8.HasValue, b.CarrierBudActMo8.Value, 0) _
                        , .CarrierBudActMo9 = If(b.CarrierBudActMo9.HasValue, b.CarrierBudActMo9.Value, 0) _
                        , .CarrierBudActMo10 = If(b.CarrierBudActMo10.HasValue, b.CarrierBudActMo10.Value, 0) _
                        , .CarrierBudActMo11 = If(b.CarrierBudActMo11.HasValue, b.CarrierBudActMo11.Value, 0) _
                        , .CarrierBudActMo12 = If(b.CarrierBudActMo12.HasValue, b.CarrierBudActMo12.Value, 0) _
                        , .CarrierBudActTotal = If(b.CarrierBudActTotal.HasValue, b.CarrierBudActTotal.Value, 0) _
                        , .CarrierBudgetUpdated = b.CarrierBudgetUpdated.ToArray()}).ToList()}).First

                Return Carrier

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function


    Public Function GetCarrier(Control As Integer) As DataTransferObjects.Carrier
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                Dim Carrier As DataTransferObjects.Carrier = (
                        From t In db.Carriers
                        Where
                        (t.CarrierControl = Control)
                        Order By t.CarrierControl Descending
                        Select selectDTOData(t, db)).FirstOrDefault()

                Return Carrier

            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                ManageLinqDataExceptions(ex, buildProcedureName("GetCarrier"))
            End Try

            Return Nothing

        End Using
    End Function


    Public Function GetCarrierControls() As List(Of Integer)
        Dim intRet As New List(Of Integer)
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                intRet = (
                    From t In db.Carriers
                        Where t.CarrierActive = True
                        Order By t.CarrierControl Descending
                        Select t.CarrierControl).ToList()


                Return intRet

            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                ManageLinqDataExceptions(ex, buildProcedureName("GetCarrier"))
            End Try

            Return intRet

        End Using
    End Function

    Public Function GetCarrierFilteredByLegalEntity(ByVal CarrierLegalEntity As String, ByVal CarrierAlphaCode As String, ByVal CarrierNumber As Integer) As DataTransferObjects.Carrier

        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'if there are no null values to deal with we can build the array directly
                If String.IsNullOrEmpty(CarrierAlphaCode) Then
                    If Not CarrierNumber = 0 Then
                        Dim Carrier As DataTransferObjects.Carrier = (
                                From d In db.Carriers
                                Where d.CarrierNumber = CarrierNumber
                                Order By d.CarrierNumber
                                Select selectDTOData(d, db)).FirstOrDefault()
                        Return Carrier
                    End If
                Else
                    Dim Carrier As DataTransferObjects.Carrier = (
                            From d In db.Carriers
                            Where d.CarrierAlphaCode = CarrierAlphaCode _
                                  And d.CarrierLegalEntity = CarrierLegalEntity
                            Order By d.CarrierNumber
                            Select selectDTOData(d, db)).FirstOrDefault()
                    Return Carrier
                End If

                'This is where we would test the CarrierNumber parameter and throw and exception'
                Dim largs As New List(Of String) From {"Carrier", "CarrierNumber", CarrierNumber, CarrierAlphaCode}
                throwInvalidKeyFilterMetaDataException(SqlFaultInfo.FaultDetailsKey.E_CannotSaveRequiredValueDoesNotMatch, largs)
                'To catch this exception, you must check the SqlFaultInfo.message property for E_InvalidKeyFilterMetaData
                'To display the actual message in the log, you must interpret the SqlFaultInfo.Details emunerator 
                'and use the String.Format method with the SqlFaultInfo.DetailsList as the parameter array
                'Typically, we use the localization logic to look up the language reference for the enumerator 
                'E_CannotSaveRequiredValueDoesNotMatch, however, the web services does not have a reference to the localization 
                'project so we must manually copy the text into the String.Format method until such time as localization has been completed
                'Example: Log(String.Format("Cannot save changes to {0}. The key field {1} must be unique; the value {2} is already in use for AlphaCode {3}.", largs)
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCarrierFilteredByLegalEntity"))
            End Try

            Return Nothing

        End Using

    End Function

    ''' <summary>
    ''' Returns all carriers filtered by Legal Entity, does not support paging use GetCarriersFilterd  if paging is required
    ''' </summary>
    ''' <param name="CarrierLegalEntity"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR v-7.0.5.110 8/12/20126
    '''   calls overload for GetCarriersFiltered
    ''' </remarks>
    Public Function GetCarriersFilteredByLegalEntity(ByVal CarrierLegalEntity As String) As DataTransferObjects.Carrier()
        Return GetCarriersFiltered(0, CarrierLegalEntity)
    End Function

    Public Function GetLatestCarrierFilteredByLegalEntity(ByVal CarrierLegalEntity As String) As DataTransferObjects.Carrier

        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'if there are no null values to deal with we can build the array directly
                Dim Carrier As DataTransferObjects.Carrier = (
                        From d In db.Carriers
                        Where d.CarrierLegalEntity = CarrierLegalEntity
                        Order By d.CarrierControl Descending
                        Select selectDTOData(d, db)).FirstOrDefault()
                Return Carrier
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetLatestCarrierFilteredByLegalEntity"))
            End Try

            Return Nothing

        End Using

    End Function

    ''' <summary>
    ''' looks up the name and number for the specific carrier control; retuns a dictionary with 
    ''' CarrierName and CarrierNumber as the keys.
    ''' </summary>
    ''' <param name="CarrierControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function getCarrierNameNumber(ByVal CarrierControl As Integer) As Dictionary(Of String, String)
        Dim dictRet As New Dictionary(Of String, String)
        If CarrierControl = 0 Then Return dictRet 'return an empty dictionary not valid
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim oCars = db.Carriers.Where(Function(x) x.CarrierControl = CarrierControl)
                For Each c As LTS.Carrier In oCars
                    If Not c Is Nothing Then
                        dictRet.Add("CarrierName", c.CarrierName)
                        dictRet.Add("CarrierNumber", c.CarrierNumber.ToString())
                    End If
                    Exit For 'just get the first on
                Next
                'If Not oCars Is Nothing AndAlso oCars.Count > 0 Then
                '    Dim oCar = oCars(0)
                '    dictRet.Add("CarrierName", oCar.CarrierName)
                '    dictRet.Add("CarrierNumber", oCar.CarrierNumber.ToString())
                'End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("getCarrierNameNumber"))
            End Try
            Return dictRet
        End Using

    End Function

    Public Function getCarrierBySCAC(ByVal SCAC As String, ByVal sLegalEntity As String) As LTS.Carrier
        Dim oRet As New LTS.Carrier
        If String.IsNullOrWhiteSpace(SCAC) Then Return oRet 'nothing to lookup

        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                oRet = db.Carriers.Where(Function(x) x.CarrierSCAC = SCAC And x.CarrierLegalEntity = sLegalEntity).FirstOrDefault()

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("getCarrierBySCAC"))
            End Try
            Return oRet
        End Using

    End Function

    ''' <summary>
    ''' looks up the name, number, and SCAC for the specific carrier control; retuns a dictionary with 
    ''' CarrierName, CarrierNumber, and CarrierSCAC as the keys.
    ''' </summary>
    ''' <param name="CarrierControl"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Added by RHR for v-7.0.6.105 -6.0.4.70 on 5/31/2017
    ''' Modified by RHR for v-8.5.4.004 on 11/17/2023
    '''     fixed bug where Dictionary(Of String, String) does not accept nothing
    ''' </remarks>
    Public Function getCarrierNameNumberSCAC(ByVal CarrierControl As Integer) As Dictionary(Of String, String)
        Dim dictRet As New Dictionary(Of String, String)
        If CarrierControl = 0 Then Return dictRet 'return an empty dictionary not valid
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim oCars = db.Carriers.Where(Function(x) x.CarrierControl = CarrierControl)
                For Each c As LTS.Carrier In oCars
                    If Not c Is Nothing Then
                        dictRet.Add("CarrierName", If(c.CarrierName, String.Empty)) 'Modified by RHR for v-8.5.4.004 on 11/17/2023
                        dictRet.Add("CarrierNumber", If(c.CarrierNumber.HasValue, c.CarrierNumber.HasValue.ToString(), String.Empty)) 'Modified by RHR for v-8.5.4.004 on 11/17/2023
                        dictRet.Add("CarrierSCAC", If(c.CarrierSCAC, String.Empty)) 'Modified by RHR for v-8.5.4.004 on 11/17/2023
                    End If
                    Exit For 'just get the first on
                Next
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("getCarrierNameNumberSCAC"))
            End Try
            Return dictRet
        End Using

    End Function

    ''' <summary>
    ''' Returns the next availableCarrier Number. Called when creating a new record in the ValidateCarrierBeforeInsert method (70)
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR for v-8.2 on 1/9/2019
    ''' we now return 1 if no data is returned
    ''' </remarks>
    Public Function GetNextCarrierNumber() As Integer
        Dim intRet As Integer = 1
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                If db.Carriers.Any(Function(x) x.CarrierNumber > 0) Then
                    'if there are no null values to deal with we can build the array directly
                    intRet = (
                        From t In db.Carriers Order By t.CarrierNumber Descending
                            Select t.CarrierNumber).FirstOrDefault()
                    If intRet > 0 Then intRet += 1
                    Return intRet
                Else
                    Return 1 'this is the first carrier
                End If


            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_SQLExceptionMSG", .Details = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                'just return 1
                Return 1
                ' Utilities.SaveAppError(ex.Message, Me.Parameters)
                ' Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData", .Details = ex.Message}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_UnExpectedMSG", .Details = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return intRet

        End Using

    End Function

    Public Function DoesRecordExist(ByVal CarrierControl As Integer) As Boolean
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim blnExists = db.Carriers.Any(Function(x) x.CarrierControl = CarrierControl)
                Return blnExists
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("DoesRecordExist"))
            End Try
        End Using
    End Function

    ''' <summary>
    ''' Deletes a Carrier record by control number without validation. It can only be called by system only process.
    ''' </summary>
    ''' <param name="ControlNumber"></param>
    ''' <remarks></remarks>
    Public Sub SystemDelete(ByVal ControlNumber As Integer)
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Dim nObject = db.Carriers.Where(Function(x) x.CarrierControl = ControlNumber).FirstOrDefault()
            If Not nObject Is Nothing AndAlso nObject.CarrierControl <> 0 Then
                db.Carriers.DeleteOnSubmit(nObject)
                Try
                    db.SubmitChanges()
                Catch ex As Exception
                    ManageLinqDataExceptions(ex, buildProcedureName("SystemDelete"), db)
                End Try
            End If
        End Using
    End Sub

    'Public Sub SystemUpdateQuick(ByVal oData As DTO.Carrier)
    '    Using db As New NGLMASCarrierDataContext(ConnectionString)

    '        'Create New Record 
    '        Dim nObject = CopyDTOToLinq(oData)
    '        ' Attach the record 
    '        db.Carriers.Attach(nObject, True)
    '        Try
    '            db.SubmitChanges()
    '        Catch ex As Exception
    '            ManageLinqDataExceptions(ex, buildProcedureName("SystemUpdateQuick"))
    '        End Try

    '    End Using
    'End Sub

    ''' <summary>
    ''' Validates the carrier name then checks if a matching carrier record exists based on the parameters provided; 
    ''' also gets the next carrier nunmber if the CarrierNumber parameter is zero;  if the validation fails the function returns 
    ''' false and updates the ValidationMsg with details about the failure.
    ''' </summary>
    ''' <param name="CarrierNumber"></param>
    ''' <param name="CarrierName"></param>
    ''' <param name="CarrierLegalEntity"></param>
    ''' <param name="CarrierAlphaCode"></param>
    ''' <param name="ValidationMsg"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ValidateCarrierBeforeInsert(ByRef CarrierNumber As Integer, ByVal CarrierName As String, ByVal CarrierLegalEntity As String, ByVal CarrierAlphaCode As String, ByRef ValidationMsg As String) As Boolean
        Dim blnRet As Boolean = True
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Validate that required fields have been entered
                If String.IsNullOrEmpty(ValidationMsg) Then ValidationMsg = ""
                Dim strSpacer As String = ""
                If String.IsNullOrEmpty(CarrierName) OrElse CarrierName.Trim.Length < 1 Then
                    blnRet = False
                    ValidationMsg &= strSpacer & "the Carrier Name is not valid"
                    strSpacer = ", and "
                End If
                Dim blnExists As Boolean = False
                If blnRet Then
                    If CarrierNumber = 0 Then
                        If Not String.IsNullOrEmpty(CarrierAlphaCode) AndAlso CarrierAlphaCode.Trim.Length > 0 Then
                            If Not String.IsNullOrEmpty(CarrierLegalEntity) AndAlso CarrierLegalEntity.Trim.Length > 0 Then
                                'it is possible for the CarrierAlphaCode to duplciate across Legal Entities
                                blnExists = db.Carriers.Any(Function(x) x.CarrierName = CarrierName And x.CarrierAlphaCode = CarrierAlphaCode And x.CarrierLegalEntity = CarrierLegalEntity)
                            Else
                                blnExists = db.Carriers.Any(Function(x) x.CarrierName = CarrierName And x.CarrierAlphaCode = CarrierAlphaCode)
                            End If
                        Else
                            blnExists = db.Carriers.Any(Function(x) x.CarrierName = CarrierName)
                        End If
                        If Not blnExists Then
                            'get the new carrier number
                            CarrierNumber = GetNextCarrierNumber()
                            If CarrierNumber = 0 Then
                                blnRet = False
                                ValidationMsg &= strSpacer & "the Carrier Number is not valid"
                                strSpacer = ", and "
                            End If
                        End If
                    Else
                        Dim intCarrierNumberFilter As Integer = CarrierNumber 'we cannot use a byRef argument in a lambda expression.
                        If db.Carriers.Any(Function(x) x.CarrierName <> CarrierName And x.CarrierNumber = intCarrierNumberFilter) Then
                            ValidationMsg &= strSpacer & "Carrier Number " + CarrierNumber.ToString() + " is already in use by another Carrier"
                            Return False
                        End If
                        If Not String.IsNullOrEmpty(CarrierAlphaCode) AndAlso CarrierAlphaCode.Trim.Length > 0 Then
                            If Not String.IsNullOrEmpty(CarrierLegalEntity) AndAlso CarrierLegalEntity.Trim.Length > 0 Then
                                'it is possible for the CarrierAlphaCode to duplciate across Legal Entities
                                blnExists = db.Carriers.Any(Function(x) x.CarrierName = CarrierName And x.CarrierNumber = intCarrierNumberFilter And x.CarrierAlphaCode = CarrierAlphaCode And x.CarrierLegalEntity = CarrierLegalEntity)
                            Else
                                blnExists = db.Carriers.Any(Function(x) x.CarrierName = CarrierName And x.CarrierNumber = intCarrierNumberFilter And x.CarrierAlphaCode = CarrierAlphaCode)
                            End If
                        Else
                            blnExists = db.Carriers.Any(Function(x) x.CarrierName = CarrierName And x.CarrierNumber = intCarrierNumberFilter)
                        End If
                    End If
                End If
                If blnExists Then
                    blnRet = False
                    ValidationMsg &= strSpacer & "the Carrier record already exists"
                    strSpacer = ", and "
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("ValidateCarrierBeforeInsert"))
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Validates the carrier name and carrier number then checks if a matching carrier record exists for a carrier other than the current carriercontrol carrier
    ''' based on the parameters provided; if the validation fails the function returns 
    ''' false and updates the ValidationMsg with details about the failure.
    ''' </summary>
    ''' <param name="CarrierControl"></param>
    ''' <param name="CarrierNumber"></param>
    ''' <param name="CarrierName"></param>
    ''' <param name="CarrierLegalEntity"></param>
    ''' <param name="CarrierAlphaCode"></param>
    ''' <param name="ValidationMsg"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ValidateCarrierBeforeUpdate(ByVal CarrierControl As Integer, ByVal CarrierNumber As Integer, ByVal CarrierName As String, ByVal CarrierLegalEntity As String, ByVal CarrierAlphaCode As String, ByRef ValidationMsg As String) As Boolean
        Dim blnRet As Boolean = True
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Validate that required fields have been entered
                If String.IsNullOrEmpty(ValidationMsg) Then ValidationMsg = ""
                Dim strSpacer As String = ""
                If String.IsNullOrEmpty(CarrierName) OrElse CarrierName.Trim.Length < 1 Then
                    blnRet = False
                    ValidationMsg &= strSpacer & "the Carrier Name is not valid"
                    strSpacer = ", and "
                End If
                If CarrierNumber = 0 Then
                    blnRet = False
                    ValidationMsg &= strSpacer & "the Carrier Number is not valid"
                    strSpacer = ", and "
                Else
                    If db.Carriers.Any(Function(x) x.CarrierControl <> CarrierControl And x.CarrierNumber = CarrierNumber) Then
                        blnRet = False
                        ValidationMsg &= strSpacer & "Carrier Number " + CarrierNumber.ToString() + " is already in use by another Carrier"
                        strSpacer = ", and "
                    End If
                End If
                Dim blnExists As Boolean = False
                If blnRet Then
                    If Not String.IsNullOrEmpty(CarrierAlphaCode) AndAlso CarrierAlphaCode.Trim.Length > 0 Then
                        If Not String.IsNullOrEmpty(CarrierLegalEntity) AndAlso CarrierLegalEntity.Trim.Length > 0 Then
                            'it is possible for the CarrierAlphaCode to duplciate across Legal Entities
                            blnExists = db.Carriers.Any(Function(x) x.CarrierControl <> CarrierControl And x.CarrierName = CarrierName And x.CarrierNumber = CarrierNumber And x.CarrierAlphaCode = CarrierAlphaCode And x.CarrierLegalEntity = CarrierLegalEntity)
                        Else
                            blnExists = db.Carriers.Any(Function(x) x.CarrierControl <> CarrierControl And x.CarrierName = CarrierName And x.CarrierNumber = CarrierNumber And x.CarrierAlphaCode = CarrierAlphaCode)
                        End If
                    Else
                        blnExists = db.Carriers.Any(Function(x) x.CarrierControl <> CarrierControl And x.CarrierName = CarrierName And x.CarrierNumber = CarrierNumber)
                    End If
                End If
                If blnExists Then
                    blnRet = False
                    ValidationMsg &= strSpacer & "the Carrier record already exists"
                    strSpacer = ", and "
                End If
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("ValidateCarrierBeforeUpdate"))
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' The CarrierActiveFlag is used to determine which records to return 
    ''' 0 = all records
    ''' 1 = only records where CarrierActive is true
    ''' Any other value returns records where CarrierActive is False
    ''' Provide an optional Legal Entity for additional filters
    ''' Returns the first 1000 records unless page options are provided
    ''' </summary>
    ''' <param name="CarrierActiveFlag"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR v-7.0.5.110 8/11/2016
    '''   added additional filters and paging support
    '''   added skip and take parameters to support
    '''   interface with TMS 365 pages
    ''' </remarks>
    Public Function GetCarriersFiltered(Optional ByVal CarrierActiveFlag As Integer = 0,
                                        Optional ByVal CarrierLegalEntity As String = Nothing,
                                        Optional ByVal SortByAlpa As Boolean = False,
                                        Optional ByVal page As Integer = 1,
                                        Optional ByVal pagesize As Integer = 1000,
                                        Optional ByVal skip As Integer = 0,
                                        Optional ByVal take As Integer = 0) As DataTransferObjects.Carrier()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                Dim intRecordCount As Integer = 0
                Dim intPageCount As Integer = 1
                'db.Log = New DebugTextWriter
                intRecordCount = (From t In db.Carriers
                    Where
                        (If(t.CarrierActive, False) = If(CarrierActiveFlag = 0, If(t.CarrierActive, False), If(CarrierActiveFlag = 1, True, False))) _
                        And
                        (String.IsNullOrWhiteSpace(CarrierLegalEntity) OrElse t.CarrierLegalEntity = CarrierLegalEntity)
                    Select t.CarrierControl).Count()

                If take <> 0 Then
                    pagesize = take
                Else
                    'calculate based on page and pagesize
                    If pagesize < 1 Then pagesize = 1
                    If intRecordCount < 1 Then intRecordCount = 1
                    If page < 1 Then page = 1
                    skip = (page - 1) * pagesize
                End If
                If intRecordCount > pagesize Then intPageCount = ((intRecordCount - 1) \ pagesize) + 1

                'Return all the carriers that match the criteria sorted order
                Dim Carriers() As DataTransferObjects.Carrier




                If SortByAlpa Then
                    Carriers = (
                        From t In db.Carriers
                            Where
                                (If(t.CarrierActive, False) = If(CarrierActiveFlag = 0, If(t.CarrierActive, False), If(CarrierActiveFlag = 1, True, False))) _
                                And
                                (String.IsNullOrWhiteSpace(CarrierLegalEntity) OrElse t.CarrierLegalEntity = CarrierLegalEntity)
                            Order By t.CarrierName
                            Select selectDTOData(t, db, page, intPageCount, intRecordCount, pagesize)).Skip(skip).Take(pagesize).ToArray()
                Else
                    Carriers = (
                        From t In db.Carriers
                            Where
                                (If(t.CarrierActive, False) = If(CarrierActiveFlag = 0, If(t.CarrierActive, False), If(CarrierActiveFlag = 1, True, False))) _
                                And
                                (String.IsNullOrWhiteSpace(CarrierLegalEntity) OrElse t.CarrierLegalEntity = CarrierLegalEntity)
                            Order By t.CarrierControl
                            Select selectDTOData(t, db, page, intPageCount, intRecordCount, pagesize)).Skip(skip).Take(pagesize).ToArray()
                End If
                Return Carriers

            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                ManageLinqDataExceptions(ex, buildProcedureName("GetCarriersFiltered"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetCarrierAllowWebTender(ByVal CarrierControl As Integer) As Boolean
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Get the newest record that matches the provided criteria
                Dim Carrier As DataTransferObjects.Carrier = (
                        From d In db.Carriers
                        Where
                        d.CarrierControl = CarrierControl
                        Select New DataTransferObjects.Carrier With {.CarrierControl = d.CarrierControl, .CarrierAllowWebTender = If(d.CarrierAllowWebTender.HasValue, d.CarrierAllowWebTender.Value, False)}).First
                'If we get here then a record exists for the desired action so return true
                Return Carrier.CarrierAllowWebTender
            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try
            'we return false by default
            Return False

        End Using
    End Function

    Public Function GetNatFuelCrossTabData() As DataTransferObjects.NatFuelCrossTab()
        'Create a data connection 
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim oList As New List(Of NatFuelCrossTab)

        Try

            Dim strSQL As String = "exec dbo.spNatFuelCrossTab "
            Dim oQRet As Ngl.Core.Data.QueryResult = oQuery.ExecuteWithFill(strSQL)
            If Not oQRet.Exception Is Nothing Then
                Utilities.SaveAppError(oQRet.Exception.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
            End If
            If Not oQRet.Data Is Nothing AndAlso oQRet.Data.Rows.Count > 0 Then
                For Each oRow As System.Data.DataRow In oQRet.Data.Rows
                    Dim oItem As New DataTransferObjects.NatFuelCrossTab
                    With oItem
                        .NatFuelID = DataTransformation.getDataRowValue(oRow, "NatFuelID", 0)

                        .NatFuelDate = DataTransformation.getDataRowValue(oRow, "NatFuelDate")

                        .NatAverage = DataTransformation.getDataRowValue(oRow, "NatAverage", 0)

                        .ZoneFuelCost1 = DataTransformation.getDataRowValue(oRow, "ZoneFuelCost1", 0)

                        .ZoneFuelCost2 = DataTransformation.getDataRowValue(oRow, "ZoneFuelCost2", 0)

                        .ZoneFuelCost3 = DataTransformation.getDataRowValue(oRow, "ZoneFuelCost3", 0)

                        .ZoneFuelCost4 = DataTransformation.getDataRowValue(oRow, "ZoneFuelCost4", 0)

                        .ZoneFuelCost5 = DataTransformation.getDataRowValue(oRow, "ZoneFuelCost5", 0)

                        .ZoneFuelCost6 = DataTransformation.getDataRowValue(oRow, "ZoneFuelCost6", 0)

                        .ZoneFuelCost7 = DataTransformation.getDataRowValue(oRow, "ZoneFuelCost7", 0)

                        .ZoneFuelCost8 = DataTransformation.getDataRowValue(oRow, "ZoneFuelCost8", 0)

                        .ZoneFuelCost9 = DataTransformation.getDataRowValue(oRow, "ZoneFuelCost9", 0)

                        .ZoneFuelName1 = DataTransformation.getDataRowValue(oRow, "ZoneFuelName1", "")

                        .ZoneFuelName2 = DataTransformation.getDataRowValue(oRow, "ZoneFuelName2", "")

                        .ZoneFuelName3 = DataTransformation.getDataRowValue(oRow, "ZoneFuelName3", "")

                        .ZoneFuelName4 = DataTransformation.getDataRowValue(oRow, "ZoneFuelName4", "")

                        .ZoneFuelName5 = DataTransformation.getDataRowValue(oRow, "ZoneFuelName5", "")

                        .ZoneFuelName6 = DataTransformation.getDataRowValue(oRow, "ZoneFuelName6", "")

                        .ZoneFuelName7 = DataTransformation.getDataRowValue(oRow, "ZoneFuelName7", "")

                        .ZoneFuelName8 = DataTransformation.getDataRowValue(oRow, "ZoneFuelName8", "")

                        .ZoneFuelName9 = DataTransformation.getDataRowValue(oRow, "ZoneFuelName9", "")

                    End With
                    oList.Add(oItem)
                Next
            Else
                oList.Add(New DataTransferObjects.NatFuelCrossTab)
            End If
            Return oList.ToArray()
        Catch ex As FaultException
            Throw
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As InvalidOperationException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            oQuery = Nothing
        End Try

        Return Nothing
    End Function
    ''' <summary>
    ''' Method to Display the List of NatFuelZones
    ''' </summary>
    ''' <param name="RecordCount">No Of Records</param>
    ''' <param name="filters">Filter settings</param>
    ''' <remarks>
    ''' Added to display the natfuel records based on Filters this is copy of existing Function(GetNatFuelCrossTabData) ,Added filters extra for paging,Sorting..etc. 
    ''' Added by ManoRama for Carrier Fuel Index maintenance changes.
    ''' </remarks>
    ''' <returns>View vNatFuelCrossTab</returns>
    Public Function GetNatFuelCrossTabData365(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.vNatFuelCrossTab()
        If filters Is Nothing Then Return Nothing
        'Create a data connection 
        Dim oQuery As New Ngl.Core.Data.Query(ConnectionString)
        Dim oList As New List(Of NatFuelCrossTab)
        Dim oList1 As New List(Of LTS.vNatFuelCrossTab)
        Dim oRet() As LTS.vNatFuelCrossTab
        Try
            Dim strSQL As String = "exec dbo.spNatFuelCrossTab "
            Dim oQRet As Ngl.Core.Data.QueryResult = oQuery.ExecuteWithFill(strSQL)
            If Not oQRet.Exception Is Nothing Then
                Utilities.SaveAppError(oQRet.Exception.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_FailedToExecute"}, New FaultReason("E_DataAccessError"))
            End If
            If Not oQRet.Data Is Nothing AndAlso oQRet.Data.Rows.Count > 0 Then
                For Each oRow As System.Data.DataRow In oQRet.Data.Rows
                    Dim oItem As New LTS.vNatFuelCrossTab
                    With oItem
                        .NatFuelID = DataTransformation.getDataRowValue(oRow, "NatFuelID", 0)

                        .NatFuelDate = DataTransformation.getDataRowValue(oRow, "NatFuelDate")

                        .NatAverage = DataTransformation.getDataRowValue(oRow, "NatAverage", 0)

                        .ZoneFuelCost1 = DataTransformation.getDataRowValue(oRow, "ZoneFuelCost1", 0)

                        .ZoneFuelCost2 = DataTransformation.getDataRowValue(oRow, "ZoneFuelCost2", 0)

                        .ZoneFuelCost3 = DataTransformation.getDataRowValue(oRow, "ZoneFuelCost3", 0)

                        .ZoneFuelCost4 = DataTransformation.getDataRowValue(oRow, "ZoneFuelCost4", 0)

                        .ZoneFuelCost5 = DataTransformation.getDataRowValue(oRow, "ZoneFuelCost5", 0)

                        .ZoneFuelCost6 = DataTransformation.getDataRowValue(oRow, "ZoneFuelCost6", 0)

                        .ZoneFuelCost7 = DataTransformation.getDataRowValue(oRow, "ZoneFuelCost7", 0)

                        .ZoneFuelCost8 = DataTransformation.getDataRowValue(oRow, "ZoneFuelCost8", 0)

                        .ZoneFuelCost9 = DataTransformation.getDataRowValue(oRow, "ZoneFuelCost9", 0)

                        .ZoneFuelName1 = DataTransformation.getDataRowValue(oRow, "ZoneFuelName1", "")

                        .ZoneFuelName2 = DataTransformation.getDataRowValue(oRow, "ZoneFuelName2", "")

                        .ZoneFuelName3 = DataTransformation.getDataRowValue(oRow, "ZoneFuelName3", "")

                        .ZoneFuelName4 = DataTransformation.getDataRowValue(oRow, "ZoneFuelName4", "")

                        .ZoneFuelName5 = DataTransformation.getDataRowValue(oRow, "ZoneFuelName5", "")

                        .ZoneFuelName6 = DataTransformation.getDataRowValue(oRow, "ZoneFuelName6", "")

                        .ZoneFuelName7 = DataTransformation.getDataRowValue(oRow, "ZoneFuelName7", "")

                        .ZoneFuelName8 = DataTransformation.getDataRowValue(oRow, "ZoneFuelName8", "")

                        .ZoneFuelName9 = DataTransformation.getDataRowValue(oRow, "ZoneFuelName9", "")

                    End With
                    oList1.Add(oItem)
                Next
            Else
                oList.Add(New DataTransferObjects.NatFuelCrossTab)
            End If
            Dim iQuery As IQueryable(Of LTS.vNatFuelCrossTab)
            iQuery = oList1.AsQueryable()
            Dim filterWhere = ""
            ' oQuery.Log = New DebugTextWriter
            If String.IsNullOrWhiteSpace(filters.sortName) Then
                filters.sortName = "NatFuelDate"
                filters.sortDirection = "DESC"
            End If
            ApplyAllFilters(iQuery, filters, filterWhere)
            PrepareQuery(iQuery, filters, RecordCount)
            oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
            Return oRet

        Catch ex As FaultException
            Throw
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As InvalidOperationException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        Finally
            oQuery = Nothing
        End Try

        Return Nothing
    End Function

    Public Function CreateNatFuelCrossTabData(ByVal oData As DataTransferObjects.NatFuelCrossTab) As Integer
        Dim intRet As Integer
        Dim strProcName As String = "dbo.spInsertNatFuelIndex"
        Dim oCmd As New System.Data.SqlClient.SqlCommand
        oCmd.Parameters.AddWithValue("@NatFuelDate", oData.NatFuelDate)
        oCmd.Parameters.AddWithValue("@NatFuelNatAvg", oData.NatAverage)
        oCmd.Parameters.AddWithValue("@NatFuelZone1Avg", oData.ZoneFuelCost1)
        oCmd.Parameters.AddWithValue("@NatFuelZone2Avg", oData.ZoneFuelCost2)
        oCmd.Parameters.AddWithValue("@NatFuelZone3Avg", oData.ZoneFuelCost3)
        oCmd.Parameters.AddWithValue("@NatFuelZone4Avg", oData.ZoneFuelCost4)
        oCmd.Parameters.AddWithValue("@NatFuelZone5Avg", oData.ZoneFuelCost5)
        oCmd.Parameters.AddWithValue("@NatFuelZone6Avg", oData.ZoneFuelCost6)
        oCmd.Parameters.AddWithValue("@NatFuelZone7Avg", oData.ZoneFuelCost7)
        oCmd.Parameters.AddWithValue("@NatFuelZone8Avg", oData.ZoneFuelCost8)
        oCmd.Parameters.AddWithValue("@NatFuelZone9Avg", oData.ZoneFuelCost9)
        runNGLStoredProcedure(oCmd, strProcName, 2)
        Try
            intRet = getScalarInteger("Select NatFuelID From dbo.NatFuel Where NatFuelDate = '" & oData.NatFuelDate.ToString & "'")
        Catch ex As FaultException
            Throw
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        End Try
        Return intRet
    End Function

    Public Sub DeleteNatFuelCrossTabData(ByVal NatFuelID As Integer)
        Try
            executeSQL("Delete from NatFuel where NatFuelID = " & NatFuelID)
        Catch ex As FaultException
            Throw
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        End Try

    End Sub

    Public Function GetCarrierOptimizerTruckData(ByVal BookControl As Integer) As DataTransferObjects.CarrierOptimizerTruckData()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                Dim CarrierOptimizerTruckData() As DataTransferObjects.CarrierOptimizerTruckData = (
                        From d In db.spGetTruckDataFile(BookControl)
                        Select New DataTransferObjects.CarrierOptimizerTruckData With {.CarrierControl = If(d.CarrierControl.HasValue, d.CarrierControl, 0),
                        .CasesAvailable = If(d.CasesAvailable.HasValue, d.CasesAvailable, 0),
                        .CubesAvailable = If(d.CubesAvailable.HasValue, d.CubesAvailable, 0),
                        .DropCost = If(d.DropCost.HasValue, d.DropCost, 0),
                        .ErrMsg = d.ErrMsg,
                        .FlatRate = If(d.FlatRate.HasValue, d.FlatRate, 0),
                        .MileRate = If(d.MileRate.HasValue, d.MileRate, 0),
                        .PerUnitCost = If(d.PerUnitCost.HasValue, d.PerUnitCost, 0),
                        .PltsAvailable = If(d.PltsAvailable.HasValue, d.PltsAvailable, 0),
                        .SpecialCodes = d.SpecialCodes,
                        .TruckControl = d.TruckControl,
                        .WgtAvailable = If(d.WgtAvailable.HasValue, d.WgtAvailable, 0)
                        }).ToArray()

                Return CarrierOptimizerTruckData

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetvCarrierFiltered(ByVal Number As Integer) As DataTransferObjects.vCarrier
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Get the newest record that matches the provided criteria
                Dim Carr As DataTransferObjects.vCarrier = (
                        From t In db.vCarriers()
                        Where t.CarrierNumber = If(Number = 0, t.CarrierNumber, Number)
                        Select New DataTransferObjects.vCarrier With {.CarrierControl = t.CarrierControl _
                        , .CarrierNumber = If(t.CarrierNumber.HasValue, t.CarrierNumber.Value, 0) _
                        , .CarrierName = t.CarrierName _
                        , .CarrierAllowWebTender = If(t.CarrierAllowWebTender.HasValue, t.CarrierAllowWebTender.Value, False)}).First

                Return Carr

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                'Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
                'throw away no data errors.
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetvCarrierData(ByVal filterOutExpenseCarriers As Boolean) As DataTransferObjects.vCarrier()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim vCarrierData() As DataTransferObjects.vCarrier
                If filterOutExpenseCarriers Then
                    vCarrierData = (
                        From d In db.Carriers() Where d.CarrierActive = True And d.CarrierTypeCode.Trim.ToUpper <> "X" Order By d.CarrierName Ascending
                            Select New DataTransferObjects.vCarrier With {.CarrierControl = d.CarrierControl,
                                .CarrierAllowWebTender = If(d.CarrierAllowWebTender.HasValue, d.CarrierAllowWebTender.Value, False),
                                .CarrierName = d.CarrierName,
                                .CarrierNumber = If(d.CarrierNumber.HasValue, d.CarrierNumber.Value, 0)}).ToArray()
                Else
                    vCarrierData = (
                        From d In db.vCarriers() Order By d.CarrierName Ascending
                            Select New DataTransferObjects.vCarrier With {.CarrierControl = d.CarrierControl,
                                .CarrierAllowWebTender = If(d.CarrierAllowWebTender.HasValue, d.CarrierAllowWebTender.Value, False),
                                .CarrierName = d.CarrierName,
                                .CarrierNumber = If(d.CarrierNumber.HasValue, d.CarrierNumber.Value, 0)}).ToArray()
                End If

                Return vCarrierData

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                'Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
                'throw away no data errors.
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

            Return Nothing

        End Using
    End Function

    Public Function GetCarrierVLookup(ByVal carrierControl As Integer) As DataTransferObjects.vLookupList

        Try
            Using db As New NGLMASCarrierDataContext(ConnectionString)
                Dim result As DataTransferObjects.vLookupList = (From t In db.Carriers
                        Where t.CarrierControl = If(carrierControl = 0, t.CarrierControl, carrierControl)
                        Select New DataTransferObjects.vLookupList _
                        With {.Control = t.CarrierControl, .Name = t.CarrierName, .Description = t.CarrierNumber}).FirstOrDefault
                Return result
            End Using
        Catch ex As System.Data.SqlClient.SqlException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
        Catch ex As InvalidOperationException
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        End Try

        Return Nothing
    End Function

    ''' <summary>
    ''' Returns -1 on error, 0 if carrier does not exist or carriercontrol if record exists
    ''' </summary>
    ''' <param name="CarrierNumber"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCarrierControlIfExist(ByVal CarrierNumber As String) As Integer
        Dim intRet As Integer = -1
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'db.Log = New DebugTextWriter
                'Get the control number
                Dim Control = (From t In db.Carriers
                        Where (t.CarrierNumber = CarrierNumber)
                        Select t.CarrierControl).First

                Return Control

            Catch ex As System.Data.SqlClient.SqlException
                Return -1
            Catch ex As InvalidOperationException
                'this indicates that no records exist so return 0
                Return 0
            Catch ex As Exception
                Throw
            End Try

            Return intRet

        End Using
    End Function

    ''' <summary>
    ''' Returns a list of active carrier controls.  
    ''' Do not use with WCF Service methods directly because list(of integer) does not implement ChangeTrackingCollection
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetActiveCarrierControls() As List(Of Integer)
        Dim lRet As New List(Of Integer)
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                lRet = (From d In db.Carriers Where d.CarrierActive = True Select d.CarrierControl).ToList()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetActiveCarrierControls"))
            End Try
            Return lRet
        End Using
    End Function

    ''' <summary>
    ''' Returns a list of all carrier controls.  
    ''' Do not use with WCF Service methods directly because list(of integer) does not implement ChangeTrackingCollection
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAllCarrierControls() As List(Of Integer)
        Dim lRet As New List(Of Integer)
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                lRet = (From d In db.Carriers Select d.CarrierControl).ToList()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetActiveCarrierControls"))
            End Try
            Return lRet
        End Using
    End Function

    ''' <summary>
    ''' Returns a comma seperated list of Language Reference Strings 
    ''' the caller must split the strings and use localization procedures to 
    ''' produce user friendly text
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ValidateCarrierBeforeSelected(ByVal CarrierControl As Integer) As String
        Dim sRet As String = ""
        Dim sMsgs As New List(Of String)
        Dim dtCurrent = Date.Now()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                Dim oCarrier = From d In db.Carriers Where d.CarrierControl = CarrierControl Select d.CarrierQualQualified, d.CarrierQualInsuranceDate, d.CarrierQualContractExpiresDate, d.CarrierQualContract

                If Not oCarrier Is Nothing Then
                    With oCarrier(0)
                        If Not If(.CarrierQualQualified, True) Then sMsgs.Add("W_CarrierNotQualified")
                        If If(.CarrierQualInsuranceDate, dtCurrent) < dtCurrent Then sMsgs.Add("W_CarrierInsExpired")
                        If Not If(.CarrierQualContract, False) OrElse If(.CarrierQualContractExpiresDate, dtCurrent) < dtCurrent Then sMsgs.Add("W_CarrierContractExpired")
                    End With
                End If

                If sMsgs.Count > 0 Then sRet = String.Join(",", sMsgs)
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("ValidateCarrierWhenSelected"))
            End Try
            Return sRet
        End Using
    End Function

    ''' <summary>
    ''' Returns a comma seperated list of Language Reference Strings 
    ''' the caller must split the strings and use localization procedures to 
    ''' produce user friendly text
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ValidateCarrierWhenSelected(ByVal CarrierControl As Integer, ByVal TotalItemCost As Decimal) As String
        Dim sRet As String = ""
        Dim sMsgs As New List(Of String)
        Dim dtCurrent = Date.Now()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                Dim oCarrier = From d In db.Carriers Where d.CarrierControl = CarrierControl
                        Select d.CarrierQualQualified,
                        d.CarrierQualInsuranceDate,
                        d.CarrierQualContractExpiresDate,
                        d.CarrierQualContract,
                        d.CarrierQualMaxPerShipment,
                        d.CarrierQualCurAllExposure,
                        d.CarrierQualMaxAllShipments

                If Not oCarrier Is Nothing Then
                    With oCarrier(0)
                        If Not If(.CarrierQualQualified, True) Then sMsgs.Add("W_CarrierNotQualified")
                        If If(.CarrierQualInsuranceDate, dtCurrent) < dtCurrent Then sMsgs.Add("W_CarrierInsExpired")
                        If Not If(.CarrierQualContract, False) OrElse If(.CarrierQualContractExpiresDate, dtCurrent) < dtCurrent Then sMsgs.Add("W_CarrierContractExpired")
                        If TotalItemCost > 0 AndAlso If(.CarrierQualMaxPerShipment, 0) < TotalItemCost Then sMsgs.Add("W_CarrierOverMaxPerShip")
                        If If(.CarrierQualMaxAllShipments, 0) < If(.CarrierQualCurAllExposure, 0) Then sMsgs.Add("W_CarrierOverMaxAllShip")
                    End With
                End If

                If sMsgs.Count > 0 Then sRet = String.Join(",", sMsgs)
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("ValidateCarrierWhenSelected"))
            End Try
            Return sRet
        End Using
    End Function

    ''' <summary>
    ''' Returns the carrier name, number and CarrierContNotifyEmail information assigned to a specific order
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCarrierContactInformation(ByRef BookControl As Integer) As LTS.spGetCarrierContactInfoResult
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Return (From d In db.spGetCarrierContactInfo(BookControl) Select d).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCarrierContactInformation"))
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' returns the Carrier Qualificaiton fields used to qualify a carrier durring selection
    ''' </summary>
    ''' <param name="CarrierControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCarrierQual(ByVal CarrierControl As Integer) As LTS.vCarrierQual
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Return db.vCarrierQuals.Where(Function(x) x.CarrierQualCarrierControl = CarrierControl).FirstOrDefault()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCarrierQual"))
            End Try

            Return Nothing

        End Using
    End Function


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="CarrierControl"></param>
    ''' <remarks>
    ''' Added By LVV on 4/18/16 to v-7.0.5.1 Prepare For EDI
    ''' </remarks>
    Public Function prepareForEDI(ByVal CarrierControl As Integer) As Boolean
        Dim blnRet As Boolean = False
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                db.spUtilityPrepareForEDI(CarrierControl)
                blnRet = True

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("prepareForEDI"))
            End Try
        End Using
        Return blnRet
    End Function

    ''' <summary>
    ''' Gets global param DATCarrierNumber and uses it to look up Carrier info.
    ''' Uses Carrier Control to get the first CarrierCont record in the table (uses 
    '''  logic to default to 800 number but use phone number if 800 is empty)
    ''' Gets the next CNS for the comp using param BookCustCompControl
    ''' Creates and returns the CarrierTenderData DTO object
    ''' </summary>
    ''' <param name="BookCustCompControl"></param>
    ''' <returns>DTO.CarrierTenderData</returns>
    ''' <remarks>
    ''' Added by LVV 5/13/16 for v-7.0.5.1 DAT
    ''' </remarks>
    Public Function GetDATCarrierTenderData(ByVal BookCustCompControl As Integer) As DataTransferObjects.CarrierTenderData
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Dim carrTender As New DataTransferObjects.CarrierTenderData
            Try
                Dim DATCarrierNumber = GetSystemParameterValue("DATCarrierNumber")

                Dim Carrier As DataTransferObjects.Carrier = (
                        From t In db.Carriers
                        Where t.CarrierNumber = DATCarrierNumber
                        Select New DataTransferObjects.Carrier With {.CarrierControl = t.CarrierControl _
                        , .CarrierName = t.CarrierName}).First

                Dim CarrierCont As DataTransferObjects.CarrierCont = (
                        From t In db.CarrierConts
                        Where t.CarrierContCarrierControl = Carrier.CarrierControl
                        Order By t.CarrierContControl
                        Select New DataTransferObjects.CarrierCont With {.CarrierContControl = t.CarrierContControl _
                        , .CarrierContName = t.CarrierContName _
                        , .CarrierContactPhone = t.CarrierContactPhone _
                        , .CarrierContact800 = t.CarrierContact800}).First

                Dim oBatch As New NGLBatchProcessDataProvider(Me.Parameters)
                Dim nextCNS = oBatch.GetNextConsNumber(BookCustCompControl)

                With carrTender
                    .CarrierControl = Carrier.CarrierControl
                    .CarrierName = Carrier.CarrierName
                    .CarrierNumber = DATCarrierNumber
                    .CarrierContControl = CarrierCont.CarrierContControl
                    .CarrierContName = CarrierCont.CarrierContName
                    .CarrierContactPhone = If(String.IsNullOrEmpty(CarrierCont.CarrierContact800), CarrierCont.CarrierContactPhone, CarrierCont.CarrierContact800)
                    .BookConsPrefix = nextCNS
                End With

                Return carrTender

            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetDATCarrierTenderData"))
            End Try

            Return Nothing

        End Using

    End Function

    ''' <summary>
    ''' Count the number of records available by filter.  set all filters to null or empty string to count all records in the table
    ''' Set the optional active parameter to true to count only active records
    ''' The CarrierActiveFlag is used to determine which records to return 
    ''' 0 = all records
    ''' 1 = only records where CarrierActive is true
    ''' Any other value returns records where CarrierActive is False
    ''' </summary>
    ''' <param name="CarrierActiveFlag"></param>
    ''' <param name="CarrierLegalEntity"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR v-7.0.5.110 8/11/2016
    '''    added support for TMS 365 paging where count is expected
    ''' </remarks>
    Public Function CountRecords(Optional ByVal CarrierActiveFlag As Integer = 0,
                                 Optional ByVal CarrierLegalEntity As String = Nothing) As Integer
        Dim intRet As Integer = 0
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'dim count = _context.Carriers.Count();
                'Get the newest record that matches the provided criteria
                intRet = (
                    From d In db.Carriers
                        Where
                            (d.CarrierActive = If(CarrierActiveFlag = 0, d.CarrierActive, If(CarrierActiveFlag = 1, True, False))) _
                            And
                            (String.IsNullOrWhiteSpace(CarrierLegalEntity) OrElse d.CarrierLegalEntity = CarrierLegalEntity)
                        Select d.CarrierControl).Count()


            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                ManageLinqDataExceptions(ex, buildProcedureName("CountRecords"))
            End Try

        End Using
        Return intRet
    End Function


#Region "TMS 365"

#Region "DEPRECIATED"

    ''' <summary>
    ''' DEPRECIATED by LVV on 6/14/18 for v-8.2 VSTS #337
    ''' Replaced by GetLegalEntityCarriersByLE()
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filters"></param>
    ''' <returns></returns>
    Public Function GetCarrierDispatchSettingsByLE(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As Boolean
        throwDepreciatedException(buildProcedureName("GetCarrierDispatchSettingsByLE"))
        Return False
    End Function

    ''' <summary>
    ''' DEPRECIATED by LVV on 7/13/18 for v-8.2
    ''' Replaced by InsertOrUpdateLegalEntityCarrier()
    ''' </summary>
    ''' <param name="LEAdminControl"></param>
    ''' <param name="CarrierControl"></param>
    ''' <param name="DispatchTypeControl"></param>
    ''' <param name="RateShopOnly"></param>
    ''' <param name="APIDispatching"></param>
    ''' <param name="APIStatusUpdates"></param>
    ''' <param name="ShowAuditFailReason"></param>
    ''' <param name="ShowPendingFeeFailReason"></param>
    ''' <param name="BillToCompControl"></param>
    ''' <param name="CarrierAccountRef"></param>
    ''' <returns></returns>
    Public Function InsertOrUpdateCarrierDispatchSetting(ByVal LEAdminControl As Integer,
                                                         ByVal CarrierControl As Integer,
                                                         ByVal DispatchTypeControl As Integer,
                                                         ByVal RateShopOnly As Boolean,
                                                         ByVal APIDispatching As Boolean,
                                                         ByVal APIStatusUpdates As Boolean,
                                                         ByVal ShowAuditFailReason As Boolean,
                                                         ByVal ShowPendingFeeFailReason As Boolean,
                                                         ByVal BillToCompControl As Integer,
                                                         ByVal CarrierAccountRef As String) As LTS.spInsertOrUpdateCarrierDispatchSettingResult
        throwDepreciatedException(buildProcedureName("InsertOrUpdateCarrierDispatchSetting"))
        Return Nothing
    End Function

    ''' <summary>
    ''' DEPRECIATED by LVV on 6/14/18 for v-8.2 VSTS #337
    ''' Replaced by DeleteLegalEntityCarrier()
    ''' </summary>
    ''' <param name="Control"></param>
    ''' <returns></returns>
    Public Function DeleteCarrierDispatchSetting(ByVal Control As Integer) As Boolean
        throwDepreciatedException(buildProcedureName("DeleteCarrierDispatchSetting"))
        Return False
    End Function

    ''' <summary>
    ''' DEPRECIATED by LVV on 6/18/18 for v-8.2 VSTS #337
    ''' Replaced by GetLECarrierAccessorial()
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="LEControl"></param>
    ''' <param name="CarrierControl"></param>
    ''' <param name="filterWhere"></param>
    ''' <param name="sortExpression"></param>
    ''' <param name="page"></param>
    ''' <param name="pagesize"></param>
    ''' <param name="skip"></param>
    ''' <param name="take"></param>
    ''' <returns></returns>
    Public Function GetCarrierLegalAccessorialXref(ByRef RecordCount As Integer,
                                                   ByVal LEControl As Integer,
                                                   ByVal CarrierControl As Integer,
                                                   Optional ByVal filterWhere As String = "",
                                                   Optional ByVal sortExpression As String = "CLAXControl Desc",
                                                   Optional ByVal page As Integer = 1,
                                                   Optional ByVal pagesize As Integer = 1000,
                                                   Optional ByVal skip As Integer = 0,
                                                   Optional ByVal take As Integer = 0) As Boolean

        throwDepreciatedException(buildProcedureName("GetCarrierLegalAccessorialXref"))
        Return False
    End Function

    ''' <summary>
    ''' DEPRECIATED by LVV on 6/18/18 for v-8.2 VSTS #337
    ''' Replaced by GetLECAForSettlement()
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="CompLE"></param>
    ''' <param name="CarrierControl"></param>
    ''' <param name="filterWhere"></param>
    ''' <param name="sortExpression"></param>
    ''' <param name="page"></param>
    ''' <param name="pagesize"></param>
    ''' <param name="skip"></param>
    ''' <param name="take"></param>
    ''' <returns></returns>
    Public Function GetCLAXForSettlement(ByRef RecordCount As Integer,
                                         ByVal CompLE As String,
                                         ByVal CarrierControl As Integer,
                                         Optional ByVal filterWhere As String = "",
                                         Optional ByVal sortExpression As String = "CLAXControl Desc",
                                         Optional ByVal page As Integer = 1,
                                         Optional ByVal pagesize As Integer = 1000,
                                         Optional ByVal skip As Integer = 0,
                                         Optional ByVal take As Integer = 0) As Boolean

        throwDepreciatedException(buildProcedureName("GetCLAXForSettlement"))
        Return False
    End Function

    ''' <summary>
    ''' DEPRECIATED by LVV on 6/18/18 for v-8.2 VSTS #337
    ''' Replaced by GetLECarrierAccessorial()
    ''' </summary>
    ''' <param name="Control"></param>
    ''' <returns></returns>
    Public Function DeleteCarrierLegalAccessorialXref(Control As Integer) As Boolean
        throwDepreciatedException(buildProcedureName("DeleteCarrierLegalAccessorialXref"))
        Return False
    End Function

    ''' <summary>
    ''' DEPRECIATED by LVV on 6/18/18 for v-8.2 VSTS #337
    ''' Replaced by InsertOrUpdateLECarrierAccessorial()
    ''' </summary>
    ''' <param name="LEAdminControl"></param>
    ''' <param name="CarrierControl"></param>
    ''' <param name="AccessorialCode"></param>
    ''' <param name="AutoApprove"></param>
    ''' <param name="AllowCarrierUpdates"></param>
    ''' <param name="AccessorialVisible"></param>
    ''' <param name="Caption"></param>
    ''' <param name="EDICode"></param>
    ''' <param name="ApproveToleranceLow"></param>
    ''' <param name="ApproveToleranceHigh"></param>
    ''' <param name="ApproveTolerancePerLow"></param>
    ''' <param name="ApproveTolerancePerHigh"></param>
    ''' <param name="AverageValue"></param>
    ''' <param name="DynamicAverageValue"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' DEPRECIATED By LVV on 6/18/18 for v-8.2 VSTS #337
    ''' Replaced By InsertOrUpdateLECarrierAccessorial
    ''' </remarks>
    Public Function InsertOrUpdateCarrierLegalAccessorialXref(ByVal LEAdminControl As Integer,
                                                              ByVal CarrierControl As Integer,
                                                              ByVal AccessorialCode As Integer,
                                                              ByVal AutoApprove As Boolean,
                                                              ByVal AllowCarrierUpdates As Boolean,
                                                              ByVal AccessorialVisible As Boolean,
                                                              ByVal Caption As String,
                                                              ByVal EDICode As String,
                                                              ByVal ApproveToleranceLow As Double,
                                                              ByVal ApproveToleranceHigh As Double,
                                                              ByVal ApproveTolerancePerLow As Double,
                                                              ByVal ApproveTolerancePerHigh As Double,
                                                              ByVal AverageValue As Decimal,
                                                              ByVal DynamicAverageValue As Boolean) As Boolean
        throwDepreciatedException(buildProcedureName("InsertOrUpdateCarrierLegalAccessorialXref"))
        Return False
    End Function

#End Region

    Public Function GetCarrierLTS(ByVal CarrierControl As Integer) As LTS.Carrier
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Return db.Carriers.Where(Function(x) x.CarrierControl = CarrierControl).FirstOrDefault()
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                ManageLinqDataExceptions(ex, buildProcedureName("GetCarrierLTS"))
            End Try
            Return Nothing
        End Using
    End Function

    ''' <summary>
    ''' Returns the count of pickup and delivery orders needed to schedule appointment
    ''' </summary>
    ''' <param name="CarrierControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCarrierOrderSummaryForChart(ByVal CarrierControl As Integer, ByVal OrderSummaryDays As Integer) As LTS.SPGetCarrierOrderSummaryForChartResult()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Return (From d In db.SPGetCarrierOrderSummaryForChart(CarrierControl, OrderSummaryDays) Select d).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCarrierOrderSummaryForChart"))
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Returns the count of pickup and delivery orders needed to schedule appointment (Grouped by SHID)
    ''' </summary>
    ''' <param name="CarrierControl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCarrierOrderSummaryForChartGrouped(ByVal CarrierControl As Integer, ByVal OrderSummaryDays As Integer) As LTS.spGetCarrierOrderSummaryForChartGroupedResult()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Return (From d In db.spGetCarrierOrderSummaryForChartGrouped(CarrierControl, OrderSummaryDays) Select d).ToArray()
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetCarrierOrderSummaryForChartGrouped"))
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Get a list of all the Carriers in the Carrier table (with optional filtering)
    ''' </summary>
    ''' <param name="RecordCount"></param>
    ''' <param name="filters"></param>
    ''' <returns></returns>
    ''' <remarks>Added By LVV on 10/19/20 for v-8.3.0.001 Task #Task #20201020161708 - Add Master Carrier Page</remarks>
    Public Function GetMasterCarriers365(ByRef RecordCount As Integer, ByVal filters As Models.AllFilters) As LTS.Carrier()
        If filters Is Nothing Then Return Nothing
        Dim oRet() As LTS.Carrier
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim iQuery As IQueryable(Of LTS.Carrier)
                iQuery = db.Carriers
                Dim filterWhere = ""
                ApplyAllFilters(iQuery, filters, filterWhere)
                PrepareQuery(iQuery, filters, RecordCount)
                oRet = iQuery.Skip(filters.skip).Take(filters.take).ToArray()
                Return oRet
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("GetMasterCarriers365"), db)
            End Try
        End Using
        Return Nothing
    End Function

    ''' <summary>
    ''' Does the same logic as the dektop client but is called by Master Carriers page in 365
    ''' </summary>
    ''' <param name="oData"></param>
    ''' <returns></returns>
    ''' <remarks>Added By LVV on 10/19/20 for v-8.3.0.001 Task #Task #20201020161708 - Add Master Carrier Page</remarks>
    Public Function InsertOrUpdateCarrier365(ByVal oData As LTS.Carrier) As Models.ResultObject
        Dim results As New Models.ResultObject
        results.Success = False
        If oData Is Nothing Then
            results.updateResultMessage(Models.ResultObject.ResultMsgType.Err, "Data cannot be null")
            Return results 'nothing to do
        End If
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim strValidationMsg As String = ""
                If oData.CarrierControl = 0 Then
                    Dim carrierNo As Integer = 0
                    If oData.CarrierNumber <> 0 Then carrierNo = oData.CarrierNumber
                    If ValidateCarrierBeforeInsert(carrierNo, oData.CarrierName, oData.CarrierLegalEntity, oData.CarrierAlphaCode, strValidationMsg) Then
                        oData.CarrierNumber = carrierNo
                        Dim intCodeVal1 As Integer = 0
                        Dim intCodeVal2 As Integer = 0
                        If Not getNewCarrierCodeValues(intCodeVal1, intCodeVal2) Then
                            Utilities.SaveAppError("Cannot save new Carrier data: unable to get new Carrier Code Values.", Me.Parameters)
                            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_GetCarrierCodeValFailure"}, New FaultReason("E_DataValidationFailure"))
                        End If
                        oData.CarrierCodeVal1 = intCodeVal1
                        oData.CarrierCodeVal2 = intCodeVal2
                        oData.CarrierModUser = Parameters.UserName
                        oData.CarrierModDate = Date.Now
                        db.Carriers.InsertOnSubmit(oData)
                    Else
                        results.Success = False
                        results.updateResultMessage(Models.ResultObject.ResultMsgType.Err, strValidationMsg)
                        Return results
                    End If
                Else
                    If ValidateCarrierBeforeUpdate(oData.CarrierControl, oData.CarrierNumber, oData.CarrierName, oData.CarrierLegalEntity, oData.CarrierAlphaCode, strValidationMsg) Then
                        oData.CarrierModUser = Parameters.UserName
                        oData.CarrierModDate = Date.Now
                        db.Carriers.Attach(oData, True)
                    Else
                        results.Success = False
                        results.updateResultMessage(Models.ResultObject.ResultMsgType.Err, strValidationMsg)
                        Return results
                    End If
                End If
                db.SubmitChanges()
                results.Success = True
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("InsertOrUpdateCarrier365"), db)
            End Try
        End Using
        Return results
    End Function

    ''' <summary>
    ''' Sends an email blast to the carriers assigned to this legal entity with information about the posting on NEXTrack
    ''' </summary>
    ''' <param name="BookControl"></param>
    ''' <remarks>
    ''' created by RHR for v-8.4.0.002 on 04/15/2021
    ''' </remarks>
    Public Sub SendNEXTStopCarrierEmails(ByVal BookControl As Integer)

        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try

                Dim oRet = db.spGenerateNEXTStopPostingEmails(BookControl, Nothing, Nothing)
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("SendNEXTStopCarrierEmails"))
            End Try

        End Using
    End Sub



#End Region

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim oCarrier = CType(oData, DataTransferObjects.Carrier)
        'Create New Record
        Return New LTS.Carrier With {.CarrierControl = oCarrier.CarrierControl _
            , .CarrierNumber = oCarrier.CarrierNumber _
            , .CarrierName = oCarrier.CarrierName _
            , .CarrierStreetAddress1 = oCarrier.CarrierStreetAddress1 _
            , .CarrierStreetAddress2 = oCarrier.CarrierStreetAddress2 _
            , .CarrierStreetAddress3 = oCarrier.CarrierStreetAddress3 _
            , .CarrierStreetCity = oCarrier.CarrierStreetCity _
            , .CarrierStreetState = oCarrier.CarrierStreetState _
            , .CarrierStreetCountry = oCarrier.CarrierStreetCountry _
            , .CarrierStreetZip = oCarrier.CarrierStreetZip _
            , .CarrierMailAddress1 = oCarrier.CarrierMailAddress1 _
            , .CarrierMailAddress2 = oCarrier.CarrierMailAddress2 _
            , .CarrierMailAddress3 = oCarrier.CarrierMailAddress3 _
            , .CarrierMailCity = oCarrier.CarrierMailCity _
            , .CarrierMailState = oCarrier.CarrierMailState _
            , .CarrierMailCountry = oCarrier.CarrierMailCountry _
            , .CarrierMailZip = oCarrier.CarrierMailZip _
            , .CarrierModDate = Date.Now _
            , .CarrierModUser = Parameters.UserName _
            , .CarrierSCAC = oCarrier.CarrierSCAC _
            , .CarrierAccountNo = oCarrier.CarrierAccountNo _
            , .CarrierTypeCode = oCarrier.CarrierTypeCode _
            , .CarrierGeneralInfo = oCarrier.CarrierGeneralInfo _
            , .CarrierActive = oCarrier.CarrierActive _
            , .CarrierWebSite = oCarrier.CarrierWebSite _
            , .CarrierEmail = oCarrier.CarrierEmail _
            , .CarrierNEXTStopAcct = oCarrier.CarrierNEXTStopAcct _
            , .CarrierNEXTStopPwd = oCarrier.CarrierNEXTStopPwd _
            , .CarrierGradReliability = oCarrier.CarrierGradReliability _
            , .CarrierGradBillingAccuracy = oCarrier.CarrierGradBillingAccuracy _
            , .CarrierGradFinancialStrength = oCarrier.CarrierGradFinancialStrength _
            , .CarrierGradEquipmentCondition = oCarrier.CarrierGradEquipmentCondition _
            , .CarrierGradContactAttitude = oCarrier.CarrierGradContactAttitude _
            , .CarrierGradDriverAttitude = oCarrier.CarrierGradDriverAttitude _
            , .CarrierGradClaimFrequency = oCarrier.CarrierGradClaimFrequency _
            , .CarrierGradClaimPayment = oCarrier.CarrierGradClaimPayment _
            , .CarrierGradGeographicCoverage = oCarrier.CarrierGradGeographicCoverage _
            , .CarrierGradCustomerService = oCarrier.CarrierGradCustomerService _
            , .CarrierGradPriceChangeNotification = oCarrier.CarrierGradPriceChangeNotification _
            , .CarrierGradPriceChangeFrequency = oCarrier.CarrierGradPriceChangeFrequency _
            , .CarrierGradPriceAggressiveness = oCarrier.CarrierGradPriceAggressiveness _
            , .CarrierGradAverage = oCarrier.CarrierGradAverage _
            , .CarrierQualInsuranceDate = oCarrier.CarrierQualInsuranceDate _
            , .CarrierQualQualified = oCarrier.CarrierQualQualified _
            , .CarrierQualAuthority = oCarrier.CarrierQualAuthority _
            , .CarrierQualContract = oCarrier.CarrierQualContract _
            , .CarrierQualSignedDate = oCarrier.CarrierQualSignedDate _
            , .CarrierQualContractExpiresDate = oCarrier.CarrierQualContractExpiresDate _
            , .CarrierQualMaxPerShipment = oCarrier.CarrierQualMaxPerShipment _
            , .CarrierQualMaxAllShipments = oCarrier.CarrierQualMaxAllShipments _
            , .CarrierQualCurAllExposure = oCarrier.CarrierQualCurAllExposure _
            , .CarrierCodeVal1 = oCarrier.CarrierCodeVal1 _
            , .CarrierCodeVal2 = oCarrier.CarrierCodeVal2 _
            , .CarrierTruckDefault = oCarrier.CarrierTruckDefault _
            , .CarrierAllowWebTender = oCarrier.CarrierAllowWebTender _
            , .CarrierIgnoreTariff = oCarrier.CarrierIgnoreTariff _
            , .CarrierSmartWayPartnerType = oCarrier.CarrierSmartWayPartnerType _
            , .CarrierSmartWayScore = oCarrier.CarrierSmartWayScore _
            , .CarrierSmartWayPartner = oCarrier.CarrierSmartWayPartner _
            , .CarrierAutoFinalize = oCarrier.CarrierAutoFinalize _
            , .CarrierUseStdFuelAddendum = oCarrier.CarrierUseStdFuelAddendum _
            , .CarrierAutoAssignProNumber = oCarrier.CarrierAutoAssignProNumber _
            , .CarrierUpdated = If(oCarrier.CarrierUpdated Is Nothing, New Byte() {}, oCarrier.CarrierUpdated) _
            , .CarrierLegalEntity = oCarrier.CarrierLegalEntity _
            , .CarrierAlphaCode = oCarrier.CarrierAlphaCode _
            , .CarrierUser1 = oCarrier.CarrierUser1 _
            , .CarrierUser2 = oCarrier.CarrierUser2 _
            , .CarrierUser3 = oCarrier.CarrierUser3 _
            , .CarrierUser4 = oCarrier.CarrierUser4 _
            , .CarrierQualUSDot = oCarrier.CarrierQualUSDot _
            , .CarrierQualCSAScore = oCarrier.CarrierQualCSAScore _
            , .CarrierCalcOnTimeServiceLevel = oCarrier.CarrierCalcOnTimeServiceLevel _
            , .CarrierAssignedOnTimeServiceLevel = oCarrier.CarrierAssignedOnTimeServiceLevel _
            , .CarrierCalcOnTimeNoMonthsUsed = oCarrier.CarrierCalcOnTimeNoMonthsUsed}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetCarrierFiltered(Control:=CType(LinqTable, LTS.Carrier).CarrierControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DataTransferObjects.QuickSaveResults
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim source As LTS.Carrier = TryCast(LinqTable, LTS.Carrier)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.Carriers
                    Where d.CarrierControl = source.CarrierControl
                    Select New DataTransferObjects.QuickSaveResults With {.Control = d.CarrierControl _
                        , .ModDate = d.CarrierModDate _
                        , .ModUser = d.CarrierModUser _
                        , .Updated = d.CarrierUpdated.ToArray}).First

            Catch ex As System.Data.SqlClient.SqlException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_SQLException"))
            Catch ex As InvalidOperationException
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_NoData"}, New FaultReason("E_InvalidOperationException"))
            Catch ex As Exception
                Utilities.SaveAppError(ex.Message, Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
            End Try

        End Using
        Return ret
    End Function

    'Protected Overrides Sub ValidateNewRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
    '    'Check if the data already exists only one allowed
    '    With CType(oData, DTO.Carrier)
    '        Try
    '            'check for required fields
    '            If .CarrierNumber = 0 Then
    '                Utilities.SaveAppError("Cannot save new Carrier data.  The Carrier number, " & .CarrierNumber & " cannot be zero", Me.Parameters)
    '                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
    '            End If

    '            If String.IsNullOrEmpty(.CarrierName) OrElse .CarrierName.Trim.Length < 1 Then
    '                Utilities.SaveAppError("Cannot save new Carrier data.  The Carrier name, " & .CarrierName & " is not valid.", Me.Parameters)
    '                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
    '            End If

    '            Dim intCodeVal1 As Integer = 0
    '            Dim intCodeVal2 As Integer = 0
    '            If Not getNewCarrierCodeValues(intCodeVal1, intCodeVal2) Then
    '                Utilities.SaveAppError("Cannot save new Carrier data: unable to get new Carrier Code Values.", Me.Parameters)
    '                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_GetCarrierCodeValFailure"}, New FaultReason("E_DataValidationFailure"))
    '            End If
    '            .CarrierCodeVal1 = intCodeVal1
    '            .CarrierCodeVal2 = intCodeVal2
    '            Dim Carrier As DTO.Carrier = ( _
    '                From t In CType(oDB, NGLMASCarrierDataContext).Carriers _
    '                 Where _
    '                     (t.CarrierName = .CarrierName _
    '                     Or _
    '                     t.CarrierNumber = .CarrierNumber) _
    '                 Select New DTO.Carrier With {.CarrierControl = t.CarrierControl}).First

    '            If Not Carrier Is Nothing Then
    '                Utilities.SaveAppError("Cannot save new Carrier data.  The Carrier number, " & .CarrierNumber & " or the Carrier name, " & .CarrierName & ",  already exist.", Me.Parameters)
    '                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
    '            End If

    '        Catch ex As FaultException
    '            Throw
    '        Catch ex As InvalidOperationException
    '            'do nothing this is the desired result.
    '        End Try
    '    End With
    'End Sub

    'Protected Overrides Sub ValidateUpdatedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DTO.DTOBaseClass)
    '    'Check if the data already exists only one allowed
    '    With CType(oData, DTO.Carrier)
    '        Try
    '            'check for required fields
    '            If .CarrierNumber = 0 Then
    '                Utilities.SaveAppError("Cannot save Carrier changes.  The Carrier number, " & .CarrierNumber & " cannot be zero", Me.Parameters)
    '                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
    '            End If

    '            If String.IsNullOrEmpty(.CarrierName) OrElse .CarrierName.Trim.Length < 1 Then
    '                Utilities.SaveAppError("Cannot save Carrier changes.  The Carrier name, " & .CarrierName & " is not valid.", Me.Parameters)
    '                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
    '            End If


    '            'Get the newest record that matches the provided criteria
    '            Dim Carrier As DTO.Carrier = ( _
    '            From t In CType(oDB, NGLMASCarrierDataContext).Carriers _
    '             Where _
    '                 (t.CarrierControl <> .CarrierControl) _
    '                 And _
    '                 (t.CarrierNumber = .CarrierNumber) _
    '             Select New DTO.Carrier With {.CarrierControl = t.CarrierControl}).First

    '            If Not Carrier Is Nothing Then
    '                Utilities.SaveAppError("Cannot save Carrier changes.  The Carrier number, " & .CarrierNumber & ",  already exist.", Me.Parameters)
    '                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
    '            End If

    '        Catch ex As FaultException
    '            Throw
    '        Catch ex As InvalidOperationException
    '            'do nothing this is the desired result.
    '        End Try
    '    End With
    'End Sub

    Protected Overrides Sub ValidateDeletedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        'Check if the carrier is being used by the book data or the lane data
        With CType(oData, DataTransferObjects.Carrier)
            Try
                'Add code here to call the Book and Lane data providers when they are created
                Dim dpBook As New NGLBookData(Me.Parameters)
                Dim dpLane As New NGLLaneData(Me.Parameters)
                Dim oBooks() As DataTransferObjects.Book
                Dim oLanes() As DataTransferObjects.Lane
                Try
                    oBooks = dpBook.GetBooksByCarrier(.CarrierControl)
                Catch ex As FaultException
                    If ex.Message <> "E_NoData" Then
                        Throw
                    End If
                End Try
                Try
                    oLanes = dpLane.GetLanesByCarrier(.CarrierControl)
                Catch ex As FaultException
                    If ex.Message <> "E_NoData" Then
                        Throw
                    End If
                End Try
                If (Not oBooks Is Nothing AndAlso oBooks.Length > 0) OrElse (Not oLanes Is Nothing AndAlso oLanes.Length > 0) Then
                    Utilities.SaveAppError("Cannot delete Carrier data.  The Carrier number, " & .CarrierNumber & " is being used and cannot be deleted. check the book or lane information.", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DataInUse"}, New FaultReason("E_DataValidationFailure"))
                End If
            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub

    'Modified By LVV on 10/14/16 for v-7.0.5.110 Carrier Contact Changes
    Protected Overrides Sub AddDetailsToLinq(ByRef LinqTable As Object, ByRef oData As DataTransferObjects.DTOBaseClass)

        With CType(LinqTable, LTS.Carrier)
            'Add carrier contact Records
            .CarrierConts.AddRange(
                From d In CType(oData, DataTransferObjects.Carrier).CarrierConts
                                      Select New LTS.CarrierCont With {.CarrierContControl = d.CarrierContControl,
                                      .CarrierContCarrierControl = d.CarrierContCarrierControl,
                                      .CarrierContName = d.CarrierContName,
                                      .CarrierContTitle = d.CarrierContTitle,
                                      .CarrierContactPhone = d.CarrierContactPhone,
                                      .CarrierContPhoneExt = d.CarrierContPhoneExt,
                                      .CarrierContactFax = d.CarrierContactFax,
                                      .CarrierContact800 = d.CarrierContact800,
                                      .CarrierContactEMail = d.CarrierContactEMail,
                                      .CarrierContactDefault = d.CarrierContactDefault,
                                      .CarrierContUpdated = If(d.CarrierContUpdated Is Nothing, New Byte() {}, d.CarrierContUpdated)})
            'Add carrier budget Records
            .CarrierBudgets.AddRange(
                From d In CType(oData, DataTransferObjects.Carrier).CarrierBudgets
                                        Select New LTS.CarrierBudget With {.CarrierBudControl = d.CarrierBudControl,
                                        .CarrierBudCarrierControl = d.CarrierBudCarrierControl,
                                        .CarrierBudModDate = Date.Now,
                                        .CarrierBudModUser = Parameters.UserName,
                                        .CarrierBudExpMo1 = d.CarrierBudExpMo1,
                                        .CarrierBudExpMo2 = d.CarrierBudExpMo2,
                                        .CarrierBudExpMo3 = d.CarrierBudExpMo3,
                                        .CarrierBudExpMo4 = d.CarrierBudExpMo4,
                                        .CarrierBudExpMo5 = d.CarrierBudExpMo5,
                                        .CarrierBudExpMo6 = d.CarrierBudExpMo6,
                                        .CarrierBudExpMo7 = d.CarrierBudExpMo7,
                                        .CarrierBudExpMo8 = d.CarrierBudExpMo8,
                                        .CarrierBudExpMo9 = d.CarrierBudExpMo9,
                                        .CarrierBudExpMo10 = d.CarrierBudExpMo10,
                                        .CarrierBudExpMo11 = d.CarrierBudExpMo11,
                                        .CarrierBudExpMo12 = d.CarrierBudExpMo12,
                                        .CarrierBudExpTotal = d.CarrierBudExpTotal,
                                        .CarrierBudActMo1 = d.CarrierBudActMo1,
                                        .CarrierBudActMo2 = d.CarrierBudActMo2,
                                        .CarrierBudActMo3 = d.CarrierBudActMo3,
                                        .CarrierBudActMo4 = d.CarrierBudActMo4,
                                        .CarrierBudActMo5 = d.CarrierBudActMo5,
                                        .CarrierBudActMo6 = d.CarrierBudActMo6,
                                        .CarrierBudActMo7 = d.CarrierBudActMo7,
                                        .CarrierBudActMo8 = d.CarrierBudActMo8,
                                        .CarrierBudActMo9 = d.CarrierBudActMo9,
                                        .CarrierBudActMo10 = d.CarrierBudActMo10,
                                        .CarrierBudActMo11 = d.CarrierBudActMo11,
                                        .CarrierBudActMo12 = d.CarrierBudActMo12,
                                        .CarrierBudActTotal = d.CarrierBudActTotal,
                                        .CarrierBudgetUpdated = If(d.CarrierBudgetUpdated Is Nothing, New Byte() {}, d.CarrierBudgetUpdated)})
            ''Add carrier edi Records
            '.CarrierEDIs.AddRange( _
            '         From d In CType(oData, DTO.Carrier).CarrierEDIs _
            '         Select New LTS.CarrierEDI With {.CarrierEDIControl = d.CarrierEDIControl, _
            '                                .CarrierEDICarrierControl = d.CarrierEDICarrierControl, _
            '                                .CarrierEDIXaction = d.CarrierEDIXaction, _
            '                                .CarrierEDIComment = d.CarrierEDIComment, _
            '                                .CarrierEDISecurityQual = d.CarrierEDISecurityQual, _
            '                                .CarrierEDISecurityCode = d.CarrierEDISecurityCode, _
            '                                .CarrierEDIPartnerQual = d.CarrierEDIPartnerQual, _
            '                                .CarrierEDIPartnerCode = d.CarrierEDIPartnerCode, _
            '                                .CarrierEDIISASequence = d.CarrierEDIISASequence, _
            '                                .CarrierEDIGSSequence = d.CarrierEDIGSSequence, _
            '                                .CarrierEDIEmailNotificationOn = d.CarrierEDIEmailNotificationOn, _
            '                                .CarrierEDIEmailAddress = d.CarrierEDIEmailAddress, _
            '                                .CarrierEDIAcknowledgementRequested = d.CarrierEDIAcknowledgementRequested, _
            '                                .CarrierEDIAcceptOn997 = d.CarrierEDIAcceptOn997, _
            '                                .CarrierEDITestCode = d.CarrierEDITestCode, _
            '                                .CarrierEDIUpdated = If(d.CarrierEDIUpdated Is Nothing, New Byte() {}, d.CarrierEDIUpdated), _
            '                                .CarrierEDIInboundFolder = d.CarrierEDIInboundFolder, _
            '                                .CarrierEDIBackupFolder = d.CarrierEDIBackupFolder, _
            '                                .CarrierEDILogFile = d.CarrierEDILogFile, _
            '                                .CarrierEDIStartTime = d.CarrierEDIStartTime, _
            '                                .CarrierEDIEndTime = d.CarrierEDIEndTime, _
            '                                .CarrierEDIDaysOfWeek = d.CarrierEDIDaysOfWeek, _
            '                                .CarrierEDISendMinutesOutbound = d.CarrierEDISendMinutesOutbound, _
            '                                .CarrierEDIFileNameBaseOutbound = d.CarrierEDIFileNameBaseOutbound, _
            '                                .CarrierEDIFileNameBaseInbound = d.CarrierEDIFileNameBaseInbound, _
            '                                .CarrierEDIWebServiceAuthKey = d.CarrierEDIWebServiceAuthKey, _
            '                                .CarrierEDINGLEDIInputWebURL = d.CarrierEDINGLEDIInputWebURL, _
            '                                .CarrierEDINGLEDI204OutputWebURL = d.CarrierEDINGLEDI204OutputWebURL, _
            '                                .CarrierEDIOutboundFolder = d.CarrierEDIOutboundFolder, _
            '                                .CarrierEDILastOutboundTransmission = d.CarrierEDILastOutboundTransmission, _
            '                                .CarrierEDIFTPOutboundFolder = d.CarrierEDIFTPOutboundFolder, _
            '                                .CarrierEDIFTPBackupFolder = d.CarrierEDIFTPBackupFolder, _
            '                                .CarrierEDIFTPInboundFolder = d.CarrierEDIFTPInboundFolder, _
            '                                .CarrierEDIFTPServer = d.CarrierEDIFTPServer, _
            '                                .CarrierEDIFTPUserName = d.CarrierEDIFTPUserName, _
            '                                .CarrierEDIFTPPassword = d.CarrierEDIFTPPassword, _
            '                                .CarrierEDIWebServiceURL = d.CarrierEDIWebServiceURL})
            ''Add carrier truck Records
            '.CarrierTrucks.AddRange( _
            '         From d In CType(oData, DTO.Carrier).CarrierTrucks _
            '         Select New LTS.CarrierTruck With {.CarrierTruckControl = d.CarrierTruckControl _
            '                                  , .CarrierTruckCarrierControl = d.CarrierTruckCarrierControl _
            '                                  , .CarrierTruckDescription = d.CarrierTruckDescription _
            '                                  , .CarrierTruckWgtFrom = d.CarrierTruckWgtFrom _
            '                                  , .CarrierTruckWgtTo = d.CarrierTruckWgtTo _
            '                                  , .CarrierTruckRateStarts = d.CarrierTruckRateStarts _
            '                                  , .CarrierTruckRateExpires = d.CarrierTruckRateExpires _
            '                                  , .CarrierTruckTL = d.CarrierTruckTL _
            '                                  , .CarrierTruckLTL = d.CarrierTruckLTL _
            '                                  , .CarrierTruckEquipment = d.CarrierTruckEquipment _
            '                                  , .CarrierTruckMileRate = d.CarrierTruckMileRate _
            '                                  , .CarrierTruckCwtRate = d.CarrierTruckCwtRate _
            '                                  , .CarrierTruckCaseRate = d.CarrierTruckCaseRate _
            '                                  , .CarrierTruckFlatRate = d.CarrierTruckFlatRate _
            '                                  , .CarrierTruckPltRate = d.CarrierTruckPltRate _
            '                                  , .CarrierTruckCubeRate = d.CarrierTruckCubeRate _
            '                                  , .CarrierTruckTLT = d.CarrierTruckTLT _
            '                                  , .CarrierTruckTMode = d.CarrierTruckTMode _
            '                                  , .CarrierTruckFAK = d.CarrierTruckFAK _
            '                                  , .CarrierTruckDisc = d.CarrierTruckDisc _
            '                                  , .CarrierTruckPUMon = d.CarrierTruckPUMon _
            '                                  , .CarrierTruckPUTue = d.CarrierTruckPUTue _
            '                                  , .CarrierTruckPUWed = d.CarrierTruckPUWed _
            '                                  , .CarrierTruckPUThu = d.CarrierTruckPUThu _
            '                                  , .CarrierTruckPUFri = d.CarrierTruckPUFri _
            '                                  , .CarrierTruckPUSat = d.CarrierTruckPUSat _
            '                                  , .CarrierTruckPUSun = d.CarrierTruckPUSun _
            '                                  , .CarrierTruckDLMon = d.CarrierTruckDLMon _
            '                                  , .CarrierTruckDLTue = d.CarrierTruckDLTue _
            '                                  , .CarrierTruckDLWed = d.CarrierTruckDLWed _
            '                                  , .CarrierTruckDLThu = d.CarrierTruckDLThu _
            '                                  , .CarrierTruckDLFri = d.CarrierTruckDLFri _
            '                                  , .CarrierTruckDLSat = d.CarrierTruckDLSat _
            '                                  , .CarrierTruckDLSun = d.CarrierTruckDLSun _
            '                                  , .CarrierTruckPayTolPerLo = d.CarrierTruckPayTolPerLo _
            '                                  , .CarrierTruckPayTolPerHi = d.CarrierTruckPayTolPerHi _
            '                                  , .CarrierTruckPayTolCurLo = d.CarrierTruckPayTolCurLo _
            '                                  , .CarrierTruckPayTolCurHi = d.CarrierTruckPayTolCurHi _
            '                                  , .CarrierTruckCurType = d.CarrierTruckCurType _
            '                                  , .CarrierTruckModUser = Parameters.UserName _
            '                                  , .CarrierTruckModDate = Date.Now _
            '                                  , .CarrierTruckRoute = d.CarrierTruckRoute _
            '                                  , .CarrierTruckMiles = d.CarrierTruckMiles _
            '                                  , .CarrierTruckBkhlCostPerc = d.CarrierTruckBkhlCostPerc _
            '                                  , .CarrierTruckPalletCostPer = d.CarrierTruckPalletCostPer _
            '                                  , .CarrierTruckFuelSurChargePerc = d.CarrierTruckFuelSurChargePerc _
            '                                  , .CarrierTruckStopCharge = d.CarrierTruckStopCharge _
            '                                  , .CarrierTruckDropCost = d.CarrierTruckDropCost _
            '                                  , .CarrierTruckUnloadDiff = d.CarrierTruckUnloadDiff _
            '                                  , .CarrierTruckCasesAvailable = d.CarrierTruckCasesAvailable _
            '                                  , .CarrierTruckCasesOpen = d.CarrierTruckCasesOpen _
            '                                  , .CarrierTruckCasesCommitted = d.CarrierTruckCasesCommitted _
            '                                  , .CarrierTruckWgtAvailable = d.CarrierTruckWgtAvailable _
            '                                  , .CarrierTruckWgtOpen = d.CarrierTruckWgtOpen _
            '                                  , .CarrierTruckWgtCommitted = d.CarrierTruckWgtCommitted _
            '                                  , .CarrierTruckCubesAvailable = d.CarrierTruckCubesAvailable _
            '                                  , .CarrierTruckCubesOpen = d.CarrierTruckCubesOpen _
            '                                  , .CarrierTruckCubesCommitted = d.CarrierTruckCubesCommitted _
            '                                  , .CarrierTruckPltsAvailable = d.CarrierTruckPltsAvailable _
            '                                  , .CarrierTruckPltsOpen = d.CarrierTruckPltsOpen _
            '                                  , .CarrierTruckPltsCommitted = d.CarrierTruckPltsCommitted _
            '                                  , .CarrierTruckTrucksAvailable = d.CarrierTruckTrucksAvailable _
            '                                  , .CarrierTruckMaxLoadsByWeek = d.CarrierTruckMaxLoadsByWeek _
            '                                  , .CarrierTruckMaxLoadsByMonth = d.CarrierTruckMaxLoadsByMonth _
            '                                  , .CarrierTruckTotalLoadsForWeek = d.CarrierTruckTotalLoadsForWeek _
            '                                  , .CarrierTruckTotalLoadsForMonth = d.CarrierTruckTotalLoadsForMonth _
            '                                  , .CarrierTruckWeekDate = d.CarrierTruckWeekDate _
            '                                  , .CarrierTruckMonthDate = d.CarrierTruckMonthDate _
            '                                  , .CarrierTruckUpdated = If(d.CarrierTruckUpdated Is Nothing, New Byte() {}, d.CarrierTruckUpdated)})

        End With
    End Sub

    Protected Overrides Sub InsertAllDetails(ByRef oDB As System.Data.Linq.DataContext, ByRef LinqTable As Object)
        With CType(oDB, NGLMASCarrierDataContext)
            .CarrierConts.InsertAllOnSubmit(CType(LinqTable, LTS.Carrier).CarrierConts)
            .CarrierBudgets.InsertAllOnSubmit(CType(LinqTable, LTS.Carrier).CarrierBudgets)
            '.CarrierEDIs.InsertAllOnSubmit(CType(LinqTable, LTS.Carrier).CarrierEDIs)
            '.CarrierTrucks.InsertAllOnSubmit(CType(LinqTable, LTS.Carrier).CarrierTrucks)
        End With

    End Sub

    Protected Overrides Sub ProcessUpdatedDetails(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        With CType(oDB, NGLMASCarrierDataContext)
            ' Process any inserted contact records 
            .CarrierConts.InsertAllOnSubmit(GetCarrierContChanges(oData, TrackingInfo.Created))
            ' Process any updated contact records
            .CarrierConts.AttachAll(GetCarrierContChanges(oData, TrackingInfo.Updated), True)
            ' Process any deleted contact records
            Dim deletedContDetails = GetCarrierContChanges(oData, TrackingInfo.Deleted)
            .CarrierConts.AttachAll(deletedContDetails, True)
            .CarrierConts.DeleteAllOnSubmit(deletedContDetails)
            ' Process any inserted Budget records 
            .CarrierBudgets.InsertAllOnSubmit(GetCarrierBudgetChanges(oData, TrackingInfo.Created))
            ' Process any updated Budget records
            .CarrierBudgets.AttachAll(GetCarrierBudgetChanges(oData, TrackingInfo.Updated), True)
            ' Process any deleted Budget records
            Dim deletedBudDetails = GetCarrierBudgetChanges(oData, TrackingInfo.Deleted)
            .CarrierBudgets.AttachAll(deletedBudDetails, True)
            .CarrierBudgets.DeleteAllOnSubmit(deletedBudDetails)
            '' Process any inserted EDI records 
            '.CarrierEDIs.InsertAllOnSubmit(GetCarrierEDIChanges(oData, TrackingInfo.Created))
            '' Process any updated EDI records
            '.CarrierEDIs.AttachAll(GetCarrierEDIChanges(oData, TrackingInfo.Updated), True)
            '' Process any deleted EDI records
            'Dim deletedEDIDetails = GetCarrierEDIChanges(oData, TrackingInfo.Deleted)
            '.CarrierEDIs.AttachAll(deletedEDIDetails, True)
            '.CarrierEDIs.DeleteAllOnSubmit(deletedEDIDetails)
            '' Process any inserted Truck records 
            '.CarrierTrucks.InsertAllOnSubmit(GetCarrierTruckChanges(oData, TrackingInfo.Created))
            '' Process any updated Truck records
            '.CarrierTrucks.AttachAll(GetCarrierTruckChanges(oData, TrackingInfo.Updated), True)
            '' Process any deleted Truck records
            'Dim deletedTruckDetails = GetCarrierTruckChanges(oData, TrackingInfo.Deleted)
            '.CarrierTrucks.AttachAll(deletedTruckDetails, True)
            '.CarrierTrucks.DeleteAllOnSubmit(deletedTruckDetails)

        End With
    End Sub

    'Modified By LVV on 10/14/16 for v-7.0.5.110 Carrier Contact Changes
    Protected Function GetCarrierContChanges(ByVal source As DataTransferObjects.Carrier, ByVal changeType As TrackingInfo) As List(Of LTS.CarrierCont)
        ' Test record details for specified change type.
        ' If Updated is null, set to byte[0] (for inserted items).
        Dim details As IEnumerable(Of LTS.CarrierCont) = (
                From d In source.CarrierConts
                Where d.TrackingState = changeType
                Select New LTS.CarrierCont With {.CarrierContControl = d.CarrierContControl,
                .CarrierContCarrierControl = d.CarrierContCarrierControl,
                .CarrierContName = d.CarrierContName,
                .CarrierContTitle = d.CarrierContTitle,
                .CarrierContactPhone = d.CarrierContactPhone,
                .CarrierContPhoneExt = d.CarrierContPhoneExt,
                .CarrierContactFax = d.CarrierContactFax,
                .CarrierContact800 = d.CarrierContact800,
                .CarrierContactEMail = d.CarrierContactEMail,
                .CarrierContactDefault = d.CarrierContactDefault,
                .CarrierContUpdated = If(d.CarrierContUpdated Is Nothing, New Byte() {}, d.CarrierContUpdated)})
        Return details.ToList()
    End Function

    Protected Function GetCarrierBudgetChanges(ByVal source As DataTransferObjects.Carrier, ByVal changeType As TrackingInfo) As List(Of LTS.CarrierBudget)
        ' Test record details for specified change type.
        ' If Updated is null, set to byte[0] (for inserted items).
        Dim details As IEnumerable(Of LTS.CarrierBudget) = (
                From d In source.CarrierBudgets
                Where d.TrackingState = changeType
                Select New LTS.CarrierBudget With {.CarrierBudControl = d.CarrierBudControl,
                .CarrierBudCarrierControl = d.CarrierBudCarrierControl,
                .CarrierBudModDate = Date.Now,
                .CarrierBudModUser = Parameters.UserName,
                .CarrierBudExpMo1 = d.CarrierBudExpMo1,
                .CarrierBudExpMo2 = d.CarrierBudExpMo2,
                .CarrierBudExpMo3 = d.CarrierBudExpMo3,
                .CarrierBudExpMo4 = d.CarrierBudExpMo4,
                .CarrierBudExpMo5 = d.CarrierBudExpMo5,
                .CarrierBudExpMo6 = d.CarrierBudExpMo6,
                .CarrierBudExpMo7 = d.CarrierBudExpMo7,
                .CarrierBudExpMo8 = d.CarrierBudExpMo8,
                .CarrierBudExpMo9 = d.CarrierBudExpMo9,
                .CarrierBudExpMo10 = d.CarrierBudExpMo10,
                .CarrierBudExpMo11 = d.CarrierBudExpMo11,
                .CarrierBudExpMo12 = d.CarrierBudExpMo12,
                .CarrierBudExpTotal = d.CarrierBudExpTotal,
                .CarrierBudActMo1 = d.CarrierBudActMo1,
                .CarrierBudActMo2 = d.CarrierBudActMo2,
                .CarrierBudActMo3 = d.CarrierBudActMo3,
                .CarrierBudActMo4 = d.CarrierBudActMo4,
                .CarrierBudActMo5 = d.CarrierBudActMo5,
                .CarrierBudActMo6 = d.CarrierBudActMo6,
                .CarrierBudActMo7 = d.CarrierBudActMo7,
                .CarrierBudActMo8 = d.CarrierBudActMo8,
                .CarrierBudActMo9 = d.CarrierBudActMo9,
                .CarrierBudActMo10 = d.CarrierBudActMo10,
                .CarrierBudActMo11 = d.CarrierBudActMo11,
                .CarrierBudActMo12 = d.CarrierBudActMo12,
                .CarrierBudActTotal = d.CarrierBudActTotal,
                .CarrierBudgetUpdated = If(d.CarrierBudgetUpdated Is Nothing, New Byte() {}, d.CarrierBudgetUpdated)})
        Return details.ToList()
    End Function

    Protected Function GetCarrierEDIChanges(ByVal source As DataTransferObjects.Carrier, ByVal changeType As TrackingInfo) As List(Of LTS.CarrierEDI)
        ' Test record details for specified change type.
        ' If Updated is null, set to byte[0] (for inserted items).
        Dim details As IEnumerable(Of LTS.CarrierEDI) = (
                From d In source.CarrierEDIs
                Where d.TrackingState = changeType
                Select New LTS.CarrierEDI With {.CarrierEDIControl = d.CarrierEDIControl,
                .CarrierEDICarrierControl = d.CarrierEDICarrierControl,
                .CarrierEDIXaction = d.CarrierEDIXaction,
                .CarrierEDIComment = d.CarrierEDIComment,
                .CarrierEDISecurityQual = d.CarrierEDISecurityQual,
                .CarrierEDISecurityCode = d.CarrierEDISecurityCode,
                .CarrierEDIPartnerQual = d.CarrierEDIPartnerQual,
                .CarrierEDIPartnerCode = d.CarrierEDIPartnerCode,
                .CarrierEDIISASequence = d.CarrierEDIISASequence,
                .CarrierEDIGSSequence = d.CarrierEDIGSSequence,
                .CarrierEDIEmailNotificationOn = d.CarrierEDIEmailNotificationOn,
                .CarrierEDIEmailAddress = d.CarrierEDIEmailAddress,
                .CarrierEDIAcknowledgementRequested = d.CarrierEDIAcknowledgementRequested,
                .CarrierEDIAcceptOn997 = d.CarrierEDIAcceptOn997,
                .CarrierEDITestCode = d.CarrierEDITestCode,
                .CarrierEDIUpdated = If(d.CarrierEDIUpdated Is Nothing, New Byte() {}, d.CarrierEDIUpdated),
                .CarrierEDIInboundFolder = d.CarrierEDIInboundFolder,
                .CarrierEDIBackupFolder = d.CarrierEDIBackupFolder,
                .CarrierEDILogFile = d.CarrierEDILogFile,
                .CarrierEDIStartTime = d.CarrierEDIStartTime,
                .CarrierEDIEndTime = d.CarrierEDIEndTime,
                .CarrierEDIDaysOfWeek = d.CarrierEDIDaysOfWeek,
                .CarrierEDISendMinutesOutbound = d.CarrierEDISendMinutesOutbound,
                .CarrierEDIFileNameBaseOutbound = d.CarrierEDIFileNameBaseOutbound,
                .CarrierEDIFileNameBaseInbound = d.CarrierEDIFileNameBaseInbound,
                .CarrierEDIWebServiceAuthKey = d.CarrierEDIWebServiceAuthKey,
                .CarrierEDINGLEDIInputWebURL = d.CarrierEDINGLEDIInputWebURL,
                .CarrierEDINGLEDI204OutputWebURL = d.CarrierEDINGLEDI204OutputWebURL,
                .CarrierEDIOutboundFolder = d.CarrierEDIOutboundFolder,
                .CarrierEDILastOutboundTransmission = d.CarrierEDILastOutboundTransmission,
                .CarrierEDIFTPOutboundFolder = d.CarrierEDIFTPOutboundFolder,
                .CarrierEDIFTPBackupFolder = d.CarrierEDIFTPBackupFolder,
                .CarrierEDIFTPInboundFolder = d.CarrierEDIFTPInboundFolder,
                .CarrierEDIFTPServer = d.CarrierEDIFTPServer,
                .CarrierEDIFTPUserName = d.CarrierEDIFTPUserName,
                .CarrierEDIFTPPassword = d.CarrierEDIFTPPassword,
                .CarrierEDIWebServiceURL = d.CarrierEDIWebServiceURL})
        Return details.ToList()
    End Function

    Protected Function GetCarrierTruckChanges(ByVal source As DataTransferObjects.Carrier, ByVal changeType As TrackingInfo) As List(Of LTS.CarrierTruck)
        ' Test record details for specified change type.
        ' If Updated is null, set to byte[0] (for inserted items).
        Dim details As IEnumerable(Of LTS.CarrierTruck) = (
                From d In source.CarrierTrucks
                Where d.TrackingState = changeType
                Select New LTS.CarrierTruck With {.CarrierTruckControl = d.CarrierTruckControl _
                , .CarrierTruckCarrierControl = d.CarrierTruckCarrierControl _
                , .CarrierTruckDescription = d.CarrierTruckDescription _
                , .CarrierTruckWgtFrom = d.CarrierTruckWgtFrom _
                , .CarrierTruckWgtTo = d.CarrierTruckWgtTo _
                , .CarrierTruckRateStarts = d.CarrierTruckRateStarts _
                , .CarrierTruckRateExpires = d.CarrierTruckRateExpires _
                , .CarrierTruckTL = d.CarrierTruckTL _
                , .CarrierTruckLTL = d.CarrierTruckLTL _
                , .CarrierTruckEquipment = d.CarrierTruckEquipment _
                , .CarrierTruckMileRate = d.CarrierTruckMileRate _
                , .CarrierTruckCwtRate = d.CarrierTruckCwtRate _
                , .CarrierTruckCaseRate = d.CarrierTruckCaseRate _
                , .CarrierTruckFlatRate = d.CarrierTruckFlatRate _
                , .CarrierTruckPltRate = d.CarrierTruckPltRate _
                , .CarrierTruckCubeRate = d.CarrierTruckCubeRate _
                , .CarrierTruckTLT = d.CarrierTruckTLT _
                , .CarrierTruckTMode = d.CarrierTruckTMode _
                , .CarrierTruckFAK = d.CarrierTruckFAK _
                , .CarrierTruckDisc = d.CarrierTruckDisc _
                , .CarrierTruckPUMon = d.CarrierTruckPUMon _
                , .CarrierTruckPUTue = d.CarrierTruckPUTue _
                , .CarrierTruckPUWed = d.CarrierTruckPUWed _
                , .CarrierTruckPUThu = d.CarrierTruckPUThu _
                , .CarrierTruckPUFri = d.CarrierTruckPUFri _
                , .CarrierTruckPUSat = d.CarrierTruckPUSat _
                , .CarrierTruckPUSun = d.CarrierTruckPUSun _
                , .CarrierTruckDLMon = d.CarrierTruckDLMon _
                , .CarrierTruckDLTue = d.CarrierTruckDLTue _
                , .CarrierTruckDLWed = d.CarrierTruckDLWed _
                , .CarrierTruckDLThu = d.CarrierTruckDLThu _
                , .CarrierTruckDLFri = d.CarrierTruckDLFri _
                , .CarrierTruckDLSat = d.CarrierTruckDLSat _
                , .CarrierTruckDLSun = d.CarrierTruckDLSun _
                , .CarrierTruckPayTolPerLo = d.CarrierTruckPayTolPerLo _
                , .CarrierTruckPayTolPerHi = d.CarrierTruckPayTolPerHi _
                , .CarrierTruckPayTolCurLo = d.CarrierTruckPayTolCurLo _
                , .CarrierTruckPayTolCurHi = d.CarrierTruckPayTolCurHi _
                , .CarrierTruckCurType = d.CarrierTruckCurType _
                , .CarrierTruckModUser = Parameters.UserName _
                , .CarrierTruckModDate = Date.Now _
                , .CarrierTruckRoute = d.CarrierTruckRoute _
                , .CarrierTruckMiles = d.CarrierTruckMiles _
                , .CarrierTruckBkhlCostPerc = d.CarrierTruckBkhlCostPerc _
                , .CarrierTruckPalletCostPer = d.CarrierTruckPalletCostPer _
                , .CarrierTruckFuelSurChargePerc = d.CarrierTruckFuelSurChargePerc _
                , .CarrierTruckStopCharge = d.CarrierTruckStopCharge _
                , .CarrierTruckDropCost = d.CarrierTruckDropCost _
                , .CarrierTruckUnloadDiff = d.CarrierTruckUnloadDiff _
                , .CarrierTruckCasesAvailable = d.CarrierTruckCasesAvailable _
                , .CarrierTruckCasesOpen = d.CarrierTruckCasesOpen _
                , .CarrierTruckCasesCommitted = d.CarrierTruckCasesCommitted _
                , .CarrierTruckWgtAvailable = d.CarrierTruckWgtAvailable _
                , .CarrierTruckWgtOpen = d.CarrierTruckWgtOpen _
                , .CarrierTruckWgtCommitted = d.CarrierTruckWgtCommitted _
                , .CarrierTruckCubesAvailable = d.CarrierTruckCubesAvailable _
                , .CarrierTruckCubesOpen = d.CarrierTruckCubesOpen _
                , .CarrierTruckCubesCommitted = d.CarrierTruckCubesCommitted _
                , .CarrierTruckPltsAvailable = d.CarrierTruckPltsAvailable _
                , .CarrierTruckPltsOpen = d.CarrierTruckPltsOpen _
                , .CarrierTruckPltsCommitted = d.CarrierTruckPltsCommitted _
                , .CarrierTruckTrucksAvailable = d.CarrierTruckTrucksAvailable _
                , .CarrierTruckMaxLoadsByWeek = d.CarrierTruckMaxLoadsByWeek _
                , .CarrierTruckMaxLoadsByMonth = d.CarrierTruckMaxLoadsByMonth _
                , .CarrierTruckTotalLoadsForWeek = d.CarrierTruckTotalLoadsForWeek _
                , .CarrierTruckTotalLoadsForMonth = d.CarrierTruckTotalLoadsForMonth _
                , .CarrierTruckWeekDate = d.CarrierTruckWeekDate _
                , .CarrierTruckMonthDate = d.CarrierTruckMonthDate _
                , .CarrierTruckTempType = d.CarrierTruckTempType _
                , .CarrierTruckHazmat = d.CarrierTruckHazmat _
                , .CarrierTruckUpdated = If(d.CarrierTruckUpdated Is Nothing, New Byte() {}, d.CarrierTruckUpdated)})
        Return details.ToList()
    End Function

    Public Function getNewCarrierCodeValues(ByRef intCodeVal1 As Integer, ByRef intCodeVal2 As Integer) As Boolean
        Dim blnRet As Boolean = False
        Try
            intCodeVal1 = getScalarInteger("Select dbo.getLastCarrierCode1() as RetVal")
            intCodeVal2 = getScalarInteger("Select dbo.getLastCarrierCode2() as RetVal")
            intCodeVal2 = intCodeVal2 + 1
            If intCodeVal2 >= 255 Then
                intCodeVal2 = 33
                intCodeVal1 = intCodeVal1 + 1
                If intCodeVal1 = 160 Then intCodeVal1 = 161
                If intCodeVal1 = 173 Then intCodeVal1 = 174
            End If
            If intCodeVal1 > 255 Then
                Utilities.SaveAppError("E_MaxNbrOfCarriers", Me.Parameters)
                Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_MaxNbrOfCarriers"}, New FaultReason("E_CreateRecordFailure"))
            End If
            If intCodeVal2 < 40 Then intCodeVal2 = 40
            If intCodeVal2 = 45 Then intCodeVal2 = 46
            If intCodeVal2 > 96 And intCodeVal2 < 123 Then intCodeVal2 = 124
            If intCodeVal2 = 126 Then intCodeVal2 = 128
            If intCodeVal2 = 127 Then intCodeVal2 = 128
            If intCodeVal2 = 160 Then intCodeVal2 = 161
            If intCodeVal1 = 173 Then intCodeVal1 = 174
            blnRet = True
        Catch ex As FaultException
            Throw
        Catch ex As Exception
            Utilities.SaveAppError(ex.Message, Me.Parameters)
            Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = ex.Message}, New FaultReason("E_UnExpected"))
        End Try
        Return blnRet
    End Function

    Protected Overrides Sub ValidateNewRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        'Check if the data already exists only one allowed       
        With CType(oData, DataTransferObjects.Carrier)
            Try
                'Validate that required fields have been entered
                Dim strKeys As String = ""
                Dim strVals As String = ""
                Dim blnValidationErr As Boolean = False
                Dim strValidationMsg As String = ""
                Dim strSpacer As String = ""
                If .CarrierNumber = 0 Then
                    blnValidationErr = True
                    strValidationMsg = "Carrier Number"
                    strSpacer = "; "
                End If

                If String.IsNullOrEmpty(.CarrierName) OrElse .CarrierName.Trim.Length < 1 Then
                    blnValidationErr = True
                    strValidationMsg &= strSpacer & "Carrier Name"
                    strSpacer = "; "
                End If

                If blnValidationErr Then
                    throwInvalidRequiredKeysException("Carrier", strValidationMsg)
                End If
                Dim blnExists As Boolean = False
                strKeys = "Carrier Name; Carrier Number"
                strVals = .CarrierName & "; " & .CarrierNumber
                If Not String.IsNullOrEmpty(.CarrierAlphaCode) AndAlso .CarrierAlphaCode.Trim.Length > 0 Then
                    If Not String.IsNullOrEmpty(.CarrierLegalEntity) AndAlso .CarrierLegalEntity.Trim.Length > 0 Then
                        'it is possible for the CarrierAlphaCode and Carrier Name to duplciate across Legal Entities
                        strKeys &= "; Carrier Alpha Code; Carrier Legal Entity"
                        strVals &= "; " & .CarrierAlphaCode & "; " & .CarrierLegalEntity
                        blnExists = CType(oDB, NGLMASCarrierDataContext).Carriers.Any(Function(x) x.CarrierName = .CarrierName And x.CarrierNumber = .CarrierNumber And x.CarrierAlphaCode = .CarrierAlphaCode And x.CarrierLegalEntity = .CarrierLegalEntity)
                    Else
                        strKeys &= ": Carrier Alpha Code"
                        strVals &= "; " & .CarrierAlphaCode
                        blnExists = CType(oDB, NGLMASCarrierDataContext).Carriers.Any(Function(x) x.CarrierName = .CarrierName And x.CarrierNumber = .CarrierNumber And x.CarrierAlphaCode = .CarrierAlphaCode)
                    End If
                Else
                    blnExists = CType(oDB, NGLMASCarrierDataContext).Carriers.Any(Function(x) x.CarrierName = .CarrierName And x.CarrierNumber = .CarrierNumber)
                End If

                If blnExists Then throwInvalidKeysAlreadyExistsException("Carrier", strKeys, strVals)

                Dim intCodeVal1 As Integer = 0
                Dim intCodeVal2 As Integer = 0
                If Not getNewCarrierCodeValues(intCodeVal1, intCodeVal2) Then
                    Utilities.SaveAppError("Cannot save new Carrier data: unable to get new Carrier Code Values.", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_GetCarrierCodeValFailure"}, New FaultReason("E_DataValidationFailure"))
                End If
                .CarrierCodeVal1 = intCodeVal1
                .CarrierCodeVal2 = intCodeVal2

            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("ValidateNewRecord"))
            End Try
        End With
    End Sub

    Protected Overrides Sub ValidateUpdatedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        'Check if the data already exists only one allowed
        With CType(oData, DataTransferObjects.Carrier)
            Try
                'Validate that required fields have been entered
                Dim blnValidationErr As Boolean = False
                Dim strValidationMsg As String = ""
                Dim strSpacer As String = ""
                If .CarrierNumber = 0 Then
                    blnValidationErr = True
                    strValidationMsg = "Carrier Number"
                    strSpacer = "; "
                End If

                If String.IsNullOrEmpty(.CarrierName) OrElse .CarrierName.Trim.Length < 1 Then
                    blnValidationErr = True
                    strValidationMsg &= strSpacer & "Carrier Name"
                    strSpacer = "; "
                End If

                If blnValidationErr Then
                    throwInvalidRequiredKeysException("Carrier", strValidationMsg)
                End If
                Dim blnExists As Boolean = False
                blnExists = CType(oDB, NGLMASCarrierDataContext).Carriers.Any(Function(x) x.CarrierControl <> .CarrierControl And x.CarrierNumber = .CarrierNumber)
                If blnExists Then throwInvalidKeyAlreadyExistsException("Carrier", "Carrier Number", .CarrierNumber)
            Catch ex As FaultException
                Throw
            Catch ex As Exception
                ManageLinqDataExceptions(ex, buildProcedureName("ValidateUpdatedRecord"))
            End Try

        End With
    End Sub

    ''' <summary>
    ''' Copies the LTS data into a new DTO object
    ''' </summary>
    ''' <param name="d"></param>
    ''' <param name="db"></param>
    ''' <param name="page"></param>
    ''' <param name="pagecount"></param>
    ''' <param name="recordcount"></param>
    ''' <param name="pagesize"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Created by RHR v-7.0.5.110 8/12/2016
    ''' </remarks>
    ''' 
    Friend Shared Function selectDTOData(ByVal d As LTS.Carrier, ByVal db As NGLMASCarrierDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000, Optional ByVal addContacts As Boolean = False) As DataTransferObjects.Carrier
        Dim oDTO As New DataTransferObjects.Carrier
        'the original Carrier DTO object was not designed to support NULL integers
        'so we need to process those values using skipObjs
        Dim skipObjs As New List(Of String) From {"CarrierUpdated",
                "CarrierNumber",
                "CarrierActive",
                "CarrierGradReliability",
                "CarrierGradBillingAccuracy",
                "CarrierGradFinancialStrength",
                "CarrierGradEquipmentCondition",
                "CarrierGradContactAttitude",
                "CarrierGradDriverAttitude",
                "CarrierGradClaimFrequency",
                "CarrierGradClaimPayment",
                "CarrierGradGeographicCoverage",
                "CarrierGradCustomerService",
                "CarrierGradPriceChangeNotification",
                "CarrierGradPriceChangeFrequency",
                "CarrierGradPriceAggressiveness",
                "CarrierGradAverage",
                "CarrierQualQualified",
                "CarrierQualContract",
                "CarrierQualMaxPerShipment",
                "CarrierQualMaxAllShipments",
                "CarrierQualCurAllExposure",
                "CarrierCodeVal1",
                "CarrierCodeVal2",
                "CarrierTruckDefault",
                "CarrierAllowWebTender",
                "CarrierSmartWayScore",
                "Page",
                "Pages",
                "RecordCount",
                "PageSize"}
        oDTO = CopyMatchingFields(oDTO, d, skipObjs)
        'add custom formatting
        With oDTO
            .CarrierNumber = If(d.CarrierNumber.HasValue, d.CarrierNumber.Value, 0)
            .CarrierActive = If(d.CarrierActive.HasValue, d.CarrierActive.Value, False)
            .CarrierGradReliability = If(d.CarrierGradReliability.HasValue, d.CarrierGradReliability.Value, 0)
            .CarrierGradBillingAccuracy = If(d.CarrierGradBillingAccuracy.HasValue, d.CarrierGradBillingAccuracy.Value, 0)
            .CarrierGradFinancialStrength = If(d.CarrierGradFinancialStrength.HasValue, d.CarrierGradFinancialStrength.Value, 0)
            .CarrierGradEquipmentCondition = If(d.CarrierGradEquipmentCondition.HasValue, d.CarrierGradEquipmentCondition.Value, 0)
            .CarrierGradContactAttitude = If(d.CarrierGradContactAttitude.HasValue, d.CarrierGradContactAttitude.Value, 0)
            .CarrierGradDriverAttitude = If(d.CarrierGradDriverAttitude.HasValue, d.CarrierGradDriverAttitude.Value, 0)
            .CarrierGradClaimFrequency = If(d.CarrierGradClaimFrequency.HasValue, d.CarrierGradClaimFrequency.Value, 0)
            .CarrierGradClaimPayment = If(d.CarrierGradClaimPayment.HasValue, d.CarrierGradClaimPayment.Value, 0)
            .CarrierGradGeographicCoverage = If(d.CarrierGradGeographicCoverage.HasValue, d.CarrierGradGeographicCoverage.Value, 0)
            .CarrierGradCustomerService = If(d.CarrierGradCustomerService.HasValue, d.CarrierGradCustomerService.Value, 0)
            .CarrierGradPriceChangeNotification = If(d.CarrierGradPriceChangeNotification.HasValue, d.CarrierGradPriceChangeNotification.Value, 0)
            .CarrierGradPriceChangeFrequency = If(d.CarrierGradPriceChangeFrequency.HasValue, d.CarrierGradPriceChangeFrequency.Value, 0)
            .CarrierGradPriceAggressiveness = If(d.CarrierGradPriceAggressiveness.HasValue, d.CarrierGradPriceAggressiveness.Value, 0)
            .CarrierGradAverage = If(d.CarrierGradAverage.HasValue, d.CarrierGradAverage.Value, 0)
            .CarrierQualQualified = If(d.CarrierQualQualified.HasValue, d.CarrierQualQualified.Value, False)
            .CarrierQualContract = If(d.CarrierQualContract.HasValue, d.CarrierQualContract.Value, False)
            .CarrierQualMaxPerShipment = If(d.CarrierQualMaxPerShipment.HasValue, d.CarrierQualMaxPerShipment.Value, 0)
            .CarrierQualMaxAllShipments = If(d.CarrierQualMaxAllShipments.HasValue, d.CarrierQualMaxAllShipments.Value, 0)
            .CarrierQualCurAllExposure = If(d.CarrierQualCurAllExposure.HasValue, d.CarrierQualCurAllExposure.Value, 0)
            .CarrierCodeVal1 = If(d.CarrierCodeVal1.HasValue, d.CarrierCodeVal1.Value, 0)
            .CarrierCodeVal2 = If(d.CarrierCodeVal2.HasValue, d.CarrierCodeVal2.Value, 0)
            .CarrierTruckDefault = If(d.CarrierTruckDefault.HasValue, d.CarrierTruckDefault.Value, 0)
            .CarrierAllowWebTender = If(d.CarrierAllowWebTender.HasValue, d.CarrierAllowWebTender.Value, False)
            .CarrierSmartWayScore = If(d.CarrierSmartWayScore.HasValue, d.CarrierSmartWayScore.Value, 0)
            .CarrierUpdated = d.CarrierUpdated.ToArray()
            .Page = page
            .Pages = pagecount
            .RecordCount = recordcount
            .PageSize = pagesize
        End With

        If addContacts Then
            oDTO.CarrierConts = (From t In d.CarrierConts Select selectDTOContData(t, db)).ToList()
        End If
        Return oDTO

    End Function

    ' ''' <summary>
    ' ''' Copies the LTS data into a new DTO object
    ' ''' </summary>
    ' ''' <param name="d"></param>
    ' ''' <param name="db"></param>
    ' ''' <param name="page"></param>
    ' ''' <param name="pagecount"></param>
    ' ''' <param name="recordcount"></param>
    ' ''' <param name="pagesize"></param>
    ' ''' <returns></returns>
    ' ''' <remarks>
    ' ''' Created by RHR v-7.0.5.110 8/12/2016
    ' ''' </remarks>
    ' ''' 
    'Friend Shared Function selectDTODataLite(ByVal d As LTS.Carrier, ByVal db As NGLMASCarrierDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000, Optional ByVal addContacts As Boolean = False) As DTO.Carrier

    '    Dim oData As New DTO.Carrier With {.CarrierControl = d.CarrierControl _
    '                                            , .CarrierNumber = If(d.CarrierNumber.HasValue, d.CarrierNumber.Value, 0) _
    '                                            , .CarrierLegalEntity = d.CarrierLegalEntity _
    '                                            , .CarrierAlphaCode = d.CarrierAlphaCode _
    '                                            , .CarrierName = d.CarrierName _
    '                                            , .CarrierStreetAddress1 = d.CarrierStreetAddress1 _
    '                                            , .CarrierStreetAddress2 = d.CarrierStreetAddress2 _
    '                                            , .CarrierStreetAddress3 = d.CarrierStreetAddress3 _
    '                                            , .CarrierStreetCity = d.CarrierStreetCity _
    '                                            , .CarrierStreetState = d.CarrierStreetState _
    '                                            , .CarrierStreetCountry = d.CarrierStreetCountry _
    '                                            , .CarrierStreetZip = d.CarrierStreetZip _
    '                                            , .CarrierMailAddress1 = d.CarrierMailAddress1 _
    '                                            , .CarrierMailAddress2 = d.CarrierMailAddress2 _
    '                                            , .CarrierMailAddress3 = d.CarrierMailAddress3 _
    '                                            , .CarrierMailCity = d.CarrierMailCity _
    '                                            , .CarrierMailState = d.CarrierMailState _
    '                                            , .CarrierMailCountry = d.CarrierMailCountry _
    '                                            , .CarrierMailZip = d.CarrierMailZip _
    '                                            , .CarrierTypeCode = d.CarrierTypeCode _
    '                                            , .CarrierSCAC = d.CarrierSCAC _
    '                                            , .CarrierWebSite = d.CarrierWebSite _
    '                                            , .CarrierEmail = d.CarrierEmail _
    '                                            , .CarrierQualInsuranceDate = d.CarrierQualInsuranceDate _
    '                                            , .CarrierQualQualified = If(d.CarrierQualQualified.HasValue, d.CarrierQualQualified.Value, True) _
    '                                            , .CarrierQualAuthority = d.CarrierQualAuthority _
    '                                            , .CarrierQualContract = If(d.CarrierQualContract.HasValue, d.CarrierQualContract.Value, False) _
    '                                            , .CarrierQualSignedDate = d.CarrierQualSignedDate _
    '                                            , .CarrierQualContractExpiresDate = d.CarrierQualContractExpiresDate _
    '                                            , .CarrierUser1 = d.CarrierUser1 _
    '                                            , .CarrierUser2 = d.CarrierUser2 _
    '                                            , .CarrierUser3 = d.CarrierUser3 _
    '                                            , .CarrierUser4 = d.CarrierUser4 _
    '                                            , .CarrierQualUSDot = d.CarrierQualUSDot _
    '                                            , .CarrierQualCSAScore = d.CarrierQualCSAScore _
    '                                            , .CarrierCalcOnTimeServiceLevel = d.CarrierCalcOnTimeServiceLevel _
    '                                            , .CarrierAssignedOnTimeServiceLevel = d.CarrierAssignedOnTimeServiceLevel _
    '                                            , .CarrierCalcOnTimeNoMonthsUsed = d.CarrierCalcOnTimeNoMonthsUsed _
    '                                            , .CarrierUpdated = d.CarrierUpdated.ToArray() _
    '                                            , .Page = page _
    '                                            , .Pages = pagecount _
    '                                            , .RecordCount = recordcount _
    '                                            , .PageSize = pagesize}

    '    Return oData

    'End Function

    ''' <summary>
    ''' Copies the LTS data into a new DTO object
    ''' </summary>
    ''' <param name="d"></param>
    ''' <param name="db"></param>
    ''' <param name="page"></param>
    ''' <param name="pagecount"></param>
    ''' <param name="recordcount"></param>
    ''' <param name="pagesize"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Modified by RHR 7.0.5.110 8/12/2016
    '''   the function is now shared and uses the CopyMatchingFields logic
    '''   we also now support paging by adding the page data to the DTO object
    ''' </remarks>
    Friend Shared Function selectDTOContData(ByVal d As LTS.CarrierCont, ByVal db As NGLMASCarrierDataContext, Optional page As Integer = 1, Optional pagecount As Integer = 1, Optional ByVal recordcount As Integer = 1, Optional pagesize As Integer = 1000) As DataTransferObjects.CarrierCont
        Dim oDTO As New DataTransferObjects.CarrierCont

        Dim skipObjs As New List(Of String) From {"CarrierContUpdated",
                "Page",
                "Pages",
                "RecordCount",
                "PageSize"}
        oDTO = CopyMatchingFields(oDTO, d, skipObjs)
        'add custom formatting
        With oDTO
            .CarrierContUpdated = d.CarrierContUpdated.ToArray()
            .Page = page
            .Pages = pagecount
            .RecordCount = recordcount
            .PageSize = pagesize
        End With
        Return oDTO



    End Function

#End Region

End Class