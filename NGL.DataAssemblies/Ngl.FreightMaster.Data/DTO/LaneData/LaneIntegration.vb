Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

Namespace DataTransferObjects
    <DataContract()> _
    Public Class LaneIntegration
        Inherits DTOBaseClass


#Region " Data Members"

        Private _LaneNumber As String = ""
        <DataMember()> _
        Public Property LaneNumber() As String
            Get
                Return Left(_LaneNumber, 50)
            End Get
            Set(ByVal value As String)
                _LaneNumber = Left(value, 50)
                NotifyPropertyChanged("LaneNumber")
            End Set
        End Property

        Private _LaneName As String = ""
        <DataMember()> _
        Public Property LaneName() As String
            Get
                Return Left(_LaneName, 50)
            End Get
            Set(ByVal value As String)
                _LaneName = Left(value, 50)
                NotifyPropertyChanged("LaneName")
            End Set
        End Property

        Private _LaneNumberMaster As String = ""
        <DataMember()> _
        Public Property LaneNumberMaster() As String
            Get
                Return Left(_LaneNumberMaster, 50)
            End Get
            Set(ByVal value As String)
                _LaneNumberMaster = Left(value, 50)
                NotifyPropertyChanged("LaneNumberMaster")
            End Set
        End Property

        Private _LaneNameMaster As String = ""
        <DataMember()> _
        Public Property LaneNameMaster() As String
            Get
                Return Left(_LaneNameMaster, 50)
            End Get
            Set(ByVal value As String)
                _LaneNameMaster = Left(value, 50)
                NotifyPropertyChanged("LaneNameMaster")
            End Set
        End Property

        
       
        Private _LaneCompNumber As String = ""
        <DataMember()> _
        Public Property LaneCompNumber() As String
            Get
                Return Left(_LaneCompNumber, 50)
            End Get
            Set(ByVal value As String)
                _LaneCompNumber = Left(value, 50)
                NotifyPropertyChanged("LaneCompNumber")
            End Set
        End Property

        Private _LaneDefaultCarrierUse As Boolean = False
        <DataMember()> _
        Public Property LaneDefaultCarrierUse() As Boolean
            Get
                Return _LaneDefaultCarrierUse
            End Get
            Set(ByVal value As Boolean)
                _LaneDefaultCarrierUse = value
                NotifyPropertyChanged("LaneDefaultCarrierUse")
            End Set
        End Property

        Private _LaneDefaultCarrierNumber As Integer = 0
        <DataMember()> _
        Public Property LaneDefaultCarrierNumber() As Integer
            Get
                Return _LaneDefaultCarrierNumber
            End Get
            Set(ByVal value As Integer)
                _LaneDefaultCarrierNumber = value
                NotifyPropertyChanged("LaneDefaultCarrierNumber")
            End Set
        End Property

        Private _LaneOrigCompNumber As String = ""
        <DataMember()> _
        Public Property LaneOrigCompNumber() As String
            Get
                Return Left(_LaneOrigCompNumber, 50)
            End Get
            Set(ByVal value As String)
                _LaneOrigCompNumber = Left(value, 50)
                NotifyPropertyChanged("LaneOrigCompNumber")
            End Set
        End Property

        Private _LaneOrigName As String = ""
        <DataMember()> _
        Public Property LaneOrigName() As String
            Get
                Return Left(_LaneOrigName, 40)
            End Get
            Set(ByVal value As String)
                _LaneOrigName = Left(value, 40)
                NotifyPropertyChanged("LaneOrigName")
            End Set
        End Property

        Private _LaneOrigAddress1 As String = ""
        <DataMember()> _
        Public Property LaneOrigAddress1() As String
            Get
                Return Left(_LaneOrigAddress1, 40)
            End Get
            Set(ByVal value As String)
                _LaneOrigAddress1 = Left(value, 40)
                NotifyPropertyChanged("LaneOrigAddress1")
            End Set
        End Property

        Private _LaneOrigAddress2 As String = ""
        <DataMember()> _
        Public Property LaneOrigAddress2() As String
            Get
                Return Left(_LaneOrigAddress2, 40)
            End Get
            Set(ByVal value As String)
                _LaneOrigAddress2 = Left(value, 40)
                NotifyPropertyChanged("LaneOrigAddress2")
            End Set
        End Property

        Private _LaneOrigAddress3 As String = ""
        <DataMember()> _
        Public Property LaneOrigAddress3() As String
            Get
                Return Left(_LaneOrigAddress3, 40)
            End Get
            Set(ByVal value As String)
                _LaneOrigAddress3 = Left(value, 40)
                NotifyPropertyChanged("LaneOrigAddress3")
            End Set
        End Property

        Private _LaneOrigCity As String = ""
        <DataMember()> _
        Public Property LaneOrigCity() As String
            Get
                Return Left(_LaneOrigCity, 25)
            End Get
            Set(ByVal value As String)
                _LaneOrigCity = Left(value, 25)
                NotifyPropertyChanged("LaneOrigCity")
            End Set
        End Property

        Private _LaneOrigState As String = ""
        <DataMember()> _
        Public Property LaneOrigState() As String
            Get
                Return Left(_LaneOrigState, 8)
            End Get
            Set(ByVal value As String)
                _LaneOrigState = Left(value, 8)
                NotifyPropertyChanged("LaneOrigState")
            End Set
        End Property

        Private _LaneOrigCountry As String = ""
        <DataMember()> _
        Public Property LaneOrigCountry() As String
            Get
                Return Left(_LaneOrigCountry, 30)
            End Get
            Set(ByVal value As String)
                _LaneOrigCountry = Left(value, 30)
                NotifyPropertyChanged("LaneOrigCountry")
            End Set
        End Property

        Private _LaneOrigZip As String = ""
        <DataMember()> _
        Public Property LaneOrigZip() As String
            Get
                Return Left(_LaneOrigZip, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
            End Get
            Set(ByVal value As String)
                _LaneOrigZip = Left(value, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
                NotifyPropertyChanged("LaneOrigZip")
            End Set
        End Property

        Private _LaneOrigContactPhone As String = ""
        <DataMember()> _
        Public Property LaneOrigContactPhone() As String
            Get
                Return Left(_LaneOrigContactPhone, 20) ' Modified by RHR for v-8.4.003 on 06/25/2021
            End Get
            Set(ByVal value As String)
                _LaneOrigContactPhone = Left(value, 20) ' Modified by RHR for v-8.4.003 on 06/25/2021
                NotifyPropertyChanged("LaneOrigContactPhone")
            End Set
        End Property

        Private _LaneOrigContactPhoneExt As String = ""
        <DataMember()> _
        Public Property LaneOrigContactPhoneExt() As String
            Get
                Return Left(_LaneOrigContactPhoneExt, 50)
            End Get
            Set(ByVal value As String)
                _LaneOrigContactPhoneExt = Left(value, 50)
                NotifyPropertyChanged("LaneOrigContactPhoneExt")
            End Set
        End Property

        Private _LaneOrigContactFax As String = ""
        <DataMember()> _
        Public Property LaneOrigContactFax() As String
            Get
                Return Left(_LaneOrigContactFax, 15)
            End Get
            Set(ByVal value As String)
                _LaneOrigContactFax = Left(value, 15)
                NotifyPropertyChanged("LaneOrigContactFax")
            End Set
        End Property

        Private _LaneDestCompNumber As String = ""
        <DataMember()> _
        Public Property LaneDestCompNumber() As String
            Get
                Return Left(_LaneDestCompNumber, 50)
            End Get
            Set(ByVal value As String)
                _LaneDestCompNumber = Left(value, 50)
                NotifyPropertyChanged("LaneDestCompNumber")
            End Set
        End Property

        Private _LaneDestName As String = ""
        <DataMember()> _
        Public Property LaneDestName() As String
            Get
                Return Left(_LaneDestName, 40)
            End Get
            Set(ByVal value As String)
                _LaneDestName = Left(value, 40)
                NotifyPropertyChanged("LaneDestName")
            End Set
        End Property

        Private _LaneDestAddress1 As String = ""
        <DataMember()> _
        Public Property LaneDestAddress1() As String
            Get
                Return Left(_LaneDestAddress1, 40)
            End Get
            Set(ByVal value As String)
                _LaneDestAddress1 = Left(value, 40)
                NotifyPropertyChanged("LaneDestAddress1")
            End Set
        End Property

        Private _LaneDestAddress2 As String = ""
        <DataMember()> _
        Public Property LaneDestAddress2() As String
            Get
                Return Left(_LaneDestAddress2, 40)
            End Get
            Set(ByVal value As String)
                _LaneDestAddress2 = Left(value, 40)
                NotifyPropertyChanged("LaneDestAddress2")
            End Set
        End Property

        Private _LaneDestAddress3 As String = ""
        <DataMember()> _
        Public Property LaneDestAddress3() As String
            Get
                Return Left(_LaneDestAddress3, 40)
            End Get
            Set(ByVal value As String)
                _LaneDestAddress3 = Left(value, 40)
                NotifyPropertyChanged("LaneDestAddress3")
            End Set
        End Property

        Private _LaneDestCity As String = ""
        <DataMember()> _
        Public Property LaneDestCity() As String
            Get
                Return Left(_LaneDestCity, 25)
            End Get
            Set(ByVal value As String)
                _LaneDestCity = Left(value, 25)
                NotifyPropertyChanged("LaneDestCity")
            End Set
        End Property

        Private _LaneDestState As String = ""
        <DataMember()> _
        Public Property LaneDestState() As String
            Get
                Return Left(_LaneDestState, 2)
            End Get
            Set(ByVal value As String)
                _LaneDestState = Left(value, 2)
                NotifyPropertyChanged("LaneDestState")
            End Set
        End Property

        Private _LaneDestCountry As String = ""
        <DataMember()> _
        Public Property LaneDestCountry() As String
            Get
                Return Left(_LaneDestCountry, 30)
            End Get
            Set(ByVal value As String)
                _LaneDestCountry = Left(value, 30)
                NotifyPropertyChanged("LaneDestCountry")
            End Set
        End Property

        Private _LaneDestZip As String = ""
        <DataMember()> _
        Public Property LaneDestZip() As String
            Get
                Return Left(_LaneDestZip, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
            End Get
            Set(ByVal value As String)
                _LaneDestZip = Left(value, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
                NotifyPropertyChanged("LaneDestZip")
            End Set
        End Property

        Private _LaneDestContactPhone As String = ""
        <DataMember()> _
        Public Property LaneDestContactPhone() As String
            Get
                Return Left(_LaneDestContactPhone, 20) ' Modified by RHR for v-8.4.003 on 06/25/2021
            End Get
            Set(ByVal value As String)
                _LaneDestContactPhone = Left(value, 20) ' Modified by RHR for v-8.4.003 on 06/25/2021
                NotifyPropertyChanged("LaneDestContactPhone")
            End Set
        End Property

        Private _LaneDestContactPhoneExt As String = ""
        <DataMember()> _
        Public Property LaneDestContactPhoneExt() As String
            Get
                Return Left(_LaneDestContactPhoneExt, 50)
            End Get
            Set(ByVal value As String)
                _LaneDestContactPhoneExt = Left(value, 50)
                NotifyPropertyChanged("LaneDestContactPhoneExt")
            End Set
        End Property

        Private _LaneDestContactFax As String = ""
        <DataMember()> _
        Public Property LaneDestContactFax() As String
            Get
                Return Left(_LaneDestContactFax, 15)
            End Get
            Set(ByVal value As String)
                _LaneDestContactFax = Left(value, 15)
                NotifyPropertyChanged("LaneDestContactFax")
            End Set
        End Property

        Private _LaneConsigneeNumber As String = ""
        <DataMember()> _
        Public Property LaneConsigneeNumber() As String
            Get
                Return Left(_LaneConsigneeNumber, 50)
            End Get
            Set(ByVal value As String)
                _LaneConsigneeNumber = Left(value, 50)
                NotifyPropertyChanged("LaneConsigneeNumber")
            End Set
        End Property

        Private _LaneRecMinIn As Integer = 0
        <DataMember()> _
        Public Property LaneRecMinIn() As Integer
            Get
                Return _LaneRecMinIn
            End Get
            Set(ByVal value As Integer)
                _LaneRecMinIn = value
                NotifyPropertyChanged("LaneRecMinIn")
            End Set
        End Property

        Private _LaneRecMinUnload As Integer = 0
        <DataMember()> _
        Public Property LaneRecMinUnload() As Integer
            Get
                Return _LaneRecMinUnload
            End Get
            Set(ByVal value As Integer)
                _LaneRecMinUnload = value
                NotifyPropertyChanged("LaneRecMinUnload")
            End Set
        End Property

        Private _LaneRecMinOut As Integer = 0
        <DataMember()> _
        Public Property LaneRecMinOut() As Integer
            Get
                Return _LaneRecMinOut
            End Get
            Set(ByVal value As Integer)
                _LaneRecMinOut = value
                NotifyPropertyChanged("LaneRecMinOut")
            End Set
        End Property

        Private _LaneAppt As Boolean = False
        <DataMember()> _
        Public Property LaneAppt() As Boolean
            Get
                Return _LaneAppt
            End Get
            Set(ByVal value As Boolean)
                _LaneAppt = value
                NotifyPropertyChanged("LaneAppt")
            End Set
        End Property

        Private _LanePalletExchange As Boolean = False
        <DataMember()> _
        Public Property LanePalletExchange() As Boolean
            Get
                Return _LanePalletExchange
            End Get
            Set(ByVal value As Boolean)
                _LanePalletExchange = value
                NotifyPropertyChanged("LanePalletExchange")
            End Set
        End Property

        Private _LanePalletType As String = ""
        <DataMember()> _
        Public Property LanePalletType() As String
            Get
                Return Left(_LanePalletType, 50)
            End Get
            Set(ByVal value As String)
                _LanePalletType = Left(value, 50)
                NotifyPropertyChanged("LanePalletType")
            End Set
        End Property

        Private _LaneBenchMiles As Double = 0
        <DataMember()> _
        Public Property LaneBenchMiles() As Double
            Get
                Return _LaneBenchMiles
            End Get
            Set(ByVal value As Double)
                _LaneBenchMiles = value
                NotifyPropertyChanged("LaneBenchMiles")
            End Set
        End Property

        Private _LaneBFC As Double = 0
        <DataMember()> _
        Public Property LaneBFC() As Double
            Get
                Return _LaneBFC
            End Get
            Set(ByVal value As Double)
                _LaneBFC = value
                NotifyPropertyChanged("LaneBFC")
            End Set
        End Property

        Private _LaneBFCType As String = ""
        <DataMember()> _
        Public Property LaneBFCType() As String
            Get
                Return Left(_LaneBFCType, 50)
            End Get
            Set(ByVal value As String)
                _LaneBFCType = Left(value, 50)
                NotifyPropertyChanged("LaneBFCType")
            End Set
        End Property

        Private _LaneRecHourStart As String = ""
        <DataMember()> _
        Public Property LaneRecHourStart() As String
            Get
                Return _LaneRecHourStart
            End Get
            Set(ByVal value As String)
                _LaneRecHourStart = value
                NotifyPropertyChanged("LaneRecHourStart")
            End Set
        End Property

        Private _LaneRecHourStop As String = ""
        <DataMember()> _
        Public Property LaneRecHourStop() As String
            Get
                Return _LaneRecHourStop
            End Get
            Set(ByVal value As String)
                _LaneRecHourStop = value
                NotifyPropertyChanged("LaneRecHourStop")
            End Set
        End Property

        Private _LaneDestHourStart As String = ""
        <DataMember()> _
        Public Property LaneDestHourStart() As String
            Get
                Return _LaneDestHourStart
            End Get
            Set(ByVal value As String)
                _LaneDestHourStart = value
                NotifyPropertyChanged("LaneDestHourStart")
            End Set
        End Property

        Private _LaneDestHourStop As String = ""
        <DataMember()> _
        Public Property LaneDestHourStop() As String
            Get
                Return _LaneDestHourStop
            End Get
            Set(ByVal value As String)
                _LaneDestHourStop = value
                NotifyPropertyChanged("LaneDestHourStop")
            End Set
        End Property

        Private _LaneComments As String = ""
        <DataMember()> _
        Public Property LaneComments() As String
            Get
                Return Left(_LaneComments, 255)
            End Get
            Set(ByVal value As String)
                _LaneComments = Left(value, 255)
                NotifyPropertyChanged("LaneComments")
            End Set
        End Property

        Private _LaneCommentsConfidential As String = ""
        <DataMember()> _
        Public Property LaneCommentsConfidential() As String
            Get
                Return Left(_LaneCommentsConfidential, 255)
            End Get
            Set(ByVal value As String)
                _LaneCommentsConfidential = Left(value, 255)
                NotifyPropertyChanged("LaneCommentsConfidential")
            End Set
        End Property
        
        Private _LaneLatitude As Double = 0
        <DataMember()> _
        Public Property LaneLatitude() As Double
            Get
                Return _LaneLatitude
            End Get
            Set(ByVal value As Double)
                _LaneLatitude = value
                NotifyPropertyChanged("LaneLatitude")
            End Set
        End Property

        Private _LaneLongitude As Double = 0
        <DataMember()> _
        Public Property LaneLongitude() As Double
            Get
                Return _LaneLongitude
            End Get
            Set(ByVal value As Double)
                _LaneLongitude = value
                NotifyPropertyChanged("LaneLongitude")
            End Set
        End Property

        Private _LaneTempType As String = ""
        <DataMember()> _
        Public Property LaneTempType() As String
            Get
                Return Left(_LaneTempType, 50)
            End Get
            Set(ByVal value As String)
                _LaneTempType = Left(value, 50)
                NotifyPropertyChanged("LaneTempType")
            End Set
        End Property

        Private _LaneTransType As String = ""
        <DataMember()> _
        Public Property LaneTransType() As String
            Get
                Return Left(_LaneTransType, 50)
            End Get
            Set(ByVal value As String)
                _LaneTransType = Left(value, 50)
                NotifyPropertyChanged("LaneTransType")
            End Set
        End Property

        Private _LanePrimaryBuyer As String = ""
        <DataMember()> _
        Public Property LanePrimaryBuyer() As String
            Get
                Return Left(_LanePrimaryBuyer, 50)
            End Get
            Set(ByVal value As String)
                _LanePrimaryBuyer = Left(value, 50)
                NotifyPropertyChanged("LanePrimaryBuyer")
            End Set
        End Property
       

        Private _LaneAptDelivery As Boolean = False
        <DataMember()> _
        Public Property LaneAptDelivery() As Boolean
            Get
                Return _LaneAptDelivery
            End Get
            Set(ByVal value As Boolean)
                _LaneAptDelivery = value
                NotifyPropertyChanged("LaneAptDelivery")
            End Set
        End Property

        Private _BrokerNumber As String = ""
        <DataMember()> _
        Public Property BrokerNumber() As String
            Get
                Return Left(_BrokerNumber, 50)
            End Get
            Set(ByVal value As String)
                _BrokerNumber = Left(value, 50)
                NotifyPropertyChanged("BrokerNumber")
            End Set
        End Property

        Private _BrokerName As String = ""
        <DataMember()> _
        Public Property BrokerName() As String
            Get
                Return Left(_BrokerName, 40)
            End Get
            Set(ByVal value As String)
                _BrokerName = Left(value, 40)
                NotifyPropertyChanged("BrokerName")
            End Set
        End Property

        Private _LaneOriginAddressUse As Boolean = False
        <DataMember()> _
        Public Property LaneOriginAddressUse() As Boolean
            Get
                Return _LaneOriginAddressUse
            End Get
            Set(ByVal value As Boolean)
                _LaneOriginAddressUse = value
                NotifyPropertyChanged("LaneOriginAddressUse")
            End Set
        End Property

        Private _LaneCarrierEquipmentCodes As String = ""
        <DataMember()> _
        Public Property LaneCarrierEquipmentCodes() As String
            Get
                Return Left(_LaneCarrierEquipmentCodes, 50)
            End Get
            Set(ByVal value As String)
                _LaneCarrierEquipmentCodes = Left(value, 50)
                NotifyPropertyChanged("LaneCarrierEquipmentCodes")
            End Set
        End Property

        Private _LaneChepGLID As String = ""
        <DataMember()> _
        Public Property LaneChepGLID() As String
            Get
                Return Left(_LaneChepGLID, 50)
            End Get
            Set(ByVal value As String)
                _LaneChepGLID = Left(value, 50)
                NotifyPropertyChanged("LaneChepGLID")
            End Set
        End Property

        Private _LaneCarrierTypeCode As String = ""
        <DataMember()> _
        Public Property LaneCarrierTypeCode() As String
            Get
                Return Left(_LaneCarrierTypeCode, 20)
            End Get
            Set(ByVal value As String)
                _LaneCarrierTypeCode = Left(value, 20)
                NotifyPropertyChanged("LaneCarrierTypeCode")
            End Set
        End Property


        Private _LanePickUpMon As Boolean = False
        <DataMember()> _
        Public Property LanePickUpMon() As Boolean
            Get
                Return _LanePickUpMon
            End Get
            Set(ByVal value As Boolean)
                _LanePickUpMon = value
                NotifyPropertyChanged("LanePickUpMon")
            End Set
        End Property


        Private _LanePickUpTue As Boolean = False
        <DataMember()> _
        Public Property LanePickUpTue() As Boolean
            Get
                Return _LanePickUpTue
            End Get
            Set(ByVal value As Boolean)
                _LanePickUpTue = value
                NotifyPropertyChanged("LanePickUpTue")
            End Set
        End Property


        Private _LanePickUpWed As Boolean = False
        <DataMember()> _
        Public Property LanePickUpWed() As Boolean
            Get
                Return _LanePickUpWed
            End Get
            Set(ByVal value As Boolean)
                _LanePickUpWed = value
                NotifyPropertyChanged("LanePickUpWed")
            End Set
        End Property


        Private _LanePickUpThu As Boolean = False
        <DataMember()> _
        Public Property LanePickUpThu() As Boolean
            Get
                Return _LanePickUpThu
            End Get
            Set(ByVal value As Boolean)
                _LanePickUpThu = value
                NotifyPropertyChanged("LanePickUpThu")
            End Set
        End Property


        Private _LanePickUpFri As Boolean = False
        <DataMember()> _
        Public Property LanePickUpFri() As Boolean
            Get
                Return _LanePickUpFri
            End Get
            Set(ByVal value As Boolean)
                _LanePickUpFri = value
                NotifyPropertyChanged("LanePickUpFri")
            End Set
        End Property


        Private _LanePickUpSat As Boolean = False
        <DataMember()> _
        Public Property LanePickUpSat() As Boolean
            Get
                Return _LanePickUpSat
            End Get
            Set(ByVal value As Boolean)
                _LanePickUpSat = value
                NotifyPropertyChanged("LanePickUpSat")
            End Set
        End Property


        Private _LanePickUpSun As Boolean = False
        <DataMember()> _
        Public Property LanePickUpSun() As Boolean
            Get
                Return _LanePickUpSun
            End Get
            Set(ByVal value As Boolean)
                _LanePickUpSun = value
                NotifyPropertyChanged("LanePickUpSun")
            End Set
        End Property


        Private _LaneDropOffMon As Boolean = False
        <DataMember()> _
        Public Property LaneDropOffMon() As Boolean
            Get
                Return _LaneDropOffMon
            End Get
            Set(ByVal value As Boolean)
                _LaneDropOffMon = value
                NotifyPropertyChanged("LaneDropOffMon")
            End Set
        End Property


        Private _LaneDropOffTue As Boolean = False
        <DataMember()> _
        Public Property LaneDropOffTue() As Boolean
            Get
                Return _LaneDropOffTue
            End Get
            Set(ByVal value As Boolean)
                _LaneDropOffTue = value
                NotifyPropertyChanged("LaneDropOffTue")
            End Set
        End Property


        Private _LaneDropOffWed As Boolean = False
        <DataMember()> _
        Public Property LaneDropOffWed() As Boolean
            Get
                Return _LaneDropOffWed
            End Get
            Set(ByVal value As Boolean)
                _LaneDropOffWed = value
                NotifyPropertyChanged("LaneDropOffWed")
            End Set
        End Property


        Private _LaneDropOffThu As Boolean = False
        <DataMember()> _
        Public Property LaneDropOffThu() As Boolean
            Get
                Return _LaneDropOffThu
            End Get
            Set(ByVal value As Boolean)
                _LaneDropOffThu = value
                NotifyPropertyChanged("LaneDropOffThu")
            End Set
        End Property


        Private _LaneDropOffFri As Boolean = False
        <DataMember()> _
        Public Property LaneDropOffFri() As Boolean
            Get
                Return _LaneDropOffFri
            End Get
            Set(ByVal value As Boolean)
                _LaneDropOffFri = value
                NotifyPropertyChanged("LaneDropOffFri")
            End Set
        End Property


        Private _LaneDropOffSat As Boolean = False
        <DataMember()> _
        Public Property LaneDropOffSat() As Boolean
            Get
                Return _LaneDropOffSat
            End Get
            Set(ByVal value As Boolean)
                _LaneDropOffSat = value
                NotifyPropertyChanged("LaneDropOffSat")
            End Set
        End Property


        Private _LaneDropOffSun As Boolean = False
        <DataMember()> _
        Public Property LaneDropOffSun() As Boolean
            Get
                Return _LaneDropOffSun
            End Get
            Set(ByVal value As Boolean)
                _LaneDropOffSun = value
                NotifyPropertyChanged("LaneDropOffSun")
            End Set
        End Property


#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New LaneIntegration
            instance = DirectCast(MemberwiseClone(), LaneIntegration)

            Return instance
        End Function

        Public Function Item(ByVal propertyname As String) As Object

            Return Me.[GetType]().GetProperty(propertyname).GetValue(Me, Nothing)

        End Function

#End Region

    End Class
End Namespace