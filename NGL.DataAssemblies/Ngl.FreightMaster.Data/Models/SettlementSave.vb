Imports Map = Ngl.API.Mapping
Imports DTran = Ngl.Core.Utility.DataTransformation
Imports DTO = Ngl.FreightMaster.Data.DataTransferObjects


Namespace Models
    'Added By LVV On 10/10/17 For v-8.0 TMS365
    ' Modified by RHR for v-8.2 on 6/10/2019
    '   added Total Fuel
    ' Modified by RHr for v-8.2.0.117 on 8/19/19
    '   added  APControl, APCustomerID, CarrierNumber, APBillDate, and APReceivedDate properties
    Public Class SettlementSave

        Private _ID As Integer
        Private _BookSHID As String
        Private _BookControl As Integer
        Private _CarrierControl As Integer
        Private _CompControl As Integer
        Private _InvoiceAmt As Decimal
        Private _LineHaul As Decimal
        Private _TotalFuel As Decimal
        Private _InvoiceNo As String
        Private _BookCarrBLNumber As String
        Private _BookFinAPActWgt As Integer
        Private _Fees As SettlementFee()
        Private _APControl As Integer?
        Private _APCustomerID As String
        Private _CarrierNumber As Integer
        Private _APBillDate As Date?
        Private _APReceivedDate As Date?

        'Modified by RHR for v-8.5.4.006 on 05/15/2024
        'Added new properties for API integration
        Private _APWeekEnding As String
        Private _APAccountNbr As String
        Private _APChargeCustomer As String
        Private _APConsignment As String
        Private _APCarrierNotes As String
        Private _APService As String
        Private _APUser1 As String
        Private _APUser2 As String
        Private _APUser3 As String
        Private _APUser4 As String
        Private _FuelFactor As String
        Private _APTotalItems As Integer
        Private _APTotalCubes As Double
        Private _APTotalWeight As Double
        Private _APTotalTaxes As Decimal
        Private _APTotalExtraCharges As Decimal
        Private _APTotalFees As Decimal
        Private _APCubicConversionFactor As Double
        Private _PickupAddress As Models.AddressBook
        Private _DeliveryAddress As Models.AddressBook
        Private _BookCarrierPro As String

        Public Property ID() As Integer
            Get
                Return _ID
            End Get
            Set(ByVal value As Integer)
                _ID = value
            End Set
        End Property

        Public Property BookSHID() As String
            Get
                Return _BookSHID
            End Get
            Set(ByVal value As String)
                _BookSHID = value
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

        Public Property CarrierControl() As Integer
            Get
                Return _CarrierControl
            End Get
            Set(ByVal value As Integer)
                _CarrierControl = value
            End Set
        End Property

        Public Property CompControl() As Integer
            Get
                Return _CompControl
            End Get
            Set(ByVal value As Integer)
                _CompControl = value
            End Set
        End Property

        Public Property InvoiceAmt() As Decimal
            Get
                Return _InvoiceAmt
            End Get
            Set(ByVal value As Decimal)
                _InvoiceAmt = value
            End Set
        End Property

        Public Property LineHaul() As Decimal
            Get
                Return _LineHaul
            End Get
            Set(ByVal value As Decimal)
                _LineHaul = value
            End Set
        End Property

        Public Property TotalFuel() As Decimal
            Get
                Return _TotalFuel
            End Get
            Set(ByVal value As Decimal)
                _TotalFuel = value
            End Set
        End Property

        Public Property InvoiceNo() As String
            Get
                Return _InvoiceNo
            End Get
            Set(ByVal value As String)
                _InvoiceNo = value
            End Set
        End Property

        Public Property BookCarrBLNumber() As String
            Get
                Return _BookCarrBLNumber
            End Get
            Set(ByVal value As String)
                _BookCarrBLNumber = value
            End Set
        End Property

        Public Property BookFinAPActWgt() As Integer
            Get
                Return _BookFinAPActWgt
            End Get
            Set(ByVal value As Integer)
                _BookFinAPActWgt = value
            End Set
        End Property

        Public Property Fees() As SettlementFee()
            Get
                Return _Fees
            End Get
            Set(ByVal value As SettlementFee())
                _Fees = value
            End Set
        End Property

        Public Property APControl() As Integer?
            Get
                Return _APControl
            End Get
            Set(ByVal value As Integer?)
                _APControl = value
            End Set
        End Property


        Public Property APCustomerID() As String
            Get
                Return _APCustomerID
            End Get
            Set(ByVal value As String)
                _APCustomerID = value
            End Set
        End Property

        Public Property CarrierNumber() As Integer
            Get
                Return _CarrierNumber
            End Get
            Set(ByVal value As Integer)
                _CarrierNumber = value
            End Set
        End Property

        Public Property APBillDate() As Date?
            Get
                Return _APBillDate
            End Get
            Set(ByVal value As Date?)
                _APBillDate = value
            End Set
        End Property

        Public Property APReceivedDate() As Date?
            Get
                Return _APReceivedDate
            End Get
            Set(ByVal value As Date?)
                _APReceivedDate = value
            End Set
        End Property

        'Modified by RHR for v-8.5.4.006 on 05/15/2024
        'Added new properties for API integration
        Public Property APWeekEnding As String
            Get
                Return _APWeekEnding
            End Get
            Set(ByVal value As String)
                _APWeekEnding = value
            End Set
        End Property

        Public Property APAccountNbr As String
            Get
                Return _APAccountNbr
            End Get
            Set(ByVal value As String)
                _APAccountNbr = value
            End Set
        End Property

        Public Property APChargeCustomer As String
            Get
                Return _APChargeCustomer
            End Get
            Set(ByVal value As String)
                _APChargeCustomer = value
            End Set
        End Property

        Public Property APConsignment As String
            Get
                Return _APConsignment
            End Get
            Set(ByVal value As String)
                _APConsignment = value
            End Set
        End Property

        Public Property APCarrierNotes As String
            Get
                Return _APCarrierNotes
            End Get
            Set(ByVal value As String)
                _APCarrierNotes = value
            End Set
        End Property

        Public Property APService As String
            Get
                Return _APService
            End Get
            Set(ByVal value As String)
                _APService = value
            End Set
        End Property

        Public Property APUser1 As String
            Get
                Return _APUser1
            End Get
            Set(ByVal value As String)
                _APUser1 = value
            End Set
        End Property

        Public Property APUser2 As String
            Get
                Return _APUser2
            End Get
            Set(ByVal value As String)
                _APUser2 = value
            End Set
        End Property
        Public Property APUser3 As String
            Get
                Return _APUser3
            End Get
            Set(ByVal value As String)
                _APUser3 = value
            End Set
        End Property

        Public Property APUser4 As String
            Get
                Return _APUser4
            End Get
            Set(ByVal value As String)
                _APUser4 = value
            End Set
        End Property
        Public Property FuelFactor As String
            Get
                Return _FuelFactor
            End Get
            Set(ByVal value As String)
                _FuelFactor = value
            End Set
        End Property
        Public Property APTotalItems As Integer
            Get
                Return _APTotalItems
            End Get
            Set(ByVal value As Integer)
                _APTotalItems = value
            End Set
        End Property

        Public Property APTotalCubes As Double
            Get
                Return _APTotalCubes
            End Get
            Set(ByVal value As Double)
                _APTotalCubes = value
            End Set
        End Property

        Public Property APTotalWeight As Double
            Get
                Return _APTotalWeight
            End Get
            Set(ByVal value As Double)
                _APTotalWeight = value
            End Set
        End Property

        Public Property APTotalTaxes As Decimal
            Get
                Return _APTotalTaxes
            End Get
            Set(ByVal value As Decimal)
                _APTotalTaxes = value
            End Set
        End Property

        Public Property APTotalExtraCharges As Decimal
            Get
                Return _APTotalExtraCharges
            End Get
            Set(ByVal value As Decimal)
                _APTotalExtraCharges = value
            End Set
        End Property

        Public Property APTotalFees As Decimal
            Get
                Return _APTotalFees
            End Get
            Set(ByVal value As Decimal)
                _APTotalFees = value
            End Set
        End Property

        Public Property APCubicConversionFactor As Double
            Get
                Return _APCubicConversionFactor
            End Get
            Set(ByVal value As Double)
                _APCubicConversionFactor = value
            End Set
        End Property

        Public Property PickupAddress As Models.AddressBook
            Get
                Return _PickupAddress
            End Get
            Set(ByVal value As Models.AddressBook)
                _PickupAddress = value
            End Set
        End Property

        Public Property DeliveryAddress As Models.AddressBook
            Get
                Return _DeliveryAddress
            End Get
            Set(ByVal value As Models.AddressBook)
                _DeliveryAddress = value
            End Set
        End Property

        Public Property BookCarrierPro As String
            Get
                Return _BookCarrierPro
            End Get
            Set(ByVal value As String)
                _BookCarrierPro = value
            End Set
        End Property

        'Modified by RHR for v-8.5.4.006 on 05/29/2024 added APIFees
        'add property APIFees    
        Private _APIFees As New List(Of Map.Fees)
        Public Property APIFees() As List(Of Map.Fees)
            Get
                Return _APIFees
            End Get
            Set(ByVal value As List(Of Map.Fees))
                _APIFees = value
            End Set
        End Property

        ''' <summary>
        ''' Map this Model data to API Settlement Save
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' Modified by RHR for v-8.5.4.006 on 05/29/2024 
        ''' </remarks>
        Public Function MapNGLAPISettlementSave() As Map.SettlementSave
            Return New Map.SettlementSave With {
                .ID = ID,
                .BookSHID = BookSHID,
                .BookControl = BookControl,
                .CarrierControl = CarrierControl,
                .CompControl = CompControl,
                .InvoiceAmt = InvoiceAmt,
                .LineHaul = LineHaul,
                .TotalFuel = TotalFuel,
                .InvoiceNo = InvoiceNo,
                .BookCarrBLNumber = BookCarrBLNumber,
                .BookFinAPActWgt = BookFinAPActWgt,
                .APControl = APControl,
                .APCustomerID = APCustomerID,
                .CarrierNumber = CarrierNumber,
                .APBillDate = APBillDate,
                .APReceivedDate = APReceivedDate,
                .APWeekEnding = APWeekEnding,
                .APAccountNbr = APAccountNbr,
                 .APChargeCustomer = APChargeCustomer,
                .APConsignment = APConsignment,
                .APCarrierNotes = APCarrierNotes,
                .APService = APService,
                .APUser1 = APUser1,
                .APUser2 = APUser2,
                .APUser3 = APUser3,
                .APUser4 = APUser4,
                .FuelFactor = FuelFactor,
                .APTotalItems = APTotalItems,
                .APTotalCubes = APTotalCubes,
                .APTotalWeight = APTotalWeight,
                .APTotalTaxes = APTotalTaxes,
                .APTotalExtraCharges = APTotalExtraCharges,
                .APTotalFees = APTotalFees,
                .APCubicConversionFactor = APCubicConversionFactor,
                .PickupAddress = PickupAddress.MapNGLAPIAddressBook(),
                .DeliveryAddress = DeliveryAddress.MapNGLAPIAddressBook(),
                .Fees = APIFees,
                .BookCarrierPro = BookCarrierPro
            }
        End Function

        'add a function MapAPINGLSettlementSave to save Map.SettlementSave to Model.SettlementSave  
        Public Shared Function MapAPINGLSettlementSave(ByVal settlementSave As Map.SettlementSave) As SettlementSave
            Return New SettlementSave With {
                .ID = settlementSave.ID,
                .BookSHID = settlementSave.BookSHID,
                .BookControl = settlementSave.BookControl,
                .CarrierControl = settlementSave.CarrierControl,
                .CompControl = settlementSave.CompControl,
                .InvoiceAmt = settlementSave.InvoiceAmt,
                .LineHaul = settlementSave.LineHaul,
                .TotalFuel = settlementSave.TotalFuel,
                .InvoiceNo = settlementSave.InvoiceNo,
                .BookCarrBLNumber = settlementSave.BookCarrBLNumber,
                .BookFinAPActWgt = settlementSave.BookFinAPActWgt,
                .APControl = settlementSave.APControl,
                .APCustomerID = settlementSave.APCustomerID,
                .CarrierNumber = settlementSave.CarrierNumber,
                .APBillDate = settlementSave.APBillDate,
                .APReceivedDate = settlementSave.APReceivedDate,
                .APWeekEnding = settlementSave.APWeekEnding,
                .APAccountNbr = settlementSave.APAccountNbr,
                .APChargeCustomer = settlementSave.APChargeCustomer,
                .APConsignment = settlementSave.APConsignment,
                .APCarrierNotes = settlementSave.APCarrierNotes,
                .APService = settlementSave.APService,
                .APUser1 = settlementSave.APUser1,
                .APUser2 = settlementSave.APUser2,
                .APUser3 = settlementSave.APUser3,
                .APUser4 = settlementSave.APUser4,
                .FuelFactor = settlementSave.FuelFactor,
                .APTotalItems = settlementSave.APTotalItems,
                .APTotalCubes = settlementSave.APTotalCubes,
                .APTotalWeight = settlementSave.APTotalWeight,
                .APTotalTaxes = settlementSave.APTotalTaxes,
                .APTotalExtraCharges = settlementSave.APTotalExtraCharges,
                .APTotalFees = settlementSave.APTotalFees,
                .APCubicConversionFactor = settlementSave.APCubicConversionFactor,
                .PickupAddress = Models.AddressBook.MapAPINGLAddressBook(settlementSave.PickupAddress),
                .DeliveryAddress = Models.AddressBook.MapAPINGLAddressBook(settlementSave.DeliveryAddress),
                .APIFees = settlementSave.Fees,
                .BookCarrierPro = settlementSave.BookCarrierPro
                }
        End Function


    End Class


End Namespace


