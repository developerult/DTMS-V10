Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports Ngl.Core.ChangeTracker
Imports Serilog

Namespace DataTransferObjects
    <DataContract()> _
    Public Class SpotRate
        Inherits DTOBaseClass

        Public Sub New ()
            Me.Logger = Me.Logger.ForContext(of SpotRate)
        End Sub
#Region " Data Members"

        Private _BookRevs As List(Of BookRevenue)
        <DataMember()> _
        Public Property BookRevs() As List(Of BookRevenue)
            Get
                Return _BookRevs
            End Get
            Set(ByVal value As List(Of BookRevenue))
                _BookRevs = value
            End Set
        End Property

        Private _allocationFormula As tblTarBracketType
        <DataMember()> _
         Public Property allocationFormula() As tblTarBracketType
            Get
                Return _allocationFormula
            End Get
            Set(ByVal value As tblTarBracketType)
                _allocationFormula = value
            End Set
        End Property

        Private _totalLineHaulCost As Decimal = 0
        <DataMember()> _
        Public Property totalLineHaulCost() As Decimal
            Get
                Return _totalLineHaulCost
            End Get
            Set(ByVal value As Decimal)
                _totalLineHaulCost = value
            End Set
        End Property

        Private _BookFees As List(Of BookFee)
        <DataMember()> _
        Public Property BookFees() As List(Of BookFee)
            Get
                Return _BookFees
            End Get
            Set(ByVal value As List(Of BookFee))
                _BookFees = value
            End Set
        End Property

        Private _DeleteTariffFees As Boolean = False
        <DataMember()> _
         Public Property DeleteTariffFees() As Boolean
            Get
                Return _DeleteTariffFees
            End Get
            Set(ByVal value As Boolean)
                _DeleteTariffFees = value
            End Set
        End Property

        Private _DeleteLaneFees As Boolean = False
        <DataMember()> _
        Public Property DeleteLaneFees() As Boolean
            Get
                Return _DeleteLaneFees
            End Get
            Set(ByVal value As Boolean)
                _DeleteLaneFees = value
            End Set
        End Property

        Private _DeleteOrderFees As Boolean = False
        <DataMember()> _
        Public Property DeleteOrderFees() As Boolean
            Get
                Return _DeleteOrderFees
            End Get
            Set(ByVal value As Boolean)
                _DeleteOrderFees = value
            End Set
        End Property

        Private _useCarrierFuel As Boolean = False
        <DataMember()> _
        Public Property UseCarrierFuelAddendum() As Boolean
            Get
                Return _useCarrierFuel
            End Get
            Set(ByVal value As Boolean)
                _useCarrierFuel = value
            End Set
        End Property

        Private _carrierControl As Integer
        <DataMember()> _
        Public Property CarrierControl() As Integer
            Get
                Return _carrierControl
            End Get
            Set(ByVal value As Integer)
                _carrierControl = value
            End Set
        End Property

        Private _State As String
        <DataMember()> _
         Public Property State() As String
            Get
                Return _State
            End Get
            Set(ByVal value As String)
                _State = value
            End Set
        End Property

        Private _EffectiveDate As Date
        <DataMember()> _
        Public Property EffectiveDate() As Date
            Get
                Return _EffectiveDate
            End Get
            Set(ByVal value As Date)
                _EffectiveDate = value
            End Set
        End Property

        Private _AvgFuelPrice As Decimal
        <DataMember()> _
        Public Property AvgFuelPrice() As Decimal
            Get
                Return _AvgFuelPrice
            End Get
            Set(ByVal value As Decimal)
                _AvgFuelPrice = value
            End Set
        End Property

        Private _AutoCalculateBFC As Boolean = True
        <DataMember()> _
         Public Property AutoCalculateBFC() As Boolean
            Get
                Return _AutoCalculateBFC
            End Get
            Set(ByVal value As Boolean)
                _AutoCalculateBFC = value
            End Set
        End Property

        Private _totalBFC As Decimal = 0
        <DataMember()> _
        Public Property TotalBFC() As Decimal
            Get
                Return _totalBFC
            End Get
            Set(ByVal value As Decimal)
                _totalBFC = value
            End Set
        End Property

        Private _allocationBFCFormula As tblTarBracketType
        <DataMember()> _
        Public Property AllocationBFCFormula() As tblTarBracketType
            Get
                Return _allocationBFCFormula
            End Get
            Set(ByVal value As tblTarBracketType)
                _allocationBFCFormula = value
            End Set
        End Property

        Private _BookRevNegRevenueValue As Integer
        <DataMember()>
        Public Property BookRevNegRevenueValue() As Integer
            Get
                Return _BookRevNegRevenueValue
            End Get
            Set(ByVal value As Integer)
                _BookRevNegRevenueValue = value
            End Set
        End Property

        Private _blnLockBFCCost As Boolean = True
        ''' <summary>
        ''' Flag to force a lock on the BFC when using spot rate logic default is true 
        ''' </summary>
        ''' <returns></returns>
        <DataMember()>
        Public Property LockBFCCost() As Boolean
            Get
                Return _blnLockBFCCost
            End Get
            Set(ByVal value As Boolean)
                _blnLockBFCCost = value
            End Set
        End Property

        Private _blnLockAllCost As Boolean = True
        ''' <summary>
        ''' Flag to force a lock on the Carrier Costs when using spot rate logic default is true for API this should be false
        ''' </summary>
        ''' <returns></returns>
        <DataMember()>
        Public Property LockAllCost() As Boolean
            Get
                Return _blnLockAllCost
            End Get
            Set(ByVal value As Boolean)
                _blnLockAllCost = value
            End Set
        End Property

        Private _blnFromAPI As Boolean = False
        ''' <summary>
        ''' Flag to manage which fields are automatically reset by the spot rate engine.  Specific API fields should not be modified
        ''' </summary>
        ''' <returns></returns>
        <DataMember()>
        Public Property FromAPI() As Boolean
            Get
                Return _blnFromAPI
            End Get
            Set(ByVal value As Boolean)
                _blnFromAPI = value
            End Set
        End Property

#End Region

#Region " Public Methods"
        Public Overrides Function Clone() As DTOBaseClass
            Dim instance As New SpotRate
            instance = DirectCast(MemberwiseClone(), SpotRate)
            Return instance
        End Function

#End Region

    End Class
End Namespace