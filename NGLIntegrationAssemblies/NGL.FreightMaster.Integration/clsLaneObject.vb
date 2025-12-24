<Serializable()> _
Public Class clsLaneObject
    Public LaneNumber As String = ""
    Public LaneName As String = ""
    Public LaneNumberMaster As String = ""
    Public LaneNameMaster As String = ""
    Public LaneCompNumber As String = ""
    Public LaneDefaultCarrierUse As Boolean = False
    Public LaneDefaultCarrierNumber As Integer = 0
    Public LaneOrigCompNumber As String = ""
    Public LaneOrigName As String = ""
    Public LaneOrigAddress1 As String = ""
    Public LaneOrigAddress2 As String = ""
    Public LaneOrigAddress3 As String = ""
    Public LaneOrigCity As String = ""
    Public LaneOrigState As String = ""
    Public LaneOrigCountry As String = ""
    Public LaneOrigZip As String = ""
    Public LaneOrigContactPhone As String = ""
    Public LaneOrigContactPhoneExt As String = ""
    Public LaneOrigContactFax As String = ""
    Public LaneDestCompNumber As String = ""
    Public LaneDestName As String = ""
    Public LaneDestAddress1 As String = ""
    Public LaneDestAddress2 As String = ""
    Public LaneDestAddress3 As String = ""
    Public LaneDestCity As String = ""
    Public LaneDestState As String = ""
    Public LaneDestCountry As String = ""
    Public LaneDestZip As String = ""
    Public LaneDestContactPhone As String = ""
    Public LaneDestContactPhoneExt As String = ""
    Public LaneDestContactFax As String = ""
    Public LaneConsigneeNumber As String = ""
    Public LaneRecMinIn As Integer = 0
    Public LaneRecMinUnload As Integer = 0
    Public LaneRecMinOut As Integer = 0
    Public LaneAppt As Boolean = False
    Public LanePalletExchange As Boolean = False
    Public LanePalletType As String = ""
    Public LaneBenchMiles As Integer = 0
    Public LaneBFC As Double = 0
    Public LaneBFCType As String = ""
    Public LaneRecHourStart As String = ""
    Public LaneRecHourStop As String = ""
    Public LaneDestHourStart As String = ""
    Public LaneDestHourStop As String = ""
    Public LaneComments As String = ""
    Public LaneCommentsConfidential As String = ""
    Public LaneLatitude As Double = 0
    Public LaneLongitude As Double = 0
    Public LaneTempType As Short = 0
    Public LaneTransType As Short = 0
    Public LanePrimaryBuyer As String = ""
    Public LaneAptDelivery As Boolean = False
    Public BrokerNumber As String = ""
    Public BrokerName As String = ""
    Public LaneOriginAddressUse As Boolean = False
    Public LaneCarrierEquipmentCodes As String = ""
    Public LaneChepGLID As String = ""
    Public LaneCarrierTypeCode As String = ""
    Public LanePickUpMon As Boolean = True
    Public LanePickUpTue As Boolean = True
    Public LanePickUpWed As Boolean = True
    Public LanePickUpThu As Boolean = True
    Public LanePickUpFri As Boolean = True
    Public LanePickUpSat As Boolean = True
    Public LanePickUpSun As Boolean = True
    Public LaneDropOffMon As Boolean = True
    Public LaneDropOffTue As Boolean = True
    Public LaneDropOffWed As Boolean = True
    Public LaneDropOffThu As Boolean = True
    Public LaneDropOffFri As Boolean = True
    Public LaneDropOffSat As Boolean = True
    Public LaneDropOffSun As Boolean = True

End Class

