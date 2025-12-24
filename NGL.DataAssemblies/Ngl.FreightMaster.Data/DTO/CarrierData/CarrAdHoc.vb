Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class CarrAdHoc
        Inherits DTOBaseClass


#Region " Data Members"

        Private _CarrAdHocControl As Integer = 0
        <DataMember()> _
        Public Property CarrAdHocControl() As Integer
            Get
                Return _CarrAdHocControl
            End Get
            Set(ByVal value As Integer)
                _CarrAdHocControl = value
            End Set
        End Property

        Private _CarrAdHocNumber As Integer = 0
        <DataMember()> _
        Public Property CarrAdHocNumber() As Integer
            Get
                Return _CarrAdHocNumber
            End Get
            Set(ByVal value As Integer)
                _CarrAdHocNumber = value
            End Set
        End Property

        Private _CarrAdHocName As String = ""
        <DataMember()> _
        Public Property CarrAdHocName() As String
            Get
                Return Left(_CarrAdHocName, 40)
            End Get
            Set(ByVal value As String)
                _CarrAdHocName = Left(value, 40)
            End Set
        End Property

        Private _CarrAdHocStreetAddress1 As String = ""
        <DataMember()> _
        Public Property CarrAdHocStreetAddress1() As String
            Get
                Return Left(_CarrAdHocStreetAddress1, 40)
            End Get
            Set(ByVal value As String)
                _CarrAdHocStreetAddress1 = Left(value, 40)
            End Set
        End Property

        Private _CarrAdHocStreetAddress2 As String = ""
        <DataMember()> _
        Public Property CarrAdHocStreetAddress2() As String
            Get
                Return Left(_CarrAdHocStreetAddress2, 40)
            End Get
            Set(ByVal value As String)
                _CarrAdHocStreetAddress2 = Left(value, 40)
            End Set
        End Property

        Private _CarrAdHocStreetAddress3 As String = ""
        <DataMember()> _
        Public Property CarrAdHocStreetAddress3() As String
            Get
                Return Left(_CarrAdHocStreetAddress3, 40)
            End Get
            Set(ByVal value As String)
                _CarrAdHocStreetAddress3 = Left(value, 40)
            End Set
        End Property

        Private _CarrAdHocStreetCity As String = ""
        <DataMember()> _
        Public Property CarrAdHocStreetCity() As String
            Get
                Return Left(_CarrAdHocStreetCity, 25)
            End Get
            Set(ByVal value As String)
                _CarrAdHocStreetCity = Left(value, 25)
            End Set
        End Property

        Private _CarrAdHocStreetState As String = ""
        <DataMember()> _
        Public Property CarrAdHocStreetState() As String
            Get
                Return Left(_CarrAdHocStreetState, 8)
            End Get
            Set(ByVal value As String)
                _CarrAdHocStreetState = Left(value, 8)
            End Set
        End Property

        Private _CarrAdHocStreetCountry As String = ""
        <DataMember()> _
        Public Property CarrAdHocStreetCountry() As String
            Get
                Return Left(_CarrAdHocStreetCountry, 30)
            End Get
            Set(ByVal value As String)
                _CarrAdHocStreetCountry = Left(value, 30)
            End Set
        End Property

        Private _CarrAdHocStreetZip As String = ""
        <DataMember()> _
        Public Property CarrAdHocStreetZip() As String
            Get
                Return Left(_CarrAdHocStreetZip, 50)
            End Get
            Set(ByVal value As String)
                _CarrAdHocStreetZip = Left(value, 50)
            End Set
        End Property

        Private _CarrAdHocMailAddress1 As String = ""
        <DataMember()> _
        Public Property CarrAdHocMailAddress1() As String
            Get
                Return Left(_CarrAdHocMailAddress1, 40)
            End Get
            Set(ByVal value As String)
                _CarrAdHocMailAddress1 = Left(value, 40)
            End Set
        End Property

        Private _CarrAdHocMailAddress2 As String = ""
        <DataMember()> _
        Public Property CarrAdHocMailAddress2() As String
            Get
                Return Left(_CarrAdHocMailAddress2, 40)
            End Get
            Set(ByVal value As String)
                _CarrAdHocMailAddress2 = Left(value, 40)
            End Set
        End Property

        Private _CarrAdHocMailAddress3 As String = ""
        <DataMember()> _
        Public Property CarrAdHocMailAddress3() As String
            Get
                Return Left(_CarrAdHocMailAddress3, 40)
            End Get
            Set(ByVal value As String)
                _CarrAdHocMailAddress3 = Left(value, 40)
            End Set
        End Property

        Private _CarrAdHocMailCity As String = ""
        <DataMember()> _
        Public Property CarrAdHocMailCity() As String
            Get
                Return Left(_CarrAdHocMailCity, 25)
            End Get
            Set(ByVal value As String)
                _CarrAdHocMailCity = Left(value, 25)
            End Set
        End Property

        Private _CarrAdHocMailState As String = ""
        <DataMember()> _
        Public Property CarrAdHocMailState() As String
            Get
                Return Left(_CarrAdHocMailState, 8)
            End Get
            Set(ByVal value As String)
                _CarrAdHocMailState = Left(value, 8)
            End Set
        End Property

        Private _CarrAdHocMailCountry As String = ""
        <DataMember()> _
        Public Property CarrAdHocMailCountry() As String
            Get
                Return Left(_CarrAdHocMailCountry, 30)
            End Get
            Set(ByVal value As String)
                _CarrAdHocMailCountry = Left(value, 30)
            End Set
        End Property

        Private _CarrAdHocMailZip As String = ""
        <DataMember()> _
        Public Property CarrAdHocMailZip() As String
            Get
                Return Left(_CarrAdHocMailZip, 10)
            End Get
            Set(ByVal value As String)
                _CarrAdHocMailZip = Left(value, 10)
            End Set
        End Property

        Private _CarrAdHocModDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CarrAdHocModDate() As System.Nullable(Of Date)
            Get
                Return _CarrAdHocModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _CarrAdHocModDate = value
            End Set
        End Property

        Private _CarrAdHocModUser As String = ""
        <DataMember()> _
        Public Property CarrAdHocModUser() As String
            Get
                Return Left(_CarrAdHocModUser, 100)
            End Get
            Set(ByVal value As String)
                _CarrAdHocModUser = Left(value, 100)
            End Set
        End Property

        Private _CarrAdHocSCAC As String = ""
        <DataMember()> _
        Public Property CarrAdHocSCAC() As String
            Get
                Return Left(_CarrAdHocSCAC, 4)
            End Get
            Set(ByVal value As String)
                _CarrAdHocSCAC = Left(value, 4)
            End Set
        End Property

        Private _CarrAdHocAccountNo As String = ""
        <DataMember()> _
        Public Property CarrAdHocAccountNo() As String
            Get
                Return Left(_CarrAdHocAccountNo, 30)
            End Get
            Set(ByVal value As String)
                _CarrAdHocAccountNo = Left(value, 30)
            End Set
        End Property

        Private _CarrAdHocTypeCode As String = ""
        <DataMember()> _
        Public Property CarrAdHocTypeCode() As String
            Get
                Return Left(_CarrAdHocTypeCode, 1)
            End Get
            Set(ByVal value As String)
                _CarrAdHocTypeCode = Left(value, 1)
            End Set
        End Property

        Private _CarrAdHocGeneralInfo As String = ""
        <DataMember()> _
        Public Property CarrAdHocGeneralInfo() As String
            Get
                Return Left(_CarrAdHocGeneralInfo, 255)
            End Get
            Set(ByVal value As String)
                _CarrAdHocGeneralInfo = Left(value, 255)
            End Set
        End Property

        Private _CarrAdHocActive As Boolean = False
        <DataMember()> _
        Public Property CarrAdHocActive() As Boolean
            Get
                Return _CarrAdHocActive
            End Get
            Set(ByVal value As Boolean)
                _CarrAdHocActive = value
            End Set
        End Property

        Private _CarrAdHocWebSite As String = ""
        <DataMember()> _
        Public Property CarrAdHocWebSite() As String
            Get
                Return Left(_CarrAdHocWebSite, 255)
            End Get
            Set(ByVal value As String)
                _CarrAdHocWebSite = Left(value, 255)
            End Set
        End Property

        Private _CarrAdHocEmail As String = ""
        <DataMember()> _
        Public Property CarrAdHocEmail() As String
            Get
                Return Left(_CarrAdHocEmail, 50)
            End Get
            Set(ByVal value As String)
                _CarrAdHocEmail = Left(value, 50)
            End Set
        End Property

        Private _CarrAdHocNEXTStopAcct As String = ""
        <DataMember()> _
        Public Property CarrAdHocNEXTStopAcct() As String
            Get
                Return Left(_CarrAdHocNEXTStopAcct, 50)
            End Get
            Set(ByVal value As String)
                _CarrAdHocNEXTStopAcct = Left(value, 50)
            End Set
        End Property

        Private _CarrAdHocNEXTStopPwd As String = ""
        <DataMember()> _
        Public Property CarrAdHocNEXTStopPwd() As String
            Get
                Return Left(_CarrAdHocNEXTStopPwd, 50)
            End Get
            Set(ByVal value As String)
                _CarrAdHocNEXTStopPwd = Left(value, 50)
            End Set
        End Property


        Private _CarrAdHocGradReliability As Integer = 0
        <DataMember()> _
        Public Property CarrAdHocGradReliability() As Integer
            Get
                Return _CarrAdHocGradReliability
            End Get
            Set(ByVal value As Integer)
                _CarrAdHocGradReliability = value
            End Set
        End Property

        Private _CarrAdHocGradBillingAccuracy As Integer = 0
        <DataMember()> _
        Public Property CarrAdHocGradBillingAccuracy() As Integer
            Get
                Return _CarrAdHocGradBillingAccuracy
            End Get
            Set(ByVal value As Integer)
                _CarrAdHocGradBillingAccuracy = value
            End Set
        End Property

        Private _CarrAdHocGradFinancialStrength As Integer = 0
        <DataMember()> _
        Public Property CarrAdHocGradFinancialStrength() As Integer
            Get
                Return _CarrAdHocGradFinancialStrength
            End Get
            Set(ByVal value As Integer)
                _CarrAdHocGradFinancialStrength = value
            End Set
        End Property

        Private _CarrAdHocGradEquipmentCondition As Integer = 0
        <DataMember()> _
        Public Property CarrAdHocGradEquipmentCondition() As Integer
            Get
                Return _CarrAdHocGradEquipmentCondition
            End Get
            Set(ByVal value As Integer)
                _CarrAdHocGradEquipmentCondition = value
            End Set
        End Property

        Private _CarrAdHocGradContactAttitude As Integer = 0
        <DataMember()> _
        Public Property CarrAdHocGradContactAttitude() As Integer
            Get
                Return _CarrAdHocGradContactAttitude
            End Get
            Set(ByVal value As Integer)
                _CarrAdHocGradContactAttitude = value
            End Set
        End Property

        Private _CarrAdHocGradDriverAttitude As Integer = 0
        <DataMember()> _
        Public Property CarrAdHocGradDriverAttitude() As Integer
            Get
                Return _CarrAdHocGradDriverAttitude
            End Get
            Set(ByVal value As Integer)
                _CarrAdHocGradDriverAttitude = value
            End Set
        End Property

        Private _CarrAdHocGradClaimFrequency As Integer = 0
        <DataMember()> _
        Public Property CarrAdHocGradClaimFrequency() As Integer
            Get
                Return _CarrAdHocGradClaimFrequency
            End Get
            Set(ByVal value As Integer)
                _CarrAdHocGradClaimFrequency = value
            End Set
        End Property


        Private _CarrAdHocGradClaimPayment As Integer = 0
        <DataMember()> _
        Public Property CarrAdHocGradClaimPayment() As Integer
            Get
                Return _CarrAdHocGradClaimPayment
            End Get
            Set(ByVal value As Integer)
                _CarrAdHocGradClaimPayment = value
            End Set
        End Property

        Private _CarrAdHocGradGeographicCoverage As Integer = 0
        <DataMember()> _
        Public Property CarrAdHocGradGeographicCoverage() As Integer
            Get
                Return _CarrAdHocGradGeographicCoverage
            End Get
            Set(ByVal value As Integer)
                _CarrAdHocGradGeographicCoverage = value
            End Set
        End Property

        Private _CarrAdHocGradCustomerService As Integer = 0
        <DataMember()> _
        Public Property CarrAdHocGradCustomerService() As Integer
            Get
                Return _CarrAdHocGradCustomerService
            End Get
            Set(ByVal value As Integer)
                _CarrAdHocGradCustomerService = value
            End Set
        End Property

        Private _CarrAdHocGradPriceChangeNotification As Integer = 0
        <DataMember()> _
        Public Property CarrAdHocGradPriceChangeNotification() As Integer
            Get
                Return _CarrAdHocGradPriceChangeNotification
            End Get
            Set(ByVal value As Integer)
                _CarrAdHocGradPriceChangeNotification = value
            End Set
        End Property

        Private _CarrAdHocGradPriceChangeFrequency As Integer = 0
        <DataMember()> _
        Public Property CarrAdHocGradPriceChangeFrequency() As Integer
            Get
                Return _CarrAdHocGradPriceChangeFrequency
            End Get
            Set(ByVal value As Integer)
                _CarrAdHocGradPriceChangeFrequency = value
            End Set
        End Property

        Private _CarrAdHocGradPriceAggressiveness As Integer = 0
        <DataMember()> _
        Public Property CarrAdHocGradPriceAggressiveness() As Integer
            Get
                Return _CarrAdHocGradPriceAggressiveness
            End Get
            Set(ByVal value As Integer)
                _CarrAdHocGradPriceAggressiveness = value
            End Set
        End Property

        Private _CarrAdHocGradAverage As Double = 0
        <DataMember()> _
        Public Property CarrAdHocGradAverage() As Double
            Get
                Return _CarrAdHocGradAverage
            End Get
            Set(ByVal value As Double)
                _CarrAdHocGradAverage = value
            End Set
        End Property

        Private _CarrAdHocQualInsuranceDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CarrAdHocQualInsuranceDate() As System.Nullable(Of Date)
            Get
                Return _CarrAdHocQualInsuranceDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _CarrAdHocQualInsuranceDate = value
            End Set
        End Property

        Private _CarrAdHocQualQualified As Boolean = False
        <DataMember()> _
        Public Property CarrAdHocQualQualified() As Boolean
            Get
                Return _CarrAdHocQualQualified
            End Get
            Set(ByVal value As Boolean)
                _CarrAdHocQualQualified = value
            End Set
        End Property

        Private _CarrAdHocQualAuthority As String = ""
        <DataMember()> _
        Public Property CarrAdHocQualAuthority() As String
            Get
                Return Left(_CarrAdHocQualAuthority, 15)
            End Get
            Set(ByVal value As String)
                _CarrAdHocQualAuthority = Left(value, 15)
            End Set
        End Property

        Private _CarrAdHocQualContract As Boolean = False
        <DataMember()> _
        Public Property CarrAdHocQualContract() As Boolean
            Get
                Return _CarrAdHocQualContract
            End Get
            Set(ByVal value As Boolean)
                _CarrAdHocQualContract = value
            End Set
        End Property

        Private _CarrAdHocQualSignedDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CarrAdHocQualSignedDate() As System.Nullable(Of Date)
            Get
                Return _CarrAdHocQualSignedDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _CarrAdHocQualSignedDate = value
            End Set
        End Property

        Private _CarrAdHocQualContractExpiresDate As System.Nullable(Of Date)
        <DataMember()> _
        Public Property CarrAdHocQualContractExpiresDate() As System.Nullable(Of Date)
            Get
                Return _CarrAdHocQualContractExpiresDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _CarrAdHocQualContractExpiresDate = value
            End Set
        End Property

        Private _CarrAdHocQualMaxPerShipment As Decimal = 0
        <DataMember()> _
        Public Property CarrAdHocQualMaxPerShipment() As Decimal
            Get
                Return _CarrAdHocQualMaxPerShipment
            End Get
            Set(ByVal value As Decimal)
                _CarrAdHocQualMaxPerShipment = value
            End Set
        End Property

        Private _CarrAdHocQualMaxAllShipments As Decimal = 0
        <DataMember()> _
        Public Property CarrAdHocQualMaxAllShipments() As Decimal
            Get
                Return _CarrAdHocQualMaxAllShipments
            End Get
            Set(ByVal value As Decimal)
                _CarrAdHocQualMaxAllShipments = value
            End Set
        End Property

        Private _CarrAdHocQualCurAllExposure As Decimal = 0
        <DataMember()> _
        Public Property CarrAdHocQualCurAllExposure() As Decimal
            Get
                Return _CarrAdHocQualCurAllExposure
            End Get
            Set(ByVal value As Decimal)
                _CarrAdHocQualCurAllExposure = value
            End Set
        End Property

        Private _CarrAdHocCodeVal1 As Byte = 0
        <DataMember()> _
        Public Property CarrAdHocCodeVal1() As Byte
            Get
                Return _CarrAdHocCodeVal1
            End Get
            Set(ByVal value As Byte)
                _CarrAdHocCodeVal1 = value
            End Set
        End Property

        Private _CarrAdHocCodeVal2 As Byte = 0
        <DataMember()> _
        Public Property CarrAdHocCodeVal2() As Byte
            Get
                Return _CarrAdHocCodeVal2
            End Get
            Set(ByVal value As Byte)
                _CarrAdHocCodeVal2 = value
            End Set
        End Property

        Private _CarrAdHocTruckDefault As Integer = 0
        <DataMember()> _
        Public Property CarrAdHocTruckDefault() As Integer
            Get
                Return _CarrAdHocTruckDefault
            End Get
            Set(ByVal value As Integer)
                _CarrAdHocTruckDefault = value
            End Set
        End Property

        Private _CarrAdHocAllowWebTender As Boolean = False
        <DataMember()> _
        Public Property CarrAdHocAllowWebTender() As Boolean
            Get
                Return _CarrAdHocAllowWebTender
            End Get
            Set(ByVal value As Boolean)
                _CarrAdHocAllowWebTender = value
            End Set
        End Property

        Private _CarrAdHocIgnoreTariff As Boolean = False
        <DataMember()> _
        Public Property CarrAdHocIgnoreTariff() As Boolean
            Get
                Return _CarrAdHocIgnoreTariff
            End Get
            Set(ByVal value As Boolean)
                _CarrAdHocIgnoreTariff = value
            End Set
        End Property

        Private _CarrAdHocSmartWayPartnerType As String = ""
        <DataMember()> _
        Public Property CarrAdHocSmartWayPartnerType() As String
            Get
                Return Left(_CarrAdHocSmartWayPartnerType, 50)
            End Get
            Set(ByVal value As String)
                _CarrAdHocSmartWayPartnerType = Left(value, 50)
            End Set
        End Property

        Private _CarrAdHocSmartWayScore As Decimal = 0
        <DataMember()> _
        Public Property CarrAdHocSmartWayScore() As Decimal
            Get
                Return _CarrAdHocSmartWayScore
            End Get
            Set(ByVal value As Decimal)
                _CarrAdHocSmartWayScore = value
            End Set
        End Property

        Private _CarrAdHocSmartWayPartner As Boolean = False
        <DataMember()> _
        Public Property CarrAdHocSmartWayPartner() As Boolean
            Get
                Return _CarrAdHocSmartWayPartner
            End Get
            Set(ByVal value As Boolean)
                _CarrAdHocSmartWayPartner = value
            End Set
        End Property

        Private _CarrAdHocUpdated As Byte()
        <DataMember()> _
        Public Property CarrAdHocUpdated() As Byte()
            Get
                Return _CarrAdHocUpdated
            End Get
            Set(ByVal value As Byte())
                _CarrAdHocUpdated = value
            End Set
        End Property

        Private _CarrAdHocBudgets As List(Of CarrAdHocBudget)
        <DataMember()> _
        Public Property CarrAdHocBudgets() As List(Of CarrAdHocBudget)
            Get
                Return _CarrAdHocBudgets
            End Get
            Set(ByVal value As List(Of CarrAdHocBudget))
                _CarrAdHocBudgets = value
            End Set
        End Property

        Private _CarrAdHocConts As List(Of CarrAdHocCont)
        <DataMember()> _
        Public Property CarrAdHocConts() As List(Of CarrAdHocCont)
            Get
                Return _CarrAdHocConts
            End Get
            Set(ByVal value As List(Of CarrAdHocCont))
                _CarrAdHocConts = value
            End Set
        End Property


#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CarrAdHoc
            instance = DirectCast(MemberwiseClone(), CarrAdHoc)
            instance.CarrAdHocBudgets = Nothing
            For Each item In CarrAdHocBudgets
                instance.CarrAdHocBudgets.Add(DirectCast(item.Clone, CarrAdHocBudget))
            Next
            instance.CarrAdHocConts = Nothing
            For Each item In CarrAdHocConts
                instance.CarrAdHocConts.Add(DirectCast(item.Clone, CarrAdHocCont))
            Next
            Return instance
        End Function

#End Region

    End Class
End Namespace