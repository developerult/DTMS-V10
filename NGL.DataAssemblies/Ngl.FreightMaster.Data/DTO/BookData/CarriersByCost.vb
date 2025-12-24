Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker
Imports Destructurama
Imports Destructurama.Attributed
Imports Serilog

Namespace DataTransferObjects
    '''<summary>
    ''' used to hold a list of carrier results from the tariff and spot rate calculators
    ''' </summary>
    ''' <remarks>
    ''' Modified by RHR v-7.0.5.100 4/20/2016 
    ''' Created InterlinePoint Data Property
    ''' </remarks>

    <DataContract()>
    Public Class CarriersByCost
        Inherits DTOBaseClass

        Public Sub New()
            Me.Logger = Me.Logger.ForContext(Of CarriersByCost)
        End Sub

#Region " Enums"

        Public Enum MessageEnum
            None
            M_NoOrdersFound '"No BookRevenue data found."
            M_InvalidMessageFormat '"Invalid Message Format; the message [ {0} ] may require missing parameters."
            M_SQLFaultCannotReadTariff '"Cannot Read Tariff Information. Reason: {0} Message: {1} Details: {2}."
            M_SQLFaultCannotReadNonServicePoint '"Cannot Read Non Service Point Information. Reason: {0} Message: {1} Details: {2}."
            M_NonServicePoint '"Tariff is not allowed because order number {0} ships to a Non-Service Point." 
            M_SQLFaultCannotReadInterlinePoint '"Cannot Read Interline Point Information. Reason: {0} Message: {1} Details: {2}."
            M_InterlinePointRestricted '"Tariff is not allowed because order number {0} has a restriction against Interline Points." 
            M_SQLFaultCannotReadMinCharge '"Cannot Read Minimum Charge Information. Reason: {0} Message: {1} Details: {2}."
            M_SQLFaultCannotReadDiscounts  '"Cannot Read Discount Information. Reason: {0} Message: {1} Details: {2}."
            M_LineHaulAdjustedByNegotiatedMinCharge '"The line haul cost has been adjusted because of a negotiated minimum charge, {0}, for contract. "
            M_LineHaulAdjustedByMinCharge '"The line haul cost has been adjusted because of a minimum charge, {0}, assigned to the published rate."
            M_ZeroMilesUsingMinCharge '"The miles are zero or the distance rate is invalid; using minimum cost."
            W_CarrierContractExpired '"**** WARNING ****  This Carriers Contract With Us Has Expired!"
            W_CarrierInsExpired '"**** WARNING ****  This Carriers Insurance Has Expired!"
            W_CarrierNotQualified   '"**** WARNING ****  This Carrier Is Not Qualified"
            W_CarrierOverMaxAllShip '"**** WARNING ****  This Carriers is over the maximum exposure for all shipments!"
            W_CarrierOverMaxPerShip '"**** WARNING ****  This Carriers is over the maximum exposure per shipment!"
            M_SQLFaultCannotReadCarrierQual  '"Cannot Read Carrier Qualification Information. Reason: {0} Message: {1} Details: {2}."
            M_LaneProfileNotSupported '"One or more of the Lane profile specific fees associated with this load are not supported."
            M_SQLFaultCannotReadCarrierFees  '"Cannot Read Carrier Fee Information. Reason: {0} Message: {1} Details: {2}."
            M_DiscountMinValueNotExceeded '"The discount applied is using the minimum amount."
            M_SQLFaultCannotReadCarrTarContract  '"Cannot Read Carrier Tariff Contract Information. Reason: {0} Message: {1} Details: {2}."
            M_SQLFaultCannotReadCarrierTariffNoDriveDays '"Cannot Read Carrier Tariff No Drive Days Information. Reason: {0} Message: {1} Details: {2}."
            E_UnExpected '"An Unexpected Error Has Occurred!  Check details for more information.   You should manually refresh your data to be sure you have the latest changes."
            E_InvalidEstDateSetup '"Cannot calculate Estimated Delivery or Must Leave By Dates; SetupData method failed: the load or carrier data is not valid."
            E_InvalidEstDateReqDate '"Cannot calculate Must Leave By Date; the Required Date is not valid."
            E_InvalidEstDateLoadDate '"Cannot calculate Estimated Delivery Date; the Load Date is not valid."
            E_CannotCalcEstDates '"Cannot calculate Estimated Delivery or Must Leave By Dates because {0}."
        End Enum
#End Region

#Region " Data Members"

        Private _CarrierControl As Integer = 0
        <DataMember()>
        Public Property CarrierControl() As Integer
            Get
                Return _CarrierControl
            End Get
            Set(ByVal value As Integer)
                _CarrierControl = value
            End Set
        End Property

        Private _CarrierNumber As Integer = 0
        <DataMember()>
        Public Property CarrierNumber() As Integer
            Get
                Return _CarrierNumber
            End Get
            Set(ByVal value As Integer)
                _CarrierNumber = value
            End Set
        End Property

        Private _CarrierName As String = ""
        <DataMember()>
        Public Property CarrierName() As String
            Get
                Return _CarrierName
            End Get
            Set(ByVal value As String)
                _CarrierName = value
            End Set
        End Property

        Private _CarrierMileRate As Decimal = 0
        <DataMember()>
        Public Property CarrierMileRate() As Decimal
            Get
                Return _CarrierMileRate
            End Get
            Set(ByVal value As Decimal)
                _CarrierMileRate = value
            End Set
        End Property

        Private _CarrierLbsRate As Decimal = 0
        <DataMember()>
        Public Property CarrierLbsRate() As Decimal
            Get
                Return _CarrierLbsRate
            End Get
            Set(ByVal value As Decimal)
                _CarrierLbsRate = value
            End Set
        End Property

        Private _CarrierCubeRate As Decimal = 0
        <DataMember()>
        Public Property CarrierCubeRate() As Decimal
            Get
                Return _CarrierCubeRate
            End Get
            Set(ByVal value As Decimal)
                _CarrierCubeRate = value
            End Set
        End Property

        Private _CarrierCaseRate As Decimal = 0
        <DataMember()>
        Public Property CarrierCaseRate() As Decimal
            Get
                Return _CarrierCaseRate
            End Get
            Set(ByVal value As Decimal)
                _CarrierCaseRate = value
            End Set
        End Property

        Private _CarrierPltRate As Decimal = 0
        <DataMember()>
        Public Property CarrierPltRate() As Decimal
            Get
                Return _CarrierPltRate
            End Get
            Set(ByVal value As Decimal)
                _CarrierPltRate = value
            End Set
        End Property

        Private _CarrierRate As Decimal = 0
        <DataMember()>
        Public Property CarrierRate() As Decimal
            Get

                'If _CarrierRate = 0 Then
                Select Case _CarrTarEquipMatTarRateTypeControl
                    Case Utilities.TariffRateType.DistanceK
                        _CarrierRate = _CarrierMileRate
                    Case Utilities.TariffRateType.DistanceM
                        _CarrierRate = _CarrierMileRate
                    Case Utilities.TariffRateType.FlatRate
                        _CarrierRate = _CarrierTLCost
                    Case Utilities.TariffRateType.ClassRate
                        _CarrierRate = _CarrierLbsRate
                    Case Utilities.TariffRateType.UnitOfMeasure
                        Select Case _CarrTarEquipMatTarBracketTypeControl
                            Case Utilities.BracketType.Pallets
                                _CarrierRate = _CarrierPltRate
                            Case Utilities.BracketType.FlatPallet
                                'Modified by RHR for v-8.5.4.002 on 08/25/2023 added FlatPallet logic
                                _CarrierRate = _CarrierPltRate
                            Case Utilities.BracketType.Quantity
                                _CarrierRate = _CarrierCaseRate
                            Case Utilities.BracketType.Volume
                                _CarrierRate = _CarrierCubeRate
                            Case Utilities.BracketType.Distance
                                _CarrierRate = _CarrierMileRate
                            Case Else
                                _CarrierRate = _CarrierLbsRate
                        End Select
                    Case Else
                        _CarrierRate = _CarrierLbsRate
                End Select
                'End If

                Return _CarrierRate
            End Get
            Set(ByVal value As Decimal)
                _CarrierRate = value
            End Set
        End Property

        Private _CarrierTLCost As Decimal = 0
        <DataMember()>
        Public Property CarrierTLCost() As Decimal
            Get
                Return _CarrierTLCost
            End Get
            Set(ByVal value As Decimal)
                _CarrierTLCost = value
            End Set
        End Property

        Private _CarrierEquipment As String = ""
        <DataMember()>
        Public Property CarrierEquipment() As String
            Get
                Return _CarrierEquipment
            End Get
            Set(ByVal value As String)
                _CarrierEquipment = value
            End Set
        End Property

        Private _CarrierCost As Decimal = 0
        <DataMember()>
        Public Property CarrierCost() As Decimal
            Get
                Return _CarrierCost
            End Get
            Set(ByVal value As Decimal)
                _CarrierCost = value
            End Set
        End Property

        Private _CarrierMinCost As Decimal = 0
        <DataMember()>
        Public Property CarrierMinCost() As Decimal
            Get
                Return _CarrierMinCost
            End Get
            Set(ByVal value As Decimal)
                _CarrierMinCost = value
            End Set
        End Property

        Private _CarrierRateExpires As System.Nullable(Of Date)
        <DataMember()>
        Public Property CarrierRateExpires() As System.Nullable(Of Date)
            Get
                Return _CarrierRateExpires
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _CarrierRateExpires = value
            End Set
        End Property

        ''' <summary>
        ''' ErrMsg has been depreciated and replaced by the Messages dictionay list as of v-6.4 4/18/14
        ''' </summary>
        ''' <remarks></remarks>
        Private _ErrMsg As String = ""
        <DataMember()>
        Public Property ErrMsg() As String
            Get
                Return _ErrMsg
            End Get
            Set(ByVal value As String)
                _ErrMsg = value
            End Set
        End Property

        Private _Messages As New Dictionary(Of String, List(Of NGLMessage))
        <DataMember()>
        Public Property Messages As Dictionary(Of String, List(Of NGLMessage))
            Get
                Return _Messages
            End Get
            Set(ByVal value As Dictionary(Of String, List(Of NGLMessage)))
                _Messages = value
            End Set
        End Property

        Private _StopCharges As Decimal = 0
        <DataMember()>
        Public Property StopCharges() As Decimal
            Get
                Return _StopCharges
            End Get
            Set(ByVal value As Decimal)
                _StopCharges = value
            End Set
        End Property

        Private _PickCharges As Decimal = 0
        <DataMember()>
        Public Property PickCharges() As Decimal
            Get
                Return _PickCharges
            End Get
            Set(ByVal value As Decimal)
                _PickCharges = value
            End Set
        End Property

        Private _FuelCost As Decimal = 0
        <DataMember()>
        Public Property FuelCost() As Decimal
            Get
                Return _FuelCost
            End Get
            Set(ByVal value As Decimal)
                _FuelCost = value
            End Set
        End Property

        Private _OtherFees As Decimal = 0
        <DataMember()>
        Public Property OtherFees() As Decimal
            Get
                Return _OtherFees
            End Get
            Set(ByVal value As Decimal)
                _OtherFees = value
            End Set
        End Property

        Private _TotalAccessorial As Decimal = 0
        <DataMember()>
        Public Property TotalAccessorial() As Decimal
            Get
                Return _TotalAccessorial
            End Get
            Set(ByVal value As Decimal)
                _TotalAccessorial = value
            End Set
        End Property

        Private _BookRevLoadMiles As System.Nullable(Of Double)
        <DataMember()>
        Public Property BookRevLoadMiles() As System.Nullable(Of Double)
            Get
                Return _BookRevLoadMiles
            End Get
            Set(ByVal value As System.Nullable(Of Double))
                _BookRevLoadMiles = value
            End Set
        End Property

        Private _BookCarrTruckControl As Integer = 0
        <DataMember()>
        Public Property BookCarrTruckControl() As Integer
            Get
                Return _BookCarrTruckControl
            End Get
            Set(ByVal value As Integer)
                _BookCarrTruckControl = value
            End Set
        End Property

        Private _BookCarrTarControl As Integer = 0
        <DataMember()>
        Public Property BookCarrTarControl() As Integer
            Get
                Return _BookCarrTarControl
            End Get
            Set(ByVal value As Integer)
                _BookCarrTarControl = value
            End Set
        End Property

        Private _BookCarrTarRevisionNumber As Integer = 0
        <DataMember()>
        Public Property BookCarrTarRevisionNumber() As Integer
            Get
                Return _BookCarrTarRevisionNumber
            End Get
            Set(ByVal value As Integer)
                _BookCarrTarRevisionNumber = value
            End Set
        End Property

        Private _BookCarrTarName As String
        <DataMember(), LogWithName("Tariff")>
        Public Property BookCarrTarName As String
            Get
                Return Left(_BookCarrTarName, 50)
            End Get
            Set(value As String)
                _BookCarrTarName = Left(value, 50)
            End Set
        End Property

        Private _BookCarrTarEquipControl As Integer = 0
        <DataMember()>
        Public Property BookCarrTarEquipControl() As Integer
            Get
                Return _BookCarrTarEquipControl
            End Get
            Set(ByVal value As Integer)
                _BookCarrTarEquipControl = value
            End Set
        End Property

        Private _BookCarrTarEquipName As String
        <DataMember(), LogWithName("Equipment")>
        Public Property BookCarrTarEquipName As String
            Get
                Return Left(_BookCarrTarEquipName, 50)
            End Get
            Set(value As String)
                _BookCarrTarEquipName = Left(value, 50)
            End Set
        End Property

        Private _BookCarrTarEquipMatControl As Integer = 0
        <DataMember()>
        Public Property BookCarrTarEquipMatControl() As Integer
            Get
                Return _BookCarrTarEquipMatControl
            End Get
            Set(ByVal value As Integer)
                _BookCarrTarEquipMatControl = value
            End Set
        End Property

        Private _BookCarrTarEquipMatName As String
        <DataMember(), LogWithName("TariffEquipmentMatrixName")>
        Public Property BookCarrTarEquipMatName As String
            Get
                Return Left(_BookCarrTarEquipMatName, 50)
            End Get
            Set(value As String)
                _BookCarrTarEquipMatName = Left(value, 50)
            End Set
        End Property

        Private _BookCarrTarEquipMatDetControl As Integer = 0
        <DataMember()>
        Public Property BookCarrTarEquipMatDetControl() As Integer
            Get
                Return _BookCarrTarEquipMatDetControl
            End Get
            Set(ByVal value As Integer)
                _BookCarrTarEquipMatDetControl = value
            End Set
        End Property

        Private _BookCarrTarEquipMatDetID As Integer = 0
        <DataMember()>
        Public Property BookCarrTarEquipMatDetID() As Integer
            Get
                Return _BookCarrTarEquipMatDetID
            End Get
            Set(ByVal value As Integer)
                _BookCarrTarEquipMatDetID = value
            End Set
        End Property

        Private _BookCarrTarEquipMatDetValue As System.Nullable(Of Decimal)
        <DataMember()>
        Public Property BookCarrTarEquipMatDetValue() As System.Nullable(Of Decimal)
            Get
                Return _BookCarrTarEquipMatDetValue
            End Get
            Set(ByVal value As System.Nullable(Of Decimal))
                _BookCarrTarEquipMatDetValue = value
            End Set
        End Property

        Private _BookModeTypeControl As Integer = 0
        <DataMember()>
        Public Property BookModeTypeControl() As Integer
            Get
                Return _BookModeTypeControl
            End Get
            Set(ByVal value As Integer)
                _BookModeTypeControl = value
            End Set
        End Property

        Private _ModeTypeName As String
        <DataMember()>
        Public Property ModeTypeName() As String
            Get
                Return _ModeTypeName
            End Get
            Set(ByVal value As String)
                _ModeTypeName = value
            End Set
        End Property

        Private _BookAllowInterlinePoints As Boolean = True
        <DataMember(), LogWithName("AllowInterlinePoints")>
        Public Property BookAllowInterlinePoints() As Boolean
            Get
                Return _BookAllowInterlinePoints
            End Get
            Set(ByVal value As Boolean)
                _BookAllowInterlinePoints = value
            End Set
        End Property

        Private _BookTransType As String = ""
        <DataMember()>
        Public Property BookTransType() As String
            Get
                Return Left(_BookTransType, 50)
            End Get
            Set(ByVal value As String)
                _BookTransType = Left(value, 50)
            End Set
        End Property

        Private _BookRevDiscount As Decimal = 0
        <DataMember(), LogWithName("Discount")>
        Public Property BookRevDiscount() As Decimal
            Get
                Return _BookRevDiscount
            End Get
            Set(ByVal value As Decimal)
                _BookRevDiscount = value
            End Set
        End Property

        Private _BookRevLineHaul As Decimal = 0
        <DataMember(), LogWithName("LineHaul")>
        Public Property BookRevLineHaul() As Decimal
            Get
                Return _BookRevLineHaul
            End Get
            Set(ByVal value As Decimal)
                _BookRevLineHaul = value
            End Set
        End Property

        Private _CarrTarTempType As Integer = 0
        <DataMember()>
        Public Property CarrTarTempType() As Integer
            Get
                Return _CarrTarTempType
            End Get
            Set(ByVal value As Integer)
                If (Me._CarrTarTempType <> value) Then
                    Me._CarrTarTempType = value
                    Me.SendPropertyChanged("CarrTarTempType")
                End If
            End Set
        End Property

        Private _TempTypeName As String
        <DataMember(), LogWithName("TemperatureRequirements")>
        Public Property TempTypeName() As String
            Get
                Return _TempTypeName
            End Get
            Set(ByVal value As String)
                _TempTypeName = value
            End Set
        End Property

        Private _CarrTarEquipMatClassTypeControl As Integer = 0
        <DataMember()>
        Public Property CarrTarEquipMatClassTypeControl() As Integer
            Get
                Return _CarrTarEquipMatClassTypeControl
            End Get
            Set(ByVal value As Integer)
                If (Me._CarrTarEquipMatClassTypeControl <> value) Then
                    Me._CarrTarEquipMatClassTypeControl = value
                    Me.SendPropertyChanged("CarrTarEquipMatClassTypeControl")
                End If
            End Set
        End Property

        Private _ClassTypeName As String
        <DataMember()>
        Public Property ClassTypeName() As String
            Get
                Return _ClassTypeName
            End Get
            Set(ByVal value As String)
                _ClassTypeName = value
            End Set
        End Property

        Private _CarrTarEquipMatTarRateTypeControl As Integer = 0
        <DataMember()>
        Public Property CarrTarEquipMatTarRateTypeControl() As Integer
            Get
                Return _CarrTarEquipMatTarRateTypeControl
            End Get
            Set(ByVal value As Integer)
                If (Me._CarrTarEquipMatTarRateTypeControl <> value) Then
                    Me._CarrTarEquipMatTarRateTypeControl = value
                    Me.SendPropertyChanged("CarrTarEquipMatTarRateTypeControl")
                End If
            End Set
        End Property

        Private _RateTypeName As String
        <DataMember()>
        Public Property RateTypeName() As String
            Get
                Return _RateTypeName
            End Get
            Set(ByVal value As String)
                _RateTypeName = value
            End Set
        End Property

        Private _CarrTarEquipMatTarBracketTypeControl As Integer = 0
        <DataMember()>
        Public Property CarrTarEquipMatTarBracketTypeControl() As Integer
            Get
                Return _CarrTarEquipMatTarBracketTypeControl
            End Get
            Set(ByVal value As Integer)
                If (Me._CarrTarEquipMatTarBracketTypeControl <> value) Then
                    Me._CarrTarEquipMatTarBracketTypeControl = value
                    Me.SendPropertyChanged("CarrTarEquipMatTarBracketTypeControl")
                End If
            End Set
        End Property

        Private _BracketTypeName As String
        <DataMember()>
        Public Property BracketTypeName() As String
            Get
                Return _BracketTypeName
            End Get
            Set(ByVal value As String)
                _BracketTypeName = value
            End Set
        End Property

        Private _CarrTarWillDriveSunday As Boolean = False
        <DataMember()>
        Public Property CarrTarWillDriveSunday() As Boolean
            Get
                Return _CarrTarWillDriveSunday
            End Get
            Set(ByVal value As Boolean)
                _CarrTarWillDriveSunday = value
            End Set
        End Property

        Private _CarrTarWillDriveSaturday As Boolean = False
        <DataMember()>
        Public Property CarrTarWillDriveSaturday() As Boolean
            Get
                Return _CarrTarWillDriveSaturday
            End Get
            Set(ByVal value As Boolean)
                _CarrTarWillDriveSaturday = value
            End Set
        End Property

        Private _CarrTarOutbound As Boolean = False
        <DataMember(), LogWithName("Outbound")>
        Public Property CarrTarOutbound() As Boolean
            Get
                Return _CarrTarOutbound
            End Get
            Set(ByVal value As Boolean)
                _CarrTarOutbound = value
            End Set
        End Property

        Private _CarrTarEquipMatMaxDays As Integer = 0
        <DataMember()>
        Public Property CarrTarEquipMatMaxDays() As Integer
            Get
                Return _CarrTarEquipMatMaxDays
            End Get
            Set(ByVal value As Integer)
                _CarrTarEquipMatMaxDays = value
            End Set
        End Property

        Private _BookMustLeaveByDateTime As Date?
        <DataMember()>
        Public Property BookMustLeaveByDateTime() As Date?
            Get
                Return _BookMustLeaveByDateTime
            End Get
            Set(ByVal value As Date?)
                _BookMustLeaveByDateTime = value
            End Set
        End Property


        Private _BookExpDelDateTime As Date?
        <DataMember()>
        Public Property BookExpDelDateTime() As Date?
            Get
                Return _BookExpDelDateTime
            End Get
            Set(ByVal value As Date?)
                _BookExpDelDateTime = value
            End Set
        End Property

        ''' <summary>
        ''' Place holder for future functionality
        ''' </summary>
        ''' <remarks></remarks>
        Private _BookOutOfRouteMiles As Double
        <DataMember()>
        Public Property BookOutOfRouteMiles() As Double
            Get
                Return _BookOutOfRouteMiles
            End Get
            Set(ByVal value As Double)
                _BookOutOfRouteMiles = value
            End Set
        End Property

        Private _UpchargePercent As Double
        <DataMember()>
        Public Property UpchargePercent() As Double
            Get
                Return _UpchargePercent
            End Get
            Set(ByVal value As Double)
                _UpchargePercent = value
            End Set
        End Property

        Private _UpchargeCarrierCost As Decimal = 0
        <DataMember()>
        Public Property UpchargeCarrierCost() As Decimal
            Get
                Return _UpchargeCarrierCost
            End Get
            Set(ByVal value As Decimal)
                _UpchargeCarrierCost = value
            End Set
        End Property

        Private _HasMessages As Boolean = False
        <DataMember()>
        Public Property HasMessages() As Boolean
            Get
                Return _HasMessages
            End Get
            Set(ByVal value As Boolean)
                _HasMessages = value
            End Set
        End Property

        Private _AllowSelect As Boolean = False
        <DataMember()>
        Public Property AllowSelect() As Boolean
            Get
                Return _AllowSelect
            End Get
            Set(ByVal value As Boolean)
                _AllowSelect = value
            End Set
        End Property

        ''' <summary>
        ''' show alert icon when cost is zero or when user cannot select the record
        ''' </summary> 
        ''' <remarks></remarks>
        Private _HasAlert As Boolean = False
        <DataMember()>
        Public Property HasAlert() As Boolean
            Get
                Return _HasAlert
            End Get
            Set(ByVal value As Boolean)
                _HasAlert = value
            End Set
        End Property

        ''' <summary>
        ''' show info icon when user is allowed to select the record and there are messages and cost are not zero
        ''' </summary> 
        ''' <remarks></remarks>
        Private _HasInfo As Boolean = False
        <DataMember()>
        Public Property HasInfo() As Boolean
            Get
                Return _HasInfo
            End Get
            Set(ByVal value As Boolean)
                _HasInfo = value
            End Set
        End Property


        Private _InterlinePoint As Boolean = False
        ''' <summary>
        ''' Flag to identify carriers that require an inteline carrier to delivery the load to one or more of the stops on this load
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Created by RHR v-7.0.5.100 4/20/2016 
        ''' </remarks>
        <DataMember()>
        Public Property InterlinePoint() As Boolean
            Get
                Return _InterlinePoint
            End Get
            Set(ByVal value As Boolean)
                _InterlinePoint = value
            End Set
        End Property

        'Added By LVV on 10/27/16 for v-7.0.5.110 Lane Default Carrier Enhancements
        Private _CarrierIgnoreTariff As Boolean = False
        <DataMember()>
        Public Property CarrierIgnoreTariff() As Boolean
            Get
                Return _CarrierIgnoreTariff
            End Get
            Set(ByVal value As Boolean)
                _CarrierIgnoreTariff = value
            End Set
        End Property

        'Added By LVV on 10/28/16 for v-7.0.5.110 Lane Default Carrier Enhancements
        Private _BookRevPreferredCarrier As Boolean = False
        <DataMember()>
        Public Property BookRevPreferredCarrier() As Boolean
            Get
                Return _BookRevPreferredCarrier
            End Get
            Set(ByVal value As Boolean)
                _BookRevPreferredCarrier = value
            End Set
        End Property

        'Added by LVV 11/1/16 for v-7.0.5.110 Lane Default Carrier Enhancements
        Private _RestrictCarrierSelection As Boolean = False
        <DataMember()>
        Public Property RestrictCarrierSelection() As Boolean
            Get
                Return _RestrictCarrierSelection
            End Get
            Set(ByVal value As Boolean)
                _RestrictCarrierSelection = value
            End Set
        End Property

        'Added by LVV 11/1/16 for v-7.0.5.110 Lane Default Carrier Enhancements
        Private _WarnOnRestrictedCarrierSelection As Boolean = False
        <DataMember()>
        Public Property WarnOnRestrictedCarrierSelection() As Boolean
            Get
                Return _WarnOnRestrictedCarrierSelection
            End Get
            Set(ByVal value As Boolean)
                _WarnOnRestrictedCarrierSelection = value
            End Set
        End Property


        Private _BookCarrTransitTime As Integer = 0
        ''' <summary>
        ''' Store the calculated Transit time in hours for the entire load        ''' 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Added by RHR for v-8.5.3.006 on 12/06/2022 
        '''   new logic used to calculate actual transit time using 
        '''   new parameter value adjustments for LTL and Truckload shipments
        '''   when using a tariff
        '''   'In future versions this data will be calculated by stop and will
        '''   alow for detailed delivery times using time windows
        ''' </remarks>
        <DataMember()>
        Public Property BookCarrTransitTime() As Integer
            Get
                Return _BookCarrTransitTime
            End Get
            Set(ByVal value As Integer)
                _BookCarrTransitTime = value
            End Set
        End Property

        ''' <summary>
        ''' set to true when a specific carrier rate is not available but messages or warnings are still required
        ''' Not to be used for logs
        ''' </summary>
        ''' <remarks>
        ''' Created by RHR on 04/15/2024 for v-8.5.4.006
        ''' </remarks>
        Private _postMessagesOnly As Boolean = False
        Public Property postMessagesOnly() As Boolean
            Get
                Return _postMessagesOnly
            End Get
            Set(ByVal value As Boolean)
                _postMessagesOnly = value
            End Set
        End Property

        Public Property HasCarrierBeenInvalidated As Boolean = False

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New CarriersByCost
            instance = DirectCast(MemberwiseClone(), CarriersByCost)
            Return instance
        End Function

        Public Sub setUpcharge(ByVal dblPerc As Double)
            Me.UpchargePercent = dblPerc
            Me.UpchargeCarrierCost = Me.CarrierCost + (Me.CarrierCost * dblPerc)
        End Sub

        Public Overrides Function ToString() As String
            Return $"{Me.CarrierName} offering rate: {Me.CarrierRate} until {Me.CarrierRateExpires} for {Me.BookCarrTarName} ({Me.RateTypeName}, Bracket:{Me.BracketTypeName} ) TariffEquipment: {Me.BookCarrTarEquipName} CarrierEquipment: {Me.CarrierEquipment}  {Me.ModeTypeName} {Me.ClassTypeName} {Me.TempTypeName} {Me.BookCarrTarEquipMatName}  "
        End Function

