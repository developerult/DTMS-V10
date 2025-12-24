Imports Map = Ngl.API.Mapping
Imports DTran = Ngl.Core.Utility.DataTransformation
Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects

Namespace Models

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks>
    ''' Added By LVV On 10/10/17 For v-8.0 TMS365 
    ''' Modified by RHR for v-8.2 on 7/5/2019 
    '''   Added the StopSequence property; a new field with a Default of -1. 
    '''     When the value is -1 the caller must lookup the value using the BookControl property if it exists.
    '''     If it does not exist the caller must set the value to 0.  
    '''     This data maps to the APMassEntryHistoryFees.APMHFeesStopSequence and assists with accessoiral fee allocation of billed fees.
    '''   Added the BookCarrOrderNumber property; a new field with a Default of null/nothing.
    '''     When this value is null/nothing (not an empty string) the caller must lookup the value using the BookControl property if it exists.  
    '''     If it does not exist the value should be set to an empty string.  
    '''     This value maps to APMassEntryHistoryFees.APMHFeesOrderNumber and assists with accessoiral fee allocation of billed fees.
    '''   Added new property BFPControl typically zero used to assist with auto approval a reference to the BookFeesPending PK
    '''   Added new properties: MissingFee and BilledFee used to assist with approval and historical recording
    '''     MissingFees can only be true when the carrier provides at least one fee
    '''     BookedFees are only true when actually provided by the carrier.
    '''     MissingFees must be manually approved by a user before the audit will pass
    '''     BookedFee must be true to save in the Billed Fees historical record
    '''     NOTE: In v-8.2.1.004 logic has been added to include all expected and/or pending costs as part of the 
    '''     freight bill processing logic and both billed fees and missing expected fees will be added to the APMassEntryFees record (Pending approval)
    '''     Missing Fees are written to the pending fees table.  When they are marked as approved they are no longer displayed as missing
    ''' Modified By LVV on 3/15/20
    '''   Added New Fields OrigName, DestName, CNS, SHID, FeeAllocationTypeControl, FeeAllocationTypeName, FeeAllocationTypeDesc
    '''     These are all part of the update to the Detailed Freight Bill Entry screen
    ''' Modified by RHR for v-8.5.4.006 on 05/29/2024 added APIFees property
    ''' </remarks>
    Public Class SettlementFee

        Private _Control As Integer
        Private _BookControl As Integer
        Private _Minimum As Decimal
        Private _Cost As Decimal
        Private _AccessorialCode As Integer
        Private _Caption As String
        Private _AutoApprove As Boolean
        Private _AllowCarrierUpdates As Boolean
        Private _FeeIndex As Integer
        Private _Pending As Boolean
        Private _Msg As String
        Private _StopSequence As Integer = -1          ' Missing (New field Default -1, When -1 the system must lookup Using BookControl) Maps To APMassEntryHistoryFees.APMHFeesStopSequence
        Private _BookCarrOrderNumber As String = Nothing     ' Missng (ne field Default null, When a bookcontrol exists And this Is null lookup the value In the book table) Maps To  APMassEntryHistoryFees.APMHFeesOrderNumber
        Private _BFPControl As Integer
        Private _EDICode As String
        Private _BookOrderSequence As Integer
        Private _MissingFee As Boolean = False
        Private _BilledFee As Boolean = False
        Private _OrigName As String 'Added By LVV on 3/15/20
        Private _DestName As String 'Added By LVV on 3/15/20
        Private _OrigZip As String 'Added By LVV on 4/1/20
        Private _DestZip As String 'Added By LVV on 4/1/20
        Private _CNS As String 'Added By LVV on 3/15/20
        Private _SHID As String 'Added By LVV on 3/15/20
        Private _FeeAllocationTypeControl As Integer 'Added By LVV on 3/15/20
        Private _FeeAllocationTypeName As String 'Added By LVV on 3/15/20
        Private _FeeAllocationTypeDesc As String 'Added By LVV on 3/15/20

        Public Property Control() As Integer
            Get
                Return _Control
            End Get
            Set(ByVal value As Integer)
                _Control = value
            End Set
        End Property

        Public Property BookControl() As Integer
            Get
                Return _BookControl
            End Get
            Set(ByVal value As Integer)
                _BookControl = value
            End Set
        End Property

        Public Property Minimum() As Decimal
            Get
                Return _Minimum
            End Get
            Set(ByVal value As Decimal)
                _Minimum = value
            End Set
        End Property

        Public Property Cost() As Decimal
            Get
                Return _Cost
            End Get
            Set(ByVal value As Decimal)
                _Cost = value
            End Set
        End Property

        Public Property AccessorialCode() As Integer
            Get
                Return _AccessorialCode
            End Get
            Set(ByVal value As Integer)
                _AccessorialCode = value
            End Set
        End Property

        Public Property Caption() As String
            Get
                Return _Caption
            End Get
            Set(ByVal value As String)
                _Caption = value
            End Set
        End Property

        Public Property AutoApprove() As Boolean
            Get
                Return _AutoApprove
            End Get
            Set(ByVal value As Boolean)
                _AutoApprove = value
            End Set
        End Property

        Public Property AllowCarrierUpdates() As Boolean
            Get
                Return _AllowCarrierUpdates
            End Get
            Set(ByVal value As Boolean)
                _AllowCarrierUpdates = value
            End Set
        End Property

        Public Property FeeIndex() As Integer
            Get
                Return _FeeIndex
            End Get
            Set(ByVal value As Integer)
                _FeeIndex = value
            End Set
        End Property

        Public Property Pending() As Boolean
            Get
                Return _Pending
            End Get
            Set(ByVal value As Boolean)
                _Pending = value
            End Set
        End Property

        Public Property Msg() As String
            Get
                Return _Msg
            End Get
            Set(ByVal value As String)
                _Msg = value
            End Set
        End Property

        Public Property StopSequence() As Integer
            Get
                Return _StopSequence
            End Get
            Set(ByVal value As Integer)
                _StopSequence = value
            End Set
        End Property

        Public Property BookCarrOrderNumber() As String
            Get
                If Not String.IsNullOrWhiteSpace(_BookCarrOrderNumber) Then _BookCarrOrderNumber = Left(_BookCarrOrderNumber, 20)
                Return _BookCarrOrderNumber
            End Get
            Set(ByVal value As String)
                _BookCarrOrderNumber = value
            End Set
        End Property

        Public Property BFPControl() As Integer
            Get
                Return _BFPControl
            End Get
            Set(ByVal value As Integer)
                _BFPControl = value
            End Set
        End Property

        Public Property EDICode() As String
            Get
                Return _EDICode
            End Get
            Set(ByVal value As String)
                _EDICode = value
            End Set
        End Property

        Public Property BookOrderSequence() As Integer
            Get
                Return _BookOrderSequence
            End Get
            Set(ByVal value As Integer)
                _BookOrderSequence = value
            End Set
        End Property

        Public Property MissingFee() As Boolean
            Get
                Return _MissingFee
            End Get
            Set(ByVal value As Boolean)
                _MissingFee = value
            End Set
        End Property

        Public Property BilledFee() As Boolean
            Get
                Return _BilledFee
            End Get
            Set(ByVal value As Boolean)
                _BilledFee = value
            End Set
        End Property

        'Added By LVV on 3/15/20
        Public Property OrigName() As String
            Get
                Return _OrigName
            End Get
            Set(ByVal value As String)
                _OrigName = value
            End Set
        End Property

        Public Property DestName() As String
            Get
                Return _DestName
            End Get
            Set(ByVal value As String)
                _DestName = value
            End Set
        End Property

        'Added By LVV on 4/1/20
        Public Property OrigZip() As String
            Get
                Return _OrigZip
            End Get
            Set(ByVal value As String)
                _OrigZip = value
            End Set
        End Property

        Public Property DestZip() As String
            Get
                Return _DestZip
            End Get
            Set(ByVal value As String)
                _DestZip = value
            End Set
        End Property

        'Added By LVV on 3/15/20
        Public Property CNS() As String
            Get
                Return _CNS
            End Get
            Set(ByVal value As String)
                _CNS = value
            End Set
        End Property

        Public Property SHID() As String
            Get
                Return _SHID
            End Get
            Set(ByVal value As String)
                _SHID = value
            End Set
        End Property

        Public Property FeeAllocationTypeControl() As Integer
            Get
                Return _FeeAllocationTypeControl
            End Get
            Set(ByVal value As Integer)
                _FeeAllocationTypeControl = value
            End Set
        End Property

        Public Property FeeAllocationTypeName() As String
            Get
                Return _FeeAllocationTypeName
            End Get
            Set(ByVal value As String)
                _FeeAllocationTypeName = value
            End Set
        End Property

        Public Property FeeAllocationTypeDesc() As String
            Get
                Return _FeeAllocationTypeDesc
            End Get
            Set(ByVal value As String)
                _FeeAllocationTypeDesc = value
            End Set
        End Property



    End Class

    Public Class SettlementFBDEData

        Private _LastStopCompControl As Integer
        Private _LastStopCompName As String
        Private _LastStopCarrierControl As Integer
        Private _LastStopCarrierName As String
        Private _LastStopCompLE As String
        Private _LastStopCompLEControl As Integer

        Private _ShowAuditFailReason As Boolean
        Private _ShowPendingFeeFailReason As Boolean
        Private _APMessage As String

        Private _LoadFees As SettlementFee()
        Private _OrderFees As SettlementFee()
        Private _OrigFees As SettlementFee()
        Private _DestFees As SettlementFee()

        Public Property LastStopCompControl() As Integer
            Get
                Return _LastStopCompControl
            End Get
            Set(ByVal value As Integer)
                _LastStopCompControl = value
            End Set
        End Property

        Public Property LastStopCompName() As String
            Get
                Return _LastStopCompName
            End Get
            Set(ByVal value As String)
                _LastStopCompName = value
            End Set
        End Property

        Public Property LastStopCarrierControl() As Integer
            Get
                Return _LastStopCarrierControl
            End Get
            Set(ByVal value As Integer)
                _LastStopCarrierControl = value
            End Set
        End Property

        Public Property LastStopCarrierName() As String
            Get
                Return _LastStopCarrierName
            End Get
            Set(ByVal value As String)
                _LastStopCarrierName = value
            End Set
        End Property

        Public Property LastStopCompLE() As String
            Get
                Return _LastStopCompLE
            End Get
            Set(ByVal value As String)
                _LastStopCompLE = value
            End Set
        End Property

        Public Property LastStopCompLEControl() As Integer
            Get
                Return _LastStopCompLEControl
            End Get
            Set(ByVal value As Integer)
                _LastStopCompLEControl = value
            End Set
        End Property

        Public Property ShowAuditFailReason() As Boolean
            Get
                Return _ShowAuditFailReason
            End Get
            Set(ByVal value As Boolean)
                _ShowAuditFailReason = value
            End Set
        End Property

        Public Property ShowPendingFeeFailReason() As Boolean
            Get
                Return _ShowPendingFeeFailReason
            End Get
            Set(ByVal value As Boolean)
                _ShowPendingFeeFailReason = value
            End Set
        End Property

        Public Property APMessage() As String
            Get
                Return _APMessage
            End Get
            Set(ByVal value As String)
                _APMessage = value
            End Set
        End Property

        Public Property LoadFees() As SettlementFee()
            Get
                Return _LoadFees
            End Get
            Set(ByVal value As SettlementFee())
                _LoadFees = value
            End Set
        End Property

        Public Property OrderFees() As SettlementFee()
            Get
                Return _OrderFees
            End Get
            Set(ByVal value As SettlementFee())
                _OrderFees = value
            End Set
        End Property

        Public Property OrigFees() As SettlementFee()
            Get
                Return _OrigFees
            End Get
            Set(ByVal value As SettlementFee())
                _OrigFees = value
            End Set
        End Property

        Public Property DestFees() As SettlementFee()
            Get
                Return _DestFees
            End Get
            Set(ByVal value As SettlementFee())
                _DestFees = value
            End Set
        End Property

    End Class

End Namespace


