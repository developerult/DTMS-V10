Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker

'Added by LVV 5/18/16 for v-7.0.5.110 DAT
Namespace DataTransferObjects
    ''' <summary>
    ''' DTO object for tblLoadTender data.  New methods and services now use direct refernce to LTS object.    ''' 
    ''' </summary>
    ''' <remarks>
    ''' Modified by RHR for v-8.1 on 3/23/2018
    '''   updated with new fields for backward compatibility.
    '''   new functionality should access dia via LTS objecs directly.
    ''' Modified by RHR for v-8.5.4.001 on 7/17/2023  added missing data elements
    ''' </remarks>
    <DataContract()>
    Public Class tblLoadTender
        Inherits DTOBaseClass

#Region "Enums"
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <remarks>
        ''' Modified by RHR for v-8.2 on 12/22/2018
        '''   changed RateQuote to LoadBoard
        '''   added SpotRate and RateShopping
        '''   added comments/descriptions
        ''' </remarks>
        Public Enum LoadTenderTypeEnum
            None = 0
            Manual = 1          'A load was manually tendered-dispatched by phone, fax, or email
            EDI204 = 2          'A load was tendered to a carrier via EDI 204
            Web = 3             'A load was tendered to a carrier via NEXTrack Web Tendering
            CPU = 4             'Customer Pick Up
            DAT = 5             'The load was posted and later tendered-dispatched to a carrier via DAT
            NextStop = 6        'A load has been posted on NextStop, stays as NextStop when a carrier bid is accepted
            P44 = 7             'Load Tender is set to P44 when a P44 API bid is dispatched
            LoadBoard = 8       'Pending loads rated from the load board that have not been dispatched
            SpotRate = 9        'User Entered Spot Rate, dispatching is manual but not required
            RateShopping = 10   'Pending loads from rate shopping that do not have a booking record and are not dispatched
        End Enum

        Public Enum LoadTenderStatusCodeEnum
            None = 0
            Pending = 1
            EDI997Pass = 2
            EDI997Fail = 3
            DataValidationFail = 4
            UpdateSent = 5
            CancellationSent = 6
            ManualRetransmit = 7
            TransmissionStopped = 8
            EDI820Received = 9
            Archived = 10
            EDI990Accept = 11
            EDI990Reject = 12
            FreightBillReceived = 13
            EDI214ReceivedWithErrors = 14
            EDI214Received = 15
            DATPosted = 16
            'PostExpired = 17
            'PostDeleted = 18
            _Error = 19
            DATAccepted = 20
            DATExpired = 21
            DATDeleted = 22
            NStopError = 23
            NStopPosted = 24
            NStopAccepted = 25
            NStopExpired = 26
            NStopDeleted = 27
            NStopUpdated = 28
            DATUpdated = 29
            DATError = 30
        End Enum

        ''' <summary>
        ''' [tblBidType] Enum version 
        ''' </summary>
        ''' <remarks>
        ''' Modified by RHR for v-8.2 on 12/04/2018 
        '''   added Spot Rate value of 4
        ''' Changes made here must also be applied to tblBidType
        '''     typically via the spUtilityPopulateLookupLists70
        '''         Insert the default tblBidType section
        ''' Note: BidTypeEnums are associated with SSOAAccount
        '''     the numbers are different and should not be confused
        '''     with the actual SSOA account table.
        '''     Both are required for Carrier APIs to work
        '''     Each BidType and SSOA has an SSOAType configured
        '''     in the tblSSOALEConfig settings. 1 to many
        '''     and may use a specific APIStatusCodeEnum
        ''' </remarks>
        Public Enum BidTypeEnum
            None = 0
            NextStop = 1
            NGLTariff = 2
            P44 = 3
            Spot = 4
            CHRAPI = 5
            UPSAPI = 6
            YRCAPI = 7
            JTSAPI = 8
            FedXAPI = 9
            EngageLaneAPI = 10
            HMBayAPI = 11
            FFEAPI = 12
            EVANSTSAPI = 13
            FROZENLOGAPI = 14
            HUDSONAPI = 15
            JBPARTNERSAPI = 16
            LANTERAPI = 17
            TQLAPI = 18
            GTZAPI = 19
        End Enum


        ''' <summary>
        ''' Bid table status codes
        ''' </summary>
        ''' <remarks>
        ''' Modified by RHR for v-8.1 on 2/13/2018
        '''   added Quoted enum value 8 for Rate Shopping Quotes
        ''' </remarks>
        Public Enum BidStatusCodeEnum
            None = 0
            Active = 1
            CarDeleted = 2
            OpsReject = 3
            OpsAccept = 4
            Expired = 5
            BidError = 6
            OpsDeletePost = 7
            Quoted = 8
        End Enum

        ''' <summary>
        '''Enum values with similar to database settings for CarrierEquipCodes
        '''Used to assign Mode of Delivery
        ''' </summary>
        ''' <remarks>
        ''' Changes here must also be made in the following
        ''' spPopulateStandardCarrierEquipCodes
        ''' spQryExpectedStandardEquipCodes
        ''' spMigrateStandardCarrierEquipCodes
        ''' In the future we need to find a way to synchronize these data objects
        ''' </remarks>
        Public Enum APIStatusCodeEnum
            None = 0 ' Default is Spot
            D48 = 1 ' Maps to DRY48: Dry 48 FT Van
            F48 = 2 ' Maps to FRZ48: Frozen 48 FT Van
            R48 = 3 ' Maps to REF48: REF 48 FT Van
            D53 = 4 ' Maps to DRY53: Dry 53 FT Van
            F53 = 5 ' Maps to FRZ53: Frozen 48 FT Van
            R53 = 6 ' Maps to REF53: REF 53 FT Van
            LD3 = 7 ' Maps to LD3: Container Code
            FB = 8 ' Maps to FB: Flat Bed
            C40H = 9 ' Maps to 1 X 40 HC: 40 FT HIGH CUBE CONTAINER
            C40S = 10 ' Maps to 1 X 40: 40 FT STANDARD CONTAINER
            C20S = 11 ' Maps to 1 X 20: 20 FT STANDARD CONTAINER
            C20R = 12 ' Maps to 1 X 20 REF: 20 FT REEFER CONTAINER
            C40R = 13 ' Maps to 1 X 40 REF: 40 FT REEFER CONTAINER
            RBC = 14 ' Maps to Rail: Rail Car - Box
            RL20 = 15 ' Maps to Rail20: Rail 20 FT Foot Container
            RL40 = 16 ' Maps to Rail40: Rail 40 FT Foot Container
            RL53 = 17 ' Maps to Rail53: Rail 53 FT Container
            RL48 = 18 ' Maps to Rail48: Rail 48 FT Container
            RRBC = 19 ' Maps to RailREF: Rail Car - Box REF
            RR20 = 20 ' Maps to Rail20REF: Rail 20 FT Foot REF Container
            RR40 = 21 ' Maps to Rail40REF: Rail 40 FT Foot REF Container
            RR53 = 22 ' Maps to Rail53REF: Rail 53 FT REF Container
            RR48 = 23 ' Maps to Rail48REF: Rail 48 FT REF Container
            RFBC = 24 ' Maps to RailFRZ: Rail Car - Box FRZ
            RF20 = 25 ' Maps to Rail20FRZ: Rail 20 FT Foot FRZ Container
            RF40 = 26 ' Maps to Rail40FRZ: Rail 40 FT Foot FRZ Container
            RF53 = 27 ' Maps to Rail53FRZ: Rail 53 FT FRZ Container
            RF48 = 28 ' Maps to Rail48FRZ: Rail 48 FT FRZ Container
            NA = 29 ' Maps to Unassigned: Unassigned
            SVD = 30 ' Maps to SprinterVan: Sprinter Van
            TT = 31 ' Maps to Tank: Tank Truck
            O53 = 32 ' Maps to Ocean53: Ocean 53 FT Container
            O48 = 33 ' Maps to Ocean48: Ocean 48 FT Container
            O40 = 34 ' Maps to Ocean40: Ocean 40 FT Container
            O20 = 35 ' Maps to Ocean20: Ocean 20 FT Container
            OR53 = 36 ' Maps to Ocean53REF: Ocean 53 FT REF Container
            OR48 = 37 ' Maps to Ocean48REF: Ocean 48 FT REF Container
            OR40 = 38 ' Maps to Ocean40REF: Ocean 40 FT REF Container
            OR20 = 39 ' Maps to Ocean20REF: Ocean 20 FT REF Container
            OF53 = 40 ' Maps to Ocean53FRZ: Ocean 53 FT FRZ Container
            OF48 = 41 ' Maps to Ocean48FRZ: Ocean 48 FT FRZ Container
            Of40 = 42 ' Maps to Ocean40FRZ: Ocean 40 FT FRZ Container
            OF20 = 43 ' Maps to Ocean20FRZ: Ocean 20 FT FRZ Container
            LTL = 44 ' Maps to LTL: LTL
            LTLR = 45 ' Maps to LTLR: LTLR
            INT = 46 ' Maps to Intermodal: Intermodal - Flat
            Par = 47 ' Maps to Parcel: Parcel Standard
            PNDA = 48 ' Maps to NextDayAM: Parcel Next Day AM
            PND = 49 ' Maps to NextDay: Parcel Next Day
            PGND = 50 ' Maps to Ground: Parcel Ground
            PGS = 51 ' Maps to GlobalSaver: Parcel Global Saver
            PGE = 52 ' Maps to GlobalExpress: Parcel Global Express
            PGEX = 53 ' Maps to GlobalExpedited: Parcel Global Expedited
            PEXP = 54 ' Maps to ExpressPlus: Parcel Express Plus
            P3D = 55 ' Maps to 3rdDay: Parcel 3rd Day
            P2DA = 56 ' Maps to 2ndDayAM: Parcel 2nd Day AM
            P2D = 57 ' Maps to 2ndDay: Parcel 2nd Day
            LTLG = 58 ' Maps to LTL G: LTL Guaranteed 
            LTGR = 59 ' Maps to LTL GR: LTL Guaranteed Ref
            LTLA = 60 ' Maps to LTL AM: LTL Guaranteed AM
            LTAR = 61 ' Maps to LTL AR: LTL Guaranteed AM Ref
            LTGF = 62 ' Maps to LTL GF: LTL Guaranteed Frz
            LTAF = 63 ' Maps to LTL AF: LTL Guaranteed AM Frz
            AAMD = 64  'Maps to Air AM: Air Next Day AM Dry
            ANDD = 65   'Maps to Air NextDay: Air Next Day Dry
            AAMR = 66  'Maps to Air AM R: Air Next Day AM Ref
            ANR = 67   'Maps to Air NextDay R: Air Next Day Ref'
            AAMF = 68  'Maps to Air AM F: Air Next Day AM Frz
            ANF = 69   'Maps to Air NextDay F: Air Next Day Frz
        End Enum



        '<DataContract()> _
        'Public Enum LoadTenderStatusCodeEnum
        '    <EnumMember(Value:="0")> None = 0
        '    <EnumMember(Value:="1")> Pending = 1
        '    <EnumMember(Value:="2")> EDI997Pass = 2
        '    <EnumMember(Value:="3")> EDI997Fail = 3
        '    <EnumMember(Value:="4")> DataValidationFail = 4
        '    <EnumMember(Value:="5")> UpdateSent = 5
        '    <EnumMember(Value:="6")> CancellationSent = 6
        '    <EnumMember(Value:="7")> ManualRetransmit = 7
        '    <EnumMember(Value:="8")> TransmissionStopped = 8
        '    <EnumMember(Value:="9")> EDI820Received = 9
        '    <EnumMember(Value:="10")> Archived = 10
        '    <EnumMember(Value:="11")> EDI990Accept = 11
        '    <EnumMember(Value:="12")> EDI990Reject = 12
        '    <EnumMember(Value:="13")> FreightBillReceived = 13
        '    <EnumMember(Value:="14")> EDI214ReceivedWithErrors = 14
        '    <EnumMember(Value:="15")> EDI214Received = 15
        '    <EnumMember(Value:="16")> Posted = 16
        '    <EnumMember(Value:="17")> PostExpired = 17
        '    <EnumMember(Value:="18")> PostDeleted = 18
        '    <EnumMember(Value:="19")> _Error = 19
        '    <EnumMember(Value:="20")> Accepted = 20
        '    <EnumMember(Value:="21")> Expired = 21
        '    <EnumMember(Value:="22")> Deleted = 22
        'End Enum

        '<DataContract()> _
        ' Public Enum LoadTenderTypeEnum
        '    <EnumMember(Value:="0")> None = 0
        '    <EnumMember(Value:="1")> Manual = 1
        '    <EnumMember(Value:="2")> EDI204 = 2
        '    <EnumMember(Value:="3")> Web = 3
        '    <EnumMember(Value:="4")> CPU = 4
        '    <EnumMember(Value:="5")> DAT = 5
        'End Enum