#Region " Message Enum Processing"
        Public Sub AddMessage(ByVal key As String, ByVal RawMessage As String, ByVal overrideAlwaystrue As Boolean)
            If Messages Is Nothing Then Messages = New Dictionary(Of String, List(Of NGLMessage))
            Messages.Add(key, New List(Of NGLMessage)({New NGLMessage(RawMessage)}))
        End Sub
        Public Sub AddMessage(ByVal key As String, ByVal ParamArray p() As String)
            Try
                Dim par As New List(Of NGLMessage)
                If Not p Is Nothing AndAlso p.Length > 0 Then
                    For Each s In p
                        par.Add(New NGLMessage(s))
                    Next
                End If
                If Messages Is Nothing Then Messages = New Dictionary(Of String, List(Of NGLMessage))
                If Not Messages.ContainsKey(key) Then
                    Messages.Add(key, par)
                Else
                    par.AddRange(Messages(key))
                    Messages(key) = par
                End If
            Catch ex As System.FormatException
                AddMessage(MessageEnum.M_InvalidMessageFormat, key)
            Catch ex As Exception
                Throw
            End Try
        End Sub

        Public Sub AddMessage(ByVal item As MessageEnum, ByVal ParamArray p() As String)
            Dim key As String = getMessageLocalizedString(item)

            AddMessage(key, p)
        End Sub

        Public Sub AddMessage(ByVal item As MessageEnum)
            Dim key As String = getMessageLocalizedString(item)
            AddMessage(key, Nothing)
        End Sub

        Public Sub AddMessage(ByVal item As MessageEnum, ByVal s As List(Of String))
            If (s Is Nothing OrElse s.Count < 1) Then
                AddMessage(item)
            Else
                AddMessage(item, s.ToArray())
            End If
        End Sub

        Public Shared Function getMessageNotLocalizedString(ByVal item As MessageEnum, Optional ByVal sdefault As String = "N/A") As String
            Dim strReturn = sdefault
            Try
                Select Case item
                    Case MessageEnum.M_NoOrdersFound
                        strReturn = "No BookRevenue data found."
                    Case MessageEnum.M_InvalidMessageFormat
                        strReturn = "Invalid Message Format; the message [ {0} ] may require missing parameters."
                    Case MessageEnum.M_SQLFaultCannotReadTariff
                        strReturn = "Cannot Read Tariff Information. Reason: {0} Message: {1} Details: {2}."
                    Case MessageEnum.M_SQLFaultCannotReadNonServicePoint
                        strReturn = "Cannot Read Non Service Point Information. Reason: {0} Message: {1} Details: {2}."
                    Case MessageEnum.M_NonServicePoint
                        strReturn = "Tariff is not allowed because order number {0} ships to a Non-Service Point."
                    Case MessageEnum.M_SQLFaultCannotReadInterlinePoint
                        strReturn = "Cannot Read Interline Point Information. Reason: {0} Message: {1} Details: {2}."
                    Case MessageEnum.M_InterlinePointRestricted
                        strReturn = "Tariff is not allowed because order number {0} has a restriction against Interline Points."
                    Case MessageEnum.M_SQLFaultCannotReadMinCharge
                        strReturn = "Cannot Read Minimum Charge Information. Reason: {0} Message: {1} Details: {2}."
                    Case MessageEnum.M_SQLFaultCannotReadDiscounts
                        strReturn = "Cannot Read Discount Information. Reason: {0} Message: {1} Details: {2}."
                    Case MessageEnum.M_LineHaulAdjustedByNegotiatedMinCharge
                        strReturn = "The line haul cost has been adjusted because of a negotiated minimum charge, {0}, for contract."
                    Case MessageEnum.M_LineHaulAdjustedByMinCharge
                        strReturn = "The line haul cost has been adjusted because of a minimum charge, {0}, assigned to the published rate."
                    Case MessageEnum.M_ZeroMilesUsingMinCharge
                        strReturn = "The miles are zero or the distance rate is invalid; using minimum cost."
                    Case MessageEnum.W_CarrierContractExpired
                        strReturn = "**** WARNING ****  This Carriers Contract With Us Has Expired!"
                    Case MessageEnum.W_CarrierInsExpired
                        strReturn = "**** WARNING ****  This Carriers Insurance Has Expired!"
                    Case MessageEnum.W_CarrierNotQualified
                        strReturn = "**** WARNING ****  This Carrier Is Not Qualified"
                    Case MessageEnum.W_CarrierOverMaxAllShip
                        strReturn = "**** WARNING ****  This Carriers is over the maximum exposure for all shipments!"
                    Case MessageEnum.W_CarrierOverMaxPerShip
                        strReturn = "**** WARNING ****  This Carriers is over the maximum exposure per shipment!"
                    Case MessageEnum.M_SQLFaultCannotReadCarrierQual
                        strReturn = "Cannot Read Carrier Qualification Information. Reason: {0} Message: {1} Details: {2}."
                    Case MessageEnum.M_LaneProfileNotSupported
                        strReturn = "One or more of the Lane profile specific fees associated with this load are not supported."
                    Case MessageEnum.M_SQLFaultCannotReadCarrierFees
                        strReturn = "Cannot Read Carrier Fee Information. Reason: {0} Message: {1} Details: {2}."
                    Case MessageEnum.M_DiscountMinValueNotExceeded
                        strReturn = "The discount applied is using the minimum amount."
                    Case MessageEnum.M_SQLFaultCannotReadCarrTarContract
                        strReturn = "Cannot Read Carrier Tariff Contract Information. Reason: {0} Message: {1} Details: {2}."
                    Case MessageEnum.M_SQLFaultCannotReadCarrierTariffNoDriveDays
                        strReturn = "Cannot Read Carrier Tariff No Drive Days Information. Reason: {0} Message: {1} Details: {2}."
                    Case MessageEnum.E_UnExpected
                        strReturn = "An Unexpected Error Has Occurred!  Check details for more information.   You should manually refresh your data to be sure you have the latest changes."
                    Case MessageEnum.E_InvalidEstDateSetup
                        strReturn = "Cannot calculate Estimated Delivery or Must Leave By Dates; SetupData method failed: the load or carrier data is not valid."
                    Case MessageEnum.E_InvalidEstDateReqDate
                        strReturn = "Cannot calculate Must Leave By Date; the Required Date is not valid."
                    Case MessageEnum.E_InvalidEstDateLoadDate
                        strReturn = "Cannot calculate Estimated Delivery Date; the Load Date is not valid."
                    Case MessageEnum.E_CannotCalcEstDates
                        strReturn = "Cannot calculate Estimated Delivery or Must Leave By Dates because {0}."
                End Select

            Catch ex As System.ArgumentNullException
                'enum type or value is nothing so return default
                Return strReturn
            Catch ex As System.ArgumentException
                'the item is not valid so return default
                Return strReturn
            Catch ex As Exception
                Throw
            End Try

            Return strReturn
        End Function

        Public Shared Function getMessageLocalizedString(ByVal item As MessageEnum, Optional ByVal sdefault As String = "N/A") As String
            Dim strReturn = sdefault
            Try
                Dim Enumerator As Type = GetType(MessageEnum)
                strReturn = [Enum].GetName(Enumerator, item)
            Catch ex As System.ArgumentNullException
                'enum type or value is nothing so return default
                Return strReturn
            Catch ex As System.ArgumentException
                'the item is not valid so return default
                Return strReturn
            Catch ex As Exception
                Throw
            End Try

            Return strReturn
        End Function

        ''' <summary>
        ''' Parses the MessageEnum using strEnum and returns the actual Enum if strEnum is not valid retuns the default MessageEnum E_UnExpected
        ''' </summary>
        ''' <param name="strEnum"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getMessageEnumFromString(ByVal strEnum As String) As MessageEnum
            Dim enmVal As MessageEnum = MessageEnum.None
            [Enum].TryParse(strEnum, enmVal)
            Return enmVal
        End Function

        Public Function concatMessage() As String
            If Messages Is Nothing OrElse Messages.Count < 1 Then Return ""
            Dim sb As New System.Text.StringBuilder()
            Return concatMessage(sb).ToString()
        End Function

        Public Function concatMessage(ByRef sb As System.Text.StringBuilder) As System.Text.StringBuilder
            If sb Is Nothing Then sb = New System.Text.StringBuilder()

            If Messages Is Nothing OrElse Messages.Count < 1 Then Return sb

            For Each m In Messages
                Dim eMsg = getMessageEnumFromString(m.Key)
                If eMsg = MessageEnum.None Then
                    sb.Append(m.Key)
                    sb.AppendLine()
                Else
                    Dim sMsg = getMessageNotLocalizedString(eMsg, m.Key)
                    If Not String.IsNullOrWhiteSpace(sMsg) Then
                        If Not m.Value Is Nothing AndAlso m.Value.Count > 0 Then
                            Try
                                sb.AppendFormat(sMsg, m.Value.Select(Function(x) x.Message).ToArray())
                            Catch ex As System.FormatException
                                sb.Append(sMsg)
                            End Try
                        Else
                            sb.Append(sMsg)
                        End If
                        sb.AppendLine()
                    End If
                End If
            Next
            Return sb
        End Function

