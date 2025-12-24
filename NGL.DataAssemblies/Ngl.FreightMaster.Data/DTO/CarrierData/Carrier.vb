Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class Carrier
        Inherits DTOBaseClass


#Region " Data Members"

        Private _CarrierControl As Integer = 0
        <DataMember()> _
        Public Property CarrierControl() As Integer
            Get
                Return _CarrierControl
            End Get
            Set(ByVal value As Integer)
                _CarrierControl = value
            End Set
        End Property

        Private _CarrierNumber As Integer = 0
        <DataMember()> _
        Public Property CarrierNumber() As Integer
            Get
                Return _CarrierNumber
            End Get
            Set(ByVal value As Integer)
                _CarrierNumber = value
            End Set
        End Property

        Private _CarrierName As String = ""
        <DataMember()> _
        Public Property CarrierName() As String
            Get
                Return Left(_CarrierName, 40)
            End Get
            Set(ByVal value As String)
                _CarrierName = Left(value, 40)
            End Set
        End Property

        Private _CarrierStreetAddress1 As String = ""
        <DataMember()> _
        Public Property CarrierStreetAddress1() As String
            Get
                Return Left(_CarrierStreetAddress1, 40)
            End Get
            Set(ByVal value As String)
                _CarrierStreetAddress1 = Left(value, 40)
            End Set
        End Property

        Private _CarrierStreetAddress2 As String = ""
        <DataMember()> _
        Public Property CarrierStreetAddress2() As String
            Get
                Return Left(_CarrierStreetAddress2, 40)
            End Get
            Set(ByVal value As String)
                _CarrierStreetAddress2 = Left(value, 40)
            End Set
        End Property

        Private _CarrierStreetAddress3 As String = ""
        <DataMember()> _
        Public Property CarrierStreetAddress3() As String
            Get
                Return Left(_CarrierStreetAddress3, 40)
            End Get
            Set(ByVal value As String)
                _CarrierStreetAddress3 = Left(value, 40)
            End Set
        End Property

        Private _CarrierStreetCity As String = ""
        <DataMember()> _
        Public Property CarrierStreetCity() As String
            Get
                Return Left(_CarrierStreetCity, 25)
            End Get
            Set(ByVal value As String)
                _CarrierStreetCity = Left(value, 25)
            End Set
        End Property

        Private _CarrierStreetState As String = ""
        <DataMember()> _
        Public Property CarrierStreetState() As String
            Get
                Return Left(_CarrierStreetState, 8)
            End Get
            Set(ByVal value As String)
                _CarrierStreetState = Left(value, 8)
            End Set
        End Property

        Private _CarrierStreetCountry As String = ""
        <DataMember()> _
        Public Property CarrierStreetCountry() As String
            Get
                Return Left(_CarrierStreetCountry, 30)
            End Get
            Set(ByVal value As String)
                _CarrierStreetCountry = Left(value, 30)
            End Set
        End Property

        Private _CarrierStreetZip As String = ""
        <DataMember()> _
        Public Property CarrierStreetZip() As String
            Get
                Return Left(_CarrierStreetZip, 50)
            End Get
            Set(ByVal value As String)
                _CarrierStreetZip = Left(value, 50)
            End Set
        End Property

        Private _CarrierMailAddress1 As String = ""
        <DataMember()> _
        Public Property CarrierMailAddress1() As String
            Get
                Return Left(_CarrierMailAddress1, 40)
            End Get
            Set(ByVal value As String)
                _CarrierMailAddress1 = Left(value, 40)
            End Set
        End Property

        Private _CarrierMailAddress2 As String = ""
        <DataMember()> _
        Public Property CarrierMailAddress2() As String
            Get
                Return Left(_CarrierMailAddress2, 40)
            End Get
            Set(ByVal value As String)
                _CarrierMailAddress2 = Left(value, 40)
            End Set
        End Property

        Private _CarrierMailAddress3 As String = ""
        <DataMember()> _
        Public Property CarrierMailAddress3() As String
            Get
                Return Left(_CarrierMailAddress3, 40)
            End Get
            Set(ByVal value As String)
                _CarrierMailAddress3 = Left(value, 40)
            End Set
        End Property

        Private _CarrierMailCity As String = ""
        <DataMember()> _
        Public Property CarrierMailCity() As String
            Get
                Return Left(_CarrierMailCity, 25)
            End Get
            Set(ByVal value As String)
                _CarrierMailCity = Left(value, 25)
            End Set
        End Property

        Private _CarrierMailState As String = ""
        <DataMember()> _
        Public Property CarrierMailState() As String
            Get
                Return Left(_CarrierMailState, 8)
            End Get
            Set(ByVal value As String)
                _CarrierMailState = Left(value, 8)
            End Set
        End Property

        Private _CarrierMailCountry As String = ""
        <DataMember()> _
        Public Property CarrierMailCountry() As String
            Get
                Return Left(_CarrierMailCountry, 30)
            End Get
            Set(ByVal value As String)
                _CarrierMailCountry = Left(value, 30)
            End Set
        End Property

        Private _CarrierMailZip As String = ""
        <DataMember()> _
        Public Property CarrierMailZip() As String
            Get
                Return Left(_CarrierMailZip, 20) ' Modified by RHR for v-8.4.003 on 06/25/2021
            End Get
            Set(ByVal value As String)
                _CarrierMailZip = Left(value, 20) ' Modified by RHR for v-8.4.003 on 06/25/2021
            End Set
        End Property

        Private _CarrierModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CarrierModDate() As System.Nullable(Of Date)
            Get
                Return _CarrierModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _CarrierModDate = value
            End Set
        End Property

        Private _CarrierModUser As String = ""
        <DataMember()> _
        Public Property CarrierModUser() As String
            Get
                Return Left(_CarrierModUser, 100)
            End Get
            Set(ByVal value As String)
                _CarrierModUser = Left(value, 100)
            End Set
        End Property

        Private _CarrierSCAC As String = ""
        <DataMember()> _
        Public Property CarrierSCAC() As String
            Get
                Return Left(_CarrierSCAC, 4)
            End Get
            Set(ByVal value As String)
                _CarrierSCAC = Left(value, 4)
            End Set
        End Property

        Private _CarrierAccountNo As String = ""
        <DataMember()> _
        Public Property CarrierAccountNo() As String
            Get
                Return Left(_CarrierAccountNo, 30)
            End Get
            Set(ByVal value As String)
                _CarrierAccountNo = Left(value, 30)
            End Set
        End Property

        Private _CarrierTypeCode As String = ""
        <DataMember()> _
        Public Property CarrierTypeCode() As String
            Get
                Return Left(_CarrierTypeCode, 1)
            End Get
            Set(ByVal value As String)
                _CarrierTypeCode = Left(value, 1)
            End Set
        End Property

        Private _CarrierGeneralInfo As String = ""
        <DataMember()> _
        Public Property CarrierGeneralInfo() As String
            Get
                Return Left(_CarrierGeneralInfo, 255)
            End Get
            Set(ByVal value As String)
                _CarrierGeneralInfo = Left(value, 255)
            End Set
        End Property

        Private _CarrierActive As Boolean = True
        <DataMember()> _
        Public Property CarrierActive() As Boolean
            Get
                Return _CarrierActive
            End Get
            Set(ByVal value As Boolean)
                _CarrierActive = value
            End Set
        End Property

        Private _CarrierWebSite As String = ""
        <DataMember()> _
        Public Property CarrierWebSite() As String
            Get
                Return Left(_CarrierWebSite, 255)
            End Get
            Set(ByVal value As String)
                _CarrierWebSite = Left(value, 255)
            End Set
        End Property

        Private _CarrierEmail As String = ""
        <DataMember()> _
        Public Property CarrierEmail() As String
            Get
                Return Left(_CarrierEmail, 50)
            End Get
            Set(ByVal value As String)
                _CarrierEmail = Left(value, 50)
            End Set
        End Property

        Private _CarrierNEXTStopAcct As String = ""
        <DataMember()> _
        Public Property CarrierNEXTStopAcct() As String
            Get
                Return Left(_CarrierNEXTStopAcct, 50)
            End Get
            Set(ByVal value As String)
                _CarrierNEXTStopAcct = Left(value, 50)
            End Set
        End Property

        Private _CarrierNEXTStopPwd As String = ""
        <DataMember()> _
        Public Property CarrierNEXTStopPwd() As String
            Get
                Return Left(_CarrierNEXTStopPwd, 50)
            End Get
            Set(ByVal value As String)
                _CarrierNEXTStopPwd = Left(value, 50)
            End Set
        End Property


        Private _CarrierGradReliability As Integer = 0
        <DataMember()> _
        Public Property CarrierGradReliability() As Integer
            Get
                Return _CarrierGradReliability
            End Get
            Set(ByVal value As Integer)
                _CarrierGradReliability = value
            End Set
        End Property

        Private _CarrierGradBillingAccuracy As Integer = 0
        <DataMember()> _
        Public Property CarrierGradBillingAccuracy() As Integer
            Get
                Return _CarrierGradBillingAccuracy
            End Get
            Set(ByVal value As Integer)
                _CarrierGradBillingAccuracy = value
            End Set
        End Property

        Private _CarrierGradFinancialStrength As Integer = 0
        <DataMember()> _
        Public Property CarrierGradFinancialStrength() As Integer
            Get
                Return _CarrierGradFinancialStrength
            End Get
            Set(ByVal value As Integer)
                _CarrierGradFinancialStrength = value
            End Set
        End Property

        Private _CarrierGradEquipmentCondition As Integer = 0
        <DataMember()> _
        Public Property CarrierGradEquipmentCondition() As Integer
            Get
                Return _CarrierGradEquipmentCondition
            End Get
            Set(ByVal value As Integer)
                _CarrierGradEquipmentCondition = value
            End Set
        End Property

        Private _CarrierGradContactAttitude As Integer = 0
        <DataMember()> _
        Public Property CarrierGradContactAttitude() As Integer
            Get
                Return _CarrierGradContactAttitude
            End Get
            Set(ByVal value As Integer)
                _CarrierGradContactAttitude = value
            End Set
        End Property

        Private _CarrierGradDriverAttitude As Integer = 0
        <DataMember()> _
        Public Property CarrierGradDriverAttitude() As Integer
            Get
                Return _CarrierGradDriverAttitude
            End Get
            Set(ByVal value As Integer)
                _CarrierGradDriverAttitude = value
            End Set
        End Property

        Private _CarrierGradClaimFrequency As Integer = 0
        <DataMember()> _
        Public Property CarrierGradClaimFrequency() As Integer
            Get
                Return _CarrierGradClaimFrequency
            End Get
            Set(ByVal value As Integer)
                _CarrierGradClaimFrequency = value
            End Set
        End Property


        Private _CarrierGradClaimPayment As Integer = 0
        <DataMember()> _
        Public Property CarrierGradClaimPayment() As Integer
            Get
                Return _CarrierGradClaimPayment
            End Get
            Set(ByVal value As Integer)
                _CarrierGradClaimPayment = value
            End Set
        End Property

        Private _CarrierGradGeographicCoverage As Integer = 0
        <DataMember()> _
        Public Property CarrierGradGeographicCoverage() As Integer
            Get
                Return _CarrierGradGeographicCoverage
            End Get
            Set(ByVal value As Integer)
                _CarrierGradGeographicCoverage = value
            End Set
        End Property

        Private _CarrierGradCustomerService As Integer = 0
        <DataMember()> _
        Public Property CarrierGradCustomerService() As Integer
            Get
                Return _CarrierGradCustomerService
            End Get
            Set(ByVal value As Integer)
                _CarrierGradCustomerService = value
            End Set
        End Property

        Private _CarrierGradPriceChangeNotification As Integer = 0
        <DataMember()> _
        Public Property CarrierGradPriceChangeNotification() As Integer
            Get
                Return _CarrierGradPriceChangeNotification
            End Get
            Set(ByVal value As Integer)
                _CarrierGradPriceChangeNotification = value
            End Set
        End Property

        Private _CarrierGradPriceChangeFrequency As Integer = 0
        <DataMember()> _
        Public Property CarrierGradPriceChangeFrequency() As Integer
            Get
                Return _CarrierGradPriceChangeFrequency
            End Get
            Set(ByVal value As Integer)
                _CarrierGradPriceChangeFrequency = value
            End Set
        End Property

        Private _CarrierGradPriceAggressiveness As Integer = 0
        <DataMember()> _
        Public Property CarrierGradPriceAggressiveness() As Integer
            Get
                Return _CarrierGradPriceAggressiveness
            End Get
            Set(ByVal value As Integer)
                _CarrierGradPriceAggressiveness = value
            End Set
        End Property

        Private _CarrierGradAverage As Double = 0
        <DataMember()> _
        Public Property CarrierGradAverage() As Double
            Get
                Return _CarrierGradAverage
            End Get
            Set(ByVal value As Double)
                _CarrierGradAverage = value
            End Set
        End Property

        Private _CarrierQualInsuranceDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CarrierQualInsuranceDate() As System.Nullable(Of Date)
            Get
                If Not _CarrierQualInsuranceDate.HasValue Then _CarrierQualInsuranceDate = Date.Now().AddYears(1)
                Return _CarrierQualInsuranceDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _CarrierQualInsuranceDate = value
            End Set
        End Property

        Private _CarrierQualQualified As Boolean = False
        <DataMember()> _
        Public Property CarrierQualQualified() As Boolean
            Get
                Return _CarrierQualQualified
            End Get
            Set(ByVal value As Boolean)
                _CarrierQualQualified = value
            End Set
        End Property

        Private _CarrierQualAuthority As String = ""
        <DataMember()> _
        Public Property CarrierQualAuthority() As String
            Get
                Return Left(_CarrierQualAuthority, 15)
            End Get
            Set(ByVal value As String)
                _CarrierQualAuthority = Left(value, 15)
            End Set
        End Property

        Private _CarrierQualContract As Boolean = False
        <DataMember()> _
        Public Property CarrierQualContract() As Boolean
            Get
                Return _CarrierQualContract
            End Get
            Set(ByVal value As Boolean)
                _CarrierQualContract = value
            End Set
        End Property

        Private _CarrierQualSignedDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CarrierQualSignedDate() As System.Nullable(Of Date)
            Get
                Return _CarrierQualSignedDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _CarrierQualSignedDate = value
            End Set
        End Property

        Private _CarrierQualContractExpiresDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CarrierQualContractExpiresDate() As System.Nullable(Of Date)
            Get
                Return _CarrierQualContractExpiresDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _CarrierQualContractExpiresDate = value
            End Set
        End Property

        Private _CarrierQualMaxPerShipment As Decimal = 0
        <DataMember()> _
        Public Property CarrierQualMaxPerShipment() As Decimal
            Get
                Return _CarrierQualMaxPerShipment
            End Get
            Set(ByVal value As Decimal)
                _CarrierQualMaxPerShipment = value
            End Set
        End Property

        Private _CarrierQualMaxAllShipments As Decimal = 0
        <DataMember()> _
        Public Property CarrierQualMaxAllShipments() As Decimal
            Get
                Return _CarrierQualMaxAllShipments
            End Get
            Set(ByVal value As Decimal)
                _CarrierQualMaxAllShipments = value
            End Set
        End Property

        Private _CarrierQualCurAllExposure As Decimal = 0
        <DataMember()> _
        Public Property CarrierQualCurAllExposure() As Decimal
            Get
                Return _CarrierQualCurAllExposure
            End Get
            Set(ByVal value As Decimal)
                _CarrierQualCurAllExposure = value
            End Set
        End Property

        Private _CarrierCodeVal1 As Byte = 0
        <DataMember()> _
        Public Property CarrierCodeVal1() As Byte
            Get
                Return _CarrierCodeVal1
            End Get
            Set(ByVal value As Byte)
                _CarrierCodeVal1 = value
            End Set
        End Property

        Private _CarrierCodeVal2 As Byte = 0
        <DataMember()> _
        Public Property CarrierCodeVal2() As Byte
            Get
                Return _CarrierCodeVal2
            End Get
            Set(ByVal value As Byte)
                _CarrierCodeVal2 = value
            End Set
        End Property

        Private _CarrierTruckDefault As Integer = 0
        <DataMember()> _
        Public Property CarrierTruckDefault() As Integer
            Get
                Return _CarrierTruckDefault
            End Get
            Set(ByVal value As Integer)
                _CarrierTruckDefault = value
            End Set
        End Property

        Private _CarrierAllowWebTender As Boolean = True
        <DataMember()> _
        Public Property CarrierAllowWebTender() As Boolean
            Get
                Return _CarrierAllowWebTender
            End Get
            Set(ByVal value As Boolean)
                _CarrierAllowWebTender = value
            End Set
        End Property

        Private _CarrierIgnoreTariff As Boolean = False
        <DataMember()> _
        Public Property CarrierIgnoreTariff() As Boolean
            Get
                Return _CarrierIgnoreTariff
            End Get
            Set(ByVal value As Boolean)
                _CarrierIgnoreTariff = value
            End Set
        End Property

        Private _CarrierSmartWayPartnerType As String = ""
        <DataMember()> _
        Public Property CarrierSmartWayPartnerType() As String
            Get
                Return Left(_CarrierSmartWayPartnerType, 50)
            End Get
            Set(ByVal value As String)
                _CarrierSmartWayPartnerType = Left(value, 50)
            End Set
        End Property

        Private _CarrierSmartWayScore As Decimal = 0
        <DataMember()> _
        Public Property CarrierSmartWayScore() As Decimal
            Get
                Return _CarrierSmartWayScore
            End Get
            Set(ByVal value As Decimal)
                _CarrierSmartWayScore = value
            End Set
        End Property

        Private _CarrierSmartWayPartner As Boolean = False
        <DataMember()> _
        Public Property CarrierSmartWayPartner() As Boolean
            Get
                Return _CarrierSmartWayPartner
            End Get
            Set(ByVal value As Boolean)
                _CarrierSmartWayPartner = value
            End Set
        End Property

        Private _CarrierUpdated As Byte()
        <DataMember()> _
        Public Property CarrierUpdated() As Byte()
            Get
                Return _CarrierUpdated
            End Get
            Set(ByVal value As Byte())
                _CarrierUpdated = value
            End Set
        End Property

        Private _CarrierAutoFinalize As Boolean = False
        <DataMember()> _
        Public Property CarrierAutoFinalize() As Boolean
            Get
                Return _CarrierAutoFinalize
            End Get
            Set(ByVal value As Boolean)
                _CarrierAutoFinalize = value
            End Set
        End Property

        Private _CarrierUseStdFuelAddendum As Boolean = False
        <DataMember()> _
        Public Property CarrierUseStdFuelAddendum() As Boolean
            Get
                Return _CarrierUseStdFuelAddendum
            End Get
            Set(ByVal value As Boolean)
                _CarrierUseStdFuelAddendum = value
            End Set
        End Property

        Private _CarrierAutoAssignProNumber As Boolean = False
        <DataMember()> _
        Public Property CarrierAutoAssignProNumber() As Boolean
            Get
                Return _CarrierAutoAssignProNumber
            End Get
            Set(ByVal value As Boolean)
                _CarrierAutoAssignProNumber = value
            End Set
        End Property

        Private _CarrierBudgets As List(Of CarrierBudget)
        <DataMember()> _
        Public Property CarrierBudgets() As List(Of CarrierBudget)
            Get
                Return _CarrierBudgets
            End Get
            Set(ByVal value As List(Of CarrierBudget))
                _CarrierBudgets = value
            End Set
        End Property

        Private _CarrierConts As List(Of CarrierCont)
        <DataMember()> _
        Public Property CarrierConts() As List(Of CarrierCont)
            Get
                Return _CarrierConts
            End Get
            Set(ByVal value As List(Of CarrierCont))
                _CarrierConts = value
            End Set
        End Property

        Private _CarrierEDIs As List(Of CarrierEDI)
        <DataMember()> _
        Public Property CarrierEDIs() As List(Of CarrierEDI)
            Get
                Return _CarrierEDIs
            End Get
            Set(ByVal value As List(Of CarrierEDI))
                _CarrierEDIs = value
            End Set
        End Property

        Private _CarrierTrucks As List(Of CarrierTruck)
        <DataMember()> _
        Public Property CarrierTrucks() As List(Of CarrierTruck)
            Get
                Return _CarrierTrucks
            End Get
            Set(ByVal value As List(Of CarrierTruck))
                _CarrierTrucks = value
            End Set
        End Property

        Private _CarrierLegalEntity As String = ""
        <DataMember()> _
        Public Property CarrierLegalEntity() As String
            Get
                Return Left(_CarrierLegalEntity, 50)
            End Get
            Set(ByVal value As String)
                _CarrierLegalEntity = Left(value, 50)
            End Set
        End Property

        Private _CarrierAlphaCode As String = ""
        <DataMember()> _
        Public Property CarrierAlphaCode() As String
            Get
                Return Left(_CarrierAlphaCode, 50)
            End Get
            Set(ByVal value As String)
                _CarrierAlphaCode = Left(value, 50)
            End Set
        End Property

        Private _CarrierUser1 As String = ""
        <DataMember()> _
        Public Property CarrierUser1() As String
            Get
                Return Left(_CarrierUser1, 4000)
            End Get
            Set(ByVal value As String)
                _CarrierUser1 = Left(value, 4000)
            End Set
        End Property

        Private _CarrierUser2 As String = ""
        <DataMember()> _
        Public Property CarrierUser2() As String
            Get
                Return Left(_CarrierUser2, 4000)
            End Get
            Set(ByVal value As String)
                _CarrierUser2 = Left(value, 4000)
            End Set
        End Property

        Private _CarrierUser3 As String = ""
        <DataMember()> _
        Public Property CarrierUser3() As String
            Get
                Return Left(_CarrierUser3, 4000)
            End Get
            Set(ByVal value As String)
                _CarrierUser3 = Left(value, 4000)
            End Set
        End Property

        Private _CarrierUser4 As String = ""
        <DataMember()> _
        Public Property CarrierUser4() As String
            Get
                Return Left(_CarrierUser4, 4000)
            End Get
            Set(ByVal value As String)
                _CarrierUser4 = Left(value, 4000)
            End Set
        End Property

        Private _CarrierCurType As Integer = 0
        <DataMember()> _
        Public Property CarrierCurType() As Integer
            Get
                Return _CarrierCurType
            End Get
            Set(ByVal value As Integer)
                _CarrierCurType = value
            End Set
        End Property

        Private _CarrierQualUSDot As String = ""
        <DataMember()> _
        Public Property CarrierQualUSDot() As String
            Get
                Return Left(_CarrierQualUSDot, 15)
            End Get
            Set(ByVal value As String)
                _CarrierQualUSDot = Left(value, 15)
            End Set
        End Property

        Private _CarrierQualCSAScore As String = ""
        <DataMember()> _
        Public Property CarrierQualCSAScore() As String
            Get
                Return Left(_CarrierQualCSAScore, 15)
            End Get
            Set(ByVal value As String)
                _CarrierQualCSAScore = Left(value, 15)
            End Set
        End Property

        Private _CarrierCalcOnTimeServiceLevel As Decimal = 0
        <DataMember()> _
        Public Property CarrierCalcOnTimeServiceLevel() As Decimal
            Get
                Return _CarrierCalcOnTimeServiceLevel
            End Get
            Set(ByVal value As Decimal)
                _CarrierCalcOnTimeServiceLevel = value
            End Set
        End Property

        Private _CarrierAssignedOnTimeServiceLevel As Decimal = 0
        <DataMember()> _
        Public Property CarrierAssignedOnTimeServiceLevel() As Decimal
            Get
                Return _CarrierAssignedOnTimeServiceLevel
            End Get
            Set(ByVal value As Decimal)
                _CarrierAssignedOnTimeServiceLevel = value
            End Set
        End Property

        Private _CarrierCalcOnTimeNoMonthsUsed As Decimal = 0
        <DataMember()> _
        Public Property CarrierCalcOnTimeNoMonthsUsed() As Decimal
            Get
                Return _CarrierCalcOnTimeNoMonthsUsed
            End Get
            Set(ByVal value As Decimal)
                _CarrierCalcOnTimeNoMonthsUsed = value
            End Set
        End Property


#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New Carrier
            instance = DirectCast(MemberwiseClone(), Carrier)
            instance.CarrierBudgets = Nothing
            For Each item In CarrierBudgets
                instance.CarrierBudgets.Add(DirectCast(item.Clone, CarrierBudget))
            Next
            instance.CarrierConts = Nothing
            For Each item In CarrierConts
                instance.CarrierConts.Add(DirectCast(item.Clone, CarrierCont))
            Next
            instance.CarrierEDIs = Nothing
            For Each item In CarrierEDIs
                instance.CarrierEDIs.Add(DirectCast(item.Clone, CarrierEDI))
            Next
            instance.CarrierTrucks = Nothing
            For Each item In CarrierTrucks
                instance.CarrierTrucks.Add(DirectCast(item.Clone, CarrierTruck))
            Next
            Return instance
        End Function

#End Region

    End Class
End Namespace