<Serializable()> _
Public Class clsLaneObject60 : Inherits clsImportDataBase
    Private _LaneNumber As String = ""
    Public Property LaneNumber As String
        Get
            Return Left(_LaneNumber, 150)
        End Get
        Set(value As String)
            _LaneNumber = Left(value, 150)
        End Set
    End Property

    Private _LaneName As String = ""
    Public Property LaneName As String
        Get
            Return Left(_LaneName, 140)
        End Get
        Set(value As String)
            _LaneName = Left(value, 140)
        End Set
    End Property

    Private _LaneNumberMaster As String = ""
    Public Property LaneNumberMaster As String
        Get
            Return Left(_LaneNumberMaster, 50)
        End Get
        Set(value As String)
            _LaneNumberMaster = Left(value, 50)
        End Set
    End Property

    Private _LaneNameMaster As String = ""
    Public Property LaneNameMaster As String
        Get
            Return Left(_LaneNameMaster, 50)
        End Get
        Set(value As String)
            _LaneNameMaster = Left(value, 50)
        End Set
    End Property

    Private _LaneCompNumber As String = ""
    Public Property LaneCompNumber As String
        Get
            Return Left(_LaneCompNumber, 50)
        End Get
        Set(value As String)
            _LaneCompNumber = Left(value, 50)
        End Set
    End Property

    Private _LaneDefaultCarrierUse As Boolean = False
    Public Property LaneDefaultCarrierUse As Boolean
        Get
            Return _LaneDefaultCarrierUse
        End Get
        Set(value As Boolean)
            _LaneDefaultCarrierUse = value
        End Set
    End Property

    Private _LaneDefaultCarrierNumber As Integer = 0
    Public Property LaneDefaultCarrierNumber As Integer
        Get
            Return _LaneDefaultCarrierNumber
        End Get
        Set(value As Integer)
            _LaneDefaultCarrierNumber = value
        End Set
    End Property

    Private _LaneOrigCompNumber As String = ""
    Public Property LaneOrigCompNumber As String
        Get
            Return Left(_LaneOrigCompNumber, 50)
        End Get
        Set(value As String)
            _LaneOrigCompNumber = Left(value, 50)
        End Set
    End Property

    Private _LaneOrigName As String = ""
    Public Property LaneOrigName As String
        Get
            Return Left(_LaneOrigName, 40)
        End Get
        Set(value As String)
            _LaneOrigName = Left(value, 40)
        End Set
    End Property

    Private _LaneOrigAddress1 As String = ""
    Public Property LaneOrigAddress1 As String
        Get
            Return Left(_LaneOrigAddress1, 40)
        End Get
        Set(value As String)
            _LaneOrigAddress1 = Left(value, 40)
        End Set
    End Property

    Private _LaneOrigAddress2 As String = ""
    Public Property LaneOrigAddress2 As String
        Get
            Return Left(_LaneOrigAddress2, 40)
        End Get
        Set(value As String)
            _LaneOrigAddress2 = Left(value, 40)
        End Set
    End Property

    Private _LaneOrigAddress3 As String = ""
    Public Property LaneOrigAddress3 As String
        Get
            Return Left(_LaneOrigAddress3, 40)
        End Get
        Set(value As String)
            _LaneOrigAddress3 = Left(value, 40)
        End Set
    End Property

    Private _LaneOrigCity As String = ""
    Public Property LaneOrigCity As String
        Get
            Return Left(_LaneOrigCity, 25)
        End Get
        Set(value As String)
            _LaneOrigCity = Left(value, 25)
        End Set
    End Property

    Private _LaneOrigState As String = ""
    Public Property LaneOrigState As String
        Get
            Return Left(_LaneOrigState, 2)
        End Get
        Set(value As String)
            _LaneOrigState = Left(value, 2)
        End Set
    End Property

    Private _LaneOrigCountry As String = ""
    Public Property LaneOrigCountry As String
        Get
            Return Left(_LaneOrigCountry, 30)
        End Get
        Set(value As String)
            _LaneOrigCountry = Left(value, 30)
        End Set
    End Property

    Private _LaneOrigZip As String = ""
    Public Property LaneOrigZip As String
        Get
            Return Left(_LaneOrigZip, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
        End Get
        Set(value As String)
            _LaneOrigZip = Left(value, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
        End Set
    End Property

    Private _LaneOrigContactPhone As String = ""
    Public Property LaneOrigContactPhone As String
        Get
            Return Left(_LaneOrigContactPhone, 20)  'Modified by RHR for v-8.4.003 on 06/25/2021
        End Get
        Set(value As String)
            _LaneOrigContactPhone = Left(value, 20)  'Modified by RHR for v-8.4.003 on 06/25/2021
        End Set
    End Property

    Private _LaneOrigContactPhoneExt As String = ""
    Public Property LaneOrigContactPhoneExt As String
        Get
            Return Left(_LaneOrigContactPhoneExt, 50)
        End Get
        Set(value As String)
            _LaneOrigContactPhoneExt = Left(value, 50)
        End Set
    End Property

    Private _LaneOrigContactFax As String = ""
    Public Property LaneOrigContactFax As String
        Get
            Return Left(_LaneOrigContactFax, 15)
        End Get
        Set(value As String)
            _LaneOrigContactFax = Left(value, 15)
        End Set
    End Property

    Private _LaneDestCompNumber As String = ""
    Public Property LaneDestCompNumber As String
        Get
            Return Left(_LaneDestCompNumber, 50)
        End Get
        Set(value As String)
            _LaneDestCompNumber = Left(value, 50)
        End Set
    End Property

    Private _LaneDestName As String = ""
    Public Property LaneDestName As String
        Get
            Return Left(_LaneDestName, 40)
        End Get
        Set(value As String)
            _LaneDestName = Left(value, 40)
        End Set
    End Property

    Private _LaneDestAddress1 As String = ""
    Public Property LaneDestAddress1 As String
        Get
            Return Left(_LaneDestAddress1, 40)
        End Get
        Set(value As String)
            _LaneDestAddress1 = Left(value, 40)
        End Set
    End Property

    Private _LaneDestAddress2 As String = ""
    Public Property LaneDestAddress2 As String
        Get
            Return Left(_LaneDestAddress2, 40)
        End Get
        Set(value As String)
            _LaneDestAddress2 = Left(value, 40)
        End Set
    End Property

    Private _LaneDestAddress3 As String = ""
    Public Property LaneDestAddress3 As String
        Get
            Return Left(_LaneDestAddress3, 40)
        End Get
        Set(value As String)
            _LaneDestAddress3 = Left(value, 40)
        End Set
    End Property

    Private _LaneDestCity As String = ""
    Public Property LaneDestCity As String
        Get
            Return Left(_LaneDestCity, 25)
        End Get
        Set(value As String)
            _LaneDestCity = Left(value, 25)
        End Set
    End Property

    Private _LaneDestState As String = ""
    Public Property LaneDestState As String
        Get
            Return Left(_LaneDestState, 2)
        End Get
        Set(value As String)
            _LaneDestState = Left(value, 2)
        End Set
    End Property

    Private _LaneDestCountry As String = ""
    Public Property LaneDestCountry As String
        Get
            Return Left(_LaneDestCountry, 30)
        End Get
        Set(value As String)
            _LaneDestCountry = Left(value, 30)
        End Set
    End Property

    Private _LaneDestZip As String = ""
    Public Property LaneDestZip As String
        Get
            Return Left(_LaneDestZip, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
        End Get
        Set(value As String)
            _LaneDestZip = Left(value, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
        End Set
    End Property

    Private _LaneDestContactPhone As String = ""
    Public Property LaneDestContactPhone As String
        Get
            Return Left(_LaneDestContactPhone, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
        End Get
        Set(value As String)
            _LaneDestContactPhone = Left(value, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
        End Set
    End Property

    Private _LaneDestContactPhoneExt As String = ""
    Public Property LaneDestContactPhoneExt As String
        Get
            Return Left(_LaneDestContactPhoneExt, 50)
        End Get
        Set(value As String)
            _LaneDestContactPhoneExt = Left(value, 50)
        End Set
    End Property

    Private _LaneDestContactFax As String = ""
    Public Property LaneDestContactFax As String
        Get
            Return Left(_LaneDestContactFax, 15)
        End Get
        Set(value As String)
            _LaneDestContactFax = Left(value, 15)
        End Set
    End Property

    Private _LaneConsigneeNumber As String = ""
    Public Property LaneConsigneeNumber As String
        Get
            Return Left(_LaneConsigneeNumber, 50)
        End Get
        Set(value As String)
            _LaneConsigneeNumber = Left(value, 50)
        End Set
    End Property

    Private _LaneRecMinIn As Integer = 0
    Public Property LaneRecMinIn As Integer
        Get
            Return _LaneRecMinIn
        End Get
        Set(value As Integer)
            _LaneRecMinIn = value
        End Set
    End Property

    Private _LaneRecMinUnload As Integer = 0
    Public Property LaneRecMinUnload As Integer
        Get
            Return _LaneRecMinUnload
        End Get
        Set(value As Integer)
            _LaneRecMinUnload = value
        End Set
    End Property

    Private _LaneRecMinOut As Integer = 0
    Public Property LaneRecMinOut As Integer
        Get
            Return _LaneRecMinOut
        End Get
        Set(value As Integer)
            _LaneRecMinOut = value
        End Set
    End Property

    Private _LaneAppt As Boolean = False
    Public Property LaneAppt As Boolean
        Get
            Return _LaneAppt
        End Get
        Set(value As Boolean)
            _LaneAppt = value
        End Set
    End Property

    Private _LanePalletExchange As Boolean = False
    Public Property LanePalletExchange As Boolean
        Get
            Return _LanePalletExchange
        End Get
        Set(value As Boolean)
            _LanePalletExchange = value
        End Set
    End Property

    Private _LanePalletType As String = ""
    Public Property LanePalletType As String
        Get
            Return Left(_LanePalletType, 50)
        End Get
        Set(value As String)
            _LanePalletType = Left(value, 50)
        End Set
    End Property

    Private _LaneBenchMiles As Integer = 0
    Public Property LaneBenchMiles As Integer
        Get
            Return _LaneBenchMiles
        End Get
        Set(value As Integer)
            _LaneBenchMiles = value
        End Set
    End Property

    Private _LaneBFC As Double = 0
    Public Property LaneBFC As Double
        Get
            Return _LaneBFC
        End Get
        Set(value As Double)
            _LaneBFC = value
        End Set
    End Property

    Private _LaneBFCType As String = ""
    Public Property LaneBFCType As String
        Get
            Return Left(_LaneBFCType, 50)
        End Get
        Set(value As String)
            _LaneBFCType = Left(value, 50)
        End Set
    End Property

    Private _LaneRecHourStart As String = ""
    Public Property LaneRecHourStart As String
        Get
            Return cleanDate(_LaneRecHourStart)
        End Get
        Set(value As String)
            _LaneRecHourStart = value
        End Set
    End Property

    Private _LaneRecHourStop As String = ""
    Public Property LaneRecHourStop As String
        Get
            Return cleanDate(_LaneRecHourStop)
        End Get
        Set(value As String)
            _LaneRecHourStop = value
        End Set
    End Property

    Private _LaneDestHourStart As String = ""
    Public Property LaneDestHourStart As String
        Get
            Return cleanDate(_LaneDestHourStart)
        End Get
        Set(value As String)
            _LaneDestHourStart = value
        End Set
    End Property

    Private _LaneDestHourStop As String = ""
    Public Property LaneDestHourStop As String
        Get
            Return cleanDate(_LaneDestHourStop)
        End Get
        Set(value As String)
            _LaneDestHourStop = value
        End Set
    End Property

    Private _LaneComments As String = ""
    Public Property LaneComments As String
        Get
            Return Left(_LaneComments, 255)
        End Get
        Set(value As String)
            _LaneComments = Left(value, 255)
        End Set
    End Property

    Private _LaneCommentsConfidential As String = ""
    Public Property LaneCommentsConfidential As String
        Get
            Return Left(_LaneCommentsConfidential, 255)
        End Get
        Set(value As String)
            _LaneCommentsConfidential = Left(value, 255)
        End Set
    End Property

    Private _LaneLatitude As Double = 0
    Public Property LaneLatitude As Double
        Get
            Return _LaneLatitude
        End Get
        Set(value As Double)
            _LaneLatitude = value
        End Set
    End Property

    Private _LaneLongitude As Double = 0
    Public Property LaneLongitude As Double
        Get
            Return _LaneLongitude
        End Get
        Set(value As Double)
            _LaneLongitude = value
        End Set
    End Property

    Private _LaneTempType As Short = 0
    Public Property LaneTempType As Short
        Get
            Return _LaneTempType
        End Get
        Set(value As Short)
            _LaneTempType = value
        End Set
    End Property

    Private _LaneTransType As Short = 0
    Public Property LaneTransType As Short
        Get
            Return _LaneTransType
        End Get
        Set(value As Short)
            _LaneTransType = value
        End Set
    End Property

    Private _LanePrimaryBuyer As String = ""
    Public Property LanePrimaryBuyer As String
        Get
            Return Left(_LanePrimaryBuyer, 50)
        End Get
        Set(value As String)
            _LanePrimaryBuyer = Left(value, 50)
        End Set
    End Property

    Private _LaneAptDelivery As Boolean = False
    Public Property LaneAptDelivery As Boolean
        Get
            Return _LaneAptDelivery
        End Get
        Set(value As Boolean)
            _LaneAptDelivery = value
        End Set
    End Property

    Public _BrokerNumber As String = ""
    Public Property BrokerNumber As String
        Get
            Return Left(_BrokerNumber, 50)
        End Get
        Set(value As String)
            _BrokerNumber = Left(value, 50)
        End Set
    End Property

    Public _BrokerName As String = ""
    Public Property BrokerName As String
        Get
            Return Left(_BrokerName, 30)
        End Get
        Set(value As String)
            _BrokerName = Left(value, 30)
        End Set
    End Property

    Private _LaneOriginAddressUse As Boolean = False
    Public Property LaneOriginAddressUse As Boolean
        Get
            Return _LaneOriginAddressUse
        End Get
        Set(value As Boolean)
            _LaneOriginAddressUse = value
        End Set
    End Property

    Private _LaneCarrierEquipmentCodes As String = ""
    Public Property LaneCarrierEquipmentCodes As String
        Get
            Return Left(_LaneCarrierEquipmentCodes, 50)
        End Get
        Set(value As String)
            _LaneCarrierEquipmentCodes = Left(value, 50)
        End Set
    End Property

    Private _LaneChepGLID As String = ""
    Public Property LaneChepGLID As String
        Get
            Return Left(_LaneChepGLID, 50)
        End Get
        Set(value As String)
            _LaneChepGLID = Left(value, 50)
        End Set
    End Property

    Private _LaneCarrierTypeCode As String = ""
    Public Property LaneCarrierTypeCode As String
        Get
            Return Left(_LaneCarrierTypeCode, 20)
        End Get
        Set(value As String)
            _LaneCarrierTypeCode = Left(value, 20)
        End Set
    End Property

    Private _LanePickUpMon As Boolean = True
    Public Property LanePickUpMon As Boolean
        Get
            Return _LanePickUpMon
        End Get
        Set(value As Boolean)
            _LanePickUpMon = value
        End Set
    End Property

    Private _LanePickUpTue As Boolean = True
    Public Property LanePickUpTue As Boolean
        Get
            Return _LanePickUpTue
        End Get
        Set(value As Boolean)
            _LanePickUpTue = value
        End Set
    End Property

    Private _LanePickUpWed As Boolean = True
    Public Property LanePickUpWed As Boolean
        Get
            Return _LanePickUpWed
        End Get
        Set(value As Boolean)
            _LanePickUpWed = value
        End Set
    End Property

    Private _LanePickUpThu As Boolean = True
    Public Property LanePickUpThu As Boolean
        Get
            Return _LanePickUpThu
        End Get
        Set(value As Boolean)
            _LanePickUpThu = value
        End Set
    End Property

    Private _LanePickUpFri As Boolean = True
    Public Property LanePickUpFri As Boolean
        Get
            Return _LanePickUpFri
        End Get
        Set(value As Boolean)
            _LanePickUpFri = value
        End Set
    End Property

    Private _LanePickUpSat As Boolean = True
    Public Property LanePickUpSat As Boolean
        Get
            Return _LanePickUpSat
        End Get
        Set(value As Boolean)
            _LanePickUpSat = value
        End Set
    End Property

    Private _LanePickUpSun As Boolean = True
    Public Property LanePickUpSun As Boolean
        Get
            Return _LanePickUpSun
        End Get
        Set(value As Boolean)
            _LanePickUpSun = value
        End Set
    End Property

    Private _LaneDropOffMon As Boolean = True
    Public Property LaneDropOffMon As Boolean
        Get
            Return _LaneDropOffMon
        End Get
        Set(value As Boolean)
            _LaneDropOffMon = value
        End Set
    End Property

    Private _LaneDropOffTue As Boolean = True
    Public Property LaneDropOffTue As Boolean
        Get
            Return _LaneDropOffTue
        End Get
        Set(value As Boolean)
            _LaneDropOffTue = value
        End Set
    End Property

    Private _LaneDropOffWed As Boolean = True
    Public Property LaneDropOffWed As Boolean
        Get
            Return _LaneDropOffWed
        End Get
        Set(value As Boolean)
            _LaneDropOffWed = value
        End Set
    End Property

    Private _LaneDropOffThu As Boolean = True
    Public Property LaneDropOffThu As Boolean
        Get
            Return _LaneDropOffThu
        End Get
        Set(value As Boolean)
            _LaneDropOffThu = value
        End Set
    End Property

    Private _LaneDropOffFri As Boolean = True
    Public Property LaneDropOffFri As Boolean
        Get
            Return _LaneDropOffFri
        End Get
        Set(value As Boolean)
            _LaneDropOffFri = value
        End Set
    End Property

    Private _LaneDropOffSat As Boolean = True
    Public Property LaneDropOffSat As Boolean
        Get
            Return _LaneDropOffSat
        End Get
        Set(value As Boolean)
            _LaneDropOffSat = value
        End Set
    End Property

    Private _LaneDropOffSun As Boolean = True
    Public Property LaneDropOffSun As Boolean
        Get
            Return _LaneDropOffSun
        End Get
        Set(value As Boolean)
            _LaneDropOffSun = value
        End Set
    End Property



    Private _LaneDefaultRouteSequence As Integer = 0
    Public Property LaneDefaultRouteSequence As Integer
        Get
            Return _LaneDefaultRouteSequence
        End Get
        Set(value As Integer)
            _LaneDefaultRouteSequence = value
        End Set
    End Property

    Private _LaneRouteGuideNumber As String = ""
    Public Property LaneRouteGuideNumber As String
        Get
            Return Left(_LaneRouteGuideNumber, 50)
        End Get
        Set(value As String)
            _LaneRouteGuideNumber = Left(value, 50)
        End Set
    End Property

    Private _LaneIsCrossDockFacility As Boolean = False
    Public Property LaneIsCrossDockFacility As Boolean
        Get
            Return _LaneIsCrossDockFacility
        End Get
        Set(value As Boolean)
            _LaneIsCrossDockFacility = value
        End Set
    End Property


End Class

<Serializable()> _
Public Class clsLaneObject70 : Inherits clsLaneObject60

    Private _LaneLegalEntity As String = ""
    Public Property LaneLegalEntity As String
        Get
            Return Left(_LaneLegalEntity, 50)
        End Get
        Set(value As String)
            _LaneLegalEntity = Left(value, 50)
        End Set
    End Property

    Private _LaneCompAlphaCode As String = ""
    Public Property LaneCompAlphaCode() As String
        Get
            Return Left(_LaneCompAlphaCode, 50)
        End Get
        Set(ByVal value As String)
            _LaneCompAlphaCode = Left(value, 50)
        End Set
    End Property

    Private _LaneRequiredOnTimeServiceLevel As Decimal
    Public Property LaneRequiredOnTimeServiceLevel() As Decimal
        Get
            Return _LaneRequiredOnTimeServiceLevel
        End Get
        Set(ByVal value As Decimal)
            _LaneRequiredOnTimeServiceLevel = value
        End Set
    End Property

    Private _LaneCalcOnTimeServiceLevel As Decimal
    Public Property LaneCalcOnTimeServiceLevel() As Decimal
        Get
            Return _LaneCalcOnTimeServiceLevel
        End Get
        Set(ByVal value As Decimal)
            _LaneCalcOnTimeServiceLevel = value
        End Set
    End Property

    Private _LaneCalcOnTimeNoMonthsUsed As Decimal
    Public Property LaneCalcOnTimeNoMonthsUsed() As Decimal
        Get
            Return _LaneCalcOnTimeNoMonthsUsed
        End Get
        Set(ByVal value As Decimal)
            _LaneCalcOnTimeNoMonthsUsed = value
        End Set
    End Property
    
    Private _LaneModeTypeControl As Integer = 3 'Truck
    Public Property LaneModeTypeControl() As Integer
        Get
            Return _LaneModeTypeControl
        End Get
        Set(ByVal value As Integer)
            _LaneModeTypeControl = value
        End Set
    End Property

    Private _LaneUser1 As String = ""
    Public Property LaneUser1 As String
        Get
            Return Left(_LaneUser1, 4000)
        End Get
        Set(value As String)
            _LaneUser1 = Left(value, 4000)
        End Set
    End Property

    Private _LaneUser2 As String = ""
    Public Property LaneUser2 As String
        Get
            Return Left(_LaneUser2, 4000)
        End Get
        Set(value As String)
            _LaneUser2 = Left(value, 4000)
        End Set
    End Property

    Private _LaneUser3 As String = ""
    Public Property LaneUser3 As String
        Get
            Return Left(_LaneUser3, 4000)
        End Get
        Set(value As String)
            _LaneUser3 = Left(value, 4000)
        End Set
    End Property

    Private _LaneUser4 As String = ""
    Public Property LaneUser4 As String
        Get
            Return Left(_LaneUser4, 4000)
        End Get
        Set(value As String)
            _LaneUser4 = Left(value, 4000)
        End Set
    End Property

    Private _LaneOrigLegalEntity As String = ""
    Public Property LaneOrigLegalEntity As String
        Get
            Return Left(_LaneOrigLegalEntity, 50)
        End Get
        Set(value As String)
            _LaneOrigLegalEntity = Left(value, 50)
        End Set
    End Property

    Private _LaneOrigCompAlphaCode As String = ""
    Public Property LaneOrigCompAlphaCode() As String
        Get
            Return Left(_LaneOrigCompAlphaCode, 50)
        End Get
        Set(ByVal value As String)
            _LaneOrigCompAlphaCode = Left(value, 50)
        End Set
    End Property

    Private _LaneDestLegalEntity As String = ""
    Public Property LaneDestLegalEntity As String
        Get
            Return Left(_LaneDestLegalEntity, 50)
        End Get
        Set(value As String)
            _LaneDestLegalEntity = Left(value, 50)
        End Set
    End Property

    Private _LaneDestCompAlphaCode As String = ""
    Public Property LaneDestCompAlphaCode() As String
        Get
            Return Left(_LaneDestCompAlphaCode, 50)
        End Get
        Set(ByVal value As String)
            _LaneDestCompAlphaCode = Left(value, 50)
        End Set
    End Property

    Public Shared Function GenerateSampleObject(ByVal LaneName As String, ByVal LaneNumber As String, ByVal CompNumber As Integer, ByVal CompAlphaCode As String, ByVal CompLegalEntity As String) As clsLaneObject70

        Return New clsLaneObject70 With { _
                .LaneName = LaneName,
               .LaneNumber = LaneNumber,
               .LaneCompAlphaCode = CompAlphaCode,
               .LaneCompNumber = CompNumber.ToString(),
               .LaneOrigCompAlphaCode = CompAlphaCode,
               .LaneOrigCompNumber = CompNumber.ToString(),
               .LaneLegalEntity = CompLegalEntity,
               .LaneDestName = "Customer 01",
               .LaneDestAddress1 = "123 Any Street",
               .LaneDestCity = "Any Town",
               .LaneDestState = "IL",
               .LaneDestCountry = "US",
               .LaneDestZip = "60611",
               .LaneOriginAddressUse = False,
               .LanePalletType = "N",
               .LaneBFC = 100,
               .LaneBFCType = "PERC",
               .LaneComments = "comments",
               .LaneCommentsConfidential = "confidential comments"}

    End Function

End Class


<Serializable()>
Public Class clsLaneObject80 : Inherits clsLaneObject70

    Private _LaneOrigContactEmail As String
    Public Property LaneOrigContactEmail() As String
        Get
            Return Left(_LaneOrigContactEmail, 50)
        End Get
        Set(ByVal value As String)
            _LaneOrigContactEmail = Left(value, 50)
        End Set
    End Property

    Private _LaneDestContactEmail As String
    Public Property LaneDestContactEmail() As String
        Get
            Return Left(_LaneDestContactEmail, 50)
        End Get
        Set(ByVal value As String)
            _LaneDestContactEmail = Left(value, 50)
        End Set
    End Property
End Class