#End Region

        ''' <summary>
        ''' Updates the both the BookModeTypeControl and the ModeTypeName properties
        ''' </summary>
        ''' <param name="ModeTypeControl"></param>
        ''' <remarks></remarks>
        Public Sub setModeType(ByVal ModeTypeControl As Integer)
            BookModeTypeControl = ModeTypeControl
            If ModeTypeControl > 0 Then
                Try
                    If [Enum].IsDefined(GetType(Utilities.TariffModeType), ModeTypeControl) Then
                        Dim enmVal As Utilities.TariffModeType = ModeTypeControl
                        ModeTypeName = [Enum].GetName(GetType(Utilities.TariffModeType), enmVal)
                    Else
                        ModeTypeName = ""
                    End If
                Catch ex As Exception
                    ModeTypeName = ""
                End Try
            End If

        End Sub

        ''' <summary>
        ''' Updates the both the CarrTarEquipMatTarBracketTypeControl and the BracketTypeName properties
        ''' </summary>
        ''' <param name="BracketTypeControl"></param>
        ''' <remarks></remarks>
        Public Sub setBracketType(ByVal BracketTypeControl As Integer)
            CarrTarEquipMatTarBracketTypeControl = BracketTypeControl
            If BracketTypeControl > 0 Then
                Try
                    If [Enum].IsDefined(GetType(Utilities.BracketType), BracketTypeControl) Then
                        Dim enmVal As Utilities.BracketType = BracketTypeControl
                        BracketTypeName = [Enum].GetName(GetType(Utilities.BracketType), enmVal)
                    Else
                        BracketTypeName = ""
                    End If
                Catch ex As Exception
                    BracketTypeName = ""
                End Try
            End If
        End Sub

        ''' <summary>
        ''' Updates the both the CarrTarTempType and the TempTypeName properties
        ''' </summary>
        ''' <param name="TempTypeControl"></param>
        ''' <remarks></remarks>
        Public Sub setTempType(ByVal TempTypeControl As Integer)
            CarrTarTempType = TempTypeControl
            If TempTypeControl < 0 Then
                Try
                    If [Enum].IsDefined(GetType(Utilities.TariffTempType), TempTypeControl) Then
                        Dim enmVal As Utilities.TariffTempType = TempTypeControl
                        TempTypeName = [Enum].GetName(GetType(Utilities.TariffTempType), enmVal)
                    Else
                        TempTypeName = ""
                    End If
                Catch ex As Exception
                    TempTypeName = ""
                End Try
            End If
        End Sub

        ''' <summary>
        ''' Updates the both the CarrTarEquipMatTarRateTypeControl and the RateTypeName properties
        ''' </summary>
        ''' <param name="RateTypeControl"></param>
        ''' <remarks></remarks>
        Public Sub setRateType(ByVal RateTypeControl As Integer)
            CarrTarEquipMatTarRateTypeControl = RateTypeControl
            If RateTypeControl > 0 Then
                Try
                    If [Enum].IsDefined(GetType(Utilities.TariffRateType), RateTypeControl) Then
                        Dim enmVal As Utilities.TariffRateType = RateTypeControl
                        RateTypeName = [Enum].GetName(GetType(Utilities.TariffRateType), enmVal)
                    Else
                        RateTypeName = ""
                    End If
                Catch ex As Exception
                    RateTypeName = ""
                End Try
            End If
        End Sub

        ''' <summary>
        ''' Updates the both the CarrTarEquipMatClassTypeControl and the ClassTypeName properties
        ''' </summary>
        ''' <param name="ClassTypeControl"></param>
        ''' <remarks></remarks>
        Public Sub setClassType(ByVal ClassTypeControl As Integer)
            CarrTarEquipMatClassTypeControl = ClassTypeControl
            If ClassTypeControl > 0 Then
                Try
                    If [Enum].IsDefined(GetType(Utilities.TariffClassType), ClassTypeControl) Then
                        Dim enmVal As Utilities.TariffClassType = ClassTypeControl
                        ClassTypeName = [Enum].GetName(GetType(Utilities.TariffClassType), enmVal)
                    Else
                        ClassTypeName = ""
                    End If
                Catch ex As Exception
                    ClassTypeName = ""
                End Try
            End If
        End Sub




#End Region
    End Class


End Namespace