#End Region

#Region " Data Members "

        Private _LoadTenderControl As Integer = 0
        Friend Shared LoadTenderStatusCode As Object

        <DataMember()>
        Public Property LoadTenderControl() As Integer
            Get
                Return _LoadTenderControl
            End Get
            Set(ByVal value As Integer)
                _LoadTenderControl = value
            End Set
        End Property

        Private _LTLoadTenderTypeControl As Integer = 0
        <DataMember()>
        Public Property LTLoadTenderTypeControl() As Integer
            Get
                Return _LTLoadTenderTypeControl
            End Get
            Set(ByVal value As Integer)
                _LTLoadTenderTypeControl = value
            End Set
        End Property

        Private _LTCarrierControl As Integer = 0
        <DataMember()>
        Public Property LTCarrierControl() As Integer
            Get
                Return _LTCarrierControl
            End Get
            Set(ByVal value As Integer)
                _LTCarrierControl = value
            End Set
        End Property

        Private _LTCarrierSCAC As String = ""
        <DataMember()>
        Public Property LTCarrierSCAC() As String
            Get
                Return Left(_LTCarrierSCAC, 4)
            End Get
            Set(ByVal value As String)
                _LTCarrierSCAC = Left(value, 4)
            End Set
        End Property

        Private _LTCarrierNumber As Integer = 0
        <DataMember()>
        Public Property LTCarrierNumber() As Integer
            Get
                Return _LTCarrierNumber
            End Get
            Set(ByVal value As Integer)
                _LTCarrierNumber = value
            End Set
        End Property

        Private _LTCarrierName As String = ""
        <DataMember()>
        Public Property LTCarrierName() As String
            Get
                Return Left(_LTCarrierName, 40)
            End Get
            Set(ByVal value As String)
                _LTCarrierName = Left(value, 40)
            End Set
        End Property

        Private _LTBookControl As Integer = 0
        <DataMember()>
        Public Property LTBookControl() As Integer
            Get
                Return _LTBookControl
            End Get
            Set(ByVal value As Integer)
                _LTBookControl = value
            End Set
        End Property

        Private _LTBookProNumber As String = ""
        <DataMember()>
        Public Property LTBookProNumber() As String
            Get
                Return Left(_LTBookProNumber, 20)
            End Get
            Set(ByVal value As String)
                _LTBookProNumber = Left(value, 20)
            End Set
        End Property

        Private _LTBookConsPrefix As String = ""
        <DataMember()>
        Public Property LTBookConsPrefix() As String
            Get
                Return Left(_LTBookConsPrefix, 20)
            End Get
            Set(ByVal value As String)
                _LTBookConsPrefix = Left(value, 20)
            End Set
        End Property

        Private _LTBookSHID As String = ""
        <DataMember()>
        Public Property LTBookSHID() As String
            Get
                Return Left(_LTBookSHID, 50)
            End Get
            Set(ByVal value As String)
                _LTBookSHID = Left(value, 50)
            End Set
        End Property

        Private _LTBookStopNo As Short = 0
        <DataMember()>
        Public Property LTBookStopNo() As Short
            Get
                Return _LTBookStopNo
            End Get
            Set(ByVal value As Short)
                _LTBookStopNo = value
            End Set
        End Property

        Private _LTBookCarrOrderNumber As String = ""
        <DataMember()>
        Public Property LTBookCarrOrderNumber() As String
            Get
                Return Left(_LTBookCarrOrderNumber, 20)
            End Get
            Set(ByVal value As String)
                _LTBookCarrOrderNumber = Left(value, 20)
            End Set
        End Property

        Private _LTBookOrderSequence As Integer = 0
        <DataMember()>
        Public Property LTBookOrderSequence() As Integer
            Get
                Return _LTBookOrderSequence
            End Get
            Set(ByVal value As Integer)
                _LTBookOrderSequence = value
            End Set
        End Property

        Private _LTBookRouteFinalCode As String = ""
        <DataMember()>
        Public Property LTBookRouteFinalCode() As String
            Get
                Return Left(_LTBookRouteFinalCode, 2)
            End Get
            Set(ByVal value As String)
                _LTBookRouteFinalCode = Left(value, 2)
            End Set
        End Property

        Private _LTBookTransactionPurpose As String = ""
        <DataMember()>
        Public Property LTBookTransactionPurpose() As String
            Get
                Return Left(_LTBookTransactionPurpose, 2)
            End Get
            Set(ByVal value As String)
                _LTBookTransactionPurpose = Left(value, 2)
            End Set
        End Property

        Private _LTBookTotalCases As Integer = 0
        <DataMember()>
        Public Property LTBookTotalCases() As Integer
            Get
                Return _LTBookTotalCases
            End Get
            Set(ByVal value As Integer)
                _LTBookTotalCases = value
            End Set
        End Property

        Private _LTBookTotalWgt As Double = 0
        <DataMember()>
        Public Property LTBookTotalWgt() As Double
            Get
                Return _LTBookTotalWgt
            End Get
            Set(ByVal value As Double)
                _LTBookTotalWgt = value
            End Set
        End Property

        Private _LTBookTotalPL As Double = 0
        <DataMember()>
        Public Property LTBookTotalPL() As Double
            Get
                Return _LTBookTotalPL
            End Get
            Set(ByVal value As Double)
                _LTBookTotalPL = value
            End Set
        End Property

        Private _LTBookTotalCube As Integer = 0
        <DataMember()>
        Public Property LTBookTotalCube() As Integer
            Get
                Return _LTBookTotalCube
            End Get
            Set(ByVal value As Integer)
                _LTBookTotalCube = value
            End Set
        End Property

        Private _LTBookDateLoad As System.Nullable(Of Date)
        <DataMember()>
        Public Property LTBookDateLoad() As System.Nullable(Of Date)
            Get
                Return _LTBookDateLoad
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _LTBookDateLoad = value
            End Set
        End Property

        Private _LTBookDateRequired As System.Nullable(Of Date)
        <DataMember()>
        Public Property LTBookDateRequired() As System.Nullable(Of Date)
            Get
                Return _LTBookDateRequired
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _LTBookDateRequired = value
            End Set
        End Property

        Private _LTBookCarrScheduleDate As System.Nullable(Of Date)
        <DataMember()>
        Public Property LTBookCarrScheduleDate() As System.Nullable(Of Date)
            Get
                Return _LTBookCarrScheduleDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _LTBookCarrScheduleDate = value
            End Set
        End Property

        Private _LTBookCarrScheduleTime As System.Nullable(Of Date)
        <DataMember()>
        Public Property LTBookCarrScheduleTime() As System.Nullable(Of Date)
            Get
                Return _LTBookCarrScheduleTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _LTBookCarrScheduleTime = value
            End Set
        End Property

        Private _LTBookCarrApptDate As System.Nullable(Of Date)
        <DataMember()>
        Public Property LTBookCarrApptDate() As System.Nullable(Of Date)
            Get
                Return _LTBookCarrApptDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _LTBookCarrApptDate = value
            End Set
        End Property

        Private _LTBookCarrApptTime As System.Nullable(Of Date)
        <DataMember()>
        Public Property LTBookCarrApptTime() As System.Nullable(Of Date)
            Get
                Return _LTBookCarrApptTime
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _LTBookCarrApptTime = value
            End Set
        End Property

        Private _LTBookOrigName As String = ""
        <DataMember()>
        Public Property LTBookOrigName() As String
            Get
                Return Left(_LTBookOrigName, 40)
            End Get
            Set(ByVal value As String)
                _LTBookOrigName = Left(value, 40)
            End Set
        End Property

        Private _LTBookOrigAddress1 As String = ""
        <DataMember()>
        Public Property LTBookOrigAddress1() As String
            Get
                Return Left(_LTBookOrigAddress1, 40)
            End Get
            Set(ByVal value As String)
                _LTBookOrigAddress1 = Left(value, 40)
            End Set
        End Property

        Private _LTBookOrigAddress2 As String = ""
        <DataMember()>
        Public Property LTBookOrigAddress2() As String
            Get
                Return Left(_LTBookOrigAddress2, 40)
            End Get
            Set(ByVal value As String)
                _LTBookOrigAddress2 = Left(value, 40)
            End Set
        End Property

        Private _LTBookOrigAddress3 As String = ""
        <DataMember()>
        Public Property LTBookOrigAddress3() As String
            Get
                Return Left(_LTBookOrigAddress3, 40)
            End Get
            Set(ByVal value As String)
                _LTBookOrigAddress3 = Left(value, 40)
            End Set
        End Property

        Private _LTBookOrigCity As String = ""
        <DataMember()>
        Public Property LTBookOrigCity() As String
            Get
                Return Left(_LTBookOrigCity, 25)
            End Get
            Set(ByVal value As String)
                _LTBookOrigCity = Left(value, 25)
            End Set
        End Property

        Private _LTBookOrigState As String = ""
        <DataMember()>
        Public Property LTBookOrigState() As String
            Get
                Return Left(_LTBookOrigState, 8)
            End Get
            Set(ByVal value As String)
                _LTBookOrigState = Left(value, 8)
            End Set
        End Property

        Private _LTBookOrigCountry As String = ""
        <DataMember()>
        Public Property LTBookOrigCountry() As String
            Get
                Return Left(_LTBookOrigCountry, 30)
            End Get
            Set(ByVal value As String)
                _LTBookOrigCountry = Left(value, 30)
            End Set
        End Property

        Private _LTBookOrigZip As String = ""
        <DataMember()>
        Public Property LTBookOrigZip() As String
            Get
                Return Left(_LTBookOrigZip, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
            End Get
            Set(ByVal value As String)
                _LTBookOrigZip = Left(value, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
            End Set
        End Property

        Private _LTBookOrigPhone As String = ""
        <DataMember()>
        Public Property LTBookOrigPhone() As String
            Get
                Return Left(_LTBookOrigPhone, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
            End Get
            Set(ByVal value As String)
                _LTBookOrigPhone = Left(value, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
            End Set
        End Property

        Private _LTBookOrigIDENTIFICATIONCODEQUALIFIER As String = ""
        <DataMember()>
        Public Property LTBookOrigIDENTIFICATIONCODEQUALIFIER() As String
            Get
                Return Left(_LTBookOrigIDENTIFICATIONCODEQUALIFIER, 2)
            End Get
            Set(ByVal value As String)
                _LTBookOrigIDENTIFICATIONCODEQUALIFIER = Left(value, 2)
            End Set
        End Property

        Private _LTBookOrigCompanyNumber As String = ""
        <DataMember()>
        Public Property LTBookOrigCompanyNumber() As String
            Get
                Return Left(_LTBookOrigCompanyNumber, 50)
            End Get
            Set(ByVal value As String)
                _LTBookOrigCompanyNumber = Left(value, 50)
            End Set
        End Property

        Private _LTBookDestName As String = ""
        <DataMember()>
        Public Property LTBookDestName() As String
            Get
                Return Left(_LTBookDestName, 40)
            End Get
            Set(ByVal value As String)
                _LTBookDestName = Left(value, 40)
            End Set
        End Property

        Private _LTBookDestAddress1 As String = ""
        <DataMember()>
        Public Property LTBookDestAddress1() As String
            Get
                Return Left(_LTBookDestAddress1, 40)
            End Get
            Set(ByVal value As String)
                _LTBookDestAddress1 = Left(value, 40)
            End Set
        End Property

        Private _LTBookDestAddress2 As String = ""
        <DataMember()>
        Public Property LTBookDestAddress2() As String
            Get
                Return Left(_LTBookDestAddress2, 40)
            End Get
            Set(ByVal value As String)
                _LTBookDestAddress2 = Left(value, 40)
            End Set
        End Property

        Private _LTBookDestAddress3 As String = ""
        <DataMember()>
        Public Property LTBookDestAddress3() As String
            Get
                Return Left(_LTBookDestAddress3, 40)
            End Get
            Set(ByVal value As String)
                _LTBookDestAddress3 = Left(value, 40)
            End Set
        End Property

        Private _LTBookDestCity As String = ""
        <DataMember()>
        Public Property LTBookDestCity() As String
            Get
                Return Left(_LTBookDestCity, 25)
            End Get
            Set(ByVal value As String)
                _LTBookDestCity = Left(value, 25)
            End Set
        End Property

        Private _LTBookDestState As String = ""
        <DataMember()>
        Public Property LTBookDestState() As String
            Get
                Return Left(_LTBookDestState, 2)
            End Get
            Set(ByVal value As String)
                _LTBookDestState = Left(value, 2)
            End Set
        End Property

        Private _LTBookDestCountry As String = ""
        <DataMember()>
        Public Property LTBookDestCountry() As String
            Get
                Return Left(_LTBookDestCountry, 30)
            End Get
            Set(ByVal value As String)
                _LTBookDestCountry = Left(value, 30)
            End Set
        End Property

        Private _LTBookDestZip As String = ""
        <DataMember()>
        Public Property LTBookDestZip() As String
            Get
                Return Left(_LTBookDestZip, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
            End Get
            Set(ByVal value As String)
                _LTBookDestZip = Left(value, 20) 'Modified by RHR for v-8.4.003 on 06/25/2021
            End Set
        End Property

        Private _LTBookDestPhone As String = ""
        <DataMember()>
        Public Property LTBookDestPhone() As String
            Get
                Return Left(_LTBookDestPhone, 20)  'Modified by RHR for v-8.4.003 on 06/25/2021
            End Get
            Set(ByVal value As String)
                _LTBookDestPhone = Left(value, 20)  'Modified by RHR for v-8.4.003 on 06/25/2021
            End Set
        End Property

        Private _LTBookDestIDENTIFICATIONCODEQUALIFIER As String = ""
        <DataMember()>
        Public Property LTBookDestIDENTIFICATIONCODEQUALIFIER() As String
            Get
                Return Left(_LTBookDestIDENTIFICATIONCODEQUALIFIER, 2)
            End Get
            Set(ByVal value As String)
                _LTBookDestIDENTIFICATIONCODEQUALIFIER = Left(value, 2)
            End Set
        End Property

        Private _LTBookDestCompanyNumber As String = ""
        <DataMember()>
        Public Property LTBookDestCompanyNumber() As String
            Get
                Return Left(_LTBookDestCompanyNumber, 50)
            End Get
            Set(ByVal value As String)
                _LTBookDestCompanyNumber = Left(value, 50)
            End Set
        End Property

        Private _LTBookLoadPONumber As String = ""
        <DataMember()>
        Public Property LTBookLoadPONumber() As String
            Get
                Return Left(_LTBookLoadPONumber, 20)
            End Get
            Set(ByVal value As String)
                _LTBookLoadPONumber = Left(value, 20)
            End Set
        End Property

        Private _LTBookLoadCom As String
        <DataMember()>
        Public Property LTBookLoadCom() As String
            Get
                If Len(Trim(_LTBookLoadCom)) < 1 Then _LTBookLoadCom = "D"
                Return _LTBookLoadCom
            End Get
            Set(ByVal value As String)
                _LTBookLoadCom = value
            End Set
        End Property

        Private _LTCommCodeDescription As String = ""
        <DataMember()>
        Public Property LTCommCodeDescription() As String
            Get
                Return Left(_LTCommCodeDescription, 40)
            End Get
            Set(ByVal value As String)
                _LTCommCodeDescription = Left(value, 40)
            End Set
        End Property

        Private _LTLaneComments As String = ""
        <DataMember()>
        Public Property LTLaneComments() As String
            Get
                Return Left(_LTLaneComments, 255)
            End Get
            Set(ByVal value As String)
                _LTLaneComments = Left(value, 255)
            End Set
        End Property

        Private _LTLaneOriginAddressUse As Boolean = False
        <DataMember()>
        Public Property LTLaneOriginAddressUse() As Boolean
            Get
                Return _LTLaneOriginAddressUse
            End Get
            Set(ByVal value As Boolean)
                _LTLaneOriginAddressUse = value
            End Set
        End Property

        Private _LTBookTrackDate As System.Nullable(Of Date)
        <DataMember()>
        Public Property LTBookTrackDate() As System.Nullable(Of Date)
            Get
                Return _LTBookTrackDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _LTBookTrackDate = value
            End Set
        End Property

        Private _LTCompEDISecurityQual As String = ""
        <DataMember()>
        Public Property LTCompEDISecurityQual() As String
            Get
                Return Left(_LTCompEDISecurityQual, 2)
            End Get
            Set(ByVal value As String)
                _LTCompEDISecurityQual = Left(value, 2)
            End Set
        End Property

        Private _LTCompEDISecurityCode As String = ""
        <DataMember()>
        Public Property LTCompEDISecurityCode() As String
            Get
                Return Left(_LTCompEDISecurityCode, 10)
            End Get
            Set(ByVal value As String)
                _LTCompEDISecurityCode = Left(value, 10)
            End Set
        End Property

        Private _LTCompEDIPartnerQual As String = ""
        <DataMember()>
        Public Property LTCompEDIPartnerQual() As String
            Get
                Return Left(_LTCompEDIPartnerQual, 2)
            End Get
            Set(ByVal value As String)
                _LTCompEDIPartnerQual = Left(value, 2)
            End Set
        End Property

        Private _LTCompEDIPartnerCode As String = ""
        <DataMember()>
        Public Property LTCompEDIPartnerCode() As String
            Get
                Return Left(_LTCompEDIPartnerCode, 15)
            End Get
            Set(ByVal value As String)
                _LTCompEDIPartnerCode = Left(value, 15)
            End Set
        End Property

        Private _LTCompEDIEmailNotificationOn As Boolean = False
        <DataMember()>
        Public Property LTCompEDIEmailNotificationOn() As Boolean
            Get
                Return _LTCompEDIEmailNotificationOn
            End Get
            Set(ByVal value As Boolean)
                _LTCompEDIEmailNotificationOn = value
            End Set
        End Property

        Private _LTCompEDIEmailAddress As String = ""
        <DataMember()>
        Public Property LTCompEDIEmailAddress() As String
            Get
                Return Left(_LTCompEDIEmailAddress, 255)
            End Get
            Set(ByVal value As String)
                _LTCompEDIEmailAddress = Left(value, 255)
            End Set
        End Property

        Private _LTCompEDIAcknowledgementRequested As Boolean = False
        <DataMember()>
        Public Property LTCompEDIAcknowledgementRequested() As Boolean
            Get
                Return _LTCompEDIAcknowledgementRequested
            End Get
            Set(ByVal value As Boolean)
                _LTCompEDIAcknowledgementRequested = value
            End Set
        End Property

        Private _LTCompEDIMethodOfPayment As String = ""
        <DataMember()>
        Public Property LTCompEDIMethodOfPayment() As String
            Get
                Return Left(_LTCompEDIMethodOfPayment, 2)
            End Get
            Set(ByVal value As String)
                _LTCompEDIMethodOfPayment = Left(value, 2)
            End Set
        End Property

        Private _LTBookRouteConsFlag As Boolean = False
        <DataMember()>
        Public Property LTBookRouteConsFlag() As Boolean
            Get
                Return _LTBookRouteConsFlag
            End Get
            Set(ByVal value As Boolean)
                _LTBookRouteConsFlag = value
            End Set
        End Property

        Private _LTBookRevTotalCost As Decimal = 0
        <DataMember()>
        Public Property LTBookRevTotalCost() As Decimal
            Get
                Return _LTBookRevTotalCost
            End Get
            Set(ByVal value As Decimal)
                _LTBookRevTotalCost = value
            End Set
        End Property

        Private _LTBillToCompName As String = ""
        <DataMember()>
        Public Property LTBillToCompName() As String
            Get
                Return Left(_LTBillToCompName, 40)
            End Get
            Set(ByVal value As String)
                _LTBillToCompName = Left(value, 40)
            End Set
        End Property

        Private _LTBillToCompNumber As String = ""
        <DataMember()>
        Public Property LTBillToCompNumber() As String
            Get
                Return Left(_LTBillToCompNumber, 50)
            End Get
            Set(ByVal value As String)
                _LTBillToCompNumber = Left(value, 50)
            End Set
        End Property

        Private _LTBillToCompAddress1 As String = ""
        <DataMember()>
        Public Property LTBillToCompAddress1() As String
            Get
                Return Left(_LTBillToCompAddress1, 40)
            End Get
            Set(ByVal value As String)
                _LTBillToCompAddress1 = Left(value, 40)
            End Set
        End Property

        Private _LTBillToCompAddress2 As String = ""
        <DataMember()>
        Public Property LTBillToCompAddress2() As String
            Get
                Return Left(_LTBillToCompAddress2, 40)
            End Get
            Set(ByVal value As String)
                _LTBillToCompAddress2 = Left(value, 40)
            End Set
        End Property

        Private _LTBillToCompCity As String = ""
        <DataMember()>
        Public Property LTBillToCompCity() As String
            Get
                Return Left(_LTBillToCompCity, 25)
            End Get
            Set(ByVal value As String)
                _LTBillToCompCity = Left(value, 25)
            End Set
        End Property

        Private _LTBillToCompState As String = ""
        <DataMember()>
        Public Property LTBillToCompState() As String
            Get
                Return Left(_LTBillToCompState, 8)
            End Get
            Set(ByVal value As String)
                _LTBillToCompState = Left(value, 8)
            End Set
        End Property

        Private _LTBillToCompZip As String = ""
        <DataMember()>
        Public Property LTBillToCompZip() As String
            Get
                Return Left(_LTBillToCompZip, 10)
            End Get
            Set(ByVal value As String)
                _LTBillToCompZip = Left(value, 10)
            End Set
        End Property

        Private _LTBillToCompCountry As String = ""
        <DataMember()>
        Public Property LTBillToCompCountry() As String
            Get
                Return Left(_LTBillToCompCountry, 30)
            End Get
            Set(ByVal value As String)
                _LTBillToCompCountry = Left(value, 30)
            End Set
        End Property

        Private _LTEDICombineOrdersForStops As Decimal = 0
        <DataMember()>
        Public Property LTEDICombineOrdersForStops() As Decimal
            Get
                Return _LTEDICombineOrdersForStops
            End Get
            Set(ByVal value As Decimal)
                _LTEDICombineOrdersForStops = value
            End Set
        End Property

        Private _LTBookCustCompControl As Integer = 0
        <DataMember()>
        Public Property LTBookCustCompControl() As Integer
            Get
                Return _LTBookCustCompControl
            End Get
            Set(ByVal value As Integer)
                _LTBookCustCompControl = value
            End Set
        End Property

        Private _LTCompName As String = ""
        <DataMember()>
        Public Property LTCompName() As String
            Get
                Return Left(_LTCompName, 40)
            End Get
            Set(ByVal value As String)
                _LTCompName = Left(value, 40)
            End Set
        End Property

        'EDI 204 Maintenanace Screen fields
        Private _LT204FirstDateSent As System.Nullable(Of Date)
        <DataMember()>
        Public Property LT204FirstDateSent() As System.Nullable(Of Date)
            Get
                Return _LT204FirstDateSent
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _LT204FirstDateSent = value
            End Set
        End Property

        Private _LT204LastDateSent As System.Nullable(Of Date)
        <DataMember()>
        Public Property LT204LastDateSent() As System.Nullable(Of Date)
            Get
                Return _LT204LastDateSent
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _LT204LastDateSent = value
            End Set
        End Property

        Private _LT204997Received As Boolean = False
        <DataMember()>
        Public Property LT204997Received() As Boolean
            Get
                Return _LT204997Received
            End Get
            Set(ByVal value As Boolean)
                _LT204997Received = value
            End Set
        End Property

        Private _LT204997ReceivedDate As System.Nullable(Of Date)
        <DataMember()>
        Public Property LT204997ReceivedDate() As System.Nullable(Of Date)
            Get
                Return _LT204997ReceivedDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _LT204997ReceivedDate = value
            End Set
        End Property

        Private _LT990Received As Boolean = False
        <DataMember()>
        Public Property LT990Received() As Boolean
            Get
                Return _LT990Received
            End Get
            Set(ByVal value As Boolean)
                _LT990Received = value
            End Set
        End Property

        Private _LT990ReceivedDate As System.Nullable(Of Date)
        <DataMember()>
        Public Property LT990ReceivedDate() As System.Nullable(Of Date)
            Get
                Return _LT990ReceivedDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _LT990ReceivedDate = value
            End Set
        End Property

        Private _LT204Retry As Integer = 0
        <DataMember()>
        Public Property LT204Retry() As Integer
            Get
                Return _LT204Retry
            End Get
            Set(ByVal value As Integer)
                _LT204Retry = value
            End Set
        End Property

        Private _LTStatusCode As Integer? = 0
        <DataMember()>
        Public Property LTStatusCode() As Integer?
            Get
                Return _LTStatusCode
            End Get
            Set(ByVal value As Integer?)
                _LTStatusCode = value
            End Set
        End Property

        Private _LTMessage As String = ""
        <DataMember()>
        Public Property LTMessage() As String
            Get
                Return _LTMessage
            End Get
            Set(ByVal value As String)
                _LTMessage = value
            End Set
        End Property

        Private _LT204FileName204 As String = ""
        <DataMember()>
        Public Property LT204FileName204() As String
            Get
                Return Left(_LT204FileName204, 255)
            End Get
            Set(ByVal value As String)
                _LT204FileName204 = Left(value, 255)
                'Me.SendPropertyChanged("LT204FileName204")
            End Set
        End Property

        Private _LT204FileName997 As String = ""
        <DataMember()>
        Public Property LT204FileName997() As String
            Get
                Return Left(_LT204FileName997, 255)
            End Get
            Set(ByVal value As String)
                _LT204FileName997 = Left(value, 255)
                'Me.SendPropertyChanged("LT204FileName997")
            End Set
        End Property

        Private _LT204FileName990 As String = ""
        <DataMember()>
        Public Property LT204FileName990() As String
            Get
                Return Left(_LT204FileName990, 255)
            End Get
            Set(ByVal value As String)
                _LT204FileName990 = Left(value, 255)
                'Me.SendPropertyChanged("LT204FileName990")
            End Set
        End Property

        Private _LT204GS06 As Integer = 0
        <DataMember()>
        Public Property LT204GS06() As Integer
            Get
                Return _LT204GS06
            End Get
            Set(ByVal value As Integer)
                _LT204GS06 = value
            End Set
        End Property

        Private _LTArchived As Boolean = False
        <DataMember()>
        Public Property LTArchived() As Boolean
            Get
                Return _LTArchived
            End Get
            Set(ByVal value As Boolean)
                _LTArchived = value
            End Set
        End Property

        'DAT specific Fields

        Private _LTDATRefID As String = ""
        <DataMember()>
        Public Property LTDATRefID() As String
            Get
                Return Left(_LTDATRefID, 8)
            End Get
            Set(ByVal value As String)
                _LTDATRefID = Left(value, 8)
            End Set
        End Property

        Private _LTDATEquipType As String = ""
        <DataMember()>
        Public Property LTDATEquipType() As String
            Get
                Return Left(_LTDATEquipType, 50)
            End Get
            Set(ByVal value As String)
                _LTDATEquipType = Left(value, 50)
            End Set
        End Property

        Private _LTDATComment1 As String = ""
        <DataMember()>
        Public Property LTDATComment1() As String
            Get
                Return Left(_LTDATComment1, 70)
            End Get
            Set(ByVal value As String)
                _LTDATComment1 = Left(value, 70)
            End Set
        End Property

        Private _LTDATComment2 As String = ""
        <DataMember()>
        Public Property LTDATComment2() As String
            Get
                Return Left(_LTDATComment2, 70)
            End Get
            Set(ByVal value As String)
                _LTDATComment2 = Left(value, 70)
            End Set
        End Property

        Private _LTDATEarliestAvailable As System.Nullable(Of Date)
        <DataMember()>
        Public Property LTDATEarliestAvailable() As System.Nullable(Of Date)
            Get
                Return _LTDATEarliestAvailable
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _LTDATEarliestAvailable = value
            End Set
        End Property

        Private _LTDATLastestAvailable As System.Nullable(Of Date)
        <DataMember()>
        Public Property LTDATLastestAvailable() As System.Nullable(Of Date)
            Get
                Return _LTDATLastestAvailable
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _LTDATLastestAvailable = value
            End Set
        End Property

        Private _LTDATPoster As String = ""
        <DataMember()>
        Public Property LTDATPoster() As String
            Get
                Return Left(_LTDATPoster, 100)
            End Get
            Set(ByVal value As String)
                _LTDATPoster = Left(value, 100)
            End Set
        End Property

        Private _LTPosterUserControl As Integer = 0
        <DataMember()>
        Public Property LTPosterUserControl() As Integer
            Get
                Return _LTPosterUserControl
            End Get
            Set(ByVal value As Integer)
                _LTPosterUserControl = value
            End Set
        End Property

        Private _LTTenderedDate As System.Nullable(Of Date)
        <DataMember()>
        Public Property LTTenderedDate() As System.Nullable(Of Date)
            Get
                Return _LTTenderedDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _LTTenderedDate = value
            End Set
        End Property

        Private _LTExpires As System.Nullable(Of Date)
        <DataMember()>
        Public Property LTExpires() As System.Nullable(Of Date)
            Get
                Return _LTExpires
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _LTExpires = value
            End Set
        End Property

        Private _LTExpired As Boolean = False
        <DataMember()>
        Public Property LTExpired() As Boolean
            Get
                Return _LTExpired
            End Get
            Set(ByVal value As Boolean)
                _LTExpired = value
            End Set
        End Property

        Private _LTBookTotalMiles As Double = 0
        <DataMember()>
        Public Property LTBookTotalMiles() As Double
            Get
                Return _LTBookTotalMiles
            End Get
            Set(ByVal value As Double)
                _LTBookTotalMiles = value
            End Set
        End Property

        Private _LTModDate As System.Nullable(Of Date)
        <DataMember()>
        Public Property LTModDate() As System.Nullable(Of Date)
            Get
                Return _LTModDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _LTModDate = value
                'Me.SendPropertyChanged("LTModDate")
            End Set
        End Property

        Private _LTModUser As String = ""
        <DataMember()>
        Public Property LTModUser() As String
            Get
                Return Left(_LTModUser, 100)
            End Get
            Set(ByVal value As String)
                _LTModUser = Left(value, 100)
                'Me.SendPropertyChanged("LTModUser")
            End Set
        End Property

        Private _LTUpdated As Byte()
        <DataMember()>
        Public Property LTUpdated() As Byte()
            Get
                Return _LTUpdated
            End Get
            Set(ByVal value As Byte())
                _LTUpdated = value
            End Set
        End Property

        'Begin Modified by RHR for v-8.1 on 3/23/2018
        Private _LTBookOrigContactName As String
        <DataMember()>
        Public Property LTBookOrigContactName() As String
            Get
                Return Left(_LTBookOrigContactName, 50)
            End Get
            Set(ByVal value As String)
                _LTBookOrigContactName = Left(value, 50)
            End Set
        End Property

        Private _LTBookOrigContactEmail As String
        <DataMember()>
        Public Property LTBookOrigContactEmail() As String
            Get
                Return Left(_LTBookOrigContactEmail, 50)
            End Get
            Set(ByVal value As String)
                _LTBookOrigContactEmail = Left(value, 50)
            End Set
        End Property

        Private _LTBookOrigEmergencyContactPhone As String
        <DataMember()>
        Public Property LTBookOrigEmergencyContactPhone() As String
            Get
                Return Left(_LTBookOrigEmergencyContactPhone, 20) ' Modified by RHR for v-8.4.003 on 06/25/2021
            End Get
            Set(ByVal value As String)
                _LTBookOrigEmergencyContactPhone = Left(value, 20) ' Modified by RHR for v-8.4.003 on 06/25/2021
            End Set
        End Property

        Private _LTBookOrigEmergencyContactName As String
        <DataMember()>
        Public Property LTBookOrigEmergencyContactName() As String
            Get
                Return Left(_LTBookOrigEmergencyContactName, 50)
            End Get
            Set(ByVal value As String)
                _LTBookOrigEmergencyContactName = Left(value, 50)
            End Set
        End Property

        Private _LTBookDestContactName As String
        <DataMember()>
        Public Property LTBookDestContactName() As String
            Get
                Return Left(_LTBookDestContactName, 50)
            End Get
            Set(ByVal value As String)
                _LTBookDestContactName = Left(value, 50)
            End Set
        End Property

        Private _LTBookDestContactEmail As String
        <DataMember()>
        Public Property LTBookDestContactEmail() As String
            Get
                Return Left(_LTBookDestContactEmail, 50)
            End Get
            Set(ByVal value As String)
                _LTBookDestContactEmail = Left(value, 50)
            End Set
        End Property

        Private _LTBookDestEmergencyContactPhone As String
        <DataMember()>
        Public Property LTBookDestEmergencyContactPhone() As String
            Get
                Return Left(_LTBookDestEmergencyContactPhone, 20) ' Modified by RHR for v-8.4.003 on 06/25/2021
            End Get
            Set(ByVal value As String)
                _LTBookDestEmergencyContactPhone = Left(value, 20) ' Modified by RHR for v-8.4.003 on 06/25/2021
            End Set
        End Property

        Private _LTBookDestEmergencyContactName As String
        <DataMember()>
        Public Property LTBookDestEmergencyContactName() As String
            Get
                Return Left(_LTBookDestEmergencyContactName, 50)
            End Get
            Set(ByVal value As String)
                _LTBookDestEmergencyContactName = Left(value, 50)
            End Set
        End Property

        'P44 dispatch fields
        Private _LTSIBILLOFLADING As String
        <DataMember()>
        Public Property LTSIBILLOFLADING() As String
            Get
                Return Left(_LTSIBILLOFLADING, 100)
            End Get
            Set(ByVal value As String)
                _LTSIBILLOFLADING = Left(value, 100)
            End Set
        End Property

        Private _LTSIPURCHASEORDER As String
        <DataMember()>
        Public Property LTSIPURCHASEORDER() As String
            Get
                Return Left(_LTSIPURCHASEORDER, 100)
            End Get
            Set(ByVal value As String)
                _LTSIPURCHASEORDER = Left(value, 100)
            End Set
        End Property

        Private _LTSICUSTOMERREFERENCE As String
        <DataMember()>
        Public Property LTSICUSTOMERREFERENCE() As String
            Get
                Return Left(_LTSICUSTOMERREFERENCE, 100)
            End Get
            Set(ByVal value As String)
                _LTSICUSTOMERREFERENCE = Left(value, 100)
            End Set
        End Property

        Private _LTSIPICKUP As String
        <DataMember()>
        Public Property LTSIPICKUP() As String
            Get
                Return Left(_LTSIPICKUP, 100)
            End Get
            Set(ByVal value As String)
                _LTSIPICKUP = Left(value, 100)
            End Set
        End Property

        Private _LTSIPRO As String
        <DataMember()>
        Public Property LTSIPRO() As String
            Get
                Return Left(_LTSIPRO, 100)
            End Get
            Set(ByVal value As String)
                _LTSIPRO = Left(value, 100)
            End Set
        End Property

        Private _LTSIEXTERNAL As String
        <DataMember()>
        Public Property LTSIEXTERNAL() As String
            Get
                Return Left(_LTSIEXTERNAL, 100)
            End Get
            Set(ByVal value As String)
                _LTSIEXTERNAL = Left(value, 100)
            End Set
        End Property

        Private _LTSISYSTEMGENERATED As String
        <DataMember()>
        Public Property LTSISYSTEMGENERATED() As String
            Get
                Return Left(_LTSISYSTEMGENERATED, 100)
            End Get
            Set(ByVal value As String)
                _LTSISYSTEMGENERATED = Left(value, 100)
            End Set
        End Property

        Private _LTSICapacityProviderBolUrl As String
        <DataMember()>
        Public Property LTSICapacityProviderBolUrl() As String
            Get
                Return Left(_LTSICapacityProviderBolUrl, 1000)
            End Get
            Set(ByVal value As String)
                _LTSICapacityProviderBolUrl = Left(value, 1000)
            End Set
        End Property

        Private _LTSIPackingVisualizationUrl As String
        <DataMember()>
        Public Property LTSIPackingVisualizationUrl() As String
            Get
                Return Left(_LTSIPackingVisualizationUrl, 1000)
            End Get
            Set(ByVal value As String)
                _LTSIPackingVisualizationUrl = Left(value, 1000)
            End Set
        End Property

        Private _LTSIPickupNote As String
        <DataMember()>
        Public Property LTSIPickupNote() As String
            Get
                Return Left(_LTSIPickupNote, 4000)
            End Get
            Set(ByVal value As String)
                _LTSIPickupNote = Left(value, 4000)
            End Set
        End Property

        Private _LTSIPickupDateTime As Date?
        <DataMember()>
        Public Property LTSIPickupDateTime() As Date?
            Get
                Return _LTSIPickupDateTime
            End Get
            Set(ByVal value As Date?)
                _LTSIPickupDateTime = value
            End Set
        End Property


        Private _LTLTTTControl As Integer
        ''' <summary>
        ''' Load Tender Type Control maps to values like: 1 for Outbound, 2 for Transfer, or 3 for Inbound
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' A reference to tblLoadTenderTransType which holds a list of tran types for Load Tenders:
        ''' Inbound, Outbound, or Transfers.  Each tblLoadTenderTransType has a 1 to 1 relationship 
        ''' with a LaneTransNumber,  in v-8.1 these values are not user configurable; the mapping is:]
        ''' LaneTransNumber 4, Outbound Vendor Delivered, equals 1 Outbound
        ''' LaneTransNumber 7, Transfer, equals 2 transfer
        ''' LaneTransNumber 8, Inbound FOB Vendor Dock, equals 3 Inbound
        ''' Used to create company And lane records for dispated loads from ngl api rate shopping
        ''' </remarks>
        <DataMember()>
        Public Property LTLTTTControl() As Integer
            Get
                Return _LTLTTTControl
            End Get
            Set(ByVal value As Integer)
                _LTLTTTControl = value
            End Set
        End Property


        Private _LTLLaneWeightUnit As String

        ''' <summary>
        ''' values Like "LB" "KG"
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Populate from Javascript array in NGLObject.js, no lookup table needed. Default value is LB
        ''' but may be linked to a company/legal entity setting in the future.
        ''' </remarks>
        <DataMember()>
        Public Property LTLLaneWeightUnit() As String
            Get
                Return Left(_LTLLaneWeightUnit, 4)
            End Get
            Set(ByVal value As String)
                _LTLLaneWeightUnit = Left(value, 4)
            End Set
        End Property

        Private _LTLLaneLengthUnit As String
        ''' <summary>
        ''' Values like "IN" "CM" "FT" "M" 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Populate from Javascript array in NGLObject.js, no lookup table needed. Default value is "IN"
        ''' but may be linked to a company/legal entity setting in the future.
        ''' </remarks>
        <DataMember()>
        Public Property LTLLaneLengthUnit() As String
            Get
                Return Left(_LTLLaneLengthUnit, 4)
            End Get
            Set(ByVal value As String)
                _LTLLaneLengthUnit = Left(value, 4)
            End Set
        End Property

        Private _LTLAPIPickupNote As String
        ''' <summary>
        ''' Maps To BookNotesVisible1
        ''' </summary>
        ''' <returns></returns>
        <DataMember()>
        Public Property LTLAPIPickupNote() As String
            Get
                Return Left(_LTLAPIPickupNote, 255)
            End Get
            Set(ByVal value As String)
                _LTLAPIPickupNote = Left(value, 255)
            End Set
        End Property

        Private _LTLAPIQuoteNumber As String
        ''' <summary>
        ''' NGL API Quote Number from Rate Shopping
        ''' </summary>
        ''' <returns></returns>
        <DataMember()>
        Public Property LTLAPIQuoteNumber() As String
            Get
                Return Left(_LTLAPIQuoteNumber, 100)
            End Get
            Set(ByVal value As String)
                _LTLAPIQuoteNumber = Left(value, 100)
            End Set
        End Property
        'End Modified by RHR for v-8.1 on 3/23/2018

        'Begin Modified by RHR for v-8.5.4.001 on 7/17/2023  added missing data elements

        Private _LTBookCarrBookDate As Date?
        <DataMember()>
        Public Property LTBookCarrBookDate() As Date?
            Get
                Return _LTBookCarrBookDate
            End Get
            Set(ByVal value As Date?)
                _LTBookCarrBookDate = value
            End Set
        End Property

        Private _LTBookCarrBookTime As Date?
        <DataMember()>
        Public Property LTBookCarrBookTime() As Date?
            Get
                Return _LTBookCarrBookTime
            End Get
            Set(ByVal value As Date?)
                _LTBookCarrBookTime = value
            End Set
        End Property

        Private _LTBookCarrBookEndTime As Date?
        <DataMember()>
        Public Property LTBookCarrBookEndTime() As Date?
            Get
                Return _LTBookCarrBookEndTime
            End Get
            Set(ByVal value As Date?)
                _LTBookCarrBookEndTime = value
            End Set
        End Property

        Private _LTBookCarrPODate As Date?
        <DataMember()>
        Public Property LTBookCarrPODate() As Date?
            Get
                Return _LTBookCarrPODate
            End Get
            Set(ByVal value As Date?)
                _LTBookCarrPODate = value
            End Set
        End Property

        Private _LTBookCarrPOTime As Date?
        <DataMember()>
        Public Property LTBookCarrPOTime() As Date?
            Get
                Return _LTBookCarrPOTime
            End Get
            Set(ByVal value As Date?)
                _LTBookCarrPOTime = value
            End Set
        End Property

        Private _LTBookCarrPOEndTime As Date?
        <DataMember()>
        Public Property LTBookCarrPOEndTime() As Date?
            Get
                Return _LTBookCarrPOEndTime
            End Get
            Set(ByVal value As Date?)
                _LTBookCarrPOEndTime = value
            End Set
        End Property

        Private _LTLaneCommentsConfidential As String
        <DataMember()>
        Public Property LTLaneCommentsConfidential() As String
            Get
                Return Left(_LTLaneCommentsConfidential, 255)
            End Get
            Set(ByVal value As String)
                _LTLaneCommentsConfidential = Left(value, 255)
            End Set
        End Property

        Private _LTBookShipCarrierProNumber As String
        <DataMember()>
        Public Property LTBookShipCarrierProNumber() As String
            Get
                Return Left(_LTBookShipCarrierProNumber, 20)
            End Get
            Set(ByVal value As String)
                _LTBookShipCarrierProNumber = Left(value, 20)
            End Set
        End Property

        Private _LTBookShipCarrierName As String
        <DataMember()>
        Public Property LTBookShipCarrierName() As String
            Get
                Return Left(_LTBookShipCarrierName, 60)
            End Get
            Set(ByVal value As String)
                _LTBookShipCarrierName = Left(value, 60)
            End Set
        End Property

        Private _LTBookShipCarrierNumber As String
        <DataMember()>
        Public Property LTBookShipCarrierNumber() As String
            Get
                Return Left(_LTBookShipCarrierNumber, 80)
            End Get
            Set(ByVal value As String)
                _LTBookShipCarrierNumber = Left(value, 80)
            End Set
        End Property

        Private _LTLinearFeet As Integer = 0
        <DataMember()>
        Public Property LTLinearFeet() As Integer
            Get
                Return _LTLinearFeet
            End Get
            Set(ByVal value As Integer)
                _LTLinearFeet = value
            End Set
        End Property

        Private _LTBookDestCompControl As Integer = 0
        <DataMember()>
        Public Property LTBookDestCompControl() As Integer
            Get
                Return _LTBookDestCompControl
            End Get
            Set(ByVal value As Integer)
                _LTBookDestCompControl = value
            End Set
        End Property

        Private _LTBookOrigCompControl As Integer = 0
        <DataMember()>
        Public Property LTBookOrigCompControl() As Integer
            Get
                Return _LTBookOrigCompControl
            End Get
            Set(ByVal value As Integer)
                _LTBookOrigCompControl = value
            End Set
        End Property

        Private _LTNotes As String
        <DataMember()>
        Public Property LTNotes() As String
            Get
                Return _LTNotes
            End Get
            Set(ByVal value As String)
                _LTNotes = value
            End Set
        End Property

        Private _LTSelectedForExport As Boolean?
        <DataMember()>
        Public Property LTSelectedForExport() As Boolean?
            Get
                Return _LTSelectedForExport
            End Get
            Set(ByVal value As Boolean?)
                _LTSelectedForExport = value
            End Set
        End Property

        'End Modified by RHR for v-8.5.4.001 on 7/17/2023  added missing data elements

        'ADD FIELDS THAT NEED TO GET PASSED TO DAT POST METHOD (LOGIN INFO, ETC.)

        Private _SSOAUserName As String = ""
        <DataMember()>
        Public Property SSOAUserName() As String
            Get
                Return Left(_SSOAUserName, 255)
            End Get
            Set(ByVal value As String)
                _SSOAUserName = Left(value, 255)
            End Set
        End Property

        Private _SSOAPassword As String = ""
        <DataMember()>
        Public Property SSOAPassword() As String
            Get
                Return Left(_SSOAPassword, 100)
            End Get
            Set(ByVal value As String)
                _SSOAPassword = Left(value, 100)
            End Set
        End Property

        Private _TokenString As String = ""
        <DataMember()>
        Public Property TokenString() As String
            Get
                Return _TokenString
            End Get
            Set(ByVal value As String)
                _TokenString = value
            End Set
        End Property

        Private _TokenExpiresDate As System.Nullable(Of Date)
        <DataMember()>
        Public Property TokenExpiresDate() As System.Nullable(Of Date)
            Get
                Return _TokenExpiresDate
            End Get
            Set(ByVal value As System.Nullable(Of Date))
                _TokenExpiresDate = value
            End Set
        End Property

        'This is an enum used by the DAT dll to identify action (post, login etc.)
        'Login,
        'Post, = 1
        'Search,
        'LookupCarrier,
        'LookupRates,
        'Alarm,
        'LookupDobCarriers,
        'LookupDobEvents,
        'LookupSignedCarriers
        Private _DATFeature As Integer = 0
        <DataMember()>
        Public Property DATFeature() As Integer
            Get
                Return _DATFeature
            End Get
            Set(ByVal value As Integer)
                _DATFeature = value
            End Set
        End Property

        Private _Database As String = ""
        <DataMember()>
        Public Property Database() As String
            Get
                If String.IsNullOrEmpty(_Database) Then _Database = ""
                Return _Database
            End Get
            Set(ByVal value As String)
                _Database = value
            End Set
        End Property

        Private _DBServer As String = ""
        <DataMember()>
        Public Property DBServer() As String
            Get
                If String.IsNullOrEmpty(_DBServer) Then _DBServer = ""
                Return _DBServer
            End Get
            Set(ByVal value As String)
                _DBServer = value
            End Set
        End Property

        Private _ConnectionString As String = ""
        <DataMember()>
        Public Property ConnectionString() As String
            Get
                If String.IsNullOrEmpty(_ConnectionString) Then _ConnectionString = ""
                Return _ConnectionString
            End Get
            Set(ByVal value As String)
                _ConnectionString = value
            End Set
        End Property

        Private _UserName As String = ""
        <DataMember()>
        Public Property UserName() As String
            Get
                If String.IsNullOrEmpty(_UserName) Then _UserName = ""
                Return _UserName
            End Get
            Set(ByVal value As String)
                _UserName = value
            End Set
        End Property

        Private _UserSecurityControl As Integer = 0
        <DataMember()>
        Public Property UserSecurityControl() As Integer
            Get
                Return _UserSecurityControl
            End Get
            Set(ByVal value As Integer)
                _UserSecurityControl = value
            End Set
        End Property

        Private _SSOALoginURL As String
        <DataMember()>
        Public Property SSOALoginURL() As String
            Get
                Return Left(_SSOALoginURL, 1000)
            End Get
            Set(ByVal value As String)
                _SSOALoginURL = Left(value, 1000)
            End Set
        End Property

        'Private _LTStatusEnum As LoadTenderStatusCodeEnum
        '<DataMember()> _
        'Public Property LTStatusEnum() As LoadTenderStatusCodeEnum
        '    Get
        '        Return _LTStatusEnum
        '    End Get
        '    Set(ByVal value As LoadTenderStatusCodeEnum)
        '        _LTStatusEnum = value
        '    End Set
        'End Property



#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New tblLoadTender
            instance = DirectCast(MemberwiseClone(), tblLoadTender)
            Return instance
        End Function

        ''' <summary>
        ''' Get the name of the status code using the APIStatusCodeEnum
        ''' </summary>
        ''' <param name="eType"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' Modified by RHR for v-8.5.3.007 on 03/29/2023
        '''   fixed bug for parameter eType was using BidTypeEnum but should have 
        '''   been using APIStatusCodeEnum
        ''' </remarks>
        Public Shared Function GetAPIStatusCodeName(ByVal eType As APIStatusCodeEnum) As String
            Dim sRet As String = "Dry53"
            Select Case CInt(eType)
                Case 0 : sRet = "Spot"
                Case 1 : sRet = "DRY48"
                Case 2 : sRet = "FRZ48"
                Case 3 : sRet = "REF48"
                Case 4 : sRet = "DRY53"
                Case 5 : sRet = "FRZ53"
                Case 6 : sRet = "REF53"
                Case 7 : sRet = "LD3"
                Case 8 : sRet = "FB"
                Case 9 : sRet = "1 X 40 HC"
                Case 10 : sRet = "1 X 40"
                Case 11 : sRet = "1 X 20"
                Case 12 : sRet = "1 X 20 REF"
                Case 13 : sRet = "1 X 40 REF"
                Case 14 : sRet = "Rail"
                Case 15 : sRet = "Rail20"
                Case 16 : sRet = "Rail40"
                Case 17 : sRet = "Rail53"
                Case 18 : sRet = "Rail48"
                Case 19 : sRet = "RailREF"
                Case 20 : sRet = "Rail20REF"
                Case 21 : sRet = "Rail40REF"
                Case 22 : sRet = "Rail53REF"
                Case 23 : sRet = "Rail48REF"
                Case 24 : sRet = "RailFRZ"
                Case 25 : sRet = "Rail20FRZ"
                Case 26 : sRet = "Rail40FRZ"
                Case 27 : sRet = "Rail53FRZ"
                Case 28 : sRet = "Rail48FRZ"
                Case 29 : sRet = "Unassigned"
                Case 30 : sRet = "SprinterVan"
                Case 31 : sRet = "Tank"
                Case 32 : sRet = "Ocean53"
                Case 33 : sRet = "Ocean48"
                Case 34 : sRet = "Ocean40"
                Case 35 : sRet = "Ocean20"
                Case 36 : sRet = "Ocean53REF"
                Case 37 : sRet = "Ocean48REF"
                Case 38 : sRet = "Ocean40REF"
                Case 39 : sRet = "Ocean20REF"
                Case 40 : sRet = "Ocean53FRZ"
                Case 41 : sRet = "Ocean48FRZ"
                Case 42 : sRet = "Ocean40FRZ"
                Case 43 : sRet = "Ocean20FRZ"
                Case 44 : sRet = "LTL"
                Case 45 : sRet = "LTLR"
                Case 46 : sRet = "Intermodal"
                Case 47 : sRet = "Parcel"
                Case 48 : sRet = "NextDayAM"
                Case 49 : sRet = "NextDay"
                Case 50 : sRet = "Ground"
                Case 51 : sRet = "GlobalSaver"
                Case 52 : sRet = "GlobalExpress"
                Case 53 : sRet = "GlobalExpedited"
                Case 54 : sRet = "ExpressPlus"
                Case 55 : sRet = "3rdDay"
                Case 56 : sRet = "2ndDayAM"
                Case 57 : sRet = "2ndDay"
                Case 58 : sRet = "LTL G"
                Case 59 : sRet = "LTL GR"
                Case 60 : sRet = "LTL AM"
                Case 61 : sRet = "LTL AR"
                Case 62 : sRet = "LTL GF"
                Case 63 : sRet = "LTL AF"
                Case Else : sRet = "Dry53"
            End Select

            Return sRet

        End Function


        Public Shared Function GetAPICarrierServiceReference(ByVal eTypeCode As Ngl.FreightMaster.Data.DataTransferObjects.tblLoadTender.BidTypeEnum, ByVal sBidServiceType As String) As Ngl.FreightMaster.Data.DataTransferObjects.tblLoadTender.APIStatusCodeEnum
            Dim eReturn As APIStatusCodeEnum = Ngl.FreightMaster.Data.DataTransferObjects.tblLoadTender.APIStatusCodeEnum.None
            'Note: when eReturn is None (0) the spInsertPickList50 procedure will assign the value based on the following
            '   (a) look up the service reference code using the BookCarrTarrEquipmentControl
            '   (b) Set to a default value of Spot when the equipment control is zero or does not exist
            ' as of v-8.5.3.005 all none API rates will return the default of None
            Select Case eTypeCode
                Case BidTypeEnum.P44 ' P44
                    Select Case sBidServiceType
                        Case "Standard Rate"
                            eReturn = APIStatusCodeEnum.LTL
                        Case "Guaranteed"
                            eReturn = APIStatusCodeEnum.LTLG
                        Case "Guaranteed by AM (Noon)"
                            eReturn = APIStatusCodeEnum.LTLA
                        Case Else
                            eReturn = APIStatusCodeEnum.LTL
                    End Select
                Case BidTypeEnum.CHRAPI ' CHR
                    eReturn = APIStatusCodeEnum.D53
                Case BidTypeEnum.UPSAPI ' UPS
                    Select Case sBidServiceType
                        Case "UPSNextDayAir"
                            eReturn = APIStatusCodeEnum.PND
                        Case "UPS2ndDayAir"
                            eReturn = APIStatusCodeEnum.P2D
                        Case "UPSGround"
                            eReturn = APIStatusCodeEnum.PGND
                        Case "UPSWorldwideExpress"
                            eReturn = APIStatusCodeEnum.PGE
                        Case "UPSWorldwideExpedited"
                            eReturn = APIStatusCodeEnum.PGEX
                        Case "UPSStandard"
                            eReturn = APIStatusCodeEnum.Par
                        Case "UPS3DaySelect"
                            eReturn = APIStatusCodeEnum.P3D
                        Case "UPSNextDayAirSaver"
                            eReturn = APIStatusCodeEnum.PND
                        Case "UPSNextDayAirEarly"
                            eReturn = APIStatusCodeEnum.PNDA
                        Case "UPSWorldwideExpressPlus"
                            eReturn = APIStatusCodeEnum.PEXP
                        Case "UPS2ndDayAirAM"
                            eReturn = APIStatusCodeEnum.P2DA
                        Case "UPSWorldwideSaver"
                            eReturn = APIStatusCodeEnum.PGS
                        Case Else
                            eReturn = APIStatusCodeEnum.Par
                    End Select
                Case BidTypeEnum.YRCAPI ' YRC
                    eReturn = APIStatusCodeEnum.LTL
                Case BidTypeEnum.JTSAPI ' JTS
                    eReturn = APIStatusCodeEnum.LTL
                Case BidTypeEnum.FedXAPI ' FedX
                    eReturn = APIStatusCodeEnum.LTL
                Case Else
                    eReturn = APIStatusCodeEnum.None
            End Select

            Return eReturn

        End Function


        ''' <summary>
        ''' use BidTypeEnum to return a matching description that can be used as CarrTarName
        ''' </summary>
        ''' <param name="eTypeCode"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' Not currently being used?  some code appears to be lost?
        ''' </remarks>
        Public Shared Function GetBookCarrTarNameForBidTypeEnum(ByVal eTypeCode As BidTypeEnum) As String
            Dim sRet As String = "API Rate"
            Select Case eTypeCode
                Case BidTypeEnum.None ' None
                    sRet = "API RATE"
                Case BidTypeEnum.NextStop ' NEXTStop
                    sRet = "NEXTStop"
                Case BidTypeEnum.NGLTariff ' Tariff
                    sRet = "Tariff"
                Case BidTypeEnum.P44 ' P44
                    sRet = "API RATE"
                Case BidTypeEnum.Spot ' Spot
                    sRet = "Spot"
                Case BidTypeEnum.CHRAPI ' CHR
                    sRet = "CHR"
                Case BidTypeEnum.UPSAPI ' UPS
                    sRet = "UPS"
                Case BidTypeEnum.YRCAPI ' YRC
                    sRet = "YRC"
                Case BidTypeEnum.JTSAPI ' JTS
                    sRet = "JTS"
                Case BidTypeEnum.FedXAPI ' FedX
                    sRet = "FedEx"
                Case Else
                    sRet = "Spot"
            End Select

            Return sRet
        End Function



#End Region

    End Class
End Namespace
