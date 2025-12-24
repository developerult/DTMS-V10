Imports System.Data.Linq
Imports System.ServiceModel
Imports NGL.Core.ChangeTracker

Public Class NGLCarrAdHocData : Inherits NDPBaseClass

#Region " Constructors "

    Public Sub New(ByVal oParameters As WCFParameters)
        MyBase.New()
        Me.Parameters = oParameters
        Dim db As New NGLMASCarrierDataContext(ConnectionString)
        Me.LinqTable = db.CarrAdHocs
        Me.LinqDB = db
        Me.SourceClass = "NGLCarrAdHocData"
    End Sub

#End Region

#Region " Properties "

    Protected Overrides Property LinqTable() As Object
        Get
            If _LinqTable Is Nothing Then
                Dim db As New NGLMASCarrierDataContext(ConnectionString)
                _LinqTable = db.CarrAdHocs
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
        Return GetCarrAdHocFiltered(Control:=Control)
    End Function

    Public Overrides Function GetRecordsFiltered() As DataTransferObjects.DTOBaseClass()
        Return GetCarrAdHocsFiltered()
    End Function

    Public Function GetCarrAdHocFiltered(Optional ByVal Control As Integer = 0, Optional ByVal Number As Integer = 0, Optional ByVal Name As String = "") As DataTransferObjects.CarrAdHoc
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim oDLO As New DataLoadOptions
                oDLO.LoadWith(Of LTS.CarrAdHoc)(Function(t As LTS.CarrAdHoc) t.CarrAdHocConts)
                oDLO.LoadWith(Of LTS.CarrAdHoc)(Function(t As LTS.CarrAdHoc) t.CarrAdHocBudgets)
                db.LoadOptions = oDLO
                'Get the newest record that matches the provided criteria
                Dim CarrAdHoc As DataTransferObjects.CarrAdHoc = (
                        From t In db.CarrAdHocs
                        Where
                        (t.CarrAdHocControl = If(Control = 0, t.CarrAdHocControl, Control)) _
                        And
                        (t.CarrAdHocNumber = If(Number = 0, t.CarrAdHocNumber, Number)) _
                        And
                        (Name Is Nothing OrElse String.IsNullOrEmpty(Name) OrElse t.CarrAdHocName = Name)
                        Order By t.CarrAdHocControl Descending
                        Select New DataTransferObjects.CarrAdHoc With {.CarrAdHocControl = t.CarrAdHocControl _
                        , .CarrAdHocNumber = If(t.CarrAdHocNumber.HasValue, t.CarrAdHocNumber.Value, 0) _
                        , .CarrAdHocName = t.CarrAdHocName _
                        , .CarrAdHocStreetAddress1 = t.CarrAdHocStreetAddress1 _
                        , .CarrAdHocStreetAddress2 = t.CarrAdHocStreetAddress2 _
                        , .CarrAdHocStreetAddress3 = t.CarrAdHocStreetAddress3 _
                        , .CarrAdHocStreetCity = t.CarrAdHocStreetCity _
                        , .CarrAdHocStreetState = t.CarrAdHocStreetState _
                        , .CarrAdHocStreetCountry = t.CarrAdHocStreetCountry _
                        , .CarrAdHocStreetZip = t.CarrAdHocStreetZip _
                        , .CarrAdHocMailAddress1 = t.CarrAdHocMailAddress1 _
                        , .CarrAdHocMailAddress2 = t.CarrAdHocMailAddress2 _
                        , .CarrAdHocMailAddress3 = t.CarrAdHocMailAddress3 _
                        , .CarrAdHocMailCity = t.CarrAdHocMailCity _
                        , .CarrAdHocMailState = t.CarrAdHocMailState _
                        , .CarrAdHocMailCountry = t.CarrAdHocMailCountry _
                        , .CarrAdHocMailZip = t.CarrAdHocMailZip _
                        , .CarrAdHocModDate = t.CarrAdHocModDate _
                        , .CarrAdHocModUser = t.CarrAdHocModUser _
                        , .CarrAdHocSCAC = t.CarrAdHocSCAC _
                        , .CarrAdHocAccountNo = t.CarrAdHocAccountNo _
                        , .CarrAdHocTypeCode = t.CarrAdHocTypeCode _
                        , .CarrAdHocGeneralInfo = t.CarrAdHocGeneralInfo _
                        , .CarrAdHocActive = If(t.CarrAdHocActive.HasValue, t.CarrAdHocActive.Value, False) _
                        , .CarrAdHocWebSite = t.CarrAdHocWebSite _
                        , .CarrAdHocEmail = t.CarrAdHocEmail _
                        , .CarrAdHocNEXTStopAcct = t.CarrAdHocNEXTStopAcct _
                        , .CarrAdHocNEXTStopPwd = t.CarrAdHocNEXTStopPwd _
                        , .CarrAdHocGradReliability = If(t.CarrAdHocGradReliability.HasValue, t.CarrAdHocGradReliability.Value, 0) _
                        , .CarrAdHocGradBillingAccuracy = If(t.CarrAdHocGradBillingAccuracy.HasValue, t.CarrAdHocGradBillingAccuracy.Value, 0) _
                        , .CarrAdHocGradFinancialStrength = If(t.CarrAdHocGradFinancialStrength.HasValue, t.CarrAdHocGradFinancialStrength.Value, 0) _
                        , .CarrAdHocGradEquipmentCondition = If(t.CarrAdHocGradEquipmentCondition.HasValue, t.CarrAdHocGradEquipmentCondition.Value, 0) _
                        , .CarrAdHocGradContactAttitude = If(t.CarrAdHocGradContactAttitude.HasValue, t.CarrAdHocGradContactAttitude.Value, 0) _
                        , .CarrAdHocGradDriverAttitude = If(t.CarrAdHocGradDriverAttitude.HasValue, t.CarrAdHocGradDriverAttitude.Value, 0) _
                        , .CarrAdHocGradClaimFrequency = If(t.CarrAdHocGradClaimFrequency.HasValue, t.CarrAdHocGradClaimFrequency.Value, 0) _
                        , .CarrAdHocGradClaimPayment = If(t.CarrAdHocGradClaimPayment.HasValue, t.CarrAdHocGradClaimPayment.Value, 0) _
                        , .CarrAdHocGradGeographicCoverage = If(t.CarrAdHocGradGeographicCoverage.HasValue, t.CarrAdHocGradGeographicCoverage.Value, 0) _
                        , .CarrAdHocGradCustomerService = If(t.CarrAdHocGradCustomerService.HasValue, t.CarrAdHocGradCustomerService.Value, 0) _
                        , .CarrAdHocGradPriceChangeNotification = If(t.CarrAdHocGradPriceChangeNotification.HasValue, t.CarrAdHocGradPriceChangeNotification.Value, 0) _
                        , .CarrAdHocGradPriceChangeFrequency = If(t.CarrAdHocGradPriceChangeFrequency.HasValue, t.CarrAdHocGradPriceChangeFrequency.Value, 0) _
                        , .CarrAdHocGradPriceAggressiveness = If(t.CarrAdHocGradPriceAggressiveness.HasValue, t.CarrAdHocGradPriceAggressiveness.Value, 0) _
                        , .CarrAdHocGradAverage = If(t.CarrAdHocGradAverage.HasValue, t.CarrAdHocGradAverage.Value, 0) _
                        , .CarrAdHocQualInsuranceDate = t.CarrAdHocQualInsuranceDate _
                        , .CarrAdHocQualQualified = If(t.CarrAdHocQualQualified.HasValue, t.CarrAdHocQualQualified.Value, False) _
                        , .CarrAdHocQualAuthority = t.CarrAdHocQualAuthority _
                        , .CarrAdHocQualContract = If(t.CarrAdHocQualContract.HasValue, t.CarrAdHocQualContract.Value, False) _
                        , .CarrAdHocQualSignedDate = t.CarrAdHocQualSignedDate _
                        , .CarrAdHocQualContractExpiresDate = t.CarrAdHocQualContractExpiresDate _
                        , .CarrAdHocQualMaxPerShipment = If(t.CarrAdHocQualMaxPerShipment.HasValue, t.CarrAdHocQualMaxPerShipment.Value, 0) _
                        , .CarrAdHocQualMaxAllShipments = If(t.CarrAdHocQualMaxAllShipments.HasValue, t.CarrAdHocQualMaxAllShipments.Value, 0) _
                        , .CarrAdHocQualCurAllExposure = If(t.CarrAdHocQualCurAllExposure.HasValue, t.CarrAdHocQualCurAllExposure.Value, 0) _
                        , .CarrAdHocCodeVal1 = If(t.CarrAdHocCodeVal1.HasValue, t.CarrAdHocCodeVal1.Value, 0) _
                        , .CarrAdHocCodeVal2 = If(t.CarrAdHocCodeVal2.HasValue, t.CarrAdHocCodeVal2.Value, 0) _
                        , .CarrAdHocTruckDefault = If(t.CarrAdHocTruckDefault.HasValue, t.CarrAdHocTruckDefault.Value, 0) _
                        , .CarrAdHocAllowWebTender = If(t.CarrAdHocAllowWebTender.HasValue, t.CarrAdHocAllowWebTender.Value, False) _
                        , .CarrAdHocIgnoreTariff = t.CarrAdHocIgnoreTariff _
                        , .CarrAdHocSmartWayPartnerType = t.CarrAdHocSmartWayPartnerType _
                        , .CarrAdHocSmartWayScore = If(t.CarrAdHocSmartWayScore.HasValue, t.CarrAdHocSmartWayScore.Value, 0) _
                        , .CarrAdHocSmartWayPartner = t.CarrAdHocSmartWayPartner _
                        , .CarrAdHocUpdated = t.CarrAdHocUpdated.ToArray() _
                        , .CarrAdHocConts = (
                        From d In t.CarrAdHocConts
                        Select New DataTransferObjects.CarrAdHocCont With {.CarrAdHocContControl = d.CarrAdHocContControl _
                        , .CarrAdHocContCarrAdHocControl = d.CarrAdHocContCarrAdHocControl _
                        , .CarrAdHocContName = d.CarrAdHocContName _
                        , .CarrAdHocContTitle = d.CarrAdHocContTitle _
                        , .CarrAdHocContactPhone = d.CarrAdHocContactPhone _
                        , .CarrAdHocContactFax = d.CarrAdHocContactFax _
                        , .CarrAdHocContPhoneExt = d.CarrAdHocContPhoneExt _
                        , .CarrAdHocContact800 = d.CarrAdHocContact800 _
                        , .CarrAdHocContactEMail = d.CarrAdHocContactEMail _
                        , .CarrAdHocContUpdated = d.CarrAdHocContUpdated.ToArray()}).ToList() _
                        , .CarrAdHocBudgets = (
                        From b In t.CarrAdHocBudgets
                        Select New DataTransferObjects.CarrAdHocBudget With {.CarrAdHocBudControl = b.CarrAdHocBudControl _
                        , .CarrAdHocBudCarrAdHocControl = b.CarrAdHocBudCarrAdHocControl _
                        , .CarrAdHocBudModDate = b.CarrAdHocBudModDate _
                        , .CarrAdHocBudModUser = b.CarrAdHocBudModUser _
                        , .CarrAdHocBudExpMo1 = If(b.CarrAdHocBudExpMo1.HasValue, b.CarrAdHocBudExpMo1.Value, 0) _
                        , .CarrAdHocBudExpMo2 = If(b.CarrAdHocBudExpMo2.HasValue, b.CarrAdHocBudExpMo2.Value, 0) _
                        , .CarrAdHocBudExpMo3 = If(b.CarrAdHocBudExpMo3.HasValue, b.CarrAdHocBudExpMo3.Value, 0) _
                        , .CarrAdHocBudExpMo4 = If(b.CarrAdHocBudExpMo4.HasValue, b.CarrAdHocBudExpMo4.Value, 0) _
                        , .CarrAdHocBudExpMo5 = If(b.CarrAdHocBudExpMo5.HasValue, b.CarrAdHocBudExpMo5.Value, 0) _
                        , .CarrAdHocBudExpMo6 = If(b.CarrAdHocBudExpMo6.HasValue, b.CarrAdHocBudExpMo6.Value, 0) _
                        , .CarrAdHocBudExpMo7 = If(b.CarrAdHocBudExpMo7.HasValue, b.CarrAdHocBudExpMo7.Value, 0) _
                        , .CarrAdHocBudExpMo8 = If(b.CarrAdHocBudExpMo8.HasValue, b.CarrAdHocBudExpMo8.Value, 0) _
                        , .CarrAdHocBudExpMo9 = If(b.CarrAdHocBudExpMo9.HasValue, b.CarrAdHocBudExpMo9.Value, 0) _
                        , .CarrAdHocBudExpMo10 = If(b.CarrAdHocBudExpMo10.HasValue, b.CarrAdHocBudExpMo10.Value, 0) _
                        , .CarrAdHocBudExpMo11 = If(b.CarrAdHocBudExpMo11.HasValue, b.CarrAdHocBudExpMo11.Value, 0) _
                        , .CarrAdHocBudExpMo12 = If(b.CarrAdHocBudExpMo12.HasValue, b.CarrAdHocBudExpMo12.Value, 0) _
                        , .CarrAdHocBudExpTotal = If(b.CarrAdHocBudExpTotal.HasValue, b.CarrAdHocBudExpTotal.Value, 0) _
                        , .CarrAdHocBudActMo1 = If(b.CarrAdHocBudActMo1.HasValue, b.CarrAdHocBudActMo1.Value, 0) _
                        , .CarrAdHocBudActMo2 = If(b.CarrAdHocBudActMo2.HasValue, b.CarrAdHocBudActMo2.Value, 0) _
                        , .CarrAdHocBudActMo3 = If(b.CarrAdHocBudActMo3.HasValue, b.CarrAdHocBudActMo3.Value, 0) _
                        , .CarrAdHocBudActMo4 = If(b.CarrAdHocBudActMo4.HasValue, b.CarrAdHocBudActMo4.Value, 0) _
                        , .CarrAdHocBudActMo5 = If(b.CarrAdHocBudActMo5.HasValue, b.CarrAdHocBudActMo5.Value, 0) _
                        , .CarrAdHocBudActMo6 = If(b.CarrAdHocBudActMo6.HasValue, b.CarrAdHocBudActMo6.Value, 0) _
                        , .CarrAdHocBudActMo7 = If(b.CarrAdHocBudActMo7.HasValue, b.CarrAdHocBudActMo7.Value, 0) _
                        , .CarrAdHocBudActMo8 = If(b.CarrAdHocBudActMo8.HasValue, b.CarrAdHocBudActMo8.Value, 0) _
                        , .CarrAdHocBudActMo9 = If(b.CarrAdHocBudActMo9.HasValue, b.CarrAdHocBudActMo9.Value, 0) _
                        , .CarrAdHocBudActMo10 = If(b.CarrAdHocBudActMo10.HasValue, b.CarrAdHocBudActMo10.Value, 0) _
                        , .CarrAdHocBudActMo11 = If(b.CarrAdHocBudActMo11.HasValue, b.CarrAdHocBudActMo11.Value, 0) _
                        , .CarrAdHocBudActMo12 = If(b.CarrAdHocBudActMo12.HasValue, b.CarrAdHocBudActMo12.Value, 0) _
                        , .CarrAdHocBudActTotal = If(b.CarrAdHocBudActTotal.HasValue, b.CarrAdHocBudActTotal.Value, 0) _
                        , .CarrAdHocBudgetUpdated = b.CarrAdHocBudgetUpdated.ToArray()}).ToList()}).First

                Return CarrAdHoc

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

    ''' <summary>
    ''' The CarrAdHocActiveFlag is used to determine which records to return 
    ''' 0 = all records
    ''' 1 = only records where CarrAdHocActive is true
    ''' Any other value returns records where CarrAdHocActive is False
    ''' </summary>
    ''' <param name="CarrAdHocActiveFlag"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 

        Public Function GetCarrAdHocsFiltered(Optional ByVal CarrAdHocActiveFlag As Integer = 0) As DataTransferObjects.CarrAdHoc()
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Return all the contacts that match the criteria sorted by name
                Dim CarrAdHocs() As DataTransferObjects.CarrAdHoc = (
                        From t In db.CarrAdHocs
                        Where
                        t.CarrAdHocActive = If(CarrAdHocActiveFlag = 0, t.CarrAdHocActive, If(CarrAdHocActiveFlag = 1, True, False))
                        Order By t.CarrAdHocControl
                        Select New DataTransferObjects.CarrAdHoc With {.CarrAdHocControl = t.CarrAdHocControl _
                        , .CarrAdHocNumber = If(t.CarrAdHocNumber.HasValue, t.CarrAdHocNumber.Value, 0) _
                        , .CarrAdHocName = t.CarrAdHocName _
                        , .CarrAdHocStreetAddress1 = t.CarrAdHocStreetAddress1 _
                        , .CarrAdHocStreetAddress2 = t.CarrAdHocStreetAddress2 _
                        , .CarrAdHocStreetAddress3 = t.CarrAdHocStreetAddress3 _
                        , .CarrAdHocStreetCity = t.CarrAdHocStreetCity _
                        , .CarrAdHocStreetState = t.CarrAdHocStreetState _
                        , .CarrAdHocStreetCountry = t.CarrAdHocStreetCountry _
                        , .CarrAdHocStreetZip = t.CarrAdHocStreetZip _
                        , .CarrAdHocMailAddress1 = t.CarrAdHocMailAddress1 _
                        , .CarrAdHocMailAddress2 = t.CarrAdHocMailAddress2 _
                        , .CarrAdHocMailAddress3 = t.CarrAdHocMailAddress3 _
                        , .CarrAdHocMailCity = t.CarrAdHocMailCity _
                        , .CarrAdHocMailState = t.CarrAdHocMailState _
                        , .CarrAdHocMailCountry = t.CarrAdHocMailCountry _
                        , .CarrAdHocMailZip = t.CarrAdHocMailZip _
                        , .CarrAdHocModDate = t.CarrAdHocModDate _
                        , .CarrAdHocModUser = t.CarrAdHocModUser _
                        , .CarrAdHocSCAC = t.CarrAdHocSCAC _
                        , .CarrAdHocAccountNo = t.CarrAdHocAccountNo _
                        , .CarrAdHocTypeCode = t.CarrAdHocTypeCode _
                        , .CarrAdHocGeneralInfo = t.CarrAdHocGeneralInfo _
                        , .CarrAdHocActive = If(t.CarrAdHocActive.HasValue, t.CarrAdHocActive.Value, False) _
                        , .CarrAdHocWebSite = t.CarrAdHocWebSite _
                        , .CarrAdHocEmail = t.CarrAdHocEmail _
                        , .CarrAdHocNEXTStopAcct = t.CarrAdHocNEXTStopAcct _
                        , .CarrAdHocNEXTStopPwd = t.CarrAdHocNEXTStopPwd _
                        , .CarrAdHocGradReliability = If(t.CarrAdHocGradReliability.HasValue, t.CarrAdHocGradReliability.Value, 0) _
                        , .CarrAdHocGradBillingAccuracy = If(t.CarrAdHocGradBillingAccuracy.HasValue, t.CarrAdHocGradBillingAccuracy.Value, 0) _
                        , .CarrAdHocGradFinancialStrength = If(t.CarrAdHocGradFinancialStrength.HasValue, t.CarrAdHocGradFinancialStrength.Value, 0) _
                        , .CarrAdHocGradEquipmentCondition = If(t.CarrAdHocGradEquipmentCondition.HasValue, t.CarrAdHocGradEquipmentCondition.Value, 0) _
                        , .CarrAdHocGradContactAttitude = If(t.CarrAdHocGradContactAttitude.HasValue, t.CarrAdHocGradContactAttitude.Value, 0) _
                        , .CarrAdHocGradDriverAttitude = If(t.CarrAdHocGradDriverAttitude.HasValue, t.CarrAdHocGradDriverAttitude.Value, 0) _
                        , .CarrAdHocGradClaimFrequency = If(t.CarrAdHocGradClaimFrequency.HasValue, t.CarrAdHocGradClaimFrequency.Value, 0) _
                        , .CarrAdHocGradClaimPayment = If(t.CarrAdHocGradClaimPayment.HasValue, t.CarrAdHocGradClaimPayment.Value, 0) _
                        , .CarrAdHocGradGeographicCoverage = If(t.CarrAdHocGradGeographicCoverage.HasValue, t.CarrAdHocGradGeographicCoverage.Value, 0) _
                        , .CarrAdHocGradCustomerService = If(t.CarrAdHocGradCustomerService.HasValue, t.CarrAdHocGradCustomerService.Value, 0) _
                        , .CarrAdHocGradPriceChangeNotification = If(t.CarrAdHocGradPriceChangeNotification.HasValue, t.CarrAdHocGradPriceChangeNotification.Value, 0) _
                        , .CarrAdHocGradPriceChangeFrequency = If(t.CarrAdHocGradPriceChangeFrequency.HasValue, t.CarrAdHocGradPriceChangeFrequency.Value, 0) _
                        , .CarrAdHocGradPriceAggressiveness = If(t.CarrAdHocGradPriceAggressiveness.HasValue, t.CarrAdHocGradPriceAggressiveness.Value, 0) _
                        , .CarrAdHocGradAverage = If(t.CarrAdHocGradAverage.HasValue, t.CarrAdHocGradAverage.Value, 0) _
                        , .CarrAdHocQualInsuranceDate = t.CarrAdHocQualInsuranceDate _
                        , .CarrAdHocQualQualified = If(t.CarrAdHocQualQualified.HasValue, t.CarrAdHocQualQualified.Value, False) _
                        , .CarrAdHocQualAuthority = t.CarrAdHocQualAuthority _
                        , .CarrAdHocQualContract = If(t.CarrAdHocQualContract.HasValue, t.CarrAdHocQualContract.Value, False) _
                        , .CarrAdHocQualSignedDate = t.CarrAdHocQualSignedDate _
                        , .CarrAdHocQualContractExpiresDate = t.CarrAdHocQualContractExpiresDate _
                        , .CarrAdHocQualMaxPerShipment = If(t.CarrAdHocQualMaxPerShipment.HasValue, t.CarrAdHocQualMaxPerShipment.Value, 0) _
                        , .CarrAdHocQualMaxAllShipments = If(t.CarrAdHocQualMaxAllShipments.HasValue, t.CarrAdHocQualMaxAllShipments.Value, 0) _
                        , .CarrAdHocQualCurAllExposure = If(t.CarrAdHocQualCurAllExposure.HasValue, t.CarrAdHocQualCurAllExposure.Value, 0) _
                        , .CarrAdHocCodeVal1 = If(t.CarrAdHocCodeVal1.HasValue, t.CarrAdHocCodeVal1.Value, 0) _
                        , .CarrAdHocCodeVal2 = If(t.CarrAdHocCodeVal2.HasValue, t.CarrAdHocCodeVal2.Value, 0) _
                        , .CarrAdHocTruckDefault = If(t.CarrAdHocTruckDefault.HasValue, t.CarrAdHocTruckDefault.Value, 0) _
                        , .CarrAdHocAllowWebTender = If(t.CarrAdHocAllowWebTender.HasValue, t.CarrAdHocAllowWebTender.Value, False) _
                        , .CarrAdHocIgnoreTariff = t.CarrAdHocIgnoreTariff _
                        , .CarrAdHocSmartWayPartnerType = t.CarrAdHocSmartWayPartnerType _
                        , .CarrAdHocSmartWayScore = If(t.CarrAdHocSmartWayScore.HasValue, t.CarrAdHocSmartWayScore.Value, 0) _
                        , .CarrAdHocSmartWayPartner = t.CarrAdHocSmartWayPartner _
                        , .CarrAdHocUpdated = t.CarrAdHocUpdated.ToArray()}).ToArray()
                Return CarrAdHocs

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

    Public Function GetCarrAdHocAllowWebTender(ByVal CarrAdHocControl As Integer) As Boolean
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                'Get the newest record that matches the provided criteria
                Dim CarrAdHoc As DataTransferObjects.CarrAdHoc = (
                        From d In db.CarrAdHocs
                        Where
                        d.CarrAdHocControl = CarrAdHocControl
                        Select New DataTransferObjects.CarrAdHoc With {.CarrAdHocControl = d.CarrAdHocControl, .CarrAdHocAllowWebTender = If(d.CarrAdHocAllowWebTender.HasValue, d.CarrAdHocAllowWebTender.Value, False)}).First
                'If we get here then a record exists for the desired action so return true
                Return CarrAdHoc.CarrAdHocAllowWebTender
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

#End Region

#Region "Protected Functions"

    Protected Overrides Function CopyDTOToLinq(ByVal oData As DataTransferObjects.DTOBaseClass) As Object
        Dim oCarrAdHoc = CType(oData, DataTransferObjects.CarrAdHoc)
        'Create New Record
        Return New LTS.CarrAdHoc With {.CarrAdHocControl = oCarrAdHoc.CarrAdHocControl _
            , .CarrAdHocNumber = oCarrAdHoc.CarrAdHocNumber _
            , .CarrAdHocName = oCarrAdHoc.CarrAdHocName _
            , .CarrAdHocStreetAddress1 = oCarrAdHoc.CarrAdHocStreetAddress1 _
            , .CarrAdHocStreetAddress2 = oCarrAdHoc.CarrAdHocStreetAddress2 _
            , .CarrAdHocStreetAddress3 = oCarrAdHoc.CarrAdHocStreetAddress3 _
            , .CarrAdHocStreetCity = oCarrAdHoc.CarrAdHocStreetCity _
            , .CarrAdHocStreetState = oCarrAdHoc.CarrAdHocStreetState _
            , .CarrAdHocStreetCountry = oCarrAdHoc.CarrAdHocStreetCountry _
            , .CarrAdHocStreetZip = oCarrAdHoc.CarrAdHocStreetZip _
            , .CarrAdHocMailAddress1 = oCarrAdHoc.CarrAdHocMailAddress1 _
            , .CarrAdHocMailAddress2 = oCarrAdHoc.CarrAdHocMailAddress2 _
            , .CarrAdHocMailAddress3 = oCarrAdHoc.CarrAdHocMailAddress3 _
            , .CarrAdHocMailCity = oCarrAdHoc.CarrAdHocMailCity _
            , .CarrAdHocMailState = oCarrAdHoc.CarrAdHocMailState _
            , .CarrAdHocMailCountry = oCarrAdHoc.CarrAdHocMailCountry _
            , .CarrAdHocMailZip = oCarrAdHoc.CarrAdHocMailZip _
            , .CarrAdHocModDate = Date.Now _
            , .CarrAdHocModUser = Parameters.UserName _
            , .CarrAdHocSCAC = oCarrAdHoc.CarrAdHocSCAC _
            , .CarrAdHocAccountNo = oCarrAdHoc.CarrAdHocAccountNo _
            , .CarrAdHocTypeCode = oCarrAdHoc.CarrAdHocTypeCode _
            , .CarrAdHocGeneralInfo = oCarrAdHoc.CarrAdHocGeneralInfo _
            , .CarrAdHocActive = oCarrAdHoc.CarrAdHocActive _
            , .CarrAdHocWebSite = oCarrAdHoc.CarrAdHocWebSite _
            , .CarrAdHocEmail = oCarrAdHoc.CarrAdHocEmail _
            , .CarrAdHocNEXTStopAcct = oCarrAdHoc.CarrAdHocNEXTStopAcct _
            , .CarrAdHocNEXTStopPwd = oCarrAdHoc.CarrAdHocNEXTStopPwd _
            , .CarrAdHocGradReliability = oCarrAdHoc.CarrAdHocGradReliability _
            , .CarrAdHocGradBillingAccuracy = oCarrAdHoc.CarrAdHocGradBillingAccuracy _
            , .CarrAdHocGradFinancialStrength = oCarrAdHoc.CarrAdHocGradFinancialStrength _
            , .CarrAdHocGradEquipmentCondition = oCarrAdHoc.CarrAdHocGradEquipmentCondition _
            , .CarrAdHocGradContactAttitude = oCarrAdHoc.CarrAdHocGradContactAttitude _
            , .CarrAdHocGradDriverAttitude = oCarrAdHoc.CarrAdHocGradDriverAttitude _
            , .CarrAdHocGradClaimFrequency = oCarrAdHoc.CarrAdHocGradClaimFrequency _
            , .CarrAdHocGradClaimPayment = oCarrAdHoc.CarrAdHocGradClaimPayment _
            , .CarrAdHocGradGeographicCoverage = oCarrAdHoc.CarrAdHocGradGeographicCoverage _
            , .CarrAdHocGradCustomerService = oCarrAdHoc.CarrAdHocGradCustomerService _
            , .CarrAdHocGradPriceChangeNotification = oCarrAdHoc.CarrAdHocGradPriceChangeNotification _
            , .CarrAdHocGradPriceChangeFrequency = oCarrAdHoc.CarrAdHocGradPriceChangeFrequency _
            , .CarrAdHocGradPriceAggressiveness = oCarrAdHoc.CarrAdHocGradPriceAggressiveness _
            , .CarrAdHocGradAverage = oCarrAdHoc.CarrAdHocGradAverage _
            , .CarrAdHocQualInsuranceDate = oCarrAdHoc.CarrAdHocQualInsuranceDate _
            , .CarrAdHocQualQualified = oCarrAdHoc.CarrAdHocQualQualified _
            , .CarrAdHocQualAuthority = oCarrAdHoc.CarrAdHocQualAuthority _
            , .CarrAdHocQualContract = oCarrAdHoc.CarrAdHocQualContract _
            , .CarrAdHocQualSignedDate = oCarrAdHoc.CarrAdHocQualSignedDate _
            , .CarrAdHocQualContractExpiresDate = oCarrAdHoc.CarrAdHocQualContractExpiresDate _
            , .CarrAdHocQualMaxPerShipment = oCarrAdHoc.CarrAdHocQualMaxPerShipment _
            , .CarrAdHocQualMaxAllShipments = oCarrAdHoc.CarrAdHocQualMaxAllShipments _
            , .CarrAdHocQualCurAllExposure = oCarrAdHoc.CarrAdHocQualCurAllExposure _
            , .CarrAdHocCodeVal1 = oCarrAdHoc.CarrAdHocCodeVal1 _
            , .CarrAdHocCodeVal2 = oCarrAdHoc.CarrAdHocCodeVal2 _
            , .CarrAdHocTruckDefault = oCarrAdHoc.CarrAdHocTruckDefault _
            , .CarrAdHocAllowWebTender = oCarrAdHoc.CarrAdHocAllowWebTender _
            , .CarrAdHocIgnoreTariff = oCarrAdHoc.CarrAdHocIgnoreTariff _
            , .CarrAdHocSmartWayPartnerType = oCarrAdHoc.CarrAdHocSmartWayPartnerType _
            , .CarrAdHocSmartWayScore = oCarrAdHoc.CarrAdHocSmartWayScore _
            , .CarrAdHocSmartWayPartner = oCarrAdHoc.CarrAdHocSmartWayPartner _
            , .CarrAdHocUpdated = If(oCarrAdHoc.CarrAdHocUpdated Is Nothing, New Byte() {}, oCarrAdHoc.CarrAdHocUpdated)}
    End Function

    Protected Overrides Function GetDTOUsingLinqTable(ByVal LinqTable As Object) As DataTransferObjects.DTOBaseClass
        Return GetCarrAdHocFiltered(Control:=CType(LinqTable, LTS.CarrAdHoc).CarrAdHocControl)
    End Function

    Protected Overrides Function GetQuickSaveResults(ByVal LinqTable As Object) As DataTransferObjects.QuickSaveResults
        Dim ret As DataTransferObjects.QuickSaveResults
        Using db As New NGLMASCarrierDataContext(ConnectionString)
            Try
                Dim source As LTS.CarrAdHoc = TryCast(LinqTable, LTS.CarrAdHoc)
                If source Is Nothing Then Return Nothing

                ret = (From d In db.CarrAdHocs
                    Where d.CarrAdHocControl = source.CarrAdHocControl
                    Select New DataTransferObjects.QuickSaveResults With {.Control = d.CarrAdHocControl _
                        , .ModDate = d.CarrAdHocModDate _
                        , .ModUser = d.CarrAdHocModUser _
                        , .Updated = d.CarrAdHocUpdated.ToArray}).First

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

    Protected Overrides Sub ValidateNewRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        'Check if the data already exists only one allowed
        With CType(oData, DataTransferObjects.CarrAdHoc)
            Try
                'check for required fields
                If .CarrAdHocNumber = 0 Then
                    Utilities.SaveAppError("Cannot save new CarrAdHoc data.  The CarrAdHoc number, " & .CarrAdHocNumber & " cannot be zero", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                End If

                If String.IsNullOrEmpty(.CarrAdHocName) OrElse .CarrAdHocName.Trim.Length < 1 Then
                    Utilities.SaveAppError("Cannot save new CarrAdHoc data.  The CarrAdHoc name, " & .CarrAdHocName & " is not valid.", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                End If

                Dim intCodeVal1 As Integer = 0
                Dim intCodeVal2 As Integer = 0
                If Not getNewCarrAdHocCodeValues(intCodeVal1, intCodeVal2) Then
                    Utilities.SaveAppError("Cannot save new CarrAdHoc data: unable to get new CarrAdHoc Code Values.", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_GetCarrAdHocCodeValFailure"}, New FaultReason("E_DataValidationFailure"))
                End If
                .CarrAdHocCodeVal1 = intCodeVal1
                .CarrAdHocCodeVal2 = intCodeVal2
                Dim CarrAdHoc As DataTransferObjects.CarrAdHoc = (
                        From t In CType(oDB, NGLMASCarrierDataContext).CarrAdHocs
                        Where
                        (t.CarrAdHocName = .CarrAdHocName _
                         Or
                         t.CarrAdHocNumber = .CarrAdHocNumber)
                        Select New DataTransferObjects.CarrAdHoc With {.CarrAdHocControl = t.CarrAdHocControl}).First

                If Not CarrAdHoc Is Nothing Then
                    Utilities.SaveAppError("Cannot save new CarrAdHoc data.  The CarrAdHoc number, " & .CarrAdHocNumber & " or the CarrAdHoc name, " & .CarrAdHocName & ",  already exist.", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                End If

            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub

    Protected Overrides Sub ValidateUpdatedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        'Check if the data already exists only one allowed
        With CType(oData, DataTransferObjects.CarrAdHoc)
            Try
                'check for required fields
                If .CarrAdHocNumber = 0 Then
                    Utilities.SaveAppError("Cannot save CarrAdHoc changes.  The CarrAdHoc number, " & .CarrAdHocNumber & " cannot be zero", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                End If

                If String.IsNullOrEmpty(.CarrAdHocName) OrElse .CarrAdHocName.Trim.Length < 1 Then
                    Utilities.SaveAppError("Cannot save CarrAdHoc changes.  The CarrAdHoc name, " & .CarrAdHocName & " is not valid.", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                End If


                'Get the newest record that matches the provided criteria
                Dim CarrAdHoc As DataTransferObjects.CarrAdHoc = (
                        From t In CType(oDB, NGLMASCarrierDataContext).CarrAdHocs
                        Where
                        (t.CarrAdHocControl <> .CarrAdHocControl) _
                        And
                        (t.CarrAdHocNumber = .CarrAdHocNumber)
                        Select New DataTransferObjects.CarrAdHoc With {.CarrAdHocControl = t.CarrAdHocControl}).First

                If Not CarrAdHoc Is Nothing Then
                    Utilities.SaveAppError("Cannot save CarrAdHoc changes.  The CarrAdHoc number, " & .CarrAdHocNumber & ",  already exist.", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_InvalidKeyField"}, New FaultReason("E_DataValidationFailure"))
                End If

            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub

    Protected Overrides Sub ValidateDeletedRecord(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        'Check if the CarrAdHoc is being used by the book data or the lane data
        With CType(oData, DataTransferObjects.CarrAdHoc)
            Try
                'Add code here to call the Book and Lane data providers when they are created
                Dim dpBook As New NGLBookData(Me.Parameters)
                Dim dpLane As New NGLLaneData(Me.Parameters)
                Dim oBooks() As DataTransferObjects.Book
                Dim oLanes() As DataTransferObjects.Lane
                Try
                    oBooks = dpBook.GetBooksByCarrier(.CarrAdHocControl)
                Catch ex As FaultException
                    If ex.Message <> "E_NoData" Then
                        Throw
                    End If
                End Try
                Try
                    oLanes = dpLane.GetLanesByCarrier(.CarrAdHocControl)
                Catch ex As FaultException
                    If ex.Message <> "E_NoData" Then
                        Throw
                    End If
                End Try
                If (Not oBooks Is Nothing AndAlso oBooks.Length > 0) OrElse (Not oLanes Is Nothing AndAlso oLanes.Length > 0) Then
                    Utilities.SaveAppError("Cannot delete CarrAdHoc data.  The CarrAdHoc number, " & .CarrAdHocNumber & " is being used and cannot be deleted. check the book or lane information.", Me.Parameters)
                    Throw New FaultException(Of SqlFaultInfo)(New SqlFaultInfo With {.Message = "E_DataInUse"}, New FaultReason("E_DataValidationFailure"))
                End If
            Catch ex As FaultException
                Throw
            Catch ex As InvalidOperationException
                'do nothing this is the desired result.
            End Try
        End With
    End Sub

    Protected Overrides Sub AddDetailsToLinq(ByRef LinqTable As Object, ByRef oData As DataTransferObjects.DTOBaseClass)

        With CType(LinqTable, LTS.CarrAdHoc)
            'Add CarrAdHoc contact Records
            .CarrAdHocConts.AddRange(
                From d In CType(oData, DataTransferObjects.CarrAdHoc).CarrAdHocConts
                                        Select New LTS.CarrAdHocCont With {.CarrAdHocContControl = d.CarrAdHocContControl,
                                        .CarrAdHocContCarrAdHocControl = d.CarrAdHocContCarrAdHocControl,
                                        .CarrAdHocContName = d.CarrAdHocContName,
                                        .CarrAdHocContTitle = d.CarrAdHocContTitle,
                                        .CarrAdHocContactPhone = d.CarrAdHocContactPhone,
                                        .CarrAdHocContPhoneExt = d.CarrAdHocContPhoneExt,
                                        .CarrAdHocContactFax = d.CarrAdHocContactFax,
                                        .CarrAdHocContact800 = d.CarrAdHocContact800,
                                        .CarrAdHocContactEMail = d.CarrAdHocContactEMail,
                                        .CarrAdHocContUpdated = If(d.CarrAdHocContUpdated Is Nothing, New Byte() {}, d.CarrAdHocContUpdated)})
            'Add CarrAdHoc budget Records
            .CarrAdHocBudgets.AddRange(
                From d In CType(oData, DataTransferObjects.CarrAdHoc).CarrAdHocBudgets
                                          Select New LTS.CarrAdHocBudget With {.CarrAdHocBudControl = d.CarrAdHocBudControl,
                                          .CarrAdHocBudCarrAdHocControl = d.CarrAdHocBudCarrAdHocControl,
                                          .CarrAdHocBudModDate = Date.Now,
                                          .CarrAdHocBudModUser = Parameters.UserName,
                                          .CarrAdHocBudExpMo1 = d.CarrAdHocBudExpMo1,
                                          .CarrAdHocBudExpMo2 = d.CarrAdHocBudExpMo2,
                                          .CarrAdHocBudExpMo3 = d.CarrAdHocBudExpMo3,
                                          .CarrAdHocBudExpMo4 = d.CarrAdHocBudExpMo4,
                                          .CarrAdHocBudExpMo5 = d.CarrAdHocBudExpMo5,
                                          .CarrAdHocBudExpMo6 = d.CarrAdHocBudExpMo6,
                                          .CarrAdHocBudExpMo7 = d.CarrAdHocBudExpMo7,
                                          .CarrAdHocBudExpMo8 = d.CarrAdHocBudExpMo8,
                                          .CarrAdHocBudExpMo9 = d.CarrAdHocBudExpMo9,
                                          .CarrAdHocBudExpMo10 = d.CarrAdHocBudExpMo10,
                                          .CarrAdHocBudExpMo11 = d.CarrAdHocBudExpMo11,
                                          .CarrAdHocBudExpMo12 = d.CarrAdHocBudExpMo12,
                                          .CarrAdHocBudExpTotal = d.CarrAdHocBudExpTotal,
                                          .CarrAdHocBudActMo1 = d.CarrAdHocBudActMo1,
                                          .CarrAdHocBudActMo2 = d.CarrAdHocBudActMo2,
                                          .CarrAdHocBudActMo3 = d.CarrAdHocBudActMo3,
                                          .CarrAdHocBudActMo4 = d.CarrAdHocBudActMo4,
                                          .CarrAdHocBudActMo5 = d.CarrAdHocBudActMo5,
                                          .CarrAdHocBudActMo6 = d.CarrAdHocBudActMo6,
                                          .CarrAdHocBudActMo7 = d.CarrAdHocBudActMo7,
                                          .CarrAdHocBudActMo8 = d.CarrAdHocBudActMo8,
                                          .CarrAdHocBudActMo9 = d.CarrAdHocBudActMo9,
                                          .CarrAdHocBudActMo10 = d.CarrAdHocBudActMo10,
                                          .CarrAdHocBudActMo11 = d.CarrAdHocBudActMo11,
                                          .CarrAdHocBudActMo12 = d.CarrAdHocBudActMo12,
                                          .CarrAdHocBudActTotal = d.CarrAdHocBudActTotal,
                                          .CarrAdHocBudgetUpdated = If(d.CarrAdHocBudgetUpdated Is Nothing, New Byte() {}, d.CarrAdHocBudgetUpdated)})

        End With
    End Sub

    Protected Overrides Sub InsertAllDetails(ByRef oDB As System.Data.Linq.DataContext, ByRef LinqTable As Object)
        With CType(oDB, NGLMASCarrierDataContext)
            .CarrAdHocConts.InsertAllOnSubmit(CType(LinqTable, LTS.CarrAdHoc).CarrAdHocConts)
            .CarrAdHocBudgets.InsertAllOnSubmit(CType(LinqTable, LTS.CarrAdHoc).CarrAdHocBudgets)

        End With

    End Sub

    Protected Overrides Sub ProcessUpdatedDetails(ByRef oDB As System.Data.Linq.DataContext, ByRef oData As DataTransferObjects.DTOBaseClass)
        With CType(oDB, NGLMASCarrierDataContext)
            ' Process any inserted contact records 
            .CarrAdHocConts.InsertAllOnSubmit(GetCarrAdHocContChanges(oData, TrackingInfo.Created))
            ' Process any updated contact records
            .CarrAdHocConts.AttachAll(GetCarrAdHocContChanges(oData, TrackingInfo.Updated), True)
            ' Process any deleted contact records
            Dim deletedContDetails = GetCarrAdHocContChanges(oData, TrackingInfo.Deleted)
            .CarrAdHocConts.AttachAll(deletedContDetails, True)
            .CarrAdHocConts.DeleteAllOnSubmit(deletedContDetails)
            ' Process any inserted Budget records 
            .CarrAdHocBudgets.InsertAllOnSubmit(GetCarrAdHocBudgetChanges(oData, TrackingInfo.Created))
            ' Process any updated Budget records
            .CarrAdHocBudgets.AttachAll(GetCarrAdHocBudgetChanges(oData, TrackingInfo.Updated), True)
            ' Process any deleted Budget records
            Dim deletedBudDetails = GetCarrAdHocBudgetChanges(oData, TrackingInfo.Deleted)
            .CarrAdHocBudgets.AttachAll(deletedBudDetails, True)
            .CarrAdHocBudgets.DeleteAllOnSubmit(deletedBudDetails)


        End With
    End Sub

    Protected Function GetCarrAdHocContChanges(ByVal source As DataTransferObjects.CarrAdHoc, ByVal changeType As TrackingInfo) As List(Of LTS.CarrAdHocCont)
        ' Test record details for specified change type.
        ' If Updated is null, set to byte[0] (for inserted items).
        Dim details As IEnumerable(Of LTS.CarrAdHocCont) = (
                From d In source.CarrAdHocConts
                Where d.TrackingState = changeType
                Select New LTS.CarrAdHocCont With {.CarrAdHocContControl = d.CarrAdHocContControl,
                .CarrAdHocContCarrAdHocControl = d.CarrAdHocContCarrAdHocControl,
                .CarrAdHocContName = d.CarrAdHocContName,
                .CarrAdHocContTitle = d.CarrAdHocContTitle,
                .CarrAdHocContactPhone = d.CarrAdHocContactPhone,
                .CarrAdHocContPhoneExt = d.CarrAdHocContPhoneExt,
                .CarrAdHocContactFax = d.CarrAdHocContactFax,
                .CarrAdHocContact800 = d.CarrAdHocContact800,
                .CarrAdHocContactEMail = d.CarrAdHocContactEMail,
                .CarrAdHocContUpdated = If(d.CarrAdHocContUpdated Is Nothing, New Byte() {}, d.CarrAdHocContUpdated)})
        Return details.ToList()
    End Function

    Protected Function GetCarrAdHocBudgetChanges(ByVal source As DataTransferObjects.CarrAdHoc, ByVal changeType As TrackingInfo) As List(Of LTS.CarrAdHocBudget)
        ' Test record details for specified change type.
        ' If Updated is null, set to byte[0] (for inserted items).
        Dim details As IEnumerable(Of LTS.CarrAdHocBudget) = (
                From d In source.CarrAdHocBudgets
                Where d.TrackingState = changeType
                Select New LTS.CarrAdHocBudget With {.CarrAdHocBudControl = d.CarrAdHocBudControl,
                .CarrAdHocBudCarrAdHocControl = d.CarrAdHocBudCarrAdHocControl,
                .CarrAdHocBudModDate = Date.Now,
                .CarrAdHocBudModUser = Parameters.UserName,
                .CarrAdHocBudExpMo1 = d.CarrAdHocBudExpMo1,
                .CarrAdHocBudExpMo2 = d.CarrAdHocBudExpMo2,
                .CarrAdHocBudExpMo3 = d.CarrAdHocBudExpMo3,
                .CarrAdHocBudExpMo4 = d.CarrAdHocBudExpMo4,
                .CarrAdHocBudExpMo5 = d.CarrAdHocBudExpMo5,
                .CarrAdHocBudExpMo6 = d.CarrAdHocBudExpMo6,
                .CarrAdHocBudExpMo7 = d.CarrAdHocBudExpMo7,
                .CarrAdHocBudExpMo8 = d.CarrAdHocBudExpMo8,
                .CarrAdHocBudExpMo9 = d.CarrAdHocBudExpMo9,
                .CarrAdHocBudExpMo10 = d.CarrAdHocBudExpMo10,
                .CarrAdHocBudExpMo11 = d.CarrAdHocBudExpMo11,
                .CarrAdHocBudExpMo12 = d.CarrAdHocBudExpMo12,
                .CarrAdHocBudExpTotal = d.CarrAdHocBudExpTotal,
                .CarrAdHocBudActMo1 = d.CarrAdHocBudActMo1,
                .CarrAdHocBudActMo2 = d.CarrAdHocBudActMo2,
                .CarrAdHocBudActMo3 = d.CarrAdHocBudActMo3,
                .CarrAdHocBudActMo4 = d.CarrAdHocBudActMo4,
                .CarrAdHocBudActMo5 = d.CarrAdHocBudActMo5,
                .CarrAdHocBudActMo6 = d.CarrAdHocBudActMo6,
                .CarrAdHocBudActMo7 = d.CarrAdHocBudActMo7,
                .CarrAdHocBudActMo8 = d.CarrAdHocBudActMo8,
                .CarrAdHocBudActMo9 = d.CarrAdHocBudActMo9,
                .CarrAdHocBudActMo10 = d.CarrAdHocBudActMo10,
                .CarrAdHocBudActMo11 = d.CarrAdHocBudActMo11,
                .CarrAdHocBudActMo12 = d.CarrAdHocBudActMo12,
                .CarrAdHocBudActTotal = d.CarrAdHocBudActTotal,
                .CarrAdHocBudgetUpdated = If(d.CarrAdHocBudgetUpdated Is Nothing, New Byte() {}, d.CarrAdHocBudgetUpdated)})
        Return details.ToList()
    End Function

    Private Function getNewCarrAdHocCodeValues(ByRef intCodeVal1 As Integer, ByRef intCodeVal2 As Integer) As Boolean
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
#End Region

End Class